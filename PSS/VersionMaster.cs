﻿using System;
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
    public partial class VersionMaster : PSS.TemplateForm
    {
        private byte nMode = 0; //switch for Add or Edit Mode, 1 - Add New Record, 2 - Edit Record

        private bool mouseDown;// for dragging and dropping data form panel (pnlRecord)
        private Point mousePos;// for dragging and dropping data form panel (pnlRecord)
        private string[] arrCol;// for record search dropdown data fields container
        private int nIndex;//index holder for currently selected row in master datagridview (dgvFile)
        private int nCtr = 0;//counter for keypress search functionality on master datagridgriew (dgvFile)
        private int nSw = 0;//switch for keypress search functionality on master datagridgriew (dgvFile)
        private string strFileAccess = "RO";//user data form access, default value

        protected DataTable dtMaster = new DataTable();

        public VersionMaster()
        {
            InitializeComponent();
            //Record Navigation
            bnMoveFirst.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveLast.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveNext.Click += new EventHandler(MoveRecordClickHandler);
            bnMovePrevious.Click += new EventHandler(MoveRecordClickHandler);
            //
            //File Maintenance Commands
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
            //
            //Master Datagridview Events Handlers
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            //
            //Hiding/Unhiding Datagridview Columns
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
            //
            //Display Option for Master Datagridview (optional)
            chkShowInactive.Click += new EventHandler(chkShowInactiveClickHandler);
        }

        private void VersionMaster_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "VersionMaster");
            LoadRecords();
            BuildPrintItems();
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
            chkShowInactive.Visible = true;
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            dtMaster.Columns.Add("SystemName", typeof(String));
            dtMaster.Columns.Add("VersionNo", typeof(String));
            dtMaster.Columns.Add("VersionNotes", typeof(String));
            dtMaster.Columns.Add("VersionDate", typeof(DateTime));
            dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            dtMaster.Columns.Add("DateCreated", typeof(DateTime));
            dtMaster.Columns.Add("CreatedByID", typeof(Int16));
            dtMaster.Columns.Add("LastUserID", typeof(Int16));
            bsMaster.DataSource = dtMaster;
            ControlDataBindings();
        }

        private void ControlDataBindings()
        {
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            cboSystems.DataBindings.Add("Text", bsMaster, "SystemName", true);
            txtVno.DataBindings.Add("Text", bsMaster, "VersionNo", true);
            txtNotes.DataBindings.Add("Text", bsMaster, "VersionNotes", true);
            this.dtpDateReleased.DataBindings.Add("Value", bsMaster, "VersionDate", true);
        }
        private void LoadRecords()
        {
            DataTable dt = PSSClass.Versions.VersionMaster();
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "VersionNo <> ''";
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
            dgvFile.Columns["SystemName"].HeaderText = "SYSTEM NAME";
            dgvFile.Columns["SystemName"].Width = 150;
            dgvFile.Columns["VersionNo"].HeaderText = "VERSION NO";
            dgvFile.Columns["VersionNo"].Width = 150;
            dgvFile.Columns["VersionNotes"].HeaderText = "NOTES";
            dgvFile.Columns["VersionNotes"].Width = 350;
            dgvFile.Columns["VersionNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["VersionDate"].HeaderText = "VERSION DATE";
            dgvFile.Columns["VersionDate"].Width = 150;
            dgvFile.Columns["VersionDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["VersionDate"].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm:ss tt";
            dgvFile.Columns["VersionNotes"].Width = 350;
            dgvFile.Columns["DateCreated"].Visible = false;
            dgvFile.Columns["CreatedByID"].Visible = false;
            dgvFile.Columns["LastUpdate"].Visible = false;
            dgvFile.Columns["LastUserID"].Visible = false;
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[6];

            //items[0] = new ToolStripMenuItem();
            //items[0].Name = "ServiceCode";
            //items[0].Text = "Sorted by Service Code";
            //items[0].Click += new EventHandler(PrintSC);

            //items[1] = new ToolStripMenuItem();
            //items[1].Name = "ServiceDesc";
            //items[1].Text = "Sorted by Service Description";
            //items[1].Click += new EventHandler(PrintSCDesc);

            //items[2] = new ToolStripMenuItem();
            //items[2].Name = "ServiceDept";
            //items[2].Text = "Grouped by Department";
            //items[2].Click += new EventHandler(PrintSCDept);

            //items[3] = new ToolStripMenuItem();
            //items[3].Name = "ServiceDept";
            //items[3].Text = "Grouped by Duration";
            //items[3].Click += new EventHandler(PrintSCDuration);

            //items[4] = new ToolStripMenuItem();
            //items[4].Name = "ServiceGLCode";
            //items[4].Text = "Grouped by GL Code";
            //items[4].Click += new EventHandler(PrintSCGLCode);

            //items[5] = new ToolStripMenuItem();
            //items[5].Name = "ServiceInactive";
            //items[5].Text = "Inactive Service Codes";
            //items[5].Click += new EventHandler(PrintSCInactive);

            //tsddbPrint.DropDownItems.AddRange(items);
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;

            dtMaster.Rows.Clear();
            DataRow dR = dtMaster.NewRow();
            dR["SystemName"] = dgvFile.CurrentRow.Cells["SystemName"].Value;
            dR["VersionNo"] = dgvFile.CurrentRow.Cells["VersionNo"].Value;
            dR["VersionNotes"] = dgvFile.CurrentRow.Cells["VersionNotes"].Value;
            dR["VersionDate"] = dgvFile.CurrentRow.Cells["VersionDate"].Value;
            dR["DateCreated"] = dgvFile.CurrentRow.Cells["DateCreated"].Value;
            dR["CreatedByID"] = dgvFile.CurrentRow.Cells["CreatedByID"].Value;
            dR["LastUpdate"] = dgvFile.CurrentRow.Cells["LastUpdate"].Value;
            dR["LastUserID"] = dgvFile.CurrentRow.Cells["LastUserID"].Value;
            dtMaster.Rows.Add(dR);
            dtMaster.AcceptChanges();
            bsMaster.DataSource = dtMaster;
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

        private void AddRecord()
        {
            nMode = 1;
            AddEditMode(true); tsbDelete.Enabled = false;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false; tsbRefresh.Enabled = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            cboSystems.Enabled = true; 
            txtVno.Enabled = true;
            txtVno.Focus();
            dtMaster.Rows.Clear();
            DataRow dR = dtMaster.NewRow();
            dR["SystemName"] = "";
            dR["VersionNo"] = "";
            dR["VersionNotes"] = "";
            dR["VersionDate"] = DateTime.Now;
            dR["CreatedByID"] = 1;
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = 1;
            dtMaster.Rows.Add(dR);
        }

        private void EditClickHandler(object sender, EventArgs e)
        {
            EditRecord();
        }

        private void DeleteClickHandler(object sender, EventArgs e)
        {
            DeleteRecord();
        }

        private void EditRecord()
        {
            nMode = 2;
            AddEditMode(true); tsbDelete.Enabled = false;
            OpenControls(this.pnlRecord, true);
            LoadData();
            btnClose.Visible = false; dtpDateReleased.Focus(); tsbRefresh.Enabled = false;
            cboSystems.Enabled = false; txtVno.Enabled = false;
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

                sqlcmd.Parameters.AddWithValue("@VerNo", txtVno.Text);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelVerMaster";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
                    AddEditMode(false);//Initialize Toolbar

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Problem encountered: " + ex.Message + Environment.NewLine + "Record is not deleted!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    return;
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            ClearControls(this);
            LoadRecords();
            bsFile.Filter = "VersionNo <> ''";
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false); tsbDelete.Enabled = true;
        }


        private void SaveClickHandler(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void SaveRecord()
        {
            if (txtVno.Text.Trim() == "")
            {
                MessageBox.Show("Please enter service description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtVno.Focus();
                return;
            }

            bsMaster.EndEdit();
            DataTable dt = dtMaster.GetChanges();
            if (dt == null)
            {
                MessageBox.Show("No data to be saved.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            dt = null;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlDataAdapter da = new SqlDataAdapter("Select SystemName, VersionNo, VersionDate, VersionNotes, LastUpdate,DateCreated,CreatedByID,LastUserID from [VersionMaster]", sqlcnn);
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);

            dt = dtMaster.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                try
                {
                    cmdBuilder.GetInsertCommand();
                    da.Update(dtMaster);
                }
                catch { }
            }
            dt = null;

            dt = dtMaster.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                dtMaster.Rows[0]["LastUpdate"] = DateTime.Now;
                dtMaster.Rows[0]["LastUserID"] = 1;
                cmdBuilder.GetUpdateCommand();
                da.Update(dtMaster);
                dt.Dispose();
            }
            da.Dispose(); cmdBuilder.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();

            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false); //Initialize Toolbar
            LoadRecords();
            PSSClass.General.FindRecord("SystemName", cboSystems.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            nMode = 0;
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
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
            AddEditMode(false); //Initialize Toolbar
            LoadRecords();
            bsFile.Filter = "VersionNo <> ''";
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            nMode = 0;
        }

        private void CloseClickHandler(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
            OpenControls(this.pnlRecord, false);
            LoadData();
        }

        private void chkShowInactiveClickHandler(object sender, EventArgs e)
        {
            //LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
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

        private void cklItemChkEventHandler(object sender, EventArgs e)
        {
            if (cklColumns.Items.Count == 1)
            {
                if (cklColumns.GetItemCheckState(0) == CheckState.Checked)
                {
                    cklColumns.SetItemCheckState(0, CheckState.Checked);
                }
                else
                {
                    cklColumns.SetItemCheckState(0, CheckState.Unchecked);
                }
                return;
            }

            string strCol = cklColumns.Items[cklColumns.SelectedIndex].ToString().Replace(" ", "");
            if (dgvFile.Columns[strCol].Visible == true)
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = false;
            else
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = true;
            cklColumns.Visible = false;
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
                bsFile.Filter = "VersionNo <> ''";
                PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                dgvFile.Select();
                if (pnlRecord.Visible == true)
                    LoadData();
            }
            catch { }
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
            //LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
            LoadRecords();
            tsbRefresh.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
            AddEditMode(false);//Initialize Toolbar
            FileAccess();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr += 1;
            if (nCtr > 1)
            {
                DataTable dt = new DataTable();
                dt = PSSClass.Versions.VersionMaster();
                PSSClass.DataEntry.DGVSearch(tstbSearchField.Text, tstbSearch.Text, dt, bsFile);
                nCtr = 0;
                tstbSearch.Text = "";
                timer1.Enabled = false;
                nSw = 0;
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

        private void pnlRecord_MouseDown(object sender, MouseEventArgs e)
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

        private void lblHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
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

        private void VersionMaster_KeyDown(object sender, KeyEventArgs e)
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
