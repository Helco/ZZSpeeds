#include "shared.hpp"
#include "resource.h"
#include <timeapi.h>
#include <chrono>

static int maxFramerate = 120;
using FramerateClock = std::chrono::steady_clock; // why not, just hope it doesn't run backwards
using FramerateTimepoint = std::chrono::time_point<FramerateClock>;

using FnWndProc = int(__stdcall*)(HWND, UINT, WPARAM, LPARAM);
using FnGetCursorPos = BOOL(__stdcall*)(LPPOINT);
using FnUICursor_update = int(__fastcall*)(void*, void*, void*);
using FnUICursor_setVisible = void(__fastcall*)(void*, void*, bool);
using FnCallSetCursorPos = BOOL(__stdcall*)(int, int);
using FnFindGameCD = bool(__cdecl*)(const char*, const char*);
using FnCheckSerialNumber = bool(__cdecl*)(const char*);
using FnGame_tick = double(__fastcall*)(DWORD, void*);
using FnCreateDialogParamA = HWND(__stdcall*)(HINSTANCE, LPCSTR, HWND, DLGPROC, LPARAM);
using FnCreateSingleInstanceMutex = bool(__fastcall*)(DWORD, HANDLE*);
using FnInputMgr_update = void(__fastcall*)(void*, void*);

static FnWndProc originalWndProc = nullptr;
static FnGetCursorPos originalGetCursorPos = GetCursorPos;
static FnUICursor_update originalUICursor_update = nullptr;
static FnUICursor_setVisible originalUICursor_setVisible = nullptr;
static FnCallSetCursorPos originalCallSetCursorPos = nullptr;
static FnFindGameCD originalFindGameCD = nullptr;
static FnCheckSerialNumber originalCheckSerialNumber = nullptr;
static FnGame_tick originalGame_tick = nullptr;
static FnCreateDialogParamA originalCreateDialogParamA = CreateDialogParamA;
static FnCreateSingleInstanceMutex originalCreateSingleInstanceMutex = nullptr;
static FnInputMgr_update originalInputMgr_update = nullptr;

static const int* resolutionModeIndex = nullptr;
static const ResolutionMode* resolutionModes = nullptr;

static GameVersion gameVersion;
static WZConfig curWZConfig = LoadWZConfig();
static bool isWindowActivated = false;
static bool wasWindowedMode = false;
static bool shouldSkipLauncher = false;
static bool shouldTransformGetCursorPos = false;
static HWND hWnd = nullptr;
static HMODULE ownHModule = nullptr;
static DLGPROC originalVideoSettingsDialogProc = nullptr;
static FramerateClock framerateClock;
static FramerateTimepoint frameStart = framerateClock.now();
static int overrideWindowX = -1;

RECT GetWindowContentRect(HWND hWnd)
{
	RECT rect;
	GetClientRect(hWnd, &rect);
	MapWindowPoints(hWnd, HWND_DESKTOP, reinterpret_cast<POINT*>(&rect), 2);
	return rect;
}

int __stdcall MyWndProc (HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	::hWnd = hWnd; // we need the correct HWND, this might not be the one returned by GetTopWindow(0)
	bool shouldRepositionWindow = false;

	if (msg == WM_ACTIVATE) {
		isWindowActivated = wParam > 0;
		shouldRepositionWindow = isWindowActivated;
		if (wParam == 0 && curWZConfig.ignoreFocusLoss)
			msg = WM_NULL;
	}

	if (msg == WM_WINDOWPOSCHANGED)
		shouldRepositionWindow = true;

	if (shouldRepositionWindow) {
		// the centering should not be too annoying, do it only if things change
		const ResolutionMode& mode = resolutionModes[std::clamp(*resolutionModeIndex, 0, 5)];
		MONITORINFO monitorInfo;
		RECT windowSize;
		auto monitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST);
		ZeroMemory(&monitorInfo, sizeof(MONITORINFO));
		monitorInfo.cbSize = sizeof(MONITORINFO);
		GetMonitorInfo(monitor, &monitorInfo);
		GetWindowRect(hWnd, &windowSize);
		windowSize.right = windowSize.left + mode.width;
		windowSize.bottom = windowSize.top + mode.height;
		AdjustWindowRect(&windowSize, 0, true);
		SetWindowPos(hWnd, HWND_TOP,
			overrideWindowX >= 0
				? overrideWindowX
				:(monitorInfo.rcMonitor.left + monitorInfo.rcMonitor.right) / 2 - (windowSize.right - windowSize.left) / 2,
			(monitorInfo.rcMonitor.top + monitorInfo.rcMonitor.bottom) / 2 - (windowSize.bottom - windowSize.top) / 2,
			windowSize.right - windowSize.left,
			windowSize.bottom - windowSize.top,
			0);
	}

	if (isWindowActivated) {
		// ClipCursor is reverted each time a window is created
		// which in modern windows systems is about anytime anything happens
		// so we just call ClipCursor a lot, which is not pretty but works
		RECT rect = GetWindowContentRect(hWnd);
		ClipCursor(&rect);
	}

	return originalWndProc(hWnd, msg, wParam, lParam);
}

