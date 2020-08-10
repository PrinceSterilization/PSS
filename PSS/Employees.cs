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
    public partial class Employees : PSS.TemplateForm
    {
        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";

        protected int nEmp = 0;
        protected int nOther = 0;
        protected DataTable dtStates = new DataTable();
        private DataTable dtWLAppr = new DataTable();
        private DataTable dtApprovers = new DataTable();

        public Employees()
        {
            InitializeComponent();

            //BuildPrintItems();
            //BuildSearchItems();

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
            tstbSearch.KeyPress += new KeyPressEventHandler(SearchKeyPressHandler);
            tsbSearch.Click += new EventHandler(SearchOKClickHandler);
            tsbFilter.Click += new EventHandler(SearchFilterClickHandler);
            tsbRefresh.Click += new EventHandler(RefreshClickHandler);
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            chkShowInactive.Click += new EventHandler(chkShowInactiveClickHandler);

            LoadHMO();
            LoadEthnicities();
            LoadStates();
            LoadDepartments();
            LoadDischargeTypes();
            LoadEmpTypes();
            LoadRelations();
            LoadLaborGrades();
            LoadEducation();
            LoadWLApprovers();
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

        private void LoadRecords(int cSKey, int cDispType)
        {
            nMode = 0;
            DataTable dt = new DataTable();
            dt = PSSClass.Employees.EmployeesMaster(cSKey, cDispType);
            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;

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
                    val = string.Concat(val.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

                    items[i].Text = val;
                    items[i].Click += new EventHandler(SearchItemClickHandler);
                    arrCol[i] = colFile.DataType.ToString();
                    cklColumns.Items.Add(val);
                    //}
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

        private void SearchItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            tstbSearchField.Text = clickedItem.Name;
            tstbSearch.SelectAll();
            tstbSearch.Focus();
            nIndex = tsddbSearch.DropDownItems.IndexOf(clickedItem);
            tslSearchData.Text = clickedItem.Text;
        }

        private void chkShowInactiveClickHandler(object sender, EventArgs e)
        {
            if (chkShowInactive.Checked)
            {
                LoadRecords(1,2);
            }
            else
            {
                LoadRecords(1,1);
            }
        }

        private void LoadHMO()
        {
            DataSet ds = new DataSet();

            cboHMO.DataSource = null;
            ds = PSSClass.Employees.HMODDL();
            if (ds == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            cboHMO.DataSource = ds.Tables[0];
            cboHMO.DisplayMember = "HMODesc";
            cboHMO.ValueMember = "HMOCode";
            ds.Dispose();
        }

        private void LoadStates()
        {
            dtStates = PSSClass.States.StatesDDL();            

            cboStates.DataSource = null;
            if (dtStates == null)
            {
                return;
            }
            cboStates.DataSource = dtStates;
            cboStates.DisplayMember = "State";
            cboStates.ValueMember = "State";
        }

        private void LoadDepartments()
        {
            cboDepartments.DataSource = null;
            DataTable dt = new DataTable();
            dt = PSSClass.Departments.DepartmentsDDL();
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            cboDepartments.DataSource = dt;
            cboDepartments.DisplayMember = "DepartmentCode";
            cboDepartments.ValueMember = "DepartmentID";
        }

        private void LoadWLApprovers()
        {
            cboWLApprovers.DataSource = null;
            dtApprovers = new DataTable();
            dtApprovers = PSSClass.Employees.WLApproversDDL();
            if (dtApprovers == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            cboWLApprovers.DataSource = dtApprovers;
            cboWLApprovers.DisplayMember = "EmpName";
            cboWLApprovers.ValueMember = "EmployeeID";
        }

        private void LoadEthnicities()
        {
            DataSet ds = new DataSet();
            cboStates.DataSource = null;
            ds = PSSClass.Employees.EthnicityDDL();
            if (ds == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            cboEthnicities.DataSource = ds.Tables[0];
            cboEthnicities.DisplayMember = "EthnicityDesc";
            cboEthnicities.ValueMember = "EthnicityID";
            ds.Dispose();
        }

        private void LoadDischargeTypes()
        {
            DataSet ds = new DataSet();
            cboDisTypes.DataSource = null;
            ds = PSSClass.Employees.DischargeDDL();
            if (ds == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            DataRow newRow = ds.Tables[0].NewRow();

            newRow["DischargeDesc"] = " N/A ";
            newRow["DischargeID"] = 0;

            ds.Tables[0].Rows.Add(newRow);
            DataView dv = ds.Tables[0].DefaultView;
            dv.Sort = "DischargeDesc";
            DataTable dt = dv.ToTable();

            cboDisTypes.DataSource = dt;
            cboDisTypes.DisplayMember = "DischargeDesc";
            cboDisTypes.ValueMember = "DischargeID";
            ds.Dispose();
        }

        private void LoadEmpTypes()
        {
            DataSet ds = new DataSet();
            cboEmpTypes.DataSource = null;
            ds = PSSClass.Employees.EmpTypesDDL();
            if (ds == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            cboEmpTypes.DataSource = ds.Tables[0];
            cboEmpTypes.DisplayMember = "EmpTypeDesc";
            cboEmpTypes.ValueMember = "EmpTypeID";
            ds.Dispose();
        }

        private void LoadRelations()
        {
            DataSet ds = new DataSet();
            cboRelations.DataSource = null;
            ds = PSSClass.Employees.RelationsDDL();
            if (ds == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            cboRelations.DataSource = ds.Tables[0];
            cboRelations.DisplayMember = "RelationDesc";
            cboRelations.ValueMember = "RelationID";
            ds.Dispose();
        }

        private void LoadLaborGrades()
        {
            DataSet ds = new DataSet();
            cboLaborGrades.DataSource = null;
            ds = PSSClass.Employees.LaborGradesDDL();
            if (ds == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            cboLaborGrades.DataSource = ds.Tables[0];
            cboLaborGrades.DisplayMember = "LaborGradeDesc";
            cboLaborGrades.ValueMember = "LaborGradeID";
            ds.Dispose();
        }

        private void LoadEducation()
        {
            DataSet ds = new DataSet();
            dgvEducation.DataSource = null;
            ds = PSSClass.Employees.EducationList();
            if (ds == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            dgvEducation.DataSource = ds.Tables[0];
            dgvEducation.Columns[0].HeaderText = "LEVEL";
            dgvEducation.Columns[2].HeaderText = "ATTAINED";
            dgvEducation.Columns[0].Width = 60;
            dgvEducation.Columns[1].Visible = false;
            dgvEducation.Columns[2].Width = 80;

            //DataGridViewCheckBoxColumn btnColumn = new DataGridViewCheckBoxColumn();
            //btnColumn.HeaderText = "ATTAINED";
            //btnColumn.Width = 80;
            //dgvEducation.Columns.AddRange(btnColumn);
            StandardDGVSetting(dgvEducation);
            ds.Dispose();
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData();
                btnClose.Visible = true;
            }
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns[0].HeaderText = "EMP. CODE";
            dgvFile.Columns[1].HeaderText = "LAST NAME";
            dgvFile.Columns[2].HeaderText = "FIRST NAME";
            dgvFile.Columns[3].HeaderText = "M.I.";
            dgvFile.Columns[4].HeaderText = "NICKNAME";
            dgvFile.Columns[5].HeaderText = "HOME PHONE NO.";
            dgvFile.Columns[6].HeaderText = "MOBILE PHONE NO.";
            dgvFile.Columns[7].HeaderText = "ID";
            dgvFile.Columns[8].HeaderText = "E-MAIL ADDRESS";
            dgvFile.Columns[0].Width = 100;
            dgvFile.Columns[1].Width = 200;
            dgvFile.Columns[2].Width = 200;
            dgvFile.Columns[3].Width = 50;
            dgvFile.Columns[4].Width = 100;
            dgvFile.Columns[5].Width = 150;
            dgvFile.Columns[6].Width = 150;
            dgvFile.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns[7].Visible = false;
            dgvFile.Columns[8].Width = 350;
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            GetPersonalData();
            GetEmploymentData();
            GetOtherData();
            tbcEmployee.SelectedIndex = 0; 
            ClearControls(tabOtherData);
            OpenControls(tabEmployment, true);
            OpenControls(tabOtherData, true);
        }

        private void GetPersonalData()
        {
            ClearControls(pnlRecord);
            txtID.Text = dgvFile.CurrentRow.Cells[7].Value.ToString();
            DataTable dt = new DataTable();
            dt = PSSClass.Employees.PersonalData(Convert.ToInt16(txtID.Text));
            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            txtCode.Text = dt.Rows[0]["EmployeeCode"].ToString();
            txtLastName.Text = dt.Rows[0]["LastName"].ToString();
            txtFirstName.Text = dt.Rows[0]["FirstName"].ToString();
            txtMI.Text = dt.Rows[0]["MidInitial"].ToString();
            txtInitials.Text = dt.Rows[0]["Initials"].ToString();
            txtNickName.Text = dt.Rows[0]["NickName"].ToString();
            try
            {
                dtpBirthDate.Value = Convert.ToDateTime(dt.Rows[0]["BirthDate"]);
                dtpBirthDate.Format = DateTimePickerFormat.Custom;
                dtpBirthDate.CustomFormat = "MM/dd/yyyy";
                dtpBirthDate.Checked = true;
            }
            catch 
            {
                dtpBirthDate.Format = DateTimePickerFormat.Custom;
                dtpBirthDate.CustomFormat = " ";
                dtpBirthDate.Checked = false;
            }
            if (dt.Rows[0]["Gender"].ToString() == "M")
                rdoMale.Checked = true;
            else
                rdoFemale.Checked = true;
            txtSSSNo.Text = dt.Rows[0]["SSSNo"].ToString();
            txtPlateNo.Text = dt.Rows[0]["PlateNo"].ToString();
            txtStAddress.Text = dt.Rows[0]["HomeAddress"].ToString();
            txtCityAddress.Text = dt.Rows[0]["CityAddress"].ToString();
            cboStates.Text = dt.Rows[0]["StateCode"].ToString();
            txtZIPCode.Text = dt.Rows[0]["ZipCode"].ToString();
            cboHMO.SelectedValue  = dt.Rows[0]["HMOCode"].ToString();
            cboEthnicities.SelectedValue = dt.Rows[0]["EthnicityID"];
            mskResNo.Text = dt.Rows[0]["HomePhoneNo"].ToString();
            mskMobileNo.Text = dt.Rows[0]["MobilePhoneNo"].ToString();
            txtEMail.Text = dt.Rows[0]["EMailAddress"].ToString();
            txtContactName.Text = dt.Rows[0]["EmergencyContact"].ToString();
            cboRelations.Text= dt.Rows[0]["EmergencyRelation"].ToString();
            mskContactNos.Text = dt.Rows[0]["EmergencyNo"].ToString();
            chkPensioner.Checked = Convert.ToBoolean(dt.Rows[0]["Pensioner"].ToString());
            chkVeteran.Checked = Convert.ToBoolean(dt.Rows[0]["Veteran"].ToString());
            
        }

        private void GetEmploymentData()
        {
            ClearControls(tabEmployment);
            txtID.Text = dgvFile.CurrentRow.Cells[7].Value.ToString();
            DataTable dt = new DataTable();
            dt = PSSClass.Employees.EmploymentData(Convert.ToInt16(txtID.Text));
            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            try
            {
                cboLaborGrades.SelectedValue = dt.Rows[0]["LaborGradeID"];
            }
            catch { }
            try
            {
                dtpDateHired.Value = Convert.ToDateTime(dt.Rows[0]["HireDate"]);
                dtpDateHired.Format = DateTimePickerFormat.Custom;
                dtpDateHired.CustomFormat = "MM/dd/yyyy";
                dtpDateHired.Checked = true;
            }
            catch 
            {
                dtpDateHired.Format = DateTimePickerFormat.Custom;
                dtpDateHired.CustomFormat = " ";
                dtpDateHired.Checked = false;
            }
            try
            {
                cboEmpTypes.SelectedValue = dt.Rows[0]["EmpTypeID"].ToString();
                txtShiftCode.Text = dt.Rows[0]["ShiftCode"].ToString();
                txtTaxCode.Text = dt.Rows[0]["TaxCode"].ToString();
                txtPlantNo.Text = dt.Rows[0]["PlantNo"].ToString();
            }
            catch { }
            try
            {
                dtpFullTime.Value = Convert.ToDateTime(dt.Rows[0]["FullTimeDate"]);
                dtpFullTime.Format = DateTimePickerFormat.Custom;
                dtpFullTime.CustomFormat = "MM/dd/yyyy";
                dtpFullTime.Checked = true;
            }
            catch
            {
                dtpFullTime.Format = DateTimePickerFormat.Custom;
                dtpFullTime.CustomFormat = " ";
                dtpFullTime.Checked = false;
            }
            try
            {
                dtpPartTime.Value = Convert.ToDateTime(dt.Rows[0]["PartTimeDate"]);
                dtpPartTime.Format = DateTimePickerFormat.Custom;
                dtpPartTime.CustomFormat = "MM/dd/yyyy";
                dtpPartTime.Checked = true;
            }
            catch
            {
                dtpPartTime.Format = DateTimePickerFormat.Custom;
                dtpPartTime.CustomFormat = " ";
                dtpPartTime.Checked = false;
            }
            try
            {
                if (dt.Rows[0]["Active"].ToString() == "True")
                {
                    rdoActive.Checked = true;
                }
                else
                {
                    rdoInactive.Checked = true;
                }
                txtJobTitle.Text = dt.Rows[0]["JobTitle"].ToString();
                cboDepartments.SelectedValue = dt.Rows[0]["DepartmentID"];
                cboEmpLevels.SelectedIndex = Convert.ToInt16(dt.Rows[0]["EmpLevelCode"]);
                txtCoEMail.Text = dt.Rows[0]["CompanyEMail"].ToString();
                txtPhoneExt.Text = dt.Rows[0]["PhoneExtNo"].ToString();
                txtLoginName.Text = dt.Rows[0]["LoginName"].ToString();
                cboDisTypes.SelectedValue = dt.Rows[0]["DischargeID"];
            }
            catch { }
            try
            {
                chkExempt.Checked = Convert.ToBoolean(dt.Rows[0]["Exempt"]);
            }
            catch { }
            try
            {
                chkWLApprover.Checked = Convert.ToBoolean(dt.Rows[0]["WLApprover"]);
            }
            catch { }
            try
            {
                dtpLastWork.Value = Convert.ToDateTime(dt.Rows[0]["LastWorkdate"]);
                dtpLastWork.Format = DateTimePickerFormat.Custom;
                dtpLastWork.CustomFormat = "MM/dd/yyyy";
                dtpLastWork.Checked = true;
            }
            catch
            {
                dtpLastWork.Format = DateTimePickerFormat.Custom;
                dtpLastWork.CustomFormat = " ";
                dtpLastWork.Checked = false;
            }
            try
            {
                txtESignPassword.Text = dt.Rows[0]["ESignPassword"].ToString();
                if (dt.Rows[0]["Analyst"].ToString() == "True")
                    chkAnalyst.Checked = true;
                if (dt.Rows[0]["StudyDirector"].ToString() == "True")
                    chkSD.Checked = true;
                txtNotes.Text = dt.Rows[0]["EmpNotes"].ToString();
            }
            catch { }
            //picPhoto.Image = null;
            //picPhoto.Invalidate();
            try
            {
                picPhoto.Load(@"\\PSAPP01\IT Files\PTS\Images\hr\" + txtLoginName.Text + ".jpg");
            }
            catch
            {
                picPhoto.Load(@"\\PSAPP01\IT Files\PTS\Images\" + "PSS Logo.png");
            }
        }

        private void GetOtherData()
        {
            txtID.Text = dgvFile.CurrentRow.Cells[7].Value.ToString();
            DataTable dt = new DataTable();
            dt = PSSClass.Employees.EducationData(Convert.ToInt16(txtID.Text));
            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            for (int i = 0; i < dgvEducation.Rows.Count; i++)
            {
                DataRow[] dr = dt.Select("EducationID=" + dgvEducation.Rows[i].Cells[1].Value.ToString());
                if (dr.Length != 0)
                    dgvEducation.Rows[i].Cells[2].Value = 1;
                else
                    dgvEducation.Rows[i].Cells[2].Value = 0;
            }
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

        private void SearchKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SearchFilterClickHandler(null, null);
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            if (chkShowInactive.Checked == true)
                LoadRecords(1, 2);
            else
                LoadRecords(1, 1);
            bsFile.Filter = "EmployeeID <> 0";
            tsbRefresh.Enabled = false;
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

        private void SearchOKClickHandler(object sender, EventArgs e)
        {
            try
            {
                bsFile.Filter = "EmployeeID<>0";
                PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                dgvFile.Select();
                if (pnlRecord.Visible == true)
                    LoadData();
            }
            catch { }
        }

        private void dgvKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void AddClickHandler(object sender, EventArgs e)
        {
            AddRecord();
        }

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            btnEditEmp.Visible = false;

            cboDisTypes.SelectedIndex = 0;
            
            dtpBirthDate.Format = DateTimePickerFormat.Custom;
            dtpBirthDate.CustomFormat = " ";
            dtpBirthDate.Checked = false;

            dtpDateHired.Format = DateTimePickerFormat.Custom;
            dtpDateHired.CustomFormat = " ";
            dtpDateHired.Checked = false;

            dtpPartTime.Format = DateTimePickerFormat.Custom;
            dtpPartTime.CustomFormat = " ";
            dtpPartTime.Checked = false;

            dtpFullTime.Format = DateTimePickerFormat.Custom;
            dtpFullTime.CustomFormat = " ";
            dtpFullTime.Checked = false;

            dtpLastWork.Format = DateTimePickerFormat.Custom;
            dtpLastWork.CustomFormat = " ";
            dtpLastWork.Checked = false;

            cboLaborGrades.SelectedIndex = 0;
            cboEmpTypes.SelectedIndex = 0;
            cboDepartments.SelectedIndex = 0;
            cboDisTypes.SelectedIndex = 0;
            dtpLastWork.Checked = false;
            rdoActive.Checked = true;
            txtCode.Focus();
        }

        private void EditClickHandler(object sender, EventArgs e)
        {
            EditRecord();
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            LoadData();
            txtCode.Focus(); btnClose.Visible = false; btnEditEmp.Visible = false;
        }

        private void SaveClickHandler(object sender, EventArgs e)
        {
            SavePersonalData();
        }

        private void SavePersonalData()
        {
            string strPhone = "";
            if (txtCode.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Employee Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }
            if (txtFirstName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter First Name of employee.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtFirstName.Focus();
                return;
            }
            if (txtLastName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Last Name of employee.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLastName.Focus();
                return;
            }

            if (nMode == 1)
                txtID.Text = PSSClass.DataEntry.NewEmpID("EmpPersonal", "EmployeeID").ToString();

            if (PSSClass.DataEntry.MatchingRecord("EmployeeID", "EmployeeCode", "EmpPersonal", txtCode.Text, nMode, Convert.ToInt16(txtID.Text), "") == true)
            {
                MessageBox.Show("Matching employee number found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }

            if (nMode == 1)
            {
                if (dtpDateHired.Checked == false)
                {
                    MessageBox.Show("Please enter date of hiring.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    dtpDateHired.Focus();
                    return;
                }
                if (rdoActive.Checked == false && rdoInactive.Checked == false)
                {
                    MessageBox.Show("Please select work status.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            if (rdoActive.Checked == false && rdoInactive.Checked == false)
            {
                MessageBox.Show("Please select work status.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                sqlcnn.Dispose();
                MessageBox.Show("Connection problem encountered. Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.TinyInt));
            sqlcmd.Parameters["@nMode"].Value = nMode;

            sqlcmd.Parameters.Add(new SqlParameter("@EmpID", SqlDbType.Int));
            sqlcmd.Parameters["@EmpID"].Value = Convert.ToInt16(txtID.Text);

            sqlcmd.Parameters.Add(new SqlParameter("@EmpCode", SqlDbType.NVarChar));
            if (txtCode.Text.Trim() == "")
                sqlcmd.Parameters["@EmpCode"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@EmpCode"].Value = txtCode.Text.ToUpper();

            sqlcmd.Parameters.Add(new SqlParameter("@LName", SqlDbType.NVarChar));
            if (txtLastName.Text.Trim() == "")
                sqlcmd.Parameters["@LName"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@LName"].Value = txtLastName.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@FName", SqlDbType.NVarChar));
            if (txtFirstName.Text.Trim() == "")
                sqlcmd.Parameters["@FName"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@FName"].Value = txtFirstName.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@MI", SqlDbType.NChar));
            if (txtMI.Text.Trim() == "")
                sqlcmd.Parameters["@MI"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@MI"].Value = txtMI.Text.ToUpper();

            sqlcmd.Parameters.Add(new SqlParameter("@Init", SqlDbType.NVarChar));
            if (txtInitials.Text.Trim() == "")
                sqlcmd.Parameters["@Init"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@Init"].Value = txtInitials.Text.ToUpper();

            sqlcmd.Parameters.Add(new SqlParameter("@NName", SqlDbType.NVarChar));
            if (txtNickName.Text.Trim() == "")
                sqlcmd.Parameters["@NName"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@NName"].Value = txtNickName.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@Birthday", SqlDbType.SmallDateTime));
            if (dtpBirthDate.Checked == true)
                sqlcmd.Parameters["@Birthday"].Value = dtpBirthDate.Value;
            else
                sqlcmd.Parameters["@Birthday"].Value = DBNull.Value;

            sqlcmd.Parameters.Add(new SqlParameter("@Sex", SqlDbType.NChar));
            if (rdoMale.Checked == true)
            {
                sqlcmd.Parameters["@Sex"].Value = "M";
            }
            else
            {
                sqlcmd.Parameters["@Sex"].Value = "F";
            }

            sqlcmd.Parameters.Add(new SqlParameter("@HMO", SqlDbType.NVarChar));
            sqlcmd.Parameters["@HMO"].Value = cboHMO.SelectedValue;

            sqlcmd.Parameters.Add(new SqlParameter("@Ethnicity", SqlDbType.TinyInt));
            sqlcmd.Parameters["@Ethnicity"].Value = cboEthnicities.SelectedValue;

            sqlcmd.Parameters.Add(new SqlParameter("@SSS", SqlDbType.NVarChar));
            if (txtSSSNo.Text.Trim() == "")
                sqlcmd.Parameters["@SSS"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@SSS"].Value = txtSSSNo.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@Plate", SqlDbType.NVarChar));
            if (txtPlateNo.Text.Trim() == "")
                sqlcmd.Parameters["@Plate"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@Plate"].Value = txtPlateNo.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@ResAddress", SqlDbType.NVarChar));
            if (txtStAddress.Text.Trim() == "")
                sqlcmd.Parameters["@ResAddress"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@ResAddress"].Value = txtStAddress.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@CityAddress", SqlDbType.NVarChar));
            if (txtCityAddress.Text.Trim() == "")
                sqlcmd.Parameters["@CityAddress"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@CityAddress"].Value = txtCityAddress.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@State", SqlDbType.NVarChar));
            sqlcmd.Parameters["@State"].Value = cboStates.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@Zip", SqlDbType.NVarChar));
            if (txtZIPCode.Text.Trim() == "")
                sqlcmd.Parameters["@Zip"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@Zip"].Value = txtZIPCode.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@ResPhone", SqlDbType.NVarChar));
            if (mskResNo.Text.Trim() == "" || mskResNo.Text == "( ) - ; ( ) -")
                sqlcmd.Parameters["@ResPhone"].Value = DBNull.Value;
            else
            {
                strPhone =  mskResNo.Text.Replace("; ( ) -", "");
                strPhone = strPhone.Replace("( )", "");
                sqlcmd.Parameters["@ResPhone"].Value = strPhone;
            }

            sqlcmd.Parameters.Add(new SqlParameter("@MobPhone", SqlDbType.NVarChar));
            if (mskMobileNo.Text.Trim() == "" || mskMobileNo.Text == "( ) - ; ( ) -")
                sqlcmd.Parameters["@MobPhone"].Value = DBNull.Value;
            else
            {
                strPhone = mskMobileNo.Text.Replace("; ( ) -", "");
                strPhone = strPhone.Replace("( )", "");
                sqlcmd.Parameters["@MobPhone"].Value = strPhone;
            }

            sqlcmd.Parameters.Add(new SqlParameter("@PEMail", SqlDbType.NVarChar));
            if (txtEMail.Text.Trim() == "")
                sqlcmd.Parameters["@PEMail"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@PEMail"].Value = txtEMail.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@EContact", SqlDbType.NVarChar));
            if (txtContactName.Text.Trim() == "")
                sqlcmd.Parameters["@EContact"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@EContact"].Value = txtContactName.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@EContactRel", SqlDbType.NVarChar));
            if (cboRelations.Text.Trim() == "")
                sqlcmd.Parameters["@EContactRel"].Value = DBNull.Value;
            else
                sqlcmd.Parameters["@EContactRel"].Value = cboRelations.Text;

            sqlcmd.Parameters.Add(new SqlParameter("@EContactNo", SqlDbType.NVarChar));
            if (mskContactNos.Text.Trim() == "" || mskContactNos.Text == "( ) - ; ( ) -")
                sqlcmd.Parameters["@EContactNo"].Value = DBNull.Value;
            else
            {
                strPhone = mskContactNos.Text.Replace("; ( ) -", "");
                strPhone = strPhone.Replace("( )", "");
                sqlcmd.Parameters["@EContactNo"].Value = strPhone;
            }

            sqlcmd.Parameters.Add(new SqlParameter("@Vet", SqlDbType.Bit));
            sqlcmd.Parameters["@Vet"].Value = chkVeteran.CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@Pension", SqlDbType.Bit));
            sqlcmd.Parameters["@Pension"].Value = chkPensioner.CheckState;

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID;

            sqlcmd.CommandType = CommandType.StoredProcedure;
            try
            {
                sqlcmd.CommandText = "spAddEditPersonal";
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sqlcmd.Dispose(); sqlcnn.Dispose();
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
            if (nMode == 1)
            {
                SaveEmploymentData();
                SaveOtherData();
            }
            nMode = 0;
            CancelSave();
        }

        private void SaveEmploymentData()
        {
            try
            {
                if (txtTaxCode.Text.Trim() != "")
                {
                    int n = int.Parse(txtTaxCode.Text);
                }
            }
            catch
            {
                MessageBox.Show("Tax code should be numeric.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                if (txtShiftCode.Text.Trim() != "")
                {
                    int n = int.Parse(txtShiftCode.Text);
                }
            }
            catch
            {
                MessageBox.Show("Shift Code should be numeric.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                if (txtPlantNo.Text.Trim() != "")
                {
                    int n = int.Parse(txtPlantNo.Text);
                }
            }
            catch
            {
                MessageBox.Show("Plant No. should be numeric.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered. Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@EmpID", Convert.ToInt16(txtID.Text));
            sqlcmd.Parameters.AddWithValue("@JobDesc", txtJobTitle.Text.Trim());
            if (cboLaborGrades.SelectedValue != null)
                sqlcmd.Parameters.AddWithValue("@LaborGradeID", cboLaborGrades.SelectedValue);
            else
                sqlcmd.Parameters.AddWithValue("@LaborGradeID",DBNull.Value);

            if (dtpDateHired.Checked == true)
                sqlcmd.Parameters.AddWithValue("@DHired", dtpDateHired.Value);
            else
                sqlcmd.Parameters.AddWithValue("@DHired", DBNull.Value);

            if (dtpFullTime.Checked == true)
                sqlcmd.Parameters.AddWithValue("@DFullTime",dtpFullTime.Value);
            else
                sqlcmd.Parameters.AddWithValue("@DFullTime",DBNull.Value);

            if (dtpPartTime.Checked == true)
                sqlcmd.Parameters.AddWithValue("@DPartTime", dtpPartTime.Value);
            else
                sqlcmd.Parameters.AddWithValue("@DPartTime", DBNull.Value);

            if (cboEmpTypes.SelectedValue != null)
                sqlcmd.Parameters.AddWithValue("@EmpTypeID", cboEmpTypes.SelectedValue);
            else
                sqlcmd.Parameters.AddWithValue("@EmpTypeID", DBNull.Value);

            sqlcmd.Parameters.AddWithValue("@EmpLevel", cboEmpLevels.SelectedIndex);

            if (rdoActive.Checked == true)
                sqlcmd.Parameters.AddWithValue("@WorkStatusCode", 1);
            else
                sqlcmd.Parameters.AddWithValue("@WorkStatusCode", 0);

            if (txtTaxCode.Text.Trim() != "")
                sqlcmd.Parameters.AddWithValue("@TaxCode", Convert.ToInt16(txtTaxCode.Text));
            else
                sqlcmd.Parameters.AddWithValue("@TaxCode", DBNull.Value);

            if (txtShiftCode.Text.Trim() != "")
                sqlcmd.Parameters.AddWithValue("@ShiftCode", Convert.ToInt16(txtShiftCode.Text));
            else
                sqlcmd.Parameters.AddWithValue("@ShiftCode", DBNull.Value);

            if (txtPlantNo.Text.Trim() != "")
                sqlcmd.Parameters.AddWithValue("@PlantNo", Convert.ToInt16(txtPlantNo.Text));
            else
                sqlcmd.Parameters.AddWithValue("@PlantNo", DBNull.Value);

            if (cboDepartments.SelectedValue != null)
                sqlcmd.Parameters.AddWithValue("@DeptID", cboDepartments.SelectedValue);
            else
                sqlcmd.Parameters.AddWithValue("@DeptID", DBNull.Value);

            sqlcmd.Parameters.AddWithValue("@CoEMail", txtCoEMail.Text.Trim());

            if (txtPhoneExt.Text.Trim() != "")
                sqlcmd.Parameters.AddWithValue("@PhoneExt", txtPhoneExt.Text);
            else
                sqlcmd.Parameters.AddWithValue("@PhoneExt", DBNull.Value);

            sqlcmd.Parameters.AddWithValue("@Analyst",Convert.ToBoolean(chkAnalyst.CheckState));
            sqlcmd.Parameters.AddWithValue("@SD", Convert.ToBoolean(chkSD.CheckState));
            sqlcmd.Parameters.AddWithValue("@Login", txtLoginName.Text.Trim());
            if (cboDisTypes.SelectedValue != null)
                sqlcmd.Parameters.AddWithValue("@DisID", cboDisTypes.SelectedValue);
            else
                sqlcmd.Parameters.AddWithValue("@DisID", DBNull.Value);

            if (dtpLastWork.Checked == true)
                sqlcmd.Parameters.AddWithValue("@LWorkDate", dtpLastWork.Value);
            else
                sqlcmd.Parameters.AddWithValue("@LWorkDate", DBNull.Value);

            sqlcmd.Parameters.AddWithValue("@EPassword", txtESignPassword.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Exempt", Convert.ToBoolean(chkExempt.CheckState));
            sqlcmd.Parameters.AddWithValue("@WLApprover", Convert.ToBoolean(chkWLApprover.CheckState));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            //try
            //{
                sqlcmd.CommandText = "spAddEditEmployment";
                sqlcmd.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    sqlcmd.Dispose(); sqlcnn.Dispose();
            //    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
            sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
        }

        private void SaveOtherData()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered. Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@EmpID", SqlDbType.Int));
            sqlcmd.Parameters["@EmpID"].Value = Convert.ToInt16(txtID.Text);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            try
            {
                sqlcmd.CommandText = "spDelEmpEducation";
                sqlcmd.ExecuteNonQuery();
            }
            catch {}

            sqlcmd.Parameters.Add(new SqlParameter("@EduID", SqlDbType.SmallInt));
            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.CommandType = CommandType.StoredProcedure;

            for (int i = 0; i < dgvEducation.Rows.Count; i++)
            {
                if (dgvEducation.Rows[i].Cells[2].Value.ToString() == "True")
                {
                    sqlcmd.Parameters["@EmpID"].Value = Convert.ToInt16(txtID.Text);
                    sqlcmd.Parameters["@EduID"].Value = Convert.ToInt16(dgvEducation.Rows[i].Cells[1].Value);
                    sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID;
                    try
                    {
                        sqlcmd.CommandText = "spUpdEducation";
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        sqlcmd.Dispose(); sqlcnn.Dispose();
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
                }
            }
            sqlcmd.Dispose();
            DataTable dtX = dtWLAppr.GetChanges(); //DataRowState.Modified
            if (dtX != null && dtX.Rows.Count > 0)
            {
                string strAppr = "";
                for (int i = 0; i < dtWLAppr.Rows.Count; i++)
                {
                    if (dtWLAppr.Rows[i].RowState.ToString() != "Deleted")
                    {
                        strAppr = strAppr + dtWLAppr.Rows[i]["EmployeeID"].ToString() + ",";
                    }
                }

                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@EmpID", Convert.ToInt16(txtID.Text));
                sqlcmd.Parameters.AddWithValue("@Approvers", strAppr);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    sqlcmd.CommandText = "spUpdEmpApprovers";
                    sqlcmd.ExecuteNonQuery();
                }
                catch { }
                sqlcmd.Dispose();
                dtX.Dispose();
            }
            sqlcnn.Close(); sqlcnn.Dispose();
            btnCancelEmp_Click(null, null);
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
            if (chkShowInactive.Checked == true)
                LoadRecords(1,2);
            else
                LoadRecords(1, 1);
            //pnlRecord.Visible = false; dgvFile.Visible = true; 
            bnFile.Enabled = true; btnEditEmp.Visible = true;
            nMode = 0; nEmp = 0; nOther = 0;
        }

        private void DeleteClickHandler(object sender, EventArgs e)
        {
            DeleteRecord();
        }

        private void DeleteRecord()
        {
            if (pnlRecord.Visible == false)
                LoadData();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                SqlCommand sqlcmd = new SqlCommand();

                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.Add(new SqlParameter("@EmpID", SqlDbType.Int));
                sqlcmd.Parameters["@EmpID"].Value = Convert.ToInt16(txtID.Text);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelPersonal";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            if (chkShowInactive.Checked == true)
                LoadRecords(1, 2);
            else
                LoadRecords(1, 1);
        }

        private void dtpBirthDate_ValueChanged(object sender, EventArgs e)
        {
            dtpBirthDate.Format = DateTimePickerFormat.Custom;
            dtpBirthDate.CustomFormat = "MM/dd/yyyy";
        }

        private void Employees_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Employees");
            if (chkShowInactive.Checked == true)
                LoadRecords(1, 2);
            else
                LoadRecords(1, 1);
            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }
            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();           
        }

        private void mskResNo_Enter(object sender, EventArgs e)
        {
            mskResNo.BackColor = Color.PaleGreen;
        }

        private void mskResNo_Leave(object sender, EventArgs e)
        {
            mskResNo.BackColor = Color.White;
        }

        private void mskMobileNo_Leave(object sender, EventArgs e)
        {
            mskMobileNo.BackColor = Color.White;
        }

        private void mskMobileNo_Enter(object sender, EventArgs e)
        {
            mskMobileNo.BackColor = Color.PaleGreen;
        }

        private void mskContactNos_Leave(object sender, EventArgs e)
        {
            mskContactNos.BackColor = Color.White;
        }

        private void mskContactNos_Enter(object sender, EventArgs e)
        {
            mskContactNos.BackColor = Color.PaleGreen;
        }

        private void Employees_KeyDown(object sender, KeyEventArgs e)
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
                    //ATTENTION
                    if (nMode != 0)
                        SavePersonalData();
                    break;

                case Keys.F6:
                    //ATTENTION
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

        private void btnEditEmp_Click(object sender, EventArgs e)
        {
            btnEditEmp.Visible = false; btnSaveEmp.Visible = true; btnCancelEmp.Visible = true;
            if (tbcEmployee.SelectedTab.Name == "tabEmployment")
            {
                OpenControls(tabEmployment, true);
                OpenControls(tabOtherData, false);
                nEmp = 1; nOther = 0;
                dtpPartTime.Format = DateTimePickerFormat.Custom;
                dtpPartTime.CustomFormat = "MM/dd/yyyy";
            }
            else
            {
                OpenControls(tabOtherData, true);
                OpenControls(tabEmployment, false);
                nOther = 1; nEmp = 0;
            }
        }

        private void btnCancelEmp_Click(object sender, EventArgs e)
        {
            btnEditEmp.Visible = true; btnSaveEmp.Visible = false; btnCancelEmp.Visible = false; btnSaveEmp.Visible = true;
            if (tbcEmployee.SelectedTab.Name == "tabEmployment")
            {
                ClearControls(tabEmployment);
                OpenControls(tabEmployment, false);
                GetEmploymentData();
            }
            else
            {
                ClearControls(tabOtherData);
                OpenControls(tabOtherData, false);
                GetOtherData();
            }
            nEmp = 0; nOther = 0;
        }

        private void btnSaveEmp_Click(object sender, EventArgs e)
        {
            SaveEmploymentData();
            SaveOtherData();
        }

        private void tbcEmployee_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (nEmp == 1)
                tbcEmployee.SelectTab(0);
            else if (nOther == 1)
                tbcEmployee.SelectTab(1);
        }

        private void dtpDateHired_ValueChanged(object sender, EventArgs e)
        {
            dtpDateHired.Format = DateTimePickerFormat.Custom;
            dtpDateHired.CustomFormat = "MM/dd/yyyy";
        }

        private void dtpFullTime_ValueChanged(object sender, EventArgs e)
        {
            dtpFullTime.Format = DateTimePickerFormat.Custom;
            dtpFullTime.CustomFormat = "MM/dd/yyyy";
        }

        private void dtpPartTime_ValueChanged(object sender, EventArgs e)
        {
            dtpPartTime.Format = DateTimePickerFormat.Custom;
            dtpPartTime.CustomFormat = "MM/dd/yyyy";
        }

        private void dtpLastWork_ValueChanged(object sender, EventArgs e)
        {
            dtpLastWork.Format = DateTimePickerFormat.Custom;
            dtpLastWork.CustomFormat = "MM/dd/yyyy";
        }

        private void tbcEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbcEmployee.SelectedIndex == 1)
            {
                dtWLAppr = null; cboWLApprovers.Enabled = false;
                dtWLAppr = PSSClass.Employees.WLApprovers(Convert.ToInt16(txtID.Text));
                dgvWLApprovers.DataSource = dtWLAppr;
                dgvWLApprovers.Columns["EmpName"].Width = 160;
                dgvWLApprovers.Columns["EmployeeID"].Visible = false;
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus(); 
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

        private void btnDelApprover_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnDelApprover.Text == "De&lete")
                {
                    dgvWLApprovers.Rows.RemoveAt(dgvWLApprovers.CurrentCell.RowIndex);
                }
                else
                {
                    btnDelApprover.Text = "De&lete";
                    btnAddApprover.Text = "A&dd";
                    lblAddNewAppr.Visible = false;
                    cboWLApprovers.SelectedIndex = -1;
                    cboWLApprovers.Enabled = false;
                }
            }
            catch { }
        }

        private void btnAddApprover_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnAddApprover.Text == "A&dd")
                {
                    cboWLApprovers.Enabled = true;
                    cboWLApprovers.DroppedDown = true;
                    btnAddApprover.Text = "OK";
                    btnDelApprover.Text = "Cancel";
                    lblAddNewAppr.Visible = true;
                }
                else
                {
                    DataRow dR = dtWLAppr.NewRow();
                    dR["EmpName"] = cboWLApprovers.Text;
                    dR["EmployeeID"] = cboWLApprovers.SelectedValue;
                    dtWLAppr.Rows.Add(dR);
                    btnAddApprover.Text = "A&dd";
                    btnDelApprover.Text = "De&lete";
                    lblAddNewAppr.Visible = false;
                    cboWLApprovers.SelectedIndex = -1;
                    cboWLApprovers.Enabled = false;
                }
            }
            catch { }
        }

        private void cboWLApprovers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch { }
        }
    }
}