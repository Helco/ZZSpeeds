cmake_minimum_required(VERSION 3.12)

project(SuperCrateBox)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

################
# Dependencies #
################
find_package(nlohmann_json CONFIG REQUIRED)

find_path(DETOURS_INCLUDE_DIR NAMES detours.h PATH_SUFFIXES detours)
find_library(DETOURS_LIBRARY_PATH NAMES detours PATH_SUFFIXES detours)
if ((DETOURS_INCLUDE_DIR STREQUAL "DETOURS_INCLUDE_DIR-NOTFOUND") OR
	(DETOURS_LIBRARY_PATH STREQUAL "DETOURS_LIBRARY_PATH-NOTFOUND"))
	message(FATAL_ERROR, "Could not find detours")
endif()
add_library(detours STATIC IMPORTED)
target_include_directories(detours INTERFACE ${DETOURS_INCLUDE_DIR})
set_property(TARGET detours PROPERTY IMPORTED_LOCATION ${DETOURS_LIBRARY_PATH})

#################
# Build targets #
#################

add_library(zzplugin INTERFACE)
target_include_directories(zzplugin INTERFACE zzplugin)

add_library(zzshared STATIC)
target_link_libraries(zzshared PUBLIC zzplugin)
target_include_directories(zzshared PUBLIC zzshared)
target_sources(zzshared PRIVATE
	zzshared/zzshared.cpp
)

add_library(zzloader MODULE)
target_link_libraries(zzloader PRIVATE
	zzshared
	detours
	nlohmann_json
)
target_sources(zzloader PRIVATE
	zzloader/zzloader.cpp
)

add_executable(zzlauncher)
target_link_libraries(zzlauncher PRIVATE
	zzshared
	detours
)
target_sources(zzlauncher PRIVATE
	zzlauncher/zzlauncher.cpp
)
