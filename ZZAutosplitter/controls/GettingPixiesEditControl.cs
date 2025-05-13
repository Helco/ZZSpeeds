using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZZAutosplitter.controls
{
    public partial class GettingPixiesEditControl : SplitRuleEditControl
    {
        private readonly Database database;
        private readonly SplitRulePixies rule;
        private readonly bool isInitialised;

        public GettingPixiesEditControl(SplitRulePixies rule, Database database)
        {
            this.rule = rule;
            this.database = database;
            InitializeComponent();

            comboElement.SelectedIndex = rule.Exactly ? 0 : 1;
            numericAmount.Value = rule.Amount;
            isInitialised = true;
            UpdateIcon();
        }

        private void UpdateIcon() => buttonIcon.BackgroundImage = rule.GetIcon(database);

        private void numericAmount_ValueChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Amount = (int)numericAmount.Value;
            InvokeRuleChanged();
        }

        private void comboElement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Exactly = comboElement.SelectedIndex == 0;
            InvokeRuleChanged();
        }

        private void buttonIcon_Click(object sender, EventArgs e)
        {
            var box = new IconSelectionBox(database, IconSets.None | IconSets.Items | IconSets.Fairies | IconSets.Faces);
            box.SelectedIcon = rule.OverrideIcon ?? database.PixieIconId;
            if (box.ShowDialog() == DialogResult.OK)
            {
                rule.OverrideIcon = box.SelectedIcon;
                InvokeRuleChanged();
            }
            UpdateIcon();
        }
    }
}