// mind the second argument, its place is caller-owned in thiscall but not in fastcall, keep its value!
int __fastcall MyUICursor_update(void* thiz, void* dummy, void* time)
{
	shouldTransformGetCursorPos = true;
	auto result = originalUICursor_update(thiz, dummy, time);
	shouldTransformGetCursorPos = false;
	return result;
}

void __fastcall MyUICursor_setVisible(void* thiz, void* dummy, bool visible)
{
	shouldTransformGetCursorPos = true;
	originalUICursor_setVisible(thiz, dummy, visible);
	shouldTransformGetCursorPos = false;
}

BOOL __stdcall MyGetCursorPos(LPPOINT point)
{
	BOOL result = originalGetCursorPos(point);
	if (!result)
		return FALSE;

	if (shouldTransformGetCursorPos) {
		// Zanzarah uses GetClientRect to transform from screen to client rect
		// this is of course wrong, GetClientRect is already client coordinates
		// but we have to be careful to not interfer with DirectInput which also uses GetCursorPos :(
		RECT rect = GetWindowContentRect(hWnd);
		point->x -= rect.left;
		point->y -= rect.top;
	}
	return TRUE;
}

BOOL __stdcall MyCallSetCursorPos(int x, int y)
{
	// no guard necessary, this is a zanzarah function only called by itself
	RECT rect = GetWindowContentRect(hWnd);
	return originalCallSetCursorPos(
		x + rect.left,
		y + rect.top
	);
}

bool __cdecl MyFindGameCD(const char* volumeName, const char* checkFilename)
{
	return true;
}

bool __cdecl MyCheckSerialNumber(const char* serialNumber)
{
	return true;
}

double __fastcall MyGame_tick(DWORD thizAddr, void* dummy)
{
	const float maxDelay = 1000.0f / maxFramerate;
	float diff = static_cast<float>(
		std::chrono::duration_cast<std::chrono::milliseconds>(framerateClock.now() - frameStart).count()
	);

	int newDelay = lroundf(std::clamp(maxDelay - diff, 0.0f, maxDelay));
	if (newDelay > 0)
		Sleep(newDelay);
	frameStart = framerateClock.now();

	return originalGame_tick(thizAddr, dummy);
}

int __stdcall MyVideoSettingsDialogProc(HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	if (msg == WM_INITDIALOG)
	{
		CheckDlgButton(hWnd, IDC_CHECK_WINDOWED, curWZConfig.windowedMode);
		CheckDlgButton(hWnd, IDC_CHECK_IGNOREFOCUSLOSS, curWZConfig.ignoreFocusLoss);
	}
	else if (msg == WM_DESTROY) {
		auto newWZConfig = curWZConfig;
		auto hChkWindowed = GetDlgItem(hWnd, IDC_CHECK_WINDOWED);
		newWZConfig.windowedMode = SendMessage(hChkWindowed, BM_GETCHECK, 0, 0) == 1;
		auto hChkIgnoreFocusLoss = GetDlgItem(hWnd, IDC_CHECK_IGNOREFOCUSLOSS);
		newWZConfig.ignoreFocusLoss = SendMessage(hChkIgnoreFocusLoss, BM_GETCHECK, 0, 0) == 1;
		
		bool shouldRestart = newWZConfig.windowedMode != curWZConfig.windowedMode;
		SaveWZConfig(newWZConfig);
		curWZConfig = newWZConfig;

		if (shouldRestart)
			ExitProcess(ExitCodeNoLauncher);
	}

	return originalVideoSettingsDialogProc(hWnd, msg, wParam, lParam);
}

