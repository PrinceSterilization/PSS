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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

namespace PSS
{
    public partial class InvoiceExport : PSS.TemplateForm
    {
        byte nMode = 0;

        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";

        DataTable dtInvMaster = new DataTable();                                         // MY 04/24/2015 - GridView Invoice Master table from New GIS
        DataTable dtInvDetails = new DataTable();                                        // MY 04/24/2015 - GridView Regular Invoice Detail table from New GIS
        DataTable dtOldInvDetails = new DataTable();                                     // MY 09/09/2015 - GridView Regular Invoice Detail table from Old GIS
        DataTable dtOldNSDetails = new DataTable();                                      // MY 09/14/2015 - GridView NS Invoice Detail table from Old GIS


        public InvoiceExport()
        {
            InitializeComponent();
            BuildPrintItems();

            tsbExit.Click += new EventHandler(CloseClickHandler);
            tsbSearch.Click += new EventHandler(SearchOKClickHandler);
            tsbFilter.Click += new EventHandler(SearchFilterClickHandler);
            tsbRefresh.Click += new EventHandler(RefreshClickHandler);
            tsbAdd.Enabled = false;
            tsbEdit.Enabled = false;
            tsbDelete.Enabled = false;
            tsbCancel.Enabled = false;

            GetDefaultDates();
           
            LoadInvMaster(Convert.ToDateTime(mskStartDate.Text),Convert.ToDateTime(mskEndDate.Text));
        }      

        private void GetDefaultDates()
        {
            mskStartDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            mskEndDate.Text = DateTime.Now.ToString("MM/dd/yyyy");          
        }

        private void LoadInvMaster(DateTime cStartDate, DateTime cEndDate)
        {
            dtInvMaster = PSSClass.ACCPAC.InvExpMaster(cStartDate, cEndDate);

            if (dtInvMaster == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }

            bsFile.DataSource = dtInvMaster;
            bnFile.BindingSource = bsFile;
            dgvInvMaster.DataSource = bsFile;   

            DataGridSetting();            
        }

