using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using ZZAutosplitter.versions;

namespace ZZAutosplitter
{
    class GameState : IDisposable
    {
        public LiveSplitState LiveSplitState { get; }
        public Process Process { get; }
        public GameVersion Version { get; }
        public Settings Settings { get; }
        public IntPtr GamePtr { get; private set; } = IntPtr.Zero;
        public Inventory Inventory => (Inventory)triggers?[0];

        public IntPtr PlayerPtr => GamePtr == IntPtr.Zero ? IntPtr.Zero
            : GamePtr + Version.OffGameToPlayer;
        public IntPtr ResMgrPtr => GamePtr == IntPtr.Zero ? IntPtr.Zero
            : GamePtr + Version.OffGameToResMgr;

        private bool isDisposed;
        private ITrigger[] triggers = null;
        private MemoryWatcherList memWatchers = null;

        public GameState(LiveSplitState liveSplitState, Process process, GameVersion version, Settings settings)
        {
            LiveSplitState = liveSplitState;
            Process = process;
            Version = version;
            Settings = settings;
        }

        public bool FindGamePointer()
        {
            if (GamePtr != IntPtr.Zero)
                return true;

            var sigTarget = new SigScanTarget(Version.SigGame);
            foreach (var page in Process.MemoryPages().Where(p => p.Protect.HasFlag(MemPageProtect.PAGE_READWRITE)))
            {
                var sigScanner = new SignatureScanner(Process, page.BaseAddress, (int)page.RegionSize);
                GamePtr = sigScanner.Scan(sigTarget);
                if (GamePtr != IntPtr.Zero)
                    return true;
            }

            GamePtr = IntPtr.Zero;
            return false;
        }

        private void InitialiseTriggers()
        {
            triggers = new[]
            {
                new Inventory(this)
            };

            memWatchers = new MemoryWatcherList();
            memWatchers.AddRange(triggers.SelectMany(t => t.MemoryWatchers));
        }

        public void UpdateTriggers()
        {
            if (triggers == null)
                InitialiseTriggers();

            memWatchers.UpdateAll(Process);
            foreach (var trigger in triggers)
                trigger.Update();
        }

        protected virtual void DoDispose()
        {
            if (isDisposed)
                return;
            isDisposed = true;
            foreach (var trigger in triggers ?? Array.Empty<ITrigger>())
                trigger.Dispose();
            triggers = null;
        }

        public void Dispose()
        {
            DoDispose();
            GC.SuppressFinalize(this);
        }
    }
}
