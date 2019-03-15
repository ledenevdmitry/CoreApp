namespace CoreApp
{
    partial class CPatchesListForm
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
            this.LboxCPatches = new System.Windows.Forms.ListBox();
            this.BtConfirm = new System.Windows.Forms.Button();
            this.SCMain = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).BeginInit();
            this.SCMain.Panel1.SuspendLayout();
            this.SCMain.Panel2.SuspendLayout();
            this.SCMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // LboxCPatches
            // 
            this.LboxCPatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LboxCPatches.FormattingEnabled = true;
            this.LboxCPatches.ItemHeight = 16;
            this.LboxCPatches.Location = new System.Drawing.Point(0, 0);
            this.LboxCPatches.Name = "LboxCPatches";
            this.LboxCPatches.Size = new System.Drawing.Size(663, 450);
            this.LboxCPatches.TabIndex = 0;
            // 
            // BtConfirm
            // 
            this.BtConfirm.Location = new System.Drawing.Point(12, 193);
            this.BtConfirm.Name = "BtConfirm";
            this.BtConfirm.Size = new System.Drawing.Size(109, 60);
            this.BtConfirm.TabIndex = 0;
            this.BtConfirm.Text = "Выбрать";
            this.BtConfirm.UseVisualStyleBackColor = true;
            this.BtConfirm.Click += new System.EventHandler(this.BtConfirm_Click);
            // 
            // SCMain
            // 
            this.SCMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SCMain.Location = new System.Drawing.Point(0, 0);
            this.SCMain.Name = "SCMain";
            // 
            // SCMain.Panel1
            // 
            this.SCMain.Panel1.Controls.Add(this.LboxCPatches);
            // 
            // SCMain.Panel2
            // 
            this.SCMain.Panel2.Controls.Add(this.BtConfirm);
            this.SCMain.Size = new System.Drawing.Size(800, 450);
            this.SCMain.SplitterDistance = 663;
            this.SCMain.TabIndex = 1;
            // 
            // CPatchesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SCMain);
            this.Name = "CPatchesForm";
            this.Text = "CPatchesForm";
            this.SCMain.Panel1.ResumeLayout(false);
            this.SCMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).EndInit();
            this.SCMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LboxCPatches;
        private System.Windows.Forms.Button BtConfirm;
        private System.Windows.Forms.SplitContainer SCMain;
    }
}