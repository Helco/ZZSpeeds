#pragma once
#include <cstdint>
#include <memory>
#include <functional>

#include "HookTarget.hpp"

#ifdef _MSC_VER
#define ZZPLUGIN_EXPORT __declspec(dllexport)
#else
#define ZZPLUGIN_EXPORT
#endif

static_assert(sizeof(void*) == sizeof(uint32_t), "Zanzarah runs in x86, so does this project!");

namespace zz
{
	class NonMovable
	{
	private:
		NonMovable(const NonMovable&) = delete;
		NonMovable(NonMovable&&) = delete;
	};

	class IPlugin;
	class IPluginRegistry : NonMovable
	{
	public:
		virtual void Register(std::unique_ptr<IPlugin> plugin) = 0;
	};

	struct ConfigVarId { uint32_t value; };
	class IPluginInit : NonMovable
	{
	public:
		virtual GameVersion GameVersion() const = 0;
		virtual void* HookRaw(HookTarget target, void* functionAddress) = 0;
	};

	class IPlugin
	{
	public:
		virtual ~IPlugin() = default;

		virtual const char* Name() const = 0;
		virtual const char* DisplayName() const = 0;
		virtual const char* Author() const = 0;
		virtual const char* Version() const = 0;

		virtual void OnInit(zz::IPluginInit& registry) = 0;
	};
}

extern "C" ZZPLUGIN_EXPORT void register_plugins(zz::IPluginRegistry& registry);
