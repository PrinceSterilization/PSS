namespace PSS
{
    partial class AuditVisit
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuditVisit));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.dgvAudits = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.pnlAudit = new System.Windows.Forms.Panel();
            this.lblAudit = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblVisit = new System.Windows.Forms.Label();
            this.dgvVisits = new System.Windows.Forms.DataGridView();
            this.lblReminder = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAudits)).BeginInit();
            this.pnlAudit.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisits)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(10, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(334, 132);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(10, 183);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(334, 156);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // dgvAudits
            // 
            this.dgvAudits.AllowUserToAddRows = false;
            this.dgvAudits.AllowUserToDeleteRows = false;
            this.dgvAudits.AllowUserToResizeColumns = false;
            this.dgvAudits.AllowUserToResizeRows = false;
            this.dgvAudits.BackgroundColor = System.Drawing.Color.White;
            this.dgvAudits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAudits.Location = new System.Drawing.Point(10, 42);
            this.dgvAudits.Name = "dgvAudits";
            this.dgvAudits.ReadOnly = true;
            this.dgvAudits.RowHeadersVisible = false;
            this.dgvAudits.Size = new System.Drawing.Size(473, 79);
            this.dgvAudits.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Brush Script MT", 24F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(11, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 34);
            this.label1.TabIndex = 4;
            this.label1.Text = "This Week at";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Garamond", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label2.Location = new System.Drawing.Point(170, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(443, 34);
            this.label2.TabIndex = 5;
            this.label2.Text = "Prince Sterilization Services, LLC.";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(753, 352);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(91, 30);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "O&K";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlAudit
            // 
            this.pnlAudit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAudit.Controls.Add(this.lblAudit);
            this.pnlAudit.Controls.Add(this.dgvAudits);
            this.pnlAudit.Location = new System.Drawing.Point(346, 45);
            this.pnlAudit.Name = "pnlAudit";
            this.pnlAudit.Size = new System.Drawing.Size(498, 132);
            this.pnlAudit.TabIndex = 7;
            // 
            // lblAudit
            // 
            this.lblAudit.Font = new System.Drawing.Font("Brush Script MT", 21.75F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAudit.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblAudit.Location = new System.Drawing.Point(10, -1);
            this.lblAudit.Name = "lblAudit";
            this.lblAudit.Size = new System.Drawing.Size(473, 38);
            this.lblAudit.TabIndex = 0;
            this.lblAudit.Text = "Audit Schedule";
            this.lblAudit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblVisit);
            this.panel1.Controls.Add(this.dgvVisits);
            this.panel1.Location = new System.Drawing.Point(346, 183);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(498, 156);
            this.panel1.TabIndex = 8;
            // 
            // lblVisit
            // 
            this.lblVisit.Font = new System.Drawing.Font("Brush Script MT", 21.75F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVisit.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblVisit.Location = new System.Drawing.Point(3, -1);
            this.lblVisit.Name = "lblVisit";
            this.lblVisit.Size = new System.Drawing.Size(473, 37);
            this.lblVisit.TabIndex = 0;
            this.lblVisit.Text = "Visit Schedule";
            this.lblVisit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvVisits
            // 
            this.dgvVisits.AllowUserToAddRows = false;
            this.dgvVisits.AllowUserToDeleteRows = false;
            this.dgvVisits.AllowUserToResizeColumns = false;
            this.dgvVisits.AllowUserToResizeRows = false;
            this.dgvVisits.BackgroundColor = System.Drawing.Color.White;
            this.dgvVisits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVisits.Location = new System.Drawing.Point(9, 36);
            this.dgvVisits.Name = "dgvVisits";
            this.dgvVisits.ReadOnly = true;
            this.dgvVisits.RowHeadersVisible = false;
            this.dgvVisits.Size = new System.Drawing.Size(473, 103);
            this.dgvVisits.TabIndex = 2;
            // 
            // lblReminder
            // 
            this.lblReminder.AutoSize = true;
            this.lblReminder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReminder.Location = new System.Drawing.Point(7, 359);
            this.lblReminder.Name = "lblReminder";
            this.lblReminder.Size = new System.Drawing.Size(517, 16);
            this.lblReminder.TabIndex = 9;
            this.lblReminder.Text = "Please observe the necessary precautions for client visits as appropriate.";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // AuditVisit
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(859, 396);
            this.ControlBox = false;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblReminder);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlAudit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "AuditVisit";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PSS REMINDERS";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AuditVisit_FormClosing);
            this.Load += new System.EventHandler(this.AuditVisit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAudits)).EndInit();
            this.pnlAudit.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.DataGridView dgvAudits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pnlAudit;
        private System.Windows.Forms.Label lblAudit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblVisit;
        private System.Windows.Forms.DataGridView dgvVisits;
        private System.Windows.Forms.Label lblReminder;
        private System.Windows.Forms.Timer timer1;
    }
}