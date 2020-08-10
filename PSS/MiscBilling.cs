using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;

namespace PSS
{
    public partial class MiscBilling : PSS.TemplateForm
    {
        public byte nFB;
        public int nInvceNo;
        public int nSpID;
        public int nConID;
        public string strSpName;
        public string strConName;

        private byte nMode = 0;
        //variables for record panel drag and drop
        private bool mouseDown;
        private Point mousePos;
        //
        private string[] arrCol;
        private int nIndex;
        //private int nSave = 1; //for checking if save is successful
        //private byte nRev = 0;
        private byte nAddRepl = 0; //Identifier for Add New Test Items/Replace Quote Ref

        private string strFileAccess = "RO"; //User's Access to this File

        //for DatagridView search
        private int nCtr = 0;
        private int nSw = 0;
        //======================

        private List<int> nListDelFees = new List<int>();
        
        protected DataTable dtSponsors = new DataTable();
        protected DataTable dtContacts = new DataTable();
        protected DataTable dtInvoice = new DataTable();
        protected DataTable dtPrepayments = new DataTable();
        protected DataTable dtBillItems = new DataTable();
        protected DataTable dtBillSummary = new DataTable();
        protected DataTable dtOtherFees = new DataTable();
        protected DataTable dtSC = new DataTable();

        public MiscBilling()
        {
            InitializeComponent();
            bnMoveFirst.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveLast.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveNext.Click += new EventHandler(MoveRecordClickHandler);
            bnMovePrevious.Click += new EventHandler(MoveRecordClickHandler);
            tsbAdd.Click += new EventHandler(AddClickHandler);
            tsbEdit.Click += new EventHandler(EditClickHandler);
            //tsbDelete.Click += new EventHandler(DeleteClickHandler);
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
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void MiscBilling_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "FinalBilling");

            LoadRecords();

            BuildPrintItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            //
            dtInvoice.Columns.Add("CompanyCode", typeof(string));
            dtInvoice.Columns.Add("InvoiceNo", typeof(string));
            dtInvoice.Columns.Add("InvoiceDate", typeof(DateTime));
            dtInvoice.Columns.Add("InvoiceType", typeof(Int16));
            dtInvoice.Columns.Add("DateMailed", typeof(DateTime));
            dtInvoice.Columns.Add("DateRevised", typeof(DateTime));
            dtInvoice.Columns.Add("RevisedByID", typeof(Int16));
            dtInvoice.Columns.Add("MailMode", typeof(Int16));
            dtInvoice.Columns.Add("MailedBy", typeof(Int16));
            dtInvoice.Columns.Add("SponsorID", typeof(Int16));
            dtInvoice.Columns.Add("SponsorName", typeof(string));
            dtInvoice.Columns.Add("ContactID", typeof(Int16));
            dtInvoice.Columns.Add("ContactName", typeof(string));
            dtInvoice.Columns.Add("InvoiceNotes", typeof(string));
            dtInvoice.Columns.Add("NonPrintingNotes", typeof(string));
            dtInvoice.Columns.Add("Posted", typeof(bool));
            dtInvoice.Columns.Add("DateCreated", typeof(DateTime));
            dtInvoice.Columns.Add("CreatedByID", typeof(Int16));
            dtInvoice.Columns.Add("LastUpdate", typeof(DateTime));
            dtInvoice.Columns.Add("LastUserID", typeof(Int16));
            //chkExtraInvoice
            dtInvoice.Columns.Add("ExtraInvoice", typeof(bool));


            bsInvoice.DataSource = dtInvoice;

            dtPrepayments.Columns.Add("InvoiceNo", typeof(Int32));
            dtPrepayments.Columns.Add("InvoiceDate", typeof(DateTime));
            dtPrepayments.Columns.Add("ServiceCode", typeof(Int16));
            dtPrepayments.Columns.Add("ServiceDesc", typeof(string));
            dtPrepayments.Columns.Add("AmtDue", typeof(decimal));
            dgvPrepayments.DataSource = dtPrepayments;
            PrepaymentGridSetting();

            dtBillItems.Columns.Add("InvID", typeof(Int64));
            dtBillItems.Columns.Add("RptNo", typeof(Int32));
            dtBillItems.Columns.Add("LogNo", typeof(Int32));
            dtBillItems.Columns.Add("SC", typeof(Int16));
            dtBillItems.Columns.Add("QuoteNo", typeof(string));
            dtBillItems.Columns.Add("PONo", typeof(string));
            dtBillItems.Columns.Add("CtrlNo", typeof(string));
            dtBillItems.Columns.Add("TestDesc", typeof(string));
            dtBillItems.Columns.Add("RushTest", typeof(bool));
            dtBillItems.Columns.Add("BillQty", typeof(decimal));
            dtBillItems.Columns.Add("UnitPrice", typeof(decimal));
            dtBillItems.Columns.Add("Prepayments", typeof(decimal));
            dtBillItems.Columns.Add("AmtDue", typeof(decimal));
            dtBillItems.Columns.Add("RushFee", typeof(decimal));
            dtBillItems.Columns.Add("QuotationNo", typeof(string));
            dtBillItems.Columns.Add("RevisionNo", typeof(Int16));
            dtBillItems.Columns.Add("CtrldSubs", typeof(bool));
            dtBillItems.Columns.Add("CtrldSubsOrig", typeof(bool));
            bsBillItems.DataSource = dtBillItems;
            bnBillItems.BindingSource = bsBillItems;
            dgvBillItems.DataSource = bsBillItems;
            BillItemsGridSetting();

            dtBillSummary.Columns.Add("ReportNo", typeof(Int32));
            dtBillSummary.Columns.Add("PSSNo", typeof(Int32));
            dtBillSummary.Columns.Add("SC", typeof(Int16));
            dtBillSummary.Columns.Add("SCDesc", typeof(string));
            dtBillSummary.Columns.Add("Amount", typeof(decimal));
            bsBillSummary.DataSource = dtBillSummary;
            bnBillSummary.BindingSource = bsBillSummary;
            dgvBillSummary.DataSource = bsBillSummary;
            BillSummaryGidSetting();

            dtOtherFees.Columns.Add("InvoiceID", typeof(Int64));
            dtOtherFees.Columns.Add("ReportNo", typeof(Int32));
            dtOtherFees.Columns.Add("PSSNo", typeof(Int32));
            dtOtherFees.Columns.Add("ServiceCode", typeof(Int16));
            dtOtherFees.Columns.Add("ServiceDesc", typeof(string));
            dtOtherFees.Columns.Add("TestDesc1", typeof(string));
            dtOtherFees.Columns.Add("BillQty", typeof(decimal));
            dtOtherFees.Columns.Add("UnitPrice", typeof(decimal));
            dtOtherFees.Columns.Add("Amount", typeof(decimal));
            dtOtherFees.Columns.Add("QuotationNo", typeof(string));
            dtOtherFees.Columns.Add("RevisionNo", typeof(Int16));
            dtOtherFees.Columns.Add("ControlNo", typeof(Int16));
            dtOtherFees.Columns.Add("QCmpyCode", typeof(string));
            dtOtherFees.Columns.Add("LCmpyCode", typeof(string));
            dtOtherFees.Columns.Add("RCmpyCode", typeof(string));
            bsOtherFees.DataSource = dtOtherFees;
            bnOtherFees.BindingSource = bsOtherFees;
            dgvOtherFees.DataSource = bsOtherFees;
            OtherFeesGridSetting();

            dtSC = PSSClass.ServiceCodes.SCDDL();

            if (nFB == 1)
            {
                AddRecord();
                AddEditMode(true);
                //LoadPO();
            }
            else if (nFB == 2)
            {
                PSSClass.General.FindRecord("InvoiceNo", nInvceNo.ToString(), bsFile, dgvFile);
                LoadData();
            }
        }

        private void MiscBilling_KeyDown(object sender, KeyEventArgs e)
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

