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
    public partial class CheckConnection : Form
    {
        private int nCtr = 0;

        public CheckConnection()
        {
            InitializeComponent();
        }

        private void CheckConnection_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            GetConnection();
        }

        private void GetConnection()
        {
            SqlConnection sqlConn = GISClass.DBConnection.GISConnection();
            if (sqlConn == null)
            {
                lblConnection.ForeColor = Color.Firebrick;
                lblConnection.Text = "Connection problem encountered!" + Environment.NewLine + "Please contact the IT Department " + Environment.NewLine + 
                                     " or click Connect to try again.";
                btnTryAgain.Visible = true; btnCancel.Visible = true;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnTryAgain_Click(object sender, EventArgs e)
        {
            lblConnection.ForeColor = Color.Black;
            lblConnection.Text = "Reconnecting to GIS database..." +  Environment.NewLine + "please standby.";
            nCtr = 0;
            timer1.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr++;
            if (nCtr > 2)
            {
                btnTryAgain.Visible = false; btnCancel.Visible = false;
                GetConnection();
                timer1.Enabled = false;
            }
        }
    }
}
