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
    public partial class States : PSS.TemplateForm
    {
        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private DataSet dsRegions = new DataSet();
        private string strFileAccess = "RO"; //User's Access to this File

        public static string FormName = "";

        public States()
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
        }

        private void LoadRecords()
        {
            DataTable dt = PSSClass.States.StatesMaster(1);

            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            FileAccess();
        }

        private void LoadRegions()
        {
            DataSet ds = new DataSet();

            cboRegions.DataSource = null;
            ds = PSSClass.Regions.RegionsDDL();
            if (ds == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            cboRegions.DataSource = ds.Tables[0];
            cboRegions.DisplayMember = "RegionName";
            cboRegions.ValueMember = "RegionCode";
            ds.Dispose();
        }
        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[2];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "StateName";
            items[0].Text = "Sorted by State Name";
            items[0].Click += new EventHandler(PrintStateNameClickHandler);

            items[1] = new ToolStripMenuItem();
            items[1].Name = "StateCode";
            items[1].Text = "Grouped by Regions";
            items[1].Click += new EventHandler(PrintStateRegionClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.States.StatesMaster(1);
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }

            int i = 0;
            int n = 0;

            arrCol = new string[dt.Columns.Count];

            ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

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
                i += 1;
            }
            tsddbSearch.DropDownItems.AddRange(items);
            tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
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
        private void PrintStateNameClickHandler(object sender, EventArgs e)
        {
            RegStatesRpt rpt = new RegStatesRpt();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "StatesRef";
            rpt.rptLabel = "STATES REFERENCE LIST";
            rpt.Show();
        }

        private void PrintStateRegionClickHandler(object sender, EventArgs e)
        {
            RegStatesRpt rpt = new RegStatesRpt();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "StatesByRegion";
            rpt.rptLabel = "STATES REFERENCE LIST - GROUPED BY REGION";
            rpt.Show();
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
            try
            {
                bsFile.Filter = "StateCode<>''";
                PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                dgvFile.Select();
                if (pnlRecord.Visible == true)
                    LoadData();
            }
            catch { }
        }

        private void SearchFilterClickHandler(object sender, EventArgs e)
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

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            bsFile.Filter = "StateCode<>''";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtCode.Text = dgvFile.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dgvFile.CurrentRow.Cells[1].Value.ToString();
            cboRegions.Text = dgvFile.CurrentRow.Cells[2].Value.ToString();
        }

        private bool MatchingRecord(string strKeyField, string strMatchField, string strTableName, string strMatchData)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand("SELECT " + strKeyField + ", " + strMatchField + " FROM " + strTableName +
                                               " WHERE " + strMatchField + "='" + strMatchData + "'", sqlcnn);
            SqlDataReader sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                if (nMode == 1)
                    return true;
                else
                {
                    sqldr.Read();
                    string strID = Convert.ToString(sqldr.GetValue(0));
                    if (strID != txtCode.Text)
                        return true;
                }
            }
            sqldr.Close(); sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
            return false;
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
            dgvFile.Columns[0].HeaderText = "CODE";
            dgvFile.Columns[1].HeaderText = "NAME";
            dgvFile.Columns[2].HeaderText = "REGION";
            dgvFile.Columns[0].Width = 160;
            dgvFile.Columns[0].DefaultCellStyle.Padding = new Padding(30, 0, 0, 0);
            dgvFile.Columns[1].Width = 300;
            dgvFile.Columns[2].Width = 300;
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
            txtCode.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            LoadData();
            txtCode.ReadOnly = true;
            txtName.Focus(); btnClose.Visible = false;
        }

        private void DeleteRecord()
        {
            LoadData();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                //SqlConnection sqlcnn = new MdiGIS().MDFConnection();
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                SqlCommand sqlcmd = new SqlCommand();

                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.Add(new SqlParameter("@StateCode", SqlDbType.NVarChar));
                sqlcmd.Parameters["@StateCode"].Value = txtCode.Text;

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDeleteState";

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
            if (txtCode.Text.Trim() == "")
            {
                MessageBox.Show("State Code is blank.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }

            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("State Name is blank.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            if (MatchingRecord("StateCode", "StateCode", "States", txtCode.Text) == true)
            {
                MessageBox.Show("Matching record found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }

            if (MatchingRecord("StateCode", "StateName", "States", txtName.Text) == true)
            {
                MessageBox.Show("Matching record found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@StateCode", SqlDbType.NVarChar));
            sqlcmd.Parameters["@StateCode"].Value = txtCode.Text.ToUpper();

            sqlcmd.Parameters.Add(new SqlParameter("@StateName", SqlDbType.NVarChar));
            sqlcmd.Parameters["@StateName"].Value = txtName.Text.Trim();

            sqlcmd.Parameters.Add(new SqlParameter("@RegCode", SqlDbType.NVarChar));
            sqlcmd.Parameters["@RegCode"].Value = cboRegions.SelectedValue;

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = 0; // LogIn.txtUserID

            sqlcmd.CommandType = CommandType.StoredProcedure;
            if (nMode == 1)
            {
                sqlcmd.CommandText = "spAddState";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    int code = System.Runtime.InteropServices.Marshal.GetExceptionCode();
                    if (code == -532462766)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
            else if (nMode == 2)
            {
                sqlcmd.CommandText = "spEditState";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            dgvFile.Refresh();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("StateCode", txtCode.Text, bsFile, dgvFile);
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

        //private void dvwSetUp(DataGridView dgvObj, DataView dvw)
        //{
        //    dgvObj.Columns[0].Width = 142;
        //    dgvObj.Columns[1].Visible = false;
        //    dgvObj.DataSource = dvw;
        //}

        private void States_KeyDown(object sender, KeyEventArgs e)
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

        private void States_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "States");
            LoadRegions();
            LoadRecords();

            BuildPrintItems();
            BuildSearchItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
        }
        
        private void dgvRegions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
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
                mouseDown = false;
        }

        private void pnlRecord_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void txtRegion_DoubleClick(object sender, EventArgs e)
        {
            OpenRegions();
        }

        private void OpenRegions()
        {
            if (nMode == 1 || nMode == 2)
                FormName = "RegionDataEntry";
            else
                FormName = "RegionDataView";

            int intOpen = PSSClass.General.OpenForm(typeof(Regions));

            if (intOpen == 0)
            {
                Form childForm = new Regions();
                childForm.MdiParent = this.MdiParent;
                childForm.Text = "Regions";
                childForm.Show();
            }
            else
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(Regions))
                    {
                        form.WindowState = FormWindowState.Maximized;
                        break;
                    }
                }
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; 
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
        }
    }
}
