namespace GIS
{
    partial class NSManifest
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvCurrent = new System.Windows.Forms.DataGridView();
            this.dgvPrevious = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCurrent = new System.Windows.Forms.TextBox();
            this.txtPrevious = new System.Windows.Forms.TextBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.dgvNoMatchPrevious = new System.Windows.Forms.DataGridView();
            this.FillCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ROWID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvNoMatchCurrent = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtNMPrevious = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNMCurrent = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtUpdated = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.dgvUpdated = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExportCurrent = new System.Windows.Forms.Button();
            this.btnExportPrevious = new System.Windows.Forms.Button();
            this.btnFixX = new System.Windows.Forms.Button();
            this.btnNSCase = new System.Windows.Forms.Button();
            this.bsCurrent = new System.Windows.Forms.BindingSource(this.components);
            this.bsPrevious = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrevious)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNoMatchPrevious)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNoMatchCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpdated)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPrevious)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Teal;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "NEW / CURRENT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Teal;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(23, 356);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 22);
            this.label2.TabIndex = 1;
            this.label2.Text = "CURRENT / PREVIOUS";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvCurrent
            // 
            this.dgvCurrent.AllowUserToAddRows = false;
            this.dgvCurrent.AllowUserToDeleteRows = false;
            this.dgvCurrent.BackgroundColor = System.Drawing.Color.White;
            this.dgvCurrent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCurrent.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvCurrent.Location = new System.Drawing.Point(23, 42);
            this.dgvCurrent.Name = "dgvCurrent";
            this.dgvCurrent.Size = new System.Drawing.Size(1850, 258);
            this.dgvCurrent.TabIndex = 2;
            // 
            // dgvPrevious
            // 
            this.dgvPrevious.AllowUserToAddRows = false;
            this.dgvPrevious.AllowUserToDeleteRows = false;
            this.dgvPrevious.BackgroundColor = System.Drawing.Color.White;
            this.dgvPrevious.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrevious.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPrevious.Location = new System.Drawing.Point(23, 378);
            this.dgvPrevious.Name = "dgvPrevious";
            this.dgvPrevious.Size = new System.Drawing.Size(1850, 258);
            this.dgvPrevious.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DarkRed;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(23, 299);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "TOTAL";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.DarkRed;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(23, 635);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label4.Size = new System.Drawing.Size(74, 21);
            this.label4.TabIndex = 5;
            this.label4.Text = "TOTAL";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCurrent
            // 
            this.txtCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCurrent.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrent.Location = new System.Drawing.Point(97, 299);
            this.txtCurrent.Name = "txtCurrent";
            this.txtCurrent.ReadOnly = true;
            this.txtCurrent.Size = new System.Drawing.Size(55, 21);
            this.txtCurrent.TabIndex = 6;
            this.txtCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPrevious
            // 
            this.txtPrevious.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrevious.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrevious.Location = new System.Drawing.Point(97, 635);
            this.txtPrevious.Name = "txtPrevious";
            this.txtPrevious.ReadOnly = true;
            this.txtPrevious.Size = new System.Drawing.Size(55, 21);
            this.txtPrevious.TabIndex = 7;
            this.txtPrevious.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnCompare
            // 
            this.btnCompare.BackColor = System.Drawing.Color.Moccasin;
            this.btnCompare.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompare.Location = new System.Drawing.Point(1773, 642);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(100, 30);
            this.btnCompare.TabIndex = 8;
            this.btnCompare.Text = "&Compare";
            this.btnCompare.UseVisualStyleBackColor = false;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // dgvNoMatchPrevious
            // 
            this.dgvNoMatchPrevious.AllowUserToAddRows = false;
            this.dgvNoMatchPrevious.AllowUserToDeleteRows = false;
            this.dgvNoMatchPrevious.BackgroundColor = System.Drawing.Color.White;
            this.dgvNoMatchPrevious.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNoMatchPrevious.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FillCode,
            this.ROWID});
            this.dgvNoMatchPrevious.Location = new System.Drawing.Point(1101, 705);
            this.dgvNoMatchPrevious.Name = "dgvNoMatchPrevious";
            this.dgvNoMatchPrevious.ReadOnly = true;
            this.dgvNoMatchPrevious.RowHeadersVisible = false;
            this.dgvNoMatchPrevious.Size = new System.Drawing.Size(203, 88);
            this.dgvNoMatchPrevious.TabIndex = 9;
            // 
            // FillCode
            // 
            this.FillCode.HeaderText = "NO MATCHING FILL CODES";
            this.FillCode.Name = "FillCode";
            this.FillCode.ReadOnly = true;
            this.FillCode.Width = 180;
            // 
            // ROWID
            // 
            this.ROWID.HeaderText = "ROW ID";
            this.ROWID.Name = "ROWID";
            this.ROWID.ReadOnly = true;
            this.ROWID.Visible = false;
            // 
            // dgvNoMatchCurrent
            // 
            this.dgvNoMatchCurrent.AllowUserToAddRows = false;
            this.dgvNoMatchCurrent.AllowUserToDeleteRows = false;
            this.dgvNoMatchCurrent.BackgroundColor = System.Drawing.Color.White;
            this.dgvNoMatchCurrent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNoMatchCurrent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvNoMatchCurrent.Location = new System.Drawing.Point(1327, 705);
            this.dgvNoMatchCurrent.Name = "dgvNoMatchCurrent";
            this.dgvNoMatchCurrent.ReadOnly = true;
            this.dgvNoMatchCurrent.RowHeadersVisible = false;
            this.dgvNoMatchCurrent.Size = new System.Drawing.Size(203, 88);
            this.dgvNoMatchCurrent.TabIndex = 10;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "NO MATCHING FILL CODES";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 180;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "ROW ID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // txtNMPrevious
            // 
            this.txtNMPrevious.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNMPrevious.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNMPrevious.Location = new System.Drawing.Point(1175, 792);
            this.txtNMPrevious.Name = "txtNMPrevious";
            this.txtNMPrevious.ReadOnly = true;
            this.txtNMPrevious.Size = new System.Drawing.Size(55, 21);
            this.txtNMPrevious.TabIndex = 12;
            this.txtNMPrevious.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.DarkRed;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(1101, 792);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label5.Size = new System.Drawing.Size(74, 21);
            this.label5.TabIndex = 11;
            this.label5.Text = "TOTAL";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNMCurrent
            // 
            this.txtNMCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNMCurrent.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNMCurrent.Location = new System.Drawing.Point(1401, 792);
            this.txtNMCurrent.Name = "txtNMCurrent";
            this.txtNMCurrent.ReadOnly = true;
            this.txtNMCurrent.Size = new System.Drawing.Size(55, 21);
            this.txtNMCurrent.TabIndex = 14;
            this.txtNMCurrent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.DarkRed;
            this.label6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(1327, 792);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label6.Size = new System.Drawing.Size(74, 21);
            this.label6.TabIndex = 13;
            this.label6.Text = "TOTAL";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(1098, 688);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "CURRENT vs NEW";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(1324, 688);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 15);
            this.label8.TabIndex = 16;
            this.label8.Text = "NEW vs CURRENT";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(1550, 688);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 15);
            this.label9.TabIndex = 20;
            this.label9.Text = "UPDATED";
            // 
            // txtUpdated
            // 
            this.txtUpdated.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUpdated.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpdated.Location = new System.Drawing.Point(1627, 792);
            this.txtUpdated.Name = "txtUpdated";
            this.txtUpdated.ReadOnly = true;
            this.txtUpdated.Size = new System.Drawing.Size(55, 21);
            this.txtUpdated.TabIndex = 19;
            this.txtUpdated.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.DarkRed;
            this.label10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(1553, 792);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label10.Size = new System.Drawing.Size(74, 21);
            this.label10.TabIndex = 18;
            this.label10.Text = "TOTAL";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvUpdated
            // 
            this.dgvUpdated.AllowUserToAddRows = false;
            this.dgvUpdated.AllowUserToDeleteRows = false;
            this.dgvUpdated.BackgroundColor = System.Drawing.Color.White;
            this.dgvUpdated.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUpdated.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgvUpdated.Location = new System.Drawing.Point(1553, 705);
            this.dgvUpdated.Name = "dgvUpdated";
            this.dgvUpdated.ReadOnly = true;
            this.dgvUpdated.RowHeadersVisible = false;
            this.dgvUpdated.Size = new System.Drawing.Size(203, 88);
            this.dgvUpdated.TabIndex = 17;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "UPDATE FILL CODES";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 180;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "ROW ID";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // btnExportCurrent
            // 
            this.btnExportCurrent.BackColor = System.Drawing.Color.Moccasin;
            this.btnExportCurrent.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportCurrent.Location = new System.Drawing.Point(1773, 6);
            this.btnExportCurrent.Name = "btnExportCurrent";
            this.btnExportCurrent.Size = new System.Drawing.Size(100, 30);
            this.btnExportCurrent.TabIndex = 21;
            this.btnExportCurrent.Text = "Export To Excel";
            this.btnExportCurrent.UseVisualStyleBackColor = false;
            // 
            // btnExportPrevious
            // 
            this.btnExportPrevious.BackColor = System.Drawing.Color.Moccasin;
            this.btnExportPrevious.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportPrevious.Location = new System.Drawing.Point(1773, 342);
            this.btnExportPrevious.Name = "btnExportPrevious";
            this.btnExportPrevious.Size = new System.Drawing.Size(100, 30);
            this.btnExportPrevious.TabIndex = 22;
            this.btnExportPrevious.Text = "Export To Excel";
            this.btnExportPrevious.UseVisualStyleBackColor = false;
            this.btnExportPrevious.Click += new System.EventHandler(this.btnExportPrevious_Click);
            // 
            // btnFixX
            // 
            this.btnFixX.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFixX.Location = new System.Drawing.Point(1775, 306);
            this.btnFixX.Name = "btnFixX";
            this.btnFixX.Size = new System.Drawing.Size(97, 26);
            this.btnFixX.TabIndex = 23;
            this.btnFixX.Text = "Trim \"X\"";
            this.btnFixX.UseVisualStyleBackColor = true;
            this.btnFixX.Click += new System.EventHandler(this.btnFixX_Click);
            // 
            // btnNSCase
            // 
            this.btnNSCase.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNSCase.Location = new System.Drawing.Point(1672, 306);
            this.btnNSCase.Name = "btnNSCase";
            this.btnNSCase.Size = new System.Drawing.Size(97, 26);
            this.btnNSCase.TabIndex = 24;
            this.btnNSCase.Text = "NS Case";
            this.btnNSCase.UseVisualStyleBackColor = true;
            this.btnNSCase.Click += new System.EventHandler(this.btnNSCase_Click);
            // 
            // Manifest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1897, 838);
            this.Controls.Add(this.btnNSCase);
            this.Controls.Add(this.btnFixX);
            this.Controls.Add(this.btnExportPrevious);
            this.Controls.Add(this.btnExportCurrent);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtUpdated);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dgvUpdated);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtNMCurrent);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtNMPrevious);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgvNoMatchCurrent);
            this.Controls.Add(this.dgvNoMatchPrevious);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.txtPrevious);
            this.Controls.Add(this.txtCurrent);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dgvPrevious);
            this.Controls.Add(this.dgvCurrent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Manifest";
            this.Tag = "Manifest";
            this.Text = "Manifest";
            this.Load += new System.EventHandler(this.Manifest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrevious)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNoMatchPrevious)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNoMatchCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUpdated)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPrevious)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvCurrent;
        private System.Windows.Forms.DataGridView dgvPrevious;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCurrent;
        private System.Windows.Forms.TextBox txtPrevious;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.BindingSource bsCurrent;
        private System.Windows.Forms.BindingSource bsPrevious;
        private System.Windows.Forms.DataGridView dgvNoMatchPrevious;
        private System.Windows.Forms.DataGridViewTextBoxColumn FillCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ROWID;
        private System.Windows.Forms.DataGridView dgvNoMatchCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.TextBox txtNMPrevious;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNMCurrent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtUpdated;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvUpdated;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.Button btnExportCurrent;
        private System.Windows.Forms.Button btnExportPrevious;
        private System.Windows.Forms.Button btnFixX;
        private System.Windows.Forms.Button btnNSCase;
    }
}