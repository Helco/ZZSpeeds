using System;
using System.Collections.Generic;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter.injectors
{
    internal class IsLoadingInjector : IInjector
    {
        private IntPtr isLoading;
        public IntPtr IsLoading => isLoading == IntPtr.Zero
            ? throw new InvalidOperationException("ToggleIsLoadingInjector was not initialized")
            : isLoading;

        public void InjectInto(GameState state)
        {
            var process = state.Process;
            var version = state.Version;

            var totalMemorySize =
                version.ToggleIsLoadingCodeSize * 2 +
                sizeof(int) * 2;
            var memBegin = process.AllocateMemory(totalMemorySize);
            isLoading = memBegin + 0;
            var startLoading = memBegin + 4;
            var endLoading = memBegin + 4 + version.ToggleIsLoadingCodeSize;

            void InjectToggleLoadScene(int srcStart, int srcEnd, IntPtr dest)
            {
                var gate = process.WriteDetour(
                    src: new IntPtr(srcStart),
                    srcEnd - srcStart,
                    dest);
                var code = version.ToggleIsLoadingCode(gate, dest, isLoading);
                process.WriteBytes(dest, code);
            }
            InjectToggleLoadScene(version.OffLoadSceneEnterStart, version.OffLoadSceneEnterEnd, startLoading);
            InjectToggleLoadScene(version.OffLoadSceneExitStart, version.OffLoadSceneExitEnd, endLoading);

            process.WriteValue(isLoading, 0);
        }

        public void Dispose() { }
    }
}
