namespace PSS
{
    partial class HRReports
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
            this.pnlReports = new System.Windows.Forms.Panel();
            this.rdoEmployeesList = new System.Windows.Forms.RadioButton();
            this.rdoEmpTerByPeriod = new System.Windows.Forms.RadioButton();
            this.rdoEmpHiredByPeriod = new System.Windows.Forms.RadioButton();
            this.rdoBirthdayList = new System.Windows.Forms.RadioButton();
            this.rdoEmpPhoneLN = new System.Windows.Forms.RadioButton();
            this.rdoEmpPhoneFN = new System.Windows.Forms.RadioButton();
            this.rdoYrTurnover = new System.Windows.Forms.RadioButton();
            this.rdo401K = new System.Windows.Forms.RadioButton();
            this.rdoStaffByDate = new System.Windows.Forms.RadioButton();
            this.rdoFormerEmp = new System.Windows.Forms.RadioButton();
            this.rdoHiredEmp = new System.Windows.Forms.RadioButton();
            this.rdoYearlyStaff = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoCurrentStaff = new System.Windows.Forms.RadioButton();
            this.btnProceed = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label20 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.pnlYear = new System.Windows.Forms.Panel();
            this.pnlDateRange = new System.Windows.Forms.Panel();
            this.rdoEducation = new System.Windows.Forms.RadioButton();
            this.pnlReports.SuspendLayout();
            this.pnlYear.SuspendLayout();
            this.pnlDateRange.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlReports
            // 
            this.pnlReports.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlReports.Controls.Add(this.rdoEducation);
            this.pnlReports.Controls.Add(this.rdoEmployeesList);
            this.pnlReports.Controls.Add(this.rdoEmpTerByPeriod);
            this.pnlReports.Controls.Add(this.rdoEmpHiredByPeriod);
            this.pnlReports.Controls.Add(this.rdoBirthdayList);
            this.pnlReports.Controls.Add(this.rdoEmpPhoneLN);
            this.pnlReports.Controls.Add(this.rdoEmpPhoneFN);
            this.pnlReports.Controls.Add(this.rdoYrTurnover);
            this.pnlReports.Controls.Add(this.rdo401K);
            this.pnlReports.Controls.Add(this.rdoStaffByDate);
            this.pnlReports.Controls.Add(this.rdoFormerEmp);
            this.pnlReports.Controls.Add(this.rdoHiredEmp);
            this.pnlReports.Controls.Add(this.rdoYearlyStaff);
            this.pnlReports.Controls.Add(this.label1);
            this.pnlReports.Controls.Add(this.rdoCurrentStaff);
            this.pnlReports.Location = new System.Drawing.Point(24, 41);
            this.pnlReports.Name = "pnlReports";
            this.pnlReports.Size = new System.Drawing.Size(346, 273);
            this.pnlReports.TabIndex = 422;
            // 
            // rdoEmployeesList
            // 
            this.rdoEmployeesList.AutoSize = true;
            this.rdoEmployeesList.Location = new System.Drawing.Point(25, 234);
            this.rdoEmployeesList.Name = "rdoEmployeesList";
            this.rdoEmployeesList.Size = new System.Drawing.Size(179, 17);
            this.rdoEmployeesList.TabIndex = 423;
            this.rdoEmployeesList.Text = "Employees List in a Given Period";
            this.rdoEmployeesList.UseVisualStyleBackColor = true;
            this.rdoEmployeesList.CheckedChanged += new System.EventHandler(this.rdoEmployeesList_CheckedChanged);
            // 
            // rdoEmpTerByPeriod
            // 
            this.rdoEmpTerByPeriod.AutoSize = true;
            this.rdoEmpTerByPeriod.Location = new System.Drawing.Point(25, 217);
            this.rdoEmpTerByPeriod.Name = "rdoEmpTerByPeriod";
            this.rdoEmpTerByPeriod.Size = new System.Drawing.Size(216, 17);
            this.rdoEmpTerByPeriod.TabIndex = 422;
            this.rdoEmpTerByPeriod.Text = "Employees Terminated in a Given Period";
            this.rdoEmpTerByPeriod.UseVisualStyleBackColor = true;
            this.rdoEmpTerByPeriod.CheckedChanged += new System.EventHandler(this.rdoEmpTerByPeriod_CheckedChanged);
            // 
            // rdoEmpHiredByPeriod
            // 
            this.rdoEmpHiredByPeriod.AutoSize = true;
            this.rdoEmpHiredByPeriod.Location = new System.Drawing.Point(25, 200);
            this.rdoEmpHiredByPeriod.Name = "rdoEmpHiredByPeriod";
            this.rdoEmpHiredByPeriod.Size = new System.Drawing.Size(188, 17);
            this.rdoEmpHiredByPeriod.TabIndex = 421;
            this.rdoEmpHiredByPeriod.Text = "Employees Hired in a Given Period";
            this.rdoEmpHiredByPeriod.UseVisualStyleBackColor = true;
            this.rdoEmpHiredByPeriod.CheckedChanged += new System.EventHandler(this.rdoEmpHiredByPeriod_CheckedChanged);
            // 
            // rdoBirthdayList
            // 
            this.rdoBirthdayList.AutoSize = true;
            this.rdoBirthdayList.Location = new System.Drawing.Point(25, 183);
            this.rdoBirthdayList.Name = "rdoBirthdayList";
            this.rdoBirthdayList.Size = new System.Drawing.Size(136, 17);
            this.rdoBirthdayList.TabIndex = 420;
            this.rdoBirthdayList.Text = "Employees Birthday List";
            this.rdoBirthdayList.UseVisualStyleBackColor = true;
            this.rdoBirthdayList.CheckedChanged += new System.EventHandler(this.rdoBirthdayList_CheckedChanged);
            // 
            // rdoEmpPhoneLN
            // 
            this.rdoEmpPhoneLN.AutoSize = true;
            this.rdoEmpPhoneLN.Location = new System.Drawing.Point(25, 166);
            this.rdoEmpPhoneLN.Name = "rdoEmpPhoneLN";
            this.rdoEmpPhoneLN.Size = new System.Drawing.Size(197, 17);
            this.rdoEmpPhoneLN.TabIndex = 419;
            this.rdoEmpPhoneLN.Text = "Employees Phone List by Last Name";
            this.rdoEmpPhoneLN.UseVisualStyleBackColor = true;
            this.rdoEmpPhoneLN.CheckedChanged += new System.EventHandler(this.rdoEmpPhoneLN_CheckedChanged);
            // 
            // rdoEmpPhoneFN
            // 
            this.rdoEmpPhoneFN.AutoSize = true;
            this.rdoEmpPhoneFN.Location = new System.Drawing.Point(25, 149);
            this.rdoEmpPhoneFN.Name = "rdoEmpPhoneFN";
            this.rdoEmpPhoneFN.Size = new System.Drawing.Size(196, 17);
            this.rdoEmpPhoneFN.TabIndex = 418;
            this.rdoEmpPhoneFN.Text = "Employees Phone List by First Name";
            this.rdoEmpPhoneFN.UseVisualStyleBackColor = true;
            this.rdoEmpPhoneFN.CheckedChanged += new System.EventHandler(this.rdoEmpPhoneFN_CheckedChanged);
            // 
            // rdoYrTurnover
            // 
            this.rdoYrTurnover.AutoSize = true;
            this.rdoYrTurnover.Location = new System.Drawing.Point(25, 133);
            this.rdoYrTurnover.Name = "rdoYrTurnover";
            this.rdoYrTurnover.Size = new System.Drawing.Size(100, 17);
            this.rdoYrTurnover.TabIndex = 417;
            this.rdoYrTurnover.Text = "Yearly Turnover";
            this.rdoYrTurnover.UseVisualStyleBackColor = true;
            this.rdoYrTurnover.CheckedChanged += new System.EventHandler(this.rdoYrTurnover_CheckedChanged);
            // 
            // rdo401K
            // 
            this.rdo401K.AutoSize = true;
            this.rdo401K.Location = new System.Drawing.Point(25, 116);
            this.rdo401K.Name = "rdo401K";
            this.rdo401K.Size = new System.Drawing.Size(218, 17);
            this.rdo401K.TabIndex = 416;
            this.rdo401K.Text = "Employees 401K Census in a Given Year";
            this.rdo401K.UseVisualStyleBackColor = true;
            this.rdo401K.CheckedChanged += new System.EventHandler(this.rdo401K_CheckedChanged);
            // 
            // rdoStaffByDate
            // 
            this.rdoStaffByDate.AutoSize = true;
            this.rdoStaffByDate.Location = new System.Drawing.Point(25, 63);
            this.rdoStaffByDate.Name = "rdoStaffByDate";
            this.rdoStaffByDate.Size = new System.Drawing.Size(256, 17);
            this.rdoStaffByDate.TabIndex = 415;
            this.rdoStaffByDate.Text = "Hired or Terminated Employees in a Given Period";
            this.rdoStaffByDate.UseVisualStyleBackColor = true;
            this.rdoStaffByDate.CheckedChanged += new System.EventHandler(this.rdoStaffByDate_CheckedChanged);
            // 
            // rdoFormerEmp
            // 
            this.rdoFormerEmp.AutoSize = true;
            this.rdoFormerEmp.Location = new System.Drawing.Point(25, 99);
            this.rdoFormerEmp.Name = "rdoFormerEmp";
            this.rdoFormerEmp.Size = new System.Drawing.Size(208, 17);
            this.rdoFormerEmp.TabIndex = 5;
            this.rdoFormerEmp.Text = "Terminated Employees in a Given Year";
            this.rdoFormerEmp.UseVisualStyleBackColor = true;
            this.rdoFormerEmp.CheckedChanged += new System.EventHandler(this.rdoFormerEmp_CheckedChanged);
            // 
            // rdoHiredEmp
            // 
            this.rdoHiredEmp.AutoSize = true;
            this.rdoHiredEmp.Location = new System.Drawing.Point(25, 81);
            this.rdoHiredEmp.Name = "rdoHiredEmp";
            this.rdoHiredEmp.Size = new System.Drawing.Size(180, 17);
            this.rdoHiredEmp.TabIndex = 4;
            this.rdoHiredEmp.Text = "Hired Employees in a Given Year";
            this.rdoHiredEmp.UseVisualStyleBackColor = true;
            this.rdoHiredEmp.CheckedChanged += new System.EventHandler(this.rdoHiredEmp_CheckedChanged);
            // 
            // rdoYearlyStaff
            // 
            this.rdoYearlyStaff.AutoSize = true;
            this.rdoYearlyStaff.Location = new System.Drawing.Point(25, 46);
            this.rdoYearlyStaff.Name = "rdoYearlyStaff";
            this.rdoYearlyStaff.Size = new System.Drawing.Size(93, 17);
            this.rdoYearlyStaff.TabIndex = 3;
            this.rdoYearlyStaff.Text = "Yearly Staffing";
            this.rdoYearlyStaff.UseVisualStyleBackColor = true;
            this.rdoYearlyStaff.CheckedChanged += new System.EventHandler(this.rdoYearlyStaff_CheckedChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(346, 19);
            this.label1.TabIndex = 414;
            this.label1.Text = "Report Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rdoCurrentStaff
            // 
            this.rdoCurrentStaff.AutoSize = true;
            this.rdoCurrentStaff.Checked = true;
            this.rdoCurrentStaff.Location = new System.Drawing.Point(25, 29);
            this.rdoCurrentStaff.Name = "rdoCurrentStaff";
            this.rdoCurrentStaff.Size = new System.Drawing.Size(98, 17);
            this.rdoCurrentStaff.TabIndex = 2;
            this.rdoCurrentStaff.TabStop = true;
            this.rdoCurrentStaff.Text = "Current Staffing";
            this.rdoCurrentStaff.UseVisualStyleBackColor = true;
            this.rdoCurrentStaff.CheckedChanged += new System.EventHandler(this.rdoCurrentStaff_CheckedChanged);
            // 
            // btnProceed
            // 
            this.btnProceed.Location = new System.Drawing.Point(311, 320);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(59, 24);
            this.btnProceed.TabIndex = 6;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.UseVisualStyleBackColor = true;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.Color.DarkRed;
            this.lblProgress.Location = new System.Drawing.Point(21, 359);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(203, 13);
            this.lblProgress.TabIndex = 418;
            this.lblProgress.Text = "Generating report...please standby";
            this.lblProgress.Visible = false;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label18.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(229, 8);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(26, 19);
            this.label18.TabIndex = 421;
            this.label18.Text = "To";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label19.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(86, 8);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(39, 19);
            this.label19.TabIndex = 420;
            this.label19.Text = "From";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "MM/dd/yyyy";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(255, 8);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(98, 20);
            this.dtpEnd.TabIndex = 417;
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "MM/dd/yyyy";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(125, 8);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(98, 20);
            this.dtpStart.TabIndex = 416;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.SteelBlue;
            this.label20.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(6, 8);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(79, 19);
            this.label20.TabIndex = 419;
            this.label20.Text = "Date Range";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.SteelBlue;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 19);
            this.label2.TabIndex = 423;
            this.label2.Text = "Report Year";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Items.AddRange(new object[] {
            "2016",
            "2015",
            "2014",
            "2013",
            "2012",
            "2011",
            "2010"});
            this.cboYear.Location = new System.Drawing.Point(90, 7);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(72, 21);
            this.cboYear.TabIndex = 424;
            // 
            // pnlYear
            // 
            this.pnlYear.Controls.Add(this.label2);
            this.pnlYear.Controls.Add(this.cboYear);
            this.pnlYear.Location = new System.Drawing.Point(18, 320);
            this.pnlYear.Name = "pnlYear";
            this.pnlYear.Size = new System.Drawing.Size(168, 34);
            this.pnlYear.TabIndex = 425;
            this.pnlYear.Visible = false;
            // 
            // pnlDateRange
            // 
            this.pnlDateRange.Controls.Add(this.label20);
            this.pnlDateRange.Controls.Add(this.dtpStart);
            this.pnlDateRange.Controls.Add(this.dtpEnd);
            this.pnlDateRange.Controls.Add(this.label19);
            this.pnlDateRange.Controls.Add(this.label18);
            this.pnlDateRange.Location = new System.Drawing.Point(17, 6);
            this.pnlDateRange.Name = "pnlDateRange";
            this.pnlDateRange.Size = new System.Drawing.Size(360, 32);
            this.pnlDateRange.TabIndex = 426;
            this.pnlDateRange.Visible = false;
            // 
            // rdoEducation
            // 
            this.rdoEducation.AutoSize = true;
            this.rdoEducation.Location = new System.Drawing.Point(25, 251);
            this.rdoEducation.Name = "rdoEducation";
            this.rdoEducation.Size = new System.Drawing.Size(127, 17);
            this.rdoEducation.TabIndex = 427;
            this.rdoEducation.Text = "Employees Education";
            this.rdoEducation.UseVisualStyleBackColor = true;
            this.rdoEducation.CheckedChanged += new System.EventHandler(this.rdoEducation_CheckedChanged);
            // 
            // HRReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 392);
            this.Controls.Add(this.pnlDateRange);
            this.Controls.Add(this.pnlYear);
            this.Controls.Add(this.pnlReports);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnProceed);
            this.Name = "HRReports";
            this.Text = "HR Reports";
            this.Load += new System.EventHandler(this.HRReports_Load);
            this.pnlReports.ResumeLayout(false);
            this.pnlReports.PerformLayout();
            this.pnlYear.ResumeLayout(false);
            this.pnlDateRange.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlReports;
        private System.Windows.Forms.RadioButton rdoYrTurnover;
        private System.Windows.Forms.RadioButton rdo401K;
        private System.Windows.Forms.RadioButton rdoStaffByDate;
        private System.Windows.Forms.RadioButton rdoFormerEmp;
        private System.Windows.Forms.RadioButton rdoHiredEmp;
        private System.Windows.Forms.RadioButton rdoYearlyStaff;
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoCurrentStaff;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.RadioButton rdoEmpPhoneLN;
        private System.Windows.Forms.RadioButton rdoEmpPhoneFN;
        private System.Windows.Forms.Panel pnlYear;
        private System.Windows.Forms.Panel pnlDateRange;
        private System.Windows.Forms.RadioButton rdoBirthdayList;
        private System.Windows.Forms.RadioButton rdoEmpHiredByPeriod;
        private System.Windows.Forms.RadioButton rdoEmpTerByPeriod;
        private System.Windows.Forms.RadioButton rdoEmployeesList;
        private System.Windows.Forms.RadioButton rdoEducation;
    }
}