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

namespace GIS
{
    public partial class IngredionManifestLog : GIS.TemplateForm
    {
        public byte nFR;
        public Int64 nLogNo;

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

        private byte lMoveNext;
        private int colIndex = 0, rowIndex = 0;

        private bool bAnalyst;
        private bool bManager;
        private string strSAPDate;
        private string strSAPTime;

        //for DatagridView search
        private int nCtr = 0;
        private int nSw = 0;
        //======================

        List<string> strList = new List<string>();
        List<string> strListSC = new List<string>();
        List<string> strListQ = new List<string>();
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
        DataTable dtSCExt = new DataTable();//Parent Table
        DataTable dtSCExtData = new DataTable();//Child Table 
        DataTable dtSlashExt = new DataTable();//Parent Table
        DataTable dtSlashExtData = new DataTable();//Child Table
        //Ingredion Tables
        DataTable dtFillCodes = new DataTable();//derived from Manifest

        public IngredionManifestLog()
        {
            InitializeComponent();

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
            txtContact.GotFocus += new EventHandler(txtContactEnterHandler);
            txtSponsorID.GotFocus += new EventHandler(txtSponsorIDEnterHandler);
            txtContactID.GotFocus += new EventHandler(txtContactIDEnterHandler);
            txtPONo.KeyPress += new KeyPressEventHandler(txtPONoKeyPressHandler);
            dgvPONo.LostFocus += new EventHandler(dgvPONoOnLeave);
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[2];

            items[0] = new ToolStripMenuItem();
            items[0].Text = "Login Sheet";
            items[0].Click += new EventHandler(PrtLoginClickHandler);

            items[1] = new ToolStripMenuItem();
            items[1].Text = "Sample Labels";
            items[1].Click += new EventHandler(PrtLabelsClickHandler);

            //items[2] = new ToolStripMenuItem();
            //items[2].Text = "Grouped By Size";
            //items[2].Click += new EventHandler(PrintSpSizeClickHandler);

            //items[3] = new ToolStripMenuItem();
            //items[3].Text = "Grouped By Industry";
            //items[3].Click += new EventHandler(PrintSpIndustryClickHandler);

            //items[4] = new ToolStripMenuItem();
            //items[4].Text = "Grouped By Region/State";
            //items[4].Click += new EventHandler(PrintSpRegStateClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            DataTable dt = new DataTable();
            dt = GISClass.Samples.SampLogMaster();
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }

            //int ndx = 0;
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

        private void DateSAPBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yy");
            else
                e.Value = "__/__/__";
        }

        private void LoadRecords()
        {
            nMode = 0;

            if (nSearch == 99)
                dtLogMaster = GISClass.Ingredion.IngredionLogMaster();
            //else if (nSearch == 2)
            //    dtLogMaster = GISClass.Samples.LogSearchSamples(strCriteria, strData);
            //else if (nSearch == 3)
            //    dtLogMaster = GISClass.Samples.LogSearchMaster(strCriteria, strData);
            //else if (nSearch == 4)
            //    dtLogMaster = GISClass.Samples.LogSearchTests(strCriteria, strData, nSSC, nSSpID);
            //else if (nSearch == 5)
            //    dtLogMaster = GISClass.Samples.LogSearchInv(strData);

            if (nSearch != 99 && (dtLogMaster == null || dtLogMaster.Rows.Count == 0))
            {
                MessageBox.Show("No matching records found.");
                dtLogMaster = GISClass.Ingredion.IngredionLogMaster();
            }
            bsFile.DataSource = dtLogMaster;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            if (tsddbSearch.DropDownItems.Count == 0)
            {
                //int ndx = 0;
                int i = 0;
                int n = 0;

                arrCol = new string[dtLogMaster.Columns.Count];

                //foreach (DataColumn colFile in sqlds.Tables["Sponsors"].Columns)
                //{
                //    ndx = colFile.ColumnName.IndexOf("ID"); //search for the existence of the word "ID" in the field name
                //    if (ndx != -1)
                //    {
                //        n += 1;
                //    }
                //}

                ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

                foreach (DataColumn colFile in dtLogMaster.Columns)
                {
                    //ndx = colFile.ColumnName.IndexOf("ID"); //search for the existence of the word "ID" in the field name
                    //if (ndx == -1)
                    //{
                    items[i] = new ToolStripMenuItem();
                    items[i].Name = colFile.ColumnName;

                    //items[i].Text = colFile.ColumnName;

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
            FileAccess();
            pnlDataControl.Visible = false;
        }

        private void LoadSponsorsDDL()
        {
            dtSponsors = GISClass.Ingredion.SponsorNames();
            dgvSponsors.DataSource = null;
            dgvSponsors.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
        }

        private void LoadSC()
        {
            DataTable dt = new DataTable();
            dtSC = GISClass.ServiceCodes.SCDDL();
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

            dtSCMaster = GISClass.ServiceCodes.SCDDLCombo();
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
            dtPONo = GISClass.PO.PODDL(Convert.ToInt16(txtSponsorID.Text));
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
            dt = GISClass.Employees.StudyDirectors();
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
            dgvFile.Columns["GBLNo"].HeaderText = "GBL NO.";
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
            //dgvFile.Columns["DateCreated"].HeaderText = "REPORT DATE";
            dgvFile.Columns["DateEMailed"].HeaderText = "REPORT MAIL DATE";
            dgvFile.Columns["InvoiceNo"].HeaderText = "INV. NO.";
            dgvFile.Columns["InvoiceDate"].HeaderText = "INV. DATE";
            dgvFile.Columns["DateMailed"].HeaderText = "INV. MAIL DATE";
            dgvFile.Columns["LoginName"].HeaderText = "CREATED BY";
            //dgvFile.Columns[9].HeaderText = "ACK. REPORT";
            //dgvFile.Columns[11].HeaderText = "REPORT NO.";
            //dgvFile.Columns[12].HeaderText = "REV. NO.";
            //dgvFile.Columns[13].HeaderText = "PDF";
            //dgvFile.Columns[14].HeaderText = "REPORT DATE";
            //dgvFile.Columns[15].HeaderText = "HOLD TEST";
            //dgvFile.Columns[16].HeaderText = "REPORT MAIL DATE";
            //dgvFile.Columns[17].HeaderText = "E-MAILED";
            //dgvFile.Columns[18].HeaderText = "INVOICE NO.";
            //dgvFile.Columns[19].HeaderText = "CR HOLD";
            //dgvFile.Columns[20].HeaderText = "INVOICE DATE";
            //dgvFile.Columns[21].HeaderText = "INVOICE MAIL DATE";
            dgvFile.Columns["GBLNo"].Width = 75;
            dgvFile.Columns["GBLNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            dgvFile.Columns["RevisionNo"].Width = 50;
            dgvFile.Columns["RevisionNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvFile.Columns["DateCreated"].Width = 75;
            //dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            //dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            dgvFile.Columns["DateChecked"].Visible = false;
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
            ClearControls(this.pnlDataControl);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            btnClose.Visible = true; btnClose.BringToFront();
            OpenControls(pnlRecord, false); OpenControls(pnlOthers, false);
            OpenControls(tabComments, false);

            cboSlashSC.Enabled = false;

            txtSponsorID.Text = dgvFile.CurrentRow.Cells["SpID"].Value.ToString();
            txtSponsor.Text = dgvFile.CurrentRow.Cells["SpName"].Value.ToString();
            txtContactID.Text = dgvFile.CurrentRow.Cells["ContactID"].Value.ToString();
            txtContact.Text = dgvFile.CurrentRow.Cells["ConName"].Value.ToString();
            txtArticle.Text = dgvFile.CurrentRow.Cells["Article"].Value.ToString();

            nMode = 0; 
            if (nFR == 1)
                txtLogNo.Text = nLogNo.ToString();
            else
                txtLogNo.Text = dgvFile.CurrentRow.Cells[0].Value.ToString();

            //DataTable dt = new DataTable();
            dtLogFM = GISClass.Samples.LogMasterData(Convert.ToInt32(txtLogNo.Text));
            if (dtLogFM == null || dtLogFM.Rows.Count == 0)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in pnlDataControl.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in pnlOthers.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in tabComments.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in grpStorage.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in pnlIL.Controls)
            {
                c.DataBindings.Clear();
            }
            bsLogFM.DataSource = dtLogFM;

            tabLogin.SelectedIndex = 1;
            ((TextBox)tabComments.Controls["txtGenDesc"]).DataBindings.Add("Text", bsLogFM, "SampleDesc");
            tabLogin.SelectedIndex = 0;
            txtSponsorID.DataBindings.Add("Text", bsLogFM, "SponsorID");
            txtContactID.DataBindings.Add("Text", bsLogFM, "ContactID");
            chkCtrldSubs.DataBindings.Add("Checked", bsLogFM, "CtrldSubstance", true);
            chkRush.DataBindings.Add("Checked", bsLogFM, "Rush",true);
            chkAnalyst.DataBindings.Add("Checked", bsLogFM, "AnalystDone",true);
            chkManager.DataBindings.Add("Checked", bsLogFM, "ManagerChecked",true);
            txtArticle.DataBindings.Add("Text", bsLogFM, "ArticleDesc");
            //txtImageFile.DataBindings.Add("Text", bsLogFM, "ImageFileName");
            //txtGenDesc.DataBindings.Add("Text", bsLogFM, "SampleDesc");
            txtAddlNotes.DataBindings.Add("Text", bsLogFM, "AddlNotes");
            txtStorageCode.DataBindings.Add("Text", bsLogFM, "StorageCode");
            txtRecCode.DataBindings.Add("Text", bsLogFM, "ReceiptCode");
            txtOtherStorage.DataBindings.Add("Text", bsLogFM, "StorageDesc");
            txtILGBLNo.DataBindings.Add("Text", bsLogFM, "RetestGBLNo");
            txtSSFormNo.DataBindings.Add("Text", bsLogFM, "SSFormNo");

            Binding DateCreatedBinding;
            DateCreatedBinding = new Binding("Value", bsLogFM, "DateCreated");
            DateCreatedBinding.Format += new ConvertEventHandler(DateCreatedBinding_Format);
            dtpEntered.DataBindings.Add(DateCreatedBinding);

            Binding DateRecBinding;
            DateRecBinding = new Binding("Value", bsLogFM, "DateReceived");
            DateRecBinding.Format += new ConvertEventHandler(DateRecBinding_Format);
            dtpReceived.DataBindings.Add(DateRecBinding);

            Binding DateCancBinding;
            DateCancBinding = new Binding("Text", bsLogFM, "DateCancelled");
            DateCancBinding.Format += new ConvertEventHandler(DateCancBinding_Format);
            mskDateCancelled.DataBindings.Add(DateCancBinding);

            Binding DateSAPBinding;
            DateSAPBinding = new Binding("Text", bsLogFM, "DateSAP");
            //DateSAPBinding.Format += new ConvertEventHandler(DateSAPBinding_Format);
            mskDateSAP.DataBindings.Add(DateSAPBinding);

            //if (dtLogFM.Rows[0]["SampleDesc"] != null && dtLogFM.Rows[0]["SampleDesc"].ToString() != "")
            //{
            //    txtGenDesc.Text = dtLogFM.Rows[0]["SampleDesc"].ToString();
            //}

            if (dtLogFM.Rows[0]["DateCancelled"].ToString() != "")
                chkCancelled.Checked = true;
            else
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

            if (dtLogFM.Rows[0]["RetestGBLNo"].ToString() != "" && Convert.ToInt32(dtLogFM.Rows[0]["RetestGBLNo"]) != 0)
            {
                chkReTest.Checked = true; txtILGBLNo.Text = dtLogFM.Rows[0]["RetestGBLNo"].ToString();
            }
            else
            {
                chkReTest.Checked = false;
            }
            if (dtLogFM.Rows[0]["AnalystDone"].ToString() == "True")
                chkAnalyst.Checked = true;
            else
                chkAnalyst.Checked = false;

            if (dtLogFM.Rows[0]["ManagerChecked"].ToString() == "True")
                chkManager.Checked = true;
            else
                chkManager.Checked = false;

            if (dtLogFM.Rows[0]["DateSAP"].ToString() != "")
            {
                DateTime dte = Convert.ToDateTime(dtLogFM.Rows[0]["DateSAP"]);
                cboSAPTime.Text = dte.ToString("HH");
                strSAPTime = dte.ToString("HH");
                mskDateSAP.Text = dte.ToString("MM/dd/yy");
                chkCtrldSubs.Checked = true;
            }
            else
                chkCtrldSubs.Checked= false;

            cboQuotes.Enabled = false; btnSelQTests.Enabled = false; btnUnSelQTests.Enabled = false; btnSelAllTests.Enabled = false; btnTests.Enabled = false;
            LoadSamples();
            LoadSlashAddlData();
            LoadLogTests();
            LoadSamplesSC();
            LoadBillingRef();
            LoadSlashExtData();
            LoadSCExtData();
            SamplesAddlDataLabels();
            btnAddSample.Enabled = true; btnEditSample.Enabled = true; btnDelSample.Enabled = true; btnSaveSample.Enabled = false; btnCancelSample.Enabled = false;
            pnlQuotes.Visible = false;
            btnLSPreview.Enabled = true; btnLSPrinter.Enabled = true; btnDataForm.Enabled = true; cboChainOfCustody.Enabled = true; btnPrintCOC.Enabled = true;

            if (txtSponsorID.Text == "260") //GIBRALTAR LABORATORIES
            {
                btnAddTest.Enabled = true; btnDelTest.Enabled = true;
            }
            else
            {
                btnAddTest.Enabled = false; btnDelTest.Enabled = false;
            }
            tabLogin.SelectedIndex = 0;

            //if (strFileAccess == "RW" || strFileAccess == "FA")
            //    btnCopyGBL.Enabled = true;
            //else
                btnCopyGBL.Enabled = false;
            //if (chkAnalyst.Checked == true)
            //else
            //    pnlIngredion.Visible = false;
            pnlDataControl.Visible = true; pnlDataControl.BringToFront(); //pnlDataControl.Location = new Point(840, 120);

            if (GISClass.Users.UserDeptName(LogIn.nUserID).ToString().ToUpper() == "RECEIVING")
                pnlDataControl.Enabled = false;
            else if (strFileAccess == "RO")
                pnlDataControl.Enabled = false;
            else
            {
                if (strFileAccess == "RW")
                {
                    pnlDataControl.Enabled = true;
                    chkManager.Enabled = false; mskDateSAP.Enabled = true;
                    if (chkAnalyst.Checked == true)
                        bAnalyst = true;
                    else
                        bAnalyst = false;
                }
                else if (strFileAccess == "FA")
                {
                    pnlDataControl.Enabled = true;
                    if (chkAnalyst.Checked == true)
                        bAnalyst = true;
                    else if (chkAnalyst.Checked == false)
                        bAnalyst = false;

                    if (chkManager.Checked == true)
                        bManager = true;
                    else
                        bManager = false;

                    strSAPDate = mskDateSAP.Text;

                    chkManager.Enabled = true; mskDateSAP.Enabled = true;
                }
                if (chkManager.Checked == true)
                    pnlDataControl.Enabled = false;
                else
                    pnlDataControl.Enabled = true;
            }
            pnlIL.Visible = false;
        }

        private void LoadSamplesSC()
        {
            dtSampleSC = GISClass.Samples.SampleLogSC(Convert.ToInt32(txtLogNo.Text));
            if (dtSampleSC == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bsSampleSC.DataSource = dtSampleSC;
            dgvSampleSC.DataSource = bsSampleSC;            
            //dtSampleSC.DefaultView.Sort = "SlashNo ASC";

            //DataTable dt = new DataTable();
            //dt = GISClass.Samples.SampleLogSC(Convert.ToInt32(txtLogNo.Text));

            //dtSampleSC.Rows.Clear();
            
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dR;
            //    dR = dtSampleSC.NewRow();
            //    dtSampleSC.Rows.Add(dR);
            //}
            List<string> strListSSC = new List<string>();
            List<string> strLSC = new List<string>();
            
            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                strLSC.Add(dtLogTests.Rows[i]["ServiceCode"].ToString());
            }

            for (int i = 0; i < dgvSamples.Rows.Count -1; i++)
            {
                strListSSC.Add(dgvSamples.Rows[i].Cells["SlashNo"].Value.ToString());
            }

            if (strListSSC.Count > 0)
                ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = strListSSC.ToArray();

            if (strLSC.Count > 0)
                ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["ServiceCode"]).DataSource = strLSC.ToArray();

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dgvSampleSC.Rows[i].Cells[0].Value = dt.Rows[i]["SlashNo"].ToString();
            //    dgvSampleSC.Rows[i].Cells[1].Value = dt.Rows[i]["SC"].ToString();
            //}

            for (int i = 0; i < dtSampleSC.Rows.Count; i++)
            {
                dgvSampleSC.Rows[i].Cells["SlashNo"].Value = dtSampleSC.Rows[i]["Slash"].ToString();
                dgvSampleSC.Rows[i].Cells["ServiceCode"].Value = dtSampleSC.Rows[i]["SC"].ToString();
            }
            //dgvSampleSC.Columns[2].Visible = false; dgvSampleSC.Columns[3].Visible = false;
            dtrLogTests.Enabled = false;
        }

        private void LoadLogTests()
        {
            //dtLogTests.Rows.Clear();
            //DataTable dt = new DataTable();
            dtLogTests = GISClass.Samples.LogTestsData(Convert.ToInt32(txtLogNo.Text));
            if (dtLogTests == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dR;
            //    dR = dtLogTests.NewRow();
            //    dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
            //    dR["ServiceDesc"] = dt.Rows[i]["ServiceDesc"];
            //    dR["ProtocolNo"] = dt.Rows[i]["ProtocolNo"];
            //    dR["StartDate"] = dt.Rows[i]["StartDate"];
            //    dR["EndDate"] = dt.Rows[i]["EndDate"];
            //    dR["PONo"] = dt.Rows[i]["PONo"];
            //    dR["TestSamples"] = dt.Rows[i]["TestSamples"];
            //    dR["BillQty"] = dt.Rows[i]["BillQty"];
            //    dR["QuotationNo"] = dt.Rows[i]["QuotationNo"];
            //    dR["BookNo"] = dt.Rows[i]["BookNo"];
            //    dR["EC"] = dt.Rows[i]["EC"];
            //    dR["ECCompType"] = dt.Rows[i]["ECCompType"];
            //    dR["ECLength"] = dt.Rows[i]["ECLength"];
            //    dR["ECEndDate"] = dt.Rows[i]["ECEndDate"];
            //    dR["DateSampled"] = dt.Rows[i]["DateSampled"];
            //    dR["QuoteFlag"] = dt.Rows[i]["QuoteFlag"];
            //    dtLogTests.Rows.Add(dR);
            //}
            bsLogTests.DataSource = dtLogTests;
            dtrLogTests.DataSource = bsLogTests;
            //dtLogTests.DefaultView.Sort = "ServiceCode ASC";
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
            bsFile.Filter = "GBLNo<>0";
        }

        private void SearchItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            //tslSearch.Text = "By " + clickedItem.Text;
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
                bsFile.Filter = "GBLNo<>0";
                GISClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
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
            if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "ReportNo" && dgvFile.CurrentCell.Value.ToString() != "")
            {
                int intOpen = GISClass.General.OpenForm(typeof(FinalReports));

                if (intOpen == 1)
                {
                    GISClass.General.CloseForm(typeof(FinalReports));
                }
                FinalReports childForm = new FinalReports();
                childForm.Text = "Final Reports";
                childForm.MdiParent = this.MdiParent;
                childForm.nRptNo = Convert.ToInt32(dgvFile.CurrentCell.Value);
                childForm.nLSw = 1;
                childForm.Show();
            }
            else if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "InvoiceNo" && dgvFile.CurrentCell.Value.ToString() != "")
            {
                int intOpen = GISClass.General.OpenForm(typeof(FinalBilling));

                if (intOpen == 1)
                {
                    GISClass.General.CloseForm(typeof(FinalBilling));
                }
                FinalBilling childForm = new FinalBilling();
                childForm.Text = "Final Billing";
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
            if (nSw == 0)
            {
                nSw = 1;
                timer1.Enabled = true;
            }
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
                nCtr = 0;
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
                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "" || dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateChecked"].Value.ToString() != "" ||
                    dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["InvoiceNo"].Value.ToString() != "")
                {
                    tsbEdit.Enabled = false;
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
            pnlDataControl.Visible = false;
            pnlSCExtData.Visible = false; pnlSlashExtData.Visible = false;

            btnDataForm.Enabled = false; btnLSPreview.Enabled = false; btnLSPrinter.Enabled = false; btnFAXEMail.Enabled = false;//to make sure user can't click while still adding the record           
            txtLogNo.ReadOnly = true;
            txtLogNo.Text = "(New)";
            //Default Values
            rdoSAmbient.Checked = true; rdoRAmbient.Checked = true;

            //dtpSAPDate.Format = DateTimePickerFormat.Custom;
            //dtpSAPDate.CustomFormat = " ";

            dtpReceived.Value = DateTime.Now;
            dtpEntered.Value = DateTime.Now; dtpEntered.Enabled = false;
            txtSponsorID.Focus();

            strList.Clear(); strListQ.Clear(); strListSC.Clear();
            
            dtLogFM.Rows.Clear();
            dtSamples.Rows.Clear(); dtLogTests.Rows.Clear(); dtSampleSC.Rows.Clear(); dtBilling.Rows.Clear();
            dtSCExtData.Rows.Clear(); dtSlashExtData.Rows.Clear(); dtrLogTests.Enabled = true; dtSamplesAddl.Rows.Clear(); 

            ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["SlashNo"]).DataSource = null;
            ((DataGridViewComboBoxColumn)dgvSampleSC.Columns["ServiceCode"]).DataSource = null;

            cboQuotes.DataSource = null;
            btnAddSample.Enabled = true; btnEditSample.Enabled = false; btnDelSample.Enabled = true; btnSaveSample.Enabled = false; btnCancelSample.Enabled = false;
            if (txtSponsorID.Text == "260")
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

            //LoadSC();

            //SAMPLES & SERVICE CODE ASSIGNMENT
            //dtSampleSC.Rows.Clear(); 

            //bsSampleSC.DataSource = dtSampleSC;
            //bnSampleSC.BindingSource = bsSampleSC;
            //dgvSampleSC.DataSource = bsSampleSC;

            //dtSampleSC.Columns.Add("SlashNo", typeof(string));
            //dtSampleSC.Columns.Add("SC", typeof(string));
            //dtSampleSC.Columns.Add("Slash", typeof(string));
            //dtSampleSC.Columns.Add("ServiceCode", typeof(string));

            //dgvSampleSC.Columns.RemoveAt(0);
            //dgvSampleSC.Columns.RemoveAt(0);
            //dgvSampleSC.Columns.RemoveAt(0);
            //dgvSampleSC.Columns.RemoveAt(0);

            //DataGridViewComboBoxColumn cboSN = new DataGridViewComboBoxColumn();
            //DataGridViewComboBoxColumn cboSSC = new DataGridViewComboBoxColumn();
            //DataGridViewTextBoxColumn txtSlash = new DataGridViewTextBoxColumn();
            //DataGridViewTextBoxColumn txtSC = new DataGridViewTextBoxColumn();

            //dgvSampleSC.Columns.Add(cboSN);
            //dgvSampleSC.Columns.Add(cboSSC);
            //dgvSampleSC.Columns.Add(txtSlash);
            //dgvSampleSC.Columns.Add(txtSC);

            //dgvSampleSC.Columns[0].Name = "SlashNo";
            //dgvSampleSC.Columns[1].Name = "SC";
            //dgvSampleSC.Columns[2].Name = "Slash";
            //dgvSampleSC.Columns[3].Name = "ServiceCode";

            //dgvSampleSC.Columns["SlashNo"].Width = 75;
            //dgvSampleSC.Columns["SC"].Width = 60;
            //dgvSampleSC.Columns["SlashNo"].HeaderText = "Slash No.";
            //dgvSampleSC.Columns["SC"].HeaderText = "Service Code";

            //StandardDGVSetting(dgvSampleSC);
            //dgvSampleSC.Columns[0].HeaderText = "Slash No.";
            //dgvSampleSC.Columns[1].HeaderText = "Service Code";
            //dgvSampleSC.Columns[0].Width = 75;
            //dgvSampleSC.Columns[1].Width = 60;
            //dgvSampleSC.Columns[2].Visible = false;
            //dgvSampleSC.Columns[3].Visible = false;

            //Billing Information
            //dtTests.Rows.Clear(); dtTests.Columns.Clear();
            //dgvTests.Rows.Clear(); dgvTests.Columns.Clear();

            //DataGridViewTextBoxColumn dgvCol1 = new DataGridViewTextBoxColumn();
            //dgvCol1.Name = "QuoteNo";
            //DataGridViewTextBoxColumn dgvCol2 = new DataGridViewTextBoxColumn();
            //dgvCol2.Name = "ServiceCode";
            //DataGridViewTextBoxColumn dgvCol3 = new DataGridViewTextBoxColumn();
            //dgvCol3.Name = "ServiceDesc";
            //DataGridViewTextBoxColumn dgvCol4 = new DataGridViewTextBoxColumn();
            //dgvCol4.Name = "TestDesc1";
            //DataGridViewTextBoxColumn dgvCol5 = new DataGridViewTextBoxColumn();
            //dgvCol5.Name = "EstimatedFee";
            //DataGridViewTextBoxColumn dgvCol6 = new DataGridViewTextBoxColumn();
            //dgvCol6.Name = "ControlNo";
            //dgvTests.Columns.Add(dgvCol1);
            //dgvTests.Columns.Add(dgvCol2);
            //dgvTests.Columns.Add(dgvCol3);
            //dgvTests.Columns.Add(dgvCol4);
            //dgvTests.Columns.Add(dgvCol5);
            //dgvTests.Columns.Add(dgvCol6);

            //DataGridViewTextBoxColumn dBQty = new DataGridViewTextBoxColumn();
            //dBQty.HeaderText = "BILL QTY.";
            //dBQty.Width = 75;
            //dBQty.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvTests.Columns.Add(dBQty);

            //DataGridViewCheckBoxColumn dTest = new DataGridViewCheckBoxColumn();
            //dTest.HeaderText = "SELECT TEST";
            //dTest.TrueValue = 1;
            //dTest.FalseValue = 0;
            //dTest.Width = 85;
            //dgvTests.Columns.Add(dTest);

            //StandardDGVSetting(dgvTests);
            //dgvTests.Columns[0].HeaderText = "QUOTE NO.";
            //dgvTests.Columns[0].Width = 85;
            //dgvTests.Columns[1].HeaderText = "SC";
            //dgvTests.Columns[1].Width = 58;
            //dgvTests.Columns[2].HeaderText = "SERVICE DESCRIPTION";
            //dgvTests.Columns[2].Width = 270;
            //dgvTests.Columns[3].HeaderText = "TEST DESCRIPTION";
            //dgvTests.Columns[3].Width = 350;
            //dgvTests.Columns[4].Visible = false;
            //dgvTests.Columns[5].Visible = false;
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
            if (txtSponsorID.Text == "260")
            {
                btnAddTest.Enabled = true; btnDelTest.Enabled = true;
            }
            else
            {
                btnAddTest.Enabled = false; btnDelTest.Enabled = false;
            }
            if (strFileAccess != "FA")
                chkManager.Enabled = false;
            else
                chkManager.Enabled = true;
            chkCancelled.Enabled = false;
            txtSSFormNo.Enabled = false;
        }

