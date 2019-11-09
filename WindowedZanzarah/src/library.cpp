#include "shared.hpp"
#include <timeapi.h>
#include <chrono>
#include <algorithm>

static int maxFramerate = 120;
using FramerateClock = std::chrono::steady_clock; // why not, just hope it doesn't run backwards
using FramerateTimepoint = std::chrono::time_point<FramerateClock>;

using FnWndProc = int(__stdcall*)(HWND, UINT, WPARAM, LPARAM);
using FnGetCursorPos = BOOL(__stdcall*)(LPPOINT);
using FnUICursor_update = int(__fastcall*)(void*, void*, void*);
using FnCallSetCursorPos = BOOL(__stdcall*)(int, int);
using FnFindGameCD = bool(__cdecl*)(const char*, const char*);
using FnCheckSerialNumber = bool(__cdecl*)(const char*);
using FnGame_tick = double(__fastcall*)(DWORD, void*);

static FnWndProc originalWndProc = nullptr;
static FnGetCursorPos originalGetCursorPos = GetCursorPos;
static FnUICursor_update originalUICursor_update = nullptr;
static FnCallSetCursorPos originalCallSetCursorPos = nullptr;
static FnFindGameCD originalFindGameCD = nullptr;
static FnCheckSerialNumber originalCheckSerialNumber = nullptr;
static FnGame_tick originalGame_tick = nullptr;

static GameVersion gameVersion;
static bool isWindowActivated = false;
static bool shouldTransformGetCursorPos = false;
static HWND hWnd = nullptr;
static FramerateClock framerateClock;
static FramerateTimepoint frameStart = framerateClock.now();

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
	}

	if (msg == WM_WINDOWPOSCHANGED)
		shouldRepositionWindow = true;

	if (shouldRepositionWindow) {
		// the centering should not be too annoying, do it only if things change
		MONITORINFO monitorInfo;
		RECT windowSize;
		auto monitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST);
		ZeroMemory(&monitorInfo, sizeof(MONITORINFO));
		monitorInfo.cbSize = sizeof(MONITORINFO);
		GetMonitorInfo(monitor, &monitorInfo);
		GetWindowRect(hWnd, &windowSize);
		windowSize.right = windowSize.left + 1024;
		windowSize.bottom = windowSize.top + 768;
		AdjustWindowRect(&windowSize, 0, true);
		SetWindowPos(hWnd, HWND_TOP,
			(monitorInfo.rcMonitor.left + monitorInfo.rcMonitor.right) / 2 - (windowSize.right - windowSize.left) / 2,
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

		auto versionOpt = GetGameVersion();
		if (!versionOpt.has_value())
			ErrorExit("Unknown game version (how did you get here?!)");
		gameVersion = versionOpt.value();
		originalWndProc = reinterpret_cast<FnWndProc>(gameVersion.info.addrWndProc);
		originalUICursor_update = reinterpret_cast<FnUICursor_update>(gameVersion.info.addrUICursor_update);
		originalCallSetCursorPos = reinterpret_cast<FnCallSetCursorPos>(gameVersion.info.addrCallSetCursorPos);
		originalGame_tick = reinterpret_cast<FnGame_tick>(gameVersion.info.addrGame_tick);
		if (gameVersion.info.addrFindGameCD != NO_HOOK_NECESSARY)
			originalFindGameCD = reinterpret_cast<FnFindGameCD>(gameVersion.info.addrFindGameCD);
		if (gameVersion.info.addrCheckSerialNumber != NO_HOOK_NECESSARY)
			originalCheckSerialNumber = reinterpret_cast<FnCheckSerialNumber>(gameVersion.info.addrCheckSerialNumber);

		SafeDetourCall(DetourTransactionBegin(), "beginning attach transaction");
		SafeDetourCall(DetourUpdateThread(GetCurrentThread()), "updating thread");
		SafeDetourCall(DetourAttach(&(PVOID&)originalWndProc, MyWndProc), "attaching to wndproc");
		SafeDetourCall(DetourAttach(&(PVOID&)originalUICursor_update, MyUICursor_update), "attaching to UICursor_update");
		SafeDetourCall(DetourAttach(&(PVOID&)originalGetCursorPos, MyGetCursorPos), "attaching to GetCursorPos");
		SafeDetourCall(DetourAttach(&(PVOID&)originalCallSetCursorPos, MyCallSetCursorPos), "attaching to CallSetCursorPos");
		SafeDetourCall(DetourAttach(&(PVOID&)originalGame_tick, MyGame_tick), "attaching to Game_tick");
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
		SafeDetourCall(DetourDetach(&(PVOID&)originalWndProc, MyWndProc), "detaching from wndproc");
		SafeDetourCall(DetourDetach(&(PVOID&)originalUICursor_update, MyUICursor_update), "detaching from UICursor_update");
		SafeDetourCall(DetourDetach(&(PVOID&)originalGetCursorPos, MyGetCursorPos), "detaching from GetCursorPos");
		SafeDetourCall(DetourDetach(&(PVOID&)originalCallSetCursorPos, MyCallSetCursorPos), "detaching from CallSetCursorPos");
		SafeDetourCall(DetourDetach(&(PVOID&)originalGame_tick, MyGame_tick), "detaching from Game_tick");
		if (originalFindGameCD != nullptr)
			SafeDetourCall(DetourDetach(&(PVOID&)originalFindGameCD, MyFindGameCD), "detaching from FindGameCD");
		if (originalCheckSerialNumber != nullptr)
			SafeDetourCall(DetourAttach(&(PVOID&)originalCheckSerialNumber, MyCheckSerialNumber), "attaching to CheckSerialNumber");
		SafeDetourCall(DetourTransactionCommit(), "committing detach transaction");
	}break;
	}

	return TRUE;
}
