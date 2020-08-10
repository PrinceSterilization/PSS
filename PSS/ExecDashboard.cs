using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Net.Mail;
using System.Net;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;

namespace PSS
{
    public partial class ExecDashboard : PSS.TemplateForm
    {
        byte nMode = 0;


        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;

        DataTable dtDashboard = new DataTable();                                         // MY 09/11/2015 - GridView Dashboard table
        DataTable dtDashboardSummary = new DataTable();                                  // MY 09/17/2015 - GridView Dashboard Summary table
        DataTable dtGenEntry = new DataTable();                                          // MY 09/15/2015 - GridView General Entry table
        private string strFileAccess = "RO";

        public ExecDashboard()
        {
            InitializeComponent();

            tsddbSearch.Enabled = false;
            LoadYear();
            GetDefaultYear();
            GetDefaultMonth();
            GetLastEntryDate();
            GetDefaultDates();
            
            LoadRecords(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text));            

            BuildPrintItems();
            BuildSearchItems();

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
            dgvDashboard.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvDashboard.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvDashboard.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            //dgvDashboard.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            //cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords(DateTime cStartDate, DateTime cEndDate)
        {
            DataTable dt = new DataTable();
            dt = PSSClass.ACCPAC.Dashboard(cStartDate, cEndDate, "Dashboard");

            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
      
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvDashboard.DataSource = bsFile;
            pnlDetails.Visible = true;
            DataGridSetting();          
            tsbDelete.Enabled = false;
            tsbCancel.Enabled = false;
            LoadSummary(Convert.ToDateTime(mskStartDate.Text), cEndDate, "Dashboard");  
        }

        private void GetDefaultDates()
        {  
            string strdate;

            strdate = cboMonth.Text.Trim() + "/01/" + cboYear.Text.Trim();

            DateTime startEntryDate;
            DateTime endEntryDate;

            DateTime now = Convert.ToDateTime(strdate);

            startEntryDate = new DateTime(now.Year, now.Month, 1);
            endEntryDate = startEntryDate.AddMonths(1).AddDays(-1);

            mskStartDate.Text = startEntryDate.ToString("MM/dd/yyyy");
            mskEndDate.Text = endEntryDate.ToString("MM/dd/yyyy");
        }

        private void CloseClickHandler(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
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

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "DashboardYearlyReport";
            items[0].Text = "Dashboard Yearly Report";
            items[0].Click += new EventHandler(PrintDashboardYearlyClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            //int i = 0;

            //DataTable dt = new DataTable();
            //dt = PSSClass.Quotations.QuoteFollowUp(0);

            //if (dt == null)
            //{
            //    MessageBox.Show("Connection problem encountered during build-up of search items." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    nMode = 9;
            //    return;
            //}
            //arrCol = new string[dt.Columns.Count];

            //ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length];

            //foreach (DataColumn colFile in dt.Columns)
            //{
            //    items[i] = new ToolStripMenuItem();
            //    items[i].Name = colFile.ColumnName;

            //    //Using LINQ to insert space between capital letters
            //    var val = colFile.ColumnName;
            //    val = string.Concat(val.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

            //    items[i].Text = val;
            //    items[i].Click += new EventHandler(SearchItemClickHandler);
            //    arrCol[i] = colFile.DataType.ToString();
            //    cklColumns.Items.Add(val);
            //    i += 1;
            //}
            //for (int j = 0; j < cklColumns.Items.Count; j++)
            //{
            //    cklColumns.SetItemChecked(j, true);
            //}

            //tsddbSearch.DropDownItems.AddRange(items);
            //tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            //tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
        }

        private void dgvCellMouseClickEventHandler(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                cklColumns.Visible = true; cklColumns.BringToFront();
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

        private void cklSelIdxChEventHandler(object sender, EventArgs e)
        {
            string strCol = cklColumns.Items[cklColumns.SelectedIndex].ToString().Replace(" ", "");
            if (dgvDashboard.Columns[strCol].Visible == true)
                dgvDashboard.Columns[cklColumns.SelectedIndex].Visible = false;
            else
                dgvDashboard.Columns[cklColumns.SelectedIndex].Visible = true;
            cklColumns.Visible = false;
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
            if (tstbSearch.Text.Trim() != "")
            {
                try
                {
                    bsFile.Filter = "EntryDate<>'01/01/1800'";
                    PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvDashboard);
                    dgvDashboard.Select();                                        
                    LoadRecords(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text));
                }
                catch { }
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
                    else
                    {
                        bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;
                    }

                    tsbRefresh.Enabled = true;
                    
                }
                catch { }
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            bsFile.Filter = "EntryDate<>'01/01/1800";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
           
        }

