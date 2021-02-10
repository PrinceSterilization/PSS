//MDIGIS.cs
// AUTHOR       : ALEXANDER M. DELA CRUZ
// TITLE        : Software Developer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Multiple Document Interface parent window.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace PSS
{
    public partial class MDIPSS : Form
    {
        private int childFormNumber = 0;
        private bool mouseDown;
        private Point mousePos;
        //private int nMsgCtr = 0;

        private BackgroundWorker m_oWorker;
        byte bCancelled = 0;

        //public int nCID = 0;
        
        protected DataTable dtSponsors = new DataTable();

        Process procPHRS;

        public MDIPSS()
        {
            InitializeComponent();
            m_oWorker = new BackgroundWorker();
            m_oWorker.WorkerSupportsCancellation = true;
            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
        }

        private void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            byte bSw = 0;
            DateTime dteRem = DateTime.Now;
            while (bCancelled == 0)
            {
                if (m_oWorker.CancellationPending == false)
                {
                    if (bSw == 0)
                    {
                        byte nRem = 0;
                        DataTable dtRpts = PSSClass.FinalReports.FinrRptSDESign();
                        if (dtRpts != null && dtRpts.Rows.Count > 0)
                        {
                            nRem = 1;
                        }
                        //PO Approval
                        DataTable dtPendingPO = PSSClass.Procurements.POFirstApprovalESign();
                        if (dtPendingPO != null && dtPendingPO.Rows.Count > 0)
                        {
                            nRem = 1;
                        }
                        //Work Leave Approval
                        DataTable dtWLRem = PSSClass.Users.WorkLeaveRem(Convert.ToInt16(LogIn.nUserID));//LogIn.nUserID 114
                        if (dtWLRem != null && dtWLRem.Rows.Count > 0)
                        {
                            nRem = 1;
                        }
                        DataTable dtWLEmp = PSSClass.Users.WorkLeaveEmp();
                        if (dtWLEmp != null && dtWLEmp.Rows.Count > 0)
                        {
                            nRem = 1;
                        }
                        if (nRem == 1)
                        {
                            Messenger childForm = new Messenger();
                            childForm.ShowDialog();
                            childForm.BringToFront();
                            System.Threading.Thread.Sleep(1000);
                        }
                        bSw = 1;
                        dteRem = DateTime.Now;
                    }
                    else if (bSw == 2)
                    {
                        DataTable dtUpdates = PSSClass.Users.UserSysUpdates(LogIn.nUserID);
                        if (dtUpdates != null && dtUpdates.Rows.Count > 0)
                        {
                            MessageBox.Show("You have new updates for upload." + Environment.NewLine + "Please logout and login back to PTS" + Environment.NewLine +
                                            "to get the latest updates.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            dtUpdates.Dispose();
                        }
                        bSw = 1;
                    }
                    //else if (dteRem.AddMinutes(2).ToString("hh:mm:ss") == DateTime.Now.ToString("hh:mm:ss"))// || DateTime.Now.ToString("hh:mm:ss") == "09:40:00")//|| DateTime.Now.ToString("hh:mm:ss") == "16:30:00")
                    else if (DateTime.Now.ToString("hh:mm:ss") == "10:00:00" || DateTime.Now.ToString("hh:mm:ss") == "13:00:00" || DateTime.Now.ToString("hh:mm:ss") == "16:00:00")
                        bSw = 0;
                    else if ((DateTime.Now.ToString("hh:mm:ss").Substring(4, 1) == "3" || DateTime.Now.ToString("hh:mm:ss").Substring(4, 1) == "6" ||
                        DateTime.Now.ToString("hh:mm:ss").Substring(4, 1) == "9") && DateTime.Now.ToString("hh:mm:ss").Substring(6, 2) == "00")
                    {
                        bSw = 2;
                    }
                }
                else
                {
                    e.Cancel = true;
                    bCancelled = 1;
                }
            }
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
            this.Close();
        }

        private void tsmiSponsors_Click(object sender, EventArgs e)
        {
            Form childForm = new Sponsors();
            childForm.MdiParent = this;
            childForm.Text = "SPONSORS ((Window " + childFormNumber++ + ")";
            try
            {
                childForm.Show();
            }
            catch { }
        }

        private void lnkHomePage_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.princesterilization.com");

            //VirtualLogin childForm = new VirtualLogin();
            //childForm.MdiParent = this;
            //childForm.Text = "Virtual Login ((Window " + childFormNumber++ + ")";
            //try
            //{
            //    childForm.Show();
            //}
            //catch { }
        }

        private void lnkIntranet_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("http://pssql:81");
        }

        private void MDIGIS_Load(object sender, EventArgs e)
        {
            //if (LogIn.strUserID == "kkohan" || LogIn.strUserID == "drinaldi" || LogIn.strUserID == "adelacruz")
            //{
            //    DataTable dtAAMI = PSSClass.Samples.AAMIQtrly();
            //    if (dtAAMI != null && dtAAMI.Rows.Count > 0)
            //    {
            //        MessageBox.Show("Quarterly AAMI reminders list is available.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //    dtAAMI.Dispose();
            //}
            string strCurrFile = Application.StartupPath + @"\PTS.exe";
            System.Reflection.Assembly assInfo = System.Reflection.Assembly.ReflectionOnlyLoadFrom(strCurrFile);
            System.Diagnostics.FileVersionInfo currFileVer = System.Diagnostics.FileVersionInfo.GetVersionInfo(assInfo.Location);
            string strCurrVer = currFileVer.FileVersion;
            tsslFileVersion.Text = "Current Version: " + "1.0.0.0";

            tsslblUserID.Text = LogIn.strUserID;

            foreach (Control control in this.Controls)
            {
                if (control is MdiClient)
                {
                    //control.BackColor = Color.SteelBlue;
                    control.BackgroundImageLayout = ImageLayout.Stretch;
                    control.BackgroundImage = Image.FromFile(Application.StartupPath + @"\PSS Background.jpg");
                    break;
                }
            }
           
            DataTable dt = new DataTable();
            dt = PSSClass.General.UserAccess(LogIn.nUserID);
            foreach (ToolStripMenuItem mnu in menuStrip.Items) 
            {
                int t = mnu.DropDownItems.Count;
                if (t > 0)
                {
                    for (int x = 0; x < t; x++)
                    {
                        if (mnu.DropDownItems[x].GetType().ToString() == "System.Windows.Forms.ToolStripMenuItem")
                        {
                            ToolStripDropDownItem smnu = (ToolStripDropDownItem)mnu.DropDownItems[x];
                            int sC = smnu.DropDownItems.Count;
                            if (sC > 0)
                            {
                                for (int w = 0; w < sC; w++)
                                {
                                    if (smnu.DropDownItems[w].GetType().ToString() == "System.Windows.Forms.ToolStripMenuItem")
                                    {
                                        int s = smnu.DropDownItems.Count;
                                        if (s > 0)
                                        {
                                            for (int a = 0; a < s; a++)
                                            {
                                                if (smnu.DropDownItems[a].GetType().ToString() == "System.Windows.Forms.ToolStripMenuItem")
                                                {
                                                    ToolStripMenuItem smn = (ToolStripMenuItem)smnu.DropDownItems[a];
                                                    if (smnu.DropDownItems[a].GetType().ToString() == "System.Windows.Forms.ToolStripMenuItem")
                                                    {
                                                        for (int k = 0; k < dt.Rows.Count; k++)
                                                        {
                                                            if (smn.Tag.ToString() == dt.Rows[k]["WinForm"].ToString())
                                                            {
                                                                smn.Enabled = true;
                                                                smnu.Enabled = true;
                                                                mnu.Enabled = true;
                                                                break;
                                                            }
                                                            else
                                                                smn.Enabled = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    if (smnu.Tag.ToString() == dt.Rows[k]["WinForm"].ToString())
                                    {
                                        smnu.Enabled = true;
                                        mnu.Enabled = true;
                                        break;
                                    }
                                    else
                                        smnu.Enabled = false;
                                }
                            }
                        }
                        if (mnu.Tag.ToString() == "WindowsMenu" || mnu.Tag.ToString() == "HelpMenu")
                            mnu.Enabled = true;
                    }
                }
            }
            //shortcut buttons on toolstrip
            foreach (ToolStripItem tls in toolStrip.Items)
            {
                if (tls.GetType().ToString() == "System.Windows.Forms.ToolStripButton")
                {
                    try
                    {
                        for (int k = 0; k < dt.Rows.Count; k++)
                        {
                            if (tls.Tag.ToString() == dt.Rows[k]["WinForm"].ToString())
                            {
                                tls.Enabled = true;
                                break;
                            }
                            else
                                tls.Enabled = false;
                        }
                    }
                    catch { }
                }
            }
            dt.Dispose();

            //Check for Reminders
            //============================================================================
            string strStartDate = "", strEndDate = "";

            DataTable dtAudits = new DataTable();
            DataTable dtVisits = new DataTable();

            DateTime dtStartWeek = DateTime.Now.AddDays((int)DateTime.Now.DayOfWeek * -1);
            DateTime dtEndWeek = dtStartWeek.AddDays(6);

            strStartDate = dtStartWeek.ToShortDateString();
            strEndDate = dtEndWeek.ToShortDateString();

            dtStartWeek = Convert.ToDateTime(strStartDate);
            dtEndWeek = Convert.ToDateTime(strEndDate);

            dtAudits = PSSClass.Visits.VisitAuditWeek(dtStartWeek, dtEndWeek);
            dtVisits = PSSClass.Visits.VisitWeek(dtStartWeek, dtEndWeek);

            if ((dtAudits != null && dtAudits.Rows.Count > 0) || (dtVisits != null && dtVisits.Rows.Count > 0))
            {
                AuditVisit auv = new AuditVisit();
                auv.ShowDialog();
            }
            //============================================================================

            //DataTable dtB = PSSClass.Employees.BirthdayCelebrants();
            //if (dtB != null && dtB.Rows.Count > 0)
            //{
            //    BirthdayGreet frm = new BirthdayGreet();
            //    frm.Show();
            //}
            //dtB.Dispose();


            //GY 2020 Comment out pop-up window prompt
            //m_oWorker.RunWorkerAsync();
        }

        private void tsmiLogIns_Click(object sender, EventArgs e)
        {
            Form childForm = new SamplesLogin();
            childForm.MdiParent = this;
            childForm.Text = "SAMPLES LOGIN ((Window " + childFormNumber++ + ")";
            childForm.Show(); 
        }

        private void tsbQoutations_Click(object sender, EventArgs e)
        {
            tsmiQuotations_Click(this, null);
        }

        private void tsbSponsors_Click(object sender, EventArgs e)
        {
            tsmiSponsors_Click(this, null);
        }

        private void tsbStates_Click(object sender, EventArgs e)
        {
            tsmiStates_Click(this, null);
        }

        private void tsbRegions_Click(object sender, EventArgs e)
        {
            tsmiRegions_Click(this, null);
        }

        private void tsiSOPCheck_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(SOPCheck));

            //if (intOpen == 0)
            //{
            //    SOPCheck childForm = new SOPCheck();
            //    childForm.MdiParent = this;
            //    childForm.Text = "SOP FILE CHECK";
            //    childForm.Show();
            //}
        }

        private void tsmiEmployees_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(Employees));

            if (intOpen == 0)
            {
                Employees childForm = new Employees();
                childForm.MdiParent = this;
                childForm.Text = "EMPLOYEES 201 FILE";
                childForm.Show();
            }
        }

        private void tsmiQuotations_Click(object sender, EventArgs e)
        {
            Form childForm = new Quotes();
            childForm.MdiParent = this;
            childForm.Text = "QUOTATIONS ((Window " + childFormNumber++ + ")";
            childForm.Show(); 
        }

        private void tsmiServiceCodes_Click(object sender, EventArgs e)
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
            int intOpen = PSSClass.General.OpenForm(typeof(ServiceCodes));

            if (intOpen == 0)
            {
                Form childForm = new ServiceCodes();
                childForm.MdiParent = this;
                childForm.Text = "SERVICE CODES";
                childForm.Show();
            }
        }

        private void tsmiStates_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(States));

            if (intOpen == 0)
            {
                Form childForm = new States();
                childForm.MdiParent = this;
                childForm.Text = "STATES";
                childForm.Show();
            }
        }

        private void tsmiRegions_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(Regions));

            if (intOpen == 0)
            {
                Form childForm = new Regions();
                childForm.MdiParent = this;
                childForm.Text = "REGIONS";
                childForm.Show();
            }
        }

        private void tsmiDTR_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(DTR));

            if (intOpen == 0)
            {
                Form childForm = new DTR();
                childForm.MdiParent = this;
                childForm.Text = "DAILY TIME ATTENDANCE";
                childForm.Show();
            }
        }

        private void MDIGIS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_oWorker != null && m_oWorker.IsBusy)
            {
                m_oWorker.CancelAsync();
                m_oWorker.Dispose();
            }
            m_oWorker = null;
            bCancelled = 1;

            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }

            string strCompName = System.Net.Dns.GetHostEntry("").HostName;

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problen encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.Parameters.AddWithValue("@LType", 0);
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
            if (procPHRS != null)
            {
                try
                {
                    procPHRS.Kill();
                }
                catch { }
            }
            ////bgwMessenger.CancelAsync();
            //PSSClass.General.CloseForm(typeof(GISMessenger));
            //PSSClass.MemoryManagement.FlushMemory();
        }

        private void tsmiGIS1_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(@"C:\Users\" + LogIn.strUserID + @"\Desktop\gis_startSQL.mdb");
        }

        private void LoadSponsorsDDL()
        {
            dgvSponsors.DataSource = null;

            dtSponsors = PSSClass.Sponsors.SponsorNamesDDL();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            dgvSponsors.DataSource = dtSponsors;
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = true;
        }

        private void pnlTestFinder_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlTestFinder.Location = PointToClient(this.pnlTestFinder.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlTestFinder_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void pnlTestFinder_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlTestFinder.Visible = false;
        }

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                //pnlTestFinder.Location = PointToClient(this.pnlTestFinder.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
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

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                dgvSponsors.Visible = false;
            }
            else if (e.KeyChar == 27)
                dgvSponsors.Visible = false;
            else
            {
                txtSponsor.Text = ""; dgvSponsors.Visible = false;
            }
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSponsors;
            dvwSponsors = new DataView(dtSponsors, "SponsorName like '%" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
            dgvSponsors.DataSource = dvwSponsors;
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            dgvSponsors.Visible = false;
            btnSearch_Click(null, null);
        }

        private void dgvSponsors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSponsors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                dgvSponsors.Visible = false;
                btnSearch_Click(null, null);
            }
            else if (e.KeyChar == 27)
            {
                dgvSponsors.Visible = false;
            }
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            LoadSponsorsDDL();
            dgvSponsors.Visible = true; dgvSponsors.BringToFront();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable dtQ = new DataTable();
            if (txtSponsorID.Text.Trim() == "")
                dtQ = PSSClass.Quotations.FindTestItems(txtTestDesc.Text, 0, Convert.ToInt16(chkAnyMatch.CheckState));
            else
                dtQ = PSSClass.Quotations.FindTestItems(txtTestDesc.Text, Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(chkAnyMatch.CheckState));
            if (dtQ == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (dtQ.Rows.Count == 0)
            {
                if (txtSponsorID.Text.Trim() == "")
                    dtQ = PSSClass.Quotations.FindTestItemsCom(txtTestDesc.Text, 0, Convert.ToInt16(chkAnyMatch.CheckState));
                else
                    dtQ = PSSClass.Quotations.FindTestItemsCom(txtTestDesc.Text, Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(chkAnyMatch.CheckState));
            }
            dgvTestItems.DataSource = dtQ;
            dgvTestItems.EnableHeadersVisualStyles = false;
            dgvTestItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTestItems.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvTestItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTestItems.Columns["TestDesc1"].HeaderText = "TEST DESCRIPTION";
            dgvTestItems.Columns["QuoteNo"].HeaderText = "QUOTATION NO.";
            dgvTestItems.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvTestItems.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvTestItems.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvTestItems.Columns["UnitPrice"].HeaderText = "UNIT PRICE";
            dgvTestItems.Columns["TestDesc1"].Width = 450;
            dgvTestItems.Columns["QuoteNo"].Width = 90;
            dgvTestItems.Columns["ServiceCode"].Width = 75;
            dgvTestItems.Columns["SponsorID"].Width = 75;
            dgvTestItems.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTestItems.Columns["SponsorName"].Width = 295;
            dgvTestItems.Columns["QuoteNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTestItems.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTestItems.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTestItems.Columns["UnitPrice"].DefaultCellStyle.Format = "$#,##0.00";
        }

        private void dgvTestItems_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(dgvTestItems.CurrentRow.Cells[1].Value.ToString());
        }

        private void tsslTestFinder_Click(object sender, EventArgs e)
        {
            if (dgvSponsors.Rows.Count == 0)
                LoadSponsorsDDL();

            if (pnlTestFinder.Visible == false)
            {
                pnlTestFinder.Visible = true;
            }
            pnlTestFinder.BringToFront();
        }

        private void tsmiQuoteColHeaders_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(QuoteCol));

            //if (intOpen == 0)
            //{
            //    QuoteCol childForm = new QuoteCol();
            //    childForm.MdiParent = this;
            //    childForm.Text = "QUOTATION COLUMN HEADERS";
            //    childForm.Show();
            //}
        }

        private void tsmiSamplesTracker_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(SamplesTracker));

            //if (intOpen == 0)
            //{
            //    Form childForm = new SamplesTracker();
            //    childForm.MdiParent = this;
            //    childForm.Text = "SAMPLES TRACKER";
            //    childForm.Show();
            //}
        }

        private void tsmiSlashLabels_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(SlashDataLabels));

            //if (intOpen == 0)
            //{
            //    Form childForm = new SlashDataLabels();
            //    childForm.MdiParent = this;
            //    childForm.Text = "SLASH DATA LABELS";
            //    childForm.Show();
            //}
        }

        private void tsmiExtSlashLabels_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(ExtSlashDataLabels));

            //if (intOpen == 0)
            //{
            //    Form childForm = new ExtSlashDataLabels();
            //    childForm.MdiParent = this;
            //    childForm.Text = "EXTENDED SLASH DATA LABELS";
            //    childForm.Show();
            //}
        }

        private void tsmiExtSCLabels_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(ExtDataLabels));

            //if (intOpen == 0)
            //{
            //    ExtDataLabels childForm = new ExtDataLabels();
            //    childForm.MdiParent = this;
            //    childForm.Text = "EXTENDED DATA LABELS";
            //    childForm.Show();
            //}
        }

        private void tsmiFinalRpts_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(FinalReports));

            if (intOpen == 0)
            {
                Form childForm = new FinalReports();
                childForm.MdiParent = this;
                childForm.Text = "FINAL REPORTS";
                childForm.Show();
            }
        }

        private void prepaymentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(PrePayment));
            if (intOpen == 0)
            {
                PrePayment childForm = new PrePayment();
                childForm.MdiParent = this;
                childForm.Text = "PREPAYMENTS";
                childForm.Show();
            }
        }

        private void hlpAboutGIS_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(AboutGIS));

            //if (intOpen == 0)
            //{
            //    AboutGIS childForm = new AboutGIS();
            //    childForm.MdiParent = this;
            //    childForm.Text = "About GIS";
            //    childForm.Show();
            //}
        }

        private void tsmiPrepayPercent_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(PercentPrepay));

            if (intOpen == 0)
            {
                Form childForm = new PercentPrepay();
                childForm.MdiParent = this;
                childForm.Text = "PREPAYMENT PERCENTAGES";
                childForm.Show();
            }
        }

        private void tsmiControlPages_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(ControlPages));

            if (intOpen == 0)
            {
                Form childForm = new ControlPages();
                childForm.MdiParent = this;
                childForm.Text = "CONTROL PAGES";
                childForm.Show();
            }
        }

        private void tsmiIIOOS_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(OOS));

            if (intOpen == 0)
            {
                Form childForm = new OOS();
                childForm.MdiParent = this;
                childForm.Text = "INTERNAL INVESTIGATION/OOS";
                childForm.Show();
            }
        }

        private void tsmiAudit_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(AuditLog));

            if (intOpen == 0)
            {
                Form childForm = new AuditLog();
                childForm.MdiParent = this;
                childForm.Text = "AUDIT LOG";
                childForm.Show();
            }
        }

        private void tsmiPMRC_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(PMRC));

            if (intOpen == 0)
            {
                Form childForm = new PMRC();
                childForm.MdiParent = this;
                childForm.Text = "PMRC Text";
                childForm.Show();
            }
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = true;
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void txtTestDesc_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void btnPrtPreview_Click(object sender, EventArgs e)
        {
            if (dgvTestItems.Rows.Count == 0)
            {
                MessageBox.Show("No quotes to print preview.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            QuotationRpt rptQuotation = new QuotationRpt();
            rptQuotation.WindowState = FormWindowState.Maximized;
            rptQuotation.nQ = 2;
            try
            {
                string strQuote = dgvTestItems.Rows[dgvTestItems.CurrentCell.RowIndex].Cells["QuoteNo"].Value.ToString();
                string strQNo = strQuote.Substring(0, 9);
                string strRevNo = strQuote.Substring(strQuote.IndexOf("R") + 1, strQuote.Length - (strQuote.IndexOf("R") + 1));
                if (strQuote.Substring(0, 1) == "P")
                {
                    rptQuotation.CmpyCode = "P";
                    strQNo = strQuote.Substring(1, 9);
                }
                else
                {
                    rptQuotation.CmpyCode = "G";
                    strQNo = strQuote.Substring(0, 9);
                }
                rptQuotation.QuoteNo = strQNo;
                rptQuotation.RevNo = Convert.ToInt16(strRevNo);
                rptQuotation.Show();
            }
            catch { }
        }

        private void tsslSpecialChars_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(SpecialChars));

            if (intOpen == 0)
            {
                Form childForm = new SpecialChars();
                childForm.Text = "SPECIAL CHARACTERS";
                childForm.WindowState = FormWindowState.Normal;
                childForm.ShowDialog();
            }
        }

        private void tsmiFinalBilling_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(FinalBilling));

            if (intOpen == 0)
            {
                Form childForm = new FinalBilling();
                childForm.MdiParent = this;
                childForm.Text = "FINAL BILLING";
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();
            }
        }

        private void tsmiCNCTests_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(NonConformingControl));

            if (intOpen == 0)
            {
                Form childForm = new NonConformingControl();
                childForm.MdiParent = this;
                childForm.Text = "CONTROL FOR NON-CONFORMING TESTS";
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();
            }
        }

        private void tsmiVendors_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(Vendors));

            if (intOpen == 0)
            {
                Form childForm = new Vendors();
                childForm.MdiParent = this;
                childForm.Text = "VENDORS";
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();
            }
        }

        private void tsmiPurchaseOrders_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(PurchaseOrder));

            if (intOpen == 0)
            {
                Form childForm = new PurchaseOrder();
                childForm.MdiParent = this;
                childForm.Text = "PURCHASE ORDERS";
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();
            }
        }

        private void tsmiQuoteFollowup_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(QuoteFollowUp));

            if (intOpen == 0)
            {
                Form childForm = new QuoteFollowUp();
                childForm.MdiParent = this;
                childForm.Text = "QUOTE FOLLOW-UP";
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();
            }
        }

        private void tsmiMgntRpts_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(ManagementRpts));

            if (intOpen == 0)
            {
                Form childForm = new ManagementRpts();
                childForm.MdiParent = this;
                childForm.Text = "MANAGEMENT REPORTS";
                childForm.WindowState = FormWindowState.Normal;
                childForm.Show();
            }
        }

        private void txtTestDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnSearch_Click(null, null);
        }

        private void tsmiGISACCPAC_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(BudgetMaint));

            //if (intOpen == 0)
            //{
            //    BudgetMaint childForm = new BudgetMaint();
            //    childForm.MdiParent = this;
            //    childForm.Text = "ANNUAL BUDGET FILE MAINTENANCE";
            //    //childForm.WindowState = FormWindowState.Maximized;
            //    childForm.Show();
            //}
        }

        private void tsmiESignatures_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(FinalRptESign));

            if (intOpen == 0)
            {
                FinalRptESign childForm = new FinalRptESign();
                childForm.MdiParent = this;
                childForm.Text = "E-SIGNATURES";
                childForm.Show();
            }
        }

        private void tsmiLocations_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(Location));

            if (intOpen == 0)
            {
                Location childForm = new Location();
                childForm.MdiParent = this;
                childForm.Text = "LOCATIONS";
                childForm.Show();
            }
        }

        private void tsmiFinancialRpt_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(FinancialRpt));

            if (intOpen == 0)
            {
                FinancialRpt childForm = new FinancialRpt();
                childForm.MdiParent = this;
                childForm.Text = "FINANCIAL REPORTS";
                childForm.Show();
            }
        }

        private void tsmiScanInv_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(ScanInvoice));

            if (intOpen == 0)
            {
                Form childForm = new ScanInvoice();
                childForm.Text = "SCAN INVOICE";
                childForm.WindowState = FormWindowState.Normal;
                childForm.ShowDialog();
            }
        }

        private void tsmiUnits_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(Units));

            if (intOpen == 0)
            {
                Units childForm = new Units();
                childForm.MdiParent = this;
                childForm.Text = "UNITS";
                childForm.Show();
            }
        }

        private void tsmiInvDataExp_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(InvoiceExport));

            if (intOpen == 0)
            {
                InvoiceExport childForm = new InvoiceExport();
                childForm.MdiParent = this;
                childForm.Text = "INVOICE DATA EXPORT";
                childForm.Show();
            }
        }

        private void tsmiBillingReview_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(AutoBilling));

            //if (intOpen == 0)
            //{
            //    AutoBilling childForm = new AutoBilling();
            //    childForm.MdiParent = this;
            //    childForm.Text = "AUTOMATED BILLING";
            //    childForm.Show();
            //}
            ////Form1 frm = new Form1();
            ////frm.MdiParent = this;
            ////frm.Show();
        }

        private void tsmiUsersProfile_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(UsersProfile));

            if (intOpen == 0)
            {
                UsersProfile childForm = new UsersProfile();
                childForm.MdiParent = this;
                childForm.Text = "USERS PROFILE";
                childForm.Show();
            } 
        }

        private void tsmiSysForms_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(SystemForms));

            if (intOpen == 0)
            {
                SystemForms childForm = new SystemForms();
                childForm.MdiParent = this;
                childForm.Text = "SYSTEM FORMS";
                childForm.Show();
            } 
        }

        private void tsmiVersionMaster_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(VersionMaster));

            if (intOpen == 0)
            {
                VersionMaster childForm = new VersionMaster();
                childForm.MdiParent = this;
                childForm.Text = "VERSIONS MASTER FILE";
                childForm.Show();
            } 
        }

        private void tsmiVersionUsers_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(VersionUsers));

            if (intOpen == 0)
            {
                VersionUsers childForm = new VersionUsers();
                childForm.MdiParent = this;
                childForm.Text = "VERSIONS USERS";
                childForm.Show();
            } 
        }

        private void tsmiQFExclusion_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(FollowupsExcluded));

            if (intOpen == 0)
            {
                FollowupsExcluded childForm = new FollowupsExcluded();
                childForm.MdiParent = this;
                childForm.Text = "QUOTATION FOLLOW-UP EXCLUSION LIST";
                childForm.Show();
            } 
        }

        private void tsmiPOBalance_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(POBalance));

            if (intOpen == 0)
            {
                POBalance childForm = new POBalance();
                childForm.MdiParent = this;
                childForm.Text = "PO BALANCE MONITORING";
                childForm.Show();
            } 
        }

        private void tsslSearchLogin_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(LoginSearch));

            if (intOpen == 0)
            {
                LoginSearch childForm = new LoginSearch();
                childForm.Text = "SEARCH LOGIN RECORDS";
                childForm.Show();
            }           
        }

        private void tsmiPR_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(PurchaseRequisition));

            if (intOpen == 0)
            {
                PurchaseRequisition childForm = new PurchaseRequisition();
                childForm.Text = "PURCHASE REQUISITIONS";
                childForm.MdiParent = this;
                childForm.Show();
            }

            //int intOpen = PSSClass.General.OpenForm(typeof(PurchaseRequisitionX));

            //if (intOpen == 0)
            //{
            //    PurchaseRequisitionX childForm = new PurchaseRequisitionX();
            //    childForm.Text = "PURCHASE REQUISITIONS";
            //    childForm.MdiParent = this;
            //    childForm.Show();
            //}    
        }

        private void tsmiCatalogNames_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(CatalogNames));

            if (intOpen == 0)
            {
                CatalogNames childForm = new CatalogNames();
                childForm.Text = "CATALOG NAMES";
                childForm.MdiParent = this;
                childForm.Show();
            }         
        }

        private void tsmiCatalogs_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(Catalog));

            if (intOpen == 0)
            {
                Catalog childForm = new Catalog();
                childForm.MdiParent = this;
                childForm.Text = "CATALOGS";
                childForm.Show();
            }
        }

        private void tsmiEquipment_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(Equipment));

            if (intOpen == 0)
            {
                Form childForm = new Equipment();
                childForm.MdiParent = this;
                childForm.Text = "EQUIPMENT";
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();
            }
        }

        private void tsmiDepartments_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(Departments));

            if (intOpen == 0)
            {
                Departments childForm = new Departments();
                childForm.MdiParent = this;
                childForm.Text = "DEPARTMENTS";
                childForm.Show();
            }
        }

        private void tsbDepartments_Click(object sender, EventArgs e)
        {
            tsmiServiceDept_Click(null, null);
        }

        private void tsslMessenger_Click(object sender, EventArgs e)
        {
            ////MessageBox.Show("This feature is temporarily unavailable." + Environment.NewLine + "Upgrades are being undertaken.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //byte bOpen = 0;

            //foreach (Form form in Application.OpenForms)
            //{
            //    if (form.GetType() == typeof(GISMessenger))
            //    {
            //        form.Text = "GIS Messenger";
            //        form.WindowState = FormWindowState.Normal;

            //        form.Show();
            //        bOpen = 1;
            //        break;
            //    }
            //}

            //if (bOpen == 0)
            //{
            //    GISMessenger childForm = new GISMessenger();
            //    childForm.nUID = LogIn.nUserID;
            //    childForm.Text = "GIS Messenger";
            //    childForm.WindowState = FormWindowState.Normal;
            //    childForm.Show();
            //}

            //int intOpen = PSSClass.General.OpenForm(typeof(GISMessenger));

            //if (intOpen == 0)
            //{
            //    GISMessenger childForm = new GISMessenger();
            //    childForm.MdiParent = this;
            //    childForm.nUID = LogIn.nUserID;
            //    childForm.Text = "GIS Messenger";
            //    childForm.WindowState = FormWindowState.Normal; 
            //    childForm.Show();
            //}
        }

        private void tsslOldGIS_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(@"C:\Users\" + LogIn.strUserID + @"\Desktop\gis_startSQL.mdb");
        }

        //private void bgwMessenger_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    int bM = 0;

        //    BackgroundWorker worker = sender as BackgroundWorker;

        //    while (true)
        //    {
        //        if ((worker.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            break;
        //        }
        //        else
        //        {
        //            System.Threading.Thread.Sleep(500);
        //            try
        //            {
        //                MessageQueue msgQ;
        //                System.Messaging.Message m = new System.Messaging.Message();

        //                string strCompID = PSSClass.Users.CompID(LogIn.nUserID);
        //                string strUserID = PSSClass.Users.LogID(LogIn.nUserID);
        //                msgQ = new MessageQueue("FormatName:DIRECT=OS:" + strCompID + "\\Private$\\" + strUserID);

        //                nMsgCtr = 0;

        //                MessageEnumerator Enumerator = msgQ.GetMessageEnumerator2();
        //                msgQ.MessageReadPropertyFilter.SetAll();

        //                while (Enumerator.MoveNext())
        //                {
        //                    nMsgCtr++;
        //                }
        //                msgQ.Close();
        //                if (nMsgCtr > 0)
        //                    break;
        //            }
        //            catch { }
        //            try
        //            {
        //                if (bM == 0)
        //                {
        //                    tsslMessenger.Text = " GIS Messenger -";
        //                    bM++;
        //                }
        //                else if (bM == 1)
        //                {
        //                    tsslMessenger.Text = " GIS Messenger --";
        //                    bM++;
        //                }
        //                else if (bM == 2)
        //                {
        //                    tsslMessenger.Text = " GIS Messenger -->";
        //                    bM = 0;
        //                }
        //            }
        //            catch { }
        //        }
        //    }
        //}

        //private void bgwMessenger_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (nMsgCtr > 0)
        //    {
        //        byte bOpen = 0;

        //        foreach (Form form in Application.OpenForms)
        //        {
        //            if (form.GetType() == typeof(GISMessenger))
        //            {
        //                form.Text = "GIS Messenger";
        //                form.WindowState = FormWindowState.Normal;
        //                form.Show();
        //                form.Select();
        //                bOpen = 1;
        //                break;
        //            }
        //        }

        //        if (bOpen == 0)
        //        {
        //            GISMessenger childForm = new GISMessenger();
        //            childForm.Text = "GIS Messenger";
        //            childForm.WindowState = FormWindowState.Normal;
        //            childForm.Show();
        //            childForm.Select();
        //        }
        //    }
        //    nMsgCtr = 0;
        //    if (bgwMessenger.IsBusy != true)
        //    {
        //        bgwMessenger.RunWorkerAsync();
        //    }
        //}

        private void tsmiGFRS_Click(object sender, EventArgs e)
        {
            Process.Start(@"S:\IT Files\Installers\GFRS\Copytrack\gfrs.EXE");
        }

        private void tsmiPOESignatures_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(POESign));

            if (intOpen == 0)
            {
                POESign childForm = new POESign();
                childForm.MdiParent = this;
                childForm.Text = "PO E-SIGNATURES";
                childForm.Show();
            }
        }

        private void tsmiLettersMemos_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(DocumentMaster));

            if (intOpen == 0)
            {
                DocumentMaster childForm = new DocumentMaster();
                childForm.MdiParent = this;
                childForm.Text = "DOCUMENTS MASTER";
                childForm.Show();
            }
        }

        private void lnkDrivve_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://192.168.100.27:8080");
        }

        private void tsmiAcctgReports_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(AcctgReports));

            if (intOpen == 0)
            {
                AcctgReports childForm = new AcctgReports();
                childForm.MdiParent = this;
                childForm.Text = "ACCOUNTING REPORTS";
                childForm.Show();
            }
        }

        private void tsmiServiceDept_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(ServiceDepartments));

            if (intOpen == 0)
            {
                ServiceDepartments childForm = new ServiceDepartments();
                childForm.MdiParent = this;
                childForm.Text = "Service Departments";
                childForm.Show();
            }
        }

        private void tsmiUserGroups_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(UserGroups));

            if (intOpen == 0)
            {
                UserGroups childForm = new UserGroups();
                childForm.MdiParent = this;
                childForm.Text = "User Groups";
                childForm.Show();
            }
        }

        private void tsmiScanFinRpt_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(ScanFinRpt));

            if (intOpen == 0)
            {
                Form childForm = new ScanFinRpt();
                childForm.Text = "SCAN FINAL REPORTS";
                childForm.WindowState = FormWindowState.Normal;
                childForm.ShowDialog();
            }
        }

        private void tsmiTestDataLabels_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(TestDataLabels));

            if (intOpen == 0)
            {
                TestDataLabels childForm = new TestDataLabels();
                childForm.MdiParent = this;
                childForm.Text = "TEST DATA LABELS";
                childForm.Show();
            }
        }

        private void tsmiUpdateManifest_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(Manifest));

            //if (intOpen == 0)
            //{
            //    Manifest childForm = new Manifest();
            //    childForm.MdiParent = this;
            //    childForm.Text = "INGREDION MANIFEST UPDATE";
            //    childForm.Show();
            //}
        }

        private void executiveDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(ExecDashboard));

            if (intOpen == 0)
            {
                ExecDashboard childForm = new ExecDashboard();
                childForm.MdiParent = this;
                childForm.Text = "EXECUTIVE DASHBOARD";
                childForm.Show();
            }
        }

        private void tsmiIngredionManifest_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(IngredionManifestLog));

            //if (intOpen == 0)
            //{
            //    IngredionManifestLog childForm = new IngredionManifestLog();
            //    childForm.MdiParent = this;
            //    childForm.Text = "INGREDION LOGIN - MANIFEST";
            //    childForm.Show();
            //}
        }

        private void tsmiManifestEx_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(ManifestExceptions));

            //if (intOpen == 0)
            //{
            //    ManifestExceptions childForm = new ManifestExceptions();
            //    childForm.MdiParent = this;
            //    childForm.Text = "INGREDION MANIFEST EXCEPTIONS";
            //    childForm.Show();
            //}
        }

        private void tsmiIngredionBilling_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(IngredionInvEntries));

            //if (intOpen == 0)
            //{
            //    IngredionInvEntries childForm = new IngredionInvEntries();
            //    childForm.MdiParent = Program.mdi;
            //    childForm.Text = "INGREDION BILLING REVIEW";
            //    childForm.Show();
            //}

            ////int intOpen = PSSClass.General.OpenForm(typeof(AutoInvoice));

            ////if (intOpen == 0)
            ////{
            ////    AutoInvoice childForm = new AutoInvoice();
            ////    childForm.MdiParent = Program.mdi;
            ////    childForm.Text = "BILLING REVIEW";
            ////    childForm.Show();
            ////}
        }

        private void tsmiPOReports_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(PurchaseOrderReports));

            if (intOpen == 0)
            {
                PurchaseOrderReports childForm = new PurchaseOrderReports();
                childForm.MdiParent = this;
                childForm.Text = "PURCHASE ORDER REPORTS";
                childForm.Show();
            }
        }

        private void tsmiStability_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(Stability));

            //if (intOpen == 0)
            //{
            //    Stability childForm = new Stability();
            //    childForm.MdiParent = this;
            //    childForm.Text = "STABILITY";
            //    childForm.Show();
            //}
        }

        private void tsmiTestFinder_Click(object sender, EventArgs e)
        {
            tsslTestFinder_Click(null, null);
        }

        private void tsmiExpQuotes_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(ExpiringQuotes));

            if (intOpen == 0)
            {
                ExpiringQuotes childForm = new ExpiringQuotes();
                childForm.MdiParent = this;
                childForm.Text = "EXPIRING QUOTATIONS";
                childForm.Show();
            }
        }

        private void tsmiHRReports_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(HRReports));

            if (intOpen == 0)
            {
                HRReports childForm = new HRReports();
                childForm.MdiParent = this;
                childForm.Text = "HR REPORTS";
                childForm.Show();
            }
        }

        private void tsmiLabelsGBLSlash_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(Labels));

            //if (intOpen == 0)
            //{
            //    Labels childForm = new Labels();
            //    childForm.MdiParent = this;
            //    childForm.Text = "Labels";
            //    childForm.nLabelTypeID = 1;                                           //  GBL & Slash Labels 
            //    childForm.Show();
            //}
        }

        private void tsmiLabelsIngredion_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(Labels));

            //if (intOpen == 0)
            //{
            //    Labels childForm = new Labels();
            //    childForm.MdiParent = this;
            //    childForm.Text = "Labels";
            //    childForm.nLabelTypeID = 2;                                           //  Ingredion Labels
            //    childForm.Show();
            //}
        }

        private void tsmiLabelsMedia_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(Labels));

            //if (intOpen == 0)
            //{
            //    Labels childForm = new Labels();
            //    childForm.MdiParent = this;
            //    childForm.Text = "Labels";
            //    childForm.nLabelTypeID = 3;                                           //  Media Labels 
            //    childForm.Show();
            //}
        }

        private void tsmiLabelsSterility_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(Labels));

            //if (intOpen == 0)
            //{
            //    Labels childForm = new Labels();
            //    childForm.MdiParent = this;
            //    childForm.Text = "Labels";
            //    childForm.nLabelTypeID = 4;                                           //  Sterility Labels 
            //    childForm.Show();
            //}
            System.Diagnostics.Process.Start("http://172.16.2.113/Sterilization/Login.aspx");
        }

        private void tsmiLabelsWrappedGoods_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(Labels));

            //if (intOpen == 0)
            //{
            //    Labels childForm = new Labels();
            //    childForm.MdiParent = this;
            //    childForm.Text = "Labels";
            //    childForm.nLabelTypeID = 5;                                           //  Wrapped Goods Labels 
            //    childForm.Show();
            //}
        }

        private void lblESignatures_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(LabelESign));

            //if (intOpen == 0)
            //{
            //    LabelESign childForm = new LabelESign();
            //    childForm.MdiParent = this;
            //    childForm.Text = "Labels E-Signatures";
            //    childForm.Show();
            //}
        }

        private void tsmiSOA_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(SOA));

            if (intOpen == 0)
            {
                SOA childForm = new SOA();
                childForm.MdiParent = this;
                childForm.Text = "STATEMENT OF ACCOUNT";
                childForm.Show();
            }

            //int intOpen = PSSClass.General.OpenForm(typeof(BDSReports));

            //if (intOpen == 0)
            //{
            //    BDSReports childForm = new BDSReports();
            //    childForm.MdiParent = this;
            //    childForm.Text = "BDS REPORTS UPLOAD";
            //    childForm.Show();
            //}
        }

        private void tsmiIngredionSpecial_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(IngredionSpclBatch));

            //if (intOpen == 0)
            //{
            //    IngredionSpclBatch childForm = new IngredionSpclBatch();
            //    childForm.MdiParent = this;
            //    childForm.Text = "INGREDION SPECIAL BATCH INPUT";
            //    childForm.Show();
            //}
        }

        private void tsmiEPassword_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(ESignPassword));

            if (intOpen == 0)
            {
                ESignPassword childForm = new ESignPassword();
                childForm.MdiParent = this;
                childForm.Text = "E-SIGNATURE PASSWORD";
                childForm.Show();
            }
        }

        private void tsmiVisits_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(VisitSchedules));

            if (intOpen == 0)
            {
                VisitSchedules childForm = new VisitSchedules();
                childForm.MdiParent = this;
                childForm.Text = "Visit Schedules";
                childForm.Show();
            }
        }

        private void tsmiOtherBillings_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(MiscBilling));

            if (intOpen == 0)
            {
                Form childForm = new MiscBilling();
                childForm.MdiParent = this;
                childForm.Text = "OTHER BILLINGS";
                childForm.WindowState = FormWindowState.Maximized;
                childForm.Show();
            }
        }

        private void tsmiAAMI_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(AAMIQtrlyReminder));

            if (intOpen == 0)
            {
                AAMIQtrlyReminder childForm = new AAMIQtrlyReminder();
                childForm.MdiParent = this;
                childForm.Text = "AAMI QUARTERLY REMINDER";
                childForm.WindowState = FormWindowState.Normal;
                childForm.Show();
            }
        }

        private void tsmiGRMS_Click(object sender, EventArgs e)
        {
            ////Process.Start(@"S:\IT Files\Installers\GRMS\GRMS.EXE");
            ////Process proc = new Process();
            ////proc.StartInfo.FileName = @"S:\IT Files\Installers\GRMS\GRMS.EXE";
            ////proc.StartInfo.Arguments = LogIn.strUserID + " " + LogIn.strPassword;
            ////proc.Start();
            ////proc.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

            //using (Process proc = new Process())
            //{
            //    proc.StartInfo.FileName = Application.StartupPath + @"\GRMS.exe";
            //    proc.StartInfo.Arguments = LogIn.nUserID.ToString();
            //    proc.Start();
            //    proc.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //}
        }

        private void tsmiDocTracking_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(DocumentTracking));

            //if (intOpen == 0)
            //{
            //    DocumentTracking childForm = new DocumentTracking();
            //    childForm.MdiParent = this;
            //    childForm.Text = "DOCUMENTS TRACKING";
            //    childForm.WindowState = FormWindowState.Normal;
            //    childForm.Show();
            //}
        }

        private void tsmiDocTypes_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(DocumentTypes));

            //if (intOpen == 0)
            //{
            //    DocumentTypes childForm = new DocumentTypes();
            //    childForm.MdiParent = this;
            //    childForm.Text = "DOCUMENTS TYPES";
            //    childForm.WindowState = FormWindowState.Normal;
            //    childForm.Show();
            //}

        }

        private void tsmiWLOT_Click(object sender, EventArgs e)
        {
            //using (Process proc = new Process())
            //{
            //    proc.StartInfo.FileName = Application.StartupPath + @"\PHRS\PHRS.exe";
            //    //proc.StartInfo.FileName = @"\\PSAPP01\IT Files\PHRS\PHRS.exe";
            //    proc.StartInfo.Arguments = LogIn.nUserID.ToString();
            //    proc.Start();
            //    proc.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //}
            procPHRS = new Process();
            procPHRS.StartInfo.FileName = Application.StartupPath + @"\PHRS\PHRS.exe";
            procPHRS.StartInfo.Arguments = LogIn.nUserID.ToString() + " " + LogIn.strUserID;
            procPHRS.Start();
            procPHRS.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
        }

        private void tsmiInquiries_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(Inquiries));

            //if (intOpen == 0)
            //{
            //    Inquiries childForm = new Inquiries();
            //    childForm.MdiParent = this;
            //    childForm.Text = "INQUIRIES";
            //    childForm.WindowState = FormWindowState.Normal;
            //    childForm.Show();
            //}
        }

        private void tsmiSC_Click(object sender, EventArgs e)
        {
            tsmiServiceCodes_Click(null, null);
        }

        private void tslReminders_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(UserReminders));

            //if (intOpen == 0)
            //{
            //    UserReminders childForm = new UserReminders();
            //    childForm.MdiParent = this;
            //    childForm.Text = "REMINDERS";
            //    childForm.WindowState = FormWindowState.Normal;
            //    childForm.Show();
            //}
        }

        private void tsmiChangeBackground_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(ChangeBackground));

            //if (intOpen == 0)
            //{
            //    ChangeBackground childForm = new ChangeBackground();
            //    childForm.MdiParent = this;
            //    childForm.Text = "Change Background";
            //    childForm.WindowState = FormWindowState.Normal;
            //    childForm.Show();
            //}
        }

        private void tsbRegLogin_Click(object sender, EventArgs e)
        {
            Form childForm = new SamplesLogin();
            childForm.MdiParent = this;
            childForm.Text = "SAMPLES LOGIN ((Window " + childFormNumber++ + ")";
            childForm.Show(); 
        }

        private void tsbIngLogin_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(IngredionManifestLog));

            //if (intOpen == 0)
            //{
            //    IngredionManifestLog childForm = new IngredionManifestLog();
            //    childForm.MdiParent = this;
            //    childForm.Text = "INGREDION LOGIN - MANIFEST";
            //    childForm.Show();
            //}
        }

        private void tsbBCScanLogin_Click(object sender, EventArgs e)
        {

        }

        private void tsmiUpdatePenford_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(PenfordManifest));

            //if (intOpen == 0)
            //{
            //    PenfordManifest childForm = new PenfordManifest();
            //    childForm.MdiParent = this;
            //    childForm.Text = "PENFORD MANIFEST UPDATE";
            //    childForm.Show();
            //}
        }

        private void tsmiProductLabels_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://gblnj4:85");
        }

        private void tsbSamplesLoginQ_Click(object sender, EventArgs e)
        {
            //Form childForm = new SamplesLoginQ();
            //childForm.MdiParent = this;
            //childForm.Text = "DAILY SAMPLES LOGIN ((Window " + childFormNumber++ + ")";
            //childForm.Show(); 
        }

        private void tsmiPOMaster_Click(object sender, EventArgs e)
        {
            POMaster childForm = new POMaster();
            childForm.MdiParent = this;
            childForm.Text = "PURCHASE ORDERS";
            childForm.Show();

            //SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            //if (sqlcnn == null)
            //{
            //    return;
            //}

            //SqlCommand sqlcmd = new SqlCommand("spDuplicatePO", sqlcnn);
            //sqlcmd.CommandType = CommandType.StoredProcedure;

            //try
            //{
            //    SqlDataReader sqldr = sqlcmd.ExecuteReader();
            //    DataTable dTable = new DataTable();
            //    dTable.Load(sqldr);
            //    if (dTable != null && dTable.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dTable.Rows.Count; i++)
            //        {
            //            if (dTable.Rows[i]["Amt"] == DBNull.Value || Convert.ToDecimal(dTable.Rows[i]["Amt"]) == 0 ||
            //                dTable.Rows[i]["FileLoc"] == DBNull.Value || dTable.Rows[i]["FileLoc"].ToString().Trim() == "")
            //            {
            //                sqlcmd = new SqlCommand("spUpdSponsorPO", sqlcnn);
            //                sqlcmd.CommandType = CommandType.StoredProcedure;
            //                sqlcmd.Parameters.AddWithValue("SpID", dTable.Rows[i]["SpID"]);
            //                sqlcmd.Parameters.AddWithValue("PONo", dTable.Rows[i]["PONo"]);
            //                sqlcmd.ExecuteNonQuery();
            //                sqlcmd.Dispose();
            //            }
            //        }
            //    }
            //    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            //}
            //catch
            //{
            //    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            //}
        }

        private void tsmiUpdateLogin_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(UpdateLogin));

            if (intOpen == 0)
            {
                UpdateLogin childForm = new UpdateLogin();
                childForm.MdiParent = this;
                childForm.Text = "UPDATE LOGIN DATA";
                childForm.Show();
            }
        }

        private void tsmiEqptMftr_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(EquipmentMftr));

            if (intOpen == 0)
            {
                EquipmentMftr childForm = new EquipmentMftr();
                childForm.MdiParent = this;
                childForm.Text = "EQUIPMENT MANUFACTURERS";
                childForm.Show();
            }
        }

        private void tsmiEqptTypes_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(EquipmentTypes));

            if (intOpen == 0)
            {
                EquipmentTypes childForm = new EquipmentTypes();
                childForm.MdiParent = this;
                childForm.Text = "EQUIPMENT TYPES";
                childForm.Show();
            }
        }

        private void tsmiServiceTypes_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(EquipmentSrvcTypes));

            if (intOpen == 0)
            {
                EquipmentSrvcTypes childForm = new EquipmentSrvcTypes();
                childForm.MdiParent = this;
                childForm.Text = "EQUIPMENT SERVICE TYPES";
                childForm.Show();
            }
        }

        private void tsmiVisitors_Click(object sender, EventArgs e)
        {
            //int intOpen = PSSClass.General.OpenForm(typeof(Visitors));

            //if (intOpen == 0)
            //{
            //    Visitors childForm = new Visitors();
            //    childForm.MdiParent = this;
            //    childForm.Text = "Visitors";
            //    childForm.Show();
            //}
        }

        private void tsmiScanRptQA_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(ScanFinRptQA));

            if (intOpen == 0)
            {
                ScanFinRptQA childForm = new ScanFinRptQA();
                childForm.MdiParent = this;
                childForm.Text = "REPORTS UNDER QA REVIEW";
                childForm.WindowState = FormWindowState.Normal;
                childForm.Show();
            }
        }

        private void tsmiGPLS_Click(object sender, EventArgs e)
        {
            Process.Start("http://pssql01:81"); //Login.aspx?username=" + LogIn.strUserID + "&pwd=" + LogIn.strPassword); 
        }

        private void tsmiDashBoardSter_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(SterDashboard));

            if (intOpen == 0)
            {
                SterDashboard childForm = new SterDashboard();
                childForm.MdiParent = this;
                childForm.Text = "STERILIZATION DASHBOARD";
                childForm.Show();
            }
        }

        private void lnkPCS_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://pssql01:82");
        }

        private void pnlTestFinder_Leave(object sender, EventArgs e)
        {
            pnlTestFinder.Visible = false;
        }

        private void tsbPR_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(PurchaseRequisitionX));

            if (intOpen == 0)
            {
                PurchaseRequisitionX childForm = new PurchaseRequisitionX();
                childForm.MdiParent = this;
                childForm.Text = "PURCHASE REQUISITION - TEST";
                childForm.Show();
            }
        }

        private void lnkPTS1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Users\" + LogIn.strUserID + @"\Desktop\gis_startSQL.mdb");
        }
    }
}
