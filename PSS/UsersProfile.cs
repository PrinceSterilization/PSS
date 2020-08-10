using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Data.SqlClient;
using System.Linq;

namespace PSS
{
    public partial class UsersProfile : PSS.TemplateForm
    {
        private byte nMode = 0; //switch for Add or Edit Mode, 1 - Add New Record, 2 - Edit Record

        private bool mouseDown;// for dragging and dropping data form panel (pnlRecord)
        private Point mousePos;// for dragging and dropping data form panel (pnlRecord)
        private string[] arrCol;// for record search dropdown data fields container
        private int nIndex;//index holder for currently selected row in master datagridview (dgvFile)
        private int nCtr = 0;//counter for keypress search functionality on master datagridgriew (dgvFile)
        private int nSw = 0;//switch for keypress search functionality on master datagridgriew (dgvFile)
        private string strFileAccess = "RO";//user data form access, default value
        
        protected DataTable dtMaster = new DataTable();//in-memory table
        protected DataTable dtSysAccess = new DataTable();//in-memory table
        protected DataTable dtPrtAccess = new DataTable();//in-memory table

        public UsersProfile()
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
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            //
            //Hiding/Unhiding Datagridview Columns
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
            //cklColumns.ItemCheck += new ItemCheckEventHandler(cklItemChkEventHandler);
            //
            //Display Option for Master Datagridview (optional)
            chkShowInactive.Click += new EventHandler(chkShowInactiveClickHandler);
            DataGridViewComboBoxColumn dbPrintCol = new DataGridViewComboBoxColumn();

            DataGridViewComboBoxColumn dbCol = new DataGridViewComboBoxColumn();
            DataGridViewComboBoxColumn dbCol1 = new DataGridViewComboBoxColumn();

            dbCol.DataSource = GetForms();
            dbCol.Name = "FormName";
            dbCol.DataPropertyName = "FormName";
            dbCol.ValueMember = "FormName";
            dbCol.DisplayMember = "FormName";
            dbCol1.Items.Add("FA");
            dbCol1.Items.Add("FS");
            dbCol1.Items.Add("RW");
            dbCol1.Items.Add("RO");
            dbCol1.DataPropertyName = "AccessLevel";
            dbCol1.Name = "AccessLevel";

            dgvSAccess.Columns.Add(dbCol);

            dgvSAccess.Columns.Add(dbCol1);
            dgvSAccess.Columns[0].HeaderText = "Form Name";
            dgvSAccess.Columns[0].Width = 280;
            dgvSAccess.Columns[1].Width = 90;
            dgvSAccess.Columns[1].HeaderText = "Access Level";

            dgvSAccess.AutoGenerateColumns = false;
            dbPrintCol.DataSource = GetPrinter();
            dbPrintCol.Name = "PrinterName";
            dbPrintCol.DataPropertyName = "PrinterName";
            dbPrintCol.ValueMember = "PrinterName";
            dbPrintCol.DisplayMember = "PrinterName";

            dgvPAccess.Columns.Add(dbPrintCol);
            dgvPAccess.Columns[0].Width = 370;
            dgvPAccess.Columns[0].HeaderText = "Printer Name";
            dgvPAccess.AutoGenerateColumns = false;
        }

