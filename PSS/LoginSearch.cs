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
    public partial class LoginSearch : Form
    {
        public static DataTable dtLoginSearch;

        public LoginSearch()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            byte nSw = 0;
            int nSC = 0; int nSpID = 0;
            if (dgvSearch.Rows[dgvSearch.CurrentCell.RowIndex].Cells["TableName"].Value.ToString() == "LogSamples")
                nSw = 2;
            else if (dgvSearch.Rows[dgvSearch.CurrentCell.RowIndex].Cells["TableName"].Value.ToString() == "LogMaster")
                nSw = 3;
            else if (dgvSearch.Rows[dgvSearch.CurrentCell.RowIndex].Cells["TableName"].Value.ToString() == "LogTests")
            {
                if (dgvSearch.Rows[dgvSearch.CurrentCell.RowIndex].Cells["FieldDesc"].Value.ToString() == "SC SponsorID")
                {
                    int nI = txtSearchData.Text.IndexOf(" ");
                    if (nI == -1)
                    {
                        MessageBox.Show("Invalid format.");
                        return;
                    }
                    nSC = Convert.ToInt16(txtSearchData.Text.Substring(0, nI));
                    nSpID = Convert.ToInt16(txtSearchData.Text.Substring(nI + 1, txtSearchData.Text.Length - (nI + 1)));
                    nSw = 4;
                }
                else if (dgvSearch.Rows[dgvSearch.CurrentCell.RowIndex].Cells["FieldDesc"].Value.ToString() == "Invoice No.")
                    nSw = 5;
                else
                    nSw = 4;
            }
            dtLoginSearch = new DataTable();
            if (nSw == 2)
                dtLoginSearch = PSSClass.Samples.LogSearchSamples(dgvSearch.CurrentCell.Value.ToString(), txtSearchData.Text.Trim());
            else if (nSw == 3)
                dtLoginSearch = PSSClass.Samples.LogSearchMaster(dgvSearch.CurrentCell.Value.ToString(), txtSearchData.Text.Trim());
            else if (nSw == 4)
                dtLoginSearch = PSSClass.Samples.LogSearchTests(dgvSearch.CurrentCell.Value.ToString(), txtSearchData.Text.Trim(), nSC, nSpID);
            else if (nSw == 5)
                dtLoginSearch = PSSClass.Samples.LogSearchInv(txtSearchData.Text.Trim());

            if (dtLoginSearch == null || dtLoginSearch.Rows.Count == 0)
            {
                MessageBox.Show("No matching records found or loading error encountered!" + Environment.NewLine + "System would now reload current records.");
            }
            else
            {
                SamplesLogin childForm = new SamplesLogin();
                childForm.Text = "SAMPLES LOGIN : " + dgvSearch.CurrentCell.Value.ToString() + " - " + txtSearchData.Text.Trim();
                childForm.MdiParent = Program.mdi;
                childForm.nSearch = nSw;
                childForm.Show();
            }
        }

        private void LoginSearch_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void LoginSearch_Deactivate(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void LoginSearch_Load(object sender, EventArgs e)
        {
            this.Location = new Point(1024, 75);
            DataTable dt = PSSClass.Samples.LogSearchFields();
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No defined search fields.");
                return;
            }
            dgvSearch.DataSource = dt;
            dgvSearch.ColumnHeadersVisible = false;
            dgvSearch.RowHeadersVisible = false;
            dgvSearch.Columns[0].Width = 192;
            dgvSearch.Columns[1].Visible = false;
        }

        private void dgvSearch_Click(object sender, EventArgs e)
        {
            txtSearchData.Focus();
        }

        private void txtSearchData_Enter(object sender, EventArgs e)
        {
            txtSearchData.Select();
        }
    }
}
