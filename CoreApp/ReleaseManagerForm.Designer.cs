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
            this.BtReleaseGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.BtFixpack = new System.Windows.Forms.ToolStripMenuItem();
            this.BtAddFixpack = new System.Windows.Forms.ToolStripMenuItem();
            this.BtCPatchGraph = new System.Windows.Forms.ToolStripMenuItem();
            this.BtZPatchOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.SCMain = new System.Windows.Forms.SplitContainer();
            this.mainTree = new System.Windows.Forms.TreeView();
            this.GbCPatch = new System.Windows.Forms.GroupBox();
            this.SCCPatch = new System.Windows.Forms.SplitContainer();
            this.BtCPatchRelease = new System.Windows.Forms.Button();
            this.CbCPatchRelease = new System.Windows.Forms.ComboBox();
            this.LbCPatchRelease = new System.Windows.Forms.Label();
            this.BtCPatchStatus = new System.Windows.Forms.Button();
            this.CbCPatchStatus = new System.Windows.Forms.ComboBox();
            this.LbCPatchStatus = new System.Windows.Forms.Label();
            this.SCCPatchDependencies = new System.Windows.Forms.SplitContainer();
            this.SCCPatchDependenciesFrom = new System.Windows.Forms.SplitContainer();
            this.GbCPatchDependenciesFrom = new System.Windows.Forms.GroupBox();
            this.LboxCPatchDependenciesFrom = new System.Windows.Forms.ListBox();
            this.BtCPatchAddDependenciesFrom = new System.Windows.Forms.Button();
            this.BtCPatchDeleteDependenciesFrom = new System.Windows.Forms.Button();
            this.SCCPatchDependenciesTo = new System.Windows.Forms.SplitContainer();
            this.GbCPatchDependenciesTo = new System.Windows.Forms.GroupBox();
            this.LboxCPatchDependenciesTo = new System.Windows.Forms.ListBox();
            this.BtCPatchAddDependenciesTo = new System.Windows.Forms.Button();
            this.BtCPatchDeleteDependenciesTo = new System.Windows.Forms.Button();
            this.GbZPatch = new System.Windows.Forms.GroupBox();
            this.SCZPatch = new System.Windows.Forms.SplitContainer();
            this.BtZPatchCPatch = new System.Windows.Forms.Button();
            this.CbZPatchCPatch = new System.Windows.Forms.ComboBox();
            this.LbCPatch = new System.Windows.Forms.Label();
            this.BtZPatchStatus = new System.Windows.Forms.Button();
            this.CbZPatchStatus = new System.Windows.Forms.ComboBox();
            this.LbZPatchStatus = new System.Windows.Forms.Label();
            this.SCZPatchDependencies = new System.Windows.Forms.SplitContainer();
            this.SCZPatchDependenciesFrom = new System.Windows.Forms.SplitContainer();
            this.GbZPatchDependenciesFrom = new System.Windows.Forms.GroupBox();
            this.LboxZPatchDependenciesFrom = new System.Windows.Forms.ListBox();
            this.BtZPatchAddDependenciesFrom = new System.Windows.Forms.Button();
            this.BtZPatchDeleteDependenciesFrom = new System.Windows.Forms.Button();
            this.SCZPatchDependenciesTo = new System.Windows.Forms.SplitContainer();
            this.GbZPatchDependenciesTo = new System.Windows.Forms.GroupBox();
            this.LboxZPatchDependenciesTo = new System.Windows.Forms.ListBox();
            this.BtZPatchDeleteDependenciesTo = new System.Windows.Forms.Button();
            this.BtZPatchAddDependenciesTo = new System.Windows.Forms.Button();
            this.mainTreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TSMIRename = new System.Windows.Forms.ToolStripMenuItem();
            this.BtDeleteRelease = new System.Windows.Forms.ToolStripMenuItem();
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
            ((System.ComponentModel.ISupportInitialize)(this.SCCPatchDependencies)).BeginInit();
            this.SCCPatchDependencies.Panel1.SuspendLayout();
            this.SCCPatchDependencies.Panel2.SuspendLayout();
            this.SCCPatchDependencies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCCPatchDependenciesFrom)).BeginInit();
            this.SCCPatchDependenciesFrom.Panel1.SuspendLayout();
            this.SCCPatchDependenciesFrom.Panel2.SuspendLayout();
            this.SCCPatchDependenciesFrom.SuspendLayout();
            this.GbCPatchDependenciesFrom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCCPatchDependenciesTo)).BeginInit();
            this.SCCPatchDependenciesTo.Panel1.SuspendLayout();
            this.SCCPatchDependenciesTo.Panel2.SuspendLayout();
            this.SCCPatchDependenciesTo.SuspendLayout();
            this.GbCPatchDependenciesTo.SuspendLayout();
            this.GbZPatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCZPatch)).BeginInit();
            this.SCZPatch.Panel1.SuspendLayout();
            this.SCZPatch.Panel2.SuspendLayout();
            this.SCZPatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCZPatchDependencies)).BeginInit();
            this.SCZPatchDependencies.Panel1.SuspendLayout();
            this.SCZPatchDependencies.Panel2.SuspendLayout();
            this.SCZPatchDependencies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCZPatchDependenciesFrom)).BeginInit();
            this.SCZPatchDependenciesFrom.Panel1.SuspendLayout();
            this.SCZPatchDependenciesFrom.Panel2.SuspendLayout();
            this.SCZPatchDependenciesFrom.SuspendLayout();
            this.GbZPatchDependenciesFrom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCZPatchDependenciesTo)).BeginInit();
            this.SCZPatchDependenciesTo.Panel1.SuspendLayout();
            this.SCZPatchDependenciesTo.Panel2.SuspendLayout();
            this.SCZPatchDependenciesTo.SuspendLayout();
            this.GbZPatchDependenciesTo.SuspendLayout();
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
            this.BtCheckRelease,
            this.BtReleaseGraph,
            this.BtDeleteRelease});
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
            // BtReleaseGraph
            // 
            this.BtReleaseGraph.Name = "BtReleaseGraph";
            this.BtReleaseGraph.Size = new System.Drawing.Size(216, 26);
            this.BtReleaseGraph.Text = "Граф";
            this.BtReleaseGraph.Click += new System.EventHandler(this.BtReleaseGraph_Click);
            // 
            // BtFixpack
            // 
            this.BtFixpack.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtAddFixpack,
            this.BtCPatchGraph,
            this.BtZPatchOrder});
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
            // BtCPatchGraph
            // 
            this.BtCPatchGraph.Name = "BtCPatchGraph";
            this.BtCPatchGraph.Size = new System.Drawing.Size(205, 26);
            this.BtCPatchGraph.Text = "Граф";
            this.BtCPatchGraph.Click += new System.EventHandler(this.BtCPatchGraph_Click);
            // 
            // BtZPatchOrder
            // 
            this.BtZPatchOrder.Name = "BtZPatchOrder";
            this.BtZPatchOrder.Size = new System.Drawing.Size(205, 26);
            this.BtZPatchOrder.Text = "Порядок патчей";
            this.BtZPatchOrder.Click += new System.EventHandler(this.BtZPatchOrder_Click);
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
            this.SCMain.Panel2.Controls.Add(this.GbZPatch);
            this.SCMain.Size = new System.Drawing.Size(608, 526);
            this.SCMain.SplitterDistance = 202;
            this.SCMain.SplitterWidth = 5;
            this.SCMain.TabIndex = 1;
            // 
            // mainTree
            // 
            this.mainTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTree.Location = new System.Drawing.Point(0, 0);
            this.mainTree.Margin = new System.Windows.Forms.Padding(4);
            this.mainTree.Name = "mainTree";
            this.mainTree.Size = new System.Drawing.Size(202, 526);
            this.mainTree.TabIndex = 0;
            this.mainTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.mainTree_AfterLabelEdit);
            this.mainTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mainTree_AfterSelect);
            this.mainTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mainTree_NodeMouseClick);
            this.mainTree.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mainTree_NodeMouseDoubleClick);
            // 
            // GbCPatch
            // 
            this.GbCPatch.Controls.Add(this.SCCPatch);
            this.GbCPatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbCPatch.Location = new System.Drawing.Point(0, 0);
            this.GbCPatch.Name = "GbCPatch";
            this.GbCPatch.Size = new System.Drawing.Size(401, 526);
            this.GbCPatch.TabIndex = 0;
            this.GbCPatch.TabStop = false;
            this.GbCPatch.Text = "C-патч";
            // 
            // SCCPatch
            // 
            this.SCCPatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCCPatch.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SCCPatch.IsSplitterFixed = true;
            this.SCCPatch.Location = new System.Drawing.Point(3, 18);
            this.SCCPatch.Name = "SCCPatch";
            this.SCCPatch.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SCCPatch.Panel1
            // 
            this.SCCPatch.Panel1.Controls.Add(this.BtCPatchRelease);
            this.SCCPatch.Panel1.Controls.Add(this.CbCPatchRelease);
            this.SCCPatch.Panel1.Controls.Add(this.LbCPatchRelease);
            this.SCCPatch.Panel1.Controls.Add(this.BtCPatchStatus);
            this.SCCPatch.Panel1.Controls.Add(this.CbCPatchStatus);
            this.SCCPatch.Panel1.Controls.Add(this.LbCPatchStatus);
            // 
            // SCCPatch.Panel2
            // 
            this.SCCPatch.Panel2.Controls.Add(this.SCCPatchDependencies);
            this.SCCPatch.Size = new System.Drawing.Size(395, 505);
            this.SCCPatch.SplitterDistance = 128;
            this.SCCPatch.TabIndex = 0;
            // 
            // BtCPatchRelease
            // 
            this.BtCPatchRelease.Location = new System.Drawing.Point(265, 36);
            this.BtCPatchRelease.Name = "BtCPatchRelease";
            this.BtCPatchRelease.Size = new System.Drawing.Size(109, 24);
            this.BtCPatchRelease.TabIndex = 8;
            this.BtCPatchRelease.Text = "Подтвердить";
            this.BtCPatchRelease.UseVisualStyleBackColor = true;
            this.BtCPatchRelease.Click += new System.EventHandler(this.BtCPatchRelease_Click);
            // 
            // CbCPatchRelease
            // 
            this.CbCPatchRelease.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CbCPatchRelease.FormattingEnabled = true;
            this.CbCPatchRelease.Location = new System.Drawing.Point(77, 36);
            this.CbCPatchRelease.Name = "CbCPatchRelease";
            this.CbCPatchRelease.Size = new System.Drawing.Size(182, 23);
            this.CbCPatchRelease.TabIndex = 7;
            this.CbCPatchRelease.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CbCPatchRelease_DrawItem);
            // 
            // LbCPatchRelease
            // 
            this.LbCPatchRelease.AutoSize = true;
            this.LbCPatchRelease.Location = new System.Drawing.Point(10, 36);
            this.LbCPatchRelease.Name = "LbCPatchRelease";
            this.LbCPatchRelease.Size = new System.Drawing.Size(48, 17);
            this.LbCPatchRelease.TabIndex = 6;
            this.LbCPatchRelease.Text = "Релиз";
            // 
            // BtCPatchStatus
            // 
            this.BtCPatchStatus.Location = new System.Drawing.Point(265, 9);
            this.BtCPatchStatus.Name = "BtCPatchStatus";
            this.BtCPatchStatus.Size = new System.Drawing.Size(109, 24);
            this.BtCPatchStatus.TabIndex = 2;
            this.BtCPatchStatus.Text = "Подтвердить";
            this.BtCPatchStatus.UseVisualStyleBackColor = true;
            this.BtCPatchStatus.Click += new System.EventHandler(this.BtCPatchStatus_Click);
            // 
            // CbCPatchStatus
            // 
            this.CbCPatchStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CbCPatchStatus.FormattingEnabled = true;
            this.CbCPatchStatus.Location = new System.Drawing.Point(77, 9);
            this.CbCPatchStatus.Name = "CbCPatchStatus";
            this.CbCPatchStatus.Size = new System.Drawing.Size(182, 23);
            this.CbCPatchStatus.TabIndex = 1;
            this.CbCPatchStatus.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CbCPatchStatus_DrawItem);
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
            // SCCPatchDependencies
            // 
            this.SCCPatchDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCCPatchDependencies.Location = new System.Drawing.Point(0, 0);
            this.SCCPatchDependencies.Name = "SCCPatchDependencies";
            // 
            // SCCPatchDependencies.Panel1
            // 
            this.SCCPatchDependencies.Panel1.Controls.Add(this.SCCPatchDependenciesFrom);
            // 
            // SCCPatchDependencies.Panel2
            // 
            this.SCCPatchDependencies.Panel2.Controls.Add(this.SCCPatchDependenciesTo);
            this.SCCPatchDependencies.Size = new System.Drawing.Size(395, 373);
            this.SCCPatchDependencies.SplitterDistance = 194;
            this.SCCPatchDependencies.TabIndex = 0;
            // 
            // SCCPatchDependenciesFrom
            // 
            this.SCCPatchDependenciesFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCCPatchDependenciesFrom.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SCCPatchDependenciesFrom.IsSplitterFixed = true;
            this.SCCPatchDependenciesFrom.Location = new System.Drawing.Point(0, 0);
            this.SCCPatchDependenciesFrom.Name = "SCCPatchDependenciesFrom";
            this.SCCPatchDependenciesFrom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SCCPatchDependenciesFrom.Panel1
            // 
            this.SCCPatchDependenciesFrom.Panel1.Controls.Add(this.GbCPatchDependenciesFrom);
            // 
            // SCCPatchDependenciesFrom.Panel2
            // 
            this.SCCPatchDependenciesFrom.Panel2.Controls.Add(this.BtCPatchAddDependenciesFrom);
            this.SCCPatchDependenciesFrom.Panel2.Controls.Add(this.BtCPatchDeleteDependenciesFrom);
            this.SCCPatchDependenciesFrom.Size = new System.Drawing.Size(194, 373);
            this.SCCPatchDependenciesFrom.SplitterDistance = 290;
            this.SCCPatchDependenciesFrom.TabIndex = 4;
            // 
            // GbCPatchDependenciesFrom
            // 
            this.GbCPatchDependenciesFrom.Controls.Add(this.LboxCPatchDependenciesFrom);
            this.GbCPatchDependenciesFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbCPatchDependenciesFrom.Location = new System.Drawing.Point(0, 0);
            this.GbCPatchDependenciesFrom.Name = "GbCPatchDependenciesFrom";
            this.GbCPatchDependenciesFrom.Size = new System.Drawing.Size(194, 290);
            this.GbCPatchDependenciesFrom.TabIndex = 1;
            this.GbCPatchDependenciesFrom.TabStop = false;
            this.GbCPatchDependenciesFrom.Text = "Зависит от";
            // 
            // LboxCPatchDependenciesFrom
            // 
            this.LboxCPatchDependenciesFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LboxCPatchDependenciesFrom.FormattingEnabled = true;
            this.LboxCPatchDependenciesFrom.ItemHeight = 16;
            this.LboxCPatchDependenciesFrom.Location = new System.Drawing.Point(3, 18);
            this.LboxCPatchDependenciesFrom.Name = "LboxCPatchDependenciesFrom";
            this.LboxCPatchDependenciesFrom.Size = new System.Drawing.Size(188, 269);
            this.LboxCPatchDependenciesFrom.TabIndex = 0;
            // 
            // BtCPatchAddDependenciesFrom
            // 
            this.BtCPatchAddDependenciesFrom.Location = new System.Drawing.Point(6, 41);
            this.BtCPatchAddDependenciesFrom.Name = "BtCPatchAddDependenciesFrom";
            this.BtCPatchAddDependenciesFrom.Size = new System.Drawing.Size(178, 23);
            this.BtCPatchAddDependenciesFrom.TabIndex = 3;
            this.BtCPatchAddDependenciesFrom.Text = "Добавить зависимости";
            this.BtCPatchAddDependenciesFrom.UseVisualStyleBackColor = true;
            // 
            // BtCPatchDeleteDependenciesFrom
            // 
            this.BtCPatchDeleteDependenciesFrom.Location = new System.Drawing.Point(6, 12);
            this.BtCPatchDeleteDependenciesFrom.Name = "BtCPatchDeleteDependenciesFrom";
            this.BtCPatchDeleteDependenciesFrom.Size = new System.Drawing.Size(178, 23);
            this.BtCPatchDeleteDependenciesFrom.TabIndex = 2;
            this.BtCPatchDeleteDependenciesFrom.Text = "Удалить выбранные";
            this.BtCPatchDeleteDependenciesFrom.UseVisualStyleBackColor = true;
            // 
            // SCCPatchDependenciesTo
            // 
            this.SCCPatchDependenciesTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCCPatchDependenciesTo.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SCCPatchDependenciesTo.IsSplitterFixed = true;
            this.SCCPatchDependenciesTo.Location = new System.Drawing.Point(0, 0);
            this.SCCPatchDependenciesTo.Name = "SCCPatchDependenciesTo";
            this.SCCPatchDependenciesTo.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SCCPatchDependenciesTo.Panel1
            // 
            this.SCCPatchDependenciesTo.Panel1.Controls.Add(this.GbCPatchDependenciesTo);
            // 
            // SCCPatchDependenciesTo.Panel2
            // 
            this.SCCPatchDependenciesTo.Panel2.Controls.Add(this.BtCPatchAddDependenciesTo);
            this.SCCPatchDependenciesTo.Panel2.Controls.Add(this.BtCPatchDeleteDependenciesTo);
            this.SCCPatchDependenciesTo.Size = new System.Drawing.Size(197, 373);
            this.SCCPatchDependenciesTo.SplitterDistance = 290;
            this.SCCPatchDependenciesTo.TabIndex = 5;
            // 
            // GbCPatchDependenciesTo
            // 
            this.GbCPatchDependenciesTo.Controls.Add(this.LboxCPatchDependenciesTo);
            this.GbCPatchDependenciesTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbCPatchDependenciesTo.Location = new System.Drawing.Point(0, 0);
            this.GbCPatchDependenciesTo.Name = "GbCPatchDependenciesTo";
            this.GbCPatchDependenciesTo.Size = new System.Drawing.Size(197, 290);
            this.GbCPatchDependenciesTo.TabIndex = 1;
            this.GbCPatchDependenciesTo.TabStop = false;
            this.GbCPatchDependenciesTo.Text = "Влияет на";
            // 
            // LboxCPatchDependenciesTo
            // 
            this.LboxCPatchDependenciesTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LboxCPatchDependenciesTo.FormattingEnabled = true;
            this.LboxCPatchDependenciesTo.ItemHeight = 16;
            this.LboxCPatchDependenciesTo.Location = new System.Drawing.Point(3, 18);
            this.LboxCPatchDependenciesTo.Name = "LboxCPatchDependenciesTo";
            this.LboxCPatchDependenciesTo.Size = new System.Drawing.Size(191, 269);
            this.LboxCPatchDependenciesTo.TabIndex = 0;
            // 
            // BtCPatchAddDependenciesTo
            // 
            this.BtCPatchAddDependenciesTo.Location = new System.Drawing.Point(6, 41);
            this.BtCPatchAddDependenciesTo.Name = "BtCPatchAddDependenciesTo";
            this.BtCPatchAddDependenciesTo.Size = new System.Drawing.Size(178, 23);
            this.BtCPatchAddDependenciesTo.TabIndex = 3;
            this.BtCPatchAddDependenciesTo.Text = "Добавить зависимости";
            this.BtCPatchAddDependenciesTo.UseVisualStyleBackColor = true;
            // 
            // BtCPatchDeleteDependenciesTo
            // 
            this.BtCPatchDeleteDependenciesTo.Location = new System.Drawing.Point(6, 12);
            this.BtCPatchDeleteDependenciesTo.Name = "BtCPatchDeleteDependenciesTo";
            this.BtCPatchDeleteDependenciesTo.Size = new System.Drawing.Size(178, 23);
            this.BtCPatchDeleteDependenciesTo.TabIndex = 2;
            this.BtCPatchDeleteDependenciesTo.Text = "Удалить выбранные";
            this.BtCPatchDeleteDependenciesTo.UseVisualStyleBackColor = true;
            // 
            // GbZPatch
            // 
            this.GbZPatch.Controls.Add(this.SCZPatch);
            this.GbZPatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbZPatch.Location = new System.Drawing.Point(0, 0);
            this.GbZPatch.Name = "GbZPatch";
            this.GbZPatch.Size = new System.Drawing.Size(401, 526);
            this.GbZPatch.TabIndex = 2;
            this.GbZPatch.TabStop = false;
            this.GbZPatch.Text = "Z-патч";
            // 
            // SCZPatch
            // 
            this.SCZPatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCZPatch.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.SCZPatch.IsSplitterFixed = true;
            this.SCZPatch.Location = new System.Drawing.Point(3, 18);
            this.SCZPatch.Name = "SCZPatch";
            this.SCZPatch.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SCZPatch.Panel1
            // 
            this.SCZPatch.Panel1.Controls.Add(this.BtZPatchCPatch);
            this.SCZPatch.Panel1.Controls.Add(this.CbZPatchCPatch);
            this.SCZPatch.Panel1.Controls.Add(this.LbCPatch);
            this.SCZPatch.Panel1.Controls.Add(this.BtZPatchStatus);
            this.SCZPatch.Panel1.Controls.Add(this.CbZPatchStatus);
            this.SCZPatch.Panel1.Controls.Add(this.LbZPatchStatus);
            // 
            // SCZPatch.Panel2
            // 
            this.SCZPatch.Panel2.Controls.Add(this.SCZPatchDependencies);
            this.SCZPatch.Size = new System.Drawing.Size(395, 505);
            this.SCZPatch.SplitterDistance = 107;
            this.SCZPatch.TabIndex = 0;
            // 
            // BtZPatchCPatch
            // 
            this.BtZPatchCPatch.Location = new System.Drawing.Point(265, 36);
            this.BtZPatchCPatch.Name = "BtZPatchCPatch";
            this.BtZPatchCPatch.Size = new System.Drawing.Size(109, 24);
            this.BtZPatchCPatch.TabIndex = 8;
            this.BtZPatchCPatch.Text = "Подтвердить";
            this.BtZPatchCPatch.UseVisualStyleBackColor = true;
            // 
            // CbZPatchCPatch
            // 
            this.CbZPatchCPatch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CbZPatchCPatch.FormattingEnabled = true;
            this.CbZPatchCPatch.Location = new System.Drawing.Point(77, 36);
            this.CbZPatchCPatch.Name = "CbZPatchCPatch";
            this.CbZPatchCPatch.Size = new System.Drawing.Size(182, 23);
            this.CbZPatchCPatch.TabIndex = 7;
            this.CbZPatchCPatch.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CbZPatchCPatch_DrawItem);
            // 
            // LbCPatch
            // 
            this.LbCPatch.AutoSize = true;
            this.LbCPatch.Location = new System.Drawing.Point(10, 36);
            this.LbCPatch.Name = "LbCPatch";
            this.LbCPatch.Size = new System.Drawing.Size(53, 17);
            this.LbCPatch.TabIndex = 6;
            this.LbCPatch.Text = "C-патч";
            // 
            // BtZPatchStatus
            // 
            this.BtZPatchStatus.Location = new System.Drawing.Point(265, 9);
            this.BtZPatchStatus.Name = "BtZPatchStatus";
            this.BtZPatchStatus.Size = new System.Drawing.Size(109, 24);
            this.BtZPatchStatus.TabIndex = 2;
            this.BtZPatchStatus.Text = "Подтвердить";
            this.BtZPatchStatus.UseVisualStyleBackColor = true;
            this.BtZPatchStatus.Click += new System.EventHandler(this.BtZPatchStatus_Click);
            // 
            // CbZPatchStatus
            // 
            this.CbZPatchStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CbZPatchStatus.FormattingEnabled = true;
            this.CbZPatchStatus.Location = new System.Drawing.Point(77, 9);
            this.CbZPatchStatus.Name = "CbZPatchStatus";
            this.CbZPatchStatus.Size = new System.Drawing.Size(182, 23);
            this.CbZPatchStatus.TabIndex = 1;
            this.CbZPatchStatus.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CbZPatchStatus_DrawItem);
            // 
            // LbZPatchStatus
            // 
            this.LbZPatchStatus.AutoSize = true;
            this.LbZPatchStatus.Location = new System.Drawing.Point(10, 13);
            this.LbZPatchStatus.Name = "LbZPatchStatus";
            this.LbZPatchStatus.Size = new System.Drawing.Size(53, 17);
            this.LbZPatchStatus.TabIndex = 0;
            this.LbZPatchStatus.Text = "Статус";
            // 
            // SCZPatchDependencies
            // 
            this.SCZPatchDependencies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCZPatchDependencies.Location = new System.Drawing.Point(0, 0);
            this.SCZPatchDependencies.Name = "SCZPatchDependencies";
            // 
            // SCZPatchDependencies.Panel1
            // 
            this.SCZPatchDependencies.Panel1.Controls.Add(this.SCZPatchDependenciesFrom);
            // 
            // SCZPatchDependencies.Panel2
            // 
            this.SCZPatchDependencies.Panel2.Controls.Add(this.SCZPatchDependenciesTo);
            this.SCZPatchDependencies.Size = new System.Drawing.Size(395, 394);
            this.SCZPatchDependencies.SplitterDistance = 194;
            this.SCZPatchDependencies.TabIndex = 0;
            // 
            // SCZPatchDependenciesFrom
            // 
            this.SCZPatchDependenciesFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCZPatchDependenciesFrom.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SCZPatchDependenciesFrom.IsSplitterFixed = true;
            this.SCZPatchDependenciesFrom.Location = new System.Drawing.Point(0, 0);
            this.SCZPatchDependenciesFrom.Name = "SCZPatchDependenciesFrom";
            this.SCZPatchDependenciesFrom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SCZPatchDependenciesFrom.Panel1
            // 
            this.SCZPatchDependenciesFrom.Panel1.Controls.Add(this.GbZPatchDependenciesFrom);
            // 
            // SCZPatchDependenciesFrom.Panel2
            // 
            this.SCZPatchDependenciesFrom.Panel2.Controls.Add(this.BtZPatchAddDependenciesFrom);
            this.SCZPatchDependenciesFrom.Panel2.Controls.Add(this.BtZPatchDeleteDependenciesFrom);
            this.SCZPatchDependenciesFrom.Size = new System.Drawing.Size(194, 394);
            this.SCZPatchDependenciesFrom.SplitterDistance = 290;
            this.SCZPatchDependenciesFrom.TabIndex = 4;
            // 
            // GbZPatchDependenciesFrom
            // 
            this.GbZPatchDependenciesFrom.Controls.Add(this.LboxZPatchDependenciesFrom);
            this.GbZPatchDependenciesFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbZPatchDependenciesFrom.Location = new System.Drawing.Point(0, 0);
            this.GbZPatchDependenciesFrom.Name = "GbZPatchDependenciesFrom";
            this.GbZPatchDependenciesFrom.Size = new System.Drawing.Size(194, 290);
            this.GbZPatchDependenciesFrom.TabIndex = 1;
            this.GbZPatchDependenciesFrom.TabStop = false;
            this.GbZPatchDependenciesFrom.Text = "Зависит от";
            // 
            // LboxZPatchDependenciesFrom
            // 
            this.LboxZPatchDependenciesFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LboxZPatchDependenciesFrom.FormattingEnabled = true;
            this.LboxZPatchDependenciesFrom.ItemHeight = 16;
            this.LboxZPatchDependenciesFrom.Location = new System.Drawing.Point(3, 18);
            this.LboxZPatchDependenciesFrom.Name = "LboxZPatchDependenciesFrom";
            this.LboxZPatchDependenciesFrom.Size = new System.Drawing.Size(188, 269);
            this.LboxZPatchDependenciesFrom.TabIndex = 0;
            // 
            // BtZPatchAddDependenciesFrom
            // 
            this.BtZPatchAddDependenciesFrom.Location = new System.Drawing.Point(6, 41);
            this.BtZPatchAddDependenciesFrom.Name = "BtZPatchAddDependenciesFrom";
            this.BtZPatchAddDependenciesFrom.Size = new System.Drawing.Size(178, 23);
            this.BtZPatchAddDependenciesFrom.TabIndex = 3;
            this.BtZPatchAddDependenciesFrom.Text = "Добавить зависимости";
            this.BtZPatchAddDependenciesFrom.UseVisualStyleBackColor = true;
            // 
            // BtZPatchDeleteDependenciesFrom
            // 
            this.BtZPatchDeleteDependenciesFrom.Location = new System.Drawing.Point(6, 12);
            this.BtZPatchDeleteDependenciesFrom.Name = "BtZPatchDeleteDependenciesFrom";
            this.BtZPatchDeleteDependenciesFrom.Size = new System.Drawing.Size(178, 23);
            this.BtZPatchDeleteDependenciesFrom.TabIndex = 2;
            this.BtZPatchDeleteDependenciesFrom.Text = "Удалить выбранные";
            this.BtZPatchDeleteDependenciesFrom.UseVisualStyleBackColor = true;
            // 
            // SCZPatchDependenciesTo
            // 
            this.SCZPatchDependenciesTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCZPatchDependenciesTo.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SCZPatchDependenciesTo.IsSplitterFixed = true;
            this.SCZPatchDependenciesTo.Location = new System.Drawing.Point(0, 0);
            this.SCZPatchDependenciesTo.Name = "SCZPatchDependenciesTo";
            this.SCZPatchDependenciesTo.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SCZPatchDependenciesTo.Panel1
            // 
            this.SCZPatchDependenciesTo.Panel1.Controls.Add(this.GbZPatchDependenciesTo);
            // 
            // SCZPatchDependenciesTo.Panel2
            // 
            this.SCZPatchDependenciesTo.Panel2.Controls.Add(this.BtZPatchDeleteDependenciesTo);
            this.SCZPatchDependenciesTo.Panel2.Controls.Add(this.BtZPatchAddDependenciesTo);
            this.SCZPatchDependenciesTo.Size = new System.Drawing.Size(197, 394);
            this.SCZPatchDependenciesTo.SplitterDistance = 290;
            this.SCZPatchDependenciesTo.TabIndex = 5;
            // 
            // GbZPatchDependenciesTo
            // 
            this.GbZPatchDependenciesTo.Controls.Add(this.LboxZPatchDependenciesTo);
            this.GbZPatchDependenciesTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbZPatchDependenciesTo.Location = new System.Drawing.Point(0, 0);
            this.GbZPatchDependenciesTo.Name = "GbZPatchDependenciesTo";
            this.GbZPatchDependenciesTo.Size = new System.Drawing.Size(197, 290);
            this.GbZPatchDependenciesTo.TabIndex = 1;
            this.GbZPatchDependenciesTo.TabStop = false;
            this.GbZPatchDependenciesTo.Text = "Влияет на";
            // 
            // LboxZPatchDependenciesTo
            // 
            this.LboxZPatchDependenciesTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LboxZPatchDependenciesTo.FormattingEnabled = true;
            this.LboxZPatchDependenciesTo.ItemHeight = 16;
            this.LboxZPatchDependenciesTo.Location = new System.Drawing.Point(3, 18);
            this.LboxZPatchDependenciesTo.Name = "LboxZPatchDependenciesTo";
            this.LboxZPatchDependenciesTo.Size = new System.Drawing.Size(191, 269);
            this.LboxZPatchDependenciesTo.TabIndex = 0;
            // 
            // BtZPatchDeleteDependenciesTo
            // 
            this.BtZPatchDeleteDependenciesTo.Location = new System.Drawing.Point(6, 41);
            this.BtZPatchDeleteDependenciesTo.Name = "BtZPatchDeleteDependenciesTo";
            this.BtZPatchDeleteDependenciesTo.Size = new System.Drawing.Size(178, 23);
            this.BtZPatchDeleteDependenciesTo.TabIndex = 3;
            this.BtZPatchDeleteDependenciesTo.Text = "Добавить зависимости";
            this.BtZPatchDeleteDependenciesTo.UseVisualStyleBackColor = true;
            // 
            // BtZPatchAddDependenciesTo
            // 
            this.BtZPatchAddDependenciesTo.Location = new System.Drawing.Point(6, 12);
            this.BtZPatchAddDependenciesTo.Name = "BtZPatchAddDependenciesTo";
            this.BtZPatchAddDependenciesTo.Size = new System.Drawing.Size(178, 23);
            this.BtZPatchAddDependenciesTo.TabIndex = 2;
            this.BtZPatchAddDependenciesTo.Text = "Удалить выбранные";
            this.BtZPatchAddDependenciesTo.UseVisualStyleBackColor = true;
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
            // BtDeleteRelease
            // 
            this.BtDeleteRelease.Name = "BtDeleteRelease";
            this.BtDeleteRelease.Size = new System.Drawing.Size(216, 26);
            this.BtDeleteRelease.Text = "Удалить";
            this.BtDeleteRelease.Click += new System.EventHandler(this.BtDeleteRelease_Click);
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
            this.SCCPatchDependencies.Panel1.ResumeLayout(false);
            this.SCCPatchDependencies.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCCPatchDependencies)).EndInit();
            this.SCCPatchDependencies.ResumeLayout(false);
            this.SCCPatchDependenciesFrom.Panel1.ResumeLayout(false);
            this.SCCPatchDependenciesFrom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCCPatchDependenciesFrom)).EndInit();
            this.SCCPatchDependenciesFrom.ResumeLayout(false);
            this.GbCPatchDependenciesFrom.ResumeLayout(false);
            this.SCCPatchDependenciesTo.Panel1.ResumeLayout(false);
            this.SCCPatchDependenciesTo.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCCPatchDependenciesTo)).EndInit();
            this.SCCPatchDependenciesTo.ResumeLayout(false);
            this.GbCPatchDependenciesTo.ResumeLayout(false);
            this.GbZPatch.ResumeLayout(false);
            this.SCZPatch.Panel1.ResumeLayout(false);
            this.SCZPatch.Panel1.PerformLayout();
            this.SCZPatch.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCZPatch)).EndInit();
            this.SCZPatch.ResumeLayout(false);
            this.SCZPatchDependencies.Panel1.ResumeLayout(false);
            this.SCZPatchDependencies.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCZPatchDependencies)).EndInit();
            this.SCZPatchDependencies.ResumeLayout(false);
            this.SCZPatchDependenciesFrom.Panel1.ResumeLayout(false);
            this.SCZPatchDependenciesFrom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCZPatchDependenciesFrom)).EndInit();
            this.SCZPatchDependenciesFrom.ResumeLayout(false);
            this.GbZPatchDependenciesFrom.ResumeLayout(false);
            this.SCZPatchDependenciesTo.Panel1.ResumeLayout(false);
            this.SCZPatchDependenciesTo.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCZPatchDependenciesTo)).EndInit();
            this.SCZPatchDependenciesTo.ResumeLayout(false);
            this.GbZPatchDependenciesTo.ResumeLayout(false);
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
        private System.Windows.Forms.SplitContainer SCCPatchDependencies;
        private System.Windows.Forms.Button BtCPatchStatus;
        private System.Windows.Forms.ComboBox CbCPatchStatus;
        private System.Windows.Forms.Label LbCPatchStatus;
        private System.Windows.Forms.Button BtCPatchAddDependenciesFrom;
        private System.Windows.Forms.Button BtCPatchDeleteDependenciesFrom;
        private System.Windows.Forms.ListBox LboxCPatchDependenciesFrom;
        private System.Windows.Forms.Button BtCPatchRelease;
        private System.Windows.Forms.ComboBox CbCPatchRelease;
        private System.Windows.Forms.Label LbCPatchRelease;
        private System.Windows.Forms.ContextMenuStrip mainTreeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem TSMIRename;
        private System.Windows.Forms.SplitContainer SCCPatchDependenciesFrom;
        private System.Windows.Forms.SplitContainer SCCPatchDependenciesTo;
        private System.Windows.Forms.ListBox LboxCPatchDependenciesTo;
        private System.Windows.Forms.Button BtCPatchAddDependenciesTo;
        private System.Windows.Forms.Button BtCPatchDeleteDependenciesTo;
        private System.Windows.Forms.GroupBox GbZPatch;
        private System.Windows.Forms.SplitContainer SCZPatch;
        private System.Windows.Forms.Button BtZPatchCPatch;
        private System.Windows.Forms.ComboBox CbZPatchCPatch;
        private System.Windows.Forms.Label LbCPatch;
        private System.Windows.Forms.Button BtZPatchStatus;
        private System.Windows.Forms.ComboBox CbZPatchStatus;
        private System.Windows.Forms.Label LbZPatchStatus;
        private System.Windows.Forms.SplitContainer SCZPatchDependencies;
        private System.Windows.Forms.SplitContainer SCZPatchDependenciesFrom;
        private System.Windows.Forms.ListBox LboxZPatchDependenciesFrom;
        private System.Windows.Forms.Button BtZPatchAddDependenciesFrom;
        private System.Windows.Forms.Button BtZPatchDeleteDependenciesFrom;
        private System.Windows.Forms.SplitContainer SCZPatchDependenciesTo;
        private System.Windows.Forms.ListBox LboxZPatchDependenciesTo;
        private System.Windows.Forms.Button BtZPatchDeleteDependenciesTo;
        private System.Windows.Forms.Button BtZPatchAddDependenciesTo;
        private System.Windows.Forms.ToolStripMenuItem BtReleaseGraph;
        private System.Windows.Forms.ToolStripMenuItem BtCPatchGraph;
        private System.Windows.Forms.GroupBox GbCPatchDependenciesFrom;
        private System.Windows.Forms.GroupBox GbCPatchDependenciesTo;
        private System.Windows.Forms.GroupBox GbZPatchDependenciesFrom;
        private System.Windows.Forms.GroupBox GbZPatchDependenciesTo;
        private System.Windows.Forms.ToolStripMenuItem BtZPatchOrder;
        private System.Windows.Forms.ToolStripMenuItem BtDeleteRelease;
    }
}