        private void UsersProfile_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "UsersProfile");

            LoadRecords(0);
            BuildPrintItems();
            cboUserGroups.DataSource = PSSClass.UserGroups.UserGroupsDDL();
            cboUserGroups.DisplayMember = "GroupName";
            cboUserGroups.ValueMember = "GroupID";

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
            chkShowInactive.Visible = true;
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

            dtMaster.Columns.Add("UserID", typeof(Int16));
            dtMaster.Columns.Add("UserType", typeof(Int16));
            dtMaster.Columns.Add("UserName", typeof(String));
            dtMaster.Columns.Add("GroupName", typeof(String));
            dtMaster.Columns.Add("LogonID", typeof(String));
            //dtMaster.Columns.Add("Password", typeof(String));
            //dtMaster.Columns.Add("ComputerID", typeof(String));
            dtMaster.Columns.Add("GroupID", typeof(Int16));
            dtMaster.Columns.Add("AccessLevel", typeof(Int16));
            dtMaster.Columns.Add("Active", typeof(bool));
            dtMaster.Columns.Add("Inactive", typeof(bool));
            //dtMaster.Columns.Add("DateCreated", typeof(DateTime));
            //dtMaster.Columns.Add("CreatedByID", typeof(Int16));
            //dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            //dtMaster.Columns.Add("LastUserID", typeof(Int16));
            bsUsers.DataSource = dtMaster;
            ControlDataBindings();
            DataTable dtEmp = PSSClass.Employees.EmployeesDDL();
            cboEmployees.DataSource = dtEmp;
            cboEmployees.DisplayMember = "EmpName";
            cboEmployees.ValueMember = "EmployeeID";
            //dtEmp.Dispose();
        }

        private void ControlDataBindings()
        {
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            txtUserName.DataBindings.Add("Text", bsUsers, "UserName", true);
            txtUserID.DataBindings.Add("Text", bsUsers, "UserID", true);
            txtLoginID.DataBindings.Add("Text", bsUsers, "LogonID", true);
            txtAccessLevel.DataBindings.Add("Text", bsUsers, "AccessLevel", true);
            txtUserType.DataBindings.Add("Text", bsUsers, "UserType", true);
            txtGroupID.DataBindings.Add("Text", bsUsers, "GroupID", true);
            cboEmployees.DataBindings.Add("Text", bsUsers, "UserName", true);
            cboUserGroups.DataBindings.Add("Text", bsUsers, "GroupName", true);
            chkInactive.DataBindings.Add("Checked", bsUsers, "Inactive", true);
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
            AddEditMode(true); 
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false; tsbRefresh.Enabled = false;
            ClearControls(this.pnlRecord);
            dgvSAccess.DataSource = null;
            dgvPAccess.DataSource = null;
            OpenControls(this.pnlRecord, true);
            dgvSAccess.ReadOnly = false; dgvPAccess.ReadOnly = false;

            dtMaster.Rows.Clear();
            DataRow dR = dtMaster.NewRow();
            dR["UserName"] = "";
            dR["AccessLevel"] = 0;
            dR["UserType"] = 0;
            dR["LogonID"] = "";
            //dR["ComputerID"] = "";
            dR["UserID"] = DBNull.Value;
            dR["GroupID"] = DBNull.Value;
            dR["Active"] = 1;
            dR["InActive"] = 0;
            dtMaster.Rows.Add(dR);
            txtUserType.Text = "2";
            txtAccessLevel.Text = "3";
            chkInactive.Enabled = false; rbtnEmployee.Checked = true; cboEmployees.Visible = true; cboEmployees.BringToFront();
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
            AddEditMode(true);
            OpenControls(this.pnlRecord, true);
            LoadData();
            grpUtype.Enabled = false; txtUserName.ReadOnly = true; cboEmployees.Enabled = false; txtLoginID.ReadOnly = false;
            dgvSAccess.Enabled = true; dgvSAccess.ReadOnly = false;
            btnClose.Visible = false; tsbRefresh.Enabled = false; txtLoginID.Focus();
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

                sqlcmd.Parameters.AddWithValue("@UserID", Convert.ToInt16(txtUserID.Text));
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelUser";

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
            LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
        }


        private void SaveClickHandler(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void SaveRecord()
        {
            Int16 nUID = 0;

            if (txtUserName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter user name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUserName.Focus();
                return;
            }
            if (txtLoginID.Text.Trim() == "")
            {
                MessageBox.Show("Please enter user's login ID.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLoginID.Focus();
                return;
            }

            bsUsers.EndEdit();

            if (rbtnEmployee.Checked)
            {
                txtUserType.Text = "1";
                nUID = Convert.ToInt16(PSSClass.Employees.EmpID(txtLoginID.Text.Trim()));
                if (nUID == 0)
                {
                    MessageBox.Show("Invalid login ID for employee.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtLoginID.Focus();
                    return;
                }
            }
            else if (rbtnGuest.Checked)
            {
                if (nMode == 1)
                {
                    txtUserType.Text = "2";
                    nUID = PSSClass.Users.GuestUserID();
                    if (nUID == 0)
                        nUID = 1001;
                    else
                        nUID += 1;
                }
                else
                    nUID = Convert.ToInt16(txtUserID.Text);    
            }
            byte nChanges = 0;
            DataTable dt = dtMaster.GetChanges();
            if (dt != null && dt.Rows.Count > 0)
            {
                nChanges = 1;
            }
            dt = null;
            dt = dtSysAccess.GetChanges();
            if (dt != null && dt.Rows.Count > 0)
            {
                nChanges = 1;
            }
            dt = null;
            dt = dtPrtAccess.GetChanges();
            if (dt != null && dt.Rows.Count > 0)
            {
                nChanges = 1;
            }
            if (nChanges == 0)
            {
                MessageBox.Show("No data to be saved.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            byte bAddEdit = 0, bSysAccess = 0, bPrtAccess = 0;

            dt = dtMaster.GetChanges(DataRowState.Added);
            if (dt != null)
                bAddEdit = 1;
            dt = null;
            dt = dtMaster.GetChanges(DataRowState.Modified);
            if (dt != null)
                bAddEdit = 1;

            dt = null;
            dt = dtSysAccess.GetChanges(DataRowState.Modified);
            if (dt != null)
                bSysAccess = 1;

            dt = null;
            dt = dtPrtAccess.GetChanges(DataRowState.Modified);
            if (dt != null)
                bPrtAccess = 1;
            if (nChanges == 1)
                bAddEdit = 1;

            if (bAddEdit == 1)
            {
                //xmlWriter();
                var sb = new StringBuilder();
                XmlWriter writer = XmlWriter.Create(sb);
                try
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("SysAccess");


                    foreach (DataGridViewRow row in dgvSAccess.Rows)
                    {
                        if (row.IsNewRow)
                        {
                            break;
                        }
                        writer.WriteStartElement("WinForms");
                        writer.WriteStartElement("FormName");
                        writer.WriteString(row.Cells["FormName"].Value.ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("AccessLevel");
                        writer.WriteString(row.Cells["AccessLevel"].Value.ToString());
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                //xmlPrinterWriter();
                var sp = new StringBuilder();
                writer = XmlWriter.Create(sp);
                try
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("Printers");


                    foreach (DataGridViewRow row in dgvPAccess.Rows)
                    {
                        if (row.IsNewRow)
                        {
                            break;
                        }
                        writer.WriteStartElement("PrinterName");
                        writer.WriteString(row.Cells[0].Value.ToString());
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                int nSBF = 0, nSBT = 0;

                string ssb = "<SysAccess></SysAccess>";
                string ssp = "<Printers></Printers>";
                if (sb.ToString().IndexOf("<SysAccess>") != -1)
                {
                    nSBF = sb.ToString().IndexOf("<SysAccess>");
                    nSBT = sb.ToString().IndexOf("</SysAccess>");
                    ssb = sb.ToString().Substring(nSBF, (sb.ToString().Length - nSBF));
                }
                if (sb.ToString().IndexOf("<Printers>") != -1)
                {
                    nSBF = sp.ToString().IndexOf("<Printers>");
                    nSBT = sp.ToString().IndexOf("</Printers>");
                    ssp = sp.ToString().Substring(nSBF, (sp.ToString().Length - nSBF));
                }
                sqlcmd.Parameters.AddWithValue("@nMode", nMode);
                sqlcmd.Parameters.AddWithValue("@UserID", nUID);
                sqlcmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
                if (rbtnEmployee.Checked == true)
                    sqlcmd.Parameters.AddWithValue("@UserType", 1);
                else
                    sqlcmd.Parameters.AddWithValue("@UserType", 2);
                sqlcmd.Parameters.AddWithValue("@LogonID", txtLoginID.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@GroupID", cboUserGroups.SelectedValue);
                if (rbtnAdministrator.Checked == true)
                    sqlcmd.Parameters.AddWithValue("@AccessLevel",1);
                else if (rbtnDUser.Checked == true)  
                    sqlcmd.Parameters.AddWithValue("@AccessLevel",2);
                sqlcmd.Parameters.AddWithValue("@SystemAccess", ssb);
                sqlcmd.Parameters.AddWithValue("@PrinterAccess", ssp);
                if (chkInactive.Checked == true)
                    sqlcmd.Parameters.AddWithValue("@Active", 0);
                else
                    sqlcmd.Parameters.AddWithValue("@Active", 1);
                sqlcmd.Parameters.AddWithValue("@UpdatedByID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditUser";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("PRIMARY KEY") > 0)
                    {
                        MessageBox.Show("Matching user found. Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    sqlcnn.Dispose();
                    return;
                }
                //dtMaster.Rows[0]["UserID"] = nUID;
                //dtMaster.Rows[0]["GroupID"] = cboUserGroups.SelectedValue;
                //dtMaster.Rows[0]["Active"] = 1;
                //cmdBuilder.GetInsertCommand();
                //da.Update(dtMaster);
            }
            //dt = null;
            //dt = dtMaster.GetChanges(DataRowState.Modified);
            //if (dt != null)
            //{
                //dtMaster.Rows[0]["GroupID"] = cboUserGroups.SelectedValue;
                //dtMaster.Rows[0]["LastUpdate"] = DateTime.Now;
                //dtMaster.Rows[0]["LastUserID"] = LogIn.nUserID;
                //cmdBuilder.GetUpdateCommand();
                //da.Update(dtMaster);
                //dt.Dispose();
            //}
           
            //da.Dispose(); cmdBuilder.Dispose();

            if (bSysAccess == 1)
                xmlWriter();
            if (bPrtAccess == 1)
                xmlPrinterWriter();

            sqlcnn.Close(); sqlcnn.Dispose();

            txtUserID.Text = nUID.ToString();
            
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;

            //if (chkShowInactive.Checked == false)
            //    LoadRecords(1);
            //else
            //    LoadRecords(2);
            LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
            PSSClass.General.FindRecord("UserID", txtUserID.Text, bsFile, dgvFile);
            AddEditMode(false); //Initialize Toolbar
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
            //if (chkShowInactive.Checked == false)
            //    LoadRecords(1);
            //else
            //    LoadRecords(2);
            LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
            bsFile.Filter = "UserID<> 0";
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false); //Initialize Toolbar
            nMode = 0;
        }

        private void CloseClickHandler(object sender, EventArgs e)
        {
            this.Close();
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
                bsFile.Filter = "UserID<> 0";
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
            LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
            //if (chkShowInactive.Checked == false)
            //    LoadRecords(1);
            //else
            //    LoadRecords(2);
            tsbRefresh.Enabled = false;
        }

        private void chkShowInactiveClickHandler(object sender, EventArgs e)
        {
            LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
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

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
            OpenControls(this.pnlRecord, false);
            LoadData();
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

        private void LoadRecords(Int16 cActiveSw)
        {
            pnlRecord.Visible = false; dgvFile.Visible = true;
            DataTable dt = PSSClass.Users.UsersFile(cActiveSw);
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "UserID <> 0";
            DataGridSetting();
            try
            {
                if (tsddbSearch.DropDownItems.Count == 0)
                {
                    int i = 0;

                    arrCol = new string[dt.Columns.Count];

                    ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - 1];// ToolStripMenuItem[arrCol.Length - 4];

                    foreach (DataColumn colFile in dt.Columns)
                    {
                        if (colFile.ColumnName.ToString() != "UserID")
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
            dgvFile.Columns["UserName"].HeaderText = "USER NAME";
            dgvFile.Columns["UserTypeDesc"].HeaderText = "TYPE";
            dgvFile.Columns["LogonID"].HeaderText = "LOGON ID";
            dgvFile.Columns["AccessLevelDesc"].HeaderText = "ACCESS LEVEL";
            dgvFile.Columns["UserName"].Width = 250;
            dgvFile.Columns["UserName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvFile.Columns["UserTypeDesc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvFile.Columns["AccessLevelDesc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvFile.Columns["LogonID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvFile.Columns["GroupName"].HeaderText = "GROUP MEMBERSHIP";
            dgvFile.Columns["GroupName"].Width = 250;
            dgvFile.Columns["UserType"].Visible = false;
            dgvFile.Columns["AccessLevel"].Visible = false;
            dgvFile.Columns["UserID"].Visible = false;
            dgvFile.Columns["Inactive"].Visible = false; 
            dgvFile.Columns["GroupID"].Visible = false;
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;

            dtMaster.Rows.Clear();

            DataRow dR = dtMaster.NewRow();
            dR["UserID"] = dgvFile.CurrentRow.Cells["UserID"].Value;
            dR["UserName"] = dgvFile.CurrentRow.Cells["UserName"].Value;
            dR["GroupName"] = dgvFile.CurrentRow.Cells["GroupName"].Value;
            dR["GroupID"] = dgvFile.CurrentRow.Cells["GroupID"].Value;
            dR["UserType"] = dgvFile.CurrentRow.Cells["UserType"].Value; 
            dR["LogonID"] = dgvFile.CurrentRow.Cells["LogonID"].Value;
            dR["AccessLevel"] = dgvFile.CurrentRow.Cells["AccessLevel"].Value;
            dR["Active"] = dgvFile.CurrentRow.Cells["Active"].Value;
            dR["Inactive"] = dgvFile.CurrentRow.Cells["Inactive"].Value;
            dtMaster.Rows.Add(dR);
            dtMaster.AcceptChanges();
            bsUsers.DataSource = dtMaster;
            dtSysAccess = GetXml(Convert.ToInt16(txtUserID.Text));
            dtPrtAccess = GetXmlPrinters(Convert.ToInt16(txtUserID.Text));
            dgvSAccess.DataSource = dtSysAccess;
            dgvPAccess.DataSource = dtPrtAccess;
            
            dgvSAccess.Enabled = true; dgvSAccess.ReadOnly = true;
            dgvPAccess.Enabled = true; dgvPAccess.ReadOnly = true;
            dgvSAccess.Sort(dgvSAccess.Columns[0], ListSortDirection.Ascending);
            if (txtUserType.Text == "1")
                rbtnEmployee.Checked = true;
            else
            {
                rbtnGuest.Checked = true;
                cboEmployees.Visible = true; cboEmployees.BringToFront();
            }
            if (txtAccessLevel.Text == "1")
                rbtnAdministrator.Checked = true;
            else
                rbtnDUser.Checked = true;
        }

        private DataTable GetForms()
        {
            SqlCommand dbCmd = new SqlCommand();
            DataTable tb = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter();
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.Connection = sqlcnn;
            dbCmd.CommandText = "spSystemForms";
            dap.SelectCommand = dbCmd;
            dap.Fill(tb);
            dap.Dispose();
            sqlcnn.Dispose();
            return tb;
        }


        private DataTable GetPrinter()
        {
            SqlCommand dbCmd = new SqlCommand();
            DataTable tb = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter();
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.Connection = sqlcnn;
            dbCmd.CommandText = "spGetUserPrinters";
            dap.SelectCommand = dbCmd;
            dap.Fill(tb);
            dap.Dispose();
            sqlcnn.Dispose();
            return tb;
        }

        private string GetTypeValue(int iType)
        {
            if (iType == 1)
                return "Employee";
            else
                return "Guest";
        }
        private int GetTypeCode(string sType)
        {
            if (sType == "Employee")
                return 1;
            else
                return 2;
        }
        private string GetAccessValue(int iAccess)
        {
            if (iAccess == 1)
                return "Administrator";
            else if (iAccess == 2)
                return "Power User";
            else
                return "Domain User";

        }
        private int GetAccessCode(string sAccess)
        {
            if (sAccess == "Administrator")
                return 1;
            else if (sAccess == "Power User")
                return 2;
            else
                return 3;
        }

        private void GetRadioBtnValue()
        {
            if (GetTypeCode(dgvFile.CurrentRow.Cells["UserType"].Value.ToString()) == 1)
            {
                rbtnEmployee.Checked = true;
            }
            else
            {
                rbtnGuest.Checked = true;
            }

            if (GetAccessCode(dgvFile.CurrentRow.Cells["AccessLevel"].Value.ToString()) == 1)
            {
                rbtnAdministrator.Checked = true;
            }
            else if (GetAccessCode(dgvFile.CurrentRow.Cells["AccessLevel"].Value.ToString()) == 2)
            {
                rbtnPUser.Checked = true;
            }
            else
            {
                rbtnDUser.Checked = true;
            }
        }

        private DataTable GetXml(int uid)
        {
            DataTable tb = new DataTable();
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            SqlCommand cmd = new SqlCommand("spGetXmlForms", sqlcnn);

            cmd.Parameters.AddWithValue("ID", uid);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(tb);
            dap.Dispose();
            sqlcnn.Dispose();
            return tb;
        }

        private DataTable GetXmlPrinters(int uid)
        {
            DataTable tb = new DataTable();
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }
            SqlCommand cmd = new SqlCommand("spGetXmlPrinters", sqlcnn);
            cmd.Parameters.AddWithValue("@ID", uid);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dap = new SqlDataAdapter();
            dap.SelectCommand = cmd;
            dap.Fill(tb);
            dap.Dispose();
            cmd.Parameters.Clear();
            sqlcnn.Dispose();
            return tb;

        }

        private void xmlWriter()
        {
            var sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                try
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("SysAccess");


                    foreach (DataGridViewRow row in dgvSAccess.Rows)
                    {
                        if (row.IsNewRow)
                        {
                            break;
                        }
                        writer.WriteStartElement("WinForms");
                        writer.WriteStartElement("FormName");
                        writer.WriteString(row.Cells["FormName"].Value.ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("AccessLevel");
                        writer.WriteString(row.Cells["AccessLevel"].Value.ToString());
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                SaveToDb(sb.ToString(), Convert.ToInt16(txtUserID.Text));
            }
        }

        public void xmlPrinterWriter()
        {
            var sb = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(sb))
            {
                try
                {
                    writer.WriteStartDocument();

                    writer.WriteStartElement("Printers");


                    foreach (DataGridViewRow row in dgvPAccess.Rows)
                    {
                        if (row.IsNewRow)
                        {
                            break;
                        }
                        writer.WriteStartElement("PrinterName");
                        writer.WriteString(row.Cells[0].Value.ToString());
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                SaveToDbPrint(sb.ToString(), Convert.ToInt16(txtUserID.Text));
            }
        }

        private void SaveToDb(string xml, int Uid)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            using (var command = new SqlCommand("spUpdateXMLForm", sqlcnn))
            {
                command.Parameters.Add("XML", SqlDbType.Xml, xml.Length).Value = xml;
                command.Parameters.AddWithValue("ID", Uid);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
            sqlcnn.Dispose();
        }

        private void SaveToDbPrint(string xml, int Uid)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            using (var command = new SqlCommand("spUpdateXmlPrinters", sqlcnn))
            {
                command.Parameters.Add("XML", SqlDbType.Xml, xml.Length).Value = xml;
                command.Parameters.AddWithValue("ID", Uid);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
            sqlcnn.Dispose();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
            AddEditMode(false);//Initialize Toolbar
            FileAccess();
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

        private void dgvSAccess_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                object value = dgvSAccess.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (!((DataGridViewComboBoxColumn)dgvSAccess.Columns[e.ColumnIndex]).Items.Contains(value))
                {
                    e.ThrowException = false;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr += 1;
            if (nCtr > 1)
            {
                DataTable dt = new DataTable();
                dt = PSSClass.Users.UsersFile(Convert.ToInt16(chkShowInactive.CheckState));
                PSSClass.DataEntry.DGVSearch(tstbSearchField.Text, tstbSearch.Text, dt, bsFile);
                nCtr = 0;
                tstbSearch.Text = "";
                timer1.Enabled = false;
                nSw = 0;
            }
        }

        private void dgvSAccess_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSAccess.IsCurrentCellDirty)
            {
                dgvSAccess.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvPAccess_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPAccess.IsCurrentCellDirty)
            {
                dgvPAccess.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void txtUserType_TextChanged(object sender, EventArgs e)
        {
            if (txtUserType.Text == "1")
                rbtnEmployee.Checked = true;
            else if (txtUserType.Text == "2")
                rbtnGuest.Checked = true;
        }

        private void rbtnEmployee_Click(object sender, EventArgs e)
        {
            txtUserType.Text = "1";
            cboEmployees.Visible = true; cboEmployees.BringToFront(); txtLoginID.ReadOnly = true;
        }

        private void rbtnGuest_Click(object sender, EventArgs e)
        {
            txtUserType.Text = "2"; cboEmployees.Visible = false; txtUserName.BringToFront(); txtLoginID.ReadOnly = false;
            if (nMode == 1)
            {
                txtLoginID.Text = ""; txtUserName.Focus();
            }
        }

        private void rbtnAdministrator_Click(object sender, EventArgs e)
        {
            txtAccessLevel.Text = "1";
        }

        private void rbtnDUser_Click(object sender, EventArgs e)
        {
            txtAccessLevel.Text = "3";
        }

        private void txtAccessLevel_TextChanged(object sender, EventArgs e)
        {
            if (txtAccessLevel.Text == "1")
                rbtnAdministrator.Checked = true;
            else if (txtAccessLevel.Text == "2")
                rbtnPUser.Checked = true;
            else if (txtAccessLevel.Text == "3")
                rbtnDUser.Checked = true;
        }

        //private void txtGroupID_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtGroupID.Text != "")
        //        cboUserGroups.SelectedValue = Convert.ToInt16(txtGroupID.Text);
        //    else
        //        cboUserGroups.SelectedIndex = -1;
        //}

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
            else if (strFileAccess == "FA" || strFileAccess == "FS")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = true;
            }
        }

        private void UsersProfile_KeyDown(object sender, KeyEventArgs e)
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
                    if (nMode == 0 && (strFileAccess == "FA" || strFileAccess == "FS"))
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

        private void chkShowInactive_Click(object sender, EventArgs e)
        {

        }

        private void chkInactive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInactive.Checked == true)
            {

            }
        }

        private void cboEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtLoginID.Text = PSSClass.Employees.EmpLoginID(Convert.ToInt16(cboEmployees.SelectedValue));
                txtUserName.Text = cboEmployees.Text;
                txtLoginID.ReadOnly = true;
            }
            catch { }
        }

        private void cboUserGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtGroupID.Text = cboUserGroups.SelectedValue.ToString();
            }
            catch { }
        }
    }
}
