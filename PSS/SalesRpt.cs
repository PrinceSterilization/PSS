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
    public partial class SalesRpt : Form
    {
        public string rptName = "", rptTitle = "", rptTag = "", rptLabel = "", rptFileName = "";
        public int nYr = 0, nSort = 0, nScope = 0;
        public DataTable dtSales;
        private ReportDocument crDoc;

        public SalesRpt()
        {
            InitializeComponent();
        }

        private void SalesRpt_Load(object sender, EventArgs e)
        {
            this.Text = rptTitle;
            CreateReport(this, null);
        }

        private void CreateReport(object sender, EventArgs e)
        {
            
            crDoc = new ReportDocument();
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
            this.Text = rptTitle;

            string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptName;

            DataTable dt = new DataTable();

            if (rptTag == "QNotMailed.rpt")
            {
                dt = PSSClass.Quotations.QuotesNotMailed(nYr, nSort);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cTitle"].Text = "'" + rptTitle.ToUpper() + "'";
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptTag == "QuotesRpt.rpt")
            {
                dt = PSSClass.Quotations.QuotesReport(nYr, nSort);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptTag == "QuotesRptNoSp.rpt")
            {
                dt = PSSClass.Quotations.QuotesReport(nYr, nSort);
                crDoc.Load(rpt);

                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptTag == "QuotesRptSp.rpt")
            {
                dt = PSSClass.Quotations.QuotesReport(nYr, nSort);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptTag == "QuotesRptSC.rpt")
            {
                dt = PSSClass.Quotations.QuotesReportSC(nYr);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptTag == "QuotesRptDept.rpt")
            {
                dt = PSSClass.Quotations.QuotesReportSC(nYr);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptTag == "QPPay.rpt")
            {
                dt = PSSClass.Quotations.QuotesWPPay(nYr, nSort);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cTitle"].Text = "'" + rptTitle.ToUpper() + "'";
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR : " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptTag == "QPPayAcc.rpt")
            {
                dt = PSSClass.Quotations.QuotesWPPay(nYr, nSort);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cTitle"].Text = "'" + rptTitle.ToUpper() + "'";
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR : " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptTag == "QPPayAccNoInv.rpt")
            {
                dt = PSSClass.Quotations.QuotesWPPay(nYr, nSort);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cTitle"].Text = "'" + rptTitle.ToUpper() + "'";
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR : " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptTag == "QPPayAccInv.rpt")
            {
                dt = PSSClass.Quotations.QuotesWPPay(nYr, nSort);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cTitle"].Text = "'" + rptTitle.ToUpper() + "'";
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR : " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }

            else if (rptName == "QuotesExpiring.rpt")
            {
                dt = PSSClass.Quotations.QuotesExpiring(nYr, nSort);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptName == "QuotesReissue.rpt")
            {
                dt = PSSClass.Quotations.QuotesExpiring(nYr, nSort);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "QUOTES SENT IN " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
            }
            else if (rptName == "CustomerYrReview.rpt")
            {
                dt = PSSClass.Quotations.YearlyReview(nYr);
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE YEAR " + nYr.ToString() + "'";
                int nRange = nYr - 4;
                string strRange = "";
                for (int i = 0; i < 5; i++)
                {
                    strRange += nRange.ToString() + ",";
                    nRange += 1;
                }
                crDoc.DataDefinition.FormulaFields["cRange"].Text = "'" + strRange.Substring(0,strRange.Length -1) + "'";
                crDoc.DataDefinition.FormulaFields["cYr"].Text = "" + nYr + "";
                crDoc.SetDataSource(dt);
                crDoc.Subreports["CustomerYRSpCount"].DataDefinition.FormulaFields["cRange"].Text = "'" + strRange.Substring(0, strRange.Length - 1) + "'";
                crDoc.Subreports["CustomerYrStats"].DataSourceConnections.Clear();
                crDoc.Subreports["CustomerYrStats"].SetDataSource(dtSales);
            }
            else if (rptName== "QtrlyForecast.rpt")
            {
                dt = PSSClass.Quotations.QtrlyForecast(nYr, nSort);
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + nYr.ToString() + " QUARTERLY FORECAST" + "'";
                crDoc.SetDataSource(dt);
                crDoc.SetParameterValue("@Yr", nYr, "NonForecast");
                crDoc.SetParameterValue("@Yr", nYr, "QuarterlySales");
            }
            else if (rptName == "QuotesRejected.rpt")
            {
                dt = PSSClass.Quotations.QuotesRejected(nYr);
                if (nSort == 0)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\QuotesRejected.rpt";
                else
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\QuotesRejectedSp.rpt";
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FOR THE YEAR " + nYr.ToString() + "'";
                crDoc.SetDataSource(dt);
                crDoc.SetParameterValue("@Yr", nYr);
            }
            else if (rptName == "Audit Trail - Quotations Master File" || rptName == "Audit Trail - Quotation Revisions" || rptName == "Audit Trail - Quotation Test Items")
            {
                DataTable dtRpt = new DataTable();
                dtRpt.Columns.Add("TableName", typeof(string));
                dtRpt.Columns.Add("AuditID", typeof(int));
                dtRpt.Columns.Add("ColumnName", typeof(string));
                dtRpt.Columns.Add("CurrentValue", typeof(string));
                dtRpt.Columns.Add("ActionTaken", typeof(int));
                dtRpt.Columns.Add("ActionTakenBy", typeof(string));
                dtRpt.Columns.Add("ActionDate", typeof(DateTime));

                DataTable dtSC = new DataTable();
                if (rptName == "Audit Trail - Quotations Master File")
                    dtSC = PSSClass.AuditReport.AuditQuotations();
                else if (rptName == "Audit Trail - Quotation Revisions")
                    dtSC = PSSClass.AuditReport.AuditQuotationRev();
                else if (rptName == "Audit Trail - Quotation Test Items")
                    dtSC = PSSClass.AuditReport.AuditQuotationRevTests();

                if (dtSC != null && dtSC.Rows.Count > 0)
                {
                    string strQ = ""; int nIDx = 0;
                    for (int i = 0; i < dtSC.Rows.Count; i++)
                    {
                        if (strQ != dtSC.Rows[i]["QuotationNo"].ToString())
                        {
                            strQ = dtSC.Rows[i]["QuotationNo"].ToString();
                            nIDx = i;
                        }
                        else
                        {
                            int nX = 0;
                            foreach (DataColumn dc in dtSC.Columns)
                            {
                                if (dtSC.Rows[nIDx][dc.ColumnName].ToString() != dtSC.Rows[i][dc.ColumnName].ToString() && dc.ColumnName.ToString().IndexOf("Action") == -1)
                                {
                                    nX = 1;
                                    DataRow dR = dtRpt.NewRow();
                                    dR["TableName"] = "Quotations";
                                    dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                                    dR["ColumnName"] = dc.ColumnName;
                                    dR["CurrentValue"] = dtSC.Rows[i][dc.ColumnName];
                                    dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                                    dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                                    dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                                    dtRpt.Rows.Add(dR);
                                }
                            }
                            if (nX == 1)
                            {
                                DataRow dR = dtRpt.NewRow();
                                dR["TableName"] = "Quotations";
                                dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                                dR["ColumnName"] = "Quotation No.";
                                dR["CurrentValue"] = dtSC.Rows[i]["QuotationNo"];
                                dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                                dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                                dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                                dtRpt.Rows.Add(dR);
                                nX = 0;
                            }
                        }
                    }
                }
                //rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\QuotesRejected.rpt";
                //rpt = @"\\gblnj4\GIS\Reports\" + "AuditReport.rpt";
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AuditReport.rpt";
                //rpt = Application.StartupPath + @"\Reports\" + "SpRefRegState.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cFileName"].Text = "'" + rptFileName + "'";
                crDoc.Load(rpt);
                crDoc.SetDataSource(dtRpt);
            }
            crReport.ReportSource = crDoc;
            crReport.Refresh();
        }

        private void SalesRpt_FormClosing(object sender, FormClosingEventArgs e)
        {
            crDoc.Close(); crDoc.Dispose();
            crReport.ReportSource = null; crReport.Dispose(); crReport = null;
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers();
        }
    }
}
