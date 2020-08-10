//Catalog.cs
// AUTHOR       : MARIA YOUNES
// TITLE        : Senior Programmer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Catalog Master File Maintenance

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
    public partial class Catalog : PSS.TemplateForm
    {
        public string strPRVendorID;
        public string strPRVendorName;
        public string strPRCatNameID;
        public string strPRCatName;

        public byte nCatMasterSw;

        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";

        DataTable dtVendors = new DataTable();                                              // MY 07/30/2015 - Pop-up GridView Vendors query
        DataTable dtCatNames = new DataTable();                                             // MY 07/31/2015 - Pop-up GridView Catalog Names query 
        DataTable dtCatGrades = new DataTable();                                            // MY 08/03/2015 - Pop-up GridView Catalog Grades query    

        public Catalog()                                    
                   
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
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Procurements.CatalogMaster();

            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            FileAccess();
            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

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
            //ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            //items[0] = new ToolStripMenuItem();
            //items[0].Name = "ControlOfNonConformingTesting";
            //items[0].Text = "Control of Non-Conforming Testing Sheet";
            //items[0].Click += new EventHandler(PrintNCTestingSheetClickHandler);

            //tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Procurements.CatalogMaster();

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
                    bsFile.Filter = "CatalogNo<>''";
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
            bsFile.Filter = "CatalogNo<>''";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            btnClose.Visible = true; btnClose.BringToFront();

            txtVendorID.Text = dgvFile.CurrentRow.Cells["VendorID"].Value.ToString();
            txtVendorName.Text = dgvFile.CurrentRow.Cells["VendorName"].Value.ToString();
            txtCatalogNo.Text = dgvFile.CurrentRow.Cells["CatalogNo"].Value.ToString();
            txtCatNameID.Text = dgvFile.CurrentRow.Cells["CatalogNameID"].Value.ToString();
            txtCatalogName.Text = dgvFile.CurrentRow.Cells["CatalogName"].Value.ToString();
            txtCatDesc.Text = dgvFile.CurrentRow.Cells["CatalogDesc"].Value.ToString();
            txtGradeID.Text = dgvFile.CurrentRow.Cells["GradeID"].Value.ToString();
            txtGrade.Text = dgvFile.CurrentRow.Cells["Grade"].Value.ToString();            
            txtUnitPrice.Text = dgvFile.CurrentRow.Cells["UnitPrice"].Value.ToString();
            txtWebsite.Text = dgvFile.CurrentRow.Cells["Website"].Value.ToString();

            if (dgvFile.CurrentRow.Cells["IsActive"].Value.ToString() == "True")
            {
                chkIsActive.Checked = true;
            }
            txtGradeID.Focus();
        }

        private void LoadVendors()
        {
            dtVendors = PSSClass.Procurements.CatalogVendors();
            if (dtVendors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvVendorNames.DataSource = dtVendors;
            StandardDGVSetting(dgvVendorNames);
            dgvVendorNames.Columns[0].Width = 377;
            dgvVendorNames.Columns[1].Visible = false;                                                              // Vendor ID           
        }

        private void LoadCatNames()
        {
            dtCatNames = PSSClass.Procurements.CatalogNames();
            if (dtCatNames == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCatNames.DataSource = dtCatNames;
            StandardDGVSetting(dgvCatNames);
            dgvCatNames.Columns[0].Width = 377;
            dgvCatNames.Columns[1].Visible = false;   
        }

        private void LoadCatGrades()
        {
            dtCatGrades = PSSClass.Procurements.CatalogGrades();
            if (dtCatGrades == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCatGrades.DataSource = dtCatGrades;
            StandardDGVSetting(dgvCatGrades);
            dgvCatGrades.Columns[0].Width = 152;
            dgvCatGrades.Columns[1].Visible = false;
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
            dgvFile.Columns["CatalogNo"].HeaderText = "Catalog No";
            dgvFile.Columns["CatalogName"].HeaderText = "Name";
            dgvFile.Columns["CatalogDesc"].HeaderText = "Description";
            dgvFile.Columns["Grade"].HeaderText = "Grade";     
            dgvFile.Columns["UnitPrice"].HeaderText = "Unit Price";
            dgvFile.Columns["IsActive"].HeaderText = "Active";
            dgvFile.Columns["VendorID"].Width = 50;
            dgvFile.Columns["VendorName"].Width = 300;
            dgvFile.Columns["CatalogNo"].Width = 150;
            dgvFile.Columns["CatalogName"].Width = 300;
            dgvFile.Columns["CatalogDesc"].Width = 200;
            dgvFile.Columns["Grade"].Width = 80;           
            dgvFile.Columns["IsActive"].Width = 60;
            dgvFile.Columns["CatalogNameID"].Visible = false;
            dgvFile.Columns["GradeID"].Visible = false;
            dgvFile.Columns["Website"].Visible = false;
            dgvFile.Columns["VendorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
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
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);

            if (nCatMasterSw == 1)
            {
                txtVendorID.Text = strPRVendorID;
                txtVendorName.Text = strPRVendorName;
                txtCatNameID.Text = strPRCatNameID;
                txtCatalogName.Text = strPRCatName;
                AddEditMode(true);
            }
            txtVendorID.Enabled = true; txtVendorName.Enabled = true;
            picVendors.Enabled = true; txtCatalogNo.Enabled = true; chkIsActive.Checked = true;
            txtVendorName.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            if (pnlRecord.Visible == false)
                LoadData();
            OpenControls(this.pnlRecord, true);
            txtVendorID.Enabled = false;
            txtVendorName.Enabled = false;
            picVendors.Enabled = false;
            txtCatalogNo.Enabled = false;
            txtCatNameID.Enabled = false;
            txtCatalogName.Enabled = false;
            txtGradeID.Focus();
            btnClose.Visible = false;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
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

                sqlcmd.Parameters.AddWithValue("@VendorID", txtVendorID.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@CatalogNo", txtCatalogNo.Text.Trim());
               
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelCatalogMaster";

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

            if (txtVendorID.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Vendor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtVendorID.Focus();
                return;
            }

            if (txtCatalogNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a Catalog Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCatalogNo.Focus();
                return;
            }

            // Check if Vendor/Catalog No already exists in GIS
            if (nMode == 1)
            {
                bool isExists = false;

                isExists = PSSClass.Procurements.CatVendorExists(Convert.ToInt16(txtVendorID.Text), txtCatalogNo.Text.Trim());

                if (isExists)
                {
                    MessageBox.Show("Catalog Number already exists for this vendor. Please enter another!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCatalogNo.Focus();
                    return;
                }
            }

            ////// Check if Vendor/Catalog Name already exists in GIS
            ////if (nMode == 1)
            ////{
            ////    bool isExists = false;

            ////    isExists = PSSClass.Procurements.CatNameExists(Convert.ToInt16(txtVendorID.Text), Convert.ToInt16(txtCatNameID.Text));

            ////    if (isExists)
            ////    {
            ////        MessageBox.Show("Catalog Name already exists for this vendor. Please enter another!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            ////        txtCatNameID.Focus();
            ////        return;
            ////    }
            ////}

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@VendorID", txtVendorID.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CatalogNo", txtCatalogNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CatalogNameID", txtCatNameID.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CatalogDesc", txtCatDesc.Text.Trim());

            if (txtGradeID.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@GradeID", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@GradeID", txtGradeID.Text.Trim());  
            }
                      
            sqlcmd.Parameters.AddWithValue("@UnitPrice", txtUnitPrice.Text.Trim()); 
            sqlcmd.Parameters.AddWithValue("@IsActive", Convert.ToBoolean(chkIsActive.CheckState));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditCatalogMaster";
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
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("CatalogNo", txtCatalogNo.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            LoadData();
            dgvVendorNames.Visible = false; dgvCatGrades.Visible = false; dgvCatNames.Visible = false;
            MessageBox.Show("Record successfully saved!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            ClearControls(this);
            AddEditMode(false);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            dgvVendorNames.Visible = false; dgvCatGrades.Visible = false; dgvCatNames.Visible = false; 
            nMode = 0;
        }

        private void Catalog_Load(object sender, EventArgs e)
        {
            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }
            nMode = 0;
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Catalogs");

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            LoadVendors();
            LoadCatNames();
            LoadCatGrades();

            BuildPrintItems();
            BuildSearchItems();

            if (nCatMasterSw == 1)
            {               
                AddRecord();
            }
            else
            {
                dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                try
                {
                    dgvFile.Rows[0].Selected = true; dgvFile.Select();
                }
                catch { }

                LoadRecords();
            }
        }

        private void Catalog_KeyDown(object sender, KeyEventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0;
            pnlRecord.Visible = false; 

            if (nCatMasterSw == 1)
            {
                nCatMasterSw = 0;
                this.Close(); this.Dispose();
            }
            else
            {
                pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
                dgvFile.Focus();
            }
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

        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;            
        }

        // MY 07/30/2015 - START: txt/dgvVendorNames events
        private void dgvVendorNames_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
                txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();                
                dgvVendorNames.Visible = false;
            }
        }

        private void dgvVendorNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvVendorNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
                    txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
                    dgvVendorNames.Visible = true;
                }
                else if (e.KeyChar == 27)
                {
                    dgvVendorNames.Visible = false;
                }
            }
        }
        private void txtVendorName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvVendorNames.Visible = true; dgvVendorNames.BringToFront();
            }
        }

        private void dgvVendorNames_Leave(object sender, EventArgs e)
        {
            dgvVendorNames.Visible = false;
        }

        private void txtVendorName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwVendorNames;
                dvwVendorNames = new DataView(dtVendors, "VendorName like '%" + txtVendorName.Text.Trim().Replace("'", "''") + "%'", "VendorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvVendorNames, dvwVendorNames);
            }
        }

        private void dgvVendorNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
                txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
                dgvVendorNames.Visible = true;
                dgvVendorNames.BringToFront();
            }
        }

        private void picVendors_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadVendors(); ;
                dgvVendorNames.Visible = true; dgvVendorNames.BringToFront();
            }
        }

        private void txtVendorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtVendorName.Text = PSSClass.Calibration.EqptVendorName(Convert.ToInt16(txtVendorID.Text));
                    if (txtVendorName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Vendor found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvVendorNames.Visible = false;
                    txtCatalogNo.Focus();
                }
                else
                {
                    txtVendorName.Text = ""; dgvVendorNames.Visible = false;
                }
            }
        }
       
        // MY 07/30/2015 - END: txt/dgvVendorNames events   

        // MY 07/31/2015 - START: txt/dgvVCatNames events
        private void dgvCatNames_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtCatalogName.Text = dgvCatNames.CurrentRow.Cells["CatalogName"].Value.ToString();
                txtCatNameID.Text = dgvCatNames.CurrentRow.Cells["CatalogNameID"].Value.ToString();  
                dgvCatNames.Visible = false;
            }
        }

        private void dgvCatNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCatNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCatalogName.Text = dgvCatNames.CurrentRow.Cells["CatalogName"].Value.ToString();
                    txtCatNameID.Text = dgvCatNames.CurrentRow.Cells["CatalogNameID"].Value.ToString();  
                    dgvCatNames.Visible = true;
                }
                else if (e.KeyChar == 27)
                {
                    dgvCatNames.Visible = false;
                }
            }
        }
        private void txtCatalogName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvCatNames.Visible = true; dgvCatNames.BringToFront();
            }
        }

        private void dgvCatNames_Leave(object sender, EventArgs e)
        {
            dgvCatNames.Visible = false;
        }

        private void txtCatalogName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwCatNames;
                dvwCatNames = new DataView(dtCatNames, "CatalogName like '%" + txtCatalogName.Text.Trim().Replace("'", "''") + "%'", "CatalogName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvCatNames, dvwCatNames);
            }
        }

        private void dgvCatNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtCatalogName.Text = dgvCatNames.CurrentRow.Cells["CatalogName"].Value.ToString();
                txtCatNameID.Text = dgvCatNames.CurrentRow.Cells["CatalogNameID"].Value.ToString();  
                dgvCatNames.Visible = true;
                dgvCatNames.BringToFront();
            }
        }

        private void picCatNames_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadCatNames(); ;
                dgvCatNames.Visible = true; dgvCatNames.BringToFront();
            }
        }

        private void txtCatNameID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCatalogName.Text = PSSClass.Procurements.CatName(Convert.ToInt16(txtCatNameID.Text));
                    if (txtCatalogName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Catalog Name found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvCatNames.Visible = false;
                    txtCatNameID.Focus();
                }
                else
                {
                    txtCatalogName.Text = ""; dgvCatNames.Visible = false;
                }
            }
        }
        // MY 07/31/2015 - END: txt/dgvCatNames events   

        // MY 08/03/2015 - START: txt/dgvCatGrades events
        private void dgvCatGrades_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtGrade.Text = dgvCatGrades.CurrentRow.Cells["CatalogGrade"].Value.ToString();
                txtGradeID.Text = dgvCatGrades.CurrentRow.Cells["GradeID"].Value.ToString();
                dgvCatGrades.Visible = false;
            }
        }

        private void dgvCatGrades_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCatGrades_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtGrade.Text = dgvCatGrades.CurrentRow.Cells["CatalogGrade"].Value.ToString();
                    txtGradeID.Text = dgvCatGrades.CurrentRow.Cells["GradeID"].Value.ToString();
                    dgvCatGrades.Visible = true;
                }
                else if (e.KeyChar == 27)
                {
                    dgvCatGrades.Visible = false;
                }
            }
        }
        private void txtGrade_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvCatGrades.Visible = true; dgvCatGrades.BringToFront();
            }
        }

        private void dgvCatGrades_Leave(object sender, EventArgs e)
        {
            dgvCatGrades.Visible = false;
        }

        private void txtGrade_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwCatGrades;
                dvwCatGrades = new DataView(dtCatGrades, "CatalogGrade like '%" + txtGrade.Text.Trim().Replace("'", "''") + "%'", "CatalogGrade", DataViewRowState.CurrentRows);
                dvwSetUp(dgvCatGrades, dvwCatGrades);
            }
        }

        private void dgvCatGrades_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtGrade.Text = dgvCatGrades.CurrentRow.Cells["CatalogGrade"].Value.ToString();
                txtGradeID.Text = dgvCatGrades.CurrentRow.Cells["GradeID"].Value.ToString();
                dgvCatGrades.Visible = true;
                dgvCatGrades.BringToFront();
            }
        }

        private void picGrades_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadCatGrades();
                dgvCatGrades.Visible = true; dgvCatGrades.BringToFront();
            }
        }

        private void txtGradeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtGrade.Text = PSSClass.Procurements.CatalogGradeName(Convert.ToInt16(txtGradeID.Text));
                    if (txtGrade.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Grade found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvCatGrades.Visible = false;
                    txtGradeID.Focus();
                }
                else
                {
                    txtGrade.Text = ""; dgvCatGrades.Visible = false;
                }
            }
        }

        // MY 08/03/2015 - END: txt/dgvCatGrades events   

        private void btnAddNames_Click(object sender, EventArgs e)
        {           
            int intOpen = PSSClass.General.OpenForm(typeof(CatalogNames));

            if (intOpen == 1)
            {
                PSSClass.General.CloseForm(typeof(CatalogNames));
            }
            CatalogNames childForm = new CatalogNames();
            childForm.Text = "CATALOG NAMES";
            childForm.MdiParent = this.MdiParent;        
            childForm.nCatNameSw = 1;
            childForm.Show();     
        }

        private void btnCheckBrowser_Click(object sender, EventArgs e)
        {
            if (txtWebsite.Text.Trim() == "")
            {
                MessageBox.Show("Vendor website address empty!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtWebsite.Focus();
                return;
            }       

            try
            {
                txtWebsite.Text = PSSClass.Procurements.CatalogVendorWebsite(Convert.ToInt16(txtVendorID.Text));

                System.Diagnostics.Process.Start(txtWebsite.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Please enter a valid URL for this vendor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
    }
}
