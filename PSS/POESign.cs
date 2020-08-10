using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSS
{
    public partial class POESign : Form
    {
        private DataTable dtFirstApproval = new DataTable();
        private DataTable dtSecondApproval = new DataTable();

        public POESign()
        {
            InitializeComponent();
        }

        private void LoadFirstApprovalESign()
        {
            try
            {
                dtFirstApproval = PSSClass.Procurements.POFirstApprovalESign();
                bsFirstApproval.DataSource = dtFirstApproval;
                dgvFirstApproval.DataSource = bsFirstApproval;
                FirstApprovalGridSetting();
                lblFirstApprovalTotal.Text = "TOTAL : " + dtFirstApproval.Rows.Count.ToString("#,##0");
            }
            catch { }
        }

        private void LoadSecondApprovalESign()
        {
            try
            {
                dtSecondApproval = PSSClass.Procurements.POSecondApprovalESign();
                bsSecondApproval.DataSource = dtSecondApproval;
                dgvSecondApproval.DataSource = bsSecondApproval;
                SecondApprovalGridSetting();
                lblSecondApprovalTotal.Text = "TOTAL : " + dtSecondApproval.Rows.Count.ToString("#,##0");                
            }
            catch { }
        }       

        private void FirstApprovalGridSetting()
        {
            dgvFirstApproval.EnableHeadersVisualStyles = false;
            dgvFirstApproval.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFirstApproval.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFirstApproval.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFirstApproval.Columns["PONo"].HeaderText = "PO Number";
            dgvFirstApproval.Columns["PODate"].HeaderText = "PO Date";
            dgvFirstApproval.Columns["DepartmentName"].HeaderText = "Department";  
            dgvFirstApproval.Columns["CostCenterName"].HeaderText = "Cost Center";           
            dgvFirstApproval.Columns["Requestor"].HeaderText = "Requestor";
            dgvFirstApproval.Columns["PRDate"].HeaderText = "Date Requested";   
            dgvFirstApproval.Columns["VendorName"].HeaderText = "Vendor Name";
            dgvFirstApproval.Columns["LineItemTotal"].HeaderText = "Line Item Total";
            dgvFirstApproval.Columns["OtherCharges"].HeaderText = "Other Charges";
            dgvFirstApproval.Columns["TotalPOAmount"].HeaderText = "Total PO Amount";      
            dgvFirstApproval.Columns["PONo"].Width = 75;
            dgvFirstApproval.Columns["PODate"].Width = 70;
            dgvFirstApproval.Columns["DepartmentName"].Width = 100;  
            dgvFirstApproval.Columns["CostCenterName"].Width = 100;           
            dgvFirstApproval.Columns["Requestor"].Width = 86;
            dgvFirstApproval.Columns["PRDate"].Width = 70;
            dgvFirstApproval.Columns["VendorName"].Width = 200;
            dgvFirstApproval.Columns["LineItemTotal"].Width = 80;
            dgvFirstApproval.Columns["OtherCharges"].Width = 80;
            dgvFirstApproval.Columns["TotalPOAmount"].Width = 80;
            dgvFirstApproval.Columns["PODate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFirstApproval.Columns["PRDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFirstApproval.Columns["PONo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFirstApproval.Columns["PODate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFirstApproval.Columns["PRDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFirstApproval.Columns["LineItemTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFirstApproval.Columns["LineItemTotal"].DefaultCellStyle.Format = "N2";
            dgvFirstApproval.Columns["OtherCharges"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFirstApproval.Columns["OtherCharges"].DefaultCellStyle.Format = "N2";
            dgvFirstApproval.Columns["TotalPOAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFirstApproval.Columns["TotalPOAmount"].DefaultCellStyle.Format = "N2";
        }

        private void SecondApprovalGridSetting()
        {
            dgvSecondApproval.EnableHeadersVisualStyles = false;
            dgvSecondApproval.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSecondApproval.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvSecondApproval.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSecondApproval.Columns["PONo"].HeaderText = "PO Number";
            dgvSecondApproval.Columns["PODate"].HeaderText = "PO Date";
            dgvSecondApproval.Columns["DepartmentName"].HeaderText = "Department";  
            dgvSecondApproval.Columns["CostCenterName"].HeaderText = "Cost Center";
            dgvSecondApproval.Columns["Requestor"].HeaderText = "Requestor";
            dgvSecondApproval.Columns["PRDate"].HeaderText = "Date Requested";
            dgvSecondApproval.Columns["VendorName"].HeaderText = "Vendor Name";
            dgvSecondApproval.Columns["LineItemTotal"].HeaderText = "Line Item Total";
            dgvSecondApproval.Columns["OtherCharges"].HeaderText = "Other Charges";
            dgvSecondApproval.Columns["TotalPOAmount"].HeaderText = "Total PO Amount";            
            dgvSecondApproval.Columns["PONo"].Width = 75;
            dgvSecondApproval.Columns["PODate"].Width = 70;
            dgvSecondApproval.Columns["DepartmentName"].Width = 100;
            dgvSecondApproval.Columns["CostCenterName"].Width = 100;
            dgvSecondApproval.Columns["Requestor"].Width = 86;
            dgvSecondApproval.Columns["PRDate"].Width = 70;
            dgvSecondApproval.Columns["VendorName"].Width = 200;
            dgvSecondApproval.Columns["LineItemTotal"].Width = 80;
            dgvSecondApproval.Columns["OtherCharges"].Width = 80;
            dgvSecondApproval.Columns["TotalPOAmount"].Width = 80;      
            dgvSecondApproval.Columns["PODate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvSecondApproval.Columns["PRDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvSecondApproval.Columns["PONo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSecondApproval.Columns["PODate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSecondApproval.Columns["PRDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSecondApproval.Columns["LineItemTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSecondApproval.Columns["LineItemTotal"].DefaultCellStyle.Format = "N2";
            dgvSecondApproval.Columns["OtherCharges"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSecondApproval.Columns["OtherCharges"].DefaultCellStyle.Format = "N2";
            dgvSecondApproval.Columns["TotalPOAmount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSecondApproval.Columns["TotalPOAmount"].DefaultCellStyle.Format = "N2";
        }

        private void dgvFirstApproval_DoubleClick(object sender, EventArgs e)
        {
            if (dgvFirstApproval.Rows.Count > 0)
            {
                int intOpen = PSSClass.General.OpenForm(typeof(PurchaseOrder));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(PurchaseOrder));
                }
                PurchaseOrder childForm = new PurchaseOrder();
                childForm.Text = "Purchase Order";
                childForm.MdiParent = this.MdiParent;
                childForm.strPONo = dgvFirstApproval.Rows[dgvFirstApproval.CurrentCell.RowIndex].Cells["PONo"].Value.ToString();
                childForm.nPOSw = 1;
                childForm.Show();
            }
        }

        private void dgvQA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvQA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                dgvFirstApproval_DoubleClick(null, null);
        }

        private void POESign_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            LoadFirstApprovalESign();
            LoadSecondApprovalESign();  
        }

        private void dgvSecondApproval_DoubleClick(object sender, EventArgs e)
        {
            if (dgvSecondApproval.Rows.Count > 0)
            {
                int intOpen = PSSClass.General.OpenForm(typeof(PurchaseOrder));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(PurchaseOrder));
                }
                PurchaseOrder childForm = new PurchaseOrder();
                childForm.Text = "Purchase Order";
                childForm.MdiParent = this.MdiParent;
                childForm.strPONo = dgvSecondApproval.Rows[dgvSecondApproval.CurrentCell.RowIndex].Cells["PONo"].Value.ToString();
                childForm.nPOSw = 1;
                childForm.Show();
            }
        }
    }
}
