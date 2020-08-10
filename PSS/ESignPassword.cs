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
    public partial class ESignPassword : Form
    {
        public ESignPassword()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtEPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                MessageBox.Show("Pasword do not match." + Environment.NewLine + "Please try again." , Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;                    
            }
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@EPassword", txtPassword.Text);
            sqlcmd.Parameters.AddWithValue("@EmpID", Convert.ToInt16(LogIn.nUserID));

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdEPassword";

            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            MessageBox.Show("Password successfully saved.", Application.ProductName, MessageBoxButtons.OK);
            sqlcmd.Dispose(); sqlcnn.Dispose();
            this.DialogResult = DialogResult.OK;
            this.Close(); this.Dispose();
        }

        private void ESignature_Load(object sender, EventArgs e)
        {
            int nUID = PSSClass.Users.UserID(LogIn.strUserID);
            //string strPwd = PSSClass.Users.UserEPassword(Convert.ToInt16(nUID));
            //DataTable dt = new DataTable();

            //dt = PSSClass.Employees.QAManagers();
            //if (dt == null)
            //{
            //    MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
            //cboSignatory.DataSource = dt;
            //cboSignatory.DataSource = dt;
            //cboSignatory.DisplayMember = "EmployeeName";
            //cboSignatory.ValueMember = "EmployeeID";
            //try
            //{
            //    cboSignatory.SelectedValue = nUID;
            //}
            //catch { }
            txtEPassword.Text = PSSClass.Users.UserEPassword(Convert.ToInt16(nUID));
            txtPassword.Text = txtEPassword.Text;
            txtEPassword.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void chkShow_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShow.Checked == true)
            {
                txtPassword.PasswordChar = '\0';
                txtEPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
                txtEPassword.PasswordChar = '*';
            }
        }
    }
}
