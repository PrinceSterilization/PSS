using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

namespace PSS
{
    public partial class DashboardYearlyRpt : Form
    {
        public DateTime StartDate;
        public DateTime EndDate;
        public string pubDashBoardTable; 

        public DashboardYearlyRpt()
        {
            InitializeComponent();
        }

        private void DashboardYearlyRpt_Load(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.MDFConnection("172.16.4.12", "PTSFinancials", true, "", "", "");
            //SqlConnection sqlcnn = PSSClass.DBConnection.MDFConnection("172.16.4.12", "PTS", true, "", "", "");
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ReportDocument crDoc = new ReportDocument();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

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

            string rptName;
            if (pubDashBoardTable == "Dashboard")
                rptName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "DashboardYearlyReport.rpt";
            else
                rptName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "DashboardYearlySter.rpt";


            crDoc.Load(rptName);
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }

            crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + StartDate.ToShortDateString() + "'";
            crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + EndDate.ToShortDateString() + "'";
            if (pubDashBoardTable == "Dashboard")
            {
                sqlcmd = new SqlCommand("spGetDashboardMonthly", sqlcnn);
            }
            else
            {
                sqlcmd = new SqlCommand("spGetDashboardMonthlySter", sqlcnn);
            }
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@StartDate", StartDate);
            sqlcmd.Parameters.AddWithValue("@EndDate", EndDate);

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
    }
}
