namespace CoreApp
{
    partial class AddForm
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
            this.lbName = new System.Windows.Forms.Label();
            this.TbReleaseName = new System.Windows.Forms.TextBox();
            this.BtSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(12, 9);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(60, 13);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Название:";
            // 
            // TbReleaseName
            // 
            this.TbReleaseName.Location = new System.Drawing.Point(15, 35);
            this.TbReleaseName.Name = "TbReleaseName";
            this.TbReleaseName.Size = new System.Drawing.Size(359, 20);
            this.TbReleaseName.TabIndex = 1;
            this.TbReleaseName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbName_KeyDown);
            // 
            // BtSubmit
            // 
            this.BtSubmit.Location = new System.Drawing.Point(15, 60);
            this.BtSubmit.Name = "BtSubmit";
            this.BtSubmit.Size = new System.Drawing.Size(75, 23);
            this.BtSubmit.TabIndex = 2;
            this.BtSubmit.Text = "OK";
            this.BtSubmit.UseVisualStyleBackColor = true;
            this.BtSubmit.Click += new System.EventHandler(this.BtSubmit_Click);
            // 
            // AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 95);
            this.Controls.Add(this.BtSubmit);
            this.Controls.Add(this.TbReleaseName);
            this.Controls.Add(this.lbName);
            this.Name = "AddForm";
            this.Text = "Add";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.TextBox TbReleaseName;
        private System.Windows.Forms.Button BtSubmit;
    }
}