using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Net.Mail;
using System.Net;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;

namespace PSS
{
    public partial class QuoteFollowUp : PSS.TemplateForm
    {
        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;

        DataTable dtEqptDetail = new DataTable();                                         // MY 02/11/2015 - GridView Equipment Detail table


        public QuoteFollowUp()
        {
            InitializeComponent();
            tsddbSearch.Enabled = false;
            cboCutOffDays.Text = Convert.ToString(7);
            LoadRecords(Convert.ToInt16(cboCutOffDays.Text));                             // Load all records within a 7 daytime frame; default
           
            BuildPrintItems();
            BuildSearchItems();

            bnMoveFirst.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveLast.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveNext.Click += new EventHandler(MoveRecordClickHandler);
            bnMovePrevious.Click += new EventHandler(MoveRecordClickHandler);

            tsbAdd.Click += new EventHandler(AddClickHandler);
            tsbEdit.Click += new EventHandler(EditClickHandler);
            tsbDelete.Click += new EventHandler(DeleteClickHandler);
            tsbSave.Click += new EventHandler(SaveClickHandler);
            tsbCancel.Click += new EventHandler(CancelClickHandler);
            tsbExit.Click += new EventHandler(CloseClickHandler);
            tsbSearch.Click += new EventHandler(SearchOKClickHandler);
            tsbFilter.Click += new EventHandler(SearchFilterClickHandler);
            tsbRefresh.Click += new EventHandler(RefreshClickHandler);
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords(int cDays)
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Quotations.QuoteFollowUp(cDays);                                            

            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }

            dt.Columns.Add("ChkBox", typeof(Boolean));
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvQuoteForFollowUp.DataSource = bsFile;
           
            DataGridSetting();
            txtTotalQuotes.Text = dgvQuoteForFollowUp.RowCount.ToString();
            tsbAdd.Enabled = false;
            tsbEdit.Enabled = false;
            tsbDelete.Enabled = false;
            tsbCancel.Enabled = false;
        }      
       
