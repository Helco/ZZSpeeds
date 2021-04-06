using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZAutosplitter.triggers
{
    class Start : ITrigger
    {
        private readonly GameState state;
        private readonly IntPtr savegameScreenPtr;
        private readonly MemoryWatcher<uint> currentScreenPtr;
        private readonly MemoryWatcher<int> shouldStart;
        private bool didStart = false;

        public IEnumerable<MemoryWatcher> MemoryWatchers => new MemoryWatcher[]
        {
            currentScreenPtr,
            shouldStart
        };

        public Start(GameState state)
        {
            this.state = state;
            if (!state.Process.ReadPointer(state.UIMgrPtr + state.Version.OffUIManagerToSavegameScreenPtr, out savegameScreenPtr))
                throw new ApplicationException("Could not read savegame screen ptr");
            currentScreenPtr = new MemoryWatcher<uint>(state.UIMgrPtr + state.Version.OffUIManagerToCurrentScreenPtr);
            shouldStart = new MemoryWatcher<int>(savegameScreenPtr + state.Version.OffSavegameScreenToInExitingAnimation);
        }

        public void Update()
        {
            if (!didStart &&
                currentScreenPtr.Current == (uint)savegameScreenPtr &&
                shouldStart.Current != 0)
            {
                state.TimerModel.Start();
                didStart = true;
            }
        }

        public void Dispose() { }
    }
}
