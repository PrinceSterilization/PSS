namespace PSS
{
    partial class CatalogNames
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CatalogNames));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.txtCatOrderTypeID = new GISControls.TextBoxChar();
            this.picCatOrderTypes = new System.Windows.Forms.PictureBox();
            this.txtCatOrderTypeName = new GISControls.TextBoxChar();
            this.dgvCatOrderTypes = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtCatName = new GISControls.TextBoxChar();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCatNameID = new GISControls.TextBoxChar();
            this.lblCatNameID = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCatOrderTypes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatOrderTypes)).BeginInit();
            this.SuspendLayout();
            // 
            // cklColumns
            // 
            this.cklColumns.Size = new System.Drawing.Size(122, 196);
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.txtCatOrderTypeID);
            this.pnlRecord.Controls.Add(this.picCatOrderTypes);
            this.pnlRecord.Controls.Add(this.txtCatOrderTypeName);
            this.pnlRecord.Controls.Add(this.dgvCatOrderTypes);
            this.pnlRecord.Controls.Add(this.label1);
            this.pnlRecord.Controls.Add(this.chkIsActive);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtCatName);
            this.pnlRecord.Controls.Add(this.label3);
            this.pnlRecord.Controls.Add(this.txtCatNameID);
            this.pnlRecord.Controls.Add(this.lblCatNameID);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Location = new System.Drawing.Point(12, 87);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(500, 253);
            this.pnlRecord.TabIndex = 105;
            this.pnlRecord.Visible = false;
            // 
            // txtCatOrderTypeID
            // 
            this.txtCatOrderTypeID.BackColor = System.Drawing.Color.White;
            this.txtCatOrderTypeID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCatOrderTypeID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCatOrderTypeID.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtCatOrderTypeID.Location = new System.Drawing.Point(80, 122);
            this.txtCatOrderTypeID.MaxLength = 5;
            this.txtCatOrderTypeID.Name = "txtCatOrderTypeID";
            this.txtCatOrderTypeID.Size = new System.Drawing.Size(39, 21);
            this.txtCatOrderTypeID.TabIndex = 156;
            this.txtCatOrderTypeID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCatOrderTypeID_KeyPress);
            // 
            // picCatOrderTypes
            // 
            this.picCatOrderTypes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picCatOrderTypes.BackgroundImage")));
            this.picCatOrderTypes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picCatOrderTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCatOrderTypes.Location = new System.Drawing.Point(226, 122);
            this.picCatOrderTypes.Name = "picCatOrderTypes";
            this.picCatOrderTypes.Size = new System.Drawing.Size(19, 21);
            this.picCatOrderTypes.TabIndex = 159;
            this.picCatOrderTypes.TabStop = false;
            this.picCatOrderTypes.Click += new System.EventHandler(this.picCatOrderTypes_Click);
            // 
            // txtCatOrderTypeName
            // 
            this.txtCatOrderTypeName.BackColor = System.Drawing.Color.White;
            this.txtCatOrderTypeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCatOrderTypeName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCatOrderTypeName.Location = new System.Drawing.Point(125, 122);
            this.txtCatOrderTypeName.Name = "txtCatOrderTypeName";
            this.txtCatOrderTypeName.Size = new System.Drawing.Size(120, 21);
            this.txtCatOrderTypeName.TabIndex = 157;
            this.txtCatOrderTypeName.TextChanged += new System.EventHandler(this.txtCatOrderTypeName_TextChanged);
            this.txtCatOrderTypeName.Enter += new System.EventHandler(this.txtCatOrderTypeName_Enter);
            // 
            // dgvCatOrderTypes
            // 
            this.dgvCatOrderTypes.AllowUserToAddRows = false;
            this.dgvCatOrderTypes.AllowUserToDeleteRows = false;
            this.dgvCatOrderTypes.BackgroundColor = System.Drawing.Color.White;
            this.dgvCatOrderTypes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCatOrderTypes.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Moccasin;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCatOrderTypes.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCatOrderTypes.Location = new System.Drawing.Point(125, 142);
            this.dgvCatOrderTypes.Name = "dgvCatOrderTypes";
            this.dgvCatOrderTypes.ReadOnly = true;
            this.dgvCatOrderTypes.RowHeadersVisible = false;
            this.dgvCatOrderTypes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvCatOrderTypes.Size = new System.Drawing.Size(120, 80);
            this.dgvCatOrderTypes.TabIndex = 158;
            this.dgvCatOrderTypes.Visible = false;
            this.dgvCatOrderTypes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCatOrderTypes_CellContentClick);
            this.dgvCatOrderTypes.DoubleClick += new System.EventHandler(this.dgvCatOrderTypes_DoubleClick);
            this.dgvCatOrderTypes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvCatOrderTypes_KeyDown);
            this.dgvCatOrderTypes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvCatOrderTypes_KeyPress);
            this.dgvCatOrderTypes.Leave += new System.EventHandler(this.dgvCatOrderTypes_Leave);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(13, 122);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(2);
            this.label1.Size = new System.Drawing.Size(61, 21);
            this.label1.TabIndex = 107;
            this.label1.Text = "Type:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsActive.ForeColor = System.Drawing.Color.Red;
            this.chkIsActive.Location = new System.Drawing.Point(408, 39);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(61, 19);
            this.chkIsActive.TabIndex = 106;
            this.chkIsActive.Text = "Active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(422, -2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtCatName
            // 
            this.txtCatName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCatName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCatName.Location = new System.Drawing.Point(80, 95);
            this.txtCatName.MaxLength = 100;
            this.txtCatName.Name = "txtCatName";
            this.txtCatName.Size = new System.Drawing.Size(382, 21);
            this.txtCatName.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(13, 96);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(61, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "Name:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCatNameID
            // 
            this.txtCatNameID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCatNameID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCatNameID.Location = new System.Drawing.Point(80, 69);
            this.txtCatNameID.MaxLength = 5;
            this.txtCatNameID.Name = "txtCatNameID";
            this.txtCatNameID.Size = new System.Drawing.Size(61, 21);
            this.txtCatNameID.TabIndex = 0;
            // 
            // lblCatNameID
            // 
            this.lblCatNameID.BackColor = System.Drawing.Color.Transparent;
            this.lblCatNameID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCatNameID.ForeColor = System.Drawing.Color.Black;
            this.lblCatNameID.Location = new System.Drawing.Point(13, 69);
            this.lblCatNameID.Name = "lblCatNameID";
            this.lblCatNameID.Padding = new System.Windows.Forms.Padding(2);
            this.lblCatNameID.Size = new System.Drawing.Size(61, 21);
            this.lblCatNameID.TabIndex = 3;
            this.lblCatNameID.Text = "ID:";
            this.lblCatNameID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-2, -1);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(501, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "CATALOG NAMES";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CatalogNames
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1263, 657);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "CatalogNames";
            this.Tag = "CatalogNames";
            this.Text = "CatalogNames";
            this.Load += new System.EventHandler(this.CatalogNames_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CatalogNames_KeyDown);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCatOrderTypes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCatOrderTypes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Button btnClose;
        private GISControls.TextBoxChar txtCatName;
        private System.Windows.Forms.Label label3;
        private GISControls.TextBoxChar txtCatNameID;
        private System.Windows.Forms.Label lblCatNameID;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Label label1;
        private GISControls.TextBoxChar txtCatOrderTypeID;
        private System.Windows.Forms.PictureBox picCatOrderTypes;
        private GISControls.TextBoxChar txtCatOrderTypeName;
        private System.Windows.Forms.DataGridView dgvCatOrderTypes;
    }
}
