#pragma once
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include "../detours/detours.h"

#include <optional>
#include <sstream>
#include <fstream>
#include <algorithm>

// grr... C macros messing with C++ STL
#undef min
#undef max 

static_assert(sizeof(void*) == 4, "Zanzarah runs in x86, so does WindowedZanzarah!");

static const DWORD NO_HOOK_NECESSARY = ((DWORD)-1);
static constexpr const char* WZConfigFile = "..\\Configs\\WindowedZanzarah.cfg";
static constexpr int ExitCodeNoLauncher = 42;
static constexpr const char* NoLauncherArgument = "-nolauncher";

struct GameVersionInfo
{
	const char* descriptiveName;
	DWORD exeSize; // to distinguish between versions
	DWORD resID_videoSettingsTab;
	DWORD addrWndProc;
	DWORD addrFindGameCD;
	DWORD addrCheckSerialNumber;
	DWORD addrUICursor_update;
	DWORD addrUICursor_setVisible;
	DWORD addrCallSetCursorPos; // this is a function that calls the imported SetCursorPos, lucky for us!
	DWORD addrGame_tick;
	DWORD addrResolutionModeIndex;
	DWORD addrResolutionModes;
	DWORD addrCreateSingleInstanceMutex;
	DWORD addrInputMgr_update;
};

struct GameVersion
{
	const char* filename;
	GameVersionInfo info;
};

#pragma pack(push, 1)
struct WZConfig
{
	bool windowedMode;
	bool ignoreFocusLoss;
};
#pragma pack(pop)

struct ResolutionMode
{
	int width;
	int height;
	int depth;
};

static const GameVersionInfo GameVersionInfos[] = {
	{
		"1.002 German CD Release",
		2162688,	// exeSize
		108,		// resID_videoSettingsTab
		0x403431,	// addrWndProc
		0x417B99,	// addrFindGameCD
		0x4B2DB0,	// addrCheckSerialNumber
		0x4211C4,	// addrUICursor_update
		0x42111A,	// addrUICursor_setVisible // this hook is needed because the cursor pos is guaranteed to be correct after this function
		0x42126E,	// addrCallSetCursorPos
		0x4A2840,	// addrGame_tick
		0x5C5D98,	// addrResolutionIndex
		0x5A4C90,	// addrResolutionModes
		0x4011A8,	// addrCreateSingleInstanceMutex
		0x417DCC	// addrInputMgr_update
	},

	{
		"1.010 Russian Steam Release",
		2166784,			// exeSize
		108,				// resID_videoSettingsTab
		0x403428,			// addrWndProc
		NO_HOOK_NECESSARY,	// addrFindGameCD
		NO_HOOK_NECESSARY,	// addrCheckSerialNumber
		0x4223A9,			// addrUICursor_update
		0x4222FF,			// addrUICursor_setVisible
		0x422453,			// addrCallSetCursorPos
		0x4A3E5F,			// addrGame_tick
		0x5C6D98,			// addrResolutionModeIndex
		0x5A5CA0,			// addrResolutionModes,
		0x4011A8,			// addrCreateSingleInstanceMutex
		0x417A19			// addrInputMgr_update
	},

	{ nullptr, 0, 0 }
};

static const WZConfig DefaultWZConfig = {
	false,	// windowedMode
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

WZConfig LoadWZConfig()
{
	std::ifstream stream;
	stream.exceptions(std::ifstream::failbit);
	try {
		stream.open(WZConfigFile, std::ifstream::in | std::ifstream::binary);
		size_t structureSize;
		stream.read(reinterpret_cast<char*>(&structureSize), sizeof(size_t));
		if (structureSize > sizeof(WZConfig))
			OutputDebugString("WARNING: WZConfig is being sliced! Some options will be lost");

		WZConfig config = DefaultWZConfig;
		stream.read(reinterpret_cast<char*>(&config), std::min(structureSize, sizeof(WZConfig)));
		return config;
	}
	catch (const std::ifstream::failure& e) {
		OutputDebugString("Could not load WindowedZanzarah config, reset to default values, exception was:");
		OutputDebugString(e.what());
		return DefaultWZConfig;
	}
}

void SaveWZConfig(const WZConfig& config)
{
	std::ofstream stream;
	stream.exceptions(std::ofstream::failbit);
	try {
		stream.open(WZConfigFile, std::ofstream::out | std::ofstream::binary);
		const size_t structureSize = sizeof(WZConfig);
		stream.write(reinterpret_cast<const char*>(&structureSize), sizeof(size_t));
		stream.write(reinterpret_cast<const char*>(&config), sizeof(WZConfig));
	}
	catch (const std::ofstream::failure& e) {
		OutputDebugString("Could not save WindowedZanzarah config, just ignore... exception was:");
		OutputDebugString(e.what());
	}
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
