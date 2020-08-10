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
    public partial class ScanFinRpt : Form
    {
        public ScanFinRpt()
        {
            InitializeComponent();
        }

        private void txtRptNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //Check if Report is already scanned
                string strCmpyCode = "";
                int nR = txtRptNo.Text.IndexOf("R", 1);
                string strRNo = txtRptNo.Text.Substring(nR + 1, txtRptNo.Text.Length - (nR + 1));
                bool bMailed = false;
                if (txtRptNo.Text.Substring(0, 1) == "R")
                {
                    strCmpyCode = "G";
                    bMailed = PSSClass.FinalReports.RptMailDate(strCmpyCode, Convert.ToInt32(txtRptNo.Text.Substring(2, 6)), Convert.ToInt16(strRNo));
                }
                else
                {
                    strCmpyCode = "P";
                    bMailed = PSSClass.FinalReports.RptMailDate(strCmpyCode, Convert.ToInt32(txtRptNo.Text.Substring(2, 6)), Convert.ToInt16(strRNo));
                }

                if (bMailed == true)
                {
                    DialogResult dAns = new DialogResult();
                    dAns = MessageBox.Show("Report already scanned." + Environment.NewLine + "Do you want to proceed anyway?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dAns == DialogResult.No)
                    {
                        txtRptNo.Text = "";
                        return;
                    }
                }
                //Update Report Mail Date
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@CmpyCode", strCmpyCode);
                sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text.Substring(2, 6)));
                sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
                sqlcmd.Parameters.AddWithValue("@RptDate", DateTime.Now);

                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdRptMailDate";
                try
                {
                    sqlcmd.ExecuteNonQuery();

                }
                catch
                { }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();

                this.dgvReports.Rows.Add(txtRptNo.Text, DateTime.Now.ToString());
                txtRptNo.Text = "";
            }
        }

        private void ScanFinRpt_Load(object sender, EventArgs e)
        {
            dgvReports.EnableHeadersVisualStyles = false;
            dgvReports.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReports.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvReports.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReports.Columns["ReportNo"].Width = 100;
            dgvReports.Columns["ReportNo"].HeaderText = "REPORT NO.";
            dgvReports.Columns["ReportDate"].Width = 175;
            dgvReports.Columns["ReportDate"].HeaderText = "REPORT DATE";
            dgvReports.Columns["ReportDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvReports.Columns["ReportDate"].DefaultCellStyle.Format = "MM/dd/yyy hh:mm:ss -t";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnPrtLabels_Click(object sender, EventArgs e)
        {
            LabRpt rpt = new LabRpt();
            rpt.rptName = "ReportLabel";
            rpt.WindowState = FormWindowState.Maximized;
            rpt.nRptNo = Convert.ToInt32(txtRptNo.Text);
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrtCover_Click(object sender, EventArgs e)
        {
            LabRpt rpt = new LabRpt();
            rpt.rptName = "ReportCover";
            rpt.WindowState = FormWindowState.Maximized;
            rpt.nRptNo = Convert.ToInt32(txtRptNo.Text);
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvReports_CurrentCellChanged(object sender, EventArgs e)
        {
            txtRptNo.Text = dgvReports.Rows[dgvReports.CurrentCell.RowIndex].Cells["ReportNo"].Value.ToString();
        }
    }
}
