using System;
using System.Windows.Forms;

namespace ZZAutosplitter.controls
{
    public partial class EmptySplitRuleControl : SplitRuleEditControl
    {
        private readonly ZZAutosplitter autosplitter;

        public EmptySplitRuleControl(ZZAutosplitter autosplitter)
        {
            this.autosplitter = autosplitter;
            InitializeComponent();
        }

        private void btnAddSplitRules_Click(object sender, EventArgs e)
        {
            autosplitter.AddSplitRulesAsSegments();
            MessageBox.Show($"Added {autosplitter.Settings.SplitRules.Count} splits to your run", "Zanzarah Autosplitter");
        }
    }
}
