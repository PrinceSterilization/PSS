namespace PSS
{
    partial class FollowupsExcluded
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
            this.txtBody = new GISControls.TextBoxChar();
            this.dgvQuotesExcluded = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuotesExcluded)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.txtBody);
            this.pnlRecord.Controls.Add(this.dgvQuotesExcluded);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Location = new System.Drawing.Point(145, 75);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(978, 629);
            this.pnlRecord.TabIndex = 106;
            // 
            // txtBody
            // 
            this.txtBody.BackColor = System.Drawing.Color.White;
            this.txtBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBody.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBody.ForeColor = System.Drawing.Color.Red;
            this.txtBody.Location = new System.Drawing.Point(27, 24);
            this.txtBody.MaxLength = 5;
            this.txtBody.Name = "txtBody";
            this.txtBody.Size = new System.Drawing.Size(32, 21);
            this.txtBody.TabIndex = 185;
            this.txtBody.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBody.Visible = false;
            // 
            // dgvQuotesExcluded
            // 
            this.dgvQuotesExcluded.AllowUserToAddRows = false;
            this.dgvQuotesExcluded.AllowUserToDeleteRows = false;
            this.dgvQuotesExcluded.AllowUserToResizeColumns = false;
            this.dgvQuotesExcluded.BackgroundColor = System.Drawing.Color.White;
            this.dgvQuotesExcluded.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQuotesExcluded.Location = new System.Drawing.Point(27, 66);
            this.dgvQuotesExcluded.Name = "dgvQuotesExcluded";
            this.dgvQuotesExcluded.Size = new System.Drawing.Size(922, 537);
            this.dgvQuotesExcluded.TabIndex = 8;
            this.dgvQuotesExcluded.DoubleClick += new System.EventHandler(this.dgvQuotesExcluded_DoubleClick);
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
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(980, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "NO FOLLOW-UP QUOTES";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FollowupsExcluded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1676, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FollowupsExcluded";
            this.Tag = "FollowupsExcluded";
            this.Text = "Follow-ups Exclusion List";
            this.Load += new System.EventHandler(this.FollowupsExcluded_Load);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvQuotesExcluded)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private GISControls.TextBoxChar txtBody;
        private System.Windows.Forms.DataGridView dgvQuotesExcluded;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblHeader;
    }
}
