namespace GIS
{
    partial class IngredionSpclBatch
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
            this.cboFillCodes = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.txtBags = new GISControls.TextBoxChar();
            this.label56 = new System.Windows.Forms.Label();
            this.txtLotNo = new GISControls.TextBoxChar();
            this.cboSponsorID = new System.Windows.Forms.ComboBox();
            this.cboContacts = new System.Windows.Forms.ComboBox();
            this.cboSponsors = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtQuoteNo = new GISControls.TextBoxChar();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCompBag = new GISControls.TextBoxChar();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPONo = new GISControls.TextBoxChar();
            this.btnCancelPrint = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBookNo = new GISControls.TextBoxChar();
            this.txtSSFormNo = new GISControls.TextBoxChar();
            this.label19 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboFillCodes
            // 
            this.cboFillCodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFillCodes.FormattingEnabled = true;
            this.cboFillCodes.Items.AddRange(new object[] {
            "503130",
            "503136",
            "501116",
            "501901",
            "501340",
            "501803"});
            this.cboFillCodes.Location = new System.Drawing.Point(94, 13);
            this.cboFillCodes.Name = "cboFillCodes";
            this.cboFillCodes.Size = new System.Drawing.Size(68, 21);
            this.cboFillCodes.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fill Code";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label58
            // 
            this.label58.Location = new System.Drawing.Point(15, 109);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(76, 18);
            this.label58.TabIndex = 152;
            this.label58.Text = "No. of Bags";
            // 
            // txtBags
            // 
            this.txtBags.BackColor = System.Drawing.Color.White;
            this.txtBags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBags.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBags.Location = new System.Drawing.Point(94, 106);
            this.txtBags.Name = "txtBags";
            this.txtBags.Size = new System.Drawing.Size(34, 21);
            this.txtBags.TabIndex = 5;
            // 
            // label56
            // 
            this.label56.Location = new System.Drawing.Point(15, 86);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(61, 19);
            this.label56.TabIndex = 151;
            this.label56.Text = "Lot No.";
            // 
            // txtLotNo
            // 
            this.txtLotNo.BackColor = System.Drawing.Color.White;
            this.txtLotNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLotNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLotNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLotNo.Location = new System.Drawing.Point(94, 83);
            this.txtLotNo.MaxLength = 50;
            this.txtLotNo.Name = "txtLotNo";
            this.txtLotNo.Size = new System.Drawing.Size(106, 21);
            this.txtLotNo.TabIndex = 4;
            // 
            // cboSponsorID
            // 
            this.cboSponsorID.FormattingEnabled = true;
            this.cboSponsorID.Location = new System.Drawing.Point(94, 36);
            this.cboSponsorID.Name = "cboSponsorID";
            this.cboSponsorID.Size = new System.Drawing.Size(67, 21);
            this.cboSponsorID.TabIndex = 1;
            this.cboSponsorID.SelectedIndexChanged += new System.EventHandler(this.cboSponsorID_SelectedIndexChanged);
            // 
            // cboContacts
            // 
            this.cboContacts.FormattingEnabled = true;
            this.cboContacts.Location = new System.Drawing.Point(94, 60);
            this.cboContacts.Name = "cboContacts";
            this.cboContacts.Size = new System.Drawing.Size(336, 21);
            this.cboContacts.TabIndex = 3;
            this.cboContacts.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboContacts_KeyPress);
            // 
            // cboSponsors
            // 
            this.cboSponsors.FormattingEnabled = true;
            this.cboSponsors.Location = new System.Drawing.Point(163, 36);
            this.cboSponsors.Name = "cboSponsors";
            this.cboSponsors.Size = new System.Drawing.Size(267, 21);
            this.cboSponsors.TabIndex = 2;
            this.cboSponsors.SelectedIndexChanged += new System.EventHandler(this.cboSponsors_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(15, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 19);
            this.label2.TabIndex = 300;
            this.label2.Text = "Contact";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(15, 39);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 19);
            this.label3.TabIndex = 299;
            this.label3.Text = "Sponsor";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(264, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 305;
            this.label4.Text = "Quote No.";
            // 
            // txtQuoteNo
            // 
            this.txtQuoteNo.BackColor = System.Drawing.Color.White;
            this.txtQuoteNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtQuoteNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuoteNo.Location = new System.Drawing.Point(331, 83);
            this.txtQuoteNo.MaxLength = 50;
            this.txtQuoteNo.Name = "txtQuoteNo";
            this.txtQuoteNo.ReadOnly = true;
            this.txtQuoteNo.Size = new System.Drawing.Size(99, 21);
            this.txtQuoteNo.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(15, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 18);
            this.label5.TabIndex = 307;
            this.label5.Text = "Composite Bag";
            // 
            // txtCompBag
            // 
            this.txtCompBag.BackColor = System.Drawing.Color.White;
            this.txtCompBag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCompBag.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCompBag.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCompBag.Location = new System.Drawing.Point(94, 129);
            this.txtCompBag.MaxLength = 50;
            this.txtCompBag.Name = "txtCompBag";
            this.txtCompBag.Size = new System.Drawing.Size(90, 21);
            this.txtCompBag.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(264, 108);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 19);
            this.label6.TabIndex = 309;
            this.label6.Text = "PO No.";
            // 
            // txtPONo
            // 
            this.txtPONo.BackColor = System.Drawing.Color.White;
            this.txtPONo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPONo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPONo.Location = new System.Drawing.Point(331, 106);
            this.txtPONo.MaxLength = 50;
            this.txtPONo.Name = "txtPONo";
            this.txtPONo.Size = new System.Drawing.Size(99, 21);
            this.txtPONo.TabIndex = 8;
            // 
            // btnCancelPrint
            // 
            this.btnCancelPrint.Location = new System.Drawing.Point(393, 213);
            this.btnCancelPrint.Name = "btnCancelPrint";
            this.btnCancelPrint.Size = new System.Drawing.Size(72, 27);
            this.btnCancelPrint.TabIndex = 311;
            this.btnCancelPrint.Text = "Ca&ncel";
            this.btnCancelPrint.UseVisualStyleBackColor = true;
            this.btnCancelPrint.Click += new System.EventHandler(this.btnCancelPrint_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(315, 213);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 27);
            this.btnOK.TabIndex = 310;
            this.btnOK.Text = "O&K";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOKPrint_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(264, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 16);
            this.label7.TabIndex = 313;
            this.label7.Text = "Book No.";
            // 
            // txtBookNo
            // 
            this.txtBookNo.BackColor = System.Drawing.Color.White;
            this.txtBookNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBookNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBookNo.Location = new System.Drawing.Point(331, 129);
            this.txtBookNo.MaxLength = 50;
            this.txtBookNo.Name = "txtBookNo";
            this.txtBookNo.Size = new System.Drawing.Size(99, 21);
            this.txtBookNo.TabIndex = 9;
            // 
            // txtSSFormNo
            // 
            this.txtSSFormNo.BackColor = System.Drawing.Color.White;
            this.txtSSFormNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSSFormNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSSFormNo.Location = new System.Drawing.Point(158, 17);
            this.txtSSFormNo.Name = "txtSSFormNo";
            this.txtSSFormNo.Size = new System.Drawing.Size(78, 21);
            this.txtSSFormNo.TabIndex = 314;
            this.txtSSFormNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSSFormNo_KeyPress);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(18, 20);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(134, 13);
            this.label19.TabIndex = 315;
            this.label19.Text = "Sample Submission Form #";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cboSponsorID);
            this.panel1.Controls.Add(this.cboFillCodes);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtLotNo);
            this.panel1.Controls.Add(this.txtBookNo);
            this.panel1.Controls.Add(this.label56);
            this.panel1.Controls.Add(this.txtBags);
            this.panel1.Controls.Add(this.label58);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtPONo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cboSponsors);
            this.panel1.Controls.Add(this.txtCompBag);
            this.panel1.Controls.Add(this.cboContacts);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtQuoteNo);
            this.panel1.Location = new System.Drawing.Point(21, 44);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(444, 163);
            this.panel1.TabIndex = 316;
            // 
            // IngredionSpclBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 257);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtSSFormNo);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.btnCancelPrint);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(503, 295);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(503, 295);
            this.Name = "IngredionSpclBatch";
            this.Tag = "IngredionSpclBatch";
            this.Text = "INGREDION SPECIAL BATCH ENTRY";
            this.Load += new System.EventHandler(this.IngredionSpclBatch_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboFillCodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label58;
        private GISControls.TextBoxChar txtBags;
        private System.Windows.Forms.Label label56;
        private GISControls.TextBoxChar txtLotNo;
        private System.Windows.Forms.ComboBox cboSponsorID;
        private System.Windows.Forms.ComboBox cboContacts;
        private System.Windows.Forms.ComboBox cboSponsors;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private GISControls.TextBoxChar txtQuoteNo;
        private System.Windows.Forms.Label label5;
        private GISControls.TextBoxChar txtCompBag;
        private System.Windows.Forms.Label label6;
        private GISControls.TextBoxChar txtPONo;
        private System.Windows.Forms.Button btnCancelPrint;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label7;
        private GISControls.TextBoxChar txtBookNo;
        private GISControls.TextBoxChar txtSSFormNo;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel panel1;
    }
}