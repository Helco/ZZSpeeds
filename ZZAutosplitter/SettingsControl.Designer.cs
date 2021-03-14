
namespace ZZAutosplitter
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
            this.menuAddReaching = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddDefeating = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddWatching = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEditSplit = new System.Windows.Forms.ToolStripButton();
            this.btnMoveSplitUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveSplitDown = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteSplit = new System.Windows.Forms.ToolStripButton();
            this.btnClearSplits = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxLoadTime = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoSplits = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.RightToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStripSplits.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.listSplits.Location = new System.Drawing.Point(3, 3);
            this.listSplits.Name = "listSplits";
            this.listSplits.Size = new System.Drawing.Size(623, 460);
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
            this.groupBox1.Location = new System.Drawing.Point(15, 145);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(671, 623);
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(629, 598);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(3, 22);
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            this.toolStripContainer1.RightToolStripPanel.Controls.Add(this.toolStripSplits);
            this.toolStripContainer1.Size = new System.Drawing.Size(665, 598);
            this.toolStripContainer1.TabIndex = 4;
            this.toolStripContainer1.Text = "toolStripContainer1";
            this.toolStripContainer1.TopToolStripPanelVisible = false;
            // 
            // panelSplitEdit
            // 
            this.panelSplitEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSplitEdit.Location = new System.Drawing.Point(3, 469);
            this.panelSplitEdit.Name = "panelSplitEdit";
            this.panelSplitEdit.Size = new System.Drawing.Size(623, 129);
            this.panelSplitEdit.TabIndex = 3;
            // 
            // toolStripSplits
            // 
            this.toolStripSplits.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripSplits.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSplits.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripSplits.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.splitBtnAddSplit,
            this.btnEditSplit,
            this.btnMoveSplitUp,
            this.btnMoveSplitDown,
            this.btnDeleteSplit,
            this.btnClearSplits});
            this.toolStripSplits.Location = new System.Drawing.Point(0, 4);
            this.toolStripSplits.Name = "toolStripSplits";
            this.toolStripSplits.Size = new System.Drawing.Size(36, 200);
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
            this.menuAddReaching,
            this.menuAddDefeating,
            this.menuAddWatching});
            this.splitBtnAddSplit.Image = global::ZZAutosplitter.Properties.Resources.add;
            this.splitBtnAddSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.splitBtnAddSplit.Name = "splitBtnAddSplit";
            this.splitBtnAddSplit.Size = new System.Drawing.Size(33, 28);
            this.splitBtnAddSplit.Text = "Add split";
            this.splitBtnAddSplit.ButtonClick += new System.EventHandler(this.splitBtnAddSplit_ButtonClick);
            // 
            // menuAddGettingCards
            // 
            this.menuAddGettingCards.Name = "menuAddGettingCards";
            this.menuAddGettingCards.Size = new System.Drawing.Size(287, 34);
            this.menuAddGettingCards.Text = "Getting card(s)";
            this.menuAddGettingCards.Click += new System.EventHandler(this.menuAddGettingCards_Click);
            // 
            // menuAddGettingFairiesOfClass
            // 
            this.menuAddGettingFairiesOfClass.Name = "menuAddGettingFairiesOfClass";
            this.menuAddGettingFairiesOfClass.Size = new System.Drawing.Size(287, 34);
            this.menuAddGettingFairiesOfClass.Text = "Getting fairies of class";
            // 
            // menuAddGettingTotalFairies
            // 
            this.menuAddGettingTotalFairies.Name = "menuAddGettingTotalFairies";
            this.menuAddGettingTotalFairies.Size = new System.Drawing.Size(287, 34);
            this.menuAddGettingTotalFairies.Text = "Getting total fairies";
            // 
            // menuAddReaching
            // 
            this.menuAddReaching.Name = "menuAddReaching";
            this.menuAddReaching.Size = new System.Drawing.Size(287, 34);
            this.menuAddReaching.Text = "Reaching";
            // 
            // menuAddDefeating
            // 
            this.menuAddDefeating.Name = "menuAddDefeating";
            this.menuAddDefeating.Size = new System.Drawing.Size(287, 34);
            this.menuAddDefeating.Text = "Defeating";
            // 
            // menuAddWatching
            // 
            this.menuAddWatching.Name = "menuAddWatching";
            this.menuAddWatching.Size = new System.Drawing.Size(287, 34);
            this.menuAddWatching.Text = "Watching";
            // 
            // btnEditSplit
            // 
            this.btnEditSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnEditSplit.Image = global::ZZAutosplitter.Properties.Resources.pencil;
            this.btnEditSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditSplit.Name = "btnEditSplit";
            this.btnEditSplit.Size = new System.Drawing.Size(33, 28);
            this.btnEditSplit.Text = "Edit split";
            // 
            // btnMoveSplitUp
            // 
            this.btnMoveSplitUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveSplitUp.Image = global::ZZAutosplitter.Properties.Resources.arrow_up;
            this.btnMoveSplitUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveSplitUp.Name = "btnMoveSplitUp";
            this.btnMoveSplitUp.Size = new System.Drawing.Size(33, 28);
            this.btnMoveSplitUp.Text = "Move up";
            // 
            // btnMoveSplitDown
            // 
            this.btnMoveSplitDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveSplitDown.Image = global::ZZAutosplitter.Properties.Resources.arrow_down;
            this.btnMoveSplitDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveSplitDown.Name = "btnMoveSplitDown";
            this.btnMoveSplitDown.Size = new System.Drawing.Size(33, 28);
            this.btnMoveSplitDown.Text = "Move down";
            // 
            // btnDeleteSplit
            // 
            this.btnDeleteSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteSplit.Image = global::ZZAutosplitter.Properties.Resources.delete;
            this.btnDeleteSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteSplit.Name = "btnDeleteSplit";
            this.btnDeleteSplit.Size = new System.Drawing.Size(33, 28);
            this.btnDeleteSplit.Text = "Delete split";
            // 
            // btnClearSplits
            // 
            this.btnClearSplits.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearSplits.Image = global::ZZAutosplitter.Properties.Resources.bin_closed;
            this.btnClearSplits.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearSplits.Name = "btnClearSplits";
            this.btnClearSplits.Size = new System.Drawing.Size(33, 28);
            this.btnClearSplits.Text = "Clear all splits";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBoxLoadTime);
            this.groupBox2.Controls.Add(this.checkBoxAutoStart);
            this.groupBox2.Controls.Add(this.checkBoxAutoSplits);
            this.groupBox2.Location = new System.Drawing.Point(15, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(670, 136);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Basic Settings";
            // 
            // checkBoxLoadTime
            // 
            this.checkBoxLoadTime.AutoSize = true;
            this.checkBoxLoadTime.Location = new System.Drawing.Point(6, 85);
            this.checkBoxLoadTime.Name = "checkBoxLoadTime";
            this.checkBoxLoadTime.Size = new System.Drawing.Size(176, 24);
            this.checkBoxLoadTime.TabIndex = 2;
            this.checkBoxLoadTime.Text = "Load-Time Removal";
            this.checkBoxLoadTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.AutoSize = true;
            this.checkBoxAutoStart.Location = new System.Drawing.Point(6, 55);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(146, 24);
            this.checkBoxAutoStart.TabIndex = 1;
            this.checkBoxAutoStart.Text = "Automatic Start";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoSplits
            // 
            this.checkBoxAutoSplits.AutoSize = true;
            this.checkBoxAutoSplits.Location = new System.Drawing.Point(6, 25);
            this.checkBoxAutoSplits.Name = "checkBoxAutoSplits";
            this.checkBoxAutoSplits.Size = new System.Drawing.Size(150, 24);
            this.checkBoxAutoSplits.TabIndex = 0;
            this.checkBoxAutoSplits.Text = "Automatic Splits";
            this.checkBoxAutoSplits.UseVisualStyleBackColor = true;
            // 
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(700, 780);
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
        private System.Windows.Forms.ToolStripButton btnEditSplit;
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
    }
}
