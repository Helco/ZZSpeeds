#include "shared.hpp"

using FnWndProc = int(__stdcall*)(HWND, UINT, WPARAM, LPARAM);
using FnGetCursorPos = BOOL(__stdcall*)(LPPOINT);
using FnUICursor_update = int(__fastcall*)(void*, void*, void*);
using FnCallSetCursorPos = BOOL(__stdcall*)(int, int);
using FnFindGameCD = bool(__cdecl*)(const char*, const char*);
using FnCheckSerialNumber = bool(__cdecl*)(const char*);

static FnWndProc originalWndProc = nullptr;
static FnGetCursorPos originalGetCursorPos = GetCursorPos;
static FnUICursor_update originalUICursor_update = nullptr;
static FnCallSetCursorPos originalCallSetCursorPos = nullptr;
static FnFindGameCD originalFindGameCD = nullptr;
static FnCheckSerialNumber originalCheckSerialNumber = nullptr;

static bool isWindowActivated = false;
static bool shouldTransformGetCursorPos = false;
static HWND hWnd = nullptr;

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
		SetWindowPos(hWnd, HWND_TOP,
			(monitorInfo.rcMonitor.left + monitorInfo.rcMonitor.right) / 2 - (windowSize.right - windowSize.left) / 2,
			(monitorInfo.rcMonitor.top + monitorInfo.rcMonitor.bottom) / 2 - (windowSize.bottom - windowSize.top) / 2,
			0, 0,
			SWP_NOSIZE);
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

		auto versionOpt = GetGameVersion();
		if (!versionOpt.has_value())
			ErrorExit("Unknown game version (how did you get here?!)");
		auto version = versionOpt.value();
		originalWndProc = reinterpret_cast<FnWndProc>(version.info.addrWndProc);
		originalUICursor_update = reinterpret_cast<FnUICursor_update>(version.info.addrUICursor_update);
		originalCallSetCursorPos = reinterpret_cast<FnCallSetCursorPos>(version.info.addrCallSetCursorPos);
		if (version.info.addrFindGameCD != NO_HOOK_NECESSARY)
			originalFindGameCD = reinterpret_cast<FnFindGameCD>(version.info.addrFindGameCD);
		if (version.info.addrCheckSerialNumber != NO_HOOK_NECESSARY)
			originalCheckSerialNumber = reinterpret_cast<FnCheckSerialNumber>(version.info.addrCheckSerialNumber);

		SafeDetourCall(DetourTransactionBegin(), "beginning attach transaction");
		SafeDetourCall(DetourUpdateThread(GetCurrentThread()), "updating thread");
		SafeDetourCall(DetourAttach(&(PVOID&)originalWndProc, MyWndProc), "attaching to wndproc");
		SafeDetourCall(DetourAttach(&(PVOID&)originalUICursor_update, MyUICursor_update), "attaching to UICursor_update");
		SafeDetourCall(DetourAttach(&(PVOID&)originalGetCursorPos, MyGetCursorPos), "attaching to GetCursorPos");
		SafeDetourCall(DetourAttach(&(PVOID&)originalCallSetCursorPos, MyCallSetCursorPos), "attaching to CallSetCursorPos");
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
		if (originalFindGameCD != nullptr)
			SafeDetourCall(DetourDetach(&(PVOID&)originalFindGameCD, MyFindGameCD), "detaching from FindGameCD");
		if (originalCheckSerialNumber != nullptr)
			SafeDetourCall(DetourAttach(&(PVOID&)originalCheckSerialNumber, MyCheckSerialNumber), "attaching to CheckSerialNumber");
		SafeDetourCall(DetourTransactionCommit(), "committing detach transaction");
	}break;
	}

	return TRUE;
}
