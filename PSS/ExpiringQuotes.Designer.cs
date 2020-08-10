namespace PSS
{
    partial class ExpiringQuotes
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
            this.dgvReissue = new System.Windows.Forms.DataGridView();
            this.btnPrintExcList = new System.Windows.Forms.Button();
            this.btnPrintCurrent = new System.Windows.Forms.Button();
            this.txtYear = new GISControls.TextBoxChar();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPreview = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReissue)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvReissue
            // 
            this.dgvReissue.AllowUserToAddRows = false;
            this.dgvReissue.AllowUserToDeleteRows = false;
            this.dgvReissue.AllowUserToResizeColumns = false;
            this.dgvReissue.AllowUserToResizeRows = false;
            this.dgvReissue.BackgroundColor = System.Drawing.Color.White;
            this.dgvReissue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReissue.Location = new System.Drawing.Point(13, 39);
            this.dgvReissue.Name = "dgvReissue";
            this.dgvReissue.Size = new System.Drawing.Size(760, 475);
            this.dgvReissue.TabIndex = 15;
            this.dgvReissue.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvReissue_CellBeginEdit);
            this.dgvReissue.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReissue_CellDoubleClick);
            // 
            // btnPrintExcList
            // 
            this.btnPrintExcList.Location = new System.Drawing.Point(181, 530);
            this.btnPrintExcList.Name = "btnPrintExcList";
            this.btnPrintExcList.Size = new System.Drawing.Size(106, 30);
            this.btnPrintExcList.TabIndex = 18;
            this.btnPrintExcList.Text = "Print Excluded List";
            this.btnPrintExcList.UseVisualStyleBackColor = true;
            this.btnPrintExcList.Visible = false;
            // 
            // btnPrintCurrent
            // 
            this.btnPrintCurrent.Location = new System.Drawing.Point(293, 530);
            this.btnPrintCurrent.Name = "btnPrintCurrent";
            this.btnPrintCurrent.Size = new System.Drawing.Size(106, 30);
            this.btnPrintCurrent.TabIndex = 17;
            this.btnPrintCurrent.Text = "Print Current List";
            this.btnPrintCurrent.UseVisualStyleBackColor = true;
            this.btnPrintCurrent.Visible = false;
            // 
            // txtYear
            // 
            this.txtYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYear.Location = new System.Drawing.Point(48, 13);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(45, 20);
            this.txtYear.TabIndex = 20;
            this.txtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtYear.TextChanged += new System.EventHandler(this.txtYear_TextChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.AliceBlue;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "Year";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(667, 520);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(106, 30);
            this.btnPreview.TabIndex = 21;
            this.btnPreview.Text = "Preview Quote";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // ExpiringQuotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 572);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPrintExcList);
            this.Controls.Add(this.btnPrintCurrent);
            this.Controls.Add(this.dgvReissue);
            this.Name = "ExpiringQuotes";
            this.Text = "ExpiringQuotes";
            this.Activated += new System.EventHandler(this.ExpiringQuotes_Activated);
            this.Load += new System.EventHandler(this.ExpiringQuotes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReissue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReissue;
        private System.Windows.Forms.Button btnPrintExcList;
        private System.Windows.Forms.Button btnPrintCurrent;
        private GISControls.TextBoxChar txtYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPreview;
    }
}