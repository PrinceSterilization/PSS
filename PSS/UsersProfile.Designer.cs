namespace PSS
{
    partial class UsersProfile
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.cboEmployees = new System.Windows.Forms.ComboBox();
            this.chkInactive = new System.Windows.Forms.CheckBox();
            this.dgvSAccess = new System.Windows.Forms.DataGridView();
            this.txtGroupID = new System.Windows.Forms.TextBox();
            this.cboUserGroups = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rbtnPUser = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpUtype = new System.Windows.Forms.GroupBox();
            this.rbtnEmployee = new System.Windows.Forms.RadioButton();
            this.rbtnGuest = new System.Windows.Forms.RadioButton();
            this.dgvPAccess = new System.Windows.Forms.DataGridView();
            this.txtLoginID = new System.Windows.Forms.TextBox();
            this.grpAccess = new System.Windows.Forms.GroupBox();
            this.rbtnDUser = new System.Windows.Forms.RadioButton();
            this.rbtnAdministrator = new System.Windows.Forms.RadioButton();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtAccessLevel = new System.Windows.Forms.TextBox();
            this.txtUserType = new System.Windows.Forms.TextBox();
            this.bsUsers = new System.Windows.Forms.BindingSource(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSAccess)).BeginInit();
            this.grpUtype.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPAccess)).BeginInit();
            this.grpAccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // chkShowInactive
            // 
            this.chkShowInactive.Click += new System.EventHandler(this.chkShowInactive_Click);
            // 
            // cklColumns
            // 
            this.cklColumns.Size = new System.Drawing.Size(122, 196);
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.cboEmployees);
            this.pnlRecord.Controls.Add(this.chkInactive);
            this.pnlRecord.Controls.Add(this.dgvSAccess);
            this.pnlRecord.Controls.Add(this.txtGroupID);
            this.pnlRecord.Controls.Add(this.cboUserGroups);
            this.pnlRecord.Controls.Add(this.label2);
            this.pnlRecord.Controls.Add(this.txtUserName);
            this.pnlRecord.Controls.Add(this.label1);
            this.pnlRecord.Controls.Add(this.rbtnPUser);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.grpUtype);
            this.pnlRecord.Controls.Add(this.dgvPAccess);
            this.pnlRecord.Controls.Add(this.txtLoginID);
            this.pnlRecord.Controls.Add(this.grpAccess);
            this.pnlRecord.Controls.Add(this.txtUserID);
            this.pnlRecord.Controls.Add(this.label7);
            this.pnlRecord.Controls.Add(this.label5);
            this.pnlRecord.Controls.Add(this.label6);
            this.pnlRecord.Controls.Add(this.label3);
            this.pnlRecord.Controls.Add(this.label4);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.txtAccessLevel);
            this.pnlRecord.Controls.Add(this.txtUserType);
            this.pnlRecord.ImeMode = System.Windows.Forms.ImeMode.Close;
            this.pnlRecord.Location = new System.Drawing.Point(12, 91);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(480, 500);
            this.pnlRecord.TabIndex = 108;
            this.pnlRecord.Visible = false;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // cboEmployees
            // 
            this.cboEmployees.FormattingEnabled = true;
            this.cboEmployees.Location = new System.Drawing.Point(129, 68);
            this.cboEmployees.Name = "cboEmployees";
            this.cboEmployees.Size = new System.Drawing.Size(220, 23);
            this.cboEmployees.TabIndex = 420;
            this.cboEmployees.Visible = false;
            this.cboEmployees.SelectedIndexChanged += new System.EventHandler(this.cboEmployees_SelectedIndexChanged);
            // 
            // chkInactive
            // 
            this.chkInactive.AutoSize = true;
            this.chkInactive.Location = new System.Drawing.Point(355, 43);
            this.chkInactive.Name = "chkInactive";
            this.chkInactive.Size = new System.Drawing.Size(67, 19);
            this.chkInactive.TabIndex = 419;
            this.chkInactive.Text = "Inactive";
            this.chkInactive.UseVisualStyleBackColor = true;
            this.chkInactive.CheckedChanged += new System.EventHandler(this.chkInactive_CheckedChanged);
            // 
            // dgvSAccess
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Bisque;
            this.dgvSAccess.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSAccess.BackgroundColor = System.Drawing.Color.White;
            this.dgvSAccess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSAccess.Location = new System.Drawing.Point(19, 216);
            this.dgvSAccess.Name = "dgvSAccess";
            this.dgvSAccess.ReadOnly = true;
            this.dgvSAccess.Size = new System.Drawing.Size(441, 264);
            this.dgvSAccess.TabIndex = 4;
            this.dgvSAccess.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvSAccess_CurrentCellDirtyStateChanged);
            this.dgvSAccess.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSAccess_DataError);
            // 
            // txtGroupID
            // 
            this.txtGroupID.Location = new System.Drawing.Point(407, 216);
            this.txtGroupID.Name = "txtGroupID";
            this.txtGroupID.Size = new System.Drawing.Size(32, 21);
            this.txtGroupID.TabIndex = 418;
            // 
            // cboUserGroups
            // 
            this.cboUserGroups.FormattingEnabled = true;
            this.cboUserGroups.Location = new System.Drawing.Point(130, 154);
            this.cboUserGroups.Name = "cboUserGroups";
            this.cboUserGroups.Size = new System.Drawing.Size(219, 23);
            this.cboUserGroups.TabIndex = 417;
            this.cboUserGroups.SelectedIndexChanged += new System.EventHandler(this.cboUserGroups_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 15);
            this.label2.TabIndex = 416;
            this.label2.Text = "Group Membership";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(129, 70);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(220, 21);
            this.txtUserName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Firebrick;
            this.label1.Location = new System.Drawing.Point(17, 352);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 15);
            this.label1.TabIndex = 415;
            this.label1.Text = "Printer Access";
            this.label1.Visible = false;
            // 
            // rbtnPUser
            // 
            this.rbtnPUser.AutoSize = true;
            this.rbtnPUser.Location = new System.Drawing.Point(129, 183);
            this.rbtnPUser.Name = "rbtnPUser";
            this.rbtnPUser.Size = new System.Drawing.Size(90, 19);
            this.rbtnPUser.TabIndex = 1;
            this.rbtnPUser.TabStop = true;
            this.rbtnPUser.Text = "Power User";
            this.rbtnPUser.UseVisualStyleBackColor = true;
            this.rbtnPUser.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(407, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 22);
            this.btnClose.TabIndex = 414;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpUtype
            // 
            this.grpUtype.Controls.Add(this.rbtnEmployee);
            this.grpUtype.Controls.Add(this.rbtnGuest);
            this.grpUtype.Location = new System.Drawing.Point(130, 30);
            this.grpUtype.Name = "grpUtype";
            this.grpUtype.Size = new System.Drawing.Size(219, 35);
            this.grpUtype.TabIndex = 0;
            this.grpUtype.TabStop = false;
            // 
            // rbtnEmployee
            // 
            this.rbtnEmployee.AutoSize = true;
            this.rbtnEmployee.Checked = true;
            this.rbtnEmployee.Location = new System.Drawing.Point(9, 12);
            this.rbtnEmployee.Name = "rbtnEmployee";
            this.rbtnEmployee.Size = new System.Drawing.Size(80, 19);
            this.rbtnEmployee.TabIndex = 0;
            this.rbtnEmployee.TabStop = true;
            this.rbtnEmployee.Text = "Employee";
            this.rbtnEmployee.UseVisualStyleBackColor = true;
            this.rbtnEmployee.Click += new System.EventHandler(this.rbtnEmployee_Click);
            // 
            // rbtnGuest
            // 
            this.rbtnGuest.AutoSize = true;
            this.rbtnGuest.Location = new System.Drawing.Point(110, 12);
            this.rbtnGuest.Name = "rbtnGuest";
            this.rbtnGuest.Size = new System.Drawing.Size(58, 19);
            this.rbtnGuest.TabIndex = 1;
            this.rbtnGuest.Text = "Guest";
            this.rbtnGuest.UseVisualStyleBackColor = true;
            this.rbtnGuest.Click += new System.EventHandler(this.rbtnGuest_Click);
            // 
            // dgvPAccess
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Bisque;
            this.dgvPAccess.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPAccess.BackgroundColor = System.Drawing.Color.White;
            this.dgvPAccess.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPAccess.Location = new System.Drawing.Point(19, 370);
            this.dgvPAccess.Name = "dgvPAccess";
            this.dgvPAccess.ReadOnly = true;
            this.dgvPAccess.Size = new System.Drawing.Size(441, 110);
            this.dgvPAccess.TabIndex = 5;
            this.dgvPAccess.Visible = false;
            this.dgvPAccess.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvPAccess_CurrentCellDirtyStateChanged);
            // 
            // txtLoginID
            // 
            this.txtLoginID.Location = new System.Drawing.Point(129, 96);
            this.txtLoginID.Name = "txtLoginID";
            this.txtLoginID.ReadOnly = true;
            this.txtLoginID.Size = new System.Drawing.Size(220, 21);
            this.txtLoginID.TabIndex = 2;
            // 
            // grpAccess
            // 
            this.grpAccess.Controls.Add(this.rbtnDUser);
            this.grpAccess.Controls.Add(this.rbtnAdministrator);
            this.grpAccess.Location = new System.Drawing.Point(130, 113);
            this.grpAccess.Name = "grpAccess";
            this.grpAccess.Size = new System.Drawing.Size(219, 35);
            this.grpAccess.TabIndex = 3;
            this.grpAccess.TabStop = false;
            // 
            // rbtnDUser
            // 
            this.rbtnDUser.AutoSize = true;
            this.rbtnDUser.Location = new System.Drawing.Point(110, 11);
            this.rbtnDUser.Name = "rbtnDUser";
            this.rbtnDUser.Size = new System.Drawing.Size(99, 19);
            this.rbtnDUser.TabIndex = 2;
            this.rbtnDUser.TabStop = true;
            this.rbtnDUser.Text = "Domain User";
            this.rbtnDUser.UseVisualStyleBackColor = true;
            this.rbtnDUser.Click += new System.EventHandler(this.rbtnDUser_Click);
            // 
            // rbtnAdministrator
            // 
            this.rbtnAdministrator.AutoSize = true;
            this.rbtnAdministrator.Location = new System.Drawing.Point(6, 11);
            this.rbtnAdministrator.Name = "rbtnAdministrator";
            this.rbtnAdministrator.Size = new System.Drawing.Size(98, 19);
            this.rbtnAdministrator.TabIndex = 0;
            this.rbtnAdministrator.TabStop = true;
            this.rbtnAdministrator.Text = "Administrator";
            this.rbtnAdministrator.UseVisualStyleBackColor = true;
            this.rbtnAdministrator.Click += new System.EventHandler(this.rbtnAdministrator_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(314, 96);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(35, 21);
            this.txtUserID.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Firebrick;
            this.label7.Location = new System.Drawing.Point(16, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 15);
            this.label7.TabIndex = 10;
            this.label7.Text = "System Access";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Access Level";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Login ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "User Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "User Type";
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.CornflowerBlue;
            this.lblHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHeader.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblHeader.Location = new System.Drawing.Point(-1, -1);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(480, 22);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "USER PROFILE";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseDown);
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // txtAccessLevel
            // 
            this.txtAccessLevel.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccessLevel.Location = new System.Drawing.Point(397, 216);
            this.txtAccessLevel.Name = "txtAccessLevel";
            this.txtAccessLevel.Size = new System.Drawing.Size(33, 17);
            this.txtAccessLevel.TabIndex = 18;
            this.txtAccessLevel.TextChanged += new System.EventHandler(this.txtAccessLevel_TextChanged);
            // 
            // txtUserType
            // 
            this.txtUserType.Font = new System.Drawing.Font("Arial", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserType.Location = new System.Drawing.Point(410, 216);
            this.txtUserType.Name = "txtUserType";
            this.txtUserType.Size = new System.Drawing.Size(50, 17);
            this.txtUserType.TabIndex = 108;
            this.txtUserType.TextChanged += new System.EventHandler(this.txtUserType_TextChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            // 
            // UsersProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1206, 674);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "UsersProfile";
            this.Load += new System.EventHandler(this.UsersProfile_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UsersProfile_KeyDown);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSAccess)).EndInit();
            this.grpUtype.ResumeLayout(false);
            this.grpUtype.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPAccess)).EndInit();
            this.grpAccess.ResumeLayout(false);
            this.grpAccess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.GroupBox grpUtype;
        private System.Windows.Forms.RadioButton rbtnEmployee;
        private System.Windows.Forms.RadioButton rbtnGuest;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.DataGridView dgvPAccess;
        private System.Windows.Forms.DataGridView dgvSAccess;
        private System.Windows.Forms.TextBox txtLoginID;
        private System.Windows.Forms.GroupBox grpAccess;
        private System.Windows.Forms.RadioButton rbtnDUser;
        private System.Windows.Forms.RadioButton rbtnPUser;
        private System.Windows.Forms.RadioButton rbtnAdministrator;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TextBox txtAccessLevel;
        private System.Windows.Forms.TextBox txtUserType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.BindingSource bsUsers;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ComboBox cboUserGroups;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGroupID;
        private System.Windows.Forms.CheckBox chkInactive;
        private System.Windows.Forms.ComboBox cboEmployees;
    }
}
