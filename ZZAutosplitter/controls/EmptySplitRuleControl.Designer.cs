
namespace ZZAutosplitter.controls
{
    partial class EmptySplitRuleControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddSplitRules = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnAddSplitRules, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(415, 84);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnAddSplitRules
            // 
            this.btnAddSplitRules.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddSplitRules.AutoSize = true;
            this.btnAddSplitRules.Enabled = false;
            this.btnAddSplitRules.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSplitRules.Location = new System.Drawing.Point(139, 27);
            this.btnAddSplitRules.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAddSplitRules.Name = "btnAddSplitRules";
            this.btnAddSplitRules.Size = new System.Drawing.Size(136, 30);
            this.btnAddSplitRules.TabIndex = 0;
            this.btnAddSplitRules.Text = "Add all to Splits";
            this.btnAddSplitRules.UseVisualStyleBackColor = true;
            this.btnAddSplitRules.Visible = false;
            this.btnAddSplitRules.Click += new System.EventHandler(this.btnAddSplitRules_Click);
            // 
            // EmptySplitRuleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "EmptySplitRuleControl";
            this.Size = new System.Drawing.Size(415, 84);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnAddSplitRules;
    }
}
