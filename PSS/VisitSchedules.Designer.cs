namespace PSS
{
    partial class VisitSchedules
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VisitSchedules));
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.chkCancelled = new System.Windows.Forms.CheckBox();
            this.dgvSponsors = new System.Windows.Forms.DataGridView();
            this.dgvVisitors = new System.Windows.Forms.DataGridView();
            this.btnOpen = new System.Windows.Forms.Button();
            this.picVisitors = new System.Windows.Forms.PictureBox();
            this.txtSponsorID = new GISControls.TextBoxChar();
            this.picSponsors = new System.Windows.Forms.PictureBox();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.txtVisitor = new GISControls.TextBoxChar();
            this.txtVisitorID = new GISControls.TextBoxChar();
            this.pnlPurpose = new System.Windows.Forms.Panel();
            this.rdoVisit = new System.Windows.Forms.RadioButton();
            this.rdoInspection = new System.Windows.Forms.RadioButton();
            this.rdoAudit = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.pnlVisitorType = new System.Windows.Forms.Panel();
            this.rdoOthers = new System.Windows.Forms.RadioButton();
            this.rdoClient = new System.Windows.Forms.RadioButton();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtAgendaFile = new GISControls.TextBoxChar();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bsSponsors = new System.Windows.Forms.BindingSource(this.components);
            this.opnFile = new System.Windows.Forms.OpenFileDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisitors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVisitors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).BeginInit();
            this.pnlPurpose.SuspendLayout();
            this.pnlVisitorType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSponsors)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.chkCancelled);
            this.pnlRecord.Controls.Add(this.dgvSponsors);
            this.pnlRecord.Controls.Add(this.dgvVisitors);
            this.pnlRecord.Controls.Add(this.btnOpen);
            this.pnlRecord.Controls.Add(this.picVisitors);
            this.pnlRecord.Controls.Add(this.txtSponsorID);
            this.pnlRecord.Controls.Add(this.picSponsors);
            this.pnlRecord.Controls.Add(this.txtSponsor);
            this.pnlRecord.Controls.Add(this.txtVisitor);
            this.pnlRecord.Controls.Add(this.txtVisitorID);
            this.pnlRecord.Controls.Add(this.pnlPurpose);
            this.pnlRecord.Controls.Add(this.label8);
            this.pnlRecord.Controls.Add(this.label4);
            this.pnlRecord.Controls.Add(this.dtpEndTime);
            this.pnlRecord.Controls.Add(this.label3);
            this.pnlRecord.Controls.Add(this.dtpStartTime);
            this.pnlRecord.Controls.Add(this.pnlVisitorType);
            this.pnlRecord.Controls.Add(this.dtpDate);
            this.pnlRecord.Controls.Add(this.label2);
            this.pnlRecord.Controls.Add(this.label1);
            this.pnlRecord.Controls.Add(this.btnBrowse);
            this.pnlRecord.Controls.Add(this.label5);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtAgendaFile);
            this.pnlRecord.Controls.Add(this.label6);
            this.pnlRecord.Controls.Add(this.label7);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Location = new System.Drawing.Point(23, 91);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(610, 386);
            this.pnlRecord.TabIndex = 106;
            this.pnlRecord.Visible = false;
            // 
            // chkCancelled
            // 
            this.chkCancelled.AutoSize = true;
            this.chkCancelled.Location = new System.Drawing.Point(245, 41);
            this.chkCancelled.Name = "chkCancelled";
            this.chkCancelled.Size = new System.Drawing.Size(82, 19);
            this.chkCancelled.TabIndex = 17;
            this.chkCancelled.Text = "Cancelled";
            this.chkCancelled.UseVisualStyleBackColor = true;
            // 
            // dgvSponsors
            // 
            this.dgvSponsors.AllowUserToAddRows = false;
            this.dgvSponsors.AllowUserToDeleteRows = false;
            this.dgvSponsors.BackgroundColor = System.Drawing.Color.White;
            this.dgvSponsors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSponsors.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSponsors.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSponsors.Location = new System.Drawing.Point(194, 179);
            this.dgvSponsors.Name = "dgvSponsors";
            this.dgvSponsors.ReadOnly = true;
            this.dgvSponsors.RowHeadersVisible = false;
            this.dgvSponsors.Size = new System.Drawing.Size(391, 181);
            this.dgvSponsors.TabIndex = 10;
            this.dgvSponsors.DoubleClick += new System.EventHandler(this.dgvSponsors_DoubleClick);
            this.dgvSponsors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSponsors_KeyDown);
            this.dgvSponsors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSponsors_KeyPress);
            this.dgvSponsors.Leave += new System.EventHandler(this.dgvSponsors_Leave);
            // 
            // dgvVisitors
            // 
            this.dgvVisitors.AllowUserToAddRows = false;
            this.dgvVisitors.AllowUserToDeleteRows = false;
            this.dgvVisitors.BackgroundColor = System.Drawing.Color.White;
            this.dgvVisitors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVisitors.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvVisitors.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvVisitors.Location = new System.Drawing.Point(194, 203);
            this.dgvVisitors.Name = "dgvVisitors";
            this.dgvVisitors.ReadOnly = true;
            this.dgvVisitors.RowHeadersVisible = false;
            this.dgvVisitors.Size = new System.Drawing.Size(391, 157);
            this.dgvVisitors.TabIndex = 13;
            this.dgvVisitors.DoubleClick += new System.EventHandler(this.dgvVisitors_DoubleClick);
            this.dgvVisitors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVisitors_KeyDown);
            this.dgvVisitors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvVisitors_KeyPress);
            this.dgvVisitors.Leave += new System.EventHandler(this.dgvVisitors_Leave);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(517, 248);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(68, 24);
            this.btnOpen.TabIndex = 16;
            this.btnOpen.Text = "&Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // picVisitors
            // 
            this.picVisitors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picVisitors.BackgroundImage")));
            this.picVisitors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picVisitors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picVisitors.Location = new System.Drawing.Point(566, 183);
            this.picVisitors.Name = "picVisitors";
            this.picVisitors.Size = new System.Drawing.Size(19, 21);
            this.picVisitors.TabIndex = 140;
            this.picVisitors.TabStop = false;
            this.picVisitors.Click += new System.EventHandler(this.picVisitors_Click);
            // 
            // txtSponsorID
            // 
            this.txtSponsorID.BackColor = System.Drawing.Color.White;
            this.txtSponsorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsorID.Location = new System.Drawing.Point(138, 159);
            this.txtSponsorID.Name = "txtSponsorID";
            this.txtSponsorID.Size = new System.Drawing.Size(57, 21);
            this.txtSponsorID.TabIndex = 8;
            this.txtSponsorID.TextChanged += new System.EventHandler(this.txtSponsorID_TextChanged);
            this.txtSponsorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsorID_KeyPress);
            // 
            // picSponsors
            // 
            this.picSponsors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSponsors.BackgroundImage")));
            this.picSponsors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSponsors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSponsors.Location = new System.Drawing.Point(566, 159);
            this.picSponsors.Name = "picSponsors";
            this.picSponsors.Size = new System.Drawing.Size(19, 21);
            this.picSponsors.TabIndex = 136;
            this.picSponsors.TabStop = false;
            this.picSponsors.Click += new System.EventHandler(this.picSponsors_Click);
            // 
            // txtSponsor
            // 
            this.txtSponsor.BackColor = System.Drawing.Color.White;
            this.txtSponsor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsor.Location = new System.Drawing.Point(194, 159);
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(373, 21);
            this.txtSponsor.TabIndex = 9;
            this.txtSponsor.TextChanged += new System.EventHandler(this.txtSponsor_TextChanged_1);
            this.txtSponsor.Enter += new System.EventHandler(this.txtSponsor_Enter);
            this.txtSponsor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsor_KeyPress);
            // 
            // txtVisitor
            // 
            this.txtVisitor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVisitor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVisitor.Location = new System.Drawing.Point(194, 183);
            this.txtVisitor.MaxLength = 50;
            this.txtVisitor.Name = "txtVisitor";
            this.txtVisitor.Size = new System.Drawing.Size(373, 21);
            this.txtVisitor.TabIndex = 12;
            this.txtVisitor.TextChanged += new System.EventHandler(this.txtVisitorsName_TextChanged);
            this.txtVisitor.Enter += new System.EventHandler(this.txtVisitorsName_Enter);
            this.txtVisitor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVistorsName_KeyPress);
            // 
            // txtVisitorID
            // 
            this.txtVisitorID.BackColor = System.Drawing.Color.White;
            this.txtVisitorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVisitorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVisitorID.Location = new System.Drawing.Point(138, 183);
            this.txtVisitorID.MaxLength = 5;
            this.txtVisitorID.Name = "txtVisitorID";
            this.txtVisitorID.Size = new System.Drawing.Size(58, 21);
            this.txtVisitorID.TabIndex = 11;
            // 
            // pnlPurpose
            // 
            this.pnlPurpose.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPurpose.Controls.Add(this.rdoVisit);
            this.pnlPurpose.Controls.Add(this.rdoInspection);
            this.pnlPurpose.Controls.Add(this.rdoAudit);
            this.pnlPurpose.Location = new System.Drawing.Point(138, 134);
            this.pnlPurpose.Name = "pnlPurpose";
            this.pnlPurpose.Size = new System.Drawing.Size(251, 22);
            this.pnlPurpose.TabIndex = 4;
            // 
            // rdoVisit
            // 
            this.rdoVisit.AutoSize = true;
            this.rdoVisit.Location = new System.Drawing.Point(187, 0);
            this.rdoVisit.Name = "rdoVisit";
            this.rdoVisit.Size = new System.Drawing.Size(48, 19);
            this.rdoVisit.TabIndex = 7;
            this.rdoVisit.TabStop = true;
            this.rdoVisit.Text = "Visit";
            this.rdoVisit.UseVisualStyleBackColor = true;
            // 
            // rdoInspection
            // 
            this.rdoInspection.AutoSize = true;
            this.rdoInspection.Location = new System.Drawing.Point(84, 0);
            this.rdoInspection.Name = "rdoInspection";
            this.rdoInspection.Size = new System.Drawing.Size(82, 19);
            this.rdoInspection.TabIndex = 6;
            this.rdoInspection.TabStop = true;
            this.rdoInspection.Text = "Inspection";
            this.rdoInspection.UseVisualStyleBackColor = true;
            // 
            // rdoAudit
            // 
            this.rdoAudit.AutoSize = true;
            this.rdoAudit.Location = new System.Drawing.Point(7, 0);
            this.rdoAudit.Name = "rdoAudit";
            this.rdoAudit.Size = new System.Drawing.Size(52, 19);
            this.rdoAudit.TabIndex = 5;
            this.rdoAudit.TabStop = true;
            this.rdoAudit.Text = "Audit";
            this.rdoAudit.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(18, 137);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(2);
            this.label8.Size = new System.Drawing.Size(118, 18);
            this.label8.TabIndex = 129;
            this.label8.Text = "Purpose";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(18, 87);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(2);
            this.label4.Size = new System.Drawing.Size(118, 18);
            this.label4.TabIndex = 128;
            this.label4.Text = "End Time";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "hh:mm: tt";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(138, 85);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.ShowUpDown = true;
            this.dtpEndTime.Size = new System.Drawing.Size(98, 21);
            this.dtpEndTime.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(18, 63);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(118, 19);
            this.label3.TabIndex = 126;
            this.label3.Text = "Start Time";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "hh:mm: tt";
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(138, 61);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(98, 21);
            this.dtpStartTime.TabIndex = 1;
            // 
            // pnlVisitorType
            // 
            this.pnlVisitorType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVisitorType.Controls.Add(this.rdoOthers);
            this.pnlVisitorType.Controls.Add(this.rdoClient);
            this.pnlVisitorType.Location = new System.Drawing.Point(138, 110);
            this.pnlVisitorType.Name = "pnlVisitorType";
            this.pnlVisitorType.Size = new System.Drawing.Size(175, 22);
            this.pnlVisitorType.TabIndex = 3;
            // 
            // rdoOthers
            // 
            this.rdoOthers.AutoSize = true;
            this.rdoOthers.Location = new System.Drawing.Point(84, 0);
            this.rdoOthers.Name = "rdoOthers";
            this.rdoOthers.Size = new System.Drawing.Size(62, 19);
            this.rdoOthers.TabIndex = 4;
            this.rdoOthers.TabStop = true;
            this.rdoOthers.Text = "Others";
            this.rdoOthers.UseVisualStyleBackColor = true;
            this.rdoOthers.CheckedChanged += new System.EventHandler(this.rdoOthers_CheckedChanged);
            // 
            // rdoClient
            // 
            this.rdoClient.AutoSize = true;
            this.rdoClient.Location = new System.Drawing.Point(7, 0);
            this.rdoClient.Name = "rdoClient";
            this.rdoClient.Size = new System.Drawing.Size(72, 19);
            this.rdoClient.TabIndex = 3;
            this.rdoClient.TabStop = true;
            this.rdoClient.Text = "Sponsor";
            this.rdoClient.UseVisualStyleBackColor = true;
            this.rdoClient.CheckedChanged += new System.EventHandler(this.rdoClient_CheckedChanged);
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "MM/dd/yyyy";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(138, 38);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(100, 21);
            this.dtpDate.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(18, 112);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(2);
            this.label2.Size = new System.Drawing.Size(118, 18);
            this.label2.TabIndex = 122;
            this.label2.Text = "Visitor Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(18, 185);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(118, 18);
            this.label1.TabIndex = 121;
            this.label1.Text = "Visitor\'s ID/Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(443, 248);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(68, 24);
            this.btnBrowse.TabIndex = 15;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(18, 161);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(2);
            this.label5.Size = new System.Drawing.Size(118, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = "Sponsor ID/Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(532, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtAgendaFile
            // 
            this.txtAgendaFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAgendaFile.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAgendaFile.Location = new System.Drawing.Point(138, 207);
            this.txtAgendaFile.MaxLength = 255;
            this.txtAgendaFile.Multiline = true;
            this.txtAgendaFile.Name = "txtAgendaFile";
            this.txtAgendaFile.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAgendaFile.Size = new System.Drawing.Size(447, 35);
            this.txtAgendaFile.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(18, 207);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(2);
            this.label6.Size = new System.Drawing.Size(118, 21);
            this.label6.TabIndex = 5;
            this.label6.Text = "Agenda File Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(18, 40);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(2);
            this.label7.Size = new System.Drawing.Size(118, 21);
            this.label7.TabIndex = 3;
            this.label7.Text = "Date of Visit";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(612, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "VISIT SCHEDULE";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // opnFile
            // 
            this.opnFile.FileName = "opnFile";
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // VisitSchedules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1916, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "VisitSchedules";
            this.Tag = "VisitSchedules";
            this.Activated += new System.EventHandler(this.VisitSchedules_Activated);
            this.Load += new System.EventHandler(this.VisitSchedules_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VisitSchedules_KeyDown);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisitors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVisitors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).EndInit();
            this.pnlPurpose.ResumeLayout(false);
            this.pnlPurpose.PerformLayout();
            this.pnlVisitorType.ResumeLayout(false);
            this.pnlVisitorType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSponsors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClose;
        private GISControls.TextBoxChar txtAgendaFile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel pnlVisitorType;
        private System.Windows.Forms.RadioButton rdoOthers;
        private System.Windows.Forms.RadioButton rdoClient;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.BindingSource bsSponsors;
        private System.Windows.Forms.Panel pnlPurpose;
        private System.Windows.Forms.RadioButton rdoInspection;
        private System.Windows.Forms.RadioButton rdoAudit;
        private System.Windows.Forms.Label label8;
        private GISControls.TextBoxChar txtVisitor;
        private GISControls.TextBoxChar txtVisitorID;
        private GISControls.TextBoxChar txtSponsor;
        private System.Windows.Forms.PictureBox picSponsors;
        private GISControls.TextBoxChar txtSponsorID;
        private System.Windows.Forms.DataGridView dgvSponsors;
        private System.Windows.Forms.DataGridView dgvVisitors;
        private System.Windows.Forms.OpenFileDialog opnFile;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PictureBox picVisitors;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.RadioButton rdoVisit;
        private System.Windows.Forms.CheckBox chkCancelled;
    }
}
