namespace PSS
{
    partial class InvoiceExport
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
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.btnExportNS = new System.Windows.Forms.Button();
            this.dgvOldInvDetails = new System.Windows.Forms.DataGridView();
            this.btnExportOld = new System.Windows.Forms.Button();
            this.pnlInvDetails = new System.Windows.Forms.Panel();
            this.lblDetPONo = new System.Windows.Forms.Label();
            this.txtDetPONo = new GISControls.TextBoxChar();
            this.txtDetInvDate = new GISControls.TextBoxChar();
            this.lblDetInvDate = new System.Windows.Forms.Label();
            this.txtDetInvTotal = new GISControls.TextBoxChar();
            this.lblDetInvTotal = new System.Windows.Forms.Label();
            this.txtDetInvNo = new GISControls.TextBoxChar();
            this.lblDetInvNo = new System.Windows.Forms.Label();
            this.btnCloseDetails = new System.Windows.Forms.Button();
            this.lblInvDetails = new System.Windows.Forms.Label();
            this.dgvInvDetails = new System.Windows.Forms.DataGridView();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape2 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.pnlCalendar = new System.Windows.Forms.Panel();
            this.cal = new System.Windows.Forms.MonthCalendar();
            this.label9 = new System.Windows.Forms.Label();
            this.lblInvNo = new System.Windows.Forms.Label();
            this.mskEndDate = new System.Windows.Forms.MaskedTextBox();
            this.mskStartDate = new System.Windows.Forms.MaskedTextBox();
            this.dgvOldNSDetails = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.txtTotInvAmt = new GISControls.TextBoxChar();
            this.lblTotInvAmt = new System.Windows.Forms.Label();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.txtTotInvCount = new GISControls.TextBoxChar();
            this.lblTotalInvoices = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.dgvInvMaster = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectangleShape3 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.bsInvDetails = new System.Windows.Forms.BindingSource(this.components);
            this.bsInvPaySched = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldInvDetails)).BeginInit();
            this.pnlInvDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvDetails)).BeginInit();
            this.pnlCalendar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldNSDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInvDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInvPaySched)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.btnExportNS);
            this.pnlRecord.Controls.Add(this.dgvOldInvDetails);
            this.pnlRecord.Controls.Add(this.btnExportOld);
            this.pnlRecord.Controls.Add(this.pnlInvDetails);
            this.pnlRecord.Controls.Add(this.pnlCalendar);
            this.pnlRecord.Controls.Add(this.lblInvNo);
            this.pnlRecord.Controls.Add(this.mskEndDate);
            this.pnlRecord.Controls.Add(this.mskStartDate);
            this.pnlRecord.Controls.Add(this.dgvOldNSDetails);
            this.pnlRecord.Controls.Add(this.label1);
            this.pnlRecord.Controls.Add(this.btnLoad);
            this.pnlRecord.Controls.Add(this.txtTotInvAmt);
            this.pnlRecord.Controls.Add(this.lblTotInvAmt);
            this.pnlRecord.Controls.Add(this.lblEndDate);
            this.pnlRecord.Controls.Add(this.txtTotInvCount);
            this.pnlRecord.Controls.Add(this.lblTotalInvoices);
            this.pnlRecord.Controls.Add(this.btnExport);
            this.pnlRecord.Controls.Add(this.dgvInvMaster);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.lblStartDate);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.shapeContainer1);
            this.pnlRecord.Location = new System.Drawing.Point(10, 75);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(896, 619);
            this.pnlRecord.TabIndex = 106;
            // 
            // btnExportNS
            // 
            this.btnExportNS.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportNS.ForeColor = System.Drawing.Color.Black;
            this.btnExportNS.Location = new System.Drawing.Point(520, 570);
            this.btnExportNS.Name = "btnExportNS";
            this.btnExportNS.Size = new System.Drawing.Size(111, 35);
            this.btnExportNS.TabIndex = 292;
            this.btnExportNS.Text = "NS Export";
            this.btnExportNS.UseVisualStyleBackColor = true;
            this.btnExportNS.Visible = false;
            this.btnExportNS.Click += new System.EventHandler(this.btnExportNS_Click);
            // 
            // dgvOldInvDetails
            // 
            this.dgvOldInvDetails.AllowUserToAddRows = false;
            this.dgvOldInvDetails.AllowUserToDeleteRows = false;
            this.dgvOldInvDetails.AllowUserToResizeColumns = false;
            this.dgvOldInvDetails.BackgroundColor = System.Drawing.Color.White;
            this.dgvOldInvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOldInvDetails.Location = new System.Drawing.Point(24, 34);
            this.dgvOldInvDetails.Name = "dgvOldInvDetails";
            this.dgvOldInvDetails.ReadOnly = true;
            this.dgvOldInvDetails.Size = new System.Drawing.Size(83, 35);
            this.dgvOldInvDetails.TabIndex = 291;
            this.dgvOldInvDetails.Visible = false;
            // 
            // btnExportOld
            // 
            this.btnExportOld.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportOld.ForeColor = System.Drawing.Color.Black;
            this.btnExportOld.Location = new System.Drawing.Point(636, 570);
            this.btnExportOld.Name = "btnExportOld";
            this.btnExportOld.Size = new System.Drawing.Size(111, 35);
            this.btnExportOld.TabIndex = 290;
            this.btnExportOld.Text = "Old GIS Export";
            this.btnExportOld.UseVisualStyleBackColor = true;
            this.btnExportOld.Visible = false;
            this.btnExportOld.Click += new System.EventHandler(this.btnExportOld_Click);
            // 
            // pnlInvDetails
            // 
            this.pnlInvDetails.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlInvDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInvDetails.Controls.Add(this.lblDetPONo);
            this.pnlInvDetails.Controls.Add(this.txtDetPONo);
            this.pnlInvDetails.Controls.Add(this.txtDetInvDate);
            this.pnlInvDetails.Controls.Add(this.lblDetInvDate);
            this.pnlInvDetails.Controls.Add(this.txtDetInvTotal);
            this.pnlInvDetails.Controls.Add(this.lblDetInvTotal);
            this.pnlInvDetails.Controls.Add(this.txtDetInvNo);
            this.pnlInvDetails.Controls.Add(this.lblDetInvNo);
            this.pnlInvDetails.Controls.Add(this.btnCloseDetails);
            this.pnlInvDetails.Controls.Add(this.lblInvDetails);
            this.pnlInvDetails.Controls.Add(this.dgvInvDetails);
            this.pnlInvDetails.Controls.Add(this.shapeContainer2);
            this.pnlInvDetails.Location = new System.Drawing.Point(286, 135);
            this.pnlInvDetails.Name = "pnlInvDetails";
            this.pnlInvDetails.Size = new System.Drawing.Size(555, 429);
            this.pnlInvDetails.TabIndex = 107;
            this.pnlInvDetails.Visible = false;
            // 
            // lblDetPONo
            // 
            this.lblDetPONo.BackColor = System.Drawing.Color.Transparent;
            this.lblDetPONo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetPONo.ForeColor = System.Drawing.Color.Black;
            this.lblDetPONo.Location = new System.Drawing.Point(32, 53);
            this.lblDetPONo.Name = "lblDetPONo";
            this.lblDetPONo.Padding = new System.Windows.Forms.Padding(2);
            this.lblDetPONo.Size = new System.Drawing.Size(59, 25);
            this.lblDetPONo.TabIndex = 421;
            this.lblDetPONo.Text = "PO No:";
            this.lblDetPONo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDetPONo
            // 
            this.txtDetPONo.BackColor = System.Drawing.Color.White;
            this.txtDetPONo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetPONo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetPONo.ForeColor = System.Drawing.Color.Red;
            this.txtDetPONo.Location = new System.Drawing.Point(34, 81);
            this.txtDetPONo.MaxLength = 5;
            this.txtDetPONo.Name = "txtDetPONo";
            this.txtDetPONo.ReadOnly = true;
            this.txtDetPONo.Size = new System.Drawing.Size(230, 21);
            this.txtDetPONo.TabIndex = 420;
            // 
            // txtDetInvDate
            // 
            this.txtDetInvDate.BackColor = System.Drawing.Color.White;
            this.txtDetInvDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetInvDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetInvDate.ForeColor = System.Drawing.Color.Red;
            this.txtDetInvDate.Location = new System.Drawing.Point(345, 81);
            this.txtDetInvDate.MaxLength = 5;
            this.txtDetInvDate.Name = "txtDetInvDate";
            this.txtDetInvDate.ReadOnly = true;
            this.txtDetInvDate.Size = new System.Drawing.Size(74, 21);
            this.txtDetInvDate.TabIndex = 419;
            // 
            // lblDetInvDate
            // 
            this.lblDetInvDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDetInvDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetInvDate.ForeColor = System.Drawing.Color.Black;
            this.lblDetInvDate.Location = new System.Drawing.Point(342, 53);
            this.lblDetInvDate.Name = "lblDetInvDate";
            this.lblDetInvDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblDetInvDate.Size = new System.Drawing.Size(68, 25);
            this.lblDetInvDate.TabIndex = 418;
            this.lblDetInvDate.Text = "Inv Date:";
            this.lblDetInvDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDetInvTotal
            // 
            this.txtDetInvTotal.BackColor = System.Drawing.Color.White;
            this.txtDetInvTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetInvTotal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetInvTotal.ForeColor = System.Drawing.Color.Red;
            this.txtDetInvTotal.Location = new System.Drawing.Point(433, 81);
            this.txtDetInvTotal.MaxLength = 5;
            this.txtDetInvTotal.Name = "txtDetInvTotal";
            this.txtDetInvTotal.ReadOnly = true;
            this.txtDetInvTotal.Size = new System.Drawing.Size(86, 21);
            this.txtDetInvTotal.TabIndex = 417;
            this.txtDetInvTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDetInvTotal
            // 
            this.lblDetInvTotal.BackColor = System.Drawing.Color.Transparent;
            this.lblDetInvTotal.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetInvTotal.ForeColor = System.Drawing.Color.Black;
            this.lblDetInvTotal.Location = new System.Drawing.Point(428, 53);
            this.lblDetInvTotal.Name = "lblDetInvTotal";
            this.lblDetInvTotal.Padding = new System.Windows.Forms.Padding(2);
            this.lblDetInvTotal.Size = new System.Drawing.Size(71, 25);
            this.lblDetInvTotal.TabIndex = 416;
            this.lblDetInvTotal.Text = "Inv Total:";
            this.lblDetInvTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDetInvNo
            // 
            this.txtDetInvNo.BackColor = System.Drawing.Color.White;
            this.txtDetInvNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDetInvNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDetInvNo.ForeColor = System.Drawing.Color.Red;
            this.txtDetInvNo.Location = new System.Drawing.Point(272, 81);
            this.txtDetInvNo.MaxLength = 5;
            this.txtDetInvNo.Name = "txtDetInvNo";
            this.txtDetInvNo.ReadOnly = true;
            this.txtDetInvNo.Size = new System.Drawing.Size(62, 21);
            this.txtDetInvNo.TabIndex = 415;
            // 
            // lblDetInvNo
            // 
            this.lblDetInvNo.BackColor = System.Drawing.Color.Transparent;
            this.lblDetInvNo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetInvNo.ForeColor = System.Drawing.Color.Black;
            this.lblDetInvNo.Location = new System.Drawing.Point(267, 53);
            this.lblDetInvNo.Name = "lblDetInvNo";
            this.lblDetInvNo.Padding = new System.Windows.Forms.Padding(2);
            this.lblDetInvNo.Size = new System.Drawing.Size(59, 25);
            this.lblDetInvNo.TabIndex = 414;
            this.lblDetInvNo.Text = "Inv No:";
            this.lblDetInvNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCloseDetails
            // 
            this.btnCloseDetails.BackColor = System.Drawing.Color.Firebrick;
            this.btnCloseDetails.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCloseDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseDetails.ForeColor = System.Drawing.Color.White;
            this.btnCloseDetails.Location = new System.Drawing.Point(520, -1);
            this.btnCloseDetails.Name = "btnCloseDetails";
            this.btnCloseDetails.Size = new System.Drawing.Size(34, 34);
            this.btnCloseDetails.TabIndex = 413;
            this.btnCloseDetails.Text = " [X]";
            this.btnCloseDetails.UseVisualStyleBackColor = false;
            this.btnCloseDetails.Click += new System.EventHandler(this.btnCloseDetails_Click);
            // 
            // lblInvDetails
            // 
            this.lblInvDetails.BackColor = System.Drawing.Color.SteelBlue;
            this.lblInvDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvDetails.ForeColor = System.Drawing.Color.White;
            this.lblInvDetails.Location = new System.Drawing.Point(-3, 0);
            this.lblInvDetails.Name = "lblInvDetails";
            this.lblInvDetails.Size = new System.Drawing.Size(557, 33);
            this.lblInvDetails.TabIndex = 0;
            this.lblInvDetails.Text = "INVOICE DETAILS";
            this.lblInvDetails.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvInvDetails
            // 
            this.dgvInvDetails.AllowUserToAddRows = false;
            this.dgvInvDetails.AllowUserToDeleteRows = false;
            this.dgvInvDetails.AllowUserToResizeColumns = false;
            this.dgvInvDetails.BackgroundColor = System.Drawing.Color.White;
            this.dgvInvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvDetails.Location = new System.Drawing.Point(22, 124);
            this.dgvInvDetails.Name = "dgvInvDetails";
            this.dgvInvDetails.ReadOnly = true;
            this.dgvInvDetails.Size = new System.Drawing.Size(510, 280);
            this.dgvInvDetails.TabIndex = 197;
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape2});
            this.shapeContainer2.Size = new System.Drawing.Size(553, 427);
            this.shapeContainer2.TabIndex = 422;
            this.shapeContainer2.TabStop = false;
            // 
            // rectangleShape2
            // 
            this.rectangleShape2.Location = new System.Drawing.Point(22, 48);
            this.rectangleShape2.Name = "rectangleShape2";
            this.rectangleShape2.Size = new System.Drawing.Size(509, 66);
            // 
            // pnlCalendar
            // 
            this.pnlCalendar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCalendar.Controls.Add(this.cal);
            this.pnlCalendar.Controls.Add(this.label9);
            this.pnlCalendar.Location = new System.Drawing.Point(421, 53);
            this.pnlCalendar.Name = "pnlCalendar";
            this.pnlCalendar.Size = new System.Drawing.Size(246, 184);
            this.pnlCalendar.TabIndex = 279;
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
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(27, 128);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 21);
            this.label9.TabIndex = 152;
            this.label9.Text = "PO No.";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label9.Visible = false;
            // 
            // lblInvNo
            // 
            this.lblInvNo.BackColor = System.Drawing.Color.Transparent;
            this.lblInvNo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvNo.ForeColor = System.Drawing.Color.Black;
            this.lblInvNo.Location = new System.Drawing.Point(766, 72);
            this.lblInvNo.Name = "lblInvNo";
            this.lblInvNo.Padding = new System.Windows.Forms.Padding(2);
            this.lblInvNo.Size = new System.Drawing.Size(96, 26);
            this.lblInvNo.TabIndex = 289;
            this.lblInvNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mskEndDate
            // 
            this.mskEndDate.Location = new System.Drawing.Point(223, 78);
            this.mskEndDate.Mask = "00/00/0000";
            this.mskEndDate.Name = "mskEndDate";
            this.mskEndDate.RejectInputOnFirstFailure = true;
            this.mskEndDate.Size = new System.Drawing.Size(70, 21);
            this.mskEndDate.TabIndex = 280;
            this.mskEndDate.ValidatingType = typeof(System.DateTime);
            this.mskEndDate.DoubleClick += new System.EventHandler(this.mskEndDate_DoubleClick);
            // 
            // mskStartDate
            // 
            this.mskStartDate.Location = new System.Drawing.Point(223, 51);
            this.mskStartDate.Mask = "00/00/0000";
            this.mskStartDate.Name = "mskStartDate";
            this.mskStartDate.RejectInputOnFirstFailure = true;
            this.mskStartDate.Size = new System.Drawing.Size(70, 21);
            this.mskStartDate.TabIndex = 200;
            this.mskStartDate.ValidatingType = typeof(System.DateTime);
            this.mskStartDate.DoubleClick += new System.EventHandler(this.mskStartDate_DoubleClick);
            // 
            // dgvOldNSDetails
            // 
            this.dgvOldNSDetails.AllowUserToAddRows = false;
            this.dgvOldNSDetails.AllowUserToDeleteRows = false;
            this.dgvOldNSDetails.AllowUserToResizeColumns = false;
            this.dgvOldNSDetails.BackgroundColor = System.Drawing.Color.White;
            this.dgvOldNSDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOldNSDetails.Location = new System.Drawing.Point(24, 75);
            this.dgvOldNSDetails.Name = "dgvOldNSDetails";
            this.dgvOldNSDetails.ReadOnly = true;
            this.dgvOldNSDetails.Size = new System.Drawing.Size(83, 31);
            this.dgvOldNSDetails.TabIndex = 198;
            this.dgvOldNSDetails.Visible = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(465, 29);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(128, 24);
            this.label1.TabIndex = 194;
            this.label1.Text = "Export Summary:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.ForeColor = System.Drawing.Color.Black;
            this.btnLoad.Location = new System.Drawing.Point(318, 57);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(64, 41);
            this.btnLoad.TabIndex = 193;
            this.btnLoad.Text = "Load Invoices";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // txtTotInvAmt
            // 
            this.txtTotInvAmt.BackColor = System.Drawing.Color.White;
            this.txtTotInvAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotInvAmt.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotInvAmt.ForeColor = System.Drawing.Color.Red;
            this.txtTotInvAmt.Location = new System.Drawing.Point(637, 82);
            this.txtTotInvAmt.MaxLength = 5;
            this.txtTotInvAmt.Name = "txtTotInvAmt";
            this.txtTotInvAmt.ReadOnly = true;
            this.txtTotInvAmt.Size = new System.Drawing.Size(86, 21);
            this.txtTotInvAmt.TabIndex = 190;
            this.txtTotInvAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotInvAmt
            // 
            this.lblTotInvAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblTotInvAmt.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotInvAmt.ForeColor = System.Drawing.Color.Black;
            this.lblTotInvAmt.Location = new System.Drawing.Point(465, 76);
            this.lblTotInvAmt.Name = "lblTotInvAmt";
            this.lblTotInvAmt.Padding = new System.Windows.Forms.Padding(2);
            this.lblTotInvAmt.Size = new System.Drawing.Size(163, 30);
            this.lblTotInvAmt.TabIndex = 189;
            this.lblTotInvAmt.Text = "Total Invoice Amount:";
            this.lblTotInvAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEndDate
            // 
            this.lblEndDate.BackColor = System.Drawing.Color.Transparent;
            this.lblEndDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndDate.ForeColor = System.Drawing.Color.Black;
            this.lblEndDate.Location = new System.Drawing.Point(134, 76);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblEndDate.Size = new System.Drawing.Size(83, 25);
            this.lblEndDate.TabIndex = 187;
            this.lblEndDate.Text = "End Date:";
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTotInvCount
            // 
            this.txtTotInvCount.BackColor = System.Drawing.Color.White;
            this.txtTotInvCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotInvCount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotInvCount.ForeColor = System.Drawing.Color.Red;
            this.txtTotInvCount.Location = new System.Drawing.Point(637, 57);
            this.txtTotInvCount.MaxLength = 5;
            this.txtTotInvCount.Name = "txtTotInvCount";
            this.txtTotInvCount.ReadOnly = true;
            this.txtTotInvCount.Size = new System.Drawing.Size(45, 21);
            this.txtTotInvCount.TabIndex = 184;
            this.txtTotInvCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotalInvoices
            // 
            this.lblTotalInvoices.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalInvoices.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalInvoices.ForeColor = System.Drawing.Color.Black;
            this.lblTotalInvoices.Location = new System.Drawing.Point(481, 53);
            this.lblTotalInvoices.Name = "lblTotalInvoices";
            this.lblTotalInvoices.Padding = new System.Windows.Forms.Padding(2);
            this.lblTotalInvoices.Size = new System.Drawing.Size(147, 26);
            this.lblTotalInvoices.TabIndex = 15;
            this.lblTotalInvoices.Text = "Total Invoice Count:";
            this.lblTotalInvoices.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.Black;
            this.btnExport.Location = new System.Drawing.Point(751, 570);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(111, 35);
            this.btnExport.TabIndex = 11;
            this.btnExport.Text = "Export File";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dgvInvMaster
            // 
            this.dgvInvMaster.AllowUserToAddRows = false;
            this.dgvInvMaster.AllowUserToDeleteRows = false;
            this.dgvInvMaster.AllowUserToResizeColumns = false;
            this.dgvInvMaster.BackgroundColor = System.Drawing.Color.White;
            this.dgvInvMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInvMaster.Location = new System.Drawing.Point(34, 135);
            this.dgvInvMaster.Name = "dgvInvMaster";
            this.dgvInvMaster.ReadOnly = true;
            this.dgvInvMaster.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInvMaster.Size = new System.Drawing.Size(826, 429);
            this.dgvInvMaster.TabIndex = 8;
            this.dgvInvMaster.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInvMaster_CellDoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(818, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblStartDate
            // 
            this.lblStartDate.BackColor = System.Drawing.Color.Transparent;
            this.lblStartDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.ForeColor = System.Drawing.Color.Black;
            this.lblStartDate.Location = new System.Drawing.Point(134, 51);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblStartDate.Size = new System.Drawing.Size(83, 25);
            this.lblStartDate.TabIndex = 3;
            this.lblStartDate.Text = "Start Date:";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(898, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "INVOICE EXPORT";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape1,
            this.rectangleShape3});
            this.shapeContainer1.Size = new System.Drawing.Size(894, 617);
            this.shapeContainer1.TabIndex = 9;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.Location = new System.Drawing.Point(457, 40);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(297, 69);
            // 
            // rectangleShape3
            // 
            this.rectangleShape3.Location = new System.Drawing.Point(121, 39);
            this.rectangleShape3.Name = "rectangleShape3";
            this.rectangleShape3.Size = new System.Drawing.Size(286, 68);
            // 
            // InvoiceExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1596, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "InvoiceExport";
            this.Tag = "InvoiceExport";
            this.Text = "Invoice Export";
            this.Activated += new System.EventHandler(this.InvoiceExport_Activated);
            this.Load += new System.EventHandler(this.InvoiceExport_Load);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldInvDetails)).EndInit();
            this.pnlInvDetails.ResumeLayout(false);
            this.pnlInvDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvDetails)).EndInit();
            this.pnlCalendar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldNSDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInvMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInvDetails)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInvPaySched)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private GISControls.TextBoxChar txtTotInvCount;
        private System.Windows.Forms.Label lblTotalInvoices;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridView dgvInvMaster;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblHeader;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape3;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblTotInvAmt;
        private GISControls.TextBoxChar txtTotInvAmt;
        private System.Windows.Forms.Button btnLoad;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bsInvDetails;
        private System.Windows.Forms.BindingSource bsInvPaySched;
        private System.Windows.Forms.DataGridView dgvOldNSDetails;
        private System.Windows.Forms.MaskedTextBox mskStartDate;
        private System.Windows.Forms.Panel pnlCalendar;
        private System.Windows.Forms.MonthCalendar cal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.MaskedTextBox mskEndDate;
        private System.Windows.Forms.Label lblInvNo;
        private System.Windows.Forms.Panel pnlInvDetails;
        private System.Windows.Forms.Label lblDetPONo;
        private GISControls.TextBoxChar txtDetPONo;
        private GISControls.TextBoxChar txtDetInvDate;
        private System.Windows.Forms.Label lblDetInvDate;
        private GISControls.TextBoxChar txtDetInvTotal;
        private System.Windows.Forms.Label lblDetInvTotal;
        private GISControls.TextBoxChar txtDetInvNo;
        private System.Windows.Forms.Label lblDetInvNo;
        private System.Windows.Forms.Button btnCloseDetails;
        private System.Windows.Forms.Label lblInvDetails;
        private System.Windows.Forms.DataGridView dgvInvDetails;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape2;
        private System.Windows.Forms.Button btnExportOld;
        private System.Windows.Forms.DataGridView dgvOldInvDetails;
        private System.Windows.Forms.Button btnExportNS;
    }
}
