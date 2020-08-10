using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Drawing.Printing;

namespace PSS
{
    public partial class EquipmentServiceReport : Form
    {
        public string rptName = "";

        public EquipmentServiceReport()
        {
            InitializeComponent();
        }

        private void EquipmentServiceReport_Load(object sender, EventArgs e)
        {
            pnlReport.Visible = true;   
            pnlReport.Left = (this.ClientSize.Width - pnlReport.Width) / 2 ;
            pnlReport.Top = (this.ClientSize.Height - pnlReport.Height) / 2;
        }

        private void LoadReport()
        {
            pnlReport.Visible = false;
            
            Tables CrTables;
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            crConnectionInfo.Type = ConnectionInfoType.SQL;
            crConnectionInfo.ServerName = "172.16.4.12";
            crConnectionInfo.DatabaseName = "PTS";
            crConnectionInfo.IntegratedSecurity = false;
            crConnectionInfo.UserID = "sa";
            crConnectionInfo.Password = "Pass2018";
            crtableLogoninfo.ConnectionInfo = crConnectionInfo;

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ReportDocument crDoc = new ReportDocument();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;
            string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EquipmentServiceReport.rpt";
            crDoc.Load(rpt);
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
            sqlcmd = new SqlCommand("spRptEqptSrvcReport", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;           

            sqlcmd.Parameters.AddWithValue("@StartDate", Convert.ToDateTime(mskStartDate.Text));
            sqlcmd.Parameters.AddWithValue("@EndDate", Convert.ToDateTime(mskEndDate.Text));

            sqldr = sqlcmd.ExecuteReader();

            DataTable dTable = new DataTable();

            try
            {
                dTable.Load(sqldr);
                sqlcnn.Dispose();
            }
            catch
            {
                sqlcnn.Dispose();
            }
            crDoc.SetDataSource(dTable);
            crReport.ReportSource = crDoc;
            crReport.Refresh();
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {          
            if (mskStartDate.MaskFull == false)
            {
                MessageBox.Show("Start Date is empty!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mskStartDate.Focus();
                return;
            }

            if (mskEndDate.MaskFull == false)
            {
                MessageBox.Show("End Date is empty!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mskEndDate.Focus();
                return;
            }

            int result = DateTime.Compare(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text));

            if (result > 0)
            {
                MessageBox.Show("Start Date is greater than End Date!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mskStartDate.Focus();
                return;
            }

            LoadReport();
        }

        private void btnPrintCancel_Click(object sender, EventArgs e)
        {
            pnlReport.Visible = false;
            this.Dispose(); ;
        }

        // MY 08/14/2015 - Start: Date events          
        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (pnlCalendar.Location == new Point(24, 30))
            {
                mskStartDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            else if (pnlCalendar.Location == new Point(24, 49))
            {
                mskEndDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }           

            pnlCalendar.Visible = false;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void mskStartDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.BringToFront(); pnlCalendar.Location = new Point(24, 30);         
        }

        private void mskEndDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.BringToFront(); pnlCalendar.Location = new Point(24, 49);
        }
           
       // MY 08/14/2015 - End: End Date events  

    }
}
