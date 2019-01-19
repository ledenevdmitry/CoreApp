namespace CoreApp
{
    partial class ReleaseForm
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
            this.BtRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.BtAddRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.BtCheckRelease = new System.Windows.Forms.ToolStripMenuItem();
            this.BtFixpack = new System.Windows.Forms.ToolStripMenuItem();
            this.BtAddFixpack = new System.Windows.Forms.ToolStripMenuItem();
            this.LBoxReleases = new System.Windows.Forms.ListBox();
            this.LBoxFixpacks = new System.Windows.Forms.ListBox();
            this.LbReleases = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtRelease,
            this.BtFixpack});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(456, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "releaseMenu";
            // 
            // BtRelease
            // 
            this.BtRelease.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtAddRelease,
            this.BtCheckRelease,
            this.удалитьToolStripMenuItem});
            this.BtRelease.Name = "BtRelease";
            this.BtRelease.Size = new System.Drawing.Size(51, 20);
            this.BtRelease.Text = "Релиз";
            // 
            // BtAddRelease
            // 
            this.BtAddRelease.Name = "BtAddRelease";
            this.BtAddRelease.Size = new System.Drawing.Size(180, 22);
            this.BtAddRelease.Text = "Добавить";
            this.BtAddRelease.Click += new System.EventHandler(this.BtAddRelease_Click);
            // 
            // BtCheckRelease
            // 
            this.BtCheckRelease.Name = "BtCheckRelease";
            this.BtCheckRelease.Size = new System.Drawing.Size(180, 22);
            this.BtCheckRelease.Text = "Проверить";
            // 
            // BtFixpack
            // 
            this.BtFixpack.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtAddFixpack});
            this.BtFixpack.Name = "BtFixpack";
            this.BtFixpack.Size = new System.Drawing.Size(70, 20);
            this.BtFixpack.Text = "Поставка";
            // 
            // BtAddFixpack
            // 
            this.BtAddFixpack.Name = "BtAddFixpack";
            this.BtAddFixpack.Size = new System.Drawing.Size(180, 22);
            this.BtAddFixpack.Text = "Добавить ОП/ФП";
            this.BtAddFixpack.Click += new System.EventHandler(this.BtAddFixpack_Click);
            // 
            // LBoxReleases
            // 
            this.LBoxReleases.FormattingEnabled = true;
            this.LBoxReleases.Location = new System.Drawing.Point(12, 53);
            this.LBoxReleases.Name = "LBoxReleases";
            this.LBoxReleases.Size = new System.Drawing.Size(208, 381);
            this.LBoxReleases.TabIndex = 1;
            this.LBoxReleases.SelectedIndexChanged += new System.EventHandler(this.LBoxReleases_SelectedIndexChanged);
            // 
            // LBoxFixpacks
            // 
            this.LBoxFixpacks.FormattingEnabled = true;
            this.LBoxFixpacks.Location = new System.Drawing.Point(235, 53);
            this.LBoxFixpacks.Name = "LBoxFixpacks";
            this.LBoxFixpacks.Size = new System.Drawing.Size(208, 381);
            this.LBoxFixpacks.TabIndex = 2;
            // 
            // LbReleases
            // 
            this.LbReleases.AutoSize = true;
            this.LbReleases.Location = new System.Drawing.Point(12, 37);
            this.LbReleases.Name = "LbReleases";
            this.LbReleases.Size = new System.Drawing.Size(46, 13);
            this.LbReleases.TabIndex = 3;
            this.LbReleases.Text = "Релизы";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(232, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Фикспаки/поставки";
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            // 
            // ReleaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LbReleases);
            this.Controls.Add(this.LBoxFixpacks);
            this.Controls.Add(this.LBoxReleases);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ReleaseForm";
            this.Text = "ReleaseForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReleaseForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem BtRelease;
        private System.Windows.Forms.ToolStripMenuItem BtFixpack;
        private System.Windows.Forms.ListBox LBoxReleases;
        private System.Windows.Forms.ListBox LBoxFixpacks;
        private System.Windows.Forms.Label LbReleases;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem BtAddRelease;
        private System.Windows.Forms.ToolStripMenuItem BtCheckRelease;
        private System.Windows.Forms.ToolStripMenuItem BtAddFixpack;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
    }
}