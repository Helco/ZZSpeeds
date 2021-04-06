using System;
using System.Collections.Generic;
using System.Linq;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter.triggers
{
    class SplitOnReaching : SplitRuleTrigger<SplitRuleReaching>
    {
        private readonly MemoryWatcher<int> sceneId;

        public SplitOnReaching(GameState state, SplitRuleReaching rule) : base(state, rule)
        {
            sceneId = new MemoryWatcher<int>(
                state.ResMgrPtr +
                state.Version.OffResMgrToScene +
                state.Version.OffSceneToDataset +
                state.Version.OffDatasetToDatasetStruct +
                state.Version.OffDatasetStructToSceneId);
        }

        public override IEnumerable<MemoryWatcher> MemoryWatchers => new[]
        {
            sceneId
        };

        protected override bool ShouldSplit() =>
            sceneId.Current == Rule.Scene.index;
    }
}
