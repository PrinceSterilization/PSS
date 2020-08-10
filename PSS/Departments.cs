using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

namespace PSS
{
    public partial class Departments : PSS.TemplateForm
    {
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private int nCtr = 0;
        private int nSw = 0;
        private string strFileAccess = "RO";

        public Departments()
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
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            nMode = 0;
            DataTable dt = PSSClass.Departments.DepartmentsMaster(1);
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "DepartmentID <> 0";
            DataGridSetting();
            FileAccess();
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

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[2];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "DepartmentCode";
            items[0].Text = "Sorted by Department Code";
            items[0].Click += new EventHandler(PrintDeptCodeClickHandler);

            items[1] = new ToolStripMenuItem();
            items[1].Name = "DepartmentName";
            items[1].Text = "Sorted by Department Name";
            items[1].Click += new EventHandler(PrintDeptNameClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Departments.DepartmentsMaster(1);
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
            string strCol =cklColumns.Items[cklColumns.SelectedIndex].ToString().Replace(" ","");
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
            DepartmentRpt rptDeptNameList = new DepartmentRpt();
            rptDeptNameList.WindowState = FormWindowState.Maximized;
            rptDeptNameList.rptName = "DeptName";
            rptDeptNameList.rptLabel = "DEPARTMENTS REFERENCE LIST SORTED BY NAME";
            rptDeptNameList.Show();
        }

        private void PrintDeptCodeClickHandler(object sender, EventArgs e)
        {
            //RptDept rptDeptNameList = new RptDept();
            //rptDeptNameList.WindowState = FormWindowState.Maximized;
            //rptDeptNameList.rptName = "DeptCode";
            //rptDeptNameList.rptLabel = "DEPARTMENTS REFERENCE LIST SORTED BY CODE";
            //rptDeptNameList.Show();


            string rpt = "";
            string strServer = "PSSQL01";
            string strDBName = "PTS";
            ReportDocument crDoc = new ReportDocument();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();

            rpt = @"\\gblnj4\GIS\Reports\" + "DepartmentNames.rpt";
            //rpt = Application.StartupPath + @"\Reports\" + "DepartmentsRef.rpt";
            crConnectionInfo.Type = ConnectionInfoType.SQL;
            crConnectionInfo.ServerName = strServer;
            crConnectionInfo.DatabaseName = strDBName;
            crConnectionInfo.IntegratedSecurity = true;
            crtableLogoninfo.ConnectionInfo = crConnectionInfo;
            crDoc.Load(rpt);
            DataTable dt = new DataTable();
            if (rptName == "DeptCode")
                dt = PSSClass.Departments.DepartmentsMaster(1);
            else
                dt = PSSClass.Departments.DepartmentsMaster(2);
            crDoc.Load(rpt);
            crDoc.SetDataSource(dt);

            ////Open the PrintDialog
            //this.printDialog1.Document = this.printDocument1;
            //DialogResult dr = this.printDialog1.ShowDialog();
            //if (dr == DialogResult.OK)
            //{
            //    //Get the Copy times
            //    int nCopy = this.printDocument1.PrinterSettings.Copies;
            //    //Get the number of Start Page
            //    int sPage = this.printDocument1.PrinterSettings.FromPage;
            //    //Get the number of End Page
            //    int ePage = this.printDocument1.PrinterSettings.ToPage;
            //    //Get the printer name
            //    string PrinterName = this.printDocument1.PrinterSettings.PrinterName;


            //    try
            //    {
            //        //Set the printer name to print the report to. By default the sample
            //        //report does not have a defult printer specified. This will tell the
            //        //engine to use the specified printer to print the report. Print out 
            //        //a test page (from Printer properties) to get the correct value.

            //        crDoc.PrintOptions.PrinterName = PrinterName;


            //        //Start the printing process. Provide details of the print job
            //        //using the arguments.
            //        crDoc.PrintToPrinter(nCopy, false, sPage, ePage);
            //    }
            //    catch (Exception err)
            //    {
            //        MessageBox.Show(err.ToString());
            //    }

            //}
            crDoc.PrintOptions.PrinterName = PSSClass.Users.UserPrinterName(LogIn.nUserID); //@"\\it5\46 IT Brother Printer";// @"\\it5\46 Brother FAX"; 
            crDoc.PrintToPrinter(1, false, 0, 0);
            //crDoc.PrintOptions.PrinterName = @"\\it5\46 IT Brother Printer";
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
                    bsFile.Filter = "DepartmentID<>0";
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
            txtCode.Text = dgvFile.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dgvFile.CurrentRow.Cells[1].Value.ToString();
            txtID.Text = dgvFile.CurrentRow.Cells[2].Value.ToString();
            txtSeqNo.Text = dgvFile.CurrentRow.Cells[3].Value.ToString();
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
            dgvFile.Columns[0].HeaderText = "DEPARTMENT CODE";
            dgvFile.Columns[1].HeaderText = "DEPARTMENT NAME";
            dgvFile.Columns[2].HeaderText = "DEPARTMENT ID";
            dgvFile.Columns[3].HeaderText = "SEQUENCE NO.";
            dgvFile.Columns[0].Width = 160;
            dgvFile.Columns[0].DefaultCellStyle.Padding = new Padding(30, 0, 0, 0);
            dgvFile.Columns[1].Width = 300;
            dgvFile.Columns[2].Visible = false;
            dgvFile.Columns[3].Width = 150;
            dgvFile.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true); tsbDelete.Enabled = false;
            txtCode.Focus();
        }

