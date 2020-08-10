using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace GIS
{
    public partial class TestDataIngredion : Form
    {
        public Int32 nLogNo;
        public int nServiceCode;
        public Int32 nRptNo;
        public int nSponsorID;
        public int nContactID;

        private DataTable dtTestData;


        public TestDataIngredion()
        {
            InitializeComponent();
        }

        private void TestDataIngredion_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

            txtGBLNo.Text = nLogNo.ToString();
            txtGBLNo.BackColor = Color.SteelBlue;
            txtGBLNo.ForeColor = Color.White;
            txtReportNo.Text = nRptNo.ToString();

            if (GISClass.FinalReports.RptDateMailed(Convert.ToInt32(txtReportNo.Text)).IndexOf("1900") > 0)
                txtDateEMailed.Text = "";
            else
                txtDateEMailed.Text = GISClass.FinalReports.RptDateMailed(Convert.ToInt32(txtReportNo.Text));

            dtTestData = GISClass.Samples.IngredionTestData(nLogNo);
            if (dtTestData == null || dtTestData.Rows.Count == 0)
            {
                dtTestData = GISClass.Samples.IngredionLogTest(nLogNo, LogIn.nUserID);
                foreach (DataRow row in dtTestData.Rows)
                {
                    row.SetAdded();
                }
            }

            dgvTestResults.DataSource = dtTestData;

            dgvTestResults.EnableHeadersVisualStyles = false;
            dgvTestResults.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTestResults.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvTestResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTestResults.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvTestResults.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvTestResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvTestResults.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvTestResults.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvTestResults.Columns["DateTested"].HeaderText = "DATE TESTED";
            dgvTestResults.Columns["TestResult"].HeaderText = "RESULT";
            dgvTestResults.Columns["Note"].HeaderText = "NOTE";
            dgvTestResults.Columns["AnalystID"].HeaderText = "ANALYST ID";
            dgvTestResults.Columns["Analyst"].HeaderText = "ANALYST";
            dgvTestResults.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTestResults.Columns["ServiceCode"].Width = 80;
            dgvTestResults.Columns["ServiceDesc"].Width = 250;
            dgvTestResults.Columns["DateTested"].Width = 80;
            dgvTestResults.Columns["DateTested"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTestResults.Columns["DateTested"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvTestResults.Columns["TestResult"].Width = 80;
            dgvTestResults.Columns["Note"].Width = 150;
            dgvTestResults.Columns["AnalystID"].Width = 80;
            dgvTestResults.Columns["AnalystID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTestResults.Columns["Analyst"].Width = 100;

            //for (int i = 0; i < dgvTestResults.Rows.Count; i++)
            //{
            //    if (dgvTestResults.Rows[i].Cells["AnalystID"].Value.ToString() == "")
            //    {
            //        dgvTestResults.Rows[i].Cells["AnalystID"].Value = LogIn.nUserID;
            //        dgvTestResults.Rows[i].Cells["Analyst"].Value = LogIn.strUserID;
            //    }
            //    if (dgvTestResults.Rows[i].Cells["DateTested"].Value.ToString() == "")
            //    {
            //        dgvTestResults.Rows[i].Cells["DateTested"].Value = DateTime.Now.ToShortDateString();
            //    }
            //}
        }

        private void dgvTestResults_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (txtDateEMailed.Text != "")
                e.Cancel = true;

            if (dgvTestResults.CurrentCell.OwningColumn.Name.ToString() == "ServiceCode" || dgvTestResults.CurrentCell.OwningColumn.Name.ToString() == "ServiceDesc" ||
                dgvTestResults.CurrentCell.OwningColumn.Name.ToString() == "AnalystID" || dgvTestResults.CurrentCell.OwningColumn.Name.ToString() == "Analyst")
                e.Cancel = true;
        }

        private void dgvTestResults_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
            //{
            //    dgvTestResults.Rows[e.RowIndex].ErrorText = "Cell value must not be empty!";
            //    e.Cancel = true;
            //}

            //try
            //{
            //    DateTime.Parse(dgvTestResults.Rows[e.RowIndex].Cells["DateTested"].Value.ToString());
            //}
            //catch
            //{
            //    dgvTestResults.Rows[e.RowIndex].ErrorText = "Invalid format";
            //    e.Cancel = true;
            //}
        }

        private void dgvTestResults_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvTestResults.IsCurrentCellDirty)
            {
                dgvTestResults.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvTestResults_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            {
                //object value = dgvSamples.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                //if (!((DataGridViewComboBoxColumn)dgvSamples.Columns[e.ColumnIndex]).Items.Contains(value))
                //{
                //    ((DataGridViewComboBoxColumn)dgvSamples.Columns[e.ColumnIndex]).Items.Add(value);
                //    e.ThrowException = false;
                //}
                e.ThrowException = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();

            DataTable dt = new DataTable();
            dt = dtTestData.GetChanges();
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    if (dtTestData.Rows[i].RowState.ToString() == "Added")
                        sqlcmd.Parameters.AddWithValue("@nMode", 1);
                    else
                        sqlcmd.Parameters.AddWithValue("@nMode", 2);
                    sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                    sqlcmd.Parameters.AddWithValue("@SC", dt.Rows[i]["ServiceCode"].ToString());
                    sqlcmd.Parameters.AddWithValue("@DateTested", dt.Rows[i]["DateTested"]);
                    sqlcmd.Parameters.AddWithValue("@TestResult", dt.Rows[i]["TestResult"]);
                    sqlcmd.Parameters.AddWithValue("@Note", dt.Rows[i]["Note"]);
                    sqlcmd.Parameters.AddWithValue("@AnalystID", dt.Rows[i]["AnalystID"]);
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spAddEditIngredionData";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                    sqlcmd.Dispose();
                }
            }
            sqlcmd.Dispose(); sqlcnn.Dispose();
            this.Dispose();
        }

        private void btnEMail_Click(object sender, EventArgs e)
        {
            if (txtDateEMailed.Text == "")
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                SqlCommand sqlcmd = new SqlCommand();

                //Save Data 
                //bsMain.EndEdit();
                DataTable dt = new DataTable();
                dt = dtTestData.GetChanges();
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        if (dtTestData.Rows[i].RowState.ToString() == "Added")
                            sqlcmd.Parameters.AddWithValue("@nMode", 1);
                        else
                            sqlcmd.Parameters.AddWithValue("@nMode", 2);
                        sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                        sqlcmd.Parameters.AddWithValue("@SC", dt.Rows[i]["ServiceCode"].ToString());
                        sqlcmd.Parameters.AddWithValue("@DateTested", dt.Rows[i]["DateTested"]);
                        sqlcmd.Parameters.AddWithValue("@TestResult", dt.Rows[i]["TestResult"]);
                        sqlcmd.Parameters.AddWithValue("@Note", dt.Rows[i]["Note"]);
                        sqlcmd.Parameters.AddWithValue("@AnalystID", dt.Rows[i]["AnalystID"]);
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spAddEditIngredionData";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch
                        {
                        }
                        sqlcmd.Dispose();
                    }
                }
                sqlcmd.Dispose(); sqlcnn.Dispose();
            }
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to send this to the Sponsor?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
            if (txtReportNo.Text == "" || txtReportNo.Text == "0" ||txtDateEMailed.Text == "") // Revised 5/4/2016
            {
                //Create a New Report
                if (nRptNo == 0)
                {
                    DialogResult dAns = new DialogResult();
                    dAns = MessageBox.Show("A new report would be created." + Environment.NewLine + "Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dAns == DialogResult.No)
                    {
                        return;
                    }
                    pnlTestSched.Visible = true;
                    return;
                }
                if (File.Exists(@"\\GBLNJ4\GIS\Reports\IngredionRpt.rpt") == false)
                {
                    MessageBox.Show("Report file is under construction." + Environment.NewLine + "Please contact the IT Department for updates.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                LabRpt rpt = new LabRpt();
                rpt.rptName = "FinalRptIngredion";
                rpt.rptFile = "IngredionRpt.rpt";
                rpt.WindowState = FormWindowState.Maximized;
                rpt.nLogNo = Convert.ToInt32(txtGBLNo.Text);
                rpt.nRptNo = Convert.ToInt32(txtReportNo.Text);
                rpt.SpID = nSponsorID;
                rpt.nRevNo = 0;
                try
                {
                    rpt.Show();
                }
                catch { }
                rpt.WindowState = FormWindowState.Minimized;
            }
            SendMail();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dtpStartDate.Value > dtpEndDate.Value)
            {
                MessageBox.Show("Invalid start date and end date range.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            pnlTestSched.Visible = false;
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();
            
            //FinalReport Master Record
            txtReportNo.Text = GISClass.General.NewID("FinalRptMaster", "ReportNo").ToString();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", 1);
            sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtReportNo.Text));
            sqlcmd.Parameters.AddWithValue("@RptNotes", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@IntNotes", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@Method", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@Conclusion", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@Purpose", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@Results", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@Memo", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@SpID", nSponsorID);
            sqlcmd.Parameters.AddWithValue("@ConID", nContactID );
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditFinRptMstr";
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
            sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            //Final Report Revision
            sqlcmd.Parameters.AddWithValue("@nMode", 1);
            sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtReportNo.Text));
            sqlcmd.Parameters.AddWithValue("@RevNo", 0);
            sqlcmd.Parameters.AddWithValue("@RevDate", DateTime.Now);
            sqlcmd.Parameters.AddWithValue("@StudyDirID", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@ReasonCode", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@Reason", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditfinRptRev";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch
            {
            }
            sqlcmd.Dispose();
            //Final Report GBLs
            for (int i = 0; i < dgvTestResults.Rows.Count; i++)
            {
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", 1);
                sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtReportNo.Text));
                sqlcmd.Parameters.AddWithValue("@RevNo", 0);
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dgvTestResults.Rows[i].Cells["ServiceCode"].Value));
                sqlcmd.Parameters.AddWithValue("@FormatNo", 1);
                sqlcmd.Parameters.AddWithValue("@DteOn", dtpStartDate.Value);
                sqlcmd.Parameters.AddWithValue("@DteOff", dtpEndDate.Value);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditFinRptLogs";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch
                {
                }
                sqlcmd.Dispose();
                //Update Log Test's ReportNo field
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtReportNo.Text));
                sqlcmd.Parameters.AddWithValue("@RevNo", 0);
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(dgvTestResults.Rows[i].Cells["ServiceCode"].Value));
                sqlcmd.Parameters.AddWithValue("@FormatNo", 1);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdLogTestRptNo";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlcmd.Dispose();
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();

            if (File.Exists(@"\\GBLNJ4\GIS\Reports\IngredionRpt.rpt") == false)
            {
                MessageBox.Show("Report file is under construction." + Environment.NewLine + "Please contact the IT Department for updates.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LabRpt rpt = new LabRpt();
            rpt.rptName = "FinalRptIngredion";
            rpt.rptFile = "IngredionRpt.rpt";
            rpt.WindowState = FormWindowState.Maximized;
            rpt.nLogNo = Convert.ToInt32(txtGBLNo.Text);
            rpt.nRptNo = Convert.ToInt32(txtReportNo.Text);
            rpt.SpID = nSponsorID;
            rpt.nRevNo = 0;
            try
            {
                rpt.Show();
            }
            catch { }
            SendMail();
        }

        private void SendMail()
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            string strEMail = "";

            sqlcmd = new SqlCommand("SELECT EMailAddress FROM ContactEMAddresses WHERE ContactID = " +  nContactID +
                                    " AND FinalReports = 1", sqlcnn);
            sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                strEMail = sqldr.GetValue(0).ToString();
            }
            sqldr.Close(); sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();

            txtTo.Text = strEMail.Replace(";", "; ");

            dgvTestResults.Enabled = false; btnClose.Enabled = false; btnEMail.Enabled = false;
            pnlEMail.Visible = true; pnlEMail.BringToFront(); //pnlEMail.Location = new Point(50, 50);

            txtSubject.Text = "R-" + nRptNo.ToString() + "-R0" + " Lot: " + GISClass.Samples.LotNo(nLogNo) + " " + GISClass.Samples.ArticleDesc(nLogNo);

            txtBody.Text = "Dear " + GISClass.Contacts.ConFirstName(nContactID, nSponsorID) + ", " + Environment.NewLine + Environment.NewLine +
                       "Thank you for your support of Gibraltar Laboratories. " + Environment.NewLine + "Please find attached our Final Report on the samples of " + GISClass.Samples.ArticleDesc(nLogNo) + "." +
                       Environment.NewLine + Environment.NewLine;
                       //+ "<i>Gibraltar Laboratories is pleased to announce the availability of our new Report Server. This advanced technology will " + Environment.NewLine + "make <u>your job easier</u>.  " +
                       //"Specifically you will have real-time access to up to 6 months of your e-reports. Perfect for when you" + Environment.NewLine + "cannot locate a report. You will be able to login to the protected " +
                       //"server to retrieve your results without having to send an" + Environment.NewLine + "e-mail or pick up the phone.</i>" + Environment.NewLine + Environment.NewLine +
                       //"If you are interested, Please contact with " + "<a href=" + "mailto:kkohan@gibraltarlabsinc.com " + ">Kristah Kohan</a> for further details. " + Environment.NewLine + Environment.NewLine +
                       //"To see your most current report, as well as a historical listing of previously issued reports, please click on the " + Environment.NewLine +
                       //"<a href=" + "http://www.gibraltarlabsinc.com" + ">Gibraltar Laboratories Homepage</a> and select Login." + Environment.NewLine + Environment.NewLine;

            lnkReport.Text = @"\\GBLNJ4\GIS\Reports\" + "E_" + txtReportNo.Text + ".R0" + ".pdf";
            //lnkReport_LinkClicked(null, null);
        }

        private void lnkReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkReport.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnCloseEMail_Click(object sender, EventArgs e)
        {
            pnlEMail.Visible = false; dgvTestResults.Enabled = true; btnClose.Enabled = true; btnEMail.Enabled = true;
        }

        private void btnSendEMail_Click(object sender, EventArgs e)
        {
            string strBody = "";

            strBody = txtBody.Text.Replace("\r\n", "<br />");
            string strSignature = ReadSignature();
            strBody = strBody + "<br /><br />" + strSignature;

            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.

            Outlook._MailItem oMsg = (Outlook._MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            oMsg.HTMLBody = "<FONT face=\"Arial\">";
            oMsg.HTMLBody += strBody;
            //Add an attachment.
            oMsg.Attachments.Add(lnkReport.Text);
            //Subject line
            oMsg.Subject = txtSubject.Text;
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

            oMsg.CC = txtCC.Text;

            //oMsg.Display();

            //Send.
            //oMsg.Send(); //error here
            //((Outlook._MailItem)oMsg).Send(); -- this works
            oMsg.Display();

            // Clean up.
            //oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;

            if (txtDateEMailed.Text == "")
            {
                //UPDATE EMAIL DATE
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                sqlcmd = new SqlCommand("UPDATE FinalRptRev SET DateEMailed = GetDate(), EMailedByID=" + LogIn.nUserID + " " +
                                        "WHERE ReportNo=" + nRptNo.ToString() + " AND RevisionNo=0", sqlcnn);
                sqlcmd.ExecuteNonQuery();
                sqlcmd.Dispose();
                sqlcnn.Close(); sqlcnn.Dispose();
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
    }
}
