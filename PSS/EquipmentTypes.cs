// EquipmentTypes.cs
// AUTHOR       : Stanley Tsao
// DATE         : 08-31-2017
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Equipment Type File Maintenance

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.ComponentModel;

namespace PSS
{
    public partial class EquipmentTypes : PSS.TemplateForm
    {
        private byte nMode = 0, nFMode = 0;
        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "FA";
        private DataTable dtEquipmentType = new DataTable();
        private DataTable dtFreq = new DataTable();
        //private string svSType = "";

        public EquipmentTypes()
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

        private void EquipmentType_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "EquipmentTypes");

            dtEquipmentType.Columns.Add("TypeID", typeof(Int16));
            dtEquipmentType.Columns.Add("TypeDesc", typeof(string));
            bsFile.DataSource = dtEquipmentType;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;

            dtFreq.Columns.Add("ServiceType", typeof(string));
            dtFreq.Columns.Add("ServiceName", typeof(string));
            dtFreq.Columns.Add("FreqMeasure", typeof(string));
            dtFreq.Columns.Add("FreqNo", typeof(Int16));
            bsFreq.DataSource = dtFreq;
            dgvFreq.DataSource = bsFreq;
            dgvFreqSetting();

            //Binding Master Record
            txtID.DataBindings.Add("Text", bsFile, "TypeID");
            txtDesc.DataBindings.Add("Text", bsFile, "TypeDesc");

