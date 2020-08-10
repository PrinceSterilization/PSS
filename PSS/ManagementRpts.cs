using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core; // Office 12.0 Library
using System.Threading;
using System.Data.SqlClient;
using System.IO;

namespace PSS
{
    public partial class ManagementRpts : Form
    {
        private string strFY = DateTime.Now.Year.ToString();
        private byte nTimer = 0;
        private int nRNo = 1;
        private string strFileAccess = "";

        public virtual event EventHandler WorkStart;
        public virtual event EventHandler WorkFinished;

        // Events
        public void OnWorkStart(object sender, EventArgs e)
        {
            if (WorkStart != null) { WorkStart(sender, e); }
        }

        public void OnWorkFinished(object sender, EventArgs e)
        {
            if (WorkFinished != null) { WorkFinished(sender, e); }
        }


        private int nProRev = 0, nRev = 0;
        private DataTable dtSC = new DataTable();

        public ManagementRpts()
        {
            InitializeComponent();
        }

        private void ManagementRpts_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            string dte = "1/1/" + DateTime.Now.Year.ToString();
            string sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");

            dtpStart.Value = Convert.ToDateTime(sdte);
            dtpFrom.Value = Convert.ToDateTime(sdte);            

            int nY = DateTime.Now.Year;
            for (int i = 1; i < 8; i++)
            {
                cboVitalFY.Items.Add(nY.ToString());
                nY--;
            }

            cboVitalFY.SelectedIndex = 0;
            cboProformaRev.SelectedIndex = 0;

            DataTable dt = PSSClass.ServiceCodes.SCDDL();

            DataView dv = dt.DefaultView;
            dv.Sort = "ServiceCode ASC";
            dtSC = dv.ToTable();

            for (int i = 0; i < dtSC.Rows.Count; i++)
            {
                cboSCRev.Items.Add(dtSC.Rows[i]["ServiceCode"].ToString());
                cboSC.Items.Add(dtSC.Rows[i]["ServiceCode"].ToString());
            }
            cboSCRev.Items.Insert(0, "All");
            cboSCRev.SelectedIndex = 0;
            cboSC.Items.Insert(0, "All");
            cboSC.SelectedIndex = 0;
            //Sponsor ID
            dt = PSSClass.Sponsors.SpIDDDL();
            dv = dt.DefaultView;
            dv.Sort = "SponsorID ASC";
            DataTable dtSp = dv.ToTable();

            for (int i = 0; i < dtSp.Rows.Count; i++)
            {
                cboSpID.Items.Add(dtSp.Rows[i]["SponsorID"].ToString());
            }
            cboSpID.Items.Insert(0, "All");
            cboSpID.SelectedIndex = 0;

            //Sponsor Name
            dt = PSSClass.Sponsors.SponsorNamesDDL();
            dv = dt.DefaultView;
            dv.Sort = "SponsorName ASC";
            dtSp = dv.ToTable();

            for (int i = 0; i < dtSp.Rows.Count; i++)
            {
                cboSponsorRev.Items.Add(dtSp.Rows[i]["SponsorName"].ToString());
            }
            cboSponsorRev.Items.Insert(0, "--Select Sponsor --");
            cboSponsorRev.SelectedIndex = 0;

            for (int i = 1; i < 16; i++)
            {
                cboPivotStyle.Items.Add(i.ToString());
            }
            cboPivotStyle.SelectedIndex = 0;

            //Service Departments
            DataTable dtDeptCbo =  PSSClass.ServiceDepartments.DepartmentsMgnt();
            DataRow dr = dtDeptCbo.NewRow();
            dr["DepartmentName"] = "--Select All--";
            dr["DepartmentID"] = 0;
            dtDeptCbo.Rows.InsertAt(dr, 0);
            cboDepartments.DataSource = dtDeptCbo;
            cboDepartments.SelectedIndex = 0;
            cboDepartments.DisplayMember = "DepartmentName";
            cboDepartments.ValueMember = "DepartmentID";

            DataTable dtDept = PSSClass.ServiceDepartments.DepartmentsMgnt();
            DataGridViewTextBoxColumn dbCol1 = new DataGridViewTextBoxColumn();
            DataGridViewCheckBoxColumn dbCol2 = new DataGridViewCheckBoxColumn();
            DataGridViewTextBoxColumn dbCol3 = new DataGridViewTextBoxColumn();

            dbCol1.Name = "DepartmentName";
            dbCol1.HeaderText = "DEPARTMENT NAME";
            dbCol2.Name = "Selected";
            dbCol2.HeaderText = "SELECT";
            dbCol3.Name = "DepartmentID";
            dbCol3.Visible = false;
            
            dgvDepartments.Columns.AddRange(new DataGridViewColumn[] { dbCol1, dbCol2, dbCol3 });
            dgvDepartments.Columns[0].Width = 175;
            dgvDepartments.Columns[1].Width = 50;

