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
    public partial class ScanFinRptQA : Form
    {
        private DataTable dtTestStatus = new DataTable();

        public ScanFinRptQA()
        {
            InitializeComponent();
        }

        private void txtRptNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                int nR = txtRptNo.Text.IndexOf("R", 1);
                string strRptNo = txtRptNo.Text.Substring(2, 6);
                string strRevNo = txtRptNo.Text.Substring(nR + 1, txtRptNo.Text.Length - (nR + 1));

                txtRptNo.Text = "";
                DataTable dtLogs = PSSClass.FinalReports.GetRptGBLSC(Convert.ToInt32(strRptNo), Convert.ToInt16(strRevNo));
                if (dtLogs != null && dtLogs.Rows.Count > 0)
                {
                    SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                    if (sqlcnn == null)
                    {
                        MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    for (int i = 0; i < dtLogs.Rows.Count; i++)
                    {
                        //Check if Report is already scanned
                        DataTable dt = PSSClass.Samples.CheckQAReview(Convert.ToInt32(dtLogs.Rows[i]["GBLNo"]), Convert.ToInt16(dtLogs.Rows[i]["ServiceCode"]));
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["ReportNo"].ToString() == strRptNo && dt.Rows[0]["RevisionNo"].ToString() == strRevNo)
                        {
                            dt.Dispose();
                            MessageBox.Show("Report is already scanned.", Application.ProductName);
                            break;
                        }
                        //Update Status Code
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;
                        sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(dtLogs.Rows[i]["GBLNo"]));
                        sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dtLogs.Rows[i]["ServiceCode"]));
                        sqlcmd.Parameters.AddWithValue("@ScanDate", DateTime.Now);
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spUpdTestStatus";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();

                        }
                        catch
                        { }
                        sqlcmd.Dispose();
                    }
                    sqlcnn.Close(); sqlcnn.Dispose();
                    dtLogs.Dispose();
                } 
                txtRptNo.Text = "";
                LoadStatus();
            }
        }

        private void ScanFinRptQA_Load(object sender, EventArgs e)
        {
            LoadStatus();
        }

        private void LoadStatus()
        {
            dtTestStatus = PSSClass.Samples.QAUnderReview();
            dgvFile.DataSource = dtTestStatus;
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["ReportNo"].HeaderText = "REPORT NO.";
            dgvFile.Columns["ReportNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ReportNo"].Width = 80;
            dgvFile.Columns["GBLNo"].HeaderText = "GBL NO.";
            dgvFile.Columns["GBLNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["GBLNo"].Width = 80;
            dgvFile.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvFile.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ServiceCode"].Width = 120;
            dgvFile.Columns["LotNo"].HeaderText = "LOT NO.";
            dgvFile.Columns["LotNo"].Width = 200;
            dgvFile.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["SponsorName"].Width = 310;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
