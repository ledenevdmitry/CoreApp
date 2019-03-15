namespace CoreApp
{
    partial class CheckReleaseForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvObjects = new System.Windows.Forms.DataGridView();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.настройкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIUmState = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.objPage = new System.Windows.Forms.TabPage();
            this.intersectionsPage = new System.Windows.Forms.TabPage();
            this.dgvIntersections = new System.Windows.Forms.DataGridView();
            this.allPrereq = new System.Windows.Forms.TabPage();
            this.dgvAllDependencies = new System.Windows.Forms.DataGridView();
            this.notFoundObjPage = new System.Windows.Forms.TabPage();
            this.dgvLostDependencies = new System.Windows.Forms.DataGridView();
            this.notFoundFiles = new System.Windows.Forms.TabPage();
            this.dgvNotFoundFiles = new System.Windows.Forms.DataGridView();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CheckAllCPatchesInDir = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvObjects)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.objPage.SuspendLayout();
            this.intersectionsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIntersections)).BeginInit();
            this.allPrereq.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllDependencies)).BeginInit();
            this.notFoundObjPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLostDependencies)).BeginInit();
            this.notFoundFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotFoundFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvObjects
            // 
            this.dgvObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvObjects.Location = new System.Drawing.Point(0, 0);
            this.dgvObjects.Name = "dgvObjects";
            this.dgvObjects.RowHeadersVisible = false;
            this.dgvObjects.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvObjects.Size = new System.Drawing.Size(626, 374);
            this.dgvObjects.TabIndex = 0;
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.настройкаToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(634, 24);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "menuStrip1";
            // 
            // настройкаToolStripMenuItem
            // 
            this.настройкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIUmState});
            this.настройкаToolStripMenuItem.Name = "настройкаToolStripMenuItem";
            this.настройкаToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.настройкаToolStripMenuItem.Text = "Настройка";
            // 
            // TSMIUmState
            // 
            this.TSMIUmState.Name = "TSMIUmState";
            this.TSMIUmState.Size = new System.Drawing.Size(180, 22);
            this.TSMIUmState.Text = "Учитывать УМ";
            this.TSMIUmState.Click += new System.EventHandler(this.TSMIUmState_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.objPage);
            this.mainTabControl.Controls.Add(this.intersectionsPage);
            this.mainTabControl.Controls.Add(this.allPrereq);
            this.mainTabControl.Controls.Add(this.notFoundObjPage);
            this.mainTabControl.Controls.Add(this.notFoundFiles);
            this.mainTabControl.Location = new System.Drawing.Point(0, 27);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(634, 400);
            this.mainTabControl.TabIndex = 3;
            this.mainTabControl.SelectedIndexChanged += new System.EventHandler(this.mainTabControl_SelectedIndexChanged);
            // 
            // objPage
            // 
            this.objPage.Controls.Add(this.dgvObjects);
            this.objPage.Location = new System.Drawing.Point(4, 22);
            this.objPage.Name = "objPage";
            this.objPage.Padding = new System.Windows.Forms.Padding(3);
            this.objPage.Size = new System.Drawing.Size(626, 374);
            this.objPage.TabIndex = 0;
            this.objPage.Text = "Все объекты";
            this.objPage.UseVisualStyleBackColor = true;
            // 
            // intersectionsPage
            // 
            this.intersectionsPage.Controls.Add(this.dgvIntersections);
            this.intersectionsPage.Location = new System.Drawing.Point(4, 22);
            this.intersectionsPage.Name = "intersectionsPage";
            this.intersectionsPage.Padding = new System.Windows.Forms.Padding(3);
            this.intersectionsPage.Size = new System.Drawing.Size(626, 374);
            this.intersectionsPage.TabIndex = 1;
            this.intersectionsPage.Text = "Пересечения";
            this.intersectionsPage.UseVisualStyleBackColor = true;
            // 
            // dgvIntersections
            // 
            this.dgvIntersections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIntersections.Location = new System.Drawing.Point(0, 0);
            this.dgvIntersections.Name = "dgvIntersections";
            this.dgvIntersections.RowHeadersVisible = false;
            this.dgvIntersections.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvIntersections.Size = new System.Drawing.Size(626, 374);
            this.dgvIntersections.TabIndex = 1;
            // 
            // allPrereq
            // 
            this.allPrereq.Controls.Add(this.dgvAllDependencies);
            this.allPrereq.Location = new System.Drawing.Point(4, 22);
            this.allPrereq.Name = "allPrereq";
            this.allPrereq.Size = new System.Drawing.Size(626, 374);
            this.allPrereq.TabIndex = 2;
            this.allPrereq.Text = "Все пререквизиты";
            this.allPrereq.UseVisualStyleBackColor = true;
            // 
            // dgvAllDependencies
            // 
            this.dgvAllDependencies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllDependencies.Location = new System.Drawing.Point(0, 0);
            this.dgvAllDependencies.Name = "dgvAllDependencies";
            this.dgvAllDependencies.RowHeadersVisible = false;
            this.dgvAllDependencies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvAllDependencies.Size = new System.Drawing.Size(626, 374);
            this.dgvAllDependencies.TabIndex = 1;
            // 
            // notFoundObjPage
            // 
            this.notFoundObjPage.Controls.Add(this.dgvLostDependencies);
            this.notFoundObjPage.Location = new System.Drawing.Point(4, 22);
            this.notFoundObjPage.Name = "notFoundObjPage";
            this.notFoundObjPage.Size = new System.Drawing.Size(626, 374);
            this.notFoundObjPage.TabIndex = 3;
            this.notFoundObjPage.Text = "Ненайденные пререквизиты";
            this.notFoundObjPage.UseVisualStyleBackColor = true;
            // 
            // dgvLostDependencies
            // 
            this.dgvLostDependencies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLostDependencies.Location = new System.Drawing.Point(0, 0);
            this.dgvLostDependencies.Name = "dgvLostDependencies";
            this.dgvLostDependencies.RowHeadersVisible = false;
            this.dgvLostDependencies.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvLostDependencies.Size = new System.Drawing.Size(626, 374);
            this.dgvLostDependencies.TabIndex = 1;
            // 
            // notFoundFiles
            // 
            this.notFoundFiles.Controls.Add(this.dgvNotFoundFiles);
            this.notFoundFiles.Location = new System.Drawing.Point(4, 22);
            this.notFoundFiles.Name = "notFoundFiles";
            this.notFoundFiles.Size = new System.Drawing.Size(626, 374);
            this.notFoundFiles.TabIndex = 4;
            this.notFoundFiles.Text = "Ненайденные файлы";
            this.notFoundFiles.UseVisualStyleBackColor = true;
            // 
            // dgvNotFoundFiles
            // 
            this.dgvNotFoundFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotFoundFiles.Location = new System.Drawing.Point(0, 0);
            this.dgvNotFoundFiles.Name = "dgvNotFoundFiles";
            this.dgvNotFoundFiles.RowHeadersVisible = false;
            this.dgvNotFoundFiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvNotFoundFiles.Size = new System.Drawing.Size(626, 374);
            this.dgvNotFoundFiles.TabIndex = 2;
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckAllCPatchesInDir});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // CheckAllFixpacksInDir
            // 
            this.CheckAllCPatchesInDir.Name = "CheckAllFixpacksInDir";
            this.CheckAllCPatchesInDir.Size = new System.Drawing.Size(180, 22);
            this.CheckAllCPatchesInDir.Text = "Проверить";
            this.CheckAllCPatchesInDir.Click += new System.EventHandler(this.CheckAllCPatchesInDir_Click);
            // 
            // CheckReleaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 427);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "CheckReleaseForm";
            this.Text = "Объекты поставки";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgvObjects)).EndInit();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.mainTabControl.ResumeLayout(false);
            this.objPage.ResumeLayout(false);
            this.intersectionsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIntersections)).EndInit();
            this.allPrereq.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllDependencies)).EndInit();
            this.notFoundObjPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLostDependencies)).EndInit();
            this.notFoundFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotFoundFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvObjects;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage objPage;
        private System.Windows.Forms.TabPage intersectionsPage;
        private System.Windows.Forms.DataGridView dgvIntersections;
        private System.Windows.Forms.TabPage allPrereq;
        private System.Windows.Forms.DataGridView dgvAllDependencies;
        private System.Windows.Forms.TabPage notFoundObjPage;
        private System.Windows.Forms.DataGridView dgvLostDependencies;
        private System.Windows.Forms.TabPage notFoundFiles;
        private System.Windows.Forms.DataGridView dgvNotFoundFiles;
        private System.Windows.Forms.ToolStripMenuItem настройкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TSMIUmState;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CheckAllCPatchesInDir;
    }
}

