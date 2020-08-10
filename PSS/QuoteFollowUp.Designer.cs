namespace PSS
{
    partial class QuoteFollowUp
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
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.txtCmpyCode = new GISControls.TextBoxChar();
            this.txtRevNo = new GISControls.TextBoxChar();
            this.txtQuoteNo = new GISControls.TextBoxChar();
            this.lnkCurrPDF = new System.Windows.Forms.LinkLabel();
            this.btnCreatePDF = new System.Windows.Forms.Button();
            this.txtBody = new GISControls.TextBoxChar();
            this.txtTotalQuotes = new GISControls.TextBoxChar();
            this.lblTotalQuotes = new System.Windows.Forms.Label();
            this.cboCutOffDays = new System.Windows.Forms.ComboBox();
            this.chkCheckAll = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.dgvQuoteForFollowUp = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCutOff = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape3 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuoteForFollowUp)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.txtCmpyCode);
            this.pnlRecord.Controls.Add(this.txtRevNo);
            this.pnlRecord.Controls.Add(this.txtQuoteNo);
            this.pnlRecord.Controls.Add(this.lnkCurrPDF);
            this.pnlRecord.Controls.Add(this.btnCreatePDF);
            this.pnlRecord.Controls.Add(this.txtBody);
            this.pnlRecord.Controls.Add(this.txtTotalQuotes);
            this.pnlRecord.Controls.Add(this.lblTotalQuotes);
            this.pnlRecord.Controls.Add(this.cboCutOffDays);
            this.pnlRecord.Controls.Add(this.chkCheckAll);
            this.pnlRecord.Controls.Add(this.btnSend);
            this.pnlRecord.Controls.Add(this.dgvQuoteForFollowUp);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.lblCutOff);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.shapeContainer1);
            this.pnlRecord.Location = new System.Drawing.Point(139, 75);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(978, 629);
            this.pnlRecord.TabIndex = 105;
            // 
            // txtCmpyCode
            // 
            this.txtCmpyCode.BackColor = System.Drawing.Color.White;
            this.txtCmpyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCmpyCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCmpyCode.ForeColor = System.Drawing.Color.Red;
            this.txtCmpyCode.Location = new System.Drawing.Point(65, 26);
            this.txtCmpyCode.MaxLength = 5;
            this.txtCmpyCode.Name = "txtCmpyCode";
            this.txtCmpyCode.Size = new System.Drawing.Size(32, 21);
            this.txtCmpyCode.TabIndex = 190;
            this.txtCmpyCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCmpyCode.Visible = false;
            // 
            // txtRevNo
            // 
            this.txtRevNo.BackColor = System.Drawing.Color.White;
            this.txtRevNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRevNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRevNo.ForeColor = System.Drawing.Color.Red;
            this.txtRevNo.Location = new System.Drawing.Point(27, 53);
            this.txtRevNo.MaxLength = 5;
            this.txtRevNo.Name = "txtRevNo";
            this.txtRevNo.Size = new System.Drawing.Size(32, 21);
            this.txtRevNo.TabIndex = 189;
            this.txtRevNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRevNo.Visible = false;
            // 
            // txtQuoteNo
            // 
            this.txtQuoteNo.BackColor = System.Drawing.Color.White;
            this.txtQuoteNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuoteNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuoteNo.ForeColor = System.Drawing.Color.Red;
            this.txtQuoteNo.Location = new System.Drawing.Point(27, 26);
            this.txtQuoteNo.MaxLength = 5;
            this.txtQuoteNo.Name = "txtQuoteNo";
            this.txtQuoteNo.Size = new System.Drawing.Size(32, 21);
            this.txtQuoteNo.TabIndex = 188;
            this.txtQuoteNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtQuoteNo.Visible = false;
            // 
            // lnkCurrPDF
            // 
            this.lnkCurrPDF.AutoSize = true;
            this.lnkCurrPDF.Location = new System.Drawing.Point(24, 578);
            this.lnkCurrPDF.Name = "lnkCurrPDF";
            this.lnkCurrPDF.Size = new System.Drawing.Size(104, 15);
            this.lnkCurrPDF.TabIndex = 187;
            this.lnkCurrPDF.TabStop = true;
            this.lnkCurrPDF.Text = "View Current PDF";
            this.lnkCurrPDF.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCurrPDF_LinkClicked);
            // 
            // btnCreatePDF
            // 
            this.btnCreatePDF.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreatePDF.ForeColor = System.Drawing.Color.Red;
            this.btnCreatePDF.Location = new System.Drawing.Point(751, 568);
            this.btnCreatePDF.Name = "btnCreatePDF";
            this.btnCreatePDF.Size = new System.Drawing.Size(96, 35);
            this.btnCreatePDF.TabIndex = 186;
            this.btnCreatePDF.Text = "CREATE PDF";
            this.btnCreatePDF.UseVisualStyleBackColor = true;
            this.btnCreatePDF.Click += new System.EventHandler(this.btnCreatePDF_Click);
            // 
            // txtBody
            // 
            this.txtBody.BackColor = System.Drawing.Color.White;
            this.txtBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBody.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBody.ForeColor = System.Drawing.Color.Red;
            this.txtBody.Location = new System.Drawing.Point(27, 80);
            this.txtBody.MaxLength = 5;
            this.txtBody.Name = "txtBody";
            this.txtBody.Size = new System.Drawing.Size(32, 21);
            this.txtBody.TabIndex = 185;
            this.txtBody.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBody.Visible = false;
            // 
            // txtTotalQuotes
            // 
            this.txtTotalQuotes.BackColor = System.Drawing.Color.White;
            this.txtTotalQuotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalQuotes.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalQuotes.ForeColor = System.Drawing.Color.Red;
            this.txtTotalQuotes.Location = new System.Drawing.Point(528, 59);
            this.txtTotalQuotes.MaxLength = 5;
            this.txtTotalQuotes.Name = "txtTotalQuotes";
            this.txtTotalQuotes.Size = new System.Drawing.Size(32, 21);
            this.txtTotalQuotes.TabIndex = 184;
            this.txtTotalQuotes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblTotalQuotes
            // 
            this.lblTotalQuotes.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalQuotes.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalQuotes.ForeColor = System.Drawing.Color.Black;
            this.lblTotalQuotes.Location = new System.Drawing.Point(445, 48);
            this.lblTotalQuotes.Name = "lblTotalQuotes";
            this.lblTotalQuotes.Padding = new System.Windows.Forms.Padding(2);
            this.lblTotalQuotes.Size = new System.Drawing.Size(77, 43);
            this.lblTotalQuotes.TabIndex = 15;
            this.lblTotalQuotes.Text = "Total Quotes:";
            this.lblTotalQuotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboCutOffDays
            // 
            this.cboCutOffDays.FormattingEnabled = true;
            this.cboCutOffDays.Items.AddRange(new object[] {
            " 0",
            " 7",
            "14",
            "21",
            "28",
            "35",
            "42",
            "49",
            "56"});
            this.cboCutOffDays.Location = new System.Drawing.Point(293, 58);
            this.cboCutOffDays.Name = "cboCutOffDays";
            this.cboCutOffDays.Size = new System.Drawing.Size(86, 23);
            this.cboCutOffDays.TabIndex = 14;
            this.cboCutOffDays.SelectedIndexChanged += new System.EventHandler(this.cboCutOffDays_SelectedIndexChanged);
            this.cboCutOffDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboCutOffDays_KeyPress);
            // 
            // chkCheckAll
            // 
            this.chkCheckAll.AutoSize = true;
            this.chkCheckAll.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCheckAll.ForeColor = System.Drawing.Color.Red;
            this.chkCheckAll.Location = new System.Drawing.Point(658, 61);
            this.chkCheckAll.Name = "chkCheckAll";
            this.chkCheckAll.Size = new System.Drawing.Size(79, 19);
            this.chkCheckAll.TabIndex = 12;
            this.chkCheckAll.Text = "Select All";
            this.chkCheckAll.UseVisualStyleBackColor = true;
            this.chkCheckAll.CheckedChanged += new System.EventHandler(this.chkCheckAll_CheckedChanged);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.ForeColor = System.Drawing.Color.Red;
            this.btnSend.Location = new System.Drawing.Point(853, 568);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(96, 35);
            this.btnSend.TabIndex = 11;
            this.btnSend.Text = "SEND";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // dgvQuoteForFollowUp
            // 
            this.dgvQuoteForFollowUp.AllowUserToAddRows = false;
            this.dgvQuoteForFollowUp.AllowUserToDeleteRows = false;
            this.dgvQuoteForFollowUp.AllowUserToResizeColumns = false;
            this.dgvQuoteForFollowUp.BackgroundColor = System.Drawing.Color.White;
            this.dgvQuoteForFollowUp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuoteForFollowUp.Location = new System.Drawing.Point(27, 111);
            this.dgvQuoteForFollowUp.Name = "dgvQuoteForFollowUp";
            this.dgvQuoteForFollowUp.Size = new System.Drawing.Size(922, 451);
            this.dgvQuoteForFollowUp.TabIndex = 8;
            this.dgvQuoteForFollowUp.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvQuoteForFollowUp_CellBeginEdit);
            this.dgvQuoteForFollowUp.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvQuoteForFollowUp_CellClick);
            this.dgvQuoteForFollowUp.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvQuoteForFollowUp_CurrentCellDirtyStateChanged);
            this.dgvQuoteForFollowUp.DoubleClick += new System.EventHandler(this.dgvQuoteForFollowUp_DoubleClick);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(900, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCutOff
            // 
            this.lblCutOff.BackColor = System.Drawing.Color.Transparent;
            this.lblCutOff.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCutOff.ForeColor = System.Drawing.Color.Black;
            this.lblCutOff.Location = new System.Drawing.Point(228, 47);
            this.lblCutOff.Name = "lblCutOff";
            this.lblCutOff.Padding = new System.Windows.Forms.Padding(2);
            this.lblCutOff.Size = new System.Drawing.Size(74, 43);
            this.lblCutOff.TabIndex = 3;
            this.lblCutOff.Text = "Cut-Off Days:";
            this.lblCutOff.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(980, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "QUOTE FOLLOW-UP";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape3});
            this.shapeContainer1.Size = new System.Drawing.Size(976, 627);
            this.shapeContainer1.TabIndex = 9;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape3
            // 
            this.rectangleShape3.Location = new System.Drawing.Point(162, 39);
            this.rectangleShape3.Name = "rectangleShape3";
            this.rectangleShape3.Size = new System.Drawing.Size(637, 61);
            // 
            // QuoteFollowUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1188, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "QuoteFollowUp";
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuoteForFollowUp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblCutOff;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.DataGridView dgvQuoteForFollowUp;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private System.Windows.Forms.CheckBox chkCheckAll;
        private System.Windows.Forms.Button btnSend;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape3;
        private System.Windows.Forms.ComboBox cboCutOffDays;
        private System.Windows.Forms.Label lblTotalQuotes;
        private GISControls.TextBoxChar txtTotalQuotes;
        private GISControls.TextBoxChar txtBody;
        private System.Windows.Forms.Button btnCreatePDF;
        private System.Windows.Forms.LinkLabel lnkCurrPDF;
        private GISControls.TextBoxChar txtRevNo;
        private GISControls.TextBoxChar txtQuoteNo;
        private GISControls.TextBoxChar txtCmpyCode;
    }
}
