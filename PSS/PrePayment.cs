using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace PSS
{
    public partial class PrePayment : PSS.TemplateForm
    {
        public static string strQuoteNo;
        public static int nRevNo;
        public byte nQSw = 0;
        public Int32 nInvNo;

        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";

        //for DatagridView search
        private int nCtr = 0;
        private int nSw = 0;
        //======================

        private DataTable dtPPDtls = new DataTable();
        private DataTable dtInvoice = new DataTable();

        public PrePayment()
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

        private void PrePayment_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Prepayments");

            LoadRecords();
            BuildPrintItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

            dtInvoice.Columns.Add("CompanyCode", typeof(string));
            dtInvoice.Columns.Add("InvoiceNo", typeof(string));
            dtInvoice.Columns.Add("InvoiceDate", typeof(DateTime));
            dtInvoice.Columns.Add("InvoiceType", typeof(Int16));
            dtInvoice.Columns.Add("DateMailed", typeof(DateTime));
            dtInvoice.Columns.Add("DateRevised", typeof(DateTime));
            dtInvoice.Columns.Add("MailMode", typeof(Int16));
            dtInvoice.Columns.Add("MailedBy", typeof(string));
            dtInvoice.Columns.Add("SponsorID", typeof(Int16));
            dtInvoice.Columns.Add("SponsorName", typeof(string));
            dtInvoice.Columns.Add("ContactID", typeof(Int16));
            dtInvoice.Columns.Add("ContactName", typeof(string));
            dtInvoice.Columns.Add("InvoiceNotes", typeof(string));
            dtInvoice.Columns.Add("NonPrintingNotes", typeof(string));
            dtInvoice.Columns.Add("HeaderText", typeof(string));
            dtInvoice.Columns.Add("QuoteNo", typeof(string));
            dtInvoice.Columns.Add("PONo", typeof(string));
            bsInvoice.DataSource = dtInvoice;
            //Invoice Details
            dtPPDtls.Columns.Add("ServiceCode", typeof(Int16));
            dtPPDtls.Columns.Add("ServiceDesc", typeof(string));
            dtPPDtls.Columns.Add("AccountID", typeof(string));
            dtPPDtls.Columns.Add("Amount", typeof(decimal));
            dtPPDtls.Columns.Add("Adjustments", typeof(decimal));
            dtPPDtls.Columns.Add("InvoiceID", typeof(Int64));

            if (nQSw == 1)
            {
                PSSClass.General.FindRecord("InvoiceNo", nInvNo.ToString(), bsFile, dgvFile);
                LoadData();
            }
        }

        private void LoadRecords()
        {
            nMode = 0;
            DataTable dt = new DataTable();
            dt = PSSClass.Billing.PrepayMaster();
            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DataView dataView = new DataView(dt);
            bsFile.DataSource = dataView;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            if (tsddbSearch.DropDownItems.Count == 0)
            {
                //int ndx = 0;
                int i = 0;
                int n = 0;

                arrCol = new string[dt.Columns.Count];

                //foreach (DataColumn colFile in sqlds.Tables["Sponsors"].Columns)
                //{
                //    ndx = colFile.ColumnName.IndexOf("ID"); //search for the existence of the word "ID" in the field name
                //    if (ndx != -1)
                //    {
                //        n += 1;
                //    }
                //}

                ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

                foreach (DataColumn colFile in dt.Columns)
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
            dgvFile.Columns["CompanyCode"].Width = 75;
            dgvFile.Columns["InvoiceNo"].HeaderText = "INVOICE NO.";
            dgvFile.Columns["InvoiceNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["InvoiceDate"].HeaderText = "INVOICE DATE";
            dgvFile.Columns["InvoiceDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["InvoiceDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["InvoiceDate"].Width = 90;
            dgvFile.Columns["DateMailed"].HeaderText = "DATE MAILED";
            dgvFile.Columns["DateMailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateMailed"].DefaultCellStyle.Format = "MM/dd/yy hh:mm tt";
            dgvFile.Columns["DateMailed"].Width = 120;
            dgvFile.Columns["MailType"].HeaderText = "MAIL MODE";
            dgvFile.Columns["MailType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["QuoteNo"].HeaderText = "QUOTATION NO.";
            dgvFile.Columns["QuoteNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["PONo"].HeaderText = "PO NO.";
            dgvFile.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorID"].Width = 85;
            dgvFile.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["SponsorName"].Width = 300;
            dgvFile.Columns["Contact"].HeaderText = "CONTACT";
            dgvFile.Columns["Contact"].Width = 200;
            dgvFile.Columns["Amount"].HeaderText = "AMOUNT";
            dgvFile.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["Amount"].DefaultCellStyle.Format = "#,##0.00";
            dgvFile.Columns["Amount"].Width = 90;
            dgvFile.Columns["SponsorID"].Visible = false;
            dgvFile.Columns["ContactID"].Visible = false;
            dgvFile.Columns["HeaderText"].Visible = false;
        }

        private void BuildPrintItems()
        {
            //Create Print Menu Dropdown List
            if (tsddbPrint.DropDownItems.Count == 0)
            {
                DataTable dt = PSSClass.General.ReportsList("Prepayments");
                if (dt.Rows.Count > 0)
                {
                    //ToolStripMenuItem[] items = new ToolStripMenuItem[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tsddbPrint.DropDownItems.Add(dt.Rows[i]["ReportTitle"].ToString(), null, PrintRptClickHandler);
                        //items[i] = new ToolStripMenuItem();
                        //items[i].Name = dt.Rows[i]["ReportName"].ToString();
                        //items[i].Text = dt.Rows[i]["ReportTitle"].ToString();
                        //items[i].Click += new EventHandler(PrintRptClickHandler);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tsddbPrint.DropDownItems[i].Name = dt.Rows[i]["ReportName"].ToString();
                    }
                    //tsddbPrint.DropDownItems.AddRange(items);
                    //tsddbPrint.DropDownItems.Add("-", null, PrintRptClickHandler);
                }
            }
        }

        private void PrintRptClickHandler(object sender, EventArgs e)
        {
            SalesRptSettings rpt = new SalesRptSettings();
            rpt.WindowState = FormWindowState.Normal;

            string s = sender.GetType().ToString();
            rpt.rptTitle = rptTitle.Replace("&&", "&");
            rpt.rptName = rptName;
            rpt.Text = rptTitle.Replace("&&", "&");
            rpt.ShowDialog();
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData();
                btnClose.Visible = true;
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
        
        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
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
            bsInvoice.CancelEdit();
            ClearControls(pnlRecord);
            AddEditMode(false);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            btnEMail.Enabled = true; btnPrintPreview.Enabled = true; btnPrint.Enabled = true;
            nMode = 0;
        }

        private void LoadData()
        {
            nMode = 0;
            OpenControls(pnlRecord, false);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            dtInvoice.Rows.Clear();
            txtCmpyCode.Text = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["CompanyCode"].Value.ToString();
            txtInvNo.Text = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["InvoiceNo"].Value.ToString();
            LoadInvoice();
            LoadBillItems();
            btnClose.Visible = true; dgvPrepayDtls.Enabled = true;
            if (txtDateRev.Text != "")
                chkRevised.Checked = true;
            else
                chkRevised.Checked = false;
            if (txtDateCancelled.Text != "")
                chkCancelled.Checked = true;
            else
                chkCancelled.Checked = false;
            //Check if Paid
            picPaid.Visible = false; lblDatePaid.Visible = false;
            DataTable dt = PSSClass.FinalBilling.InvPaid(Convert.ToInt32(txtInvNo.Text));
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["AmountPaid"].ToString() != "" && Convert.ToDecimal(dt.Rows[0]["AmountPaid"]) * (-1) >= Convert.ToDecimal(txtAmtDue.Text.Replace("$", "")))
                {
                    picPaid.Visible = true; lblDatePaid.Visible = true; lblDatePaid.Text = dt.Rows[0]["DatePaid"].ToString();
                }
                else
                {
                    picPaid.Visible = false; lblDatePaid.Visible = false;
                }
                dt.Dispose();
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

                dtInvoice = PSSClass.Billing.PrepayInvMstrRec (txtCmpyCode.Text, Convert.ToInt32(txtInvNo.Text));
                bsInvoice.DataSource = dtInvoice;

                txtCmpyCode.DataBindings.Add("Text", bsInvoice, "CompanyCode", true);
                txtInvNo.DataBindings.Add("Text", bsInvoice, "InvoiceNo", true);
                dtpInvDate.DataBindings.Add("Value", bsInvoice, "InvoiceDate", true);
                txtSponsorID.DataBindings.Add("Text", bsInvoice, "SponsorID",true);
                txtSponsor.DataBindings.Add("Text", bsInvoice, "SponsorName", true);
                txtContactID.DataBindings.Add("Text", bsInvoice, "ContactID", true);
                txtContact.DataBindings.Add("Text", bsInvoice, "ContactName", true);
                txtDateMailed.DataBindings.Add("Text", bsInvoice, "DateMailed", true);
                txtDateRev.DataBindings.Add("Text", bsInvoice, "DateRevised", true);
                txtDateCancelled.DataBindings.Add("Text", bsInvoice, "DateCancelled", true);
                txtMMode.DataBindings.Add("Text", bsInvoice, "MailMode", true);
                txtMailedBy.DataBindings.Add("Text", bsInvoice, "MailedBy", true);
                txtInvNotes.DataBindings.Add("Text", bsInvoice, "InvoiceNotes", true);
                txtIntNotes.DataBindings.Add("Text", bsInvoice, "NonPrintingNotes", true);
                //chkPosted.DataBindings.Add("Checked", bsInvoice, "Posted", true);
                txtRefQuote.DataBindings.Add("Text", bsInvoice, "QuoteNo", true);
                txtPONo.DataBindings.Add("Text", bsInvoice, "PONo", true);
                txtHeader.DataBindings.Add("Text", bsInvoice, "HeaderText", true);
            }
            catch { }
        }

        private void LoadBillItems()
        {
            dtPPDtls = PSSClass.Billing.PrepayInvDtls(txtCmpyCode.Text, Convert.ToInt32(txtInvNo.Text));
            bsInvDtls.DataSource = dtPPDtls;
            dgvPrepayDtls.DataSource = bsInvDtls;
            BillItemsGridSetting();
            txtAmtDue.Text = "0.00"; txtAmount.Text = " 0.00"; txtAdjustments.Text = "0.00";
            decimal nAmt, nAdj;
            for (int i = 0; i < dtPPDtls.Rows.Count; i++)
            {
                nAmt= 0; nAdj = 0;
                if (dtPPDtls.Rows[i]["Amount"].ToString() != "")
                    nAmt = Convert.ToDecimal(dtPPDtls.Rows[i]["Amount"].ToString());
                if (dtPPDtls.Rows[i]["Adjustments"].ToString() != "")
                    nAdj = Convert.ToDecimal(dtPPDtls.Rows[i]["Adjustments"].ToString());
                txtAmount.Text = (Convert.ToDecimal(txtAmount.Text) + nAmt).ToString("#,##0.00");
                txtAdjustments.Text = (Convert.ToDecimal(txtAdjustments.Text) + nAdj).ToString("#,##0.00");
                txtAmtDue.Text = (Convert.ToDecimal(txtAmtDue.Text) + nAmt + nAdj).ToString("#,##0.00");
            }
        }


        private void BillItemsGridSetting()
        {
            dgvPrepayDtls.EnableHeadersVisualStyles = false;
            dgvPrepayDtls.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPrepayDtls.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvPrepayDtls.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPrepayDtls.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvPrepayDtls.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPrepayDtls.Columns["ServiceDesc"].HeaderText = "DESCRIPTION";
            dgvPrepayDtls.Columns["AccountID"].HeaderText = "GL CODE";
            dgvPrepayDtls.Columns["AccountID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPrepayDtls.Columns["Amount"].HeaderText = "AMOUNT";
            dgvPrepayDtls.Columns["Adjustments"].HeaderText = "ADJUSTMENTS";
            dgvPrepayDtls.Columns["ServiceCode"].Width = 100;
            dgvPrepayDtls.Columns["ServiceDesc"].Width = 370;
            dgvPrepayDtls.Columns["AccountID"].Width = 100;
            dgvPrepayDtls.Columns["Amount"].Width = 115;
            dgvPrepayDtls.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPrepayDtls.Columns["Amount"].DefaultCellStyle.Format = "#,##0.00";
            dgvPrepayDtls.Columns["Adjustments"].Width = 115;
            dgvPrepayDtls.Columns["Adjustments"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPrepayDtls.Columns["Adjustments"].DefaultCellStyle.Format = "#,##0.00";
            dgvPrepayDtls.Columns["InvoiceID"].Visible = false;
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            if (dgvFile.Rows.Count > 0)
            {
                if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "QuoteNo" && dgvFile.CurrentCell.Value.ToString() != "")
                {
                    int nI = dgvFile.CurrentCell.Value.ToString().IndexOf("R");
                    string strQNo = dgvFile.CurrentCell.Value.ToString().Substring(0, nI - 1);
            
                    int intOpen = PSSClass.General.OpenForm(typeof(Quotes));

                    if (intOpen == 1)
                    {
                        PSSClass.General.CloseForm(typeof(Quotes));
                    }
                    Quotes childForm = new Quotes();
                    childForm.Text = "QUOTATIONS";
                    childForm.MdiParent = this.MdiParent;
                    childForm.strQuoteNo = strQNo;
                    childForm.nPSw = 1;
                    childForm.Show();
                }
                else
                    LoadData();
            }
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

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            LoadRecords();
            bsFile.Filter = "InvoiceNo<>0";
            tsbRefresh.Enabled = false;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
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

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            btnEMail.Enabled = false; btnPrintPreview.Enabled = false; btnPrint.Enabled = false;

            ClearControls(pnlRecord);
            OpenControls(pnlRecord, false);

            dtpInvDate.Enabled = true; txtHeader.ReadOnly = false;
            txtInvNotes.ReadOnly = false; txtIntNotes.ReadOnly = false;
            dgvPrepayDtls.Enabled = true;

            using (PrepaymentList ppaylist = new PrepaymentList())
            {
                ppaylist.ShowDialog();
                if (ppaylist.DialogResult == DialogResult.Cancel)
                {
                    ppaylist.Dispose();
                    nMode = 0;
                    CancelSave();
                    return;
                }
                ppaylist.Dispose();
            }
            dtPPDtls.Rows.Clear();
            txtCmpyCode.Text = "P";
            DataTable dt = new DataTable();
            dt = PSSClass.Billing.PrepayInvRef(txtCmpyCode.Text, strQuoteNo, nRevNo);
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Incomplete billing information." + Environment.NewLine + "Please check quotation details.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dtInvoice.Rows.Clear();
            DataRow dR = dtInvoice.NewRow();
            dR["CompanyCode"] = txtCmpyCode.Text;
            dR["InvoiceNo"] = "(New)";
            dR["InvoiceDate"] = DateTime.Now;
            dR["InvoiceType"] = 1;
            dR["DateMailed"] = DBNull.Value;
            dR["MailMode"] = DBNull.Value;
            dR["MailedBy"] = "";
            dR["SponsorID"] = dt.Rows[0]["SponsorID"];
            dR["SponsorName"] = dt.Rows[0]["SponsorName"];
            dR["ContactID"] = dt.Rows[0]["ContactID"];
            dR["ContactName"] = dt.Rows[0]["ContactName"];
            dR["InvoiceNotes"] = "THIS INVOICE REPRESENTS PREPAYMENT FOR THE " + dt.Rows[0]["InvoiceNotes"].ToString().ToUpper() + Environment.NewLine + " AS PER QUOTE #" + strQuoteNo + ".R" + nRevNo.ToString();
            dR["NonPrintingNotes"] = "";
            dR["HeaderText"] = "";
            dR["QuoteNo"] = strQuoteNo + ".R" + nRevNo.ToString();
            dR["PONo"] = dt.Rows[0]["Comments"].ToString().Replace("PO", "").Replace("#","").Trim();          
            dtInvoice.Rows.Add(dR);
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
            txtMMode.DataBindings.Add("Text", bsInvoice, "MailMode", true);
            txtMailedBy.DataBindings.Add("Text", bsInvoice, "MailedBy", true);
            txtInvNotes.DataBindings.Add("Text", bsInvoice, "InvoiceNotes", true);
            txtIntNotes.DataBindings.Add("Text", bsInvoice, "NonPrintingNotes", true);
            txtRefQuote.DataBindings.Add("Text", bsInvoice, "QuoteNo", true);
            txtPONo.DataBindings.Add("Text", bsInvoice, "PONo", true);
            txtHeader.DataBindings.Add("Text", bsInvoice, "HeaderText", true);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtPPDtls.NewRow();
                    dr["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                    dr["ServiceDesc"] = dt.Rows[i]["ServiceDesc"];
                    dr["AccountID"] = dt.Rows[i]["AccountID"]; ;
                    dr["Amount"] = dt.Rows[i]["Amount"];
                    dr["Adjustments"] = dt.Rows[i]["Adjustments"];
                    dr["InvoiceID"] = dt.Rows[i]["InvoiceID"];
                    dtPPDtls.Rows.Add(dr);
                }
            }
            bsInvDtls.DataSource = dtPPDtls;
            dgvPrepayDtls.DataSource = bsInvDtls;
            txtAmtDue.Text = "0.00"; txtAmount.Text = " 0.00"; txtAdjustments.Text = "0.00";
            for (int i = 0; i < dtPPDtls.Rows.Count; i++)
            {
                txtAmtDue.Text = (Convert.ToDecimal(txtAmtDue.Text) + Convert.ToDecimal(dtPPDtls.Rows[i]["Amount"].ToString()) + Convert.ToDecimal(dtPPDtls.Rows[i]["Adjustments"].ToString())).ToString("#,##0.00");
                txtAmount.Text = (Convert.ToDecimal(txtAmount.Text) + Convert.ToDecimal(dtPPDtls.Rows[i]["Amount"].ToString())).ToString("#,##0.00");
                txtAdjustments.Text = (Convert.ToDecimal(txtAdjustments.Text) + Convert.ToDecimal(dtPPDtls.Rows[i]["Adjustments"].ToString())).ToString("#,##0.00");
            }
            BillItemsGridSetting();
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;

            if (pnlRecord.Visible == false)
            {
                LoadData();
            }
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront(); 
            OpenControls(pnlRecord, true);
            btnClose.Visible = false;
            nMode = 2;
            txtSponsorID.Select(); txtSponsorID.SelectAll();
            btnEMail.Enabled = false;
        }

        private void SaveRecord()
        {
            if (nMode == 1)
                txtInvNo.Text = PSSClass.General.NewID("Invoices", "InvoiceNo").ToString();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(txtInvNo.Text));
            sqlcmd.Parameters.AddWithValue("@InvDate", dtpInvDate.Value);
            sqlcmd.Parameters.AddWithValue("@InvType", 1);
            sqlcmd.Parameters.AddWithValue("@Header", txtHeader.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@ConID", Convert.ToInt16(txtContactID.Text.Trim()));
            sqlcmd.Parameters.AddWithValue("@InvPrt", txtInvNotes.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@InvNonPrt", txtIntNotes.Text.Trim());
            if (chkRevised.Checked == true)
                sqlcmd.Parameters.AddWithValue("@DateRev", DateTime.Now);
            else
                sqlcmd.Parameters.AddWithValue("@DateRev", DBNull.Value);
            if (chkCancelled.Checked == true)
                sqlcmd.Parameters.AddWithValue("@DateCanc", DateTime.Now);
            else
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
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            sqlcmd.Dispose();

            int nI = txtRefQuote.Text.IndexOf("R");

            string strQNo = txtRefQuote.Text.Substring(0, nI);
            string strRNo = txtRefQuote.Text.Substring((nI + 1), txtRefQuote.Text.Length - (nI + 1));

            for (int i = 0; i < dgvPrepayDtls.Rows.Count; i++)
            {
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", nMode);
                sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(txtInvNo.Text));
                sqlcmd.Parameters.AddWithValue("@InvID", Convert.ToInt32(dgvPrepayDtls.Rows[i].Cells["InvoiceID"].Value));
                sqlcmd.Parameters.AddWithValue("@QuoteNo", strQNo);
                sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
                sqlcmd.Parameters.AddWithValue("@CtrlNo", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@PONo", txtPONo.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@PSSNo", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@RptNo", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dgvPrepayDtls.Rows[i].Cells["ServiceCode"].Value));
                sqlcmd.Parameters.AddWithValue("@SCType", 1);
                sqlcmd.Parameters.AddWithValue("@BillQty", 1);
                sqlcmd.Parameters.AddWithValue("@UPrice", Convert.ToDecimal(dgvPrepayDtls.Rows[i].Cells["Amount"].Value));
                sqlcmd.Parameters.AddWithValue("@Amt", Convert.ToDecimal(dgvPrepayDtls.Rows[i].Cells["Amount"].Value));
                sqlcmd.Parameters.AddWithValue("@Adj", Convert.ToDecimal(dgvPrepayDtls.Rows[i].Cells["Adjustments"].Value.ToString().Replace(",", "")));
                sqlcmd.Parameters.AddWithValue("@PrePay", 0);
                sqlcmd.Parameters.AddWithValue("@RushTest", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@QCmpyCode", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@LCmpyCode", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@RCmpyCode", DBNull.Value);
                //sqlcmd.Parameters.AddWithValue("@AcctID", 0);//removed 1/27/2016
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
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
            sqlcmd.Dispose();
            sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@QuoteNo", strQNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
            sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(txtInvNo.Text));
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
            string strInvNo = txtInvNo.Text;
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            btnPrintPreview.Enabled = true; btnPrint.Enabled = true; btnEMail.Enabled = true; btnClose.Visible = true;
            OpenControls(pnlRecord, false);
            AddEditMode(false); //Initialize Toolbar
            LoadRecords();//Reload updated records
            PSSClass.General.FindRecord("InvoiceNo", strInvNo, bsFile, dgvFile);
            LoadData();
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

        private void dgvPrepayDtls_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;
            else if (dgvPrepayDtls.CurrentCell.OwningColumn.Name != "Adjustments")
                e.Cancel = true;
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            if (PSSClass.Sponsors.HasMultipleBillAddr(Convert.ToInt16(txtSponsorID.Text)) == true)
            {
                MessageBox.Show("This Sponsor has multiple billing addresses." + Environment.NewLine + "Please setup 1 billing address and try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            AcctgRpt rptPrepayment = new AcctgRpt();
            rptPrepayment.WindowState = FormWindowState.Maximized;
            rptPrepayment.nQ = 1;
            rptPrepayment.rptName = "PrepayInvoice";
            rptPrepayment.CmpyCode = txtCmpyCode.Text;
            try
            {
                rptPrepayment.nInvNo = Convert.ToInt32(txtInvNo.Text);
                rptPrepayment.Show();
            }
            catch { }
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (PSSClass.Sponsors.HasMultipleBillAddr(Convert.ToInt16(txtSponsorID.Text)) == true)
            {
                MessageBox.Show("This Sponsor has multiple billing addresses." + Environment.NewLine + "Please setup 1 billing address and try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            AcctgRpt rptPrepayment = new AcctgRpt();
            rptPrepayment.WindowState = FormWindowState.Maximized;
            rptPrepayment.nQ = 2;
            rptPrepayment.rptName = "PrepayInvoice";
            try
            {
                rptPrepayment.CmpyCode = txtCmpyCode.Text;
                rptPrepayment.nInvNo = Convert.ToInt32(txtInvNo.Text);
                rptPrepayment.Show();
            }
            catch { }
        }

        private void btnEMail_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                MessageBox.Show("This invoice is in add or edit mode. " + Environment.NewLine + "Please save or cancel changes made." + Environment.NewLine + "Process is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            if (txtInvNo.Text == "")
            {
                MessageBox.Show("Invoice is not yet created.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (PSSClass.Sponsors.HasMultipleBillAddr(Convert.ToInt16(txtSponsorID.Text)) == true)
            {
                MessageBox.Show("This Sponsor has multiple billing addresses." + Environment.NewLine + "Please setup 1 billing address and try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            AcctgRpt rptPrepayment = new AcctgRpt();
            rptPrepayment.WindowState = FormWindowState.Maximized;
            rptPrepayment.nQ = 3;
            rptPrepayment.CmpyCode = txtCmpyCode.Text;
            rptPrepayment.rptName = "PrepayInvoice";
            try
            {
                rptPrepayment.nInvNo = Convert.ToInt32(txtInvNo.Text);
                rptPrepayment.Show();
            }
            catch { }
            rptPrepayment.Close(); rptPrepayment.Dispose();

            lstAttachment.Items.Clear();
            lnkInvoice.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\Invoices\" + DateTime.Now.Year.ToString() + "\\" + Convert.ToInt32(txtInvNo.Text).ToString("0000000") + ".pdf";
            lstAttachment.Items.Add(lnkInvoice.Text);

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            sqlcmd = new SqlCommand("SELECT QuoteCCEMailAd FROM Employment WHERE LoginName = '" + LogIn.strUserID  + "'", sqlcnn);
            sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                txtCC.Text = sqldr.GetValue(0).ToString();
            }
            else
            {
                MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sqldr.Close(); sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();

            DataTable dt = new DataTable();
            dt = PSSClass.Sponsors.APData(Convert.ToInt16(txtSponsorID.Text));
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No A/P contact data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string strAP = dt.Rows[0]["APContact"].ToString();
            txtTo.Text = dt.Rows[0]["APEMail"].ToString();
            txtSubject.Text = "Invoice No. " + Convert.ToInt32(txtInvNo.Text).ToString("0000000");

            //txtBCC.Text = "ar@gibraltarlabsinc.com"; //A/R Monitoring
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
        }

        private void btnCancelSend_Click(object sender, EventArgs e)
        {
            pnlEMail.Visible = false; pnlRecord.Enabled = true;
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

        private void dgvPrepayDtls_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            txtAmount.Text = "0.00"; txtAmtDue.Text = "0.00"; txtAdjustments.Text = "0.00";
            for (int i = 0; i < dgvPrepayDtls.Rows.Count; i++)
            {
                txtAmtDue.Text = (Convert.ToDecimal(txtAmtDue.Text) + Convert.ToDecimal(dgvPrepayDtls.Rows[i].Cells["Amount"].Value) + Convert.ToDecimal(dgvPrepayDtls.Rows[i].Cells["Adjustments"].Value)).ToString("#,##0.00");
                txtAmount.Text = (Convert.ToDecimal(txtAmount.Text) + Convert.ToDecimal(dgvPrepayDtls.Rows[i].Cells["Amount"].Value)).ToString("#,##0.00");
                txtAdjustments.Text = (Convert.ToDecimal(txtAdjustments.Text) + Convert.ToDecimal(dgvPrepayDtls.Rows[i].Cells["Adjustments"].Value)).ToString("#,##0.00");
            }
        }

        private void txtRefQuote_DoubleClick(object sender, EventArgs e)
        {
            int nI = txtRefQuote.Text.IndexOf("R");
            string strQNo = txtRefQuote.Text.Substring(0, nI-1);
            
            int intOpen = PSSClass.General.OpenForm(typeof(Quotes));

            if (intOpen == 1)
            {
                PSSClass.General.CloseForm(typeof(Quotes));
            }
            Quotes childForm = new Quotes();
            childForm.Text = "QUOTATIONS";
            childForm.MdiParent = this.MdiParent;
            childForm.strQuoteNo = strQNo;
            childForm.nPSw = 1;
            childForm.Show();
        }

        private void txtPONo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtInvNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtRefQuote_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtDateMailed_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtMailedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtMailMode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtContactID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtDateRev_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtDateCancelled_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void FileAccess()
        {
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsddbPrint.Enabled = false; btnPrintPreview.Enabled = false; btnPrint.Enabled = false; btnEMail.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = false; btnPrintPreview.Enabled = true; btnPrint.Enabled = true; btnEMail.Enabled = false; 
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true; btnPrintPreview.Enabled = true; btnPrint.Enabled = true; btnEMail.Enabled = true; //tsbDelete.Enabled = false;
            }
            tsddbSearch.Enabled = true;
        }

        private void PrePayment_KeyDown(object sender, KeyEventArgs e)
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
    }
}
