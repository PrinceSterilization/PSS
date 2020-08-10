namespace PSS
{
    partial class POBalance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POBalance));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtSponsorID = new GISControls.TextBoxChar();
            this.picSponsors = new System.Windows.Forms.PictureBox();
            this.txtSponsor = new GISControls.TextBoxChar();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvSponsors = new System.Windows.Forms.DataGridView();
            this.label7 = new System.Windows.Forms.Label();
            this.cboPO = new System.Windows.Forms.ComboBox();
            this.btnCancelPrint = new System.Windows.Forms.Button();
            this.btnOKPrint = new System.Windows.Forms.Button();
            this.rdoRegular = new System.Windows.Forms.RadioButton();
            this.rdoPreBilled = new System.Windows.Forms.RadioButton();
            this.btnPOBalSum = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSponsorID
            // 
            this.txtSponsorID.BackColor = System.Drawing.Color.White;
            this.txtSponsorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSponsorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSponsorID.Location = new System.Drawing.Point(127, 50);
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
            this.picSponsors.Location = new System.Drawing.Point(570, 50);
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
            this.txtSponsor.Location = new System.Drawing.Point(200, 50);
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
            this.label5.Location = new System.Drawing.Point(16, 52);
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
            this.dgvSponsors.Location = new System.Drawing.Point(200, 70);
            this.dgvSponsors.Name = "dgvSponsors";
            this.dgvSponsors.ReadOnly = true;
            this.dgvSponsors.RowHeadersVisible = false;
            this.dgvSponsors.Size = new System.Drawing.Size(389, 79);
            this.dgvSponsors.TabIndex = 87;
            this.dgvSponsors.Visible = false;
            this.dgvSponsors.DoubleClick += new System.EventHandler(this.dgvSponsors_DoubleClick);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(15, 77);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(2);
            this.label7.Size = new System.Drawing.Size(111, 21);
            this.label7.TabIndex = 89;
            this.label7.Text = "PO No.";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPO
            // 
            this.cboPO.FormattingEnabled = true;
            this.cboPO.Location = new System.Drawing.Point(127, 73);
            this.cboPO.Name = "cboPO";
            this.cboPO.Size = new System.Drawing.Size(252, 21);
            this.cboPO.TabIndex = 90;
            // 
            // btnCancelPrint
            // 
            this.btnCancelPrint.Location = new System.Drawing.Point(517, 115);
            this.btnCancelPrint.Name = "btnCancelPrint";
            this.btnCancelPrint.Size = new System.Drawing.Size(72, 27);
            this.btnCancelPrint.TabIndex = 92;
            this.btnCancelPrint.Text = "Cl&ose";
            this.btnCancelPrint.UseVisualStyleBackColor = true;
            this.btnCancelPrint.Click += new System.EventHandler(this.btnCancelPrint_Click);
            // 
            // btnOKPrint
            // 
            this.btnOKPrint.Location = new System.Drawing.Point(439, 115);
            this.btnOKPrint.Name = "btnOKPrint";
            this.btnOKPrint.Size = new System.Drawing.Size(72, 27);
            this.btnOKPrint.TabIndex = 91;
            this.btnOKPrint.Text = "O&K";
            this.btnOKPrint.UseVisualStyleBackColor = true;
            this.btnOKPrint.Click += new System.EventHandler(this.btnOKPrint_Click);
            // 
            // rdoRegular
            // 
            this.rdoRegular.AutoSize = true;
            this.rdoRegular.Location = new System.Drawing.Point(209, 100);
            this.rdoRegular.Name = "rdoRegular";
            this.rdoRegular.Size = new System.Drawing.Size(92, 17);
            this.rdoRegular.TabIndex = 93;
            this.rdoRegular.TabStop = true;
            this.rdoRegular.Text = "Regular Billing";
            this.rdoRegular.UseVisualStyleBackColor = true;
            // 
            // rdoPreBilled
            // 
            this.rdoPreBilled.AutoSize = true;
            this.rdoPreBilled.Checked = true;
            this.rdoPreBilled.Location = new System.Drawing.Point(127, 100);
            this.rdoPreBilled.Name = "rdoPreBilled";
            this.rdoPreBilled.Size = new System.Drawing.Size(69, 17);
            this.rdoPreBilled.TabIndex = 94;
            this.rdoPreBilled.TabStop = true;
            this.rdoPreBilled.Text = "Pre-Billed";
            this.rdoPreBilled.UseVisualStyleBackColor = true;
            // 
            // btnPOBalSum
            // 
            this.btnPOBalSum.Location = new System.Drawing.Point(423, 17);
            this.btnPOBalSum.Name = "btnPOBalSum";
            this.btnPOBalSum.Size = new System.Drawing.Size(166, 27);
            this.btnPOBalSum.TabIndex = 95;
            this.btnPOBalSum.Text = "&Generate PO Balances Report";
            this.btnPOBalSum.UseVisualStyleBackColor = true;
            this.btnPOBalSum.Click += new System.EventHandler(this.btnPOBalSum_Click);
            // 
            // POBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(604, 161);
            this.Controls.Add(this.btnPOBalSum);
            this.Controls.Add(this.dgvSponsors);
            this.Controls.Add(this.rdoPreBilled);
            this.Controls.Add(this.rdoRegular);
            this.Controls.Add(this.btnCancelPrint);
            this.Controls.Add(this.btnOKPrint);
            this.Controls.Add(this.cboPO);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSponsorID);
            this.Controls.Add(this.picSponsors);
            this.Controls.Add(this.txtSponsor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "POBalance";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "a";
            this.Load += new System.EventHandler(this.POBalance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picSponsors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSponsors)).EndInit();
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
        private System.Windows.Forms.Button btnCancelPrint;
        private System.Windows.Forms.Button btnOKPrint;
        private System.Windows.Forms.RadioButton rdoRegular;
        private System.Windows.Forms.RadioButton rdoPreBilled;
        private System.Windows.Forms.Button btnPOBalSum;
    }
}