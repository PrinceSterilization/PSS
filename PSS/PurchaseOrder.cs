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
using System.Net.Mail;
using System.Net;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;

namespace PSS
{
    public partial class PurchaseOrder : PSS.TemplateForm
    {
        public string strPONo;
        public byte nPOSw;

        public byte nPRModuleSw;

        byte nMode = 0;        

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";

        DataTable dtRequestors = new DataTable();                                           // MY 01/06/2015 - Pop-up GridView Requestors query
        DataTable dtInspectors = new DataTable();                                           // MY 03/04/2015 - Pop-up GridView Inspectors query
        DataTable dtCostCenters = new DataTable();                                          // MY 07/27/2015 - Pop-up GridView Cost Centers query
        DataTable dtVendors = new DataTable();                                              // MY 02/25/2015 - Pop-up GridView Vendors query
        DataTable dtCatNos = new DataTable();                                               // MY 07/31/2015 - Pop-up GridView Cat Nos query    
        DataTable dtCatNames = new DataTable();                                             // MY 07/31/2015 - Pop-up GridView Cat Names query   
        DataTable dtCatInfo = new DataTable();                                              // MY 02/25/2015 - Pop-up GridView Cat No query    
        DataTable dtOtherFeesList = new DataTable();                                        // MY 07/14/2015 - Pop-up GridView Fees query
        DataTable dtVendorInfo = new DataTable();                                           // MY 02/27/2015 - GridView Vendor Info query
      
        DataTable dtMaster = new DataTable();                                               // MY 07/07/2015 - datatable for Master
        DataTable dtDetail = new DataTable();                                               // MY 07/07/2015 - datatable for Detail
        DataTable dtOtherFees = new DataTable();                                            // MY 07/09/2015 - datatable for Other Fees

        public PurchaseOrder()
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
            tsbAdd.Enabled = false;
        }

        private void LoadRecords()
        {
            nMode = 0;
    
            DataTable dt = new DataTable();
            dt = PSSClass.Procurements.POMaster();

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
            pnlPODetailGrid.Visible = false;
            pnlPODetail.Visible = false;
            pnlOtherFees.Visible = false;
            pnlRecord.Visible = false;            
            FileAccess();           
        }

