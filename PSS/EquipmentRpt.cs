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
    public partial class EquipmentRpt : Form
    {        
        public string rptName = "";
        public string pubEqptCode;
        public string pubEqptSrvcType;

        private ReportDocument crDoc = new ReportDocument();

        public EquipmentRpt()
        {
            InitializeComponent();
        }

        private void EquipmentRpt_Load(object sender, EventArgs e)
        {
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

            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;
            
            string rpt ="";
            if (rptName == "EqptValMstrPlan")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ValCalDatabase.rpt";
                sqlcmd = new SqlCommand("spGetValMstrPlan", sqlcnn);
            }
            else if (rptName == "EqptServiceRecord")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EqptServiceRecord.rpt";
                sqlcmd = new SqlCommand("spRptEqptServiceRecord", sqlcnn);
                sqlcmd.Parameters.AddWithValue("EqptCode", pubEqptCode);
                sqlcmd.Parameters.AddWithValue("SrvcType", pubEqptSrvcType);
                sqlcmd.CommandType = CommandType.StoredProcedure;
            }
            crDoc.Load(rpt);
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
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

        private void EquipmentRpt_FormClosing(object sender, FormClosingEventArgs e)
        {
            crReport.Dispose(); crReport = null;
            crDoc.Close(); crDoc.Dispose();
        }
    }
}
