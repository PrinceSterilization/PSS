//ControlPages.cs
// AUTHOR       : MARIA YOUNES
// TITLE        : Senior Programmer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Control Pages File Maintenance

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
using System.Drawing.Printing;

namespace PSS
{
    public partial class ControlPages : PSS.TemplateForm
    {
        byte nMode = 0;
        byte nProcess = 0;                                               
                                                                                       // MY 11/24/2014 
                                                                                       // 1 Original - Generate Control Page button clicked
                                                                                       // 2 Added    - Add button clicked
                                                                                       // 3 Voided   - Void button clicked  
 
        private int nRequestType;                                                      // MY 06/23/2015 1 = Deactive and Restore GBL   2 = Update Old Service code            
                                                                       
        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private string strFileAccess = "RO";
        byte nView = 0;                                                                // 0 as default, 1 if show all GBL/ServiceCode list under specified Book No
        
        private bool printClicked;
        string strGBLList;
        string strPrinterName;
        string strRptName;
        string strGroupCode;
        string strGroup;

        DataTable dtControlPageGBL = new DataTable();                                  // MY 11/18/2014 - Pop-up GridView ControlPageGBL table
        DataTable dtControlPageNumbers = new DataTable();                              // MY 11/18/2014 - Pop-up GridView ControlPageNumbers table
        DataTable dtGBLList = new DataTable();                                         // MY 01/08/2015 - Pop-up GridView GBLList to display all GBLs/service codes available for selected Book No
        DataTable dtRequestors = new DataTable();                                      // MY 06/24/2015 - Pop-up GridView Requestor query

        public ControlPages()
        {
            InitializeComponent();

            LoadRecords();
            LoadBookNos();
            LoadRequestors();
           
            BuildPrintItems();
            BuildSearchItems();

            strGroupCode = PSSClass.Users.UserGroupCode(LogIn.nUserID);
            strGroup = PSSClass.Users.UserDeptName(LogIn.nUserID);

            btnAdmin.Visible = false;
            txtTotPgNeeded.Enabled = false;
            txtPagesToAdd.Enabled = false;
            chkPrintAll.Enabled = false;
            btnAddPage.Enabled = false;
            btnVoid.Enabled = false;
            printClicked = false;
            
            bnMoveFirst.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveLast.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveNext.Click += new EventHandler(MoveRecordClickHandler);
            bnMovePrevious.Click += new EventHandler(MoveRecordClickHandler);

            tsbAdd.Click += new EventHandler(AddClickHandler);
            tsbEdit.Click += new EventHandler(EditClickHandler);
            tsbCancel.Click += new EventHandler(CancelClickHandler);
            tsbExit.Click += new EventHandler(CloseClickHandler);
            tsbSearch.Click += new EventHandler(SearchOKClickHandler);
            tsbFilter.Click += new EventHandler(SearchFilterClickHandler);
            tsbRefresh.Click += new EventHandler(RefreshClickHandler);
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        // MYounes 11/18/2014 - START: Load Data Modules
        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.QA.ControlPageMaster();
            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;            
            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            FileAccess();          
        }

        private void FileAccess()
        {  
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsbDelete.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = false;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbDelete.Enabled = false;
            }
        }
       
        private void LoadBookNos()
        {
            cboBookNo.Text = "";
            cboBookNo.DataSource = null;
            DataTable dt = new DataTable();
            dt = PSSClass.QA.BookNos();
            if (dt == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            cboBookNo.DataSource = dt;
            cboBookNo.DisplayMember = "BookNo";
            cboBookNo.ValueMember = "BookNo";
        }

        private void LoadRequestors()
        {
            dtRequestors = null;
            dtRequestors = PSSClass.QA.ControlPageEmployees();
            if (dtRequestors == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                return;
            }
            if (dtRequestors.Rows.Count == 0)
                return;

            dgvRequestors.DataSource = dtRequestors;
            StandardDGVSetting(dgvRequestors);
            dgvRequestors.Columns[0].Width = 195;
            dgvRequestors.Columns[1].Visible = false;
        }

        private void LoadData()
        {           
            ClearControls(this.pnlRecord);          
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtCmpyCode.Text = dgvFile.CurrentRow.Cells["CompanyCode"].Value.ToString();
            cboBookNo.Text = dgvFile.CurrentRow.Cells["BookNo"].Value.ToString();   
            txtControlPageID.Text = dgvFile.CurrentRow.Cells["ControlPageID"].Value.ToString();
            txtGBLNo.Text = dgvFile.CurrentRow.Cells["PSSNo"].Value.ToString();
            txtServiceCode.Text = dgvFile.CurrentRow.Cells["ServiceCode"].Value.ToString();            
            txtTotalPage.Text = dgvFile.CurrentRow.Cells["TotalPages"].Value.ToString();
            btnGenerate.Enabled = false;
            nView = 0;
            LoadControlPageGBL(Convert.ToByte(nView), cboBookNo.Text, Convert.ToInt64(txtControlPageID.Text), Convert.ToInt64(txtGBLNo.Text), Convert.ToInt16(txtServiceCode.Text));
            LoadControlPageNumbers(Convert.ToInt64(txtControlPageID.Text));
            cboBookNo.Enabled = false;

            btnAddPage.Enabled = false;
            btnVoid.Enabled = false;
            btnAdmin.Visible = false;

            CheckPrintEnabled();
        }

        private void LoadControlPageGBL(Byte cView, string cBookNo, Int64 cControlPageID, Int64 cGBLNo, int cServiceCode) 
        {
            dtControlPageGBL = null;
            dtControlPageGBL = PSSClass.QA.ControlPagePSS(cView, cBookNo, cControlPageID, cGBLNo, cServiceCode);
            bsGBLSelection.DataSource = dtControlPageGBL;
            dgvGBLSelection.DataSource = bsGBLSelection;                           
        }

        private void LoadControlPageNumbers(Int64 cControlPageID)
        {
            dtControlPageNumbers = null;
            dtControlPageNumbers = PSSClass.QA.ControlPageNumbers(cControlPageID);
            bsCPNumbers.DataSource = dtControlPageNumbers;           
            dgvControlPageNumbers.DataSource = bsCPNumbers;      
        }
        
        private void LoadGBLList(string cBookNo)
        {
            dtGBLList = null;
            dtGBLList = PSSClass.QA.PSSList(cBookNo);            
            bsGBLList.DataSource = dtGBLList;
            dgvGBLList.DataSource = bsGBLList;
            if (dgvGBLList.Rows.Count == 0)
            {
                MessageBox.Show("There are no more GBLs left to process for this Book Number!" + Environment.NewLine + "Please choose another Book Number.");
                return;
            }
        }

        // MYounes 11/18/2014 - END: Load Data Modules

        private void CheckPrintEnabled()
        {
            // Check if detail records already printed
            int nPrinted = 0;
            int intUserID = LogIn.nUserID;
            for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)
            {
                if (dgvControlPageNumbers.Rows[j].Cells["Printed"].Value.ToString() == "True")
                {
                    nPrinted++;
                }
            }
            //1029  Gabrielle Mastej
            //1021  Phyllis DeCilla
            //1014  German Yemets
            // 270  Michael Pannullo
            // 482  Chad Zapata
            // 114  Jozef Mastej
            if (intUserID == 1029 || intUserID == 1021 || intUserID == 1014 || intUserID == 270 || intUserID == 482 || intUserID == 114)
            {
                btnPrintTo45.Visible = true;
                btnPrintToQA45.Visible = true;
            }
            else
            {
                btnPrintTo45.Visible = false;
                btnPrintToQA45.Visible = false;
            }

            if (nPrinted == dgvControlPageNumbers.Rows.Count)
            {
                btnPrintTo122.Enabled = false;
                btnPrintTo16.Enabled = false;
                btnPrintTo45.Enabled = false;
                btnPrintToQA45.Enabled = false;
                // btnPrintTo45.Visible = false;
                //if (btnPrintTo45.Visible == true)
                //{
                //    btnPrintTo45.Enabled = false;
                //}
            }
            else
            {
                btnPrintTo122.Enabled = true;
                btnPrintTo16.Enabled = true;
                btnPrintTo45.Enabled = true;
                btnPrintToQA45.Enabled = true;
                //if (btnPrintTo45.Visible == true)
                //{
                //    btnPrintTo45.Enabled = true;
                //}
            }

            
            

        }

