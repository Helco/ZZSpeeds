#pragma once
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include "../detours/detours.h"

#include <optional>
#include <sstream>

static const DWORD NO_CRACK_NECESSARY = ((DWORD)-1);

struct GameVersionInfo
{
	const char* descriptiveName;
	DWORD exeSize; // to distinguish between versions
	DWORD addrWndProc;
	DWORD addrFindGameCD;
};

struct GameVersion
{
	const char* filename;
	GameVersionInfo info;
};

static const GameVersionInfo GameVersionInfos[] = {
	{ "1.002", 2162688, 0x403431, 0x417B99 }, // German CD release
	{ "1.010", 2162784, 0x403428, NO_CRACK_NECESSARY }, // Steam release (Russian)
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
