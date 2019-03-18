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
            this.LViewScenarioLines = new System.Windows.Forms.ListView();
            this.mainColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LbNotInScenario = new System.Windows.Forms.Label();
            this.PbNotInScenario = new System.Windows.Forms.PictureBox();
            this.LbNotInFiles = new System.Windows.Forms.Label();
            this.PbNotInFiles = new System.Windows.Forms.PictureBox();
            this.LbNormal = new System.Windows.Forms.Label();
            this.PbNormal = new System.Windows.Forms.PictureBox();
            this.BtSave = new System.Windows.Forms.Button();
            this.BtDeleteLines = new System.Windows.Forms.Button();
            this.BtDown = new System.Windows.Forms.Button();
            this.BtUp = new System.Windows.Forms.Button();
            this.BtLoadToCVS = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).BeginInit();
            this.SCMain.Panel1.SuspendLayout();
            this.SCMain.Panel2.SuspendLayout();
            this.SCMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInScenario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNormal)).BeginInit();
            this.SuspendLayout();
            // 
            // SCMain
            // 
            this.SCMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SCMain.Location = new System.Drawing.Point(0, 0);
            this.SCMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SCMain.Name = "SCMain";
            // 
            // SCMain.Panel1
            // 
            this.SCMain.Panel1.Controls.Add(this.LViewScenarioLines);
            // 
            // SCMain.Panel2
            // 
            this.SCMain.Panel2.Controls.Add(this.BtLoadToCVS);
            this.SCMain.Panel2.Controls.Add(this.LbNotInScenario);
            this.SCMain.Panel2.Controls.Add(this.PbNotInScenario);
            this.SCMain.Panel2.Controls.Add(this.LbNotInFiles);
            this.SCMain.Panel2.Controls.Add(this.PbNotInFiles);
            this.SCMain.Panel2.Controls.Add(this.LbNormal);
            this.SCMain.Panel2.Controls.Add(this.PbNormal);
            this.SCMain.Panel2.Controls.Add(this.BtSave);
            this.SCMain.Panel2.Controls.Add(this.BtDeleteLines);
            this.SCMain.Panel2.Controls.Add(this.BtDown);
            this.SCMain.Panel2.Controls.Add(this.BtUp);
            this.SCMain.Size = new System.Drawing.Size(661, 463);
            this.SCMain.SplitterDistance = 450;
            this.SCMain.SplitterWidth = 5;
            this.SCMain.TabIndex = 0;
            // 
            // LViewScenarioLines
            // 
            this.LViewScenarioLines.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.LViewScenarioLines.AutoArrange = false;
            this.LViewScenarioLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.mainColumn});
            this.LViewScenarioLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LViewScenarioLines.HideSelection = false;
            this.LViewScenarioLines.Location = new System.Drawing.Point(0, 0);
            this.LViewScenarioLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.LViewScenarioLines.Name = "LViewScenarioLines";
            this.LViewScenarioLines.Size = new System.Drawing.Size(450, 463);
            this.LViewScenarioLines.TabIndex = 0;
            this.LViewScenarioLines.UseCompatibleStateImageBehavior = false;
            this.LViewScenarioLines.View = System.Windows.Forms.View.Details;
            // 
            // mainColumn
            // 
            this.mainColumn.Text = "Строки сценария";
            // 
            // LbNotInScenario
            // 
            this.LbNotInScenario.AutoSize = true;
            this.LbNotInScenario.Location = new System.Drawing.Point(41, 317);
            this.LbNotInScenario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbNotInScenario.Name = "LbNotInScenario";
            this.LbNotInScenario.Size = new System.Drawing.Size(158, 17);
            this.LbNotInScenario.TabIndex = 13;
            this.LbNotInScenario.Text = "Файла нет в сценарии";
            // 
            // PbNotInScenario
            // 
            this.PbNotInScenario.Location = new System.Drawing.Point(9, 317);
            this.PbNotInScenario.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PbNotInScenario.Name = "PbNotInScenario";
            this.PbNotInScenario.Size = new System.Drawing.Size(24, 16);
            this.PbNotInScenario.TabIndex = 12;
            this.PbNotInScenario.TabStop = false;
            // 
            // LbNotInFiles
            // 
            this.LbNotInFiles.AutoSize = true;
            this.LbNotInFiles.Location = new System.Drawing.Point(41, 294);
            this.LbNotInFiles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbNotInFiles.Name = "LbNotInFiles";
            this.LbNotInFiles.Size = new System.Drawing.Size(146, 17);
            this.LbNotInFiles.TabIndex = 11;
            this.LbNotInFiles.Text = "Строки нет в файлах";
            // 
            // PbNotInFiles
            // 
            this.PbNotInFiles.Location = new System.Drawing.Point(9, 294);
            this.PbNotInFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PbNotInFiles.Name = "PbNotInFiles";
            this.PbNotInFiles.Size = new System.Drawing.Size(24, 16);
            this.PbNotInFiles.TabIndex = 10;
            this.PbNotInFiles.TabStop = false;
            // 
            // LbNormal
            // 
            this.LbNormal.AutoSize = true;
            this.LbNormal.Location = new System.Drawing.Point(41, 271);
            this.LbNormal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbNormal.Name = "LbNormal";
            this.LbNormal.Size = new System.Drawing.Size(102, 17);
            this.LbNormal.TabIndex = 9;
            this.LbNormal.Text = "Все в порядке";
            // 
            // PbNormal
            // 
            this.PbNormal.Location = new System.Drawing.Point(9, 271);
            this.PbNormal.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PbNormal.Name = "PbNormal";
            this.PbNormal.Size = new System.Drawing.Size(24, 16);
            this.PbNormal.TabIndex = 8;
            this.PbNormal.TabStop = false;
            // 
            // BtSave
            // 
            this.BtSave.Location = new System.Drawing.Point(17, 192);
            this.BtSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(155, 28);
            this.BtSave.TabIndex = 7;
            this.BtSave.Text = "Сохранить локально";
            this.BtSave.UseVisualStyleBackColor = true;
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtDeleteLines
            // 
            this.BtDeleteLines.Location = new System.Drawing.Point(17, 156);
            this.BtDeleteLines.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtDeleteLines.Name = "BtDeleteLines";
            this.BtDeleteLines.Size = new System.Drawing.Size(155, 28);
            this.BtDeleteLines.TabIndex = 6;
            this.BtDeleteLines.Text = "Удалить строки";
            this.BtDeleteLines.UseVisualStyleBackColor = true;
            this.BtDeleteLines.Click += new System.EventHandler(this.BtDeleteLines_Click);
            // 
            // BtDown
            // 
            this.BtDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtDown.Location = new System.Drawing.Point(65, 74);
            this.BtDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtDown.Name = "BtDown";
            this.BtDown.Size = new System.Drawing.Size(68, 54);
            this.BtDown.TabIndex = 5;
            this.BtDown.Text = "↓";
            this.BtDown.UseVisualStyleBackColor = true;
            this.BtDown.Click += new System.EventHandler(this.BtDown_Click);
            // 
            // BtUp
            // 
            this.BtUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtUp.Location = new System.Drawing.Point(65, 14);
            this.BtUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtUp.Name = "BtUp";
            this.BtUp.Size = new System.Drawing.Size(68, 54);
            this.BtUp.TabIndex = 4;
            this.BtUp.Text = "↑";
            this.BtUp.UseVisualStyleBackColor = true;
            this.BtUp.Click += new System.EventHandler(this.BtUp_Click);
            // 
            // BtLoadToCVS
            // 
            this.BtLoadToCVS.Location = new System.Drawing.Point(17, 228);
            this.BtLoadToCVS.Margin = new System.Windows.Forms.Padding(4);
            this.BtLoadToCVS.Name = "BtLoadToCVS";
            this.BtLoadToCVS.Size = new System.Drawing.Size(155, 28);
            this.BtLoadToCVS.TabIndex = 14;
            this.BtLoadToCVS.Text = "Внести в СКВ";
            this.BtLoadToCVS.UseVisualStyleBackColor = true;
            this.BtLoadToCVS.Click += new System.EventHandler(this.BtLoadToCVS_Click);
            // 
            // ScenarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 463);
            this.Controls.Add(this.SCMain);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ScenarioForm";
            this.Text = "Сценарий";
            this.Resize += new System.EventHandler(this.ScenarioForm_Resize);
            this.SCMain.Panel1.ResumeLayout(false);
            this.SCMain.Panel2.ResumeLayout(false);
            this.SCMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).EndInit();
            this.SCMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInScenario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNormal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SCMain;
        private System.Windows.Forms.Button BtSave;
        private System.Windows.Forms.Button BtDeleteLines;
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
        private System.Windows.Forms.Button BtLoadToCVS;
    }
}