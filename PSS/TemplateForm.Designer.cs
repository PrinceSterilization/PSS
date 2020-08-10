namespace PSS
{
    partial class TemplateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlsFile = new System.Windows.Forms.ToolStrip();
            this.tsbAdd = new System.Windows.Forms.ToolStripButton();
            this.tsbEdit = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbPrint = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbSearch = new System.Windows.Forms.ToolStripDropDownButton();
            this.tslSearch = new System.Windows.Forms.ToolStripLabel();
            this.tstbSearch = new System.Windows.Forms.ToolStripTextBox();
            this.tsbSearch = new System.Windows.Forms.ToolStripButton();
            this.tsbFilter = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExit = new System.Windows.Forms.ToolStripButton();
            this.tstbSearchField = new System.Windows.Forms.ToolStripTextBox();
            this.bnFile = new System.Windows.Forms.BindingNavigator(this.components);
            this.bnCount = new System.Windows.Forms.ToolStripLabel();
            this.bnMoveFirst = new System.Windows.Forms.ToolStripButton();
            this.bnMovePrevious = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bnPosition = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bnMoveNext = new System.Windows.Forms.ToolStripButton();
            this.bnMoveLast = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tslSearchData = new System.Windows.Forms.ToolStripLabel();
            this.dgvFile = new System.Windows.Forms.DataGridView();
            this.chkFullText = new System.Windows.Forms.CheckBox();
            this.chkShowInactive = new System.Windows.Forms.CheckBox();
            this.cklColumns = new System.Windows.Forms.CheckedListBox();
            this.bsFile = new System.Windows.Forms.BindingSource(this.components);
            this.lblLoadStatus = new System.Windows.Forms.Label();
            this.tlsFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bnFile)).BeginInit();
            this.bnFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.SuspendLayout();
            // 
            // tlsFile
            // 
            this.tlsFile.AllowMerge = false;
            this.tlsFile.CanOverflow = false;
            this.tlsFile.GripMargin = new System.Windows.Forms.Padding(0);
            this.tlsFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAdd,
            this.tsbEdit,
            this.tsbDelete,
            this.toolStripSeparator1,
            this.tsbSave,
            this.tsbCancel,
            this.toolStripSeparator2,
            this.tsddbPrint,
            this.toolStripSeparator3,
            this.tsddbSearch,
            this.tslSearch,
            this.tstbSearch,
            this.tsbSearch,
            this.tsbFilter,
            this.tsbRefresh,
            this.toolStripSeparator4,
            this.tsbExit,
            this.tstbSearchField});
            this.tlsFile.Location = new System.Drawing.Point(0, 0);
            this.tlsFile.Name = "tlsFile";
            this.tlsFile.Padding = new System.Windows.Forms.Padding(0);
            this.tlsFile.Size = new System.Drawing.Size(1916, 50);
            this.tlsFile.TabIndex = 1;
            this.tlsFile.TabStop = true;
            this.tlsFile.Text = "File Maintenance";
            // 
            // tsbAdd
            // 
            this.tsbAdd.BackColor = System.Drawing.SystemColors.Control;
            this.tsbAdd.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbAdd.ForeColor = System.Drawing.Color.White;
            this.tsbAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsbAdd.Image")));
            this.tsbAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAdd.Name = "tsbAdd";
            this.tsbAdd.Size = new System.Drawing.Size(46, 47);
            this.tsbAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbAdd.ToolTipText = "Add a new record (or press F2)";
            this.tsbAdd.Click += new System.EventHandler(this.tsbAdd_Click);
            // 
            // tsbEdit
            // 
            this.tsbEdit.BackColor = System.Drawing.SystemColors.Control;
            this.tsbEdit.Image = ((System.Drawing.Image)(resources.GetObject("tsbEdit.Image")));
            this.tsbEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEdit.Name = "tsbEdit";
            this.tsbEdit.Size = new System.Drawing.Size(47, 47);
            this.tsbEdit.ToolTipText = "Edit a record  (or press F3)";
            this.tsbEdit.Click += new System.EventHandler(this.tsbEdit_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.BackColor = System.Drawing.SystemColors.Control;
            this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDelete.Enabled = false;
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(46, 47);
            this.tsbDelete.Text = "Delete";
            this.tsbDelete.ToolTipText = "Delete a record (or press F4)";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 50);
            // 
            // tsbSave
            // 
            this.tsbSave.BackColor = System.Drawing.SystemColors.Control;
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Enabled = false;
            this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(46, 47);
            this.tsbSave.Text = "Save";
            this.tsbSave.ToolTipText = "Save new record or changes made (or press F5)";
            // 
            // tsbCancel
            // 
            this.tsbCancel.BackColor = System.Drawing.SystemColors.Control;
            this.tsbCancel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCancel.Enabled = false;
            this.tsbCancel.Image = ((System.Drawing.Image)(resources.GetObject("tsbCancel.Image")));
            this.tsbCancel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCancel.Name = "tsbCancel";
            this.tsbCancel.Size = new System.Drawing.Size(46, 47);
            this.tsbCancel.Text = "Cancel";
            this.tsbCancel.ToolTipText = "Do not save data or changes made (or press F6)";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 50);
            // 
            // tsddbPrint
            // 
            this.tsddbPrint.BackColor = System.Drawing.SystemColors.Control;
            this.tsddbPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsddbPrint.Image")));
            this.tsddbPrint.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsddbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbPrint.Name = "tsddbPrint";
            this.tsddbPrint.Size = new System.Drawing.Size(55, 47);
            this.tsddbPrint.ToolTipText = "Print a report (or press F7)";
            this.tsddbPrint.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsddbPrint_DropDownItemClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 50);
            // 
            // tsddbSearch
            // 
            this.tsddbSearch.BackColor = System.Drawing.SystemColors.Control;
            this.tsddbSearch.ForeColor = System.Drawing.Color.DarkRed;
            this.tsddbSearch.Image = ((System.Drawing.Image)(resources.GetObject("tsddbSearch.Image")));
            this.tsddbSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsddbSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbSearch.Name = "tsddbSearch";
            this.tsddbSearch.Size = new System.Drawing.Size(55, 47);
            this.tsddbSearch.ToolTipText = "Search or filter records (or press F8)";
            this.tsddbSearch.Click += new System.EventHandler(this.tsbSearch_Click);
            // 
            // tslSearch
            // 
            this.tslSearch.Name = "tslSearch";
            this.tslSearch.Size = new System.Drawing.Size(0, 47);
            // 
            // tstbSearch
            // 
            this.tstbSearch.AutoSize = false;
            this.tstbSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tstbSearch.Name = "tstbSearch";
            this.tstbSearch.Size = new System.Drawing.Size(200, 23);
            this.tstbSearch.Enter += new System.EventHandler(this.tstbSearch_Enter);
            // 
            // tsbSearch
            // 
            this.tsbSearch.Image = ((System.Drawing.Image)(resources.GetObject("tsbSearch.Image")));
            this.tsbSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSearch.Name = "tsbSearch";
            this.tsbSearch.Size = new System.Drawing.Size(62, 47);
            this.tsbSearch.Text = "&Search";
            this.tsbSearch.ToolTipText = "Execute full text search";
            this.tsbSearch.Click += new System.EventHandler(this.tsbSearch_Click);
            // 
            // tsbFilter
            // 
            this.tsbFilter.Image = ((System.Drawing.Image)(resources.GetObject("tsbFilter.Image")));
            this.tsbFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFilter.Name = "tsbFilter";
            this.tsbFilter.Size = new System.Drawing.Size(53, 47);
            this.tsbFilter.Text = "&Filter";
            this.tsbFilter.ToolTipText = "Execute filter records";
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tsbRefresh.Image")));
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(66, 47);
            this.tsbRefresh.Text = "&Refresh";
            this.tsbRefresh.ToolTipText = "Refresh the list";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 50);
            // 
            // tsbExit
            // 
            this.tsbExit.BackColor = System.Drawing.SystemColors.Control;
            this.tsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbExit.Image")));
            this.tsbExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Size = new System.Drawing.Size(47, 47);
            this.tsbExit.ToolTipText = "Close and exit from this file";
            // 
            // tstbSearchField
            // 
            this.tstbSearchField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tstbSearchField.Name = "tstbSearchField";
            this.tstbSearchField.Size = new System.Drawing.Size(100, 50);
            this.tstbSearchField.Visible = false;
            // 
            // bnFile
            // 
            this.bnFile.AddNewItem = null;
            this.bnFile.CountItem = this.bnCount;
            this.bnFile.DeleteItem = null;
            this.bnFile.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bnMoveFirst,
            this.bnMovePrevious,
            this.bindingNavigatorSeparator,
            this.bnPosition,
            this.bnCount,
            this.bindingNavigatorSeparator1,
            this.bnMoveNext,
            this.bnMoveLast,
            this.bindingNavigatorSeparator2,
            this.toolStripLabel1,
            this.tslSearchData});
            this.bnFile.Location = new System.Drawing.Point(0, 50);
            this.bnFile.MoveFirstItem = this.bnMoveFirst;
            this.bnFile.MoveLastItem = this.bnMoveLast;
            this.bnFile.MoveNextItem = this.bnMoveNext;
            this.bnFile.MovePreviousItem = this.bnMovePrevious;
            this.bnFile.Name = "bnFile";
            this.bnFile.PositionItem = this.bnPosition;
            this.bnFile.Size = new System.Drawing.Size(1916, 25);
            this.bnFile.TabIndex = 1;
            this.bnFile.TabStop = true;
            this.bnFile.Text = "bindingNavigator1";
            // 
            // bnCount
            // 
            this.bnCount.Name = "bnCount";
            this.bnCount.Size = new System.Drawing.Size(35, 22);
            this.bnCount.Text = "of {0}";
            this.bnCount.ToolTipText = "Total number of items";
            // 
            // bnMoveFirst
            // 
            this.bnMoveFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bnMoveFirst.Image = ((System.Drawing.Image)(resources.GetObject("bnMoveFirst.Image")));
            this.bnMoveFirst.Name = "bnMoveFirst";
            this.bnMoveFirst.RightToLeftAutoMirrorImage = true;
            this.bnMoveFirst.Size = new System.Drawing.Size(23, 22);
            this.bnMoveFirst.Text = "Move first";
            // 
            // bnMovePrevious
            // 
            this.bnMovePrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bnMovePrevious.Image = ((System.Drawing.Image)(resources.GetObject("bnMovePrevious.Image")));
            this.bnMovePrevious.Name = "bnMovePrevious";
            this.bnMovePrevious.RightToLeftAutoMirrorImage = true;
            this.bnMovePrevious.Size = new System.Drawing.Size(23, 22);
            this.bnMovePrevious.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bnPosition
            // 
            this.bnPosition.AccessibleName = "Position";
            this.bnPosition.AutoSize = false;
            this.bnPosition.Name = "bnPosition";
            this.bnPosition.Size = new System.Drawing.Size(50, 23);
            this.bnPosition.Text = "0";
            this.bnPosition.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bnMoveNext
            // 
            this.bnMoveNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bnMoveNext.Image = ((System.Drawing.Image)(resources.GetObject("bnMoveNext.Image")));
            this.bnMoveNext.Name = "bnMoveNext";
            this.bnMoveNext.RightToLeftAutoMirrorImage = true;
            this.bnMoveNext.Size = new System.Drawing.Size(23, 22);
            this.bnMoveNext.Text = "Move next";
            // 
            // bnMoveLast
            // 
            this.bnMoveLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bnMoveLast.Image = ((System.Drawing.Image)(resources.GetObject("bnMoveLast.Image")));
            this.bnMoveLast.Name = "bnMoveLast";
            this.bnMoveLast.RightToLeftAutoMirrorImage = true;
            this.bnMoveLast.Size = new System.Drawing.Size(23, 22);
            this.bnMoveLast.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.AutoSize = false;
            this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(156, 22);
            this.toolStripLabel1.Text = "Search/Filter Data Name :";
            this.toolStripLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tslSearchData
            // 
            this.tslSearchData.AutoSize = false;
            this.tslSearchData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tslSearchData.ForeColor = System.Drawing.Color.Maroon;
            this.tslSearchData.Name = "tslSearchData";
            this.tslSearchData.Size = new System.Drawing.Size(202, 22);
            this.tslSearchData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvFile
            // 
            this.dgvFile.AllowUserToAddRows = false;
            this.dgvFile.AllowUserToDeleteRows = false;
            this.dgvFile.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Bisque;
            this.dgvFile.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFile.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.dgvFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFile.Location = new System.Drawing.Point(0, 75);
            this.dgvFile.Name = "dgvFile";
            this.dgvFile.ReadOnly = true;
            this.dgvFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvFile.Size = new System.Drawing.Size(1916, 629);
            this.dgvFile.StandardTab = true;
            this.dgvFile.TabIndex = 2;
            this.dgvFile.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFile_CellClick);
            this.dgvFile.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFile_ColumnHeaderMouseClick);
            this.dgvFile.Click += new System.EventHandler(this.dgvFile_Click);
            // 
            // chkFullText
            // 
            this.chkFullText.AutoSize = true;
            this.chkFullText.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.chkFullText.Location = new System.Drawing.Point(638, 53);
            this.chkFullText.Name = "chkFullText";
            this.chkFullText.Size = new System.Drawing.Size(100, 19);
            this.chkFullText.TabIndex = 100;
            this.chkFullText.TabStop = false;
            this.chkFullText.Text = "F&ull Text Filter";
            this.chkFullText.UseVisualStyleBackColor = false;
            // 
            // chkShowInactive
            // 
            this.chkShowInactive.AutoSize = true;
            this.chkShowInactive.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.chkShowInactive.Location = new System.Drawing.Point(899, 53);
            this.chkShowInactive.Name = "chkShowInactive";
            this.chkShowInactive.Size = new System.Drawing.Size(151, 19);
            this.chkShowInactive.TabIndex = 101;
            this.chkShowInactive.TabStop = false;
            this.chkShowInactive.Text = "Show Inactive Records";
            this.chkShowInactive.UseVisualStyleBackColor = false;
            this.chkShowInactive.Visible = false;
            // 
            // cklColumns
            // 
            this.cklColumns.CheckOnClick = true;
            this.cklColumns.FormattingEnabled = true;
            this.cklColumns.Location = new System.Drawing.Point(0, 75);
            this.cklColumns.Name = "cklColumns";
            this.cklColumns.Size = new System.Drawing.Size(139, 212);
            this.cklColumns.TabIndex = 104;
            this.cklColumns.Visible = false;
            // 
            // lblLoadStatus
            // 
            this.lblLoadStatus.AutoSize = true;
            this.lblLoadStatus.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblLoadStatus.ForeColor = System.Drawing.Color.DarkRed;
            this.lblLoadStatus.Location = new System.Drawing.Point(1072, 54);
            this.lblLoadStatus.Name = "lblLoadStatus";
            this.lblLoadStatus.Size = new System.Drawing.Size(256, 15);
            this.lblLoadStatus.TabIndex = 106;
            this.lblLoadStatus.Text = "Connecting to database...0 seconds elapsed.";
            this.lblLoadStatus.Visible = false;
            // 
            // TemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1916, 704);
            this.Controls.Add(this.lblLoadStatus);
            this.Controls.Add(this.cklColumns);
            this.Controls.Add(this.chkShowInactive);
            this.Controls.Add(this.chkFullText);
            this.Controls.Add(this.dgvFile);
            this.Controls.Add(this.bnFile);
            this.Controls.Add(this.tlsFile);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "TemplateForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TemplateForm_FormClosing);
            this.Load += new System.EventHandler(this.TemplateForm_Load);
            this.tlsFile.ResumeLayout(false);
            this.tlsFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bnFile)).EndInit();
            this.bnFile.ResumeLayout(false);
            this.bnFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        public System.Windows.Forms.ToolStrip tlsFile;
        public System.Windows.Forms.ToolStripDropDownButton tsddbPrint;
        public System.Windows.Forms.ToolStripDropDownButton tsddbSearch;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        public System.Windows.Forms.BindingNavigator bnFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        public System.Windows.Forms.ToolStripLabel tslSearch;
        public System.Windows.Forms.ToolStripButton tsbFilter;
        public System.Windows.Forms.ToolStripTextBox tstbSearch;
        public System.Windows.Forms.ToolStripButton tsbSearch;
        public System.Windows.Forms.ToolStripButton tsbAdd;
        public System.Windows.Forms.ToolStripButton tsbEdit;
        public System.Windows.Forms.ToolStripButton tsbDelete;
        public System.Windows.Forms.ToolStripButton tsbSave;
        public System.Windows.Forms.ToolStripButton tsbCancel;
        public System.Windows.Forms.ToolStripButton tsbExit;
        public System.Windows.Forms.ToolStripTextBox tstbSearchField;
        public System.Windows.Forms.ToolStripButton tsbRefresh;
        public System.Windows.Forms.ToolStripLabel bnCount;
        public System.Windows.Forms.ToolStripButton bnMoveFirst;
        public System.Windows.Forms.ToolStripButton bnMovePrevious;
        public System.Windows.Forms.ToolStripTextBox bnPosition;
        public System.Windows.Forms.ToolStripButton bnMoveNext;
        public System.Windows.Forms.ToolStripButton bnMoveLast;
        public System.Windows.Forms.DataGridView dgvFile;
        public System.Windows.Forms.BindingSource bsFile;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        public System.Windows.Forms.ToolStripLabel tslSearchData;
        public System.Windows.Forms.CheckBox chkFullText;
        public System.Windows.Forms.CheckBox chkShowInactive;
        public System.Windows.Forms.CheckedListBox cklColumns;
        public System.Windows.Forms.Label lblLoadStatus;
    }
}