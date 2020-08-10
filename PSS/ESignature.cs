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
    public partial class ESignature : Form
    {
        public byte eSign;
        public int eRptNo;
        public int eRevNo;
        public Int64 eGBLNo;
        public Int16 eSterClassID;
        public string ePONo;
        
        public ESignature()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cboSignatory.SelectedIndex == -1)
            {
                MessageBox.Show("Please select your name from the list.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable dt = PSSClass.Employees.ValidSignature(Convert.ToInt16(cboSignatory.SelectedValue), txtEPassword.Text.Trim());
            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Invalid e-signature." + Environment.NewLine + "Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtEPassword.Focus();
                return;
            }
            
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nESign", eSign);
            sqlcmd.Parameters.AddWithValue("@RptNo", eRptNo);
            sqlcmd.Parameters.AddWithValue("@PSSNo", eGBLNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", eRevNo);
            sqlcmd.Parameters.AddWithValue("@SterClassID", eSterClassID);
            sqlcmd.Parameters.AddWithValue("@PONo", ePONo);
            sqlcmd.Parameters.AddWithValue("@UserID", Convert.ToInt16(cboSignatory.SelectedValue));

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdESignature";

            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlcmd.Dispose(); sqlcnn.Dispose();
            this.DialogResult = DialogResult.OK;
            this.Close(); this.Dispose();
        }

        private void ESignature_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (eSign == 1 || eSign == 7)
            {
                dt = PSSClass.Employees.QAManagers();
                if (dt == null)
                {
                    MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //Added JM 8/24/2016
                if (eSign == 7)
                {
                    DataRow dr = dt.NewRow();
                    dr["EmployeeName"] = "Mastej, Jozef";
                    dr["EmployeeID"] = 114;
                    dt.Rows.Add(dr);
                }
                //
            }
            else if (eSign == 2)
            {
                dt = PSSClass.Employees.StudyDirectors();
                if (dt == null)
                {
                    MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (eSign == 5 || eSign == 6)
            {
                dt = PSSClass.Employees.POApprovers();
                if (dt == null)
                {
                    MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            else if (eSign == 8)
            {
                dt = PSSClass.Employees.LabelApprovers();
                if (dt == null)
                {
                    MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            cboSignatory.DataSource = dt;
            cboSignatory.DisplayMember = "EmployeeName";
            cboSignatory.ValueMember = "EmployeeID";
            cboSignatory.SelectedIndex = -1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