        private void BuildPrintItems()
        {
            //Create Print Menu Dropdown List
            if (tsddbPrint.DropDownItems.Count == 0)
            {
                DataTable dt = PSSClass.General.ReportsList("FinalBilling");
                if (dt.Rows.Count > 0)
                {
                    ToolStripMenuItem[] items = new ToolStripMenuItem[dt.Rows.Count];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        items[i] = new ToolStripMenuItem();
                        items[i].Name = dt.Rows[i]["ReportName"].ToString();
                        items[i].Text = dt.Rows[i]["ReportTitle"].ToString();
                        items[i].Click += new EventHandler(PrintRptClickHandler);
                    }
                    tsddbPrint.DropDownItems.AddRange(items);
                }
            }
        }

        private void FileAccess()
        {
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsddbPrint.Enabled = false; 
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = false; btnPrintPreview.Enabled = true; btnPrint.Enabled = true; btnEMail.Enabled = false; btnScan.Enabled = true;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true; btnPrintPreview.Enabled = true; btnPrint.Enabled = true; btnEMail.Enabled = true; btnScan.Enabled = true;
            }
            tsddbSearch.Enabled = true;
        }

        private void PrintRptClickHandler(object sender, EventArgs e)
        {
            AcctgRpt rptForInvoice = new AcctgRpt();
            rptForInvoice.WindowState = FormWindowState.Maximized;
            rptForInvoice.nQ = 1;
            rptForInvoice.rptName = rptName;
            try
            {
                rptForInvoice.Show();
            }
            catch { }
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

        private void SaveClickHandler(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            LoadRecords(); //tsbRefresh.Enabled = false;
        }

        public void SearchOKClickHandler(object sender, EventArgs e)
        {
            try
            {
                bsFile.Filter = "InvoiceNo<>0";
                PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                dgvFile.Select();
            }
            catch { }
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
            }
        }

        private void dgvCellMouseClickEventHandler(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                cklColumns.Visible = true; cklColumns.BringToFront();
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

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
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

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            if (dgvFile.Rows.Count > 0)
                LoadData();
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
            if (dtSponsors.Rows.Count == 0)
                LoadSponsorsDDL();

            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false; 
            btnAddOthFees.Enabled = true;
            ClearControls(pnlRecord);
            OpenControls(pnlRecord, true);
            dtInvoice.Rows.Clear(); dtPrepayments.Rows.Clear(); dtBillItems.Rows.Clear(); dtBillSummary.Rows.Clear(); dtOtherFees.Rows.Clear();
            txtPOAmt.ReadOnly = true;

            DataRow dr;
            dr = dtInvoice.NewRow();

            dr["CompanyCode"] = "P";
            dr["InvoiceNo"] = "(New)";
            dr["InvoiceDate"] = DateTime.Now;
            dr["InvoiceType"] = 3;
            dr["DateMailed"] = DBNull.Value;
            dr["MailMode"] = DBNull.Value;
            dr["MailedBy"] = DBNull.Value;
            if (nFB == 1)
            {
                dr["SponsorID"] = nSpID;
                dr["SponsorName"] = strSpName;
                dr["ContactID"] = nConID;
                dr["ContactName"] = strConName;
            }
            else
            {
                dr["SponsorID"] = DBNull.Value;
                dr["SponsorName"] = "";
                dr["ContactID"] = DBNull.Value;
                dr["ContactName"] = "";
            }
            dr["InvoiceNotes"] = "";
            dr["NonPrintingNotes"] = "";
            dr["Posted"] = false;
            //chkExtraInvoice
            dr["ExtraInvoice"] = false;

            //dr["DateRevised"] = DBNull.Value;
            //dr["DateCancelled"] = DBNull.Value;
            dr["DateCreated"] = DateTime.Now;
            dr["CreatedByID"] = LogIn.nUserID;
            dr["LastUpdate"] = DateTime.Now;
            dr["LastUserID"] = LogIn.nUserID;
            dtInvoice.Rows.Add(dr);
            bsInvoice.DataSource = dtInvoice;
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            txtCmpyCode.DataBindings.Add("Text", bsInvoice, "CompanyCode");
            txtInvNo.DataBindings.Add("Text", bsInvoice, "InvoiceNo");
            txtSponsorID.DataBindings.Add("Text", bsInvoice, "SponsorID");
            txtSponsor.DataBindings.Add("Text", bsInvoice, "SponsorName", true);
            txtContactID.DataBindings.Add("Text", bsInvoice, "ContactID");
            txtContact.DataBindings.Add("Text", bsInvoice, "ContactName", true);
            txtDateMailed.DataBindings.Add("Text", bsInvoice, "DateMailed", true);
            mskInvDate.DataBindings.Add("Text", bsInvoice, "InvoiceDate", true);
            txtMMode.DataBindings.Add("Text", bsInvoice, "MailMode", true);
            txtMailedBy.DataBindings.Add("Text", bsInvoice, "MailedBy", true);
            txtInvNotes.DataBindings.Add("Text", bsInvoice, "InvoiceNotes", true);
            txtIntNotes.DataBindings.Add("Text", bsInvoice, "NonPrintingNotes", true);
            chkPosted.DataBindings.Add("Checked", bsInvoice, "Posted", true);
            //chkExtraInvoice
            chkExtraInvoice.DataBindings.Add("Checked", bsInvoice, "ExtraInvoice", true);
            btnReplacePO.Enabled = false; dgvBillItems.ReadOnly = true; dgvOtherFees.ReadOnly = false;
            cboPO.Text = "PO";
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;

            if (dtSponsors.Rows.Count == 0)
                LoadSponsorsDDL();
            if (pnlRecord.Visible == false)
            {
                LoadData();
            }
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();
            OpenControls(pnlRecord, true);
            nMode = 2;
            btnReplacePO.Enabled = false; dgvBillItems.ReadOnly = true; dgvOtherFees.ReadOnly = false; txtPOAmt.ReadOnly = true;
            btnReplQuote.Enabled = true; btnEditPO.Enabled = true; btnAddItems.Enabled = true; btnAddOthFees.Enabled = true;
        }

        private void SaveRecord()
        {
            if (txtSponsorID.Text.Trim() == "")
            {
                MessageBox.Show("Please select Sponsor name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSponsorID.Select();
                return;
            }
            //if (txtContactID.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please select Contact name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    txtContact.Select();
            //    return;
            //}

            //if (cboPO.Text == "" && chkCancelled.Checked == false)
            //{
            //    MessageBox.Show("GIS would not accept a blank PO.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    cboPO.Select();
            //    return;
            //}
            //if (dtBillItems.Rows.Count == 0 && chkCancelled.Checked == false)
            //{
            //    MessageBox.Show("No billable items found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    cboPO.Select();
            //    return;
            //}
            UpdateInvMaster();
            UpdateInvBillItems();
            string strInvNo = txtInvNo.Text;
            nMode = 0;
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("InvoiceNo", strInvNo, bsFile, dgvFile);
            LoadData();
            txtInvNo.Focus();
        }

        private void UpdateInvMaster()
        {
            if (nMode == 1)
            {
                txtCmpyCode.Text = "P";
                txtInvNo.Text = PSSClass.General.NewInvNo("Invoices", "InvoiceNo").ToString();
            }

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            //SqlDataAdapter da = new SqlDataAdapter("SELECT CompanyCode, InvoiceNo, InvoiceDate, InvoiceType, SponsorID, ContactID, " +
            //                                       "InvoiceNotes, NonPrintingNotes, DateRevised, RevisedByID, DateCreated, CreatedByID, LastUpdate, LastUserID " +
            //                                       "FROM Invoices", sqlcnn);
            //chkExtraInvoice
            SqlDataAdapter da = new SqlDataAdapter("SELECT CompanyCode, InvoiceNo, InvoiceDate, InvoiceType, SponsorID, ContactID, " +
                                                   "InvoiceNotes, NonPrintingNotes, DateRevised, RevisedByID, DateCreated, CreatedByID, LastUpdate, LastUserID, ExtraInvoice " +
                                                   "FROM Invoices", sqlcnn);

            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);

            if (nMode == 1)
            {
                cmdBuilder.GetInsertCommand();
                dtInvoice.Rows[bsInvoice.Position]["CompanyCode"] = txtCmpyCode.Text;
                dtInvoice.Rows[bsInvoice.Position]["InvoiceNo"] = Convert.ToInt32(txtInvNo.Text);
                bsInvoice.EndEdit();
                da.Update(dtInvoice);
            }
            else
            {
                bsInvoice.EndEdit();
                cmdBuilder.GetUpdateCommand();
                da.Update(dtInvoice);
            }
            da.Dispose(); cmdBuilder.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
        }

        private void UpdateInvBillItems()
        {
            bsBillItems.EndEdit(); bsOtherFees.EndEdit();
            //DataTable dt = new DataTable();

            //dt.Columns.Add("InvoiceNo", typeof(string));
            //dt.Columns.Add("InvoiceID", typeof(int));
            //dt.Columns.Add("QuotationNo", typeof(string));
            //dt.Columns.Add("RevisionNo", typeof(Int16));
            //dt.Columns.Add("ControlNo", typeof(Int16));
            //dt.Columns.Add("PONo", typeof(string));
            //dt.Columns.Add("PSSNo", typeof(Int32));
            //dt.Columns.Add("ReportNo", typeof(Int32));
            //dt.Columns.Add("ServiceCode", typeof(Int16));
            //dt.Columns.Add("ServiceType", typeof(Int16));
            //dt.Columns.Add("BillQty", typeof(decimal));
            //dt.Columns.Add("UnitPrice", typeof(decimal));
            //dt.Columns.Add("Amount", typeof(decimal));
            //dt.Columns.Add("Rush", typeof(bool));
            //dt.Columns.Add("AccountID", typeof(string));
            //dt.Columns.Add("Adjustments", typeof(decimal));
            //dt.Columns.Add("DateCreated", typeof(DateTime));
            //dt.Columns.Add("CreatedByID", typeof(Int16));
            //dt.Columns.Add("LastUpdate", typeof(DateTime));
            //dt.Columns.Add("LastUserID", typeof(Int16));

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd;

            string strQN = "", strQNo= "", strRNo = ""; int nI = 0;
            for (int i = 0; i < dtBillItems.Rows.Count; i++)
            {
                if (nMode == 1 || dtBillItems.Rows[i].RowState.ToString() == "Added" || dtBillItems.Rows[i].RowState.ToString() == "Modified")
                {
                    strQN = dtBillItems.Rows[i]["QuoteNo"].ToString();
                    nI = strQN.IndexOf("R");
                    strQNo = strQN.Substring(0, nI - 1);
                    strRNo = strQN.Substring(nI + 1, strQN.Length - (nI + 1));

                    if (dtBillItems.Rows[i].RowState.ToString() == "Added")
                        nMode = 1;
                    else if (dtBillItems.Rows[i].RowState.ToString() == "Modified")
                        nMode = 2;
                    
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@nMode", nMode);
                    sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                    sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(txtInvNo.Text));
                    if (nMode == 1)
                        sqlcmd.Parameters.AddWithValue("@InvID", 0);
                    else
                        sqlcmd.Parameters.AddWithValue("@InvID", Convert.ToInt64(dtBillItems.Rows[i]["InvID"]));
                    sqlcmd.Parameters.AddWithValue("@QuoteNo", strQNo);
                    sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
                    sqlcmd.Parameters.AddWithValue("@CtrlNo", Convert.ToInt16(dtBillItems.Rows[i]["CtrlNo"]));
                    sqlcmd.Parameters.AddWithValue("@PONo", cboPO.Text);
                    sqlcmd.Parameters.AddWithValue("@PSSNo", Convert.ToInt32(dtBillItems.Rows[i]["LogNo"]));
                    if (dtBillItems.Rows[i]["RptNo"] == DBNull.Value)
                        sqlcmd.Parameters.AddWithValue("@RptNo", DBNull.Value);
                    else
                        sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(dtBillItems.Rows[i]["RptNo"]));
                    sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt32(dtBillItems.Rows[i]["SC"]));
                    sqlcmd.Parameters.AddWithValue("@SCType", 2);
                    sqlcmd.Parameters.AddWithValue("@BillQty", Convert.ToDecimal(dtBillItems.Rows[i]["BillQty"]));
                    sqlcmd.Parameters.AddWithValue("@UPrice", Convert.ToDecimal(dtBillItems.Rows[i]["UnitPrice"]));
                    sqlcmd.Parameters.AddWithValue("@Amt", Convert.ToDecimal(dtBillItems.Rows[i]["BillQty"]) * Convert.ToDecimal(dtBillItems.Rows[i]["UnitPrice"]));
                    sqlcmd.Parameters.AddWithValue("@PrePay", Convert.ToDecimal(dtBillItems.Rows[i]["Prepayments"]));
                    if (dtBillItems.Rows[i]["RushTest"].ToString() == "True")
                        sqlcmd.Parameters.AddWithValue("@RushTest", 1);
                    else
                        sqlcmd.Parameters.AddWithValue("@RushTest", 0);
                    sqlcmd.Parameters.AddWithValue("@Adj", 0);
                    sqlcmd.Parameters.AddWithValue("@QCmpyCode", dtBillItems.Rows[i]["QCmpyCode"].ToString());
                    sqlcmd.Parameters.AddWithValue("@LCmpyCode", dtBillItems.Rows[i]["LCmpyCode"].ToString());
                    sqlcmd.Parameters.AddWithValue("@RCmpyCode", dtBillItems.Rows[i]["RCmpyCode"].ToString());
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spAddEditInvDtls";
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sqlcmd.Dispose();
                }
            }
            //Other Fees
            for (int j = 0; j < dtOtherFees.Rows.Count; j++)
            {
                if (dtOtherFees.Rows[j].RowState.ToString() == "Added" || dtOtherFees.Rows[j].RowState.ToString() == "Modified")
                {
                    strQN = dtOtherFees.Rows[j]["QuotationNo"].ToString();
                    //if (strQN.Length > 0)
                    //{
                    //    nI = strQN.IndexOf("R");
                    //    strQNo = strQN.Substring(0, nI - 1);
                    //    strRNo = strQN.Substring(nI + 1, strQN.Length - (nI + 1));
                    //}

                    if (dtOtherFees.Rows[j].RowState.ToString() == "Added")
                        nMode = 1;
                    else if (dtOtherFees.Rows[j].RowState.ToString() == "Modified")
                        nMode = 2;

                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@nMode", nMode);
                    sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
                    sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(txtInvNo.Text));
                    if (nMode == 1)
                        sqlcmd.Parameters.AddWithValue("@InvID", 0);
                    else
                        sqlcmd.Parameters.AddWithValue("@InvID", Convert.ToInt64(dtOtherFees.Rows[j]["InvoiceID"]));

                    if (strQN.Length > 0)
                    {
                        sqlcmd.Parameters.AddWithValue("@QuoteNo", dtOtherFees.Rows[j]["QuotationNo"].ToString());
                        sqlcmd.Parameters.AddWithValue("@RevNo", dtOtherFees.Rows[j]["RevisionNo"]);
                        sqlcmd.Parameters.AddWithValue("@CtrlNo", dtOtherFees.Rows[j]["ControlNo"]);
                    }
                    else
                    {
                        sqlcmd.Parameters.AddWithValue("@QuoteNo", DBNull.Value);
                        sqlcmd.Parameters.AddWithValue("@RevNo", DBNull.Value);
                        sqlcmd.Parameters.AddWithValue("@CtrlNo", DBNull.Value);
                    }
                    sqlcmd.Parameters.AddWithValue("@PONo", cboPO.Text);
                    if (dtOtherFees.Rows[j]["PSSNo"] != DBNull.Value)
                        sqlcmd.Parameters.AddWithValue("@PSSNo", Convert.ToInt32(dtOtherFees.Rows[j]["PSSNo"]));
                    else
                        sqlcmd.Parameters.AddWithValue("@PSSNo", DBNull.Value);
                    if (dtOtherFees.Rows[j]["ReportNo"] != DBNull.Value)
                        sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(dtOtherFees.Rows[j]["ReportNo"]));
                    else
                        sqlcmd.Parameters.AddWithValue("@RptNo", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt32(dtOtherFees.Rows[j]["ServiceCode"]));
                    sqlcmd.Parameters.AddWithValue("@SCType", 3);
                    sqlcmd.Parameters.AddWithValue("@BillQty", Convert.ToDecimal(dtOtherFees.Rows[j]["BillQty"]));
                    sqlcmd.Parameters.AddWithValue("@UPrice", Convert.ToDecimal(dtOtherFees.Rows[j]["UnitPrice"]));
                    sqlcmd.Parameters.AddWithValue("@Amt", Convert.ToDecimal(dtOtherFees.Rows[j]["Amount"]));
                    sqlcmd.Parameters.AddWithValue("@PrePay", 0);
                    sqlcmd.Parameters.AddWithValue("@Adj", 0);
                    sqlcmd.Parameters.AddWithValue("@QCmpyCode", dtOtherFees.Rows[j]["QCmpyCode"].ToString());
                    sqlcmd.Parameters.AddWithValue("@LCmpyCode", dtOtherFees.Rows[j]["LCmpyCode"].ToString());
                    sqlcmd.Parameters.AddWithValue("@RCmpyCode", dtOtherFees.Rows[j]["RCmpyCode"].ToString());
                    sqlcmd.Parameters.AddWithValue("@RushTest", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spAddEditInvDtls";
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sqlcmd.Dispose();
                }

                //DataRow dR = dt.NewRow();
                //dR["InvoiceNo"] = Convert.ToInt32(txtInvNo.Text);
                //dR["InvoiceID"] = dtBillItems.Rows[i]["InvID"];
                //dR["QuotationNo"] = strQNo;
                //dR["RevisionNo"] = Convert.ToInt16(strRNo);
                //dR["PONo"] = cboPO.Text;
                //dR["ControlNo"] = dtBillItems.Rows[i]["CtrlNo"];
                //dR["PSSNo"] = dtBillItems.Rows[i]["LogNo"];
                //dR["ReportNo"] = dtBillItems.Rows[i]["RptNo"];
                //dR["ServiceCode"] = dtBillItems.Rows[i]["SC"];
                //dR["ServiceType"] = 2;
                //dR["BillQty"] = dtBillItems.Rows[i]["BillQty"];
                //dR["UnitPrice"] = dtBillItems.Rows[i]["UnitPrice"];
                //dR["Amount"] = dtBillItems.Rows[i]["AmtDue"];
                //dR["Rush"] = dtBillItems.Rows[i]["RushTest"];
                //dR["AccountID"] = "0";
                //dR["Adjustments"] = DBNull.Value;
                //dR["DateCreated"] = DateTime.Now;
                //dR["CreatedByID"] = LogIn.nUserID;
                //dR["LastUpdate"] = DateTime.Now;
                //dR["LastUserID"] = LogIn.nUserID;
                //dt.Rows.Add(dR);
            }
            //Delete Other Fees marked DELETED
            if (nListDelFees.Count > 0)
            {
                for (int i = 0; i < nListDelFees.Count; i++)
                {
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;
                    sqlcmd.Parameters.AddWithValue("@InvID", nListDelFees[i]);
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spDelOtherFee";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    sqlcmd.Dispose();
                }
            }
            //SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            //SqlDataAdapter da;
            //if (nMode == 1)
            //    da = new SqlDataAdapter("SELECT * FROM InvoiceDetails", sqlcnn);
            //else
            //    da = new SqlDataAdapter("SELECT * FROM InvoiceDetails WHERE InvoiceNo=" + Convert.ToInt32(txtInvNo.Text), sqlcnn);

            //SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);

            //if (nMode == 1)
            //{
            //    cmdBuilder.GetInsertCommand();
            //    da.Update(dt);
            //}
            //else
            //{
            //    cmdBuilder.GetUpdateCommand();
            //    da.Update(dt);
            //}

            //dt.Rows.Clear();

            //if (nMode == 1)
            //{
            //    cmdBuilder.GetInsertCommand();
            //    da.Update(dt);
            //}
            //else
            //{
            //    dt.AcceptChanges();
            //    cmdBuilder.GetUpdateCommand();
            //    da.Update(dt);
            //}
            //dt.Dispose();
            //da.Dispose(); cmdBuilder.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
        }

        private void CancelSave()
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
            if (nFB == 1 || nFB == 2)
            {
                nFB = 0;
                SendKeys.Send("{F12}");
                return;
            }
            bsInvoice.CancelEdit();
            ClearControls(pnlRecord);
            AddEditMode(false);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true; dgvSponsors.Visible = false; dgvContacts.Visible = false;
            nMode = 0;
        }

        private void LoadData()
        {
            nMode = 0;
            btnReplacePO.Enabled = true; btnReplQuote.Enabled = false; btnEditPO.Enabled = false; btnAddItems.Enabled = false; btnAddOthFees.Enabled = false;
            OpenControls(pnlRecord, false);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            dgvBillItems.Enabled = true; dgvBillSummary.Enabled = true; dgvOtherFees.Enabled = true; dgvPrepayments.Enabled = true;
            dtInvoice.Rows.Clear();
            txtCmpyCode.Text = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["CompanyCode"].Value.ToString();
            txtInvNo.Text = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["InvoiceNo"].Value.ToString();
            LoadInvoice();
            LoadBillItems();
            LoadPrepayments();
            LoadBillSummary();
            LoadOtherFees();
            //Check if Invoice is Paid
            picPaid.Visible = false; lblDatePaid.Visible = false;
            DataTable dt = PSSClass.FinalBilling.InvPaid(Convert.ToInt32(txtInvNo.Text));
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["AmountPaid"].ToString() != "" && Convert.ToDecimal(dt.Rows[0]["AmountPaid"]) * (-1) >= Convert.ToDecimal(txtInvTotal.Text.Replace("$", "")))
                {
                    picPaid.Visible = true; lblDatePaid.Visible = true; lblDatePaid.Text = dt.Rows[0]["DatePaid"].ToString();
                }
                else
                {
                    picPaid.Visible = false; lblDatePaid.Visible = false;
                }
                dt.Dispose();
            }
            btnClose.Visible = true; 
        }

        private void LoadRecords()
        {
            nMode = 0;
            DataTable dtQ = PSSClass.OtherBillings.OtherBillMaster(1);
            bsFile.DataSource = dtQ;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "InvoiceNo<>0";
            DataGridSetting();
            if (tsddbSearch.DropDownItems.Count == 0)
            {
                //int ndx = 0;
                int i = 0;
                int n = 0;

                arrCol = new string[dtQ.Columns.Count];

                //foreach (DataColumn colFile in sqlds.Tables["Sponsors"].Columns)
                //{
                //    ndx = colFile.ColumnName.IndexOf("ID"); //search for the existence of the word "ID" in the field name
                //    if (ndx != -1)
                //    {
                //        n += 1;
                //    }
                //}

                ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

                foreach (DataColumn colFile in dtQ.Columns)
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
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["CompanyCode"].HeaderText = "CMPY CODE";
            dgvFile.Columns["CompanyCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["InvoiceNo"].HeaderText = "INVOICE NO.";
            dgvFile.Columns["InvoiceNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["InvoiceDate"].HeaderText = "DATE";
            dgvFile.Columns["InvoiceDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["InvoiceDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateMailed"].HeaderText = "DATE MAILED";
            dgvFile.Columns["DateMailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateMailed"].Width = 120;
            dgvFile.Columns["MailMode"].HeaderText = "MAIL MODE";
            dgvFile.Columns["MailMode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["ContactName"].HeaderText = "CONTACT NAME";
            dgvFile.Columns["SponsorName"].Width = 300;
            dgvFile.Columns["ContactName"].Width = 200;
            dgvFile.Columns["InvoiceType"].Visible = false;
            dgvFile.Columns["ContactID"].Visible = false;
            dgvFile.Columns["InvoiceNotes"].Visible = false;
            dgvFile.Columns["NonPrintingNotes"].Visible = false;
            //chkExtraInvoice
            dgvFile.Columns["ExtraInvoice"].Visible = false;
            dgvFile.Columns[0].Frozen = true;
        }

        private void LoadSponsorsDDL()
        {
            dgvSponsors.DataSource = null;

            dtSponsors = PSSClass.Sponsors.SponsorNamesDDL();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvSponsors.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
        }

        private void LoadContactsDDL(int cSpID)
        {
            dgvContacts.DataSource = null;
            //dtContacts = PSSClass.FinalBilling.ContactsDDLInv(cSpID);
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

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSponsors.TabIndex = 3; dgvContacts.Visible = false;
            }
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 27)
                    dgvSponsors.Visible = false;
                else if (e.KeyChar == 13)
                {
                    dgvSponsors.Select();
                }
                txtSponsorID.Text = ""; txtContactID.Text = ""; txtContact.Text = ""; lnkPOPDF.Text = "PO (PDF)"; cboPO.DataSource = null; txtPOAmt.Text = "";
            }
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSponsors;
                if (chkWildSpCon.Checked == true)
                    dvwSponsors = new DataView(dtSponsors, "SponsorName like '%" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
                else
                    dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);

                PSSClass.General.DGVSetUp(dgvSponsors, dvwSponsors, 369);
            }
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            txtContactID.Text = ""; txtContact.Text = ""; dgvContacts.DataSource = null;
            dgvSponsors.Visible = false; lnkPOPDF.Text = "PO (PDF)";
            LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
            txtContact.Focus();
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
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                txtContactID.Text = ""; txtContact.Text = ""; dgvContacts.DataSource = null;
                LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                txtContact.Focus();
                dgvSponsors.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvSponsors.Visible = false;
            }
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false; dgvContacts.Visible = false;
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    if (txtSponsorID.Text.Trim() != "")
                    {
                        txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                        LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                        dgvSponsors.Visible = false; txtContact.Focus();
                    }
                }
                else if (e.KeyChar == 27)
                {
                    dgvSponsors.Visible = false;
                }
                else
                {
                    txtSponsor.Text = ""; txtContactID.Text = ""; txtContact.Text = ""; dgvContacts.Visible = false;
                }
            }
        }

        private void txtSponsorID_Leave(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (txtSponsorID.Text.Trim() != "")
                {
                    txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                    LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    dgvSponsors.Visible = false;
                }
            }
        }

        private void txtContact_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    dgvContacts.Visible = true; dgvContacts.BringToFront(); dgvSponsors.Visible = false;
                }
                catch { }
            }
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                    try
                    {
                        txtContact.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
                        dgvContacts.Visible = false; txtInvNotes.Select();
                    }
                    catch { }
                else if (e.KeyChar == 27)
                    dgvContacts.Visible = false;
                else
                    txtContactID.Text = "";
            }
        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    DataView dvwContacts;
                    if (chkWildSpCon.Checked == true)
                        dvwContacts = new DataView(dtContacts, "Contact like '%" + txtContact.Text.Trim() + "%'", "Contact", DataViewRowState.CurrentRows);
                    else
                        dvwContacts = new DataView(dtContacts, "Contact like '" + txtContact.Text.Trim() + "%'", "Contact", DataViewRowState.CurrentRows);

                    PSSClass.General.DGVSetUp(dgvContacts, dvwContacts, 369);
                }
                catch { }

            }
        }

        private void dgvContacts_DoubleClick(object sender, EventArgs e)
        {
            if (dgvContacts.Rows.Count == 0)
            {
                MessageBox.Show("Contacts list is empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsorID.Focus();
                return;
            }
            txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
            txtContactID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
            dgvContacts.Visible = false; dgvSponsors.Visible = false;
            cboPO.DataSource = null;
            dtBillItems.Rows.Clear(); dtBillSummary.Rows.Clear(); dtOtherFees.Rows.Clear(); dtPrepayments.Rows.Clear();
            //LoadPO();
            cboPO.Select();
        }

        private void LoadPO()
        {
            DataTable dt = PSSClass.FinalBilling.FinServicesPO(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtContactID.Text));
            cboPO.DataSource = dt;
            cboPO.DisplayMember = "PONo";
            cboPO.ValueMember = "PONo";
            if (dt.Rows.Count != 0)
            {
                cboPO.SelectedIndex = 0;
                lnkPOPDF.Text = PSSClass.FinalBilling.ExPOPDF(Convert.ToInt16(txtSponsorID.Text), cboPO.Text);
                LoadBillItems();
                if (dtBillItems == null || dtBillItems.Rows.Count == 0)
                {
                    MessageBox.Show("Report(s) are not available or not e-mailed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    CancelSave();
                    return;
                }   
                LoadPrepayments();
                LoadBillSummary();
                LoadOtherFees();
            }
        }

        private void LoadInvoice()
        {
            try
            {
                foreach (Control c in pnlRecord.Controls)
                {
                    c.DataBindings.Clear();
                }

                dtInvoice = PSSClass.FinalBilling.LoadInvMasterRec(txtCmpyCode.Text, Convert.ToInt32(txtInvNo.Text));
                bsInvoice.DataSource = dtInvoice;

                txtInvNo.DataBindings.Add("Text", bsInvoice, "InvoiceNo");
                txtSponsorID.DataBindings.Add("Text", bsInvoice, "SponsorID");
                txtSponsor.DataBindings.Add("Text", bsInvoice, "SponsorName", true);
                txtContactID.DataBindings.Add("Text", bsInvoice, "ContactID");
                txtContact.DataBindings.Add("Text", bsInvoice, "ContactName", true);
                txtDateMailed.DataBindings.Add("Text", bsInvoice, "DateMailed", true);
                txtDateRev.DataBindings.Add("Text", bsInvoice, "DateRevised", true);
                txtRevByID.DataBindings.Add("Text", bsInvoice, "RevisedByID", true);
                txtDateCancelled.DataBindings.Add("Text", bsInvoice, "DateCancelled", true);
                txtMMode.DataBindings.Add("Text", bsInvoice, "MailMode", true);
                txtMailedBy.DataBindings.Add("Text", bsInvoice, "MailedBy", true);
                txtInvNotes.DataBindings.Add("Text", bsInvoice, "InvoiceNotes", true);
                txtIntNotes.DataBindings.Add("Text", bsInvoice, "NonPrintingNotes", true);
                chkPosted.DataBindings.Add("Checked", bsInvoice, "Posted", true);
                //chkExtraInvoice
                chkExtraInvoice.DataBindings.Add("Checked", bsInvoice, "ExtraInvoice", true);
                Binding InvDateBinding;
                InvDateBinding = new Binding("Text", bsInvoice, "InvoiceDate");
                InvDateBinding.Format += new ConvertEventHandler(DateBinding_Format);
                mskInvDate.DataBindings.Add(InvDateBinding);

                cboPO.Text = PSSClass.FinalBilling.ExInvPO(Convert.ToInt32(txtInvNo.Text));
                lnkPOPDF.Text = PSSClass.FinalBilling.ExPOPDF(Convert.ToInt16(txtSponsorID.Text), cboPO.Text);
                txtPOAmt.Text = PSSClass.FinalBilling.ExPOBalance(Convert.ToInt16(txtSponsorID.Text), cboPO.Text).ToString("$#,##0.00");
                if (txtDateCancelled.Text != "")
                    chkCancelled.Checked = true;
                else
                    chkCancelled.Checked = false;
                if (txtDateRev.Text != "")
                    chkRevised.Checked = true;
                else
                    chkRevised.Checked = false;

                decimal nAmt = PSSClass.FinalBilling.ExPOAmount(Convert.ToInt16(txtSponsorID.Text), cboPO.Text);
                if ( nAmt == 0)
                {
                    lnkPOPDF.Text = "PO information is missing or incomplete.";
                }
            }
            catch { }
        }

        private void DateBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "__/__/____";
        }

        private void LoadBillItems()
        {
            dtBillItems.Rows.Clear();
            try
            {
                if (nMode == 1)
                {
                    dtBillItems = PSSClass.FinalBilling.FinBillServices(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtContactID.Text), cboPO.Text);
                }
                else
                {
                    dtBillItems = PSSClass.FinalBilling.LoadInvServiceFees(Convert.ToInt32(txtInvNo.Text));
                }
                bsBillItems.DataSource = dtBillItems;
                bnBillItems.BindingSource = bsBillItems;
                dgvBillItems.DataSource = bsBillItems;
                BillItemsGridSetting();

                //if (nMode == 1)
                //{
                    //cboPO.Text = dtBillItems.Rows[0]["PONo"].ToString();
                    //txtPOAmt.Text = PSSClass.FinalBilling.ExPOBalance(Convert.ToInt16(txtSponsorID.Text), cboPO.Text).ToString("$#,##0.00");
                //}
            }
            catch { }
        }

        private void BillItemsGridSetting()
        {
            dgvBillItems.EnableHeadersVisualStyles = false;
            dgvBillItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillItems.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvBillItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvBillItems.Columns["RptNo"].HeaderText = "REPORT NO.";
            dgvBillItems.Columns["LogNo"].HeaderText = "PSS NO.";
            dgvBillItems.Columns["SC"].HeaderText = "SERVICE CODE";
            dgvBillItems.Columns["QuoteNo"].HeaderText = "QUOTE NO.";
            dgvBillItems.Columns["TestDesc"].HeaderText = "TEST DESCRIPTION";
            dgvBillItems.Columns["RushTest"].HeaderText = "RUSH";
            dgvBillItems.Columns["BillQty"].HeaderText = "QUANTITY";
            dgvBillItems.Columns["UnitPrice"].HeaderText = "UNIT PRICE";
            dgvBillItems.Columns["Prepayments"].HeaderText = "PREPAYMENTS";
            dgvBillItems.Columns["AmtDue"].HeaderText = "AMOUNT DUE";
            dgvBillItems.Columns["RptNo"].Width = 70;
            dgvBillItems.Columns["RptNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillItems.Columns["LogNo"].Width = 70;
            dgvBillItems.Columns["LogNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillItems.Columns["SC"].Width = 70;
            dgvBillItems.Columns["SC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillItems.Columns["QuoteNo"].Width = 100;
            dgvBillItems.Columns["QuoteNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillItems.Columns["TestDesc"].Width = 415;
            dgvBillItems.Columns["RushTest"].Width = 50;
            dgvBillItems.Columns["BillQty"].Width = 75;
            dgvBillItems.Columns["BillQty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBillItems.Columns["BillQty"].DefaultCellStyle.Format = "#,##0.00";
            dgvBillItems.Columns["UnitPrice"].Width = 75;
            dgvBillItems.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBillItems.Columns["UnitPrice"].DefaultCellStyle.Format = "$#,##0.00";
            dgvBillItems.Columns["Prepayments"].Width = 100;
            dgvBillItems.Columns["Prepayments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBillItems.Columns["Prepayments"].DefaultCellStyle.Format = "$#,##0.00";
            dgvBillItems.Columns["AmtDue"].Width = 90;
            dgvBillItems.Columns["AmtDue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBillItems.Columns["AmtDue"].DefaultCellStyle.Format = "$#,##0.00";
            dgvBillItems.Columns["InvID"].Visible = false;
            dgvBillItems.Columns["CtrlNo"].Visible = false;
            dgvBillItems.Columns["RushFee"].Visible = false;
            dgvBillItems.Columns["PONo"].Visible = false;
            dgvBillItems.Columns["QuotationNo"].Visible = false;
            dgvBillItems.Columns["RevisionNo"].Visible = false;
            dgvBillItems.Columns["CtrldSubs"].Visible = false;
            dgvBillItems.Columns["CtrldSubsOrig"].Visible = false;
        }

        private void PrepaymentGridSetting()
        {
            dgvPrepayments.EnableHeadersVisualStyles = false;
            dgvPrepayments.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPrepayments.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvPrepayments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPrepayments.Columns["InvoiceNo"].Width = 68;
            dgvPrepayments.Columns["InvoiceNo"].HeaderText = "INV. NO.";
            dgvPrepayments.Columns["InvoiceNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPrepayments.Columns["InvoiceDate"].Width = 75;
            dgvPrepayments.Columns["InvoiceDate"].HeaderText = "DATE";
            dgvPrepayments.Columns["InvoiceDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPrepayments.Columns["InvoiceDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvPrepayments.Columns["InvoiceDate"].Width = 75;
            dgvPrepayments.Columns["ServiceCode"].Width = 75;
            dgvPrepayments.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvPrepayments.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPrepayments.Columns["ServiceDesc"].Width = 315;
            dgvPrepayments.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvPrepayments.Columns["ServiceDesc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvPrepayments.Columns["AmtDue"].Width = 90;
            dgvPrepayments.Columns["AmtDue"].HeaderText = "AMOUNT";
            dgvPrepayments.Columns["AmtDue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPrepayments.Columns["AmtDue"].DefaultCellStyle.Format = "$#,##0.00";
            dgvPrepayments.Columns["InvoiceNo"].Frozen = true;
        }


        private void LoadOtherFees()
        {
            dtOtherFees.Rows.Clear();
            if (txtSponsor.Text.IndexOf("INGREDION") == -1)
            {
                try
                {
                    if (nMode != 0)
                    {
                        if (dgvBillItems.Rows.Count > 0)
                        {
                            string strRptNo = "";

                            //string strSC = dgvBillItems.Rows[0].Cells["ServiceCode"].Value.ToString();

                            for (int i = 0; i < dgvBillItems.Rows.Count; i++)
                            {
                                if (dgvBillItems.Rows[i].Cells["RptNo"].Value.ToString() != strRptNo)
                                {
                                    strRptNo = dgvBillItems.Rows[i].Cells["RptNo"].Value.ToString();
                                    DataTable dt = new DataTable();
                                    dt = PSSClass.FinalBilling.FinRptOtherFees(Convert.ToInt32(dgvBillItems.Rows[i].Cells["RptNo"].Value));
                                    if (dt != null)
                                    {
                                        for (int j = 0; j < dt.Rows.Count; j++)
                                        {
                                            DataRow dR = dtOtherFees.NewRow();

                                            dR["ReportNo"] = dt.Rows[j]["ReportNo"];
                                            dR["ServiceCode"] = dt.Rows[j]["ServiceCode"];
                                            dR["ServiceDesc"] = dt.Rows[j]["ServiceDesc"];
                                            dR["TestDesc1"] = dt.Rows[j]["TestDesc1"];
                                            dR["BillQty"] = dt.Rows[j]["BillQty"];
                                            dR["UnitPrice"] = dt.Rows[j]["UnitPrice"];
                                            dR["Amount"] = dt.Rows[j]["Amount"];
                                            dR["QuotationNo"] = dt.Rows[j]["QuotationNo"];
                                            dR["RevisionNo"] = dt.Rows[j]["RevisionNo"];
                                            dR["ControlNo"] = dt.Rows[j]["ControlNo"];
                                            dR["QCmpyCode"] = dt.Rows[j]["QCmpyCode"];
                                            dR["LCmpyCode"] = dt.Rows[j]["LCmpyCode"];
                                            dR["RCmpyCode"] = dt.Rows[j]["RCmpyCode"];
                                            dtOtherFees.Rows.Add(dR);
                                        }
                                        dt.Rows.Clear();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        dtOtherFees = PSSClass.FinalBilling.LoadInvOtherFees(Convert.ToInt32(txtInvNo.Text));
                        dtOtherFees.AcceptChanges();
                    }
                }
                catch { }
            }
            else
            {
                try
                {
                    if (nMode != 0)
                    {
                        for (int i = 0; i < dgvBillSummary.Rows.Count; i++)
                        {
                            DataRow dR;
                            if (dgvBillSummary.Rows[i].Cells[0].Value.ToString() != "")
                            {
                                dR = dtOtherFees.NewRow();

                                dR["ReportNo"] = Convert.ToInt32(dgvBillSummary.Rows[i].Cells[0].Value);
                                dR["ServiceCode"] = 553;
                                dR["ServiceDesc"] = "Electronic Reporting Fee";
                                dR["TestDesc1"] = "Electronic Reporting Fee";
                                dR["BillQty"] = 1;
                                dR["UnitPrice"] = 42.19;
                                dR["Amount"] = 42.19;
                                dR["QuotationNo"] = "2015.1403";
                                dR["RevisionNo"] = 0;
                                dR["ControlNo"] = 24;
                                dtOtherFees.Rows.Add(dR);
                            }
                            int nBQty = 0;
                            try
                            {
                                nBQty = Convert.ToInt16(PSSClass.Samples.SlashSCLast(Convert.ToInt32(dgvBillSummary.Rows[i].Cells[1].Value), Convert.ToInt16(dgvBillSummary.Rows[i].Cells[2].Value)));
                            }
                            catch { }
                            if (nBQty != 0 && Convert.ToInt16(dgvBillSummary.Rows[i].Cells["SC"].Value) != 46)
                            {
                                dR = dtOtherFees.NewRow();
                                dR["ReportNo"] = Convert.ToInt32(dgvBillSummary.Rows[i].Cells[0].Value);
                                dR["ServiceCode"] = 554;
                                dR["ServiceDesc"] = "Sample Storage and Disposal Fee";
                                dR["TestDesc1"] = "Sample Storage and Disposal Fee";
                                dR["BillQty"] = nBQty;
                                dR["UnitPrice"] = 1.56;
                                dR["Amount"] = nBQty * 1.56;
                                dR["QuotationNo"] = "2015.1403";
                                dR["RevisionNo"] = 0;
                                dR["ControlNo"] = 25;
                                dtOtherFees.Rows.Add(dR);
                            }
                        }
                    }
                    else
                    {
                        dtOtherFees = PSSClass.FinalBilling.LoadInvOtherFees(Convert.ToInt32(txtInvNo.Text));
                        dtOtherFees.AcceptChanges();
                    }
                }
                catch { }
            }
            bsOtherFees.DataSource = dtOtherFees;
            bnOtherFees.BindingSource = bsOtherFees;
            dgvOtherFees.DataSource = bsOtherFees;
            OtherFeesGridSetting();

            //Replace Test Description with Service Description
            for (int i = 0; i < dgvOtherFees.Rows.Count; i++)
            {
                dgvOtherFees.Rows[i].Cells["TestDesc1"].Value = dgvOtherFees.Rows[i].Cells["ServiceDesc"].Value;
            }
            //

            decimal nOth = 0;
            for (int i = 0; i < dtOtherFees.Rows.Count; i++)
            {
                nOth += Convert.ToDecimal(dtOtherFees.Rows[i]["Amount"]);
            }
            txtOtherFees.Text = nOth.ToString("$#,###0.00");
            try
            {
                decimal nTotal = 0;
                nTotal = Convert.ToDecimal(txtOtherFees.Text.Replace("$", "")) + Convert.ToDecimal(txtServiceFees.Text.Replace("$", ""));// -Convert.ToDecimal(txtPP.Text.Replace("$", ""));
                txtInvTotal.Text = nTotal.ToString("$#,##0.00");
            }
            catch { }
        }

        private void OtherFeesGridSetting()
        {
            dgvOtherFees.EnableHeadersVisualStyles = false;
            dgvOtherFees.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOtherFees.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvOtherFees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvOtherFees.Columns["InvoiceID"].HeaderText = "INV. ID";
            dgvOtherFees.Columns["ReportNo"].HeaderText = "REPORT NO.";
            dgvOtherFees.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvOtherFees.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvOtherFees.Columns["BillQty"].HeaderText = "QUANTITY";
            dgvOtherFees.Columns["UnitPrice"].HeaderText = "UNIT PRICE";
            dgvOtherFees.Columns["Amount"].HeaderText = "AMOUNT DUE";
            dgvOtherFees.Columns["ReportNo"].Width = 80;
            dgvOtherFees.Columns["ReportNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOtherFees.Columns["ServiceCode"].Width = 70;
            dgvOtherFees.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvOtherFees.Columns["ServiceDesc"].Width = 190;
            dgvOtherFees.Columns["TestDesc1"].HeaderText = "TEST DESCRIPTION";
            dgvOtherFees.Columns["TestDesc1"].Width = 190;
            dgvOtherFees.Columns["BillQty"].Width = 75;
            dgvOtherFees.Columns["BillQty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOtherFees.Columns["BillQty"].DefaultCellStyle.Format = "#,##0.00";
            dgvOtherFees.Columns["UnitPrice"].Width = 75;
            dgvOtherFees.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOtherFees.Columns["UnitPrice"].DefaultCellStyle.Format = "$#,##0.00";
            dgvOtherFees.Columns["Amount"].Width = 90;
            dgvOtherFees.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvOtherFees.Columns["Amount"].DefaultCellStyle.Format = "$#,##0.00";
            dgvOtherFees.Columns["InvoiceID"].Visible = false;
            dgvOtherFees.Columns["PSSNo"].Visible = false;
            dgvOtherFees.Columns["ServiceDesc"].Visible = false;
            dgvOtherFees.Columns["QuotationNo"].Visible = false;
            dgvOtherFees.Columns["RevisionNo"].Visible = false;
            dgvOtherFees.Columns["ControlNo"].Visible = false;
            dgvOtherFees.Columns["QCmpyCode"].Visible = false;
            dgvOtherFees.Columns["LCmpyCode"].Visible = false;
            dgvOtherFees.Columns["RCmpyCode"].Visible = false;
        }

        private void LoadBillSummary()
        {
            dtBillSummary.Rows.Clear();
            if (dtBillItems.Rows.Count != 0)
            {
                List<string> strListQ = new List<string>();
                string strQ = ""; int nRptNo = 0; int nRptSv = 0; int nPSSNo = 0; int nSC = 0;
                decimal dAmt = 0;
                for (int i = 0; i < dtBillItems.Rows.Count; i++)
                {

                    if (dtBillItems.Rows[i]["RptNo"].ToString() != "")
                        nRptNo = Convert.ToInt32(dtBillItems.Rows[i]["RptNo"]);
                    else
                        nRptNo = 0;
                    if (nRptSv != nRptNo)
                    {
                        if (dAmt != 0)
                        {
                            DataRow dR = dtBillSummary.NewRow();
                            dR["ReportNo"] = nRptSv; 
                            dR["PSSNo"] = nPSSNo;
                            dR["SC"] = nSC;
                            dR["SCDesc"] = PSSClass.ServiceCodes.SCDesc(nSC, dtSC);
                            dR["Amount"] = dAmt;
                            dtBillSummary.Rows.Add(dR);
                            dAmt = 0;
                        }
                        nRptSv = nRptNo;
                        nPSSNo = Convert.ToInt32(dtBillItems.Rows[i]["LogNo"]);
                        nSC = Convert.ToInt32(dtBillItems.Rows[i]["SC"]);
                    }
                    else if (nPSSNo != Convert.ToInt32(dtBillItems.Rows[i]["LogNo"]))
                    {
                        if (dAmt != 0)
                        {
                            DataRow dR = dtBillSummary.NewRow();
                            dR["ReportNo"] = nRptSv; 
                            dR["PSSNo"] = nPSSNo;
                            dR["SC"] = nSC;
                            dR["SCDesc"] = PSSClass.ServiceCodes.SCDesc(nSC, dtSC);
                            dR["Amount"] = dAmt;
                            dtBillSummary.Rows.Add(dR);
                            dAmt = 0;
                        }
                        nPSSNo = Convert.ToInt32(dtBillItems.Rows[i]["LogNo"]);
                        nSC = Convert.ToInt32(dtBillItems.Rows[i]["SC"]);
                    }
                    if (nSC != Convert.ToInt32(dtBillItems.Rows[i]["SC"]))
                    {
                        if (dAmt != 0)
                        {
                            DataRow dR = dtBillSummary.NewRow();
                            dR["ReportNo"] = nRptSv; 
                            dR["PSSNo"] = nPSSNo;
                            dR["SC"] = nSC;
                            dR["SCDesc"] = PSSClass.ServiceCodes.SCDesc(nSC, dtSC);
                            dR["Amount"] = dAmt;
                            dtBillSummary.Rows.Add(dR);
                            dAmt = 0;
                        }
                        nSC = Convert.ToInt32(dtBillItems.Rows[i]["SC"]);
                    }
                    if (strQ != dtBillItems.Rows[i]["QuoteNo"].ToString())
                    {
                        strListQ.Add(dtBillItems.Rows[i]["QuoteNo"].ToString());
                        strQ = dtBillItems.Rows[i]["QuoteNo"].ToString();
                    }
                    if (dtBillItems.Rows[i]["AmtDue"] != null && dtBillItems.Rows[i]["AmtDue"].ToString() != "")
                        dAmt += Convert.ToDecimal(dtBillItems.Rows[i]["AmtDue"]);
                }
                if (dAmt != 0)
                {
                    DataRow dRow = dtBillSummary.NewRow();
                    dRow["ReportNo"] = nRptSv; 
                    dRow["PSSNo"] = nPSSNo;
                    dRow["SC"] = nSC;
                    dRow["SCDesc"] = PSSClass.ServiceCodes.SCDesc(nSC, dtSC);
                    dRow["Amount"] = dAmt;
                    dtBillSummary.Rows.Add(dRow);
                }
                for (int i = 0; i < strListQ.Count; i++)
                {
                    string strQN = strListQ[i];
                    int nI = strQN.IndexOf("R");
                    string strQNo = strQN.Substring(0, nI - 1);
                    string strRNo = strQN.Substring(nI + 1, strQN.Length - (nI + 1));
                }
            }
            bsBillSummary.DataSource = dtBillSummary;
            dgvBillSummary.DataSource = bsBillSummary;
            decimal nTotal = 0;
            for (int i = 0; i < dtBillSummary.Rows.Count; i++)
            {
                nTotal += Convert.ToDecimal(dtBillSummary.Rows[i]["Amount"]);
            }
            txtServiceFees.Text = nTotal.ToString("$#,###0.00");
            try
            {
                decimal nTot = 0;
                nTot = Convert.ToDecimal(txtOtherFees.Text.Replace("$", "")) + Convert.ToDecimal(txtServiceFees.Text.Replace("$", ""));// -Convert.ToDecimal(txtPP.Text.Replace("$", ""));
                txtInvTotal.Text = nTot.ToString("$#,##0.00");
            }
            catch { }
        }

        private void BillSummaryGidSetting()
        {
            dgvBillSummary.EnableHeadersVisualStyles = false;
            dgvBillSummary.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillSummary.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvBillSummary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBillSummary.Columns["ReportNo"].Width = 70;
            dgvBillSummary.Columns["ReportNo"].HeaderText = "REPORT NO.";
            dgvBillSummary.Columns["ReportNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillSummary.Columns["PSSNo"].Width = 70;
            dgvBillSummary.Columns["PSSNo"].HeaderText = "PSS NO.";
            dgvBillSummary.Columns["PSSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillSummary.Columns["SC"].Width = 70;
            dgvBillSummary.Columns["SC"].HeaderText = "SERVICE CODE";
            dgvBillSummary.Columns["SC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillSummary.Columns["SCDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvBillSummary.Columns["SCDesc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvBillSummary.Columns["SCDesc"].Width = 168;
            dgvBillSummary.Columns["Amount"].Width = 90;
            dgvBillSummary.Columns["Amount"].HeaderText = "AMOUNT DUE";
            dgvBillSummary.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBillSummary.Columns["Amount"].DefaultCellStyle.Format = "$#,##0.00";
        }

        private void LoadPrepayments()
        {
            dtPrepayments.Rows.Clear();
            DataTable dt = new DataTable();

            if (dgvBillItems.Rows.Count > 0)
            {
                string strQNo = "";
                for (int i = 0; i < dgvBillItems.Rows.Count; i++)
                {
                    if (strQNo != dgvBillItems.Rows[i].Cells["QuoteNo"].Value.ToString())
                    {
                        strQNo = dgvBillItems.Rows[i].Cells["QuoteNo"].Value.ToString();
                        int nI = strQNo.IndexOf("R");
                        string strQN = strQNo.Substring(0, nI - 1);
                        string strRNo = strQNo.Substring(nI + 1, strQNo.Length - (nI + 1));

                        dt = PSSClass.FinalBilling.InvPrepay(dgvBillItems.Rows[i].Cells["QCmpyCode"].Value.ToString(), strQN, Convert.ToInt16(strRNo));
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                if (Convert.ToDecimal(dt.Rows[j]["AmtDue"]) != 0 && dt.Rows[j]["ServiceCode"].ToString() == dgvBillItems.Rows[i].Cells["SC"].Value.ToString())
                                {
                                    DataRow dr = dtPrepayments.NewRow();
                                    dr["InvoiceNo"] = dt.Rows[j]["InvoiceNo"];
                                    dr["InvoiceDate"] = dt.Rows[j]["InvoiceDate"];
                                    dr["ServiceCode"] = dt.Rows[j]["ServiceCode"];
                                    dr["ServiceDesc"] = dt.Rows[j]["ServiceDesc"];
                                    dr["AmtDue"] = dt.Rows[j]["AmtDue"];
                                    dtPrepayments.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }
                dgvPrepayments.DataSource = dtPrepayments;
                PrepaymentGridSetting();
                decimal nPP = 0;
                for (int i = 0; i < dtPrepayments.Rows.Count; i++)
                {
                    nPP += Convert.ToDecimal(dtPrepayments.Rows[i]["AmtDue"]);
                }
                txtPP.Text = nPP.ToString("$#,###0.00");
                try
                {
                    decimal nTot = 0;
                    nTot = Convert.ToDecimal(txtOtherFees.Text.Replace("$", "")) + Convert.ToDecimal(txtServiceFees.Text.Replace("$", "")); // - Convert.ToDecimal(txtPP.Text.Replace("$", ""));
                    txtInvTotal.Text = nTot.ToString("$#,##0.00");
                }
                catch { }
            }
        }

        private void pnlRecord_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
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

        private void pnlRecord_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void dgvBillItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvBillItems.IsCurrentCellDirty)
            //    dgvBillItems.CommitEdit(DataGridViewDataErrorContexts.Commit);

            //if (dgvBillItems.CurrentCell.OwningColumn.Name.ToString() == "RushTest")
            //{
            //    if (dgvBillItems.CurrentCell.Value.ToString() == "True")
            //    {
            //        dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["AmtDue"].Value = Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["BillQty"].Value) *
            //                                                                                     Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["UnitPrice"].Value) * 2;
            //    }
            //    else
            //    {
            //        dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["AmtDue"].Value = Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["BillQty"].Value) *
            //                                                                                     Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["UnitPrice"].Value);
            //    }
            //}
        }

        private void dgvBillItems_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;
            else if (dgvBillItems.CurrentCell.OwningColumn.Name.ToString() != "BillQty" && dgvBillItems.CurrentCell.OwningColumn.Name.ToString() != "UnitPrice" && 
                     dgvBillItems.CurrentCell.OwningColumn.Name.ToString() != "Prepayments" && dgvBillItems.CurrentCell.OwningColumn.Name.ToString() != "RushTest")
                e.Cancel = true;
        }

        private void dgvContacts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvBillItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvBillItems.IsCurrentCellDirty)
                dgvBillItems.CommitEdit(DataGridViewDataErrorContexts.Commit);

            if (dgvBillItems.CurrentCell.OwningColumn.Name.ToString() == "Prepayments")
                dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["Prepayments"].Value = Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["Prepayments"].Value) * (-1);

            if (dgvBillItems.CurrentCell.OwningColumn.Name.ToString() == "RushTest")
                dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["RushFee"].Value = Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["UnitPrice"].Value);

            if (dgvBillItems.CurrentCell.OwningColumn.Name.ToString() == "BillQty" || dgvBillItems.CurrentCell.OwningColumn.Name.ToString() == "UnitPrice" ||
                dgvBillItems.CurrentCell.OwningColumn.Name.ToString() == "Prepayments")
            {
                dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["AmtDue"].Value = Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["BillQty"].Value) *
                                                                                             Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["UnitPrice"].Value) +
                                                                                             Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["Prepayments"].Value);
                LoadBillSummary();
            }
        }

        private void cboPO_SelectedValueChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    txtPOAmt.Text = PSSClass.FinalBilling.ExPOAmount(Convert.ToInt16(txtSponsorID.Text), cboPO.Text).ToString("$#,##0.00");
            //    dtBillItems.Rows.Clear(); dtBillSummary.Rows.Clear(); dtOtherFees.Rows.Clear(); dtPrepayments.Rows.Clear();
            //}
            //catch { }
        }

        private void dgvContacts_KeyPress(object sender, KeyPressEventArgs e)
        {
             if (e.KeyChar == 13)
             {
                 txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
                 txtContactID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
                 dgvContacts.Visible = false; dgvSponsors.Visible = false;
                 cboPO.DataSource = null;
                 dtBillItems.Rows.Clear(); dtBillSummary.Rows.Clear(); dtOtherFees.Rows.Clear(); dtPrepayments.Rows.Clear();
                 //LoadPO();
                 cboPO.Select();
             }
            else if (e.KeyChar == 27)
                dgvContacts.Visible = false;
        }

        private void dgvContacts_Leave(object sender, EventArgs e)
        {
            dgvContacts.Visible = false;
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void txtContactID_Enter(object sender, EventArgs e)
        {
            dgvContacts.Visible = false; dgvSponsors.Visible = false;
        }

        private void txtContactID_Leave(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (txtContactID.Text.Trim() != "")
                {
                    txtContact.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
                    dgvContacts.Visible = false;
                }
            }
        }

        private void cboPO_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtPOAmt.Text = PSSClass.FinalBilling.ExPOBalance(Convert.ToInt16(txtSponsorID.Text), cboPO.Text).ToString("$#,##0.00");
                dtBillItems.Rows.Clear(); dtBillSummary.Rows.Clear(); dtOtherFees.Rows.Clear(); dtPrepayments.Rows.Clear();
            }
            catch { }
            LoadBillItems();
            LoadPrepayments();
            LoadBillSummary();
            LoadOtherFees();
        }

        private void lnkPOPDF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkPOPDF.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            LoadSponsorsDDL();
            dgvSponsors.Visible = true;
        }

        private void picContacts_Click(object sender, EventArgs e)
        {
            LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
            dgvContacts.Visible = true;
        }

        private void txtSponsor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                txtSponsorID.Text = ""; txtContactID.Text = ""; txtContact.Text = ""; lnkPOPDF.Text = "PO (PDF)"; cboPO.DataSource = null; txtPOAmt.Text = "";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task(s)?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }

            if (nFB == 1 || nFB == 2)
            {
                nFB = 0;
                SendKeys.Send("{F12}");
                return;
            }
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
            AddEditMode(false);//Initialize Toolbar
            FileAccess();
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            if (PSSClass.Sponsors.HasMultipleBillAddr(Convert.ToInt16(txtSponsorID.Text)) == true)
            {
                MessageBox.Show("This Sponsor has multiple billing addresses.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //INGREDION
            int nSC = 0;
            int nI = 0;
            for (int i = 0; i < dtBillItems.Rows.Count; i++)
            {
                nSC = Convert.ToInt16(dtBillItems.Rows[i]["SC"]);
                if (nSC > 1000 && nSC < 2000 && nSC != 1079)
                {
                    nI = 1;
                }
            }
            AcctgRpt rptInvoice = new AcctgRpt();
            rptInvoice.WindowState = FormWindowState.Maximized;
            rptInvoice.nQ = 1;
            rptInvoice.rptName = "InvoiceOthers";

            try
            {
                rptInvoice.nInvNo = Convert.ToInt32(txtInvNo.Text);
                rptInvoice.Show();
            }
            catch { }
        }

        private void btnEMailQ_Click(object sender, EventArgs e)
        {
            if (txtInvNo.Text == "")
            {
                MessageBox.Show("Invoice is not not yet created.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (PSSClass.Sponsors.HasMultipleBillAddr(Convert.ToInt16(txtSponsorID.Text)) == true)
            {
                MessageBox.Show("This Sponsor has multiple billing addresses.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int nSC = 0; int nI = 0;
            for (int i = 0; i < dtBillItems.Rows.Count; i++)
            {
                nSC = Convert.ToInt16(dtBillItems.Rows[i]["SC"]);
                if (nSC > 1000 && nSC < 2000 && nSC != 1079)
                {
                    nI = 1;
                }
            }
            AcctgRpt rptInvoice = new AcctgRpt();
            rptInvoice.WindowState = FormWindowState.Maximized;
            rptInvoice.nQ = 3;
            rptInvoice.rptName = "InvoiceOthers";
            try
            {
                rptInvoice.nInvNo = Convert.ToInt32(txtInvNo.Text);
                rptInvoice.Show();
            }
            catch { }

            rptInvoice.Close(); rptInvoice.Dispose();
            lstAttachment.Items.Clear();
            lnkInvoice.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\Invoices\" + DateTime.Now.Year.ToString() + @"\I-" + Convert.ToInt32(txtInvNo.Text).ToString("0000000") + ".pdf";
            lstAttachment.Items.Add(lnkInvoice.Text);

            DataTable dt = new DataTable();
            dt = PSSClass.Sponsors.APData(Convert.ToInt16(txtSponsorID.Text));
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No A/P contact data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string strAP = dt.Rows[0]["APContact"].ToString();
            txtTo.Text = dt.Rows[0]["APEMail"].ToString();
            txtSubject.Text = "Invoice No. " + txtInvNo.Text;
            txtBCC.Text = "accounting@princesterilization.com; djprince@princesterilization.com; jmastej@princesterilization.com"; //"ar@gibraltarlabsinc.com"; //A/R Monitoring
            dt.Dispose();
            // Set HTMLBody. 
            //add the body of the email
            txtBody.Text = "Dear " + strAP + ";" + Environment.NewLine + Environment.NewLine +
                      "We appreciate your business with us!" + Environment.NewLine + Environment.NewLine +
                      "The attached invoice is being submitted for payment processing." + Environment.NewLine + Environment.NewLine +
                      "Should you have any questions or clarifications, please do not hesitate to contact me." + Environment.NewLine + Environment.NewLine +
                      "Thank you for your continued support!";
            pnlEMail.Visible = true; pnlEMail.BringToFront();
            pnlEMail.Left = 300; pnlEMail.Top = 150;
            pnlRecord.Enabled = false;
        }

        private void btnCancelSend_Click(object sender, EventArgs e)
        {
            pnlEMail.Visible = false; pnlRecord.Enabled = true;
        }

        private void lnkInvoice_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkInvoice.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult result = ofdAttachment.ShowDialog();
            if (result == DialogResult.OK)
            {
                string strFile = ofdAttachment.FileName;
                lstAttachment.Items.Add(strFile);
                lnkInvoice.Text = strFile;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstAttachment.SelectedIndex == -1)
                return;

            lstAttachment.Items.RemoveAt(lstAttachment.SelectedIndex);
            try
            {
                lnkInvoice.Text = lstAttachment.Items[0].ToString();
            }
            catch
            {
                lnkInvoice.Text = "";
            }
        }

        private void btnSendEMail_Click(object sender, EventArgs e)
        {
            if (lstAttachment.Items.Count == 0)
            {
                MessageBox.Show("No attachment found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            string strBody = txtBody.Text.Replace("\r\n", "<br />");
            string strSignature = ReadSignature();
            strBody = strBody + "<br /><br />" + strSignature;

            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.

            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            oMsg.HTMLBody = "<FONT face=\"Arial\">";
            oMsg.HTMLBody += strBody.Trim();
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

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(txtInvNo.Text));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdInvEDate";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            pnlEMail.Visible = false; pnlRecord.Enabled = true;
            LoadData();
            AddEditMode(false);
            nMode = 0;
            // Collect garbage.
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers();
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (PSSClass.Sponsors.HasMultipleBillAddr(Convert.ToInt16(txtSponsorID.Text)) == true)
            {
                MessageBox.Show("This Sponsor has multiple billing addresses.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //INGREDION
            int nSC = 0; int nI = 0;
            for (int i = 0; i < dtBillItems.Rows.Count; i++)
            {
                nSC = Convert.ToInt16(dtBillItems.Rows[i]["SC"]);
                if (nSC > 1000 && nSC < 2000 && nSC != 1079)
                {
                    nI = 1;
                }
            }
            AcctgRpt rptInvoice = new AcctgRpt();
            rptInvoice.nQ = 2;
            rptInvoice.WindowState = FormWindowState.Maximized;
            rptInvoice.rptName = "InvoiceOthers";
            try
            {
                rptInvoice.nInvNo = Convert.ToInt32(txtInvNo.Text);
                rptInvoice.Show();
            }
            catch { }
        }

        private void dgvBillItems_DoubleClick(object sender, EventArgs e)
        {
            if (dgvBillItems.Rows.Count > 0 && dgvBillItems.CurrentCell.OwningColumn.Name == "RptNo" && dgvBillItems.CurrentCell.Value.ToString() != "")
            {
                int intOpen = PSSClass.General.OpenForm(typeof(FinalReports));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(FinalReports));
                }
                FinalReports childForm = new FinalReports();
                childForm.Text = "FINAL REPORTS";
                childForm.MdiParent = this.MdiParent;
                childForm.nRptNo = Convert.ToInt32(dgvBillItems.CurrentCell.Value.ToString());
                childForm.nLSw = 1;
                childForm.Show();
            }
            else if (dgvBillItems.Rows.Count > 0 && dgvBillItems.CurrentCell.OwningColumn.Name == "LogNo" && dgvBillItems.CurrentCell.Value.ToString() != "")
            {
                int intOpen = PSSClass.General.OpenForm(typeof(SamplesLogin));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(SamplesLogin));
                }
                SamplesLogin childForm = new SamplesLogin();
                childForm.Text = "SAMPLES LOGIN";
                childForm.MdiParent = this.MdiParent;
                childForm.nLogNo = Convert.ToInt32(dgvBillItems.CurrentCell.Value.ToString());
                childForm.nFR = 1;
                childForm.Show();
            }
            else if (dgvBillItems.Rows.Count > 0 && dgvBillItems.CurrentCell.OwningColumn.Name == "QuoteNo" && dgvBillItems.CurrentCell.Value.ToString() != "")
            {
                int intOpen = PSSClass.General.OpenForm(typeof(Quotes));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(Quotes));
                }
                Quotes childForm = new Quotes();
                childForm.Text = "QUOTATIONS";
                childForm.MdiParent = this.MdiParent;
                childForm.strQuoteNo = dgvBillItems.CurrentCell.Value.ToString().Substring(0,9);
                childForm.nPSw = 1;
                childForm.Show();
            }
        }

        private void txtMMode_TextChanged(object sender, EventArgs e)
        {
            if (txtMMode.Text == "1")
                txtMailMode.Text = "E-Mail";
            else if (txtMMode.Text == "2")
                txtMailMode.Text = "Mail";
            else
                txtMailMode.Text = "";
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            int nX = (pnlRecord.Location.X + pnlRecord.Width - pnlScan.Width) / 2;
            int nY = (pnlRecord.Location.Y + pnlRecord.Height - pnlScan.Height) / 2;
            pnlRecord.Enabled = false; pnlScan.Visible = true; pnlScan.BringToFront(); pnlScan.Location = new Point(nX, nY);
            tlsFile.Enabled = false;
            txtScanInvNo.Text = txtInvNo.Text; txtScanInvNo.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlRecord.Enabled = true; pnlScan.Visible = false; tlsFile.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(txtScanInvNo.Text));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdInvMailDate";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            pnlRecord.Enabled = true; pnlScan.Visible = false; tlsFile.Enabled = true;
            LoadInvoice();
        }

        //private void cboPO_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    e.Handled = true;
        //}

        private void txtInvNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void chkRevised_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRevised.Checked)
            {
                txtRevByID.Text = LogIn.nUserID.ToString();
                dtInvoice.Rows[0]["RevisedByID"] = Convert.ToInt16(txtRevByID.Text);
                txtDateRev.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
                dtInvoice.Rows[0]["DateRevised"] = Convert.ToDateTime(txtDateRev.Text);
            }
            else
            {
                txtDateRev.Text = ""; txtRevByID.Text = "";
                dtInvoice.Rows[0]["DateRevised"] = DBNull.Value;
                dtInvoice.Rows[0]["RevisedByID"] = DBNull.Value;
            }
        }
        
        private void chkExtraInvoice_CheckedChanged(object sender, EventArgs e)
        {
            //chkExtraInvoice
            try
            {
                if (dtInvoice.Rows.Count>0)
                {
                    if (chkExtraInvoice.Checked)
                    {
                        dtInvoice.Rows[0]["ExtraInvoice"] = true;
                    }
                    else
                    {
                        dtInvoice.Rows[0]["ExtraInvoice"] = false;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        private void mskDateCreated_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
                pnlCalendar.Visible = true; pnlCalendar.Location = new Point(318, 33);
        }

        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            mskInvDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            pnlCalendar.Visible = false;
            dtInvoice.Rows[0]["InvoiceDate"] = Convert.ToDateTime(mskInvDate.Text).ToShortDateString();
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void btnReplacePO_Click(object sender, EventArgs e)
        {
            tsbSave.Enabled = false; tsbCancel.Enabled = false;
            int nX = (pnlRecord.Location.X + pnlRecord.Width - pnlReplacePO.Width) / 2;
            int nY = (pnlRecord.Location.Y + pnlRecord.Height - pnlReplacePO.Height) / 2;
            pnlRecord.Enabled = false;
            pnlReplacePO.Visible = true; pnlReplacePO.BringToFront();
            pnlReplacePO.Location = new Point(nX, nY);
            txtCurrentPO.Text = cboPO.Text;
            txtReplacementPO.Select();
            txtReplacementPO.Focus();
        }

        private void btnCancelReplace_Click(object sender, EventArgs e)
        {
            pnlReplacePO.Visible = false; pnlRecord.Enabled = true;
            AddEditMode(false);
        }

        private void btnOKReplace_Click(object sender, EventArgs e)
        {
            if (txtReplacementPO.Text.Trim() == "")
            {
                MessageBox.Show("Please enter the replacement PO number.");
                txtReplacementPO.Select(); txtReplacementPO.Focus();
                return;
            }
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("WARNING: This process would replace the" + Environment.NewLine + "current PO number. Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dReply == DialogResult.No)
            {
                return;
            }
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@CurrPO", txtCurrentPO.Text);
            sqlcmd.Parameters.AddWithValue("@NewPO", txtReplacementPO.Text);
            sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(txtInvNo.Text));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdInvPONo";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcmd.Dispose(); sqlcnn.Dispose();
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            pnlReplacePO.Visible = false;
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
            AddEditMode(false);
            FileAccess();
        }

        private void dgvBillItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvBillItems.IsCurrentCellDirty)
            {
                dgvBillItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvOtherFees_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvOtherFees.IsCurrentCellDirty)
            {
                dgvOtherFees.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvOtherFees_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;
            else if (dgvOtherFees.CurrentCell.OwningColumn.Name.ToString() != "ReportNo" && dgvOtherFees.CurrentCell.OwningColumn.Name.ToString() != "BillQty" && dgvOtherFees.CurrentCell.OwningColumn.Name.ToString() != "UnitPrice")
                e.Cancel = true;
        }

        private void dgvOtherFees_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvOtherFees.IsCurrentCellDirty)
                dgvOtherFees.CommitEdit(DataGridViewDataErrorContexts.Commit);

            if (dgvOtherFees.CurrentCell.OwningColumn.Name.ToString() == "BillQty" || dgvOtherFees.CurrentCell.OwningColumn.Name.ToString() == "UnitPrice")
            {
                {
                    dgvOtherFees.Rows[dgvOtherFees.CurrentCell.RowIndex].Cells["Amount"].Value = Convert.ToDecimal(dgvOtherFees.Rows[dgvOtherFees.CurrentCell.RowIndex].Cells["BillQty"].Value) *
                                                                                                 Convert.ToDecimal(dgvOtherFees.Rows[dgvOtherFees.CurrentCell.RowIndex].Cells["UnitPrice"].Value);
                    decimal nTotal = 0;
                    for (int i = 0; i < dtOtherFees.Rows.Count; i++)
                    {
                        if (dtOtherFees.Rows[i].RowState.ToString() != "Deleted" && dtOtherFees.Rows[i].RowState.ToString() != "Detached")
                            nTotal += Convert.ToDecimal(dtOtherFees.Rows[i]["Amount"]);
                    }
                    txtOtherFees.Text = nTotal.ToString("$#,###0.00");
                    try
                    {
                        decimal nTot = 0;
                        nTot = Convert.ToDecimal(txtOtherFees.Text.Replace("$", "")) + Convert.ToDecimal(txtServiceFees.Text.Replace("$", ""));// -Convert.ToDecimal(txtPP.Text.Replace("$", ""));
                        txtInvTotal.Text = nTot.ToString("$#,##0.00");
                    }
                    catch { }
                }
            }
        }

        private void dgvBillItems_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;
        }

        private void dgvOtherFees_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;
            else if (nMode == 2)
                nListDelFees.Add(Convert.ToInt32(dgvOtherFees.CurrentRow.Cells["InvoiceID"].Value));
        }

        private void btnReplQuote_Click(object sender, EventArgs e)
        {
            nAddRepl = 1;
            pnlRecord.Enabled = false;
            pnlAddReplQ.Visible = true; pnlAddReplQ.BringToFront(); pnlAddReplQ.Location = new Point(300, 50);
            LoadQuotes();
        }

        private void btnCancelItem_Click(object sender, EventArgs e)
        {
            pnlAddReplQ.Visible = false; pnlRecord.Enabled = true;
        }

        private void LoadQuotes()
        {
            DataTable dt = new DataTable();
            if (txtSponsor.Text.IndexOf("INGREDION") != -1)
            {
                dt = PSSClass.Quotations.LoadIngredionQuotes();
                cboQuotes.DisplayMember = "QuotationNo";
                cboQuotes.ValueMember = "QuotationNo";

                DataRow row = dt.NewRow();
                row["QuotationNo"] = "-select-";
                dt.Rows.InsertAt(row, 0);
                //dt.Columns.Add("QuotationNo", typeof(string));
                //DataRow row = dt.NewRow();
                //row["QuotationNo"] = "-select-";
                //dt.Rows.InsertAt(row, 0);
                //row = dt.NewRow();
                //row["QuotationNo"] = "2015.0992";
                //dt.Rows.InsertAt(row, 1);
                //row = dt.NewRow();
                //row["QuotationNo"] = "2015.1403";
                //dt.Rows.InsertAt(row, 2);
                //row = dt.NewRow();
                //row["QuotationNo"] = "2015.1674";
                //dt.Rows.InsertAt(row, 3);
                //row = dt.NewRow();
                //row["QuotationNo"] = "2015.1738";
                //dt.Rows.InsertAt(row, 4);
                //cboQuotes.DisplayMember = "QuotationNo";
                //cboQuotes.ValueMember = "QuotationNo";
            }
            else
            {
                dt = PSSClass.Quotations.LoadQuotesMisc(Convert.ToInt16(txtSponsorID.Text));
                cboQuotes.DisplayMember = "QuotationNo";
                cboQuotes.ValueMember = "QuotationNo";

                DataRow row = dt.NewRow();
                row["QuotationNo"] = "-select-";
                dt.Rows.InsertAt(row, 0);
            }
            cboQuotes.DataSource = dt;
            cboQuotes.Refresh();
            dgvTestItems.RowCount = 0;
        }

        private void LoadSC()
        {
            DataTable dtSCO = new DataTable();
            dtSCO = PSSClass.ServiceCodes.PrepayOtherCost();

            DataRow row = dtSCO.NewRow();
            row["ServiceDesc"] = "-select-";
            row["ServiceCode"] = 0;
            dtSCO.Rows.InsertAt(row, 0);
            cboSC.Refresh();

            cboSC.DataSource = dtSCO;
            cboSC.DisplayMember = "ServiceDesc";
            cboSC.ValueMember = "ServiceCode";
            cboSC.SelectedIndex = 0;
        }

        private void cboQuotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboQuotes.SelectedIndex != 0)
            {
                DataTable dt = new DataTable();
                try
                {
                    dgvTestItems.RowCount = 0;
                    
                    dt = PSSClass.Quotations.LoadInvTests(txtCmpyCode.Text, cboQuotes.Text.ToString());
                    if (dt == null)
                    {
                        MessageBox.Show("Connection problems. Please contact your system administrator.");
                        return;
                    }
                    if ((nAddRepl != 3)&&(dgvBillItems.Rows.Count>0))
                    {
                        DataRow[] foundrows = dt.Select("ServiceCode = " + dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["SC"].Value);
                        foreach (DataRow row in foundrows)
                        {
                            dgvTestItems.Rows.Add(row["QuoteNo"].ToString(), row["ServiceCode"].ToString(), row["TestDesc1"].ToString(), row["UnitPrice"].ToString(), row["ControlNo"].ToString());
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dgvTestItems.Rows.Add(dt.Rows[i]["QuoteNo"].ToString(), dt.Rows[i]["ServiceCode"].ToString(), dt.Rows[i]["TestDesc1"].ToString(), dt.Rows[i]["UnitPrice"].ToString(), dt.Rows[i]["ControlNo"].ToString());
                        }
                    }
                   
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    dt.Dispose();
                }
                
            }
        }

        private void btnOKItem_Click(object sender, EventArgs e)
        {
            pnlAddReplQ.Visible = false; pnlRecord.Enabled = true;
            
            if (dgvTestItems.Rows.Count == 0 && cboSC.SelectedIndex == 0)
                return;

            if (nAddRepl == 3 && dgvTestItems.Rows.Count == 0 && cboSC.SelectedIndex != 0)
            {
                DataRow dR = dtOtherFees.NewRow();
                dR["InvoiceID"] = 0;
                dR["ReportNo"] = DBNull.Value;
                dR["PSSNo"] = DBNull.Value;
                dR["ServiceCode"] = Convert.ToInt16(lblSCDesc.Text);
                dR["TestDesc1"] = "";
                dR["BillQty"] = 1;
                dR["UnitPrice"] = 0;
                dR["Amount"] = 0;
                dR["QuotationNo"] = dgvTestItems.Rows[dgvTestItems.CurrentCell.RowIndex].Cells["QuotationNo"].Value.ToString();
                dR["RevisionNo"] = Convert.ToInt16(dgvTestItems.Rows[dgvTestItems.CurrentCell.RowIndex].Cells["RevisionNo"].Value.ToString());
                dR["ControlNo"] = Convert.ToInt16(dgvTestItems.Rows[dgvTestItems.CurrentCell.RowIndex].Cells["ControlNo"].Value.ToString());
                dR["QCmpyCode"] = cboQuotes.SelectedValue.ToString().Trim();
                dtOtherFees.Rows.Add(dR);

                decimal nTotal = 0;
                for (int i = 0; i < dtOtherFees.Rows.Count; i++)
                {
                    if (dtOtherFees.Rows[i].RowState.ToString() != "Deleted" && dtOtherFees.Rows[i].RowState.ToString() != "Detached")
                        nTotal += Convert.ToDecimal(dtOtherFees.Rows[i]["Amount"]);
                }
                txtOtherFees.Text = nTotal.ToString("$#,###0.00");
                txtInvTotal.Text = nTotal.ToString("$#,###0.00");
                return;
            }

            if (nAddRepl == 1)
            {
                dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["CtrlNo"].Value = dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["CtrlNo"].Value;
                dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["TestDesc"].Value = dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["TestDesc"].Value;
                dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["QuoteNo"].Value = dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["QuoteNo"].Value;
                dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["UnitPrice"].Value = dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["UnitPrice"].Value;
                dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["AmtDue"].Value = Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["BillQty"].Value) *
                                                                                                Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["UnitPrice"].Value) +
                                                                                                Convert.ToDecimal(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["Prepayments"].Value);
                LoadBillSummary();
            }
            else
            {
                string strQN, strQNo, strRNo, strCtrlNo = ""; int nI;
                if (dgvTestItems.Rows.Count > 0)
                    strCtrlNo = dgvTestItems.Rows[dgvTestItems.CurrentCell.RowIndex].Cells["CtrlNo"].Value.ToString();

                for (int i = 0; i < dgvTestItems.Rows.Count; i++)
                {
                    if (dgvTestItems.Rows[i].Cells["CtrlNo"].Value.ToString() == strCtrlNo)
                    {
                        if (nAddRepl == 2)
                            //strQN = dtBillItems.Rows[i]["QuoteNo"].ToString();
                            strQN = dgvBillItems.Rows[dgvBillItems.CurrentRow.Index].Cells["QuoteNo"].Value.ToString();
                        else
                            strQN = dgvTestItems.Rows[i].Cells["QuoteNo"].Value.ToString();

                        nI = strQN.IndexOf("R");
                        strQNo = strQN.Substring(0, nI - 1);
                        strRNo = strQN.Substring(nI + 1, strQN.Length - (nI + 1));

                        if (nAddRepl == 2)
                        {
                            DataRow dR = dtBillItems.NewRow();
                            dR["InvID"] = 0;
                            dR["RptNo"] = Convert.ToInt32(dgvBillItems.Rows[dgvBillItems.CurrentRow.Index].Cells["RptNo"].Value);
                            dR["LogNo"] = Convert.ToInt32(dgvBillItems.Rows[dgvBillItems.CurrentRow.Index].Cells["LogNo"].Value);
                            dR["SC"] = Convert.ToInt16(dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["SC"].Value);
                            dR["QuoteNo"] = dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["QuoteNo"].Value;
                            dR["PONo"] = "";
                            dR["CtrlNo"] = Convert.ToInt16(dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["CtrlNo"].Value);
                            dR["TestDesc"] = dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["TestDesc"].Value;
                            dR["RushTest"] = false;
                            dR["BillQty"] = 1;
                            dR["UnitPrice"] = Convert.ToDecimal(dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["UnitPrice"].Value);
                            dR["Prepayments"] = 0;
                            dR["AmtDue"] = Convert.ToDecimal(dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["UnitPrice"].Value);
                            dR["RushFee"] = Convert.ToDecimal(dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["UnitPrice"].Value);
                            dR["QuotationNo"] = strQNo;
                            dR["RevisionNo"] = Convert.ToInt16(strRNo);
                            dtBillItems.Rows.Add(dR);
                        }
                        else
                        {
                            DataRow dR = dtOtherFees.NewRow();
                            dR["InvoiceID"] = 0;
                            dR["ReportNo"] = DBNull.Value;
                            dR["PSSNo"] = DBNull.Value;
                            dR["ServiceCode"] = Convert.ToInt16(dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["SC"].Value);
                            dR["TestDesc1"] = dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["TestDesc"].Value;
                            dR["BillQty"] = 1;
                            dR["UnitPrice"] = Convert.ToDecimal(dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["UnitPrice"].Value);
                            dR["Amount"] = Convert.ToDecimal(dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["UnitPrice"].Value);
                            dR["QuotationNo"] = strQNo;
                            dR["RevisionNo"] = Convert.ToInt16(strRNo);
                            dR["ControlNo"] = Convert.ToInt16(dgvTestItems.Rows[dgvTestItems.CurrentRow.Index].Cells["CtrlNo"].Value);
                            dtOtherFees.Rows.Add(dR);
                        }
                    }
                }
                if (nAddRepl == 2)
                    LoadBillSummary();
                else if (nAddRepl == 3)
                {
                    decimal nTotal = 0;
                    for (int i = 0; i < dtOtherFees.Rows.Count; i++)
                    {
                        if (dtOtherFees.Rows[i].RowState.ToString() != "Deleted" && dtOtherFees.Rows[i].RowState.ToString() != "Detached")
                            nTotal += Convert.ToDecimal(dtOtherFees.Rows[i]["Amount"]);
                    }
                    txtOtherFees.Text = nTotal.ToString("$#,###0.00");
                    txtInvTotal.Text = nTotal.ToString("$#,##0.00");
                    //try
                    //{
                    //    decimal nTot = 0;
                    //    nTot = Convert.ToDecimal(txtOtherFees.Text.Replace("$", "")) + Convert.ToDecimal(txtServiceFees.Text.Replace("$", ""));// -Convert.ToDecimal(txtPP.Text.Replace("$", ""));
                    //    txtInvTotal.Text = nTot.ToString("$#,##0.00");
                    //}
                    //catch { }
                }
            }
        }

        private void pnlAddReplQ_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void pnlAddReplQ_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlAddReplQ.Location = PointToClient(this.pnlAddReplQ.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlAddReplQ_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void btnAddItems_Click(object sender, EventArgs e)
        {
            nAddRepl = 2;
            pnlRecord.Enabled = false;
            pnlAddReplQ.Visible = true; pnlAddReplQ.BringToFront(); pnlAddReplQ.Location = new Point(300, 50);
            LoadQuotes(); 
        }

        private void btnAddOthFees_Click(object sender, EventArgs e)
        {
            nAddRepl = 3;
            pnlRecord.Enabled = false;
            pnlAddReplQ.Visible = true; pnlAddReplQ.BringToFront(); pnlAddReplQ.Location = new Point(300, 50);
            LoadQuotes();
            LoadSC();
        }

        private void btnEditPO_Click(object sender, EventArgs e)
        {
            int nX = (pnlRecord.Location.X + pnlRecord.Width - pnlScan.Width) / 2;
            int nY = (pnlRecord.Location.Y + pnlRecord.Height - pnlScan.Height) / 2;
            pnlRecord.Enabled = false; pnlEditPO.Visible = true; pnlEditPO.BringToFront(); pnlEditPO.Location = new Point(nX, nY);
            tlsFile.Enabled = false;
            txtPONo.Focus();
        }

        private void btnOKPO_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["LogNo"].Value));
            sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dgvBillItems.Rows[dgvBillItems.CurrentCell.RowIndex].Cells["SC"].Value));
            sqlcmd.Parameters.AddWithValue("@PONo", txtPONo.Text);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdLogTestPONo";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            pnlRecord.Enabled = true; pnlEditPO.Visible = false; tlsFile.Enabled = true;
            LoadInvoice();
        }

        private void btnCancelPO_Click(object sender, EventArgs e)
        {
            pnlRecord.Enabled = true; pnlEditPO.Visible = false; tlsFile.Enabled = true;
        }

        private void FinalBilling_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtSponsors.Dispose(); dtContacts.Dispose(); dtInvoice.Dispose();
            dtPrepayments.Dispose(); dtBillItems.Dispose(); dtBillSummary.Dispose();
            dtOtherFees.Dispose(); dtSC.Dispose();
            bsFile.Dispose(); bsBillItems.Dispose(); bsBillSummary.Dispose();
            bsInvoice.Dispose(); bsOtherFees.Dispose();
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers();
        }

        private void cboSC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSC.SelectedIndex == 0)
                lblSCDesc.Text = "";
            else
                lblSCDesc.Text = cboSC.SelectedValue.ToString();
        }

        private void dgvOtherFees_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            decimal nTotal = 0;
            for (int i = 0; i < dtOtherFees.Rows.Count; i++)
            {
                if (dtOtherFees.Rows[i].RowState.ToString() != "Deleted" && dtOtherFees.Rows[i].RowState.ToString() != "Detached")
                    nTotal += Convert.ToDecimal(dtOtherFees.Rows[i]["Amount"]);
            }
            txtOtherFees.Text = nTotal.ToString("$#,###0.00");
            txtInvTotal.Text = nTotal.ToString("$#,##0.00");
        }

       
    }
}