        private void PrintDashboardYearlyClickHandler(object sender, EventArgs e)
        {
            if (cboYear.Text.Trim() == "")
            {
                MessageBox.Show("Please enter report year!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboYear.Focus();
                return;
            }

            if (Convert.ToInt32(cboYear.Text.Trim()) < 2000 || Convert.ToInt32(cboYear.Text.Trim()) > 2500)
            {
                MessageBox.Show("Please enter valid year!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboYear.Focus();
                return;
            }

            DashboardYearlyRpt rpt = new DashboardYearlyRpt();

            String strRptStart;
            String strRptEnd;

            strRptStart = "01/01/" + cboYear.Text;
            strRptEnd   = "12/31/" + cboYear.Text;

            rpt.WindowState = FormWindowState.Maximized;
            rpt.StartDate = Convert.ToDateTime(strRptStart);
            rpt.EndDate = Convert.ToDateTime(strRptEnd);
            rpt.pubDashBoardTable = "Dashboard";

            try
            {
                rpt.Show();
            }
            catch { }
        }
      
        private void LoadData()
        {
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            pnlGenEntry.Visible = false;            

            nMode = 0;

            btnClose.Visible = true; btnClose.BringToFront();
          
            ClearGenEntry();

            String strDate;

            strDate = dgvDashboard.CurrentRow.Cells["EntryDate"].Value.ToString();
           
            LoadGenEntry(Convert.ToDateTime(strDate));            

            AddEditMode(false);

            OpenControls(pnlRecord, false);
            
        }

        private void LoadYear()
        {
            cboYear.Text = "";
            cboYear.DataSource = null;
            DataTable dt = new DataTable();
            dt = PSSClass.ACCPAC.DashboardYears();
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            cboYear.DataSource = dt;
            cboYear.DisplayMember = "YearNo";
            cboYear.ValueMember = "YearNo";
        }
        
        private void LoadGenEntry(DateTime cEntryDate)
        {
            dtGenEntry= null;
            dtGenEntry = PSSClass.ACCPAC.DashboardRecord(cEntryDate, "Dashboard");
            if (dtGenEntry == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            bsGenEntry.DataSource = dtGenEntry;
            bnGenEntry.BindingSource = bsGenEntry;
            ClearControls(this.pnlGenEntry);
            
            BindGenEntry();
        }

        private void LoadSummary(DateTime cStartDate, DateTime cEndDate, string cDashBoardTable)
        {
            dtDashboardSummary = null;
            dtDashboardSummary = PSSClass.ACCPAC.DashboardSummary(cStartDate, cEndDate, "Dashboard");

            if (dtDashboardSummary == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }

            bsSummary.DataSource = dtDashboardSummary;
            dgvSummary.DataSource = bsSummary;
            DataGridSummarySetting();
        }

        private void ClearGenEntry()
        {
            foreach (Control c in pnlGenEntry.Controls)
            {
                c.DataBindings.Clear();
            }
        }

        private void CreateDetailStructure()
        {
            dtGenEntry.Columns.Add("EntryDate", typeof(DateTime));
            dtGenEntry.Columns.Add("CashReceipts", typeof(decimal));
            dtGenEntry.Columns.Add("CheckBalance", typeof(decimal));
            dtGenEntry.Columns.Add("CashReceipts2", typeof(decimal));
            dtGenEntry.Columns.Add("CheckBalance2", typeof(decimal));
            dtGenEntry.Columns.Add("NewInvoice", typeof(decimal));
            dtGenEntry.Columns.Add("AdjNewInvoice", typeof(decimal));
            dtGenEntry.Columns.Add("NewPayables", typeof(decimal));
            dtGenEntry.Columns.Add("PayrollTaxes", typeof(decimal));
            dtGenEntry.Columns.Add("PayrollDeposit", typeof(decimal));
            dtGenEntry.Columns.Add("Miscellaneous", typeof(decimal));
        }

        private void BindGenEntry()
        {
            // Clear bindings first
            foreach (Control c in pnlGenEntry.Controls)
            {
                c.DataBindings.Clear();
            }

            try
            {
                txtCashReceipts1.DataBindings.Add("Text", bsGenEntry, "CashReceipts");
                txtChkBal1.DataBindings.Add("Text", bsGenEntry, "CheckBalance");
                txtCashReceipts2.DataBindings.Add("Text", bsGenEntry, "CashReceipts2", true, DataSourceUpdateMode.Never, "", "#,##0.00");
                txtChkBal2.DataBindings.Add("Text", bsGenEntry, "CheckBalance2", true, DataSourceUpdateMode.Never, "", "#,##0.00");
                txtNewInvoice.DataBindings.Add("Text", bsGenEntry, "NewInvoice", true, DataSourceUpdateMode.Never, "", "#,##0.00");
                txtAdjNewInv.DataBindings.Add("Text", bsGenEntry, "AdjNewInvoice", true, DataSourceUpdateMode.Never, "", "#,##0.00");
                txtNewPay.DataBindings.Add("Text", bsGenEntry, "NewPayables", true, DataSourceUpdateMode.Never, "", "#,##0.00");
                txtPayTaxes.DataBindings.Add("Text", bsGenEntry, "PayrollTaxes", true, DataSourceUpdateMode.Never, "", "#,##0.00");
                txtPayDeposit.DataBindings.Add("Text", bsGenEntry, "PayrollDeposit", true, DataSourceUpdateMode.Never, "", "#,##0.00");
                txt401K.DataBindings.Add("Text", bsGenEntry, "Miscellaneous", true, DataSourceUpdateMode.Never, "", "#,##0.00");

                Binding EntryDateCreatedBinding;
                EntryDateCreatedBinding = new Binding("Text", bsGenEntry, "EntryDate");
                EntryDateCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
                mskEntryDate.DataBindings.Add(EntryDateCreatedBinding); 
            }
            catch
            { }
        }
        private void GetLastEntryDate()
        {
            String lastDateEntered;

            lastDateEntered = PSSClass.ACCPAC.LastEntryDate("Dashboard");
            txtLastEntry.Text = lastDateEntered.Substring(0, 10);
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void DataGridSetting()
        {
           
            dgvDashboard.DefaultCellStyle.Font = new Font("Arial", 8);
            dgvDashboard.ColumnHeadersHeight = 12;
            dgvDashboard.RowTemplate.Height = 15;           
            dgvDashboard.EnableHeadersVisualStyles = false;
            dgvDashboard.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDashboard.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvDashboard.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDashboard.Columns["EntryDate"].HeaderText = "Entry Date";
            dgvDashboard.Columns["DayName"].HeaderText = "Day";  
            dgvDashboard.Columns["CashReceipts2"].HeaderText = "Cash Receipts";
            dgvDashboard.Columns["CheckBalance2"].HeaderText = "Check Balance";
            dgvDashboard.Columns["NewInvoice"].HeaderText = "New Invoice";
            dgvDashboard.Columns["AdjNewInvoice"].HeaderText = "Adj New Invoice";
            dgvDashboard.Columns["NewPayables"].HeaderText = "New Payables";
            dgvDashboard.Columns["PayrollTaxes"].HeaderText = "Payroll Taxes";
            dgvDashboard.Columns["PayrollDeposit"].HeaderText = "Payroll deposit";
            dgvDashboard.Columns["Miscellaneous"].HeaderText = "401 K";
            dgvDashboard.Columns["EntryDate"].Width = 70;
            dgvDashboard.Columns["DayName"].Width = 35; 
            dgvDashboard.Columns["CashReceipts2"].Width = 90;
            dgvDashboard.Columns["CheckBalance2"].Width = 90;
            dgvDashboard.Columns["NewInvoice"].Width = 90;
            dgvDashboard.Columns["AdjNewInvoice"].Width = 90;
            dgvDashboard.Columns["NewPayables"].Width = 90;
            dgvDashboard.Columns["PayrollTaxes"].Width = 90;
            dgvDashboard.Columns["PayrollDeposit"].Width = 90;
            dgvDashboard.Columns["Miscellaneous"].Width = 90;
            dgvDashboard.Columns["CashReceipts"].Visible = false;
            dgvDashboard.Columns["CheckBalance"].Visible = false;
            dgvDashboard.Columns["SnakeGoddess"].Visible = false;
            dgvDashboard.Columns["CreatedByID"].Visible = false;
            dgvDashboard.Columns["DateCreated"].Visible = false;
            dgvDashboard.Columns["LastUpdate"].Visible = false;
            dgvDashboard.Columns["LastUserID"].Visible = false;
            dgvDashboard.Columns["CashReceipts2"].DefaultCellStyle.Format = "#,##0.00";
            dgvDashboard.Columns["CheckBalance2"].DefaultCellStyle.Format = "#,##0.00";
            dgvDashboard.Columns["NewInvoice"].DefaultCellStyle.Format = "#,##0.00";
            dgvDashboard.Columns["AdjNewInvoice"].DefaultCellStyle.Format = "#,##0.00";
            dgvDashboard.Columns["NewPayables"].DefaultCellStyle.Format = "#,##0.00";
            dgvDashboard.Columns["PayrollTaxes"].DefaultCellStyle.Format = "#,##0.00";
            dgvDashboard.Columns["PayrollDeposit"].DefaultCellStyle.Format = "#,##0.00";
            dgvDashboard.Columns["Miscellaneous"].DefaultCellStyle.Format = "#,##0.00";
            dgvDashboard.Columns["EntryDate"].DefaultCellStyle.Format = "MM/dd/yyyy";           
            dgvDashboard.Columns["EntryDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDashboard.Columns["DayName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;  
            dgvDashboard.Columns["CashReceipts2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDashboard.Columns["CheckBalance2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDashboard.Columns["NewInvoice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDashboard.Columns["AdjNewInvoice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDashboard.Columns["NewPayables"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDashboard.Columns["PayrollTaxes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDashboard.Columns["PayrollDeposit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDashboard.Columns["Miscellaneous"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;           
        }

        private void DataGridSummarySetting()
        {
            
            dgvSummary.DefaultCellStyle.Font = new Font("Arial", 8);
            dgvSummary.ColumnHeadersHeight = 12;
            dgvSummary.RowTemplate.Height = 15;  
            dgvSummary.ColumnHeadersVisible = false;
            dgvSummary.Columns["CashReceipts2"].HeaderText = "Cash Receipts";
            dgvSummary.Columns["CheckBalance2"].HeaderText = "Check Balance";
            dgvSummary.Columns["NewInvoice"].HeaderText = "New Invoice";
            dgvSummary.Columns["AdjNewInvoice"].HeaderText = "Adj New Invoice";
            dgvSummary.Columns["NewPayables"].HeaderText = "New Payables";
            dgvSummary.Columns["PayrollTaxes"].HeaderText = "Payroll Taxes";
            dgvSummary.Columns["PayrollDeposit"].HeaderText = "Payroll deposit";
            dgvSummary.Columns["Miscellaneous"].HeaderText = "401 K";            
            dgvSummary.Columns["CashReceipts2"].Width = 90;
            dgvSummary.Columns["CheckBalance2"].Width = 90;
            dgvSummary.Columns["NewInvoice"].Width = 90;
            dgvSummary.Columns["AdjNewInvoice"].Width = 90;
            dgvSummary.Columns["NewPayables"].Width = 90;
            dgvSummary.Columns["PayrollTaxes"].Width = 90;
            dgvSummary.Columns["PayrollDeposit"].Width = 90;
            dgvSummary.Columns["Miscellaneous"].Width = 90;            
            dgvSummary.Columns["CashReceipts2"].DefaultCellStyle.Format = "#,##0.00";
            dgvSummary.Columns["CheckBalance2"].DefaultCellStyle.Format = "#,##0.00";
            dgvSummary.Columns["NewInvoice"].DefaultCellStyle.Format = "#,##0.00";
            dgvSummary.Columns["AdjNewInvoice"].DefaultCellStyle.Format = "#,##0.00";
            dgvSummary.Columns["NewPayables"].DefaultCellStyle.Format = "#,##0.00";
            dgvSummary.Columns["PayrollTaxes"].DefaultCellStyle.Format = "#,##0.00";
            dgvSummary.Columns["PayrollDeposit"].DefaultCellStyle.Format = "#,##0.00";
            dgvSummary.Columns["Miscellaneous"].DefaultCellStyle.Format = "#,##0.00";
            dgvSummary.Columns["CashReceipts"].Visible = false;
            dgvSummary.Columns["CheckBalance"].Visible = false;           
            dgvSummary.Columns["CashReceipts2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["CheckBalance2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["NewInvoice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["AdjNewInvoice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["NewPayables"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["PayrollTaxes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["PayrollDeposit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["Miscellaneous"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;  
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
            OpenControls(this.pnlRecord, false);
            LoadData();
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
                OpenControls(this.pnlRecord, false);
                LoadData();
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
            pnlGenEntry.Visible = true;
            ClearControls(this.pnlGenEntry);
            OpenControls(this.pnlGenEntry, true);
           
            txtCashReceipts2.Text = "";           
            txtNewInvoice.Text = "";
            txtAdjNewInv.Text = "";
            txtNewPay.Text = "";
            txtPayTaxes.Text = "";
            txtPayDeposit.Text = "";
            txt401K.Text = "";

            lblNew.Text = "< New >";                      

            GetLastEntryDate();

            string strDate;
            DateTime dtNewDate;
            strDate = txtLastEntry.Text;

            dtNewDate = Convert.ToDateTime(strDate);

            mskEntryDate.Text = dtNewDate.AddDays(1).ToString("MM/dd/yyyy");

            LoadRecords(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEntryDate.Text));

            txtCashReceipts2.Focus();
            mskStartDate.Enabled = false;
            mskEndDate.Enabled = false;
            btnLoad2.Enabled = false;
            btnCreate.Enabled = false;
            GetDayName();
        }

        private void EditRecord()
        {           
            nMode = 2;           
            LoadData();
            pnlGenEntry.Visible = true;
            pnlGenEntry.BringToFront();
            OpenControls(this.pnlGenEntry, true);            
            txtCashReceipts2.Focus(); btnClose.Visible = false;
            AddEditMode(true);
            mskStartDate.Enabled = false;
            mskEndDate.Enabled = false;
            btnLoad2.Enabled = false;
            btnCreate.Enabled = false;
            GetDayName();
        }

        private void DeleteRecord()
        {
        }

        private void SaveRecord()
        {
            if (nMode == 1)
            {
                // Allow only Current Date and below
                var dtDateTime = DateTime.Now;
                var dtDate = dtDateTime.Date;

                var mskDateTime = Convert.ToDateTime(mskEntryDate.Text);
                var mskDate = mskDateTime.Date;

                if (mskDate > dtDate)
                {
                    MessageBox.Show("Entry for future dates not allowed!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    mskEntryDate.Focus();
                    return;
                }

                // Allow only new day
                bool isExists = false;

                isExists = PSSClass.ACCPAC.EntryDateExists(Convert.ToDateTime(mskEntryDate.Text), "Dashboard");

                if (isExists)
                {
                    MessageBox.Show("Entry Date already exists. Please choose another!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    mskEntryDate.Focus();
                    return;
                }
            }

            SqlConnection sqlcnn = PSSClass.DBConnection.MDFConnection("172.16.4.12", "PTSFinancials", true, "", "", "");
            SqlCommand sqlcmd = new SqlCommand("spAddEditDashboardEntry", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@EntryDate", Convert.ToDateTime(mskEntryDate.Text));

            if (txtCashReceipts1.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@CashReceipts", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@CashReceipts", Convert.ToDecimal(txtCashReceipts1.Text));
            }
             if (txtChkBal1.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@CheckBalance", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@CheckBalance", Convert.ToDecimal(txtChkBal1.Text));
             }
            if (txtCashReceipts2.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@CashReceipts2", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@CashReceipts2", Convert.ToDecimal(txtCashReceipts2.Text));
            }
            if (txtChkBal2.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@CheckBalance2", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@CheckBalance2", Convert.ToDecimal(txtChkBal2.Text));
            }
            if (txtNewInvoice.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@NewInvoice", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@NewInvoice", Convert.ToDecimal(txtNewInvoice.Text));
            }
            if (txtAdjNewInv.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@AdjNewInvoice", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@AdjNewInvoice", Convert.ToDecimal(txtAdjNewInv.Text));
            }
            if (txtNewPay.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@NewPayables", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@NewPayables", Convert.ToDecimal(txtNewPay.Text));
            }
            if (txtPayTaxes.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@PayrollTaxes", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@PayrollTaxes", Convert.ToDecimal(txtPayTaxes.Text));
            }
            if (txtPayDeposit.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@PayrollDeposit", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@PayrollDeposit", Convert.ToDecimal(txtPayDeposit.Text));
            }
            if (txt401K.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@Miscellaneous", 0.00);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@Miscellaneous", Convert.ToDecimal(txt401K.Text));
            }
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

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
            sqlcnn.Dispose();
            dgvDashboard.Refresh();
            pnlGenEntry.Visible = false;
            GetLastEntryDate();
            LoadRecords(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(txtLastEntry.Text));            
            AddEditMode(false);
            mskStartDate.Enabled = true;
            mskEndDate.Enabled = true;
            btnLoad2.Enabled = true;
        }

        private void GetDefaultYear()
        {
            DateTime mDate;
            mDate = DateTime.Now;
            cboYear.Text = Convert.ToString(mDate.Year);
        }

        private void GetDefaultMonth()
        {
            DateTime mDate;
            mDate = DateTime.Now;
            cboMonth.Text = Convert.ToString(mDate.Month);
        }

        private void GetDayName()
        {
            DateTime mDate;
            mDate = Convert.ToDateTime(mskEntryDate.Text);
            lblDayName.Text = "( " + Convert.ToString(mDate.DayOfWeek) + " )";
        }

        private void GenerateEntryDays(DateTime cDate)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.MDFConnection("172.16.4.12", "PTSFinancials", true, "", "", "");
            SqlCommand sqlcmd = new SqlCommand("spAddDashboardAutoEntry", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@EntryDate", cDate);            
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

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
            sqlcnn.Dispose();
            dgvDashboard.Refresh();
            GetLastEntryDate();
            GetDefaultDates();
            LoadRecords(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text));
            AddEditMode(false);
            mskStartDate.Enabled = true;
            mskEndDate.Enabled = true;
            btnLoad2.Enabled = true;
        }

        private void CancelSave()
        {
            btnClose_Click(null, null);
        }

        private void ExecDashboard_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "ExecDashBoard");
            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }
            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
            CreateDetailStructure();
        }

        private void ExecDashboard_KeyDown(object sender, KeyEventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlGenEntry.Visible = false;
            AddEditMode(false);
            mskStartDate.Enabled = true;
            mskEndDate.Enabled = true;
            btnLoad2.Enabled = true;
            btnClose.Visible = true;
            btnCreate.Enabled = true;

            //pnlRecord.Visible = false; dgvDashboard.Visible = true; dgvDashboard.BringToFront(); btnClose.Visible = false;
            //dgvDashboard.Focus();
            //this.Close(); this.Dispose();
        }

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
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

        private void DateBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (cboYear.Text.Trim() == "")
            {
                MessageBox.Show("Year is empty!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboYear.Focus();
                return;
            }

            int n;
            bool isNumeric;
            
            isNumeric = int.TryParse(cboYear.Text.Trim(), out n);

            if (isNumeric)
            {
                if (Convert.ToInt16(cboYear.Text.Trim()) < 2000)
                {
                    MessageBox.Show("Invalid Year!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboYear.Text = "";
                    cboYear.Focus();
                    return;
                }               
            }
            else
            {
                MessageBox.Show("Invalid Year!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboYear.Text = "";
                cboYear.Focus();
                return;
            }

            if (cboMonth.Text.Trim() == "")
            {
                MessageBox.Show("Month is empty!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboMonth.Focus();
                return;
            }
            
            isNumeric = int.TryParse(cboMonth.Text.Trim(), out n);

            if (isNumeric)
            {
                if (Convert.ToInt16(cboMonth.Text.Trim()) < 1)
                {
                    MessageBox.Show("Invalid Month!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboMonth.Text = "";
                    cboMonth.Focus();
                    return;
                }

                if (Convert.ToInt16(cboMonth.Text) > 12)
                {
                    MessageBox.Show("Invalid Month!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboMonth.Text = "";
                    cboMonth.Focus();
                    return;
                }

            }
            else
            {
                MessageBox.Show("Invalid Month!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboMonth.Text = "";
                cboMonth.Focus();
                return;
            }
            GetDefaultDates();
            
            LoadRecords(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text));

            if (dgvDashboard.Rows.Count == 0)
            {
                MessageBox.Show("No records found for this date range. Pls. try again!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataGridSetting();
            AddEditMode(false);
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

        // MY 09/11/2015 - Start: Date events       
        private void mskEntryDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.BringToFront(); pnlCalendar.Visible = true; pnlCalendar.Location = new Point(422, 15); 
        }

        private void mskStartDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar2.BringToFront(); pnlCalendar2.Visible = true; pnlCalendar2.Location = new Point(437, 404); 
        }

        private void mskEndDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar2.BringToFront(); pnlCalendar2.Visible = true; pnlCalendar2.Location = new Point(597, 404); 
        }

        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            mskEntryDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            GetDayName();
            pnlCalendar.Visible = false;
        }

        private void cal2_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (pnlCalendar2.Location == new Point(437, 404))
            {
                mskStartDate.Text = cal2.SelectionRange.Start.ToString("MM/dd/yyyy");                
            }
            else if (pnlCalendar2.Location == new Point(597, 404))
            {
                mskEndDate.Text = cal2.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            pnlCalendar2.Visible = false;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void cal2_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar2.Visible = false;
        }
        // MY 09/11/2015 - Start: End Date events    

        private void btnCloseDetails_Click(object sender, EventArgs e)
        {
            pnlGenEntry.Visible = false;
            AddEditMode(false);
            mskStartDate.Enabled = true;
            mskEndDate.Enabled = true;
            btnLoad2.Enabled = true;
            btnClose.Visible = true;
            btnCreate.Enabled = true;
        }

        private void txtCashReceipts1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}"); txtCashReceipts1.Text = Convert.ToDecimal(txtCashReceipts1.Text).ToString("#,##0.00");
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void txtChkBal1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void txtCashReceipts2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void txtChkBal2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}"); 
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void txtNewInvoice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}"); 
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void txtAdjNewInv_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}"); 
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)45;
        }   
        
        private void txtNewPay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}"); 
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void txtPayTaxes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}"); 
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void txtPayDeposit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void txt401K_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}"); 
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}"); 
            }
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DateTime startDate;
            DateTime endDate;

            startDate = Convert.ToDateTime(mskStartDate.Text);
            endDate = Convert.ToDateTime(mskEndDate.Text);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                GenerateEntryDays(date);
            }        
        }

        private void txtChkBal2_Leave(object sender, EventArgs e)
        {
            try
            {
                txtChkBal2.Text = Convert.ToDecimal(txtChkBal2.Text).ToString("#,##0.00");
            }
            catch { }
        }

        private void txtNewInvoice_Leave(object sender, EventArgs e)
        {
            try
            {
                txtNewInvoice.Text = Convert.ToDecimal(txtNewInvoice.Text).ToString("#,##0.00");
            }
            catch { }
        }

        private void txtAdjNewInv_Leave(object sender, EventArgs e)
        {
            try
            {
                txtAdjNewInv.Text = Convert.ToDecimal(txtAdjNewInv.Text).ToString("#,##0.00");
            }
            catch { }
        }

        private void txtNewPay_Leave(object sender, EventArgs e)
        {
            try
            {
                txtNewPay.Text = Convert.ToDecimal(txtNewPay.Text).ToString("#,##0.00");
            }
            catch { }
        }

        private void txtPayTaxes_Leave(object sender, EventArgs e)
        {
            try
            {
                txtPayTaxes.Text = Convert.ToDecimal(txtPayTaxes.Text).ToString("#,##0.00");
            }
            catch { }
        }

        private void txtPayDeposit_Leave(object sender, EventArgs e)
        {
            try
            {
                txtPayDeposit.Text = Convert.ToDecimal(txtPayDeposit.Text).ToString("#,##0.00");
            }
            catch { }
        }

        private void txt401K_Leave(object sender, EventArgs e)
        {
            try
            {
                txt401K.Text = Convert.ToDecimal(txt401K.Text).ToString("#,##0.00");
            }
            catch { }
        }

        private void txtCashReceipts2_Leave(object sender, EventArgs e)
        {
            try
            {
                txtCashReceipts2.Text = Convert.ToDecimal(txtCashReceipts2.Text).ToString("#,##0.00");
            }
            catch { }
        }

        private void cboMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLoad_Click(null, null);
        }
    }
}
