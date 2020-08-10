namespace GIS
{
    partial class IngredionInvEntries
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IngredionInvEntries));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvFile = new System.Windows.Forms.DataGridView();
            this.btnCreate = new System.Windows.Forms.Button();
            this.dgvSponsors = new System.Windows.Forms.DataGridView();
            this.txtSponsorID = new GISControls.TextBoxChar();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.picQuotes = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtTime = new System.Windows.Forms.TextBox();
            this.lstAttachment = new System.Windows.Forms.ListBox();
            this.btnValSpLotNo = new System.Windows.Forms.Button();
            this.pnlReplace = new System.Windows.Forms.Panel();
            this.btnUpdateCancel = new System.Windows.Forms.Button();
            this.btnUpdateOK = new System.Windows.Forms.Button();
            this.pnlUpdate = new System.Windows.Forms.Panel();
            this.txtTempID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboSponsorID = new System.Windows.Forms.ComboBox();
            this.cboContacts = new System.Windows.Forms.ComboBox();
            this.cboSponsors = new System.Windows.Forms.ComboBox();
            this.txtGBLNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.btnReplaceCancel = new System.Windows.Forms.Button();
            this.btnReplaceOK = new System.Windows.Forms.Button();
            this.dgvReplace = new System.Windows.Forms.DataGridView();
            this.TempID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GBLNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SponsorID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SponsorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Contact = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ContactID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label23 = new System.Windows.Forms.Label();
            this.bsFile = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQuotes)).BeginInit();
            this.pnlReplace.SuspendLayout();
            this.pnlUpdate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReplace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFile
            // 
            this.dgvFile.BackgroundColor = System.Drawing.Color.White;
            this.dgvFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFile.Location = new System.Drawing.Point(29, 62);
            this.dgvFile.Name = "dgvFile";
            this.dgvFile.Size = new System.Drawing.Size(1280, 470);
            this.dgvFile.TabIndex = 0;
            this.dgvFile.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvFile_CellBeginEdit);
            this.dgvFile.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvFile_CurrentCellDirtyStateChanged);
            this.dgvFile.DoubleClick += new System.EventHandler(this.dgvFile_DoubleClick);
            // 
            // btnCreate
            // 
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.Location = new System.Drawing.Point(922, 549);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(117, 34);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "Create Invoice";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // dgvSponsors
            // 
            this.dgvSponsors.AllowUserToAddRows = false;
            this.dgvSponsors.AllowUserToDeleteRows = false;
            this.dgvSponsors.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSponsors.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSponsors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSponsors.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSponsors.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSponsors.Location = new System.Drawing.Point(165, 55);
            this.dgvSponsors.Name = "dgvSponsors";
            this.dgvSponsors.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSponsors.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSponsors.RowHeadersVisible = false;
            this.dgvSponsors.Size = new System.Drawing.Size(413, 477);
            this.dgvSponsors.TabIndex = 7;
            this.dgvSponsors.Visible = false;
            // 
            // txtSponsorID
            // 
            this.txtSponsorID.BackColor = System.Drawing.Color.White;
            this.txtSponsorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsorID.Location = new System.Drawing.Point(92, 35);
            this.txtSponsorID.Name = "txtSponsorID";
            this.txtSponsorID.Size = new System.Drawing.Size(74, 21);
            this.txtSponsorID.TabIndex = 5;
            // 
            // txtSponsor
            // 
            this.txtSponsor.BackColor = System.Drawing.Color.White;
            this.txtSponsor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsor.Location = new System.Drawing.Point(165, 35);
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(395, 21);
            this.txtSponsor.TabIndex = 6;
            // 
            // picQuotes
            // 
            this.picQuotes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picQuotes.BackgroundImage")));
            this.picQuotes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picQuotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picQuotes.Location = new System.Drawing.Point(559, 35);
            this.picQuotes.Name = "picQuotes";
            this.picQuotes.Size = new System.Drawing.Size(19, 21);
            this.picQuotes.TabIndex = 100;
            this.picQuotes.TabStop = false;
            this.picQuotes.Visible = false;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(26, 34);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 21);
            this.label6.TabIndex = 291;
            this.label6.Text = "Sponsor";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(587, 34);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(80, 23);
            this.btnFilter.TabIndex = 292;
            this.btnFilter.Text = "&Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(673, 34);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.TabIndex = 293;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPreview.Location = new System.Drawing.Point(1194, 549);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(117, 34);
            this.btnPreview.TabIndex = 294;
            this.btnPreview.Text = "Preview Invoice";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(29, 549);
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(157, 20);
            this.txtTime.TabIndex = 295;
            this.txtTime.Visible = false;
            // 
            // lstAttachment
            // 
            this.lstAttachment.FormattingEnabled = true;
            this.lstAttachment.Location = new System.Drawing.Point(1141, 14);
            this.lstAttachment.Name = "lstAttachment";
            this.lstAttachment.Size = new System.Drawing.Size(47, 43);
            this.lstAttachment.TabIndex = 296;
            this.lstAttachment.Visible = false;
            // 
            // btnValSpLotNo
            // 
            this.btnValSpLotNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValSpLotNo.Location = new System.Drawing.Point(1045, 549);
            this.btnValSpLotNo.Name = "btnValSpLotNo";
            this.btnValSpLotNo.Size = new System.Drawing.Size(143, 34);
            this.btnValSpLotNo.TabIndex = 297;
            this.btnValSpLotNo.Text = "Validate Sponsor/Lot #";
            this.btnValSpLotNo.UseVisualStyleBackColor = true;
            this.btnValSpLotNo.Click += new System.EventHandler(this.btnValSpLotNo_Click);
            // 
            // pnlReplace
            // 
            this.pnlReplace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlReplace.Controls.Add(this.btnUpdateCancel);
            this.pnlReplace.Controls.Add(this.btnUpdateOK);
            this.pnlReplace.Controls.Add(this.pnlUpdate);
            this.pnlReplace.Controls.Add(this.dgvReplace);
            this.pnlReplace.Controls.Add(this.label23);
            this.pnlReplace.Location = new System.Drawing.Point(165, 118);
            this.pnlReplace.Name = "pnlReplace";
            this.pnlReplace.Size = new System.Drawing.Size(984, 324);
            this.pnlReplace.TabIndex = 298;
            this.pnlReplace.Visible = false;
            // 
            // btnUpdateCancel
            // 
            this.btnUpdateCancel.Location = new System.Drawing.Point(871, 278);
            this.btnUpdateCancel.Name = "btnUpdateCancel";
            this.btnUpdateCancel.Size = new System.Drawing.Size(66, 23);
            this.btnUpdateCancel.TabIndex = 21;
            this.btnUpdateCancel.Text = "Ca&ncel";
            this.btnUpdateCancel.UseVisualStyleBackColor = true;
            this.btnUpdateCancel.Click += new System.EventHandler(this.btnUpdateCancel_Click);
            // 
            // btnUpdateOK
            // 
            this.btnUpdateOK.Location = new System.Drawing.Point(799, 278);
            this.btnUpdateOK.Name = "btnUpdateOK";
            this.btnUpdateOK.Size = new System.Drawing.Size(66, 23);
            this.btnUpdateOK.TabIndex = 22;
            this.btnUpdateOK.Text = "O&K";
            this.btnUpdateOK.UseVisualStyleBackColor = true;
            this.btnUpdateOK.Click += new System.EventHandler(this.btnUpdateOK_Click);
            // 
            // pnlUpdate
            // 
            this.pnlUpdate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpdate.Controls.Add(this.txtTempID);
            this.pnlUpdate.Controls.Add(this.label4);
            this.pnlUpdate.Controls.Add(this.label3);
            this.pnlUpdate.Controls.Add(this.cboSponsorID);
            this.pnlUpdate.Controls.Add(this.cboContacts);
            this.pnlUpdate.Controls.Add(this.cboSponsors);
            this.pnlUpdate.Controls.Add(this.txtGBLNo);
            this.pnlUpdate.Controls.Add(this.label2);
            this.pnlUpdate.Controls.Add(this.label1);
            this.pnlUpdate.Controls.Add(this.label24);
            this.pnlUpdate.Controls.Add(this.btnReplaceCancel);
            this.pnlUpdate.Controls.Add(this.btnReplaceOK);
            this.pnlUpdate.Location = new System.Drawing.Point(306, 69);
            this.pnlUpdate.Name = "pnlUpdate";
            this.pnlUpdate.Size = new System.Drawing.Size(406, 203);
            this.pnlUpdate.TabIndex = 21;
            this.pnlUpdate.Visible = false;
            // 
            // txtTempID
            // 
            this.txtTempID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTempID.Location = new System.Drawing.Point(88, 46);
            this.txtTempID.Name = "txtTempID";
            this.txtTempID.ReadOnly = true;
            this.txtTempID.Size = new System.Drawing.Size(71, 20);
            this.txtTempID.TabIndex = 301;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(22, 46);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 20);
            this.label4.TabIndex = 300;
            this.label4.Text = "Temp. ID";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Firebrick;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(-1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(406, 21);
            this.label3.TabIndex = 299;
            this.label3.Text = "UPDATE SPONSOR/CONTACT";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboSponsorID
            // 
            this.cboSponsorID.FormattingEnabled = true;
            this.cboSponsorID.Location = new System.Drawing.Point(88, 97);
            this.cboSponsorID.Name = "cboSponsorID";
            this.cboSponsorID.Size = new System.Drawing.Size(50, 21);
            this.cboSponsorID.TabIndex = 298;
            this.cboSponsorID.SelectedIndexChanged += new System.EventHandler(this.cboSponsorID_SelectedIndexChanged);
            // 
            // cboContacts
            // 
            this.cboContacts.FormattingEnabled = true;
            this.cboContacts.Location = new System.Drawing.Point(88, 123);
            this.cboContacts.Name = "cboContacts";
            this.cboContacts.Size = new System.Drawing.Size(292, 21);
            this.cboContacts.TabIndex = 297;
            // 
            // cboSponsors
            // 
            this.cboSponsors.FormattingEnabled = true;
            this.cboSponsors.Location = new System.Drawing.Point(144, 97);
            this.cboSponsors.Name = "cboSponsors";
            this.cboSponsors.Size = new System.Drawing.Size(237, 21);
            this.cboSponsors.TabIndex = 296;
            this.cboSponsors.SelectedIndexChanged += new System.EventHandler(this.cboSponsors_SelectedIndexChanged);
            // 
            // txtGBLNo
            // 
            this.txtGBLNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGBLNo.Location = new System.Drawing.Point(88, 72);
            this.txtGBLNo.Name = "txtGBLNo";
            this.txtGBLNo.ReadOnly = true;
            this.txtGBLNo.Size = new System.Drawing.Size(71, 20);
            this.txtGBLNo.TabIndex = 295;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(22, 123);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 21);
            this.label2.TabIndex = 294;
            this.label2.Text = "Contact";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(22, 97);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 21);
            this.label1.TabIndex = 293;
            this.label1.Text = "Sponsor";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label24
            // 
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(22, 72);
            this.label24.Margin = new System.Windows.Forms.Padding(0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(63, 20);
            this.label24.TabIndex = 292;
            this.label24.Text = "GBL No.";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnReplaceCancel
            // 
            this.btnReplaceCancel.Location = new System.Drawing.Point(321, 159);
            this.btnReplaceCancel.Name = "btnReplaceCancel";
            this.btnReplaceCancel.Size = new System.Drawing.Size(60, 23);
            this.btnReplaceCancel.TabIndex = 20;
            this.btnReplaceCancel.Text = "Ca&ncel";
            this.btnReplaceCancel.UseVisualStyleBackColor = true;
            this.btnReplaceCancel.Click += new System.EventHandler(this.btnReplaceCancel_Click);
            // 
            // btnReplaceOK
            // 
            this.btnReplaceOK.Location = new System.Drawing.Point(255, 159);
            this.btnReplaceOK.Name = "btnReplaceOK";
            this.btnReplaceOK.Size = new System.Drawing.Size(60, 23);
            this.btnReplaceOK.TabIndex = 19;
            this.btnReplaceOK.Text = "O&K";
            this.btnReplaceOK.UseVisualStyleBackColor = true;
            this.btnReplaceOK.Click += new System.EventHandler(this.btnReplaceOK_Click);
            // 
            // dgvReplace
            // 
            this.dgvReplace.AllowUserToAddRows = false;
            this.dgvReplace.AllowUserToDeleteRows = false;
            this.dgvReplace.BackgroundColor = System.Drawing.Color.White;
            this.dgvReplace.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReplace.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TempID,
            this.GBLNo,
            this.SponsorID,
            this.SponsorName,
            this.Contact,
            this.ContactID});
            this.dgvReplace.Location = new System.Drawing.Point(28, 49);
            this.dgvReplace.Name = "dgvReplace";
            this.dgvReplace.Size = new System.Drawing.Size(909, 223);
            this.dgvReplace.TabIndex = 3;
            this.dgvReplace.DoubleClick += new System.EventHandler(this.dgvReplace_DoubleClick);
            // 
            // TempID
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TempID.DefaultCellStyle = dataGridViewCellStyle4;
            this.TempID.HeaderText = "Temp. ID";
            this.TempID.Name = "TempID";
            this.TempID.ReadOnly = true;
            // 
            // GBLNo
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.GBLNo.DefaultCellStyle = dataGridViewCellStyle5;
            this.GBLNo.HeaderText = "GBL No.";
            this.GBLNo.Name = "GBLNo";
            this.GBLNo.ReadOnly = true;
            // 
            // SponsorID
            // 
            this.SponsorID.HeaderText = "Sponsor ID";
            this.SponsorID.Name = "SponsorID";
            this.SponsorID.ReadOnly = true;
            this.SponsorID.Visible = false;
            // 
            // SponsorName
            // 
            this.SponsorName.HeaderText = "Sponsor Name";
            this.SponsorName.Name = "SponsorName";
            this.SponsorName.ReadOnly = true;
            this.SponsorName.Width = 350;
            // 
            // Contact
            // 
            this.Contact.HeaderText = "Contact";
            this.Contact.Name = "Contact";
            this.Contact.ReadOnly = true;
            this.Contact.Width = 300;
            // 
            // ContactID
            // 
            this.ContactID.HeaderText = "Contact ID";
            this.ContactID.Name = "ContactID";
            this.ContactID.ReadOnly = true;
            this.ContactID.Visible = false;
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.Firebrick;
            this.label23.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(-3, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(986, 21);
            this.label23.TabIndex = 2;
            this.label23.Text = "REPLACE SPONSOR / CONTACT";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IngredionInvEntries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1343, 612);
            this.Controls.Add(this.lstAttachment);
            this.Controls.Add(this.pnlReplace);
            this.Controls.Add(this.btnValSpLotNo);
            this.Controls.Add(this.txtTime);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.picQuotes);
            this.Controls.Add(this.dgvSponsors);
            this.Controls.Add(this.txtSponsorID);
            this.Controls.Add(this.txtSponsor);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.dgvFile);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1359, 650);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1359, 650);
            this.Name = "IngredionInvEntries";
            this.Text = "IngredionInvTemp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IngredionInvEntries_FormClosing);
            this.Load += new System.EventHandler(this.IngredionInvTemp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picQuotes)).EndInit();
            this.pnlReplace.ResumeLayout(false);
            this.pnlUpdate.ResumeLayout(false);
            this.pnlUpdate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReplace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFile;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.BindingSource bsFile;
        private System.Windows.Forms.DataGridView dgvSponsors;
        private GISControls.TextBoxChar txtSponsorID;
        private GISControls.TextBoxChar txtSponsor;
        private System.Windows.Forms.PictureBox picQuotes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.ListBox lstAttachment;
        private System.Windows.Forms.Button btnValSpLotNo;
        private System.Windows.Forms.Panel pnlReplace;
        private System.Windows.Forms.DataGridView dgvReplace;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnReplaceCancel;
        private System.Windows.Forms.Button btnReplaceOK;
        private System.Windows.Forms.Panel pnlUpdate;
        private System.Windows.Forms.Button btnUpdateCancel;
        private System.Windows.Forms.Button btnUpdateOK;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.ComboBox cboContacts;
        private System.Windows.Forms.ComboBox cboSponsors;
        private System.Windows.Forms.TextBox txtGBLNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboSponsorID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn TempID;
        private System.Windows.Forms.DataGridViewTextBoxColumn GBLNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SponsorID;
        private System.Windows.Forms.DataGridViewTextBoxColumn SponsorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contact;
        private System.Windows.Forms.DataGridViewTextBoxColumn ContactID;
        private System.Windows.Forms.TextBox txtTempID;
        private System.Windows.Forms.Label label4;
    }
}