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
    public partial class OOS : PSS.TemplateForm
    {
        byte nMode = 0;
        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "FA";

        DataTable dtOOSTypes = new DataTable();
        DataTable dtAnalysts = new DataTable();
        DataTable dtSponsor = new DataTable();
        DataTable dtSupervisors = new DataTable();
        DataTable dtMaster = new DataTable();
        DataTable dtSlashes = new DataTable();
        DataTable dtEquipments = new DataTable();
        DataTable dtReagents = new DataTable();

        public OOS()
        {
            InitializeComponent();
            dtClientNoted.Format = DateTimePickerFormat.Custom;
            dtClientNoted.CustomFormat = "MM/dd/yyyy hh:mm tt";
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
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        // LOAD DATATABLES ========================================================================================================================================================

        private void OOS_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "OOS");

            LoadRecords();
            LoadGBLNos();
            LoadOOSTypes();
            LoadAnalysts();
            LoadSupervisors();            

            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            CreateMasterStructure();
            CreateEQStructure();
            CreateRAStructure();
            BuildPrintItems();
            BuildSearchItems();            
            dtClientNoted.Format = DateTimePickerFormat.Custom;
            dtClientNoted.CustomFormat = "MM/dd/yyyy hh:mm tt";
        }

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.QA.OOSMaster();
            nMode = 0;
            if (dt == null)
                ConnectionError();
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            FileAccess();
        }

        private void LoadGBLNos()
        {
            cboGBLNo.DataSource = null;
            DataTable dt = new DataTable();
            dt = PSSClass.QA.OOSPSSNos();
            if (dt == null || dt.Rows.Count == 0)
                ConnectionError();
            cboGBLNo.DataSource = dt;
            cboGBLNo.DisplayMember = "PSSNo";
            cboGBLNo.ValueMember = "PSSNo";
        }

        private void LoadOOSTypes()
        {
            dgvOOSTypes.DataSource = null;

            dtOOSTypes = PSSClass.QA.OOSTypes();
            if (dtOOSTypes == null || dtOOSTypes.Rows.Count == 0)
                ConnectionError();

            dgvOOSTypes.DataSource = dtOOSTypes;
            StandardDGVSetting(dgvOOSTypes);
            dgvOOSTypes.Columns[0].Width = 160;
            dgvOOSTypes.Columns[1].Visible = false;
        }

        private void LoadAnalysts()
        {
            dgvAnalysts.DataSource = null;

            dtAnalysts = PSSClass.QA.OOSAnalysts();
            if (dtAnalysts == null || dtAnalysts.Rows.Count == 0)
                ConnectionError();

            dgvAnalysts.DataSource = dtAnalysts;
            StandardDGVSetting(dgvAnalysts);
            dgvAnalysts.Columns[0].Width = 377;
            dgvAnalysts.Columns[1].Visible = false;
        }

        private void LoadSupervisors()
        {
            dgvSupervisors.DataSource = null;

            dtSupervisors = PSSClass.QA.OOSSupervisors();
            if (dtSupervisors == null || dtSupervisors.Rows.Count == 0)
                ConnectionError();

            dgvSupervisors.DataSource = dtSupervisors;
            StandardDGVSetting(dgvSupervisors);
            dgvSupervisors.Columns[0].Width = 377;
            dgvSupervisors.Columns[1].Visible = false;
        }

        private void LoadData()
        {
            nMode = 0;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            btnClose.Visible = true; btnClose.BringToFront();

            dgvFile.Rows[0].Selected = true;
            txtOOSID.Text = dgvFile.CurrentRow.Cells["OOSID"].Value.ToString();

            LoadMaster(Convert.ToInt16(txtOOSID.Text));
            LoadEquipments();
            LoadReagents();

            AddEditMode(false);
            FileAccess();

            OpenControls(pnlRecord, false);
            OpenControls(pnlSlashes, false);
            OpenControls(pnlTabDetails, false);

            //Disable tabDetail Controls -ST 07/28/2017
            TabControl.TabPageCollection pages = tabDetails.TabPages;
            foreach (TabPage page in pages)
            {
                page.Show();
                foreach (Control ctrl in page.Controls)
                {
                    if (ctrl is TextBox || ctrl is CheckBox || ctrl is DateTimePicker || ctrl is Panel)
                    {
                        ctrl.Enabled = false;
                    }
                }
            }
            tabDetails.SelectedIndex = 0;
        }

        private void LoadMaster(int cOOSID)
        {
            try
            {
                dtMaster = PSSClass.QA.OOSMain(Convert.ToInt16(txtOOSID.Text));
                bsMaster.DataSource = dtMaster;
                BindMaster();
            }
            catch { }
        }

        private void LoadSponsor(Int64 cGBLNo)
        {
            dgvSponsor.DataSource = null;

            dtSponsor = PSSClass.QA.OOSSponsor(cGBLNo);
            if (dtSponsor == null || dtSponsor.Rows.Count == 0)
                ConnectionError();

            dgvSponsor.DataSource = dtSponsor;
            txtSponsorID.Text = dgvSponsor.Rows[0].Cells["SponsorID"].Value.ToString();
            txtSponsorName.Text = dgvSponsor.Rows[0].Cells["SponsorName"].Value.ToString();
        }

        private void LoadServiceCodes(Int64 cGBLNo)
        {
            cboServiceCode.DataSource = null;
            DataTable dt = new DataTable();
            dt = PSSClass.QA.OOSServiceCodes(Convert.ToInt64(cboGBLNo.Text));
            if (dt == null || dt.Rows.Count == 0)
                ConnectionError();

            cboServiceCode.DataSource = dt;
            cboServiceCode.DisplayMember = "ServiceCode";
            cboServiceCode.ValueMember = "ServiceCode";
        }

        private void LoadSlashes()
        {
            if (txtOOSID.Text != "")
            {
                dtSlashes = null;
                dtSlashes = PSSClass.QA.OOSSlashes(Convert.ToInt16(txtOOSID.Text));
                bsSlashes.DataSource = dtSlashes;
                dgvSlashes.DataSource = bsSlashes;
                DataGridSlashesSetting();
            }
        }

        private void LoadSlashesByGBL(Int64 cGBLNo)
        {
            dtSlashes = null;
            dtSlashes = PSSClass.QA.OOSSlashesByPSS(cGBLNo);
            if (dtSlashes == null)
                ConnectionError();

            bsSlashes.DataSource = dtSlashes;
            dgvSlashes.DataSource = bsSlashes;
            DataGridSlashesSetting();
        }

        private void LoadEquipments()
        {
            dtEquipments = null;
            dtEquipments = PSSClass.QA.OOSEquipments(Convert.ToInt16(txtOOSID.Text));
            if (dtEquipments == null)
                ConnectionError();

            bsEquipments.DataSource = dtEquipments;
            bnEquipments.BindingSource = bsEquipments;
            dgvEquipmentList.DataSource = bsEquipments;

            DataGridEquipmentsSetting();
            BindEquipment();
        }

        private void LoadReagents()
        {
            dtReagents = null;
            dtReagents = PSSClass.QA.OOSReagents(Convert.ToInt16(txtOOSID.Text));
            bsReagents.DataSource = dtReagents;
            bnReagents.BindingSource = bsReagents;
            dgvReagentList.DataSource = bsReagents;

            DataGridReagentsSetting();
            BindReagent();
        }

        // CREATE/BUILD APP ========================================================================================================================================================

        private void CreateMasterStructure()
        {     
            dtMaster.Columns.Add("OOSID", typeof(Int16));
            dtMaster.Columns.Add("PSSNo", typeof(Int64));
            dtMaster.Columns.Add("ServiceCode", typeof(Int16));
            dtMaster.Columns.Add("OOSTypeID", typeof(Int16));
            dtMaster.Columns.Add("OOSTypeName", typeof(string));
            dtMaster.Columns.Add("OOSNo", typeof(string));
            dtMaster.Columns.Add("SponsorID", typeof(Int16));
            dtMaster.Columns.Add("SponsorName", typeof(string));
            dtMaster.Columns.Add("DateObserved", typeof(DateTime));
            dtMaster.Columns.Add("NotedFailure", typeof(string));
            dtMaster.Columns.Add("AnalystID", typeof(Int16));
            dtMaster.Columns.Add("AnalystName", typeof(string));
            dtMaster.Columns.Add("SupervisorID", typeof(Int16));
            dtMaster.Columns.Add("SupervisorName", typeof(string));
            dtMaster.Columns.Add("TestProcList", typeof(string));
            dtMaster.Columns.Add("HadTechChart1", typeof(bool));
            dtMaster.Columns.Add("DidUnderstand", typeof(bool));
            dtMaster.Columns.Add("DidUnderstandDesc", typeof(string));
            dtMaster.Columns.Add("DidPerform", typeof(bool));
            dtMaster.Columns.Add("DidPerformDesc", typeof(string));
            dtMaster.Columns.Add("WasValidMethod", typeof(bool));
            dtMaster.Columns.Add("WasValidMethodDesc", typeof(string));
            dtMaster.Columns.Add("HadTechChart2", typeof(bool));
            dtMaster.Columns.Add("HadTechChart2Desc", typeof(string));
            dtMaster.Columns.Add("WereCalcsCorrect", typeof(bool));
            dtMaster.Columns.Add("WereCalcsCorrectDesc", typeof(string));
            dtMaster.Columns.Add("WasInsCorrect", typeof(bool));
            dtMaster.Columns.Add("WasInsCorrectDesc", typeof(string));
            dtMaster.Columns.Add("WasInsCalibrated", typeof(bool));
            dtMaster.Columns.Add("WasInsCalibratedDesc", typeof(string));
            dtMaster.Columns.Add("WasRACorrect", typeof(bool));
            dtMaster.Columns.Add("WasRACorrectDesc", typeof(string));
            dtMaster.Columns.Add("WasRAExpired", typeof(bool));
            dtMaster.Columns.Add("WasRAExpiredDesc", typeof(string));
            dtMaster.Columns.Add("WasRAProperlyPrep", typeof(bool));
            dtMaster.Columns.Add("WasRAProperyPrepDesc", typeof(string));
            dtMaster.Columns.Add("ClientNoteDate", typeof(DateTime));
            dtMaster.Columns.Add("ClientContact", typeof(string));
            dtMaster.Columns.Add("DidClientRequestOOS", typeof(bool));
            dtMaster.Columns.Add("WereNotesGood", typeof(bool));
            dtMaster.Columns.Add("WereNotesGoodDesc", typeof(string));
            dtMaster.Columns.Add("DoesCauseExist", typeof(bool));
            dtMaster.Columns.Add("DoesCauseExistDesc", typeof(string));
            dtMaster.Columns.Add("ReviewSummary", typeof(string));
            dtMaster.Columns.Add("InvestigationReason", typeof(string));
            dtMaster.Columns.Add("ProcessSeqSummary", typeof(string));
            dtMaster.Columns.Add("CorrectiveActions", typeof(string));
            dtMaster.Columns.Add("IsInvConclusive", typeof(bool));
            dtMaster.Columns.Add("IsInvConclusiveDesc", typeof(string));
            dtMaster.Columns.Add("EquipmentData", typeof(string));
            dtMaster.Columns.Add("ReagentData", typeof(string));
            dtMaster.Columns.Add("CreatedByID", typeof(Int16));
            dtMaster.Columns.Add("DateCreated", typeof(DateTime));
            dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            dtMaster.Columns.Add("LastUserID", typeof(Int16));
            bsMaster.DataSource = dtMaster;
        }

        private void CreateEQStructure()
        {        
            dtEquipments.Columns.Add("OOSID", typeof(Int16));
            dtEquipments.Columns.Add("EquipmentNo", typeof(string));
            dtEquipments.Columns.Add("EquipmentName", typeof(string));
            dtEquipments.Columns.Add("CalibrationDate", typeof(DateTime));
            dtEquipments.Columns.Add("MaintenanceDate", typeof(DateTime));
            bsEquipments.DataSource = dtEquipments;
        }

        private void CreateRAStructure()
        {        
            dtReagents.Columns.Add("OOSID", typeof(Int16));
            dtReagents.Columns.Add("ReagentNo", typeof(string));
            dtReagents.Columns.Add("ReagentName", typeof(string));
            dtReagents.Columns.Add("PrepDate", typeof(DateTime));
            dtReagents.Columns.Add("ExpDate", typeof(DateTime));
            bsReagents.DataSource = dtReagents;
        }

        private void BuildSearchItems()
        {
            int itemsShown = 14;
            DataTable dt = new DataTable();
            dt = PSSClass.QA.OOSMaster();
            if (dt == null)
                ConnectionError();
            arrCol = new string[dt.Columns.Count];
            ToolStripMenuItem[] items = new ToolStripMenuItem[itemsShown];

            for (int i = 0; i < itemsShown; i++)
            {
                items[i] = new ToolStripMenuItem();
                items[i].Name = dt.Columns[i].ColumnName;

                //Using LINQ to insert space between capital letters
                var val = dt.Columns[i].ColumnName;
                val = string.Concat(val.Select((x, y) => (char.IsUpper(x) && y > 0 && (char.IsLower(val[y - 1]) || (y < val.Count() - 1 && char.IsLower(val[y + 1])))) ? " " + x : x.ToString()));  //Exclude consecutive capital letters such as 'ID'  -ST 07/28/2017

                items[i].Text = val;
                items[i].Click += new EventHandler(SearchItemClickHandler);
                arrCol[i] = dt.Columns[i].DataType.ToString();
            }

            //Populate ckLColumns and check visible columns
            int j = 0;
            foreach (DataColumn colFile in dt.Columns)
            {
                var val = dt.Columns[j].ColumnName;
                val = string.Concat(val.Select((x, y) => (char.IsUpper(x) && y > 0 && (char.IsLower(val[y - 1]) || (y < val.Count() - 1 && char.IsLower(val[y + 1])))) ? " " + x : x.ToString()));
                cklColumns.Items.Add(val);
                if (dgvFile.Columns[j].Visible == true)
                {
                    cklColumns.SetItemChecked(j, true);
                }
                j += 1;
            }

            tsddbSearch.DropDownItems.AddRange(items);
            tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];
            items[0] = new ToolStripMenuItem();
            items[0].Name = "AnalyticalLabInvestigation";
            items[0].Text = "Analytical Laboratory Investigation";
            items[0].Click += new EventHandler(PrintLabInvReportClickHandler);
            tsddbPrint.DropDownItems.AddRange(items);
        }

        // DISPLAY SETTINGS ========================================================================================================================================================

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvFile.Columns["PSSNo"].Width = 100;
            dgvFile.Columns["ServiceCode"].Width = 70;
            dgvFile.Columns["OOSTypeID"].Width = 100;
            dgvFile.Columns["OOSTypeName"].Width = 100;
            dgvFile.Columns["OOSNo"].Width = 70;
            dgvFile.Columns["DateObserved"].Width = 70;
            dgvFile.Columns["SponsorID"].Width = 100;
            dgvFile.Columns["SponsorName"].Width = 300;
            dgvFile.Columns["NotedFailure"].Width = 350;
            dgvFile.Columns["AnalystName"].Width = 200;
            dgvFile.Columns["SupervisorName"].Width = 200;

            dgvFile.Columns["DateObserved"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["ClientNoteDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["PSSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["OOSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["OOSTypeName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateObserved"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataTable dt = new DataTable();
            dt = PSSClass.QA.OOSMaster(); int i = 0;
            arrCol = new string[dgvFile.Columns.Count];

            foreach (DataColumn colFile in dt.Columns)
            {
                dgvFile.Columns[i].Name = colFile.ColumnName;
                
                var val = colFile.ColumnName;
                val = string.Concat(val.Select((x, y) => (char.IsUpper(x) && y > 0 && (char.IsLower(val[y - 1]) || (y < val.Count() - 1 && char.IsLower(val[y + 1])))) ? " " + x : x.ToString()));

                dgvFile.Columns[i].HeaderText = val;
                i += 1;
            }
            dgvFile.Columns["OOSID"].HeaderText = "Index ID";
            dgvFile.Columns["OOSTypeID"].HeaderText = "Type ID";
            dgvFile.Columns["OOSTypeName"].HeaderText = "Investigation Type";

            for (int j = 14; j < dgvFile.Columns.Count; j++)
            {
                dgvFile.Columns[j].Visible = false;
            }
            dgvFile.Columns["OOSID"].Visible = false;
            dgvFile.Columns["OOSTypeID"].Visible = false;
            dgvFile.Columns["SponsorID"].Visible = false;
            dgvFile.Columns["AnalystID"].Visible = false;
            dgvFile.Columns["SupervisorID"].Visible = false;
            
        }

        private void DataGridSlashesSetting()
        {
            dgvSlashes.EnableHeadersVisualStyles = false;
            dgvSlashes.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSlashes.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvSlashes.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSlashes.Columns["SlashNo"].HeaderText = "Slash No";
            dgvSlashes.Columns["IsSelected"].HeaderText = "Select";
            dgvSlashes.Columns["SlashNo"].Width = 100;
            dgvSlashes.Columns["IsSelected"].Width = 100;
            dgvSlashes.Columns["SlashNo"].DefaultCellStyle.Padding = new Padding(30, 0, 0, 0);
            dgvSlashes.Columns["SlashNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        }

        private void DataGridEquipmentsSetting()
        {
            dgvEquipmentList.EnableHeadersVisualStyles = false;
            dgvEquipmentList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEquipmentList.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvEquipmentList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEquipmentList.Columns["EquipmentNo"].HeaderText = "Eqpt Ref";
            dgvEquipmentList.Columns["EquipmentName"].HeaderText = "Eqpt Name";
            dgvEquipmentList.Columns["CalibrationDate"].HeaderText = "Cal Date";
            dgvEquipmentList.Columns["MaintenanceDate"].HeaderText = "Maint Date";
            dgvEquipmentList.Columns["EquipmentNo"].Width = 100;
            dgvEquipmentList.Columns["EquipmentName"].Width = 100;
            dgvEquipmentList.Columns["CalibrationDate"].Width = 73;
            dgvEquipmentList.Columns["MaintenanceDate"].Width = 73;
            dgvEquipmentList.Columns["CalibrationDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvEquipmentList.Columns["MaintenanceDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvEquipmentList.Columns["EquipmentNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            dgvEquipmentList.Columns["OOSID"].Visible = false;
        }

        private void DataGridReagentsSetting()
        {
            dgvReagentList.EnableHeadersVisualStyles = false;
            dgvReagentList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReagentList.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvReagentList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReagentList.Columns["ReagentNo"].HeaderText = "Reagent Ref";
            dgvReagentList.Columns["ReagentName"].HeaderText = "Reagent Name";
            dgvReagentList.Columns["PrepDate"].HeaderText = "Prep Date";
            dgvReagentList.Columns["ExpDate"].HeaderText = "Exp Date";
            dgvReagentList.Columns["ReagentNo"].Width = 100;
            dgvReagentList.Columns["ReagentName"].Width = 100;
            dgvReagentList.Columns["PrepDate"].Width = 73;
            dgvReagentList.Columns["ExpDate"].Width = 73;
            dgvReagentList.Columns["PrepDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvReagentList.Columns["ExpDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvReagentList.Columns["ReagentNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft;
            dgvReagentList.Columns["OOSID"].Visible = false;
        }

        // SET DATA BINDINGS ========================================================================================================================================================

        private void BindMaster()
        {
            // Clear bindings -ST 08/07/2017
            TabControl.TabPageCollection pages = tabDetails.TabPages;
            foreach (TabPage page in pages)
            {
                foreach (Control ctrl in page.Controls)
                {
                    if (ctrl is TextBox || ctrl is CheckBox || ctrl is DateTimePicker || ctrl is Panel)
                    {
                        ctrl.DataBindings.Clear();
                    }
                }
            }

            foreach (Control c in pnlRecord.Controls)
                c.DataBindings.Clear();

            txtOOSID.DataBindings.Add("Text", bsMaster, "OOSID");
            cboGBLNo.DataBindings.Add("Text", bsMaster, "PSSNo");
            cboServiceCode.DataBindings.Add("Text", bsMaster, "ServiceCode");
            txtOOSTypeID.DataBindings.Add("Text", bsMaster, "OOSTypeID");
            txtOOSType.DataBindings.Add("Text", bsMaster, "OOSTypeName");
            txtOOSNo.DataBindings.Add("Text", bsMaster, "OOSNo");
            txtSponsorID.DataBindings.Add("Text", bsMaster, "SponsorID");
            txtSponsorName.DataBindings.Add("Text", bsMaster, "SponsorName");
            txtNotedFailure.DataBindings.Add("Text", bsMaster, "NotedFailure");
            txtAnalystID.DataBindings.Add("Text", bsMaster, "AnalystID");
            txtAnalyst.DataBindings.Add("Text", bsMaster, "AnalystName");
            txtSupervisorID.DataBindings.Add("Text", bsMaster, "SupervisorID");
            txtSupervisor.DataBindings.Add("Text", bsMaster, "SupervisorName");
            txtListProcedureDesc.DataBindings.Add("Text", bsMaster, "TestProcList");
            chkHadTechChart1.DataBindings.Add("Checked", bsMaster, "HadTechChart1");
            chkDidTechUnderstand.DataBindings.Add("Checked", bsMaster, "DidUnderstand");
            txtDidTechUnderstandDesc.DataBindings.Add("Text", bsMaster, "DidUnderstandDesc");
            chkDidTechPerform.DataBindings.Add("Checked", bsMaster, "DidPerform");
            txtDidTechPerformDesc.DataBindings.Add("Text", bsMaster, "DidPerformDesc");
            chkWasValidMethod.DataBindings.Add("Checked", bsMaster, "WasValidMethod");
            txtWasValidMethodDesc.DataBindings.Add("Text", bsMaster, "WasValidMethodDesc");
            chkHadTechChart2.DataBindings.Add("Checked", bsMaster, "HadTechChart2");
            txtHadTechChart2Desc.DataBindings.Add("Text", bsMaster, "HadTechChart2Desc");
            chkWereCalcsCorrect.DataBindings.Add("Checked", bsMaster, "WereCalcsCorrect");
            txtWereCalcsCorrectDesc.DataBindings.Add("Text", bsMaster, "WereCalcsCorrectDesc");
            chkWasInsCorrect.DataBindings.Add("Checked", bsMaster, "WasInsCorrect");
            txtWasInsCorrectDesc.DataBindings.Add("Text", bsMaster, "WasInsCorrectDesc");
            chkWasInsCalibrated.DataBindings.Add("Checked", bsMaster, "WasInsCalibrated");
            txtWasInsCalibratedDesc.DataBindings.Add("Text", bsMaster, "WasInsCalibratedDesc");

            Binding DateClientNotedBinding;
            DateClientNotedBinding = new Binding("Text", bsMaster, "ClientNoteDate");
            DateClientNotedBinding.Format += new ConvertEventHandler(DateTimeBinding_Format);
            dtClientNoted.DataBindings.Add(DateClientNotedBinding);
            chkWasRACorrect.DataBindings.Add("Checked", bsMaster, "WasRACorrect");
            txtWasRACorrectDesc.DataBindings.Add("Text", bsMaster, "WasRACorrectDesc");
            chkWasRAExpired.DataBindings.Add("Checked", bsMaster, "WasRAExpired");
            txtWasRAExpiredDesc.DataBindings.Add("Text", bsMaster, "WasRAExpiredDesc");
            chkWasRAProperlyPrep.DataBindings.Add("Checked", bsMaster, "WasRAProperlyPrep");
            txtWasRAProperlyPrepDesc.DataBindings.Add("Text", bsMaster, "WasRAProperyPrepDesc");
            txtContactName.DataBindings.Add("Text", bsMaster, "ClientContact");
            chkDidClientRequest.DataBindings.Add("Checked", bsMaster, "DidClientRequestOOS");
            chkWereNotesGood.DataBindings.Add("Checked", bsMaster, "WereNotesGood");
            txtWereNotesGoodDesc.DataBindings.Add("Text", bsMaster, "WereNotesGoodDesc");
            chkDoesCauseExist.DataBindings.Add("Checked", bsMaster, "DoesCauseExist");
            txtCauseDesc.DataBindings.Add("Text", bsMaster, "DoesCauseExistDesc");
            txtReviewSummary.DataBindings.Add("Text", bsMaster, "ReviewSummary");

            Binding DateCreatedBinding;
            DateCreatedBinding = new Binding("Text", bsMaster, "DateObserved");
            DateCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskDateObserved.DataBindings.Add(DateCreatedBinding);
            txtInvestigationReason.DataBindings.Add("Text", bsMaster, "InvestigationReason");
            txtProcessSeqSummary.DataBindings.Add("Text", bsMaster, "ProcessSeqSummary");
            txtCorrectiveActions.DataBindings.Add("Text", bsMaster, "CorrectiveActions");
            txtIsInvConclusiveDesc.DataBindings.Add("Text", bsMaster, "IsInvConclusiveDesc");
            chkIsInvConclusive.DataBindings.Add("Checked", bsMaster, "IsInvConclusive");
            txtEqptData.DataBindings.Add("Text", bsMaster, "EquipmentData");
            txtReagentData.DataBindings.Add("Text", bsMaster, "ReagentData");
        }

        private void BindEquipment()
        {
            foreach (Control c in pnlEQDetail.Controls)
                c.DataBindings.Clear();

            txtEqptOOSID.DataBindings.Add("Text", bsEquipments, "OOSID");
            txtEQRef.DataBindings.Add("Text", bsEquipments, "EquipmentNo");
            txtEQName.DataBindings.Add("Text", bsEquipments, "EquipmentName");

            Binding DateCreatedBinding;
            DateCreatedBinding = new Binding("Text", bsEquipments, "CalibrationDate");
            DateCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskCalDate.DataBindings.Add(DateCreatedBinding);

            DateCreatedBinding = new Binding("Text", bsEquipments, "MaintenanceDate");
            DateCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskMntcDate.DataBindings.Add(DateCreatedBinding);
        }

        private void BindReagent()
        {
            foreach (Control c in pnlRADetail.Controls)
                c.DataBindings.Clear();

            txtReagentOOSID.DataBindings.Add("Text", bsReagents, "OOSID");
            txtRARef.DataBindings.Add("Text", bsReagents, "ReagentNo");
            txtRAName.DataBindings.Add("Text", bsReagents, "ReagentName");

            Binding DateCreatedBinding;
            DateCreatedBinding = new Binding("Text", bsReagents, "PrepDate");
            DateCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskPrepDate.DataBindings.Add(DateCreatedBinding);

            DateCreatedBinding = new Binding("Text", bsReagents, "ExpDate");
            DateCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskExpDate.DataBindings.Add(DateCreatedBinding);
        }

        private void DateBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "";
        }

        private void DateTimeBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy hh:mm tt");
            else
                e.Value = "";
        }

        // MAIN FUNCTIONS: ADD, EDIT, DELETE, & SAVE ============================================================================================================================================

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            ClearControls(this.pnlTabDetails);
            ClearControls(this.pnlEQDetail);
            ClearControls(this.pnlRADetail);
            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlTabDetails, true);
            dtMaster.Rows.Clear();
            dtEquipments.Rows.Clear();
            dtReagents.Rows.Clear();
            dtSlashes.Rows.Clear();
            AddEditMode(true);

            cboGBLNo.Focus();
            txtSponsorName.ReadOnly = true;

            // Create Master Data Row
            dtMaster.Rows.Clear();
            DataRow dR = dtMaster.NewRow();

            dR["OOSID"] = 1;
            dR["PSSNo"] = DBNull.Value;
            dR["ServiceCode"] = DBNull.Value;
            dR["OOSTypeID"] = DBNull.Value;
            dR["OOSTypeName"] = "";
            dR["OOSNo"] = DBNull.Value;
            dR["SponsorID"] = 0;
            dR["SponsorName"] = "";
            dR["DateObserved"] = DBNull.Value;
            dR["NotedFailure"] = DBNull.Value;
            dR["AnalystID"] = DBNull.Value;
            dR["AnalystName"] = "";
            dR["SupervisorID"] = DBNull.Value;
            dR["SupervisorName"] = DBNull.Value;
            dR["TestProcList"] = DBNull.Value;
            dR["HadTechChart1"] = false;
            dR["DidUnderstand"] = false;
            dR["DidUnderstandDesc"] = DBNull.Value;
            dR["DidPerform"] = false;
            dR["DidPerformDesc"] = DBNull.Value;
            dR["WasValidMethod"] = false;
            dR["WasValidMethodDesc"] = "";
            dR["HadTechChart2"] = false;
            dR["HadTechChart2Desc"] = "";
            dR["WereCalcsCorrect"] = false;
            dR["WereCalcsCorrectDesc"] = "";
            dR["WasInsCorrect"] = false;
            dR["WasInsCorrectDesc"] = "";
            dR["WasInsCalibrated"] = false;
            dR["WasInsCalibratedDesc"] = "";
            dR["WasRACorrect"] = false;
            dR["WasRACorrectDesc"] = "";
            dR["WasRAExpired"] = false;
            dR["WasRAExpiredDesc"] = "";
            dR["WasRAProperlyPrep"] = false;
            dR["WasRAProperyPrepDesc"] = "";
            dR["ClientNoteDate"] = DBNull.Value;
            dR["ClientContact"] = "";
            dR["DidClientRequestOOS"] = false;
            dR["WereNotesGood"] = false;
            dR["WereNotesGoodDesc"] = "N/A";
            dR["DoesCauseExist"] = false;
            dR["DoesCauseExistDesc"] = "N/A";
            dR["ReviewSummary"] = "";
            dR["InvestigationReason"] = "";
            dR["ProcessSeqSummary"] = "";
            dR["CorrectiveActions"] = "";
            dR["IsInvConclusive"] = false;
            dR["IsInvConclusiveDesc"] = "From the investigation, no laboratory error or other assignable cause was found within Gibraltar.";
            dR["EquipmentData"] = "";
            dR["ReagentData"] = "";
            dR["CreatedByID"] = LogIn.nUserID;
            dR["DateCreated"] = DateTime.Now;
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;

            dtMaster.Rows.Add(dR);
            bsMaster.DataSource = dtMaster;
            BindMaster();

            txtOOSID.Text = "< New >";
            dtClientNoted.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

            //Enable tabDetail Controls -ST 07/28/2017
            TabControl.TabPageCollection pages = tabDetails.TabPages;
            foreach (TabPage page in pages)
            {
                foreach (Control ctrl in page.Controls)
                {
                    ctrl.Enabled = true;
                    if (ctrl is TextBox && ctrl.Text == "N/A")
                    {
                        ((TextBox)ctrl).ReadOnly = true;
                    }
                    foreach (Control pnlCtrl in ctrl.Controls)
                    {
                        pnlCtrl.Enabled = true;
                    }
                }
            }

            btnOKEQDetail.Enabled = false;
            btnCancelEQDetail.Enabled = false;
            btnOKRADetail.Enabled = false;
            btnCancelRADetail.Enabled = false;
            if (dgvEquipmentList.Rows.Count == 0)
                NoEQDetails();
            if (dgvReagentList.Rows.Count == 0)
                NoRADetails();
        }

        private void EditRecord()
        {            
            if (dgvFile.Rows.Count == 0)
                return;

            LoadData();
            nMode = 2;
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();

            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlSlashes, true);
            OpenControls(this.pnlTabDetails, true);
            btnClose.Visible = false;
            txtOOSID.Enabled = false;
            AddEditMode(true);

            //Enable tabDetail Controls -ST 07/28/2017
            TabControl.TabPageCollection pages = tabDetails.TabPages;
            foreach (TabPage page in pages)
            {
                foreach (Control ctrl in page.Controls)
                {
                    ctrl.Enabled = true;
                    if (ctrl is TextBox && ctrl.Text == "N/A")
                    {
                        ((TextBox)ctrl).ReadOnly = true;
                    }
                    foreach (Control pnlCtrl in ctrl.Controls)
                    {
                        pnlCtrl.Enabled = true;
                    }
                }
            }

            btnOKEQDetail.Enabled = false;
            btnCancelEQDetail.Enabled = false;
            btnOKRADetail.Enabled = false;
            btnCancelRADetail.Enabled = false;
            if (dgvEquipmentList.Rows.Count == 0)
                NoEQDetails();
            if (dgvReagentList.Rows.Count == 0)
                NoRADetails();
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
                sqlcmd.Parameters.Add(new SqlParameter("@OOSID", SqlDbType.Int));
                sqlcmd.Parameters["@OOSID"].Value = Convert.ToInt16(txtOOSID.Text);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelOOS";
                sqlcmd.ExecuteNonQuery();
            }
            LoadRecords();
        }

        private void DeleteOOSSlashes()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.Add(new SqlParameter("@OOSID", SqlDbType.Int));
            sqlcmd.Parameters["@OOSID"].Value = Convert.ToInt16(txtOOSID.Text);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelOOSSlashes";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveRecord()
        {
            bsMaster.EndEdit();
            DataTable dt = dtMaster.GetChanges();
            if (dt != null)
            {
                int nPR = ValidateMaster();
                if (nPR == 0)
                {
                    dt.Dispose();
                    return;
                }
                SaveMaster();
                dt.Dispose();
            }            
            
            bsEquipments.EndEdit();
            DataTable EQdt = dtEquipments.GetChanges();
            if (EQdt != null)
                CreateEqptXML();

            bsReagents.EndEdit();
            DataTable RAdt = dtReagents.GetChanges();
            if (RAdt != null)
                CreateReagentXML();

            // Cancel save if no changes were made
            if (dt == null && EQdt == null && RAdt == null)
            {
                CancelSave();
                return;
            }

            SaveSlashRecords();
            LoadRecords();
            PSSClass.General.FindRecord("OOSID", txtOOSID.Text, bsFile, dgvFile);
            LoadData();
            MessageBox.Show("Record successfully saved!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void SaveMaster()
        {
            if (nMode == 1)
                txtOOSID.Text = PSSClass.DataEntry.NewID("OOSMaster", "OOSID").ToString();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@OOSID", Convert.ToInt16(txtOOSID.Text));
            sqlcmd.Parameters.AddWithValue("@PSSNo", Convert.ToInt64(cboGBLNo.Text));
            sqlcmd.Parameters.AddWithValue("@ServiceCode", Convert.ToInt16(cboServiceCode.Text));
            sqlcmd.Parameters.AddWithValue("@OOSTypeID", Convert.ToInt16(txtOOSTypeID.Text));
            sqlcmd.Parameters.AddWithValue("@OOSNo", txtOOSNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@DateObserved", Convert.ToDateTime(mskDateObserved.Text));
            sqlcmd.Parameters.AddWithValue("@NotedFailure", txtNotedFailure.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@AnalystID", Convert.ToInt16(txtAnalystID.Text));
            sqlcmd.Parameters.AddWithValue("@SupervisorID", Convert.ToInt16(txtSupervisorID.Text));
            sqlcmd.Parameters.AddWithValue("@TestProcList", txtListProcedureDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@HadTechChart1", Convert.ToBoolean(chkHadTechChart1.CheckState));
            sqlcmd.Parameters.AddWithValue("@DidUnderstand", Convert.ToBoolean(chkDidTechUnderstand.CheckState));
            sqlcmd.Parameters.AddWithValue("@DidUnderstandDesc", txtDidTechUnderstandDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@DidPerform", Convert.ToBoolean(chkDidTechPerform.CheckState));
            sqlcmd.Parameters.AddWithValue("@DidPerformDesc", txtDidTechPerformDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@WasValidMethod", Convert.ToBoolean(chkWasValidMethod.CheckState));
            sqlcmd.Parameters.AddWithValue("@WasValidMethodDesc", txtWasValidMethodDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@HadTechChart2", Convert.ToBoolean(chkHadTechChart2.CheckState));
            sqlcmd.Parameters.AddWithValue("@HadTechChart2Desc", txtHadTechChart2Desc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@WereCalcsCorrect", Convert.ToBoolean(chkWereCalcsCorrect.CheckState));
            sqlcmd.Parameters.AddWithValue("@WereCalcsCorrectDesc", txtWereCalcsCorrectDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@WasInsCorrect", Convert.ToBoolean(chkWasInsCorrect.CheckState));
            sqlcmd.Parameters.AddWithValue("@WasInsCorrectDesc", txtWasInsCorrectDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@WasInsCalibrated", Convert.ToBoolean(chkWasInsCalibrated.CheckState));
            sqlcmd.Parameters.AddWithValue("@WasInsCalibratedDesc", txtWasInsCalibratedDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@WasRACorrect", Convert.ToBoolean(chkWasRACorrect.CheckState));
            sqlcmd.Parameters.AddWithValue("@WasRACorrectDesc", txtWasRACorrectDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@WasRAExpired", Convert.ToBoolean(chkWasRAExpired.CheckState));
            sqlcmd.Parameters.AddWithValue("@WasRAExpiredDesc", txtWasRAExpiredDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@WasRAProperlyPrep", Convert.ToBoolean(chkWasRAProperlyPrep.CheckState));
            sqlcmd.Parameters.AddWithValue("@WasRAProperyPrepDesc", txtWasRAProperlyPrepDesc.Text.Trim());
            if (dtClientNoted.Text == "")
                sqlcmd.Parameters.AddWithValue("@ClientNoteDate", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@ClientNoteDate", Convert.ToDateTime(dtClientNoted.Text));
            sqlcmd.Parameters.AddWithValue("@ClientContact", txtContactName.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@DidClientRequestOOS", Convert.ToBoolean(chkDidClientRequest.CheckState));
            sqlcmd.Parameters.AddWithValue("@WereNotesGood", Convert.ToBoolean(chkWereNotesGood.CheckState));
            sqlcmd.Parameters.AddWithValue("@WereNotesGoodDesc", txtWereNotesGoodDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@DoesCauseExist", Convert.ToBoolean(chkDoesCauseExist.CheckState));
            sqlcmd.Parameters.AddWithValue("@DoesCauseExistDesc", txtCauseDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@ReviewSummary", txtReviewSummary.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@InvestigationReason", txtInvestigationReason.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@ProcessSeqSummary", txtProcessSeqSummary.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CorrectiveActions", txtCorrectiveActions.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@IsInvConclusive", Convert.ToBoolean(chkIsInvConclusive.CheckState));
            sqlcmd.Parameters.AddWithValue("@IsInvConclusiveDesc", txtIsInvConclusiveDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditOOS";
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

        private void CreateEqptXML()
        {
            bsEquipments.EndEdit();
            string strXML = "<EquipmentList>";
            if (dgvEquipmentList.Rows.Count != 0)
            {
                for (int i = 0; i < dgvEquipmentList.Rows.Count; i++)
                {
                    if (dgvEquipmentList.Rows[i].Cells[1].Value != null)
                    {
                        strXML = strXML + "<Equipment><OOSID>" + txtOOSID.Text + "</OOSID>" +
                                          "<EquipmentNo>" + PurgeString(dgvEquipmentList.Rows[i].Cells["EquipmentNo"].Value.ToString()) + "</EquipmentNo>" +
                                          "<EquipmentName>" + PurgeString(dgvEquipmentList.Rows[i].Cells["EquipmentName"].Value.ToString()) + "</EquipmentName>" +
                                          "<CalibrationDate>" + PurgeString(dgvEquipmentList.Rows[i].Cells["CalibrationDate"].Value.ToString()) + "</CalibrationDate>" +
                                          "<MaintenanceDate>" + PurgeString(dgvEquipmentList.Rows[i].Cells["MaintenanceDate"].Value.ToString()) + "</MaintenanceDate></Equipment>";
                    }
                }
            }
            strXML = strXML + "</EquipmentList>";
            SaveEquipmentXML(strXML);
        }

        private void CreateReagentXML()
        {
            bsReagents.EndEdit();
            string strXML = "<ReagentList>";
            if (dgvReagentList.Rows.Count != 0)
            {
                for (int i = 0; i < dgvReagentList.Rows.Count; i++)
                {
                    if (dgvReagentList.Rows[i].Cells[1].Value != null)
                    {
                        strXML = strXML + "<Reagent><OOSID>" + txtOOSID.Text + "</OOSID>" +
                                          "<ReagentNo>" + PurgeString(dgvReagentList.Rows[i].Cells["ReagentNo"].Value.ToString()) + "</ReagentNo>" +
                                          "<ReagentName>" + PurgeString(dgvReagentList.Rows[i].Cells["ReagentName"].Value.ToString()) + "</ReagentName>" +
                                          "<PrepDate>" + PurgeString(dgvReagentList.Rows[i].Cells["PrepDate"].Value.ToString()) + "</PrepDate>" +
                                          "<ExpDate>" + PurgeString(dgvReagentList.Rows[i].Cells["ExpDate"].Value.ToString()) + "</ExpDate></Reagent>";
                    }
                }                
            }
            strXML = strXML + "</ReagentList>";
            SaveReagentXML(strXML);
        }
        private void SaveEquipmentXML(String cStrXML)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@OOSID", Convert.ToInt16(txtOOSID.Text));
            sqlcmd.Parameters.AddWithValue("@XMLData", cStrXML);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdOOSEquipmentXML";
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

        private void SaveReagentXML(String cStrXML)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@OOSID", Convert.ToInt16(txtOOSID.Text));
            sqlcmd.Parameters.AddWithValue("@XMLData", cStrXML);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdOOSReagentXML";
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

        private void SaveSlashRecords()
        {
            if (dgvSlashes.Rows.Count != 0)
            {
                for (int i = 0; i < dgvSlashes.Rows.Count; i++)
                {
                    if (nMode == 2)
                    {
                        if (dgvSlashes.Rows[i].Cells["IsSelected"].Value.ToString() == "True")
                            AddEditSlashRecords(i);
                    }
                    else
                        AddEditSlashRecords(i);
                }
                dgvSlashes.Refresh();
            }
        }

        private void AddEditSlashRecords(int cRow)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@OOSID", Convert.ToInt16(txtOOSID.Text));
            sqlcmd.Parameters.AddWithValue("@SlashNo", dgvSlashes.Rows[cRow].Cells["SlashNo"].Value);
            sqlcmd.Parameters.AddWithValue("@IsSelected", dgvSlashes.Rows[cRow].Cells["IsSelected"].Value);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditOOSSlashes";
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

        private void UpdateSlashSelections(byte cSelect)
        {
            if (dgvSlashes.Rows.Count != 0)
            {
                for (int j = 0; j < dgvSlashes.Rows.Count; j++)
                {
                    dgvSlashes.Rows[j].Cells["IsSelected"].Value = cSelect;
                }
            }
        }

        private void CancelSave()
        {
            AddEditMode(false);
            if (nMode == 2)
            {
                LoadRecords();
                PSSClass.General.FindRecord("OOSID", txtOOSID.Text, bsFile, dgvFile);
            }
            else
            {
                LoadRecords();
            }
        }

        // EQ & RA PANEL FUNCTIONS ========================================================================================================================================================

        private void btnAddEQDetail_Click(object sender, EventArgs e)
        {
            ClearControls(this.pnlEQDetail);
            OpenControls(this.pnlEQDetail, true);
            txtEQRef.Focus();

            btnAddEQDetail.Enabled = false;
            btnDeleteEQDetail.Enabled = false;
            btnOKEQDetail.Enabled = true;
            btnCancelEQDetail.Enabled = true;

            EnableEQDetails();
            AddEditMode(true);
            
            foreach (Control c in pnlEQDetail.Controls)
            {
                c.DataBindings.Clear();
            }
        }

        private void btnDeleteEQDetail_Click(object sender, EventArgs e)
        {
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this equipment?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }

            foreach (DataGridViewRow item in this.dgvEquipmentList.SelectedRows)
            {
                dgvEquipmentList.Rows.RemoveAt(item.Index);
            }

            if (dgvEquipmentList.Rows.Count == 0)
                NoEQDetails();

            AddEditMode(true);
        }

        private void btnOKEQDetail_Click(object sender, EventArgs e)
        {
            int nPR = ValidateEQDetails();
            if (nPR == 0)
            {
                return;
            }

            DataRow dR = dtEquipments.NewRow();

            dR["OOSID"] = DBNull.Value;
            dR["EquipmentNo"] = txtEQRef.Text;
            dR["EquipmentName"] = txtEQName.Text;
            if (mskCalDate.MaskFull == false)
            {
                dR["CalibrationDate"] = DBNull.Value;
            }
            else
            {
                dR["CalibrationDate"] = mskCalDate.Text;
            }
            if (mskMntcDate.MaskFull == false)
            {
                dR["MaintenanceDate"] = DBNull.Value;
            }
            else
            {
                dR["MaintenanceDate"] = mskMntcDate.Text;
            }
            dtEquipments.Rows.Add(dR);
            bsEquipments.DataSource = dtEquipments;
            bnEquipments.BindingSource = bsEquipments;
            dgvEquipmentList.DataSource = bsEquipments;

            BindEquipment();
            btnAddEQDetail.Enabled = true;
            btnDeleteEQDetail.Enabled = true;
            btnOKEQDetail.Enabled = false;
            btnCancelEQDetail.Enabled = false;
            
            AddEditMode(true);
            DataGridEquipmentsSetting();
        }

        private void btnCancelEQDetail_Click(object sender, EventArgs e)
        {
            BindEquipment();
            btnAddEQDetail.Enabled = true;
            btnOKEQDetail.Enabled = false;
            btnCancelEQDetail.Enabled = false;
            if (dgvEquipmentList.Rows.Count == 0)
            {
                NoEQDetails();
            }
            else
            {
                btnDeleteEQDetail.Enabled = true;
            }
            
            AddEditMode(true);

            DataGridEquipmentsSetting();

        }

        private void btnAddRADetail_Click(object sender, EventArgs e)
        {
            ClearControls(this.pnlRADetail);
            OpenControls(this.pnlRADetail, true);
            txtRARef.Focus();

            btnAddRADetail.Enabled = false;
            btnDeleteRADetail.Enabled = false;
            btnOKRADetail.Enabled = true;
            btnCancelRADetail.Enabled = true;

            EnableRADetails();
            AddEditMode(true);

            foreach (Control c in pnlRADetail.Controls)
            {
                c.DataBindings.Clear();
            }
        }

        private void btnDeleteRADetail_Click(object sender, EventArgs e)
        {
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this reagent?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }

            foreach (DataGridViewRow item in this.dgvReagentList.SelectedRows)
            {
                dgvReagentList.Rows.RemoveAt(item.Index);
            }

            if (dgvReagentList.Rows.Count == 0)
                NoRADetails();

            AddEditMode(true);
        }

        private void btnOKRADetail_Click(object sender, EventArgs e)
        {
            int nPR = ValidateRADetails();
            if (nPR == 0)
            {
                return;
            }

            DataRow dR = dtReagents.NewRow();

            dR["OOSID"] = DBNull.Value;
            dR["ReagentNo"] = txtRARef.Text;
            dR["ReagentName"] = txtRAName.Text;
            if (mskPrepDate.MaskFull == false)
            {
                dR["PrepDate"] = DBNull.Value;
            }
            else
            {
                dR["PrepDate"] = mskPrepDate.Text;
            }
            if (mskExpDate.MaskFull == false)
            {
                dR["ExpDate"] = DBNull.Value;
            }
            else
            {
                dR["ExpDate"] = mskExpDate.Text;
            }
            dtReagents.Rows.Add(dR);
            bsReagents.DataSource = dtReagents;
            bnReagents.BindingSource = bsReagents;
            dgvReagentList.DataSource = bsReagents;

            BindReagent();
            btnAddRADetail.Enabled = true;
            btnDeleteRADetail.Enabled = true;
            btnOKRADetail.Enabled = false;
            btnCancelRADetail.Enabled = false;
            
            AddEditMode(true);

            DataGridReagentsSetting();
        }

        private void btnCancelRADetail_Click(object sender, EventArgs e)
        {
            BindReagent();
            btnAddRADetail.Enabled = true;
            btnOKRADetail.Enabled = false;
            btnCancelRADetail.Enabled = false;
            if (dgvReagentList.Rows.Count == 0)
            {
                NoRADetails();
            }
            else
            {
                btnDeleteRADetail.Enabled = true;
            }
            
            AddEditMode(true);

            DataGridEquipmentsSetting();
        }

        private void EnableEQDetails()
        {
            txtEQRef.Enabled = true;
            txtEQName.Enabled = true;
            mskCalDate.Enabled = true;
            mskMntcDate.Enabled = true;
        }

        private void EnableRADetails()
        {
            txtRARef.Enabled = true;
            txtRAName.Enabled = true;
            mskPrepDate.Enabled = true;
            mskExpDate.Enabled = true;
        }

        private void NoEQDetails()
        {
            btnDeleteEQDetail.Enabled = false;
            txtEQRef.Enabled = false;
            txtEQName.Enabled = false;
            mskCalDate.Enabled = false;
            mskMntcDate.Enabled = false;
        }

        private void NoRADetails()
        {
            btnDeleteRADetail.Enabled = false;
            txtRARef.Enabled = false;
            txtRAName.Enabled = false;
            mskPrepDate.Enabled = false;
            mskExpDate.Enabled = false;
        }

        // EVENT HANDLERS ========================================================================================================================================================

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

        private void PrintLabInvReportClickHandler(object sender, EventArgs e)
        {
            AnaLabInvestigationRpt rpt = new AnaLabInvestigationRpt();
            txtOOSID.Text = dgvFile.CurrentRow.Cells["OOSID"].Value.ToString();
            rpt.OOSID = Convert.ToInt16(txtOOSID.Text);
            rpt.WindowState = FormWindowState.Maximized;
            try
            {
                rpt.Show();
            }
            catch { }
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
                    bsFile.Filter = "OOSID<>0";
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
            bsFile.Filter = "OOSID<>0";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }
        
        private void CancelClickHandler(object sender, EventArgs e)
        {
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
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

        private void txtOOSTypeIDEnterHandler(object sender, EventArgs e)
        {
            dgvOOSTypes.Visible = false;
        }

        private void txtAnalystIDEnterHandler(object sender, EventArgs e)
        {
            dgvAnalysts.Visible = false;
        }

        private void txtSupervisorIDEnterHandler(object sender, EventArgs e)
        {
            dgvSupervisors.Visible = false;
        }             

        private void OOS_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    if (nMode == 0 && strFileAccess != "RO")
                        AddRecord();
                    break;

                case Keys.F3:
                    if (nMode == 0 && strFileAccess != "RO")
                        EditRecord();
                    break;

                case Keys.F4:
                    if (nMode == 0 && strFileAccess == "FA")
                        DeleteRecord();
                    break;

                case Keys.F5:
                    if (nMode != 0)
                        SaveRecord();
                    break;

                case Keys.F6:
                    if (nMode != 0)
                        CancelClickHandler(sender, e);
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

        private void cboGBLNo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboGBLNo.Text.Trim() != "")
                {
                    if (nMode == 1)
                    {
                        LoadSlashesByGBL(Convert.ToInt64(cboGBLNo.Text));
                    }
                    else
                    {
                        LoadSlashes();                        
                    }
                    LoadSponsor(Convert.ToInt64(cboGBLNo.Text));
                    LoadServiceCodes(Convert.ToInt64(cboGBLNo.Text));                    
                }
            }
            catch { }
        }

        private void cboServiceCode_SelectedValueChanged(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                try
                {
                    if (cboServiceCode.Text.Trim() != "")
                    {                        
                        DataGridEquipmentsSetting();
                        DataGridReagentsSetting();
                    }
                }
                catch { }
            }
        }
        
        // MY 12/02/2014 - START: txt/dgvOOSTypes events
        private void dgvOOSTypes_DoubleClick(object sender, EventArgs e)
        {
            txtOOSType.Text = dgvOOSTypes.CurrentRow.Cells[0].Value.ToString();
            txtOOSTypeID.Text = dgvOOSTypes.CurrentRow.Cells[1].Value.ToString();
            dgvOOSTypes.Visible = false;
        }

        private void dgvOOSTypes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvOOSTypes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtOOSType.Text = dgvOOSTypes.CurrentRow.Cells[0].Value.ToString();
                txtOOSTypeID.Text = dgvOOSTypes.CurrentRow.Cells[1].Value.ToString();
                dgvOOSTypes.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvOOSTypes.Visible = false;
            }
        }
        private void txtOOSType_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvOOSTypes.Visible = true; dgvOOSTypes.BringToFront();
            }
        }

        private void dgvOOSTypes_Leave(object sender, EventArgs e)
        {
            dgvOOSTypes.Visible = false;
        }

        private void txtOOSType_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwOOSTypes;
                dvwOOSTypes = new DataView(dtOOSTypes, "OOSTypeName like '%" + txtOOSType.Text.Trim().Replace("'", "''") + "%'", "OOSTypeName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvOOSTypes, dvwOOSTypes);

                txtInvestigationReason.Text = txtOOSType.Text;
            }
        }

        private void dgvOOSTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtOOSType.Text = dgvOOSTypes.CurrentRow.Cells[0].Value.ToString();
            txtOOSTypeID.Text = dgvOOSTypes.CurrentRow.Cells[1].Value.ToString();
            dgvOOSTypes.Visible = false;
        }

        private void picOOSTypes_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                LoadOOSTypes();
                dgvOOSTypes.Visible = true; dgvOOSTypes.BringToFront();
            }
        }

        private void txtOOSTypeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtOOSType.Text = PSSClass.QA.OOSTypeName(Convert.ToInt16(txtOOSTypeID.Text));
           
                if (txtOOSType.Text.Trim() == "")
                {
                    MessageBox.Show("No matching OOS Type ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvOOSTypes.Visible = false;
            }
            else
            {
                txtOOSType.Text = ""; dgvOOSTypes.Visible = false;
            }
        }
        
        // MY 12/01/2014 - START: txt/dgvAnalysts events
        private void dgvAnalysts_DoubleClick(object sender, EventArgs e)
        {
            txtAnalyst.Text = dgvAnalysts.CurrentRow.Cells[0].Value.ToString();
            txtAnalystID.Text = dgvAnalysts.CurrentRow.Cells[1].Value.ToString();
            dgvAnalysts.Visible = false;
        }

        private void dgvAnalysts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvAnalysts_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtAnalyst.Text = dgvAnalysts.CurrentRow.Cells[0].Value.ToString();
                txtAnalystID.Text = dgvAnalysts.CurrentRow.Cells[1].Value.ToString();
                dgvAnalysts.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvAnalysts.Visible = false;
            }
        }
        private void txtAnalyst_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {                
                dgvAnalysts.Visible = true; dgvAnalysts.BringToFront();
            }
        }
              
        private void dgvAnalysts_Leave(object sender, EventArgs e)
        {
            dgvAnalysts.Visible = false;
        }

        private void txtAnalyst_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwAnalysts;
                dvwAnalysts = new DataView(dtAnalysts, "AnalystName like '%" + txtAnalyst.Text.Trim().Replace("'", "''") + "%'", "AnalystName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvAnalysts, dvwAnalysts);                
            }
        }

        private void dgvAnalysts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtAnalyst.Text = dgvAnalysts.CurrentRow.Cells[0].Value.ToString();
            txtAnalystID.Text = dgvAnalysts.CurrentRow.Cells[1].Value.ToString();
            dgvAnalysts.Visible = false;
        }

        private void picAnalysts_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                LoadAnalysts();
                dgvAnalysts.Visible = true; dgvAnalysts.BringToFront();
            }
        }

        private void txtAnalystID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtAnalyst.Text = PSSClass.QA.OOSAnalystName(Convert.ToInt16(txtAnalystID.Text));
                if (txtAnalyst.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Analyst ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvAnalysts.Visible = false;
            }
            else
            {
                txtAnalyst.Text = ""; dgvAnalysts.Visible = false;
            }
        }

        // MY 12/02/2014 - START: txt/dgvSupervisors events
        private void dgvSupervisors_DoubleClick(object sender, EventArgs e)
        {
            txtSupervisor.Text = dgvSupervisors.CurrentRow.Cells[0].Value.ToString();
            txtSupervisorID.Text = dgvSupervisors.CurrentRow.Cells[1].Value.ToString();
            dgvSupervisors.Visible = false;
        }

        private void dgvSupervisors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSupervisors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSupervisor.Text = dgvSupervisors.CurrentRow.Cells[0].Value.ToString();
                txtSupervisorID.Text = dgvSupervisors.CurrentRow.Cells[1].Value.ToString();
                dgvSupervisors.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvSupervisors.Visible = false;
            }
        }
        private void txtSupervisor_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvSupervisors.Visible = true; dgvSupervisors.BringToFront();
            }
        }

        private void dgvSupervisors_Leave(object sender, EventArgs e)
        {
            dgvSupervisors.Visible = false;
        }

        private void txtSupervisor_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSupervisors;
                dvwSupervisors = new DataView(dtSupervisors, "SupervisorName like '%" + txtSupervisor.Text.Trim().Replace("'", "''") + "%'", "SupervisorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvSupervisors, dvwSupervisors);
            }
        }

        private void dgvSupervisors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtSupervisor.Text = dgvSupervisors.CurrentRow.Cells[0].Value.ToString();
            txtSupervisorID.Text = dgvSupervisors.CurrentRow.Cells[1].Value.ToString();
            dgvSupervisors.Visible = false;
        }

        private void picSupervisors_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                LoadSupervisors();
                dgvSupervisors.Visible = true; dgvSupervisors.BringToFront();
            }
        }

        private void txtSupervisorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSupervisor.Text = PSSClass.QA.OOSSupervisorName(Convert.ToInt16(txtSupervisorID.Text));
                if (txtSupervisor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Supervisor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvSupervisors.Visible = false;
            }
            else
            {
                txtSupervisor.Text = ""; dgvSupervisors.Visible = false;
            }
        }
        // MY 12/01/2014 - END: txt/dgvSupervisors events

        private void dgvSlashes_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (nMode == 0)
                e.Cancel = true;
            else if (e.ColumnIndex < 1)
                e.Cancel = true;            
        }
      
        private void dgvSlashes_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvSlashes.IsCurrentCellDirty)
                dgvSlashes.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }       

        private void cboGBLNo_KeyPress(object sender, KeyPressEventArgs e)
        {      
            if (e.KeyChar == 13)
            {   
                if (cboGBLNo.Text.Trim() != "")
                {
                    // Check if PSS entered is numeric
                    int Num;
                    bool isNum = int.TryParse(cboGBLNo.Text.ToString(), out Num);
                    if (!isNum)
                    {
                        MessageBox.Show("Entry must be numeric!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    // Check if PSS exists in PTS
                    bool isExists = false;
                    isExists = PSSClass.QA.OOSPSSExists(Convert.ToInt64(cboGBLNo.Text));
                    if (!isExists)
                    {
                        MessageBox.Show("PSS Number not found. Please enter a valid PSS Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (nMode == 1)
                    {
                        try
                        {
                            LoadSponsor(Convert.ToInt64(cboGBLNo.Text));
                            LoadServiceCodes(Convert.ToInt64(cboGBLNo.Text));
                            LoadSlashesByGBL(Convert.ToInt64(cboGBLNo.Text));
                            txtOOSType.Focus();
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

        private void chkAll_CheckStateChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (chkAll.Checked)
                {
                    UpdateSlashSelections(1);
                }
                else
                {
                    UpdateSlashSelections(0);
                }
            }
        }

        private void alert(Control focus, string str)
        {
            MessageBox.Show(str, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            focus.Focus();
        }

        private int ValidateMaster()
        {            
            if (cboGBLNo.Text.Trim() == "")
            {
                alert(cboGBLNo, "Please choose a PSS Number!");
                return 0;
            }
            if (cboServiceCode.Text.Trim() == "")
            {
                alert(cboServiceCode, "Please choose a Service Code!");
                return 0;
            }
            if (txtOOSTypeID.Text.Trim() == "")
            {
                alert(txtOOSTypeID, "Please choose an OOS Type!");
                return 0;
            }
            if (txtOOSNo.Text.Trim() == "" && txtOOSTypeID.Text.Trim() == "1")
            {
                alert(txtOOSNo, "Please enter an OOS Number!");
                return 0;
            }
            if (mskDateObserved.MaskFull == false)
            {
                alert(mskDateObserved, "Please enter date when issue was observed!");
                return 0;
            }
            if (txtAnalystID.Text.Trim() == "")
            {
                alert(txtAnalystID, "Please choose an Analyst!");
                return 0;
            }    
            if (txtSupervisorID.Text.Trim() == "")
            {
                alert(txtSupervisorID, "Please choose a Supervisor!");
                return 0;
            }
            // Check if any slash record was selected
            int nSelect = 0;
            for (int j = 0; j < dgvSlashes.Rows.Count; j++)
            {
                if (dgvSlashes.Rows[j].Cells["IsSelected"].Value.ToString() == "True")
                {
                    nSelect++;
                }
            }
            if (nSelect == 0)
            {
                alert(chkAll, "Please select a slash number to report!");
                return 0;
            }
            // Check if PSS/Service code/OOS Type ID is unique
            if (nMode == 1)
            {
                if (cboServiceCode.Text.Trim() != "")
                {
                    bool isUnique = false;
                    try
                    {
                        isUnique = PSSClass.QA.OOSPSSCodeUnique(Convert.ToInt64(cboGBLNo.Text), Convert.ToInt16(cboServiceCode.Text), Convert.ToInt16(txtOOSTypeID.Text));
                        if (isUnique)
                        {
                            MessageBox.Show("PSS Number/Service Code/Type already exists. Please check your entries!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return 0;
                        }
                    }
                    catch { }
                }
            }
            return 1;
        }

        private int ValidateEQDetails()
        {
            if (nMode != 0)
            {
                if (txtEQRef.Text.Trim() == "")
                {
                    alert(txtEQRef, "Please enter Equipment Reference!");
                    return 0;
                }
                DateTime dte;
                bool isDte = DateTime.TryParse(mskCalDate.Text, out dte);
                if (!isDte)
                {
                    alert(mskCalDate, "Entry must be a valid Calibration date!");
                    return 0;
                }
                isDte = DateTime.TryParse(mskMntcDate.Text, out dte);
                if (!isDte)
                {
                    alert(mskMntcDate, "Entry must be a valid Maintenance date!");
                    return 0;
                }
            }
            return 1;
        }

        private int ValidateRADetails()
        {
            if (nMode != 0)
            {
                if (txtRARef.Text.Trim() == "")
                {
                    alert(txtRARef, "Please enter Reagent Reference!");
                    return 0;
                }
                DateTime dte;
                bool isDte = DateTime.TryParse(mskPrepDate.Text, out dte);
                if (!isDte)
                {
                    alert(mskPrepDate, "Entry must be a valid Preparation date!");
                    return 0;
                }
                isDte = DateTime.TryParse(mskExpDate.Text, out dte);
                if (!isDte)
                {
                    alert(mskExpDate, "Entry must be a valid Expiration date!");
                    return 0;
                }
            }
            return 1;
        }        

        
        // Start: CheckBoxes N/A Routine
        private void NA(CheckBox cb, TextBox tb)
        {
            if (cb.Checked)
            {
                tb.Text = "N/A";
                tb.ReadOnly = true;
            }
            else
            {
                tb.Text = "";
                tb.ReadOnly = false;
            }
        }

        private void chkDidTechUnderstand_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkDidTechUnderstand, txtDidTechUnderstandDesc);
        }

        private void chkDidTechPerform_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkDidTechPerform, txtDidTechPerformDesc);
        }

        private void chkWasValidMethod_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkWasValidMethod, txtWasValidMethodDesc);
        }

        private void chkHadTechChart2_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkHadTechChart2, txtHadTechChart2Desc);
        }

        private void chkWereCalcsCorrect_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkWereCalcsCorrect, txtWereCalcsCorrectDesc);
        }

        private void chkWasInsCorrect_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkWasInsCorrect,txtWasInsCorrectDesc);
        }       

        private void chkWasInsCalibrated_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkWasInsCalibrated, txtWasInsCalibratedDesc);
        }

        private void chkWasRACorrect_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkWasRACorrect, txtWasRACorrectDesc);
        }

        private void chkWasRAExpired_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkWasRAExpired, txtWasRAExpiredDesc);
        }

        private void chkWasRAProperlyPrep_CheckStateChanged(object sender, EventArgs e)
        {
            NA(chkWasRAProperlyPrep, txtWasRAProperlyPrepDesc);
        }

        private void chkWereNotesGood_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkWereNotesGood.Checked)
            {
                txtWereNotesGoodDesc.Text = "";
                txtWereNotesGoodDesc.ReadOnly = false;
            }
            else
            {
                txtWereNotesGoodDesc.Text = "N/A";
                txtWereNotesGoodDesc.ReadOnly = true;
            }
        }

        private void chkDoesCauseExist_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkDoesCauseExist.Checked)
            {
                txtCauseDesc.Text = "";
                txtCauseDesc.ReadOnly = false;
            }
            else
            {
                txtCauseDesc.Text = "N/A";
                txtCauseDesc.ReadOnly = true;
            }
        }

        private void chkIsInvConclusive_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkIsInvConclusive.Checked)
            {
                txtIsInvConclusiveDesc.Text = "";
            }
            else
            {
                txtIsInvConclusiveDesc.Text = "From the investigation, no laboratory error or other assignable cause was found within Gibraltar.";
            }
        }        

        // MY 05/06/2015 - Start: Date events          
        private void mskDateObserved_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(210, 163);
        }

        private void mskCalDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(449, 249);          
        }

        private void mskMntcDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(449, 246);
        }

        private void mskPrepDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(450, 249);
        }

        private void mskExpDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(450, 246);
        }

        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (pnlCalendar.Location == new Point(210, 163))
            {
                mskDateObserved.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            else if (pnlCalendar.Location == new Point(449, 249))
            {
                mskCalDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            else if (pnlCalendar.Location == new Point(449, 246))
            {
                mskMntcDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            else if (pnlCalendar.Location == new Point(450, 249))
            {
                mskPrepDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            else if (pnlCalendar.Location == new Point(450, 246))
            {
                mskExpDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            pnlCalendar.Visible = false;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }        

        public void ConnectionError()
        {
            MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please contact IT.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
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
                if (nMode == 0)  //Enable Delete button for FA access  -ST 07/28/2017
                {
                    tsbDelete.Enabled = true;
                }
            }
        }
    }
}
