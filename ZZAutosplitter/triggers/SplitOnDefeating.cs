using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter.triggers
{
    class SplitOnDefeating : SplitRuleTrigger<SplitRuleDefeating>
    {
        private readonly MemoryWatcher<uint> lastNpcUID;
        private readonly MemoryWatcher<int> lastFightResult;

        public SplitOnDefeating(GameState state, SplitRuleDefeating rule) : base(state, rule)
        {
            var injector = state.GetInjector<injectors.DefeatInjector>();
            lastNpcUID = new(injector.LastNpcUID);
            lastFightResult = new(injector.LastFightResult);
        }

        public override IEnumerable<MemoryWatcher> MemoryWatchers => new MemoryWatcher[]
        {
            lastNpcUID,
            lastFightResult
        };

        protected override bool ShouldSplit() =>
            lastFightResult.Current == 0 && // player won fight
            lastNpcUID.Current == Rule.UID;
    }
}
