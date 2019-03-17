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
            this.SCMainPanel = new System.Windows.Forms.SplitContainer();
            this.BtConfirm = new System.Windows.Forms.Button();
            this.BtDown = new System.Windows.Forms.Button();
            this.BtUp = new System.Windows.Forms.Button();
            this.LboxZPatchOrder = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.SCMainPanel)).BeginInit();
            this.SCMainPanel.Panel1.SuspendLayout();
            this.SCMainPanel.Panel2.SuspendLayout();
            this.SCMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // SCMainPanel
            // 
            this.SCMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SCMainPanel.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SCMainPanel.Location = new System.Drawing.Point(0, 0);
            this.SCMainPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SCMainPanel.Name = "SCMainPanel";
            // 
            // SCMainPanel.Panel1
            // 
            this.SCMainPanel.Panel1.Controls.Add(this.LboxZPatchOrder);
            // 
            // SCMainPanel.Panel2
            // 
            this.SCMainPanel.Panel2.Controls.Add(this.BtConfirm);
            this.SCMainPanel.Panel2.Controls.Add(this.BtDown);
            this.SCMainPanel.Panel2.Controls.Add(this.BtUp);
            this.SCMainPanel.Size = new System.Drawing.Size(600, 366);
            this.SCMainPanel.SplitterDistance = 406;
            this.SCMainPanel.SplitterWidth = 3;
            this.SCMainPanel.TabIndex = 0;
            // 
            // BtConfirm
            // 
            this.BtConfirm.Location = new System.Drawing.Point(24, 302);
            this.BtConfirm.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BtConfirm.Name = "BtConfirm";
            this.BtConfirm.Size = new System.Drawing.Size(139, 53);
            this.BtConfirm.TabIndex = 2;
            this.BtConfirm.Text = "Подтвердить";
            this.BtConfirm.UseVisualStyleBackColor = true;
            this.BtConfirm.Click += new System.EventHandler(this.BtConfirm_Click);
            // 
            // BtDown
            // 
            this.BtDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtDown.Location = new System.Drawing.Point(65, 177);
            this.BtDown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.BtUp.Location = new System.Drawing.Point(65, 128);
            this.BtUp.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BtUp.Name = "BtUp";
            this.BtUp.Size = new System.Drawing.Size(51, 44);
            this.BtUp.TabIndex = 0;
            this.BtUp.Text = "↑";
            this.BtUp.UseVisualStyleBackColor = true;
            this.BtUp.Click += new System.EventHandler(this.BtUp_Click);
            // 
            // LboxZPatchOrder
            // 
            this.LboxZPatchOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LboxZPatchOrder.FormattingEnabled = true;
            this.LboxZPatchOrder.Location = new System.Drawing.Point(0, 0);
            this.LboxZPatchOrder.Margin = new System.Windows.Forms.Padding(2);
            this.LboxZPatchOrder.Name = "LboxZPatchOrder";
            this.LboxZPatchOrder.Size = new System.Drawing.Size(406, 366);
            this.LboxZPatchOrder.TabIndex = 0;
            // 
            // ZPatchOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.SCMainPanel);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ZPatchOrderForm";
            this.Text = "ZPatchOrderForm";
            this.SCMainPanel.Panel1.ResumeLayout(false);
            this.SCMainPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SCMainPanel)).EndInit();
            this.SCMainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer SCMainPanel;
        private System.Windows.Forms.Button BtDown;
        private System.Windows.Forms.Button BtUp;
        private System.Windows.Forms.Button BtConfirm;
        private System.Windows.Forms.ListBox LboxZPatchOrder;
    }
}