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
    public partial class IngredionInvEntries : Form
    {
        private DataTable dtInvEntries = new DataTable();
        private DataTable dtSponsors = new DataTable();
        private DataTable dtSorted = new DataTable();
        private byte nValid = 0;

        public IngredionInvEntries()
        {
            InitializeComponent();
        }

        private void IngredionInvTemp_Load(object sender, EventArgs e)
        {
            LoadRecords();
            LoadSponsorsDDL();
        }

        private void LoadRecords()
        {
            dtInvEntries = GISClass.Ingredion.InvoiceEntries();
            bsFile.DataSource = dtInvEntries;
            bsFile.Filter = "SponsorID<>0";
            dgvFile.DataSource = bsFile;
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["TempID"].HeaderText = "TEMP ID";
            dgvFile.Columns["TempID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateCreated"].HeaderText = "DATE";
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateCreated"].Width = 100;
            dgvFile.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvFile.Columns["SponsorID"].Width = 80;
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["SponsorName"].Width = 390;
            dgvFile.Columns["Contact"].HeaderText = "CONTACT NAME";
            dgvFile.Columns["Contact"].Width = 250;
            dgvFile.Columns["GBLNo"].HeaderText = "GBL NO.";
            dgvFile.Columns["GBLNo"].Width = 100;
            dgvFile.Columns["GBLNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ReportNo"].HeaderText = "REPORT NO.";
            dgvFile.Columns["ReportNo"].Width = 100;
            dgvFile.Columns["ReportNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["Reviewed"].HeaderText = "REVIEWED";
            //dgvFile.Sort(dgvFile.Columns["SponsorID"], ListSortDirection.Ascending);
        }

        private void LoadSponsorsDDL()
        {
            dtSponsors = GISClass.Ingredion.SponsorNames();
            dgvSponsors.DataSource = null;
            dgvSponsors.DataSource = dtSponsors;
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
            dgvSponsors.EnableHeadersVisualStyles = false;
            dgvSponsors.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSponsors.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvSponsors.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSponsors.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvSponsors.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvSponsors.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (dtInvEntries.Rows.Count > 0)
            {
                //btnValSpLotNo_Click(null, null);
                //if (nValid == 0)
                //    return;

                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("You are about to create invoices." + Environment.NewLine + "Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
                DataTable dt = dtInvEntries.DefaultView.ToTable(true, "SponsorID", "Contact");
                DataView dv = dt.DefaultView;
                dv.Sort = "SponsorID, Contact";
                dtSorted = dv.ToTable();

                DataView dvw = dtInvEntries.DefaultView;
                dvw.Sort = "SponsorID ASC, Contact ASC";
                dgvFile.DataSource = dvw; //rebind the data source

                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();

                int nSpID = 0; string strContact = "", strInvNo = "0";
                for (int i = 0; i < dtSorted.Rows.Count; i++)
                {
                    nSpID = Convert.ToInt16(dtSorted.Rows[i]["SponsorID"]);
                    strContact = dtSorted.Rows[i]["Contact"].ToString();
                    int nI = 0;// int nGo = 1;

                    //if (nSpID == 1745 && (DateTime.Now.Day == 13 || DateTime.Now.Day >= 28))
                    //    nGo = 1;
                    //else if (nSpID != 1745)
                    //    nGo = 1;

                    //if (nGo == 1)
                    //{
                        for (int j = 0; j < dgvFile.Rows.Count; j++)
                        {
                            if (nSpID == Convert.ToInt32(dgvFile.Rows[j].Cells["SponsorID"].Value) && strContact == dgvFile.Rows[j].Cells["Contact"].Value.ToString())
                            {
                                //DateTime dte = Convert.ToDateTime(dgvFile.Rows[j].Cells["DateCreated"].Value.ToString());
                                //int nYr = dte.Year;
                                //if (nYr == 2015)
                                //{
                                    if (nI == 0)
                                    {
                                        nI = 1;
                                        strInvNo = GISClass.General.NewID("Invoices", "InvoiceNo").ToString();

                                        sqlcmd = new SqlCommand();
                                        sqlcmd.Connection = sqlcnn;
                                        sqlcmd.Parameters.AddWithValue("@TID", Convert.ToInt16(dgvFile.Rows[j].Cells["TempID"].Value));
                                        sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(strInvNo));
                                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                                        sqlcmd.CommandType = CommandType.StoredProcedure;
                                        sqlcmd.CommandText = "spIngredionMasterInv";

                                        try
                                        {
                                            sqlcmd.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            break;
                                        }
                                        sqlcmd.Dispose();
                                    }
                                    sqlcmd = new SqlCommand();
                                    sqlcmd.Connection = sqlcnn;
                                    sqlcmd.Parameters.AddWithValue("@TID", Convert.ToInt32(dgvFile.Rows[j].Cells[0].Value));
                                    sqlcmd.Parameters.AddWithValue("@InvNo", Convert.ToInt32(strInvNo));
                                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                                    sqlcmd.CommandType = CommandType.StoredProcedure;
                                    sqlcmd.CommandText = "spIngredionDetailsInv";

                                    try
                                    {
                                        sqlcmd.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        break;
                                    }
                                    sqlcmd.Dispose();
                                //}
                            }
                            else if (nI == 1)
                            {
                                SendInvoice(Convert.ToInt32(strInvNo), nSpID);
                                break;
                            }
                            ////MessageBox.Show("Invoice successfully created.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    //}
                }
                //CONVERTS SPECIAL BATCH INVOICES AMOUNT TO ZERO - AMDC 05/27/2016
                DataTable dtX = GISClass.Ingredion.InvSpecialBatches();
                if (dtX != null && dtX.Rows.Count > 0)
                {
                    for (int i = 0; i < dtX.Rows.Count; i++)
                    {
                        GISClass.Ingredion.InvSpclBatchZero(Convert.ToInt32(dtX.Rows[i]["InvoiceNo"]), Convert.ToInt32(dtX.Rows[i]["GBLNo"]), DateTime.Now.ToString("MM/dd/yyyy")); //DateTime.Now.ToShortDateString()
                    }
                }
                sqlcnn.Close(); sqlcnn.Dispose();
                LoadRecords();
            }
        }

        private void SendInvoice(int cInv, int cSpID)
        {
            AcctgRpt rptInvoice = new AcctgRpt();
            rptInvoice.WindowState = FormWindowState.Maximized;
            rptInvoice.nQ = 3;
            rptInvoice.rptName = "InvoiceIngredion";
            try
            {
                rptInvoice.nInvNo = cInv;
                rptInvoice.Show();
            }
            catch { }
            rptInvoice.Close(); rptInvoice.Dispose();

            lstAttachment.Items.Clear();
            //lstAttachment.Items.Add(Application.StartupPath + @"\\Reports\" + "EI-" +  cInv.ToString() + ".pdf");
            lstAttachment.Items.Add(@"\\gblnj4\GIS\Reports\" + "EI-" + cInv.ToString() + ".pdf");

            DataTable dt = new DataTable();
            dt = GISClass.Sponsors.APData(cSpID);
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No A/P contact data found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string strAP = dt.Rows[0]["APContact"].ToString();
            string strTo = dt.Rows[0]["APEMail"].ToString();
            string strSubject = "Invoice No. " + cInv.ToString();
            string strBCC = "ar@gibraltarlabsinc.com";// jmastej@gibraltarlabsinc.com"; //A/R Monitoring
            dt.Dispose();
            // Set HTMLBody. 
            //add the body of the email
            string strBody = "Dear " + strAP + ";" + Environment.NewLine + Environment.NewLine +
                      "We appreciate your business with us!" + Environment.NewLine + Environment.NewLine +
                      "The attached invoice is being submitted for payment processing." + Environment.NewLine + Environment.NewLine +
                      "Also attached is the updated Statement of Account for your review." + Environment.NewLine +
                      "Please request for a copy of any missing invoices." + Environment.NewLine + Environment.NewLine + 
                      "Should you have any questions or clarifications, please do not" + Environment.NewLine + 
                      "hesitate to contact me." + Environment.NewLine + Environment.NewLine +
                      "Thank you for your continued support!";


            strBody = strBody.Replace("\r\n", "<br />");
            string strSignature = ReadSignature();
            strBody = strBody + "<br /><br />" + strSignature;

            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.

            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
            oMsg.HTMLBody = "<FONT face=\"Arial\">";
            oMsg.HTMLBody += strBody.Trim();
            //Add an attachment.
            for (int i = 0; i < lstAttachment.Items.Count; i++)
            {
                //strFile = Path.GetFileName(lstAttachment.Items[i].ToString());
                oMsg.Attachments.Add(lstAttachment.Items[i].ToString());
            }
            //Subject line
            oMsg.Subject = strSubject;
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
            //Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(txtTo.Text); // "adelacruz@gibraltarlabsinc.com"

            string[] EMAddresses = strTo.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < EMAddresses.Length; i++)
            {
                if (EMAddresses[i].Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                    oRecip.Resolve();
                }
            }
            //oMsg.CC = txtCC.Text;
            oMsg.BCC = strBCC;

            //oRecip.Resolve();
            oMsg.Display();

            //Send.
            //((Outlook._MailItem)oMsg).Send();

            // Clean up.
            //oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;

            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@InvNo", cInv);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdInvEDate";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
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

        private void btnFilter_Click(object sender, EventArgs e)
        {
            if (txtSponsorID.Text.Trim() != "")
            {
                try
                {
                    bsFile.Filter = "SponsorID" + "=" + txtSponsorID.Text;
                    dgvFile.Visible = true; dgvFile.BringToFront();
                }
                catch { }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            AcctgRpt rptDraftInvoice = new AcctgRpt();
            rptDraftInvoice.WindowState = FormWindowState.Maximized;
            rptDraftInvoice.nQ = 1;
            rptDraftInvoice.rptName = "DraftInvoiceMfst";
            try
            {
                rptDraftInvoice.nInvNo = Convert.ToInt32(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells[0].Value);
                rptDraftInvoice.Show();
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.DayOfWeek.ToString() == "Friday" && DateTime.Now.TimeOfDay.Hours == 17)
            {
                timer1.Enabled = false;
                //MessageBox.Show("The system is now ready to generate final invoices.");
                btnCreate_Click(null, null);
            }
        }

        private void dgvFile_DoubleClick(object sender, EventArgs e)
        {
            if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "GBLNo" && dgvFile.CurrentCell.Value.ToString() != "")
            {
                int intOpen = GISClass.General.OpenForm(typeof(SamplesLogin));

                if (intOpen == 1)
                {
                    GISClass.General.CloseForm(typeof(SamplesLogin));
                }
                IngredionManifestLog childForm = new IngredionManifestLog();
                childForm.Text = "INGREDION SAMPLES LOGIN";
                childForm.MdiParent = this.MdiParent;
                childForm.nLogNo = Convert.ToInt32(dgvFile.CurrentCell.Value.ToString());
                childForm.nFR = 1;
                childForm.Show();
            }
        }

        private void dgvFile_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvFile.CurrentCell.OwningColumn.Name.ToString() != "Reviewed")
            {
                e.Cancel = true;
            }
        }

        private void dgvFile_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvFile.IsCurrentCellDirty)
            {
                dgvFile.CommitEdit(DataGridViewDataErrorContexts.Commit);
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@TempID", Convert.ToInt32(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["TempID"].Value));
                if (dgvFile.CurrentCell.Value.ToString() == "True")
                    sqlcmd.Parameters.AddWithValue("@DateReviewed", DateTime.Now);
                else
                    sqlcmd.Parameters.AddWithValue("@DateReviewed", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spIngredionInvUpdReview";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
        }

        private void btnValSpLotNo_Click(object sender, EventArgs e)
        {
            string strLNo = ""; int nCtr = 0; string strLNF = "";
            string strInvalid = ""; string strTID = ""; string strGBL = "";
            dgvReplace.RowCount = 0;

            for (int i = 0; i < dgvFile.Rows.Count; i++)
            {
                DataTable dt = GISClass.Ingredion.LotNo(Convert.ToInt32(dgvFile.Rows[i].Cells["GBLNo"].Value));
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["LotNo"].ToString().Length >= 3)
                        strLNo = dt.Rows[0]["LotNo"].ToString().Substring(0, 3);
                    else
                        strLNo = "0";
                    
                    strLNF = dt.Rows[0]["LotNo"].ToString();

                    bool bAllChars = strLNo.All(Char.IsLetter);
                    if (bAllChars == true && dgvFile.Rows[i].Cells["SponsorID"].Value.ToString() == "1745")
                    {
                        if (strLNF.IndexOf("-") == -1)
                        {
                            strInvalid = strInvalid + "GBL #" + dgvFile.Rows[i].Cells["GBLNo"].Value.ToString() + Environment.NewLine;
                            strTID = strTID + dgvFile.Rows[i].Cells["TempID"].Value.ToString() + ",";
                            strGBL = strGBL + dgvFile.Rows[i].Cells["GBLNo"].Value.ToString() + ",";
                            nCtr++;
                        }
                    }
                }
            }
            if (nCtr > 0)
            {
                nValid = 0;
                MessageBox.Show("Errors found for lot number entries.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataTable dtSp = GISClass.Ingredion.SponsorNames();
                cboSponsors.DataSource = dtSp;
                cboSponsors.DisplayMember = "SponsorName";
                cboSponsors.ValueMember = "SponsorID";

                cboSponsorID.DataSource = dtSp;
                cboSponsorID.DisplayMember = "SponsorID";
                cboSponsorID.ValueMember = "SponsorName";

                pnlReplace.Visible = true; pnlReplace.BringToFront(); dgvFile.Enabled = false; btnValSpLotNo.Enabled = false; btnPreview.Enabled = false;
                string[] strSplit = strTID.Split(',');
                string[] strGBLSplit = strGBL.Split(',');
                for (int i = 0; i < strSplit.Length; i++)
                {
                    if (strSplit[i] != "")
                    {
                        string[] row = new string[] { strSplit[i], strGBLSplit[i], "", "" };
                        dgvReplace.Rows.Add(row);
                    }
                }
            }
            else
            {
                nValid = 1; 
                MessageBox.Show("Validation process completed. No errors found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void cboSponsors_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtContacts = GISClass.Ingredion.Contacts(Convert.ToInt16(cboSponsors.SelectedValue));
                cboContacts.DataSource = dtContacts;
                cboContacts.DisplayMember = "Contact";
                cboContacts.ValueMember = "ContactID";
            }
            catch { }
        }

        private void btnUpdateCancel_Click(object sender, EventArgs e)
        {
            pnlReplace.Visible = false; dgvFile.Enabled = true; btnValSpLotNo.Enabled = true; btnPreview.Enabled = true;
        }

        private void dgvReplace_DoubleClick(object sender, EventArgs e)
        {
            if (dgvReplace.Rows.Count > 0 && dgvReplace.CurrentCell.OwningColumn.Name == "GBLNo" && dgvReplace.CurrentCell.Value.ToString() != "")
            {
                int intOpen = GISClass.General.OpenForm(typeof(SamplesLogin));

                if (intOpen == 1)
                {
                    GISClass.General.CloseForm(typeof(SamplesLogin));
                }
                IngredionManifestLog childForm = new IngredionManifestLog();
                childForm.Text = "INGREDION SAMPLES LOGIN";
                childForm.MdiParent = this.MdiParent;
                childForm.nLogNo = Convert.ToInt32(dgvReplace.CurrentCell.Value.ToString());
                childForm.nFR = 1;
                childForm.Show();
                return;
            }
            dgvReplace.Enabled = false; pnlUpdate.Visible = true; pnlUpdate.BringToFront(); btnUpdateOK.Enabled = false; btnUpdateCancel.Enabled = false;
            try
            {
                txtTempID.Text = dgvReplace.Rows[dgvReplace.CurrentCell.RowIndex].Cells["TempID"].Value.ToString();
                txtGBLNo.Text = dgvReplace.Rows[dgvReplace.CurrentCell.RowIndex].Cells["GBLNo"].Value.ToString();
                cboSponsors.Text = dgvReplace.Rows[dgvReplace.CurrentCell.RowIndex].Cells["SponsorName"].Value.ToString();
                cboContacts.Text = dgvReplace.Rows[dgvReplace.CurrentCell.RowIndex].Cells["Contact"].Value.ToString();
            }
            catch { }
        }

        private void btnReplaceCancel_Click(object sender, EventArgs e)
        {
            pnlUpdate.Visible = false; btnUpdateOK.Enabled = true; btnUpdateCancel.Enabled = true; dgvReplace.Enabled = true;
        }

        private void btnReplaceOK_Click(object sender, EventArgs e)
        {
            if (cboSponsorID.SelectedIndex == -1 || cboSponsors.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Sponsor.");
                return;
            }
            if (cboContacts.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Contact.");
                return;
            }
            dgvReplace.Rows[dgvReplace.CurrentCell.RowIndex].Cells["SponsorName"].Value = cboSponsors.Text;
            dgvReplace.Rows[dgvReplace.CurrentCell.RowIndex].Cells["SponsorID"].Value = cboSponsorID.Text;
            dgvReplace.Rows[dgvReplace.CurrentCell.RowIndex].Cells["Contact"].Value = cboContacts.Text;
            dgvReplace.Rows[dgvReplace.CurrentCell.RowIndex].Cells["ContactID"].Value = cboContacts.SelectedValue;
            pnlUpdate.Visible = false; btnUpdateOK.Enabled = true; btnUpdateCancel.Enabled = true; dgvReplace.Enabled = true;
        }

        private void cboSponsorID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboSponsors.SelectedValue = cboSponsorID.Text;
            }
            catch {}
        }

        private void btnUpdateOK_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd;
            byte nX = 0;
            for (int i = 0; i < dgvReplace.Rows.Count; i++)
            {
                if (dgvReplace.Rows[i].Cells["SponsorID"].Value.ToString() == "" || dgvReplace.Rows[i].Cells["ContactID"].Value.ToString() == "")
                {
                    MessageBox.Show("A row contains blank Sponsor or Contact." + Environment.NewLine + "Please complete the data beore proceeding.", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    nX = 1;
                    break;
                }
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@TempID", Convert.ToInt16(dgvReplace.Rows[i].Cells["TempID"].Value));
                sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt32(dgvReplace.Rows[i].Cells["SponsorID"].Value));
                sqlcmd.Parameters.AddWithValue("@ConID", Convert.ToInt32(dgvReplace.Rows[i].Cells["ContactID"].Value));
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spIngredionUpdSpContact";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlcmd.Dispose();
            }
            LoadRecords();
            pnlUpdate.Visible = false; btnUpdateOK.Enabled = true; btnUpdateCancel.Enabled = true; dgvReplace.Enabled = true;
            if (nX == 0)
                MessageBox.Show("Update completed.");
        }

        private void IngredionInvEntries_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
