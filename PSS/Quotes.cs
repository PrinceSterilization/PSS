//Quotes.cs
// AUTHOR       : ALEXANDER M. DELA CRUZ
// TITLE        : Software Developer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Quotations File Maintenance

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;
using System.Diagnostics;

namespace PSS
{
    public partial class Quotes : PSS.TemplateForm
    {
        public static string strCopyQuote = "";
        public string strQuoteNo;
        public string pubCmpyCode;
        public byte nPSw;
        //
        public Int16 lnkRevNo;
        private string pSponsor;
        private int pSponsorID;
        private string pContact;
        private int pContactID;
        private string pSp;
        private int pSpID;
        private string pCon;
        private int pConID;
        //
        private byte nMode = 0;
        //variables for record panel drag and drop
        private bool mouseDown;
        private Point mousePos;
        private bool mouseDownP;
        private Point mousePosP;
        //
        private string[] arrCol;
        private int nIndex;
        private int nSave = 1; //for checking if save is successful
        private byte nRev = 0;
        private string strFileAccess = "RO"; //User's Access to this File
        private string strComboSC = "";
        private byte nS = 0;//SC Desc Selecteion ID
        private byte nU = 0; //Unit Desc Selection ID
        private bool bPPy = false; // prepayment for Group I SCs (Prepayment items)
        private string strPPQ = "";// Quote No. for Prepayment storage 
        private byte nSp = 1; //Switch for Sponsor Selection

        protected DataTable dtSponsors = new DataTable();
        protected DataTable dtContacts = new DataTable();
        protected DataTable dtQuote = new DataTable();
        protected DataTable dtRevisions = new DataTable();
        protected DataTable dtRevTests = new DataTable();
        protected DataTable dtSC = new DataTable();
        protected DataTable dtUnits = new DataTable();

        //for DatagridView search
        private int nCtr = 0;
        private int nSw = 0;
        //======================

        private string strSC = "", strTDesc1 = "", strTDesc2 = "", strTDesc3 = "", strTDesc4 = "", strPrice = "0.00";

