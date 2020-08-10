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
    public partial class UpdateLogin : Form
    {
        private DataTable dtPO = new DataTable();
        private DataTable dtBillRef = new DataTable();


        public UpdateLogin()
        {
            InitializeComponent();
        }

        private void btnRefreshPO_Click(object sender, EventArgs e)
        {
            dtPO = PSSClass.FinalBilling.POLogVsBilling();
            dgvPONo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPONo.EnableHeadersVisualStyles = false;
            dgvPONo.DataSource = dtPO;
            dgvPONo.Columns["PSSNo"].HeaderText = "PSS No.";
            dgvPONo.Columns["PSSNo"].Width = 70;
            dgvPONo.Columns["PSSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPONo.Columns["ServiceCode"].HeaderText = "Service Code";
            dgvPONo.Columns["ServiceCode"].Width = 70;
            dgvPONo.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPONo.Columns["LTPONo"].HeaderText = "Log PO#";
            dgvPONo.Columns["LTPONo"].Width = 100;
            dgvPONo.Columns["IPONo"].HeaderText = "Inv. PO#";
            dgvPONo.Columns["IPONo"].Width = 100;
            dgvPONo.Columns["InvoiceNo"].HeaderText = "Inv. No.";
            dgvPONo.Columns["InvoiceNo"].Width = 70;
            dgvPONo.Columns["InvoiceNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPONo.Columns["SponsorID"].HeaderText = "Sponsor ID";
            dgvPONo.Columns["SponsorID"].Width = 70;
            dgvPONo.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPONo.Columns["SponsorName"].HeaderText = "Sponsor Name";
            dgvPONo.Columns["SponsorName"].Width = 350;
            dgvPONo.Columns["CompanyCode"].Visible = false;
        }

        private void UpdateLogin_Load(object sender, EventArgs e)
        {
            btnRefreshPO_Click(null, null);
        }

        private void btnUpdatePO_Click(object sender, EventArgs e)
        {
            byte bSw = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("CmpyCode", dgvPONo.Rows[dgvPONo.CurrentCell.RowIndex].Cells["CompanyCode"].Value.ToString());
            sqlcmd.Parameters.AddWithValue("LogNo", Convert.ToInt32(dgvPONo.Rows[dgvPONo.CurrentCell.RowIndex].Cells["PSSNo"].Value));
            sqlcmd.Parameters.AddWithValue("SC", Convert.ToInt16(dgvPONo.Rows[dgvPONo.CurrentCell.RowIndex].Cells["ServiceCode"].Value));
            sqlcmd.Parameters.AddWithValue("PONo", dgvPONo.Rows[dgvPONo.CurrentCell.RowIndex].Cells["IPONo"].Value);
            sqlcmd.Parameters.AddWithValue("UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdLogTestPONo";
            try
            {
                sqlcmd.ExecuteNonQuery();
                MessageBox.Show("PO No. successfully update.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                bSw = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            if (bSw == 1)
                btnRefreshPO_Click(null, null);
        }
    }
}
