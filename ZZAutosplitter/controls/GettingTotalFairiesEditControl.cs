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
    public partial class GettingTotalFairiesEditControl : SplitRuleEditControl
    {
        private readonly Database database;
        private readonly SplitRuleGettingTotalFairies rule;
        private readonly bool isInitialised;

        public GettingTotalFairiesEditControl(SplitRuleGettingTotalFairies rule, Database database)
        {
            this.rule = rule;
            this.database = database;
            InitializeComponent();

            numericAmount.Value = rule.Amount;
            pictureBox.Image = rule.GetIcon(database);
            isInitialised = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Amount = (int)numericAmount.Value;
            InvokeRuleChanged();
        }
    }
}
