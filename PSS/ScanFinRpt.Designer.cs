namespace PSS
{
    partial class ScanFinRpt
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRptNo = new System.Windows.Forms.TextBox();
            this.dgvReports = new System.Windows.Forms.DataGridView();
            this.ReportNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReportDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPrtLabels = new System.Windows.Forms.Button();
            this.btnPrtCover = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Report No.";
            // 
            // txtRptNo
            // 
            this.txtRptNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRptNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRptNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRptNo.Location = new System.Drawing.Point(94, 32);
            this.txtRptNo.MaxLength = 11;
            this.txtRptNo.Name = "txtRptNo";
            this.txtRptNo.Size = new System.Drawing.Size(99, 21);
            this.txtRptNo.TabIndex = 1;
            this.txtRptNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRptNo_KeyPress);
            // 
            // dgvReports
            // 
            this.dgvReports.AllowUserToAddRows = false;
            this.dgvReports.AllowUserToDeleteRows = false;
            this.dgvReports.BackgroundColor = System.Drawing.Color.White;
            this.dgvReports.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReports.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ReportNo,
            this.ReportDate});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReports.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvReports.Location = new System.Drawing.Point(29, 67);
            this.dgvReports.Name = "dgvReports";
            this.dgvReports.ReadOnly = true;
            this.dgvReports.Size = new System.Drawing.Size(334, 335);
            this.dgvReports.TabIndex = 2;
            this.dgvReports.CurrentCellChanged += new System.EventHandler(this.dgvReports_CurrentCellChanged);
            // 
            // ReportNo
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ReportNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.ReportNo.HeaderText = "Report No.";
            this.ReportNo.Name = "ReportNo";
            this.ReportNo.ReadOnly = true;
            // 
            // ReportDate
            // 
            dataGridViewCellStyle2.Format = "MM/dd/yyyy hh:mm:ss AM/PM";
            dataGridViewCellStyle2.NullValue = null;
            this.ReportDate.DefaultCellStyle = dataGridViewCellStyle2;
            this.ReportDate.HeaderText = "Date";
            this.ReportDate.Name = "ReportDate";
            this.ReportDate.ReadOnly = true;
            // 
            // btnPrtLabels
            // 
            this.btnPrtLabels.Location = new System.Drawing.Point(90, 412);
            this.btnPrtLabels.Name = "btnPrtLabels";
            this.btnPrtLabels.Size = new System.Drawing.Size(87, 26);
            this.btnPrtLabels.TabIndex = 3;
            this.btnPrtLabels.Text = "Print &Label";
            this.btnPrtLabels.UseVisualStyleBackColor = true;
            this.btnPrtLabels.Click += new System.EventHandler(this.btnPrtLabels_Click);
            // 
            // btnPrtCover
            // 
            this.btnPrtCover.Location = new System.Drawing.Point(183, 412);
            this.btnPrtCover.Name = "btnPrtCover";
            this.btnPrtCover.Size = new System.Drawing.Size(87, 26);
            this.btnPrtCover.TabIndex = 4;
            this.btnPrtCover.Text = "Print &Cover";
            this.btnPrtCover.UseVisualStyleBackColor = true;
            this.btnPrtCover.Click += new System.EventHandler(this.btnPrtCover_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(276, 412);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 26);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Cl&ose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ScanFinRpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 467);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrtCover);
            this.Controls.Add(this.btnPrtLabels);
            this.Controls.Add(this.dgvReports);
            this.Controls.Add(this.txtRptNo);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScanFinRpt";
            this.Tag = "ScanFinalReports";
            this.Text = "SCAN FINAL REPORT";
            this.Load += new System.EventHandler(this.ScanFinRpt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReports)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRptNo;
        private System.Windows.Forms.DataGridView dgvReports;
        private System.Windows.Forms.Button btnPrtLabels;
        private System.Windows.Forms.Button btnPrtCover;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReportDate;
        private System.Windows.Forms.Button btnClose;
    }
}