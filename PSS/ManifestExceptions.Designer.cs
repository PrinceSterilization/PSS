namespace GIS
{
    partial class ManifestExceptions
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
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.chkIsNestle = new System.Windows.Forms.CheckBox();
            this.chkIsLeprino = new System.Windows.Forms.CheckBox();
            this.chkIsRapid = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtFillCode = new GISControls.TextBoxChar();
            this.lblFillCode = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape2 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.chkIsActive);
            this.pnlRecord.Controls.Add(this.chkIsNestle);
            this.pnlRecord.Controls.Add(this.chkIsLeprino);
            this.pnlRecord.Controls.Add(this.chkIsRapid);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtFillCode);
            this.pnlRecord.Controls.Add(this.lblFillCode);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Controls.Add(this.shapeContainer1);
            this.pnlRecord.Location = new System.Drawing.Point(135, 75);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(376, 254);
            this.pnlRecord.TabIndex = 106;
            this.pnlRecord.Visible = false;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsActive.ForeColor = System.Drawing.Color.Red;
            this.chkIsActive.Location = new System.Drawing.Point(286, 42);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(61, 19);
            this.chkIsActive.TabIndex = 109;
            this.chkIsActive.Text = "Active";
            this.chkIsActive.UseVisualStyleBackColor = true;
            // 
            // chkIsNestle
            // 
            this.chkIsNestle.AutoSize = true;
            this.chkIsNestle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsNestle.ForeColor = System.Drawing.Color.Black;
            this.chkIsNestle.Location = new System.Drawing.Point(123, 188);
            this.chkIsNestle.Name = "chkIsNestle";
            this.chkIsNestle.Size = new System.Drawing.Size(62, 19);
            this.chkIsNestle.TabIndex = 108;
            this.chkIsNestle.Text = "Nestle";
            this.chkIsNestle.UseVisualStyleBackColor = true;
            // 
            // chkIsLeprino
            // 
            this.chkIsLeprino.AutoSize = true;
            this.chkIsLeprino.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsLeprino.ForeColor = System.Drawing.Color.Black;
            this.chkIsLeprino.Location = new System.Drawing.Point(123, 163);
            this.chkIsLeprino.Name = "chkIsLeprino";
            this.chkIsLeprino.Size = new System.Drawing.Size(68, 19);
            this.chkIsLeprino.TabIndex = 107;
            this.chkIsLeprino.Text = "Leprino";
            this.chkIsLeprino.UseVisualStyleBackColor = true;
            // 
            // chkIsRapid
            // 
            this.chkIsRapid.AutoSize = true;
            this.chkIsRapid.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIsRapid.ForeColor = System.Drawing.Color.Black;
            this.chkIsRapid.Location = new System.Drawing.Point(123, 138);
            this.chkIsRapid.Name = "chkIsRapid";
            this.chkIsRapid.Size = new System.Drawing.Size(102, 19);
            this.chkIsRapid.TabIndex = 106;
            this.chkIsRapid.Text = "Rapid Method";
            this.chkIsRapid.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(297, -2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtFillCode
            // 
            this.txtFillCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFillCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFillCode.Location = new System.Drawing.Point(123, 80);
            this.txtFillCode.MaxLength = 15;
            this.txtFillCode.Name = "txtFillCode";
            this.txtFillCode.Size = new System.Drawing.Size(124, 21);
            this.txtFillCode.TabIndex = 0;
            // 
            // lblFillCode
            // 
            this.lblFillCode.BackColor = System.Drawing.Color.Transparent;
            this.lblFillCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFillCode.ForeColor = System.Drawing.Color.Black;
            this.lblFillCode.Location = new System.Drawing.Point(51, 80);
            this.lblFillCode.Name = "lblFillCode";
            this.lblFillCode.Padding = new System.Windows.Forms.Padding(2);
            this.lblFillCode.Size = new System.Drawing.Size(66, 21);
            this.lblFillCode.TabIndex = 3;
            this.lblFillCode.Text = "Fill Code:";
            this.lblFillCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-2, -1);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(375, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "MANIFEST EXCEPTIONS";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseDown);
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape2,
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(374, 252);
            this.shapeContainer1.TabIndex = 110;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape2
            // 
            this.rectangleShape2.Location = new System.Drawing.Point(31, 66);
            this.rectangleShape2.Name = "rectangleShape2";
            this.rectangleShape2.Size = new System.Drawing.Size(310, 46);
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.Location = new System.Drawing.Point(31, 117);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(310, 104);
            // 
            // ManifestExceptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1916, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ManifestExceptions";
            this.Activated += new System.EventHandler(this.ManifestExceptions_Activated);
            this.Load += new System.EventHandler(this.ManifestExceptions_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManifestExceptions_KeyDown);
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
        private System.Windows.Forms.CheckBox chkIsRapid;
        private System.Windows.Forms.Button btnClose;
        private GISControls.TextBoxChar txtFillCode;
        private System.Windows.Forms.Label lblFillCode;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.CheckBox chkIsNestle;
        private System.Windows.Forms.CheckBox chkIsLeprino;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape2;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
    }
}
