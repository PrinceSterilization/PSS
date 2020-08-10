namespace PSS
{
    partial class Location
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
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtLocationID = new System.Windows.Forms.TextBox();
            this.txtLocationDesc = new System.Windows.Forms.TextBox();
            this.lblLocationDesc = new System.Windows.Forms.Label();
            this.lblLocationID = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape2 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
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
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtLocationID);
            this.pnlRecord.Controls.Add(this.txtLocationDesc);
            this.pnlRecord.Controls.Add(this.lblLocationDesc);
            this.pnlRecord.Controls.Add(this.lblLocationID);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.shapeContainer1);
            this.pnlRecord.Location = new System.Drawing.Point(12, 88);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(563, 222);
            this.pnlRecord.TabIndex = 110;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(490, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 22);
            this.btnClose.TabIndex = 395;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtLocationID
            // 
            this.txtLocationID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLocationID.ForeColor = System.Drawing.Color.Red;
            this.txtLocationID.Location = new System.Drawing.Point(134, 92);
            this.txtLocationID.MaxLength = 4;
            this.txtLocationID.Name = "txtLocationID";
            this.txtLocationID.ReadOnly = true;
            this.txtLocationID.Size = new System.Drawing.Size(80, 21);
            this.txtLocationID.TabIndex = 0;
            this.txtLocationID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLocationDesc
            // 
            this.txtLocationDesc.Location = new System.Drawing.Point(134, 119);
            this.txtLocationDesc.Name = "txtLocationDesc";
            this.txtLocationDesc.Size = new System.Drawing.Size(359, 21);
            this.txtLocationDesc.TabIndex = 1;
            // 
            // lblLocationDesc
            // 
            this.lblLocationDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblLocationDesc.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocationDesc.ForeColor = System.Drawing.Color.Black;
            this.lblLocationDesc.Location = new System.Drawing.Point(46, 120);
            this.lblLocationDesc.Name = "lblLocationDesc";
            this.lblLocationDesc.Padding = new System.Windows.Forms.Padding(2);
            this.lblLocationDesc.Size = new System.Drawing.Size(82, 21);
            this.lblLocationDesc.TabIndex = 166;
            this.lblLocationDesc.Text = "Description:";
            this.lblLocationDesc.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblLocationID
            // 
            this.lblLocationID.BackColor = System.Drawing.Color.Transparent;
            this.lblLocationID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocationID.ForeColor = System.Drawing.Color.Black;
            this.lblLocationID.Location = new System.Drawing.Point(52, 95);
            this.lblLocationID.Name = "lblLocationID";
            this.lblLocationID.Padding = new System.Windows.Forms.Padding(2);
            this.lblLocationID.Size = new System.Drawing.Size(79, 21);
            this.lblLocationID.TabIndex = 162;
            this.lblLocationID.Text = "Location ID:";
            this.lblLocationID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(559, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "LOCATION MASTER";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape2});
            this.shapeContainer1.Size = new System.Drawing.Size(561, 220);
            this.shapeContainer1.TabIndex = 184;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape2
            // 
            this.rectangleShape2.Location = new System.Drawing.Point(28, 52);
            this.rectangleShape2.Name = "rectangleShape2";
            this.rectangleShape2.Size = new System.Drawing.Size(502, 132);
            // 
            // Location
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1676, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "Location";
            this.Activated += new System.EventHandler(this.Location_Activated);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Location_KeyDown);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.TextBox txtLocationID;
        private System.Windows.Forms.TextBox txtLocationDesc;
        private System.Windows.Forms.Label lblLocationDesc;
        private System.Windows.Forms.Label lblLocationID;
        private System.Windows.Forms.Label lblHeader;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape2;
        private System.Windows.Forms.Button btnClose;
    }
}
