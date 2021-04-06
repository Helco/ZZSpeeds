using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter.triggers
{
    class SplitOnDefeating : SplitRuleTrigger<SplitRuleDefeating>
    {
        private readonly IntPtr dialogScreenPtr;
        private readonly MemoryWatcher<uint> currentScreenPtr;
        private readonly MemoryWatcher<int> causeType;
        private readonly MemoryWatcher<uint> currentNPCPtr;

        public SplitOnDefeating(GameState state, SplitRuleDefeating rule) : base(state, rule)
        {
            if (!state.Process.ReadPointer(state.UIMgrPtr + state.Version.OffUIManagerToDialogScreenPtr, out dialogScreenPtr))
                throw new ApplicationException("Could not read dialog screen ptr");
            currentScreenPtr = new MemoryWatcher<uint>(state.UIMgrPtr + state.Version.OffUIManagerToCurrentScreenPtr);
            causeType = new MemoryWatcher<int>(dialogScreenPtr + state.Version.OffDialogScreenToCauseType);
            currentNPCPtr = new MemoryWatcher<uint>(state.PlayerPtr + state.Version.OffPlayerToCurrentNPC);
        }

        public override IEnumerable<MemoryWatcher> MemoryWatchers => new MemoryWatcher[]
        {
            currentScreenPtr,
            causeType,
            currentNPCPtr
        };

        protected override bool ShouldSplit()
        {
            if (currentScreenPtr.Current != (uint)dialogScreenPtr ||
                causeType.Current != 4 || // player won fight
                currentNPCPtr.Current == 0)
                return false;

            if (!State.Process.ReadPointer(new IntPtr(currentNPCPtr.Current + State.Version.OffNPCToDatabaseRow), out var npcDbRowPtr) ||
                !State.Process.ReadValue(npcDbRowPtr + State.Version.OffDatabaseRowToUID, out uint npcUID))
            {
                Debug.WriteLine("Could not read NPC database UID");
                return false;
            }

            Debug.WriteLine($"You defeated {npcUID:X8}");
            return npcUID == Rule.UID;
        }
    }
}
