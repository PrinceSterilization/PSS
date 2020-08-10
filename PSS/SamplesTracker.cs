using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GIS
{
    public partial class SamplesTracker : Form
    {
        public SamplesTracker()
        {
            InitializeComponent();
        }

        private void SamplesTracker_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

            dgvSamples.ColumnCount = 7;
            dgvSamples.Columns[0].HeaderText = "GBL No.";
            dgvSamples.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns[1].HeaderText = "SC";
            dgvSamples.Columns[1].Width = 60;
            dgvSamples.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns[2].Width = 60;
            dgvSamples.Columns[2].HeaderText = "Slash No.";
            dgvSamples.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns[2].Width = 100;
            dgvSamples.Columns[3].HeaderText = "Sample Description";
            dgvSamples.Columns[3].Width = 250;
            dgvSamples.Columns[4].HeaderText = "Sponsor ID";
            dgvSamples.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSamples.Columns[4].Width = 90;
            dgvSamples.Columns[5].HeaderText = "Sponsor Name";
            dgvSamples.Columns[5].Width = 300;
            dgvSamples.Columns[6].HeaderText = "Stage";
            dgvSamples.Columns[6].Width = 60;
            StandardDGVSetting(dgvSamples);
        }
        private void StandardDGVSetting(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void txtBarCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string strSC = ""; string strSlash = "";
                int nG = txtBarCode.Text.IndexOf("*");
                string strGBLNo = txtBarCode.Text.Substring(0, nG);
                int nSC = txtBarCode.Text.LastIndexOf("*");
                if (nSC != nG)
                {
                    strSC = txtBarCode.Text.Substring(nG + 1, nSC - 2);
                    strSlash = txtBarCode.Text.Substring(nSC + 1, txtBarCode.Text.Length - (nSC + 1));
                }
                else
                {
                    strSC = txtBarCode.Text.Substring(nG + 1, txtBarCode.Text.Length - (nG+1));
                }
                string strStage = "Receiving";
                int nStage = 0;

                if (strSlash != "")
                {
                    nStage = GISClass.Samples.SampleStage(Convert.ToInt32(strGBLNo), Convert.ToInt16(strSC), strSlash);
                    if (nStage == 0)
                    {
                        strStage = "Staging";
                    }
                    else if (nStage == 1)
                        strStage = "Testing";
                    else
                    {
                        MessageBox.Show("Stage has been completed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    var index = dgvSamples.Rows.Add();
                    dgvSamples.Rows[index].Cells[0].Value = strGBLNo;
                    dgvSamples.Rows[index].Cells[1].Value = strSC;
                    dgvSamples.Rows[index].Cells[2].Value = strSlash;

                    DataTable dt = GISClass.Samples.SampleSpecs(Convert.ToInt32(strGBLNo), strSlash);
                    if (dt == null)
                    {
                        MessageBox.Show("Unexpected error. No matching sample entry.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    dgvSamples.Rows[index].Cells[3].Value = dt.Rows[0]["SampleDesc"];
                    dgvSamples.Rows[index].Cells[4].Value = dt.Rows[0]["SpID"];
                    dgvSamples.Rows[index].Cells[5].Value = dt.Rows[0]["SpName"];
                    dgvSamples.Rows[index].Cells[6].Value = strStage;
                }
                else
                {
                    nStage = GISClass.Samples.SampleStageDoc(Convert.ToInt32(strGBLNo), Convert.ToInt16(strSC));
                    if (nStage == 0)
                    {
                        MessageBox.Show("No previous stages have been undertaken.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (nStage == 7)
                    {
                        MessageBox.Show("Process completed.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    else if (nStage == 2)
                        strStage = "Test Done";
                    else if (nStage == 3)
                        strStage = "QA";
                    else if (nStage == 4)
                        strStage = "QA Reqmnt";
                    else if (nStage == 5)
                        strStage = "QA Correction";
                    else if (nStage == 6)
                        strStage = "Final QA";
                  

                    var idx = dgvSamples.Rows.Add();
                    dgvSamples.Rows[idx].Cells[0].Value = strGBLNo;
                    dgvSamples.Rows[idx].Cells[1].Value = strSC;
                    dgvSamples.Rows[idx].Cells[2].Value = strSlash;

                    //DataTable dt = GISClass.Samples.SampleLogSC(Convert.ToInt32(strGBLNo), Convert.ToInt16(strSC));
                    //if (dt == null)
                    //{
                    //    MessageBox.Show("Unexpected error. No matching sample entry.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    return;
                    //}
                    //dgvSamples.Rows[idx].Cells[3].Value = "N/A";
                    //dgvSamples.Rows[idx].Cells[4].Value = dt.Rows[0]["SpID"];
                    //dgvSamples.Rows[idx].Cells[5].Value = dt.Rows[0]["SpName"];
                    //dgvSamples.Rows[idx].Cells[6].Value = strStage;
                }
                txtBarCode.Text = "";
                txtBarCode.Focus();
            }
        }

        private void btnAcknowledge_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvSamples.Rows.Count; i++)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                SqlCommand sqlcmd = new SqlCommand();

                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.Add(new SqlParameter("@LogNo", SqlDbType.Int));
                sqlcmd.Parameters["@LogNo"].Value = Convert.ToInt16(dgvSamples.Rows[i].Cells[0].Value);

                sqlcmd.Parameters.Add(new SqlParameter("@SC", SqlDbType.Int));
                sqlcmd.Parameters["@SC"].Value = Convert.ToInt16(dgvSamples.Rows[i].Cells[1].Value);

                sqlcmd.Parameters.Add(new SqlParameter("@SlashNo", SqlDbType.NVarChar));
                sqlcmd.Parameters["@SlashNo"].Value = dgvSamples.Rows[i].Cells[2].Value;

                sqlcmd.Parameters.Add(new SqlParameter("@SpID", SqlDbType.Int));
                sqlcmd.Parameters["@SpID"].Value = Convert.ToInt16(dgvSamples.Rows[i].Cells[4].Value);

                if (dgvSamples.Rows[i].Cells[6].Value.ToString() == "Staging")
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.TinyInt));
                    sqlcmd.Parameters["@nMode"].Value = 1;

                    sqlcmd.Parameters.Add(new SqlParameter("@StageNo", SqlDbType.SmallInt));
                    sqlcmd.Parameters["@StageNo"].Value = 1;
                }
                else if (dgvSamples.Rows[i].Cells[6].Value.ToString() == "Testing")
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.TinyInt));
                    sqlcmd.Parameters["@nMode"].Value = 2;

                    sqlcmd.Parameters.Add(new SqlParameter("@StageNo", SqlDbType.SmallInt));
                    sqlcmd.Parameters["@StageNo"].Value = 2;
                }
                else if (dgvSamples.Rows[i].Cells[6].Value.ToString() == "Test Done")
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.TinyInt));
                    sqlcmd.Parameters["@nMode"].Value = 3;

                    sqlcmd.Parameters.Add(new SqlParameter("@StageNo", SqlDbType.SmallInt));
                    sqlcmd.Parameters["@StageNo"].Value = 3;
                }
                else if (dgvSamples.Rows[i].Cells[6].Value.ToString() == "QA")
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.TinyInt));
                    sqlcmd.Parameters["@nMode"].Value = 4;

                    sqlcmd.Parameters.Add(new SqlParameter("@StageNo", SqlDbType.SmallInt));
                    sqlcmd.Parameters["@StageNo"].Value = 4;
                }
                else if (dgvSamples.Rows[i].Cells[6].Value.ToString() == "QA Reqmnt")
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.TinyInt));
                    sqlcmd.Parameters["@nMode"].Value = 5;

                    sqlcmd.Parameters.Add(new SqlParameter("@StageNo", SqlDbType.SmallInt));
                    sqlcmd.Parameters["@StageNo"].Value = 5;
                }
                else if (dgvSamples.Rows[i].Cells[6].Value.ToString() == "QA Correction")
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.TinyInt));
                    sqlcmd.Parameters["@nMode"].Value = 6;

                    sqlcmd.Parameters.Add(new SqlParameter("@StageNo", SqlDbType.SmallInt));
                    sqlcmd.Parameters["@StageNo"].Value = 6;
                }
                else if (dgvSamples.Rows[i].Cells[6].Value.ToString() == "Final QA")
                {
                    sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.TinyInt));
                    sqlcmd.Parameters["@nMode"].Value = 7;

                    sqlcmd.Parameters.Add(new SqlParameter("@StageNo", SqlDbType.SmallInt));
                    sqlcmd.Parameters["@StageNo"].Value = 7;
                }
                sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
                sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID;

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditSampleTracker";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    if (ex.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                sqlcmd.Dispose();
                sqlcnn.Dispose();
            }
            MessageBox.Show("Samples/Documents acknowledged.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            dgvSamples.Rows.Clear();
            dgvSamples.RowCount = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
