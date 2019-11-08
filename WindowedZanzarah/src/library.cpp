#include "shared.hpp"

using FnWndProc = int(__stdcall*)(HWND, UINT, WPARAM, LPARAM);
static FnWndProc originalWndProc;
static bool isWindowActivated = false;

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

	if (isWindowActivated) {
		RECT rect;
		GetClientRect(hWnd, &rect);
		MapWindowPoints(hWnd, HWND_DESKTOP, reinterpret_cast<POINT*>(&rect), 2);
		ClipCursor(&rect);
	}

	if (shouldRepositionWindow) {
		// the centering should not be too annoying, do it only if things change
		auto monitor = MonitorFromWindow(hWnd, MONITOR_DEFAULTTONEAREST);
		MONITORINFO monitorInfo;
		ZeroMemory(&monitorInfo, sizeof(MONITORINFO));
		monitorInfo.cbSize = sizeof(MONITORINFO);
		GetMonitorInfo(monitor, &monitorInfo);
		RECT windowSize;
		GetWindowRect(hWnd, &windowSize);
		SetWindowPos(hWnd, HWND_TOP,
			(monitorInfo.rcMonitor.left + monitorInfo.rcMonitor.right) / 2 - (windowSize.right - windowSize.left) / 2,
			(monitorInfo.rcMonitor.top + monitorInfo.rcMonitor.bottom) / 2 - (windowSize.bottom - windowSize.top) / 2,
			0, 0,
			SWP_NOSIZE);
	}

	return originalWndProc(hWnd, msg, wParam, lParam);
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

		SafeDetourCall(DetourTransactionBegin(), "beginning attach transaction");
		SafeDetourCall(DetourUpdateThread(GetCurrentThread()), "updating thread");
		SafeDetourCall(DetourAttach(&(PVOID&)originalWndProc, MyWndProc), "attaching to wndproc");
		SafeDetourCall(DetourTransactionCommit(), "committing attach transaction");
	}break;

	case DLL_PROCESS_DETACH: {
		OutputDebugString("WindowedZanzarah hook is detaching");

		SafeDetourCall(DetourTransactionBegin(), "beginning detach transaction");
		SafeDetourCall(DetourUpdateThread(GetCurrentThread()), "updating thread");
		SafeDetourCall(DetourDetach(&(PVOID&)originalWndProc, MyWndProc), "detaching from wndproc");
		SafeDetourCall(DetourTransactionCommit(), "committing detach transaction");
	}break;
	}

	return TRUE;
}
