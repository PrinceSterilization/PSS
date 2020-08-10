//SamplesLogin.cs
// AUTHOR       : ALEXANDER M. DELA CRUZ
// TITLE        : Software Developer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Samples Login File Maintenance

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;
using System.IO;
using System.Xml;
using System.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Text.RegularExpressions;
using System.Threading;

namespace PSS
{
    public partial class SamplesLogin : PSS.TemplateForm
    {
        public byte nFR;
        public Int64 nLogNo;
        public string pubCmpyCode;

        //Login Search Parameters
        public int nSearch = 99;
        public string strCriteria = "", strData = "";
        public int nSSC = 0, nSSpID = 0;


        private byte nMode = 0;
        private byte nType = 0;
        private string strSlashNo = "";

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol = {};
        private int nIndex;
        private int nQu = 0; //Quote not required entry
        private int nQ = 0; //Quotation selected indicator
        private string[,] arrSC;
        private const string ClipboardFormat = "XML Spreadsheet";
        private string strFileAccess = "RO";
        private int nIngredion = 0;
        //private byte bWSw = 0;//Background Worker Switch

        private byte lMoveNext;
        private int colIndex = 0, rowIndex = 0;

        private object[] fkeys = new object[2];

        //for DatagridView search
        private int nCtr = 0;
        private int nSw = 0;
        //======================

        private byte bOSSF = 0; //8/24/2017 - AMDC

        List<string> strList = new List<string>();
        List<string> strListSC = new List<string>();
        List<string> strListQ = new List<string>();
        List<string> strListCmpy = new List<string>();
        List<int> nListDelSC = new List<int>();

        DataTable dtLogMaster = new DataTable(); //Master List
        DataTable dtLogFM = new DataTable(); //File Maintenance
        DataTable dtSponsors = new DataTable();
        DataTable dtContacts = new DataTable();
        DataTable dtLogTests = new DataTable();
        DataTable dtLogTestsDel = new DataTable();//Deleted Tests
        DataTable dtSamples = new DataTable();
        DataTable dtSamplesAddl = new DataTable();
        DataTable dtSC = new DataTable();
        DataTable dtSCMaster = new DataTable();
        DataTable dtSampleSC = new DataTable();
        DataTable dtPONo = new DataTable();
        DataTable dtBilling = new DataTable();
        DataTable dtSCExtData = new DataTable();//SC Extended Data Table 
        DataTable dtSlashExtData = new DataTable();//Slash Extended Data Table
        DataTable dtArticleDesc = new DataTable();

        //Ingredion Tables
        DataTable dtFillCodes = new DataTable();//derived from Manifest

        ReportDocument crDoc;

        //BackgroundWorker m_oWorker;

        public SamplesLogin()
        {
            InitializeComponent();

            //m_oWorker = new BackgroundWorker();
            //m_oWorker.WorkerSupportsCancellation = true;

            // Create a background worker thread that ReportsProgress &
            // SupportsCancellation
            // Hook up the appropriate events.

            //m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            //m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_oWorker_RunWorkerCompleted);

            bnMoveFirst.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveLast.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveNext.Click += new EventHandler(MoveRecordClickHandler);
            bnMovePrevious.Click += new EventHandler(MoveRecordClickHandler);

            tsbAdd.Click += new EventHandler(AddClickHandler);
            tsbEdit.Click += new EventHandler(EditClickHandler);
            tsbDelete.Click += new EventHandler(DeleteClickHandler);
            tsbSave.Click += new EventHandler(SaveClickHandler);
            tsbCancel.Click += new EventHandler(CancelClickHandler);
            tsbExit.Click += new EventHandler(CloseClickHandler);
            tsbSearch.Click += new EventHandler(SearchOKClickHandler);
            tsbFilter.Click += new EventHandler(SearchFilterClickHandler);
            tsbRefresh.Click += new EventHandler(RefreshClickHandler);
            tstbSearch.KeyPress += new KeyPressEventHandler(SearchKeyPressHandler);
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            //txtContact.GotFocus += new EventHandler(txtContactEnterHandler);
            //txtSponsorID.GotFocus += new EventHandler(txtSponsorIDEnterHandler);
            //txtContactID.GotFocus += new EventHandler(txtContactIDEnterHandler);
            txtPONo.KeyPress += new KeyPressEventHandler(txtPONoKeyPressHandler);
            dgvPONo.LostFocus += new EventHandler(dgvPONoOnLeave);
            lblLoadStatus.Visible = true; timer1.Enabled = true;
            lblLoadStatus.Text = "Retrieving records from database...please standby!";// +nCtr.ToString() + " second(s) elapsed.";
        }

        //private void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    for (int b = 1; b <= 10; b++)
        //    {
        //        if (m_oWorker.CancellationPending == true)
        //        {
        //            e.Cancel = true;
        //            bWSw = 1;
        //            break;
        //        }
        //        else
        //        {
        //            LoadRecords();
        //            break;
        //        }
        //    }
        //}

        //private void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    m_oWorker.Dispose();
        //    m_oWorker = null;

        //    if (bWSw == 0)
        //    {
        //        bsFile.DataSource = dtLogMaster;
        //        bnFile.BindingSource = bsFile;
        //        dgvFile.DataSource = bsFile;

        //        DataGridSetting();
        //        if (tsddbSearch.DropDownItems.Count == 0)
        //        {
        //            int i = 0;
        //            int n = 0;

        //            arrCol = new string[dtLogMaster.Columns.Count];

        //            ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

        //            foreach (DataColumn colFile in dtLogMaster.Columns)
        //            {
        //                items[i] = new ToolStripMenuItem();
        //                items[i].Name = colFile.ColumnName;

        //                //Using LINQ to insert space between capital letters
        //                var val = colFile.ColumnName;
        //                val = string.Concat(val.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

        //                items[i].Text = val;
        //                items[i].Click += new EventHandler(SearchItemClickHandler);
        //                arrCol[i] = colFile.DataType.ToString();
        //                cklColumns.Items.Add(val);
        //                //}
        //                i += 1;
        //            }
        //            for (int j = 0; j < cklColumns.Items.Count; j++)
        //            {
        //                cklColumns.SetItemChecked(j, true);
        //            }
        //            tsddbSearch.DropDownItems.AddRange(items);
        //            tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
        //            tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
        //        }
        //        FileAccess();
        //        BuildPrintItems();
        //        timer1.Enabled = false; lblLoadStatus.Visible = false;
        //        //Added 7/24/2017
        //        if (nFR == 1)
        //        {
        //            txtLogNo.Text = nLogNo.ToString();
        //            PSSClass.General.FindRecord("PSSNo", txtLogNo.Text, bsFile, dgvFile);
        //            dgvFile.Select();
        //            dgvFile.CurrentCell = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells[0];
        //            SendKeys.Send("{Enter}");
        //        }
        //    }
        //}

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[3];

            items[0] = new ToolStripMenuItem();
            items[0].Text = "Login Sheet";
            items[0].Click += new EventHandler(PrtLoginClickHandler);

            items[1] = new ToolStripMenuItem();
            items[1].Text = "Login Reports";
            items[1].Click += new EventHandler(PrtLoginRptClickHandler);

            items[2] = new ToolStripMenuItem();
            items[2].Text = "Audit Trail";
            items[2].Click += new EventHandler(PrtAuditClickHandler);

            //items[2] = new ToolStripMenuItem();
            //items[2].Text = "Audit Trail";
            //items[2].Click += new EventHandler(PrtAuditClickHandlerGPLS);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Samples.SampLogMaster();
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }

            int i = 0;
            int n = 0;

            arrCol = new string[dt.Columns.Count];

            ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

            foreach (DataColumn colFile in dt.Columns)
            {
                items[i] = new ToolStripMenuItem();
                items[i].Name = colFile.ColumnName;

                //Using LINQ to insert space between capital letters
                var val = colFile.ColumnName;
                val = string.Concat(val.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

                items[i].Text = val;
                items[i].Click += new EventHandler(SearchItemClickHandler);
                arrCol[i] = colFile.DataType.ToString();
                cklColumns.Items.Add(val);
                //}
                i += 1;
            }
            tsddbSearch.DropDownItems.AddRange(items);
            tslSearchData.Text = tsddbSearch.DropDownItems[1].Text;
            tstbSearchField.Text = tsddbSearch.DropDownItems[1].Name;
        }

        private void PrtLoginClickHandler(object sender, EventArgs e)
        {
            btnLSPreview_Click(null, null);
        }

