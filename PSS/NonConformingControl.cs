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
    public partial class NonConformingControl : PSS.TemplateForm
    {
        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";

        DataTable dtChangedBy = new DataTable();                                           // MY 12/22/2014 - Pop-up GridView ChangedBy query
        DataTable dtReviewedBy = new DataTable();                                          // MY 12/22/2014 - Pop-up GridView ReviewedBy query
        DataTable dtMaster = new DataTable();                                              // MY 06/26/2015 - datatable for NonConform Master
        DataTable dtActions = new DataTable();                                             // MY 06/26/2015 - datatable for Action List
        DataTable dtSponsor = new DataTable();                                             // MY 06/26/2015 - datatable for Sponsor

        public NonConformingControl()
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
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            txtChangedBy.GotFocus += new EventHandler(txtChangedByEnterHandler);
            txtReviewedBy.GotFocus += new EventHandler(txtReviewedByEnterHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.QA.NonConformMaster();

            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;

            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            //FileAccess();
        }

        private void FileAccess()
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "CNCT");

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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = false;
            }
        }

        private void LoadMaster(String cIndexNo)
        {
            try
            {
                dtMaster = PSSClass.QA.NonConformMain(cIndexNo);
                bsMaster.DataSource = dtMaster;              
                BindMaster();
            }
            catch { }
        }

        private void BindMaster()
        {
            // Clear bindings first
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }

            txtIndexNo.DataBindings.Add("Text", bsMaster, "IndexNo");
            cboGBLNo.DataBindings.Add("Text", bsMaster, "PSSNo");
            cboServiceCode.DataBindings.Add("Text", bsMaster, "ServiceCode");
            txtBookNo.DataBindings.Add("Text", bsMaster, "BookNo");
            txtDeptResponsible.DataBindings.Add("Text", bsMaster, "DeptResponsible");
            txtSponsorID.DataBindings.Add("Text", bsMaster, "SponsorID");
            txtSponsorName.DataBindings.Add("Text", bsMaster, "SponsorName");
            chkIsReport.DataBindings.Add("Checked", bsMaster, "IsReport");
            txtReportNo.DataBindings.Add("Text", bsMaster, "ReportNo");
            chkIsRawData.DataBindings.Add("Checked", bsMaster, "IsRawData");
            txtRawDataNo.DataBindings.Add("Text", bsMaster, "RawDataPageNo");
            txtControlPageNos.DataBindings.Add("Text", bsMaster, "ControlPgNos");
            chkIsSampleChanges.DataBindings.Add("Checked", bsMaster, "IsSampleChanges");
            txtSampleChanges.DataBindings.Add("Text", bsMaster, "SampleChanges");
            chkIsRequestChanges.DataBindings.Add("Checked", bsMaster, "IsRequestChanges");
            txtRequestChanges.DataBindings.Add("Text", bsMaster, "RequestChanges");
            chkIsGBLError.DataBindings.Add("Checked", bsMaster, "IsPSSError");
            txtGBLError.DataBindings.Add("Text", bsMaster, "PSSErrorDesc");
            chkIsQAError.DataBindings.Add("Checked", bsMaster, "IsQAError");
            chkIsLabError.DataBindings.Add("Checked", bsMaster, "IsLabError");
            chkIsLoginError.DataBindings.Add("Checked", bsMaster, "IsLoginError");
            txtErrorData.DataBindings.Add("Text", bsMaster, "ErrorData");
            chkIsLetterRequired.DataBindings.Add("Checked", bsMaster, "IsLetterRequired");
            txtLetterNo.DataBindings.Add("Text", bsMaster, "LetterNo");
            txtNotes.DataBindings.Add("Text", bsMaster, "Notes");
            txtChangedBy.DataBindings.Add("Text", bsMaster, "ChangedBy");
            txtChangedByName.DataBindings.Add("Text", bsMaster, "ChangedByName");
            txtChangeDate.DataBindings.Add("Text", bsMaster, "ChangeDate");
            txtReviewedBy.DataBindings.Add("Text", bsMaster, "ReviewedBy");
            txtReviewedByName.DataBindings.Add("Text", bsMaster, "ReviewedByName");
            txtReviewDate.DataBindings.Add("Text", bsMaster, "ReviewDate");           
        }

        private void BindActions()
        {
            foreach (Control c in pnlAction.Controls)
            {
                c.DataBindings.Clear();
            }

            tblALIndexDetailID.DataBindings.Add("Text", bsActions, "IndexNo");
            txtALGBLNo.DataBindings.Add("Text", bsActions, "PSSNo");
            txtALServiceCode.DataBindings.Add("Text", bsActions, "ServiceCode");

            txtIncorrect.DataBindings.Add("Text", bsActions, "Incorrect");
            txtCorrection.DataBindings.Add("Text", bsActions, "Correction");
            txtPage.DataBindings.Add("Text", bsActions, "PageNo");
            txtAction.DataBindings.Add("Text", bsActions, "CorrectiveAction");           
        }

        private void CreateMasterStructure()
        {
            // Create Master Data table for Add/Edit/Delete functions            
            dtMaster.Columns.Add("IndexNo", typeof(string));
            dtMaster.Columns.Add("PSSNo", typeof(Int64));
            dtMaster.Columns.Add("ServiceCode", typeof(Int16));
            dtMaster.Columns.Add("BookNo", typeof(Int16));
            dtMaster.Columns.Add("DeptResponsible", typeof(string));
            dtMaster.Columns.Add("SponsorID", typeof(Int16));
            dtMaster.Columns.Add("SponsorName", typeof(string));
            dtMaster.Columns.Add("IsReport", typeof(bool));
            dtMaster.Columns.Add("ReportNo", typeof(Int64));
            dtMaster.Columns.Add("IsRawData", typeof(bool));
            dtMaster.Columns.Add("RawDataPageNo", typeof(string));
            dtMaster.Columns.Add("ControlPgNos", typeof(string));
            dtMaster.Columns.Add("IsSampleChanges", typeof(bool));
            dtMaster.Columns.Add("SampleChanges", typeof(string));
            dtMaster.Columns.Add("IsRequestChanges", typeof(bool));
            dtMaster.Columns.Add("RequestChanges", typeof(string));
            dtMaster.Columns.Add("IsPSSError", typeof(bool));
            dtMaster.Columns.Add("PSSErrorDesc", typeof(string));
            dtMaster.Columns.Add("IsQAError", typeof(bool));
            dtMaster.Columns.Add("IsLabError", typeof(bool));
            dtMaster.Columns.Add("IsLoginError", typeof(bool));
            dtMaster.Columns.Add("ErrorData", typeof(string));
            dtMaster.Columns.Add("IsLetterRequired", typeof(bool));
            dtMaster.Columns.Add("LetterNo", typeof(Int16));
            dtMaster.Columns.Add("Notes", typeof(string));
            dtMaster.Columns.Add("ChangedBy", typeof(Int16));
            dtMaster.Columns.Add("ChangedByName", typeof(string));
            dtMaster.Columns.Add("ChangeDate", typeof(DateTime));
            dtMaster.Columns.Add("ReviewedBy", typeof(Int16));
            dtMaster.Columns.Add("ReviewedByName", typeof(string));
            dtMaster.Columns.Add("ReviewDate", typeof(DateTime));  
            dtMaster.Columns.Add("DateCreated", typeof(DateTime));
            dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            dtMaster.Columns.Add("LastUserID", typeof(Int16));
            bsMaster.DataSource = dtMaster;
        }

        private void CreateActionStructure()
        {
            // Create Action Data table for Add/Edit/Delete functions        
            bsActions.DataSource = dtActions;
            dgvActionList.DataSource = bsActions;
            dtActions.Columns.Add("IndexNo", typeof(string));
            dtActions.Columns.Add("PSSNo", typeof(Int64));
            dtActions.Columns.Add("ServiceCode", typeof(Int16));
            dtActions.Columns.Add("Incorrect", typeof(string));
            dtActions.Columns.Add("Correction", typeof(string));
            dtActions.Columns.Add("PageNo", typeof(string));
            dtActions.Columns.Add("CorrectiveAction", typeof(string));
            DataGridActionsSetting();
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
            items[0].Name = "ControlOfNonConformingTesting";
            items[0].Text = "Control of Non-Conforming Testing Sheet";
            items[0].Click += new EventHandler(PrintNCTestingSheetClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.QA.NonConformMaster();

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
        
        private void PrintNCTestingSheetClickHandler(object sender, EventArgs e)
        {
            NonConformingTestingSheet rpt = new NonConformingTestingSheet();

            txtIndexNo.Text = dgvFile.CurrentRow.Cells["IndexNo"].Value.ToString();
           
            rpt.IndexNo = txtIndexNo.Text.Trim();
           
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
                    bsFile.Filter = "PSSNo<>0";
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
            bsFile.Filter = "PSSNo<>0";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {
            nMode = 0;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            btnClose.Visible = true; btnClose.BringToFront();                      

            btnAddAction.Enabled = false;
            btnDeleteAction.Enabled = false;
            btnOKAction.Enabled = false;
            
            dgvFile.Rows[0].Selected = true;
            txtIndexNo.Text = dgvFile.CurrentRow.Cells["IndexNo"].Value.ToString();
            txtSponsorID.Text = dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString();

            LoadMaster(txtIndexNo.Text.Trim());
            LoadActions();           
            AddEditMode(false);
            OpenControls(pnlRecord, false);
            OpenControls(pnlAction, false);    
        }

        private void LoadActions()
        {
            dtActions = null;
            dtActions = PSSClass.QA.NCExtractActions(txtIndexNo.Text.Trim());
            if (dtActions == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            bsActions.DataSource = dtActions;
            bnActions.BindingSource = bsActions;
            dgvActionList.DataSource = bsActions;

            DataGridActionsSetting();
            BindActions();
        }

        private void LoadGBLList()
        {
            cboGBLNo.Text = "";
            cboGBLNo.DataSource = null;
            DataTable dt = new DataTable();
            dt = PSSClass.QA.NCPSSList();
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            cboGBLNo.DataSource = dt;
            cboGBLNo.DisplayMember = "PSSNo";
            cboGBLNo.ValueMember = "PSSNo";
        }

        private void LoadSCList(Int64 cGBLNo)
        {
            cboServiceCode.Text = "";
            cboServiceCode.DataSource = null;
            DataTable dt = new DataTable();
            dt = PSSClass.QA.NCSCList(cGBLNo);
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            cboServiceCode.DataSource = dt;
            cboServiceCode.DisplayMember = "ServiceCode";
            cboServiceCode.ValueMember = "ServiceCode";            
        }
      
        //private void LoadActionList()
        //{
        //    dtActions = null;
        //    dtActions = PSSClass.QA.NCExtractActions(txtIndexNo.Text.Trim());
        //    if (dtActions == null)
        //    {
        //        MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
        //        return;
        //    }

        //    bsActions.DataSource = dtActions;
        //    bnActions.BindingSource = bsActions;
        //    dgvActionList.DataSource = bsActions;

        //    DataGridActionsSetting();
        //    BindActions();
        //}

        private string PurgeString(string cStr)
        {
            string strX = "";

            if (cStr.Trim() != "")
            {
                strX = cStr.Replace("&", "&amp;");
                strX = strX.Replace(">", "&gt;");
                strX = strX.Replace("<", "&lt;");
                strX = strX.Replace("'", "&apos;");
                strX = strX.Replace("\"", "&quot;");
            }

            return strX;
        }

        private void LoadReportNo()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.QA.NCReportNo(Convert.ToInt64(cboGBLNo.Text), Convert.ToInt16(cboServiceCode.Text));
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            txtReportNo.Text = dt.Rows[0]["ReportNo"].ToString();

            txtReportNo.Enabled = false;
        }

        private void LoadRawDataRef()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.QA.NCRawDataRef(Convert.ToInt64(cboGBLNo.Text), Convert.ToInt16(cboServiceCode.Text));
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            txtRawDataNo.Text = dt.Rows[0]["RawDataRef"].ToString();

            txtRawDataNo.Enabled = false;
        }

        private void LoadGBLInfo()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.QA.NCPSSInfo(Convert.ToInt64(cboGBLNo.Text), Convert.ToInt16(cboServiceCode.Text));
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            txtBookNo.Text = dt.Rows[0]["BookNo"].ToString();
            txtSponsorID.Text = dt.Rows[0]["SponsorID"].ToString();
            txtSponsorName.Text = dt.Rows[0]["SponsorName"].ToString(); 
            
            txtBookNo.Enabled = false;
            
        }
        
        private void LoadControlPageRange()
        {           
            DataTable dt = new DataTable();
            dt = PSSClass.QA.NCControlPageRange(Convert.ToInt64(cboGBLNo.Text), Convert.ToInt16(cboServiceCode.Text));
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
           
            if (dt.Rows[0]["EndPageNo"].ToString() == "")
            {
                txtControlPageNos.Text = dt.Rows[0]["StartPageNo"].ToString();
            }
            else
            {
                txtControlPageNos.Text = dt.Rows[0]["StartPageNo"].ToString() + " to " + dt.Rows[0]["EndPageNo"].ToString();
            }
            txtControlPageNos.Enabled = false;           
        }

        private void LoadChangers()
        {
            dgvChangedBy.DataSource = null;

            dtChangedBy = PSSClass.QA.NCChangers();

            if (dtChangedBy == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvChangedBy.DataSource = dtChangedBy;
            StandardDGVSetting(dgvChangedBy);
            dgvChangedBy.Columns[0].Width = 300;
            dgvChangedBy.Columns[1].Visible = false;
        }

        private void LoadReviewers()
        {
            dgvReviewedBy.DataSource = null;

            dtReviewedBy = PSSClass.QA.NCReviewers();

            if (dtReviewedBy == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvReviewedBy.DataSource = dtReviewedBy;
            StandardDGVSetting(dgvReviewedBy);
            dgvReviewedBy.Columns[0].Width = 300;
            dgvReviewedBy.Columns[1].Visible = false;
        }

        private void LoadSponsor()
        {
            dtSponsor = PSSClass.QA.NCSponsor(Convert.ToInt64(cboGBLNo.Text));

            if (dtSponsor == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            txtSponsorID.Text = dtSponsor.Rows[0]["SponsorID"].ToString();
            txtSponsorName.Text = dtSponsor.Rows[0]["SponsorName"].ToString();            
        }

        private void ClearFields()
        {
            // Clear all except IndexNo, GBLNo and Service Code Fields
            txtBookNo.Text = "";
            txtDeptResponsible.Text = "";
            txtSponsorID.Text = "";
            txtSponsorName.Text = "";            
            chkIsReport.Checked = false;
            txtReportNo.Text = "";
            chkIsReport.Checked = false;
            txtRawDataNo.Text = "";
            txtControlPageNos.Text = "";
            chkIsSampleChanges.Checked = false;
            txtSampleChanges.Text = "";
            chkIsRequestChanges.Checked = false;
            txtRequestChanges.Text = "";
            chkIsGBLError.Checked = false;
            txtGBLError.Text = "";
            chkIsQAError.Checked = false;
            chkIsLabError.Checked = false;
            chkIsLoginError.Checked = false;
            chkIsLetterRequired.Checked = false;
            txtLetterNo.Text = "";
            txtNotes.Text = "";
            txtChangedBy.Text = "";
            txtChangedByName.Text = "";
            txtChangeDate.Text = "";
            txtReviewedBy.Text = "";
            txtReviewedByName.Text = "";
            txtReviewDate.Text = "";
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
            dgvFile.Columns["IndexNo"].HeaderText = "Index No";
            dgvFile.Columns["PSSNo"].HeaderText = "Order No";
            dgvFile.Columns["ServiceCode"].HeaderText = "Service Code";
            dgvFile.Columns["BookNo"].HeaderText = "Book No";            
            dgvFile.Columns["DeptResponsible"].HeaderText = "Dept Responsible";
            dgvFile.Columns["SponsorID"].HeaderText = "Sponsor ID";
            dgvFile.Columns["SponsorName"].HeaderText = "Sponsor Name";
            dgvFile.Columns["SponsorID"].HeaderText = "Sponsor ID";
            dgvFile.Columns["SponsorName"].HeaderText = "Sponsor Name";
            dgvFile.Columns["IsReport"].HeaderText = "Is Report";
            dgvFile.Columns["ReportNo"].HeaderText = "Report No";
            dgvFile.Columns["IsReport"].HeaderText = "Is Report";
            dgvFile.Columns["IsRawData"].HeaderText = "Is Raw Data";
            dgvFile.Columns["RawDataPageNo"].HeaderText = "Raw Data Page No";
            dgvFile.Columns["ControlPgNos"].HeaderText = "Control Page Numbers";
            dgvFile.Columns["IsSampleChanges"].HeaderText = "Is Sample Changes";
            dgvFile.Columns["SampleChanges"].HeaderText = "Sample Changes";
            dgvFile.Columns["IsRequestChanges"].HeaderText = "Is Request Changes";
            dgvFile.Columns["RequestChanges"].HeaderText = "Request Changes";
            dgvFile.Columns["IsPSSError"].HeaderText = "Is PSS Error";
            dgvFile.Columns["PSSErrorDesc"].HeaderText = "PSS Error Desc";
            dgvFile.Columns["IsQAError"].HeaderText = "Is QA Error";
            dgvFile.Columns["IsLabError"].HeaderText = "Is Lab Error";
            dgvFile.Columns["IsLoginError"].HeaderText = "Is Login Error";
            dgvFile.Columns["IsLetterRequired"].HeaderText = "Is Letter Required";
            dgvFile.Columns["LetterNo"].HeaderText = "Letter No";
            dgvFile.Columns["Notes"].HeaderText = "Notes";
            dgvFile.Columns["ChangedBy"].HeaderText = "ChangedBy ID";
            dgvFile.Columns["ChangedByName"].HeaderText = "Changed By";
            dgvFile.Columns["ChangeDate"].HeaderText = "Change Date";
            dgvFile.Columns["ReviewedBy"].HeaderText = "ReviewedBy ID";
            dgvFile.Columns["ReviewedByName"].HeaderText = "Reviewed By";
            dgvFile.Columns["ReviewDate"].HeaderText = "Review Date";
            dgvFile.Columns["DateCreated"].HeaderText = "Date Created";
            dgvFile.Columns["PSSNo"].Width = 100;
            dgvFile.Columns["ServiceCode"].Width = 100;
            dgvFile.Columns["BookNo"].Width = 100;
            dgvFile.Columns["DeptResponsible"].Width = 200;
            dgvFile.Columns["SponsorID"].Width = 100;
            dgvFile.Columns["SponsorName"].Width = 300;
            dgvFile.Columns["ControlPgNos"].Width = 150;
            dgvFile.Columns["SampleChanges"].Width = 300;
            dgvFile.Columns["RequestChanges"].Width = 300;
            dgvFile.Columns["PSSErrorDesc"].Width = 300;
            dgvFile.Columns["Notes"].Width = 300;
            dgvFile.Columns["ChangeDate"].Width = 80;
            dgvFile.Columns["ReviewDate"].Width = 80;
            dgvFile.Columns["DateCreated"].Width = 80;
            dgvFile.Columns["ChangedByName"].Width = 300;
            dgvFile.Columns["ReviewedByName"].Width = 300;
            dgvFile.Columns["ChangeDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["ReviewDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["PSSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["BookNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorID"].Visible = false;
            dgvFile.Columns["SampleChanges"].Visible = false;
            dgvFile.Columns["RequestChanges"].Visible = false;
            dgvFile.Columns["PSSErrorDesc"].Visible = false;
            dgvFile.Columns["IsReport"].Visible = false;
            dgvFile.Columns["IsRawData"].Visible = false;
            dgvFile.Columns["IsSampleChanges"].Visible = false;
            dgvFile.Columns["IsRequestChanges"].Visible = false;
            dgvFile.Columns["IsPSSError"].Visible = false;
            dgvFile.Columns["IsQAError"].Visible = false;
            dgvFile.Columns["IsLabError"].Visible = false;
            dgvFile.Columns["IsLoginError"].Visible = false;
            dgvFile.Columns["IsLetterRequired"].Visible = false;
            dgvFile.Columns["Notes"].Visible = false;
            dgvFile.Columns["ChangedByName"].Visible = false;
            dgvFile.Columns["ReviewedByName"].Visible = false;
            dgvFile.Columns["ChangedBy"].Visible = false;
            dgvFile.Columns["ReviewedBy"].Visible = false;
        }

        private void DataGridActionsSetting()
        {
            dgvActionList.EnableHeadersVisualStyles = false;
            dgvActionList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvActionList.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvActionList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvActionList.Columns["Incorrect"].HeaderText = "Incorrect";
            dgvActionList.Columns["Correction"].HeaderText = "Correction";
            dgvActionList.Columns["PageNo"].HeaderText = "Page No";
            dgvActionList.Columns["CorrectiveAction"].HeaderText = "Corrective Action";
            dgvActionList.Columns["Incorrect"].Width = 100;
            dgvActionList.Columns["Correction"].Width = 100;
            dgvActionList.Columns["PageNo"].Width = 70;
            dgvActionList.Columns["CorrectiveAction"].Width = 177;
            dgvActionList.Columns["IndexNo"].Visible = false;
            dgvActionList.Columns["PSSNo"].Visible = false;
            dgvActionList.Columns["ServiceCode"].Visible = false;
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
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            ClearControls(this.pnlAction);
            OpenControls(this.pnlRecord, true);
            dtMaster.Rows.Clear();
            dtActions.Rows.Clear();           
            cboGBLNo.Focus();
            txtSponsorName.ReadOnly = true;
            btnAddAction.Enabled = true;
            btnDeleteAction.Enabled = false;          

            // Create Master Data Row
            DataRow dR = dtMaster.NewRow();

            // Create Master Data table for Add/Edit/Delete functions  
            dR["IndexNo"] = DBNull.Value;
            dR["PSSNo"] = DBNull.Value;
            dR["ServiceCode"] = DBNull.Value;
            dR["BookNo"] = DBNull.Value;
            dR["DeptResponsible"] = "";
            dR["SponsorID"] = 0;
            dR["SponsorName"] = "";
            dR["IsReport"] = false;
            dR["ReportNo"] = DBNull.Value;
            dR["IsRawData"] = false;
            dR["RawDataPageNo"] = "";
            dR["ControlPgNos"] = "";
            dR["IsSampleChanges"] = false;
            dR["SampleChanges"] = "";
            dR["IsRequestChanges"] = false;
            dR["RequestChanges"] = "";
            dR["IsPSSError"] = false;
            dR["PSSErrorDesc"] = "";
            dR["IsQAError"] = false;
            dR["IsLabError"] = false;
            dR["IsLoginError"] = false;
            dR["ErrorData"] = "";
            dR["IsLetterRequired"] = false;
            dR["LetterNo"] = 0;
            dR["ChangedBy"] = DBNull.Value;
            dR["ChangedByName"] = "";
            dR["ChangeDate"] = DBNull.Value;
            dR["ReviewedBy"] = DBNull.Value;
            dR["ReviewedByName"] = "";
            dR["ReviewDate"] = DBNull.Value;
            dR["DateCreated"] = DateTime.Now;
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;       
          
            dtMaster.Rows.Add(dR);
            bsMaster.DataSource = dtMaster;
            BindMaster();
            txtIndexNo.Text = "(New)";   
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;

            LoadData();
            nMode = 2;
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();

            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlAction, true);
            cboGBLNo.Focus(); btnClose.Visible = false;
            
            btnAddAction.Enabled = true;
            btnDeleteAction.Enabled = true;
            
            tsbAdd.Enabled = false;
            tsbEdit.Enabled = false;
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
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

                sqlcmd.Parameters.AddWithValue("@IndexNo", txtIndexNo.Text.Trim());
                
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelNonConform";

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
            // Header Save routine
            bsMaster.EndEdit();

            // Validate if changes were made on the Master
            DataTable dt = dtMaster.GetChanges();
            if (dt != null)
            {
                int nRet = ValidateMaster();                                                      // Validation for Master Record
                if (nRet == 0)
                {
                    dt.Dispose();
                    return;
                }

                SaveMaster();                                                                    // Save Master Record
                dt.Dispose();
            }
            dt = null;
          
            // Reload Saved Master
            LoadRecords();

            bsFile.Filter = "IndexNo<>''";
            if (txtIndexNo.Text != "(New)")
                PSSClass.General.FindRecord("IndexNo", txtIndexNo.Text, bsFile, dgvFile);

            // Detail Save Routine     
            bsActions.EndEdit();       
            dt = dtActions.GetChanges();
            if (dt != null)
            {
                int nRet = ValidateActions();                                                    // Validation for Action Record
                if (nRet == 0)
                {
                    dt.Dispose();
                    return;
                }
                CreateActionXML();                                                               // Save Action Record
                dt.Dispose();
            }
            dt = null;   
            dgvFile.Refresh();

            btnClose.Visible = true;
            btnAddAction.Enabled = false;
            btnAddAction.Enabled = false;

            OpenControls(pnlRecord, false);           
            AddEditMode(false);
            LoadData();
            MessageBox.Show("Record successfully saved!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);           
        }

        private void SaveMaster()
        {            
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);           
            sqlcmd.Parameters.AddWithValue("@PSSNo", Convert.ToInt64(cboGBLNo.Text));
            sqlcmd.Parameters.AddWithValue("@ServiceCode", Convert.ToInt16(cboServiceCode.Text));
            sqlcmd.Parameters.AddWithValue("@BookNo", Convert.ToInt16(txtBookNo.Text));
            sqlcmd.Parameters.AddWithValue("@IndexNo", txtIndexNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@DeptResponsible", txtDeptResponsible.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@SponsorID", Convert.ToInt16(txtSponsorID.Text));

            sqlcmd.Parameters.AddWithValue("@IsReport", Convert.ToBoolean(chkIsReport.CheckState));
            if (chkIsReport.Checked)
            {
                if (Convert.ToInt64(txtReportNo.Text) == 0)
                    sqlcmd.Parameters.AddWithValue("@ReportNo", 0);
                else
                    sqlcmd.Parameters.AddWithValue("@ReportNo", Convert.ToInt64(txtReportNo.Text));
            }
           
            sqlcmd.Parameters.AddWithValue("@IsRawData", Convert.ToBoolean(chkIsRawData.CheckState));
            if (chkIsRawData.Checked)
            {
                sqlcmd.Parameters.AddWithValue("@RawDataPageNo", txtRawDataNo.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@ControlPgNos", txtControlPageNos.Text.Trim());
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@RawDataPageNo", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@ControlPgNos", DBNull.Value);
            }
            sqlcmd.Parameters.AddWithValue("@IsSampleChanges", Convert.ToBoolean(chkIsSampleChanges.CheckState));
            sqlcmd.Parameters.AddWithValue("@SampleChanges", txtSampleChanges.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@IsRequestChanges", Convert.ToBoolean(chkIsRequestChanges.CheckState));
            sqlcmd.Parameters.AddWithValue("@RequestChanges", txtRequestChanges.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@IsPSSError", Convert.ToBoolean(chkIsGBLError.CheckState));
            sqlcmd.Parameters.AddWithValue("@PSSErrorDesc", txtGBLError.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@IsQAError", Convert.ToBoolean(chkIsQAError.CheckState));
            sqlcmd.Parameters.AddWithValue("@IsLabError", Convert.ToBoolean(chkIsLabError.CheckState));
            sqlcmd.Parameters.AddWithValue("@IsLoginError", Convert.ToBoolean(chkIsLoginError.CheckState));
            sqlcmd.Parameters.AddWithValue("@IsLetterRequired", Convert.ToBoolean(chkIsLetterRequired.CheckState));
            if (txtLetterNo.Text == "" || Convert.ToInt16(txtLetterNo.Text) == 0)
            {
                sqlcmd.Parameters.AddWithValue("@LetterNo", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@LetterNo", txtLetterNo.Text);
            }
            sqlcmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@ChangedBy", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@ChangeDate", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@ReviewedBy", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@ReviewDate", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditNonConform";

            SqlParameter output = new SqlParameter("@NewIndexNo", SqlDbType.NChar, 9);
            output.Direction = ParameterDirection.Output;
            sqlcmd.Parameters.Add(output);

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
            if (nMode == 1)
                txtIndexNo.Text = output.Value.ToString();
        }

        private void CreateActionXML()
        {
            bsActions.EndEdit();

            string strXML = "<ErrorDetails>";         

            if (dgvActionList.Rows.Count != 0)
            {
                for (int i = 0; i < dgvActionList.Rows.Count; i++)
                {
                    if (dgvActionList.Rows[i].Cells[1].Value != null)
                    {    
                        strXML = strXML + "<Error><IndexNo>"   + txtIndexNo.Text.Trim()                                       + "</IndexNo>" +
                                          "<PSSNo>"            + cboGBLNo.Text.Trim()                                         + "</PSSNo>" +
                                          "<ServiceCode>"      + cboServiceCode.Text.Trim()                                   + "</ServiceCode>" +
                                          "<Incorrect>"        + PurgeString(dgvActionList.Rows[i].Cells[3].Value.ToString()) + "</Incorrect>" +
                                          "<Correction>"       + PurgeString(dgvActionList.Rows[i].Cells[4].Value.ToString()) + "</Correction>" +
                                          "<PageNo>"           + PurgeString(dgvActionList.Rows[i].Cells[5].Value.ToString()) + "</PageNo>" +
                                          "<CorrectiveAction>" + PurgeString(dgvActionList.Rows[i].Cells[6].Value.ToString()) + "</CorrectiveAction></Error>";
                    }
                }
            }
            strXML = strXML + "</ErrorDetails>";
            SaveActionXML(strXML);
        }

        private void SaveActionXML(String cStrXML)
        {           
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@IndexNo", txtIndexNo.Text.Trim());            
            sqlcmd.Parameters.AddWithValue("@XMLData", cStrXML);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdNonConformErrorXML";
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

        private void CleanUpActionFields()
        {           
            txtALGBLNo.Text = "";
            txtALServiceCode.Text = "";
            txtCorrection.Text = "";
            txtPage.Text = "";
            txtAction.Text = "";
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
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            nMode = 0;
        }

        private void NonConformingControl_Load(object sender, EventArgs e)
        {
            LoadRecords();
            LoadGBLList();
            //LoadSCList(Convert.ToInt64(cboGBLNo.Text));
            BuildPrintItems();
            BuildSearchItems();

            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            CreateMasterStructure();
            CreateActionStructure(); 
        }

        private void NonConformingControl_KeyDown(object sender, KeyEventArgs e)
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
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
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

        private void txtChangedByEnterHandler(object sender, EventArgs e)
        {
            dgvChangedBy.Visible = false;
        }

        private void txtReviewedByEnterHandler(object sender, EventArgs e)
        {
            dgvChangedBy.Visible = false;
        }

        // MY 12/22/2014 - START: txt/dgvChangedBy events
        private void dgvChangedBy_DoubleClick(object sender, EventArgs e)
        {
            txtChangedByName.Text = dgvChangedBy.CurrentRow.Cells[0].Value.ToString();
            txtChangedBy.Text = dgvChangedBy.CurrentRow.Cells[1].Value.ToString();
            dgvChangedBy.Visible = false;
        }

        private void dgvChangedBy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvChangedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtChangedByName.Text = dgvChangedBy.CurrentRow.Cells[0].Value.ToString();
                txtChangedBy.Text = dgvChangedBy.CurrentRow.Cells[1].Value.ToString();
                dgvChangedBy.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvChangedBy.Visible = false;
            }
        }
        private void txtChangedByName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvChangedBy.Visible = true; dgvChangedBy.BringToFront();
            }
        }

        private void dgvChangedBy_Leave(object sender, EventArgs e)
        {
            dgvChangedBy.Visible = false;
        }

        private void txtChangedByName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwChangedBy;
                dvwChangedBy = new DataView(dtChangedBy, "ChangedByName like '%" + txtChangedByName.Text.Trim().Replace("'", "''") + "%'", "ChangedByName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvChangedBy, dvwChangedBy);
            }
        }

        private void dgvChangedBy_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtChangedByName.Text = dgvChangedBy.CurrentRow.Cells[0].Value.ToString();
            txtChangedBy.Text = dgvChangedBy.CurrentRow.Cells[1].Value.ToString();
            dgvChangedBy.Visible = false;
        }

        private void picChangedBy_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadChangers();
                dgvChangedBy.Visible = true; dgvChangedBy.BringToFront(); 
            }
        }

        private void txtChangedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtChangedByName.Text = PSSClass.QA.NCChangedByName(Convert.ToInt16(txtChangedBy.Text));
                if (txtChangedByName.Text.Trim() == "")
                {
                    MessageBox.Show("No matching ChangedBy ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvChangedBy.Visible = false;
            }
            else
            {
                txtChangedByName.Text = ""; dgvChangedBy.Visible = false;
            }
        }
        // MY 12/22/2014 - END: txt/dgvChangedBy events       

        // MY 12/22/2014 - START: txt/dgvReviewedBy events
        private void dgvReviewedBy_DoubleClick(object sender, EventArgs e)
        {
            txtReviewedByName.Text = dgvReviewedBy.CurrentRow.Cells[0].Value.ToString();
            txtReviewedBy.Text = dgvReviewedBy.CurrentRow.Cells[1].Value.ToString();
            dgvReviewedBy.Visible = false;
        }

        private void dgvReviewedBy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvReviewedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtReviewedByName.Text = dgvReviewedBy.CurrentRow.Cells[0].Value.ToString();
                txtReviewedBy.Text = dgvReviewedBy.CurrentRow.Cells[1].Value.ToString();
                dgvReviewedBy.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvReviewedBy.Visible = false;
            }
        }
        private void txtReviewedByName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvReviewedBy.Visible = true; dgvReviewedBy.BringToFront();
            }
        }

        private void dgvReviewedBy_Leave(object sender, EventArgs e)
        {
            dgvReviewedBy.Visible = false;
        }

        private void txtReviewedByName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwReviewedBy;
                dvwReviewedBy = new DataView(dtReviewedBy, "ReviewedByName like '%" + txtReviewedByName.Text.Trim().Replace("'", "''") + "%'", "ReviewedByName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvChangedBy, dvwReviewedBy);
            }
        }

        private void dgvReviewedBy_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtReviewedByName.Text = dgvReviewedBy.CurrentRow.Cells[0].Value.ToString();
            txtReviewedBy.Text = dgvReviewedBy.CurrentRow.Cells[1].Value.ToString();
            dgvReviewedBy.Visible = false;
        }

        private void picReviewedBy_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadReviewers();
                dgvReviewedBy.Visible = true; dgvReviewedBy.BringToFront();
            }
        }

        private void txtReviewedBy_KeyPress(object sender, KeyPressEventArgs e)
        {           
            if (e.KeyChar == 13)
            {
                txtReviewedByName.Text = PSSClass.QA.NCReviewedByName(Convert.ToInt16(txtReviewedBy.Text));
                if (txtReviewedByName.Text.Trim() == "")
                {
                    MessageBox.Show("No matching ReviewedBy ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvReviewedBy.Visible = false;
            }
            else
            {
                txtReviewedByName.Text = ""; dgvReviewedBy.Visible = false;
            }
        }

        // MY 12/22/2014 - END: txt/dgvReviewedBy events   

        private void txtLetterNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void txtReportNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void cboGBLNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboGBLNo.Text.Trim() != "")
                {
                    if (nMode != 0)
                    {
                        try
                        {
                            ClearFields();                             
                            LoadSCList(Convert.ToInt64(cboGBLNo.Text));
                            LoadSponsor();    
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }
        }

        private void cboGBLNo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboGBLNo.Text.Trim() != "")
                {
                    if (nMode != 0)
                    {
                        ClearFields();                                     
                        LoadSCList(Convert.ToInt64(cboGBLNo.Text));
                        LoadSponsor();    
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void cboServiceCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboGBLNo.Text.Trim() != "" && cboServiceCode.Text.Trim() != "")
                {
                    if (nMode != 0)
                    {
                        try
                        {
                            ClearFields();                                            
                            LoadGBLInfo();
                            LoadControlPageRange();
                        }
                        catch {}
                    }
                }
            }
        }

        // MY 03/05/2015 - Start: Change Date events  
        private void txtChangeDate_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                mcChangeDate.BringToFront();
                mcChangeDate.Visible = true;
                mcChangeDate.Focus();
            }
        }

        private void txtChangeDate_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                mcChangeDate.Visible = true;
            }
        }

        private void txtChangeDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (nMode != 0)
            {
                mcChangeDate.Visible = true;
            }
            e.SuppressKeyPress = true;
        }

        private void txtChangeDate_Leave(object sender, EventArgs e)
        {
            if (!mcChangeDate.Focused)
            {
                mcChangeDate.Visible = false;
            }
        }

        private void mcChangeDate_DateSelected(object sender, DateRangeEventArgs e)
        {
            var monthCalendar = sender as MonthCalendar;
            txtChangeDate.Text = monthCalendar.SelectionStart.ToString("MM/dd/yyyy");
            monthCalendar.Visible = false;
        }
        // MY 03/05/2015 - End: Change Date events  


        // MY 03/05/2015 - Start: Review Date events  
        private void txtReviewDate_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                mcReviewDate.BringToFront();
                mcReviewDate.Visible = true;
                mcReviewDate.Focus();
            }
        }

        private void txtReviewDate_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                mcReviewDate.Visible = true;
            }
        }

        private void txtReviewDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (nMode != 0)
            {
                mcReviewDate.Visible = true;
            }
            e.SuppressKeyPress = true;
        }

        private void txtReviewDate_Leave(object sender, EventArgs e)
        {
            if (!mcReviewDate.Focused)
            {
                mcReviewDate.Visible = false;
            }
        }

        private void mcReviewDate_DateSelected(object sender, DateRangeEventArgs e)
        {
            var monthCalendar = sender as MonthCalendar;
            txtReviewDate.Text = monthCalendar.SelectionStart.ToString("MM/dd/yyyy");
            monthCalendar.Visible = false;
        }

        private int ValidateMaster()
        {            
            if (nMode != 0)
            {
                if (txtBookNo.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter a Book Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtBookNo.Focus();
                    return 0;
                }
                if (cboGBLNo.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter a PSS Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboGBLNo.Focus();
                    return 0;
                }
                if (cboServiceCode.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter a ServiceCode Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboServiceCode.Focus();
                    return 0;
                }
                if (txtSponsorID.Text.Trim() == "")
                {
                    MessageBox.Show("Please choose a Sponsor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtSponsorID.Focus();
                    return 0;
                }
                if (!chkIsReport.Checked && !chkIsRawData.Checked)
                {
                    MessageBox.Show("Please check either Report or Raw Data Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    chkIsReport.Focus();
                    return 0;
                }
                if (txtReportNo.Text.Trim() == "" && txtRawDataNo.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter a Report or Raw Data Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtReportNo.Focus();
                    return 0;
                }
            }
            return 1;
        }

        private int ValidateActions()
        {
            if (nMode != 0)
            {
                if (txtIncorrect.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Incorrect issue!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtIncorrect.Focus();
                    return 0;
                }
                if (txtCorrection.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Correction!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCorrection.Focus();
                    return 0;
                }
                if (txtPage.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Page!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtPage.Focus();
                    return 0;
                }
                if (txtAction.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Corrective Action!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtAction.Focus();
                    return 0;
                }
            }
            return 1;
        }

        private void btnAddAction_Click(object sender, EventArgs e)
        {
            ClearControls(this.pnlAction);
            OpenControls(this.pnlAction, true);
            pnlAction.Visible = true;
            txtIncorrect.Focus();
            btnAddAction.Enabled = false;
            btnDeleteAction.Enabled = false;
            btnOKAction.Enabled = true;

            AddEditMode(true);
            tsbCancel.Enabled = true;

            foreach (Control c in pnlAction.Controls)
            {
                c.DataBindings.Clear();
            }            
        }

        private void btnOKAction_Click(object sender, EventArgs e)
        {
            int nRet = ValidateActions();                                                         // Validation for Action Record
            if (nRet == 0)
            {
                return;
            }

            DataRow dR = dtActions.NewRow();

            dR["IndexNo"] = txtIndexNo.Text.Trim();
            dR["PSSNo"] = cboGBLNo.Text;
            dR["ServiceCode"] = cboServiceCode.Text;
            dR["Incorrect"] = txtIncorrect.Text;
            dR["Correction"] = txtCorrection.Text;
            dR["PageNo"] = txtPage.Text;
            dR["CorrectiveAction"] = txtAction.Text;
           
            dtActions.Rows.Add(dR);
            bsActions.DataSource = dtActions;
            bnActions.BindingSource = bsActions;
            dgvActionList.DataSource = bsActions;
            BindActions();
            btnAddAction.Enabled = true;
            btnDeleteAction.Enabled = true;
            btnOKAction.Enabled = false;

            DataGridActionsSetting();           
        }

        private void btnDeleteAction_Click(object sender, EventArgs e)
        {
            int Row = dgvActionList.CurrentRow.Index;

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
            
            dgvActionList.Rows.RemoveAt(Row);

            if (dgvActionList.Rows.Count == 0)
            {
                btnDeleteAction.Enabled = false;
            }

            CreateActionXML();

            AddEditMode(false);
        }

        private void chkIsReport_CheckStateChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (chkIsReport.Checked)
                {
                    LoadReportNo();
                }
                else
                {
                    txtReportNo.Text = "";
                }
            }
        }

        private void chkIsRawData_CheckStateChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (chkIsRawData.Checked)
                {
                    LoadRawDataRef();
                    LoadControlPageRange();
                }
                else
                {
                    txtRawDataNo.Text = "";
                    txtControlPageNos.Text = "";
                }
            }
        }

        private void btnTestData_Click(object sender, EventArgs e)
        {           
       
            bsActions.EndEdit();

            dtActions.Rows[bsActions.Position]["Incorrect"].ToString();
            for (int i = 0; i < dtActions.Rows.Count; i++)
            {
                MessageBox.Show(dtActions.Rows[i]["Incorrect"].ToString());
                MessageBox.Show(dtActions.Rows[i].RowState.ToString());
            }
                  
        }

        private void dgvCellChangedHandler(object sender, EventArgs e)
        {
            FileAccess();           
        }

        private void NonConformingControl_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}