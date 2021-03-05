#pragma once
#include <cstdint>
#include <memory>
#include <functional>

#ifdef _MSC_VER
#define ZZPLUGIN_EXPORT __declspec(dllexport)
#else
#define ZZPLUGIN_EXPORT
#endif

namespace zz
{
	enum class GameVersion : uint32_t
	{
		v1002 = 1002,
		v1008 = 1008,
		v1010 = 1010,
		unkown = 0
	};

	class NonMovable
	{
	private:
		NonMovable(const NonMovable& const) = delete;
		NonMovable(NonMovable&&) = delete;
	};

	class IPlugin;
	class IPluginRegistry : NonMovable
	{
	public:
		virtual void Register(std::unique_ptr<IPlugin> plugin) = 0;
	};

	class IHookRegistry : NonMovable
	{
	public:
		virtual void Register(HookTarget target, std::function)
	};

	class IPlugin
	{
	public:
		virtual ~IPlugin() = default;

		virtual const char* Name() const = 0;
		virtual const char* DisplayName() const = 0;
		virtual const char* Author() const = 0;
		virtual const char* Version() const = 0;

		virtual void OnHook(zz::IHookRegistry& const registry) = 0;
		virtual void OnConfig(zz::IConfigRegistry& const registry) const = 0;
		virtual void OnConfigChanged(zz::IConfig& const config) const = 0;
	};
}

extern "C" ZZPLUGIN_EXPORT void register_plugins(zz::IPluginRegistry& const registry);
