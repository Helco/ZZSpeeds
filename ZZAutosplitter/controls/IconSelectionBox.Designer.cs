
namespace ZZAutosplitter.controls
{
    partial class IconSelectionBox
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.butonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.listIcons = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // butonCancel
            // 
            this.butonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butonCancel.AutoSize = true;
            this.butonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butonCancel.Location = new System.Drawing.Point(357, 390);
            this.butonCancel.Name = "butonCancel";
            this.butonCancel.Size = new System.Drawing.Size(109, 42);
            this.butonCancel.TabIndex = 1;
            this.butonCancel.Text = "Cancel";
            this.butonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.AutoSize = true;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOK.Location = new System.Drawing.Point(242, 390);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(109, 42);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // listIcons
            // 
            this.listIcons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listIcons.HideSelection = false;
            this.listIcons.Location = new System.Drawing.Point(12, 12);
            this.listIcons.MultiSelect = false;
            this.listIcons.Name = "listIcons";
            this.listIcons.Size = new System.Drawing.Size(454, 372);
            this.listIcons.TabIndex = 2;
            this.listIcons.TileSize = new System.Drawing.Size(70, 70);
            this.listIcons.UseCompatibleStateImageBehavior = false;
            this.listIcons.View = System.Windows.Forms.View.Tile;
            this.listIcons.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listIcons_MouseDoubleClick);
            // 
            // IconSelectionBox
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.butonCancel;
            this.ClientSize = new System.Drawing.Size(478, 444);
            this.Controls.Add(this.listIcons);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.butonCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IconSelectionBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Select an icon";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ListView listIcons;
    }
}