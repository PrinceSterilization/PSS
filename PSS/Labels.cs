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
using System.Drawing.Printing;
using System.Net.Mail;
using System.Net;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;
using System.Diagnostics;

namespace GIS
{
    public partial class Labels : GIS.TemplateForm
    {

        public Int64 intGBLNo;
        public int intSterClassID;
        public byte nLabelSw;

        private byte nMode = 0;
        private int nSw = 0;
        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
       
        private int nIndex;
        private string strFileAccess = "RO";
        private Int16 sterClassID;

        string strPrinterName;
        string strRptName;
        string strProcName;
        string strLabelParam;

        private int nFmtDay;
        private int nFmtMonth;
        private int nFmtYear;

        public Int16 nLabelTypeID = 1;                                                  // 1 = GBL Slash  2 = Ingredion  3 = Media  4 = Sterility  5 = Wrapped Goods

        private long nGBLNo;
        private string sSlashNo;
        private string sIngLabelDesc;
        private string sMedName;
        private string sLotNo;        
        private long nAutoclave;
        private DateTime sPrepDate;
        private DateTime sSterDate;
        private DateTime sExpDate;
        private int nSterClassID;
        private int nSterBtwID;
        private int nLabelCount;
        
        DataTable dtFieldList = new DataTable();                                        // MY 01/20/2016 - Pop-up GridView Field 1 query
        DataTable dtSterDesc = new DataTable();                                         // MY 01/20/2016 - Pop-up GridView Field 1 query
        DataTable dtField3List = new DataTable();                                       // MY 01/20/2016 - Pop-up GridView Field 3 query
        DataTable dtCompanies = new DataTable();                                        // MY 09/01/2015 - Pop-up GridView Company query
        DataTable dtSterility = new DataTable();                                        // MY 01/21/2016 - Pop-up GridView Sterility table       

        public Labels()
        {
            InitializeComponent();

         
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
            DataTable dt = GISClass.Tools.LabelMaster(nLabelTypeID);
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "GBLNo <> 0";
            
            DataGridSetting();
        }

        private void FileAccess()
        {
            strFileAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "Labels");

            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsbDelete.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {

                tsbDelete.Enabled = false;

                if (nLabelTypeID == 4)
                {
                    tsbAdd.Enabled = true; tsbEdit.Enabled = true; 
                }
                else
                {
                    tsbAdd.Enabled = false; tsbEdit.Enabled = false; 
                }
            }
            else if (strFileAccess == "FA")
            {
                tsbDelete.Enabled = false;

                if (nLabelTypeID == 4)
                {
                    tsbAdd.Enabled = true; tsbEdit.Enabled = true;
                }
                else
                {
                    tsbAdd.Enabled = false; tsbEdit.Enabled = false;
                }
            }
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
            //int i = 0;

            //DataTable dt = new DataTable();
            //dt = GISClass.CustomerService.DocumentMaster();
            //if (dt == null)
            //{
            //    MessageBox.Show("Connection problen encountered during build-up of search items." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    nMode = 9;
            //    return;
            //}
            //arrCol = new string[dt.Columns.Count];

            //ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length];

            //foreach (DataColumn colFile in dt.Columns)
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
            //    i += 1;
            //}
            //for (int j = 0; j < cklColumns.Items.Count; j++)
            //{
            //    cklColumns.SetItemChecked(j, true);
            //}

            //tsddbSearch.DropDownItems.AddRange(items);
            //tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            //tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
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
        private void PrintDeptNameClickHandler(object sender, EventArgs e)
        {
            //RptDept rptDeptNameList = new RptDept();
            //rptDeptNameList.WindowState = FormWindowState.Maximized;
            //rptDeptNameList.rptName = "DeptName";
            //rptDeptNameList.rptLabel = "DEPARTMENTS REFERENCE LIST SORTED BY NAME";
            //rptDeptNameList.Show();
        }

