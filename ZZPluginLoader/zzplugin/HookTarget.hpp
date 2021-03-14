#pragma once
#include <cstdint>
#include <array>
#include <tuple>
#include <stdexcept>

namespace zz
{
	enum class GameVersion : uint32_t
	{
		v1002 = 1002,
		v1008 = 1008,
		v1010 = 1010,
		unkown = 0
	};
	constexpr int GameVersionCount = 3;

	class HookTarget
	{
	private:
		struct SpecificVersion
		{
			GameVersion version;
			uint32_t specificAddress;
		};

		uint32_t defaultAddress;
		std::array<SpecificVersion, GameVersionCount> versions;

		constexpr void InitializeVersions(int fromI) noexcept
		{
			for (int i = fromI; i < GameVersionCount; i++)
			{
				versions[i].specificAddress = 0;
				versions[i].version = GameVersion::unkown;
			}
		}

	public:
 		constexpr HookTarget(uint32_t _defaultAddress) noexcept :
			defaultAddress(_defaultAddress),
			versions()
		{
			InitializeVersions(0);
		}

		constexpr HookTarget(GameVersion v0, uint32_t a0) noexcept :
			defaultAddress(0),
			versions({ SpecificVersion{ v0, a0 } })
		{
			InitializeVersions(1);
		}

		constexpr HookTarget(GameVersion v0, uint32_t a0, GameVersion v1, uint32_t a1) noexcept :
			defaultAddress(0),
			versions({ SpecificVersion{ v0, a0 }, SpecificVersion{ v1, a1 } })
		{
			InitializeVersions(2);
		}

		constexpr HookTarget(GameVersion v0, uint32_t a0, GameVersion v1, uint32_t a1, GameVersion v2, uint32_t a2) noexcept :
			defaultAddress(0),
			versions({ SpecificVersion{ v0, a0 }, SpecificVersion{ v1, a1 }, SpecificVersion{ v2, a2 } })
		{
			InitializeVersions(3);
		}

		void* AddressFor(GameVersion version) const
		{
			for (int i = 0; i < GameVersionCount; i++)
			{
				if (versions[i].version == version)
					return reinterpret_cast<void*>(versions[i].specificAddress);
			}
			if (defaultAddress == 0)
				throw std::runtime_error("HookTarget does not support current game version");

			return reinterpret_cast<void*>(defaultAddress);
		}
	};
}