        public Quotes()
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
            txtSCDesc.TextChanged += new EventHandler(txtSCDescTextChangedHandler);
            txtUnit.TextChanged += new EventHandler(txtUnitTextChangedHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void Quotes_Load(object sender, EventArgs e)
        {
            try
            {
                strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Quotations");
                pnlTestItems.Visible = false;
                LoadUnits();
                LoadRecords();
                BuildPrintItems();

                this.WindowState = FormWindowState.Maximized;
                this.Focus();
                this.BringToFront();

                dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

                pnlTestItems.Visible = true; pnlTestItems.BringToFront(); pnlTestItems.Location = new Point(0, 550);
                //Master Quote Data Table
                dtQuote.Columns.Add("CompanyCode", typeof(string));
                dtQuote.Columns.Add("QuotationNo", typeof(string));
                dtQuote.Columns.Add("RevisionNo", typeof(Int16));
                dtQuote.Columns.Add("SponsorID", typeof(Int32));
                dtQuote.Columns.Add("SponsorName", typeof(string));
                dtQuote.Columns.Add("ContactID", typeof(Int32));
                dtQuote.Columns.Add("ContactName", typeof(string));
                dtQuote.Columns.Add("SecSponsorID", typeof(Int32));
                dtQuote.Columns.Add("SecSponsorName", typeof(string));
                dtQuote.Columns.Add("SecContactID", typeof(Int32));
                dtQuote.Columns.Add("SecContactName", typeof(string));
                dtQuote.Columns.Add("HideTerms", typeof(bool));
                dtQuote.Columns.Add("PriceCheck", typeof(bool));
                dtQuote.Columns.Add("GLP", typeof(bool));
                dtQuote.Columns.Add("ExcFollowUp", typeof(bool));
                dtQuote.Columns.Add("Comments", typeof(string));
                dtQuote.Columns.Add("PONo", typeof(string));
                dtQuote.Columns.Add("DateCreated", typeof(DateTime));
                dtQuote.Columns.Add("CreatedByID", typeof(Int16));
                dtQuote.Columns.Add("LastUpdate", typeof(DateTime));
                dtQuote.Columns.Add("LastUserID", typeof(Int16));
                bsQuote.DataSource = dtQuote;

                //Revisions Data Table
                dtRevisions.Columns.Add("RevisionNo", typeof(Int16));
                dtRevisions.Columns.Add("RevisionStatus", typeof(Int16));
                dtRevisions.Columns.Add("CreatedBy", typeof(string));
                dtRevisions.Columns.Add("DateAccepted", typeof(DateTime));
                dtRevisions.Columns.Add("DateRejected", typeof(DateTime));
                dtRevisions.Columns.Add("RejectedCode", typeof(Int16));
                dtRevisions.Columns.Add("RejectedDesc", typeof(string));
                dtRevisions.Columns.Add("WithPrePayment", typeof(bool));
                dtRevisions.Columns.Add("PrepayInvoiced", typeof(bool));
                dtRevisions.Columns.Add("PrepayInvoiceNo", typeof(Int32));
                dtRevisions.Columns.Add("YearsValid", typeof(Int16));
                dtRevisions.Columns.Add("DateEMailed", typeof(DateTime));
                dtRevisions.Columns.Add("EMailedBy", typeof(string));
                dtRevisions.Columns.Add("AcceptedRevFile", typeof(string));
                dtRevisions.Columns.Add("TestCategory1", typeof(string));
                dtRevisions.Columns.Add("TestCategory2", typeof(string));
                dtRevisions.Columns.Add("TestCategory3", typeof(string));
                dtRevisions.Columns.Add("TestCategory4", typeof(string));
                dtRevisions.Columns.Add("CommentsBeforeTable", typeof(string));
                dtRevisions.Columns.Add("CommentsAfterTable", typeof(string));
                dtRevisions.Columns.Add("CommentsNonPrinting", typeof(string));
                dtRevisions.Columns.Add("PriceCheck", typeof(string));
                dtRevisions.Columns.Add("DateCreated", typeof(DateTime));
                dtRevisions.Columns.Add("CreatedByID", typeof(Int16));
                dtRevisions.Columns.Add("LastUpdate", typeof(DateTime));
                dtRevisions.Columns.Add("LastUserID", typeof(Int16));
                bsRevisions.DataSource = dtRevisions;
                bnRevisions.BindingSource = bsRevisions;

                //Revision Test Services
                dtRevTests.Columns.Add("RevisionNo", typeof(Int16));
                dtRevTests.Columns.Add("ControlNo", typeof(Int16));
                dtRevTests.Columns.Add("TestNo", typeof(Int16));
                dtRevTests.Columns.Add("SubTestNo", typeof(Int16));
                dtRevTests.Columns.Add("ServiceCode", typeof(Int16));
                dtRevTests.Columns.Add("ServiceDesc", typeof(string));
                dtRevTests.Columns.Add("TestDesc1", typeof(string));
                dtRevTests.Columns.Add("TestComments", typeof(string));
                dtRevTests.Columns.Add("UnitID", typeof(Int16));
                dtRevTests.Columns.Add("UnitDesc", typeof(string));
                dtRevTests.Columns.Add("UnitPrice", typeof(decimal));
                dtRevTests.Columns.Add("BillQuantity", typeof(Int32));
                dtRevTests.Columns.Add("Amount", typeof(decimal));
                dtRevTests.Columns.Add("Rush", typeof(bool));
                dtRevTests.Columns.Add("RushPrice", typeof(decimal));
                dtRevTests.Columns.Add("RushAmount", typeof(decimal));
                dtRevTests.Columns.Add("OptionalTest", typeof(bool));
                dtRevTests.Columns.Add("ProtocolPaid", typeof(bool));
                dtRevTests.Columns.Add("DateCreated", typeof(DateTime));
                dtRevTests.Columns.Add("CreatedByID", typeof(Int16));
                dtRevTests.Columns.Add("LastUpdate", typeof(DateTime));
                dtRevTests.Columns.Add("LastUserID", typeof(Int16));
                //Gross Profit per Unit
                //dtRevTests.Columns.Add("UnitGrossProfit", typeof(decimal));

                bsTestItems.DataSource = dtRevTests;
                bnTestItems.BindingSource = bsTestItems;
                dtrTestItems.DataSource = bsRevTests;
                Binding DateCreatedBinding;
                DateCreatedBinding = new Binding("Text", bsRevisions, "DateCreated");
                DateCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
                mskDateCreated.DataBindings.Add(DateCreatedBinding);

                Binding DateAcceptedBinding;
                DateAcceptedBinding = new Binding("Text", bsRevisions, "DateAccepted");
                DateAcceptedBinding.Format += new ConvertEventHandler(DateBinding_Format);
                mskDateAccepted.DataBindings.Add(DateAcceptedBinding);

                Binding DateRejectedBinding;
                DateRejectedBinding = new Binding("Text", bsRevisions, "DateRejected");
                DateRejectedBinding.Format += new ConvertEventHandler(DateBinding_Format);
                mskDateRejected.DataBindings.Add(DateRejectedBinding);

                txtRevNo.DataBindings.Add("Text", bsRevisions, "RevisionNo");
                cboRevStatus.DataBindings.Add("SelectedIndex", bsRevisions, "RevisionStatus", true);
                cboReasons.DataBindings.Add("SelectedIndex", bsRevisions, "RejectedCode", true);
                txtOtherReason.DataBindings.Add("Text", bsRevisions, "RejectedDesc");
                txtRevCreator.DataBindings.Add("Text", bsRevisions, "CreatedBy");
                chkPPay.DataBindings.Add("Checked", bsRevisions, "WithPrepayment", true);
                txtYears.DataBindings.Add("Text", bsRevisions, "YearsValid");
                txtPDF.DataBindings.Add("Text", bsRevisions, "AcceptedRevFile");
                cboTestCat1.DataBindings.Add("Text", bsRevisions, "TestCategory1");
                cboTestCat2.DataBindings.Add("Text", bsRevisions, "TestCategory2");
                cboTestCat3.DataBindings.Add("Text", bsRevisions, "TestCategory3");
                cboTestCat4.DataBindings.Add("Text", bsRevisions, "TestCategory4");
                spbComBefTable.DataBindings.Add("Text", bsRevisions, "CommentsBeforeTable");
                spbComAftTable.DataBindings.Add("Text", bsRevisions, "CommentsAfterTable");
                spbComNonPrint.DataBindings.Add("Text", bsRevisions, "CommentsNonPrinting");
                txtDateEMailed.DataBindings.Add("Text", bsRevisions, "DateEMailed");
                txtEMailedBy.DataBindings.Add("Text", bsRevisions, "EMailedBy");
                lblInvNo.DataBindings.Add("Text", bsRevisions, "PrepayInvoiceNo");
                txtHideEstTotal.DataBindings.Add("Text", bsRevisions, "PriceCheck");//added 3/15/2017

                if (nPSw == 1 || nPSw == 2 || nPSw == 3)
                {
                    txtCmpyCode.Text = pubCmpyCode.Trim();
                    PSSClass.General.FindRecord("CmpyQuote", txtCmpyCode.Text + strQuoteNo, bsFile, dgvFile);
                    LoadData();
                    if (nPSw == 2) //Quote Followups 
                    {
                        tbcRevisions.SelectedIndex = 1;
                    }
                    else if (nPSw == 3)
                    {
                        pSponsor = txtSponsor.Text;
                        pSponsorID = Convert.ToInt16(txtSponsorID.Text);
                        pContact = txtContact.Text;
                        pContactID = Convert.ToInt16(txtContactID.Text);
                        if (txtSpID.Text != null && txtSpID.Text != "")
                        {
                            pSp = txtSp.Text;
                            pSpID = Convert.ToInt16(txtSpID.Text);
                            pCon = txtCon.Text;
                            pConID = Convert.ToInt16(txtConID.Text);
                        }
                        AddRecord();
                        AddEditMode(true);
                        txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                        LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                        txtContact.Text = pContact;
                        LoadPrevQuotes();
                        dgvSponsors.Visible = false; dgvContacts.Visible = false;
                        cboQuotes.Text = strQuoteNo + ".R" + lnkRevNo.ToString();
                        btnCopy_Click(null, null);
                        LoadRevTestRow(null, null);
                        txtYears.Focus();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void DateBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "__/__/____";
        }

        private void LoadColHeaders()
        {
            DataTable dt = new DataTable();

            dt = PSSClass.Quotations.ColHeadersDDL();
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            string filterExp = "ColumnNo = 1";
            string sortExp = "CategoryDesc";
            DataRow[] drarray;
            drarray = dt.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            for (int i = 0; i < drarray.Length; i++)
            {
                cboTestCat1.Items.Add(drarray[i]["CategoryDesc"].ToString());
            }
            if (cboTestCat1.Items.Count > 0)
                cboTestCat1.SelectedIndex = 0;

            filterExp = "ColumnNo = 2";
            drarray = dt.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            for (int i = 0; i < drarray.Length; i++)
            {
                cboTestCat2.Items.Add(drarray[i]["CategoryDesc"].ToString());
            }
            if (cboTestCat2.Items.Count > 0)
                cboTestCat2.SelectedIndex = 0;

            filterExp = "ColumnNo = 3";
            drarray = dt.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            for (int i = 0; i < drarray.Length; i++)
            {
                cboTestCat3.Items.Add(drarray[i]["CategoryDesc"].ToString());
            }

            filterExp = "ColumnNo = 4";
            drarray = dt.Select(filterExp, sortExp, DataViewRowState.CurrentRows);
            for (int i = 0; i < drarray.Length; i++)
            {
                cboTestCat4.Items.Add(drarray[i]["CategoryDesc"].ToString());
            }
            dt.Dispose();
        }

        private void LoadUnits()
        {
            dtUnits = PSSClass.Units.UnitsDLL();
            if (dtUnits == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                nMode = 9;
                return;
            }
            dgvUnits.DataSource = dtUnits;
            DataView dv = new DataView(dtUnits);
            PSSClass.General.DGVSetUp(dgvUnits, dv, 270);
        }

        private void AddRecord()
        {
            if (dtSponsors.Rows.Count == 0)
                LoadSponsorsDDL();

            if (dtSC.Rows.Count == 0)
                LoadSCDDL();

            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(pnlRecord);

            if (cboTestCat1.Items.Count == 0)
            {
                LoadColHeaders();
            }
            OpenControls(pnlRecord, true);
            tbcRevisions.SelectedIndex = 0;
            pnlTestItems.Visible = false;
            txtRevNo.Text = "0"; txtYears.Text = "1"; lblRevTests.Text = "R0"; lblEStatus.Text = "Not Sent";
            dtQuote.Rows.Clear(); dtRevisions.Rows.Clear(); dtRevTests.Rows.Clear();
            btnAddRev.Enabled = true; btnDelRev.Enabled = true;
            btnAddTestA.Enabled = true; btnAddTestB.Enabled = true; btnDelTest.Enabled = true; btnAddSubA.Enabled = true; btnAddSubB.Enabled = true; btnCopy.Enabled = true;
            btnClose.Visible = false;
            cboQuotes.Enabled = true; btnCopy.Enabled = true;
            txtSC.Enabled = true; txtSCDesc.Enabled = true;
            bnRevisions.Enabled = false; btnAddRev.Enabled = false; btnDelRev.Enabled = false;//allow only 1 revision to be added per session
            cboTestCat1.DropDownStyle = ComboBoxStyle.DropDown; cboTestCat2.DropDownStyle = ComboBoxStyle.DropDown;
            cboTestCat3.DropDownStyle = ComboBoxStyle.DropDown; cboTestCat4.DropDownStyle = ComboBoxStyle.DropDown;

            DataRow dr;
            dr = dtQuote.NewRow();
            dr["CompanyCode"] = "P";
            dr["QuotationNo"] = "(New)";
            if (nPSw == 3)
            {
                dr["SponsorID"] = pSponsorID;
                dr["ContactID"] = pContactID;
                if (pSpID != 0)
                {
                    dr["SecSponsorID"] = pSpID;
                    dr["SecContactID"] = pConID;
                }
            }
            else
            {
                dr["SponsorID"] = DBNull.Value;
                dr["ContactID"] = DBNull.Value;
                dr["SecSponsorID"] = DBNull.Value;
                dr["SecContactID"] = DBNull.Value;
            }
            dr["HideTerms"] = false;
            dr["ExcFollowUp"] = false;
            dr["Comments"] = "";
            dr["PONo"] = "";
            dr["DateCreated"] = DateTime.Now;
            dr["CreatedByID"] = LogIn.nUserID;
            dr["LastUpdate"] = DateTime.Now;
            dr["LastUserID"] = LogIn.nUserID;
            dtQuote.Rows.Add(dr);
            bsQuote.DataSource = dtQuote;
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            txtCmpyCode.DataBindings.Add("Text", bsQuote, "CompanyCode");
            txtQuoteNo.DataBindings.Add("Text", bsQuote, "QuotationNo");
            txtSponsorID.DataBindings.Add("Text", bsQuote, "SponsorID", true);
            txtContactID.DataBindings.Add("Text", bsQuote, "ContactID", true);
            txtSpID.DataBindings.Add("Text", bsQuote, "SecSponsorID", true);
            txtConID.DataBindings.Add("Text", bsQuote, "SecContactID", true);
            chkHideTerms.DataBindings.Add("Checked", bsQuote, "HideTerms", true);
            chkExclude.DataBindings.Add("Checked", bsQuote, "ExcFollowUp", true);
            txtComments.DataBindings.Add("Text", bsQuote, "Comments");
            txtPONo.DataBindings.Add("Text", bsQuote, "PONo");

            chkAdvance.Checked = false;

            btnAddRev_Click(null, null);
            if (nPSw != 3)
            {
                txtSponsor.Select(); txtSponsor.SelectAll();
            }
            spbTestDesc.IsReadOnly = false; spbTestComments.IsReadOnly = false;
            spbComBefTable.IsReadOnly = false; spbComAftTable.IsReadOnly = false; spbComNonPrint.IsReadOnly = false;
            btnEMailQ.Enabled = false;
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;
            //check if other user(s) is/are editing the same record
            DataTable dtX = PSSClass.General.CheckEditMode("Quotations", txtQuoteNo.Text, LogIn.strUserID);
            if (dtX != null && dtX.Rows.Count >= 1)
            {
                string strU = ""; int nX = 1;
                foreach (DataRow dRow in dtX.Rows)
                {
                    strU += nX.ToString() + ". " + dRow["UserLogID"] + Environment.NewLine;
                    nX++;
                }
                MessageBox.Show("The following user(s) is/are editing this record:" + Environment.NewLine + Environment.NewLine + strU + Environment.NewLine + "Please resolve changes to be made with other users.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                CancelSave();
                return;
            }
            dtX.Dispose();

            if (dtSponsors.Rows.Count == 0)
                LoadSponsorsDDL();

            if (dtSC.Rows.Count == 0)
                LoadSCDDL();

            if (pnlRecord.Visible == false)
            {
                LoadData();
            }
            try
            {
                LoadPrevQuotes();
            }
            catch
            {
                MessageBox.Show("Previous quotes cannot be loaded." + Environment.NewLine + "Sponsor ID is missing", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront(); pnlTestItems.Visible = false;
            OpenControls(pnlRecord, true);
            btnAddRev.Enabled = true; btnDelRev.Enabled = true;
            btnAddTestA.Enabled = true; btnAddTestB.Enabled = true; btnDelTest.Enabled = true; btnAddSubA.Enabled = true; btnAddSubB.Enabled = true; btnCopy.Enabled = true;
            bnRevisions.Enabled = true; btnAddRev.Enabled = true; btnDelRev.Enabled = true;//allow only 1 revision to be edited per session
            cboTestCat1.DropDownStyle = ComboBoxStyle.DropDown; cboTestCat2.DropDownStyle = ComboBoxStyle.DropDown;
            cboTestCat3.DropDownStyle = ComboBoxStyle.DropDown; cboTestCat4.DropDownStyle = ComboBoxStyle.DropDown;
            
            txtSC.Enabled = true; txtSCDesc.Enabled = true;
            btnClose.Visible = false;
            cboQuotes.Enabled = true; btnCopy.Enabled = true; pnlAdjPrice.Enabled = true;
            nMode = 2; spbTestDesc.IsReadOnly = false; spbTestComments.IsReadOnly = false;
            spbComBefTable.IsReadOnly = false; spbComAftTable.IsReadOnly = false; spbComNonPrint.IsReadOnly = false;
            //Add Edit Control for this record
            PSSClass.General.AddEditControl(1, "Quotations", txtQuoteNo.Text, LogIn.strUserID);
            btnEMailQ.Enabled = false;
        }

        private void DeleteRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;

            if (pnlRecord.Visible == false)
                LoadData();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this quotation?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ;
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.Add(new SqlParameter("@QuoteNo", SqlDbType.NVarChar));
                sqlcmd.Parameters["@QuoteNo"].Value = txtQuoteNo.Text;

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelQuote";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadRecords();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlTestItems.Visible = true; pnlRecord.Visible = false;
        }

        private void SaveRecord()
        {
            int nQ = ValidateQuote();//Validation for Quotation Master Record
            if (nQ == 0) //if validation failed, do not proceed to save
            {
                return;
            }

            nQ = ValidateRev(); //Revision Data Validation
            if (nQ == 0)
            {
                return;
            }
            nQ = ValidateTest(); //Test Items Validation
            if (nQ == 0)
            {
                return;
            }

            int nR = dtRevTests.Rows.Count;

            SaveQuotation();//Save Quotation Master Record
            if (nSave == 0) //if not successful, do not proceed to save revision record
            {
                nSave = 1;
                btnEMailQ.Enabled = true;
                return;
            }
            string strQ = txtQuoteNo.Text;
            UpdateQuotationRev();//Save Quotation Revision Record
            UpdateQuotationRevTest(); //Save Quotation Test Items
            pnlTestItems.Visible = false;
            btnClose.Visible = true; btnPrtPreview.Enabled = true;
            OpenControls(pnlRecord, false);
            AddEditMode(false);
            bnRevisions.Enabled = true;
            dtQuote.AcceptChanges();
            dtRevTests.AcceptChanges();
            dtRevisions.AcceptChanges();
            //Check for Group I SCs
            bool bPPay = false; bPPy = false;
            for (int i = 0; i < dtRevTests.Rows.Count; i++)
            {
                if (dtRevTests.Rows[i]["ServiceCode"] != null && dtRevTests.Rows[i]["ServiceCode"].ToString() != "")
                {
                    bPPay = PSSClass.ServiceCodes.SCPrepayItem(Convert.ToInt16(dtRevTests.Rows[i]["ServiceCode"]));
                    if (bPPay == true)
                        bPPy = true;
                }
            }
            if (cboRevStatus.Text == "Accepted" && lblInvoiced.Text == "(Invoiced)" && (chkPPay.Checked == true || bPPy == true))
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to create an invoice?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.Yes)
                {
                    strPPQ = txtQuoteNo.Text; 
                    txtPOInv.Text = ""; txtPOAmt.Text = "";
                    txtPOInv.Text = txtPONo.Text;
                    pnlRecord.Enabled = false;
                    pnlPO.Visible = true; pnlPO.Location = new Point(300, 300); pnlPO.BringToFront();
                    return;
                }
            }
            LoadRecords();//Reload updated records
            PSSClass.General.FindRecord("QuotationNo", strQ, bsFile, dgvFile);
            LoadData();
            //Release Edit Control for this record
            PSSClass.General.AddEditControl(0, "Quotations", txtQuoteNo.Text, LogIn.strUserID);
            //
            txtQuoteNo.Focus();
            bnRevisions.Enabled = true; btnEMailQ.Enabled = true;
            if (nPSw == 2 || nPSw == 3)
            {
                if (nPSw == 3)
                {
                    SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                    SqlCommand sqlcmd = new SqlCommand();

                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@QuoteNo", strQuoteNo);
                    sqlcmd.Parameters.AddWithValue("@ReQuoteNo", txtQuoteNo.Text);
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spUpdReQuoteNo";

                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                this.Close(); this.Dispose();
            }
        }

        private void SaveQuotation()
        {
            string strQuote = txtQuoteNo.Text;
            if (nMode == 1)
            {
                if (chkAdvance.Checked == false)
                    strQuote = PSSClass.Quotations.QuoteNo(0);
                else
                    strQuote = PSSClass.Quotations.QuoteNo(1);
                txtQuoteNo.Text = strQuote;
            }
            bsQuote.EndEdit();
            if (dtQuote.Rows[0].RowState.ToString() == "Added" || dtQuote.Rows[0].RowState.ToString() == "Modified")
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", nMode);
                sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim().Trim());
                sqlcmd.Parameters.AddWithValue("@QuoteNo", strQuote);
                sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
                sqlcmd.Parameters.AddWithValue("@ConID", Convert.ToInt16(txtContactID.Text));
                if (txtSpID.Text == "")
                    sqlcmd.Parameters.AddWithValue("@SecSpID", DBNull.Value);
                else
                    sqlcmd.Parameters.AddWithValue("@SecSpID", Convert.ToInt16(txtSpID.Text));
                if (txtConID.Text == "")
                    sqlcmd.Parameters.AddWithValue("@SecConID", DBNull.Value);
                else
                    sqlcmd.Parameters.AddWithValue("@SecConID", Convert.ToInt16(txtConID.Text));
                sqlcmd.Parameters.AddWithValue("@Comments", txtComments.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@PONo", txtPONo.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@ExclFollowUp", chkExclude.CheckState);
                sqlcmd.Parameters.AddWithValue("@Hide", chkHideTerms.CheckState);
                sqlcmd.Parameters.AddWithValue("@GLP", chkGLP.CheckState);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditQuote";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    if (ex.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        nSave = 0;
                        return;
                    }
                }
                sqlcmd.Dispose();sqlcnn.Close(); sqlcnn.Dispose();
                dtQuote.AcceptChanges();
                bsQuote.DataSource = dtQuote;
                nSave = 1;
            }
        }

        private void UpdateQuotationRev()
        {
            bsRevisions.EndEdit();
            int nAdded = 0; int nEdited = 0; int nDeleted = 0;

            DataTable dt = dtRevisions.GetChanges(DataRowState.Deleted);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nDelRev = DelQuotationRev(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, i, 3, dt);
                    nDeleted += nDelRev;
                }
            }
            dt = dtRevisions.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveQuotationRev(txtCmpyCode.Text.Trim().Trim(), txtQuoteNo.Text, i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            dt = dtRevisions.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveQuotationRev(txtCmpyCode.Text.Trim().Trim(), txtQuoteNo.Text, i, 2, dt);
                    nEdited += nSaveRev;
                }
                dt.Rows.Clear();
            }
        }

        private static int SaveQuotationRev(string cCmpy, string cQNo, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpy);
            sqlcmd.Parameters.AddWithValue("@QuoteNo", cQNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(cDT.Rows[cI]["RevisionNo"]));
            if (cDT.Rows[cI]["RevisionStatus"].ToString() == "0")
                sqlcmd.Parameters.AddWithValue("@RevStatus", 0);
            else if (cDT.Rows[cI]["RevisionStatus"].ToString() == "1")
                sqlcmd.Parameters.AddWithValue("@RevStatus", 1);
            else if (cDT.Rows[cI]["RevisionStatus"].ToString() == "2")
                sqlcmd.Parameters.AddWithValue("@RevStatus", 2);
            sqlcmd.Parameters.AddWithValue("@DteCreated", Convert.ToDateTime(cDT.Rows[cI]["DateCreated"]));
            if (cDT.Rows[cI]["RevisionStatus"].ToString() == "0")
            {
                sqlcmd.Parameters.AddWithValue("@DteAccepted", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@DteRejected", DBNull.Value);
            }
            else if (cDT.Rows[cI]["RevisionStatus"].ToString() == "1")
            {
                if (cDT.Rows[cI]["DateAccepted"] != null && cDT.Rows[cI]["DateAccepted"].ToString().Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@DteAccepted", Convert.ToDateTime(cDT.Rows[cI]["DateAccepted"]));
                else
                    sqlcmd.Parameters.AddWithValue("@DteAccepted", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@DteRejected", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@DteAccepted", DBNull.Value);
                if (cDT.Rows[cI]["DateRejected"] != null && cDT.Rows[cI]["DateRejected"].ToString().Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@DteRejected", Convert.ToDateTime(cDT.Rows[cI]["DateRejected"]));
                else
                    sqlcmd.Parameters.AddWithValue("@DteRejected", DBNull.Value);
            }
            if (cDT.Rows[cI]["RejectedCode"] == null)
                sqlcmd.Parameters.AddWithValue("@RejCode", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@RejCode", cDT.Rows[cI]["RejectedCode"]);
            if (cDT.Rows[cI]["RejectedDesc"] == null || cDT.Rows[cI]["RejectedDesc"].ToString() == "")
                sqlcmd.Parameters.AddWithValue("@RejDesc", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@RejDesc", cDT.Rows[cI]["RejectedDesc"]);
            if (cDT.Rows[cI]["YearsValid"].ToString() == "0")
                sqlcmd.Parameters.AddWithValue("@YrsValid", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@YrsValid", cDT.Rows[cI]["YearsValid"]);
            sqlcmd.Parameters.AddWithValue("@ComBefTable", cDT.Rows[cI]["CommentsBeforeTable"]);
            sqlcmd.Parameters.AddWithValue("@ComAftTable", cDT.Rows[cI]["CommentsAfterTable"]);
            sqlcmd.Parameters.AddWithValue("@ComNonPrint", cDT.Rows[cI]["CommentsNonPrinting"]);
            sqlcmd.Parameters.AddWithValue("@WithPP", cDT.Rows[cI]["WithPrepayment"]);
            sqlcmd.Parameters.AddWithValue("@TestCat1", cDT.Rows[cI]["TestCategory1"]);
            sqlcmd.Parameters.AddWithValue("@TestCat2", cDT.Rows[cI]["TestCategory2"]);
            sqlcmd.Parameters.AddWithValue("@TestCat3", cDT.Rows[cI]["TestCategory3"]);
            sqlcmd.Parameters.AddWithValue("@TestCat4", cDT.Rows[cI]["TestCategory4"]);
            sqlcmd.Parameters.AddWithValue("@AccRevFile", cDT.Rows[cI]["AcceptedRevFile"]);
            if (cDT.Rows[cI]["PriceCheck"].ToString() == "True")
                sqlcmd.Parameters.AddWithValue("@PriceCheck", true);
            else
                sqlcmd.Parameters.AddWithValue("@PriceCheck", false);
            sqlcmd.Parameters.AddWithValue("@CreatedByID", LogIn.nUserID);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditQuoteRev";
            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            {
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }

        private static int DelQuotationRev(string cCmpy, string cQNo, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }            
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            int nRNo;

            if (cDT.Rows[cI].RowState.ToString() == "Deleted")
                nRNo = Convert.ToInt16(cDT.Rows[cI]["RevisionNo", DataRowVersion.Original].ToString());
            else
                nRNo = Convert.ToInt16(cDT.Rows[cI]["RevisionNo"].ToString());

            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpy);
            sqlcmd.Parameters.AddWithValue("@QuoteNo", cQNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", nRNo);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelQuoteRev";

            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            { }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }

        private void UpdateQuotationRevTest()
        {
            bsRevTests.EndEdit();
            int nAdded = 0; int nEdited = 0; int nDeleted = 0;

            DataTable dt = dtRevTests.GetChanges(DataRowState.Deleted);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nDelRev = DelQuotationRevTest(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, i, 3, dt);
                    nDeleted += nDelRev;
                }
                dt.Rows.Clear();
            }
            dt = dtRevTests.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveQuotationRevTest(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            dt = dtRevTests.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveQuotationRevTest(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, i, 2, dt);
                    nEdited += nSaveRev;
                }
                dt.Rows.Clear();
            }
        }

        private static int SaveQuotationRevTest(string cCmpy, string cQNo, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpy);
            sqlcmd.Parameters.AddWithValue("@QuoteNo", cQNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", cDT.Rows[cI]["RevisionNo"]);
            sqlcmd.Parameters.AddWithValue("@TestNo", cDT.Rows[cI]["TestNo"]);
            sqlcmd.Parameters.AddWithValue("@CtrlNo", cDT.Rows[cI]["ControlNo"]);
            sqlcmd.Parameters.AddWithValue("@SubNo", cDT.Rows[cI]["SubTestNo"]);
            if (cDT.Rows[cI]["ServiceCode"].ToString() == "")
                sqlcmd.Parameters.AddWithValue("@SC", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@SC", cDT.Rows[cI]["ServiceCode"]);
            sqlcmd.Parameters.AddWithValue("@TestDesc1", cDT.Rows[cI]["TestDesc1"]);
            sqlcmd.Parameters.AddWithValue("@TestCom", cDT.Rows[cI]["TestComments"]);
            sqlcmd.Parameters.AddWithValue("@UnitID", cDT.Rows[cI]["UnitID"]);
            sqlcmd.Parameters.AddWithValue("@Qty", cDT.Rows[cI]["BillQuantity"]);
            sqlcmd.Parameters.AddWithValue("@UP", cDT.Rows[cI]["UnitPrice"]);
            sqlcmd.Parameters.AddWithValue("@Rush", cDT.Rows[cI]["Rush"]);
            sqlcmd.Parameters.AddWithValue("@RushPrice", cDT.Rows[cI]["RushPrice"]);
            sqlcmd.Parameters.AddWithValue("@Optional", cDT.Rows[cI]["OptionalTest"]);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            //Gross Profit
            sqlcmd.Parameters.AddWithValue("@UnitGrossProfit", cDT.Rows[cI]["UnitGrossProfit"]);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditQuoteRevTest";
            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            {
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }

        private static int DelQuotationRevTest(string cCmpy, string cQNo, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            int nRNo = Convert.ToInt16(cDT.Rows[cI]["RevisionNo", DataRowVersion.Original].ToString());
            int nCtrlNo = Convert.ToInt16(cDT.Rows[cI]["ControlNo", DataRowVersion.Original].ToString());
            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpy);
            sqlcmd.Parameters.AddWithValue("@QuoteNo", cQNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", nRNo);
            sqlcmd.Parameters.AddWithValue("@CtrlNo", nCtrlNo);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelQuoteRevTest";

            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            { }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }

        public int ValidateQuote()
        {
            if (txtSponsorID.Text.Trim() == "" || txtSponsor.Text.Trim() == "")
            {
                MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsorID.Focus();
                return 0;
            }

            if (txtContactID.Text.Trim() == "" || txtContact.Text.Trim() == "")
            {
                MessageBox.Show("Please select Contact.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtContact.Focus();
                return 0;
            }
            //Added 3/14/2016 to validate Contact when copied from old quote
            if (PSSClass.Contacts.ContactIsActive(Convert.ToInt16(txtContactID.Text)) == false)
            {
                MessageBox.Show("Contact is already inactive.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtContact.Focus();
                return 0;
            }
            return 1;
        }

        public int ValidateRev()
        {
            if (cboRevStatus.SelectedIndex == 1) //accepted
            {
                if (mskDateAccepted.MaskFull == false)
                {
                    MessageBox.Show("No date for accepted quotation.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    mskDateAccepted.Select(); mskDateAccepted.SelectAll();
                    return 0;
                }
            }
            else if (cboRevStatus.SelectedIndex == 2) //rejected
            {
                if (mskDateRejected.MaskFull == false)
                {
                    MessageBox.Show("No date for rejected quotation.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    mskDateRejected.Select(); mskDateRejected.SelectAll();
                    return 0;
                }
                if (cboReasons.SelectedIndex == 0)
                {
                    MessageBox.Show("Please enter reason for rejection.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboReasons.Select(); cboReasons.SelectAll();
                    return 0;
                }
            }
            try
            {
                int n = int.Parse(txtYears.Text); //Years of Validity
            }
            catch
            {
                MessageBox.Show("Invalid years entered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtYears.Select(); txtYears.SelectAll();
                return 0;
            }
            return 1;
        }

        public int ValidateTest()
        {
            if (txtTestNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter item number.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtTestNo.Focus();
                return 0;
            }

            if (txtSubNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter sub-item number.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSubNo.Focus();
                return 0;
            }
            return 1;
        }

        private void GetEmailBody(int cEmailType)
        {
            int nRevNo = Convert.ToInt16(txtRevNo.Text);
            string strCFName = PSSClass.Quotations.ContactFirstName(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, nRevNo);


            //Check for Sterikits SCs 
            string strSkit = "0";
            DataTable dtSKit = new DataTable();
            dtSKit = PSSClass.Quotations.QuoteSterikits(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, nRevNo);
            if (dtSKit != null && dtSKit.Rows.Count > 0)
            {
                strSkit = "1";
                dtSKit.Dispose();
            }

            //Check for Sterilization SCs 
            DataTable dtSter = new DataTable();
            dtSter = PSSClass.Quotations.QuoteSterilization(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, nRevNo);
            if (dtSter != null && dtSter.Rows.Count > 0)
            {
                strSkit = "2";
                dtSter.Dispose();
            }

            //Check for ONLINE SSF users 
            DataTable dtOSSF = new DataTable();
            dtOSSF = PSSClass.Quotations.QuoteOnlineSSF(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, nRevNo);
            if (dtOSSF != null && dtOSSF.Rows.Count > 0 && strSkit == "0")
            {
                strSkit = "3";
                dtOSSF.Dispose();
            }
            //Check for new Sponsor
            DataTable dtNewSp = new DataTable();
            dtNewSp = PSSClass.Quotations.QuoteNewSp(Convert.ToInt16(txtSponsorID.Text));
            if (dtNewSp == null || dtNewSp.Rows.Count == 0 && strSkit == "0")
            {
                strSkit = "3";
                dtNewSp.Dispose();
            }
            txtBody.Text = "";
            if (cEmailType == 1)
            {
                if (strSkit == "0")
                    txtBody.Text = "Dear " + strCFName + "," + Environment.NewLine + Environment.NewLine +
                        "I have attached our quote " + txtCmpyCode.Text.Trim().Trim() + txtQuoteNo.Text + ".R" + txtRevNo.Text + " for your review. Please check to see if it accurately reflects your order." +
                        Environment.NewLine + "If so, please sign the quote and email (preferred) or fax it to my attention: (973) 227-0812. If not, please let us know " +
                        Environment.NewLine + "what corrections you have." +Environment.NewLine + Environment.NewLine +
                         "Testing cannot begin unless all of the paperwork (Purchase Order, Quote) is signed and returned to us. Please return " +
                        Environment.NewLine + "the signed documents to my attention." + Environment.NewLine + Environment.NewLine + "Terms Policy is available at " +
                        "our website: http://princesterilization.com." + " Acceptance of this quote constitutes acceptance of PRINCE's Terms Policy." + Environment.NewLine + Environment.NewLine + "When you are ready to send samples, please complete the sample " +
                        "submission form and send it together with the samples." + Environment.NewLine + Environment.NewLine + "Please let me know if you have any questions.";
                else if (strSkit == "1")
                    txtBody.Text = "Dear " + strCFName + "," + Environment.NewLine + Environment.NewLine +
                        "I have attached our quote " + txtCmpyCode.Text.Trim().Trim() + txtQuoteNo.Text + ".R" + txtRevNo.Text + " for your review. Please check to see if it accurately reflects your order." +
                        Environment.NewLine + "If so, please sign the quote and email (preferred) or fax it to my attention: (973) 227-0812. " + 
                        "If not, please let us know " +
                        Environment.NewLine + "what corrections you have." + Environment.NewLine + Environment.NewLine + 
                        "Processing of your order will not begin unless all of the paperwork (Purchase Order, Quote) is signed and returned" +
                        Environment.NewLine + "to us. Please return the signed documents to my attention." + 
                        Environment.NewLine + Environment.NewLine + "Terms Policy is available at our website: http://princesterilization.com. " +
                        "Acceptance of this quote constitutes acceptance of PRINCE's Terms Policy." + Environment.NewLine + Environment.NewLine + "Please let me know if you have any questions.";
                else if (strSkit == "2")
                    txtBody.Text = "Dear " + strCFName + "," + Environment.NewLine + Environment.NewLine +
                        "I have attached our quote " + txtCmpyCode.Text.Trim().Trim() + txtQuoteNo.Text + ".R" + txtRevNo.Text + " for your review. Please check to see if it accurately reflects your order." +
                        Environment.NewLine + "If so, please sign the quote and email (preferred) or fax it to my attention: (973) 227-0812. If not, please let us know " +
                        Environment.NewLine + "what corrections you have." + Environment.NewLine + Environment.NewLine +
                        "Processing of your order will not begin unless all of the paperwork (Purchase Order, Quote) is signed and returned to us." +
                        Environment.NewLine + "Please return the signed documents to my attention." + Environment.NewLine + Environment.NewLine + "Terms Policy is available at " +
                        "our website: http://princesterilization.com. " +
                        "Acceptance of this quote constitutes acceptance of PRINCE's Terms Policy." + Environment.NewLine + Environment.NewLine + "When you are ready to send samples, please complete the sample " +
                        "submission form and send it together with the samples." + Environment.NewLine + Environment.NewLine + "Please let me know if you have any questions.";
                else if (strSkit == "3")
                    txtBody.Text = "Dear " + strCFName + "," + Environment.NewLine + Environment.NewLine +
                         "I have attached our quote " + txtCmpyCode.Text.Trim().Trim() + txtQuoteNo.Text + ".R" + txtRevNo.Text + " for your review. Please check to see if it accurately reflects the services" +
                         Environment.NewLine + "that you are requesting. If so, please sign the quote and email (preferred) or fax it to my attention: (973) 227-0812." +
                         Environment.NewLine + "If not, please let us know what corrections you have." + 
                         Environment.NewLine + Environment.NewLine +
                         "Testing cannot begin unless all of the paperwork (Purchase Order, Quote) is signed and returned to us. Please return " + 
                         Environment.NewLine +  "the signed documents to my attention." +
                         Environment.NewLine + Environment.NewLine + "Terms Policy is available at our website: http://princesterilization.com. " +
                         Environment.NewLine + "Acceptance of this quote constitutes acceptance of PRINCE's Terms Policy." + 
                         //"When you are ready to send samples, please log in at https://ssf.gibraltarlabsinc.com/ssf/ using your login credentials" +
                         //Environment.NewLine + "to input the test samples information online, print the sample submission form from the system and send it together with" +
                         //Environment.NewLine + "the samples. If you do not have login credentials, please call (973) 582-1514 or email us at ssf@gibraltarlabsinc.com." +
                         Environment.NewLine + Environment.NewLine + "Please let me know if you have any questions.";
            }
            else
            {
                txtBody.Text = "Dear " + strCFName + "," + Environment.NewLine + Environment.NewLine +
                               "I am following up on the attached quote you requested (" + txtCmpyCode.Text.Trim().Trim() + txtQuoteNo.Text + ".R" + txtRevNo.Text + ") - any news on whether you will be sending " +
                               "this work or order to us? " + Environment.NewLine + Environment.NewLine + "Our records indicate that we do not have a signed copy of the quote for our files. At your convenience, please e-mail (preferred) or " +
                               Environment.NewLine + "fax a signed copy to my attention:(973) 227-0812. If you have elected not to send us the work, we value any input as to why and " + 
                               Environment.NewLine + "ask if you could please let us know by checking off  one of the below:" + Environment.NewLine + Environment.NewLine +
                               "[  ] price not competitive  [  ] facility too far away  [  ] work not necessary anymore  [  ] Other: ______________________________";
            }
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
                bsFile.CancelEdit(); bsRevisions.CancelEdit(); bsRevTests.CancelEdit();
            }
            dtQuote.RejectChanges(); dtRevisions.RejectChanges(); dtRevTests.RejectChanges();
            if (nMode == 1)
                bsFile.Position = 0;
            LoadData();
            //Release Edit Control for this record
            PSSClass.General.AddEditControl(0, "Quotations", txtQuoteNo.Text, LogIn.strUserID);
            //
            bnFile.Enabled = true; btnClose.Visible = true;
            dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvServices.Visible = false; dgvUnits.Visible = false;
            bnRevisions.Enabled = true;
            AddEditMode(false);
            FileAccess();
            nMode = 0; bnRevisions.Enabled = true; btnEMailQ.Enabled = true;
           
            if (nPSw == 2 || nPSw == 3)
            {
                this.Close(); this.Dispose();
                if (nPSw == 3)
                {
                    int intOpen = PSSClass.General.OpenForm(typeof(ExpiringQuotes));

                    if (intOpen == 0)
                    {
                        ExpiringQuotes childForm = new ExpiringQuotes();
                        childForm.MdiParent = Program.mdi;
                        childForm.Text = "EXPIRING QUOTES";
                        childForm.Show();
                    }
                }
            }
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

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            LoadRecords();
            tsbRefresh.Enabled = false;
        }

        public void SearchOKClickHandler(object sender, EventArgs e)
        {
            try
            {
                bsFile.Filter = "QuotationNo<>''";
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

        private void LoadRecords()
        {
            nMode = 0;
            DataTable dtQ = PSSClass.Quotations.QuotationsMaster();
            bsFile.DataSource = dtQ;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "QuoteNo <> ''";
            dgvFile.Select();
            DataGridSetting();
            if (tsddbSearch.DropDownItems.Count == 0)
            {
                int i = 0;
                int n = 0;

                arrCol = new string[dtQ.Columns.Count];

                ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

                foreach (DataColumn colFile in dtQ.Columns)
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
            FileAccess();
        }

        private void PrintRptClickHandler(object sender, EventArgs e)
        {
            if (rptName.IndexOf("Audit") != -1)
            {
                SalesRpt rptSC = new SalesRpt();
                rptSC.WindowState = FormWindowState.Maximized;
                
                if (rptTitle == "Audit Trail - Quotations Master File")
                {
                    rptSC.rptFileName = "QUOTATIONS MASTER FILE";
                    rptSC.rptName = "Audit Trail - Quotations Master File";
                    rptSC.rptLabel = "AUDIT TRAIL - QUOTATION MASTER FILE";
                }
                else if (rptTitle == "Audit Trail - Quotation Revisions")
                {
                    rptSC.rptFileName = "QUOTATION REVISIONS";
                    rptSC.rptName = "Audit Trail - Quotation Revisions";
                    rptSC.rptLabel = "AUDIT TRAIL - QUOTATION REVISIONS";
                }
                else if (rptTitle == "Audit Trail - Quotation Test Items")
                {
                    rptSC.rptFileName = "QUOTATION TEST ITEMS";
                    rptSC.rptName = "Audit Trail - Quotation Test Items";
                    rptSC.rptLabel = "AUDIT TRAIL - QUOTATION TEST ITEMS";
                }
                rptSC.Show();
            }
            else
            {
                SalesRptSettings rpt = new SalesRptSettings();
                rpt.WindowState = FormWindowState.Normal;
                string s = sender.GetType().ToString();
                rpt.rptTitle = rptTitle.Replace("&&", "&");
                rpt.rptName = rptName;
                rpt.Text = rptTitle.Replace("&&", "&");
                rpt.ShowDialog();
            }
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["CompanyCode"].HeaderText = "CODE";
            dgvFile.Columns["QuoteNo"].HeaderText = "QUOTE NO.";
            dgvFile.Columns["Mailed"].HeaderText = "MAILED";
            dgvFile.Columns["DateEMailed"].HeaderText = "DATE E-MAILED";
            dgvFile.Columns["Status"].HeaderText = "STATUS";
            dgvFile.Columns["WithPrePayment"].HeaderText = "PREPAYMENT";
            dgvFile.Columns["PrepayInvoiced"].HeaderText = "INVOICED";
            dgvFile.Columns["DateCreated"].HeaderText = "REV. DATE";
            dgvFile.Columns["SponsorName"].HeaderText = "PRIMARY SPONSOR";
            dgvFile.Columns["ContactName"].HeaderText = "PRIMARY CONTACT";
            dgvFile.Columns["CommentsBeforeTable"].HeaderText = "REVISION COMMENTS";
            dgvFile.Columns["SecSponsorName"].HeaderText = "SECONDARY SPONSOR";
            dgvFile.Columns["SecContactName"].HeaderText = "SECONDARY CONTACT";
            dgvFile.Columns["CreatedBy"].HeaderText = "CREATED BY";
            dgvFile.Columns["Comments"].HeaderText = "COMMENTS";
            dgvFile.Columns["QuotationNo"].HeaderText = "QUOTE NO.";
            dgvFile.Columns["RevisionNo"].HeaderText = "REV. NO.";
            dgvFile.Columns["CompanyCode"].Width = 50;
            dgvFile.Columns["QuoteNo"].Width = 100;
            dgvFile.Columns["QuoteNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["Mailed"].Width = 75;
            dgvFile.Columns["Mailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateEMailed"].Width = 75;
            dgvFile.Columns["DateEMailed"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateEMailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["Status"].Width = 75;
            dgvFile.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["WithPrePayment"].Width = 90;
            dgvFile.Columns["PrepayInvoiced"].Width = 75;
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateCreated"].Width = 75;
            dgvFile.Columns["SponsorName"].Width = 300;
            dgvFile.Columns["ContactName"].Width = 200;
            dgvFile.Columns["CommentsBeforeTable"].Width = 300;
            dgvFile.Columns["SecSponsorName"].Width = 200;
            dgvFile.Columns["SecContactName"].Width = 297;
            dgvFile.Columns["CreatedBy"].Width = 85;
            dgvFile.Columns["Comments"].Width = 85;
            dgvFile.Columns["HideTerms"].Visible = false;
            dgvFile.Columns["GLP"].Visible = false;
            dgvFile.Columns["ExcFollowUp"].Visible = false;
            dgvFile.Columns["SponsorID"].Visible = false;
            dgvFile.Columns["ContactID"].Visible = false;
            dgvFile.Columns["SecSponsorID"].Visible = false;
            dgvFile.Columns["SecContactID"].Visible = false;
            dgvFile.Columns["QuotationNo"].Visible = false;
            dgvFile.Columns["RevisionNo"].Visible = false;
            dgvFile.Columns["PONo"].Visible = false;
            dgvFile.Columns["CmpyQuote"].Visible = false;
            dgvFile.Dock = DockStyle.None;
            dgvFile.Size = new Size(1330, 475);
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
            dgvSponsors.Columns[0].Width = 367;
            dgvSponsors.Columns[1].Visible = false;
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
            dgvContacts.Columns[0].Width = 367;
            dgvContacts.Columns[1].Visible = false;
        }

        private void LoadSCDDL()
        {
            dtSC = PSSClass.ServiceCodes.SCDDL();
            if (dtSC == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                nMode = 9;
                return;
            }
            dgvServices.DataSource = dtSC;
            DataView dv = new DataView(dtSC);
            PSSClass.General.DGVSetUp(dgvServices, dv, 416);
        }

        private void LoadPrevQuotes()
        {
            cboQuotes.DataSource = null;

            DataTable dtQ = new DataTable();
            dtQ = PSSClass.Quotations.LoadPrevQoutes();
            if (dtQ == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            cboQuotes.DataSource = dtQ;
            cboQuotes.SelectedIndex = -1;
            cboQuotes.DisplayMember = "QuoteNo";
            cboQuotes.ValueMember = "CompanyCode";
            strCopyQuote = "";
        }

        private void LoadRevisions(string cCmpy, string cQN)
        {
            if (cboTestCat1.Items.Count == 0)
            {
                LoadColHeaders();
            }

            dtRevisions = PSSClass.Quotations.QuoteRev(cCmpy, cQN.Trim());
            bsRevisions.DataSource = dtRevisions;
            bnRevisions.BindingSource = bsRevisions;
            bsRevisions.Position = 0;

            txtRevNo.Text = dtRevisions.Rows[bsRevisions.Position]["RevisionNo"].ToString();
            if (dtRevisions.Rows[bsRevisions.Position]["PrepayInvoiceNo"] != null && dtRevisions.Rows[bsRevisions.Position]["PrepayInvoiceNo"].ToString().Trim() != "")
            {
                lblInvoiced.Text = "(Inv. No. " + dtRevisions.Rows[bsRevisions.Position]["PrepayInvoiceNo"].ToString() + ")";
                lblInvoiced.Visible = true;
            }
            else
                lblInvoiced.Visible = false;
            
            lblRNo.Text = "R" + txtRevNo.Text;

            if (txtDateEMailed.Text != null && txtDateEMailed.Text != "")
            {
                lblEStatus.Text = "Sent";
            }
            else if (cboRevStatus.Text == "Pending")
            {
                lblEStatus.Text = "Not Sent"; 
            }
            if (txtHideEstTotal.Text == "True")
                chkHideEstTotal.Checked = true;
            else
                chkHideEstTotal.Checked = false;

        }

        private void LoadRevTests(string cCmpy, string cQN, int cRN)
        {
            try
            {
                if (dtSC.Rows.Count == 0)
                    LoadSCDDL();

                dtRevTests = PSSClass.Quotations.LoadRevTests(cCmpy, cQN, cRN);
                bsRevTests.DataSource = dtRevTests;
                bnRevTests.BindingSource = bsRevTests;

                BindRevTest();
                lblRevTests.Text = "R" + cRN.ToString();
                lblRev.Text = cQN + ".R" + cRN.ToString();

                if (txtSC.Text != "")
                    txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
                else
                    txtSCDesc.Text = "";

                lblRevTests.Text = "R" + txtRevNo.Text;
                EstimatedTotal();
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        private void LoadData()
        {
            try
            {
                nMode = 0;
                OpenControls(pnlRecord, false);
                lblInvoiced.Text = "(Invoiced)";
                LoadQuote();

                LoadRevisions(txtCmpyCode.Text.Trim(), txtQuoteNo.Text);
                LoadRevTests(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, Convert.ToInt16(txtRevNo.Text));
                pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvServices.Visible = false;
                btnAddRev.Enabled = false; btnDelRev.Enabled = false;
                btnAddTestA.Enabled = false; btnAddTestB.Enabled = false; btnDelTest.Enabled = false; btnPaste.Enabled = false; btnAddSubA.Enabled = false; btnAddSubB.Enabled = false;
                cboQuotes.Enabled = false; btnCopy.Enabled = false;
                btnClose.Visible = true; pnlTestItems.Visible = false; pnlAdjPrice.Enabled = false;
                dgvServices.Visible = false;
                cboTestCat1.DropDownStyle = ComboBoxStyle.Simple; cboTestCat2.DropDownStyle = ComboBoxStyle.Simple;
                cboTestCat3.DropDownStyle = ComboBoxStyle.Simple; cboTestCat4.DropDownStyle = ComboBoxStyle.Simple;
                chkAdvance.Checked = false; txtPercent.Text = "";
                spbTestDesc.IsReadOnly = true; spbTestComments.IsReadOnly = true;
                spbComBefTable.IsReadOnly = true; spbComAftTable.IsReadOnly = true; spbComNonPrint.IsReadOnly = true;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void LoadQuote()
        {
            dtQuote.Rows.Clear();
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            DataRow dr;
            dr = dtQuote.NewRow();
            dr["CompanyCode"] = dgvFile.CurrentRow.Cells["CompanyCode"].Value.ToString();
            dr["QuotationNo"] = dgvFile.CurrentRow.Cells["QuotationNo"].Value.ToString();
            dr["RevisionNo"] = Convert.ToInt16(dgvFile.CurrentRow.Cells["RevisionNo"].Value.ToString());
            if (dgvFile.CurrentRow.Cells["SponsorID"].Value != null && dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString().Trim() != "")
                dr["SponsorID"] = Convert.ToInt16(dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString());
            else
                dr["SponsorID"] = DBNull.Value;
            if (dgvFile.CurrentRow.Cells["SponsorName"].Value != null && dgvFile.CurrentRow.Cells["SponsorName"].Value.ToString().Trim() != "")
                dr["SponsorName"] = dgvFile.CurrentRow.Cells["SponsorName"].Value.ToString();
            else
                dr["SponsorName"] = DBNull.Value;
            if (dgvFile.CurrentRow.Cells["ContactID"].Value != null && dgvFile.CurrentRow.Cells["ContactID"].Value.ToString().Trim() != "")
                dr["ContactID"] = Convert.ToInt16(dgvFile.CurrentRow.Cells["ContactID"].Value.ToString());
            else
                dr["ContactID"] = DBNull.Value;
            if (dgvFile.CurrentRow.Cells["ContactName"].Value != null && dgvFile.CurrentRow.Cells["ContactName"].Value.ToString().Trim() != "")
                dr["ContactName"] = dgvFile.CurrentRow.Cells["ContactName"].Value.ToString();
            else
                dr["ContactName"] = DBNull.Value;
            if (dgvFile.CurrentRow.Cells["SecSponsorID"].Value != null && dgvFile.CurrentRow.Cells["SecSponsorID"].Value.ToString().Trim() != "")
                dr["SecSponsorID"] = Convert.ToInt16(dgvFile.CurrentRow.Cells["SecSponsorID"].Value.ToString());
            else
                dr["SecSponsorID"] = DBNull.Value;
            if (dgvFile.CurrentRow.Cells["SecSponsorName"].Value != null && dgvFile.CurrentRow.Cells["SecSponsorName"].Value.ToString().Trim() != "")
                dr["SecSponsorName"] = dgvFile.CurrentRow.Cells["SecSponsorName"].Value.ToString();
            else
                dr["SecSponsorName"] = DBNull.Value;
            if (dgvFile.CurrentRow.Cells["SecContactID"].Value != null && dgvFile.CurrentRow.Cells["SecContactID"].Value.ToString().Trim() != "")
                dr["SecContactID"] = Convert.ToInt16(dgvFile.CurrentRow.Cells["SecContactID"].Value.ToString());
            else
                dr["SecContactID"] = DBNull.Value;
            if (dgvFile.CurrentRow.Cells["SecContactName"].Value != null && dgvFile.CurrentRow.Cells["SecContactName"].Value.ToString().Trim() != "")
                dr["SecContactName"] = dgvFile.CurrentRow.Cells["SecContactName"].Value.ToString();
            else
                dr["SecContactName"] = DBNull.Value;
            dr["HideTerms"] = dgvFile.CurrentRow.Cells["HideTerms"].Value;
            dr["GLP"] = dgvFile.CurrentRow.Cells["GLP"].Value;
            dr["ExcFollowUp"] = dgvFile.CurrentRow.Cells["ExcFollowUp"].Value;
            dr["Comments"] = dgvFile.CurrentRow.Cells["Comments"].Value;
            dr["PONo"] = dgvFile.CurrentRow.Cells["PONo"].Value;
            dtQuote.Rows.Add(dr);
            bsQuote.DataSource = dtQuote;
            //DataBindings
            txtCmpyCode.DataBindings.Add("Text", bsQuote, "CompanyCode");
            txtQuoteNo.DataBindings.Add("Text", bsQuote, "QuotationNo");
            txtSponsorID.DataBindings.Add("Text", bsQuote, "SponsorID", true);
            txtSponsor.DataBindings.Add("Text", bsQuote, "SponsorName", true);
            txtContactID.DataBindings.Add("Text", bsQuote, "ContactID", true);
            txtContact.DataBindings.Add("Text", bsQuote, "ContactName", true);
            txtSpID.DataBindings.Add("Text", bsQuote, "SecSponsorID", true);
            txtSp.DataBindings.Add("Text", bsQuote, "SecSponsorName", true);
            txtConID.DataBindings.Add("Text", bsQuote, "SecContactID", true);
            txtCon.DataBindings.Add("Text", bsQuote, "SecContactName", true);
            chkHideTerms.DataBindings.Add("Checked", bsQuote, "HideTerms", true);
            chkGLP.DataBindings.Add("Checked", bsQuote, "GLP", true);
            chkExclude.DataBindings.Add("Checked", bsQuote, "ExcFollowUp", true);
            txtComments.DataBindings.Add("Text", bsQuote, "Comments", true);
            txtPONo.DataBindings.Add("Text", bsQuote, "PONo", true);
            txtRevNo.Text = dgvFile.CurrentRow.Cells["RevisionNo"].Value.ToString();
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
                    txtCmpyCode.Text = dgvFile.CurrentRow.Cells["CompanyCode"].Value.ToString().Trim();
                    txtQuoteNo.Text = dgvFile.CurrentRow.Cells["QuotationNo"].Value.ToString();
                    string strRevNo = dgvFile.CurrentRow.Cells["RevisionNo"].Value.ToString();
                    nIndex = dgvFile.CurrentCell.ColumnIndex;

                    tsddbSearch.DropDownItems[nIndex].Select();
                    tslSearchData.Text = tsddbSearch.DropDownItems[nIndex].Text;
                    tstbSearchField.Text = tsddbSearch.DropDownItems[nIndex].Name;

                    LoadRevTests(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, Convert.ToInt16(strRevNo));

                    bsTestItems.DataSource = dtRevTests;
                    bnTestItems.BindingSource = bsTestItems;
                    dtrTestItems.DataSource = bsTestItems;
                    if (dgvFile.Visible == true)
                    {
                        decimal nTAmt = 0;
                        nTAmt = Convert.ToDecimal(dtRevTests.Compute("Sum(Amount)", "Amount > 0"));
                        lblTotalAmt.Text = "Total Estimated Amount: $ " + nTAmt.ToString("###,##0.00"); 
                        pnlTestItems.Visible = true;
                    }
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

        private void CloseClickHandler(object sender, EventArgs e)
        {
            if (nPSw == 1)
            {
                this.Dispose();
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
            nMode = 0;
            this.Close();
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void BuildPrintItems()
        {
            //Create Print Menu Dropdown List
            if (tsddbPrint.DropDownItems.Count == 0)
            {
                DataTable dt = PSSClass.General.ReportsList("Quotations");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tsddbPrint.DropDownItems.Add(dt.Rows[i]["ReportTitle"].ToString(), null, PrintRptClickHandler);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tsddbPrint.DropDownItems[i].Name = dt.Rows[i]["ReportName"].ToString();
                    }
                }
            }
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

        private void txtContactID_TextChanged(object sender, EventArgs e)
        {
            if (txtContactID.Text.Trim() == "")
            {
                txtContact.Text = "";
            }
        }

        private void txtSpID_TextChanged(object sender, EventArgs e)
        {
            if (txtSpID.Text.Trim() == "")
            {
                txtSp.Text = "";
            }
        }

        private void txtSCDescTextChangedHandler(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    DataView dvwSC;
                    if (nS == 1)
                    {
                        nS = 0;
                        dvwSC = new DataView(dtSC, "ServiceDesc = '" + txtSCDesc.Text.Trim() + "'", "ServiceDesc", DataViewRowState.CurrentRows);
                    }
                    else
                    {
                        if (chkWSearch.Checked == true)
                            dvwSC = new DataView(dtSC, "ServiceDesc like '%" + txtSCDesc.Text.Trim() + "%'", "ServiceDesc", DataViewRowState.CurrentRows);
                        else
                            dvwSC = new DataView(dtSC, "ServiceDesc like '" + txtSCDesc.Text.Trim() + "%'", "ServiceDesc", DataViewRowState.CurrentRows);
                    }
                    PSSClass.General.DGVSetUp(dgvServices, dvwSC, 416);
                }
                catch { }
            }
        }

        private void txtUnitTextChangedHandler(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    DataView dvwUnits;
                    if (nU == 1)
                    {
                        nU = 0;
                        dvwUnits = new DataView(dtSC, "UnitDesc ='" + txtSCDesc.Text.Trim() + "'", "UnitDesc", DataViewRowState.CurrentRows);
                    }
                    else
                    {
                        dvwUnits = new DataView(dtUnits, "UnitDesc like '" + txtUnit.Text.Trim() + "%'", "UnitDesc", DataViewRowState.CurrentRows);
                    }
                    PSSClass.General.DGVSetUp(dgvUnits, dvwUnits, 270);
                }
                catch { }
            }
        }

        private void dgvContacts_DoubleClick(object sender, EventArgs e)
        {
            if (dgvContacts.Rows.Count == 0)
            {
                MessageBox.Show("No Sponsor selected. " + Environment.NewLine + "Contacts list is empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (dgvContacts.Top == 113)
                    txtSponsorID.Focus();
                else
                    txtSpID.Focus();
                return;
            }
            if (nSp == 1)//dgvContacts.Top == 117
            {
                txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
                txtContactID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
                txtComments.Focus();
            }
            else
            {
                txtCon.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
                txtConID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
            }
            dgvContacts.Visible = false; dgvSponsors.Visible = false;
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
                if (nSp == 1)//dgvContacts.Top == 117
                {
                    txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
                    txtContactID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
                    txtComments.Focus();
                }
                else
                {
                    txtCon.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
                    txtConID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
                }
                dgvContacts.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvContacts.Visible = false;
            }
        }

        private void txtContactID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                    try
                    {
                        txtContact.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
                        dgvContacts.Visible = false; txtComments.Focus();
                    }
                    catch { }
                else if (e.KeyChar == 27)
                {
                    dgvContacts.Visible = false;
                }
                else
                {
                    txtContact.Text = "";
                }
            }
        }

        private void txtConID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                    try
                    {
                        txtCon.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtConID.Text), Convert.ToInt16(txtSpID.Text));
                        dgvContacts.Visible = false; txtComments.Focus();
                    }
                    catch { }
                else if (e.KeyChar == 27)
                {
                    dgvContacts.Visible = false;
                }
                else
                {
                    txtCon.Text = "";
                }
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

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 27)
                    dgvSponsors.Visible = false;
                else
                    txtSponsorID.Text = "";
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (nPSw == 1)
            {
                nPSw = 0;
                SendKeys.Send("{F12}");
                return;
            }
            if (nMode != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task(s)?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
                //Release Edit Control for this record
                PSSClass.General.AddEditControl(0, "Quotations", txtQuoteNo.Text, LogIn.strUserID);
                //
            }
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); pnlTestItems.Visible = true; btnClose.Visible = false; dgvFile.Focus();
            dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvServices.Visible = false;
            AddEditMode(false);
            FileAccess();
            bnRevisions.Enabled = true; 
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

        private void pnlRecord_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
                return;

            string strX = cboQuotes.Text.ToUpper();

            if (strX.IndexOf("R") == -1)
            {
                MessageBox.Show("Invalid quotation format.");
                return;
            }
            
            string strCmpy = "", strQuote = "", strRev = "";

            try
            {
                strCmpy = cboQuotes.SelectedValue.ToString();
                strQuote = cboQuotes.Text.Substring(0, 9);
                strRev = cboQuotes.Text.Substring(strQuote.Length + 1, cboQuotes.Text.Length - (strQuote.Length + 1));
                strRev = strRev.Trim().Substring(1, strRev.Trim().Length - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (strRev == "")
            {
                MessageBox.Show("Invalid revision number.");
                return;
            }
            bsRevTests.EndEdit();
            int nControlNo = 1, nTestNo = 0;

            if (dtRevTests.Rows.Count > 0)
            {
                for (int i = 0; i < dtRevTests.Rows.Count; i++)
                {
                    if (dtRevTests.Rows[i].RowState.ToString() != "Deleted" && Convert.ToInt16(dtRevTests.Rows[i]["ControlNo"].ToString()) >= nControlNo)
                        nControlNo = Convert.ToInt16(dtRevTests.Rows[i]["ControlNo"].ToString());
                }
            }
            DataView dvw = dtRevTests.DefaultView;
            dvw.Sort = "TestNo, SubTestNo";
            DataTable dT = dvw.ToTable();
            if (dT.Rows.Count > 0)
            {
                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dT.Rows[i]["TestNo"].ToString()) >= nTestNo)
                        nTestNo = Convert.ToInt16(dT.Rows[i]["TestNo"].ToString());
                }
            }
            dT.Dispose();

            if (dtRevTests.Rows.Count > 0)
                nControlNo++;

            if (nRev == 1)
            {
                strCmpy = txtCmpyCode.Text.Trim();
                strQuote = txtQuoteNo.Text;
                strRev = (Convert.ToInt16(txtRevNo.Text) - 1).ToString();
                nRev = 0;
            }
            DataTable dtX = new DataTable();
            dtX = PSSClass.Quotations.LoadRevTests(strCmpy, strQuote, Convert.ToInt16(strRev));

            if (dtX == null || dtX.Rows.Count == 0)
            {
                MessageBox.Show("No matching quotation number.");
                return;
            }

            DataView dvwX = dtX.DefaultView;
            dvwX.Sort = "TestNo, SubTestNo";
            DataTable dt = dvwX.ToTable();

            if (dt.Rows.Count > 0)
            {
                int nSv = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dt.Rows[i]["TestNo"]) != nSv)
                    {
                        nSv = Convert.ToInt16(dt.Rows[i]["TestNo"]);
                        nTestNo++;
                    }
                    DataRow dr;
                    dr = dtRevTests.NewRow();
                    dr["RevisionNo"] = txtRevNo.Text;
                    dr["ControlNo"] = nControlNo; 
                    dr["TestNo"] = nTestNo; 
                    if (dt.Rows[i]["SubTestNo"] != null && dt.Rows[i]["SubTestNo"].ToString().Trim() != "")
                        dr["SubTestNo"] = dt.Rows[i]["SubTestNo"];
                    else
                        dr["SubTestNo"] = 0;
                    if (dt.Rows[i]["ServiceCode"] != null &&  dt.Rows[i]["ServiceCode"].ToString().Trim() !="")
                        dr["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                    else
                        dr["ServiceCode"] = DBNull.Value;
                    if (dt.Rows[i]["TestDesc1"] != null && dt.Rows[i]["TestDesc1"].ToString().Trim() != "")
                        dr["TestDesc1"] = dt.Rows[i]["TestDesc1"];
                    else
                        dr["TestDesc1"] = DBNull.Value;
                    if (dt.Rows[i]["UnitID"] != null && dt.Rows[i]["UnitID"].ToString() != "0")
                    {
                        dr["UnitID"] = dt.Rows[i]["UnitID"]; dr["UnitDesc"] = dt.Rows[i]["UnitDesc"];
                    }
                    else
                    {
                        dr["UnitID"] = DBNull.Value; dr["UnitDesc"] = "";
                    }
                    if (dt.Rows[i]["BillQuantity"] != null && dt.Rows[i]["BillQuantity"].ToString().Trim() != "")
                        dr["BillQuantity"] = dt.Rows[i]["BillQuantity"];
                    else
                        dr["BillQuantity"] = DBNull.Value;

                    if (dt.Rows[i]["UnitPrice"] != null && dt.Rows[i]["UnitPrice"].ToString().Trim() != "")
                        dr["UnitPrice"] = dt.Rows[i]["UnitPrice"];
                    else
                        dr["UnitPrice"] = DBNull.Value;
                    //Gross Profit
                    if (dt.Rows[i]["UnitGrossProfit"] != null && dt.Rows[i]["UnitGrossProfit"].ToString().Trim() != "")
                        dr["UnitGrossProfit"] = dt.Rows[i]["UnitGrossProfit"];
                    else
                        dr["UnitGrossProfit"] = 1;

                    if (dt.Rows[i]["RushPrice"] != null && dt.Rows[i]["RushPrice"].ToString().Trim() != "")
                        dr["RushPrice"] = dt.Rows[i]["RushPrice"];
                    else
                        dr["RushPrice"] = DBNull.Value;

                    if (dt.Rows[i]["BillQuantity"] != null && dt.Rows[i]["BillQuantity"].ToString().Trim() != "" && dt.Rows[i]["UnitPrice"] != null && dt.Rows[i]["UnitPrice"].ToString().Trim() != "")
                        dr["Amount"] = Convert.ToDecimal(dt.Rows[i]["BillQuantity"]) * Convert.ToDecimal(dt.Rows[i]["UnitPrice"]);
                    else
                        dr["Amount"] = DBNull.Value;

                    if (dt.Rows[i]["BillQuantity"] != null && dt.Rows[i]["BillQuantity"].ToString().Trim() != "" && dt.Rows[i]["RushPrice"] != null && dt.Rows[i]["RushPrice"].ToString().Trim() != "")
                        dr["RushAmount"] = Convert.ToDecimal(dt.Rows[i]["BillQuantity"]) * Convert.ToDecimal(dt.Rows[i]["RushPrice"]);
                    else
                        dr["RushAmount"] = DBNull.Value;
                    if (dt.Rows[i]["OptionalTest"] != null)
                        dr["OptionalTest"] = dt.Rows[i]["OptionalTest"];
                    else
                        dr["OptionalTest"] = DBNull.Value;
                    if (dt.Rows[i]["TestComments"] != null && dt.Rows[i]["TestComments"].ToString().Trim() != "")
                        dr["TestComments"] = dt.Rows[i]["TestComments"];
                    else
                        dr["TestComments"] = DBNull.Value;
                    dtRevTests.Rows.Add(dr);
                    nControlNo++;
                }
            }
            dt.Dispose();

            bsRevTests.DataSource = dtRevTests;
            EstimatedTotal();
            if (nPSw != 3)
            MessageBox.Show("Test items added into this revision.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CopyRevision()
        {
            if (nMode == 0)
                return;
            string strCmpy = txtCmpyCode.Text.Trim();
            string strQuote = cboQuotes.Text.Substring(0, 9);
            string strRev = cboQuotes.Text.Substring(strQuote.Length + 1, cboQuotes.Text.Length - (strQuote.Length + 1));
            strRev = strRev.Trim().Substring(1, strRev.Trim().Length - 1);

            bsRevTests.EndEdit();
            int nControlNo = 1, nTestNo = 1;

            if (dtRevTests.Rows.Count > 0)
            {
                for (int i = 0; i < dtRevTests.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dtRevTests.Rows[i]["ControlNo"].ToString()) >= nControlNo && dtRevTests.Rows[i].RowState.ToString() == "Unchanged")
                        nControlNo = Convert.ToInt16(dtRevTests.Rows[i]["ControlNo"].ToString());
                }
            }
            DataView dvw = dtRevTests.DefaultView;
            dvw.Sort = "TestNo, SubTestNo";
            DataTable dT = dvw.ToTable();
            if (dT.Rows.Count > 0)
            {
                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dT.Rows[i]["TestNo"].ToString()) >= nTestNo)
                        nTestNo = Convert.ToInt16(dT.Rows[i]["TestNo"].ToString());
                }
            }
            dT.Dispose();

            if (dtRevTests.Rows.Count > 1)
                nTestNo++;

            if (nRev == 1)
            {

                strQuote = txtQuoteNo.Text;
                strRev = (Convert.ToInt16(txtRevNo.Text) - 1).ToString();
                nRev = 0;
            }

            DataTable dt = new DataTable();
            dt = PSSClass.Quotations.LoadRevTests(strCmpy, strQuote, Convert.ToInt16(strRev));

            if (dt.Rows.Count > 0)
            {
                int nSv = Convert.ToInt16(dt.Rows[0]["TestNo"]);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtRevTests.NewRow();
                    dr["RevisionNo"] = txtRevNo.Text;
                    dr["ControlNo"] = nControlNo; 
                    if (dt.Rows[i]["TestNo"] != null && dt.Rows[i]["TestNo"].ToString().Trim() != "")
                        dr["TestNo"] = dt.Rows[i]["TestNo"];
                    else
                        dr["TestNo"] = 0;
                    if (dt.Rows[i]["SubTestNo"] != null && dt.Rows[i]["SubTestNo"].ToString().Trim() != "")
                        dr["SubTestNo"] = dt.Rows[i]["SubTestNo"];
                    else
                        dr["SubTestNo"] = 0;
                    if (dt.Rows[i]["ServiceCode"] != null && dt.Rows[i]["ServiceCode"].ToString().Trim() != "")
                        dr["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                    else
                        dr["ServiceCode"] = DBNull.Value;
                    if (dt.Rows[i]["TestDesc1"] != null && dt.Rows[i]["TestDesc1"].ToString().Trim() != "")
                        dr["TestDesc1"] = dt.Rows[i]["TestDesc1"];
                    else
                        dr["TestDesc1"] = DBNull.Value;
                    if (dt.Rows[i]["UnitID"] != null && dt.Rows[i]["UnitID"].ToString() != "0")
                    {
                        dr["UnitID"] = dt.Rows[i]["UnitID"]; dr["UnitDesc"] = dt.Rows[i]["UnitDesc"];
                    }
                    else
                    {
                        dr["UnitID"] = DBNull.Value; dr["UnitDesc"] = "";
                    }
                    if (dt.Rows[i]["BillQuantity"] != null && dt.Rows[i]["BillQuantity"].ToString().Trim() != "")
                        dr["BillQuantity"] = dt.Rows[i]["BillQuantity"];
                    else
                        dr["BillQuantity"] = DBNull.Value;

                    if (dt.Rows[i]["UnitPrice"] != null && dt.Rows[i]["UnitPrice"].ToString().Trim() != "")
                        dr["UnitPrice"] = dt.Rows[i]["UnitPrice"];
                    else
                        dr["UnitPrice"] = DBNull.Value;
                    //Gross Profit
                    if (dt.Rows[i]["UnitGrossProfit"] != null && dt.Rows[i]["UnitGrossProfit"].ToString().Trim() != "")
                        dr["UnitGrossProfit"] = dt.Rows[i]["UnitGrossProfit"];
                    else
                        dr["UnitGrossProfit"] = 1;

                    if (dt.Rows[i]["RushPrice"] != null && dt.Rows[i]["RushPrice"].ToString().Trim() != "")
                        dr["RushPrice"] = dt.Rows[i]["RushPrice"];
                    else
                        dr["RushPrice"] = DBNull.Value;

                    if (dt.Rows[i]["BillQuantity"] != null && dt.Rows[i]["BillQuantity"].ToString().Trim() != "" && dt.Rows[i]["UnitPrice"] != null && dt.Rows[i]["UnitPrice"].ToString().Trim() != "")
                        dr["Amount"] = Convert.ToDecimal(dt.Rows[i]["BillQuantity"]) * Convert.ToDecimal(dt.Rows[i]["UnitPrice"]);
                    else
                        dr["Amount"] = DBNull.Value;

                    if (dt.Rows[i]["BillQuantity"] != null && dt.Rows[i]["BillQuantity"].ToString().Trim() != "" && dt.Rows[i]["RushPrice"] != null && dt.Rows[i]["RushPrice"].ToString().Trim() != "")
                        dr["RushAmount"] = Convert.ToDecimal(dt.Rows[i]["BillQuantity"]) * Convert.ToDecimal(dt.Rows[i]["RushPrice"]);
                    else
                        dr["RushAmount"] = DBNull.Value;

                    if (dt.Rows[i]["OptionalTest"] != null)
                        dr["OptionalTest"] = dt.Rows[i]["OptionalTest"];
                    else
                        dr["OptionalTest"] = DBNull.Value;
                    if (dt.Rows[i]["TestComments"] != null && dt.Rows[i]["TestComments"].ToString().Trim() != "")
                        dr["TestComments"] = dt.Rows[i]["TestComments"];
                    else
                        dr["TestComments"] = DBNull.Value;
                    dtRevTests.Rows.Add(dr);
                    nControlNo++;
                    nTestNo++;
                }
            }
            dt.Dispose();
            bsRevTests.DataSource = dtRevTests;

            dt = new DataTable();
            dt = PSSClass.Quotations.QuoteComments(txtCmpyCode.Text.Trim(), strQuote, Convert.ToInt16(strRev));

            if (dt.Rows.Count > 0)
            {
                dtRevisions.Rows[bsRevisions.Position]["CommentsBeforeTable"] = dt.Rows[0]["CommentsBeforeTable"].ToString();
                dtRevisions.Rows[bsRevisions.Position]["CommentsBeforeTable"] = dt.Rows[0]["CommentsBeforeTable"].ToString();
                dtRevisions.Rows[bsRevisions.Position]["CommentsAfterTable"] = dt.Rows[0]["CommentsAfterTable"].ToString();
                dtRevisions.Rows[bsRevisions.Position]["CommentsNonPrinting"] = dt.Rows[0]["CommentsNonPrinting"].ToString();
                dtRevisions.Rows[bsRevisions.Position]["WithPrepayment"] = dt.Rows[0]["WithPrepayment"].ToString();
            }
            dt.Dispose();
            try
            {
                txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
            }
            catch { }
            try
            {
                cboUnits.Text = PSSClass.Units.UnitDesc(Convert.ToInt16(txtUnitID.Text), dtUnits);
            }
            catch { }
            EstimatedTotal();
            MessageBox.Show("Test items added into this revision.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelTest_Click(object sender, EventArgs e)
        {
            strSC = txtSC.Text; strTDesc1 = spbTestDesc.Text; strTDesc2 = txtTestDesc2.Text; strTDesc3 = txtTestDesc3.Text; strTDesc4 = txtTestDesc4.Text; strPrice = txtAmount.Text;
            string strTestNo = txtTestNo.Text; string strSubNo = txtSubNo.Text; string strCtrlNo = txtControlNo.Text;

            for (int i = 0; i < dtRevTests.Rows.Count; i++)
            {
                if (dtRevTests.Rows[i].RowState.ToString() != "Deleted" && strTestNo == dtRevTests.Rows[i]["TestNo"].ToString() && strSubNo == dtRevTests.Rows[i]["SubTestNo"].ToString() &&
                    strCtrlNo == dtRevTests.Rows[i]["ControlNo"].ToString())
                {
                    dtRevTests.Rows[i].Delete();
                }
            }
            EstimatedTotal();
            btnPaste.Enabled = true;
        }

        private void btnAddRev_Click(object sender, EventArgs e)
        {
            UpdateQuotationRev(); 

            if (cboTestCat1.Items.Count == 0)
            {
                LoadColHeaders();
            }
            string strRevNo = "0";
            if (nMode != 1)
                strRevNo = PSSClass.Quotations.RevNo(txtCmpyCode.Text.Trim(), txtQuoteNo.Text).ToString();

            if (strRevNo != "0")
                nRev = 1;

            string strCurrDate = DateTime.Now.ToString("MM/dd/yyyy");
            DataRow dr;
            dr = dtRevisions.NewRow();
            dr["RevisionNo"] = strRevNo;
            dr["RevisionStatus"] = 0;
            dr["DateCreated"] = strCurrDate;
            dr["CreatedBy"] = LogIn.strUserID;
            dr["DateAccepted"] = DBNull.Value;
            dr["DateRejected"] = DBNull.Value;
            dr["RejectedCode"] = DBNull.Value;
            dr["RejectedDesc"] = "";
            dr["WithPrepayment"] = false;
            dr["PrepayInvoiced"] = false;
            dr["YearsValid"] = 1;
            dr["TestCategory1"] = cboTestCat1.Items[0];
            dr["TestCategory2"] = cboTestCat2.Items[0];
            dr["TestCategory3"] = "";
            dr["TestCategory4"] = "";
            dr["CommentsBeforeTable"] = "";
            dr["CommentsAfterTable"] = "";
            dr["CommentsNonPrinting"] = "";
            dr["EMailedBy"] = "";
            dr["DateEMailed"] = DBNull.Value;
            dr["DateCreated"] = DateTime.Now;
            dr["CreatedByID"] = LogIn.nUserID;
            dr["LastUpdate"] = DateTime.Now;
            dr["LastUserID"] = LogIn.nUserID;
            dtRevisions.Rows.Add(dr);

            lblRNo.Text = "R" + strRevNo; 
            lblRevTests.Text = "R" + strRevNo;

            bsRevisions.DataSource = dtRevisions;
            bnRevisions.BindingSource = bsRevisions;
            bsRevisions.Position = dtRevisions.Rows.Count - 1;

            txtRevCreator.Text = LogIn.strUserID;
            mskDateCreated.Text = strCurrDate;
            mskDateAccepted.ReadOnly = true; mskDateRejected.ReadOnly = true; cboRevStatus.Enabled = false;
            cboTestCat1.SelectedIndex = 0; cboTestCat2.SelectedIndex = 0; cboTestCat3.SelectedIndex = -1; cboTestCat4.SelectedIndex = -1;
            cboRevStatus.SelectedIndex = 0; cboReasons.SelectedIndex = 0;
            btnClose.Visible = false; btnAddRev.Enabled = false;
            if (nRev == 1)
                CopyRevision();
            bnRevisions.Enabled = false;
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                    try
                    {
                        txtContact.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
                        dgvContacts.Visible = false; txtComments.Focus();
                    }
                    catch { }
                else if (e.KeyChar == 27)
                    dgvContacts.Visible = false;
                else
                    txtContactID.Text = "";
            }
        }

        private void txtSpID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    if (txtSponsorID.Text.Trim() != "")
                    {
                        txtSp.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSpID.Text));
                        LoadContactsDDL(Convert.ToInt16(txtSpID.Text));
                        dgvSponsors.Visible = false; txtCon.Focus();
                    }
                }
                else if (e.KeyChar == 27)
                {
                    dgvSponsors.Visible = false;
                }
                else
                {
                    txtSp.Text = ""; txtConID.Text = ""; txtCon.Text = ""; dgvContacts.Visible = false;
                }
            }
        }

        private void pnlTestItems_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownP = true;
                mousePosP = new Point(e.X, e.Y);
            }
        }

        private void pnlTestItems_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDownP)
            {
                pnlTestItems.Location = PointToClient(this.pnlTestItems.PointToScreen(new Point(e.X - mousePosP.X, e.Y - mousePosP.Y)));
            }
        }

        private void pnlTestItems_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDownP)
            {
                mouseDownP = false;
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

        private void cboQuotes_Enter(object sender, EventArgs e)
        {
            cboQuotes.Text = strCopyQuote;
        }

        private void lblHeading_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void lblHeading_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void lblHeading_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                nSp = 1;
                LoadSponsorsDDL();
                dgvSponsors.Visible = true; dgvSponsors.BringToFront();//dgvSponsors.Top = 94; 
                dgvSponsors.Top = txtSponsor.Top + txtSponsor.Height;
            }
        }

        private void picContacts_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    nSp = 1;
                    LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    dgvContacts.Visible = true; dgvContacts.BringToFront(); //dgvContacts.Top = 117; 
                    dgvContacts.Top = txtContact.Top + txtContact.Height;
                }
                catch { }
            }
        }

        private void picCon_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    nSp = 2;
                    LoadContactsDDL(Convert.ToInt16(txtSpID.Text));
                    dgvContacts.Visible = true; dgvContacts.BringToFront(); //dgvContacts.Top = 182; 
                    dgvContacts.Top = txtCon.Top + txtCon.Height;
                }
                catch { }
            }
        }

        private void picSp_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                nSp = 2;
                LoadSponsorsDDL();
                dgvSponsors.Visible = true; dgvSponsors.BringToFront();//dgvSponsors.Top = 158; 
                dgvSponsors.Top = txtSp.Top + txtSp.Height;
            }
        }

        private void txtDateEMailed_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSender_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtRevCreator_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboTestCat1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboTestCat2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboTestCat3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboTestCat4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (pnlCalendar.Location == new Point(124, 67))
                mskDateCreated.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            else if (pnlCalendar.Location == new Point(328, 100) && cboRevStatus.SelectedIndex == 1)
                mskDateAccepted.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            else if (cboRevStatus.SelectedIndex == 2)
                mskDateRejected.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");

            pnlCalendar.Visible = false;
        }

        private void mskDateAccepted_Click(object sender, EventArgs e)
        {
            if (nMode == 2 && cboRevStatus.SelectedIndex == 1)
            {
                pnlCalendar.Visible = true; pnlCalendar.Location = new Point(328, 100);
                mskDateRejected.Text = "__/__/____";
            }
        }

        private void pnlRevisions_MouseHover(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void mskDateAccepted_KeyDown(object sender, KeyEventArgs e)
        {
            if (nMode == 2 && cboRevStatus.SelectedIndex == 1)
            {
                pnlCalendar.Visible = true; pnlCalendar.Location = new Point(328, 100);
                mskDateRejected.Text = "__/__/____";
            }
            e.SuppressKeyPress = true;
        }

        private void txtYears_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtRevNo_TextChanged(object sender, EventArgs e)
        {
            if (txtRevNo.Text != "")
            {
                lblRNo.Text = "R" + txtRevNo.Text;
                if (nMode != 1)
                    LoadRevTests(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, Convert.ToInt16(txtRevNo.Text));
            }
        }

        private void txtSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
            else if (nMode == 0 || txtTestNo.Text == "" || (Char.IsDigit(e.KeyChar) == false && e.KeyChar != 8 && e.KeyChar != 46))
                e.Handled = true;
            else if (e.KeyChar == 27)
                dgvServices.Visible = false;
            else if (nMode != 0)
                txtSCDesc.Text = "";
        }

        private void txtSCDesc_Enter(object sender, EventArgs e)
        {
            if (nMode == 0 || txtTestNo.Text.Trim() == "")
                return;

            if (nMode != 0)
            {
                if (dtSC.Rows.Count == 0)
                    LoadSCDDL();
                dgvServices.Visible = true; dgvServices.BringToFront();
            }
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void dgvContacts_Leave(object sender, EventArgs e)
        {
            dgvContacts.Visible = false;
        }

        private void dgvServices_Leave(object sender, EventArgs e)
        {
            dgvServices.Visible = false;
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            if (nSp == 1)//dgvSponsors.Top == 94
            {
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                txtContactID.Text = ""; txtContact.Text = ""; dgvContacts.DataSource = null;
                LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                LoadPrevQuotes();
                txtContact.Focus();
            }
            else
            {
                txtSp.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSpID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                txtConID.Text = ""; txtCon.Text = ""; dgvContacts.DataSource = null;
                LoadContactsDDL(Convert.ToInt16(txtSpID.Text));
            }
            dgvSponsors.Visible = false;
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
                if (nSp == 1)//dgvSponsors.Top == 94
                {
                    txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                    txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                    txtContactID.Text = ""; txtContact.Text = "";
                    LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    txtContact.Focus();
                }
                else
                {
                    txtSp.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                    txtSpID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                    txtConID.Text = ""; txtCon.Text = "";
                    LoadContactsDDL(Convert.ToInt16(txtSpID.Text));
                }
                dgvSponsors.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvSponsors.Visible = false;
            }
        }

        private void dgvServices_DoubleClick(object sender, EventArgs e)
        {
            txtSCDesc.Text = dgvServices.CurrentRow.Cells[0].Value.ToString();
            txtSC.Text = dgvServices.CurrentRow.Cells[1].Value.ToString();
            if (spbTestDesc.Text.Trim() == "")
                spbTestDesc.Text = dgvServices.CurrentRow.Cells[0].Value.ToString();
            dgvServices.Visible = false; nS = 1;
        }

        private void dgvServices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvServices_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSCDesc.Text = dgvServices.CurrentRow.Cells[0].Value.ToString();
                txtSC.Text = dgvServices.CurrentRow.Cells[1].Value.ToString();
                if (spbTestDesc.Text.Trim() == "")
                    spbTestDesc.Text = dgvServices.CurrentRow.Cells[0].Value.ToString();
                dgvServices.Visible = false;
            }
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                nSp = 1;
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSponsors.TabIndex = 3; dgvContacts.Visible = false; //dgvSponsors.Top = 94; 
                dgvSponsors.Top = txtSponsor.Top + txtSponsor.Height;
            }
        }

        private void txtSp_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                nSp = 2;
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSponsors.TabIndex = 10; //dgvSponsors.Top = 158; 
                dgvSponsors.Top = txtSp.Top + txtSp.Height;
            }
        }

        private void txtSpID_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvSponsors.Visible = false; dgvContacts.Visible = false;
            }
        }

        private void txtContactID_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvContacts.Visible = false; dgvSponsors.Visible = false;
            }
        }

        private void txtContact_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    nSp = 1;
                    LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    dgvContacts.Visible = true; dgvContacts.BringToFront(); dgvContacts.TabIndex = 7; dgvSponsors.Visible = false;
                    dgvContacts.Top = txtContact.Top + txtContact.Height;//dgvContacts.Top = 117; 
                }
                catch { }
            }
        }

        private void txtConID_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvContacts.Visible = false; dgvSponsors.Visible = false;
            }
        }

        private void txtCon_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    nSp = 2;
                    LoadContactsDDL(Convert.ToInt16(txtSpID.Text));
                    dgvContacts.Visible = true; dgvContacts.BringToFront(); dgvContacts.TabIndex = 13; dgvSponsors.Visible = false;
                    dgvContacts.Top = txtCon.Top + txtCon.Height;//dgvContacts.Top = 182; 
                    txtCon.Focus();
                }
                catch { }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dtRevisions.Rows.Count.ToString());

            bsRevisions.EndEdit();
            
            MessageBox.Show(dtRevisions.Rows[0]["RevisionStatus"].ToString());
            MessageBox.Show(dtRevisions.Rows[0]["DateCreated"].ToString());
            MessageBox.Show(dtRevisions.Rows[0]["DateAccepted"].ToString());
            MessageBox.Show(dtRevisions.Rows[0]["DateRejected"].ToString());

            for (int i = 0; i < dtRevisions.Rows.Count; i++)
            {
                if (dtRevisions.Rows[i].RowState.ToString() == "Added" || dtRevisions.Rows[i].RowState.ToString() == "Modified" || dtRevisions.Rows[i].RowState.ToString() == "Unchanged")
                {
                    MessageBox.Show("RowState : " + dtRevisions.Rows[i].RowState.ToString() + " " + dtRevisions.Rows[i]["RevisionNo"].ToString());
                }
                else
                    MessageBox.Show("RowState : " + dtRevTests.Rows[i].RowState.ToString());
            }

        }

        private void txtTestDesc3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
            else if (cboTestCat3.Text == "")
                e.Handled = true;
        }

        private void txtTestDesc4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
            else if (cboTestCat4.Text == "")
                e.Handled = true;
        }

        private void txtTestNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboRevStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (cboRevStatus.SelectedIndex == 0)
                {
                    mskDateAccepted.Text = "__/__/____";
                    mskDateRejected.Text = "__/__/____";
                    cboReasons.SelectedIndex = 0; cboReasons.Enabled = false;
                }
                else if (cboRevStatus.SelectedIndex == 1)
                {
                    mskDateAccepted.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    mskDateRejected.Text = "__/__/____";
                    cboReasons.Enabled = false;
                }
                else if (cboRevStatus.SelectedIndex == 2)
                {
                    mskDateRejected.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    mskDateAccepted.Text = "__/__/____";
                    cboReasons.Enabled = true; txtOtherReason.ReadOnly = false;
                    cboReasons.SelectedIndex = 1;
                    cboReasons.SelectAll();
                }
            }
        }

        private void cboRevStatus_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnPrtPreview_Click(object sender, EventArgs e)
        {
            byte bP = 1;

            //DataTable dt = new DataTable();
            //dt = PSSClass.Sponsors.SponsorOnCH(Convert.ToInt16(txtSponsorID.Text));
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    MessageBox.Show("Sponsor is currently on Credit Hold." + Environment.NewLine + "Printing is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    bP = 0;
            //}
            //dt.Dispose();
            
            if (nMode == 0)
            {
                QuotationRpt rptQuotation = new QuotationRpt();
                rptQuotation.WindowState = FormWindowState.Maximized;
                rptQuotation.nQ = 0;
                rptQuotation.nP = bP;
                try
                {
                    int nRevNo = Convert.ToInt16(txtRevNo.Text);
                    rptQuotation.CmpyCode = txtCmpyCode.Text.Trim();
                    rptQuotation.QuoteNo = txtQuoteNo.Text;
                    rptQuotation.RevNo = nRevNo;
                    rptQuotation.nOld = 0;
                    rptQuotation.pubSpID = Convert.ToInt16(txtSponsorID.Text);
                    rptQuotation.Show();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Please complete process first before previewing.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void btnDelRev_Click(object sender, EventArgs e)
        {
            if (nMode == 2)
            {
                if (txtRevNo.Text == "0")
                {
                    DialogResult dConfirm = new DialogResult();
                    dConfirm = MessageBox.Show("Deleting this revision would delete" + Environment.NewLine + "this quotation record." + Environment.NewLine + Environment.NewLine +
                                             "Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dConfirm == DialogResult.Yes)
                    {
                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        if (sqlcnn == null)
                        {
                            MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        ;
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.Add(new SqlParameter("@QuoteNo", SqlDbType.NVarChar));
                        sqlcmd.Parameters["@QuoteNo"].Value = txtQuoteNo.Text;

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spDelQuote";

                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        AddEditMode(false);
                        LoadRecords();
                        pnlRecord.Visible = false; nMode = 0;
                    }
                    return;
                }
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Deleting this revision would also delete the " + Environment.NewLine + "corresponding test entries below." + Environment.NewLine + Environment.NewLine +
                                         "Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.Yes)
                {
                    dtRevisions.Rows[bsRevisions.Position].Delete();
                    for (int i = 0; i < dtRevTests.Rows.Count; i++)
                    {
                        dtRevTests.Rows[i].Delete();
                    }
                }
            }
            else
                dtRevisions.Rows[bsRevisions.Position].Delete();
            if (nMode != 1)
                LoadRevTests(txtCmpyCode.Text.Trim(), txtQuoteNo.Text, Convert.ToInt16(txtRevNo.Text));
        }

        private void LoadRevisionRow(object sender, EventArgs e)
        {
            txtRevNo.Text = dtRevisions.Rows[bsRevisions.Position]["RevisionNo"].ToString();

            lblRevTests.Text = "R" + txtRevNo.Text;

            lblRNo.Text = "R" + txtRevNo.Text;

            if (txtDateEMailed.Text != null && txtDateEMailed.Text != "")
            {
                lblEStatus.Text = "Sent";
            }
            else if (cboRevStatus.Text == "Pending")
            {
                lblEStatus.Text = "Not Sent"; 
            }
        }

        private void LoadRevTestRow(object sender, EventArgs e)
        {
            try
            {
                txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
            }
            catch { }

            try
            {
                cboUnits.Text = PSSClass.Units.UnitDesc(Convert.ToInt16(txtUnitID.Text), dtUnits);
            }
            catch { }
            lblRevTests.Text = "R" + txtRevNo.Text;
            EstimatedTotal();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dtRevTests.Rows.Count.ToString());
            bsRevTests.EndEdit();
            for (int i = 0; i < dtRevTests.Rows.Count; i++)
            {
                if (dtRevTests.Rows[i].RowState.ToString() == "Added" || dtRevTests.Rows[i].RowState.ToString() == "Modified" || dtRevTests.Rows[i].RowState.ToString() == "Unchanged")
                {
                    MessageBox.Show("RowState : " + dtRevTests.Rows[i].RowState.ToString() + " " + dtRevTests.Rows[i]["TestNo"].ToString() + "  " + dtRevTests.Rows[i]["SubTestNo"].ToString());
                }
                else
                    MessageBox.Show("RowState : " + dtRevTests.Rows[i].RowState.ToString());
            }
        }

        private void txtQuoteNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void mskDateCreated_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.Location = new Point(124, 67);
            }
        }

        private void mskDateCreated_KeyDown(object sender, KeyEventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.Location = new Point(124, 67);
            }
            e.SuppressKeyPress = true; 
        }

        private void txtSC_Leave(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (txtSC.Text.Trim() != "")
                {
                    txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
                    if (txtSCDesc.Text == "")
                    {
                        MessageBox.Show("Invalid service code selected." + Environment.NewLine + "Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        txtSC.Focus();
                        return;
                    }
                    if (spbTestDesc.Text.Trim() == "")
                        spbTestDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
                    spbTestDesc.Focus();
                }
                else
                    txtSCDesc.Text = "";
            }
        }

        private void txtSC_TextChanged(object sender, EventArgs e)
        {
            if (txtSC.Text != "")
            {
                try
                {
                    txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
                }
                catch
                {
                    txtSCDesc.Text = "";
                }
            }
        }

        private void btnAddTestA_Click(object sender, EventArgs e)
        {
            dgvServices.Visible = false;
            if (dtSC.Rows.Count == 0)
                LoadSCDDL();

            int nTestNo = 1; int nControlNo = 1; int nSubTestNo = 0;

            DataView dvw = dtRevTests.DefaultView;
            dvw.Sort = "TestNo, SubTestNo";
            DataTable dT = dvw.ToTable();
            if (dT.Rows.Count > 0)
            {
                nTestNo = Convert.ToInt16(txtTestNo.Text) + 1;
                int nSv = Convert.ToInt16(txtTestNo.Text);
                int nX = nTestNo;

                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dT.Rows[i]["TestNo"].ToString()) > nSv)
                    {
                        if (nX >= Convert.ToInt16(dT.Rows[i]["TestNo"].ToString()))
                        {
                            dT.Rows[i]["TestNo"] = Convert.ToInt16(dT.Rows[i]["TestNo"].ToString()) + 1;
                            nX++;
                        }
                    }
                    if (Convert.ToInt16(dT.Rows[i]["ControlNo"].ToString()) >= nControlNo)
                        nControlNo = Convert.ToInt16(dT.Rows[i]["ControlNo"].ToString());
                }
                nControlNo++;
            }

            for (int i = 0; i < dtRevTests.Rows.Count; i++)
            {
                if (dtRevTests.Rows[i].RowState.ToString() != "Deleted")
                {
                    DataRow[] foundrows = dT.Select("ControlNo=" + dtRevTests.Rows[i]["ControlNo"].ToString());
                    if (foundrows.Length > 0)
                        dtRevTests.Rows[i]["TestNo"] = foundrows[0]["TestNo"].ToString();
                }
            }

            DataRow dr;
            dr = dtRevTests.NewRow();
            dr["RevisionNo"] = Convert.ToInt16(txtRevNo.Text);
            dr["ControlNo"] = nControlNo;
            dr["TestNo"] = nTestNo;
            dr["SubTestNo"] = nSubTestNo;
            dr["ServiceCode"] = DBNull.Value;
            dr["ServiceDesc"] = "";
            dr["TestDesc1"] = "";
            dr["BillQuantity"] = DBNull.Value;
            dr["UnitID"] = DBNull.Value;
            dr["UnitDesc"] = "";
            dr["UnitPrice"] = DBNull.Value;
            dr["Amount"] = DBNull.Value;
            dr["Rush"] = false;
            dr["RushPrice"] = DBNull.Value;
            dr["RushAmount"] = DBNull.Value;
            dr["OptionalTest"] = false;
            dr["TestComments"] = "";
            //Gross Profit
            dr["UnitGrossProfit"] =1;
            dtRevTests.Rows.Add(dr);
            bsRevTests.Position = Convert.ToInt16(tstbRevTests.Text);
            BindRevTest();
            txtSC.Enabled = true; txtSCDesc.Enabled = true;
            btnClose.Visible = false; txtSC.Focus();
        }

        private void btnAddTestB_Click(object sender, EventArgs e)
        {
            dgvServices.Visible = false;
            if (dtSC.Rows.Count == 0)
                LoadSCDDL();

            DataView dvw = dtRevTests.DefaultView;
            dvw.Sort = "TestNo, SubTestNo";
            DataTable dT = dvw.ToTable();
            if (dT.Rows.Count > 0)
            {
                int nSv = Convert.ToInt16(dT.Rows[0]["TestNo"]);
                int nT = nSv + 1;
                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    if (dT.Rows[i].RowState.ToString() != "Deleted")
                    {
                        if (Convert.ToInt16(dtRevTests.Rows[i]["TestNo"].ToString()) != nSv)
                        {
                            nSv = Convert.ToInt16(dtRevTests.Rows[i]["TestNo"].ToString());
                            nT++;
                        }
                        dT.Rows[i]["TestNo"] = nT;
                    }
                }
            }
            for (int i = 0; i < dtRevTests.Rows.Count; i++)
            {
                if (dtRevTests.Rows[i].RowState.ToString() != "Deleted")
                {
                    DataRow[] foundrows = dT.Select("ControlNo=" + dtRevTests.Rows[i]["ControlNo"].ToString());
                    if (foundrows.Length > 0)
                        dtRevTests.Rows[i]["TestNo"] = foundrows[0]["TestNo"].ToString();
                }
            }
            bsRevTests.EndEdit();

            int nControlNo = 0;

            dvw = dtRevTests.DefaultView;
            dvw.Sort = "TestNo, SubTestNo";
            dT = dvw.ToTable();
            if (dT.Rows.Count > 0)
            {
                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dT.Rows[i]["ControlNo"].ToString()) >= nControlNo)
                        nControlNo = Convert.ToInt16(dT.Rows[i]["ControlNo"].ToString());
                }
                nControlNo++;
            }
            DataRow dr;
            dr = dtRevTests.NewRow();

            dr["RevisionNo"] = Convert.ToInt16(txtRevNo.Text);
            dr["ControlNo"] = nControlNo;
            dr["TestNo"] = 1;
            dr["SubTestNo"] = 0;
            dr["ServiceCode"] = DBNull.Value;
            dr["ServiceDesc"] = "";
            dr["TestDesc1"] = "";
            dr["BillQuantity"] = DBNull.Value;
            dr["UnitID"] = DBNull.Value;
            dr["UnitDesc"] = "";
            dr["UnitPrice"] = DBNull.Value;
            dr["Amount"] = DBNull.Value;
            dr["Rush"] = false;
            dr["RushPrice"] = DBNull.Value;
            dr["RushAmount"] = DBNull.Value;
            dr["OptionalTest"] = false;
            dr["TestComments"] = "";
            //Gross Profit
            dr["UnitGrossProfit"] = 1;

            dtRevTests.Rows.Add(dr);
            bsRevTests.Position = Convert.ToInt16(tstbRevTests.Text);
            BindRevTest();
            txtSC.Enabled = true; txtSCDesc.Enabled = true;
            btnClose.Visible = false; txtSC.Focus();
        }


        private void BindRevTest()
        {
            try
            {
                foreach (Control c in pnlTests.Controls)
                {
                    c.DataBindings.Clear();
                }
                txtControlNo.DataBindings.Add("Text", bsRevTests, "ControlNo");
                txtTestNo.DataBindings.Add("Text", bsRevTests, "TestNo");
                txtSubNo.DataBindings.Add("Text", bsRevTests, "SubTestNo");
                txtSC.DataBindings.Add("Text", bsRevTests, "ServiceCode");
                txtSCDesc.DataBindings.Add("Text", bsRevTests, "ServiceDesc");
                spbTestDesc.DataBindings.Add("Text", bsRevTests, "TestDesc1");
                txtBillQty.DataBindings.Add("Text", bsRevTests, "BillQuantity");
                cboUnits.DataBindings.Add("SelectedText", bsRevTests, "UnitDesc");
                txtUnitID.DataBindings.Add("Text", bsRevTests, "UnitID");
                txtUnit.DataBindings.Add("Text", bsRevTests, "UnitDesc");
                txtUnitPrice.DataBindings.Add("Text", bsRevTests, "UnitPrice");
                txtAmount.DataBindings.Add("Text", bsRevTests, "Amount");
                txtRushPrice.DataBindings.Add("Text", bsRevTests, "RushPrice");
                txtRushAmount.DataBindings.Add("Text", bsRevTests, "RushAmount");
                //txtGrossProfit
                txtGrossProfit.DataBindings.Add("Text", bsRevTests, "UnitGrossProfit");
                chkOptional.DataBindings.Add("Checked", bsRevTests, "OptionalTest", true, DataSourceUpdateMode.OnPropertyChanged, false);
                spbTestComments.DataBindings.Add("Text", bsRevTests, "TestComments");
            }
            catch (Exception)
            {

                throw;
            }
           
        }

        private void btnEstTotal_Click(object sender, EventArgs e)
        {
            bsRevTests.EndEdit();
            EstimatedTotal();
        }

        private void btnAddSubA_Click(object sender, EventArgs e)
        {
            dgvServices.Visible = false;
            if (dtSC.Rows.Count == 0)
                LoadSCDDL();

            int nTestNo = 1; int nControlNo = 0; int nSubTestNo = 0;

            DataView dvw = dtRevTests.DefaultView;
            dvw.Sort = "TestNo, SubTestNo";
            DataTable dT = dvw.ToTable();

            if (dtRevTests.Rows.Count > 0)
            {
                nTestNo = Convert.ToInt16(txtTestNo.Text);
                nSubTestNo = Convert.ToInt16(txtSubNo.Text) + 1;

                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dT.Rows[i]["TestNo"].ToString()) == nTestNo)
                    {
                        if (Convert.ToInt16(dT.Rows[i]["SubTestNo"].ToString()) >= nSubTestNo)
                            dT.Rows[i]["SubTestNo"] = Convert.ToInt16(dT.Rows[i]["SubTestNo"].ToString()) + 1;
                    }

                    if (Convert.ToInt16(dT.Rows[i]["ControlNo"].ToString()) >= nControlNo)
                        nControlNo = Convert.ToInt16(dT.Rows[i]["ControlNo"].ToString());
                }
                nControlNo++;
            }

            for (int i = 0; i < dtRevTests.Rows.Count; i++)
            {
                if (dtRevTests.Rows[i].RowState.ToString() != "Deleted")
                {
                    DataRow[] foundrows = dT.Select("ControlNo=" + dtRevTests.Rows[i]["ControlNo"].ToString());
                    if (foundrows.Length > 0)
                    {
                        dtRevTests.Rows[i]["SubTestNo"] = foundrows[0]["SubTestNo"].ToString();
                    }
                }
            }

            DataRow dr;
            dr = dtRevTests.NewRow();
            dr["RevisionNo"] = Convert.ToInt16(txtRevNo.Text);
            dr["ControlNo"] = nControlNo;
            dr["TestNo"] = nTestNo;
            dr["SubTestNo"] = nSubTestNo;
            if (txtSC.Text != "")
            {
                dr["ServiceCode"] = Convert.ToInt16(txtSC.Text);
                dr["ServiceDesc"] = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
            }
            else
            {
                dr["ServiceCode"] = DBNull.Value;
                dr["ServiceDesc"] = "";
            }
            dr["TestDesc1"] = "";
            dr["BillQuantity"] = DBNull.Value;
            dr["UnitID"] = DBNull.Value;
            dr["UnitDesc"] = "";
            dr["UnitPrice"] = DBNull.Value;
            dr["Amount"] = DBNull.Value;
            dr["Rush"] = false;
            dr["RushPrice"] = DBNull.Value;
            dr["RushAmount"] = DBNull.Value;
            dr["OptionalTest"] = false;
            dr["TestComments"] = "";
            //Gross Profit
            dr["UnitGrossProfit"] = 1;
            dtRevTests.Rows.Add(dr);
            bsRevTests.Position = Convert.ToInt16(tstbRevTests.Text);
            btnClose.Visible = false; spbTestDesc.Select(); 
        }

        private void btnAddSubB_Click(object sender, EventArgs e)
        {
            if (txtTestNo.Text.Trim() == "")
                return;

            dgvServices.Visible = false;
            if (dtSC.Rows.Count == 0)
                LoadSCDDL();

            int nTest = 1;
            if (txtSC.Text != "")
                nTest = Convert.ToInt16(txtSC.Text);

            int nTestNo = Convert.ToInt16(txtTestNo.Text); int nControlNo = 0; int nSubTestNo = 0;

            DataView dvw = dtRevTests.DefaultView;
            dvw.Sort = "TestNo, SubTestNo";
            DataTable dT = dvw.ToTable();
            if (dT.Rows.Count > 0)
            {
                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    if (Convert.ToInt16(dT.Rows[i]["ControlNo"].ToString()) >= nControlNo)
                        nControlNo = Convert.ToInt16(dT.Rows[i]["ControlNo"].ToString());
                }
                nControlNo++;
            }
            DataRow dr;
            dr = dtRevTests.NewRow();
            dr["RevisionNo"] = Convert.ToInt16(txtRevNo.Text);
            dr["ControlNo"] = nControlNo;
            dr["TestNo"] = nTestNo;
            dr["SubTestNo"] = nSubTestNo;
            if (txtSC.Text != "")
            {
                dr["ServiceCode"] = Convert.ToInt16(txtSC.Text);
                dr["ServiceDesc"] = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
            }
            else
            {
                dr["ServiceCode"] = DBNull.Value;
                dr["ServiceDesc"] = "";
            }
            dr["TestDesc1"] = "";
            dr["BillQuantity"] = DBNull.Value;
            dr["UnitID"] = DBNull.Value;
            dr["UnitDesc"] = "";
            dr["UnitPrice"] = DBNull.Value;
            dr["Amount"] = DBNull.Value;
            dr["Rush"] = false;
            dr["RushPrice"] = DBNull.Value;
            dr["RushAmount"] = DBNull.Value;
            dr["OptionalTest"] = false;
            dr["TestComments"] = "";
            //Gross Profit
            dr["UnitGrossProfit"] = 1;
            dtRevTests.Rows.Add(dr);
            if (nTest == 1)
                bsRevTests.Position = 0;
            else
                bsRevTests.Position = Convert.ToInt16(tstbRevTests.Text);
            btnClose.Visible = false; spbTestDesc.Select(); 
        }

        private void txtSC_Enter(object sender, EventArgs e)
        {
            dgvServices.Visible = false; dgvUnits.Visible = false;
        }

        private void txtSCDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0 || txtTestNo.Text.Trim() == "")
                e.Handled = true;
            else
            {
                if (e.KeyChar == 27)
                    dgvServices.Visible = false;
                else
                    txtSC.Text = "";
            }
        }

        private void txtSubNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void tbcRevisions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbcRevisions.SelectedTab.Name == "tabComments")
            {
                lblRevNo.Text = "R" + txtRevNo.Text;
                if (nMode == 0)
                {
                    spbComBefTable.IsReadOnly = true; spbComAftTable.IsReadOnly = true; spbComNonPrint.IsReadOnly = true;
                }
                else
                {
                    spbComBefTable.IsReadOnly = false; spbComAftTable.IsReadOnly = false; spbComNonPrint.IsReadOnly = false;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bsFile.EndEdit();
            MessageBox.Show("RowState : " + dtQuote.Rows[bsFile.Position].RowState.ToString());

        }

        private void mskDateCreated_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
                pnlCalendar.Visible = true; pnlCalendar.Location = new Point(124, 67);
        }

        private void mskDateRejected_Click(object sender, EventArgs e)
        {
            if (nMode == 2 && cboRevStatus.SelectedIndex == 2)
            {
                pnlCalendar.Visible = true; pnlCalendar.Location = new Point(328, 124);
                mskDateAccepted.Text = "__/__/____";
            }
        }

        private void mskDateRejected_KeyDown(object sender, KeyEventArgs e)
        {
            if (nMode == 2 && cboRevStatus.SelectedIndex == 2)
            {
                pnlCalendar.Visible = true; pnlCalendar.Location = new Point(328, 124);
                mskDateAccepted.Text = "__/__/____";
            }
            e.SuppressKeyPress = true;
        }

        private void txtSp_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSponsors;
                if (chkWildSpCon.Checked == true)
                    dvwSponsors = new DataView(dtSponsors, "SponsorName like '%" + txtSp.Text.Trim() + "%'", "SponsorName", DataViewRowState.CurrentRows);
                else
                    dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSp.Text.Trim() + "%'", "SponsorName", DataViewRowState.CurrentRows);
                PSSClass.General.DGVSetUp(dgvSponsors, dvwSponsors, 369);
            }
        }

        private void btnEMailQ_Click(object sender, EventArgs e)
        {
            //Disabled 5/10/2016 as per JM

            //DataTable dt = new DataTable();
            //dt = PSSClass.Sponsors.SponsorOnCH(Convert.ToInt16(txtSponsorID.Text));
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    MessageBox.Show("Sponsor is currently on Credit Hold." + Environment.NewLine + "Process is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    dt.Dispose();
            //    return;
            //}
            //dt.Dispose();

            if (nMode != 0)
            {
                MessageBox.Show("This quote is in add or edit mode. " + Environment.NewLine + "Please save or cancel changes made." + Environment.NewLine + "Process is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int nRevNo = Convert.ToInt16(txtRevNo.Text);
            QuotationRpt QuoteRpt = new QuotationRpt();
            QuoteRpt.WindowState = FormWindowState.Minimized;
            QuoteRpt.CmpyCode = txtCmpyCode.Text.Trim();
            QuoteRpt.QuoteNo = txtQuoteNo.Text;
            QuoteRpt.RevNo = nRevNo;
            QuoteRpt.pubSpID = Convert.ToInt16(txtSponsorID.Text);
            QuoteRpt.nQ = 1;
            QuoteRpt.Show();

            QuoteRpt.Close(); QuoteRpt.Dispose();

            txtBody.Text = ""; lstAttachment.Items.Clear();

            string strPDFFile = "";
            string strCFName = "";
            if (txtCmpyCode.Text.Trim().Trim() == "P")
            {
                strPDFFile = @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + txtQuoteNo.Text.Substring(0, 4) + @"\" + "P" + txtQuoteNo.Text + ".R" + nRevNo.ToString().Trim() + ".pdf";
                strCFName = PSSClass.Quotations.ContactFirstName("P", txtQuoteNo.Text, nRevNo);
            }
            else
            {
                strPDFFile = @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + txtQuoteNo.Text.Substring(0, 4) + @"\" + txtQuoteNo.Text + ".R" + nRevNo.ToString().Trim() + ".pdf";
                strCFName = PSSClass.Quotations.ContactFirstName(txtCmpyCode.Text.Trim().Trim(), txtQuoteNo.Text, nRevNo);
            }

            lstAttachment.Items.Add(strPDFFile);
            lnkDoc.Text = strPDFFile;

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            rdoInitial.Checked = false; rdoFollowup.Checked = false;
            if (txtDateEMailed.Text == "")
            {
                rdoInitial.Checked = true;
            }
            else
            {
                rdoFollowup.Checked = true;
            }

            txtTo.Text = "";
            txtBody.Text = txtBody.Text.Replace("\r\n", "<br />");
            txtBody.Text = txtBody.Text.Replace("<br />", Environment.NewLine);

            if (txtConID.Text != "")
            {
                DataTable dTable = new DataTable();
                //Primary Contact 
                sqlcmd = new SqlCommand("SELECT EMailAddress FROM ContactEMAddresses WHERE ContactID = " + Convert.ToInt16(txtContactID.Text) + " AND AckReports = 1", sqlcnn);
                sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    dTable.Load(sqldr);
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        txtTo.Text = txtTo.Text + dTable.Rows[i]["EMailAddress"].ToString() + "; ";
                    }
                }
                sqlcmd.Dispose();
                dTable.Rows.Clear();
                //Seconday Contact
                sqlcmd = new SqlCommand("SELECT EMailAddress FROM ContactEMAddresses WHERE ContactID = " + Convert.ToInt16(txtConID.Text) + " AND AckReports = 1", sqlcnn);
                sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    dTable.Load(sqldr);
                    for (int i = 0; i < dTable.Rows.Count; i++)
                    {
                        txtTo.Text = txtTo.Text + dTable.Rows[i]["EMailAddress"].ToString() + "; ";
                    }
                }
                sqlcmd.Dispose(); dTable.Dispose();
            }
            else
            {
                sqlcmd = new SqlCommand("SELECT EMailAddress FROM ContactEMAddresses WHERE ContactID = " + Convert.ToInt16(txtContactID.Text) + " AND AckReports = 1", sqlcnn);
                sqldr = sqlcmd.ExecuteReader();
                sqldr.Read();
                txtTo.Text = sqldr.GetValue(0).ToString();
            }

            if (txtTo.Text.Trim() == "")
            {
                MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please check Sponsor/Contact information.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sqldr.Close(); sqlcmd.Dispose();

            sqlcmd = new SqlCommand("SELECT QuoteCCEMailAd FROM Employment WHERE LoginName = '" + LogIn.strUserID + "'", sqlcnn); // txtRevCreator.text Revised 8-11-2016
            sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                txtBCC.Text = sqldr.GetValue(0).ToString();
            }
            else
            {
                MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sqldr.Close(); sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();

            txtSubject.Text = "Quote No. " + txtCmpyCode.Text.Trim().Trim() + txtQuoteNo.Text + ".R" + nRevNo.ToString();
                     
            pnlRecord.Enabled = false; pnlEMail.Visible = true; pnlEMail.Location = new Point(50, 150); pnlEMail.BringToFront();
        }

        private void cboRevStatus_DropDown(object sender, EventArgs e)
        {
            txtRevStatus.Text = cboRevStatus.Text;
        }

        private void cboRevStatus_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (cboRevStatus.SelectedIndex != 2)
                {
                    cboReasons.SelectedIndex = 0; //txtOtherReason.Text = ""; txtOtherReason.ReadOnly = true;
                }
                else
                {
                    cboReasons.SelectedIndex = 1; //txtOtherReason.ReadOnly = false;
                }
            }
            else
            {
                cboRevStatus.Text = txtRevStatus.Text;
            }
        }

        private void picSCodes_Click(object sender, EventArgs e)
        {
            if (nMode != 0 && txtTestNo.Text != "")
            {
                LoadSCDDL();
                dgvServices.Visible = true; dgvServices.BringToFront();
            }
        }

        private void txtContactID_Leave(object sender, EventArgs e)
        {
            try
            {
                txtContact.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
                dgvContacts.Visible = false; txtComments.Focus();
            }
            catch { }
        }

        private void txtConID_Leave(object sender, EventArgs e)
        {
            try
            {
                txtCon.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtConID.Text), Convert.ToInt16(txtSpID.Text));
                dgvContacts.Visible = false;
            }
            catch { }
        }

        private void txtSpID_Leave(object sender, EventArgs e)
        {
            if (txtSpID.Text.Trim() != "")
            {
                txtSp.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSpID.Text));
                LoadContactsDDL(Convert.ToInt16(txtSpID.Text));
                dgvSponsors.Visible = false; txtCon.Focus();
            }
        }

        private void txtSp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 27)
                    dgvSponsors.Visible = false;
                else
                    txtSpID.Text = "";
            }
        }

        private void txtCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                    try
                    {
                        txtCon.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtCon.Text), Convert.ToInt16(txtSpID.Text));
                        dgvContacts.Visible = false; txtComments.Focus();
                    }
                    catch { }
                if (e.KeyChar == 27)
                    dgvContacts.Visible = false;
                else
                    txtConID.Text = "";
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

        private void txtCon_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    DataView dvwContacts;
                    if (chkWildSpCon.Checked == true)
                        dvwContacts = new DataView(dtContacts, "Contact like '%" + txtCon.Text.Trim() + "%'", "Contac", DataViewRowState.CurrentRows);
                    else
                        dvwContacts = new DataView(dtContacts, "Contact like '" + txtCon.Text.Trim() + "%'", "Contact", DataViewRowState.CurrentRows);
                    PSSClass.General.DGVSetUp(dgvContacts, dvwContacts, 369);

                }
                catch { }
            }
        }

        private void btnSortItems_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvw = dtRevTests.DefaultView;
                dvw.Sort = "TestNo, SubTestNo";
                DataTable dT = dvw.ToTable();
                if (dT.Rows.Count > 0)
                {
                    int nSv = 999;
                    int nT = 0;
                    for (int i = 0; i < dT.Rows.Count; i++)
                    {
                        if (dT.Rows[i].RowState.ToString() != "Deleted")
                        {
                            if (Convert.ToInt16(dT.Rows[i]["TestNo"].ToString()) != nSv) 
                            {
                                nSv = Convert.ToInt16(dT.Rows[i]["TestNo"].ToString());
                                nT++;
                            }
                            dT.Rows[i]["TestNo"] = nT;
                        }
                    }
                }
                for (int i = 0; i < dtRevTests.Rows.Count; i++)
                {
                    if (dtRevTests.Rows[i].RowState.ToString() != "Deleted")
                    {
                        DataRow[] foundrows = dT.Select("ControlNo=" + dtRevTests.Rows[i]["ControlNo"].ToString());
                        if (foundrows.Length > 0)
                            dtRevTests.Rows[i]["TestNo"] = foundrows[0]["TestNo"].ToString();
                    }
                }
                bsRevTests.EndEdit();
            }
        }

        private void txtConID_TextChanged(object sender, EventArgs e)
        {
            if (txtConID.Text.Trim() == "")
            {
                txtCon.Text = "";
            }
        }

        private void Quotes_FormClosing(object sender, FormClosingEventArgs e)
        {
            bsFile.Dispose(); bsRevisions.Dispose(); bsRevTests.Dispose();
            dtSponsors.Dispose(); dtContacts.Dispose(); dtQuote.Dispose(); 
            dtRevisions.Dispose(); dtRevTests.Dispose();dtSC.Dispose(); dtUnits.Dispose();
            GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers();
        }

        private void dtrTestItems_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            if (e.ItemIndex < dtRevTests.Rows.Count)
            {
                switch (e.Control.Name)
                {
                    case "txtTestItem1":
                        try
                        {
                            e.Value = dtRevTests.Rows[e.ItemIndex]["TestDesc1"].ToString();
                        }
                        catch { }
                        break;
                    case "txtTestComm":
                        try
                        {
                            e.Value = dtRevTests.Rows[e.ItemIndex]["TestComments"].ToString();
                        }
                        catch { }
                        break;
                }
            }
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                btnPaste.Enabled = false;
                txtSC.Text = strSC; spbTestDesc.Text = strTDesc1; txtTestDesc2.Text = strTDesc2; txtTestDesc3.Text = strTDesc3; txtTestDesc4.Text = strTDesc4; txtAmount.Text = strPrice;
            }
        }

        private void Quotes_KeyDown(object sender, KeyEventArgs e)
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                this.ofdFile = new System.Windows.Forms.OpenFileDialog();

                // Set the file dialog to filter for graphics files. 
                this.ofdFile.Filter =
                    "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|" +
                    "PDF (*.PDF)|*.PDF|" +
                    "All files (*.*)|*.*";

                // Allow the user to select multiple images. 
                this.ofdFile.Multiselect = false;
                this.ofdFile.Title = "SELECT ACCEPTED QUOTATION PDF FILE";
                if (ofdFile.ShowDialog() == DialogResult.OK)
                {
                    txtPDF.Text = ofdFile.FileName;
                }
            }
        }

        private void btnViewPDF_Click(object sender, EventArgs e)
        {
            if (txtPDF.Text.Trim() != "")
                System.Diagnostics.Process.Start(@txtPDF.Text);
        }

        private void txtPDF_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
            {
                e.Handled = true;
            }
        }

        private void cboReasons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRevStatus.SelectedIndex == 2)
            {
                //if (cboReasons.SelectedIndex == 4)
                //{
                //    if (nMode != 0)
                //    {
                //        txtOtherReason.ReadOnly = false;
                //    }
                //}
                //else
                //{
                //    txtOtherReason.ReadOnly = true; txtOtherReason.Text = "";
                //}
                if (nMode != 0)
                {
                    txtOtherReason.ReadOnly = false;
                    if (cboReasons.SelectedIndex == 0)
                        cboReasons.SelectedIndex = 1;
                }
            }
            else
            {
                txtOtherReason.ReadOnly = true; txtOtherReason.Text = ""; 
            }
        }

        private void cboReasons_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtOtherReason_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0 || cboRevStatus.SelectedIndex != 2)//cboReasons.SelectedIndex != 4
                e.Handled = true;
        }

        private void btnEditSponsor_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(Sponsors));

            if (intOpen == 0)
            {
                Sponsors childForm = new Sponsors();
                childForm.MdiParent = this.MdiParent;
                childForm.Text = "Sponsors";
                childForm.nSpID = Convert.ToInt32(txtSponsorID.Text);
                childForm.nQuoteSw = 1;
                childForm.Show();
            }
            else
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr += 1;
            if (nCtr > 1)
            {
                DataTable dt = new DataTable();
                dt = PSSClass.Quotations.QuotationsMaster();
                PSSClass.DataEntry.DGVSearch(tstbSearchField.Text, tstbSearch.Text, dt, bsFile);
                nCtr = 0;
                tstbSearch.Text = "";
                timer1.Enabled = false;
                nSw = 0;
            }
        }

        private void btnSendEMail_Click(object sender, EventArgs e)
        {
            if (lstAttachment.Items.Count == 0)
            {
                MessageBox.Show("No attachment found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            string strTo = txtTo.Text.Replace(";", "; ");

            string strBody = ""; string strSignature = "";
            strBody = txtBody.Text.Replace("\r\n", "<br />");
            strSignature = ReadSignature();
            strBody = strBody + "<br /><br />" + strSignature;

            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.

            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

            // Set HTMLBody. 
            oMsg.HTMLBody = "<FONT face=\"Arial\">";
            oMsg.HTMLBody += strBody.Trim();

            //Add attachments.
            //oMsg.Attachments.Add(crafFile);
            for (int i = 0; i < lstAttachment.Items.Count; i++)
            {
                oMsg.Attachments.Add(lstAttachment.Items[i].ToString());
            }
            //Subject line
            oMsg.Subject = txtSubject.Text;
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
            
            string[] EMAddresses = strTo.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < EMAddresses.Length; i++)
            {
                if (EMAddresses[i].Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                    oRecip.Resolve();
                }
            }
            oMsg.BCC = txtBCC.Text;
            //oRecip.Resolve();
            oMsg.Display();
            
            // Send.
            //((Outlook._MailItem)oMsg).Send();
            
            // Clean up.
            //oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();

            sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sqlcmd = new SqlCommand("UPDATE QuotationRev SET DateEMailed=GetDate(),EMailedByID=" + LogIn.nUserID + " " +
                                    "WHERE CompanyCode = '" + txtCmpyCode.Text.Trim().Trim() + "' AND QuotationNo='" + txtQuoteNo.Text + "' AND RevisionNo=" + Convert.ToInt16(txtRevNo.Text), sqlcnn);
            sqlcmd.ExecuteNonQuery();
            
            sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
            pnlEMail.Visible = false; pnlRecord.Enabled = true;
            LoadRevisions(txtCmpyCode.Text.Trim().Trim(), txtQuoteNo.Text);
        }

        private void btnCancelSend_Click(object sender, EventArgs e)
        {
            pnlEMail.Visible = false; pnlRecord.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult result = ofdFile.ShowDialog();
            if (result == DialogResult.OK)
            {
                string strFile = ofdFile.FileName;
                lstAttachment.Items.Add(strFile);
                lnkDoc.Text = strFile;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstAttachment.SelectedIndex == -1)
                return;

            lstAttachment.Items.RemoveAt(lstAttachment.SelectedIndex);
            try
            {
                lnkDoc.Text = lstAttachment.Items[0].ToString();
            }
            catch
            {
                lnkDoc.Text = "";
            }
        }

        private void lnkDoc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkDoc.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void txtEstTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        
        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
            else if (Char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 46)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            txtAmount.Text = "";
        }

        private void EstimatedTotal()
        {
            try
            {
                if (nMode != 0)
                    bsRevTests.EndEdit();

                object nSum; object nRSum;
                nSum = dtRevTests.Compute("Sum(Amount)", "");
                nRSum = dtRevTests.Compute("Sum(RushAmount)", "" );
                decimal nT = 0, nRT = 0;
                if (nSum != null)
                    nT = Convert.ToDecimal(nSum);
                if (nRSum != DBNull.Value)
                    nRT = Convert.ToDecimal(nRSum);
                txtEstTotal.Text = nT.ToString("#,##0.00");
                txtRushTotal.Text = nRT.ToString("#,##0.00");
            }
            catch { }
        }

        private void txtBillQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
            else if (Char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 46)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            txtAmount.Text = "";
        }
        private void txtGrossProfit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == 13)//Enter Key
            {
                SendKeys.Send("{TAB}");
            }
            else if (Char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 46) //8 - The BACKSPACE key, 46- Delete Key
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            //txtAmount.Text = "";
        }

        private void txtControlNo_TextChanged(object sender, EventArgs e)
        {
            decimal nT = 0;
            try
            {
                txtAmount.Text = (Convert.ToDecimal(dtRevTests.Rows[bsRevTests.Position]["UnitPrice"]) * Convert.ToDecimal(dtRevTests.Rows[bsRevTests.Position]["BillQuantity"])).ToString("#,##0.00");
            }
            catch {}
            try
            {
                for (int i = 0; i < dtRevTests.Rows.Count; i++)
                {
                    if (dtRevTests.Rows[i].RowState.ToString() != "Deleted" && dtRevTests.Rows[i]["UnitPrice"] != null && dtRevTests.Rows[i]["BillQuantity"] != null)
                    {
                        nT += Convert.ToDecimal(dtRevTests.Rows[i]["UnitPrice"]) * Convert.ToDecimal(dtRevTests.Rows[i]["BillQuantity"]);
                    }
                }
            }
            catch { }
            txtEstTotal.Text = nT.ToString("#,##0.00");
        }

        private void txtBillQty_Leave(object sender, EventArgs e)
        {
            try
            {
                txtAmount.Text = (Convert.ToDecimal(txtBillQty.Text) * Convert.ToDecimal(txtUnitPrice.Text)).ToString();
                txtRushAmount.Text = (Convert.ToDecimal(txtBillQty.Text) * Convert.ToDecimal(txtRushPrice.Text)).ToString();
            }
            catch { }
            EstimatedTotal();
        }

        private void txtUnitPrice_Leave(object sender, EventArgs e)
        {
            try
            {
                txtAmount.Text = (Convert.ToDecimal(txtBillQty.Text) * Convert.ToDecimal(txtUnitPrice.Text)).ToString();
            }
            catch { }
            EstimatedTotal();
        }

        private void txtRushAmount_Enter(object sender, EventArgs e)
        {
            try
            {
                txtRushAmount.Text = (Convert.ToDecimal(txtBillQty.Text) * Convert.ToDecimal(txtRushPrice.Text)).ToString();
            }
            catch { }
            EstimatedTotal();
        }

        private void txtRushAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtRushPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
            else if (Char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 46)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
            txtRushAmount.Text = "";
        }

        private void txtRushPrice_Leave(object sender, EventArgs e)
        {
            try
            {
                txtRushAmount.Text = (Convert.ToDecimal(txtBillQty.Text) * Convert.ToDecimal(txtRushPrice.Text)).ToString();
            }
            catch { }
            EstimatedTotal();
        }

        private void lstAttachment_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkDoc.Text = lstAttachment.SelectedItem.ToString();
        }

        private void rdoInitial_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoInitial.Checked == true)
            {
                //string strSkit = "0";
                //DataTable dtSKit = new DataTable();
                //dtSKit = PSSClass.Quotations.QuoteSterikits(txtQuoteNo.Text, Convert.ToInt16(txtRevNo.Text));
                //if (dtSKit != null && dtSKit.Rows.Count > 0)
                //    strSkit = "1";
                //dtSKit.Dispose();

                //if (strSkit == "0")
                //    GetEmailBody(1);    // 1 = Initial Email
                //else
                //    GetEmailBody(3);    // 3 = Sterikit Email
                GetEmailBody(1);
            }
        }

        private void rdoFollowup_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFollowup.Checked == true)
            { 
                GetEmailBody(2);    // 2 = Follow-up Email
            }
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboUnits_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void cboUnits_DropDown(object sender, EventArgs e)
        {
            strComboSC = cboUnits.Text;
        }

        private void cboUnits_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (nMode == 0)
                cboUnits.Text = strComboSC;
        }

        private void txtRushTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                QuotationRpt rptQuotation = new QuotationRpt();
                rptQuotation.WindowState = FormWindowState.Maximized;
                rptQuotation.nQ = 0;
                try
                {
                    int nRevNo = Convert.ToInt16(txtRevNo.Text);

                    rptQuotation.QuoteNo = txtQuoteNo.Text;
                    rptQuotation.RevNo = nRevNo;
                    rptQuotation.nOld = 1;
                    rptQuotation.pubSpID = Convert.ToInt16(txtSponsorID.Text);
                    rptQuotation.Show();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Please complete process first before previewing.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void txtUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0 || txtTestNo.Text.Trim() == "")
                e.Handled = true;
            else
            {
                if (e.KeyChar == 27)
                    dgvUnits.Visible = false;
            }
        }

        private void txtUnit_Enter(object sender, EventArgs e)
        {
            if (nMode == 0 || txtTestNo.Text.Trim() == "")
                return;

            if (nMode != 0)
            {
                if (dtUnits.Rows.Count == 0)
                    LoadUnits();
                dgvUnits.Visible = true; dgvUnits.BringToFront(); dgvServices.Visible = false;
            }
        }

        private void dgvUnits_Leave(object sender, EventArgs e)
        {
            dgvUnits.Visible = false;
        }

        private void dgvUnits_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvUnits_DoubleClick(object sender, EventArgs e)
        {
            txtUnit.Text = dgvUnits.CurrentRow.Cells[0].Value.ToString();
            txtUnitID.Text = dgvUnits.CurrentRow.Cells[1].Value.ToString();
            dgvUnits.Visible = false;
        }

        private void dgvUnits_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtUnit.Text = dgvUnits.CurrentRow.Cells[0].Value.ToString();
                txtUnitID.Text = dgvUnits.CurrentRow.Cells[1].Value.ToString();
                dgvUnits.Visible = false;
            }
            else if (e.KeyChar == 27)
                dgvUnits.Visible = false;
        }

        private void picUnits_Click(object sender, EventArgs e)
        {
            if (nMode != 0 && txtTestNo.Text != "")
            {
                LoadUnits();
                dgvUnits.Visible = true;
            }
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvSponsors.Visible = false; dgvContacts.Visible = false; dgvUnits.Visible = false;
            }
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    if (txtSponsorID.Text.Trim() != "" && txtSponsorID.Text.All(char.IsDigit))
                    {
                        txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                        LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                        LoadPrevQuotes();
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

        private void chkOptional_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (chkOptional.Checked)
                    chkOptional.Checked = false;
                else
                    chkOptional.Checked = true;
            }
            else
            {
                if (chkOptional.Checked == true)
                {
                    if (PSSClass.ServiceCodes.SCPrepayItem(Convert.ToInt16(txtSC.Text)) == true)
                        chkOptional.Checked = false;
                }
            }
        }

        private void btnCancelInv_Click(object sender, EventArgs e)
        {
            pnlPO.Visible = false; pnlRecord.Enabled = true;
            LoadRecords();//Reload updated records
            PSSClass.General.FindRecord("QuotationNo", strPPQ, bsFile, dgvFile);
            LoadData();
            txtQuoteNo.Focus();
            bnRevisions.Enabled = true;
        }

        private void btnProceedInv_Click(object sender, EventArgs e)
        {
            if (txtPOInv.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a PO number.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();

            if (txtPOInv.Text.Trim() != "")
            {
                //Add Record to Sponsors PO Master File
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@nMode", 1);
                sqlcmd.Parameters.AddWithValue("@SponsorID", Convert.ToInt16(txtSponsorID.Text));
                sqlcmd.Parameters.AddWithValue("@PONo", txtPOInv.Text.ToUpper());
                if (txtPOAmt.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(txtPOAmt.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@Amount", 0);
                sqlcmd.Parameters.AddWithValue("@FilePath", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditPO";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch { }

                //Update PONo Field in Quotations
                sqlcmd.Dispose();
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@QuoteNo", txtQuoteNo.Text);
                if (txtPOInv.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@PONo", txtPOInv.Text.ToUpper());
                else
                    sqlcmd.Parameters.AddWithValue("@PONo", DBNull.Value);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdQuotePO";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch { }
            }

            //Create Invoice
            DataTable dt = new DataTable();

            string strNotes = "THIS INVOICE REPRESENTS PREPAYMENT AS PER QUOTE #" + txtQuoteNo.Text + ".R" + txtRevNo.Text;
            string strInvNo = PSSClass.General.NewInvNo("Invoices", "InvoiceNo").ToString();

            sqlcmd.Dispose();
            sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@nMode", 1);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", "P");
            sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(strInvNo));
            sqlcmd.Parameters.AddWithValue("@InvDate", DateTime.Now);
            sqlcmd.Parameters.AddWithValue("@InvType", 1);
            sqlcmd.Parameters.AddWithValue("@Header", "");
            sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@ConID", Convert.ToInt16(txtContactID.Text.Trim()));
            sqlcmd.Parameters.AddWithValue("@InvPrt", strNotes);
            sqlcmd.Parameters.AddWithValue("@InvNonPrt", "");
            sqlcmd.Parameters.AddWithValue("@DateRev", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@DateCanc", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            //chkExtraInvoice
            sqlcmd.Parameters.AddWithValue("@ExtraInvoice", false);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditInvMstr";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                {
                    MessageBox.Show("An invoice is created for this quote.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            sqlcmd.Dispose(); dt.Dispose();

            dt = new DataTable();
            if (bPPy == false)
                dt = PSSClass.Billing.PrepayInvAuto(txtCmpyCode.Text.Trim().Trim(), txtQuoteNo.Text, Convert.ToInt16(txtRevNo.Text));
            else
                dt = PSSClass.Billing.PrepayInvAutoPPI(txtCmpyCode.Text.Trim().Trim(), txtQuoteNo.Text, Convert.ToInt16(txtRevNo.Text));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", 1);
                sqlcmd.Parameters.AddWithValue("@CmpyCode", "P");
                sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(strInvNo));
                sqlcmd.Parameters.AddWithValue("@InvID", i);
                sqlcmd.Parameters.AddWithValue("@QuoteNo", txtQuoteNo.Text);
                sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
                sqlcmd.Parameters.AddWithValue("@CtrlNo", DBNull.Value);//added 1/27/2016
                sqlcmd.Parameters.AddWithValue("@PONo", txtPOInv.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@PSSNo", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@RptNo", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dt.Rows[i]["SC"]));
                sqlcmd.Parameters.AddWithValue("@SCType", 1);
                sqlcmd.Parameters.AddWithValue("@BillQty", 1);
                sqlcmd.Parameters.AddWithValue("@UPrice", Convert.ToDecimal(dt.Rows[i]["Amount"]));
                sqlcmd.Parameters.AddWithValue("@Amt", Convert.ToDecimal(dt.Rows[i]["Amount"]));
                sqlcmd.Parameters.AddWithValue("@Adj", 0);
                sqlcmd.Parameters.AddWithValue("@PrePay", 0);//added 1/27/2016
                sqlcmd.Parameters.AddWithValue("@RushTest", DBNull.Value);//added 1/27/2016
                sqlcmd.Parameters.AddWithValue("@QCmpyCode", txtCmpyCode.Text.Trim().Trim());
                sqlcmd.Parameters.AddWithValue("@LCmpyCode", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@RCmpyCode", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                //Gross Profit No Need to show in the Invoice Details Gross Profit
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditInvDtls";
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

            sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@QuoteNo", txtQuoteNo.Text);
            sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
            sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(strInvNo));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdPPayInv";
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
            pnlPO.Visible = false; pnlRecord.Enabled = true;
            LoadRecords();
            PSSClass.General.FindRecord("QuotationNo", txtQuoteNo.Text, bsFile, dgvFile);
            LoadData();
            bnRevisions.Enabled = true;

            PrePayment.strQuoteNo = txtQuoteNo.Text;
            PrePayment.nRevNo = Convert.ToInt16(txtRevNo.Text);

            PrePayment childForm = new PrePayment();
            childForm.MdiParent = this.MdiParent;
            childForm.Text = "PREPAYMENTS";
            childForm.nQSw = 1;
            childForm.nInvNo = Convert.ToInt32(strInvNo);
            childForm.Show();
        }

        private void txtPOAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false && e.KeyChar != 8 && e.KeyChar != 46)
                e.Handled = true;
        }

        private void lblInvNo_TextChanged(object sender, EventArgs e)
        {
            if (lblInvNo.Text != "")
            {
                lblInvoiced.Text = "(Inv. No. " + lblInvNo.Text + ")";
                lblInvoiced.Visible = true; lblInvoiced.BringToFront();
            }
            else
            {
                lblInvoiced.Visible = false;
            }
        }

        private void cboReasons_DropDown(object sender, EventArgs e)
        {
            txtRejected.Text = cboReasons.Text;
        }

        private void cboReasons_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (nMode == 0 || cboRevStatus.SelectedIndex != 2)
                cboReasons.Text = txtRejected.Text;
        }

        private void FileAccess()
        {
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; btnEMailQ.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; btnEMailQ.Enabled = false;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; btnEMailQ.Enabled = true;
            }
            else
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; btnEMailQ.Enabled = false;
            }
        }

        private void btnAdjPrice_Click(object sender, EventArgs e)
        {
            if (txtPercent.Text.Trim() == "" || txtPercent.Text.Trim() == "0")
            {
                MessageBox.Show("Please enter percentage value.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string strUP = "", strAmt = "";
            for (int i = 0; i < dtRevTests.Rows.Count; i++)
            {
                if (dtRevTests.Rows[i].RowState.ToString() != "Deleted" && dtRevTests.Rows[i]["UnitPrice"] != DBNull.Value)
                {
                    strUP = (Convert.ToDecimal(dtRevTests.Rows[i]["UnitPrice"]) * (1 + Convert.ToDecimal(txtPercent.Text) / 100)).ToString("#,##0.00");
                    decimal nUP = Math.Round(Convert.ToDecimal(strUP),1);
                    dtRevTests.Rows[i]["UnitPrice"] = nUP.ToString("#,##0.00");

                    strAmt = (Convert.ToDecimal(dtRevTests.Rows[i]["UnitPrice"]) * Convert.ToDecimal(dtRevTests.Rows[i]["BillQuantity"])).ToString("#,##0.00");
                    dtRevTests.Rows[i]["Amount"] = Convert.ToDecimal(strAmt);
                }
            }
            EstimatedTotal();
            MessageBox.Show("Prices successfully updated.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (txtPercent.Text == "" || (Char.IsDigit(e.KeyChar) == false && e.KeyChar != 8 && e.KeyChar != 46))
            //    e.Handled = true;
        }

        private void chkHideEstTotal_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHideEstTotal.Checked == true)
                txtHideEstTotal.Text = "True";
            else
                txtHideEstTotal.Text = "False";
        }

        private void txtHideEstTotal_TextChanged(object sender, EventArgs e)
        {
            if (txtHideEstTotal.Text == "True")
                chkHideEstTotal.Checked = true;
            else
                chkHideEstTotal.Checked = false;
        }
    }
}
