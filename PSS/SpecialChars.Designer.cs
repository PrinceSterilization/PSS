namespace PSS
{
    partial class SpecialChars
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvSymbols = new System.Windows.Forms.DataGridView();
            this.CharCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CharDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtChar = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bsSymbols = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSymbols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSymbols)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSymbols
            // 
            this.dgvSymbols.AllowUserToAddRows = false;
            this.dgvSymbols.AllowUserToDeleteRows = false;
            this.dgvSymbols.BackgroundColor = System.Drawing.Color.White;
            this.dgvSymbols.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSymbols.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CharCode,
            this.CharDesc});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSymbols.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSymbols.Location = new System.Drawing.Point(39, 39);
            this.dgvSymbols.Name = "dgvSymbols";
            this.dgvSymbols.ReadOnly = true;
            this.dgvSymbols.Size = new System.Drawing.Size(354, 461);
            this.dgvSymbols.TabIndex = 0;
            this.dgvSymbols.CurrentCellChanged += new System.EventHandler(this.dgvSymbols_Click);
            this.dgvSymbols.Click += new System.EventHandler(this.dgvSymbols_Click);
            // 
            // CharCode
            // 
            this.CharCode.HeaderText = "Character";
            this.CharCode.Name = "CharCode";
            this.CharCode.ReadOnly = true;
            // 
            // CharDesc
            // 
            this.CharDesc.HeaderText = "Description";
            this.CharDesc.Name = "CharDesc";
            this.CharDesc.ReadOnly = true;
            // 
            // txtChar
            // 
            this.txtChar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChar.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChar.Location = new System.Drawing.Point(305, 522);
            this.txtChar.Name = "txtChar";
            this.txtChar.Size = new System.Drawing.Size(88, 44);
            this.txtChar.TabIndex = 2;
            this.txtChar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(160, 539);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Selected Character";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // SpecialChars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(439, 599);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtChar);
            this.Controls.Add(this.dgvSymbols);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpecialChars";
            this.ShowInTaskbar = false;
            this.Text = "SpecialChars";
            this.Load += new System.EventHandler(this.SpecialChars_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSymbols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSymbols)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSymbols;
        private System.Windows.Forms.BindingSource bsSymbols;
        private System.Windows.Forms.DataGridViewTextBoxColumn CharCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CharDesc;
        private System.Windows.Forms.TextBox txtChar;
        private System.Windows.Forms.Label label1;
    }
}