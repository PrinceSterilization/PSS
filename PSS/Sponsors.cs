//Sponsors.cs
// AUTHOR       : ALEXANDER M. DELA CRUZ
// TITLE        : Software Developer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Sponsors and Contacts File Maintenance

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace PSS
{
    public partial class Sponsors : PSS.TemplateForm
    {
        public int nQuoteSw;
        public int nSpID;
        private string strFileAccess = "RO";
        //private byte nCH = 0, nCHW = 0;

        byte nMode = 0;
        DataTable dtSizes = new DataTable();
        DataTable dtIndustries = new DataTable();
        DataTable dtStates = new DataTable();
        DataTable dtAddresses = new DataTable();
        DataTable dtCouriers = new DataTable();
        DataTable dtMain = new DataTable();
        DataTable dtSpAddresses = new DataTable();
        DataTable dtContacts = new DataTable();
        DataTable dtConAddresses = new DataTable();
        DataTable dtConNumbers = new DataTable();
        DataTable dtConEMails = new DataTable();
        DataTable dtSpForecasts = new DataTable();
        DataTable dtSpCouriers = new DataTable();
        DataTable dtSpActivities = new DataTable();

        //for DatagridView search
        private int nCtr = 0;
        private int nSw = 0;
        //======================

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private byte nCN = 0;
        private byte nCE = 0;
        private byte nFC = 0;
        private byte nAN = 0;
        private byte nCR = 0;
        private string strState = "";
        private string strCourier = "";
        private int nTRec = 0;
        private int nNewAddressID = 0;
        private int nTab = 0;
        private int nConID = 0;
        private string strChCode = "";

        public Sponsors()
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
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            chkShowInactive.Click += new EventHandler(chkShowInactiveClickHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void FileAccess()
        {
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; 
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true;
            }
        }

        private void LoadStatesDDL()
        {
            dtStates = PSSClass.States.StatesDDL();
            if (dtStates == null)
            {
                return;
            }
            dgvStates.DataSource = dtStates;
        }

        private void LoadSizesDDL()
        {
            dtSizes = PSSClass.Sponsors.SpSizesDDL();

            if (dtSizes == null)
            {
                return;
            }
            cboSizes.DataSource = dtSizes;
            cboSizes.DisplayMember = "SizeDesc";
            cboSizes.ValueMember = "SizeID";
        }

        private void LoadIndustriesDDL()
        {
            dtIndustries = PSSClass.Sponsors.IndustriesDDL();

            if (dtIndustries == null)
            {
                return;
            }
            cboIndustries.DataSource = dtIndustries;
            cboIndustries.DisplayMember = "IndustryDesc";
            cboIndustries.ValueMember = "IndustryID";
        }

        private void LoadCouriersDDL()
        {
            dtCouriers = PSSClass.Couriers.CouriersDDL();

            if (dtCouriers == null)
            {
                return;
            }
            DataRow dR = dtCouriers.NewRow();
            dR["CourierCode"] = "---";
            dtCouriers.Rows.InsertAt(dR, 0);
            cboCouriers.DataSource = dtCouriers;
            cboCouriers.DisplayMember = "CourierCode";
            cboCouriers.ValueMember = "CourierCode";
        }

        private void LoadRecords(int cStatus)
        {
            DataTable dt = PSSClass.Sponsors.SponsorMaster(cStatus);
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "SponsorID <> 0";
            DataGridSetting();
            if (tsddbSearch.DropDownItems.Count == 0)
            {
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
                for (int j = 0; j < cklColumns.Items.Count; j++)
                {
                    cklColumns.SetItemChecked(j, true);
                }
                tsddbSearch.DropDownItems.AddRange(items);
                tslSearchData.Text = tsddbSearch.DropDownItems[1].Text;
                tstbSearchField.Text = tsddbSearch.DropDownItems[1].Name;
            }
            FileAccess();
        }

        private void BuildPrintItems()
        {
            tsddbPrint.DropDownItems.Add("Sorted by Sponsor ID", null, PrintSpCodeClickHandler);
            tsddbPrint.DropDownItems.Add("Sorted by Sponsor Name", null, PrintSpNameClickHandler);
            tsddbPrint.DropDownItems.Add("Grouped By Size", null, PrintSpSizeClickHandler);
            tsddbPrint.DropDownItems.Add("Grouped By Industry", null, PrintSpIndustryClickHandler);
            tsddbPrint.DropDownItems.Add("Grouped By Region/State", null, PrintSpRegStateClickHandler);
            tsddbPrint.DropDownItems.Add("Grouped By State", null, PrintSpStateClickHandler);
            tsddbPrint.DropDownItems.Add("-", null, null);
            tsddbPrint.DropDownItems.Add("On Credit Hold Warning/Credit Hold Status", null, PrintSpOnCHWClickHandler);
            tsddbPrint.DropDownItems.Add("PO-Bound/COD Status", null, PrintSpPOBoundClickHandler);
            tsddbPrint.DropDownItems.Add("-", null, null);
            tsddbPrint.DropDownItems.Add("Audit Trail - Contacts", null, PrintAudit);
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData();
                btnClose.Visible = true;
            }
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

        private void PrintSpNameClickHandler(object sender, EventArgs e)
        {
            SponsorsRpt rptSp = new SponsorsRpt();
            rptSp.WindowState = FormWindowState.Maximized;
            rptSp.rptName = "SpRefName";
            rptSp.rptLabel = "SPONSORS REFERENCE LIST";
            rptSp.Show();
        }

        private void PrintSpCodeClickHandler(object sender, EventArgs e)
        {
            SponsorsRpt rptSp = new SponsorsRpt();
            rptSp.WindowState = FormWindowState.Maximized;
            rptSp.rptName = "SpRefCode";
            rptSp.rptLabel = "SPONSORS REFERENCE LIST";
            rptSp.Show();
        }

        private void PrintSpSizeClickHandler(object sender, EventArgs e)
        {
            SponsorsRpt rptSp = new SponsorsRpt();
            rptSp.WindowState = FormWindowState.Maximized;
            rptSp.rptName = "SpRefSize";
            rptSp.rptLabel = "SPONSORS REFERENCE LIST BY SIZE";
            rptSp.Show();
        }

        private void PrintSpIndustryClickHandler(object sender, EventArgs e)
        {
            SponsorsRpt rptSp = new SponsorsRpt();
            rptSp.WindowState = FormWindowState.Maximized;
            rptSp.rptName = "SpRefIndustry";
            rptSp.rptLabel = "SPONSORS REFERENCE LIST BY INDUSTRY";
            rptSp.Show();
        }

        private void PrintSpOnCHWClickHandler(object sender, EventArgs e)
        {
            SponsorsRpt rptSp = new SponsorsRpt();
            rptSp.WindowState = FormWindowState.Maximized;
            rptSp.rptName = "SpOnCHW";
            rptSp.rptLabel = "SPONSORS ON CREDIT HOLD WARNING/CREDIT HOLD";
            rptSp.Show();
        }

        private void PrintSpPOBoundClickHandler(object sender, EventArgs e)
        {
            SponsorsRpt rptSp = new SponsorsRpt();
            rptSp.WindowState = FormWindowState.Maximized;
            rptSp.rptName = "SpPOBound";
            rptSp.rptLabel = "PO BOUND SPONSORS";
            rptSp.Show();
        }

        private void PrintSpRegStateClickHandler(object sender, EventArgs e)
        {
            SponsorsRpt rptSp = new SponsorsRpt();
            rptSp.WindowState = FormWindowState.Maximized;
            rptSp.rptName = "SpRefRegStates";
            rptSp.rptLabel = "SPONSORS REFERENCE LIST BY REGION/STATES";
            rptSp.Show();
        }

        private void PrintSpStateClickHandler(object sender, EventArgs e)
        {
            SponsorsRpt rptSp = new SponsorsRpt();
            rptSp.WindowState = FormWindowState.Maximized;
            rptSp.rptName = "SpRefStates";
            rptSp.rptLabel = "SPONSORS REFERENCE LIST BY STATE";
            rptSp.Show();
        }

        private void PrintAudit(object sender, EventArgs e)
        {
            SponsorsRpt rptSC = new SponsorsRpt();
            rptSC.WindowState = FormWindowState.Maximized;
            rptSC.rptName = "AuditTrail";
            rptSC.rptLabel = "AUDIT TRAIL";
            rptSC.rptFileName = "CONTACTS MASTER FILE";
            rptSC.Show();
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

        public void SearchOKClickHandler(object sender, EventArgs e)
        {
            try
            {
                bsFile.Filter = "SponsorID<>0";
                PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                dgvFile.Select();
                if (pnlRecord.Visible == true)
                    LoadData();
            }
            catch
            {
            }
        }

        private void SearchFilterClickHandler(object sender, EventArgs e)
        {
            if (tstbSearch.Text.Trim() != "")
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
                    else if (arrCol[nIndex] == "System.Boolean")
                    {
                        if (tstbSearch.Text.ToUpper() == "TRUE" || tstbSearch.Text == "1")
                            bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;
                        else
                            bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text + " OR " + tstbSearchField.Text + " IS NULL";
                    }
                    else
                        bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;

                    dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                    tsbRefresh.Enabled = true;
                }
                catch { }
            }
        }

        private void SearchKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SearchFilterClickHandler(null, null);
                this.Select(); 
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
            tsbRefresh.Enabled = false;
        }

        private void LoadData()
        {
            nMode = 0;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            dtMain.Rows.Clear(); dtSpAddresses.Rows.Clear();
            dtSpCouriers.Rows.Clear(); dtSpForecasts.Rows.Clear(); dtSpActivities.Rows.Clear();
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in tabAPAC.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in pnlAP.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in pnlCRApp.Controls)
            {
                c.DataBindings.Clear();
            }
            txtID.Text = dgvFile.CurrentRow.Cells[0].Value.ToString();
            dtMain = PSSClass.Sponsors.RepAPCAData(Convert.ToInt16(txtID.Text));
            bsMain.DataSource = dtMain;
            if (dtMain.Rows[0]["DatePermanent"].ToString() == "")
            {
                dtpPermDate.Format = DateTimePickerFormat.Custom;
                dtpPermDate.CustomFormat = " ";
            }
            else
            {
                dtpPermDate.Format = DateTimePickerFormat.Custom;
                dtpPermDate.CustomFormat = "MM/dd/yyyy -t";

            }
            if (dtMain.Rows[0]["CRAReturnDate"].ToString() == "")
            {
                dtpCRAReturned.Format = DateTimePickerFormat.Custom;
                dtpCRAReturned.CustomFormat = " ";
            }
            else
            {
                dtpCRAReturned.Format = DateTimePickerFormat.Custom;
                dtpCRAReturned.CustomFormat = "MM/dd/yyyy -t";
            }
            if (dtMain.Rows[0]["CRAMailDate"].ToString() == "")
            {
                dtpCRAEMailed.Format = DateTimePickerFormat.Custom;
                dtpCRAEMailed.CustomFormat = " ";
            }
            else
            {
                dtpCRAEMailed.Format = DateTimePickerFormat.Custom;
                dtpCRAEMailed.CustomFormat = "MM/dd/yyyy -t";
            }
            if (dtMain.Rows[0]["CRStatusCode"].ToString() == "0")
                rdoCRUpdated.Checked = true;
            else if (dtMain.Rows[0]["CRStatusCode"].ToString() == "1")
                rdoCHW.Checked = true;
            else if (dtMain.Rows[0]["CRStatusCode"].ToString() == "2")
                rdoCH.Checked = true;
            if (dtMain.Rows[0]["ChargeType"].ToString() == "0")
                rdoCOD.Checked = true;
            else if (dtMain.Rows[0]["ChargeType"].ToString() == "1")
                rdoPOBound.Checked = true;
            else if (dtMain.Rows[0]["ChargeType"].ToString() == "2")
                rdoCredit.Checked = true;
            //Data Bindings for Main
            txtID.DataBindings.Add("Text", bsMain, "SponsorID", true);
            txtName.DataBindings.Add("Text", bsMain, "SponsorName", true);
            dtpEntryDate.DataBindings.Add("Value", bsMain, "DateCreated", true);
            dtpPermDate.DataBindings.Add("Value", bsMain, "DatePermanent", true);
            txtCRStatus.DataBindings.Add("Text", bsMain, "CRStatusCode", true);
            txtChargeType.DataBindings.Add("Text", bsMain, "ChargeType", true);
            chkMaster.DataBindings.Add("Checked", bsMain, "MAgreement", true);
            chkQuality.DataBindings.Add("Checked", bsMain, "QAgreement", true);
            chkConfidentiality.DataBindings.Add("Checked", bsMain, "CAgreement", true);
            cboSizes.DataBindings.Add("SelectedValue", bsMain, "SizeID", true);
            cboIndustries.DataBindings.Add("SelectedValue", bsMain, "IndustryID", true);
            txtNotes.DataBindings.Add("Text", bsMain, "SponsorNotes", true);
            chkFAX.DataBindings.Add("Checked", bsMain, "FAXReport", true);
            chkPrintSRpt.DataBindings.Add("Checked", bsMain, "PrintSpeedReport", true);
            chkSendSRpt.DataBindings.Add("Checked", bsMain, "SendSpeedReport", true);
            txtAPContact.DataBindings.Add("Text", bsMain, "APContact", true);
            txtAPEMail.DataBindings.Add("Text", bsMain, "APEMail", true);
            mskAPTelNo.DataBindings.Add("Text", bsMain, "APTelNo", true);
            mskAPFAXNo.DataBindings.Add("Text", bsMain, "APFAXNo", true);
            txtBDSEMail.DataBindings.Add("Text", bsMain, "BDSEMail", true);
            chkBDSInv.DataBindings.Add("Checked", bsMain, "BDSInvoice", true);
            chkBDSUser.DataBindings.Add("Checked", bsMain, "BDSUser", true);
            chkInactive.DataBindings.Add("Checked", bsMain, "Inactive" ,false, DataSourceUpdateMode.OnPropertyChanged, false);

            dtAddresses = PSSClass.Sponsors.SpAddressesDDL(Convert.ToInt16(txtID.Text));
            dgvSpAddresses.DataSource = dtAddresses;

            LoadContacts();
            if (tbcSubData.SelectedIndex == 1)
            {
                LoadAddresses(Convert.ToInt16(txtID.Text));
                LoadConAddresses();
            }
            else if (tbcSubData.SelectedIndex == 2)
            {
                LoadConNumbers();
                LoadConEMails();
            }
            else if (tbcSubData.SelectedIndex == 3)
            {
                LoadForecasts();
            }
            else if (tbcSubData.SelectedIndex == 4)
            {
                LoadCouriers();
            }
            else if (tbcSubData.SelectedIndex == 5)
            {
                LoadActivities();
            }
            btnClose.Visible = true; btnClose.BringToFront();
            btnAddContact.Enabled = false; btnCancelContact.Enabled = false; btnDelContact.Enabled = false;
            btnAddSpAddr.Enabled = false; btnCancelSpAddr.Enabled = false; btnDelSpAddr.Enabled = false;
            btnAddConAddr.Enabled = false; btnCancelConAddr.Enabled = false; btnDelConAddr.Enabled = false;
            btnAddConNo.Enabled = false; btnCancelConNo.Enabled = false; btnDelConNo.Enabled = false;
            btnAddConEMail.Enabled = false; btnCancelConEMail.Enabled = false; btnDelConEMail.Enabled = false;
            btnAddCourier.Enabled = false; btnCancelCourier.Enabled = false; btnDelCourier.Enabled = false;
            btnAddForecast.Enabled = false; btnCancelForecast.Enabled = false; btnDelForecast.Enabled = false;
            btnAddActivity.Enabled = false; btnCancelActivity.Enabled = false; btnDelActivity.Enabled = false;
            OpenControls(pnlRecord, false);
            OpenControls(pnlAP, false); OpenControls(pnlCRApp, false);
            OpenControls(pnlCRStatus, false); OpenControls(pnlChargeType, false);
        }

        private bool MatchingRecord(string strKeyField, string strMatchField, string strTableName, string strMatchData)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            SqlCommand sqlcmd = new SqlCommand("SELECT " + strKeyField + ", " + strMatchField + " FROM " + strTableName +
                                               " WHERE " + strMatchField + "='" + strMatchData.Replace("'", "''") + "'", sqlcnn);
            SqlDataReader sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                if (nMode == 1)
                    return true;
                else
                {
                    sqldr.Read();
                    string strID = Convert.ToString(sqldr.GetValue(0));
                    int nID = Convert.ToInt16(strID);
                    if (nID != Convert.ToInt16(txtID.Text))
                        return true;
                }
            }
            sqldr.Close(); sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
            return false;
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["SponsorID"].HeaderText = "ID";
            dgvFile.Columns["SponsorName"].HeaderText = "NAME";
            dgvFile.Columns["DateEntered"].HeaderText = "DATE ENTERED";
            dgvFile.Columns["DatePermanent"].HeaderText = "DATE PERMANENT";
            dgvFile.Columns["Size"].HeaderText = "SIZE";
            dgvFile.Columns["Industry"].HeaderText = "INDUSTRY";
            dgvFile.Columns["ServiceAgreement"].HeaderText = "AGREEMENT TYPES";
            dgvFile.Columns["CreditStatus"].HeaderText = "CREDIT STATUS";
            dgvFile.Columns["ChargeType"].HeaderText = "CHARGE TYPE";
            dgvFile.Columns["SponsorStatus"].HeaderText = "STATUS";
            dgvFile.Columns["CreditLimit"].HeaderText = "CREDIT LIMIT";
            dgvFile.Columns["SponsorID"].Width = 80;
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Padding = new Padding(5, 0, 0, 0);
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorName"].Width = 375;
            dgvFile.Columns["DateEntered"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateEntered"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateEntered"].Width = 130;
            dgvFile.Columns["DatePermanent"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DatePermanent"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DatePermanent"].Width = 150;
            dgvFile.Columns["Size"].Width = 120;
            dgvFile.Columns["Industry"].Width = 150;
            dgvFile.Columns["ServiceAgreement"].Width = 200;
            dgvFile.Columns["CreditStatus"].Width = 120;
            dgvFile.Columns["ChargeType"].Width = 120;
            dgvFile.Columns["SponsorStatus"].Width = 85;
            dgvFile.Columns["SponsorName"].Frozen = true;
            chkShowInactive.Visible = true;
        }

        public void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            if (dgvFile.Rows.Count > 0)
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

        private void AddRecord()
        {
            nMode = 1;
            ClearControls(pnlRecord);
            OpenControls(pnlRecord, true);
            OpenControls(pnlAP, true); OpenControls(pnlCRApp, false);
            dtpEntryDate.Enabled = false; dtpPermDate.Enabled = false;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; tsbRefresh.Enabled = false; btnClose.Visible = true; 
            dtpPermDate.Format = DateTimePickerFormat.Custom; dtpPermDate.CustomFormat = " ";
            dtpCRAEMailed.Format = DateTimePickerFormat.Custom; dtpCRAEMailed.CustomFormat = " ";
            dtpCRAReturned.Format = DateTimePickerFormat.Custom; dtpCRAReturned.CustomFormat = " ";
            tbcSubData.SelectedIndex = 0;
            chkInactive.Checked = false; chkInactive.Enabled = false;
            btnAddContact.Enabled = false; btnCancelContact.Enabled = false; 
            btnAddConAddr.Enabled = false; btnCancelConAddr.Enabled = false;
            btnAddConNo.Enabled = false; btnCancelConNo.Enabled = false;
            btnAddConEMail.Enabled = false; btnCancelConEMail.Enabled = false;
            chkShowInactive.Visible = false; cboStates.SelectedIndex = -1;
            dtMain.Rows.Clear();
            dtContacts.Rows.Clear(); dtSpAddresses.Rows.Clear(); dtConAddresses.Rows.Clear();
            dtConNumbers.Rows.Clear(); dtConEMails.Rows.Clear(); dtSpForecasts.Rows.Clear();
            dtSpCouriers.Rows.Clear(); dtSpActivities.Rows.Clear();
            //Add Blank Main Data
            DataRow dR = dtMain.NewRow();
            dR["SponsorID"] = "(New)";
            dR["SponsorName"] = "";
            dR["DateCreated"] = DateTime.Now;
            dR["DatePermanent"] = DateTime.Now;
            dR["SizeID"] = 0;
            dR["IndustryID"] = 0;
            dR["MAgreement"] = false;
            dR["QAgreement"] = false;
            dR["CAgreement"] = false;
            dR["SponsorNotes"] = "";
            dR["FAXReport"] = false;
            dR["PrintSpeedReport"] = false;
            dR["SendSpeedReport"] = false;
            dR["CRStatusCode"] = 0;
            dR["ChargeType"] = 0;
            dR["APContact"] = "";
            dR["APTelNo"] = "";
            dR["APFAXNo"] = "";
            dR["APEMail"] = "";
            dR["BDSEMail"] = "";
            dR["BDSUser"] = false;
            dR["BDSInvoice"] = false;
            dR["CRAMailDate"] = DateTime.Now;
            dR["CRAReturnDate"] = DateTime.Now;
            dR["CreditLimit"] = 0;
            dtMain.Rows.Add(dR);
            bsMain.DataSource = dtMain;

            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in tabAPAC.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in pnlAP.Controls)
            {
                c.DataBindings.Clear();
            }
            foreach (Control c in pnlCRApp.Controls)
            {
                c.DataBindings.Clear();
            }
            //Data Bindings for Main
            txtID.DataBindings.Add("Text", bsMain, "SponsorID", true);
            txtName.DataBindings.Add("Text", bsMain, "SponsorName", true);
            dtpEntryDate.DataBindings.Add("Value", bsMain, "DateCreated", true);
            dtpPermDate.DataBindings.Add("Value", bsMain, "DatePermanent", true);
            txtCRStatus.DataBindings.Add("Text", bsMain, "CRStatusCode", true);
            txtChargeType.DataBindings.Add("Text", bsMain, "ChargeType", true);
            chkMaster.DataBindings.Add("Checked", bsMain, "MAgreement", true);
            chkQuality.DataBindings.Add("Checked", bsMain, "QAgreement", true);
            chkConfidentiality.DataBindings.Add("Checked", bsMain, "CAgreement", true);
            cboSizes.DataBindings.Add("SelectedValue", bsMain, "SizeID", true);
            cboIndustries.DataBindings.Add("SelectedValue", bsMain, "IndustryID", true);
            txtNotes.DataBindings.Add("Text", bsMain, "SponsorNotes", true);
            chkFAX.DataBindings.Add("Checked", bsMain, "FAXReport", true);
            chkPrintSRpt.DataBindings.Add("Checked", bsMain, "PrintSpeedReport", true);
            chkSendSRpt.DataBindings.Add("Checked", bsMain, "SendSpeedReport", true);
            txtAPContact.DataBindings.Add("Text", bsMain, "APContact", true);
            txtAPEMail.DataBindings.Add("Text", bsMain, "APEMail", true);
            mskAPTelNo.DataBindings.Add("Text", bsMain, "APTelNo", true);
            mskAPFAXNo.DataBindings.Add("Text", bsMain, "APFAXNo", true);
            txtBDSEMail.DataBindings.Add("Text", bsMain, "BDSEMail", true);
            chkBDSInv.DataBindings.Add("Checked", bsMain, "BDSInvoice", true);
            chkBDSUser.DataBindings.Add("Checked", bsMain, "BDSUser", true);
            btnAddSpAddr_Click(null, null);
            btnAddContact_Click(null, null);
            txtName.Focus();
        }

        private void EditRecord()
        {
            if (pnlRecord.Visible == false)
            {
                LoadData();
                tbcSubData.SelectedIndex = 1;
            }
            nMode = 2;
            strChCode = dtMain.Rows[0]["CRStatusCode"].ToString();
            OpenControls(pnlRecord, true);
            OpenControls(pnlAP, true); OpenControls(pnlCRApp, true);
            OpenControls(pnlCRStatus, true); OpenControls(pnlChargeType, true);
            txtCRAMailedBy.ReadOnly = true; dtpCRAEMailed.Enabled = false; tsbRefresh.Enabled = false;
            btnAddContact.Enabled = true; 
            btnAddConAddr.Enabled = true; 
            btnAddSpAddr.Enabled = true; 
            btnAddConNo.Enabled = true; 
            btnAddConEMail.Enabled = true; 
            btnAddForecast.Enabled = true; 
            btnAddCourier.Enabled = true; 
            btnAddActivity.Enabled = true; 
            btnClose.Visible = true;
            txtName.Focus();
        }

        private void DeleteRecord()
        {
            if (pnlRecord.Visible == false)
                LoadData();

            if (dtContacts.Rows.Count != 0)
            {
                MessageBox.Show("Please delete all contacts" + Environment.NewLine + "of this Sponsor first.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                DialogResult dConfirm = new DialogResult();
                dConfirm = MessageBox.Show("WARNING: This process would delete " + Environment.NewLine + "all records pertaining to this Sponsor!", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (dConfirm == DialogResult.OK)
                {
                    SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                    if (sqlcnn == null)
                    {
                        MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;
                    sqlcmd.Parameters.Add(new SqlParameter("@SpID", SqlDbType.Int));
                    sqlcmd.Parameters["@SpID"].Value = Convert.ToInt16(txtID.Text);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spDelSponsor";

                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Problem encountered: " + ex.Message + Environment.NewLine + "Record is not deleted!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        return;
                    }
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    LoadRecords(1);
                }
            }
        }

        private void SaveRecord()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Sponsor Name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }
            if (cboSizes.Text == "")
            {
                MessageBox.Show("Please select Sponsor size.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboSizes.Focus();
                return;
            }
            if (cboIndustries.Text == "")
            {
                MessageBox.Show("Please select industry type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboIndustries.Focus();
                return;
            }
            if (MatchingRecord("SponsorID", "SponsorName", "Sponsors", txtName.Text) == true)
            {
                MessageBox.Show("Matching Sponsor Name found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }
            string strAgreements = "";
            if (chkMaster.Checked == true && chkQuality.Checked == true && chkConfidentiality.Checked == true)
                strAgreements = "1,2,3";
            else if (chkMaster.Checked == true && chkQuality.Checked == true && chkConfidentiality.Checked == false)
                strAgreements = "1,2";
            else if (chkMaster.Checked == true && chkQuality.Checked == false && chkConfidentiality.Checked == false)
                strAgreements = "1";
            else if (chkMaster.Checked == true && chkQuality.Checked == false && chkConfidentiality.Checked == true)
                strAgreements = "1,3";
            else if (chkMaster.Checked == false && chkQuality.Checked == true && chkConfidentiality.Checked == true)
                strAgreements = "2,3";
            else if (chkMaster.Checked == false && chkQuality.Checked == false && chkConfidentiality.Checked == true)
                strAgreements = "3";
            else if (chkMaster.Checked == false && chkQuality.Checked == true && chkConfidentiality.Checked == false)
                strAgreements = "2";

            byte nM = 0;
            btnAddContact.Focus();
            bsMain.EndEdit();
            if (dtMain.Rows[0].RowState.ToString() == "Modified")
                nM = 1;
            
            if (nMode == 1 || nM == 1)
            {
                if (nMode == 1)
                {
                    txtID.Text = PSSClass.DataEntry.NewSpID("Sponsors", "SponsorID").ToString();
                }

                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", nMode);
                sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtID.Text));
                sqlcmd.Parameters.AddWithValue("@SpName", txtName.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@SpNotes", txtNotes.Text.Trim().Replace("'", "''"));
                sqlcmd.Parameters.AddWithValue("@FAXRpt", chkFAX.CheckState);
                sqlcmd.Parameters.AddWithValue("@PrintSRpt", chkPrintSRpt.CheckState);
                sqlcmd.Parameters.AddWithValue("@SendSRpt", chkSendSRpt.CheckState);
                if (rdoCHW.Checked == true)
                    sqlcmd.Parameters.AddWithValue("@CRStatus", 1);
                else if (rdoCH.Checked == true)
                    sqlcmd.Parameters.AddWithValue("@CRStatus", 2);
                else
                    sqlcmd.Parameters.AddWithValue("@CRStatus", 0);

                if (rdoCOD.Checked == true)
                    sqlcmd.Parameters.AddWithValue("@ChargeType", 0);
                else if (rdoPOBound.Checked == true)
                    sqlcmd.Parameters.AddWithValue("@ChargeType", 1);
                else
                    sqlcmd.Parameters.AddWithValue("@ChargeType", 2);

                sqlcmd.Parameters.AddWithValue("@SpSize", cboSizes.SelectedValue);
                sqlcmd.Parameters.AddWithValue("@IndID", cboIndustries.SelectedValue);
                sqlcmd.Parameters.AddWithValue("@Agreements", strAgreements);
                sqlcmd.Parameters.AddWithValue("@APContact", txtAPContact.Text.Trim().Replace("'", "''"));
                if (mskAPTelNo.Text.Trim() == "" || mskAPTelNo.Text == "(   )    -     Ext     ; (   )    -     Ext ")
                    sqlcmd.Parameters.AddWithValue("@APTelNo", DBNull.Value);
                else
                    sqlcmd.Parameters.AddWithValue("@APTelNo", mskAPTelNo.Text);
                if (mskAPFAXNo.Text.Trim() == "" || mskAPFAXNo.Text == "(   )    -     Ext     ; (   )    -     Ext ")
                    sqlcmd.Parameters.AddWithValue("@APFAXNo", DBNull.Value);
                else
                    sqlcmd.Parameters.AddWithValue("@APFAXNo", mskAPFAXNo.Text);
                sqlcmd.Parameters.AddWithValue("@APEMail", txtAPEMail.Text.Trim().Replace("'", "''"));
                sqlcmd.Parameters.AddWithValue("@BDSUser", chkBDSUser.CheckState);
                sqlcmd.Parameters.AddWithValue("@BDSEMail", txtBDSEMail.Text.Trim().Replace("'", "''"));
                sqlcmd.Parameters.AddWithValue("@BDSInv", chkBDSInv.CheckState);
                if (dtpCRAReturned.Checked == true)
                    sqlcmd.Parameters.AddWithValue("@CRARetDate", dtpCRAReturned.Value);
                else
                    sqlcmd.Parameters.AddWithValue("@CRARetDate", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@CRAFile", txtCRAFileName.Text.Trim());
                if (txtCRLimit.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@CRLimit", Convert.ToInt32(txtCRLimit.Text.Replace(",", "")));
                else
                    sqlcmd.Parameters.AddWithValue("@CRLimit", DBNull.Value);

                if (chkInactive.Checked == false)
                    sqlcmd.Parameters.AddWithValue("@Active", 1);
                else
                    sqlcmd.Parameters.AddWithValue("@Active", 0);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditSponsor";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            txtID.Focus();
            bsSpAddresses.EndEdit();
            UpdateSpAddresses(nMode); //Save Addresses
            UpdateContacts(nMode);//Save Contacts
            if (((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text != "(New)")
            {
                bsConAddresses.EndEdit(); bsConNumbers.EndEdit(); bsConEMails.EndEdit();
                UpdateConAddresses(nMode); //Save Contact Addresses
                UpdateContactNos(nMode); //Save Contact Numbers
                UpdateContactEMails(nMode); //Save Contact EMails
            }
            bsSpForecasts.EndEdit(); bsSpCouriers.EndEdit(); bsSpActivities.EndEdit();
            UpdateSpForecasts(nMode); //Save Forecast
            UpdateSpCouriers(nMode); //Save Couriers
            UpdateSpActivities(nMode); //Save Activities
            dtContacts.AcceptChanges();
            AddEditMode(false);
            LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
            PSSClass.General.FindRecord("SponsorID", txtID.Text, bsFile, dgvFile);
            LoadData();
            dgvFile.Visible = false; pnlRecord.Visible = true;
            btnClose.Visible = true; btnClose.BringToFront();
            btnAddSpAddr.Enabled = false; btnCancelSpAddr.Enabled = false;
            btnAddContact.Enabled = false; btnCancelContact.Enabled = false; 
            btnAddConAddr.Enabled = false; btnCancelConAddr.Enabled = false;
            btnAddConNo.Enabled = false; btnCancelConNo.Enabled = false;
            btnAddConEMail.Enabled = false; btnCancelConEMail.Enabled = false;
            btnAddForecast.Enabled = false; btnCancelForecast.Enabled = false;
            btnAddCourier.Enabled = false; btnCancelCourier.Enabled = false;
            btnAddActivity.Enabled = false; btnCancelActivity.Enabled = false;
            
            try
            {
                DataColumn[] keys = new DataColumn[1];
                keys[0] = dtContacts.Columns["ContactID"];
                dtContacts.PrimaryKey = keys;
                int index = dtContacts.Rows.IndexOf(dtContacts.Rows.Find(nConID));
                dtrContacts.CurrentItemIndex = index;
            }
            catch { }

            if (nM == 1)
            {
                if (rdoCHW.Checked == true && strChCode != "1")
                {
                    //Create Statement of Account
                    CreateSOA();

                    ////Get Outstanding Balance
                    decimal nB = 0;
                    nB = PSSClass.ACCPAC.ARBalance(txtID.Text);

                    //Get AP Email Address
                    DataTable dt = PSSClass.Sponsors.APData(Convert.ToInt16(txtID.Text));
                    txtTo.Text = dt.Rows[0]["APEMail"].ToString();
                    dt.Dispose();

                    txtCC.Text = "";
                    txtSubject.Text = "Credit Hold Warning";
                    txtBody.Text = "Dear Sponsor,<br><br>";
                    txtBody.Text = txtBody.Text + "This is to inform you that a review of your account indicates that you have past due invoices<br>";
                    txtBody.Text = txtBody.Text + "in the amount of " + nB.ToString("$#,##0.00") + " (see attached statement).<br><br>";
                    txtBody.Text = txtBody.Text + "If payment is to be made by credit card, please contact our accounting department at (973) 582-1520. <br><br>"; 
                    txtBody.Text = txtBody.Text + "In the meantime, your account is on Credit Hold Warning status until we receive full payment.<br>";
                    txtBody.Text = txtBody.Text + "If payment is not received in five (5) business days, your account will be automatically placed<br>";
                    txtBody.Text = txtBody.Text + "on <b>Credit Hold</b>. Credit Hold status delays the release of your results.<br><br>";
                    txtBody.Text = txtBody.Text + "Please feel free to contact us if you have any questions or to make payment arrangements.<br><br>";
                    txtBody.Text = txtBody.Text + "Thank you for your cooperation.<br><br>";
                    txtBody.Text = txtBody.Text + "Sincerely,<br><br>";
                    lblCHHeader.Text = "CREDIT HOLD WARNING STATUS - EMAIL";
                    int nX = (pnlRecord.Location.X + pnlRecord.Width - pnlEMail.Width) / 2;
                    int nY = (pnlRecord.Location.Y + pnlRecord.Height - pnlEMail.Height) / 2;
                    pnlRecord.Enabled = false; pnlEMail.Visible = true; pnlEMail.BringToFront(); pnlEMail.Location = new Point(nX, nY);
                }
                else if (rdoCH.Checked == true && strChCode != "2")
                {
                    //Create Statement of Account
                    CreateSOA();

                    //Get AP Email Address
                    DataTable dt = PSSClass.Sponsors.APData(Convert.ToInt16(txtID.Text));
                    txtTo.Text = dt.Rows[0]["APEMail"].ToString();
                    dt.Dispose();

                    //Get Sponsor's Terms of Payment
                    int nT = PSSClass.Sponsors.SponsorCRTerms(Convert.ToInt16(txtID.Text));

                    txtCC.Text = "mpannullo@princesterilization.com;jpillcorema@princesterilization.com;jmastej@princesterilization.com;" +
                                 "djprince@princesterilization.com;dlprince@princesterilization.com";
                    txtSubject.Text = "Credit Hold";
                    txtBody.Text = "Dear Sponsor, <br><br>";
                    txtBody.Text = txtBody.Text + "The release of Prince Sterilization Services communications, such as Final Reports,<br>";
                    txtBody.Text = txtBody.Text + "are governed by our policy. Please be aware that it is necessary for your company to<br>";
                    txtBody.Text = txtBody.Text + "not exceed our credit terms (" + nT.ToString() + " days" + "). Until your account is brought into a state of<br>";
                    txtBody.Text = txtBody.Text + "compliance with the credit terms that we have set for your company, we are not able<br>";
                    txtBody.Text = txtBody.Text + "to release Final Reports.<br><br>";
                    txtBody.Text = txtBody.Text + "You may remit a check or if payment is to be made by credit card, please contact<br>";
                    txtBody.Text = txtBody.Text + "our accounting department at (973) 582-1520.<br><br>";
                    txtBody.Text = txtBody.Text + "Please feel free to contact us if you need assistance.<br><br>";
                    txtBody.Text = txtBody.Text + "Sincerely,<br><br>";
                    lblCHHeader.Text = "CREDIT HOLD STATUS - EMAIL";
                    int nX = (pnlRecord.Location.X + pnlRecord.Width - pnlEMail.Width) / 2;
                    int nY = (pnlRecord.Location.Y + pnlRecord.Height - pnlEMail.Height) / 2;
                    pnlRecord.Enabled = false; pnlEMail.Visible = true; pnlEMail.BringToFront(); pnlEMail.Location = new Point(nX, nY);
                }
                else if (rdoCRUpdated.Checked == true && strChCode != "0" && strChCode == "2")
                {
                    //Create Statement of Account
                    CreateSOA();

                    //Get AP Email Address
                    DataTable dt = PSSClass.Sponsors.APData(Convert.ToInt16(txtID.Text));
                    txtTo.Text = dt.Rows[0]["APEMail"].ToString();
                    dt.Dispose();

                    txtCC.Text = "mpannullo@princesterilization.com;jpillcorema@princesterilization.com;jmastej@princesterilization.com;" +
                                 "djprince@princesterilization.com;dlprince@princesterilization.com";

                    txtSubject.Text = "Credit Hold Release";
                    txtBody.Text = "Dear Sponsor,<br><br>";
                    txtBody.Text = txtBody.Text + "Thank you for your payment and attention to this matter.<br><br>";
                    txtBody.Text = txtBody.Text + "We are pleased to inform you that your account has been taken off credit hold.<br>";
                    txtBody.Text = txtBody.Text + "Speed and/or final reports will be released as soon as results are available.<br><br>";
                    txtBody.Text = txtBody.Text + "We appreciate your business and we look forward to your continued support.<br><br>";
                    txtBody.Text = txtBody.Text + "Sincerely,<br><br>";
                    txtSubject.Text = "Credit Hold Release";
                    int nX = (pnlRecord.Location.X + pnlRecord.Width - pnlEMail.Width) / 2;
                    int nY = (pnlRecord.Location.Y + pnlRecord.Height - pnlEMail.Height) / 2;
                    pnlRecord.Enabled = false; pnlEMail.Visible = true; pnlEMail.BringToFront(); pnlEMail.Location = new Point(nX, nY);
                }
            }
            nM = 0; strChCode = "";
        }

        private void CreateSOA()
        {
            //Create Statement of Account (SOA)
            AcctgRpt rpt = new AcctgRpt();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "SOA";
            rpt.nQ = 3;
            rpt.nSpID = Convert.ToInt16(txtID.Text);
            try
            {
                rpt.Show();
            }
            catch { }
            rpt.Close(); rpt.Dispose();
            lstAttachment.Items.Clear();

            lnkSOA.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\SOA\" + DateTime.Now.Year.ToString() + @"\SOA-" + txtID.Text + ".pdf";
            lstAttachment.Items.Add(lnkSOA.Text);
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

        private void UpdateContacts(byte cMode)
        {
            bsContacts.EndEdit();
            int nAdded = 0; int nEdited = 0;
            DataTable dt = new DataTable();
            if (cMode == 2)
            {
                dt = dtContacts.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int nSaveRev = SaveContact(Convert.ToInt16(dt.Rows[i]["ContactID"]), Convert.ToInt16(txtID.Text), i, 2, dt);
                        nEdited += nSaveRev;
                    }
                    dt.Rows.Clear();
                }
            }
            dt = dtContacts.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nID = PSSClass.General.NewID("Contacts", "ContactID");
                    int nSaveRev = SaveContact(nID, Convert.ToInt16(txtID.Text), i, 1, dt);
                    nConID = nID;
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            if (nAdded != 0 || nEdited != 0)
            {
                dtContacts.AcceptChanges();
                bsContacts.DataSource = dtContacts;
                dtrContacts.CurrentItemIndex = dtContacts.Rows.Count - 1;
            }
            
        }

        private void UpdateSpAddresses(byte cMode)
        {
            int nAdded = 0; int nEdited = 0;
            DataTable dt = new DataTable();
            if (cMode == 2)
            {
                dt = dtSpAddresses.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int nSaveRev = SaveSpAddress(Convert.ToInt16(dt.Rows[i]["AddressID"]), Convert.ToInt16(txtID.Text), i, 2, dt);
                        nEdited += nSaveRev;
                    }
                    dt.Rows.Clear();
                }
            }
            dt = dtSpAddresses.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nID = PSSClass.General.NewID("SponsorAddresses", "AddressID");
                    if (cMode == 1)
                        nNewAddressID = nID;

                    int nSaveRev = SaveSpAddress(nID, Convert.ToInt16(txtID.Text), i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            if (nAdded != 0 || nEdited != 0)
            {
                dtSpAddresses.AcceptChanges();
                bsSpAddresses.DataSource = dtSpAddresses;
            }
            
        }

        private void UpdateConAddresses(byte cMode)
        {

            int nCID = 0;
            int nAdded = 0; int nEdited = 0;
            DataTable dt = new DataTable();
            if (cMode == 2)
            {
                dt = dtConAddresses.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        nCID = Convert.ToInt16(dt.Rows[i]["ConID"]);
                        if (dt.Rows[i]["ConAddressID"].ToString() != dt.Rows[i]["ConAddrID"].ToString())
                        {
                            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                            if (sqlcnn == null)
                            {
                                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                            SqlCommand sqlcmd = new SqlCommand();
                            sqlcmd.Connection = sqlcnn;

                            //Delete addresses
                            sqlcmd.Parameters.AddWithValue("@ConID", nCID);
                            sqlcmd.Parameters.AddWithValue("@AddressID", Convert.ToInt16(dt.Rows[i]["ConAddrID"]));

                            sqlcmd.CommandType = CommandType.StoredProcedure;
                            sqlcmd.CommandText = "spDelConAddress";

                            try
                            {
                                sqlcmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                            int nSaveRev = SaveConAddress(Convert.ToInt16(dt.Rows[i]["ConAddressID"]), nCID, i, 1, dt);
                            nAdded += nSaveRev;
                        }
                        else
                        {
                            int nSaveRev = SaveConAddress(Convert.ToInt16(dt.Rows[i]["ConAddressID"]), nCID, i, 2, dt);
                            nEdited += nSaveRev;
                        }
                    }
                    dt.Rows.Clear();
                }
                dt = dtConAddresses.GetChanges(DataRowState.Added);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        nCID = Convert.ToInt16(dt.Rows[i]["ConID"]);
                        if (dt.Rows[i]["ConAddressID"].ToString().Trim() != "(New)")
                        {
                            int nSaveRev = SaveConAddress(Convert.ToInt16(dt.Rows[i]["ConAddressID"]), nCID, i, 1, dt);
                            nAdded += nSaveRev;
                        }
                    }
                    dt.Rows.Clear();
                }
            }
            if (nAdded != 0 || nEdited != 0)
            {
                dtConAddresses.AcceptChanges();
                bsConAddresses.DataSource = dtConAddresses;
            }
        }

        private void UpdateContactNos(byte cMode)
        {
            int nCID = 0;
            int nAdded = 0; int nEdited = 0;
            DataTable dt = new DataTable();
            if (cMode == 2)
            {
                dt = dtConNumbers.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        nCID = Convert.ToInt16(dt.Rows[i]["ConID"]);
                        int nSaveRev = SaveConNumber(Convert.ToInt16(dt.Rows[i]["PhoneID"]), nCID, i, 2, dt);
                        nEdited += nSaveRev;
                    }
                    dt.Rows.Clear();
                }
            }
            dt = dtConNumbers.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCID = Convert.ToInt16(dt.Rows[i]["ConID"]);
                    int nID = PSSClass.General.NewID("ContactNumbers", "PhoneID");
                    if (cMode == 1)
                        nNewAddressID = nID;

                    int nSaveRev = SaveConNumber(nID, nCID, i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            if (nAdded != 0 || nEdited != 0)
            {
                dtConNumbers.AcceptChanges();
                bsConNumbers.DataSource = dtConNumbers;
            }
        }

        private void UpdateContactEMails(byte cMode)
        {
            int nCID = 0;

            int nAdded = 0; int nEdited = 0;
            DataTable dt = new DataTable();
            if (cMode == 2)
            {
                dt = dtConEMails.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        nCID = Convert.ToInt16(dt.Rows[i]["ConID"]);
                        int nSaveRev = SaveConEMail(Convert.ToInt16(dt.Rows[i]["EMailID"]), nCID, i, 2, dt);
                        nEdited += nSaveRev;
                    }
                    dt.Rows.Clear();
                }
            }
            dt = dtConEMails.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nCID = Convert.ToInt16(dt.Rows[i]["ConID"]);
                    int nID = PSSClass.General.NewID("ContactEMAddresses", "EMailID");
                    if (cMode == 1)
                        nNewAddressID = nID;

                    int nSaveRev = SaveConEMail(nID, nCID, i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            if (nAdded != 0 || nEdited != 0)
            {
                dtConEMails.AcceptChanges();
                bsConEMails.DataSource = dtConEMails;
            }
        }

        private void UpdateSpForecasts(byte cMode)
        {
            int nAdded = 0; int nEdited = 0;
            DataTable dt = new DataTable();
            if (cMode == 2)
            {
                dt = dtSpForecasts.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int nSaveRev = SaveSpForecast(Convert.ToInt16(dt.Rows[i]["ForecastYear"]), Convert.ToInt16(txtID.Text), i, 2, dt);
                        nEdited += nSaveRev;
                    }
                    dt = null;
                }
                dt = dtSpForecasts.GetChanges(DataRowState.Deleted);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int nSaveRev = DelSpForecast(Convert.ToInt16(dt.Rows[i]["ForecastYear", DataRowVersion.Original]), Convert.ToInt16(txtID.Text), i, 2, dt);
                        nEdited += nSaveRev;
                    }
                    dt = null;
                }
            }
            dt = dtSpForecasts.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveSpForecast(Convert.ToInt16(dt.Rows[i]["ForecastYear"]), Convert.ToInt16(txtID.Text), i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            if (nAdded != 0 || nEdited != 0)
            {
                dtSpForecasts.AcceptChanges();
                bsSpForecasts.DataSource = dtSpForecasts;
            }
        }

        private void UpdateSpCouriers(byte cMode)
        {
            int nAdded = 0; int nEdited = 0;
            DataTable dt = new DataTable();
            if (cMode == 2)
            {
                dt = dtSpCouriers.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int nSaveRev = SaveSpCourier(Convert.ToInt16(txtID.Text), i, 2, dt);
                        nEdited += nSaveRev;
                    }
                    dt.Rows.Clear();
                }
            }
            dt = dtSpCouriers.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveSpCourier(Convert.ToInt16(txtID.Text), i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            if (nAdded != 0 || nEdited != 0)
            {
                dtSpCouriers.AcceptChanges();
                bsSpCouriers.DataSource = dtSpCouriers;
            }
        }

        private void UpdateSpActivities(byte cMode)
        {
            int nAdded = 0; int nEdited = 0;
            DataTable dt = new DataTable();
            if (cMode == 2)
            {
                dt = dtSpActivities.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        int nSaveRev = SaveSpActivity(Convert.ToInt16(dt.Rows[i]["ActivityYear"]), Convert.ToInt16(txtID.Text), i, 2, dt);
                        nEdited += nSaveRev;
                    }
                    dt.Rows.Clear();
                }
            }
            dt = dtSpActivities.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveSpActivity(Convert.ToInt16(dt.Rows[i]["ActivityYear"]), Convert.ToInt16(txtID.Text), i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            if (nAdded != 0 || nEdited != 0)
            {
                dtSpActivities.AcceptChanges();
                bsSpActivities.DataSource = dtSpActivities;
            }
        }

        private static int SaveContact(int cID, int cSpID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            if (cDT.Rows[cI]["LastName"].ToString().Trim() != "" && cDT.Rows[cI]["FirstName"].ToString().Trim() != "")
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    return 0;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", cMode);
                sqlcmd.Parameters.AddWithValue("@ConID", cID);
                sqlcmd.Parameters.AddWithValue("@SpID", cSpID);
                sqlcmd.Parameters.AddWithValue("@Title", cDT.Rows[cI]["Title"]);
                sqlcmd.Parameters.AddWithValue("@LName", cDT.Rows[cI]["LastName"]);
                sqlcmd.Parameters.AddWithValue("@FName", cDT.Rows[cI]["FirstName"]);
                sqlcmd.Parameters.AddWithValue("@MI", cDT.Rows[cI]["MidInitial"]);
                sqlcmd.Parameters.AddWithValue("@Suffix", cDT.Rows[cI]["Suffix"]);
                sqlcmd.Parameters.AddWithValue("@Holiday", cDT.Rows[cI]["HolidayList"]);
                if (cMode == 1)
                    sqlcmd.Parameters.AddWithValue("@Active", 1);
                else
                    sqlcmd.Parameters.AddWithValue("@Active", cDT.Rows[cI]["Active"]);
                sqlcmd.Parameters.AddWithValue("@BDSUser", cDT.Rows[cI]["BDSUser"]);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditContact";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nSuccess = 1;
                }
                catch
                {
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            return nSuccess;
        }

        private static int SaveSpAddress(int cID, int cSpID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            if (cDT.Rows[cI]["StAddr"].ToString().Trim() != "")
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    return 0;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", cMode);
                sqlcmd.Parameters.AddWithValue("@AddressID", cID);
                sqlcmd.Parameters.AddWithValue("@SpID", cSpID);
                sqlcmd.Parameters.AddWithValue("@Street", cDT.Rows[cI]["StAddr"]);
                sqlcmd.Parameters.AddWithValue("@City", cDT.Rows[cI]["CityAddr"]);
                sqlcmd.Parameters.AddWithValue("@State", cDT.Rows[cI]["StateCode"]);
                sqlcmd.Parameters.AddWithValue("@Country", cDT.Rows[cI]["Country"]);
                sqlcmd.Parameters.AddWithValue("@ZipCode", cDT.Rows[cI]["ZipCode"]);
                sqlcmd.Parameters.AddWithValue("@Billing", cDT.Rows[cI]["Billing"]);
                sqlcmd.Parameters.AddWithValue("@Reports", cDT.Rows[cI]["Reports"]);
                sqlcmd.Parameters.AddWithValue("@APContact", cDT.Rows[cI]["APContact"]);
                sqlcmd.Parameters.AddWithValue("@Notes", cDT.Rows[cI]["Notes"]);
                if (cMode == 1)
                    sqlcmd.Parameters.AddWithValue("@Active", 1);
                else
                    sqlcmd.Parameters.AddWithValue("@Active", cDT.Rows[cI]["Active"]);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditSpAddress";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nSuccess = 1;
                }
                catch
                {
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            return nSuccess;
        }

        private static int SaveConAddress(int cID, int cCID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            if (cID != 0)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    return 0;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", cMode);
                sqlcmd.Parameters.AddWithValue("@AddressID", cID);
                sqlcmd.Parameters.AddWithValue("@ConID", cCID);
                sqlcmd.Parameters.AddWithValue("@Billing", cDT.Rows[cI]["Billing"]);
                sqlcmd.Parameters.AddWithValue("@Reports", cDT.Rows[cI]["Reports"]);
                sqlcmd.Parameters.AddWithValue("@Mailing", cDT.Rows[cI]["Mailing"]);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditConAddress";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nSuccess = 1;
                }
                catch
                {
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            return nSuccess;
        }

        private static int SaveConNumber(int cID, int cCID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            if (cDT.Rows[cI]["ContactNo"].ToString().Trim() != "")
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    return 0;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", cMode);
                sqlcmd.Parameters.AddWithValue("@PhoneID", cID);
                sqlcmd.Parameters.AddWithValue("@ConID", cCID);
                sqlcmd.Parameters.AddWithValue("@ContactNo", cDT.Rows[cI]["ContactNo"]);
                sqlcmd.Parameters.AddWithValue("@ExtNo", cDT.Rows[cI]["ExtNo"]);
                sqlcmd.Parameters.AddWithValue("@TelNo", cDT.Rows[cI]["ATelNo"]);
                sqlcmd.Parameters.AddWithValue("@FAXNo", cDT.Rows[cI]["AFAXNo"]);
                sqlcmd.Parameters.AddWithValue("@CPNo", cDT.Rows[cI]["ACellNo"]);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditContactNo";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nSuccess = 1;
                }
                catch
                {
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            return nSuccess;
        }

        private static int SaveConEMail(int cID, int cCID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            if (cDT.Rows[cI]["EMailAddress"].ToString().Trim() != "")
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    return 0;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", cMode);
                sqlcmd.Parameters.AddWithValue("@EMailID", cID);
                sqlcmd.Parameters.AddWithValue("@ConID", cCID);
                sqlcmd.Parameters.AddWithValue("@EMailAddress", cDT.Rows[cI]["EMailAddress"]);
                sqlcmd.Parameters.AddWithValue("@AckRpt", cDT.Rows[cI]["AckReports"]);
                sqlcmd.Parameters.AddWithValue("@SpeedRpt", cDT.Rows[cI]["SpeedReports"]);
                sqlcmd.Parameters.AddWithValue("@FinalRpt", cDT.Rows[cI]["FinalReports"]);
                sqlcmd.Parameters.AddWithValue("@BDSRpt", cDT.Rows[cI]["BDSReports"]);
                sqlcmd.Parameters.AddWithValue("@BDSInv", cDT.Rows[cI]["BDSInvoices"]);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditContactEMail";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nSuccess = 1;
                }
                catch
                {
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            return nSuccess;
        }

        private static int SaveSpForecast(int cID, int cCID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            if (Convert.ToDecimal(cDT.Rows[cI]["ForecastAmount"]) != 0)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    return 0;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", cMode);
                sqlcmd.Parameters.AddWithValue("@SpID", cCID);
                sqlcmd.Parameters.AddWithValue("@Yr", cDT.Rows[cI]["ForecastYear"]);
                sqlcmd.Parameters.AddWithValue("@Amt", cDT.Rows[cI]["ForecastAmount"]);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditSpForecast";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nSuccess = 1;
                }
                catch
                {
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            return nSuccess;
        }

        private static int DelSpForecast(int cID, int cCID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@SpID", cCID);
            sqlcmd.Parameters.AddWithValue("@Yr", Convert.ToInt16(cDT.Rows[cI]["ForecastYear", DataRowVersion.Original]));
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelSpForecast";
            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch {}
            return nSuccess;
        }

        private static int SaveSpCourier(int cID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            if (cDT.Rows[cI]["AccountNo"].ToString().Trim() != "")
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    return 0;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", cMode);
                sqlcmd.Parameters.AddWithValue("@SpID", cID);
                sqlcmd.Parameters.AddWithValue("@CourierCode", cDT.Rows[cI]["CourierCode"]);
                sqlcmd.Parameters.AddWithValue("@AcctNo", cDT.Rows[cI]["AccountNo"]);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditSpCourier";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nSuccess = 1;
                }
                catch
                {
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            return nSuccess;
        }

        private static int SaveSpActivity(int cID, int cCID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            if (cDT.Rows[cI]["ActivityNotes"].ToString().Trim() != "")
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    return 0;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", cMode);
                sqlcmd.Parameters.AddWithValue("@SpID", cCID);
                sqlcmd.Parameters.AddWithValue("@ActYr", cDT.Rows[cI]["ActivityYear"]);
                sqlcmd.Parameters.AddWithValue("@ActNotes", cDT.Rows[cI]["ActivityNotes"]);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditSpActivity";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nSuccess = 1;
                }
                catch
                {
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            return nSuccess;
        }

        private void CancelSave()
        {
            if (nQuoteSw == 1)
            {
                nQuoteSw = 0;
                SendKeys.Send("{F12}");
                return;
            }
            int nSD = nMode;
            if (nMode != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            OpenControls(pnlRecord, false);
            OpenControls(pnlAP, false); OpenControls(pnlCRApp, false);
            AddEditMode(false); 
            FileAccess();
            nMode = 0;
            if (nSD == 2)
            {
                LoadData();
            }
            else
            {
                LoadRecords(1);
                pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront();
                bnFile.Enabled = true;
            }
        }

        private void Sponsors_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Sponsors");

            LoadSizesDDL();
            LoadIndustriesDDL();
            LoadStatesDDL();
            LoadCouriersDDL();

            LoadRecords(0);
            BuildPrintItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
            chkShowInactive.Visible = true;
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            try
            {
                dgvFile.Rows[0].Selected = true; dgvFile.Select();
            }
            catch { }

            nTRec = bsFile.Count;
            //Main Data
            dtMain.Columns.Add("SponsorID", typeof(string));
            dtMain.Columns.Add("SponsorName", typeof(string));
            dtMain.Columns.Add("DateCreated", typeof(DateTime));
            dtMain.Columns.Add("DatePermanent", typeof(DateTime));
            dtMain.Columns.Add("SizeID", typeof(Int16));
            dtMain.Columns.Add("IndustryID", typeof(Int16));
            dtMain.Columns.Add("MAgreement", typeof(bool));
            dtMain.Columns.Add("QAgreement", typeof(bool));
            dtMain.Columns.Add("CAgreement", typeof(bool));
            dtMain.Columns.Add("SponsorNotes", typeof(string));
            dtMain.Columns.Add("FAXReport", typeof(bool));
            dtMain.Columns.Add("PrintSpeedReport", typeof(bool));
            dtMain.Columns.Add("SendSpeedReport", typeof(bool));
            dtMain.Columns.Add("CRStatusCode", typeof(Int16));
            dtMain.Columns.Add("ChargeType", typeof(Int16));
            dtMain.Columns.Add("APContact", typeof(string));
            dtMain.Columns.Add("APTelNo", typeof(string));
            dtMain.Columns.Add("APFAXNo", typeof(string));
            dtMain.Columns.Add("APEMail", typeof(string));
            dtMain.Columns.Add("BDSEMail", typeof(string));
            dtMain.Columns.Add("BDSUser", typeof(bool));
            dtMain.Columns.Add("BDSInvoice", typeof(bool));
            dtMain.Columns.Add("CRAMailDate", typeof(DateTime));
            dtMain.Columns.Add("CRAReturnDate", typeof(DateTime));
            dtMain.Columns.Add("CreditLimit", typeof(decimal));
            bsMain.DataSource = dtMain;
            //Sponsor Addresses
            dtSpAddresses.Columns.Add("AddressID", typeof(string));
            dtSpAddresses.Columns.Add("StAddr", typeof(string));
            dtSpAddresses.Columns.Add("CityAddr", typeof(string));
            dtSpAddresses.Columns.Add("StateCode", typeof(string));
            dtSpAddresses.Columns.Add("ZIPCode", typeof(string));
            dtSpAddresses.Columns.Add("Country", typeof(string));
            dtSpAddresses.Columns.Add("APContact", typeof(string));
            dtSpAddresses.Columns.Add("Notes", typeof(string));
            dtSpAddresses.Columns.Add("Billing", typeof(bool));
            dtSpAddresses.Columns.Add("Reports", typeof(bool));
            dtSpAddresses.Columns.Add("Active", typeof(bool));
            bsSpAddresses.DataSource = dtSpAddresses;
            //Contacts
            dtContacts.Columns.Add("ContactID", typeof(string));
            dtContacts.Columns.Add("Title", typeof(string));
            dtContacts.Columns.Add("LastName", typeof(string));
            dtContacts.Columns.Add("FirstName", typeof(string));
            dtContacts.Columns.Add("MidInitial", typeof(string));
            dtContacts.Columns.Add("Suffix", typeof(string));
            dtContacts.Columns.Add("Active", typeof(bool));
            dtContacts.Columns.Add("HolidayList", typeof(bool));
            dtContacts.Columns.Add("BDSUser", typeof(bool));
            bsContacts.DataSource = dtContacts;
            bnContacts.BindingSource = bsContacts;
            //Contact Addresses
            dtConAddresses.Columns.Add("ConID", typeof(Int16));
            dtConAddresses.Columns.Add("ConAddressID", typeof(string));
            dtConAddresses.Columns.Add("ConAddress", typeof(string));
            dtConAddresses.Columns.Add("Billing", typeof(bool));
            dtConAddresses.Columns.Add("Reports", typeof(bool));
            dtConAddresses.Columns.Add("Mailing", typeof(bool));
            dtConAddresses.Columns.Add("ConAddrID", typeof(string));
            bsConAddresses.DataSource = dtConAddresses;
            bnConAddresses.BindingSource = bsConAddresses;
            //Contact Numbers
            dtConNumbers.Columns.Add("ConID", typeof(Int16));
            dtConNumbers.Columns.Add("PhoneID", typeof(string));
            dtConNumbers.Columns.Add("ContactNo", typeof(string));
            dtConNumbers.Columns.Add("ExtNo", typeof(string));
            dtConNumbers.Columns.Add("ATelNo", typeof(bool));
            dtConNumbers.Columns.Add("AFAXNo", typeof(bool));
            dtConNumbers.Columns.Add("ACellNo", typeof(bool));
            bsConNumbers.DataSource = dtConNumbers;
            bnConNumbers.BindingSource = bsConNumbers;
            //Contact EMail Addresses
            dtConEMails.Columns.Add("ConID", typeof(Int16));
            dtConEMails.Columns.Add("EMailID", typeof(string));
            dtConEMails.Columns.Add("EMailAddress", typeof(string));
            dtConEMails.Columns.Add("AckReports", typeof(bool));
            dtConEMails.Columns.Add("SpeedReports", typeof(bool));
            dtConEMails.Columns.Add("FinalReports", typeof(bool));
            dtConEMails.Columns.Add("BDSReports", typeof(bool));
            dtConEMails.Columns.Add("BDSInvoices", typeof(bool));
            bsConEMails.DataSource = dtConEMails;
            bnConEMails.BindingSource = bsConEMails;
            //Sponsors' Sales Forecast
            dtSpForecasts.Columns.Add("ForecastYear", typeof(Int16));
            dtSpForecasts.Columns.Add("ForecastAmount", typeof(decimal));
            bsSpForecasts.DataSource = dtSpForecasts;
            bnSpForecasts.BindingSource = bsSpForecasts;
            //Sponsor's Couriers
            dtSpCouriers.Columns.Add("CourierCode", typeof(string));
            dtSpCouriers.Columns.Add("AccountNo", typeof(string));
            bsSpCouriers.DataSource = dtSpCouriers;
            bnSpCouriers.BindingSource = bsSpCouriers;
            //Sponsor's Activities
            dtSpActivities.Columns.Add("ActivityYear", typeof(Int16));
            dtSpActivities.Columns.Add("ActivityNotes", typeof(string));
            bsSpActivities.DataSource = dtSpActivities;
            bnSpActivities.BindingSource = bsSpActivities;
            if (nQuoteSw == 1)
            {
                PSSClass.General.FindRecord("SponsorID", nSpID.ToString(), bsFile, dgvFile);
                LoadData(); 
            }
        }

        private void cklSelIdxChEventHandler(object sender, EventArgs e)
        {
            string strCol = cklColumns.Items[cklColumns.SelectedIndex].ToString().Replace(" ", "");
            if (dgvFile.Columns[strCol].Visible == true)
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = false;
            else
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = true;
            cklColumns.Visible = false;
        }

        private void CloseClickHandler(object sender, EventArgs e)
        {
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

        private void chkShowInactiveClickHandler(object sender, EventArgs e)
        {
            if (chkShowInactive.Checked)
            {
                LoadRecords(1);
            }
            else
            {
                LoadRecords(0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
                SaveRecord();

            PSSClass.General.FindRecord("SponsorID", txtID.Text, bsFile, dgvFile);
            
            if (nTRec != bsFile.Count)
                tsbRefresh.Enabled = true;

            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
            AddEditMode(false);
            FileAccess();

            if (nQuoteSw == 1)
            {
                nQuoteSw = 0;
                SendKeys.Send("{F12}");
                return;
            }
        }

        private void LoadAddresses(int cSpID)
        {
            //Sponsor Addresses
            dtSpAddresses = PSSClass.Sponsors.SponsorAddresses(cSpID);
            if (dtSpAddresses == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            try
            {
                bsSpAddresses.DataSource = dtSpAddresses;
            }
            catch { }
            bnSpAddresses.BindingSource = bsSpAddresses;
            dtrSpAddresses.DataSource = bsSpAddresses;
        }

        private void LoadForecasts()
        {
            if (nMode != 1)
            {
                dtSpForecasts = PSSClass.Sponsors.SponsorForecasts(Convert.ToInt16(txtID.Text));
                if (dtSpForecasts == null)
                {
                    return;
                }
                bsSpForecasts.DataSource = dtSpForecasts;
                bnSpForecasts.BindingSource = bsSpForecasts;
                dtrSpForecasts.DataSource = bsSpForecasts;
            }
        }

        private void LoadCouriers()
        {
            if (nMode != 1)
            {
                dtSpCouriers = PSSClass.Sponsors.SponsorCouriers(Convert.ToInt16(txtID.Text));
                if (dtSpCouriers == null)
                {
                    return;
                }
                try
                {
                    bsSpCouriers.DataSource = dtSpCouriers;
                }
                catch { }
                bnSpCouriers.BindingSource = bsSpCouriers;
                dtrSpCouriers.DataSource = bsSpCouriers;
            }
        }

        private void LoadActivities()
        {
            if (nMode != 1)
            {
                dtSpActivities = PSSClass.Sponsors.SponsorActivities(Convert.ToInt16(txtID.Text));
                if (dtSpActivities == null)
                {
                    return;
                }
                bsSpActivities.DataSource = dtSpActivities;
                bnSpActivities.BindingSource = bsSpActivities;
                dtrSpActivities.DataSource = bsSpActivities;
            }
        }

        private void LoadContacts()
        {
            dtContacts = PSSClass.Contacts.ContactNames(Convert.ToInt16(txtID.Text));
            if (dtContacts == null)
                return;
            bsContacts.DataSource = dtContacts;
            bnContacts.BindingSource = bsContacts;
            dtrContacts.DataSource = bsContacts;
        }

        private void tbcSubData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nMode == 2)
            {
                byte nSw = 0;
                DataTable dt = new DataTable();

                if (nTab == 1)
                {
                    bsSpAddresses.EndEdit();
                    dt = dtSpAddresses.GetChanges(DataRowState.Modified);
                    if (dt != null)
                        nSw = 1;
                    else
                    {
                        dt = dtSpAddresses.GetChanges(DataRowState.Added);
                        if (dt != null)
                            nSw = 1;
                    }
                    if (nSw == 1)
                    {
                        UpdateSpAddresses(2);
                    }
                    nSw = 0;
                    bsConAddresses.EndEdit();
                    dt = new DataTable();
                    dt = dtConAddresses.GetChanges(DataRowState.Modified);
                    if (dt != null)
                        nSw = 1;
                    else
                    {
                        dt = dtConAddresses.GetChanges(DataRowState.Added);
                        if (dt != null)
                            nSw = 1;
                    }
                    if (nSw == 1)
                    {
                        UpdateConAddresses(2);
                    }
                }
                else if (nTab == 2)
                {
                    bsConNumbers.EndEdit();
                    dt = new DataTable();
                    dt = dtConNumbers.GetChanges(DataRowState.Modified);
                    if (dt != null)
                        nSw = 1;
                    else
                    {
                        dt = dtConNumbers.GetChanges(DataRowState.Added);
                        if (dt != null)
                            nSw = 1;
                    }
                    if (nSw == 1)
                    {
                        UpdateContactNos(2);
                    }
                    nSw = 0;
                    bsConEMails.EndEdit();
                    dt = new DataTable();
                    dt = dtConEMails.GetChanges(DataRowState.Modified);
                    if (dt != null)
                        nSw = 1;
                    else
                    {
                        dt = dtConEMails.GetChanges(DataRowState.Added);
                        if (dt != null)
                            nSw = 1;
                    }
                    if (nSw == 1)
                    {
                        UpdateContactEMails(2);
                    }
                }
                else if (nTab == 3)
                {
                    bsSpForecasts.EndEdit();
                    dt = new DataTable();
                    dt = dtSpForecasts.GetChanges(DataRowState.Modified);
                    if (dt != null)
                        nSw = 1;
                    else
                    {
                        dt = dtSpForecasts.GetChanges(DataRowState.Added);
                        if (dt != null)
                            nSw = 1;
                    }
                    if (nSw == 1)
                    {
                        UpdateSpForecasts(2);
                    }
                }
                else if (nTab == 4)
                {
                    bsSpCouriers.EndEdit();
                    dt = new DataTable();
                    dt = dtSpCouriers.GetChanges(DataRowState.Modified);
                    if (dt != null)
                        nSw = 1;
                    else
                    {
                        dt = dtSpCouriers.GetChanges(DataRowState.Added);
                        if (dt != null)
                            nSw = 1;
                    }
                    if (nSw == 1)
                    {
                        UpdateSpCouriers(2);
                    }
                }
                else if (nTab == 5)
                {
                    bsSpActivities.EndEdit();
                    dt = new DataTable();
                    dt = dtSpActivities.GetChanges(DataRowState.Modified);
                    if (dt != null)
                        nSw = 1;
                    else
                    {
                        dt = dtSpActivities.GetChanges(DataRowState.Added);
                        if (dt != null)
                            nSw = 1;
                    }
                    if (nSw == 1)
                    {
                        UpdateSpActivities(2);
                    }
                }
            }
            if (tbcSubData.SelectedIndex == 1)
            {
                if (nMode != 1)
                    LoadAddresses(Convert.ToInt16(txtID.Text));
                LoadConAddresses();
            }
            else if (tbcSubData.SelectedIndex == 2)
            {
                LoadConNumbers();
                LoadConEMails();
            }
            else if (tbcSubData.SelectedIndex == 3)
            {
                LoadForecasts();
            }
            else if (tbcSubData.SelectedIndex == 4)
            {
                LoadCouriers();
            }
            else if (tbcSubData.SelectedIndex == 5)
            {
                LoadActivities();
            }
        }

        private void pnlRecord_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
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

        private void AddEditSpAddress(bool pStatus)
        {
            btnAddSpAddr.Enabled = !pStatus; btnEditSpAddr.Enabled = !pStatus;
            btnCancelSpAddr.Enabled = pStatus;
        }

        private void LoadConAddresses()
        {
            try
            {
                //Contact Addresses
                if (((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text != "(New)")
                    dtConAddresses = PSSClass.Contacts.ContactAddresses(Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text));
                else
                {
                    dtConAddresses = PSSClass.Contacts.ContactAddresses(0);
                }
                bsConAddresses.DataSource = dtConAddresses;
                bnConAddresses.BindingSource = bsConAddresses;
                dtrConAddresses.DataSource = bsConAddresses;
            }
            catch { }
        }

        private void AddEditConEMail(bool pStatus)
        {
            btnAddConEMail.Enabled = !pStatus; btnEditConEMail.Enabled = !pStatus;
            btnSaveConEMail.Enabled = pStatus; btnCancelConEMail.Enabled = pStatus;
        }

        private void btnSaveSpAddress_Click(object sender, EventArgs e)
        {
            if (nMode == 1)
                txtSpAddressID.Text = PSSClass.General.NewID("SponsorAddresses", "AddressID").ToString();

            if (txtSpStreet.Text.Trim() == "")
            {
                MessageBox.Show("Please enter street address.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSpStreet.Focus();
                return;
            }

            if (txtSpCity.Text.Trim() == "")
            {
                MessageBox.Show("Please enter city address.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSpCity.Focus();
                return;
            }

            if (txtSpZipCode.Text.Trim() == "")
            {
                MessageBox.Show("Please enter zip code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSpZipCode.Focus();
                return;
            }

            if (txtSpAP.Text.Trim() == "")
            {
                MessageBox.Show("Please enter A/P contact.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSpAP.Focus();
                return;
            }

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@SponsorID", SqlDbType.Int));
            sqlcmd.Parameters["@SponsorID"].Value = Convert.ToInt16(txtID.Text);

            sqlcmd.Parameters.Add(new SqlParameter("@AddressID", SqlDbType.Int));
            sqlcmd.Parameters["@AddressID"].Value = Convert.ToInt16(txtSpAddressID.Text);

            sqlcmd.Parameters.Add(new SqlParameter("@Street", SqlDbType.NVarChar));
            sqlcmd.Parameters["@Street"].Value = txtSpStreet.Text.Trim();

            sqlcmd.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar));
            sqlcmd.Parameters["@City"].Value = txtSpCity.Text.Trim();

            sqlcmd.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar));
            sqlcmd.Parameters["@State"].Value = cboStates.SelectedText;

            sqlcmd.Parameters.Add(new SqlParameter("@ZipCode", SqlDbType.NVarChar));
            sqlcmd.Parameters["@ZipCode"].Value = txtSpZipCode.Text.Trim();

            sqlcmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar));
            sqlcmd.Parameters["@Country"].Value = txtSpCountry.Text.Trim();

            sqlcmd.Parameters.Add(new SqlParameter("@APContact", SqlDbType.NVarChar));
            sqlcmd.Parameters["@APContact"].Value = txtSpAP.Text.Trim();

            sqlcmd.Parameters.Add(new SqlParameter("@Billing", SqlDbType.Bit));
            sqlcmd.Parameters["@Billing"].Value = chkBillingAddr.CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@Reports", SqlDbType.Bit));
            sqlcmd.Parameters["@Reports"].Value = chkReportsAddr.CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@Notes", SqlDbType.NVarChar));
            sqlcmd.Parameters["@Notes"].Value = txtSpNotes.Text.Trim();

            sqlcmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit));
            sqlcmd.Parameters["@Active"].Value = chkActiveAddr.CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID;

            sqlcmd.CommandType = CommandType.StoredProcedure;
            if (nMode == 1)
            {
                sqlcmd.CommandText = "spAddSpAddress";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else if (nMode == 2)
            {
                sqlcmd.CommandText = "spEditSpAddress";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            AddEditSpAddress(false);
            LoadAddresses(Convert.ToInt16(txtID.Text));
        }

        private void txtSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }


        private void dtrSpAddresses_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            if (e.ItemIndex < dtSpAddresses.Rows.Count)
            {
                switch (e.Control.Name)
                {
                    case "txtSpAddressID":
                        try
                        {
                            e.Value = dtSpAddresses.Rows[e.ItemIndex]["AddressID"].ToString();
                            e.Control.BackColor = Color.GhostWhite;
                        }
                        catch { }
                        break;
                    case "txtSpStreet":
                        try
                        {
                            e.Value = dtSpAddresses.Rows[e.ItemIndex]["StAddr"].ToString();
                        }
                        catch { }
                        break;
                    case "txtSpCity":
                        try
                        {
                            e.Value = dtSpAddresses.Rows[e.ItemIndex]["CityAddr"].ToString();
                        }
                        catch { }
                        break;
                    case "txtState":
                        try
                        {
                            e.Value = dtSpAddresses.Rows[e.ItemIndex]["StateCode"].ToString();
                        }
                        catch { }
                        break;
                    case "txtSpZipCode":
                        try
                        {
                            e.Value = dtSpAddresses.Rows[e.ItemIndex]["ZipCode"].ToString();
                        }
                        catch { }
                        break;
                    case "txtSpCountry":
                        try
                        {
                            e.Value = dtSpAddresses.Rows[e.ItemIndex]["Country"].ToString();
                        }
                        catch { }
                        break;
                    case "txtSpAP":
                        try
                        {
                            e.Value = dtSpAddresses.Rows[e.ItemIndex]["APContact"].ToString();
                        }
                        catch { }
                        break;
                    case "txtSpNotes":
                        try
                        {
                            e.Value = dtSpAddresses.Rows[e.ItemIndex]["Notes"].ToString();
                        }
                        catch { }
                        break;
                }
            }
        }

        private void dtrSpAddresses_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            try
            {
                var combo = (ComboBox)e.DataRepeaterItem.Controls.Find("cboStates", false)[0];
                combo.SelectedValue = dtSpAddresses.Rows[e.DataRepeaterItem.ItemIndex]["StateCode"].ToString();
                combo.Text = combo.GetItemText(combo.SelectedItem);
            }
            catch { }
            try
            {
                var chk = (CheckBox)e.DataRepeaterItem.Controls.Find("chkBillingAddr", false)[0];
                chk.Checked = Convert.ToBoolean(dtSpAddresses.Rows[e.DataRepeaterItem.ItemIndex]["Billing"]);
            }
            catch { }
            try
            {
                var chkReportAddr = (CheckBox)e.DataRepeaterItem.Controls.Find("chkReportsAddr", false)[0];
                chkReportAddr.Checked = Convert.ToBoolean(dtSpAddresses.Rows[e.DataRepeaterItem.ItemIndex]["Reports"]);
            }
            catch { }
            try
            {
                var chkActive = (CheckBox)e.DataRepeaterItem.Controls.Find("chkActiveAddr", false)[0];
                chkActive.Checked = Convert.ToBoolean(dtSpAddresses.Rows[e.DataRepeaterItem.ItemIndex]["Active"]);
            }
            catch { }
        }

        private void dtrContacts_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            if (e.ItemIndex < dtContacts.Rows.Count)
            {
                switch (e.Control.Name)
                {
                    case "txtContactID":
                        try
                        {
                            e.Value = dtContacts.Rows[e.ItemIndex]["ContactID"];
                        }
                        catch { }
                        break;
                    case "txtContactTitle":
                        try
                        {
                            e.Value = dtContacts.Rows[e.ItemIndex]["Title"];
                        }
                        catch { }
                        break;
                    case "txtContactLastName":
                        try
                        {
                            e.Value = dtContacts.Rows[e.ItemIndex]["LastName"];
                        }
                        catch { }
                        break;
                    case "txtContactFirstName":
                        try
                        {
                            e.Value = dtContacts.Rows[e.ItemIndex]["FirstName"];
                        }
                        catch { }
                        break;
                    case "txtContactMI":
                        try
                        {
                            e.Value = dtContacts.Rows[e.ItemIndex]["MidInitial"];
                        }
                        catch { }
                        break;
                    case "txtContactSuffix":
                        try
                        {
                            e.Value = dtContacts.Rows[e.ItemIndex]["Suffix"];
                        }
                        catch { }
                        break;
                }
            }
        }
        
        private void dtrContacts_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            try
            {
                var chk = (CheckBox)e.DataRepeaterItem.Controls.Find("chkContactActive", false)[0];
                chk.Checked = Convert.ToBoolean(dtContacts.Rows[e.DataRepeaterItem.ItemIndex]["Active"]);
            }
            catch { }
            try
            {
                var chkHoliday = (CheckBox)e.DataRepeaterItem.Controls.Find("chkHoliday", false)[0];
                chkHoliday.Checked = Convert.ToBoolean(dtContacts.Rows[e.DataRepeaterItem.ItemIndex]["HolidayList"]);
            }
            catch { }
            try
            {
                var chkBDS = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConBDSUser", false)[0];
                chkBDS.Checked = Convert.ToBoolean(dtContacts.Rows[e.DataRepeaterItem.ItemIndex]["BDSUser"]);
            }
            catch { }

            try
            {
                var txtCTitle = (TextBox)e.DataRepeaterItem.Controls.Find("txtContactTitle", false)[0];
                var txtCID = (TextBox)e.DataRepeaterItem.Controls.Find("txtContactID", false)[0];
                var txtLN = (TextBox)e.DataRepeaterItem.Controls.Find("txtContactLastName", false)[0];
                var txtFN = (TextBox)e.DataRepeaterItem.Controls.Find("txtContactFirstName", false)[0];
                var txtSuffix = (TextBox)e.DataRepeaterItem.Controls.Find("txtContactSuffix", false)[0];
                var txtMI = (TextBox)e.DataRepeaterItem.Controls.Find("txtContactMI", false)[0];
                if (txtCID.Text == nConID.ToString())
                {
                    txtCTitle.BackColor = Color.LightGreen; txtCTitle.ForeColor = Color.Black;
                    txtCID.BackColor = Color.LightGreen; txtCID.ForeColor = Color.Black;
                    txtLN.BackColor = Color.LightGreen; txtLN.ForeColor = Color.Black;
                    txtFN.BackColor = Color.LightGreen; txtFN.ForeColor = Color.Black;
                    txtSuffix.BackColor = Color.LightGreen; txtSuffix.ForeColor = Color.Black;
                    txtMI.BackColor = Color.LightGreen; txtMI.ForeColor = Color.Black;
                    txtCID.Focus();
                }
                else
                {
                    txtCTitle.BackColor = Color.White; txtCTitle.ForeColor = Color.Black;
                    txtCID.BackColor = Color.White; txtCID.ForeColor = Color.Black;
                    txtLN.BackColor = Color.White; txtLN.ForeColor = Color.Black;
                    txtFN.BackColor = Color.White; txtFN.ForeColor = Color.Black;
                    txtSuffix.BackColor = Color.White; txtSuffix.ForeColor = Color.Black;
                    txtMI.BackColor = Color.White; txtMI.ForeColor = Color.Black;
                }
            }
            catch { }
        }

        private void dtrContacts_CurrentItemIndexChanged(object sender, EventArgs e)
        {
            if (nMode == 2)
            {
                byte nSw = 0;
                btnAddContact.Focus();
                bsConAddresses.EndEdit();
                DataTable dt = new DataTable();
                dt = dtConAddresses.GetChanges(DataRowState.Modified);
                if (dt != null)
                    nSw = 1;
                else
                {
                    dt = dtConAddresses.GetChanges(DataRowState.Added);
                    if (dt != null)
                        nSw = 1;
                }
                if (nSw == 1)
                {
                    UpdateConAddresses(2);
                }
                nSw = 0;
                btnAddConNo.Focus();
                bsConNumbers.EndEdit();
                dt = new DataTable();
                dt = dtConNumbers.GetChanges(DataRowState.Modified);
                if (dt != null)
                    nSw = 1;
                else
                {
                    dt = dtConNumbers.GetChanges(DataRowState.Added);
                    if (dt != null)
                        nSw = 1;
                }
                if (nSw == 1)
                {
                    UpdateContactNos(2);
                }
                nSw = 0;
                btnAddConEMail.Focus();
                bsConEMails.EndEdit();
                dt = new DataTable();
                dt = dtConEMails.GetChanges(DataRowState.Modified);
                if (dt != null)
                    nSw = 1;
                else
                {
                    dt = dtConEMails.GetChanges(DataRowState.Added);
                    if (dt != null)
                        nSw = 1;
                }
                if (nSw == 1)
                {
                    UpdateContactEMails(2);
                }
            }
            if (tbcSubData.SelectedIndex == 1)
            {
                LoadConAddresses();
            }
            else if (tbcSubData.SelectedIndex == 2)
            {
                LoadConNumbers();
                LoadConEMails();
            }

            if (dtContacts.Rows.Count > 0)
            {
                var txtCTitle = (TextBox)dtrContacts.CurrentItem.Controls.Find("txtContactTitle", false)[0];
                var txtCID = (TextBox)dtrContacts.CurrentItem.Controls.Find("txtContactID", false)[0];
                var txtLN = (TextBox)dtrContacts.CurrentItem.Controls.Find("txtContactLastName", false)[0];
                var txtFN = (TextBox)dtrContacts.CurrentItem.Controls.Find("txtContactFirstName", false)[0];
                var txtSuffix = (TextBox)dtrContacts.CurrentItem.Controls.Find("txtContactSuffix", false)[0];
                var txtMI = (TextBox)dtrContacts.CurrentItem.Controls.Find("txtContactMI", false)[0];

                if (dtrContacts.CurrentItem.BackColor == Color.LightSteelBlue)
                    try
                    {
                        txtCTitle.BackColor = Color.White; txtCTitle.ForeColor = Color.Black;
                        txtCID.BackColor = Color.White; txtCID.ForeColor = Color.Black;
                        txtLN.BackColor = Color.White; txtLN.ForeColor = Color.Black;
                        txtFN.BackColor = Color.White; txtFN.ForeColor = Color.Black;
                        txtSuffix.BackColor = Color.White; txtSuffix.ForeColor = Color.Black;
                        txtMI.BackColor = Color.White; txtMI.ForeColor = Color.Black;
                    }
                    catch { }
                else
                {
                    try
                    {
                        txtCTitle.BackColor = Color.LightSteelBlue; txtCTitle.ForeColor = Color.Black;
                        txtCID.BackColor = Color.LightSteelBlue; txtCID.ForeColor = Color.Black;
                        txtLN.BackColor = Color.LightSteelBlue; txtLN.ForeColor = Color.Black;
                        txtFN.BackColor = Color.LightSteelBlue; txtFN.ForeColor = Color.Black;
                        txtSuffix.BackColor = Color.LightSteelBlue; txtSuffix.ForeColor = Color.Black;
                        txtMI.BackColor = Color.LightSteelBlue; txtMI.ForeColor = Color.Black;
                    }
                    catch { }
                }
            }
        }

        private void btnDelContact_Click(object sender, EventArgs e)
        {
            if (dtContacts.Rows.Count == 0)
                return;

            if (dtContacts.Rows[bsContacts.Position].RowState.ToString() != "Added")
            {
                try
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("WARNING: Deleting this Contact record would also " + Environment.NewLine + "delete other data associated with this Contact." + Environment.NewLine +
                                             "Do you want to delete " + ((TextBox)dtrContacts.CurrentItem.Controls["txtContactLastName"]).Text.Trim().ToUpper() + ", " + ((TextBox)dtrContacts.CurrentItem.Controls["txtContactFirstName"]).Text.Trim().ToUpper() +
                                             "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dReply == DialogResult.Yes)
                    {
                        bsContacts.EndEdit();
                        UpdateContacts(2);

                        int nCID = Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text.Trim());

                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        if (sqlcnn == null)
                        {
                            MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        ;
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        //Delete all addresses
                        sqlcmd.Parameters.AddWithValue("@ConID", nCID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelConAddresses";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                            for (int i = 0; i < dtConAddresses.Rows.Count; i++)
                            {
                                dtConAddresses.Rows[i].Delete();
                            }
                            dtConAddresses.AcceptChanges();
                        }
                        catch (Exception ex)
                        {
                            sqlcmd.Dispose();
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //Delete all contact numbers
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@ConID", nCID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelConNos";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                            for (int i = 0; i < dtConNumbers.Rows.Count; i++)
                            {
                                dtConNumbers.Rows[i].Delete();
                            }
                            dtConNumbers.AcceptChanges();
                        }
                        catch (Exception ex)
                        {
                            sqlcmd.Dispose(); 
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        //Delete all contact email addresses
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@ConID", nCID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelConEMails";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                            for (int i = 0; i < dtConEMails.Rows.Count; i++)
                            {
                                dtConEMails.Rows[i].Delete();
                            }
                            dtConEMails.AcceptChanges();
                        }
                        catch (Exception ex)
                        {
                            sqlcmd.Dispose(); 
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        dtConEMails.AcceptChanges();
                        //Delete main contact file
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@ConID", nCID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelContact";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                            dtContacts.Rows[bsContacts.Position].Delete();
                            dtContacts.AcceptChanges();
                        }
                        catch (Exception ex)
                        {
                            sqlcmd.Dispose();
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    }
                }
                catch { }
            }
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {
            // Set the default text.
            DataRow dr;
            dr = dtContacts.NewRow();
            dr["ContactID"] = "(New)";
            dr["Title"] = "";
            dr["LastName"] = "";
            dr["FirstName"] = "";
            dr["MidInitial"] = "";
            dr["Suffix"] = "";
            dr["Active"] = true;
            dr["HolidayList"] = false;
            dr["BDSUser"] = false;
            dtContacts.Rows.Add(dr);
            bsContacts.EndEdit();
            dtrContacts.DataSource = bsContacts;
            try
            {
                int nRows = dtContacts.Rows.Count;
                dtrContacts.CurrentItemIndex = nRows - 1;
            }
            catch { }
            dtConAddresses.Rows.Clear(); dtConNumbers.Rows.Clear(); dtConEMails.Rows.Clear();
            tbcSubData.SelectedTab = tabAddress;
            btnCancelContact.Enabled = true;
            btnAddConAddr.Enabled = false; btnAddConNo.Enabled = false; btnAddConEMail.Enabled = false; 
        }

        private void txtContactLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtContactTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtContactFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtContactMI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtContactSuffix_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void chkContactActive_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrContacts.CurrentItem.Controls["chkContactActive"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrContacts.CurrentItem.Controls["chkContactActive"]).Checked = false;
                else
                    ((CheckBox)dtrContacts.CurrentItem.Controls["chkContactActive"]).Checked = true;
            }
            dtContacts.Rows[dtrContacts.CurrentItemIndex]["Active"] = ((CheckBox)dtrContacts.CurrentItem.Controls["chkContactActive"]).CheckState;
        }

        private void txtContactID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dtrConAddresses_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            if (e.ItemIndex < dtConAddresses.Rows.Count)
            {
                switch (e.Control.Name)
                {
                    case "txtConIDAddr":
                        try
                        {
                            e.Value = dtConAddresses.Rows[e.ItemIndex]["ConID"].ToString();
                        }
                        catch { }
                        break;
                    case "txtConAddressID":
                        try
                        {
                            e.Value = dtConAddresses.Rows[e.ItemIndex]["ConAddressID"];
                        }
                        catch { }
                        break;
                    case "txtConAddress":
                        try
                        {
                            e.Value = dtConAddresses.Rows[e.ItemIndex]["ConAddress"];
                        }
                        catch { }
                        break;
                    case "txtConAddrID":
                        try
                        {
                            e.Value = dtConAddresses.Rows[e.ItemIndex]["ConAddrID"];
                        }
                        catch { }
                        break;
                }
            }
        }

        private void dtrConAddresses_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            ((TextBox)dtrConAddresses.CurrentItem.Controls["txtConAddressID"]).BackColor = Color.PaleTurquoise;
            try
            {
                var chk = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConBillAddr", false)[0];
                chk.Checked = Convert.ToBoolean(dtConAddresses.Rows[e.DataRepeaterItem.ItemIndex]["Billing"]);
            }
            catch { }
            try
            {
                var chkReportAddr = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConRptAddr", false)[0];
                chkReportAddr.Checked = Convert.ToBoolean(dtConAddresses.Rows[e.DataRepeaterItem.ItemIndex]["Reports"]);
            }
            catch { }
            try
            {
                var chkMail = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConMailAddr", false)[0];
                chkMail.Checked = Convert.ToBoolean(dtConAddresses.Rows[e.DataRepeaterItem.ItemIndex]["Mailing"]);
            }
            catch { }
        }

        private void btnDelSpAddr_Click(object sender, EventArgs e)
        {
            if (dtSpAddresses.Rows.Count == 0)
                return;

            if (dtSpAddresses.Rows[bsSpAddresses.Position].RowState.ToString() != "Added")
            {
                try
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("WARNING: Deleting this address would delete" + Environment.NewLine + "other data associated with this address." + Environment.NewLine + Environment.NewLine +
                                             "Do you want to delete this address: " + Environment.NewLine + Environment.NewLine + ((TextBox)dtrSpAddresses.CurrentItem.Controls["txtSpAddressID"]).Text.Trim().ToUpper() + " - " + ((TextBox)dtrSpAddresses.CurrentItem.Controls["txtSpStreet"]).Text.Trim().ToUpper() +
                                             ", " + ((TextBox)dtrSpAddresses.CurrentItem.Controls["txtSpCity"]).Text.Trim().ToUpper() + " " + ((TextBox)dtrSpAddresses.CurrentItem.Controls["txtSpZipCode"]).Text.Trim().ToUpper() +
                                             "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dReply == DialogResult.Yes)
                    {
                        bsSpAddresses.EndEdit();
                        UpdateSpAddresses(2);

                        int nID = Convert.ToInt16(((TextBox)dtrSpAddresses.CurrentItem.Controls["txtSpAddressID"]).Text.Trim());

                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        if (sqlcnn == null)
                        {
                            MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        ;
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        //Delete all addresses
                        sqlcmd.Parameters.AddWithValue("@AddressID", nID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelSpAddress";//also deletes matching Address ID in Contacts' Addresses table

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close();  sqlcnn.Dispose();
                        dtSpAddresses.Rows[bsSpAddresses.Position].Delete();
                        dtSpAddresses.AcceptChanges();
                        LoadAddresses(Convert.ToInt16(txtID.Text));
                    }
                }
                catch { }
            }
        }

        private void txtSpStreet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtSpCity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtSpZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtSpCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtSpAP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtSpNotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void cboStates_Click(object sender, EventArgs e)
        {
            strState = ((ComboBox)dtrSpAddresses.CurrentItem.Controls["cboStates"]).Text;
            if (nMode == 0)
                ((ComboBox)dtrSpAddresses.CurrentItem.Controls["cboStates"]).Text = strState;
        }

        private void cboStates_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void chkBillingAddr_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkBillingAddr"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkBillingAddr"]).Checked = false;
                else
                    ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkBillingAddr"]).Checked = true;
            }
            dtSpAddresses.Rows[dtrSpAddresses.CurrentItemIndex]["Billing"] = ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkBillingAddr"]).CheckState;
        }

        private void chkActiveAddr_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkActiveAddr"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkActiveAddr"]).Checked = false;
                else
                    ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkActiveAddr"]).Checked = true;
            }
            dtSpAddresses.Rows[dtrSpAddresses.CurrentItemIndex]["Active"] = ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkActiveAddr"]).CheckState;
        }

        private void dgvCellMouseClickEventHandler(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                cklColumns.Visible = true; cklColumns.BringToFront();
            }
        }


        private void dgvCellChangedHandler(object sender, EventArgs e)
        {
            try
            {
                if (nMode == 0)
                {
                    nIndex = dgvFile.CurrentCell.ColumnIndex;
                    tsddbSearch.DropDownItems[nIndex].Select();
                    tslSearchData.Text = tsddbSearch.DropDownItems[nIndex].Text;
                    tstbSearchField.Text = tsddbSearch.DropDownItems[nIndex].Name;
                }
            }
            catch { }
        }

        private void txtSpAddressID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnEditSpAddr_Click(object sender, EventArgs e)
        {
            try
            {
                ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkAddEditSA"]).Checked = true;
                ((ComboBox)dtrSpAddresses.CurrentItem.Controls["cboStates"]).Enabled = true;
                btnAddSpAddr.Enabled = false; btnEditSpAddr.Enabled = false; btnDelSpAddr.Enabled = false; btnSaveSpAddr.Enabled = true; btnCancelSpAddr.Enabled = true;
            }
            catch { }
        }

        private void chkConBillAddr_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {

                if (((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConBillAddr"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConBillAddr"]).Checked = false;
                else
                    ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConBillAddr"]).Checked = true;
            }
            dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex]["Billing"] = ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConBillAddr"]).CheckState;
        }

        private void chkConRptAddr_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConRptAddr"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConrptAddr"]).Checked = false;
                else
                    ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConRptAddr"]).Checked = true;
            }
            dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex]["Reports"] = ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConRptAddr"]).CheckState;
        }

        private void chkConMailAddr_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConMailAddr"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConMailAddr"]).Checked = false;
                else
                    ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConMailAddr"]).Checked = true;
            }
            dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex]["Mailing"] = ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConMailAddr"]).CheckState;
        }

        private void btnAddConAddr_Click(object sender, EventArgs e)
        {
            dtAddresses = PSSClass.Sponsors.SpAddressesDDL(Convert.ToInt16(txtID.Text));
            dgvSpAddresses.DataSource = dtAddresses;
            
            if (dtSpAddresses.Rows.Count == 0)
            {
                MessageBox.Show("No address found for this Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dtContacts.Rows.Count == 0)
            {
                MessageBox.Show("No Contacts found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataRow dr;
            dr = dtConAddresses.NewRow();
            try
            {
                dr["ConID"] = Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text);
                dr["ConAddrID"] = Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text);
            }
            catch
            {
                dr["ConID"] = 0;
                dr["ConAddrID"] = 0;
            }
            dr["ConAddressID"] = "(New)";
            dr["ConAddress"] = "";
            dr["Billing"] = false;
            dr["Reports"] = false;
            dr["Mailing"] = false;
            dr["ConAddrID"] = "(New)";
            dtConAddresses.Rows.Add(dr);
            bsConAddresses.DataSource = dtConAddresses;
            dtrConAddresses.DataSource = bsConAddresses;
            int nRows = dtConAddresses.Rows.Count;
            dtrConAddresses.CurrentItemIndex = nRows - 1;
            ((TextBox)dtrConAddresses.CurrentItem.Controls["txtConAddressID"]).Focus();
            txtConAddressID_Enter(null, null);
            btnCancelConAddr.Enabled = true;
        }

        private void txtConAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnSaveConAddr_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@ContactID", SqlDbType.Int));
            sqlcmd.Parameters["@ContactID"].Value = Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text);

            sqlcmd.Parameters.Add(new SqlParameter("@AddressID", SqlDbType.Int));
            sqlcmd.Parameters["@AddressID"].Value = Convert.ToInt16(((TextBox)dtrConAddresses.CurrentItem.Controls["txtConAddressID"]).Text);

            sqlcmd.Parameters.Add(new SqlParameter("@Mail", SqlDbType.Bit));
            sqlcmd.Parameters["@Mail"].Value = ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConMailAddr"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@Billing", SqlDbType.Bit));
            sqlcmd.Parameters["@Billing"].Value = ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConBillAddr"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@Report", SqlDbType.Bit));
            sqlcmd.Parameters["@Report"].Value = ((CheckBox)dtrConAddresses.CurrentItem.Controls["chkConRptAddr"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID; 

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditConAddress";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Close(); sqlcnn.Dispose();
                return;
            }
            sqlcnn.Close(); sqlcnn.Dispose();
        }

        private void btnDelConAddr_Click(object sender, EventArgs e)
        {
            if (dtConAddresses.Rows.Count == 0)
                return;

            if (dtConAddresses.Rows[bsConAddresses.Position].RowState.ToString() != "Added")
            {
                try
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("Do you want to delete this address : " + Environment.NewLine + ((TextBox)dtrConAddresses.CurrentItem.Controls["txtConAddressID"]).Text.Trim().ToUpper() + " - " + ((TextBox)dtrConAddresses.CurrentItem.Controls["txtConAddress"]).Text.Trim().ToUpper() +
                                             "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dReply == DialogResult.Yes)
                    {
                        int nCID = Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text.Trim());

                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        if (sqlcnn == null)
                        {
                            MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        ;
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        //Delete all addresses
                        sqlcmd.Parameters.AddWithValue("@ConID", nCID);
                        sqlcmd.Parameters.AddWithValue("@AddressID", Convert.ToInt16(Convert.ToInt16(((TextBox)dtrConAddresses.CurrentItem.Controls["txtConAddressID"]).Text.Trim())));

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelConAddress";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        dtConAddresses.Rows[bsConAddresses.Position].Delete();
                        dtConAddresses.AcceptChanges();
                        LoadConAddresses();
                    }
                }
                catch { }
            }
            else
                dtConAddresses.Rows[bsConAddresses.Position].Delete();
        }

        private void LoadConNumbers()
        {
            try
            {
                if (((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text != "(New)")
                    dtConNumbers = PSSClass.Contacts.ContactNumbers(Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text));
                else
                    dtConNumbers = PSSClass.Contacts.ContactNumbers(0);

                bsConNumbers.DataSource = dtConNumbers;
                bnConNumbers.BindingSource = bsConNumbers;
                dtrConNumbers.DataSource = bsConNumbers;
            }
            catch { }
        }

        private void LoadConEMails()
        {
            try
            {
                if (((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text != "(New)")
                    dtConEMails = PSSClass.Contacts.ContactEMailAddresses(Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text));
                else
                    dtConEMails = PSSClass.Contacts.ContactEMailAddresses(0);

                bsConEMails.DataSource = dtConEMails;
                bnConEMails.BindingSource = bsConEMails;
                dtrConEMails.DataSource = bsConEMails;
            }
            catch { }
        }

        private void dtrConNumbers_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            try
            {
                if (e.ItemIndex < dtConNumbers.Rows.Count)
                {
                    switch (e.Control.Name)
                    {
                        case "txtConIDNo":
                            try
                            {
                                e.Value = dtConNumbers.Rows[e.ItemIndex]["ConID"].ToString();
                            }
                            catch { }
                            break;
                        case "txtConNoID":
                            try
                            {
                                e.Value = dtConNumbers.Rows[e.ItemIndex]["PhoneID"].ToString();
                                e.Control.BackColor = Color.GhostWhite;
                            }
                            catch { }
                            break;
                        case "mskPhoneNo":
                            try
                            {
                                e.Value = dtConNumbers.Rows[e.ItemIndex]["ContactNo"].ToString();
                            }
                            catch { }
                            break;
                        case "txtExtNo":
                            try
                            {
                                e.Value = dtConNumbers.Rows[e.ItemIndex]["ExtNo"].ToString();
                            }
                            catch { }
                            break;
                    }
                }
            }
            catch { }
        }

        private void dtrConNumbers_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            ((TextBox)dtrConNumbers.CurrentItem.Controls["txtConNoID"]).BackColor = Color.PaleTurquoise;
            try
            {
                var chk = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConPhone", false)[0];
                chk.Checked = Convert.ToBoolean(dtConNumbers.Rows[e.DataRepeaterItem.ItemIndex]["ATelNo"]);
            }
            catch { }
            try
            {
                var chkFAX = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConFAX", false)[0];
                chkFAX.Checked = Convert.ToBoolean(dtConNumbers.Rows[e.DataRepeaterItem.ItemIndex]["AFAXNo"]);
            }
            catch { }
            try
            {
                var chkMobile = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConMobile", false)[0];
                chkMobile.Checked = Convert.ToBoolean(dtConNumbers.Rows[e.DataRepeaterItem.ItemIndex]["ACellNo"]);
            }
            catch { }
        }

        private void dtrConEMails_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            try
            {
                if (e.ItemIndex < dtConEMails.Rows.Count)
                {
                    switch (e.Control.Name)
                    {
                        case "txtConIDEM":
                            try
                            {
                                e.Value = dtConEMails.Rows[e.ItemIndex]["ConID"].ToString();
                            }
                            catch { }
                            break;
                        case "txtConEMailID":
                            try
                            {
                                e.Value = dtConEMails.Rows[e.ItemIndex]["EMailID"].ToString();
                                e.Control.BackColor = Color.GhostWhite;
                            }
                            catch { }
                            break;
                        case "txtConEMailAddr":
                            try
                            {
                                e.Value = dtConEMails.Rows[e.ItemIndex]["EMailAddress"].ToString();
                            }
                            catch { }
                            break;
                    }
                }
            }
            catch { }
        }

        private void dtrConEMails_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            ((TextBox)dtrConEMails.CurrentItem.Controls["txtConEMailID"]).BackColor = Color.PaleTurquoise;
            try
            {
                var chkAck = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConAck", false)[0];
                chkAck.Checked = Convert.ToBoolean(dtConEMails.Rows[e.DataRepeaterItem.ItemIndex]["AckReports"]);
            }
            catch { }
            try
            {
                var chkSRpt = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConSRpt", false)[0];
                chkSRpt.Checked = Convert.ToBoolean(dtConEMails.Rows[e.DataRepeaterItem.ItemIndex]["SpeedReports"]);
            }
            catch { }
            try
            {
                var chkFRpt = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConFRpt", false)[0];
                chkFRpt.Checked = Convert.ToBoolean(dtConEMails.Rows[e.DataRepeaterItem.ItemIndex]["FinalReports"]);
            }
            catch { }
            try
            {
                var chkBDSRpt = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConBDSRpt", false)[0];
                chkBDSRpt.Checked = Convert.ToBoolean(dtConEMails.Rows[e.DataRepeaterItem.ItemIndex]["BDSReports"]);
            }
            catch { }
            try
            {
                var chkBDSInv = (CheckBox)e.DataRepeaterItem.Controls.Find("chkConBDSInv", false)[0];
                chkBDSInv.Checked = Convert.ToBoolean(dtConEMails.Rows[e.DataRepeaterItem.ItemIndex]["BDSInvoices"]);
            }
            catch { }
        }

        private void btnAddConNo_Click(object sender, EventArgs e)
        {
            if (dtContacts.Rows.Count == 0)
            {
                MessageBox.Show("No Contacts found for this Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataRow dr;
            dr = dtConNumbers.NewRow();
            try
            {
                dr["ConID"] = Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text);
            }
            catch
            {
                dr["ConID"] = 0;
            }
            dr["PhoneID"] = "(New)";
            dr["ContactNo"] = "";
            dr["ExtNo"] = "";
            dr["ATelNo"] = false;
            dr["AFAXNo"] = false;
            dr["ACellNo"] = false;
            dtConNumbers.Rows.Add(dr);
            bsConNumbers.DataSource = dtConNumbers;
            dtrConNumbers.DataSource = bsConNumbers;
            int nRows = dtConNumbers.Rows.Count;
            dtrConNumbers.CurrentItemIndex = nRows - 1;
            btnCancelConNo.Enabled = true;
        }

        private void btnAddConEMail_Click(object sender, EventArgs e)
        {
            if (dtContacts.Rows.Count == 0)
            {
                MessageBox.Show("No Contacts found for this Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DataRow dr;
            dr = dtConEMails.NewRow();
            try
            {
                dr["ConID"] = Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text);
            }
            catch
            {
                dr["ConID"] = 0;
            }
            dr["EMailID"] = "(New)";
            dr["EMailAddress"] = "";
            dr["AckReports"] = false;
            dr["SpeedReports"] = false;
            dr["FinalReports"] = false;
            dr["BDSReports"] = false;
            dr["BDSInvoices"] = false;
            dtConEMails.Rows.Add(dr);
            bsConEMails.DataSource = dtConEMails;
            dtrConEMails.DataSource = bsConEMails;
            int nRows = dtConEMails.Rows.Count;
            dtrConEMails.CurrentItemIndex = nRows - 1;
            btnCancelConEMail.Enabled = true;
        }

        private void btnDelConNo_Click(object sender, EventArgs e)
        {
            if (dtConNumbers.Rows.Count == 0)
                return;

            if (dtConNumbers.Rows[bsConNumbers.Position].RowState.ToString() != "Added")
            {
                try
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("WARNING: Deleting this contact number would delete" + Environment.NewLine + "other data associated with this contact number." + Environment.NewLine + Environment.NewLine +
                                             "Do you want to delete this contact number: " + Environment.NewLine + Environment.NewLine + ((MaskedTextBox)dtrConNumbers.CurrentItem.Controls["mskPhoneNo"]).Text +                                              
                                             "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dReply == DialogResult.Yes)
                    {
                        bsConNumbers.EndEdit();
                        UpdateContactNos(2);

                        int nID = Convert.ToInt16(((TextBox)dtrConNumbers.CurrentItem.Controls["txtConNoID"]).Text.Trim());

                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        if (sqlcnn == null)
                        {
                            MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        ;
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@PhoneID", nID);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelContactNo";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        dtConNumbers.Rows[bsConNumbers.Position].Delete();
                        dtConNumbers.AcceptChanges();
                        LoadConNumbers();
                    }
                }
                catch { }
            }
        }

        private void txtConNoID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtConEMailID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void mskPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtExtNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void chkConPhone_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConPhone"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConPhone"]).Checked = false;
                else
                    ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConPhone"]).Checked = true;
            }
            dtConNumbers.Rows[dtrConNumbers.CurrentItemIndex]["ATelNo"] = ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConPhone"]).CheckState;
        }

        private void chkConFAX_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConFAX"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConFAX"]).Checked = false;
                else
                    ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConFAX"]).Checked = true;
            }
            dtConNumbers.Rows[dtrConNumbers.CurrentItemIndex]["AFAXNo"] = ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConFAX"]).CheckState;
        }

        private void chkConMobile_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConMobile"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConMobile"]).Checked = false;
                else
                    ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConMobile"]).Checked = true;
            }
            dtConNumbers.Rows[dtrConNumbers.CurrentItemIndex]["ACellNo"] = ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConMobile"]).CheckState;
        }

        private void btnEditConNo_Click(object sender, EventArgs e)
        {
            try
            {
                nCN = 2;
                ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkAddEditCN"]).Checked = true;
                btnAddConNo.Enabled = false; btnEditConNo.Enabled = false; btnDelConNo.Enabled = false; btnSaveConNo.Enabled = true; btnCancelConNo.Enabled = true;
            }
            catch { }
        }

        private void btnDelConEMail_Click(object sender, EventArgs e)
        {
            if (dtConEMails.Rows.Count == 0)
                return;

            if (dtConEMails.Rows[bsConEMails.Position].RowState.ToString() != "Added")
            {
                try
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("WARNING: Deleting this email address would delete" + Environment.NewLine + "other data associated with this email address." + Environment.NewLine + Environment.NewLine +
                                             "Do you want to delete this email address: " + Environment.NewLine + Environment.NewLine + ((TextBox)dtrConEMails.CurrentItem.Controls["txtConEMailAddr"]).Text +
                                             "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dReply == DialogResult.Yes)
                    {
                        bsConEMails.EndEdit();
                        UpdateContactEMails(2);

                        int nID = Convert.ToInt16(((TextBox)dtrConEMails.CurrentItem.Controls["txtConEMailID"]).Text.Trim());

                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        if (sqlcnn == null)
                        {
                            MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        ;
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@EMailID", nID);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelContactEMail";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        dtConEMails.Rows[bsConEMails.Position].Delete();
                        dtConEMails.AcceptChanges();
                        LoadConEMails();
                    }
                }
                catch { }
            }
        }

        private void btnEditConEMail_Click(object sender, EventArgs e)
        {
            try
            {
                nCE = 2;
                ((CheckBox)dtrConEMails.CurrentItem.Controls["chkAddEditCE"]).Checked = true;
                btnAddConEMail.Enabled = false; btnEditConEMail.Enabled = false; btnDelConEMail.Enabled = false; btnSaveConEMail.Enabled = true; btnCancelConEMail.Enabled = true;
            }
            catch { }
        }

        private void chkConAck_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConEMails.CurrentItem.Controls["chkConAck"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConAck"]).Checked = false;
                else
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConAck"]).Checked = true;
            }
            dtConEMails.Rows[dtrConEMails.CurrentItemIndex]["AckReports"] = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConAck"]).CheckState;
        }

        private void chkConSRpt_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConEMails.CurrentItem.Controls["chkConSRpt"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConSRpt"]).Checked = false;
                else
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConSRpt"]).Checked = true;
            }
            dtConEMails.Rows[dtrConEMails.CurrentItemIndex]["SpeedReports"] = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConSRpt"]).CheckState;
        }

        private void chkConFRpt_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConEMails.CurrentItem.Controls["chkConFRpt"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConFRpt"]).Checked = false;
                else
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConFRpt"]).Checked = true;
            }
            dtConEMails.Rows[dtrConEMails.CurrentItemIndex]["FinalReports"] = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConFRpt"]).CheckState;
        }

        private void chkConBDSRpt_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSRpt"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSRpt"]).Checked = false;
                else
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSRpt"]).Checked = true;
            }
            dtConEMails.Rows[dtrConEMails.CurrentItemIndex]["BDSReports"] = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSRpt"]).CheckState;
        }

        private void chkConBDSInv_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSInv"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSInv"]).Checked = false;
                else
                    ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSInv"]).Checked = true;
            }
            dtConEMails.Rows[dtrConEMails.CurrentItemIndex]["BDSInvoices"] = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSInv"]).CheckState;
        }

        private void dtrSpForecasts_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            try
            {
                if (e.ItemIndex < dtSpForecasts.Rows.Count)
                {
                    switch (e.Control.Name)
                    {
                        case "txtYear":
                            try
                            {
                                e.Value = dtSpForecasts.Rows[e.ItemIndex]["ForecastYear"].ToString();
                                e.Control.BackColor = Color.GhostWhite;
                            }
                            catch { }
                            break;
                        case "txtAmount":
                            try
                            {
                                decimal nAmt = (decimal)dtSpForecasts.Rows[e.ItemIndex]["ForecastAmount"];
                                e.Value = nAmt.ToString("C2"); 
                            }
                            catch { }
                            break;
                    }
                }
            }
            catch { }
        }

        private void btnAddForecast_Click(object sender, EventArgs e)
        {
            DataRow dr;
            dr = dtSpForecasts.NewRow();
            dr["ForecastYear"] = DateTime.Now.Year;
            dr["ForecastAmount"] = 0;
            dtSpForecasts.Rows.Add(dr);
            bsSpForecasts.DataSource = dtSpForecasts;
            dtrSpForecasts.DataSource = bsSpForecasts;
            int nRows = dtSpForecasts.Rows.Count;
            dtrSpForecasts.CurrentItemIndex = nRows - 1;
            btnCancelForecast.Enabled = true;
        }

        private void btnDelForecast_Click(object sender, EventArgs e)
        {
            if (dtSpForecasts.Rows.Count == 0)
                return;

            if (dtSpForecasts.Rows[bsSpForecasts.Position].RowState.ToString() != "Added")
            {
                try
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("Do you want to delete this forecast record: " + Environment.NewLine + Environment.NewLine + ((TextBox)dtrSpForecasts.CurrentItem.Controls["txtYear"]).Text + " - " + 
                                             ((TextBox)dtrSpForecasts.CurrentItem.Controls["txtAmount"]).Text + "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dReply == DialogResult.Yes)
                    {
                        bsSpForecasts.EndEdit();
                        UpdateSpForecasts(2);

                        int nYr = Convert.ToInt16(((TextBox)dtrSpForecasts.CurrentItem.Controls["txtYear"]).Text.Trim());

                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        if (sqlcnn == null)
                        {
                            MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        ;
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtID.Text));
                        sqlcmd.Parameters.AddWithValue("@Yr", nYr);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelSpForecast";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        dtSpForecasts.Rows[bsSpForecasts.Position].Delete();
                        dtSpForecasts.AcceptChanges();
                        LoadForecasts();
                    }
                }
                catch { }
            }
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void btnEditForecast_Click(object sender, EventArgs e)
        {
            try
            {
                nFC = 2;
                ((CheckBox)dtrSpForecasts.CurrentItem.Controls["chkAddEditFC"]).Checked = true;
                ((TextBox)dtrSpForecasts.CurrentItem.Controls["txtAmount"]).Focus();
                ((TextBox)dtrSpForecasts.CurrentItem.Controls["txtAmount"]).SelectAll();
                btnAddForecast.Enabled = false; btnEditForecast.Enabled = false; btnDelForecast.Enabled = false; btnSaveForecast.Enabled = true; btnCancelForecast.Enabled = true;
            }
            catch { }
        }

        private void dtrSpActivities_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            try
            {
                if (e.ItemIndex < dtSpActivities.Rows.Count)
                {
                    switch (e.Control.Name)
                    {
                        case "txtActYear":
                            try
                            {
                                e.Value = dtSpActivities.Rows[e.ItemIndex]["ActivityYear"].ToString();
                                e.Control.BackColor = Color.GhostWhite;
                            }
                            catch { }
                            break;
                        case "txtActivity":
                            try
                            {
                                e.Value = dtSpActivities.Rows[e.ItemIndex]["ActivityNotes"].ToString();
                            }
                            catch { }
                            break;
                    }
                }
            }
            catch { }
        }

        private void txtActYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtActivity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void btnAddActivity_Click(object sender, EventArgs e)
        {
            DataRow dr;
            dr = dtSpActivities.NewRow();
            dr["ActivityYear"] = DateTime.Now.Year;
            dr["ActivityNotes"] = "";
            dtSpActivities.Rows.Add(dr);
            bsSpActivities.DataSource = dtSpActivities;
            int nRows = dtSpActivities.Rows.Count;
            dtrSpActivities.CurrentItemIndex = nRows - 1;
            btnCancelActivity.Enabled = true;
        }

        private void btnSaveActivity_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.SmallInt));
            sqlcmd.Parameters["@nMode"].Value = nAN;

            sqlcmd.Parameters.Add(new SqlParameter("@SponsorID", SqlDbType.Int));
            sqlcmd.Parameters["@SponsorID"].Value = Convert.ToInt16(txtID.Text);

            sqlcmd.Parameters.Add(new SqlParameter("@ActYr", SqlDbType.SmallInt));
            sqlcmd.Parameters["@ActYr"].Value = Convert.ToInt16(((TextBox)dtrSpActivities.CurrentItem.Controls["txtActYear"]).Text);

            sqlcmd.Parameters.Add(new SqlParameter("@ActNotes", SqlDbType.NVarChar));
            sqlcmd.Parameters["@ActNotes"].Value = ((TextBox)dtrSpActivities.CurrentItem.Controls["txtActivity"]).Text;

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID; 

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditSpActivities";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            btnCancelActivity_Click(null, null);
        }

        private void dtrSpForecasts_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            ((TextBox)dtrSpForecasts.CurrentItem.Controls["txtYear"]).BackColor = Color.PaleTurquoise;
        }

        private void dtrSpActivities_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            ((TextBox)dtrSpActivities.CurrentItem.Controls["txtActYear"]).BackColor = Color.PaleTurquoise;
        }

        private void dtrSpCouriers_ItemCloned(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            var combo = (ComboBox)e.DataRepeaterItem.Controls.Find("cboCouriers", false)[0];
            //Set the data source
            dtCouriers = PSSClass.Couriers.CouriersDDL();
            if (dtCouriers == null)
            {
                return;
            }
            DataRow dR = dtCouriers.NewRow();
            dR["CourierCode"] = "---";
            dtCouriers.Rows.InsertAt(dR, 0);
            combo.DataSource = dtCouriers;
            combo.DisplayMember = "CourierCode";
            combo.ValueMember = "CourierCode";
        }

        private void dtrSpCouriers_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            try
            {
                var combo = (ComboBox)e.DataRepeaterItem.Controls.Find("cboCouriers", false)[0];
                combo.Text = dtSpCouriers.Rows[e.DataRepeaterItem.ItemIndex]["CourierCode"].ToString();
            }
            catch { }
        }

        private void dtrSpCouriers_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            if (e.ItemIndex < dtSpCouriers.Rows.Count)
            {
                switch (e.Control.Name)
                {
                    case "txtAcctNo":
                        try
                        {
                            e.Value = dtSpCouriers.Rows[e.ItemIndex]["AccountNo"];
                            e.Control.BackColor = Color.GhostWhite;
                        }
                        catch { }
                        break;
                }
            }
        }

        private void btnAddCourier_Click(object sender, EventArgs e)
        {
            DataRow dr;
            dr = dtSpCouriers.NewRow();
            dr["CourierCode"] = "---";
            dr["AccountNo"] = "";
            dtSpCouriers.Rows.Add(dr);
            bsSpCouriers.DataSource = dtSpCouriers;
            int nRows = dtSpCouriers.Rows.Count;
            dtrSpCouriers.CurrentItemIndex = nRows - 1;
            btnCancelCourier.Enabled = true;
        }

        private void btnSaveCourier_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.SmallInt));
            sqlcmd.Parameters["@nMode"].Value = nCR;

            sqlcmd.Parameters.Add(new SqlParameter("@SponsorID", SqlDbType.Int));
            sqlcmd.Parameters["@SponsorID"].Value = Convert.ToInt16(txtID.Text);

            sqlcmd.Parameters.Add(new SqlParameter("@CourierCode", SqlDbType.NVarChar));
            sqlcmd.Parameters["@CourierCode"].Value = ((ComboBox)dtrSpCouriers.CurrentItem.Controls["cboCouriers"]).Text;

            sqlcmd.Parameters.Add(new SqlParameter("@AcctNo", SqlDbType.NVarChar));
            sqlcmd.Parameters["@AcctNo"].Value = ((TextBox)dtrSpCouriers.CurrentItem.Controls["txtAcctNo"]).Text;

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID; 

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditSpCourier";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            btnCancelCourier_Click(null, null);
        }

        private void btnDelCourier_Click(object sender, EventArgs e)
        {
            if (dtSpCouriers.Rows.Count == 0)
                return;

            if (dtSpCouriers.Rows[bsSpCouriers.Position].RowState.ToString() != "Added")
            {
                try
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("Do you want to delete this record: " + Environment.NewLine + Environment.NewLine + ((ComboBox)dtrSpCouriers.CurrentItem.Controls["cboCouriers"]).Text + " - " +
                                             ((TextBox)dtrSpCouriers.CurrentItem.Controls["txtAcctNo"]).Text + "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dReply == DialogResult.Yes)
                    {
                        bsSpCouriers.EndEdit();
                        UpdateSpCouriers(2);

                        string strCourierCode = ((ComboBox)dtrSpCouriers.CurrentItem.Controls["cboCouriers"]).Text.Trim();
                        string strAcctNo = ((TextBox)dtrSpCouriers.CurrentItem.Controls["txtAcctNo"]).Text.Trim();

                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        if (sqlcnn == null)
                        {
                            MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        ;
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtID.Text));
                        sqlcmd.Parameters.AddWithValue("@CourierCode", strCourierCode);
                        sqlcmd.Parameters.AddWithValue("@AcctNo", strAcctNo);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelSpCourier";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        dtSpCouriers.Rows[bsSpCouriers.Position].Delete();
                        dtSpCouriers.AcceptChanges();
                        LoadCouriers();
                    }
                }
                catch { }
            }
        }

        private void btnEditCourier_Click(object sender, EventArgs e)
        {
            try
            {
                nCR = 2;
                ((CheckBox)dtrSpCouriers.CurrentItem.Controls["chkAddEditCR"]).Checked = true;
                ((TextBox)dtrSpCouriers.CurrentItem.Controls["txtAcctNo"]).Focus();
                ((TextBox)dtrSpCouriers.CurrentItem.Controls["txtAcctNo"]).SelectAll();
                btnAddCourier.Enabled = false; btnEditCourier.Enabled = false; btnDelCourier.Enabled = false; btnSaveCourier.Enabled = true; btnCancelCourier.Enabled = true;
            }
            catch { }
        }

        private void cboCouriers_KeyDown(object sender, KeyEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void cboCouriers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void cboCouriers_Click(object sender, EventArgs e)
        {
            string strCourier = ((ComboBox)dtrSpCouriers.CurrentItem.Controls["cboCouriers"]).Text;
            if (nMode == 0)
                ((ComboBox)dtrSpCouriers.CurrentItem.Controls["cboCouriers"]).Text = strCourier;
        }

        private void btnDelActivity_Click(object sender, EventArgs e)
        {
            if (dtSpActivities.Rows.Count == 0)
                return;

            if (dtSpActivities.Rows[bsSpActivities.Position].RowState.ToString() != "Added")
            {
                try
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("Do you want to delete this record: " + Environment.NewLine + Environment.NewLine + ((TextBox)dtrSpActivities.CurrentItem.Controls["txtActYear"]).Text + " - " +
                                             ((TextBox)dtrSpActivities.CurrentItem.Controls["txtActivity"]).Text + "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dReply == DialogResult.Yes)
                    {
                        bsSpActivities.EndEdit();
                        UpdateSpActivities(2);

                        int nYr = Convert.ToInt16(((TextBox)dtrSpActivities.CurrentItem.Controls["txtActYear"]).Text.Trim());

                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        if (sqlcnn == null)
                        {
                            MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        ;
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtID.Text));
                        sqlcmd.Parameters.AddWithValue("@ActYr", nYr);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelSpActivity";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        dtSpActivities.Rows[bsSpActivities.Position].Delete();
                        dtSpActivities.AcceptChanges();
                        LoadActivities();
                    }
                }
                catch { }
            }
        }

        private void chkPermanent_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPermanent.Checked == true)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("IMPORTANT:" + Environment.NewLine + "This process would update all existing transaction" + Environment.NewLine + "records and creates a permanent ID " +
                                         "for this Sponsor." + Environment.NewLine + Environment.NewLine + "Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (dReply == DialogResult.Yes)
                {
                    DialogResult dConfirm = new DialogResult();
                    dConfirm = MessageBox.Show("GTS would now update this Sponsor's transaction records.", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (dConfirm == DialogResult.OK)
                    {

                    }
                }
            }
        }

        private void btnSaveForecast_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.SmallInt));
            sqlcmd.Parameters["@nMode"].Value = nFC;

            sqlcmd.Parameters.Add(new SqlParameter("@SponsorID", SqlDbType.Int));
            sqlcmd.Parameters["@SponsorID"].Value = Convert.ToInt16(txtID.Text);

            sqlcmd.Parameters.Add(new SqlParameter("@Yr", SqlDbType.Int));
            sqlcmd.Parameters["@Yr"].Value = Convert.ToInt16(((TextBox)dtrSpForecasts.CurrentItem.Controls["txtYear"]).Text);

            sqlcmd.Parameters.Add(new SqlParameter("@Amt", SqlDbType.Decimal));
            sqlcmd.Parameters["@Amt"].Value = Convert.ToDecimal(((TextBox)dtrSpForecasts.CurrentItem.Controls["txtAmount"]).Text);

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID; 

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditSpForecast";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            btnCancelForecast_Click(null, null);
        }

        private void btnSaveConNo_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.SmallInt));
            sqlcmd.Parameters["@nMode"].Value = nCN;

            sqlcmd.Parameters.Add(new SqlParameter("@ContactID", SqlDbType.Int));
            sqlcmd.Parameters["@ContactID"].Value = Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text);

            sqlcmd.Parameters.Add(new SqlParameter("@PhoneID", SqlDbType.Int));
            sqlcmd.Parameters["@PhoneID"].Value = Convert.ToInt16(((TextBox)dtrConNumbers.CurrentItem.Controls["txtConNoID"]).Text);

            sqlcmd.Parameters.Add(new SqlParameter("@ContactNo", SqlDbType.VarChar));
            ((MaskedTextBox)dtrConNumbers.CurrentItem.Controls["mskPhoneNo"]).TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            sqlcmd.Parameters["@ContactNo"].Value = ((MaskedTextBox)dtrConNumbers.CurrentItem.Controls["mskPhoneNo"]).Text;

            if (((TextBox)dtrConNumbers.CurrentItem.Controls["txtExtNo"]).Text.Trim() == "")
            {
                sqlcmd.Parameters.Add(new SqlParameter("@ExtNo", SqlDbType.Int));
                sqlcmd.Parameters["@ExtNo"].Value = DBNull.Value;
            }
            else
            {
                sqlcmd.Parameters.Add(new SqlParameter("@ExtNo", SqlDbType.Int));
                sqlcmd.Parameters["@ExtNo"].Value = Convert.ToInt16(((TextBox)dtrConNumbers.CurrentItem.Controls["txtExtNo"]).Text);
            }

            sqlcmd.Parameters.Add(new SqlParameter("@TelNo", SqlDbType.Bit));
            sqlcmd.Parameters["@TelNo"].Value = ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConPhone"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@FAXNo", SqlDbType.Bit));
            sqlcmd.Parameters["@FAXNo"].Value = ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConFAX"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@CPNo", SqlDbType.Bit));
            sqlcmd.Parameters["@CPNo"].Value = ((CheckBox)dtrConNumbers.CurrentItem.Controls["chkConMobile"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID; 

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditContactNo";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            btnCancelConNo_Click(null, null);
        }

        private void btnAddSpAddr_Click(object sender, EventArgs e)
        {
            DataRow dr;
            dr = dtSpAddresses.NewRow();
            dr["AddressID"] = "(New)";
            dr["StAddr"] = "";
            dr["CityAddr"] = "";
            dr["ZipCode"] = "";
            dr["Country"] = "";
            dr["StateCode"] = "";
            dr["APContact"] = "";
            dr["Notes"] = "";
            dr["Billing"] = false;
            dr["Reports"] = false;
            dr["Active"] = true;
            dtSpAddresses.Rows.Add(dr);
            bsSpAddresses.DataSource = dtSpAddresses;
            dtrSpAddresses.DataSource = bsSpAddresses;
            int nRows = dtSpAddresses.Rows.Count;
            dtrSpAddresses.CurrentItemIndex = nRows - 1;
            btnCancelSpAddr.Enabled = true;
        }

        private void btnSaveConEMail_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.SmallInt));
            sqlcmd.Parameters["@nMode"].Value = nCE;

            sqlcmd.Parameters.Add(new SqlParameter("@ContactID", SqlDbType.Int));
            sqlcmd.Parameters["@ContactID"].Value = Convert.ToInt16(((TextBox)dtrContacts.CurrentItem.Controls["txtContactID"]).Text);

            sqlcmd.Parameters.Add(new SqlParameter("@EMailID", SqlDbType.Int));
            sqlcmd.Parameters["@EMailID"].Value = Convert.ToInt16(((TextBox)dtrConEMails.CurrentItem.Controls["txtConEMailID"]).Text);

            sqlcmd.Parameters.Add(new SqlParameter("@EMailAddress", SqlDbType.VarChar));
            sqlcmd.Parameters["@EMailAddress"].Value = ((TextBox)dtrConEMails.CurrentItem.Controls["txtConEMailAddr"]).Text;

            sqlcmd.Parameters.Add(new SqlParameter("@AckRpt", SqlDbType.Bit));
            sqlcmd.Parameters["@AckRpt"].Value = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConAck"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@SpeedRpt", SqlDbType.Bit));
            sqlcmd.Parameters["@SpeedRpt"].Value = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConSRpt"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@FinalRpt", SqlDbType.Bit));
            sqlcmd.Parameters["@FinalRpt"].Value = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConFRpt"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@BDSRpt", SqlDbType.Bit));
            sqlcmd.Parameters["@BDSRpt"].Value = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSRpt"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@BDSInv", SqlDbType.Bit));
            sqlcmd.Parameters["@BDSInv"].Value = ((CheckBox)dtrConEMails.CurrentItem.Controls["chkConBDSInv"]).CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID; 

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditContactEMail";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            btnCancelConEMail_Click(null, null);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr += 1;
            if (nCtr > 1)
            {
                DataTable dt = new DataTable();
                dt = PSSClass.Sponsors.SponsorMaster(1);
                PSSClass.DataEntry.DGVSearch(tstbSearchField.Text, tstbSearch.Text, dt, bsFile);
                nCtr = 0;
                tstbSearch.Text = "";
                timer1.Enabled = false;
                nSw = 0;
            }
        }

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void lblHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void txtConAddressID_Click(object sender, EventArgs e)
        {
            if (nMode == 2)
            {
                pnlRecord.Enabled = false;
                pnlSpAddresses.Visible = true; pnlSpAddresses.Left = 114; pnlSpAddresses.Top = 258; pnlSpAddresses.BringToFront();
                dgvSpAddresses.Columns["AddressID"].Width = 50;
                dgvSpAddresses.Columns["AddressID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSpAddresses.Columns["Address"].Width = 376;
            }
        }

        private void txtConAddressID_Enter(object sender, EventArgs e)
        {
            if (nMode == 2)
            {
                pnlRecord.Enabled = false;
                pnlSpAddresses.Visible = true; pnlSpAddresses.Left = 114; pnlSpAddresses.Top = 258; pnlSpAddresses.BringToFront();
                dgvSpAddresses.Columns["AddressID"].Width = 50;
                dgvSpAddresses.Columns["AddressID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSpAddresses.Columns["Address"].Width = 376;
            }
        }

        private void dgvSpAddresses_DoubleClick(object sender, EventArgs e)
        {
            pnlSpAddresses.Visible = false; pnlRecord.Enabled = true;

            dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex]["ConAddressID"] = dgvSpAddresses.Rows[dgvSpAddresses.CurrentRow.Index].Cells[0].Value.ToString();
            dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex]["ConAddress"] = dgvSpAddresses.Rows[dgvSpAddresses.CurrentRow.Index].Cells[1].Value.ToString();
            if (((TextBox)dtrConAddresses.CurrentItem.Controls["txtConAddrID"]).Text == "(New)")
            {
                dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex]["ConAddrID"] = dgvSpAddresses.Rows[dgvSpAddresses.CurrentRow.Index].Cells[0].Value.ToString();
            }
        }

        private void cboStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var combo = (ComboBox)sender;
                var dataRepeaterItem = (Microsoft.VisualBasic.PowerPacks.DataRepeaterItem)combo.Parent;
                var dataRepeater = (Microsoft.VisualBasic.PowerPacks.DataRepeater)combo.Parent.Parent;
                dtSpAddresses.Rows[dataRepeaterItem.ItemIndex]["StateCode"] = combo.SelectedValue;
            }
            catch { }
        }

        private void txtConAddressID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void dtrContacts_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            switch (e.Control.Name)
            {
                case "txtContactID":
                    try
                    {
                        dtContacts.Rows[e.ItemIndex]["ContactID"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtContactTitle":
                    try
                    {
                        dtContacts.Rows[e.ItemIndex]["Title"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtContactLastName":
                    try
                    {
                        dtContacts.Rows[e.ItemIndex]["LastName"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtContactFirstName":
                    try
                    {
                        dtContacts.Rows[e.ItemIndex]["FirstName"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtContactMI":
                    try
                    {
                        dtContacts.Rows[e.ItemIndex]["MidInitial"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtContactSuffix":
                    try
                    {
                        dtContacts.Rows[e.ItemIndex]["Suffix"] = e.Value;
                    }
                    catch { }
                    break;
            }
        }

        private void chkHoliday_Click(object sender, EventArgs e)
        {
            dtContacts.Rows[dtrContacts.CurrentItemIndex]["HolidayList"] = ((CheckBox)dtrContacts.CurrentItem.Controls["chkHoliday"]).CheckState;
        }

        private void chkConBDSUser_Click(object sender, EventArgs e)
        {
            dtContacts.Rows[dtrContacts.CurrentItemIndex]["BDSUser"] = ((CheckBox)dtrContacts.CurrentItem.Controls["chkConBDSUser"]).CheckState;
        }

        private void dtrSpAddresses_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            switch (e.Control.Name)
            {
                case "txtSpAddressID":
                    try
                    {
                        dtSpAddresses.Rows[e.ItemIndex]["AddressID"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtSpStreet":
                    try
                    {
                        dtSpAddresses.Rows[e.ItemIndex]["StAddr"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtSpCity":
                    try
                    {
                        dtSpAddresses.Rows[e.ItemIndex]["CityAddr"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtState":
                    try
                    {
                        dtSpAddresses.Rows[e.ItemIndex]["StateCode"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtSpZipCode":
                    try
                    {
                        dtSpAddresses.Rows[e.ItemIndex]["ZipCode"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtSpCountry":
                    try
                    {
                        dtSpAddresses.Rows[e.ItemIndex]["Country"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtSpAP":
                    try
                    {
                        dtSpAddresses.Rows[e.ItemIndex]["APContact"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtSpNotes":
                    try
                    {
                        dtSpAddresses.Rows[e.ItemIndex]["Notes"] = e.Value;
                    }
                    catch { }
                    break;
            }
        }

        private void chkReportsAddr_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkReportsAddr"]).CheckState == CheckState.Checked)
                    ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkReportsAddr"]).Checked = false;
                else
                    ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkReportsAddr"]).Checked = true;
            }
            dtSpAddresses.Rows[dtrSpAddresses.CurrentItemIndex]["Reports"] = ((CheckBox)dtrSpAddresses.CurrentItem.Controls["chkReportsAddr"]).CheckState;
        }

        private void dtrConAddresses_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            switch (e.Control.Name)
            {
                case "txtConIDAddr":
                    try
                    {
                        dtConAddresses.Rows[e.ItemIndex]["ConID"] =  e.Value;
                    }
                    catch { }
                    break;
                case "txtConAddressID":
                    try
                    {
                        dtConAddresses.Rows[e.ItemIndex]["ConAddressID"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtConAddress":
                    try
                    {
                        dtConAddresses.Rows[e.ItemIndex]["ConAddress"] = e.Value;
                    }
                    catch { }
                    break;
            }
        }
 

        private void btnCloseAddresses_Click(object sender, EventArgs e)
        {
            pnlSpAddresses.Visible = false; pnlRecord.Enabled = true;
        }

        private void dgvSpAddresses_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                pnlSpAddresses.Visible = false; pnlRecord.Enabled = true;
                dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex]["ConAddressID"] = dgvSpAddresses.Rows[dgvSpAddresses.CurrentRow.Index].Cells[0].Value.ToString();
                dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex]["ConAddress"] = dgvSpAddresses.Rows[dgvSpAddresses.CurrentRow.Index].Cells[1].Value.ToString();
                if (((TextBox)dtrConAddresses.CurrentItem.Controls["txtConAddrID"]).Text == "(New)")
                {
                    dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex]["ConAddrID"] = dgvSpAddresses.Rows[dgvSpAddresses.CurrentRow.Index].Cells[0].Value.ToString();
                }

            }
            else if (e.KeyChar == 27)
            {
                pnlSpAddresses.Visible = false; pnlRecord.Enabled = true;
            }
        }

        private void dgvSpAddresses_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void cboStates_DropDown(object sender, EventArgs e)
        {
            strState = ((ComboBox)dtrSpAddresses.CurrentItem.Controls["cboStates"]).Text;
        }

        private void cboStates_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (nMode == 0)
                ((ComboBox)dtrSpAddresses.CurrentItem.Controls["cboStates"]).Text = strState;
        }

        private void dtrConNumbers_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            switch (e.Control.Name)
            {
                case "txtConIDNo":
                    try
                    {
                        dtConNumbers.Rows[e.ItemIndex]["ConID"] = e.Value;
                    }
                    catch { }
                    break;
                case "mskPhoneNo":
                    try
                    {
                        ((MaskedTextBox)dtrConNumbers.CurrentItem.Controls["mskPhoneNo"]).TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                        dtConNumbers.Rows[e.ItemIndex]["ContactNo"] = ((MaskedTextBox)dtrConNumbers.CurrentItem.Controls["mskPhoneNo"]).Text;
                    }
                    catch { }
                    break;
                case "txtExtNo":
                    try
                    {
                        dtConNumbers.Rows[e.ItemIndex]["ExtNo"] = e.Value;
                    }
                    catch { }
                    break;
            }
        }

        private void dtrConEMails_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            switch (e.Control.Name)
            {
                case "txtConIDEM":
                    try
                    {
                        dtConEMails.Rows[e.ItemIndex]["ConID"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtConEMailID":
                    try
                    {
                        dtConEMails.Rows[e.ItemIndex]["EMailID"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtConEMailAddr":
                    try
                    {
                        dtConEMails.Rows[e.ItemIndex]["EMailAddress"] = e.Value;
                    }
                    catch { }
                    break;
            }
        }

        private void dtrSpForecasts_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            switch (e.Control.Name)
            {
                case "txtYear":
                    try
                    {
                        dtSpForecasts.Rows[e.ItemIndex]["ForecastYear"] = e.Value;
                    }
                    catch { }
                    break;
                case "txtAmount":
                    try
                    {
                        dtSpForecasts.Rows[e.ItemIndex]["ForecastAmount"] = e.Value.ToString().Replace("$", "");
                    }
                    catch { }
                    break;
            }
        }

        private void txtAmount_Click(object sender, EventArgs e)
        {
            ((TextBox)dtrSpForecasts.CurrentItem.Controls["txtAmount"]).SelectAll();
        }

        private void txtAmount_Leave(object sender, EventArgs e)
        {
            try
            {
                ((TextBox)dtrSpForecasts.CurrentItem.Controls["txtAmount"]).Text = Convert.ToDecimal(((TextBox)dtrSpForecasts.CurrentItem.Controls["txtAmount"]).Text).ToString("C2");
            }
            catch { }
        }

        private void dtrSpActivities_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            switch (e.Control.Name)
            {
                case "txtActYear":
                    try
                    {
                        dtSpActivities.Rows[e.ItemIndex]["ActivityYear"] = Convert.ToInt16(e.Value);
                    }
                    catch { }
                    break;
                case "txtActivity":
                    try
                    {
                        dtSpActivities.Rows[e.ItemIndex]["ActivityNotes"] = e.Value;
                    }
                    catch { }
                    break;
            }
        }

        private void dtrSpCouriers_ItemValuePushed(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            switch (e.Control.Name)
            {
                case "txtAcctNo":
                    try
                    {
                        dtSpCouriers.Rows[e.ItemIndex]["AccountNo"] = e.Value;
                    }
                    catch { }
                    break;
            }
        }

        private void cboCouriers_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (nMode == 0)
                ((ComboBox)dtrSpCouriers.CurrentItem.Controls["cboCouriers"]).Text = strCourier;
        }

        private void cboCouriers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
                try
                {
                    var combo = (ComboBox)sender;
                    var dataRepeaterItem = (Microsoft.VisualBasic.PowerPacks.DataRepeaterItem)combo.Parent;
                    dtSpCouriers.Rows[dataRepeaterItem.ItemIndex]["CourierCode"] = combo.SelectedValue;
                }
                catch { }
        }

        private void txtState_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvStates.Visible = true; dgvStates.BringToFront(); dgvStates.Top = 450; dgvStates.Left = 366;
                dgvStates.Columns[0].Width = 40; dgvStates.Enabled = true; dgvStates.TabIndex = 4;
            }
        }

        private void txtState_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    DataView dvwStates;
                    dvwStates = new DataView(dtStates, "State like '" + ((TextBox)dtrSpAddresses.CurrentItem.Controls["txtState"]).Text.Trim().Replace("'", "''") + "%'", "State", DataViewRowState.CurrentRows);
                    dgvStates.Columns[0].Width = 40;
                    dgvStates.DataSource = dvwStates;
                }
                catch { }
            }
        }

        private void dtrSpAddresses_ItemTemplate_MouseHover(object sender, EventArgs e)
        {
            dgvStates.Visible = false;
        }

        private void txtState_Click(object sender, EventArgs e)
        {
            txtState_Enter(null, null);
        }

        private void txtState_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                dgvStates.Visible = false;
                ((TextBox)dtrSpAddresses.CurrentItem.Controls["txtSpZipCode"]).Focus();
            }
            else if (e.KeyChar == 27)
            {
                dgvStates.Visible = false;
            }
        }

        private void dgvStates_DoubleClick(object sender, EventArgs e)
        {
            ((TextBox)dtrSpAddresses.CurrentItem.Controls["txtState"]).Text = dgvStates.CurrentRow.Cells[0].Value.ToString();
            dgvStates.Visible = false;
        }

        private void picStates_Click(object sender, EventArgs e)
        {
            txtState_Enter(null, null);
        }

        private void tbcSubData_Selected(object sender, TabControlEventArgs e)
        {
            nTab = tbcSubData.SelectedIndex;
        }

        private void btnContacts_Click(object sender, EventArgs e)
        {
            bsContacts.EndEdit();
            for (int i = 0; i < dtContacts.Rows.Count; i++)
            {
                MessageBox.Show(dtContacts.Rows[i].RowState.ToString());
            }
        }

        private void btnSpAddresses_Click(object sender, EventArgs e)
        {
            bsSpAddresses.EndEdit();
            for (int i = 0; i < dtSpAddresses.Rows.Count; i++)
            {
                MessageBox.Show(dtSpAddresses.Rows[i].RowState.ToString());
            }
        }

        private void btnConAddresses_Click(object sender, EventArgs e)
        {
            bsConAddresses.EndEdit();
            for (int i = 0; i < dtConAddresses.Rows.Count; i++)
            {
                MessageBox.Show(dtConAddresses.Rows[i].RowState.ToString());
            }
        }

        private void btnConNumbers_Click(object sender, EventArgs e)
        {
            bsConNumbers.EndEdit();
            for (int i = 0; i < dtConNumbers.Rows.Count; i++)
            {
                MessageBox.Show(dtConNumbers.Rows[i].RowState.ToString());
            }
        }

        private void btnConEMails_Click(object sender, EventArgs e)
        {
            bsConEMails.EndEdit();
            for (int i = 0; i < dtConEMails.Rows.Count; i++)
            {
                MessageBox.Show(dtConEMails.Rows[i].RowState.ToString());
            }
        }

        private void btnSpForecast_Click(object sender, EventArgs e)
        {
            bsSpForecasts.EndEdit();
            for (int i = 0; i < dtSpForecasts.Rows.Count; i++)
            {
                MessageBox.Show(dtSpForecasts.Rows[i].RowState.ToString());
            }
        }

        private void btnSpCouriers_Click(object sender, EventArgs e)
        {
            bsSpCouriers.EndEdit();
            for (int i = 0; i < dtSpCouriers.Rows.Count; i++)
            {
                MessageBox.Show(dtSpCouriers.Rows[i].RowState.ToString());
            }
        }

        private void btnSpActivities_Click(object sender, EventArgs e)
        {
            bsSpActivities.EndEdit();
            for (int i = 0; i < dtSpActivities.Rows.Count; i++)
            {
                MessageBox.Show(dtSpActivities.Rows[i].RowState.ToString());
            }
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            bsMain.EndEdit();
            for (int i = 0; i < dtMain.Rows.Count; i++)
            {
                MessageBox.Show(dtMain.Rows[i].RowState.ToString());
            }
        }

        private void dgvStates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvStates_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ((TextBox)dtrSpAddresses.CurrentItem.Controls["txtState"]).Text = dgvStates.CurrentRow.Cells[0].Value.ToString();
                dgvStates.Visible = false;
            }
        }

        private void txtSpStreet_Enter(object sender, EventArgs e)
        {
            dgvStates.Visible = false;
        }

        private void txtSpCity_Enter(object sender, EventArgs e)
        {
            dgvStates.Visible = false;
        }

        private void txtSpZipCode_Enter(object sender, EventArgs e)
        {
            dgvStates.Visible = false;
        }

        private void txtSpCountry_Enter(object sender, EventArgs e)
        {
            dgvStates.Visible = false;
        }

        private void txtSpAP_Enter(object sender, EventArgs e)
        {
            dgvStates.Visible = false;
        }

        private void txtSpNotes_Enter(object sender, EventArgs e)
        {
            dgvStates.Visible = false;
        }

        private void btnCancelContact_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtContacts.Rows[dtrContacts.CurrentItemIndex].RowState.ToString() == "Added")
                {
                    dtContacts.Rows[bsContacts.Position].Delete();
                    DataTable dt = new DataTable();
                    dt = dtContacts.GetChanges(DataRowState.Added);
                    if (dt == null || dt.Rows.Count == 0)
                        btnCancelContact.Enabled = false;
                    dt.Dispose();
                }
            }
            catch { }
        }

        private void btnCancelSpAddr_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtSpAddresses.Rows[dtrSpAddresses.CurrentItemIndex].RowState.ToString() == "Added")
                {
                    dtSpAddresses.Rows[bsSpAddresses.Position].Delete();
                    DataTable dt = new DataTable();
                    dt = dtSpAddresses.GetChanges(DataRowState.Added);
                    if (dt == null || dt.Rows.Count == 0)
                        btnCancelSpAddr.Enabled = false;
                    dt.Dispose();
                }
            }
            catch { }
        }

        private void btnCancelConAddr_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtConAddresses.Rows[dtrConAddresses.CurrentItemIndex].RowState.ToString() == "Added")
                {
                    dtConAddresses.Rows[bsConAddresses.Position].Delete();
                    DataTable dt = new DataTable();
                    dt = dtConAddresses.GetChanges(DataRowState.Added);
                    if (dt == null || dt.Rows.Count == 0)
                        btnCancelConAddr.Enabled = false;
                    dt.Dispose();
                }
            }
            catch { }
        }

        private void btnCancelConNo_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtConNumbers.Rows[dtrConNumbers.CurrentItemIndex].RowState.ToString() == "Added")
                {
                    dtConNumbers.Rows[bsConNumbers.Position].Delete();
                    DataTable dt = new DataTable();
                    dt = dtConNumbers.GetChanges(DataRowState.Added);
                    if (dt == null || dt.Rows.Count == 0)
                        btnCancelConNo.Enabled = false;
                    dt.Dispose();
                }
            }
            catch { }
        }

        private void btnCancelConEMail_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtConEMails.Rows[dtrConEMails.CurrentItemIndex].RowState.ToString() == "Added")
                {
                    dtConEMails.Rows[bsConEMails.Position].Delete();
                    DataTable dt = new DataTable();
                    dt = dtConEMails.GetChanges(DataRowState.Added);
                    if (dt == null || dt.Rows.Count == 0)
                        btnCancelConEMail.Enabled = false;
                    dt.Dispose();
                }
            }
            catch { }
        }

        private void btnCancelForecast_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtSpForecasts.Rows[dtrSpForecasts.CurrentItemIndex].RowState.ToString() == "Added")
                {
                    dtSpForecasts.Rows[bsSpForecasts.Position].Delete();
                    DataTable dt = new DataTable();
                    dt = dtSpForecasts.GetChanges(DataRowState.Added);
                    if (dt == null || dt.Rows.Count == 0)
                        btnCancelForecast.Enabled = false;
                    dt.Dispose();
                }
            }
            catch { }
        }

        private void btnCancelCourier_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtSpCouriers.Rows[dtrSpCouriers.CurrentItemIndex].RowState.ToString() == "Added")
                {
                    dtSpCouriers.Rows[bsSpCouriers.Position].Delete();
                    DataTable dt = new DataTable();
                    dt = dtSpCouriers.GetChanges(DataRowState.Added);
                    if (dt == null || dt.Rows.Count == 0)
                        btnCancelCourier.Enabled = false;
                    dt.Dispose();
                }
            }
            catch { }
        }

        private void btnCancelActivity_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtSpActivities.Rows[dtrSpActivities.CurrentItemIndex].RowState.ToString() == "Added")
                {
                    dtSpActivities.Rows[bsSpActivities.Position].Delete();
                    DataTable dt = new DataTable();
                    dt = dtSpActivities.GetChanges(DataRowState.Added);
                    if (dt == null || dt.Rows.Count == 0)
                        btnCancelActivity.Enabled = false;
                    dt.Dispose();
                }
            }
            catch { }
        }

        private void rdoCRUpdated_Click(object sender, EventArgs e)
        {
            txtCRStatus.Text = "0";
        }

        private void rdoCHW_Click(object sender, EventArgs e)
        {
            txtCRStatus.Text = "1";
        }

        private void rdoCH_Click(object sender, EventArgs e)
        {
            txtCRStatus.Text = "2";
        }

        private void rdoCOD_Click(object sender, EventArgs e)
        {
            txtChargeType.Text = "0";
        }

        private void rdoPOBound_Click(object sender, EventArgs e)
        {
            txtChargeType.Text = "1";

        }

        private void rdoCredit_Click(object sender, EventArgs e)
        {
            txtChargeType.Text = "2";
        }

        private void txtConEMailAddr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtAcctNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void Sponsors_KeyDown(object sender, KeyEventArgs e)
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

        private void btnSendEMail_Click(object sender, EventArgs e)
        {
            pnlEMail.Visible = false; pnlRecord.Enabled = true;

            string strBody = txtBody.Text.Replace("\r\n", "<br />");
            string strSignature = ReadSignature();
            strBody = strBody + "<br /><br />" + strSignature;
            strBody = strBody.Replace("<br />", Environment.NewLine);

            if (lstAttachment.Items.Count == 0)
            {
                MessageBox.Show("No attachment found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.

            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            oMsg.HTMLBody = "<FONT face=\"Calibri\">";
            oMsg.HTMLBody += strBody;
            //Add an attachment.
            for (int i = 0; i < lstAttachment.Items.Count; i++)
            {
                //strFile = Path.GetFileName(lstAttachment.Items[i].ToString());
                oMsg.Attachments.Add(lstAttachment.Items[i].ToString());
            }
            //Subject line
            oMsg.Subject = txtSubject.Text;
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
            //Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(txtTo.Text); // "adelacruz@gibraltarlabsinc.com"

            string[] EMAddresses = txtTo.Text.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < EMAddresses.Length; i++)
            {
                if (EMAddresses[i].Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                    oRecip.Resolve();
                }
            }
            oMsg.CC = txtCC.Text;
            oMsg.BCC = txtBCC.Text;

            //oRecip.Resolve();
            oMsg.Display();

            //Send.
            //oMsg.Send();
            //((Outlook._MailItem)oMsg).Send();

            // Clean up.
            //oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;

            //UPDATE ACCPAC Customer CH Status 6/10/2016
            if (txtSubject.Text == "Credit Hold")
                PSSClass.ACCPAC.UpdCRStatus(txtID.Text, 1);
            else if (txtSubject.Text == "Credit Hold Release")
                PSSClass.ACCPAC.UpdCRStatus(txtID.Text, 0);
        }

        private void btnCancelSend_Click(object sender, EventArgs e)
        {
            pnlEMail.Visible = false; pnlRecord.Enabled = true;
        }

        private void lnkSOA_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkSOA.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