        // MYounes 11/20/2014 - START: Data Grid View Setup
        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["CompanyCode"].HeaderText = "Cmpy Code";
            dgvFile.Columns["DateCreated"].HeaderText = "Date Created";
            dgvFile.Columns["BookNo"].HeaderText = "Book No";
            dgvFile.Columns["ControlPageID"].HeaderText = "Control Page ID";
            dgvFile.Columns["PSSNo"].HeaderText = "PSS No";
            dgvFile.Columns["ServiceCode"].HeaderText = "Service Code";
            dgvFile.Columns["SponsorID"].HeaderText = "Sponsor ID";
            dgvFile.Columns["SponsorName"].HeaderText = "Sponsor Name";
            dgvFile.Columns["CreatedBy"].HeaderText = "Created By";  
            dgvFile.Columns["TotalPages"].HeaderText = "Total Pages";
            dgvFile.Columns["ReturnedPages"].HeaderText = "Returned Pages";
            dgvFile.Columns["OriginalPages"].HeaderText = "Original Pages";
            dgvFile.Columns["AddedPages"].HeaderText = "Added Pages";
            dgvFile.Columns["VoidedPages"].HeaderText = "Voided Pages";
            dgvFile.Columns["StartNo"].HeaderText = "Start Page";
            dgvFile.Columns["EndNo"].HeaderText = "End Page";
            dgvFile.Columns["LastCPNo"].HeaderText = "Last Page Created";
            dgvFile.Columns["DateCreated"].Width = 100;
            dgvFile.Columns["BookNo"].Width = 80;
            dgvFile.Columns["ControlPageID"].Width = 100;
            dgvFile.Columns["PSSNo"].Width = 80;
            dgvFile.Columns["ServiceCode"].Width = 100;
            dgvFile.Columns["SponsorID"].Width = 80;
            dgvFile.Columns["SponsorName"].Width = 300;
            dgvFile.Columns["CreatedBy"].Width = 80;
            dgvFile.Columns["TotalPages"].Width = 80;
            dgvFile.Columns["OriginalPages"].Width = 80;
            dgvFile.Columns["AddedPages"].Width = 80;
            dgvFile.Columns["VoidedPages"].Width = 80; 
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["BookNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ControlPageID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["PSSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["CreatedBy"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["TotalPages"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ReturnedPages"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["OriginalPages"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["AddedPages"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["VoidedPages"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["StartNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["EndNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;   
            dgvFile.Columns["LastCPNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;   
        }

        private void DataGridControlPageGBLListSetting()
        {
            dgvGBLList.EnableHeadersVisualStyles = false;
            dgvGBLList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGBLList.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvGBLList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;           
            dgvGBLList.Columns["PSSNo"].HeaderText =  "PSS No.";
            dgvGBLList.Columns["ServiceCode"].HeaderText = "Service Code";            
            dgvGBLList.Columns["PSSNo"].Width = 100;
            dgvGBLList.Columns["ServiceCode"].Width = 100;           
            dgvGBLList.Columns["PSSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGBLList.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void DataGridControlPageGBLSetting()
        {    
            dgvGBLSelection.EnableHeadersVisualStyles = false;
            dgvGBLSelection.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGBLSelection.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvGBLSelection.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvGBLSelection.Columns["ControlPageID"].HeaderText = "Control Page ID";
            dgvGBLSelection.Columns["PSSNo"].HeaderText =  "PSS No.";
            dgvGBLSelection.Columns["ServiceCode"].HeaderText = "Service Code";            
            dgvGBLSelection.Columns["ControlPageID"].Width = 100;
            dgvGBLSelection.Columns["PSSNo"].Width = 100;
            dgvGBLSelection.Columns["ServiceCode"].Width =100;
            dgvGBLSelection.Columns["ControlPageID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGBLSelection.Columns["PSSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvGBLSelection.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;            
        }

        private void DataGridControlPageNumbersSetting()
        {
            dgvControlPageNumbers.EnableHeadersVisualStyles = false;
            dgvControlPageNumbers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvControlPageNumbers.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvControlPageNumbers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvControlPageNumbers.Columns["ControlPageNo"].HeaderText = "Sequence Number";
            dgvControlPageNumbers.Columns["Printed"].HeaderText = "Printed";
            dgvControlPageNumbers.Columns["Voided"].HeaderText = "Voided";
            dgvControlPageNumbers.Columns["PageStatus"].HeaderText = "Page Status";
            dgvControlPageNumbers.Columns["PageStatusDesc"].HeaderText = "Page Status Description";
            dgvControlPageNumbers.Columns["StatusReason"].HeaderText = "Reason";
            dgvControlPageNumbers.Columns["Selector"].HeaderText = "Select";
            dgvControlPageNumbers.Columns["ControlPageID"].HeaderText = "Control Page ID";           
            dgvControlPageNumbers.Columns["PrintedByID"].HeaderText = "Printed By ID";
            dgvControlPageNumbers.Columns["PrintedBy"].HeaderText = "Printed By";
            dgvControlPageNumbers.Columns["DatePrinted"].HeaderText = "Date Printed";
            dgvControlPageNumbers.Columns["VoidedByID"].HeaderText = "Voided By ID";
            dgvControlPageNumbers.Columns["VoidedBy"].HeaderText = "Voided By";
            dgvControlPageNumbers.Columns["DateVoided"].HeaderText = "DateVoided";
            dgvControlPageNumbers.Columns["ControlPageNo"].Width = 100;
            dgvControlPageNumbers.Columns["PageStatusDesc"].Width = 100;
            dgvControlPageNumbers.Columns["StatusReason"].Width = 328;
            dgvControlPageNumbers.Columns["Printed"].Width = 60;
            dgvControlPageNumbers.Columns["Voided"].Width = 60;
            dgvControlPageNumbers.Columns["Selector"].Width = 60;
            dgvControlPageNumbers.Columns["ControlPageNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight;
            dgvControlPageNumbers.Columns["ControlPageID"].Visible = false;
            dgvControlPageNumbers.Columns["PageStatus"].Visible = false;
            dgvControlPageNumbers.Columns["PrintedByID"].Visible = false;
            dgvControlPageNumbers.Columns["PrintedBy"].Visible = false;
            dgvControlPageNumbers.Columns["DatePrinted"].Visible = false;
            dgvControlPageNumbers.Columns["VoidedByID"].Visible = false;
            dgvControlPageNumbers.Columns["VoidedBy"].Visible = false;
            dgvControlPageNumbers.Columns["DateVoided"].Visible = false;  
        }

        // MYounes 11/20/2014 - END: Data Grid View Setup

        
        // MYounes 11/20/2014 - START: Misc Routines
            
        private void UpdateControlPageLock(bool cisLocked)
        {

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@isLocked", cisLocked);          
            
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdateControlPageLock";
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

        private void UpdateControlPageReprint(Int64 cCtrlPageID, string cCtrlPageNo)
        {    
            if (dgvControlPageNumbers.Rows.Count != 0)
            {
                for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)                                  // DataGridView Detail Loop
                {             
                    if (dgvControlPageNumbers.Rows[j].Cells["Selector"].Value.ToString() == "True")
                    {
                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@ControlPageID", cCtrlPageID);
                        sqlcmd.Parameters.AddWithValue("@ControlPageNo", cCtrlPageNo);
                                  
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spUpdateControlPageReprint";
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
                }               
            }           
        }

        private void UpdateControlPagePrintStatus()
        {
            if (dgvControlPageNumbers.Rows.Count != 0)
            {
                for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)                                  // DataGridView Detail Loop
                {
                    if (dgvControlPageNumbers.Rows[j].Cells["Selector"].Value.ToString() == "True")
                    {
                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@ControlPageID", Convert.ToInt64(txtControlPageID.Text));
                        sqlcmd.Parameters.AddWithValue("@ControlPageNo", dgvControlPageNumbers.Rows[j].Cells["ControlPageNo"].Value);
                        sqlcmd.Parameters.AddWithValue("@Printed", true);
                        sqlcmd.Parameters.AddWithValue("@PrintedByID", LogIn.nUserID);
                        sqlcmd.Parameters.AddWithValue("@DatePrinted", DateTime.Now);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spUpdateControlPagePrintStatus";
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
                }
            }
        }
        private void UpdateControlPageSelected(byte cMode, byte cStatus)                                        // I'll rework this routine later 
        {
            if (dgvControlPageNumbers.Rows.Count != 0)
            {                
                if (cMode == 0)                                                                                 // cMode 0 = Do all
                {
                    for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)                                  // DataGridView Detail Loop all
                    {                       
                        UpdateControlPageSelector(cStatus, dgvControlPageNumbers.Rows[j].Cells["ControlPageNo"].Value.ToString());                       
                    }
                }
                else                
                {
                    for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)                                  // DataGridView Detail loop thru selected
                    {
                        if (dgvControlPageNumbers.Rows[j].Cells["Selector"].Value.ToString() == "True")
                        {                           
                            UpdateControlPageSelector(cStatus, dgvControlPageNumbers.Rows[j].Cells["ControlPageNo"].Value.ToString());
                        }
                    }
                }

            }
        }

        private void UpdateControlPageSelector(byte cStatus, string cPageNo)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@ControlPageID", Convert.ToInt64(txtControlPageID.Text));
            sqlcmd.Parameters.AddWithValue("@ControlPageNo", cPageNo);
            sqlcmd.Parameters.AddWithValue("@Selector", cStatus);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdateControlPageSelector";
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
               
        private void CreateControlPageNumbers()
        {
            // Create GBLList
            string oldGBL;

            if (dgvGBLSelection.Rows.Count != 0)
            {
                oldGBL = dgvGBLSelection.Rows[0].Cells["PSSNo"].Value.ToString();
                strGBLList = Convert.ToInt32(dgvGBLSelection.Rows[0].Cells["PSSNo"].Value).ToString("000000");
                
                for (int i = 1; i < dgvGBLSelection.Rows.Count; i++)
                {

                    if (dgvGBLSelection.Rows[i].Cells["PSSNo"].Value.ToString().Equals(oldGBL))
                    {
                    }
                    else
                    {
                        strGBLList = strGBLList + "," + Convert.ToInt32(dgvGBLSelection.Rows[i].Cells["PSSNo"].Value).ToString("000000");
                        oldGBL = dgvGBLSelection.Rows[0].Cells["PSSNo"].Value.ToString();
                    }
                }
            }

            // Get new Control Page ID
            txtControlPageID.Text = PSSClass.General.NewCtrlPageID("ControlPagePSS", "ControlPageID").ToString();

            // Insert new Control Page records
            for (int i = 0; i < dgvGBLSelection.Rows.Count; i++)
            {
                if (Convert.ToInt16(txtTotPgNeeded.Text) > 0)
                {
                    InsertControlPageGBLRecord(i);                                                                      
                    nProcess = 1;                                                                                       // Original; btnGenerateControlPages was clicked           
                }
            }

            txtCmpyCode.Text = "P";
            txtGBLNo.Text = dgvGBLSelection.Rows[0].Cells["PSSNo"].Value.ToString();
            txtServiceCode.Text = dgvGBLSelection.Rows[0].Cells["ServiceCode"].Value.ToString();

            // Insert new Control Page sequence records
            for (int j = 0; j < Convert.ToInt16(txtTotPgNeeded.Text); j++)
            {
                InsertControlPageNumberRecord(nProcess);                                                              
            }                                      
                      
        }

        private void AddNewControlPages(byte cProcess)
        {      
            int totPage;

            if (txtPagesToAdd.Text.Trim() != "")
            {
                totPage = Convert.ToInt16(txtPagesToAdd.Text);
 
                for (int j = 0; j < totPage; j++)
                {
                    InsertControlPageNumberRecord(cProcess);                                                           // Insert new control page sequence 0 for first detail grid line and process triggered
                }                           
                MessageBox.Show("New Control Page Sequence number(s) created!");
            }
            
        }

        private void InsertControlPageGBLRecord(int cRow)
        {              
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@CmpyCode", "P");
            sqlcmd.Parameters.AddWithValue("@ControlPageID", Convert.ToInt64(txtControlPageID.Text));
            sqlcmd.Parameters.AddWithValue("@PSSNo", dgvGBLSelection.Rows[cRow].Cells["PSSNo"].Value);
            sqlcmd.Parameters.AddWithValue("@ServiceCode", dgvGBLSelection.Rows[cRow].Cells["ServiceCode"].Value);
            sqlcmd.Parameters.AddWithValue("@BookNo", cboBookNo.Text); //Convert.ToInt16(
            sqlcmd.Parameters.AddWithValue("@TotalPages", Convert.ToInt16(txtTotPgNeeded.Text));
            sqlcmd.Parameters.AddWithValue("@PSSList", strGBLList);
            sqlcmd.Parameters.AddWithValue("@CreatedByID", LogIn.nUserID);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID); 

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddControlPagePSS";
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

        private void InsertControlPageNumberRecord(byte cProcess)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@ControlPageID", Convert.ToInt64(txtControlPageID.Text)); 
            sqlcmd.Parameters.AddWithValue("@PageStatus", cProcess);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);        

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddControlPageNumber";
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

        private void UpdateGBLTotalPages()
        {
            if (dgvGBLSelection.Rows.Count != 0)
            {            
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@ControlPageID", Convert.ToInt64(txtControlPageID.Text));
                sqlcmd.Parameters.AddWithValue("@NewPages", Convert.ToInt16(txtPagesToAdd.Text));  
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);                             

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdateControlGBLPageCount";
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

        }

        private void UpdateControlPageStatus(int cPrintStatus)
        {
            if (dgvGBLSelection.Rows.Count != 0)
            {                
                for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)                                  // DataGridView Detail Loop
                {                           
                    if (dgvControlPageNumbers.Rows[j].Cells["Selector"].Value.ToString() == "True")           // Only when checked                           
                                
                    {                               
                        SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                        SqlCommand sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@ControlPageID", Convert.ToInt64(txtControlPageID.Text));
                        sqlcmd.Parameters.AddWithValue("@ControlPageNo", dgvControlPageNumbers.Rows[j].Cells["ControlPageNo"].Value);
                        sqlcmd.Parameters.AddWithValue("@PageStatus", cPrintStatus);
                        sqlcmd.Parameters.AddWithValue("@StatusReason", dgvControlPageNumbers.Rows[j].Cells["StatusReason"].Value);

                        if (cPrintStatus == 3)                                                             // If Voided Page
                        {
                            sqlcmd.Parameters.AddWithValue("@Voided", 1);                                 
                            sqlcmd.Parameters.AddWithValue("@VoidedByID", LogIn.nUserID);  
                            sqlcmd.Parameters.AddWithValue("@DateVoided", DateTime.Now);
                        }
                        else
                        {
                            sqlcmd.Parameters.AddWithValue("@Voided", dgvControlPageNumbers.Rows[j].Cells["Voided"].Value);
                            sqlcmd.Parameters.AddWithValue("@VoidedByID", dgvControlPageNumbers.Rows[j].Cells["VoidedByID"].Value);
                            sqlcmd.Parameters.AddWithValue("@DateVoided", dgvControlPageNumbers.Rows[j].Cells["DateVoided"].Value);
                        }

                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);                             
                                
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spUpdateControlPageStatus";
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
                }

                // Redisplay both view grids  
                txtControlPageID.Text = dgvGBLSelection.CurrentRow.Cells["ControlPageID"].Value.ToString();
                txtGBLNo.Text = dgvGBLSelection.CurrentRow.Cells["PSSNo"].Value.ToString();
                txtServiceCode.Text = dgvGBLSelection.CurrentRow.Cells["ServiceCode"].Value.ToString();
                LoadControlPageGBL(Convert.ToByte(nView), cboBookNo.Text, Convert.ToInt64(txtControlPageID.Text), Convert.ToInt64(txtGBLNo.Text), Convert.ToInt16(txtServiceCode.Text));
                LoadControlPageNumbers(Convert.ToInt64(txtControlPageID.Text));

                if (cPrintStatus == 3)                  
                {
                    MessageBox.Show("Selected Control Page sequence page(s) voided!");                        
                }                                     
            }
            
        }
        
        // MYounes 11/20/2014 - END: Misc Routines

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
            //ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            //items[0] = new ToolStripMenuItem();
            //items[0].Name = "Control Page Sheet(s)";
            //items[0].Text = "Sorted by Book No";
            //items[0].Click += new EventHandler(PrintControlPageListClickHandler);

            //tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.QA.ControlPageMaster();

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

        private void dgvCellChangedHandler(object sender, EventArgs e)
        {
            try
            {
                nIndex = dgvFile.CurrentCell.ColumnIndex;

                tsddbSearch.DropDownItems[nIndex].Select();
                tslSearchData.Text = tsddbSearch.DropDownItems[nIndex].Text;
                tstbSearchField.Text = tsddbSearch.DropDownItems[nIndex].Name;
            }
            catch
            { }
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
                    bsFile.Filter = "BookNo<>0";
                    PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                    dgvFile.Select();
                    if (pnlRecord.Visible == true)
                        LoadData();
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
                    dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                    tsbRefresh.Enabled = true;
                }
                catch { }
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            bsFile.Filter = "BookNo<>'0'";
            tsbRefresh.Enabled = false; 
            tstbSearch.Text = "";            
            
        }

       
        private void CancelClickHandler(object sender, EventArgs e)
        {
            btnAdmin.Enabled = true;
            CancelSave();
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
            LoadData();
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
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
            nMode = 1;
            nView = 0;
            cboBookNo.Enabled = true;
            btnAddSelected.Enabled = true;
            btnDelSelected.Enabled = true;
            btnGenerate.Enabled = true;
            txtTotalPage.Enabled = true;

            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
          
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            tsbSave.Enabled = false;
            txtTotPgNeeded.Enabled = true;
            dgvControlPageNumbers.Enabled = false;
            txtPagesToAdd.Enabled = false;
            btnPrintTo122.Enabled = false;
            btnPrintTo16.Enabled = false;
            btnAddPage.Enabled = false;
            btnVoid.Enabled = false;
            btnAdmin.Enabled = false;
            btnPrintTo45.Enabled = false;
            btnPrintToQA45.Enabled = false;
            //if (btnPrintTo45.Visible == true)
            //{
            //    btnPrintTo45.Enabled = false;
            //    btnPrintTo45.Visible = false;
            //}
            dtGBLList.Rows.Clear();
            dtControlPageGBL.Rows.Clear();
            dtControlPageNumbers.Rows.Clear();        
                      
            cboBookNo.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            if (pnlRecord.Visible == false)
                LoadData();
            OpenControls(this.pnlRecord, true);
            UpdateControlPageSelected(0, 0);                                            // full loop and selector will be unchecked
            btnClose.Visible = false;
            cboBookNo.Enabled = false;
            txtTotPgNeeded.Enabled = false;
            btnAddSelected.Enabled = false;
            btnDelSelected.Enabled = false;
            btnGenerate.Enabled = false;

            dgvControlPageNumbers.Enabled = true;
            txtPagesToAdd.Enabled = true;

            tsbSave.Enabled = false;
            tsbCancel.Enabled = true; //Revised 3/31/2016
            btnAdmin.Visible = false;

            if (strGroupCode == "IT")
            {
                btnAdmin.Visible = true;
                btnVoid.Enabled = true;
                btnAddPage.Enabled = true;
                btnResetPrint.Visible = true;
            }
            //if (strGroupCode == "QA")
            //{                
            //    btnVoid.Enabled = true;
            //    btnAddPage.Enabled = true;
            //}

            if (strFileAccess == "RW" || strFileAccess == "FA")
            {
                btnVoid.Enabled = true;
                btnAddPage.Enabled = true;
            }       
        }

        private void DeleteRecord()
        {           
        }

        private void SaveRecord()
        {
        }
        
        private void CancelSave()
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
            ClearControls(this);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);           
            nMode = 0;
            tsbSave.Enabled = false;
            tsbCancel.Enabled = false;
        }       
    
        private void ControlPages_Load(object sender, EventArgs e)
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

            // For testing Only
            //LogIn.nUserID = 288;  -- Shiri
            //strGroupCode = "QA";

            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "ControlPages");           

            FileAccess();
            // Set up dgvGBLlist column headers
            bsGBLList.DataSource = dtGBLList;
            dgvGBLList.DataSource = bsGBLList;

            dtGBLList.Columns.Add("PSSNo", typeof(Int64));
            dtGBLList.Columns.Add("ServiceCode", typeof(int));
            DataGridControlPageGBLListSetting();

            // Set up dgvGBLSelection column headers
            bsGBLSelection.DataSource = dtControlPageGBL;
            dgvGBLSelection.DataSource = bsGBLSelection;           

            dtControlPageGBL.Columns.Add("ControlPageID", typeof(Int64));
            dtControlPageGBL.Columns.Add("PSSNo", typeof(Int64));
            dtControlPageGBL.Columns.Add("ServiceCode", typeof(int));
            DataGridControlPageGBLSetting();   

            // Set up dgvControlPageNumbers column headers  
            bsCPNumbers.DataSource = dtControlPageNumbers;
            dgvControlPageNumbers.DataSource = bsCPNumbers;

            dtControlPageNumbers.Columns.Add("ControlPageID", typeof(Int64));
            dtControlPageNumbers.Columns.Add("ControlPageNo", typeof(string));
            dtControlPageNumbers.Columns.Add("Printed", typeof(bool));
            dtControlPageNumbers.Columns.Add("Voided", typeof(bool));
            dtControlPageNumbers.Columns.Add("PageStatus", typeof(Int16));
            dtControlPageNumbers.Columns.Add("PageStatusDesc", typeof(string));
            dtControlPageNumbers.Columns.Add("StatusReason", typeof(string));
            dtControlPageNumbers.Columns.Add("Selector", typeof(bool));         
            dtControlPageNumbers.Columns.Add("PrintedByID", typeof(Int16));
            dtControlPageNumbers.Columns.Add("PrintedBy", typeof(string));
            dtControlPageNumbers.Columns.Add("DatePrinted", typeof(DateTime));
            dtControlPageNumbers.Columns.Add("VoidedByID", typeof(Int16));
            dtControlPageNumbers.Columns.Add("VoidedBy", typeof(string));
            dtControlPageNumbers.Columns.Add("DateVoided", typeof(DateTime));
            DataGridControlPageNumbersSetting();            
        }

