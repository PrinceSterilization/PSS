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
    public partial class DocumentTracking : GIS.TemplateForm
    {
        private byte nMode = 0;

        private string[] arrCol = { };
        private int nIndex;
        private string strFileAccess = "RO";
        private string strGroup = "";
        private string strRptNo = "";

        protected DataTable dtSponsors = new DataTable();
        protected DataTable dtContacts = new DataTable();
        protected DataTable dtMaster = new DataTable();
        protected DataTable dtDocTypes = new DataTable();

        public DocumentTracking()
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
            txtSponsor.GotFocus += new EventHandler(txtSponsorEnterHandler);
        }

        private void DocumentTracking_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            strFileAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "DocumentTracking");
            strGroup = GISClass.Users.UserGroupCode(LogIn.nUserID);

            LoadRecords();
            LoadSponsorsDDL();
            LoadDocTypes();
            BuildSearchItems();
            BuildPrintItems();

            dgvFile.Columns["DocNo"].HeaderText = "DOC. NO.";
            dgvFile.Columns["DocDate"].HeaderText = "DATE CREATED";
            dgvFile.Columns["DocDesc"].HeaderText = "DOCUMENT DESCRIPTION";
            dgvFile.Columns["CompanyName"].HeaderText = "COMPANY NAME";
            dgvFile.Columns["Contact"].HeaderText = "CONTACT NAME";
            dgvFile.Columns["DocTypeName"].HeaderText = "DOCUMENT TYPE";
            dgvFile.Columns["DocCreator"].HeaderText = "CREATED BY";
            dgvFile.Columns["CompanyID"].Visible = false;
            dgvFile.Columns["DocPath"].Visible = false;

            //Master File
            dtMaster.Columns.Add("DocNo", typeof(string));
            dtMaster.Columns.Add("DocDate", typeof(DateTime));
            dtMaster.Columns.Add("DocDesc", typeof(string));
            dtMaster.Columns.Add("CompanyID", typeof(Int16));
            dtMaster.Columns.Add("CompanyName", typeof(string));
            dtMaster.Columns.Add("Contact", typeof(string));
            dtMaster.Columns.Add("DocTypeID", typeof(string));
            dtMaster.Columns.Add("DocPath", typeof(string));
            dtMaster.Columns.Add("DateMailed", typeof(DateTime));
            dtMaster.Columns.Add("DateExpires", typeof(DateTime));
            dtMaster.Columns.Add("DateReturned", typeof(DateTime));
            dtMaster.Columns.Add("TrackDoc", typeof(bool));
            dtMaster.Columns.Add("DocCreator", typeof(string));
            dtMaster.Columns.Add("DateCreated", typeof(DateTime));
            dtMaster.Columns.Add("CreatedByID", typeof(Int16));
            dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            dtMaster.Columns.Add("LastUserID", typeof(Int16));
            bsMaster.DataSource = dtMaster;

            //Data Bindings
            txtDocNo.DataBindings.Add("Text", bsMaster, "DocNo");
            txtSponsorID.DataBindings.Add("Text", bsMaster, "CompanyID");
            txtSponsor.DataBindings.Add("Text", bsMaster, "CompanyName");
            txtDocDesc.DataBindings.Add("Text", bsMaster, "DocDesc");
            txtContact.DataBindings.Add("Text", bsMaster, "Contact");
            txtDocPath.DataBindings.Add("Text", bsMaster, "DocPath");
            cboDocTypes.DataBindings.Add("SelectedValue", bsMaster, "DocTypeID");
            txtCreatedBy.DataBindings.Add("Text", bsMaster, "DocCreator");

            Binding DocDateBinding;
            DocDateBinding = new Binding("Text", bsMaster, "DocDate");
            DocDateBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskDocDate.DataBindings.Add(DocDateBinding);

            Binding DateMailedBinding;
            DateMailedBinding = new Binding("Text", bsMaster, "DateMailed");
            DateMailedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskDateMailed.DataBindings.Add(DateMailedBinding);

            Binding DateExpiresBinding;
            DateExpiresBinding = new Binding("Text", bsMaster, "DateExpires");
            DateExpiresBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskDateExp.DataBindings.Add(DateExpiresBinding);

            Binding DateReturnedBinding;
            DateReturnedBinding = new Binding("Text", bsMaster, "DateReturned");
            DateReturnedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskDateRet.DataBindings.Add(DateReturnedBinding);
        }

        private void DateBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "__/__/____";
        }

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = GISClass.CustomerService.DocTrackingMaster();
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

        private void LoadDocTypes()
        {
            cboDocTypes.DataSource = null;

            dtDocTypes = GISClass.CustomerService.DocTypesAll();
            if (dtDocTypes == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            if (dtDocTypes.Rows.Count == 0)
                return;

            cboDocTypes.DataSource = dtDocTypes;
            cboDocTypes.DisplayMember = "DocTypeName";
            cboDocTypes.ValueMember = "DocTypeID";
        }

        private void LoadData()
        {
            //if (dgvFile.CurrentRow.Cells["DocDate"].Value.ToString() != "")
            //{
            //    DateTime dteCreated = Convert.ToDateTime(dgvFile.CurrentRow.Cells["DocDate"].Value.ToString());
            //    String strDteCteated = dteCreated.ToString("MM/dd/yyyy");
            //    mskDocDate.Text = strDteCteated;
            //}
            txtDocNo.Text = dgvFile.CurrentRow.Cells["DocNo"].Value.ToString();
            txtDocNoDisp.Text = dgvFile.CurrentRow.Cells["DocNo"].Value.ToString();
            dtMaster = GISClass.CustomerService.DocTrackingRec(Convert.ToInt32(txtDocNo.Text));
            if (dtMaster == null || dtMaster.Rows.Count == 0)
            {
                MessageBox.Show("No master record found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bsMaster.DataSource = dtMaster;
            OpenControls(pnlRecord, false);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            pnlCalendar.Visible = false; dgvSponsors.Visible = false; dgvContacts.Visible = false;
            //cboDocTypes.SelectedItem = dgvFile.CurrentRow.Cells["DocTypeName"].Value.ToString();
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["DocNo"].HeaderText = "DOC. NO.";
            dgvFile.Columns["DocDate"].HeaderText = "DATE CREATED";
            dgvFile.Columns["DocDesc"].HeaderText = "DOCUMENT DESCRIPTION";
            dgvFile.Columns["CompanyName"].HeaderText = "COMPANY NAME";
            dgvFile.Columns["Contact"].HeaderText = "CONTACT NAME";
            dgvFile.Columns["DocTypeName"].HeaderText = "DOCUMENT TYPE";
            dgvFile.Columns["DocNo"].Width = 80;
            dgvFile.Columns["DocNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DocDate"].Width = 90;
            dgvFile.Columns["DocDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DocDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DocTypeName"].Width = 100;
            dgvFile.Columns["DocDesc"].Width = 300;
            dgvFile.Columns["CompanyName"].Width = 300;
            dgvFile.Columns["Contact"].Width = 200;
            dgvFile.Columns[0].Frozen = true;
        }

        private void LoadSponsorsDDL()
        {
            dgvSponsors.DataSource = null;

            dtSponsors = GISClass.Sponsors.SponsorNamesDDL();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvSponsors.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 360;
            dgvSponsors.Columns[1].Visible = false;
        }

        private void LoadContactsDDL(int cSpID)
        {
            dgvContacts.DataSource = null;

            dtContacts = GISClass.Sponsors.ContactsDDL(cSpID);
            if (dtContacts == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvContacts.DataSource = dtContacts;
            StandardDGVSetting(dgvContacts);
            dgvContacts.Columns[0].Width = 183;
            dgvContacts.Columns[1].Visible = false;
        }

        private void BuildSearchItems()
        {
            //DataTable dtQ = new DataTable();
            //dtQ = GISClass.CustomerService.DocumentMaster();
            //if (dtQ == null)
            //{
            //    MessageBox.Show("Connection problems. Please contact your system administrator.");
            //    return;
            //}

            //int i = 0;
            //int n = 0;

            //arrCol = new string[dtMaster.Columns.Count];

            //ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

            //foreach (DataColumn colFile in dtMaster.Columns)
            //{
            //    items[i] = new ToolStripMenuItem();
            //    items[i].Name = colFile.ColumnName;

            //    //Using LINQ to insert space between capital letters
            //    var val = colFile.ColumnName;
            //    val = string.Concat(val.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

            //    items[i].Text = val;
            //    items[i].Click += new EventHandler(SearchItemClickHandler);
            //    arrCol[i] = colFile.DataType.ToString();
            //    cklColumns.Items.Add(val);
            //    if (dgvFile.Columns[i].Visible == true)
            //        cklColumns.SetItemCheckState(i, CheckState.Checked);
            //    else
            //        cklColumns.SetItemCheckState(i, CheckState.Unchecked);
            //    //}
            //    i += 1;
            //}
            //dtMaster.Dispose();
            //tsddbSearch.DropDownItems.AddRange(items);
            //tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            //tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;            
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0].Text = "Expiring Documents";
            items[0].Click += new EventHandler(PrtExpDocClickHandler);

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

        private void PrtExpDocClickHandler(object sender, EventArgs e)
        {
            MgmtRpts rpt = new MgmtRpts();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "DocExpiring";
            rpt.Show();
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

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            LoadRecords();
            bsFile.Filter = "DocNo <> 0";
        }

        public void SearchOKClickHandler(object sender, EventArgs e)
        {
            try
            {
                bsFile.Filter = "DocNo <> 0";
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

        private void AddRecord()
        {
            nMode = 1;
            ClearControls(pnlRecord); OpenControls(pnlRecord, true);
            AddEditMode(true); tsbRefresh.Enabled = false;
            dtMaster.Rows.Clear();
            txtDocNoDisp.Text = "(New)";
            txtDocNoDisp.ReadOnly  = true;
            DataRow dr;
            dr = dtMaster.NewRow();
            dr["DocNo"] = 0;
            dr["DocDate"] = DateTime.Now;
            dr["DocDesc"] = DBNull.Value;
            dr["DocPath"] = DBNull.Value;
            dr["CompanyID"] = DBNull.Value;
            dr["CompanyName"] = DBNull.Value;
            dr["Contact"] = DBNull.Value;
            dr["DocTypeID"] = 0;
            dr["DateMailed"] = DBNull.Value;
            dr["DateExpires"] = DBNull.Value;
            dr["DateReturned"] = DBNull.Value;
            dr["TrackDoc"] = true;
            dr["DocCreator"] = DBNull.Value;
            dr["DateCreated"] = DBNull.Value;
            dr["CreatedByID"] = DBNull.Value;
            dr["LastUpdate"] = DBNull.Value;
            dr["LastUserID"] = DBNull.Value;
            dtMaster.Rows.Add(dr);
            bsMaster.DataSource = dtMaster;

            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            pnlCalendar.Visible = false; dgvSponsors.Visible = false; dgvContacts.Visible = false;
            txtDocNo.ReadOnly = true;
            txtCreatedBy.Text = LogIn.strUserID; txtCreatedBy.ReadOnly = true;
            cboDocTypes.SelectedIndex = -1;
            cboDocTypes.Select();
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;
            nMode = 2;
            AddEditMode(true); tsbRefresh.Enabled = false;
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();
            pnlCalendar.Visible = false; dgvSponsors.Visible = false; dgvContacts.Visible = false;
            LoadData();
            OpenControls(pnlRecord, true);
            txtDocNoDisp.ReadOnly = true;
            btnClose.Visible = false;
            if (txtSponsorID.Text != "")
                LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
        }

        private void DeleteRecord()
        {
        }

        private void SaveRecord()
        {
            txtDocNo.Select();
            string strDateExp = "", strDateMailed = "", strDateRet = "";
            try
            {
                DateTime.Parse(mskDateMailed.Text);
                strDateMailed = mskDateExp.Text;
            }
            catch
            {
                MessageBox.Show("Please enter date mailed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mskDateMailed.Focus();
                return;
            }
            try
            {
                DateTime.Parse(mskDateExp.Text);
                strDateExp = mskDateExp.Text;
            }
            catch 
            {
                MessageBox.Show("Please enter date of expiration.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                mskDateExp.Focus();
                return;
            }
            try
            {
                DateTime.Parse(mskDateRet.Text);
                strDateRet = mskDateRet.Text;
            }
            catch { }
            if (txtSponsor.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Sponsor/Company Name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsor.Focus();
                return;
            }
            if (txtContact.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Contact name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsor.Focus();
                return;
            }
            if (cboDocTypes.SelectedIndex == -1)
            {
                MessageBox.Show("Please select document type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboDocTypes.Focus();
                return;
            }
            int nID = 1;
            if (nMode == 1)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered. Please contact your IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand("SELECT MAX(" + "DocNo" + ") FROM " + "DocumentMaster", sqlcnn);
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
                txtDocNo.Text = nID.ToString();
                SqlDataAdapter da = new SqlDataAdapter("SELECT DocNo, DocTypeID, DocDate, DocDesc, DocPath, CompanyID, CompanyName, Contact, " +
                                                       "DateMailed, DateExpires, DateReturned, TrackDoc, " + 
                                                       "DateCreated, CreatedByID, LastUpdate, LastUserID " +
                                                       "FROM DocumentMaster", sqlcnn);
                SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);
                cmdBuilder.GetInsertCommand();
                dtMaster.Rows[bsMaster.Position]["DocNo"] = Convert.ToInt32(txtDocNo.Text);
                dtMaster.Rows[bsMaster.Position]["DocTypeID"] = Convert.ToInt16(cboDocTypes.SelectedValue);
                dtMaster.Rows[bsMaster.Position]["DocDate"] = Convert.ToDateTime(mskDocDate.Text);
                dtMaster.Rows[bsMaster.Position]["DocDesc"] = txtDocDesc.Text;
                dtMaster.Rows[bsMaster.Position]["DocPath"] = txtDocPath.Text;
                dtMaster.Rows[bsMaster.Position]["CompanyID"] = Convert.ToInt16(txtSponsorID.Text);
                dtMaster.Rows[bsMaster.Position]["CompanyName"] = txtSponsor.Text;
                dtMaster.Rows[bsMaster.Position]["Contact"] = txtContact.Text.Trim();
                if (strDateMailed != "")
                    dtMaster.Rows[bsMaster.Position]["DateMailed"] = Convert.ToDateTime(mskDateMailed.Text);
                else
                    dtMaster.Rows[bsMaster.Position]["DateMailed"] = DBNull.Value;
                if (strDateExp != "")
                    dtMaster.Rows[bsMaster.Position]["DateExpires"] = Convert.ToDateTime(mskDateExp.Text);
                else
                    dtMaster.Rows[bsMaster.Position]["DateExpires"] = DBNull.Value;
                if (strDateRet != "")
                    dtMaster.Rows[bsMaster.Position]["DateReturned"] = Convert.ToDateTime(mskDateRet.Text);
                else
                    dtMaster.Rows[bsMaster.Position]["DateReturned"] = DBNull.Value;
                dtMaster.Rows[bsMaster.Position]["TrackDoc"] = true;
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
                    int nSID = GISClass.Sponsors.SpID(txtSponsor.Text);
                    if (nSID == 0)
                    {
                        dtMaster.Rows[bsMaster.Position]["CompanyID"] = DBNull.Value;
                        bsMaster.EndEdit();
                    }

                    SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                    if (sqlcnn == null)
                    {
                        MessageBox.Show("Connection problems encountered. Please contact your IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    SqlDataAdapter da = new SqlDataAdapter("SELECT DocNo, DocTypeID, DocDate, DocDesc, DocPath, CompanyID, CompanyName, Contact, " +
                                                           "DateMailed, DateExpires, DateReturned, TrackDoc, " +
                                                           "DateCreated, CreatedByID, LastUpdate, LastUserID " +
                                                           "FROM DocumentMaster", sqlcnn);
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
                //        SqlDataAdapter da = new SqlDataAdapter("SELECT DocNo, ProtocolNo, SponsorID, StudyDirID, " +
                //                                               "TestArticle, TestSystem, TestService, DocNotes, DateInitiated, DateCompleted," +
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
            GISClass.General.FindRecord("DocNo", txtDocNo.Text, bsFile, dgvFile);
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
            dgvSponsors.Visible = false;
        }

        private void mskDocDate_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.BringToFront();
                pnlCalendar.Left = mskDocDate.Width + mskDocDate.Left + 1;
                pnlCalendar.Top = mskDocDate.Top;
            }
        }

        private void mskDateMailed_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.BringToFront();
                pnlCalendar.Left = mskDateMailed.Width + mskDateMailed.Left + 1;
                pnlCalendar.Top = mskDateMailed.Top + mskDateMailed.Height - pnlCalendar.Height;
            }
        }

        private void mskDateExp_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.BringToFront();
                pnlCalendar.Left = mskDateExp.Width + mskDateExp.Left + 1;
                pnlCalendar.Top = mskDateExp.Top + mskDateExp.Height - pnlCalendar.Height;
            }
        }

        private void mskDateRet_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.BringToFront();
                pnlCalendar.Left = mskDateRet.Width + mskDateRet.Left + 1;
                pnlCalendar.Top = mskDateRet.Top + mskDateRet.Height - pnlCalendar.Height;
            }
        }

        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (pnlCalendar.Top == mskDocDate.Top)
            {
                mskDocDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
                mskDocDate.Select();
            }
            else if (pnlCalendar.Top == mskDateMailed.Top + mskDateMailed.Height - pnlCalendar.Height)
            {
                mskDateMailed.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
                //mskDateMailed.Select();
            }
            else if (pnlCalendar.Top == mskDateExp.Top + mskDateExp.Height - pnlCalendar.Height)
            {
                mskDateExp.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
                //mskDateExp.Select();
            }
            else if (pnlCalendar.Top == mskDateRet.Top + mskDateRet.Height - pnlCalendar.Height)
            {
                mskDateRet.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
                //mskDateRet.Select();
            }
            pnlCalendar.Visible = false;
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvSponsors.Visible = false; dgvContacts.Visible = false;
            }
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    if (txtSponsorID.Text.Trim() != "" && txtSponsorID.Text.All(char.IsDigit))
                    {
                        txtSponsor.Text = GISClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                        dgvSponsors.Visible = false;
                        LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    }
                }
                else if (e.KeyChar == 27)
                {
                    dgvSponsors.Visible = false;
                }
                else
                {
                    txtSponsor.Text = "";
                }
            }
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSponsors.TabIndex = 3;
                dgvSponsors.Top = txtSponsor.Top + txtSponsor.Height; dgvContacts.Visible = false;
            }
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 27 || e.KeyChar == 13)
                    dgvSponsors.Visible = false;
                else
                    txtSponsorID.Text = "";
            }
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSponsors;
                dvwSponsors = new DataView(dtSponsors, "SponsorName like '%" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
                GISClass.General.DGVSetUp(dgvSponsors, dvwSponsors, 360);
            }
        }

        private void txtSponsorEnterHandler(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSponsors.TabIndex = 7;
            }
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            dgvSponsors.Visible = false;
            txtContact.Text = "";
            LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
        }

        private void dgvSponsors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSponsors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                dgvSponsors.Visible = false;
                txtContact.Text = "";
                LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
            }
            else if (e.KeyChar == 27)
            {
                dgvSponsors.Visible = false;
            }
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void txtContact_Enter(object sender, EventArgs e)
        {
            if (nMode != 0 && txtSponsorID.Text != "")
            {
                dgvContacts.Visible = true; dgvContacts.BringToFront(); dgvContacts.TabIndex = 9;
            }
        }

        private void dgvContacts_DoubleClick(object sender, EventArgs e)
        {
            txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
            dgvContacts.Visible = false;
        }

        private void dgvContacts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvContacts_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
                dgvContacts.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvContacts.Visible = false;
            }
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0;
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
        }

        private void txtDocPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                this.ofdFile = new System.Windows.Forms.OpenFileDialog();

                // Set the file dialog to filter for graphics files. 
                this.ofdFile.Filter =
                    //"Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|" +
                    "Word Documents (*.DOC;*.DOCX;)|*.DOC;*.DOCX|" +
                    "PDF (*.PDF)|*.PDF|" +
                    "All files (*.*)|*.*";

                this.ofdFile.Multiselect = false;
                this.ofdFile.Title = "SELECT FILE";
                if (ofdFile.ShowDialog() == DialogResult.OK)
                {
                    txtDocPath.Text = ofdFile.FileName;
                }
            }
        }

        private void mskDocDate_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.BringToFront();
                pnlCalendar.Left = mskDocDate.Width + mskDocDate.Left + 1;
                pnlCalendar.Top = mskDocDate.Top;
            }
        }

        private void mskDateMailed_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.BringToFront();
                pnlCalendar.Left = mskDateMailed.Width + mskDateMailed.Left + 1;
                pnlCalendar.Top = mskDateMailed.Top + mskDateMailed.Height - pnlCalendar.Height;
            }
        }

        private void mskDateExp_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.BringToFront();
                pnlCalendar.Left = mskDateExp.Width + mskDateExp.Left + 1;
                pnlCalendar.Top = mskDateExp.Top + mskDateExp.Height - pnlCalendar.Height;
            }
        }

        private void mskDateRet_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                pnlCalendar.Visible = true; pnlCalendar.BringToFront();
                pnlCalendar.Left = mskDateRet.Width + mskDateRet.Left + 1;
                pnlCalendar.Top = mskDateRet.Top + mskDateRet.Height - pnlCalendar.Height;
            }
        }

        private void btnOpenDoc_Click(object sender, EventArgs e)
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

        private void mskDateRet_Leave(object sender, EventArgs e)
        {
            try
            {
                DateTime.Parse(mskDateRet.Text);
            }
            catch
            {
                dtMaster.Rows[0]["DateReturned"] = DBNull.Value;
                //mskDateRet.Text = null;
            }
        }

        private void mskDocDate_Leave(object sender, EventArgs e)
        {
            try
            {
                DateTime.Parse(mskDocDate.Text);
            }
            catch
            {
                dtMaster.Rows[0]["DocDate"] = DBNull.Value;
                //mskDocDate.Text = null;
            }
        }

        private void mskDateMailed_Leave(object sender, EventArgs e)
        {
            try
            {
                DateTime.Parse(mskDateMailed.Text);
            }
            catch
            {
                dtMaster.Rows[0]["DateMailed"] = DBNull.Value;
                //mskDateMailed.Text = null;
            }
        }

        private void mskDateExp_Leave(object sender, EventArgs e)
        {
            try
            {
                DateTime.Parse(mskDateExp.Text);
            }
            catch
            {
                dtMaster.Rows[0]["DateExpires"] = DBNull.Value;
                //mskDateExp.Text = null;
            }
        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0 && txtSponsorID.Text != "")
            {
                try
                {
                    DataView dvwContacts;
                    dvwContacts = new DataView(dtContacts, "Contact like '%" + txtContact.Text.Trim() + "%'", "Contact", DataViewRowState.CurrentRows);
                    GISClass.General.DGVSetUp(dgvContacts, dvwContacts, 183);
                }
                catch { }
            }
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            dgvSponsors.Visible = true;
        }

        private void picContacts_Click(object sender, EventArgs e)
        {
            dgvContacts.Visible = true;
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27 || e.KeyChar == 13)
            {
                dgvContacts.Visible = false; mskDateMailed.Select();
            }
        }
    }
}
