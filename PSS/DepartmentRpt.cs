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
    public partial class DepartmentRpt : Form
    {
        public string rptName;
        public string rptLabel;

        public DepartmentRpt()
        {
            InitializeComponent();
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            string rpt = "";
            string strServer = "PSSQL01";
            string strDBName = "PTS";
            ReportDocument crDoc = new ReportDocument();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            this.Text = rptLabel;

            if (rptName == "DeptCode" || rptName == "DeptName")
            {
                //rpt = @"\\gblnj6\d$\ACCPAC\PROGRAM FILES\CRYSTAL REPORTS\" + "DepartmentNames.rpt";
                //rpt = @"\\glrds01\e$\GIS\Reports\" + "DepartmentNames.rpt";
                rpt = @"\\gblnj4\GIS\Reports\" + "DepartmentNames.rpt";
                //rpt = Application.StartupPath + @"\Reports\" + "DepartmentsRef.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer ;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);
                DataTable dt = new DataTable();
                if (rptName == "DeptCode")
                    dt = PSSClass.Departments.DepartmentsMaster(1);
                else
                    dt = PSSClass.Departments.DepartmentsMaster(2);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
        }
    }
}
