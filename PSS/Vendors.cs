//Vendors.cs
// AUTHOR       : MARIA YOUNES
// TITLE        : Senior Programmer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Vendors Master File Maintenance

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

namespace PSS
{
    public partial class Vendors : PSS.TemplateForm
    {
        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";

        DataTable dtVendorTypes = new DataTable();                                      // MY 08/14/2015 - Pop-up GridView Vendor Types query
        DataTable dtApprovalDesc = new DataTable();                                     // MY 08/12/2015 - Pop-up GridView Approver Desc query
        DataTable dtCountries = new DataTable();                                        // MY 12/30/2014 - Pop-up GridView Countries query

        public Vendors()
        {
            InitializeComponent();
            LoadRecords();
            LoadVendorTypes();
            LoadApprovalDesc();
            LoadCountries();
           
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
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Procurements.VendorMaster();

            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            FileAccess();
            DataGridSetting();
            nMode = 0;
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
        }

        private void FileAccess()
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Vendors");

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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = false;
            }
        }

        private void LoadApprovalDesc()
        {
            dgvApprovalDesc.DataSource = null;

            dtApprovalDesc = PSSClass.Procurements.VendorApprovalDesc();
            if (dtApprovalDesc == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvApprovalDesc.DataSource = dtApprovalDesc;
            StandardDGVSetting(dgvApprovalDesc);
            dgvApprovalDesc.Columns[0].Width = 105;
            dgvApprovalDesc.Columns[1].Visible = false;
        }

        private void LoadVendorTypes()
        {
            dgvVendorTypes.DataSource = null;

            dtVendorTypes = PSSClass.Procurements.VendorTypes();
            if (dtVendorTypes == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvVendorTypes.DataSource = dtVendorTypes;
            StandardDGVSetting(dgvVendorTypes);
            dgvVendorTypes.Columns[0].Width = 105;
            dgvVendorTypes.Columns[1].Visible = false;
        }

        private void LoadCountries()
        {
            dgvCountryNames.DataSource = null;

            dtCountries = PSSClass.Procurements.VendorMasterCountries();
            if (dtCountries == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCountryNames.DataSource = dtCountries;
            StandardDGVSetting(dgvCountryNames);
            dgvCountryNames.Columns[0].Width = 377;
            dgvCountryNames.Columns[1].Visible = false;
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
            ToolStripMenuItem[] items = new ToolStripMenuItem[3];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "VendorQualiForm";
            items[0].Text = "Vendor Qualification Form";
            items[0].Click += new EventHandler(PrintVendorQualiClickHandler);

            items[1] = new ToolStripMenuItem();
            items[1].Name = "VendorApprovalList";
            items[1].Text = "Approved Vendors List";
            items[1].Click += new EventHandler(PrintVendorApprovalListClickHandler);

            items[2] = new ToolStripMenuItem();
            items[2].Name = "VendorDisqualifiedList";
            items[2].Text = "Disqualified Vendors List";
            items[2].Click += new EventHandler(PrintVendorDisqualifiedListClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Procurements.VendorMaster();

            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during build-up of search items." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void DateBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "";
        }
        
        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (pnlCalendar.Location == new Point(582, 33))
            {
                mskQstnDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            else if (pnlCalendar.Location == new Point(435, 309))
            {
                mskISODate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            
            pnlCalendar.Visible = false;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
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
            if (dgvFile.Columns[strCol].Visible == true)
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = false;
            else
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = true;
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

        private void PrintVendorApprovalListClickHandler(object sender, EventArgs e)
        {
            VendorApprovalList rpt = new VendorApprovalList();         

            rpt.WindowState = FormWindowState.Maximized;

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void PrintVendorDisqualifiedListClickHandler(object sender, EventArgs e)
        {
            VendorDisqualifiedList rpt = new VendorDisqualifiedList();

            rpt.WindowState = FormWindowState.Maximized;

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void PrintVendorQualiClickHandler(object sender, EventArgs e)
        {
            VendorReports rpt = new VendorReports();

            rpt.WindowState = FormWindowState.Maximized;
            rpt.nVID = Convert.ToInt32(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["VendorID"].Value);

            try
            {
                rpt.Show();
            }
            catch { }
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
                    bsFile.Filter = "VendorID<>0";
                    PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                    dgvFile.Select();
                    if (pnlRecord.Visible == true)
                        LoadData();
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
                    dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                    tsbRefresh.Enabled = true;
                }
                catch { }
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            bsFile.Filter = "VendorID<>0";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            btnClose.Visible = true; btnClose.BringToFront();

            txtVendorID.Text = dgvFile.CurrentRow.Cells["VendorID"].Value.ToString();
            txtVendorName.Text = dgvFile.CurrentRow.Cells["VendorName"].Value.ToString();
            txtVendorTypeID.Text = dgvFile.CurrentRow.Cells["VendorTypeID"].Value.ToString();
            txtVendorTypeDesc.Text = dgvFile.CurrentRow.Cells["VendorTypeDesc"].Value.ToString();
            txtGBLAcctNo.Text = dgvFile.CurrentRow.Cells["GBLAcctNo"].Value.ToString();            
            txtEnteredBy.Text = dgvFile.CurrentRow.Cells["EnteredByName"].Value.ToString();
            txtAddress1.Text = dgvFile.CurrentRow.Cells["Address1"].Value.ToString();
            txtAddress2.Text = dgvFile.CurrentRow.Cells["Address2"].Value.ToString();
            txtCity.Text = dgvFile.CurrentRow.Cells["City"].Value.ToString();
            txtState.Text = dgvFile.CurrentRow.Cells["State"].Value.ToString();
            txtZipCode.Text = dgvFile.CurrentRow.Cells["ZipCode"].Value.ToString();
            txtCountryCode.Text = dgvFile.CurrentRow.Cells["CountryCode"].Value.ToString();
            txtCountry.Text = dgvFile.CurrentRow.Cells["CountryName"].Value.ToString();
            txtWorkPhone.Text = dgvFile.CurrentRow.Cells["WorkPhone"].Value.ToString();
            txtCell.Text = dgvFile.CurrentRow.Cells["CellPhone"].Value.ToString();
            txtFax.Text = dgvFile.CurrentRow.Cells["Fax"].Value.ToString();
            txtContact.Text = dgvFile.CurrentRow.Cells["ContactName"].Value.ToString();
            txtEmail.Text = dgvFile.CurrentRow.Cells["Email"].Value.ToString();
            txtWebsite.Text = dgvFile.CurrentRow.Cells["Website"].Value.ToString();
            txtPayTerms.Text = dgvFile.CurrentRow.Cells["PaymentTerms"].Value.ToString();
            txtApprovalDescID.Text = dgvFile.CurrentRow.Cells["ApprovalDescID"].Value.ToString();
            txtApprovalDesc.Text = dgvFile.CurrentRow.Cells["ApprovalDesc"].Value.ToString();
            txtNotes.Text = dgvFile.CurrentRow.Cells["Notes"].Value.ToString();
            if (dgvFile.CurrentRow.Cells["IsActive"].Value.ToString() == "True")
            {
                chkIsActive.Checked = true;
            }            

            if (dgvFile.CurrentRow.Cells["QuestionnaireDate"].Value.ToString() != "")
            {
                mskQstnDate.Text = dgvFile.CurrentRow.Cells["QuestionnaireDate"].Value.ToString();
            }
            else
            {
                mskQstnDate.Text = " / / ";
            }

            txtISONo.Text = dgvFile.CurrentRow.Cells["ISONo"].Value.ToString();

            if (dgvFile.CurrentRow.Cells["ISOExpDate"].Value.ToString() != "")
            {
                mskISODate.Text = dgvFile.CurrentRow.Cells["ISOExpDate"].Value.ToString();
            }
            else
            {
                mskISODate.Text = " / / ";
            }

            if (dgvFile.CurrentRow.Cells["DateCreated"].Value.ToString() != "")
            {
                mskEntryDate.Text = dgvFile.CurrentRow.Cells["DateCreated"].Value.ToString();
            }
            else
            {
                mskEntryDate.Text = " / / ";
            }
            
            if (dgvFile.CurrentRow.Cells["CriticalVendor"].Value.ToString() == "True")
                chkCritical.Checked = true;
            else
                chkCritical.Checked = false;

            txtVendorName.Focus();
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
            dgvFile.Columns["VendorID"].HeaderText = "Vendor ID";
            dgvFile.Columns["VendorName"].HeaderText = "Vendor Name";
            dgvFile.Columns["VendorTypeDesc"].HeaderText = "Vendor Type";
            dgvFile.Columns["GBLAcctNo"].HeaderText = "GBL Acct No";
            dgvFile.Columns["Address1"].HeaderText = "Address 1";
            dgvFile.Columns["Address2"].HeaderText = "Address 2";
            dgvFile.Columns["ZipCode"].HeaderText = "Zip Code";
            dgvFile.Columns["CountryCode"].HeaderText = "Country";     
            dgvFile.Columns["ContactName"].HeaderText = "Contact Name";
            dgvFile.Columns["Email"].HeaderText = "Email";
            dgvFile.Columns["Website"].HeaderText = "Website";
            dgvFile.Columns["PaymentTerms"].HeaderText = "Payment Terms";
            dgvFile.Columns["ApprovalDesc"].HeaderText = "Approval Code";
            dgvFile.Columns["Notes"].HeaderText = "Notes";
            dgvFile.Columns["QuestionnaireDate"].HeaderText = "Questionnaire Date";
            dgvFile.Columns["ISONo"].HeaderText = "ISO No";
            dgvFile.Columns["ISOExpDate"].HeaderText = "ISO Expiry Date";
            dgvFile.Columns["IsActive"].HeaderText = "Active";           
            dgvFile.Columns["DateCreated"].HeaderText = "Date Created";
            dgvFile.Columns["EnteredByName"].HeaderText = "Entered By";
            dgvFile.Columns["ApprovedByName"].HeaderText = "Approved By";
            dgvFile.Columns["ReapprovedByName"].HeaderText = "Reapproved By";
            dgvFile.Columns["CriticalVendor"].HeaderText = "Critical Vendor";
            dgvFile.Columns["VendorID"].Width = 50;
            dgvFile.Columns["VendorName"].Width = 200;
            dgvFile.Columns["VendorTypeDesc"].Width = 90;
            dgvFile.Columns["GBLAcctNo"].Width = 90;
            dgvFile.Columns["ContactName"].Width = 100;
            dgvFile.Columns["State"].Width = 40;
            dgvFile.Columns["CountryCode"].Width = 50;
            dgvFile.Columns["Email"].Width = 90;
            dgvFile.Columns["Website"].Width = 90;
            dgvFile.Columns["Notes"].Width = 70;
            dgvFile.Columns["PaymentTerms"].Width = 60;
            dgvFile.Columns["ApprovalDesc"].Width = 90;
            dgvFile.Columns["ISONo"].Width = 70;
            dgvFile.Columns["ISOExpDate"].Width = 70;
            dgvFile.Columns["IsActive"].Width = 50;
            dgvFile.Columns["EnteredByName"].Width = 90;
            dgvFile.Columns["ApprovedByName"].Width = 90;
            dgvFile.Columns["ReapprovedByName"].Width = 90;
            dgvFile.Columns["CriticalVendor"].Width = 80;
            dgvFile.Columns["ISOExpDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["VendorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ISONo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["GBLAcctNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            dgvFile.Columns["State"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["CountryCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ISONo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["VendorTypeID"].Visible = false;  
            dgvFile.Columns["CountryName"].Visible = false;
            dgvFile.Columns["WorkPhone"].Visible = false;
            dgvFile.Columns["CellPhone"].Visible = false;
            dgvFile.Columns["Fax"].Visible = false;
            dgvFile.Columns["EnteredByName"].Visible = false;
            dgvFile.Columns["ApprovalDescID"].Visible = false;  
            dgvFile.Columns["IsApproved"].Visible = false;
            dgvFile.Columns["ApprovedBy"].Visible = false;
            dgvFile.Columns["DateApproved"].Visible = false;
            dgvFile.Columns["IsReapproved"].Visible = false;
            dgvFile.Columns["ReapprovedBy"].Visible = false;
            dgvFile.Columns["DateReapproved"].Visible = false;
            dgvFile.Columns["ApprovedByName"].Visible = false;
            dgvFile.Columns["ReapprovedByName"].Visible = false;
            dgvFile.Columns["QuestionnaireDate"].Visible = false;
            dgvFile.Columns["DateCreated"].Visible = false;  
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

        //private void txtApproversIDEnterHandler(object sender, EventArgs e)
        //{
        //    dgvApprovers.Visible = false;
        //}

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            txtVendorID.Text = "< New >";
            txtVendorName.Focus();
            txtEnteredBy.Enabled = false;
            mskEntryDate.Enabled = false;
            mskEntryDate.Text = DateTime.Today.ToString("mm/dd/yyyy");
            chkIsActive.Checked = true;
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            txtVendorID.Enabled = false;
            txtEnteredBy.Enabled = false;
            mskEntryDate.Enabled = false;
            LoadData();          
            btnClose.Visible = false;
        }

        private void DeleteRecord()
        {
            LoadData();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                SqlCommand sqlcmd = new SqlCommand();

                sqlcmd.Connection = sqlcnn;
                
                sqlcmd.Parameters.AddWithValue("@VendorID", Convert.ToInt16(txtVendorID.Text));

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelVendorMaster";

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
        }

        private void SaveRecord()
        {
            if (txtVendorName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Vendor Name!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtVendorName.Focus();
                return;
            }

            if (nMode == 1)
                txtVendorID.Text = PSSClass.DataEntry.NewID("Vendors", "VendorID").ToString();
                
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@VendorID", txtVendorID.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@VendorName", txtVendorName.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@VendorTypeID", Convert.ToInt16(txtVendorTypeID.Text));
            sqlcmd.Parameters.AddWithValue("@GBLAcctNo", txtGBLAcctNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Address1", txtAddress1.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Address2", txtAddress2.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@City", txtCity.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@ZipCode", txtZipCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CountryCode", txtCountryCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@WorkPhone", txtWorkPhone.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CellPhone", txtCell.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Fax", txtFax.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@ContactName", txtContact.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@PaymentTerms", txtPayTerms.Text.Trim());
            if (txtApprovalDescID.Text == "")
                sqlcmd.Parameters.AddWithValue("@ApprovalDescID", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@ApprovalDescID", Convert.ToInt16(txtApprovalDescID.Text));
            sqlcmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@IsActive", Convert.ToBoolean(chkIsActive.CheckState));
            sqlcmd.Parameters.AddWithValue("@IsApproved", 1);
            //if (txtApproveID.Text.Trim() == "")
            //{
            //    sqlcmd.Parameters.AddWithValue("@ApprovedBy", DBNull.Value);
            //}
            //else
            //{
            //    sqlcmd.Parameters.AddWithValue("@ApprovedBy", Convert.ToInt16(txtApproveID.Text));
            //}
            //if (mskApprovalDate.MaskFull == false)           
            //{              
            //    sqlcmd.Parameters.AddWithValue("@DateApproved", DBNull.Value);
            //}
            //else
            //{               
            //    sqlcmd.Parameters.AddWithValue("@DateApproved", Convert.ToDateTime(mskApprovalDate.Text));
            //}

            //sqlcmd.Parameters.AddWithValue("@IsReapproved", Convert.ToBoolean(chkIsReapproved.CheckState));
            //if (txtReapproveID.Text.Trim() == "")
            //{
            //    sqlcmd.Parameters.AddWithValue("@ReapprovedBy", DBNull.Value);
            //}
            //else
            //{
            //    sqlcmd.Parameters.AddWithValue("@ReapprovedBy", Convert.ToInt16(txtReapproveID.Text));
            //}           
            
            //if (mskReapprovalDate.MaskFull == false)
            //{               
            //    sqlcmd.Parameters.AddWithValue("@DateReapproved", DBNull.Value);
            //}
            //else
            //{
            //    sqlcmd.Parameters.AddWithValue("@DateReapproved", Convert.ToDateTime(mskReapprovalDate.Text));
            //}
            
            if (mskQstnDate.MaskFull == false)
            {
                sqlcmd.Parameters.AddWithValue("@QuestionnaireDate", DBNull.Value);               
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@QuestionnaireDate", Convert.ToDateTime(mskQstnDate.Text));              
            }

            sqlcmd.Parameters.AddWithValue("@ISONo", txtISONo.Text.Trim());

            if (mskISODate.MaskFull == false)
            {
                sqlcmd.Parameters.AddWithValue("@ISOExpDate", DBNull.Value);                
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@ISOExpDate", Convert.ToDateTime(mskISODate.Text));
            }
            sqlcmd.Parameters.AddWithValue("@Critical", Convert.ToInt16(chkCritical.CheckState));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditVendorMaster";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return;
            }
            sqlcnn.Dispose();

            dgvFile.Refresh();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            LoadRecords();
            PSSClass.General.FindRecord("VendorID", txtVendorID.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            AddEditMode(false);
            MessageBox.Show("Record successfully saved!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void CancelSave()
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
            ClearControls(this);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
        }

        private void Vendors_Load(object sender, EventArgs e)
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
        }

        private void Vendors_KeyDown(object sender, KeyEventArgs e)
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
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
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

        // MY 08/14/2015 - START: txt/dgvVendorTypes events
        private void dgvVendorTypes_DoubleClick(object sender, EventArgs e)
        {
            txtVendorTypeDesc.Text = dgvVendorTypes.CurrentRow.Cells["VendorTypeDesc"].Value.ToString();
            txtVendorTypeID.Text = dgvVendorTypes.CurrentRow.Cells["VendorTypeID"].Value.ToString();
            dgvVendorTypes.Visible = false;
        }

        private void dgvVendorTypes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvVendorTypes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtVendorTypeDesc.Text = dgvVendorTypes.CurrentRow.Cells["VendorTypeDesc"].Value.ToString();
                txtVendorTypeID.Text = dgvVendorTypes.CurrentRow.Cells["VendorTypeID"].Value.ToString();
                dgvVendorTypes.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvVendorTypes.Visible = false;
            }
        }
        private void txtVendorTypeDesc_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvVendorTypes.Visible = true; dgvVendorTypes.BringToFront();
            }
        }

        private void dgvVendorTypes_Leave(object sender, EventArgs e)
        {
            dgvVendorTypes.Visible = false;
        }

        private void txtVendorTypeDesc_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwVendorTypes;
                dvwVendorTypes = new DataView(dtVendorTypes, "VendorTypeDesc like '%" + txtVendorTypeDesc.Text.Trim().Replace("'", "''") + "%'", "VendorTypeDesc", DataViewRowState.CurrentRows);
                dvwSetUp(dgvVendorTypes, dvwVendorTypes);
            }
        }

        private void dgvVendorTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtVendorTypeDesc.Text = dgvVendorTypes.CurrentRow.Cells["VendorTypeDesc"].Value.ToString();
            txtVendorTypeID.Text = dgvVendorTypes.CurrentRow.Cells["VendorTypeID"].Value.ToString();
            dgvVendorTypes.Visible = false;
        }

        private void picVendorTypes_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                LoadVendorTypes();
                dgvVendorTypes.Visible = true; dgvVendorTypes.BringToFront();
            }
        }

        private void txtVendorTypeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtVendorTypeDesc.Text = PSSClass.Procurements.VendorTypesByName(Convert.ToInt16(txtVendorTypeID.Text));
                if (txtVendorTypeDesc.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Vendor Type found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvVendorTypes.Visible = false;
            }
            else
            {
                txtVendorTypeDesc.Text = ""; dgvVendorTypes.Visible = false;
            }
        }

        // MY 08/14/2015 - END: txt/dgvVendorTypeDesc events

        // MY 08/12/2015 - START: txt/dgvApprovalDesc events
        private void dgvApprovalDesc_DoubleClick(object sender, EventArgs e)
        {
            txtApprovalDesc.Text = dgvApprovalDesc.CurrentRow.Cells["ApprovalDesc"].Value.ToString();
            txtApprovalDescID.Text = dgvApprovalDesc.CurrentRow.Cells["ApprovalDescID"].Value.ToString();
            dgvApprovalDesc.Visible = false;
        }

        private void dgvApprovalDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvApprovalDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtApprovalDesc.Text = dgvApprovalDesc.CurrentRow.Cells["ApprovalDesc"].Value.ToString();
                txtApprovalDescID.Text = dgvApprovalDesc.CurrentRow.Cells["ApprovalDescID"].Value.ToString();
                dgvApprovalDesc.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvApprovalDesc.Visible = false;
            }
        }
        private void txtApprovalDesc_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvApprovalDesc.Visible = true; dgvApprovalDesc.BringToFront();
            }
        }

        private void dgvApprovalDesc_Leave(object sender, EventArgs e)
        {
            dgvApprovalDesc.Visible = false;
        }

        private void txtApprovalDesc_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwApprovalDesc;
                dvwApprovalDesc = new DataView(dtApprovalDesc, "ApprovalDesc like '%" + txtApprovalDesc.Text.Trim().Replace("'", "''") + "%'", "ApprovalDesc", DataViewRowState.CurrentRows);
                dvwSetUp(dgvApprovalDesc, dvwApprovalDesc);
            }
        }

        private void dgvApprovalDesc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtApprovalDesc.Text = dgvApprovalDesc.CurrentRow.Cells["ApprovalDesc"].Value.ToString();
            txtApprovalDescID.Text = dgvApprovalDesc.CurrentRow.Cells["ApprovalDescID"].Value.ToString();
            dgvApprovalDesc.Visible = false;
        }

        private void picApprovalDesc_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                LoadApprovalDesc();
                dgvApprovalDesc.Visible = true; dgvApprovalDesc.BringToFront();
            }
        }

        private void txtApprovalDescID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtApprovalDesc.Text = PSSClass.Procurements.VendorApprovalDescByName(Convert.ToInt16(txtApprovalDescID.Text));
                if (txtApprovalDesc.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Approval Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvApprovalDesc.Visible = false;
            }
            else
            {
                txtApprovalDesc.Text = ""; dgvApprovalDesc.Visible = false;
            }
        }

        // MY 08/12/2015 - END: txt/dgvApprovalDesc events

        // MY 12/31/2014 - START: txt/dgvCountryNames events
        private void dgvCountryNames_DoubleClick(object sender, EventArgs e)
        {
            txtCountry.Text = dgvCountryNames.CurrentRow.Cells["CountryName"].Value.ToString();
            txtCountryCode.Text = dgvCountryNames.CurrentRow.Cells["CountryCode"].Value.ToString();
            dgvCountryNames.Visible = false;
        }

        private void dgvCountryNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCountryNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtCountry.Text = dgvCountryNames.CurrentRow.Cells["CountryName"].Value.ToString();
                txtCountryCode.Text = dgvCountryNames.CurrentRow.Cells["CountryCode"].Value.ToString();
                dgvCountryNames.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvCountryNames.Visible = true;
            }
        }
        private void txtCountry_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvCountryNames.Visible = true; dgvCountryNames.BringToFront();
            }
        }

        private void dgvCountryNames_Leave(object sender, EventArgs e)
        {
            dgvCountryNames.Visible = false;
        }

        private void txtCountry_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwCountries;
                dvwCountries= new DataView(dtCountries, "CountryName like '%" + txtCountry.Text.Trim().Replace("'", "''") + "%'", "CountryName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvCountryNames, dvwCountries);
            }
        }

        private void dgvCountryNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCountry.Text = dgvCountryNames.CurrentRow.Cells["CountryName"].Value.ToString();
            txtCountryCode.Text = dgvCountryNames.CurrentRow.Cells["CountryCode"].Value.ToString();
            dgvCountryNames.Visible = false;
        }

        private void picCountries_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                LoadCountries();
                dgvCountryNames.Visible = true; dgvCountryNames.BringToFront();
            }
        }

        private void txtCountryCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtCountry.Text = PSSClass.Procurements.VendorMasterCountryName(txtCountryCode.Text.Trim());
                if (txtCountry.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Country Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvCountryNames.Visible = false;
            }
            else
            {
                txtCountry.Text = ""; dgvCountryNames.Visible = false;
            }
        }

        // MY 12/31/2014 - END: txt/dgvCountryNames events

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

        private void mskISODate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.BringToFront(); pnlCalendar.Location = new Point(435, 309);
        }

        private void mskQstnDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.BringToFront(); pnlCalendar.Location = new Point(582, 33);
        }

        private void Vendors_Activated(object sender, EventArgs e)
        {
            FileAccess();
        }

        private void txtApprovalDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                dgvApprovalDesc.Visible = false;
            else
                txtApprovalDescID.Text = "";
        }                   
    }
}
