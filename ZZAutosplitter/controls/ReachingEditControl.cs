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
    public partial class ReachingEditControl : SplitRuleEditControl
    {
        private readonly SplitRuleReaching rule;
        private readonly Database database;
        private readonly bool isInitialised;

        public ReachingEditControl(SplitRuleReaching rule, Database database)
        {
            this.rule = rule;
            this.database = database;
            InitializeComponent();
            comboScene.DataSource = database.SceneNames.ToArray();
            comboScene.DisplayMember = nameof(KeyValuePair<SceneId, string>.Value);

            comboScene.SelectedItem = database.SceneNames[rule.Scene];
            isInitialised = true;
            UpdateIcon();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Scene = ((KeyValuePair<SceneId, string>)comboScene.SelectedItem).Key;
            UpdateIcon();
            InvokeRuleChanged();
        }

        private void buttonIcon_Click(object sender, EventArgs e)
        {
            var box = new IconSelectionBox(database, IconSets.None | IconSets.Items | IconSets.Fairies | IconSets.Faces);
            box.SelectedIcon = rule.OverrideIcon;
            if (box.ShowDialog() == DialogResult.OK)
            {
                rule.OverrideIcon = box.SelectedIcon;
                InvokeRuleChanged();
            }
            UpdateIcon();
        }

        private void UpdateIcon() => buttonIcon.BackgroundImage = rule.GetIcon(database);
    }
}
