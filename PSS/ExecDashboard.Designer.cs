namespace PSS
{
    partial class ExecDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecDashboard));
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.pnlCalendar = new System.Windows.Forms.Panel();
            this.cal = new System.Windows.Forms.MonthCalendar();
            this.label9 = new System.Windows.Forms.Label();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.pnlCalendar2 = new System.Windows.Forms.Panel();
            this.cal2 = new System.Windows.Forms.MonthCalendar();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.txtLastEntry = new GISControls.TextBoxChar();
            this.lblLastEntry = new System.Windows.Forms.Label();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.lblAvg = new System.Windows.Forms.Label();
            this.lblSum = new System.Windows.Forms.Label();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.pnlGenEntry = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblDayName = new System.Windows.Forms.Label();
            this.bnGenEntry = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.mskEntryDate = new System.Windows.Forms.MaskedTextBox();
            this.lblNew = new System.Windows.Forms.Label();
            this.txtPayTaxes = new GISControls.TextBoxChar();
            this.lblPayTaxes = new System.Windows.Forms.Label();
            this.lblPayDep = new System.Windows.Forms.Label();
            this.lblNewInvoice = new System.Windows.Forms.Label();
            this.lblChkBal2 = new System.Windows.Forms.Label();
            this.lblCashReceipts2 = new System.Windows.Forms.Label();
            this.txtNewInvoice = new GISControls.TextBoxChar();
            this.txtChkBal2 = new GISControls.TextBoxChar();
            this.txtCashReceipts2 = new GISControls.TextBoxChar();
            this.txtChkBal1 = new GISControls.TextBoxChar();
            this.txt401K = new GISControls.TextBoxChar();
            this.lblDate = new System.Windows.Forms.Label();
            this.txtCashReceipts1 = new GISControls.TextBoxChar();
            this.txtPayDeposit = new GISControls.TextBoxChar();
            this.lblAdjNewInv = new System.Windows.Forms.Label();
            this.txtNewPay = new GISControls.TextBoxChar();
            this.lbl401K = new System.Windows.Forms.Label();
            this.txtAdjNewInv = new GISControls.TextBoxChar();
            this.lblNewPay = new System.Windows.Forms.Label();
            this.btnCloseDetails = new System.Windows.Forms.Button();
            this.lblGenEntry = new System.Windows.Forms.Label();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape2 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.dgvDashboard = new System.Windows.Forms.DataGridView();
            this.textBoxChar1 = new GISControls.TextBoxChar();
            this.txtYear = new GISControls.TextBoxChar();
            this.mskStartDate = new System.Windows.Forms.MaskedTextBox();
            this.mskEndDate = new System.Windows.Forms.MaskedTextBox();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.btnLoad2 = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape5 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectangleShape3 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.dgvSummary = new System.Windows.Forms.DataGridView();
            this.bsGenEntry = new System.Windows.Forms.BindingSource(this.components);
            this.bsSummary = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            this.pnlCalendar.SuspendLayout();
            this.pnlCalendar2.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.pnlGenEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bnGenEntry)).BeginInit();
            this.bnGenEntry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDashboard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGenEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSummary)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.pnlCalendar);
            this.pnlRecord.Controls.Add(this.cboMonth);
            this.pnlRecord.Controls.Add(this.cboYear);
            this.pnlRecord.Controls.Add(this.btnLoad);
            this.pnlRecord.Controls.Add(this.lblMonth);
            this.pnlRecord.Controls.Add(this.lblYear);
            this.pnlRecord.Controls.Add(this.pnlCalendar2);
            this.pnlRecord.Controls.Add(this.btnCreate);
            this.pnlRecord.Controls.Add(this.txtLastEntry);
            this.pnlRecord.Controls.Add(this.lblLastEntry);
            this.pnlRecord.Controls.Add(this.lblMax);
            this.pnlRecord.Controls.Add(this.lblMin);
            this.pnlRecord.Controls.Add(this.lblAvg);
            this.pnlRecord.Controls.Add(this.lblSum);
            this.pnlRecord.Controls.Add(this.pnlDetails);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.shapeContainer1);
            this.pnlRecord.Controls.Add(this.dgvSummary);
            this.pnlRecord.Location = new System.Drawing.Point(12, 87);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(864, 654);
            this.pnlRecord.TabIndex = 106;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // pnlCalendar
            // 
            this.pnlCalendar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCalendar.Controls.Add(this.cal);
            this.pnlCalendar.Controls.Add(this.label9);
            this.pnlCalendar.Location = new System.Drawing.Point(153, 6);
            this.pnlCalendar.Name = "pnlCalendar";
            this.pnlCalendar.Size = new System.Drawing.Size(246, 184);
            this.pnlCalendar.TabIndex = 286;
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
            // cboMonth
            // 
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Items.AddRange(new object[] {
            " 1",
            " 2",
            " 3",
            " 4",
            " 5",
            " 6",
            " 7",
            " 8",
            " 9",
            "10",
            "11",
            "12"});
            this.cboMonth.Location = new System.Drawing.Point(651, 614);
            this.cboMonth.MaxLength = 2;
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(45, 23);
            this.cboMonth.TabIndex = 443;
            this.cboMonth.SelectedIndexChanged += new System.EventHandler(this.cboMonth_SelectedIndexChanged);
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(521, 614);
            this.cboYear.MaxLength = 4;
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(62, 23);
            this.cboYear.TabIndex = 442;
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.ForeColor = System.Drawing.Color.Red;
            this.btnLoad.Location = new System.Drawing.Point(756, 608);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(88, 33);
            this.btnLoad.TabIndex = 441;
            this.btnLoad.Text = "Load ";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblMonth
            // 
            this.lblMonth.BackColor = System.Drawing.Color.Transparent;
            this.lblMonth.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMonth.ForeColor = System.Drawing.Color.Black;
            this.lblMonth.Location = new System.Drawing.Point(589, 613);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Padding = new System.Windows.Forms.Padding(2);
            this.lblMonth.Size = new System.Drawing.Size(56, 25);
            this.lblMonth.TabIndex = 435;
            this.lblMonth.Text = "Month:";
            this.lblMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblYear
            // 
            this.lblYear.BackColor = System.Drawing.Color.Transparent;
            this.lblYear.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYear.ForeColor = System.Drawing.Color.Black;
            this.lblYear.Location = new System.Drawing.Point(472, 613);
            this.lblYear.Name = "lblYear";
            this.lblYear.Padding = new System.Windows.Forms.Padding(2);
            this.lblYear.Size = new System.Drawing.Size(43, 25);
            this.lblYear.TabIndex = 433;
            this.lblYear.Text = "Year:";
            this.lblYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlCalendar2
            // 
            this.pnlCalendar2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCalendar2.Controls.Add(this.cal2);
            this.pnlCalendar2.Controls.Add(this.label1);
            this.pnlCalendar2.Location = new System.Drawing.Point(15, 360);
            this.pnlCalendar2.Name = "pnlCalendar2";
            this.pnlCalendar2.Size = new System.Drawing.Size(246, 184);
            this.pnlCalendar2.TabIndex = 287;
            this.pnlCalendar2.Visible = false;
            // 
            // cal2
            // 
            this.cal2.Location = new System.Drawing.Point(9, 9);
            this.cal2.Name = "cal2";
            this.cal2.TabIndex = 277;
            this.cal2.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.cal2_DateSelected);
            this.cal2.MouseLeave += new System.EventHandler(this.cal2_MouseLeave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(27, 128);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 21);
            this.label1.TabIndex = 152;
            this.label1.Text = "PO No.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Visible = false;
            // 
            // btnCreate
            // 
            this.btnCreate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.ForeColor = System.Drawing.Color.Black;
            this.btnCreate.Location = new System.Drawing.Point(829, 518);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(33, 26);
            this.btnCreate.TabIndex = 432;
            this.btnCreate.Text = "Create Days";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Visible = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // txtLastEntry
            // 
            this.txtLastEntry.BackColor = System.Drawing.Color.White;
            this.txtLastEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLastEntry.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastEntry.ForeColor = System.Drawing.Color.Black;
            this.txtLastEntry.Location = new System.Drawing.Point(99, 617);
            this.txtLastEntry.MaxLength = 12;
            this.txtLastEntry.Name = "txtLastEntry";
            this.txtLastEntry.ReadOnly = true;
            this.txtLastEntry.Size = new System.Drawing.Size(75, 21);
            this.txtLastEntry.TabIndex = 431;
            this.txtLastEntry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLastEntry
            // 
            this.lblLastEntry.BackColor = System.Drawing.Color.Transparent;
            this.lblLastEntry.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastEntry.ForeColor = System.Drawing.Color.Red;
            this.lblLastEntry.Location = new System.Drawing.Point(15, 608);
            this.lblLastEntry.Name = "lblLastEntry";
            this.lblLastEntry.Padding = new System.Windows.Forms.Padding(2);
            this.lblLastEntry.Size = new System.Drawing.Size(78, 36);
            this.lblLastEntry.TabIndex = 430;
            this.lblLastEntry.Text = "Last Entry Date:";
            this.lblLastEntry.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMax
            // 
            this.lblMax.AutoSize = true;
            this.lblMax.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMax.ForeColor = System.Drawing.Color.Red;
            this.lblMax.Location = new System.Drawing.Point(62, 584);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(32, 14);
            this.lblMax.TabIndex = 294;
            this.lblMax.Text = "Max:";
            // 
            // lblMin
            // 
            this.lblMin.AutoSize = true;
            this.lblMin.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMin.ForeColor = System.Drawing.Color.Red;
            this.lblMin.Location = new System.Drawing.Point(62, 570);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(30, 14);
            this.lblMin.TabIndex = 293;
            this.lblMin.Text = "Min:";
            // 
            // lblAvg
            // 
            this.lblAvg.AutoSize = true;
            this.lblAvg.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvg.ForeColor = System.Drawing.Color.Red;
            this.lblAvg.Location = new System.Drawing.Point(62, 556);
            this.lblAvg.Name = "lblAvg";
            this.lblAvg.Size = new System.Drawing.Size(31, 14);
            this.lblAvg.TabIndex = 292;
            this.lblAvg.Text = "Avg:";
            // 
            // lblSum
            // 
            this.lblSum.AutoSize = true;
            this.lblSum.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSum.ForeColor = System.Drawing.Color.Red;
            this.lblSum.Location = new System.Drawing.Point(62, 542);
            this.lblSum.Name = "lblSum";
            this.lblSum.Size = new System.Drawing.Size(35, 14);
            this.lblSum.TabIndex = 291;
            this.lblSum.Text = "Sum:";
            // 
            // pnlDetails
            // 
            this.pnlDetails.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Controls.Add(this.pnlGenEntry);
            this.pnlDetails.Controls.Add(this.dgvDashboard);
            this.pnlDetails.Controls.Add(this.textBoxChar1);
            this.pnlDetails.Controls.Add(this.txtYear);
            this.pnlDetails.Controls.Add(this.mskStartDate);
            this.pnlDetails.Controls.Add(this.mskEndDate);
            this.pnlDetails.Controls.Add(this.lblEndDate);
            this.pnlDetails.Controls.Add(this.lblStartDate);
            this.pnlDetails.Controls.Add(this.btnLoad2);
            this.pnlDetails.Location = new System.Drawing.Point(15, 27);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(829, 502);
            this.pnlDetails.TabIndex = 289;
            this.pnlDetails.Visible = false;
            // 
            // pnlGenEntry
            // 
            this.pnlGenEntry.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlGenEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGenEntry.Controls.Add(this.btnClose);
            this.pnlGenEntry.Controls.Add(this.lblDayName);
            this.pnlGenEntry.Controls.Add(this.bnGenEntry);
            this.pnlGenEntry.Controls.Add(this.mskEntryDate);
            this.pnlGenEntry.Controls.Add(this.lblNew);
            this.pnlGenEntry.Controls.Add(this.txtPayTaxes);
            this.pnlGenEntry.Controls.Add(this.lblPayTaxes);
            this.pnlGenEntry.Controls.Add(this.lblPayDep);
            this.pnlGenEntry.Controls.Add(this.lblNewInvoice);
            this.pnlGenEntry.Controls.Add(this.lblChkBal2);
            this.pnlGenEntry.Controls.Add(this.lblCashReceipts2);
            this.pnlGenEntry.Controls.Add(this.txtNewInvoice);
            this.pnlGenEntry.Controls.Add(this.txtChkBal2);
            this.pnlGenEntry.Controls.Add(this.txtCashReceipts2);
            this.pnlGenEntry.Controls.Add(this.txtChkBal1);
            this.pnlGenEntry.Controls.Add(this.txt401K);
            this.pnlGenEntry.Controls.Add(this.lblDate);
            this.pnlGenEntry.Controls.Add(this.txtCashReceipts1);
            this.pnlGenEntry.Controls.Add(this.txtPayDeposit);
            this.pnlGenEntry.Controls.Add(this.lblAdjNewInv);
            this.pnlGenEntry.Controls.Add(this.txtNewPay);
            this.pnlGenEntry.Controls.Add(this.lbl401K);
            this.pnlGenEntry.Controls.Add(this.txtAdjNewInv);
            this.pnlGenEntry.Controls.Add(this.lblNewPay);
            this.pnlGenEntry.Controls.Add(this.btnCloseDetails);
            this.pnlGenEntry.Controls.Add(this.lblGenEntry);
            this.pnlGenEntry.Controls.Add(this.shapeContainer2);
            this.pnlGenEntry.Location = new System.Drawing.Point(112, 79);
            this.pnlGenEntry.Name = "pnlGenEntry";
            this.pnlGenEntry.Size = new System.Drawing.Size(594, 263);
            this.pnlGenEntry.TabIndex = 287;
            this.pnlGenEntry.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(516, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblDayName
            // 
            this.lblDayName.BackColor = System.Drawing.Color.Transparent;
            this.lblDayName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDayName.ForeColor = System.Drawing.Color.Black;
            this.lblDayName.Location = new System.Drawing.Point(263, 55);
            this.lblDayName.Name = "lblDayName";
            this.lblDayName.Padding = new System.Windows.Forms.Padding(2);
            this.lblDayName.Size = new System.Drawing.Size(123, 26);
            this.lblDayName.TabIndex = 437;
            this.lblDayName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bnGenEntry
            // 
            this.bnGenEntry.AddNewItem = null;
            this.bnGenEntry.AutoSize = false;
            this.bnGenEntry.BackColor = System.Drawing.Color.AliceBlue;
            this.bnGenEntry.CountItem = this.toolStripLabel2;
            this.bnGenEntry.DeleteItem = null;
            this.bnGenEntry.Dock = System.Windows.Forms.DockStyle.None;
            this.bnGenEntry.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.bnGenEntry.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripSeparator3,
            this.toolStripTextBox2,
            this.toolStripLabel2,
            this.toolStripSeparator4,
            this.toolStripButton7,
            this.toolStripButton8});
            this.bnGenEntry.Location = new System.Drawing.Point(0, 2);
            this.bnGenEntry.MoveFirstItem = this.toolStripButton5;
            this.bnGenEntry.MoveLastItem = this.toolStripButton8;
            this.bnGenEntry.MoveNextItem = this.toolStripButton7;
            this.bnGenEntry.MovePreviousItem = this.toolStripButton6;
            this.bnGenEntry.Name = "bnGenEntry";
            this.bnGenEntry.PositionItem = this.toolStripTextBox2;
            this.bnGenEntry.Size = new System.Drawing.Size(209, 24);
            this.bnGenEntry.TabIndex = 107;
            this.bnGenEntry.Text = "bindingNavigator2";
            this.bnGenEntry.Visible = false;
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(35, 21);
            this.toolStripLabel2.Text = "of {0}";
            this.toolStripLabel2.ToolTipText = "Total number of items";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.RightToLeftAutoMirrorImage = true;
            this.toolStripButton5.Size = new System.Drawing.Size(23, 21);
            this.toolStripButton5.Text = "Move first";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.RightToLeftAutoMirrorImage = true;
            this.toolStripButton6.Size = new System.Drawing.Size(23, 21);
            this.toolStripButton6.Text = "Move previous";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 24);
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.AccessibleName = "Position";
            this.toolStripTextBox2.AutoSize = false;
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size(50, 23);
            this.toolStripTextBox2.Text = "0";
            this.toolStripTextBox2.ToolTipText = "Current position";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 24);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.RightToLeftAutoMirrorImage = true;
            this.toolStripButton7.Size = new System.Drawing.Size(23, 21);
            this.toolStripButton7.Text = "Move next";
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.RightToLeftAutoMirrorImage = true;
            this.toolStripButton8.Size = new System.Drawing.Size(23, 21);
            this.toolStripButton8.Text = "Move last";
            // 
            // mskEntryDate
            // 
            this.mskEntryDate.Location = new System.Drawing.Point(187, 59);
            this.mskEntryDate.Mask = "00/00/0000";
            this.mskEntryDate.Name = "mskEntryDate";
            this.mskEntryDate.RejectInputOnFirstFailure = true;
            this.mskEntryDate.Size = new System.Drawing.Size(70, 21);
            this.mskEntryDate.TabIndex = 436;
            this.mskEntryDate.ValidatingType = typeof(System.DateTime);
            this.mskEntryDate.DoubleClick += new System.EventHandler(this.mskEntryDate_DoubleClick);
            // 
            // lblNew
            // 
            this.lblNew.BackColor = System.Drawing.Color.Transparent;
            this.lblNew.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNew.ForeColor = System.Drawing.Color.Red;
            this.lblNew.Location = new System.Drawing.Point(435, 55);
            this.lblNew.Name = "lblNew";
            this.lblNew.Padding = new System.Windows.Forms.Padding(2);
            this.lblNew.Size = new System.Drawing.Size(91, 25);
            this.lblNew.TabIndex = 422;
            this.lblNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPayTaxes
            // 
            this.txtPayTaxes.BackColor = System.Drawing.Color.White;
            this.txtPayTaxes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPayTaxes.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayTaxes.ForeColor = System.Drawing.Color.Black;
            this.txtPayTaxes.Location = new System.Drawing.Point(434, 125);
            this.txtPayTaxes.MaxLength = 12;
            this.txtPayTaxes.Name = "txtPayTaxes";
            this.txtPayTaxes.ReadOnly = true;
            this.txtPayTaxes.Size = new System.Drawing.Size(105, 21);
            this.txtPayTaxes.TabIndex = 8;
            this.txtPayTaxes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPayTaxes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPayTaxes_KeyPress);
            this.txtPayTaxes.Leave += new System.EventHandler(this.txtPayTaxes_Leave);
            // 
            // lblPayTaxes
            // 
            this.lblPayTaxes.BackColor = System.Drawing.Color.Transparent;
            this.lblPayTaxes.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayTaxes.ForeColor = System.Drawing.Color.Black;
            this.lblPayTaxes.Location = new System.Drawing.Point(303, 125);
            this.lblPayTaxes.Name = "lblPayTaxes";
            this.lblPayTaxes.Padding = new System.Windows.Forms.Padding(2);
            this.lblPayTaxes.Size = new System.Drawing.Size(125, 25);
            this.lblPayTaxes.TabIndex = 435;
            this.lblPayTaxes.Text = "Payroll Taxes:   $";
            this.lblPayTaxes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPayDep
            // 
            this.lblPayDep.BackColor = System.Drawing.Color.Transparent;
            this.lblPayDep.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPayDep.ForeColor = System.Drawing.Color.Black;
            this.lblPayDep.Location = new System.Drawing.Point(293, 152);
            this.lblPayDep.Name = "lblPayDep";
            this.lblPayDep.Padding = new System.Windows.Forms.Padding(2);
            this.lblPayDep.Size = new System.Drawing.Size(135, 25);
            this.lblPayDep.TabIndex = 434;
            this.lblPayDep.Text = "Payroll Deposit:   $";
            this.lblPayDep.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNewInvoice
            // 
            this.lblNewInvoice.BackColor = System.Drawing.Color.Transparent;
            this.lblNewInvoice.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewInvoice.ForeColor = System.Drawing.Color.Black;
            this.lblNewInvoice.Location = new System.Drawing.Point(46, 152);
            this.lblNewInvoice.Name = "lblNewInvoice";
            this.lblNewInvoice.Padding = new System.Windows.Forms.Padding(2);
            this.lblNewInvoice.Size = new System.Drawing.Size(141, 25);
            this.lblNewInvoice.TabIndex = 433;
            this.lblNewInvoice.Text = "New Invoice:   $";
            this.lblNewInvoice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblChkBal2
            // 
            this.lblChkBal2.BackColor = System.Drawing.Color.Transparent;
            this.lblChkBal2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChkBal2.ForeColor = System.Drawing.Color.Black;
            this.lblChkBal2.Location = new System.Drawing.Point(46, 125);
            this.lblChkBal2.Name = "lblChkBal2";
            this.lblChkBal2.Padding = new System.Windows.Forms.Padding(2);
            this.lblChkBal2.Size = new System.Drawing.Size(141, 25);
            this.lblChkBal2.TabIndex = 432;
            this.lblChkBal2.Text = "Check Balance:   $";
            this.lblChkBal2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCashReceipts2
            // 
            this.lblCashReceipts2.BackColor = System.Drawing.Color.Transparent;
            this.lblCashReceipts2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashReceipts2.ForeColor = System.Drawing.Color.Black;
            this.lblCashReceipts2.Location = new System.Drawing.Point(46, 98);
            this.lblCashReceipts2.Name = "lblCashReceipts2";
            this.lblCashReceipts2.Padding = new System.Windows.Forms.Padding(2);
            this.lblCashReceipts2.Size = new System.Drawing.Size(141, 25);
            this.lblCashReceipts2.TabIndex = 431;
            this.lblCashReceipts2.Text = "Cash Receipts:   $";
            this.lblCashReceipts2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNewInvoice
            // 
            this.txtNewInvoice.BackColor = System.Drawing.Color.White;
            this.txtNewInvoice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewInvoice.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewInvoice.ForeColor = System.Drawing.Color.Black;
            this.txtNewInvoice.Location = new System.Drawing.Point(187, 154);
            this.txtNewInvoice.MaxLength = 12;
            this.txtNewInvoice.Name = "txtNewInvoice";
            this.txtNewInvoice.ReadOnly = true;
            this.txtNewInvoice.Size = new System.Drawing.Size(105, 21);
            this.txtNewInvoice.TabIndex = 5;
            this.txtNewInvoice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNewInvoice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewInvoice_KeyPress);
            this.txtNewInvoice.Leave += new System.EventHandler(this.txtNewInvoice_Leave);
            // 
            // txtChkBal2
            // 
            this.txtChkBal2.BackColor = System.Drawing.Color.White;
            this.txtChkBal2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChkBal2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChkBal2.ForeColor = System.Drawing.Color.Black;
            this.txtChkBal2.Location = new System.Drawing.Point(187, 125);
            this.txtChkBal2.MaxLength = 12;
            this.txtChkBal2.Name = "txtChkBal2";
            this.txtChkBal2.ReadOnly = true;
            this.txtChkBal2.Size = new System.Drawing.Size(105, 21);
            this.txtChkBal2.TabIndex = 4;
            this.txtChkBal2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtChkBal2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtChkBal2_KeyPress);
            this.txtChkBal2.Leave += new System.EventHandler(this.txtChkBal2_Leave);
            // 
            // txtCashReceipts2
            // 
            this.txtCashReceipts2.BackColor = System.Drawing.Color.White;
            this.txtCashReceipts2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCashReceipts2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCashReceipts2.ForeColor = System.Drawing.Color.Black;
            this.txtCashReceipts2.Location = new System.Drawing.Point(187, 98);
            this.txtCashReceipts2.MaxLength = 12;
            this.txtCashReceipts2.Name = "txtCashReceipts2";
            this.txtCashReceipts2.ReadOnly = true;
            this.txtCashReceipts2.Size = new System.Drawing.Size(105, 21);
            this.txtCashReceipts2.TabIndex = 3;
            this.txtCashReceipts2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCashReceipts2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCashReceipts2_KeyPress);
            this.txtCashReceipts2.Leave += new System.EventHandler(this.txtCashReceipts2_Leave);
            // 
            // txtChkBal1
            // 
            this.txtChkBal1.BackColor = System.Drawing.Color.White;
            this.txtChkBal1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChkBal1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChkBal1.ForeColor = System.Drawing.Color.Black;
            this.txtChkBal1.Location = new System.Drawing.Point(35, 70);
            this.txtChkBal1.MaxLength = 12;
            this.txtChkBal1.Name = "txtChkBal1";
            this.txtChkBal1.ReadOnly = true;
            this.txtChkBal1.Size = new System.Drawing.Size(22, 21);
            this.txtChkBal1.TabIndex = 2;
            this.txtChkBal1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtChkBal1.Visible = false;
            this.txtChkBal1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtChkBal1_KeyPress);
            // 
            // txt401K
            // 
            this.txt401K.BackColor = System.Drawing.Color.White;
            this.txt401K.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt401K.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt401K.ForeColor = System.Drawing.Color.Black;
            this.txt401K.Location = new System.Drawing.Point(434, 181);
            this.txt401K.MaxLength = 12;
            this.txt401K.Name = "txt401K";
            this.txt401K.ReadOnly = true;
            this.txt401K.Size = new System.Drawing.Size(105, 21);
            this.txt401K.TabIndex = 10;
            this.txt401K.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt401K.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt401K_KeyPress);
            this.txt401K.Leave += new System.EventHandler(this.txt401K_Leave);
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.Transparent;
            this.lblDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.ForeColor = System.Drawing.Color.Red;
            this.lblDate.Location = new System.Drawing.Point(73, 57);
            this.lblDate.Name = "lblDate";
            this.lblDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblDate.Size = new System.Drawing.Size(94, 25);
            this.lblDate.TabIndex = 421;
            this.lblDate.Text = "Entry Date:";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCashReceipts1
            // 
            this.txtCashReceipts1.BackColor = System.Drawing.Color.White;
            this.txtCashReceipts1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCashReceipts1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCashReceipts1.ForeColor = System.Drawing.Color.Black;
            this.txtCashReceipts1.Location = new System.Drawing.Point(35, 46);
            this.txtCashReceipts1.MaxLength = 12;
            this.txtCashReceipts1.Name = "txtCashReceipts1";
            this.txtCashReceipts1.ReadOnly = true;
            this.txtCashReceipts1.Size = new System.Drawing.Size(22, 21);
            this.txtCashReceipts1.TabIndex = 1;
            this.txtCashReceipts1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCashReceipts1.Visible = false;
            this.txtCashReceipts1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCashReceipts1_KeyPress);
            // 
            // txtPayDeposit
            // 
            this.txtPayDeposit.BackColor = System.Drawing.Color.White;
            this.txtPayDeposit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPayDeposit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPayDeposit.ForeColor = System.Drawing.Color.Black;
            this.txtPayDeposit.Location = new System.Drawing.Point(434, 154);
            this.txtPayDeposit.MaxLength = 12;
            this.txtPayDeposit.Name = "txtPayDeposit";
            this.txtPayDeposit.ReadOnly = true;
            this.txtPayDeposit.Size = new System.Drawing.Size(105, 21);
            this.txtPayDeposit.TabIndex = 9;
            this.txtPayDeposit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPayDeposit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPayDeposit_KeyPress);
            this.txtPayDeposit.Leave += new System.EventHandler(this.txtPayDeposit_Leave);
            // 
            // lblAdjNewInv
            // 
            this.lblAdjNewInv.BackColor = System.Drawing.Color.Transparent;
            this.lblAdjNewInv.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdjNewInv.ForeColor = System.Drawing.Color.Black;
            this.lblAdjNewInv.Location = new System.Drawing.Point(62, 179);
            this.lblAdjNewInv.Name = "lblAdjNewInv";
            this.lblAdjNewInv.Padding = new System.Windows.Forms.Padding(2);
            this.lblAdjNewInv.Size = new System.Drawing.Size(125, 25);
            this.lblAdjNewInv.TabIndex = 418;
            this.lblAdjNewInv.Text = "Adj New Inv:   $";
            this.lblAdjNewInv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNewPay
            // 
            this.txtNewPay.BackColor = System.Drawing.Color.White;
            this.txtNewPay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNewPay.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewPay.ForeColor = System.Drawing.Color.Black;
            this.txtNewPay.Location = new System.Drawing.Point(434, 98);
            this.txtNewPay.MaxLength = 12;
            this.txtNewPay.Name = "txtNewPay";
            this.txtNewPay.ReadOnly = true;
            this.txtNewPay.Size = new System.Drawing.Size(105, 21);
            this.txtNewPay.TabIndex = 7;
            this.txtNewPay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNewPay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewPay_KeyPress);
            this.txtNewPay.Leave += new System.EventHandler(this.txtNewPay_Leave);
            // 
            // lbl401K
            // 
            this.lbl401K.BackColor = System.Drawing.Color.Transparent;
            this.lbl401K.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl401K.ForeColor = System.Drawing.Color.Black;
            this.lbl401K.Location = new System.Drawing.Point(303, 179);
            this.lbl401K.Name = "lbl401K";
            this.lbl401K.Padding = new System.Windows.Forms.Padding(2);
            this.lbl401K.Size = new System.Drawing.Size(125, 25);
            this.lbl401K.TabIndex = 416;
            this.lbl401K.Text = "401K:   $";
            this.lbl401K.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAdjNewInv
            // 
            this.txtAdjNewInv.BackColor = System.Drawing.Color.White;
            this.txtAdjNewInv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAdjNewInv.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAdjNewInv.ForeColor = System.Drawing.Color.Black;
            this.txtAdjNewInv.Location = new System.Drawing.Point(187, 181);
            this.txtAdjNewInv.MaxLength = 12;
            this.txtAdjNewInv.Name = "txtAdjNewInv";
            this.txtAdjNewInv.ReadOnly = true;
            this.txtAdjNewInv.Size = new System.Drawing.Size(105, 21);
            this.txtAdjNewInv.TabIndex = 6;
            this.txtAdjNewInv.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAdjNewInv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAdjNewInv_KeyPress);
            this.txtAdjNewInv.Leave += new System.EventHandler(this.txtAdjNewInv_Leave);
            // 
            // lblNewPay
            // 
            this.lblNewPay.BackColor = System.Drawing.Color.Transparent;
            this.lblNewPay.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewPay.ForeColor = System.Drawing.Color.Black;
            this.lblNewPay.Location = new System.Drawing.Point(303, 98);
            this.lblNewPay.Name = "lblNewPay";
            this.lblNewPay.Padding = new System.Windows.Forms.Padding(2);
            this.lblNewPay.Size = new System.Drawing.Size(125, 25);
            this.lblNewPay.TabIndex = 414;
            this.lblNewPay.Text = "New Payables:   $";
            this.lblNewPay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCloseDetails
            // 
            this.btnCloseDetails.BackColor = System.Drawing.Color.Firebrick;
            this.btnCloseDetails.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCloseDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseDetails.ForeColor = System.Drawing.Color.White;
            this.btnCloseDetails.Location = new System.Drawing.Point(476, 0);
            this.btnCloseDetails.Name = "btnCloseDetails";
            this.btnCloseDetails.Size = new System.Drawing.Size(34, 26);
            this.btnCloseDetails.TabIndex = 413;
            this.btnCloseDetails.Text = " [X]";
            this.btnCloseDetails.UseVisualStyleBackColor = false;
            this.btnCloseDetails.Visible = false;
            this.btnCloseDetails.Click += new System.EventHandler(this.btnCloseDetails_Click);
            // 
            // lblGenEntry
            // 
            this.lblGenEntry.BackColor = System.Drawing.Color.SteelBlue;
            this.lblGenEntry.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGenEntry.ForeColor = System.Drawing.Color.White;
            this.lblGenEntry.Location = new System.Drawing.Point(-3, 0);
            this.lblGenEntry.Name = "lblGenEntry";
            this.lblGenEntry.Size = new System.Drawing.Size(595, 21);
            this.lblGenEntry.TabIndex = 0;
            this.lblGenEntry.Text = "General Entry";
            this.lblGenEntry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape2});
            this.shapeContainer2.Size = new System.Drawing.Size(592, 261);
            this.shapeContainer2.TabIndex = 422;
            this.shapeContainer2.TabStop = false;
            // 
            // rectangleShape2
            // 
            this.rectangleShape2.Location = new System.Drawing.Point(24, 43);
            this.rectangleShape2.Name = "rectangleShape2";
            this.rectangleShape2.Size = new System.Drawing.Size(542, 191);
            // 
            // dgvDashboard
            // 
            this.dgvDashboard.AllowUserToAddRows = false;
            this.dgvDashboard.AllowUserToDeleteRows = false;
            this.dgvDashboard.AllowUserToResizeColumns = false;
            this.dgvDashboard.AllowUserToResizeRows = false;
            this.dgvDashboard.BackgroundColor = System.Drawing.Color.White;
            this.dgvDashboard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDashboard.Location = new System.Drawing.Point(-1, -1);
            this.dgvDashboard.Name = "dgvDashboard";
            this.dgvDashboard.ReadOnly = true;
            this.dgvDashboard.RowHeadersVisible = false;
            this.dgvDashboard.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvDashboard.Size = new System.Drawing.Size(829, 505);
            this.dgvDashboard.TabIndex = 291;
            // 
            // textBoxChar1
            // 
            this.textBoxChar1.BackColor = System.Drawing.Color.White;
            this.textBoxChar1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxChar1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxChar1.ForeColor = System.Drawing.Color.Black;
            this.textBoxChar1.Location = new System.Drawing.Point(-13, 348);
            this.textBoxChar1.MaxLength = 4;
            this.textBoxChar1.Name = "textBoxChar1";
            this.textBoxChar1.Size = new System.Drawing.Size(49, 21);
            this.textBoxChar1.TabIndex = 436;
            // 
            // txtYear
            // 
            this.txtYear.BackColor = System.Drawing.Color.White;
            this.txtYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYear.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYear.ForeColor = System.Drawing.Color.Black;
            this.txtYear.Location = new System.Drawing.Point(-13, 321);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(49, 21);
            this.txtYear.TabIndex = 434;
            this.txtYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtYear_KeyPress);
            // 
            // mskStartDate
            // 
            this.mskStartDate.Location = new System.Drawing.Point(773, 294);
            this.mskStartDate.Mask = "00/00/0000";
            this.mskStartDate.Name = "mskStartDate";
            this.mskStartDate.RejectInputOnFirstFailure = true;
            this.mskStartDate.Size = new System.Drawing.Size(70, 21);
            this.mskStartDate.TabIndex = 284;
            this.mskStartDate.ValidatingType = typeof(System.DateTime);
            this.mskStartDate.DoubleClick += new System.EventHandler(this.mskStartDate_DoubleClick);
            // 
            // mskEndDate
            // 
            this.mskEndDate.Location = new System.Drawing.Point(776, 376);
            this.mskEndDate.Mask = "00/00/0000";
            this.mskEndDate.Name = "mskEndDate";
            this.mskEndDate.RejectInputOnFirstFailure = true;
            this.mskEndDate.Size = new System.Drawing.Size(70, 21);
            this.mskEndDate.TabIndex = 285;
            this.mskEndDate.ValidatingType = typeof(System.DateTime);
            this.mskEndDate.DoubleClick += new System.EventHandler(this.mskEndDate_DoubleClick);
            // 
            // lblEndDate
            // 
            this.lblEndDate.BackColor = System.Drawing.Color.Transparent;
            this.lblEndDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndDate.ForeColor = System.Drawing.Color.Black;
            this.lblEndDate.Location = new System.Drawing.Point(793, 421);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblEndDate.Size = new System.Drawing.Size(50, 25);
            this.lblEndDate.TabIndex = 282;
            this.lblEndDate.Text = "End:";
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStartDate
            // 
            this.lblStartDate.BackColor = System.Drawing.Color.Transparent;
            this.lblStartDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.ForeColor = System.Drawing.Color.Black;
            this.lblStartDate.Location = new System.Drawing.Point(799, 248);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblStartDate.Size = new System.Drawing.Size(47, 25);
            this.lblStartDate.TabIndex = 281;
            this.lblStartDate.Text = "Start:";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLoad2
            // 
            this.btnLoad2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad2.ForeColor = System.Drawing.Color.Black;
            this.btnLoad2.Location = new System.Drawing.Point(781, 421);
            this.btnLoad2.Name = "btnLoad2";
            this.btnLoad2.Size = new System.Drawing.Size(66, 33);
            this.btnLoad2.TabIndex = 283;
            this.btnLoad2.Text = "Load ";
            this.btnLoad2.UseVisualStyleBackColor = true;
            this.btnLoad2.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-3, -1);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(865, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "EXECUTIVE DASHBOARD";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseDown);
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape5,
            this.rectangleShape3});
            this.shapeContainer1.Size = new System.Drawing.Size(862, 652);
            this.shapeContainer1.TabIndex = 290;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape5
            // 
            this.rectangleShape5.Location = new System.Drawing.Point(465, 605);
            this.rectangleShape5.Name = "rectangleShape5";
            this.rectangleShape5.Size = new System.Drawing.Size(247, 42);
            // 
            // rectangleShape3
            // 
            this.rectangleShape3.Location = new System.Drawing.Point(49, 537);
            this.rectangleShape3.Name = "rectangleShape3";
            this.rectangleShape3.Size = new System.Drawing.Size(55, 65);
            // 
            // dgvSummary
            // 
            this.dgvSummary.AllowUserToAddRows = false;
            this.dgvSummary.AllowUserToDeleteRows = false;
            this.dgvSummary.AllowUserToResizeColumns = false;
            this.dgvSummary.AllowUserToResizeRows = false;
            this.dgvSummary.BackgroundColor = System.Drawing.Color.White;
            this.dgvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSummary.Location = new System.Drawing.Point(121, 537);
            this.dgvSummary.Name = "dgvSummary";
            this.dgvSummary.ReadOnly = true;
            this.dgvSummary.RowHeadersVisible = false;
            this.dgvSummary.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvSummary.Size = new System.Drawing.Size(723, 63);
            this.dgvSummary.TabIndex = 288;
            // 
            // ExecDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1684, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ExecDashboard";
            this.Tag = "ExecDashboard";
            this.Text = "ExecDashboard";
            this.Load += new System.EventHandler(this.ExecDashboard_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExecDashboard_KeyDown);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            this.pnlCalendar.ResumeLayout(false);
            this.pnlCalendar2.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.pnlGenEntry.ResumeLayout(false);
            this.pnlGenEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bnGenEntry)).EndInit();
            this.bnGenEntry.ResumeLayout(false);
            this.bnGenEntry.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDashboard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGenEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSummary)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.MaskedTextBox mskEndDate;
        private System.Windows.Forms.MaskedTextBox mskStartDate;
        private System.Windows.Forms.Button btnLoad2;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Panel pnlCalendar;
        private System.Windows.Forms.MonthCalendar cal;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel pnlGenEntry;
        private System.Windows.Forms.Label lblPayDep;
        private System.Windows.Forms.Label lblNewInvoice;
        private System.Windows.Forms.Label lblChkBal2;
        private System.Windows.Forms.Label lblCashReceipts2;
        private GISControls.TextBoxChar txtNewInvoice;
        private GISControls.TextBoxChar txtChkBal2;
        private GISControls.TextBoxChar txtCashReceipts2;
        private GISControls.TextBoxChar txtChkBal1;
        private GISControls.TextBoxChar txt401K;
        private System.Windows.Forms.Label lblDate;
        private GISControls.TextBoxChar txtCashReceipts1;
        private GISControls.TextBoxChar txtPayDeposit;
        private System.Windows.Forms.Label lblAdjNewInv;
        private GISControls.TextBoxChar txtNewPay;
        private System.Windows.Forms.Label lbl401K;
        private GISControls.TextBoxChar txtAdjNewInv;
        private System.Windows.Forms.Label lblNewPay;
        private System.Windows.Forms.Button btnCloseDetails;
        private System.Windows.Forms.Label lblGenEntry;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape2;
        private System.Windows.Forms.DataGridView dgvSummary;
        private GISControls.TextBoxChar txtPayTaxes;
        private System.Windows.Forms.Label lblPayTaxes;
        private System.Windows.Forms.Label lblNew;
        private System.Windows.Forms.Panel pnlDetails;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private System.Windows.Forms.MaskedTextBox mskEntryDate;
        private System.Windows.Forms.BindingNavigator bnGenEntry;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.BindingSource bsGenEntry;
        private System.Windows.Forms.DataGridView dgvDashboard;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblMin;
        private System.Windows.Forms.Label lblAvg;
        private System.Windows.Forms.Label lblSum;
        private System.Windows.Forms.BindingSource bsSummary;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape3;
        private System.Windows.Forms.Panel pnlCalendar2;
        private System.Windows.Forms.MonthCalendar cal2;
        private System.Windows.Forms.Label label1;
        private GISControls.TextBoxChar txtLastEntry;
        private System.Windows.Forms.Label lblLastEntry;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label lblDayName;
        private GISControls.TextBoxChar txtYear;
        private System.Windows.Forms.Label lblYear;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape5;
        private GISControls.TextBoxChar textBoxChar1;
        private System.Windows.Forms.Label lblMonth;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ComboBox cboMonth;
        private System.Windows.Forms.ComboBox cboYear;
    }
}
