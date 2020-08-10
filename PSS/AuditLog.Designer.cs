namespace PSS
{
    partial class AuditLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuditLog));
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.chkContBuss = new System.Windows.Forms.CheckBox();
            this.mskDateEntered = new System.Windows.Forms.MaskedTextBox();
            this.mskAuditDate = new System.Windows.Forms.MaskedTextBox();
            this.mskDateCARComp = new System.Windows.Forms.MaskedTextBox();
            this.chkRecAuditReport = new System.Windows.Forms.CheckBox();
            this.chkRecAuditAgenda = new System.Windows.Forms.CheckBox();
            this.chkCND = new System.Windows.Forms.CheckBox();
            this.chkASN = new System.Windows.Forms.CheckBox();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvEmpAuditors = new System.Windows.Forms.DataGridView();
            this.dgvSponsors = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.calAudit = new System.Windows.Forms.MonthCalendar();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.picSponsors = new System.Windows.Forms.PictureBox();
            this.picEmpAuditors = new System.Windows.Forms.PictureBox();
            this.lblDeficiencyDesc = new System.Windows.Forms.Label();
            this.txtDeficiencyDesc = new System.Windows.Forms.TextBox();
            this.lblDeficiencyNo = new System.Windows.Forms.Label();
            this.txtDeficiencyNo = new GISControls.TextBoxChar();
            this.lblCARCompleted = new System.Windows.Forms.Label();
            this.lblAuditorNames = new System.Windows.Forms.Label();
            this.txtAuditorName = new System.Windows.Forms.TextBox();
            this.lblAuditNo = new System.Windows.Forms.Label();
            this.txtAuditID = new GISControls.TextBoxChar();
            this.lblSupervisor = new System.Windows.Forms.Label();
            this.txtSponsorID = new GISControls.TextBoxChar();
            this.lblEmpAuditor = new System.Windows.Forms.Label();
            this.txtEmpAuditorID = new GISControls.TextBoxChar();
            this.txtEmpAuditor = new GISControls.TextBoxChar();
            this.lblEntryDate = new System.Windows.Forms.Label();
            this.lblAuditDate = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpAuditors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEmpAuditors)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.chkContBuss);
            this.pnlRecord.Controls.Add(this.mskDateEntered);
            this.pnlRecord.Controls.Add(this.mskAuditDate);
            this.pnlRecord.Controls.Add(this.mskDateCARComp);
            this.pnlRecord.Controls.Add(this.chkRecAuditReport);
            this.pnlRecord.Controls.Add(this.chkRecAuditAgenda);
            this.pnlRecord.Controls.Add(this.chkCND);
            this.pnlRecord.Controls.Add(this.chkASN);
            this.pnlRecord.Controls.Add(this.cboStatus);
            this.pnlRecord.Controls.Add(this.label1);
            this.pnlRecord.Controls.Add(this.dgvEmpAuditors);
            this.pnlRecord.Controls.Add(this.dgvSponsors);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.calAudit);
            this.pnlRecord.Controls.Add(this.txtSponsor);
            this.pnlRecord.Controls.Add(this.picSponsors);
            this.pnlRecord.Controls.Add(this.picEmpAuditors);
            this.pnlRecord.Controls.Add(this.lblDeficiencyDesc);
            this.pnlRecord.Controls.Add(this.txtDeficiencyDesc);
            this.pnlRecord.Controls.Add(this.lblDeficiencyNo);
            this.pnlRecord.Controls.Add(this.txtDeficiencyNo);
            this.pnlRecord.Controls.Add(this.lblCARCompleted);
            this.pnlRecord.Controls.Add(this.lblAuditorNames);
            this.pnlRecord.Controls.Add(this.txtAuditorName);
            this.pnlRecord.Controls.Add(this.lblAuditNo);
            this.pnlRecord.Controls.Add(this.txtAuditID);
            this.pnlRecord.Controls.Add(this.lblSupervisor);
            this.pnlRecord.Controls.Add(this.txtSponsorID);
            this.pnlRecord.Controls.Add(this.lblEmpAuditor);
            this.pnlRecord.Controls.Add(this.txtEmpAuditorID);
            this.pnlRecord.Controls.Add(this.txtEmpAuditor);
            this.pnlRecord.Controls.Add(this.lblEntryDate);
            this.pnlRecord.Controls.Add(this.lblAuditDate);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Location = new System.Drawing.Point(12, 87);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(765, 480);
            this.pnlRecord.TabIndex = 107;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // chkContBuss
            // 
            this.chkContBuss.AutoSize = true;
            this.chkContBuss.Location = new System.Drawing.Point(574, 119);
            this.chkContBuss.Name = "chkContBuss";
            this.chkContBuss.Size = new System.Drawing.Size(165, 19);
            this.chkContBuss.TabIndex = 405;
            this.chkContBuss.Text = "Continuation of Business";
            this.chkContBuss.UseVisualStyleBackColor = true;
            // 
            // mskDateEntered
            // 
            this.mskDateEntered.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mskDateEntered.Location = new System.Drawing.Point(173, 95);
            this.mskDateEntered.Mask = "00/00/0000";
            this.mskDateEntered.Name = "mskDateEntered";
            this.mskDateEntered.RejectInputOnFirstFailure = true;
            this.mskDateEntered.Size = new System.Drawing.Size(79, 21);
            this.mskDateEntered.TabIndex = 404;
            this.mskDateEntered.ValidatingType = typeof(System.DateTime);
            // 
            // mskAuditDate
            // 
            this.mskAuditDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mskAuditDate.Location = new System.Drawing.Point(173, 72);
            this.mskAuditDate.Mask = "00/00/0000";
            this.mskAuditDate.Name = "mskAuditDate";
            this.mskAuditDate.RejectInputOnFirstFailure = true;
            this.mskAuditDate.Size = new System.Drawing.Size(79, 21);
            this.mskAuditDate.TabIndex = 403;
            this.mskAuditDate.ValidatingType = typeof(System.DateTime);
            this.mskAuditDate.Click += new System.EventHandler(this.mskAuditDate_Click);
            this.mskAuditDate.Enter += new System.EventHandler(this.mskAuditDate_Enter);
            this.mskAuditDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mskAuditDate_KeyDown);
            this.mskAuditDate.Leave += new System.EventHandler(this.mskAuditDate_Leave);
            // 
            // mskDateCARComp
            // 
            this.mskDateCARComp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mskDateCARComp.Location = new System.Drawing.Point(173, 118);
            this.mskDateCARComp.Mask = "00/00/0000";
            this.mskDateCARComp.Name = "mskDateCARComp";
            this.mskDateCARComp.RejectInputOnFirstFailure = true;
            this.mskDateCARComp.Size = new System.Drawing.Size(79, 21);
            this.mskDateCARComp.TabIndex = 402;
            this.mskDateCARComp.ValidatingType = typeof(System.DateTime);
            this.mskDateCARComp.Click += new System.EventHandler(this.mskDateCARComp_Click);
            this.mskDateCARComp.Enter += new System.EventHandler(this.mskDateCARComp_Enter);
            this.mskDateCARComp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mskDateCARComp_KeyDown);
            // 
            // chkRecAuditReport
            // 
            this.chkRecAuditReport.AutoSize = true;
            this.chkRecAuditReport.Location = new System.Drawing.Point(395, 119);
            this.chkRecAuditReport.Name = "chkRecAuditReport";
            this.chkRecAuditReport.Size = new System.Drawing.Size(146, 19);
            this.chkRecAuditReport.TabIndex = 401;
            this.chkRecAuditReport.Text = "Received Audit Report";
            this.chkRecAuditReport.UseVisualStyleBackColor = true;
            // 
            // chkRecAuditAgenda
            // 
            this.chkRecAuditAgenda.AutoSize = true;
            this.chkRecAuditAgenda.Location = new System.Drawing.Point(395, 101);
            this.chkRecAuditAgenda.Name = "chkRecAuditAgenda";
            this.chkRecAuditAgenda.Size = new System.Drawing.Size(150, 19);
            this.chkRecAuditAgenda.TabIndex = 400;
            this.chkRecAuditAgenda.Text = "Received Audit Agenda";
            this.chkRecAuditAgenda.UseVisualStyleBackColor = true;
            // 
            // chkCND
            // 
            this.chkCND.AutoSize = true;
            this.chkCND.Location = new System.Drawing.Point(272, 119);
            this.chkCND.Name = "chkCND";
            this.chkCND.Size = new System.Drawing.Size(95, 19);
            this.chkCND.TabIndex = 399;
            this.chkCND.Text = "CND Signed";
            this.chkCND.UseVisualStyleBackColor = true;
            // 
            // chkASN
            // 
            this.chkASN.AutoSize = true;
            this.chkASN.Location = new System.Drawing.Point(272, 101);
            this.chkASN.Name = "chkASN";
            this.chkASN.Size = new System.Drawing.Size(92, 19);
            this.chkASN.TabIndex = 398;
            this.chkASN.Text = "ASN Signed";
            this.chkASN.UseVisualStyleBackColor = true;
            // 
            // cboStatus
            // 
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "Audit Not Yet Performed",
            "Waiting for Audit Report",
            "Audit Closed"});
            this.cboStatus.Location = new System.Drawing.Point(317, 72);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(275, 23);
            this.cboStatus.TabIndex = 397;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(266, 71);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(48, 21);
            this.label1.TabIndex = 396;
            this.label1.Text = "Status";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvEmpAuditors
            // 
            this.dgvEmpAuditors.AllowUserToAddRows = false;
            this.dgvEmpAuditors.AllowUserToDeleteRows = false;
            this.dgvEmpAuditors.BackgroundColor = System.Drawing.Color.White;
            this.dgvEmpAuditors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEmpAuditors.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvEmpAuditors.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvEmpAuditors.Location = new System.Drawing.Point(219, 161);
            this.dgvEmpAuditors.Name = "dgvEmpAuditors";
            this.dgvEmpAuditors.ReadOnly = true;
            this.dgvEmpAuditors.RowHeadersVisible = false;
            this.dgvEmpAuditors.Size = new System.Drawing.Size(520, 303);
            this.dgvEmpAuditors.TabIndex = 6;
            this.dgvEmpAuditors.Visible = false;
            this.dgvEmpAuditors.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEmpAuditors_CellContentClick);
            this.dgvEmpAuditors.DoubleClick += new System.EventHandler(this.dgvEmpAuditors_DoubleClick);
            this.dgvEmpAuditors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvEmpAuditors_KeyDown);
            this.dgvEmpAuditors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvEmpAuditors_KeyPress);
            this.dgvEmpAuditors.Leave += new System.EventHandler(this.dgvEmpAuditors_Leave);
            // 
            // dgvSponsors
            // 
            this.dgvSponsors.AllowUserToAddRows = false;
            this.dgvSponsors.AllowUserToDeleteRows = false;
            this.dgvSponsors.BackgroundColor = System.Drawing.Color.White;
            this.dgvSponsors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSponsors.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSponsors.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSponsors.Location = new System.Drawing.Point(219, 184);
            this.dgvSponsors.Name = "dgvSponsors";
            this.dgvSponsors.ReadOnly = true;
            this.dgvSponsors.RowHeadersVisible = false;
            this.dgvSponsors.Size = new System.Drawing.Size(520, 282);
            this.dgvSponsors.TabIndex = 9;
            this.dgvSponsors.Visible = false;
            this.dgvSponsors.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSponsors_CellContentClick);
            this.dgvSponsors.DoubleClick += new System.EventHandler(this.dgvSponsors_DoubleClick);
            this.dgvSponsors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSponsors_KeyDown);
            this.dgvSponsors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSponsors_KeyPress);
            this.dgvSponsors.Leave += new System.EventHandler(this.dgvSponsors_Leave);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(692, -2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 22);
            this.btnClose.TabIndex = 395;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // calAudit
            // 
            this.calAudit.Location = new System.Drawing.Point(498, 37);
            this.calAudit.Name = "calAudit";
            this.calAudit.TabIndex = 175;
            this.calAudit.Visible = false;
            this.calAudit.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.calAudit_DateSelected);
            this.calAudit.Leave += new System.EventHandler(this.calAudit_Leave);
            // 
            // txtSponsor
            // 
            this.txtSponsor.BackColor = System.Drawing.Color.White;
            this.txtSponsor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsor.Location = new System.Drawing.Point(219, 164);
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(502, 21);
            this.txtSponsor.TabIndex = 8;
            this.txtSponsor.TextChanged += new System.EventHandler(this.txtSponsors_TextChanged);
            this.txtSponsor.Enter += new System.EventHandler(this.txtSponsor_Enter);
            this.txtSponsor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsor_KeyPress);
            // 
            // picSponsors
            // 
            this.picSponsors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSponsors.BackgroundImage")));
            this.picSponsors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSponsors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSponsors.Location = new System.Drawing.Point(720, 164);
            this.picSponsors.Name = "picSponsors";
            this.picSponsors.Size = new System.Drawing.Size(19, 21);
            this.picSponsors.TabIndex = 171;
            this.picSponsors.TabStop = false;
            this.picSponsors.Click += new System.EventHandler(this.picSponsors_Click);
            // 
            // picEmpAuditors
            // 
            this.picEmpAuditors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picEmpAuditors.BackgroundImage")));
            this.picEmpAuditors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picEmpAuditors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picEmpAuditors.Location = new System.Drawing.Point(720, 141);
            this.picEmpAuditors.Name = "picEmpAuditors";
            this.picEmpAuditors.Size = new System.Drawing.Size(19, 21);
            this.picEmpAuditors.TabIndex = 170;
            this.picEmpAuditors.TabStop = false;
            this.picEmpAuditors.Click += new System.EventHandler(this.picEmpAuditors_Click);
            // 
            // lblDeficiencyDesc
            // 
            this.lblDeficiencyDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblDeficiencyDesc.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeficiencyDesc.ForeColor = System.Drawing.Color.Black;
            this.lblDeficiencyDesc.Location = new System.Drawing.Point(23, 345);
            this.lblDeficiencyDesc.Name = "lblDeficiencyDesc";
            this.lblDeficiencyDesc.Padding = new System.Windows.Forms.Padding(2);
            this.lblDeficiencyDesc.Size = new System.Drawing.Size(134, 35);
            this.lblDeficiencyDesc.TabIndex = 169;
            this.lblDeficiencyDesc.Text = "Description of Deficiencies";
            this.lblDeficiencyDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDeficiencyDesc
            // 
            this.txtDeficiencyDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeficiencyDesc.Location = new System.Drawing.Point(173, 345);
            this.txtDeficiencyDesc.Multiline = true;
            this.txtDeficiencyDesc.Name = "txtDeficiencyDesc";
            this.txtDeficiencyDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDeficiencyDesc.Size = new System.Drawing.Size(566, 111);
            this.txtDeficiencyDesc.TabIndex = 12;
            // 
            // lblDeficiencyNo
            // 
            this.lblDeficiencyNo.BackColor = System.Drawing.Color.Transparent;
            this.lblDeficiencyNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeficiencyNo.ForeColor = System.Drawing.Color.Black;
            this.lblDeficiencyNo.Location = new System.Drawing.Point(23, 322);
            this.lblDeficiencyNo.Name = "lblDeficiencyNo";
            this.lblDeficiencyNo.Padding = new System.Windows.Forms.Padding(2);
            this.lblDeficiencyNo.Size = new System.Drawing.Size(129, 21);
            this.lblDeficiencyNo.TabIndex = 167;
            this.lblDeficiencyNo.Text = "No. of Deficiencies";
            this.lblDeficiencyNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDeficiencyNo
            // 
            this.txtDeficiencyNo.BackColor = System.Drawing.Color.White;
            this.txtDeficiencyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeficiencyNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeficiencyNo.Location = new System.Drawing.Point(173, 322);
            this.txtDeficiencyNo.MaxLength = 3;
            this.txtDeficiencyNo.Name = "txtDeficiencyNo";
            this.txtDeficiencyNo.Size = new System.Drawing.Size(79, 21);
            this.txtDeficiencyNo.TabIndex = 11;
            this.txtDeficiencyNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDeficiencyNo_KeyPress);
            // 
            // lblCARCompleted
            // 
            this.lblCARCompleted.BackColor = System.Drawing.Color.Transparent;
            this.lblCARCompleted.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCARCompleted.ForeColor = System.Drawing.Color.Black;
            this.lblCARCompleted.Location = new System.Drawing.Point(20, 118);
            this.lblCARCompleted.Name = "lblCARCompleted";
            this.lblCARCompleted.Padding = new System.Windows.Forms.Padding(2);
            this.lblCARCompleted.Size = new System.Drawing.Size(129, 21);
            this.lblCARCompleted.TabIndex = 164;
            this.lblCARCompleted.Text = "Date CAR Completed";
            this.lblCARCompleted.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAuditorNames
            // 
            this.lblAuditorNames.BackColor = System.Drawing.Color.Transparent;
            this.lblAuditorNames.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuditorNames.ForeColor = System.Drawing.Color.Black;
            this.lblAuditorNames.Location = new System.Drawing.Point(20, 183);
            this.lblAuditorNames.Name = "lblAuditorNames";
            this.lblAuditorNames.Padding = new System.Windows.Forms.Padding(2);
            this.lblAuditorNames.Size = new System.Drawing.Size(151, 22);
            this.lblAuditorNames.TabIndex = 160;
            this.lblAuditorNames.Text = "Auditor Names";
            this.lblAuditorNames.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAuditorName
            // 
            this.txtAuditorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAuditorName.Location = new System.Drawing.Point(173, 187);
            this.txtAuditorName.Multiline = true;
            this.txtAuditorName.Name = "txtAuditorName";
            this.txtAuditorName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAuditorName.Size = new System.Drawing.Size(566, 133);
            this.txtAuditorName.TabIndex = 10;
            // 
            // lblAuditNo
            // 
            this.lblAuditNo.BackColor = System.Drawing.Color.Transparent;
            this.lblAuditNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuditNo.ForeColor = System.Drawing.Color.Black;
            this.lblAuditNo.Location = new System.Drawing.Point(23, 49);
            this.lblAuditNo.Margin = new System.Windows.Forms.Padding(0);
            this.lblAuditNo.Name = "lblAuditNo";
            this.lblAuditNo.Size = new System.Drawing.Size(127, 21);
            this.lblAuditNo.TabIndex = 157;
            this.lblAuditNo.Text = "Audit ID";
            this.lblAuditNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAuditID
            // 
            this.txtAuditID.BackColor = System.Drawing.Color.White;
            this.txtAuditID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAuditID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAuditID.ForeColor = System.Drawing.Color.Red;
            this.txtAuditID.Location = new System.Drawing.Point(173, 49);
            this.txtAuditID.MaxLength = 5;
            this.txtAuditID.Name = "txtAuditID";
            this.txtAuditID.Size = new System.Drawing.Size(79, 21);
            this.txtAuditID.TabIndex = 0;
            this.txtAuditID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSupervisor
            // 
            this.lblSupervisor.BackColor = System.Drawing.Color.Transparent;
            this.lblSupervisor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupervisor.ForeColor = System.Drawing.Color.Black;
            this.lblSupervisor.Location = new System.Drawing.Point(20, 162);
            this.lblSupervisor.Name = "lblSupervisor";
            this.lblSupervisor.Padding = new System.Windows.Forms.Padding(2);
            this.lblSupervisor.Size = new System.Drawing.Size(129, 21);
            this.lblSupervisor.TabIndex = 149;
            this.lblSupervisor.Text = "Sponsor";
            this.lblSupervisor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSponsorID
            // 
            this.txtSponsorID.BackColor = System.Drawing.Color.White;
            this.txtSponsorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsorID.Location = new System.Drawing.Point(173, 164);
            this.txtSponsorID.MaxLength = 5;
            this.txtSponsorID.Name = "txtSponsorID";
            this.txtSponsorID.Size = new System.Drawing.Size(47, 21);
            this.txtSponsorID.TabIndex = 7;
            this.txtSponsorID.Enter += new System.EventHandler(this.txtSponsorID_Enter);
            this.txtSponsorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsorID_KeyPress);
            // 
            // lblEmpAuditor
            // 
            this.lblEmpAuditor.BackColor = System.Drawing.Color.Transparent;
            this.lblEmpAuditor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpAuditor.ForeColor = System.Drawing.Color.Black;
            this.lblEmpAuditor.Location = new System.Drawing.Point(20, 141);
            this.lblEmpAuditor.Name = "lblEmpAuditor";
            this.lblEmpAuditor.Padding = new System.Windows.Forms.Padding(2);
            this.lblEmpAuditor.Size = new System.Drawing.Size(115, 21);
            this.lblEmpAuditor.TabIndex = 143;
            this.lblEmpAuditor.Text = "GBL Employee";
            this.lblEmpAuditor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtEmpAuditorID
            // 
            this.txtEmpAuditorID.BackColor = System.Drawing.Color.White;
            this.txtEmpAuditorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmpAuditorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpAuditorID.Location = new System.Drawing.Point(173, 141);
            this.txtEmpAuditorID.MaxLength = 5;
            this.txtEmpAuditorID.Name = "txtEmpAuditorID";
            this.txtEmpAuditorID.Size = new System.Drawing.Size(47, 21);
            this.txtEmpAuditorID.TabIndex = 4;
            this.txtEmpAuditorID.Enter += new System.EventHandler(this.txtEmpAuditorID_Enter);
            this.txtEmpAuditorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEmpAuditorID_KeyPress);
            // 
            // txtEmpAuditor
            // 
            this.txtEmpAuditor.BackColor = System.Drawing.Color.White;
            this.txtEmpAuditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmpAuditor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpAuditor.Location = new System.Drawing.Point(219, 141);
            this.txtEmpAuditor.Name = "txtEmpAuditor";
            this.txtEmpAuditor.Size = new System.Drawing.Size(502, 21);
            this.txtEmpAuditor.TabIndex = 5;
            this.txtEmpAuditor.TextChanged += new System.EventHandler(this.txtEmpAuditors_TextChanged);
            this.txtEmpAuditor.Enter += new System.EventHandler(this.txtEmpAuditor_Enter);
            this.txtEmpAuditor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEmpAuditor_KeyPress);
            // 
            // lblEntryDate
            // 
            this.lblEntryDate.BackColor = System.Drawing.Color.Transparent;
            this.lblEntryDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntryDate.ForeColor = System.Drawing.Color.Black;
            this.lblEntryDate.Location = new System.Drawing.Point(20, 95);
            this.lblEntryDate.Name = "lblEntryDate";
            this.lblEntryDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblEntryDate.Size = new System.Drawing.Size(115, 21);
            this.lblEntryDate.TabIndex = 135;
            this.lblEntryDate.Text = "Date Entered";
            this.lblEntryDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAuditDate
            // 
            this.lblAuditDate.BackColor = System.Drawing.Color.Transparent;
            this.lblAuditDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuditDate.ForeColor = System.Drawing.Color.Black;
            this.lblAuditDate.Location = new System.Drawing.Point(20, 72);
            this.lblAuditDate.Name = "lblAuditDate";
            this.lblAuditDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblAuditDate.Size = new System.Drawing.Size(76, 21);
            this.lblAuditDate.TabIndex = 5;
            this.lblAuditDate.Text = "Audit Date";
            this.lblAuditDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-4, -1);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(768, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "AUDIT LOG MASTER";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseDown);
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // AuditLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1354, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "AuditLog";
            this.Load += new System.EventHandler(this.AuditLog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AuditLog_KeyDown);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpAuditors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEmpAuditors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Label lblAuditorNames;
        private System.Windows.Forms.TextBox txtAuditorName;
        private System.Windows.Forms.Label lblAuditNo;
        private GISControls.TextBoxChar txtAuditID;
        private GISControls.TextBoxChar txtSponsor;
        private System.Windows.Forms.Label lblSupervisor;
        private GISControls.TextBoxChar txtSponsorID;
        private System.Windows.Forms.Label lblEmpAuditor;
        private GISControls.TextBoxChar txtEmpAuditorID;
        private GISControls.TextBoxChar txtEmpAuditor;
        private System.Windows.Forms.Label lblEntryDate;
        private System.Windows.Forms.Label lblAuditDate;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.DataGridView dgvEmpAuditors;
        private System.Windows.Forms.DataGridView dgvSponsors;
        private System.Windows.Forms.Label lblCARCompleted;
        private System.Windows.Forms.Label lblDeficiencyDesc;
        private System.Windows.Forms.TextBox txtDeficiencyDesc;
        private System.Windows.Forms.Label lblDeficiencyNo;
        private GISControls.TextBoxChar txtDeficiencyNo;
        private System.Windows.Forms.PictureBox picSponsors;
        private System.Windows.Forms.PictureBox picEmpAuditors;
        private System.Windows.Forms.MonthCalendar calAudit;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkRecAuditReport;
        private System.Windows.Forms.CheckBox chkRecAuditAgenda;
        private System.Windows.Forms.CheckBox chkCND;
        private System.Windows.Forms.CheckBox chkASN;
        private System.Windows.Forms.MaskedTextBox mskDateEntered;
        private System.Windows.Forms.MaskedTextBox mskAuditDate;
        private System.Windows.Forms.MaskedTextBox mskDateCARComp;
        private System.Windows.Forms.CheckBox chkContBuss;
    }
}
