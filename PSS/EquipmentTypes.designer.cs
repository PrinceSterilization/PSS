namespace PSS
{
    partial class EquipmentTypes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EquipmentTypes));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.btnDeleteFreq = new System.Windows.Forms.Button();
            this.btnAddFreq = new System.Windows.Forms.Button();
            this.btnCancelFreq = new System.Windows.Forms.Button();
            this.btnOKFreq = new System.Windows.Forms.Button();
            this.dgvFreq = new System.Windows.Forms.DataGridView();
            this.txtFreq = new GISControls.TextBoxChar();
            this.cboUnit = new System.Windows.Forms.ComboBox();
            this.cboSrvcType = new System.Windows.Forms.ComboBox();
            this.lbl5 = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.txtDesc = new GISControls.TextBoxChar();
            this.lbl1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtID = new GISControls.TextBoxChar();
            this.lblHeader = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.bsFreq = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFreq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFreq)).BeginInit();
            this.SuspendLayout();
            // 
            // chkFullText
            // 
            this.chkFullText.Visible = false;
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.btnDeleteFreq);
            this.pnlRecord.Controls.Add(this.btnAddFreq);
            this.pnlRecord.Controls.Add(this.btnCancelFreq);
            this.pnlRecord.Controls.Add(this.btnOKFreq);
            this.pnlRecord.Controls.Add(this.dgvFreq);
            this.pnlRecord.Controls.Add(this.txtFreq);
            this.pnlRecord.Controls.Add(this.cboUnit);
            this.pnlRecord.Controls.Add(this.cboSrvcType);
            this.pnlRecord.Controls.Add(this.lbl5);
            this.pnlRecord.Controls.Add(this.lbl4);
            this.pnlRecord.Controls.Add(this.lbl2);
            this.pnlRecord.Controls.Add(this.lbl3);
            this.pnlRecord.Controls.Add(this.txtDesc);
            this.pnlRecord.Controls.Add(this.lbl1);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtID);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.shapeContainer1);
            this.pnlRecord.Location = new System.Drawing.Point(12, 88);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(690, 277);
            this.pnlRecord.TabIndex = 105;
            this.pnlRecord.Visible = false;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // btnDeleteFreq
            // 
            this.btnDeleteFreq.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteFreq.Image")));
            this.btnDeleteFreq.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeleteFreq.Location = new System.Drawing.Point(589, 75);
            this.btnDeleteFreq.Name = "btnDeleteFreq";
            this.btnDeleteFreq.Size = new System.Drawing.Size(64, 22);
            this.btnDeleteFreq.TabIndex = 8;
            this.btnDeleteFreq.Text = "Delete";
            this.btnDeleteFreq.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteFreq.UseVisualStyleBackColor = true;
            this.btnDeleteFreq.Click += new System.EventHandler(this.btnDeleteFreq_Click);
            // 
            // btnAddFreq
            // 
            this.btnAddFreq.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFreq.Image")));
            this.btnAddFreq.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAddFreq.Location = new System.Drawing.Point(523, 75);
            this.btnAddFreq.Name = "btnAddFreq";
            this.btnAddFreq.Size = new System.Drawing.Size(64, 22);
            this.btnAddFreq.TabIndex = 2;
            this.btnAddFreq.Text = "Add";
            this.btnAddFreq.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnAddFreq.UseVisualStyleBackColor = true;
            this.btnAddFreq.Click += new System.EventHandler(this.btnAddFreq_Click);
            // 
            // btnCancelFreq
            // 
            this.btnCancelFreq.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancelFreq.Location = new System.Drawing.Point(589, 75);
            this.btnCancelFreq.Name = "btnCancelFreq";
            this.btnCancelFreq.Size = new System.Drawing.Size(64, 22);
            this.btnCancelFreq.TabIndex = 7;
            this.btnCancelFreq.Text = "Cancel";
            this.btnCancelFreq.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnCancelFreq.UseVisualStyleBackColor = true;
            this.btnCancelFreq.Click += new System.EventHandler(this.btnCancelFreq_Click);
            // 
            // btnOKFreq
            // 
            this.btnOKFreq.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOKFreq.Location = new System.Drawing.Point(523, 75);
            this.btnOKFreq.Name = "btnOKFreq";
            this.btnOKFreq.Size = new System.Drawing.Size(64, 22);
            this.btnOKFreq.TabIndex = 6;
            this.btnOKFreq.Text = "OK";
            this.btnOKFreq.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnOKFreq.UseVisualStyleBackColor = true;
            this.btnOKFreq.Click += new System.EventHandler(this.btnOKFreq_Click);
            // 
            // dgvFreq
            // 
            this.dgvFreq.AllowUserToAddRows = false;
            this.dgvFreq.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Bisque;
            this.dgvFreq.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFreq.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.dgvFreq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFreq.Location = new System.Drawing.Point(20, 100);
            this.dgvFreq.Name = "dgvFreq";
            this.dgvFreq.ReadOnly = true;
            this.dgvFreq.RowHeadersWidth = 20;
            this.dgvFreq.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvFreq.Size = new System.Drawing.Size(354, 156);
            this.dgvFreq.TabIndex = 9;
            this.dgvFreq.CurrentCellChanged += new System.EventHandler(this.dgvFreq_CurrentCellChanged);
            // 
            // txtFreq
            // 
            this.txtFreq.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFreq.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFreq.Location = new System.Drawing.Point(473, 176);
            this.txtFreq.MaxLength = 50;
            this.txtFreq.Name = "txtFreq";
            this.txtFreq.Size = new System.Drawing.Size(80, 21);
            this.txtFreq.TabIndex = 5;
            this.txtFreq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFreq_KeyPress);
            this.txtFreq.LostFocus += new System.EventHandler(this.txtFreq_LostFocus);
            // 
            // cboUnit
            // 
            this.cboUnit.BackColor = System.Drawing.Color.White;
            this.cboUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUnit.FormattingEnabled = true;
            this.cboUnit.Items.AddRange(new object[] {
            "Weeks",
            "Months",
            "Years"});
            this.cboUnit.Location = new System.Drawing.Point(473, 144);
            this.cboUnit.Name = "cboUnit";
            this.cboUnit.Size = new System.Drawing.Size(180, 23);
            this.cboUnit.TabIndex = 4;
            this.cboUnit.SelectedIndexChanged += new System.EventHandler(this.cboUnit_SelectedIndexChanged);
            // 
            // cboSrvcType
            // 
            this.cboSrvcType.BackColor = System.Drawing.Color.White;
            this.cboSrvcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSrvcType.FormattingEnabled = true;
            this.cboSrvcType.Location = new System.Drawing.Point(473, 112);
            this.cboSrvcType.Name = "cboSrvcType";
            this.cboSrvcType.Size = new System.Drawing.Size(180, 23);
            this.cboSrvcType.TabIndex = 3;
            this.cboSrvcType.SelectedIndexChanged += new System.EventHandler(this.cboSrvcType_SelectedIndexChanged);
            // 
            // lbl5
            // 
            this.lbl5.BackColor = System.Drawing.Color.Transparent;
            this.lbl5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl5.ForeColor = System.Drawing.Color.Black;
            this.lbl5.Location = new System.Drawing.Point(391, 176);
            this.lbl5.Name = "lbl5";
            this.lbl5.Padding = new System.Windows.Forms.Padding(2);
            this.lbl5.Size = new System.Drawing.Size(70, 21);
            this.lbl5.TabIndex = 15;
            this.lbl5.Text = "Frequency";
            this.lbl5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl4
            // 
            this.lbl4.BackColor = System.Drawing.Color.Transparent;
            this.lbl4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl4.ForeColor = System.Drawing.Color.Black;
            this.lbl4.Location = new System.Drawing.Point(391, 144);
            this.lbl4.Name = "lbl4";
            this.lbl4.Padding = new System.Windows.Forms.Padding(2);
            this.lbl4.Size = new System.Drawing.Size(70, 21);
            this.lbl4.TabIndex = 14;
            this.lbl4.Text = "Units";
            this.lbl4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl2
            // 
            this.lbl2.BackColor = System.Drawing.Color.Transparent;
            this.lbl2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2.ForeColor = System.Drawing.Color.Black;
            this.lbl2.Location = new System.Drawing.Point(20, 75);
            this.lbl2.Name = "lbl2";
            this.lbl2.Padding = new System.Windows.Forms.Padding(2);
            this.lbl2.Size = new System.Drawing.Size(125, 21);
            this.lbl2.TabIndex = 11;
            this.lbl2.Text = "Service Frequency";
            this.lbl2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl3
            // 
            this.lbl3.BackColor = System.Drawing.Color.Transparent;
            this.lbl3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3.ForeColor = System.Drawing.Color.Black;
            this.lbl3.Location = new System.Drawing.Point(391, 112);
            this.lbl3.Name = "lbl3";
            this.lbl3.Padding = new System.Windows.Forms.Padding(2);
            this.lbl3.Size = new System.Drawing.Size(60, 21);
            this.lbl3.TabIndex = 13;
            this.lbl3.Text = "Service Type";
            this.lbl3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDesc
            // 
            this.txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDesc.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDesc.Location = new System.Drawing.Point(100, 39);
            this.txtDesc.MaxLength = 50;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(250, 23);
            this.txtDesc.TabIndex = 1;
            // 
            // lbl1
            // 
            this.lbl1.BackColor = System.Drawing.Color.Transparent;
            this.lbl1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1.ForeColor = System.Drawing.Color.Black;
            this.lbl1.Location = new System.Drawing.Point(20, 40);
            this.lbl1.Name = "lbl1";
            this.lbl1.Padding = new System.Windows.Forms.Padding(2);
            this.lbl1.Size = new System.Drawing.Size(90, 21);
            this.lbl1.TabIndex = 10;
            this.lbl1.Text = "Equipment";
            this.lbl1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(613, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtID
            // 
            this.txtID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtID.Location = new System.Drawing.Point(300, 40);
            this.txtID.MaxLength = 50;
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(50, 21);
            this.txtID.TabIndex = 21;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(690, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Equipment Type";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(688, 275);
            this.shapeContainer1.TabIndex = 18;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.BorderColor = System.Drawing.Color.SteelBlue;
            this.rectangleShape1.Location = new System.Drawing.Point(378, 100);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(292, 155);
            // 
            // EquipmentTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1916, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "EquipmentTypes";
            this.Load += new System.EventHandler(this.EquipmentType_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EquipmentType_KeyDown);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFreq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFreq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Button btnClose;
        private GISControls.TextBoxChar txtID;
        private System.Windows.Forms.Label lblHeader;
        private GISControls.TextBoxChar txtDesc;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.ComboBox cboSrvcType;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private GISControls.TextBoxChar txtFreq;
        private System.Windows.Forms.ComboBox cboUnit;
        private System.Windows.Forms.BindingSource bsFreq;
        private System.Windows.Forms.Button btnDeleteFreq;
        private System.Windows.Forms.Button btnAddFreq;
        private System.Windows.Forms.Button btnCancelFreq;
        private System.Windows.Forms.Button btnOKFreq;
        private System.Windows.Forms.DataGridView dgvFreq;
    }
}
