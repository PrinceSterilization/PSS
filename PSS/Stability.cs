using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

namespace GIS
{
    public partial class Stability : GIS.TemplateForm
    {
        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;

        DataTable dtStability = new DataTable();                                            // MY 11/09/2015 - Pop-up GridView Stability query
        DataTable dtDepartments = new DataTable();                                          // MY 11/09/2015 - Pop-up GridView Department query
        DataTable dtSponsors = new DataTable();                                             // MY 11/09/2015 - Pop-up GridView Sponsors query

        private string strFileAccess = "RO";

        public Stability()
        {
            InitializeComponent();
            LoadDepartments();
            LoadSponsors();

            BuildPrintItems();
            BuildSearchItems();

            bnMoveFirst.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveLast.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveNext.Click += new EventHandler(MoveRecordClickHandler);
            bnMovePrevious.Click += new EventHandler(MoveRecordClickHandler);

            //tsbAdd.Click += new EventHandler(AddClickHandler);
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
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            if (dtpStart.Value > dtpEnd.Value)
            {
                MessageBox.Show("Invalid date range." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            dtStability = GISClass.Samples.StabilityInfo(Convert.ToDateTime(dtpStart.Text), Convert.ToDateTime(dtpEnd.Text), Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtDeptID.Text));
            if (dtStability == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            bsFile.DataSource = dtStability;
            bnFile.BindingSource = bsFile;
            dgvStability.DataSource = bsFile;         
            tsbAdd.Enabled = false;
            DataGridSetting();
            dgvStability.ReadOnly = true;
        }

        private void LoadDepartments()
        {
            dgvDeptNames.DataSource = null;

            dtDepartments = GISClass.Samples.StabilityDepartments();
            if (dtDepartments == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvDeptNames.DataSource = dtDepartments;
            StandardDGVSetting(dgvDeptNames);
            dgvDeptNames.Columns[0].Width = 220;
            dgvDeptNames.Columns[1].Visible = false;
        }

        private void LoadSponsors()
        {
            dtSponsors = GISClass.Samples.StabilitySponsors();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvSponsorNames.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsorNames);
            dgvSponsorNames.Columns[0].Width = 220;
            dgvSponsorNames.Columns[1].Visible = false;                                                              // Vendor ID           
        }
        private void CloseClickHandler(object sender, EventArgs e)
        {
            if (nMode == 2)
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
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "StabilityReport";
            items[0].Text = "Stability Report";
            items[0].Click += new EventHandler(PrintReport);

            
            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = dtStability = GISClass.Samples.StabilityInfo(Convert.ToDateTime(dtpStart.Text), Convert.ToDateTime(dtpEnd.Text), Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtDeptID.Text));

            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered during build-up of search items." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void dgvCellChangedHandler(object sender, EventArgs e)
        {
            try
            {
                nIndex = dgvFile.CurrentCell.ColumnIndex;

                tsddbSearch.DropDownItems[nIndex].Select();
                tslSearchData.Text = tsddbSearch.DropDownItems[nIndex].Text;
                tstbSearchField.Text = tsddbSearch.DropDownItems[nIndex].Name;
            }
            catch
            { }
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                //LoadData();
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

        private void EditClickHandler(object sender, EventArgs e)
        {
            EditRecord();
        }

        private void DeleteClickHandler(object sender, EventArgs e)
        {
            //DeleteRecord();
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
                    bsFile.Filter = "GBLNo<>''";
                    GISClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvStability);
                    dgvStability.Select();
                   // LoadRecords(Convert.ToInt16(cboCutOffDays.Text));
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
                    else
                    {
                        bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;
                    }

                    tsbRefresh.Enabled = true;
                    
                }
                catch { }
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            bsFile.Filter = "GBLNo<>''";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";            
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void DataGridSetting()
        {
            dgvStability.EnableHeadersVisualStyles = false;
            dgvStability.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStability.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvStability.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvStability.Columns["GBLNo"].HeaderText = "GBL No";
            dgvStability.Columns["SponsorID"].HeaderText = "Sponsor ID";
            dgvStability.Columns["SponsorName"].HeaderText = "Sponsor Name";
            dgvStability.Columns["ECLength"].HeaderText = "EC Length";
            dgvStability.Columns["ECEndDate"].HeaderText = "EC End Date";
            dgvStability.Columns["StartDate"].HeaderText = "Start Date";
            dgvStability.Columns["EndDate"].HeaderText = "End Date";
            dgvStability.Columns["ServiceCode"].HeaderText = "Service Code";
            dgvStability.Columns["ServiceDesc"].HeaderText = "Service Desc";
            dgvStability.Columns["IncubatorNo"].HeaderText = "Incubator No.";
            dgvStability.Columns["TempSetting"].HeaderText = "Temp Setting";
            dgvStability.Columns["GBLNo"].Width = 60;
            dgvStability.Columns["SponsorID"].Width = 55;
            dgvStability.Columns["SponsorName"].Width = 200;
            dgvStability.Columns["ServiceCode"].Width = 70;
            dgvStability.Columns["ServiceDesc"].Width = 200;
            dgvStability.Columns["IncubatorNo"].Width = 70;
            dgvStability.Columns["TempSetting"].Width = 250;
            dgvStability.Columns["ECLength"].Width = 50;
            dgvStability.Columns["ECEndDate"].Width = 75;
            dgvStability.Columns["StartDate"].Width = 75;
            dgvStability.Columns["EndDate"].Width = 75;            
            dgvStability.Columns["ECEndDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvStability.Columns["StartDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvStability.Columns["EndDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvStability.Columns["GBLNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStability.Columns["StartDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStability.Columns["EndDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStability.Columns["ECEndDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStability.Columns["ECLength"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStability.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStability.Columns["Servicecode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvStability.Columns["ECCompType"].Visible = false;
            dgvStability.Columns["DepartmentID"].Visible = false;
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
            OpenControls(this.pnlRecord, false);
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
                OpenControls(this.pnlRecord, false);
            }
        }

        private void dgvKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void LoadEntry()
        {
            txtGBLNo.Text = "";
            txtServiceCode.Text = "";
            txtServiceDesc.Text = "";
            txtIncubatorNo.Text = "";
            txtTempSetting.Text = "";

            txtGBLNo.Text = dgvStability.CurrentRow.Cells["GBLNo"].Value.ToString();
            txtServiceCode.Text = dgvStability.CurrentRow.Cells["ServiceCode"].Value.ToString();
            txtServiceDesc.Text = dgvStability.CurrentRow.Cells["ServiceDesc"].Value.ToString();

            if (dgvStability.CurrentRow.Cells["IncubatorNo"].Value == null)
            {
                txtIncubatorNo.Text = dgvStability.CurrentRow.Cells["EqptCode"].Value.ToString();
            }           
            if (dgvStability.CurrentRow.Cells["TempSetting"].Value == null)
            {
                txtTempSetting.Text = dgvStability.CurrentRow.Cells["TempSetting"].Value.ToString();
            }           
        }

        private void EditRecord()
        {
            nMode = 2;
            dgvStability.ReadOnly = false;
        }

        private void SaveRecord()
        {
            dgvStability.CurrentCell = dgvStability.Rows[0].Cells[0];
            bsFile.EndEdit();

            DataTable dtX = dtStability.GetChanges();
            if (dtX != null && dtX.Rows.Count > 0)
            {
                for (int j = 0; j < dtX.Rows.Count; j++)                                             // DataGridView Detail Loop
                {
                    SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@GBLNo", dtX.Rows[j]["GBLNo"]);
                    sqlcmd.Parameters.AddWithValue("@ServiceCode", dtX.Rows[j]["ServiceCode"]);
                    if (dtX.Rows[j]["TempSetting"].ToString() == "XXX")
                        sqlcmd.Parameters.AddWithValue("@EqptCode", "");
                    else
                        sqlcmd.Parameters.AddWithValue("@EqptCode", dtX.Rows[j]["IncubatorNo"]);
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spUpdateIncubatorInfo";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sqlcnn.Dispose();
                        return;
                    }
                    sqlcnn.Dispose();
                }
            }
            dtStability.AcceptChanges();
            dgvStability.ReadOnly = true;
            AddEditMode(false);
            tsbAdd.Enabled = false;
        }

        private void CancelSave()
        { 
            if (nMode == 2)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            pnlIncubatorEntry.Visible = false;
            LoadRecords();
            AddEditMode(false);
            nMode = 0;
            tsbAdd.Enabled = false;
            bnFile.Enabled = true;
        }

        private void StabilityReport_Load(object sender, EventArgs e)
        {
            strFileAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "Sponsors");

            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }
            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            string dte = "1/1/" + DateTime.Now.Year.ToString();
            string sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");

            dtpStart.Value = Convert.ToDateTime(sdte);
            LoadRecords();
        }

        private void StabilityReport_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //case Keys.F2:
                //    if (nMode == 0 && strFileAccess != "RO")
                //    {
                //        AddEditMode(true); AddRecord();
                //    }
                //    break;

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            this.Close(); this.Dispose();
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

        // MY 11/02/2015 - START: txt/dgvDeptNames events
        private void dgvDeptNames_DoubleClick(object sender, EventArgs e)
        {
            txtDeptID.Text = dgvDeptNames.CurrentRow.Cells["DepartmentID"].Value.ToString();
            txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
            dgvDeptNames.Visible = false;
            LoadRecords();
        }

        private void dgvDeptNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvDeptNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtDeptID.Text = dgvDeptNames.CurrentRow.Cells["DepartmentID"].Value.ToString();
                txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
                dgvDeptNames.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvDeptNames.Visible = false;
            }
        }

