namespace CoreApp
{
    partial class ZPatchOrderForm
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
            this.LboxZPatchOrder = new System.Windows.Forms.ListBox();
            this.BtConfirm = new System.Windows.Forms.Button();
            this.BtDown = new System.Windows.Forms.Button();
            this.BtUp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).BeginInit();
            this.SCMain.Panel1.SuspendLayout();
            this.SCMain.Panel2.SuspendLayout();
            this.SCMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // SCMain
            // 
            this.SCMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SCMain.Location = new System.Drawing.Point(0, 0);
            this.SCMain.Margin = new System.Windows.Forms.Padding(2);
            this.SCMain.Name = "SCMain";
            // 
            // SCMain.Panel1
            // 
            this.SCMain.Panel1.Controls.Add(this.LboxZPatchOrder);
            // 
            // SCMain.Panel2
            // 
            this.SCMain.Panel2.Controls.Add(this.BtConfirm);
            this.SCMain.Panel2.Controls.Add(this.BtDown);
            this.SCMain.Panel2.Controls.Add(this.BtUp);
            this.SCMain.Size = new System.Drawing.Size(603, 366);
            this.SCMain.SplitterDistance = 401;
            this.SCMain.SplitterWidth = 3;
            this.SCMain.TabIndex = 0;
            // 
            // LboxZPatchOrder
            // 
            this.LboxZPatchOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LboxZPatchOrder.FormattingEnabled = true;
            this.LboxZPatchOrder.Location = new System.Drawing.Point(0, 0);
            this.LboxZPatchOrder.Margin = new System.Windows.Forms.Padding(2);
            this.LboxZPatchOrder.Name = "LboxZPatchOrder";
            this.LboxZPatchOrder.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.LboxZPatchOrder.Size = new System.Drawing.Size(401, 366);
            this.LboxZPatchOrder.TabIndex = 0;
            // 
            // BtConfirm
            // 
            this.BtConfirm.Location = new System.Drawing.Point(4, 222);
            this.BtConfirm.Margin = new System.Windows.Forms.Padding(2);
            this.BtConfirm.Name = "BtConfirm";
            this.BtConfirm.Size = new System.Drawing.Size(136, 53);
            this.BtConfirm.TabIndex = 2;
            this.BtConfirm.Text = "Подтвердить перенос";
            this.BtConfirm.UseVisualStyleBackColor = true;
            this.BtConfirm.Click += new System.EventHandler(this.BtConfirm_Click);
            // 
            // BtDown
            // 
            this.BtDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtDown.Location = new System.Drawing.Point(45, 97);
            this.BtDown.Margin = new System.Windows.Forms.Padding(2);
            this.BtDown.Name = "BtDown";
            this.BtDown.Size = new System.Drawing.Size(51, 44);
            this.BtDown.TabIndex = 1;
            this.BtDown.Text = "↓";
            this.BtDown.UseVisualStyleBackColor = true;
            this.BtDown.Click += new System.EventHandler(this.BtDown_Click);
            // 
            // BtUp
            // 
            this.BtUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtUp.Location = new System.Drawing.Point(45, 48);
            this.BtUp.Margin = new System.Windows.Forms.Padding(2);
            this.BtUp.Name = "BtUp";
            this.BtUp.Size = new System.Drawing.Size(51, 44);
            this.BtUp.TabIndex = 0;
            this.BtUp.Text = "↑";
            this.BtUp.UseVisualStyleBackColor = true;
            this.BtUp.Click += new System.EventHandler(this.BtUp_Click);
            // 
            // ZPatchOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 366);
            this.Controls.Add(this.SCMain);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ZPatchOrderForm";
            this.Text = "Порядок патчей";
            this.SCMain.Panel1.ResumeLayout(false);
            this.SCMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCMain)).EndInit();
            this.SCMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SCMain;
        private System.Windows.Forms.Button BtDown;
        private System.Windows.Forms.Button BtUp;
        private System.Windows.Forms.Button BtConfirm;
        private System.Windows.Forms.ListBox LboxZPatchOrder;
    }
}