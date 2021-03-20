using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ZZAutosplitter.versions
{
    abstract class GameVersion
    {
		public static IReadOnlyCollection<GameVersion> All = new GameVersion[]
		{
			new GameVersion1002(),
			new GameVersion1010(),
		};
		public static GameVersion GetVersion(Process process) => All.FirstOrDefault(p => p.Match(process));

		public virtual bool Match(Process process) => process.ProcessName == ProcessName;

        public abstract string ProcessName { get; }
		// at some point we will want to have a second identification method to support 1.008

        public abstract string SigGame { get; }

        public abstract int OffGameToPlayer { get; }
        public abstract int OffSceneToDataset { get; }
        public abstract int OffResMgrToVideoMgr { get; }
        public abstract int OffLoadSceneEnterStart { get; }
        public abstract int OffLoadSceneEnterEnd { get; }
        public abstract int OffLoadSceneExitStart { get; }
        public abstract int OffLoadSceneExitEnd { get; }

        public virtual int LenLoadSceneEnter => OffLoadSceneEnterEnd - OffLoadSceneEnterStart;
        public virtual int LenLoadSceneExit => OffLoadSceneExitStart - OffLoadSceneExitStart;
		public virtual int LenVideoMgrFilename => 64;

		public virtual int OffPlayerToUIMgr => 0x188;
		public virtual int OffPlayerToInventory => 0x7C;
		public virtual int OffPlayerToCurrentNPC => 0x294;

		public virtual int OffUIManagerToSavegameScreenPtr => 0x84;
		public virtual int OffUIManagerToCurrentScreenPtr => 0x38;
		public virtual int OffUIManagerToDialogScreenPtr => 0x74;
		public virtual int OffUIManagerToMainMenuScreenPtr => 0x80;
		public virtual int OffSavegameScreenToInExitingAnimation => 0xF8;
		public virtual int OffInventoryToCardsList => 0x00;
		public virtual int OffListToCapacity => 0x0;
		public virtual int OffListToCount => 0x8;
		public virtual int OffListToData => 0x10;
		public virtual int OffListToIndexMap => 0x14;
		public virtual int OffInventorySlotToCardId => 0x04;
		public virtual int OffInventorySlotToAmount => 0x0C;
		public virtual int OffDialogScreenToCauseType => 0xB5C;
		public virtual int OffNPCToDatabaseRow => 0x13C;
		public virtual int OffDatabaseRowToUID => 0x14;
		public virtual int OffGameToResMgr => 0x38;
		public virtual int OffResMgrToScene => 0x8;
		public virtual int OffDatasetToDatasetStruct => 0x24;
		public virtual int OffDatasetStructToSceneId => 0x0;
		public virtual int OffVideoMgrToFilename => 0x14;
    }
}
