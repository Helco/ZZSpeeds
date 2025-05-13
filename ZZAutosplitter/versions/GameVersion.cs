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
		public abstract int OffLeaveDuelStart { get; } // actually at a specific time where the NPC is in eax
		public abstract int OffLeaveDuelEnd { get; }

        public virtual int LenLoadSceneEnter => OffLoadSceneEnterEnd - OffLoadSceneEnterStart;
        public virtual int LenLoadSceneExit => OffLoadSceneExitStart - OffLoadSceneExitStart;
		public virtual int LenVideoMgrFilename => 64;

		public virtual int OffPlayerToUIMgr => 0x188;
		public virtual int OffPlayerToInventory => 0x7C;
		public virtual int OffPlayerToCurrentNPC => 0x294;
		public virtual int OffPlayerToPixieCount => 0x480; // that is the alltime pixie count, not the current pixie count

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
		public virtual int OffGameflowToTriggerArg1 => 0x238;

		private static readonly IReadOnlyCollection<byte> leaveDuelCodeBase = new byte[]
		{
			0x8b, 0x98, 0, 0, 0, 0, // mov ebx, DWORD PTR [eax+OffNPCToDatabaseRow]
			0x8b, 0x9b, 0, 0, 0, 0, // mov ebx, DWORD PTR [ebx+OffDatabaseRowToUID]
			0x89, 0x1d, 0, 0, 0, 0, // mov DWORD PTR npcUIDTarget, ebx
			0x8b, 0x99, 0, 0, 0, 0, // mov ebx, DWORD PTR [ecx+OffGameflowToTriggerArg1]
			0x89, 0x1d, 0, 0, 0, 0, // mov DWORD PTR fightResultTarget, ebx
			0xe9, 0, 0, 0, 0		// jmp GATE
		};
		public int LeaveDuelCodeSize => leaveDuelCodeBase.Count;

		public byte[] LeaveDuelCode(IntPtr gate, IntPtr dest, IntPtr npcUIDTarget, IntPtr fightResultTarget)
        {
			var code = leaveDuelCodeBase.ToArray();
			int jumpSize = gate.ToInt32() - dest.ToInt32() - LeaveDuelCodeSize;
			BitConverter.GetBytes(OffNPCToDatabaseRow)			.CopyTo(code, 0 * 6 + 2);
			BitConverter.GetBytes(OffDatabaseRowToUID)			.CopyTo(code, 1 * 6 + 2);
			BitConverter.GetBytes(npcUIDTarget.ToInt32())		.CopyTo(code, 2 * 6 + 2);
			BitConverter.GetBytes(OffGameflowToTriggerArg1)		.CopyTo(code, 3 * 6 + 2);
			BitConverter.GetBytes(fightResultTarget.ToInt32())	.CopyTo(code, 4 * 6 + 2);
			BitConverter.GetBytes(jumpSize)						.CopyTo(code, 5 * 6 + 1);
			return code;
        }

		private static readonly IReadOnlyCollection<byte> toggleIsLoadingCodeBase = new byte[]
		{
			0x66, 0x83, 0x35, 0, 0, 0, 0, 0x01, // xor DWORD PTR [isPlayingTarget], 1
			0xE9, 0, 0, 0, 0					// jmp GATE
		};
		public int ToggleIsLoadingCodeSize => toggleIsLoadingCodeBase.Count;

		public byte[] ToggleIsLoadingCode(IntPtr gate, IntPtr dest, IntPtr isPlayingTarget)
        {
			var code = toggleIsLoadingCodeBase.ToArray();
			int jumpSize = gate.ToInt32() - dest.ToInt32() - ToggleIsLoadingCodeSize;
			BitConverter.GetBytes(isPlayingTarget.ToInt32()).CopyTo(code, 3);
			BitConverter.GetBytes(jumpSize)					.CopyTo(code, 9);
			return code;
        }
    }
}
