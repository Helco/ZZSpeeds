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
using System.Xml.Serialization;

namespace ZZAutosplitter
{
    public class ZZAutosplitter : LogicComponent
    {
        public const string ComponentName_ = "ZZAutosplitter";
        public override string ComponentName => ComponentName_;

        public Database Database { get; } = new Database();
        public Settings Settings { get; private set; } = new Settings();
        public LiveSplitState LiveSplitState { get; }

        public ZZAutosplitter(LiveSplitState state)
        {
            this.LiveSplitState = state;
        }

        public override void Dispose()
        {
        }

        public override XmlNode GetSettings(XmlDocument document) => Settings.ToXmlNode(document);
        public override void SetSettings(XmlNode xmlNode) => Settings = Settings.FromXmlNode(xmlNode);
        public override Control GetSettingsControl(LayoutMode _) => new SettingsControl(this);

        public void AddSplitRulesAsSegments()
        {
            var run = LiveSplitState.Run;
            if (run == null)
                return;
            foreach (var rule in Settings.SplitRules)
                run.AddSegment(rule.GetDescription(Database), icon: rule.GetIcon(Database));
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
    }
}
