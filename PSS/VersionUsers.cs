using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Messaging;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace PSS
{
    public partial class VersionUsers : PSS.TemplateForm
    {
        public struct CopyTrackMsg
        {
            public string Subject, From, Scope, RelDate;
        }

        private byte nMode = 0; //switch for Add or Edit Mode, 1 - Add New Record, 2 - Edit Record

        private bool mouseDown;// for dragging and dropping data form panel (pnlRecord)
        private Point mousePos;// for dragging and dropping data form panel (pnlRecord)
        private string[] arrCol;// for record search dropdown data fields container
        private int nIndex;//index holder for currently selected row in master datagridview (dgvFile)
        private int nCtr = 0;//counter for keypress search functionality on master datagridgriew (dgvFile)
        private int nSw = 0;//switch for keypress search functionality on master datagridgriew (dgvFile)
        private string strFileAccess = "RO";//user data form access, default value

        protected DataTable dtMaster = new DataTable();
    
        public VersionUsers()
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

            DataGridViewComboBoxColumn dbUname = new DataGridViewComboBoxColumn();

            dbUname.DataSource = GetUsers();
            dbUname.Name = "UserName";
            dbUname.DataPropertyName = "UserName";
            dbUname.ValueMember = "UserID";
            dbUname.DisplayMember = "UserName";
            dgvUsers.Columns.Add(dbUname);
            dgvUsers.Columns[0].Width = 250;
            dgvUsers.AutoGenerateColumns = false;
        }

        private void VersionUsers_Load(object sender, EventArgs e)
        {
            LoadRecords();
            BuildPrintItems();
            cboUsers.DataSource = PSSClass.Versions.GetUsers();
            cboUsers.DisplayMember = "UserName";
            cboUsers.ValueMember = "UserID";
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
            chkShowInactive.Visible = true;
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "VersionUsers");

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
            dtMaster.Columns.Add("SystemName", typeof(String));
            dtMaster.Columns.Add("VersionNo", typeof(String));
            dtMaster.Columns.Add("UserID", typeof(String));
            dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            dtMaster.Columns.Add("DateCreated", typeof(DateTime));
            dtMaster.Columns.Add("CreatedByID", typeof(Int16));
            dtMaster.Columns.Add("LastUserID", typeof(Int16));
            bsUsers.DataSource = dtMaster;
            //ControlDataBindings();
            tsbEdit.Enabled = false;
        }

        private DataTable GetUsers()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return null;
            }

            SqlCommand cmds = new SqlCommand();
            DataTable tb = new DataTable();
            SqlDataAdapter dap = new SqlDataAdapter();
            cmds.Connection = sqlcnn;
            cmds.CommandText = "spUsers";
            cmds.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand = cmds;
            dap.Fill(tb);
            dap.Dispose();
            return tb;
        }

        //private void ControlDataBindings()
        //{
            //foreach (Control c in pnlRecord.Controls)
            //{
            //    c.DataBindings.Clear();
            //}
            //cboSystems.DataBindings.Add("Text", bsUsers, "SystemName");
            //cboVersions.DataBindings.Add("Text", bsUsers, "VersionNo");
            //cboUname.DataBindings.Add("SelectedValue", bsUsers, "UserID");
        //}

        private void LoadRecords()
        {
            DataTable dt = PSSClass.Versions.VersionUsers();
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "VersionNo<>''";
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
                            colFile.ColumnName.ToString() != "UserID")
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
            tsbEdit.Enabled = false;
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["VersionNo"].HeaderText = "VERSION NO";
            dgvFile.Columns["UserName"].HeaderText = "USER NAME";
            dgvFile.Columns["UserName"].Width = 350;
            dgvFile.Columns["VersionNo"].Width = 250;
            dgvFile.Columns["UserID"].Visible = false;
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
            dR["UserID"] = dgvFile.CurrentRow.Cells["UserID"].Value;
            dR["DateCreated"] = dgvFile.CurrentRow.Cells["DateCreated"].Value;
            dR["CreatedByID"] = dgvFile.CurrentRow.Cells["CreatedByID"].Value;
            dR["LastUpdate"] = dgvFile.CurrentRow.Cells["LastUpdate"].Value;
            dR["LastUserID"] = dgvFile.CurrentRow.Cells["LastUserID"].Value;
            dtMaster.Rows.Add(dR);
            dtMaster.AcceptChanges();
            bsUsers.DataSource = dtMaster;
            cboSystems.Text = dgvFile.CurrentRow.Cells["SystemName"].Value.ToString();
            cboVersions.Text = dgvFile.CurrentRow.Cells["VersionNo"].Value.ToString();
            cboUsers.SelectedValue = dgvFile.CurrentRow.Cells["UserID"].Value;
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
            tsbEdit.Enabled = false;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false; tsbRefresh.Enabled = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            dgvUsers.Rows.Clear();
            dgvUsers.Visible = true;
            dtMaster.Rows.Clear();
            DataRow dR = dtMaster.NewRow();
            dR["SystemName"] = "";
            dR["VersionNo"] = "";
            dR["UserID"] = "";
            dR["CreatedByID"] = 1;
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;
            dtMaster.Rows.Add(dR);
            tsbEdit.Enabled = false; tsbDelete.Enabled = false;
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
            btnClose.Visible = true; cboVersions.Focus(); tsbRefresh.Enabled = false;
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

                sqlcmd.Parameters.AddWithValue("@VerNo", cboVersions.Text);
                sqlcmd.Parameters.AddWithValue("@VerUserID", Convert.ToInt16(cboUsers.SelectedValue));
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelVersionUser";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                    nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
                    AddEditMode(false);//Initialize Toolbar
                    tsbEdit.Enabled = false;
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

        private void DeleteRec(string verNo)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@VerNo", cboVersions.Text);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDeleteVersionAll";

            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem encountered: " + ex.Message + Environment.NewLine + "Record is not deleted!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();

        }
        private void SaveClickHandler(object sender, EventArgs e)
        {
            try
            {
                SaveRecord();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            dgvUsers.Visible = false;
        }

        private void ResetRow()
        {
            dtMaster.Rows.Clear();
            DataRow dR = dtMaster.NewRow();
            dR["DateCreated"] = DateTime.Now;
            dR["SystemName"] = cboSystems.Text;
            dR["VersionNo"] = cboVersions.Text;
            dR["UserID"] = LogIn.nUserID;
            dR["CreatedByID"] = LogIn.nUserID;
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;
            dtMaster.Rows.Add(dR);
        }

        private void SaveRecord()
        {
            string strVno;
            if (cboVersions.Text.Trim() == "")
            {
                MessageBox.Show("Please select or enter version number.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboVersions.Focus();
                return;
            }
            strVno = cboVersions.Text;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlDataAdapter da = new SqlDataAdapter("SELECT SystemName, VersionNo, UserID, DateCreated, CreatedByID, LastUpdate, LastUserID FROM VersionUsers", sqlcnn);
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);

            foreach (DataGridViewRow row in dgvUsers.Rows)
            {
                if (row.IsNewRow)
                {
                    break;
                }
                bsUsers.EndEdit();
                ResetRow();
                dtMaster.Rows[0]["SystemName"] = cboSystems.Text;
                dtMaster.Rows[0]["VersionNo"] = strVno;
                dtMaster.Rows[0]["UserID"] = row.Cells[0].Value;
                try
                {
                    cmdBuilder.GetInsertCommand();
                    da.Update(dtMaster);
                }
                catch
                { }
            }
            da.Dispose(); cmdBuilder.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
            if (dgvUsers.Rows.Count > 0)
            {
                string strScope = "";
                DateTime dteRelDate = DateTime.Now;
                DataTable dt = PSSClass.Versions.VerMasterData(cboSystems.Text, cboVersions.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //CopyTrackMsg newMsg;
                    //newMsg.Subject = cboSystems.Text + " - " + cboVersions.Text;
                    //newMsg.Scope = dt.Rows[i]["VersionNotes"].ToString().Trim();
                    //newMsg.RelDate = dt.Rows[i]["VersionDate"].ToString();
                    //newMsg.From = "GIS Administrator";                        

                    //string strCompID = PSSClass.Users.CompID(Convert.ToInt16(dtMaster.Rows[i]["UserID"]));
                    //System.Messaging.Message msg = new System.Messaging.Message();
                    //msg.Body = newMsg;
                    //MessageQueue msgQ;
                    //if (strCompID == "glrds01")
                    //    msgQ = new MessageQueue("FormatName:DIRECT=OS:" + strCompID + "\\Private$\\" + PSSClass.Users.LogID(Convert.ToInt16(dtMaster.Rows[i]["UserID"]))); 
                    //else
                    //    msgQ = new MessageQueue("FormatName:DIRECT=OS:" + strCompID + "\\Private$\\GIS"); 
                    //try
                    //{
                    //    msgQ.Send(msg);
                    //}
                    //catch { }
                    strScope = dt.Rows[0]["VersionNotes"].ToString().Trim();
                    dteRelDate = Convert.ToDateTime(dt.Rows[0]["VersionDate"]);

                }
                dt.Dispose();

                for (int i = 0; i < (dgvUsers.Rows.Count - 1) ; i++)
                {
                    string strFName = PSSClass.Users.UserFName(Convert.ToInt16(dgvUsers.Rows[i].Cells["UserName"].Value));
                    string strBody = "Dear " + strFName + ", <br /><br />";
                    strBody += "This is to inform you that a new software update is now available, as follows:<br />";
                    strBody += "<table border=1>";
                    strBody += "<tr>";
                    strBody += "<th width=" + '\u0022' + "100" + '\u0022' + ">" + "System Name</th>";
                    strBody += "<th width=" + '\u0022' + "120" + '\u0022' + ">" + "File ID</th>";
                    strBody += "<th width=" + '\u0022' + "100" + '\u0022' + ">" + "Release Date</th>";
                    strBody += "<th width=" + '\u0022' + "300" + '\u0022' + ">" + "Release Notes </th>";
                    strBody += "</tr>";

                    strBody += "<tr>";
                    strBody += "<td align=center>" + cboSystems.Text + "</td>";
                    strBody += "<td align=center>" + cboVersions.Text + "</td>";
                    strBody += "<td align=center>" + dteRelDate.ToString("MM/dd/yyyy") + "</td>";
                    strBody += "<td align=center>" + strScope + "</td>";
                    strBody += "</tr>";
                    strBody += "</table>";
                    strBody += "<br />";
                    strBody += "Please logout from GIS then login back to get the latest update.<br /><br />";
                    strBody += "Thank you.<br /><br />";

                    //Add Signature
                    string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
                    string signature = string.Empty;
                    DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
                    if (diInfo.Exists)
                    {
                        FileInfo[] fiSignature = diInfo.GetFiles("*.htm");
                        if (fiSignature.Length > 0)
                        {
                            StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
                            signature = sr.ReadToEnd();

                            if (!string.IsNullOrEmpty(signature))
                            {
                                string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
                                signature = signature.Replace(fileName + "_files/", appDataDir + "/" + fileName + "_files/");
                            }
                        }
                    }
                    strBody += signature;
                    Outlook.Application oApp = new Outlook.Application();

                    // Create a new mail item.
                    Outlook._MailItem oMsg = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                    oMsg.HTMLBody = "<FONT face=\"Arial\">";
                    oMsg.HTMLBody += strBody;

                    ////Add an attachment.
                    //oMsg.Attachments.Add(lnkReport.Text);
                    //Subject line
                    oMsg.Subject = "NEW SOFTWARE UPDATES";
                    // Add a recipient.
                    Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

                    string strEMail = PSSClass.Users.UserEMail(Convert.ToInt16(dgvUsers.Rows[i].Cells["UserName"].Value));
                    string[] EMAddresses = strEMail.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < EMAddresses.Length; k++)
                    {
                        if (EMAddresses[k].Trim() != "")
                        {
                            Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[k]);
                            oRecip.Resolve();
                        }
                    }
                    //oMsg.CC = "mprince@gibraltarlabsinc.com";
                    oMsg.Display();
                    ////Send.
                    //((Outlook._MailItem)oMsg).Send();

                    // Clean up.
                    //oRecip = null;
                    oRecips = null;
                    oMsg = null;
                    oApp = null;
                }
            }
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            LoadRecords();
            PSSClass.General.FindRecord("SystemName", cboSystems.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            AddEditMode(false); //Initialize Toolbar
            tsbEdit.Enabled = false;
            //Reload User's Access to this file - included in this function for sudden change in access level
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
            nMode = 0;
            tsbEdit.Enabled = false; 
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
            dgvUsers.Visible = false;
            ClearControls(this);
            LoadRecords();
            bsFile.Filter = "VersionNo <> ''";
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false); //Initialize Toolbar
            tsbEdit.Enabled = false;
            //Reload User's Access to this file - included in this function for sudden change in access level
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
            nMode = 0;
            tsbEdit.Enabled = false; 
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
            AddEditMode(false);//Initialize Toolbar
            //Reload User's Access to this file - included in this function for sudden change in access level
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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true;
            }
            tsbEdit.Enabled = false; 
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr += 1;
            if (nCtr > 1)
            {
                DataTable dt = new DataTable();
                dt = PSSClass.Versions.VersionUsers();
                PSSClass.DataEntry.DGVSearch(tstbSearchField.Text, tstbSearch.Text, dt, bsFile);
                nCtr = 0;
                tstbSearch.Text = "";
                timer1.Enabled = false;
                nSw = 0;
            }
        }

        private void dgvUname_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvUsers.IsCurrentCellDirty)
            {
                dgvUsers.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void VersionUsers_KeyDown(object sender, KeyEventArgs e)
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

        private void cboSystems_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboVersions.DataSource = PSSClass.Versions.GetVersions(cboSystems.Text);
            cboVersions.DisplayMember = "VersionNo";
            cboVersions.ValueMember = "VersionNo";
        }
    }
}
