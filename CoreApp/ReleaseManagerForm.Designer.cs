namespace CoreApp
{
    partial class ReleaseManagerForm
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.BtFile = new System.Windows.Forms.ToolStripMenuItem();
            this.BtSetHomePath = new System.Windows.Forms.ToolStripMenuItem();
            this.BtRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.BtAddRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.BtCheckRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.BtFixpack = new System.Windows.Forms.ToolStripMenuItem();
            this.BtAddFixpack = new System.Windows.Forms.ToolStripMenuItem();
            this.SCMain = new System.Windows.Forms.SplitContainer();
            this.mainTree = new System.Windows.Forms.TreeView();
            this.GbCPatch = new System.Windows.Forms.GroupBox();
            this.SCCPatch = new System.Windows.Forms.SplitContainer();
            this.SCDependencies = new System.Windows.Forms.SplitContainer();
            this.LbStatus = new System.Windows.Forms.Label();
            this.CbStatus = new System.Windows.Forms.ComboBox();
            this.BtStatus = new System.Windows.Forms.Button();
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).BeginInit();
            this.SCMain.Panel1.SuspendLayout();
            this.SCMain.Panel2.SuspendLayout();
            this.SCMain.SuspendLayout();
            this.GbCPatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCCPatch)).BeginInit();
            this.SCCPatch.Panel1.SuspendLayout();
            this.SCCPatch.Panel2.SuspendLayout();
            this.SCCPatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCDependencies)).BeginInit();
            this.SCDependencies.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtFile,
            this.BtRelease,
            this.BtFixpack});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.MainMenu.Size = new System.Drawing.Size(608, 28);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "releaseMenu";
            // 
            // BtFile
            // 
            this.BtFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtSetHomePath});
            this.BtFile.Name = "BtFile";
            this.BtFile.Size = new System.Drawing.Size(57, 24);
            this.BtFile.Text = "Файл";
            // 
            // BtSetHomePath
            // 
            this.BtSetHomePath.Name = "BtSetHomePath";
            this.BtSetHomePath.Size = new System.Drawing.Size(259, 26);
            this.BtSetHomePath.Text = "Задать домашнюю папку";
            this.BtSetHomePath.Click += new System.EventHandler(this.BtSetHomePath_Click);
            // 
            // BtRelease
            // 
            this.BtRelease.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtAddRelease,
            this.BtCheckRelease});
            this.BtRelease.Name = "BtRelease";
            this.BtRelease.Size = new System.Drawing.Size(61, 24);
            this.BtRelease.Text = "Релиз";
            // 
            // BtAddRelease
            // 
            this.BtAddRelease.Name = "BtAddRelease";
            this.BtAddRelease.Size = new System.Drawing.Size(216, 26);
            this.BtAddRelease.Text = "Добавить";
            this.BtAddRelease.Click += new System.EventHandler(this.BtAddRelease_Click);
            // 
            // BtCheckRelease
            // 
            this.BtCheckRelease.Name = "BtCheckRelease";
            this.BtCheckRelease.Size = new System.Drawing.Size(216, 26);
            this.BtCheckRelease.Text = "Проверить";
            // 
            // BtFixpack
            // 
            this.BtFixpack.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtAddFixpack});
            this.BtFixpack.Name = "BtFixpack";
            this.BtFixpack.Size = new System.Drawing.Size(85, 24);
            this.BtFixpack.Text = "Поставка";
            // 
            // BtAddFixpack
            // 
            this.BtAddFixpack.Name = "BtAddFixpack";
            this.BtAddFixpack.Size = new System.Drawing.Size(216, 26);
            this.BtAddFixpack.Text = "Добавить ОП/ФП";
            this.BtAddFixpack.Click += new System.EventHandler(this.BtAddFixpack_Click);
            // 
            // SCMain
            // 
            this.SCMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCMain.Location = new System.Drawing.Point(0, 28);
            this.SCMain.Margin = new System.Windows.Forms.Padding(4);
            this.SCMain.Name = "SCMain";
            // 
            // SCMain.Panel1
            // 
            this.SCMain.Panel1.Controls.Add(this.mainTree);
            // 
            // SCMain.Panel2
            // 
            this.SCMain.Panel2.Controls.Add(this.GbCPatch);
            this.SCMain.Size = new System.Drawing.Size(608, 526);
            this.SCMain.SplitterDistance = 202;
            this.SCMain.SplitterWidth = 5;
            this.SCMain.TabIndex = 1;
            this.SCMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.mainSplitter_SplitterMoved);
            // 
            // mainTree
            // 
            this.mainTree.Location = new System.Drawing.Point(4, 4);
            this.mainTree.Margin = new System.Windows.Forms.Padding(4);
            this.mainTree.Name = "mainTree";
            this.mainTree.Size = new System.Drawing.Size(193, 516);
            this.mainTree.TabIndex = 0;
            this.mainTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mainTree_AfterSelect);
            this.mainTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mainTree_NodeMouseDoubleClick);
            // 
            // GbCPatch
            // 
            this.GbCPatch.Controls.Add(this.SCCPatch);
            this.GbCPatch.Location = new System.Drawing.Point(3, 4);
            this.GbCPatch.Name = "GbCPatch";
            this.GbCPatch.Size = new System.Drawing.Size(386, 516);
            this.GbCPatch.TabIndex = 0;
            this.GbCPatch.TabStop = false;
            this.GbCPatch.Text = "C-патч";
            // 
            // SCCPatch
            // 
            this.SCCPatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCCPatch.Location = new System.Drawing.Point(3, 18);
            this.SCCPatch.Name = "SCCPatch";
            this.SCCPatch.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SCCPatch.Panel1
            // 
            this.SCCPatch.Panel1.Controls.Add(this.BtStatus);
            this.SCCPatch.Panel1.Controls.Add(this.CbStatus);
            this.SCCPatch.Panel1.Controls.Add(this.LbStatus);
            // 
            // SCCPatch.Panel2
            // 
            this.SCCPatch.Panel2.Controls.Add(this.SCDependencies);
            this.SCCPatch.Size = new System.Drawing.Size(380, 495);
            this.SCCPatch.SplitterDistance = 126;
            this.SCCPatch.TabIndex = 0;
            // 
            // SCDependencies
            // 
            this.SCDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCDependencies.Location = new System.Drawing.Point(0, 0);
            this.SCDependencies.Name = "SCDependencies";
            this.SCDependencies.Size = new System.Drawing.Size(380, 365);
            this.SCDependencies.SplitterDistance = 187;
            this.SCDependencies.TabIndex = 0;
            // 
            // LbStatus
            // 
            this.LbStatus.AutoSize = true;
            this.LbStatus.Location = new System.Drawing.Point(3, 9);
            this.LbStatus.Name = "LbStatus";
            this.LbStatus.Size = new System.Drawing.Size(53, 17);
            this.LbStatus.TabIndex = 0;
            this.LbStatus.Text = "Статус";
            // 
            // CbStatus
            // 
            this.CbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CbStatus.FormattingEnabled = true;
            this.CbStatus.Location = new System.Drawing.Point(62, 9);
            this.CbStatus.Name = "CbStatus";
            this.CbStatus.Size = new System.Drawing.Size(129, 23);
            this.CbStatus.TabIndex = 1;
            this.CbStatus.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CbStatus_DrawItem);
            // 
            // BtStatus
            // 
            this.BtStatus.Location = new System.Drawing.Point(197, 9);
            this.BtStatus.Name = "BtStatus";
            this.BtStatus.Size = new System.Drawing.Size(109, 24);
            this.BtStatus.TabIndex = 2;
            this.BtStatus.Text = "Подтвердить";
            this.BtStatus.UseVisualStyleBackColor = true;
            this.BtStatus.Click += new System.EventHandler(this.BtStatus_Click);
            // 
            // ReleaseManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 554);
            this.Controls.Add(this.SCMain);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ReleaseManagerForm";
            this.Text = "ReleaseForm";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.SCMain.Panel1.ResumeLayout(false);
            this.SCMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).EndInit();
            this.SCMain.ResumeLayout(false);
            this.GbCPatch.ResumeLayout(false);
            this.SCCPatch.Panel1.ResumeLayout(false);
            this.SCCPatch.Panel1.PerformLayout();
            this.SCCPatch.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCCPatch)).EndInit();
            this.SCCPatch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCDependencies)).EndInit();
            this.SCDependencies.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem BtRelease;
        private System.Windows.Forms.ToolStripMenuItem BtFixpack;
        private System.Windows.Forms.ToolStripMenuItem BtAddRelease;
        private System.Windows.Forms.ToolStripMenuItem BtCheckRelease;
        private System.Windows.Forms.ToolStripMenuItem BtAddFixpack;
        private System.Windows.Forms.ToolStripMenuItem BtFile;
        private System.Windows.Forms.ToolStripMenuItem BtSetHomePath;
        private System.Windows.Forms.SplitContainer SCMain;
        private System.Windows.Forms.TreeView mainTree;
        private System.Windows.Forms.GroupBox GbCPatch;
        private System.Windows.Forms.SplitContainer SCCPatch;
        private System.Windows.Forms.SplitContainer SCDependencies;
        private System.Windows.Forms.Button BtStatus;
        private System.Windows.Forms.ComboBox CbStatus;
        private System.Windows.Forms.Label LbStatus;
    }
}