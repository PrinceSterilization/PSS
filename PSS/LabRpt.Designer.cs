namespace PSS
{
    partial class LabRpt
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
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
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
            this.crReport.Size = new System.Drawing.Size(1020, 583);
            this.crReport.TabIndex = 0;
            this.crReport.Error += new CrystalDecisions.Windows.Forms.ExceptionEventHandler(this.crReport_Error);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // LabRpt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 583);
            this.Controls.Add(this.crReport);
            this.Name = "LabRpt";
            this.ShowInTaskbar = false;
            this.Text = "SALES REPORTS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LabRpt_FormClosing);
            this.Load += new System.EventHandler(this.LabRpt_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crReport;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
    }
}