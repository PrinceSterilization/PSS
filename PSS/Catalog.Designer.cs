namespace PSS
{
    partial class Catalog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Catalog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.dgvVendorNames = new System.Windows.Forms.DataGridView();
            this.dgvCatNames = new System.Windows.Forms.DataGridView();
            this.txtWebsite = new System.Windows.Forms.TextBox();
            this.btnCheckBrowser = new System.Windows.Forms.Button();
            this.txtGradeID = new GISControls.TextBoxChar();
            this.picGrades = new System.Windows.Forms.PictureBox();
            this.txtCatNameID = new GISControls.TextBoxChar();
            this.btnAddNames = new System.Windows.Forms.Button();
            this.picCatNames = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.picVendors = new System.Windows.Forms.PictureBox();
            this.lblVendor = new System.Windows.Forms.Label();
            this.txtVendorID = new GISControls.TextBoxChar();
            this.txtVendorName = new GISControls.TextBoxChar();
            this.txtCatDesc = new System.Windows.Forms.TextBox();
            this.lblCatDesc = new System.Windows.Forms.Label();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.txtCatalogNo = new System.Windows.Forms.TextBox();
            this.txtCatalogName = new System.Windows.Forms.TextBox();
            this.lblCatalogName = new System.Windows.Forms.Label();
            this.lblGrade = new System.Windows.Forms.Label();
            this.lblCatalogNo = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.dgvCatGrades = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGrades)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCatNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVendors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatGrades)).BeginInit();
            this.SuspendLayout();
            // 
            // cklColumns
            // 
            this.cklColumns.Size = new System.Drawing.Size(122, 196);
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.dgvVendorNames);
            this.pnlRecord.Controls.Add(this.dgvCatNames);
            this.pnlRecord.Controls.Add(this.txtWebsite);
            this.pnlRecord.Controls.Add(this.btnCheckBrowser);
            this.pnlRecord.Controls.Add(this.txtGradeID);
            this.pnlRecord.Controls.Add(this.picGrades);
            this.pnlRecord.Controls.Add(this.txtCatNameID);
            this.pnlRecord.Controls.Add(this.btnAddNames);
            this.pnlRecord.Controls.Add(this.picCatNames);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.picVendors);
            this.pnlRecord.Controls.Add(this.lblVendor);
            this.pnlRecord.Controls.Add(this.txtVendorID);
            this.pnlRecord.Controls.Add(this.txtVendorName);
            this.pnlRecord.Controls.Add(this.txtCatDesc);
            this.pnlRecord.Controls.Add(this.lblCatDesc);
            this.pnlRecord.Controls.Add(this.chkIsActive);
            this.pnlRecord.Controls.Add(this.txtUnitPrice);
            this.pnlRecord.Controls.Add(this.lblUnitPrice);
            this.pnlRecord.Controls.Add(this.txtGrade);
            this.pnlRecord.Controls.Add(this.txtCatalogNo);
            this.pnlRecord.Controls.Add(this.txtCatalogName);
            this.pnlRecord.Controls.Add(this.lblCatalogName);
            this.pnlRecord.Controls.Add(this.lblGrade);
            this.pnlRecord.Controls.Add(this.lblCatalogNo);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.dgvCatGrades);
            this.pnlRecord.Location = new System.Drawing.Point(21, 91);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(561, 398);
            this.pnlRecord.TabIndex = 109;
            // 
            // dgvVendorNames
            // 
            this.dgvVendorNames.AllowUserToAddRows = false;
            this.dgvVendorNames.AllowUserToDeleteRows = false;
            this.dgvVendorNames.BackgroundColor = System.Drawing.Color.White;
            this.dgvVendorNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVendorNames.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvVendorNames.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvVendorNames.Location = new System.Drawing.Point(179, 106);
            this.dgvVendorNames.Name = "dgvVendorNames";
            this.dgvVendorNames.ReadOnly = true;
            this.dgvVendorNames.RowHeadersVisible = false;
            this.dgvVendorNames.Size = new System.Drawing.Size(315, 156);
            this.dgvVendorNames.TabIndex = 2;
            this.dgvVendorNames.Visible = false;
            this.dgvVendorNames.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVendorNames_CellContentClick);
            this.dgvVendorNames.DoubleClick += new System.EventHandler(this.dgvVendorNames_DoubleClick);
            this.dgvVendorNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVendorNames_KeyDown);
            this.dgvVendorNames.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvVendorNames_KeyPress);
            this.dgvVendorNames.Leave += new System.EventHandler(this.dgvVendorNames_Leave);
            // 
            // dgvCatNames
            // 
            this.dgvCatNames.AllowUserToAddRows = false;
            this.dgvCatNames.AllowUserToDeleteRows = false;
            this.dgvCatNames.BackgroundColor = System.Drawing.Color.White;
            this.dgvCatNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCatNames.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCatNames.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCatNames.Location = new System.Drawing.Point(179, 136);
            this.dgvCatNames.Name = "dgvCatNames";
            this.dgvCatNames.ReadOnly = true;
            this.dgvCatNames.RowHeadersVisible = false;
            this.dgvCatNames.Size = new System.Drawing.Size(315, 154);
            this.dgvCatNames.TabIndex = 5;
            this.dgvCatNames.Visible = false;
            this.dgvCatNames.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCatNames_CellContentClick);
            this.dgvCatNames.DoubleClick += new System.EventHandler(this.dgvCatNames_DoubleClick);
            this.dgvCatNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCatNames_KeyDown);
            this.dgvCatNames.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvCatNames_KeyPress);
            this.dgvCatNames.Leave += new System.EventHandler(this.dgvCatNames_Leave);
            // 
            // txtWebsite
            // 
            this.txtWebsite.Location = new System.Drawing.Point(49, 61);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.Size = new System.Drawing.Size(221, 21);
            this.txtWebsite.TabIndex = 429;
            this.txtWebsite.Visible = false;
            // 
            // btnCheckBrowser
            // 
            this.btnCheckBrowser.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckBrowser.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckBrowser.Image")));
            this.btnCheckBrowser.Location = new System.Drawing.Point(497, 84);
            this.btnCheckBrowser.Name = "btnCheckBrowser";
            this.btnCheckBrowser.Size = new System.Drawing.Size(32, 25);
            this.btnCheckBrowser.TabIndex = 428;
            this.btnCheckBrowser.UseVisualStyleBackColor = true;
            this.btnCheckBrowser.Click += new System.EventHandler(this.btnCheckBrowser_Click);
            // 
            // txtGradeID
            // 
            this.txtGradeID.BackColor = System.Drawing.Color.White;
            this.txtGradeID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGradeID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGradeID.Location = new System.Drawing.Point(108, 242);
            this.txtGradeID.MaxLength = 5;
            this.txtGradeID.Name = "txtGradeID";
            this.txtGradeID.Size = new System.Drawing.Size(68, 21);
            this.txtGradeID.TabIndex = 8;
            this.txtGradeID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGradeID_KeyPress);
            // 
            // picGrades
            // 
            this.picGrades.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picGrades.BackgroundImage")));
            this.picGrades.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picGrades.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGrades.Location = new System.Drawing.Point(314, 242);
            this.picGrades.Name = "picGrades";
            this.picGrades.Size = new System.Drawing.Size(19, 21);
            this.picGrades.TabIndex = 399;
            this.picGrades.TabStop = false;
            this.picGrades.Click += new System.EventHandler(this.picGrades_Click);
            // 
            // txtCatNameID
            // 
            this.txtCatNameID.BackColor = System.Drawing.Color.White;
            this.txtCatNameID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCatNameID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCatNameID.Location = new System.Drawing.Point(107, 115);
            this.txtCatNameID.MaxLength = 5;
            this.txtCatNameID.Name = "txtCatNameID";
            this.txtCatNameID.Size = new System.Drawing.Size(68, 21);
            this.txtCatNameID.TabIndex = 3;
            this.txtCatNameID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCatNameID_KeyPress);
            // 
            // btnAddNames
            // 
            this.btnAddNames.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNames.Image")));
            this.btnAddNames.Location = new System.Drawing.Point(497, 114);
            this.btnAddNames.Name = "btnAddNames";
            this.btnAddNames.Size = new System.Drawing.Size(32, 24);
            this.btnAddNames.TabIndex = 397;
            this.btnAddNames.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnAddNames.UseVisualStyleBackColor = true;
            this.btnAddNames.Click += new System.EventHandler(this.btnAddNames_Click);
            // 
            // picCatNames
            // 
            this.picCatNames.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picCatNames.BackgroundImage")));
            this.picCatNames.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picCatNames.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCatNames.Location = new System.Drawing.Point(475, 115);
            this.picCatNames.Name = "picCatNames";
            this.picCatNames.Size = new System.Drawing.Size(19, 21);
            this.picCatNames.TabIndex = 396;
            this.picCatNames.TabStop = false;
            this.picCatNames.Click += new System.EventHandler(this.picCatNames_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(489, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 22);
            this.btnClose.TabIndex = 394;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picVendors
            // 
            this.picVendors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picVendors.BackgroundImage")));
            this.picVendors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picVendors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picVendors.Location = new System.Drawing.Point(475, 86);
            this.picVendors.Name = "picVendors";
            this.picVendors.Size = new System.Drawing.Size(19, 21);
            this.picVendors.TabIndex = 190;
            this.picVendors.TabStop = false;
            this.picVendors.Click += new System.EventHandler(this.picVendors_Click);
            // 
            // lblVendor
            // 
            this.lblVendor.BackColor = System.Drawing.Color.Transparent;
            this.lblVendor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendor.ForeColor = System.Drawing.Color.Black;
            this.lblVendor.Location = new System.Drawing.Point(46, 85);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Padding = new System.Windows.Forms.Padding(2);
            this.lblVendor.Size = new System.Drawing.Size(55, 21);
            this.lblVendor.TabIndex = 189;
            this.lblVendor.Text = "Vendor:";
            this.lblVendor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtVendorID
            // 
            this.txtVendorID.BackColor = System.Drawing.Color.White;
            this.txtVendorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVendorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendorID.Location = new System.Drawing.Point(107, 86);
            this.txtVendorID.MaxLength = 5;
            this.txtVendorID.Name = "txtVendorID";
            this.txtVendorID.Size = new System.Drawing.Size(68, 21);
            this.txtVendorID.TabIndex = 0;
            this.txtVendorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtVendorID_KeyPress);
            // 
            // txtVendorName
            // 
            this.txtVendorName.BackColor = System.Drawing.Color.White;
            this.txtVendorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVendorName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendorName.Location = new System.Drawing.Point(179, 86);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.Size = new System.Drawing.Size(314, 21);
            this.txtVendorName.TabIndex = 1;
            this.txtVendorName.TextChanged += new System.EventHandler(this.txtVendorName_TextChanged);
            this.txtVendorName.Enter += new System.EventHandler(this.txtVendorName_Enter);
            // 
            // txtCatDesc
            // 
            this.txtCatDesc.Location = new System.Drawing.Point(107, 174);
            this.txtCatDesc.MaxLength = 150;
            this.txtCatDesc.Multiline = true;
            this.txtCatDesc.Name = "txtCatDesc";
            this.txtCatDesc.Size = new System.Drawing.Size(291, 59);
            this.txtCatDesc.TabIndex = 7;
            // 
            // lblCatDesc
            // 
            this.lblCatDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblCatDesc.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCatDesc.ForeColor = System.Drawing.Color.Black;
            this.lblCatDesc.Location = new System.Drawing.Point(17, 172);
            this.lblCatDesc.Name = "lblCatDesc";
            this.lblCatDesc.Padding = new System.Windows.Forms.Padding(2);
            this.lblCatDesc.Size = new System.Drawing.Size(84, 21);
            this.lblCatDesc.TabIndex = 186;
            this.lblCatDesc.Text = "Description:";
            this.lblCatDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsActive.ForeColor = System.Drawing.Color.Red;
            this.chkIsActive.Location = new System.Drawing.Point(470, 58);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(61, 19);
            this.chkIsActive.TabIndex = 12;
            this.chkIsActive.Text = "Active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.Location = new System.Drawing.Point(107, 271);
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
            this.txtUnitPrice.TabIndex = 11;
            this.txtUnitPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUnitPrice_KeyPress);
            // 
            // lblUnitPrice
            // 
            this.lblUnitPrice.BackColor = System.Drawing.Color.Transparent;
            this.lblUnitPrice.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnitPrice.ForeColor = System.Drawing.Color.Black;
            this.lblUnitPrice.Location = new System.Drawing.Point(33, 271);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Padding = new System.Windows.Forms.Padding(2);
            this.lblUnitPrice.Size = new System.Drawing.Size(68, 21);
            this.lblUnitPrice.TabIndex = 172;
            this.lblUnitPrice.Text = "Unit Price:";
            this.lblUnitPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(179, 242);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(154, 21);
            this.txtGrade.TabIndex = 9;
            this.txtGrade.TextChanged += new System.EventHandler(this.txtGrade_TextChanged);
            this.txtGrade.Enter += new System.EventHandler(this.txtGrade_Enter);
            // 
            // txtCatalogNo
            // 
            this.txtCatalogNo.Location = new System.Drawing.Point(107, 145);
            this.txtCatalogNo.Name = "txtCatalogNo";
            this.txtCatalogNo.Size = new System.Drawing.Size(291, 21);
            this.txtCatalogNo.TabIndex = 6;
            // 
            // txtCatalogName
            // 
            this.txtCatalogName.Location = new System.Drawing.Point(179, 115);
            this.txtCatalogName.Name = "txtCatalogName";
            this.txtCatalogName.Size = new System.Drawing.Size(315, 21);
            this.txtCatalogName.TabIndex = 4;
            this.txtCatalogName.TextChanged += new System.EventHandler(this.txtCatalogName_TextChanged);
            // 
            // lblCatalogName
            // 
            this.lblCatalogName.BackColor = System.Drawing.Color.Transparent;
            this.lblCatalogName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCatalogName.ForeColor = System.Drawing.Color.Black;
            this.lblCatalogName.Location = new System.Drawing.Point(46, 115);
            this.lblCatalogName.Name = "lblCatalogName";
            this.lblCatalogName.Padding = new System.Windows.Forms.Padding(2);
            this.lblCatalogName.Size = new System.Drawing.Size(55, 21);
            this.lblCatalogName.TabIndex = 166;
            this.lblCatalogName.Text = "Name:";
            this.lblCatalogName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGrade
            // 
            this.lblGrade.BackColor = System.Drawing.Color.Transparent;
            this.lblGrade.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGrade.ForeColor = System.Drawing.Color.Black;
            this.lblGrade.Location = new System.Drawing.Point(46, 242);
            this.lblGrade.Name = "lblGrade";
            this.lblGrade.Padding = new System.Windows.Forms.Padding(2);
            this.lblGrade.Size = new System.Drawing.Size(55, 21);
            this.lblGrade.TabIndex = 165;
            this.lblGrade.Text = "Grade:";
            this.lblGrade.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCatalogNo
            // 
            this.lblCatalogNo.BackColor = System.Drawing.Color.Transparent;
            this.lblCatalogNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCatalogNo.ForeColor = System.Drawing.Color.Black;
            this.lblCatalogNo.Location = new System.Drawing.Point(14, 143);
            this.lblCatalogNo.Name = "lblCatalogNo";
            this.lblCatalogNo.Padding = new System.Windows.Forms.Padding(2);
            this.lblCatalogNo.Size = new System.Drawing.Size(87, 21);
            this.lblCatalogNo.TabIndex = 162;
            this.lblCatalogNo.Text = "Catalog No:";
            this.lblCatalogNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(561, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "CATALOG MASTER";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvCatGrades
            // 
            this.dgvCatGrades.AllowUserToAddRows = false;
            this.dgvCatGrades.AllowUserToDeleteRows = false;
            this.dgvCatGrades.BackgroundColor = System.Drawing.Color.White;
            this.dgvCatGrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCatGrades.ColumnHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCatGrades.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCatGrades.Location = new System.Drawing.Point(179, 263);
            this.dgvCatGrades.Name = "dgvCatGrades";
            this.dgvCatGrades.ReadOnly = true;
            this.dgvCatGrades.RowHeadersVisible = false;
            this.dgvCatGrades.Size = new System.Drawing.Size(154, 99);
            this.dgvCatGrades.TabIndex = 10;
            this.dgvCatGrades.Visible = false;
            this.dgvCatGrades.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCatGrades_CellContentClick);
            this.dgvCatGrades.DoubleClick += new System.EventHandler(this.dgvCatGrades_DoubleClick);
            this.dgvCatGrades.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCatGrades_KeyDown);
            this.dgvCatGrades.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvCatGrades_KeyPress);
            this.dgvCatGrades.Leave += new System.EventHandler(this.dgvCatGrades_Leave);
            // 
            // Catalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1270, 657);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Catalog";
            this.Load += new System.EventHandler(this.Catalog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Catalog_KeyDown);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGrades)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCatNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVendors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatGrades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.TextBox txtCatDesc;
        private System.Windows.Forms.Label lblCatDesc;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.TextBox txtUnitPrice;
        private System.Windows.Forms.Label lblUnitPrice;
        private System.Windows.Forms.TextBox txtGrade;
        private System.Windows.Forms.TextBox txtCatalogNo;
        private System.Windows.Forms.TextBox txtCatalogName;
        private System.Windows.Forms.Label lblCatalogName;
        private System.Windows.Forms.Label lblGrade;
        private System.Windows.Forms.Label lblCatalogNo;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox picVendors;
        private System.Windows.Forms.Label lblVendor;
        private GISControls.TextBoxChar txtVendorID;
        private GISControls.TextBoxChar txtVendorName;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvVendorNames;
        private System.Windows.Forms.PictureBox picCatNames;
        private System.Windows.Forms.DataGridView dgvCatNames;
        private System.Windows.Forms.Button btnAddNames;
        private GISControls.TextBoxChar txtCatNameID;
        private System.Windows.Forms.DataGridView dgvCatGrades;
        private System.Windows.Forms.PictureBox picGrades;
        private GISControls.TextBoxChar txtGradeID;
        private System.Windows.Forms.Button btnCheckBrowser;
        private System.Windows.Forms.TextBox txtWebsite;
    }
}
