//DocumentMaster.cs
// AUTHOR       : MARIA YOUNES
// TITLE        : Senior Programmer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  :Document Master File Maintenance

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
    public partial class DocumentMaster : PSS.TemplateForm
    {
        private byte nMode = 0;
        private int nSw = 0;
        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";
        private string defaultPath =  @"M:\Corp Documents";
        private string userDept = "";
        private string fileDesc = "";

        DataTable dtDocTypes = new DataTable();                                        // MY 09/01/2015 - Pop-up GridView Doc Type query
        DataTable dtCompanies = new DataTable();                                       // MY 09/01/2015 - Pop-up GridView Company query


        public DocumentMaster()
        {
            InitializeComponent();

            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "DocumentsMaster");
            LoadRecords();
            LoadDocTypes();
            LoadDocSponsors();
            SetUserDept();
            DataGridSetting();
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
            tstbSearch.KeyPress += new KeyPressEventHandler(SearchKeyPressHandler);
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            DataTable dt = PSSClass.CustomerService.DocumentMaster();
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "DocTypeID <> 0";
            FileAccess();
            DataGridSetting();
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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; //tsbDelete.Enabled = true;
            }
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

        private void BuildPrintItems()
        {
            //ToolStripMenuItem[] items = new ToolStripMenuItem[2];

            //items[0] = new ToolStripMenuItem();
            //items[0].Name = "DepartmentCode";
            //items[0].Text = "Sorted by Department Code";
            //items[0].Click += new EventHandler(PrintDeptCodeClickHandler);

            //items[1] = new ToolStripMenuItem();
            //items[1].Name = "DepartmentName";
            //items[1].Text = "Sorted by Department Name";
            //items[1].Click += new EventHandler(PrintDeptNameClickHandler);

            //tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.CustomerService.DocumentMaster();
            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during build-up of search items." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void dgvCellChangedHandler(object sender, EventArgs e)
        {
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
        private void PrintDeptNameClickHandler(object sender, EventArgs e)
        {
            //RptDept rptDeptNameList = new RptDept();
            //rptDeptNameList.WindowState = FormWindowState.Maximized;
            //rptDeptNameList.rptName = "DeptName";
            //rptDeptNameList.rptLabel = "DEPARTMENTS REFERENCE LIST SORTED BY NAME";
            //rptDeptNameList.Show();
        }

        private void PrintDeptCodeClickHandler(object sender, EventArgs e)
        {

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
                    bsFile.Filter = "DocTypeID<>0";
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
        private void RefreshClickHandler(object sender, EventArgs e)
        {
            LoadRecords();
            tsbRefresh.Enabled = false;
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, false);
            nMode = 0;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtCmpyCode.Text = dgvFile.CurrentRow.Cells["CompanyCode"].Value.ToString();
            txtDocTypeID.Text = dgvFile.CurrentRow.Cells["DocTypeID"].Value.ToString();
            txtDocType.Text = dgvFile.CurrentRow.Cells["DocTypeName"].Value.ToString();
            txtDocNo.Text = dgvFile.CurrentRow.Cells["DocNo"].Value.ToString();
            mskDocDate.Text = dgvFile.CurrentRow.Cells["DocDate"].Value.ToString();
            txtCreatedBy.Text = dgvFile.CurrentRow.Cells["CreatedBy"].Value.ToString();
            txtDocDesc.Text = dgvFile.CurrentRow.Cells["DocDesc"].Value.ToString();
            txtDocPath.Text = dgvFile.CurrentRow.Cells["DocPath"].Value.ToString();
            txtCompanyID.Text = dgvFile.CurrentRow.Cells["CompanyID"].Value.ToString();
            txtCompanyName.Text = dgvFile.CurrentRow.Cells["CompanyName"].Value.ToString();
            txtContact.Text = dgvFile.CurrentRow.Cells["Contact"].Value.ToString(); 
            txtFrom.Text = dgvFile.CurrentRow.Cells["DocFrom"].Value.ToString();
            txtTo.Text = dgvFile.CurrentRow.Cells["DocTo"].Value.ToString();
            txtSubject.Text = dgvFile.CurrentRow.Cells["Subject"].Value.ToString();
            if (dgvFile.CurrentRow.Cells["TrackDoc"].Value.ToString() == "True")
                chkTrack.Checked = true;
            else
                chkTrack.Checked = false;
            if (txtDocNo.Text != "")
            {
                btnMakeWord.Enabled = true;
                btnViewWord.Enabled = true;                  
            }
        }

        private void LoadDocTypes()
        {
            dgvDocTypes.DataSource = null;

            dtDocTypes = PSSClass.CustomerService.DocumentTypes();
            if (dtDocTypes == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            if (dtDocTypes.Rows.Count == 0)
                return;

            dgvDocTypes.DataSource = dtDocTypes;
            StandardDGVSetting(dgvDocTypes);
            dgvDocTypes.Columns[0].Width = 88;
            dgvDocTypes.Columns[1].Visible = false;
        }

        private void LoadDocSponsors()
        {
            dgvCompanyNames.DataSource = null;

            dtCompanies = PSSClass.CustomerService.DocumentSponsors();
            if (dtCompanies == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            if (dtCompanies.Rows.Count == 0)
                return;

            dgvCompanyNames.DataSource = dtCompanies;
            StandardDGVSetting(dgvCompanyNames);
            dgvCompanyNames.Columns[0].Width = 357;
            dgvCompanyNames.Columns[1].Visible = false;
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
            dgvFile.Columns["CompanyCode"].HeaderText = "Cmpy Code";
            dgvFile.Columns["DocTypeName"].HeaderText = "Doc Type";
            dgvFile.Columns["DocNo"].HeaderText = "Doc Number";
            dgvFile.Columns["DocDate"].HeaderText = "Doc Date";
            dgvFile.Columns["DocDesc"].HeaderText = "Description";
            dgvFile.Columns["DocPath"].HeaderText = "Full Path";
            dgvFile.Columns["CreatedBy"].HeaderText = "Created By";
            dgvFile.Columns["DateCreated"].HeaderText = "Date Created";
            dgvFile.Columns["DocTypeName"].Width = 60;
            dgvFile.Columns["CompanyCode"].Width = 60;
            dgvFile.Columns["DocNo"].Width = 60;
            dgvFile.Columns["DocDate"].Width = 70;
            dgvFile.Columns["DocDesc"].Width = 250;
            dgvFile.Columns["CompanyName"].Width = 250;
            dgvFile.Columns["Contact"].Width = 250;
            dgvFile.Columns["DocFrom"].Width = 200;
            dgvFile.Columns["DocTo"].Width = 200;
            dgvFile.Columns["Subject"].Width = 200;
            dgvFile.Columns["CreatedByID"].Width = 70;
            dgvFile.Columns["DateCreated"].Width = 80;
            dgvFile.Columns["DocTypeID"].Visible = false;
            dgvFile.Columns["CompanyID"].Visible = false;  
            dgvFile.Columns["LastUpdate"].Visible = false;
            dgvFile.Columns["LastUserID"].Visible = false;
            dgvFile.Columns["CreatedByID"].Visible = false;
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["CreatedBy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["CompanyCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DocNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DocTypeName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["TrackDoc"].Visible = false;
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
            OpenControls(this.pnlRecord, false);
            LoadData();
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (nSw == 0)
            {
                nSw = 1;
                // timer1.Enabled = true;
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
                //nCtr = 0;
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
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            txtDocType.Focus();
            txtCmpyCode.Text = "P";
            txtDocNo.Text = "< New >";           
            mskDocDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            txtDocPath.Text = defaultPath;
            txtDocNo.Enabled = false;
            txtCreatedBy.Enabled = false;
            txtDocPath.Enabled = false;
            btnViewWord.Enabled = false;
            btnMakeWord.Enabled = false;           
        }

        private void EditRecord()
        { 
            LoadData();
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            txtDocDesc.Focus(); btnClose.Visible = false;
            txtDocTypeID.Enabled = false;
            txtDocType.Enabled = false;
            picDocType.Enabled = false;
            txtDocNo.Enabled = false;
            txtCreatedBy.Enabled = false;
            txtDocPath.Enabled = false;
            btnViewWord.Enabled = false;
            btnMakeWord.Enabled = false;     
        }

        private void DeleteRecord()
        {
        }

        private void SaveRecord()
        {
            if (txtDocTypeID.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Document Type!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDocTypeID.Focus();
                return;
            }

            if (txtDocTypeID.Text.Trim() == "1")
            {
                if (txtDocDesc.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter a Description!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtDocDesc.Focus();
                    return;
                }
                if (txtCompanyName.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Company Name!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCompanyName.Focus();
                    return;
                }
                if (txtContact.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Contact Name(s)!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtDocDesc.Focus();
                    return;
                }
            }
            else
            {
                if (txtFrom.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter From info!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtFrom.Focus();
                    return;
                }

                if (txtTo.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter To info!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtTo.Focus();
                    return;
                }
                if (txtSubject.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Subject!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtSubject.Focus();
                    return;
                }
            }

            if (nMode == 1)
                txtDocNo.Text = PSSClass.General.NewDocNo("DocumentMaster", "DocNo").ToString();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
            sqlcmd.Parameters.AddWithValue("@DocNo", Convert.ToInt16(txtDocNo.Text));
            sqlcmd.Parameters.AddWithValue("@DocTypeID", Convert.ToInt16(txtDocTypeID.Text));
            sqlcmd.Parameters.AddWithValue("@DocDate", Convert.ToDateTime(mskDocDate.Text));
            sqlcmd.Parameters.AddWithValue("@DocDesc", txtDocDesc.Text);
            sqlcmd.Parameters.AddWithValue("@DocPath", txtDocPath.Text);
            sqlcmd.Parameters.AddWithValue("@CompanyID", txtCompanyID.Text);
            sqlcmd.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text);
            sqlcmd.Parameters.AddWithValue("@Contact", txtContact.Text);
            sqlcmd.Parameters.AddWithValue("@From", txtFrom.Text);
            sqlcmd.Parameters.AddWithValue("@To", txtTo.Text);
            sqlcmd.Parameters.AddWithValue("@Subject", txtSubject.Text);
            sqlcmd.Parameters.AddWithValue("@TrackDoc", Convert.ToInt16(chkTrack.CheckState));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditDocumentMaster";
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
            dgvFile.Refresh();
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("DocNo", txtDocNo.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            LoadData();
            btnClose.Visible = true;
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
            AddEditMode(false);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            nMode = 0;
        }

        private void CreateDocument()
        {
            try
            {
                //Create an instance for word app
                Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

                //Set animation status for word application
                //winword.ShowAnimation = false;

                //Set status for word application is to be visible or not.
                winword.Visible = false;

                //Create a missing variable for missing value
                object missing = System.Reflection.Missing.Value;

                //Create a new document
                Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

                //string docType = "";
                //string fileDesc = "";

                //docType = txtDocType.Text;

                //fileDesc = defaultPath + @"\" + txtDocType.Text + "s" + @"\" + userDept + @"\" + docType.Substring(0, 1) + txtDocNo.Text + ".docx";

                SetFileDesc();

                //Add the footers into the document
                foreach (Microsoft.Office.Interop.Word.Section wordSection in document.Sections)
                {

                    //Get the footer range and add the footer details.
                    Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 10;
                    footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Text = fileDesc;
                }

                object filename = @fileDesc;
                
                document.SaveAs(ref filename);
                ((Microsoft.Office.Interop.Word._Document)document).Close(ref missing, ref missing, ref missing);
                document = null;
                ((Microsoft.Office.Interop.Word._Application)winword).Quit(ref missing, ref missing, ref missing);
                winword = null;
                MessageBox.Show("Document created successfully!");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetFileDesc()
        {
            string docType = "";
            
            docType = txtDocType.Text;

            //fileDesc = defaultPath + @"\" + txtDocType.Text + "s" + @"\" + userDept + @"\" + docType.Substring(0, 1) + txtDocNo.Text + ".docx";

            //Revised as per JM 3/14/2016
            fileDesc = defaultPath + @"\" + txtDocType.Text + "s" + @"\" + docType.Substring(0, 1) + txtDocNo.Text + ".docx";
        }

        private void SetFullPath()
        {
            txtDocPath.Text = defaultPath + @"\" + txtDocType.Text + "s";
        }

        private void SetUserDept()
        {
            userDept = PSSClass.Users.UserDeptName(LogIn.nUserID);
        }

        private void UpdatePath()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@DocNo", Convert.ToInt16(txtDocNo.Text));
            sqlcmd.Parameters.AddWithValue("@DocPath", fileDesc);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdateDocumentPath";
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
        }

        private void DocumentMaster_Load(object sender, EventArgs e)
        {
            pnlRecord.Visible = false;          

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();           
        }

        private void DocumentMaster_KeyDown(object sender, KeyEventArgs e)
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
            nMode = 0;
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
        }

        // MY 09/01/2015 - START: txt/dgvDocTypes events
        private void dgvDocTypes_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtDocType.Text = dgvDocTypes.CurrentRow.Cells[0].Value.ToString();
                txtDocTypeID.Text = dgvDocTypes.CurrentRow.Cells[1].Value.ToString();
                dgvDocTypes.Visible = false;
                SetFullPath();

                if (txtDocType.Text == "Letter")
                {
                    txtDocDesc.Focus();
                }
                else
                {
                    txtFrom.Focus();
                }
            }
        }

        private void dgvDocTypes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvDocTypes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtDocType.Text = dgvDocTypes.CurrentRow.Cells[0].Value.ToString();
                    txtDocTypeID.Text = dgvDocTypes.CurrentRow.Cells[1].Value.ToString();
                    dgvDocTypes.Visible = false;
                    SetFullPath();
                }
                else if (e.KeyChar == 27)
                {
                    dgvDocTypes.Visible = false;
                }
            }
        }
        private void txtDocType_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvDocTypes.Visible = true; dgvDocTypes.BringToFront();
            }
        }

        private void dgvDocTypes_Leave(object sender, EventArgs e)
        {
            dgvDocTypes.Visible = false;
        }

        private void txtDocType_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwCatOrderTypes;
                dvwCatOrderTypes = new DataView(dtDocTypes, "DocTypeName like '%" + txtDocType.Text.Trim().Replace("'", "''") + "%'", "DocTypeName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvDocTypes, dvwCatOrderTypes);
                dgvDocTypes.Columns[0].Width = 88;
            }
        }

        private void dgvDocTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtDocType.Text = dgvDocTypes.CurrentRow.Cells[0].Value.ToString();
                txtDocTypeID.Text = dgvDocTypes.CurrentRow.Cells[1].Value.ToString();
                dgvDocTypes.Visible = false;
                SetFullPath();
            }
        }

        private void picDocTypes_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadDocTypes();
                dgvDocTypes.Visible = true; dgvDocTypes.BringToFront();
            }
        }

        private void txtDocTypeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtDocType.Text = PSSClass.CustomerService.DocumentTypeName(Convert.ToInt16(txtDocTypeID.Text));

                    if (txtDocType.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Document Type found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvDocTypes.Visible = false;
                    SetFullPath();
                }
                else
                {
                    txtDocType.Text = ""; dgvDocTypes.Visible = false;
                }
            }
        }            
        // MY 09/01/2015 - END: txt/dgvDocTypes events

        // MY 09/02/2015 - START: txt/dgvCompanies events
        private void dgvCompanyNames_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtCompanyName.Text = dgvCompanyNames.CurrentRow.Cells[0].Value.ToString();
                txtCompanyID.Text = dgvCompanyNames.CurrentRow.Cells[1].Value.ToString();
                dgvCompanyNames.Visible = false;
            }
        }

        private void dgvCompanyNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCompanyNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCompanyName.Text = dgvCompanyNames.CurrentRow.Cells[0].Value.ToString();
                    txtCompanyID.Text = dgvCompanyNames.CurrentRow.Cells[1].Value.ToString();
                    dgvCompanyNames.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvDocTypes.Visible = false;
                }
            }
        }

        private void txtCompanyName_Leave(object sender, EventArgs e)
        {
            if (txtCompanyName.Text.Trim() == "")
            {
                txtCompanyID.Text = "";
            }
        }

        private void dgvCompanyNames_Leave(object sender, EventArgs e)
        {
            dgvCompanyNames.Visible = false;
        }
       
        private void dgvCompanyNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtCompanyName.Text = dgvCompanyNames.CurrentRow.Cells[0].Value.ToString();
                txtCompanyID.Text = dgvCompanyNames.CurrentRow.Cells[1].Value.ToString();
                dgvCompanyNames.Visible = false;
            }
        }

        private void picCompanies_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadDocSponsors();
                dgvCompanyNames.Visible = true; dgvCompanyNames.BringToFront();
            }
        }

        private void txtCompanyID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCompanyName.Text = PSSClass.CustomerService.DocumentSponsorName(Convert.ToInt16(txtCompanyID.Text));

                    if (txtCompanyName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Sponsor found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvCompanyNames.Visible = false;
                }
                else
                {
                    txtCompanyName.Text = ""; dgvCompanyNames.Visible = false;
                }
            }
        }
        // MY 09/02/2015 - END: txt/dgvCompanies events

        private void btnMakeWord_Click(object sender, EventArgs e)
        {
            CreateDocument();
            UpdatePath();
            dgvFile.Refresh();
            LoadRecords();
            PSSClass.General.FindRecord("DocNo", txtDocNo.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            LoadData();
            btnViewWord.Enabled = true;
        }

        private void mskDocDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; //pnlCalendar.Location = new Point(306, 83); 
            pnlCalendar.BringToFront();
        }

        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {           
            mskDocDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");   
            pnlCalendar.Visible = false;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void btnViewWord_Click(object sender, EventArgs e)
        {
            if (txtDocPath.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Path!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDocPath.Focus();
                return;
            }
           
            try
            {
                System.Diagnostics.Process.Start(@txtDocPath.Text);
            }
            catch 
            {
                MessageBox.Show("File not found!" + Environment.NewLine + "Please create document first.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
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

        private void txtCompanyName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwCompanies;
                dvwCompanies = new DataView(dtCompanies, "sponsorName like '" + txtCompanyName.Text.Trim().Replace("'", "''") + "%'", "sponsorName", DataViewRowState.CurrentRows);
                PSSClass.General.DGVSetUp(dgvCompanyNames, dvwCompanies, 360);
            }
        }

        private void txtCompanyName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvCompanyNames.Visible = true; dgvCompanyNames.BringToFront();
            }
        }
    }
}