        private void PrintDeptCodeClickHandler(object sender, EventArgs e)
        {

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
                    bsFile.Filter = "GBLNo<>0";
                    GISClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
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

        private void ClearSterility()
        {
            foreach (Control c in pnlSterility.Controls)
            {
                c.DataBindings.Clear();
            }
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            ClearControls(this.pnlGBLSlash);
            ClearControls(this.pnlIngredion);
            ClearControls(this.pnlMedia);
            ClearControls(this.pnlWrappedGoods);
            ClearControls(this.pnlSterility);
            OpenControls(this.pnlRecord, false);
            ClearSterility();           

            if (nLabelTypeID != 4 )
            {
                pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;  
            }
            else
            {
                cboGBLNo3.Text = dgvFile.CurrentRow.Cells["GBLNo"].Value.ToString();
                txtSterClassID.Text = dgvFile.CurrentRow.Cells["SterClassID"].Value.ToString();
                txtReviewDate.Text = dgvFile.CurrentRow.Cells["ReviewDate"].Value.ToString();
                txtReviewer.Text = dgvFile.CurrentRow.Cells["Reviewer"].Value.ToString();
                txtApprovalDate.Text = dgvFile.CurrentRow.Cells["ApprovalDate"].Value.ToString();
                txtApprover.Text = dgvFile.CurrentRow.Cells["Approver"].Value.ToString();

                switch (Convert.ToInt16(txtSterClassID.Text))
                {
                    case 1: // GBL Routine 
                        rdoGBLRoutine.Checked = true;
                        break;
                    case 2: // Catalent                       
                        rdoCatalent.Checked = true;
                        break;
                    case 3: // Cytonet Kit                   
                        rdoCytonet.Checked = true;
                        break;
                    case 4: // Cytonet Media Fill Kit        
                        rdoCytonetMedia.Checked = true;
                        break;
                    default:
                        break;
                }
                pnlRecord.Visible = false;  dgvFile.Visible = true;
                LoadSterilityDetails(Convert.ToInt64(cboGBLNo3.Text.Trim()), Convert.ToInt16(txtSterClassID.Text.Trim()));
                btnClose.Visible = true; btnClose.BringToFront();
                btnAddDetail.Enabled = false;
                btnDeleteDetail.Enabled = false;
                btnOKDetail.Enabled = false;
            }

            PanelSetting();

            if (strFileAccess != "RO")
            {
                if (dgvSterility.RowCount != 0)
                {
                    btnDeleteDetail.Enabled = true;
                }

                if (txtReviewer.Text.Trim() != "")
                {
                    btnSterSubmit.Enabled = false;
                }
                else
                {
                    btnSterSubmit.Enabled = true;
                }

                if (txtReviewer.Text.Trim() != "" && txtApprover.Text.Trim() == "")
                {
                    btnApproverESign.Enabled = true;
                }
                else
                {
                    btnApproverESign.Enabled = false;
                }                
            }
        }

        private void LoadSterilityDetails(Int64 cGBLNo, Int16 cSterClassID)
        {           
            try
            {
                dtSterility = null;
                dtSterility = GISClass.Tools.LabelSterilityDetails(cGBLNo, cSterClassID);
                bsSterility.DataSource = dtSterility;
                bnSterility.BindingSource = bsSterility;
                dgvSterility.DataSource = bsSterility;
                DataGridControlSterilitySetting();
                //ClearControls(this.pnlSterility);
                BindSterility();    
            }
            catch { }

        }

        private void LoadGBLNos(Int16 cLabelType)
        {
            if (cLabelType == 1 || cLabelType == 2 || cLabelType == 4)
            {    
                cboGBLNo1.DataSource = null;
                cboGBLNo2.DataSource = null;
                cboGBLNo3.DataSource = null;

                DataTable dt = new DataTable();
                dt = GISClass.Tools.LabelGBLNos();
                if (dt == null)
                {
                    MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                    return;
                }

                if (dt.Rows.Count == 0)
                    return;

                if (cLabelType == 1)
                {
                    cboGBLNo1.DataSource = dt;
                    cboGBLNo1.DisplayMember = "GBLNo";
                    cboGBLNo1.ValueMember = "GBLNo";
                    cboGBLNo1.SelectedIndex = 0;
                 
                }
                else if (cLabelType == 2)
                {
                    cboGBLNo2.DataSource = dt;
                    cboGBLNo2.DisplayMember = "GBLNo";
                    cboGBLNo2.ValueMember = "GBLNo";
                    cboGBLNo2.SelectedIndex = 0;
                    cboGBLNo2.Text= cboGBLNo2.SelectedValue.ToString();
                }
                else
                {
                    cboGBLNo3.DataSource = dt;
                    cboGBLNo3.DisplayMember = "GBLNo";
                    cboGBLNo3.ValueMember = "GBLNo";
                    cboGBLNo3.SelectedIndex = 0; 
                }
            }
        }

        private void LoadAutoclave(Int16 cLabelType)
        {
            if (cLabelType == 3 || cLabelType == 5)
            {
                cboMediaAutoclave.DataSource = null;
                cboWGAutoclave.DataSource = null;

                DataTable dt = new DataTable();
                dt = GISClass.Tools.LabelAutoclave(cLabelType);
                if (dt == null)
                {
                    MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                    return;
                }

                if (dt.Rows.Count == 0)
                    return;

                if (cLabelType == 3)
                {
                    cboMediaAutoclave.DataSource = dt;
                    cboMediaAutoclave.DisplayMember = "AutoclaveNo";
                    cboMediaAutoclave.ValueMember = "AutoclaveNo";
                }
                else
                {
                    cboWGAutoclave.DataSource = dt;
                    cboWGAutoclave.DisplayMember = "AutoclaveNo";
                    cboWGAutoclave.ValueMember = "AutoclaveNo";
                }
            }
        }
      
        private void LoadMedia()
        {
            cboMedia.DataSource = null;
            DataTable dt = new DataTable();
            dt = GISClass.Tools.LabelMedia();
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            if (dt.Rows.Count == 0)
                return;

            cboMedia.DataSource = dt;
            cboMedia.DisplayMember = "MediaName";
            cboMedia.ValueMember = "MediaName";
        }

        private void LoadIngLabelDesc()
        {
            cboIngLabelDesc.DataSource = null;
            DataTable dt = new DataTable();
            dt = GISClass.Tools.LabelIngDesc();
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            if (dt.Rows.Count == 0)
                return;

            cboIngLabelDesc.DataSource = dt;
            cboIngLabelDesc.DisplayMember = "IngLabelDesc";
            cboIngLabelDesc.ValueMember = "IngLabelID";
        }
        
        private void LoadFieldList(Int16 cSterLabelTypeID, Int16 cFieldno)
        {
            //dgvFld1.DataSource = null;

            //dtFieldList = GISClass.Tools.LabelFieldList(cSterLabelTypeID, cFieldno);
            //if (dtFieldList == null)
            //{
            //    MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
            //    return;
            //}
            //dgvFld1.DataSource = dtFieldList;
            //StandardDGVSetting(dgvFld1);
            //dgvFld1.Columns[0].Width = 377;
            //dgvFld1.Columns[1].Visible = false;

            //dgvFld1.DataSource = null;

            //dtField1List = GISClass.Tools.LabelFieldList(cSterLabelTypeID, cFieldno);
            //if (dtField1List == null)
            //{
            //    MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
            //    return;
            //}
            //dgvFld1.DataSource = dtField1List;
            //StandardDGVSetting(dgvFld1);
            //dgvFld1.Columns[0].Width = 377;
            //dgvFld1.Columns[1].Visible = false;

            //dgvFld1.DataSource = null;

            //dtField1List = GISClass.Tools.LabelFieldList(cSterLabelTypeID, cFieldno);
            //if (dtField1List == null)
            //{
            //    MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
            //    return;
            //}
            //dgvFld1.DataSource = dtField1List;
            //StandardDGVSetting(dgvFld1);
            //dgvFld1.Columns[0].Width = 377;
            //dgvFld1.Columns[1].Visible = false;
        }


        private void LoadSterDesc(Int16 cSterClassID)
        {
            dgvFld1.DataSource = null;

            dtSterDesc = GISClass.Tools.LabelSterDesc(cSterClassID);
            if (dtSterDesc == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvFld1.DataSource = dtSterDesc;
            StandardDGVSetting(dgvFld1);
            dgvFld1.Columns["Field1"].Width = 377;
            dgvFld1.Columns["Field2"].Visible = false;
            dgvFld1.Columns["Field3"].Visible = false;
            dgvFld1.Columns["SterilityID"].Visible = false;              
        }

        private void LoadSterility()
        {
            cboGBLNo3.Text = dgvFile.CurrentRow.Cells["GBLNo"].Value.ToString();           
        }

        private void InitParams()
        {
            // Load some dummy data for Label app args; it needs all params sent from New GIS even when not required by label.btw
            nGBLNo = 1;
            sSlashNo = "12";
            sIngLabelDesc = "IngLabelDesc";
            sMedName = "MediumName";
            sLotNo = "12345";
            nAutoclave = 0;
            sPrepDate = DateTime.Now;
            sSterDate = DateTime.Now;
            sExpDate = DateTime.Now;
            nSterClassID = 1;
            nSterBtwID = 1;
            nLabelCount = 1;
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

             switch (nLabelTypeID)
            {
                case 1: // GBL Nos/Slash
                    dgvFile.Columns["LabelID"].HeaderText = "Label ID";
                    dgvFile.Columns["GBLNo"].HeaderText = "GBL No";
                    dgvFile.Columns["SlashNo"].HeaderText = "Slash No";
                    dgvFile.Columns["IsPrinted"].HeaderText = "Printed";
                    dgvFile.Columns["IsCancelled"].HeaderText = "Cancelled";
                    dgvFile.Columns["CreatedBy"].HeaderText = "Created By";
                    dgvFile.Columns["DateCreated"].HeaderText = "Date Created";                 
                    dgvFile.Columns["LabelID"].Width = 70;
                    dgvFile.Columns["GBLNo"].Width = 80;
                    dgvFile.Columns["SlashNo"].Width = 80;
                    dgvFile.Columns["CreatedBy"].Width = 90;
                    dgvFile.Columns["DateCreated"].Width = 70;
                    dgvFile.Columns["LabelTypeID"].Visible = false;  
                    dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["LabelID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvFile.Columns["GBLNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvFile.Columns["SlashNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    break;
                case 2: // Ingredion
                    dgvFile.Columns["LabelID"].HeaderText = "Label ID";
                    dgvFile.Columns["GBLNo"].HeaderText = "GBL No";
                    dgvFile.Columns["IngLabelDesc"].HeaderText = "Label Description";
                    dgvFile.Columns["IsPrinted"].HeaderText = "Printed";
                    dgvFile.Columns["IsCancelled"].HeaderText = "Cancelled";
                    dgvFile.Columns["CreatedBy"].HeaderText = "Created By";
                    dgvFile.Columns["DateCreated"].HeaderText = "Date Created";
                    dgvFile.Columns["LabelID"].Width = 70;
                    dgvFile.Columns["GBLNo"].Width = 80;
                    dgvFile.Columns["IngLabelDesc"].Width = 400;
                    dgvFile.Columns["CreatedBy"].Width = 90;
                    dgvFile.Columns["DateCreated"].Width = 70;
                    dgvFile.Columns["LabelTypeID"].Visible = false;
                    dgvFile.Columns["IngLabelID"].Visible = false;
                    dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["LabelID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvFile.Columns["GBLNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                    
                    dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    break;
                case 3: // Media
                    dgvFile.Columns["LabelID"].HeaderText = "Label ID";                  
                    dgvFile.Columns["MediaName"].HeaderText = "Media Name";
                    dgvFile.Columns["PrepDate"].HeaderText = "Prep Date";
                    dgvFile.Columns["LotNo"].HeaderText = "Lot No";
                    dgvFile.Columns["AutoclaveNo"].HeaderText = "Autoclave No";
                    dgvFile.Columns["ExpiryDate"].HeaderText = "Expiry Date";
                    dgvFile.Columns["Period"].HeaderText = "Period";
                    dgvFile.Columns["IsPrinted"].HeaderText = "Printed";
                    dgvFile.Columns["IsCancelled"].HeaderText = "Cancelled";
                    dgvFile.Columns["CreatedBy"].HeaderText = "Created By";
                    dgvFile.Columns["DateCreated"].HeaderText = "Date Created";
                    dgvFile.Columns["LabelID"].Width = 70;
                    dgvFile.Columns["MediaName"].Width = 200;
                    dgvFile.Columns["CreatedBy"].Width = 90;
                    dgvFile.Columns["PrepDate"].Width = 70;
                    dgvFile.Columns["ExpiryDate"].Width = 70;
                    dgvFile.Columns["DateCreated"].Width = 70;
                    dgvFile.Columns["LabelTypeID"].Visible = false;
                    dgvFile.Columns["MediaID"].Visible = false;
                    dgvFile.Columns["PrepDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["ExpiryDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["LabelID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                    
                    dgvFile.Columns["PrepDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvFile.Columns["ExpiryDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    break;
                 case 4: // Sterility   
                    dgvFile.Columns["GBLNo"].HeaderText = "GBL No";
                    dgvFile.Columns["SponsorID"].HeaderText = "Sponsor ID";
                    dgvFile.Columns["SponsorName"].HeaderText = "Sponsor Name";
                    dgvFile.Columns["SterClassDesc"].HeaderText = "Classification";
                    dgvFile.Columns["Reviewer"].HeaderText = "Reviewer";
                    dgvFile.Columns["Approver"].HeaderText = "Approver";
                    //dgvFile.Columns["CreatedBy"].HeaderText = "Created By";
                    //dgvFile.Columns["DateCreated"].HeaderText = "Date Created";
                    dgvFile.Columns["GBLNo"].Width = 80;
                    dgvFile.Columns["SponsorID"].Width = 70;
                    dgvFile.Columns["SponsorName"].Width = 300;
                    dgvFile.Columns["SterClassDesc"].Width = 300;
                    dgvFile.Columns["Reviewer"].Width = 80;
                    dgvFile.Columns["Approver"].Width = 70;
                    //dgvFile.Columns["CreatedBy"].Width = 80;
                    //dgvFile.Columns["DateCreated"].Width = 70;
                    dgvFile.Columns["SterClassID"].Visible = false;
                    dgvFile.Columns["ReviewedByID"].Visible = false;
                    dgvFile.Columns["ApprovedByID"].Visible = false;     
                    dgvFile.Columns["ReviewDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["ApprovalDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvFile.Columns["ReviewDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvFile.Columns["ApprovalDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    break;
                 case 5: // Wrapped Goods
                    dgvFile.Columns["LabelID"].HeaderText = "Label ID";
                    dgvFile.Columns["StrlznDate"].HeaderText = "Sterilization Date";
                    dgvFile.Columns["AutoclaveNo"].HeaderText = "Autoclave No";
                    dgvFile.Columns["Period"].HeaderText = "Period";
                    dgvFile.Columns["ExpiryDate"].HeaderText = "Expiry Date";
                    dgvFile.Columns["IsPrinted"].HeaderText = "Printed";
                    dgvFile.Columns["IsCancelled"].HeaderText = "Cancelled";
                    dgvFile.Columns["CreatedBy"].HeaderText = "Created By";
                    dgvFile.Columns["DateCreated"].HeaderText = "Date Created";
                    dgvFile.Columns["LabelID"].Width = 70;
                    dgvFile.Columns["StrlznDate"].Width = 70;                   
                    dgvFile.Columns["CreatedBy"].Width = 90;
                    dgvFile.Columns["DateCreated"].Width = 70;
                    dgvFile.Columns["LabelTypeID"].Visible = false;
                    dgvFile.Columns["StrlznDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["ExpiryDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
                    dgvFile.Columns["LabelID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                    
                    dgvFile.Columns["StrlznDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    break;
                default:
                    break;
            }
        }

        private void DataGridControlSterilitySetting()
        {
            dgvSterility.EnableHeadersVisualStyles = false;
            dgvSterility.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSterility.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvSterility.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            //dgvSterility.RowHeadersVisible = true;
            dgvSterility.Columns["SlashNo"].HeaderText = "Slash No";
            dgvSterility.Columns["Field1"].HeaderText = "Field 1";
            dgvSterility.Columns["Field2"].HeaderText = "Field 2";
            dgvSterility.Columns["Field3"].HeaderText = "Field 3";
            dgvSterility.Columns["LotNo"].HeaderText = "Lot No";
            dgvSterility.Columns["SKUNo"].HeaderText = "GBL SKU";
            dgvSterility.Columns["LoadNo"].HeaderText = "Load No";
            dgvSterility.Columns["ProcDate"].HeaderText = "Proc Date";
            dgvSterility.Columns["ExpiryDate"].HeaderText = "Exp Date";
            dgvSterility.Columns["LabelCount"].HeaderText = "Label Count";
           
            dgvSterility.Columns["SlashNo"].Width = 40;
            dgvSterility.Columns["Field1"].Width = 130;
            dgvSterility.Columns["Field2"].Width = 130;
            dgvSterility.Columns["Field3"].Width = 130;
            dgvSterility.Columns["LotNo"].Width = 60;
            dgvSterility.Columns["SKUNo"].Width = 60;
            dgvSterility.Columns["LoadNo"].Width = 60;
            dgvSterility.Columns["ProcDate"].Width = 70;
            dgvSterility.Columns["ExpiryDate"].Width = 70;
            dgvSterility.Columns["LabelCount"].Width = 40;
            dgvSterility.Columns["LabelID"].Visible = false;
            dgvSterility.Columns["LabelTypeID"].Visible = false;
            dgvSterility.Columns["GBLNo"].Visible = false;
            dgvSterility.Columns["SterClassID"].Visible = false;
            dgvSterility.Columns["SterilityID"].Visible = false;
            dgvSterility.Columns["ReviewedByID"].Visible = false;
            dgvSterility.Columns["Reviewer"].Visible = false;
            dgvSterility.Columns["ReviewDate"].Visible = false;
            dgvSterility.Columns["ApprovedByID"].Visible = false;
            dgvSterility.Columns["Approver"].Visible = false;
            dgvSterility.Columns["ApprovalDate"].Visible = false;
            dgvSterility.Columns["ProcDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvSterility.Columns["ExpiryDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvSterility.Columns["SlashNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSterility.Columns["LabelCount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;    
        }

        private void PanelSetting()
        {
            switch (nLabelTypeID)
            {
                case 1: // GBL Nos/Slash
                    pnlRecord.Size = new Size(577, 316);                  
                    btnClose.Location = new Point(499, -1);
                    lblHeader.Size = new Size(579, 21);
                    pnlGBLSlash.Location = new Point(39, 61);
                    pnlGBLSlash.Visible = true;                    
                    break;
                case 2: // Ingredion
                    pnlRecord.Size = new Size(960, 430);                   
                    btnClose.Location = new Point(881, -1);
                    lblHeader.Size = new Size(960, 21);
                    pnlIngredion.Location = new Point(31, 70);
                    pnlIngredion.Visible = true;
                    pnlIngredion.BringToFront();
                    txtSponsor.ReadOnly = true;                    
                    break;
                case 3: // Media
                    pnlRecord.Size = new Size(577, 430);                    
                    btnClose.Location = new Point(499, -1);
                    lblHeader.Size = new Size(579, 21);
                    pnlMedia.Visible = true;                    
                    break;
                case 4: // Sterility
                    pnlRecord.Size = new Size(939, 629);                  
                    pnlSterility.Location = new Point(40, 106);                   
                    break;
                case 5: // Wrapped Goods
                    pnlRecord.Size = new Size(577, 380);                    
                    btnClose.Location = new Point(499, -1);
                    lblHeader.Size = new Size(579, 21);
                    pnlWrappedGoods.Location = new Point(38, 58);
                    pnlWrappedGoods.Visible = true;                    
                    break;
                default:
                    break;
            }
        }
            
        private void PrintLabel(string cLabelParam)
        {           
            try
            {   
                using (Process proc = new Process())
                {
                    // Use this for Prod
                    proc.StartInfo.FileName = @"\\gblnj4\GIS\Labels\LabelMain.exe";

                    // Use this for Testing
                    //proc.StartInfo.FileName = @"C:\Maria\Dev\LabelMain\LabelMain\bin\Debug\LabelMain.exe";

                    proc.StartInfo.Arguments = cLabelParam;                
                    proc.Start();   
                }
                MessageBox.Show("Label printing successful!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }  
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; pnlSterility.Visible = true;  btnClose.Visible = true;
            OpenControls(this.pnlRecord, false);
            LoadData();
            pnlRecord.Size = new Size(939, 629);
            pnlSterility.Location = new Point(30, 40);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = true;
            pnlSterility.Visible = true;
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (nSw == 0)
            {
                nSw = 1;                
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
            }
        }

        private void dgvKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void UpdateSterilityDetails()
        {
            bsSterility.EndEdit();
            DataTable dt = dtSterility.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtLabelID.Text = GISClass.DataEntry.NewID("Labels", "LabelID").ToString();
                    SaveSterilityDetails(Convert.ToInt16(txtLabelID.Text), Convert.ToInt64(cboGBLNo3.Text), i, 1, dt);
                }
                dt.Rows.Clear();
            }

            dt = dtSterility.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SaveSterilityDetails(Convert.ToInt16(dt.Rows[i]["LabelID"].ToString()), Convert.ToInt64(cboGBLNo3.Text), i, 2, dt);
                }
                dt.Rows.Clear();
            }
        }

        private void DeleteDetail(int cLabelID)
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("LabelID", cLabelID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelLabelSterility";

            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadData();
        }

        private static int SaveSterilityDetails(int cLabelID, Int64 cGBLNo, int cRow, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            if (sqlcnn == null)
            {
                return 0;
            }

            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
            sqlcmd.Parameters.AddWithValue("@LabelID", cLabelID);
            sqlcmd.Parameters.AddWithValue("@LabelTypeID", 4);                        //4 = Sterility Labels
            sqlcmd.Parameters.AddWithValue("@GBLNo", cGBLNo);
            sqlcmd.Parameters.AddWithValue("@SterClassID", Convert.ToInt16(cDT.Rows[cRow]["SterClassID"].ToString()));
            sqlcmd.Parameters.AddWithValue("@SterilityID", Convert.ToInt16(cDT.Rows[cRow]["SterilityID"].ToString()));
            sqlcmd.Parameters.AddWithValue("@SlashNo", cDT.Rows[cRow]["SlashNo"].ToString());
            sqlcmd.Parameters.AddWithValue("@Field1", cDT.Rows[cRow]["Field1"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@Field2", cDT.Rows[cRow]["Field2"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@Field3", cDT.Rows[cRow]["Field3"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@LotNo", cDT.Rows[cRow]["LotNo"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@SKUNo", cDT.Rows[cRow]["SKUNo"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@LoadNo", cDT.Rows[cRow]["LoadNo"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@ProcDate", Convert.ToDateTime(cDT.Rows[cRow]["ProcDate"].ToString()));
            sqlcmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(cDT.Rows[cRow]["ExpiryDate"].ToString()));
            sqlcmd.Parameters.AddWithValue("@LabelCount", Convert.ToInt16(cDT.Rows[cRow]["LabelCount"].ToString()));

            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditLabelSterility";
            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            {
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }
        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Size = new Size(939, 629);
            pnlSterility.Location = new Point(30, 40);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            pnlSterility.Visible = true;
            ClearControls(this.pnlRecord);
            ClearControls(this.pnlSterility); 
            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlSterility, true);           
            dtSterility.Rows.Clear();

            picField1.Enabled = true;
           
            btnAddDetail.Enabled = true;
            btnDeleteDetail.Enabled = false;
            pnlSterEntry.Enabled = false;
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;            

            LoadData();
            nMode = 2;

            pnlRecord.Size = new Size(939, 629);
            pnlSterility.Location = new Point(30, 40);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
                     
            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlSterEntry, true);
            OpenControls(this.pnlSterility, false);
           
            txtSterSlashNo.Focus();        

            btnAddDetail.Enabled = true;
            btnDeleteDetail.Enabled = true;
           
            tsbAdd.Enabled = false;
            tsbEdit.Enabled = false;
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;
        }

        private void DeleteRecord()
        {
        }

        private void SaveRecord()
        {           
            // Init Variables
            Int64 intGBLNo = 0;
            String strSlashNo = "";
            int intMediaID = 0;            
            String strLotNo = "";
            int intAutoclaveNo = 0;           
            int intIngLabelID = 0;
            int intLabelCount = 1;
            int intPeriod = 0;

            switch (nLabelTypeID)
            {
                case 1: // GBL Nos/Slash
                    intGBLNo = Convert.ToInt64(cboGBLNo1.Text);
                    strSlashNo = txtSlashNo.Text.Trim();                   
                    break;
                case 2: // Ingredion
                    intGBLNo = Convert.ToInt64(cboGBLNo2.Text);
                    intIngLabelID = Convert.ToInt16(txtIngLabelID.Text);                    
                    break;
                case 3: // Media
                    intMediaID = Convert.ToInt16(txtMediaID.Text);                   
                    strLotNo = txtMediaLotNo.Text.Trim();
                             
                   
                    if (rdo3Months.Checked)
                    {
                        intPeriod = 3;
                    }
                    else if (rdo1YearMedium.Checked)
                    {
                        intPeriod = 12;
                    }
                    break;
                case 4: // Sterility                    

                    bsSterility.EndEdit();

                    // Validate if changes were made on the Detail
                    DataTable dtDetails = dtSterility.GetChanges();
                    if (dtDetails != null)
                    {
                        int nPR = ValidateDetails();                                                   // Validation for PR Detail Record
                        if (nPR == 0)
                        {
                            dtDetails.Dispose();
                            return;
                        }
                        UpdateSterilityDetails();                                                             // Save PR Detail Record
                        dtDetails.Dispose();                       

                    }

                    break;
                case 5: // Wrapped Goods               
                    intAutoclaveNo = Convert.ToInt16(cboWGAutoclave.Text);
                    if (rdo6Months.Checked)
                    {
                        intPeriod = 6;
                    }
                    else if (rdo1YearWG.Checked)
                    {
                        intPeriod = 12;
                    }
                    break;
                default:
                    break;
            }
            
            //SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            //SqlCommand sqlcmd = new SqlCommand();
            //sqlcmd.Connection = sqlcnn;

            //sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            //sqlcmd.Parameters.AddWithValue("@LabelID", Convert.ToInt16(txtLabelID.Text));
            //sqlcmd.Parameters.AddWithValue("@LabelTypeID", nLabelTypeID);
            //sqlcmd.Parameters.AddWithValue("@GBLNo", intGBLNo);
            //sqlcmd.Parameters.AddWithValue("@SlashNo", strSlashNo);
            //sqlcmd.Parameters.AddWithValue("@MediaID", intMediaID);
            //sqlcmd.Parameters.AddWithValue("@LotNo", strLotNo);
            //sqlcmd.Parameters.AddWithValue("@AutoclaveNo", intAutoclaveNo);
            //sqlcmd.Parameters.AddWithValue("@Period", intPeriod);
            //sqlcmd.Parameters.AddWithValue("@Field1", strField1);
            //sqlcmd.Parameters.AddWithValue("@Field2", strField2);
            //sqlcmd.Parameters.AddWithValue("@Field3", strField3);
            //sqlcmd.Parameters.AddWithValue("@SKUNo", strSKUNo);
            //sqlcmd.Parameters.AddWithValue("@LoadNo", strLoadNo);
            //sqlcmd.Parameters.AddWithValue("@IngLabelID", intIngLabelID);
            //sqlcmd.Parameters.AddWithValue("@LabelCount", intLabelCount);

            //if (nLabelTypeID == 3) // Media
            //{
            //    sqlcmd.Parameters.AddWithValue("@PrepDate", Convert.ToDateTime(dtpPrepDate.Text));
            //    sqlcmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(dtpMediaExpDate.Text));
            //    sqlcmd.Parameters.AddWithValue("@ProcDate", DBNull.Value);
            //    sqlcmd.Parameters.AddWithValue("@StrlznDate", DBNull.Value);
            //}
           
            //else if (nLabelTypeID == 5) // Wrapped Goods
            //{
            //    sqlcmd.Parameters.AddWithValue("@PrepDate", DBNull.Value);
            //    sqlcmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(dtpWGExpDate.Text));
            //    sqlcmd.Parameters.AddWithValue("@ProcDate", DBNull.Value);
            //    sqlcmd.Parameters.AddWithValue("@StrlznDate", Convert.ToDateTime(dtpStrlznDate.Text));
            //}
            //else
            //{
            //    sqlcmd.Parameters.AddWithValue("@PrepDate", DBNull.Value);
            //    sqlcmd.Parameters.AddWithValue("@ExpiryDate", DBNull.Value);
            //    sqlcmd.Parameters.AddWithValue("@ProcDate", DBNull.Value);
            //    sqlcmd.Parameters.AddWithValue("@StrlznDate", DBNull.Value);
            //}
           
            //sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            //sqlcmd.CommandType = CommandType.StoredProcedure;
            //sqlcmd.CommandText = "spAddEditLabels";
            //try
            //{
            //    sqlcmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    sqlcnn.Dispose();
            //    return;
            //}
            //sqlcnn.Dispose();
            //dgvFile.Refresh();
            //LoadRecords();
            //GISClass.General.FindRecord("DocNo", txtDocNo.Text, bsFile, dgvFile);
            //ClearControls(this.pnlRecord);
            AddEditMode(false);
            //LoadData();
            btnClose.Visible = true;
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

        private void CalculateExpiryDate(String cStarDate, int cLength)
        {

            DateTime expDt;
            DateTime dt = Convert.ToDateTime(cStarDate);
            if (cLength == 1)
            {
                expDt = dt.AddYears(cLength);
            }
            else
            {
                expDt = dt.AddMonths(cLength);
            }
            switch (nLabelTypeID)
            {
                case 3:
                    dtpMediaExpDate.Value = expDt;
                    break;
                case 5:
                    dtpWGExpDate.Value = expDt;
                    break;
                default:
                    break;
            }
        }        

        private void Labels_Load(object sender, EventArgs e)
        {
            FileAccess();

            InitParams();

            pnlRecord.Visible = false;

            dtpProcDate.Value = DateTime.Now;
            dtpPrepDate.Value = DateTime.Now;
            dtpStrlznDate.Value = DateTime.Now;
            dtpMediaExpDate.Value = DateTime.Now;
            dtpWGExpDate.Value = DateTime.Now;

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            if (nLabelTypeID == 4)
            {
                LoadRecords();
                LoadSterDesc(1);
                CreateDetailStructure(); 
            }          
    
            switch (nLabelTypeID)
            {
                case 1:                     // GBL Slash
                    LoadGBLNos(nLabelTypeID);
                    break;
                case 2:                     // Ingredion
                    LoadGBLNos(nLabelTypeID);

                    if (cboGBLNo2.Text.Trim() == "")
                    {
                        txtSponsor.Text = "";
                    }
                    else
                    {
                        txtSponsor.Text = GISClass.Tools.LabelIngSponsor(Convert.ToInt64(cboGBLNo2.Text));
                    }
                    LoadIngLabelDesc();
                    break;
                case 3:                     // Media
                    dtpMediaExpDate.Format = DateTimePickerFormat.Custom;
                    dtpMediaExpDate.CustomFormat = "MM/dd/yyyy";
                    LoadMedia();
                    LoadAutoclave(nLabelTypeID);
                    break;
                case 4:                     // Sterility
                    LoadGBLNos(nLabelTypeID);
                    break;
                case 5:                     // Wrapped Goods
                    dtpStrlznDate.Format = DateTimePickerFormat.Custom;
                    dtpStrlznDate.CustomFormat = "MM/dd/yyyy";    
                    LoadAutoclave(nLabelTypeID);
                    break;
                default:
                    break;
            } 

            if (nLabelSw == 1)
            {
                GISClass.General.FindRecord("GBLNo", intGBLNo.ToString(), bsFile, dgvFile);               
            }
            LoadData();
        }

        private void CreateDetailStructure()
        {
            // Create Master Data table for Add/Edit/Delete functions     
            bsSterility.DataSource = dtSterility;
            dgvSterility.DataSource = bsSterility;
            dtSterility.Columns.Add("LabelID", typeof(Int32));
            dtSterility.Columns.Add("LabelTypeID", typeof(Int16));
            dtSterility.Columns.Add("GBLNo", typeof(Int64));
            dtSterility.Columns.Add("SterClassID", typeof(Int16));
            dtSterility.Columns.Add("SterilityID", typeof(Int16));           
            dtSterility.Columns.Add("SlashNo", typeof(string));
            dtSterility.Columns.Add("Field1", typeof(string));
            dtSterility.Columns.Add("Field2", typeof(string));
            dtSterility.Columns.Add("Field3", typeof(string));
            dtSterility.Columns.Add("LotNo", typeof(string));
            dtSterility.Columns.Add("SKUno", typeof(Int64));
            dtSterility.Columns.Add("LoadNo", typeof(string));
            dtSterility.Columns.Add("ProcDate", typeof(DateTime));
            dtSterility.Columns.Add("ExpiryDate", typeof(DateTime));
            dtSterility.Columns.Add("LabelCount", typeof(Int16));
            dtSterility.Columns.Add("ReviewedByID", typeof(Int16));
            dtSterility.Columns.Add("Reviewer", typeof(string));
            dtSterility.Columns.Add("ReviewDate", typeof(DateTime));
            dtSterility.Columns.Add("ApprovedByID", typeof(Int16));
            dtSterility.Columns.Add("Approver", typeof(string));
            dtSterility.Columns.Add("ApprovalDate", typeof(DateTime));

            DataGridControlSterilitySetting();
            //dtSterility.Columns.Add("CreatedByID", typeof(Int16));
            //dtSterility.Columns.Add("LastUpdate", typeof(DateTime));
            //dtSterility.Columns.Add("LastUserID", typeof(Int16));
        }

        private void BindSterility()
        {
            // Clear bindings first
            foreach (Control c in pnlSterility.Controls)
            {
                c.DataBindings.Clear();
            }
            
            try
            {
                txtLabelID.DataBindings.Add("Text", bsSterility, "LabelID");
                txtLabelTypeID.DataBindings.Add("Text", bsSterility, "LabelTypeID"); 
                cboGBLNo3.DataBindings.Add("Text", bsSterility, "GBLNo");
                txtSterClassID.DataBindings.Add("Text", bsSterility, "SterClassID");
                txtSterilityID.DataBindings.Add("Text", bsSterility, "SterilityID");
                txtSterSlashNo.DataBindings.Add("Text", bsSterility, "SlashNo");
                txtFld1.DataBindings.Add("Text", bsSterility, "Field1");
                txtFld2.DataBindings.Add("Text", bsSterility, "Field2");
                txtFld3.DataBindings.Add("Text", bsSterility, "Field3");
                txtSterLotNo.DataBindings.Add("Text", bsSterility, "LotNo");
                txtSKUNo.DataBindings.Add("Text", bsSterility, "SKUNo");
                txtLoadNo.DataBindings.Add("Text", bsSterility, "LoadNo");
                txtSterLabelCount.DataBindings.Add("Text", bsSterility, "LabelCount");
                txtReviewedByID.DataBindings.Add("Text", bsSterility, "ReviewedByID");
                txtReviewer.DataBindings.Add("Text", bsSterility, "Reviewer");
                txtApprovedByID.DataBindings.Add("Text", bsSterility, "ApprovedByID");
                txtApprover.DataBindings.Add("Text", bsSterility, "Approver");            

                Binding ProcDateBinding;
                ProcDateBinding = new Binding("Text", bsSterility, "ProcDate");
                ProcDateBinding.Format += new ConvertEventHandler(DateBinding_Format);
                dtpProcDate.DataBindings.Add(ProcDateBinding);

                Binding ExpDateBinding;
                ExpDateBinding = new Binding("Text", bsSterility, "ExpiryDate");
                ExpDateBinding.Format += new ConvertEventHandler(DateBinding_Format);
                dtpSterExpiryDate.DataBindings.Add(ExpDateBinding);
                
            }
            catch
            { }
        }

        private void DateBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "";
        }

        public void SendEMail(int cMode)
        {
            try
            {
                string strReviewerEmail = "";
                string strApproverEmail = "";
              
                string strRevFirstName = "";
                string strApproverFirstName = "";                

                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@GBLNo", Convert.ToInt64(cboGBLNo3.Text.Trim()));
                sqlcmd.Parameters.AddWithValue("@SterClassID", Convert.ToInt16(txtSterClassID.Text.Trim()));

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spGetLabelEmailAddresses";

                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strReviewerEmail = sqldr.GetValue(0).ToString();
                    strApproverEmail = sqldr.GetValue(1).ToString();                    
                    strRevFirstName = sqldr.GetValue(2).ToString();
                    strApproverFirstName = sqldr.GetValue(3).ToString();                   
                }
                else
                {
                    MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                sqldr.Close(); sqlcmd.Dispose();

                //For Testing
                //string strEMail = "myounes@gibraltarlabsinc.com; adelacruz@gibraltarlabsinc.com; mvenanzi@gibraltarlabsinc.com;"; 

                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                oMsg.HTMLBody = "<FONT face=\"Arial\">";

                string strClassName = "";

                switch (Convert.ToInt16(txtSterClassID.Text))
                {
                    case 1:
                        strClassName = "GBL Routine";
                        break;
                    case 2:                     
                        strClassName = "Catalent ";
                        break;
                    case 3:                 
                        strClassName = "Cytonet Kit";
                        break;
                    case 4:      
                        strClassName = "Cytonet Media Fill Kit";
                        break;
                    default:
                        break;
                }
                String Body1 = null;

                // Set Email body.   
                if (cMode == 1)
                {
                    Body1 = "Dear " + strRevFirstName + "," + Environment.NewLine + Environment.NewLine +
                                   "Label for GBL# " + cboGBLNo3.Text.Trim() + " has been approved. " + Environment.NewLine;
                }
                else if (cMode == 2)
                {
                    Body1 = "Dear " + strApproverFirstName + "," + Environment.NewLine + Environment.NewLine +
                                  "Label for GBL# " + cboGBLNo3.Text.Trim() + " has been submitted for your approval. Please check. " + Environment.NewLine;
                }
               
                txtBody.Text = Body1;

                string strBody = txtBody.Text.Replace("\r\n", "<br />");
                string strSignature = ReadSignature();
                strBody = strBody + "<br /><br />" + strSignature;

                oMsg.HTMLBody += strBody.Trim();

                //Subject line
                oMsg.Subject = "Sterilization Label Approval Email for GBL #: " + cboGBLNo3.Text.Trim() + " - " + strClassName;
               
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

                if (cMode == 1)
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(strReviewerEmail);
                    oMsg.CC = strApproverEmail;
                }
                else if (cMode == 2)
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(strApproverEmail);
                    oMsg.CC = strReviewerEmail;
                }               

                //Outlook..Recipient oRecip = (Outlook.Recipient)oRecips.Add(strReviewerEmail);

                //oRecip.Resolve();
                //oMsg.Display();
                // Send.
                //oMsg.Send();
               ((Outlook._MailItem)oMsg).Send();

                // Clean up.
                oRecips = null;
                oMsg = null;
                oApp = null;

                // Update Email info
                UpdateLabelEmailDate(cboGBLNo3.Text.Trim());

                if (cMode == 1)
                {
                    MessageBox.Show(cboGBLNo3.Text.Trim() + " has been sent for approval!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (cMode == 2)
                {
                    MessageBox.Show(cboGBLNo3.Text.Trim() + " has been approved and a corresponding email was sent to the requestor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private string ReadSignature()
        {
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
            return signature;
        }
        private void Labels_KeyDown(object sender, KeyEventArgs e)
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
            nMode = 0;

            if (nLabelTypeID != 4)
            {
                pnlRecord.Visible = false; btnClose.Visible = false;
                this.Close();
            }
            else
            {
                pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
                LoadRecords();
                dgvFile.Focus();
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (txtSlashNo.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Slash Count!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSlashNo.Focus();
                return;
            }

            // This is for GBL No/Slash No labels
            strLabelParam = "";
            strLabelParam = Convert.ToString(nLabelTypeID) + " " + cboGBLNo1.Text.Trim() + " " + txtSlashNo.Text.Trim() + " " + sIngLabelDesc + 
                            " "  + sMedName + " " + sLotNo + " " + Convert.ToString(nAutoclave)   + " " + sPrepDate.ToShortDateString() +
                            " " + sSterDate.ToShortDateString() + " " + sExpDate.ToShortDateString() + " " + Convert.ToString(nSterClassID) +
                            " " + Convert.ToString(nSterBtwID)  + " " + Convert.ToString(nLabelCount);
            PrintLabel(strLabelParam);              
        }

        private void cboGBLNo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboGBLNo1.Text.Trim() != "")
                {
                    // Check if GBL entered is numeric
                    int Num;
                    bool isNum = int.TryParse(cboGBLNo1.Text.ToString(), out Num);
                    if (!isNum)
                    {
                        MessageBox.Show("Entry must be numeric!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    // Check if GBL exists in GIS
                    bool isExists = false;

                    isExists = GISClass.Tools.LabelGBLExists(Convert.ToInt64(cboGBLNo1.Text));

                    if (!isExists)
                    {
                        MessageBox.Show("GBL Number not found. Please enter a valid GBL Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }                   
                }
            }
        }

        private void rdo6Months_Click(object sender, EventArgs e)
        {
            string dtStr = dtpStrlznDate.Value.ToShortDateString();
            CalculateExpiryDate(dtStr, 6);
        }

        private void rdo1YearWG_Click(object sender, EventArgs e)
        {
            string dtStr = dtpStrlznDate.Value.ToShortDateString();
            CalculateExpiryDate(dtStr, 1);
        }

        private void dtpStrlznDate_ValueChanged(object sender, EventArgs e)
        {
            rdo6Months.Checked =false;
            rdo1YearWG.Checked = false;           
        }

        private void btnMediaPrint_Click(object sender, EventArgs e)
        {
            if (txtMediaLabelCount.Text.Trim() == "")
            {
                MessageBox.Show("Please enter label Count!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMediaLabelCount.Focus();
                return;
            }

            // This is for Media labels
            strLabelParam = "";

            strLabelParam = Convert.ToString(nLabelTypeID) + " " + Convert.ToString(nGBLNo) + " " + sSlashNo + " " + sIngLabelDesc +
                            " " + (char)34 + cboMedia.Text.Trim() + (char)34 + " " + (char)34 + txtMediaLotNo.Text.Trim() + (char)34 + 
                            " " + cboMediaAutoclave.Text.Trim() + " " + dtpPrepDate.Value.ToShortDateString() + " " + sSterDate.ToShortDateString() + 
                            " " + dtpMediaExpDate.Value.ToShortDateString() + " " + Convert.ToString(nSterClassID) + " " +
                            Convert.ToString(nSterBtwID)  + " " + txtMediaLabelCount.Text.Trim();
                 
            PrintLabel(strLabelParam); ;  
        }

        private void btnIngPrint_Click(object sender, EventArgs e)
        {
            // This is for Ingredion labels
            strLabelParam = "";
            strLabelParam = Convert.ToString(nLabelTypeID) + " " + cboGBLNo2.Text.Trim() + " " + sSlashNo + " " + (char)34 + cboIngLabelDesc.Text.Trim() + (char)34 +
                            " " + sMedName + " " + sLotNo + " " + Convert.ToString(nAutoclave) + " " + sPrepDate.ToShortDateString() +
                            " " + sSterDate.ToShortDateString() + " " + sExpDate.ToShortDateString() + " " + Convert.ToString(nSterClassID) +
                            " " + Convert.ToString(nSterBtwID) + " " + Convert.ToString(nLabelCount);
            PrintLabel(strLabelParam);   
        }
       
        private void rdoFresh_CheckedChanged(object sender, EventArgs e)
        {
            string dtStr = dtpPrepDate.Value.ToShortDateString();
           //MessageBox.Show(Convert.ToString(dtpPrepDate.Value); 
            CalculateExpiryDate(dtStr, 1);
        }

        private void rdo3Months_CheckedChanged(object sender, EventArgs e)
        {
            string dtStr = dtpPrepDate.Value.ToShortDateString();
            CalculateExpiryDate(dtStr, 3);
        }

        private void rdo1YearMedium_CheckedChanged(object sender, EventArgs e)
        {
            string dtStr = dtpPrepDate.Value.ToShortDateString();
            CalculateExpiryDate(dtStr, 1);
        }

        private void cboGBLNo2_Click(object sender, EventArgs e)
        {
            if (cboGBLNo2.Text.Trim() != "")
            {
                txtSponsor.Text = GISClass.Tools.LabelIngSponsor(Convert.ToInt64(cboGBLNo2.Text));
            }
        }

        private void btnSterSubmit_Click(object sender, EventArgs e)
        {
            if (cboGBLNo3.Text.Trim() == "")
            {
                return;
            }

            if (dgvSterility.RowCount == 0)
            {
                MessageBox.Show("There are no line details yet for this GBL Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("This GBL Label will be submitted now for approval. Are you sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }

            UpdateLabelReviewer();
            LoadData();
            SendEMail(2);
            btnSterSubmit.Enabled = false;            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cboGBLNo1.Text = "";
            txtSlashNo.Text = "";
            chkComposite.Checked = false;
            txtLabelCount.Text = "";
        }

        private void btnMediaClear_Click(object sender, EventArgs e)
        {
            cboMedia.Text = "";
            txtMediaLotNo.Text = "";
            cboMediaAutoclave.Text = "";
            rdoFresh.Checked = false;
            rdo3Months.Checked = false;
            rdo1YearMedium.Checked = false;
            dtpPrepDate.Value = DateTime.Now;
            dtpMediaExpDate.Value = DateTime.Now;
        }

        private void btnIngClear_Click(object sender, EventArgs e)
        {
            cboGBLNo2.Text = "";
            txtSponsor.Text = "";
            cboIngLabelDesc.Text = "";
        }

        private void btnWGClear_Click(object sender, EventArgs e)
        {
            dtpStrlznDate.Value = DateTime.Now;
            dtpWGExpDate.Value = DateTime.Now;
            cboWGAutoclave.Text = "";
            rdo6Months.Checked = false;
            rdo1YearWG.Checked = false;
        }

        private void btnWGPrint_Click(object sender, EventArgs e)
        {
            if (txtWGLabelCount.Text.Trim() == "")
            {
                MessageBox.Show("Please enter label Count!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtWGLabelCount.Focus();
                return;
            }

            // This is for Wrapped Goods labels
            strLabelParam = "";
            strLabelParam = Convert.ToString(nLabelTypeID) + " " + Convert.ToString(nGBLNo) + " " + sSlashNo + " " + sIngLabelDesc +
                            " " + sMedName + " " + sLotNo + " " +  cboWGAutoclave.Text.Trim() + " " + sPrepDate.ToShortDateString() +
                            " " + dtpStrlznDate.Value.ToShortDateString() + " " + dtpWGExpDate.Value.ToShortDateString() + " " + 
                            Convert.ToString(nSterClassID) + " " + Convert.ToString(nSterBtwID) + " " + txtWGLabelCount.Text.Trim();
            PrintLabel(strLabelParam);        
        }

        private String ConvertLabelDates(int cDa, int cMo, int cYr)
        {
            string strFinalDay = (cDa.ToString("00"));
            string strFinalMo = (cMo.ToString("00"));

            return (strFinalMo + strFinalDay + cYr.ToString());
        }

        // MY 01/19/2016 - START: txt/dgvFld1 events
        private void dgvFld1_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtFld1.Text = dgvFld1.CurrentRow.Cells["Field1"].Value.ToString();
                txtFld2.Text = dgvFld1.CurrentRow.Cells["Field2"].Value.ToString();
                txtFld3.Text = dgvFld1.CurrentRow.Cells["Field3"].Value.ToString();
                txtSterilityID.Text = dgvFld1.CurrentRow.Cells["SterilityID"].Value.ToString();
                dgvFld1.Visible = false;
            }
        }

        private void dgvFld1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvFld1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtFld1.Text = dgvFld1.CurrentRow.Cells["Field1"].Value.ToString();
                    txtFld2.Text = dgvFld1.CurrentRow.Cells["Field2"].Value.ToString();
                    txtFld3.Text = dgvFld1.CurrentRow.Cells["Field3"].Value.ToString();
                    txtSterilityID.Text = dgvFld1.CurrentRow.Cells["SterilityID"].Value.ToString();
                    dgvFld1.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvFld1.Visible = false;
                }
            }
        }
        private void txtFld1_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvFld1.Visible = true; dgvFld1.BringToFront();
            }
        }

        private void dgvFld1_Leave(object sender, EventArgs e)
        {
            dgvFld1.Visible = false;
        }

        private void txtFld1_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwField1;
                dvwField1 = new DataView(dtSterDesc, "Field1 like '%" + txtFld1.Text.Trim().Replace("'", "''") + "%'", "Field1", DataViewRowState.CurrentRows);
                dvwSetUp(dgvFld1, dvwField1);
            }
        }

        private void dgvFld1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtFld1.Text = dgvFld1.CurrentRow.Cells["Field1"].Value.ToString();
                txtFld2.Text = dgvFld1.CurrentRow.Cells["Field2"].Value.ToString();
                txtFld3.Text = dgvFld1.CurrentRow.Cells["Field3"].Value.ToString();
                txtSterilityID.Text = dgvFld1.CurrentRow.Cells["SterilityID"].Value.ToString();
                dgvFld1.Visible = false;
                dgvFld1.BringToFront();
            }
        }

        private void picFld1_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadSterDesc(sterClassID);                
                dgvFld1.Visible = true; dgvFld1.BringToFront();
            }
        }
        // MY 01/19/2016 - END: txt/dgvFld1 events    

        private void rdoGBLRoutine_CheckedChanged(object sender, EventArgs e)
        {
            sterClassID = 1;
        }

        private void rdoCatalent_CheckedChanged(object sender, EventArgs e)
        {
            sterClassID = 2;
        }

        private void rdoCytonet_CheckedChanged(object sender, EventArgs e)
        {
            sterClassID = 3;
        }

        private void rdoCytonetMedia_CheckedChanged(object sender, EventArgs e)
        {
            sterClassID = 4;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            int nStart;
            int nEnd;

            nStart = Convert.ToInt16(txtStart.Text);
            nEnd   = Convert.ToInt16(txtEnd.Text);

            for (int i = 1; i <= nEnd; i++)
            {                
                dgvSterility.Rows[i].Cells["EquipmentNo"].Value = txtFld1.Text;
                dgvSterility.Rows[i].Cells["EquipmentName"].Value = txtFld1.Text;
                dgvSterility.Rows[i].Cells["CalibrationDate"].Value = txtFld1.Text;
                dgvSterility.Rows[i].Cells["MaintenanceDate"].Value = txtFld1.Text;               
            }
        }

        private void btnOKDetail_Click(object sender, EventArgs e)
        {
            int nPR = ValidateDetails();                                                         // Validation for PR Detail Record
            if (nPR == 0)
            {
                return;
            }

            int nR ;
            
            if (txtRowCount.Text == "")
            {
                nR = 1;
            }
            else
            {
                 nR = Convert.ToInt16(txtRowCount.Text);
            }

            CreateSterilityRow(Convert.ToInt16(txtSterSlashNo.Text));                         
            bsSterility.DataSource = dtSterility;
            dgvSterility.DataSource = bsSterility;

            BindSterility();
            DataGridControlSterilitySetting();
           
            btnAddDetail.Enabled = true;
            btnDeleteDetail.Enabled = true;
            btnOKDetail.Enabled = false;
            tsbSave.Enabled = true;          
        }

        private void CreateSterilityRow(Int16 cSlashNo)
        {
            DataRow dR = dtSterility.NewRow();
            dR["LabelID"] = 1;
            dR["LabelTypeID"] = nLabelTypeID;
            dR["GBLNo"] = Convert.ToInt64(cboGBLNo3.Text);
            dR["SterClassID"] = Convert.ToInt16(txtSterClassID.Text);
            dR["SterilityID"] = Convert.ToInt16(txtSterilityID.Text);
            dR["SlashNo"] = cSlashNo;
            dR["Field1"] = txtFld1.Text;
            dR["Field2"] = txtFld2.Text;
            dR["Field3"] = txtFld3.Text;
            dR["LotNo"] = txtSterLotNo.Text;
            dR["SKUNo"] = txtSKUNo.Text;
            dR["LoadNo"] = txtLoadNo.Text;
            dR["ProcDate"] = Convert.ToDateTime(dtpProcDate.Text);
            dR["ExpiryDate"] = Convert.ToDateTime(dtpSterExpiryDate.Text);
            dR["LabelCount"] = Convert.ToInt16(txtSterLabelCount.Text);
            //dR["CreatedByID"] = LogIn.nUserID;
            //dR["DateCreated"] = DateTime.Now;
            //dR["LastUpdate"] = DateTime.Now;
            //dR["LastUserID"] = LogIn.nUserID;

            dtSterility.Rows.Add(dR);

            //bsSterility.DataSource = dtSterility;
            //dgvSterility.DataSource = bsSterility;
        }

        private void UpdateLabelReviewer()
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection(); ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@GBLNo", Convert.ToInt64(cboGBLNo3.Text.Trim()));
            sqlcmd.Parameters.AddWithValue("@SterClassID", Convert.ToInt16(txtSterClassID.Text.Trim()));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdateLabelReviewer";

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

        private void UpdateLabelEmailDate(string cPONo)
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@GBLNo", Convert.ToInt64(cboGBLNo3.Text.Trim()));
            sqlcmd.Parameters.AddWithValue("@SterClassID", Convert.ToInt16(txtSterClassID.Text.Trim()));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdateLabelEmailDate";

            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private int ValidateDetails()
        {
            if (nMode != 0)
            {
                if (txtSterSlashNo.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Slash No!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtSterSlashNo.Focus();
                    return 0;
                }     
                if (txtFld1.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Field 1!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtFld1.Focus();
                    return 0;
                }               
            }
            return 1;
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            nMode = 1;
            
            pnlSterEntry.Enabled = true;
            ClearControls(this.pnlSterEntry);
            OpenControls(this.pnlSterEntry, true);           

            btnAddDetail.Enabled = false;
            btnDeleteDetail.Enabled = false;
            btnOKDetail.Enabled = true;

            AddEditMode(true);

            picField1.Enabled = true;
            tsbCancel.Enabled = true;

            foreach (Control c in pnlSterEntry.Controls)
            {
                c.DataBindings.Clear();
            }
            
            //txtSterClassID.Text = Convert.ToString(sterClassID);
        }

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            int dRow = dgvSterility.CurrentRow.Index;
            int intLabelID;

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }

            intLabelID = Convert.ToInt16(dgvSterility.CurrentRow.Cells["LabelID"].Value.ToString());

            dgvSterility.Rows.RemoveAt(dRow);

            if (dgvSterility.Rows.Count == 0)
            {
                btnDeleteDetail.Enabled = false;
            }

            DeleteDetail(intLabelID);

            AddEditMode(false);
        }

        private void rdoGBLRoutine_Click(object sender, EventArgs e)
        {            
            txtSterClassID.Text = Convert.ToString(1);
        }

        private void rdoCatalent_Click(object sender, EventArgs e)
        {
            txtSterClassID.Text = Convert.ToString(2);
        }

        private void rdoCytonet_Click(object sender, EventArgs e)
        {
            txtSterClassID.Text = Convert.ToString(3);
        }

        private void rdoCytonetMedia_Click(object sender, EventArgs e)
        {
            txtSterClassID.Text = Convert.ToString(4);
        }
        
        private void cboGBLNo2_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboGBLNo2.Text.Trim() != "")
                {
                    txtSponsor.Text = GISClass.Tools.LabelIngSponsor(Convert.ToInt64(cboGBLNo2.Text));
                }
            }
            catch { }
        }

        private void btnApproverESign_Click(object sender, EventArgs e)
        {
            using (ESignature eSignature = new ESignature())
            {
                eSignature.Location = new Point(405, 340);
                eSignature.eSign = 8;
                eSignature.eGBLNo = Convert.ToInt64(cboGBLNo3.Text.Trim());
                eSignature.eSterClassID = Convert.ToInt16(txtSterClassID.Text.Trim());
                if (eSignature.ShowDialog() == DialogResult.OK)
                {
                    SendEMail(1);
                    LoadData();
                    AddEditMode(false);
                    nMode = 0;
                }
            }
        }

        private void btnCatalent_Click(object sender, EventArgs e)
        {
            // This is for Sterility Catalent labels
            strLabelParam = "";

            nSterBtwID = 1;
            strLabelParam = Convert.ToString(nLabelTypeID) + " " + cboGBLNo3.Text.Trim() + " " + sSlashNo + " " + sIngLabelDesc +
                            " " + sMedName + " " + sLotNo + " " + Convert.ToString(nAutoclave) + " " + sPrepDate.ToShortDateString() +
                            " " + sSterDate.ToShortDateString() + " " + sExpDate.ToShortDateString() + " " + txtSterClassID.Text.Trim() +
                            " " + Convert.ToString(nSterBtwID) + " " + Convert.ToString(nLabelCount);
            PrintLabel(strLabelParam);   
        }

        private void btnFullKit_Click(object sender, EventArgs e)
        {
            // This is for Sterility Full-Kit labels
            strLabelParam = "";
            nSterBtwID = 2;
            strLabelParam = Convert.ToString(nLabelTypeID) + " " + cboGBLNo3.Text.Trim() + " " + sSlashNo + " " + sIngLabelDesc +
                            " " + sMedName + " " + sLotNo + " " + Convert.ToString(nAutoclave) + " " + sPrepDate.ToShortDateString() +
                            " " + sSterDate.ToShortDateString() + " " + sExpDate.ToShortDateString() + " " + txtSterClassID.Text.Trim() +
                            " " + Convert.ToString(nSterBtwID) + " " + Convert.ToString(nLabelCount);
            PrintLabel(strLabelParam);   
        }

        private void btnSingleItem_Click(object sender, EventArgs e)
        {
            // This is for Sterility Single labels
            strLabelParam = "";
            nSterBtwID = 3;
            strLabelParam = Convert.ToString(nLabelTypeID) + " " + cboGBLNo3.Text.Trim() + " " + sSlashNo + " " + sIngLabelDesc +
                            " " + sMedName + " " + sLotNo + " " + Convert.ToString(nAutoclave) + " " + sPrepDate.ToShortDateString() +
                            " " + sSterDate.ToShortDateString() + " " + sExpDate.ToShortDateString() + " " + txtSterClassID.Text.Trim() +
                            " " + Convert.ToString(nSterBtwID) + " " + Convert.ToString(nLabelCount);
            PrintLabel(strLabelParam);   
        }

        private void btnNonSterile_Click(object sender, EventArgs e)
        {
            // This is for Sterility Non-Sterile labels
            strLabelParam = "";
            nSterBtwID = 4;
            strLabelParam = Convert.ToString(nLabelTypeID) + " " + cboGBLNo3.Text.Trim() + " " + sSlashNo + " " + sIngLabelDesc +
                            " " + sMedName + " " + sLotNo + " " + Convert.ToString(nAutoclave) + " " + sPrepDate.ToShortDateString() +
                            " " + sSterDate.ToShortDateString() + " " + sExpDate.ToShortDateString() + " " + txtSterClassID.Text.Trim() +
                            " " + Convert.ToString(nSterBtwID) + " " + Convert.ToString(nLabelCount);
            PrintLabel(strLabelParam);   
        }

        private void btnSterCopy_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt16(txtStart.Text) > Convert.ToInt16(txtEnd.Text))
            {
                MessageBox.Show("Invalid copy range!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int nR = Convert.ToInt16(txtEnd.Text) - Convert.ToInt16(txtStart.Text);
            int nS = Convert.ToInt16(txtStart.Text);
            int nE = Convert.ToInt16(txtEnd.Text);

            Int16 nSlashNo = Convert.ToInt16(txtSterSlashNo.Text.Trim());

            for (int i = 0; i <= nR; i++)
            {
                nSlashNo += 1;                
                CreateSterilityRow(nSlashNo);
            }
        }
  
    }
}
