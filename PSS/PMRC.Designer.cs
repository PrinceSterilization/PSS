namespace PSS
{
    partial class PMRC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PMRC));
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtID = new System.Windows.Forms.TextBox();
            this.dgvSC = new System.Windows.Forms.DataGridView();
            this.dgvSponsors = new System.Windows.Forms.DataGridView();
            this.label16 = new System.Windows.Forms.Label();
            this.txtMethod = new System.Windows.Forms.TextBox();
            this.txtConclusion = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.txtPurpose = new System.Windows.Forms.TextBox();
            this.picSC = new System.Windows.Forms.PictureBox();
            this.txtSC = new GISControls.TextBoxChar();
            this.txtSCDesc = new GISControls.TextBoxChar();
            this.label1 = new System.Windows.Forms.Label();
            this.picSponsors = new System.Windows.Forms.PictureBox();
            this.txtSponsorID = new GISControls.TextBoxChar();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.label5 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).BeginInit();
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
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtID);
            this.pnlRecord.Controls.Add(this.dgvSC);
            this.pnlRecord.Controls.Add(this.dgvSponsors);
            this.pnlRecord.Controls.Add(this.label16);
            this.pnlRecord.Controls.Add(this.txtMethod);
            this.pnlRecord.Controls.Add(this.txtConclusion);
            this.pnlRecord.Controls.Add(this.label17);
            this.pnlRecord.Controls.Add(this.label19);
            this.pnlRecord.Controls.Add(this.label18);
            this.pnlRecord.Controls.Add(this.txtResults);
            this.pnlRecord.Controls.Add(this.txtPurpose);
            this.pnlRecord.Controls.Add(this.picSC);
            this.pnlRecord.Controls.Add(this.txtSC);
            this.pnlRecord.Controls.Add(this.txtSCDesc);
            this.pnlRecord.Controls.Add(this.label1);
            this.pnlRecord.Controls.Add(this.picSponsors);
            this.pnlRecord.Controls.Add(this.txtSponsorID);
            this.pnlRecord.Controls.Add(this.txtSponsor);
            this.pnlRecord.Controls.Add(this.label5);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Location = new System.Drawing.Point(12, 88);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(588, 420);
            this.pnlRecord.TabIndex = 109;
            this.pnlRecord.Visible = false;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(511, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 381;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(522, 24);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(47, 21);
            this.txtID.TabIndex = 380;
            this.txtID.Visible = false;
            // 
            // dgvSC
            // 
            this.dgvSC.AllowUserToAddRows = false;
            this.dgvSC.AllowUserToDeleteRows = false;
            this.dgvSC.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgvSC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSC.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSC.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSC.Location = new System.Drawing.Point(169, 64);
            this.dgvSC.Name = "dgvSC";
            this.dgvSC.ReadOnly = true;
            this.dgvSC.RowHeadersVisible = false;
            this.dgvSC.Size = new System.Drawing.Size(400, 339);
            this.dgvSC.TabIndex = 2;
            this.dgvSC.Visible = false;
            this.dgvSC.DoubleClick += new System.EventHandler(this.dgvSC_DoubleClick);
            this.dgvSC.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSC_KeyDown);
            this.dgvSC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSC_KeyPress);
            // 
            // dgvSponsors
            // 
            this.dgvSponsors.AllowUserToAddRows = false;
            this.dgvSponsors.AllowUserToDeleteRows = false;
            this.dgvSponsors.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgvSponsors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSponsors.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSponsors.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSponsors.Location = new System.Drawing.Point(169, 87);
            this.dgvSponsors.Name = "dgvSponsors";
            this.dgvSponsors.ReadOnly = true;
            this.dgvSponsors.RowHeadersVisible = false;
            this.dgvSponsors.Size = new System.Drawing.Size(400, 316);
            this.dgvSponsors.TabIndex = 5;
            this.dgvSponsors.Visible = false;
            this.dgvSponsors.DoubleClick += new System.EventHandler(this.dgvSponsors_DoubleClick);
            this.dgvSponsors.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSponsors_KeyDown);
            this.dgvSponsors.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSponsors_KeyPress);
            this.dgvSponsors.Leave += new System.EventHandler(this.dgvSponsors_Leave);
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Font = new System.Drawing.Font("Arial Narrow", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Image = ((System.Drawing.Image)(resources.GetObject("label16.Image")));
            this.label16.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label16.Location = new System.Drawing.Point(17, 172);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(97, 75);
            this.label16.TabIndex = 376;
            this.label16.Text = "Method";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMethod
            // 
            this.txtMethod.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMethod.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMethod.Location = new System.Drawing.Point(113, 172);
            this.txtMethod.Multiline = true;
            this.txtMethod.Name = "txtMethod";
            this.txtMethod.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMethod.Size = new System.Drawing.Size(460, 75);
            this.txtMethod.TabIndex = 373;
            // 
            // txtConclusion
            // 
            this.txtConclusion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConclusion.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConclusion.Location = new System.Drawing.Point(113, 328);
            this.txtConclusion.Multiline = true;
            this.txtConclusion.Name = "txtConclusion";
            this.txtConclusion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConclusion.Size = new System.Drawing.Size(460, 75);
            this.txtConclusion.TabIndex = 375;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label17.Font = new System.Drawing.Font("Arial Narrow", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Image = ((System.Drawing.Image)(resources.GetObject("label17.Image")));
            this.label17.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label17.Location = new System.Drawing.Point(17, 328);
            this.label17.Margin = new System.Windows.Forms.Padding(0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(97, 75);
            this.label17.TabIndex = 377;
            this.label17.Text = "Conclusion";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Font = new System.Drawing.Font("Arial Narrow", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Image = ((System.Drawing.Image)(resources.GetObject("label19.Image")));
            this.label19.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label19.Location = new System.Drawing.Point(17, 250);
            this.label19.Margin = new System.Windows.Forms.Padding(0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(97, 75);
            this.label19.TabIndex = 379;
            this.label19.Text = "Results";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label18.Font = new System.Drawing.Font("Arial Narrow", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Image = ((System.Drawing.Image)(resources.GetObject("label18.Image")));
            this.label18.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label18.Location = new System.Drawing.Point(17, 94);
            this.label18.Margin = new System.Windows.Forms.Padding(0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(97, 75);
            this.label18.TabIndex = 378;
            this.label18.Text = "Purpose";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtResults
            // 
            this.txtResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtResults.Location = new System.Drawing.Point(113, 250);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResults.Size = new System.Drawing.Size(460, 75);
            this.txtResults.TabIndex = 374;
            // 
            // txtPurpose
            // 
            this.txtPurpose.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPurpose.Location = new System.Drawing.Point(113, 94);
            this.txtPurpose.Multiline = true;
            this.txtPurpose.Name = "txtPurpose";
            this.txtPurpose.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPurpose.Size = new System.Drawing.Size(460, 75);
            this.txtPurpose.TabIndex = 372;
            // 
            // picSC
            // 
            this.picSC.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSC.BackgroundImage")));
            this.picSC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSC.Location = new System.Drawing.Point(550, 44);
            this.picSC.Name = "picSC";
            this.picSC.Size = new System.Drawing.Size(19, 21);
            this.picSC.TabIndex = 125;
            this.picSC.TabStop = false;
            this.picSC.Click += new System.EventHandler(this.picSC_Click);
            // 
            // txtSC
            // 
            this.txtSC.BackColor = System.Drawing.Color.White;
            this.txtSC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSC.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSC.Location = new System.Drawing.Point(113, 44);
            this.txtSC.MaxLength = 5;
            this.txtSC.Name = "txtSC";
            this.txtSC.Size = new System.Drawing.Size(57, 21);
            this.txtSC.TabIndex = 0;
            this.txtSC.Enter += new System.EventHandler(this.txtSC_Enter);
            this.txtSC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSC_KeyPress);
            // 
            // txtSCDesc
            // 
            this.txtSCDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSCDesc.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSCDesc.Location = new System.Drawing.Point(169, 44);
            this.txtSCDesc.MaxLength = 50;
            this.txtSCDesc.Name = "txtSCDesc";
            this.txtSCDesc.Size = new System.Drawing.Size(382, 21);
            this.txtSCDesc.TabIndex = 1;
            this.txtSCDesc.TextChanged += new System.EventHandler(this.txtSCDesc_TextChanged);
            this.txtSCDesc.Enter += new System.EventHandler(this.txtSCDesc_Enter);
            this.txtSCDesc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSCDesc_KeyPress);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(14, 46);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(99, 19);
            this.label1.TabIndex = 123;
            this.label1.Text = "Service Code";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picSponsors
            // 
            this.picSponsors.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picSponsors.BackgroundImage")));
            this.picSponsors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picSponsors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSponsors.Location = new System.Drawing.Point(550, 67);
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
            this.txtSponsorID.Location = new System.Drawing.Point(113, 67);
            this.txtSponsorID.MaxLength = 5;
            this.txtSponsorID.Name = "txtSponsorID";
            this.txtSponsorID.Size = new System.Drawing.Size(57, 21);
            this.txtSponsorID.TabIndex = 3;
            this.txtSponsorID.Enter += new System.EventHandler(this.txtSponsorID_Enter);
            this.txtSponsorID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsorID_KeyPress);
            // 
            // txtSponsor
            // 
            this.txtSponsor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsor.Location = new System.Drawing.Point(169, 67);
            this.txtSponsor.MaxLength = 50;
            this.txtSponsor.Name = "txtSponsor";
            this.txtSponsor.Size = new System.Drawing.Size(382, 21);
            this.txtSponsor.TabIndex = 4;
            this.txtSponsor.TextChanged += new System.EventHandler(this.txtSponsor_TextChanged);
            this.txtSponsor.Enter += new System.EventHandler(this.txtSponsor_Enter);
            this.txtSponsor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSponsor_KeyPress);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(14, 69);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(2);
            this.label5.Size = new System.Drawing.Size(99, 19);
            this.label5.TabIndex = 7;
            this.label5.Text = "Sponsor";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(589, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "PMRC DEFAULT TEXT";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PMRC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(900, 657);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "PMRC";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PMRC_KeyDown);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.DataGridView dgvSponsors;
        private System.Windows.Forms.DataGridView dgvSC;
        private System.Windows.Forms.PictureBox picSC;
        private GISControls.TextBoxChar txtSC;
        private GISControls.TextBoxChar txtSCDesc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picSponsors;
        private GISControls.TextBoxChar txtSponsorID;
        private GISControls.TextBoxChar txtSponsor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtMethod;
        private System.Windows.Forms.TextBox txtConclusion;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.TextBox txtPurpose;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Button btnClose;
    }
}