        private void EditRecord()
        {
            DataTable dtX = PSSClass.General.CheckEditMode("Departments", txtID.Text, LogIn.strUserID);
            if (dtX != null && dtX.Rows.Count >= 1)
            {
                string strU = "";
                foreach (DataRow dRow in dtX.Rows)
                {
                    strU += dRow["UserLogID"] + Environment.NewLine;
                }
                MessageBox.Show("The following user(s) is/are editing this record:" + Environment.NewLine + strU + Environment.NewLine + "Please resolve changes to be made with the other users.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            dtX.Dispose();
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            LoadData();
            txtCode.Focus(); btnClose.Visible = false;
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

        private void SaveRecord()
        {
            if (txtCode.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Department Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }

            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Department Name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            //PSSClass.General.SpellCheckIt(txtName);

            if (nMode == 1)
                txtID.Text = PSSClass.DataEntry.NewID("Departments", "DepartmentID").ToString();

            if (PSSClass.DataEntry.MatchingRecord("DepartmentID", "DepartmentCode", "Departments", txtCode.Text, nMode, Convert.ToInt16(txtID.Text), "") == true)
            {
                MessageBox.Show("Matching department code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }

            if (PSSClass.DataEntry.MatchingRecord("DepartmentID", "DepartmentName", "Departments", txtName.Text, nMode, Convert.ToInt16(txtID.Text), "") == true)
            {
                MessageBox.Show("Matching department name found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.SmallInt));
            sqlcmd.Parameters["@nMode"].Value = nMode;

            sqlcmd.Parameters.Add(new SqlParameter("@DeptID", SqlDbType.Int));
            sqlcmd.Parameters["@DeptID"].Value = Convert.ToInt16(txtID.Text);

            sqlcmd.Parameters.Add(new SqlParameter("@DeptCode", SqlDbType.VarChar));
            sqlcmd.Parameters["@DeptCode"].Value = txtCode.Text.ToUpper();

            sqlcmd.Parameters.Add(new SqlParameter("@DeptName", SqlDbType.VarChar));
            sqlcmd.Parameters["@DeptName"].Value = txtName.Text.ToUpper();

            sqlcmd.Parameters.Add(new SqlParameter("@DeptSeqNo", SqlDbType.Int));
            if (txtSeqNo.Text.Trim() == "")
                sqlcmd.Parameters["@DeptSeqNo"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@DeptSeqNo"].Value = Convert.ToInt16(txtSeqNo.Text);
            
            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID;

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditDept";
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
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("DepartmentID", txtID.Text, bsFile, dgvFile);
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
            ClearControls(this);
            AddEditMode(false);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            nMode = 0;
        }

        private void Departments_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Departments");

            LoadRecords();
            DataGridSetting();
            BuildPrintItems();
            BuildSearchItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Departments");

            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true;
            }
        }

        private void Departments_KeyDown(object sender, KeyEventArgs e)
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
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
        }

        private void txtSeqNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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
                dt = PSSClass.Departments.DepartmentsMaster(1);
                PSSClass.DataEntry.DGVSearch(tstbSearchField.Text, tstbSearch.Text, dt, bsFile);
                nCtr = 0;
                tstbSearch.Text = "";
                timer1.Enabled = false;
                nSw = 0;
            }
        }

        //private void txtName_Enter(object sender, EventArgs e)
        //{
        //    string strC = textBoxSpeller1.Content.ToString();
        //    MessageBox.Show(strC);
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
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = true;
            }
            else 
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsbDelete.Enabled = false;
            }
        }
    }
}
