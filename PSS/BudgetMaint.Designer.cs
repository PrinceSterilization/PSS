namespace GIS
{
    partial class BudgetMaint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BudgetMaint));
            this.dgvFile = new System.Windows.Forms.DataGridView();
            this.bsFile = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cboFY = new System.Windows.Forms.ComboBox();
            this.bnFile = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.lblUpdate = new System.Windows.Forms.Label();
            this.btnGenBudget = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPercent = new System.Windows.Forms.TextBox();
            this.tmrCalculate = new System.Windows.Forms.Timer(this.components);
            this.btnPreviewVar = new System.Windows.Forms.Button();
            this.pnlReports = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.cboQtrly = new System.Windows.Forms.ComboBox();
            this.cboMonthly = new System.Windows.Forms.ComboBox();
            this.rdoQtrly = new System.Windows.Forms.RadioButton();
            this.rdoMonthly = new System.Windows.Forms.RadioButton();
            this.rdoYTD = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bnFile)).BeginInit();
            this.bnFile.SuspendLayout();
            this.pnlReports.SuspendLayout();
            this.SuspendLayout();
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
            this.dgvFile.Location = new System.Drawing.Point(21, 50);
            this.dgvFile.Name = "dgvFile";
            this.dgvFile.Size = new System.Drawing.Size(702, 390);
            this.dgvFile.TabIndex = 0;
            this.dgvFile.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvFile_CellBeginEdit);
            this.dgvFile.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFile_CellEndEdit);
            this.dgvFile.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvFile_EditingControlShowing);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Year";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboFY
            // 
            this.cboFY.FormattingEnabled = true;
            this.cboFY.Items.AddRange(new object[] {
            "2016"});
            this.cboFY.Location = new System.Drawing.Point(56, 23);
            this.cboFY.Name = "cboFY";
            this.cboFY.Size = new System.Drawing.Size(64, 21);
            this.cboFY.TabIndex = 2;
            this.cboFY.SelectedIndexChanged += new System.EventHandler(this.cboFY_SelectedIndexChanged);
            // 
            // bnFile
            // 
            this.bnFile.AddNewItem = null;
            this.bnFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bnFile.CountItem = this.bindingNavigatorCountItem;
            this.bnFile.DeleteItem = null;
            this.bnFile.Dock = System.Windows.Forms.DockStyle.None;
            this.bnFile.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.bnFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem});
            this.bnFile.Location = new System.Drawing.Point(21, 441);
            this.bnFile.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bnFile.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bnFile.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bnFile.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bnFile.Name = "bnFile";
            this.bnFile.PositionItem = this.bindingNavigatorPositionItem;
            this.bnFile.Size = new System.Drawing.Size(194, 25);
            this.bnFile.TabIndex = 3;
            this.bnFile.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(590, 447);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(133, 23);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "&Update Budget";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // lblUpdate
            // 
            this.lblUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUpdate.AutoSize = true;
            this.lblUpdate.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdate.ForeColor = System.Drawing.Color.DarkRed;
            this.lblUpdate.Location = new System.Drawing.Point(297, 452);
            this.lblUpdate.Name = "lblUpdate";
            this.lblUpdate.Size = new System.Drawing.Size(158, 14);
            this.lblUpdate.TabIndex = 6;
            this.lblUpdate.Text = "Updating budget amounts...";
            this.lblUpdate.Visible = false;
            // 
            // btnGenBudget
            // 
            this.btnGenBudget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenBudget.Location = new System.Drawing.Point(590, 23);
            this.btnGenBudget.Name = "btnGenBudget";
            this.btnGenBudget.Size = new System.Drawing.Size(133, 23);
            this.btnGenBudget.TabIndex = 7;
            this.btnGenBudget.Text = "&Generate Annual Budget";
            this.btnGenBudget.UseVisualStyleBackColor = true;
            this.btnGenBudget.Click += new System.EventHandler(this.btnGenBudget_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(399, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Revenue Mark-up (%)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPercent
            // 
            this.txtPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPercent.Location = new System.Drawing.Point(531, 25);
            this.txtPercent.Name = "txtPercent";
            this.txtPercent.Size = new System.Drawing.Size(53, 20);
            this.txtPercent.TabIndex = 9;
            this.txtPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPercent_KeyPress);
            // 
            // tmrCalculate
            // 
            this.tmrCalculate.Interval = 1000;
            this.tmrCalculate.Tick += new System.EventHandler(this.tmrCalculate_Tick);
            // 
            // btnPreviewVar
            // 
            this.btnPreviewVar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviewVar.Location = new System.Drawing.Point(451, 447);
            this.btnPreviewVar.Name = "btnPreviewVar";
            this.btnPreviewVar.Size = new System.Drawing.Size(133, 23);
            this.btnPreviewVar.TabIndex = 10;
            this.btnPreviewVar.Text = "&Preview Variance Report";
            this.btnPreviewVar.UseVisualStyleBackColor = true;
            this.btnPreviewVar.Click += new System.EventHandler(this.btnPreviewVar_Click);
            // 
            // pnlReports
            // 
            this.pnlReports.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlReports.Controls.Add(this.btnClose);
            this.pnlReports.Controls.Add(this.btnPreview);
            this.pnlReports.Controls.Add(this.cboQtrly);
            this.pnlReports.Controls.Add(this.cboMonthly);
            this.pnlReports.Controls.Add(this.rdoQtrly);
            this.pnlReports.Controls.Add(this.rdoMonthly);
            this.pnlReports.Controls.Add(this.rdoYTD);
            this.pnlReports.Controls.Add(this.label3);
            this.pnlReports.Location = new System.Drawing.Point(224, 132);
            this.pnlReports.Name = "pnlReports";
            this.pnlReports.Size = new System.Drawing.Size(323, 223);
            this.pnlReports.TabIndex = 11;
            this.pnlReports.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(193, 161);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(88, 23);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Cl&ose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(99, 161);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(88, 23);
            this.btnPreview.TabIndex = 13;
            this.btnPreview.Text = "Print &Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // cboQtrly
            // 
            this.cboQtrly.FormattingEnabled = true;
            this.cboQtrly.Items.AddRange(new object[] {
            "1st Quarter",
            "2nd Quarter",
            "3rd Quarter",
            "4th Quarter"});
            this.cboQtrly.Location = new System.Drawing.Point(142, 76);
            this.cboQtrly.Name = "cboQtrly";
            this.cboQtrly.Size = new System.Drawing.Size(139, 21);
            this.cboQtrly.TabIndex = 12;
            // 
            // cboMonthly
            // 
            this.cboMonthly.FormattingEnabled = true;
            this.cboMonthly.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.cboMonthly.Location = new System.Drawing.Point(142, 102);
            this.cboMonthly.Name = "cboMonthly";
            this.cboMonthly.Size = new System.Drawing.Size(139, 21);
            this.cboMonthly.TabIndex = 11;
            // 
            // rdoQtrly
            // 
            this.rdoQtrly.AutoSize = true;
            this.rdoQtrly.Location = new System.Drawing.Point(54, 80);
            this.rdoQtrly.Name = "rdoQtrly";
            this.rdoQtrly.Size = new System.Drawing.Size(67, 17);
            this.rdoQtrly.TabIndex = 10;
            this.rdoQtrly.TabStop = true;
            this.rdoQtrly.Text = "Quarterly";
            this.rdoQtrly.UseVisualStyleBackColor = true;
            this.rdoQtrly.CheckedChanged += new System.EventHandler(this.rdoQtrly_CheckedChanged);
            // 
            // rdoMonthly
            // 
            this.rdoMonthly.AutoSize = true;
            this.rdoMonthly.Location = new System.Drawing.Point(54, 103);
            this.rdoMonthly.Name = "rdoMonthly";
            this.rdoMonthly.Size = new System.Drawing.Size(62, 17);
            this.rdoMonthly.TabIndex = 9;
            this.rdoMonthly.TabStop = true;
            this.rdoMonthly.Text = "Monthly";
            this.rdoMonthly.UseVisualStyleBackColor = true;
            this.rdoMonthly.CheckedChanged += new System.EventHandler(this.rdoMonthly_CheckedChanged);
            // 
            // rdoYTD
            // 
            this.rdoYTD.AutoSize = true;
            this.rdoYTD.Location = new System.Drawing.Point(54, 57);
            this.rdoYTD.Name = "rdoYTD";
            this.rdoYTD.Size = new System.Drawing.Size(89, 17);
            this.rdoYTD.TabIndex = 8;
            this.rdoYTD.TabStop = true;
            this.rdoYTD.Text = "Year-To-Date";
            this.rdoYTD.UseVisualStyleBackColor = true;
            this.rdoYTD.CheckedChanged += new System.EventHandler(this.rdoYTD_CheckedChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.SteelBlue;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(321, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "VARIANCE REPORTS";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BudgetMaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 491);
            this.Controls.Add(this.pnlReports);
            this.Controls.Add(this.btnPreviewVar);
            this.Controls.Add(this.txtPercent);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGenBudget);
            this.Controls.Add(this.lblUpdate);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.bnFile);
            this.Controls.Add(this.cboFY);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvFile);
            this.MinimizeBox = false;
            this.Name = "BudgetMaint";
            this.Text = "BudgetMaint";
            this.Activated += new System.EventHandler(this.BudgetMaint_Activated);
            this.Deactivate += new System.EventHandler(this.BudgetMaint_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BudgetMaint_FormClosing);
            this.Load += new System.EventHandler(this.BudgetMaint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bnFile)).EndInit();
            this.bnFile.ResumeLayout(false);
            this.bnFile.PerformLayout();
            this.pnlReports.ResumeLayout(false);
            this.pnlReports.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFile;
        private System.Windows.Forms.BindingSource bsFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboFY;
        private System.Windows.Forms.BindingNavigator bnFile;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label lblUpdate;
        private System.Windows.Forms.Button btnGenBudget;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPercent;
        private System.Windows.Forms.Timer tmrCalculate;
        private System.Windows.Forms.Button btnPreviewVar;
        private System.Windows.Forms.Panel pnlReports;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.ComboBox cboQtrly;
        private System.Windows.Forms.ComboBox cboMonthly;
        private System.Windows.Forms.RadioButton rdoQtrly;
        private System.Windows.Forms.RadioButton rdoMonthly;
        private System.Windows.Forms.RadioButton rdoYTD;
        private System.Windows.Forms.Label label3;
    }
}