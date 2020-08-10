namespace GIS
{
    partial class TestDataIngredion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestDataIngredion));
            this.label40 = new System.Windows.Forms.Label();
            this.dgvTestResults = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnEMail = new System.Windows.Forms.Button();
            this.txtGBLNo = new System.Windows.Forms.TextBox();
            this.txtReportNo = new GISControls.TextBoxChar();
            this.label9 = new System.Windows.Forms.Label();
            this.pnlTestSched = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.pnlEMail = new System.Windows.Forms.Panel();
            this.lnkReport = new System.Windows.Forms.LinkLabel();
            this.label36 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.txtSubject = new GISControls.TextBoxChar();
            this.label30 = new System.Windows.Forms.Label();
            this.txtCC = new GISControls.TextBoxChar();
            this.label28 = new System.Windows.Forms.Label();
            this.txtTo = new GISControls.TextBoxChar();
            this.txtBody = new System.Windows.Forms.TextBox();
            this.btnSendEMail = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.btnCloseEMail = new System.Windows.Forms.Button();
            this.txtDateEMailed = new GISControls.TextBoxChar();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestResults)).BeginInit();
            this.pnlTestSched.SuspendLayout();
            this.pnlEMail.SuspendLayout();
            this.SuspendLayout();
            // 
            // label40
            // 
            this.label40.BackColor = System.Drawing.Color.Transparent;
            this.label40.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.ForeColor = System.Drawing.Color.Black;
            this.label40.Location = new System.Drawing.Point(20, 21);
            this.label40.Margin = new System.Windows.Forms.Padding(0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(57, 21);
            this.label40.TabIndex = 13;
            this.label40.Text = "GBL No.";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvTestResults
            // 
            this.dgvTestResults.AllowUserToAddRows = false;
            this.dgvTestResults.AllowUserToDeleteRows = false;
            this.dgvTestResults.AllowUserToResizeColumns = false;
            this.dgvTestResults.AllowUserToResizeRows = false;
            this.dgvTestResults.BackgroundColor = System.Drawing.Color.White;
            this.dgvTestResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTestResults.Location = new System.Drawing.Point(23, 58);
            this.dgvTestResults.Name = "dgvTestResults";
            this.dgvTestResults.Size = new System.Drawing.Size(881, 231);
            this.dgvTestResults.TabIndex = 15;
            this.dgvTestResults.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvTestResults_CellBeginEdit);
            this.dgvTestResults.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvTestResults_CellValidating);
            this.dgvTestResults.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvTestResults_CurrentCellDirtyStateChanged);
            this.dgvTestResults.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvTestResults_DataError);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(824, 295);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 25);
            this.btnClose.TabIndex = 115;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEMail
            // 
            this.btnEMail.Image = ((System.Drawing.Image)(resources.GetObject("btnEMail.Image")));
            this.btnEMail.Location = new System.Drawing.Point(738, 295);
            this.btnEMail.Name = "btnEMail";
            this.btnEMail.Size = new System.Drawing.Size(80, 25);
            this.btnEMail.TabIndex = 114;
            this.btnEMail.Text = "&EMail";
            this.btnEMail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEMail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEMail.UseVisualStyleBackColor = true;
            this.btnEMail.Click += new System.EventHandler(this.btnEMail_Click);
            // 
            // txtGBLNo
            // 
            this.txtGBLNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGBLNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGBLNo.Location = new System.Drawing.Point(80, 22);
            this.txtGBLNo.Name = "txtGBLNo";
            this.txtGBLNo.ReadOnly = true;
            this.txtGBLNo.Size = new System.Drawing.Size(87, 21);
            this.txtGBLNo.TabIndex = 116;
            this.txtGBLNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtReportNo
            // 
            this.txtReportNo.BackColor = System.Drawing.Color.White;
            this.txtReportNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtReportNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReportNo.Location = new System.Drawing.Point(830, 31);
            this.txtReportNo.Name = "txtReportNo";
            this.txtReportNo.ReadOnly = true;
            this.txtReportNo.Size = new System.Drawing.Size(74, 21);
            this.txtReportNo.TabIndex = 120;
            this.txtReportNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(761, 32);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 21);
            this.label9.TabIndex = 119;
            this.label9.Text = "Report No.";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTestSched
            // 
            this.pnlTestSched.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTestSched.Controls.Add(this.btnOK);
            this.pnlTestSched.Controls.Add(this.label2);
            this.pnlTestSched.Controls.Add(this.dtpEndDate);
            this.pnlTestSched.Controls.Add(this.dtpStartDate);
            this.pnlTestSched.Controls.Add(this.label1);
            this.pnlTestSched.Controls.Add(this.label55);
            this.pnlTestSched.Location = new System.Drawing.Point(346, 15);
            this.pnlTestSched.Name = "pnlTestSched";
            this.pnlTestSched.Size = new System.Drawing.Size(217, 134);
            this.pnlTestSched.TabIndex = 121;
            this.pnlTestSched.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(126, 90);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(69, 25);
            this.btnOK.TabIndex = 145;
            this.btnOK.Text = "O&K";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 144;
            this.label2.Text = "End Date";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "MM/dd/yyyy";
            this.dtpEndDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(94, 62);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(101, 22);
            this.dtpEndDate.TabIndex = 143;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "MM/dd/yyyy";
            this.dtpStartDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(94, 34);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(101, 22);
            this.dtpStartDate.TabIndex = 142;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(17, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 141;
            this.label1.Text = "Start Date";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label55
            // 
            this.label55.BackColor = System.Drawing.Color.SteelBlue;
            this.label55.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label55.ForeColor = System.Drawing.Color.White;
            this.label55.Location = new System.Drawing.Point(0, 0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(215, 19);
            this.label55.TabIndex = 140;
            this.label55.Text = "TEST SCHEDULE";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlEMail
            // 
            this.pnlEMail.BackColor = System.Drawing.Color.Bisque;
            this.pnlEMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEMail.Controls.Add(this.lnkReport);
            this.pnlEMail.Controls.Add(this.label36);
            this.pnlEMail.Controls.Add(this.label34);
            this.pnlEMail.Controls.Add(this.label31);
            this.pnlEMail.Controls.Add(this.txtSubject);
            this.pnlEMail.Controls.Add(this.label30);
            this.pnlEMail.Controls.Add(this.txtCC);
            this.pnlEMail.Controls.Add(this.label28);
            this.pnlEMail.Controls.Add(this.txtTo);
            this.pnlEMail.Controls.Add(this.txtBody);
            this.pnlEMail.Controls.Add(this.btnSendEMail);
            this.pnlEMail.Controls.Add(this.label33);
            this.pnlEMail.Controls.Add(this.btnCloseEMail);
            this.pnlEMail.Location = new System.Drawing.Point(191, 12);
            this.pnlEMail.Name = "pnlEMail";
            this.pnlEMail.Size = new System.Drawing.Size(527, 312);
            this.pnlEMail.TabIndex = 379;
            this.pnlEMail.Visible = false;
            // 
            // lnkReport
            // 
            this.lnkReport.Location = new System.Drawing.Point(98, 223);
            this.lnkReport.Name = "lnkReport";
            this.lnkReport.Size = new System.Drawing.Size(403, 21);
            this.lnkReport.TabIndex = 404;
            this.lnkReport.TabStop = true;
            this.lnkReport.Text = "Final Report";
            this.lnkReport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkReport_LinkClicked);
            // 
            // label36
            // 
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.ForeColor = System.Drawing.Color.Black;
            this.label36.Location = new System.Drawing.Point(23, 224);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(78, 18);
            this.label36.TabIndex = 403;
            this.label36.Text = "Attachment :";
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.ForeColor = System.Drawing.Color.Black;
            this.label34.Location = new System.Drawing.Point(23, 130);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(55, 18);
            this.label34.TabIndex = 401;
            this.label34.Text = "Body :";
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(23, 101);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(55, 18);
            this.label31.TabIndex = 400;
            this.label31.Text = "Subject :";
            // 
            // txtSubject
            // 
            this.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSubject.Location = new System.Drawing.Point(101, 96);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(400, 20);
            this.txtSubject.TabIndex = 399;
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.ForeColor = System.Drawing.Color.Black;
            this.label30.Image = ((System.Drawing.Image)(resources.GetObject("label30.Image")));
            this.label30.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label30.Location = new System.Drawing.Point(23, 76);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(55, 18);
            this.label30.TabIndex = 398;
            this.label30.Text = "CC :";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCC
            // 
            this.txtCC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCC.Location = new System.Drawing.Point(101, 71);
            this.txtCC.Name = "txtCC";
            this.txtCC.Size = new System.Drawing.Size(400, 20);
            this.txtCC.TabIndex = 397;
            // 
            // label28
            // 
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.ForeColor = System.Drawing.Color.Black;
            this.label28.Image = ((System.Drawing.Image)(resources.GetObject("label28.Image")));
            this.label28.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label28.Location = new System.Drawing.Point(23, 49);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(55, 18);
            this.label28.TabIndex = 396;
            this.label28.Text = "To :";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTo
            // 
            this.txtTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTo.Location = new System.Drawing.Point(101, 44);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(400, 20);
            this.txtTo.TabIndex = 395;
            // 
            // txtBody
            // 
            this.txtBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBody.Location = new System.Drawing.Point(101, 123);
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBody.Size = new System.Drawing.Size(400, 90);
            this.txtBody.TabIndex = 394;
            // 
            // btnSendEMail
            // 
            this.btnSendEMail.BackColor = System.Drawing.Color.AliceBlue;
            this.btnSendEMail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSendEMail.Location = new System.Drawing.Point(363, 254);
            this.btnSendEMail.Name = "btnSendEMail";
            this.btnSendEMail.Size = new System.Drawing.Size(69, 25);
            this.btnSendEMail.TabIndex = 390;
            this.btnSendEMail.Text = "Se&nd";
            this.btnSendEMail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSendEMail.UseVisualStyleBackColor = false;
            this.btnSendEMail.Click += new System.EventHandler(this.btnSendEMail_Click);
            // 
            // label33
            // 
            this.label33.BackColor = System.Drawing.Color.SteelBlue;
            this.label33.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.White;
            this.label33.Location = new System.Drawing.Point(-1, 0);
            this.label33.Margin = new System.Windows.Forms.Padding(0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(527, 21);
            this.label33.TabIndex = 388;
            this.label33.Text = "E-Mail Final Report";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCloseEMail
            // 
            this.btnCloseEMail.BackColor = System.Drawing.Color.AliceBlue;
            this.btnCloseEMail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCloseEMail.Location = new System.Drawing.Point(438, 254);
            this.btnCloseEMail.Name = "btnCloseEMail";
            this.btnCloseEMail.Size = new System.Drawing.Size(69, 25);
            this.btnCloseEMail.TabIndex = 386;
            this.btnCloseEMail.Text = "Cl&ose";
            this.btnCloseEMail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseEMail.UseVisualStyleBackColor = false;
            this.btnCloseEMail.Click += new System.EventHandler(this.btnCloseEMail_Click);
            // 
            // txtDateEMailed
            // 
            this.txtDateEMailed.BackColor = System.Drawing.Color.White;
            this.txtDateEMailed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDateEMailed.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDateEMailed.Location = new System.Drawing.Point(99, 295);
            this.txtDateEMailed.Name = "txtDateEMailed";
            this.txtDateEMailed.ReadOnly = true;
            this.txtDateEMailed.Size = new System.Drawing.Size(160, 21);
            this.txtDateEMailed.TabIndex = 381;
            this.txtDateEMailed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(20, 297);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 21);
            this.label3.TabIndex = 380;
            this.label3.Text = "Date Mailed:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TestDataIngredion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 346);
            this.Controls.Add(this.pnlEMail);
            this.Controls.Add(this.txtDateEMailed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pnlTestSched);
            this.Controls.Add(this.txtReportNo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtGBLNo);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnEMail);
            this.Controls.Add(this.dgvTestResults);
            this.Controls.Add(this.label40);
            this.Name = "TestDataIngredion";
            this.Tag = "TestDataIngredion";
            this.Text = "TestDataIngredion";
            this.Load += new System.EventHandler(this.TestDataIngredion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestResults)).EndInit();
            this.pnlTestSched.ResumeLayout(false);
            this.pnlEMail.ResumeLayout(false);
            this.pnlEMail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.DataGridView dgvTestResults;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnEMail;
        private System.Windows.Forms.TextBox txtGBLNo;
        private GISControls.TextBoxChar txtReportNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel pnlTestSched;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Panel pnlEMail;
        private System.Windows.Forms.LinkLabel lnkReport;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label31;
        private GISControls.TextBoxChar txtSubject;
        private System.Windows.Forms.Label label30;
        private GISControls.TextBoxChar txtCC;
        private System.Windows.Forms.Label label28;
        private GISControls.TextBoxChar txtTo;
        private System.Windows.Forms.TextBox txtBody;
        private System.Windows.Forms.Button btnSendEMail;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Button btnCloseEMail;
        private GISControls.TextBoxChar txtDateEMailed;
        private System.Windows.Forms.Label label3;
    }
}