        private void FileAccess()
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "InvDataExport");

            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsbDelete.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = false;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = true;
            }
        }

        private void LoadInvDetails(DateTime cStartDate, DateTime cEndDate, Int64 cInvNo)
        {
            dtInvDetails = PSSClass.ACCPAC.InvExpDetails(cStartDate, cEndDate, cInvNo);

            if (dtInvDetails == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            dgvInvDetails.DataSource = dtInvDetails;          
        }       

        private void LoadOldInvDetails(DateTime cStartDate, DateTime cEndDate)
        {
            dtOldInvDetails = PSSClass.ACCPAC.InvExpDetails2(cStartDate, cEndDate);

            if (dtOldInvDetails == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            dgvOldInvDetails.DataSource = dtOldInvDetails;
        }

        private void LoadOldNSDetails(DateTime cStartDate, DateTime cEndDate)
        {
            dtOldNSDetails = PSSClass.ACCPAC.InvExpDetails3(cStartDate, cEndDate);

            if (dtOldNSDetails == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            dgvOldNSDetails.DataSource = dtOldNSDetails;
        }

        private static void CreateCSVFile(DataTable cDT, string strFileName)
        {
            try
            {
                //string strFilePath = @"\\PSAPP01\IT Files\ACCPAC\" + strFileName;
                string strFilePath = @"M:\Accounting\ACCPAC CSV Export\" + strFileName;
                
                StreamWriter sw = new StreamWriter(strFilePath, false);

                // Create Column Headers
                for (int i = 0; i < cDT.Columns.Count; i++)
                {
                    if (!(i == 1 || i == 5 || i == 7))
                    {
                        sw.Write(cDT.Columns[i]);
                        if (i < cDT.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                }
                sw.Write(sw.NewLine);

                foreach (DataRow dr in cDT.Rows)
                {
                    for (int i = 0; i < cDT.Columns.Count; i++)
                    {

                        if (!(i == 1 || i == 5 || i == 7))
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                string value = "";
                                string formattedValue = "";

                                value = dr[i].ToString();
                                switch (i)
                                {
                                    case 4:
                                        DateTime dt = Convert.ToDateTime(value);
                                        formattedValue = dt.ToString("yyyyMMdd");
                                        break;
                                    default:
                                        if (value.Contains(','))
                                        {
                                            formattedValue = String.Format("\"{0}\"", value);
                                        }
                                        else
                                        {
                                            formattedValue = value;
                                        }
                                        break;
                                }

                                sw.Write(formattedValue);
                            }
                            if (i < cDT.Columns.Count - 1)
                            {
                                sw.Write(",");
                            }
                        }
                       
                    }
                    sw.Write(sw.NewLine);
                
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        
        private void GetTotalInvAmount()
        {
            decimal totInvAmount = 0;

            for (int j = 0; j < dgvInvMaster.Rows.Count; j++)
            {
                totInvAmount = totInvAmount + Convert.ToDecimal(dgvInvMaster.Rows[j].Cells["INVTOTAL"].Value.ToString());
            }
            txtTotInvAmt.Text = totInvAmount.ToString("C");           
        }

        private void GetTotalOldInvAmount()
        {
            decimal totInvAmount = 0;
            int totInvoiceCount = 0;
            string strInvNo = "";

            strInvNo = dgvOldInvDetails.Rows[0].Cells["IDINVC"].Value.ToString();

            if (dgvOldInvDetails.Rows.Count > 0)
            {
                totInvoiceCount = 1;
            }

            for (int j = 0; j < dgvOldInvDetails.Rows.Count; j++)
            {
                if (strInvNo != dgvOldInvDetails.Rows[j].Cells["IDINVC"].Value.ToString())
                {
                    strInvNo = dgvOldInvDetails.Rows[j].Cells["IDINVC"].Value.ToString();
                    totInvoiceCount = totInvoiceCount + 1;
                }

                totInvAmount = totInvAmount + Convert.ToDecimal(dgvOldInvDetails.Rows[j].Cells["AMTEXTN"].Value.ToString());
            }
            txtTotInvAmt.Text = totInvAmount.ToString("C");
            txtTotInvCount.Text = Convert.ToString(totInvoiceCount);            
        }

        private void GetTotalNSInvAmount()
        {
            decimal totInvAmount = 0;
            int totInvoiceCount = 0;
            string strInvNo = "";

            strInvNo = dgvOldNSDetails.Rows[0].Cells["IDINVC"].Value.ToString();

            if (dgvOldNSDetails.Rows.Count > 0)
            {
                totInvoiceCount = 1;
            }

            for (int j = 0; j < dgvOldNSDetails.Rows.Count; j++)
            {
                if (strInvNo != dgvOldNSDetails.Rows[j].Cells["IDINVC"].Value.ToString())
                {
                    strInvNo = dgvOldNSDetails.Rows[j].Cells["IDINVC"].Value.ToString();
                    totInvoiceCount = totInvoiceCount + 1;
                }

                totInvAmount = totInvAmount + Convert.ToDecimal(dgvOldNSDetails.Rows[j].Cells["AMTEXTN"].Value.ToString());
            }
            txtTotInvAmt.Text = totInvAmount.ToString("C");
            txtTotInvCount.Text = Convert.ToString(totInvoiceCount);
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
            items[0].Name = "InvoiceExportDetails";
            items[0].Text = "Invoice Export Details";
            items[0].Click += new EventHandler(PrintInvExportDetailsClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Quotations.QuoteFollowUp(0);

            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered during build-up of search items." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            arrCol = new string[dt.Columns.Count];

            ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length];

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

        private void PrintInvExportDetailsClickHandler(object sender, EventArgs e)
        {
            InvoiceExportRpt rpt = new InvoiceExportRpt();

            rpt.invStartDate = Convert.ToDateTime(mskStartDate.Text);
            rpt.invEndDate   = Convert.ToDateTime(mskEndDate.Text);

            rpt.WindowState = FormWindowState.Maximized;

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {          
        }               

        private void DataGridSetting()
        {
            dgvInvMaster.EnableHeadersVisualStyles = false;
            dgvInvMaster.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvMaster.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvInvMaster.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInvMaster.Columns["SPONSOR"].HeaderText = "Sponsor Name";           
            dgvInvMaster.Columns["IDINVC"].HeaderText = "Invoice No";          
            dgvInvMaster.Columns["DATEINVC"].HeaderText = "Date Invoiced";
            dgvInvMaster.Columns["DATEEMAILED"].HeaderText = "Date Emailed";
            dgvInvMaster.Columns["INVNOTES"].HeaderText = "Notes";
            dgvInvMaster.Columns["INVTOTAL"].HeaderText = "Invoice Total";   
            dgvInvMaster.Columns["SPONSOR"].Width = 300;           
            dgvInvMaster.Columns["IDINVC"].Width = 80;           
            dgvInvMaster.Columns["DATEINVC"].Width = 73;
            dgvInvMaster.Columns["DATEEMAILED"].Width = 73;
            dgvInvMaster.Columns["INVNOTES"].Width = 150; 
            dgvInvMaster.Columns["INVTOTAL"].Width = 90;
            dgvInvMaster.Columns["DATEINVC"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvInvMaster.Columns["DATEINVC"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvInvMaster.Columns["DATEEMAILED"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;  
            dgvInvMaster.Columns["INVTOTAL"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvInvMaster.Columns["INVTOTAL"].DefaultCellStyle.Format = "N2"; 
            dgvInvMaster.Columns["CUSTID"].Visible = false;
        }

        private void DataGridInvDetailsSetting()
        {
            dgvInvDetails.EnableHeadersVisualStyles = false;
            dgvInvDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvInvDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvInvDetails.Columns["IDITEM"].HeaderText = "Service Code";
            dgvInvDetails.Columns["IDITEMDESC"].HeaderText = "Service Description";    
            dgvInvDetails.Columns["DISTCODE"].HeaderText = "Dist Code";        
            dgvInvDetails.Columns["QTYINVC"].HeaderText = "Qty";
            dgvInvDetails.Columns["AMTPRIC"].HeaderText = "Unit Price";
            dgvInvDetails.Columns["AMTEXTN"].HeaderText = "Amount Total";
            dgvInvDetails.Columns["IDITEM"].Width = 62;
            dgvInvDetails.Columns["IDITEMDESC"].Width = 118;   
            dgvInvDetails.Columns["DISTCODE"].Width = 50;
            dgvInvDetails.Columns["QTYINVC"].Width = 40;
            dgvInvDetails.Columns["AMTPRIC"].Width = 90;
            dgvInvDetails.Columns["AMTEXTN"].Width = 90;
            dgvInvDetails.Columns["QTYINVC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;  
            dgvInvDetails.Columns["AMTPRIC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvInvDetails.Columns["AMTPRIC"].DefaultCellStyle.Format = "N2";
            dgvInvDetails.Columns["AMTEXTN"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvInvDetails.Columns["AMTEXTN"].DefaultCellStyle.Format = "N2";
            dgvInvDetails.Columns["CUSTID"].Visible = false;
            dgvInvDetails.Columns["SPONSOR"].Visible = false;
            dgvInvDetails.Columns["CUSTPO"].Visible = false;
            dgvInvDetails.Columns["IDINVC"].Visible = false;         
            dgvInvDetails.Columns["DATEINVC"].Visible = false;
            dgvInvDetails.Columns["DATEEMAILED"].Visible = false;            
        }    

        private void InvoiceExport_Load(object sender, EventArgs e)
        {
            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }
            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
            tsbAdd.Enabled = false;
            tsbEdit.Enabled = false;
            tsbDelete.Enabled = false;
            tsbCancel.Enabled = false; 
        }      

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();            
            this.Close(); this.Dispose();
        }
       
        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (mskStartDate.MaskFull == false)
            {
                MessageBox.Show("Start Date is empty!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mskStartDate.Focus();
                return;
            }

            if (mskEndDate.MaskFull == false)
            {
                MessageBox.Show("End Date is empty!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mskEndDate.Focus();
                return;
            }

            int result = DateTime.Compare(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text));

            if (result > 0)
            {
                MessageBox.Show("Start Date is greater than End Date!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mskStartDate.Focus();
                return;
            }

            LoadInvMaster(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text));

            if (dtInvMaster.Rows.Count == 0)
            {
                MessageBox.Show("No Invoice records found for this date range. Pls. try again!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            txtTotInvCount.Text = "";
            txtTotInvAmt.Text = "";

            DataGridSetting();      
           
        }       

        private void btnExport_Click(object sender, EventArgs e)
        {
            pnlInvDetails.Visible = false;

            if (dgvInvMaster.Rows.Count == 0)
            {
                MessageBox.Show("Please load invoices first!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                btnLoad.Focus();
                return;
            }
 
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Are you ready to export this Invoice file to ACCPAC?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                string strExpFileName = "";
                DateTime fileDate = Convert.ToDateTime(mskStartDate.Text);
                
                strExpFileName = "PSSInvExport_" + fileDate.ToString("yyyyMMdd") + ".csv";
                LoadInvDetails(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text),0);

                if (dgvInvDetails.RowCount > 0)
                {
                    CreateCSVFile(dtInvDetails, strExpFileName);
                    txtTotInvCount.Text = Convert.ToString(dgvInvMaster.RowCount);
                    GetTotalInvAmount();
                    MessageBox.Show("ACCPAC export file successfully created!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("No records to process!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnExportOld_Click(object sender, EventArgs e)
        {
            pnlInvDetails.Visible = false;                     

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Are you ready to export the regular invoices from the Old GIS to ACCPAC?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                string strExpFileName = "";

                DateTime fileDate = Convert.ToDateTime(mskStartDate.Text);

                strExpFileName = "PSSInvExport_Old_" + fileDate.ToString("yyyyMMdd") + ".csv";
                LoadOldInvDetails(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text));

                if (dgvOldInvDetails.RowCount > 0)
                {
                    CreateCSVFile(dtOldInvDetails, strExpFileName);

                    GetTotalOldInvAmount();
                    MessageBox.Show("ACCPAC file for Old GIS Invoices successfully created!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("No records to process!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
        }

        private void btnExportNS_Click(object sender, EventArgs e)
        {
            pnlInvDetails.Visible = false;

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Are you ready to export the NS invoices from the Old GIS to ACCPAC?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                string strExpFileName = "";

                DateTime fileDate = Convert.ToDateTime(mskStartDate.Text);

                strExpFileName = "PSSInvExport_NS_" + fileDate.ToString("yyyyMMdd") + ".csv";
                LoadOldNSDetails(Convert.ToDateTime(mskStartDate.Text), Convert.ToDateTime(mskEndDate.Text));

                if (dgvOldNSDetails.RowCount > 0)
                {
                    CreateCSVFile(dtOldNSDetails, strExpFileName);
                    txtTotInvCount.Text = Convert.ToString(dgvOldNSDetails.RowCount);
                    GetTotalNSInvAmount();
                    MessageBox.Show("ACCPAC export file for NS Invoices successfully created!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show("No records to process!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }       
     
        // MY 05/06/2015 - Start: Date events          
        private void mskStartDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(410, 40);
        }

        private void mskEndDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(410, 76);
        }   

        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (pnlCalendar.Location == new Point(410, 40))
            {
                mskStartDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
                mskEndDate.Text   = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            else if (pnlCalendar.Location == new Point(410, 76))
            {
                mskEndDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            
            pnlCalendar.Visible = false;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }
        // MY 05/06/2015 - Start: End Date events     

        private void btnCloseDetails_Click(object sender, EventArgs e)
        {
            pnlInvDetails.Visible = false;
        }       

        private void dgvInvMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadInvDetails(Convert.ToDateTime(mskStartDate.Text),
                         Convert.ToDateTime(mskEndDate.Text),
                         Convert.ToInt64(dgvInvMaster.CurrentRow.Cells["IDINVC"].Value.ToString()));

            DataGridInvDetailsSetting();
            pnlInvDetails.Visible = true;

            decimal totInvTotal = 0;

            dgvInvDetails.Rows[0].Selected = true;
            totInvTotal = Convert.ToDecimal(dgvInvMaster.CurrentRow.Cells["INVTOTAL"].Value.ToString());
            txtDetPONo.Text = dgvInvDetails.CurrentRow.Cells["CUSTPO"].Value.ToString().TrimStart();
            txtDetInvNo.Text = dgvInvDetails.CurrentRow.Cells["IDINVC"].Value.ToString().TrimStart();
            txtDetInvDate.Text = dgvInvDetails.CurrentRow.Cells["DATEINVC"].Value.ToString();
            txtDetInvTotal.Text = totInvTotal.ToString("$##,###0.00");
        }

        private void InvoiceExport_Activated(object sender, EventArgs e)
        {
            FileAccess();
        }        
        
    }
}