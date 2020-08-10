using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;

namespace PSS
{
    public partial class Units : PSS.TemplateForm
    {
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private int nCtr = 0;
        private int nSw = 0;
        private string strFileAccess = "RO";
        private DataTable dtUnits = new DataTable();

        public Units()
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
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
            chkShowInactive.Click += new EventHandler(chkShowInactiveClickHandler);
        }

        private void Units_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Units");

            LoadRecords();
            DataGridSetting();
            //BuildPrintItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

            dtUnits.Columns.Add("UnitID", typeof(Int16));
            dtUnits.Columns.Add("UnitDesc", typeof(string));
            dtUnits.Columns.Add("DateCreated", typeof(DateTime));
            dtUnits.Columns.Add("CreatedByID", typeof(Int16));
            dtUnits.Columns.Add("LastUpdate", typeof(DateTime));
            dtUnits.Columns.Add("LastUserID", typeof(Int16));
            bsUnits.DataSource = dtUnits;
        }

        private void LoadRecords()
        {
            nMode = 0;
            DataTable dt = PSSClass.Units.UnitsMaster(1);
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "UnitID<>0";
            DataGridSetting();
            try
            {
                if (tsddbSearch.DropDownItems.Count == 0)
                {
                    int i = 0;

                    arrCol = new string[dt.Columns.Count];

                    ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - 4];// ToolStripMenuItem[arrCol.Length - 4];

                    foreach (DataColumn colFile in dt.Columns)
                    {
                        if (colFile.ColumnName.ToString() != "DateCreated" &&
                            colFile.ColumnName.ToString() != "CreatedByID" &&
                            colFile.ColumnName.ToString() != "LastUpdate" &&
                            colFile.ColumnName.ToString() != "LastUserID")
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
                    }
                    for (int j = 0; j < cklColumns.Items.Count; j++)
                    {
                        cklColumns.SetItemChecked(j, true);
                    }
                    tsddbSearch.DropDownItems.AddRange(items);
                    tslSearchData.Text = tsddbSearch.DropDownItems[1].Text;
                    tstbSearchField.Text = tsddbSearch.DropDownItems[1].Name;
                }
            }
            catch (Exception c)
            {
                MessageBox.Show(c.ToString());
            }
            FileAccess();
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["UnitDesc"].HeaderText = "UNIT DESCRIPTION";
            dgvFile.Columns["UnitID"].HeaderText = "UNIT ID";
            dgvFile.Columns["UnitDesc"].Width = 150;
            dgvFile.Columns["UnitID"].Visible = false;
            dgvFile.Columns["DateCreated"].Visible = false;
            dgvFile.Columns["CreatedByID"].Visible = false;
            dgvFile.Columns["LastUpdate"].Visible = false;
            dgvFile.Columns["LastUserID"].Visible = false;
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtDesc.Text = dgvFile.CurrentRow.Cells["UnitDesc"].Value.ToString();
            txtID.Text = dgvFile.CurrentRow.Cells["UnitID"].Value.ToString();


        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[2];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "UnitDesc";
            items[0].Text = "Sorted by Unit Description";
            items[0].Click += new EventHandler(PrintUnitDescClickHandler);

            //items[1] = new ToolStripMenuItem();
            //items[1].Name = "DepartmentName";
            //items[1].Text = "Sorted by Department Name";
            //items[1].Click += new EventHandler(PrintDeptNameClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void PrintUnitDescClickHandler(object sender, EventArgs e)
        {
            //DepartmentRpt rptDeptNameList = new DepartmentRpt();
            //rptDeptNameList.WindowState = FormWindowState.Maximized;
            //rptDeptNameList.rptName = "UnitDesc";
            //rptDeptNameList.rptLabel = "UNITS OF MEASURE REFERENCE LIST - SORTED BY DESCRIPTION";
            //rptDeptNameList.Show();
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

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
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

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            OpenControls(this.pnlRecord, true);

            dtUnits.Rows.Clear();
            DataRow dR = dtUnits.NewRow();
            dR["UnitID"] = DBNull.Value;
            dR["UnitDesc"] = "";
            dR["DateCreated"] = DateTime.Now;
            dR["CreatedByID"] = LogIn.nUserID;
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;
            dtUnits.Rows.Add(dR);
            bsUnits.DataSource = dtUnits;
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            txtID.DataBindings.Add("Text", bsUnits, "UnitID");
            txtDesc.DataBindings.Add("Text", bsUnits, "UnitDesc");
            txtDesc.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();
            dtUnits.Rows.Clear();
            try
            {
                DataRow dR = dtUnits.NewRow();
                dR["UnitID"] = Convert.ToInt16(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["UnitID"].Value);
                dR["UnitDesc"] = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["UnitDesc"].Value;
                dR["DateCreated"] = Convert.ToDateTime(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateCreated"].Value);
                dR["CreatedByID"] = Convert.ToInt16(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["CreatedByID"].Value);
                dR["LastUpdate"] = Convert.ToDateTime(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["LastUpdate"].Value);
                dR["LastUserID"] = Convert.ToInt16(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["LastUserID"].Value);
                dtUnits.Rows.Add(dR);
                dtUnits.AcceptChanges();
                bsUnits.DataSource = dtUnits;

                foreach (Control c in pnlRecord.Controls)
                {
                    c.DataBindings.Clear();
                }
                txtID.DataBindings.Add("Text", bsUnits, "UnitID");
                txtDesc.DataBindings.Add("Text", bsUnits, "UnitDesc");
            }
            catch { }

            txtDesc.Focus(); btnClose.Visible = false;
        }

        private void SaveRecord()
        {
            if (txtDesc.Text.Trim() == "")
            {
                MessageBox.Show("Please enter description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDesc.Focus();
                return;
            }

            if (nMode == 1)
                txtID.Text = PSSClass.DataEntry.NewID("Units", "UnitID").ToString();

            if (PSSClass.DataEntry.MatchingRecord("UnitID", "UnitDesc", "Units", txtDesc.Text, nMode, Convert.ToInt16(txtID.Text), "") == true)
            {
                MessageBox.Show("Matching description found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDesc.Focus();
                return;
            }

            bsUnits.EndEdit();

            DataTable dt = dtUnits.GetChanges();
            if (dt == null)
            {
                MessageBox.Show("No changes made.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
                ClearControls(this.pnlRecord);
                AddEditMode(false);
                return;
            }
            dt.Dispose();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Units", sqlcnn);

            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);

            if (nMode == 1)
            {
                cmdBuilder.GetInsertCommand();
                bsUnits.EndEdit();
                da.Update(dtUnits);
            }
            else
            {
                try
                {
                    dtUnits.Rows[bsUnits.Position]["LastUpdate"] = DateTime.Now;
                    dtUnits.Rows[bsUnits.Position]["LastUserID"] = LogIn.nUserID;
                    bsUnits.EndEdit();
                    cmdBuilder.GetUpdateCommand();
                    da.Update(dtUnits);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            da.Dispose(); cmdBuilder.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();

            dgvFile.Refresh();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("UnitID", txtID.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
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

        private void DeleteRecord()
        {
            LoadData();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problen encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.Add(new SqlParameter("@DeptID", SqlDbType.SmallInt));
                sqlcmd.Parameters["@DeptID"].Value = Convert.ToInt16(txtID.Text);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelDepartment";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            LoadRecords();
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
                    bsFile.Filter = "UnitID<>0";
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

        private void chkShowInactiveClickHandler(object sender, EventArgs e)
        {
            if (chkShowInactive.Checked)
            {
                LoadRecords();
            }
            else
            {
                LoadRecords();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
            FileAccess();
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

        private void Units_KeyDown(object sender, KeyEventArgs e)
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
