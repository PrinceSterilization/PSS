//Equipment.cs
// AUTHOR       : MARIA YOUNES
// TITLE        : Senior Programmer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Equipment Master File Maintenance

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
//using CrystalDecisions.ReportSource;

namespace PSS
{
    public partial class Equipment : PSS.TemplateForm
    {

        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";
        private Int16 nDSw = 0; //Details File Maintenance switch
        
        DataTable dtDepartments = new DataTable();                                        // MY 01/05/2015 - Pop-up GridView Departments query
        DataTable dtLocations = new DataTable();                                          // MY 01/05/2015 - Pop-up GridView Locations query
        DataTable dtEqptTypes = new DataTable();                                          // MY 01/05/2015 - Pop-up GridView EqptTypes query
        DataTable dtManufacturers = new DataTable();                                      // MY 01/05/2015 - Pop-up GridView Manufacturers query
        DataTable dtSrvcTypes = new DataTable();                                          // MY 02/12/2015 - Pop-up GridView Service Types query
        DataTable dtVendors = new DataTable();                                            // MY 02/12/2015 - Pop-up GridView Vendors query
        DataTable dtMaster = new DataTable();                                             // MY 06/29/2015 - datatable for EquipmentMaster
        DataTable dtEqptDetail = new DataTable();                                         // MY 02/11/2015 - datatable for EquipmentDetail
        DataTable dtServiceFreq = new DataTable();                                        // AMDC 09/01/2017 - datatable for Equipment Type Service Frequency

        public Equipment()
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
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
            chkShowInactive.Click += new EventHandler(chkShowInactiveClickHandler);
        }

        private void LoadRecords(byte cEqptStatus)
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Calibration.EqptMaster(cEqptStatus);

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
            dgvDeptNames.Visible = false; dgvLocationNames.Visible = false; dgvMftrNames.Visible = false;
            dgvEqptTypes.Visible = false; dgvSrvcNames.Visible = false; dgvVendorNames.Visible = false;
            AddEditMode(false);

            FileAccess();
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

        private void FileAccess()
        {
            //strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Equipment");

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
        }

