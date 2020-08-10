namespace GIS
{
    partial class UserReminders
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvReminders = new System.Windows.Forms.DataGridView();
            this.ItemDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewLinkColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvEmpOnWL = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReminders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpOnWL)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvReminders
            // 
            this.dgvReminders.AllowUserToAddRows = false;
            this.dgvReminders.AllowUserToDeleteRows = false;
            this.dgvReminders.AllowUserToResizeColumns = false;
            this.dgvReminders.AllowUserToResizeRows = false;
            this.dgvReminders.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReminders.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReminders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReminders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemDesc,
            this.Total});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReminders.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvReminders.EnableHeadersVisualStyles = false;
            this.dgvReminders.Location = new System.Drawing.Point(32, 33);
            this.dgvReminders.Name = "dgvReminders";
            this.dgvReminders.ReadOnly = true;
            this.dgvReminders.RowHeadersWidth = 20;
            this.dgvReminders.Size = new System.Drawing.Size(444, 143);
            this.dgvReminders.TabIndex = 3;
            this.dgvReminders.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReminders_CellContentClick);
            // 
            // ItemDesc
            // 
            this.ItemDesc.HeaderText = "DESCRIPTION";
            this.ItemDesc.MinimumWidth = 300;
            this.ItemDesc.Name = "ItemDesc";
            this.ItemDesc.ReadOnly = true;
            this.ItemDesc.Width = 300;
            // 
            // Total
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Total.DefaultCellStyle = dataGridViewCellStyle2;
            this.Total.HeaderText = "TOTAL";
            this.Total.MinimumWidth = 100;
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(29, 196);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Employees on Work Leave:";
            // 
            // dgvEmpOnWL
            // 
            this.dgvEmpOnWL.AllowUserToAddRows = false;
            this.dgvEmpOnWL.AllowUserToDeleteRows = false;
            this.dgvEmpOnWL.AllowUserToOrderColumns = true;
            this.dgvEmpOnWL.AllowUserToResizeColumns = false;
            this.dgvEmpOnWL.AllowUserToResizeRows = false;
            this.dgvEmpOnWL.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.SteelBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEmpOnWL.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvEmpOnWL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvEmpOnWL.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvEmpOnWL.EnableHeadersVisualStyles = false;
            this.dgvEmpOnWL.Location = new System.Drawing.Point(32, 214);
            this.dgvEmpOnWL.MaximumSize = new System.Drawing.Size(444, 150);
            this.dgvEmpOnWL.MinimumSize = new System.Drawing.Size(444, 150);
            this.dgvEmpOnWL.Name = "dgvEmpOnWL";
            this.dgvEmpOnWL.ReadOnly = true;
            this.dgvEmpOnWL.RowHeadersWidth = 20;
            this.dgvEmpOnWL.Size = new System.Drawing.Size(444, 150);
            this.dgvEmpOnWL.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(389, 370);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 33);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Cl&ose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // UserReminders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 426);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgvEmpOnWL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvReminders);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(523, 464);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(523, 464);
            this.Name = "UserReminders";
            this.Text = "REMINDERS";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.UserReminders_Activated);
            this.Deactivate += new System.EventHandler(this.UserReminders_Deactivate);
            this.Load += new System.EventHandler(this.UserReminders_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReminders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpOnWL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvReminders;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDesc;
        private System.Windows.Forms.DataGridViewLinkColumn Total;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvEmpOnWL;
        private System.Windows.Forms.Button btnClose;
    }
}