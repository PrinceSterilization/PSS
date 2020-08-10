using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Management.Instrumentation;

namespace GIS
{
    public partial class TerminateGIS : Form
    {
        public string pubUserName = "";
        public string pubComputer = "";


        public TerminateGIS()
        {
            InitializeComponent();
        }

        private void TerminateGIS_Load(object sender, EventArgs e)
        {
            DialogResult dRes = new DialogResult();
            dRes = MessageBox.Show("GIS would now be terminated at workstation " + pubComputer + Environment.NewLine + "Please confirm process to terminate GIS.", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dRes == DialogResult.Cancel)
            {
                this.Dispose();
                this.DialogResult = DialogResult.Cancel;
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Please enter password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPassword.Select();
                return;
            }

            System.Management.ConnectionOptions connOptions = new ConnectionOptions();
            connOptions.Impersonation = ImpersonationLevel.Impersonate;
            connOptions.EnablePrivileges = true;
            connOptions.Username = "gblrom";// pubComputer; // +pubUserName;
            connOptions.Password = "Gbl@122"; // txtPassword.Text;

            ManagementScope scope = new ManagementScope("\\\\" + pubComputer + "\\root\\cimv2", connOptions);
            scope.Connect();
            ObjectQuery query = new ObjectQuery("SELECT * FROM WIN32_PROCESS WHERE Name='GIS.exe'");//ControlPages.Exe//Name='GISMain.exe' OR 
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection objectcollection = searcher.Get();

            if (pubComputer == "glrds01")
            {
                foreach (ManagementObject Obj in objectcollection)
                {
                    ManagementBaseObject outParams = Obj.InvokeMethod("GetOwner", null, null);
                    if (outParams["User"].ToString().ToUpper() == pubUserName.ToUpper())
                    {
                        Obj.InvokeMethod("Terminate", null);
                    }
                }
            }
            else
            {
                foreach (ManagementObject Obj in objectcollection)
                {
                    Obj.InvokeMethod("Terminate", null);
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

    }
}
