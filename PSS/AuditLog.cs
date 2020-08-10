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

namespace PSS
{
    public partial class AuditLog : PSS.TemplateForm
    {
        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";

        DataTable dtEmpAuditors = new DataTable();                                        // MY 12/04/2014 - Pop-up GridView Employee Auditors query
        DataTable dtSponsors = new DataTable();                                           // MY 12/04/2014 - Pop-up GridView Sponsors query

        public AuditLog()
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
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            txtEmpAuditorID.GotFocus += new EventHandler(txtEmpAuditorIDEnterHandler);
            txtSponsorID.GotFocus += new EventHandler(txtSponsorIDEnterHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.QA.AuditMaster();

            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;

            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            FileAccess();
            nMode = 0;
        }

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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; //tsbDelete.Enabled = true;
            }
        }

        private void LoadEmpAuditors()
        {
            dgvEmpAuditors.DataSource = null;

            dtEmpAuditors = PSSClass.QA.AMEmpAuditors();
            if (dtEmpAuditors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvEmpAuditors.DataSource = dtEmpAuditors;
            StandardDGVSetting(dgvEmpAuditors);
            dgvEmpAuditors.Columns[0].Width = 500;
            dgvEmpAuditors.Columns[1].Visible = false;
        }

        private void LoadSponsors()
        {
            dgvSponsors.DataSource = null;

            dtSponsors = PSSClass.QA.AMSponsors();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvSponsors.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 500;
            dgvSponsors.Columns[1].Visible = false;           
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
            this.Close();
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "AuditLogDetail";
            items[0].Text = "Audit Log Detail";
            items[0].Click += new EventHandler(PrintAuditLogDetailClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.QA.AuditMaster();

            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during build-up of search items." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
               
        private void PrintAuditLogDetailClickHandler(object sender, EventArgs e)
        {
            if (dgvFile.RowCount == 0)
            {
                MessageBox.Show("No records to print!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);               
                return;
            }

            AuditLogDetailRpt rpt = new AuditLogDetailRpt();
            txtSponsorID.Text = dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString();
            rpt.SponsorID = Convert.ToInt16(txtSponsorID.Text);
            rpt.WindowState = FormWindowState.Maximized;

            try
            {
                rpt.Show();
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
            if (tstbSearch.Text.Trim() != "")
            {
                try
                {
                    bsFile.Filter = "AuditID<>0";
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
                    else
                    {
                        bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;
                    }
                    dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                    tsbRefresh.Enabled = true;
                }
                catch { }
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            bsFile.Filter = "AuditID<>0";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            btnClose.Visible = true; btnClose.BringToFront();
            
            txtAuditID.Text = dgvFile.CurrentRow.Cells["AuditID"].Value.ToString();
            DateTime dtDateCreated = Convert.ToDateTime(dgvFile.CurrentRow.Cells["DateCreated"].Value.ToString());
            String fmtDateCreated = dtDateCreated.ToString("MM/dd/yyyy");
            mskDateEntered.Text = fmtDateCreated;
            if (dgvFile.CurrentRow.Cells["AuditDate"].Value.ToString() != "")
            {
                DateTime dtAuditDate = Convert.ToDateTime(dgvFile.CurrentRow.Cells["AuditDate"].Value.ToString());
                String fmtAuditDate = dtAuditDate.ToString("MM/dd/yyyy");
                mskAuditDate.Text = fmtAuditDate;
            }
            if (dgvFile.CurrentRow.Cells["DateCARCompleted"].Value.ToString() != "")
            {
                DateTime dtCARCompleted = Convert.ToDateTime(dgvFile.CurrentRow.Cells["DateCARCompleted"].Value.ToString());
                String fmtCARCompleted = dtCARCompleted.ToString("MM/dd/yyyy");
                mskDateCARComp.Text = fmtCARCompleted;
            }            
            txtEmpAuditorID.Text = dgvFile.CurrentRow.Cells["EmpAuditorID"].Value.ToString();
            txtEmpAuditor.Text = dgvFile.CurrentRow.Cells["EmpAuditorName"].Value.ToString();
            txtSponsorID.Text = dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString();
            txtSponsor.Text = dgvFile.CurrentRow.Cells["SponsorName"].Value.ToString();
            txtAuditorName.Text = dgvFile.CurrentRow.Cells["AuditorName"].Value.ToString();
            txtDeficiencyNo.Text = dgvFile.CurrentRow.Cells["DeficiencyNo"].Value.ToString();
            txtDeficiencyDesc.Text = dgvFile.CurrentRow.Cells["DeficiencyDesc"].Value.ToString();
            cboStatus.SelectedItem = dgvFile.CurrentRow.Cells["AuStatus"].Value.ToString();
            if (dgvFile.CurrentRow.Cells["ASNSigned"].Value.ToString() == "True")
                chkASN.Checked = true;
            else
                chkASN.Checked = false;
            if (dgvFile.CurrentRow.Cells["CNDSigned"].Value.ToString() == "True")
                chkCND.Checked = true;
            else
                chkCND.Checked = false;
            if (dgvFile.CurrentRow.Cells["AgendaReceived"].Value.ToString() == "True")
                chkRecAuditAgenda.Checked = true;
            else
                chkRecAuditAgenda.Checked = false;
            if (dgvFile.CurrentRow.Cells["ReportReceived"].Value.ToString() == "True")
                chkRecAuditReport.Checked = true;
            else
                chkRecAuditReport.Checked = false;
            if (dgvFile.CurrentRow.Cells["ContOfBusiness"].Value.ToString() == "True")
                chkContBuss.Checked = true;
            else
                chkContBuss.Checked = false;
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
            dgvFile.Columns["DateCreated"].HeaderText = "Date Entered";
            dgvFile.Columns["AuditDate"].HeaderText = "Audit Date";
            dgvFile.Columns["AuStatus"].HeaderText = "Audit Status";
            dgvFile.Columns["DateCarCompleted"].HeaderText = "Date CAR Completed";
            dgvFile.Columns["EmpAuditorName"].HeaderText = "PSS Employee";
            dgvFile.Columns["SponsorName"].HeaderText = "Sponsor Name";
            dgvFile.Columns["AuditorName"].HeaderText = "Auditor Name(s)";
            dgvFile.Columns["DeficiencyNo"].HeaderText = "No. of Deficiencies";
            dgvFile.Columns["DeficiencyDesc"].HeaderText = "Description of Deficiencies";
            dgvFile.Columns["ASNSigned"].HeaderText = "ASN Signed";
            dgvFile.Columns["CNDSigned"].HeaderText = "CND Signed";
            dgvFile.Columns["AgendaReceived"].HeaderText = "Agenda Received";
            dgvFile.Columns["ReportReceived"].HeaderText = "Report Received";
            dgvFile.Columns["ContOfBusiness"].HeaderText = "Cont. of Business";
            dgvFile.Columns["DateCreated"].Width = 75;
            dgvFile.Columns["AuditDate"].Width = 75;
            dgvFile.Columns["AuStatus"].Width = 150;
            dgvFile.Columns["DateCarCompleted"].Width = 100;            
            dgvFile.Columns["EmpAuditorName"].Width = 150;            
            dgvFile.Columns["SponsorName"].Width = 250;
            dgvFile.Columns["AuditorName"].Width = 250;
            dgvFile.Columns["DeficiencyNo"].Width = 80;
            dgvFile.Columns["DeficiencyDesc"].Width = 300;
            dgvFile.Columns["ASNSigned"].Width = 65;
            dgvFile.Columns["CNDSigned"].Width = 65;
            dgvFile.Columns["AgendaReceived"].Width = 65;
            dgvFile.Columns["ReportReceived"].Width = 65;
            dgvFile.Columns["ContOfBusiness"].Width = 65;
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["AuditDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateCARCompleted"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["AuditDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateCARCompleted"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DeficiencyNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["EmpAuditorID"].Visible = false;
            dgvFile.Columns["SponsorID"].Visible = false;
            dgvFile.Columns["AuditID"].Visible = false;
            dgvFile.Columns["AuditDate"].Frozen = true;
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
                LoadData();
            }
        }

        private void dgvKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void txtEmpAuditorIDEnterHandler(object sender, EventArgs e)
        {
            dgvEmpAuditors.Visible = false;
        }

        private void txtSponsorIDEnterHandler(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            txtAuditID.Text = "< New >";            
            mskDateEntered.Enabled = false;
           
            DateTime dtEntryDate = DateTime.Today;
            String fmtEntryDate = dtEntryDate.ToString("MM/dd/yyyy");
            mskDateEntered.Text = fmtEntryDate;
            txtDeficiencyNo.Text = "0";
            mskAuditDate.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            if (pnlRecord.Visible == false)
                LoadData();
            OpenControls(this.pnlRecord, true);
            txtAuditID.Enabled = false;
            mskDateEntered.Enabled = false;
            txtAuditorName.Focus();
            btnClose.Visible = false;
        }

        private void DeleteRecord()
        {
            LoadData();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                SqlCommand sqlcmd = new SqlCommand();

                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.Add(new SqlParameter("@AuditID", SqlDbType.Int));
                sqlcmd.Parameters["@AuditID"].Value = Convert.ToInt16(txtAuditID.Text);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelAuditMaster";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadRecords();
        }

        private void SaveRecord()
        {
            if (txtEmpAuditorID.Text.Trim() == "")
            {
                MessageBox.Show("Please choose an Employee Auditor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtEmpAuditorID.Focus();
                return;
            }

            if (txtSponsorID.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Sponsor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsorID.Focus();
                return;
            }
            //if (txtDeficiencyNo.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please enter number of deficiency.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    txtDeficiencyNo.Focus();
            //    return;
            //}
            if (Convert.ToInt16(txtDeficiencyNo.Text) > 999)
            {
                MessageBox.Show("Deficiency number can't be more than 999!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDeficiencyNo.Focus();
                return;
            }

            //if (cboStatus.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Please select audit status.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    cboStatus.Focus();
            //    return;
            //}

            byte nC = 0;
            try
            {
                DateTime dtCARComp = Convert.ToDateTime(mskDateCARComp.Text);
                nC = 1;
            }
            catch {}

            if (nMode == 1)
                txtAuditID.Text = PSSClass.DataEntry.NewID("AuditLogMaster", "AuditID").ToString();           

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@AuditID", Convert.ToInt16(txtAuditID.Text));
            if (mskAuditDate.Text != "")
            {
                sqlcmd.Parameters.AddWithValue("@AuditDate", Convert.ToDateTime(mskAuditDate.Text));
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@AuditDate", DBNull.Value);
            }
            if (nC == 1)
            {
                sqlcmd.Parameters.AddWithValue("@DateCARCompleted", Convert.ToDateTime(mskDateCARComp.Text));
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@DateCARCompleted", DBNull.Value);
            }
            sqlcmd.Parameters.AddWithValue("@DeficiencyNo", Convert.ToInt16(txtDeficiencyNo.Text));
            sqlcmd.Parameters.AddWithValue("@EmpAuditorID", Convert.ToInt16(txtEmpAuditorID.Text));
            sqlcmd.Parameters.AddWithValue("@SponsorID", Convert.ToInt16(txtSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@AuditorName", txtAuditorName.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@DeficiencyDesc", txtDeficiencyDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@StatusCode", cboStatus.SelectedIndex + 1);
            sqlcmd.Parameters.AddWithValue("@ASNSigned", Convert.ToInt16(chkASN.CheckState));
            sqlcmd.Parameters.AddWithValue("@CNDSigned", Convert.ToInt16(chkASN.CheckState));
            sqlcmd.Parameters.AddWithValue("@AgendaRec", Convert.ToInt16(chkRecAuditAgenda.CheckState));
            sqlcmd.Parameters.AddWithValue("@RptRec", Convert.ToInt16(chkRecAuditReport.CheckState));
            sqlcmd.Parameters.AddWithValue("@ContBuss", Convert.ToInt16(chkContBuss.CheckState));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditAuditMaster";
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
            sqlcmd.Dispose(); sqlcnn.Dispose(); 
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            LoadRecords();
            PSSClass.General.FindRecord("AuditID", txtAuditID.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            AddEditMode(false);
            MessageBox.Show("Record successfully saved!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            nMode = 0;
        }

        private void AuditLog_Load(object sender, EventArgs e)
        {
            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }

            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "AuditLog");
            LoadRecords();
            LoadEmpAuditors();
            LoadSponsors();

            BuildPrintItems();
            BuildSearchItems();

            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
        }

        private void AuditLog_KeyDown(object sender, KeyEventArgs e)
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            LoadRecords();
            dgvFile.Focus();            
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
        
        // MY 12/04/2014 - START: txt/dgvEmpAuditors events
        private void dgvEmpAuditors_DoubleClick(object sender, EventArgs e)
        {
            txtEmpAuditor.Text = dgvEmpAuditors.CurrentRow.Cells["EmpAuditorName"].Value.ToString();
            txtEmpAuditorID.Text = dgvEmpAuditors.CurrentRow.Cells["EmpAuditorID"].Value.ToString();
            dgvEmpAuditors.Visible = false;
        }

        private void dgvEmpAuditors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvEmpAuditors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtEmpAuditor.Text = dgvEmpAuditors.CurrentRow.Cells["EmpAuditorName"].Value.ToString();
                txtEmpAuditorID.Text = dgvEmpAuditors.CurrentRow.Cells["EmpAuditorID"].Value.ToString();
                dgvEmpAuditors.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvEmpAuditors.Visible = false;
            }
        }
       
        private void dgvEmpAuditors_Leave(object sender, EventArgs e)
        {
            dgvEmpAuditors.Visible = false;
        }

        private void txtEmpAuditors_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwEmpAuditors;
                dvwEmpAuditors = new DataView(dtEmpAuditors, "EmpAuditorName like '%" + txtEmpAuditor.Text.Trim().Replace("'", "''") + "%'", "EmpAuditorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvEmpAuditors, dvwEmpAuditors);
                dgvEmpAuditors.Columns[0].Width = 352;
            }
        }

        private void dgvEmpAuditors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtEmpAuditor.Text = dgvEmpAuditors.CurrentRow.Cells["EmpAuditorName"].Value.ToString();
            txtEmpAuditorID.Text = dgvEmpAuditors.CurrentRow.Cells["EmpAuditorID"].Value.ToString();
            dgvEmpAuditors.Visible = false;
        }

        private void picEmpAuditors_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                LoadEmpAuditors();
                dgvEmpAuditors.Visible = true; dgvEmpAuditors.BringToFront(); //dgvEmpAuditors.Top = 75;
            }
        }

        private void txtEmpAuditorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
                e.Handled = e.KeyChar != (char)Keys.Back;

            if (e.KeyChar == 13)
            {
                txtEmpAuditor.Text = PSSClass.QA.AMEmpAuditorName(Convert.ToInt16(txtEmpAuditorID.Text));

                if (txtEmpAuditor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Employee Auditor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvEmpAuditors.Visible = false;
            }
            else
            {
                txtEmpAuditor.Text = ""; dgvEmpAuditors.Visible = false;
            }
        }

        private void txtEmpAuditorID_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
              dgvEmpAuditors.Visible = true; dgvEmpAuditors.BringToFront();
              dgvSponsors.Visible = false; 
            }
        }

        // MY 12/02/2014 - END: txt/dgvEmpAuditors events

        // MY 12/05/2014 - START: txt/dgvSponsors events
        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells["SponsorName"].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells["SponsorID"].Value.ToString();
            dgvSponsors.Visible = false;
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
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells["SponsorName"].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells["SponsorID"].Value.ToString();
                dgvSponsors.Visible = false;
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

        private void txtSponsors_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSponsors;
                dvwSponsors = new DataView(dtSponsors, "SponsorName like '%" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvSponsors, dvwSponsors);
                dgvSponsors.Columns[0].Width = 352;
            }
        }

        private void dgvSponsors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells["SponsorName"].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells["SponsorID"].Value.ToString();
            dgvSponsors.Visible = false;
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                LoadSponsors();
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); 
            }
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
                e.Handled = e.KeyChar != (char)Keys.Back;

            if (e.KeyChar == 13)
            {
                txtSponsor.Text = PSSClass.QA.AMSponsorName(Convert.ToInt16(txtSponsorID.Text));

                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvSponsors.Visible = false;
            }
            else
            {
                txtSponsor.Text = ""; dgvSponsors.Visible = false;
            }
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvSponsors.Visible = false; dgvEmpAuditors.Visible = false;
            }
        }

        // MY 12/02/2014 - END: txt/dgvSponsors events


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

        private void txtDeficiencyNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
                e.Handled = e.KeyChar != (char)Keys.Back;
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvEmpAuditors.Visible = false;
            }
        }

        private void txtEmpAuditor_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvEmpAuditors.Visible = true; dgvEmpAuditors.BringToFront(); dgvSponsors.Visible = false;
            }
        }

        private void txtEmpAuditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                dgvEmpAuditors.Visible = false;
            else if (e.KeyChar == 13)
                dgvEmpAuditors.Select();
            else
                txtEmpAuditorID.Text = "";
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                dgvSponsors.Visible = false;
            else if (e.KeyChar == 13)
                dgvSponsors.Select();
            else
                txtSponsorID.Text = "";
        }

        private void calAudit_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (calAudit.Top == mskAuditDate.Top)
            {
                mskAuditDate.Text = calAudit.SelectionRange.Start.ToString("MM/dd/yyyy");
                mskAuditDate.Select();
            }
            else
            {
                mskDateCARComp.Text = calAudit.SelectionRange.Start.ToString("MM/dd/yyyy");
                mskDateCARComp.Select();
            }
            calAudit.Visible = false;
        }

        private void mskAuditDate_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                calAudit.Visible = true; calAudit.BringToFront();
                calAudit.Left = mskAuditDate.Left + mskAuditDate.Width + 2;
                calAudit.Top = mskAuditDate.Top;
                calAudit.Select();
            }
        }

        private void mskAuditDate_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                calAudit.Visible = true; calAudit.BringToFront();
                calAudit.Left = mskAuditDate.Left + mskAuditDate.Width + 2;
                calAudit.Top = mskAuditDate.Top;
                calAudit.Select();
            }
        }

        private void mskAuditDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (nMode != 0)
            {
                calAudit.Visible = true;
            }
            e.SuppressKeyPress = true;
        }

        private void mskDateCARComp_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                calAudit.Visible = true; calAudit.BringToFront();
                calAudit.Left = mskDateCARComp.Left + mskDateCARComp.Width + 2;
                calAudit.Top = mskDateCARComp.Top;
                calAudit.Select();
            }
        }

        private void mskDateCARComp_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                calAudit.Visible = true; calAudit.BringToFront();
                calAudit.Left = mskDateCARComp.Left + mskDateCARComp.Width + 2;
                calAudit.Top = mskDateCARComp.Top;
                calAudit.Select();
            }
        }

        private void mskDateCARComp_KeyDown(object sender, KeyEventArgs e)
        {
            if (nMode != 0)
            {
                calAudit.Visible = true;
            }
            e.SuppressKeyPress = true;
        }

        private void mskAuditDate_Leave(object sender, EventArgs e)
        {
            if (!calAudit.Focused)
            {
                calAudit.Visible = false;
            }
        }

        private void calAudit_Leave(object sender, EventArgs e)
        {
            calAudit.Visible = false;
        }
    }
}