            LoadRecords();
            DataGridSetting();
            BuildPrintItems();
            tsddbPrint.Enabled = false;

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

        }

        public static DataTable dtLoader(string SP)
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
            nMode = 0; nFMode = 0;
            
            dtEquipmentType = dtLoader("spGetEquipmentTypes");
            bsFile.DataSource = dtEquipmentType;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "TypeID<>0";

            DataGridSetting();
            FileAccess();

            if (tsddbSearch.DropDownItems.Count == 0)
            {
                Int16 i = 0;
                arrCol = new string[dtEquipmentType.Columns.Count];
                ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length];
                foreach (DataColumn colFile in dtEquipmentType.Columns)
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
        }

        private void LoadData()
        {
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            cboSrvcType.SelectedIndex = -1;
            cboUnit.SelectedIndex = -1;
            txtFreq.Text = "";
            ResetSrvcTypesDropdown();
            LoadFreq();
            if (dgvFreq.Rows.Count > 0)
                dgvFreq.CurrentCell = dgvFreq.Rows[0].Cells["ServiceName"];
            dgvFreq.Focus();
            AddFreqMode(0);
        }

        private void LoadFreq()
        {
            dtFreq = FreqLoader();
            if (dtFreq != null)
            {
                try
                {
                    bsFreq.DataSource = dtFreq;
                    dgvFreq.DataSource = bsFreq;
                    dgvFreqSetting();
                }
                catch { }
            }
        }

        private DataTable FreqLoader()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                ConnectionError();
                return null;
            }
            SqlCommand sqlcmd = new SqlCommand("spExEqptTypeFreq", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@ID", Convert.ToInt16(dgvFile.CurrentRow.Cells["TypeID"].Value.ToString()));
            try
            {
                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                DataTable dTable = new DataTable();
                dTable.Load(sqldr);
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                return dTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                return null;
            }
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["TypeDesc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvFile.Columns["TypeDesc"].HeaderText = "Equipment";
            dgvFile.Columns["TypeDesc"].Width = 250;
            dgvFile.Columns["TypeID"].Visible = false;
        }

        private void dgvFreqSetting()
        {
            dgvFreq.EnableHeadersVisualStyles = false;
            dgvFreq.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFreq.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFreq.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFreq.Columns["FreqNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFreq.Columns["FreqMeasure"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFreq.Columns["ServiceName"].HeaderText = "Service";
            dgvFreq.Columns["FreqNo"].HeaderText = "Frequency";
            dgvFreq.Columns["FreqMeasure"].HeaderText = "Unit";
            dgvFreq.Columns["ServiceName"].Width = 150;
            dgvFreq.Columns["FreqMeasure"].Width = 85;
            dgvFreq.Columns["FreqNo"].Width = 80;
            dgvFreq.Columns["ServiceType"].Visible = false;
        }

        private void BindingDetails()
        {
            cboSrvcType.DataBindings.Add("SelectedValue", bsFreq, "ServiceType");
            cboSrvcType.DataBindings.Add("Text", bsFreq, "ServiceName");
            cboUnit.DataBindings.Add("Text", bsFreq, "FreqMeasure");
            txtFreq.DataBindings.Add("Text", bsFreq, "FreqNo");
        }

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            OpenControls(this.pnlRecord, true);

            dtFreq.Rows.Clear();
            txtDesc.Text = "";
            txtDesc.Focus();
            AddFreqMode(1);
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();
            btnClose.Visible = false;
            LoadData();
            AddFreqMode(1);
            txtDesc.Focus();
        }

        private void SaveRecord()
        {
            Int16 nTID = 0;

            if (txtDesc.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Type Description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDesc.Focus();
                return;
            }            

            if (PSSClass.DataEntry.MatchingRecord("TypeID", "TypeDesc", "EquipmentType", txtDesc.Text, nMode, Convert.ToInt16(txtID.Text), " AND IsActive = 1") == true)
            {
                MessageBox.Show("Matching equipment found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDesc.Focus();
                return;
            }
            dgvFreq.Focus();
            //Check if any changes were made
            bsFile.EndEdit();
            DataTable dtE = dtEquipmentType.GetChanges();

            bsFreq.EndEdit();
            DataTable dtF = dtFreq.GetChanges();

            if (dtE == null && dtF == null)
            {
                pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
                nFMode = 0; nMode = 0;
                AddEditMode(false);
                FileAccess();
                return;
            }
            else if (dtE != null && dtF == null)
            {
                MessageBox.Show("Please enter service types details.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DataTable dtX = new DataTable();
            dtX = dtLoader("spGetEquipmentSrvcTypes");

            byte nDSw = 0;
            for (int i = 0; i < dtX.Rows.Count ; i++)
            {
                Int16 nC = 0;
                for (int j = 0; j < dgvFreq.Rows.Count; j++)
                {
                    if (dtX.Rows[i]["ServiceName"].ToString() == dgvFreq.Rows[j].Cells["ServiceName"].Value.ToString())
                    {
                        nC++;
                    }
                }
                if (nC > 1)
                {
                    nDSw = 1;
                    break;
                }
            }
            dtX.Dispose();
            if (nDSw == 1)
            {
                MessageBox.Show("Duplicate service type found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dtE != null)
                dtE.Dispose();
            if (dtF != null)
                dtF.Dispose();   

            //Create XML data from dgvFreq subtable
            string strXML = "";
            if (dgvFreq.Rows.Count != 0)
            {
                strXML = "<Frequency>";
                for (int i = 0; i < dgvFreq.Rows.Count; i++)
                {
                    if (dgvFreq.Rows[i].Cells[1].Value != null)
                    {
                        strXML = strXML + "<Srvc><ServiceType>" + PurgeString(dgvFreq.Rows[i].Cells["ServiceType"].Value.ToString()) + "</ServiceType>" +
                                          "<ServiceName>" + PurgeString(dgvFreq.Rows[i].Cells["ServiceName"].Value.ToString()) + "</ServiceName>" +
                                          "<FreqNo>" + PurgeString(dgvFreq.Rows[i].Cells["FreqNo"].Value.ToString()) + "</FreqNo>" +
                                          "<FreqMeasure>" + PurgeString(dgvFreq.Rows[i].Cells["FreqMeasure"].Value.ToString()) + "</FreqMeasure></Srvc>";
                    }
                }
                strXML = strXML + "</Frequency>";
            }
            if (nMode == 1)
                txtID.Text = PSSClass.General.NewID("EquipmentType", "TypeID").ToString();

            nTID = Convert.ToInt16(txtID.Text);
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@TypeID", Convert.ToInt16(txtID.Text));
            sqlcmd.Parameters.AddWithValue("@TypeDesc", txtDesc.Text);
            sqlcmd.Parameters.AddWithValue("@XMLData", strXML);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditEquipmentType";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("TypeID", nTID.ToString(), bsFile, dgvFile);
        }

        private void CancelSave()
        {
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            LoadRecords();
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
                    ConnectionError();
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@TypeID", Convert.ToInt16(txtID.Text));
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelEquipmentType";

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
            nMode = 0;
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];
            items[0] = new ToolStripMenuItem();
            items[0].Name = "PrintRpt";
            items[0].Text = "Print Report";
            items[0].Click += new EventHandler(PrintRpt);
            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void PrintRpt(object sender, EventArgs e)
        {
            //PrintEquipmentType rpt = new PrintEquipmentType();
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
                    bsFile.Filter = "TypeID<>0";
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
                SearchFilterClickHandler(null, null);
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
                cklColumns.Visible = true;
                cklColumns.BringToFront();
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
            LoadRecords();
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

        private void EquipmentType_KeyDown(object sender, KeyEventArgs e)
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

        public void ConnectionError()
        {
            MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please contact IT.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
        }

        //-----FREQUENCY SUBTABLE START------
        //Freq Controls
        private void btnAddFreq_Click(object sender, EventArgs e)
        {
            nFMode = 1;
            AddSrvcTypesDropdown();
            AddFreqMode(2);
            dgvFreq.Enabled = false;
            cboSrvcType.SelectedIndex = -1;
            cboUnit.SelectedIndex = -1;
            txtFreq.Text = "";
        }

        private void btnDeleteFreq_Click(object sender, EventArgs e)
        {
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this service?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
            try
            {
                dgvFreq.Rows.RemoveAt(dgvFreq.CurrentCell.RowIndex);
            }
            catch { }
            
            AddFreqMode(1);
        }

        private void btnOKFreq_Click(object sender, EventArgs e)
        {
            if (cboSrvcType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Service.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboSrvcType.DroppedDown = true;
                return;
            }

            if (cboUnit.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a Unit.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboUnit.DroppedDown = true;
                return;
            }

            if (txtFreq.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a Frequency.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtFreq.Focus();
                return;
            }
            int n = dtFreq.Rows.Count;

            foreach (DataGridViewRow Row in dgvFreq.Rows)
            {
                if (cboSrvcType.SelectedValue.ToString() == Row.Cells["ServiceType"].Value.ToString())
                    {
                    MessageBox.Show("Matching Service found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboSrvcType.DroppedDown = true;
                    return;
                }
            }

            DataRow dR = dtFreq.NewRow();
            dR["ServiceType"] = cboSrvcType.SelectedValue.ToString();
            dR["ServiceName"] = cboSrvcType.Text;
            dR["FreqMeasure"] = cboUnit.Text;
            dR["FreqNo"] = Convert.ToInt16(txtFreq.Text.Trim());
            dtFreq.Rows.Add(dR);
            bsFreq.DataSource = dtFreq;
            dgvFreq.DataSource = bsFreq;
            dgvFreqSetting();

            ResetSrvcTypesDropdown();
            AddFreqMode(1);
            nFMode = 0;
            dgvFreq.Enabled = true;
            if (dgvFreq.Rows.Count > 0)
                dgvFreq.CurrentCell = dgvFreq.Rows[0].Cells["ServiceName"];
        }

        private void btnCancelFreq_Click(object sender, EventArgs e)
        {
            ResetSrvcTypesDropdown();
            AddFreqMode(1);
            dgvFreq.Enabled = true;
            if (dgvFreq.Rows.Count > 0)
            {
                dgvFreq.CurrentCell = dgvFreq.Rows[0].Cells["ServiceName"];
            }
        }

        private void AddFreqMode(Int16 mode) // 0: View mode, 1: Edit mode, 2: Add/Delete mode
        {
            btnAddFreq.Visible = false;
            btnDeleteFreq.Visible = false;
            btnOKFreq.Visible = false;
            btnCancelFreq.Visible = false;
            cboSrvcType.Enabled = false;
            txtFreq.Enabled = false;
            cboUnit.Enabled = false;
            dgvFreq.Enabled = true;

            if (mode == 1)
            {
                btnAddFreq.Visible = true;
                if (dgvFreq.Rows.Count != 0)
                {
                    btnDeleteFreq.Visible = true;
                    cboSrvcType.Enabled = true;
                    txtFreq.Enabled = true;
                    cboUnit.Enabled = true;
                }
            }

            if (mode == 2)
            {
                btnOKFreq.Visible = true;
                btnCancelFreq.Visible = true;
                cboSrvcType.Enabled = true;
                txtFreq.Enabled = true;
                cboUnit.Enabled = true;
                dgvFreq.Enabled = false;
            }
        }

        //Validates txtFreq entry is an integer
        private void txtFreq_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //Validates ServiceType is unique
        private bool matchService()
        {
            string svStr = "";
            dgvFreq.Sort(dgvFreq.Columns["ServiceType"], ListSortDirection.Ascending);
            for (int i = 0; i < dgvFreq.Rows.Count; i++)
            {
                if (svStr == dgvFreq.Rows[i].Cells["ServiceType"].Value.ToString())
                {
                    return true;
                }
                svStr = dgvFreq.Rows[i].Cells["ServiceType"].Value.ToString();
            }
            return false;
        }

        //Handle Freq controls lose focus events
        private void cboSrvcType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (nFMode != 1)
                {
                    dgvFreq.Rows[dgvFreq.CurrentCell.RowIndex].Cells["ServiceName"].Value = cboSrvcType.Text;
                    dgvFreq.Rows[dgvFreq.CurrentCell.RowIndex].Cells["ServiceType"].Value = cboSrvcType.SelectedValue;
                }
            }
            catch { }
        }

        private void cboUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (nFMode != 1)
                {
                    dgvFreq.Rows[dgvFreq.CurrentCell.RowIndex].Cells["FreqMeasure"].Value = cboUnit.Text;
                }
            }
            catch { }
        }

        private void txtFreq_LostFocus(object sender, EventArgs e)
        {
            try
            {
                if (nFMode != 1)
                {
                    dgvFreq.Rows[dgvFreq.CurrentCell.RowIndex].Cells["FreqNo"].Value = txtFreq.Text;
                }
            }
            catch { }
        }

        private string PurgeString(string cStr)
        {
            string strX = "";
            if (cStr.Trim() != "")
            {
                strX = cStr.Replace("&", "&amp;");
                strX = strX.Replace(">", "&gt;");
                strX = strX.Replace("<", "&lt;");
                strX = strX.Replace("'", "&apos;");
                strX = strX.Replace("\"", "&quot;");
            }
            return strX;
        }

        private void ResetSrvcTypesDropdown()
        {
            DataTable dt = new DataTable();
            dt = dtLoader("spGetEquipmentSrvcTypes");
            cboSrvcType.DataSource = dt;
            cboSrvcType.ValueMember = "ServiceType";
            cboSrvcType.DisplayMember = "ServiceName";           
        }

        private void AddSrvcTypesDropdown()
        {
            DataTable dt = new DataTable();
            dt = dtLoader("spGetEquipmentSrvcTypes");

            DataTable dtX = new DataTable();
            dtX.Columns.Add("ServiceType", typeof(string));
            dtX.Columns.Add("ServiceName", typeof(string));

            byte nSw = 0;
            try
            {
                if (dgvFreq.Rows.Count == 0)
                {
                    cboSrvcType.DataSource = dt;
                    cboSrvcType.ValueMember = "ServiceType";
                    cboSrvcType.DisplayMember = "ServiceName";
                    cboSrvcType.SelectedIndex = -1;
                }
                else
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dgvFreq.Rows.Count; j++)
                        {
                            if (dt.Rows[i]["ServiceType"].ToString() == dgvFreq.Rows[j].Cells["ServiceType"].Value.ToString())
                            {
                                nSw = 0; break;
                            }
                            else
                            {
                                nSw = 1;
                            }
                        }
                        if (nSw == 1)
                        {
                            DataRow dR = dtX.NewRow();
                            dR["ServiceType"] = dt.Rows[i]["serviceType"];
                            dR["ServiceName"] = dt.Rows[i]["serviceName"];
                            dtX.Rows.Add(dR);
                        }
                        nSw = 0;
                    }
                    cboSrvcType.DataSource = dtX;
                    cboSrvcType.ValueMember = "ServiceType";
                    cboSrvcType.DisplayMember = "ServiceName";
                    cboSrvcType.SelectedIndex = -1;
                }
            }
            catch { }
        }

        private void dgvFreq_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                cboSrvcType.SelectedValue = dgvFreq.Rows[dgvFreq.CurrentCell.RowIndex].Cells["ServiceType"].Value.ToString();
                cboSrvcType.Text = dgvFreq.Rows[dgvFreq.CurrentCell.RowIndex].Cells["ServiceName"].Value.ToString();
                cboUnit.Text = dgvFreq.Rows[dgvFreq.CurrentCell.RowIndex].Cells["FreqMeasure"].Value.ToString();
                txtFreq.Text = dgvFreq.Rows[dgvFreq.CurrentCell.RowIndex].Cells["FreqNo"].Value.ToString();
            }
            catch { }
        }

        //-----FREQUENCY SUBTABLE END-----
    }
}
