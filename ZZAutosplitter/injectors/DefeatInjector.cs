using System;
using System.Diagnostics;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter.injectors
{
    internal class DefeatInjector : IInjector
    {
        private IntPtr lastNpcUID, lastFightResult;

        public IntPtr LastNpcUID => lastNpcUID == IntPtr.Zero
            ? throw new InvalidOperationException("DefeatInjector was not initialised")
            : lastNpcUID;

        public IntPtr LastFightResult => lastFightResult == IntPtr.Zero
            ? throw new InvalidOperationException("DefeatInjector was not initialised")
            : lastFightResult;

        public void InjectInto(GameState state)
        {
            var process = state.Process;
            var version = state.Version;

            var totalMemorySize =
                version.LeaveDuelCodeSize +
                sizeof(int) * 2;
            var memBegin = process.AllocateMemory(totalMemorySize);
            lastNpcUID = memBegin + 0;
            lastFightResult = memBegin + 4;
            var codeBegin = memBegin + 8;

            var gate = process.WriteDetour(
                src: new IntPtr(version.OffLeaveDuelStart),
                version.OffLeaveDuelEnd - version.OffLeaveDuelStart,
                dest: codeBegin);
            var code = version.LeaveDuelCode(gate, codeBegin, lastNpcUID, lastFightResult);
            process.WriteBytes(codeBegin, code);
        }

        public void Dispose() { }
    }
}
