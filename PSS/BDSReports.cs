using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;

namespace GIS
{
    public partial class BDSReports : Form
    {
        public BDSReports()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataTable dt = GISClass.FinalReports.BDSReports();
            bsFile.DataSource = dt;
            dgvFile.DataSource = bsFile;

            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["RptNo"].HeaderText = "REPORT NO.";
            dgvFile.Columns["RevNo"].HeaderText = "REV. NO.";
            dgvFile.Columns["DteEMailed"].HeaderText = "DATE E-MAILED";
            dgvFile.Columns["Sponsor"].HeaderText = "SPONSOR";
            dgvFile.Columns["Contact"].HeaderText = "CONTACT";
            dgvFile.Columns["CEmail"].HeaderText = "CONTACT E-MAIL ADDRESS";
            dgvFile.Columns["SEMail"].HeaderText = "SENDER E-MAIL ADDRESS";
            dgvFile.Columns["SendToBDS"].HeaderText = "SEND";
            dgvFile.Columns["RptNo"].Width = 70;
            dgvFile.Columns["RptNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["RevNo"].Width = 50;
            dgvFile.Columns["RevNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DteEMailed"].Width = 80;
            dgvFile.Columns["DteEMailed"].DefaultCellStyle.Format = "MM/dd/yyyy"; // hh:mm:ss tt
            dgvFile.Columns["DteEMailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["Sponsor"].Width = 250;
            dgvFile.Columns["Contact"].Width = 110;
            dgvFile.Columns["CEMail"].Width = 200;
            dgvFile.Columns["SEMail"].Width = 170;
            dgvFile.Columns["SendToBDS"].Width = 50;
            dgvFile.Columns["SendToBDS"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["WordReport"].Width = 75;
            dgvFile.Columns["WordReport"].HeaderText = "WORD REPORT";
            dgvFile.Columns["Article"].Visible = false;
            dgvFile.Columns["YrCreated"].Visible = false;
        }

        private void BDSReports_Load(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            string strUser = "", strRecipients = "", strReport = "", strFile = "", strArticle = "", strPath = "";
            for (int i = 0; i < dgvFile.Rows.Count; i++)
            {
                if (dgvFile.Rows[i].Cells["SendToBDS"].Value.ToString() == "True")
                {
                    strUser = "BDSadmin@gibraltarlabsinc.com"; //dgvFile.Rows[i].Cells["SEMail"].Value.ToString(); ////
                    strRecipients = dgvFile.Rows[i].Cells["CEMail"].Value.ToString();
                    strArticle = dgvFile.Rows[i].Cells["Article"].Value.ToString();
                    if (dgvFile.Rows[i].Cells["WordReport"].Value.ToString() == "True")
                    {
                        if (File.Exists(@"S:\Rpts\" + dgvFile.Rows[i].Cells["YrCreated"].Value.ToString() + "\\R-" + dgvFile.Rows[i].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[i].Cells["RevNo"].Value.ToString() + ".doc"))
                        {
                            strFile = "R-" + dgvFile.Rows[i].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[i].Cells["RevNo"].Value.ToString() + ".doc";
                            strReport = "R-" + dgvFile.Rows[i].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[i].Cells["RevNo"].Value.ToString() + ".doc";
                        }
                        else if (File.Exists(@"S:\Rpts\" + dgvFile.Rows[i].Cells["YrCreated"].Value.ToString() + "\\R-" + dgvFile.Rows[i].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[i].Cells["RevNo"].Value.ToString() + ".docx"))
                        {
                            strFile = "R-" + dgvFile.Rows[i].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[i].Cells["RevNo"].Value.ToString() + ".docx";
                            strReport = "R-" + dgvFile.Rows[i].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[i].Cells["RevNo"].Value.ToString() + ".docx";
                        }
                        strPath = @"S:\Rpts\" + dgvFile.Rows[i].Cells["YrCreated"].Value.ToString() + "\\";
                    }
                    else
                    {
                        strReport = "E_" + dgvFile.Rows[i].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[i].Cells["RevNo"].Value.ToString();
                        strPath = @"\\GBLNJ4\GIS\Reports\";
                        strFile = "E_" + dgvFile.Rows[i].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[i].Cells["RevNo"].Value.ToString() + ".pdf";
                    }

                    SendToBDS(strUser, strRecipients, strReport, strFile, strArticle, strPath);
                }
            }
            MessageBox.Show("Process completed.");
            btnRefresh_Click(null, null);
        }

        private void SendToBDS(string cUser, string cRecipients, string cRptNo, string cFile, string cArticle, string cPath)
        {
            cArticle = cArticle.Replace(">", "&gt;");
            cArticle = cArticle.Replace("&", "&amp;");
            cArticle = cArticle.Replace("<", "&lt;");
            cArticle = cArticle.Replace("'", "&apos;");
            cArticle = cArticle.Replace("\"", "&quot;");

            string[] EMAddresses = cRecipients.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string strBody =
            "<fds version=" + (char)34 + "1.0" + (char)34 + ">" + Environment.NewLine +
            "<requester>" + Environment.NewLine +
            "<username>" + cUser + "</username>" + Environment.NewLine +
            "<confirmation-options>" + Environment.NewLine +
            "<notification-emails>" + Environment.NewLine +
            "<email-address>" + cUser + "</email-address>" + Environment.NewLine +
            "</notification-emails>" + Environment.NewLine +
            "</confirmation-options>" + Environment.NewLine +
            "</requester>" + Environment.NewLine +
            "<express-delivery>" + Environment.NewLine +
            "<recipients>" + Environment.NewLine +
            "<to>" + Environment.NewLine ;
            for (int i = 0; i < EMAddresses.Count(); i++)
            {
                strBody = strBody + "<email-address>" + EMAddresses[i] + "</email-address>" + Environment.NewLine;
            }
            strBody = strBody + "</to>" + Environment.NewLine +
            "<cc>" + Environment.NewLine +
            "<email-address></email-address>" + Environment.NewLine +
            "</cc>" + Environment.NewLine +
            "<bcc>" + Environment.NewLine +
            "<email-address></email-address>" + Environment.NewLine +
            "</bcc>" + Environment.NewLine +
            "</recipients>" + Environment.NewLine +
            "<subject>" + cRptNo + " " + cArticle + "</subject>" + Environment.NewLine +
            "<secure-message use-default=" + (char)34 + "N" + (char)34 + ">There is a secured message waiting for you.</secure-message>" + Environment.NewLine +
            "<files>" + Environment.NewLine +
            "<file><filename>" + cFile + "</filename>" + Environment.NewLine +
            "<description>File Attached</description>" + Environment.NewLine +
            "</file>" + Environment.NewLine +
            "</files>" + Environment.NewLine +
            "<notification-message use-default=" + (char)34 + "N" + (char)34 + ">Please click on the Sender Link below to Register for the Gibraltar Laboratories Report Server.</notification-message>" + Environment.NewLine +
            "<delivery-options>" + Environment.NewLine +
            "<date-available>" + DateTime.Now.ToShortDateString() + "</date-available>" + Environment.NewLine +
            "<date-expires>" + DateTime.Now.AddMonths(6).ToShortDateString() + "</date-expires>" + Environment.NewLine +
            "<notify-recipients>N</notify-recipients>" + Environment.NewLine +
            "<require-sign-in>Y</require-sign-in>" + Environment.NewLine +
            "<notify-on-access>" + Environment.NewLine +
            "<notification-frequency>FT</notification-frequency>" + Environment.NewLine +
            "<notification-emails>" + Environment.NewLine +
            "<email-address>reports@gibraltarlabsdocs.com</email-address>" + Environment.NewLine +
            "</notification-emails>" + Environment.NewLine +
            "</notify-on-access>" + Environment.NewLine +
            "</delivery-options>" + Environment.NewLine +
            "</express-delivery>" + Environment.NewLine +
            "</fds>";

            //OUTLOOK
            strBody = strBody.Replace("\r\n", "<br />");
            string strSignature = ReadSignature();
            strBody = strBody + "<br /><br />" + strSignature;

            Outlook.Application oApp = new Outlook.Application();
            Outlook.NameSpace ns = oApp.GetNamespace("MAPI");//BDS Testing
            ns.Logon();//BDS Testing


            // Create a new mail item.
            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            oMsg.BodyFormat = Outlook.OlBodyFormat.olFormatPlain; 
            oMsg.Attachments.Add(cPath + cFile);
            //oMsg.Attachments.Add(@"\\GBLNJ4\GIS\Reports\" + cFile);
            //Subject line
            oMsg.Subject = "REPORTS SENT TO BDS";
            //oMsg.CC = txtCC.Text;
            //oMsg.BCC = txtBCC.Text;
            oMsg.Body = strBody;//BDS Testing
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
            Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("bds@gibraltarlabsdocs.com");//BDS Testing
            oRecip.Resolve();

            //oMsg.Display();
            ////Send.
            ////oMsg.Send();
            ((Outlook._MailItem)oMsg).Send();

            ////Clean up.
            oRecip = null;//BDS Testing
            oRecips = null;
            oMsg = null;
            oApp = null;
            ns = null;
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

        private void lnkFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkFile.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void dgvFile_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dgvFile.CurrentCell != null)
            {
                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["WordReport"].Value.ToString() == "True")
                {
                    if (File.Exists(@"S:\Rpts\" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["YrCreated"].Value.ToString() + "\\R-" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RevNo"].Value.ToString() + ".doc"))
                        lnkFile.Text = @"S:\Rpts\" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["YrCreated"].Value.ToString() + "\\R-" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RevNo"].Value.ToString() + ".doc";
                    else if (File.Exists(@"S:\Rpts\" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["YrCreated"].Value.ToString() + "\\R-" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RevNo"].Value.ToString() + ".docx"))
                        lnkFile.Text = @"S:\Rpts\" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["YrCreated"].Value.ToString() + "\\R-" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RevNo"].Value.ToString() + ".docx";
                }
                else
                {
                    if (File.Exists(@"\\gblnj4\GIS\Reports\E_" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RevNo"].Value.ToString() + ".pdf"))
                        lnkFile.Text = @"\\gblnj4\GIS\Reports\E_" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RptNo"].Value.ToString() + ".R" + dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RevNo"].Value.ToString() + ".pdf";
                }
            }
        }
    }
}
