namespace PSS
{
    partial class ControlPages
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPages));
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.btnPrintToQA45 = new System.Windows.Forms.Button();
            this.btnPrintTo45 = new System.Windows.Forms.Button();
            this.txtCmpyCode = new System.Windows.Forms.TextBox();
            this.pnlAdmin = new System.Windows.Forms.Panel();
            this.dgvRequestors = new System.Windows.Forms.DataGridView();
            this.lblRequestor = new System.Windows.Forms.Label();
            this.txtRequestedByID = new GISControls.TextBoxChar();
            this.picRequestors = new System.Windows.Forms.PictureBox();
            this.txtRequestor = new GISControls.TextBoxChar();
            this.label1 = new System.Windows.Forms.Label();
            this.txtChangeReason = new GISControls.TextBoxChar();
            this.txtAdmBookNo = new GISControls.TextBoxChar();
            this.lblAdmBookNo = new System.Windows.Forms.Label();
            this.txtNewServiceCode = new GISControls.TextBoxChar();
            this.lblNewServiceCode = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnUpdateServiceCode = new System.Windows.Forms.Button();
            this.lblAdmin = new System.Windows.Forms.Label();
            this.btnReuse = new System.Windows.Forms.Button();
            this.lblOldServiceCode = new System.Windows.Forms.Label();
            this.txtOldServiceCode = new GISControls.TextBoxChar();
            this.lblGBLNo = new System.Windows.Forms.Label();
            this.txtAdmGBLNo = new GISControls.TextBoxChar();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.btnResetPrint = new System.Windows.Forms.Button();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.btnPrintTo122 = new System.Windows.Forms.Button();
            this.txtUserPrinterName = new GISControls.TextBoxChar();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkPrintAll = new System.Windows.Forms.CheckBox();
            this.txtControlPageID = new System.Windows.Forms.TextBox();
            this.txtTotPgNeeded = new GISControls.TextBoxChar();
            this.lblCPNumList = new System.Windows.Forms.Label();
            this.lblTotPgCount = new System.Windows.Forms.Label();
            this.lblGBLSelector = new System.Windows.Forms.Label();
            this.lblCPGBLList = new System.Windows.Forms.Label();
            this.btnDelSelected = new System.Windows.Forms.Button();
            this.btnAddSelected = new System.Windows.Forms.Button();
            this.dgvGBLList = new System.Windows.Forms.DataGridView();
            this.lblPagesToAdd = new System.Windows.Forms.Label();
            this.txtPagesToAdd = new System.Windows.Forms.TextBox();
            this.btnVoid = new System.Windows.Forms.Button();
            this.btnPrintTo16 = new System.Windows.Forms.Button();
            this.btnAddPage = new System.Windows.Forms.Button();
            this.dgvControlPageNumbers = new System.Windows.Forms.DataGridView();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dgvGBLSelection = new System.Windows.Forms.DataGridView();
            this.txtColumn = new System.Windows.Forms.TextBox();
            this.cboBookNo = new System.Windows.Forms.ComboBox();
            this.txtStatusReason = new System.Windows.Forms.TextBox();
            this.txtPrintStatus = new System.Windows.Forms.TextBox();
            this.txtGRow = new System.Windows.Forms.TextBox();
            this.lblBookNo = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtGBLNo = new System.Windows.Forms.TextBox();
            this.txtTotalPage = new System.Windows.Forms.TextBox();
            this.txtServiceCode = new System.Windows.Forms.TextBox();
            this.bsGBLSelection = new System.Windows.Forms.BindingSource(this.components);
            this.bsCPNumbers = new System.Windows.Forms.BindingSource(this.components);
            this.bsGBLList = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            this.pnlAdmin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequestors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRequestors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGBLList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvControlPageNumbers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGBLSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGBLSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCPNumbers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGBLList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.btnPrintToQA45);
            this.pnlRecord.Controls.Add(this.btnPrintTo45);
            this.pnlRecord.Controls.Add(this.txtCmpyCode);
            this.pnlRecord.Controls.Add(this.pnlAdmin);
            this.pnlRecord.Controls.Add(this.btnResetPrint);
            this.pnlRecord.Controls.Add(this.btnAdmin);
            this.pnlRecord.Controls.Add(this.btnPrintTo122);
            this.pnlRecord.Controls.Add(this.txtUserPrinterName);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.chkPrintAll);
            this.pnlRecord.Controls.Add(this.txtControlPageID);
            this.pnlRecord.Controls.Add(this.txtTotPgNeeded);
            this.pnlRecord.Controls.Add(this.lblCPNumList);
            this.pnlRecord.Controls.Add(this.lblTotPgCount);
            this.pnlRecord.Controls.Add(this.lblGBLSelector);
            this.pnlRecord.Controls.Add(this.lblCPGBLList);
            this.pnlRecord.Controls.Add(this.btnDelSelected);
            this.pnlRecord.Controls.Add(this.btnAddSelected);
            this.pnlRecord.Controls.Add(this.dgvGBLList);
            this.pnlRecord.Controls.Add(this.lblPagesToAdd);
            this.pnlRecord.Controls.Add(this.txtPagesToAdd);
            this.pnlRecord.Controls.Add(this.btnVoid);
            this.pnlRecord.Controls.Add(this.btnPrintTo16);
            this.pnlRecord.Controls.Add(this.btnAddPage);
            this.pnlRecord.Controls.Add(this.dgvControlPageNumbers);
            this.pnlRecord.Controls.Add(this.btnGenerate);
            this.pnlRecord.Controls.Add(this.dgvGBLSelection);
            this.pnlRecord.Controls.Add(this.txtColumn);
            this.pnlRecord.Controls.Add(this.cboBookNo);
            this.pnlRecord.Controls.Add(this.txtStatusReason);
            this.pnlRecord.Controls.Add(this.txtPrintStatus);
            this.pnlRecord.Controls.Add(this.txtGRow);
            this.pnlRecord.Controls.Add(this.lblBookNo);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.txtGBLNo);
            this.pnlRecord.Controls.Add(this.txtTotalPage);
            this.pnlRecord.Controls.Add(this.txtServiceCode);
            this.pnlRecord.Location = new System.Drawing.Point(12, 89);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(836, 649);
            this.pnlRecord.TabIndex = 105;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // btnPrintToQA45
            // 
            this.btnPrintToQA45.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintToQA45.Location = new System.Drawing.Point(215, 589);
            this.btnPrintToQA45.Name = "btnPrintToQA45";
            this.btnPrintToQA45.Size = new System.Drawing.Size(93, 23);
            this.btnPrintToQA45.TabIndex = 414;
            this.btnPrintToQA45.Text = "Print at QA 45";
            this.btnPrintToQA45.UseVisualStyleBackColor = true;
            this.btnPrintToQA45.Click += new System.EventHandler(this.btnPrintToQA45_Click);
            // 
            // btnPrintTo45
            // 
            this.btnPrintTo45.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintTo45.Location = new System.Drawing.Point(314, 589);
            this.btnPrintTo45.Name = "btnPrintTo45";
            this.btnPrintTo45.Size = new System.Drawing.Size(127, 23);
            this.btnPrintTo45.TabIndex = 413;
            this.btnPrintTo45.Text = "Print at Office on 45";
            this.btnPrintTo45.UseVisualStyleBackColor = true;
            this.btnPrintTo45.Click += new System.EventHandler(this.btnPrintTo45_Click);
            // 
            // txtCmpyCode
            // 
            this.txtCmpyCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCmpyCode.Location = new System.Drawing.Point(3, 43);
            this.txtCmpyCode.Multiline = true;
            this.txtCmpyCode.Name = "txtCmpyCode";
            this.txtCmpyCode.Size = new System.Drawing.Size(25, 21);
            this.txtCmpyCode.TabIndex = 393;
            this.txtCmpyCode.Visible = false;
            // 
            // pnlAdmin
            // 
            this.pnlAdmin.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlAdmin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAdmin.Controls.Add(this.dgvRequestors);
            this.pnlAdmin.Controls.Add(this.lblRequestor);
            this.pnlAdmin.Controls.Add(this.txtRequestedByID);
            this.pnlAdmin.Controls.Add(this.picRequestors);
            this.pnlAdmin.Controls.Add(this.txtRequestor);
            this.pnlAdmin.Controls.Add(this.label1);
            this.pnlAdmin.Controls.Add(this.txtChangeReason);
            this.pnlAdmin.Controls.Add(this.txtAdmBookNo);
            this.pnlAdmin.Controls.Add(this.lblAdmBookNo);
            this.pnlAdmin.Controls.Add(this.txtNewServiceCode);
            this.pnlAdmin.Controls.Add(this.lblNewServiceCode);
            this.pnlAdmin.Controls.Add(this.btnOK);
            this.pnlAdmin.Controls.Add(this.btnUpdateServiceCode);
            this.pnlAdmin.Controls.Add(this.lblAdmin);
            this.pnlAdmin.Controls.Add(this.btnReuse);
            this.pnlAdmin.Controls.Add(this.lblOldServiceCode);
            this.pnlAdmin.Controls.Add(this.txtOldServiceCode);
            this.pnlAdmin.Controls.Add(this.lblGBLNo);
            this.pnlAdmin.Controls.Add(this.txtAdmGBLNo);
            this.pnlAdmin.Controls.Add(this.shapeContainer1);
            this.pnlAdmin.Location = new System.Drawing.Point(194, 166);
            this.pnlAdmin.Name = "pnlAdmin";
            this.pnlAdmin.Size = new System.Drawing.Size(430, 367);
            this.pnlAdmin.TabIndex = 106;
            this.pnlAdmin.Visible = false;
            // 
            // dgvRequestors
            // 
            this.dgvRequestors.AllowUserToAddRows = false;
            this.dgvRequestors.AllowUserToDeleteRows = false;
            this.dgvRequestors.BackgroundColor = System.Drawing.Color.White;
            this.dgvRequestors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRequestors.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRequestors.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRequestors.Location = new System.Drawing.Point(177, 152);
            this.dgvRequestors.Name = "dgvRequestors";
            this.dgvRequestors.ReadOnly = true;
            this.dgvRequestors.RowHeadersVisible = false;
            this.dgvRequestors.Size = new System.Drawing.Size(215, 122);
            this.dgvRequestors.TabIndex = 412;
            this.dgvRequestors.Visible = false;
            this.dgvRequestors.DoubleClick += new System.EventHandler(this.dgvRequestors_DoubleClick);
            this.dgvRequestors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvRequestors_KeyDown);
            this.dgvRequestors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvRequestors_KeyPress);
            this.dgvRequestors.Leave += new System.EventHandler(this.dgvRequestors_Leave);
            // 
            // lblRequestor
            // 
            this.lblRequestor.BackColor = System.Drawing.Color.Transparent;
            this.lblRequestor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequestor.ForeColor = System.Drawing.Color.Black;
            this.lblRequestor.Location = new System.Drawing.Point(28, 119);
            this.lblRequestor.Name = "lblRequestor";
            this.lblRequestor.Padding = new System.Windows.Forms.Padding(2);
            this.lblRequestor.Size = new System.Drawing.Size(73, 44);
            this.lblRequestor.TabIndex = 411;
            this.lblRequestor.Text = "Change Requestor:";
            this.lblRequestor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRequestedByID
            // 
            this.txtRequestedByID.BackColor = System.Drawing.Color.White;
            this.txtRequestedByID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRequestedByID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRequestedByID.Location = new System.Drawing.Point(103, 132);
            this.txtRequestedByID.MaxLength = 5;
            this.txtRequestedByID.Name = "txtRequestedByID";
            this.txtRequestedByID.Size = new System.Drawing.Size(68, 21);
            this.txtRequestedByID.TabIndex = 410;
            this.txtRequestedByID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRequestedByID_KeyPress);
            // 
            // picRequestors
            // 
            this.picRequestors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picRequestors.BackgroundImage")));
            this.picRequestors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picRequestors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picRequestors.Location = new System.Drawing.Point(372, 132);
            this.picRequestors.Name = "picRequestors";
            this.picRequestors.Size = new System.Drawing.Size(20, 21);
            this.picRequestors.TabIndex = 409;
            this.picRequestors.TabStop = false;
            this.picRequestors.Click += new System.EventHandler(this.picRequestors_Click);
            // 
            // txtRequestor
            // 
            this.txtRequestor.BackColor = System.Drawing.Color.White;
            this.txtRequestor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRequestor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRequestor.Location = new System.Drawing.Point(177, 132);
            this.txtRequestor.Name = "txtRequestor";
            this.txtRequestor.Size = new System.Drawing.Size(196, 21);
            this.txtRequestor.TabIndex = 408;
            this.txtRequestor.TextChanged += new System.EventHandler(this.txtRequestor_TextChanged);
            this.txtRequestor.Enter += new System.EventHandler(this.txtRequestor_Enter);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(28, 176);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(138, 21);
            this.label1.TabIndex = 407;
            this.label1.Text = "Reason for Change:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtChangeReason
            // 
            this.txtChangeReason.BackColor = System.Drawing.Color.White;
            this.txtChangeReason.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChangeReason.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChangeReason.ForeColor = System.Drawing.Color.Black;
            this.txtChangeReason.Location = new System.Drawing.Point(31, 200);
            this.txtChangeReason.Multiline = true;
            this.txtChangeReason.Name = "txtChangeReason";
            this.txtChangeReason.Size = new System.Drawing.Size(361, 74);
            this.txtChangeReason.TabIndex = 406;
            // 
            // txtAdmBookNo
            // 
            this.txtAdmBookNo.BackColor = System.Drawing.Color.White;
            this.txtAdmBookNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAdmBookNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdmBookNo.ForeColor = System.Drawing.Color.Black;
            this.txtAdmBookNo.Location = new System.Drawing.Point(103, 59);
            this.txtAdmBookNo.MaxLength = 50;
            this.txtAdmBookNo.Name = "txtAdmBookNo";
            this.txtAdmBookNo.Size = new System.Drawing.Size(84, 21);
            this.txtAdmBookNo.TabIndex = 405;
            this.txtAdmBookNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAdmBookNo_KeyPress);
            // 
            // lblAdmBookNo
            // 
            this.lblAdmBookNo.BackColor = System.Drawing.Color.Transparent;
            this.lblAdmBookNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdmBookNo.ForeColor = System.Drawing.Color.Black;
            this.lblAdmBookNo.Location = new System.Drawing.Point(24, 59);
            this.lblAdmBookNo.Name = "lblAdmBookNo";
            this.lblAdmBookNo.Padding = new System.Windows.Forms.Padding(2);
            this.lblAdmBookNo.Size = new System.Drawing.Size(72, 21);
            this.lblAdmBookNo.TabIndex = 404;
            this.lblAdmBookNo.Text = "Book No:";
            this.lblAdmBookNo.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txtNewServiceCode
            // 
            this.txtNewServiceCode.BackColor = System.Drawing.Color.White;
            this.txtNewServiceCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewServiceCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewServiceCode.ForeColor = System.Drawing.Color.Black;
            this.txtNewServiceCode.Location = new System.Drawing.Point(329, 86);
            this.txtNewServiceCode.MaxLength = 50;
            this.txtNewServiceCode.Name = "txtNewServiceCode";
            this.txtNewServiceCode.Size = new System.Drawing.Size(63, 21);
            this.txtNewServiceCode.TabIndex = 403;
            this.txtNewServiceCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewServiceCode_KeyPress);
            // 
            // lblNewServiceCode
            // 
            this.lblNewServiceCode.BackColor = System.Drawing.Color.Transparent;
            this.lblNewServiceCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewServiceCode.ForeColor = System.Drawing.Color.Black;
            this.lblNewServiceCode.Location = new System.Drawing.Point(203, 83);
            this.lblNewServiceCode.Name = "lblNewServiceCode";
            this.lblNewServiceCode.Padding = new System.Windows.Forms.Padding(2);
            this.lblNewServiceCode.Size = new System.Drawing.Size(115, 21);
            this.lblNewServiceCode.TabIndex = 402;
            this.lblNewServiceCode.Text = "New Service Code:";
            this.lblNewServiceCode.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Firebrick;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOK.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(394, -3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(35, 22);
            this.btnOK.TabIndex = 401;
            this.btnOK.Text = "[X]";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnUpdateServiceCode
            // 
            this.btnUpdateServiceCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdateServiceCode.Location = new System.Drawing.Point(226, 313);
            this.btnUpdateServiceCode.Name = "btnUpdateServiceCode";
            this.btnUpdateServiceCode.Size = new System.Drawing.Size(166, 23);
            this.btnUpdateServiceCode.TabIndex = 400;
            this.btnUpdateServiceCode.Text = "Update Service Code";
            this.btnUpdateServiceCode.UseVisualStyleBackColor = true;
            this.btnUpdateServiceCode.Click += new System.EventHandler(this.btnUpdateServiceCode_Click);
            // 
            // lblAdmin
            // 
            this.lblAdmin.BackColor = System.Drawing.Color.SteelBlue;
            this.lblAdmin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAdmin.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdmin.ForeColor = System.Drawing.Color.White;
            this.lblAdmin.Location = new System.Drawing.Point(0, -2);
            this.lblAdmin.Name = "lblAdmin";
            this.lblAdmin.Size = new System.Drawing.Size(428, 21);
            this.lblAdmin.TabIndex = 399;
            this.lblAdmin.Text = "Admin Functions";
            this.lblAdmin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReuse
            // 
            this.btnReuse.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReuse.Location = new System.Drawing.Point(38, 313);
            this.btnReuse.Name = "btnReuse";
            this.btnReuse.Size = new System.Drawing.Size(166, 23);
            this.btnReuse.TabIndex = 398;
            this.btnReuse.Text = "Reuse PSS Number";
            this.btnReuse.UseVisualStyleBackColor = true;
            this.btnReuse.Click += new System.EventHandler(this.btnReuse_Click);
            // 
            // lblOldServiceCode
            // 
            this.lblOldServiceCode.BackColor = System.Drawing.Color.Transparent;
            this.lblOldServiceCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOldServiceCode.ForeColor = System.Drawing.Color.Black;
            this.lblOldServiceCode.Location = new System.Drawing.Point(203, 59);
            this.lblOldServiceCode.Name = "lblOldServiceCode";
            this.lblOldServiceCode.Padding = new System.Windows.Forms.Padding(2);
            this.lblOldServiceCode.Size = new System.Drawing.Size(115, 21);
            this.lblOldServiceCode.TabIndex = 298;
            this.lblOldServiceCode.Text = "Old Service Code:";
            this.lblOldServiceCode.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txtOldServiceCode
            // 
            this.txtOldServiceCode.BackColor = System.Drawing.Color.White;
            this.txtOldServiceCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOldServiceCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOldServiceCode.ForeColor = System.Drawing.Color.Black;
            this.txtOldServiceCode.Location = new System.Drawing.Point(329, 62);
            this.txtOldServiceCode.MaxLength = 50;
            this.txtOldServiceCode.Name = "txtOldServiceCode";
            this.txtOldServiceCode.Size = new System.Drawing.Size(63, 21);
            this.txtOldServiceCode.TabIndex = 1;
            this.txtOldServiceCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtOldServiceCode_KeyPress);
            // 
            // lblGBLNo
            // 
            this.lblGBLNo.BackColor = System.Drawing.Color.Transparent;
            this.lblGBLNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGBLNo.ForeColor = System.Drawing.Color.Black;
            this.lblGBLNo.Location = new System.Drawing.Point(24, 80);
            this.lblGBLNo.Name = "lblGBLNo";
            this.lblGBLNo.Padding = new System.Windows.Forms.Padding(2);
            this.lblGBLNo.Size = new System.Drawing.Size(72, 21);
            this.lblGBLNo.TabIndex = 250;
            this.lblGBLNo.Text = "PSS No:";
            this.lblGBLNo.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // txtAdmGBLNo
            // 
            this.txtAdmGBLNo.BackColor = System.Drawing.Color.White;
            this.txtAdmGBLNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAdmGBLNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdmGBLNo.ForeColor = System.Drawing.Color.Black;
            this.txtAdmGBLNo.Location = new System.Drawing.Point(103, 83);
            this.txtAdmGBLNo.MaxLength = 50;
            this.txtAdmGBLNo.Name = "txtAdmGBLNo";
            this.txtAdmGBLNo.Size = new System.Drawing.Size(84, 21);
            this.txtAdmGBLNo.TabIndex = 0;
            this.txtAdmGBLNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAdmGBLNo_KeyPress);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1,
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(428, 365);
            this.shapeContainer1.TabIndex = 281;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 16;
            this.lineShape1.X2 = 408;
            this.lineShape1.Y1 = 298;
            this.lineShape1.Y2 = 298;
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.Location = new System.Drawing.Point(17, 42);
            this.rectangleShape1.Name = "rectangleShape3";
            this.rectangleShape1.Size = new System.Drawing.Size(392, 307);
            // 
            // btnResetPrint
            // 
            this.btnResetPrint.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetPrint.Location = new System.Drawing.Point(642, 616);
            this.btnResetPrint.Name = "btnResetPrint";
            this.btnResetPrint.Size = new System.Drawing.Size(83, 23);
            this.btnResetPrint.TabIndex = 392;
            this.btnResetPrint.Text = "Reset Print";
            this.btnResetPrint.UseVisualStyleBackColor = true;
            this.btnResetPrint.Visible = false;
            this.btnResetPrint.Click += new System.EventHandler(this.btnResetPrint_Click);
            // 
            // btnAdmin
            // 
            this.btnAdmin.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdmin.Location = new System.Drawing.Point(733, 616);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(69, 23);
            this.btnAdmin.TabIndex = 397;
            this.btnAdmin.Text = "Admin";
            this.btnAdmin.UseVisualStyleBackColor = true;
            this.btnAdmin.Visible = false;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);
            // 
            // btnPrintTo122
            // 
            this.btnPrintTo122.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintTo122.Location = new System.Drawing.Point(447, 589);
            this.btnPrintTo122.Name = "btnPrintTo122";
            this.btnPrintTo122.Size = new System.Drawing.Size(126, 23);
            this.btnPrintTo122.TabIndex = 396;
            this.btnPrintTo122.Text = "Print to Sterilization";
            this.btnPrintTo122.UseVisualStyleBackColor = true;
            this.btnPrintTo122.Click += new System.EventHandler(this.btnPrintTo122_Click);
            // 
            // txtUserPrinterName
            // 
            this.txtUserPrinterName.BackColor = System.Drawing.Color.White;
            this.txtUserPrinterName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserPrinterName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserPrinterName.ForeColor = System.Drawing.Color.Red;
            this.txtUserPrinterName.Location = new System.Drawing.Point(3, 176);
            this.txtUserPrinterName.MaxLength = 5;
            this.txtUserPrinterName.Name = "txtUserPrinterName";
            this.txtUserPrinterName.Size = new System.Drawing.Size(25, 21);
            this.txtUserPrinterName.TabIndex = 395;
            this.txtUserPrinterName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUserPrinterName.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(763, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 22);
            this.btnClose.TabIndex = 394;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkPrintAll
            // 
            this.chkPrintAll.AutoSize = true;
            this.chkPrintAll.Location = new System.Drawing.Point(725, 256);
            this.chkPrintAll.Name = "chkPrintAll";
            this.chkPrintAll.Size = new System.Drawing.Size(76, 19);
            this.chkPrintAll.TabIndex = 186;
            this.chkPrintAll.Text = "Check All";
            this.chkPrintAll.UseVisualStyleBackColor = true;
            this.chkPrintAll.CheckStateChanged += new System.EventHandler(this.chkPrintAll_CheckStateChanged);
            // 
            // txtControlPageID
            // 
            this.txtControlPageID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtControlPageID.Location = new System.Drawing.Point(3, 70);
            this.txtControlPageID.Multiline = true;
            this.txtControlPageID.Name = "txtControlPageID";
            this.txtControlPageID.Size = new System.Drawing.Size(25, 21);
            this.txtControlPageID.TabIndex = 185;
            this.txtControlPageID.Visible = false;
            // 
            // txtTotPgNeeded
            // 
            this.txtTotPgNeeded.BackColor = System.Drawing.Color.White;
            this.txtTotPgNeeded.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotPgNeeded.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotPgNeeded.ForeColor = System.Drawing.Color.Red;
            this.txtTotPgNeeded.Location = new System.Drawing.Point(748, 145);
            this.txtTotPgNeeded.MaxLength = 5;
            this.txtTotPgNeeded.Name = "txtTotPgNeeded";
            this.txtTotPgNeeded.Size = new System.Drawing.Size(32, 21);
            this.txtTotPgNeeded.TabIndex = 183;
            this.txtTotPgNeeded.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotPgNeeded.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTotPgNeeded_KeyPress);
            // 
            // lblCPNumList
            // 
            this.lblCPNumList.BackColor = System.Drawing.Color.Transparent;
            this.lblCPNumList.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPNumList.ForeColor = System.Drawing.Color.Red;
            this.lblCPNumList.Location = new System.Drawing.Point(153, 254);
            this.lblCPNumList.Name = "lblCPNumList";
            this.lblCPNumList.Padding = new System.Windows.Forms.Padding(2);
            this.lblCPNumList.Size = new System.Drawing.Size(552, 21);
            this.lblCPNumList.TabIndex = 182;
            this.lblCPNumList.Text = "<<<<<     Control Page Sequence Numbers List     >>>>>";
            this.lblCPNumList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotPgCount
            // 
            this.lblTotPgCount.BackColor = System.Drawing.Color.Transparent;
            this.lblTotPgCount.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotPgCount.ForeColor = System.Drawing.Color.Black;
            this.lblTotPgCount.Location = new System.Drawing.Point(717, 90);
            this.lblTotPgCount.Name = "lblTotPgCount";
            this.lblTotPgCount.Padding = new System.Windows.Forms.Padding(2);
            this.lblTotPgCount.Size = new System.Drawing.Size(92, 53);
            this.lblTotPgCount.TabIndex = 179;
            this.lblTotPgCount.Text = "Total Pages      Needed:";
            this.lblTotPgCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGBLSelector
            // 
            this.lblGBLSelector.BackColor = System.Drawing.Color.Transparent;
            this.lblGBLSelector.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGBLSelector.ForeColor = System.Drawing.Color.Red;
            this.lblGBLSelector.Location = new System.Drawing.Point(33, 72);
            this.lblGBLSelector.Name = "lblGBLSelector";
            this.lblGBLSelector.Padding = new System.Windows.Forms.Padding(2);
            this.lblGBLSelector.Size = new System.Drawing.Size(259, 21);
            this.lblGBLSelector.TabIndex = 176;
            this.lblGBLSelector.Text = "<<<<<     PSS Nos. to Choose From:      >>>>>";
            this.lblGBLSelector.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCPGBLList
            // 
            this.lblCPGBLList.BackColor = System.Drawing.Color.Transparent;
            this.lblCPGBLList.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPGBLList.ForeColor = System.Drawing.Color.Red;
            this.lblCPGBLList.Location = new System.Drawing.Point(368, 72);
            this.lblCPGBLList.Name = "lblCPGBLList";
            this.lblCPGBLList.Padding = new System.Windows.Forms.Padding(2);
            this.lblCPGBLList.Size = new System.Drawing.Size(310, 21);
            this.lblCPGBLList.TabIndex = 173;
            this.lblCPGBLList.Text = "<<<<<     Control Page PSS Nos. List     >>>>>";
            this.lblCPGBLList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDelSelected
            // 
            this.btnDelSelected.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelSelected.ForeColor = System.Drawing.Color.Red;
            this.btnDelSelected.Location = new System.Drawing.Point(301, 184);
            this.btnDelSelected.Name = "btnDelSelected";
            this.btnDelSelected.Size = new System.Drawing.Size(35, 24);
            this.btnDelSelected.TabIndex = 171;
            this.btnDelSelected.Text = "<";
            this.btnDelSelected.UseVisualStyleBackColor = true;
            this.btnDelSelected.Click += new System.EventHandler(this.btnDelSelected_Click);
            // 
            // btnAddSelected
            // 
            this.btnAddSelected.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSelected.ForeColor = System.Drawing.Color.Red;
            this.btnAddSelected.Location = new System.Drawing.Point(301, 136);
            this.btnAddSelected.Name = "btnAddSelected";
            this.btnAddSelected.Size = new System.Drawing.Size(35, 24);
            this.btnAddSelected.TabIndex = 170;
            this.btnAddSelected.Text = ">";
            this.btnAddSelected.UseVisualStyleBackColor = true;
            this.btnAddSelected.Click += new System.EventHandler(this.btnAddSelected_Click);
            // 
            // dgvGBLList
            // 
            this.dgvGBLList.AllowUserToAddRows = false;
            this.dgvGBLList.AllowUserToDeleteRows = false;
            this.dgvGBLList.AllowUserToResizeColumns = false;
            this.dgvGBLList.AllowUserToResizeRows = false;
            this.dgvGBLList.BackgroundColor = System.Drawing.Color.White;
            this.dgvGBLList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGBLList.Location = new System.Drawing.Point(33, 96);
            this.dgvGBLList.Name = "dgvGBLList";
            this.dgvGBLList.ReadOnly = true;
            this.dgvGBLList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvGBLList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGBLList.Size = new System.Drawing.Size(259, 145);
            this.dgvGBLList.TabIndex = 168;
            // 
            // lblPagesToAdd
            // 
            this.lblPagesToAdd.BackColor = System.Drawing.Color.Transparent;
            this.lblPagesToAdd.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPagesToAdd.ForeColor = System.Drawing.Color.Black;
            this.lblPagesToAdd.Location = new System.Drawing.Point(31, 588);
            this.lblPagesToAdd.Name = "lblPagesToAdd";
            this.lblPagesToAdd.Padding = new System.Windows.Forms.Padding(2);
            this.lblPagesToAdd.Size = new System.Drawing.Size(143, 21);
            this.lblPagesToAdd.TabIndex = 167;
            this.lblPagesToAdd.Text = "Individual Pages to Add:";
            this.lblPagesToAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPagesToAdd
            // 
            this.txtPagesToAdd.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPagesToAdd.Location = new System.Drawing.Point(174, 589);
            this.txtPagesToAdd.Name = "txtPagesToAdd";
            this.txtPagesToAdd.Size = new System.Drawing.Size(35, 21);
            this.txtPagesToAdd.TabIndex = 5;
            this.txtPagesToAdd.Text = "1";
            this.txtPagesToAdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPagesToAdd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPagesToAdd_KeyPress);
            // 
            // btnVoid
            // 
            this.btnVoid.Enabled = false;
            this.btnVoid.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVoid.Location = new System.Drawing.Point(761, 589);
            this.btnVoid.Name = "btnVoid";
            this.btnVoid.Size = new System.Drawing.Size(39, 23);
            this.btnVoid.TabIndex = 8;
            this.btnVoid.Text = "Void";
            this.btnVoid.UseVisualStyleBackColor = true;
            this.btnVoid.Click += new System.EventHandler(this.btnVoid_Click);
            // 
            // btnPrintTo16
            // 
            this.btnPrintTo16.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrintTo16.Location = new System.Drawing.Point(579, 589);
            this.btnPrintTo16.Name = "btnPrintTo16";
            this.btnPrintTo16.Size = new System.Drawing.Size(101, 23);
            this.btnPrintTo16.TabIndex = 6;
            this.btnPrintTo16.Text = "Print to Bldg 16";
            this.btnPrintTo16.UseVisualStyleBackColor = true;
            this.btnPrintTo16.Click += new System.EventHandler(this.btnPrintTo16_Click);
            // 
            // btnAddPage
            // 
            this.btnAddPage.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPage.Location = new System.Drawing.Point(686, 589);
            this.btnAddPage.Name = "btnAddPage";
            this.btnAddPage.Size = new System.Drawing.Size(69, 23);
            this.btnAddPage.TabIndex = 7;
            this.btnAddPage.Text = "Add Page";
            this.btnAddPage.UseVisualStyleBackColor = true;
            this.btnAddPage.Click += new System.EventHandler(this.btnAddPage_Click);
            // 
            // dgvControlPageNumbers
            // 
            this.dgvControlPageNumbers.AllowUserToAddRows = false;
            this.dgvControlPageNumbers.AllowUserToDeleteRows = false;
            this.dgvControlPageNumbers.AllowUserToResizeColumns = false;
            this.dgvControlPageNumbers.BackgroundColor = System.Drawing.Color.White;
            this.dgvControlPageNumbers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvControlPageNumbers.Location = new System.Drawing.Point(33, 278);
            this.dgvControlPageNumbers.Name = "dgvControlPageNumbers";
            this.dgvControlPageNumbers.Size = new System.Drawing.Size(768, 302);
            this.dgvControlPageNumbers.TabIndex = 4;
            this.dgvControlPageNumbers.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvControlPageNumbers_CellBeginEdit);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.ForeColor = System.Drawing.Color.Red;
            this.btnGenerate.Location = new System.Drawing.Point(725, 184);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(84, 34);
            this.btnGenerate.TabIndex = 3;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dgvGBLSelection
            // 
            this.dgvGBLSelection.AllowUserToAddRows = false;
            this.dgvGBLSelection.AllowUserToDeleteRows = false;
            this.dgvGBLSelection.AllowUserToResizeColumns = false;
            this.dgvGBLSelection.AllowUserToResizeRows = false;
            this.dgvGBLSelection.BackgroundColor = System.Drawing.Color.White;
            this.dgvGBLSelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGBLSelection.Location = new System.Drawing.Point(345, 96);
            this.dgvGBLSelection.Name = "dgvGBLSelection";
            this.dgvGBLSelection.ReadOnly = true;
            this.dgvGBLSelection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGBLSelection.Size = new System.Drawing.Size(360, 145);
            this.dgvGBLSelection.TabIndex = 2;
            this.dgvGBLSelection.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvGBLSelection_CellBeginEdit);
            this.dgvGBLSelection.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGBLSelection_CellClick);
            // 
            // txtColumn
            // 
            this.txtColumn.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtColumn.Location = new System.Drawing.Point(3, 426);
            this.txtColumn.Name = "txtColumn";
            this.txtColumn.Size = new System.Drawing.Size(25, 21);
            this.txtColumn.TabIndex = 157;
            this.txtColumn.Visible = false;
            // 
            // cboBookNo
            // 
            this.cboBookNo.FormattingEnabled = true;
            this.cboBookNo.Location = new System.Drawing.Point(122, 39);
            this.cboBookNo.Name = "cboBookNo";
            this.cboBookNo.Size = new System.Drawing.Size(86, 23);
            this.cboBookNo.TabIndex = 9;
            this.cboBookNo.SelectedValueChanged += new System.EventHandler(this.cboBookNo_SelectedValueChanged);
            this.cboBookNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboBookNo_KeyPress);
            // 
            // txtStatusReason
            // 
            this.txtStatusReason.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusReason.Location = new System.Drawing.Point(3, 399);
            this.txtStatusReason.Name = "txtStatusReason";
            this.txtStatusReason.Size = new System.Drawing.Size(25, 21);
            this.txtStatusReason.TabIndex = 152;
            this.txtStatusReason.Visible = false;
            // 
            // txtPrintStatus
            // 
            this.txtPrintStatus.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrintStatus.Location = new System.Drawing.Point(3, 372);
            this.txtPrintStatus.Name = "txtPrintStatus";
            this.txtPrintStatus.Size = new System.Drawing.Size(25, 21);
            this.txtPrintStatus.TabIndex = 151;
            this.txtPrintStatus.Visible = false;
            // 
            // txtGRow
            // 
            this.txtGRow.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGRow.Location = new System.Drawing.Point(3, 345);
            this.txtGRow.Name = "txtGRow";
            this.txtGRow.Size = new System.Drawing.Size(25, 21);
            this.txtGRow.TabIndex = 150;
            this.txtGRow.Visible = false;
            // 
            // lblBookNo
            // 
            this.lblBookNo.BackColor = System.Drawing.Color.Transparent;
            this.lblBookNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBookNo.ForeColor = System.Drawing.Color.Black;
            this.lblBookNo.Location = new System.Drawing.Point(29, 41);
            this.lblBookNo.Name = "lblBookNo";
            this.lblBookNo.Padding = new System.Windows.Forms.Padding(2);
            this.lblBookNo.Size = new System.Drawing.Size(99, 21);
            this.lblBookNo.TabIndex = 147;
            this.lblBookNo.Text = "Book No:";
            this.lblBookNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-2, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(837, 21);
            this.lblHeader.TabIndex = 10;
            this.lblHeader.Text = "CONTROL PAGE MASTER";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseDown);
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // txtGBLNo
            // 
            this.txtGBLNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGBLNo.Location = new System.Drawing.Point(3, 96);
            this.txtGBLNo.Name = "txtGBLNo";
            this.txtGBLNo.Size = new System.Drawing.Size(25, 21);
            this.txtGBLNo.TabIndex = 148;
            this.txtGBLNo.Visible = false;
            // 
            // txtTotalPage
            // 
            this.txtTotalPage.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalPage.Location = new System.Drawing.Point(3, 122);
            this.txtTotalPage.Name = "txtTotalPage";
            this.txtTotalPage.Size = new System.Drawing.Size(25, 21);
            this.txtTotalPage.TabIndex = 153;
            this.txtTotalPage.Visible = false;
            // 
            // txtServiceCode
            // 
            this.txtServiceCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServiceCode.Location = new System.Drawing.Point(3, 149);
            this.txtServiceCode.Name = "txtServiceCode";
            this.txtServiceCode.Size = new System.Drawing.Size(25, 21);
            this.txtServiceCode.TabIndex = 149;
            this.txtServiceCode.Visible = false;
            // 
            // ControlPages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1276, 755);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ControlPages";
            this.Load += new System.EventHandler(this.ControlPages_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ControlPages_KeyDown);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            this.pnlAdmin.ResumeLayout(false);
            this.pnlAdmin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRequestors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRequestors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGBLList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvControlPageNumbers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGBLSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGBLSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCPNumbers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGBLList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ComboBox cboBookNo;
        private System.Windows.Forms.TextBox txtTotalPage;
        private System.Windows.Forms.TextBox txtStatusReason;
        private System.Windows.Forms.TextBox txtPrintStatus;
        private System.Windows.Forms.TextBox txtGRow;
        private System.Windows.Forms.TextBox txtServiceCode;
        private System.Windows.Forms.TextBox txtGBLNo;
        private System.Windows.Forms.Label lblBookNo;
        private System.Windows.Forms.TextBox txtColumn;
        private System.Windows.Forms.Label lblPagesToAdd;
        private System.Windows.Forms.TextBox txtPagesToAdd;
        private System.Windows.Forms.Button btnVoid;
        private System.Windows.Forms.Button btnPrintTo16;
        private System.Windows.Forms.Button btnAddPage;
        private System.Windows.Forms.DataGridView dgvControlPageNumbers;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.DataGridView dgvGBLSelection;
        private System.Windows.Forms.BindingSource bsGBLSelection;
        private System.Windows.Forms.BindingSource bsCPNumbers;
        private System.Windows.Forms.DataGridView dgvGBLList;
        private System.Windows.Forms.BindingSource bsGBLList;
        private System.Windows.Forms.Button btnDelSelected;
        private System.Windows.Forms.Button btnAddSelected;
        private System.Windows.Forms.Label lblTotPgCount;
        private System.Windows.Forms.Label lblGBLSelector;
        private System.Windows.Forms.Label lblCPGBLList;
        private System.Windows.Forms.Label lblCPNumList;
        private GISControls.TextBoxChar txtTotPgNeeded;
        private System.Windows.Forms.TextBox txtControlPageID;
        private System.Windows.Forms.CheckBox chkPrintAll;
        private System.Windows.Forms.Button btnClose;
        private GISControls.TextBoxChar txtUserPrinterName;
        private System.Windows.Forms.Button btnPrintTo122;
        private System.Windows.Forms.Button btnAdmin;
        private System.Windows.Forms.Panel pnlAdmin;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnUpdateServiceCode;
        private System.Windows.Forms.Label lblAdmin;
        private System.Windows.Forms.Button btnReuse;
        private System.Windows.Forms.Label lblOldServiceCode;
        private GISControls.TextBoxChar txtOldServiceCode;
        private System.Windows.Forms.Label lblGBLNo;
        private GISControls.TextBoxChar txtAdmGBLNo;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private System.Windows.Forms.Label lblNewServiceCode;
        private GISControls.TextBoxChar txtNewServiceCode;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private GISControls.TextBoxChar txtAdmBookNo;
        private System.Windows.Forms.Label lblAdmBookNo;
        private System.Windows.Forms.Label label1;
        private GISControls.TextBoxChar txtChangeReason;
        private System.Windows.Forms.PictureBox picRequestors;
        private GISControls.TextBoxChar txtRequestor;
        private System.Windows.Forms.Label lblRequestor;
        private GISControls.TextBoxChar txtRequestedByID;
        private System.Windows.Forms.DataGridView dgvRequestors;
        private System.Windows.Forms.Button btnResetPrint;
        private System.Windows.Forms.TextBox txtCmpyCode;
        private System.Windows.Forms.Button btnPrintTo45;
        private System.Windows.Forms.Button btnPrintToQA45;
    }
}
