using System;
using System.Collections.Generic;
using System.Diagnostics;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter
{
    internal interface ITrigger : IDisposable
    {
        IEnumerable<MemoryWatcher> MemoryWatchers { get; }

        void Update();
    }
}