        private void LoadSamples()
        {
            dtSamples = GISClass.Samples.LogSamplesData(Convert.ToInt32(txtLogNo.Text));
            bsSamples.DataSource = dtSamples;
            bnSamples.BindingSource = bsSamples;
            dgvSamples.DataSource = bsSamples;
        }

        private void LoadBillingRef()
        {
            dtBilling.Rows.Clear();
            dtBilling = GISClass.Samples.LogBillingRef(Convert.ToInt32(txtLogNo.Text));

            bsBilling.DataSource = dtBilling;
            dgvTests.DataSource = bsBilling;
            dgvTests.Columns["UnitPrice"].Visible = false;
            dgvTests.Columns["RushPrice"].Visible = false;
            dgvTests.Columns["ControlNo"].Visible = false;
        }

        private void LoadSlashExtData()
        {
            try
            {
                dtSlashExtData.Rows.Clear();
                DataTable dt = new DataTable();
                dt = GISClass.Samples.ExSlashSCExtData(Convert.ToInt32(txtLogNo.Text), Convert.ToInt16(((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text));
                if (dt == null || dt.Rows.Count == 0)
                    return;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dR = dtSlashExtData.NewRow();
                    dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                    dR["SlashNo"] = dt.Rows[i]["SlashNo"];
                    dR["SlashExtData1"] = dt.Rows[i]["SlashExtData1"];
                    dR["SlashExtData2"] = dt.Rows[i]["SlashExtData2"];
                    dR["SlashExtData3"] = dt.Rows[i]["SlashExtData3"];
                    dR["SlashExtData4"] = dt.Rows[i]["SlashExtData4"];
                    dR["SlashExtData5"] = dt.Rows[i]["SlashExtData5"];
                    dR["SlashExtData6"] = dt.Rows[i]["SlashExtData6"];
                    dR["SlashExtData7"] = dt.Rows[i]["SlashExtData7"];
                    dR["SlashExtData8"] = dt.Rows[i]["SlashExtData8"];
                    dR["SlashExtData9"] = dt.Rows[i]["SlashExtData9"];
                    dR["SlashExtData10"] = dt.Rows[i]["SlashExtData10"];
                    dtSlashExtData.Rows.Add(dR);
                }
                dtSlashExtData.AcceptChanges();
                bsSlashExtData.DataSource = dtSlashExtData;
                bnSlashExtData.BindingSource = bsSlashExtData;
            }
            catch { }
        }

        private void LoadSlashAddlData()
        {
            try
            {
                dtSamplesAddl.Rows.Clear();
                DataTable dt = new DataTable();
                dt = GISClass.Samples.ExSlashAddlData(Convert.ToInt32(txtLogNo.Text));//, dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString()
                if (dt == null || dt.Rows.Count == 0)
                    return;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dR = dtSamplesAddl.NewRow();
                    dR["SlashNo"] = dt.Rows[i]["SlashNo"];
                    dR["OtherDesc3"] = dt.Rows[i]["AddlData1"];
                    dR["OtherDesc4"] = dt.Rows[i]["AddlData2"];
                    dR["OtherDesc5"] = dt.Rows[i]["AddlData3"];
                    dR["OtherDesc6"] = dt.Rows[i]["AddlData4"];
                    dR["OtherDesc7"] = dt.Rows[i]["AddlData5"];
                    dR["OtherDesc8"] = dt.Rows[i]["AddlData6"];
                    dR["OtherDesc9"] = dt.Rows[i]["AddlData7"];
                    dR["OtherDesc10"] = dt.Rows[i]["AddlData8"];
                    dR["OtherDesc11"] = dt.Rows[i]["AddlData9"];
                    dR["OtherDesc12"] = dt.Rows[i]["AddlData10"];
                    dR["OtherDesc13"] = dt.Rows[i]["AddlData11"];
                    dR["OtherDesc14"] = dt.Rows[i]["AddlData12"];
                    dR["OtherDesc15"] = dt.Rows[i]["AddlData13"];
                    dtSamplesAddl.Rows.Add(dR);
                }
                dtSamplesAddl.AcceptChanges();
                bsSamplesAddl.DataSource = dtSamplesAddl;
            }
            catch { }
        }