        private void ControlPages_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    if (nMode == 0 && strFileAccess != "RO")
                    {
                        AddEditMode(true); AddRecord(); 
                    }
                    break;

                case Keys.F3:
                    if (nMode == 0 && strFileAccess != "RO")
                    {
                        AddEditMode(true); EditRecord(); 
                    }
                    break;

                //case Keys.F4:
                //    if (nMode == 0)
                //    {
                //        DeleteRecord();
                //    }
                //    break;

                case Keys.F5:
                    if (nMode != 0)
                        SaveRecord();
                    break;

                case Keys.F6:
                    if (nMode != 0)
                        CancelSave();
                    break;

                case Keys.F7:
                    if (nMode == 0)
                        tsddbPrint.ShowDropDown();
                    break;

                case Keys.F8:
                    if (nMode == 0)
                        tsddbSearch.ShowDropDown();
                    break;
                case Keys.F9:
                    if (nMode == 0)
                        SearchOKClickHandler(null, null);
                    break;

                case Keys.F10:
                    if (nMode == 0)
                        SearchFilterClickHandler(null, null);
                    break;

                case Keys.F11:
                    if (nMode == 0)
                        RefreshClickHandler(null, null);
                    break;

                case Keys.F12:
                    if (nMode != 0)
                    {
                        DialogResult dReply = new DialogResult();
                        dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dReply == DialogResult.No)
                            return;
                    }
                    this.Close();
                    break;

                default:
                    break;
            }
        }

        // MY 11/18/2014 - START: Panel Mouse Events
        private void pnlRecord_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void pnlRecord_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void pnlRecord_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }
        // MY 11/18/2014 - END: Panel Mouse events

        
        private void txtSeqNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
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
         
        private void btnClose_Click(object sender, EventArgs e)
        {
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;      
            dgvFile.Focus();
        }

        private void cboBookNo_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboBookNo.Text.Trim() != "")
                {
                    if (nMode == 1)
                    {
                        LoadGBLList(cboBookNo.Text); //Convert.ToInt16(
                    }
                    else
                    {
                        if (dgvGBLSelection.Rows.Count > 0)
                        {
                            nView = 0;
                            txtControlPageID.Text = dgvGBLSelection.Rows[0].Cells["ControlPageiD"].Value.ToString();
                            txtGBLNo.Text = dgvGBLSelection.Rows[0].Cells["PSSNo"].Value.ToString();
                            txtServiceCode.Text = dgvGBLSelection.Rows[0].Cells["ServiceCode"].Value.ToString();
                            LoadControlPageGBL(Convert.ToByte(nView), cboBookNo.Text, Convert.ToInt64(txtControlPageID.Text), Convert.ToInt64(txtGBLNo.Text), Convert.ToInt16(txtServiceCode.Text));
                            LoadControlPageNumbers(Convert.ToInt64(txtControlPageID.Text));
                           
                        }
                        else
                        {
                            txtControlPageID.Text = "";
                            txtGBLNo.Text = "";
                            txtServiceCode.Text = "";                            
                            dtControlPageGBL.Rows.Clear();
                            dtControlPageNumbers.Rows.Clear();                         
                        }
                    }
                }
            }
            catch { }           
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (cboBookNo.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Book Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboBookNo.Focus();
                return;
            }
            else
           {
               if (dgvGBLSelection.Rows.Count == 0)
               {
                    MessageBox.Show("No PSS/Service Codes found which are needed to generate Control Pages. Please make your PSS selections first!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cboBookNo.Focus();
                    return;
               }
               else
               {           
                    bool isLocked = PSSClass.DataEntry.IsLocked("ControlPageLock", "isLocked");

                    if (isLocked)
                    {
                        MessageBox.Show("Control Page Table is locked! Please wait and try again.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        cboBookNo.Focus();
                        return;
                    }

                    if (txtTotPgNeeded.Text != "")                            
                    {
                        nMode = 0;
                        nView = 3;      // redisplay using only control page id
                        UpdateControlPageLock(true);
                        CreateControlPageNumbers();
                        dtGBLList.Rows.Clear();
                        dtControlPageGBL.Rows.Clear();
                        LoadControlPageGBL(Convert.ToByte(nView), cboBookNo.Text, Convert.ToInt64(txtControlPageID.Text), Convert.ToInt64(txtGBLNo.Text), Convert.ToInt16(txtServiceCode.Text));
                        LoadControlPageNumbers(Convert.ToInt64(txtControlPageID.Text));
                        UpdateControlPageLock(false);
                        btnClose.Visible = true;
                        tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsbCancel.Enabled = false;
                        btnClose.Enabled = true;
                        btnGenerate.Enabled = false;

                        dgvControlPageNumbers.Enabled = false;
                        txtPagesToAdd.Enabled = true;
                        btnPrintTo122.Enabled = true;
                        btnPrintTo16.Enabled = true;
                        btnAddPage.Enabled = true;
                        btnVoid.Enabled = false;
                        btnAdmin.Enabled = true;    
                        chkPrintAll.Checked = false;
                        if (btnPrintTo45.Visible == true)
                        {
                            btnPrintTo45.Enabled = true;
                        }
                        if (btnPrintToQA45.Visible == true)
                        {
                            btnPrintToQA45.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter Total Pages needed!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dgvGBLSelection.Focus();
                    }             
                }
            }
                   
        }

        private void dgvGBLSelection_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (nMode == 1)
            {
                btnGenerate.Enabled = true;
            }

            if (txtControlPageID.Text.Trim() != "")
            {
                LoadControlPageNumbers(Convert.ToInt64(txtControlPageID.Text));
            }
        }
        
        private void dgvControlPageNumbers_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (printClicked)
            {
                if (e.ColumnIndex < 4)
                    e.Cancel = true;
            }
            else
            {
                if (nMode == 0)
                    e.Cancel = true;
                else if (e.ColumnIndex < 4)
                    e.Cancel = true;
            }
        }

        private void btnAddPage_Click(object sender, EventArgs e)
        {
            if (txtPagesToAdd.Text.Trim() == "")
            {
                MessageBox.Show("Please specify the number of pages you want to add!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPagesToAdd.Focus();
                return;
            }

            if (Convert.ToInt16(txtPagesToAdd.Text) < 1)
            {
                MessageBox.Show("Additional Pages can't be less than 1!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPagesToAdd.Text = "";
                txtPagesToAdd.Focus();
                return;
            }

            if (Convert.ToInt16(txtPagesToAdd.Text) > 20)
            {
                MessageBox.Show("Additional Pages can't be more than 20!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPagesToAdd.Text = "";
                txtPagesToAdd.Focus();
                return;
            }
                           
            nProcess = 2;
            btnPrintTo122.Enabled = true;
            btnPrintTo16.Enabled = true;

            // Add Page    
            UpdateControlPageSelected(0, 0);                                                                        // Loop and clear all previously selected records
            AddNewControlPages(nProcess);
            UpdateGBLTotalPages();
            LoadControlPageNumbers(Convert.ToInt64(txtControlPageID.Text));
            txtPagesToAdd.Text = "";                                                                                // clear this; user might accidentally hit Add again
            tsbEdit.Enabled = true;
            tsbCancel.Enabled = false;
            tsbSave.Enabled = false;
            btnClose.Visible = true;            
        }

        private void PrintReport(string cPrinterName, string cRptName)
        {
            int intUserID = 0;

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            ReportDocument crDoc = new ReportDocument();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;            

            intUserID = LogIn.nUserID;    

            crDoc.Load(cRptName);
            sqlcmd = new SqlCommand("spRptControPageSheet", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@ControlPageID", Convert.ToInt64(txtControlPageID.Text));

            sqldr = sqlcmd.ExecuteReader();

            DataTable dTable = new DataTable();

            try
            {
                dTable.Load(sqldr);
                sqlcnn.Dispose();
            }
            catch
            {
                sqlcnn.Dispose();
            }
            crDoc.SetDataSource(dTable);

            System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();

            try
            {
                printerSettings.PrinterName = cPrinterName;
                crDoc.PrintToPrinter(printerSettings, new PageSettings(), false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            finally 
            {
                crDoc.Dispose();
            }     

            nMode = 0;
            UpdateControlPageSelected(1, 1);                                                                      // partial loop and selector will be checked
            UpdateControlPagePrintStatus();
            UpdateControlPageSelected(0, 0);                                                                      // Clear all selected

            txtPagesToAdd.Text = "";
            LoadControlPageNumbers(Convert.ToInt64(txtControlPageID.Text));
            cboBookNo.Enabled = false;
            
        }

        private void btnPrintTo16_Click(object sender, EventArgs e)
        {
            // This print-out is for Bldg 16, QA printer
            //strPrinterName = @"\\psapp01.corp.princesterilization.com\PSS QA";////PSSClass.QA.PrinterName(7);
            strPrinterName = @"\\psapp01\Kyocera QA 2";////PSSClass.QA.PrinterName(7); 
            strRptName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ControlPageForm_16.rpt";
            //strRptName = @"S:\IT Files\PSS\" + "ControlPageForm_16.rpt";            
            PrintReport(strPrinterName, strRptName);
            btnPrintTo45.Enabled = false;
            btnPrintTo122.Enabled = false;
            btnPrintTo16.Enabled = false;
            btnPrintToQA45.Enabled = false;
        }

        private void btnPrintTo122_Click(object sender, EventArgs e)
        {
            // This print-out is for Bldg 122, QA printer         
            strPrinterName = @"\\psapp01.corp.princesterilization.com\Sterilization Kyocera";////PSSClass.QA.PrinterName(7); 
            strRptName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ControlPageForm_16.rpt";
            PrintReport(strPrinterName, strRptName);
            btnPrintTo45.Enabled = false;
            btnPrintTo122.Enabled = false;
            btnPrintTo16.Enabled = false;
            btnPrintToQA45.Enabled = false;

        }
        //private void PrintReport1(string cPrinterName, string cRptName)
        //{
        //    int intUserID = 0;

        //    SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
        //    if (sqlcnn == null)
        //    {
        //        MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //        return;
        //    }

        //    ReportDocument crDoc = new ReportDocument();
        //    SqlCommand sqlcmd = new SqlCommand();
        //    SqlDataReader sqldr;

        //    intUserID = LogIn.nUserID;

        //    crDoc.Load(cRptName);
        //    sqlcmd = new SqlCommand("spRptControPageSheet", sqlcnn);
        //    sqlcmd.CommandType = CommandType.StoredProcedure;

        //    sqlcmd.Parameters.AddWithValue("@ControlPageID", 1160);

        //    sqldr = sqlcmd.ExecuteReader();

        //    DataTable dTable = new DataTable();

        //    try
        //    {
        //        dTable.Load(sqldr);
        //        sqlcnn.Dispose();
        //    }
        //    catch
        //    {
        //        sqlcnn.Dispose();
        //    }
        //    crDoc.SetDataSource(dTable);

        //    System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();

        //    try
        //    {
        //        printerSettings.PrinterName = cPrinterName;
        //        crDoc.PrintToPrinter(printerSettings, new PageSettings(), false);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    nMode = 0;
        //    //UpdateControlPageSelected(1, 1);                                                                      // partial loop and selector will be checked
        //    //UpdateControlPagePrintStatus();
        //    //UpdateControlPageSelected(0, 0);                                                                      // Clear all selected

        //    txtPagesToAdd.Text = "";
        //    LoadControlPageNumbers(1160);
        //    cboBookNo.Enabled = false;
        //    crDoc.Dispose();
        //}

        private void btnPrintTo45_Click(object sender, EventArgs e)
        {
            try
            {
                // This print-out is for 45 Office
                //strPrinterName = @"\\psapp04.corp.princesterilization.com\45 Office C3320i";
                strPrinterName = @"\\psapp01\45 Office Printer";
                //strPrinterName = @"\\psapp01\Toshiba 5005ac";
                strRptName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ControlPageForm_16.rpt";
                PrintReport(strPrinterName, strRptName);
                btnPrintTo45.Enabled = false;
                btnPrintTo122.Enabled = false;
                btnPrintTo16.Enabled = false;
                btnPrintToQA45.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void btnVoid_Click(object sender, EventArgs e)
        {
            // Check first if any record was selected
            int nSelect = 0; 
            for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)
            {
                if (dgvControlPageNumbers.Rows[j].Cells["Selector"].Value.ToString() == "True")
                {
                    nSelect++;
                }
            }

            if (nSelect == 0)
            {
                MessageBox.Show("Please select a page to void!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                     
                return;
            }

            // Check selected record has a reason entered by user
            nSelect = 0;
            for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)
            {
                if (Convert.ToInt16(dgvControlPageNumbers.Rows[j].Cells["PageStatus"].Value) != 3)
                {
                    if (dgvControlPageNumbers.Rows[j].Cells["StatusReason"].Value.ToString() != "")
                    {
                        nSelect++;
                    }
                }
            }

            if (nSelect == 0)
            {
                MessageBox.Show("Please enter reason for voiding!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you really want to void the selected Control Page records?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {                          
                nProcess = 3;                                                                                       // Voided; btnVoid was clicked
                UpdateControlPageSelected(0, 0);                                                                    // loop thru all records for this PageID and clear selector
                UpdateControlPageStatus(nProcess);
                txtControlPageID.Text = dgvGBLSelection.CurrentRow.Cells["ControlPageID"].Value.ToString();
                txtGBLNo.Text = dgvGBLSelection.CurrentRow.Cells["PSSNo"].Value.ToString();
                txtServiceCode.Text = dgvGBLSelection.CurrentRow.Cells["ServiceCode"].Value.ToString();
                LoadControlPageNumbers(Convert.ToInt64(txtControlPageID.Text));
                tsbCancel.Enabled = false;
                btnClose.Visible = true;
            }
            else
            {
                MessageBox.Show("You chose not to void the selected Control Page records!");
            }
            
        }

        private void dgvGBLSelection_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {           
            e.Cancel = true;              
        }
     
        private void txtPagesToAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void btnAddSelected_Click(object sender, EventArgs e)
        {
            if (dgvGBLList.Rows.Count == 0)
            {
                MessageBox.Show("No more PSS to add!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboBookNo.Focus();
                return;
            }
            
            // check if entry already exists!
            if (dgvGBLSelection.Rows.Count != 0)
            {
                for (int j = 0; j < dgvGBLSelection.Rows.Count; j++)                                       
                {
                    if (
                        dgvGBLSelection.Rows[j].Cells["PSSNo"].Value.ToString().Equals(dgvGBLList.CurrentRow.Cells["PSSNo"].Value.ToString()) &&
                        dgvGBLSelection.Rows[j].Cells["ServiceCode"].Value.ToString().Equals(dgvGBLList.CurrentRow.Cells["ServiceCode"].Value.ToString())
                        )
                    {
                        MessageBox.Show("This GBL/Service Code entry already exists!");
                        return;
                    }
                }                
            }

            // Add data
            foreach (DataGridViewRow row in dgvGBLList.SelectedRows)
            {
                DataRow dR = dtControlPageGBL.NewRow();
                dR["ControlPageID"] = 0;
                dR["PSSNo"] = row.Cells["PSSNo"].Value.ToString();
                dR["ServiceCode"] = row.Cells["ServiceCode"].Value.ToString();
                dtControlPageGBL.Rows.Add(dR);
            }

            dtControlPageGBL.AcceptChanges();
            bsGBLSelection.DataSource = dtControlPageGBL;        

        }

        private void btnDelSelected_Click(object sender, EventArgs e)
        {
            if (dgvGBLSelection.Rows.Count == 0)
            {
                MessageBox.Show("No more GBLs to remove!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboBookNo.Focus();
                return;
            }

            foreach (DataGridViewRow row in dgvGBLSelection.SelectedRows)
            {
                DataRow dR = dtGBLList.NewRow();
                dR["PSSNo"] = Convert.ToInt64(row.Cells["PSSNo"].Value.ToString());
                dR["ServiceCode"] = Convert.ToInt16(row.Cells["ServiceCode"].Value.ToString());
                dtGBLList.Rows.Add(dR);
                bsGBLList.DataSource = dtGBLList;
                dgvGBLList.DataSource = bsGBLList;
                dgvGBLSelection.Rows.RemoveAt(row.Index);
            }
        }

        private void txtTotPgNeeded_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void CheckAllPages()
        {
            if (dgvControlPageNumbers.Rows.Count != 0)
            {
                // Check All
                for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)
                {
                    if (dgvControlPageNumbers.Rows[j].Cells["Printed"].Value.ToString() == "")
                    {
                        dgvControlPageNumbers.Rows[j].Cells["Selector"].Value = 1;
                    }
                }
            }
        }

        private void UncheckAllPages()
        {
            if (dgvControlPageNumbers.Rows.Count != 0)
            {
                // Uncheck All
                for (int j = 0; j < dgvControlPageNumbers.Rows.Count; j++)
                {
                    dgvControlPageNumbers.Rows[j].Cells["Selector"].Value = 0;                    
                }
            }
        }

        private void chkPrintAll_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkPrintAll.Checked)
            {
                CheckAllPages();
            }
            else
            {
                UncheckAllPages();
            }
        }

        private void cboBookNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (cboBookNo.Text.Trim() != "")
                {
                    if (nMode == 1)
                    {
                        try
                        {
                            LoadGBLList(cboBookNo.Text);//Convert.ToInt16(
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }                  
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            pnlAdmin.Visible = true;
            btnAdmin.Enabled = false;
            btnPrintTo122.Enabled = false;
            btnPrintTo16.Enabled = false;
            btnAddPage.Enabled = false;
            btnVoid.Enabled = false;
            btnPrintTo45.Enabled = false;
            btnPrintToQA45.Enabled = false;
            //if (btnPrintTo45.Visible == true)
            //{
            //    btnPrintTo45.Enabled = false;
            //    btnPrintTo45.Visible = false;
            //}
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            pnlAdmin.Visible = false;
            btnAdmin.Enabled = true;
            btnPrintTo122.Enabled = true;
            btnPrintTo16.Enabled = true;
            btnAddPage.Enabled = true;
            btnVoid.Enabled = true;
            btnPrintTo45.Enabled = true;
            btnPrintToQA45.Enabled = true;
            //if (btnPrintTo45.Visible == true)
            //{
            //    btnPrintTo45.Enabled = true;
            //}
            ClearControls(this.pnlAdmin);  
        }

        private void AdminFunctions(int cRequestType, Int32 cBookNo, Int64 cGBLNo, Int32 cOldServiceCode, Int32 cNewServiceCode, Int16 cRequestedByID, String cChangeReason)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@RequestType", cRequestType);
            sqlcmd.Parameters.AddWithValue("@BookNo", cBookNo);          
            sqlcmd.Parameters.AddWithValue("@GBLNo", cGBLNo);
            sqlcmd.Parameters.AddWithValue("@OldServiceCode", cOldServiceCode);
            sqlcmd.Parameters.AddWithValue("@NewServiceCode", cNewServiceCode);
            sqlcmd.Parameters.AddWithValue("@RequestedByID", cRequestedByID);
            sqlcmd.Parameters.AddWithValue("@ChangeReason", cChangeReason);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdateControlPageAdmin";

            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReuse_Click(object sender, EventArgs e)
        {
            nRequestType = 1;

            // Validate entries
            int nAdmin = ValidateAdminDetails();                                                      // Validation for Admin Details

            if (nAdmin == 0)
            {
                return;
            }
                                  
            // Restore GBL
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("You are about to unlink and reuse a GBL Number from a previously printed Control Page. Are you really sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                AdminFunctions(
                                1, 
                                Convert.ToInt32(txtAdmBookNo.Text), 
                                Convert.ToInt64(txtAdmGBLNo.Text), 
                                Convert.ToInt32(txtOldServiceCode.Text), 
                                0, 
                                Convert.ToInt16(txtRequestedByID.Text), 
                                Convert.ToString(txtChangeReason.Text)
                               );
                MessageBox.Show("This GBL Number has been restored successfully and can now be used for new Control Pages!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtAdmGBLNo.Text = "";
            }
        }

        private void btnUpdateServiceCode_Click(object sender, EventArgs e)
        {
            nRequestType = 2;

            // Validate entries
            int nAdmin = ValidateAdminDetails();                                                      // Validation for Admin Details

            if (nAdmin == 0)
            {
                return;
            }          

            // Update Service Code
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("You are about to update an existing Service Code used in a previously printed Control Page. Are you really sure?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                AdminFunctions(
                                2, 
                                Convert.ToInt32(txtAdmBookNo.Text), 
                                Convert.ToInt64(txtAdmGBLNo.Text), 
                                Convert.ToInt32(txtOldServiceCode.Text), 
                                Convert.ToInt32(txtNewServiceCode.Text), 
                                Convert.ToInt16(txtRequestedByID.Text), 
                                Convert.ToString(txtChangeReason.Text)
                              );
                MessageBox.Show("This New Service Code has been applied successfully to the affected Control Pages!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtAdmGBLNo.Text = "";
                txtOldServiceCode.Text = "";
                txtNewServiceCode.Text = "";
            }
        }

        private int ValidateAdminDetails()
        {
            int nVal;

            if (txtAdmBookNo.Text.Trim() == "")
            {
                MessageBox.Show("Please specify a Book Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtAdmBookNo.Focus();
                return 0;
            }           

            if (txtAdmGBLNo.Text.Trim() == "")
            {
                MessageBox.Show("Please specify a GBL Number!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtAdmGBLNo.Focus();
                return 0;
            }

            if (txtOldServiceCode.Text.Trim() == "")
            {
                MessageBox.Show("Please specify a Service Code!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtOldServiceCode.Focus();
                return 0;
            }            

            if (nRequestType == 2)
            {
                if (txtNewServiceCode.Text.Trim() == "")
                {
                    MessageBox.Show("Please specify the New Service Code you want to apply!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtNewServiceCode.Focus();
                    return 0;
                }               
            }

            if (txtRequestedByID.Text.Trim() == "")
            {
                MessageBox.Show("Please specify Requestor!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtRequestedByID.Focus();
                return 0;
            }

            if (txtChangeReason.Text.Trim() == "")
            {
                MessageBox.Show("Please specify Reason for change!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtChangeReason.Focus();
                return 0;
            }

            // Check if entries exist
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                sqlcnn.Dispose();
                return 0;
            }

            SqlCommand sqlcmd = new SqlCommand("SELECT BookNo, GBLNo, ServiceCode FROM ControlPageGBL cp (NOLOCK) " +
                                              "WHERE cp.GBLNo = " +
                                                     Convert.ToInt64(txtAdmGBLNo.Text) + " AND cp.ServiceCode = " +
                                                     Convert.ToInt32(txtOldServiceCode.Text) + " AND cp.BookNo = " +
                                                     Convert.ToInt32(txtAdmBookNo.Text), sqlcnn);

            SqlDataReader sqldr = sqlcmd.ExecuteReader();
            if (!sqldr.HasRows)
            {
                MessageBox.Show("Book/GBL/Service Code do not exist! Please check your entries.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                sqldr.Close(); sqlcmd.Dispose();
                sqlcnn.Close(); sqlcnn.Dispose();
                txtAdmBookNo.Focus();
                return 0;
            }
            return 1;
        }

        private void txtAdmGBLNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void txtOldServiceCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void txtNewServiceCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        private void txtAdmBookNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar))
            {
            }
            else
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }

        // MY 06/24/2015 - START: txt/dgvRequestedBy events
        private void dgvRequestors_DoubleClick(object sender, EventArgs e)
        {
            txtRequestor.Text = dgvRequestors.CurrentRow.Cells[0].Value.ToString();
            txtRequestedByID.Text = dgvRequestors.CurrentRow.Cells[1].Value.ToString();
            dgvRequestors.Visible = false;
        }

        private void dgvRequestors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvRequestors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtRequestor.Text = dgvRequestors.CurrentRow.Cells[0].Value.ToString();
                txtRequestedByID.Text = dgvRequestors.CurrentRow.Cells[1].Value.ToString();
                dgvRequestors.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvRequestors.Visible = false;
            }
        }
        private void txtRequestor_Enter(object sender, EventArgs e)
        {           
            dgvRequestors.Visible = true; dgvRequestors.BringToFront();           
        }

        private void dgvRequestors_Leave(object sender, EventArgs e)
        {
            dgvRequestors.Visible = false;
        }

        private void txtRequestor_TextChanged(object sender, EventArgs e)
        {
            DataView dvwRequestedBy;
            dvwRequestedBy = new DataView(dtRequestors, "RequestedByName like '%" + txtRequestor.Text.Trim().Replace("'", "''") + "%'", "RequestedByName", DataViewRowState.CurrentRows);
            dvwSetUp(dgvRequestors, dvwRequestedBy);            
        }

        private void dgvRequestors_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtRequestor.Text = dgvRequestors.CurrentRow.Cells[0].Value.ToString();
            txtRequestedByID.Text = dgvRequestors.CurrentRow.Cells[1].Value.ToString();
            dgvRequestors.Visible = false;
        }

        private void picRequestors_Click(object sender, EventArgs e)
        {          
            LoadRequestors();
            dgvRequestors.Visible = true; dgvRequestors.BringToFront();           
        }

        private void txtRequestedByID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtRequestor.Text = PSSClass.QA.ControlPageEmployeeName(Convert.ToInt16(txtRequestedByID.Text));
                if (txtRequestor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Requestor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvRequestors.Visible = false;
            }
            else
            {
                txtRequestor.Text = ""; dgvRequestors.Visible = false;
            }
        }
        // MY 06/24/2015 - END: txt/dgvRequestedBy events   

        private void btnResetPrint_Click(object sender, EventArgs e)
        {  
            if (dgvGBLSelection.Rows.Count != 0)
            {
                Int64 intCtrlPageID;
                string strCtrlPageNo;

                for (int j = 0; j < dgvGBLSelection.Rows.Count; j++)
                {
                    if (dgvControlPageNumbers.Rows[j].Cells["Selector"].Value.ToString() == "True")
                    {
                        intCtrlPageID = Convert.ToInt64(dgvControlPageNumbers.Rows[j].Cells["ControlPageID"].Value.ToString());
                        strCtrlPageNo = dgvControlPageNumbers.Rows[j].Cells["ControlPageNo"].Value.ToString();
                        UpdateControlPageReprint(intCtrlPageID, strCtrlPageNo);
                    }
                }

                MessageBox.Show("Reprint successful!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
            }
            btnResetPrint.Enabled = false;
            btnAdmin.Enabled = false;
            AddEditMode(false);           
        }

        private void btnPrintToQA45_Click(object sender, EventArgs e)
        {
            try
            {
                // This print-out is for 45 Office
                strPrinterName = @"\\psapp04\QA Printer";
                strRptName = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "ControlPageForm_16.rpt";
                PrintReport(strPrinterName, strRptName);
                btnPrintTo45.Enabled = false;
                btnPrintTo122.Enabled = false;
                btnPrintTo16.Enabled = false;
                btnPrintToQA45.Enabled = false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
