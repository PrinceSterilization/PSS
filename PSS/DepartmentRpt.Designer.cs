﻿namespace PSS
{
    partial class DepartmentRpt
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
            this.crReport.Size = new System.Drawing.Size(876, 551);
            this.crReport.TabIndex = 0;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 551);
            this.Controls.Add(this.crReport);
            this.Name = "ReportsForm";
            this.Text = "ReportsForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ReportsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crReport;
    }
}