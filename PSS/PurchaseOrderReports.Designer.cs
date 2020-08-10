namespace PSS
{
    partial class PurchaseOrderReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseOrderReports));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.rdoByVendor = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlReports = new System.Windows.Forms.Panel();
            this.rdoByDept = new System.Windows.Forms.RadioButton();
            this.btnProcess = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.picDeptNames = new System.Windows.Forms.PictureBox();
            this.txtDeptName = new GISControls.TextBoxChar();
            this.dgvDeptNames = new System.Windows.Forms.DataGridView();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape2 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.picVendorNames = new System.Windows.Forms.PictureBox();
            this.dgvVendorNames = new System.Windows.Forms.DataGridView();
            this.txtVendorName = new GISControls.TextBoxChar();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.lblVendor = new System.Windows.Forms.Label();
            this.txtVendorID = new GISControls.TextBoxChar();
            this.txtDeptID = new GISControls.TextBoxChar();
            this.chkClear = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlReports.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeptNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeptNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVendorNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorNames)).BeginInit();
            this.SuspendLayout();
            // 
            // rdoByVendor
            // 
            this.rdoByVendor.AutoSize = true;
            this.rdoByVendor.Location = new System.Drawing.Point(335, 36);
            this.rdoByVendor.Name = "rdoByVendor";
            this.rdoByVendor.Size = new System.Drawing.Size(107, 17);
            this.rdoByVendor.TabIndex = 415;
            this.rdoByVendor.Text = "Usage by Vendor";
            this.rdoByVendor.UseVisualStyleBackColor = true;
            this.rdoByVendor.Click += new System.EventHandler(this.rdoByVendor_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(513, 19);
            this.label1.TabIndex = 414;
            this.label1.Text = "Reports";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlReports
            // 
            this.pnlReports.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlReports.Controls.Add(this.rdoByVendor);
            this.pnlReports.Controls.Add(this.label1);
            this.pnlReports.Controls.Add(this.rdoByDept);
            this.pnlReports.Location = new System.Drawing.Point(37, 61);
            this.pnlReports.Name = "pnlReports";
            this.pnlReports.Size = new System.Drawing.Size(514, 67);
            this.pnlReports.TabIndex = 422;
            // 
            // rdoByDept
            // 
            this.rdoByDept.AutoSize = true;
            this.rdoByDept.Checked = true;
            this.rdoByDept.Location = new System.Drawing.Point(59, 36);
            this.rdoByDept.Name = "rdoByDept";
            this.rdoByDept.Size = new System.Drawing.Size(128, 17);
            this.rdoByDept.TabIndex = 25;
            this.rdoByDept.TabStop = true;
            this.rdoByDept.Text = "Usage by Department";
            this.rdoByDept.UseVisualStyleBackColor = true;
            this.rdoByDept.Click += new System.EventHandler(this.rdoByDept_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(492, 355);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(59, 24);
            this.btnProcess.TabIndex = 412;
            this.btnProcess.Text = "&Proceed";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProceed_Click);
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.SteelBlue;
            this.label20.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(36, 35);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(79, 19);
            this.label20.TabIndex = 416;
            this.label20.Text = "Date Range";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.ForeColor = System.Drawing.Color.Red;
            this.lblProgress.Location = new System.Drawing.Point(33, 361);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(203, 13);
            this.lblProgress.TabIndex = 421;
            this.lblProgress.Text = "Generating report...please standby";
            this.lblProgress.Visible = false;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label18.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(259, 35);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(26, 19);
            this.label18.TabIndex = 420;
            this.label18.Text = "To";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label19.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(116, 35);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(39, 19);
            this.label19.TabIndex = 419;
            this.label19.Text = "From";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "MM/dd/yyyy";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(285, 35);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(98, 20);
            this.dtpEnd.TabIndex = 418;
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "MM/dd/yyyy";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(155, 35);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(98, 20);
            this.dtpStart.TabIndex = 417;
            // 
            // picDeptNames
            // 
            this.picDeptNames.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picDeptNames.BackgroundImage")));
            this.picDeptNames.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picDeptNames.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picDeptNames.Location = new System.Drawing.Point(249, 168);
            this.picDeptNames.Name = "picDeptNames";
            this.picDeptNames.Size = new System.Drawing.Size(19, 21);
            this.picDeptNames.TabIndex = 418;
            this.picDeptNames.TabStop = false;
            this.picDeptNames.Click += new System.EventHandler(this.picDeptNames_Click);
            // 
            // txtDeptName
            // 
            this.txtDeptName.BackColor = System.Drawing.Color.White;
            this.txtDeptName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeptName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeptName.Location = new System.Drawing.Point(54, 168);
            this.txtDeptName.Name = "txtDeptName";
            this.txtDeptName.Size = new System.Drawing.Size(214, 21);
            this.txtDeptName.TabIndex = 417;
            this.txtDeptName.TextChanged += new System.EventHandler(this.txtDeptName_TextChanged);
            // 
            // dgvDeptNames
            // 
            this.dgvDeptNames.AllowUserToAddRows = false;
            this.dgvDeptNames.AllowUserToDeleteRows = false;
            this.dgvDeptNames.BackgroundColor = System.Drawing.Color.White;
            this.dgvDeptNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeptNames.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDeptNames.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDeptNames.Location = new System.Drawing.Point(54, 188);
            this.dgvDeptNames.Name = "dgvDeptNames";
            this.dgvDeptNames.ReadOnly = true;
            this.dgvDeptNames.RowHeadersVisible = false;
            this.dgvDeptNames.Size = new System.Drawing.Size(214, 125);
            this.dgvDeptNames.TabIndex = 423;
            this.dgvDeptNames.Visible = false;
            this.dgvDeptNames.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDeptNames_CellContentClick);
            this.dgvDeptNames.DoubleClick += new System.EventHandler(this.dgvDeptNames_DoubleClick);
            this.dgvDeptNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvDeptNames_KeyDown);
            this.dgvDeptNames.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvDeptNames_KeyPress);
            this.dgvDeptNames.Leave += new System.EventHandler(this.dgvDeptNames_Leave);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape2,
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(583, 391);
            this.shapeContainer1.TabIndex = 424;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape2
            // 
            this.rectangleShape2.Location = new System.Drawing.Point(299, 151);
            this.rectangleShape2.Name = "rectangleShape2";
            this.rectangleShape2.Size = new System.Drawing.Size(251, 186);
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.Location = new System.Drawing.Point(36, 150);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(251, 187);
            // 
            // picVendorNames
            // 
            this.picVendorNames.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picVendorNames.BackgroundImage")));
            this.picVendorNames.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picVendorNames.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picVendorNames.Location = new System.Drawing.Point(513, 168);
            this.picVendorNames.Name = "picVendorNames";
            this.picVendorNames.Size = new System.Drawing.Size(19, 21);
            this.picVendorNames.TabIndex = 426;
            this.picVendorNames.TabStop = false;
            this.picVendorNames.Click += new System.EventHandler(this.picVendorNames_Click);
            // 
            // dgvVendorNames
            // 
            this.dgvVendorNames.AllowUserToAddRows = false;
            this.dgvVendorNames.AllowUserToDeleteRows = false;
            this.dgvVendorNames.BackgroundColor = System.Drawing.Color.White;
            this.dgvVendorNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVendorNames.ColumnHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvVendorNames.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvVendorNames.Location = new System.Drawing.Point(318, 188);
            this.dgvVendorNames.Name = "dgvVendorNames";
            this.dgvVendorNames.ReadOnly = true;
            this.dgvVendorNames.RowHeadersVisible = false;
            this.dgvVendorNames.Size = new System.Drawing.Size(214, 125);
            this.dgvVendorNames.TabIndex = 427;
            this.dgvVendorNames.Visible = false;
            this.dgvVendorNames.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVendorNames_CellContentClick);
            this.dgvVendorNames.DoubleClick += new System.EventHandler(this.dgvVendorNames_DoubleClick);
            this.dgvVendorNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVendorNames_KeyDown);
            this.dgvVendorNames.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvVendorNames_KeyPress);
            this.dgvVendorNames.Leave += new System.EventHandler(this.dgvVendorNames_Leave);
            // 
            // txtVendorName
            // 
            this.txtVendorName.BackColor = System.Drawing.Color.White;
            this.txtVendorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVendorName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendorName.Location = new System.Drawing.Point(318, 168);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.Size = new System.Drawing.Size(214, 21);
            this.txtVendorName.TabIndex = 425;
            this.txtVendorName.TextChanged += new System.EventHandler(this.txtVendorName_TextChanged);
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepartment.ForeColor = System.Drawing.Color.Red;
            this.lblDepartment.Location = new System.Drawing.Point(58, 144);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(86, 13);
            this.lblDepartment.TabIndex = 428;
            this.lblDepartment.Text = "Departments: ";
            // 
            // lblVendor
            // 
            this.lblVendor.AutoSize = true;
            this.lblVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVendor.ForeColor = System.Drawing.Color.Red;
            this.lblVendor.Location = new System.Drawing.Point(315, 144);
            this.lblVendor.Name = "lblVendor";
            this.lblVendor.Size = new System.Drawing.Size(61, 13);
            this.lblVendor.TabIndex = 429;
            this.lblVendor.Text = "Vendors: ";
            // 
            // txtVendorID
            // 
            this.txtVendorID.BackColor = System.Drawing.Color.White;
            this.txtVendorID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtVendorID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendorID.Location = new System.Drawing.Point(315, 8);
            this.txtVendorID.MaxLength = 5;
            this.txtVendorID.Name = "txtVendorID";
            this.txtVendorID.Size = new System.Drawing.Size(68, 21);
            this.txtVendorID.TabIndex = 430;
            this.txtVendorID.Visible = false;
            // 
            // txtDeptID
            // 
            this.txtDeptID.BackColor = System.Drawing.Color.White;
            this.txtDeptID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDeptID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDeptID.Location = new System.Drawing.Point(241, 8);
            this.txtDeptID.MaxLength = 5;
            this.txtDeptID.Name = "txtDeptID";
            this.txtDeptID.Size = new System.Drawing.Size(68, 21);
            this.txtDeptID.TabIndex = 431;
            this.txtDeptID.Visible = false;
            // 
            // chkClear
            // 
            this.chkClear.AutoSize = true;
            this.chkClear.Location = new System.Drawing.Point(507, 40);
            this.chkClear.Name = "chkClear";
            this.chkClear.Size = new System.Drawing.Size(50, 17);
            this.chkClear.TabIndex = 432;
            this.chkClear.Text = "Clear";
            this.chkClear.UseVisualStyleBackColor = true;
            this.chkClear.CheckStateChanged += new System.EventHandler(this.chkClear_CheckStateChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(427, 355);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(59, 24);
            this.btnClose.TabIndex = 433;
            this.btnClose.Text = "Cl&ose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // PurchaseOrderReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 391);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chkClear);
            this.Controls.Add(this.txtDeptID);
            this.Controls.Add(this.txtVendorID);
            this.Controls.Add(this.lblVendor);
            this.Controls.Add(this.lblDepartment);
            this.Controls.Add(this.picVendorNames);
            this.Controls.Add(this.dgvVendorNames);
            this.Controls.Add(this.txtVendorName);
            this.Controls.Add(this.picDeptNames);
            this.Controls.Add(this.dgvDeptNames);
            this.Controls.Add(this.pnlReports);
            this.Controls.Add(this.txtDeptName);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.shapeContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PurchaseOrderReports";
            this.Text = "PurchaseOrderReports";
            this.Load += new System.EventHandler(this.PurchaseOrderReports_Load);
            this.pnlReports.ResumeLayout(false);
            this.pnlReports.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeptNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeptNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVendorNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendorNames)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoByVendor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel pnlReports;
        private System.Windows.Forms.RadioButton rdoByDept;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.PictureBox picDeptNames;
        private GISControls.TextBoxChar txtDeptName;
        private System.Windows.Forms.DataGridView dgvDeptNames;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape2;
        private System.Windows.Forms.PictureBox picVendorNames;
        private System.Windows.Forms.DataGridView dgvVendorNames;
        private GISControls.TextBoxChar txtVendorName;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.Label lblVendor;
        private GISControls.TextBoxChar txtVendorID;
        private GISControls.TextBoxChar txtDeptID;
        private System.Windows.Forms.CheckBox chkClear;
        private System.Windows.Forms.Button btnClose;
    }
}