        private void dgvDeptNames_Leave(object sender, EventArgs e)
        {
            dgvDeptNames.Visible = false;
        }

        private void txtDeptName_TextChanged(object sender, EventArgs e)
        {
            DataView dvwDeptNames;
            dvwDeptNames = new DataView(dtDepartments, "DepartmentName like '%" + txtDeptName.Text.Trim().Replace("'", "''") + "%'", "DepartmentName", DataViewRowState.CurrentRows);
            dgvDeptNames.Columns[0].Width = 220;
            dgvDeptNames.DataSource = dvwDeptNames;
        }

        private void dgvDeptNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtDeptID.Text = dgvDeptNames.CurrentRow.Cells["DepartmentID"].Value.ToString();
            txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
            dgvDeptNames.Visible = false;
            dgvDeptNames.BringToFront();
            LoadRecords();
        }

        private void picDeptNames_Click(object sender, EventArgs e)
        {
            txtSponsorID.Text = "0";
            txtSponsorName.Text = "";

            LoadDepartments();
            dgvDeptNames.Visible = true; dgvDeptNames.BringToFront();
        }
        // MY 11/02/2015 - END: txt/dgvDeptNames events    

        // MY 11/03/2015 - START: txt/dgvSponsorNames events
        private void dgvSponsorNames_DoubleClick(object sender, EventArgs e)
        {
            txtSponsorID.Text = dgvSponsorNames.CurrentRow.Cells["SponsorID"].Value.ToString();
            txtSponsorName.Text = dgvSponsorNames.CurrentRow.Cells["SponsorName"].Value.ToString();
            dgvSponsorNames.Visible = false;
            LoadRecords();
        }

