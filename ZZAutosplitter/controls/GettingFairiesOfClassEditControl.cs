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
    public partial class GettingFairiesOfClassEditControl : SplitRuleEditControl
    {
        private readonly Database database;
        private readonly SplitRuleGettingFairiesOfClass rule;
        private readonly bool isInitialised;

        public GettingFairiesOfClassEditControl(SplitRuleGettingFairiesOfClass rule, Database database)
        {
            this.rule = rule;
            this.database = database;
            InitializeComponent();

            comboElement.DataSource = Enum.GetValues(typeof(ElementType));
            comboElement.SelectedItem = rule.Type;
            numericAmount.Value = rule.Amount;
            isInitialised = true;
            UpdateIcon();
        }

        private void UpdateIcon() => pictureBox.Image = rule.GetIcon(database);

        private void numericAmount_ValueChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Amount = (int)numericAmount.Value;
            InvokeRuleChanged();
        }

        private void comboElement_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Type = (ElementType)comboElement.SelectedItem;
            UpdateIcon();
            InvokeRuleChanged();
        }
    }
}
