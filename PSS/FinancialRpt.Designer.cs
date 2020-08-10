namespace PSS
{
    partial class FinancialRpt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FinancialRpt));
            this.txtName = new GISControls.TextBoxChar();
            this.label6 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lnkFile = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lnkAR = new System.Windows.Forms.LinkLabel();
            this.btnAR = new System.Windows.Forms.Button();
            this.txtAR = new GISControls.TextBoxChar();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlPeriod = new System.Windows.Forms.Panel();
            this.btnPrtPrinter = new System.Windows.Forms.Button();
            this.txtYear = new GISControls.TextBoxChar();
            this.picSponsors = new System.Windows.Forms.PictureBox();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.rdoYTD = new System.Windows.Forms.RadioButton();
            this.rdoSelSponsor = new System.Windows.Forms.RadioButton();
            this.rdoAllSponsors = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rdoRegBilling = new System.Windows.Forms.RadioButton();
            this.rdoPrepayments = new System.Windows.Forms.RadioButton();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.btnPrtPreview = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.cboMonths = new System.Windows.Forms.ComboBox();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnProceed = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.cboFSYear = new System.Windows.Forms.ComboBox();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.label11 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.chkSummary = new System.Windows.Forms.CheckBox();
            this.rdoAcctgF1 = new System.Windows.Forms.RadioButton();
            this.rdoAcctgF2 = new System.Windows.Forms.RadioButton();
            this.rdoStdFormat = new System.Windows.Forms.RadioButton();
            this.btnRptDetails = new System.Windows.Forms.Button();
            this.pnlPeriod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(144, 245);
            this.txtName.MaxLength = 255;
            this.txtName.Multiline = true;
            this.txtName.Name = "txtName";
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtName.Size = new System.Drawing.Size(382, 35);
            this.txtName.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(17, 247);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(2);
            this.label6.Size = new System.Drawing.Size(123, 21);
            this.label6.TabIndex = 7;
            this.label6.Text = "File Location/Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(532, 245);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(68, 24);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lnkFile
            // 
            this.lnkFile.Location = new System.Drawing.Point(141, 283);
            this.lnkFile.Name = "lnkFile";
            this.lnkFile.Size = new System.Drawing.Size(442, 14);
            this.lnkFile.TabIndex = 399;
            this.lnkFile.TabStop = true;
            this.lnkFile.Text = "Financial Report";
            this.lnkFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFile_LinkClicked);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(17, 215);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(219, 27);
            this.label1.TabIndex = 400;
            this.label1.Text = "PSS FINANCIAL REPORT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 317);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(2);
            this.label2.Size = new System.Drawing.Size(219, 27);
            this.label2.TabIndex = 405;
            this.label2.Text = "A/R AGING REPORT";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lnkAR
            // 
            this.lnkAR.Location = new System.Drawing.Point(141, 385);
            this.lnkAR.Name = "lnkAR";
            this.lnkAR.Size = new System.Drawing.Size(442, 14);
            this.lnkAR.TabIndex = 404;
            this.lnkAR.TabStop = true;
            this.lnkAR.Text = "Financial Report";
            this.lnkAR.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkAR_LinkClicked);
            // 
            // btnAR
            // 
            this.btnAR.Location = new System.Drawing.Point(532, 347);
            this.btnAR.Name = "btnAR";
            this.btnAR.Size = new System.Drawing.Size(68, 24);
            this.btnAR.TabIndex = 403;
            this.btnAR.Text = "&Browse";
            this.btnAR.UseVisualStyleBackColor = true;
            this.btnAR.Click += new System.EventHandler(this.btnAR_Click);
            // 
            // txtAR
            // 
            this.txtAR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAR.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAR.Location = new System.Drawing.Point(144, 347);
            this.txtAR.MaxLength = 255;
            this.txtAR.Multiline = true;
            this.txtAR.Name = "txtAR";
            this.txtAR.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAR.Size = new System.Drawing.Size(382, 35);
            this.txtAR.TabIndex = 401;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(17, 349);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(123, 21);
            this.label3.TabIndex = 402;
            this.label3.Text = "File Location/Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlPeriod
            // 
            this.pnlPeriod.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlPeriod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPeriod.Controls.Add(this.btnPrtPrinter);
            this.pnlPeriod.Controls.Add(this.txtYear);
            this.pnlPeriod.Controls.Add(this.picSponsors);
            this.pnlPeriod.Controls.Add(this.txtSponsor);
            this.pnlPeriod.Controls.Add(this.rdoYTD);
            this.pnlPeriod.Controls.Add(this.rdoSelSponsor);
            this.pnlPeriod.Controls.Add(this.rdoAllSponsors);
            this.pnlPeriod.Controls.Add(this.label10);
            this.pnlPeriod.Controls.Add(this.label9);
            this.pnlPeriod.Controls.Add(this.label5);
            this.pnlPeriod.Controls.Add(this.rdoRegBilling);
            this.pnlPeriod.Controls.Add(this.rdoPrepayments);
            this.pnlPeriod.Controls.Add(this.rdoAll);
            this.pnlPeriod.Controls.Add(this.btnPrtPreview);
            this.pnlPeriod.Controls.Add(this.dtpTo);
            this.pnlPeriod.Controls.Add(this.dtpFrom);
            this.pnlPeriod.Controls.Add(this.label8);
            this.pnlPeriod.Controls.Add(this.label7);
            this.pnlPeriod.Location = new System.Drawing.Point(20, 207);
            this.pnlPeriod.Name = "pnlPeriod";
            this.pnlPeriod.Size = new System.Drawing.Size(582, 204);
            this.pnlPeriod.TabIndex = 407;
            this.pnlPeriod.Visible = false;
            // 
            // btnPrtPrinter
            // 
            this.btnPrtPrinter.Location = new System.Drawing.Point(469, 85);
            this.btnPrtPrinter.Name = "btnPrtPrinter";
            this.btnPrtPrinter.Size = new System.Drawing.Size(92, 24);
            this.btnPrtPrinter.TabIndex = 121;
            this.btnPrtPrinter.Text = "&Print to Printer";
            this.btnPrtPrinter.UseVisualStyleBackColor = true;
            this.btnPrtPrinter.Visible = false;
            // 
            // txtYear
            // 
            this.txtYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYear.Location = new System.Drawing.Point(270, 80);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(50, 20);
            this.txtYear.TabIndex = 120;
            // 
            // picSponsors
            // 
            this.picSponsors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSponsors.BackgroundImage")));
            this.picSponsors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSponsors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSponsors.Location = new System.Drawing.Point(542, 58);
            this.picSponsors.Name = "picSponsors";
            this.picSponsors.Size = new System.Drawing.Size(19, 21);
            this.picSponsors.TabIndex = 119;
            this.picSponsors.TabStop = false;
            // 
            // txtSponsor
            // 
            this.txtSponsor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsor.Location = new System.Drawing.Point(270, 58);
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(275, 21);
            this.txtSponsor.TabIndex = 15;
            // 
            // rdoYTD
            // 
            this.rdoYTD.AutoSize = true;
            this.rdoYTD.ForeColor = System.Drawing.Color.White;
            this.rdoYTD.Location = new System.Drawing.Point(155, 83);
            this.rdoYTD.Name = "rdoYTD";
            this.rdoYTD.Size = new System.Drawing.Size(89, 17);
            this.rdoYTD.TabIndex = 14;
            this.rdoYTD.Text = "Year-To-Date";
            this.rdoYTD.UseVisualStyleBackColor = true;
            // 
            // rdoSelSponsor
            // 
            this.rdoSelSponsor.AutoSize = true;
            this.rdoSelSponsor.ForeColor = System.Drawing.Color.White;
            this.rdoSelSponsor.Location = new System.Drawing.Point(155, 60);
            this.rdoSelSponsor.Name = "rdoSelSponsor";
            this.rdoSelSponsor.Size = new System.Drawing.Size(109, 17);
            this.rdoSelSponsor.TabIndex = 13;
            this.rdoSelSponsor.Text = "Selected Sponsor";
            this.rdoSelSponsor.UseVisualStyleBackColor = true;
            // 
            // rdoAllSponsors
            // 
            this.rdoAllSponsors.AutoSize = true;
            this.rdoAllSponsors.Checked = true;
            this.rdoAllSponsors.ForeColor = System.Drawing.Color.White;
            this.rdoAllSponsors.Location = new System.Drawing.Point(155, 39);
            this.rdoAllSponsors.Name = "rdoAllSponsors";
            this.rdoAllSponsors.Size = new System.Drawing.Size(83, 17);
            this.rdoAllSponsors.TabIndex = 12;
            this.rdoAllSponsors.TabStop = true;
            this.rdoAllSponsors.Text = "All Sponsors";
            this.rdoAllSponsors.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(152, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(145, 18);
            this.label10.TabIndex = 11;
            this.label10.Text = "SCOPE";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(152, 118);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(145, 18);
            this.label9.TabIndex = 9;
            this.label9.Text = "RANGE";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(14, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "REVENUE TYPE";
            // 
            // rdoRegBilling
            // 
            this.rdoRegBilling.AutoSize = true;
            this.rdoRegBilling.ForeColor = System.Drawing.Color.White;
            this.rdoRegBilling.Location = new System.Drawing.Point(17, 83);
            this.rdoRegBilling.Name = "rdoRegBilling";
            this.rdoRegBilling.Size = new System.Drawing.Size(92, 17);
            this.rdoRegBilling.TabIndex = 7;
            this.rdoRegBilling.Text = "Regular Billing";
            this.rdoRegBilling.UseVisualStyleBackColor = true;
            // 
            // rdoPrepayments
            // 
            this.rdoPrepayments.AutoSize = true;
            this.rdoPrepayments.ForeColor = System.Drawing.Color.White;
            this.rdoPrepayments.Location = new System.Drawing.Point(17, 60);
            this.rdoPrepayments.Name = "rdoPrepayments";
            this.rdoPrepayments.Size = new System.Drawing.Size(86, 17);
            this.rdoPrepayments.TabIndex = 6;
            this.rdoPrepayments.Text = "Prepayments";
            this.rdoPrepayments.UseVisualStyleBackColor = true;
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Checked = true;
            this.rdoAll.ForeColor = System.Drawing.Color.White;
            this.rdoAll.Location = new System.Drawing.Point(17, 39);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(68, 17);
            this.rdoAll.TabIndex = 5;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "All Types";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // btnPrtPreview
            // 
            this.btnPrtPreview.Location = new System.Drawing.Point(303, 160);
            this.btnPrtPreview.Name = "btnPrtPreview";
            this.btnPrtPreview.Size = new System.Drawing.Size(67, 24);
            this.btnPrtPreview.TabIndex = 4;
            this.btnPrtPreview.Text = "&OK";
            this.btnPrtPreview.UseVisualStyleBackColor = true;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(200, 162);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(97, 20);
            this.dtpTo.TabIndex = 3;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(200, 139);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(97, 20);
            this.dtpFrom.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(152, 168);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "To:";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(152, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "From :";
            // 
            // label24
            // 
            this.label24.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label24.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(20, 76);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(84, 21);
            this.label24.TabIndex = 413;
            this.label24.Text = "Cut-off Month";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboMonths
            // 
            this.cboMonths.AutoCompleteCustomSource.AddRange(new string[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.cboMonths.FormattingEnabled = true;
            this.cboMonths.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.cboMonths.Location = new System.Drawing.Point(104, 76);
            this.cboMonths.Name = "cboMonths";
            this.cboMonths.Size = new System.Drawing.Size(145, 21);
            this.cboMonths.TabIndex = 412;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.Color.DarkRed;
            this.lblProgress.Location = new System.Drawing.Point(18, 185);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(203, 13);
            this.lblProgress.TabIndex = 411;
            this.lblProgress.Text = "Generating report...please standby";
            this.lblProgress.Visible = false;
            this.lblProgress.Click += new System.EventHandler(this.lblProgress_Click);
            // 
            // btnProceed
            // 
            this.btnProceed.Location = new System.Drawing.Point(189, 148);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(59, 24);
            this.btnProceed.TabIndex = 410;
            this.btnProceed.Text = "Proceed";
            this.btnProceed.UseVisualStyleBackColor = true;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label23.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(20, 51);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(84, 21);
            this.label23.TabIndex = 409;
            this.label23.Text = "Fiscal Year";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboFSYear
            // 
            this.cboFSYear.AutoCompleteCustomSource.AddRange(new string[] {
            "2015",
            "2014",
            "2013",
            "2012",
            "2011"});
            this.cboFSYear.FormattingEnabled = true;
            this.cboFSYear.Location = new System.Drawing.Point(104, 51);
            this.cboFSYear.Name = "cboFSYear";
            this.cboFSYear.Size = new System.Drawing.Size(59, 21);
            this.cboFSYear.TabIndex = 408;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(625, 439);
            this.shapeContainer1.TabIndex = 414;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderWidth = 3;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 20;
            this.lineShape1.X2 = 601;
            this.lineShape1.Y1 = 203;
            this.lineShape1.Y2 = 203;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Arial", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(17, 21);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(2);
            this.label11.Size = new System.Drawing.Size(325, 27);
            this.label11.TabIndex = 415;
            this.label11.Text = "PSS FINANCIAL  REPORTS (ACCPAC)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // chkSummary
            // 
            this.chkSummary.Location = new System.Drawing.Point(21, 100);
            this.chkSummary.Name = "chkSummary";
            this.chkSummary.Size = new System.Drawing.Size(132, 22);
            this.chkSummary.TabIndex = 416;
            this.chkSummary.Text = "Summary";
            this.chkSummary.UseVisualStyleBackColor = true;
            // 
            // rdoAcctgF1
            // 
            this.rdoAcctgF1.Location = new System.Drawing.Point(20, 138);
            this.rdoAcctgF1.Name = "rdoAcctgF1";
            this.rdoAcctgF1.Size = new System.Drawing.Size(130, 19);
            this.rdoAcctgF1.TabIndex = 417;
            this.rdoAcctgF1.Text = "Accounting Format 1";
            this.rdoAcctgF1.UseVisualStyleBackColor = true;
            // 
            // rdoAcctgF2
            // 
            this.rdoAcctgF2.Location = new System.Drawing.Point(20, 156);
            this.rdoAcctgF2.Name = "rdoAcctgF2";
            this.rdoAcctgF2.Size = new System.Drawing.Size(130, 19);
            this.rdoAcctgF2.TabIndex = 418;
            this.rdoAcctgF2.Text = "Accounting Format 2";
            this.rdoAcctgF2.UseVisualStyleBackColor = true;
            // 
            // rdoStdFormat
            // 
            this.rdoStdFormat.Checked = true;
            this.rdoStdFormat.Location = new System.Drawing.Point(20, 119);
            this.rdoStdFormat.Name = "rdoStdFormat";
            this.rdoStdFormat.Size = new System.Drawing.Size(130, 22);
            this.rdoStdFormat.TabIndex = 419;
            this.rdoStdFormat.TabStop = true;
            this.rdoStdFormat.Text = "Standard Format";
            this.rdoStdFormat.UseVisualStyleBackColor = true;
            // 
            // btnRptDetails
            // 
            this.btnRptDetails.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnRptDetails.FlatAppearance.BorderColor = System.Drawing.Color.Navy;
            this.btnRptDetails.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCoral;
            this.btnRptDetails.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CornflowerBlue;
            this.btnRptDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRptDetails.Location = new System.Drawing.Point(169, 51);
            this.btnRptDetails.Name = "btnRptDetails";
            this.btnRptDetails.Size = new System.Drawing.Size(80, 24);
            this.btnRptDetails.TabIndex = 420;
            this.btnRptDetails.Text = "View Details";
            this.btnRptDetails.UseVisualStyleBackColor = false;
            this.btnRptDetails.Click += new System.EventHandler(this.btnRptDetails_Click);
            // 
            // FinancialRpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 439);
            this.Controls.Add(this.btnRptDetails);
            this.Controls.Add(this.rdoStdFormat);
            this.Controls.Add(this.rdoAcctgF2);
            this.Controls.Add(this.rdoAcctgF1);
            this.Controls.Add(this.chkSummary);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.cboMonths);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnProceed);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.cboFSYear);
            this.Controls.Add(this.pnlPeriod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lnkAR);
            this.Controls.Add(this.btnAR);
            this.Controls.Add(this.txtAR);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lnkFile);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "FinancialRpt";
            this.Tag = "FinancialReports";
            this.Text = "FinancialRpt";
            this.Load += new System.EventHandler(this.FinancialRpt_Load);
            this.pnlPeriod.ResumeLayout(false);
            this.pnlPeriod.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GISControls.TextBoxChar txtName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.LinkLabel lnkFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkAR;
        private System.Windows.Forms.Button btnAR;
        private GISControls.TextBoxChar txtAR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlPeriod;
        private GISControls.TextBoxChar txtSponsor;
        private System.Windows.Forms.RadioButton rdoYTD;
        private System.Windows.Forms.RadioButton rdoSelSponsor;
        private System.Windows.Forms.RadioButton rdoAllSponsors;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdoRegBilling;
        private System.Windows.Forms.RadioButton rdoPrepayments;
        private System.Windows.Forms.RadioButton rdoAll;
        private System.Windows.Forms.Button btnPrtPreview;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private GISControls.TextBoxChar txtYear;
        private System.Windows.Forms.PictureBox picSponsors;
        private System.Windows.Forms.Button btnPrtPrinter;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox cboMonths;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.ComboBox cboFSYear;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox chkSummary;
        private System.Windows.Forms.RadioButton rdoAcctgF1;
        private System.Windows.Forms.RadioButton rdoAcctgF2;
        private System.Windows.Forms.RadioButton rdoStdFormat;
        private System.Windows.Forms.Button btnRptDetails;
    }
}