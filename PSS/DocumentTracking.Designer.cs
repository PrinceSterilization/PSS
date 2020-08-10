namespace GIS
{
    partial class DocumentTracking
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentTracking));
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.txtDocNoDisp = new System.Windows.Forms.TextBox();
            this.pnlCalendar = new System.Windows.Forms.Panel();
            this.cal = new System.Windows.Forms.MonthCalendar();
            this.dgvSponsors = new System.Windows.Forms.DataGridView();
            this.btnOpenDoc = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.dgvContacts = new System.Windows.Forms.DataGridView();
            this.mskDateRet = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.picContacts = new System.Windows.Forms.PictureBox();
            this.txtDocDesc = new System.Windows.Forms.TextBox();
            this.lblDocDesc = new System.Windows.Forms.Label();
            this.mskDateExp = new System.Windows.Forms.MaskedTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mskDateMailed = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.picSponsors = new System.Windows.Forms.PictureBox();
            this.txtSponsorID = new GISControls.TextBoxChar();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.label5 = new System.Windows.Forms.Label();
            this.cboDocTypes = new System.Windows.Forms.ComboBox();
            this.lblCreatedBy = new System.Windows.Forms.Label();
            this.txtCreatedBy = new System.Windows.Forms.TextBox();
            this.lblContact = new System.Windows.Forms.Label();
            this.txtDocPath = new System.Windows.Forms.TextBox();
            this.mskDocDate = new System.Windows.Forms.MaskedTextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblDocType = new System.Windows.Forms.Label();
            this.txtContact = new System.Windows.Forms.TextBox();
            this.txtDocNo = new System.Windows.Forms.TextBox();
            this.lblDocDate = new System.Windows.Forms.Label();
            this.lblDocNo = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.bsMaster = new System.Windows.Forms.BindingSource(this.components);
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            this.pnlCalendar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContacts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContacts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMaster)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.txtDocNoDisp);
            this.pnlRecord.Controls.Add(this.pnlCalendar);
            this.pnlRecord.Controls.Add(this.dgvSponsors);
            this.pnlRecord.Controls.Add(this.btnOpenDoc);
            this.pnlRecord.Controls.Add(this.btnBrowse);
            this.pnlRecord.Controls.Add(this.dgvContacts);
            this.pnlRecord.Controls.Add(this.mskDateRet);
            this.pnlRecord.Controls.Add(this.label4);
            this.pnlRecord.Controls.Add(this.picContacts);
            this.pnlRecord.Controls.Add(this.txtDocDesc);
            this.pnlRecord.Controls.Add(this.lblDocDesc);
            this.pnlRecord.Controls.Add(this.mskDateExp);
            this.pnlRecord.Controls.Add(this.label3);
            this.pnlRecord.Controls.Add(this.mskDateMailed);
            this.pnlRecord.Controls.Add(this.label2);
            this.pnlRecord.Controls.Add(this.label1);
            this.pnlRecord.Controls.Add(this.picSponsors);
            this.pnlRecord.Controls.Add(this.txtSponsorID);
            this.pnlRecord.Controls.Add(this.txtSponsor);
            this.pnlRecord.Controls.Add(this.label5);
            this.pnlRecord.Controls.Add(this.cboDocTypes);
            this.pnlRecord.Controls.Add(this.lblCreatedBy);
            this.pnlRecord.Controls.Add(this.txtCreatedBy);
            this.pnlRecord.Controls.Add(this.lblContact);
            this.pnlRecord.Controls.Add(this.txtDocPath);
            this.pnlRecord.Controls.Add(this.mskDocDate);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.lblDocType);
            this.pnlRecord.Controls.Add(this.txtContact);
            this.pnlRecord.Controls.Add(this.txtDocNo);
            this.pnlRecord.Controls.Add(this.lblDocDate);
            this.pnlRecord.Controls.Add(this.lblDocNo);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Location = new System.Drawing.Point(22, 91);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(623, 394);
            this.pnlRecord.TabIndex = 111;
            this.pnlRecord.Visible = false;
            // 
            // txtDocNoDisp
            // 
            this.txtDocNoDisp.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocNoDisp.Location = new System.Drawing.Point(146, 41);
            this.txtDocNoDisp.Name = "txtDocNoDisp";
            this.txtDocNoDisp.ReadOnly = true;
            this.txtDocNoDisp.Size = new System.Drawing.Size(70, 21);
            this.txtDocNoDisp.TabIndex = 458;
            this.txtDocNoDisp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pnlCalendar
            // 
            this.pnlCalendar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCalendar.Controls.Add(this.cal);
            this.pnlCalendar.Location = new System.Drawing.Point(276, 93);
            this.pnlCalendar.Name = "pnlCalendar";
            this.pnlCalendar.Size = new System.Drawing.Size(246, 185);
            this.pnlCalendar.TabIndex = 14;
            this.pnlCalendar.Visible = false;
            // 
            // cal
            // 
            this.cal.Location = new System.Drawing.Point(9, 9);
            this.cal.Name = "cal";
            this.cal.TabIndex = 277;
            this.cal.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.cal_DateSelected);
            this.cal.MouseLeave += new System.EventHandler(this.cal_MouseLeave);
            // 
            // dgvSponsors
            // 
            this.dgvSponsors.AllowUserToAddRows = false;
            this.dgvSponsors.AllowUserToDeleteRows = false;
            this.dgvSponsors.BackgroundColor = System.Drawing.Color.White;
            this.dgvSponsors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSponsors.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSponsors.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSponsors.Location = new System.Drawing.Point(213, 183);
            this.dgvSponsors.Name = "dgvSponsors";
            this.dgvSponsors.ReadOnly = true;
            this.dgvSponsors.RowHeadersVisible = false;
            this.dgvSponsors.Size = new System.Drawing.Size(380, 163);
            this.dgvSponsors.TabIndex = 8;
            this.dgvSponsors.Visible = false;
            this.dgvSponsors.DoubleClick += new System.EventHandler(this.dgvSponsors_DoubleClick);
            this.dgvSponsors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSponsors_KeyDown);
            this.dgvSponsors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSponsors_KeyPress);
            this.dgvSponsors.Leave += new System.EventHandler(this.dgvSponsors_Leave);
            // 
            // btnOpenDoc
            // 
            this.btnOpenDoc.Location = new System.Drawing.Point(473, 340);
            this.btnOpenDoc.Name = "btnOpenDoc";
            this.btnOpenDoc.Size = new System.Drawing.Size(120, 26);
            this.btnOpenDoc.TabIndex = 457;
            this.btnOpenDoc.Text = "&Open Document";
            this.btnOpenDoc.UseVisualStyleBackColor = true;
            this.btnOpenDoc.Click += new System.EventHandler(this.btnOpenDoc_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(522, 138);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(71, 22);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // dgvContacts
            // 
            this.dgvContacts.AllowUserToAddRows = false;
            this.dgvContacts.AllowUserToDeleteRows = false;
            this.dgvContacts.BackgroundColor = System.Drawing.Color.White;
            this.dgvContacts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContacts.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvContacts.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvContacts.Location = new System.Drawing.Point(146, 207);
            this.dgvContacts.Name = "dgvContacts";
            this.dgvContacts.ReadOnly = true;
            this.dgvContacts.RowHeadersVisible = false;
            this.dgvContacts.Size = new System.Drawing.Size(204, 127);
            this.dgvContacts.TabIndex = 10;
            this.dgvContacts.Visible = false;
            this.dgvContacts.DoubleClick += new System.EventHandler(this.dgvContacts_DoubleClick);
            this.dgvContacts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvContacts_KeyDown);
            this.dgvContacts.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvContacts_KeyPress);
            // 
            // mskDateRet
            // 
            this.mskDateRet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mskDateRet.Location = new System.Drawing.Point(146, 259);
            this.mskDateRet.Mask = "00/00/0000";
            this.mskDateRet.Name = "mskDateRet";
            this.mskDateRet.RejectInputOnFirstFailure = true;
            this.mskDateRet.Size = new System.Drawing.Size(73, 21);
            this.mskDateRet.TabIndex = 13;
            this.mskDateRet.ValidatingType = typeof(System.DateTime);
            this.mskDateRet.Click += new System.EventHandler(this.mskDateRet_Click);
            this.mskDateRet.Enter += new System.EventHandler(this.mskDateRet_Enter);
            this.mskDateRet.Leave += new System.EventHandler(this.mskDateRet_Leave);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(23, 259);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(2);
            this.label4.Size = new System.Drawing.Size(112, 21);
            this.label4.TabIndex = 455;
            this.label4.Text = "Date Returned";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picContacts
            // 
            this.picContacts.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picContacts.BackgroundImage")));
            this.picContacts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picContacts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picContacts.Location = new System.Drawing.Point(331, 187);
            this.picContacts.Name = "picContacts";
            this.picContacts.Size = new System.Drawing.Size(19, 21);
            this.picContacts.TabIndex = 452;
            this.picContacts.TabStop = false;
            this.picContacts.Click += new System.EventHandler(this.picContacts_Click);
            // 
            // txtDocDesc
            // 
            this.txtDocDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDocDesc.Location = new System.Drawing.Point(146, 283);
            this.txtDocDesc.MaxLength = 150;
            this.txtDocDesc.Multiline = true;
            this.txtDocDesc.Name = "txtDocDesc";
            this.txtDocDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDocDesc.Size = new System.Drawing.Size(449, 51);
            this.txtDocDesc.TabIndex = 14;
            // 
            // lblDocDesc
            // 
            this.lblDocDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblDocDesc.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocDesc.ForeColor = System.Drawing.Color.Black;
            this.lblDocDesc.Location = new System.Drawing.Point(23, 283);
            this.lblDocDesc.Name = "lblDocDesc";
            this.lblDocDesc.Padding = new System.Windows.Forms.Padding(2);
            this.lblDocDesc.Size = new System.Drawing.Size(112, 21);
            this.lblDocDesc.TabIndex = 450;
            this.lblDocDesc.Text = "Description";
            this.lblDocDesc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mskDateExp
            // 
            this.mskDateExp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mskDateExp.Location = new System.Drawing.Point(146, 235);
            this.mskDateExp.Mask = "00/00/0000";
            this.mskDateExp.Name = "mskDateExp";
            this.mskDateExp.RejectInputOnFirstFailure = true;
            this.mskDateExp.Size = new System.Drawing.Size(73, 21);
            this.mskDateExp.TabIndex = 12;
            this.mskDateExp.ValidatingType = typeof(System.DateTime);
            this.mskDateExp.Click += new System.EventHandler(this.mskDateExp_Click);
            this.mskDateExp.Enter += new System.EventHandler(this.mskDateExp_Enter);
            this.mskDateExp.Leave += new System.EventHandler(this.mskDateExp_Leave);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(23, 235);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(112, 21);
            this.label3.TabIndex = 448;
            this.label3.Text = "Date Expires";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mskDateMailed
            // 
            this.mskDateMailed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mskDateMailed.Location = new System.Drawing.Point(146, 211);
            this.mskDateMailed.Mask = "00/00/0000";
            this.mskDateMailed.Name = "mskDateMailed";
            this.mskDateMailed.RejectInputOnFirstFailure = true;
            this.mskDateMailed.Size = new System.Drawing.Size(73, 21);
            this.mskDateMailed.TabIndex = 11;
            this.mskDateMailed.Click += new System.EventHandler(this.mskDateMailed_Click);
            this.mskDateMailed.Enter += new System.EventHandler(this.mskDateMailed_Enter);
            this.mskDateMailed.Leave += new System.EventHandler(this.mskDateMailed_Leave);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(23, 211);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(2);
            this.label2.Size = new System.Drawing.Size(112, 21);
            this.label2.TabIndex = 446;
            this.label2.Text = "Date Mailed";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(23, 139);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(118, 21);
            this.label1.TabIndex = 444;
            this.label1.Text = "File Name/Location";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picSponsors
            // 
            this.picSponsors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSponsors.BackgroundImage")));
            this.picSponsors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSponsors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSponsors.Location = new System.Drawing.Point(574, 163);
            this.picSponsors.Name = "picSponsors";
            this.picSponsors.Size = new System.Drawing.Size(19, 21);
            this.picSponsors.TabIndex = 443;
            this.picSponsors.TabStop = false;
            this.picSponsors.Click += new System.EventHandler(this.picSponsors_Click);
            // 
            // txtSponsorID
            // 
            this.txtSponsorID.BackColor = System.Drawing.Color.White;
            this.txtSponsorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsorID.Location = new System.Drawing.Point(146, 163);
            this.txtSponsorID.MaxLength = 5;
            this.txtSponsorID.Name = "txtSponsorID";
            this.txtSponsorID.Size = new System.Drawing.Size(68, 21);
            this.txtSponsorID.TabIndex = 6;
            this.txtSponsorID.Enter += new System.EventHandler(this.txtSponsorID_Enter);
            this.txtSponsorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsorID_KeyPress);
            // 
            // txtSponsor
            // 
            this.txtSponsor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsor.Location = new System.Drawing.Point(213, 163);
            this.txtSponsor.MaxLength = 50;
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(362, 21);
            this.txtSponsor.TabIndex = 7;
            this.txtSponsor.TextChanged += new System.EventHandler(this.txtSponsor_TextChanged);
            this.txtSponsor.Enter += new System.EventHandler(this.txtSponsor_Enter);
            this.txtSponsor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsor_KeyPress);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(23, 163);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(2);
            this.label5.Size = new System.Drawing.Size(112, 21);
            this.label5.TabIndex = 442;
            this.label5.Text = "Sponsor ID/Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboDocTypes
            // 
            this.cboDocTypes.FormattingEnabled = true;
            this.cboDocTypes.Items.AddRange(new object[] {
            "Letter",
            "Memo",
            "Quality Agreement",
            "Non-Disclosure Agreement",
            "Protocol"});
            this.cboDocTypes.Location = new System.Drawing.Point(146, 64);
            this.cboDocTypes.Name = "cboDocTypes";
            this.cboDocTypes.Size = new System.Drawing.Size(186, 23);
            this.cboDocTypes.TabIndex = 1;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.BackColor = System.Drawing.Color.Transparent;
            this.lblCreatedBy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.ForeColor = System.Drawing.Color.Black;
            this.lblCreatedBy.Location = new System.Drawing.Point(23, 115);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Padding = new System.Windows.Forms.Padding(2);
            this.lblCreatedBy.Size = new System.Drawing.Size(112, 21);
            this.lblCreatedBy.TabIndex = 438;
            this.lblCreatedBy.Text = "Created By";
            this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCreatedBy
            // 
            this.txtCreatedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreatedBy.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreatedBy.Location = new System.Drawing.Point(146, 115);
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.ReadOnly = true;
            this.txtCreatedBy.Size = new System.Drawing.Size(186, 21);
            this.txtCreatedBy.TabIndex = 3;
            // 
            // lblContact
            // 
            this.lblContact.BackColor = System.Drawing.Color.Transparent;
            this.lblContact.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContact.ForeColor = System.Drawing.Color.Black;
            this.lblContact.Location = new System.Drawing.Point(23, 187);
            this.lblContact.Name = "lblContact";
            this.lblContact.Padding = new System.Windows.Forms.Padding(2);
            this.lblContact.Size = new System.Drawing.Size(112, 21);
            this.lblContact.TabIndex = 426;
            this.lblContact.Text = "Contact Name";
            this.lblContact.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDocPath
            // 
            this.txtDocPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDocPath.Location = new System.Drawing.Point(146, 139);
            this.txtDocPath.Name = "txtDocPath";
            this.txtDocPath.Size = new System.Drawing.Size(370, 21);
            this.txtDocPath.TabIndex = 4;
            this.txtDocPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDocPath_KeyPress);
            // 
            // mskDocDate
            // 
            this.mskDocDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mskDocDate.Location = new System.Drawing.Point(146, 90);
            this.mskDocDate.Mask = "00/00/0000";
            this.mskDocDate.Name = "mskDocDate";
            this.mskDocDate.RejectInputOnFirstFailure = true;
            this.mskDocDate.Size = new System.Drawing.Size(73, 21);
            this.mskDocDate.TabIndex = 2;
            this.mskDocDate.ValidatingType = typeof(System.DateTime);
            this.mskDocDate.Click += new System.EventHandler(this.mskDocDate_Click);
            this.mskDocDate.Enter += new System.EventHandler(this.mskDocDate_Enter);
            this.mskDocDate.Leave += new System.EventHandler(this.mskDocDate_Leave);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(551, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 22);
            this.btnClose.TabIndex = 394;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblDocType
            // 
            this.lblDocType.BackColor = System.Drawing.Color.Transparent;
            this.lblDocType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocType.ForeColor = System.Drawing.Color.Black;
            this.lblDocType.Location = new System.Drawing.Point(23, 64);
            this.lblDocType.Name = "lblDocType";
            this.lblDocType.Padding = new System.Windows.Forms.Padding(2);
            this.lblDocType.Size = new System.Drawing.Size(112, 21);
            this.lblDocType.TabIndex = 189;
            this.lblDocType.Text = "Document Type";
            this.lblDocType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtContact
            // 
            this.txtContact.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContact.Location = new System.Drawing.Point(146, 187);
            this.txtContact.Name = "txtContact";
            this.txtContact.Size = new System.Drawing.Size(186, 21);
            this.txtContact.TabIndex = 9;
            this.txtContact.TextChanged += new System.EventHandler(this.txtContact_TextChanged);
            this.txtContact.Enter += new System.EventHandler(this.txtContact_Enter);
            this.txtContact.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtContact_KeyPress);
            // 
            // txtDocNo
            // 
            this.txtDocNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocNo.Location = new System.Drawing.Point(146, 42);
            this.txtDocNo.Name = "txtDocNo";
            this.txtDocNo.ReadOnly = true;
            this.txtDocNo.Size = new System.Drawing.Size(70, 21);
            this.txtDocNo.TabIndex = 0;
            this.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblDocDate
            // 
            this.lblDocDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDocDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocDate.ForeColor = System.Drawing.Color.Black;
            this.lblDocDate.Location = new System.Drawing.Point(23, 90);
            this.lblDocDate.Name = "lblDocDate";
            this.lblDocDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblDocDate.Size = new System.Drawing.Size(112, 21);
            this.lblDocDate.TabIndex = 166;
            this.lblDocDate.Text = "Date Created";
            this.lblDocDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDocNo
            // 
            this.lblDocNo.BackColor = System.Drawing.Color.Transparent;
            this.lblDocNo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocNo.ForeColor = System.Drawing.Color.Red;
            this.lblDocNo.Location = new System.Drawing.Point(23, 42);
            this.lblDocNo.Name = "lblDocNo";
            this.lblDocNo.Padding = new System.Windows.Forms.Padding(2);
            this.lblDocNo.Size = new System.Drawing.Size(105, 18);
            this.lblDocNo.TabIndex = 162;
            this.lblDocNo.Text = "Document No.";
            this.lblDocNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(623, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "DOCUMENT DATA";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ofdFile
            // 
            this.ofdFile.FileName = "openFileDialog1";
            // 
            // DocumentTracking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1916, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "DocumentTracking";
            this.Load += new System.EventHandler(this.DocumentTracking_Load);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            this.pnlCalendar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContacts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContacts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsMaster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.ComboBox cboDocTypes;
        private System.Windows.Forms.Panel pnlCalendar;
        private System.Windows.Forms.MonthCalendar cal;
        private System.Windows.Forms.Label lblCreatedBy;
        private System.Windows.Forms.TextBox txtCreatedBy;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.TextBox txtDocPath;
        private System.Windows.Forms.MaskedTextBox mskDocDate;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblDocType;
        private System.Windows.Forms.TextBox txtContact;
        private System.Windows.Forms.TextBox txtDocNo;
        private System.Windows.Forms.Label lblDocDate;
        private System.Windows.Forms.Label lblDocNo;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox picContacts;
        private System.Windows.Forms.DataGridView dgvSponsors;
        private System.Windows.Forms.TextBox txtDocDesc;
        private System.Windows.Forms.Label lblDocDesc;
        private System.Windows.Forms.MaskedTextBox mskDateExp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox mskDateMailed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picSponsors;
        private GISControls.TextBoxChar txtSponsorID;
        private GISControls.TextBoxChar txtSponsor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvContacts;
        private System.Windows.Forms.MaskedTextBox mskDateRet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.BindingSource bsMaster;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.OpenFileDialog ofdFile;
        private System.Windows.Forms.Button btnOpenDoc;
        private System.Windows.Forms.TextBox txtDocNoDisp;
    }
}
