#pragma once
#define WIN32_LEAN_AND_MEAN
#include <Windows.h>
#include "../detours/detours.h"

#include <optional>
#include <sstream>
#include <fstream>
#include <algorithm>
#include <vector>
#include <string_view>
#include <span>
#include <cassert>

// grr... C macros messing with C++ STL
#undef min
#undef max 

static_assert(sizeof(void*) == 4, "Zanzarah runs in x86, so does WindowedZanzarah!");

enum class PatchKind
{
	Constant,
	MemSet,
	ResWidth,
	ResHeight,
	HeightOverWidth,
	RatioFactor
};

struct PatchPiece
{
	PatchKind kind;
	std::string_view data;
	size_t count;

	constexpr PatchPiece(PatchKind kind = PatchKind::Constant) : kind(kind), count(0) {}
	constexpr PatchPiece(const char *data) : kind(PatchKind::Constant), data(data), count(0) {}
};

PatchPiece PatchMemSet(const char *byte, size_t count)
{
	PatchPiece piece(PatchKind::MemSet);
	piece.data = byte;
	piece.count = count;
	return piece;
}

class PatchEntry
{
	static constexpr const size_t maxPieces = 8;

	PatchPiece allPieces[maxPieces];
public:
	DWORD offset;
	std::span<PatchPiece> pieces;

	constexpr PatchEntry(DWORD offset, std::initializer_list<PatchPiece> pieces)
		: offset(offset)
		, allPieces()
		, pieces(&allPieces[0], pieces.size())
	{
		assert(pieces.size() <= maxPieces);
		std::copy(pieces.begin(), pieces.end(), allPieces);
	}
};

PatchEntry PatchEntryRatioFactor(DWORD offset) {
	return { offset, { PatchKind::RatioFactor } };
}

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
	std::span<const PatchEntry> highResolutionPatch;
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
	bool applyHighResolutionPatch;
	bool padding;

	uint16_t resWidth, resHeight;
};
#pragma pack(pop)

struct ResolutionMode
{
	int width;
	int height;
	int depth;

	std::string toString() const
	{
		std::stringstream sstream;
		sstream << width << 'x' << height << 'x' << depth;
		return sstream.str();
	}

	bool operator == (const ResolutionMode &other)
	{
		return width == other.width && height == other.height && depth == other.depth;
	}
};

// converted from https://web.archive.org/web/20180313011325/http://forum.daedalic.de/viewtopic.php?f=273&t=5949#p25774
// TODO: Annotate with meaning per entry

static const PatchEntry SteamHighResolutionPatch[] = {
	{ 0x001078, { "9090" } },
	{ 0x01128c, { "e90e2c1900" } },
	{ 0x0f5c43, { "59595968", PatchKind::ResHeight, "68", PatchKind::ResWidth, "5233c9894c2414894c2410b820000000" } },
	{ 0x1a3e9f, { "8d732c8bcec7442408", PatchKind::ResWidth, "c744240c", PatchKind::ResHeight, "e9d8d3e6ff" } },
	{ 0x1a5fcc, { PatchKind::HeightOverWidth } },
	{ 0x1a6508, { PatchKind::HeightOverWidth } },
	{ 0x019430, { "e987aa1800", PatchMemSet("90", 26) } },
	{ 0x1a3ebc, { "c74108", PatchKind::ResWidth, "c7410c", PatchKind::ResHeight, "c7411020000000c74114000000005ec20400" } },

	PatchEntryRatioFactor(0x1a60ac), // skills
	PatchEntryRatioFactor(0x1a7278), // icon
	PatchEntryRatioFactor(0x1a727c),
	PatchEntryRatioFactor(0x1a7290), // gauge
	PatchEntryRatioFactor(0x1a7294),
	PatchEntryRatioFactor(0x1a6e6c), // bars
	PatchEntryRatioFactor(0x1a72ac),

	// Non-original patches
	{ 0x0F510A, { "E963010000" } } // speeds up startup by ignoring some self-diagnosis related to color widths?
};

static const GameVersionInfo GameVersionInfos[] = {
	{
		.descriptiveName = "1.002 German CD Release",
		.exeSize = 2162688,	// exeSize
		.resID_videoSettingsTab = 108,		// resID_videoSettingsTab
		.addrWndProc = 0x403431,	// addrWndProc
		.addrFindGameCD = 0x417B99,	// addrFindGameCD
		.addrCheckSerialNumber = 0x4B2DB0,	// addrCheckSerialNumber
		.addrUICursor_update = 0x4211C4,	// addrUICursor_update
		.addrUICursor_setVisible = 0x42111A,	// addrUICursor_setVisible // this hook is needed because the cursor pos is guaranteed to be correct after this function
		.addrCallSetCursorPos = 0x42126E,	// addrCallSetCursorPos
		.addrGame_tick = 0x4A2840,	// addrGame_tick
		.addrResolutionModeIndex = 0x5C5D98,	// addrResolutionIndex
		.addrResolutionModes = 0x5A4C90,	// addrResolutionModes
		.addrCreateSingleInstanceMutex = 0x4011A8,	// addrCreateSingleInstanceMutex
		.addrInputMgr_update = 0x417DCC	// addrInputMgr_update
	},

	{
		.descriptiveName = "1.010 Russian Steam Release",
		.exeSize = 2166784,
		.resID_videoSettingsTab = 108,
		.addrWndProc = 0x403428,
		.addrFindGameCD = NO_HOOK_NECESSARY,
		.addrCheckSerialNumber = NO_HOOK_NECESSARY,
		.addrUICursor_update = 0x4223A9,
		.addrUICursor_setVisible = 0x4222FF,
		.addrCallSetCursorPos = 0x422453,
		.addrGame_tick = 0x4A3E5F,
		.addrResolutionModeIndex = 0x5C6D98,
		.addrResolutionModes = 0x5A5CA0,
		.addrCreateSingleInstanceMutex = 0x4011A8,
		.addrInputMgr_update = 0x417A19,
		.highResolutionPatch = { SteamHighResolutionPatch, _countof(SteamHighResolutionPatch) }
	},

	{ nullptr, 0, 0 }
};

static const WZConfig DefaultWZConfig = {
	.windowedMode = false,
	.ignoreFocusLoss = false,
	.applyHighResolutionPatch = false,

	.resWidth = 1024,
	.resHeight = 768
};

struct
{
public:
	DWORD offset;
	std::vector<uint8_t> pattern1; // from offset to uint32 of resolution width
	std::vector<uint8_t> pattern2; // from end of resolution width to start of resolution height

	const uint8_t* BasePointer() const { return reinterpret_cast<const uint8_t*>(offset); }
	auto OffsetToPattern2() const { return pattern1.size() + sizeof(uint32_t); }
	uint32_t ResolutionWidth() const { return *reinterpret_cast<const uint32_t*>(offset + pattern1.size()); }
	uint32_t ResolutionHeight() const { return *reinterpret_cast<const uint32_t*>(offset + OffsetToPattern2() + pattern2.size()); }

	bool IsInstalled() const
	{
		return
			memcmp(BasePointer(), pattern1.data(), pattern1.size()) == 0 &&
			memcmp(BasePointer() + OffsetToPattern2(), pattern2.data(), pattern2.size()) == 0;
	}
} static const WidescreenPatchDetection = {
	0x5A3EBC,
	{ 0xC7, 0x41, 0x08 },
	{ 0xC7, 0x41, 0x0C }
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

void __declspec(noreturn) ErrorExit(const char* const message)
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
