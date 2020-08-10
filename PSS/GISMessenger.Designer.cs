namespace GIS
{
    partial class GISMessenger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GISMessenger));
            this.rtbMessages = new System.Windows.Forms.RichTextBox();
            this.rtbNewMessage = new System.Windows.Forms.RichTextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblProfile = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvSDNames = new System.Windows.Forms.DataGridView();
            this.bsSDNames = new System.Windows.Forms.BindingSource(this.components);
            this.picImage = new System.Windows.Forms.PictureBox();
            this.dtpExtDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoThisDate = new System.Windows.Forms.RadioButton();
            this.rdoExtendTo = new System.Windows.Forms.RadioButton();
            this.rdoShowAll = new System.Windows.Forms.RadioButton();
            this.bnEmployees = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSDNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSDNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bnEmployees)).BeginInit();
            this.bnEmployees.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbMessages
            // 
            this.rtbMessages.BackColor = System.Drawing.Color.PapayaWhip;
            this.rtbMessages.Location = new System.Drawing.Point(353, 98);
            this.rtbMessages.Name = "rtbMessages";
            this.rtbMessages.ReadOnly = true;
            this.rtbMessages.Size = new System.Drawing.Size(464, 251);
            this.rtbMessages.TabIndex = 5;
            this.rtbMessages.Text = "";
            // 
            // rtbNewMessage
            // 
            this.rtbNewMessage.Location = new System.Drawing.Point(353, 368);
            this.rtbNewMessage.MaxLength = 150;
            this.rtbNewMessage.Name = "rtbNewMessage";
            this.rtbNewMessage.Size = new System.Drawing.Size(464, 81);
            this.rtbNewMessage.TabIndex = 6;
            this.rtbNewMessage.Text = "";
            this.rtbNewMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtbNewMessage_KeyPress);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSend.Location = new System.Drawing.Point(738, 455);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(79, 27);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "Se&nd";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(351, 354);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "New Message";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "MM/dd/yyyy";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Location = new System.Drawing.Point(427, 75);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(81, 20);
            this.dtpDate.TabIndex = 1;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.BackColor = System.Drawing.Color.DarkRed;
            this.lblProfile.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.ForeColor = System.Drawing.Color.White;
            this.lblProfile.Location = new System.Drawing.Point(18, 455);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(400, 14);
            this.lblProfile.TabIndex = 13;
            this.lblProfile.Text = "Employee profile is not yet added. Please contact the IT Software Team.";
            this.lblProfile.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(738, 22);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(79, 27);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Cl&ose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            this.btnClose.Enter += new System.EventHandler(this.btnClose_Enter);
            // 
            // dgvSDNames
            // 
            this.dgvSDNames.AllowUserToAddRows = false;
            this.dgvSDNames.AllowUserToDeleteRows = false;
            this.dgvSDNames.BackgroundColor = System.Drawing.Color.White;
            this.dgvSDNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSDNames.ColumnHeadersVisible = false;
            this.dgvSDNames.Location = new System.Drawing.Point(21, 98);
            this.dgvSDNames.Name = "dgvSDNames";
            this.dgvSDNames.ReadOnly = true;
            this.dgvSDNames.RowHeadersVisible = false;
            this.dgvSDNames.RowTemplate.Height = 50;
            this.dgvSDNames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSDNames.Size = new System.Drawing.Size(327, 351);
            this.dgvSDNames.StandardTab = true;
            this.dgvSDNames.TabIndex = 8;
            this.dgvSDNames.CurrentCellChanged += new System.EventHandler(this.dgvSDNames_CurrentCellChanged);
            // 
            // picImage
            // 
            this.picImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picImage.Location = new System.Drawing.Point(288, 35);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(60, 60);
            this.picImage.TabIndex = 16;
            this.picImage.TabStop = false;
            // 
            // dtpExtDate
            // 
            this.dtpExtDate.CustomFormat = "MM/dd/yyyy";
            this.dtpExtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExtDate.Location = new System.Drawing.Point(608, 75);
            this.dtpExtDate.Name = "dtpExtDate";
            this.dtpExtDate.Size = new System.Drawing.Size(81, 20);
            this.dtpExtDate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(350, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Select Messages:";
            // 
            // rdoThisDate
            // 
            this.rdoThisDate.AutoSize = true;
            this.rdoThisDate.ForeColor = System.Drawing.Color.White;
            this.rdoThisDate.Location = new System.Drawing.Point(354, 77);
            this.rdoThisDate.Name = "rdoThisDate";
            this.rdoThisDate.Size = new System.Drawing.Size(71, 17);
            this.rdoThisDate.TabIndex = 0;
            this.rdoThisDate.TabStop = true;
            this.rdoThisDate.Text = "This Date";
            this.rdoThisDate.UseVisualStyleBackColor = true;
            this.rdoThisDate.CheckedChanged += new System.EventHandler(this.rdoThisDate_CheckedChanged);
            // 
            // rdoExtendTo
            // 
            this.rdoExtendTo.AutoSize = true;
            this.rdoExtendTo.ForeColor = System.Drawing.Color.White;
            this.rdoExtendTo.Location = new System.Drawing.Point(524, 77);
            this.rdoExtendTo.Name = "rdoExtendTo";
            this.rdoExtendTo.Size = new System.Drawing.Size(87, 17);
            this.rdoExtendTo.TabIndex = 2;
            this.rdoExtendTo.TabStop = true;
            this.rdoExtendTo.Text = "To This Date";
            this.rdoExtendTo.UseVisualStyleBackColor = true;
            this.rdoExtendTo.CheckedChanged += new System.EventHandler(this.rdoExtendTo_CheckedChanged);
            // 
            // rdoShowAll
            // 
            this.rdoShowAll.AutoSize = true;
            this.rdoShowAll.ForeColor = System.Drawing.Color.White;
            this.rdoShowAll.Location = new System.Drawing.Point(705, 77);
            this.rdoShowAll.Name = "rdoShowAll";
            this.rdoShowAll.Size = new System.Drawing.Size(117, 17);
            this.rdoShowAll.TabIndex = 4;
            this.rdoShowAll.TabStop = true;
            this.rdoShowAll.Text = "Show All Messages";
            this.rdoShowAll.UseVisualStyleBackColor = true;
            this.rdoShowAll.CheckedChanged += new System.EventHandler(this.rdoShowAll_CheckedChanged);
            // 
            // bnEmployees
            // 
            this.bnEmployees.AddNewItem = null;
            this.bnEmployees.CountItem = this.bindingNavigatorCountItem;
            this.bnEmployees.DeleteItem = null;
            this.bnEmployees.Dock = System.Windows.Forms.DockStyle.None;
            this.bnEmployees.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.bnEmployees.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem});
            this.bnEmployees.Location = new System.Drawing.Point(21, 69);
            this.bnEmployees.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bnEmployees.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bnEmployees.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bnEmployees.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bnEmployees.Name = "bnEmployees";
            this.bnEmployees.PositionItem = this.bindingNavigatorPositionItem;
            this.bnEmployees.Size = new System.Drawing.Size(194, 25);
            this.bnEmployees.TabIndex = 21;
            this.bnEmployees.Text = "bindingNavigator1";
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
            // GISMessenger
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.CadetBlue;
            this.ClientSize = new System.Drawing.Size(840, 504);
            this.ControlBox = false;
            this.Controls.Add(this.dtpExtDate);
            this.Controls.Add(this.bnEmployees);
            this.Controls.Add(this.rdoShowAll);
            this.Controls.Add(this.rdoExtendTo);
            this.Controls.Add(this.rdoThisDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.dgvSDNames);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblProfile);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.rtbNewMessage);
            this.Controls.Add(this.rtbMessages);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GISMessenger";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "GIS Messenger";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.GISMessenger_Activated);
            this.Load += new System.EventHandler(this.GISMessenger_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSDNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSDNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bnEmployees)).EndInit();
            this.bnEmployees.ResumeLayout(false);
            this.bnEmployees.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbMessages;
        private System.Windows.Forms.RichTextBox rtbNewMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgvSDNames;
        private System.Windows.Forms.BindingSource bsSDNames;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.DateTimePicker dtpExtDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoThisDate;
        private System.Windows.Forms.RadioButton rdoExtendTo;
        private System.Windows.Forms.RadioButton rdoShowAll;
        private System.Windows.Forms.BindingNavigator bnEmployees;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
    }
}