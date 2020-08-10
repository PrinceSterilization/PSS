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
    public partial class ScanInvoice : Form
    {
        public ScanInvoice()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
                //Check if Invoice is already scanned

                bool bMailed = PSSClass.FinalBilling.InvMailDate(Convert.ToInt32(txtInvNo.Text));
                if (bMailed == true)
                {
                    DialogResult dAns = new DialogResult();
                    dAns = MessageBox.Show("Invoice already scanned." + Environment.NewLine + "Do you want to proceed anyway?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dAns == DialogResult.No)
                    {
                        txtInvNo.Text = "";
                        return;
                    }
                }
                //Update Invoice Mail Date
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(txtInvNo.Text));
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdInvMailDate";
                try
                {
                    sqlcmd.ExecuteNonQuery();

                }
                catch
                { }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                this.Close(); this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); this.Dispose();
        }
    }
}
