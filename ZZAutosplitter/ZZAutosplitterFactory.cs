using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.Model;
using LiveSplit.UI.Components;

namespace ZZAutosplitter
{
    public class ZZAutosplitterFactory : IComponentFactory
    {
        public string ComponentName => ZZAutosplitter.ComponentName_;

        public string Description => "Autosplitter for Zanzarah";

        public ComponentCategory Category => ComponentCategory.Control;

        public string UpdateName => ComponentName;

        public string UpdateURL => "https://raw.githubusercontent.com/Helco/ZZSpeeds/release/";

        public string XMLURL => UpdateURL + "ZZAutosplitter.update.xml";

        public Version Version => typeof(ZZAutosplitter).Assembly.GetName().Version;

        public IComponent Create(LiveSplitState state) => new ZZAutosplitter(state);
    }
}
