using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            checkBoxAutoSplits.DataBindings.Add(nameof(CheckBox.Checked), settings, nameof(settings.EnableAutoSplits), false, DataSourceUpdateMode.OnPropertyChanged);
            checkBoxAutoStart.DataBindings.Add(nameof(CheckBox.Checked), settings, nameof(settings.EnableAutoStart), false, DataSourceUpdateMode.OnPropertyChanged);
            checkBoxLoadTime.DataBindings.Add(nameof(CheckBox.Checked), settings, nameof(settings.EnableLoadTimeRemoval), false, DataSourceUpdateMode.OnPropertyChanged);

            listSplits.LargeImageList ??= new ImageList();
            listSplits.LargeImageList.Images.AddRange(Enumerable
                .Repeat(SystemIcons.Question.ToBitmap(), settings.SplitRules.Count)
                .ToArray());
            listSplits.Items.AddRange(Enumerable
                .Range(0, settings.SplitRules.Count)
                .Select(i => new ListViewItem())
                .Select((itm, i) => ModifySplitItem(itm, i, settings.SplitRules[i]))
                .ToArray());
        }

        private ListViewItem ModifySplitItem(ListViewItem item, int index, SplitRule rule)
        {
            item ??= new();
            item.Checked = rule.Enabled;
            item.Text = rule.GetDescription(database);
            item.ImageList.Images[index] = rule.GetIcon(database) ?? item.ImageList.Images[index];
            return item;
        }
    }
}
