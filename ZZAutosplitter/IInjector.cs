using System;
using System.Diagnostics;

namespace ZZAutosplitter
{
    internal interface IInjector : IDisposable
    {
        void InjectInto(GameState state);
    }
}
