using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

namespace PSS
{
    public partial class PurchaseRequisitionX : PSS.TemplateForm
    {
        public string strPRNo;
        public byte nPRSw;

        byte nMode = 0, nDMode = 0;        

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";

        DataTable dtRequestors = new DataTable();                                           // MY 01/06/2015 - Pop-up GridView Requestors query
        DataTable dtDepartments = new DataTable();                                          // MY 10/28/2015 - Pop-up GridView Department query
        DataTable dtCostCenters = new DataTable();                                          // MY 01/06/2015 - Pop-up GridView Cost Center query
        DataTable dtVendors = new DataTable();                                              // MY 02/25/2015 - Pop-up GridView Vendors query
        DataTable dtCatNos = new DataTable();                                               // MY 07/31/2015 - Pop-up GridView Cat Nos query    
        DataTable dtCatNames = new DataTable();                                             // MY 07/31/2015 - Pop-up GridView Cat Names query   
        DataTable dtCatInfo = new DataTable();                                              // MY 02/25/2015 - Pop-up GridView Cat No query    
        DataTable dtOtherFeesList = new DataTable();                                        // MY 07/14/2015 - Pop-up GridView Fees query
        DataTable dtVendorInfo = new DataTable();                                           // MY 02/27/2015 - GridView Vendor Info query
      
        DataTable dtMaster = new DataTable();                                               // MY 07/07/2015 - datatable for Master
        DataTable dtDetail = new DataTable();                                               // MY 07/07/2015 - datatable for Detail
        DataTable dtOtherFees = new DataTable();                                            // MY 07/09/2015 - datatable for Detail
        DataTable dtCatGrades = new DataTable();                                            // MY 08/03/2015 - Pop-up GridView Catalog Grades query    

        DataTable dtUnits = new DataTable();                                                // AMDC 03/03/2018 - Pop-up GridView Units of Measure query    
        DataTable dtCostItems = new DataTable();                                            // AMDC 03/03/2018 - Pop-up GridView Cost Items query    

        public PurchaseRequisitionX()
        {
            InitializeComponent();
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
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);            
        }

        private void LoadRecords()
        {
            nMode = 0;
    
            DataTable dt = new DataTable();
            dt = PSSClass.Procurements.PRMaster();

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
            dgvFile.Visible = true; dgvFile.BringToFront();
            pnlPRDetailGrid.Visible = false;
            pnlPRDetail.Visible = false;
            pnlOtherFees.Visible = false;
            pnlRecord.Visible = false;
            FileAccess();   
        }

