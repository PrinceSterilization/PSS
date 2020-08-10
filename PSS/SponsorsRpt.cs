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
    public partial class SponsorsRpt : Form
    {
        public string rptName;
        public string rptLabel;
        public string rptFileName;

        public SponsorsRpt()
        {
            InitializeComponent();
        }

        private void RptSponsors_Load(object sender, EventArgs e)
        {
            string rpt = "";

            ReportDocument crDoc = new ReportDocument();
            
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

            DataTable dt = new DataTable();

            this.Text = rptLabel;

            if (rptName == "SpRefCode")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpRef.rpt";
                crDoc.Load(rpt);
                dt = PSSClass.Sponsors.SpRptRef(1);
            }
            else if (rptName == "SpRefName")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpRef.rpt";
                crDoc.Load(rpt);
                dt = PSSClass.Sponsors.SpRptRef(2);
            }
            else if (rptName == "SpRefSize")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\"  + "SpRefSize.rpt";
                crDoc.Load(rpt);
                dt = PSSClass.Sponsors.SpRptRefSize();
            }
            else if (rptName == "SpRefIndustry")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\"  + "SpRefIndustry.rpt";
                crDoc.Load(rpt);
                dt = PSSClass.Sponsors.SpRptRefInd();
            }
            else if (rptName == "SpRefRegStates")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\"  + "SpRefRegStates.rpt";
                crDoc.Load(rpt);
                dt = PSSClass.Sponsors.SpRptRegStates();
            }
            else if (rptName == "SpRefStates")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpRefStates.rpt";
                crDoc.Load(rpt);
                dt = PSSClass.Sponsors.SpRptRegStates();
            }
            else if (rptName == "SpOnCHW")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SponsorsOnCHW.rpt";
                crDoc.Load(rpt);
                dt = PSSClass.Sponsors.SponsorsOnCHW();
            }
            else if (rptName == "SpPOBound")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SponsorsPOBound.rpt";
                crDoc.Load(rpt);
                dt = PSSClass.Sponsors.SponsorsPOBound();
            }
            else if (rptName == "AuditTrail")
            {
                //DataTable dtRpt = new DataTable();
                //dtRpt.Columns.Add("TableName", typeof(string));
                //dtRpt.Columns.Add("AuditID", typeof(int));
                //dtRpt.Columns.Add("ColumnName", typeof(string));
                //dtRpt.Columns.Add("CurrentValue", typeof(string));
                //dtRpt.Columns.Add("ActionTaken", typeof(int));
                //dtRpt.Columns.Add("ActionTakenBy", typeof(string));
                //dtRpt.Columns.Add("ActionDate", typeof(DateTime));

                //DataTable dtSC = PSSClass.AuditReport.AuditContacts();
                //if (dtSC != null && dtSC.Rows.Count > 0)
                //{
                //    int nSC = 0; int nIDx = 0;
                //    for (int i = 0; i < dtSC.Rows.Count; i++)
                //    {
                //        if (nSC != Convert.ToInt16(dtSC.Rows[i]["ContactID"]))
                //        {
                //            nSC = Convert.ToInt16(dtSC.Rows[i]["ContactID"]);
                //            nIDx = i;
                //        }
                //        else
                //        {
                //            int nX = 0;
                //            foreach (DataColumn dc in dtSC.Columns)
                //            {
                //                if (dtSC.Rows[nIDx][dc.ColumnName].ToString() != dtSC.Rows[i][dc.ColumnName].ToString() && dc.ColumnName.ToString().IndexOf("Action") == -1)
                //                {
                //                    nX = 1;
                //                    DataRow dR = dtRpt.NewRow();
                //                    dR["TableName"] = "Contacts";
                //                    dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                //                    dR["ColumnName"] = dc.ColumnName;
                //                    dR["CurrentValue"] = dtSC.Rows[i][dc.ColumnName];
                //                    dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                //                    dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                //                    dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                //                    dtRpt.Rows.Add(dR);
                //                }
                //            }
                //            if (nX == 1)
                //            {
                //                DataRow dR = dtRpt.NewRow();
                //                dR["TableName"] = "Contacts";
                //                dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                //                dR["ColumnName"] = "ContactID";
                //                dR["CurrentValue"] = dtSC.Rows[i]["ContactID"];
                //                dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                //                dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                //                dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                //                dtRpt.Rows.Add(dR);
                //                nX = 0;
                //            }
                //        }
                //    }
                //}
                //rpt = @"\\gblnj4\GIS\Reports\" + "AuditReport.rpt";
                
                //crDoc.Load(rpt);
                //crDoc.DataDefinition.FormulaFields["cFileName"].Text = "'" + rptFileName + "'";
                //crDoc.SetDataSource(dtRpt);
                //crReport.ReportSource = crDoc;
                //crReport.Refresh();
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
