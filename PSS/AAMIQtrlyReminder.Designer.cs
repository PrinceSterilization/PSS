namespace PSS
{
    partial class AAMIQtrlyReminder
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblCutOff = new System.Windows.Forms.Label();
            this.dgvFile = new System.Windows.Forms.DataGridView();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnExclPerm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "AAMI QUARTERLY TEST REMINDER";
            // 
            // lblCutOff
            // 
            this.lblCutOff.AutoSize = true;
            this.lblCutOff.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCutOff.Location = new System.Drawing.Point(21, 40);
            this.lblCutOff.Name = "lblCutOff";
            this.lblCutOff.Size = new System.Drawing.Size(57, 19);
            this.lblCutOff.TabIndex = 1;
            this.lblCutOff.Text = "AS OF";
            // 
            // dgvFile
            // 
            this.dgvFile.AllowUserToAddRows = false;
            this.dgvFile.AllowUserToDeleteRows = false;
            this.dgvFile.AllowUserToOrderColumns = true;
            this.dgvFile.AllowUserToResizeColumns = false;
            this.dgvFile.AllowUserToResizeRows = false;
            this.dgvFile.BackgroundColor = System.Drawing.Color.White;
            this.dgvFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFile.Location = new System.Drawing.Point(25, 62);
            this.dgvFile.Name = "dgvFile";
            this.dgvFile.Size = new System.Drawing.Size(1010, 475);
            this.dgvFile.TabIndex = 2;
            this.dgvFile.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvFile_CellBeginEdit);
            this.dgvFile.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvFile_CurrentCellDirtyStateChanged);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(953, 544);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(84, 24);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send &E-Mail";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(902, 40);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(52, 19);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Total:";
            // 
            // btnExclPerm
            // 
            this.btnExclPerm.Location = new System.Drawing.Point(25, 544);
            this.btnExclPerm.Name = "btnExclPerm";
            this.btnExclPerm.Size = new System.Drawing.Size(118, 24);
            this.btnExclPerm.TabIndex = 5;
            this.btnExclPerm.Text = "&Exclude Permanently";
            this.btnExclPerm.UseVisualStyleBackColor = true;
            this.btnExclPerm.Click += new System.EventHandler(this.btnExclPerm_Click);
            // 
            // AAMIQtrlyReminder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 587);
            this.Controls.Add(this.btnExclPerm);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.dgvFile);
            this.Controls.Add(this.lblCutOff);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1078, 625);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1078, 625);
            this.Name = "AAMIQtrlyReminder";
            this.Text = "AAMI Qtrly Reminder";
            this.Load += new System.EventHandler(this.AAMIQtrlyReminder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblCutOff;
        private System.Windows.Forms.DataGridView dgvFile;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnExclPerm;
    }
}