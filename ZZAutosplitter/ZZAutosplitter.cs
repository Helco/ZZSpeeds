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

        public ZZAutosplitter(LiveSplitState state)
        {
            LiveSplitState = state;
            updateTask = Task.Run(UpdateLoop);
        }

        public override void Dispose()
        {
            cts.Cancel();
            updateTask.Wait(TimeSpan.FromSeconds(3));
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

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }

        private async Task UpdateLoop()
        {
            Process process = null;
            IntPtr gamePointer = IntPtr.Zero;

            while (!cts.IsCancellationRequested)
            {
                if ((process?.HasExited ?? true) && !FindProcess(out process))
                {
                    await Task.Delay(Settings.DelayProcessScanner);
                    continue;
                }
                if (gamePointer == IntPtr.Zero && !FindGamePointer(process, out gamePointer))
                {
                    await Task.Delay(Settings.DelayGamePointerScanner);
                    continue;
                }
                UpdateTriggers(process, gamePointer);
                await Task.Delay(Settings.DelayUpdateTriggers);
            }
        }

        private bool FindProcess(out Process process)
        {
            process = null;
            return false;
        }

        private bool FindGamePointer(Process process, out IntPtr gamePointer)
        {
            gamePointer = IntPtr.Zero;
            return false;
        }

        private void UpdateTriggers(Process process, IntPtr gamePointer)
        {

        }
    }
}
