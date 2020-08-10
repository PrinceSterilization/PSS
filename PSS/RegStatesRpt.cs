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
    public partial class RegStatesRpt : Form
    {
        public string rptName;
        public string rptLabel;

        public RegStatesRpt()
        {
            InitializeComponent();
        }

        private void RptRegStates_Load(object sender, EventArgs e)
        {
            string rpt = "";
            string strServer = "PSSQL01";
            string strDBName = "PTS";
            ReportDocument crDoc = new ReportDocument();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            this.Text = rptLabel;

            if (rptName == "RegCode")
            {
                rpt = @"\\gblnj4\GIS\Reports\" + "DepartmentCodes.rpt";
                //rpt = Application.StartupPath + @"\Reports\" + "RegionsRef.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);
                DataTable dt = new DataTable();
                dt = PSSClass.Regions.RegionsMaster(1);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "RegName")
            {
                rpt = @"\\gblnj4\GIS\Reports\" + "DepartmentNames.rpt";
                //rpt = Application.StartupPath + @"\Reports\" + "RegionsRef.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);
                DataTable dt = new DataTable();
                dt = PSSClass.Regions.RegionsMaster(2);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "StatesRef")
            {
                rpt = @"\\gblnj4\GIS\Reports\" + "StatesRef.rpt";
                //rpt = Application.StartupPath + @"\Reports\" + "StatesRef.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);
                DataTable dt = new DataTable();
                dt = PSSClass.States.StatesMaster(1);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "StatesByRegion")
            {
                rpt = @"\\gblnj4\GIS\Reports\" + "StatesRef.rpt";
                //rpt = Application.StartupPath + @"\Reports\" + "StatesByRegion.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);
                DataTable dt = new DataTable();
                dt = PSSClass.States.StatesMaster(1);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
        }
    }
}
