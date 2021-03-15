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
    public partial class WatchingEditControl : SplitRuleEditControl
    {
        private readonly SplitRuleWatching rule;
        private readonly Database database;
        private readonly bool isInitialised;

        public WatchingEditControl(SplitRuleWatching rule, Database database)
        {
            this.rule = rule;
            this.database = database;
            InitializeComponent();
            comboVideo.DataSource = database.Videos;
            comboVideo.SelectedItem = rule.Video;

            isInitialised = true;
            pictureBox.Image = rule.GetIcon(database);
        }

        private void comboVideo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isInitialised) return;
            rule.Video = (VideoId)comboVideo.SelectedItem;
            InvokeRuleChanged();
        }
    }
}
