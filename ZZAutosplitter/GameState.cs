using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using ZZAutosplitter.versions;
using ZZAutosplitter.triggers;

namespace ZZAutosplitter
{
    class GameState : IDisposable
    {
        public Database Database { get; }
        public LiveSplitState LiveSplitState { get; }
        public TimerModel TimerModel { get; }
        public Process Process { get; }
        public GameVersion Version { get; }
        public Settings Settings { get; }
        public IntPtr GamePtr { get; private set; } = IntPtr.Zero;
        public Inventory Inventory => (Inventory)triggers?[0];

        /* At the start event (current runs start with a click on a new game button)
         * the inventory is not cleared yet so all "getting" split rules would
         * get triggered from the last run.
         * We prevent that by disabling all split rule triggers until we are no
         * longer in the savegame screen.
         */
        public bool AreSplitsEnabled { get; set; } = false;

        public IntPtr PlayerPtr => GamePtr == IntPtr.Zero ? IntPtr.Zero
            : GamePtr + Version.OffGameToPlayer;
        public IntPtr ResMgrPtr => GamePtr == IntPtr.Zero ? IntPtr.Zero
            : GamePtr + Version.OffGameToResMgr;
        public IntPtr UIMgrPtr => GamePtr == IntPtr.Zero ? IntPtr.Zero
            : PlayerPtr + Version.OffPlayerToUIMgr;

        private bool isDisposed;
        private List<ITrigger> triggers = new();
        private Dictionary<Type, IInjector> injectors = new();
        private MemoryWatcherList memWatchers = null;

        public GameState(Database database, LiveSplitState liveSplitState, Process process, GameVersion version, Settings settings)
        {
            Database = database;
            LiveSplitState = liveSplitState;
            Process = process;
            Version = version;
            Settings = settings;

            TimerModel = new TimerModel() { CurrentState = liveSplitState };
            TimerModel.OnReset += (_, __) => triggers.Clear();
            TimerModel.OnStart += (_, __) => AreSplitsEnabled = false;
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
            triggers.Clear();
            triggers.Add(new Inventory(this));

            if (Settings.EnableAutoSplits)
            {
                triggers.Add(new EnableSplitTriggers(this));
                triggers.AddRange(Settings.SplitRules
                    .Where(rule => rule.Enabled)
                    .Select<SplitRule, ITrigger>(rule => rule switch
                    {
                        SplitRuleGettingCards gc            => new SplitOnGettingCards(this, gc),
                        SplitRuleGettingFairiesOfClass gfoc => new SplitOnGettingFairiesOfClass(this, gfoc),
                        SplitRuleGettingTotalFairies gtf    => new SplitOnGettingTotalFairies(this, gtf),
                        SplitRuleReaching r                 => new SplitOnReaching(this, r),
                        SplitRuleDefeating d                => new SplitOnDefeating(this, d),
                        SplitRuleWatching w                 => new SplitOnWatching(this, w),
                        SplitRulePixies p                   => new SplitOnPixies(this, p),
                        _ => throw new NotSupportedException($"Unsupported split rule type {rule?.GetType()}")
                    }));
            }

            if (Settings.EnableAutoStart)
                triggers.Add(new Start(this));

            if (Settings.EnableLoadTimeRemoval)
                triggers.Add(new LoadRemover(this));

            memWatchers = new MemoryWatcherList();
            memWatchers.AddRange(triggers.SelectMany(t => t.MemoryWatchers));
        }

        public void UpdateTriggers()
        {
            if (!triggers.Any())
                InitialiseTriggers();

            memWatchers.UpdateAll(Process);
            foreach (var trigger in triggers)
                trigger.Update();
        }

        public T GetInjector<T>() where T : IInjector, new()
        {
            if (injectors.TryGetValue(typeof(T), out var prevInjector))
                return (T)prevInjector;

            var injector = new T();
            injector.InjectInto(this);
            injectors[typeof(T)] = injector;
            return injector;
        }

        protected virtual void DoDispose()
        {
            if (isDisposed)
                return;
            isDisposed = true;
            foreach (var trigger in triggers)
                trigger.Dispose();
            triggers.Clear();
        }

        public void Dispose()
        {
            DoDispose();
            GC.SuppressFinalize(this);
        }
    }
}
