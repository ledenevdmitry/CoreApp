﻿namespace CoreApp
{
    partial class ZPatchesListForm
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
            this.LboxZPatches = new System.Windows.Forms.ListBox();
            this.BtConfirm = new System.Windows.Forms.Button();
            this.SCMain = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).BeginInit();
            this.SCMain.Panel1.SuspendLayout();
            this.SCMain.Panel2.SuspendLayout();
            this.SCMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // LboxZPatches
            // 
            this.LboxZPatches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LboxZPatches.FormattingEnabled = true;
            this.LboxZPatches.Location = new System.Drawing.Point(0, 0);
            this.LboxZPatches.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.LboxZPatches.Name = "LboxZPatches";
            this.LboxZPatches.Size = new System.Drawing.Size(464, 366);
            this.LboxZPatches.TabIndex = 0;
            // 
            // BtConfirm
            // 
            this.BtConfirm.Location = new System.Drawing.Point(9, 157);
            this.BtConfirm.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BtConfirm.Name = "BtConfirm";
            this.BtConfirm.Size = new System.Drawing.Size(82, 49);
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
            this.SCMain.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SCMain.Name = "SCMain";
            // 
            // SCMain.Panel1
            // 
            this.SCMain.Panel1.Controls.Add(this.LboxZPatches);
            // 
            // SCMain.Panel2
            // 
            this.SCMain.Panel2.Controls.Add(this.BtConfirm);
            this.SCMain.Size = new System.Drawing.Size(600, 366);
            this.SCMain.SplitterDistance = 464;
            this.SCMain.SplitterWidth = 3;
            this.SCMain.TabIndex = 1;
            // 
            // ZPatchesListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.SCMain);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ZPatchesListForm";
            this.Text = "Список патчей";
            this.SCMain.Panel1.ResumeLayout(false);
            this.SCMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).EndInit();
            this.SCMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox LboxZPatches;
        private System.Windows.Forms.Button BtConfirm;
        private System.Windows.Forms.SplitContainer SCMain;
    }
}