        private void LoadDepartments()
        {
            dgvDeptNames.DataSource = null;

            dtDepartments = PSSClass.Calibration.EqptDepartments();
            if (dtDepartments == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvDeptNames.DataSource = dtDepartments;
            StandardDGVSetting(dgvDeptNames);
            dgvDeptNames.Columns[0].Width = 304;
            dgvDeptNames.Columns[1].Visible = false;
        }

        private void LoadLocations()
        {
            dgvLocationNames.DataSource = null;

            dtLocations = PSSClass.Calibration.EqptLocations();
            if (dtLocations == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvLocationNames.DataSource = dtLocations;
            StandardDGVSetting(dgvLocationNames);
            dgvLocationNames.Columns[0].Width = 304;
            dgvLocationNames.Columns[1].Visible = false;
        }

        private void LoadTypes()
        {
            dgvEqptTypes.DataSource = null;

            dtEqptTypes = PSSClass.Calibration.EqptTypes();
            if (dtEqptTypes == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvEqptTypes.DataSource = dtEqptTypes;
            StandardDGVSetting(dgvEqptTypes);
            dgvEqptTypes.Columns[0].Width = 304;
            dgvEqptTypes.Columns[1].Visible = false;
        }

        private void LoadManufacturers()
        {
            dgvMftrNames.DataSource = null;

            dtManufacturers = PSSClass.Calibration.EqptManufacturers();
            if (dtManufacturers == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvMftrNames.DataSource = dtManufacturers;
            StandardDGVSetting(dgvMftrNames);
            dgvMftrNames.Columns[0].Width = 304;
            dgvMftrNames.Columns[1].Visible = false;
        }

        private void LoadServiceTypes()
        {
            dtSrvcTypes = PSSClass.Calibration.EqptSrvcTypes();
            if (dtSrvcTypes == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvSrvcNames.DataSource = dtSrvcTypes;
            StandardDGVSetting(dgvSrvcNames);
            dgvSrvcNames.Columns[0].Width = 164;
            dgvSrvcNames.Columns[1].Visible = false;
        }

        private void LoadVendors()
        {
            dtVendors = PSSClass.Calibration.EqptVendors();
            if (dtVendors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvVendorNames.DataSource = dtVendors;
            StandardDGVSetting(dgvVendorNames);
            dgvVendorNames.Columns[0].Width = 164;
            dgvVendorNames.Columns[1].Visible = false;
        }

        private void CreateMasterStructure()
        {
            // Create Master Data table for Add/Edit/Delete functions            
            dtMaster.Columns.Add("EqptCode", typeof(string));
            dtMaster.Columns.Add("EqptDesc", typeof(string));
            dtMaster.Columns.Add("Category", typeof(string));
            dtMaster.Columns.Add("EqptBookNo", typeof(string));
            dtMaster.Columns.Add("UsageBookNo", typeof(Int16));
            dtMaster.Columns.Add("InvoiceNo", typeof(Int64));
            dtMaster.Columns.Add("DeptCode", typeof(string));
            dtMaster.Columns.Add("DeptName", typeof(string));
            dtMaster.Columns.Add("LocationID", typeof(string));
            dtMaster.Columns.Add("Location", typeof(string));
            dtMaster.Columns.Add("EqptType", typeof(Int16));
            dtMaster.Columns.Add("TypeName", typeof(string));
            dtMaster.Columns.Add("MftrID", typeof(Int16));
            dtMaster.Columns.Add("MftrName", typeof(string));
            dtMaster.Columns.Add("Notes", typeof(string));
            dtMaster.Columns.Add("SerialNo", typeof(string));
            dtMaster.Columns.Add("ModelNo", typeof(string));
            dtMaster.Columns.Add("TempSetting", typeof(string));
            dtMaster.Columns.Add("TempRange", typeof(string));
            dtMaster.Columns.Add("SoftwareVer", typeof(string));
            //dtMaster.Columns.Add("OpSystem", typeof(string));
            //dtMaster.Columns.Add("CPU", typeof(string));
            //dtMaster.Columns.Add("Memory", typeof(string));
            //dtMaster.Columns.Add("HardDrive", typeof(string));
            dtMaster.Columns.Add("FreqPM", typeof(Int16));
            dtMaster.Columns.Add("FreqMapping", typeof(Int16));
            dtMaster.Columns.Add("FreqCalibration", typeof(Int16));
            dtMaster.Columns.Add("DateRetired", typeof(DateTime));
            dtMaster.Columns.Add("Inactive", typeof(bool));
            dtMaster.Columns.Add("CreatedByID", typeof(Int16));
            dtMaster.Columns.Add("CreatedByName", typeof(string));
            dtMaster.Columns.Add("DateCreated", typeof(DateTime));
            dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            dtMaster.Columns.Add("LastUserID", typeof(Int16));
            bsMaster.DataSource = dtMaster;
            BindMaster();
        }

        private void CreateDetailStructure()
        {
            // Create Detail Data table for Add/Edit/Delete functions            
            dtEqptDetail.Columns.Add("DateCreated", typeof(DateTime));
            dtEqptDetail.Columns.Add("EqptDetailID", typeof(Int16));
            dtEqptDetail.Columns.Add("EqptCode", typeof(string));
            dtEqptDetail.Columns.Add("ServiceType", typeof(string));
            dtEqptDetail.Columns.Add("ServiceDate", typeof(DateTime));
            dtEqptDetail.Columns.Add("VendorName", typeof(string));
            dtEqptDetail.Columns.Add("VendorID", typeof(Int16));
            dtEqptDetail.Columns.Add("Notes", typeof(string));
            dtEqptDetail.Columns.Add("ServiceName", typeof(string));
            dtEqptDetail.Columns.Add("CreatedByName", typeof(string));
            dtEqptDetail.Columns.Add("CreatedByID", typeof(Int16));
            bsEqptDetail.DataSource = dtEqptDetail;
            bnEquipments.BindingSource = bsEqptDetail;
            BindDetail();
        }

        private void BindMaster()
        {
            // Clear bindings first
            //foreach (Control c in pnlRecord.Controls)
            //{
            //    c.DataBindings.Clear();
            //}

            txtEqptCode.DataBindings.Add("Text", bsMaster,"EqptCode");
            txtEqptDesc.DataBindings.Add("Text", bsMaster,"EqptDesc");
            txtCategory.DataBindings.Add("Text", bsMaster,"Category");
            txtBookNo.DataBindings.Add("Text", bsMaster,"EqptBookNo");
            txtUsageBookNo.DataBindings.Add("Text", bsMaster, "UsageBookNo");
            txtInvoiceNo.DataBindings.Add("Text", bsMaster, "InvoiceNo");
            txtDeptCode.DataBindings.Add("Text", bsMaster,"DeptCode");
            txtDeptName.DataBindings.Add("Text", bsMaster,"DeptName");
            txtLocID.DataBindings.Add("Text", bsMaster,"LocationID");
            txtLocName.DataBindings.Add("Text", bsMaster,"Location");
            txtTypeID.DataBindings.Add("Text", bsMaster,"EqptType");
            txtEqptType.DataBindings.Add("Text", bsMaster,"TypeName");
            txtMftrID.DataBindings.Add("Text", bsMaster,"MftrID");
            txtMftrName.DataBindings.Add("Text", bsMaster,"MftrName");
            txtNotes.DataBindings.Add("Text", bsMaster,"Notes");
            txtSerialNo.DataBindings.Add("Text", bsMaster,"SerialNo");
            txtModelNo.DataBindings.Add("Text", bsMaster,"ModelNo");
            txtTempSetting.DataBindings.Add("Text", bsMaster,"TempSetting");
            txtTempRange.DataBindings.Add("Text", bsMaster, "TempRange");
            txtSoftwareVer.DataBindings.Add("Text", bsMaster, "SoftwareVer");
            //txtOpSystem.DataBindings.Add("Text", bsMaster, "OpSystem");
            //txtCPU.DataBindings.Add("Text", bsMaster,"CPU");
            //txtMemory.DataBindings.Add("Text", bsMaster,"Memory");
            //txtHDrive.DataBindings.Add("Text", bsMaster, "HardDrive");
            txtFreqPM.DataBindings.Add("Text", bsMaster, "FreqPM");
            txtFreqMapping.DataBindings.Add("Text", bsMaster, "FreqMapping");
            txtFreqCalibration.DataBindings.Add("Text", bsMaster, "FreqCalibration");
            //chkInActive.DataBindings.Add("Checked", bsMaster, "IsActive");
            chkInActive.DataBindings.Add("Checked", bsMaster, "Inactive");

            txtCreatedByID.DataBindings.Add("Text", bsMaster, "CreatedByID");
            txtEnteredByName.DataBindings.Add("Text", bsMaster, "CreatedByName");
           
            Binding DateCreatedBinding;
            DateCreatedBinding = new Binding("Text", bsMaster, "DateCreated");
            DateCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtEntryDate.DataBindings.Add(DateCreatedBinding);

            Binding DateRetiredBinding;
            DateRetiredBinding = new Binding("Text", bsMaster, "DateRetired");
            DateRetiredBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskDateRetired.DataBindings.Add(DateRetiredBinding);     
        }

        private void BindDetail()
        {
            foreach (Control c in pnlEqptDetail.Controls)
            {
                c.DataBindings.Clear();
            }

            txtEqptDetailID.DataBindings.Add("Text", bsEqptDetail, "EqptDetailID");
            txtCreatedByName.DataBindings.Add("Text", bsEqptDetail, "CreatedByName");
            txtSrvcType.DataBindings.Add("Text", bsEqptDetail, "ServiceType");
            txtSrvcName.DataBindings.Add("Text", bsEqptDetail, "ServiceName");
            txtVendorID.DataBindings.Add("Text", bsEqptDetail, "VendorID");
            txtVendorName.DataBindings.Add("Text", bsEqptDetail, "VendorName");
            txtDetailNotes.DataBindings.Add("Text", bsEqptDetail, "Notes");

            Binding DateServiceBinding;
            DateServiceBinding = new Binding("Text", bsEqptDetail, "ServiceDate");
            DateServiceBinding.Format += new ConvertEventHandler(DateBinding_Format);
            mskServiceDate.DataBindings.Add(DateServiceBinding);            
        }

        private void DateBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "";
        }

        private void InsertEqptServiceDetail()
        {
            if (nMode == 3)
                txtEqptDetailID.Text = PSSClass.DataEntry.NewID("EquipmentDetails", "EqptDetailID").ToString();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@EqptDetailID", Convert.ToInt16(txtEqptDetailID.Text));
            sqlcmd.Parameters.AddWithValue("@EqptCode", txtEqptCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@ServiceType", txtSrvcType.Text.Trim());            
            sqlcmd.Parameters.AddWithValue("@ServiceDate", Convert.ToDateTime(mskServiceDate.Text));            
            sqlcmd.Parameters.AddWithValue("@VendorID", Convert.ToInt16(txtVendorID.Text));
            sqlcmd.Parameters.AddWithValue("@Notes", txtDetailNotes.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@UserID", 0);
            //sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditEqptDetail";
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
            ToolStripMenuItem[] items = new ToolStripMenuItem[3];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "EquipmentServiceReport";
            items[0].Text = "Equipment Service Report";
            items[0].Click += new EventHandler(PrintServiceReportClickHandler);

            items[1] = new ToolStripMenuItem();
            items[1].Name = "EquipmentValMstrPlan";
            items[1].Text = "Equipment Validation Master Plan";
            items[1].Click += new EventHandler(PrintValMstrPlanClickHandler);

            items[2] = new ToolStripMenuItem();
            items[2].Name = "EquipmentServiceRecord";
            items[2].Text = "Equipment Service Record";
            items[2].Click += new EventHandler(PrintServiceRecordClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Calibration.EqptMaster(0);

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
        private void PrintValMstrPlanClickHandler(object sender, EventArgs e)
        {
            EquipmentRpt rpt = new EquipmentRpt();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "EqptValMstrPlan";
            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void PrintServiceReportClickHandler(object sender, EventArgs e)
        {
            EquipmentServiceReport rpt = new EquipmentServiceReport();
            rpt.WindowState = FormWindowState.Maximized;
            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void PrintServiceRecordClickHandler(object sender, EventArgs e)
        {
            if (txtEqptCode.Text == "")
            {
                MessageBox.Show("Please open equipment data" + Environment.NewLine + "and select a service type.", Application.ProductName);
                return;
            }
            if (dgvFrequency.Rows.Count == 0 || dgvFrequency.Rows[0].Cells["ServiceType"].Value == null ||
                dgvFrequency.Rows[0].Cells["ServiceType"].Value.ToString() == "")
            {
                MessageBox.Show("No service type data selected.", Application.ProductName);
                return;
            }
            EquipmentRpt rpt = new EquipmentRpt();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "EqptServiceRecord";
            rpt.pubEqptCode = txtEqptCode.Text;
            rpt.pubEqptSrvcType = dgvFrequency.Rows[dgvFrequency.CurrentCell.RowIndex].Cells["ServiceType"].Value.ToString(); 
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
                    bsFile.Filter = "EqptCode <> ''";
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
            bsFile.Filter = "EqptCode <> ''";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }       

        private void LoadData()
        {
            nMode = 0;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            btnClose.Visible = true; btnClose.BringToFront();

            dgvFile.Rows[0].Selected = true;
            txtEqptCode.Text = dgvFile.CurrentRow.Cells["EqptCode"].Value.ToString();

            LoadMaster(Convert.ToString(txtEqptCode.Text));
            LoadEquipmentDetail(Convert.ToString(txtEqptCode.Text));
            OpenControls(pnlRecord, false);
            OpenControls(pnlEqptDetail, false);
            //if (dgvEqptDetail.RowCount != 0)
            //{
                btnAddDetail.Visible = true; btnDeleteDetail.Visible = true; btnOKDetail.Visible = false; btnCancelDetail.Visible = false;
            //}
            lblAddDetail.Visible = false;
            dgvFrequency.Enabled = true;
        }

        private void LoadMaster(string cEqptCode)
        {
            dtMaster = PSSClass.Calibration.EqptMain(Convert.ToString(txtEqptCode.Text));
            bsMaster.DataSource = dtMaster;
            dgvFrequency.DataSource = null;
            try
            {
                dtServiceFreq = PSSClass.Calibration.EqptServiceFreq(Convert.ToInt16(txtTypeID.Text));
                dgvFrequency.DataSource = dtServiceFreq;
                dgvFrequency.Columns["ServiceType"].HeaderText = "SERVICE TYPE";
                dgvFrequency.Columns["ServiceType"].Width = 125;
                dgvFrequency.Columns["FreqNo"].HeaderText = "FREQUENCY";
                dgvFrequency.Columns["FreqNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFrequency.Columns["FreqNo"].Width = 80;
                dgvFrequency.Columns["FreqMeasure"].HeaderText = "TIME MEASURE";
                dgvFrequency.Columns["FreqMeasure"].Width = 80;
                dgvFrequency.Columns["ServiceName"].Visible = false;
                StandardDGVSetting(dgvFrequency);
                dgvFrequency.Enabled = true;
            }
            catch { }
        }

        private void LoadEquipmentDetail(String cEqptCode)
        {
            dtEqptDetail = null;
            dtEqptDetail = PSSClass.Calibration.EqptDetail(cEqptCode);
            bsEqptDetail.DataSource = dtEqptDetail;
            bnEquipments.BindingSource = bsEqptDetail;
            dgvEqptDetail.DataSource = bsEqptDetail;

            DataGridEqptDetailSetting();
            BindDetail();
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
            dgvFile.Columns["EqptCode"].HeaderText = "EQPT REF. NO.";
            dgvFile.Columns["EqptDesc"].HeaderText = "DESCRIPTION";
            dgvFile.Columns["Category"].HeaderText = "CATEGORY";
            dgvFile.Columns["EqptBookNo"].HeaderText = "IOQ BOOK NO.";
            dgvFile.Columns["UsageBookNo"].HeaderText = "USAGE BOOK NO";
            dgvFile.Columns["InvoiceNo"].HeaderText = "INVOICE NO.";
            dgvFile.Columns["DeptCode"].HeaderText = "DEPT CODE";
            dgvFile.Columns["DeptName"].HeaderText = "DEPARTMENT NAME";
            dgvFile.Columns["LocationID"].HeaderText = "LOCATION ID";
            dgvFile.Columns["Location"].HeaderText = "LOCATION";
            dgvFile.Columns["EqptType"].HeaderText = "EQPT TYPE";
            dgvFile.Columns["TypeName"].HeaderText = "TYPE";
            dgvFile.Columns["MftrID"].HeaderText = "MFTR ID";
            dgvFile.Columns["MftrName"].HeaderText = "MANUFACTURER";
            dgvFile.Columns["Notes"].HeaderText = "NOTES";
            dgvFile.Columns["SerialNo"].HeaderText = "SERIAL NO.";
            dgvFile.Columns["ModelNo"].HeaderText = "MODEL NO.";
            dgvFile.Columns["TempSetting"].HeaderText = "TEMP. SETTING";
            dgvFile.Columns["TempRange"].HeaderText = "TEMP. RANGE";
            dgvFile.Columns["SoftwareVer"].HeaderText = "SOFTWARE VER.";
            dgvFile.Columns["DateCreated"].HeaderText = "DATE CREATED";
            dgvFile.Columns["CreatedByName"].HeaderText = "ENTERED BY";
            dgvFile.Columns["SVDueDate"].HeaderText = "SV DUE";
            dgvFile.Columns["PMDueDate"].HeaderText = "PM DUE";
            dgvFile.Columns["MDueDate"].HeaderText = "VL DUE";
            dgvFile.Columns["CLDueDate"].HeaderText = "CL DUE";
            dgvFile.Columns["DateRetired"].HeaderText = "DATE RETIRED";
            dgvFile.Columns["InActive"].HeaderText = "INACTIVE";
            dgvFile.Columns["EqptDesc"].Width = 300;           
            dgvFile.Columns["Location"].Width = 250;
            dgvFile.Columns["TypeName"].Width = 150;
            dgvFile.Columns["MftrName"].Width = 200;
            dgvFile.Columns["SerialNo"].Width = 180;
            dgvFile.Columns["SVDueDate"].Width = 70;
            dgvFile.Columns["PMDueDate"].Width = 70;
            dgvFile.Columns["MDueDate"].Width = 70;
            dgvFile.Columns["CLDueDate"].Width = 70;
            dgvFile.Columns["DateRetired"].Width = 70;
            dgvFile.Columns["InActive"].Width = 50;
            dgvFile.Columns["Category"].Visible = false;
            dgvFile.Columns["UsageBookNo"].Visible = false;
            dgvFile.Columns["DeptCode"].Visible = false;
            dgvFile.Columns["LocationID"].Visible = false;
            dgvFile.Columns["InvoiceNo"].Visible = false;
            dgvFile.Columns["EqptType"].Visible = false;
            dgvFile.Columns["MftrID"].Visible = false;
            dgvFile.Columns["Notes"].Visible = false;
            dgvFile.Columns["TempRange"].Visible = false;
            dgvFile.Columns["SoftwareVer"].Visible = false;
            dgvFile.Columns["CreatedByID"].Visible = false;     
            dgvFile.Columns["DateCreated"].Visible = false;
            dgvFile.Columns["CreatedByName"].Visible = false;
            dgvFile.Columns["Inactive"].Visible = false;
            dgvFile.Columns["DateRetired"].Visible = false;
            dgvFile.Columns["DateRetired"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["ModelNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["PMDueDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["MDueDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["CLDueDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void DataGridEqptDetailSetting()
        {
            dgvEqptDetail.EnableHeadersVisualStyles = false;
            dgvEqptDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEqptDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvEqptDetail.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEqptDetail.Columns["DateCreated"].HeaderText = "Date Entered";
            dgvEqptDetail.Columns["EqptCode"].HeaderText = "Equipment Code";
            dgvEqptDetail.Columns["ServiceType"].HeaderText = "Srvc Type";            
            dgvEqptDetail.Columns["ServiceDate"].HeaderText = "Service Date";
            dgvEqptDetail.Columns["VendorName"].HeaderText = "Vendor Name";
            dgvEqptDetail.Columns["Notes"].HeaderText = "Notes";
            dgvEqptDetail.Columns["ServiceName"].HeaderText = "Srvc Name";
            dgvEqptDetail.Columns["CreatedByName"].HeaderText = "Created By";               
            dgvEqptDetail.Columns["VendorID"].HeaderText = "Vendor ID";
            dgvEqptDetail.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvEqptDetail.Columns["ServiceDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvEqptDetail.Columns["DateCreated"].Width = 70;
            dgvEqptDetail.Columns["ServiceType"].Width = 40;
            dgvEqptDetail.Columns["ServiceDate"].Width = 70;
            dgvEqptDetail.Columns["VendorName"].Width = 140;
            dgvEqptDetail.Columns["Notes"].Width = 130;  
            dgvEqptDetail.Columns["CreatedByName"].Width = 60;
            dgvEqptDetail.Columns["ServiceType"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEqptDetail.Columns["EqptDetailID"].Visible = false;
            dgvEqptDetail.Columns["EqptCode"].Visible = false;
            dgvEqptDetail.Columns["ServiceName"].Visible = false;
            dgvEqptDetail.Columns["VendorID"].Visible = false;
            dgvEqptDetail.Columns["CreatedByID"].Visible = false;
            if (dgvEqptDetail.RowCount > 0)
            {
                dgvEqptDetail.Rows[0].Selected = true;
            }
            BindDetail();
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
            ClearControls(this.pnlEqptDetail);
            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlEqptDetail, false);
            dtMaster.Rows.Clear();
            dtEqptDetail.Rows.Clear();            
            txtEqptCode.Focus();
            btnAddDetail.Visible = true; btnDeleteDetail.Visible = true; btnOKDetail.Visible = false; btnCancelDetail.Visible = false;
            btnClose.Visible = false;

            // Create Master Data Row
            dtMaster.Rows.Clear();
            DataRow dR = dtMaster.NewRow();

            dR["EqptCode"] = DBNull.Value;
            dR["EqptDesc"] = DBNull.Value;
            dR["Category"] = DBNull.Value;
            dR["EqptBookNo"] = DBNull.Value;
            dR["UsageBookNo"] = DBNull.Value;
            dR["InvoiceNo"] = DBNull.Value;
            dR["DeptCode"] = DBNull.Value;
            dR["DeptName"] = "";
            dR["LocationID"] = DBNull.Value;
            dR["Location"] = "";
            dR["EqptType"] = DBNull.Value;
            dR["TypeName"] = "";
            dR["MftrID"] = DBNull.Value;
            dR["MftrName"] = "";
            dR["Notes"] = DBNull.Value;
            dR["SerialNo"] = DBNull.Value;
            dR["ModelNo"] = "";
            dR["TempSetting"] = DBNull.Value;
            //dR["OpSystem"] = DBNull.Value;
            //dR["CPU"] = DBNull.Value;
            //dR["Memory"] = DBNull.Value;
            //dR["HardDrive"] = DBNull.Value;
            dR["FreqPM"] = DBNull.Value;
            dR["FreqMapping"] = DBNull.Value;
            dR["FreqCalibration"] = DBNull.Value;
            dR["DateRetired"] = DBNull.Value;
            dR["InActive"] = false;
            dR["CreatedByID"] = LogIn.nUserID;
            dR["CreatedByName"] = LogIn.strUserID;
            dR["DateCreated"] = DateTime.Now;
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;

            dtMaster.Rows.Add(dR);
            bsMaster.DataSource = dtMaster;

            //BindMaster();   
           
            txtEnteredByName.Enabled = false;
            txtEntryDate.Enabled = false;
            txtEntryDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
            txtCategory.Text = "LabE";

            //LoadEquipmentDetail(txtEqptCode.Text); - disabled 1/11/2018
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;

            LoadData();
            nMode = 2; nDSw = 2;
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();

            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlEqptDetail, true);
            txtEqptCode.Focus(); txtEqptCode.Enabled = false;
            AddEditMode(true);

            btnAddDetail.Visible = true; btnDeleteDetail.Visible = true; btnOKDetail.Visible = false; btnCancelDetail.Visible = false;
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

                sqlcmd.Parameters.AddWithValue("@EqptCode", txtEqptCode.Text.Trim());

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelEquipmentMaster";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadRecords(Convert.ToByte(chkShowInactive.CheckState));
        }

        private void DeleteDetail(int cDetailID)
        {           
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@EqptDetailID", cDetailID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelEquipmentDetail";

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

            // Detail Save Routine
            bsEqptDetail.EndEdit();
            //Remove Deleted Records
            dt = dtEqptDetail.GetChanges(DataRowState.Deleted);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DeleteDetail(Convert.ToInt16(dt.Rows[i]["DetailID"]));
                }
            }
            dt = null;
            // Validate if changes were made on the EQ Detail
            dt = dtEqptDetail.GetChanges();
            if (dt != null)
            {
                int nRet = ValidateDetail();                                                    // Validation for Detail Record
                if (nRet == 0)
                {
                    dt.Dispose();
                    return;
                }
                UpdateEqptDetails();                                                           // Save Detail Record
                dt.Dispose();
            }
            dt = null;
            dgvFile.Refresh();
            btnClose.Visible = true;
            nMode = 0;
            OpenControls(pnlRecord, false);
            byte bInactive = Convert.ToByte(chkInActive.CheckState);
            LoadRecords(Convert.ToByte(chkShowInactive.CheckState));
            if (bInactive != 1)
            {
                PSSClass.General.FindRecord("EqptDesc", txtEqptDesc.Text.Trim(), bsFile, dgvFile);
                LoadData();
            }
            MessageBox.Show("Record successfully saved!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void SaveMaster()
        {   
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@EqptCode", txtEqptCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@EqptDesc", txtEqptDesc.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Category", txtCategory.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@EqptBookNo", txtBookNo.Text.Trim());
            if (txtUsageBookNo.Text.Trim() == "")
                sqlcmd.Parameters.AddWithValue("@UsageBookNo", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@UsageBookNo", Convert.ToInt16(txtUsageBookNo.Text.Trim()));
            if (txtInvoiceNo.Text.Trim() == "")
                sqlcmd.Parameters.AddWithValue("@InvoiceNo", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@InvoiceNo", Convert.ToInt64(txtInvoiceNo.Text));
            sqlcmd.Parameters.AddWithValue("@DeptCode", txtDeptCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@LocationID", txtLocID.Text.Trim());
            if (txtTypeID.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@EqptType", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@EqptType", Convert.ToInt16(txtTypeID.Text));
            }
            if (txtMftrID.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@MftrID", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@MftrID", Convert.ToInt16(txtMftrID.Text));
            }            
            sqlcmd.Parameters.AddWithValue("@MftrName", txtMftrName.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Notes", txtNotes.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@SerialNo", txtSerialNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@ModelNo", txtModelNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@TempSetting", txtTempSetting.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@TempRange", txtTempRange.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@SoftwareVer", txtSoftwareVer.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@OpSystem", txtOpSystem.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CPU", txtCPU.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Memory", txtMemory.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@HardDrive", txtHDrive.Text.Trim());
            if (txtFreqPM.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@FreqPM", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@FreqPM", Convert.ToInt16(txtFreqPM.Text));
            }
            if (txtFreqMapping.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@FreqMapping", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@FreqMapping", Convert.ToInt16(txtFreqMapping.Text));
            }
            if (txtFreqCalibration.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@FreqCalibration", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@FreqCalibration", Convert.ToInt16(txtFreqCalibration.Text));
            }
            if (mskDateRetired.MaskFull == false)
            {
                sqlcmd.Parameters.AddWithValue("@DateRetired", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@DateRetired", Convert.ToDateTime(mskDateRetired.Text));
            }
            if (chkInActive.CheckState == CheckState.Checked)
                sqlcmd.Parameters.AddWithValue("@IsActive", false);
            else
                sqlcmd.Parameters.AddWithValue("@IsActive", 1);
            sqlcmd.Parameters.AddWithValue("@CreatedByID", LogIn.nUserID);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditEquipmentMaster";
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

        private static int SaveDetail(int cEqptDetailID, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
            sqlcmd.Parameters.AddWithValue("@EqptDetailID", cEqptDetailID);
            sqlcmd.Parameters.AddWithValue("@EqptCode", Convert.ToString(cDT.Rows[cI]["EqptCode"]));
            sqlcmd.Parameters.AddWithValue("@ServiceType", Convert.ToString(cDT.Rows[cI]["ServiceType"]));
            if (cDT.Rows[cI]["ServiceDate"].ToString() == "")
            {
                sqlcmd.Parameters.AddWithValue("@ServiceDate", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@ServiceDate", Convert.ToDateTime(cDT.Rows[cI]["ServiceDate"]));
            }
            sqlcmd.Parameters.AddWithValue("@VendorID", Convert.ToInt16(cDT.Rows[cI]["VendorID"]));
            sqlcmd.Parameters.AddWithValue("@Notes", Convert.ToString(cDT.Rows[cI]["Notes"]));
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditEquipmentDetail";
            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return 0;
            }
            sqlcnn.Dispose();
            return nSuccess;
        }

        private void UpdateEqptDetails()
        {                               
            DataTable dt = dtEqptDetail.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtEqptDetailID.Text = PSSClass.DataEntry.NewID("EquipmentDetails", "EqptDetailID").ToString();
                    SaveDetail(Convert.ToUInt16(txtEqptDetailID.Text), i, 1, dt);
                }
                dt.Rows.Clear();
            }
            dt = dtEqptDetail.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SaveDetail(Convert.ToUInt16(dt.Rows[i]["EqptDetailID"].ToString()), i, 2, dt);                    
                }
                dt.Rows.Clear();
            }
        }

        private int ValidateMaster()
        {
            if (txtEqptDesc.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a description!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtEqptDesc.Focus();
                return 0;
            }

            if (txtDeptName.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Department Name!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDeptCode.Focus();
                return 0;
            }

            if (txtLocName.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Location!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLocID.Focus();
                return 0;
            }

            if (txtEqptType.Text.Trim() == "")
            {
                MessageBox.Show("Please choose Equipment Type!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtTypeID.Focus();
                return 0;
            }

            if (txtMftrName.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Manufacturer!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMftrID.Focus();
                return 0;
            }
            return 1;
        }

        private int ValidateDetail()
        {       
            if (txtSrvcType.Text.Trim() == "")
            {
                MessageBox.Show("Please enter a Service Type!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSrvcType.Focus();
                return 0;
            }

            if (txtVendorID.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Vendor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtVendorID.Focus();
                return 0;
            }            
            return 1;
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
            LoadRecords(Convert.ToByte(chkShowInactive.CheckState));
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            nMode = 0;
        }

        private void Equipment_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "Equipment");

            LoadRecords(0);
            LoadDepartments();
            LoadLocations();
            LoadTypes();
            LoadManufacturers();
            LoadServiceTypes();
            LoadVendors();

            BuildPrintItems();
            BuildSearchItems();

            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            CreateMasterStructure();
            CreateDetailStructure();  
        }

        private void Equipment_KeyDown(object sender, KeyEventArgs e)
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
            dgvFile.Focus();
            AddEditMode(false);
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

        // MY 01/05/2015 - START: txt/dgvDeptNames events
        private void dgvDeptNames_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
                txtDeptCode.Text = dgvDeptNames.CurrentRow.Cells["DepartmentCode"].Value.ToString();
                dgvDeptNames.Visible = false;
            }
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
                txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
                txtDeptCode.Text = dgvDeptNames.CurrentRow.Cells["DepartmentCode"].Value.ToString();
                dgvDeptNames.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvDeptNames.Visible = false;
            }
        }
        private void txtDeptName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvDeptNames.Visible = true; dgvDeptNames.BringToFront();
            }
        }

        private void dgvDeptNames_Leave(object sender, EventArgs e)
        {
            dgvDeptNames.Visible = false;
        }

        private void txtDeptName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwDeptNames;
                dvwDeptNames = new DataView(dtDepartments, "DepartmentName like '" + txtDeptName.Text.Trim().Replace("'", "''") + "%'", "DepartmentName", DataViewRowState.CurrentRows);
                dvwSetUpWidth(dgvDeptNames, dvwDeptNames,304);
            }
        }

        private void dgvDeptNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
                txtDeptCode.Text = dgvDeptNames.CurrentRow.Cells["DepartmentCode"].Value.ToString();
                dgvDeptNames.Visible = false;
            }
        }

        private void picDepts_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadDepartments();
                dgvDeptNames.Visible = true; dgvDeptNames.BringToFront();
            }
        }

