namespace CoreApp
{
    partial class ObjectForm
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
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PBChecks = new System.Windows.Forms.ProgressBar();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.objPage = new System.Windows.Forms.TabPage();
            this.intersectionsPage = new System.Windows.Forms.TabPage();
            this.dgvIntersections = new System.Windows.Forms.DataGridView();
            this.allPrereq = new System.Windows.Forms.TabPage();
            this.dgvWrongOrder = new System.Windows.Forms.DataGridView();
            this.notFoundObjPage = new System.Windows.Forms.TabPage();
            this.dgvNotFound = new System.Windows.Forms.DataGridView();
            this.notFoundFiles = new System.Windows.Forms.TabPage();
            this.dgvNotFoundFiles = new System.Windows.Forms.DataGridView();
            this.TSMICheckAllFixpacksInDir = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvObjects)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.objPage.SuspendLayout();
            this.intersectionsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIntersections)).BeginInit();
            this.allPrereq.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWrongOrder)).BeginInit();
            this.notFoundObjPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotFound)).BeginInit();
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
            this.файлToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(634, 24);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMICheckAllFixpacksInDir});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // PBChecks
            // 
            this.PBChecks.Location = new System.Drawing.Point(534, 0);
            this.PBChecks.Name = "PBChecks";
            this.PBChecks.Size = new System.Drawing.Size(100, 21);
            this.PBChecks.TabIndex = 2;
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
            this.allPrereq.Controls.Add(this.dgvWrongOrder);
            this.allPrereq.Location = new System.Drawing.Point(4, 22);
            this.allPrereq.Name = "allPrereq";
            this.allPrereq.Size = new System.Drawing.Size(626, 374);
            this.allPrereq.TabIndex = 2;
            this.allPrereq.Text = "Все пререквизиты";
            this.allPrereq.UseVisualStyleBackColor = true;
            // 
            // dgvWrongOrder
            // 
            this.dgvWrongOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWrongOrder.Location = new System.Drawing.Point(0, 0);
            this.dgvWrongOrder.Name = "dgvWrongOrder";
            this.dgvWrongOrder.RowHeadersVisible = false;
            this.dgvWrongOrder.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvWrongOrder.Size = new System.Drawing.Size(626, 374);
            this.dgvWrongOrder.TabIndex = 1;
            // 
            // notFoundObjPage
            // 
            this.notFoundObjPage.Controls.Add(this.dgvNotFound);
            this.notFoundObjPage.Location = new System.Drawing.Point(4, 22);
            this.notFoundObjPage.Name = "notFoundObjPage";
            this.notFoundObjPage.Size = new System.Drawing.Size(626, 374);
            this.notFoundObjPage.TabIndex = 3;
            this.notFoundObjPage.Text = "Ненайденные пререквизиты";
            this.notFoundObjPage.UseVisualStyleBackColor = true;
            // 
            // dgvNotFound
            // 
            this.dgvNotFound.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotFound.Location = new System.Drawing.Point(0, 0);
            this.dgvNotFound.Name = "dgvNotFound";
            this.dgvNotFound.RowHeadersVisible = false;
            this.dgvNotFound.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvNotFound.Size = new System.Drawing.Size(626, 374);
            this.dgvNotFound.TabIndex = 1;
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
            // TSMICheckAllFixpacksInDir
            // 
            this.TSMICheckAllFixpacksInDir.Name = "TSMICheckAllFixpacksInDir";
            this.TSMICheckAllFixpacksInDir.Size = new System.Drawing.Size(285, 22);
            this.TSMICheckAllFixpacksInDir.Text = "Проверить все поставки внутри папки";
            this.TSMICheckAllFixpacksInDir.Click += new System.EventHandler(this.CheckAllFixpacksInDir_Click);
            // 
            // ObjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 427);
            this.Controls.Add(this.mainTabControl);
            this.Controls.Add(this.PBChecks);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "ObjectForm";
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvWrongOrder)).EndInit();
            this.notFoundObjPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotFound)).EndInit();
            this.notFoundFiles.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotFoundFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvObjects;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ProgressBar PBChecks;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage objPage;
        private System.Windows.Forms.TabPage intersectionsPage;
        private System.Windows.Forms.DataGridView dgvIntersections;
        private System.Windows.Forms.TabPage allPrereq;
        private System.Windows.Forms.DataGridView dgvWrongOrder;
        private System.Windows.Forms.TabPage notFoundObjPage;
        private System.Windows.Forms.DataGridView dgvNotFound;
        private System.Windows.Forms.TabPage notFoundFiles;
        private System.Windows.Forms.DataGridView dgvNotFoundFiles;
        private System.Windows.Forms.ToolStripMenuItem TSMICheckAllFixpacksInDir;
    }
}

