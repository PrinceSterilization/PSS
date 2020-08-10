namespace PSS
{
    partial class UpdateLogin
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvPONo = new System.Windows.Forms.DataGridView();
            this.dgvBillRef = new System.Windows.Forms.DataGridView();
            this.btnUpdatePO = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRefreshPO = new System.Windows.Forms.Button();
            this.btnRefreshBill = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPONo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillRef)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(23, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(940, 453);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnRefreshPO);
            this.tabPage1.Controls.Add(this.btnUpdatePO);
            this.tabPage1.Controls.Add(this.dgvPONo);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(932, 427);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "PO Number";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnRefreshBill);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.dgvBillRef);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(932, 427);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Billing Reference";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvPONo
            // 
            this.dgvPONo.BackgroundColor = System.Drawing.Color.White;
            this.dgvPONo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPONo.Location = new System.Drawing.Point(17, 17);
            this.dgvPONo.Name = "dgvPONo";
            this.dgvPONo.Size = new System.Drawing.Size(891, 342);
            this.dgvPONo.TabIndex = 0;
            // 
            // dgvBillRef
            // 
            this.dgvBillRef.BackgroundColor = System.Drawing.Color.White;
            this.dgvBillRef.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBillRef.Location = new System.Drawing.Point(21, 19);
            this.dgvBillRef.Name = "dgvBillRef";
            this.dgvBillRef.Size = new System.Drawing.Size(888, 343);
            this.dgvBillRef.TabIndex = 0;
            // 
            // btnUpdatePO
            // 
            this.btnUpdatePO.Location = new System.Drawing.Point(801, 365);
            this.btnUpdatePO.Name = "btnUpdatePO";
            this.btnUpdatePO.Size = new System.Drawing.Size(107, 31);
            this.btnUpdatePO.TabIndex = 1;
            this.btnUpdatePO.Text = "Update";
            this.btnUpdatePO.UseVisualStyleBackColor = true;
            this.btnUpdatePO.Click += new System.EventHandler(this.btnUpdatePO_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(802, 368);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnRefreshPO
            // 
            this.btnRefreshPO.Location = new System.Drawing.Point(688, 365);
            this.btnRefreshPO.Name = "btnRefreshPO";
            this.btnRefreshPO.Size = new System.Drawing.Size(107, 31);
            this.btnRefreshPO.TabIndex = 2;
            this.btnRefreshPO.Text = "Refresh";
            this.btnRefreshPO.UseVisualStyleBackColor = true;
            this.btnRefreshPO.Click += new System.EventHandler(this.btnRefreshPO_Click);
            // 
            // btnRefreshBill
            // 
            this.btnRefreshBill.Location = new System.Drawing.Point(689, 369);
            this.btnRefreshBill.Name = "btnRefreshBill";
            this.btnRefreshBill.Size = new System.Drawing.Size(107, 31);
            this.btnRefreshBill.TabIndex = 3;
            this.btnRefreshBill.Text = "Refresh";
            this.btnRefreshBill.UseVisualStyleBackColor = true;
            // 
            // UpdateLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 502);
            this.Controls.Add(this.tabControl1);
            this.Name = "UpdateLogin";
            this.Tag = "UpdateLogin";
            this.Text = "UpdateLogin";
            this.Load += new System.EventHandler(this.UpdateLogin_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPONo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBillRef)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnRefreshPO;
        private System.Windows.Forms.Button btnUpdatePO;
        private System.Windows.Forms.DataGridView dgvPONo;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnRefreshBill;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dgvBillRef;

    }
}