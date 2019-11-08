#include "shared.hpp"

using FnWndProc = int(__stdcall*)(HWND, UINT, WPARAM, LPARAM);
static FnWndProc originalWndProc = nullptr;
using FnFindGameCD = bool(__cdecl*)(const char*, const char*);
static FnFindGameCD originalFindGameCD = nullptr;
static bool isWindowActivated = false;

RECT GetWindowContentRect(HWND hWnd)
{
	RECT rect;
	GetClientRect(hWnd, &rect);
	MapWindowPoints(hWnd, HWND_DESKTOP, reinterpret_cast<POINT*>(&rect), 2);
	return rect;
}

int __stdcall MyWndProc (HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam)
{
	// ClipCursor is reverted each time a window is created
	// which in modern windows systems is about anytime anything happens
	// so we just call ClipCursor a lot, which is not pretty but works

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
		RECT rect = GetWindowContentRect(hWnd);
		ClipCursor(&rect);
	}

	return originalWndProc(hWnd, msg, wParam, lParam);
}

bool __cdecl MyFindGameCD(const char* volumeName, const char* checkFilename)
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
		if (version.info.addrFindGameCD != NO_CRACK_NECESSARY)
			originalFindGameCD = reinterpret_cast<FnFindGameCD>(version.info.addrFindGameCD);

		SafeDetourCall(DetourTransactionBegin(), "beginning attach transaction");
		SafeDetourCall(DetourUpdateThread(GetCurrentThread()), "updating thread");
		SafeDetourCall(DetourAttach(&(PVOID&)originalWndProc, MyWndProc), "attaching to wndproc");
		if (originalFindGameCD != nullptr)
			SafeDetourCall(DetourAttach(&(PVOID&)originalFindGameCD, MyFindGameCD), "attaching to FindGameCD");
		SafeDetourCall(DetourTransactionCommit(), "committing attach transaction");
	}break;

	case DLL_PROCESS_DETACH: {
		OutputDebugString("WindowedZanzarah hook is detaching");

		SafeDetourCall(DetourTransactionBegin(), "beginning detach transaction");
		SafeDetourCall(DetourUpdateThread(GetCurrentThread()), "updating thread");
		SafeDetourCall(DetourDetach(&(PVOID&)originalWndProc, MyWndProc), "detaching from wndproc");
		if (originalFindGameCD != nullptr)
			SafeDetourCall(DetourDetach(&(PVOID&)originalFindGameCD, MyFindGameCD), "detaching from FindGameCD");
		SafeDetourCall(DetourTransactionCommit(), "committing detach transaction");
	}break;
	}

	return TRUE;
}
