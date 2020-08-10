namespace PSS
{
    partial class EquipmentServiceReport
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
            this.crReport = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.pnlReport = new System.Windows.Forms.Panel();
            this.pnlCalendar = new System.Windows.Forms.Panel();
            this.cal = new System.Windows.Forms.MonthCalendar();
            this.label9 = new System.Windows.Forms.Label();
            this.mskEndDate = new System.Windows.Forms.MaskedTextBox();
            this.mskStartDate = new System.Windows.Forms.MaskedTextBox();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.btnPrintCancel = new System.Windows.Forms.Button();
            this.lblReportOptions = new System.Windows.Forms.Label();
            this.btnPrintReport = new System.Windows.Forms.Button();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape6 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.pnlReport.SuspendLayout();
            this.pnlCalendar.SuspendLayout();
            this.SuspendLayout();
            // 
            // crReport
            // 
            this.crReport.ActiveViewIndex = -1;
            this.crReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crReport.Cursor = System.Windows.Forms.Cursors.Default;
            this.crReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crReport.Location = new System.Drawing.Point(0, 0);
            this.crReport.Name = "crReport";
            this.crReport.Size = new System.Drawing.Size(465, 462);
            this.crReport.TabIndex = 1;
            // 
            // pnlReport
            // 
            this.pnlReport.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlReport.Controls.Add(this.pnlCalendar);
            this.pnlReport.Controls.Add(this.mskEndDate);
            this.pnlReport.Controls.Add(this.mskStartDate);
            this.pnlReport.Controls.Add(this.lblEndDate);
            this.pnlReport.Controls.Add(this.lblStartDate);
            this.pnlReport.Controls.Add(this.btnPrintCancel);
            this.pnlReport.Controls.Add(this.lblReportOptions);
            this.pnlReport.Controls.Add(this.btnPrintReport);
            this.pnlReport.Controls.Add(this.shapeContainer2);
            this.pnlReport.Location = new System.Drawing.Point(103, 40);
            this.pnlReport.Name = "pnlReport";
            this.pnlReport.Size = new System.Drawing.Size(293, 396);
            this.pnlReport.TabIndex = 300;
            this.pnlReport.Visible = false;
            // 
            // pnlCalendar
            // 
            this.pnlCalendar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCalendar.Controls.Add(this.cal);
            this.pnlCalendar.Controls.Add(this.label9);
            this.pnlCalendar.Location = new System.Drawing.Point(24, 30);
            this.pnlCalendar.Name = "pnlCalendar";
            this.pnlCalendar.Size = new System.Drawing.Size(246, 185);
            this.pnlCalendar.TabIndex = 402;
            this.pnlCalendar.Visible = false;
            // 
            // cal
            // 
            this.cal.Location = new System.Drawing.Point(8, 9);
            this.cal.Name = "cal";
            this.cal.TabIndex = 277;
            this.cal.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.cal_DateSelected);
            this.cal.MouseLeave += new System.EventHandler(this.cal_MouseLeave);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(27, 128);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 21);
            this.label9.TabIndex = 152;
            this.label9.Text = "PO No.";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label9.Visible = false;
            // 
            // mskEndDate
            // 
            this.mskEndDate.Location = new System.Drawing.Point(150, 283);
            this.mskEndDate.Mask = "00/00/0000";
            this.mskEndDate.Name = "mskEndDate";
            this.mskEndDate.RejectInputOnFirstFailure = true;
            this.mskEndDate.Size = new System.Drawing.Size(70, 20);
            this.mskEndDate.TabIndex = 303;
            this.mskEndDate.ValidatingType = typeof(System.DateTime);
            this.mskEndDate.DoubleClick += new System.EventHandler(this.mskEndDate_DoubleClick);
            // 
            // mskStartDate
            // 
            this.mskStartDate.Location = new System.Drawing.Point(150, 254);
            this.mskStartDate.Mask = "00/00/0000";
            this.mskStartDate.Name = "mskStartDate";
            this.mskStartDate.RejectInputOnFirstFailure = true;
            this.mskStartDate.Size = new System.Drawing.Size(70, 20);
            this.mskStartDate.TabIndex = 302;
            this.mskStartDate.ValidatingType = typeof(System.DateTime);
            this.mskStartDate.DoubleClick += new System.EventHandler(this.mskStartDate_DoubleClick);
            // 
            // lblEndDate
            // 
            this.lblEndDate.BackColor = System.Drawing.Color.Transparent;
            this.lblEndDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndDate.ForeColor = System.Drawing.Color.Black;
            this.lblEndDate.Location = new System.Drawing.Point(57, 283);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblEndDate.Size = new System.Drawing.Size(87, 21);
            this.lblEndDate.TabIndex = 179;
            this.lblEndDate.Text = "End Date:";
            this.lblEndDate.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblStartDate
            // 
            this.lblStartDate.BackColor = System.Drawing.Color.Transparent;
            this.lblStartDate.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.ForeColor = System.Drawing.Color.Black;
            this.lblStartDate.Location = new System.Drawing.Point(57, 254);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Padding = new System.Windows.Forms.Padding(2);
            this.lblStartDate.Size = new System.Drawing.Size(87, 21);
            this.lblStartDate.TabIndex = 178;
            this.lblStartDate.Text = "Start Date:";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // btnPrintCancel
            // 
            this.btnPrintCancel.Location = new System.Drawing.Point(165, 341);
            this.btnPrintCancel.Name = "btnPrintCancel";
            this.btnPrintCancel.Size = new System.Drawing.Size(69, 23);
            this.btnPrintCancel.TabIndex = 10;
            this.btnPrintCancel.Text = "Cancel";
            this.btnPrintCancel.UseVisualStyleBackColor = true;
            this.btnPrintCancel.Click += new System.EventHandler(this.btnPrintCancel_Click);
            // 
            // lblReportOptions
            // 
            this.lblReportOptions.BackColor = System.Drawing.Color.SteelBlue;
            this.lblReportOptions.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportOptions.ForeColor = System.Drawing.Color.White;
            this.lblReportOptions.Location = new System.Drawing.Point(0, 0);
            this.lblReportOptions.Name = "lblReportOptions";
            this.lblReportOptions.Size = new System.Drawing.Size(292, 26);
            this.lblReportOptions.TabIndex = 0;
            this.lblReportOptions.Text = "Report Date Range";
            this.lblReportOptions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrintReport
            // 
            this.btnPrintReport.Location = new System.Drawing.Point(90, 341);
            this.btnPrintReport.Name = "btnPrintReport";
            this.btnPrintReport.Size = new System.Drawing.Size(69, 23);
            this.btnPrintReport.TabIndex = 9;
            this.btnPrintReport.Text = "Print";
            this.btnPrintReport.UseVisualStyleBackColor = true;
            this.btnPrintReport.Click += new System.EventHandler(this.btnPrintReport_Click);
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape6});
            this.shapeContainer2.Size = new System.Drawing.Size(291, 394);
            this.shapeContainer2.TabIndex = 7;
            this.shapeContainer2.TabStop = false;
            // 
            // rectangleShape6
            // 
            this.rectangleShape6.Location = new System.Drawing.Point(24, 238);
            this.rectangleShape6.Name = "rectangleShape1";
            this.rectangleShape6.Size = new System.Drawing.Size(245, 88);
            // 
            // EquipmentServiceReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 462);
            this.Controls.Add(this.pnlReport);
            this.Controls.Add(this.crReport);
            this.Name = "EquipmentServiceReport";
            this.Text = "EquipmentServiceReport";
            this.Load += new System.EventHandler(this.EquipmentServiceReport_Load);
            this.pnlReport.ResumeLayout(false);
            this.pnlReport.PerformLayout();
            this.pnlCalendar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crReport;
        private System.Windows.Forms.Panel pnlReport;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Button btnPrintCancel;
        private System.Windows.Forms.Label lblReportOptions;
        private System.Windows.Forms.Button btnPrintReport;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape6;
        private System.Windows.Forms.MaskedTextBox mskEndDate;
        private System.Windows.Forms.MaskedTextBox mskStartDate;
        private System.Windows.Forms.Panel pnlCalendar;
        private System.Windows.Forms.MonthCalendar cal;
        private System.Windows.Forms.Label label9;
    }
}