#include "shared.hpp"

#include <iostream>
#include <string>
#include <sstream>

int runZanzarah(const char* filename, const char* dllPath, const std::string& cmdLinePrefix, std::optional<int> quickload = {})
{
	std::ostringstream cmdLineBuilder;
	cmdLineBuilder << cmdLinePrefix;
	if (quickload.has_value())
		cmdLineBuilder << " -quickload(" << quickload.value() << ")";
	auto cmdLine = cmdLineBuilder.str();

	STARTUPINFO startupInfo;
	ZeroMemory(&startupInfo, sizeof(startupInfo));
	startupInfo.cb = sizeof(STARTUPINFO);
	startupInfo.hStdOutput = GetStdHandle(STD_OUTPUT_HANDLE);
	startupInfo.hStdError = GetStdHandle(STD_ERROR_HANDLE);
	PROCESS_INFORMATION processInfo;
	ZeroMemory(&processInfo, sizeof(processInfo));
	if (!DetourCreateProcessWithDllEx(
		filename,	  // lpApplicationName
		&cmdLine[0],  // lpCommandLine
		NULL,		  // lpProcessAttributes
		NULL,		  // lpThreadAttributes
		FALSE,		  // bInheritHandles
		0,			  // dwCreationFlags
		NULL,		  // lpEnvironment
		NULL,		  // lpCurrentDirectory
		&startupInfo, // lpStartupInfo
		&processInfo, // lpProcessInformation,
		dllPath,	  // lpDllName
		NULL))		  // pfCreateProcessA
		ErrorExit("Could not start zanzarah");

	WaitForSingleObject(processInfo.hProcess, INFINITE);
	int32_t exitCode = -1;
	GetExitCodeProcess(processInfo.hProcess, reinterpret_cast<DWORD*>(&exitCode));
	return exitCode;
}

int main(int argc, char* argv[])
{
	auto versionOpt = GetGameVersion();
	if (!versionOpt)
		ErrorExit("Could not found supported version of Zanzarah");
	auto version = versionOpt.value();

	std::string cmdLinePrefix = version.filename;
	cmdLinePrefix.append(" -zanzarah -windowed");
	for (int i = 1; i < argc; i++)
	{
		cmdLinePrefix.append(argv[i]);
		cmdLinePrefix.append(" ");
	}

	std::string dllPath = argv[0];
	dllPath = dllPath.substr(0, dllPath.length() - 3) + "dll";

	std::optional<int> quickload = std::nullopt;
	while (true)
	{
		auto exitCode = runZanzarah(version.filename, dllPath.c_str(), cmdLinePrefix, quickload);
		if (exitCode < 0)
			break;
		else if (exitCode == 0) {
			std::cerr << "Restart without quickload" << std::endl;
			quickload = std::nullopt;
		}
		else {
			std::cerr << "Restart with quickload of slot " << exitCode << std::endl;
			quickload = exitCode;
		}
	}

	std::cerr << "No restart, exiting..." << std::endl;
	return 0;
}