            foreach (DataRow row in dtDept.Rows)
            {
                dgvDepartments.Rows.Add(row["DepartmentName"].ToString(), false, row["DepartmentID"]);
            }
            dgvDepartments.EnableHeadersVisualStyles = false;
            dgvDepartments.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDepartments.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvDepartments.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDepartments.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvDepartments.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvDepartments.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //Default to Checked - 4-16-2018
            for (int i = 0; i < dgvDepartments.Rows.Count; i++)
            {
                dgvDepartments.Rows[i].Cells["Selected"].Value = true;
            }
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "ManagementRpts");
            tabMgmntRpts.SelectedIndex = 0;
        }

        private void btnInvTotal_Click(object sender, EventArgs e)
        {
            MgmtRpts rpt = new MgmtRpts();
            rpt.rptName = "InvYrTotal";
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLogTotal_Click(object sender, EventArgs e)
        {
            MgmtRpts rpt = new MgmtRpts();
            rpt.rptName = "LogYrTotal";
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            rpt.nGBL = Convert.ToInt16(chkGBL.CheckState);
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRptTotal_Click(object sender, EventArgs e)
        {
            MgmtRpts rpt = new MgmtRpts();
            rpt.rptName = "RptYrTotal";
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            rpt.nGBL = Convert.ToInt16(chkGBL.CheckState);
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNewCustomers_Click(object sender, EventArgs e)
        {
            MgmtRpts rpt = new MgmtRpts();
            rpt.rptName = "NewSponsors";
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMgmtSummary_Click(object sender, EventArgs e)
        {
            DataTable dt = PSSClass.ManagementReports.SummaryTable(Convert.ToInt16(cboVitalFY.Text));
            //dt.Columns.Add("SalesLogin", typeof(decimal));
            //dt.Columns.Add("SalesRpt", typeof(decimal));
            //dt.Columns.Add("SalesInv", typeof(decimal));
            //dt.Columns.Add("LoginRpt", typeof(decimal));

            //dt.Columns.Add("Sales", typeof(decimal));
            dgvSummary.DataSource = dt;
            dgvSummary.Columns["Mo"].HeaderText = "Month #";
            dgvSummary.Columns["Mo"].Width = 65;
            dgvSummary.Columns["NoLogIns"].HeaderText = "Logins";
            dgvSummary.Columns["NoLogIns"].Width = 70;
            dgvSummary.Columns["NoRpts"].HeaderText = "Reports";
            dgvSummary.Columns["NoRpts"].Width = 70;
            dgvSummary.Columns["NoInv"].HeaderText = "Invoices";
            dgvSummary.Columns["NoInv"].Width = 70;
            dgvSummary.Columns["Mo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSummary.Columns["NoLogins"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["NoLogins"].DefaultCellStyle.Format = "#,##0";
            dgvSummary.Columns["NoRpts"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["NoRpts"].DefaultCellStyle.Format = "#,##0";
            dgvSummary.Columns["NoInv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["NoInv"].DefaultCellStyle.Format = "#,##0";
            dgvSummary.Columns["Sales"].DefaultCellStyle.Format = "#,##0";
            dgvSummary.Columns["Sales"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["Sales"].Width = 80;
            dgvSummary.Columns["SalesRpt"].HeaderText = "$/Report";
            dgvSummary.Columns["SalesRpt"].DefaultCellStyle.Format = "$#,##0";
            dgvSummary.Columns["SalesRpt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["SalesRpt"].Width = 80;
            dgvSummary.Columns["SalesInv"].HeaderText = "$/Invoice";
            dgvSummary.Columns["SalesInv"].DefaultCellStyle.Format = "$#,##0";
            dgvSummary.Columns["SalesInv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["SalesInv"].Width = 80;
            dgvSummary.Columns["SalesLogin"].HeaderText = "$/Login";
            dgvSummary.Columns["SalesLogin"].DefaultCellStyle.Format = "$#,##0";
            dgvSummary.Columns["SalesLogin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["SalesLogin"].Width = 80;
            dgvSummary.Columns["LoginRpt"].HeaderText = "Login/Report";
            dgvSummary.Columns["LoginRpt"].DefaultCellStyle.Format = "#,##0.00";
            dgvSummary.Columns["LoginRpt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSummary.Columns["LoginRpt"].Width = 80;
            dgvSummary.EnableHeadersVisualStyles = false;
            dgvSummary.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSummary.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvSummary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSummary.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvSummary.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvSummary.RowHeadersVisible = false;

            pnlSummary.Visible = true; pnlSummary.BringToFront();
            lblYr.Text = "FY " + cboVitalFY.Text;

            //DataTable dtSales = PSSClass.ManagementReports.SalesFrAccess(Convert.ToInt16(cboVitalFY.Text));
            //if (dtSales != null && dtSales.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dgvSummary.Rows.Count; i++)
            //    {
            //        for (int j = 0; j < dtSales.Rows.Count; j++)
            //        {
            //            if (Convert.ToDecimal(dtSales.Rows[j]["Sales"]) != 0)
            //            {
            //                if (Convert.ToInt16(dgvSummary.Rows[i].Cells[0].Value).ToString("00") == dtSales.Rows[j]["Record No"].ToString().Substring(4, 2))
            //                {
            //                    dgvSummary.Rows[i].Cells[4].Value = Convert.ToDecimal(dtSales.Rows[j]["Sales"]);
            //                }
            //            }
            //        }
            //    }
            //}
        }

        private void btnSalesGraph_Click(object sender, EventArgs e)
        {
            MgmtRpts rpt = new MgmtRpts();
            rpt.rptName = "MgmtGraph";
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnQuoteReport_Click(object sender, EventArgs e)
        {
            SalesRpt rpt = new SalesRpt();
            rpt.rptTitle = "Quotes Report";
            rpt.rptTag = "QuotesRpt.rpt";
            rpt.rptName = "QuotesRpt.rpt";
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            rpt.nSort = 1;
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProformaRev_Click(object sender, EventArgs e)
        {
            lblVitalProgress.Visible = true;
            if (cboProformaRev.SelectedIndex == 0)
                nProRev = 1; // All Sponsors
            else if (cboProformaRev.SelectedIndex == 1)
            //    nProRev = 2; // NON-INGREDION
                nProRev = 4; 
            //else if (cboProformaRev.SelectedIndex == 2)
            //    nProRev = 3; // INGREDION
            //else if (cboProformaRev.SelectedIndex == 3)
            //    nProRev = 4; // Stability
            //else if (cboProformaRev.SelectedIndex == 4)
            //    nProRev = 5; // Sterilization

            strFY = cboVitalFY.Text;
            if (chkPivot.Checked == true)
                ExportToExcelProforma();
            else
            {
                int nSpID = 0; int nSCID = 0; byte nPRev = 0;
                //DataTable dt = new DataTable();

                if (cboProformaRev.SelectedIndex == 0) // All Sponsors
                {
                    //dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 1); 
                    nPRev = 1;
                }
                else if (cboProformaRev.SelectedIndex == 1) // NON-INGREDION
                {
                    //dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 2); 
                    //nPRev = 2;
                    nPRev = 4;
                }
                //else if (cboProformaRev.SelectedIndex == 2) //INGREDION
                //{
                //    //dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 3); 
                //    nPRev = 3;
                //}
                //else if (cboProformaRev.SelectedIndex == 3) //Stability
                //{
                //    //dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 4); 
                //    nPRev = 4;
                //}
                //else if (cboProformaRev.SelectedIndex == 4) //Sterilization
                //{
                //    //dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 5); 
                //    nPRev = 5;
                //}

                //{
                //    try
                //    {
                //        nSpID = Convert.ToInt16(cboSpID.Text);
                //        dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), Convert.ToInt16(cboSpID.Text), 0, 3); //selected Sponsor
                //        nPRev = 1;
                //    }
                //    catch
                //    {
                //        MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //        return;
                //    }
                //}
                //else if (cboProformaRev.SelectedIndex == 3)
                //    try
                //    {
                //        nSCID = Convert.ToInt16(cboSCRev.Text);
                //        dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, Convert.ToInt16(cboSCRev.Text), 4); //selected Service Code
                //        nPRev = 2;
                //    }
                //    catch
                //    {
                //        MessageBox.Show("Please select Service Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //        return;
                //    }
                //else if (cboProformaRev.SelectedIndex == 4)
                //    dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 5);
                //else if (cboProformaRev.SelectedIndex == 5)
                //{
                //    dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 6);
                //    nPRev = 5;
                //}

                //if (dt == null || dt.Rows.Count == 0)
                //{
                //    MessageBox.Show("No records found for selected setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}
                //else
                //{
                //    dt.Dispose();
                //}

                
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "ProformaRev";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = nSpID;
                rpt.SC = nSCID;
                rpt.nMgmtRev = nPRev;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            lblVitalProgress.Visible = false;
        }

        private void cboVitalFY_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnSCRev_Click(object sender, EventArgs e)
        {
            lblVitalProgress.Visible = true;
            if (cboSCRev.SelectedIndex == 0 && rdoGrpSummary.Checked == false && rdoGrpDetails.Checked == false)
            {
                nRev = 3; lblSCDesc.Text = ""; //All Service Codes
            }
            else if (cboSCRev.SelectedIndex != 0 && cboSCRev.SelectedIndex != -1 && rdoGrpSummary.Checked == false && rdoGrpDetails.Checked == false)
                nRev = 4; // selected Service Code
            else if (rdoGrpSummary.Checked == true)
            {
                if (cboSCRev.SelectedIndex == 0)
                    nRev = 9;
                else if (cboSCRev.SelectedIndex != 0 && cboSCRev.SelectedIndex != -1)
                    nRev = 10;
            }
            else
            {
                if (cboSCRev.SelectedIndex == 0)
                    nRev = 11;
                else if (cboSCRev.SelectedIndex != 0 && cboSCRev.SelectedIndex != -1)
                    nRev = 12;
            }
            if (chkPivot.Checked == true)
                ExportToExcelRevenue();
            else
            {
                int nSCID = 0;
                DataTable dt = new DataTable();

                if (cboSCRev.SelectedIndex == 0)
                    dt = PSSClass.ManagementReports.RevPivotRpt(Convert.ToInt16(cboVitalFY.Text), 0, 0);
                else
                {
                    try
                    {
                        nSCID = Convert.ToInt16(cboSCRev.Text);
                        dt = PSSClass.ManagementReports.RevPivotRpt(Convert.ToInt16(cboVitalFY.Text), 0, Convert.ToInt16(cboSCRev.Text)); //selected SC
                    }
                    catch
                    {
                        MessageBox.Show("Please select Service Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No records found for selected setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "MgmtRevenue";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = 0;
                rpt.SC = nSCID;
                rpt.nMgmtRev = nRev;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            lblVitalProgress.Visible = false;
        }

        private void ExportToExcelProforma()
        {
            DataTable dt = new DataTable();

            if (cboProformaRev.SelectedIndex == 0) // All Sponsors - first column in PIVOT TABLE
                dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 0);
            else if (cboProformaRev.SelectedIndex == 1) // All Service Codes - same data source, different column settings in PIVOT TABLE, SC first
                dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 1);
            else if (cboProformaRev.SelectedIndex == 2)
            {
                try
                {
                    int nSpID = Convert.ToInt16(cboSpID.Text);
                    dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), Convert.ToInt16(cboSpID.Text), 0, 2); //selected Sponsor
                }
                catch
                {
                    MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (cboProformaRev.SelectedIndex == 3)
                try
                {
                    int nSCID = Convert.ToInt16(cboSCRev.Text);
                    dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, Convert.ToInt16(cboSCRev.Text), 3); //selected Service Code
                }
                catch
                {
                    MessageBox.Show("Please select Service Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            else if (cboProformaRev.SelectedIndex == 4)
                dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 4);
            else if (cboProformaRev.SelectedIndex == 5)
                dt = PSSClass.ManagementReports.ProformaRev(Convert.ToInt16(cboVitalFY.Text), 0, 0, 5); 

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No records found for selected setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            dgvProforma.DataSource = dt;

            Thread t1 = new Thread
            (
              delegate()
              {
                  OnWorkStart(dgvProforma, new EventArgs());

                  // Declare missing object.
                  Object oMissing = System.Reflection.Missing.Value;

                  // Change current thread culture to ("en-US").
                  // System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                  // Create a new Excel instance.
                  Excel.Application oExcel = new Excel.Application();

                  // Set Excel workbook to open with only 1 worsheet.
                  oExcel.SheetsInNewWorkbook = 1;

                  // Set the UserControl property so Excel won't shut down.
                  oExcel.UserControl = true;

                  // Add a workbook.
                  Excel.Workbook oBook = oExcel.Workbooks.Add(oMissing);

                  // Get worksheets collection 
                  Excel.Sheets oSheetsColl = oExcel.Worksheets;

                  // Get Worksheet number 1
                  Excel.Worksheet oSheet = (Excel.Worksheet)oSheetsColl.get_Item(1);

                  oSheet.Name = "Details Data";

                  // Export Data to Excel worksheet to pivot table.
                  int colIndex = 0;
                  foreach (DataGridViewColumn column in dgvProforma.Columns)
                  {
                      // Export all columns.
                      oSheet.Cells[1, colIndex + 1] = column.HeaderText;
                      for (int row = 1; row < 1 + dgvProforma.Rows.Count - 1; row++)
                      { oSheet.Cells[row + 1, colIndex + 1] = dgvProforma[colIndex, row - 1].Value; }
                      colIndex++;
                  }

                  // Get the range of the cells containing the exported data.
                  int lastCol = oSheet.UsedRange.Columns.Count;
                  int lastRow = oSheet.UsedRange.Rows.Count;

                  // Create the Range.
                  Excel.Range oSourceData = oSheet.get_Range((object)oSheet.Cells[1, 1], (object)oSheet.Cells[lastRow, lastCol]); //I cast it bec of error coming out in above row code

                  // Create Pivot Table.
                  Excel.PivotTable table1 = oSheet.PivotTableWizard(Excel.XlPivotTableSourceType.xlDatabase, oSourceData,
                                            oMissing, "PivotTable1", true, true, true, false, oMissing,
                                            oMissing, false, false, Excel.XlOrder.xlDownThenOver, 5,
                                            oMissing, oMissing);

                  // Show / Hide Pivot Fields Table
                  oBook.ShowPivotTableFieldList = false;// true;

                  // Set table format.
                  //table1.Format(Excel.XlPivotFormatType.xlTable4);
                  table1.Format(Excel.XlPivotFormatType.xlReport4);

                  //// Page Fields
                  //Excel.PivotField oPivotField1 = (Excel.PivotField)table1.PivotFields("SpID");
                  //oPivotField1.Caption = "Sponsor ID";
                  //oPivotField1.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlPageField;
                  //oPivotField1.Position = 1;
                  //oPivotField1.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;

                  // Row Fields
                  if (nProRev == 1 || nProRev == 3)
                  {
                      Excel.PivotField oPivotField2 = (Excel.PivotField)table1.PivotFields("SponsorName");
                      oPivotField2.Caption = "Sponsor Name";
                      oPivotField2.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField2.Position = 1;
                      oPivotField2.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;

                      Excel.PivotField oPivotField3 = (Excel.PivotField)table1.PivotFields("SCDesc");
                      oPivotField3.Caption = "Service Code and Description";
                      oPivotField3.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField3.Position = 2;
                      oPivotField3.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                  }
                  else if (nProRev == 2 || nProRev == 4)
                  {
                      Excel.PivotField oPivotField3 = (Excel.PivotField)table1.PivotFields("SCDesc");
                      oPivotField3.Caption = "Service Code and Description";
                      oPivotField3.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField3.Position = 1;
                      oPivotField3.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;

                      Excel.PivotField oPivotField2 = (Excel.PivotField)table1.PivotFields("SponsorName");
                      oPivotField2.Caption = "Sponsor Name";
                      oPivotField2.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField2.Position = 2;
                      oPivotField2.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                  }

                  // Column Fields
                  Excel.PivotField oPivotField11 = (Excel.PivotField)table1.PivotFields("RevMonth");
                  oPivotField11.Caption = "Month";
                  oPivotField11.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlColumnField;
                  oPivotField11.Position = 1;
                  oPivotField11.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;

                  // Data Fields
                  Excel.PivotField oPivotField21 = (Excel.PivotField)table1.PivotFields("ProLabQty");
                  oPivotField21.Caption = @"Lab - Bill Qty";
                  oPivotField21.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlDataField;
                  oPivotField21.Position = 1;
                  oPivotField21.NumberFormat = "0";

                  Excel.PivotField oPivotField22 = (Excel.PivotField)table1.PivotFields("ProLabAmt");
                  oPivotField22.Caption = @"Lab - Amount";
                  oPivotField22.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlDataField;
                  oPivotField22.Position = 2;
                  oPivotField22.NumberFormat = "$#,##0";

                  Excel.PivotField oPivotField23 = (Excel.PivotField)table1.PivotFields("ProInvQty");
                  oPivotField23.Caption = @"Inv - Bill Qty";
                  oPivotField23.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlDataField;
                  oPivotField23.Position = 3;
                  oPivotField23.NumberFormat = "0";

                  Excel.PivotField oPivotField24 = (Excel.PivotField)table1.PivotFields("ProInvAmt");
                  oPivotField24.Caption = @"Inv - Amount";
                  oPivotField24.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlDataField;
                  oPivotField24.Position = 4;
                  oPivotField24.NumberFormat = "$#,##0";

                  //Pivot Rows and Columns Range
                  int pCol = table1.ColumnRange.Count;
                  int pRow = table1.RowRange.Count;

                  // Remove Subtotals
                  //for (int index = 1; index < lastRow; index++) { try { oPivotField1.set_Subtotals(index, false); } catch (Exception ex) { } }

                  // Remove blank lines.
                  //try { oPivotField1.LayoutBlankLine = false; }
                  //catch { }

                  // Set Name and format to current worksheet.

                  Excel.Worksheet oDetailsPTSheet = (Excel.Worksheet)oExcel.ActiveSheet;

                  Excel.Range oInsert = (Excel.Range)oDetailsPTSheet.Rows[1];
                  oInsert.Insert(); oInsert.Insert(); oInsert.Insert(); oInsert.Insert(); oInsert.Insert(); oInsert.Insert(); oInsert.Insert();

                  oDetailsPTSheet.Cells[1, 1] = "PROFORMA REPORT";
                  oDetailsPTSheet.Cells[2, 1] = "FY " + strFY;
                  oDetailsPTSheet.Cells[3, 1] = "PROFORMA LAB : Testing not yet completed or ";
                  oDetailsPTSheet.Cells[4, 1] = "testing is completed but final report is not mailed, ";
                  oDetailsPTSheet.Cells[5, 1] = "not ready for invoicing.";
                  oDetailsPTSheet.Cells[6, 1] = "PROFORMA INVOICING : Testing is done, final report is mailed, ";
                  oDetailsPTSheet.Cells[7, 1] = "ready for invoicing.";
                  oDetailsPTSheet.Cells[1, 1].Font.Name = "Times New Roman";
                  oDetailsPTSheet.Cells[1, 1].Font.Size = 14;
                  oDetailsPTSheet.Cells[1, 1].Font.Bold = true;
                  oDetailsPTSheet.Cells[2, 1].Font.Name = "Times New Roman";
                  oDetailsPTSheet.Cells[2, 1].Font.Size = 14;
                  oDetailsPTSheet.Cells[2, 1].Font.Bold = true;
                  oDetailsPTSheet.Cells[3, 1].Font.Color = Color.Red;
                  oDetailsPTSheet.Cells[4, 1].Font.Color = Color.Red;
                  oDetailsPTSheet.Cells[4, 1].InsertIndent(12);
                  oDetailsPTSheet.Cells[5, 1].InsertIndent(12);
                  oDetailsPTSheet.Cells[5, 1].Font.Color = Color.Blue;
                  oDetailsPTSheet.Cells[6, 1].Font.Color = Color.Red;
                  oDetailsPTSheet.Cells[7, 1].Font.Color = Color.Blue;
                  oDetailsPTSheet.Cells[7, 1].InsertIndent(12);
                  oDetailsPTSheet.Name = "PROFORMA";
                  oDetailsPTSheet.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                  oDetailsPTSheet.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                  //oDetailsPTSheet.Cells.ShrinkToFit = true;
                  oDetailsPTSheet.Columns.UseStandardWidth = true;
                  oDetailsPTSheet.Cells.Font.Bold = false;
                  oDetailsPTSheet.Cells.Font.Italic = false;

                  Excel.Range oNoteBorder = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[3, 1], (object)oDetailsPTSheet.Cells[7, 1]);
                  oNoteBorder.BorderAround(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThin);

                  // Show / Hide GridLines.
                  oExcel.ActiveWindow.DisplayGridlines = false;

                  // Freeze Window
                  //Excel.Range oFreezePosit = (Excel.Range)oDetailsPTSheet.get_Range(oDetailsPTSheet.Cells[8, 4], oDetailsPTSheet.Cells[8, 4]);
                  Excel.Range oFreezePosit = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[11, 3], (object)oDetailsPTSheet.Cells[11, 3]);
                  oFreezePosit.Select();
                  oExcel.ActiveWindow.FreezePanes = true;

                  // Set Columns width.
                  //Excel.Range oCol1 = (Excel.Range)oDetailsPTSheet.get_Range(oDetailsPTSheet.Cells[1, 1], oDetailsPTSheet.Cells[1, 1]); oCol1.ColumnWidth = 30;
                  //Excel.Range oCol2 = (Excel.Range)oDetailsPTSheet.get_Range(oDetailsPTSheet.Cells[1, 2], oDetailsPTSheet.Cells[1, 2]); oCol2.ColumnWidth = 10;
                  //Excel.Range oCol3 = (Excel.Range)oDetailsPTSheet.get_Range(oDetailsPTSheet.Cells[1, 3], oDetailsPTSheet.Cells[1, 3]); oCol3.ColumnWidth = 9;

                  //Excel.Range oCol1 = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[1, 1], (object)oDetailsPTSheet.Cells[1, 1]);
                  //Excel.Range oCol1 = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[1, 1], (object)oDetailsPTSheet.Cells[pRow, 1]);
                  //oCol1.ColumnWidth = 25;

                  Excel.Range oCol1 = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[11, 1], (object)oDetailsPTSheet.Cells[pRow, pCol]);
                  oCol1.EntireColumn.AutoFit();

                  Excel.Range oCol2 = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[11, 3], (object)oDetailsPTSheet.Cells[pRow, pCol]);
                  oCol2.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                  //// Print Setup.

                  //try { oDetailsPTSheet.PageSetup.PrintTitleRows = "$7:$7"; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.LeftHeader = "&9" + ": &D-&T" + "\r" + " &P &N"; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.CenterHeader = "&" + "8" + ": &\"Arial,Bold Italic\"&F"; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.RightHeader = "&" + "8" + " "; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.LeftFooter = "&" + "8" + "Confidential - " + "" + "&" + "6" + "\r" + "Path: " + "&6&Z&F"; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.CenterFooter = "&" + "8" + " " + Environment.UserName + "\r" + ": " + Environment.MachineName; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.RightFooter = "&" + "8" + ": &\"Arial,Bold Italic\"&A"; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.LeftMargin = oExcel.Application.InchesToPoints(0.25); }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.RightMargin = oExcel.Application.InchesToPoints(0.25); }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.TopMargin = oExcel.Application.InchesToPoints(0.72); }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.BottomMargin = oExcel.Application.InchesToPoints(0.72); }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.HeaderMargin = oExcel.Application.InchesToPoints(0.17); }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.FooterMargin = oExcel.Application.InchesToPoints(0.25); }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.PrintHeadings = false; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.PrintGridlines = false; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.PrintComments = Microsoft.Office.Interop.Excel.XlPrintLocation.xlPrintNoComments; }
                  //catch { }

                  ////try { oDetailsPTSheet.PageSetup.PrintQuality = 600; }catch { }

                  //try { oDetailsPTSheet.PageSetup.CenterHorizontally = false; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.CenterVertically = false; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.Draft = false; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.PaperSize = Microsoft.Office.Interop.Excel.XlPaperSize.xlPaperA4; }
                  //catch { }

                  ////try { oDetailsPTSheet.PageSetup.FirstPageNumber = xlAutomatic; }catch { }

                  //try { oDetailsPTSheet.PageSetup.Order = Microsoft.Office.Interop.Excel.XlOrder.xlDownThenOver; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.BlackAndWhite = false; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.Zoom = false; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.FitToPagesWide = 1; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.FitToPagesTall = 500; }
                  //catch { }

                  //try { oDetailsPTSheet.PageSetup.PrintErrors = Microsoft.Office.Interop.Excel.XlPrintErrors.xlPrintErrorsDisplayed; }
                  //catch { }

                  // Wrap titles

                  ////Excel.Range oTitleRange = (Excel.Range)oDetailsPTSheet.get_Range(oDetailsPTSheet.Cells[7, 1], oDetailsPTSheet.Cells[7, 3]);
                  //Excel.Range oTitleRange = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[7, 1], (object)oDetailsPTSheet.Cells[7, 3]);

                  //oTitleRange.Cells.WrapText = true;
                  //oTitleRange.Cells.RowHeight = 62;
                  //oTitleRange.Interior.ColorIndex = 4;

                  //oTitleRange.Interior.Pattern = Microsoft.Office.Interop.Excel.XlPattern.xlPatternSolid;

                  // Hide WorkSheets.

                  oSheet.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;

                  // Make Excel visible to the user.
                  oExcel.Visible = true;

                  // Release the variables.
                  oBook = null;

                  //oExcel.Quit();
                  oExcel = null;

                  // Collect garbage.
                  GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers();
                  OnWorkFinished(dgvProforma, new EventArgs());
                  MessageBox.Show("Proforma Revenue report is generated.");
              }
            );
            t1.Start();
        }

        private void cboSCRev_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(cboSCRev.Text), dtSC);
            }
            catch { }
        }

        private void cboSCRev_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                lblSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(cboSCRev.Text), dtSC);
            }
            catch { }
        }

        private void cboSpID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboSpID.Text.ToUpper() != "ALL")
                    cboSponsorRev.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(cboSpID.Text));
                else
                    cboSponsorRev.SelectedIndex = 0;
            }
            catch { }
        }

        private void cboSpID_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                if (cboSpID.Text.ToUpper() != "ALL")
                    cboSponsorRev.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(cboSpID.Text));
                else
                    cboSponsorRev.SelectedIndex = 0;
            }
            catch { }
        }

        private void cboSponsorRev_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboSpID.Text = PSSClass.Sponsors.SpID(cboSponsorRev.Text).ToString();
            }
            catch { }
        }

        private void cboSponsorRev_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                cboSpID.Text = PSSClass.Sponsors.SpID(cboSponsorRev.Text).ToString();
            }
            catch { }
        }

        private void btnSponsorRev_Click(object sender, EventArgs e)
        {
            lblVitalProgress.Visible = true;
            if (cboSpID.Text == "0")
            {
                cboSpID.Text = "All";
                cboSponsorRev.Text = "-- Select Sponsor --";
            }
            if (cboSpID.SelectedIndex == 0 && rdoGrpSummary.Checked == false && rdoGrpDetails.Checked == false)
                nRev = 1; // All Sponsors
            else if (cboSpID.SelectedIndex != 0 && cboSpID.SelectedIndex != -1 && rdoGrpSummary.Checked == false && rdoGrpDetails.Checked == false)
                nRev = 2; // Selected Sponsor
            else if (rdoGrpSummary.Checked == true)
            {
                if (cboSpID.SelectedIndex == 0)
                    nRev = 5;
                else if (cboSpID.SelectedIndex != 0 && cboSpID.SelectedIndex != -1)
                    nRev = 6;
            }
            else
            {
                if (cboSpID.SelectedIndex == 0)
                    nRev = 7;
                else if (cboSpID.SelectedIndex != 0 && cboSpID.SelectedIndex != -1)
                    nRev = 8;
            }
            strFY = cboVitalFY.Text;

            if (chkPivot.Checked == true)
                ExportToExcelRevenue();
            else
            {
                int nSpID = 0;
                DataTable dt = new DataTable();

                if (cboSponsorRev.SelectedIndex == 0)
                    dt = PSSClass.ManagementReports.RevPivotRpt(Convert.ToInt16(cboVitalFY.Text), 0, 0);
                else
                {
                    try
                    {
                        nSpID = Convert.ToInt16(cboSpID.Text);
                        dt = PSSClass.ManagementReports.RevPivotRpt(Convert.ToInt16(cboVitalFY.Text), Convert.ToInt16(cboSpID.Text), 0); //selected Sponsor
                    }
                    catch
                    {
                        MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No records found for selected setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "MgmtRevenue";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = nSpID;
                rpt.SC = 0;
                rpt.nMgmtRev = nRev;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            lblVitalProgress.Visible = false;
        }

        private void ExportToExcelRevenue()
        {
            //timer1.Enabled = true;
            //lblTimer.Visible = true;

            DataTable dt = new DataTable();

            ////====
            //string strConn = "Data Source=glsql02;Initial Catalog=GISdb;Integrated Security=true;uid=;pwd=";
            //SqlConnection conn = new SqlConnection(strConn);
            //try
            //{
            //    conn.Open();
            //}
            //catch
            //{
            //    return;
            //}
            //SqlCommand sqlcmd = new SqlCommand("spRevenues", conn);
            //sqlcmd.CommandType = CommandType.StoredProcedure;
            //sqlcmd.Parameters.AddWithValue("@Yr", Convert.ToInt16(cboVitalFY.Text));
            //try
            //{
            //    SqlDataReader sqldr = sqlcmd.ExecuteReader();
            //    dt.Load(sqldr);
            //}
            //catch
            //{
            //    sqlcmd.Dispose(); conn.Close(); conn.Dispose();
            //    return;
            //}
            ////===========

            //Disable for Consolidate Revenue Testing
            //=======================================
            if (nRev == 1)
                dt = PSSClass.ManagementReports.RevPivotRpt(Convert.ToInt16(cboVitalFY.Text), 0, 0);// All Sponsors - first column in PIVOT TABLE
            else if (nRev == 2)
            {
                try
                {
                    int nSpID = Convert.ToInt16(cboSpID.Text);
                    dt = PSSClass.ManagementReports.RevPivotRpt(Convert.ToInt16(cboVitalFY.Text), Convert.ToInt16(cboSpID.Text), 0); //selected Sponsor
                }
                catch
                {
                    MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (nRev == 3)
            {
                dt = PSSClass.ManagementReports.RevPivotRpt(Convert.ToInt16(cboVitalFY.Text), 0, 0); //All Service Codes
            }
            else if (nRev == 4)
            {
                try
                {
                    int nSCID = Convert.ToInt16(cboSCRev.Text);
                    dt = PSSClass.ManagementReports.RevPivotRpt(Convert.ToInt16(cboVitalFY.Text), 0, Convert.ToInt16(cboSCRev.Text)); //selected Service Code
                }
                catch
                {
                    MessageBox.Show("Please select a Service Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }           
            //===================================

            if (dt == null || dt.Rows.Count == 0)
            {
                //sqlcmd.Dispose(); conn.Close(); conn.Dispose();
                MessageBox.Show("No records found for selected setting.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int nPStyle = Convert.ToInt16(cboPivotStyle.Text);

            dgvRevenues.DataSource = dt;

            Thread t1 = new Thread
            (
              delegate()
              {
                  OnWorkStart(dgvRevenues, new EventArgs());

                  // Declare missing object.
                  Object oMissing = System.Reflection.Missing.Value;

                  // Change current thread culture to ("en-US").
                  // System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

                  // Create a new Excel instance.
                  Excel.Application oExcel = new Excel.Application();

                  // Set Excel workbook to open with only 1 worsheet.
                  oExcel.SheetsInNewWorkbook = 1;

                  // Set the UserControl property so Excel won't shut down.
                  oExcel.UserControl = true;

                  // Add a workbook.
                  Excel.Workbook oBook = oExcel.Workbooks.Add(oMissing);

                  // Get worksheets collection 
                  Excel.Sheets oSheetsColl = oExcel.Worksheets;

                  // Get Worksheet number 1
                  Excel.Worksheet oSheet = (Excel.Worksheet)oSheetsColl.get_Item(1);

                  oSheet.Name = "Details Data";

                  // Export Data to Excel worksheet to pivot table.
                  int colIndex = 0;
                  foreach (DataGridViewColumn column in dgvRevenues.Columns)
                  {
                      // Export all columns.
                      oSheet.Cells[1, colIndex + 1] = column.HeaderText;
                      for (int row = 1; row < 1 + dgvRevenues.Rows.Count - 1; row++)
                      {
                          if (dgvRevenues[colIndex, row - 1].Value != null)
                          {
                              oSheet.Cells[row + 1, colIndex + 1] = dgvRevenues[colIndex, row - 1].Value;
                          }
                          else
                              oSheet.Cells[row + 1, colIndex + 1] = 0;
                      }

                      colIndex++;
                  }

                  // Get the range of the cells containing the exported data.
                  int lastCol = oSheet.UsedRange.Columns.Count;
                  int lastRow = oSheet.UsedRange.Rows.Count;

                  // Create the Range.
                  Excel.Range oSourceData = oSheet.get_Range((object)oSheet.Cells[1, 1], (object)oSheet.Cells[lastRow, lastCol]); //I cast it bec of error coming out in above row code

                  // Create Pivot Table.
                  Excel.PivotTable table1 = oSheet.PivotTableWizard(Excel.XlPivotTableSourceType.xlDatabase, oSourceData,
                                            oMissing, "PivotTable1", true, true, true, false, oMissing,
                                            oMissing, false, false, Excel.XlOrder.xlDownThenOver, 5,
                                            oMissing, oMissing);

                  // Show / Hide Pivot Fields Table
                  oBook.ShowPivotTableFieldList = true;

                  // Set table format.
                  if (nPStyle == 1)
                  {
                    table1.Format(Excel.XlPivotFormatType.xlReport1);
                  }
                  else if (nPStyle == 2)
                  {
                    table1.Format(Excel.XlPivotFormatType.xlReport2);
                  }
                  else if (nPStyle == 3)
                  {
                    table1.Format(Excel.XlPivotFormatType.xlReport3);
                  }
                  else if (nPStyle == 4)
                  {
                      table1.Format(Excel.XlPivotFormatType.xlReport4);
                  }

                  //// Page Fields
                  //Excel.PivotField oPivotField1 = (Excel.PivotField)table1.PivotFields("SponsorID");
                  //oPivotField1.Caption = "Sponsor ID";
                  //oPivotField1.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlPageField;
                  //oPivotField1.Position = 1;
                  //oPivotField1.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;

                  // Row Fields
                  if (nRev == 1 || nRev == 2)
                  {
                      Excel.PivotField oPivotField1 = (Excel.PivotField)table1.PivotFields("SponsorName");
                      //Excel.PivotField oPivotField1 = (Excel.PivotField)table1.PivotFields("Sponsor"); //old GIS
                      oPivotField1.Caption = "Sponsor Name";
                      oPivotField1.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField1.Position = 1;
                      oPivotField1.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                      try { oPivotField1.LayoutBlankLine = false; }
                      catch { }
                      //Remove Subtotals
                      for (int index = 1; index < lastRow; index++) 
                      { 
                          try { oPivotField1.set_Subtotals(index, false); 
                      
                          } 
                          catch 
                          { } 
                      }

                      Excel.PivotField oPivotField2 = (Excel.PivotField)table1.PivotFields("SponsorID");
                      //Excel.PivotField oPivotField2 = (Excel.PivotField)table1.PivotFields("SponsorID"); //old GIS
                      oPivotField2.Caption = "Sponsor ID";
                      oPivotField2.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField2.Position = 2;
                      oPivotField2.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                      try { oPivotField2.LayoutBlankLine = false; }
                      catch { }
                      //Remove Subtotals
                      for (int index = 1; index < lastRow; index++) 
                      { 
                          try 
                          { 
                              oPivotField2.set_Subtotals(index, false); 
                          } 
                          catch 
                          { } 
                      }
                      Excel.PivotField oPivotField3 = (Excel.PivotField)table1.PivotFields("ServiceCode");
                      //Excel.PivotField oPivotField3 = (Excel.PivotField)table1.PivotFields("ServCode");//old GIS
                      oPivotField3.Caption = "Service Code";
                      oPivotField3.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField3.Position = 3;
                      oPivotField3.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                      try { oPivotField3.LayoutBlankLine = false; }
                      catch { }
                      //Remove Subtotals
                      for (int index = 1; index < lastRow; index++) 
                      { 
                          try 
                          { 
                              oPivotField3.set_Subtotals(index, false); 
                          } 
                          catch { } 
                      }
                      Excel.PivotField oPivotField4 = (Excel.PivotField)table1.PivotFields("SCDesc");
                      //Excel.PivotField oPivotField4 = (Excel.PivotField)table1.PivotFields("SCDescrip");//old GIS
                      oPivotField4.Caption = "Service Description";
                      oPivotField4.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField4.Position = 4;
                      oPivotField4.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                      try { oPivotField4.LayoutBlankLine = false; }
                      catch { }
                      //Remove Subtotals
                      for (int index = 1; index < lastRow; index++) 
                      { 
                          try 
                          { 
                              oPivotField4.set_Subtotals(index, false); 
                          } 
                          catch { } 
                      }
                  }
                  else if (nRev == 3 || nRev == 4)
                  {
                      Excel.PivotField oPivotField1 = (Excel.PivotField)table1.PivotFields("ServiceCode");
                      //Excel.PivotField oPivotField1 = (Excel.PivotField)table1.PivotFields("ServCode");//old GIS
                      oPivotField1.Caption = "Service Code";
                      oPivotField1.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField1.Position = 1;
                      oPivotField1.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                      try { oPivotField1.LayoutBlankLine = false; }
                      catch { }
                      //Remove Subtotals
                      for (int index = 1; index < lastRow; index++) 
                      { 
                          try 
                          { 
                              oPivotField1.set_Subtotals(index, false); 
                          } 
                          catch { } 
                      }
                      Excel.PivotField oPivotField2 = (Excel.PivotField)table1.PivotFields("SCDesc");
                      //Excel.PivotField oPivotField2 = (Excel.PivotField)table1.PivotFields("SCDescrip");//old GIS
                      oPivotField2.Caption = "Service Description";
                      oPivotField2.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField2.Position = 2;
                      oPivotField2.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                      try { oPivotField2.LayoutBlankLine = false; }
                      catch { }
                      //Remove Subtotals
                      for (int index = 1; index < lastRow; index++) 
                      { 
                          try 
                          { 
                              oPivotField2.set_Subtotals(index, false); 
                          } 
                          catch { } 
                      }
                      Excel.PivotField oPivotField3 = (Excel.PivotField)table1.PivotFields("SponsorName");
                      //Excel.PivotField oPivotField3 = (Excel.PivotField)table1.PivotFields("Sponsor"); //old GIS
                      oPivotField3.Caption = "Sponsor Name";
                      oPivotField3.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField3.Position = 3;
                      oPivotField3.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                      try { oPivotField3.LayoutBlankLine = false; }
                      catch { }
                      //Remove Subtotals
                      for (int index = 1; index < lastRow; index++) 
                      { 
                          try 
                          { 
                              oPivotField3.set_Subtotals(index, false); 
                          } 
                          catch { } 
                      }
                      //Excel.PivotField oPivotField5 = (Excel.PivotField)table1.PivotFields("SponsorID");
                      Excel.PivotField oPivotField4 = (Excel.PivotField)table1.PivotFields("SponsorID"); //old GIS
                      oPivotField4.Caption = "Sponsor ID";
                      oPivotField4.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlRowField;
                      oPivotField4.Position = 2;
                      oPivotField4.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;
                      try { oPivotField4.LayoutBlankLine = false; }
                      catch { }
                      //Remove Subtotals
                      for (int index = 1; index < lastRow; index++) 
                      { 
                          try 
                          { 
                              oPivotField4.set_Subtotals(index, false); 
                          } 
                          catch { } 
                      }
                  }
                  // Column Fields
                  Excel.PivotField oPivotField11 = (Excel.PivotField)table1.PivotFields("RevMonth");
                  //Excel.PivotField oPivotField11 = (Excel.PivotField)table1.PivotFields("InvMonth");//old GIS
                  oPivotField11.Caption = "Month";
                  oPivotField11.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlColumnField;
                  oPivotField11.Position = 1;
                  oPivotField11.LayoutForm = Microsoft.Office.Interop.Excel.XlLayoutFormType.xlTabular;

                  // Data Fields
                  Excel.PivotField oPivotField21 = (Excel.PivotField)table1.PivotFields("BillQty");
                  //Excel.PivotField oPivotField21 = (Excel.PivotField)table1.PivotFields("Qty");//old GIS
                  oPivotField21.Caption = @" Bill Qty";
                  oPivotField21.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlDataField;
                  oPivotField21.Position = 1;
                  oPivotField21.Function = Excel.XlConsolidationFunction.xlSum;
                  oPivotField21.NumberFormat = "0";

                  Excel.PivotField oPivotField22 = (Excel.PivotField)table1.PivotFields("Amount");
                  //Excel.PivotField oPivotField22 = (Excel.PivotField)table1.PivotFields("ExtPrice");//old GIS
                  oPivotField22.Caption = @" Amount";
                  oPivotField22.Orientation = Microsoft.Office.Interop.Excel.XlPivotFieldOrientation.xlDataField;
                  oPivotField22.Position = 2;
                  oPivotField22.Function = Excel.XlConsolidationFunction.xlSum;
                  oPivotField22.NumberFormat = "$#,##0";

                  //Pivot Rows and Columns Range
                  int pCol = table1.ColumnRange.Count;
                  int pRow = table1.RowRange.Count;

                  // Set Name and format to current worksheet.

                  Excel.Worksheet oDetailsPTSheet = (Excel.Worksheet)oExcel.ActiveSheet;

                  Excel.Range oInsert = (Excel.Range)oDetailsPTSheet.Rows[1];
                  oInsert.Insert(); oInsert.Insert(); oInsert.Insert();

                  oDetailsPTSheet.Cells[1, 1] = "REVENUE REPORT";
                  oDetailsPTSheet.Cells[2, 1] = "FY " + strFY;
                  oDetailsPTSheet.Cells[1, 1].Font.Name = "Times New Roman";
                  oDetailsPTSheet.Cells[1, 1].Font.Size = 14;
                  oDetailsPTSheet.Cells[1, 1].Font.Bold = true;
                  oDetailsPTSheet.Cells[2, 1].Font.Name = "Times New Roman";
                  oDetailsPTSheet.Cells[2, 1].Font.Size = 14;
                  oDetailsPTSheet.Cells[2, 1].Font.Bold = true;

                  oDetailsPTSheet.Name = "REVENUES";
                  oDetailsPTSheet.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                  oDetailsPTSheet.Cells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

                  //oDetailsPTSheet.Cells.ShrinkToFit = true;
                  oDetailsPTSheet.Columns.UseStandardWidth = true;
                  oDetailsPTSheet.Cells.Font.Bold = false;
                  oDetailsPTSheet.Cells.Font.Italic = false;

                  // Show / Hide GridLines.
                  oExcel.ActiveWindow.DisplayGridlines = false;

                  // Freeze Window
                  //Excel.Range oFreezePosit = (Excel.Range)oDetailsPTSheet.get_Range(oDetailsPTSheet.Cells[8, 4], oDetailsPTSheet.Cells[8, 4]);
                  Excel.Range oFreezePosit = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[7, 3], (object)oDetailsPTSheet.Cells[7, 3]);
                  oFreezePosit.Select();
                  oExcel.ActiveWindow.FreezePanes = true;

                  Excel.Range oCol1 = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[1, 1], (object)oDetailsPTSheet.Cells[pRow, pCol]);
                  oCol1.EntireColumn.AutoFit();

                  Excel.Range oCol2 = oDetailsPTSheet.get_Range((object)oDetailsPTSheet.Cells[7, 3], (object)oDetailsPTSheet.Cells[pRow, pCol]);
                  oCol2.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                  // Hide WorkSheets.

                  oSheet.Visible = Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetHidden;

                  // Make Excel visible to the user.
                  oExcel.Visible = true;
                  // Release the variables.

                  //oBook.Close(false, oMissing, oMissing);
                  oBook = null;

                  //oExcel.Quit();
                  oExcel = null;

                  // Collect garbage.
                  GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers();
                  OnWorkFinished(dgvRevenues, new EventArgs());
                  MessageBox.Show("Revenue report is generated.");
              }
            );
            t1.Start();
        }

        private void cboSCRev_KeyPress(object sender, KeyPressEventArgs e)
        {
            lblSCDesc.Text = "";
        }

        private void cboProformaRev_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nTimer == 0)
            {
                nTimer = 1;
                timer1.Enabled = false;
                GenerateReport();
                lblProgress.Visible = false;
            }
        }

        //private void btnChemistry_Click(object sender, EventArgs e)
        //{
        //    if (!rdoCompletedTests.Checked && !rdoOutstandingTests.Checked)
        //    {
        //        MessageBox.Show("Please select report option.");
        //        return;
        //    }

        //    if (rdoCompletedTests.Checked)
        //    {
        //        nRNo = 11;
        //    }
        //    else if (rdoOutstandingTests.Checked)
        //    {
        //        nRNo = 21;
        //    }
        //    nTimer = 0; timer1.Enabled = true; lblProgress.Visible = true;
        //}

        private void btnTestsForCompletion_Click(object sender, EventArgs e)
        {
            nTimer = 0; timer1.Enabled = true; nRNo = 40; lblProgress.Visible = true; 
        }

        private void ManagementRpts_SizeChanged(object sender, EventArgs e)
        {
            //this.Size = new Size(767, 546);
        }

        //private void btnMicroBiology_Click(object sender, EventArgs e)
        //{
        //    if (!rdoCompletedTests.Checked && !rdoOutstandingTests.Checked)
        //    {
        //        MessageBox.Show("Please select report option.");
        //        return;
        //    }
            
        //    if (rdoCompletedTests.Checked)
        //    {
        //        nRNo = 13;
        //    }
        //    else if (rdoOutstandingTests.Checked)
        //    {
        //        nRNo = 23;
        //    }
        //    nTimer = 0; timer1.Enabled = true; lblProgress.Visible = true;
        //}

        private void GenerateReport()
        {
            if (nRNo == 11)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "TestsCompleted";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = 0;
                rpt.SC = 0;
                rpt.nMgmtRev = 1;
                rpt.dteStart = dtpStart.Value;
                rpt.dteEnd = dtpEnd.Value;
                string strD = "";
                for (int i = 0; i < dgvDepartments.Rows.Count; i++)
                {
                    if (dgvDepartments.Rows[i].Cells["Selected"].Value.ToString() == "True")
                    {
                        strD = strD + dgvDepartments.Rows[i].Cells["DepartmentID"].Value.ToString() + ",";
                    }
                }
                if (strD.Length > 0)
                {
                    strD = strD.Substring(0, strD.Length - 1);
                }
                rpt.strDept = strD;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (nRNo == 12)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "OutstandingTests";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = 0;
                rpt.SC = 0;
                rpt.nMgmtRev = 1;
                rpt.dteStart = dtpStart.Value;
                rpt.dteEnd = dtpEnd.Value;
                //rpt.nDeptID = 8;

                string strD = "";
                for (int i = 0; i < dgvDepartments.Rows.Count; i++)
                {
                    if (dgvDepartments.Rows[i].Cells["Selected"].Value.ToString() == "True")
                    {
                        strD = strD + dgvDepartments.Rows[i].Cells["DepartmentID"].Value.ToString() + ",";
                    }
                }
                if (strD.Length > 0)
                {
                    strD = strD.Substring(0, strD.Length - 1);
                }
                rpt.strDept = strD;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (nRNo == 13)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "TestsCompleted";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = 0;
                rpt.SC = 0;
                rpt.nMgmtRev = 1;
                rpt.dteStart = dtpStart.Value;
                rpt.dteEnd = dtpEnd.Value;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
            else if (nRNo == 22)
            {

            }
            else if (nRNo == 23)
            {
                //Video vidGIS;
                ////int width = pnlVideo.Width;
                ////int height = pnlVideo.Height;
                ////pnlVideo.Size = new Size(width, height);
                //// load the selected video file 
                ////vidGIS = new Video(Application.StartupPath + "\\gbl wars.wmv");
                //vidGIS = new Video(@"\\gblnj4\d$\GIS\videos\working.avi");
                //// set the panel as the video object’s owner 
                ////vidGIS.Owner = pnlVideo;
                //if (vidGIS.State != StateFlags.Running)
                //{
                //    vidGIS.Play();
                //}
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "OutstandingTests";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = 0;
                rpt.SC = 0;
                rpt.nMgmtRev = 1;
                rpt.nDeptID = 9;
                rpt.dteStart = dtpStart.Value;
                rpt.dteEnd = dtpEnd.Value;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (nRNo == 40)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "TestsForCompletion";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = 0;
                rpt.SC = 0;
                rpt.nMgmtRev = 1;
                rpt.dteStart = dtpStart.Value;
                rpt.dteEnd = dtpEnd.Value;
                rpt.nDeptID = 9;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (nRNo == 50)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "UnmailedReports";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = 0;
                rpt.SC = 0;
                //rpt.nMgmtRev = 1;
                rpt.dteStart = dtpStart.Value;
                rpt.dteEnd = dtpEnd.Value;
                //rpt.nDeptID = 9;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (nRNo == 60)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "StabilityReport";
                rpt.dteStart = dtpStart.Value;
                rpt.dteEnd = dtpEnd.Value;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (nRNo == 70)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "EqptServiceSched";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.SpID = 0;
                rpt.SC = 0;
                //rpt.nMgmtRev = 1;
                rpt.dteStart = dtpStart.Value;
                rpt.dteEnd = dtpEnd.Value;
                //rpt.nDeptID = 9;
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (nRNo == 80)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "SCDepartments";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                rpt.dteStart = dtpStart.Value;
                rpt.dteEnd = dtpEnd.Value;
                rpt.nDeptID = Convert.ToInt16(cboDepartments.SelectedValue);
                if (cboSC.Text == "All")
                    rpt.SC = 0;
                else
                    rpt.SC = Convert.ToInt16(cboSC.Text);
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (nRNo == 341)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "IngredionRevenue";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (nRNo == 340)
            {
                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = "IngredionProfDtls";
                rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
                try
                {
                    rpt.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            lblVitalProgress.Visible = false;
        }

        //private void btnUnmailedRpts_Click(object sender, EventArgs e)
        //{
        //    nTimer = 0; timer1.Enabled = true; nRNo = 50; lblProgress.Visible = true; 
        //}

        private void tabMgmntRpts_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 2 || e.TabPageIndex == 4)
            {
                if (strFileAccess != "FA")
                {
                    MessageBox.Show("You have no access at this time.");
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void btnEqptServiceSched_Click(object sender, EventArgs e)
        {
            nTimer = 0; timer1.Enabled = true; nRNo = 70; lblProgress.Visible = true; 
        }

        //private void btnTestsOnHold_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show("Report is under construction.");
        //}

        private void btnStabilityRpt_Click(object sender, EventArgs e)
        {
            nTimer = 0; timer1.Enabled = true; nRNo = 60; lblProgress.Visible = true; 
        }

        private void btnSDReport_Click(object sender, EventArgs e)
        {
            btnSDReport.Enabled = false; btnTestsForCompletion.Enabled = false; btnEqptServiceSched.Enabled = false; btnStabilityRpt.Enabled = false; 
            pnlDepartments.Enabled = false; pnlSCDept.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlSummary.Visible = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!rdoCompletedTests.Checked && !rdoOutstandingTests.Checked)
            {
                MessageBox.Show("Please select report option.");
                return;
            }
            nTimer = 0; timer1.Enabled = true; lblProgress.Visible = true;
        }

        private void rdoOutstandingTests_Click(object sender, EventArgs e)
        {
            nRNo = 12; lblVitalProgress.Visible = true;
        }

        private void rdoCompletedTests_Click(object sender, EventArgs e)
        {
            nRNo = 11; timer1.Enabled = true; nTimer = 0; lblVitalProgress.Visible = true;
        }

        private void btnIngredionRev_Click(object sender, EventArgs e)
        {
            if (chkSummary.Checked == false)
                nRNo = 340; 
            else
                nRNo = 341;

            timer1.Enabled = true; nTimer = 0; lblVitalProgress.Visible = true;
        }

        private void cboVitalFY_SelectedIndexChanged(object sender, EventArgs e)
        {
            string dte = "1/1/" + cboVitalFY.Text;
            string sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");
            dtpFrom.Value = Convert.ToDateTime(sdte);

            dte = "12/31/" + cboVitalFY.Text;
            sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");
            dtpTo.Value = Convert.ToDateTime(sdte);
        }

        private void btnInactiveCustomers_Click(object sender, EventArgs e)
        {
            MgmtRpts rpt = new MgmtRpts();
            rpt.rptName = "InactiveSponsors";
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ManagementRpts_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            //this.Size = new Size(767, 546);
        }

        private void btnSDCancel_Click(object sender, EventArgs e)
        {
            pnlDepartments.Enabled = true;
            btnTestsForCompletion.Enabled = true; btnEqptServiceSched.Enabled = true; btnStabilityRpt.Enabled = true;
            //pnlSDReport.Visible = false;
        }

        private void btnSDOK_Click(object sender, EventArgs e)
        {
            //pnlDepartments.Enabled = true;
            //btnTestsForCompletion.Enabled = true; btnEqptServiceSched.Enabled = true; btnStabilityRpt.Enabled = true;
            //pnlSDReport.Visible = false;

            //MgmtRpts rpt = new MgmtRpts();
            //rpt.rptName = "StudyDirReport";
            //rpt.WindowState = FormWindowState.Maximized;
            //rpt.dteStart = dtpStart.Value;
            //rpt.dteEnd = dtpEnd.Value;
            //rpt.nSDID = Convert.ToInt16(cboServDept.SelectedValue);
            //try
            //{
            //    rpt.Show();
            //}
            //catch
            //{
            //    MessageBox.Show("Report cannot be loaded." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
        }

        private void btnCancelSD_Click(object sender, EventArgs e)
        {
            btnSDReport.Enabled = true; btnTestsForCompletion.Enabled = true; btnEqptServiceSched.Enabled = true; btnStabilityRpt.Enabled = true;
            pnlDepartments.Enabled = true; pnlSCDept.Visible = false;
        }

        private void btnPrintSD_Click(object sender, EventArgs e)
        {
            nTimer = 0; timer1.Enabled = true; nRNo = 80; lblProgress.Visible = true; 
        }

        private void cboSC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(cboSC.Text), dtSC);
            }
            catch 
            {
                txtSCDesc.Text = "";
            }
        }

        private void cboSC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(cboSC.Text), dtSC);
            }
            catch 
            {
                txtSCDesc.Text = "";
            }
        }

        private void btnClearSel_Click(object sender, EventArgs e)
        {
            rdoGrpDetails.Checked = false; rdoGrpSummary.Checked = false;
            chkSummary.Checked = false; chkGBL.Checked = false; 
        }

        private void btnSpeedResp_Click(object sender, EventArgs e)
        {
            lblVitalProgress.Visible = true;
            MgmtRpts rpt = new MgmtRpts();
            rpt.rptName = "SpeedResponse";
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            if (cboMonths.SelectedIndex == -1)
                rpt.nMo = 0;
            else
                rpt.nMo = Convert.ToInt16(cboMonths.SelectedIndex + 1);
            if (cboSpID.Text == "All")
                rpt.SpID = 0;
            else
                rpt.SpID = Convert.ToInt16(cboSpID.Text);

            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            lblVitalProgress.Visible = false;
        }

        private void btnRptGBLErr_Click(object sender, EventArgs e)
        {
            lblVitalProgress.Visible = true;
            MgmtRpts rpt = new MgmtRpts();
            rpt.rptName = "RptGBLErrors";
            rpt.nYr = Convert.ToInt16(cboVitalFY.Text);
            rpt.nGBL = Convert.ToInt16(chkGBL.CheckState);             
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            lblVitalProgress.Visible = false;
        }

        private void btnSpeedResp_MouseEnter(object sender, EventArgs e)
        {
            lblSpeedResponse.Visible = true;
        }

        private void btnSpeedResp_MouseLeave(object sender, EventArgs e)
        {
            lblSpeedResponse.Visible = false;
        }

        private void btnPrintAudit_Click(object sender, EventArgs e)
        {
            if (Convert.ToDateTime(dtpAuditFrom.Value.ToShortDateString()) > Convert.ToDateTime(dtpAuditTo.Value.ToShortDateString()))
            {
                MessageBox.Show("Invalid date range.", Application.ProductName);
                return;
            }
            if (cboAuditFile.SelectedIndex == -1)
            {
                MessageBox.Show("Please select file.", Application.ProductName);
                return;
            }
            lblAuditProgress.Visible = true;
            if (cboAuditFile.SelectedIndex == 0)
            {
                AuditRpt rpt = new AuditRpt();
                rpt.rptName = "Audit LogMaster";
                rpt.dteFrom = dtpAuditFrom.Value;
                rpt.dteTo = dtpAuditTo.Value;
                try
                {
                    rpt.Show();
                }
                catch { }
                lblAuditProgress.Visible = false;
            }
        }

        private void cboMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void btnTATDashboard_Click(object sender, EventArgs e)
        {
            TATDashBoard frm = new TATDashBoard();
            frm.WindowState = FormWindowState.Normal;
            frm.Text = "TAT DASHBOARD";
            frm.dteRangeFrom = dtpFrom.Value;
            frm.dteRangeTo = dtpTo.Value;
            frm.Show();
        }
    }
}