        private void LoadSCExtData()
        {
            try
            {
                dtSCExtData.Rows.Clear(); 
                DataTable dt = new DataTable();
                dt = GISClass.Samples.ExSCExtData(Convert.ToInt32(txtLogNo.Text));
                if (dt == null || dt.Rows.Count == 0)
                    return;

                cboStudyDir.SelectedValue = Convert.ToInt16(dt.Rows[0]["StudyDirID"]);
                txtStudyNo.Text = dt.Rows[0]["StudyNo"].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (dt.Rows[i]["SCExtData" + (j + 1)].ToString() != "")
                        {

                            string strExtData = dt.Rows[i]["SCExtData" + (j + 1)].ToString();
                            string[] arrExtData = strExtData.Split(',');


                            DataRow dR = dtSCExtData.NewRow();
                            dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                            dR["StudyNo"] = dt.Rows[i]["StudyNo"];
                            dR["StudyDirID"] = dt.Rows[i]["StudyDirID"];
                            dR["SCExtDataLabel"] = arrExtData[0];
                            dR["SCExtDataValue"] = arrExtData[1];
                            dR["PrtNotes"] = dt.Rows[i]["PrtNotes"];
                            dR["NonPrtNotes"] = dt.Rows[i]["NonPrtNotes"];
                            dtSCExtData.Rows.Add(dR);
                        }
                        //dgvSCExtData.Rows[j].Cells["Label"].Value = arrExtData[0];
                        //dgvSCExtData.Rows[j].Cells["Value"].Value = arrExtData[1];
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
            dtAddl = GISClass.Samples.SlashAddlLabels(Convert.ToInt16(txtSponsorID.Text));
            if (dtAddl == null)
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

            txtLogNo.Text = dgvFile.CurrentRow.Cells[0].Value.ToString();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
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
            int nT = 0;
            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                if (dtLogTests.Rows[i].RowState.ToString() != "Deleted")
                {
                    nT++;
                }
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
            if (txtSponsorID.Text != "260")
            {
                byte nBill = 0;
                for (int i = 0; i < dtBilling.Rows.Count; i++)
                {
                    if (dtBilling.Rows[i].RowState.ToString() != "Deleted" && Convert.ToDecimal(dtBilling.Rows[i]["BillQty"]) > 0)
                    {
                        nBill = 1;
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
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
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
                    //txtLogNo.Text = GISClass.General.NewID("LogMaster", "GBLNo").ToString();
                    txtLogNo.Text = GISClass.General.NewGBLNo("LogMaster", "GBLNo").ToString();
                else
                    bsLogFM.EndEdit();

                sqlcmd.Parameters.AddWithValue("@nMode", nMode);
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
                    //sqlcmd.Parameters.Add(new SqlParameter("@AnaDoneDate", SqlDbType.DateTime));
                    //sqlcmd.Parameters["@AnaDoneDate"].Value = DateTime.Now;
                }
                else
                {
                    sqlcmd.Parameters["@AnaDone"].Value = DBNull.Value;
                    //sqlcmd.Parameters.Add(new SqlParameter("@AnaDoneDate", SqlDbType.DateTime));
                    //sqlcmd.Parameters["@AnaDoneDate"].Value = DBNull.Value;
                }

                sqlcmd.Parameters.Add(new SqlParameter("@MngrChecked", SqlDbType.Bit));
                if (chkManager.Checked)
                {
                    sqlcmd.Parameters["@MngrChecked"].Value = true;
                    //sqlcmd.Parameters.Add(new SqlParameter("@MgrCheckedDate", SqlDbType.DateTime));
                    //sqlcmd.Parameters["@MgrCheckedDate"].Value = DateTime.Now;
                }
                else
                {
                    sqlcmd.Parameters["@MngrChecked"].Value = DBNull.Value;
                    //sqlcmd.Parameters.Add(new SqlParameter("@MgrCheckedDate", SqlDbType.DateTime));
                    //sqlcmd.Parameters["@MgrCheckedDate"].Value = DBNull.Value;
                }
                if (chkCancelled.Checked)
                {
                    sqlcmd.Parameters.AddWithValue("@DteCancelled", mskDateCancelled.Text);
                }
                else
                    sqlcmd.Parameters.AddWithValue("@DteCancelled", DBNull.Value);
                if (txtILGBLNo.Text != "")
                    sqlcmd.Parameters.AddWithValue("@RetestNo", Convert.ToInt32(txtILGBLNo.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@RetestNo", DBNull.Value);
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
            }
            //SAMPLES DATA
            //Remove Deleted Records, if any
            DataTable dt = dtSamples.GetChanges(DataRowState.Deleted);
            if (dt != null && dt.Rows.Count > 0)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
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
            bsSlashExtData.EndEdit();
            //Add/Update Records
            string strSampleXML = "";
            if (dtSamples.Rows.Count > 0)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }
                for (int i = 0; i < dtSamples.Rows.Count; i++)
                {
                    if (dtSamples.Rows[i].RowState.ToString() == "Added" || dtSamples.Rows[i].RowState.ToString() == "Modified")
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
                        strSampleXML = strSampleXML + "<Value1>" + dtSamples.Rows[i]["OtherDesc1"].ToString() + "</Value1>";
                        strSampleXML = strSampleXML + "<Value2>" + dtSamples.Rows[i]["OtherDesc2"].ToString() + "</Value2>";

                        if (dgvSlashExtData.Rows.Count > 0)
                        {

                        }
                        //Columns 2 and 3 would be part of the Additional Data to be saved in the Extended Data XML column
                        if (dgvAddlData.Rows.Count != 0)
                        {
                            for (int k = 0; k < dtSamplesAddl.Rows.Count; k++)
                            {
                                if (dtSamples.Rows[i]["SlashNo"].ToString().Trim() == dtSamplesAddl.Rows[k]["SlashNo"].ToString().Trim())
                                {
                                    for (int j = 1; j < 14; j++)
                                    {
                                        try
                                        {
                                            if (dtSamplesAddl.Rows[k][j] == null || dtSamplesAddl.Rows[k][j].ToString().Trim() == "")
                                                strSampleXML = strSampleXML + "<Value" + (j + 2).ToString().Trim() + ">" + "" + "</Value" + (j + 2).ToString().Trim() + ">";
                                            else
                                                strSampleXML = strSampleXML + "<Value" + (j + 2).ToString().Trim() + ">" + dtSamplesAddl.Rows[k][j].ToString().Trim() + "</Value" + (j + 2).ToString().Trim() + ">";
                                        }
                                        catch { }
                                    }
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
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
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
            bsSCExtData.EndEdit();
            if (dtSCExtData.Rows.Count > 0)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }

                int nV = 1;
                if (dtLogTests != null && dtLogTests.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLogTests.Rows.Count; i++)
                    {
                        string strSCExData = "<SCExtData>";
                        int nJ = dgvSCExtData.Rows.Count - 1;
                        for (int j = 0; j < nJ; j++)//dtSCExtData 
                        {
                            if (dtLogTests.Rows[i]["ServiceCode"].ToString() == dgvSCExtData.Rows[j].Cells["SC"].Value.ToString())// && dtLogTests.Rows[i].RowState.ToString() != "Deleted"
                            {
                                strSCExData = strSCExData + "<Value" + (nV).ToString().Trim() + ">" + dgvSCExtData.Rows[j].Cells["Label"].Value.ToString() + "," + dgvSCExtData.Rows[j].Cells["Value"].Value.ToString() +
                                        "</Value" + (nV).ToString().Trim() + ">";
                                nV++;
                            }
                        }
                        if (strSCExData != "")
                        {
                            if (txtPrtNotes.Text.Trim() != "")
                                strSCExData = strSCExData + "<PrintingNotes>" + dtSCExtData.Rows[i]["PrtNotes"].ToString() + "</PrintingNotes>";
                            else
                                strSCExData = strSCExData + "<PrintingNotes></PrintingNotes>";
                            if (txtNonPrtNotes.Text.Trim() != "")
                                strSCExData = strSCExData + "<NonPrintingNotes>" + dtSCExtData.Rows[i]["NonPrtNotes"].ToString() + "</NonPrintingNotes>";
                            else
                                strSCExData = strSCExData + "<NonPrintingNotes></NonPrintingNotes>";
                            strSCExData = strSCExData + "</SCExtData>";

                            SqlCommand sqlcmd = new SqlCommand();
                            sqlcmd.Connection = sqlcnn;

                            sqlcmd.CommandType = CommandType.StoredProcedure;
                            sqlcmd.CommandText = "spUpdSCExtData";
                            sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(txtLogNo.Text));
                            sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dtLogTests.Rows[i]["ServiceCode"].ToString()));
                            sqlcmd.Parameters.AddWithValue("@StudyNo", dtSCExtData.Rows[i]["StudyNo"].ToString());
                            sqlcmd.Parameters.AddWithValue("@StudyDirID", dtSCExtData.Rows[i]["StudyDirID"].ToString());
                            sqlcmd.Parameters.AddWithValue("@SCExt", strSCExData);

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
                            nV = 1;
                        }
                    }
                }
                sqlcnn.Close(); sqlcnn.Dispose();
            }
            // SC/Slash 
            //Remove Deleted Records, if any
            dt = dtSampleSC.GetChanges(DataRowState.Deleted);
            if (dt != null && dt.Rows.Count > 0)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
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
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
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
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }
                n = GISClass.Samples.DelLogBillingRef(Convert.ToInt32(txtLogNo.Text));
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
            GISClass.Samples.AddTestStatus(Convert.ToInt32(txtLogNo.Text));
            if (nFR == 1)
            {
                nFR = 0;
                SendKeys.Send("{F12}");
                return;
            }
            InitializeFile();
        }

        private void InitializeFile()
        {
            AddEditMode(false); //Initialize Toolbar
            LoadRecords();
            if (txtLogNo.Text != "(New)")
                GISClass.General.FindRecord("GBLNo", txtLogNo.Text, bsFile, dgvFile);
            btnClose.Visible = true;
            btnLSPreview.Enabled = true; btnLSPrinter.Enabled = true; btnDataForm.Enabled = true; 
            btnAddSample.Enabled = true; btnEditSample.Enabled = true; btnDelSample.Enabled = true; btnSaveSample.Enabled = false; btnCancelSample.Enabled = false;
            LoadData();
        }

