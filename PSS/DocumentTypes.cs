using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;

namespace GIS
{
    public partial class DocumentTypes : GIS.TemplateForm
    {
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol = { };
        private int nIndex;
        private string strFileAccess = "RO";

        protected DataTable dtMaster = new DataTable();

        public DocumentTypes()
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
        }

        private void DocumentTypes_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            strFileAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "DocumentTypes");
            LoadRecords();
            BuildSearchItems();
            BuildPrintItems();

            dgvFile.Columns["DocTypeID"].HeaderText = "DOC. NO.";
            dgvFile.Columns["DocTypeName"].HeaderText = "DOC. NAME";
            dgvFile.Columns["DocSeqNo"].HeaderText = "SEQ. NO.";

            //Master File
            dtMaster.Columns.Add("DocTypeID", typeof(string));
            dtMaster.Columns.Add("DocTypeName", typeof(string));
            dtMaster.Columns.Add("DocSeqNo", typeof(Int16));
            dtMaster.Columns.Add("DateCreated", typeof(DateTime));
            dtMaster.Columns.Add("CreatedByID", typeof(Int16));
            dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            dtMaster.Columns.Add("LastUserID", typeof(Int16));
            bsMaster.DataSource = dtMaster;

            //Data Bindings
            txtID.DataBindings.Add("Text", bsMaster, "DocTypeID");
            txtName.DataBindings.Add("Text", bsMaster, "DocTypeName");
            txtSeqNo.DataBindings.Add("Text", bsMaster, "DocSeqNo");
        }

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = GISClass.CustomerService.DocTypesMaster();
            if (dt == null)
            {
                nMode = 9;
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            nMode = 0;
            dt.Dispose(); btnClose.Enabled = true;
            FileAccess();
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["DocTypeName"].HeaderText = "DOC. NAME";
            dgvFile.Columns["DocSeqNo"].HeaderText = "SEQ. NO";
            dgvFile.Columns["DocSeqNo"].Width = 150;
            dgvFile.Columns["DocSeqNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DocTypeName"].Width =300;
            dgvFile.Columns["DocTypeID"].Visible = false;
        }

        private void FileAccess()
        {
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsddbPrint.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true;
            }
            else
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false;
            }
            tsddbSearch.Enabled = true;
        }

        private void BuildSearchItems()
        {
            DataTable dtQ = new DataTable();
            dtQ = GISClass.CustomerService.DocTypesMaster();
            if (dtQ == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }

            int i = 0;
            int n = 0;

            arrCol = new string[dtQ.Columns.Count];

            ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

            foreach (DataColumn colFile in dtQ.Columns)
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
                if (dgvFile.Columns[i].Visible == true)
                    cklColumns.SetItemCheckState(i, CheckState.Checked);
                else
                    cklColumns.SetItemCheckState(i, CheckState.Unchecked);
                //}
                i += 1;
            }
            tsddbSearch.DropDownItems.AddRange(items);
            tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0].Text = "Master List";
            items[0].Click += new EventHandler(PrtMasterClickHandler);

            //items[1] = new ToolStripMenuItem();
            //items[1].Text = "GBL Master Study";
            //items[1].Click += new EventHandler(PrtGRMasterClickHandler);

            //items[2] = new ToolStripMenuItem();
            //items[2].Text = "Study Initiation";
            //items[2].Click += new EventHandler(PrtInitiationClickHandler);

            //items[3] = new ToolStripMenuItem();
            //items[3].Text = "Studies not Verified";
            //items[3].Click += new EventHandler(PrtNotVerifiedClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void PrtMasterClickHandler(object sender, EventArgs e)
        {

        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData();
                btnClose.Visible = true;
            }
        }

        private void LoadData()
        {
            //if (dgvFile.CurrentRow.Cells["DocDate"].Value.ToString() != "")
            //{
            //    DateTime dteCreated = Convert.ToDateTime(dgvFile.CurrentRow.Cells["DocDate"].Value.ToString());
            //    String strDteCteated = dteCreated.ToString("MM/dd/yyyy");
            //    mskDocDate.Text = strDteCteated;
            //}
            txtID.Text = dgvFile.CurrentRow.Cells["DocTypeID"].Value.ToString();
            dtMaster = GISClass.CustomerService.DocTypeRec(Convert.ToInt32(txtID.Text));
            if (dtMaster == null || dtMaster.Rows.Count == 0)
            {
                MessageBox.Show("No master record found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bsMaster.DataSource = dtMaster;
            OpenControls(pnlRecord, false);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
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

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            LoadRecords();
            bsFile.Filter = "DocTypeID <> 0";
        }

        public void SearchOKClickHandler(object sender, EventArgs e)
        {
            try
            {
                bsFile.Filter = "DocTypeID <> 0";
                GISClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                dgvFile.Select();
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
                    if (chkFullText.Checked == true)
                        bsFile.Filter = tstbSearchField.Text + "='" + strSearch + "'";
                    else
                        bsFile.Filter = tstbSearchField.Text + " LIKE '%" + strSearch + "%'";
                }
                else if (arrCol[nIndex] == "System.DateTime")
                {
                    bsFile.Filter = tstbSearchField.Text + " = '" + tstbSearch.Text + "'";
                }
                else
                {
                    bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;
                }
                dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                tsbRefresh.Enabled = true;
            }
            catch
            {
            }
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

        private void SearchKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SearchFilterClickHandler(null, null);
            }
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

                FileAccess();
            }
            catch { }
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
            else if (e.KeyChar == 8)
            {
                tstbSearch.Text = tstbSearch.Text.Substring(0, tstbSearch.TextLength - 1);
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

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            if (dgvFile.Rows.Count > 0)
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
            nMode = 0;
            this.Close();
        }
        
        private void AddRecord()
        {
            nMode = 1;
            ClearControls(pnlRecord); OpenControls(pnlRecord, true);
            AddEditMode(true); tsbRefresh.Enabled = false;
            dtMaster.Rows.Clear();
            txtID.Text = "(New)";
            txtID.ReadOnly = false;
            DataRow dr;
            dr = dtMaster.NewRow();
            dr["DocTypeID"] = 0;
            dr["DocTypeName"] = DBNull.Value;
            dr["DocSeqNo"] = DBNull.Value;
            dr["DateCreated"] = DBNull.Value;
            dr["CreatedByID"] = DBNull.Value;
            dr["LastUpdate"] = DBNull.Value;
            dr["LastUserID"] = DBNull.Value;
            dtMaster.Rows.Add(dr);
            bsMaster.DataSource = dtMaster;

            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;
            nMode = 2;
            AddEditMode(true); tsbRefresh.Enabled = false;
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();
            LoadData();
            OpenControls(pnlRecord, true);
            txtID.ReadOnly = true;
            btnClose.Visible = false;
        }

        private void DeleteRecord()
        {
        }

        private void SaveRecord()
        {
            int nID = 1;
            if (nMode == 1)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered. Please contact your IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand("SELECT MAX(" + "DocTypeID" + ") FROM " + "DocumentTypes", sqlcnn);
                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    if (sqldr.GetValue(0).ToString() == "")
                        nID = 1;
                    else
                        nID = Convert.ToInt32(sqldr.GetValue(0)) + 1;
                }
                sqldr.Close(); sqlcmd.Dispose();
                txtID.Text = nID.ToString();
                SqlDataAdapter da = new SqlDataAdapter("SELECT DocTypeID, DocTypeName, DocSeqNo," +
                                                       "DateCreated, CreatedByID, LastUpdate, LastUserID " +
                                                       "FROM DocumentTypes", sqlcnn);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);
                cmdBuilder.GetInsertCommand();
                dtMaster.Rows[bsMaster.Position]["DocTypeID"] = Convert.ToInt32(txtID.Text);
                dtMaster.Rows[bsMaster.Position]["DocTypeName"] = txtName.Text.Trim();
                dtMaster.Rows[bsMaster.Position]["DocSeqNo"] = Convert.ToInt16(txtSeqNo.Text);
                dtMaster.Rows[bsMaster.Position]["DateCreated"] = DateTime.Now;
                dtMaster.Rows[bsMaster.Position]["CreatedByID"] = LogIn.nUserID;
                dtMaster.Rows[bsMaster.Position]["LastUpdate"] = DateTime.Now;
                dtMaster.Rows[bsMaster.Position]["LastUserID"] = LogIn.nUserID;
                bsMaster.EndEdit();
                da.Update(dtMaster);
                sqlcnn.Close(); sqlcnn.Dispose();
            }
            else
            {
                bsMaster.EndEdit();
                DataTable dtU = dtMaster.GetChanges(DataRowState.Modified);
                if (dtU != null && dtU.Rows.Count > 0)
                {
                    SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                    if (sqlcnn == null)
                    {
                        MessageBox.Show("Connection problems encountered. Please contact your IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    SqlDataAdapter da = new SqlDataAdapter("SELECT DocTypeID, DocTypeName, DocSeqNo," +
                                                           "DateCreated, CreatedByID, LastUpdate, LastUserID " +
                                                           "FROM DocumentTypes", sqlcnn);
                    SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);
                    cmdBuilder.GetUpdateCommand();
                    dtMaster.Rows[bsMaster.Position]["LastUpdate"] = DateTime.Now;
                    dtMaster.Rows[bsMaster.Position]["LastUserID"] = LogIn.nUserID;
                    da.Update(dtMaster);
                    da.Dispose(); cmdBuilder.Dispose();
                    sqlcnn.Close(); sqlcnn.Dispose();
                    dtU.Dispose();
                }
                //else
                //{
                //    DataTable dtX = dtMaster.GetChanges(DataRowState.Deleted);
                //    if (dtX.Rows.Count > 0)
                //    {
                //        SqlConnection sqlcnn = GRMSClass.DBConnection.GRMSConnection();
                //        if (sqlcnn == null)
                //        {
                //            MessageBox.Show("Connection problems encountered. Please contact your IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //            return;
                //        }
                //        SqlDataAdapter da = new SqlDataAdapter("SELECT DocTypeID, ProtocolNo, SponsorID, StudyDirID, " +
                //                                               "TestArticle, TestSystem, TestService, DocTypeIDtes, DateInitiated, DateCompleted," +
                //                                               "DateReported, DateCancelled, DateArchived, Verified," +
                //                                               "DateCreated, CreatedByID, LastUpdate, LastUserID " +
                //                                               "FROM GRMaster", sqlcnn);
                //        SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);
                //        cmdBuilder.GetDeleteCommand();
                //        da.Update(dtMaster);
                //        da.Dispose(); cmdBuilder.Dispose();
                //        sqlcnn.Close(); sqlcnn.Dispose();
                //    }
                //}
            }

            AddEditMode(false); tsbRefresh.Enabled = true;
            LoadRecords();
            GISClass.General.FindRecord("DocTypeID", txtID.Text, bsFile, dgvFile);
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront();
            nMode = 0; btnClose.Visible = true;
            LoadData();
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
            AddEditMode(false); tsbRefresh.Enabled = true;
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); bnFile.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0;
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
        }
    }
}
