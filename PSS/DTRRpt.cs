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
    public partial class DTRRpt : Form
    {
        public string rptName;
        public string rptLabel;
        public string rptLogDate;
        public string rptDateFrom;
        public string rptDateTo;
        public string rptDateFrom2;
        public string rptDateTo2;

        public Int16 rptEmpID;

        public DTRRpt()
        {
            InitializeComponent();
        }

        private void ReportsDTR_Load(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ReportDocument crDoc = new ReportDocument();
            SqlCommand sqlcmd = new SqlCommand();

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

            string rpt = "";

            DataTable dt = new DataTable();
            if (rptName == "DTA")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "DTA.rpt";
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["strPeriod"].Text = "'" + Convert.ToDateTime(rptLogDate).ToLongDateString().ToUpper() + "'";
                dt = PSSClass.DTR.RptDTA(Convert.ToDateTime(rptLogDate));
            }
            else if (rptName == "PTA" || rptName == "PTASummary")
            {
                if (rptName == "PTA")
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PTA.rpt";
                else
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PTASummary.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["strPeriod"].Text = "'" + rptLogDate + "'";
                dt = PSSClass.DTR.RptPTA(Convert.ToDateTime(rptDateFrom), Convert.ToDateTime(rptDateTo));
            }
            else if (rptName == "EmpPTA")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PTAEMp.rpt";
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["strPeriod"].Text = "'" + rptLogDate + "'";
                dt = PSSClass.DTR.RptPTAEmp(Convert.ToDateTime(rptDateFrom), Convert.ToDateTime(rptDateTo), rptEmpID);
            }
            else if (rptName == "PayrollTA")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\PayHoursDtls.rpt";
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["strPeriod"].Text = "'" + rptLogDate + "'";
                dt = PSSClass.DTR.RptPayrollHours(Convert.ToDateTime(rptDateFrom), Convert.ToDateTime(rptDateTo), Convert.ToDateTime(rptDateFrom2), Convert.ToDateTime(rptDateTo2));
            }
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
            crDoc.SetDataSource(dt);
            crReport.ReportSource = crDoc;
            crReport.Refresh();
        }
    }
}
