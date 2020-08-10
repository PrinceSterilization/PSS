namespace PSS
{
    partial class EMLocations
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
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.cboSponsor = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtISO = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtLocationPoint = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtSampleNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.grpUtype = new System.Windows.Forms.GroupBox();
            this.rbtnAir = new System.Windows.Forms.RadioButton();
            this.rbtnSurface = new System.Windows.Forms.RadioButton();
            this.txtLocId = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.TextBox();
            this.bsLocation = new System.Windows.Forms.BindingSource(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            this.grpUtype.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsLocation)).BeginInit();
            this.SuspendLayout();
            // 
            // cklColumns
            // 
            this.cklColumns.Size = new System.Drawing.Size(122, 180);
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.chkActive);
            this.pnlRecord.Controls.Add(this.cboSponsor);
            this.pnlRecord.Controls.Add(this.label7);
            this.pnlRecord.Controls.Add(this.txtISO);
            this.pnlRecord.Controls.Add(this.label5);
            this.pnlRecord.Controls.Add(this.label1);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtLocationPoint);
            this.pnlRecord.Controls.Add(this.txtDescription);
            this.pnlRecord.Controls.Add(this.txtSampleNo);
            this.pnlRecord.Controls.Add(this.label6);
            this.pnlRecord.Controls.Add(this.label3);
            this.pnlRecord.Controls.Add(this.label4);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.grpUtype);
            this.pnlRecord.Controls.Add(this.txtLocId);
            this.pnlRecord.Controls.Add(this.txtType);
            this.pnlRecord.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.pnlRecord.Location = new System.Drawing.Point(258, 92);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(480, 228);
            this.pnlRecord.TabIndex = 109;
            this.pnlRecord.Visible = false;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(397, 39);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(57, 19);
            this.chkActive.TabIndex = 24;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // cboSponsor
            // 
            this.cboSponsor.FormattingEnabled = true;
            this.cboSponsor.Location = new System.Drawing.Point(135, 172);
            this.cboSponsor.Name = "cboSponsor";
            this.cboSponsor.Size = new System.Drawing.Size(291, 23);
            this.cboSponsor.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(26, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 15);
            this.label7.TabIndex = 21;
            this.label7.Text = "Sponsor";
            // 
            // txtISO
            // 
            this.txtISO.Location = new System.Drawing.Point(135, 145);
            this.txtISO.Name = "txtISO";
            this.txtISO.Size = new System.Drawing.Size(45, 21);
            this.txtISO.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "ISO Class";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 18;
            this.label1.Text = "Location Type";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Red;
            this.btnClose.Location = new System.Drawing.Point(411, -3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(68, 24);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "Close [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtLocationPoint
            // 
            this.txtLocationPoint.Location = new System.Drawing.Point(135, 39);
            this.txtLocationPoint.Name = "txtLocationPoint";
            this.txtLocationPoint.Size = new System.Drawing.Size(171, 21);
            this.txtLocationPoint.TabIndex = 1;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(135, 87);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(171, 21);
            this.txtDescription.TabIndex = 2;
            // 
            // txtSampleNo
            // 
            this.txtSampleNo.Location = new System.Drawing.Point(135, 63);
            this.txtSampleNo.Name = "txtSampleNo";
            this.txtSampleNo.Size = new System.Drawing.Size(66, 21);
            this.txtSampleNo.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Sample No.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Location Point";
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.CornflowerBlue;
            this.lblHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHeader.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblHeader.Location = new System.Drawing.Point(-1, -1);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(478, 22);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "User Profile";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseDown);
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // grpUtype
            // 
            this.grpUtype.Controls.Add(this.rbtnAir);
            this.grpUtype.Controls.Add(this.rbtnSurface);
            this.grpUtype.Location = new System.Drawing.Point(135, 103);
            this.grpUtype.Name = "grpUtype";
            this.grpUtype.Size = new System.Drawing.Size(171, 36);
            this.grpUtype.TabIndex = 0;
            this.grpUtype.TabStop = false;
            // 
            // rbtnAir
            // 
            this.rbtnAir.AutoSize = true;
            this.rbtnAir.Checked = true;
            this.rbtnAir.Location = new System.Drawing.Point(6, 11);
            this.rbtnAir.Name = "rbtnAir";
            this.rbtnAir.Size = new System.Drawing.Size(39, 19);
            this.rbtnAir.TabIndex = 0;
            this.rbtnAir.TabStop = true;
            this.rbtnAir.Text = "Air";
            this.rbtnAir.UseVisualStyleBackColor = true;
            this.rbtnAir.CheckedChanged += new System.EventHandler(this.rbtnAir_CheckedChanged);
            // 
            // rbtnSurface
            // 
            this.rbtnSurface.AutoSize = true;
            this.rbtnSurface.Location = new System.Drawing.Point(72, 11);
            this.rbtnSurface.Name = "rbtnSurface";
            this.rbtnSurface.Size = new System.Drawing.Size(67, 19);
            this.rbtnSurface.TabIndex = 1;
            this.rbtnSurface.TabStop = true;
            this.rbtnSurface.Text = "Surface";
            this.rbtnSurface.UseVisualStyleBackColor = true;
            this.rbtnSurface.CheckedChanged += new System.EventHandler(this.rbtnSurface_CheckedChanged);
            // 
            // txtLocId
            // 
            this.txtLocId.Location = new System.Drawing.Point(203, 39);
            this.txtLocId.Name = "txtLocId";
            this.txtLocId.Size = new System.Drawing.Size(66, 21);
            this.txtLocId.TabIndex = 23;
            // 
            // txtType
            // 
            this.txtType.Location = new System.Drawing.Point(235, 39);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(51, 21);
            this.txtType.TabIndex = 109;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // EMLocations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(900, 657);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "EMLocations";
            this.Load += new System.EventHandler(this.EMLocation_Load);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            this.grpUtype.ResumeLayout(false);
            this.grpUtype.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsLocation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.ComboBox cboSponsor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtISO;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtLocationPoint;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtSampleNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.GroupBox grpUtype;
        private System.Windows.Forms.RadioButton rbtnAir;
        private System.Windows.Forms.RadioButton rbtnSurface;
        private System.Windows.Forms.TextBox txtLocId;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.BindingSource bsLocation;
        private System.Windows.Forms.Timer timer1;
    }
}
