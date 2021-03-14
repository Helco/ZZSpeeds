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
        private readonly Database database;
        private readonly Settings settings;

        public SettingsControl(Database database, Settings settings)
        {
            this.database = database;
            this.settings = settings;
            InitializeComponent();
            this.Dock = DockStyle.Fill;

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
            if (index < 0)
                return;
            var rule = settings.SplitRules[index];

            SplitRuleEditControl editControl = rule switch
            {
                SplitRuleGettingCards gc => new GettingCardsEditControl(gc, database),

                _ => null
            };
            if (editControl == null)
                return;

            editControl.Dock = DockStyle.Fill;
            editControl.OnRuleChanged += () => ModifySplitItem(listSplits.Items[index], index, rule);
            panelSplitEdit.Controls.Add(editControl);
        }

        private void menuAddGettingCards_Click(object sender, EventArgs e) => AddNewRule(new SplitRuleGettingCards());

        private void AddNewRule(SplitRule rule)
        {
            settings.SplitRules.Add(rule);
            if (listSplits.Items.Count == listSplits.SmallImageList.Images.Count)
                listSplits.SmallImageList.Images.Add(SystemIcons.Question.ToBitmap());

            var listItem = listSplits.Items.Add(new ListViewItem());
            ModifySplitItem(listItem, listItem.Index, rule);
            listSplits.SelectedIndices.Clear();
            listSplits.SelectedIndices.Add(listItem.Index);
        }

        private void listSplits_ItemChecked(object sender, ItemCheckedEventArgs e) =>
            settings.SplitRules[e.Item.Index].Enabled = e.Item.Checked;
    }
}
