using LiveSplit.ComponentUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZAutosplitter.triggers
{
    abstract class SplitRuleTrigger<TSplitRule> : ITrigger where TSplitRule : SplitRule
    {
        protected abstract bool ShouldSplit();

        public GameState State { get; }
        public TSplitRule Rule { get; }
        public bool DidSplit { get; private set; } = false;

        public SplitRuleTrigger(GameState state, TSplitRule rule)
        {
            State = state;
            Rule = rule;
        }

        public void Update()
        {
            if (State.AreSplitsEnabled)
                return; // We are still in the savegame screen and should not split yet

            if (!DidSplit && ShouldSplit())
            {
                State.TimerModel.Split();
                DidSplit = true;
            }
        }

        public virtual IEnumerable<MemoryWatcher> MemoryWatchers => Enumerable.Empty<MemoryWatcher>();

        public void Dispose() { }
    }
}
