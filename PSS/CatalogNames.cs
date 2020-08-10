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
    public partial class CatalogNames : PSS.TemplateForm
    {
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;      
        private int nSw = 0;
        private string strFileAccess = "RO";

        public byte nCatNameSw;

        DataTable dtCatOrderTypes = new DataTable();                                        // MY 08/06/2015 - Pop-up GridView Catalog Order Type query

        public CatalogNames()
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
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            DataTable dt = PSSClass.Procurements.CatalogNameMaster();
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "CatalogNameID <> 0";
            FileAccess();
            DataGridSetting();
        }

        private void FileAccess()
        {
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
            dt = PSSClass.Procurements.CatalogNames();
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
                    bsFile.Filter = "CatalogNameID<>0";
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
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtCatNameID.Text = dgvFile.CurrentRow.Cells["CatalogNameID"].Value.ToString();
            txtCatName.Text = dgvFile.CurrentRow.Cells["CatalogName"].Value.ToString();
            txtCatOrderTypeID.Text = dgvFile.CurrentRow.Cells["CatalogOrderTypeID"].Value.ToString();
            txtCatOrderTypeName.Text = dgvFile.CurrentRow.Cells["CatalogOrderTypeName"].Value.ToString();   
            if (dgvFile.CurrentRow.Cells["IsActive"].Value.ToString() == "True")
            {
                chkIsActive.Checked = true;
            } 
        }

        private void LoadCatOrderTypes()
        {
            dgvCatOrderTypes.DataSource = null;

            dtCatOrderTypes = PSSClass.Procurements.CatalogOrderTypes();
            if (dtCatOrderTypes == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            if (dtCatOrderTypes.Rows.Count == 0)
                return;

            dgvCatOrderTypes.DataSource = dtCatOrderTypes;
            StandardDGVSetting(dgvCatOrderTypes);
            dgvCatOrderTypes.Columns[0].Width = 110;
            dgvCatOrderTypes.Columns[1].Visible = false;
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
            dgvFile.Columns["CatalogNameID"].HeaderText = "Catalog Name ID";
            dgvFile.Columns["CatalogName"].HeaderText = "Catalog Name";
            dgvFile.Columns["CatalogOrderTypeName"].HeaderText = "Order Type";
            dgvFile.Columns["IsActive"].HeaderText = "Active";
            dgvFile.Columns["CatalogNameID"].Width = 80;
            dgvFile.Columns["CatalogName"].Width = 320;
            dgvFile.Columns["IsActive"].Width = 60;
            dgvFile.Columns["CatalogOrderTypeID"].Visible = false;
            dgvFile.Columns["CreatedByID"].Visible = false;
            dgvFile.Columns["DateCreated"].Visible = false;
            dgvFile.Columns["LastUpdate"].Visible = false;
            dgvFile.Columns["LastUserID"].Visible = false;
            dgvFile.Columns["CatalogNameID"].DefaultCellStyle.Padding = new Padding(30, 0, 0, 0);                   
            dgvFile.Columns["CatalogNameID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            txtCatNameID.Enabled = false;
            txtCatNameID.Text = "< New >";
            txtCatName.Focus();
            chkIsActive.Checked = true;
            txtCatOrderTypeID.Text = "1";
            txtCatOrderTypeName.Text = "Materials";
            if (nCatNameSw == 1)
            {
                AddEditMode(true);
            }
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            LoadData();
            txtCatNameID.Focus(); btnClose.Visible = false;
            txtCatNameID.Enabled = false;
        }

        private void DeleteRecord()
        {           
        }

        private void SaveRecord()
        {
            if (txtCatName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Catalog Name!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCatNameID.Focus();
                return;
            }

            // Check if Catalog name already exists in GIS
            if (nMode == 1)
            {
                bool isExists = false;

                isExists = PSSClass.Procurements.CatNameMasterExists(txtCatName.Text.Trim());

                if (isExists)
                {
                    MessageBox.Show("Catalog Name already exists!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCatName.Focus();
                    return;
                }
            }

            if (nMode == 1)
                txtCatNameID.Text = PSSClass.DataEntry.NewID("CatalogNames", "CatalogNameID").ToString();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@CatalogNameID", Convert.ToInt16(txtCatNameID.Text));
            sqlcmd.Parameters.AddWithValue("@CatalogName", txtCatName.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CatalogOrderTypeID", Convert.ToInt16(txtCatOrderTypeID.Text));
            sqlcmd.Parameters.AddWithValue("@IsActive", Convert.ToBoolean(chkIsActive.CheckState));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
 
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditCatalogNames";
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
            pnlRecord.Visible = false; 

            if (nCatNameSw == 1)
            {
                nCatNameSw = 0;
                this.Close(); this.Dispose();
            }
            else
            {
                dgvFile.Visible = true; bnFile.Enabled = true;
                AddEditMode(false);
                LoadRecords();
                PSSClass.General.FindRecord("CatalogNameID", txtCatNameID.Text, bsFile, dgvFile);
                ClearControls(this.pnlRecord);
            }
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
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true; dgvCatOrderTypes.Visible = false;
            nMode = 0;
        }

        private void CatalogNames_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "CatalogNames");

            LoadRecords();
            LoadCatOrderTypes();
            DataGridSetting();
            BuildPrintItems();
            BuildSearchItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
            
            if (nCatNameSw == 1)
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

        private void CatalogNames_KeyDown(object sender, KeyEventArgs e)
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

            if (nCatNameSw == 1)
            {
                nCatNameSw = 0;
                this.Close(); this.Dispose();
            }
            else
            {
                dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
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

        // MY 08/06/2015 - START: txt/dgvCatOrderTypes events
        private void dgvCatOrderTypes_DoubleClick(object sender, EventArgs e)
        {
            txtCatOrderTypeName.Text = dgvCatOrderTypes.CurrentRow.Cells[0].Value.ToString();
            txtCatOrderTypeID.Text = dgvCatOrderTypes.CurrentRow.Cells[1].Value.ToString();
            dgvCatOrderTypes.Visible = false;
        }

        private void dgvCatOrderTypes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCatOrderTypes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtCatOrderTypeName.Text = dgvCatOrderTypes.CurrentRow.Cells[0].Value.ToString();
                txtCatOrderTypeID.Text = dgvCatOrderTypes.CurrentRow.Cells[1].Value.ToString();
                dgvCatOrderTypes.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvCatOrderTypes.Visible = false;
            }
        }
        private void txtCatOrderTypeName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvCatOrderTypes.Visible = true; dgvCatOrderTypes.BringToFront();
            }
        }

        private void dgvCatOrderTypes_Leave(object sender, EventArgs e)
        {
            dgvCatOrderTypes.Visible = false;
        }

        private void txtCatOrderTypeName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwCatOrderTypes;
                dvwCatOrderTypes = new DataView(dtCatOrderTypes, "CatOrderTypeName like '%" + txtCatOrderTypeName.Text.Trim().Replace("'", "''") + "%'", "CatOrderTypeName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvCatOrderTypes, dvwCatOrderTypes);
            }
        }

        private void dgvCatOrderTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCatOrderTypeName.Text = dgvCatOrderTypes.CurrentRow.Cells[0].Value.ToString();
            txtCatOrderTypeID.Text = dgvCatOrderTypes.CurrentRow.Cells[1].Value.ToString();
            dgvCatOrderTypes.Visible = false;
        }

        private void picCatOrderTypes_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadCatOrderTypes();
                dgvCatOrderTypes.Visible = true; dgvCatOrderTypes.BringToFront(); 
            }
        }

        private void txtCatOrderTypeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //Attention: to be re coded
                //txtCatOrderTypeName.Text = PSSClass.QA.OOSTypes(Convert.ToInt16(txtCatOrderTypeID.Text));

                if (txtCatOrderTypeName.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Catalog Order Type ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvCatOrderTypes.Visible = false;
            }
            else if (nMode != 0)
            {
                txtCatOrderTypeName.Text = ""; dgvCatOrderTypes.Visible = false;
            }
        }
        // MY 08/06/2015 - END: txt/dgvCatOrderTypes events
    }
}
