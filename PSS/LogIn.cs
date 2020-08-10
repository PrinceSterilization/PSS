using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Management;
using System.Management.Instrumentation;
using System.IO;

namespace PSS
{
    public partial class LogIn : Form
    {

        protected int nCtr = 0;

        public static int nUserID;
        public static string strUserID;
        public static string strPassword;

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool LockWorkStation();

        [DllImport("ADVAPI32.dll", EntryPoint =
        "LogonUserW", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool LogonUser(string lpszUsername,
        string lpszDomain, string lpszPassword, int dwLogonType,
        int dwLogonProvider, ref IntPtr phToken);

        public LogIn()
        {
            InitializeComponent();
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.White;

            //Enable For Development local testing. 
            //Disable these 2 lines for Production
            //=========================================================
            txtUserName.Text = Environment.UserName;
            txtPassword.Select(); txtPassword.SelectAll();
            //=========================================================

            //Enable these 3 lines for Production, then compile before deployment
            //=========================================================
            //txtUserName.Text = strUserID;
            //txtPassword.Text = strPassword;
            //btnLogIn_Click(null, null);
            //=========================================================
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            //using (CheckConnection chkConn = new CheckConnection())
            //{
            //    chkConn.ShowDialog();
            //    if (chkConn.DialogResult == DialogResult.Cancel)
            //    {
            //        chkConn.Dispose();
            //        Application.Exit();
            //        return;
            //    }
            //}
            string domainName = PSSClass.Security.GetDomainName(txtUserName.Text); // Extract domain name 
            //form provide DomainUsername e.g Domainname\Username
            string userName = PSSClass.Security.GetUsername(txtUserName.Text);  // Extract user name 
            //from provided DomainUsername e.g Domainname\Username

            IntPtr token = IntPtr.Zero;
            bool result = LogonUser(userName, domainName, txtPassword.Text, 3, 1, ref token);
            if (result)
            {
                //Properties.Settings.Default.UserID = txtUserName.Text;
                nUserID = PSSClass.Users.UserID(txtUserName.Text);
                strUserID = txtUserName.Text;
                if (nUserID == 0)
                {
                    MessageBox.Show("No user account found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                nCtr+=1;
                if (nCtr == 3)
                {
                    MessageBox.Show("Your account has been locked out." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LockWorkStation();
                    this.Dispose();
                    this.DialogResult = DialogResult.Cancel;
                    Application.Exit();
                    return;
                }   
                //If not authenticated then display an error message
                MessageBox.Show("Invalid account or password.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();
                return;
            }

            string strCompName = System.Net.Dns.GetHostEntry("").HostName;

            DataTable dt = PSSClass.Users.UserCurrLogin(LogIn.nUserID);
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["LogType"].ToString() == "1" && strCompName != dt.Rows[0]["ComputerName"].ToString())
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("You are currently logged in at workstation " + dt.Rows[0]["ComputerName"].ToString() + Environment.NewLine + "Do you want to login in this workstation?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    this.Dispose();
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }

                DialogResult dRes = new DialogResult();
                dRes = MessageBox.Show("PTS would now be terminated at workstation " + dt.Rows[0]["ComputerName"].ToString() + Environment.NewLine + "Please confirm process to terminate PTS.", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dRes == DialogResult.Cancel)
                {
                    this.Dispose();
                    this.DialogResult = DialogResult.Cancel;
                    return;
                }

                System.Management.ConnectionOptions connOptions = new ConnectionOptions();
                connOptions.Impersonation = ImpersonationLevel.Impersonate;
                connOptions.EnablePrivileges = true;
                connOptions.Username = "pssrom";// pubUserName;
                connOptions.Password = "Pss@1657"; // txtPassword.Text;

                ManagementScope scope = new ManagementScope("\\\\" + dt.Rows[0]["ComputerName"].ToString() + "\\root\\cimv2", connOptions);
                scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM WIN32_PROCESS WHERE Name='PTS.exe'");//ControlPages.Exe//Name='GISMain.exe' OR 
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection objectcollection = searcher.Get();

                if (LogIn.strUserID != "rcarandang" && LogIn.strUserID != "mmoreno")//exclude Terminal Server
                {
                    //    foreach (ManagementObject Obj in objectcollection)
                    //    {
                    //        ManagementBaseObject outParams = Obj.InvokeMethod("GetOwner", null, null);
                    //        if (outParams["User"].ToString().ToUpper() == dt.Rows[0]["ComputerName"].ToString().ToUpper())
                    //        {
                    //            Obj.InvokeMethod("Terminate", null);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    foreach (ManagementObject Obj in objectcollection)
                    {
                        Obj.InvokeMethod("Terminate", null);
                    }
                }
                //using (TerminateGIS xGIS = new TerminateGIS())
                //{
                //    xGIS.Location = new Point(245, 250);
                //    xGIS.pubComputer = dt.Rows[0]["ComputerName"].ToString();
                //    xGIS.pubUserName = txtUserName.Text;
                //    if (xGIS.ShowDialog() == DialogResult.Cancel)
                //    {
                //        this.Dispose();
                //        this.DialogResult = DialogResult.Cancel;
                //        return;
                //    }
                //}
                dt.Dispose();
            }

            ////Get File Version of the Current Application
            string strCurrFile = Application.StartupPath + @"\PTS.exe";
            System.Reflection.Assembly assInfo = System.Reflection.Assembly.ReflectionOnlyLoadFrom(strCurrFile);
            System.Diagnostics.FileVersionInfo currFileVer = System.Diagnostics.FileVersionInfo.GetVersionInfo(assInfo.Location);
            string strCurrVer = currFileVer.FileVersion;

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problen encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@UserID", nUserID);
            sqlcmd.Parameters.AddWithValue("@LType", 1);
            //sqlcmd.Parameters.AddWithValue("@FileVer", strCurrVer);
            sqlcmd.Parameters.AddWithValue("@CompName", strCompName);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUserLogInOut";

            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            strPassword = txtPassword.Text;

            if (File.Exists(@"\\PSAPP01\PTS\Images\PSS Background New.jpg"))
            {
                File.Copy(@"\\PSAPP01\IT Files\PTS\Images\PSS Background New.jpg", Application.StartupPath + @"\PSS Background.jpg", true);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

    }
}