        private void CloseClickHandler(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            this.Close();
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "QuoteFollowUpList";
            items[0].Text = "Quote Follow-Up List";
            items[0].Click += new EventHandler(PrintQuoteFollowUpListClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Quotations.QuoteFollowUp(0);

            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered during build-up of search items." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            arrCol = new string[dt.Columns.Count];

            ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length];

            foreach (DataColumn colFile in dt.Columns)
            {
                items[i] = new ToolStripMenuItem();
                items[i].Name = colFile.ColumnName;

                //Using LINQ to insert space between capital letters
                var val = colFile.ColumnName;
                val = string.Concat(val.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

                items[i].Text = val;
                items[i].Click += new EventHandler(SearchItemClickHandler);
                arrCol[i] = colFile.DataType.ToString();
                cklColumns.Items.Add(val);
                i += 1;
            }
            for (int j = 0; j < cklColumns.Items.Count; j++)
            {
                cklColumns.SetItemChecked(j, true);
            }

            tsddbSearch.DropDownItems.AddRange(items);
            tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
        }       

        private void dgvCellMouseClickEventHandler(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                cklColumns.Visible = true; cklColumns.BringToFront();
            }
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData();
                btnClose.Visible = true;
            }
        }

        private void cklSelIdxChEventHandler(object sender, EventArgs e)
        {
            string strCol = cklColumns.Items[cklColumns.SelectedIndex].ToString().Replace(" ", "");
            if (dgvFile.Columns[strCol].Visible == true)
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = false;
            else
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = true;
            cklColumns.Visible = false;
        }

        private void AddClickHandler(object sender, EventArgs e)
        {
            AddRecord();
        }

        private void EditClickHandler(object sender, EventArgs e)
        {
            EditRecord();
        }

        private void DeleteClickHandler(object sender, EventArgs e)
        {
            DeleteRecord();
        }

        private void SaveClickHandler(object sender, EventArgs e)
        {
            SaveRecord();
        }       

        private void SearchItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            tstbSearchField.Text = clickedItem.Name;
            tstbSearch.SelectAll();
            tstbSearch.Focus();
            nIndex = tsddbSearch.DropDownItems.IndexOf(clickedItem);
            tslSearchData.Text = clickedItem.Text;
        }

        private void SearchOKClickHandler(object sender, EventArgs e)
        {
            if (tstbSearch.Text.Trim() != "")
            {
                try
                {
                    bsFile.Filter = "QuotationNo<>''";
                    PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvQuoteForFollowUp);
                    dgvQuoteForFollowUp.Select();
                    if (pnlRecord.Visible == true)
                        cboCutOffDays.Text = Convert.ToString(7);
                        LoadRecords(Convert.ToInt16(cboCutOffDays.Text));
                }
                catch { }
            }
        }

        private void SearchFilterClickHandler(object sender, EventArgs e)
        {
            if (tstbSearch.Text.Trim() != "")
            {
                try
                {
                    if (arrCol[nIndex] == "System.String")
                    {
                        string strSearch = tstbSearch.Text.Replace("'", "''");
                        DateTime dte;
                        if (DateTime.TryParse(strSearch, out dte))
                        {
                            bsFile.Filter = tstbSearchField.Text + " = '" + Convert.ToDateTime(tstbSearch.Text).ToString("MM/dd/yyyy") + "'";
                        }
                        else
                        {
                            if (chkFullText.Checked == true)
                                bsFile.Filter = tstbSearchField.Text + "='" + strSearch + "'";
                            else
                                bsFile.Filter = tstbSearchField.Text + " LIKE '%" + strSearch + "%'";
                        }
                    }
                    else if (arrCol[nIndex] == "System.DateTime")
                    {
                        bsFile.Filter = tstbSearchField.Text + " = #" + Convert.ToDateTime(tstbSearch.Text).ToString("MM/dd/yyyy") + "#";
                    }
                    else
                    {
                        bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;
                    }
                    
                    tsbRefresh.Enabled = true;
                    txtTotalQuotes.Text = dgvQuoteForFollowUp.RowCount.ToString();
                }
                catch { }
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            bsFile.Filter = "QuotationNo<>''";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
            txtTotalQuotes.Text = dgvQuoteForFollowUp.RowCount.ToString();
        }

        private void PrintQuoteFollowUpListClickHandler(object sender, EventArgs e)
        {
            QuoteFollowUpSheet rpt = new QuoteFollowUpSheet();
            
            if (Convert.ToInt16(cboCutOffDays.Text) > 0)
            {
                rpt.CutOffDays = Convert.ToInt16(cboCutOffDays.Text);
            }
            else
            {
                rpt.CutOffDays = 1000;
            }

            rpt.WindowState = FormWindowState.Maximized;

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void LoadData()
        {           
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void DataGridSetting()
        {
            dgvQuoteForFollowUp.EnableHeadersVisualStyles = false;
            dgvQuoteForFollowUp.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQuoteForFollowUp.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvQuoteForFollowUp.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvQuoteForFollowUp.Columns["CompanyCode"].HeaderText = "Cmpy Code";
            dgvQuoteForFollowUp.Columns["QuotationNo"].HeaderText = "Quote No";
            dgvQuoteForFollowUp.Columns["RevisionNo"].HeaderText = "Rev No";
            dgvQuoteForFollowUp.Columns["DateCreated"].HeaderText = "Date Created";
            dgvQuoteForFollowUp.Columns["DateEmailed"].HeaderText = "Date Emailed";
            dgvQuoteForFollowUp.Columns["FollowUpDate"].HeaderText = "Follow-Up Date";
            dgvQuoteForFollowUp.Columns["SponsorName"].HeaderText = "Sponsor Name";
            dgvQuoteForFollowUp.Columns["ContactName"].HeaderText = "ContactName";
            dgvQuoteForFollowUp.Columns["ChkBox"].HeaderText = "Select";
            dgvQuoteForFollowUp.Columns["CompanyCode"].Width = 65;
            dgvQuoteForFollowUp.Columns["QuotationNo"].Width = 96;
            dgvQuoteForFollowUp.Columns["RevisionNo"].Width = 40;
            dgvQuoteForFollowUp.Columns["DateCreated"].Width = 75;
            dgvQuoteForFollowUp.Columns["DateEmailed"].Width = 75;
            dgvQuoteForFollowUp.Columns["FollowUpDate"].Width = 75;
            dgvQuoteForFollowUp.Columns["SponsorName"].Width = 230;
            dgvQuoteForFollowUp.Columns["ContactName"].Width = 155;
            dgvQuoteForFollowUp.Columns["ChkBox"].Width = 50;
            dgvQuoteForFollowUp.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvQuoteForFollowUp.Columns["DateEmailed"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvQuoteForFollowUp.Columns["FollowUpDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvQuoteForFollowUp.Columns["CompanyCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQuoteForFollowUp.Columns["QuotationNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQuoteForFollowUp.Columns["RevisionNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQuoteForFollowUp.Columns["SponsorID"].Visible = false;
            dgvQuoteForFollowUp.Columns["ContactID"].Visible = false;
            dgvQuoteForFollowUp.Columns["EmailAddress"].Visible = false;
           
        }     
        
        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
            OpenControls(this.pnlRecord, false);
            LoadData();
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
                OpenControls(this.pnlRecord, false);
                LoadData();
            }
        }

        private void dgvKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }        

        private void AddRecord()
        {            
        }

        private void EditRecord()
        {                  
        }

        private void DeleteRecord()
        {           
        }

        private void SaveRecord()
        {           
        }

        private void CancelSave()
        {
        }

        private void QuoteFollowUp_Load(object sender, EventArgs e)
        {
            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }
            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();          

        }

        private void QuoteFollowUp_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    if (nMode == 0)
                    {
                        AddRecord();
                    }
                    break;

                case Keys.F3:
                    if (nMode == 0)
                    {
                        EditRecord();
                    }
                    break;

                case Keys.F4:
                    if (nMode == 0)
                    {
                        DeleteRecord();
                    }
                    break;

                case Keys.F5:
                    if (nMode == 1 || nMode == 2)
                    {
                        SaveRecord();
                    }
                    break;

                case Keys.F6:
                    if (nMode == 1 || nMode == 2)
                    {
                        CancelSave();
                    }
                    break;

                case Keys.F7:
                    if (nMode == 0)
                    {
                        tsddbPrint.ShowDropDown();
                    }
                    break;

                case Keys.F8:
                    if (nMode == 0)
                    {
                        tsddbSearch.ShowDropDown();
                    }
                    break;
                case Keys.F12:
                    if (nMode == 1 || nMode == 2)
                    {
                        DialogResult dReply = new DialogResult();
                        dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dReply == DialogResult.No)
                        {
                            return;
                        }
                    }
                    this.Close();
                    break;

                default:
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            this.Close(); this.Dispose();
        }

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void lblHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void chkCheckAll_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvQuoteForFollowUp.Rows.Count != 0)
            {
                for (int j = 0; j < dgvQuoteForFollowUp.Rows.Count; j++)                                  // DataGridView Detail Loop
                {                
                    dgvQuoteForFollowUp.Rows[j].Cells["ChkBox"].Value = chkCheckAll.CheckState;                   
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {  
            // Check first if any record was selected
            int nSelect = 0;
            for (int j = 0; j < dgvQuoteForFollowUp.Rows.Count; j++)
            {
                if (dgvQuoteForFollowUp.Rows[j].Cells["ChkBox"].Value.ToString() == "True")
                {
                    nSelect++;
                }
            }

            if (nSelect == 0)
            {
                MessageBox.Show("Please select a quote to send!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Are you sure you want to send email?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }

            // Send email 
            for (int j = 0; j < dgvQuoteForFollowUp.Rows.Count; j++)        
            {
                if (dgvQuoteForFollowUp.Rows[j].Cells["ChkBox"].Value.ToString() == "True")
                {
                    String strContact = dgvQuoteForFollowUp.Rows[j].Cells["ContactName"].Value.ToString();
                    String strFName = strContact.Substring(0, strContact.IndexOf(' '));       
                    String strEmailAddress = dgvQuoteForFollowUp.Rows[j].Cells["EmailAddress"].Value.ToString();
                    String strRefNo = "";
                    if (dgvQuoteForFollowUp.Rows[j].Cells["CompanyCode"].Value.ToString().Trim() == "P")
                        strRefNo = dgvQuoteForFollowUp.Rows[j].Cells["CompanyCode"].Value.ToString().Trim() + dgvQuoteForFollowUp.Rows[j].Cells["QuotationNo"].Value.ToString() + ".R" + dgvQuoteForFollowUp.Rows[j].Cells["RevisionNo"].Value.ToString();
                    else
                        strRefNo = dgvQuoteForFollowUp.Rows[j].Cells["QuotationNo"].Value.ToString() + ".R" + dgvQuoteForFollowUp.Rows[j].Cells["RevisionNo"].Value.ToString();
                   
                    int nCID = Convert.ToInt16(dgvQuoteForFollowUp.Rows[j].Cells["ContactID"].Value);
                    sendEMail(nCID, strFName, strEmailAddress, strRefNo);
                }
            }
            //// After sending email, reset cutoff-Days and then reload 
            //cboCutOffDays.Text = Convert.ToString(7);
            LoadRecords(Convert.ToInt16(cboCutOffDays.Text));                                                    // Load all records within 7 day time frame as default
        }       

        public void sendEMail(int cCID, string cContact, string cEmailAddr, string cRefNo)
        {
            try
            {
                string strEMail = ""; string strEM = "";
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();

                sqlcmd = new SqlCommand("SELECT TOP 1 EMailAddress FROM ContactEMAddresses WHERE ContactID = " + cCID +
                                   " AND AckReports = 1", sqlcnn);
                SqlDataReader sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strEMail = sqldr.GetValue(0).ToString();
                    strEM = strEMail;
                }
                else
                {
                    MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                sqldr.Close(); sqlcmd.Dispose();

                //For Testing
                //strEMail = "myounes@gibraltarlabsinc.com; adelacruz@gibraltarlabsinc.com; drinaldi@gibraltarlabsinc.com;"; 

                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                oMsg.HTMLBody = "<FONT face=\"Arial\">";

                // Set Email body.
                //string strRefNo = "";
                //if (txtCmpyCode.Text == "G")
                //    strRefNo = cRefNo.Substring(1, cRefNo.Length - 1);

                String Body1 = "Dear " + cContact + "," + Environment.NewLine + Environment.NewLine +
                               "I am following up on the attached quote you requested (" + cRefNo + ") - any news on whether you will be sending this work or order to us? " + Environment.NewLine + 
                               "Our records indicate that we do not have a signed copy of the quote for our files. At your convenience, please e-mail  " + Environment.NewLine +
                               "a signed copy to my attention." + Environment.NewLine + Environment.NewLine;
                String Body2 = "If you have elected not to send us the work, we value any input as to why and ask if you could please let us know by checking off one " + Environment.NewLine + 
                               "of the below:" + Environment.NewLine + Environment.NewLine;
                String Body3 = "[ ] price not competitive"  + Environment.NewLine + 
                               "[ ] facility too far away" + Environment.NewLine + 
                               "[ ] work not necessary anymore" + Environment.NewLine + 
                               "[ ] Other: ___________________________________________" + Environment.NewLine ;

                txtBody.Text = Body1 + Body2 + Body3;

                string strBody = txtBody.Text.Replace("\r\n", "<br />");
                string strSignature = ReadSignature();
                strBody = strBody + "<br /><br />" + strSignature;

                oMsg.HTMLBody += strBody.Trim();
               
                //Add an attachment
                String sDisplayName = "MyAttachment";
                string strQNo = "";
                //String sFileName =  @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + txtQuoteNo.Text.Substring(0,4) + "\\" + cRefNo + ".pdf";
                if (cRefNo.Substring(0, 1) == "P")
                    strQNo = cRefNo.Substring(1, cRefNo.Length - 1);
                else
                    strQNo = cRefNo;

                String sFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + strQNo.Substring(0, 4) + "\\" + cRefNo + ".pdf";
                int iPosition = (int)oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;

                if (File.Exists(sFileName))                                                                                  // Send attachment only if file exists
                {
                    Outlook.Attachment oAttach = oMsg.Attachments.Add(sFileName, iAttachType, iPosition, sDisplayName);

                    //Subject line
                    oMsg.Subject = "Quote Follow-Up Email - (" + cRefNo + ")";
                    // Add a recipient.
                    Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                    // Change the recipient in the next line if necessary.
                    //Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("myounes@gibraltarlabsinc.com");
                    string[] EMAddresses = strEMail.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < EMAddresses.Length; i++)
                    {
                        if (EMAddresses[i].Trim() != "")
                        {
                            //Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("drinaldi@gibraltarlabsinc.com");
                            Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                            oRecip.Resolve();
                        }
                    }
                    ////oRecip.Resolve();
                    oMsg.Display();
                    //// Send.
                    //((Outlook._MailItem)oMsg).Send();
                    //// Clean up.
                    //oRecip = null;

                    UpdateDateEmailed(cRefNo);
                    oRecips = null;
                }
                else
                    MessageBox.Show("PDF file does not exist. " + sFileName);
                
                oMsg = null;
                oApp = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;          
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

        private void cboCutOffDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadRecords(Convert.ToInt16(cboCutOffDays.Text));
            }
            catch { }
        }

        private void UpdateDateEmailed(string cQuotationNo)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection(); ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            //MessageBox.Show(cQuotationNo);
            
            int nI = cQuotationNo.IndexOf("R");
            string strRNo = cQuotationNo.Substring(nI+1, cQuotationNo.Length - (nI+1));
            sqlcmd.Parameters.AddWithValue("@CmpyCode", cQuotationNo.Substring(0,1));
            sqlcmd.Parameters.AddWithValue("@QuoteNo", cQuotationNo.Substring(1,9));
            sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo)); 
           
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdateQFollowUpDateEMailed";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return;
            }
            sqlcnn.Dispose();
        }

        private void cboCutOffDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboCutOffDays.Text.Trim() != "")
                {                    
                    try
                    {
                        LoadRecords(Convert.ToInt16(cboCutOffDays.Text));
                    }
                    catch { }
                   
                }
            }                 
        }

        private void dgvQuoteForFollowUp_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvQuoteForFollowUp.IsCurrentCellDirty)
            {
                dgvQuoteForFollowUp.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvQuoteForFollowUp_DoubleClick(object sender, EventArgs e)
        {
            if (dgvQuoteForFollowUp.Rows.Count > 0)
            {
                int intOpen = PSSClass.General.OpenForm(typeof(Quotes));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(Quotes));
                }
                Quotes childForm = new Quotes();
                childForm.Text = "QUOTATIONS";
                childForm.MdiParent = this.MdiParent;
                childForm.pubCmpyCode = dgvQuoteForFollowUp.CurrentRow.Cells["CompanyCode"].Value.ToString().Trim();
                childForm.strQuoteNo = dgvQuoteForFollowUp.CurrentCell.Value.ToString().Substring(0, 9);
                childForm.nPSw = 1;
                childForm.Show();
            }
        }

        private void btnCreatePDF_Click(object sender, EventArgs e)
        {          
            String strQNo = dgvQuoteForFollowUp.CurrentRow.Cells["QuotationNo"].Value.ToString();
            Int16 intRevNo = Convert.ToInt16(dgvQuoteForFollowUp.CurrentRow.Cells["RevisionNo"].Value.ToString());
            string strCmpy = dgvQuoteForFollowUp.CurrentRow.Cells["CompanyCode"].Value.ToString().Trim();

            CreatePDF(strCmpy, strQNo, intRevNo);
        }

        public void CreatePDF(string cCmpyCode, string cGBLNo, Int16 cRevNo)
        {
            try
            {
                QuotationRpt QuoteRpt = new QuotationRpt();
                QuoteRpt.WindowState = FormWindowState.Minimized;
                QuoteRpt.CmpyCode = cCmpyCode;
                QuoteRpt.QuoteNo = cGBLNo;
                QuoteRpt.RevNo = cRevNo;
                QuoteRpt.nQ = 9;
                QuoteRpt.Show();
                QuoteRpt.Close(); QuoteRpt.Dispose();
                MessageBox.Show("PDF file created.", "PTS");
            }
            catch 
            { 
                MessageBox.Show("Error in creating PDF file.", "PTS");
            }
        }

        private void lnkCurrPDF_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkCurrPDF.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void dgvQuoteForFollowUp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCmpyCode.Text = dgvQuoteForFollowUp.CurrentRow.Cells["CompanyCode"].Value.ToString().Trim();
            txtQuoteNo.Text = dgvQuoteForFollowUp.CurrentRow.Cells["QuotationNo"].Value.ToString();
            txtRevNo.Text = dgvQuoteForFollowUp.CurrentRow.Cells["RevisionNo"].Value.ToString();
            if (txtCmpyCode.Text == "P") 
                lnkCurrPDF.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + txtQuoteNo.Text.Substring(0,4) + "\\" + txtCmpyCode.Text + txtQuoteNo.Text + ".R" + txtRevNo.Text + ".pdf";
            else
                lnkCurrPDF.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + txtQuoteNo.Text.Substring(0, 4) + "\\" + txtQuoteNo.Text + ".R" + txtRevNo.Text + ".pdf";
        }

        private void dgvQuoteForFollowUp_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvQuoteForFollowUp.CurrentCell.OwningColumn.Name.ToString() != "ChkBox")
                e.Cancel = true;
            else
            {
                txtCmpyCode.Text = dgvQuoteForFollowUp.CurrentRow.Cells["CompanyCode"].Value.ToString().Trim();
                txtQuoteNo.Text = dgvQuoteForFollowUp.CurrentRow.Cells["QuotationNo"].Value.ToString();
                txtRevNo.Text = dgvQuoteForFollowUp.CurrentRow.Cells["RevisionNo"].Value.ToString();
                if (txtCmpyCode.Text == "P")
                    lnkCurrPDF.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + txtQuoteNo.Text.Substring(0, 4) + "\\" + txtCmpyCode.Text + txtQuoteNo.Text + ".R" + txtRevNo.Text + ".pdf";
                else
                    lnkCurrPDF.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + txtQuoteNo.Text.Substring(0, 4) + "\\" + txtQuoteNo.Text + ".R" + txtRevNo.Text + ".pdf";
            }
        }
    }
}
