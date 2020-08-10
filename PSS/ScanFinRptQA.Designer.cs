namespace PSS
{
    partial class ScanFinRptQA
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrtCover = new System.Windows.Forms.Button();
            this.btnPrtLabels = new System.Windows.Forms.Button();
            this.txtRptNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvFile = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(776, 401);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 26);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Cl&ose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrtCover
            // 
            this.btnPrtCover.Location = new System.Drawing.Point(669, 401);
            this.btnPrtCover.Name = "btnPrtCover";
            this.btnPrtCover.Size = new System.Drawing.Size(87, 26);
            this.btnPrtCover.TabIndex = 10;
            this.btnPrtCover.Text = "Update Status";
            this.btnPrtCover.UseVisualStyleBackColor = true;
            this.btnPrtCover.Visible = false;
            // 
            // btnPrtLabels
            // 
            this.btnPrtLabels.Location = new System.Drawing.Point(576, 401);
            this.btnPrtLabels.Name = "btnPrtLabels";
            this.btnPrtLabels.Size = new System.Drawing.Size(87, 26);
            this.btnPrtLabels.TabIndex = 9;
            this.btnPrtLabels.Text = "Print &Label";
            this.btnPrtLabels.UseVisualStyleBackColor = true;
            this.btnPrtLabels.Visible = false;
            // 
            // txtRptNo
            // 
            this.txtRptNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRptNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRptNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRptNo.Location = new System.Drawing.Point(93, 30);
            this.txtRptNo.MaxLength = 11;
            this.txtRptNo.Name = "txtRptNo";
            this.txtRptNo.Size = new System.Drawing.Size(99, 21);
            this.txtRptNo.TabIndex = 7;
            this.txtRptNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRptNo_KeyPress);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 19);
            this.label1.TabIndex = 6;
            this.label1.Text = "Report No.";
            // 
            // dgvFile
            // 
            this.dgvFile.AllowUserToAddRows = false;
            this.dgvFile.AllowUserToDeleteRows = false;
            this.dgvFile.AllowUserToOrderColumns = true;
            this.dgvFile.AllowUserToResizeColumns = false;
            this.dgvFile.AllowUserToResizeRows = false;
            this.dgvFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvFile.BackgroundColor = System.Drawing.Color.White;
            this.dgvFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFile.Location = new System.Drawing.Point(27, 57);
            this.dgvFile.Name = "dgvFile";
            this.dgvFile.ReadOnly = true;
            this.dgvFile.RowHeadersWidth = 25;
            this.dgvFile.Size = new System.Drawing.Size(836, 338);
            this.dgvFile.TabIndex = 12;
            // 
            // ScanFinRptQA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 455);
            this.Controls.Add(this.dgvFile);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrtCover);
            this.Controls.Add(this.btnPrtLabels);
            this.Controls.Add(this.txtRptNo);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScanFinRptQA";
            this.Text = "ScanFinRptQA";
            this.Load += new System.EventHandler(this.ScanFinRptQA_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrtCover;
        private System.Windows.Forms.Button btnPrtLabels;
        private System.Windows.Forms.TextBox txtRptNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvFile;
    }
}