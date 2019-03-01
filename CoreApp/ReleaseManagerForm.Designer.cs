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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.BtFile = new System.Windows.Forms.ToolStripMenuItem();
            this.BtSetHomePath = new System.Windows.Forms.ToolStripMenuItem();
            this.BtRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.BtAddRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.BtCheckRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.BtFixpack = new System.Windows.Forms.ToolStripMenuItem();
            this.BtAddFixpack = new System.Windows.Forms.ToolStripMenuItem();
            this.mainSplitter = new System.Windows.Forms.SplitContainer();
            this.mainTree = new System.Windows.Forms.TreeView();
            this.GbCPatch = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitter)).BeginInit();
            this.mainSplitter.Panel1.SuspendLayout();
            this.mainSplitter.Panel2.SuspendLayout();
            this.mainSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtFile,
            this.BtRelease,
            this.BtFixpack});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(608, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "releaseMenu";
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
            this.BtAddRelease.Size = new System.Drawing.Size(161, 26);
            this.BtAddRelease.Text = "Добавить";
            this.BtAddRelease.Click += new System.EventHandler(this.BtAddRelease_Click);
            // 
            // BtCheckRelease
            // 
            this.BtCheckRelease.Name = "BtCheckRelease";
            this.BtCheckRelease.Size = new System.Drawing.Size(161, 26);
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
            this.BtAddFixpack.Size = new System.Drawing.Size(205, 26);
            this.BtAddFixpack.Text = "Добавить ОП/ФП";
            this.BtAddFixpack.Click += new System.EventHandler(this.BtAddFixpack_Click);
            // 
            // mainSplitter
            // 
            this.mainSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitter.Location = new System.Drawing.Point(0, 28);
            this.mainSplitter.Margin = new System.Windows.Forms.Padding(4);
            this.mainSplitter.Name = "mainSplitter";
            // 
            // mainSplitter.Panel1
            // 
            this.mainSplitter.Panel1.Controls.Add(this.mainTree);
            // 
            // mainSplitter.Panel2
            // 
            this.mainSplitter.Panel2.Controls.Add(this.GbCPatch);
            this.mainSplitter.Size = new System.Drawing.Size(608, 526);
            this.mainSplitter.SplitterDistance = 202;
            this.mainSplitter.SplitterWidth = 5;
            this.mainSplitter.TabIndex = 1;
            this.mainSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.mainSplitter_SplitterMoved);
            // 
            // mainTree
            // 
            this.mainTree.Location = new System.Drawing.Point(4, 4);
            this.mainTree.Margin = new System.Windows.Forms.Padding(4);
            this.mainTree.Name = "mainTree";
            this.mainTree.Size = new System.Drawing.Size(193, 516);
            this.mainTree.TabIndex = 0;
            this.mainTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mainTree_NodeMouseDoubleClick);
            // 
            // GbCPatch
            // 
            this.GbCPatch.Location = new System.Drawing.Point(3, 4);
            this.GbCPatch.Name = "GbCPatch";
            this.GbCPatch.Size = new System.Drawing.Size(386, 516);
            this.GbCPatch.TabIndex = 0;
            this.GbCPatch.TabStop = false;
            this.GbCPatch.Text = "C-патч";
            // 
            // ReleaseManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 554);
            this.Controls.Add(this.mainSplitter);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ReleaseManagerForm";
            this.Text = "ReleaseForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainSplitter.Panel1.ResumeLayout(false);
            this.mainSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitter)).EndInit();
            this.mainSplitter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem BtRelease;
        private System.Windows.Forms.ToolStripMenuItem BtFixpack;
        private System.Windows.Forms.ToolStripMenuItem BtAddRelease;
        private System.Windows.Forms.ToolStripMenuItem BtCheckRelease;
        private System.Windows.Forms.ToolStripMenuItem BtAddFixpack;
        private System.Windows.Forms.ToolStripMenuItem BtFile;
        private System.Windows.Forms.ToolStripMenuItem BtSetHomePath;
        private System.Windows.Forms.SplitContainer mainSplitter;
        private System.Windows.Forms.TreeView mainTree;
        private System.Windows.Forms.GroupBox GbCPatch;
    }
}