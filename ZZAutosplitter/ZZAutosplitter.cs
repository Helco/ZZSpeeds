using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System.Threading;
using ZZAutosplitter.versions;
using LiveSplit.ComponentUtil;

namespace ZZAutosplitter
{
    public class ZZAutosplitter : LogicComponent
    {
        public const string ComponentName_ = "ZZAutosplitter";
        public override string ComponentName => ComponentName_;

        public Database Database { get; } = new Database();
        public Settings Settings { get; private set; } = new Settings();
        public LiveSplitState LiveSplitState { get; }

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly Task updateTask;
        private GameState gameState = null;

        public ZZAutosplitter(LiveSplitState state)
        {
            LiveSplitState = state;
            updateTask = Task.Run(UpdateLoop);
        }

        public override void Dispose()
        {
            cts.Cancel();
            updateTask.Wait(TimeSpan.FromSeconds(3));
            gameState?.Dispose();
        }

        public override XmlNode GetSettings(XmlDocument document) => Settings.ToXmlNode(document);
        public override void SetSettings(XmlNode xmlNode) => Settings = Settings.FromXmlNode(xmlNode);
        public override Control GetSettingsControl(LayoutMode _) => new controls.SettingsControl(this);

        public void AddSplitRulesAsSegments()
        {
            var run = LiveSplitState.Run;
            if (run == null)
                return;
            foreach (var rule in Settings.SplitRules)
                run.AddSegment(rule.GetDescription(Database), icon: rule.GetIcon(Database));
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            if (updateTask.Exception != null)
                throw updateTask.Exception;
        }

        private async Task UpdateLoop()
        {
            Process process = null;
            GameVersion gameVersion = null;
            IntPtr gamePointer = IntPtr.Zero;

            while (!cts.IsCancellationRequested)
            {
                if (process is null or { HasExited: true })
                {
                    gameState?.Dispose();
                    gameState = null;
                    if (!FindProcess(out process, out gameVersion))
                    {
                        await Task.Delay(Settings.DelayProcessScanner);
                        continue;
                    }
                }

                gameState ??= new GameState(Database, LiveSplitState, process, gameVersion, Settings);
                if (gameState.FindGamePointer())
                {
                    gameState.UpdateTriggers();
                    await Task.Delay(Settings.DelayUpdateTriggers);
                }
                else
                    await Task.Delay(Settings.DelayGamePointerScanner);
            }
        }

        private bool FindProcess(out Process process, out GameVersion gameVersion)
        {
            foreach (var p in Process.GetProcesses())
            {
                gameVersion = GameVersion.GetVersion(p);
                if (gameVersion != null && !p.HasExited)
                {
                    process = p;
                    return true;
                }
            }

            process = null;
            gameVersion = null;
            return false;
        }
    }
}