        private void CreateInvoice()
        {
            //Create Invoice
            DataTable dt = new DataTable();

            //string strComBefTable = "";
            //DataTable dt = GISClass.Quotations.QuoteComments(txtQuoteNo.Text, Convert.ToInt16(txtRevNo.Text));
            //if (dt != null && dt.Rows.Count > 0)
            //    strComBefTable = dt.Rows[0]["CommentsBeforeTable"].ToString();

            string strID = GISClass.General.NewID("InvMasterTemp", "TempID").ToString();
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                InitializeFile();
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@nMode", 1);
            sqlcmd.Parameters.AddWithValue("@TID", Convert.ToInt32(strID));
            sqlcmd.Parameters.AddWithValue("@InvNo", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@InvDate", DateTime.Now);
            sqlcmd.Parameters.AddWithValue("@InvType", 2);
            sqlcmd.Parameters.AddWithValue("@Header", "");
            sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@ConID", Convert.ToInt16(txtContactID.Text.Trim()));
            sqlcmd.Parameters.AddWithValue("@InvPrt", "");
            sqlcmd.Parameters.AddWithValue("@InvNonPrt", "");
            //sqlcmd.Parameters.AddWithValue("@DateRev", DBNull.Value);
            //sqlcmd.Parameters.AddWithValue("@DateCanc", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditInvMstrTemp";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException exSQL)
            {
                if (exSQL.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                {
                    MessageBox.Show(exSQL.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlcmd.Dispose(); dt.Dispose();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlcmd.Dispose(); dt.Dispose();
                return;
            }

            sqlcmd.Dispose(); dt.Dispose();

            //Get No. of Samples
            string strSlashNo = "";
            DataView dvw = dtSamples.DefaultView;
            dvw.Sort = "SlashNo";
            DataTable dT = dvw.ToTable();
            if (dT.Rows.Count > 0)
            {
                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    strSlashNo = dT.Rows[i]["SlashNo"].ToString();
                }
            }
            dT.Dispose();
            //Extract Bill Qty
            string strOBQty = "0";
            byte nComp = 0;
            if (strSlashNo.IndexOf("-") >= 0)
            {
                int nIdx = strSlashNo.IndexOf("-");
                strOBQty = strSlashNo.Substring(nIdx + 1, strSlashNo.Length - (nIdx + 1));
                nComp = 1;
            }
            else
            {
                strOBQty = strSlashNo;
            }
            //Get Report No if available
            int nRptNo = 0, nCtrlNo = 0;
            decimal nBQty = 0, nUP = 0, nAmt = 0;
            if (dtLogTests.Rows[0]["ReportNo"].ToString() != "")
            {
                nRptNo = Convert.ToInt32(dtLogTests.Rows[0]["ReportNo"].ToString());
            }
            string strPONo = dtLogTests.Rows[0]["PONo"].ToString();
            Int32 nGBL = Convert.ToInt32(dtLogFM.Rows[0]["LogNo"].ToString());
            string strQ = "", strRNo = "0";
            for (int i = 0; i < dtBilling.Rows.Count; i++)
            {
                if (dtBilling.Rows[i].RowState.ToString() != "Deleted")
                {
                    strQ = dtBilling.Rows[i]["QuoteNo"].ToString();
                    int nI = strQ.IndexOf("R");
                    string strQNo = strQ.Substring(0, nI - 1);
                    strRNo = strQ.Substring(nI + 1, strQ.Length - (nI + 1));

                    nBQty = Convert.ToDecimal(dtBilling.Rows[i]["BillQty"]);
                    nUP = Convert.ToDecimal(dtBilling.Rows[i]["UnitPrice"]);
                    nAmt = nBQty * nUP;
                    nCtrlNo = Convert.ToInt16(dtBilling.Rows[i]["ControlNo"]);

                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;
                    sqlcmd.Parameters.AddWithValue("@nMode", 1);
                    sqlcmd.Parameters.AddWithValue("@TID", Convert.ToInt32(strID));
                    sqlcmd.Parameters.AddWithValue("@InvID", i);
                    sqlcmd.Parameters.AddWithValue("@QuoteNo", strQ);
                    sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
                    sqlcmd.Parameters.AddWithValue("@CtrlNo", nCtrlNo);
                    sqlcmd.Parameters.AddWithValue("@PONo", strPONo);
                    sqlcmd.Parameters.AddWithValue("@GBLNo", nGBL);
                    if (nRptNo == 0)
                        sqlcmd.Parameters.AddWithValue("@RptNo", DBNull.Value);
                    else
                        sqlcmd.Parameters.AddWithValue("@RptNo", nRptNo);
                    sqlcmd.Parameters.AddWithValue("@SC", dtBilling.Rows[i]["ServiceCode"]);
                    sqlcmd.Parameters.AddWithValue("@SCType", 2);
                    sqlcmd.Parameters.AddWithValue("@BillQty", nBQty);
                    sqlcmd.Parameters.AddWithValue("@UPrice", nUP);
                    sqlcmd.Parameters.AddWithValue("@Amt", nAmt);
                    sqlcmd.Parameters.AddWithValue("@Adj", 0);
                    sqlcmd.Parameters.AddWithValue("@AcctID", 0);
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spAddEditInvDtlsTemp";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        if (ex.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            if (chkReTest.Checked == false)
            {
                //Additional Charges 554 and 1079
                //Extract Unit Prices
                //554 
                nCtrlNo = 0;
                DataTable dtX = GISClass.Quotations.QuoteUnitPrice(strQ, Convert.ToInt16(strRNo), 554);
                if (dtX != null)
                {
                    nUP = Convert.ToDecimal(dtX.Rows[0]["UnitPrice"]);
                    nCtrlNo = Convert.ToInt16(dtX.Rows[0]["ControlNo"]);
                    dtX.Dispose();
                }
                nAmt = nUP * Convert.ToDecimal(strOBQty);
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@nMode", 1);
                sqlcmd.Parameters.AddWithValue("@TID", Convert.ToInt32(strID));
                sqlcmd.Parameters.AddWithValue("@InvID", 0);
                sqlcmd.Parameters.AddWithValue("@QuoteNo", strQ);
                sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
                sqlcmd.Parameters.AddWithValue("@CtrlNo", nCtrlNo);
                sqlcmd.Parameters.AddWithValue("@PONo", strPONo);
                sqlcmd.Parameters.AddWithValue("@GBLNo", nGBL);
                if (nRptNo == 0)
                    sqlcmd.Parameters.AddWithValue("@RptNo", DBNull.Value);
                else
                    sqlcmd.Parameters.AddWithValue("@RptNo", nRptNo);
                sqlcmd.Parameters.AddWithValue("@SC", 554);
                sqlcmd.Parameters.AddWithValue("@SCType", 2);
                sqlcmd.Parameters.AddWithValue("@BillQty", Convert.ToDecimal(strOBQty));
                sqlcmd.Parameters.AddWithValue("@UPrice", nUP);
                sqlcmd.Parameters.AddWithValue("@Amt", nAmt);
                sqlcmd.Parameters.AddWithValue("@Adj", 0);
                sqlcmd.Parameters.AddWithValue("@AcctID", 0);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditInvDtlsTemp";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch
                { }
                //1079 Composite
                if (nComp == 1)
                {
                    nCtrlNo = 0;
                    dtX = GISClass.Quotations.QuoteUnitPrice(strQ, Convert.ToInt16(strRNo), 1079);
                    if (dtX != null)
                    {
                        nUP = Convert.ToDecimal(dtX.Rows[0]["UnitPrice"]);
                        nCtrlNo = Convert.ToInt16(dtX.Rows[0]["ControlNo"]);
                        dtX.Dispose();
                    }
                    nAmt = nUP * Convert.ToDecimal(strOBQty);
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;
                    sqlcmd.Parameters.AddWithValue("@nMode", 1);
                    sqlcmd.Parameters.AddWithValue("@TID", Convert.ToInt32(strID));
                    sqlcmd.Parameters.AddWithValue("@InvID", 0);
                    sqlcmd.Parameters.AddWithValue("@QuoteNo", strQ);
                    sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
                    sqlcmd.Parameters.AddWithValue("@CtrlNo", nCtrlNo);
                    sqlcmd.Parameters.AddWithValue("@PONo", strPONo);
                    sqlcmd.Parameters.AddWithValue("@GBLNo", nGBL);
                    if (nRptNo == 0)
                        sqlcmd.Parameters.AddWithValue("@RptNo", DBNull.Value);
                    else
                        sqlcmd.Parameters.AddWithValue("@RptNo", nRptNo);
                    sqlcmd.Parameters.AddWithValue("@SC", 1079);
                    sqlcmd.Parameters.AddWithValue("@SCType", 2);
                    sqlcmd.Parameters.AddWithValue("@BillQty", Convert.ToDecimal(strOBQty));
                    sqlcmd.Parameters.AddWithValue("@UPrice", nUP);
                    sqlcmd.Parameters.AddWithValue("@Amt", nAmt);
                    sqlcmd.Parameters.AddWithValue("@Adj", 0);
                    sqlcmd.Parameters.AddWithValue("@AcctID", 0);
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spAddEditInvDtlsTemp";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch
                    { }
                }
            }
            //553 - Electronic Reporting Fee
            if (nRptNo != 0)
            {
                nCtrlNo = 0;
                DataTable dtER = GISClass.Quotations.QuoteUnitPrice(strQ, Convert.ToInt16(strRNo), 553);
                if (dtER != null)
                {
                    nUP = Convert.ToDecimal(dtER.Rows[0]["UnitPrice"]);
                    nCtrlNo = Convert.ToInt16(dtER.Rows[0]["ControlNo"]);
                    dtER.Dispose();
                }
                nAmt = nUP * 1; //Convert.ToDecimal(strOBQty);
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@nMode", 1);
                sqlcmd.Parameters.AddWithValue("@TID", Convert.ToInt32(strID));
                sqlcmd.Parameters.AddWithValue("@InvID", 0);
                sqlcmd.Parameters.AddWithValue("@QuoteNo", strQ);
                sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
                sqlcmd.Parameters.AddWithValue("@CtrlNo", nCtrlNo);
                sqlcmd.Parameters.AddWithValue("@PONo", strPONo);
                sqlcmd.Parameters.AddWithValue("@GBLNo", nGBL);
                if (nRptNo == 0)
                    sqlcmd.Parameters.AddWithValue("@RptNo", DBNull.Value);
                else
                    sqlcmd.Parameters.AddWithValue("@RptNo", nRptNo);
                sqlcmd.Parameters.AddWithValue("@SC", 553);
                sqlcmd.Parameters.AddWithValue("@SCType", 2);
                sqlcmd.Parameters.AddWithValue("@BillQty", 1);//Convert.ToDecimal(strOBQty)
                sqlcmd.Parameters.AddWithValue("@UPrice", nUP);
                sqlcmd.Parameters.AddWithValue("@Amt", nAmt);
                sqlcmd.Parameters.AddWithValue("@Adj", 0);
                sqlcmd.Parameters.AddWithValue("@AcctID", 0);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditInvDtlsTemp";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch
                { }
            }
            MessageBox.Show("Invoice entry successfully created.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveLogTests(int cI, int cMode)
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
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
                pnlIL.Visible = false;
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
            dgvSponsors.Visible = false; dgvContacts.Visible = false; pnlSCExtData.Visible = false; pnlSlashExtData.Visible = false;
            pnlQuotes.Visible = false;
            strList.Clear(); strListSC.Clear(); strListQ.Clear(); nQ = 0; nQu = 0; 
        }
       
        private void picSponsors_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadSponsorsDDL();
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSponsors.Top = 75;
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
                row["QuotationNo"] = "2017.1223";//2015.0992 "2012.0464"; // for maintenance
                dt.Rows.InsertAt(row, 0);
                cboQuotes.DisplayMember = "QuotationNo";
                cboQuotes.ValueMember = "QuotationNo";
            }
            else
            {
                dt = GISClass.Quotations.LoadQuotes(Convert.ToInt16(txtSponsorID.Text));
                cboQuotes.DisplayMember = "QuotationNo";
                cboQuotes.ValueMember = "QuotationNo";

                DataRow row = dt.NewRow();
                row["QuotationNo"] = "-select-";
                dt.Rows.InsertAt(row, 0);
            }
            cboQuotes.DataSource = dt;
            cboQuotes.Refresh();
            pnlQuotes.Visible = true;
        }

        //private void LoadPONo()
        //{
        //    dgvPONo.DataSource = null;

        //    dtPONo = GISClass.PO.PODDL(Convert.ToInt16(txtSponsorID.Text));
        //    if (dtPONo != null)
        //    {
        //        dgvPONo.DataSource = dtPONo;
        //        StandardDGVSetting(dgvPONo);
        //    }
        //}

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = GISClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                cboQuotes.DataSource = null; dgvPONo.DataSource = null; //dgvTests.Rows.Clear();
                LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                LoadPO();
                if (txtSponsorID.Text == "260")
                {
                    btnAddTest.Enabled = true; btnDelTest.Enabled = true;
                }
                else
                {
                    btnAddTest.Enabled = false; btnDelTest.Enabled = false;
                }
                dgvSponsors.Visible = false; txtContact.Text = ""; txtContactID.Text = "";
                strList.Clear(); strListQ.Clear(); strListSC.Clear();
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
                if (txtSponsor.Text.IndexOf("INGREDION") != -1)
                {
                    if (dtPONo != null && dtPONo.Rows.Count > 0)
                        txtILPO.Text = dtPONo.Rows[0]["PONo"].ToString();
                    else
                        txtILPO.Text = "";

                    if (dtFillCodes == null || dtFillCodes.Rows.Count == 0)
                    {
                        dtFillCodes = null;
                        dtFillCodes = GISClass.Samples.IngredionManifest();
                        dgvFillCodes.DataSource = dtFillCodes;
                        dgvFillCodes.Columns["FillCode"].Width = 94;
                    }
                    pnlRecord.Enabled = false; pnlIL.Visible = true; pnlIL.BringToFront(); pnlIL.Location = new Point(200, 200); txtReminder.Text = "";
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

            dtContacts = GISClass.Sponsors.ContactsDDL(cSpID);
            if (dtContacts == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvContacts.DataSource = dtContacts;
            StandardDGVSetting(dgvContacts);
            dgvContacts.Columns[0].Width = 369;
            dgvContacts.Columns[1].Visible = false;
        }

        private void txtSponsorIDEnterHandler(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void txtContactIDEnterHandler(object sender, EventArgs e)
        {
            dgvContacts.Visible = false;
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSponsors;
                dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvSponsors, dvwSponsors);
            }
        }

        private void txtContactEnterHandler(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    dgvContacts.Visible = true; dgvContacts.BringToFront();// dgvContacts.Top = 113;
                }
                catch { }
            }
        }

        private void txtContactID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                    try
                    {
                        txtContact.Text = GISClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
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
            //dtSamples.Rows.Clear(); dtSampleSC.Rows.Clear(); dtLogTests.Rows.Clear(); dtBilling.Rows.Clear();

            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
            cboQuotes.DataSource = null; dgvPONo.DataSource = null;
            LoadPO();
            if (txtSponsorID.Text == "260")
            {
                btnAddTest.Enabled = true; btnDelTest.Enabled = true;
            }
            else
            {
                btnAddTest.Enabled = false; btnDelTest.Enabled = false;
            }
            dgvSponsors.Visible = false; txtContact.Text = ""; txtContactID.Text = "";
            strList.Clear(); strListQ.Clear(); strListSC.Clear();
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
            if (txtSponsorID.Text == "260") //GIBRALTAR LABORATORIES
            {
                btnAddTest.Enabled = true; btnDelTest.Enabled = true;
            }
            else
            {
                btnAddTest.Enabled = false; btnDelTest.Enabled = false;
            }
            if (txtSponsor.Text.IndexOf("INGREDION") != -1)
            {
                if (dtPONo != null && dtPONo.Rows.Count > 0)
                    txtILPO.Text = dtPONo.Rows[0]["PONo"].ToString();
                else
                    txtILPO.Text = "";
                if (dtFillCodes == null || dtFillCodes.Rows.Count == 0)
                {
                    dtFillCodes = null;
                    dtFillCodes = GISClass.Samples.IngredionManifest();
                    dgvFillCodes.DataSource = dtFillCodes;
                    dgvFillCodes.Columns["FillCode"].Width = 94;
                }
                pnlRecord.Enabled = false; pnlIL.Visible = true; pnlIL.BringToFront(); pnlIL.Location = new Point(200, 200); txtReminder.Text = "";
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
                cboQuotes.DataSource = null; dgvPONo.DataSource = null; //dgvTests.Rows.Clear();
                LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                LoadPO();
                if (txtSponsorID.Text == "260")
                {
                    btnAddTest.Enabled = true; btnDelTest.Enabled = true;
                }
                else
                {
                    btnAddTest.Enabled = false; btnDelTest.Enabled = false;
                }
                dgvSponsors.Visible = false; txtContact.Text = ""; txtContactID.Text = "";
                strList.Clear(); strListQ.Clear(); strListSC.Clear();
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
                if (txtSponsor.Text.IndexOf("INGREDION") != -1)
                {
                    if (dtPONo != null && dtPONo.Rows.Count > 0)
                        txtILPO.Text = dtPONo.Rows[0]["PONo"].ToString();
                    else
                        txtILPO.Text = "";
                    if (dtFillCodes == null || dtFillCodes.Rows.Count == 0)
                    {
                        dtFillCodes = null;
                        dtFillCodes = GISClass.Samples.IngredionManifest();
                        dgvFillCodes.DataSource = dtFillCodes;
                        dgvFillCodes.Columns["FillCode"].Width = 94;
                    }
                    pnlRecord.Enabled = false; pnlIL.Visible = true; pnlIL.BringToFront(); pnlIL.Location = new Point(182, 80); txtReminder.Text = "";
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
                dgvContacts.Visible = false;
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
            dgvContacts.Visible = false;
        }

        private void btnTests_Click(object sender, EventArgs e)
        {
            if (dgvTests.Rows.Count == 0)
                return;

            List<string> strLSC = new List<string>(); //SC list to be added
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
                                //if (dtBilling.Rows[j].RowState.ToString() != "Deleted")
                                //{
                                    if (dtBilling.Rows[j].RowState.ToString() != "Deleted" && dtLogTests.Rows[i]["ServiceCode"].ToString() == dtBilling.Rows[j]["ServiceCode"].ToString() && dtBilling.Rows[j]["SelectedTest"].ToString() == "True") //check if SC is selected dgvTests.Rows[j].Cells[5].Value.ToString() 
                                    {
                                        nSC = 1;  //mark if existing
                                        //MessageBox.Show(dtLogTests.Rows[i]["ServiceCode"].ToString()  + ", " + dtBilling.Rows[j]["ServiceCode"].ToString() + ", " + dtBilling.Rows[j]["SelectedTest"].ToString());
                                        break;
                                    }
                                //}
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
                //if (dtBilling.Rows[i].RowState.ToString() != "Deleted")
                //{
                    if (dtBilling.Rows[i].RowState.ToString() != "Deleted" && dtBilling.Rows[i]["SelectedTest"].ToString() == "True") //selected dgvTests.Rows[i].Cells[5].Value.ToString() == "True"
                    {
                        strList.Add(dtBilling.Rows[i]["ServiceCode"].ToString() + "-" + dtBilling.Rows[i]["QuoteNo"].ToString()); //add to Master SC List dgvTests.Rows[i].Cells["QuoteNo"].Value.ToString()
                        nT += 1;
                    }
                //}
            }

            if (strList.Count == 0) //no items selected, exit from this routine
            {
                nQu = 0;
                return;
            }

            DataRow dr;
            strList.Sort();//sort the Master SC List
            strListSC.Clear(); strListQ.Clear(); //initialize lists for SC and Quotes

            int n = 0;
            int idx = 0;
            string strSave = "";           
            for (int i = 0; i < strList.Count; i++)
            {
                n = strList[i].IndexOf("-");
                if (strList[i].Substring(0, n) != strSave)
                {
                    strSave = strList[i].Substring(0, n);
                    dr = dtSCMaster.NewRow();
                    dr["ServiceCode"] = strSave;
                    dtSCMaster.Rows.Add(dr);
                    strListSC.Add(strSave);
                    strListQ.Add(strList[i].Substring(n + 1, strList[i].Length - (n + 1)));
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
                strListQ.Add(strList[idx].Substring(n + 1, strList[idx].Length - (n + 1)));
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
                    int nDuration = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(strListSC[i]));
                    //Tests to be done
                    strTestDesc = "";
                    for (int j = 0; j < dtBilling.Rows.Count; j++)
                    {
                        //if (dtBilling.Rows[j].RowState.ToString() != "Deleted")
                        //{
                            if (dtBilling.Rows[j].RowState.ToString() != "Deleted" && strListSC[i].ToString() == dtBilling.Rows[j]["ServiceCode"].ToString() && dtBilling.Rows[j]["SelectedTest"].ToString() == "True")
                            {
                                strTestDesc = strTestDesc + dtBilling.Rows[j]["TestDesc1"].ToString() + ", " + Environment.NewLine;
                                nQty = nQty + Convert.ToDecimal(dtBilling.Rows[j]["BillQty"]);
                                if (Convert.ToBoolean(dtBilling.Rows[j]["Rush"]) == true)
                                    chkRush.Checked = true;
                            }
                        //}
                    }
                    dr = dtLogTests.NewRow();
                    dr["ServiceCode"] = Convert.ToInt16(strListSC[i]);
                    dr["ServiceDesc"] = "";
                    dr["ProtocolNo"] = "";
                    dr["StartDate"] = DateTime.Now;
                    dr["EndDate"] = DateTime.Now.AddDays(nDuration);
                    dr["QuotationNo"] = strListQ[i];
                    dr["BillQty"] = nQty;
                    dr["TestSamples"] = "";
                    dr["Slashes"] = "";
                    dr["PONo"] = "";
                    dr["BookNo"] = 0;
                    dr["EC"] = false;
                    dr["ECCompType"] = DBNull.Value;
                    dr["ECLength"] = DBNull.Value;
                    dr["ECEndDate"] = DBNull.Value;
                    dr["DateSampled"] = DBNull.Value;
                    dr["QuoteFlag"] = "1";
                    dr["ReportNo"] = 0;
                    dr["AddlNotes"] = strTestDesc.Substring(0, strTestDesc.Length - 2);
                    dtLogTests.Rows.Add(dr);
                }
                else
                {
                    //Tests to be done
                    strTestDesc = "";
                    for (int j = 0; j < dtBilling.Rows.Count; j++)
                    {
                        //if (dtBilling.Rows[j].RowState.ToString() != "Deleted")
                        //{
                            if (dtBilling.Rows[j].RowState.ToString() != "Deleted" && strListSC[i].ToString() == dtBilling.Rows[j]["ServiceCode"].ToString() && dtBilling.Rows[j]["SelectedTest"].ToString() == "True")
                            {
                                strTestDesc = strTestDesc + dtBilling.Rows[j]["TestDesc1"].ToString() + ", " + Environment.NewLine ;
                                nQty = nQty + Convert.ToDecimal(dtBilling.Rows[j]["BillQty"]);
                                if (dtBilling.Rows[j]["Rush"] != DBNull.Value && Convert.ToBoolean(dtBilling.Rows[j]["Rush"]) == true)
                                    chkRush.Checked = true;
                            }
                        //}
                    }
                    int nI = dtLogTests.Rows.IndexOf(foundRows[0]);
                    dtLogTests.Rows[nI]["BillQty"] = nQty;
                    dtLogTests.Rows[nI]["QuotationNo"] = strListQ[i];
                    dtLogTests.Rows[nI]["AddlNotes"] = strTestDesc.Substring(0, strTestDesc.Length - 2);
                }
            }

            //DataTable dt = dtLogTests.GetChanges(DataRowState.Deleted);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    foreach (DataRow dRow in dt.Rows)
            //    {
            //        dtLogTestsDel.ImportRow(dRow);
            //    }
            //}

            DataView dvw = new DataView(dtLogTests);
            dvw.RowFilter = "ServiceCode <> 0";
            dtLogTests = dvw.ToTable();

            dtLogTests.DefaultView.ToTable(true, "ServiceCode");
            bsLogTests.DataSource = dtLogTests;
            //try
            //{
            dtrLogTests.DataSource = bsLogTests;//to be resolved - this error comes out when I initially populate the SC ComboBox
            //}
            //catch { }
            ////SAMPLES & SERVICE CODE ASSIGNMENT
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
                    //dtSampleSC.Rows[row.Index].Delete();
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
            Process.Start(Application.StartupPath + @"\c_cs_alpha.pdf");
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
                GISClass.General.DGVSetUp(dgvContacts, dvwContacts, 369);
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

        //private void dtpSAPDate_ValueChanged(object sender, EventArgs e)
        //{
        //    dtpSAPDate.Format = DateTimePickerFormat.Custom;
        //    dtpSAPDate.CustomFormat = "MM/dd/yyyy";
        //}

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

            //Upload Data Labels

            //DataTable dt = GISClass.Samples.ExSCExtDataLabels(Convert.ToInt16(txtSCExt.Text), Convert.ToInt16(txtSponsorID.Text));
            //if (dt == null || dt.Rows.Count == 0)
            //{
            //    MessageBox.Show("No defined labels for this Sponsor/Service Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            //if (dt.Rows[0]["Label1"].ToString() == "")
            //{
            //    MessageBox.Show("No defined labels for this Sponsor/Service Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}


            //CHECK ROUTINE 8/21/15
            //===============
            DataTable dt = GISClass.Samples.ExExtDataLabels();
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

            var stringArr = dt.AsEnumerable().Select(r => r.Field<string>("DataLabelDesc")).ToArray();

            ((DataGridViewComboBoxColumn)dgvSCExtData.Columns["Label"]).DataSource = stringArr;

            //lblSCExt1.Text = dt.Rows[0]["Label1"].ToString();
            //lblSCExt2.Text = dt.Rows[0]["Label2"].ToString();
            //lblSCExt3.Text = dt.Rows[0]["Label3"].ToString();
            //lblSCExt4.Text = dt.Rows[0]["Label4"].ToString();
            //lblSCExt5.Text = dt.Rows[0]["Label5"].ToString();
            //lblSCExt6.Text = dt.Rows[0]["Label6"].ToString();
            //lblSCExt7.Text = dt.Rows[0]["Label7"].ToString();
            //lblSCExt8.Text = dt.Rows[0]["Label8"].ToString();
            //lblSCExt9.Text = dt.Rows[0]["Label9"].ToString();
            //lblSCExt10.Text = dt.Rows[0]["Label10"].ToString();

            DataRow[] foundRows;
            foundRows = dtSCExtData.Select("ServiceCode = " + ((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
            if (foundRows.Length == 0)
            {
                //txtStudyNo.Text = ""; cboStudyDir.SelectedIndex = 0;
                //txtSC1.Text = ""; txtSC2.Text = ""; txtSC3.Text = ""; txtSC4.Text = ""; txtSC5.Text = "";
                //txtSC6.Text = ""; txtSC7.Text = ""; txtSC8.Text = ""; txtSC9.Text = ""; txtSC10.Text = "";
            }
            else
            {
                DataTable dtX = new DataTable();
                dtX = foundRows.CopyToDataTable();
                dgvSCExtData.RowCount = dtX.Rows.Count + 1;
                try
                {
                    cboStudyDir.SelectedValue = Convert.ToInt16(dtX.Rows[0]["StudyDirID"]);
                    txtStudyNo.Text = dtX.Rows[0]["StudyNo"].ToString();
                    for (int r = 0; r < dtX.Rows.Count; r++)
                    {
                        dgvSCExtData.Rows[r].Cells["Label"].Value = dtX.Rows[r]["SCExtDataLabel"].ToString();
                        dgvSCExtData.Rows[r].Cells["Value"].Value = dtX.Rows[r]["SCExtDataValue"].ToString();
                        dgvSCExtData.Rows[r].Cells["SC"].Value = txtSCExt.Text;
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

        //private void dtrLogTests_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        //{
        //    DataTable dt = dsLogTests.Tables[0];
        //    switch (e.Control.Name)
        //    {
        //        case "cboSC":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["ServiceCode"] = e.Value;
        //                int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(e.Value));
        //                lblSCDesc.Text = GISClass.ServiceCodes.SCDesc(Convert.ToInt16(dt.Rows[e.ItemIndex]["ServiceCode"]));
        //                dt.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["StartDate"]).AddDays(n);
        //            }
        //            catch { }
        //            break;
        //        case "cboProtocolNo":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["ProtocolNo"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "dtpStartDate":
        //            try
        //            {
        //                int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dt.Rows[e.ItemIndex]["ServiceCode"]));
        //                dt.Rows[e.ItemIndex]["StartDate"] = e.Value;
        //                dt.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(e.Value).AddDays(n);
        //            }
        //            catch { }
        //            break;
        //        case "dtpEndDate":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["EndDate"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "txtPONo":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["PONo"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "txtSamplesNo":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["TestSamples"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "txtBillQty":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["BillQty"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "txtQuoteNo":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["QuoteNo"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "txtBookNo":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["BookNo"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "chkEC":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["EC"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "rdoECDD":
        //            try
        //            {
        //                if (nType == 1)
        //                {
        //                    dt.Rows[e.ItemIndex]["ECCompType"] = "D";
        //                    dt.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["StartDate"]).AddDays(Convert.ToInt16(dt.Rows[e.ItemIndex]["ECLength"]));
        //                    int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dt.Rows[e.ItemIndex]["ServiceCode"]));
        //                    dt.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
        //                }
        //            }
        //            catch { }
        //            break;
        //        case "rdoECWW":
        //            try
        //            {
        //                if (nType == 2)
        //                {
        //                    dt.Rows[e.ItemIndex]["ECCompType"] = "W";
        //                    dt.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["StartDate"]).AddDays(Convert.ToInt16(dt.Rows[e.ItemIndex]["ECLength"]) * 7);
        //                    int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dt.Rows[e.ItemIndex]["ServiceCode"]));
        //                    dt.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
        //                }
        //            }
        //            catch { }
        //            break;
        //        case "rdoECMM":
        //            try
        //            {
        //                if (nType == 3)
        //                {
        //                    dt.Rows[e.ItemIndex]["ECCompType"] = "M";
        //                    dt.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["StartDate"]).AddMonths(Convert.ToInt16(dt.Rows[e.ItemIndex]["ECLength"]));
        //                    int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dt.Rows[e.ItemIndex]["ServiceCode"]));
        //                    dt.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
        //                }
        //            }
        //            catch { }
        //            break;
        //        case "rdoECYY":
        //            try
        //            {
        //                if (nType == 4)
        //                {
        //                    dt.Rows[e.ItemIndex]["ECCompType"] = "Y";
        //                    dt.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["StartDate"]).AddYears(Convert.ToInt16(dt.Rows[e.ItemIndex]["ECLength"]));
        //                    int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dt.Rows[e.ItemIndex]["ServiceCode"]));
        //                    dt.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
        //                }
        //            }
        //            catch { }
        //            break;
        //        case "txtECLength":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["ECLength"] = Convert.ToInt16(e.Value);
        //                if (dt.Rows[e.ItemIndex]["ECCompType"].ToString() == "D")
        //                {
        //                    dt.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["StartDate"]).AddDays(Convert.ToInt16(e.Value));
        //                }
        //                else if (dt.Rows[e.ItemIndex]["ECCompType"].ToString() == "W")
        //                {
        //                    dt.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["StartDate"]).AddDays(Convert.ToInt16(e.Value) * 7);
        //                }
        //                else if (dt.Rows[e.ItemIndex]["ECCompType"].ToString() == "M")
        //                {
        //                    dt.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["StartDate"]).AddMonths(Convert.ToInt16(e.Value));
        //                }
        //                else if (dt.Rows[e.ItemIndex]["ECCompType"].ToString() == "Y")
        //                {
        //                    dt.Rows[e.ItemIndex]["ECEndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["StartDate"]).AddYears(Convert.ToInt16(e.Value));
        //                }
        //                int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dt.Rows[e.ItemIndex]["ServiceCode"]));
        //                dt.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dt.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
        //            }
        //            catch { }
        //            break;
        //        case "dtpECEndDate":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["ECEndDate"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "dtpDateSampled":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["DateSampled"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "chkComm":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["Comm"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        case "chkHoldTest":
        //            try
        //            {
        //                dt.Rows[e.ItemIndex]["HoldTest"] = e.Value;
        //            }
        //            catch { }
        //            break;
        //        default:
        //            MessageBox.Show("Error during ItemValuePushed unexpected control: " + e.Control.Name);
        //            break;
        //    }
        //}

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
            dgvSponsors.Visible = false; dgvContacts.Visible = false; pnlQuotes.Visible = false; pnlDataControl.Visible = false;
            AddEditMode(false); 
            FileAccess();
            try
            {
                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() != "" || dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "" ||
                    dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["InvoiceNo"].Value.ToString() != "")
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
       
        private void rdoECMM_CheckedChanged(object sender, EventArgs e)
        {
            //RadioButton rdo = sender as RadioButton;
            //nType = 3;
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Focus();
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Select();
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).SelectAll();
        }

        private void rdoECWW_CheckedChanged(object sender, EventArgs e)
        {
            //RadioButton rdo = sender as RadioButton;
            //nType = 2;
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Focus();
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Select();
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).SelectAll();
        }

        private void rdoECYY_CheckedChanged(object sender, EventArgs e)
        {
            //RadioButton rdo = sender as RadioButton;
            //nType = 4;
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Focus();
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Select();
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).SelectAll();
        }

        private void rdoECDD_CheckedChanged(object sender, EventArgs e)
        {
            //RadioButton rdo = sender as RadioButton;
            //nType = 1;
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Focus();
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).Select();
            ((TextBox)dtrLogTests.CurrentItem.Controls["txtECLength"]).SelectAll();
        }

        private void CopyRow()
        {
            DataRow drSample;
            if (dgvSamples.Rows.Count > 1)// || dgvSamples.CurrentRow.Cells[1].Value.ToString().Trim() == ""
            {
                if (dgvSamples.CurrentRow.Cells["SlashNo"].Value.ToString().Trim() != "")
                {
                    drSample = dtSamples.NewRow();
                    try
                    {
                        drSample["SlashNo"] = (Convert.ToInt16(dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value) + 1).ToString("000");
                        //drSample["SlashOrig"] = (Convert.ToInt16(dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value) + 1).ToString("000");
                    }
                    catch
                    {
                        drSample["SlashNo"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value.ToString();
                        //drSample["SlashOrig"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SlashNo"].Value.ToString();
                    }
                    drSample["SampleDesc"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["SampleDesc"].Value.ToString(); // dtSamples.Rows[dgvSamples.CurrentRow.Index]["SampleDesc"].ToString();
                    drSample["OtherDesc1"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["OtherDesc1"].Value.ToString(); // dtSamples.Rows[dgvSamples.CurrentRow.Index]["OtherDesc1"].ToString();
                    drSample["OtherDesc2"] = dgvSamples.Rows[dgvSamples.CurrentRow.Index].Cells["OtherDesc2"].Value.ToString(); // dtSamples.Rows[dgvSamples.CurrentRow.Index]["OtherDesc2"].ToString();
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
                decimal nPrice = GISClass.Quotations.QuoteRushPrice(strQNo, Convert.ToInt16(strRNo), Convert.ToInt16(strCNo));
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
                case "txtSC"://cboSC
                    try
                    {
                        dtLogTests.Rows[e.ItemIndex]["ServiceCode"] = e.Value;
                        int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(e.Value));
                        dtLogTests.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["StartDate"]).AddDays(n);
                        dtLogTests.Rows[e.ItemIndex]["EndDate"] = ((DateTimePicker)dtrLogTests.CurrentItem.Controls["dtpStartDate"]).Value.AddDays(n);
                        ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Visible = false;
                    }
                    catch { }
                    break;
                case "txtProtocolNo"://cboSC
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
                        int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
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
                case "txtPONo"://cboPONo
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
                            int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
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
                            int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
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
                            int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
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
                            int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
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
                        int n = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]));
                        dtLogTests.Rows[e.ItemIndex]["EndDate"] = Convert.ToDateTime(dtLogTests.Rows[e.ItemIndex]["ECEndDate"]).AddDays(n);
                    }
                    catch { }
                    break;
                case "txtSamples":
                    //try
                    //{
                        dtLogTests.Rows[e.ItemIndex]["TestSamples"] = e.Value;
                    //}
                    //catch { }
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
            dt = GISClass.Quotations.LoadQuotesSpArticle(Convert.ToInt16(txtSponsorID.Text),txtArticle.Text.Trim());

            cboQuotes.DisplayMember = "QuotationNo";
            cboQuotes.ValueMember = "QuotationNo";

            DataRow row = dt.NewRow();
            row["QuotationNo"] = "-select-";
            dt.Rows.InsertAt(row, 0);
            cboQuotes.DataSource = dt;
            //chkSelectAll.Checked = false;
        }

        private void txtQuotes_Enter(object sender, EventArgs e)
        {
            dgvQuotes.Visible = true; dgvQuotes.BringToFront();
        }

        private void picQuotes_Click(object sender, EventArgs e)
        {
            dgvQuotes.Visible = true; dgvQuotes.BringToFront();
        }

        //private void dtrSampleSC_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        //{
        //    DataTable dt = dtSampleSC;

        //    if (e.ItemIndex < dt.Rows.Count)
        //    {
        //        switch (e.Control.Name)
        //        {
        //            case "cboSNo":
        //                try
        //                {
        //                    e.Value = dt.Rows[e.ItemIndex]["SlashNo"].ToString();
        //                }
        //                catch { }
        //                break;
        //            case "cboSCSample":
        //                try
        //                {
        //                    e.Value = dt.Rows[e.ItemIndex]["SC"];
        //                }
        //                catch { }
        //                break;
        //        }
        //    }
        //}

        //private void dtrSampleSC_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        //{
        //    var combo4 = (ComboBox)e.DataRepeaterItem.Controls.Find("cboSNo", false)[0];
        //}

        //private void dtrSampleSC_ItemCloned(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        //{
            //((ComboBox)dtrSampleSC.CurrentItem.Controls["cboSNo"]).Items.Clear();
            //var combo4 = (ComboBox)e.DataRepeaterItem.Controls.Find("cboSNo", false)[0];
            //combo4.Items.AddRange(strListSSC.ToArray());
            //((ComboBox)dtrSampleSC.CurrentItem.Controls["cboSNo"]).Items.AddRange(strListSSC.ToArray());
        //}

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
                //cboS.Text = dtLogTests.Rows[e.DataRepeaterItem.ItemIndex]["Servicecode"].ToString();

                if (((TextBox)dtrLogTests.CurrentItem.Controls["txtQuote"]).Text == "1")
                {
                    cboS.Enabled = false; ((TextBox)dtrLogTests.CurrentItem.Controls["txtBillQty"]).ReadOnly = true;
                }
                else
                {
                    cboS.Enabled = true; ((TextBox)dtrLogTests.CurrentItem.Controls["txtBillQty"]).ReadOnly = false;
                }

                //if (dtLogTests.Rows[e.DataRepeaterItem.ItemIndex].RowState.ToString() == "Deleted")
                //{
                //    MessageBox.Show("deleted");
                //}
                //var cboP = (ComboBox)e.DataRepeaterItem.Controls.Find("cboPONo", false)[0];
                //cboP.Text = dtLogTests.Rows[e.DataRepeaterItem.ItemIndex]["PONo"].ToString();
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
                drSample["SlashNo"] = (i + 1).ToString() + "(" + GISClass.General.CompositeEntry(Convert.ToInt16(txtCount.Text)) + ")";
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
                //This block deletes all entries in the Slash/SC datagridview when a Slash is deleted/edited
                //if (dgvSampleSC.Rows.Count != 0)
                //{
                //    for (int i = 0; i < dgvSampleSC.Rows.Count; i++)
                //    {
                //        int nI = 0;
                //        for (int j = 0; j < strListSSC.Count; j++)
                //        {
                //            if (dgvSampleSC.Rows[i].Cells[0].Value.ToString() == strListSSC[j].ToString())
                //            {
                //                nI = 1;
                //                break;
                //            }
                //        }
                //        if (nI == 0)
                //            dgvSampleSC.Rows[i].Selected = true;
                //    }
                //    foreach (DataGridViewRow row in dgvSampleSC.SelectedRows)
                //    {
                //        dgvSampleSC.Rows.Remove(row);
                //    }
                //}
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

                    //if (nA == 0)
                    //{
                    //    dgvAddlData.Rows.Add(nS.ToString("000") + "-" + nE.ToString("000"));
                    //}
                }
                else if (chkSingleRow.Checked && chkAlpha.Checked)
                {
                    nR += 1;

                    DataRow drSample;
                    drSample = dtSamples.NewRow();
                    drSample["SlashNo"] = GISClass.General.CompositeEntry(nR);
                    drSample["SampleDesc"] = dgvSamples.CurrentRow.Cells[2].Value.ToString();
                    drSample["OtherDesc1"] = dgvSamples.CurrentRow.Cells[3].Value.ToString();
                    drSample["OtherDesc2"] = dgvSamples.CurrentRow.Cells[4].Value.ToString();
                    drSample["SlashID"] = 0;
                    dtSamples.Rows.Add(drSample);

                    //if (nA == 0)
                    //{
                    //    dgvAddlData.Rows.Add(GISClass.General.CompositeEntry(nR));
                    //}
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

                        //if (nA == 0)
                        //    dgvAddlData.Rows.Add(nS.ToString("000"));

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
                //dtLogTests.AcceptChanges();
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
            //e.Handled = true; //Allow it for the meantime
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

        private void cboQuotes_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //if (nMode != 0)
            //{
            //    DataTable dt = new DataTable();
            //    dt = GISClass.Quotations.LoadLoginTests(cboQuotes.Text.ToString());
            //    if (dt == null)
            //    {
            //        MessageBox.Show("Connection problems. Please contact your system administrator.");
            //        return;
            //    }
            //    if (dgvTests.Rows.Count != 0)
            //    {
            //        byte nM = 0;
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            for (int j = 0; j < dtBilling.Rows.Count; j++)
            //            {
            //                if (dt.Rows[i]["QuoteNo"].ToString().Trim() == dtBilling.Rows[j]["QuoteNo"].ToString().Trim() && dt.Rows[i]["ControlNo"].ToString().Trim() == dtBilling.Rows[j]["ControlNo"].ToString().Trim())
            //                {
            //                    nM = 1;
            //                    break;
            //                }
            //            }
            //            if (nM == 0)
            //            {
            //                DataRow dR;
            //                dR = dtBilling.NewRow();
            //                dR["QuoteNo"] = dt.Rows[i]["QuoteNo"];
            //                dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
            //                dR["ServiceDesc"] = dt.Rows[i]["ServiceDesc"];
            //                dR["TestDesc1"] = dt.Rows[i]["TestDesc1"];
            //                dR["UnitDesc"] = dt.Rows[i]["UnitDesc"];
            //                dR["BillQty"] = 0;
            //                dR["SelectedTest"] = false;
            //                dR["Rush"] = false;
            //                dR["UnitPrice"] = dt.Rows[i]["UnitPrice"];
            //                dR["RushPrice"] = dt.Rows[i]["RushPrice"];
            //                dR["ControlNo"] = dt.Rows[i]["ControlNo"];
            //                dtBilling.Rows.Add(dR);
            //            }
            //            nM = 0;
            //        }
            //    }
            //    else
            //    {
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            DataRow dR;
            //            dR = dtBilling.NewRow();
            //            dR["QuoteNo"] = dt.Rows[i]["QuoteNo"];
            //            dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
            //            dR["ServiceDesc"] = dt.Rows[i]["ServiceDesc"];
            //            dR["TestDesc1"] = dt.Rows[i]["TestDesc1"];
            //            dR["UnitDesc"] = dt.Rows[i]["UnitDesc"];
            //            dR["BillQty"] = 0;
            //            dR["SelectedTest"] = false;
            //            dR["Rush"] = false;
            //            dR["UnitPrice"] = dt.Rows[i]["UnitPrice"];
            //            dR["RushPrice"] = dt.Rows[i]["RushPrice"];
            //            dR["ControlNo"] = dt.Rows[i]["ControlNo"];
            //            dtBilling.Rows.Add(dR);
            //        }
            //    }
            //    dt.Dispose();
            //    dgvTests.DataSource = dtBilling;
            //}
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
                dtPONo = GISClass.PO.PODDL(Convert.ToInt16(txtSponsorID.Text));
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
            string strSampFrom = "";
            string strSampTo = "";

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
                dr["ServiceCode"] = 0;//----
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
            dtSCMaster = GISClass.ServiceCodes.SCDDLCombo();
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
                        dtSampleSC.Rows[row.Index].Delete();//Error found here 12/30/14
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
            DataTable dt = GISClass.Samples.ExExtDataLabels();
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
                dt = GISClass.Samples.ExSlashDataLabels(Convert.ToInt16(txtSponsorID.Text));
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
            //if (((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text == "154")
            //{
            //    int intEMOpen = GISClass.General.OpenForm(typeof(TestDataValuesEM));

            //    if (intEMOpen == 0)
            //    {
            //        TestDataValuesEM childForm = new TestDataValuesEM();
            //        childForm.Text = "TEST DATA VALUES -ENVIRONMENTAL MONITORING";
            //        childForm.nLogNo = Convert.ToInt64(txtLogNo.Text);
            //        if (((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "")
            //            childForm.nRptNo = Convert.ToInt32(((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text);
            //        else
            //            childForm.nRptNo = 0;
            //        childForm.nServiceCode = Convert.ToInt16(((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
            //        childForm.nSponsorID = Convert.ToInt16(txtSponsorID.Text);
            //        childForm.nSlashes = Convert.ToInt16(((TextBox)dtrLogTests.CurrentItem.Controls["txtSamples"]).Text);
            //        childForm.ShowDialog();
            //        //childForm.Show();
            //    }
            //}
            //else 
            //if (txtSponsor.Text.IndexOf("INGREDION") >= 0)
            //{
                if (Convert.ToInt16(((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text) > 1000)
                {
                    TestDataIngredion childForm = new TestDataIngredion();
                    childForm.Text = "TEST DATA VALUES - INGREDION";
                    childForm.nLogNo = Convert.ToInt32(txtLogNo.Text);
                    if (((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "")
                        childForm.nRptNo = Convert.ToInt32(((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text);
                    else
                        childForm.nRptNo = 0;
                    childForm.nSponsorID = Convert.ToInt16(txtSponsorID.Text);
                    childForm.nContactID = Convert.ToInt16(txtContactID.Text);
                    childForm.ShowDialog();
                }
                else
                {
                    int intOpen = GISClass.General.OpenForm(typeof(TestDataValues));

                    if (intOpen == 0)
                    {
                        TestDataValues childForm = new TestDataValues();
                        //childForm.MdiParent = this;
                        childForm.Text = "TEST DATA VALUES";
                        childForm.nLogNo = Convert.ToInt64(txtLogNo.Text);
                        if (((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "")
                            childForm.nRptNo = Convert.ToInt32(((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text);
                        else
                            childForm.nRptNo = 0;
                        childForm.nServiceCode = Convert.ToInt16(((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
                        childForm.nSponsorID = Convert.ToInt16(txtSponsorID.Text);
                        childForm.ShowDialog();
                    }
                }

            //}
            //else
            //{
            //    int intOpen = GISClass.General.OpenForm(typeof(TestDataValues));

            //    if (intOpen == 0)
            //    {
            //        TestDataValues childForm = new TestDataValues();
            //        //childForm.MdiParent = this;
            //        childForm.Text = "TEST DATA VALUES";
            //        childForm.nLogNo = Convert.ToInt64(txtLogNo.Text);
            //        if (((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text != "")
            //            childForm.nRptNo = Convert.ToInt32(((Label)dtrLogTests.CurrentItem.Controls["lblReportNo"]).Text);
            //        else
            //            childForm.nRptNo = 0;
            //        childForm.nServiceCode = Convert.ToInt16(((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
            //        childForm.nSponsorID = Convert.ToInt16(txtSponsorID.Text);
            //        childForm.ShowDialog();
            //    }
            //}
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
            OpenControls(pnlRecord, true); 
            btnSelQTests.Enabled = true; btnUnSelQTests.Enabled = true; btnSelAllTests.Enabled = true; btnTests.Enabled = true; cboQuotes.Enabled = true; 
            txtLogNo.ReadOnly = true;
            txtLogNo.Text = "(New)";
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
                dtLogTests.Rows[i]["TestSamples"] = "";
                dtLogTests.Rows[i]["Slashes"] = "";
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
            for (int i = 0; i < dtBilling.Rows.Count; i++)
            {
                if (dtBilling.Rows[i].RowState.ToString() != "Deleted")
                {
                    dtBilling.Rows[i]["BillQty"] = 0;
                    dtBilling.Rows[i]["SelectedTest"] = false;
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

        private void cboSC_Leave(object sender, EventArgs e)
        {
            //try
            //{
            //    int nI = dtrLogTests.CurrentItem.ItemIndex;
            //    if (((ComboBox)dtrLogTests.CurrentItem.Controls["cboSC"]).Text != "")
            //    {
            //        dtLogTests.Rows[nI]["ServiceCode"] = ((ComboBox)dtrLogTests.CurrentItem.Controls["cboSC"]).Text;
            //        dtLogTests.Rows[nI]["ServiceDesc"] = GISClass.ServiceCodes.SCDesc(Convert.ToInt16(((ComboBox)dtrLogTests.CurrentItem.Controls["cboSC"]).Text), dtSC);
            //    }
            //    else
            //    {
            //        dtLogTests.Rows[nI]["ServiceDesc"] = "";
            //    }
            //}
            //catch 
            //{}
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
                    case "txtSC"://cboSC
                        try
                        {
                            e.Value = dtLogTests.Rows[e.ItemIndex]["ServiceCode"].ToString();
                        }
                        catch { }
                        break;
                    case "lblSCDesc":
                        try
                        {
                            e.Value = GISClass.ServiceCodes.SCDesc(Convert.ToInt16(dtLogTests.Rows[e.ItemIndex]["ServiceCode"]), dtSC);
                        }
                        catch { }
                        break;
                    case "txtProtocolNo"://cboProtocolNo
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
                    case "txtPONo"://cboPONo
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
                            e.Value = dtLogTests.Rows[e.ItemIndex]["Slashes"].ToString();
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
                                e.Value = 1;
                            }
                            else
                            {
                                e.Value = 0;
                            }
                        }
                        catch { }
                        break;
                    case "rdoECWW":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "W")
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
                    case "rdoECMM":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "M")
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
                    case "rdoECYY":
                        try
                        {
                            if (dtLogTests.Rows[e.ItemIndex]["ECCompType"].ToString() == "Y")
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
                                e.Value = " ";
                                dtrLogTests.ItemTemplate.Controls["lblECEndDateX"].Visible = false;
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
                                e.Value = " ";
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
                dgvSponsors.Visible = true; dgvSponsors.BringToFront();
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
            ((Label)dtrLogTests.CurrentItem.Controls["lblSCDesc"]).Text = GISClass.ServiceCodes.SCDesc(Convert.ToInt16(((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text), dtSC);
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
                //((Label)dtrLogTests.CurrentItem.Controls["lblSCDesc"]).Text = GISClass.ServiceCodes.SCDesc(Convert.ToInt16(((TextBox)dtrLogTests.CurrentItem.Controls["txtSC"]).Text), dtSC);
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
                    dtSlashExtData.PrimaryKey = new DataColumn[] { dtSlashExtData.Columns["SlashNo"], dtSlashExtData.Columns["ExtDataLabel"] };

                    // Create an array for the key values to find. 
                    object[] fkeys = new object[2];

                   
                    for (int i = 0; i < dgvSlashExtData.Rows.Count - 1; i++)
                    {
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
            pnlSlashExtData.Visible = false; pnlRecord.Enabled = true;
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
            //if (nMode != 0)
            //{
            //    MessageBox.Show("You are still in file maintenance mode.");
            //    if (e.CloseReason == CloseReason.UserClosing)
            //    {

            //    }
            //    e.Cancel = true;
            //}
            e.Cancel = true;
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
            if (nMode == 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to send this Acknowledgement "  + Environment.NewLine + "Notification to the Sponsor?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.Yes)
                {
                    nReply = 1;
                }

                LabRpt rpt = new LabRpt();
                rpt.rptName = "Acknowledgement";
                rpt.WindowState = FormWindowState.Maximized;
                rpt.nLogNo = Convert.ToInt32(txtLogNo.Text);
                rpt.SpID = Convert.ToInt16(txtSponsorID.Text);
                try
                {
                    rpt.Show();
                }
                catch
                {
                    MessageBox.Show("Report cannot be loaded." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //rpt.WindowState = FormWindowState.Minimized;


                //string pdfFile = Application.StartupPath + @"\Reports\" + txtQuoteNo.Text + ".R" + nRevNo.ToString() + ".pdf";
                //string crafFile = Application.StartupPath + @"\Reports\" + "Credit Application Form.pdf";
                if (nReply == 0)
                    return;

                string pdfFile = @"\\gblnj4\GIS\Reports\A-" + txtLogNo.Text + ".pdf";

                string strText = "", strEMail = "";
                string strCFName = "";// GISClass.Quotations.ContactFirstName(txtQuoteNo.Text, nRevNo);
                string strSignature = ReadSignature();

                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }            
                SqlCommand sqlcmd = new SqlCommand();
                SqlDataReader sqldr;
                //sqlcmd = new SqlCommand("SELECT InitialEMailText FROM QuotationSetUp", sqlcnn);
                //SqlDataReader sqldr = sqlcmd.ExecuteReader();
                //if (sqldr.HasRows)
                //{
                //    sqldr.Read();
                //    strText = "Dear " + strCFName + "," + "<br /><br />" + sqldr.GetValue(0).ToString();
                //}
                //sqldr.Close(); sqlcmd.Dispose();
                //if (strText.Trim() == "")
                //{
                //    MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}

                //int nR = 0;
                //sqlcmd = new SqlCommand("SELECT AckRevisionNo FROM LogMaster WHERE GBLNo = " + Convert.ToInt64(txtLogNo.Text), sqlcnn);
                //sqldr = sqlcmd.ExecuteReader();
                //if (sqldr.HasRows)
                //{
                //    sqldr.Read();
                //    if (sqldr.GetValue(0) != null)
                //        nR = Convert.ToInt16(sqldr.GetValue(0).ToString()) + 1;
                //}
                //sqldr.Close(); sqlcmd.Dispose();

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
                //if (strText.Trim() == "")
                //{
                //    MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please contact Technical Services Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}
                //sqlcnn.Close(); sqlcnn.Dispose();
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.

                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                // Set HTMLBody. 
                //add the body of the email
                strText = "Dear " + strCFName + ", <br/><br/>" +  "Thank you for your submission of " + txtArticle.Text + " for " +
                    ((Label)dtrLogTests.CurrentItem.Controls["lblSCDesc"]).Text + " <br>testing. " + 
                    "We acknowledge receipt of the samples as described in the attached <b>Acknowledgment Notification</b>. <br/> " +
                    "In this attachment you will find important information that provides the estimated start and completion dates <br/> " + 
                    "of your testing as well as details that will appear in the Final Report. It is critical for you to review it and <br/> " +
                    "immediately inform us in writing of any questions or corrections that you have. Non-acknowledgement of this <br/>" +
                    "document is considered by GBL as the Sponsor's acceptance and agreement of the accuracy and correctness of our <br/> " + 
                    "records and releases Gibraltar from any further liabilities. Failure by Sponsor to return GBL's Terms, Conditions <br/> " + 
                    "and Pricing Policy is taken by Gibraltar as acceptance of same by Sponsor. " +
                    "Thank you for your support of <br/>Gibraltar Laboratories.<br><br>";

                oMsg.HTMLBody = "<FONT face=\"Arial\">";
                oMsg.HTMLBody += strText.Trim() + strSignature;
                //Add an attachment.
                oMsg.Attachments.Add(pdfFile);
                //oMsg.Attachments.Add(crafFile);
                //Subject line
                oMsg.Subject = "GBL: " + txtLogNo.Text + " Lot: " + dgvSamples.Rows[0].Cells[3].Value.ToString() + " Article: " + txtArticle.Text.Trim();
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                //Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(strEMail); // "adelacruz@gibraltarlabsinc.com"

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
                //oMsg.Display();

                // Send.
                //oMsg.Send();
                ((Outlook._MailItem)oMsg).Send();
                // Clean up.
                //oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;

                //sqlcnn = GISClass.DBConnection.GISConnection();
                //if (sqlcnn == null)
                //{
                //    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}
                //sqlcmd = new SqlCommand("UPDATE QuotationRev SET DateEMailed=GetDate(),EMailedByID=" + LogIn.nUserID + " " +
                //                        "WHERE QuotationNo='" + txtQuoteNo.Text + "' AND RevisionNo=" + nRevNo, sqlcnn);
                //sqlcmd.ExecuteNonQuery();
                //string strCRADte = GISClass.Sponsors.CRADate(Convert.ToInt16(txtSponsorID.Text));
                //if (strCRADte == "")
                //{
                //    sqlcmd.Dispose();
                //    sqlcmd = new SqlCommand("UPDATE Sponsors SET DateMailedCRA=GetDate(),CRAMailedByID=" + LogIn.nUserID + " " +
                //                            "WHERE SponsorID =" + Convert.ToInt16(txtSponsorID.Text), sqlcnn);
                //    sqlcmd.ExecuteNonQuery();
                //}
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
            e.SuppressKeyPress = true; //disable typing date and force user select date from Calendar
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
                //string strCol = dgvSamples.CurrentCell.OwningColumn.Name;
                //object clipData = Clipboard.GetData(ClipboardFormat);


                //MemoryStream ms = clipData as MemoryStream;
                //if (ms != null)
                //{
                //    XmlDocument xml = new XmlDocument();
                //    xml.Load(ms);
                //    XmlNodeList table = xml.GetElementsByTagName("Table");
                //    if (table.Count > 0)
                //    {
                //        foreach (XmlNode row in table[0].ChildNodes)
                //        {
                //            DataRow dR = dtSamples.NewRow();
                //            //XmlNode xmlSlash = row.ChildNodes[0];
                //            //MessageBox.Show(xmlSlash.ChildNodes[0].InnerText);
                //            //XmlNode xmlSampDesc = row.ChildNodes.Item(1);
                //            //XmlNode xmlOtherDesc1 = row.ChildNodes.Item(2);
                //            //XmlNode xmlOtherDesc2 = row.ChildNodes.Item(3);
                //            //dR["SlashNo"] = xmlSlash.InnerText;
                //            //dR["SampleDesc"] = xmlSampDesc.InnerText;
                //            //dR["OtherDesc1"] = xmlOtherDesc1.InnerText;
                //            //dR["OtherDesc2"] = xmlOtherDesc2.InnerText;
                //            int n = 0;
                //            //foreach (XmlNode cell in row.ChildNodes)
                //            //{
                //            //    dR[n] = cell.InnerText;
                //            //    n++;
                //            //}
                //            XmlNode cell = row.ChildNodes.Item(0);
                //            if (dgvSamples.CurrentCell.ColumnIndex == 1)
                //            {   
                //                dR["SlashNo"] = cell.InnerText;
                //                dR["SampleDesc"] = "";
                //                dR["OtherDesc1"] = "";
                //                dR["OtherDesc2"] = "";
                //            }
                //            else if (dgvSamples.CurrentCell.ColumnIndex == 2)
                //            {
                //                dR["SlashNo"] = "";
                //                dR["SampleDesc"] = cell.InnerText;
                //                dR["OtherDesc1"] = "";
                //                dR["OtherDesc2"] = "";
                //            }
                //            else if (dgvSamples.CurrentCell.ColumnIndex == 3)
                //            {
                //                dR["SlashNo"] = "";
                //                dR["SampleDesc"] = "";
                //                dR["OtherDesc1"] = cell.InnerText;
                //                dR["OtherDesc2"] = "";
                //            }
                //            else if (dgvSamples.CurrentCell.ColumnIndex == 4)
                //            {
                //                dR["SlashNo"] = "";
                //                dR["SampleDesc"] = "";
                //                dR["OtherDesc1"] = "";
                //                dR["OtherDesc2"] = cell.InnerText;
                //            }
                //            dtSamples.Rows.Add(dR);
                //        }
                //    }
                //}
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
                        //if (dgvSamples.CurrentCell.ColumnIndex == 1)
                        //{
                        //    dtSamples.Rows[row]["SlashNo"] = f;
                        //    dtSamples.Rows[row]["SampleDesc"] = "";
                        //    dtSamples.Rows[row]["OtherDesc1"] = "";
                        //    dtSamples.Rows[row]["OtherDesc2"] = "";
                        //}
                        //else if (dgvSamples.CurrentCell.ColumnIndex == 2)
                        //{
                        //    dtSamples.Rows[row]["SlashNo"] = "";
                        //    dtSamples.Rows[row]["SampleDesc"] = f;
                        //    dtSamples.Rows[row]["OtherDesc1"] = "";
                        //    dtSamples.Rows[row]["OtherDesc2"] = "";
                        //}
                        //else if (dgvSamples.CurrentCell.ColumnIndex == 3)
                        //{
                        //    dtSamples.Rows[row]["SlashNo"] = "";
                        //    dtSamples.Rows[row]["SampleDesc"] = "";
                        //    dtSamples.Rows[row]["OtherDesc1"] = f;
                        //    dtSamples.Rows[row]["OtherDesc2"] = "";
                        //}
                        //else if (dgvSamples.CurrentCell.ColumnIndex == 4)
                        //{
                        //    dtSamples.Rows[row]["SlashNo"] = "";
                        //    dtSamples.Rows[row]["SampleDesc"] = "";
                        //    dtSamples.Rows[row]["OtherDesc1"] = "";
                        //    dtSamples.Rows[row]["OtherDesc2"] = f;
                        //}
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
                        
            DataTable dt = new DataTable();
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
            if (nMode != 0)
            {
                //if (txtStudyNo.Text == "" || cboStudyDir.SelectedIndex <= 0)
                //{
                //    {
                //        DialogResult dReply = new DialogResult();
                //        dReply = MessageBox.Show("No Study Number/Director entry provided.", Application.ProductName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                //        if (dReply == DialogResult.Retry)
                //            return;
                //        else
                //        {
                //            pnlSCExtData.Visible = false; pnlRecord.Enabled = true;
                //            return;
                //        }
                //    }
                //}
                pnlSCExtData.Visible = false; pnlRecord.Enabled = true;
                List<string> strLabel = new List<string>();
                List<string> strValue = new List<string>();
                for (int i = 0; i < dgvSCExtData.Rows.Count - 1; i++)
                {
                    if (dgvSCExtData.Rows[i].Cells["Label"].Value != null && dgvSCExtData.Rows[i].Cells["Label"].Value.ToString() != "")
                        strLabel.Add(dgvSCExtData.Rows[i].Cells["Label"].Value.ToString());
                    if (dgvSCExtData.Rows[i].Cells["Value"].Value != null && dgvSCExtData.Rows[i].Cells["Value"].Value.ToString() != "")
                        strValue.Add(dgvSCExtData.Rows[i].Cells["Value"].Value.ToString());

                }
                int nM = 0;
                for (int i = 0; i < dtSCExtData.Rows.Count; i++)
                {
                    if (((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text == dtSCExtData.Rows[i]["ServiceCode"].ToString())
                    {
                        nM = 1;
                        break;
                    }
                }
                if (nM == 0)
                {
                    for (int i = 0; i < dgvSCExtData.Rows.Count; i++)
                    {
                        if (dgvSCExtData.Rows[i].Cells["Value"].Value != null && dgvSCExtData.Rows[i].Cells["Value"].Value.ToString().Trim() != "")
                        {
                            DataRow dR;
                            dR = dtSCExtData.NewRow();
                            dR["ServiceCode"] = Convert.ToInt16(((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
                            if (txtStudyNo.Text.Trim() != "")
                                dR["StudyNo"] = Convert.ToInt32(txtStudyNo.Text.Trim());
                            else
                                dR["StudyNo"] = DBNull.Value;
                            if (cboStudyDir.SelectedIndex != -1)
                                dR["StudyDirID"] = cboStudyDir.SelectedValue;
                            else
                                dR["StudyDirID"] = DBNull.Value;
                            dR["SCExtDataLabel"] = dgvSCExtData.Rows[i].Cells["Label"].Value;
                            dR["SCExtDataValue"] = dgvSCExtData.Rows[i].Cells["Value"].Value;
                            dR["PrtNotes"] = txtPrtNotes.Text.Trim();
                            dR["NonPrtNotes"] = txtNonPrtNotes.Text.Trim();
                            dtSCExtData.Rows.Add(dR);
                        }
                    }
                }
                else
                {
                    dtSCExtData.PrimaryKey = new DataColumn[] { dtSCExtData.Columns["ServiceCode"], dtSCExtData.Columns["SCExtDataLabel"] };

                    // Create an array for the key values to find. 
                    object[] fkeys = new object[2];


                    for (int i = 0; i < dgvSCExtData.Rows.Count - 1; i++)
                    {
                        fkeys[0] = txtSCExt.Text;
                        fkeys[1] = dgvSCExtData.Rows[i].Cells["Label"].Value;

                        DataRow foundRow = dtSCExtData.Rows.Find(fkeys);
                        if (foundRow == null)
                        {
                            DataRow dR;
                            dR = dtSCExtData.NewRow();
                            dR["ServiceCode"] = Convert.ToInt16(((GISControls.TextBoxChar)dtrLogTests.CurrentItem.Controls["txtSC"]).Text);
                            if (txtStudyNo.Text.Trim() != "")
                                dR["StudyNo"] = Convert.ToInt32(txtStudyNo.Text.Trim());
                            else
                                dR["StudyNo"] = DBNull.Value;
                            if (cboStudyDir.SelectedIndex != -1)
                                dR["StudyDirID"] = cboStudyDir.SelectedValue;
                            else
                                dR["StudyDirID"] = DBNull.Value;
                            dR["SCExtDataLabel"] = dgvSCExtData.Rows[i].Cells["Label"].Value;
                            dR["SCExtDataValue"] = dgvSCExtData.Rows[i].Cells["Value"].Value;
                            dR["PrtNotes"] = txtPrtNotes.Text.Trim();
                            dR["NonPrtNotes"] = txtNonPrtNotes.Text.Trim();
                            dtSCExtData.Rows.Add(dR);
                        }
                        else
                        {
                            int n = dtSCExtData.Rows.IndexOf(foundRow);
                            if (txtStudyNo.Text.Trim() != "")
                                dtSCExtData.Rows[n]["StudyNo"] = Convert.ToInt32(txtStudyNo.Text.Trim());
                            else
                                dtSCExtData.Rows[n]["StudyNo"] = DBNull.Value;
                            if (cboStudyDir.SelectedIndex != -1)
                                dtSCExtData.Rows[n]["StudyDirID"] = cboStudyDir.SelectedValue;
                            else
                                dtSCExtData.Rows[n]["StudyDirID"] = DBNull.Value;
                            dtSCExtData.Rows[n]["SCExtDataValue"] = dgvSCExtData.Rows[i].Cells["Value"].Value;
                        }
                    }
                }
            }
            pnlSCExtData.Visible = false; pnlRecord.Enabled = true;
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

        private void dgvSlashExtData_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            //try
            //{
            //    DataRow dR = dtSlashExtData.NewRow();
            //    dR["SlashNo"] = txtSlashExt.Text;
            //    dR["ExtDataLabel"] = dgvSlashExtData.CurrentRow.Cells["Label"].Value;
            //    dR["ExtDataValue"] = dgvSlashExtData.CurrentRow.Cells["Value"].Value;
            //    dtSlashExtData.Rows.Add(dR);
            //}
            //catch { }
        }

        private void dgvSlashExtData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    dtSlashExtData.Rows[bsSlashExtData.Position]["ExtDataLabel"] = dgvSlashExtData.CurrentRow.Cells["Label"].Value;
            //    dtSlashExtData.Rows[bsSlashExtData.Position]["ExtDataValue"] = dgvSlashExtData.CurrentRow.Cells["Value"].Value;
            //    dtSlashExtData.Rows[bsSlashExtData.Position]["SlashNo"] = txtSlashExt.Text;
            //}
            //catch { }
        }

        private void txtSlashExt_TextChanged(object sender, EventArgs e)
        {
            //DataRow[] foundRows;
            //foundRows = dtSlashExtData.Select("SlashNo = '" + txtSlashExt.Text + "'");
            //if (foundRows.Length != 0)
            //{
            //    DataTable dtX = new DataTable();
            //    dtX = foundRows.CopyToDataTable();
            //    dgvSlashExtData.DataSource = dtX;
            //    //dgvSlashExtData.RowCount = dtX.Rows.Count + 1;
            //    //try
            //    //{
            //    //    for (int r = 0; r < dtX.Rows.Count; r++)
            //    //    {
            //    //        dgvSlashExtData.Rows[r].Cells["Label"].Value = dtX.Rows[r]["ExtDataLabel"].ToString();
            //    //        dgvSlashExtData.Rows[r].Cells["Value"].Value = dtX.Rows[r]["ExtDataValue"].ToString();
            //    //        dgvSlashExtData.Rows[r].Cells["SlashNo"].Value = dtX.Rows[r]["SlashNo"].ToString();
            //    //    }
            //    //}
            //    //catch { }
            //}
        }

        private void GetSlashExtData()
        {
            DataTable dt = GISClass.Samples.ExExtDataLabels();
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
                    //((DataGridViewComboBoxColumn)dgvSampleSC.Columns[e.ColumnIndex]).Items.Add(value);
                    e.ThrowException = false;
                }
            }
        }

        private void btnLSPrinter_Click(object sender, EventArgs e)
        {
            //string strPrinter = GISClass.Users.UserPrinterName(LogIn.nUserID);
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            SqlDataReader sqldr;

            ReportDocument crDoc = new ReportDocument();

            byte nM = 0; byte nB = 0;
            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                if (Convert.ToInt16(dtLogTests.Rows[i]["ServiceCode"].ToString()) > 1000 && Convert.ToInt16(dtLogTests.Rows[i]["ServiceCode"].ToString()) < 2000)
                {
                    nM = 1;
                    break;
                }
            }
            string rpt = "";
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
            sqldr = sqlcmd.ExecuteReader();
            DataTable dTable = new DataTable();
            try
            {
                dTable.Load(sqldr);
            }
            catch
            {
            }
            crDoc.SetDataSource(dTable);
            System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();

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
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            //crDoc.PrintOptions.PrinterName = strPrinter;// @"\\it5\46 IT Brother Printer";
            //crDoc.PrintToPrinter(2, false, 0, 0);
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

        private void btnILCancel_Click(object sender, EventArgs e)
        {
            pnlIL.Visible = false; pnlRecord.Enabled = true;
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

        //private void btnILOK_Click(object sender, EventArgs e)
        //{
        //    pnlIL.Visible = false; pnlRecord.Enabled = true;
        //    txtArticle.Text = txtFillCode.Text;
        //    //btnViewTests.Enabled = false;                
        //    DataTable dtM = GISClass.Samples.NSManifest(txtFillCode.Text);
        //    if (dtM != null && dtM.Rows.Count > 0)
        //    {
        //        DataTable dt = GISClass.Quotations.LoadLoginTests("2012.0464");//cboQuotes.Text.ToString()
        //        if (dt == null)
        //        {
        //            MessageBox.Show("Connection problems. Please contact your system administrator.");
        //            return;
        //        }
        //        DataRow dr;
        //        dtBilling.Rows.Clear();
        //        for (int i = 0; i < dtM.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < dt.Rows.Count; j++)
        //            {
        //                if (dtM.Rows[i]["ServiceCode"].ToString() == dt.Rows[j]["ServiceCode"].ToString())
        //                {
        //                    DataRow dR;
        //                    dR = dtBilling.NewRow();
        //                    dR["QuoteNo"] = dt.Rows[j]["QuoteNo"];
        //                    dR["ServiceCode"] = dt.Rows[j]["ServiceCode"];
        //                    dR["ServiceDesc"] = dt.Rows[j]["ServiceDesc"];
        //                    dR["TestDesc1"] = dt.Rows[j]["TestDesc1"];
        //                    dR["UnitDesc"] = dt.Rows[j]["UnitDesc"];
        //                    dR["BillQty"] = 1;
        //                    dR["SelectedTest"] = true;
        //                    dR["Rush"] = false;
        //                    dR["UnitPrice"] = dt.Rows[j]["UnitPrice"];
        //                    dR["RushPrice"] = dt.Rows[j]["RushPrice"];
        //                    dR["ControlNo"] = dt.Rows[j]["ControlNo"];
        //                    dtBilling.Rows.Add(dR);
        //                }
        //            }
        //            //
        //            int nDuration = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dtM.Rows[i]["ServiceCode"]));
        //            dr = dtLogTests.NewRow();
        //            dr["ServiceCode"] = Convert.ToInt16(dtM.Rows[i]["ServiceCode"]);
        //            dr["ServiceDesc"] = GISClass.ServiceCodes.SCDesc(Convert.ToInt16(dtM.Rows[i]["ServiceCode"]), dtSC);
        //            dr["ProtocolNo"] = "";
        //            dr["StartDate"] = DateTime.Now;
        //            dr["EndDate"] = DateTime.Now.AddDays(nDuration);
        //            dr["QuotationNo"] = "2012.0464";
        //            dr["BillQty"] = 1;
        //            dr["TestSamples"] = 1;
        //            dr["PONo"] = txtILPO.Text;
        //            dr["BookNo"] = txtILBookNo.Text;
        //            dr["EC"] = false;
        //            dr["ECCompType"] = 1;
        //            dr["ECLength"] = 0;
        //            dr["ECEndDate"] = DateTime.Now;
        //            dr["DateSampled"] = DateTime.Now;
        //            dr["QuoteFlag"] = "1";
        //            dr["ReportNo"] = 0;
        //            dr["AddlNotes"] = GISClass.ServiceCodes.SCDesc(Convert.ToInt16(dtM.Rows[i]["ServiceCode"]), dtSC);
        //            dtLogTests.Rows.Add(dr);
        //        }
        //        dtM.Dispose(); dt.Dispose();

        //        bsBilling.DataSource = dtBilling;
        //        dgvTests.DataSource = bsBilling;
        //        dgvTests.Columns["UnitPrice"].Visible = false;
        //        dgvTests.Columns["RushPrice"].Visible = false;
        //        dgvTests.Columns["ControlNo"].Visible = false;

        //        //Samples
        //        DataRow dRS = dtSamples.NewRow();
        //        dRS["SlashNo"] = txtILSlash.Text;
        //        dRS["OtherDesc1"] = txtILLotNo.Text;
        //        dRS["SlashID"] = 1;
        //        dtSamples.Rows.Add(dRS);

        //        bsSamples.EndEdit();
        //        cboSlashSC.SelectedIndex = 0;
        //        btnSlashSC_Click(null, null);
        //    }
        //}

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
                dt = GISClass.Quotations.LoadLoginTests(cboQuotes.Text.ToString());
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
                    //SendKeys.Send("{UP}");
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
            tsddbSearch.Enabled = true;
        }

        private void txtFillCode_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
                dgvFillCodes.Visible = true;
            else
                dgvFillCodes.Visible = false;
        }

        private void dgvFillCodes_DoubleClick(object sender, EventArgs e)
        {
            txtFillCode.Text = dgvFillCodes.CurrentRow.Cells[0].Value.ToString();
            dgvFillCodes.Visible = false;
            txtReminder.Text = "";
            if (nMode == 1)
            {
                if (GISClass.Ingredion.CheckLeprino(txtFillCode.Text) == true)
                    txtReminder.Text = "REMINDER: Another GBL must be created for Leprino Foods Company.";

                if (GISClass.Ingredion.CheckUSP(txtFillCode.Text) == true)
                {
                    if (txtReminder.Text != "")
                        txtReminder.Text = txtReminder.Text + Environment.NewLine + "REMINDER: Please limit composite entry to a range of 5 (i.e. 001-005).";
                    else
                        txtReminder.Text = "REMINDER: Please limit composite entry to a range of 5 (i.e. 001-005).";
                }
                if (GISClass.Ingredion.CheckIndividualTest(txtFillCode.Text) == true)
                {
                    if (txtReminder.Text != "")
                        txtReminder.Text = txtReminder.Text + Environment.NewLine + "REMINDER: B. cereus needs to be tested individually.";
                    else
                        txtReminder.Text = "REMINDER: B. cereus needs to be tested individually.";
                }

                if (txtReminder.Text != "")
                    txtReminder.Visible = true; 
                else
                    txtReminder.Visible = false; 
            }
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
                txtReminder.Text = "";
                if (nMode == 1)
                {
                    if (GISClass.Ingredion.CheckLeprino(txtFillCode.Text) == true)
                        txtReminder.Text = "REMINDER: Another GBL must be created for Leprino Foods Company.";

                    if (GISClass.Ingredion.CheckUSP(txtFillCode.Text) == true)
                    {
                        if (txtReminder.Text != "")
                            txtReminder.Text = txtReminder.Text + Environment.NewLine + "REMINDER: Please limit composite entry to a range of 5 (i.e. 001-005).";
                        else
                            txtReminder.Text = "REMINDER: Please limit composite entry to a range of 5 (i.e. 001-005).";
                    }
                    if (GISClass.Ingredion.CheckIndividualTest(txtFillCode.Text) == true)
                    {
                        if (txtReminder.Text != "")
                            txtReminder.Text = txtReminder.Text + Environment.NewLine + "REMINDER: B. cereus needs to be tested individually.";
                        else
                            txtReminder.Text = "REMINDER: B. cereus needs to be tested individually.";
                    }

                    if (txtReminder.Text != "")
                        txtReminder.Visible = true;
                    else
                        txtReminder.Visible = false; 
                }
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

        private void btnILCancel_Click_1(object sender, EventArgs e)
        {
            pnlIL.Visible = false; pnlRecord.Enabled = true;
        }

        private void btnILOK_Click(object sender, EventArgs e)
        {
            if (chkReTest.Checked == true && txtILSamples.Text.Trim() == "")
            {
                MessageBox.Show("Please enter no. of samples.");
                return;
            }

            //if (chkReTest.Checked == false && txtILSamples.Text.Trim() == "" && rdoSpecial.Checked == true)
            //{
            //    MessageBox.Show("Please enter no. of samples.");
            //    return;
            //}

            DataTable dt = GISClass.Quotations.LoadLoginTests("2017.1223"); // 2015.0992 "2012.0464"cboQuotes.Text.ToString()
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }

            //Extract Test Samples
            string strSamples = "0"; int nSamples = 0;
            if (txtILSlash.Text.IndexOf("-") >= 0)
            {
                int nIdx = txtILSlash.Text.IndexOf("-");
                strSamples = txtILSlash.Text.Substring(nIdx + 1, txtILSlash.Text.Length - (nIdx + 1));
            }
            else
            {
                strSamples = txtILSlash.Text;
            }
            try
            {
                nSamples = Convert.ToInt16(strSamples);
            }
            catch { }
            
            pnlIL.Visible = false; pnlRecord.Enabled = true;
            txtArticle.Text = txtFillCode.Text;

            if (chkReTest.Checked == false)
            {
                DataTable dtM = new DataTable();
                dtM = GISClass.Samples.NSManifest(txtFillCode.Text);
                if (dtM != null && dtM.Rows.Count > 0)
                {
                    dtSamples.Rows.Clear(); dtBilling.Rows.Clear(); dtLogTests.Rows.Clear(); dtSampleSC.Rows.Clear();

                    DataRow dr;
                    for (int i = 0; i < dtM.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dtM.Rows[i]["ServiceCode"].ToString() == dt.Rows[j]["ServiceCode"].ToString() &&
                                (dt.Rows[j]["TestDesc1"].ToString().IndexOf("Extra dilution") == -1))
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
                        int nDuration = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dtM.Rows[i]["ServiceCode"]));
                        dr = dtLogTests.NewRow();
                        dr["ServiceCode"] = Convert.ToInt16(dtM.Rows[i]["ServiceCode"]);
                        dr["ServiceDesc"] = "";
                        dr["ProtocolNo"] = "";
                        dr["StartDate"] = DateTime.Now;
                        dr["EndDate"] = DateTime.Now.AddDays(nDuration);
                        dr["QuotationNo"] = "2017.1223.R0";//2015.0992.R2 "2012.0464"; //Changed from R1 8/3/2016
                        dr["BillQty"] = 1;
                        dr["TestSamples"] = nSamples;
                        if (nSamples > 1)
                            dr["Slashes"] = "1-" + nSamples.ToString();
                        else
                            dr["Slashes"] = "1";
                        dr["PONo"] = txtILPO.Text;
                        if (txtILBookNo.Text.Trim() == "")
                            dr["BookNo"] = "0";
                        else
                            dr["BookNo"] = txtILBookNo.Text;
                        dr["EC"] = false;
                        dr["ECCompType"] = DBNull.Value;
                        dr["ECLength"] = DBNull.Value;
                        dr["ECEndDate"] = DBNull.Value;
                        dr["DateSampled"] = DBNull.Value;
                        dr["QuoteFlag"] = "1";
                        dr["ReportNo"] = 0;
                        dr["AddlNotes"] = GISClass.ServiceCodes.SCDesc(Convert.ToInt16(dtM.Rows[i]["ServiceCode"]), dtSC);
                        dtLogTests.Rows.Add(dr);
                    }
                }
                dtM.Dispose();
                //Samples
                DataRow dRS = dtSamples.NewRow();
                dRS["SlashNo"] = txtILSlash.Text;
                dRS["OtherDesc1"] = txtILLotNo.Text;
                dRS["SlashID"] = 1;
                dtSamples.Rows.Add(dRS);
                bsSamples.EndEdit();
                dgvSamples.DataSource = bsSamples;
            }
            else
            {
                txtGenDesc.Text = "Original GBL #";
                dtSamples.Rows.Clear(); dtBilling.Rows.Clear(); dtLogTests.Rows.Clear(); dtSampleSC.Rows.Clear();
                DataRow dr;
                for (int i = 0; i < dgvManifestSC.Rows.Count; i++)
                {
                    if (dgvManifestSC.Rows[i].Cells[1].Value.ToString() == "True")
                    {
                        int nDuration = GISClass.ServiceCodes.SCDuration(Convert.ToInt16(dgvManifestSC.Rows[i].Cells["SC"].Value));
                        dr = dtLogTests.NewRow();
                        dr["ServiceCode"] = Convert.ToInt16(dgvManifestSC.Rows[i].Cells["SC"].Value);
                        dr["ServiceDesc"] = "";// GISClass.ServiceCodes.SCDesc(Convert.ToInt16(dgvManifestSC.Rows[i].Cells["SC"].Value), dtSC);
                        dr["ProtocolNo"] = "";
                        dr["StartDate"] = DateTime.Now;
                        dr["EndDate"] = DateTime.Now.AddDays(nDuration);
                        dr["QuotationNo"] = "2017.1223.R0";//2015.0992.R2 2012.0464";//Changed from R1 8/3/2016
                        dr["BillQty"] = Convert.ToDecimal(txtILSamples.Text);
                        dr["TestSamples"] = Convert.ToDecimal(txtILSamples.Text);
                        dr["Slashes"] = "1-" + txtILSamples.Text;
                        dr["PONo"] = txtILPO.Text;
                        dr["BookNo"] = txtILBookNo.Text;
                        dr["EC"] = false;
                        dr["ECCompType"] = DBNull.Value;
                        dr["ECLength"] = DBNull.Value;
                        dr["ECEndDate"] = DBNull.Value;
                        dr["DateSampled"] = DBNull.Value;
                        dr["QuoteFlag"] = "1";
                        dr["ReportNo"] = 0;
                        dr["AddlNotes"] = "";// GISClass.ServiceCodes.SCDesc(Convert.ToInt16(dgvManifestSC.Rows[i].Cells["SC"].Value), dtSC);
                        dtLogTests.Rows.Add(dr);
                        //Billing References
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            if (dgvManifestSC.Rows[i].Cells[0].Value.ToString() == dt.Rows[j]["ServiceCode"].ToString() &&
                                (dt.Rows[j]["TestDesc1"].ToString().IndexOf("Extra dilution") == -1 || dt.Rows[j]["TestDesc1"].ToString().IndexOf("Extra Dilution") == -1))
                            {
                                DataRow dR;
                                dR = dtBilling.NewRow();
                                dR["QuoteNo"] = dt.Rows[j]["QuoteNo"];
                                dR["ServiceCode"] = dt.Rows[j]["ServiceCode"];
                                dR["ServiceDesc"] = dt.Rows[j]["ServiceDesc"];
                                dR["TestDesc1"] = dt.Rows[j]["TestDesc1"];
                                dR["UnitDesc"] = dt.Rows[j]["UnitDesc"];
                                dR["BillQty"] = Convert.ToDecimal(txtILSamples.Text);
                                dR["SelectedTest"] = true;
                                dR["Rush"] = false;
                                dR["UnitPrice"] = dt.Rows[j]["UnitPrice"];
                                dR["RushPrice"] = dt.Rows[j]["RushPrice"];
                                dR["ControlNo"] = dt.Rows[j]["ControlNo"];
                                dtBilling.Rows.Add(dR);
                            }
                        }
                        txtGenDesc.Text = txtGenDesc.Text + txtILGBLNo.Text + "/1." + dgvManifestSC.Rows[i].Cells["SC"].Value.ToString() + ", ";
                    }
                }
                dt.Dispose();
                //Samples
                DataRow dRS;
                for (int i = 0; i < Convert.ToInt16(txtILSamples.Text); i++)
                {
                    dRS = dtSamples.NewRow();
                    dRS["SlashNo"] = (i + 1).ToString("000");
                    dRS["SlashID"] = (i + 1);
                    dRS["OtherDesc1"] = txtILLotNo.Text;
                    dtSamples.Rows.Add(dRS);
                }
                bsSamples.EndEdit();
                dgvSamples.DataSource = bsSamples;
            }
            bsBilling.DataSource = dtBilling;
            dgvTests.DataSource = bsBilling;
            dgvTests.Columns["UnitPrice"].Visible = false;
            dgvTests.Columns["RushPrice"].Visible = false;
            dgvTests.Columns["ControlNo"].Visible = false;
            cboSlashSC.SelectedIndex = 0;
            btnSlashSC_Click(null, null);
            if (nMode == 1)
            {
                txtReminder.Text = "";
                
            }
        }

        private void chkReTest_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReTest.Checked == true)
            {
                DataTable dtM = GISClass.Samples.NSManifest(txtFillCode.Text);
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
                dgvManifestSC.RowCount = 0;
        }

        private void IngredionManifestLog_Load(object sender, EventArgs e)
        {
            strFileAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "IngredionManifest");

            LoadRecords();
            LoadSponsorsDDL();
            LoadStudyDir();
            LoadSC();

            BuildPrintItems();
            //MAIN
            dtLogFM.Columns.Add("ContactID", typeof(Int32));
            dtLogFM.Columns.Add("CtrldSubstance", typeof(bool));
            dtLogFM.Columns.Add("Rush", typeof(bool));
            dtLogFM.Columns.Add("ArticleDesc", typeof(string));
            dtLogFM.Columns.Add("AnalystDone", typeof(Boolean));
            dtLogFM.Columns.Add("ManagerChecked", typeof(Boolean));
            dtLogFM.Columns.Add("ImageFileName", typeof(string));
            dtLogFM.Columns.Add("SampleDesc", typeof(string));
            dtLogFM.Columns.Add("AddlNotes", typeof(string));
            dtLogFM.Columns.Add("StorageCode", typeof(string));
            dtLogFM.Columns.Add("ReceiptCode", typeof(string));
            dtLogFM.Columns.Add("StorageDesc", typeof(string));
            dtLogFM.Columns.Add("DateCreated", typeof(DateTime));
            dtLogFM.Columns.Add("DateReceived", typeof(DateTime));
            dtLogFM.Columns.Add("DateCancelled", typeof(DateTime));
            dtLogFM.Columns.Add("SAPDate", typeof(DateTime));
            bsLogFM.DataSource = dtLogFM;

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
            dgvSamples.Columns["SampleDesc"].HeaderText = "Additional Description";// Sample/
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
            dtLogTests.Columns["ServiceDesc"].MaxLength = 1000;
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
            bsBilling.DataSource = dtBilling;
            dgvTests.DataSource = bsBilling;

            StandardDGVSetting(dgvTests);
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

            //SC Extended Data
            dtSCExtData.Columns.Add("ServiceCode", typeof(Int16));
            dtSCExtData.Columns.Add("StudyNo", typeof(Int32));
            dtSCExtData.Columns.Add("StudyDirID", typeof(Int16));
            dtSCExtData.Columns.Add("SCExtDataLabel", typeof(string));
            dtSCExtData.Columns.Add("SCExtDataValue", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData1", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData2", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData3", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData4", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData5", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData6", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData7", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData8", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData9", typeof(string));
            //dtSCExtData.Columns.Add("SCExtData10", typeof(string));
            dtSCExtData.Columns.Add("PrtNotes", typeof(string));
            dtSCExtData.Columns.Add("NonPrtNotes", typeof(string));
            bsSCExtData.DataSource = dtSCExtData;

            //txtSCExt.DataBindings.Add("Text", bsSCExtData, "ServiceCode", true); 
            txtStudyNo.DataBindings.Add("Text", bsSCExtData, "StudyNo", true);
            txtStudyDirID.DataBindings.Add("Text", bsSCExtData, "StudyDirID", true);
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

            //Slash Extended Data - Parent Table
            //dtSlashExt.Columns.Add("SlashNo", typeof(string));
            //bsSlashExt.DataSource = dtSlashExt;
            //bnSlashExtData.BindingSource = bsSlashExt;

            //txtSlashExt.DataBindings.Add("Text", bsSlashExt, "SlashNo", true);
            //txtSlash1.DataBindings.Add("Text", bsSlashExtData, "SlashExtData1", true);
            //txtSlash2.DataBindings.Add("Text", bsSlashExtData, "SlashExtData2", true);
            //txtSlash3.DataBindings.Add("Text", bsSlashExtData, "SlashExtData3", true);
            //txtSlash4.DataBindings.Add("Text", bsSlashExtData, "SlashExtData4", true);
            //txtSlash5.DataBindings.Add("Text", bsSlashExtData, "SlashExtData5", true);
            //txtSlash6.DataBindings.Add("Text", bsSlashExtData, "SlashExtData6", true);
            //txtSlash7.DataBindings.Add("Text", bsSlashExtData, "SlashExtData7", true);
            //txtSlash8.DataBindings.Add("Text", bsSlashExtData, "SlashExtData8", true);
            //txtSlash9.DataBindings.Add("Text", bsSlashExtData, "SlashExtData9", true);
            //txtSlash10.DataBindings.Add("Text", bsSlashExtData, "SlashExtData10", true);

            //Slash Extended Data 
            dtSlashExtData.Columns.Add("SlashNo", typeof(string));
            dtSlashExtData.Columns.Add("ExtDataLabel", typeof(string));
            dtSlashExtData.Columns.Add("ExtDataValue", typeof(string));
            bsSlashExtData.DataSource = dtSlashExtData;

            //dtSlashExtData.Columns.Add("SlashExtData1", typeof(string));
            //dtSlashExtData.Columns.Add("SlashExtData2", typeof(string));
            //dtSlashExtData.Columns.Add("SlashExtData3", typeof(string));
            //dtSlashExtData.Columns.Add("SlashExtData4", typeof(string));
            //dtSlashExtData.Columns.Add("SlashExtData5", typeof(string));
            //dtSlashExtData.Columns.Add("SlashExtData6", typeof(string));
            //dtSlashExtData.Columns.Add("SlashExtData7", typeof(string));
            //dtSlashExtData.Columns.Add("SlashExtData8", typeof(string));
            //dtSlashExtData.Columns.Add("SlashExtData9", typeof(string));
            //dtSlashExtData.Columns.Add("SlashExtData10", typeof(string));


            //txtSlashExt.DataBindings.Add("Text", bsSlashExtData, "SlashNo", true); 
            //txtSlash1.DataBindings.Add("Text", bsSlashExtData, "SlashExtData1", true);
            //txtSlash2.DataBindings.Add("Text", bsSlashExtData, "SlashExtData2", true);
            //txtSlash3.DataBindings.Add("Text", bsSlashExtData, "SlashExtData3", true);
            //txtSlash4.DataBindings.Add("Text", bsSlashExtData, "SlashExtData4", true);
            //txtSlash5.DataBindings.Add("Text", bsSlashExtData, "SlashExtData5", true);
            //txtSlash6.DataBindings.Add("Text", bsSlashExtData, "SlashExtData6", true);
            //txtSlash7.DataBindings.Add("Text", bsSlashExtData, "SlashExtData7", true);
            //txtSlash8.DataBindings.Add("Text", bsSlashExtData, "SlashExtData8", true);
            //txtSlash9.DataBindings.Add("Text", bsSlashExtData, "SlashExtData9", true);
            //txtSlash10.DataBindings.Add("Text", bsSlashExtData, "SlashExtData10", true);

            if (nFR == 1)
            {
                txtLogNo.Text = nLogNo.ToString();
                GISClass.General.FindRecord("GBLNo", txtLogNo.Text, bsFile, dgvFile);
                btnClose.Visible = true;
                btnLSPreview.Enabled = true; btnDataForm.Enabled = true;
                dgvFile.Select();
                SendKeys.Send("{Enter}");
            }
        }

        private void IngredionManifestLog_KeyDown(object sender, KeyEventArgs e)
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
                    if (nMode == 0 && strFileAccess != "RO")
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

        private void btnCancelData_Click(object sender, EventArgs e)
        {
            chkAnalyst.Checked = bAnalyst;
            chkManager.Checked = bManager;
            mskDateSAP.Text = strSAPDate; cboSAPTime.Text = strSAPTime;
        }

        private void btnOKData_Click(object sender, EventArgs e)
        {
            string SAPDateTime = "";
           
            SAPDateTime = mskDateSAP.Text + " " + cboSAPTime.Text + ":00:00";
            try
            {
                DateTime dteSAP = Convert.ToDateTime(SAPDateTime);
            }
            catch 
            {
                MessageBox.Show("Invalid SAP Date/Time entry.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (chkManager.Checked == true)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("An invoice entry would be created." + Environment.NewLine + "Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    chkManager.Checked = false;
                    return;
                }
            }
            bsLogFM.EndEdit();

            DataTable dt = new DataTable();
            dt = dtLogFM.GetChanges();
            if (dt != null && dt.Rows.Count > 0)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    InitializeFile();
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@SampleNo", Convert.ToInt32(txtLogNo.Text));
                if (chkAnalyst.Checked == true)
                {
                    sqlcmd.Parameters.AddWithValue("@AnaDone", true);
                    sqlcmd.Parameters.AddWithValue("@AnaDoneDate", DateTime.Now);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@AnaDone", false);
                    sqlcmd.Parameters.AddWithValue("@AnaDoneDate", DBNull.Value);
                }
                if (chkManager.Checked)
                {
                    sqlcmd.Parameters.AddWithValue("@MngrChecked", true);
                    sqlcmd.Parameters.AddWithValue("@MgrCheckedDate", DateTime.Now);
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@MngrChecked", false);
                    sqlcmd.Parameters.AddWithValue("@MgrCheckedDate", DBNull.Value);
                }
                if (SAPDateTime != "")
                    sqlcmd.Parameters.AddWithValue("@SAPDate", Convert.ToDateTime(SAPDateTime));
                else
                    sqlcmd.Parameters.AddWithValue("@SAPDate", DBNull.Value);

                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spIngredionUpdAnalyst";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    InitializeFile();
                    return;
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();

                if (chkManager.Checked == true)
                    CreateInvoice();

                InitializeFile();
            }
        }

        private void mskDateSAP_Enter(object sender, EventArgs e)
        {
            mskDateSAP.SelectAll(); //SendKeys.Send("^{A}");
        }

        private void mskDateSAP_DoubleClick(object sender, EventArgs e)
        {
            mskDateSAP.SelectAll();
        }

        private void cboSAPTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void mskDateSAP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (chkManager.Checked == false)
                e.Handled = true;
        }

        private void txtSSFormNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                dgvSponsors.Visible = false; dgvContacts.Visible = false;

                DataTable dtX = GISClass.Samples.SSFLogMaster(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX == null || dtX.Rows.Count == 0)
                {
                    MessageBox.Show("No matching SSF number", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                txtSponsor.Text = dtX.Rows[0]["SponsorName"].ToString();
                txtSponsorID.Text = dtX.Rows[0]["SponsorID"].ToString();
                txtContactID.Text = dtX.Rows[0]["ContactID"].ToString();
                txtArticle.Text = dtX.Rows[0]["ArticleDesc"].ToString();
                txtAddlNotes.Text = dtX.Rows[0]["Comments"].ToString();
                if (dtX.Rows[0]["DEACategoryA"].ToString() != "" || dtX.Rows[0]["DEACategoryB"].ToString() != "")
                    chkCtrldSubs.Checked = true;
                else
                    chkCtrldSubs.Checked = false;
                txtContact.Text = GISClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
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

                dgvPONo.DataSource = null;
                dtPONo = GISClass.PO.PODDL(Convert.ToInt16(txtSponsorID.Text));
                if (dtPONo != null)
                {
                    try
                    {
                        ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvPONo"]).DataSource = dtPONo;
                        ((DataGridView)dtrLogTests.CurrentItem.Controls["dgvSC"]).Columns[0].Width = 125;
                    }
                    catch { }
                }
                if (dtPONo != null && dtPONo.Rows.Count > 0)
                    txtILPO.Text = dtPONo.Rows[0]["PONo"].ToString();
                else
                    txtILPO.Text = "";
                if (dtFillCodes == null || dtFillCodes.Rows.Count == 0)
                {
                    dtFillCodes = null;
                    dtFillCodes = GISClass.Samples.IngredionManifest();
                    dgvFillCodes.DataSource = dtFillCodes;
                    dgvFillCodes.Columns["FillCode"].Width = 94;
                }
                dtX = null;
                dtX = GISClass.Samples.SSFLogSamples(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX != null && dtX.Rows.Count > 0)
                {
                    if (dtX.Rows.Count == 1 && Convert.ToInt16(dtX.Rows[0]["IngSamplesQty"]) > 1)
                        txtILSlash.Text = "001-" + Convert.ToInt16(dtX.Rows[0]["IngSamplesQty"]).ToString("000");
                    else
                        txtILSlash.Text = "001";
                    txtFillCode.Text = dtX.Rows[0]["ArticleDesc"].ToString();
                    txtILLotNo.Text = dtX.Rows[0]["LotNo"].ToString();
                    txtILBookNo.Text = "828"; //to be added in the settings
                    btnILOK_Click(null, null);
                    chkCtrldSubs.Checked = false;
                }
            }
        }

        private void dtrLogTests_Scroll(object sender, ScrollEventArgs e)
        {
            ((Button)dtrLogTests.CurrentItem.Controls["btnSC"]).Select();
        }

        private void dgvTests_DoubleClick(object sender, EventArgs e)
        {
            if (dgvTests.CurrentCell.OwningColumn.Name.ToString() == "QuoteNo")
            {
                LinkQuote(dgvTests.CurrentCell.Value.ToString());
            }
        }

        private void LinkQuote(string cQNo)
        {
            QuotationRpt rptQuotation = new QuotationRpt();
            rptQuotation.WindowState = FormWindowState.Maximized;
            rptQuotation.nQ = 0;
            rptQuotation.nP = 1;
            try
            {

                int nI = cQNo.IndexOf("R");
                string strQNo = cQNo.Substring(0, nI - 1);
                string strRNo = cQNo.Substring((nI + 1), cQNo.Length - (nI + 1));

                int nRevNo = Convert.ToInt16(strRNo);

                rptQuotation.QuoteNo = strQNo;
                rptQuotation.RevNo = nRevNo;
                rptQuotation.nOld = 0;
                rptQuotation.Show();
            }
            catch { }
        }

        private void txtQuoteNo_DoubleClick(object sender, EventArgs e)
        {
            LinkQuote(((TextBox)dtrLogTests.CurrentItem.Controls["txtQuoteNo"]).Text);
        }

        private void txtILGBLNo_KeyPress(object sender, KeyPressEventArgs e)
        {
             if ((Char.IsDigit(e.KeyChar) == false && e.KeyChar != 8)) // e.KeyChar != 46 -> period
                e.Handled = true;
        }

        private void txtILBookNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar) == false && e.KeyChar != 8)) // e.KeyChar != 46 -> period
                e.Handled = true;
        }

        private void txtILSamples_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsDigit(e.KeyChar) == false && e.KeyChar != 8)) // e.KeyChar != 46 -> period
                e.Handled = true;
        }
    }
}
