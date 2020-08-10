using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSS
{
    public partial class POBalance : Form
    {
        DataTable dtSponsors = new DataTable();
        DataTable dtPO = new DataTable();

        public POBalance()
        {
            InitializeComponent();
        }

        private void POBalance_Load(object sender, EventArgs e)
        {
            dtSponsors = PSSClass.Sponsors.SponsorNamesDDL();
            dgvSponsors.DataSource = null;
            dgvSponsors.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
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

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                LoadPO();
            }
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSponsors;
            dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
            dvwSetUp(dgvSponsors, dvwSponsors);
        }

        public void dvwSetUp(DataGridView dgvObj, DataView dvw)
        {
            dgvObj.Columns[0].Width = 369;
            dgvObj.Columns[1].Visible = false;
            dgvObj.DataSource = dvw;
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
            LoadPO();
        }

        private void btnOKPrint_Click(object sender, EventArgs e)
        {
            if (txtSponsorID.Text == "")
            {
                MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cboPO.SelectedIndex == -1)
            {
                MessageBox.Show("Please select PO No.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            AcctgRpt rpt = new AcctgRpt();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "POBalance";
            rpt.nQ = 1;
            if (rdoPreBilled.Checked == true)
                rpt.nPOBType = 1;
            else
                rpt.nPOBType = 2;
            rpt.nSpID = Convert.ToInt16(txtSponsorID.Text);
            rpt.strPO = cboPO.Text;
            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void btnCancelPrint_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = true; dgvSponsors.BringToFront();
        }

        private void LoadPO()
        {

            dtPO = PSSClass.Billing.SponsorPO(Convert.ToInt16(txtSponsorID.Text));
            if (dtPO == null || dtPO.Rows.Count == 0)
            {
                MessageBox.Show("No POs found on file.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            cboPO.DataSource = dtPO;
            cboPO.DisplayMember = "PONo";
            cboPO.ValueMember = "PONo";
            if (dtPO.Rows.Count > 0)
            {
                cboPO.SelectedIndex = 0;
            }
        }

        private void btnPOBalSum_Click(object sender, EventArgs e)
        {
            AcctgRpt rpt = new AcctgRpt();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "POBalanceSum";
            rpt.nQ = 1;
            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void txtSponsorID_Leave(object sender, EventArgs e)
        {
            try
            {
                txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            catch { }
        }
    }
}
