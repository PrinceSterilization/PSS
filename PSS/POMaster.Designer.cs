namespace PSS
{
    partial class POMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POMaster));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPONotes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkPOStatus = new System.Windows.Forms.CheckBox();
            this.lblPOStatus = new System.Windows.Forms.Label();
            this.dtpPODate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAmount = new GISControls.TextBoxChar();
            this.label1 = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.picSponsors = new System.Windows.Forms.PictureBox();
            this.txtSponsorID = new GISControls.TextBoxChar();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtName = new GISControls.TextBoxChar();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPONo = new GISControls.TextBoxChar();
            this.label7 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.dgvSponsors = new System.Windows.Forms.DataGridView();
            this.bsSponsors = new System.Windows.Forms.BindingSource(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.cmbStatusPO = new System.Windows.Forms.ComboBox();
            this.txtCrntYrAmnt = new GISControls.TextBoxChar();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNxtYrsAmnt = new GISControls.TextBoxChar();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSponsors)).BeginInit();
            this.SuspendLayout();
            // 
            // chkShowInactive
            // 
            this.chkShowInactive.Location = new System.Drawing.Point(768, 53);
            this.chkShowInactive.Size = new System.Drawing.Size(141, 19);
            this.chkShowInactive.Text = "Show Completed PO";
            this.chkShowInactive.CheckedChanged += new System.EventHandler(this.ChkShowInactive_CheckedChanged);
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.txtNxtYrsAmnt);
            this.pnlRecord.Controls.Add(this.label9);
            this.pnlRecord.Controls.Add(this.txtCrntYrAmnt);
            this.pnlRecord.Controls.Add(this.label8);
            this.pnlRecord.Controls.Add(this.label4);
            this.pnlRecord.Controls.Add(this.txtPONotes);
            this.pnlRecord.Controls.Add(this.label3);
            this.pnlRecord.Controls.Add(this.lblStatus);
            this.pnlRecord.Controls.Add(this.chkPOStatus);
            this.pnlRecord.Controls.Add(this.lblPOStatus);
            this.pnlRecord.Controls.Add(this.dtpPODate);
            this.pnlRecord.Controls.Add(this.label2);
            this.pnlRecord.Controls.Add(this.txtAmount);
            this.pnlRecord.Controls.Add(this.label1);
            this.pnlRecord.Controls.Add(this.btnView);
            this.pnlRecord.Controls.Add(this.picSponsors);
            this.pnlRecord.Controls.Add(this.txtSponsorID);
            this.pnlRecord.Controls.Add(this.txtSponsor);
            this.pnlRecord.Controls.Add(this.btnBrowse);
            this.pnlRecord.Controls.Add(this.label5);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtName);
            this.pnlRecord.Controls.Add(this.label6);
            this.pnlRecord.Controls.Add(this.txtPONo);
            this.pnlRecord.Controls.Add(this.label7);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.dgvSponsors);
            this.pnlRecord.Location = new System.Drawing.Point(538, 157);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(620, 384);
            this.pnlRecord.TabIndex = 105;
            this.pnlRecord.Visible = false;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Maroon;
            this.label4.Location = new System.Drawing.Point(141, 292);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(2);
            this.label4.Size = new System.Drawing.Size(456, 43);
            this.label4.TabIndex = 128;
            this.label4.Text = "* Use this field for any comment or notes (i.e. Stability mature date, or target " +
    "invoicing date)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPONotes
            // 
            this.txtPONotes.Location = new System.Drawing.Point(141, 231);
            this.txtPONotes.MaxLength = 499;
            this.txtPONotes.Multiline = true;
            this.txtPONotes.Name = "txtPONotes";
            this.txtPONotes.Size = new System.Drawing.Size(456, 58);
            this.txtPONotes.TabIndex = 127;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(14, 254);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(78, 23);
            this.label3.TabIndex = 126;
            this.label3.Text = "PO Notes";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblStatus.Location = new System.Drawing.Point(141, 344);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 16);
            this.lblStatus.TabIndex = 125;
            this.lblStatus.Text = "Status";
            // 
            // chkPOStatus
            // 
            this.chkPOStatus.AutoSize = true;
            this.chkPOStatus.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkPOStatus.FlatAppearance.BorderSize = 2;
            this.chkPOStatus.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.chkPOStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkPOStatus.Font = new System.Drawing.Font("Arial Narrow", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPOStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkPOStatus.Location = new System.Drawing.Point(229, 340);
            this.chkPOStatus.Name = "chkPOStatus";
            this.chkPOStatus.Size = new System.Drawing.Size(88, 24);
            this.chkPOStatus.TabIndex = 124;
            this.chkPOStatus.Text = "Cancel PO";
            this.chkPOStatus.CheckedChanged += new System.EventHandler(this.ChkPOStatus_CheckedChanged);
            // 
            // lblPOStatus
            // 
            this.lblPOStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblPOStatus.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPOStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.lblPOStatus.Location = new System.Drawing.Point(14, 341);
            this.lblPOStatus.Name = "lblPOStatus";
            this.lblPOStatus.Padding = new System.Windows.Forms.Padding(2);
            this.lblPOStatus.Size = new System.Drawing.Size(77, 23);
            this.lblPOStatus.TabIndex = 123;
            this.lblPOStatus.Text = "PO Status";
            this.lblPOStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpPODate
            // 
            this.dtpPODate.CustomFormat = "MM/dd/yyyy";
            this.dtpPODate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPODate.Location = new System.Drawing.Point(141, 93);
            this.dtpPODate.Name = "dtpPODate";
            this.dtpPODate.ShowCheckBox = true;
            this.dtpPODate.Size = new System.Drawing.Size(107, 21);
            this.dtpPODate.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(14, 92);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(2);
            this.label2.Size = new System.Drawing.Size(123, 23);
            this.label2.TabIndex = 122;
            this.label2.Text = "PO Date";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAmount
            // 
            this.txtAmount.BackColor = System.Drawing.Color.White;
            this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAmount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.Location = new System.Drawing.Point(141, 156);
            this.txtAmount.MaxLength = 0;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(107, 21);
            this.txtAmount.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(14, 155);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(123, 23);
            this.label1.TabIndex = 121;
            this.label1.Text = "Total Amount ($)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(529, 67);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(68, 24);
            this.btnView.TabIndex = 4;
            this.btnView.Text = "&View";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // picSponsors
            // 
            this.picSponsors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSponsors.BackgroundImage")));
            this.picSponsors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSponsors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSponsors.Location = new System.Drawing.Point(578, 45);
            this.picSponsors.Name = "picSponsors";
            this.picSponsors.Size = new System.Drawing.Size(19, 21);
            this.picSponsors.TabIndex = 120;
            this.picSponsors.TabStop = false;
            this.picSponsors.Click += new System.EventHandler(this.picSponsors_Click);
            // 
            // txtSponsorID
            // 
            this.txtSponsorID.BackColor = System.Drawing.Color.White;
            this.txtSponsorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsorID.Location = new System.Drawing.Point(141, 45);
            this.txtSponsorID.MaxLength = 5;
            this.txtSponsorID.Name = "txtSponsorID";
            this.txtSponsorID.Size = new System.Drawing.Size(57, 21);
            this.txtSponsorID.TabIndex = 0;
            this.txtSponsorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsorID_KeyPress);
            // 
            // txtSponsor
            // 
            this.txtSponsor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsor.Location = new System.Drawing.Point(197, 45);
            this.txtSponsor.MaxLength = 50;
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(382, 21);
            this.txtSponsor.TabIndex = 1;
            this.txtSponsor.TextChanged += new System.EventHandler(this.txtSponsor_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(529, 122);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(68, 24);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(14, 44);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(2);
            this.label5.Size = new System.Drawing.Size(123, 23);
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
            this.btnClose.Location = new System.Drawing.Point(542, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(141, 117);
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
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label6.Location = new System.Drawing.Point(14, 124);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(2);
            this.label6.Size = new System.Drawing.Size(123, 21);
            this.label6.TabIndex = 5;
            this.label6.Text = "File Location/Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPONo
            // 
            this.txtPONo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPONo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPONo.Location = new System.Drawing.Point(141, 69);
            this.txtPONo.MaxLength = 50;
            this.txtPONo.Name = "txtPONo";
            this.txtPONo.Size = new System.Drawing.Size(382, 21);
            this.txtPONo.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(14, 69);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(2);
            this.label7.Size = new System.Drawing.Size(123, 21);
            this.label7.TabIndex = 3;
            this.label7.Text = "PO No.";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(622, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "PURCHASE ORDER";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseDown);
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
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
            this.dgvSponsors.Location = new System.Drawing.Point(197, 65);
            this.dgvSponsors.Name = "dgvSponsors";
            this.dgvSponsors.ReadOnly = true;
            this.dgvSponsors.RowHeadersVisible = false;
            this.dgvSponsors.Size = new System.Drawing.Size(400, 120);
            this.dgvSponsors.TabIndex = 2;
            this.dgvSponsors.Visible = false;
            this.dgvSponsors.DoubleClick += new System.EventHandler(this.dgvSponsors_DoubleClick);
            this.dgvSponsors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSponsors_KeyDown);
            this.dgvSponsors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSponsors_KeyPress);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // cmbStatusPO
            // 
            this.cmbStatusPO.FormattingEnabled = true;
            this.cmbStatusPO.Items.AddRange(new object[] {
            "Show All"});
            this.cmbStatusPO.Location = new System.Drawing.Point(916, 50);
            this.cmbStatusPO.Name = "cmbStatusPO";
            this.cmbStatusPO.Size = new System.Drawing.Size(121, 23);
            this.cmbStatusPO.TabIndex = 107;
            this.cmbStatusPO.Text = "Select PO Status";
            this.cmbStatusPO.SelectedIndexChanged += new System.EventHandler(this.CmbStatusPO_SelectedIndexChanged);
            // 
            // txtCrntYrAmnt
            // 
            this.txtCrntYrAmnt.BackColor = System.Drawing.Color.White;
            this.txtCrntYrAmnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCrntYrAmnt.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCrntYrAmnt.Location = new System.Drawing.Point(141, 179);
            this.txtCrntYrAmnt.MaxLength = 0;
            this.txtCrntYrAmnt.Name = "txtCrntYrAmnt";
            this.txtCrntYrAmnt.Size = new System.Drawing.Size(107, 21);
            this.txtCrntYrAmnt.TabIndex = 129;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(14, 178);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(2);
            this.label8.Size = new System.Drawing.Size(123, 23);
            this.label8.TabIndex = 130;
            this.label8.Text = "Current Year PO Amt";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNxtYrsAmnt
            // 
            this.txtNxtYrsAmnt.BackColor = System.Drawing.Color.White;
            this.txtNxtYrsAmnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNxtYrsAmnt.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNxtYrsAmnt.Location = new System.Drawing.Point(141, 202);
            this.txtNxtYrsAmnt.MaxLength = 0;
            this.txtNxtYrsAmnt.Name = "txtNxtYrsAmnt";
            this.txtNxtYrsAmnt.Size = new System.Drawing.Size(107, 21);
            this.txtNxtYrsAmnt.TabIndex = 131;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(14, 201);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(2);
            this.label9.Size = new System.Drawing.Size(123, 23);
            this.label9.TabIndex = 132;
            this.label9.Text = "Next Years PO Amt";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // POMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1386, 704);
            this.Controls.Add(this.pnlRecord);
            this.Controls.Add(this.cmbStatusPO);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "POMaster";
            this.Load += new System.EventHandler(this.POMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.POMaster_KeyDown);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.cmbStatusPO, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSponsors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClose;
        private GISControls.TextBoxChar txtName;
        private System.Windows.Forms.Label label6;
        private GISControls.TextBoxChar txtPONo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblHeader;
        private GISControls.TextBoxChar txtSponsor;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.DataGridView dgvSponsors;
        private System.Windows.Forms.PictureBox picSponsors;
        private System.Windows.Forms.BindingSource bsSponsors;
        private GISControls.TextBoxChar txtSponsorID;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnView;
        private GISControls.TextBoxChar txtAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpPODate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPOStatus;
        private System.Windows.Forms.CheckBox chkPOStatus;
        private System.Windows.Forms.ComboBox cmbStatusPO;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPONotes;
        private System.Windows.Forms.Label label3;
        private GISControls.TextBoxChar txtNxtYrsAmnt;
        private System.Windows.Forms.Label label9;
        private GISControls.TextBoxChar txtCrntYrAmnt;
        private System.Windows.Forms.Label label8;
    }
}
