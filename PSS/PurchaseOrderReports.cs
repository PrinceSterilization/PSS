using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

namespace PSS
{
    public partial class PurchaseOrderReports : Form
    {
        private int nTimer = 0, nRNo = 1;

        DataTable dtDepartments = new DataTable();                                          // MY 11/02/2015 - Pop-up GridView Department query
        DataTable dtVendors = new DataTable();                                              // MY 11/02/2015 - Pop-up GridView Vendors query

        public PurchaseOrderReports()
        {
            InitializeComponent();
            LoadDepartments();
            LoadVendors();           
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            nTimer = 0; timer1.Enabled = true; lblProgress.Visible = true;
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

        private void GenerateReport()
        {
            if (nRNo == 1 || nRNo == 2)
            {
                POReports rpt = new POReports();
                if (nRNo == 1)
                    rpt.rptName = "POUsageByDepartment";
                else if (nRNo == 2)
                    rpt.rptName = "POUsageByVendor";

                if (txtDeptID.Text.Trim() == "")
                {
                    rpt.nDepartmentID = 0;
                }
                else
                {
                    rpt.nDepartmentID = Convert.ToInt16(txtDeptID.Text);
                }
                if (txtVendorID.Text.Trim() == "")
                {
                    rpt.nVendorID = 0;
                }
                else
                {
                    rpt.nVendorID = Convert.ToInt16(txtVendorID.Text);
                }
                if (rdoByDept.Checked)
                {
                    rpt.nUsageByType = 1;
                }
                if (rdoByVendor.Checked)
                {
                    rpt.nUsageByType = 2;
                }
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
        }

        public void StandardDGVSetting(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void PurchaseOrderReports_Load(object sender, EventArgs e)
        {
            string dte = "1/1/" + DateTime.Now.Year.ToString();
            string sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");

            dtpStart.Value = Convert.ToDateTime(sdte);
        }

        private void rdoByDept_Click(object sender, EventArgs e)
        {
            nRNo = 1;
            txtVendorID.Text = "";
            txtVendorName.Text = "";
            txtDeptName.Focus();
        }

        private void rdoByVendor_Click(object sender, EventArgs e)
        {
            nRNo = 2;
            txtDeptID.Text = "";
            txtDeptName.Text = "";
            txtVendorName.Focus();
        }

        // MY 11/02/2015 - START: txt/dgvDeptNames events
        private void dgvDeptNames_DoubleClick(object sender, EventArgs e)
        {
            txtDeptID.Text = dgvDeptNames.CurrentRow.Cells["DepartmentID"].Value.ToString();
            txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
            dgvDeptNames.Visible = false;            
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
                txtDeptID.Text = dgvDeptNames.CurrentRow.Cells["DepartmentID"].Value.ToString();
                txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
                dgvDeptNames.Visible = false;   
            }
            else if (e.KeyChar == 27)
            {
                dgvDeptNames.Visible = false;
            }            
        }

        private void dgvDeptNames_Leave(object sender, EventArgs e)
        {
            dgvDeptNames.Visible = false;
        }

        private void txtDeptName_TextChanged(object sender, EventArgs e)
        {            
            DataView dvwDeptNames;
            dvwDeptNames = new DataView(dtDepartments, "DepartmentName like '%" + txtDeptName.Text.Trim().Replace("'", "''") + "%'", "DepartmentName", DataViewRowState.CurrentRows);
            dgvDeptNames.Columns[0].Width = 369;
            dgvDeptNames.DataSource = dvwDeptNames;            
        }

        private void dgvDeptNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtDeptID.Text = dgvDeptNames.CurrentRow.Cells["DepartmentID"].Value.ToString();
            txtDeptName.Text = dgvDeptNames.CurrentRow.Cells["DepartmentName"].Value.ToString();
            dgvDeptNames.Visible = false;
            dgvDeptNames.BringToFront();            
        }

        private void picDeptNames_Click(object sender, EventArgs e)
        {
            rdoByDept.Checked = true;
            txtVendorID.Text = "";
            txtVendorName.Text = "";
            chkClear.Checked = false;
            LoadDepartments();
            dgvDeptNames.Visible = true; dgvDeptNames.BringToFront();            
        }
        // MY 11/02/2015 - END: txt/dgvDeptNames events    

        // MY 11/03/2015 - START: txt/dgvVendorNames events
        private void dgvVendorNames_DoubleClick(object sender, EventArgs e)
        {
            txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
            txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
            dgvVendorNames.Visible = false;
        }

        private void dgvVendorNames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvVendorNames_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
                txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
                dgvVendorNames.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvVendorNames.Visible = false;
            }
        }

        private void dgvVendorNames_Leave(object sender, EventArgs e)
        {
            dgvVendorNames.Visible = false;
        }

        private void txtVendorName_TextChanged(object sender, EventArgs e)
        {
            DataView dvwVendorNames;
            dvwVendorNames = new DataView(dtVendors, "VendorName like '%" + txtVendorName.Text.Trim().Replace("'", "''") + "%'", "VendorName", DataViewRowState.CurrentRows);
            dgvVendorNames.Columns[0].Width = 369;
            dgvVendorNames.DataSource = dvwVendorNames;
        }

        private void dgvVendorNames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtVendorID.Text = dgvVendorNames.CurrentRow.Cells["VendorID"].Value.ToString();
            txtVendorName.Text = dgvVendorNames.CurrentRow.Cells["VendorName"].Value.ToString();
            dgvVendorNames.Visible = false;
            dgvVendorNames.BringToFront();
        }

        private void picVendorNames_Click(object sender, EventArgs e)
        {
            rdoByVendor.Checked = true;
            txtDeptID.Text = "";
            txtDeptName.Text = "";
            chkClear.Checked = false;
            LoadVendors();
            dgvVendorNames.Visible = true; dgvVendorNames.BringToFront();
        }
        // MY 11/03/2015 - END: txt/dgvVendorNames events   

        private void chkClear_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkClear.Checked)
            {
                txtDeptID.Text = "";
                txtDeptName.Text = "";
                txtVendorID.Text = "";
                txtVendorName.Text = "";
            }            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); this.Dispose();
        }
           
    }
}
