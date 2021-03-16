using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZZAutosplitter.controls;

namespace ZZAutosplitter
{
    partial class SettingsControl : UserControl
    {
        private readonly ZZAutosplitter autosplitter;
        private readonly Database database;
        private readonly Settings settings;

        public SettingsControl(ZZAutosplitter autosplitter)
        {
            this.autosplitter = autosplitter;
            database = autosplitter.Database;
            settings = autosplitter.Settings;
            InitializeComponent();
            Dock = DockStyle.Fill;

            checkBoxAutoSplits.DataBindings.Add(nameof(CheckBox.Checked), settings, nameof(settings.EnableAutoSplits), false, DataSourceUpdateMode.OnPropertyChanged);
            checkBoxAutoStart.DataBindings.Add(nameof(CheckBox.Checked), settings, nameof(settings.EnableAutoStart), false, DataSourceUpdateMode.OnPropertyChanged);
            checkBoxLoadTime.DataBindings.Add(nameof(CheckBox.Checked), settings, nameof(settings.EnableLoadTimeRemoval), false, DataSourceUpdateMode.OnPropertyChanged);

            var iconSize = database.GetIconFor(ElementType.Nature).Size; // faces are larger
            listSplits.SmallImageList = new ImageList();
            listSplits.SmallImageList.ImageSize = iconSize;
            listSplits.SmallImageList.Images.AddRange(Enumerable
                .Repeat(SystemIcons.Question.ToBitmap(), settings.SplitRules.Count)
                .ToArray());
            listSplits.Items.AddRange(Enumerable
                .Range(0, settings.SplitRules.Count)
                .Select(i => new ListViewItem())
                .Select((itm, i) => ModifySplitItem(itm, i, settings.SplitRules[i]))
                .ToArray());
            listSplits.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listSplits_SelectedIndexChanged(null, null);
        }

        private ListViewItem ModifySplitItem(ListViewItem item, int index, SplitRule rule)
        {
            item ??= new();
            item.Checked = rule.Enabled;
            item.Text = rule.GetDescription(database);
            item.ImageIndex = index;

            var icon = rule.GetIcon(database) ?? item.ImageList.Images[index];
            if (icon.Size != listSplits.SmallImageList.ImageSize)
                icon = new Bitmap(icon, listSplits.SmallImageList.ImageSize);
            listSplits.SmallImageList.Images[index] = icon;
            listSplits.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            return item;
        }

        private void splitBtnAddSplit_ButtonClick(object sender, EventArgs e) => splitBtnAddSplit.ShowDropDown();

        private void listSplits_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelSplitEdit.Controls.Clear();
            var index = listSplits.SelectedIndices.Count == 0 ? -1 : listSplits.SelectedIndices[0];
            var rule = index < 0 ? null : settings.SplitRules[index];

            SplitRuleEditControl editControl = rule switch
            {
                null => new EmptySplitRuleControl(autosplitter),
                SplitRuleGettingCards gc => new GettingCardsEditControl(gc, database),
                SplitRuleGettingFairiesOfClass gfoc => new GettingFairiesOfClassEditControl(gfoc, database),
                SplitRuleGettingTotalFairies gtf => new GettingTotalFairiesEditControl(gtf, database),
                SplitRuleReaching r => new ReachingEditControl(r, database),
                SplitRuleDefeating d => new DefeatingEditControl(d, database),
                SplitRuleWatching w => new WatchingEditControl(w, database),
                _ => throw new NotImplementedException($"Unimplemented edit control for {rule}")
            };
            if (editControl == null)
                return;

            editControl.Dock = DockStyle.Fill;
            if (index >= 0)
                editControl.OnRuleChanged += () => ModifySplitItem(listSplits.Items[index], index, rule);
            panelSplitEdit.Controls.Add(editControl);
        }

        private void menuAddGettingCards_Click(object sender, EventArgs e) => AddNewRule(new SplitRuleGettingCards());
        private void menuAddGettingFairiesOfClass_Click(object sender, EventArgs e) => AddNewRule(new SplitRuleGettingFairiesOfClass());
        private void menuAddGettingTotalFairies_Click(object sender, EventArgs e) => AddNewRule(new SplitRuleGettingTotalFairies());
        private void menuAddReaching_Click(object sender, EventArgs e) => AddNewRule(new SplitRuleReaching());
        private void menuAddDefeating_Click(object sender, EventArgs e) => AddNewRule(new SplitRuleDefeating());
        private void menuAddWatching_Click(object sender, EventArgs e) => AddNewRule(new SplitRuleWatching());

        private void AddNewRule(SplitRule rule)
        {
            settings.SplitRules.Add(rule);
            if (listSplits.Items.Count == listSplits.SmallImageList.Images.Count)
                listSplits.SmallImageList.Images.Add(SystemIcons.Question.ToBitmap());

            var listItem = listSplits.Items.Add(new ListViewItem());
            listItem.Checked = true;
            ModifySplitItem(listItem, listItem.Index, rule);
            listSplits.SelectedIndices.Clear();
            listSplits.SelectedIndices.Add(listItem.Index);
        }

        private void listSplits_ItemChecked(object sender, ItemCheckedEventArgs e) =>
            settings.SplitRules[e.Item.Index].Enabled = e.Item.Checked;

        private void btnMoveSplitUp_Click(object sender, EventArgs e) => MoveSplit(-1);
        private void btnMoveSplitDown_Click(object sender, EventArgs e) => MoveSplit(+1);

        private void MoveSplit(int delta)
        {
            if (delta == 0 || Math.Abs(delta) > 1)
                throw new ArgumentOutOfRangeException();
            if (listSplits.SelectedIndices.Count == 0)
                return;
            int from = listSplits.SelectedIndices[0];
            int to = from + delta;
            if (to < 0 || to >= listSplits.Items.Count)
                return;

            var tmp = settings.SplitRules[from];
            settings.SplitRules[from] = settings.SplitRules[to];
            settings.SplitRules[to] = tmp;
            ModifySplitItem(listSplits.Items[from], from, settings.SplitRules[from]);
            ModifySplitItem(listSplits.Items[to], to, settings.SplitRules[to]);
            listSplits.SelectedIndices.Clear();
            listSplits.SelectedIndices.Add(to);
        }

        private void btnDeleteSplit_Click(object sender, EventArgs e)
        {
            if (listSplits.SelectedIndices.Count == 0)
                return;

            var index = listSplits.SelectedIndices[0];
            settings.SplitRules.RemoveAt(index);
            listSplits.Items.RemoveAt(index);
        }

        private void btnClearSplits_Click(object sender, EventArgs e)
        {
            if (listSplits.Items.Count == 0)
                return;
            var result = MessageBox.Show(
                "Do you really want to delete all split rules?", "Confirm clearing",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                settings.SplitRules.Clear();
                listSplits.Items.Clear();
                listSplits.SelectedIndices.Clear();
                panelSplitEdit.Controls.Clear();
            }
        }
    }
}
