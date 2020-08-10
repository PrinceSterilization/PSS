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
    public partial class MgmtRpts : Form
    {
        public string rptName;
        public string rptTitle;
        public int nYr;
        public int nMo;
        public int nQtr;
        public int nGBL;
        public int SpID = 0;
        public int SC = 0;
        public int nMgmtRev = 0;
        public int nDeptID = 0;
        public int nFSFormat = 0;
        public string strDept = "";
        public DateTime dteStart;
        public DateTime dteEnd;
        public int nSDID = 0;
        public DataTable dtRpt;

        private ReportDocument crDoc;

        private int nDay;

        public MgmtRpts()
        {
            InitializeComponent();
        }

        private void CreateReport()
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
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            crDoc = new ReportDocument();

            if (rptName == "InvYrTotal")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "InvYrTotal.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cYr"].Text = "'FY " + nYr.ToString() + "'";

                sqlcmd = new SqlCommand("spInvYrTotRpt", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
            }
            else if (rptName == "LogYrTotal")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LogYrTotal.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cGBL"].Text = "" + nGBL + "";
                crDoc.DataDefinition.FormulaFields["cYr"].Text = "'FY " + nYr.ToString() + "'";

                sqlcmd = new SqlCommand("spLogYrTotRpt", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
                sqlcmd.Parameters.AddWithValue("@GBL", nGBL);
            }
            else if (rptName == "RptYrTotal")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "RptYrTotal.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cGBL"].Text = "" + nGBL + "";
                crDoc.DataDefinition.FormulaFields["cYr"].Text = "'FY " + nYr.ToString() + "'";

                sqlcmd = new SqlCommand("spRptYrTot", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
                sqlcmd.Parameters.AddWithValue("@GBL", nGBL);
            }
            else if (rptName == "NewSponsors")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "NewSponsors.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FOR THE YEAR " + nYr.ToString() + "'";

                sqlcmd = new SqlCommand("spNewCustomers", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
            }
            else if (rptName == "InactiveSponsors")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "InactiveSponsors.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FOR THE YEAR " + nYr.ToString() + "'";

                sqlcmd = new SqlCommand("spInactiveCustomers", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
            }
            else if (rptName == "MgmtGraph")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "MgmtSummary.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cYr"].Text = "'FY " + nYr.ToString() + "'";

                sqlcmd = new SqlCommand("spMgmtTotRpt", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
            }
            else if (rptName == "MgmtQuoteRpt")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "QuotesByDept.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FY " + nYr.ToString() + "'";

                sqlcmd = new SqlCommand("spQuoteAnaDept", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
                sqlcmd.Parameters.AddWithValue("@Sort", 1);
            }
            else if (rptName == "ProformaRev")
            {
                string rpt = "";
                if (nMgmtRev >= 1 && nMgmtRev <= 4)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ProformaRev.rpt";
                else
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ProformaRevSC.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FY " + nYr.ToString() + "'";
                if (nMgmtRev == 1)
                {
                    crDoc.DataDefinition.FormulaFields["cCategory"].Text = "'ALL SPONSORS'";
                    sqlcmd = new SqlCommand("spProformaRevAll", sqlcnn);
                }
                //else if (nMgmtRev == 2)
                //{
                //    crDoc.DataDefinition.FormulaFields["cCategory"].Text = "'NON-INGREDION SPONSORS'";
                //    sqlcmd = new SqlCommand("spProformaRev", sqlcnn);
                //}
                //else if (nMgmtRev == 3)
                //{
                //    crDoc.DataDefinition.FormulaFields["cCategory"].Text = "'INGREDION'";
                //    sqlcmd = new SqlCommand("spProformaRevIng", sqlcnn);
                //}
                else if (nMgmtRev == 4)
                {
                    crDoc.DataDefinition.FormulaFields["cCategory"].Text = "'STABILITY'";
                    sqlcmd = new SqlCommand("spProformaRevSta", sqlcnn);
                }
                //else if (nMgmtRev == 5)
                //{
                //    crDoc.DataDefinition.FormulaFields["cCategory"].Text = "'STERILIZATION'";
                //    sqlcmd = new SqlCommand("spProformaRevSter", sqlcnn);
                //}

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@Year", nYr);
                sqlcmd.Parameters.AddWithValue("@SpID", SpID);
                sqlcmd.Parameters.AddWithValue("@SC", SC);
                sqlcmd.CommandTimeout = 60;
            }
            else if (rptName == "IngredionRevenue")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "IngredionRevenue.rpt";
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cYear"].Text = "'" + nYr.ToString() + "'";
                sqlcmd = new SqlCommand("spIngredionRevRpt", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
            }
            else if (rptName == "IngredionProfDtls")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "IngredionProfDtls.rpt";
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cYear"].Text = "'" + nYr.ToString() + "'";
                sqlcmd = new SqlCommand("spIngredionProfDtls", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
            }
            else if (rptName == "MgmtRevenue")
            {
                string rpt = "";
                if (nMgmtRev == 1 || nMgmtRev == 2)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "MgmtRevenue.rpt";
                else if (nMgmtRev == 3 || nMgmtRev == 4)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "MgmtRevSC.rpt";
                else if (nMgmtRev >= 5 && nMgmtRev <= 8)
                {
                    if (nMgmtRev == 5 || nMgmtRev == 6)
                        rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "MgmtRevSpSumm.rpt";
                    else
                        rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "MgmtRevSpDtls.rpt";
                }
                else if (nMgmtRev >= 9 && nMgmtRev <= 12)
                {
                    if (nMgmtRev == 9 || nMgmtRev == 10)
                        rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "MgmtRevSCSumm.rpt";
                    else
                        rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "MgmtRevSCDtls.rpt";
                }

                crDoc.Load(rpt);

                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FY " + nYr.ToString() + "'";
                sqlcmd = new SqlCommand("spRevPivotRpt", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@Year", nYr);
                if (nMgmtRev == 1 || nMgmtRev == 3 || nMgmtRev == 5 || nMgmtRev == 7 || nMgmtRev == 9 || nMgmtRev == 11)
                {
                    sqlcmd.Parameters.AddWithValue("@SpID", 0);
                    sqlcmd.Parameters.AddWithValue("@SC", 0);
                }
                else if (nMgmtRev == 2 || nMgmtRev == 4 || nMgmtRev == 6 || nMgmtRev == 8 || nMgmtRev == 10 || nMgmtRev == 12)
                {
                    sqlcmd.Parameters.AddWithValue("@SpID", SpID);
                    sqlcmd.Parameters.AddWithValue("@SC", SC);
                }
            }
            else if (rptName == "PSSFinancial")
            {
                sqlcnn.Close(); sqlcmd.Dispose();
                sqlcnn = PSSClass.DBConnection.MDFConnection("172.16.4.12", "PTSFinancials", false, "sa", "Pass2018", "");
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                sqlcmd = new SqlCommand();
                crConnectionInfo.DatabaseName = "PTSFinancials";
                crConnectionInfo.IntegratedSecurity = false;
                crConnectionInfo.UserID = "sa";
                crConnectionInfo.Password = "Pass2018";
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;

                this.Text = "FINANCIAL REPORT";
                string strDay = "";

                decimal nIT = PSSClass.Financials.IncomeTax(nYr);

                string[] strMonths = new string[] {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                int[] nLYMonths = new int[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                int[] nNLYMonths = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

                int nR = nYr % 4;
                if (nR == 0)
                    strDay = nLYMonths[nMo-1].ToString();
                else
                    strDay = nNLYMonths[nMo-1].ToString();

                string rpt = "";
                if (nFSFormat == 0)
                    //rpt = Application.StartupPath + @"\Reports\" + "PSSFinancial.rpt";
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PSSFinancial.rpt";
                else if (nFSFormat == 1)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PSSFSSummary.rpt";
                else if (nFSFormat == 2)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PSSFinancialNoPer.rpt";
                else if (nFSFormat == 3)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PSSFinancialGL.rpt";

                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FOR THE PERIOD ENDED " + strMonths[nMo-1].ToUpper() + " " + strDay + ", " + nYr.ToString() + "'";
                crDoc.DataDefinition.FormulaFields["strMo"].Text = "'" + nMo.ToString() + "'";
                crDoc.DataDefinition.FormulaFields["cTaxes"].Text = nIT.ToString();
                sqlcmd = new SqlCommand("spLinkFinancial", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
            }
            else if (rptName == "PSSFinancial_12Month")
            {
                sqlcnn.Close(); sqlcmd.Dispose();
                sqlcnn = PSSClass.DBConnection.MDFConnection("172.16.4.12", "PTSFinancials", false, "sa", "Pass2018", "");
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                sqlcmd = new SqlCommand();
                crConnectionInfo.DatabaseName = "PTSFinancials";
                crConnectionInfo.IntegratedSecurity = false;
                crConnectionInfo.UserID = "sa";
                crConnectionInfo.Password = "Pass2018";
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;

                this.Text = "FINANCIAL REPORT::12 MONTH OVERVIEW";
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PSSFinancial_12Month.rpt";
                //string strDay = "";

                //decimal nIT = PSSClass.Financials.IncomeTax(nYr);

                //string[] strMonths = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                //int[] nLYMonths = new int[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                //int[] nNLYMonths = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

                //int nR = nYr % 4;
                //if (nR == 0)
                //    strDay = nLYMonths[nMo - 1].ToString();
                //else
                //    strDay = nNLYMonths[nMo - 1].ToString();

                //string rpt = "";
                //if (nFSFormat == 0)                    
                //    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PSSFinancial_12Month.rpt";
                //else if (nFSFormat == 1)
                //    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PSSFSSummary.rpt";
                //else if (nFSFormat == 2)
                //    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PSSFinancialNoPer.rpt";
                //else if (nFSFormat == 3)
                //    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PSSFinancialGL.rpt";

                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                //foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                //{
                //    crtableLogoninfo = CrTable.LogOnInfo;
                //    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                //    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                //}
                //crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FOR THE PERIOD ENDED " + strMonths[nMo - 1].ToUpper() + " " + strDay + ", " + nYr.ToString() + "'";
                //crDoc.DataDefinition.FormulaFields["strMo"].Text = "'" + nMo.ToString() + "'";
                //crDoc.DataDefinition.FormulaFields["cTaxes"].Text = nIT.ToString();
                sqlcmd = new SqlCommand("spLinkFinancial", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
            }
            else if (rptName.IndexOf("Variance") != -1)
            {
                this.Text = "VARIANCE REPORT - BUDGET VS. ACTUAL";

                string strDay = "";
                string rpt = "";
                decimal nIT = PSSClass.Financials.IncomeTax(nYr);

                string[] strMonths = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                int[] nLYMonths = new int[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                int[] nNLYMonths = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

                int nR = nYr % 4;
                if (nR == 0)
                    strDay = nLYMonths[nMo - 1].ToString();
                else
                    strDay = nNLYMonths[nMo - 1].ToString();

                if (rptName == "VarianceYTD")
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "Variance.rpt";
                }
                else if (rptName == "VarianceMonthly")
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "VarianceMonthly.rpt";
                }
                else
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "VarianceQtrly.rpt";
                }
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cMo"].Text = "'" + nMo.ToString() + "'";
                crDoc.DataDefinition.FormulaFields["cYr"].Text = "'" + nYr.ToString() + "'";
                crDoc.DataDefinition.FormulaFields["cDay"].Text = "'" + strDay + "'";
                crDoc.DataDefinition.FormulaFields["cQtr"].Text = "'" + nQtr.ToString() + "'";
            }

            else if (rptName == "TestsCompleted")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "TestsCompleted.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["dteStart"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["dteEnd"].Text = "'" + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptTestsCompleted", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
                sqlcmd.Parameters.AddWithValue("@DeptList", strDept); //nDeptID
            }
            else if (rptName == "OutstandingTests")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "OutstandingTests.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["dteStart"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["dteEnd"].Text = "'" + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptOutstandingGBLTesting", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
                sqlcmd.Parameters.AddWithValue("@DeptList", strDept); //nDeptID
            }
            else if (rptName == "TestsForCompletion")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "TestsForCompletion.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["dteStart"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["dteEnd"].Text = "'" + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptTestsForCompletion", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
            }
            else if (rptName == "StabilityReport")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "StabilityReport-New.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'RANGE PERIOD: " + dteStart.ToShortDateString() + " - " + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spStabilityReport", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@SDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EDate", dteEnd);
            }
            else if (rptName == "StudyDirReport")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "StudyDirRpt.rpt";

                crDoc.Load(rpt);
                if (dteStart == dteEnd)
                    crDoc.DataDefinition.FormulaFields["strPeriod"].Text = "'FOR THE PERIOD: " + dteStart.ToShortDateString() + "'";
                else
                    crDoc.DataDefinition.FormulaFields["strPeriod"].Text = "'FOR THE PERIOD: " + dteStart.ToShortDateString() + " - " + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spStudyDirReport", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
                sqlcmd.Parameters.AddWithValue("@SDID", nSDID);
            }
            else if (rptName == "UnmailedReports")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "UnmailedReports.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["dteStart"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["dteEnd"].Text = "'" + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptUnmailedReports", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
            }
            else if (rptName == "EqptServiceSched")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EquipmentServiceSched.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["dteStart"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["dteEnd"].Text = "'" + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptEqptSrvcSched", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
            }
            else if (rptName == "SCDepartments")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SCDepartments.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + dteStart.ToShortDateString() + " - " + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptDeptSCSP", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
                sqlcmd.Parameters.AddWithValue("@DeptID", nDeptID);
                sqlcmd.Parameters.AddWithValue("@SC", SC);
            }
            else if (rptName == "DocExpiring")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "DocExpiring.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'As of " + DateTime.Now.ToString("MMMM dd, yyyy") + "'";
                sqlcmd = new SqlCommand("spDocExpiringAlert", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
            }
            else if (rptName == "SpeedResponse")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedResponse.rpt";
                string strSpName = PSSClass.Sponsors.SpName(SpID);
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cYr"].Text = "'" + nYr.ToString("###0") + "'";
                crDoc.DataDefinition.FormulaFields["cMo"].Text = "'" + nMo.ToString("###0") + "'";
                crDoc.DataDefinition.FormulaFields["cSpID"].Text = "'" + SpID.ToString() + "'";
                crDoc.DataDefinition.FormulaFields["cSpName"].Text = "'" + strSpName + "'";
            }
            else if (rptName == "RptGBLErrors")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "RptGBLErrors.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cYr"].Text = "'" + nYr.ToString()  + "'";
                crDoc.DataDefinition.FormulaFields["cGBL"].Text = "'" + nGBL.ToString() + "'";

                sqlcmd = new SqlCommand("spRptGBLErrors", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@Yr", nYr);
                sqlcmd.Parameters.AddWithValue("@IncGBL", nGBL);
                sqlcmd.CommandTimeout = 60;
            }

            DataTable dTable = new DataTable();
            if (rptName == "PSSFinancial")
            {
                dTable = PSSClass.Financials.AcctSummary(nYr);
            }
            else if (rptName == "PSSFinancial_12Month")
            {
                dTable = PSSClass.Financials.AcctSummary(nYr);
            }
            else if (rptName.IndexOf("Variance") != -1)
            {
                dTable = dtRpt;
            }
            else if (rptName == "SpeedResponse")
            {
                dTable.Columns.Add("Category", typeof(int));
                dTable.Columns.Add("NoDays", typeof(int));
                dTable.Columns.Add("DayCount", typeof(int));
                //LogTests Start Date
                DataTable dtX = PSSClass.ManagementReports.SpeedResponse(1, Convert.ToInt16(nYr), Convert.ToInt16(nMo), "LT.StartDate", Convert.ToInt16(SpID));
                for (int i = 0; i < dtX.Rows.Count; i++)
                {
                    DataRow dR = dTable.NewRow();
                    dR["Category"] = dtX.Rows[i]["Category"];
                    dR["NoDays"] = dtX.Rows[i]["NoDays"];
                    dR["DayCount"] = dtX.Rows[i]["DayCount"];
                    dTable.Rows.Add(dR);
                }
                //FinalRptLog DateOn LogTests End Date
                dtX = PSSClass.ManagementReports.SpeedResponse(2, Convert.ToInt16(nYr), Convert.ToInt16(nMo), "FL.DateOn", Convert.ToInt16(SpID));
                for (int i = 0; i < dtX.Rows.Count; i++)
                {
                    DataRow dR = dTable.NewRow();
                    dR["Category"] = dtX.Rows[i]["Category"];
                    dR["NoDays"] = dtX.Rows[i]["NoDays"];
                    dR["DayCount"] = dtX.Rows[i]["DayCount"];
                    dTable.Rows.Add(dR);
                }
                //LogTests End Date
                dtX = PSSClass.ManagementReports.SpeedResponse(3, Convert.ToInt16(nYr), Convert.ToInt16(nMo), "LT.EndDate", Convert.ToInt16(SpID));
                for (int i = 0; i < dtX.Rows.Count; i++)
                {
                    DataRow dR = dTable.NewRow();
                    dR["Category"] = dtX.Rows[i]["Category"];
                    dR["NoDays"] = dtX.Rows[i]["NoDays"];
                    dR["DayCount"] = dtX.Rows[i]["DayCount"];
                    dTable.Rows.Add(dR);
                }
                //FinalRptLog DateOff
                dtX = PSSClass.ManagementReports.SpeedResponse(4, Convert.ToInt16(nYr), Convert.ToInt16(nMo), "FL.DateOff", Convert.ToInt16(SpID));
                for (int i = 0; i < dtX.Rows.Count; i++)
                {
                    DataRow dR = dTable.NewRow();
                    dR["Category"] = dtX.Rows[i]["Category"];
                    dR["NoDays"] = dtX.Rows[i]["NoDays"];
                    dR["DayCount"] = dtX.Rows[i]["DayCount"];
                    dTable.Rows.Add(dR);
                }
                //LogTest Start Date vs FinalRptLog DateOn
                dtX = PSSClass.ManagementReports.SpeedResponseSD(4, Convert.ToInt16(nYr), Convert.ToInt16(nMo), Convert.ToInt16(SpID));
                for (int i = 0; i < dtX.Rows.Count; i++)
                {
                    DataRow dR = dTable.NewRow();
                    dR["Category"] = dtX.Rows[i]["Category"];
                    dR["NoDays"] = dtX.Rows[i]["NoDays"];
                    dR["DayCount"] = dtX.Rows[i]["DayCount"];
                    dTable.Rows.Add(dR);
                }
            }
            else
            {
                sqldr = sqlcmd.ExecuteReader();
                try
                {
                    dTable.Load(sqldr);
                }
                catch { }
            }
            crDoc.SetDataSource(dTable);
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
            crReport.ReportSource = crDoc;
            crReport.ShowGroupTreeButton = false;
            crReport.Refresh();
            sqlcnn.Close(); sqlcnn.Dispose();
            this.WindowState = FormWindowState.Maximized;
        }

        private void MgmtRpts_Load(object sender, EventArgs e)
        {
            CreateReport();
        }

        private void MgmtRpts_FormClosing(object sender, FormClosingEventArgs e)
        {
            crDoc.Close(); crDoc.Dispose();
            crReport.ReportSource = null; crReport.Dispose(); crReport = null;
            //GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers();
        }
    }
}
