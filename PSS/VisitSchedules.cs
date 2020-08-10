using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;

namespace PSS
{
    public partial class VisitSchedules : PSS.TemplateForm
    {
        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private int nCtr = 0;
        private int nSw = 0;
        private int visitid = 0;
        private DataTable dtSponsors = new DataTable();
        private DataTable dtVisitors = new DataTable();
        private string strFileAccess = "RO";

        public VisitSchedules()
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
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void PrtLoginClickHandler(object sender, EventArgs e)
        {
            btnLSPreview_Click(null, null);
        }

        private void btnLSPreview_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                LabRpt rpt = new LabRpt();
                rpt.rptName = "LoadSchedules";
                rpt.WindowState = FormWindowState.Maximized;
        

                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    string exc = ex.Message;
                }
            }
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0].Text = "Visit Sheet";
            items[0].Click += new EventHandler(PrtLoginClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void VisitSchedules_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "VisitSchedules");

            LoadRecords();
            LoadSponsorsDDL();
            LoadVisitorsProc();
            
            dgvSponsors.Visible = false;
            dgvVisitors.Visible = false;
            DataGridSetting();
            BuildPrintItems();
            BuildSearchItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
          
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false;
            }
            else if (strFileAccess == "RW" || strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true;
            }
            else
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false;
            }
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Visits.LoadSchedules();
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

        private void LoadSponsorsDDL()
        {
            dgvSponsors.DataSource = null;
            dtSponsors = PSSClass.Sponsors.SponsorNamesDDL();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            bsSponsors.DataSource = dtSponsors;
            dgvSponsors.DataSource = bsSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 371;
            dgvSponsors.Columns[1].Visible = false;
        }


        private void LoadVisitorsProc()
        {
            //call procedure, populate and be able to select...and save
            dgvVisitors.DataSource = null;
            dtVisitors = PSSClass.Visits.LoadVisitors();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            dgvVisitors.DataSource = dtVisitors;
            StandardDGVSetting(dgvVisitors);
            dgvVisitors.Columns[0].Width = 371;
            dgvVisitors.Columns[1].Visible = false;
        }

        private void LoadRecords()
        {
            nMode = 0;
            DataTable dt = PSSClass.Visits.LoadSchedules();
            bsFile.DataSource = dt;
            bsFile.Filter = "VisitID <> 0";
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            FileAccess();
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

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            dtpStartTime.Value = Convert.ToDateTime("09:00 AM");
            dtpEndTime.Value = Convert.ToDateTime("05:00 PM");
            btnBrowse.Enabled = true;
            dgvSponsors.Visible = false; dgvVisitors.Visible = false;
            dtpDate.Value = Convert.ToDateTime(DateTime.Now.ToShortDateString()); dtpDate.Select();
            btnOpen.Enabled = false;
        }

        private void EditRecord()
        {
            nMode = 2;
            LoadData();
            OpenControls(this.pnlRecord, true);
            btnClose.Visible = false; btnBrowse.Enabled = true;btnOpen.Enabled = false;
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

            }
            LoadRecords();
        }


        private void SaveRecord()
        {
            int visitorType = 0;
            int VisitCode = 0;
            int visitorID = 0;
            //int saveSuccess = 0;
            
            //Disabled by AMDC 4/28/2016
            //==========================
            //if (txtName.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please enter a File Name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    txtName.Focus();
            //    return;
            //}
            //==========================

            //select valid visitor Type
            if (rdoClient.Checked == false && rdoOthers.Checked == false)
            {
                MessageBox.Show("Please select a valid Visitor Type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else {
                if (rdoClient.Checked == true)

                {
                    visitorType = 1;
                }
                else if (rdoOthers.Checked == true)
                {
                    visitorType = 2;
                    visitorID = int.Parse(txtVisitorID.Text);
                }
            }
            //select a valid visitor code

            if (rdoAudit.Checked == false && rdoInspection.Checked == false && rdoVisit.Checked == false)
            {
                MessageBox.Show("Please select a valid Visit Type.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else 
            {
                if (rdoAudit.Checked == true)
                    VisitCode = 1;
                else if (rdoInspection.Checked == true)
                    VisitCode = 2;
                else if (rdoVisit.Checked == true)
                    VisitCode = 3;
            }

            OpenControls(this.pnlRecord, false);

            string sEnd = dtpEndTime.Value.ToString("HH:mm:ss.fff");
            TimeSpan tsEnd = TimeSpan.Parse(sEnd);

            string sDate = dtpStartTime.Value.ToString("HH:mm:ss.fff");
            TimeSpan tsStart = TimeSpan.Parse(sDate);

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@VisitID", visitid);
            sqlcmd.Parameters.AddWithValue("@VisitDate", dtpDate.Value);
            sqlcmd.Parameters.AddWithValue("@StartTime", tsStart);
            sqlcmd.Parameters.AddWithValue("@EndTime", tsEnd);
            sqlcmd.Parameters.AddWithValue("@VisitorType", visitorType);
            sqlcmd.Parameters.AddWithValue("@SponsorID", txtSponsorID.Text);
            //sqlcmd.Parameters.AddWithValue("@SponsorName", txtSponsor.Text); Disabled by AMDC 4/28/2016
            //sqlcmd.Parameters.AddWithValue("@VisitorName", txtVisitorsName.Text);//Disabled by AMDC 4/28/2016
            sqlcmd.Parameters.AddWithValue("@FileLocation", txtAgendaFile.Text);
            sqlcmd.Parameters.AddWithValue("@VisitorID", visitorID);
            sqlcmd.Parameters.AddWithValue("@VisitCode", VisitCode);//Disabled by AMDC 4/28/2016
            sqlcmd.Parameters.AddWithValue("@Cancelled", Convert.ToBoolean(chkCancelled.CheckState));//Disabled by AMDC 4/28/2016
            //sqlcmd.Parameters.AddWithValue("@CreatedByID", 375);//Disabled by AMDC 4/28/2016
            //sqlcmd.Parameters.AddWithValue("@LastUpdateByID", 375);//Disabled by AMDC 4/28/2016
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID); //Added by AMDC 4/28/2016

            sqlcmd.CommandType = CommandType.StoredProcedure;
            //sqlcmd.CommandText = "spScheduleVisit"; disabled by AMDC 4/28/2016
            sqlcmd.CommandText = "spAddEditVisitSched";//created by AMDC 4/28/2016
            try
            {
                //saveSuccess = sqlcmd.ExecuteNonQuery(); Disabled by AMDC 4/28/2016
                sqlcmd.ExecuteNonQuery();
                //Disabled by AMDC 4/28/2016
                //============================
                //if(saveSuccess != 0)
                //{
                //    MessageBox.Show("Record saved successfully", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                //}
                //else
                //{
                //    MessageBox.Show("Cannot Save at this time, Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //============================
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
            PSSClass.General.FindRecord("VisitDate", dtpDate.Value.ToShortDateString(), bsFile, dgvFile);
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
            OpenControls(this.pnlRecord, false);
            ClearControls(this);
            AddEditMode(false);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            nMode = 0;
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

        private void SearchOKClickHandler(object sender, EventArgs e)
        {
            if (tstbSearch.Text.Trim() != "")
            {
                try
                {
                    bsFile.Filter = "DateFrom is not null";
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

        private void SearchItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            tstbSearchField.Text = clickedItem.Name;
            tstbSearch.SelectAll();
            tstbSearch.Focus();
            nIndex = tsddbSearch.DropDownItems.IndexOf(clickedItem);
            tslSearchData.Text = clickedItem.Text;
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            LoadRecords();
            tsbRefresh.Enabled = false;
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
            FileAccess();

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

        private void cklSelIdxChEventHandler(object sender, EventArgs e)
        {
            string strCol = cklColumns.Items[cklColumns.SelectedIndex].ToString().Replace(" ", "");
            if (dgvFile.Columns[strCol].Visible == true)
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = false;
            else
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = true;
            cklColumns.Visible = false;
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns[0].HeaderText = "VISIT DATE";
            dgvFile.Columns["VisitDate"].DefaultCellStyle.Format = "MM/dd/yyyy";//added by AMDC
            dgvFile.Columns[1].HeaderText = "START TIME";
            dgvFile.Columns[2].HeaderText = "END TIME";
            dgvFile.Columns[3].HeaderText = "PURPOSE";
            dgvFile.Columns[4].HeaderText = "VISITOR'S NAME";
            dgvFile.Columns[5].HeaderText = "AGENDA FILE";
            dgvFile.Columns[6].HeaderText = "CANCELLED";
            dgvFile.Columns[0].Width = 90;
            dgvFile.Columns[1].Width = 80;
            dgvFile.Columns[2].Width = 80;
            dgvFile.Columns[2].Visible = true;
            dgvFile.Columns[6].Visible = false;
            dgvFile.Columns[7].Visible = false;
            dgvFile.Columns[8].Visible = false;
            dgvFile.Columns[9].Visible = false;
            dgvFile.Columns[10].Visible = false;//added by AMDC 4/28/2016
            dgvFile.Columns[3].Width = 80;
            dgvFile.Columns[4].Width = 500;
            dgvFile.Columns[5].Width = 200;
            dgvFile.Columns[6].Width = 100;
            dgvFile.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;//revised by AMDC 4/28/2016
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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; 
            }
            else
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsbDelete.Enabled = false;
            }
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            btnClose.Visible = true; btnClose.BringToFront();
            OpenControls(pnlRecord, false); OpenControls(pnlVisitorType, false); OpenControls(pnlPurpose, false); btnBrowse.Enabled = false;
            //txtSponsorID.Enabled = true; disabled by AMDC 4/28/2016
            //txtVisitorsID.Enabled = true; disabled by AMDC 4/28/2016
            //txtSponsor.Enabled = true; disabled by AMDC 4/28/2016
            //txtVisitorsName.Enabled = true; disabled by AMDC 4/28/2016
            txtSponsorID.Text = dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString();
            txtSponsor.Text = dgvFile.CurrentRow.Cells["SponsorName"].Value.ToString();
            dtpDate.Value = Convert.ToDateTime(dgvFile.CurrentRow.Cells["VisitDate"].Value.ToString());
            txtVisitorID.Text = dgvFile.CurrentRow.Cells["VisitCode"].Value.ToString();
            txtVisitor.Text  = dgvFile.CurrentRow.Cells["VisitorName"].Value.ToString();
            txtAgendaFile.Text = dgvFile.CurrentRow.Cells["AgendaFile"].Value.ToString();//added by AMDC 4/29/2016
            visitid = int.Parse(dgvFile.CurrentRow.Cells["VisitID"].Value.ToString());
            if (dgvFile.CurrentRow.Cells["StartTime"].Value.ToString() != string.Empty)
            {
                dtpStartTime.Value = Convert.ToDateTime(dgvFile.CurrentRow.Cells["StartTime"].Value.ToString());
            }
            if (dgvFile.CurrentRow.Cells["EndTime"].Value.ToString() != string.Empty)
            {
                dtpEndTime.Value = Convert.ToDateTime(dgvFile.CurrentRow.Cells["EndTime"].Value.ToString());
            }
            //if (dgvFile.CurrentRow.Cells["VisitCode"].Value.ToString() == "1") //disabled by AMDC 4/28/2016
            if (dgvFile.CurrentRow.Cells["VisitorType"].Value.ToString() == "1")
            {
                rdoClient.Checked = true;
                txtVisitorID.Text = string.Empty;
                txtVisitor.Text = string.Empty;
            }
            //   txtVisitorsID = null;
            else if (dgvFile.CurrentRow.Cells["VisitorType"].Value.ToString() == "2") //revised by AMDC 4/28/2016 as per above
            {
                rdoOthers.Checked = true;
                txtSponsorID.Text = string.Empty;
                txtSponsor.Text = string.Empty;
            }
            //  }

            //select a valid visit code
            {
                if (dgvFile.CurrentRow.Cells["Purpose"].Value.ToString() == "Audit")
                    rdoAudit.Checked = true;
                else if (dgvFile.CurrentRow.Cells["Purpose"].Value.ToString() == "Inspection")
                    rdoInspection.Checked = true;
                else if (dgvFile.CurrentRow.Cells["Purpose"].Value.ToString() == "Visit")
                    rdoVisit.Checked = true;
            }
            chkCancelled.Checked = Convert.ToBoolean(dgvFile.CurrentRow.Cells["Cancelled"].Value);
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            dgvSponsors.Visible = false;
            dgvVisitors.Visible = false;
            btnOpen.Enabled = true; btnBrowse.Enabled = false;
        }


        private void VisitSchedules_KeyDown(object sender, KeyEventArgs e)
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
                //    if (nMode == 0 && strFileAccess == "FA")
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

        private void rdoClient_CheckedChanged(object sender, EventArgs e)
        {
            txtVisitorID.Enabled = false;
            txtVisitor.Enabled = false;
            txtVisitor.Text = string.Empty;
            txtVisitorID.Text = string.Empty;
            txtSponsorID.Enabled = true;
            txtSponsor.Enabled = true;
            dgvSponsors.Visible = true;
            dgvVisitors.Visible = false;
        }

        private void rdoOthers_CheckedChanged(object sender, EventArgs e)
        {
            txtVisitorID.Enabled = true;
            txtVisitor.Enabled = true;
            txtSponsorID.Enabled = false;
            txtSponsor.Enabled = false;
            txtSponsor.Text = string.Empty;
            txtSponsorID.Text = string.Empty;
            dgvSponsors.Visible = false;
            dgvVisitors.Visible = true;
        }

        private void txtSponsor_TextChanged_1(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSponsors;
                dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvSponsors, dvwSponsors);
            }
        }

        private void txtSponsorID_TextChanged(object sender, EventArgs e)
        {
            if (txtSponsorID.Text.Trim() == "")
            {
                txtSponsor.Text = "";
            }
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
        
            dgvSponsors.Visible = false; 
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                dgvSponsors.Visible = false;
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront();
            }
        }

        private void dgvVisitors_DoubleClick(object sender, EventArgs e)
        {
            txtVisitor.Text = dgvVisitors.CurrentRow.Cells[0].Value.ToString();
            txtVisitorID.Text = dgvVisitors.CurrentRow.Cells[1].Value.ToString();
            dgvSponsors.Visible = false;
            dgvVisitors.Visible = false;
        }

        private void txtVisitorsName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvVisitors.Visible = true; dgvVisitors.BringToFront();
            }
        }

        private void txtVistorsName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                dgvVisitors.Visible = false;

        }

        private void txtVisitorsName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwVisitors;
                dvwVisitors = new DataView(dtVisitors, "VisitorName like '" + txtVisitor.Text.Trim().Replace("'", "''") + "%'", "VisitorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvVisitors, dvwVisitors);
            }
        }

        private void btnLSPrinter_Click(object sender, EventArgs e)
        {
            //
            string strPrinter = PSSClass.Users.UserPrinterName(LogIn.nUserID);

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ReportDocument crDoc = new ReportDocument();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            sqlcmd.Connection = sqlcnn;

            string rpt = @"\\gblnj4\GIS\Reports\" + "LoadSchedules.rpt";

            crDoc.Load(rpt);

            sqlcmd = new SqlCommand("spLoadSchedules", sqlcnn);
     
            sqldr = sqlcmd.ExecuteReader();

            DataTable dTable = new DataTable();

            try
            {
                dTable.Load(sqldr);
            }
            catch
            {
            }
            crDoc.SetDataSource(dTable);
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();

            //Open the PrintDialog
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.AllowSelection = true;
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.AllowCurrentPage = true;
            DialogResult dr = this.printDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //Get the Copy times
                int nCopy = this.printDocument1.PrinterSettings.Copies;
                //Get the number of Start Page
                int sPage = this.printDocument1.PrinterSettings.FromPage;
                //Get the number of End Page
                int ePage = this.printDocument1.PrinterSettings.ToPage;
                //Get the printer name
                string PrinterName = this.printDocument1.PrinterSettings.PrinterName;
                try
                {
                    crDoc.PrintOptions.PrinterName = PrinterName;
                    crDoc.PrintToPrinter(nCopy, false, sPage, ePage);
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult result = opnFile.ShowDialog(); 
            {
                string file = opnFile.FileName;
                txtAgendaFile.Text = file;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
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
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.Select();
            dgvSponsors.Visible = false; 
            AddEditMode(false);
            FileAccess();
            try
            {
                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() != "" || dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "")
                    tsbEdit.Enabled = false;
            }
            catch { }
        }

        private void picSponsors_Click(object sender, EventArgs e)//added by AMDC 04/18/2016
        {

            dgvSponsors.Visible = true;
        }

        private void dgvSponsors_KeyDown(object sender, KeyEventArgs e)//added by AMDC 04/18/2016
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSponsors_KeyPress(object sender, KeyPressEventArgs e)//revised by AMDC 04/18/2016
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                dgvSponsors.Visible = false; 
            }
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)//added by AMDC 04/18/2016
        {
            dgvSponsors.Visible = false;
        }

        private void dgvVisitors_KeyDown(object sender, KeyEventArgs e)//added by AMDC 04/18/2016
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvVisitors_KeyPress(object sender, KeyPressEventArgs e)//added by AMDC 04/18/2016
        {
            if (e.KeyChar == 13)
            {
                txtVisitor.Text = dgvVisitors.CurrentRow.Cells[0].Value.ToString();
                txtVisitorID.Text = dgvVisitors.CurrentRow.Cells[1].Value.ToString();
                dgvVisitors.Visible = false;
            }
        }

        private void dgvVisitors_Leave(object sender, EventArgs e)//added by AMDC 04/18/2016
        {
            dgvVisitors.Visible = false;
        }

        private void picVisitors_Click(object sender, EventArgs e) //added by AMDC 04/18/2016
        {
            dgvVisitors.Visible = true;
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e) //added by AMDC 04/18/2016
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text == "")
                {
                    MessageBox.Show("Invalid Sponsor ID.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtSponsorID.Focus();
                    return;
                }
                dgvSponsors.Visible = false; txtAgendaFile.Focus();
            }
            else
            {
                txtSponsor.Text = "";
            }
        }

        private void VisitSchedules_Activated(object sender, EventArgs e)//added by AMDC 04/18/2016
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnOpen_Click(object sender, EventArgs e)//added by AMDC 4/29/2016
        {
            try
            {
                System.Diagnostics.Process.Start(txtAgendaFile.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

    }
}
