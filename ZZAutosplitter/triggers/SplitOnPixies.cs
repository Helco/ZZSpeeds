using System;
using System.Collections.Generic;
using System.Linq;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter.triggers
{
    internal class SplitOnPixies : SplitRuleTrigger<SplitRulePixies>
    {
        private readonly MemoryWatcher<int> pixieCount;
        
        public SplitOnPixies(GameState state, SplitRulePixies rule) : base(state, rule)
        {
            pixieCount = new MemoryWatcher<int>(
                state.PlayerPtr +
                state.Version.OffPlayerToPixieCount);
        }

        public override IEnumerable<MemoryWatcher> MemoryWatchers => new[]
        {
            pixieCount
        };

        protected override bool ShouldSplit() =>
            (Rule.Exactly && pixieCount.Current == Rule.Amount) ||
            (!Rule.Exactly && pixieCount.Current >= Rule.Amount);
    }
}