        private void PrtLoginRptClickHandler(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                pnlReports.Visible = true; pnlReports.BringToFront();
                int nY = DateTime.Now.Year;
                for (int i = 1; i < 8; i++)
                {
                    cboVitalFY.Items.Add(nY.ToString());
                    nY--;
                }
                cboVitalFY.SelectedIndex = 0;
                btnPrint.Select();
            }
        }

        private void PrtLabelsClickHandler(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                LabRpt rpt = new LabRpt();
                rpt.rptName = "SampleLabels";
                rpt.WindowState = FormWindowState.Maximized;
                rpt.nLogNo = Convert.ToInt32(txtLogNo.Text);
                rpt.nSC = Convert.ToInt16(((ComboBox)dtrLogTests.CurrentItem.Controls["cboSC"]).Text);
                try
                {
                    rpt.Show();
                }
                catch { }
            }
        }

        private void PrtAuditClickHandler(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (rptTitle.IndexOf("Audit") != -1)
                {
                    if (rptTitle == "Audit Trail")
                    {

                        string strPattern = "([a-z?])[_ ]?([A-Z])";

                        DataTable dtAudit = new DataTable();
                        dtAudit.Columns.Add("KeyDataName", typeof(string));
                        dtAudit.Columns.Add("KeyDataValue", typeof(string));
                        dtAudit.Columns.Add("DataName", typeof(string));
                        dtAudit.Columns.Add("DataType", typeof(string));
                        dtAudit.Columns.Add("ActionTaken", typeof(string));
                        dtAudit.Columns.Add("ServiceCode", typeof(string));
                        dtAudit.Columns.Add("OldValue", typeof(string));
                        dtAudit.Columns.Add("NewValue", typeof(string));
                        dtAudit.Columns.Add("DateDone", typeof(string));
                        dtAudit.Columns.Add("DoneBy", typeof(string));

                        //DataTable dtAuLogGIS = PSSClass.AuditReport.AuLogMasterGIS(Convert.ToDateTime("05/01/2016"), Convert.ToDateTime("05/05/2016"));
                        //DataTable dtAuLog = PSSClass.AuditReport.AuLogMaster(Convert.ToDateTime("05/01/2016"), Convert.ToDateTime("05/05/2016"));

                        if (dgvFile.Rows.Count > 0)
                            txtLogNo.Text = dgvFile.CurrentRow.Cells["PSSNo"].Value.ToString();

                        //Log Master Table
                        DataTable dtAuLogGIS = PSSClass.AuditReport.AuLogMasterPTS(Convert.ToInt32(txtLogNo.Text));
                        //if (dtAuLogGIS != null)
                        //    MessageBox.Show("Passed LogMaster 1. " + dtAuLogGIS.Rows.Count.ToString());
                        //else
                        //    MessageBox.Show("No audit records found.");
                        DataTable dtAuLog = PSSClass.AuditReport.AuLogMaster(Convert.ToInt32(txtLogNo.Text)); //Convert.ToDateTime("05/01/2016"), Convert.ToDateTime("05/05/2016")
                        //if (dtAuLog != null)
                        //    MessageBox.Show("Passed LogMaster 2." + dtAuLog.Rows.Count.ToString());
                        //else
                        //    MessageBox.Show("No audit records found.");

                        DataTable dtAuSamples;
                        DataTable dtAuLogTests;

                        if (dtAuLog != null && dtAuLog.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtAuLog.Columns.Count; j++)
                            {
                                if (j < dtAuLog.Columns.Count - 3)
                                {
                                    if (dtAuLogGIS.Rows[0][j].ToString() != dtAuLog.Rows[0][j].ToString())
                                    {
                                        string strType = dtAuLog.Columns[j].DataType.ToString();
                                        DataRow dR = dtAudit.NewRow();
                                        dR["KeyDataName"] = "PSS No.";
                                        dR["KeyDataValue"] = dtAuLog.Rows[0]["PSSNo"].ToString();
                                        dR["DataName"] = Regex.Replace(dtAuLog.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                        dR["DataType"] = strType;
                                        dR["OldValue"] = dtAuLog.Rows[0][j];
                                        dR["NewValue"] = dtAuLogGIS.Rows[0][j];
                                        dR["ServiceCode"] = "";
                                        dR["ActionTaken"] = dtAuLog.Rows[0]["FileMaintCode"].ToString();
                                        dR["Datedone"] = dtAuLog.Rows[0]["FileMaintDate"].ToString();
                                        dR["DoneBy"] = dtAuLog.Rows[0]["FileMaintByID"].ToString();
                                        dtAudit.Rows.Add(dR);
                                    }
                                }
                            }
                            MessageBox.Show("Passed Audit Tests" + dtAudit.Rows.Count.ToString());
                            int k = 1;
                            for (int i = 0; i < dtAuLog.Rows.Count; i++)
                            {
                                if (k >= dtAuLog.Rows.Count)
                                    break;
                                for (int j = 0; j < dtAuLog.Columns.Count; j++)
                                {
                                    if (j < dtAuLog.Columns.Count - 3)
                                    {
                                        if (dtAuLog.Rows[i][j].ToString() != dtAuLog.Rows[k][j].ToString())
                                        {
                                            string strType = dtAuLog.Columns[j].DataType.ToString();

                                            DataRow dR = dtAudit.NewRow();
                                            dR["KeyDataName"] = "PSS No.";
                                            dR["KeyDataValue"] = dtAuLog.Rows[i]["PSSNo"].ToString();
                                            dR["DataType"] = strType;
                                            dR["DataName"] = Regex.Replace(dtAuLog.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                            dR["OldValue"] = dtAuLog.Rows[k][j];
                                            dR["NewValue"] = dtAuLog.Rows[i][j];
                                            dR["ServiceCode"] = "";
                                            dR["ActionTaken"] = dtAuLog.Rows[k]["FileMaintCode"].ToString();
                                            dR["Datedone"] = dtAuLog.Rows[k]["FileMaintDate"].ToString();
                                            dR["DoneBy"] = dtAuLog.Rows[k]["FileMaintByID"].ToString();
                                            dtAudit.Rows.Add(dR);
                                        }
                                    }
                                }
                                k++;
                            }
                        }
                        //Log Samples
                        DataTable dtAuSamplesGIS = PSSClass.AuditReport.AuLogSamplesPTS(Convert.ToInt32(txtLogNo.Text));
                        if (dtAuSamplesGIS != null && dtAuSamplesGIS.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAuSamplesGIS.Rows.Count; i++)
                            {
                                dtAuSamples = PSSClass.AuditReport.AuLogSamples(Convert.ToInt32(txtLogNo.Text), Convert.ToInt32(dtAuSamplesGIS.Rows[i]["SlashID"]));
                                if (dtAuSamples != null && dtAuSamples.Rows.Count > 0)
                                {
                                    //Latest Update
                                    for (int j = 0; j < dtAuSamples.Columns.Count; j++)
                                    {
                                        if (j < dtAuSamples.Columns.Count - 3)
                                        {
                                            if (dtAuSamplesGIS.Rows[i][j].ToString() != dtAuSamples.Rows[0][j].ToString())
                                            {
                                                string strType = dtAuSamples.Columns[j].DataType.ToString();
                                                DataRow dR = dtAudit.NewRow();
                                                dR["KeyDataName"] = "PSS No.";
                                                dR["KeyDataValue"] = dtAuSamples.Rows[0]["PSSNo"].ToString();
                                                dR["DataType"] = strType;
                                                dR["DataName"] = Regex.Replace(dtAuSamples.Columns[j].ColumnName.ToString(), strPattern, "$1 $2"); //dtAuLog.Columns[j].ColumnName.ToString(); 
                                                dR["OldValue"] = dtAuSamples.Rows[0][j];
                                                dR["NewValue"] = dtAuSamplesGIS.Rows[i][j];
                                                dR["ServiceCode"] = "";
                                                dR["ActionTaken"] = dtAuSamples.Rows[0]["FileMaintCode"].ToString();
                                                dR["Datedone"] = dtAuSamples.Rows[0]["FileMaintDate"].ToString();
                                                dR["DoneBy"] = dtAuSamples.Rows[0]["FileMaintByID"].ToString();
                                                dtAudit.Rows.Add(dR);
                                            }
                                        }
                                    }
                                    //Previous Updates
                                    int m = 1;
                                    for (int s = 0; s < dtAuSamples.Rows.Count; s++)
                                    {
                                        if (m >= dtAuSamples.Rows.Count)
                                            break;
                                        for (int j = 0; j < dtAuSamples.Columns.Count; j++)
                                        {
                                            if (j < dtAuSamples.Columns.Count - 3)
                                            {
                                                if (dtAuSamples.Rows[s][j].ToString() != dtAuSamples.Rows[m][j].ToString())
                                                {
                                                    string strType = dtAuSamples.Columns[j].DataType.ToString();
                                                    DataRow dR = dtAudit.NewRow();
                                                    dR["KeyDataName"] = "PSS No.";
                                                    dR["KeyDataValue"] = dtAuSamples.Rows[s]["PSSNo"].ToString();
                                                    dR["DataType"] = strType;
                                                    dR["DataName"] = Regex.Replace(dtAuSamples.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                                    dR["OldValue"] = dtAuSamples.Rows[m][j];
                                                    dR["NewValue"] = dtAuSamples.Rows[s][j];
                                                    dR["ServiceCode"] = "";
                                                    dR["ActionTaken"] = dtAuSamples.Rows[m]["FileMaintCode"].ToString();
                                                    dR["Datedone"] = dtAuSamples.Rows[m]["FileMaintDate"].ToString();
                                                    dR["DoneBy"] = dtAuSamples.Rows[m]["FileMaintByID"].ToString();
                                                    dtAudit.Rows.Add(dR);
                                                }
                                            }
                                        }
                                        m++;
                                    }
                                }
                            }
                        }
                        //Log Tests
                        //Log Samples
                        DataTable dtAuLogTestsGIS = PSSClass.AuditReport.AuLogTestsPTS(Convert.ToInt32(txtLogNo.Text));
                        if (dtAuLogTestsGIS != null && dtAuLogTestsGIS.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAuLogTestsGIS.Rows.Count; i++)
                            {
                                dtAuLogTests = PSSClass.AuditReport.AuLogTests(Convert.ToInt32(txtLogNo.Text), Convert.ToInt32(dtAuLogTestsGIS.Rows[i]["ServiceCode"]));
                                if (dtAuLogTests != null && dtAuLogTests.Rows.Count > 0)
                                {
                                    //Latest Update
                                    for (int j = 0; j < dtAuLogTests.Columns.Count; j++)
                                    {
                                        if (j < dtAuLogTests.Columns.Count - 3)
                                        {
                                            if (dtAuLogTestsGIS.Rows[i][j].ToString() != dtAuLogTests.Rows[0][j].ToString())
                                            {
                                                string strType = dtAuLogTests.Columns[j].DataType.ToString();
                                                DataRow dR = dtAudit.NewRow();
                                                dR["KeyDataName"] = "PSS No.";
                                                dR["KeyDataValue"] = dtAuLogTests.Rows[0]["PSSNo"].ToString();
                                                dR["DataType"] = strType;
                                                dR["DataName"] = Regex.Replace(dtAuLogTests.Columns[j].ColumnName.ToString(), strPattern, "$1 $2"); //dtAuLog.Columns[j].ColumnName.ToString(); 
                                                dR["OldValue"] = dtAuLogTests.Rows[0][j];
                                                dR["NewValue"] = dtAuLogTestsGIS.Rows[i][j];
                                                dR["ServiceCode"] = dtAuLogTestsGIS.Rows[i]["ServiceCode"].ToString();
                                                dR["ActionTaken"] = dtAuLogTests.Rows[0]["FileMaintCode"].ToString();
                                                dR["Datedone"] = dtAuLogTests.Rows[0]["FileMaintDate"].ToString();
                                                dR["DoneBy"] = dtAuLogTests.Rows[0]["FileMaintByID"].ToString();
                                                dtAudit.Rows.Add(dR);
                                            }
                                        }
                                    }
                                    //Previous Updates
                                    int m = 1;
                                    for (int s = 0; s < dtAuLogTests.Rows.Count; s++)
                                    {
                                        if (m >= dtAuLogTests.Rows.Count)
                                            break;
                                        for (int j = 0; j < dtAuLogTests.Columns.Count; j++)
                                        {
                                            if (j < dtAuLogTests.Columns.Count - 3)
                                            {
                                                if (dtAuLogTests.Rows[s][j].ToString() != dtAuLogTests.Rows[m][j].ToString())
                                                {
                                                    string strType = dtAuLogTests.Columns[j].DataType.ToString();
                                                    DataRow dR = dtAudit.NewRow();
                                                    dR["KeyDataName"] = "PSS No.";
                                                    dR["KeyDataValue"] = dtAuLogTests.Rows[s]["PSSNo"].ToString();
                                                    dR["DataType"] = strType;
                                                    dR["DataName"] = Regex.Replace(dtAuLogTests.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                                    dR["OldValue"] = dtAuLogTests.Rows[m][j];
                                                    dR["NewValue"] = dtAuLogTests.Rows[s][j];
                                                    dR["ServiceCode"] = dtAuLogTestsGIS.Rows[i]["ServiceCode"].ToString();
                                                    dR["ActionTaken"] = dtAuLogTests.Rows[m]["FileMaintCode"].ToString();
                                                    dR["Datedone"] = dtAuLogTests.Rows[m]["FileMaintDate"].ToString();
                                                    dR["DoneBy"] = dtAuLogTests.Rows[m]["FileMaintByID"].ToString();
                                                    dtAudit.Rows.Add(dR);
                                                }
                                            }
                                        }
                                        m++;
                                    }
                                }
                            }
                        }
                        
                        MessageBox.Show("Passed LogGIS " + dtAuLogGIS.Rows.Count.ToString());
                        MessageBox.Show("Passed Audit Logout " + dtAuLog.Rows.Count.ToString());
                        MessageBox.Show("Passed Audit Tests" + dtAudit.Rows.Count.ToString());

                        if (dtAudit == null || dtAudit.Rows.Count == 0)
                        {
                            MessageBox.Show("No audit records found.", Application.ProductName);
                            return;
                        }
                        AuditRpt rpt = new AuditRpt();
                        rpt.rptName = "Audit LogMaster";
                        rpt.dt = dtAudit;
                        rpt.WindowState = FormWindowState.Maximized;
                        rpt.Show();
                    }
                    else if (rptTitle == "Audit Trail - Slashes")
                    {
                        //rptSC.rptFileName = "SAMPLES LOGIN SLASHES";
                        //rptSC.rptName = "Audit Trail - Samples Login Slashes";
                        //LabRpt rptSC = new LabRpt();
                        //rptSC.WindowState = FormWindowState.Maximized;
                        //rptSC.Show();
                    }
                }
            }
        }

        private void PrtAuditClickHandlerGPLS(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (rptTitle.IndexOf("Audit") != -1)
                {
                    if (rptTitle == "Audit Trail")
                    {

                        string strPattern = "([a-z?])[_ ]?([A-Z])";

                        DataTable dtAudit = new DataTable();
                        dtAudit.Columns.Add("KeyDataName", typeof(string));
                        dtAudit.Columns.Add("KeyDataValue", typeof(string));
                        dtAudit.Columns.Add("KeyDataDesc", typeof(string));
                        dtAudit.Columns.Add("DataName", typeof(string));
                        dtAudit.Columns.Add("DataType", typeof(string));
                        dtAudit.Columns.Add("ActionTaken", typeof(string));
                        dtAudit.Columns.Add("OldValue", typeof(string));
                        dtAudit.Columns.Add("NewValue", typeof(string));
                        dtAudit.Columns.Add("DateDone", typeof(string));
                        dtAudit.Columns.Add("DoneBy", typeof(string));

                        int nProdID = 46;

                        //Products Master Table
                        DataTable dtAuLogGPLS = PSSClass.AuditReport.ProductsGPLS(nProdID);

                        DataTable dtAuLog = PSSClass.AuditReport.AuProducts(nProdID);

                        if (dtAuLog != null && dtAuLog.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtAuLog.Columns.Count; j++)
                            {
                                if (j < dtAuLog.Columns.Count - 3)
                                {
                                    if (dtAuLogGPLS.Rows[0][j].ToString() != dtAuLog.Rows[0][j].ToString())
                                    {
                                        string strType = dtAuLog.Columns[j].DataType.ToString();
                                        DataRow dR = dtAudit.NewRow();
                                        dR["KeyDataName"] = "Product ID";
                                        dR["KeyDataValue"] = dtAuLog.Rows[0]["ProductID"].ToString();
                                        dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                        dR["DataName"] = Regex.Replace(dtAuLog.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                        dR["DataType"] = strType;
                                        dR["OldValue"] = dtAuLog.Rows[0][j];
                                        dR["NewValue"] = dtAuLogGPLS.Rows[0][j];
                                        dR["ActionTaken"] = dtAuLog.Rows[0]["FileMaintCode"].ToString();
                                        dR["Datedone"] = dtAuLog.Rows[0]["FileMaintDate"].ToString();
                                        dR["DoneBy"] = dtAuLog.Rows[0]["FileMaintByID"].ToString();
                                        dtAudit.Rows.Add(dR);
                                    }
                                }
                            }

                            int k = 1;
                            for (int i = 0; i < dtAuLog.Rows.Count; i++)
                            {
                                if (k >= dtAuLog.Rows.Count)
                                    break;
                                for (int j = 0; j < dtAuLog.Columns.Count; j++)
                                {
                                    if (j < dtAuLog.Columns.Count - 3)
                                    {
                                        if (dtAuLog.Rows[i][j].ToString() != dtAuLog.Rows[k][j].ToString())
                                        {
                                            string strType = dtAuLog.Columns[j].DataType.ToString();

                                            DataRow dR = dtAudit.NewRow();
                                            dR["KeyDataName"] = "Product ID";
                                            dR["KeyDataValue"] = dtAuLog.Rows[i]["ProductID"].ToString();
                                            dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                            dR["DataType"] = strType;
                                            dR["DataName"] = Regex.Replace(dtAuLog.Columns[j].ColumnName.ToString(), strPattern, "$1 $2");
                                            dR["OldValue"] = dtAuLog.Rows[k][j];
                                            dR["NewValue"] = dtAuLog.Rows[i][j];
                                            dR["ActionTaken"] = dtAuLog.Rows[k]["FileMaintCode"].ToString();
                                            dR["Datedone"] = dtAuLog.Rows[k]["FileMaintDate"].ToString();
                                            dR["DoneBy"] = dtAuLog.Rows[k]["FileMaintByID"].ToString();
                                            dtAudit.Rows.Add(dR);
                                        }
                                    }
                                }
                                k++;
                            }
                        }
                        //Get All Label No. Activities
                        DataTable dtAuLabelNos = PSSClass.AuditReport.LabelNosGPLS(nProdID);
                        if (dtAuLabelNos != null && dtAuLabelNos.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                            {
                                if (dtAuLabelNos.Rows[i]["DatePrinted"] != DBNull.Value)
                                {
                                    string strType = dtAuLabelNos.Columns["DatePrinted"].DataType.ToString();
                                    DataRow dR = dtAudit.NewRow();
                                    dR["KeyDataName"] = "PRODUCT ID";
                                    dR["KeyDataValue"] = nProdID.ToString();
                                    dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                    dR["DataType"] = strType;
                                    dR["DataName"] = "Date Printed (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                        dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                                    dR["OldValue"] = DBNull.Value;
                                    dR["NewValue"] = dtAuLabelNos.Rows[i]["DatePrinted"].ToString();
                                    dR["ActionTaken"] = "2";
                                    dR["Datedone"] = dtAuLabelNos.Rows[i]["DatePrinted"].ToString();
                                    dR["DoneBy"] = PSSClass.Users.GPLSUserName(Convert.ToInt16(dtAuLabelNos.Rows[i]["PrintedByID"]));
                                    dtAudit.Rows.Add(dR);
                                }
                            }
                            //Voided
                            for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                            {
                                if (dtAuLabelNos.Rows[i]["DateVoided"] != DBNull.Value)
                                {
                                    string strType = dtAuLabelNos.Columns["DateVoided"].DataType.ToString();
                                    DataRow dR = dtAudit.NewRow();
                                    dR["KeyDataName"] = "PRODUCT ID";
                                    dR["KeyDataValue"] = nProdID.ToString();
                                    dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                    dR["DataType"] = strType;
                                    dR["DataName"] = "DateVoided (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                        dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                                    dR["OldValue"] = DBNull.Value;
                                    dR["NewValue"] = dtAuLabelNos.Rows[i]["DateVoided"].ToString();
                                    dR["ActionTaken"] = "2";
                                    dR["Datedone"] = dtAuLabelNos.Rows[i]["DateVoided"].ToString();
                                    dR["DoneBy"] = PSSClass.Users.GPLSUserName(Convert.ToInt16(dtAuLabelNos.Rows[i]["VoidedByID"]));
                                    dtAudit.Rows.Add(dR);
                                }
                            }
                            //Approved
                            for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                            {
                                if (dtAuLabelNos.Rows[i]["DateApproved"] != DBNull.Value)
                                {
                                    string strType = dtAuLabelNos.Columns["DateApproved"].DataType.ToString();
                                    DataRow dR = dtAudit.NewRow();
                                    dR["KeyDataName"] = "PRODUCT ID";
                                    dR["KeyDataValue"] = nProdID.ToString();
                                    dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                    dR["DataType"] = strType;
                                    dR["DataName"] = "DateApproved (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                        dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                                    dR["OldValue"] = DBNull.Value;
                                    dR["NewValue"] = dtAuLabelNos.Rows[i]["DateApproved"].ToString();
                                    dR["ActionTaken"] = "2";
                                    dR["Datedone"] = dtAuLabelNos.Rows[i]["DateApproved"].ToString();
                                    dR["DoneBy"] = PSSClass.Users.GPLSUserName(Convert.ToInt16(dtAuLabelNos.Rows[i]["ApprovedByID"]));
                                    dtAudit.Rows.Add(dR);
                                }
                            }
                            //Added
                            for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                            {
                                if (dtAuLabelNos.Rows[i]["LabelStatus"].ToString() == "2")
                                {
                                    string strType = dtAuLabelNos.Columns["DateApproved"].DataType.ToString();
                                    DataRow dR = dtAudit.NewRow();
                                    dR["KeyDataName"] = "PRODUCT ID";
                                    dR["KeyDataValue"] = nProdID.ToString();
                                    dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                    dR["DataType"] = strType;
                                    dR["DataName"] = "DateApproved (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                        dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                                    dR["OldValue"] = DBNull.Value;
                                    dR["NewValue"] = dtAuLabelNos.Rows[i]["DateApproved"].ToString();
                                    dR["ActionTaken"] = "1";
                                    dR["Datedone"] = dtAuLabelNos.Rows[i]["DateApproved"].ToString();
                                    dR["DoneBy"] = PSSClass.Users.GPLSUserName(Convert.ToInt16(dtAuLabelNos.Rows[i]["ApprovedByID"]));
                                    dtAudit.Rows.Add(dR);
                                }
                            }
                            //Rejected
                            for (int i = 0; i < dtAuLabelNos.Rows.Count; i++)
                            {
                                if (dtAuLabelNos.Rows[i]["DateRejected"] != DBNull.Value)
                                {
                                    string strType = dtAuLabelNos.Columns["DateRejected"].DataType.ToString();
                                    DataRow dR = dtAudit.NewRow();
                                    dR["KeyDataName"] = "PRODUCT ID";
                                    dR["KeyDataValue"] = nProdID.ToString();
                                    dR["KeyDataDesc"] = dtAuLogGPLS.Rows[0]["ProductDesc"].ToString();
                                    dR["DataType"] = strType;
                                    dR["DataName"] = "DateRejected (" + dtAuLabelNos.Rows[i]["CtrlID"].ToString() + "-" +
                                        dtAuLabelNos.Rows[i]["CategoryCode"].ToString() + "-" + Convert.ToInt16(dtAuLabelNos.Rows[i]["LabelNo"]).ToString("000") + ")";
                                    dR["OldValue"] = DBNull.Value;
                                    dR["NewValue"] = dtAuLabelNos.Rows[i]["DateRejected"].ToString();
                                    dR["ActionTaken"] = "2";
                                    dR["Datedone"] = dtAuLabelNos.Rows[i]["DateRejected"].ToString();
                                    dR["DoneBy"] = PSSClass.Users.GPLSUserName(Convert.ToInt16(dtAuLabelNos.Rows[i]["RejectedByID"]));
                                    dtAudit.Rows.Add(dR);
                                }
                            }
                        }
                        if (dtAudit == null || dtAudit.Rows.Count == 0)
                        {
                            MessageBox.Show("No audit records found.", Application.ProductName);
                            return;
                        }
                        AuditRpt rpt = new AuditRpt();
                        rpt.rptName = "Audit Products";
                        rpt.dt = dtAudit;
                        rpt.WindowState = FormWindowState.Maximized;
                        rpt.Show();
                    }
                }
            }
        }


        private void SamplesLogin_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "SamplesLogin");

            LoadRecords();
            LoadSponsorsDDL();
            LoadStudyDir();
            LoadSC();

            //m_oWorker.RunWorkerAsync();

            BuildPrintItems();
            //MAIN
            dtLogFM.Columns.Add("CompanyCode", typeof(string));
            dtLogFM.Columns.Add("SponsorID", typeof(Int16));
            dtLogFM.Columns.Add("SponsorName", typeof(string));
            dtLogFM.Columns.Add("ContactID", typeof(Int16));
            dtLogFM.Columns.Add("ContactName", typeof(string));
            dtLogFM.Columns.Add("CtrldSubstance", typeof(bool));
            dtLogFM.Columns.Add("Rush", typeof(bool));
            dtLogFM.Columns.Add("ArticleDesc", typeof(string));
            dtLogFM.Columns.Add("SampleDesc", typeof(string));
            dtLogFM.Columns.Add("AddlNotes", typeof(string));
            dtLogFM.Columns.Add("StorageCode", typeof(string));
            dtLogFM.Columns.Add("ReceiptCode", typeof(string));
            dtLogFM.Columns.Add("StorageDesc", typeof(string));
            dtLogFM.Columns.Add("SSFormNo", typeof(Int32));
            dtLogFM.Columns.Add("AnalystDone", typeof(bool));
            dtLogFM.Columns.Add("ManagerChecked", typeof(bool));
            dtLogFM.Columns.Add("DateCreated", typeof(DateTime));
            dtLogFM.Columns.Add("DateReceived", typeof(DateTime));
            dtLogFM.Columns.Add("DateCancelled", typeof(DateTime));
            bsLogFM.DataSource = dtLogFM;
            //6/14/2017
            txtCmpyCode.DataBindings.Add("Text", bsLogFM, "CompanyCode");
            txtSponsorID.DataBindings.Add("Text", bsLogFM, "SponsorID");
            txtSponsor.DataBindings.Add("Text", bsLogFM, "SponsorName");
            txtContact.DataBindings.Add("Text", bsLogFM, "ContactName");
            txtContactID.DataBindings.Add("Text", bsLogFM, "ContactID");
            chkCtrldSubs.DataBindings.Add("Checked", bsLogFM, "CtrldSubstance", true);
            chkRush.DataBindings.Add("Checked", bsLogFM, "Rush", true);
            txtArticle.DataBindings.Add("Text", bsLogFM, "ArticleDesc");
            txtGenDesc.DataBindings.Add("Text", bsLogFM, "SampleDesc"); //added 7/6/2016
            txtAddlNotes.DataBindings.Add("Text", bsLogFM, "AddlNotes");
            txtStorageCode.DataBindings.Add("Text", bsLogFM, "StorageCode");
            txtRecCode.DataBindings.Add("Text", bsLogFM, "ReceiptCode");
            txtOtherStorage.DataBindings.Add("Text", bsLogFM, "StorageDesc");
            txtSSFormNo.DataBindings.Add("Text", bsLogFM, "SSFormNo"); //added 2/8/2017        
            chkAnalyst.DataBindings.Add("Checked", bsLogFM, "AnalystDone", true);
            chkManager.DataBindings.Add("Checked", bsLogFM, "ManagerChecked", true);
            dtpEntered.DataBindings.Add("Value", bsLogFM, "DateCreated", true);
            dtpReceived.DataBindings.Add("Value", bsLogFM, "DateReceived", true);
            mskDateCancelled.DataBindings.Add("Text", bsLogFM, "DateCancelled", true);
            //===========

            //SAMPLES 
            dtSamples.Columns.Add("SlashNo", typeof(string));
            dtSamples.Columns.Add("SampleDesc", typeof(string));
            dtSamples.Columns.Add("OtherDesc1", typeof(string));
            dtSamples.Columns.Add("OtherDesc2", typeof(string));
            dtSamples.Columns.Add("SlashID", typeof(Int16));
            bsSamples.DataSource = dtSamples;
            bnSamples.BindingSource = bsSamples;
            dgvSamples.DataSource = bsSamples;

            dgvSamples.Columns["SlashNo"].HeaderText = "Slash No.";
            dgvSamples.Columns["SlashNo"].Width = 80;
            dgvSamples.Columns["SampleDesc"].HeaderText = "Additional Description";
            dgvSamples.Columns["SampleDesc"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvSamples.Columns["SampleDesc"].Width = 300;
            dgvSamples.Columns["OtherDesc1"].HeaderText = "Lot No."; //default header
            dgvSamples.Columns["OtherDesc1"].Width = 115;
            dgvSamples.Columns["OtherDesc1"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvSamples.Columns["OtherDesc2"].HeaderText = "Other ID"; //default header
            dgvSamples.Columns["OtherDesc2"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvSamples.Columns["OtherDesc2"].Width = 115;
            dgvSamples.Columns["SlashID"].Visible = false;
            dgvSamples.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            DataGridViewButtonColumn btnExtData = new DataGridViewButtonColumn();
            btnExtData.Text = "Ext. Data";
            btnExtData.Name = "ExtData";
            btnExtData.HeaderText = "";
            btnExtData.Width = 65;
            btnExtData.UseColumnTextForButtonValue = true;
            dgvSamples.Columns.AddRange(btnExtData);

            DataGridViewButtonColumn btnCopyRow = new DataGridViewButtonColumn();
            btnCopyRow.Text = "Copy Row";
            btnCopyRow.Name = "CopyRow";
            btnCopyRow.HeaderText = "";
            btnCopyRow.Width = 70;
            btnCopyRow.UseColumnTextForButtonValue = true;
            dgvSamples.Columns.AddRange(btnCopyRow);

            StandardDGVSetting(dgvSamples);

            //SAMPLES ADDL DATA
            dtSamplesAddl.Columns.Add("SlashNo", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc3", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc4", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc5", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc6", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc7", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc8", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc9", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc10", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc11", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc12", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc13", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc14", typeof(string));
            dtSamplesAddl.Columns.Add("OtherDesc15", typeof(string));
            bsSamplesAddl.DataSource = dtSamplesAddl;
            dgvAddlData.DataSource = bsSamplesAddl;
            //Tests Table
            dtLogTests.Columns.Add("ServiceCode", typeof(Int16));
            dtLogTests.Columns.Add("ServiceDesc", typeof(string));
            dtLogTests.Columns.Add("ProtocolNo", typeof(string));
            dtLogTests.Columns.Add("StartDate", typeof(DateTime));
            dtLogTests.Columns.Add("EndDate", typeof(DateTime));
            dtLogTests.Columns.Add("PONo", typeof(string));
            dtLogTests.Columns.Add("TestSamples", typeof(string));
            dtLogTests.Columns.Add("Slashes", typeof(string));
            dtLogTests.Columns.Add("BillQty", typeof(int));
            dtLogTests.Columns.Add("QuotationNo", typeof(string));
            dtLogTests.Columns.Add("BookNo", typeof(int));
            dtLogTests.Columns.Add("EC", typeof(bool));
            dtLogTests.Columns.Add("ECCompType", typeof(Int16));
            dtLogTests.Columns.Add("ECLength", typeof(Int16));
            dtLogTests.Columns.Add("ECEndDate", typeof(DateTime));
            dtLogTests.Columns.Add("DateSampled", typeof(DateTime));
            dtLogTests.Columns.Add("QuoteFlag", typeof(string));
            dtLogTests.Columns.Add("ReportNo", typeof(Int32));
            dtLogTests.Columns.Add("StudyNo", typeof(string));
            dtLogTests.Columns.Add("StudyDirID", typeof(Int16));
            dtLogTests.Columns.Add("SCExtData", typeof(string));
            dtLogTests.Columns.Add("AddlNotes", typeof(string));
            dtLogTests.Columns.Add("QCompanyCode", typeof(string));
            bsLogTests.DataSource = dtLogTests;
            bnLogTests.BindingSource = bsLogTests;
            dtrLogTests.DataSource = bsLogTests;

            //SAMPLES/SC Table
            DataGridViewComboBoxColumn cboSN = new DataGridViewComboBoxColumn();
            DataGridViewComboBoxColumn cboSSC = new DataGridViewComboBoxColumn();

            dgvSampleSC.Columns.Add(cboSN);
            dgvSampleSC.Columns.Add(cboSSC);

            dgvSampleSC.Columns[0].Name = "SlashNo";
            dgvSampleSC.Columns[1].Name = "ServiceCode";

            dgvSampleSC.Columns["SlashNo"].Width = 75;
            dgvSampleSC.Columns["ServiceCode"].Width = 60;
            dgvSampleSC.Columns["SlashNo"].HeaderText = "Slash No.";
            dgvSampleSC.Columns["ServiceCode"].HeaderText = "Service Code";

            dtSampleSC.Columns.Add("Slash", typeof(string));
            dtSampleSC.Columns.Add("SC", typeof(Int16));

            bsSampleSC.DataSource = dtSampleSC;
            bnSampleSC.BindingSource = bsSampleSC;
            dgvSampleSC.DataSource = bsSampleSC;

            StandardDGVSetting(dgvSampleSC);
            dgvSampleSC.Columns[2].Visible = false;
            dgvSampleSC.Columns[3].Visible = false;

            //Billing References
            dtBilling.Columns.Add("CmpyCode", typeof(string));
            dtBilling.Columns.Add("QuoteNo", typeof(string));
            dtBilling.Columns.Add("ServiceCode", typeof(Int16));
            dtBilling.Columns.Add("ServiceDesc", typeof(string));
            dtBilling.Columns.Add("TestDesc1", typeof(string));
            dtBilling.Columns.Add("UnitDesc", typeof(string));
            dtBilling.Columns.Add("BillQty", typeof(Int16));
            dtBilling.Columns.Add("SelectedTest", typeof(bool));
            dtBilling.Columns.Add("Rush", typeof(bool));
            dtBilling.Columns.Add("UnitPrice", typeof(decimal));
            dtBilling.Columns.Add("RushPrice", typeof(decimal));
            dtBilling.Columns.Add("ControlNo", typeof(Int16));
            dtBilling.Columns.Add("QCmpyCode", typeof(string));
            bsBilling.DataSource = dtBilling;
            dgvTests.DataSource = bsBilling;

            StandardDGVSetting(dgvTests);
            dgvTests.Columns["CmpyCode"].HeaderText = "CMPY CODE";
            dgvTests.Columns["QuoteNo"].HeaderText = "QUOTE NO.";
            dgvTests.Columns["QuoteNo"].Width = 85;
            dgvTests.Columns["QuoteNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTests.Columns["ServiceCode"].HeaderText = "SC";
            dgvTests.Columns["ServiceCode"].Width = 55;
            dgvTests.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTests.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvTests.Columns["ServiceDesc"].Width = 250;
            dgvTests.Columns["TestDesc1"].HeaderText = "TEST DESCRIPTION";
            dgvTests.Columns["TestDesc1"].Width = 300;
            dgvTests.Columns["UnitDesc"].HeaderText = "UNIT";
            dgvTests.Columns["UnitDesc"].Width = 82;
            dgvTests.Columns["BillQty"].HeaderText = "BILL QTY.";
            dgvTests.Columns["BillQty"].Width = 55;
            dgvTests.Columns["BillQty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTests.Columns["BillQty"].DefaultCellStyle.Format = "#,##0";
            dgvTests.Columns["SelectedTest"].HeaderText = "SELECT";
            dgvTests.Columns["SelectedTest"].Width = 55;
            dgvTests.Columns["Rush"].HeaderText = "RUSH";
            dgvTests.Columns["Rush"].Width = 55;
            dgvTests.Columns["UnitPrice"].Visible = false;
            dgvTests.Columns["RushPrice"].Visible = false;
            dgvTests.Columns["ControlNo"].Visible = false;
            dgvTests.Columns["CmpyCode"].Visible = false;
            dgvTests.Columns["QCmpyCode"].Visible = false;
            
            //SC Extended Data
            dtSCExtData.Columns.Add("ServiceCode", typeof(Int16));
            dtSCExtData.Columns.Add("StudyNo", typeof(Int32));
            dtSCExtData.Columns.Add("StudyDirID", typeof(Int16));
            dtSCExtData.Columns.Add("SCExtDataLabel", typeof(string));
            dtSCExtData.Columns.Add("SCExtDataValue", typeof(string));
            dtSCExtData.Columns.Add("PrtNotes", typeof(string));
            dtSCExtData.Columns.Add("NonPrtNotes", typeof(string));
            bsSCExtData.DataSource = dtSCExtData;

            //txtStudyNo.DataBindings.Add("Text", bsSCExtData, "StudyNo", true);
            //txtStudyDirID.DataBindings.Add("Text", bsSCExtData, "StudyDirID", true);
            txtSC1.DataBindings.Add("Text", bsSCExtData, "SCExtData1", true);
            txtSC2.DataBindings.Add("Text", bsSCExtData, "SCExtData2", true);
            txtSC3.DataBindings.Add("Text", bsSCExtData, "SCExtData3", true);
            txtSC4.DataBindings.Add("Text", bsSCExtData, "SCExtData4", true);
            txtSC5.DataBindings.Add("Text", bsSCExtData, "SCExtData5", true);
            txtSC6.DataBindings.Add("Text", bsSCExtData, "SCExtData6", true);
            txtSC7.DataBindings.Add("Text", bsSCExtData, "SCExtData7", true);
            txtSC8.DataBindings.Add("Text", bsSCExtData, "SCExtData8", true);
            txtSC9.DataBindings.Add("Text", bsSCExtData, "SCExtData9", true);
            txtSC10.DataBindings.Add("Text", bsSCExtData, "SCExtData10", true);
            txtPrtNotes.DataBindings.Add("Text", bsSCExtData, "PrtNotes", true);
            txtNonPrtNotes.DataBindings.Add("Text", bsSCExtData, "NonPrtNotes", true);

            //Slash Extended Data 
            dtSlashExtData.Columns.Add("SlashNo", typeof(string));
            dtSlashExtData.Columns.Add("ExtDataLabel", typeof(string));
            dtSlashExtData.Columns.Add("ExtDataValue", typeof(string));
            bsSlashExtData.DataSource = dtSlashExtData;

            if (nFR == 1)
            {
                txtCmpyCode.Text = pubCmpyCode;
                txtLogNo.Text = nLogNo.ToString();
                PSSClass.General.FindRecord("PSSNo", txtLogNo.Text, bsFile, dgvFile);
                dgvFile.Select();
                dgvFile.CurrentCell = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells[0];
                SendKeys.Send("{Enter}");
            }

        }

        private void DateCreatedBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "__/__/____";
        }

        private void DateRecBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "__/__/____";
        }
        
        private void DateCancBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "__/__/____";
        }

        private void LoadRecords()
        {
            nMode = 0; nIngredion = 0;
            if (nSearch == 99)
                dtLogMaster = PSSClass.Samples.SampLogMaster();
            else if (nSearch == 2)
                dtLogMaster = LoginSearch.dtLoginSearch;  //PSSClass.Samples.LogSearchSamples(strCriteria, strData);
            else if (nSearch == 3)
                dtLogMaster = LoginSearch.dtLoginSearch; //PSSClass.Samples.LogSearchMaster(strCriteria, strData);
            else if (nSearch == 4)
                dtLogMaster = LoginSearch.dtLoginSearch; //PSSClass.Samples.LogSearchTests(strCriteria, strData, nSSC, nSSpID);
            else if (nSearch == 5)
                dtLogMaster = LoginSearch.dtLoginSearch; //PSSClass.Samples.LogSearchInv(strData);
            else if (nSearch == 13)
                dtLogMaster = PSSClass.Samples.LogSearchMaster(strCriteria, strData);
            else if (nSearch == 88)
                dtLogMaster = PSSClass.Samples.LogSearchMaster("PSS No.", txtLogNo.Text);

            if (nSearch != 99 && (dtLogMaster == null || dtLogMaster.Rows.Count == 0))
            {
                MessageBox.Show("No matching records found or loading error encountered!" + Environment.NewLine + "System would now reload current records.");
                nCtr = 0;
                dtLogMaster = PSSClass.Samples.SampLogMaster();
            }
            bsFile.DataSource = dtLogMaster;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            if (tsddbSearch.DropDownItems.Count == 0)
            {
                int i = 0;
                int n = 0;

                arrCol = new string[dtLogMaster.Columns.Count];

                ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

                foreach (DataColumn colFile in dtLogMaster.Columns)
                {
                    items[i] = new ToolStripMenuItem();
                    items[i].Name = colFile.ColumnName;

                    //Using LINQ to insert space between capital letters
                    var val = colFile.ColumnName;
                    val = string.Concat(val.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

                    items[i].Text = val;
                    items[i].Click += new EventHandler(SearchItemClickHandler);
                    arrCol[i] = colFile.DataType.ToString();
                    cklColumns.Items.Add(val);
                    //}
                    i += 1;
                }
                for (int j = 0; j < cklColumns.Items.Count; j++)
                {
                    cklColumns.SetItemChecked(j, true);
                }
                tsddbSearch.DropDownItems.AddRange(items);
                tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
                tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
            }
            dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false; 
            FileAccess();
            timer1.Enabled = false; lblLoadStatus.Visible = false;
        }

        private void LoadSponsorsDDL()
        {
            dtSponsors = PSSClass.Sponsors.SponsorNamesDDL();
            dgvSponsors.DataSource = null;
            dgvSponsors.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
        }

        private void LoadSC()
        {
            DataTable dt = new DataTable();
            dtSC = PSSClass.ServiceCodes.SCDDL();
            if (dtSC == null)
            {
                return;                
            }
            arrSC = new string[dtSC.Rows.Count - 1, 2];
            for (int i = 0; i < dtSC.Rows.Count - 1; i++)
            {
                arrSC[i,0] = dtSC.Rows[i]["ServiceCode"].ToString();
                arrSC[i, 1] = dtSC.Rows[i]["ServiceDesc"].ToString();
            }

            dtSCMaster = PSSClass.ServiceCodes.SCDDLCombo();
            if (dtSCMaster == null)
            {
                return;
            }
            dgvSC.DataSource = dtSCMaster;
        }

        private void LoadPO()
        {
            if (txtSponsorID.Text == "")
            {
                MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dgvPONo.DataSource = null;
            dtPONo = PSSClass.PO.PODDL(Convert.ToInt16(txtSponsorID.Text));
            if (dtPONo != null)
            {
                try
                {
                    ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).DataSource = dtPONo;
                    ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Columns[0].Width = 125;
                }
                catch { }
            }
        }

        private void LoadStudyDir()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Employees.StudyDirectors();
            if (dt == null)
            {
                return;
            }
            cboStudyDir.DataSource = dt;
            cboStudyDir.DisplayMember = "EmployeeName";
            cboStudyDir.ValueMember = "EmployeeID";

            DataRow dR = dt.NewRow();
            dR["EmployeeName"] = "--select--";
            dR["EmployeeID"] = "0";
            dt.Rows.InsertAt(dR, 0);
            cboStudyDir.SelectedIndex = 0;
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["CompanyCode"].HeaderText = "CMPY CODE";
            dgvFile.Columns["PSSNo"].HeaderText = "ORDER NO.";
            dgvFile.Columns["RecDate"].HeaderText = "DATE RECEIVED";
            dgvFile.Columns["SC"].HeaderText = "SERVICE CODE";
            dgvFile.Columns["SCDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvFile.Columns["SpID"].HeaderText = "SPONSOR ID";
            dgvFile.Columns["SpName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["ConName"].HeaderText = "CONTACT";
            dgvFile.Columns["Article"].HeaderText = "ARTICLE NAME";
            dgvFile.Columns["PONo"].HeaderText = "PO NO.";
            dgvFile.Columns["ReportNo"].HeaderText = "REPORT NO.";
            dgvFile.Columns["RevisionNo"].HeaderText = "REV. NO.";
            dgvFile.Columns["DateEMailed"].HeaderText = "REPORT MAIL DATE";
            dgvFile.Columns["InvoiceNo"].HeaderText = "INV. NO.";
            dgvFile.Columns["InvoiceDate"].HeaderText = "INV. DATE";
            dgvFile.Columns["DateMailed"].HeaderText = "INV. MAIL DATE";
            dgvFile.Columns["LoginName"].HeaderText = "CREATED BY";
            dgvFile.Columns["CompanyCode"].Width = 75;
            dgvFile.Columns["PSSNo"].Width = 75;
            dgvFile.Columns["PSSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["RecDate"].Width = 75;
            dgvFile.Columns["RecDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["RecDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SC"].Width = 75;
            dgvFile.Columns["SC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SCDesc"].Width = 100;
            dgvFile.Columns["SpID"].Width = 75;
            dgvFile.Columns["SpID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SpName"].Width = 150;
            dgvFile.Columns["ConName"].Width = 150;
            dgvFile.Columns["Article"].Width = 200;
            dgvFile.Columns["PONo"].Width = 80;
            dgvFile.Columns["ReportNo"].Width = 80;
            dgvFile.Columns["ReportNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["REPORT DATE"].Width = 80;
            dgvFile.Columns["REPORT DATE"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["REPORT DATE"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["RevisionNo"].Width = 50;
            dgvFile.Columns["RevisionNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateEMailed"].Width = 95;
            dgvFile.Columns["DateEMailed"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateEMailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["InvoiceNo"].Width = 80;
            dgvFile.Columns["InvoiceNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["InvoiceDate"].Width = 90;
            dgvFile.Columns["InvoiceDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["InvoiceDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateMailed"].Width = 95;
            dgvFile.Columns["DateMailed"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateMailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["WordReport"].Visible = false;
            dgvFile.Columns["DateApproved"].Visible = false;
            dgvFile.Columns["SCSp"].Visible = false;
            dgvFile.Columns["ContactID"].Visible = false;
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData(); btnClose.Visible = true; 
            }
        }

        private void LoadData()
        {
            //ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            btnClose.Visible = true; btnClose.BringToFront();
            OpenControls(pnlRecord, false); OpenControls(pnlOthers, false); OpenControls(pnlIngredion, false);
            OpenControls(tabComments, false);

            cboSlashSC.Enabled = false;

            ////txtSponsorID.Text = dgvFile.CurrentRow.Cells["SpID"].Value.ToString();
            ////txtSponsor.Text = dgvFile.CurrentRow.Cells["SpName"].Value.ToString();
            ////txtContactID.Text = dgvFile.CurrentRow.Cells["ContactID"].Value.ToString();
            ////txtContact.Text = dgvFile.CurrentRow.Cells["ConName"].Value.ToString();
            ////txtArticle.Text = dgvFile.CurrentRow.Cells["Article"].Value.ToString();

            nMode = 0; nIngredion = 0;
            ////if (nFR == 1)
            ////    txtLogNo.Text = nLogNo.ToString();
            ////else
            ////    txtLogNo.Text = dgvFile.CurrentRow.Cells[0].Value.ToString();

            if (nFR != 1)
            {
                txtCmpyCode.Text = dgvFile.CurrentRow.Cells["CompanyCode"].Value.ToString();
                txtLogNo.Text = dgvFile.CurrentRow.Cells["PSSNo"].Value.ToString();
            }

            //dtLogFM = null;
            dtLogFM = PSSClass.Samples.LogMasterData(txtCmpyCode.Text, Convert.ToInt32(txtLogNo.Text));
            if (dtLogFM == null || dtLogFM.Rows.Count == 0)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            //foreach (Control c in pnlRecord.Controls)
            //{
            //    c.DataBindings.Clear();
            //}
            //foreach (Control c in pnlOthers.Controls)
            //{
            //    c.DataBindings.Clear();
            //}
            //foreach (Control c in tabComments.Controls)
            //{
            //    c.DataBindings.Clear();
            //}
            //foreach (Control c in grpStorage.Controls)
            //{
            //    c.DataBindings.Clear();
            //}
            bsLogFM.DataSource = dtLogFM;
            //txtSponsorID.DataBindings.Add("Text", bsLogFM, "SponsorID");
            //txtSponsor.DataBindings.Add("Text", bsLogFM, "SponsorName");
            //txtContact.DataBindings.Add("Text", bsLogFM, "ContactName");
            //txtContactID.DataBindings.Add("Text", bsLogFM, "ContactID");
            //chkCtrldSubs.DataBindings.Add("Checked", bsLogFM, "CtrldSubstance", true);
            //chkRush.DataBindings.Add("Checked", bsLogFM, "Rush", true);
            //txtArticle.DataBindings.Add("Text", bsLogFM, "ArticleDesc");
            //txtAddlNotes.DataBindings.Add("Text", bsLogFM, "AddlNotes");
            //txtStorageCode.DataBindings.Add("Text", bsLogFM, "StorageCode");
            //txtRecCode.DataBindings.Add("Text", bsLogFM, "ReceiptCode");
            //txtOtherStorage.DataBindings.Add("Text", bsLogFM, "StorageDesc");
            //txtGenDesc.DataBindings.Add("Text", bsLogFM, "SampleDesc"); //added 7/6/2016
            //txtSSFormNo.DataBindings.Add("Text", bsLogFM, "SSFormNo"); //added 2/8/2017           

            ////tabLogin.SelectedIndex = 1;
            ////((TextBox)tabComments.Controls["txtGenDesc"]).DataBindings.Add("Text", bsLogFM, "SampleDesc");
            ////tabLogin.SelectedIndex = 0;



            //Binding DateCreatedBinding;
            //DateCreatedBinding = new Binding("Value", bsLogFM, "DateCreated");
            //DateCreatedBinding.Format += new ConvertEventHandler(DateCreatedBinding_Format);
            //dtpEntered.DataBindings.Add(DateCreatedBinding);

            //Binding DateRecBinding;
            //DateRecBinding = new Binding("Value", bsLogFM, "DateReceived");
            //DateRecBinding.Format += new ConvertEventHandler(DateRecBinding_Format);
            //dtpReceived.DataBindings.Add(DateRecBinding);

            //Binding DateCancBinding;
            //DateCancBinding = new Binding("Text", bsLogFM, "DateCancelled");
            //DateCancBinding.Format += new ConvertEventHandler(DateCancBinding_Format);
            //mskDateCancelled.DataBindings.Add(DateCancBinding);

            ////if (dtLogFM.Rows[0]["DateCancelled"].ToString() != "")
            ////    chkCancelled.Checked = true;
            ////else
                chkCancelled.Checked = false;

            if (dtLogFM.Rows[0]["StorageCode"].ToString() == "1")
                rdoSAmbient.Checked = true;
            else if (dtLogFM.Rows[0]["StorageCode"].ToString() == "2")
                rdoRefrigerator.Checked = true;
            else if (dtLogFM.Rows[0]["StorageCode"].ToString() == "3")
                rdoFreezer20.Checked = true;
            else if (dtLogFM.Rows[0]["StorageCode"].ToString() == "4")
                rdoFreezer80.Checked = true;
            else if (dtLogFM.Rows[0]["StorageCode"].ToString() == "5")
                rdoOther.Checked = true;

            if (dtLogFM.Rows[0]["ReceiptCode"].ToString() == "1")
                rdoRAmbient.Checked = true;
            else if (dtLogFM.Rows[0]["ReceiptCode"].ToString() == "2")
                rdoIcePack.Checked = true;
            else if (dtLogFM.Rows[0]["ReceiptCode"].ToString()  == "3")
                rdoDryIce.Checked = true;

            lnkImgFile.Text = PSSClass.Sponsors.SamplePicPath(Convert.ToInt16(txtSponsorID.Text)) + "\\" + txtLogNo.Text;

            cboQuotes.Enabled = false; btnSelQTests.Enabled = false; btnUnSelQTests.Enabled = false; btnSelAllTests.Enabled = false; btnTests.Enabled = false;
            LoadSamples();
            LoadLogTests();
            LoadSamplesSC();
            LoadBillingRef();
            LoadSlashExtData();
            LoadSCExtData();
            SamplesAddlDataLabels();
            btnAddSample.Enabled = true; btnEditSample.Enabled = true; btnDelSample.Enabled = true; btnSaveSample.Enabled = false; btnCancelSample.Enabled = false;
            pnlQuotes.Visible = false;
            btnLSPreview.Enabled = true; btnLSPrinter.Enabled = true; btnDataForm.Enabled = true; cboChainOfCustody.Enabled = true; btnPrintCOC.Enabled = true;

            if (txtSponsorID.Text == "130") //GIBRALTAR LABORATORIES
            {
                btnAddTest.Enabled = true; btnDelTest.Enabled = true;
            }
            else
            {
                btnAddTest.Enabled = false; btnDelTest.Enabled = false;
            }
            tabLogin.SelectedIndex = 0;
            if (strFileAccess == "RW" || strFileAccess == "FA")
                btnCopyGBL.Enabled = true;
            else
                btnCopyGBL.Enabled = false;
        }

        private void LoadSamplesSC()
        {
            //dgvSampleSC.DataSource = null;

            //DataGridViewComboBoxColumn colSC = new DataGridViewComboBoxColumn();
            //colSC.DataSource = dtLogTests;
            //colSC.ValueMember = "ServiceCode";
            //colSC.DisplayMember = "ServiceCode";
            //colSC.DataPropertyName = "ServiceCode";
            //dgvSampleSC.Columns.Add(colSC);


            //DataGridViewComboBoxColumn colSlash = new DataGridViewComboBoxColumn();
            //colSlash.DataSource = dtSamples;
            //colSlash.ValueMember = "SlashNo";
            //colSlash.DisplayMember = "SlashNo";
            //colSlash.DataPropertyName = "SlashNo";
            //dgvSampleSC.Columns.Add(colSlash);


            List<string> strListSSC = new List<string>();
            List<string> strLSC = new List<string>();

            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                strLSC.Add(dtLogTests.Rows[i]["ServiceCode"].ToString());
            }

            for (int i = 0; i < dgvSamples.Rows.Count - 1; i++)
            {
                strListSSC.Add(dgvSamples.Rows[i].Cells["SlashNo"].Value.ToString());
            }

            if (strListSSC.Count > 0)
                ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();

            if (strLSC.Count > 0)
                ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["ServiceCode"]).DataSource = strLSC.ToArray();


            dtSampleSC = PSSClass.Samples.SampleLogSC(Convert.ToInt32(txtLogNo.Text));
            if (dtSampleSC == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bsSampleSC.DataSource = dtSampleSC;
            bnSampleSC.BindingSource = bsSampleSC;
            dgvSampleSC.DataSource = bsSampleSC;

            for (int i = 0; i < dtSampleSC.Rows.Count; i++)
            {
                dgvSampleSC.Rows[i].Cells["SlashNo"].Value = dtSampleSC.Rows[i]["Slash"].ToString();
                dgvSampleSC.Rows[i].Cells["ServiceCode"].Value = dtSampleSC.Rows[i]["SC"].ToString();
            }
        }

        private void LoadLogTests()
        {
            dtLogTests = PSSClass.Samples.LogTestsData(txtCmpyCode.Text, Convert.ToInt32(txtLogNo.Text));
            if (dtLogTests == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            bsLogTests.DataSource = dtLogTests;
            dtrLogTests.DataSource = bsLogTests;
            dtrLogTests.Enabled = false;
        }

        private void AddClickHandler(object sender, EventArgs e)
        {
            AddRecord();
        }

        private void EditClickHandler(object sender, EventArgs e)
        {
            EditRecord();
        }

        private void DeleteClickHandler(object sender, EventArgs e)
        {
            DeleteRecord();
        }

        private void SaveClickHandler(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void CloseClickHandler(object sender, EventArgs e)
        {
            if (nFR == 1)
            {
                nFR = 0;
                SendKeys.Send("{F12}");
                return;
            }
            if (nMode != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            this.Close();
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            tsbRefresh.Enabled = false;
            nSearch = 99;
            LoadRecords();
            bsFile.Filter = "PSSNo<>0";
        }

        private void SearchItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            tstbSearchField.Text = clickedItem.Name;
            tstbSearch.SelectAll();
            tstbSearch.Focus();
            nIndex = tsddbSearch.DropDownItems.IndexOf(clickedItem);
            tslSearchData.Text = clickedItem.Text;
        }

        private void SearchOKClickHandler(object sender, EventArgs e)
        {
            try
            {
                bsFile.Filter = "PSSNo<>0";
                PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                dgvFile.Select();
                if (pnlRecord.Visible == true)
                    LoadData();
            }
            catch
            {
            }
        }

        private void SearchKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SearchFilterClickHandler(null, null);
            }
        }

        private void SearchFilterClickHandler(object sender, EventArgs e)
        {
            try
            {
                if (arrCol[nIndex] == "System.String")
                {
                    string strSearch = tstbSearch.Text.Replace("'", "''");
                    DateTime dte;
                    if (DateTime.TryParse(strSearch, out dte))
                    {
                        bsFile.Filter = tstbSearchField.Text + " = '" + Convert.ToDateTime(tstbSearch.Text).ToString("MM/dd/yyyy") + "'";
                    }
                    else
                    {
                        if (chkFullText.Checked == true)
                            bsFile.Filter = tstbSearchField.Text + "='" + strSearch + "'";
                        else
                            bsFile.Filter = tstbSearchField.Text + " LIKE '%" + strSearch + "%'";
                    }
                }
                else if (arrCol[nIndex] == "System.DateTime")
                {
                    bsFile.Filter = tstbSearchField.Text + " = #" + Convert.ToDateTime(tstbSearch.Text).ToString("MM/dd/yyyy") + "#";
                }
                else
                {
                    bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;
                }
                dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                tsbRefresh.Enabled = true;
            }
            catch
            {}
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            string strAccess = "";
            if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "ReportNo" && dgvFile.CurrentCell.Value.ToString() != "")
            {
                strAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "FinalReports");
                if (strAccess == "")
                    return;

                int intOpen = PSSClass.General.OpenForm(typeof(FinalReports));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(FinalReports));
                }
                FinalReports childForm = new FinalReports();
                childForm.Text = "FINAL REPORTS";
                childForm.MdiParent = this.MdiParent;
                childForm.nRptNo = Convert.ToInt32(dgvFile.CurrentCell.Value);
                childForm.nLSw = 1;
                childForm.Show();
            }
            else if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "InvoiceNo" && dgvFile.CurrentCell.Value.ToString() != "")
            {
                strAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "FinalBilling");
                if (strAccess == "")
                    return;

                int intOpen = PSSClass.General.OpenForm(typeof(FinalBilling));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(FinalBilling));
                }
                FinalBilling childForm = new FinalBilling();
                childForm.Text = "FINAL BILLING";
                childForm.MdiParent = this.MdiParent;
                childForm.nInvceNo = Convert.ToInt32(dgvFile.CurrentCell.Value);
                childForm.nFB= 2;
                childForm.Show();
            }
            else
                LoadData();
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            //if (nSw == 0)
            //{
            //    nSw = 1;
            //    timer1.Enabled = true;
            //}
            if (e.KeyChar == 13)
            {
                pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
                OpenControls(this.pnlRecord, false);
                if (dgvFile.Rows.Count > 0)
                    LoadData();
            }
            else if (e.KeyChar == 8)
            {
                tstbSearch.Text = tstbSearch.Text.Substring(0, tstbSearch.TextLength - 1);
            }
            else
            {
                tstbSearch.Text = tstbSearch.Text + e.KeyChar.ToString();
                //nCtr = 0;
            }
        }

        private void dgvKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCellChangedHandler(object sender, EventArgs e)
        {
            FileAccess();

            try
            {
                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() != "" || dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "")
                {
                    tsbEdit.Enabled = false; btnFAXEMail.Enabled = false;
                }
            }
            catch { }
            
            try
            {
                nIndex = dgvFile.CurrentCell.ColumnIndex;

                tsddbSearch.DropDownItems[nIndex].Select();
                tslSearchData.Text = tsddbSearch.DropDownItems[nIndex].Text;
                tstbSearchField.Text = tsddbSearch.DropDownItems[nIndex].Name;
            }
            catch
            { }
        }

        private void  AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            btnUnSelQTests.Enabled = true; btnSelQTests.Enabled = true; btnSelAllTests.Enabled = true; btnTests.Enabled = true;
            ClearControls(pnlRecord); ClearControls(pnlSCExtData); ClearControls(pnlSlashExtData); ClearControls(tabComments);
            OpenControls(pnlRecord, true); OpenControls(pnlSCExtData, true); OpenControls(pnlSlashExtData, true);
            pnlIngredion.Enabled = false;
            pnlSCExtData.Visible = false; pnlSlashExtData.Visible = false;

            btnDataForm.Enabled = false; btnLSPreview.Enabled = false; btnLSPrinter.Enabled = false; btnFAXEMail.Enabled = false;//to make sure user can't click while still adding the record           
            txtCmpyCode.Text = "P";
            txtLogNo.ReadOnly = true;
            txtLogNo.Text = "(New)";
            //Default Values
            rdoSAmbient.Checked = true; rdoRAmbient.Checked = true;

            dtpSAPDate.Format = DateTimePickerFormat.Custom;
            dtpSAPDate.CustomFormat = " ";

            dtpReceived.Value = DateTime.Now;
            dtpEntered.Value = DateTime.Now; dtpEntered.Enabled = false;
            txtSponsorID.Focus();

            strList.Clear(); strListQ.Clear(); strListSC.Clear(); strListCmpy.Clear(); 
            dtLogFM.Rows.Clear();
            dtSamples.Rows.Clear(); dtSamplesAddl.Rows.Clear(); dtLogTests.Rows.Clear(); dtSampleSC.Rows.Clear(); dtBilling.Rows.Clear();
            dtSCExtData.Rows.Clear(); dtSlashExtData.Rows.Clear(); dtrLogTests.Enabled = true;

            ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = null;
            ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["ServiceCode"]).DataSource = null;

            cboQuotes.DataSource = null;
            btnAddSample.Enabled = true; btnEditSample.Enabled = false; btnDelSample.Enabled = true; btnSaveSample.Enabled = false; btnCancelSample.Enabled = false;
            if (txtSponsorID.Text == "130")
            {
                btnAddTest.Enabled = true; btnDelTest.Enabled = true;
            }
            else
            {
                btnAddTest.Enabled = false; btnDelTest.Enabled = false;
            }

            DataRow drSample;
            drSample = dtSamples.NewRow();
            drSample["SlashNo"] = "";
            drSample["SampleDesc"] = "";
            drSample["OtherDesc1"] = "";
            drSample["OtherDesc2"] = "";
            drSample["SlashID"] = 0;
            dtSamples.Rows.Add(drSample);
            chkCancelled.Enabled = false;
            txtSSFormNo.Enabled = true;
            txtSSFormNo.Focus();
        }

        private void EditRecord()
        {
            if (pnlRecord.Visible == false)
                LoadData();

            nMode = 2;
            OpenControls(pnlRecord, true);
            OpenControls(tabComments, true);
            pnlSCExtData.Visible = false; pnlSlashExtData.Visible = false;
            pnlSCExtData.Enabled = true; pnlSlashExtData.Enabled = true;
            dtrLogTests.Enabled = true; cboQuotes.Enabled = true; dgvTests.ReadOnly = false; btnClose.Visible = false;
            btnSelQTests.Enabled = true; btnUnSelQTests.Enabled = true; btnSelAllTests.Enabled = true; btnTests.Enabled = true; btnCloseTests.Enabled = true;
            btnDataForm.Enabled = false; btnLSPreview.Enabled = false; btnLSPrinter.Enabled = false; btnFAXEMail.Enabled = false;
            if (txtSponsorID.Text == "130")
            {
                btnAddTest.Enabled = true; btnDelTest.Enabled = true;
            }
            else
            {
                btnAddTest.Enabled = false; btnDelTest.Enabled = false;
            }
            chkCancelled.Enabled = false;
            txtSSFormNo.Enabled = false;
        }

        private void LoadSamples()
        {
            dtSamples = PSSClass.Samples.LogSamplesData(txtCmpyCode.Text, Convert.ToInt32(txtLogNo.Text));
            bsSamples.DataSource = dtSamples;
            bnSamples.BindingSource = bsSamples;
            dgvSamples.DataSource = bsSamples;
        }

        private void LoadBillingRef()
        {
            dtBilling.Rows.Clear();
            dtBilling = PSSClass.Samples.LogBillingRef(txtCmpyCode.Text, Convert.ToInt32(txtLogNo.Text));

            bsBilling.DataSource = dtBilling;
            dgvTests.DataSource = bsBilling;
            dgvTests.Columns["UnitPrice"].Visible = false;
            dgvTests.Columns["RushPrice"].Visible = false;
            dgvTests.Columns["ControlNo"].Visible = false;
            dgvTests.Columns["CmpyCode"].Visible = false;
            dgvTests.Columns["QCmpyCode"].Visible = false;
        }

        //private void LoadSlashExtData()
        //{
        //    try
        //    {
        //        dtSlashExtData.Rows.Clear();
        //        DataTable dt = new DataTable();
        //        dt = PSSClass.Samples.ExSlashSCExtData(Convert.ToInt32(txtLogNo.Text));
        //        if (dt == null || dt.Rows.Count == 0)
        //            return;

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            DataRow dR = dtSlashExtData.NewRow();
        //            dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
        //            dR["SlashNo"] = dt.Rows[i]["SlashNo"];
        //            dR["SlashExtData1"] = dt.Rows[i]["SlashExtData1"];
        //            dR["SlashExtData2"] = dt.Rows[i]["SlashExtData2"];
        //            dR["SlashExtData3"] = dt.Rows[i]["SlashExtData3"];
        //            dR["SlashExtData4"] = dt.Rows[i]["SlashExtData4"];
        //            dR["SlashExtData5"] = dt.Rows[i]["SlashExtData5"];
        //            dR["SlashExtData6"] = dt.Rows[i]["SlashExtData6"];
        //            dR["SlashExtData7"] = dt.Rows[i]["SlashExtData7"];
        //            dR["SlashExtData8"] = dt.Rows[i]["SlashExtData8"];
        //            dR["SlashExtData9"] = dt.Rows[i]["SlashExtData9"];
        //            dR["SlashExtData10"] = dt.Rows[i]["SlashExtData10"];
        //            dR["SlashExtData11"] = dt.Rows[i]["SlashExtData11"];
        //            dR["SlashExtData12"] = dt.Rows[i]["SlashExtData12"];
        //            dR["SlashExtData13"] = dt.Rows[i]["SlashExtData13"];
        //            dtSlashExtData.Rows.Add(dR);
        //        }
        //        dtSlashExtData.AcceptChanges();
        //        bsSlashExtData.DataSource = dtSlashExtData;
        //        bnSlashExtData.BindingSource = bsSlashExtData;
        //    }
        //    catch { }
        //}

        private void LoadSlashExtData()
        {
            try
            {
                dtSlashExtData.Rows.Clear();
                DataTable dt = new DataTable();
                dt = PSSClass.Samples.ExSlashAddlData(Convert.ToInt32(txtLogNo.Text));
                if (dt == null || dt.Rows.Count == 0)
                    return;

                string strExtData = "";
                string[] arrExtData = new string[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        if (dt.Rows[i]["AddlData" + (j + 1)].ToString() != "")
                        {
                            strExtData = dt.Rows[i]["AddlData" + (j + 1)].ToString();
                            arrExtData = strExtData.Split(',');
                            DataRow dR = dtSlashExtData.NewRow();
                            dR["SlashNo"] = dt.Rows[i]["SlashNo"];
                            if (arrExtData[0] != "")
                            {
                                dR["ExtDataLabel"] = arrExtData[0];
                                dR["ExtDataValue"] = arrExtData[1];
                            }
                            else
                            {
                                dR["ExtDataLabel"] = "";
                                dR["ExtDataValue"] = "";
                            }
                            dtSlashExtData.Rows.Add(dR);
                        }
                    }
                }
                dtSlashExtData.AcceptChanges();
                bsSlashExtData.DataSource = dtSlashExtData;
            }
            catch { }
        }

        private void LoadSCExtData()
        {
            try
            {
                dtSCExtData.Rows.Clear();
                DataTable dt = new DataTable();
                dt = PSSClass.Samples.ExSCExtData(Convert.ToInt32(txtLogNo.Text));
                if (dt == null || dt.Rows.Count == 0)
                    return;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strExtData = "";
                    string[] arrExtData = new string[0];
                    int nSD = 0;
                    
                    if (dt.Rows[i]["StudyNo"] != null && dt.Rows[i]["StudyNo"].ToString() != "")
                        nSD = 1;

                    for (int j = 0; j < 10; j++)
                    {
                        if (dt.Rows[i]["SCExtData" + (j + 1).ToString()].ToString() != "")
                        {
                            strExtData = dt.Rows[i]["SCExtData" + (j + 1)].ToString();
                            nSD = 2;
                            break;
                        }
                    }
                    if (nSD == 1)
                    {
                        DataRow dR = dtSCExtData.NewRow();
                        dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                        dR["StudyNo"] = dt.Rows[i]["StudyNo"];
                        dR["StudyDirID"] = dt.Rows[i]["StudyDirID"];
                        dR["SCExtDataLabel"] = "";
                        dR["SCExtDataValue"] = "";
                        dR["PrtNotes"] = dt.Rows[i]["PrtNotes"];
                        dR["NonPrtNotes"] = dt.Rows[i]["NonPrtNotes"];
                        dtSCExtData.Rows.Add(dR);
                    }
                    else if (nSD == 2)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (dt.Rows[i]["SCExtData" + (j + 1)].ToString() != "")
                            {
                                strExtData = dt.Rows[i]["SCExtData" + (j + 1)].ToString();
                                arrExtData = strExtData.Split(',');
                                DataRow dR = dtSCExtData.NewRow();
                                dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                                dR["StudyNo"] = dt.Rows[i]["StudyNo"];
                                dR["StudyDirID"] = dt.Rows[i]["StudyDirID"];
                                if (arrExtData[0] != "")
                                {
                                    dR["SCExtDataLabel"] = arrExtData[0];
                                    dR["SCExtDataValue"] = arrExtData[1];
                                }
                                else
                                {
                                    dR["SCExtDataLabel"] = "";
                                    dR["SCExtDataValue"] = "";
                                }
                                dR["PrtNotes"] = dt.Rows[i]["PrtNotes"];
                                dR["NonPrtNotes"] = dt.Rows[i]["NonPrtNotes"];
                                dtSCExtData.Rows.Add(dR);
                            }
                        }
                    }
                }
                dtSCExtData.AcceptChanges();
                bsSCExtData.DataSource = dtSCExtData;
            }
            catch { }
        }

        private void SamplesAddlDataLabels()
        {
            DataTable dtAddl = new DataTable();
            dtAddl = PSSClass.Samples.SlashAddlLabels(Convert.ToInt16(txtSponsorID.Text));
            if (dtAddl == null || dtAddl.Rows.Count == 0)
            {
                return;
            }
            if (dtAddl.Rows[0]["Label3"].ToString() == "")
            {
                return;
            }
            dgvAddlData.Columns[0].HeaderText = "Slash No.";
            dgvAddlData.Columns[0].Width = 83;
            dgvAddlData.Columns[1].HeaderText = dtAddl.Rows[0]["Label3"].ToString();
            dgvAddlData.Columns[1].Width = 83;
            dgvAddlData.Columns[2].HeaderText = dtAddl.Rows[0]["Label4"].ToString();
            dgvAddlData.Columns[2].Width = 83;
            dgvAddlData.Columns[3].HeaderText = dtAddl.Rows[0]["Label5"].ToString();
            dgvAddlData.Columns[3].Width = 83;
            dgvAddlData.Columns[4].HeaderText = dtAddl.Rows[0]["Label6"].ToString();
            dgvAddlData.Columns[4].Width = 83;
            dgvAddlData.Columns[5].HeaderText = dtAddl.Rows[0]["Label7"].ToString();
            dgvAddlData.Columns[5].Width = 83;
            dgvAddlData.Columns[6].HeaderText = dtAddl.Rows[0]["Label8"].ToString();
            dgvAddlData.Columns[6].Width = 83;
            dgvAddlData.Columns[7].HeaderText = dtAddl.Rows[0]["Label9"].ToString();
            dgvAddlData.Columns[7].Width = 83;
            dgvAddlData.Columns[8].HeaderText = dtAddl.Rows[0]["Label10"].ToString();
            dgvAddlData.Columns[8].Width = 83;
            dgvAddlData.Columns[9].HeaderText = dtAddl.Rows[0]["Label11"].ToString();
            dgvAddlData.Columns[9].Width = 83;
            dgvAddlData.Columns[10].HeaderText = dtAddl.Rows[0]["Label12"].ToString();
            dgvAddlData.Columns[10].Width = 83;
            dgvAddlData.Columns[11].HeaderText = dtAddl.Rows[0]["Label13"].ToString();
            dgvAddlData.Columns[11].Width = 83;
            dgvAddlData.Columns[12].HeaderText = dtAddl.Rows[0]["Label14"].ToString();
            dgvAddlData.Columns[12].Width = 83;
            dgvAddlData.Columns[13].HeaderText = dtAddl.Rows[0]["Label15"].ToString();
            dgvAddlData.Columns[13].Width = 83;
            StandardDGVSetting(dgvAddlData);
        }

        private void DeleteRecord()
        {
            if (pnlRecord.Visible != true)
                LoadData();

            txtLogNo.Text = dgvFile.CurrentRow.Cells["PSSNo"].Value.ToString();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problen encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.Add(new SqlParameter("@LogNo", SqlDbType.Int));
                sqlcmd.Parameters["@LogNo"].Value = Convert.ToInt32(txtLogNo.Text);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelLogin";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            LoadRecords();
        }

        private void SaveRecord()
        {
            int nT = 0; byte nPO = 0;
            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                if (dtLogTests.Rows[i].RowState.ToString() != "Deleted")
                {
                    nT++;
                    if (dtLogTests.Rows[i]["PONo"].ToString().Trim() == "")
                    {
                        nPO = 1; break;
                    }
                }
            }
            if (nPO == 1 && txtSponsorID.Text != "130")
            {
                MessageBox.Show("Blank PO entry found. If PO is not applicable," + Environment.NewLine +
                                "please indicate N/A or CC for credit card.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (nT == 0)
            {
                MessageBox.Show("No test items entered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dgvSampleSC.Rows.Count == 0)
            {
                MessageBox.Show("No Service Code/Slash assignment found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //Validation Master Record
            if (txtSponsorID.Text == "")
            {
                MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSponsor.Focus();
                return;
            }
            if (txtContactID.Text == "")
            {
                MessageBox.Show("Please select Contact.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContact.Focus();
                return;
            }
            if (PSSClass.Contacts.ContactActive(Convert.ToInt16(txtContactID.Text)) == false)
            {
                MessageBox.Show("Contact had been marked INACTIVE." + Environment.NewLine + "Please select an active contact.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (bOSSF == 1)
                {
                    txtContact.Text = ""; txtContactID.Text = "0"; txtContact.Focus();
                    bOSSF = 0;
                }
                else
                {
                    txtContact.Focus(); return;
                }
            }
            if (txtArticle.Text.Trim() == "")
            {
                MessageBox.Show("Please enter article description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtArticle.Focus();
                return;
            }
            tabLogin.SelectedIndex = 1;
            if (txtGenDesc.Text.Trim() == "")
            {
                MessageBox.Show("Please enter sample description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                tabLogin.SelectedIndex = 1;
                txtGenDesc.Select();
                return;
            }
            if (rdoOther.Checked && txtOtherStorage.Text.Trim() == "")
            {
                MessageBox.Show("Please enter storage description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ((Button)dtrLogTests.CurrentItem.Controls["btnSC"]).Select();

            bsBilling.EndEdit();
            if (txtSponsorID.Text != "130")
            {
                byte nBill = 0;
                for (int i = 0; i < dtBilling.Rows.Count; i++)
                {
                    if (dtBilling.Rows[i].RowState.ToString() != "Deleted")
                    {
                        if (Convert.ToDecimal(dtBilling.Rows[i]["BillQty"]) > 0)
                        {
                            nBill = 1;
                        }
                    }
                }
                if (nBill == 0)
                {
                    MessageBox.Show("No billable quantity entered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            bsLogFM.EndEdit();
            if (nMode == 1 || dtLogFM.Rows[0].RowState.ToString() == "Modified")
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                
                //Log Master Record
                if (nMode == 1)
                {
                    //txtLogNo.Text = PSSClass.General.NewID("LogMaster", "PSSNo").ToString(); -- Revised 4/11/2016 to allow entry of GBL series 450K up Stability records
                    txtLogNo.Text = PSSClass.General.NewPSSNo("LogMaster", "PSSNo").ToString();
                    txtCmpyCode.Text = "P";
                }
                else
                    bsLogFM.EndEdit();

                //Other ID inclusion into the Gen Desc" 10/26/2017 AMDC
                if (chkOtherID.Checked == true && dgvSamples.Rows.Count > 0)
                {
                    string strOtherID = "", strSv = "";
                    for (int k = 0; k < dgvSamples.Rows.Count - 1; k++)
                    {
                        if (dgvSamples.Rows[k].Cells["OtherDesc2"].Value.ToString().Trim() != "" &&
                            dgvSamples.Rows[k].Cells["OtherDesc2"].Value.ToString().Trim() != strSv)
                        {
                            strSv = dgvSamples.Rows[k].Cells["OtherDesc2"].Value.ToString().Trim();
                            strOtherID += dgvSamples.Rows[k].Cells["OtherDesc2"].Value.ToString().Trim() + ",";
                        }
                    }
                    if (strOtherID != "")
                    {
                        if (txtGenDesc.Text.IndexOf("Other ID") == -1)
                            txtGenDesc.Text = txtGenDesc.Text + " Other ID: " + strOtherID.Substring(0, strOtherID.Length - 1);
                        else
                            txtGenDesc.Text = txtGenDesc.Text + ", " + strOtherID.Substring(0, strOtherID.Length - 1);
                    }
                }
                //End of Other ID inclusion
                sqlcmd.Parameters.AddWithValue("@nMode", nMode);
                sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                sqlcmd.Parameters.AddWithValue("@SampleNo", Convert.ToInt32(txtLogNo.Text));
                sqlcmd.Parameters.AddWithValue("@RecDte", dtpReceived.Value);
                sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
                sqlcmd.Parameters.AddWithValue("@ConID", Convert.ToInt16(txtContactID.Text));
                sqlcmd.Parameters.AddWithValue("@Article", txtArticle.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@CtlSub", chkCtrldSubs.CheckState);
                sqlcmd.Parameters.AddWithValue("@SampDesc", txtGenDesc.Text.Trim());
                if (txtAddlNotes.Text.Trim() == "")
                    sqlcmd.Parameters.AddWithValue("@AddlNotes", DBNull.Value);
                else
                    sqlcmd.Parameters.AddWithValue("@AddlNotes", txtAddlNotes.Text.Trim());
                if (txtSSFormNo.Text.Trim() == "")
                    sqlcmd.Parameters.AddWithValue("@SSNo", DBNull.Value);
                else
                    sqlcmd.Parameters.AddWithValue("@SSNo", Convert.ToInt32(txtSSFormNo.Text.Trim()));
                sqlcmd.Parameters.AddWithValue("@Rush", chkRush.CheckState);
                if (rdoSAmbient.Checked)
                    sqlcmd.Parameters.AddWithValue("@StorageCode", 1);
                else if (rdoRefrigerator.Checked)
                    sqlcmd.Parameters.AddWithValue("@StorageCode", 2);
                else if (rdoFreezer20.Checked)
                    sqlcmd.Parameters.AddWithValue("@StorageCode", 3);
                else if (rdoFreezer80.Checked)
                    sqlcmd.Parameters.AddWithValue("@StorageCode", 4);
                else if (rdoOther.Checked)
                    sqlcmd.Parameters.AddWithValue("@StorageCode", 5);
                else 
                    sqlcmd.Parameters.AddWithValue("@StorageCode", 1);//added 5/9/2016
                if (rdoOther.Checked)
                    sqlcmd.Parameters.AddWithValue("@StorageDesc", txtOtherStorage.Text.Trim());
                else
                    sqlcmd.Parameters.AddWithValue("@StorageDesc", DBNull.Value);
                if (rdoRAmbient.Checked)
                    sqlcmd.Parameters.AddWithValue("@ReceiptCode", 1);
                else if (rdoIcePack.Checked)
                    sqlcmd.Parameters.AddWithValue("@ReceiptCode", 2);
                else if (rdoDryIce.Checked)
                    sqlcmd.Parameters.AddWithValue("@ReceiptCode", 3);

                sqlcmd.Parameters.AddWithValue("@Locked", chkLocked.CheckState);

                sqlcmd.Parameters.Add(new SqlParameter("@AnaDone", SqlDbType.Bit));
                if (chkAnalyst.Checked == true)
                {
                    sqlcmd.Parameters["@AnaDone"].Value = true;
                }
                else
                {
                    sqlcmd.Parameters["@AnaDone"].Value = DBNull.Value;
                }

                sqlcmd.Parameters.Add(new SqlParameter("@MngrChecked", SqlDbType.Bit));
                if (chkManager.Checked)
                {
                    sqlcmd.Parameters["@MngrChecked"].Value = true;
                }
                else
                {
                    sqlcmd.Parameters["@MngrChecked"].Value = DBNull.Value;
                }
                if (chkCancelled.Checked)
                {
                    sqlcmd.Parameters.AddWithValue("@DteCancelled", mskDateCancelled.Text);
                }
                else
                    sqlcmd.Parameters.AddWithValue("@DteCancelled", DBNull.Value);

                sqlcmd.Parameters.AddWithValue("@RetestNo", 0);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditLogMstr";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException exSql)
                {
                    if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                    {
                        MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    InitializeFile();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    InitializeFile();
                    return;
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                if (nMode == 2)
                {
                    PSSClass.Samples.UpdRptSponsor(Convert.ToInt32(txtLogNo.Text), Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtContactID.Text), LogIn.nUserID);
                }
            }
            //SAMPLES DATA
            //Remove Deleted Records, if any
            DataTable dt = dtSamples.GetChanges(DataRowState.Deleted);
            if (dt != null && dt.Rows.Count > 0)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                    sqlcmd.Parameters.AddWithValue("@LogNo", txtLogNo.Text);
                    sqlcmd.Parameters.AddWithValue("@SlashNo", dt.Rows[i]["SlashNo",DataRowVersion.Original]);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spDelLogSlash";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException exSql)
                    {
                        if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                        {
                            MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        InitializeFile();
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        InitializeFile();
                        return;
                    }
                    sqlcmd.Dispose(); 
                }
                sqlcnn.Close(); sqlcnn.Dispose();
            }
            bsSamples.EndEdit();
            //Extended Slash Data
            if (pnlSlashExtData.Visible == true)
            {
                btnCloseSlashExt_Click(null, null);
            }
            bsSlashExtData.EndEdit();
            //Add/Update Records
            string strSampleXML = ""; string strX = "";
            if (dtSamples.Rows.Count > 0)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }
                for (int i = 0; i < dtSamples.Rows.Count; i++)
                {
                    strSampleXML = ""; strX = "";
                    if (dtSamples.Rows[i].RowState.ToString() != "Deleted" && (dtSamples.Rows[i].RowState.ToString() == "Added" || dtSamples.Rows[i].RowState.ToString() == "Modified" || nMode == 2))
                    {
                        if (dtSamples.Rows[i].RowState.ToString() == "Added")
                        {
                            object objMax;
                            objMax = dtSamples.Compute("Max(SlashID)", "");
                            int nMax = Convert.ToInt16(objMax) + 1;
                            dtSamples.Rows[i]["SlashID"] = nMax;
                        }

                        byte nM = 0;
                        strSampleXML = "<SamplesData>";
                        //Sample Additional XML Data
                        //Columns 1 (Sample No) and 2 (Sample Description) would be saved in a regular table column
                        strX = dtSamples.Rows[i]["OtherDesc1"].ToString().Trim().Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strSampleXML = strSampleXML + "<Value1>" + strX + "</Value1>"; 

                        strX = dtSamples.Rows[i]["OtherDesc2"].ToString().Trim().Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strSampleXML = strSampleXML + "<Value2>" + strX + "</Value2>";

                        //Columns 2 and 3 would be part of the Additional Data to be saved in the Extended Data XML column

                        if (dtSlashExtData.Rows.Count != 0)
                        {
                            int nV = 3;
                            for (int k = 0; k < dtSlashExtData.Rows.Count; k++)
                            {
                                if (dtSamples.Rows[i]["SlashNo"].ToString().Trim() == dtSlashExtData.Rows[k]["SlashNo"].ToString().Trim())
                                {
                                    try
                                    {
                                        if (dtSlashExtData.Rows[k]["ExtDataValue"] == null || dtSlashExtData.Rows[k]["ExtDataValue"].ToString().Trim() == "")
                                            strSampleXML = strSampleXML + "<Value" + (nV).ToString().Trim() + ">" + "" + "</Value" + (nV).ToString().Trim() + ">";
                                        else
                                        {
                                            //Add Condition (Date) for: 
                                            //BMT Sterilizer Load# and
                                            //Lytzen Oven Load# 
                                            string strSelectedOption = dtSlashExtData.Rows[k]["ExtDataLabel"].ToString().Trim().Replace("&", "&amp;");
                                            string strSelectedValue = dtSlashExtData.Rows[k]["ExtDataValue"].ToString().Trim().Replace("&", "&amp;");
                                            //string strRecordedDate = dateTimePicker1.Value.ToShortDateString();
                                            if (strSelectedOption == "BMT Sterilizer Load#" || strSelectedOption == "Lytzen Oven Load#")
                                            {
                                                strX = strSelectedOption + "," + strSelectedValue + "|";//Add Here DatePicker Value
                                            }
                                            else
                                            {
                                                strX = strSelectedOption + "," + strSelectedValue;
                                            }
                                            //strX = dtSlashExtData.Rows[k]["ExtDataLabel"].ToString().Trim().Replace("&", "&amp;") + "," + dtSlashExtData.Rows[k]["ExtDataValue"].ToString().Trim().Replace("&", "&amp;");
                                            strX = strX.Replace(">", "&gt;");
                                            strX = strX.Replace("<", "&lt;");
                                            strX = strX.Replace("'", "&apos;");
                                            strX = strX.Replace("\"", "&quot;");
                                            strSampleXML = strSampleXML + "<Value" + (nV).ToString().Trim() + ">" + strX + "</Value" + (nV).ToString().Trim() + ">";
                                        }
                                        nV++;
                                    }
                                    catch { }
                                }
                            }
                        }
                        strSampleXML = strSampleXML + "</SamplesData>";

                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        if (nM == 1 || dtSamples.Rows[i].RowState.ToString() == "Added")
                            sqlcmd.Parameters.AddWithValue("@nMode", 1);
                        else
                            sqlcmd.Parameters.AddWithValue("@nMode", 2);
                        sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                        sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                        sqlcmd.Parameters.AddWithValue("@SlashID", dtSamples.Rows[i]["SlashID"]);
                        sqlcmd.Parameters.AddWithValue("@SlashNo", dtSamples.Rows[i]["SlashNo"].ToString());
                        sqlcmd.Parameters.AddWithValue("@SampleDesc", dtSamples.Rows[i]["SampleDesc"].ToString());
                        if (strSampleXML == "")
                            sqlcmd.Parameters.AddWithValue("@AddlData", "<SamplesData></SamplesData>");
                        else
                            sqlcmd.Parameters.AddWithValue("@AddlData", strSampleXML);
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spAddEditLogSample";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (System.Data.SqlClient.SqlException exSql)
                        {
                            if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                            {
                                MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                                InitializeFile();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                            InitializeFile();
                            return;
                        }
                        sqlcmd.Dispose(); 
                    }
                }
                sqlcnn.Close(); sqlcnn.Dispose();
            }
            //LOG TESTS
            //Remove Deleted Records, if any
            if (nListDelSC.Count > 0)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }
                //Recall Deleted Tests if re-added into the list
                for (int i = 0; i < dtLogTests.Rows.Count; i++)
                {
                    if (dtLogTests.Rows[i].RowState.ToString() != "Deleted")
                    {
                        for (int j = 0; j < nListDelSC.Count; j++)
                        {
                            if (dtLogTests.Rows[i]["ServiceCode"].ToString() == nListDelSC[j].ToString())
                            {
                                nListDelSC.RemoveAt(j);
                            }
                        }
                    }
                }
                for (int i = 0; i < nListDelSC.Count; i++)
                {
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                    sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                    sqlcmd.Parameters.AddWithValue("@SC", nListDelSC[i]);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spDelLogTest";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException exSql)
                    {
                        if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                        {
                            MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                            InitializeFile();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        InitializeFile();
                        return;
                    }
                    sqlcmd.Dispose(); 
                }
                sqlcnn.Close(); sqlcnn.Dispose();
            }
            bsLogTests.EndEdit();
            //Add/Update Records
            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                if (dtLogTests.Rows[i].RowState.ToString() == "Added" || dtLogTests.Rows[i].RowState.ToString() == "Modified")
                {
                    try
                    {
                        if (dtLogTests.Rows[i].RowState.ToString() == "Added")
                            nMode = 1;
                        else
                            nMode = 2;
                        SaveLogTests(i, nMode);      
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        if (ex.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                        {
                            try
                            {
                                SaveLogTests(i, 2);
                            }
                            catch
                            { }
                        }
                    }
                }
            }
            //Extended SC Data
            if (pnlSCExtData.Visible == true)
            {
                btnCloseExtSC_Click(null, null);
            }
            bsSCExtData.EndEdit();
            //DataTable dtX = dtSCExtData.GetChanges();
            if (dtSCExtData != null && dtSCExtData.Rows.Count > 0)
            {
                if (dtLogTests != null && dtLogTests.Rows.Count > 0)
                {
                    SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                    if (sqlcnn == null)
                    {
                        MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        InitializeFile();
                        return;
                    }
                    for (int i = 0; i < dtLogTests.Rows.Count; i++)
                    {
                        string strSCExData = "<SCExtData>";
                        string strPrint = "", strNPrint = "", strSN = "", strSD = "";

                        DataRow[] foundRows = dtSCExtData.Select("ServiceCode = " + dtLogTests.Rows[i]["ServiceCode"]);//, "ServiceCode ASC"
                        if (foundRows.Length > 0)
                        {
                            DataTable dtSorted = foundRows.CopyToDataTable();

                            int nV = 1;

                            for (int j = 0; j < dtSorted.Rows.Count; j++)
                            {
                                if (dtSorted.Rows[j].RowState.ToString() != "Deleted" && dtLogTests.Rows[i]["ServiceCode"].ToString() == dtSorted.Rows[j]["ServiceCode"].ToString())//dgvSCExtData.Rows[j].Cells["SC"].Value.ToString()
                                {
                                    if (dtSorted.Rows[j]["SCExtDataLabel"].ToString().Trim() != "")
                                    {
                                        strSCExData = strSCExData + "<Value" + (nV).ToString().Trim() + ">" + dtSorted.Rows[j]["SCExtDataLabel"].ToString() + "," + dtSorted.Rows[j]["SCExtDataValue"].ToString() +
                                                      "</Value" + (nV).ToString().Trim() + ">";
                                    }
                                    nV++;
                                    strPrint = dtSorted.Rows[j]["PrtNotes"].ToString();
                                    strNPrint = dtSorted.Rows[j]["NonPrtNotes"].ToString();
                                    if (dtSorted.Rows[j]["StudyNo"].ToString() != "0")
                                        strSN = dtSorted.Rows[j]["StudyNo"].ToString();
                                    if (dtSorted.Rows[j]["StudyDirID"].ToString() != "0")
                                        strSD = dtSorted.Rows[j]["StudyDirID"].ToString();
                                }
                            }
                        }
                        if (strPrint != "")
                            strSCExData = strSCExData + "<PrintingNotes>" + strPrint + "</PrintingNotes>";
                        else
                            strSCExData = strSCExData + "<PrintingNotes></PrintingNotes>";
                        if (strNPrint != "")
                            strSCExData = strSCExData + "<NonPrintingNotes>" + strNPrint + "</NonPrintingNotes>";
                        else
                            strSCExData = strSCExData + "<NonPrintingNotes></NonPrintingNotes>";
                        strSCExData = strSCExData + "</SCExtData>";

                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spUpdSCExtData";
                        sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                        sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                        sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dtLogTests.Rows[i]["ServiceCode"].ToString()));
                        if (strSN != "")
                            sqlcmd.Parameters.AddWithValue("@StudyNo", Convert.ToInt32(strSN));
                        else
                            sqlcmd.Parameters.AddWithValue("@StudyNo", DBNull.Value);
                        if (strSD != "")
                            sqlcmd.Parameters.AddWithValue("@StudyDirID", Convert.ToInt16(strSD));
                        else
                            sqlcmd.Parameters.AddWithValue("@StudyDirID", DBNull.Value);
                        sqlcmd.Parameters.AddWithValue("@SCExt", strSCExData);
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (System.Data.SqlClient.SqlException exSql)
                        {
                            if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                            {
                                MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                                InitializeFile();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                            InitializeFile();
                            return;
                        }
                        sqlcmd.Dispose();
                    }
                    sqlcnn.Close(); sqlcnn.Dispose();
                }
            }
            // SC/Slash 
            //Remove Deleted Records, if any
            dt = dtSampleSC.GetChanges(DataRowState.Deleted);
            if (dt != null && dt.Rows.Count > 0)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                    sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                    sqlcmd.Parameters.AddWithValue("@SlashNo", dt.Rows[i]["Slash",DataRowVersion.Original]);
                    sqlcmd.Parameters.AddWithValue("@SC", dt.Rows[i]["SC", DataRowVersion.Original]);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spDelLogSlashSC";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException exSql)
                    {
                        if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                        {
                            MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                            InitializeFile();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        InitializeFile();
                        return;
                    }
                    sqlcmd.Dispose();
                }
                sqlcnn.Close(); sqlcnn.Dispose();
            }
            //Add/Update Records
            bsSampleSC.EndEdit();
            if (dtSampleSC.Rows.Count > 0)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }

                for (int i = 0; i < dtSampleSC.Rows.Count; i++)
                {
                    if (dtSampleSC.Rows[i].RowState.ToString() == "Added" || dtSampleSC.Rows[i].RowState.ToString() == "Modified")
                    {
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;
                        if (dtSampleSC.Rows[i].RowState.ToString() == "Added")
                            sqlcmd.Parameters.AddWithValue("@nMode", 1);
                        else
                            sqlcmd.Parameters.AddWithValue("@nMode", 2);
                        sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                        sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                        sqlcmd.Parameters.AddWithValue("@SlashNo", dtSampleSC.Rows[i]["Slash"]);
                        sqlcmd.Parameters.AddWithValue("@SC", dtSampleSC.Rows[i]["SC"]);
                        sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
                        sqlcmd.Parameters.AddWithValue("@TestResults", "<TestData></TestData>");
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spAddEditSlashSC";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (System.Data.SqlClient.SqlException exSql)
                        {
                            if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                            {
                                MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                                InitializeFile();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                            InitializeFile();
                            return;
                        }
                        sqlcmd.Dispose();
                    }
                }
                sqlcnn.Close(); sqlcnn.Dispose();
            }
            // Billing Reference Data
            bsBilling.EndEdit();
            dt = dtBilling.GetChanges();
            int n = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }
                n = PSSClass.Samples.DelLogBillingRef(txtCmpyCode.Text, Convert.ToInt32(txtLogNo.Text));
                for (int i = 0; i < dgvTests.Rows.Count; i++)
                {
                    if (dgvTests.Rows[i].Cells["BillQty"].Value != null && dgvTests.Rows[i].Cells["BillQty"].Value.ToString() != "0")
                    {
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        string strQ = dgvTests.Rows[i].Cells["QuoteNo"].Value.ToString();
                        int nI = strQ.IndexOf("R");
                        string strQNo = strQ.Substring(0, nI - 1);
                        string strRNo = strQ.Substring((nI + 1), strQ.Length - (nI + 1));

                        sqlcmd.Parameters.AddWithValue("@nMode", 1);
                        sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                        sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                        sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dgvTests.Rows[i].Cells["ServiceCode"].Value));
                        sqlcmd.Parameters.AddWithValue("@QuoteNo", strQNo);
                        sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
                        sqlcmd.Parameters.AddWithValue("@ControlNo", Convert.ToInt16(dgvTests.Rows[i].Cells["ControlNo"].Value));
                        sqlcmd.Parameters.AddWithValue("@BillQty", Convert.ToInt16(dgvTests.Rows[i].Cells["BillQty"].Value));
                        if (dgvTests.Rows[i].Cells["UnitPrice"].Value != DBNull.Value && dgvTests.Rows[i].Cells["UnitPrice"].Value.ToString() != "0")
                            sqlcmd.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(dgvTests.Rows[i].Cells["UnitPrice"].Value));
                        else
                            sqlcmd.Parameters.AddWithValue("@UnitPrice", DBNull.Value);
                        if (dgvTests.Rows[i].Cells["Rush"].Value.ToString() != "False")
                            sqlcmd.Parameters.AddWithValue("@Rush", dtBilling.Rows[i]["Rush"]);
                        else
                            sqlcmd.Parameters.AddWithValue("@Rush", DBNull.Value);
                        if (dgvTests.Rows[i].Cells["RushPrice"].Value != DBNull.Value && dgvTests.Rows[i].Cells["RushPrice"].Value.ToString() != "0")
                            sqlcmd.Parameters.AddWithValue("@RushPrice", Convert.ToDecimal(dgvTests.Rows[i].Cells["RushPrice"].Value.ToString()));
                        else
                            sqlcmd.Parameters.AddWithValue("@RushPrice", DBNull.Value);
                        sqlcmd.Parameters.AddWithValue("@QCmpyCode", dgvTests.Rows[i].Cells["QCmpyCode"].Value.ToString());
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spAddEditBillRef";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (System.Data.SqlClient.SqlException exSql)
                        {
                            if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                            {
                                MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                                InitializeFile();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                            InitializeFile();
                            return;
                        }
                        sqlcmd.Dispose(); 
                    }
                }
                sqlcnn.Close(); sqlcnn.Dispose();
            }
            //Inserts Record to Sample Tracking Database - TestStatus Table 10/10/2017
            PSSClass.Samples.AddTestStatus(Convert.ToInt32(txtLogNo.Text));

            if (nFR == 1)
            {
                nFR = 0; nMode = 0;
                SendKeys.Send("{F12}");
                return;
            }
            InitializeFile();
        }

        private void InitializeFile()
        {
            AddEditMode(false);
            
            LoadRecords();            
            bsFile.Filter = "PSSNo<>0";

            if (txtLogNo.Text != "(New)")
                PSSClass.General.FindRecord("PSSNo", txtLogNo.Text, bsFile, dgvFile);

            btnClose.Visible = true;
            btnLSPreview.Enabled = true; btnLSPrinter.Enabled = true; btnDataForm.Enabled = true;
            btnAddSample.Enabled = true; btnEditSample.Enabled = true; btnDelSample.Enabled = true; btnSaveSample.Enabled = false; btnCancelSample.Enabled = false;
            LoadData();
        }

        private void SaveLogTests(int cI, int cMode)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
            sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
            sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dtLogTests.Rows[cI]["ServiceCode"].ToString()));
            if (dtLogTests.Rows[cI]["ProtocolNo"].ToString() != "")
                sqlcmd.Parameters.AddWithValue("@ProtNo", dtLogTests.Rows[cI]["ProtocolNo"].ToString());
            else
                sqlcmd.Parameters.AddWithValue("@ProtNo", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@StartDte", Convert.ToDateTime(dtLogTests.Rows[cI]["StartDate"].ToString()));
            sqlcmd.Parameters.AddWithValue("@EndDte", Convert.ToDateTime(dtLogTests.Rows[cI]["EndDate"].ToString()));

            if (dtLogTests.Rows[cI]["QuotationNo"].ToString() == "")
                sqlcmd.Parameters.AddWithValue("@QuoteNo", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@QuoteNo", dtLogTests.Rows[cI]["QuotationNo"].ToString());

            if (dtLogTests.Rows[cI]["PONo"].ToString() == "")
                sqlcmd.Parameters.AddWithValue("@PONo", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@PONo", dtLogTests.Rows[cI]["PONo"].ToString());
            sqlcmd.Parameters.AddWithValue("@Samples", dtLogTests.Rows[cI]["TestSamples"].ToString());
            sqlcmd.Parameters.AddWithValue("@Slashes", dtLogTests.Rows[cI]["Slashes"].ToString());
            sqlcmd.Parameters.AddWithValue("@BookNo", dtLogTests.Rows[cI]["BookNo"].ToString());
            sqlcmd.Parameters.AddWithValue("@BillQty", Convert.ToInt16(dtLogTests.Rows[cI]["BillQty"].ToString()));
            if (dtLogTests.Rows[cI]["EC"].ToString() == "True")
                sqlcmd.Parameters.AddWithValue("@EC", true);
            else
                sqlcmd.Parameters.AddWithValue("@EC", DBNull.Value);
            if (dtLogTests.Rows[cI]["ECCompType"] != null && dtLogTests.Rows[cI]["ECCompType"].ToString().Trim() != "")
            {
                if (dtLogTests.Rows[cI]["ECCompType"].ToString() == "1")
                    sqlcmd.Parameters.AddWithValue("@ECType", "D");
                else
                    sqlcmd.Parameters.AddWithValue("@ECType", dtLogTests.Rows[cI]["ECCompType"].ToString());
            }
            else
                sqlcmd.Parameters.AddWithValue("@ECType", DBNull.Value);
            if (dtLogTests.Rows[cI]["ECLength"] != null && dtLogTests.Rows[cI]["ECLength"].ToString().Trim() != "")
                sqlcmd.Parameters.AddWithValue("@ECLen", Convert.ToInt16(dtLogTests.Rows[cI]["ECLength"].ToString()));
            else
                sqlcmd.Parameters.AddWithValue("@ECLen", DBNull.Value);
            if (dtLogTests.Rows[cI]["ECEndDate"] != null && dtLogTests.Rows[cI]["ECEndDate"].ToString().Trim() != "")
                sqlcmd.Parameters.AddWithValue("@ECEndDte", Convert.ToDateTime(dtLogTests.Rows[cI]["ECEndDate"].ToString()));
            else
                sqlcmd.Parameters.AddWithValue("@ECEndDte", DBNull.Value);
            if (dtLogTests.Rows[cI]["DateSampled"] != null && dtLogTests.Rows[cI]["DateSampled"].ToString().Trim() != "")
                sqlcmd.Parameters.AddWithValue("@DteSampled", Convert.ToDateTime(dtLogTests.Rows[cI]["DateSampled"].ToString()));
            else
                sqlcmd.Parameters.AddWithValue("@DteSampled", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@ExtData", "<SCExtData></SCExtData>");
            sqlcmd.Parameters.AddWithValue("@AddlNotes", dtLogTests.Rows[cI]["AddlNotes"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@QCmpyCode", dtLogTests.Rows[cI]["QCompanyCode"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditLogTest";
            sqlcmd.ExecuteNonQuery();
        }

        private void CancelSave()
        {
            if (nFR == 1)
            {
                nFR = 0;
                SendKeys.Send("{F12}");
                return;
            }
            if (nMode != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Are you sure you want to cancel?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            ClearControls(pnlRecord); 
            ClearControls(tabComments);
            AddEditMode(false); 
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            pnlSCExtData.Visible = false; pnlSlashExtData.Visible = false;
            pnlQuotes.Visible = false;
            strList.Clear(); strListSC.Clear(); strListQ.Clear(); strListCmpy.Clear(); nQ = 0; nQu = 0;
            try
            {
                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() != "" || dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "")
                {
                    tsbEdit.Enabled = false; btnFAXEMail.Enabled = false;
                }
            }
            catch { }
        }
       
        private void picSponsors_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadSponsorsDDL();
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSponsors.Top = 75;
                dgvContacts.Visible = false; dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false; 
            }
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void picContacts_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    dgvContacts.Visible = true; dgvContacts.BringToFront(); dgvContacts.Top = 98;
                    dgvSponsors.Visible = false; dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false;
                }
                catch { }
            }
        }

        private void LoadQuotes()
        {
            DataTable dt = new DataTable();
            if (txtSponsor.Text.IndexOf("INGREDION") != -1)
            {
                dt.Columns.Add("QuotationNo", typeof(string));
                DataRow row = dt.NewRow();
                row["QuotationNo"] = "-select-";
                dt.Rows.InsertAt(row, 0);

                //row = dt.NewRow();
                //row["QuotationNo"] = "2016.1433"; // added 9-9-2016
                //dt.Rows.InsertAt(row, 1);

                row = dt.NewRow();
                row["QuotationNo"] = "2017.1439"; // replacement for 2016.1433 added 11-21-2017
                dt.Rows.InsertAt(row, 1);

                row = dt.NewRow();
                row["QuotationNo"] = "2015.1738";
                dt.Rows.InsertAt(row, 2);

                row = dt.NewRow();
                row["QuotationNo"] = "2015.1674";
                dt.Rows.InsertAt(row, 3);

                row = dt.NewRow();
                row["QuotationNo"] = "2017.1223"; //07-21-2017
                dt.Rows.InsertAt(row, 4);

                //row = dt.NewRow();
                //row["QuotationNo"] = "2017.1223"; //07-21-2017
                //dt.Rows.InsertAt(row, 4);

                row = dt.NewRow();
                row["QuotationNo"] = "2017.0567"; // added 3/31/2017
                dt.Rows.InsertAt(row, 5);
                //row = dt.NewRow();
                //row["QuotationNo"] = "2015.1403";// disabled 9-9-2016
                //dt.Rows.InsertAt(row, 2);

                row = dt.NewRow();
                row["QuotationNo"] = "2018.0055"; // added 3/14/2018
                dt.Rows.InsertAt(row, 6);

                cboQuotes.DisplayMember = "QuotationNo";
                cboQuotes.ValueMember = "QuotationNo";
            }
            else
            {
                dt = PSSClass.Quotations.LoadQuotes(Convert.ToInt16(txtSponsorID.Text));
                cboQuotes.DisplayMember = "QuotationNo";
                cboQuotes.ValueMember = "CompanyCode";

                DataRow row = dt.NewRow();
                row["QuotationNo"] = "-select-";
                row["CompanyCode"] = "P";
                dt.Rows.InsertAt(row, 0);
            }
            cboQuotes.DataSource = dt;
            cboQuotes.Refresh();
            pnlQuotes.Visible = true;
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                cboQuotes.DataSource = null; dgvPONo.DataSource = null; 
                LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                LoadPO();
                if (txtSponsorID.Text == "130")
                {
                    btnAddTest.Enabled = true; btnDelTest.Enabled = true;
                }
                else
                {
                    btnAddTest.Enabled = false; btnDelTest.Enabled = false;
                }
                dgvSponsors.Visible = false; txtContact.Text = ""; txtContactID.Text = "";
                strList.Clear(); strListQ.Clear(); strListSC.Clear(); strListCmpy.Clear();
                
                SetUpSlashLabels();
                SamplesAddlDataLabels();
                if (dtLogTests.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLogTests.Rows.Count; i++)
                    {
                        dtLogTests.Rows[i]["QuotationNo"] = "";
                        dtLogTests.Rows[i]["PONo"] = "";
                    }
                }

                if (dtBilling.Rows.Count > 0)
                {
                    for (int i = dtBilling.Rows.Count - 1; i >= 0; i--)
                    {
                        dtBilling.Rows.RemoveAt(i);
                    }
                }
            }
            else
            {
                txtSponsor.Text = ""; dgvSponsors.Visible = false; txtContactID.Text = ""; txtContact.Text = ""; dgvContacts.Visible = false;
            }
        }
        private void LoadContactsDDL(int cSpID)
        {
            dgvContacts.DataSource = null;

            dtContacts = PSSClass.Sponsors.ContactsDDL(cSpID);
            if (dtContacts == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvContacts.DataSource = dtContacts;
            StandardDGVSetting(dgvContacts);
            dgvContacts.Columns[0].Width = 369;
            dgvContacts.Columns[1].Visible = false;

            //AMDC
            dgvArticleDesc.DataSource = null;
            dtArticleDesc = PSSClass.Samples.GetArticleDesc(Convert.ToInt16(txtSponsorID.Text));
            if (dtArticleDesc == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvArticleDesc.DataSource = dtArticleDesc;
            dgvArticleDesc.Columns[0].Width = 442;
            StandardDGVSetting(dgvArticleDesc);
        }

        //private void txtSponsorIDEnterHandler(object sender, EventArgs e)
        //{
        //    dgvSponsors.Visible = false;
        //}

        //private void txtContactIDEnterHandler(object sender, EventArgs e)
        //{
        //    dgvContacts.Visible = false;
        //}

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSponsors;
                dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvSponsors, dvwSponsors);
            }
        }

        //private void txtContactEnterHandler(object sender, EventArgs e)
        //{
        //    if (nMode != 0)
        //    {
        //        try
        //        {
        //            LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
        //            dgvContacts.Visible = true; dgvContacts.BringToFront();
        //            dgvSponsors.Visible = false; dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false;
        //        }
        //        catch { }
        //    }
        //}

        private void txtContactID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                    try
                    {
                        txtContact.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
                    }
                    catch { }
                else
                {
                    txtContact.Text = ""; dgvContacts.Visible = false;
                }
            }
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtILPO.Text = ""; txtILSlash.Text = ""; txtFillCode.Text = ""; txtILBookNo.Text = "828";

            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
            cboQuotes.DataSource = null; dgvPONo.DataSource = null;
            LoadPO();
            
            if (pnlQuotes.Visible == true)
                LoadQuotes();

            if (txtSponsorID.Text == "130")
            {
                btnAddTest.Enabled = true; btnDelTest.Enabled = true;
            }
            else
            {
                btnAddTest.Enabled = false; btnDelTest.Enabled = false;
            }
            dgvSponsors.Visible = false; txtContact.Text = ""; txtContactID.Text = "";
            strList.Clear(); strListQ.Clear(); strListSC.Clear(); strListCmpy.Clear();
         
            SetUpSlashLabels();
            SamplesAddlDataLabels();
            if (dtLogTests.Rows.Count > 0)
            {
                for (int i = 0; i < dtLogTests.Rows.Count; i++)
                {
                    if (dtLogTests.Rows[i].RowState.ToString() != "Deleted")
                    {
                        dtLogTests.Rows[i]["QuotationNo"] = "";
                        dtLogTests.Rows[i]["PONo"] = "";
                    }
                }
            }
            if (dtBilling.Rows.Count > 0)
            {
                for (int i = dtBilling.Rows.Count - 1; i >= 0; i--)
                {
                    dtBilling.Rows.RemoveAt(i);
                }
            }

            if (txtSponsorID.Text == "130") //GIBRALTAR LABORATORIES
            {
                btnAddTest.Enabled = true; btnDelTest.Enabled = true;
            }
            else
            {
                btnAddTest.Enabled = false; btnDelTest.Enabled = false;
            }
        }

        private void dgvSponsors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSponsors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtILPO.Text = ""; txtILSlash.Text = ""; txtFillCode.Text = ""; txtILBookNo.Text = "828";
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                cboQuotes.DataSource = null; dgvPONo.DataSource = null; 
                LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                LoadPO();
                if (txtSponsorID.Text == "130")
                {
                    btnAddTest.Enabled = true; btnDelTest.Enabled = true;
                }
                else
                {
                    btnAddTest.Enabled = false; btnDelTest.Enabled = false;
                }
                dgvSponsors.Visible = false; txtContact.Text = ""; txtContactID.Text = "";
                strList.Clear(); strListQ.Clear(); strListSC.Clear(); strListCmpy.Clear();
                
                SetUpSlashLabels();
                SamplesAddlDataLabels();
                if (dtLogTests.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLogTests.Rows.Count; i++)
                    {
                        dtLogTests.Rows[i]["QuotationNo"] = "";
                        dtLogTests.Rows[i]["PONo"] = "";
                    }
                }

                if (dtBilling.Rows.Count > 0)
                {
                    for (int i = dtBilling.Rows.Count - 1; i >= 0; i--)
                    {
                        dtBilling.Rows.RemoveAt(i);
                    }
                }
            }
            else if (e.KeyChar == 27)
            {
                dgvSponsors.Visible = false;
            }
        }

        private void dgvContacts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvContacts_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
                txtContactID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
                dgvContacts.Visible = false; dgvSamples.Select(); //amdc
            }
            else if (e.KeyChar == 27)
            {
                dgvContacts.Visible = false;
            }
        }

        private void dgvContacts_DoubleClick(object sender, EventArgs e)
        {
            txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
            txtContactID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
            dgvContacts.Visible = false; dgvSamples.Select(); //amdc
        }

        private void btnTests_Click(object sender, EventArgs e)
        {
            if (dgvTests.Rows.Count == 0)
                return;

            List<string> strLSC = new List<string>();
            int nSC = 0; //identifies existing SC in the Log Tests table

            if (nQ == 1)//a test item is selected from Quote
            {
                if (dtLogTests.Rows.Count > 0) //if Log Tests table has rows
                {
                    for (int i = 0; i < dtLogTests.Rows.Count; i++) //iterate through the rows
                    {
                        if (dtLogTests.Rows[i].RowState.ToString() != "Deleted")
                        {
                            for (int j = 0; j < dtBilling.Rows.Count; j++) //iterate through the tests datagridrow dgvTests.Rows.Count
                            {
                                if (dtBilling.Rows[j].RowState.ToString() != "Deleted" && dtLogTests.Rows[i]["ServiceCode"].ToString() == dtBilling.Rows[j]["ServiceCode"].ToString() && dtBilling.Rows[j]["SelectedTest"].ToString() == "True")
                                {
                                    nSC = 1;  //mark if existing
                                    break;
                                }
                            }
                            if (nSC == 0) //if SC does not exist and flag is for Quote entry 
                            {
                                strLSC.Add(dtLogTests.Rows[i]["ServiceCode"].ToString()); //add the SC to the list
                            }
                            nSC = 0; //initialize and proceed with the iteration
                        }
                    }
                }
                nQ = 0;
            }

            if (strLSC.Count > 0) //if SC list has elements
            {
                for (int i = 0; i < strLSC.Count ; i++)
                {
                    foreach (DataRow row in dtLogTests.Rows)
                    {
                        if (row.RowState.ToString()!= "Deleted" && strLSC[i].ToString() == row["ServiceCode"].ToString())
                        {
                            row.Delete(); //delete row to make sure that only those that are selected from the quote would be in the list

                            //???
                            if (nMode == 2)
                                nListDelSC.Add(Convert.ToInt16(strLSC[i]));

                            break;
                        }
                    }
                    strList.RemoveAll(item => item != strListSC[i]); //remove all SCs that are not in the Master SC List
                }
            }
           
            //populate the SC data source of the Samples/SC datagridview
            dtSCMaster.Rows.Clear(); //For Sample/SC data entry

            int nT = 0;
             
            for (int i = 0; i < dtBilling.Rows.Count; i++)
            {
                if (dtBilling.Rows[i].RowState.ToString() != "Deleted" && dtBilling.Rows[i]["SelectedTest"].ToString() == "True") 
                {
                    strList.Add(dtBilling.Rows[i]["ServiceCode"].ToString() + "-" + dtBilling.Rows[i]["QuoteNo"].ToString().Trim() + "*" + dtBilling.Rows[i]["QCmpyCode"].ToString());
                    nT += 1;
                }
            }

            if (strList.Count == 0) //no items selected, exit from this routine
            {
                nQu = 0;
                return;
            }

            DataRow dr;
            strList.Sort();//sort the Master SC List
            strListSC.Clear(); strListQ.Clear(); strListCmpy.Clear(); //initialize lists for SC and Quotes

            int n = 0, nCmpy = 0;
            int idx = 0;
            string strSave = "";           
            for (int i = 0; i < strList.Count; i++)
            {
                n = strList[i].IndexOf("-");
                nCmpy = strList[i].IndexOf("*");
                if (strList[i].Substring(0, n) != strSave)
                {
                    strSave = strList[i].Substring(0, n);
                    dr = dtSCMaster.NewRow();
                    dr["ServiceCode"] = strSave;
                    dtSCMaster.Rows.Add(dr);
                    strListSC.Add(strSave);
                    strListQ.Add(strList[i].Substring(n + 1, strList[i].Length - (n + 3)));
                    strListCmpy.Add(strList[i].Substring(nCmpy + 1, strList[i].Length - (nCmpy + 1)));
                }
            }
            idx = strList.Count - 1;
            if (strList[idx].Substring(0, n) != strSave)
            {
                strSave = strList[idx].Substring(0, n);
                dr = dtSCMaster.NewRow();
                dr["ServiceCode"] = strSave;
                dtSCMaster.Rows.Add(dr);
                strListSC.Add(strSave);
                strListQ.Add(strList[idx].Substring(n + 1, strList[idx].Length - (n + 3)));
                strListCmpy.Add(strList[idx].Substring(nCmpy + 1, strList[idx].Length - (nCmpy + 1)));
            }

            bsSCDDL.DataSource = dtSCMaster;
            string strTestDesc = "";
            nT = strListSC.Count;
            chkRush.Checked = false;

            for (int i = 0; i < nT; i++)
            {
                decimal nQty = 0;
                DataRow[] foundRows;
                foundRows = dtLogTests.Select("ServiceCode = " +  strListSC[i].ToString());
                if (foundRows.Length == 0)
                {
                    int nDuration = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(strListSC[i]));
                    //Tests to be done
                    strTestDesc = "";
                    for (int j = 0; j < dtBilling.Rows.Count; j++)
                    {
                        if (dtBilling.Rows[j].RowState.ToString() != "Deleted" && strListSC[i].ToString() == dtBilling.Rows[j]["ServiceCode"].ToString() && dtBilling.Rows[j]["SelectedTest"].ToString() == "True")
                        {
                            strTestDesc = strTestDesc + dtBilling.Rows[j]["TestDesc1"].ToString() + ", " + "Bill Qty: " + dtBilling.Rows[j]["BillQty"].ToString() + ", " + Environment.NewLine;
                            nQty = nQty + Convert.ToDecimal(dtBilling.Rows[j]["BillQty"]);
                            if (Convert.ToBoolean(dtBilling.Rows[j]["Rush"]) == true)
                                chkRush.Checked = true;
                        }
                    }
                    strTestDesc = strTestDesc.Trim();
                    dr = dtLogTests.NewRow();
                    dr["ServiceCode"] = Convert.ToInt16(strListSC[i]);
                    dr["ServiceDesc"] = "";
                    dr["ProtocolNo"] = "";
                    dr["StartDate"] = DateTime.Now;
                    dr["EndDate"] = DateTime.Now.AddDays(nDuration);
                    dr["QuotationNo"] = strListQ[i];
                    dr["BillQty"] = nQty;
                    dr["TestSamples"] = "1";
                    dr["Slashes"] = "";
                    dr["PONo"] = "N/A";
                    dr["BookNo"] = 0;
                    dr["EC"] = false;
                    dr["ECCompType"] = DBNull.Value;
                    dr["ECLength"] = DBNull.Value;
                    dr["ECEndDate"] = DBNull.Value;
                    dr["DateSampled"] = DBNull.Value;
                    dr["QuoteFlag"] = "1";
                    dr["ReportNo"] = 0;
                    dr["AddlNotes"] = strTestDesc.Substring(0, strTestDesc.Length - 1);
                    dr["QCompanyCode"] = strListCmpy[i];
                    dtLogTests.Rows.Add(dr);
                }
                else
                {
                    //Tests to be done
                    strTestDesc = "";
                    for (int j = 0; j < dtBilling.Rows.Count; j++)
                    {
                        if (dtBilling.Rows[j].RowState.ToString() != "Deleted" && strListSC[i].ToString() == dtBilling.Rows[j]["ServiceCode"].ToString() && dtBilling.Rows[j]["SelectedTest"].ToString() == "True")
                        {
                            strTestDesc = strTestDesc + dtBilling.Rows[j]["TestDesc1"].ToString() + ", " + "Bill Qty: " + dtBilling.Rows[j]["BillQty"].ToString() + ", " + Environment.NewLine;
                            nQty = nQty + Convert.ToDecimal(dtBilling.Rows[j]["BillQty"]);
                            if (dtBilling.Rows[j]["Rush"] != DBNull.Value && Convert.ToBoolean(dtBilling.Rows[j]["Rush"]) == true)
                                chkRush.Checked = true;
                        }
                    }
                    strTestDesc = strTestDesc.Trim();
                    int nI = dtLogTests.Rows.IndexOf(foundRows[0]);
                    dtLogTests.Rows[nI]["BillQty"] = nQty;
                    dtLogTests.Rows[nI]["QuotationNo"] = strListQ[i];
                    dtLogTests.Rows[nI]["QCompanyCode"] = strListCmpy[i];
                    dtLogTests.Rows[nI]["AddlNotes"] = strTestDesc.Substring(0, strTestDesc.Length - 1);
                }
            }

            DataView dvw = new DataView(dtLogTests);
            dvw.RowFilter = "ServiceCode <> 0";
            dtLogTests = dvw.ToTable();

            dtLogTests.DefaultView.ToTable(true, "ServiceCode");
            bsLogTests.DataSource = dtLogTests;
            dtrLogTests.DataSource = bsLogTests;

            //SAMPLES & SERVICE CODE ASSIGNMENT
            if (dgvSampleSC.Rows.Count != 0)
            {
                List<string> strListSSC = new List<string>();
                if (dgvSampleSC.Rows.Count == 1)
                    strListSSC.Add(dgvSamples.Rows[0].Cells["SlashNo"].Value.ToString());
                else
                {
                    for (int i = 0; i < dgvSamples.Rows.Count - 1; i++)
                    {
                        strListSSC.Add(dgvSamples.Rows[i].Cells["SlashNo"].Value.ToString());
                    }
                }
                ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();
            }
            //This block deletes all entries in the Slash/SC datagridview when a SC is unselected
            if (dgvSampleSC.Rows.Count != 0)
            {
                for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                {
                    int nI = 0;
                    for (int j = 0; j < strListSC.Count; j++)
                    {
                        if (dgvSampleSC.Rows[i].Cells[1].Value.ToString() == strListSC[j].ToString())
                        {
                            nI = 1;
                            break;
                        }
                    }
                    if (nI == 0)
                        dgvSampleSC.Rows[i].Selected = true;
                }
                foreach (DataGridViewRow row in dgvSampleSC.SelectedRows)
                {
                    dgvSampleSC.Rows.Remove(row);
                }
            }
            //End of Block
            ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["ServiceCode"]).DataSource = strListSC.ToArray();
            pnlQuotes.Visible = false;
            nQu = 0;
        }

        private void dgvTests_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;
            else
            {
                if (dgvTests.CurrentCell.OwningColumn.Name.ToString() != "BillQty" && dgvTests.CurrentCell.OwningColumn.Name.ToString() != "SelectedTest" && dgvTests.CurrentCell.OwningColumn.Name.ToString() != "Rush")
                    e.Cancel = true;
                else if (dgvTests.CurrentCell.OwningColumn.Name.ToString() == "SelectedTest" && dgvTests.Rows[e.RowIndex].Cells["BillQty"].Value.ToString() == "0")
                    e.Cancel = true;
                else if (dgvTests.CurrentCell.OwningColumn.Name.ToString() == "Rush" && dgvTests.Rows[e.RowIndex].Cells["BillQty"].Value.ToString() == "0")
                    e.Cancel = true;
            }
        }

        private void pnlRecord_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void pnlRecord_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlRecord_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                mouseDown = false;
        }

        private void lnkRefCtrlSubs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Process.Start(@"\\gblnj4\GIS\Reports\Controlled Substances.pdf");
            System.Diagnostics.Process.Start("http://www.deadiversion.usdoj.gov/schedules/orangebook/orangebook.pdf");//direct to FDA's website
        }

        private void chkCancelled_CheckedChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (chkCancelled.Checked == true)
                {
                    pnlCalendar.Visible = true;
                }
                else
                {
                    pnlCalendar.Visible = false; mskDateCancelled.Text = "__/__/_____";
                }
            }
        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataView dvwContacts;
                dvwContacts = new DataView(dtContacts, "Contact like '" + txtContact.Text.Trim().Replace("'", "''") + "%'", "Contact", DataViewRowState.CurrentRows);
                PSSClass.General.DGVSetUp(dgvContacts, dvwContacts, 369);
            }
            catch { }
        }

        private void dtpReceived_Validating(object sender, CancelEventArgs e)
        {
            if (dtpReceived.Value > dtpEntered.Value)
            {
                MessageBox.Show("Received date is invalid.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtpReceived.Value = DateTime.Now;
                e.Cancel = true;
            }
        }

        private void dtpSAPDate_ValueChanged(object sender, EventArgs e)
        {
            dtpSAPDate.Format = DateTimePickerFormat.Custom;
            dtpSAPDate.CustomFormat = "MM/dd/yyyy";
        }

        private void dgvSamples_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvSamples.CurrentCell.OwningColumn.Name == "SlashNo") 
                {
                    if (dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString().Trim() != "")
                    {
                        try
                        {
                            int n = int.Parse(dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString());
                            dgvSamples.CurrentRow.Cells["SlashNo"].Value = n.ToString("000");
                        }
                        catch { }

                        List<string> strListSSC = new List<string>();
                        for (int i = 0; i < dgvSamples.Rows.Count - 1; i++)
                        {
                            strListSSC.Add(dgvSamples.Rows[i].Cells["SlashNo"].Value.ToString());
                        }
                        //Update Data Source for Slashes (combobox cell) in Slash/SC Datagridview
                        ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();
                    }
                    if (dgvSamples.Rows[e.RowIndex].Cells["SlashID"].Value == DBNull.Value)
                        dgvSamples.Rows[e.RowIndex].Cells["SlashID"].Value = 0;
                }
            }
            catch { }
            lMoveNext = 1;
            colIndex = e.ColumnIndex;
            rowIndex = e.RowIndex;
        }

        private void dgvSlashSC_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                dtrLogTests.RemoveAt(dtrLogTests.CurrentItemIndex);
            }
            catch { }
        }

        private void btnExtSCData_Click(object sender, EventArgs e)
        {
            if (dtLogTests.Rows.Count == 0 || txtSponsorID.Text == "")
                return;

            txtGBLSCExt.Text = txtLogNo.Text;
            txtSCExt.Text = ((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text;
            txtSCSpExt.Text = ((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text + "," + txtSponsorID.Text;
            txtStudyNo.Text = "";
            cboStudyDir.SelectedValue = 0;

            DataTable dt = PSSClass.Samples.ExExtDataLabels();
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No defined labels.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            dgvSCExtData.Columns.Clear(); dgvSCExtData.Rows.Clear();
            DataGridViewComboBoxColumn cboLabels = new DataGridViewComboBoxColumn();
            DataGridViewTextBoxColumn txtValues = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn txtSC = new DataGridViewTextBoxColumn();

            dgvSCExtData.Columns.Add(cboLabels);
            dgvSCExtData.Columns.Add(txtValues);
            dgvSCExtData.Columns.Add(txtSC);

            dgvSCExtData.Columns[0].Name = "Label";
            dgvSCExtData.Columns[1].Name = "Value";
            dgvSCExtData.Columns[2].Name = "SC";

            dgvSCExtData.Columns["Label"].Width = 250;
            dgvSCExtData.Columns["Value"].Width = 200;
            dgvSCExtData.Columns["Label"].HeaderText = "DATA LABEL";
            dgvSCExtData.Columns["Value"].HeaderText = "VALUE";
            dgvSCExtData.Columns["SC"].Visible = false;
            StandardDGVSetting(dgvSCExtData);
            dgvSCExtData.Enabled = true;

            var stringArr = dt.AsEnumerable().Select(r => r.Field<string>("DataLabelDesc")).ToArray();

            ((DataGridViewComboBoxColumn)dgvSCExtData.Columns["Label"]).DataSource = stringArr;

            DataRow[] foundRows;
            foundRows = dtSCExtData.Select("ServiceCode = " + ((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
            if (foundRows.Length == 0)
            {
            }
            else
            {
                DataTable dtX = new DataTable();
                dtX = foundRows.CopyToDataTable();
                txtStudyNo.Text = dtX.Rows[0]["StudyNo"].ToString();
                if (dtX.Rows[0]["StudyDirID"] != null && dtX.Rows[0]["StudyDirID"].ToString() != "")
                    cboStudyDir.SelectedValue = Convert.ToInt16(dtX.Rows[0]["StudyDirID"].ToString());
                else
                    cboStudyDir.SelectedValue = 0;

                try
                {
                    for (int r = 0; r < dtX.Rows.Count; r++)
                    {
                        dgvSCExtData.Rows.Add(dtX.Rows[r]["SCExtDataLabel"].ToString(), dtX.Rows[r]["SCExtDataValue"].ToString(), txtSCExt.Text);
                    }
                }
                catch { }
            }

            if (nMode == 0)
            {
                OpenControls(pnlSCExt, false); btnSCExtSave.Enabled = false;
            }
            else
            {
                OpenControls(pnlSCExt, true); btnSCExtSave.Enabled = true;
            }
            pnlSCExtData.Visible = true; pnlSCExtData.BringToFront(); pnlSCExtData.Location = new Point(178, 101);
            pnlRecord.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (nFR == 1)
            {
                nFR = 0;
                SendKeys.Send("{F12}");
                return;
            }
            if (nMode != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.Select();
            dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false;
            pnlQuotes.Visible = false;
            AddEditMode(false); 
            FileAccess();
            try
            {
                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() != "" || dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "")
                    tsbEdit.Enabled = false;
            }
            catch { }
        }

        private void chkEC_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = sender as CheckBox;
                if (chk.Checked)
                {
                    dtrLogTests.CurrentItem.Controls["lblDateSampledX"].Visible = false;
                    dtrLogTests.CurrentItem.Controls["lblECEndDateX"].Visible = false;
                }
                else
                {
                    dtrLogTests.CurrentItem.Controls["lblDateSampledX"].Visible = true;
                    dtrLogTests.CurrentItem.Controls["lblECEndDateX"].Visible = true;
                }
            }
            catch { }
        }
       
        private void SamplesLogin_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    if (nMode == 0 && strFileAccess != "RO")
                    {
                        AddEditMode(true); AddRecord();
                    }
                    break;

                case Keys.F3:
                    if (nMode == 0 && strFileAccess != "RO" && dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() == "")
                    {
                        AddEditMode(true); EditRecord();
                    }
                    break;

                //case Keys.F4:
                //    if (nMode == 0)
                //    {
                //        DeleteRecord();
                //    }
                //    break;

                case Keys.F5:
                    if (nMode != 0)
                        SaveRecord();
                    break;

                case Keys.F6:
                    if (nMode != 0)
                        CancelSave();
                    break;

                case Keys.F7:
                    if (nMode == 0)
                        tsddbPrint.ShowDropDown();
                    break;

                case Keys.F8:
                    if (nMode == 0)
                        tsddbSearch.ShowDropDown();
                    break;
                case Keys.F9:
                    if (nMode == 0)
                        SearchOKClickHandler(null, null);
                    break;

                case Keys.F10:
                    if (nMode == 0)
                        SearchFilterClickHandler(null, null);
                    break;

                case Keys.F11:
                    if (nMode == 0)
                        RefreshClickHandler(null, null);
                    break;

                case Keys.F12:
                    if (nMode != 0)
                    {
                        DialogResult dReply = new DialogResult();
                        dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dReply == DialogResult.No)
                            return;
                    }
                    this.Close();
                    break;

                default:
                    break;
            }
        }

        private void rdoECMM_CheckedChanged(object sender, EventArgs e)
        {
            nType = 3;
            try
            {
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Focus();
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Select();
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).SelectAll();
            }
            catch { }
        }

        private void rdoECWW_CheckedChanged(object sender, EventArgs e)
        {
            nType = 2;
            try
            {
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Focus();
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Select();
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).SelectAll();
            }
            catch { }
        }

        private void rdoECYY_CheckedChanged(object sender, EventArgs e)
        {
            nType = 4;
            try
            {
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Focus();
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Select();
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).SelectAll();
            }
            catch { }
        }

        private void rdoECDD_CheckedChanged(object sender, EventArgs e)
        {
            nType = 1;
            try
            {
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Focus();
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Select();
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).SelectAll();
            }
            catch { }
        }

        private void CopyRow()
        {
            DataRow drSample;
            if (dgvSamples.Rows.Count > 1)
            {
                if (dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString().Trim() != "")
                {
                    drSample = dtSamples.NewRow();
                    try
                    {
                        drSample["SlashNo"] = (Convert.ToInt16(dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value) + 1).ToString("000");
                    }
                    catch
                    {
                        drSample["SlashNo"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value.ToString();
                    }
                    drSample["SampleDesc"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SampleDesc"].Value.ToString();
                    drSample["OtherDesc1"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["OtherDesc1"].Value.ToString();
                    drSample["OtherDesc2"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["OtherDesc2"].Value.ToString();
                    drSample["SlashID"] = 0;

                    dtSamples.Rows.Add(drSample);
                    try
                    {
                        List<string> strListSSC = new List<string>();
                        for (int i = 0; i < dgvSamples.Rows.Count - 1; i++)
                        {
                            strListSSC.Add(dgvSamples.Rows[i].Cells["SlashNo"].Value.ToString());
                        }
                        ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();
                    }
                    catch { }

                    if (dgvSamples.Rows.Count > 1)
                        dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Selected = true;
                }
            }
            else
            {
                drSample = dtSamples.NewRow();
                drSample["SlashNo"] = "";
                drSample["SlashID"] = 0;
                drSample["SampleDesc"] = "";
                drSample["OtherDesc1"] = "";
                drSample["OtherDesc2"] = "";

                dtSamples.Rows.Add(drSample);
            }
        }

        private void dgvTests_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTests.CurrentCell.OwningColumn.Name.ToString() == "BillQty")
            {
                if (dgvTests.CurrentCell.Value.ToString() != "0")
                    dgvTests.CurrentRow.Cells["SelectedTest"].Value = 1;
                else
                {
                    dgvTests.CurrentRow.Cells["SelectedTest"].Value = 0;
                    dgvTests.CurrentRow.Cells["Rush"].Value = 0;
                }
            }
            if (dgvTests.CurrentCell.OwningColumn.Name.ToString() == "SelectedTest" && dgvTests.CurrentCell.Value.ToString() == "False")
                dgvTests.CurrentRow.Cells["BillQty"].Value = 0;
            else if (dgvTests.CurrentCell.OwningColumn.Name.ToString() == "Rush" && dgvTests.CurrentCell.Value.ToString() == "False")
                dgvTests.CurrentRow.Cells["RushPrice"].Value = 0;
            else if (dgvTests.CurrentCell.OwningColumn.Name.ToString() == "Rush" && dgvTests.CurrentCell.Value.ToString() == "True")
            {
                string strQ = dgvTests.CurrentRow.Cells["QuoteNo"].Value.ToString();
                int nI = strQ.IndexOf("R");
                string strQNo = strQ.Substring(0, nI - 1);
                string strRNo = strQ.Substring(nI + 1, strQ.Length - (nI + 1));
                string strCNo = dgvTests.CurrentRow.Cells["ControlNo"].Value.ToString();
                decimal nPrice = PSSClass.Quotations.QuoteRushPrice(strQNo, Convert.ToInt16(strRNo), Convert.ToInt16(strCNo));
                if (nPrice == 0)
                    dgvTests.CurrentRow.Cells["RushPrice"].Value = (Convert.ToDecimal(dgvTests.CurrentRow.Cells["UnitPrice"].Value) * 2).ToString();
                else
                    dgvTests.CurrentRow.Cells["RushPrice"].Value = nPrice;
            }
            nQ = 1;
        }

        private void btnCloseTests_Click(object sender, EventArgs e)
        {
            pnlQuotes.Visible = false; dgvSamples.Visible = true; dgvSamples.BringToFront();
            
            if (nQ == 1 && nMode != 0)
                btnTests_Click(null, null);
        }

        private void txtQuoteNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dtrLogTests_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            switch (e.Control.Name)
            {
                case "txtCmpyCodeLT":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["CompanyCode"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtSC":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["ServiceCode"] = e.Value;
                        int n = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(e.Value));
                        dtLogTests.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddDays(n);
                        dtLogTests.Rows[e.ItemIndex]["EndDate"] = ((DateTimePicker)dtrLogTests.CurrentItem.Controls["dtpStartDate"]).Value.AddDays(n);
                        ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = false;
                    }
                    catch { }
                    break;
                case "txtProtocolNo":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["ProtocolNo"] = e.Value;
                    }
                    catch { }
                    break;
                case "dtpStartDate":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["StartDate"] = e.Value;
                        int n = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
                        dtLogTests.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(e.Value).AddDays(n);
                    }
                    catch { }
                    break;
                case "dtpEndDate":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["EndDate"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtPONo":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["PONo"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtBookNo":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["BookNo"] = e.Value;
                    }
                    catch { }
                    break;
                case "chkEC":
                     try
                    {
                        dtLogTests.Rows[e.ItemIndex]["EC"] = e.Value;
                    }
                    catch { }
                    break;
                case "rdoECDD":
                    try
                    {
                        if (nType == 1)
                        {
                            dtLogTests.Rows[e.ItemIndex]["ECCompType"] = "D";
                            dtLogTests.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddDays(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ECLength"]));
                            int n = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
                            dtLogTests.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
                        }
                    }
                    catch { }
                    break;
                case "rdoECWW":
                    try
                    {
                        if (nType == 2)
                        {
                            dtLogTests.Rows[e.ItemIndex]["ECCompType"] = "W";
                            dtLogTests.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddDays(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ECLength"]) * 7);
                            int n = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
                            dtLogTests.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
                        }
                    }
                    catch { }
                    break;
                case "rdoECMM":
                    try
                    {
                        if (nType == 3)
                        {
                            dtLogTests.Rows[e.ItemIndex]["ECCompType"] = "M";
                            dtLogTests.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddMonths(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ECLength"]));
                            int n = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
                            dtLogTests.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
                        }
                    }
                    catch { }
                    break;
                case "rdoECYY":
                    try
                    {
                        if (nType == 4)
                        {
                            dtLogTests.Rows[e.ItemIndex]["ECCompType"] = "Y";
                            dtLogTests.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddYears(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ECLength"]));
                            int n = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
                            dtLogTests.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
                        }
                    }
                    catch { }
                    break;
                case "txtECLength":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["ECLength"] = Convert.ToInt16(e.Value);
                        if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "D")
                        {
                            dtLogTests.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddDays(Convert.ToInt16(e.Value));
                        }
                        else if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "W")
                        {
                            dtLogTests.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddDays(Convert.ToInt16(e.Value) * 7);
                        }
                        else if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "M")
                        {
                            dtLogTests.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddMonths(Convert.ToInt16(e.Value));
                        }
                        else if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "Y")
                        {
                            dtLogTests.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddYears(Convert.ToInt16(e.Value));
                        }
                        int n = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
                        dtLogTests.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
                    }
                    catch { }
                    break;
                case "txtSamples":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["TestSamples"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtSlashNos":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["Slashes"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtBillQty":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["BillQty"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtQuote":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["QuoteFlag"] = e.Value;
                    }
                    catch { }
                    break;
                case "dtpDateSampled":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["DateSampled"] = e.Value;
                    }
                    catch { }
                    break;
                case "dtpECEndDate":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["ECEndDate"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtAddNotes":
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["AddlNotes"] = e.Value;
                    }
                    catch { }
                    break;
            }
        }

        private void btnViewTests_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (txtSponsorID.Text.Trim() == "")
                {
                    pnlQuotes.Visible = false;
                    return;
                }
                LoadQuotes();
                nQu = 1;
                nQ = 0;
            }
            pnlQuotes.Visible = true; pnlQuotes.BringToFront();
        }

        private void btnSearchArticle_Click(object sender, EventArgs e)
        {
            cboQuotes.DataSource = null;
            cboQuotes.Items.Clear();

            DataTable dt = new DataTable();
            dt = PSSClass.Quotations.LoadQuotesSpArticle(Convert.ToInt16(txtSponsorID.Text),txtArticle.Text.Trim());

            cboQuotes.DisplayMember = "QuotationNo";
            cboQuotes.ValueMember = "QuotationNo";

            DataRow row = dt.NewRow();
            row["QuotationNo"] = "-select-";
            row["CompanyCode"] = "P";
            dt.Rows.InsertAt(row, 0);
            cboQuotes.DataSource = dt;
        }

        private void txtQuotes_Enter(object sender, EventArgs e)
        {
            dgvQuotes.Visible = true; dgvQuotes.BringToFront();
        }

        private void picQuotes_Click(object sender, EventArgs e)
        {
            dgvQuotes.Visible = true; dgvQuotes.BringToFront();
        }

        private void btnAddSCS_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataRow dRow;
                dRow = dtSampleSC.NewRow();
                dRow["Slash"] = "";
                dRow["SC"] = 0;
                try
                {
                    dRow["SlashNo"] = "";
                    dRow["ServiceCode"] = 0;
                }
                catch { }
                dtSampleSC.Rows.Add(dRow);
            }
        }

        private void dtrLogTests_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            try
            {
                var cboS = (ComboBox)e.DataRepeaterItem.Controls.Find("cboSC", false)[0];

                if (((TextBox)dtrLogTests.CurrentItem.Controls["txtQuote"]).Text == "1")
                {
                    cboS.Enabled = false; ((TextBox)dtrLogTests.CurrentItem.Controls["txtBillQty"]).ReadOnly = true;
                }
                else
                {
                    cboS.Enabled = true; ((TextBox)dtrLogTests.CurrentItem.Controls["txtBillQty"]).ReadOnly = false;
                }
            }
            catch { }
        }

        private void btnComposite_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
                return;

            if (txtCount.Text.Trim() == "")
                return;
            
            if (dgvSamples.Rows.Count == 0)
                return;

            for (int i = 0; i < Convert.ToInt16(txtRows.Text); i++)
            {
                DataRow drSample;
                drSample = dtSamples.NewRow();
                drSample["SlashNo"] = (i + 1).ToString() + "(" + PSSClass.General.CompositeEntry(Convert.ToInt16(txtCount.Text)) + ")";
                drSample["SampleDesc"] = "";
                drSample["OtherDesc1"] = "";
                drSample["OtherDesc2"] = "";
                drSample["SlashID"] = 0;
                dtSamples.Rows.Add(drSample);
            }

            try
            {
                List<string> strListSSC = new List<string>();
                for (int i = 0; i < dgvSamples.Rows.Count - 1; i++)
                {
                    strListSSC.Add(dgvSamples.Rows[i].Cells[1].Value.ToString());
                }
                
                ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();

                dgvSamples.CurrentCell = dgvSamples[0, dtSamples.Rows.Count - 1];
                if (dgvSamples.Rows.Count - 1 != dgvAddlData.Rows.Count)
                    dgvAddlData.Rows.Add(dgvSamples.CurrentCell.Value);
            }
            catch { }
        }

        private void btnSeries_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
                return;

            if (dgvSamples.Rows.Count <= 1)
                return;

            if (txtSeriesFr.Text.Trim() == "" || txtSeriesTo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a range.",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (Convert.ToInt16(txtSeriesFr.Text) > Convert.ToInt16(txtSeriesTo.Text))
            {
                MessageBox.Show("Invalid range.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string strDesc = dgvSamples.CurrentRow.Cells["SampleDesc"].Value.ToString();
                int nR = Convert.ToInt16(txtSeriesTo.Text) - Convert.ToInt16(txtSeriesFr.Text);
                int nS = Convert.ToInt16(txtSeriesFr.Text);
                int nE = Convert.ToInt16(txtSeriesTo.Text);

                SamplesAddlDataLabels();

                if (chkSingleRow.Checked && chkAlpha.Checked == false) 
                {
                    DataRow drSample;
                    drSample = dtSamples.NewRow();
                    drSample["SlashNo"] = nS.ToString("000") + "-" + nE.ToString("000");
                    drSample["SampleDesc"] = dgvSamples.CurrentRow.Cells[2].Value.ToString();
                    drSample["OtherDesc1"] = dgvSamples.CurrentRow.Cells[3].Value.ToString();
                    drSample["OtherDesc2"] = dgvSamples.CurrentRow.Cells[4].Value.ToString();
                    drSample["SlashID"] = 0;
                    dtSamples.Rows.Add(drSample);
                }
                else if (chkSingleRow.Checked && chkAlpha.Checked)
                {
                    nR += 1;

                    DataRow drSample;
                    drSample = dtSamples.NewRow();
                    drSample["SlashNo"] = PSSClass.General.CompositeEntry(nR);
                    drSample["SampleDesc"] = dgvSamples.CurrentRow.Cells[2].Value.ToString();
                    drSample["OtherDesc1"] = dgvSamples.CurrentRow.Cells[3].Value.ToString();
                    drSample["OtherDesc2"] = dgvSamples.CurrentRow.Cells[4].Value.ToString();
                    drSample["SlashID"] = 0;
                    dtSamples.Rows.Add(drSample);
                }
                else
                {
                    for (int i = 0; i <= nR; i++)
                    {
                        DataRow drSample;
                        drSample = dtSamples.NewRow();
                        drSample["SlashNo"] = nS.ToString("000");
                        drSample["SampleDesc"] = dgvSamples.CurrentRow.Cells[2].Value.ToString();
                        drSample["OtherDesc1"] = dgvSamples.CurrentRow.Cells[3].Value.ToString();
                        drSample["OtherDesc2"] = dgvSamples.CurrentRow.Cells[4].Value.ToString();
                        drSample["SlashID"] = 0;
                        dtSamples.Rows.Add(drSample);
                        nS++;
                    }
                }
                try
                {
                    List<string> strListSSC = new List<string>();
                    for (int i = 0; i < dgvSamples.Rows.Count - 1; i++)
                    {
                        strListSSC.Add(dgvSamples.Rows[i].Cells[1].Value.ToString());
                    }
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnDelTest_Click(object sender, EventArgs e)
        {
            if (nMode != 0 && dtLogTests.Rows.Count > 0 && dtLogTests.Rows[dtrLogTests.CurrentItemIndex].RowState.ToString() != "Deleted")
            {
                string strSC = dtLogTests.Rows[dtrLogTests.CurrentItemIndex]["Servicecode"].ToString();
                if (nMode == 2)
                    nListDelSC.Add(Convert.ToInt16(strSC));

                dtLogTests.Rows[dtrLogTests.CurrentItemIndex].Delete();

                for (int i = 0; i < dgvTests.Rows.Count; i++)
                {
                    if (dgvTests.Rows[i].Cells["ServiceCode"].Value.ToString() == strSC)
                    {
                        dgvTests.Rows[i].Cells["BillQty"].Value = 0;
                        dgvTests.Rows[i].Cells["SelectedTest"].Value = 0;
                        dgvTests.Rows[i].Cells["Rush"].Value = 0;
                    }
                }
                bsLogTests.DataSource = dtLogTests;
                dtrLogTests.DataSource = bsLogTests;

                List<string> strLSC = new List<string>();
                strLSC.Add(strSC);

                //This block deletes all entries in the Slash/SC datagridview when a test is deleted
                if (dgvSampleSC.Rows.Count != 0)
                {
                    for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                    {
                        for (int j = 0; j < strLSC.Count; j++)
                        {
                            if (dgvSampleSC.Rows[i].Cells["SC"].Value.ToString() == strLSC[j].ToString())
                            {
                                dgvSampleSC.Rows[i].Selected = true;
                            }
                        }
                    }
                    foreach (DataGridViewRow row in dgvSampleSC.SelectedRows)
                    {
                        dgvSampleSC.Rows.Remove(row);
                    }
                }
            }
        }

        private void dgvTests_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            nQ = 1;
        }

        private void btnUnSelQTests_Click(object sender, EventArgs e)
        {
            if (dgvTests.Rows.Count == 0)
                return;
            for (int i = 0; i < dgvTests.Rows.Count; i++)
            {
                if (dgvTests.Rows[i].Selected == true)
                {
                    dgvTests.Rows[i].Cells["BillQty"].Value = 0;
                    dgvTests.Rows[i].Cells["SelectedTest"].Value = 0;
                    dgvTests.Rows[i].Cells["Rush"].Value = 0;
                }
            }
        }

        private void btnSelQTests_Click(object sender, EventArgs e)
        {
            if (dgvTests.Rows.Count == 0)
                return;

            string strQ = dgvTests.CurrentRow.Cells[0].Value.ToString();
            for (int i = 0; i < dgvTests.Rows.Count; i++)
            {
                if (dgvTests.Rows[i].Selected == true)
                    dgvTests.Rows[i].Cells[5].Value = 1;
            }
        }

        private void btnSelAllTests_Click(object sender, EventArgs e)
        {
            if (dgvTests.Rows.Count == 0)
                return;

            for (int i = 0; i < dgvTests.Rows.Count; i++)
            {
                dgvTests.Rows[i].Cells[5].Value = 1;
            }
        }

        private void dgvPONo_DoubleClick(object sender, EventArgs e)
        {
            ((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtPONo"]).Text = ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).CurrentRow.Cells[0].Value.ToString();
            ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).Visible = false; 
        }

        private void dgvPONo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvPONo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtPONo"]).Text = ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).CurrentRow.Cells[0].Value.ToString();
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).Visible = false; 
            }
            else if (e.KeyChar == 27)
            {
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).Visible = false; 
            }
        }

        private void txtPONoKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            //e.Handled = true;
        }

        private void dgvPONoOnLeave(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).Visible = false;
            }
            catch { }
        }

        private void cboSlashSC_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (dtSamples.Rows.Count == 0)
                return;

            List<string> strListSSC = new List<string>();
            List<string> strLSC = new List<string>();
            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                if (dtLogTests.Rows[i].RowState.ToString() != "Deleted")
                {
                    strLSC.Add(dtLogTests.Rows[i]["ServiceCode"].ToString());
                }
            }
            for (int i = 0; i < dgvSamples.Rows.Count - 1; i++)
            {
                strListSSC.Add(dgvSamples.Rows[i].Cells["SlashNo"].Value.ToString());
            }

            if (cboSlashSC.SelectedIndex == 0)
            {
                for (int i = 0; i < dtLogTests.Rows.Count; i++)
                {
                    if (dtLogTests.Rows[i].RowState.ToString() != "Deleted")
                    {
                        for (int j = 0; j < dgvSamples.Rows.Count - 1; j++)
                        {
                            byte nSC = 0;
                            if (dgvSamples.Rows[j].Cells["SlashNo"].Value.ToString().Trim() != "")
                            {
                                for (int k = 0; k < dgvSampleSC.Rows.Count; k++)
                                {
                                    if (dgvSamples.Rows[j].Cells["SlashNo"].Value.ToString() == dgvSampleSC.Rows[k].Cells["Slash"].Value.ToString() &&
                                        dtLogTests.Rows[i]["ServiceCode"].ToString() == dgvSampleSC.Rows[k].Cells["SC"].Value.ToString())
                                    {
                                        nSC = 1;
                                        break;
                                    }
                                }
                                if (nSC == 0)
                                {
                                    DataRow dR;
                                    dR = dtSampleSC.NewRow();
                                    dR["Slash"] = dgvSamples.Rows[j].Cells["SlashNo"].Value.ToString();
                                    dR["SC"] = dtLogTests.Rows[i]["ServiceCode"].ToString();
                                    dtSampleSC.Rows.Add(dR);
                                    dgvSampleSC.DataSource = bsSampleSC;
                                }
                                nSC = 0;
                            }
                        }
                    }
                }
                if (strListSSC.Count > 0)
                {
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns[0]).DataSource = strListSSC.ToArray();
                }
                if (strLSC.Count > 0)
                {
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns[1]).DataSource = strLSC.ToArray();
                }

                for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                {
                    dgvSampleSC.Rows[i].Cells["SlashNo"].Value = dgvSampleSC.Rows[i].Cells["Slash"].Value.ToString();
                    dgvSampleSC.Rows[i].Cells["ServiceCode"].Value = dgvSampleSC.Rows[i].Cells["SC"].Value.ToString();
                }
            }
            else if (cboSlashSC.SelectedIndex == 1)
            {
                byte nSC = 0;
                if (dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value.ToString().Trim() != "")
                {
                    for (int j = 0; j < dtLogTests.Rows.Count; j++)
                    {
                        if (dtLogTests.Rows[j].RowState.ToString() != "Deleted")
                        {
                            for (int k = 0; k < dgvSampleSC.Rows.Count; k++)
                            {
                                if (dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value.ToString() == dgvSampleSC.Rows[k].Cells["Slash"].Value.ToString() &&
                                    dtLogTests.Rows[j]["ServiceCode"].ToString() == dgvSampleSC.Rows[k].Cells["SC"].Value.ToString())
                                {
                                    nSC = 1;
                                    break;
                                }
                            }
                            if (nSC == 0)
                            {
                                DataRow dR;
                                dR = dtSampleSC.NewRow();
                                dR["Slash"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value.ToString();
                                dR["SC"] = dtLogTests.Rows[j]["ServiceCode"].ToString();
                                dtSampleSC.Rows.Add(dR);
                                dgvSampleSC.DataSource = bsSampleSC;
                            }
                            nSC = 0;
                        }
                    }
                }
                if (strListSSC.Count > 0)
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();

                if (strLSC.Count > 0)
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["ServiceCode"]).DataSource = strLSC.ToArray();

                for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                {
                    dgvSampleSC.Rows[i].Cells["SlashNo"].Value = dgvSampleSC.Rows[i].Cells["Slash"].Value.ToString();
                    dgvSampleSC.Rows[i].Cells["ServiceCode"].Value = dgvSampleSC.Rows[i].Cells["SC"].Value.ToString();
                }
            }
            else if (cboSlashSC.SelectedIndex == 2)
            {
                byte nSC = 0;
                for (int k = 0; k < dgvSampleSC.Rows.Count; k++)
                {
                    if (dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value.ToString() == dgvSampleSC.Rows[k].Cells["Slash"].Value.ToString() &&
                        dtLogTests.Rows[dtrLogTests.CurrentItemIndex]["ServiceCode"].ToString() == dgvSampleSC.Rows[k].Cells["SC"].Value.ToString())
                    {
                        nSC = 1;
                        break;
                    }
                }
                if (nSC == 0)
                {
                    DataRow dR;
                    dR = dtSampleSC.NewRow();
                    dR["Slash"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value.ToString();
                    dR["SC"] = dtLogTests.Rows[dtrLogTests.CurrentItemIndex]["ServiceCode"].ToString();
                    dtSampleSC.Rows.Add(dR);
                    dgvSampleSC.DataSource = bsSampleSC;
                }

                if (dgvSamples.Rows.Count == 1)
                    return;

                if (dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString().Trim() == "" || dtLogTests.Rows.Count == 0)
                    return;
                if (strListSSC.Count > 0)
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();

                if (strLSC.Count > 0)
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["ServiceCode"]).DataSource = strLSC.ToArray();

                int n = dgvSampleSC.Rows.Count - 1;

                dgvSampleSC.Rows[n].Cells[0].Value = dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString();
                dgvSampleSC.Rows[n].Cells[1].Value = dtLogTests.Rows[dtrLogTests.CurrentItemIndex]["ServiceCode"].ToString();
            }
            else if (cboSlashSC.SelectedIndex == 3)
            {
                byte nSC = 0;
                if (dtLogTests.Rows[dtrLogTests.CurrentItemIndex].RowState.ToString() != "Deleted")
                {
                    for (int j = 0; j < dgvSamples.Rows.Count - 1; j++)
                    {
                        for (int k = 0; k < dgvSampleSC.Rows.Count; k++)
                        {
                            if (dgvSamples.Rows[j].Cells["SlashNo"].Value.ToString() == dgvSampleSC.Rows[k].Cells["Slash"].Value.ToString() &&
                                dtLogTests.Rows[dtrLogTests.CurrentItemIndex]["ServiceCode"].ToString() == dgvSampleSC.Rows[k].Cells["SC"].Value.ToString())
                            {
                                nSC = 1;
                                break;
                            }
                        }
                        if (nSC == 0)
                        {
                            DataRow dR;
                            dR = dtSampleSC.NewRow();
                            dR["Slash"] = dgvSamples.Rows[j].Cells["SlashNo"].Value.ToString();
                            dR["SC"] = dtLogTests.Rows[dtrLogTests.CurrentItemIndex]["ServiceCode"].ToString();
                            dtSampleSC.Rows.Add(dR);
                            dgvSampleSC.DataSource = bsSampleSC;
                        }
                        nSC = 0;
                    }
                }
                if (strListSSC.Count > 0)
                {
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();
                }

                if (strLSC.Count > 0)
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["ServiceCode"]).DataSource = strLSC.ToArray();

                for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                {
                    dgvSampleSC.Rows[i].Cells["SlashNo"].Value = dgvSampleSC.Rows[i].Cells["Slash"].Value.ToString();
                    dgvSampleSC.Rows[i].Cells["ServiceCode"].Value = dgvSampleSC.Rows[i].Cells["SC"].Value.ToString();
                }
            }
        }

        private void btnSlashSC_Click(object sender, EventArgs e)
        {
            cboSlashSC_SelectionChangeCommitted(null, null);
        }

        private void btnPONo_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvPONo.DataSource = null;
                dtPONo = PSSClass.PO.PODDL(Convert.ToInt16(txtSponsorID.Text));
                if (dtPONo != null)
                {
                    ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).DataSource = dtPONo;
                    ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).Columns[0].Width = 130;
                    ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).Visible = true;
                    StandardDGVSetting(dgvPONo);
                    ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).Select();
                }
            }
        }

        private void btnNoSamples_Click(object sender, EventArgs e)
        {
            int n = 0;
            string strSampFrom = "";//, strSFrom = "";
            string strSampTo = "";//, strSTo = "";

            for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
            {
                if (((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text == dgvSampleSC.Rows[i].Cells[1].Value.ToString())
                {
                    if (n == 0)
                    {
                        strSampFrom = dgvSampleSC.Rows[i].Cells[0].Value.ToString();
                        n = 1;
                    }
                    else
                    {
                        strSampTo = dgvSampleSC.Rows[i].Cells[0].Value.ToString();
                    }
                }
            }

            if (strSampTo == "")
                strSampTo = strSampFrom;

            try
            {
                int nF = int.Parse(strSampFrom);
                strSampFrom = nF.ToString().Trim();
            }
            catch { }
            try
            {
                int nT = int.Parse(strSampTo);
                strSampTo = nT.ToString().Trim();
            }
            catch { }

            int nDF = strSampFrom.IndexOf("-");
            if (nDF != -1)
            {
                string strFrom = strSampFrom.Substring(0, nDF);
                try
                {
                    int nT = int.Parse(strFrom);
                    strSampFrom = nT.ToString().Trim();
                }
                catch { }
            }

            int nDT = strSampTo.IndexOf("-");
            if (nDT != -1)
            {
                string strTo = strSampTo.Substring(0, nDT);
                try
                {
                    int nT = int.Parse(strTo);
                    strSampTo = nT.ToString().Trim();
                }
                catch { }
            }

            if (strSampFrom == strSampTo || strSampTo == "")
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtSlashNos"]).Text = strSampFrom;
            else
                ((TextBox)dtrLogTests.CurrentItem.Controls["txtSlashNos"]).Text = strSampFrom + "-" + strSampTo;
        }

        private void btnCloseAddlData_Click(object sender, EventArgs e)
        {
            pnlAddlData.Visible = false;
        }

        private void dgvAddlData_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0)
                e.Cancel = true;
        }

        private void btnAddTest_Click(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = false;
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).Visible = false;
            }
            catch { }

            if (nMode != 0)
            {
                try
                {
                    ((TextBox)dtrLogTests.CurrentItem.Controls["txtQuote"]).Text = "0";
                }
                catch { }
                nQu = 0;
                DataRow dr;
                dr = dtLogTests.NewRow();
                dr["ServiceCode"] = 0;
                dr["ServiceDesc"] = "";
                dr["ProtocolNo"] = "";
                dr["StartDate"] = DateTime.Now;
                dr["EndDate"] = DateTime.Now;
                dr["PONo"] = "----";
                dr["TestSamples"] = "";
                dr["Slashes"] = "";
                dr["BillQty"] = 0;
                dr["QuotationNo"] = "";
                dr["BookNo"] = 0;
                dr["EC"] = false;
                dr["ECCompType"] = DBNull.Value;
                dr["ECLength"] = DBNull.Value;
                dr["ECEndDate"] = DBNull.Value;
                dr["DateSampled"] = DBNull.Value;
                dr["QuoteFlag"] = "0";
                dr["ReportNo"] = 0;
                dr["AddlNotes"] = "";
                dtLogTests.Rows.Add(dr);
                bsLogTests.DataSource = dtLogTests;
                dtrLogTests.DataSource = bsLogTests;
                int nRows = dtLogTests.Rows.Count;
                try
                {
                    dtrLogTests.CurrentItemIndex = nRows - 1;
                }
                catch { }
            }
            dtSCMaster = PSSClass.ServiceCodes.SCDDLCombo();
            if (dtSCMaster == null)
            {
                return;
            }
            dgvSC.DataSource = dtSCMaster;
        }

        private void dtrLogTests_ItemsRemoved(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterAddRemoveItemsEventArgs e)
        {
            if (nQu == 0)
            {
                List<string> strListSC = new List<string>();
                if (dtLogTests.Rows.Count != 0)
                {
                    for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtLogTests.Rows.Count; j++)
                        {
                            if (dtLogTests.Rows[j].HasVersion(DataRowVersion.Original))
                            {
                                if (dgvSampleSC.Rows[i].Cells[1].Value.ToString() == dtLogTests.Rows[j]["ServiceCode", DataRowVersion.Original].ToString() && dtLogTests.Rows[j].RowState.ToString() == "Deleted")
                                {
                                    dgvSampleSC.Rows[i].Selected = true;
                                }
                            }
                        }
                    }

                    foreach (DataGridViewRow row in dgvSampleSC.SelectedRows)
                    {
                        dtSampleSC.Rows[row.Index].Delete();
                    }
                }
                else
                {
                    for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                    {
                        dgvSampleSC.Rows[i].Selected = true;
                    }
                    foreach (DataGridViewRow row in dgvSampleSC.SelectedRows)
                    {
                        dtSampleSC.Rows[row.Index].Delete();
                    }
                }
            }
        }


        private void cboSC_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SendKeys.Send("{TAB}");
        }

        private void cboSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((TextBox)dtrLogTests.CurrentItem.Controls["txtQuote"]).Text == "1")
                e.Handled = true;
        }

        private void pnlSCExtData_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlSCExtData.Location = PointToClient(this.pnlSCExtData.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlSCExtData_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                mouseDown = false;
        }

        private void pnlSCExtData_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void dgvSCExtData_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.ColumnIndex == 2)
                e.Cancel = true;

            if ((dgvSCExtData.Rows[dgvSCExtData.CurrentCell.RowIndex].Cells[0].Value.ToString() == "" && e.ColumnIndex == 1) || (dgvSCExtData.Rows[dgvSCExtData.CurrentCell.RowIndex].Cells[2].Value.ToString() == "" && e.ColumnIndex == 3))
                e.Cancel = true;
        }

        private void btnExtSlash_Click(object sender, EventArgs e)
        {
            DataTable dt = PSSClass.Samples.ExExtDataLabels();
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No defined labels.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var stringArr = dt.AsEnumerable().Select(r => r.Field<string>("DataLabelDesc")).ToArray();

            dgvSCExtData.Columns.Clear(); dgvSCExtData.Rows.Clear();

            DataGridViewComboBoxColumn cboLabels = new DataGridViewComboBoxColumn();
            DataGridViewTextBoxColumn txtValues = new DataGridViewTextBoxColumn();

            dgvSlashExtData.Columns.Add(cboLabels);
            dgvSlashExtData.Columns.Add(txtValues);
            dgvSlashExtData.Columns[0].Name = "Label";
            dgvSlashExtData.Columns[1].Name = "Value";

            dgvSlashExtData.Columns["Label"].Width = 250;
            dgvSlashExtData.Columns["Value"].Width = 200;
            dgvSlashExtData.Columns["Label"].HeaderText = "DATA LABEL";
            dgvSlashExtData.Columns["Value"].HeaderText = "VALUE";
            StandardDGVSetting(dgvSlashExtData);

            ((DataGridViewComboBoxColumn)dgvSlashExtData.Columns["Label"]).DataSource = stringArr;

            txtGBLSlashExt.Text = txtLogNo.Text;
            txtSlashExt.Text = dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString();
            pnlSlashExtData.Visible = true; pnlSlashExtData.BringToFront(); pnlSlashExtData.Location = new Point(178, 101);
            pnlSlashExt.Enabled = true; pnlRecord.Enabled = false;

            if (nMode == 0)
                dgvSlashExtData.ReadOnly = true;
            else
                dgvSlashExtData.ReadOnly = false;

            DataRow[] foundRows;
            foundRows = dtSlashExtData.Select("SlashNo = '" + dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString() + "'");
            if (foundRows.Length == 0)
            {
            }
            else
            {
                DataTable dtX = new DataTable();
                dtX = foundRows.CopyToDataTable();
                try
                {
                    for (int r = 0; r < dtX.Rows.Count; r++)
                    {
                        dgvSlashExtData.Rows[r].Cells["Label"].Value = dtX.Rows[r]["ExtDataLabel"].ToString();
                        dgvSlashExtData.Rows[r].Cells["Value"].Value = dtX.Rows[r]["ExtDataValue"].ToString();
                        dgvSlashExtData.Rows[r].Cells["SlashNo"].Value = dtX.Rows[r]["SlashNo"].ToString();
                    }
                }
                catch { }
            }
        }

        private void SetUpSlashLabels()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = PSSClass.Samples.ExSlashDataLabels(Convert.ToInt16(txtSponsorID.Text));
                if (dt == null)
                    return;

                dgvSamples.Columns["OtherDesc1"].HeaderText = dt.Rows[0]["Label1"].ToString();
                dgvSamples.Columns["OtherDesc2"].HeaderText = dt.Rows[0]["Label2"].ToString();
                dt.Dispose();
            }
            catch { }
        }

        private void btnDataForm_Click(object sender, EventArgs e)
        {
            if (((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text == "2122" || ((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text == "2123" || ((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text == "295")
            {
                if (PSSClass.General.UserFileAccess(LogIn.nUserID, "TestDataValuesEM") == "")
                {
                    MessageBox.Show("You have no permission to" + Environment.NewLine + "perform this task at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            if (PSSClass.General.UserFileAccess(LogIn.nUserID, "TestDataValues") == "")
            {
                MessageBox.Show("You have no permission to" + Environment.NewLine + "perform this task at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text == "2122" || ((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text == "2123" || ((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text == "295") 
            {
                int intEMOpen = PSSClass.General.OpenForm(typeof(TestDataValuesEM));

                if (intEMOpen == 0)
                {
                    TestDataValuesEM childForm = new TestDataValuesEM();
                    childForm.Text = "TEST DATA VALUES -ENVIRONMENTAL MONITORING";
                    childForm.pubCmpy = txtCmpyCode.Text;
                    childForm.nLogNo = Convert.ToInt64(txtLogNo.Text);
                    if (((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "" && ((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "0")
                        childForm.nRptNo = Convert.ToInt32(((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text);
                    else
                        childForm.nRptNo = 0;
                    childForm.nServiceCode = Convert.ToInt16(((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
                    childForm.nSponsorID = Convert.ToInt16(txtSponsorID.Text);
                    if (((TextBox)dtrLogTests.CurrentItem.Controls["txtSamples"]).Text.Trim() == "") 
                        childForm.nSlashes = 0;
                    else
                        childForm.nSlashes = Convert.ToInt16(((TextBox)dtrLogTests.CurrentItem.Controls["txtSamples"]).Text); 
                    childForm.ShowDialog();
                }
            }
            else
            {
                int intOpen = PSSClass.General.OpenForm(typeof(TestDataValues));

                if (intOpen == 0)
                {
                    TestDataValues childForm = new TestDataValues();
                    childForm.Text = "TEST DATA VALUES";
                    childForm.nLogNo = Convert.ToInt64(txtLogNo.Text);
                    if (((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "" && ((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "0")
                        childForm.nRptNo = Convert.ToInt32(((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text);
                    else
                        childForm.nRptNo = 0;
                    childForm.nServiceCode = Convert.ToInt16(((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
                    childForm.nSponsorID = Convert.ToInt16(txtSponsorID.Text);
                    childForm.ShowDialog();
                }
            }
        }

        private void dgvSamples_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (nMode == 0)
            {
                e.Cancel = true;
                return;
            }
            else if (dgvSamples.CurrentCell.OwningColumn.Name.ToString() == "SlashNo")
            {
                strSlashNo = dgvSamples.CurrentCell.Value.ToString();
            }
            lMoveNext = 0;
        }

        private void btnDelSample_Click(object sender, EventArgs e)
        {
            if (dgvSamples.Rows.Count > 1 && nMode != 0)
            {
                for (int i = 0; i < dtSlashExtData.Rows.Count; i++)
                {
                    if (dtSlashExtData.Rows[i].RowState.ToString() != "Deleted")
                    {
                        if (dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value.ToString() == dtSlashExtData.Rows[i]["SlashNo"].ToString())
                        {
                            dtSlashExtData.Rows[i].Delete();
                        }
                    }
                }
                
                dgvSamples.Rows.RemoveAt(dgvSamples.CurrentRow.Index);
                try
                {
                    List<string> strListSSC = new List<string>();
                    for (int i = 0; i < dgvSamples.Rows.Count - 1; i++)
                    {
                        strListSSC.Add(dgvSamples.Rows[i].Cells[1].Value.ToString());
                    }
                    //This block deletes all entries in the Slash/SC datagridview when a Slash is deleted
                    if (dgvSampleSC.Rows.Count != 0)
                    {
                        for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                        {
                            int nI = 0;
                            for (int j = 0; j < strListSSC.Count; j++)
                            {
                                if (dgvSampleSC.Rows[i].Cells[0].Value.ToString() == strListSSC[j].ToString())
                                {
                                    nI = 1;
                                    break;
                                }
                            }
                            if (nI == 0)
                                dgvSampleSC.Rows[i].Selected = true;
                        }
                        foreach (DataGridViewRow row in dgvSampleSC.SelectedRows)
                        {
                            dgvSampleSC.Rows.Remove(row);
                        }
                    }
                    //This block deletes all entries in the Additional Slash Data datagridview when a Slash is deleted
                    if (dgvAddlData.Rows.Count != 0)
                    {
                        for (int i = 0; i < dgvAddlData.Rows.Count; i++)
                        {
                            int nI = 0;
                            for (int j = 0; j < strListSSC.Count; j++)
                            {
                                if (dgvAddlData.Rows[i].Cells[0].Value.ToString() == strListSSC[j].ToString())
                                {
                                    nI = 1;
                                    break;
                                }
                            }
                            if (nI == 0)
                                dgvAddlData.Rows[i].Selected = true;
                        }
                        foreach (DataGridViewRow row in dgvAddlData.SelectedRows)
                        {
                            dgvAddlData.Rows.Remove(row); 
                        }
                    }
                    ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();
                }
                catch { }
            }
        }

        private void btnCopyGBL_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
                return;

            nMode = 1;
            AddEditMode(true);
            OpenControls(pnlRecord, true); pnlIngredion.Enabled = false;
            btnSelQTests.Enabled = true; btnUnSelQTests.Enabled = true; btnSelAllTests.Enabled = true; btnTests.Enabled = true; cboQuotes.Enabled = true; 
            txtLogNo.ReadOnly = true;
            txtLogNo.Text = "(New)";
            // 4-30-2017
            txtSSFormNo.Text = "";
            chkCtrldSubs.Checked = false; chkRush.Checked = false;
            //
            strList.Clear(); strListQ.Clear(); strListSC.Clear(); //added 12-14-15

            for (int i = 0; i < dtSamples.Rows.Count; i++)
            {
                dtSamples.Rows[i]["OtherDesc1"] = "";
                dtSamples.Rows[i]["OtherDesc2"] = "";
            }
            dtSamples.AcceptChanges();
            for (int i = 0; i < dtSamples.Rows.Count; i++)
            {
                dtSamples.Rows[i].SetAdded();
            }

            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                dtLogTests.Rows[i]["PONo"] = "";
                dtLogTests.Rows[i]["BillQty"] = 0;
                dtLogTests.Rows[i]["ReportNo"] = 0;
                dtLogTests.Rows[i]["QuotationNo"] = "";
                dtLogTests.Rows[i]["TestSamples"] = "";
                dtLogTests.Rows[i]["Slashes"] = "";
                dtLogTests.Rows[i]["AddlNotes"] = "";
            }
            dtLogTests.AcceptChanges();

            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                dtLogTests.Rows[i].SetAdded();
            }

            for (int i = 0; i < dtSampleSC.Rows.Count; i++)
            {
                dtSampleSC.Rows[i].SetAdded();
            }
            for (int i = 0; i < dtSamplesAddl.Rows.Count; i++)
            {
                dtSamplesAddl.Rows[i].SetAdded();
            }
            for (int i = 0; i < dtSlashExtData.Rows.Count; i++) 
            {
                dtSlashExtData.Rows[i].SetAdded();
            }
            for (int i = 0; i < dtSCExtData.Rows.Count; i++) 
            {
                dtSCExtData.Rows[i].SetAdded();
            }
            cboQuotes.DataSource = null;
            for (int i = 0; i < dtBilling.Rows.Count; i++)
            {
                if (dtBilling.Rows[i].RowState.ToString() != "Deleted")
                {
                    dtBilling.Rows[i].Delete();
                }
            }
            dtBilling.AcceptChanges();
            for (int i = 0; i < dtBilling.Rows.Count; i++)
            {
                dtBilling.Rows[i].SetAdded();
            }
            dtpReceived.Value = DateTime.Now;
            dtpEntered.Value = DateTime.Now;
        }

        private void btnCancelTest_Click(object sender, EventArgs e)
        {
            try
            {
                bsLogTests.RemoveCurrent();
                if (dtLogTests.Rows.Count == 0)
                    btnCancelTest.Enabled = false;
            }
            catch
            { }
        }

        private void btnCheckRows_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dtLogTests.Rows.Count.ToString());
        }

        private void cboPONo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dtrLogTests_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            if (e.ItemIndex < dtLogTests.Rows.Count)
            {
                switch (e.Control.Name)
                {
                    case "txtCmpyCodeLT":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["CompanyCode"].ToString();
                        }
                        catch { }
                        break;
                    case "txtSC":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["ServiceCode"].ToString();
                        }
                        catch { }
                        break;
                    case "lblSCDesc":
                        try
                        {
                            e.Value = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]), dtSC);
                        }
                        catch { }
                        break;
                    case "txtProtocolNo":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["ProtocolNo"];
                        }
                        catch { }
                        break;
                    case "dtpStartDate":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["StartDate"];
                        }
                        catch { }
                        break;
                    case "dtpEndDate":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["EndDate"];
                        }
                        catch { }
                        break;
                    case "txtPONo":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["PONo"];
                        }
                        catch { }
                        break;
                    case "txtSamples":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["TestSamples"];
                        }
                        catch { }
                        break;
                    case "txtSlashNos":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["Slashes"];
                        }
                        catch { }
                        break;
                    case "txtBillQty":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["BillQty"].ToString();
                        }
                        catch { }
                        break;
                    case "txtQuoteNo":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["QuotationNo"];
                        }
                        catch { }
                        break;
                    case "txtBookNo":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["BookNo"].ToString();
                        }
                        catch { }
                        break;
                    case "lblECChamber":
                        try
                        {
                            e.Value = "EC Chamber";
                        }
                        catch { }
                        break;
                    case "chkEC":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["EC"].ToString() == "True")
                            {
                                e.Value = 1;
                            }
                            else
                            {
                                e.Value = 0;
                            }
                        }
                        catch { }
                        break;
                    case "lblECType":
                        e.Value = "Type :";
                        break;
                    case "rdoECDD":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "D")
                            {
                                e.Value = true;
                            }
                            else
                            {
                                e.Value = false;
                            }
                        }
                        catch { }
                        break;
                    case "rdoECWW":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "W")
                            {
                                e.Value = true;
                            }
                            else
                            {
                                e.Value = false;
                            }
                        }
                        catch { }
                        break;
                    case "rdoECMM":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "M")
                            {
                                e.Value = true;
                            }
                            else
                            {
                                e.Value = false;
                            }
                        }
                        catch { }
                        break;
                    case "rdoECYY":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "Y")
                            {
                                e.Value = true;
                            }
                            else
                            {
                                e.Value = false;
                            }
                        }
                        catch { }
                        break;
                    case "lblECLength":
                        e.Value = "Length :";
                        break;
                    case "txtECLength":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["ECLength"].ToString();
                        }
                        catch { }
                        break;
                    case "lblECEndDate":
                        e.Value = "End Date :";
                        break;
                    case "dtpECEndDate":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["ECEndDate"];
                        }
                        catch { }
                        break;
                    case "lblDateSampled":
                        e.Value = "Date Sampled:";
                        break;
                    case "dtpDateSampled":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["DateSampled"];
                        }
                        catch { }
                        break;
                    case "lblReportNo":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["ReportNo"].ToString();
                        }
                        catch { }
                        break;
                    case "lblLastLabelFirst":
                        e.Value = "";
                        break;
                    case "lblLastLabelSec":
                        e.Value = "";
                        break;
                    case "lblECEndDateX":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["EC"].ToString() == "False")
                            {
                                e.Value = "X";
                                dtrLogTests.ItemTemplate.Controls["lblECEndDateX"].Visible = true;
                            }
                            else
                            {
                                e.Control.Visible = false;
                            }
                        }
                        catch { }
                        break;
                    case "lblDateSampledX":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["EC"].ToString() == "False")
                            {
                                e.Value = "X";
                            }
                            else
                            {
                                e.Control.Visible = false;
                            }
                        }
                        catch { }
                        break;
                    case "txtQuote":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["QuoteFlag"];
                        }
                        catch { }
                        break;
                    case "txtAddNotes":
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["AddlNotes"];
                        }
                        catch { }
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bsSamples.EndEdit();
            for (int i = 0; i < dtSamples.Rows.Count; i++)
            {
                MessageBox.Show(dtSamples.Rows[i].RowState.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bsLogTests.EndEdit();
            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                MessageBox.Show(dtLogTests.Rows[i].RowState.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bsSampleSC.EndEdit();
            for (int i = 0; i < dtSampleSC.Rows.Count; i++)
            {
                MessageBox.Show(dtSampleSC.Rows[i].RowState.ToString());
            }
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvContacts.Visible = false; dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false;
            }
        }

        private void dgvSamples_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                if (dgvSamples.CurrentCell.OwningColumn.Name == "CopyRow")
                    CopyRow();
            }
            if (dgvSamples.CurrentCell.OwningColumn.Name == "ExtData")
                GetSlashExtData();
        }

        private void dgvSC_DoubleClick(object sender, EventArgs e)
        {
            ((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text = ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).CurrentRow.Cells[0].Value.ToString();
            ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = false;
            ((Label)dtrLogTests.CurrentItem.Controls["lblSCDesc"]).Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text), dtSC);
        }

        private void btnSC_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (((TextBox)dtrLogTests.CurrentItem.Controls["txtQuoteNo"]).Text != "")
                    return;

                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = true;
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Columns[0].Width = 47;
            }
        }

        private void txtSC_Enter(object sender, EventArgs e)
        {
            if (nMode == 0 || ((TextBox)dtrLogTests.CurrentItem.Controls["txtQuoteNo"]).Text != "")
                return;

            ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = true;
            ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Columns[0].Width = 47;
        }

        private void dgvSC_Leave(object sender, EventArgs e)
        {
            ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = false;
        }

        private void txtSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0 || ((TextBox)dtrLogTests.CurrentItem.Controls["txtQuoteNo"]).Text != "")
                e.Handled = true;

            if (e.KeyChar == 13)
            {
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = false;
            }
            else
                ((Label)dtrLogTests.CurrentItem.Controls["lblSCDesc"]).Text = "";
        }

        private void dtrLogTests_CurrentItemIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = false;
                ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).Visible = false;
                ((Button)dtrLogTests.CurrentItem.Controls["btnSC"]).Select();
                if (((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "" && ((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "0")
                {
                    if (PSSClass.FinalReports.RptDateApproved(Convert.ToInt32(((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text)))
                        dtrLogTests.CurrentItem.Enabled = false;
                    else
                        dtrLogTests.CurrentItem.Enabled = true;
                }
            }
            catch { }
        }

        private void txtSC_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    DataView dvwSC;
                    string strSC = ((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text.Trim().Replace("'", "''");
                    dvwSC = new DataView(dtSCMaster, "ServiceCode like '%" + strSC + "%'", "ServiceCode", DataViewRowState.CurrentRows);
                    ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).DataSource = dvwSC;
                }
                catch { }
            }
        }

        private void btnDelSCS_Click(object sender, EventArgs e)
        {
            if (nMode != 0 && dgvSampleSC.Rows.Count > 0)
            {
                dgvSampleSC.Rows.RemoveAt(dgvSampleSC.CurrentRow.Index);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bsBilling.EndEdit();
            for (int i = 0; i < dtBilling.Rows.Count; i++)
            {
                MessageBox.Show(dtBilling.Rows[i].RowState.ToString());
            }
        }

        private void dtrLogTests_DeletingItems(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterAddRemoveItemsCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;

            if (dtLogTests.Rows[dtrLogTests.CurrentItemIndex].RowState.ToString() != "Deleted")
            {
                string strSC = ((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text;

                List<string> strLSC = new List<string>();
                strLSC.Add(strSC);

                //This block deletes all entries in the Slash/SC datagridview when a Slash is changed
                if (dgvSampleSC.Rows.Count != 0)
                {
                    for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                    {
                        for (int j = 0; j < strLSC.Count; j++)
                        {
                            if (dgvSampleSC.Rows[i].Cells["SC"].Value.ToString() == strLSC[j].ToString())
                            {
                                dgvSampleSC.Rows[i].Selected = true;
                            }
                        }
                    }
                    foreach (DataGridViewRow row in dgvSampleSC.SelectedRows)
                    {
                        dgvSampleSC.Rows.Remove(row);
                    }
                }
                //dtLogTests.AcceptChanges();
                for (int i = 0; i < dgvTests.Rows.Count; i++)
                {
                    if (dgvTests.Rows[i].Cells["ServiceCode"].Value.ToString() == strSC)
                    {
                        dgvTests.Rows[i].Cells["BillQty"].Value = 0;
                        dgvTests.Rows[i].Cells["SelectedTest"].Value = 0;
                        dgvTests.Rows[i].Cells["Rush"].Value = 0;
                    }
                }
            }
        }

        private void dgvSC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void lblSCDesc_MouseHover(object sender, EventArgs e)
        {
            ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = false;
        }

        private void pnlSlashExtData_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                mouseDown = false;
        }

        private void pnlSlashExtData_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlSlashExtData.Location = PointToClient(this.pnlSlashExtData.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlSlashExtData_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void btnCloseSlashExt_Click(object sender, EventArgs e)
        {
            pnlSlashExtData.Visible = false; pnlRecord.Enabled = true;

            if (nMode != 0)
            {
                List<string> strLabel = new List<string>();
                List<string> strValue = new List<string>();
                for (int i = 0; i < dgvSlashExtData.Rows.Count - 1; i++)
                {
                    if (dgvSlashExtData.Rows[i].Cells["Label"].Value != null && dgvSlashExtData.Rows[i].Cells["Label"].Value.ToString() != "")
                        strLabel.Add(dgvSlashExtData.Rows[i].Cells["Label"].Value.ToString());
                    if (dgvSlashExtData.Rows[i].Cells["Value"].Value != null && dgvSlashExtData.Rows[i].Cells["Value"].Value.ToString() != "")
                        strValue.Add(dgvSlashExtData.Rows[i].Cells["Value"].Value.ToString());
                }
                int nM = 0;
                for (int i = 0; i < dtSlashExtData.Rows.Count; i++)
                {
                    if (txtSlashExt.Text == dtSlashExtData.Rows[i]["SlashNo"].ToString())
                    {
                        nM = 1;
                        break;
                    }
                }
                if (nM == 0)
                {
                    for (int i = 0; i < dgvSlashExtData.Rows.Count; i++)
                    {
                        if (dgvSlashExtData.Rows[i].Cells["Value"].Value != null && dgvSlashExtData.Rows[i].Cells["Value"].Value.ToString().Trim() != "")
                        {
                            DataRow dR;
                            dR = dtSlashExtData.NewRow();
                            dR["SlashNo"] = txtSlashExt.Text;
                            dR["ExtDataLabel"] = dgvSlashExtData.Rows[i].Cells["Label"].Value;
                            dR["ExtDataValue"] = dgvSlashExtData.Rows[i].Cells["Value"].Value;
                            dtSlashExtData.Rows.Add(dR);
                        }
                    }
                }
                else
                {
                    // Create an array for the key values to find. 
                    object[] fkeys = new object[2];

                    for (int i = 0; i < dgvSlashExtData.Rows.Count - 1; i++)
                    {
                        dtSlashExtData.PrimaryKey = new DataColumn[] { dtSlashExtData.Columns["SlashNo"], dtSlashExtData.Columns["ExtDataLabel"] };

                        fkeys[0] = txtSlashExt.Text;
                        fkeys[1] = dgvSlashExtData.Rows[i].Cells["Label"].Value;

                        DataRow foundRow = dtSlashExtData.Rows.Find(fkeys);
                        if (foundRow == null)
                        {
                            DataRow dR;
                            dR = dtSlashExtData.NewRow();
                            dR["SlashNo"] = txtSlashExt.Text;
                            dR["ExtDataLabel"] = dgvSlashExtData.Rows[i].Cells["Label"].Value;
                            dR["ExtDataValue"] = dgvSlashExtData.Rows[i].Cells["Value"].Value;
                            dtSlashExtData.Rows.Add(dR);
                        }
                        else
                        {
                            int n = dtSlashExtData.Rows.IndexOf(foundRow);
                            dtSlashExtData.Rows[n]["ExtDataValue"] = dgvSlashExtData.Rows[i].Cells["Value"].Value;
                        }
                    }
                }
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            bsSlashExtData.EndEdit();
            for (int i = 0; i < dtSlashExtData.Rows.Count; i++)
            {
                MessageBox.Show(dtSlashExtData.Rows[i].RowState.ToString());
            }
        }

        private void SamplesLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (m_oWorker != null && m_oWorker.IsBusy)
            //{
            //    m_oWorker.CancelAsync();
            //    bWSw = 1;
            //}

            dtLogMaster.Dispose(); dtLogFM.Dispose(); dtSponsors.Dispose(); dtContacts.Dispose();
            dtLogTests.Dispose(); dtLogTestsDel.Dispose(); dtSamples.Dispose();
            dtSamplesAddl.Dispose(); dtSC.Dispose(); dtSCMaster.Dispose();
            dtSampleSC.Dispose(); dtPONo.Dispose(); dtBilling.Dispose();
            dtSCExtData.Dispose(); dtSlashExtData.Dispose();
            try
            {
                crDoc.Close(); crDoc.Dispose(); this.Dispose();
                //GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();  GC.WaitForPendingFinalizers();
            }
            catch { }

            if (nFR == 5)
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(TATDashBoard))
                    {
                        form.Activate();
                        form.BringToFront();
                        form.WindowState = FormWindowState.Maximized;
                        break;
                    }
                }
            }
        }

        private void txtSlash1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt1.Text == "")
                e.Handled = true;
        }

        private void txtSlash2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt2.Text == "")
                e.Handled = true;
        }

        private void txtSlash3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt3.Text == "")
                e.Handled = true;
        }

        private void txtSlash4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt4.Text == "")
                e.Handled = true;
        }

        private void txtSlash5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt5.Text == "")
                e.Handled = true;
        }

        private void txtSlash6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt6.Text == "")
                e.Handled = true;
        }

        private void txtSlash7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt7.Text == "")
                e.Handled = true;
        }

        private void txtSlash8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt8.Text == "")
                e.Handled = true;
        }

        private void txtSlash9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt9.Text == "")
                e.Handled = true;
        }

        private void txtSlash10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSlashExt10.Text == "")
                e.Handled = true;
        }

        private void txtSC1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt1.Text == "")
                e.Handled = true;
        }

        private void txtSC2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt2.Text == "")
                e.Handled = true;
        }

        private void txtSC3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt3.Text == "")
                e.Handled = true;
        }

        private void txtSC4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt4.Text == "")
                e.Handled = true;
        }

        private void txtSC5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt5.Text == "")
                e.Handled = true;
        }

        private void txtSC6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt6.Text == "")
                e.Handled = true;
        }

        private void txtSC7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt7.Text == "")
                e.Handled = true;
        }

        private void txtSC8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt8.Text == "")
                e.Handled = true;
        }

        private void txtSC9_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt9.Text == "")
                e.Handled = true;
        }

        private void txtSC10_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (lblSCExt10.Text == "")
                e.Handled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bsSCExtData.EndEdit();
            for (int i = 0; i < dtSCExtData.Rows.Count; i++)
            {
                MessageBox.Show(dtSCExtData.Rows[i].RowState.ToString());
            }
        }

        private void btnFAXEMail_Click(object sender, EventArgs e)
        {
            byte nReply = 0;
            Int16 nRNo = 0;
            if (nMode == 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to send this Acknowledgement " + Environment.NewLine + "Notification to the Sponsor?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.Yes)
                {
                    nReply = 1;
                    nRNo = PSSClass.Samples.LogRevNo(Convert.ToInt32(txtLogNo.Text));
                    if (nRNo != 99)
                    {
                        DialogResult dRev = new DialogResult();
                        dRev = MessageBox.Show("Are you sending a revised Acknowledgement Notification?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dRev == DialogResult.Yes)
                        {
                            nRNo += 1;
                            PSSClass.Samples.UpdLogRevNo(Convert.ToInt32(txtLogNo.Text), nRNo);
                        }
                    }
                    else
                    {
                        nRNo = 0;
                        PSSClass.Samples.UpdLogRevNo(Convert.ToInt32(txtLogNo.Text), 0);
                    }
                }

                LabRpt rpt = new LabRpt();
                rpt.rptName = "Acknowledgement";
                rpt.WindowState = FormWindowState.Maximized;
                rpt.CmpyCode = txtCmpyCode.Text; 
                rpt.nLogNo = Convert.ToInt32(txtLogNo.Text);
                rpt.SpID = Convert.ToInt16(txtSponsorID.Text);
                rpt.nRevNo = nRNo;
                try
                {
                    rpt.Show();
                }
                catch
                {
                    MessageBox.Show("Report cannot be loaded." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (nReply == 0)
                    return;

                rpt.Close(); rpt.Dispose();

                string pdfFile = "";

                if (nRNo == 0)
                    pdfFile = @"\\PSAPP01\IT Files\PTS\PDF Reports\Acknowledgements\" + DateTime.Now.Year.ToString() + "\\" + "A-" + Convert.ToInt32(txtLogNo.Text).ToString("000000")  + ".pdf";
                else
                    pdfFile = @"\\PSAPP01\IT Files\PTS\PDF Reports\Acknowledgements\" + DateTime.Now.Year.ToString() + "\\" + "A-" + Convert.ToInt32(txtLogNo.Text).ToString("000000") + "-R" + nRNo.ToString().Trim() + ".pdf";

                string strText = "", strEMail = "";
                string strCFName = "";
                string strSignature = ReadSignature();

                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }            
                SqlCommand sqlcmd = new SqlCommand();
                SqlDataReader sqldr;
                sqlcmd = new SqlCommand("SELECT FirstName FROM Contacts WHERE ContactID = " + Convert.ToInt16(txtContactID.Text), sqlcnn);
                sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strCFName = sqldr.GetValue(0).ToString();
                }
                sqldr.Close(); sqlcmd.Dispose();

                sqlcmd = new SqlCommand("SELECT EMailAddress FROM ContactEMAddresses WHERE ContactID = " + Convert.ToInt16(txtContactID.Text) +
                                        " AND AckReports = 1", sqlcnn);
                sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strEMail = sqldr.GetValue(0).ToString();
                    strEMail.Replace(";", ",");
                }
                sqldr.Close(); sqlcmd.Dispose();

                //Check for Resell Service Code
                string strResell = "";
                int nSC = Convert.ToInt16(((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
                sqlcmd = new SqlCommand("SELECT ResellService FROM ServiceCodes WHERE ServiceCode = " + nSC, sqlcnn);
                sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strResell = sqldr.GetValue(0).ToString();      
                }
                sqldr.Close(); sqlcmd.Dispose();
                //

                Outlook.Application oApp = new Outlook.Application();

                // Create a new mail item.

                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                // Set HTMLBody. 
                //add the body of the email
                if (strResell != "")
                {
                    strText = "Dear " + strCFName + ", <br/><br/>" + 
                              "Thank you for your order of <b>" + txtArticle.Text + "</b> for " + ((Label)dtrLogTests.CurrentItem.Controls["lblSCDesc"]).Text + ".<br/><br/> " + 
                              "Please see the attached <b>Acknowledgment Notification</b>. In this attachment you will find important information and estimated shipping <br/> "+
                              "date for your order. It is critical for you to review it and immediately inform us in writing of any questions or corrections that you have. <br/><br/> " +
                              "Non-acknowledgement of this document is considered by PRINCE as the Sponsor's acceptance and agreement of the accuracy and correctness <br/> " +
                              "of our records and releases PRINCE from any further liabilities. Failure by Sponsor to return PRINCE's Terms, Conditions and Pricing Policy is taken <br/> " +
                              "by PRINCE as acceptance of same by Sponsor. Thank you for your support of <b>Prince Sterilization Services</b>.<br/><br/>";
                }
                else
                strText = "Dear " + strCFName + ", <br/><br/>" +  
                    "Thank you for your submission of <b>" + txtArticle.Text + "</b> for <b>" + ((Label)dtrLogTests.CurrentItem.Controls["lblSCDesc"]).Text + " </b> testing.<br/> " + 
                    "We acknowledge receipt of the samples as described in the attached <b>Acknowledgment Notification</b>. <br/><br/> " +
                    "In this attachment you will find important information that provides the estimated start and completion <br/> " +
                    "dates of your testing as well as details that will appear in the Final Report. It is critical for you to review it <br/> " +
                    "and immediately inform us in writing of any questions or corrections that you have. <br/><br/> " +
                    "Non-acknowledgement of this document is considered by PRINCE as the Sponsor's acceptance and agreement <br/>" + 
                    "of the accuracy and correctness of our records and releases PRINCE from any further liabilities. Failure by <br/>" + 
                    "Sponsor to return PRINCE's Terms, Conditions and Pricing Policy is taken by Prince Sterilization Services as acceptance of same by <br/> " + 
                    "Sponsor. Thank you for your support of <b>Prince Sterilization Services.</b><br/><br/>";

                oMsg.HTMLBody = "<FONT face=\"Arial\">";
                oMsg.HTMLBody += strText.Trim() + strSignature;
                //Add an attachment.
                oMsg.Attachments.Add(pdfFile);
                //oMsg.Attachments.Add(crafFile);
                //Subject line
                if (dgvSamples.Rows[0].Cells[4].Value.ToString().Trim() != "")
                    oMsg.Subject = "PSS: " + Convert.ToInt32(txtLogNo.Text).ToString("000000") + " Lot: " + dgvSamples.Rows[0].Cells[4].Value.ToString() + " Article: " + txtArticle.Text.Trim();
                else
                    oMsg.Subject = "PSS: " + Convert.ToInt32(txtLogNo.Text).ToString("000000") + " Article: " + txtArticle.Text.Trim();
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

                string[] EMAddresses = strEMail.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < EMAddresses.Length; i++)
                {
                    if (EMAddresses[i].Trim() != "")
                    {
                        Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                        oRecip.Resolve();
                    }
                }
                //oRecip.Resolve();
                oMsg.Display();

                // Send.
                //oMsg.Send();
                //((Outlook._MailItem)oMsg).Send();
                
                // Clean up.
                //oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;

                sqlcmd.Dispose();
                sqlcnn.Close(); sqlcnn.Dispose();
            }               
        }

        private string ReadSignature()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            string signature = string.Empty;
            DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
            if (diInfo.Exists)
            {
                FileInfo[] fiSignature = diInfo.GetFiles("*.htm");
                if (fiSignature.Length > 0)
                {
                    StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
                    signature = sr.ReadToEnd();

                    if (!string.IsNullOrEmpty(signature))
                    {
                        string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
                        signature = signature.Replace(fileName + "_files/", appDataDir + "/" + fileName + "_files/");
                    }
                }
            }
            return signature;
        }
        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            mskDateCancelled.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            pnlCalendar.Visible = false;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void tabComments_MouseHover(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            bsLogFM.EndEdit();
            for (int i = 0; i < dtLogFM.Rows.Count; i++)
            {
                MessageBox.Show(dtLogFM.Rows[i].RowState.ToString());
            }
        }

        private void mskDateCancelled_Click(object sender, EventArgs e)
        {
            if (nMode != 0 && chkCancelled.Checked == true)
                pnlCalendar.Visible = true;
        }

        private void mskDateCancelled_Enter(object sender, EventArgs e)
        {
            if (nMode != 0 && chkCancelled.Checked == true)
                pnlCalendar.Visible = true;
        }

        private void mskDateCancelled_KeyDown(object sender, KeyEventArgs e)
        {
            if (nMode != 0 && chkCancelled.Checked == true)
            {
                pnlCalendar.Visible = true; 
            }
            e.SuppressKeyPress = true; 
        }

        private void txtOtherStorage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (rdoOther.Checked == false)
                e.Handled = true;
        }

        private void txtGBLSCExt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSCExt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSCSpExt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtGBLSlashExt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSlashExt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSlashSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSlashSCSp_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void rdoRAmbient_CheckedChanged(object sender, EventArgs e)
        {
            txtRecCode.Text = "1";
        }

        private void rdoRefrigerator_CheckedChanged(object sender, EventArgs e)
        {
            txtStorageCode.Text = "2";
        }

        private void rdoFreezer20_CheckedChanged(object sender, EventArgs e)
        {
            txtStorageCode.Text = "3";
        }

        private void rdoFreezer80_CheckedChanged(object sender, EventArgs e)
        {
            txtStorageCode.Text = "4";
        }

        private void rdoOther_CheckedChanged(object sender, EventArgs e)
        {
            txtStorageCode.Text = "5";
        }

        private void rdoIcePack_CheckedChanged(object sender, EventArgs e)
        {
            txtRecCode.Text = "2";
        }

        private void rdoDryIce_CheckedChanged(object sender, EventArgs e)
        {
            txtRecCode.Text = "3";
        }

        private void btnAddSample_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
                CopyRow();
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                dgvSponsors.Visible = false;
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                dgvContacts.Visible = false;
        }

        private void btnPasteExcel_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsData(ClipboardFormat))
            {
                string strCol = dgvSamples.CurrentCell.OwningColumn.Name;
                string s = Clipboard.GetText();
                string[] lines = s.Replace("\n", "").Split('\r');
                string[] fields;
                int row = dgvSamples.CurrentCell.RowIndex;
                int column = dgvSamples.CurrentCell.ColumnIndex;
                int nR = dtSamples.Rows.Count;
                foreach (string l in lines)
                {
                    fields = l.Split('\t');
                    foreach (string f in fields)
                    {
                        dgvSamples[column, row].Value = f;
                        if (row < nR)
                            row++;
                    }
                }
            }
        }

        private void dgvSamples_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (dgvSamples.IsCurrentCellInEditMode == false)
                {
                    if (dgvSamples.CurrentCell.ColumnIndex == 5)
                    {
                        try
                        {
                            int nR = dgvSamples.CurrentCell.RowIndex + 1;
                            dgvSamples.CurrentCell = dgvSamples.Rows[nR].Cells[0];
                            SendKeys.Send("{LEFT}");
                            SendKeys.Send("{LEFT}");
                            SendKeys.Send("{LEFT}");
                            SendKeys.Send("{LEFT}");
                        }
                        catch {}
                    }
                    else
                        SendKeys.Send("{RIGHT}");
                }
            }
        }

        private void btnPrintCOC_Click(object sender, EventArgs e)
        {
            if (cboChainOfCustody.SelectedIndex <= 1 && chkCtrldSubs.Checked == false)
            {
                MessageBox.Show("Invalid selection. Controlled substance must be selected.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
                        
            LabRpt rpt = new LabRpt();
            if (cboChainOfCustody.SelectedIndex == 0)
            {
                rpt.rptName = "CSCOC";
                rpt.WindowState = FormWindowState.Maximized;
                rpt.nLogNo = Convert.ToInt32(txtLogNo.Text);
                rpt.COCSlash = "";
            }
            else if (cboChainOfCustody.SelectedIndex == 1)
            {
                rpt.rptName = "CSCOC";
                rpt.WindowState = FormWindowState.Maximized;
                rpt.nLogNo = Convert.ToInt32(txtLogNo.Text);
                rpt.COCSlash = dgvSamples.Rows[dgvSamples.CurrentCell.RowIndex].Cells["SlashNo"].Value.ToString();
            }
            else if (cboChainOfCustody.SelectedIndex == 2)
            {
                rpt.rptName = "GLPCOC";
                rpt.WindowState = FormWindowState.Maximized;
                rpt.nLogNo = Convert.ToInt32(txtLogNo.Text);
                rpt.COCSlash = "";
            }
            else if (cboChainOfCustody.SelectedIndex == 3)
            {
                rpt.rptName = "GLPCOC";
                rpt.WindowState = FormWindowState.Maximized;
                rpt.nLogNo = Convert.ToInt32(txtLogNo.Text);
                rpt.COCSlash = dgvSamples.Rows[dgvSamples.CurrentCell.RowIndex].Cells["SlashNo"].Value.ToString(); ;
            }

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void btnCloseExtSC_Click(object sender, EventArgs e)
        {
            pnlSCExtData.Visible = false; pnlRecord.Enabled = true;
            if (nMode != 0)
            {
                //Check if Study Data are entered
                if (txtStudyNo.Text.Trim() == "")
                    txtStudyNo.Text = "0";
                if (cboStudyDir.SelectedIndex == -1)
                    cboStudyDir.SelectedIndex = 0;
                //Check if Extended Data are entered
                List<string> strLabel = new List<string>();
                List<string> strValue = new List<string>();
                for (int i = 0; i < dgvSCExtData.Rows.Count - 1; i++)
                {
                    if (dgvSCExtData.Rows[i].Cells["Label"].Value != null && dgvSCExtData.Rows[i].Cells["Label"].Value.ToString() != "")
                        strLabel.Add(dgvSCExtData.Rows[i].Cells["Label"].Value.ToString());
                    if (dgvSCExtData.Rows[i].Cells["Value"].Value != null && dgvSCExtData.Rows[i].Cells["Value"].Value.ToString() != "")
                        strValue.Add(dgvSCExtData.Rows[i].Cells["Value"].Value.ToString());
                }
                if (strLabel.Count == 0)
                {
                    if (txtStudyNo.Text != "0" || cboStudyDir.SelectedIndex != 0)
                    {
                        if (dtSCExtData == null || dtSCExtData.Rows.Count == 0)
                        {
                            DataRow dR;
                            dR = dtSCExtData.NewRow();
                            dR["ServiceCode"] = Convert.ToInt16(txtSCExt.Text);
                            dR["StudyNo"] = Convert.ToInt32(txtStudyNo.Text.Trim());
                            dR["StudyDirID"] = cboStudyDir.SelectedValue;
                            dR["SCExtDataLabel"] = "";
                            dR["SCExtDataValue"] = "";
                            dR["PrtNotes"] = txtPrtNotes.Text.Trim();
                            dR["NonPrtNotes"] = txtNonPrtNotes.Text.Trim();
                            dtSCExtData.Rows.Add(dR);
                            return;
                        }
                        else
                        {
                            byte bSM = 0;
                            for (int i = 0; i < dtSCExtData.Rows.Count; i++)
                            {
                                if (dtSCExtData.Rows[i].RowState.ToString() != "Deleted" && dtSCExtData.Rows[i]["ServiceCode"].ToString() == txtSCExt.Text)
                                {
                                    bSM = 1;
                                    break;
                                }
                            }
                            if (bSM == 0)
                            {
                                DataRow dR;
                                dR = dtSCExtData.NewRow();
                                dR["ServiceCode"] = Convert.ToInt16(txtSCExt.Text);
                                dR["StudyNo"] = Convert.ToInt32(txtStudyNo.Text.Trim());
                                dR["StudyDirID"] = cboStudyDir.SelectedValue;
                                dR["SCExtDataLabel"] = "";
                                dR["SCExtDataValue"] = "";
                                dR["PrtNotes"] = txtPrtNotes.Text.Trim();
                                dR["NonPrtNotes"] = txtNonPrtNotes.Text.Trim();
                                dtSCExtData.Rows.Add(dR);
                                return;
                            }
                        }
                    }
                    else
                    {
                        DataRow[] foundRows;
                        foundRows = dtSCExtData.Select("ServiceCode = " + ((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
                        if (foundRows.Length == 1)
                        {
                            for (int i = 0; i < dtSCExtData.Rows.Count; i++)
                            {
                                if (dtSCExtData.Rows[i].RowState.ToString() != "Deleted" && txtSCExt.Text == dtSCExtData.Rows[i]["ServiceCode"].ToString())
                                {
                                    dtSCExtData.Rows[i]["StudyNo"] = 0;
                                    dtSCExtData.Rows[i]["StudyDirID"] = 0; 
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (dtSCExtData == null || dtSCExtData.Rows.Count == 0)
                    {
                        for (int i = 0; i < dgvSCExtData.Rows.Count - 1; i++)
                        {
                            DataRow dR;
                            dR = dtSCExtData.NewRow();
                            dR["ServiceCode"] = Convert.ToInt16(txtSCExt.Text);
                            dR["StudyNo"] = Convert.ToInt32(txtStudyNo.Text.Trim());
                            dR["StudyDirID"] = cboStudyDir.SelectedValue;
                            dR["SCExtDataLabel"] = dgvSCExtData.Rows[i].Cells["Label"].Value.ToString();
                            dR["SCExtDataValue"] = dgvSCExtData.Rows[i].Cells["Value"].Value.ToString();
                            dR["PrtNotes"] = txtPrtNotes.Text.Trim();
                            dR["NonPrtNotes"] = txtNonPrtNotes.Text.Trim();
                            dtSCExtData.Rows.Add(dR);
                        }
                    }
                    else
                    {
                        dtSCExtData.PrimaryKey = new DataColumn[] { dtSCExtData.Columns["ServiceCode"], dtSCExtData.Columns["SCExtDataLabel"] };
                        fkeys[0] = txtSCExt.Text;
                        for (int i = 0; i < dgvSCExtData.Rows.Count - 1; i++)
                        {
                            if (dgvSCExtData.Rows[i].Cells["Value"].Value.ToString() != "")
                            {
                                fkeys[1] = dgvSCExtData.Rows[i].Cells["Label"].Value;
                                string x = dgvSCExtData.Rows[i].Cells["Value"].Value.ToString();

                                DataRow foundRow = dtSCExtData.Rows.Find(fkeys);
                                if (foundRow == null)
                                {
                                    DataRow dR;
                                    dR = dtSCExtData.NewRow();
                                    dR["ServiceCode"] = Convert.ToInt16(txtSCExt.Text);
                                    dR["StudyNo"] = Convert.ToInt32(txtStudyNo.Text.Trim());
                                    dR["StudyDirID"] = cboStudyDir.SelectedValue;
                                    dR["SCExtDataLabel"] = dgvSCExtData.Rows[i].Cells["Label"].Value.ToString();
                                    dR["SCExtDataValue"] = dgvSCExtData.Rows[i].Cells["Value"].Value.ToString();
                                    dR["PrtNotes"] = txtPrtNotes.Text.Trim();
                                    dR["NonPrtNotes"] = txtNonPrtNotes.Text.Trim();
                                    dtSCExtData.Rows.Add(dR);
                                }
                                else
                                {
                                    int n = dtSCExtData.Rows.IndexOf(foundRow);
                                    dtSCExtData.Rows[n]["StudyNo"] = Convert.ToInt32(txtStudyNo.Text.Trim());
                                    dtSCExtData.Rows[n]["StudyDirID"] = cboStudyDir.SelectedValue;
                                    dtSCExtData.Rows[n]["SCExtDataValue"] = x;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void txtSponsorID_TextChanged(object sender, EventArgs e)
        {
            if (txtSponsorID.Text.Trim() == "")
            {
                txtSponsor.Text = "";
            }
        }

        private void dgvTests_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (nMode != 0)
            {
                if (dgvTests.CurrentCell.OwningColumn.Name.ToString() == "BillQty")
                {
                    int i;

                    if (!int.TryParse(Convert.ToString(e.FormattedValue), out i))
                    {
                        MessageBox.Show("Entry must be numeric.");
                        e.Cancel = true;
                        SendKeys.Send("{ESC}");
                    }
                }
            }
        }

        private void dgvSlashExtData_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;

            dgvSlashExtData.NotifyCurrentCellDirty(true);
        }

        private void GetSlashExtData()
        {
            DataTable dt = PSSClass.Samples.ExExtDataLabels();
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No defined labels.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var stringArr = dt.AsEnumerable().Select(r => r.Field<string>("DataLabelDesc")).ToArray();

            dgvSlashExtData.Columns.Clear(); dgvSlashExtData.Rows.Clear();

            DataGridViewComboBoxColumn cboLabels = new DataGridViewComboBoxColumn();
            DataGridViewTextBoxColumn txtValues = new DataGridViewTextBoxColumn();

            dgvSlashExtData.Columns.Add(cboLabels);
            dgvSlashExtData.Columns.Add(txtValues);
            dgvSlashExtData.Columns[0].Name = "Label";
            dgvSlashExtData.Columns[1].Name = "Value";

            dgvSlashExtData.Columns["Label"].Width = 250;
            dgvSlashExtData.Columns["Value"].Width = 200;
            dgvSlashExtData.Columns["Label"].HeaderText = "DATA LABEL";
            dgvSlashExtData.Columns["Value"].HeaderText = "VALUE";
            StandardDGVSetting(dgvSlashExtData);

            ((DataGridViewComboBoxColumn)dgvSlashExtData.Columns["Label"]).DataSource = stringArr;

            txtGBLSlashExt.Text = txtLogNo.Text;
            txtSlashExt.Text = dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString();
            pnlSlashExtData.Visible = true; pnlSlashExtData.BringToFront(); pnlSlashExtData.Location = new Point(178, 101);
            pnlSlashExt.Enabled = true; pnlRecord.Enabled = false;

            if (nMode == 0)
                dgvSlashExtData.ReadOnly = true;
            else
                dgvSlashExtData.ReadOnly = false;

            DataRow[] foundRows;
            foundRows = dtSlashExtData.Select("SlashNo = '" + dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString() + "'");
            if (foundRows.Length == 0)
            {
            }
            else
            {
                DataTable dtX = new DataTable();
                dtX = foundRows.CopyToDataTable();
                dgvSlashExtData.RowCount = dtX.Rows.Count + 1;
                try
                {
                    for (int r = 0; r < dtX.Rows.Count; r++)
                    {
                        dgvSlashExtData.Rows[r].Cells["Label"].Value = dtX.Rows[r]["ExtDataLabel"].ToString();
                        dgvSlashExtData.Rows[r].Cells["Value"].Value = dtX.Rows[r]["ExtDataValue"].ToString();
                    }
                }
                catch { }
            }
        }

        private void dgvSamples_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                object value = dgvSamples.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!((DataGridViewComboBoxColumn)dgvSamples.Columns[e.ColumnIndex]).Items.Contains(value))
                {
                    ((DataGridViewComboBoxColumn)dgvSamples.Columns[e.ColumnIndex]).Items.Add(value);
                    e.ThrowException = false;
                }
            }
        }

        private void dgvSampleSC_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                object value = dgvSampleSC.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!((DataGridViewComboBoxColumn)dgvSampleSC.Columns[e.ColumnIndex]).Items.Contains(value))
                {
                    e.ThrowException = false;
                }
            }
        }

        private void btnLSPrinter_Click(object sender, EventArgs e)
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

            crDoc = new ReportDocument();
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            SqlDataReader sqldr;
            string rpt = "";

            if (txtSponsor.Text.IndexOf("INGREDION") != -1)
            {
                byte nM = 0; byte nB = 0;
                for (int i = 0; i < dtLogTests.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dtLogTests.Rows[i]["ServiceCode"].ToString()) > 1000 && Convert.ToInt16(dtLogTests.Rows[i]["ServiceCode"].ToString()) < 2000)
                    {
                        nM = 1;
                        break;
                    }
                }
                if (txtArticle.Text.Trim() == "503130")
                {
                    rpt = @"\\gblnj4\GIS\Reports\" + "LoginSheet_503130.rpt";
                    nB = 1;
                }
                else if (txtArticle.Text.Trim() == "503136")
                {
                    rpt = @"\\gblnj4\GIS\Reports\" + "LoginSheet_503136.rpt";
                    nB = 1;
                }
                else if (txtArticle.Text.Trim() == "501116")
                {
                    rpt = @"\\gblnj4\GIS\Reports\" + "LoginSheet_501116.rpt";
                    nB = 1;
                }
                else if (txtArticle.Text.Trim() == "501901")
                {
                    rpt = @"\\gblnj4\GIS\Reports\" + "LoginSheet_501901.rpt";
                    nB = 1;
                }
                else if (txtArticle.Text.Trim() == "501340")
                {
                    rpt = @"\\gblnj4\GIS\Reports\" + "LoginSheet_501340.rpt";
                    nB = 1;
                }
                else if (txtArticle.Text.Trim() == "501803")
                {
                    rpt = @"\\gblnj4\GIS\Reports\" + "LoginSheet_501803.rpt";
                    nB = 1;
                }
                else if (nM == 1)
                    rpt = @"\\gblnj4\GIS\Reports\" + "LoginSheet_Manifest_Revised.rpt";
                else
                    rpt = @"\\gblnj4\GIS\Reports\" + "LoginSheetX.rpt";

                if (txtArticle.Text.Trim() == "503130" || txtArticle.Text.Trim() == "503136" || txtArticle.Text.Trim() == "501116" ||
                    txtArticle.Text.Trim() == "501901" || txtArticle.Text.Trim() == "501340" || txtArticle.Text.Trim() == "501803")
                {
                    sqlcmd = new SqlCommand("spLoginSlashIng", sqlcnn);
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                    sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
                }
                else
                {
                    sqlcmd = new SqlCommand("spLoginSlashes", sqlcnn);
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                    sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
                }

                sqlcmd.ExecuteNonQuery();

                sqlcmd = new SqlCommand("spLoginSheetIng", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
                sqlcmd.ExecuteNonQuery();
                crDoc.Load(rpt);
                if (nM == 0 && nB == 0)
                {
                    sqlcmd = new SqlCommand("spLoginSheet", sqlcnn);
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                    sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
                }
                else
                {
                    sqlcmd = new SqlCommand("spLoginSlashes", sqlcnn);
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                    sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));

                    sqlcmd.ExecuteNonQuery();

                    sqlcmd = new SqlCommand("spLoginSheetIng", sqlcnn);
                    sqlcmd.CommandType = CommandType.StoredProcedure;

                    sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                    sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
                    sqlcmd.ExecuteNonQuery();
                }
            }
            else
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "LoginSheet.rpt";
                crDoc.Load(rpt);
                sqlcmd = new SqlCommand("spLoginSheet", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
            }
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
            }
            catch
            {}
            crDoc.SetDataSource(dTable);
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();

            //Open the PrintDialog
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.AllowSelection = true;
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.AllowCurrentPage = true;
            DialogResult dr = this.printDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //Get the Copy times
                int nCopy = this.printDocument1.PrinterSettings.Copies;
                //Get the number of Start Page
                int sPage = this.printDocument1.PrinterSettings.FromPage;
                //Get the number of End Page
                int ePage = this.printDocument1.PrinterSettings.ToPage;
                //Get the printer name
                string PrinterName = this.printDocument1.PrinterSettings.PrinterName;
                try
                {
                    crDoc.PrintOptions.PrinterName = PrinterName;
                    crDoc.PrintToPrinter(nCopy, false, sPage, ePage);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
            }
            dTable.Dispose(); crDoc.Close(); crDoc.Dispose();
        }

        private void dgvSamples_CurrentCellDirtyStateChanged(object sender, EventArgs e) 
        {
            if (dgvSamples.IsCurrentCellDirty)
            {
                dgvSamples.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvSlashExtData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSlashExtData.IsCurrentCellDirty)
            {
                dgvSlashExtData.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void txtStudyNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
            else if ((e.KeyChar != 13 && e.KeyChar != 8 && e.KeyChar < 48) || e.KeyChar > 57)
                e.Handled = true;
        }

        private void dgvSCExtData_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSCExtData.IsCurrentCellDirty)
            {
                dgvSCExtData.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void chkRush_Click(object sender, EventArgs e)
        {
            if (chkRush.Checked == true)
                chkRush.Checked = false;
            else
                chkRush.Checked = true;
        }

        private void dgvTests_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvTests.IsCurrentCellDirty)
            {
                dgvTests.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void txtFillCode_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
                dgvFillCodes.Visible = true;
            else
                dgvFillCodes.Visible = false;
        }

        private void btnILCancel_Click(object sender, EventArgs e)
        {
            pnlIL.Visible = false; pnlRecord.Enabled = true;
        }

        private void dgvFillCodes_DoubleClick(object sender, EventArgs e)
        {
            txtFillCode.Text = dgvFillCodes.CurrentRow.Cells[0].Value.ToString();
            dgvFillCodes.Visible = false;
        }

        private void dgvFillCodes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvFillCodes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtFillCode.Text = dgvFillCodes.CurrentRow.Cells[0].Value.ToString();
                dgvFillCodes.Visible = false;
            }
            else if (e.KeyChar == 27)
                dgvFillCodes.Visible = false;
        }

        private void txtFillCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
            else if (e.KeyChar == 27)
                dgvFillCodes.Visible = false;
        }

        private void picFillCodes_Click(object sender, EventArgs e)
        {
            dgvFillCodes.Visible = true;
        }

        private void txtFillCode_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwFillCodes;
                dvwFillCodes = new DataView(dtFillCodes, "FillCode like '%" + txtFillCode.Text.Trim().Replace("'", "''") + "%'", "FillCode", DataViewRowState.CurrentRows);
                dgvFillCodes.DataSource = dvwFillCodes;
                StandardDGVSetting(dgvFillCodes);
            }
        }

        private void txtILPO_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void pnlIL_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlIL.Location = PointToClient(this.pnlIL.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlIL_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                mouseDown = false;
        }

        private void pnlIL_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void btnILOK_Click(object sender, EventArgs e)
        {
            pnlIL.Visible = false; pnlRecord.Enabled = true;
            txtArticle.Text = txtFillCode.Text;
               
            DataTable dtM = PSSClass.Samples.NSManifest(txtFillCode.Text);
            if (dtM != null && dtM.Rows.Count > 0)
            {
                nIngredion = 1;
                DataTable dt = PSSClass.Quotations.LoadLoginTests(txtCmpyCode.Text, "2017.1223");
                if (dt == null)
                {
                    MessageBox.Show("Connection problems. Please contact your system administrator.");
                    return;
                }
                DataRow dr;
                dtBilling.Rows.Clear();
                for (int i = 0; i < dtM.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dtM.Rows[i]["ServiceCode"].ToString() == dt.Rows[j]["ServiceCode"].ToString())
                        {
                            DataRow dR;
                            dR = dtBilling.NewRow();
                            dR["QuoteNo"] = dt.Rows[j]["QuoteNo"];
                            dR["ServiceCode"] = dt.Rows[j]["ServiceCode"];
                            dR["ServiceDesc"] = dt.Rows[j]["ServiceDesc"];
                            dR["TestDesc1"] = dt.Rows[j]["TestDesc1"];
                            dR["UnitDesc"] = dt.Rows[j]["UnitDesc"];
                            dR["BillQty"] = 1;
                            dR["SelectedTest"] = true;
                            dR["Rush"] = false;
                            dR["UnitPrice"] = dt.Rows[j]["UnitPrice"];
                            dR["RushPrice"] = dt.Rows[j]["RushPrice"];
                            dR["ControlNo"] = dt.Rows[j]["ControlNo"];
                            dtBilling.Rows.Add(dR);
                        }
                    }
                    //
                    int nDuration = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(dtM.Rows[i]["ServiceCode"]));
                    dr = dtLogTests.NewRow();
                    dr["ServiceCode"] = Convert.ToInt16(dtM.Rows[i]["ServiceCode"]);
                    dr["ServiceDesc"] = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(dtM.Rows[i]["ServiceCode"]), dtSC);
                    dr["ProtocolNo"] = "";
                    dr["StartDate"] = DateTime.Now;
                    dr["EndDate"] = DateTime.Now.AddDays(nDuration);
                    dr["QuotationNo"] = "2017.1223.R0";//2015.0992.R2 7-21-2017; Changed from R1 8/3/2016
                    dr["BillQty"] = 1;
                    dr["TestSamples"] = 1;
                    dr["Slashes"] = "";
                    dr["PONo"] = txtILPO.Text;
                    dr["BookNo"] = txtILBookNo.Text;
                    dr["EC"] = false;
                    dr["ECCompType"] = DBNull.Value;
                    dr["ECLength"] = DBNull.Value;
                    dr["ECEndDate"] = DBNull.Value;
                    dr["DateSampled"] = DBNull.Value;
                    dr["QuoteFlag"] = "1";
                    dr["ReportNo"] = 0;
                    dr["AddlNotes"] = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(dtM.Rows[i]["ServiceCode"]), dtSC);
                    dtLogTests.Rows.Add(dr);
                }
                dtM.Dispose(); dt.Dispose();
                bsBilling.DataSource = dtBilling;
                dgvTests.DataSource = bsBilling;
                dgvTests.Columns["UnitPrice"].Visible = false;
                dgvTests.Columns["RushPrice"].Visible = false;
                dgvTests.Columns["ControlNo"].Visible = false;
                //Samples
                dtSamples.Rows[0]["SlashNo"] = txtILSlash.Text;
                bsSamples.EndEdit();
                cboSlashSC.SelectedIndex = 0;
                btnSlashSC_Click(null, null);
            }
        }

        private void chkReTest_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReTest.Checked == true)
            {
                DataTable dtM = PSSClass.Samples.NSManifest(txtFillCode.Text);
                if (dtM != null && dtM.Rows.Count > 0)
                {
                    for (int i = 0; i < dtM.Rows.Count; i++)
                    {
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(dgvManifestSC);
                        newRow.Cells[0].Value = dtM.Rows[i]["ServiceCode"].ToString();
                        newRow.Cells[1].Value = false;
                        dgvManifestSC.Rows.Add(newRow);
                    }
                }
            }
            else
                dgvManifestSC.RowCount = 0 ;
        }

        private void dgvManifestSC_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0)
                e.Cancel = true;
        }

        private void cboQuotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataTable dt = new DataTable();
                if (cboQuotes.SelectedValue == null)
                    dt = PSSClass.Quotations.LoadLoginTests("P", cboQuotes.Text.ToString());
                else
                    dt = PSSClass.Quotations.LoadLoginTests(cboQuotes.SelectedValue.ToString(), cboQuotes.Text.ToString());
                if (dt == null)
                {
                    MessageBox.Show("Connection problems. Please contact your system administrator.");
                    return;
                }
                if (dgvTests.Rows.Count != 0)
                {
                    byte nM = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtBilling.Rows.Count; j++)
                        {
                            if (dtBilling.Rows[j].RowState.ToString() != "Deleted" && dt.Rows[i]["QuoteNo"].ToString().Trim() == dtBilling.Rows[j]["QuoteNo"].ToString().Trim() && dt.Rows[i]["ControlNo"].ToString().Trim() == dtBilling.Rows[j]["ControlNo"].ToString().Trim())
                            {
                                nM = 1;
                                break;
                            }
                        }
                        if (nM == 0)
                        {
                            DataRow dR;
                            dR = dtBilling.NewRow();
                            dR["CmpyCode"] = txtCmpyCode.Text;
                            dR["QuoteNo"] = dt.Rows[i]["QuoteNo"];
                            dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                            dR["ServiceDesc"] = dt.Rows[i]["ServiceDesc"];
                            dR["TestDesc1"] = dt.Rows[i]["TestDesc1"];
                            dR["UnitDesc"] = dt.Rows[i]["UnitDesc"];
                            dR["BillQty"] = 0;
                            dR["SelectedTest"] = false;
                            dR["Rush"] = false;
                            dR["UnitPrice"] = dt.Rows[i]["UnitPrice"];
                            dR["RushPrice"] = dt.Rows[i]["RushPrice"];
                            dR["ControlNo"] = dt.Rows[i]["ControlNo"];
                            dR["QCmpyCode"] = cboQuotes.SelectedValue.ToString().Trim();
                            dtBilling.Rows.Add(dR);
                        }
                        nM = 0;
                    }
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dR;
                        dR = dtBilling.NewRow();
                        dR["CmpyCode"] = txtCmpyCode.Text;
                        dR["QuoteNo"] = dt.Rows[i]["QuoteNo"];
                        dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                        dR["ServiceDesc"] = dt.Rows[i]["ServiceDesc"];
                        dR["TestDesc1"] = dt.Rows[i]["TestDesc1"];
                        dR["UnitDesc"] = dt.Rows[i]["UnitDesc"];
                        dR["BillQty"] = 0;
                        dR["SelectedTest"] = false;
                        dR["Rush"] = false;
                        dR["UnitPrice"] = dt.Rows[i]["UnitPrice"];
                        dR["RushPrice"] = dt.Rows[i]["RushPrice"];
                        dR["ControlNo"] = dt.Rows[i]["ControlNo"];
                        dR["QCmpyCode"] = dt.Rows[i]["CompanyCode"].ToString().Trim();
                        dtBilling.Rows.Add(dR);
                    }
                }
                dt.Dispose();
                bsBilling.DataSource = dtBilling;
                dgvTests.DataSource = bsBilling;
                dgvTests.Select();
            }
        }

        private void cboQuotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                cboQuotes_SelectedIndexChanged(null, null);
        }

        private void txtLogNo_TextChanged(object sender, EventArgs e)
        {
            if (nMode == 0)
            {

            }
        }

        private void btnLSPreview_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (txtSponsor.Text.IndexOf("INGREDION") != -1) //INGREDION
                {
                    LabRpt rpt = new LabRpt();
                    rpt.rptName = "LoginSheet";
                    rpt.WindowState = FormWindowState.Maximized;
                    rpt.nLogNo = Convert.ToInt32(txtLogNo.Text);
                    rpt.SpID = Convert.ToInt16(txtSponsorID.Text);

                    if (txtArticle.Text == "503130" || txtArticle.Text == "503136" || txtArticle.Text == "501116" || txtArticle.Text == "501901" || txtArticle.Text == "501340" || txtArticle.Text == "501803")
                    {
                        rpt.nIngredion = 1;
                        rpt.strBatchNo = txtArticle.Text;
                    }
                    else
                    {
                        for (int i = 0; i < dtLogTests.Rows.Count; i++)
                        {
                            if (Convert.ToInt16(dtLogTests.Rows[i]["ServiceCode"].ToString()) > 1000 && Convert.ToInt16(dtLogTests.Rows[i]["ServiceCode"].ToString()) < 2000)
                            {
                                rpt.nIngredion = 1;
                                break;
                            }
                        }
                    }
                    try
                    {
                        rpt.Show();
                    }
                    catch { }
                }
                else
                {
                    if (txtLogNo.Text == "")
                        txtLogNo.Text = dgvFile.CurrentRow.Cells["PSSNo"].Value.ToString();
                    if (txtSponsorID.Text == "")
                        txtSponsorID.Text = dgvFile.CurrentRow.Cells["SpID"].Value.ToString();
                    //Regular Login - NON-INGREDION 
                    LabRpt rpt = new LabRpt();
                    rpt.rptName = "LoginSheet";
                    rpt.WindowState = FormWindowState.Maximized;
                    rpt.nLogNo = Convert.ToInt32(txtLogNo.Text);
                    rpt.CmpyCode = txtCmpyCode.Text;
                    rpt.SpID = Convert.ToInt16(txtSponsorID.Text);
                    rpt.nYr = dtpEntered.Value.Year;
                    try
                    {
                        rpt.Show();
                    }
                    catch { }
                }
            }
        }

        private void txtSamples_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
                e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void dgvSamples_SelectionChanged(object sender, EventArgs e)
        {
            if (lMoveNext == 1)
            {
                lMoveNext = 0;
                dgvSamples.CurrentCell = dgvSamples[colIndex, rowIndex];
                if (dgvSamples.CurrentCell.ColumnIndex == 5)
                {
                    try
                    {
                        int nR = dgvSamples.CurrentCell.RowIndex + 1;
                        dgvSamples.CurrentCell = dgvSamples.Rows[nR].Cells[0];
                        SendKeys.Send("{LEFT}");
                        SendKeys.Send("{LEFT}");
                        SendKeys.Send("{LEFT}");
                        SendKeys.Send("{LEFT}");
                    }
                    catch { }
                }
                else 
                {
                    SendKeys.Send("{RIGHT}");
                }
            }
        }

        private void dgvSamples_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSCExtData_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["SC"].Value = txtSCExt.Text;
        }

        private void FileAccess()
        {
            //Reload User's Access to this file - included in this function for sudden change in access level

            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; btnFAXEMail.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true; btnFAXEMail.Enabled = false;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true; btnFAXEMail.Enabled = true;
            }
            else 
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsddbPrint.Enabled = false; btnFAXEMail.Enabled = false;
            }
            tsddbSearch.Enabled = true;
        }

        private void cboStudyDir_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtStudyDirID.Text = cboStudyDir.SelectedValue.ToString();
            }
            catch { }
        }

        private void dgvSCExtData_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (nMode != 0)
            {
                dtSCExtData.PrimaryKey = new DataColumn[] { dtSCExtData.Columns["ServiceCode"], dtSCExtData.Columns["SCExtDataLabel"] };
                fkeys[0] = txtSCExt.Text;
                fkeys[1] = dgvSCExtData.Rows[e.Row.Index].Cells["Label"].Value;
                DataRow foundRow = dtSCExtData.Rows.Find(fkeys);
                if (foundRow != null)
                {
                    int n = dtSCExtData.Rows.IndexOf(foundRow);
                    DataRow[] foundRows;
                    foundRows = dtSCExtData.Select("ServiceCode = " + ((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
                    if (foundRows.Length > 1)
                        dtSCExtData.Rows[n].Delete();
                    else
                    {
                        dtSCExtData.Rows[n]["SCExtDataLabel"] = "";
                        dtSCExtData.Rows[n]["SCExtDataValue"] = "";
                    }
                }
            }
        }

        private void dgvSCExtData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    fkeys[0] = txtSCExt.Text;
                    fkeys[1] = dgvSCExtData.Rows[e.RowIndex].Cells["Label"].Value.ToString();
                }
                catch { }
            }
        }

        private void dgvSamples_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;
        }

        private void SamplesLogin_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Refresh();
        }

        private void lnkImgFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkImgFile.Text + ".jpg");
            }
            catch
            {
                try
                {
                    System.Diagnostics.Process.Start(lnkImgFile.Text + ".png");
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void dgvTests_DoubleClick(object sender, EventArgs e)
        {
            if (dgvTests.CurrentCell.OwningColumn.Name.ToString() == "QuoteNo")
            {
                //lnkQuote.Text = @"\\gblnj4\gis\reports\" + dgvTests.CurrentCell.Value.ToString().Trim() + ".pdf";
                //lnkQuote_LinkClicked(null, null);
                LinkQuote(dgvTests.Rows[dgvTests.CurrentCell.RowIndex].Cells["QCmpyCode"].Value.ToString().Trim(), dgvTests.CurrentCell.Value.ToString());
            }
        }

        //private void txtQuoteNo_DoubleClick(object sender, EventArgs e)
        //{
        //    LinkQuote(txtCmpyCode.Text, ((TextBox)dtrLogTests.CurrentItem.Controls["txtQuoteNo"]).Text);
        //}

        private void LinkQuote(string cCmpyCode, string cQNo)
        {
            QuotationRpt rptQuotation = new QuotationRpt();
            rptQuotation.WindowState = FormWindowState.Maximized;
            rptQuotation.nQ = 0;
            rptQuotation.nP = 0;
            try
            {

                int nI = cQNo.IndexOf("R");
                string strQNo = cQNo.Substring(0, nI - 1);
                string strRNo = cQNo.Substring((nI + 1), cQNo.Length - (nI + 1));

                int nRevNo = Convert.ToInt16(strRNo);
                rptQuotation.CmpyCode = cCmpyCode;
                rptQuotation.QuoteNo = strQNo;
                rptQuotation.RevNo = nRevNo;
                rptQuotation.nOld = 0;
                rptQuotation.Show();
            }
            catch { }  
        }


        private void txtSSFormNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                DataTable dtSSF = PSSClass.Samples.LogSearchSSFNo(Convert.ToInt32(txtSSFormNo.Text));
                if (dtSSF.Rows.Count > 0)
                {
                    dtSSF.Dispose();
                    MessageBox.Show("SSF number already scanned!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bOSSF = 1;

                dgvSponsors.Visible = false; dgvContacts.Visible = false;
                dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false;

                DataTable dtX = PSSClass.Samples.SSFLogBillRef(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX != null && dtX.Rows.Count > 0)
                {
                    string strQNo = dtX.Rows[0]["QuoteNo"].ToString();
                    string strRNo = dtX.Rows[0]["RevNo"].ToString();
                    DataTable dtQ = PSSClass.Quotations.QuoteRevStatus(strQNo, Convert.ToInt16(strRNo));
                    if (dtQ != null && dtQ.Rows.Count > 0)
                    {
                        if (dtQ.Rows[0]["RevisionStatus"].ToString() == "0" && dtQ.Rows[0]["WithPrepayment"].ToString() == "True")
                        //Quote is Pending and With Prepayment
                        {
                            MessageBox.Show("The quote is pending and requires a prepayment." + Environment.NewLine +
                                "Please contact Technical Services.", Application.ProductName);
                            txtSSFormNo.Text = "";
                            return;
                        }
                        else if (dtQ.Rows[0]["RevisionStatus"].ToString() == "0" && dtQ.Rows[0]["WithPrepayment"].ToString() == "False")
                        //Quote is Pending and no Prepayment required
                        {
                            DialogResult dReply = new DialogResult();
                            dReply = MessageBox.Show("The quote is currently pending." + Environment.NewLine + "Are the required documents submitted?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dReply == DialogResult.No)
                            {
                                MessageBox.Show("Required documents must be submitted." + Environment.NewLine +
                                "Please contact the Technical Services.", Application.ProductName);
                                txtSSFormNo.Text = "";
                                return;
                            }
                            MessageBox.Show("Please forward documents to Technical Services.", Application.ProductName);
                            PSSClass.Quotations.QuoteUpdStatus(strQNo, Convert.ToInt16(strRNo), Convert.ToInt16(LogIn.nUserID));
                        }
                    }
                }
                dtX = null;
                dtX = PSSClass.Samples.SSFLogMaster(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX == null || dtX.Rows.Count == 0)
                {
                    MessageBox.Show("No matching SSF number", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                txtSponsor.Text = dtX.Rows[0]["SponsorName"].ToString();
                txtSponsorID.Text = dtX.Rows[0]["SponsorID"].ToString();
                if (dtX.Rows[0]["PrimaryContactID"] != DBNull.Value && dtX.Rows[0]["PrimaryContactID"].ToString() != "0") // 4-30-2017
                    txtContactID.Text = dtX.Rows[0]["PrimaryContactID"].ToString();
                else
                    txtContactID.Text = dtX.Rows[0]["ContactID"].ToString();
                txtContact.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
                txtArticle.Text = dtX.Rows[0]["ArticleDesc"].ToString().Replace("\t", " ");//4-24/2017
                if (txtArticle.Text.Length > 255)
                    txtArticle.Text = txtArticle.Text.Substring(0, 254);
                txtAddlNotes.Text = dtX.Rows[0]["Comments"].ToString();
                txtGenDesc.Text = dtX.Rows[0]["ArticleDesc"].ToString();
                if ((dtX.Rows[0]["DEACategoryA"] == DBNull.Value && dtX.Rows[0]["DEACategoryB"] == DBNull.Value) ||
                     (Convert.ToBoolean(dtX.Rows[0]["DEACategoryA"]) == false && Convert.ToBoolean(dtX.Rows[0]["DEACategoryB"]) == false))
                    chkCtrldSubs.Checked = false;
                else
                    chkCtrldSubs.Checked = true;                
                if (dtX.Rows[0]["ReceiptCode"].ToString() == "1")
                    rdoRAmbient.Checked = true;
                else if (dtX.Rows[0]["ReceiptCode"].ToString() == "2")
                    rdoIcePack.Checked = true;
                else if (dtX.Rows[0]["ReceiptCode"].ToString() == "3")
                    rdoDryIce.Checked = true;
                else if (dtX.Rows[0]["ReceiptCode"].ToString() == "4")
                    rdoOther.Checked = true;
                if (dtX.Rows[0]["StorageCode"].ToString() == "1")
                    rdoSAmbient.Checked = true;
                else if (dtX.Rows[0]["StorageCode"].ToString() == "2")
                    rdoRefrigerator.Checked = true;
                else if (dtX.Rows[0]["StorageCode"].ToString() == "3")
                    rdoFreezer20.Checked = true;
                else if (dtX.Rows[0]["ReceiptCode"].ToString() == "4")
                {
                    rdoFreezer80.Checked = true;
                    txtOtherStorage.Text = dtX.Rows[0]["StorageDesc"].ToString();
                }
                string strGenDesc = "";
                dtX = null;
                dtX = PSSClass.Samples.SSFLogSamples(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX != null && dtX.Rows.Count > 0)
                {
                    dtSlashExtData.Rows.Clear();
                    dtSamples.Rows.Clear();
                    for (int i = 0; i < dtX.Rows.Count; i++)
                    {
                        string strXData = "";
                        string strSlashNo = "";
                        int nX = 0;
                        Int16 nExData = 0;

                        if (dtX.Rows[i]["SlashQty"] != DBNull.Value)
                        {
                            DataRow dR = dtSamples.NewRow();
                            if (dtX.Rows.Count == 1 && Convert.ToInt16(dtX.Rows[i]["SlashQty"]) > 1)
                            {
                                dR["SlashNo"] = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000") + "-" + Convert.ToInt16(dtX.Rows[i]["SlashQty"]).ToString("000");
                                strSlashNo = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000") + "-" + Convert.ToInt16(dtX.Rows[i]["SlashQty"]).ToString("000");
                            }
                            else
                            {
                                dR["SlashNo"] = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000"); //(i + 1).ToString("000");
                                strSlashNo = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000");
                            }
                            dR["SampleDesc"] = dtX.Rows[i]["AddlDesc"];
                            dR["OtherDesc1"] = dtX.Rows[i]["LotNo"];
                            dR["OtherDesc2"] = dtX.Rows[i]["OtherID"];
                            dR["SlashID"] = i;
                            dtSamples.Rows.Add(dR);
                            strGenDesc += dtX.Rows[i]["AddlDesc"] + Environment.NewLine;
                        }
                        else
                        {
                            if (dtX.Rows[i]["SampleQty"] != null)
                            {
                                int n = 1;
                                try
                                {
                                    n = Convert.ToInt16(dtX.Rows[i]["SampleQty"]);
                                }
                                catch
                                {
                                    string strX = dtX.Rows[i]["SampleQty"].ToString();
                                    string strNo = "";
                                    string strNum = "";
                                    for (int k = 0; k < strX.Length; k++)
                                    {
                                        if (strNum.IndexOf(strX.Substring(k, 1)) != -1)
                                        {
                                            strNo += strX.Substring(k, 1);
                                        }
                                    }
                                    if (strNo.Trim() != "")
                                    {
                                        n = Convert.ToInt16(strNo);
                                    }
                                }
                                if (n > 1)
                                {
                                    DataRow dR = dtSamples.NewRow();
                                    dR["SlashNo"] = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000") + "-" + Convert.ToInt16(dtX.Rows[i]["SampleQty"]).ToString("000");
                                    dR["SampleDesc"] = dtX.Rows[i]["AddlDesc"];
                                    dR["OtherDesc1"] = dtX.Rows[i]["LotNo"];
                                    dR["OtherDesc2"] = dtX.Rows[i]["OtherID"];
                                    dR["SlashID"] = i;
                                    dtSamples.Rows.Add(dR);
                                    strGenDesc += dtX.Rows[i]["AddlDesc"] + Environment.NewLine;
                                    //
                                    strSlashNo = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000") + "-" + Convert.ToInt16(dtX.Rows[i]["SampleQty"]).ToString("000");
                                }
                                else
                                {
                                    DataRow dR = dtSamples.NewRow();
                                    dR["SlashNo"] = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000");
                                    dR["SampleDesc"] = dtX.Rows[i]["AddlDesc"];
                                    dR["OtherDesc1"] = dtX.Rows[i]["LotNo"];
                                    dR["OtherDesc2"] = dtX.Rows[i]["OtherID"];
                                    dR["SlashID"] = i;
                                    dtSamples.Rows.Add(dR);
                                    strGenDesc += dtX.Rows[i]["AddlDesc"] + Environment.NewLine;
                                    //
                                    strSlashNo = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000");
                                }
                            }
                        }
                        //Extended Slash Data
                        for (int j = 0; j < 10; j++)
                        {
                            if (dtX.Rows[i]["ExtData" + j.ToString("00")] != null && dtX.Rows[i]["ExtData" + j.ToString("00")].ToString().Trim() != "")
                            {
                                strXData = dtX.Rows[i]["ExtData" + j.ToString("00")].ToString();
                                nX = strXData.IndexOf(",");
                                nExData = Convert.ToInt16(strXData.Substring(0, nX));
                                DataRow dREx = dtSlashExtData.NewRow();
                                dREx["SlashNo"] = strSlashNo;
                                dREx["ExtDataLabel"] = PSSClass.ExtDataLabels.ExtDataLabel(nExData);
                                dREx["ExtDataValue"] = strXData.Substring(nX + 1, strXData.Length - nX - 1);
                                dtSlashExtData.Rows.Add(dREx);
                            }
                        }
                    }
                    bsSamples.DataSource = dtSamples;
                    bnSamples.BindingSource = bsSamples;
                    dgvSamples.DataSource = bsSamples;
                }

                DataTable dtNotes = new DataTable();
                dtNotes.Columns.Add("SC", typeof(Int16));
                dtNotes.Columns.Add("SCNotes", typeof(string));
                dtNotes.Columns.Add("Billqty", typeof(Int16));

                dtX = null;
                dtX = PSSClass.Samples.SSFLogBillRef(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX != null && dtX.Rows.Count > 0)
                {
                    dtBilling.Rows.Clear();
                    for (int i = 0; i < dtX.Rows.Count; i++)
                    {
                        DataRow dR = dtBilling.NewRow();
                        dR["QuoteNo"] = dtX.Rows[i]["QuoteNo"] + ".R" + dtX.Rows[i]["RevNo"];
                        dR["ControlNo"] = dtX.Rows[i]["ControlNo"];
                        dR["ServiceCode"] = dtX.Rows[i]["ServiceCode"];
                        dR["ServiceDesc"] = dtX.Rows[i]["ServiceDesc"];
                        dR["TestDesc1"] = dtX.Rows[i]["TestDesc1"];
                        dR["UnitDesc"] = dtX.Rows[i]["UnitDesc"];
                        dR["UnitPrice"] = dtX.Rows[i]["UnitPrice"];
                        dR["RushPrice"] = dtX.Rows[i]["RushPrice"];
                        dR["BillQty"] = dtX.Rows[i]["BillQty"];
                        dR["SelectedTest"] = true;
                        dR["Rush"] = dtX.Rows[i]["Rush"];
                        if (dtX.Rows[i]["Rush"].ToString() == "True")
                        {
                            chkRush.Checked = true;
                        }
                        dtBilling.Rows.Add(dR);

                        DataRow dRN = dtNotes.NewRow();
                        dRN["SC"] = dtX.Rows[i]["ServiceCode"];
                        dRN["SCNotes"] = dtX.Rows[i]["TestDesc1"];
                        dRN["BillQty"] = dtX.Rows[i]["BillQty"];
                        dtNotes.Rows.Add(dRN);
                    }
                    bsBilling.DataSource = dtBilling;
                    dgvTests.DataSource = bsBilling;
                }
                else
                {
                    dtBilling.Rows.Clear();
                    DataRow dR = dtBilling.NewRow();
                    dR["QuoteNo"] = "";
                    dR["ControlNo"] = 0;
                    dR["ServiceCode"] = 0;
                    dR["ServiceDesc"] = "";
                    dR["TestDesc1"] = "";
                    dR["UnitDesc"] = "";
                    dR["UnitPrice"] = 0;
                    dR["RushPrice"] = 0;
                    dR["BillQty"] = 1;
                    dR["SelectedTest"] = true;
                    dR["Rush"] = false;
                    chkRush.Checked = false;
                    dtBilling.Rows.Add(dR);

                    DataRow dRN = dtNotes.NewRow();
                    dRN["SC"] = 0;
                    dRN["SCNotes"] = "";
                    dRN["BillQty"] = 1;
                    dtNotes.Rows.Add(dRN);
                    bsBilling.DataSource = dtBilling;
                    dgvTests.DataSource = bsBilling;
                }
                dtX = null;
                dtX = PSSClass.Samples.SSFLogTests(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX != null && dtX.Rows.Count > 0)
                {
                    dtLogTests.Rows.Clear();
                    string strPONo = "", strBookNo = "0";
                    int nD = 1;
                    if (txtSponsor.Text.IndexOf("INGREDION") != -1)
                    {
                        strPONo = PSSClass.Sponsors.SponsorLastPO(Convert.ToInt16(txtSponsorID.Text));
                        strBookNo = "828";
                    }
                    else
                    {
                        strPONo = dtX.Rows[0]["PONO"].ToString();
                    }

                    for (int i = 0; i < dtX.Rows.Count; i++)
                    {
                        DataRow[] foundRows;
                        string strNotes = "";
                        int nBQty = 0;
                        // Get Addl Notes per SC
                        foundRows = dtNotes.Select("SC = " + dtX.Rows[i]["SC"]);
                        if (foundRows.Count() > 0)
                        {
                            for (int j = 0; j < foundRows.Length; j++)
                            {
                                strNotes += foundRows[j]["SCNotes"].ToString() + " BillQty: " + foundRows[j]["BillQty"].ToString() + ", " + Environment.NewLine;
                                if (foundRows[j]["BillQty"] != null)
                                    nBQty += Convert.ToInt16(foundRows[j]["BillQty"]);
                            }
                        }
                        nD = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(dtX.Rows[i]["SC"]));
                        DataRow dR = dtLogTests.NewRow();
                        dR["ServiceCode"] = dtX.Rows[i]["SC"];
                        dR["ServiceDesc"] = "";
                        dR["QuotationNo"] = dtX.Rows[i]["QuoteNo"];
                        dR["PONo"] = strPONo;
                        dR["BookNo"] = Convert.ToInt16(strBookNo);
                        if (strNotes.Trim() != "")
                            dR["AddlNotes"] = strNotes.Substring(0, strNotes.Length - 1);
                        else
                            dR["AddlNotes"] = "";
                        if (dtX.Rows[i]["SamplesQty"] != DBNull.Value)
                            dR["TestSamples"] = dtX.Rows[i]["SamplesQty"];
                        else
                            dR["TestSamples"] = 1;
                        dR["BillQty"] = nBQty;
                        dR["QuoteFlag"] = "";
                        dR["ReportNo"] = DBNull.Value;
                        dR["StartDate"] = DateTime.Now;
                        dR["EndDate"] = DateTime.Now.AddDays(nD);
                        dR["StudyNo"] = DBNull.Value;
                        dR["StudyDirID"] = DBNull.Value;
                        dR["EC"] = DBNull.Value;
                        dR["ECCompType"] = DBNull.Value;
                        dR["ECLength"] = DBNull.Value;
                        dR["ECEndDate"] = DBNull.Value;
                        dR["DateSampled"] = DBNull.Value;
                        dtLogTests.Rows.Add(dR);
                    }
                    bsLogTests.DataSource = dtLogTests;
                    bnLogTests.BindingSource = bsLogTests;
                    dtrLogTests.DataSource = bsLogTests;
                }
                else
                {
                    MessageBox.Show("No test(s) required selected.", Application.ProductName);
                    DataRow dR = dtLogTests.NewRow();
                    dR["ServiceCode"] = 0;
                    dR["ServiceDesc"] = "";
                    dR["QuotationNo"] = "";
                    dR["PONo"] = "";
                    dR["BookNo"] = 0;
                    dR["AddlNotes"] = "";
                    dR["TestSamples"] = 1;
                    dR["BillQty"] = 0;
                    dR["QuoteFlag"] = "";
                    dR["ReportNo"] = DBNull.Value;
                    dR["StartDate"] = DBNull.Value;
                    dR["EndDate"] = DBNull.Value;
                    dR["StudyNo"] = DBNull.Value;
                    dR["StudyDirID"] = DBNull.Value;
                    dR["EC"] = DBNull.Value;
                    dR["ECCompType"] = DBNull.Value;
                    dR["ECLength"] = DBNull.Value;
                    dR["ECEndDate"] = DBNull.Value;
                    dR["DateSampled"] = DBNull.Value;
                    dtLogTests.Rows.Add(dR);
                    bsLogTests.DataSource = dtLogTests;
                    bnLogTests.BindingSource = bsLogTests;
                    dtrLogTests.DataSource = bsLogTests;
                }
                dtX = null;
                dtX = PSSClass.Samples.SSFLogSampleSC(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX != null && dtX.Rows.Count > 0)
                {
                    dtSampleSC.Rows.Clear();
                    for (int i = 0; i < dtX.Rows.Count; i++)
                    {
                        DataRow dR = dtSampleSC.NewRow();

                        if (dtX.Rows.Count == 1 && Convert.ToInt16(dtX.Rows[i]["SamplesQty"]) > 1)
                        {
                            dR["Slash"] = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000") + "-" + Convert.ToInt16(dtX.Rows[i]["SamplesQty"]).ToString("000");
                        }
                        else
                            dR["Slash"] = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000"); //(i + 1).ToString("000");
                        dR["SC"] = dtX.Rows[i]["SC"];
                        try
                        {
                            dR["SlashNo"] = (Convert.ToInt16(dtX.Rows[i]["SlashNo"]) + 1).ToString("000");
                            dR["ServiceCode"] = dtX.Rows[i]["SC"];
                        }
                        catch { }
                        dtSampleSC.Rows.Add(dR);
                    }
                    bsSampleSC.DataSource = dtSampleSC;
                    bnSampleSC.BindingSource = bsSampleSC;
                    dgvSampleSC.DataSource = bsSampleSC;
                    List<string> strListSSC = new List<string>();
                    List<string> strLSC = new List<string>();
                    for (int i = 0; i < dtLogTests.Rows.Count; i++)
                    {
                        if (dtLogTests.Rows[i].RowState.ToString() != "Deleted")
                        {
                            strLSC.Add(dtLogTests.Rows[i]["ServiceCode"].ToString());
                        }
                    }
                    for (int i = 0; i < dgvSamples.Rows.Count - 1; i++)
                    {
                        strListSSC.Add(dgvSamples.Rows[i].Cells["SlashNo"].Value.ToString());
                    }
                    if (strListSSC.Count > 0)
                    {
                        ((DataGridViewComboBoxColumn)dgvSampleSC.Columns[0]).DataSource = strListSSC.ToArray();
                    }
                    if (strLSC.Count > 0)
                    {
                        ((DataGridViewComboBoxColumn)dgvSampleSC.Columns[1]).DataSource = strLSC.ToArray();
                    }
                    for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                    {
                        dgvSampleSC.Rows[i].Cells["SlashNo"].Value = dgvSampleSC.Rows[i].Cells["Slash"].Value.ToString();
                        dgvSampleSC.Rows[i].Cells["ServiceCode"].Value = dgvSampleSC.Rows[i].Cells["SC"].Value.ToString();
                    }
                }
                else
                {
                    dtSampleSC.Rows.Clear();
                    DataRow dR = dtSampleSC.NewRow();
                    dR["Slash"] = "";
                    dR["SC"] = 0;
                    dtSampleSC.Rows.Add(dR);
                }
                if (txtSponsor.Text.IndexOf("INGREDION") != -1)
                {
                    if (PSSClass.Ingredion.CheckLeprino(txtArticle.Text) == true)
                        MessageBox.Show("REMINDER: Another GBL must be created for Leprino Foods Company.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (PSSClass.Ingredion.CheckUSP(txtArticle.Text) == true)
                        MessageBox.Show("REMINDER: Please limit composite entry to a range of 5 (i.e. 001-005).", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (PSSClass.Ingredion.CheckIndividualTest(txtArticle.Text) == true)
                        MessageBox.Show("REMINDER: B. cereus needs to be tested individually.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (strGenDesc.Trim() != "")
                    txtGenDesc.Text = strGenDesc;
                else
                    txtGenDesc.Text = txtArticle.Text;

                if (strGenDesc.Trim() != "")
                    txtGenDesc.Text = strGenDesc;
                else
                    txtGenDesc.Text = txtArticle.Text;

                nSearch = 99; //Refreshes List - Reloads all records, 88 - reloads only the current login scanned to speed loading
                SaveRecord();
                EditRecord();
                AddEditMode(true);
                PSSClass.Samples.UpdSubmitStatus(Convert.ToInt32(txtSSFormNo.Text));
                bOSSF = 0;
            }
        }

        private void dtrLogTests_Scroll(object sender, ScrollEventArgs e)
        {
            ((Button)dtrLogTests.CurrentItem.Controls["btnSC"]).Select();
        }

        private void picArticleDesc_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (dgvArticleDesc.Visible == true)
                    dgvArticleDesc.Visible = false;
                else
                {
                    dgvArticleDesc.Visible = true; dgvArticleDesc.BringToFront();
                    dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvGenDesc.Visible = false;
                    txtArticle.Select();
                }
            }
        }

        private void dgvArticleDesc_DoubleClick(object sender, EventArgs e)
        {
            //AMDC
            txtArticle.Text = dgvArticleDesc.Rows[dgvArticleDesc.CurrentCell.RowIndex].Cells["ArticleDesc"].Value.ToString();
            dgvArticleDesc.Visible = false; //txtGenDesc.Text = "";
            dgvSamples.Select();
        }

        private void dgvArticleDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            //AMDC
            if (e.KeyChar == 13)
            {
                txtArticle.Text = dgvArticleDesc.Rows[dgvArticleDesc.CurrentCell.RowIndex].Cells["ArticleDesc"].Value.ToString();
                dgvArticleDesc.Visible = false; //txtGenDesc.Text = "";
                dgvSamples.Select(); 
            }
            else if (e.KeyChar == 27)
            {
                dgvArticleDesc.Visible = false;
            }
        }

        private void txtArticle_TextChanged(object sender, EventArgs e)
        {
            //AMDC
            if (nMode != 0)
            {
                try
                {
                    DataView dvwArticleDesc;
                    dvwArticleDesc = new DataView(dtArticleDesc, "ArticleDesc  like '%" + txtArticle.Text.Trim().Replace("'", "''").Replace("%", "") + "%'", "ArticleDesc", DataViewRowState.CurrentRows);
                    dgvArticleDesc.DataSource = dvwArticleDesc;
                    dgvArticleDesc.Columns[0].Width = 442;
                }
                catch { }
            }
        }

        private void txtArticle_Enter(object sender, EventArgs e)
        {
            //AMDC
            if (nMode == 1)
            {
                dgvArticleDesc.Visible = true; dgvArticleDesc.BringToFront();
                dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvGenDesc.Visible = false;
            }
        }

        private void txtArticle_KeyPress(object sender, KeyPressEventArgs e)
        {
            //AMDC
            if (e.KeyChar == 27)
                dgvArticleDesc.Visible = false;
            else if (dgvArticleDesc.Visible == false)
            {
                dgvArticleDesc.Visible = true; dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvGenDesc.Visible = false;
            }
        }

        private void btnGenDesc_Click(object sender, EventArgs e)
        {
            //AMDC
            if (nMode != 0)
            {
                try
                {
                    DataTable dtGenDesc = PSSClass.Samples.GetGenDesc(Convert.ToInt16(txtSponsorID.Text), txtArticle.Text.Trim());
                    if (dtGenDesc != null)
                    {
                        dgvGenDesc.DataSource = dtGenDesc;
                        dgvGenDesc.Visible = true; dgvGenDesc.BringToFront(); dgvGenDesc.Columns[0].Width = 725;
                        dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvArticleDesc.Visible = false;
                    }
                }
                catch { }
            }
        }

        private void dgvGenDesc_DoubleClick(object sender, EventArgs e)
        {
            //AMDC
            txtGenDesc.Text = dgvGenDesc.Rows[dgvGenDesc.CurrentRow.Index].Cells["GenDesc"].Value.ToString();
            dgvGenDesc.Visible = false;
        }

        private void dgvGenDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            //AMDC
            if (e.KeyChar == 27)
            {
                dgvGenDesc.Visible = false;
            }
        }

        private void dgvArticleDesc_Leave(object sender, EventArgs e)
        {
            dgvArticleDesc.Visible = false;
        }

        private void dgvGenDesc_Leave(object sender, EventArgs e)
        {
            dgvGenDesc.Visible = false;
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false;
        }

        private void txtContactID_Enter(object sender, EventArgs e)
        {
            dgvContacts.Visible = false; dgvSponsors.Visible = false; dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false;
        }

        private void btnHideDesc_Click(object sender, EventArgs e)
        {
            dgvGenDesc.Visible = false;
        }

        private void btnRptClose_Click(object sender, EventArgs e)
        {
            pnlReports.Visible = false;
        }

        private void cboVitalFY_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dte = "1/1/" + cboVitalFY.Text;
            string sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");
            dtpFrom.Value = Convert.ToDateTime(sdte);

            dte = "12/31/" + cboVitalFY.Text;
            sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");
            dtpTo.Value = Convert.ToDateTime(sdte);
        }

        private void rdoYearly_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoYearly.Checked)
            {
                cboVitalFY.Select();
                cboVitalFY.SelectedIndex = 0;
                cboVitalFY.DroppedDown = true;
                dtpFrom.Enabled = false; dtpTo.Enabled = false;
            }
        }

        private void rdoMonthly_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMonthly.Checked)
            {
                cboMonths.Select();
                cboMonths.SelectedIndex = 0;
                cboMonths.DroppedDown = true;
                dtpFrom.Enabled = false; dtpTo.Enabled = false;
            }
        }

        private void rdoDaily_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDaily.Checked)
            {
                dtpFrom.Value = DateTime.Now; dtpTo.Value = DateTime.Now;
                dtpFrom.Enabled = true; dtpTo.Enabled = false;
                dtpFrom.Select();
                SendKeys.Send("%{DOWN}");
            }
        }

        private void rdoDateRange_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDateRange.Checked)
            {
                dtpFrom.Enabled = true; dtpTo.Enabled = true;
                dtpFrom.Select();
                SendKeys.Send("%{DOWN}");
            }
        }

        private void pnlReports_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void pnlReports_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlReports.Location = PointToClient(this.pnlReports.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlReports_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                mouseDown = false;
        }

        private void label63_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void label63_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlReports.Location = PointToClient(this.pnlReports.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void label63_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                mouseDown = false;
        }

        private void pnlReports_Leave(object sender, EventArgs e)
        {
            pnlReports.Visible = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (rdoDateRange.Checked)
            {
                if (dtpFrom.Value > dtpTo.Value)
                {
                    MessageBox.Show("Date range is invalid.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            LabRpt rpt = new LabRpt();
            rpt.rptName = "LoginsReport";
            rpt.WindowState = FormWindowState.Maximized;
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            if (rdoYearly.Checked)
            {
                string strSQL = "SELECT MONTH(LM.DateCreated) AS Mo, U.UserName, COUNT(LM.PSSNo) AS TotLogins " +
                                "FROM LOGMASTER LM LEFT OUTER JOIN " +
                                "Users U ON LM.CreatedByID = U.UserID " +
                                "WHERE YEAR(LM.DateCreated) = " + cboVitalFY.Text + " ";

                if (!chkIncPSS.Checked)
                    strSQL = strSQL + "AND LM.SponsorID <> 130 ";

                strSQL = strSQL + "GROUP BY MONTH(LM.DateCreated), U.UserName";
                rpt.strFilter = strSQL;
                rpt.rptFileName = "RptLoginsYearly";
                rpt.nFormat = 1;
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                if (chkShowDetails.Checked)
                    rpt.nExType = 1;
                else
                    rpt.nExType = 2;

            }
            else if (rdoMonthly.Checked)
            {
                string strSQL = "SELECT MONTH(LM.DateCreated) AS Mo, DAY(LM.DateCreated) AS Dy, U.UserName, COUNT(LM.PSSNo) AS TotLogins " +
                                "FROM LOGMASTER LM LEFT OUTER JOIN " +
                                "Users U ON LM.CreatedByID = U.UserID " +
                                "WHERE YEAR(LM.DateCreated) = " + cboVitalFY.Text + " AND MONTH(LM.DateCreated) = " + (cboMonths.SelectedIndex + 1).ToString() + " ";
                if (!chkIncPSS.Checked)
                    strSQL = strSQL + "AND LM.SponsorID <> 130 ";

                strSQL = strSQL + "GROUP BY MONTH(LM.DateCreated), DAY(LM.DateCreated), U.UserName";

                rpt.strFilter = strSQL;
                rpt.nFormat = 2;
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.nMo = cboMonths.SelectedIndex + 1;
                rpt.rptFileName = "RptLoginsMonthly";
                if (chkShowDetails.Checked)
                    rpt.nExType = 1;
                else
                    rpt.nExType = 2;
            }
            else if (rdoDaily.Checked)
            {
                string strSQL = "SELECT U.UserName, LM.PSSNo " +
                                "FROM LOGMASTER LM LEFT OUTER JOIN " +
                                "Users U ON LM.CreatedByID = U.UserID " +
                                "WHERE Convert(date, LM.DateCreated, 101) = '" + dtpFrom.Value.ToShortDateString() + "'";//YEAR(LM.DateCreated) = " + cboVitalFY.Text + " AND MONTH(LM.DateCreated) = " + (cboMonths.SelectedIndex + 1) + " " +
                if (!chkIncPSS.Checked)
                    strSQL = strSQL + "AND LM.SponsorID <> 130 ";

                rpt.strFilter = strSQL;
                rpt.nFormat = 3;
                rpt.nYr = dtpFrom.Value.Year;
                rpt.nMo = dtpFrom.Value.Month;
                rpt.nDy = dtpFrom.Value.Day;
                rpt.rptFileName = "RptLoginsDaily";
                if (chkShowDetails.Checked)
                    rpt.nExType = 1;
                else
                    rpt.nExType = 2;
            }
            //else if (rdoDateRange.Checked && chkIncGBL.Checked)
            //{
            //    rpt.strFilter = "CONVERT(date, LM.DateCreated, 101) >= '" + dtpFrom.Value.ToString("MM/dd/yyyy") + "' AND CONVERT(date, LM.DateCreated, 101) <= '" + dtpTo.Value.ToString("MM/dd/yyyy") + "'";
            //    rpt.nFormat = 4;
            //    rpt.nYr = dtpFrom.Value.Year;
            //    rpt.nMo = dtpFrom.Value.Month;
            //    rpt.nDy = dtpFrom.Value.Day;
            //    rpt.pubRangeTo = dtpTo.Value;
            //}
            //else if (rdoDateRange.Checked && !chkIncGBL.Checked)
            //{
            //    rpt.strFilter = "CONVERT(date, LM.DateCreated, 101) >= '" + dtpFrom.Value.ToString("MM/dd/yyyy") + "' AND CONVERT(date, LM.DateCreated, 101) <= '" + dtpTo.Value.ToString("MM/dd/yyyy") + "' AND LM.SponsorID <> 130";
            //    rpt.nFormat = 4;
            //    rpt.nYr = dtpFrom.Value.Year;
            //    rpt.nMo = dtpFrom.Value.Month;
            //    rpt.nDy = dtpFrom.Value.Day;
            //    rpt.pubRangeTo = dtpTo.Value;
            //}
            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr++;
            lblLoadStatus.Text = "Retrieving records from database...please standby!";// +nCtr.ToString() + " second(s) elapsed.";
        }

        private void txtContact_Enter(object sender, EventArgs e)
        {
            if (nMode != 0 && dgvContacts.Rows.Count > 0 && txtSponsorID.Text.Trim() != "")
            {
                try
                {
                    LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    dgvContacts.Visible = true; dgvContacts.BringToFront();
                    dgvSponsors.Visible = false; dgvArticleDesc.Visible = false; dgvGenDesc.Visible = false;
                }
                catch { }
            }    
        }

        private void dgvGenDesc_Enter(object sender, EventArgs e)
        {
            if (txtSponsorID.Text == "1345" || txtSponsorID.Text == "3058" || //Collagen
                    txtSponsorID.Text == "1805") //Maquet
                chkOtherID.Checked = true;
            else
                chkOtherID.Checked = false;
        }

        private void txtContactID_TextChanged(object sender, EventArgs e)
        {
            if (txtContactID.Text.Trim() == "")
            {
                txtContact.Text = ""; dgvContacts.Visible = false;
            }
        }
    }
}
