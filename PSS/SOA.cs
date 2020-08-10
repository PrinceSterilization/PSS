using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace PSS
{
    public partial class SOA: Form
    {
        DataTable dtSponsors = new DataTable();

        public SOA()
        {
            InitializeComponent();
        }

        public void StandardDGVSetting(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSponsors;
            dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
            dvwSetUp(dgvSponsors, dvwSponsors);
        }

        public void dvwSetUp(DataGridView dgvObj, DataView dvw)
        {
            dgvObj.Columns[0].Width = 369;
            dgvObj.Columns[1].Visible = false;
            dgvObj.DataSource = dvw;
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            dgvSponsors.Visible = true; dgvSponsors.BringToFront();
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            dgvSponsors.Visible = false;
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            if (pnlEMail.Enabled == false)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront();
            }
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void btnEMail_Click(object sender, EventArgs e)
        {
            if (txtSponsor.Text.Trim() == "")
            {
                MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            AcctgRpt rpt = new AcctgRpt();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "SOA";
            rpt.nQ = 3;
            rpt.nSpID = Convert.ToInt16(txtSponsorID.Text);
            try
            {
                rpt.Show();
            }
            catch { }
            rpt.Close(); rpt.Dispose();
            txtSponsorID.ReadOnly = true; txtSponsor.ReadOnly = true; picSponsors.Enabled = false;
            btnEMail.Enabled = false; btnPrtPreview.Enabled = false; btnClose.Enabled = false; pnlEMail.Enabled = true;
            lnkFile.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\SOA\" + DateTime.Now.Year.ToString() + @"\SOA-" + txtSponsorID.Text + ".pdf";

            //Get AP Email Address
            DataTable dt = PSSClass.Sponsors.APData(Convert.ToInt16(txtSponsorID.Text));
            txtTo.Text = dt.Rows[0]["APEMail"].ToString();
            dt.Dispose();

            txtSubject.Text = "Statement of Account";
            txtBody.Text = txtBody.Text + "Dear Accounts Payable," + Environment.NewLine + Environment.NewLine;
            txtBody.Text = txtBody.Text + "Attached is the updated Statement for your review." + Environment.NewLine + Environment.NewLine;
            txtBody.Text = txtBody.Text + "Please let us know if you need a copy of any of the invoices." + Environment.NewLine + Environment.NewLine;
            txtBody.Text = txtBody.Text + "If you have any questions/inquiries, please feel free to contact us." + Environment.NewLine + Environment.NewLine;
        }

        private void SOA_Load(object sender, EventArgs e)
        {
            dtSponsors = PSSClass.Sponsors.SponsorNamesDDL();
            dgvSponsors.DataSource = null;
            dgvSponsors.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
        }

        private void btnPrtPreview_Click(object sender, EventArgs e)
        {
            if (txtSponsor.Text.Trim() == "")
            {
                MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            AcctgRpt rpt = new AcctgRpt();
            rpt.WindowState = FormWindowState.Maximized;
            rpt.rptName = "SOA";
            //if (chkPDF.Checked == true)
            //    rpt.nQ = 3;
            //else
                rpt.nQ = 1;
            rpt.nSpID = Convert.ToInt16(txtSponsorID.Text);
            try
            {
                rpt.Show();
            }
            catch { }
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

        private void btnCancelEmail_Click(object sender, EventArgs e)
        {
            txtSponsorID.ReadOnly = false; txtSponsor.ReadOnly = false; picSponsors.Enabled = true;
            btnEMail.Enabled = true; btnPrtPreview.Enabled = true; btnClose.Enabled = true; pnlEMail.Enabled = false;
            txtTo.Text = ""; txtCC.Text = ""; txtSubject.Text = ""; txtBody.Text = ""; lnkFile.Text = "Statement of Account";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();  this.Dispose();
        }

        private void btnSendEMail_Click(object sender, EventArgs e)
        {
            string strBody = txtBody.Text;
            strBody = strBody.Replace("\r\n", "<br />");
            string strSignature = ReadSignature();
            strBody = strBody + "<br /><br />" + strSignature;

            //OUTLOOK
            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.
            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            oMsg.HTMLBody = "<FONT face=\"Arial\">";
            oMsg.HTMLBody += strBody.Trim();
            //Subject line
            oMsg.Subject = txtSubject.Text;
            oMsg.CC = txtCC.Text;
            //oMsg.BCC = txtBCC.Text;
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
            string[] EMAddresses = txtTo.Text.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < EMAddresses.Length; i++)
            {
                if (EMAddresses[i].Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                    oRecip.Resolve();
                }
            }
            ////Add an attachment.
            //for (int i = 0; i < lstAttachment.Items.Count; i++)
            //{
            //    //strFile = Path.GetFileName(lstAttachment.Items[i].ToString());
            //    oMsg.Attachments.Add(lstAttachment.Items[i].ToString());
            //}
            oMsg.Attachments.Add(lnkFile.Text);
            oMsg.Display();
            ////Send.
            ////oMsg.Send();
            ////((Outlook._MailItem)oMsg).Send();

            //Clean up.
            oRecips = null;
            oMsg = null;
            oApp = null;
            txtSponsorID.ReadOnly = false; txtSponsor.ReadOnly = false; picSponsors.Enabled = true;
            btnEMail.Enabled = true; btnPrtPreview.Enabled = true; btnClose.Enabled = true; pnlEMail.Enabled = false;
            txtTo.Text = ""; txtCC.Text = ""; txtSubject.Text = ""; txtBody.Text = ""; lnkFile.Text = "Statement of Account";
        }

        //private void SendToBDS()
        //{
        //    string strBody =
        //    "<fds version=" + (char)34 + "1.0" + (char)34 + ">" + Environment.NewLine +
        //    "<requester>" + Environment.NewLine +
        //    "<username>adelacruz@gibraltarlabsinc.com</username>" + Environment.NewLine +
        //    "<confirmation-options>" + Environment.NewLine +
        //    "<notification-emails>" + Environment.NewLine +
        //    "<email-address>adelacruz@gibraltarlabsinc.com</email-address>" + Environment.NewLine +
        //    "</notification-emails>" + Environment.NewLine +
        //    "</confirmation-options>" + Environment.NewLine +
        //    "</requester>" + Environment.NewLine +
        //    "<express-delivery>" + Environment.NewLine +
        //    "<recipients>" + Environment.NewLine +
        //    "<to>" + Environment.NewLine +
        //    "<email-address>mvenanzi@gibraltarlabsinc.com</email-address>" + Environment.NewLine +
        //    "</to>" + Environment.NewLine +
        //    "<cc>" + Environment.NewLine +
        //    "<email-address></email-address>" + Environment.NewLine +
        //    "</cc>" + Environment.NewLine +
        //    "<bcc>" + Environment.NewLine +
        //    "<email-address></email-address>" + Environment.NewLine +
        //    "</bcc>" + Environment.NewLine +
        //    "</recipients>" + Environment.NewLine +
        //    "<subject>TEST REPORT</subject>" + Environment.NewLine +
        //    "<secure-message use-default=" + (char)34 + "N" + (char)34 + ">There is a secured message waiting for you</secure-message>" + Environment.NewLine +
        //    "<files>" + Environment.NewLine +
        //    "<file><filename>" + "SOA-130.pdf" + "</filename>" + Environment.NewLine +
        //    "<description>File Attached</description>" + Environment.NewLine +
        //    "</file>" + Environment.NewLine +
        //    "</files>" + Environment.NewLine +
        //    "<notification-message use-default=" + (char)34 + "N" + (char)34 + ">Please click on the Sender Link below to Register for the Gibraltar Laboratories Report Server.</notification-message>" + Environment.NewLine +
        //    "<delivery-options>" + Environment.NewLine +
        //    "<date-available>" + DateTime.Now.ToShortDateString() + "</date-available>" + Environment.NewLine +
        //    "<date-expires>" + DateTime.Now.AddDays(6).ToShortDateString() + "</date-expires>" + Environment.NewLine +
        //    "<notify-recipients>Y</notify-recipients>" + Environment.NewLine +
        //    "<require-sign-in>Y</require-sign-in>" + Environment.NewLine +
        //    "<notify-on-access>" + Environment.NewLine +
        //    "<notification-frequency>FT</notification-frequency>" + Environment.NewLine +
        //    "<notification-emails>" + Environment.NewLine +
        //    "<email-address>reports@gibraltarlabsdocs.com</email-address>" + Environment.NewLine +
        //    "</notification-emails>" + Environment.NewLine +
        //    "</notify-on-access>" + Environment.NewLine +
        //    "</delivery-options>" + Environment.NewLine +
        //    "</express-delivery>" + Environment.NewLine +
        //    "</fds>";

        //    //MailMessage mailmsg = new MailMessage();
        //    //SmtpClient client = new SmtpClient();
        //    //Attachment att = new Attachment(lnkFile.Text);
        //    //client.Port = 25;
        //    //client.Host = "download.gibraltarlabsinc.com";
        //    ////client.Timeout = 10000;
        //    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    //client.UseDefaultCredentials = false;
        //    //client.Credentials = new System.Net.NetworkCredential("bds@gibraltarlabsdocs.com","Pass122");
        //    //mailmsg.From = new MailAddress("adelacruz@gibraltarlabsinc.com");
        //    //mailmsg.To.Add(new MailAddress("mvenanzi@gibraltarlabsinc.com"));
        //    //mailmsg.Subject = "Test E-Mail";
        //    //mailmsg.IsBodyHtml = false;
        //    //mailmsg.Body = strBody;
        //    //mailmsg.Attachments.Add(att);
        //    //client.Send(mailmsg);

        //    ////OUTLOOK
        //    strBody = strBody.Replace("\r\n", "<br />");
        //    string strSignature = ReadSignature();
        //    strBody = strBody + "<br /><br />" + strSignature;

        //    Outlook.Application oApp = new Outlook.Application();
        //    Outlook.NameSpace ns = oApp.GetNamespace("MAPI");//BDS Testing
        //    ns.Logon();//BDS Testing

        //    // Create a new mail item.
        //    Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
        //    oMsg.BodyFormat = Outlook.OlBodyFormat.olFormatPlain; //BDS Testing
        //    //oMsg.HTMLBody = "<FONT face=\"Arial\">";
        //    //oMsg.HTMLBody += strBody.Trim();

        //    //Add an attachment.
        //    //for (int i = 0; i < lstAttachment.Items.Count; i++)
        //    //{
        //    //    //strFile = Path.GetFileName(lstAttachment.Items[i].ToString());
        //    //    oMsg.Attachments.Add(lstAttachment.Items[i].ToString());
        //    //}
        //    oMsg.Attachments.Add(lnkFile.Text);
        //    //Subject line
        //    oMsg.Subject = txtSubject.Text;
        //    oMsg.CC = txtCC.Text;
        //    //oMsg.BCC = txtBCC.Text;
        //    oMsg.Body = strBody;//BDS Testing
        //    // Add a recipient.
        //    Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
        //    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("bds@gibraltarlabsdocs.com");//BDS Testing
        //    oRecip.Resolve();
        //    //string[] EMAddresses = txtTo.Text.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //    //for (int i = 0; i < EMAddresses.Length; i++)
        //    //{
        //    //    if (EMAddresses[i].Trim() != "")
        //    //    {
        //    //        Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
        //    //        oRecip.Resolve();
        //    //    }
        //    //}
        //    oMsg.Display();
        //    //////Send.
        //    //////oMsg.Send();
        //    //////((Outlook._MailItem)oMsg).Send();

        //    ////Clean up.
        //    oRecip = null;//BDS Testing
        //    oRecips = null;
        //    oMsg = null;
        //    oApp = null;
        //}

        private void PrintMenuItems(object menuItem, System.IO.StreamWriter sw)
        {
            if (menuItem as Office.CommandBarButton == null)
            {
                // This is a menu bar popup control.
                sw.WriteLine((menuItem as Office.CommandBarPopup).Caption +
                    "t" + (menuItem as Office.CommandBarPopup).Id.ToString());
                if ((menuItem as Office.CommandBarPopup).accChildCount > 0)
                {
                    for (int j = 1; j <= (menuItem as Office.CommandBarPopup).accChildCount; j++)
                    {
                        PrintMenuItems((menuItem as Office.CommandBarPopup).get_accChild(j), sw);
                    }
                }
            }
            else
            {
                // Must be a command
                if ((menuItem as Office.CommandBarControl).Caption == "New &Mail Message Usingt31146")
                {
                    sw.WriteLine("t" + (menuItem as Office.CommandBarControl).Caption +
                                        "t" + (menuItem as Office.CommandBarControl).Id.ToString() + "t" + (menuItem as Office.CommandBarControl).accName + "t" + (menuItem as Office.CommandBarControl).Creator);
                }
                else
                sw.WriteLine("t" + (menuItem as Office.CommandBarControl).Caption +
                    "t" + (menuItem as Office.CommandBarControl).Id.ToString() + "t" + (menuItem as Office.CommandBarControl).Type.ToString() + "t" + (menuItem as Office.CommandBarControl).Tag);
            }
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

        private void txtSponsorID_Leave(object sender, EventArgs e)
        {
            if (txtSponsorID.Text.Trim() != "")
            {
                txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
        }
    }
}
