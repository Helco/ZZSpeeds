using System;
using System.Collections.Generic;
using System.Linq;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter.triggers
{
    class SplitOnWatching : SplitRuleTrigger<SplitRuleWatching>
    {
        private readonly StringWatcher videoFilename;

        public SplitOnWatching(GameState state, SplitRuleWatching rule) : base(state, rule)
        {
            videoFilename = new StringWatcher(
                state.ResMgrPtr +
                state.Version.OffResMgrToVideoMgr +
                state.Version.OffVideoMgrToFilename,
                ReadStringType.ASCII,
                state.Version.LenVideoMgrFilename);
        }

        public override IEnumerable<MemoryWatcher> MemoryWatchers => new[]
        {
            videoFilename
        };

        protected override bool ShouldSplit() =>
            videoFilename.Current.Equals(State.Database.GetFileNameFor(Rule.Video),
                StringComparison.InvariantCultureIgnoreCase);
    }
}
