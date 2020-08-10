namespace GIS
{
    partial class BDSReports
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
            this.dgvFile = new System.Windows.Forms.DataGridView();
            this.bsFile = new System.Windows.Forms.BindingSource(this.components);
            this.dtpToMain = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.lnkFile = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFile
            // 
            this.dgvFile.AllowUserToAddRows = false;
            this.dgvFile.AllowUserToDeleteRows = false;
            this.dgvFile.BackgroundColor = System.Drawing.Color.White;
            this.dgvFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFile.Location = new System.Drawing.Point(35, 74);
            this.dgvFile.Name = "dgvFile";
            this.dgvFile.RowHeadersWidth = 20;
            this.dgvFile.Size = new System.Drawing.Size(1097, 406);
            this.dgvFile.TabIndex = 0;
            this.dgvFile.CurrentCellChanged += new System.EventHandler(this.dgvFile_CurrentCellChanged);
            // 
            // dtpToMain
            // 
            this.dtpToMain.CustomFormat = "MM/dd/yyyy";
            this.dtpToMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToMain.Location = new System.Drawing.Point(72, 51);
            this.dtpToMain.Name = "dtpToMain";
            this.dtpToMain.Size = new System.Drawing.Size(98, 20);
            this.dtpToMain.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(32, 51);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 19);
            this.label10.TabIndex = 390;
            this.label10.Text = "Date:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(1042, 37);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 31);
            this.btnRefresh.TabIndex = 391;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(1042, 489);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(90, 31);
            this.btnUpload.TabIndex = 392;
            this.btnUpload.Text = "&Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // lnkFile
            // 
            this.lnkFile.AutoSize = true;
            this.lnkFile.Location = new System.Drawing.Point(32, 498);
            this.lnkFile.Name = "lnkFile";
            this.lnkFile.Size = new System.Drawing.Size(58, 13);
            this.lnkFile.TabIndex = 393;
            this.lnkFile.TabStop = true;
            this.lnkFile.Text = "Report File";
            this.lnkFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFile_LinkClicked);
            // 
            // BDSReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 556);
            this.Controls.Add(this.lnkFile);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dtpToMain);
            this.Controls.Add(this.dgvFile);
            this.Name = "BDSReports";
            this.Text = "BDSReports";
            this.Load += new System.EventHandler(this.BDSReports_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFile;
        private System.Windows.Forms.BindingSource bsFile;
        private System.Windows.Forms.DateTimePicker dtpToMain;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.LinkLabel lnkFile;
    }
}