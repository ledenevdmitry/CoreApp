namespace CoreApp.Forms
{
    partial class ScenarioForm
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
            this.SCMain = new System.Windows.Forms.SplitContainer();
            this.BtDown = new System.Windows.Forms.Button();
            this.BtUp = new System.Windows.Forms.Button();
            this.BtDeleteLine = new System.Windows.Forms.Button();
            this.BtSave = new System.Windows.Forms.Button();
            this.LViewScenarioLines = new System.Windows.Forms.ListView();
            this.mainColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PbNormal = new System.Windows.Forms.PictureBox();
            this.LbNormal = new System.Windows.Forms.Label();
            this.LbNotInFiles = new System.Windows.Forms.Label();
            this.PbNotInFiles = new System.Windows.Forms.PictureBox();
            this.LbNotInScenario = new System.Windows.Forms.Label();
            this.PbNotInScenario = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).BeginInit();
            this.SCMain.Panel1.SuspendLayout();
            this.SCMain.Panel2.SuspendLayout();
            this.SCMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInScenario)).BeginInit();
            this.SuspendLayout();
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
            this.SCMain.Panel1.Controls.Add(this.LViewScenarioLines);
            // 
            // SCMain.Panel2
            // 
            this.SCMain.Panel2.Controls.Add(this.LbNotInScenario);
            this.SCMain.Panel2.Controls.Add(this.PbNotInScenario);
            this.SCMain.Panel2.Controls.Add(this.LbNotInFiles);
            this.SCMain.Panel2.Controls.Add(this.PbNotInFiles);
            this.SCMain.Panel2.Controls.Add(this.LbNormal);
            this.SCMain.Panel2.Controls.Add(this.PbNormal);
            this.SCMain.Panel2.Controls.Add(this.BtSave);
            this.SCMain.Panel2.Controls.Add(this.BtDeleteLine);
            this.SCMain.Panel2.Controls.Add(this.BtDown);
            this.SCMain.Panel2.Controls.Add(this.BtUp);
            this.SCMain.Size = new System.Drawing.Size(496, 376);
            this.SCMain.SplitterDistance = 333;
            this.SCMain.TabIndex = 0;
            // 
            // BtDown
            // 
            this.BtDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtDown.Location = new System.Drawing.Point(49, 60);
            this.BtDown.Margin = new System.Windows.Forms.Padding(2);
            this.BtDown.Name = "BtDown";
            this.BtDown.Size = new System.Drawing.Size(51, 44);
            this.BtDown.TabIndex = 5;
            this.BtDown.Text = "↓";
            this.BtDown.UseVisualStyleBackColor = true;
            // 
            // BtUp
            // 
            this.BtUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtUp.Location = new System.Drawing.Point(49, 11);
            this.BtUp.Margin = new System.Windows.Forms.Padding(2);
            this.BtUp.Name = "BtUp";
            this.BtUp.Size = new System.Drawing.Size(51, 44);
            this.BtUp.TabIndex = 4;
            this.BtUp.Text = "↑";
            this.BtUp.UseVisualStyleBackColor = true;
            this.BtUp.Click += new System.EventHandler(this.BtUp_Click);
            // 
            // BtDeleteLine
            // 
            this.BtDeleteLine.Location = new System.Drawing.Point(22, 127);
            this.BtDeleteLine.Name = "BtDeleteLine";
            this.BtDeleteLine.Size = new System.Drawing.Size(107, 23);
            this.BtDeleteLine.TabIndex = 6;
            this.BtDeleteLine.Text = "Удалить строку";
            this.BtDeleteLine.UseVisualStyleBackColor = true;
            // 
            // BtSave
            // 
            this.BtSave.Location = new System.Drawing.Point(22, 156);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(107, 23);
            this.BtSave.TabIndex = 7;
            this.BtSave.Text = "Сохранить";
            this.BtSave.UseVisualStyleBackColor = true;
            // 
            // LViewScenarioLines
            // 
            this.LViewScenarioLines.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.LViewScenarioLines.AutoArrange = false;
            this.LViewScenarioLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.mainColumn});
            this.LViewScenarioLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LViewScenarioLines.Location = new System.Drawing.Point(0, 0);
            this.LViewScenarioLines.Name = "LViewScenarioLines";
            this.LViewScenarioLines.Size = new System.Drawing.Size(333, 376);
            this.LViewScenarioLines.TabIndex = 0;
            this.LViewScenarioLines.UseCompatibleStateImageBehavior = false;
            this.LViewScenarioLines.View = System.Windows.Forms.View.Details;
            // 
            // mainColumn
            // 
            this.mainColumn.Text = "Строки сценария";
            // 
            // PbNormal
            // 
            this.PbNormal.Location = new System.Drawing.Point(6, 206);
            this.PbNormal.Name = "PbNormal";
            this.PbNormal.Size = new System.Drawing.Size(18, 13);
            this.PbNormal.TabIndex = 8;
            this.PbNormal.TabStop = false;
            // 
            // LbNormal
            // 
            this.LbNormal.AutoSize = true;
            this.LbNormal.Location = new System.Drawing.Point(30, 206);
            this.LbNormal.Name = "LbNormal";
            this.LbNormal.Size = new System.Drawing.Size(80, 13);
            this.LbNormal.TabIndex = 9;
            this.LbNormal.Text = "Все в порядке";
            // 
            // LbNotInFiles
            // 
            this.LbNotInFiles.AutoSize = true;
            this.LbNotInFiles.Location = new System.Drawing.Point(30, 225);
            this.LbNotInFiles.Name = "LbNotInFiles";
            this.LbNotInFiles.Size = new System.Drawing.Size(112, 13);
            this.LbNotInFiles.TabIndex = 11;
            this.LbNotInFiles.Text = "Строки нет в файлах";
            // 
            // PbNotInFiles
            // 
            this.PbNotInFiles.Location = new System.Drawing.Point(6, 225);
            this.PbNotInFiles.Name = "PbNotInFiles";
            this.PbNotInFiles.Size = new System.Drawing.Size(18, 13);
            this.PbNotInFiles.TabIndex = 10;
            this.PbNotInFiles.TabStop = false;
            // 
            // LbNotInScenario
            // 
            this.LbNotInScenario.AutoSize = true;
            this.LbNotInScenario.Location = new System.Drawing.Point(30, 244);
            this.LbNotInScenario.Name = "LbNotInScenario";
            this.LbNotInScenario.Size = new System.Drawing.Size(122, 13);
            this.LbNotInScenario.TabIndex = 13;
            this.LbNotInScenario.Text = "Файла нет в сценарии";
            // 
            // PbNotInScenario
            // 
            this.PbNotInScenario.Location = new System.Drawing.Point(6, 244);
            this.PbNotInScenario.Name = "PbNotInScenario";
            this.PbNotInScenario.Size = new System.Drawing.Size(18, 13);
            this.PbNotInScenario.TabIndex = 12;
            this.PbNotInScenario.TabStop = false;
            // 
            // ScenarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 376);
            this.Controls.Add(this.SCMain);
            this.Name = "ScenarioForm";
            this.Text = "Сценарий";
            this.Resize += new System.EventHandler(this.ScenarioForm_Resize);
            this.SCMain.Panel1.ResumeLayout(false);
            this.SCMain.Panel2.ResumeLayout(false);
            this.SCMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).EndInit();
            this.SCMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInScenario)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SCMain;
        private System.Windows.Forms.Button BtSave;
        private System.Windows.Forms.Button BtDeleteLine;
        private System.Windows.Forms.Button BtDown;
        private System.Windows.Forms.Button BtUp;
        private System.Windows.Forms.ListView LViewScenarioLines;
        private System.Windows.Forms.ColumnHeader mainColumn;
        private System.Windows.Forms.Label LbNotInScenario;
        private System.Windows.Forms.PictureBox PbNotInScenario;
        private System.Windows.Forms.Label LbNotInFiles;
        private System.Windows.Forms.PictureBox PbNotInFiles;
        private System.Windows.Forms.Label LbNormal;
        private System.Windows.Forms.PictureBox PbNormal;
    }
}