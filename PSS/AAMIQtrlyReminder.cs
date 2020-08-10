using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;

namespace PSS
{
    public partial class AAMIQtrlyReminder : Form
    {
        private DataTable dt = new DataTable();
        private DataTable dtEx = new DataTable();

        public AAMIQtrlyReminder()
        {
            InitializeComponent();
        }

        private void AAMIQtrlyReminder_Load(object sender, EventArgs e)
        {
            LoadExclusion();
            LoadReminders();
        }

        private void LoadExclusion()
        {
            dtEx = PSSClass.Samples.AAMIExclusion();
        }

        private void LoadReminders()
        {
            dgvFile.DataSource = null;

            dt = PSSClass.Samples.AAMIQtrly();
            dgvFile.DataSource = dt;
            lblCutOff.Text = "AS OF " + DateTime.Now.ToString("MMMM dd, yyyy").ToUpper();

            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvFile.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvFile.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvFile.Columns["LogNo"].HeaderText = "GBL NO.";
            dgvFile.Columns["LogNo"].Width = 60;
            dgvFile.Columns["LogNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SC"].HeaderText = "SERVICE CODE";
            dgvFile.Columns["SC"].Width = 60;
            dgvFile.Columns["SC"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SCDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvFile.Columns["SCDesc"].Width = 180;
            dgvFile.Columns["Article"].HeaderText = "ARTICLE";
            dgvFile.Columns["Article"].Width = 175;
            dgvFile.Columns["SpID"].HeaderText = "SPONSOR ID";
            dgvFile.Columns["SpID"].Width = 65;
            dgvFile.Columns["SpID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SpName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["SpName"].Width = 150;
            dgvFile.Columns["ConFN"].HeaderText = "CONTACT - FIRST NAME";
            dgvFile.Columns["ConFN"].Width = 100;
            dgvFile.Columns["ConLN"].HeaderText = "CONTACT - LAST NAME";
            dgvFile.Columns["ConLN"].Width = 100;
            dgvFile.Columns["Include"].HeaderText = "EXCLUDE";
            dgvFile.Columns["Include"].Width = 60;
            dgvFile.Columns["Include"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["EMail"].Visible = false;
            dgvFile.Columns["ConID"].Visible = false;
            lblTotal.Text = "TOTAL : " + dgvFile.Rows.Count.ToString("#,##0");
        }

        private void dgvFile_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvFile.CurrentCell.OwningColumn.Name.ToString() != "Include")
                e.Cancel = true;

            dgvFile.NotifyCurrentCellDirty(true);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string strGBL, strSC, strCFN, strArticle, strEMail;
            for (int i = 0; i < dgvFile.Rows.Count; i++)
            {
                if (dgvFile.Rows[i].Cells["Include"].Value.ToString() == "False")
                {
                    strGBL = dgvFile.Rows[i].Cells["LogNo"].Value.ToString();
                    strSC = dgvFile.Rows[i].Cells["SC"].Value.ToString();
                    strCFN = dgvFile.Rows[i].Cells["ConFN"].Value.ToString();
                    strArticle = dgvFile.Rows[i].Cells["Article"].Value.ToString();
                    strEMail = dgvFile.Rows[i].Cells["EMail"].Value.ToString();
                    SendMail(strGBL, strSC, strCFN, strArticle, strEMail);
                }
            }
            //Refresh List
            LoadReminders();
        }

        private void SendMail(string cGBL, string cSC, string cConFN, string cArticle, string cConEMail)
        {
            string strBody = "Dear " + cConFN + "," + Environment.NewLine + Environment.NewLine +
                            "This email serves as a reminder that it is almost time for your next quarterly verification on " + Environment.NewLine +
                            cArticle + "." + Environment.NewLine + Environment.NewLine +
                            "If you are following a VD Max Method please submit 10 NON-STERILE samples for bioburden and 10 STERILE" + Environment.NewLine +
                            "samples that have been sterilized at your 10²" + "  audit dose." + Environment.NewLine + Environment.NewLine + //(char)175 + (char)185 + 
                            "If you are following Method 1 please submit 10 NON-STERILE samples for bioburden and 100 STERILE samples " + Environment.NewLine +
                            "that have been sterilized at your 10² audit dose." + Environment.NewLine + Environment.NewLine +
                            "If you have any questions on the above please contact Mr. Jozef Mastej (jmastej@gibraltarlabsinc.com, ext. 611)" + Environment.NewLine +
                            "or myself." + Environment.NewLine + Environment.NewLine +
                            "Please remember to send the samples to our 16 Montesano Road, Fairfield, NJ address using a completed sample " + Environment.NewLine +
                            "submission form." + Environment.NewLine + Environment.NewLine;
                            //"Sincerely," + Environment.NewLine + Environment.NewLine + 
                            //"Kristah Kohan" + Environment.NewLine + Environment.NewLine + 
                            //"Technical Services Manager" + Environment.NewLine + Environment.NewLine + 
                            //"Gibraltar Laboratories, Inc." + Environment.NewLine + Environment.NewLine + 
                            //"122 Fairfield Road" + Environment.NewLine + Environment.NewLine + 
                            //"16 Montesano Road(shipping / receiving)" + Environment.NewLine + Environment.NewLine + 
                            //"Fairfield, NJ 07004" + Environment.NewLine + Environment.NewLine +
                            //"973-227-6882, ext. 534" + Environment.NewLine + Environment.NewLine + 
                            //"973-227-0812 (fax)";
            strBody = strBody.Replace("\r\n", "<br />");
            string strSignature = ReadSignature();
            strBody = strBody + "<br /><br />" + strSignature;

            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.
            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            //oMsg.HTMLBody = "<FONT face=\"Times New Roman\" size=\"10\">";
            oMsg.HTMLBody = "<FONT face=\"Arial\">";
            oMsg.HTMLBody += strBody.Trim();

            ////Add an attachment.
            //for (int i = 0; i < lstAttachment.Items.Count; i++)
            //{
            //    //strFile = Path.GetFileName(lstAttachment.Items[i].ToString());
            //    oMsg.Attachments.Add(lstAttachment.Items[i].ToString());
            //}
            //oMsg.Attachments.Add(lnkFile.Text);
            //Subject line
            oMsg.Subject = "Quarterly Test Reminder";
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

            string[] EMAddresses = cConEMail.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < EMAddresses.Length; i++)
            {
                if (EMAddresses[i].Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                    oRecip.Resolve();
                }
            }
            //oMsg.CC = txtCC.Text;
            //oMsg.BCC = txtBCC.Text;

            //oRecip.Resolve();
            oMsg.Display();

            ////Send.
            ////oMsg.Send();
            ////((Outlook._MailItem)oMsg).Send();

            //Clean up.
            //oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;
            //Update LogMaster Special Data for AAMI
            string strData = "";
            strData = "<SpecialData>" + "<AAMI GBLNo='" + cGBL +"' ServiceCode='" + cSC + "'>" + "<EMailDate>" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "</EMailDate>" + "</AAMI></SpecialData>";
            PSSClass.Samples.UpdLogSpclData(Convert.ToInt32(cGBL), strData);
        }

