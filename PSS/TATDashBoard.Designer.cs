namespace PSS
{
    partial class TATDashBoard
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TATDashBoard));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlAllChart = new System.Windows.Forms.Panel();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvDataSource = new System.Windows.Forms.DataGridView();
            this.pnlNotes = new System.Windows.Forms.Panel();
            this.dgvSponsors = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.picSponsors = new System.Windows.Forms.PictureBox();
            this.txtSponsorID = new GISControls.TextBoxChar();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSCDesc = new GISControls.TextBoxChar();
            this.picSC = new System.Windows.Forms.PictureBox();
            this.txtSC = new GISControls.TextBoxChar();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvSC = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlTATChart = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblGroup = new System.Windows.Forms.Label();
            this.cboGrpCode = new System.Windows.Forms.ComboBox();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.pnlDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSC)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlAllChart
            // 
            this.pnlAllChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAllChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAllChart.Location = new System.Drawing.Point(12, 96);
            this.pnlAllChart.Name = "pnlAllChart";
            this.pnlAllChart.Size = new System.Drawing.Size(538, 252);
            this.pnlAllChart.TabIndex = 0;
            // 
            // pnlDetails
            // 
            this.pnlDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDetails.Controls.Add(this.label2);
            this.pnlDetails.Controls.Add(this.dgvDataSource);
            this.pnlDetails.Location = new System.Drawing.Point(5, 358);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(545, 265);
            this.pnlDetails.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(4, -1);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(2);
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 117;
            this.label2.Text = "Data Source";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvDataSource
            // 
            this.dgvDataSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataSource.BackgroundColor = System.Drawing.Color.White;
            this.dgvDataSource.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataSource.Location = new System.Drawing.Point(7, 18);
            this.dgvDataSource.Name = "dgvDataSource";
            this.dgvDataSource.ReadOnly = true;
            this.dgvDataSource.Size = new System.Drawing.Size(535, 244);
            this.dgvDataSource.TabIndex = 0;
            this.dgvDataSource.DoubleClick += new System.EventHandler(this.dgvDataSource_DoubleClick);
            // 
            // pnlNotes
            // 
            this.pnlNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlNotes.BackColor = System.Drawing.Color.LightBlue;
            this.pnlNotes.Location = new System.Drawing.Point(556, 358);
            this.pnlNotes.Name = "pnlNotes";
            this.pnlNotes.Size = new System.Drawing.Size(369, 265);
            this.pnlNotes.TabIndex = 2;
            // 
            // dgvSponsors
            // 
            this.dgvSponsors.AllowUserToAddRows = false;
            this.dgvSponsors.AllowUserToDeleteRows = false;
            this.dgvSponsors.BackgroundColor = System.Drawing.Color.White;
            this.dgvSponsors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSponsors.ColumnHeadersVisible = false;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSponsors.DefaultCellStyle = dataGridViewCellStyle13;
            this.dgvSponsors.Location = new System.Drawing.Point(200, 30);
            this.dgvSponsors.Name = "dgvSponsors";
            this.dgvSponsors.ReadOnly = true;
            this.dgvSponsors.RowHeadersVisible = false;
            this.dgvSponsors.Size = new System.Drawing.Size(390, 318);
            this.dgvSponsors.TabIndex = 117;
            this.dgvSponsors.Visible = false;
            this.dgvSponsors.DoubleClick += new System.EventHandler(this.dgvSponsors_DoubleClick);
            this.dgvSponsors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSponsors_KeyPress);
            this.dgvSponsors.Leave += new System.EventHandler(this.dgvSponsors_Leave);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(518, 57);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 124;
            this.btnOK.Text = "O&K";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtSponsor
            // 
            this.txtSponsor.BackColor = System.Drawing.Color.White;
            this.txtSponsor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsor.Location = new System.Drawing.Point(200, 10);
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(372, 21);
            this.txtSponsor.TabIndex = 114;
            this.txtSponsor.TextChanged += new System.EventHandler(this.txtSponsor_TextChanged);
            this.txtSponsor.Enter += new System.EventHandler(this.txtSponsor_Enter);
            this.txtSponsor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsor_KeyPress);
            // 
            // picSponsors
            // 
            this.picSponsors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSponsors.BackgroundImage")));
            this.picSponsors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSponsors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSponsors.Location = new System.Drawing.Point(571, 10);
            this.picSponsors.Name = "picSponsors";
            this.picSponsors.Size = new System.Drawing.Size(19, 21);
            this.picSponsors.TabIndex = 115;
            this.picSponsors.TabStop = false;
            this.picSponsors.Click += new System.EventHandler(this.picSponsors_Click);
            // 
            // txtSponsorID
            // 
            this.txtSponsorID.BackColor = System.Drawing.Color.White;
            this.txtSponsorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsorID.Location = new System.Drawing.Point(127, 10);
            this.txtSponsorID.Name = "txtSponsorID";
            this.txtSponsorID.Size = new System.Drawing.Size(74, 21);
            this.txtSponsorID.TabIndex = 113;
            this.txtSponsorID.Enter += new System.EventHandler(this.txtSponsorID_Enter);
            this.txtSponsorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsorID_KeyPress);
            this.txtSponsorID.Leave += new System.EventHandler(this.txtSponsorID_Leave);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(5, 12);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(2);
            this.label5.Size = new System.Drawing.Size(110, 18);
            this.label5.TabIndex = 116;
            this.label5.Text = "Sponsor ID/Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSCDesc
            // 
            this.txtSCDesc.BackColor = System.Drawing.Color.White;
            this.txtSCDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSCDesc.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSCDesc.Location = new System.Drawing.Point(200, 33);
            this.txtSCDesc.Name = "txtSCDesc";
            this.txtSCDesc.Size = new System.Drawing.Size(372, 21);
            this.txtSCDesc.TabIndex = 126;
            this.txtSCDesc.TextChanged += new System.EventHandler(this.txtSCDesc_TextChanged);
            this.txtSCDesc.Enter += new System.EventHandler(this.txtSCDesc_Enter);
            this.txtSCDesc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSCDesc_KeyPress);
            // 
            // picSC
            // 
            this.picSC.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSC.BackgroundImage")));
            this.picSC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSC.Location = new System.Drawing.Point(571, 33);
            this.picSC.Name = "picSC";
            this.picSC.Size = new System.Drawing.Size(19, 21);
            this.picSC.TabIndex = 127;
            this.picSC.TabStop = false;
            this.picSC.Click += new System.EventHandler(this.picSC_Click);
            // 
            // txtSC
            // 
            this.txtSC.BackColor = System.Drawing.Color.White;
            this.txtSC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSC.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSC.Location = new System.Drawing.Point(127, 33);
            this.txtSC.Name = "txtSC";
            this.txtSC.Size = new System.Drawing.Size(74, 21);
            this.txtSC.TabIndex = 125;
            this.txtSC.Enter += new System.EventHandler(this.txtSC_Enter);
            this.txtSC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSC_KeyPress);
            this.txtSC.Leave += new System.EventHandler(this.txtSC_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(5, 35);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(120, 18);
            this.label1.TabIndex = 128;
            this.label1.Text = "Service Code/Desc";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvSC
            // 
            this.dgvSC.AllowUserToAddRows = false;
            this.dgvSC.AllowUserToDeleteRows = false;
            this.dgvSC.BackgroundColor = System.Drawing.Color.White;
            this.dgvSC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSC.ColumnHeadersVisible = false;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSC.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvSC.Location = new System.Drawing.Point(200, 53);
            this.dgvSC.Name = "dgvSC";
            this.dgvSC.ReadOnly = true;
            this.dgvSC.RowHeadersVisible = false;
            this.dgvSC.Size = new System.Drawing.Size(390, 295);
            this.dgvSC.TabIndex = 129;
            this.dgvSC.Visible = false;
            this.dgvSC.DoubleClick += new System.EventHandler(this.dgvSC_DoubleClick);
            this.dgvSC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSC_KeyPress);
            this.dgvSC.Leave += new System.EventHandler(this.dgvSC_Leave);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(6, 57);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(125, 18);
            this.label3.TabIndex = 131;
            this.label3.Text = "Date Covered: From";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "MM/dd/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(127, 57);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(89, 20);
            this.dtpFrom.TabIndex = 132;
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "MM/dd/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(255, 57);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(89, 20);
            this.dtpTo.TabIndex = 133;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(226, 57);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(2);
            this.label4.Size = new System.Drawing.Size(27, 20);
            this.label4.TabIndex = 134;
            this.label4.Text = "To";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlTATChart
            // 
            this.pnlTATChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTATChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTATChart.Location = new System.Drawing.Point(553, 96);
            this.pnlTATChart.Name = "pnlTATChart";
            this.pnlTATChart.Size = new System.Drawing.Size(369, 252);
            this.pnlTATChart.TabIndex = 130;
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.BackColor = System.Drawing.Color.White;
            this.lblProgress.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.Color.DarkRed;
            this.lblProgress.Location = new System.Drawing.Point(599, 58);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(323, 20);
            this.lblProgress.TabIndex = 135;
            this.lblProgress.Text = "Retrieving data and generating chart....";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProgress.Visible = false;
            // 
            // lblGroup
            // 
            this.lblGroup.BackColor = System.Drawing.Color.Transparent;
            this.lblGroup.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGroup.ForeColor = System.Drawing.Color.Black;
            this.lblGroup.Location = new System.Drawing.Point(597, 12);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Padding = new System.Windows.Forms.Padding(2);
            this.lblGroup.Size = new System.Drawing.Size(84, 18);
            this.lblGroup.TabIndex = 136;
            this.lblGroup.Text = "Test Method";
            this.lblGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGroup.Visible = false;
            // 
            // cboGrpCode
            // 
            this.cboGrpCode.FormattingEnabled = true;
            this.cboGrpCode.Items.AddRange(new object[] {
            "--select--",
            "MF",
            "DI",
            "DI+S",
            "MF+IPM"});
            this.cboGrpCode.Location = new System.Drawing.Point(682, 10);
            this.cboGrpCode.Name = "cboGrpCode";
            this.cboGrpCode.Size = new System.Drawing.Size(129, 21);
            this.cboGrpCode.TabIndex = 137;
            this.cboGrpCode.Visible = false;
            this.cboGrpCode.SelectedIndexChanged += new System.EventHandler(this.cboGrpCode_SelectedIndexChanged);
            this.cboGrpCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboGrpCode_KeyPress);
            // 
            // cboCategory
            // 
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Items.AddRange(new object[] {
            "1: Date Received to Proposed Start Date",
            "2: Date Received to Actual Start Date",
            "3: Actual Start Date to Actual End Date",
            "4: Actual End Date to Report Date",
            "5: Report Date to QA Approved Date",
            "6: QA Approved Date to Report Mail Date",
            "7: Report Mail Date to Invoice Date",
            "8: Invoice Date to Invoice Mail Date",
            "9: Date Received to Actual End Date",
            "10: Date Received to Invoice Mail Date",
            "11: Date Received to Report Mail Date"});
            this.cboCategory.Location = new System.Drawing.Point(682, 34);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(240, 21);
            this.cboCategory.TabIndex = 139;
            this.cboCategory.Visible = false;
            this.cboCategory.SelectedIndexChanged += new System.EventHandler(this.cboCategory_SelectedIndexChanged);
            this.cboCategory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCategory_KeyPress);
            // 
            // lblCategory
            // 
            this.lblCategory.BackColor = System.Drawing.Color.Transparent;
            this.lblCategory.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCategory.ForeColor = System.Drawing.Color.Black;
            this.lblCategory.Location = new System.Drawing.Point(597, 36);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Padding = new System.Windows.Forms.Padding(2);
            this.lblCategory.Size = new System.Drawing.Size(73, 18);
            this.lblCategory.TabIndex = 138;
            this.lblCategory.Text = "Category";
            this.lblCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCategory.Visible = false;
            // 
            // TATDashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(937, 635);
            this.Controls.Add(this.cboCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cboGrpCode);
            this.Controls.Add(this.lblGroup);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.dgvSponsors);
            this.Controls.Add(this.dgvSC);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pnlTATChart);
            this.Controls.Add(this.txtSCDesc);
            this.Controls.Add(this.picSC);
            this.Controls.Add(this.txtSC);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtSponsor);
            this.Controls.Add(this.picSponsors);
            this.Controls.Add(this.txtSponsorID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pnlNotes);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.pnlAllChart);
            this.Name = "TATDashBoard";
            this.Text = "TAT DashBoard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TATDashBoard_FormClosing);
            this.Load += new System.EventHandler(this.TATDashBoard_Load);
            this.pnlDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlAllChart;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Panel pnlNotes;
        private System.Windows.Forms.DataGridView dgvSponsors;
        private System.Windows.Forms.Button btnOK;
        private GISControls.TextBoxChar txtSponsor;
        private System.Windows.Forms.PictureBox picSponsors;
        private GISControls.TextBoxChar txtSponsorID;
        private System.Windows.Forms.Label label5;
        private GISControls.TextBoxChar txtSCDesc;
        private System.Windows.Forms.PictureBox picSC;
        private GISControls.TextBoxChar txtSC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvSC;
        private System.Windows.Forms.DataGridView dgvDataSource;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlTATChart;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblGroup;
        private System.Windows.Forms.ComboBox cboGrpCode;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.Label lblCategory;

    }
}