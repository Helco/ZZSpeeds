#pragma once
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include "../detours/detours.h"

#include <optional>
#include <sstream>

static_assert(sizeof(void*) == 4, "Zanzarah runs in x86, so does WindowedZanzarah!");

static const DWORD NO_HOOK_NECESSARY = ((DWORD)-1);

struct GameVersionInfo
{
	const char* descriptiveName;
	DWORD exeSize; // to distinguish between versions
	DWORD addrWndProc;
	DWORD addrFindGameCD;
	DWORD addrCheckSerialNumber;
	DWORD addrUICursor_update;
	DWORD addrCallSetCursorPos; // this is a function that calls the imported SetCursorPos, lucky for us!
};

struct GameVersion
{
	const char* filename;
	GameVersionInfo info;
};

static const GameVersionInfo GameVersionInfos[] = {
	{
		"1.002 German CD Release",
		2162688,	// exeSize
		0x403431,	// addrWndProc
		0x417B99,	// addrFindGameCD
		0x4B2DB0,	// addrCheckSerialNumber
		0x4211C4,	// addrUICursor_update
		0x42126E	// addrCallSetCursorPos
	},

	{
		"1.010 Russian Steam Release",
		2166784,			// exeSize
		0x403428,			// addrWndProc
		NO_HOOK_NECESSARY,	// addrFindGameCD
		NO_HOOK_NECESSARY,	// addrCheckSerialNumber
		0x4223A9,			// addrUICursor_update
		0x422453			// addrCallSetCursorPos
	},

	{ nullptr, 0, 0 }
};

std::optional<GameVersion> GetGameVersionOf(const char* filename)
{
	WIN32_FILE_ATTRIBUTE_DATA fad;
	if (!GetFileAttributesExA(filename, GetFileExInfoStandard, &fad))
		return {};

	const GameVersionInfo* curInfo = GameVersionInfos;
	while (curInfo->descriptiveName != nullptr)
	{
		if (curInfo->exeSize == fad.nFileSizeLow) {
			GameVersion version;
			version.filename = filename;
			version.info = *curInfo;
			return version;
		}
		curInfo++;
	}
	return {};
}

std::optional<GameVersion> GetGameVersion()
{
	auto version = GetGameVersionOf("zanthp.exe");
	if (!version)
		version = GetGameVersionOf("main.exe");
	return version;
}

// https://stackoverflow.com/questions/1387064/how-to-get-the-error-message-from-the-error-code-returned-by-getlasterror
std::string GetErrorAsString(DWORD errorMessageID)
{
	if (errorMessageID == 0)
		return std::string(); //No error message has been recorded

	LPSTR messageBuffer = nullptr;
	size_t size = FormatMessageA(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL, errorMessageID, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT), (LPSTR)&messageBuffer, 0, NULL);

	std::string message(messageBuffer, size - 2); // remove the newline and carriage return
	LocalFree(messageBuffer);
	return message;
}

void ErrorExit(const char* const message)
{
	OutputDebugString(message);
	DebugBreak();
	ExitProcess(-1); // std::terminate crashes in the hooked process
}

void SafeDetourCall(LONG error, const char* position = nullptr)
{
	if (error != NO_ERROR) {
		std::ostringstream errorMessage;
		errorMessage << "Detour says " << error << " (" << GetErrorAsString(error) << ")";
		if (position != nullptr)
			errorMessage << " when trying to " << position;
		auto errorMessageStr = errorMessage.str();
		ErrorExit(errorMessageStr.c_str());
	}
}
