// EquipmentMftr.cs
// AUTHOR       : Stanley Tsao
// DATE         : 08-18-2017
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Equipment Manufacturer File Maintenance

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
    public partial class EquipmentMftr : PSS.TemplateForm
    {
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private int nSw = 0;
        private string strFileAccess = "FA";
        private DataTable dtEquipmentMftr = new DataTable();

        public EquipmentMftr()
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

        private void EquipmentMftr_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "EquipmentMftr");

            LoadRecords();
            DataGridSetting();
            BuildPrintItems();
            tsddbPrint.Enabled = false;

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

            dtEquipmentMftr.Columns.Add("MftrID", typeof(Int16));
            dtEquipmentMftr.Columns.Add("MftrName", typeof(string));
            bsEquipmentMftr.DataSource = dtEquipmentMftr;
        }

        public static DataTable MasterLoader(string SP)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return null;
            }
            SqlCommand sqlcmd = new SqlCommand(SP, sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            try
            {
                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                DataTable dTable = new DataTable();
                dTable.Load(sqldr);
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                return dTable;
            }
            catch
            {
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                return null;
            }
        }

        private void LoadRecords()
        {
            nMode = 0;
            DataTable dt = MasterLoader("spGetEquipmentMftr");
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "MftrID<>0";
            DataGridSetting();

            if (tsddbSearch.DropDownItems.Count == 0)
            {
                Int64 i = 0;
                arrCol = new string[dt.Columns.Count];
                ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length];
                foreach (DataColumn colFile in dt.Columns)
                {
                    items[i] = new ToolStripMenuItem();
                    items[i].Name = colFile.ColumnName;

                    //Using LINQ to insert space before non-consecutive capital letters
                    var val = colFile.ColumnName;
                    val = string.Concat(val.Select((x, y) => (char.IsUpper(x) && y > 0 && (char.IsLower(val[y - 1]) || (y < val.Count() - 1 && char.IsLower(val[y + 1])))) ? " " + x : x.ToString()));

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

            FileAccess();
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["MftrName"].HeaderText = "Manufaturers";
            dgvFile.Columns["MftrName"].Width = 300;
            dgvFile.Columns["MftrID"].Visible = false;
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtDesc.Text = dgvFile.CurrentRow.Cells["MftrName"].Value.ToString();
            txtID.Text = dgvFile.CurrentRow.Cells["MftrID"].Value.ToString();
            txtID.ReadOnly = true;
        }

        private void BuildPrintItems()
        {
            tsddbPrint.Click += new EventHandler(PrintClick);
        }        
        
        private void PrintClick(object sender, EventArgs e)
        {
            //PrintEquipmentMftr rpt = new PrintEquipmentMftr();
            //rpt.WindowState = FormWindowState.Maximized;
            //rpt.Show();
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

            dtEquipmentMftr.Rows.Clear();
            DataRow dR = dtEquipmentMftr.NewRow();
            dR["MftrID"] = DBNull.Value;
            dR["MftrName"] = "";
            dtEquipmentMftr.Rows.Add(dR);
            bsEquipmentMftr.DataSource = dtEquipmentMftr;
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            txtID.DataBindings.Add("Text", bsEquipmentMftr, "MftrID");
            txtDesc.DataBindings.Add("Text", bsEquipmentMftr, "MftrName");
            txtDesc.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();
            dtEquipmentMftr.Rows.Clear();
            try
            {
                DataRow dR = dtEquipmentMftr.NewRow();
                dR["MftrID"] = Convert.ToInt16(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["MftrID"].Value);
                dR["MftrName"] = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["MftrName"].Value;
                dtEquipmentMftr.Rows.Add(dR);
                dtEquipmentMftr.AcceptChanges();
                bsEquipmentMftr.DataSource = dtEquipmentMftr;

                foreach (Control c in pnlRecord.Controls)
                {
                    c.DataBindings.Clear();
                }
                txtID.DataBindings.Add("Text", bsEquipmentMftr, "MftrID");
                txtDesc.DataBindings.Add("Text", bsEquipmentMftr, "MftrName");
            }
            catch { }

            txtDesc.Focus(); btnClose.Visible = false;
        }
        
        private void SaveRecord()
        {
            bsEquipmentMftr.EndEdit();
            DataTable dt = dtEquipmentMftr.GetChanges();
            if (dt == null)
            {
                pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
                ClearControls(this.pnlRecord);
                AddEditMode(false);
                FileAccess();
                return;
            }
            dt.Dispose();

            if (txtDesc.Text.Trim() == "")
            {
                MessageBox.Show("Please enter description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDesc.Focus();
                return;
            }

            if (nMode == 1)
                txtID.Text = PSSClass.DataEntry.NewID("EquipmentMftr", "MftrID").ToString();
            
            if (PSSClass.DataEntry.MatchingRecord("MftrID", "MftrName", "EquipmentMftr", txtDesc.Text, nMode, Convert.ToInt16(txtID.Text), "") == true)
            {
                MessageBox.Show("Matching description found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDesc.Focus();
                return;
            }
            
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@ID", Convert.ToInt16(txtID.Text));
            sqlcmd.Parameters.AddWithValue("@Desc", txtDesc.Text);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditEquipmentMftr";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            dgvFile.Refresh();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("MftrID", txtID.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            nMode = 0;
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
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            LoadRecords();
            if (nMode == 2)
                PSSClass.General.FindRecord("MftrID", txtID.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
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
                    MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@ID", Convert.ToInt16(txtID.Text));
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelEquipmentMftr";

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
            dgvFile.Refresh();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            LoadRecords();
            ClearControls(this.pnlRecord);
            nMode = 0;
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
                    bsFile.Filter = "MftrID<>0";
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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = true;
            }
        }

        private void EquipmentMftr_KeyDown(object sender, KeyEventArgs e)
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
                case Keys.F4:
                    if (nMode == 0 && strFileAccess == "FA")
                    {
                        DeleteRecord();
                    }
                    break;
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
