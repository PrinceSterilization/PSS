using System;
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
    public partial class TestDataLabels : PSS.TemplateForm
    {
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private int nBatch = 0, nBatchN90 = 0;
        private string strFileAccess = "RO";
        private int nR = 0;

        private string[,] arrLabels = new string[6, 15] ;
        private string[,] arrLabelsN90 = new string[6, 15];

        DataTable dtHeaders = new DataTable();
        DataTable dtSponsors = new DataTable();
        DataTable dtSC = new DataTable();
        
        public TestDataLabels()
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "TestDataLabels");

            LoadRecords();
            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }

            InitializeComponent();

            LoadSponsorsDDL();
            LoadSCDDL();

            BuildPrintItems();
            BuildSearchItems();

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
            tstbSearch.KeyPress += new KeyPressEventHandler(SearchKeyPressHandler);
            tsbFilter.Click += new EventHandler(SearchFilterClickHandler);
            tsbRefresh.Click += new EventHandler(RefreshClickHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            dgvFile.Click += new EventHandler(dgvClickEventHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            dtHeaders = PSSClass.Samples.TestDataLabels();
            if (dtHeaders == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            bsFile.DataSource = dtHeaders;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            FileAccess();
            dgvFile.CurrentCell = dgvFile.Rows[nR].Cells[0];
            if (dgvFile.CurrentRow.Cells["Applied"].Value.ToString() == "True")
                tsbEdit.Enabled = false;
            else
                tsbEdit.Enabled = true;
        }

        private void LoadSponsorsDDL()
        {
            dgvSponsors.DataSource = null;
            dtSponsors = PSSClass.Sponsors.SponsorNamesDDL();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            dgvSponsors.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 458;
            dgvSponsors.Columns[1].Visible = false;
        }

        private void LoadSCDDL()
        {
            dgvSC.DataSource = null;
            dtSC = PSSClass.ServiceCodes.SCDDL();
            if (dtSC == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            dgvSC.DataSource = dtSC;
            StandardDGVSetting(dgvSC);
            dgvSC.Columns[0].Width = 458;
            dgvSC.Columns[1].Visible = false;
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvFile.Columns["ServiceCode"].Width = 75;
            dgvFile.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvFile.Columns["ServiceDesc"].Width = 300;
            dgvFile.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvFile.Columns["SponsorID"].Width = 75;
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["SponsorName"].Width = 300;
            dgvFile.Columns["FormatNo"].HeaderText = "FORMAT NO.";
            dgvFile.Columns["FormatNo"].Width = 75;
            dgvFile.Columns["FormatNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["TableDesc"].HeaderText = "DESCRIPTION";
            dgvFile.Columns["TableDesc"].Width = 250;
            dgvFile.Columns["TableReportID"].HeaderText = "TABLE REPORT ID";
            dgvFile.Columns["TableReportID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["FinalReportID"].HeaderText = "FINAL REPORT ID";
            dgvFile.Columns["FinalReportID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["TableReportID"].Width = 75;
            dgvFile.Columns["FinalReportID"].Width = 75;
            dgvFile.Columns["Applied"].Visible = true;
            dgvFile.Columns["Applied"].HeaderText = "APPLIED";
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
            //ToolStripMenuItem[] items = new ToolStripMenuItem[2];

            //items[0] = new ToolStripMenuItem();
            //items[0].Name = "DepartmentCode";
            //items[0].Text = "Sorted by Department Code";
            //items[0].Click += new EventHandler(PrintDeptCodeClickHandler);

            //items[1] = new ToolStripMenuItem();
            //items[1].Name = "DepartmentName";
            //items[1].Text = "Sorted by Department Name";
            //items[1].Click += new EventHandler(PrintDeptNameClickHandler);

            //tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Samples.TestDataLabels();
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

        private void dgvClickEventHandler(object sender, EventArgs e)
        {
            tstbSearchField.Text = dgvFile.CurrentCell.OwningColumn.Name;
        }

        private void dgvCellChangedHandler(object sender, EventArgs e)
        {
            try
            {
                nIndex = dgvFile.CurrentCell.ColumnIndex;
                nR = dgvFile.CurrentCell.RowIndex;

                tsddbSearch.DropDownItems[nIndex].Select();
                tslSearchData.Text = tsddbSearch.DropDownItems[nIndex].Text;
                tstbSearchField.Text = tsddbSearch.DropDownItems[nIndex].Name;
                FileAccess();
                if (dgvFile.CurrentRow.Cells["Applied"].Value.ToString() == "True")
                    tsbEdit.Enabled = false;
                else
                    tsbEdit.Enabled = true;
            }
            catch { }
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
        //private void PrintDeptNameClickHandler(object sender, EventArgs e)
        //{
        //    RptDept rptDeptNameList = new RptDept();
        //    rptDeptNameList.WindowState = FormWindowState.Maximized;
        //    rptDeptNameList.rptName = "DeptName";
        //    rptDeptNameList.rptLabel = "DEPARTMENTS REFERENCE LIST SORTED BY NAME";
        //    rptDeptNameList.Show();
        //}

        //private void PrintDeptCodeClickHandler(object sender, EventArgs e)
        //{
        //    RptDept rptDeptNameList = new RptDept();
        //    rptDeptNameList.WindowState = FormWindowState.Maximized;
        //    rptDeptNameList.rptName = "DeptCode";
        //    rptDeptNameList.rptLabel = "DEPARTMENTS REFERENCE LIST SORTED BY CODE";
        //    rptDeptNameList.Show();
        //}

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
                    bsFile.Filter = "SponsorID<>0";
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
            bsFile.Filter = "SponsorID<>0";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void SearchKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SearchFilterClickHandler(null, null);
            }
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnCopy.Visible = true;
            txtSC.Text = dgvFile.CurrentRow.Cells["ServiceCode"].Value.ToString();
            txtSCDesc.Text = dgvFile.CurrentRow.Cells["ServiceDesc"].Value.ToString();
            txtSponsorID.Text = dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString();
            txtSponsor.Text = dgvFile.CurrentRow.Cells["SponsorName"].Value.ToString();
            txtFormatNo.Text = dgvFile.CurrentRow.Cells["FormatNo"].Value.ToString();
            txtDesc.Text = dgvFile.CurrentRow.Cells["TableDesc"].Value.ToString();
            txtTableRptID.Text = dgvFile.CurrentRow.Cells["TableReportID"].Value.ToString();
            txtFinalRptID.Text = dgvFile.CurrentRow.Cells["FinalReportID"].Value.ToString();
            if (dgvFile.CurrentRow.Cells["Applied"].Value.ToString() == "True")
                chkApplied.Checked = true;
            else
                chkApplied.Checked = false;

            DataTable dt = new DataTable();
            dt = PSSClass.Samples.ExTestDataLabels(Convert.ToInt16(txtSC.Text), Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtFormatNo.Text));
            if (dt == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            btnBatch.Text = "&Next";
            //Load Labels to array
            //First 90
            arrLabels = new string[6, 15];
            nBatch = 0;
            SetUpDGV();
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 1).ToString();
                arrLabels[0, i] = dt.Rows[0][strLbl].ToString();
            }

            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 16).ToString();
                arrLabels[1, i] = dt.Rows[0][strLbl].ToString();
            }

            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 31).ToString();
                arrLabels[2, i] = dt.Rows[0][strLbl].ToString();
            }
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 46).ToString();
                arrLabels[3, i] = dt.Rows[0][strLbl].ToString();
            }
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 61).ToString();
                arrLabels[4, i] = dt.Rows[0][strLbl].ToString();
            }
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 76).ToString();
                arrLabels[5, i] = dt.Rows[0][strLbl].ToString();
            }
            //Load Labels to array
            //Next 90
            arrLabelsN90 = new string[6, 15];
            nBatchN90 = 0;
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 1 + 90).ToString();
                try
                {
                    arrLabelsN90[0, i] = dt.Rows[0][strLbl].ToString();
                }
                catch
                {
                    arrLabelsN90[0, i] = "";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 16 + 90).ToString();
                try
                {
                    arrLabelsN90[1, i] = dt.Rows[0][strLbl].ToString();
                }
                catch
                {
                    arrLabelsN90[1, i] = "";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 31 + 90).ToString();
                try
                {
                    arrLabelsN90[2, i] = dt.Rows[0][strLbl].ToString();
                }
                catch 
                {
                    arrLabelsN90[2, i] = "";
                }
            }
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 46 + 90).ToString();
                try
                {
                    arrLabelsN90[3, i] = dt.Rows[0][strLbl].ToString();
                }
                catch
                {
                    arrLabelsN90[3, i] = "";
                }
            }
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 61 + 90).ToString();
                try
                {
                    arrLabelsN90[4, i] = dt.Rows[0][strLbl].ToString();
                }
                catch
                {
                    arrLabelsN90[4, i] = "";
                }
            }
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "L" + (i + 76 + 90).ToString();
                try
                {
                    arrLabelsN90[5, i] = dt.Rows[0][strLbl].ToString();
                }
                catch
                {
                    arrLabelsN90[5, i] = "";
                }
            }
            //Load arrayvalues to datagridviews
            //First 90
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    dgvCol1.Rows[i].Cells[1].Value = arrLabels[0, i];
                }
                catch { }
            }
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    dgvCol2.Rows[i].Cells[1].Value = arrLabels[1, i];
                }
                catch { }
            }
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    dgvCol3.Rows[i].Cells[1].Value = arrLabels[2, i];
                }
                catch { }
            }
            //Next 90
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    dgvCol4.Rows[i].Cells[1].Value = arrLabelsN90[0, i];
                }
                catch { }
            }
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    dgvCol5.Rows[i].Cells[1].Value = arrLabelsN90[1, i];
                }
                catch { }
            }
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    dgvCol6.Rows[i].Cells[1].Value = arrLabelsN90[2, i];
                }
                catch { }
            }

            txtSD1.Text = dt.Rows[0]["SD1"].ToString();
            txtSD2.Text = dt.Rows[0]["SD2"].ToString(); 
            txtSD3.Text = dt.Rows[0]["SD3"].ToString();
            txtSD4.Text = dt.Rows[0]["SD4"].ToString();
            dgvSC.Visible = false; dgvSponsors.Visible = false;
        }

        private void SetUpDGV()
        {
            //First90
            dgvCol1.Rows.Clear(); dgvCol1.Columns.Clear();
            dgvCol2.Rows.Clear(); dgvCol2.Columns.Clear();
            dgvCol3.Rows.Clear(); dgvCol3.Columns.Clear();

            dgvCol1.RowCount = 15; dgvCol2.RowCount = 15; dgvCol3.RowCount = 15;
            dgvCol1.ColumnCount = 2; dgvCol2.ColumnCount = 2; dgvCol3.ColumnCount = 2;

            dgvCol1.Columns[0].Width = 95; dgvCol1.Columns[1].Width = 196; dgvCol1.Enabled = true;
            dgvCol2.Columns[0].Width = 95; dgvCol2.Columns[1].Width = 196; dgvCol2.Enabled = true;
            dgvCol3.Columns[0].Width = 95; dgvCol3.Columns[1].Width = 196; dgvCol3.Enabled = true;

            dgvCol1.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            dgvCol2.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            dgvCol3.Columns[0].DefaultCellStyle.BackColor = Color.Beige;

            //Next 90
            dgvCol4.Rows.Clear(); dgvCol4.Columns.Clear();
            dgvCol5.Rows.Clear(); dgvCol5.Columns.Clear();
            dgvCol6.Rows.Clear(); dgvCol6.Columns.Clear();

            dgvCol4.RowCount = 15; dgvCol5.RowCount = 15; dgvCol6.RowCount = 15;
            dgvCol4.ColumnCount = 2; dgvCol5.ColumnCount = 2; dgvCol6.ColumnCount = 2;

            dgvCol4.Columns[0].Width = 95; dgvCol4.Columns[1].Width = 196; dgvCol4.Enabled = true;
            dgvCol5.Columns[0].Width = 95; dgvCol5.Columns[1].Width = 196; dgvCol5.Enabled = true;
            dgvCol6.Columns[0].Width = 95; dgvCol6.Columns[1].Width = 196; dgvCol6.Enabled = true;

            dgvCol4.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            dgvCol5.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            dgvCol6.Columns[0].DefaultCellStyle.BackColor = Color.Beige;

            for (int i = 0; i < 15; i++)
            {
                if (nBatch == 0)
                    dgvCol1.Rows[i].Cells[0].Value = "Label  " + (i + 1).ToString();
                else if (nBatch == 1)
                    dgvCol1.Rows[i].Cells[0].Value = "Label  " + (i + 1 + 45).ToString();
            }

            for (int i = 0; i < 15; i++)
            {
                if (nBatch == 0)
                    dgvCol2.Rows[i].Cells[0].Value = "Label  " + (i + 16).ToString();
                else if (nBatch == 1)
                    dgvCol2.Rows[i].Cells[0].Value = "Label  " + (i + 16 + 45).ToString();
            }

            for (int i = 0; i < 15; i++)
            {
                if (nBatch == 0)
                    dgvCol3.Rows[i].Cells[0].Value = "Label  " + (i + 31).ToString();
                else if (nBatch == 1)
                    dgvCol3.Rows[i].Cells[0].Value = "Label  " + (i + 31 + 45).ToString();
            }
            dgvCol1.ClearSelection(); dgvCol2.ClearSelection(); dgvCol3.ClearSelection();
            dgvCol1.CurrentCell = dgvCol1.Rows[0].Cells[1];

            for (int i = 0; i < 15; i++)
            {
                if (nBatchN90 == 0)
                    dgvCol4.Rows[i].Cells[0].Value = "Label  " + (i + 1 + 90).ToString();
                else if (nBatchN90 == 1)
                    dgvCol4.Rows[i].Cells[0].Value = "Label  " + (i + 1 + 45 + 90).ToString();
            }

            for (int i = 0; i < 15; i++)
            {
                if (nBatchN90 == 0)
                    dgvCol5.Rows[i].Cells[0].Value = "Label  " + (i + 16 + 90).ToString();
                else if (nBatchN90 == 1)
                    dgvCol5.Rows[i].Cells[0].Value = "Label  " + (i + 16 + 45 + 90).ToString();
            }

            for (int i = 0; i < 15; i++)
            {
                if (nBatchN90 == 0)
                    dgvCol6.Rows[i].Cells[0].Value = "Label  " + (i + 31 + 90).ToString();
                else if (nBatchN90 == 1)
                    dgvCol6.Rows[i].Cells[0].Value = "Label  " + (i + 31 + 45 + 90).ToString();
            }
            dgvCol4.ClearSelection(); dgvCol5.ClearSelection(); dgvCol6.ClearSelection();
            dgvCol4.CurrentCell = dgvCol4.Rows[0].Cells[1]; 
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
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

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false; btnCopy.Visible = false;
            txtSC.Enabled = true; txtSCDesc.Enabled = true;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            SetUpDGV();
            arrLabels = new string[6, 15];
            nBatch = 0; nBatchN90 = 0; btnClear.Enabled = true;
            txtSC.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            LoadData();
            //if (chkApplied.Checked == true)
            //{
            //    MessageBox.Show("This form has been applied to reports." + Environment.NewLine + "Editing mode is terminated.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    ClearControls(this);
            //    AddEditMode(false);
            //    LoadRecords();
            //    dgvSC.Visible = false; dgvSponsors.Visible = false;
            //    pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            //    nMode = 0; btnBatch.Text = "&Next"; btnClear.Enabled = false;
            //    return;
            //}
            btnClear.Enabled = true;
            txtSponsorID.ReadOnly = true; txtSponsor.ReadOnly = true; btnCopy.Visible = false;
            txtSC.ReadOnly = true; txtSCDesc.ReadOnly = true; txtFormatNo.ReadOnly = true;
            btnClose.Visible = false; txtDesc.SelectAll(); txtDesc.Focus(); 
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
                ;
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.Add(new SqlParameter("@SC", SqlDbType.SmallInt));
                sqlcmd.Parameters["@SC"].Value = Convert.ToInt16(txtSC.Text);

                sqlcmd.Parameters.Add(new SqlParameter("@SpID", SqlDbType.SmallInt));
                sqlcmd.Parameters["@SpID"].Value = Convert.ToInt16(txtSponsorID.Text);

                sqlcmd.Parameters.Add(new SqlParameter("@FormatNo", SqlDbType.TinyInt));
                sqlcmd.Parameters["@FormatNo"].Value = Convert.ToInt16(txtFormatNo.Text);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelTestDataLabel";

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
            if (txtSponsorID.Text.Trim() == "")
            {
                MessageBox.Show("Please enter select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsor.Focus();
                return;
            }

            if (txtSC.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Service Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSC.Focus();
                return;
            }

            if (txtFormatNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Format No..", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtFormatNo.Focus();
                return;
            }
            else
            {
                try
                {
                    int n = int.Parse(txtFormatNo.Text);
                    if (n < 0)
                    {
                        MessageBox.Show("Please enter a valid integer number > 0.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtFormatNo.Focus();
                        return;
                    }
                }
                catch 
                {
                    MessageBox.Show("Please enter a number.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtFormatNo.Focus();
                    return;
                }
            }

            chkApplied.Focus();

            string strDataLabels = "<TestDataLabels SC=" + (char)34 + txtSC.Text + (char)34 + " FormatNo=" + (char)34 + txtFormatNo.Text + (char)34 + ">";
            //First 90           
            string strX = "";
            for (int i = 0; i < 15; i++)
            {
                if (arrLabels[0, i] != null)
                {
                    if (arrLabels[0, i].ToString().Trim() != "")
                    {
                        strX = arrLabels[0, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strDataLabels = strDataLabels + "<Label" + (i + 1).ToString() + ">" + strX + "</Label" + (i + 1).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 1).ToString() + "></Label" + (i + 1).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 1).ToString() + "></Label" + (i + 1).ToString() + ">";
                }
            }
            
            for (int i = 0; i < 15; i++)
            {
                if (arrLabels[1, i] != null)
                {
                    if (arrLabels[1, i].ToString().Trim() != "")
                    {
                        strX = arrLabels[1, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strDataLabels = strDataLabels + "<Label" + (i + 16).ToString() + ">" + strX + "</Label" + (i + 16).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 16).ToString() + "></Label" + (i + 16).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 16).ToString() + "></Label" + (i + 16).ToString() + ">";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (arrLabels[2, i] != null)
                {
                    if (arrLabels[2, i].ToString().Trim() != "")
                    {
                        strX = arrLabels[2, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strDataLabels = strDataLabels + "<Label" + (i + 31).ToString() + ">" + strX + "</Label" + (i + 31).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 31).ToString() + "></Label" + (i + 31).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 31).ToString() + "></Label" + (i + 31).ToString() + ">";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (arrLabels[3, i] != null)
                {
                    if (arrLabels[3, i].ToString().Trim() != "")
                    {
                        strX = arrLabels[3, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strDataLabels = strDataLabels + "<Label" + (i + 46).ToString() + ">" + strX + "</Label" + (i + 46).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 46).ToString() + "></Label" + (i + 46).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 46).ToString() + "></Label" + (i + 46).ToString() + ">";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (arrLabels[4, i] != null)
                {
                    if (arrLabels[4, i].ToString().Trim() != "")
                    {
                        strX = arrLabels[4, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");

                        strDataLabels = strDataLabels + "<Label" + (i + 61).ToString() + ">" + strX + "</Label" + (i + 61).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 61).ToString() + "></Label" + (i + 61).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 61).ToString() + "></Label" + (i + 61).ToString() + ">";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (arrLabels[5, i] != null)
                {
                    if (arrLabels[5, i].ToString().Trim() != "")
                    {
                        strX = arrLabels[5, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");

                        strDataLabels = strDataLabels + "<Label" + (i + 76).ToString() + ">" + strX + "</Label" + (i + 76).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 76).ToString() + "></Label" + (i + 76).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 76).ToString() + "></Label" + (i + 76).ToString() + ">";
                }
            }
            //Next90
            for (int i = 0; i < 15; i++)
            {
                if (arrLabelsN90[0, i] != null)
                {
                    if (arrLabelsN90[0, i].ToString().Trim() != "")
                    {
                        strX = arrLabelsN90[0, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strDataLabels = strDataLabels + "<Label" + (i + 91).ToString() + ">" + strX + "</Label" + (i + 91).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 91).ToString() + "></Label" + (i + 91).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 91).ToString() + "></Label" + (i + 91).ToString() + ">";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (arrLabelsN90[1, i] != null)
                {
                    if (arrLabelsN90[1, i].ToString().Trim() != "")
                    {
                        strX = arrLabelsN90[1, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strDataLabels = strDataLabels + "<Label" + (i + 106).ToString() + ">" + strX + "</Label" + (i + 106).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 106).ToString() + "></Label" + (i + 106).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 106).ToString() + "></Label" + (i + 106).ToString() + ">";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (arrLabelsN90[2, i] != null)
                {
                    if (arrLabelsN90[2, i].ToString().Trim() != "")
                    {
                        strX = arrLabelsN90[2, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strDataLabels = strDataLabels + "<Label" + (i + 121).ToString() + ">" + strX + "</Label" + (i + 121).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 121).ToString() + "></Label" + (i + 121).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 121).ToString() + "></Label" + (i + 121).ToString() + ">";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (arrLabelsN90[3, i] != null)
                {
                    if (arrLabelsN90[3, i].ToString().Trim() != "")
                    {
                        strX = arrLabelsN90[3, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");
                        strDataLabels = strDataLabels + "<Label" + (i + 136).ToString() + ">" + strX + "</Label" + (i + 136).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 136).ToString() + "></Label" + (i + 136).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 136).ToString() + "></Label" + (i + 136).ToString() + ">";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (arrLabelsN90[4, i] != null)
                {
                    if (arrLabelsN90[4, i].ToString().Trim() != "")
                    {
                        strX = arrLabelsN90[4, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");

                        strDataLabels = strDataLabels + "<Label" + (i + 151).ToString() + ">" + strX + "</Label" + (i + 151).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 151).ToString() + "></Label" + (i + 151).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 151).ToString() + "></Label" + (i + 151).ToString() + ">";
                }
            }

            for (int i = 0; i < 15; i++)
            {
                if (arrLabelsN90[5, i] != null)
                {
                    if (arrLabelsN90[5, i].ToString().Trim() != "")
                    {
                        strX = arrLabelsN90[5, i].Replace("&", "&amp;");
                        strX = strX.Replace(">", "&gt;");
                        strX = strX.Replace("<", "&lt;");
                        strX = strX.Replace("'", "&apos;");
                        strX = strX.Replace("\"", "&quot;");

                        strDataLabels = strDataLabels + "<Label" + (i + 166).ToString() + ">" + strX + "</Label" + (i + 166).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 166).ToString() + "></Label" + (i + 166).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 166).ToString() + "></Label" + (i + 166).ToString() + ">";
                }
            }
            strX = txtSD1.Text.Trim().Replace("&", "&amp;");
            strX = strX.Replace(">", "&gt;");
            strX = strX.Replace("<", "&lt;");
            strX = strX.Replace("'", "&apos;");
            strX = strX.Replace("\"", "&quot;");

            strDataLabels = strDataLabels + "<SD1>" + strX + "</SD1>";

            strX = txtSD2.Text.Trim().Replace("&", "&amp;");
            strX = strX.Replace(">", "&gt;");
            strX = strX.Replace("<", "&lt;");
            strX = strX.Replace("'", "&apos;");
            strX = strX.Replace("\"", "&quot;");

            strDataLabels = strDataLabels + "<SD2>" + strX + "</SD2>";

            strX = txtSD3.Text.Trim().Replace("&", "&amp;");
            strX = strX.Replace(">", "&gt;");
            strX = strX.Replace("<", "&lt;");
            strX = strX.Replace("'", "&apos;");
            strX = strX.Replace("\"", "&quot;");

            strDataLabels = strDataLabels + "<SD3>" + strX + "</SD3>";

            strX = txtSD4.Text.Trim().Replace("&", "&amp;");
            strX = strX.Replace(">", "&gt;");
            strX = strX.Replace("<", "&lt;");
            strX = strX.Replace("'", "&apos;");
            strX = strX.Replace("\"", "&quot;");

            strDataLabels = strDataLabels + "<SD4>" + strX + "</SD4>";

            strDataLabels = strDataLabels + "</TestDataLabels>";

            //strDataLabels = strDataLabels.Replace("<", "&lt;");
            //strDataLabels = strDataLabels.Replace(">", "&gt;");
            //strDataLabels = strDataLabels.Replace("'", "&apos;");
            //strDataLabels = strDataLabels.Replace("\"", "&quot;");
            //strDataLabels = strDataLabels.Replace("&", "&amp;");

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(txtSC.Text));
            sqlcmd.Parameters.AddWithValue("@FormatNo", Convert.ToInt16(txtFormatNo.Text));
            sqlcmd.Parameters.AddWithValue("@Desc", txtDesc.Text);
            sqlcmd.Parameters.AddWithValue("@TableRptID", txtTableRptID.Text);
            sqlcmd.Parameters.AddWithValue("@FinalRptID", txtFinalRptID.Text);
            sqlcmd.Parameters.AddWithValue("@DataLabels", strDataLabels);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditTestDataLabels";
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
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            dgvSC.Visible = false; dgvSponsors.Visible = false;
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("SponsorID", txtSponsorID.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            nMode = 0; nBatch = 0; nBatchN90 = 0; btnBatch.Text = "&Next"; btnClear.Enabled = false;
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
            dgvSC.Visible = false; dgvSponsors.Visible = false;
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            nMode = 0; btnBatch.Text = "&Next"; btnClear.Enabled = false;
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

        private void dgvCol1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol2_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0)) 
                e.Cancel = true;
        }

        private void dgvCol3_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvSponsors.Visible = false; txtFormatNo.Focus();
            }
            else if (nMode == 1)
            {
                txtSponsor.Text = ""; dgvSponsors.Visible = false;
            }
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); txtSponsor.SelectAll(); dgvSC.Visible = false;
            }
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSponsors;
            dvwSponsors = new DataView(dtSponsors, "SponsorName like '%" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
            dvwSetUp(dgvSponsors, dvwSponsors); dgvSponsors.Columns[0].Width = 380;
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            dgvSponsors.Visible = false; txtFormatNo.Focus();
        }

        private void dgvSponsors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                dgvSponsors.Visible = false; txtFormatNo.Focus();
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

        private void dgvSponsors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                LoadSponsorsDDL(); dgvSponsors.Visible = true;
            }
        }

        private void txtSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
                if (txtSCDesc.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Service Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvSC.Visible = false; txtSponsor.Focus();
            }
            else if (nMode == 1)
            {
                dgvSC.Visible = false; txtSCDesc.Text = "";
            }
        }

        private void txtSCDesc_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSC;
            dvwSC= new DataView(dtSC, "ServiceDesc like '%" + txtSCDesc.Text.Trim().Replace("'", "''") + "%'", "ServiceDesc", DataViewRowState.CurrentRows);
            dvwSetUp(dgvSC, dvwSC); dgvSC.Columns[0].Width = 380;
        }

        private void picSC_Click(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                LoadSCDDL(); dgvSC.Visible = true;
            }
        }

        private void txtSCDesc_Enter(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                dgvSC.Visible = true; dgvSC.BringToFront(); txtSC.SelectAll();
            }
        }

        private void dgvSC_DoubleClick(object sender, EventArgs e)
        {
            txtSCDesc.Text = dgvSC.CurrentRow.Cells[0].Value.ToString();
            txtSC.Text = dgvSC.CurrentRow.Cells[1].Value.ToString();
            dgvSC.Visible = false; txtSponsor.Focus();
        }

        private void dgvSC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSCDesc.Text = dgvSC.CurrentRow.Cells[0].Value.ToString();
                txtSC.Text = dgvSC.CurrentRow.Cells[1].Value.ToString();
                dgvSC.Visible = false; txtSponsor.Focus();
            }
            else if (e.KeyChar == 27)
            {
                dgvSC.Visible = false;
            }
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 1)
                txtSponsorID.Text = "";
        }

        private void txtSCDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 1)
                txtSC.Text = "";
        }

        private void dgvSC_Leave(object sender, EventArgs e)
        {
            dgvSC.Visible = false;
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                dgvSponsors.Visible = false; dgvSC.Visible = false;
            }
        }

        private void dgvCol1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvCol1.CurrentRow.Cells[0].Value.ToString().IndexOf("15") != -1)
                {
                    dgvCol1.ClearSelection();
                    dgvCol2.Select();
                    dgvCol2.CurrentCell = dgvCol2.Rows[0].Cells[1];
                }
            }
        }

        private void dgvCol2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvCol2.CurrentRow.Cells[0].Value.ToString().IndexOf("30") != -1)
                {
                    dgvCol2.ClearSelection();
                    dgvCol3.Select();
                    dgvCol3.CurrentCell = dgvCol3.Rows[0].Cells[1];
                }
            }
        }

        private void dgvCol3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvCol3.CurrentRow.Cells[0].Value.ToString().IndexOf("45") != -1)
                {
                    dgvCol3.ClearSelection();
                    dgvCol1.Select();
                    dgvCol1.CurrentCell = dgvCol1.Rows[0].Cells[1];
                }
            }
        }

        private void btnBatch_Click(object sender, EventArgs e)
        {
            if (btnBatch.Text == "&Next")
            {
                nBatch = 1;
            }
            else
            {
                nBatch = 0;
            }

            SetUpDGV();
            if (btnBatch.Text == "&Next")
            {
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol1.Rows[i].Cells[1].Value = arrLabels[3, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol2.Rows[i].Cells[1].Value = arrLabels[4, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol3.Rows[i].Cells[1].Value = arrLabels[5, i];
                    }
                    catch { }
                }
                btnBatch.Text = "&Previous";
            }
            else
            {
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol1.Rows[i].Cells[1].Value = arrLabels[0, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol2.Rows[i].Cells[1].Value = arrLabels[1, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol3.Rows[i].Cells[1].Value = arrLabels[2, i];
                    }
                    catch { }
                }
                btnBatch.Text = "&Next";
            }
        }

        private void dgvCol1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCol1.CurrentCell.Value != null)
            {
                if (nBatch == 0)
                    arrLabels[0, e.RowIndex] = dgvCol1.CurrentCell.Value.ToString();
                else
                    arrLabels[3, e.RowIndex] = dgvCol1.CurrentCell.Value.ToString();
            }
            else
            {
                if (nBatch == 0)
                    arrLabels[0, e.RowIndex] = "";
                else
                    arrLabels[3, e.RowIndex] = "";
            }
        }

        private void dgvCol2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCol2.CurrentCell.Value != null)
            {
                if (nBatch == 0)
                    arrLabels[1, e.RowIndex] = dgvCol2.CurrentCell.Value.ToString();
                else
                    arrLabels[4, e.RowIndex] = dgvCol2.CurrentCell.Value.ToString();
            }
            else
            {
                if (nBatch == 0)
                    arrLabels[1, e.RowIndex] = "";
                else
                    arrLabels[4, e.RowIndex] = "";
            }
        }

        private void dgvCol3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCol3.CurrentCell.Value != null)
            {
                if (nBatch == 0)
                    arrLabels[2, e.RowIndex] = dgvCol3.CurrentCell.Value.ToString();
                else
                    arrLabels[5, e.RowIndex] = dgvCol3.CurrentCell.Value.ToString();
            }
            else
            {
                if (nBatch == 0)
                    arrLabels[2, e.RowIndex] = "";
                else
                    arrLabels[5, e.RowIndex] = "";
            }
        }

        private void txtFormatNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtDesc.Focus();
            }
        }

        private void TestDataLabels_Load(object sender, EventArgs e)
        {
            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();   
        }

        private void txtSC_Enter(object sender, EventArgs e)
        {
            dgvSC.Visible = false; dgvSponsors.Visible = false;
        }

        private void txtTableRptID_Enter(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                txtTableRptID.Text = txtSC.Text + "_" + txtFormatNo.Text;
            }
        }

        private void txtFinalRptID_Enter(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                txtFinalRptID.Text = txtSC.Text + "_" + txtFormatNo.Text + "_Final";
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            AddEditMode(true);
            nMode = 1;
            OpenControls(pnlRecord,true);
            //txtSC.Text = ""; txtSCDesc.Text = "";
            txtSponsorID.Text = ""; txtSponsor.Text = ""; 
            txtSC.Focus();
            btnCopy.Visible = false; btnClear.Enabled = true;
        }

        private void txtSC_Leave(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (txtSCDesc.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Service Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
                dgvSC.Visible = false; txtSponsor.Focus();
            }
        }

        private void btnNext90_Click(object sender, EventArgs e)
        {
            if (btnNext90.Text == "&Next")
            {
                nBatchN90 = 1;
            }
            else
            {
                nBatchN90 = 0;
            }
            SetUpDGV();
            if (btnNext90.Text == "&Next")
            {
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol4.Rows[i].Cells[1].Value = arrLabelsN90[3, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol5.Rows[i].Cells[1].Value = arrLabelsN90[4, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol6.Rows[i].Cells[1].Value = arrLabelsN90[5, i];
                    }
                    catch { }
                }
                btnNext90.Text = "&Previous";
            }
            else
            {
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol4.Rows[i].Cells[1].Value = arrLabelsN90[0, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol5.Rows[i].Cells[1].Value = arrLabelsN90[1, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol6.Rows[i].Cells[1].Value = arrLabelsN90[2, i];
                    }
                    catch { }
                }
                btnNext90.Text = "&Next";
            }
        }

        private void dgvCol4_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol5_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol6_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tabLabels.SelectedTab.Name == "tabNext90")
            {
                if (dgvCol4.CurrentCell.Value != null)
                {
                    if (nBatchN90 == 0)
                        arrLabelsN90[0, e.RowIndex] = dgvCol4.CurrentCell.Value.ToString();
                    else
                        arrLabelsN90[3, e.RowIndex] = dgvCol4.CurrentCell.Value.ToString();
                }
                else
                {
                    if (nBatchN90 == 0)
                        arrLabelsN90[0, e.RowIndex] = "";
                    else
                        arrLabelsN90[3, e.RowIndex] = "";
                }
            }
        }

        private void dgvCol5_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tabLabels.SelectedTab.Name == "tabNext90")
            {
                if (dgvCol5.CurrentCell.Value != null)
                {
                    if (nBatchN90 == 0)
                        arrLabelsN90[1, e.RowIndex] = dgvCol5.CurrentCell.Value.ToString();
                    else
                        arrLabelsN90[4, e.RowIndex] = dgvCol5.CurrentCell.Value.ToString();
                }
                else
                {
                    if (nBatchN90 == 0)
                        arrLabelsN90[1, e.RowIndex] = "";
                    else
                        arrLabelsN90[4, e.RowIndex] = "";
                }
            }
        }

        private void dgvCol6_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (tabLabels.SelectedTab.Name == "tabNext90")
            {
                if (dgvCol6.CurrentCell.Value != null)
                {
                    if (nBatchN90 == 0)
                        arrLabelsN90[2, e.RowIndex] = dgvCol6.CurrentCell.Value.ToString();
                    else
                        arrLabelsN90[5, e.RowIndex] = dgvCol6.CurrentCell.Value.ToString();
                }
                else
                {
                    if (nBatchN90 == 0)
                        arrLabelsN90[2, e.RowIndex] = "";
                    else
                        arrLabelsN90[5, e.RowIndex] = "";
                }
            }
        }

        private void tabLabels_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //if (e.TabPage.Name == "tabNext90")
            //{
            //    MessageBox.Show("Tab page is under construction. Please contact" + Environment.NewLine + "the IT Department for updates.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    e.Cancel = true;
            //}
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("This process would remove all data labels in this tab. " + Environment.NewLine + "Are you sure you want to do this?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                //Initialize arrays
                for (int i = 0; i < dgvCol1.Rows.Count; i++)
                {
                    arrLabels[0, i] = "";
                }
                for (int i = 0; i < dgvCol2.Rows.Count; i++)
                {
                    arrLabels[1, i] = "";
                }
                for (int i = 0; i < dgvCol3.Rows.Count; i++)
                {
                    arrLabels[2, i] = "";
                }

                //Load array values to datagridviews
                //First 90
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol1.Rows[i].Cells[1].Value = arrLabels[0, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol2.Rows[i].Cells[1].Value = arrLabels[1, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol3.Rows[i].Cells[1].Value = arrLabels[2, i];
                    }
                    catch { }
                }

                //Next 90 Initialize arrays 
                for (int i = 0; i < dgvCol4.Rows.Count; i++)
                {
                    arrLabels[0, i] = "";
                }
                for (int i = 0; i < dgvCol5.Rows.Count; i++)
                {
                    arrLabels[1, i] = "";
                }
                for (int i = 0; i < dgvCol6.Rows.Count; i++)
                {
                    arrLabels[2, i] = "";
                }

                //Load array values to datagridviews
                //Next 90
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol4.Rows[i].Cells[1].Value = arrLabels[0, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol5.Rows[i].Cells[1].Value = arrLabels[1, i];
                    }
                    catch { }
                }
                for (int i = 0; i < 15; i++)
                {
                    try
                    {
                        dgvCol6.Rows[i].Cells[1].Value = arrLabels[2, i];
                    }
                    catch { }
                }
            }
        }

        private void tabLabels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabLabels.SelectedIndex == 0)
                btnBatch_Click(null, null);
            else
            {
                btnNext90.Text = "&Previous";
                btnNext90_Click(null, null);
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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; //tsbDelete.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
        }

        private void TestDataLabels_KeyDown(object sender, KeyEventArgs e)
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

        private void dgvCol1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvCol1.IsCurrentCellDirty)
            {
                dgvCol1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvCol2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvCol2.IsCurrentCellDirty)
            {
                dgvCol2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvCol3_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvCol3.IsCurrentCellDirty)
            {
                dgvCol3.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
