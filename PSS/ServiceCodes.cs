//ServiceCodes.cs
// AUTHOR       : ALEXANDER M. DELA CRUZ
// TITLE        : Software Developer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Service Codes File Maintenance
// Added 7 new columns/controls: TEST SERVICES -> METHODS and Fixed bug with Dept not saving - Stanley Tsao 08/24/2017

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
    public partial class ServiceCodes : PSS.TemplateForm
    {
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private int nCtr = 0;
        private int nSw = 0;
        private string strFileAccess = "RO";

        public ServiceCodes()
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
            chkShowInactive.Click += new EventHandler(chkShowInactiveClickHandler);
        }

        private void LoadRecords(int cStatus)
        {
            DataTable dt = PSSClass.ServiceCodes.SCMaster(cStatus);
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "ServiceCode <> 0";
            DataGridSetting();

            if (tsddbSearch.DropDownItems.Count == 0)
            {
                int i = 0;
                int n = 0;

                arrCol = new string[dt.Columns.Count];
              
                ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

                foreach (DataColumn colFile in dt.Columns)
                {
                    items[i] = new ToolStripMenuItem();
                    items[i].Name = colFile.ColumnName;

                    //Using LINQ to insert space between capital letters
                    var val = colFile.ColumnName;
                    val = string.Concat(val.Select((x, y) => (char.IsUpper(x) && y > 0 && (char.IsLower(val[y - 1]) || (y < val.Count() - 1 && char.IsLower(val[y + 1])))) ? " " + x : x.ToString()));

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
                tslSearchData.Text = tsddbSearch.DropDownItems[1].Text;
                tstbSearchField.Text = tsddbSearch.DropDownItems[1].Name;
            }
            FileAccess();
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
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = true;
            }
        }

        private void LoadDeptDDL()
        {
            cboDepartments.DataSource = null;
            DataTable dt = new DataTable();
            dt = PSSClass.ServiceDepartments.DepartmentsDDL();
            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SendKeys.Send("{F12}");
                return;
            }
            cboDepartments.DataSource = dt;
            cboDepartments.DisplayMember = "DepartmentName";
            cboDepartments.ValueMember = "DepartmentID";
        }

        private void LoadGLDDL()
        {
            DataSet ds = new DataSet();

            cboGLCodes.DataSource = null;
            ds = PSSClass.ACCPAC.GLCodesDDL();
            if (ds == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SendKeys.Send("{F12}");
                return;
            }
            cboGLCodes.DataSource = ds.Tables[0];
            cboGLCodes.DisplayMember = "GLCode";
            cboGLCodes.ValueMember = "AcctDesc";
            ds.Dispose();
            
            ds = new DataSet();

            cboGLDesc.DataSource = null;
            ds = PSSClass.ACCPAC.GLDescDDL();
            if (ds == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SendKeys.Send("{F12}");
                return;
            }
            cboGLDesc.DataSource = ds.Tables[0];
            cboGLDesc.DisplayMember = "AcctDesc";
            cboGLDesc.ValueMember = "GLCode";
            ds.Dispose();
        }

        private void LoadSvcCategoriesDDL()
        {
            cboSvcCategories.DataSource = null;
            DataTable dt = new DataTable();
            //dt = PSSClass.ServiceCodes.SCPopulateDDL("Select SvcCatID,SvcCatName from ServiceCodesCategories where SvcCatStatus='Active' order by SvcCatOrder");
            dt = PSSClass.ServiceCodes.SCDDLSvcCat();
            
            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                SendKeys.Send("{F12}");
                return;
            }
            
            cboSvcCategories.DataSource = dt;
            cboSvcCategories.DisplayMember = "SvcCatName";
            cboSvcCategories.ValueMember = "SvcCatID";
            cboSvcCategories.SelectedValue = 1;
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dtpDateTill.Format = DateTimePickerFormat.Custom;
            dtpDateTill.CustomFormat = "MMM dd, yyyy";
        }


        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvFile.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["Duration"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DepartmentCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["GLCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SCStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["Prepayment"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["OtherCost"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvFile.Columns["AltCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvFile.Columns["USPCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvFile.Columns["ServiceDesc"].HeaderText = "DESCRIPTION";
            dgvFile.Columns["Duration"].HeaderText = "DURATION (Days)";
            dgvFile.Columns["DepartmentCode"].HeaderText = "DEPARTMENT";
            dgvFile.Columns["GLCode"].HeaderText = "GL CODE";
            dgvFile.Columns["SCStatus"].HeaderText = "STATUS";
            dgvFile.Columns["Prepayment"].HeaderText = "PREPAYMENT ITEM";
            dgvFile.Columns["OtherCost"].HeaderText = "OTHER COST";
            dgvFile.Columns["TestService"].HeaderText = "TEST SERVICE";
            dgvFile.Columns["StabilityService"].HeaderText = "STABILITY SERVICE";
            dgvFile.Columns["ResellService"].HeaderText = "STERIKIT SERVICE";
            //dgvFile.Columns["AltCode"].HeaderText = "ALT CODE";
            //dgvFile.Columns["AltDesc"].HeaderText = "ALT DESCRIPTION";
            //dgvFile.Columns["USPCode"].HeaderText = "USP CODE";
            dgvFile.Columns["Category"].HeaderText = "SERVICE CATEGORY";


            //NEW FIELDS
            dgvFile.Columns["OldServDesc"].HeaderText = "OLD DESCRIPTION";
            dgvFile.Columns["OldSCDescExpDate"].HeaderText = "EXPIRATION DATE";
            //END OF NEW FIELDS


            //dgvFile.Columns["Method"].HeaderText = "METHOD";
            dgvFile.Columns["ServiceCode"].Width = 80;            
            dgvFile.Columns["ServiceDesc"].Width = 380;
            dgvFile.Columns["Duration"].Width = 90;            
            dgvFile.Columns["DepartmentCode"].Width = 110;
            dgvFile.Columns["GLCode"].Width = 90;
            dgvFile.Columns["SCStatus"].Width = 80;
            dgvFile.Columns["Prepayment"].Width = 90;
            dgvFile.Columns["OtherCost"].Width = 80;
            dgvFile.Columns["TestService"].Width = 80;
            dgvFile.Columns["StabilityService"].Width = 80;
            dgvFile.Columns["ResellService"].Width = 80;
            //dgvFile.Columns["AltCode"].Width = 70;
            //dgvFile.Columns["AltDesc"].Width = 300;
            //dgvFile.Columns["USPCode"].Width = 70;
            dgvFile.Columns["Category"].Width = 170;
            //dgvFile.Columns["Method"].Width = 170;


            ////NEW FIELDS
            dgvFile.Columns["OldServDesc"].Width = 380;
            dgvFile.Columns["OldSCDescExpDate"].Width = 80;
            ////END OF NEW FIELDS

            dgvFile.Columns["AltCode"].Visible = false;
            dgvFile.Columns["AltDesc"].Visible = false;
            dgvFile.Columns["USPCode"].Visible = false;
            dgvFile.Columns["Method"].Visible = false;

            dgvFile.Columns["ServiceCode"].CellTemplate.ValueType = typeof(int);
        }

        private void ServiceCodes_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "ServiceCodes");

            LoadGLDDL();
            LoadDeptDDL();
            LoadSvcCategoriesDDL();

            LoadRecords(0);
            BuildPrintItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();
            chkShowInactive.Visible = true;
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
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
            ToolStripMenuItem[] items = new ToolStripMenuItem[5];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "ServiceCode";
            items[0].Text = "Sorted by Service Code";
            items[0].Click += new EventHandler(PrintSC);

            items[1] = new ToolStripMenuItem();
            items[1].Name = "ServiceDesc";
            items[1].Text = "Sorted by Service Description";
            items[1].Click += new EventHandler(PrintSCDesc);

            //items[2] = new ToolStripMenuItem();
            //items[2].Name = "ServiceDept";
            //items[2].Text = "Grouped by Department";
            //items[2].Click += new EventHandler(PrintSCDept);

            items[2] = new ToolStripMenuItem();
            items[2].Name = "ServiceDept";
            items[2].Text = "Grouped by Duration";
            items[2].Click += new EventHandler(PrintSCDuration);

            items[3] = new ToolStripMenuItem();
            items[3].Name = "ServiceGLCode";
            items[3].Text = "Grouped by GL Code";
            items[3].Click += new EventHandler(PrintSCGLCode);

            items[4] = new ToolStripMenuItem();
            items[4].Name = "ServiceInactive";
            items[4].Text = "Inactive Service Codes";
            items[4].Click += new EventHandler(PrintSCInactive);

            //items[5] = new ToolStripMenuItem();
            //items[5].Name = "AuditTrail";
            //items[5].Text = "Audit Trail";
            //items[5].Click += new EventHandler(PrintAudit);

            tsddbPrint.DropDownItems.AddRange(items);
        }


        private void PrintSCDesc(object sender, EventArgs e)
        {
            RptSC rptSC = new RptSC();
            rptSC.WindowState = FormWindowState.Maximized;
            rptSC.rptName = "SCDesc";
            rptSC.rptLabel = "SERVICE CODES REFERENCE LIST SORTED BY DESCRIPTION";
            rptSC.Show();
        }

        private void PrintSC(object sender, EventArgs e)
        {
            RptSC rptSC = new RptSC();
            rptSC.WindowState = FormWindowState.Maximized;
            rptSC.rptName = "SCRef";
            rptSC.rptLabel = "SERVICE CODES REFERENCE LIST SORTED BY CODE";
            rptSC.Show();
        }

        private void PrintSCDept(object sender, EventArgs e)
        {
            RptSC rptSC = new RptSC();
            rptSC.WindowState = FormWindowState.Maximized;
            rptSC.rptName = "SCDept";
            rptSC.rptLabel = "SERVICE CODES REFERENCE LIST GROUPED BY DEPARTMENT";
            rptSC.Show();
        }

        private void PrintSCDuration(object sender, EventArgs e)
        {
            RptSC rptSC = new RptSC();
            rptSC.WindowState = FormWindowState.Maximized;
            rptSC.rptName = "SCDuration";
            rptSC.rptLabel = "SERVICE CODES REFERENCE LIST GROUPED BY DURATION";
            rptSC.Show();
        }

        private void PrintSCGLCode(object sender, EventArgs e)
        {
            RptSC rptSC = new RptSC();
            rptSC.WindowState = FormWindowState.Maximized;
            rptSC.rptName = "SCGLCode";
            rptSC.rptLabel = "SERVICE CODES REFERENCE LIST GROUPED BY GL CODE";
            rptSC.Show();
        }

        private void PrintSCInactive(object sender, EventArgs e)
        {
            RptSC rptSC = new RptSC();
            rptSC.WindowState = FormWindowState.Maximized;
            rptSC.rptName = "SCInactive";
            rptSC.rptLabel = "SERVICE CODES REFERENCE LIST - INACTIVE";
            rptSC.Show();
        }

        private void PrintAudit(object sender, EventArgs e)
        {
            RptSC rptSC = new RptSC();
            rptSC.WindowState = FormWindowState.Maximized;
            rptSC.rptName = "AuditTrail";
            rptSC.rptLabel = "AUDIT TRAIL";
            rptSC.rptFileName = "SERVICE CODES MASTER FILE";
            rptSC.Show();
        }

        private void AddClickHandler(object sender, EventArgs e)
        {
            AddRecord();
        }

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false; tsbRefresh.Enabled = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            txtCode.Focus(); txtCode.ReadOnly = false;
            chkInactive.Checked = false; chkInactive.Enabled = false;
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
            LoadData();
            OpenControls(this.pnlRecord, true);
            txtCode.ReadOnly = true; btnClose.Visible = false; txtDesc.Focus(); chkInactive.Enabled = true; tsbRefresh.Enabled = false;
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

                sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(txtCode.Text));
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelSC";

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
                dgvFile.Refresh();
                pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
                AddEditMode(false);
                LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
                ClearControls(this.pnlRecord);
                nMode = 0;
            }
        }


        private void SaveClickHandler(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void SaveRecord()
        {
            if (txtCode.Text.Trim() == "")
            {
                MessageBox.Show("Please enter service code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }

            if (PSSClass.DataEntry.MatchingRecord("ServiceCode", "ServiceCode", "ServiceCodes", txtCode.Text, nMode,Convert.ToInt16(txtCode.Text), " AND Active = 1") == true)
            {
                MessageBox.Show("Matching Service Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }

            if (txtDesc.Text.Trim() == "")
            {
                MessageBox.Show("Please enter service description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDesc.Focus();
                return;
            }

            if (PSSClass.DataEntry.MatchingRecord("ServiceCode", "ServiceDesc", "ServiceCodes", txtDesc.Text, nMode, Convert.ToInt16(txtCode.Text), " AND Active = 1") == true)
            {
                MessageBox.Show("Matching record found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDesc.Focus();
                return;
            }

            if (txtDuration.Text.Trim() == "")
            {
                txtDuration.Text = "0";
            }

            if (cboGLCodes.Text == "")
            {
                MessageBox.Show("Please select GL code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboGLCodes.Focus();
                return;
            }
            if (cboSvcCategories.Text == "")
            {
                MessageBox.Show("Please select Service Category.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboSvcCategories.Focus();
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

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@SC",Convert.ToInt16(txtCode.Text) );
            sqlcmd.Parameters.AddWithValue("@SCDesc", txtDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Duration", Convert.ToInt16(txtDuration.Text));
            sqlcmd.Parameters.AddWithValue("@GLCode",cboGLCodes.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Prepayment", HandleCheckBoxSave(cbPrepayment));
            sqlcmd.Parameters.AddWithValue("@OtherCost", HandleCheckBoxSave(cbOtherCost));
            sqlcmd.Parameters.AddWithValue("@Test", HandleCheckBoxSave(cbTest));
            sqlcmd.Parameters.AddWithValue("@Stability", HandleCheckBoxSave(cbStability));
            sqlcmd.Parameters.AddWithValue("@Resell", HandleCheckBoxSave(cbResell));
            sqlcmd.Parameters.AddWithValue("@AltDesc", txtAltDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Method", txtMethod.Text.Trim());
            string strSvcCat = cboSvcCategories.SelectedValue.ToString().Trim();           
            sqlcmd.Parameters.AddWithValue("@SvcCatID", Convert.ToInt32(strSvcCat));

            ////NEW CONTROLS ADD SC PREVIOUS DESCRIPTION AND DATE WHEN IT HAS BEEN CHANGED                
            if (txtOldName.Text.Trim().Length > 0 && txtOldName.Text.Trim().Length < 150)
            {
                string strTemp = txtOldName.Text.Trim();
                sqlcmd.Parameters.AddWithValue("@OldSCDesc", strTemp);
                sqlcmd.Parameters.AddWithValue("@OldSCDescExpDate", dtpDateTill.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@OldSCDesc", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@OldSCDescExpDate", DBNull.Value);
            }
            ////END NEW CONTROLS

            if (chkInactive.Checked == true)
                sqlcmd.Parameters.AddWithValue("@Status", 0);
            else
                sqlcmd.Parameters.AddWithValue("@Status", 1);

            if (cboDepartments.SelectedValue != null)
                sqlcmd.Parameters.AddWithValue("@DeptID", cboDepartments.SelectedValue);
            else
                sqlcmd.Parameters.AddWithValue("@DeptID", DBNull.Value);

            if (txtAltCode.Text.Trim() != "")
                sqlcmd.Parameters.AddWithValue("@AltCode", Convert.ToInt16(txtAltCode.Text));
            else
                sqlcmd.Parameters.AddWithValue("@AltCode", DBNull.Value);

            if (txtUSP.Text.Trim() != "")
                sqlcmd.Parameters.AddWithValue("@USP", Convert.ToInt16(txtUSP.Text));
            else
                sqlcmd.Parameters.AddWithValue("@USP", DBNull.Value);

            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID); 
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditSC";

            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                MessageBox.Show("Problem encountered: " + ex.Message + Environment.NewLine + "Record is not saved!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            dgvFile.Refresh();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false); 
            LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
            PSSClass.General.FindRecord("ServiceCode", txtCode.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            nMode = 0;
        }

        private int HandleCheckBoxSave(CheckBox checkBox)
        {
            if (checkBox.Checked == true)
            {
                return 1;
            }
            return 0;
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
            AddEditMode(false);
            LoadRecords(Convert.ToInt16(chkShowInactive.CheckState));
            bsFile.Filter = "ServiceCode<>0";
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            nMode = 0;
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData();
                btnClose.Visible = true;
            }
        }

        private void LoadData()
        {
            //ClearControls(this.pnlRecord);
            //ClearControls(this.pnlIngredion);

            OpenControls(this.pnlRecord, false);
            OpenControls(this.pnlIngredion, false);

            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtCode.Text = dgvFile.CurrentRow.Cells["ServiceCode"].Value.ToString();
            txtDesc.Text = dgvFile.CurrentRow.Cells["ServiceDesc"].Value.ToString();



            //NEW FIELDS
            //Uncomment next 1 line
            txtOldName.Text = dgvFile.CurrentRow.Cells["OldServDesc"].Value.ToString();
            
            dtpDateTill.Format = DateTimePickerFormat.Custom;
            if (string.IsNullOrEmpty(txtOldName.Text.Trim()))
            {
                dtpDateTill.CustomFormat = " ";
            }
            else
            {
                dtpDateTill.CustomFormat = "MMM dd, yyyy";
                //Uncomment next 1 line
                dtpDateTill.Value = Convert.ToDateTime(dgvFile.CurrentRow.Cells["OldSCDescExpDate"].Value);
            }
            
            //END OF NEW FIELDS



            txtDuration.Text = dgvFile.CurrentRow.Cells["Duration"].Value.ToString();
            cboDepartments.Text = LoadDeptName();
            cboGLCodes.Text = dgvFile.CurrentRow.Cells["GLCode"].Value.ToString();
            cboGLDesc.Text = PSSClass.ACCPAC.GLDesc(cboGLCodes.Text);
            cbPrepayment.Checked = HandleCheckBoxLoad("Prepayment");
            cbOtherCost.Checked = HandleCheckBoxLoad("OtherCost");
            cbTest.Checked = HandleCheckBoxLoad("TestService");
            cbStability.Checked = HandleCheckBoxLoad("StabilityService");
            cbResell.Checked = HandleCheckBoxLoad("ResellService");
            txtAltCode.Text = dgvFile.CurrentRow.Cells["AltCode"].Value.ToString();
            txtAltDesc.Text = dgvFile.CurrentRow.Cells["AltDesc"].Value.ToString();
            txtUSP.Text = dgvFile.CurrentRow.Cells["USPCode"].Value.ToString();
            txtMethod.Text = dgvFile.CurrentRow.Cells["Method"].Value.ToString();
            cboSvcCategories.Text=dgvFile.CurrentRow.Cells["Category"].Value.ToString();

            if (dgvFile.CurrentRow.Cells["SCStatus"].Value.ToString() == "Inactive")
                chkInactive.Checked = true;
            else
                chkInactive.Checked = false;
        }

        private bool HandleCheckBoxLoad(string columnName)
        {
            if (dgvFile.CurrentRow.Cells[columnName].Value.ToString() == "True")
            {
                return true;
            }
            return false;
        }

        private string LoadDeptName()
        {
            string DeptName = "";
            using (SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection())
            {
                SqlCommand sqlcmd = new SqlCommand("SELECT DepartmentName FROM ServiceDepartments WHERE DepartmentCode LIKE @Code", sqlcnn);
                sqlcmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = dgvFile.CurrentRow.Cells["DepartmentCode"].Value.ToString();
                try
                {
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                    while (sqldr.Read())
                    {
                        DeptName = String.Format("{0}", sqldr[0]);
                    }
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                }
                catch (Exception ex)
                {
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return DeptName;
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
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

        private void cboGLCodes_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cboGLDesc.Text = cboGLCodes.SelectedValue.ToString();
        }

        private void cboGLDesc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cboGLCodes.Text = cboGLDesc.SelectedValue.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task(s)?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
            AddEditMode(false);
            FileAccess();
        }

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
                bsFile.Filter = "ServiceCode<>0";
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
            tsbRefresh.Enabled = false;
        }

        private void chkShowInactiveClickHandler(object sender, EventArgs e)
        {
            if (chkShowInactive.Checked)
            {
                LoadRecords(1);
            }
            else
            {
                LoadRecords(0);
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

        private void ServiceCodes_KeyDown(object sender, KeyEventArgs e)
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

        private void txtCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtAltCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtUSP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr += 1;
            if (nCtr > 1)
            {
                DataTable dt = new DataTable();
                dt = PSSClass.ServiceCodes.SCMaster(2);
                PSSClass.DataEntry.DGVSearch(tstbSearchField.Text, tstbSearch.Text, dt, bsFile);
                nCtr = 0;
                tstbSearch.Text = "";
                timer1.Enabled = false;
                nSw = 0;
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
    }
}
