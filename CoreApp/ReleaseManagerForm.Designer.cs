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
            this.components = new System.ComponentModel.Container();
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
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.LbCPatchParent = new System.Windows.Forms.Label();
            this.BtCPatchStatus = new System.Windows.Forms.Button();
            this.CbCPatchStatus = new System.Windows.Forms.ComboBox();
            this.LbCPatchStatus = new System.Windows.Forms.Label();
            this.SCDependencies = new System.Windows.Forms.SplitContainer();
            this.BtCPatchAddDependenciesFrom = new System.Windows.Forms.Button();
            this.BtCPatchDeleteDependenciesFrom = new System.Windows.Forms.Button();
            this.LbCPatchDependenciesFrom = new System.Windows.Forms.Label();
            this.LboxCPatchDependenciesFrom = new System.Windows.Forms.ListBox();
            this.BtCPatchAddDependenciesTo = new System.Windows.Forms.Button();
            this.BtCPatchDeleteDependenciesTo = new System.Windows.Forms.Button();
            this.LbCPatchDependenciesTo = new System.Windows.Forms.Label();
            this.LboxCPatchDependenciesTo = new System.Windows.Forms.ListBox();
            this.mainTreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TSMIRename = new System.Windows.Forms.ToolStripMenuItem();
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
            this.SCDependencies.Panel1.SuspendLayout();
            this.SCDependencies.Panel2.SuspendLayout();
            this.SCDependencies.SuspendLayout();
            this.mainTreeContextMenu.SuspendLayout();
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
            this.mainTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.mainTree_AfterLabelEdit);
            this.mainTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mainTree_AfterSelect);
            this.mainTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mainTree_NodeMouseClick);
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
            this.SCCPatch.Panel1.Controls.Add(this.button1);
            this.SCCPatch.Panel1.Controls.Add(this.comboBox1);
            this.SCCPatch.Panel1.Controls.Add(this.LbCPatchParent);
            this.SCCPatch.Panel1.Controls.Add(this.BtCPatchStatus);
            this.SCCPatch.Panel1.Controls.Add(this.CbCPatchStatus);
            this.SCCPatch.Panel1.Controls.Add(this.LbCPatchStatus);
            // 
            // SCCPatch.Panel2
            // 
            this.SCCPatch.Panel2.Controls.Add(this.SCDependencies);
            this.SCCPatch.Size = new System.Drawing.Size(380, 495);
            this.SCCPatch.SplitterDistance = 126;
            this.SCCPatch.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(265, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 24);
            this.button1.TabIndex = 8;
            this.button1.Text = "Подтвердить";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(77, 36);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(182, 24);
            this.comboBox1.TabIndex = 7;
            // 
            // LbCPatchParent
            // 
            this.LbCPatchParent.AutoSize = true;
            this.LbCPatchParent.Location = new System.Drawing.Point(10, 36);
            this.LbCPatchParent.Name = "LbCPatchParent";
            this.LbCPatchParent.Size = new System.Drawing.Size(48, 17);
            this.LbCPatchParent.TabIndex = 6;
            this.LbCPatchParent.Text = "Релиз";
            // 
            // BtCPatchStatus
            // 
            this.BtCPatchStatus.Location = new System.Drawing.Point(265, 9);
            this.BtCPatchStatus.Name = "BtCPatchStatus";
            this.BtCPatchStatus.Size = new System.Drawing.Size(109, 24);
            this.BtCPatchStatus.TabIndex = 2;
            this.BtCPatchStatus.Text = "Подтвердить";
            this.BtCPatchStatus.UseVisualStyleBackColor = true;
            this.BtCPatchStatus.Click += new System.EventHandler(this.BtStatus_Click);
            // 
            // CbCPatchStatus
            // 
            this.CbCPatchStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CbCPatchStatus.FormattingEnabled = true;
            this.CbCPatchStatus.Location = new System.Drawing.Point(77, 9);
            this.CbCPatchStatus.Name = "CbCPatchStatus";
            this.CbCPatchStatus.Size = new System.Drawing.Size(182, 23);
            this.CbCPatchStatus.TabIndex = 1;
            this.CbCPatchStatus.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CbStatus_DrawItem);
            // 
            // LbCPatchStatus
            // 
            this.LbCPatchStatus.AutoSize = true;
            this.LbCPatchStatus.Location = new System.Drawing.Point(10, 13);
            this.LbCPatchStatus.Name = "LbCPatchStatus";
            this.LbCPatchStatus.Size = new System.Drawing.Size(53, 17);
            this.LbCPatchStatus.TabIndex = 0;
            this.LbCPatchStatus.Text = "Статус";
            // 
            // SCDependencies
            // 
            this.SCDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCDependencies.Location = new System.Drawing.Point(0, 0);
            this.SCDependencies.Name = "SCDependencies";
            // 
            // SCDependencies.Panel1
            // 
            this.SCDependencies.Panel1.Controls.Add(this.BtCPatchAddDependenciesFrom);
            this.SCDependencies.Panel1.Controls.Add(this.BtCPatchDeleteDependenciesFrom);
            this.SCDependencies.Panel1.Controls.Add(this.LbCPatchDependenciesFrom);
            this.SCDependencies.Panel1.Controls.Add(this.LboxCPatchDependenciesFrom);
            // 
            // SCDependencies.Panel2
            // 
            this.SCDependencies.Panel2.Controls.Add(this.BtCPatchAddDependenciesTo);
            this.SCDependencies.Panel2.Controls.Add(this.BtCPatchDeleteDependenciesTo);
            this.SCDependencies.Panel2.Controls.Add(this.LbCPatchDependenciesTo);
            this.SCDependencies.Panel2.Controls.Add(this.LboxCPatchDependenciesTo);
            this.SCDependencies.Size = new System.Drawing.Size(380, 365);
            this.SCDependencies.SplitterDistance = 187;
            this.SCDependencies.TabIndex = 0;
            // 
            // BtCPatchAddDependenciesFrom
            // 
            this.BtCPatchAddDependenciesFrom.Location = new System.Drawing.Point(6, 334);
            this.BtCPatchAddDependenciesFrom.Name = "BtCPatchAddDependenciesFrom";
            this.BtCPatchAddDependenciesFrom.Size = new System.Drawing.Size(178, 23);
            this.BtCPatchAddDependenciesFrom.TabIndex = 3;
            this.BtCPatchAddDependenciesFrom.Text = "Добавить зависимости";
            this.BtCPatchAddDependenciesFrom.UseVisualStyleBackColor = true;
            // 
            // BtCPatchDeleteDependenciesFrom
            // 
            this.BtCPatchDeleteDependenciesFrom.Location = new System.Drawing.Point(6, 305);
            this.BtCPatchDeleteDependenciesFrom.Name = "BtCPatchDeleteDependenciesFrom";
            this.BtCPatchDeleteDependenciesFrom.Size = new System.Drawing.Size(178, 23);
            this.BtCPatchDeleteDependenciesFrom.TabIndex = 2;
            this.BtCPatchDeleteDependenciesFrom.Text = "Удалить выбранные";
            this.BtCPatchDeleteDependenciesFrom.UseVisualStyleBackColor = true;
            // 
            // LbCPatchDependenciesFrom
            // 
            this.LbCPatchDependenciesFrom.AutoSize = true;
            this.LbCPatchDependenciesFrom.Location = new System.Drawing.Point(10, 15);
            this.LbCPatchDependenciesFrom.Name = "LbCPatchDependenciesFrom";
            this.LbCPatchDependenciesFrom.Size = new System.Drawing.Size(85, 17);
            this.LbCPatchDependenciesFrom.TabIndex = 1;
            this.LbCPatchDependenciesFrom.Text = "Зависит от:";
            // 
            // LboxCPatchDependenciesFrom
            // 
            this.LboxCPatchDependenciesFrom.FormattingEnabled = true;
            this.LboxCPatchDependenciesFrom.ItemHeight = 16;
            this.LboxCPatchDependenciesFrom.Location = new System.Drawing.Point(6, 35);
            this.LboxCPatchDependenciesFrom.Name = "LboxCPatchDependenciesFrom";
            this.LboxCPatchDependenciesFrom.Size = new System.Drawing.Size(178, 260);
            this.LboxCPatchDependenciesFrom.TabIndex = 0;
            // 
            // BtCPatchAddDependenciesTo
            // 
            this.BtCPatchAddDependenciesTo.Location = new System.Drawing.Point(8, 334);
            this.BtCPatchAddDependenciesTo.Name = "BtCPatchAddDependenciesTo";
            this.BtCPatchAddDependenciesTo.Size = new System.Drawing.Size(178, 23);
            this.BtCPatchAddDependenciesTo.TabIndex = 5;
            this.BtCPatchAddDependenciesTo.Text = "Добавить зависимости";
            this.BtCPatchAddDependenciesTo.UseVisualStyleBackColor = true;
            // 
            // BtCPatchDeleteDependenciesTo
            // 
            this.BtCPatchDeleteDependenciesTo.Location = new System.Drawing.Point(8, 305);
            this.BtCPatchDeleteDependenciesTo.Name = "BtCPatchDeleteDependenciesTo";
            this.BtCPatchDeleteDependenciesTo.Size = new System.Drawing.Size(178, 23);
            this.BtCPatchDeleteDependenciesTo.TabIndex = 4;
            this.BtCPatchDeleteDependenciesTo.Text = "Удалить выбранные";
            this.BtCPatchDeleteDependenciesTo.UseVisualStyleBackColor = true;
            // 
            // LbCPatchDependenciesTo
            // 
            this.LbCPatchDependenciesTo.AutoSize = true;
            this.LbCPatchDependenciesTo.Location = new System.Drawing.Point(13, 16);
            this.LbCPatchDependenciesTo.Name = "LbCPatchDependenciesTo";
            this.LbCPatchDependenciesTo.Size = new System.Drawing.Size(72, 17);
            this.LbCPatchDependenciesTo.TabIndex = 2;
            this.LbCPatchDependenciesTo.Text = "Влият на:";
            // 
            // LboxCPatchDependenciesTo
            // 
            this.LboxCPatchDependenciesTo.FormattingEnabled = true;
            this.LboxCPatchDependenciesTo.ItemHeight = 16;
            this.LboxCPatchDependenciesTo.Location = new System.Drawing.Point(5, 36);
            this.LboxCPatchDependenciesTo.Name = "LboxCPatchDependenciesTo";
            this.LboxCPatchDependenciesTo.Size = new System.Drawing.Size(178, 260);
            this.LboxCPatchDependenciesTo.TabIndex = 1;
            // 
            // mainTreeContextMenu
            // 
            this.mainTreeContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainTreeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIRename});
            this.mainTreeContextMenu.Name = "mainTreeContextMenu";
            this.mainTreeContextMenu.Size = new System.Drawing.Size(191, 28);
            // 
            // TSMIRename
            // 
            this.TSMIRename.Name = "TSMIRename";
            this.TSMIRename.Size = new System.Drawing.Size(190, 24);
            this.TSMIRename.Text = "Переименовать";
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
            this.SCDependencies.Panel1.ResumeLayout(false);
            this.SCDependencies.Panel1.PerformLayout();
            this.SCDependencies.Panel2.ResumeLayout(false);
            this.SCDependencies.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCDependencies)).EndInit();
            this.SCDependencies.ResumeLayout(false);
            this.mainTreeContextMenu.ResumeLayout(false);
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
        private System.Windows.Forms.Button BtCPatchStatus;
        private System.Windows.Forms.ComboBox CbCPatchStatus;
        private System.Windows.Forms.Label LbCPatchStatus;
        private System.Windows.Forms.Button BtCPatchAddDependenciesFrom;
        private System.Windows.Forms.Button BtCPatchDeleteDependenciesFrom;
        private System.Windows.Forms.Label LbCPatchDependenciesFrom;
        private System.Windows.Forms.ListBox LboxCPatchDependenciesFrom;
        private System.Windows.Forms.Button BtCPatchAddDependenciesTo;
        private System.Windows.Forms.Button BtCPatchDeleteDependenciesTo;
        private System.Windows.Forms.Label LbCPatchDependenciesTo;
        private System.Windows.Forms.ListBox LboxCPatchDependenciesTo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label LbCPatchParent;
        private System.Windows.Forms.ContextMenuStrip mainTreeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem TSMIRename;
    }
}