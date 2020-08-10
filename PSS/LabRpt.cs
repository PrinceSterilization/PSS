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
using KeepAutomation.Barcode.Bean;

namespace PSS
{
    public partial class LabRpt : Form
    {
        public string rptName;
        public string rptTitle;
        public string rptFileName;
        public int nYr;
        public int nMo;
        public int nDy;
        public int nRptNo;
        public int nRevNo;
        public int nSort;
        public int nLogNo;
        public string CmpyCode;
        public int SpID;
        public int nSC;
        public int nNxtPg = 0;
        public string rptFile;
        public byte nExType;
        public byte nERpt;
        public string COCSlash;
        public int nIngredion = 0;
        public string strBatchNo = "";
        public int nFormat = 1;
        public byte bCFR = 0;
        public Int16 nF329 = 0;
        public string strFilter = "";
        public DateTime pubRangeTo;
        public string pubSlashNo;
        
        private ReportDocument crDoc = new ReportDocument();

        public LabRpt()
        {
            InitializeComponent();
        }

        private void CreateReport(object sender, EventArgs e)
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

            if (rptName == "LoginSheet")
            {
                string rpt = "";
                //string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet.rpt";
                if (nIngredion == 0)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet.rpt";
                else if (nIngredion == 1 && strBatchNo == "503130")
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet_503130.rpt";
                }
                else if (nIngredion == 1 && strBatchNo == "503136")
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet_503136.rpt";
                }
                else if (nIngredion == 1 && strBatchNo == "501116")
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet_501116.rpt";
                }
                else if (nIngredion == 1 && strBatchNo == "501901")
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet_501901.rpt";
                }
                else if (nIngredion == 1 && strBatchNo == "501340")
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet_501340.rpt";
                }
                else if (nIngredion == 1 && strBatchNo == "501803")
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet_501803.rpt";
                }
                else if (nIngredion == 1)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet_Manifest.rpt";

                //string rpt = Application.StartupPath + @"\Reports\" + "LoginSheet.rpt";

                crDoc.Load(rpt);

                if (nIngredion == 0)
                {
                    sqlcmd = new SqlCommand("spLoginSheet", sqlcnn);
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.Parameters.AddWithValue("@CmpyCode", CmpyCode);
                    sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                    sqlcmd.Parameters.AddWithValue("@SpID", SpID);
                }
                else
                {
                    if (strBatchNo != "")
                    {
                        sqlcmd = new SqlCommand("spLoginSlashIng", sqlcnn);
                        sqlcmd.CommandType = CommandType.StoredProcedure;

                        sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                        sqlcmd.Parameters.AddWithValue("@SpID", SpID);
                    }
                    else
                    {
                        sqlcmd = new SqlCommand("spLoginSlashes", sqlcnn);
                        sqlcmd.CommandType = CommandType.StoredProcedure;

                        sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                        sqlcmd.Parameters.AddWithValue("@SpID", SpID);
                    }

                    sqlcmd.ExecuteNonQuery();

                    sqlcmd = new SqlCommand("spLoginSheetIng", sqlcnn);
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                    sqlcmd.Parameters.AddWithValue("@SpID", SpID);
                    sqlcmd.ExecuteNonQuery();
                }
            }
            else if (rptName == "Acknowledgement")
            {
                string rpt = "";
                if (nRevNo == 0)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "Acknowledgement.rpt";
                else
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AcknowledgementRev.rpt";
                
                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spLoginSheet", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@CmpyCode", CmpyCode);
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SpID", SpID);
            }

            else if (rptName == "CSCOC")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ChainCustody.rpt";

                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spRptChainCustody", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SlashNo", COCSlash);
            }
            else if (rptName == "GLPCOC")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "GLPChainCustody.rpt";

                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spRptChainCustody", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SlashNo", COCSlash);
            }
            else if (rptName == "SampleLabels")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SampleLabels.rpt";
                //string rpt = Application.StartupPath + @"\Reports\" + "SampleLabels.rpt";

                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spSampleLabels", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.Add(new SqlParameter("@LogNo", SqlDbType.Int));
                sqlcmd.Parameters["@LogNo"].Value = nLogNo;

                sqlcmd.Parameters.Add(new SqlParameter("@SC", SqlDbType.Int));
                sqlcmd.Parameters["@SC"].Value = nSC;
            }
            else if (rptName == "SlashLabels")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SlashLabels.rpt";
                crDoc.Load(rpt);
                sqlcmd = new SqlCommand("spSlashLabels", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
            }
            else if (rptName == "SlashOneLabel")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SlashOneLabel.rpt";
                crDoc.Load(rpt);
                sqlcmd = new SqlCommand("spSlashOneLabel", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SlashNo", pubSlashNo);
            }
            else if (rptName == "LoginsReport")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptFileName + ".rpt";
                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spRptLogins", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("FilterStr", strFilter);
            }
            else if (rptName == "Audit Trail - Samples Login Master File")
            {
                return;
            }
            else if (rptName == "Audit Trail - Samples Login Slashes")
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
                dtSC = PSSClass.AuditReport.AuditLogSamples();
                if (dtSC != null && dtSC.Rows.Count > 0)
                {
                    int nIDx = 0;
                    string strSv = "";
                    for (int i = 0; i < dtSC.Rows.Count; i++)
                    {
                        if (strSv != dtSC.Rows[i]["GBLNo"].ToString() + dtSC.Rows[i]["SlashNo"].ToString())
                        {
                            strSv = dtSC.Rows[i]["GBLNo"].ToString() + dtSC.Rows[i]["SlashNo"].ToString();
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
                                    dR["TableName"] = "Samples Login";
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
                                dR["TableName"] = "Samples Login";
                                dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                                dR["ColumnName"] = "GBLNo";
                                dR["CurrentValue"] = dtSC.Rows[i]["GBLNo"];
                                dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                                dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                                dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                                dtRpt.Rows.Add(dR);
                                nX = 0;
                            }
                        }
                    }
                }
                crDoc.Load(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AuditReport.rpt");
                crDoc.DataDefinition.FormulaFields["cFileName"].Text = "'" + rptFileName + "'";

                crDoc.SetDataSource(dtRpt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
                return;
            }
            else if (rptName == "Audit Trail - Final Report Master File")
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
                dtSC = PSSClass.AuditReport.AuditFinalRptMaster();
                if (dtSC != null && dtSC.Rows.Count > 0)
                {
                    Int32 nSC = 0; int nIDx = 0;
                    for (int i = 0; i < dtSC.Rows.Count; i++)
                    {
                        if (nSC != Convert.ToInt32(dtSC.Rows[i]["ReportNo"]))
                        {
                            nSC = Convert.ToInt32(dtSC.Rows[i]["ReportNo"]);
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
                                    dR["TableName"] = "Final Reports";
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
                                dR["TableName"] = "Final Reports";
                                dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                                dR["ColumnName"] = "ReportNo";
                                dR["CurrentValue"] = dtSC.Rows[i]["ReportNo"];
                                dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                                dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                                dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                                dtRpt.Rows.Add(dR);
                                nX = 0;
                            }
                        }
                    }
                }
                crDoc.Load(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AuditReport.rpt");
                crDoc.DataDefinition.FormulaFields["cFileName"].Text = "'" + rptFileName + "'";

                crDoc.SetDataSource(dtRpt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
                return;
            }
            else if (rptName == "Audit Trail - Final Report Revisions")
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
                dtSC = PSSClass.AuditReport.AuditFinalRptRev();
                if (dtSC != null && dtSC.Rows.Count > 0)
                {
                    int nIDx = 0;
                    string strSv = "";
                    for (int i = 0; i < dtSC.Rows.Count; i++)
                    {
                        if (strSv != dtSC.Rows[i]["ReportNo"].ToString() + dtSC.Rows[i]["RevisionNo"].ToString())
                        {
                            strSv = dtSC.Rows[i]["ReportNo"].ToString() + dtSC.Rows[i]["RevisionNo"].ToString();
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
                                    dR["TableName"] = "Final Reports";
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
                                dR["TableName"] = "Final Reports";
                                dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                                dR["ColumnName"] = "ReportNo";
                                dR["CurrentValue"] = dtSC.Rows[i]["ReportNo"];
                                dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                                dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                                dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                                dtRpt.Rows.Add(dR);

                                dR = dtRpt.NewRow();
                                dR["TableName"] = "Final Reports";
                                dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                                dR["ColumnName"] = "RevisionNo";
                                dR["CurrentValue"] = dtSC.Rows[i]["RevisionNo"];
                                dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                                dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                                dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                                dtRpt.Rows.Add(dR);
                                nX = 0;
                            }
                        }
                    }
                }
                crDoc.Load(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AuditReport.rpt");
                crDoc.DataDefinition.FormulaFields["cFileName"].Text = "'" + rptFileName + "'";

                crDoc.SetDataSource(dtRpt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
                return;
            }
            else if (rptName == "Audit Trail - PO Details")
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
                dtSC = PSSClass.AuditReport.AuditPODetails();
                if (dtSC != null && dtSC.Rows.Count > 0)
                {
                    Int32 nSC = 0; int nIDx = 0;
                    for (int i = 0; i < dtSC.Rows.Count; i++)
                    {
                        if (nSC != Convert.ToInt32(dtSC.Rows[i]["PODetailID"]))
                        {
                            nSC = Convert.ToInt32(dtSC.Rows[i]["PODetailID"]);
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
                                    dR["TableName"] = "Purchase Orders";
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
                                dR["TableName"] = "Purchase Orders";
                                dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                                dR["ColumnName"] = "PONo";
                                dR["CurrentValue"] = dtSC.Rows[i]["PONo"];
                                dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                                dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                                dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                                dtRpt.Rows.Add(dR);
                                nX = 0;
                            }
                        }
                    }
                }
                crDoc.Load(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AuditReport.rpt");
                crDoc.DataDefinition.FormulaFields["cFileName"].Text = "'" + rptFileName + "'";

                crDoc.SetDataSource(dtRpt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
                return;
            }
            else if (rptName == "LoadSchedules")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoadSchedules.rpt";
                //string rpt = Application.StartupPath + @"\Reports\" + "SampleLabels.rpt";

                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spLoadSchedules", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                //sqlcmd.Parameters.Add(new SqlParameter("@LogNo", SqlDbType.Int));
                //sqlcmd.Parameters["@LogNo"].Value = nLogNo;

                //sqlcmd.Parameters.Add(new SqlParameter("@SC", SqlDbType.Int));
                //sqlcmd.Parameters["@SC"].Value = nSC;
            }
            else if (rptName == "Audit Trail - Control Page Numbers")
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
                dtSC = PSSClass.AuditReport.AuditCtrlPgNos();
                if (dtSC != null && dtSC.Rows.Count > 0)
                {
                    Int32 nSC = 0; int nIDx = 0;
                    for (int i = 0; i < dtSC.Rows.Count; i++)
                    {
                        if (nSC != Convert.ToInt32(dtSC.Rows[i]["ControlPageID"]))
                        {
                            nSC = Convert.ToInt32(dtSC.Rows[i]["ControlPageID"]);
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
                                    dR["TableName"] = "Control Page Numbers";
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
                                dR["TableName"] = "Control Page Numbers";
                                dR["AuditID"] = dtSC.Rows[i]["AuditID"];
                                dR["ColumnName"] = "Control Page ID";
                                dR["CurrentValue"] = dtSC.Rows[i]["ControlPageID"];
                                dR["ActionTaken"] = dtSC.Rows[i]["ActionTaken"];
                                dR["ActionTakenBy"] = dtSC.Rows[i]["ActionTakenBy"].ToString();
                                dR["ActionDate"] = dtSC.Rows[i]["ActionDate"].ToString();
                                dtRpt.Rows.Add(dR);
                                nX = 0;
                            }
                        }
                    }
                }
                crDoc.Load(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AuditReport.rpt");
                crDoc.DataDefinition.FormulaFields["cFileName"].Text = "'" + rptFileName + "'";

                crDoc.SetDataSource(dtRpt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
                return;
            }
            else if (rptName == "SpeedReport")
            {
                string rpt = "";// @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRptMain.rpt";
                //string rpt = Application.StartupPath + @"\Reports\" + "SpeedRptMain.rpt";

                if (nSC == 297)
                {
                    if (rptFile.IndexOf("297_NVP") != -1)
                    {
                        rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRpt_297_NVP.rpt"; rptFileName = "SpeedRpt_297_NVP.rpt";
                    }
                    else
                    {
                        rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRpt_297.rpt"; rptFileName = "SpeedRpt_297.rpt";
                    }
                    crDoc.Load(rpt);
                    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SCSPRpt", @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptFile,
                    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 500);
                    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("NonViable", @"\\gblnj4\GIS\Reports\297_1_1345a.rpt",
                    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 1, 1, 11800, 500);
                    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("NonViable", @"\\gblnj4\GIS\Reports\297_1_1345b.rpt",
                    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 1, 1, 11800, 500);
                    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("NonViable", @"\\gblnj4\GIS\Reports\297_1_1345c.rpt",
                    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[3], 1, 1, 11800, 500);
                    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("NonViable", @"\\gblnj4\GIS\Reports\297_1_1345d.rpt",
                    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[4], 1, 1, 11800, 500);
                }
                else if (nSC == 295 && rptFile.IndexOf("295_1") != -1)
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRpt_295.rpt"; rptFileName = "SpeedRpt_295.rpt";
                    crDoc.Load(rpt);
                }
                else if (nSC == 332 && rptFile.IndexOf("332_3_x") != -1) // nFormat == 3)
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRpt_332_3.rpt"; rptFileName = "SpeedRpt_332_3.rpt";
                    crDoc.Load(rpt);
                    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Efficacy", @"\\gblnj4\GIS\Reports\332_3_Efficacy.rpt",
                    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);
                    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Growth Promotion", @"\\gblnj4\GIS\Reports\332_3_GrowthPromo.rpt",
                    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 1, 1, 11800, 100);
                    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Recovery", @"\\gblnj4\GIS\Reports\332_3_Recovery.rpt",
                    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 1, 1, 11800, 100);
                }
                else if (nSC == 167 && (nFormat == 5 || nFormat == 6))
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRpt_167_6.rpt"; 
                    rptFileName = "SpeedRpt_167_6.rpt";
                    crDoc.Load(rpt);
                }
                else if (nSC == 495 && (nFormat == 1 || nFormat == 3))
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRpt_495.rpt"; 
                    rptFileName = "SpeedRpt_495.rpt";
                    crDoc.Load(rpt);
                }
                else if (nSC == 2122 || nSC == 2123)
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRpt_2122_1_173.rpt";
                    crDoc.Load(rpt);
                    //rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRptMainEM.rpt";
                    //crDoc.Load(rpt);
                    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SCSPRpt", @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptFile,
                    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 0, 11800, 500);
                }
                else if (nSC == 4 && nFormat == 3 && SpID == 2974)
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRpt_4_3_2974.rpt";
                    rptFileName = "SpeedRpt_4_3_2974.rpt";
                    crDoc.Load(rpt);
                }
                else
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SpeedRptMain.rpt";
                    crDoc.Load(rpt);
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SCSPRpt", @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptFile,
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 0, 11800, 500);
                }
                crDoc.Refresh();
                foreach (CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj in crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects)
                {
                    // Check to see if it is subreport type
                    if (repObj.Kind == CrystalDecisions.ReportAppServer.ReportDefModel.CrReportObjectKindEnum.crReportObjectKindSubreport)
                    {
                        // clone the report object
                        CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        // tell the report to update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                    }
                }
                if (nSC == 2122 || nSC == 2123 || nSC == 295)
                {
                    sqlcmd = new SqlCommand("spSpeedReportEM", sqlcnn);
                }
                else
                    sqlcmd = new SqlCommand("spSpeedReport", sqlcnn);

                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@CmpyCode",CmpyCode);
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SC", nSC);
                sqlcmd.Parameters.AddWithValue("@SpID", SpID);
            }
            else if (rptName == "Ingredion")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptFile;
                crDoc.Load(rpt);
                //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SCSPRpt", @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptFile,
                //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 0, 11800, 500);

                //crDoc.Refresh();
                //foreach (CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj in crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects)
                //{
                //    // Check to see if it is subreport type
                //    if (repObj.Kind == CrystalDecisions.ReportAppServer.ReportDefModel.CrReportObjectKindEnum.crReportObjectKindSubreport)
                //    {
                //        // clone the report object
                //        CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //        //modify the line style  
                //        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //        // tell the report to update the report object.
                //        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                //    }
                //}
                sqlcmd = new SqlCommand("spIngredionReport", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
            }
            else if (rptName == "FinalReport")
            {
                string rpt = "";
                if (bCFR == 1)
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptFile;
                    crDoc.Load(rpt);
                }
                else if (nSC == 297)
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_297.rpt";
                    crDoc.Load(rpt);

                    if (rptFile.IndexOf("297_NVP") != -1)
                    {
                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\GBLNJ4\GIS\Reports\297_NVP1a.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);
                        CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                        //Clone
                        CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                        // update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                    }
                    else
                    {
                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\GBLNJ4\GIS\Reports\297_1.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);

                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpta", @"\\GBLNJ4\GIS\Reports\297_1_1345a.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 1, 1, 11800, 100);

                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptb", @"\\GBLNJ4\GIS\Reports\297_1_1345b.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 1, 1, 11800, 100);

                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptc", @"\\GBLNJ4\GIS\Reports\297_1_1345c.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[3], 1, 1, 11800, 100);

                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptd", @"\\GBLNJ4\GIS\Reports\297_1_1345d.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[4], 1, 1, 11800, 100);

                        //added 5/2/2016 for SP ID 156
                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpte", @"\\GBLNJ4\GIS\Reports\297_1_1345e.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[5], 1, 1, 11800, 100);

                        //crDoc.Refresh();

                        CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                        //Clone
                        CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                        // update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                        repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                        newObj = repObj.Clone(true);
                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                        // update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                        repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2].ReportObjects[0];
                        newObj = repObj.Clone(true);

                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        // update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                        repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[3].ReportObjects[0];
                        newObj = repObj.Clone(true);

                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        // update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                        repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[4].ReportObjects[0];
                        newObj = repObj.Clone(true);
                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        // update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                        repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[5].ReportObjects[0];
                        newObj = repObj.Clone(true);
                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        // update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                    }
                }
                else if (nSC == 332 && rptFile.IndexOf("332_3_x") != -1)// nFormat == 3)
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_332_3.rpt";
                    crDoc.Load(rpt);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Efficacy", @"\\GBLNJ4\GIS\Reports\332_3a_Efficacy.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);//332_3_Efficacy.rpt original rpt - changed 2/9/16

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("GrowthPromo", @"\\GBLNJ4\GIS\Reports\332_3_GrowthPromo.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                    if (PSSClass.FinalReports.CategoryData332(nLogNo, nSC, SpID) != "N")
                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Recovery", @"\\GBLNJ4\GIS\Reports\332_3_Recovery.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 0, 1, 11800, 100);

                    //crDoc.Refresh();

                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                    //Clone
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    if (PSSClass.FinalReports.CategoryData332(nLogNo, nSC, SpID) != "N")
                    {
                        repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2].ReportObjects[0];
                        newObj = repObj.Clone(true);
                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        // update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                    }
                }
                else if (nSC == 167 && (nFormat == 5 || nFormat == 6))
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_332_3.rpt";
                    crDoc.Load(rpt);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Efficacy", @"\\GBLNJ4\GIS\Reports\332_3a_Efficacy.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("GrowthPromo", @"\\GBLNJ4\GIS\Reports\332_3_GrowthPromo.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                    //crDoc.Refresh();

                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                    //Clone
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
                //else if (nSC == 495 && (nFormat == 1 || nFormat == 3))
                //{
                //    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_332_3.rpt";
                //    crDoc.Load(rpt);

                //    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Efficacy", @"\\GBLNJ4\GIS\Reports\495_Efficacy.rpt",
                //        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                //    //crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("GrowthPromo", @"\\GBLNJ4\GIS\Reports\495_Preserve.rpt",
                //    //    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                //    //crDoc.Refresh();

                //    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //    //Clone
                //    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //    //modify the line style  
                //    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                //    // update the report object.
                //    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                //    //repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                //    //newObj = repObj.Clone(true);
                //    ////modify the line style  
                //    //newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //    //newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //    //newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                //    //newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                //    //// update the report object.
                //    //crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                //}
                else if (nSC == 295 && rptFile.IndexOf("295_1") != -1)
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_295.rpt";
                    crDoc.Load(rpt);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Reading1", @"\\GBLNJ4\GIS\Reports\295_11.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Reading2", @"\\GBLNJ4\GIS\Reports\295_12.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Reading3", @"\\GBLNJ4\GIS\Reports\295_13.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 0, 1, 11800, 100);

                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                    //Clone
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
                else if (nSC == 329 && SpID == 1787 && nFormat == 1 && nF329 != 329)
                {
                    if (nF329 == 43)
                        rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_329_43.rpt";
                    else if (nF329 == 276)
                        rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_329_276.rpt";

                    crDoc.Load(rpt);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("329_rpt", @"\\GBLNJ4\GIS\Reports\329_1_1787.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                    if (nF329 == 43)
                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("329_subrpt", @"\\GBLNJ4\GIS\Reports\43_1_1787.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);
                    else if (nF329 == 276)
                        crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("329_subrpt", @"\\GBLNJ4\GIS\Reports\276_1_1787.rpt",
                                crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                    crDoc.Refresh();

                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                    //Clone
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
                else
                {
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRptMain.rpt";
                    crDoc.Load(rpt);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptFile,
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100); // 2, 2, 11800, 1000);

                    //crDoc.Refresh();

                    foreach (CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj in crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects)
                    {
                        // Check if it is subreport type
                        if (repObj.Kind == CrystalDecisions.ReportAppServer.ReportDefModel.CrReportObjectKindEnum.crReportObjectKindSubreport)
                        {
                            // clone the report object
                            CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                            //modify the line style  
                            newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                            newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                            newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                            newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                            // update the report object.
                            crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                        }
                    }
                }
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spFinRptMain", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@CmpyCode", CmpyCode);
                sqlcmd.Parameters.AddWithValue("@RptNo", nRptNo);
                sqlcmd.Parameters.AddWithValue("@RevNo", nRevNo);
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
            }
            else if (rptName == "FinalRptIngredion")
            {
                string rpt = "";

                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRptIngredion.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptSamples", @"\\GBLNJ4\GIS\Reports\LogSamples.rpt",
                      crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptFile,
                crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 1, 1, 11800, 100); // 2, 2, 11800, 1000);

                //crDoc.Refresh();
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //Clone
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);

                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                sqlcmd = new SqlCommand("spFinRptMain", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@CmpyCode", CmpyCode);
                sqlcmd.Parameters.AddWithValue("@RptNo", nRptNo);
                sqlcmd.Parameters.AddWithValue("@RevNo", nRevNo);
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
            }
            else if (rptName == "ReportCover")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ReportCoverPage.rpt";
                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spRptSponsorAddress", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(nRptNo));
            }
            else if (rptName == "ReportLabel")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ReportLabel.rpt";
                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spRptSponsorAddress", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(nRptNo));
            }
            sqldr = sqlcmd.ExecuteReader();
            DataTable dTable = new DataTable();
            try
            {
                dTable.Load(sqldr);
            }
            catch
            {
            }
            ////TEST LOGIN SHEET DATA TABLE WITH 2D BARCODE
            //if (rptName == "LoginSheet")
            //{
            //    //dTable = null;
            //    DataTable dtBarCode = new DataTable();
            //    sqlcnn = PSSClass.DBConnection.PSSConnection();
            //    if (sqlcnn == null)
            //    {
            //        sqlcnn.Dispose();
            //    }
            //    sqlcmd = new SqlCommand("spLoginBarCode", sqlcnn);
            //    sqlcmd.CommandType = CommandType.StoredProcedure;
            //    sqlcmd.Parameters.AddWithValue("GBLNo", nLogNo);
            //    sqlcmd.Parameters.AddWithValue("SC",232);

            //    try
            //    {
            //        sqldr = sqlcmd.ExecuteReader();
            //        dtBarCode.Load(sqldr);
            //    }
            //    catch
            //    {
            //        sqlcnn.Dispose();
            //    }
            //    BarCode qrcode = new BarCode();
            //    //Barcode settings
            //    qrcode.Symbology = KeepAutomation.Barcode.Symbology.QRCode;
            //    qrcode.X = 6;
            //    qrcode.Y = 6;
            //    qrcode.LeftMargin = 24;
            //    qrcode.RightMargin = 24;
            //    qrcode.TopMargin = 24;
            //    qrcode.BottomMargin = 24;
            //    qrcode.ImageFormat = System.Drawing.Imaging.ImageFormat.Png;

            //    foreach (System.Data.DataRow dr in dtBarCode.Rows)
            //    {
            //        qrcode.CodeToEncode = dr["GBLNo"].ToString() + "-" + dr["SC"].ToString();
            //        byte[] imageData = qrcode.generateBarcodeToByteArray();
            //        dr["BarCode"] = imageData;
            //    }
            //    crDoc.Subreports["LoginBarCode"].SetDataSource(dtBarCode);
            //    //TEST LOGIN SHEET DATA TABLE WITH 2D BARCODE
            //}

            crDoc.SetDataSource(dTable);
            if (rptName == "SpeedReport")
            {
                if (nSC == 2122 || nSC == 2123)
                {

                }
                else
                if (rptFileName == null || rptFileName.IndexOf("SpeedRpt_") == -1)
                {
                    crDoc.SetParameterValue("@LogNo", nLogNo, "SCSPRpt");
                    crDoc.SetParameterValue("@SC", nSC, "SCSPRpt");
                    crDoc.SetParameterValue("@SpID", SpID, "SCSPRpt");
                }
            }
            else if (rptName == "FinalReport")
            {
                crDoc.DataDefinition.FormulaFields["cNewPage"].Text = "'" + nNxtPg.ToString() + "'";
                crDoc.DataDefinition.FormulaFields["cEReport"].Text = "'" + nERpt.ToString() + "'";
                if (bCFR == 1)
                {
                    DataTable dtX = PSSClass.FinalReports.GetRptGBL(nRptNo, Convert.ToInt16(nRevNo));
                    if (dtX != null && dtX.Rows.Count > 1)
                    {
                        string strGBL = "", strSC = "", strSCDesc = "", strSvSC = "";
                        Int32 nSvGBL= 0;
                        for (int i = 0; i < dtX.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dtX.Rows[i]["GBLNo"]) != nSvGBL)
                            {
                                nSvGBL = Convert.ToInt32(dtX.Rows[i]["GBLNo"]);
                                strGBL += dtX.Rows[i]["GBLNo"].ToString() + ", ";
                            }
                            if (strSvSC != dtX.Rows[i]["ServiceCode"].ToString())
                            {
                                strSC += dtX.Rows[i]["ServiceCode"].ToString() + ", ";
                                strSCDesc += dtX.Rows[i]["ServiceDesc"] + ", ";
                                strSvSC = dtX.Rows[i]["ServiceCode"].ToString();
                            }
                        }
                        try
                        {
                            strGBL = strGBL.Trim();
                            strSC = strSC.Trim();
                            strSCDesc = strSCDesc.Trim();
                            crDoc.DataDefinition.FormulaFields["cGBL"].Text = "'" + strGBL.Substring(0, strGBL.Length - 1) + "'";
                            crDoc.DataDefinition.FormulaFields["cSC"].Text = "'" + strSC.Substring(0, strSC.Length - 1) + "'";
                            crDoc.DataDefinition.FormulaFields["cSCDesc"].Text = "'" + strSCDesc.Substring(0, strSCDesc.Length - 1) + "'";
                        }
                        catch { }
                        dtX.Dispose();
                    }
                    crDoc.SetParameterValue("@RptNo", nRptNo);
                    crDoc.SetParameterValue("@RevNo", nRevNo);
                }
                else if (nSC == 297)
                {
                    if (rptFile.IndexOf("297_NVP") != -1)
                    {
                        crDoc.SetParameterValue("@LogNo", nLogNo, "SubRpt");
                        crDoc.SetParameterValue("@SC", nSC, "SubRpt");
                        crDoc.SetParameterValue("@SpID", SpID, "SubRpt");
                    }
                    else
                    {
                        crDoc.SetParameterValue("@LogNo", nLogNo, "SubRpt");
                        crDoc.SetParameterValue("@SC", nSC, "SubRpt");
                        crDoc.SetParameterValue("@SpID", SpID, "SubRpt");

                        crDoc.SetParameterValue("@LogNo", nLogNo, "SubRpta");
                        crDoc.SetParameterValue("@SC", nSC, "SubRpta");
                        crDoc.SetParameterValue("@SpID", SpID, "SubRpta");


                        crDoc.SetParameterValue("@LogNo", nLogNo, "SubRptb");
                        crDoc.SetParameterValue("@SC", nSC, "SubRptb");
                        crDoc.SetParameterValue("@SpID", SpID, "SubRptb");

                        crDoc.SetParameterValue("@LogNo", nLogNo, "SubRptc");
                        crDoc.SetParameterValue("@SC", nSC, "SubRptc");
                        crDoc.SetParameterValue("@SpID", SpID, "SubRptc");

                        crDoc.SetParameterValue("@LogNo", nLogNo, "SubRptd");
                        crDoc.SetParameterValue("@SC", nSC, "SubRptd");
                        crDoc.SetParameterValue("@SpID", SpID, "SubRptd");

                        crDoc.SetParameterValue("@LogNo", nLogNo, "SubRpte");
                        crDoc.SetParameterValue("@SC", nSC, "SubRpte");
                        crDoc.SetParameterValue("@SpID", SpID, "SubRpte");
                    }
                }
                else if (nSC == 332 && rptFile.IndexOf("332_3_x") != -1)//nFormat == 3)
                {
                    crDoc.SetParameterValue("@LogNo", nLogNo, "Efficacy");
                    crDoc.SetParameterValue("@SC", nSC, "Efficacy");
                    crDoc.SetParameterValue("@SpID", SpID, "Efficacy");

                    crDoc.SetParameterValue("@LogNo", nLogNo, "GrowthPromo");
                    crDoc.SetParameterValue("@SC", nSC, "GrowthPromo");
                    crDoc.SetParameterValue("@SpID", SpID, "GrowthPromo");

                    if (PSSClass.FinalReports.CategoryData332(nLogNo, nSC, SpID) != "N")
                    {
                        crDoc.SetParameterValue("@LogNo", nLogNo, "Recovery");
                        crDoc.SetParameterValue("@SC", nSC, "Recovery");
                        crDoc.SetParameterValue("@SpID", SpID, "Recovery");
                    }
                }
                else if (nSC == 167 && (nFormat == 5 || nFormat == 6))
                {
                    crDoc.SetParameterValue("@LogNo", nLogNo, "Efficacy");
                    crDoc.SetParameterValue("@SC", nSC, "Efficacy");
                    crDoc.SetParameterValue("@SpID", SpID, "Efficacy");

                    crDoc.SetParameterValue("@LogNo", nLogNo, "GrowthPromo");
                    crDoc.SetParameterValue("@SC", nSC, "GrowthPromo");
                    crDoc.SetParameterValue("@SpID", SpID, "GrowthPromo");
                }
                else if (nSC == 295 && rptFile.IndexOf("295_1") != -1)
                {
                    crDoc.SetParameterValue("@LogNo", nLogNo, "Reading1");
                    crDoc.SetParameterValue("@SC", nSC, "Reading1");
                    crDoc.SetParameterValue("@SpID", SpID, "Reading1");

                    crDoc.SetParameterValue("@LogNo", nLogNo, "Reading2");
                    crDoc.SetParameterValue("@SC", nSC, "Reading2");
                    crDoc.SetParameterValue("@SpID", SpID, "Reading2");

                    crDoc.SetParameterValue("@LogNo", nLogNo, "Reading3");
                    crDoc.SetParameterValue("@SC", nSC, "Reading3");
                    crDoc.SetParameterValue("@SpID", SpID, "Reading3");
                }
                else if (nSC == 329 && SpID == 1787 && nFormat == 1 && nF329 != 329)
                {
                    crDoc.SetParameterValue("@LogNo", nLogNo, "329_rpt");
                    crDoc.SetParameterValue("@SC", nSC, "329_rpt");
                    crDoc.SetParameterValue("@SpID", SpID, "329_rpt");

                    crDoc.SetParameterValue("@LogNo", nLogNo, "329_subrpt");
                    if (nF329 == 43)
                        crDoc.SetParameterValue("@SC", 43, "329_subrpt");
                    else if (nF329 == 276)
                        crDoc.SetParameterValue("@SC", 276, "329_subrpt");
                    crDoc.SetParameterValue("@SpID", SpID, "329_subrpt");
                }
                else
                {
                    crDoc.SetParameterValue("@RptNo", nRptNo);
                    crDoc.SetParameterValue("@RevNo", nRevNo);
                    crDoc.SetParameterValue("@LogNo", nLogNo, "SubRpt");
                    crDoc.SetParameterValue("@SC", nSC, "SubRpt");
                    crDoc.SetParameterValue("@SpID", SpID, "SubRpt");
                }
                if (nExType == 0)
                {
                    crReport.ShowExportButton = false; crReport.ShowPrintButton = false; crReport.ShowCopyButton = false; crReport.ShowGroupTreeButton = false;
                }
            }
            else if (rptName == "LoginSheet")
            {
                if (nIngredion == 1 && strBatchNo == "")
                {
                    crDoc.SetParameterValue("@LogNo", nLogNo, "LoginSubRptSlashes");
                    crDoc.SetParameterValue("@SpID", SpID, "LoginSubRptSlashes");
                }
            }
            else if (rptName == "LoginsReport")
            {
                string strDte = "";
                if (nFormat == 3)
                    strDte = nMo.ToString("00") + "/" + nDy.ToString("00") + "/" + nYr.ToString();
                else
                    strDte = nMo.ToString("00") + "/01/" + nYr.ToString();

                if (nFormat == 1)
                {
                    if (nYr != DateTime.Now.Year)
                        crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FOR THE YEAR " + nYr.ToString() + "'";
                    else
                        crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'AS OF " + DateTime.Now.ToString("MMMM d, yyyy").ToUpper() + "'";
                    crDoc.DataDefinition.FormulaFields["cFormat"].Text = "'" + nExType.ToString()  + "'";
                }
                else if (nFormat == 2)
                    crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FOR THE MONTH OF: " + Convert.ToDateTime(strDte).ToString("MMMM yyyy").ToUpper() + "'";
                else if (nFormat == 3)
                    crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'DAILY LOGIN : " + Convert.ToDateTime(strDte).ToString("MMMM dd, yyyy").ToUpper() + "'";
                else if (nFormat == 4)
                    crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'DATE RANGE: " + Convert.ToDateTime(strDte).ToString("MMMM dd, yyyy").ToUpper() + " - " +
                                          pubRangeTo.ToString("MMMM dd, yyyy").ToUpper() + "'";
                crDoc.DataDefinition.FormulaFields["cFormat"].Text = "'" + nExType.ToString() + "'";
            }
            else if (rptName == "FinalRptIngredion")
            {
                crDoc.DataDefinition.FormulaFields["cNewPage"].Text = "'0'";

                crDoc.SetParameterValue("@LogNo", nLogNo, "SubRptSamples");
                crDoc.SetParameterValue("@LogNo", nLogNo, "SubRpt");

            }
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }            
            crReport.ReportSource = crDoc;
            crReport.Refresh();
            sqlcnn.Close(); sqlcnn.Dispose();

            if (rptName == "Acknowledgement")
            {
                crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                if (nRevNo == 0)
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Acknowledgements\" + DateTime.Now.Year.ToString() + "\\" + "A-" + nLogNo.ToString("000000") + ".pdf";
                else
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Acknowledgements\" + DateTime.Now.Year.ToString() + "\\" + "A-" + nLogNo.ToString("000000") + "-R" + nRevNo.ToString().Trim() + ".pdf";
                crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                crDoc.Export();
                if (nRevNo == 0)
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Acknowledgements\" + DateTime.Now.Year.ToString() + "\\" + "A-" + nLogNo.ToString("000000") + "_" + DateTime.Now.Month.ToString("00") +
                                              DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                else
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Acknowledgements\" + DateTime.Now.Year.ToString() + "\\" + "A-" + nLogNo.ToString("000000") + "-R" + nRevNo.ToString().Trim() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                                              DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                crDoc.Export();
            }
            else if (rptName == "LoginSheet")
            {
                crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\LoginSheets\" + nYr.ToString() + "\\" + "L-P" + nLogNo.ToString("000000") + ".pdf";
                crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                crDoc.Export();
                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\LoginSheets\" + nYr.ToString() + "\\" + "L-P" + nLogNo.ToString("000000") + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                                         DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                crDoc.Export();
            }
            else if (rptName == "SpeedReport")
            {
                crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\SpeedReports\" + "SR-" + nLogNo.ToString() + "-" + nSC.ToString() + ".pdf";
                crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                crDoc.Export();
                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\SpeedReports\" + "SR-" + nLogNo.ToString() + "-" + nSC.ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                                         DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                crDoc.Export();
            }
            else if (rptName == "FinalRptIngredion")
            {
                crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "R-" + nRptNo.ToString() + ".R" + nRevNo.ToString() + ".pdf";
                crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                crDoc.Export();
                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "R-" + nRptNo.ToString() + ".R" + nRevNo.ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                                         DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                crDoc.Export();

                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "E_" + nRptNo.ToString() + ".R" + nRevNo.ToString() + ".pdf";
                crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                crDoc.Export();
                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "E_" + nRptNo.ToString() + ".R" + nRevNo.ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                                          DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                crDoc.Export();
            }
            else if (rptName == "FinalReport")
            {
                if (nExType == 3)
                {
                    crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\" +  DateTime.Now.Year.ToString() + "\\" + "E_" + nRptNo.ToString() + ".R" + nRevNo.ToString() + ".pdf";
                    crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                    crDoc.Export();
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\" + DateTime.Now.Year.ToString() + "\\" + "E_" + nRptNo.ToString() + ".R" + nRevNo.ToString() + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                                              DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                    crDoc.Export();
                }
            }
            //Clean up
            dTable.Dispose();
        }

        private void LabRpt_Load(object sender, EventArgs e)
        {
            this.Text = rptTitle;
            CreateReport(this, null);
        }

        private void LabRpt_FormClosing(object sender, FormClosingEventArgs e)
        {
            crDoc.Close(); crDoc.Dispose();
            this.Dispose();
        }

        private void crReport_Error(object source, CrystalDecisions.Windows.Forms.ExceptionEventArgs e)
        {
            e.Handled = true;
        }
    } 
}