        private void txtDeptCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtDeptName.Text = PSSClass.Calibration.EqptDeptName(txtDeptCode.Text.Trim());
                    if (txtDeptName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Dept Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvDeptNames.Visible = false;
                }
                else
                {
                    txtDeptName.Text = ""; dgvDeptNames.Visible = false;
                }
            }
        }

        // MY 01/05/2015 - END: txt/dgvDeptNames events        

        // MY 01/05/2015 - START: txt/dgvLocationNames events
        private void dgvLocationNames_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtLocName.Text = dgvLocationNames.CurrentRow.Cells["LocationDesc"].Value.ToString();
                txtLocID.Text = dgvLocationNames.CurrentRow.Cells["LocationID"].Value.ToString();
                dgvLocationNames.Visible = false;
            }
        }

        private void dgvLocationNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvLocationNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtLocName.Text = dgvLocationNames.CurrentRow.Cells["LocationDesc"].Value.ToString();
                    txtLocID.Text = dgvLocationNames.CurrentRow.Cells["LocationID"].Value.ToString();
                    dgvLocationNames.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvLocationNames.Visible = false;
                }
            }
        }
        private void txtLocName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvLocationNames.Visible = true; dgvLocationNames.BringToFront();
            }
        }

        private void dgvLocationNames_Leave(object sender, EventArgs e)
        {
            dgvLocationNames.Visible = false;
        }

        private void txtLocName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwLocNames;
                dvwLocNames = new DataView(dtLocations, "LocationDesc like '" + txtLocName.Text.Trim().Replace("'", "''") + "%'", "LocationDesc", DataViewRowState.CurrentRows);
                dvwSetUpWidth(dgvLocationNames, dvwLocNames, 304);
            }
        }

        private void dgvLocationNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtLocName.Text = dgvLocationNames.CurrentRow.Cells["LocationDesc"].Value.ToString();
                txtLocID.Text = dgvLocationNames.CurrentRow.Cells["LocationID"].Value.ToString();
                dgvLocationNames.Visible = false;
            }
        }

        private void picLocations_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadLocations();
                dgvLocationNames.Visible = true; dgvLocationNames.BringToFront();
            }
        }

        private void txtLocID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtLocName.Text = PSSClass.Calibration.EqptLocName(txtLocID.Text.Trim());
                    if (txtLocName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Location ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvLocationNames.Visible = false;
                }
                else
                {
                    txtLocName.Text = ""; dgvLocationNames.Visible = false;
                }
            }
        }

        // MY 01/05/2015 - END: txt/dgvLocationNames events      

        // MY 01/05/2015 - START: txt/dgvEqptTypes events
        private void dgvEqptTypes_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtEqptType.Text = dgvEqptTypes.CurrentRow.Cells["TypeDesc"].Value.ToString();
                txtTypeID.Text = dgvEqptTypes.CurrentRow.Cells["TypeID"].Value.ToString();
                dgvEqptTypes.Visible = false;
            }
        }

        private void dgvEqptTypes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvEqptTypes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtEqptType.Text = dgvEqptTypes.CurrentRow.Cells["TypeDesc"].Value.ToString();
                    txtTypeID.Text = dgvEqptTypes.CurrentRow.Cells["TypeID"].Value.ToString();
                    dgvEqptTypes.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvEqptTypes.Visible = false;
                }
            }
        }
        private void txtEqptType_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvEqptTypes.Visible = true; dgvEqptTypes.BringToFront();
            }
        }

        private void dgvEqptTypes_Leave(object sender, EventArgs e)
        {
            dgvEqptTypes.Visible = false;
        }

        private void txtEqptType_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwEqptTypes;
                dvwEqptTypes = new DataView(dtEqptTypes, "TypeDesc like '" + txtEqptType.Text.Trim().Replace("'", "''") + "%'", "TypeDesc", DataViewRowState.CurrentRows);
                dvwSetUpWidth(dgvEqptTypes, dvwEqptTypes, 304);
            }
        }

        private void dgvEqptTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtEqptType.Text = dgvEqptTypes.CurrentRow.Cells["TypeDesc"].Value.ToString();
                txtTypeID.Text = dgvEqptTypes.CurrentRow.Cells["TypeID"].Value.ToString();
                dgvEqptTypes.Visible = false;
            }
        }

        private void picTypes_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadTypes();
                dgvEqptTypes.Visible = true; dgvEqptTypes.BringToFront();
            }
        }

        private void txtTypeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                    return;
                }
                if (e.KeyChar == 13)
                {
                    txtEqptType.Text = PSSClass.Calibration.EqptTypeDesc(Convert.ToInt16(txtTypeID.Text));
                    if (txtEqptType.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Type ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvEqptTypes.Visible = false;
                }
                else
                {
                    txtEqptType.Text = ""; dgvEqptTypes.Visible = false;
                }
            }
        }

        // MY 01/05/2015 - END: txt/dgvEqptTypes events  

        // MY 01/05/2015 - START: txt/dgvMftrNames events
        private void dgvMftrNames_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtMftrName.Text = dgvMftrNames.CurrentRow.Cells["MftrName"].Value.ToString();
                txtMftrID.Text = dgvMftrNames.CurrentRow.Cells["MftrID"].Value.ToString();
                dgvMftrNames.Visible = false;
            }
        }

        private void dgvMftrNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvMftrNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtMftrName.Text = dgvMftrNames.CurrentRow.Cells["MftrName"].Value.ToString();
                    txtMftrID.Text = dgvMftrNames.CurrentRow.Cells["MftrID"].Value.ToString();
                    dgvMftrNames.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvMftrNames.Visible = false;
                }
            }
        }
        private void txtMftrName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvMftrNames.Visible = true; dgvMftrNames.BringToFront();
            }
        }

        private void dgvMftrNames_Leave(object sender, EventArgs e)
        {
            dgvMftrNames.Visible = false;
        }

        private void txtMftrName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwMftrNames;
                dvwMftrNames = new DataView(dtManufacturers, "MftrName like '%" + txtMftrName.Text.Trim().Replace("'", "''") + "%'", "MftrName", DataViewRowState.CurrentRows);
                dvwSetUpWidth(dgvMftrNames, dvwMftrNames, 304);
            }
        }

        private void txtMftrName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtMftrID.Text = "";
                txtNotes.Select();
                txtNotes.Focus();
            }
            else if (e.KeyChar == 27)
            {
                dgvMftrNames.Visible = false;
            }
        }

        private void dgvMftrNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtMftrName.Text = dgvMftrNames.CurrentRow.Cells["MftrName"].Value.ToString();
                txtMftrID.Text = dgvMftrNames.CurrentRow.Cells["MftrID"].Value.ToString();
                dgvMftrNames.Visible = false;
            }
        }

        private void picMftrNames_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadManufacturers();
                dgvMftrNames.Visible = true; dgvMftrNames.BringToFront();
            }
        }

        private void txtMftrID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                    return;
                }
                if (e.KeyChar == 13)
                {
                    txtMftrName.Text = PSSClass.Calibration.EqptMftrName(Convert.ToInt16(txtMftrID.Text));
                    if (txtMftrName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Manufacturer ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvMftrNames.Visible = false; 
                }
                else
                {
                    txtMftrName.Text = ""; dgvMftrNames.Visible = false;
                }
            }
        }

        // MY 01/05/2015 - END: txt/dgvMftrNames events  

        // MY 02/12/2015 - START: txt/dgvSrvcNames events
        private void dgvSrvcNames_DoubleClick(object sender, EventArgs e)
        {
            if (nDSw != 0)
            {
                txtSrvcName.Text = dgvSrvcNames.CurrentRow.Cells["ServiceName"].Value.ToString();
                txtSrvcType.Text = dgvSrvcNames.CurrentRow.Cells["ServiceType"].Value.ToString();
                dgvSrvcNames.Visible = false;
            }
        }

        private void dgvSrvcNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nDSw != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtSrvcName.Text = dgvSrvcNames.CurrentRow.Cells["ServiceName"].Value.ToString();
                    txtSrvcType.Text = dgvSrvcNames.CurrentRow.Cells["ServiceType"].Value.ToString();
                    dgvSrvcNames.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvSrvcNames.Visible = false;
                }
            }
        }
        private void txtSrvcName_Enter(object sender, EventArgs e)
        {
            if (nDSw != 0)
            {
                dgvSrvcNames.Visible = true; dgvSrvcNames.BringToFront(); dgvVendorNames.Visible = false;
            }
        }

        private void dgvSrvcNames_Leave(object sender, EventArgs e)
        {
            dgvSrvcNames.Visible = false;
        }

        private void txtSrvcName_TextChanged(object sender, EventArgs e)
        {
            if (nDSw != 0)
            {
                DataView dvwSrvcNames;
                dvwSrvcNames = new DataView(dtSrvcTypes, "ServiceName like '" + txtSrvcName.Text.Trim().Replace("'", "''") + "%'", "ServiceName", DataViewRowState.CurrentRows);
                dvwSetUpWidth(dgvSrvcNames, dvwSrvcNames, 166);
            }
        }

        private void picSrvcTypes_Click(object sender, EventArgs e)
        {
            if (nDSw != 0)
            {
                LoadServiceTypes(); ;
                dgvSrvcNames.Visible = true; dgvSrvcNames.BringToFront();
            }
        }

        private void txtSrvcType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nDSw != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtSrvcName.Text = PSSClass.Calibration.EqptSrvcName(txtSrvcType.Text);
                    if (txtSrvcName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Service Type found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvSrvcNames.Visible = false;
                }
                else
                {
                    txtSrvcName.Text = ""; dgvSrvcNames.Visible = false;
                }
            }
        }

        // MY 02/12/2015 - END: txt/dgvSrvcNames events  

        // MY 02/13/2015 - START: txt/dgvVendorNames events
        private void dgvVendorNames_DoubleClick(object sender, EventArgs e)
        {
            if (nDSw != 0)
            {
                txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
                txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
                dgvVendorNames.Visible = false;
            }
        }
       
       private void dgvVendorNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nDSw != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
                    txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
                    dgvVendorNames.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvVendorNames.Visible = false;
                }
            }
        }
        private void txtVendorName_Enter(object sender, EventArgs e)
        {
            if (nDSw != 0)
            {
                dgvVendorNames.Visible = true; dgvVendorNames.BringToFront(); dgvSrvcNames.Visible = false;
            }
        }

        private void dgvVendorNames_Leave(object sender, EventArgs e)
        {
            dgvVendorNames.Visible = false;
        }

        private void txtVendorName_TextChanged(object sender, EventArgs e)
        {
            if (nDSw != 0)
            {
                DataView dvwVendorNames;
                dvwVendorNames = new DataView(dtVendors, "VendorName like '%" + txtVendorName.Text.Trim().Replace("'", "''") + "%'", "VendorName", DataViewRowState.CurrentRows);
                dvwSetUpWidth(dgvVendorNames, dvwVendorNames,166);
            }
        }

        private void picVendors_Click(object sender, EventArgs e)
        {
            if (nDSw != 0)
            {
                LoadVendors(); ;
                dgvVendorNames.Visible = true; dgvVendorNames.BringToFront();
            }
        }

        private void txtVendorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nDSw != 0)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                    return;
                }
                if (e.KeyChar == 13)
                {
                    txtVendorName.Text = PSSClass.Calibration.EqptVendorName(Convert.ToInt16(txtVendorID.Text));
                    if (txtVendorName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Vendor found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvVendorNames.Visible = false;
                }
                else
                {
                    txtVendorName.Text = ""; dgvVendorNames.Visible = false;
                }
            }
        }
        // MY 02/13/2015 - END: txt/dgvVendorNames events         

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            ClearControls(this.pnlEqptDetail);
            OpenControls(this.pnlEqptDetail, true);
            pnlEqptDetail.Visible = true;

            btnAddDetail.Visible = false; btnOKDetail.Visible = true; btnCancelDetail.Visible = true; btnDeleteDetail.Visible = false;
            btnClose.Visible = false; lblAddDetail.Visible = true;

            AddEditMode(true);

            foreach (Control c in pnlEqptDetail.Controls)
            {
                c.DataBindings.Clear();
            }
            txtCreatedByID.Text = LogIn.nUserID.ToString();
            txtCreatedByName.Text = PSSClass.Users.UserName(LogIn.nUserID);
            mskServiceDate.Text = DateTime.Now.ToShortDateString();
            txtCreatedByName.ReadOnly = true;
            mskServiceDate.Focus();
            nDSw = 1;
        }       

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            int Row = dgvEqptDetail.CurrentRow.Index;
            int intDetailID;

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
            else
            { 
                dReply = new DialogResult();
                dReply = MessageBox.Show("Selected service record will be deleted" + Environment.NewLine + "permanently. Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            intDetailID = Convert.ToInt16(txtEqptDetailID.Text);           
            dgvEqptDetail.Rows.RemoveAt(Row);
            if (dgvEqptDetail.Rows.Count == 0)
            {
                btnDeleteDetail.Enabled = false;
            }
            DeleteDetail(intDetailID);
        }

        private void btnOKDetail_Click(object sender, EventArgs e)
        {
            int nRet = ValidateDetail();  // Validation for Detail Record
            if (nRet == 0)
            {
                return;
            }
            if (nDSw == 1)
            {
                DataRow dR = dtEqptDetail.NewRow();
                dR["DateCreated"] = DateTime.Now.ToShortDateString();
                dR["EqptDetailID"] = 0;
                dR["EqptCode"] = txtEqptCode.Text;
                dR["CreatedByName"] = PSSClass.Users.UserName(LogIn.nUserID); ;
                dR["CreatedByID"] = Convert.ToInt16(txtCreatedByID.Text);
                dR["ServiceType"] = txtSrvcType.Text;
                dR["ServiceName"] = txtSrvcName.Text;
                dR["VendorID"] = txtVendorID.Text;
                dR["VendorName"] = txtVendorName.Text;
                dR["Notes"] = txtDetailNotes.Text.Trim();
                dR["ServiceDate"] = Convert.ToDateTime(mskServiceDate.Text);
                //dtEqptDetail.Rows.Add(dR);
                dtEqptDetail.Rows.InsertAt(dR, 0);//10-22-2017 AMDC
            }
            else 
            {
                bsEqptDetail.EndEdit();
            }
            bsEqptDetail.DataSource = dtEqptDetail;
            bnEquipments.BindingSource = bsEqptDetail;
            dgvEqptDetail.DataSource = bsEqptDetail;
            DataGridEqptDetailSetting();
            if (nDSw == 1)
            {
                //bsEqptDetail.Position = dtEqptDetail.Rows.Count - 1;
                bsEqptDetail.Position = 0;//10-22-2017 AMDC
                BindDetail();
            }
            nDSw = 0;
            btnAddDetail.Visible = true; btnOKDetail.Visible = false; btnCancelDetail.Visible = false; btnDeleteDetail.Visible = true;
            lblAddDetail.Visible = false;
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

        private void txtInvoiceNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }
        
        private void mskDateRetired_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(725, 139);
        }

        private void mskServiceDate_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(684, 231);
        }

        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (pnlCalendar.Location == new Point(725, 139))
            {
                mskDateRetired.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            else if (pnlCalendar.Location == new Point(684, 231))
            {
                mskServiceDate.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }           

            pnlCalendar.Visible = false;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false;
        }

        private void txtEqptCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (txtEqptCode.Text.Trim() != "")
                {
                   
                    // Check if Eqpt Code exists in Equipment Master table
                    bool isExists = false;

                    isExists = PSSClass.Calibration.EqptCodeExists(txtEqptCode.Text);

                    if (isExists)
                    {
                        MessageBox.Show("This Equipment Reference Code already exists. Please try again!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        SendKeys.Send("{tab}");
                    }
                }
            }
        }

        private void txtUsageBookNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
               !char.IsDigit(e.KeyChar) &&
               e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void dgvSrvcNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void txtVendorID_Enter(object sender, EventArgs e)
        {
            dgvVendorNames.Visible = false;
        }

        private void btnCancelDetail_Click(object sender, EventArgs e)
        {
            btnAddDetail.Visible = true; btnOKDetail.Visible = false; btnCancelDetail.Visible = false; btnDeleteDetail.Visible = true;
            lblAddDetail.Visible = false;

            bsEqptDetail.DataSource = dtEqptDetail;
            bnEquipments.BindingSource = bsEqptDetail;
            dgvEqptDetail.DataSource = bsEqptDetail;
            DataGridEqptDetailSetting();
            bsEqptDetail.Position = dtEqptDetail.Rows.Count - 1;
            nDSw = 0;
        }

        private void txtDeptCode_Leave(object sender, EventArgs e)
        {
            if (nMode != 0 && txtDeptCode.Text.Trim() != "")
            {
                txtDeptName.Text = PSSClass.Calibration.EqptDeptName(txtDeptCode.Text.Trim());
                if (txtDeptName.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Dept Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtDeptCode.Text = ""; txtDeptCode.Focus();
                }
            }
        }

        private void txtLocID_Leave(object sender, EventArgs e)
        {
            if (nMode != 0 && txtLocID.Text.Trim() != "")
            {
                try
                {
                    txtLocName.Text = PSSClass.Calibration.EqptLocName(txtLocID.Text.Trim());
                    if (txtLocName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Location ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtLocID.Text = ""; txtLocID.Focus();
                    }
                }
                catch { }
            }
        }

        private void txtTypeID_Leave(object sender, EventArgs e)
        {
            if (nMode != 0 && txtTypeID.Text.Trim() != "")
            {
                try
                {
                    txtEqptType.Text = PSSClass.Calibration.EqptTypeDesc(Convert.ToInt16(txtTypeID.Text));
                    if (txtEqptType.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Type ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtTypeID.Text = ""; txtTypeID.Focus();
                    }
                }
                catch { }
            }
        }

        private void txtMftrID_Leave(object sender, EventArgs e)
        {
            if (nMode != 0 && txtMftrID.Text.Trim() != "")
            {
                try
                {
                    txtMftrName.Text = PSSClass.Calibration.EqptMftrName(Convert.ToInt16(txtMftrID.Text));
                    if (txtMftrName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Manufacturer ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtMftrID.Text = ""; txtMftrID.Focus();
                    }
                }
                catch { }
            }
        }

        private void txtVendorID_Leave(object sender, EventArgs e)
        {
            if (nDSw != 0 && txtVendorID.Text.Trim() != "")
            {
                try
                {
                    txtVendorName.Text = PSSClass.Calibration.EqptVendorName(Convert.ToInt16(txtVendorID.Text));
                    if (txtVendorName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Vendor found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtVendorID.Text = ""; txtVendorID.Focus();
                        return;
                    }
                }
                catch { }
            }
        }

        private void txtSrvcType_Leave(object sender, EventArgs e)
        {
            if (nDSw != 0 && txtSrvcType.Text.Trim() != "")
            {
                txtSrvcName.Text = PSSClass.Calibration.EqptSrvcName(txtSrvcType.Text);
                if (txtSrvcName.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Service Type found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtSrvcType.Text = ""; txtSrvcType.Focus();
                    return;
                }
                dgvSrvcNames.Visible = false;
            }
        }
    }
}