        private void dgvSponsorNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSponsorNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsorID.Text = dgvSponsorNames.CurrentRow.Cells["SponsorID"].Value.ToString();
                txtSponsorName.Text = dgvSponsorNames.CurrentRow.Cells["SponsorName"].Value.ToString();
                dgvSponsorNames.Visible = false;
                LoadRecords();
            }
            else if (e.KeyChar == 27)
            {
                dgvSponsorNames.Visible = false;
            }
        }

        private void dgvSponsorNames_Leave(object sender, EventArgs e)
        {
            dgvSponsorNames.Visible = false;
        }

        private void txtSponsorName_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSponsorNames;
            dvwSponsorNames = new DataView(dtSponsors, "SponsorName like '%" + txtSponsorName.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
            dgvSponsorNames.Columns[0].Width = 220;
            dgvSponsorNames.DataSource = dvwSponsorNames;
        }

        private void dgvSponsorNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSponsorID.Text = dgvSponsorNames.CurrentRow.Cells["SponsorID"].Value.ToString();
            txtSponsorName.Text = dgvSponsorNames.CurrentRow.Cells["SponsorName"].Value.ToString();
            dgvSponsorNames.Visible = false;
            dgvSponsorNames.BringToFront();
        }

        private void picSponsorNames_Click(object sender, EventArgs e)
        {
            txtDeptID.Text = "0";
            txtDeptName.Text = "";

            LoadSponsors();
            dgvSponsorNames.Visible = true; dgvSponsorNames.BringToFront();
        }
        // MY 11/03/2015 - END: txt/dgvSponsorNames events   

        private void btnPrint_Click(object sender, EventArgs e)
        {
            SaveRecord();
            PrintReport(null, null);
        }

        private void PrintReport(object sender, EventArgs e)
        {
            StabilityReport rpt = new StabilityReport();
            if (txtDeptID.Text.Trim() == "0")
            {
                rpt.nDepartmentID = 0;
            }
            else
            {
                rpt.nDepartmentID = Convert.ToInt16(txtDeptID.Text.Trim());
            }

            if (txtSponsorID.Text.Trim() == "0")
            {
                rpt.nSponsorID = 0;
            }
            else
            {
                rpt.nSponsorID = Convert.ToInt16(txtSponsorID.Text.Trim());
            }
            rpt.dteStart = dtpStart.Value;
            rpt.dteEnd = dtpEnd.Value;
            rpt.WindowState = FormWindowState.Maximized;
            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void btnCloseDetails_Click(object sender, EventArgs e)
        {
            pnlIncubatorEntry.Visible = false;
            AddEditMode(false);
            tsbAdd.Enabled = false;
        }

        private void dgvStability_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadEntry();
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void dgvStability_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (nMode == 0 || dgvStability.CurrentCell.OwningColumn.Name.ToString() != "IncubatorNo")
                e.Cancel = true;
        }

        private void dgvStability_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStability.CurrentCell.OwningColumn.Name.ToString() == "IncubatorNo")
            {
                if (GISClass.Equipment.TempSetting(dgvStability.Rows[e.RowIndex].Cells["IncubatorNo"].Value.ToString()) != "")
                    dgvStability.Rows[e.RowIndex].Cells["TempSetting"].Value = GISClass.Equipment.TempSetting(dgvStability.Rows[e.RowIndex].Cells["IncubatorNo"].Value.ToString());
                else
                    dgvStability.Rows[e.RowIndex].Cells["TempSetting"].Value = "XXX";
            }
        }

        private void btnClearSel_Click(object sender, EventArgs e)
        {
            txtDeptID.Text = "0";
            txtDeptName.Text = "";
            txtSponsorID.Text = "0";
            txtSponsorName.Text = "";
            dgvDeptNames.Visible = false;
            dgvSponsorNames.Visible = false;
            LoadRecords();
        }
    }
}
