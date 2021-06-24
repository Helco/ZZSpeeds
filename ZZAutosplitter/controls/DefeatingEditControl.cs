using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZZAutosplitter.controls
{
    public partial class DefeatingEditControl : SplitRuleEditControl
    {
        private readonly SplitRuleDefeating rule;
        private readonly Database database;
        private readonly bool isInitialised;

        public DefeatingEditControl(SplitRuleDefeating rule, Database database)
        {
            this.rule = rule;
            this.database = database;
            InitializeComponent();
            comboPreset.DataSource = database.NPCPresets;
            comboPreset.DisplayMember = nameof(SplitRuleDefeating.Name);
            textUID.ValidatingType = typeof(DefeatingEditControl);
            textUID.Text = rule.UID.ToString("X8");
            textName.Text = rule.Name;

            isInitialised = true;
            UpdateIcon();
        }

        public static object Parse(string uid) => UIDRegex.IsMatch(uid)
            ? Convert.ToUInt32(uid, 16)
            : null;

        private static readonly Regex UIDRegex = new Regex("^[a-fA-F0-9]{1,8}$", RegexOptions.Compiled);
        private void textUID_Validating(object sender, CancelEventArgs e)
        {
            //if (!UIDRegex.IsMatch(textUID.Text))
                //e.Cancel = true;
        }

        private void textUID_Validated(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            //rule.UID = Convert.ToUInt32(textUID.Text, 16);
        }

        private void textName_TextChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Name = textName.Text;
            InvokeRuleChanged();
        }

        private void buttonFace_Click(object sender, EventArgs e)
        {
            var box = new IconSelectionBox(database, IconSets.Faces | IconSets.Fairies);
            if (box.ShowDialog() == DialogResult.OK)
            {
                rule.Icon = box.SelectedIcon;
                InvokeRuleChanged();
            }
            UpdateIcon();
        }

        private void UpdateIcon() => buttonFace.BackgroundImage = rule.GetIcon(database);

        private void textUID_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            if (!e.IsValidInput || e.ReturnValue == null)
                toolTipUID.Show("Invalid UID", textUID, 0, -20, 5000);
            else if (isInitialised)
            {
                rule.UID = (uint)e.ReturnValue;
                InvokeRuleChanged();
            }
        }

        private void btnApplyPreset_Click(object sender, EventArgs e)
        {
            if (comboPreset.SelectedItem == null)
                return;
            var preset = (SplitRuleDefeating)comboPreset.SelectedItem;
            rule.UID = preset.UID;
            rule.Name = preset.Name;
            rule.Icon = preset.Icon;
            textUID.Text = rule.UID.ToString("X8");
            textName.Text = rule.Name;
            UpdateIcon();
            InvokeRuleChanged();
        }
    }
}