        private void FileAccess()
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "PurchaseRequisition");

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

        private void LoadPRMaster(string cCmpyCode, string cPRNo)
        {
            try
            {
                //dtMaster = null;
                dtMaster = PSSClass.Procurements.PRMain(cCmpyCode, cPRNo);
                //bsPRMaster.DataSource = dtMaster;
                //BindPRMaster();
            }
            catch { }
        }

        private void LoadPRDetails(string cCmpyCode, string cPRNo)
        {
            dtDetail = null;
            dtDetail = PSSClass.Procurements.PRDetails(cCmpyCode, cPRNo);
            if (dtDetail == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            bsPRDetails.DataSource = dtDetail;
            bnPRDetails.BindingSource = bsPRDetails;
            dgvPRDetails.DataSource = bsPRDetails;
            DataGridPRDetailsSetting();
            ClearControls(this.pnlPRDetail);
            BindPRDetails();            
        }

        private void LoadPROtherFees(string cCmpyCode, string cPRNo)
        {
            dtOtherFees = null;
            dtOtherFees = PSSClass.Procurements.PROtherFees(cCmpyCode, cPRNo);
            if (dtOtherFees == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            bsPROtherFees.DataSource = dtOtherFees;
            bnPROtherFees.BindingSource = bsPROtherFees;
            dgvPROtherFees.DataSource = bsPROtherFees;
            DataGridOtherFeesSetting();
            BindPROtherFees();             
        }

        private void LoadPayTerms()
        {
            cboPayTerms.Text = "";
            cboPayTerms.DataSource = null;
            DataTable dt = new DataTable();
            dt = PSSClass.Procurements.PRPayTerms();
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            cboPayTerms.DataSource = dt;
            cboPayTerms.DisplayMember = "PayTermDesc";
            cboPayTerms.ValueMember = "PayTermDesc";
        }

        private void LoadOtherFees()
        {
            dgvFees.DataSource = null;

            dtOtherFeesList = PSSClass.Procurements.PROtherFeesList();
            if (dtOtherFeesList == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvFees.DataSource = dtOtherFeesList;
            StandardDGVSetting(dgvFees);
            dgvFees.Columns[0].Width = 167;
            dgvFees.Columns[1].Visible = false;
        }

        //private void LoadCatNos(Int16 cVendorID)
        //{
        //    dgvCatNos.DataSource = null;

        //    if (cVendorID.ToString() == "")
        //    {
        //        MessageBox.Show("Please select vendor's name.");
        //        return;
        //    }
        //    dtCatNos = PSSClass.Procurements.PRCatNos(cVendorID);
        //    if (dtCatNos == null)
        //    {
        //        MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
        //        return;
        //    }
        //    dgvCatNos.DataSource = dtCatNos;
        //    StandardDGVSetting(dgvCatNos);
        //    dgvCatNos.Columns[0].Width = 285;
        //}

        //Revised PR Module
        private void LoadCatNos(Int16 cVendorID, Int16 cCatNameID)
        {
            dgvCatNos.DataSource = null;

            if (cVendorID.ToString() == "")
            {
                MessageBox.Show("Please select vendor's name.");
                return;
            }
            dtCatNos = PSSClass.Procurements.PRCatNosX(cVendorID, cCatNameID);
            if (dtCatNos == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCatNos.DataSource = dtCatNos;
            StandardDGVSetting(dgvCatNos);
            dgvCatNos.Columns[0].Width = 285;
        }


        private void LoadCatNames()
        {
            dgvCatNames.DataSource = null;

            dtCatNames = PSSClass.Procurements.PRCatNames(Convert.ToInt16(txtVendorID.Text));
            if (dtCatNames == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCatNames.DataSource = dtCatNames;
            StandardDGVSetting(dgvCatNames);
            dgvCatNames.Columns[0].Width = 285;
            dgvCatNames.Columns[1].Visible = false;
        }

        private void LoadCatInfo(Int16 cVendorID, String cCatNo)
        {
            dgvCatInfo.DataSource = null;

            dtCatInfo = PSSClass.Procurements.PRCatInfo(cVendorID, cCatNo);
            if (dtCatInfo == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCatInfo.DataSource = dtCatInfo;

            if (dgvCatInfo.Rows.Count != 0)
            {
                txtCatNameID.Text = dgvCatInfo.Rows[0].Cells["CatalogNameID"].Value.ToString();
                txtCatName.Text = dgvCatInfo.Rows[0].Cells["CatalogName"].Value.ToString(); 
                txtCatDesc.Text = dgvCatInfo.Rows[0].Cells["CatalogDesc"].Value.ToString();
                txtGradeID.Text = dgvCatInfo.Rows[0].Cells["GradeID"].Value.ToString();
                txtGrade.Text = dgvCatInfo.Rows[0].Cells["Grade"].Value.ToString();               
                txtUnitPrice.Text = dgvCatInfo.Rows[0].Cells["UnitPrice"].Value.ToString();
            }
        }

        private void LoadDepartments()
        {
            dgvDeptNames.DataSource = null;

            dtDepartments = PSSClass.Procurements.PRMDepartments();
            if (dtDepartments == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvDeptNames.DataSource = dtDepartments;
            StandardDGVSetting(dgvDeptNames);
            dgvDeptNames.Columns[0].Width = 240;
            dgvDeptNames.Columns[1].Visible = false;          
        }

        private void LoadCostCenters()
        {
            dgvCostCenters.DataSource = null;

            dtCostCenters = PSSClass.Procurements.PRMCostCenters();
            if (dtCostCenters == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCostCenters.DataSource = dtCostCenters;
            StandardDGVSetting(dgvCostCenters);
            dgvCostCenters.Columns[0].Width = 240;
            dgvCostCenters.Columns[1].Visible = false;
        }

        private void LoadCatGrades()
        {
            dtCatGrades = PSSClass.Procurements.CatalogGrades();
            if (dtCatGrades == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCatGrades.DataSource = dtCatGrades;
            StandardDGVSetting(dgvCatGrades);
            dgvCatGrades.Columns[0].Width = 152;
            dgvCatGrades.Columns[1].Visible = false;
        }

        private void LoadRequestors()
        {
            dgvRequestors.DataSource = null;

            dtRequestors = PSSClass.Procurements.PRMRequestors();
            if (dtRequestors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvRequestors.DataSource = dtRequestors;
            StandardDGVSetting(dgvRequestors);
            dgvRequestors.Columns[0].Width = 377;
            dgvRequestors.Columns[1].Visible = false;
        }

        private void LoadUnits()
        {
            dtUnits = PSSClass.Procurements.InvtyUnits();
            if (dtUnits == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvUnits.DataSource = dtUnits;
            StandardDGVSetting(dgvUnits);
            dgvUnits.Columns[0].Width = 100;
            dgvUnits.Columns[1].Visible = false;

            dgvConvUnits.DataSource = dtUnits;
            StandardDGVSetting(dgvConvUnits);
            dgvConvUnits.Columns[0].Width = 100;
            dgvConvUnits.Columns[1].Visible = false;
        }

        private void LoadCostItems()
        {
            dtCostItems = PSSClass.Procurements.CostItems();
            if (dtCostItems == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCostItems.DataSource = dtCostItems;
            StandardDGVSetting(dgvCostItems);
            dgvCostItems.Columns[0].Width = 318;
            dgvCostItems.Columns[1].Visible = false;
        }

        private void LoadLocations()
        {
            DataTable dtX = PSSClass.Procurements.DeliveryLocations();
            if (dtX == null || dtX.Rows.Count == 0)
            {
                MessageBox.Show("No GBL locations setup found. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            cboDelLoc.DataSource = dtX;
            cboDelLoc.DisplayMember = "Location";
            cboDelLoc.ValueMember = "GBLID";
        }

        private void LoadVendors()
        {
            dtVendors = PSSClass.Procurements.PRVendors();
            if (dtVendors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvVendorNames.DataSource = dtVendors;
            StandardDGVSetting(dgvVendorNames);
            dgvVendorNames.Columns[0].Width = 377;
            dgvVendorNames.Columns[1].Visible = false;                                                              // Vendor ID           
        }

        private void LoadVendorInfo(int cVendorID)
        {

            dtVendorInfo = PSSClass.Procurements.PRVendorInfo(cVendorID);
            if (dtVendorInfo == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvVendorInfo.DataSource = dtVendorInfo;

            txtAcctNo.Text = dgvVendorInfo.Rows[0].Cells["GBLAcctNo"].Value.ToString();
            cboPayTerms.Text = dgvVendorInfo.Rows[0].Cells["PaymentTerms"].Value.ToString();
            txtWorkPhone.Text = dgvVendorInfo.Rows[0].Cells["WorkPhone"].Value.ToString();
            txtCell.Text = dgvVendorInfo.Rows[0].Cells["CellPhone"].Value.ToString();
            txtFax.Text = dgvVendorInfo.Rows[0].Cells["Fax"].Value.ToString();
            //txtContact.Text = dgvVendorInfo.Rows[0].Cells["ContactName"].Value.ToString();
            //txtEmail.Text = dgvVendorInfo.Rows[0].Cells["Email"].Value.ToString();
            txtWebsite.Text = dgvVendorInfo.Rows[0].Cells["Website"].Value.ToString();
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
            //ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            //items[0] = new ToolStripMenuItem();
            //items[0].Name = "PurchaseRequisitionForm";
            //items[0].Text = "Purchase Requisition Form";
            //items[0].Click += new EventHandler(PrintPurchaseReqFormClickHandler);

            //tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()                            //(string cPRNo)
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Procurements.PRMaster();

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

        private void GetPRSubTotalOtherFees()
        {
            decimal totOtherFees = 0;
            decimal amount = 0;

            for (int j = 0; j < dgvPROtherFees.Rows.Count; j++)
            {
                amount = Convert.ToDecimal(dgvPROtherFees.Rows[j].Cells["Amount"].Value.ToString());
                totOtherFees = totOtherFees + amount;
            }                
          
            txtSubOtherFees.Text = totOtherFees.ToString("C");
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

        private void PrintPurchaseReqFormClickHandler(object sender, EventArgs e)
        {
            PurchaseRequisitionRpt rpt = new PurchaseRequisitionRpt();

            rpt.WindowState = FormWindowState.Maximized;
            rpt.CmpyCode = txtCmpyCode.Text;
            rpt.PRNo = txtPRNo.Text;

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
                    bsFile.Filter = "PRNo<>''";
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
            bsFile.Filter = "PRNo<>''";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {
            nMode = 0;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            pnlPRDetailGrid.Visible = true; pnlPRDetail.Visible = true;
            btnClose.Visible = true; btnClose.BringToFront();

            btnAddFee.Enabled = false;
            btnDeleteFee.Enabled = false;
            btnOKFee.Enabled = false;
            btnAddCharges.Enabled = false;
            btnCheckBrowser.Enabled = false;
            btnPrint.Enabled = false;

            txtPRNo.Text = dgvFile.CurrentRow.Cells["PRNo"].Value.ToString();
            txtCmpyCode.Text = dgvFile.CurrentRow.Cells["CompanyCode"].Value.ToString();
            LoadPRMaster(txtCmpyCode.Text, txtPRNo.Text);
            LoadPRDetails(txtCmpyCode.Text, txtPRNo.Text);
            LoadPROtherFees(txtCmpyCode.Text, txtPRNo.Text);
            
            OpenControls(pnlPRDetail, false);
            lblLock.Text = "Locked";

            if (txtVendorID.Text != "")
            {
                LoadVendorInfo(Convert.ToInt16(txtVendorID.Text));
            }

            if (strFileAccess != "RO")
            {
                AddEditMode(false);
                btnPrint.Enabled = true;
                chkShipTo.Enabled = true;
            }
            if (txtReviewedByID.Text.Trim() != "")
            {
                if (dgvPRDetails.RowCount != 0 && strFileAccess != "RO")
                    btnDeleteDetail.Enabled = true;
                else
                    btnDeleteDetail.Enabled = false;
                if (dgvPROtherFees.RowCount != 0 && strFileAccess != "RO")
                    btnDeleteFee.Enabled = true;
                else
                    btnDeleteFee.Enabled = false;
            }

            if (nMode == 0)
            {
                if (txtPONo.Text.Trim() == "" && txtPRNo.Text.Trim() != "")
                {
                    btnGenerate.Enabled = true;
                }
                else
                {
                    btnGenerate.Enabled = false;
                }
            }
        }

        private void LoadEmailSignatories()
        {
            if (txtDateSubmitted.Text != "")
            {              
                if (txtReviewDate.Text == "")
                {
                    btnReviewerESign.Enabled = true;
                    btnApproverESign.Enabled = false;
                    btnEMail.Enabled = false;
                }
                else
                {
                    btnReviewerESign.Enabled = false;
                    if (txtApprovalDate.Text == "")
                    {
                        btnApproverESign.Enabled = true;
                        btnEMail.Enabled = false;
                    }                    
                }
            }
                       
        }
               
        private void BindPRMaster()
        {
            // Clear bindings first
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            txtCmpyCode.DataBindings.Add("Text", bsPRMaster, "CompanyCode");
            txtPRNo.DataBindings.Add("Text", bsPRMaster, "PRNo");
            txtPONo.DataBindings.Add("Text", bsPRMaster, "PONo");
            txtDeptID.DataBindings.Add("Text", bsPRMaster, "DepartmentID");
            txtDeptName.DataBindings.Add("Text", bsPRMaster, "DepartmentName");
            txtCostCenterID.DataBindings.Add("Text", bsPRMaster, "CostCenterID");
            txtCostCenterName.DataBindings.Add("Text", bsPRMaster, "CostCenterName");
            txtGLCode.DataBindings.Add("Text", bsPRMaster, "GLCode");
            txtRequestedBy.DataBindings.Add("Text", bsPRMaster, "RequestedBy");
            txtRequestor.DataBindings.Add("Text", bsPRMaster, "Requestor");
            txtVendorID.DataBindings.Add("Text", bsPRMaster, "VendorID");
            txtVendorName.DataBindings.Add("Text", bsPRMaster, "VendorName");
            txtAcctNo.DataBindings.Add("Text", bsPRMaster, "AcctNo");
            cboPayTerms.DataBindings.Add("Text", bsPRMaster, "PayTerms");
            txtContact.DataBindings.Add("Text", bsPRMaster, "ContactName");
            txtEmail.DataBindings.Add("Text", bsPRMaster, "Email");
            txtLineItemTotal.DataBindings.Add("Text", bsPRMaster, "LineItemTotal");
            txtOtherCharges.DataBindings.Add("Text", bsPRMaster, "OtherCharges");  
            txtPRAmount.DataBindings.Add("Text", bsPRMaster, "TotalPRAmount");          
            chkIsCancelled.DataBindings.Add("Checked", bsPRMaster, "IsCancelled");        
            txtReviewedByID.DataBindings.Add("Text", bsPRMaster, "ReviewedByID");
            txtReviewer.DataBindings.Add("Text", bsPRMaster, "Reviewer");
            txtApprovedByID.DataBindings.Add("Text", bsPRMaster, "ApprovedByID");
            txtApprover.DataBindings.Add("Text", bsPRMaster, "Approver");
                                 
            Binding DatePRCreatedBinding;
            DatePRCreatedBinding = new Binding("Text", bsPRMaster, "DateCreated");
            DatePRCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtPRDate.DataBindings.Add(DatePRCreatedBinding);

            Binding DatePOCreatedBinding;
            DatePOCreatedBinding = new Binding("Text", bsPRMaster, "PODate");
            DatePOCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtPODate.DataBindings.Add(DatePOCreatedBinding);

            Binding DateReviewedBinding;
            DateReviewedBinding = new Binding("Text", bsPRMaster, "ReviewDate");
            DateReviewedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtReviewDate.DataBindings.Add(DateReviewedBinding);

            Binding DateCancelledBinding;
            DateCancelledBinding = new Binding("Text", bsPRMaster, "DateCancelled");
            DateCancelledBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtDateCancelled.DataBindings.Add(DateCancelledBinding);

            Binding DateApprovedBinding;
            DateApprovedBinding = new Binding("Text", bsPRMaster, "ApprovalDate");
            DateApprovedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtApprovalDate.DataBindings.Add(DateApprovedBinding);   
        }

        private void BindPRDetails()
        {
            // Clear bindings first
            foreach (Control c in pnlPRDetail.Controls)
            {
                c.DataBindings.Clear();
            }    

            try
            {
                txtCatNo.DataBindings.Add("Text", bsPRDetails, "CatalogNo");
                txtCatNameID.DataBindings.Add("Text", bsPRDetails, "CatalogNameID");   
                txtCatName.DataBindings.Add("Text", bsPRDetails, "CatalogName");
                txtCatDesc.DataBindings.Add("Text", bsPRDetails, "CatalogDesc");
                txtGradeID.DataBindings.Add("Text", bsPRDetails, "GradeID");
                txtGrade.DataBindings.Add("Text", bsPRDetails, "Grade");
                txtVendorQuoteNo.DataBindings.Add("Text", bsPRDetails, "VendorQuoteNo");
                chkCOA.DataBindings.Add("Checked", bsPRDetails, "IsCOARequired");
                chkMSD.DataBindings.Add("Checked", bsPRDetails, "IsMSDRequired");
                txtQuantity.DataBindings.Add("Text", bsPRDetails, "Quantity");               
                txtUnitPrice.DataBindings.Add("Text", bsPRDetails, "UnitPrice");
                txtTotPrice.DataBindings.Add("Text", bsPRDetails, "TotalPrice");
                chkInStock.DataBindings.Add("Checked", bsPRDetails, "IsInStock");
                chkBackOrdered.DataBindings.Add("Checked", bsPRDetails, "IsBackOrdered");                   
                txtDetPRNo.DataBindings.Add("Text", bsPRDetails, "PRNo");
                txtPRDetailID.DataBindings.Add("Text", bsPRDetails, "PRDetailID");
                txtUnitID.DataBindings.Add("Text", bsPRDetails, "UnitID");
                txtUnit.DataBindings.Add("Text", bsPRDetails, "UnitDesc");
                txtCostItemID.DataBindings.Add("Text", bsPRDetails, "CostItemID");
                txtCostItem.DataBindings.Add("Text", bsPRDetails, "CostItemDesc");

                Binding DateDetailCreatedBinding;
                DateDetailCreatedBinding = new Binding("Text", bsPRDetails, "DateCreated");
                DateDetailCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
                txtDateCreated.DataBindings.Add(DateDetailCreatedBinding); 
            }
            catch
            { }
        }

        private void BindPROtherFees()
        {
            // Clear bindings first
            foreach (Control c in pnlOtherFees.Controls)
            {
                c.DataBindings.Clear();
            }

            ClearControls(this.pnlOtherFees);            

            txtFeeCode.DataBindings.Add("Text", bsPROtherFees, "FeeCode");
            txtFeeName.DataBindings.Add("Text", bsPROtherFees, "FeeDesc");
            txtFeeAmount.DataBindings.Add("Text", bsPROtherFees, "Amount");
            txtPRNoOtherCharges.DataBindings.Add("Text", bsPROtherFees, "PRNo");
            txtOtherFeesID.DataBindings.Add("Text", bsPROtherFees, "OtherFeesID");

            Binding DateOtherFeesCreatedBinding;
            DateOtherFeesCreatedBinding = new Binding("Text", bsPROtherFees, "DateCreated");
            DateOtherFeesCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtOtherFeesDateEntered.DataBindings.Add(DateOtherFeesCreatedBinding);
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
            dgvFile.Columns["CompanyCode"].HeaderText = "CMPY C0DE";
            dgvFile.Columns["PRNo"].HeaderText = "PR No";
            dgvFile.Columns["DateCreated"].HeaderText = "PR Date";
            dgvFile.Columns["PONo"].HeaderText = "PO No";
            dgvFile.Columns["PODate"].HeaderText = "PO Date";
            dgvFile.Columns["DepartmentName"].HeaderText = "Department";
            dgvFile.Columns["CostCenterName"].HeaderText = "Cost Center";           
            dgvFile.Columns["Requestor"].HeaderText = "Requestor";                       
            dgvFile.Columns["VendorName"].HeaderText = "Vendor Name";            
            dgvFile.Columns["PayTerms"].HeaderText = "Payment Terms";
            dgvFile.Columns["LineItemTotal"].HeaderText = "Line Item Total";
            dgvFile.Columns["OtherCharges"].HeaderText = "Other Charges";  
            dgvFile.Columns["TotalPRAmount"].HeaderText = "Total PR Amount";           
            dgvFile.Columns["IsCancelled"].HeaderText = "Cancelled";
            dgvFile.Columns["DateCancelled"].HeaderText = "Date Cancelled";              
            dgvFile.Columns["Reviewer"].HeaderText = "Reviewer";
            dgvFile.Columns["Approver"].HeaderText = "Approver";
            dgvFile.Columns["PRNo"].Width = 80;
            dgvFile.Columns["DateCreated"].Width = 70;
            dgvFile.Columns["CompanyCode"].Width = 75;
            dgvFile.Columns["PONo"].Width = 75;
            dgvFile.Columns["PODate"].Width = 70;
            dgvFile.Columns["DepartmentName"].Width = 160;
            dgvFile.Columns["CostCenterName"].Width = 300;
            dgvFile.Columns["GLCode"].Width = 60;  
            dgvFile.Columns["Requestor"].Width = 130;            
            dgvFile.Columns["VendorName"].Width = 200;
            dgvFile.Columns["PayTerms"].Width = 90;            
            dgvFile.Columns["AcctNo"].Width = 150;
            dgvFile.Columns["TotalPRAmount"].Width = 80;
            dgvFile.Columns["IsCancelled"].Width = 70;
            dgvFile.Columns["DateCancelled"].Width = 70;
            dgvFile.Columns["Reviewer"].Width = 90;
            dgvFile.Columns["Approver"].Width = 90;
            dgvFile.Columns["DepartmentID"].Visible = false;
            dgvFile.Columns["CostCenterID"].Visible = false;
            dgvFile.Columns["GLCode"].Visible = false;
            dgvFile.Columns["RequestedBy"].Visible = false;
            dgvFile.Columns["VendorID"].Visible = false;            
            dgvFile.Columns["AcctNo"].Visible = false;
            dgvFile.Columns["ContactName"].Visible = false;
            dgvFile.Columns["Email"].Visible = false;   
            dgvFile.Columns["ReviewedByID"].Visible = false;            
            dgvFile.Columns["ReviewDate"].Visible = false;
            dgvFile.Columns["ApprovedByID"].Visible = false;           
            dgvFile.Columns["ApprovalDate"].Visible = false;
            dgvFile.Columns["CreatedByID"].Visible = false;           
            dgvFile.Columns["LastUpdate"].Visible = false;
            dgvFile.Columns["LastUserID"].Visible = false;
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["PODate"].DefaultCellStyle.Format = "MM/dd/yyyy"; 
            dgvFile.Columns["DateCancelled"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["GLCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["PayTerms"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["LineItemTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["LineItemTotal"].DefaultCellStyle.Format = "N2";
            dgvFile.Columns["OtherCharges"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["OtherCharges"].DefaultCellStyle.Format = "N2";
            dgvFile.Columns["TotalPRAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["TotalPRAmount"].DefaultCellStyle.Format = "N2";
        }

        private void DataGridPRDetailsSetting()
        {
            dgvPRDetails.EnableHeadersVisualStyles = false;
            dgvPRDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPRDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvPRDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPRDetails.Columns["CatalogNo"].HeaderText = "Cat No";
            dgvPRDetails.Columns["CatalogName"].HeaderText = "Catalog Name";
            dgvPRDetails.Columns["Grade"].HeaderText = "Grade";
            dgvPRDetails.Columns["Quantity"].HeaderText = "Qty";
            dgvPRDetails.Columns["UnitPrice"].HeaderText = "Unit Price";
            dgvPRDetails.Columns["TotalPrice"].HeaderText = "Total Price";
            dgvPRDetails.Columns["CatalogNo"].Width = 110;
            dgvPRDetails.Columns["CatalogName"].Width = 180;
            dgvPRDetails.Columns["Quantity"].Width = 50;
            dgvPRDetails.Columns["UnitPrice"].Width = 80;
            dgvPRDetails.Columns["TotalPrice"].Width = 60;
            dgvPRDetails.Columns["CatalogNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvPRDetails.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPRDetails.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPRDetails.Columns["TotalPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPRDetails.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
            dgvPRDetails.Columns["TotalPrice"].DefaultCellStyle.Format = "N2";

            dgvPRDetails.Columns["PRDetailID"].Visible = false;
            dgvPRDetails.Columns["PRNo"].Visible = false;
            dgvPRDetails.Columns["CatalogNameID"].Visible = false;
            dgvPRDetails.Columns["CatalogDesc"].Visible = false;
            dgvPRDetails.Columns["GradeID"].Visible = false;
            dgvPRDetails.Columns["Grade"].Visible = false;
            dgvPRDetails.Columns["VendorQuoteNo"].Visible = false;
            dgvPRDetails.Columns["IsCOARequired"].Visible = false;
            dgvPRDetails.Columns["IsMSDRequired"].Visible = false;
            dgvPRDetails.Columns["IsInStock"].Visible = false;
            dgvPRDetails.Columns["IsBackOrdered"].Visible = false;
            dgvPRDetails.Columns["UnitID"].Visible = false;
            dgvPRDetails.Columns["UnitDesc"].Visible = false;
            dgvPRDetails.Columns["CostItemID"].Visible = false;
            dgvPRDetails.Columns["CostItemDesc"].Visible = false;
            dgvPRDetails.Columns["CompanyCode"].Visible = false;
            dgvPRDetails.Columns["CreatedByID"].Visible = false;
            dgvPRDetails.Columns["DateCreated"].Visible = false;
            dgvPRDetails.Columns["LastUpdate"].Visible = false;
            dgvPRDetails.Columns["LastUserID"].Visible = false;
        }

        private void DataGridOtherFeesSetting()
        {
            dgvPROtherFees.EnableHeadersVisualStyles = false;
            dgvPROtherFees.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPROtherFees.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvPROtherFees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPROtherFees.Columns["FeeCode"].HeaderText = "Fee Code";
            dgvPROtherFees.Columns["FeeDesc"].HeaderText = "Description";
            dgvPROtherFees.Columns["Amount"].HeaderText = "Amount";
            dgvPROtherFees.Columns["FeeDesc"].Width = 241;
            dgvPROtherFees.Columns["Amount"].Width = 80;
            dgvPROtherFees.Columns["PRNo"].Visible = false;
            dgvPROtherFees.Columns["OtherFeesID"].Visible = false;
            dgvPROtherFees.Columns["FeeCode"].Visible = false;
            dgvPROtherFees.Columns["CreatedByID"].Visible = false;
            dgvPROtherFees.Columns["DateCreated"].Visible = false;
            dgvPROtherFees.Columns["LastUpdate"].Visible = false;
            dgvPROtherFees.Columns["LastUserID"].Visible = false;
            dgvPROtherFees.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPROtherFees.Columns["Amount"].DefaultCellStyle.Format = "N2";
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
            pnlPRDetailGrid.Visible = true; pnlPRDetail.Visible = true;
            ClearControls(this.pnlRecord);
            ClearControls(this.pnlPRDetail);
            ClearControls(this.pnlPRDetailGrid);
            ClearControls(this.pnlOtherFees);
            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlPRDetail, false);
            OpenControls(this.pnlPRDetailGrid, false);
            OpenControls(this.pnlOtherFees, false);
            dtMaster.Rows.Clear();
            dtDetail.Rows.Clear();
            dtOtherFees.Rows.Clear();

            picCatNames.Enabled = false;
            picCatNos.Enabled = false;
            picGrades.Enabled = false;

            btnAddDetail.Enabled = true; btnEditDetail.Enabled = true; btnDeleteDetail.Enabled = true;
            btnAddCharges.Enabled = true; btnGenerate.Enabled = false;btnCheckBrowser.Enabled = false;

            txtGLCode.ReadOnly = true;

            // Create PRMaster Data Row            
            DataRow dR = dtMaster.NewRow();

            dR["CompanyCode"] = "P";
            dR["PRNo"] = "< New >";
            dR["DateCreated"] = DateTime.Now;    
            dR["PONo"] = DBNull.Value;
            dR["PODate"] = DBNull.Value;

            switch (LogIn.nUserID)
            {
                case 328:                                   // Mahesh
                    dR["DepartmentID"] = 2;
                    dR["DepartmentName"] = "Chemistry";
                    dR["CostCenterID"] = 1;
                    dR["CostCenterName"] = "CHEMISTRY SUPPLIES";
                    dR["GLCode"] = 3100;
                    break;
                case 263:                                   // Remy
                    dR["DepartmentID"] = 1;
                    dR["DepartmentName"] = "Calibration/Repair";
                    dR["CostCenterID"] = 7;
                    dR["CostCenterName"] = "GENERAL LAB SUPPLIES";
                    dR["GLCode"] = 3122;
                    break;               
                default:
                    dR["DepartmentID"] = DBNull.Value;
                    dR["DepartmentName"] = DBNull.Value;
                    dR["CostCenterID"] = DBNull.Value;
                    dR["CostCenterName"] = DBNull.Value;
                    dR["GLCode"] = DBNull.Value;
                    break;
            }          
            dR["RequestedBy"] = LogIn.nUserID;
            dR["Requestor"] = PSSClass.Users.UserWholeName(LogIn.nUserID);        
            dR["VendorID"] = DBNull.Value;
            dR["VendorName"] = DBNull.Value;           
            dR["AcctNo"] = DBNull.Value;
            dR["PayTerms"] = DBNull.Value;
            dR["ContactName"] = DBNull.Value;
            dR["Email"] = DBNull.Value;
            dR["LineItemTotal"] = DBNull.Value;
            dR["OtherCharges"] = DBNull.Value;
            dR["TotalPRAmount"] = DBNull.Value;           
            dR["IsCancelled"] = false;
            dR["DateCancelled"] = DBNull.Value;
            dR["ReviewedByID"] = DBNull.Value;
            dR["Reviewer"] = DBNull.Value;
            dR["ReviewDate"] = DBNull.Value;
            dR["ApprovedByID"] = DBNull.Value;
            dR["Approver"] = DBNull.Value;
            dR["ApprovalDate"] = DBNull.Value;
            dR["CreatedByID"] = LogIn.nUserID;                    
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;
            dtMaster.Rows.Add(dR);
            bsPRMaster.DataSource = dtMaster;
            BindPRMaster();
            txtCostCenterName.Select();
            txtCostCenterName.Focus();     
        }

        private void DateBinding_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value.ToString() != "")
                e.Value = ((DateTime)e.Value).ToString("MM/dd/yyyy");
            else
                e.Value = "";
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;

            if (txtPONo.Text.Trim() != "")
            {
                MessageBox.Show("A PO has been generated for this request. Edit is not allowed!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                AddEditMode(false);
                return;
            }

            LoadData();
            nMode = 2;
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();
           
            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlPRDetail, true);
            OpenControls(this.pnlOtherFees, true);
            txtRequestedBy.Focus(); btnClose.Visible = false;

            picCatNames.Enabled = true;
            picCatNos.Enabled = true;
            picGrades.Enabled = true;

            btnAddDetail.Enabled = true;
            btnDeleteDetail.Enabled = true;
            btnAddCharges.Enabled = true;

            tsbAdd.Enabled = false;
            tsbEdit.Enabled = false;
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;

            lblLock.Text = "Unlocked";
            if (txtVendorID.Text != "")
            {
                ////Revised PR Module
                //if (txtCatNameID.Text == "")
                //    txtCatNameID.Text = "0";
                LoadCatNos(Convert.ToInt16(txtVendorID.Text), Convert.ToInt16(txtCatNameID.Text));
                //LoadCatNos(Convert.ToInt16(txtVendorID.Text));
            }
        }

        private void DeleteRecord()
        {

        }

        private void DeleteDetail(int cPRDetailID)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("PRDetailID", cPRDetailID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelPRDetail";

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

        private void DeleteOtherFees(int cPROtherFeesID)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@PROtherFeesID", cPROtherFeesID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelPROtherFees";

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

        private void SaveRecord()
        {
            // Master Save Routine
            bsPRMaster.EndEdit();                     

            // Validate if changes were made on the Master
            DataTable dtHeader = dtMaster.GetChanges();
            if (dtHeader != null)
            { 
                int nPR = ValidateMaster();                                                      // Validation for PR Master Record
                if (nPR == 0)
                {
                    dtHeader.Dispose();
                    return;
                }
                
                SavePRMaster();                                                                 // Save PR Master Record

                dtHeader.Dispose();   
            }                      
           
            // Detail Save Routine
            bsPRDetails.EndEdit();
         
            // Validate if changes were made on the Detail
            DataTable dtDetails = dtDetail.GetChanges();
            if (dtDetails != null)
            {
                int nPR = ValidateDetails();                                                   // Validation for PR Detail Record
                if (nPR == 0)
                {
                    dtDetails.Dispose();
                    return;
                }
                UpdatePRDetails();                                                             // Save PR Detail Record
                dtDetails.Dispose();

                for (int i = 0; i < dgvPRDetails.Rows.Count; i++)
                {                   
                    UpdateCatalogMaster(
                        Convert.ToInt16(txtVendorID.Text.Trim()),
                        Convert.ToString(dgvPRDetails.Rows[i].Cells["CatalogNo"].Value),
                        Convert.ToInt16(dgvPRDetails.Rows[i].Cells["CatalogNameID"].Value),
                        Convert.ToString(dgvPRDetails.Rows[i].Cells["CatalogName"].Value),
                        Convert.ToString(dgvPRDetails.Rows[i].Cells["CatalogDesc"].Value),
                        Convert.ToInt16(dgvPRDetails.Rows[i].Cells["GradeID"].Value),
                        Convert.ToDecimal(dgvPRDetails.Rows[i].Cells["UnitPrice"].Value)                        
                        );
                }

                UpdatePRCatNameID();
             
            }

            // Other Fees Save Routine
            bsPROtherFees.EndEdit();

            // Validate if changes were made on the Detail
            DataTable dtCharges = dtOtherFees.GetChanges();
            if (dtCharges != null)
            {
                int nPR = ValidateOtherFees();                                                 // Validation for PR Other Fees Record
                if (nPR == 0)
                {
                    dtDetails.Dispose();
                    return;
                }
                UpdatePROtherFees();                                                          // Save PR Detail Record
                dtCharges.Dispose();
            }                   
           
            // Update PRMaster Totals
            UpdatePRMasterTotals();

            // Update Vendor Info
            UpdatePRVendorInfo();

            // Reload
            dgvFile.Refresh();
            btnClose.Visible = true;
            btnAddDetail.Enabled = false;
            OpenControls(pnlRecord, false);
            LoadRecords();
            PSSClass.General.FindRecord("PRNo", txtPRNo.Text, bsFile, dgvFile);         
            LoadData();
            MessageBox.Show("Record successfully saved!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
             
        private void SavePRMaster()
        {
            DataTable dtX = dtMaster.GetChanges(DataRowState.Added);
            if (dtX != null & dtX.Rows.Count > 0)
            {
                nMode = 1;
                txtCmpyCode.Text = "P";
                txtPRNo.Text = PSSClass.Procurements.NewPRID();
                dtX.Dispose();
            }
            dtX = dtMaster.GetChanges(DataRowState.Modified);
            if (dtX != null & dtX.Rows.Count > 0)
            {
                nMode = 2;
                dtX.Dispose();
            }
            if (dtX == null)
                dtX.Dispose();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@PRNo", txtPRNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@DepartmentID", Convert.ToInt16(txtDeptID.Text));
            sqlcmd.Parameters.AddWithValue("@CostCenterID", Convert.ToInt16(txtCostCenterID.Text));
            sqlcmd.Parameters.AddWithValue("@RequestedBy", Convert.ToInt16(txtRequestedBy.Text));           
            sqlcmd.Parameters.AddWithValue("@VendorID", Convert.ToInt16(txtVendorID.Text));                   
            sqlcmd.Parameters.AddWithValue("@AcctNo", txtAcctNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@PayTerms", cboPayTerms.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@ContactName", txtContact.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@IsCancelled", Convert.ToBoolean(chkIsCancelled.CheckState));
            if (txtDateCancelled.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@DateCancelled", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@DateCancelled", Convert.ToDateTime(txtDateCancelled.Text));
            }
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditPRMaster";
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

        private static int SavePRDetails(int cPRDetailID, string cCmpyCode, string cPRNo, int cRow, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
            sqlcmd.Parameters.AddWithValue("@PRDetailID", cPRDetailID);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpyCode);
            sqlcmd.Parameters.AddWithValue("@PRNo", cPRNo);
            sqlcmd.Parameters.AddWithValue("@CatalogNo", cDT.Rows[cRow]["CatalogNo"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@CatalogNameID", Convert.ToInt16(cDT.Rows[cRow]["CatalogNameID"].ToString()));
            sqlcmd.Parameters.AddWithValue("@CatalogDesc", cDT.Rows[cRow]["CatalogDesc"].ToString().Trim());
            sqlcmd.Parameters.AddWithValue("@GradeID", Convert.ToInt16(cDT.Rows[cRow]["GradeID"].ToString()));
            sqlcmd.Parameters.AddWithValue("@VendorQuoteNo", cDT.Rows[cRow]["VendorQuoteNo"].ToString());
            sqlcmd.Parameters.AddWithValue("@IsCOARequired", Convert.ToBoolean(cDT.Rows[cRow]["IsCOARequired"].ToString()));
            sqlcmd.Parameters.AddWithValue("@IsMSDRequired", Convert.ToBoolean(cDT.Rows[cRow]["IsMSDRequired"].ToString()));
            sqlcmd.Parameters.AddWithValue("@Quantity", Convert.ToInt16(cDT.Rows[cRow]["Quantity"].ToString()));          
            sqlcmd.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(cDT.Rows[cRow]["UnitPrice"].ToString()));
            sqlcmd.Parameters.AddWithValue("@TotalPrice", Convert.ToDecimal(cDT.Rows[cRow]["TotalPrice"].ToString()));
            sqlcmd.Parameters.AddWithValue("@IsInStock", Convert.ToBoolean(cDT.Rows[cRow]["IsInStock"].ToString()));
            sqlcmd.Parameters.AddWithValue("@IsBackOrdered", Convert.ToBoolean(cDT.Rows[cRow]["IsBackOrdered"].ToString()));
            
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditPRDetail";
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

        private static int SavePROtherFees(string cCmpyCode, int cPROtherFeesID, string cPRNo, int cRow, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }

            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpyCode);
            sqlcmd.Parameters.AddWithValue("@PRNo", cPRNo);
            sqlcmd.Parameters.AddWithValue("@OtherFeesID", cPROtherFeesID);            
            sqlcmd.Parameters.AddWithValue("@FeeCode", cDT.Rows[cRow]["FeeCode"].ToString());            
            sqlcmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(cDT.Rows[cRow]["Amount"].ToString()));            

            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditPROtherFees";
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

        private void UpdatePROtherFees()
        {
            bsPROtherFees.EndEdit();
          
            DataTable dt = dtOtherFees.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtOtherFeesID.Text = PSSClass.DataEntry.NewID("PROtherFees", "OtherFeesID").ToString();

                    SavePROtherFees(txtCmpyCode.Text, Convert.ToInt16(txtOtherFeesID.Text), txtPRNo.Text, i, 1, dt);                    
                }
                dt.Rows.Clear();
            }
            dt = dtOtherFees.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SavePROtherFees(txtCmpyCode.Text, Convert.ToInt16(dt.Rows[i]["OtherFeesID"].ToString()), txtPRNo.Text, i, 2, dt);
                }
                dt.Rows.Clear();
            }
        }

        private void UpdatePRDetails()
        {
            bsPRDetails.EndEdit();
            DataTable dt = dtDetail.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtPRDetailID.Text = PSSClass.DataEntry.NewID("PRDetails", "PRDetailID").ToString();
                    SavePRDetails(Convert.ToInt16(txtPRDetailID.Text), txtCmpyCode.Text, txtPRNo.Text, i, 1, dt);
                }
                dt.Rows.Clear();
            }
            dt = dtDetail.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SavePRDetails(Convert.ToInt16(dt.Rows[i]["PRDetailID"].ToString()), txtCmpyCode.Text, txtPRNo.Text, i, 2, dt);
                }
                dt.Rows.Clear();
            }
            dt = dtDetail.GetChanges(DataRowState.Deleted);
            if (dt != null)
            {
                int intPRDetailID;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    intPRDetailID = Convert.ToInt16(dgvPRDetails.Rows[i].Cells["PRDetailID"].Value.ToString());
                    if (intPRDetailID != 0)
                        DeleteDetail(intPRDetailID);
                }
                dt.Rows.Clear();
            }
            if (dt != null)
                dt.Dispose();
        }

        private void UpdatePRMasterTotals()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection(); ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@PRNo", txtPRNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdatePRMasterTotals";
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

        private void UpdatePRCatNameID()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection(); ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@PRNo", txtPRNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdatePRCatNameID";
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

        private void UpdatePRVendorInfo()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection(); ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@VendorID", Convert.ToInt16(txtVendorID.Text));
            sqlcmd.Parameters.AddWithValue("@AcctNo", txtAcctNo.Text.Trim());         
            sqlcmd.Parameters.AddWithValue("@WorkPhone", txtWorkPhone.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CellPhone", txtCell.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Fax", txtFax.Text.Trim());
            //sqlcmd.Parameters.AddWithValue("@ContactName", txtContact.Text.Trim());
            //sqlcmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@PaymentTerms", cboPayTerms.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdatePRVendorInfo";
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

        private void UpdateCatalogMaster(int cVendorID, String cCatalogNo, int cCatalogNameID, String cCatalogName, String cCatalogDesc, int cGradeID, decimal cUnitPrice)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection(); ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@VendorID", cVendorID);
            sqlcmd.Parameters.AddWithValue("@CatalogNo", cCatalogNo);
            if (txtCatNameID.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@CatalogNameID", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@CatalogNameID", cCatalogNameID);
            }
            sqlcmd.Parameters.AddWithValue("@CatalogName", cCatalogName);
            sqlcmd.Parameters.AddWithValue("@CatalogDesc", cCatalogDesc);
            if (txtGradeID.Text.Trim() == "")
            {
                sqlcmd.Parameters.AddWithValue("@GradeID", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@GradeID", cGradeID);
            }
            sqlcmd.Parameters.AddWithValue("@UnitPrice", cUnitPrice);
            sqlcmd.Parameters.AddWithValue("@IsActive", true);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdateCatalogMaster";
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

        private int ValidateMaster()
        {           
            if (txtRequestedBy.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Requestor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtRequestedBy.Focus();
                return 0;
            }
            if (txtDeptName.Text.Trim() == "")
            {
                MessageBox.Show("Please choose Department!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDeptID.Focus();
                return 0;
            }
            if (txtCostCenterName.Text.Trim() == "")
            {
                MessageBox.Show("Please choose Cost Center!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCostCenterID.Focus();
                return 0;
            }
            if (txtVendorID.Text.Trim() == "")
            {
                MessageBox.Show("Please choose Vendor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtVendorID.Focus();
                return 0;
            }           
            
            if (cboPayTerms.Text.Trim() == "")
            {
                MessageBox.Show("Please choose Payment Term!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboPayTerms.Focus();
                return 0;
            }
            return 1;
        }

        private int ValidateDetails()
        {
            if (nDMode != 0)
            {
                if (txtCatNameID.Text.Trim() == "" || txtCatNameID.Text.Trim() == "0")
                {
                    MessageBox.Show("Please select catalog name!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCatName.Focus();
                    return 0;
                }
                if (txtCatNo.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Catalog number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCatNo.Focus();
                    return 0;
                }
                if (txtGradeID.Text.Trim() == "" || txtGradeID.Text.Trim() == "0")
                {
                    MessageBox.Show("Please select product grade!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtGrade.Focus();
                    return 0;
                }
                if (txtCostItemID.Text.Trim() == "" || txtCostItemID.Text.Trim() == "0")
                {
                    MessageBox.Show("Please select cost item category!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCostItem.Focus();
                    return 0;
                }
                if (txtUnitID.Text.Trim() == "" || txtUnitID.Text.Trim() == "0")
                {
                    MessageBox.Show("Please select unit of measure!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtUnit.Focus();
                    return 0;
                }
                if (txtQuantity.Text.Trim() == "" || Convert.ToInt16(txtQuantity.Text.Trim()) == 0)
                {
                    MessageBox.Show("Please enter Quantity!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtQuantity.Focus();
                    return 0;
                } 
            } 
            return 1;
        }

        private int ValidateOtherFees()
        {
            if (nMode != 0)
            {
                if (txtFeeName.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Fee Type!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtFeeName.Focus();
                    return 0;
                }
                if (txtFeeAmount.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter an amount!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtFeeAmount.Focus();
                    return 0;
                }
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
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            dgvRequestors.Visible = false;
            dgvCostCenters.Visible = false;
            dgvVendorNames.Visible = false;
            dgvCatNames.Visible = false;
            dgvCatNos.Visible = false; 
            AddEditMode(false);
            nMode = 0;
        }

        private void PurchaseRequisition_Load(object sender, EventArgs e)
        {
            LoadRecords();
            LoadRequestors();
            LoadPayTerms();
            LoadDepartments();
            LoadCostCenters();
            LoadVendors();
            LoadOtherFees();
            LoadCatGrades();
            LoadUnits();
            LoadCostItems();
            LoadLocations();
            BuildPrintItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();                      

            CreateMasterStructure();
            CreateDetailStructure();
            CreateOtherFeesStructure();
            
            if (nPRSw == 1)
            {
                PSSClass.General.FindRecord("PRNo", strPRNo, bsFile, dgvFile);
                LoadData();
            }                       
        }       

        private void CreateMasterStructure()
        {
            // Create PR Master Data table for Add/Edit/Delete functions
            dtMaster.Columns.Add("CompanyCode", typeof(string));
            dtMaster.Columns.Add("PRNo", typeof(string));
            dtMaster.Columns.Add("DateCreated", typeof(DateTime));
            dtMaster.Columns.Add("PONo", typeof(string));
            dtMaster.Columns.Add("PODate", typeof(DateTime));
            dtMaster.Columns.Add("DepartmentID", typeof(Int16));
            dtMaster.Columns.Add("DepartmentName", typeof(string));
            dtMaster.Columns.Add("CostCenterID", typeof(Int16));
            dtMaster.Columns.Add("CostCenterName", typeof(string));
            dtMaster.Columns.Add("GLCode", typeof(string));
            dtMaster.Columns.Add("RequestedBy", typeof(Int16));
            dtMaster.Columns.Add("Requestor", typeof(string));
            dtMaster.Columns.Add("VendorID", typeof(Int16));
            dtMaster.Columns.Add("VendorName", typeof(string));
            dtMaster.Columns.Add("AcctNo", typeof(string));
            dtMaster.Columns.Add("PayTerms", typeof(string));
            dtMaster.Columns.Add("ContactName", typeof(string));
            dtMaster.Columns.Add("Email", typeof(string));
            dtMaster.Columns.Add("LineItemTotal", typeof(decimal));
            dtMaster.Columns.Add("OtherCharges", typeof(decimal));
            dtMaster.Columns.Add("TotalPRAmount", typeof(decimal));
            dtMaster.Columns.Add("IsCancelled", typeof(bool));
            dtMaster.Columns.Add("DateCancelled", typeof(DateTime));
            dtMaster.Columns.Add("ReviewedByID", typeof(Int16));
            dtMaster.Columns.Add("Reviewer", typeof(string));
            dtMaster.Columns.Add("ReviewDate", typeof(DateTime));
            dtMaster.Columns.Add("ApprovedByID", typeof(Int16));
            dtMaster.Columns.Add("Approver", typeof(string));
            dtMaster.Columns.Add("ApprovalDate", typeof(DateTime));
            dtMaster.Columns.Add("CreatedByID", typeof(Int16));
            dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            dtMaster.Columns.Add("LastUserID", typeof(Int16));
            bsPRMaster.DataSource = dtMaster;
            BindPRMaster();
        }

        private void CreateDetailStructure()
        {
            // Create PR Detail Data table for Add/Edit/Delete functions
            bsPRDetails.DataSource = dtDetail;
            dgvPRDetails.DataSource = bsPRDetails;
            dtDetail.Columns.Add("CatalogNo", typeof(string));
            dtDetail.Columns.Add("CatalogNameID", typeof(Int16));
            dtDetail.Columns.Add("CatalogName", typeof(string));
            dtDetail.Columns.Add("CatalogDesc", typeof(string));
            dtDetail.Columns.Add("GradeID", typeof(Int16));
            dtDetail.Columns.Add("Grade", typeof(string));
            dtDetail.Columns.Add("VendorQuoteNo", typeof(string));
            dtDetail.Columns.Add("IsCOARequired", typeof(bool));
            dtDetail.Columns.Add("IsMSDRequired", typeof(bool));
            dtDetail.Columns.Add("Quantity", typeof(Int16));
            dtDetail.Columns.Add("UnitPrice", typeof(decimal));
            dtDetail.Columns.Add("TotalPrice", typeof(decimal));
            dtDetail.Columns.Add("IsInStock", typeof(bool));
            dtDetail.Columns.Add("IsBackOrdered", typeof(bool));
            dtDetail.Columns.Add("UnitID", typeof(Int16));
            dtDetail.Columns.Add("UnitDesc", typeof(string));
            dtDetail.Columns.Add("CostItemID", typeof(Int16));
            dtDetail.Columns.Add("CostItemDesc", typeof(string));
            dtDetail.Columns.Add("CreatedByID", typeof(Int16));
            dtDetail.Columns.Add("DateCreated", typeof(DateTime));
            dtDetail.Columns.Add("LastUpdate", typeof(DateTime));
            dtDetail.Columns.Add("LastUserID", typeof(Int16));
            dtDetail.Columns.Add("CompanyCode", typeof(string));
            dtDetail.Columns.Add("PRNo", typeof(string));
            dtDetail.Columns.Add("PRDetailID", typeof(Int16));
            DataGridPRDetailsSetting();
        }

        private void CreateOtherFeesStructure()
        {
            // Create PR Other Charges Data table for Add/Edit/Delete functions            
            bsPROtherFees.DataSource = dtOtherFees;
            dgvPROtherFees.DataSource = bsPROtherFees;
            dtOtherFees.Columns.Add("FeeCode", typeof(Int16));
            dtOtherFees.Columns.Add("FeeDesc", typeof(string));
            dtOtherFees.Columns.Add("Amount", typeof(decimal));
            dtOtherFees.Columns.Add("CreatedByID", typeof(Int16));
            dtOtherFees.Columns.Add("DateCreated", typeof(DateTime));
            dtOtherFees.Columns.Add("LastUpdate", typeof(DateTime));
            dtOtherFees.Columns.Add("LastUserID", typeof(Int16));
            dtOtherFees.Columns.Add("PRNo", typeof(string));
            dtOtherFees.Columns.Add("OtherFeesID", typeof(Int16));           
            DataGridOtherFeesSetting();
        }

        private void PrintReport()
        {
            PurchaseRequisitionRpt rpt = new PurchaseRequisitionRpt();

            txtCmpyCode.Text = dgvFile.CurrentRow.Cells["CompanyCode"].Value.ToString();
            txtPRNo.Text = dgvFile.CurrentRow.Cells["PRNo"].Value.ToString();
            rpt.CmpyCode = txtCmpyCode.Text.Trim();
            rpt.PRNo = txtPRNo.Text.Trim();

            if (chkShipTo.Checked)
            {
                rpt.DlvrTo = 1;
            }
            else
            {
                rpt.DlvrTo = 2;
            }

            rpt.WindowState = FormWindowState.Maximized;

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void PurchaseRequistion_KeyDown(object sender, KeyEventArgs e)
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
            nMode = 0;       

            if (nPRSw == 1)
            {
                nPRSw = 0;                
                this.Close(); this.Dispose();
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

        // MY 01/06/2015 - START: txt/dgvRequestors events
        private void dgvRequestors_DoubleClick(object sender, EventArgs e)
        {
            txtRequestor.Text = dgvRequestors.CurrentRow.Cells["Requestor"].Value.ToString();
            txtRequestedBy.Text = dgvRequestors.CurrentRow.Cells["RequestedBy"].Value.ToString();
            dgvRequestors.Visible = false;        
        }

        private void dgvRequestors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvRequestors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtRequestor.Text = dgvRequestors.CurrentRow.Cells["Requestor"].Value.ToString();
                txtRequestedBy.Text = dgvRequestors.CurrentRow.Cells["RequestedBy"].Value.ToString();
                dgvRequestors.Visible = false;            
            }
            else if (e.KeyChar == 27)
            {
                dgvRequestors.Visible = false;
            }
        }
        private void txtRequestor_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvRequestors.Visible = true; dgvRequestors.BringToFront();
            }
        }

        private void dgvRequestors_Leave(object sender, EventArgs e)
        {
            dgvRequestors.Visible = false;
        }

        private void txtRequestor_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwRequestors;
                dvwRequestors = new DataView(dtRequestors, "Requestor like '%" + txtRequestor.Text.Trim().Replace("'", "''") + "%'", "Requestor", DataViewRowState.CurrentRows);
                dvwSetUp(dgvRequestors, dvwRequestors);
            }
        }

        private void dgvRequestors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtRequestor.Text = dgvRequestors.CurrentRow.Cells["Requestor"].Value.ToString();
            txtRequestedBy.Text = dgvRequestors.CurrentRow.Cells["RequestedBy"].Value.ToString();
            dgvRequestors.Visible = false;       
        }

        private void picRequestors_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadRequestors();
                dgvRequestors.Visible = true; dgvRequestors.BringToFront();
            }
        }

        private void txtRequestedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtRequestor.Text = PSSClass.Procurements.PRMRequestor(txtRequestedBy.Text.Trim());
                if (txtRequestor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Requestor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvRequestors.Visible = false;
            }
            else
            {
                txtRequestor.Text = ""; dgvRequestors.Visible = false;
            }
        }

        // MY 01/06/2015 - END: txt/dgvRequestors events   

        // MY 10/29/2015 - START: txt/dgvDepartment events
        private void dgvDeptNames_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
                txtDeptID.Text = dgvDeptNames.CurrentRow.Cells["DepartmentID"].Value.ToString();  
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
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
                    txtDeptID.Text = dgvDeptNames.CurrentRow.Cells["DepartmentID"].Value.ToString();
                    dgvDeptNames.Visible = false;  
                }
                else if (e.KeyChar == 27)
                {
                    dgvDeptNames.Visible = false;
                }
            }
        }
        private void txtDeptID_Enter(object sender, EventArgs e)
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
                dvwDeptNames = new DataView(dtDepartments, "DepartmentName like '%" + txtDeptName.Text.Trim().Replace("'", "''") + "%'", "DepartmentName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvDeptNames, dvwDeptNames);   
             }
        }

        private void dgvDeptNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
                txtDeptID.Text = dgvDeptNames.CurrentRow.Cells["DepartmentID"].Value.ToString();
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

        private void txtDeptID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtDeptName.Text = PSSClass.Procurements.PRMDeptName(Convert.ToInt16(txtDeptID.Text.Trim()));
                    if (txtDeptName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Dept ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        // MY 10/29/2015 - END: txt/dgvDeptNames events     

        // MY 07/24/2015 - START: txt/dgvCostCenter events
        private void dgvCostCenters_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtCostCenterName.Text = dgvCostCenters.CurrentRow.Cells["CostCenterName"].Value.ToString();
                txtCostCenterID.Text = dgvCostCenters.CurrentRow.Cells["CostCenterID"].Value.ToString();
                dgvCostCenters.Visible = false;
                try
                {
                    txtGLCode.Text = PSSClass.Procurements.PRGLCode(Convert.ToInt16(txtCostCenterID.Text));
                    txtVendorName.Focus();
                }
                catch { }
            }
        }

        private void dgvCostCenters_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCostCenters_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCostCenterName.Text = dgvCostCenters.CurrentRow.Cells["CostCenterName"].Value.ToString();
                    txtCostCenterID.Text = dgvCostCenters.CurrentRow.Cells["CostCenterID"].Value.ToString();
                    dgvCostCenters.Visible = false;

                    try
                    {
                        txtGLCode.Text = PSSClass.Procurements.PRGLCode(Convert.ToInt16(txtCostCenterID.Text));
                    }
                    catch { }
                }
                else if (e.KeyChar == 27)
                {
                    dgvCostCenters.Visible = false;
                }
            }
        }
        private void txtCostCenterID_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvCostCenters.Visible = true; dgvCostCenters.BringToFront();
            }
        }

        private void dgvCostCenters_Leave(object sender, EventArgs e)
        {
            dgvCostCenters.Visible = false;
        }

        private void txtCostCenterName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwCostCenters;
                dvwCostCenters = new DataView(dtCostCenters, "CostCenterName like '%" + txtCostCenterName.Text.Trim().Replace("'", "''") + "%'", "CostCenterName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvCostCenters, dvwCostCenters);
            }
        }

        private void dgvCostCenters_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtCostCenterName.Text = dgvCostCenters.CurrentRow.Cells["CostCenterName"].Value.ToString();
                txtCostCenterID.Text = dgvCostCenters.CurrentRow.Cells["CostCenterID"].Value.ToString();
                dgvCostCenters.Visible = false;
                // Get GL Code
                try
                {
                    txtGLCode.Text = PSSClass.Procurements.PRGLCode(Convert.ToInt16(txtCostCenterID.Text));
                }
                catch { }
            }
        }

        private void picCostCenters_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadCostCenters();
                dgvCostCenters.Visible = true; dgvCostCenters.BringToFront();
            }
        }

        private void txtCostCenterID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCostCenterName.Text = PSSClass.Procurements.PRMCostCenterName(Convert.ToInt16(txtCostCenterID.Text.Trim()));
                    if (txtCostCenterName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Cost Center ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvCostCenters.Visible = false;
                    // Get GL Code
                    try
                    {
                        txtGLCode.Text = PSSClass.Procurements.PRGLCode(Convert.ToInt16(txtCostCenterID.Text));
                    }
                    catch { }
                }
                else
                {
                    txtCostCenterName.Text = ""; dgvCostCenters.Visible = false;
                }
            }
        }

        // MY 01/06/2015 - END: txt/dgvCostCenter events     
        // MY 02/25/2015 - START: txt/dgvVendorNames events
        private void dgvVendorNames_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
                txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
                LoadVendorInfo(Convert.ToInt16(txtVendorID.Text.Trim()));
                dgvVendorNames.Visible = false;
            }
        }

        private void dgvVendorNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvVendorNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
                    txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
                    LoadVendorInfo(Convert.ToInt16(txtVendorID.Text.Trim()));
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
            if (nMode != 0)
            {
                dgvVendorNames.Visible = true; dgvVendorNames.BringToFront();
            }
        }

        private void dgvVendorNames_Leave(object sender, EventArgs e)
        {         
            dgvVendorNames.Visible = false;
        }

        private void txtVendorName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwVendorNames;
                dvwVendorNames = new DataView(dtVendors, "VendorName like '%" + txtVendorName.Text.Trim().Replace("'", "''") + "%'", "VendorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvVendorNames, dvwVendorNames);
            }
        }

        private void dgvVendorNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
                txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
                LoadVendorInfo(Convert.ToInt16(txtVendorID.Text.Trim()));
                dgvVendorNames.Visible = false;
                dgvVendorNames.BringToFront();
            }
        }

        private void picVendors_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadVendors();
                btnCheckBrowser.Enabled = true;
                dgvVendorNames.Visible = true; dgvVendorNames.BringToFront();
            }
        }

        private void txtVendorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtVendorName.Text = PSSClass.Calibration.EqptVendorName(Convert.ToInt16(txtVendorID.Text));
                    if (txtVendorName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Vendor found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    LoadVendorInfo(Convert.ToInt16(txtVendorID.Text));
                    ////Revised PR Module
                    LoadCatNos(Convert.ToInt16(txtVendorID.Text), 0);
                    //LoadCatNos(Convert.ToInt16(txtVendorID.Text));
                    dgvVendorNames.Visible = false;
                }
                else
                {
                    txtVendorID.Text = ""; dgvVendorNames.Visible = false;
                }
            }
        }
        // MY 02/25/2015 - END: txt/dgvVendorNames events       

        // MY 04/03/2015 - START: txt/dgvFees events
        private void dgvFees_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtFeeName.Text = dgvFees.CurrentRow.Cells["Description"].Value.ToString();
                txtFeeCode.Text = dgvFees.CurrentRow.Cells["FeeCode"].Value.ToString();
                dgvFees.Visible = false;
            }
        }

        private void dgvFees_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtFeeName.Text = dgvFees.CurrentRow.Cells["Description"].Value.ToString();
                    txtFeeCode.Text = dgvFees.CurrentRow.Cells["FeeCode"].Value.ToString();
                    dgvFees.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvFees.Visible = false;
                }
            }
        }
        private void txtFeeName_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvFees.Visible = true; dgvFees.BringToFront();
            }
        }

        private void dgvFees_Leave(object sender, EventArgs e)
        {
            dgvFees.Visible = false;
        }

        private void txtFeeName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwFeeNames;
                dvwFeeNames = new DataView(dtOtherFeesList, "Description like '%" + txtFeeName.Text.Trim().Replace("'", "''") + "%'", "Description", DataViewRowState.CurrentRows);
                dvwSetUp(dgvFees, dvwFeeNames);
            }
        }

        private void dgvFees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtFeeName.Text = dgvFees.CurrentRow.Cells["Description"].Value.ToString();
                txtFeeCode.Text = dgvFees.CurrentRow.Cells["FeeCode"].Value.ToString();
                dgvFees.Visible = false;
                dgvFees.BringToFront();
            }           
        }

        private void picFees_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadOtherFees();
                dgvFees.Visible = true; dgvFees.BringToFront();
            }
        }

        private void txtFeeCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtFeeName.Text = PSSClass.Procurements.PROtherFeeName(Convert.ToInt16(txtFeeCode.Text));
                    if (txtFeeName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Fee found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvFees.Visible = false;
                }
                else
                {
                    txtFeeName.Text = ""; dgvFees.Visible = false;
                }
            }
        }

        // MY 04/03/2015 - END: txt/dgvFees events     

        // MY 08/04/2015 - START: txt/dgvCatNames events
        private void dgvCatNames_DoubleClick(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                txtCatName.Text = dgvCatNames.CurrentRow.Cells["CatalogName"].Value.ToString();
                txtCatNameID.Text = dgvCatNames.CurrentRow.Cells["CatalogNameID"].Value.ToString();
                txtCatNo.Text = ""; txtCatDesc.Text = "";
                LoadCatNos(Convert.ToInt16(txtVendorID.Text), Convert.ToInt16(txtCatNameID.Text));
                dgvCatNames.Visible = false;
            }
        }

        private void dgvCatNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCatNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nDMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCatName.Text = dgvCatNames.CurrentRow.Cells["CatalogName"].Value.ToString();
                    txtCatNameID.Text = dgvCatNames.CurrentRow.Cells["CatalogNameID"].Value.ToString();
                    txtCatNo.Text = ""; txtCatDesc.Text = "";
                    LoadCatNos(Convert.ToInt16(txtVendorID.Text), Convert.ToInt16(txtCatNameID.Text));
                    dgvCatNames.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvCatNames.Visible = false;
                }
            }
        }
        private void txtCatName_Enter(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                dgvCatNames.Visible = true; dgvCatNames.BringToFront();
            }
        }

        private void dgvCatNames_Leave(object sender, EventArgs e)
        {
            dgvCatNames.Visible = false;
        }

        private void txtCatName_TextChanged(object sender, EventArgs e)
        {
            if (nDMode != 0)

            {      
                DataView dvwCatNames;
                dvwCatNames = new DataView(dtCatNames, "CatalogName like '%" + txtCatName.Text.Trim().Replace("'", "''") + "%'", "CatalogName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvCatNames, dvwCatNames);
            }
        }

        private void picCatNames_Click(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                LoadCatNames();
                dgvCatNames.Visible = true; dgvCatNames.BringToFront();
            }
        }

        private void txtCatName_KeyPress(object sender, KeyPressEventArgs e)
        {  
            if (e.KeyChar == 13)
            {
                txtCatNo.Select();
                txtCatNo.Focus();
            }
            else if (e.KeyChar == 27)
            {
                dgvCatNames.Visible = false;
            }
        }

        // MY 08/04/2015 - END: txt/dgvCatNames events     

        // MY 07/31/2015 - START: txt/dgvCatNos events
        private void dgvCatNos_DoubleClick(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                txtCatNo.Text = dgvCatNos.CurrentRow.Cells["CatalogNo"].Value.ToString();
                LoadCatInfo(Convert.ToInt16(txtVendorID.Text), txtCatNo.Text);
                dgvCatNos.Visible = false;
            }
        }

        private void dgvCatNos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCatNos_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCatNo.Text = dgvCatNos.CurrentRow.Cells["CatalogNo"].Value.ToString();
                    LoadCatInfo(Convert.ToInt16(txtVendorID.Text), txtCatNo.Text);
                    dgvCatNos.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvCatNos.Visible = false;
                }
            }
        }       

        private void dgvCatNos_Leave(object sender, EventArgs e)
        {
            dgvCatNos.Visible = false;
        }

        private void txtCatNo_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwCatNos;
                dvwCatNos = new DataView(dtCatNos, "CatalogNo like '%" + txtCatNo.Text.Trim().Replace("'", "''") + "%'", "CatalogNo", DataViewRowState.CurrentRows);
                dgvCatNos.Columns[0].Width = 285;
                dgvCatNos.DataSource = dvwCatNos;
            }
        }

        private void picCatNos_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (txtVendorID.Text.Trim() == "")
                {
                    MessageBox.Show("Please select vendor's name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtVendorName.Focus();
                    return;
                }
                //Revised PR Module
                if (txtCatNameID.Text.Trim() == "")
                {
                    txtCatNameID.Text = "0";
                }
                LoadCatNos(Convert.ToInt16(txtVendorID.Text), Convert.ToInt16(txtCatNameID.Text));
                dgvCatNos.Visible = true; dgvCatNos.BringToFront();
            }
        }

        private void txtCatNo_KeyPress(object sender, KeyPressEventArgs e)
        { 
            if (e.KeyChar == 13)
            {
                LoadCatInfo(Convert.ToInt16(txtVendorID.Text), txtCatNo.Text);
                txtCatDesc.Select();
                txtCatDesc.Focus();
            }
            else if (e.KeyChar == 27)
            {
                dgvCatNos.Visible = false;
            }
        }

        // MY 07/31/2015 - END: txt/dgvCatNos events    

        // MY 08/03/2015 - START: txt/dgvCatGrades events
        private void dgvCatGrades_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtGrade.Text = dgvCatGrades.CurrentRow.Cells["CatalogGrade"].Value.ToString();
                txtGradeID.Text = dgvCatGrades.CurrentRow.Cells["GradeID"].Value.ToString();
                dgvCatGrades.Visible = false;
            }
        }

        private void dgvCatGrades_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvCatGrades_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtGrade.Text = dgvCatGrades.CurrentRow.Cells["CatalogGrade"].Value.ToString();
                    txtGradeID.Text = dgvCatGrades.CurrentRow.Cells["GradeID"].Value.ToString();
                    dgvCatGrades.Visible = true;
                }
                else if (e.KeyChar == 27)
                {
                    dgvCatGrades.Visible = false;
                }
            }
        }
       
        private void dgvCatGrades_Leave(object sender, EventArgs e)
        {
            dgvCatGrades.Visible = false;
        }

        private void txtGrade_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwCatGrades;
                dvwCatGrades = new DataView(dtCatGrades, "CatalogGrade like '%" + txtGrade.Text.Trim().Replace("'", "''") + "%'", "CatalogGrade", DataViewRowState.CurrentRows);
                dvwSetUp(dgvCatGrades, dvwCatGrades);
            }
        }

        private void dgvCatGrades_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtGrade.Text = dgvCatGrades.CurrentRow.Cells["CatalogGrade"].Value.ToString();
                txtGradeID.Text = dgvCatGrades.CurrentRow.Cells["GradeID"].Value.ToString();
                dgvCatGrades.Visible = true;
                dgvCatGrades.BringToFront();
            }
        }

        private void picGrades_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadCatGrades();
                dgvCatGrades.Visible = true; dgvCatGrades.BringToFront();
            }
        }

        // MY 08/03/2015 - END: txt/dgvCatGrades events   

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

        private void dgvPRDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BindPRDetails();
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            if (txtVendorID.Text == "")
            {
                MessageBox.Show("Please select vendor.", Application.ProductName);
                return;
            }
            //if (nMode == 0)
            //{
            //    AddEditMode(true);
            //    OpenControls(pnlRecord, true);
            //    nMode = 1;
            //}
            nDMode = 1;
            OpenControls(this.pnlPRDetail, true);

            btnAddDetail.Visible = false; btnEditDetail.Visible = false; btnDeleteDetail.Visible = false;
            btnOKDetail.Visible = true; btnCancelDetail.Visible = true;

            picCatNames.Enabled = true;
            picCatNos.Enabled = true;
            picGrades.Enabled = true;

            lblLock.Text = "Unlocked";
            txtDateCreated.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");

            picCatNames_Click(null, null);
            txtCatName.Select();
            txtCatName.Focus();

            DataRow dR = dtDetail.NewRow();
            dR["CatalogNo"] = "";
            dR["CatalogNameID"] = 0;
            dR["CatalogName"] = "";
            dR["CatalogDesc"] = "";
            dR["GradeID"] = 0;
            dR["Grade"] = "";
            dR["VendorQuoteNo"] = "";
            dR["IsCOARequired"] = false;
            dR["IsMSDRequired"] = false;
            dR["Quantity"] = 0;
            dR["UnitPrice"] = 0;
            dR["TotalPrice"] = 0;
            dR["IsInStock"] = false;
            dR["IsBackOrdered"] = false;
            dR["UnitID"] = 0;
            dR["UnitDesc"] = "";
            dR["CostItemID"] = 0;
            dR["CostItemDesc"] = "";
            dR["PRNo"] = txtPRNo.Text;
            dR["PRDetailID"] = 0;
            dR["DateCreated"] = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            dtDetail.Rows.Add(dR);
            bsPRDetails.Position = dtDetail.Rows.Count - 1;
            dgvPRDetails.Enabled = false;
        }        

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {    
            if (dgvPRDetails.Rows.Count == 0)
                return;

            int dRow = dgvPRDetails.CurrentRow.Index;

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
            dgvPRDetails.Rows.RemoveAt(dRow);
            if (dgvPRDetails.Rows.Count == 0)
            {
                btnDeleteDetail.Enabled = false;
            }
        }        
       
        private void cboPayTerms_Click(object sender, EventArgs e)
        {
            if (cboPayTerms.Text.Trim() != "")
            {
                try
                {
                    LoadPayTerms();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void txtCatNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtCatNo.Text.Trim() != "")
            {
                if (nMode != 0)
                {
                    try
                    {
                        LoadCatInfo(Convert.ToInt16(txtVendorID.Text), txtCatNo.Text);
                    }
                    catch
                    {
                    }
                }
            }

        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;
        }      
        
        private void btnOKDetail_Click(object sender, EventArgs e)
        {
            int nPR = ValidateDetails();                                                         // Validation for PR Detail Record
            if (nPR == 0)
            {
                return;
            }
            dtDetail.Rows[bsPRDetails.Position]["CatalogNo"] = txtCatNo.Text;
            if (txtCatNameID.Text.Trim() == "")
            {
                txtCatNameID.Text = PSSClass.DataEntry.NewID("CatalogNames", "CatalogNameID").ToString();               // for New Catalog names
            }
            dtDetail.Rows[bsPRDetails.Position]["CatalogNameID"] = Convert.ToInt16(txtCatNameID.Text);
            dtDetail.Rows[bsPRDetails.Position]["CatalogName"] = txtCatName.Text;
            dtDetail.Rows[bsPRDetails.Position]["CatalogDesc"] = txtCatDesc.Text;
            dtDetail.Rows[bsPRDetails.Position]["GradeID"] = txtGradeID.Text;
            dtDetail.Rows[bsPRDetails.Position]["Grade"] = txtGrade.Text;
            dtDetail.Rows[bsPRDetails.Position]["VendorQuoteNo"] = txtVendorQuoteNo.Text;
            dtDetail.Rows[bsPRDetails.Position]["IsCOARequired"] = chkCOA.CheckState;
            dtDetail.Rows[bsPRDetails.Position]["IsMSDRequired"] = chkMSD.CheckState;
            dtDetail.Rows[bsPRDetails.Position]["Quantity"] = Convert.ToInt16(txtQuantity.Text);
            dtDetail.Rows[bsPRDetails.Position]["UnitPrice"] = Convert.ToDecimal(txtUnitPrice.Text);
            dtDetail.Rows[bsPRDetails.Position]["TotalPrice"] = Convert.ToDecimal(txtTotPrice.Text);
            dtDetail.Rows[bsPRDetails.Position]["IsInStock"] = chkInStock.CheckState;
            dtDetail.Rows[bsPRDetails.Position]["IsBackOrdered"] = chkBackOrdered.CheckState;
            dtDetail.Rows[bsPRDetails.Position]["UnitDesc"] = txtUnit.Text;
            dtDetail.Rows[bsPRDetails.Position]["UnitID"] = txtUnitID.Text;
            dtDetail.Rows[bsPRDetails.Position]["CostItemDesc"] = txtCostItem.Text;
            dtDetail.Rows[bsPRDetails.Position]["CostItemID"] = txtCostItemID.Text;
            dtDetail.Rows[bsPRDetails.Position]["CreatedByID"] = LogIn.nUserID;
            dtDetail.Rows[bsPRDetails.Position]["DateCreated"] = DateTime.Now;
            dtDetail.Rows[bsPRDetails.Position]["LastUpdate"] = DateTime.Now;
            dtDetail.Rows[bsPRDetails.Position]["LastUserID"] = LogIn.nUserID;
            dtDetail.Rows[bsPRDetails.Position]["PRNo"] = txtPRNo.Text;
            dtDetail.Rows[bsPRDetails.Position]["PRDetailID"] = 1;
            //if (nDMode == 1)
            //{
            //    DataRow dR = dtDetail.NewRow();
            //    dR["CatalogNo"] = txtCatNo.Text;
            //    if (txtCatNameID.Text.Trim() == "")
            //    {
            //        txtCatNameID.Text = PSSClass.DataEntry.NewID("CatalogNames", "CatalogNameID").ToString();               // for New Catalog names
            //    }
            //    dR["CatalogNameID"] = Convert.ToInt16(txtCatNameID.Text);
            //    dR["CatalogName"] = txtCatName.Text;
            //    dR["CatalogDesc"] = txtCatDesc.Text;
            //    dR["GradeID"] = txtGradeID.Text;
            //    dR["Grade"] = txtGrade.Text;
            //    dR["VendorQuoteNo"] = txtVendorQuoteNo.Text;
            //    dR["IsCOARequired"] = chkCOA.CheckState;
            //    dR["IsMSDRequired"] = chkMSD.CheckState;
            //    dR["Quantity"] = Convert.ToInt16(txtQuantity.Text);
            //    dR["UnitPrice"] = Convert.ToDecimal(txtUnitPrice.Text);
            //    dR["TotalPrice"] = Convert.ToDecimal(txtTotPrice.Text);
            //    dR["IsInStock"] = chkInStock.CheckState;
            //    dR["IsBackOrdered"] = chkBackOrdered.CheckState;
            //    dR["UnitDesc"] = txtUnit.Text;
            //    dR["UnitID"] = txtUnitID.Text;
            //    dR["CostItemDesc"] = txtCostItem.Text;
            //    dR["CostItemID"] = txtCostItemID.Text;
            //    dR["CreatedByID"] = LogIn.nUserID;
            //    dR["DateCreated"] = DateTime.Now;
            //    dR["LastUpdate"] = DateTime.Now;
            //    dR["LastUserID"] = LogIn.nUserID;
            //    dR["PRNo"] = txtPRNo.Text;
            //    dR["PRDetailID"] = 1;
            //    dtDetail.Rows.Add(dR);
            //    bsPRDetails.Position = dtDetail.Rows.Count - 1;
            //}
            bsPRDetails.DataSource = dtDetail;
            //dgvPRDetails.DataSource = bsPRDetails;
            //BindPRDetails();
            btnAddDetail.Enabled = true;
            btnDeleteDetail.Enabled = true;
            btnAddDetail.Visible = true; btnEditDetail.Visible = true; btnDeleteDetail.Visible = true;
            btnOKDetail.Visible = false; btnCancelDetail.Visible = false;
            dgvPRDetails.Enabled = true;
            DataGridPRDetailsSetting();
            dgvPRDetails.CurrentCell = dgvPRDetails.Rows[0].Cells[0];
            dgvPRDetails.Rows[0].Selected = true;
            OpenControls(this.pnlPRDetail, false);
            lblLock.Text = "Locked";
        }
      
        private void btnAddCharges_Click(object sender, EventArgs e)
        {
            btnAddCharges.Enabled = false;       
            pnlOtherFees.Visible = true; pnlOtherFees.BringToFront();
            OpenControls(this.pnlOtherFees, true);
            btnAddFee.Enabled = true;
            if (dgvFees.RowCount > 0)
            {
                btnDeleteFee.Enabled = true;
            }
            GetPRSubTotalOtherFees();
        }

        private void btnCloseFees_Click(object sender, EventArgs e)
        {
            pnlOtherFees.Visible = false;
            btnAddCharges.Enabled = true;           
            btnClose.Enabled = true;
            GetPRSubTotalOtherFees();
        }

        private void btnAddFee_Click(object sender, EventArgs e)
        {
            nMode = 1;
            btnClose.Enabled = false;
            ClearControls(this.pnlOtherFees);
            OpenControls(this.pnlOtherFees, true);

            txtFeeName.Focus();
            btnAddFee.Enabled = false;
            btnDeleteFee.Enabled = false;
            btnOKFee.Enabled = true;

            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;

            foreach (Control c in pnlOtherFees.Controls)
            {
                c.DataBindings.Clear();
            }
        }

        private void btnDeleteFee_Click(object sender, EventArgs e)
        {
            int dRow = dgvPROtherFees.CurrentRow.Index;
            int intOtherFeesID;

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
                     
            intOtherFeesID = Convert.ToInt16(dgvPROtherFees.CurrentRow.Cells["OtherFeesID"].Value.ToString());

            dgvPROtherFees.Rows.RemoveAt(dRow);

            if (dgvPROtherFees.Rows.Count == 0)
            {
                btnDeleteFee.Enabled = false;
            }

            DeleteOtherFees(intOtherFeesID);

            AddEditMode(false);
        }

        private void btnOKFee_Click(object sender, EventArgs e)
        {
            int nPR = ValidateOtherFees();                                                         // Validation for PR Other Fees Record
            if (nPR == 0)
            {
                return;
            }

            DataRow dR = dtOtherFees.NewRow();

            dR["FeeCode"] = txtFeeCode.Text;
            dR["FeeDesc"] = txtFeeName.Text;
            dR["Amount"] = txtFeeAmount.Text;            
            dR["CreatedByID"] = LogIn.nUserID;
            dR["DateCreated"] = DateTime.Now;
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;
            dR["PRNo"] = txtPRNo.Text;
            dR["OtherFeesID"] = 1;
            dtOtherFees.Rows.Add(dR);
            bsPROtherFees.DataSource = dtOtherFees;
            dgvPROtherFees.DataSource = bsPROtherFees;            

            BindPROtherFees();
            GetPRSubTotalOtherFees();
            btnAddFee.Enabled = true;
            btnDeleteFee.Enabled = true;
            btnAddFee.Focus();
            btnOKFee.Enabled = false;
            tsbSave.Enabled = true;
        }

        private void txtFeeAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != (char)46;            
        }  

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (dgvPRDetails.RowCount == 0)
            {
                MessageBox.Show("There are no items yet for this PR!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Are you sure you want to generate a PO for this?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@PRNo", txtPRNo.Text.Trim());            
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddPORecord";

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
            MessageBox.Show("New PO has been successfully created!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnTestData_Click(object sender, EventArgs e)
        {
            bsPRMaster.EndEdit();

            dtMaster.Rows[bsPRMaster.Position]["PRNo"].ToString();
            for (int i = 0; i < dtMaster.Rows.Count; i++)
            {
                MessageBox.Show(dtMaster.Rows[i]["PRNo"].ToString());
                MessageBox.Show(dtMaster.Rows[i].RowState.ToString());
            }
        }       

        private void GetItemTotalPrice()
        {
            decimal totPrice = 0;

            if (txtQuantity.Text.Trim() != "" && txtUnitPrice.Text.Trim() != "")
            {
                decimal unitPrice = Convert.ToDecimal(txtUnitPrice.Text);
                int qty = Convert.ToInt16(txtQuantity.Text);
                totPrice = unitPrice * qty;
            }
            txtTotPrice.Text = Convert.ToString(totPrice);
        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            GetItemTotalPrice();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            GetItemTotalPrice();
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(CatalogNames));

            if (intOpen == 1)
            {
                PSSClass.General.CloseForm(typeof(CatalogNames));
            }
            CatalogNames childForm = new CatalogNames();
            childForm.Text = "CATALOG NAMES";          
            childForm.MdiParent = this.MdiParent;
            childForm.nCatNameSw = 1;
            childForm.Show();   
        }

        private void btnAddCatalog_Click(object sender, EventArgs e)
        {
            if (txtVendorID.Text.Trim() == "")
            {
                MessageBox.Show("Please choose Vendor first!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtVendorID.Focus();
                return;
            }           

            int intOpen = PSSClass.General.OpenForm(typeof(Catalog));

            if (intOpen == 1)
            {
                PSSClass.General.CloseForm(typeof(Catalog));
            }
            Catalog childForm = new Catalog();
            childForm.Text = "CATALOG MASTER";
            childForm.MdiParent = this.MdiParent;
            childForm.strPRVendorID = txtVendorID.Text;
            childForm.strPRVendorName = txtVendorName.Text;
            childForm.strPRCatNameID = txtCatNameID.Text;
            childForm.strPRCatName = txtCatName.Text;
            childForm.nCatMasterSw = 1;
            childForm.Show();        
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintReport();
        }

        private void btnCheckBrowser_Click(object sender, EventArgs e)
        {
            if (txtWebsite.Text.Trim() == "")
            {
                MessageBox.Show("Vendor website address empty!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtWebsite.Focus();
                return;
            }       

            try
            {
                txtWebsite.Text = PSSClass.Procurements.CatalogVendorWebsite(Convert.ToInt16(txtVendorID.Text));

                System.Diagnostics.Process.Start(txtWebsite.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Please enter a valid URL for this vendor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void txtPONo_Click(object sender, EventArgs e)
        {            
            int intOpen = PSSClass.General.OpenForm(typeof(PurchaseOrder));

            if (intOpen == 1)
            {
                PSSClass.General.CloseForm(typeof(PurchaseOrder));
            }
            PurchaseOrder childForm = new PurchaseOrder();
            childForm.Text = "PURCHASE ORDER";
            childForm.MdiParent = this.MdiParent;
            childForm.strPONo = txtPONo.Text;
            childForm.nPRModuleSw = 1;            
            childForm.Show();       
        }

        private void picUnits_Click(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                LoadUnits();
                dgvUnits.Visible = true; dgvUnits.BringToFront();
            }
        }

        private void picCostItems_Click(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                LoadCostItems();
                dgvCostItems.Visible = true; dgvCostItems.BringToFront();
            }
        }

        private void txtCostItem_Enter(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                HideMasterGrids(); HideDetailsGrids();
                dgvCostItems.Visible = true; dgvCostItems.BringToFront();
            }
        }

        private void HideMasterGrids()
        {
            dgvRequestors.Visible = false; dgvDeptNames.Visible = false; dgvCostCenters.Visible = false; dgvVendorNames.Visible = false;
        }

        private void HideDetailsGrids()
        {
            dgvCatNames.Visible = false; dgvCatNos.Visible = false; dgvCatGrades.Visible = false; dgvUnits.Visible = false; dgvCostItems.Visible = false;
        }

        private void txtCostItem_TextChanged(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                DataView dvwCostItems;
                dvwCostItems = new DataView(dtCostItems, "CostItemDesc like '%" + txtCostItem.Text.Trim().Replace("'", "''") + "%'", "CostItemDesc", DataViewRowState.CurrentRows);
                dvwSetUpWidth(dgvCostItems, dvwCostItems, 318);
            }
        }

        private void txtUnit_TextChanged(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                DataView dvwUnits;
                dvwUnits = new DataView(dtUnits, "ShortDesc like '%" + txtUnit.Text.Trim().Replace("'", "''") + "%'", "ShortDesc", DataViewRowState.CurrentRows);
                dvwSetUpWidth(dgvUnits, dvwUnits, 100);
            }
        }

        private void txtUnit_Enter(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                dgvUnits.Visible = true; dgvUnits.BringToFront();
                dgvCatGrades.Visible = false; dgvCatNos.Visible = false; dgvCatNames.Visible = false; dgvCostItems.Visible = false;
            }
        }

        private void txtUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtUnitID.Text = "";
        }

        private void btnEditDetail_Click(object sender, EventArgs e)
        {
            nDMode = 2;
            dgvPRDetails.Enabled = false;
            OpenControls(pnlPRDetail, true);
            btnAddDetail.Visible = false; btnEditDetail.Visible = false; btnDeleteDetail.Visible = false;
            btnCancelDetail.Visible = true; btnOKDetail.Visible = true;
        }

        private void dgvCostItems_DoubleClick(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                txtCostItem.Text = dgvCostItems.CurrentRow.Cells["CostItemDesc"].Value.ToString();
                txtCostItemID.Text = dgvCostItems.CurrentRow.Cells["CostItemID"].Value.ToString();
                dgvCostItems.Visible = false;
            }
        }

        private void dgvCostItems_Leave(object sender, EventArgs e)
        {
            dgvCostItems.Visible = false;
        }

        private void dgvCostItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nDMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCostItem.Text = dgvCostItems.CurrentRow.Cells["CostItemDesc"].Value.ToString();
                    txtCostItemID.Text = dgvCostItems.CurrentRow.Cells["CostItemID"].Value.ToString();
                    dgvCostItems.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvCostItems.Visible = false;
                }
            }
        }

        private void btnCancelDetail_Click(object sender, EventArgs e)
        {
            if (nDMode == 1)
                //dtDetail.RejectChanges();
                dtDetail.Rows.RemoveAt(dtDetail.Rows.Count - 1);
            else
                bsPRDetails.CancelEdit();

            //BindPRDetails();
            dgvCatNames.Visible = false; dgvCatNos.Visible = false; dgvCatGrades.Visible = false; dgvUnits.Visible = false; dgvCostItems.Visible = false;
            btnAddDetail.Visible = true; btnEditDetail.Visible = true; btnDeleteDetail.Visible = true;
            btnOKDetail.Visible = false; btnCancelDetail.Visible = false;
            dgvPRDetails.Enabled = true;
            dgvPRDetails.CurrentCell = dgvPRDetails.Rows[0].Cells[0];
            dgvPRDetails.Rows[0].Selected = true;
            OpenControls(pnlPRDetail, false);
            lblLock.Text = "Locked";
            nDMode = 0;
        }

        private void dgvUnits_DoubleClick(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                txtUnit.Text = dgvUnits.CurrentRow.Cells["ShortDesc"].Value.ToString();
                txtUnitID.Text = dgvUnits.CurrentRow.Cells["UnitID"].Value.ToString();
                dgvUnits.Visible = false;
            }
        }

        private void dgvUnits_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nDMode != 0)
            {
                txtUnit.Text = dgvUnits.CurrentRow.Cells["ShortDesc"].Value.ToString();
                txtUnitID.Text = dgvUnits.CurrentRow.Cells["UnitID"].Value.ToString();
                dgvUnits.Visible = false;
            }
        }

        private void dgvUnits_Leave(object sender, EventArgs e)
        {
            dgvUnits.Visible = false;
        }

        private void txtCatNo_Enter(object sender, EventArgs e)
        {
            dgvCatNos.Visible = true; dgvCatNos.BringToFront();
        }

        private void txtConvUnitID_Enter(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                dgvConvUnits.Visible = true; dgvConvUnits.BringToFront();
                dgvUnits.Visible = false; dgvCatGrades.Visible = false; dgvCatNos.Visible = false; dgvCatNames.Visible = false; dgvCostItems.Visible = false;
            }
        }

        private void dgvConvUnits_DoubleClick(object sender, EventArgs e)
        {
            if (nDMode != 0)
            {
                txtConvUnit.Text = dgvConvUnits.CurrentRow.Cells["ShortDesc"].Value.ToString();
                txtConvUnitID.Text = dgvConvUnits.CurrentRow.Cells["UnitID"].Value.ToString();
                dgvConvUnits.Visible = false;
            }
        }
    }
}
