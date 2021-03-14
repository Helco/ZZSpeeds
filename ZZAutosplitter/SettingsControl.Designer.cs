
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolStripSplits = new System.Windows.Forms.ToolStrip();
            this.splitBtnAddSplit = new System.Windows.Forms.ToolStripSplitButton();
            this.menuAddGettingCards = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuAddGettingFairiesOfClass = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAddGettingTotalFairies = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
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
            this.listSplits.HideSelection = false;
            this.listSplits.Location = new System.Drawing.Point(6, 25);
            this.listSplits.Name = "listSplits";
            this.listSplits.Size = new System.Drawing.Size(601, 590);
            this.listSplits.TabIndex = 1;
            this.listSplits.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.toolStripSplits);
            this.groupBox1.Controls.Add(this.listSplits);
            this.groupBox1.Location = new System.Drawing.Point(15, 145);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(671, 621);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Splits";
            // 
            // toolStripSplits
            // 
            this.toolStripSplits.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripSplits.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripSplits.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripSplits.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.splitBtnAddSplit,
            this.btnEditSplit,
            this.btnMoveSplitUp,
            this.btnMoveSplitDown,
            this.btnDeleteSplit,
            this.btnClearSplits});
            this.toolStripSplits.Location = new System.Drawing.Point(620, 22);
            this.toolStripSplits.Name = "toolStripSplits";
            this.toolStripSplits.Size = new System.Drawing.Size(48, 596);
            this.toolStripSplits.TabIndex = 2;
            this.toolStripSplits.Text = "toolStrip1";
            // 
            // splitBtnAddSplit
            // 
            this.splitBtnAddSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
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
            this.splitBtnAddSplit.Size = new System.Drawing.Size(43, 28);
            this.splitBtnAddSplit.Text = "Add split";
            // 
            // menuAddGettingCards
            // 
            this.menuAddGettingCards.Name = "menuAddGettingCards";
            this.menuAddGettingCards.Size = new System.Drawing.Size(287, 34);
            this.menuAddGettingCards.Text = "Getting card(s)";
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
            this.btnEditSplit.Size = new System.Drawing.Size(43, 28);
            this.btnEditSplit.Text = "Edit split";
            // 
            // btnMoveSplitUp
            // 
            this.btnMoveSplitUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveSplitUp.Image = global::ZZAutosplitter.Properties.Resources.arrow_up;
            this.btnMoveSplitUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveSplitUp.Name = "btnMoveSplitUp";
            this.btnMoveSplitUp.Size = new System.Drawing.Size(43, 28);
            this.btnMoveSplitUp.Text = "Move up";
            // 
            // btnMoveSplitDown
            // 
            this.btnMoveSplitDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMoveSplitDown.Image = global::ZZAutosplitter.Properties.Resources.arrow_down;
            this.btnMoveSplitDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveSplitDown.Name = "btnMoveSplitDown";
            this.btnMoveSplitDown.Size = new System.Drawing.Size(43, 28);
            this.btnMoveSplitDown.Text = "Move down";
            // 
            // btnDeleteSplit
            // 
            this.btnDeleteSplit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDeleteSplit.Image = global::ZZAutosplitter.Properties.Resources.delete;
            this.btnDeleteSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteSplit.Name = "btnDeleteSplit";
            this.btnDeleteSplit.Size = new System.Drawing.Size(43, 28);
            this.btnDeleteSplit.Text = "Delete split";
            // 
            // btnClearSplits
            // 
            this.btnClearSplits.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnClearSplits.Image = global::ZZAutosplitter.Properties.Resources.bin_closed;
            this.btnClearSplits.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearSplits.Name = "btnClearSplits";
            this.btnClearSplits.Size = new System.Drawing.Size(43, 28);
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
            // SettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsControl";
            this.Size = new System.Drawing.Size(700, 778);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
    }
}
