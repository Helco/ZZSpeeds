#include "shared.hpp"

#include <iostream>
#include <string>
#include <sstream>

int runZanzarah(const char* filename, const char* dllPath, std::string&& cmdLine)
{
	STARTUPINFO startupInfo;
	ZeroMemory(&startupInfo, sizeof(startupInfo));
	startupInfo.cb = sizeof(STARTUPINFO);
	startupInfo.hStdOutput = GetStdHandle(STD_OUTPUT_HANDLE);
	startupInfo.hStdError = GetStdHandle(STD_ERROR_HANDLE);
	PROCESS_INFORMATION processInfo;
	ZeroMemory(&processInfo, sizeof(processInfo));
	if (!DetourCreateProcessWithDllEx(
		filename,		// lpApplicationName
		cmdLine.data(),	// lpCommandLine
		NULL,			// lpProcessAttributes
		NULL,			// lpThreadAttributes
		FALSE,			// bInheritHandles
		0,				// dwCreationFlags
		NULL,			// lpEnvironment
		NULL,			// lpCurrentDirectory
		&startupInfo,	// lpStartupInfo
		&processInfo,	// lpProcessInformation,
		dllPath,		// lpDllName
		NULL))			// pfCreateProcessA
		ErrorExit("Could not start zanzarah");

	WaitForSingleObject(processInfo.hProcess, INFINITE);
	int32_t exitCode = -1;
	GetExitCodeProcess(processInfo.hProcess, reinterpret_cast<DWORD*>(&exitCode));
	return exitCode;
}

int main(int argc, char* argv[])
{
	const auto versionOpt = GetGameVersion();
	if (!versionOpt.has_value())
		ErrorExit("Could not found supported version of Zanzarah");
	const auto &version = versionOpt.value();

	std::string cmdLinePrefix = version.filename;
	cmdLinePrefix.append(" -zanzarah ");
	for (int i = 1; i < argc; i++)
	{
		cmdLinePrefix.append(argv[i]);
		cmdLinePrefix.append(" ");
	}

	std::string dllPath = argv[0];
	dllPath = dllPath.substr(0, dllPath.length() - 3) + "dll";

	int exitCode = 0;
	std::stringstream cmdLine;
	while (exitCode >= 0)
	{
		cmdLine.str("");
		cmdLine.clear();
		cmdLine << cmdLinePrefix;

		auto config = LoadWZConfig(); // reload every time, because it could have changed
		if (config.windowedMode)
			cmdLine << "-windowed ";
		
		if (exitCode == 0)
			std::cerr << "Restart without quickload" << std::endl;
		else if (exitCode == ExitCodeNoLauncher) {
			std::cerr << "Restart without launcher" << std::endl;
			cmdLine << NoLauncherArgument << " ";
		}
		else if (exitCode <= 10) {
			std::cerr << "Restart with quickload of slot " << exitCode << std::endl;
			cmdLine << "-quickload(" << exitCode << ") ";
		}
		else {
			std::cerr << "Warning: Unknown exit code " << exitCode << std::endl;
		}

		exitCode = runZanzarah(version.filename, dllPath.c_str(), std::move(cmdLine.str()));
	}

	std::cerr << "No restart, exiting..." << std::endl;
	return 0;
}
