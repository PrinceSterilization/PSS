namespace PSS
{
    partial class SOA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SOA));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtSponsorID = new GISControls.TextBoxChar();
            this.picSponsors = new System.Windows.Forms.PictureBox();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvSponsors = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.cboPO = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrtPreview = new System.Windows.Forms.Button();
            this.rdoRegular = new System.Windows.Forms.RadioButton();
            this.rdoPreBilled = new System.Windows.Forms.RadioButton();
            this.btnEMail = new System.Windows.Forms.Button();
            this.lnkFile = new System.Windows.Forms.LinkLabel();
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
            this.btnCancelEmail = new System.Windows.Forms.Button();
            this.pnlEMail = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.chkPDF = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).BeginInit();
            this.pnlEMail.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSponsorID
            // 
            this.txtSponsorID.BackColor = System.Drawing.Color.White;
            this.txtSponsorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsorID.Location = new System.Drawing.Point(124, 27);
            this.txtSponsorID.Name = "txtSponsorID";
            this.txtSponsorID.Size = new System.Drawing.Size(74, 21);
            this.txtSponsorID.TabIndex = 83;
            this.txtSponsorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsorID_KeyPress);
            this.txtSponsorID.Leave += new System.EventHandler(this.txtSponsorID_Leave);
            // 
            // picSponsors
            // 
            this.picSponsors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSponsors.BackgroundImage")));
            this.picSponsors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSponsors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSponsors.Location = new System.Drawing.Point(567, 27);
            this.picSponsors.Name = "picSponsors";
            this.picSponsors.Size = new System.Drawing.Size(19, 21);
            this.picSponsors.TabIndex = 85;
            this.picSponsors.TabStop = false;
            this.picSponsors.Click += new System.EventHandler(this.picSponsors_Click);
            // 
            // txtSponsor
            // 
            this.txtSponsor.BackColor = System.Drawing.Color.White;
            this.txtSponsor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsor.Location = new System.Drawing.Point(197, 27);
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(372, 21);
            this.txtSponsor.TabIndex = 84;
            this.txtSponsor.TextChanged += new System.EventHandler(this.txtSponsor_TextChanged);
            this.txtSponsor.Enter += new System.EventHandler(this.txtSponsor_Enter);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(13, 29);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(2);
            this.label5.Size = new System.Drawing.Size(110, 18);
            this.label5.TabIndex = 86;
            this.label5.Text = "Sponsor ID/Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.dgvSponsors.Location = new System.Drawing.Point(197, 47);
            this.dgvSponsors.Name = "dgvSponsors";
            this.dgvSponsors.ReadOnly = true;
            this.dgvSponsors.RowHeadersVisible = false;
            this.dgvSponsors.Size = new System.Drawing.Size(389, 379);
            this.dgvSponsors.TabIndex = 87;
            this.dgvSponsors.Visible = false;
            this.dgvSponsors.DoubleClick += new System.EventHandler(this.dgvSponsors_DoubleClick);
            this.dgvSponsors.Leave += new System.EventHandler(this.dgvSponsors_Leave);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(220, 3);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(2);
            this.label7.Size = new System.Drawing.Size(111, 21);
            this.label7.TabIndex = 89;
            this.label7.Text = "PO No.";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label7.Visible = false;
            // 
            // cboPO
            // 
            this.cboPO.FormattingEnabled = true;
            this.cboPO.Location = new System.Drawing.Point(283, 3);
            this.cboPO.Name = "cboPO";
            this.cboPO.Size = new System.Drawing.Size(151, 21);
            this.cboPO.TabIndex = 90;
            this.cboPO.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(514, 66);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 27);
            this.btnClose.TabIndex = 92;
            this.btnClose.Text = "Cl&ose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrtPreview
            // 
            this.btnPrtPreview.Location = new System.Drawing.Point(425, 66);
            this.btnPrtPreview.Name = "btnPrtPreview";
            this.btnPrtPreview.Size = new System.Drawing.Size(83, 27);
            this.btnPrtPreview.TabIndex = 91;
            this.btnPrtPreview.Text = "Print Preview";
            this.btnPrtPreview.UseVisualStyleBackColor = true;
            this.btnPrtPreview.Click += new System.EventHandler(this.btnPrtPreview_Click);
            // 
            // rdoRegular
            // 
            this.rdoRegular.AutoSize = true;
            this.rdoRegular.Location = new System.Drawing.Point(106, 7);
            this.rdoRegular.Name = "rdoRegular";
            this.rdoRegular.Size = new System.Drawing.Size(92, 17);
            this.rdoRegular.TabIndex = 93;
            this.rdoRegular.TabStop = true;
            this.rdoRegular.Text = "Regular Billing";
            this.rdoRegular.UseVisualStyleBackColor = true;
            this.rdoRegular.Visible = false;
            // 
            // rdoPreBilled
            // 
            this.rdoPreBilled.AutoSize = true;
            this.rdoPreBilled.Checked = true;
            this.rdoPreBilled.Location = new System.Drawing.Point(24, 7);
            this.rdoPreBilled.Name = "rdoPreBilled";
            this.rdoPreBilled.Size = new System.Drawing.Size(69, 17);
            this.rdoPreBilled.TabIndex = 94;
            this.rdoPreBilled.TabStop = true;
            this.rdoPreBilled.Text = "Pre-Billed";
            this.rdoPreBilled.UseVisualStyleBackColor = true;
            this.rdoPreBilled.Visible = false;
            // 
            // btnEMail
            // 
            this.btnEMail.Location = new System.Drawing.Point(336, 66);
            this.btnEMail.Name = "btnEMail";
            this.btnEMail.Size = new System.Drawing.Size(83, 27);
            this.btnEMail.TabIndex = 96;
            this.btnEMail.Text = "E-Mail";
            this.btnEMail.UseVisualStyleBackColor = true;
            this.btnEMail.Click += new System.EventHandler(this.btnEMail_Click);
            // 
            // lnkFile
            // 
            this.lnkFile.Location = new System.Drawing.Point(104, 223);
            this.lnkFile.Name = "lnkFile";
            this.lnkFile.Size = new System.Drawing.Size(403, 21);
            this.lnkFile.TabIndex = 416;
            this.lnkFile.TabStop = true;
            this.lnkFile.Text = "Statement of Account";
            this.lnkFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFile_LinkClicked);
            // 
            // label36
            // 
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.ForeColor = System.Drawing.Color.Black;
            this.label36.Location = new System.Drawing.Point(29, 223);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(78, 18);
            this.label36.TabIndex = 415;
            this.label36.Text = "Attachment :";
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.ForeColor = System.Drawing.Color.Black;
            this.label34.Location = new System.Drawing.Point(29, 129);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(55, 18);
            this.label34.TabIndex = 414;
            this.label34.Text = "Body :";
            // 
            // label31
            // 
            this.label31.BackColor = System.Drawing.Color.Transparent;
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(29, 100);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(55, 18);
            this.label31.TabIndex = 413;
            this.label31.Text = "Subject :";
            // 
            // txtSubject
            // 
            this.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSubject.Location = new System.Drawing.Point(107, 95);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(400, 20);
            this.txtSubject.TabIndex = 412;
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.Color.Transparent;
            this.label30.ForeColor = System.Drawing.Color.Black;
            this.label30.Image = ((System.Drawing.Image)(resources.GetObject("label30.Image")));
            this.label30.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label30.Location = new System.Drawing.Point(29, 70);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(55, 18);
            this.label30.TabIndex = 411;
            this.label30.Text = "CC :";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCC
            // 
            this.txtCC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCC.Location = new System.Drawing.Point(107, 70);
            this.txtCC.Name = "txtCC";
            this.txtCC.Size = new System.Drawing.Size(400, 20);
            this.txtCC.TabIndex = 410;
            // 
            // label28
            // 
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.ForeColor = System.Drawing.Color.Black;
            this.label28.Image = ((System.Drawing.Image)(resources.GetObject("label28.Image")));
            this.label28.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label28.Location = new System.Drawing.Point(29, 42);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(55, 18);
            this.label28.TabIndex = 409;
            this.label28.Text = "To :";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTo
            // 
            this.txtTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTo.Location = new System.Drawing.Point(107, 43);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(400, 20);
            this.txtTo.TabIndex = 408;
            // 
            // txtBody
            // 
            this.txtBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBody.Location = new System.Drawing.Point(107, 122);
            this.txtBody.Multiline = true;
            this.txtBody.Name = "txtBody";
            this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBody.Size = new System.Drawing.Size(400, 90);
            this.txtBody.TabIndex = 407;
            // 
            // btnSendEMail
            // 
            this.btnSendEMail.BackColor = System.Drawing.Color.AliceBlue;
            this.btnSendEMail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSendEMail.Location = new System.Drawing.Point(369, 253);
            this.btnSendEMail.Name = "btnSendEMail";
            this.btnSendEMail.Size = new System.Drawing.Size(69, 25);
            this.btnSendEMail.TabIndex = 406;
            this.btnSendEMail.Text = "Se&nd";
            this.btnSendEMail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSendEMail.UseVisualStyleBackColor = false;
            this.btnSendEMail.Click += new System.EventHandler(this.btnSendEMail_Click);
            // 
            // btnCancelEmail
            // 
            this.btnCancelEmail.BackColor = System.Drawing.Color.AliceBlue;
            this.btnCancelEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCancelEmail.Location = new System.Drawing.Point(444, 253);
            this.btnCancelEmail.Name = "btnCancelEmail";
            this.btnCancelEmail.Size = new System.Drawing.Size(69, 25);
            this.btnCancelEmail.TabIndex = 405;
            this.btnCancelEmail.Text = "Ca&ncel";
            this.btnCancelEmail.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelEmail.UseVisualStyleBackColor = false;
            this.btnCancelEmail.Click += new System.EventHandler(this.btnCancelEmail_Click);
            // 
            // pnlEMail
            // 
            this.pnlEMail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEMail.Controls.Add(this.label28);
            this.pnlEMail.Controls.Add(this.label1);
            this.pnlEMail.Controls.Add(this.lnkFile);
            this.pnlEMail.Controls.Add(this.btnCancelEmail);
            this.pnlEMail.Controls.Add(this.label36);
            this.pnlEMail.Controls.Add(this.btnSendEMail);
            this.pnlEMail.Controls.Add(this.label34);
            this.pnlEMail.Controls.Add(this.txtBody);
            this.pnlEMail.Controls.Add(this.label31);
            this.pnlEMail.Controls.Add(this.txtTo);
            this.pnlEMail.Controls.Add(this.txtSubject);
            this.pnlEMail.Controls.Add(this.txtCC);
            this.pnlEMail.Controls.Add(this.label30);
            this.pnlEMail.Enabled = false;
            this.pnlEMail.Location = new System.Drawing.Point(16, 116);
            this.pnlEMail.Name = "pnlEMail";
            this.pnlEMail.Size = new System.Drawing.Size(570, 310);
            this.pnlEMail.TabIndex = 417;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(4, 3);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(110, 18);
            this.label1.TabIndex = 418;
            this.label1.Text = "E-Mail Setup:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkPDF
            // 
            this.chkPDF.AutoSize = true;
            this.chkPDF.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPDF.Location = new System.Drawing.Point(486, 8);
            this.chkPDF.Name = "chkPDF";
            this.chkPDF.Size = new System.Drawing.Size(100, 17);
            this.chkPDF.TabIndex = 95;
            this.chkPDF.Text = "Create PDF File";
            this.chkPDF.UseVisualStyleBackColor = true;
            this.chkPDF.Visible = false;
            // 
            // SOA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(606, 442);
            this.Controls.Add(this.dgvSponsors);
            this.Controls.Add(this.pnlEMail);
            this.Controls.Add(this.btnEMail);
            this.Controls.Add(this.chkPDF);
            this.Controls.Add(this.rdoPreBilled);
            this.Controls.Add(this.rdoRegular);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrtPreview);
            this.Controls.Add(this.cboPO);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSponsorID);
            this.Controls.Add(this.picSponsors);
            this.Controls.Add(this.txtSponsor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SOA";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "a";
            this.Load += new System.EventHandler(this.SOA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).EndInit();
            this.pnlEMail.ResumeLayout(false);
            this.pnlEMail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GISControls.TextBoxChar txtSponsorID;
        private System.Windows.Forms.PictureBox picSponsors;
        private GISControls.TextBoxChar txtSponsor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvSponsors;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboPO;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrtPreview;
        private System.Windows.Forms.RadioButton rdoRegular;
        private System.Windows.Forms.RadioButton rdoPreBilled;
        private System.Windows.Forms.Button btnEMail;
        private System.Windows.Forms.LinkLabel lnkFile;
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
        private System.Windows.Forms.Button btnCancelEmail;
        private System.Windows.Forms.Panel pnlEMail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkPDF;
    }
}