HWND __stdcall MyCreateDialogParamA(HINSTANCE hInstance, LPCSTR lpTemplateName, HWND hWndParent, DLGPROC lpDialogFunc, LPARAM dwInitParam)
{
	auto dialogId = reinterpret_cast<DWORD>(lpTemplateName);
	if (dialogId != gameVersion.info.resID_videoSettingsTab)
		return originalCreateDialogParamA(hInstance, lpTemplateName, hWndParent, lpDialogFunc, dwInitParam);

	if (shouldSkipLauncher)
		EndDialog(GetParent(hWndParent), 1); // hWndParent is tab container, grand parent is the dialog

	originalVideoSettingsDialogProc = lpDialogFunc;
	return originalCreateDialogParamA(
		reinterpret_cast<HINSTANCE>(ownHModule), // In 32Bit-Window HMODULE and HINSTANCE are exactly the same -_-
		reinterpret_cast<LPCSTR>(IDD_VIDEOSETTINGSTAB),
		hWndParent,
		MyVideoSettingsDialogProc,
		dwInitParam
	);
}

bool __fastcall MyCreateSingleInstanceMutex(DWORD _, HANDLE*)
{
	return true;
}

void __fastcall MyInputMgr_update(void* thiz, void* dummy)
{
	if (isWindowActivated)
		originalInputMgr_update(thiz, dummy);
}

std::optional<int> findParameterArg(const std::string& haystack, const char* needleStart, const char* needleEnd)
{
	size_t startI = haystack.find(needleStart, 0);
	if (startI == std::string::npos)
		return std::nullopt;
	startI += strlen(needleStart);
	size_t endI = haystack.find(needleEnd, startI);
	if (endI == std::string::npos)
		return std::nullopt;

	std::string arg = haystack.substr(startI, endI - startI);
	return atoi(arg.c_str());
}

void parseCommandLine()
{
	std::string commandLine(GetCommandLineA());

	auto argMaxFramerate = findParameterArg(commandLine, "-fps(", ")");
	maxFramerate = argMaxFramerate.value_or(maxFramerate);

	auto argWindowX = findParameterArg(commandLine, "-windowX(", ")");
	overrideWindowX = argWindowX.value_or(overrideWindowX);

	shouldSkipLauncher = commandLine.find(NoLauncherArgument) != std::string::npos;
}

BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH: {
		OutputDebugString("WindowedZanzarah hook is attaching");
		DetourRestoreAfterWith();

		parseCommandLine();
		timeBeginPeriod(1);
		ownHModule = hModule;

		auto versionOpt = GetGameVersion();
		if (!versionOpt.has_value())
			ErrorExit("Unknown game version (how did you get here?!)");
		gameVersion = versionOpt.value();
		originalWndProc = reinterpret_cast<FnWndProc>(gameVersion.info.addrWndProc);
		originalUICursor_update = reinterpret_cast<FnUICursor_update>(gameVersion.info.addrUICursor_update);
		originalUICursor_setVisible = reinterpret_cast<FnUICursor_setVisible>(gameVersion.info.addrUICursor_setVisible);
		originalCallSetCursorPos = reinterpret_cast<FnCallSetCursorPos>(gameVersion.info.addrCallSetCursorPos);
		originalGame_tick = reinterpret_cast<FnGame_tick>(gameVersion.info.addrGame_tick);
		originalCreateSingleInstanceMutex = reinterpret_cast<FnCreateSingleInstanceMutex>(gameVersion.info.addrCreateSingleInstanceMutex);
		originalInputMgr_update = reinterpret_cast<FnInputMgr_update>(gameVersion.info.addrInputMgr_update);
		resolutionModeIndex = reinterpret_cast<const int*>(gameVersion.info.addrResolutionModeIndex);
		resolutionModes = reinterpret_cast<const ResolutionMode*>(gameVersion.info.addrResolutionModes);
		if (gameVersion.info.addrFindGameCD != NO_HOOK_NECESSARY)
			originalFindGameCD = reinterpret_cast<FnFindGameCD>(gameVersion.info.addrFindGameCD);
		if (gameVersion.info.addrCheckSerialNumber != NO_HOOK_NECESSARY)
			originalCheckSerialNumber = reinterpret_cast<FnCheckSerialNumber>(gameVersion.info.addrCheckSerialNumber);

		SafeDetourCall(DetourTransactionBegin(), "beginning attach transaction");
		SafeDetourCall(DetourUpdateThread(GetCurrentThread()), "updating thread");
		if (curWZConfig.windowedMode) {
			wasWindowedMode = true;
			SafeDetourCall(DetourAttach(&(PVOID&)originalWndProc, MyWndProc), "attaching to wndproc");
			SafeDetourCall(DetourAttach(&(PVOID&)originalUICursor_update, MyUICursor_update), "attaching to UICursor_update");
			SafeDetourCall(DetourAttach(&(PVOID&)originalUICursor_setVisible, MyUICursor_setVisible), "attaching to UICursor_setVisible");
			SafeDetourCall(DetourAttach(&(PVOID&)originalGetCursorPos, MyGetCursorPos), "attaching to GetCursorPos");
			SafeDetourCall(DetourAttach(&(PVOID&)originalCallSetCursorPos, MyCallSetCursorPos), "attaching to CallSetCursorPos");
		}
		SafeDetourCall(DetourAttach(&(PVOID&)originalGame_tick, MyGame_tick), "attaching to Game_tick");
		SafeDetourCall(DetourAttach(&(PVOID&)originalCreateDialogParamA, MyCreateDialogParamA), "attaching to CreateDialogParamA");
		SafeDetourCall(DetourAttach(&(PVOID&)originalCreateSingleInstanceMutex, MyCreateSingleInstanceMutex), "attaching to createSingleInstanceMutex");
		SafeDetourCall(DetourAttach(&(PVOID&)originalInputMgr_update, MyInputMgr_update), "attaching to InputMgr_update");
		if (originalFindGameCD != nullptr)
			SafeDetourCall(DetourAttach(&(PVOID&)originalFindGameCD, MyFindGameCD), "attaching to FindGameCD");
		if (originalCheckSerialNumber != nullptr)
			SafeDetourCall(DetourAttach(&(PVOID&)originalCheckSerialNumber, MyCheckSerialNumber), "attaching to CheckSerialNumber");
		SafeDetourCall(DetourTransactionCommit(), "committing attach transaction");
	}break;

	case DLL_PROCESS_DETACH: {
		OutputDebugString("WindowedZanzarah hook is detaching");

		SafeDetourCall(DetourTransactionBegin(), "beginning detach transaction");
		SafeDetourCall(DetourUpdateThread(GetCurrentThread()), "updating thread");
		if (wasWindowedMode) {
			SafeDetourCall(DetourDetach(&(PVOID&)originalWndProc, MyWndProc), "detaching from wndproc");
			SafeDetourCall(DetourDetach(&(PVOID&)originalUICursor_update, MyUICursor_update), "detaching from UICursor_update");
			SafeDetourCall(DetourAttach(&(PVOID&)originalUICursor_setVisible, MyUICursor_setVisible), "attaching to UICursor_setVisible");
			SafeDetourCall(DetourDetach(&(PVOID&)originalGetCursorPos, MyGetCursorPos), "detaching from GetCursorPos");
			SafeDetourCall(DetourDetach(&(PVOID&)originalCallSetCursorPos, MyCallSetCursorPos), "detaching from CallSetCursorPos");
		}
		SafeDetourCall(DetourDetach(&(PVOID&)originalGame_tick, MyGame_tick), "detaching from Game_tick");
		SafeDetourCall(DetourDetach(&(PVOID&)originalCreateDialogParamA, MyCreateDialogParamA), "detaching from CreateDialogParamA");
		SafeDetourCall(DetourDetach(&(PVOID&)originalCreateSingleInstanceMutex, MyCreateSingleInstanceMutex), "detaching from createSingleInstanceMutex");
		SafeDetourCall(DetourDetach(&(PVOID&)originalInputMgr_update, MyInputMgr_update), "detaching from InputMgr_update");
		if (originalFindGameCD != nullptr)
			SafeDetourCall(DetourDetach(&(PVOID&)originalFindGameCD, MyFindGameCD), "detaching from FindGameCD");
		if (originalCheckSerialNumber != nullptr)
			SafeDetourCall(DetourAttach(&(PVOID&)originalCheckSerialNumber, MyCheckSerialNumber), "attaching to CheckSerialNumber");
		SafeDetourCall(DetourTransactionCommit(), "committing detach transaction");
	}break;
	}

	return TRUE;
}
