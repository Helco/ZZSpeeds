using System;
using System.Collections.Generic;
using LiveSplit.ComponentUtil;
using LiveSplit.Model;

namespace ZZAutosplitter.triggers
{
    internal class LoadRemover : ITrigger
    {
        private readonly MemoryWatcher<int> isLoading;
        private readonly LiveSplitState liveSplitState;
        public IEnumerable<MemoryWatcher> MemoryWatchers => new[] { isLoading };

        public LoadRemover(GameState state)
        {
            var injector = state.GetInjector<injectors.IsLoadingInjector>();
            isLoading = new(injector.IsLoading);
            liveSplitState = state.LiveSplitState;
        }

        public void Update() =>
            liveSplitState.IsGameTimePaused = isLoading.Current != 0;

        public void Dispose() { }
    }
}
