namespace GIS
{
    partial class SOPCheck
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
            this.btnProceed = new System.Windows.Forms.Button();
            this.dgvSOP = new System.Windows.Forms.DataGridView();
            this.colSOP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSOPFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSOP)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProceed
            // 
            this.btnProceed.Location = new System.Drawing.Point(42, 37);
            this.btnProceed.Name = "btnProceed";
            this.btnProceed.Size = new System.Drawing.Size(153, 38);
            this.btnProceed.TabIndex = 0;
            this.btnProceed.Text = "&Proceed";
            this.btnProceed.UseVisualStyleBackColor = true;
            this.btnProceed.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // dgvSOP
            // 
            this.dgvSOP.AllowUserToDeleteRows = false;
            this.dgvSOP.BackgroundColor = System.Drawing.Color.White;
            this.dgvSOP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSOP.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSOP,
            this.colSOPFileName});
            this.dgvSOP.Location = new System.Drawing.Point(42, 94);
            this.dgvSOP.Name = "dgvSOP";
            this.dgvSOP.ReadOnly = true;
            this.dgvSOP.Size = new System.Drawing.Size(602, 341);
            this.dgvSOP.TabIndex = 1;
            // 
            // colSOP
            // 
            this.colSOP.HeaderText = "DEFAULT FILE NAME";
            this.colSOP.Name = "colSOP";
            this.colSOP.ReadOnly = true;
            // 
            // colSOPFileName
            // 
            this.colSOPFileName.HeaderText = "ACTUAL FILE NAME";
            this.colSOPFileName.Name = "colSOPFileName";
            this.colSOPFileName.ReadOnly = true;
            // 
            // SOPCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 480);
            this.Controls.Add(this.dgvSOP);
            this.Controls.Add(this.btnProceed);
            this.Name = "SOPCheck";
            this.Text = "SOPCheck";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSOP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProceed;
        private System.Windows.Forms.DataGridView dgvSOP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSOP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSOPFileName;
    }
}