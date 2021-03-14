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

        private readonly Database database = new Database();
        private Settings settings = new Settings();

        public ZZAutosplitter(LiveSplitState state)
        {

        }

        public override void Dispose()
        {
        }

        public override XmlNode GetSettings(XmlDocument document) => settings.ToXmlNode(document);
        public override void SetSettings(XmlNode xmlNode) => settings = Settings.FromXmlNode(xmlNode);
        public override Control GetSettingsControl(LayoutMode _) => new SettingsControl(database, settings);


        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
    }
}