        private string ReadSignature()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            string signature = string.Empty;
            DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
            if (diInfo.Exists)
            {
                FileInfo[] fiSignature = diInfo.GetFiles("*.htm");
                if (fiSignature.Length > 0)
                {
                    StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
                    signature = sr.ReadToEnd();

                    if (!string.IsNullOrEmpty(signature))
                    {
                        string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
                        signature = signature.Replace(fileName + "_files/", appDataDir + "/" + fileName + "_files/");
                    }
                }
            }
            return signature;
        }

        private void dgvFile_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvFile.IsCurrentCellDirty)
            {
                dgvFile.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void btnExclPerm_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Include"].ToString() == "True")
                {
                    DataRow dR = dtEx.NewRow();
                    dR["SponsorID"] = dt.Rows[i]["SpID"];
                    dR["ContactID"] = dt.Rows[i]["ConID"];
                    dtEx.Rows.Add(dR);
                }
            }
            DataTable dtX = dtEx.GetChanges(DataRowState.Added);
            if (dtX != null && dtX.Rows.Count > 0)
            {
                DataView dv = dtEx.DefaultView;
                dv.Sort = "SponsorID ASC, ContactID ASC";
                DataTable dtSorted = dv.ToTable();

                string strXML = "<AAMI>";
                int nSv = 0, nCon = 0;
                for (int i = 0; i < dtSorted.Rows.Count; i++)
                {
                    if (nSv != Convert.ToInt16(dtSorted.Rows[i]["SponsorID"]))
                    {
                        if (nCon != 0)
                        {
                            strXML = strXML + "</Sponsor>";
                        }
                        nSv = Convert.ToInt16(dtSorted.Rows[i]["SponsorID"]);
                        strXML = strXML + "<Sponsor ID=" + (char)34 + nSv.ToString() + (char)34 + ">";
                        nCon = 0;
                    }
                    strXML = strXML + "<ContactID>" + dtSorted.Rows[i]["ContactID"].ToString() + "</ContactID>";
                    nCon++;
                }
                if (nCon != 0)
                {
                    strXML = strXML + "</Sponsor>";
                }
                strXML = strXML + "</AAMI>";
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    sqlcnn.Dispose();
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand("spUpdAAMIExcl", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@strXML", strXML);

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch
                {
                    sqlcnn.Dispose();
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            //Refresh List
            LoadReminders();
        }
    }
}
