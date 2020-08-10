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
    public partial class RptSC : Form
    {
        public string rptName;
        public string rptLabel;
        public string rptFileName;

        public RptSC()
        {
            InitializeComponent();
        }

        private void RptMasterFiles_Load(object sender, EventArgs e)
        {
            string rpt = "";
            string strServer = "GLSQL03";
            string strDBName = "PSS";
            ReportDocument crDoc = new ReportDocument();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            this.Text = rptLabel;

            if (rptName == "SCRef")
            {
                //rpt = @"\\gblnj4\GIS\Reports\" + "SCRef.rpt";
                rpt = Application.StartupPath + @"\Reports\" + "SCRef.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cStatus"].Text = "'" + "1" + "'";

                DataTable dt = new DataTable();
                dt = PSSClass.ServiceCodes.SCMaster(1);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "SCDesc")
            {
                //rpt = @"\\gblnj4\GIS\Reports\" + "SCDesc.rpt";
                rpt = Application.StartupPath + @"\Reports\" + "SCDesc.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);

                DataTable dt = new DataTable();
                dt = PSSClass.ServiceCodes.SCMaster(2);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            //else if (rptName == "SCDept")
            //{
            //    //rpt = @"\\gblnj4\GIS\Reports\" + "SCDept.rpt";
            //    rpt = Application.StartupPath + @"\Reports\" + "SCDept.rpt";
            //    crConnectionInfo.Type = ConnectionInfoType.SQL;
            //    crConnectionInfo.ServerName = strServer;
            //    crConnectionInfo.DatabaseName = strDBName;
            //    crConnectionInfo.IntegratedSecurity = true;
            //    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
            //    crDoc.Load(rpt);

            //    DataTable dt = new DataTable();
            //    dt = PSSClass.ServiceCodes.SCMaster(1);
            //    crDoc.Load(rpt);
            //    crDoc.SetDataSource(dt);
            //    crReport.ReportSource = crDoc;
            //    crReport.Refresh();
            //}
            else if (rptName == "SCDuration")
            {
                //rpt = @"\\gblnj4\GIS\Reports\" + "SCDuration.rpt";
                rpt = Application.StartupPath + @"\Reports\" + "SCDuration.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);

                DataTable dt = new DataTable();
                dt = PSSClass.ServiceCodes.SCMaster(1);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "SCGLCode")
            {
                //rpt = @"\\gblnj4\GIS\Reports\" + "SCGLCode.rpt";
                rpt = Application.StartupPath + @"\Reports\" + "SCGLCode.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);

                DataTable dt = new DataTable();
                dt = PSSClass.ServiceCodes.SCMaster(1);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "SCInactive")
            {
                //rpt = @"\\gblnj4\GIS\Reports\" + "SCRef.rpt";
                rpt = Application.StartupPath + @"\Reports\" + "SCRef.rpt";
                crConnectionInfo.Type = ConnectionInfoType.SQL;
                crConnectionInfo.ServerName = strServer;
                crConnectionInfo.DatabaseName = strDBName;
                crConnectionInfo.IntegratedSecurity = true;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cStatus"].Text = "'" + "0" + "'";

                DataTable dt = new DataTable();
                dt = PSSClass.ServiceCodes.SCMaster(2);
                crDoc.Load(rpt);
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            //else if (rptName == "AuditTrail")
            //{
            //    DataTable dtRpt = new DataTable();
            //    dtRpt.Columns.Add("TableName", typeof(string));
            //    dtRpt.Columns.Add("AuditID", typeof(int));
            //    dtRpt.Columns.Add("ColumnName", typeof(string));
            //    dtRpt.Columns.Add("CurrentValue", typeof(string));
            //    dtRpt.Columns.Add("ActionTaken", typeof(int));
            //    dtRpt.Columns.Add("ActionTakenBy", typeof(string));
            //    dtRpt.Columns.Add("ActionDate", typeof(DateTime));

            //    DataTable dtSC = PSSClass.AuditReport.AuditSC();
            //    if (dtSC != null && dtSC.Rows.Count > 0)
            //    {
            //        int nSC = 0; int nIDx = 0;
            //        for (int i = 0; i < dtSC.Rows.Count; i++)
            //        {
            //            if (nSC != Convert.ToInt16(dtSC.Rows[i]["ServiceCode"]))
            //            {
            //                nSC = Convert.ToInt16(dtSC.Rows[i]["ServiceCode"]);
            //                nIDx = i;
            //            }
            //            else
            //            {
            //                int nX = 0;
            //                foreach (DataColumn dc in dtSC.Columns)
            //                {
            //                    if (dtSC.Rows[nIDx][dc.ColumnName].ToString() != dtSC.Rows[i][dc.ColumnName].ToString() && dc.ColumnName.ToString().IndexOf("Action") == -1)
            //                    {
            //                        nX = 1;
            //                        DataRow dR = dtRpt.NewRow();
            //                        dR["TableName"] = "Service Codes";
            //                        dR["AuditID"] = dtSC.Rows[i]["AuditID"];
            //                        dR["ColumnName"] = dc.ColumnName;
            //                        dR["CurrentValue"] = dtSC.Rows[i][dc.ColumnName];
            //                        dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
            //                        dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
            //                        dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
            //                        dtRpt.Rows.Add(dR);
            //                    }
            //                }
            //                if (nX == 1)
            //                {
            //                    DataRow dR = dtRpt.NewRow();
            //                    dR["TableName"] = "Service Codes";
            //                    dR["AuditID"] = dtSC.Rows[i]["AuditID"];
            //                    dR["ColumnName"] = "ServiceCode";
            //                    dR["CurrentValue"] = dtSC.Rows[i]["ServiceCode"];
            //                    dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
            //                    dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
            //                    dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
            //                    dtRpt.Rows.Add(dR);
            //                    nX = 0;
            //                }
            //            }
            //        }
            //    }
            //    rpt = @"\\gblnj4\GIS\Reports\" + "AuditReport.rpt";
            //    //rpt = Application.StartupPath + @"\Reports\" + "SCRef.rpt";
            //    crConnectionInfo.Type = ConnectionInfoType.SQL;
            //    crConnectionInfo.ServerName = strServer;
            //    crConnectionInfo.DatabaseName = strDBName;
            //    crConnectionInfo.IntegratedSecurity = true;
            //    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
            //    crDoc.Load(rpt);
            //    if (rptName == "AuditTrail")
            //        crDoc.DataDefinition.FormulaFields["cFileName"].Text = "'" + rptFileName + "'";

            //    //DataTable dt = new DataTable();
            //    //dt = PSSClass.ServiceCodes.SCMaster(1);
            //    crDoc.Load(rpt);
            //    crDoc.SetDataSource(dtRpt);
            //    crReport.ReportSource = crDoc;
            //    crReport.Refresh();
            //}
        }
    }
}
