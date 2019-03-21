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
            this.LbOldScenario = new System.Windows.Forms.Label();
            this.PbOldScenario = new System.Windows.Forms.PictureBox();
            this.BtAppendByNewPatches = new System.Windows.Forms.Button();
            this.BtLoadToCVS = new System.Windows.Forms.Button();
            this.LbNotInScenario = new System.Windows.Forms.Label();
            this.PbNotInScenario = new System.Windows.Forms.PictureBox();
            this.LbNotInFiles = new System.Windows.Forms.Label();
            this.PbNotInFiles = new System.Windows.Forms.PictureBox();
            this.LbNewScenarioNormal = new System.Windows.Forms.Label();
            this.PbNewScenarioNormal = new System.Windows.Forms.PictureBox();
            this.BtSave = new System.Windows.Forms.Button();
            this.BtDeleteLines = new System.Windows.Forms.Button();
            this.BtDown = new System.Windows.Forms.Button();
            this.BtUp = new System.Windows.Forms.Button();
            this.LbOnlyInZPatchScenario = new System.Windows.Forms.Label();
            this.PbOnlyInZPatchScenario = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).BeginInit();
            this.SCMain.Panel1.SuspendLayout();
            this.SCMain.Panel2.SuspendLayout();
            this.SCMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbOldScenario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInScenario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNewScenarioNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbOnlyInZPatchScenario)).BeginInit();
            this.SuspendLayout();
            // 
            // SCMain
            // 
            this.SCMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SCMain.Location = new System.Drawing.Point(0, 0);
            this.SCMain.Margin = new System.Windows.Forms.Padding(4);
            this.SCMain.Name = "SCMain";
            // 
            // SCMain.Panel1
            // 
            this.SCMain.Panel1.Controls.Add(this.LViewScenarioLines);
            // 
            // SCMain.Panel2
            // 
            this.SCMain.Panel2.Controls.Add(this.LbOnlyInZPatchScenario);
            this.SCMain.Panel2.Controls.Add(this.PbOnlyInZPatchScenario);
            this.SCMain.Panel2.Controls.Add(this.LbOldScenario);
            this.SCMain.Panel2.Controls.Add(this.PbOldScenario);
            this.SCMain.Panel2.Controls.Add(this.BtAppendByNewPatches);
            this.SCMain.Panel2.Controls.Add(this.BtLoadToCVS);
            this.SCMain.Panel2.Controls.Add(this.LbNotInScenario);
            this.SCMain.Panel2.Controls.Add(this.PbNotInScenario);
            this.SCMain.Panel2.Controls.Add(this.LbNotInFiles);
            this.SCMain.Panel2.Controls.Add(this.PbNotInFiles);
            this.SCMain.Panel2.Controls.Add(this.LbNewScenarioNormal);
            this.SCMain.Panel2.Controls.Add(this.PbNewScenarioNormal);
            this.SCMain.Panel2.Controls.Add(this.BtSave);
            this.SCMain.Panel2.Controls.Add(this.BtDeleteLines);
            this.SCMain.Panel2.Controls.Add(this.BtDown);
            this.SCMain.Panel2.Controls.Add(this.BtUp);
            this.SCMain.Size = new System.Drawing.Size(713, 536);
            this.SCMain.SplitterDistance = 462;
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
            this.LViewScenarioLines.Margin = new System.Windows.Forms.Padding(4);
            this.LViewScenarioLines.Name = "LViewScenarioLines";
            this.LViewScenarioLines.Size = new System.Drawing.Size(462, 536);
            this.LViewScenarioLines.TabIndex = 0;
            this.LViewScenarioLines.UseCompatibleStateImageBehavior = false;
            this.LViewScenarioLines.View = System.Windows.Forms.View.Details;
            // 
            // mainColumn
            // 
            this.mainColumn.Text = "Строки сценария";
            // 
            // LbOldScenario
            // 
            this.LbOldScenario.AutoSize = true;
            this.LbOldScenario.Location = new System.Drawing.Point(41, 404);
            this.LbOldScenario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbOldScenario.Name = "LbOldScenario";
            this.LbOldScenario.Size = new System.Drawing.Size(125, 17);
            this.LbOldScenario.TabIndex = 17;
            this.LbOldScenario.Text = "Старый сценарий";
            // 
            // PbOldScenario
            // 
            this.PbOldScenario.Location = new System.Drawing.Point(9, 404);
            this.PbOldScenario.Margin = new System.Windows.Forms.Padding(4);
            this.PbOldScenario.Name = "PbOldScenario";
            this.PbOldScenario.Size = new System.Drawing.Size(24, 16);
            this.PbOldScenario.TabIndex = 16;
            this.PbOldScenario.TabStop = false;
            // 
            // BtAppendByNewPatches
            // 
            this.BtAppendByNewPatches.Location = new System.Drawing.Point(32, 186);
            this.BtAppendByNewPatches.Margin = new System.Windows.Forms.Padding(4);
            this.BtAppendByNewPatches.Name = "BtAppendByNewPatches";
            this.BtAppendByNewPatches.Size = new System.Drawing.Size(155, 50);
            this.BtAppendByNewPatches.TabIndex = 15;
            this.BtAppendByNewPatches.Text = "Добавить в конец по другим патчам";
            this.BtAppendByNewPatches.UseVisualStyleBackColor = true;
            // 
            // BtLoadToCVS
            // 
            this.BtLoadToCVS.Location = new System.Drawing.Point(32, 280);
            this.BtLoadToCVS.Margin = new System.Windows.Forms.Padding(4);
            this.BtLoadToCVS.Name = "BtLoadToCVS";
            this.BtLoadToCVS.Size = new System.Drawing.Size(155, 28);
            this.BtLoadToCVS.TabIndex = 14;
            this.BtLoadToCVS.Text = "Внести в СКВ";
            this.BtLoadToCVS.UseVisualStyleBackColor = true;
            this.BtLoadToCVS.Click += new System.EventHandler(this.BtLoadToCVS_Click);
            // 
            // LbNotInScenario
            // 
            this.LbNotInScenario.AutoSize = true;
            this.LbNotInScenario.Location = new System.Drawing.Point(41, 380);
            this.LbNotInScenario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbNotInScenario.Name = "LbNotInScenario";
            this.LbNotInScenario.Size = new System.Drawing.Size(158, 17);
            this.LbNotInScenario.TabIndex = 13;
            this.LbNotInScenario.Text = "Файла нет в сценарии";
            // 
            // PbNotInScenario
            // 
            this.PbNotInScenario.Location = new System.Drawing.Point(9, 380);
            this.PbNotInScenario.Margin = new System.Windows.Forms.Padding(4);
            this.PbNotInScenario.Name = "PbNotInScenario";
            this.PbNotInScenario.Size = new System.Drawing.Size(24, 16);
            this.PbNotInScenario.TabIndex = 12;
            this.PbNotInScenario.TabStop = false;
            // 
            // LbNotInFiles
            // 
            this.LbNotInFiles.AutoSize = true;
            this.LbNotInFiles.Location = new System.Drawing.Point(41, 357);
            this.LbNotInFiles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbNotInFiles.Name = "LbNotInFiles";
            this.LbNotInFiles.Size = new System.Drawing.Size(146, 17);
            this.LbNotInFiles.TabIndex = 11;
            this.LbNotInFiles.Text = "Строки нет в файлах";
            // 
            // PbNotInFiles
            // 
            this.PbNotInFiles.Location = new System.Drawing.Point(9, 357);
            this.PbNotInFiles.Margin = new System.Windows.Forms.Padding(4);
            this.PbNotInFiles.Name = "PbNotInFiles";
            this.PbNotInFiles.Size = new System.Drawing.Size(24, 16);
            this.PbNotInFiles.TabIndex = 10;
            this.PbNotInFiles.TabStop = false;
            // 
            // LbNewScenarioNormal
            // 
            this.LbNewScenarioNormal.AutoSize = true;
            this.LbNewScenarioNormal.Location = new System.Drawing.Point(41, 334);
            this.LbNewScenarioNormal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbNewScenarioNormal.Name = "LbNewScenarioNormal";
            this.LbNewScenarioNormal.Size = new System.Drawing.Size(102, 17);
            this.LbNewScenarioNormal.TabIndex = 9;
            this.LbNewScenarioNormal.Text = "Все в порядке";
            // 
            // PbNewScenarioNormal
            // 
            this.PbNewScenarioNormal.Location = new System.Drawing.Point(9, 334);
            this.PbNewScenarioNormal.Margin = new System.Windows.Forms.Padding(4);
            this.PbNewScenarioNormal.Name = "PbNewScenarioNormal";
            this.PbNewScenarioNormal.Size = new System.Drawing.Size(24, 16);
            this.PbNewScenarioNormal.TabIndex = 8;
            this.PbNewScenarioNormal.TabStop = false;
            // 
            // BtSave
            // 
            this.BtSave.Location = new System.Drawing.Point(32, 244);
            this.BtSave.Margin = new System.Windows.Forms.Padding(4);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(155, 28);
            this.BtSave.TabIndex = 7;
            this.BtSave.Text = "Сохранить локально";
            this.BtSave.UseVisualStyleBackColor = true;
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtDeleteLines
            // 
            this.BtDeleteLines.Location = new System.Drawing.Point(32, 150);
            this.BtDeleteLines.Margin = new System.Windows.Forms.Padding(4);
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
            // LbOnlyInZPatchScenario
            // 
            this.LbOnlyInZPatchScenario.AutoSize = true;
            this.LbOnlyInZPatchScenario.Location = new System.Drawing.Point(41, 428);
            this.LbOnlyInZPatchScenario.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LbOnlyInZPatchScenario.Name = "LbOnlyInZPatchScenario";
            this.LbOnlyInZPatchScenario.Size = new System.Drawing.Size(190, 17);
            this.LbOnlyInZPatchScenario.TabIndex = 19;
            this.LbOnlyInZPatchScenario.Text = "Только в сценарии Z-патча";
            // 
            // PbOnlyInZPatchScenario
            // 
            this.PbOnlyInZPatchScenario.Location = new System.Drawing.Point(9, 428);
            this.PbOnlyInZPatchScenario.Margin = new System.Windows.Forms.Padding(4);
            this.PbOnlyInZPatchScenario.Name = "PbOnlyInZPatchScenario";
            this.PbOnlyInZPatchScenario.Size = new System.Drawing.Size(24, 16);
            this.PbOnlyInZPatchScenario.TabIndex = 18;
            this.PbOnlyInZPatchScenario.TabStop = false;
            // 
            // ScenarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 536);
            this.Controls.Add(this.SCMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ScenarioForm";
            this.Text = "Сценарий";
            this.Resize += new System.EventHandler(this.ScenarioForm_Resize);
            this.SCMain.Panel1.ResumeLayout(false);
            this.SCMain.Panel2.ResumeLayout(false);
            this.SCMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).EndInit();
            this.SCMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbOldScenario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInScenario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNotInFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbNewScenarioNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbOnlyInZPatchScenario)).EndInit();
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
        private System.Windows.Forms.Label LbNewScenarioNormal;
        private System.Windows.Forms.PictureBox PbNewScenarioNormal;
        private System.Windows.Forms.Button BtLoadToCVS;
        private System.Windows.Forms.Button BtAppendByNewPatches;
        private System.Windows.Forms.Label LbOldScenario;
        private System.Windows.Forms.PictureBox PbOldScenario;
        private System.Windows.Forms.Label LbOnlyInZPatchScenario;
        private System.Windows.Forms.PictureBox PbOnlyInZPatchScenario;
    }
}