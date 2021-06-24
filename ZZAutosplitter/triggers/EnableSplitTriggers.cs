using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZAutosplitter.triggers
{
    class EnableSplitTriggers : ITrigger
    {
        private readonly GameState state;
        private readonly IntPtr savegameScreenPtr;
        private readonly MemoryWatcher<uint> currentScreenPtr;
        private bool didEnabledSplits = false;

        public IEnumerable<MemoryWatcher> MemoryWatchers => new[]
        {
            currentScreenPtr
        };

        public EnableSplitTriggers(GameState state)
        {
            this.state = state;
            if (!state.Process.ReadPointer(state.UIMgrPtr + state.Version.OffUIManagerToSavegameScreenPtr, out savegameScreenPtr))
                throw new ApplicationException("Could not read savegame screen ptr");
            currentScreenPtr = new MemoryWatcher<uint>(state.UIMgrPtr + state.Version.OffUIManagerToCurrentScreenPtr);
        }

        public void Update()
        {
            if (!didEnabledSplits && currentScreenPtr.Current != (uint)savegameScreenPtr)
            {
                state.AreSplitsEnabled = true;
                didEnabledSplits = true;
            }
        }

        public void Dispose() { }
    }
}
