using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace GIS
{
    public partial class AutoBilling : Form
    {
        private DataTable dtInvEntries = new DataTable();
        private DataTable dtSponsors = new DataTable();
        private DataTable dtSorted = new DataTable();

        public AutoBilling()
        {
            InitializeComponent();
        }

        private void AutoInvoice_Load(object sender, EventArgs e)
        {
            LoadSponsorsDDL();
        }

        private void LoadSponsorsDDL()
        {
            DataTable dt = GISClass.Sponsors.SponsorNamesDDL();
            DataView dv = new DataView(dt);
            dv.RowFilter = "SponsorID = 1492 OR SponsorID = 58 OR SponsorID = 840";
            dtSponsors = dv.ToTable();
            dgvSponsors.DataSource = null;
            dgvSponsors.DataSource = dtSponsors;
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
            dgvSponsors.EnableHeadersVisualStyles = false;
            dgvSponsors.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSponsors.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvSponsors.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSponsors.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvSponsors.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvSponsors.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = true; dgvSponsors.BringToFront();
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSponsors;
            dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
            dgvSponsors.DataSource = dvwSponsors;
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                dgvSponsors.Visible = false;
            else
                txtSponsorID.Text = "";
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = GISClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                LoadInvoiceRef();
            }
            else
            {
                txtSponsor.Text = ""; dgvSponsors.Visible = false;
                dgvFile.DataSource = null;
            }
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            dgvSponsors.Visible = true; dgvSponsors.BringToFront();
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            dgvSponsors.Visible = false;
            LoadInvoiceRef();
        }

        private void dgvSponsors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void LoadInvoiceRef()
        {
            dtInvEntries = GISClass.FinalBilling.AutoInvoiceRef(Convert.ToInt16(txtSponsorID.Text));
            bsFile.DataSource = dtInvEntries;
            dgvFile.DataSource = bsFile;
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["PONo"].HeaderText = "PO NO.";
            dgvFile.Columns["PONo"].Width = 100;
            dgvFile.Columns["ContactName"].HeaderText = "CONTACT NAME";
            dgvFile.Columns["ContactName"].Width = 150;
            dgvFile.Columns["RptNo"].HeaderText = "REPORT NO.";
            dgvFile.Columns["RptNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["RptNo"].Width = 70;
            dgvFile.Columns["LogNo"].HeaderText = "GBL NO.";
            dgvFile.Columns["LogNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["LogNo"].Width = 70;
            dgvFile.Columns["QuoteNo"].HeaderText = "QUOTE NO.";
            dgvFile.Columns["QuoteNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["QuoteNo"].Width = 80;
            dgvFile.Columns["SC"].HeaderText = "SERVICE CODE";
            dgvFile.Columns["SC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SC"].Width = 70;
            dgvFile.Columns["SCDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvFile.Columns["SCDesc"].Width = 120;
            dgvFile.Columns["TestDesc"].HeaderText = "TEST DESCRIPTION";
            dgvFile.Columns["TestDesc"].Width = 250;
            dgvFile.Columns["RushTest"].HeaderText = "RUSH";
            dgvFile.Columns["RushTest"].Width = 50;
            dgvFile.Columns["BillQty"].HeaderText = "BILL QTY.";
            dgvFile.Columns["BillQty"].Width = 70;
            dgvFile.Columns["BillQty"].DefaultCellStyle.Format = "##0";
            dgvFile.Columns["BillQty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["UnitPrice"].HeaderText = "UNIT PRICE";
            dgvFile.Columns["UnitPrice"].DefaultCellStyle.Format = "#,##0.00";
            dgvFile.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["UnitPrice"].Width = 70;
            dgvFile.Columns["AmtDue"].HeaderText = "AMOUNT DUE";
            dgvFile.Columns["AmtDue"].DefaultCellStyle.Format = "#,##0.00";
            dgvFile.Columns["AmtDue"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["AmtDue"].Width = 70;
            dgvFile.Columns["CtrldSubs"].HeaderText = "CONTROLLED SUBSTANCE";
            dgvFile.Columns["RegPrice"].Visible = false;
            dgvFile.Columns["RushFee"].Visible = false;
            dgvFile.Columns["ContactID"].Visible = false;
        }

        private void dgvFile_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if (dgvFile.CurrentCell.OwningColumn.Name.ToString() != "BillQty" && 
            //    dgvFile.CurrentCell.OwningColumn.Name.ToString() != "CtrldSubs") 
                //dgvFile.CurrentCell.OwningColumn.Name.ToString() != "UnitPrice" && dgvFile.CurrentCell.OwningColumn.Name.ToString() != "Prepayments"
                //dgvFile.CurrentCell.OwningColumn.Name.ToString() != "RushTest"
                e.Cancel = true;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            lblPrintPreview.Visible = true;
            AcctgRpt rpt = new AcctgRpt();
            rpt.rptName = "TemporaryInvoice";
            rpt.strPO = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["PONo"].Value.ToString();
            rpt.nConID = Convert.ToInt16(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["ContactID"].Value);
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            lblPrintPreview.Visible = false;
        }

        private void dgvFile_DoubleClick(object sender, EventArgs e)
        {
            if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "RptNo" && dgvFile.CurrentCell.Value.ToString() != "")
            {
                //strAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "FinalReports");
                //if (strAccess == "")
                //    return;

                int intOpen = GISClass.General.OpenForm(typeof(FinalReports));

                if (intOpen == 1)
                {
                    GISClass.General.CloseForm(typeof(FinalReports));
                }
                FinalReports childForm = new FinalReports();
                childForm.Text = "FINAL REPORTS";
                childForm.MdiParent = this.MdiParent;
                childForm.nRptNo = Convert.ToInt32(dgvFile.CurrentCell.Value);
                childForm.nLSw = 1;
                childForm.Show();
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {

        }
    }
}