        private void FileAccess()
        {
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; btnSubmit.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = true; btnSubmit.Enabled = false;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = true; btnSubmit.Enabled = true;
            }
        }

        private void LoadPOMaster(string cCmpyCode, string cPONo)
        {
            try
            {
                dtMaster = null;
                dtMaster = PSSClass.Procurements.POMain(cCmpyCode, cPONo);
                bsPOMaster.DataSource = dtMaster;
                BindPOMaster();
            }
            catch { }
        }

        private void LoadPODetails(string cCmpyCode, string cPONo)
        {
            dtDetail = null;
            dtDetail = PSSClass.Procurements.PODetails(cCmpyCode, cPONo);
            if (dtDetail == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            bsPODetails.DataSource = dtDetail;
            bnPODetails.BindingSource = bsPODetails;
            dgvPODetails.DataSource = bsPODetails;
            DataGridPODetailsSetting();
            BindPODetails();            
        }

        private void LoadPOOtherFees(string cCmpyCode, string cPONo)
        {
            dtOtherFees = null;
            dtOtherFees = PSSClass.Procurements.POOtherFees(cCmpyCode, cPONo);
            if (dtOtherFees == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }

            bsPOOtherFees.DataSource = dtOtherFees;
            bnPOOtherFees.BindingSource = bsPOOtherFees;
            dgvPOOtherFees.DataSource = bsPOOtherFees;
            DataGridOtherFeesSetting();
            BindPOOtherFees();             
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

        private void LoadCatNos(Int16 cVendorID, Int16 cCatalogNameID)
        {
            dgvCatNos.DataSource = null;

            dtCatNos = PSSClass.Procurements.POCatNos(cVendorID, cCatalogNameID);
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

            dtCatNames = PSSClass.Procurements.CatalogNames();
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

            dtCatInfo = PSSClass.Procurements.POCatInfo(cVendorID, cCatNo);
            if (dtCatInfo == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCatInfo.DataSource = dtCatInfo;

            if (dgvCatInfo.Rows.Count != 0)
            {
                txtCatDesc.Text = dgvCatInfo.Rows[0].Cells["CatalogDesc"].Value.ToString();
                txtGradeID.Text = dgvCatInfo.Rows[0].Cells["GradeID"].Value.ToString();
                txtGrade.Text = dgvCatInfo.Rows[0].Cells["Grade"].Value.ToString();               
                txtUnitPrice.Text = dgvCatInfo.Rows[0].Cells["UnitPrice"].Value.ToString();
            }
        }

        private void LoadCostCenters()
        {
            dgvCostCenters.DataSource = null;

            dtCostCenters = PSSClass.Procurements.POMCostCenters();
            if (dtCostCenters == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvCostCenters.DataSource = dtCostCenters;
            StandardDGVSetting(dgvCostCenters);
            dgvCostCenters.Columns[0].Width = 377;
            dgvCostCenters.Columns[1].Visible = false;
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

        private void CheckInspector()
        {
            chkDoesMeetSpecs.Enabled = false;
            txtInspectedByID.Enabled = false;
            txtInspector.Enabled = false;
            picInspectors.Enabled = false;
            mskDateInspected.Enabled = false;

            if (txtApprover.Text.Trim() == "")
            {
                return;
            }

            bool blnInspectorExists;

            blnInspectorExists = PSSClass.Procurements.POInspectorExists(LogIn.nUserID);

            if (blnInspectorExists)
            {
                chkDoesMeetSpecs.Enabled = true;
                txtInspectedByID.Enabled = true;
                txtInspector.Enabled = true;
                picInspectors.Enabled = true;
                mskDateInspected.Enabled = true;
            }            
        }

        private void LoadInspectors()
        {
            dgvInspectors.DataSource = null;

            dtInspectors = PSSClass.Procurements.PODInspectors();
            if (dtInspectors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvInspectors.DataSource = dtInspectors;
            StandardDGVSetting(dgvInspectors);
            dgvInspectors.Columns[0].Width = 180;
            dgvInspectors.Columns[1].Visible = false;
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
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0] = new ToolStripMenuItem();
            items[0].Name = "PurchaseOrderItemsNotReceived";
            items[0].Text = "PO Items Not Received";
            items[0].Click += new EventHandler(PrintPOItemsNotReceivedClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Procurements.POMaster();

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

        private void GetPOSubTotalOtherFees()
        {
            decimal totOtherFees = 0;
            decimal amount = 0;

            for (int j = 0; j < dgvPOOtherFees.Rows.Count; j++)
            {
                amount = Convert.ToDecimal(dgvPOOtherFees.Rows[j].Cells["Amount"].Value.ToString());
                totOtherFees = totOtherFees + amount;
            }                
          
            txtSubOtherFees.Text = totOtherFees.ToString("C");
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

        private void PrintPurchaseOrderClickHandler(object sender, EventArgs e)
        {
            PurchaseOrdersRpt rpt = new PurchaseOrdersRpt();

            rpt.WindowState = FormWindowState.Maximized;
            rpt.PONo = txtPONo.Text;

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void PrintPOItemsNotReceivedClickHandler(object sender, EventArgs e)
        {
            PurchaseOrderItemsNotReceived rpt = new PurchaseOrderItemsNotReceived();

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
                    bsFile.Filter = "PONo<>''";
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
            bsFile.Filter = "PONo<>''";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {        
            nMode = 0;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            pnlPODetailGrid.Visible = true; pnlPODetail.Visible = true;
            btnClose.Visible = true; btnClose.BringToFront();

            btnAddDetail.Enabled = false;
            btnDeleteDetail.Enabled = false;
            btnOKDetail.Enabled = false;
            btnAddFee.Enabled = false;
            btnDeleteFee.Enabled = false;
            btnOKFee.Enabled = false;
            btnAddCharges.Enabled = false;
            btnDeleteFee.Enabled = false;
            btnCheckBrowser.Enabled = false;
            btnPrint.Enabled = false;
 
            ClearPOMaster();
            ClearPODetail();
            ClearPOOtherFees();

            //dgvFile.Rows[0].Selected = true;
            txtCmpyCode.Text = dgvFile.CurrentRow.Cells["CompanyCode"].Value.ToString();
            txtPONo.Text = dgvFile.CurrentRow.Cells["PONo"].Value.ToString();

            LoadPOMaster(txtCmpyCode.Text, txtPONo.Text);
            LoadPODetails(txtCmpyCode.Text, txtPONo.Text);
            LoadPOOtherFees(txtCmpyCode.Text, txtPONo.Text);

            if (txtVendorID.Text != "")
            {
                LoadVendorInfo(Convert.ToInt16(txtVendorID.Text));
            }

            OpenControls(pnlRecord, false);
            OpenControls(pnlPODetail, false);
            OpenControls(pnlOtherFees, false);

            mskDateBackOrder.Enabled = false;
            mskDateInspected.Enabled = false;

            btnDeleteDetail.Enabled = false; 
           
            btnDeleteFee.Enabled = false;
            btnSubmit.Enabled = false;
            btnApproverESign.Enabled = false;
            btnPresApproverESign.Enabled = false;

            if (strFileAccess == "FA")
            {
                if (dgvPODetails.RowCount != 0)
                {
                    btnDeleteFee.Enabled = true;
                }

                if (txtReviewer.Text.Trim() != "")
                {
                    btnSubmit.Enabled = false;
                }
                else
                {
                    btnSubmit.Enabled = true;
                }

                if (txtReviewer.Text.Trim() != "" && txtApprover.Text.Trim() == "")
                {
                    btnApproverESign.Enabled = true;
                }
                else
                {
                    btnApproverESign.Enabled = false;
                }

                if (Convert.ToDecimal(txtPOAmount.Text) >= 2500)
                {
                    if (txtReviewer.Text.Trim() != "" && txtApprover.Text.Trim() != "" && txtPresApprover.Text.Trim() == "")
                    {
                        btnPresApproverESign.Enabled = true;
                    }
                    else
                    {
                        btnPresApproverESign.Enabled = false;
                    }
                }
            }

            if (strFileAccess != "RO")
            {
                btnPrint.Enabled = true; btnEMailPO.Enabled = true;
            }
                     
            txtPONo.Focus();  
        }

        private void LoadEmailSignatories()
        {
            if (txtDateSubmitted.Text != "")
            {              
                if (txtReviewDate.Text == "")
                {
                    btnReviewerESign.Enabled = true;
                    btnApproverESign.Enabled = false;
                    btnPresApproverESign.Enabled = false;
                    btnEMail.Enabled = false;
                }
                else
                {
                    btnReviewerESign.Enabled = false;
                    if (txtApprovalDate.Text == "")
                    {
                        btnApproverESign.Enabled = true;
                        btnPresApproverESign.Enabled = false;
                        btnEMail.Enabled = false;
                    }
                    else
                    {
                        btnApproverESign.Enabled = false;
                        if (Convert.ToDecimal(txtPOAmount.Text) > 25000)                                // If PR Amount is over $25K, President's approval is required
                        {
                            if (txtPresApprovalDate.Text == "")
                            {
                                btnPresApproverESign.Enabled = true;
                                btnEMail.Enabled = false;
                            }
                            else
                            {
                                btnPresApproverESign.Enabled = false;
                                if (txtDateEmailed.Text == "")
                                    btnEMail.Enabled = true;
                                else
                                    btnEMail.Enabled = false;
                            }
                        }
                        else
                        {
                            btnPresApproverESign.Enabled = false;
                            if (txtDateEmailed.Text == "")
                                btnEMail.Enabled = true;
                            else
                                btnEMail.Enabled = false;
                        }
                    }
                }
            }

            else
            {
                //chkSubmitForReview.Enabled = true;
                //chkOtherCharges.Enabled = false;
                //chkPOApproval.Enabled = false;
                //chkDLPApproval.Enabled = false;
            }
           
        }
               
        private void BindPOMaster()
        {
            // Clear bindings first
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
            txtCmpyCode.DataBindings.Add("Text", bsPOMaster, "CompanyCode");
            txtPRNo.DataBindings.Add("Text", bsPOMaster, "PRNo");
            txtPONo.DataBindings.Add("Text", bsPOMaster, "PONo");
            txtDeptID.DataBindings.Add("Text", bsPOMaster, "DepartmentID");
            txtDeptName.DataBindings.Add("Text", bsPOMaster, "DepartmentName");
            txtCostCenterID.DataBindings.Add("Text", bsPOMaster, "CostCenterID");
            txtCostCenterName.DataBindings.Add("Text", bsPOMaster, "CostCenterName");
            txtGLCode.DataBindings.Add("Text", bsPOMaster, "GLCode");
            txtRequestedBy.DataBindings.Add("Text", bsPOMaster, "RequestedBy");
            txtRequestor.DataBindings.Add("Text", bsPOMaster, "Requestor");
            txtVendorID.DataBindings.Add("Text", bsPOMaster, "VendorID");
            txtVendorName.DataBindings.Add("Text", bsPOMaster, "VendorName");
            txtConfirmNo.DataBindings.Add("Text", bsPOMaster, "ConfirmNo");
            txtAcctNo.DataBindings.Add("Text", bsPOMaster, "AcctNo");
            cboPayTerms.DataBindings.Add("Text", bsPOMaster, "PayTerms");
            txtContact.DataBindings.Add("Text", bsPOMaster, "ContactName");
            txtEmail.DataBindings.Add("Text", bsPOMaster, "Email");
            txtLineItemTotal.DataBindings.Add("Text", bsPOMaster, "LineItemTotal");
            txtOtherCharges.DataBindings.Add("Text", bsPOMaster, "OtherCharges");  
            txtPOAmount.DataBindings.Add("Text", bsPOMaster, "TotalPOAmount");          
            chkIsCancelled.DataBindings.Add("Checked", bsPOMaster, "IsCancelled");
            chkSubmitForReview.DataBindings.Add("Checked", bsPOMaster, "SubmittedForReview");            
            chkOtherCharges.DataBindings.Add("Checked", bsPOMaster, "OtherChargesReviewed");
            chkPOApproval.DataBindings.Add("Checked", bsPOMaster, "ForPOApproval");
            chkDLPApproval.DataBindings.Add("Checked", bsPOMaster, "ForDLPApproval");
            txtReviewedByID.DataBindings.Add("Text", bsPOMaster, "ReviewedByID");
            txtReviewer.DataBindings.Add("Text", bsPOMaster, "Reviewer");           
            txtApprovedByID.DataBindings.Add("Text", bsPOMaster, "ApprovedByID");
            txtApprover.DataBindings.Add("Text", bsPOMaster, "Approver");            
            txtPressID.DataBindings.Add("Text", bsPOMaster, "ExecutiveID");
            txtPresApprover.DataBindings.Add("Text", bsPOMaster, "ExecutiveApprover");
            txtEmailedByID.DataBindings.Add("Text", bsPOMaster, "EmailedByID");
            txtEmailedBy.DataBindings.Add("Text", bsPOMaster, "EmailedBy");   

            Binding DatePRCreatedBinding;
            DatePRCreatedBinding = new Binding("Text", bsPOMaster, "PRDate");
            DatePRCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtPRDate.DataBindings.Add(DatePRCreatedBinding);

            Binding DatePOCreatedBinding;
            DatePOCreatedBinding = new Binding("Text", bsPOMaster, "PODate");
            DatePOCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtPODate.DataBindings.Add(DatePOCreatedBinding);

            Binding DateCancelledBinding;
            DateCancelledBinding = new Binding("Text", bsPOMaster, "DateCancelled");
            DateCancelledBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtDateCancelled.DataBindings.Add(DateCancelledBinding);

            Binding DateReviewedBinding;
            DateReviewedBinding = new Binding("Text", bsPOMaster, "ReviewDate");
            DateReviewedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtReviewDate.DataBindings.Add(DateReviewedBinding);

            Binding DateSubmittedBinding;
            DateSubmittedBinding = new Binding("Text", bsPOMaster, "DateSubmitted");
            DateSubmittedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtDateSubmitted.DataBindings.Add(DateSubmittedBinding);

            Binding DateOtherChargesBinding;
            DateOtherChargesBinding = new Binding("Text", bsPOMaster, "DateOtherCharges");
            DateOtherChargesBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtDateOtherCharges.DataBindings.Add(DateOtherChargesBinding);         

            Binding DateApprovedBinding;
            DateApprovedBinding = new Binding("Text", bsPOMaster, "ApprovalDate");
            DateApprovedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtApprovalDate.DataBindings.Add(DateApprovedBinding);

            Binding DatePOApprovalBinding;
            DatePOApprovalBinding = new Binding("Text", bsPOMaster, "DatePOApproval");
            DatePOApprovalBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtDatePOApproval.DataBindings.Add(DatePOApprovalBinding);

            Binding DateDLPApprovalBinding;
            DateDLPApprovalBinding = new Binding("Text", bsPOMaster, "DateDLPApproval");
            DateDLPApprovalBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtDateDLPApproval.DataBindings.Add(DateDLPApprovalBinding);

            Binding DateExecApprovalBinding;
            DateExecApprovalBinding = new Binding("Text", bsPOMaster, "ExecApprovalDate");
            DateExecApprovalBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtPresApprovalDate.DataBindings.Add(DateExecApprovalBinding);

            Binding DateEmailedBinding;
            DateEmailedBinding = new Binding("Text", bsPOMaster, "DateEmailed");
            DateEmailedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtDateEmailed.DataBindings.Add(DateEmailedBinding); 
        }

        private void BindPODetails()
        {
            // Clear bindings first
            foreach (Control c in pnlPODetail.Controls)
            {
                c.DataBindings.Clear();
            }

            ClearControls(this.pnlPODetail);    

            try
            {                
                txtCatNo.DataBindings.Add("Text", bsPODetails, "CatalogNo");
                txtCatNameID.DataBindings.Add("Text", bsPODetails, "CatalogNameID");
                txtCatName.DataBindings.Add("Text", bsPODetails, "CatalogName");
                txtCatDesc.DataBindings.Add("Text", bsPODetails, "CatalogDesc");
                txtGradeID.DataBindings.Add("Text", bsPODetails, "GradeID");
                txtGrade.DataBindings.Add("Text", bsPODetails, "Grade");
                txtVendorQuoteNo.DataBindings.Add("Text", bsPODetails, "VendorQuoteNo");
                chkCOA.DataBindings.Add("Checked", bsPODetails, "IsCOARequired");
                chkMSD.DataBindings.Add("Checked", bsPODetails, "IsMSDRequired");
                txtQuantity.DataBindings.Add("Text", bsPODetails, "Quantity");               
                txtUnitPrice.DataBindings.Add("Text", bsPODetails, "UnitPrice");
                txtTotPrice.DataBindings.Add("Text", bsPODetails, "TotalPrice");
                chkInStock.DataBindings.Add("Checked", bsPODetails, "IsInStock");
                chkBackOrdered.DataBindings.Add("Checked", bsPODetails, "IsBackOrdered");
                chkRejectItem.DataBindings.Add("Checked", bsPODetails, "IsRejected");
                txtRejectReason.DataBindings.Add("Text", bsPODetails, "RejectReason");
                chkDoesMeetSpecs.DataBindings.Add("Checked", bsPODetails, "DoesMeetSpecs");
                txtInspectedByID.DataBindings.Add("Text", bsPODetails, "InspectedByID");
                txtInspector.DataBindings.Add("Text", bsPODetails, "Inspector");
                txtDetPRNo.DataBindings.Add("Text", bsPODetails, "PONo");
                txtPODetailID.DataBindings.Add("Text", bsPODetails, "PODetailID");

                Binding DateDetailCreatedBinding;
                DateDetailCreatedBinding = new Binding("Text", bsPODetails, "DateCreated");
                DateDetailCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
                txtDateCreated.DataBindings.Add(DateDetailCreatedBinding);               

                Binding DateInspectedBinding;
                DateInspectedBinding = new Binding("Text", bsPODetails, "DateInspected");
                DateInspectedBinding.Format += new ConvertEventHandler(DateBinding_Format);
                mskDateInspected.DataBindings.Add(DateInspectedBinding);

                Binding DateBackOrderBinding;
                DateBackOrderBinding = new Binding("Text", bsPODetails, "DateBackOrder");
                DateBackOrderBinding.Format += new ConvertEventHandler(DateBinding_Format);
                mskDateBackOrder.DataBindings.Add(DateBackOrderBinding);
            }
            catch
            { }
        }

        private void BindPOOtherFees()
        {
            // Clear bindings first
            foreach (Control c in pnlOtherFees.Controls)
            {
                c.DataBindings.Clear();
            }

            ClearControls(this.pnlOtherFees);                

            txtFeeCode.DataBindings.Add("Text", bsPOOtherFees, "FeeCode");
            txtFeeName.DataBindings.Add("Text", bsPOOtherFees, "FeeDesc");
            txtFeeAmount.DataBindings.Add("Text", bsPOOtherFees, "Amount");
            txtPONoOtherCharges.DataBindings.Add("Text", bsPOOtherFees, "PONo");
            txtOtherFeesID.DataBindings.Add("Text", bsPOOtherFees, "OtherFeesID");

            Binding DateOtherFeesCreatedBinding;
            DateOtherFeesCreatedBinding = new Binding("Text", bsPOOtherFees, "DateCreated");
            DateOtherFeesCreatedBinding.Format += new ConvertEventHandler(DateBinding_Format);
            txtOtherFeesDateEntered.DataBindings.Add(DateOtherFeesCreatedBinding);
        }

        private void ClearPOMaster()
        {
            foreach (Control c in pnlRecord.Controls)
            {
                c.DataBindings.Clear();
            }
        }

        private void ClearPODetail()
        {
            foreach (Control c in pnlPODetail.Controls)
            {
                c.DataBindings.Clear();
            }
        }

        private void ClearPOOtherFees()
        {
            foreach (Control c in pnlOtherFees.Controls)
            {
                c.DataBindings.Clear();
            }
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
            dgvFile.Columns["CompanyCode"].HeaderText = "CMPY Code";
            dgvFile.Columns["PONo"].HeaderText = "PO No";
            dgvFile.Columns["PODate"].HeaderText = "PO Date";
            dgvFile.Columns["PRNo"].HeaderText = "PR No";
            dgvFile.Columns["PRDate"].HeaderText = "PR Date";
            dgvFile.Columns["DepartmentName"].HeaderText = "Department";
            dgvFile.Columns["CostCenterName"].HeaderText = "Cost Center";
            dgvFile.Columns["GLCode"].HeaderText = "GL Code";        
            dgvFile.Columns["Requestor"].HeaderText = "Requestor";                       
            dgvFile.Columns["VendorID"].HeaderText = "Vendor ID";
            dgvFile.Columns["VendorName"].HeaderText = "Vendor Name";            
            dgvFile.Columns["PayTerms"].HeaderText = "Payment Terms";
            dgvFile.Columns["LineItemTotal"].HeaderText = "Line Item Total";
            dgvFile.Columns["OtherCharges"].HeaderText = "Other Charges";  
            dgvFile.Columns["TotalPOAmount"].HeaderText = "Total PO Amount";           
            dgvFile.Columns["IsCancelled"].HeaderText = "Cancelled";
            dgvFile.Columns["DateCancelled"].HeaderText = "Date Cancelled";   
            dgvFile.Columns["SubmittedForReview"].HeaderText = "Submitted For Review";
            dgvFile.Columns["DateSubmitted"].HeaderText = "Date Submitted";
            dgvFile.Columns["Reviewer"].HeaderText = "Reviewer";
            dgvFile.Columns["Approver"].HeaderText = "Approver";
            dgvFile.Columns["ExecutiveApprover"].HeaderText = "Exec Approver";
            dgvFile.Columns["EmailedBy"].HeaderText = "Emailed By";
            dgvFile.Columns["DateEmailed"].HeaderText = "Date Emailed";
            dgvFile.Columns["PRNo"].Width = 77;           
            dgvFile.Columns["PONo"].Width = 77;
            dgvFile.Columns["PODate"].Width = 70;
            dgvFile.Columns["PRDate"].Width = 70;
            dgvFile.Columns["DepartmentName"].Width = 160;
            dgvFile.Columns["CostCenterName"].Width = 280;         
            dgvFile.Columns["Requestor"].Width = 130;            
            dgvFile.Columns["VendorName"].Width = 200;            
            dgvFile.Columns["ConfirmNo"].Width = 150;
            dgvFile.Columns["AcctNo"].Width = 150;
            dgvFile.Columns["TotalPOAmount"].Width = 80;
            dgvFile.Columns["IsCancelled"].Width = 70;
            dgvFile.Columns["DateCancelled"].Width = 70;
            dgvFile.Columns["DateSubmitted"].Width = 70;
            dgvFile.Columns["DateEmailed"].Width = 70;
            dgvFile.Columns["SubmittedForReview"].Width = 80;
            dgvFile.Columns["Reviewer"].Width = 80;
            dgvFile.Columns["Approver"].Width = 70;
            dgvFile.Columns["ExecutiveApprover"].Width = 70;
            dgvFile.Columns["DepartmentID"].Visible = false;
            dgvFile.Columns["CostCenterID"].Visible = false;
            dgvFile.Columns["GLCode"].Visible = false;
            dgvFile.Columns["RequestedBy"].Visible = false;
            dgvFile.Columns["VendorID"].Visible = false;
            dgvFile.Columns["PayTerms"].Visible = false;
            dgvFile.Columns["ConfirmNo"].Visible = false;
            dgvFile.Columns["AcctNo"].Visible = false;
            dgvFile.Columns["ContactName"].Visible = false;
            dgvFile.Columns["Email"].Visible = false;
            dgvFile.Columns["DateSubmitted"].Visible = false;
            dgvFile.Columns["OtherChargesReviewed"].Visible = false;
            dgvFile.Columns["DateOtherCharges"].Visible = false;
            dgvFile.Columns["ForPOApproval"].Visible = false;
            dgvFile.Columns["DateforPOApproval"].Visible = false;
            dgvFile.Columns["ForDLPApproval"].Visible = false;
            dgvFile.Columns["DateForDLPApproval"].Visible = false;
            dgvFile.Columns["ReviewedByID"].Visible = false;            
            dgvFile.Columns["ReviewDate"].Visible = false;
            dgvFile.Columns["ApprovedByID"].Visible = false;           
            dgvFile.Columns["ApprovalDate"].Visible = false;
            dgvFile.Columns["ExecutiveID"].Visible = false;           
            dgvFile.Columns["ExecApprovalDate"].Visible = false;
            dgvFile.Columns["EmailedByID"].Visible = false;
            dgvFile.Columns["EmailedBy"].Visible = false;   
            dgvFile.Columns["CreatedByID"].Visible = false;           
            dgvFile.Columns["LastUpdate"].Visible = false;
            dgvFile.Columns["LastUserID"].Visible = false;           
            dgvFile.Columns["PODate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["PRDate"].DefaultCellStyle.Format = "MM/dd/yyyy"; 
            dgvFile.Columns["DateCancelled"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateSubmitted"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateEmailed"].DefaultCellStyle.Format = "MM/dd/yyyy"; 
            dgvFile.Columns["GLCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["PayTerms"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["LineItemTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["LineItemTotal"].DefaultCellStyle.Format = "N2";
            dgvFile.Columns["OtherCharges"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["OtherCharges"].DefaultCellStyle.Format = "N2";
            dgvFile.Columns["TotalPOAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["TotalPOAmount"].DefaultCellStyle.Format = "N2";
        }

        private void DataGridPODetailsSetting()
        {
            dgvPODetails.EnableHeadersVisualStyles = false;
            dgvPODetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPODetails.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvPODetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPODetails.Columns["CatalogNo"].HeaderText = "Cat No";
            dgvPODetails.Columns["CatalogName"].HeaderText = "Catalog Name";                   
            dgvPODetails.Columns["Quantity"].HeaderText = "Qty";            
            dgvPODetails.Columns["UnitPrice"].HeaderText = "Unit Price";
            dgvPODetails.Columns["TotalPrice"].HeaderText = "Total Price";
            dgvPODetails.Columns["IsRejected"].HeaderText = "Rejected"; 
            dgvPODetails.Columns["CatalogNo"].Width = 100;
            dgvPODetails.Columns["CatalogName"].Width = 180;
            dgvPODetails.Columns["Quantity"].Width = 50;          
            dgvPODetails.Columns["UnitPrice"].Width = 80;
            dgvPODetails.Columns["TotalPrice"].Width = 60;
            dgvPODetails.Columns["IsRejected"].Width = 70;                          
            dgvPODetails.Columns["CatalogNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvPODetails.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPODetails.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPODetails.Columns["TotalPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPODetails.Columns["UnitPrice"].DefaultCellStyle.Format = "N2";
            dgvPODetails.Columns["TotalPrice"].DefaultCellStyle.Format = "N2";

            dgvPODetails.Columns["PODetailID"].Visible = false;
            dgvPODetails.Columns["PONo"].Visible = false;
            dgvPODetails.Columns["CatalogNameID"].Visible = false;
            dgvPODetails.Columns["CatalogDesc"].Visible = false;
            dgvPODetails.Columns["GradeID"].Visible = false;
            dgvPODetails.Columns["Grade"].Visible = false;
            dgvPODetails.Columns["VendorQuoteNo"].Visible = false;
            dgvPODetails.Columns["IsCOARequired"].Visible = false;
            dgvPODetails.Columns["IsMSDRequired"].Visible = false;           
            dgvPODetails.Columns["TotalPrice"].Visible = false;
            dgvPODetails.Columns["IsInStock"].Visible = false;
            dgvPODetails.Columns["IsBackOrdered"].Visible = false;
            dgvPODetails.Columns["DateBackOrder"].Visible = false;
            dgvPODetails.Columns["RejectReason"].Visible = false;
            dgvPODetails.Columns["DoesMeetSpecs"].Visible = false;
            dgvPODetails.Columns["InspectedByID"].Visible = false;
            dgvPODetails.Columns["Inspector"].Visible = false;
            dgvPODetails.Columns["DateInspected"].Visible = false; 
            dgvPODetails.Columns["CreatedByID"].Visible = false;
            dgvPODetails.Columns["DateCreated"].Visible = false;
            dgvPODetails.Columns["LastUpdate"].Visible = false;
            dgvPODetails.Columns["LastUserID"].Visible = false;
        }

        private void DataGridOtherFeesSetting()
        {
            dgvPOOtherFees.EnableHeadersVisualStyles = false;
            dgvPOOtherFees.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPOOtherFees.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvPOOtherFees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;           
            dgvPOOtherFees.Columns["FeeCode"].HeaderText = "Fee Code";
            dgvPOOtherFees.Columns["FeeDesc"].HeaderText = "Description";  
            dgvPOOtherFees.Columns["Amount"].HeaderText = "Amount";
            dgvPOOtherFees.Columns["FeeDesc"].Width = 241;
            dgvPOOtherFees.Columns["Amount"].Width = 80;
            dgvPOOtherFees.Columns["PONo"].Visible = false;
            dgvPOOtherFees.Columns["OtherFeesID"].Visible = false;
            dgvPOOtherFees.Columns["FeeCode"].Visible = false;
            dgvPOOtherFees.Columns["CreatedByID"].Visible = false;
            dgvPOOtherFees.Columns["DateCreated"].Visible = false;
            dgvPOOtherFees.Columns["LastUpdate"].Visible = false;
            dgvPOOtherFees.Columns["LastUserID"].Visible = false;
            dgvPOOtherFees.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPOOtherFees.Columns["Amount"].DefaultCellStyle.Format = "N2";
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
            pnlPODetailGrid.Visible = true; pnlPODetail.Visible = true;
            ClearControls(this.pnlRecord);
            ClearControls(this.pnlPODetail);
            ClearControls(this.pnlPODetailGrid);
            ClearControls(this.pnlOtherFees);
            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlPODetail, false);
            OpenControls(this.pnlPODetailGrid, false);
            OpenControls(this.pnlOtherFees, false);
            dtMaster.Rows.Clear();
            dtDetail.Rows.Clear();
            dtOtherFees.Rows.Clear();
            
            txtPONo.Focus();

            btnAddDetail.Enabled = true;
            btnDeleteDetail.Enabled = false;
            btnAddCharges.Enabled = true;

            // Create PRMaster Data Row            
            DataRow dR = dtMaster.NewRow();          

            dR["PONo"] = "< New >";
            dR["PODate"] = DateTime.Now; 
            dR["PRNo"] = DBNull.Value;
            dR["PRDate"] = DBNull.Value;
            dR["DepartmentID"] = DBNull.Value;
            dR["DepartmentName"] = DBNull.Value;
            dR["CostCenterID"] = DBNull.Value;
            dR["CostCenterName"] = DBNull.Value;
            dR["GLCode"] = DBNull.Value;
            dR["RequestedBy"] = DBNull.Value;
            dR["Requestor"] = DBNull.Value;           
            dR["VendorID"] = DBNull.Value;
            dR["VendorName"] = DBNull.Value;
            dR["ConfirmNo"] = DBNull.Value;
            dR["AcctNo"] = DBNull.Value;
            dR["PayTerms"] = DBNull.Value;
            dR["ContactName"] = DBNull.Value;
            dR["Email"] = DBNull.Value;
            dR["LineItemTotal"] = DBNull.Value;
            dR["OtherCharges"] = DBNull.Value;
            dR["TotalPOAmount"] = DBNull.Value;           
            dR["IsCancelled"] = false;
            dR["DateCancelled"] = DBNull.Value;
            dR["SubmittedForReview"] = false;
            dR["DateSubmitted"] = DBNull.Value;
            dR["OtherChargesReviewed"] = false;
            dR["DateOtherCharges"] = DBNull.Value;
            dR["ForPOApproval"] = false;
            dR["DateforPOApproval"] = DBNull.Value;
            dR["ForDLPApproval"] = false;
            dR["DateForDLPApproval"] = DBNull.Value;
            dR["ReviewedByID"] = DBNull.Value;
            dR["ReviewDate"] = DBNull.Value;
            dR["ApprovedByID"] = DBNull.Value;
            dR["ApprovalDate"] = DBNull.Value;
            dR["ExecutiveID"] = DBNull.Value;
            dR["ExecApprovalDate"] = DBNull.Value;
            dR["CreatedByID"] = LogIn.nUserID;                
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;
            dtMaster.Rows.Add(dR);
            bsPOMaster.DataSource = dtMaster;

            BindPOMaster();

            chkDoesMeetSpecs.Enabled = false;
            txtInspectedByID.Enabled = false;
            txtInspector.Enabled = false;
            picInspectors.Enabled = false;
            mskDateInspected.Enabled = false;
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

            // If PO has been approved/ lock it if all items are inspected
            if (txtApprover.Text.Trim() != "")
            {
                //if (Convert.ToDecimal(txtPOAmount.Text) < 2500)
                //{
                // Check if all items have been inspected
                int nInspected = 0;


                for (int j = 0; j < dgvPODetails.Rows.Count; j++)
                {
                    if (dgvPODetails.Rows[j].Cells["InspectedByID"].Value.ToString() != "")
                    {
                        nInspected++;
                    }
                }

                if (nInspected == dgvPODetails.Rows.Count)
                {
                    MessageBox.Show("This PO is now locked. No edits allowed!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    tsbEdit.Enabled = true;
                    tsbSave.Enabled = false;
                    tsbCancel.Enabled = false;
                    return;
                }
                //}
            }

            LoadData();
            nMode = 2;
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront();
           
            OpenControls(this.pnlRecord, true);
            OpenControls(this.pnlOtherFees, true);
            OpenControls(this.pnlPODetail, true);
            btnClose.Visible = false;
           
            btnAddCharges.Enabled = true;

            tsbEdit.Enabled = false;
            tsbSave.Enabled = true;
            tsbCancel.Enabled = true;

            // Master
            txtPRNo.Enabled = false;
            txtPONo.Enabled = false;
            txtDeptID.Enabled = false;
            txtDeptName.Enabled = false; 
            txtCostCenterID.Enabled = false;
            txtCostCenterName.Enabled = false;
            txtGLCode.Enabled = false;
            txtRequestedBy.Enabled = false;
            txtRequestor.Enabled = false;
            txtVendorID.Enabled = false;
            txtVendorName.Enabled = false;
           
            txtLineItemTotal.Enabled = false;
            txtOtherCharges.Enabled = false;
            txtPOAmount.Enabled = false;           
            chkSubmitForReview.Enabled = false;
            chkOtherCharges.Enabled = false;
            chkPOApproval.Enabled = false;
            chkDLPApproval.Enabled = false;            
            txtPRDate.Enabled = false;            
            txtPODate.Enabled = false;
            picRequestors.Enabled = false;
            picCostCenters.Enabled = false;
            picVendors.Enabled = false;

            //Detail
            txtCatNo.Enabled = false;
            txtCatName.Enabled = false;
            txtGrade.Enabled = false;
            txtCatDesc.Enabled = false;         
            picCatNames.Enabled = false;
            picCatNos.Enabled = false;
            txtTotPrice.Enabled = false;
            btnCheckBrowser.Enabled = true;
            mskDateBackOrder.Enabled = true;

            if (strFileAccess != "FA")
            {
                btnSubmit.Enabled = false;
                btnAddFee.Enabled = false;
                btnDeleteFee.Enabled = false;
                btnCheckBrowser.Enabled = false;

                txtConfirmNo.Enabled = false;
                txtAcctNo.Enabled = false;
                txtWorkPhone.Enabled = false;
                txtCell.Enabled = false;
                txtFax.Enabled = false;
                txtContact.Enabled = false;
                txtEmail.Enabled = false;
                txtWebsite.Enabled = false;
                
                cboPayTerms.Enabled = false;
                txtVendorQuoteNo.Enabled = false;
                txtQuantity.Enabled = false;
                txtUnitPrice.Enabled = false;

                txtFeeCode.Enabled = false;
                txtFeeName.Enabled = false;
                txtFeeAmount.Enabled = false;
                picFees.Enabled = false;

                mskDateBackOrder.Enabled = false;
                chkBackOrdered.Enabled = false;
                chkMSD.Enabled = false;
                chkCOA.Enabled = false;
                chkInStock.Enabled = false;
            }

            if (txtReviewer.Text.Trim() != "" && txtApprover.Text.Trim() == "")
            {
                chkRejectItem.Enabled = true;
                txtRejectReason.Enabled = true;
            }
            else
            {
                chkRejectItem.Enabled = false;
                txtRejectReason.Enabled = false;
            }  

            CheckInspector();
            btnAddDetail.Enabled = true; btnDeleteDetail.Enabled = true; OpenControls(pnlPODetail, true); pnlPODetail.Enabled = true; pnlRecord.Enabled = true; //Validation
        }

        private void DeleteRecord()
        {
        }

        private void DeleteDetail(int cPODetailID)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("PODetailID", cPODetailID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelPODetail";

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

            sqlcmd.Parameters.AddWithValue("@POOtherFeesID", cPROtherFeesID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelPOOtherFees";

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
            // Master Save routine
            bsPOMaster.EndEdit();
           
            // Validate if changes were made on the Master
            DataTable dtHeader = dtMaster.GetChanges();
            if (dtHeader != null)
            { 
                int nPO = ValidateMaster();                                                      // Validation for PR Master Record
                if (nPO == 0)
                {
                    dtHeader.Dispose();
                    return;
                }
               
                SavePOMaster();                                                                 // Save PR Master Record
                dtHeader.Dispose();   
            }                      
           
            // Detail Save Routine
            bsPODetails.EndEdit();
         
            // Validate if changes were made on the Detail
            DataTable dtDetails = dtDetail.GetChanges();
            if (dtDetails != null)
            {
                int nPO = ValidateDetails();                                                   // Validation for PR Detail Record
                if (nPO == 0)
                {
                    dtDetails.Dispose();
                    return;
                }
                UpdatePODetails();                                                             // Save PR Detail Record
                dtDetails.Dispose();
                if (txtReviewer.Text.Trim() == "")
                {
                    UpdateCatalogMaster();
                }
            }

            // Other Fees Save Routine
            bsPOOtherFees.EndEdit();

            // Validate if changes were made on the Detail
            DataTable dtCharges = dtOtherFees.GetChanges();
            if (dtCharges != null)
            {
                int nPO = ValidateOtherFees();                                                 // Validation for PR Other Fees Record
                if (nPO == 0)
                {
                    dtDetails.Dispose();
                    return;
                }
                UpdatePOOtherFees();                                                          // Save PR Detail Record
                dtCharges.Dispose();
            }            

            // Update POMaster Totals
            UpdatePOMasterTotals();

            // Update Vendor Info
            UpdatePOVendorInfo();

            // Reload
            dgvFile.Refresh();
            btnClose.Visible = true;
            btnAddDetail.Enabled = false;
            OpenControls(pnlRecord, false);
            LoadRecords();
            PSSClass.General.FindRecord("PONo", txtPONo.Text, bsFile, dgvFile);          
            LoadData();
            tsbSave.Enabled = false;
            tsbCancel.Enabled = false;
            MessageBox.Show("Record successfully saved!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void SavePOMaster()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@PONo", txtPONo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@DeptID", txtDeptID.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CostCenterID", txtCostCenterID.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@RequestedBy", Convert.ToInt16(txtRequestedBy.Text));           
            sqlcmd.Parameters.AddWithValue("@VendorID", Convert.ToInt16(txtVendorID.Text));
            sqlcmd.Parameters.AddWithValue("@ConfirmNo", txtConfirmNo.Text.Trim());          
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
            sqlcmd.CommandText = "spEditPORecord";
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

        private static int SavePODetails(int cPODetailID, string cCmpyCode, string cPONo, int cRow, byte cMode, DataTable cDT)
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
            sqlcmd.Parameters.AddWithValue("@PODetailID", cPODetailID);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpyCode);
            sqlcmd.Parameters.AddWithValue("@PONo", cPONo);
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
            if (cDT.Rows[cRow]["DateBackOrder"].ToString() == "")
            {
                sqlcmd.Parameters.AddWithValue("@DateBackOrder", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@DateBackOrder", Convert.ToDateTime(cDT.Rows[cRow]["DateBackOrder"].ToString()));
            }
            sqlcmd.Parameters.AddWithValue("@IsRejected", Convert.ToBoolean(cDT.Rows[cRow]["IsRejected"].ToString()));
            sqlcmd.Parameters.AddWithValue("@RejectReason", cDT.Rows[cRow]["RejectReason"].ToString());
            sqlcmd.Parameters.AddWithValue("@DoesMeetSpecs", Convert.ToBoolean(cDT.Rows[cRow]["DoesMeetSpecs"].ToString()));
            if (cDT.Rows[cRow]["InspectedByID"].ToString() == "")
            {
                sqlcmd.Parameters.AddWithValue("@InspectedByID", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@DateInspected", DBNull.Value);
            }
            else
            {
                sqlcmd.Parameters.AddWithValue("@InspectedByID", Convert.ToInt16(cDT.Rows[cRow]["InspectedByID"].ToString()));
                sqlcmd.Parameters.AddWithValue("@DateInspected", Convert.ToDateTime(cDT.Rows[cRow]["DateInspected"].ToString()));              
            }            
           
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditPODetail";
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

        private static int SavePOOtherFees(int cPOOtherFeesID, string cCmpyCode, string cPONo, int cRow, byte cMode, DataTable cDT)
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
            sqlcmd.Parameters.AddWithValue("@PONo", cPONo);
            sqlcmd.Parameters.AddWithValue("@OtherFeesID", cPOOtherFeesID);           
            sqlcmd.Parameters.AddWithValue("@FeeCode", cDT.Rows[cRow]["FeeCode"].ToString());            
            sqlcmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(cDT.Rows[cRow]["Amount"].ToString()));  
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditPOOtherFees";
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

        private void UpdatePOOtherFees()
        {
            bsPOOtherFees.EndEdit();
          
            DataTable dt = dtOtherFees.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtOtherFeesID.Text = PSSClass.DataEntry.NewID("POOtherFees", "OtherFeesID").ToString();

                    SavePOOtherFees(Convert.ToInt16(txtOtherFeesID.Text), txtCmpyCode.Text, txtPONo.Text, i, 1, dt);                    
                }
                dt.Rows.Clear();
            }
            dt = dtOtherFees.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SavePOOtherFees(Convert.ToInt16(dt.Rows[i]["OtherFeesID"].ToString()), txtCmpyCode.Text, txtPONo.Text, i, 2, dt);
                }
                dt.Rows.Clear();
            }
        }

        private void UpdatePODetails()
        {
            bsPODetails.EndEdit();
            DataTable dt = dtDetail.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtPODetailID.Text = PSSClass.DataEntry.NewID("PODetails", "PODetailID").ToString();
                    SavePODetails(Convert.ToInt16(txtPODetailID.Text), txtCmpyCode.Text, txtPONo.Text, i, 1, dt);
                }
                dt.Rows.Clear();
            }

            dt = dtDetail.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SavePODetails(Convert.ToInt16(dt.Rows[i]["PODetailID"].ToString()), txtCmpyCode.Text, txtPONo.Text, i, 2, dt);
                }
                dt.Rows.Clear();
            }
        }

        private void UpdatePOMasterTotals()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection(); ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@PONo", txtPONo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdatePOMasterTotals";
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

        private void UpdateCatalogMaster()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection(); ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@VendorID", txtVendorID.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CatalogNo", txtCatNo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@CatalogNameID", txtCatNameID.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@UnitPrice", txtUnitPrice.Text.Trim());           
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdatePOItemInfo";

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

        private void UpdatePOVendorInfo()
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
            sqlcmd.CommandText = "spUpdatePOVendorInfo";
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

        private void UpdatePOReviewer()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection(); ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim());           
            sqlcmd.Parameters.AddWithValue("@PONo", txtPONo.Text.Trim());           
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdatePOReviewer";

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
            //if (txtConfirmNo.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please enter Confirmation Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    txtConfirmNo.Focus();
            //    return 0;
            //}
            //if (txtAcctNo.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please enter Account Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    txtAcctNo.Focus();
            //    return 0;
            //}
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
            if (nMode != 0)
            {
                if (txtQuantity.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter Quantity!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtQuantity.Focus();
                    return 0;
                }
                if (txtUnitPrice.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter a Unit Price!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtUnitPrice.Focus();
                    return 0;
                }
                if (txtInspectedByID.Text.Trim() != "" && mskDateInspected.MaskFull == false)
                {
                    MessageBox.Show("Please enter an Inspection Date!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    mskDateInspected.Focus();
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
            AddEditMode(false);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true; dgvInspectors.Visible = false;
            nMode = 0;
        }

        private void PurchaseOrder_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "PurchaseOrders");
            //LogIn.nUserID = 379; // ALerro
            LoadRecords();
            LoadRequestors();
            LoadPayTerms();
            LoadCostCenters();
            LoadVendors();            
            LoadInspectors();           
            LoadOtherFees();

            BuildPrintItems();

            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();                      

            CreateMasterStructure();
            CreateDetailStructure();
            CreateOtherFeesStructure();
            
            if (nPOSw == 1 || nPRModuleSw == 1)
            {
                PSSClass.General.FindRecord("PONo", strPONo, bsFile, dgvFile);
                LoadData();
            }                                 
        }       

        private void CreateMasterStructure()
        {
            // Create PR Master Data table for Add/Edit/Delete functions  
            bsPOMaster.DataSource = dtMaster;
            dtMaster.Columns.Add("CompanyCode", typeof(string));
            dtMaster.Columns.Add("PONo", typeof(string));
            dtMaster.Columns.Add("PODate", typeof(DateTime));
            dtMaster.Columns.Add("PRNo", typeof(string));
            dtMaster.Columns.Add("PRDate", typeof(DateTime));
            dtMaster.Columns.Add("DepartmentID", typeof(Int16));
            dtMaster.Columns.Add("DepartmentName", typeof(string));
            dtMaster.Columns.Add("CostCenterID", typeof(Int16));
            dtMaster.Columns.Add("CostCenterName", typeof(string));
            dtMaster.Columns.Add("GLCode", typeof(string)); 
            dtMaster.Columns.Add("RequestedBy", typeof(Int16));
            dtMaster.Columns.Add("Requestor", typeof(string));
            dtMaster.Columns.Add("VendorID", typeof(Int16));
            dtMaster.Columns.Add("VendorName", typeof(string));
            dtMaster.Columns.Add("ConfirmNo", typeof(string));
            dtMaster.Columns.Add("AcctNo", typeof(string));
            dtMaster.Columns.Add("PayTerms", typeof(string));
            dtMaster.Columns.Add("ContactName", typeof(string));
            dtMaster.Columns.Add("Email", typeof(string));
            dtMaster.Columns.Add("LineItemTotal", typeof(decimal));
            dtMaster.Columns.Add("OtherCharges", typeof(decimal));
            dtMaster.Columns.Add("TotalPOAmount", typeof(decimal));
            dtMaster.Columns.Add("IsCancelled", typeof(bool));
            dtMaster.Columns.Add("DateCancelled", typeof(DateTime));
            dtMaster.Columns.Add("SubmittedForReview", typeof(bool));
            dtMaster.Columns.Add("DateSubmitted", typeof(DateTime));
            dtMaster.Columns.Add("OtherChargesReviewed", typeof(bool));
            dtMaster.Columns.Add("DateOtherCharges", typeof(DateTime));
            dtMaster.Columns.Add("ForPOApproval", typeof(bool));
            dtMaster.Columns.Add("DateforPOApproval", typeof(DateTime));
            dtMaster.Columns.Add("ForDLPApproval", typeof(bool));
            dtMaster.Columns.Add("DateForDLPApproval", typeof(DateTime));
            dtMaster.Columns.Add("ReviewedByID", typeof(Int16));
            dtMaster.Columns.Add("Reviewer", typeof(string));
            dtMaster.Columns.Add("ReviewDate", typeof(DateTime));
            dtMaster.Columns.Add("ApprovedByID", typeof(Int16));
            dtMaster.Columns.Add("Approver", typeof(string));
            dtMaster.Columns.Add("ApprovalDate", typeof(DateTime));
            dtMaster.Columns.Add("ExecutiveID", typeof(Int16));
            dtMaster.Columns.Add("ExecutiveApprover", typeof(string));
            dtMaster.Columns.Add("ExecApprovalDate", typeof(DateTime));
            dtMaster.Columns.Add("EmailedByID", typeof(Int16));
            dtMaster.Columns.Add("EmailedBy", typeof(string));
            dtMaster.Columns.Add("DateEmailed", typeof(DateTime));
            dtMaster.Columns.Add("CreatedByID", typeof(Int16));         
            dtMaster.Columns.Add("LastUpdate", typeof(DateTime));
            dtMaster.Columns.Add("LastUserID", typeof(Int16));           
        }

        private void CreateDetailStructure()
        {
            // Create PR Detail Data table for Add/Edit/Delete functions
            bsPODetails.DataSource = dtDetail;
            dgvPODetails.DataSource = bsPODetails;
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
            dtDetail.Columns.Add("DateBackOrder", typeof(DateTime));
            dtDetail.Columns.Add("IsRejected", typeof(bool));
            dtDetail.Columns.Add("RejectReason", typeof(string));
            dtDetail.Columns.Add("DoesMeetSpecs", typeof(bool));
            dtDetail.Columns.Add("InspectedByID", typeof(Int16));
            dtDetail.Columns.Add("Inspector", typeof(string));
            dtDetail.Columns.Add("DateInspected", typeof(DateTime));
            dtDetail.Columns.Add("CreatedByID", typeof(Int16));
            dtDetail.Columns.Add("DateCreated", typeof(DateTime));
            dtDetail.Columns.Add("LastUpdate", typeof(DateTime));
            dtDetail.Columns.Add("LastUserID", typeof(Int16));
            dtDetail.Columns.Add("PONo", typeof(string));
            dtDetail.Columns.Add("PODetailID", typeof(Int16));
            DataGridPODetailsSetting();
        }

        private void CreateOtherFeesStructure()
        {
            // Create PR Other Charges Data table for Add/Edit/Delete functions            
            bsPOOtherFees.DataSource = dtOtherFees;
            dgvPOOtherFees.DataSource = bsPOOtherFees;
            dtOtherFees.Columns.Add("FeeCode", typeof(Int16));
            dtOtherFees.Columns.Add("FeeDesc", typeof(string));
            dtOtherFees.Columns.Add("Amount", typeof(decimal));
            dtOtherFees.Columns.Add("CreatedByID", typeof(Int16));
            dtOtherFees.Columns.Add("DateCreated", typeof(DateTime));
            dtOtherFees.Columns.Add("LastUpdate", typeof(DateTime));
            dtOtherFees.Columns.Add("LastUserID", typeof(Int16));
            dtOtherFees.Columns.Add("PONo", typeof(string));
            dtOtherFees.Columns.Add("OtherFeesID", typeof(Int16));
            DataGridOtherFeesSetting();
        }

        public void SendEMail(int cMode)
        {
            try
            {
                string strReviewerEmail = "";
                string strApproverEmail = "";
                string strExecutiveEmail = "";
                string strRevFirstName = "";
                string strApproverFirstName = "";
                string strExecFirstName = "";   

                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
               
                sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@PONo", txtPONo.Text.Trim());

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spGetPOEmailAddresses";

                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strReviewerEmail = sqldr.GetValue(0).ToString();
                    strApproverEmail = sqldr.GetValue(1).ToString();
                    strExecutiveEmail = sqldr.GetValue(2).ToString();
                    strRevFirstName = sqldr.GetValue(3).ToString();
                    strApproverFirstName = sqldr.GetValue(4).ToString();
                    strExecFirstName = sqldr.GetValue(5).ToString(); 
                }
                else
                {
                    MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                sqldr.Close(); sqlcmd.Dispose();

                //For Testing
                //string strEMail = "myounes@gibraltarlabsinc.com; adelacruz@gibraltarlabsinc.com;"; 

                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                oMsg.HTMLBody = "<FONT face=\"Arial\">";

                String Body1 = null;

                // Set Email body.   
                if (cMode == 1)
                {
                    Body1 = "Dear " + strRevFirstName + "," + Environment.NewLine + Environment.NewLine +
                                   txtPONo.Text.Trim() + " for $" + txtPOAmount.Text.Trim() + " has been approved. " + Environment.NewLine;
                }
                else if (cMode == 2)
                {
                    Body1 = "Dear " + strApproverFirstName + "," + Environment.NewLine + Environment.NewLine +
                                  txtPONo.Text.Trim() + " for $" + txtPOAmount.Text.Trim() + " has been submitted for your approval. Please check. " + Environment.NewLine;
                }
                else if (cMode == 3)
                {
                    Body1 = "Dear " + strExecFirstName + "," + Environment.NewLine + Environment.NewLine +
                                  txtPONo.Text.Trim() + " for $" + txtPOAmount.Text.Trim() + " has been submitted for your approval. Please check. " + Environment.NewLine;
                }
                txtBody.Text = Body1; 

                string strBody = txtBody.Text.Replace("\r\n", "<br />");
                string strSignature = ReadSignature();
                strBody = strBody + "<br /><br />" + strSignature;

                oMsg.HTMLBody += strBody.Trim();

                //Subject line
                if (cMode == 1)
                {
                    oMsg.Subject = "Purchase Order Approval Email - Ref No: ( " + txtPONo.Text.Trim() + " )";
                }
                else
                {
                    oMsg.Subject = "Purchase Order Submission for Approval Email - Ref No: ( " + txtPONo.Text.Trim() + " )";
                }

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
                else if (cMode == 3)
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(strExecutiveEmail);
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
                UpdatePOEmailDate(txtCmpyCode.Text, txtPONo.Text.Trim());

                MessageBox.Show(txtPONo.Text.Trim() + " has been approved and a corresponding email was sent to the requestor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void UpdatePOEmailDate(string cCmpyCode, string cPONo)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpyCode);
            sqlcmd.Parameters.AddWithValue("@PONo", cPONo);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdatePOEmailDate";

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

        private void PrintReport()
        {
            PurchaseOrdersRpt rpt = new PurchaseOrdersRpt();
            txtCmpyCode.Text = dgvFile.CurrentRow.Cells["CompanyCode"].Value.ToString();
            txtPONo.Text = dgvFile.CurrentRow.Cells["PONo"].Value.ToString();
            rpt.CmpyCode = txtCmpyCode.Text.Trim();
            rpt.PONo = txtPONo.Text.Trim();

            if (chkShipTo.Checked)
                rpt.DlvrTo = 1;
            else
                rpt.DlvrTo = 2;
            if (strFileAccess == "RO")
                rpt.pubPrtMode = 1;
            else
                rpt.pubPrtMode = 2;
            try
            {
                rpt.WindowState = FormWindowState.Maximized;
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in creating the PO..." + ex.Message, Application.ProductName);
                return;
            }
        }

        private void PurchaseOrder_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                //case Keys.F2:
                //    if (nMode == 0 && strFileAccess != "RO")
                //    {
                //        AddEditMode(true); AddRecord();
                //    }
                //    break;

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
            pnlRecord.Visible = false;

            if (nPOSw == 1 || nPRModuleSw == 1)
            {
                nPOSw = 0;
                nPRModuleSw = 0;
                this.Close(); this.Dispose();
            }
            else
            {
                pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; 
                LoadRecords();
                dgvFile.Focus();
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

        // MY 07/10/2015 - START: txt/dgvInspectors events
        private void dgvInspectors_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtInspector.Text = dgvInspectors.CurrentRow.Cells["InspectedByName"].Value.ToString();
                txtInspectedByID.Text = dgvInspectors.CurrentRow.Cells["InspectedBy"].Value.ToString();
                dgvInspectors.Visible = false;
            }
        }

        private void dgvInspectors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvInspectors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtInspector.Text = dgvInspectors.CurrentRow.Cells["InspectedByName"].Value.ToString();
                    txtInspectedByID.Text = dgvInspectors.CurrentRow.Cells["InspectedBy"].Value.ToString();
                    dgvInspectors.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvInspectors.Visible = false;
                }
            }
        }
        private void txtInspector_Enter(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvInspectors.Visible = true; dgvInspectors.BringToFront();
            }
        }

        private void dgvInspectors_Leave(object sender, EventArgs e)
        {
            dgvInspectors.Visible = false;
        }

        private void txtInspector_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwInspectors;
                dvwInspectors = new DataView(dtInspectors, "InspectedByName like '%" + txtInspector.Text.Trim().Replace("'", "''") + "%'", "InspectedByName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvInspectors, dvwInspectors);
            }
        }

        private void dgvInspectors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode != 0)
            {
                txtInspector.Text = dgvInspectors.CurrentRow.Cells["InspectedByName"].Value.ToString();
                txtInspectedByID.Text = dgvInspectors.CurrentRow.Cells["InspectedBy"].Value.ToString();
                dgvInspectors.Visible = false;
            }
        }

        private void picInspectors_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                LoadInspectors();
                dgvInspectors.Visible = true; dgvInspectors.BringToFront();
            }
        }

        private void txtInspectedByID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtInspector.Text = PSSClass.Procurements.PODInspector(txtInspectedByID.Text.Trim());
                    if (txtInspector.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Inspector ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvInspectors.Visible = false;
                }
                else
                {
                    txtInspector.Text = ""; dgvInspectors.Visible = false;
                }
            }
        }

        // MY 07/10/2015 - END: txt/dgvInspectors events        

        // MY 07/27/2015 - START: txt/dgvCostCenter events
        private void dgvCostCenters_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtCostCenterName.Text = dgvCostCenters.CurrentRow.Cells["CostCenterName"].Value.ToString();
                txtCostCenterID.Text = dgvCostCenters.CurrentRow.Cells["CostCenterID"].Value.ToString();
                dgvCostCenters.Visible = false;
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
                    txtCostCenterName.Text = PSSClass.Procurements.POMCostCenterName(Convert.ToInt16(txtCostCenterID.Text.Trim()));
                    if (txtCostCenterName.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Cost Center ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvCostCenters.Visible = false;
                    // Get GL Code
                    try
                    {
                        txtGLCode.Text = PSSClass.Procurements.POGLCode(Convert.ToInt16(txtCostCenterID.Text));
                    }
                    catch { }
                }
                else
                {
                    txtCostCenterName.Text = ""; dgvCostCenters.Visible = false;
                }
            }
        }

        // MY 07/27/2015 - END: txt/dgvCostCenter events       

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
           // BindPRDetails();
        }

        private void btnAddDetail_Click(object sender, EventArgs e)
        {
            nMode = 1;      
            ClearControls(this.pnlPODetail);
            OpenControls(this.pnlPODetail, true);
            txtQuantity.Focus();
            btnAddDetail.Enabled = false;
            btnDeleteDetail.Enabled = false;
            btnOKDetail.Enabled = true;

            AddEditMode(true);
            tsbCancel.Enabled = true;
            txtDateCreated.Text = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");     
            foreach (Control c in pnlPODetail.Controls)
            {
                c.DataBindings.Clear();
            }
            txtCatName.Enabled = true; picCatNames.Enabled = true;
        }        

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {  
            int dRow = dgvPODetails.CurrentRow.Index;
            int intPODetailID;

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
         
            intPODetailID = Convert.ToInt16(dgvPODetails.CurrentRow.Cells["PODetailID"].Value.ToString());

            dgvPODetails.Rows.RemoveAt(dRow);

            if (dgvPODetails.Rows.Count == 0)
            {
                btnDeleteDetail.Enabled = false;
            }

            DeleteDetail(intPODetailID);

            AddEditMode(false);
        }     

        private void btnApproverESign_Click(object sender, EventArgs e)
        {

            using (ESignature eSignature = new ESignature())
            {
                eSignature.Location = new Point(405, 340);
                eSignature.eSign = 5;
                eSignature.ePONo = txtPONo.Text.Trim();
                if (eSignature.ShowDialog() == DialogResult.OK)
                {
                    if (Convert.ToDecimal(txtPOAmount.Text) < 2500)
                    {
                        SendEMail(1);
                    }
                    LoadData();
                    AddEditMode(false);
                    nMode = 0;
                }
            }
        }

        private void btnPresApproverESign_Click(object sender, EventArgs e)
        {
            using (ESignature eSignature = new ESignature())
            {
                eSignature.Location = new Point(405, 340);
                eSignature.eSign = 6;
                eSignature.ePONo = txtPONo.Text.Trim();
                if (eSignature.ShowDialog() == DialogResult.OK)
                {
                    SendEMail(1);
                    LoadData();
                    AddEditMode(false);
                    nMode = 0;
                }
            }
        }

        private void btnEMail_Click(object sender, EventArgs e)
        {
            if (txtPRNo.Text == "")
            {
                MessageBox.Show("Purchase Requisition has not been created!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
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

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (nMode == 2)
            {
                GetItemTotalPrice();
            }
        }

        private void txtUnitPrice_TextChanged(object sender, EventArgs e)
        {
            if (nMode == 2)
            {
                GetItemTotalPrice();
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

            DataRow dR = dtDetail.NewRow();           
            dR["CatalogNo"] = txtCatNo.Text;
            dR["CatalogNameID"] = txtCatNameID.Text;
            dR["CatalogName"] = txtCatName.Text;
            dR["GradeID"] = txtGradeID.Text;
            dR["Grade"] = txtGrade.Text;
            dR["IsCOARequired"] = chkCOA.CheckState;
            dR["IsMSDRequired"] = chkMSD.CheckState;
            dR["Quantity"] = Convert.ToInt16(txtQuantity.Text);
            dR["QtyDesc"] = txtCatDesc.Text;
            dR["UnitPrice"] = Convert.ToDecimal(txtUnitPrice.Text);
            dR["TotalPrice"] = Convert.ToDecimal(txtTotPrice.Text);
            dR["IsInStock"] = chkInStock.CheckState;
            dR["IsBackOrdered"] = chkBackOrdered.CheckState;
            dR["IsRejected"] = false;
            dR["RejectReason"] = txtRejectReason.Text;
            dR["DoesMeetSpecs"] = chkDoesMeetSpecs.CheckState;
            dR["InspectedByID"] = txtInspectedByID.Text;
            dR["DateInspected"] = mskDateInspected.Text;
            dR["CreatedByID"] = LogIn.nUserID;
            dR["DateCreated"] = DateTime.Now;
            dR["LastUpdate"] = DateTime.Now;
            dR["LastUserID"] = LogIn.nUserID;
            dR["PONo"] = txtPONo.Text;
            dR["PODetailID"] = 1;
            dtDetail.Rows.Add(dR);
            bsPODetails.DataSource = dtDetail;
            dgvPODetails.DataSource = bsPODetails;

            BindPODetails();
            btnAddDetail.Enabled = true;
            btnDeleteDetail.Enabled = true;
            btnOKDetail.Enabled = false;
            tsbSave.Enabled = true;

            DataGridPODetailsSetting();
        }
      
        private void btnAddCharges_Click(object sender, EventArgs e)
        {
            btnAddCharges.Enabled = false;       
            pnlOtherFees.Visible = true; pnlOtherFees.BringToFront();
            OpenControls(this.pnlOtherFees, false);

            picFees.Enabled = false;
            btnAddFee.Enabled = false;
            btnDeleteFee.Enabled = false;

            if (strFileAccess == "FA")
            {
                if (txtApprovedByID.Text.Trim() == "")
                {
                    OpenControls(this.pnlOtherFees, true);

                    btnAddFee.Enabled = true;

                    if (dgvFees.RowCount > 0)
                    {
                        btnDeleteFee.Enabled = true;
                    }

                }
            }
            GetPOSubTotalOtherFees();
        }

        private void btnCloseFees_Click(object sender, EventArgs e)
        {           
            pnlOtherFees.Visible = false;
            btnAddCharges.Enabled = true;           
            btnClose.Enabled = true;
            GetPOSubTotalOtherFees();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtPONo.Text == "")
            {
                return;
            }

            if (dgvPODetails.RowCount == 0)
            {
                MessageBox.Show("There are no items yet for this PO!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //if (txtConfirmNo.Text.Trim() == "")
            //{
            //    MessageBox.Show("Pls. enter Confirmation number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    txtConfirmNo.Focus();
            //    return;
            //}

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("This PO will be submitted now for management approval. Are you sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }          

            UpdatePOReviewer();
            LoadData();
            SendEMail(2);
            btnSubmit.Enabled = false;
            btnApproverESign.Enabled = true;
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
            int dRow = dgvPOOtherFees.CurrentRow.Index;
            int intOtherFeesID;

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }           

            intOtherFeesID = Convert.ToInt16(dgvPOOtherFees.CurrentRow.Cells["OtherFeesID"].Value.ToString());

            dgvPOOtherFees.Rows.RemoveAt(dRow);

            if (dgvPOOtherFees.Rows.Count == 0)
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
            dR["PONo"] = txtPRNo.Text;
            dR["OtherFeesID"] = 1;
            dtOtherFees.Rows.Add(dR);
            bsPOOtherFees.DataSource = dtOtherFees;
            dgvPOOtherFees.DataSource = bsPOOtherFees;            

            BindPOOtherFees();
            GetPOSubTotalOtherFees();
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

        private void mskDateInspected_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(759, 127); pnlCalendar.BringToFront(); 
        }

        private void mskDateBackOrder_DoubleClick(object sender, EventArgs e)
        {
            pnlCalendar.Visible = true; pnlCalendar.Location = new Point(759, 103); pnlCalendar.BringToFront(); 
        }

        private void cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            if (pnlCalendar.Location == new Point(759, 127))
            {
                mskDateInspected.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            else if (pnlCalendar.Location == new Point(759, 103))
            {
                 mskDateBackOrder.Text = cal.SelectionRange.Start.ToString("MM/dd/yyyy");
            }
            
            pnlCalendar.Visible = false;
        }

        private void cal_MouseLeave(object sender, EventArgs e)
        {
            pnlCalendar.Visible = false; 
        }        

        private void btnTestData_Click(object sender, EventArgs e)
        {
            bsPOMaster.EndEdit();

            dtMaster.Rows[bsPOMaster.Position]["PONo"].ToString();
            for (int i = 0; i < dtMaster.Rows.Count; i++)
            {
                MessageBox.Show(dtMaster.Rows[i]["PONo"].ToString());
                MessageBox.Show(dtMaster.Rows[i].RowState.ToString());
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintReport();
        }

        private void txtPRNo_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(PurchaseRequisition));

            if (intOpen == 1)
            {
                PSSClass.General.CloseForm(typeof(PurchaseRequisition));
            }
            PurchaseRequisition childForm = new PurchaseRequisition();
            childForm.Text = "PURCHASE REQUISITION";
            childForm.MdiParent = this.MdiParent;
            childForm.strPRNo = txtPRNo.Text;
            childForm.nPRSw = 1;
            childForm.Show();      
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

        private void btnCancelSend_Click(object sender, EventArgs e)
        {
            pnlEMail.Visible = false;
        }

        private void btnEMailPO_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (Convert.ToDecimal(txtPOAmount.Text) < 25000 && txtApprovalDate.Text == "")
                {
                    MessageBox.Show("PO is not yet approved at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Convert.ToDecimal(txtPOAmount.Text) > 25000 && txtPresApprovalDate.Text == "")
                {
                    MessageBox.Show("PO requires approval by the President.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (strFileAccess != "FA")
                {
                    MessageBox.Show("You have no permission to e-mail a PO at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtEmail.Text.Trim() == "")
                {
                    MessageBox.Show("No e-mail address entered. Please provide " + Environment.NewLine +
                                    "e-mail address in the send email box.", Application.ProductName);
                }
                PurchaseOrdersRpt rpt = new PurchaseOrdersRpt();
                txtPONo.Text = dgvFile.CurrentRow.Cells["PONo"].Value.ToString();
                rpt.PONo = txtPONo.Text.Trim();
                if (chkShipTo.Checked)
                    rpt.DlvrTo = 1;
                else
                    rpt.DlvrTo = 2;

                rpt.pubPrtMode = 3;
                try
                {
                    rpt.WindowState = FormWindowState.Minimized;
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in loading..." + ex.Message, Application.ProductName);
                }
                rpt.Close(); rpt.Dispose();
                txtEMailBody.Text = ""; lstAttachment.Items.Clear();
                txtTo.Text = txtEmail.Text.Trim();

                string strPDFFile = @"\\GBLNJ4\GIS\PO\" + DateTime.Now.Year.ToString() + "\\" + txtPONo.Text + ".pdf";

                lstAttachment.Items.Add(strPDFFile);
                lnkDoc.Text = strPDFFile;

                txtEMailBody.Text = "Dear Vendor," + Environment.NewLine + Environment.NewLine; //+ txtContact.Text.Trim() + "," 
                txtEMailBody.Text = txtEMailBody.Text +
                "Please find attached approved Purchase Order for the Quote provided," + Environment.NewLine +
                "acknowledge this order and provide an order number. Please indicate" + Environment.NewLine +
                "the delivery date and notify us immediately for any delay in the order." + Environment.NewLine + Environment.NewLine +
                "Also, any pallet delivery would require to have trucks with lift-gate." + Environment.NewLine + Environment.NewLine +
                "Please contact us by email if any additional information is needed.";
                txtEMailBody.Text = txtEMailBody.Text.Replace("\r\n", "<br />");
                txtEMailBody.Text = txtEMailBody.Text.Replace("<br />", Environment.NewLine);
                pnlEMail.Visible = true; pnlEMail.BringToFront();
            }
        }

        private void lnkDoc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkDoc.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnSendEMail_Click(object sender, EventArgs e)
        {
            pnlEMail.Visible = false;

            string strBody = "";

            strBody = txtEMailBody.Text.Replace("\r\n", "<br />");
            string strSignature = ReadSignature();
            strBody = strBody + "<br /><br />" + strSignature;

            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.

            Outlook._MailItem oMsg = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            oMsg.HTMLBody = "<FONT face=\"Arial\">";
            oMsg.HTMLBody += strBody;
            //Add an attachment.
            oMsg.Attachments.Add(lnkDoc.Text);
            //Subject line
            oMsg.Subject = txtSubject.Text;
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

            string[] EMAddresses = txtTo.Text.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < EMAddresses.Length; i++)
            {
                if (EMAddresses[i].Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                    oRecip.Resolve();
                }
            }
            oMsg.CC = txtCC.Text;
            oMsg.Display();
            //Send.
            //oMsg.Send(); //error here
            //((Outlook._MailItem)oMsg).Send();

            // Clean up.
            //oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;
        }

        private void txtCatName_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)

            {
                try
                {
                    DataView dvwCatNames;
                    dvwCatNames = new DataView(dtCatNames, "CatalogName like '%" + txtCatName.Text.Trim().Replace("'", "''") + "%'", "CatalogName", DataViewRowState.CurrentRows);
                    dvwSetUp(dgvCatNames, dvwCatNames);
                }
                catch {

                }
                
            }
        }

        private void txtCatNo_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    DataView dvwCatNos;
                    dvwCatNos = new DataView(dtCatNos, "CatalogNo like '%" + txtCatNo.Text.Trim().Replace("'", "''") + "%'", "CatalogNo", DataViewRowState.CurrentRows);
                    dgvCatNos.Columns[0].Width = 369;
                    dgvCatNos.DataSource = dvwCatNos;
                }
                catch { }
                
            }
        }

        private void picCatNames_Click(object sender, EventArgs e)
        {
            dgvCatNames.Visible = true; dgvCatNames.BringToFront();
        }

        private void picCatNos_Click(object sender, EventArgs e)
        {
            dgvCatNos.Visible = true; dgvCatNos.BringToFront();
        }

        private void txtCatName_Enter(object sender, EventArgs e)
        {
            dgvCatNames.Visible = true; dgvCatNames.BringToFront();
        }

        private void txtCatNo_Enter(object sender, EventArgs e)
        {
            dgvCatNos.Visible = true; dgvCatNos.BringToFront();
        }

        private void dgvCatNames_DoubleClick(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                txtCatName.Text = dgvCatNames.CurrentRow.Cells["CatalogName"].Value.ToString();
                txtCatNameID.Text = dgvCatNames.CurrentRow.Cells["CatalogNameID"].Value.ToString();
                dgvCatNames.Visible = false;
            }
        }

        private void dgvCatNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                {
                    txtCatName.Text = dgvCatNames.CurrentRow.Cells["CatalogName"].Value.ToString();
                    txtCatNameID.Text = dgvCatNames.CurrentRow.Cells["CatalogNameID"].Value.ToString();
                    dgvCatNames.Visible = false;
                }
                else if (e.KeyChar == 27)
                {
                    dgvCatNames.Visible = false;
                }
            }
        }

        private void dgvCatNames_Leave(object sender, EventArgs e)
        {
            dgvCatNames.Visible = false;
        }
    }
}
