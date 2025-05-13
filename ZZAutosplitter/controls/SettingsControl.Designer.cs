
namespace ZZAutosplitter.controls
{
    partial class SettingsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.listSplits = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.panelSplitEdit = new System.Windows.Forms.Panel();
            this.toolStripSplits = new System.Windows.Forms.ToolStrip();
            this.splitBtnAddSplit = new System.Windows.Forms.ToolStripSplitButton();
            this.menuAddGettingCards = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddGettingFairiesOfClass = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddGettingTotalFairies = new System.Windows.Forms.ToolStripMenuItem();
            this.gettingPixiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddReaching = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddDefeating = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddWatching = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMoveSplitUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveSplitDown = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteSplit = new System.Windows.Forms.ToolStripButton();
            this.btnClearSplits = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numericDelayTriggers = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericDelayGamePtr = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numericDelayProcess = new System.Windows.Forms.NumericUpDown();
            this.checkBoxLoadTime = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoSplits = new System.Windows.Forms.CheckBox();
            this.toolTipDelays = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.RightToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStripSplits.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDelayTriggers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDelayGamePtr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDelayProcess)).BeginInit();
            this.SuspendLayout();
            // 
            // listSplits
            // 
            this.listSplits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listSplits.CheckBoxes = true;
            this.listSplits.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listSplits.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listSplits.HideSelection = false;
            this.listSplits.Location = new System.Drawing.Point(2, 2);
            this.listSplits.Margin = new System.Windows.Forms.Padding(2);
            this.listSplits.Name = "listSplits";
            this.listSplits.Size = new System.Drawing.Size(401, 299);
            this.listSplits.TabIndex = 1;
            this.listSplits.UseCompatibleStateImageBehavior = false;
            this.listSplits.View = System.Windows.Forms.View.Details;
            this.listSplits.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listSplits_ItemChecked);
            this.listSplits.SelectedIndexChanged += new System.EventHandler(this.listSplits_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 10176;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.toolStripContainer1);
            this.groupBox1.Location = new System.Drawing.Point(10, 94);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(447, 405);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Split Rules";
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.listSplits);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panelSplitEdit);
            this.toolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(403, 388);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(2, 15);
            this.toolStripContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            this.toolStripContainer1.RightToolStripPanel.Controls.Add(this.toolStripSplits);
            this.toolStripContainer1.Size = new System.Drawing.Size(443, 388);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // panelSplitEdit
            // 
            this.panelSplitEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSplitEdit.Location = new System.Drawing.Point(2, 304);
            this.panelSplitEdit.Margin = new System.Windows.Forms.Padding(2);
            this.panelSplitEdit.Name = "panelSplitEdit";
            this.panelSplitEdit.Size = new System.Drawing.Size(399, 84);
            this.panelSplitEdit.TabIndex = 3;
            // 
            // toolStripSplits
            // 
            this.toolStripSplits.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripSplits.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSplits.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripSplits.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.splitBtnAddSplit,
            this.btnMoveSplitUp,
            this.btnMoveSplitDown,
            this.btnDeleteSplit,
            this.btnClearSplits});
            this.toolStripSplits.Location = new System.Drawing.Point(0, 4);
            this.toolStripSplits.Name = "toolStripSplits";
            this.toolStripSplits.Size = new System.Drawing.Size(40, 181);
            this.toolStripSplits.TabIndex = 2;
            this.toolStripSplits.Text = "toolStrip1";
            // 
            // splitBtnAddSplit
            // 
            this.splitBtnAddSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.splitBtnAddSplit.DropDownButtonWidth = 0;
            this.splitBtnAddSplit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAddGettingCards,
            this.menuAddGettingFairiesOfClass,
            this.menuAddGettingTotalFairies,
            this.gettingPixiesToolStripMenuItem,
            this.menuAddReaching,
            this.menuAddDefeating,
            this.menuAddWatching});
            this.splitBtnAddSplit.Image = global::ZZAutosplitter.Properties.Resources.add;
            this.splitBtnAddSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.splitBtnAddSplit.Name = "splitBtnAddSplit";
            this.splitBtnAddSplit.Size = new System.Drawing.Size(38, 28);
            this.splitBtnAddSplit.Text = "Add split";
            this.splitBtnAddSplit.ButtonClick += new System.EventHandler(this.splitBtnAddSplit_ButtonClick);
            // 
            // menuAddGettingCards
            // 
            this.menuAddGettingCards.Name = "menuAddGettingCards";
            this.menuAddGettingCards.Size = new System.Drawing.Size(238, 26);
            this.menuAddGettingCards.Text = "Getting card(s)";
            this.menuAddGettingCards.Click += new System.EventHandler(this.menuAddGettingCards_Click);
            // 
            // menuAddGettingFairiesOfClass
            // 
            this.menuAddGettingFairiesOfClass.Name = "menuAddGettingFairiesOfClass";
            this.menuAddGettingFairiesOfClass.Size = new System.Drawing.Size(238, 26);
            this.menuAddGettingFairiesOfClass.Text = "Getting fairies of class";
            this.menuAddGettingFairiesOfClass.Click += new System.EventHandler(this.menuAddGettingFairiesOfClass_Click);
            // 
            // menuAddGettingTotalFairies
            // 
            this.menuAddGettingTotalFairies.Name = "menuAddGettingTotalFairies";
            this.menuAddGettingTotalFairies.Size = new System.Drawing.Size(238, 26);
            this.menuAddGettingTotalFairies.Text = "Getting total fairies";
            this.menuAddGettingTotalFairies.Click += new System.EventHandler(this.menuAddGettingTotalFairies_Click);
            // 
            // gettingPixiesToolStripMenuItem
            // 
            this.gettingPixiesToolStripMenuItem.Name = "gettingPixiesToolStripMenuItem";
            this.gettingPixiesToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.gettingPixiesToolStripMenuItem.Text = "Getting pixies";
            this.gettingPixiesToolStripMenuItem.Click += new System.EventHandler(this.menuAddGettingPixies_Click);
            // 
            // menuAddReaching
            // 
            this.menuAddReaching.Name = "menuAddReaching";
            this.menuAddReaching.Size = new System.Drawing.Size(238, 26);
            this.menuAddReaching.Text = "Reaching";
            this.menuAddReaching.Click += new System.EventHandler(this.menuAddReaching_Click);
            // 
            // menuAddDefeating
            // 
            this.menuAddDefeating.Name = "menuAddDefeating";
            this.menuAddDefeating.Size = new System.Drawing.Size(238, 26);
            this.menuAddDefeating.Text = "Defeating";
            this.menuAddDefeating.Click += new System.EventHandler(this.menuAddDefeating_Click);
            // 
            // menuAddWatching
            // 
            this.menuAddWatching.Name = "menuAddWatching";
            this.menuAddWatching.Size = new System.Drawing.Size(238, 26);
            this.menuAddWatching.Text = "Watching";
            this.menuAddWatching.Click += new System.EventHandler(this.menuAddWatching_Click);
            // 
            // btnMoveSplitUp
            // 
            this.btnMoveSplitUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveSplitUp.Image = global::ZZAutosplitter.Properties.Resources.arrow_up;
            this.btnMoveSplitUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveSplitUp.Name = "btnMoveSplitUp";
            this.btnMoveSplitUp.Size = new System.Drawing.Size(38, 28);
            this.btnMoveSplitUp.Text = "Move up";
            this.btnMoveSplitUp.Click += new System.EventHandler(this.btnMoveSplitUp_Click);
            // 
            // btnMoveSplitDown
            // 
            this.btnMoveSplitDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveSplitDown.Image = global::ZZAutosplitter.Properties.Resources.arrow_down;
            this.btnMoveSplitDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveSplitDown.Name = "btnMoveSplitDown";
            this.btnMoveSplitDown.Size = new System.Drawing.Size(38, 28);
            this.btnMoveSplitDown.Text = "Move down";
            this.btnMoveSplitDown.Click += new System.EventHandler(this.btnMoveSplitDown_Click);
            // 
            // btnDeleteSplit
            // 
            this.btnDeleteSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteSplit.Image = global::ZZAutosplitter.Properties.Resources.delete;
            this.btnDeleteSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteSplit.Name = "btnDeleteSplit";
            this.btnDeleteSplit.Size = new System.Drawing.Size(38, 28);
            this.btnDeleteSplit.Text = "Delete split";
            this.btnDeleteSplit.Click += new System.EventHandler(this.btnDeleteSplit_Click);
            // 
            // btnClearSplits
            // 
            this.btnClearSplits.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearSplits.Image = global::ZZAutosplitter.Properties.Resources.bin_closed;
            this.btnClearSplits.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearSplits.Name = "btnClearSplits";
            this.btnClearSplits.Size = new System.Drawing.Size(38, 28);
            this.btnClearSplits.Text = "Clear all splits";
            this.btnClearSplits.Click += new System.EventHandler(this.btnClearSplits_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numericDelayTriggers);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numericDelayGamePtr);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.numericDelayProcess);
            this.groupBox2.Controls.Add(this.checkBoxLoadTime);
            this.groupBox2.Controls.Add(this.checkBoxAutoStart);
            this.groupBox2.Controls.Add(this.checkBoxAutoSplits);
            this.groupBox2.Location = new System.Drawing.Point(10, 2);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(447, 88);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Basic Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(306, 56);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Delay in-game scanner (ms)";
            // 
            // numericDelayTriggers
            // 
            this.numericDelayTriggers.Location = new System.Drawing.Point(247, 55);
            this.numericDelayTriggers.Margin = new System.Windows.Forms.Padding(2);
            this.numericDelayTriggers.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericDelayTriggers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericDelayTriggers.Name = "numericDelayTriggers";
            this.numericDelayTriggers.Size = new System.Drawing.Size(55, 20);
            this.numericDelayTriggers.TabIndex = 7;
            this.toolTipDelays.SetToolTip(this.numericDelayTriggers, "Time between two in-game checks.\r\nLower values might result in slightly more accu" +
        "rate timings but will result in higher CPU usage.");
            this.numericDelayTriggers.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(306, 36);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Delay initial scanner (ms)";
            // 
            // numericDelayGamePtr
            // 
            this.numericDelayGamePtr.Location = new System.Drawing.Point(247, 35);
            this.numericDelayGamePtr.Margin = new System.Windows.Forms.Padding(2);
            this.numericDelayGamePtr.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericDelayGamePtr.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericDelayGamePtr.Name = "numericDelayGamePtr";
            this.numericDelayGamePtr.Size = new System.Drawing.Size(55, 20);
            this.numericDelayGamePtr.TabIndex = 5;
            this.toolTipDelays.SetToolTip(this.numericDelayGamePtr, "Time between two initial game scans.\r\nLower only if the autosplitter does not rea" +
        "ct fast enough after resetting the game!");
            this.numericDelayGamePtr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(306, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(159, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Delay process scanner (ms)";
            // 
            // numericDelayProcess
            // 
            this.numericDelayProcess.Location = new System.Drawing.Point(247, 16);
            this.numericDelayProcess.Margin = new System.Windows.Forms.Padding(2);
            this.numericDelayProcess.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericDelayProcess.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericDelayProcess.Name = "numericDelayProcess";
            this.numericDelayProcess.Size = new System.Drawing.Size(55, 20);
            this.numericDelayProcess.TabIndex = 3;
            this.toolTipDelays.SetToolTip(this.numericDelayProcess, "Time between two process scans.\r\nLower only if the Autosplitter does not react fa" +
        "st enough after resetting the game!");
            this.numericDelayProcess.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkBoxLoadTime
            // 
            this.checkBoxLoadTime.AutoSize = true;
            this.checkBoxLoadTime.Location = new System.Drawing.Point(4, 55);
            this.checkBoxLoadTime.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxLoadTime.Name = "checkBoxLoadTime";
            this.checkBoxLoadTime.Size = new System.Drawing.Size(141, 19);
            this.checkBoxLoadTime.TabIndex = 2;
            this.checkBoxLoadTime.Text = "Load-Time Removal";
            this.checkBoxLoadTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(4, 36);
            this.checkBoxAutoStart.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(111, 19);
            this.checkBoxAutoStart.TabIndex = 1;
            this.checkBoxAutoStart.Text = "Automatic Start";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoSplits
            // 
            this.checkBoxAutoSplits.AutoSize = true;
            this.checkBoxAutoSplits.Location = new System.Drawing.Point(4, 16);
            this.checkBoxAutoSplits.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxAutoSplits.Name = "checkBoxAutoSplits";
            this.checkBoxAutoSplits.Size = new System.Drawing.Size(116, 19);
            this.checkBoxAutoSplits.TabIndex = 0;
            this.checkBoxAutoSplits.Text = "Automatic Splits";
            this.checkBoxAutoSplits.UseVisualStyleBackColor = true;
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(467, 507);
            this.groupBox1.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.RightToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.RightToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStripSplits.ResumeLayout(false);
            this.toolStripSplits.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericDelayTriggers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDelayGamePtr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericDelayProcess)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listSplits;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStripSplits;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxAutoStart;
        private System.Windows.Forms.CheckBox checkBoxAutoSplits;
        private System.Windows.Forms.CheckBox checkBoxLoadTime;
        private System.Windows.Forms.ToolStripButton btnMoveSplitUp;
        private System.Windows.Forms.ToolStripButton btnMoveSplitDown;
        private System.Windows.Forms.ToolStripButton btnDeleteSplit;
        private System.Windows.Forms.ToolStripSplitButton splitBtnAddSplit;
        private System.Windows.Forms.ToolStripMenuItem menuAddGettingCards;
        private System.Windows.Forms.ToolStripMenuItem menuAddReaching;
        private System.Windows.Forms.ToolStripMenuItem menuAddDefeating;
        private System.Windows.Forms.ToolStripMenuItem menuAddWatching;
        private System.Windows.Forms.ToolStripButton btnClearSplits;
        private System.Windows.Forms.ToolStripMenuItem menuAddGettingFairiesOfClass;
        private System.Windows.Forms.ToolStripMenuItem menuAddGettingTotalFairies;
        private System.Windows.Forms.Panel panelSplitEdit;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericDelayTriggers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericDelayGamePtr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericDelayProcess;
        private System.Windows.Forms.ToolTip toolTipDelays;
        private System.Windows.Forms.ToolStripMenuItem gettingPixiesToolStripMenuItem;
    }
}
