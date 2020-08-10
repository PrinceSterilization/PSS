namespace PSS
{
    partial class SalesRptSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SalesRptSettings));
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.rdoValueAsc = new System.Windows.Forms.RadioButton();
            this.rdoValueDesc = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoSponsor = new System.Windows.Forms.RadioButton();
            this.rdoQuoteNo = new System.Windows.Forms.RadioButton();
            this.txtYear = new GISControls.TextBoxChar();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancelPrint = new System.Windows.Forms.Button();
            this.btnOKPrint = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.btnClear = new System.Windows.Forms.Button();
            this.pnlSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSettings
            // 
            this.pnlSettings.Controls.Add(this.rdoValueAsc);
            this.pnlSettings.Controls.Add(this.rdoValueDesc);
            this.pnlSettings.Controls.Add(this.label2);
            this.pnlSettings.Controls.Add(this.rdoSponsor);
            this.pnlSettings.Controls.Add(this.rdoQuoteNo);
            this.pnlSettings.Controls.Add(this.txtYear);
            this.pnlSettings.Controls.Add(this.label1);
            this.pnlSettings.Location = new System.Drawing.Point(161, 52);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(201, 137);
            this.pnlSettings.TabIndex = 10;
            // 
            // rdoValueAsc
            // 
            this.rdoValueAsc.Location = new System.Drawing.Point(13, 117);
            this.rdoValueAsc.Name = "rdoValueAsc";
            this.rdoValueAsc.Size = new System.Drawing.Size(185, 17);
            this.rdoValueAsc.TabIndex = 13;
            this.rdoValueAsc.TabStop = true;
            this.rdoValueAsc.Text = "Sorted by Value - Ascending";
            this.rdoValueAsc.UseVisualStyleBackColor = true;
            // 
            // rdoValueDesc
            // 
            this.rdoValueDesc.Location = new System.Drawing.Point(12, 97);
            this.rdoValueDesc.Name = "rdoValueDesc";
            this.rdoValueDesc.Size = new System.Drawing.Size(185, 17);
            this.rdoValueDesc.TabIndex = 12;
            this.rdoValueDesc.TabStop = true;
            this.rdoValueDesc.Text = "Sorted by Value - Descending";
            this.rdoValueDesc.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "(Enter blank to print all years)";
            this.label2.Visible = false;
            // 
            // rdoSponsor
            // 
            this.rdoSponsor.Location = new System.Drawing.Point(11, 77);
            this.rdoSponsor.Name = "rdoSponsor";
            this.rdoSponsor.Size = new System.Drawing.Size(143, 17);
            this.rdoSponsor.TabIndex = 8;
            this.rdoSponsor.TabStop = true;
            this.rdoSponsor.Text = "Sorted by Sponsor Name";
            this.rdoSponsor.UseVisualStyleBackColor = true;
            // 
            // rdoQuoteNo
            // 
            this.rdoQuoteNo.Checked = true;
            this.rdoQuoteNo.Location = new System.Drawing.Point(11, 58);
            this.rdoQuoteNo.Name = "rdoQuoteNo";
            this.rdoQuoteNo.Size = new System.Drawing.Size(143, 20);
            this.rdoQuoteNo.TabIndex = 7;
            this.rdoQuoteNo.TabStop = true;
            this.rdoQuoteNo.Text = "Sorted by Quotation No.";
            this.rdoQuoteNo.UseVisualStyleBackColor = true;
            // 
            // txtYear
            // 
            this.txtYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYear.Location = new System.Drawing.Point(48, 12);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(45, 20);
            this.txtYear.TabIndex = 6;
            this.txtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.AliceBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Year";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancelPrint
            // 
            this.btnCancelPrint.Location = new System.Drawing.Point(290, 204);
            this.btnCancelPrint.Name = "btnCancelPrint";
            this.btnCancelPrint.Size = new System.Drawing.Size(72, 27);
            this.btnCancelPrint.TabIndex = 9;
            this.btnCancelPrint.Text = "Cl&ose";
            this.btnCancelPrint.UseVisualStyleBackColor = true;
            this.btnCancelPrint.Click += new System.EventHandler(this.btnCancelPrint_Click);
            // 
            // btnOKPrint
            // 
            this.btnOKPrint.Location = new System.Drawing.Point(212, 204);
            this.btnOKPrint.Name = "btnOKPrint";
            this.btnOKPrint.Size = new System.Drawing.Size(72, 27);
            this.btnOKPrint.TabIndex = 8;
            this.btnOKPrint.Text = "O&K";
            this.btnOKPrint.UseVisualStyleBackColor = true;
            this.btnOKPrint.Click += new System.EventHandler(this.btnOKPrint_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(55, 52);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.Location = new System.Drawing.Point(36, 31);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(346, 216);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(417, 289);
            this.shapeContainer1.TabIndex = 12;
            this.shapeContainer1.TabStop = false;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(115, 204);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(91, 27);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "&Clear Selection";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // SalesRptSettings
            // 
            this.AcceptButton = this.btnOKPrint;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 289);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pnlSettings);
            this.Controls.Add(this.btnCancelPrint);
            this.Controls.Add(this.btnOKPrint);
            this.Controls.Add(this.shapeContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SalesRptSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sales Report Settings";
            this.Load += new System.EventHandler(this.SalesRptSettings_Load);
            this.pnlSettings.ResumeLayout(false);
            this.pnlSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.RadioButton rdoSponsor;
        private System.Windows.Forms.RadioButton rdoQuoteNo;
        private GISControls.TextBoxChar txtYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancelPrint;
        private System.Windows.Forms.Button btnOKPrint;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoValueAsc;
        private System.Windows.Forms.RadioButton rdoValueDesc;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private System.Windows.Forms.Button btnClear;
    }
}