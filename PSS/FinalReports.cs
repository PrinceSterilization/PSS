//FinalReports.cs
// AUTHOR       : ALEXANDER M. DELA CRUZ
// TITLE        : Software Developer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Final Reports File Maintenance

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Diagnostics;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Drawing.Printing; // 2/22/2016

namespace PSS
{
    public partial class FinalReports : PSS.TemplateForm
    {
        public byte nLSw = 0; //Login
        //public byte nDSw; //Data Form
        public string pubCmpyCode;
        public Int32 nRptNo;
        public int nFormat;
        public string stFormat;

        byte nMode = 0;
        byte nSw = 0;
        byte nOSw = 0;
        byte nMWSw = 0; //MS Word With Other Charges

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol = { };
        private int nIndex;
        private byte nR = 0;
        private byte nSave = 0;
        private int nCtr = 0;
        private int nReason = 0;
        private byte nBilling = 0;
        private string strFileAccess = "RO";
        private string strGroup = "";
        private Int16 nSC329 = 0;

        private string strRptGBL = "";
        private string strRptSC = "";

        protected DataTable dtSponsors = new DataTable();
        protected DataTable dtContacts = new DataTable();
        protected DataTable dtGBLDDL = new DataTable();
        protected DataTable dtSCDDL = new DataTable();
        protected DataTable dtSC = new DataTable();

        protected DataTable dtRptMstr = new DataTable();
        protected DataTable dtRptRev = new DataTable();
        protected DataTable dtOtherFees = new DataTable();
        protected DataTable dtRptLogs = new DataTable();
        protected DataTable dtCtrlPages = new DataTable();
        protected DataTable dtPMRC = new DataTable();
        protected DataTable dtRptExt = new DataTable();

        private ReportDocument crDoc;

        public FinalReports()
        {
            InitializeComponent();
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
            tstbSearch.KeyPress += new KeyPressEventHandler(SearchKeyPressHandler);
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            txtSponsor.GotFocus += new EventHandler(txtSponsorEnterHandler);
            txtContact.GotFocus += new EventHandler(txtContactEnterHandler);
            txtContactID.GotFocus += new EventHandler(txtContactIDEnterHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void AddRecord()
        {
            nMode = 1;
            cboGBLs.CausesValidation = true; cboSCs.CausesValidation = true; dgvFile.CausesValidation = false;

            ClearControls(pnlRecord); OpenControls(pnlRecord, true);

            dtRptMstr.Rows.Clear(); dtRptRev.Rows.Clear(); dtRptLogs.Rows.Clear();
            txtRptNo.Text = "(New)";

            DataRow dr;
            dr = dtRptMstr.NewRow();
            dr["CompanyCode"] = "P";
            dr["ReportNo"] = txtRptNo.Text;
            dr["SponsorID"] = DBNull.Value;
            dr["SponsorName"] = "";
            dr["ContactID"] = DBNull.Value;
            dr["ContactName"] = "";
            dr["ReportNotes"] = "";
            dr["InternalNotes"] = "";
            dr["Purpose"] = "";
            dr["Method"] = "";
            dr["Results"] = "";
            dr["Conclusion"] = "";
            dr["Memorandum"] = "";
            dtRptMstr.Rows.Add(dr);
            dtRptMstr.AcceptChanges();
            bsRptMstr.DataSource = dtRptMstr;

            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            txtRptNo.ReadOnly = true;

            btnAddRev_Click(null, null);
            txtSponsor.Focus();
            //if (strGroup == "EXEC" || strGroup == "IT" || LogIn.nUserID == 73) //added K Kohan 11/14/2016
            //{
            //    chkCancelled.Visible = true; chkNoCharge.Visible = true; lnkCancDtls.Visible = true;
            //}
        }

        private void txtSponsorEnterHandler(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSponsors.TabIndex = 3; dgvContacts.Visible = false;
            }
        }

        private void EditRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;

            nMode = 2;
            cboGBLs.CausesValidation = true; cboSCs.CausesValidation = true; 
            dgvFile.Visible = false; pnlRecord.Visible = true; pnlRecord.BringToFront(); 

            LoadData();
            OpenControls(pnlRecord, true);
            btnClose.Visible = false;
            btnAddRev.Enabled = true; btnDelRev.Enabled = true;
            btnAddLog.Enabled = true; btnDelLog.Enabled = true;
        }

        private void DeleteRecord()
        {
            if (dgvFile.Rows.Count == 0)
                return;

            LoadData();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this report?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                SqlCommand sqlcmd = new SqlCommand();

                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelFinRpt";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            LoadRecords();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
        }

        private void SaveRecord()
        {
            //Main Report Data Validation
            int nV = ValidateReport();
            if (nV == 1)
            {
                nV = 0;
                return;
            }
            //Revision Log Validation
            nV = ValidateLogs();
            if (nV == 1)
            {
                nV = 0;
                return;
            }
            lblCH.Visible = false;
            UpdateRptMstr();
            UpdateRptRev();
            UpdateRptLogs();
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("ReportNo", txtRptNo.Text, bsFile, dgvFile);
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront();
            nMode = 0; nR = 0; btnClose.Visible = true; 
            //if (nLSw == 1) // Disabled 2-27-2017
            //{
            //    nLSw = 0;
            //    SendKeys.Send("{F12}");
            //    return;
            //}
            LoadData();
            txtRptNo.Focus();
        }

        private void UpdateRptMstr()
        {
            bsRptMstr.EndEdit();
            if (nMode == 1)
            {
                SaveRptMstr(1);
            }
            else if (nMode == 2)
            {
                DataTable dt = new DataTable();
                dt = dtRptMstr.GetChanges(DataRowState.Modified);
                if (dt != null)
                {
                    SaveRptMstr(2);
                }
                //UpdateCancelFee();
            }
        }

        private void UpdateCancelFee()
        {
            if ((strGroup == "EXEC" || strGroup == "IT" || LogIn.nUserID == 73)) //added K Kohan 11-14-2016
            {

                if (chkCancelled.Checked == true && rdoNoCharge.Checked == false && rdoFixAmount.Checked == false && rdoStandard.Checked == false)
                {
                    MessageBox.Show("Please indicate charges.", Application.ProductName);
                    return;
                }
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                SqlCommand sqlcmd = new SqlCommand();

                sqlcmd.Connection = sqlcnn;
                bsRptExt.EndEdit();
                if (dtRptExt == null || dtRptExt.Rows.Count == 0)
                {
                    if (chkCancelled.Checked == true)
                    {
                        DataRow dRow = dtRptExt.NewRow();
                        dRow["Cancelled"] = true;
                        if (rdoNoCharge.Checked == true) // || Convert.ToDecimal(txtCancFee.Text) == 0
                        {
                            dRow["CancellationFee"] = 0;
                            dRow["NoCharge"] = true;
                        }
                        else
                        {
                            dRow["CancellationFee"] = Convert.ToDecimal(txtCancFee.Text);
                            dRow["NoCharge"] = false;
                        }
                        dRow["CancellationCode"] = Convert.ToInt16(txtCancCode.Text);
                        dtRptExt.Rows.Add(dRow);
                    }
                    else if (chkNoCharge.Checked == true) // || Convert.ToDecimal(txtCancFee.Text) == 0
                    {
                        DataRow dRow = dtRptExt.NewRow();
                        dRow["NoCharge"] = true;
                        dRow["Cancelled"] = false;
                        dRow["CancellationFee"] = 0;
                        dRow["CancellationCode"] = txtCancCode.Text;
                        dtRptExt.Rows.Add(dRow);
                    }
                    if (dtRptExt.Rows.Count > 0)
                    {
                        DialogResult dReply = new DialogResult();
                        dReply = MessageBox.Show("Do you want to save changes?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dReply == DialogResult.Yes)
                        {
                            sqlcmd.Dispose();
                            sqlcmd = new SqlCommand();
                            sqlcmd.Connection = sqlcnn;
                            sqlcmd.Parameters.AddWithValue("@nMode", 1);
                            sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                            sqlcmd.Parameters.AddWithValue("@Canc", dtRptExt.Rows[0]["Cancelled"]);
                            sqlcmd.Parameters.AddWithValue("@DteCanc", DateTime.Now);
                            sqlcmd.Parameters.AddWithValue("@CancFee", dtRptExt.Rows[0]["CancellationFee"]);
                            sqlcmd.Parameters.AddWithValue("@CancCode", dtRptExt.Rows[0]["CancellationCode"]);
                            sqlcmd.Parameters.AddWithValue("@NoCh", dtRptExt.Rows[0]["NoCharge"]);
                            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                            sqlcmd.CommandType = CommandType.StoredProcedure;
                            sqlcmd.CommandText = "spAddEditFinRptExt";
                            try
                            {
                                sqlcmd.ExecuteNonQuery();
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    DataTable dtX = dtRptExt.GetChanges(DataRowState.Modified);
                    if (dtX != null && dtX.Rows.Count > 0)
                    {
                        if (nMode == 0)
                        {
                            DialogResult dReply = new DialogResult();
                            dReply = MessageBox.Show("Do you want to save changes?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dReply == DialogResult.Yes)
                            {
                                if (Convert.ToDecimal(dtRptExt.Rows[0]["CancellationFee"]) == 0)
                                {
                                    dtRptExt.Rows[0]["NoCharge"] = true;
                                    dtRptExt.Rows[0]["CancellationCode"] = txtCancCode.Text;
                                    bsRptExt.EndEdit();
                                }
                                sqlcmd.Dispose();
                                sqlcmd = new SqlCommand();
                                sqlcmd.Connection = sqlcnn;
                                sqlcmd.Parameters.AddWithValue("@nMode", 2);
                                sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                                sqlcmd.Parameters.AddWithValue("@Canc", dtRptExt.Rows[0]["Cancelled"]);
                                sqlcmd.Parameters.AddWithValue("@DteCanc", DateTime.Now);
                                sqlcmd.Parameters.AddWithValue("@CancFee", dtRptExt.Rows[0]["CancellationFee"]);
                                sqlcmd.Parameters.AddWithValue("@CancCode", dtRptExt.Rows[0]["CancellationCode"]);
                                sqlcmd.Parameters.AddWithValue("@NoCh", dtRptExt.Rows[0]["NoCharge"]);
                                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                                sqlcmd.CommandType = CommandType.StoredProcedure;
                                sqlcmd.CommandText = "spAddEditFinRptExt";
                                try
                                {
                                    sqlcmd.ExecuteNonQuery();
                                }
                                catch { }
                            }
                        }
                    }
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                chkCancelled.Checked = false; chkNoCharge.Checked = false;
                rdoNoCharge.Checked = false; rdoStandard.Checked = false; rdoFixAmount.Checked = false;
                dtRptExt = PSSClass.FinalReports.CancelledRpt(Convert.ToInt32(txtRptNo.Text));
                bsRptExt.DataSource = dtRptExt;
                if (nMode == 0)
                    lnkCancDtls.Visible = false;
                else
                    lnkCancDtls.Visible = true;
                if (dtRptExt.Rows.Count > 0)
                {
                    if (dtRptExt.Rows[0]["CancellationCode"].ToString() == "0")
                        rdoNoCharge.Checked = true;
                    else if (dtRptExt.Rows[0]["CancellationCode"].ToString() == "1")
                        rdoStandard.Checked = true;
                    else if (dtRptExt.Rows[0]["CancellationCode"].ToString() == "2")
                        rdoFixAmount.Checked = true;
                }
                pnlCancelFee.Visible = false; btnClose.Enabled = true;
            }
        }

        private void SaveRptMstr(int nM)
        {
            if (nMode == 1)
                txtRptNo.Text = PSSClass.General.NewRptNo("FinalRptMaster", "ReportNo").ToString();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
            sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
            if (txtRptNotes.ToString().Trim() == "")
                sqlcmd.Parameters.AddWithValue("@RptNotes", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@RptNotes", txtRptNotes.Text.Trim());
            if (txtIntNotes.ToString().Trim() == "")
                sqlcmd.Parameters.AddWithValue("@IntNotes", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@IntNotes", txtIntNotes.Text.Trim());
            if (txtMethod.ToString().Trim() == "")
                sqlcmd.Parameters.AddWithValue("@Method", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@Method", txtMethod.Text.Trim());
            if (txtConclusion.ToString().Trim() == "")
                sqlcmd.Parameters.AddWithValue("@Conclusion", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@Conclusion", txtConclusion.Text.Trim());
            if (txtPurpose.ToString().Trim() == "")
                sqlcmd.Parameters.AddWithValue("@Purpose", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@Purpose", txtPurpose.Text.Trim());
            if (txtResults.ToString().Trim() == "")
                sqlcmd.Parameters.AddWithValue("@Results", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@Results", txtResults.Text.Trim());
            if (txtMemo.ToString().Trim() == "")
                sqlcmd.Parameters.AddWithValue("@Memo", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@Memo", txtMemo.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@ConID", Convert.ToInt16(txtContactID.Text));
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
                    nSave = 1;
                    return;
                }
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
        }

        private static int DelRptRev(string cCmpyCode, string cRNo, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            int nRNo;

            if (cDT.Rows[cI].RowState.ToString() == "Deleted")
                nRNo = Convert.ToInt16(cDT.Rows[cI]["RevisionNo", DataRowVersion.Original].ToString());
            else
                nRNo = Convert.ToInt16(cDT.Rows[cI]["RevisionNo"].ToString());

            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpyCode);
            sqlcmd.Parameters.AddWithValue("@RptNo", cRNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", nRNo);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelFinRptRev";

            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            { }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }

        private static int DelRptLogs(string cCmpyCode, string cRNo, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            int nRNo;

            if (cDT.Rows[cI].RowState.ToString() == "Deleted")
                nRNo = Convert.ToInt16(cDT.Rows[cI]["RevisionNo", DataRowVersion.Original].ToString());
            else
                nRNo = Convert.ToInt16(cDT.Rows[cI]["RevisionNo"].ToString());

            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpyCode);
            sqlcmd.Parameters.AddWithValue("@RptNo", cRNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", nRNo);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelFinRptLogs";

            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            { }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }


        private void UpdateRptRev()
        {
            bsRptRev.EndEdit();
            int nAdded = 0; int nEdited = 0; int nDeleted = 0;

            DataTable dt = dtRptRev.GetChanges(DataRowState.Deleted);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nDelRev = DelRptRev(txtCmpyCode.Text, txtRptNo.Text, i, 3, dt);
                    nDeleted += nDelRev;
                }
            }
            dt = dtRptRev.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveRptRev(txtCmpyCode.Text, txtRptNo.Text, i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            dt = dtRptRev.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveRptRev(txtCmpyCode.Text, txtRptNo.Text, i, 2, dt);
                    nEdited += nSaveRev;
                }
                dt.Rows.Clear();
            }
        }

        private void UpdateRptLogs()
        {
            bsRptLogs.EndEdit();

            int nAdded = 0; int nEdited = 0; int nDeleted = 0;

            DataTable dt = dtRptLogs.GetChanges(DataRowState.Deleted);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nDelRev = DelRptLogs(txtCmpyCode.Text, txtRptNo.Text, i, 3, dt);
                    nDeleted += nDelRev;
                }
            }
            dt = dtRptLogs.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveRptLogs(txtCmpyCode.Text, txtRptNo.Text, i, 1, dt);
                    nAdded += nSaveRev;
                }
                dt.Rows.Clear();
            }
            dt = dtRptLogs.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int nSaveRev = SaveRptLogs(txtCmpyCode.Text, txtRptNo.Text, i, 2, dt);
                    nEdited += nSaveRev;
                }
                dt.Rows.Clear();
            }
        }


        private static int SaveRptRev(string cCmpyCode, string cRNo, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpyCode); 
            sqlcmd.Parameters.AddWithValue("@RptNo", cRNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", cDT.Rows[cI]["RevisionNo"]);
            sqlcmd.Parameters.AddWithValue("@RevDate",  cDT.Rows[cI]["ReportDate"]);
            sqlcmd.Parameters.AddWithValue("@StudyDirID", cDT.Rows[cI]["StudyDirID"]); 
            if (cDT.Rows[cI]["ReasonCode"] == null)
                sqlcmd.Parameters.AddWithValue("@ReasonCode", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@ReasonCode", cDT.Rows[cI]["ReasonCode"]);

            if (cDT.Rows[cI]["Reason"] == null || cDT.Rows[cI]["Reason"].ToString() == "")
                sqlcmd.Parameters.AddWithValue("@Reason", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@Reason", cDT.Rows[cI]["Reason"]);

            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditFinRptRev";
            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            {
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }

        private static int SaveRptLogs(string cCmpyCode, string cRNo, int cI, byte cMode, DataTable cDT)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", cMode);
            sqlcmd.Parameters.AddWithValue("@CmpyCode", cCmpyCode);
            sqlcmd.Parameters.AddWithValue("@RptNo", cRNo);
            sqlcmd.Parameters.AddWithValue("@RevNo",  Convert.ToInt16(cDT.Rows[cI]["RevisionNo"]));
            sqlcmd.Parameters.AddWithValue("@LogNo", cDT.Rows[cI]["PSSNo"]);
            sqlcmd.Parameters.AddWithValue("@SC", cDT.Rows[cI]["ServiceCode"]);
            sqlcmd.Parameters.AddWithValue("@FormatNo", cDT.Rows[cI]["DataFormat"]);
            sqlcmd.Parameters.AddWithValue("@DteOn", cDT.Rows[cI]["DateOn"]);
            sqlcmd.Parameters.AddWithValue("@DteOff", cDT.Rows[cI]["DateOff"]);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditFinRptLogs";
            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            {
            }
            sqlcmd.Dispose();
            //Update Log Test's ReportNo field
            sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@RptNo", cRNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(cDT.Rows[cI]["RevisionNo"]));
            sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(cDT.Rows[cI]["PSSNo"]));
            sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(cDT.Rows[cI]["ServiceCode"]));
            sqlcmd.Parameters.AddWithValue("@FormatNo", Convert.ToInt16(cDT.Rows[cI]["DataFormat"]));
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
            sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }

        public int ValidateReport()
        {
            if (txtSponsorID.Text.Trim() == "" || txtSponsor.Text.Trim() == "")
            {
                MessageBox.Show("Please select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsorID.Focus();
                return 1;
            }
            if (txtContactID.Text.Trim() == "" || txtContact.Text.Trim() == "")
            {
                MessageBox.Show("Please select Contact.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtContact.Focus();
                return 1;
            }
            if (cboStudyDir.SelectedIndex == -1)
            {
                MessageBox.Show("Please select Study Director.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboStudyDir.Focus();
                return 1;
            }
            return 0;
        }

        public int ValidateLogs()
        {
            bsRptLogs.EndEdit();
            if (dtRptLogs == null || dtRptLogs.Rows.Count == 0)
            {
                MessageBox.Show("No login entries found." + Environment.NewLine + "Please check your entries.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bsRptLogs.Position = dtRptLogs.Rows.Count;
                return 1;
            }

            string strSC = "", strLogNo = "";
            byte nBlank = 0;

            DataTable dt = new DataTable();
            DataView dvw = dtRptLogs.DefaultView;
            dvw.Sort = "PSSNo, ServiceCode";
            dt = dvw.ToTable();
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No login entries found." + Environment.NewLine + "Please check your entries.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bsRptLogs.Position = dtRptLogs.Rows.Count;
                return 1;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i].RowState != DataRowState.Deleted)
                {
                    if (strLogNo == dt.Rows[i]["PSSNo"].ToString() && strSC == dt.Rows[i]["ServiceCode"].ToString() && strLogNo != "" && strSC != "")
                    {
                        MessageBox.Show("Duplicate Order No./Service Code found." + Environment.NewLine + "Please check your entries.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        bsRptLogs.Position = dtRptLogs.Rows.Count;
                        return 1;
                    }
                    else
                    {
                        strLogNo = dt.Rows[i]["PSSNo"].ToString();
                        strSC = dt.Rows[i]["ServiceCode"].ToString();
                    }
                    if (dt.Rows[i]["PSSNo"].ToString().Trim() == "" || dt.Rows[i]["ServiceCode"].ToString().Trim() == "" || dt.Rows[i]["DataFormat"].ToString().Trim() == "")
                    {
                        nBlank = 1;
                    }
                    //check for duplicate in database just in case of error in saving
                    DataTable dtX = PSSClass.FinalReports.FinRptPSSSC(txtCmpyCode.Text, Convert.ToInt32(dt.Rows[i]["PSSNo"]), Convert.ToInt16(dt.Rows[i]["ServiceCode"]));
                    if (dtX.Rows.Count > 0 && nMode == 1)
                    {
                        MessageBox.Show("Matching Order No. and Service Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        bsRptLogs.Position = dtRptLogs.Rows.Count;
                        return 1;
                    }
                    dtX.Dispose();
                }
            }
            dt.Dispose(); dvw.Dispose();
            if (nBlank == 1)
            {
                MessageBox.Show("Blank entry is found." + Environment.NewLine + "Please check your entries.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                bsRptLogs.Position = dtRptLogs.Rows.Count;
                return 1;
            }
            else
                return 0;
        }

        private void CancelSave()
        {
            cboGBLs.CausesValidation = false; cboSCs.CausesValidation = false; dgvFile.CausesValidation = false;
            if (nLSw == 1 || nLSw == 2 || nLSw == 3)
            {
                nLSw = 0;
                SendKeys.Send("{F12}");
                return;
            }
            if (nMode != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            lblCH.Visible = false;
            ClearControls(this);
            AddEditMode(false);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); bnFile.Enabled = true;
            dgvSponsors.Visible = false; dgvContacts.Visible = false;
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData();
                btnClose.Visible = true;
            }
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

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            string strRptNo = "";
            if (nLSw == 1 || nBilling == 1)
            {
                nBilling = 0; nLSw = 0; 
                if (dgvFile.Rows.Count > 0)
                    strRptNo = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["ReportNo"].Value.ToString();
                LoadRecords();
                bsFile.Filter = "ReportNo<>0";
                if (strRptNo != "")
                    PSSClass.General.FindRecord("ReportNo", strRptNo, bsFile, dgvFile);
            }
            else
            {
                if (dgvFile.Rows.Count > 0)
                    strRptNo = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["ReportNo"].Value.ToString();
                LoadRecords();
                bsFile.Filter = "ReportNo<>0";
                if (strRptNo != "")
                    PSSClass.General.FindRecord("ReportNo", strRptNo, bsFile, dgvFile);
            }
        }

        public void SearchOKClickHandler(object sender, EventArgs e)
        {
            try
            {
                bsFile.Filter = "ReportNo<>0";
                PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                dgvFile.Select();
            }
            catch { }
        }

        private void SearchFilterClickHandler(object sender, EventArgs e)
        {
            try
            {
                if (arrCol[nIndex] == "System.String")
                {
                    string strSearch = tstbSearch.Text.Replace("'", "''");
                    if (chkFullText.Checked == true)
                        bsFile.Filter = tstbSearchField.Text + "='" + strSearch + "'";
                    else
                        bsFile.Filter = tstbSearchField.Text + " LIKE '%" + strSearch + "%'";
                }
                else if (arrCol[nIndex] == "System.DateTime")
                {
                    bsFile.Filter = tstbSearchField.Text + " = '" + tstbSearch.Text + "'";
                }
                else
                {
                    bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;
                }
                dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                tsbRefresh.Enabled = true;
            }
            catch
            {
            }
        }

        private void SearchKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SearchFilterClickHandler(null, null);
            }
        }

        private void dgvCellMouseClickEventHandler(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                cklColumns.Visible = true; cklColumns.BringToFront();
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

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            if (nLSw == 1 || nLSw == 2 || nLSw == 3 || nBilling == 1)
                dt = PSSClass.FinalReports.FinalRptESign(nRptNo);
            else
                dt = PSSClass.FinalReports.FinalRptMaster(1);
    
            if (dt == null)
            {
                nMode = 9;
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            
            DataGridSetting();
            
            nMode = 0;
            dt.Dispose(); btnClose.Enabled = true;
            FileAccess();
            try
            {
                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() != "" ||
                       dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateESigned"].Value.ToString() != "" ||
                       dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "")
                {
                    tsbEdit.Enabled = false;
                }
            }
            catch { }
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["CompanyCode"].HeaderText = "CMPY CODE";
            dgvFile.Columns["ReportNo"].HeaderText = "REPORT NO.";
            dgvFile.Columns["RevisionNo"].HeaderText = "REV. NO.";
            dgvFile.Columns["ReportDate"].HeaderText = "REPORT DATE";
            dgvFile.Columns["PSSNo"].HeaderText = "PSS NO.";
            dgvFile.Columns["ServiceCode"].HeaderText = "SC";
            dgvFile.Columns["ServiceDesc"].HeaderText = "SC DESCRIPTION";
            dgvFile.Columns["DateESigned"].HeaderText = "DATE E-SIGNED";
            dgvFile.Columns["DateEMailed"].HeaderText = "DATE MAILED/ SCANNED";
            dgvFile.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvFile.Columns["Sponsor"].HeaderText = "SPONSOR";
            dgvFile.Columns["Contact"].HeaderText = "CONTACT";
            if (nLSw == 0)
            {
                dgvFile.Columns["InvoiceNo"].HeaderText = "INV. NO.";
                dgvFile.Columns["InvoiceNo"].Width = 80;
                dgvFile.Columns["InvoiceNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFile.Columns["InvoiceDate"].HeaderText = "INV. DATE";
                dgvFile.Columns["InvoiceDate"].Width = 90;
                dgvFile.Columns["InvoiceDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFile.Columns["InvoiceDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                dgvFile.Columns["InvDateMailed"].HeaderText = "INV. MAIL DATE";
                dgvFile.Columns["InvDateMailed"].Width = 90;
                dgvFile.Columns["InvDateMailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFile.Columns["InvDateMailed"].DefaultCellStyle.Format = "MM/dd/yyyy";
            }
            dgvFile.Columns["ReportNo"].Width = 90;
            dgvFile.Columns["ReportNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["RevisionNo"].Width = 70;
            dgvFile.Columns["RevisionNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ReportDate"].Width = 80;
            dgvFile.Columns["ReportDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["ReportDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["CompanyCode"].Width = 75;
            dgvFile.Columns["PSSNo"].Width = 80;
            dgvFile.Columns["PSSNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ServiceCode"].Width = 75;
            dgvFile.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ServiceDesc"].Width = 200;
            dgvFile.Columns["DateESigned"].Width = 90;
            dgvFile.Columns["DateESigned"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateESigned"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["DateEMailed"].Width = 112;
            dgvFile.Columns["DateEMailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["DateEMailed"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["Sponsor"].Width = 200;
            dgvFile.Columns["SponsorID"].Width = 75;
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["Contact"].Width = 150;
            dgvFile.Columns["DateApproved"].Visible = false;
            dgvFile.Columns["ContactID"].Visible = false;
            dgvFile.Columns["Cancelled"].Visible = false;
            dgvFile.Columns["NoCharge"].Visible = false;
            dgvFile.Columns[0].Frozen = true;
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
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;
        }

        private void LoadContactsDDL(int cSpID)
        {
            DataTable dtX = new DataTable();
            dtX = PSSClass.Sponsors.SponsorOnCH(Convert.ToInt16(txtSponsorID.Text));
            if (dtX != null && dtX.Rows.Count > 0)
                lblCH.Visible = true;
            else
                lblCH.Visible = false;
            dtX.Dispose();

            dtContacts = PSSClass.FinalReports.ContactsDDLRpt(cSpID);
            if (dtContacts == null)
            {
                MessageBox.Show("Connection problems encountered. " + Environment.NewLine + "Please contact your system administrator.");
                txtSponsor.Focus(); 
                return;
            }
            else if (dtContacts.Rows.Count == 0)
            {
                dgvContacts.Visible = false; dgvSponsors.Visible = false; txtSponsorID.Focus();
                MessageBox.Show("No outstanding tests for final report.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            dgvContacts.DataSource = dtContacts;
            StandardDGVSetting(dgvContacts);
            dgvContacts.Columns[0].Width = 369;
            dgvContacts.Columns[1].Visible = false;
            dgvContacts.ColumnHeadersVisible = false;
            if (dtContacts.Rows.Count == 1)
            {
                txtContactID.Text = dtContacts.Rows[0]["ContactID"].ToString();
                txtContact.Text = dtContacts.Rows[0]["Contact"].ToString();
                dgvContacts.Visible = false; cboStudyDir.Select();
                LoadLogsForRpt();
            }
        }

        private void LoadFinRptMstr()
        {
            chkCancelled.Checked = false; chkNoCharge.Checked = false; lnkCancDtls.Visible = false;
            dtRptMstr = PSSClass.FinalReports.ExFinRptMstr(txtCmpyCode.Text, Convert.ToInt32(txtRptNo.Text));
            if (dtRptMstr == null || dtRptMstr.Rows.Count == 0)
            {
                MessageBox.Show("No master record found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bsRptMstr.DataSource = dtRptMstr;
        }

        private void LoadFinRptRev()
        {
            picWordRpt.Visible = false; lblDateScanned.Visible = false;lblScanDteRpt.Visible = false; 
            chkWordRpt.Checked = false; chkNextPage.Checked = false;

            dtRptRev = PSSClass.FinalReports.ExFinRptRev(txtCmpyCode.Text, Convert.ToInt32(txtRptNo.Text));
            if (dtRptRev == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bsRptRev.DataSource = dtRptRev;
            bnRevisions.BindingSource = bsRptRev;

            OpenControls(pnlRevisions, false);

            lblRevNo.Text = "R" + txtRevNo.Text;
            btnAddRev.Enabled = true; btnEditRev.Enabled = true; btnDelRev.Enabled = true; btnSaveRev.Enabled = false; btnCancelRev.Enabled = false;
            tlsFile.Enabled = true; btnClose.Visible = true;
            if (txtDateESigned.Text == "")
                txtESignedBy.Text = "";

            picWordRpt.Visible = false; lblDateScanned.Visible = false;
            lblDateScanned.Visible = false; lblScanDteRpt.Visible = false;
            lblDateScanned.Text = dtRptRev.Rows[0]["DateScanned"].ToString();

            if (lblDateScanned.Text != "")
            {
                lblScanDteRpt.Visible = true; lblDateScanned.Visible = true;
                lblScanDteRpt.BringToFront(); lblDateScanned.BringToFront();
            }
        }

        private void LoadFinRevLogs()
        {
            if (txtRptNo.Text.Trim() == "(New)" || txtRevNo.Text.Trim() == "")
                return;

            dtRptLogs = PSSClass.FinalReports.ExFinRevLogs(txtCmpyCode.Text, Convert.ToInt32(txtRptNo.Text), Convert.ToInt16(txtRevNo.Text));
            if (dtRptLogs == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bsRptLogs.DataSource = dtRptLogs;
            bnRptLogs.BindingSource = bsRptLogs;

            btnAddLog.Enabled = true; btnDelLog.Enabled = true; 
            tlsFile.Enabled = true; btnClose.Visible = true; bnRptLogs.Enabled = true;
            lblLogRevNo.Text = "R" + txtRevNo.Text;
            dtpDateOn.Enabled = false; dtpDateOff.Enabled = false;

            if (strGroup == "EXEC" || strGroup == "IT" || LogIn.nUserID == 73 || LogIn.nUserID == 247 || LogIn.nUserID == 394) //added K Kohan 11-14-2016, added Ruffy 2-1-2018, added Marlyn 2-26-2018
            {
                chkCancelled.Visible = true; chkNoCharge.Visible = true;
                chkCancelled.Enabled = true; chkNoCharge.Enabled = true;
                dtRptExt = PSSClass.FinalReports.CancelledRpt(Convert.ToInt32(txtRptNo.Text));
                bsRptExt.DataSource = dtRptExt;
                lnkCancDtls.Visible = true;
                OpenControls(pnlCancelFee, false); btnOKCanc.Text = "Cl&ose";
                if (dtRptExt.Rows.Count > 0)
                {
                    if (dtRptExt.Rows[0]["CancellationCode"].ToString() == "0")
                        rdoNoCharge.Checked = true;
                    else if (dtRptExt.Rows[0]["CancellationCode"].ToString() == "1")
                        rdoStandard.Checked = true;
                    else if (dtRptExt.Rows[0]["CancellationCode"].ToString() == "2")
                        rdoFixAmount.Checked = true;
                }
            }
            //Implementation of Multi-SCs Report
            //if (dtRptLogs.Rows.Count > 1)
            //{
            //    if (nMode == 0 && (strGroup == "QA" || strGroup == "IT"))
            //    {
            //        chkDataSource.Visible = true;
            //        if (txtDateApproved.Text == "")
            //            chkDataSource.Enabled = true;
            //        else
            //            chkDataSource.Enabled = false;

            //        strRptGBL = ""; strRptSC = "";
            //        DataTable dtDtls = PSSClass.FinalReports.GetFinRptDtls(Convert.ToInt32(txtRptNo.Text), Convert.ToInt16(txtRevNo.Text));
            //        if (dtDtls != null && dtDtls.Rows.Count > 0)
            //        {
            //            strRptGBL = dtDtls.Rows[0]["PSSNo"].ToString();
            //            strRptSC = dtDtls.Rows[0]["SC"].ToString();
            //            if (strRptGBL == cboGBLs.Text && strRptSC == cboSCs.Text)
            //                chkDataSource.Checked = true;
            //            else
            //                chkDataSource.Checked = false;
            //            dtDtls.Dispose();
            //        }
            //    }
            //    else
            //        chkDataSource.Visible = false;
            //}
            //else
            //    chkDataSource.Visible = false;
        }

        private void LoadData()
        {
            OpenControls(pnlRecord, false);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtCmpyCode.Text = dgvFile.CurrentRow.Cells["Companycode"].Value.ToString();
            txtRptNo.Text = dgvFile.CurrentRow.Cells["ReportNo"].Value.ToString();
            LoadFinRptMstr();
            LoadFinRptRev();
            LoadFinRevLogs();
            btnClose.Visible = true;
            btnAddRev.Enabled = false; btnDelRev.Enabled = false;
            btnAddLog.Enabled = false; btnDelLog.Enabled = false;
            chkNextPage.Enabled = true;
            chkLocked.Enabled = true;
            bnRevisions.Enabled = true;
            txtDescription.ReadOnly = true;

            if (txtDateApproved.Text == "")
            {
                btnQAESign.Enabled = true; chkLocked.Checked = false; chkLocked.Enabled = false;
                btnESign.Enabled = false; btnEMail.Enabled = false;
            }
            else
            {
                btnQAESign.Enabled = false; chkLocked.Checked = true; tsbEdit.Enabled = false;
                if (txtDateESigned.Text == "")
                {
                    btnESign.Enabled = true; btnEMail.Enabled = false;
                }
                else
                {
                    btnESign.Enabled = false; btnEMail.Enabled = true; chkLocked.Checked = true; tsbEdit.Enabled = false;
                    if (txtDateEMailed.Text == "")
                        btnEMail.Enabled = true;
                    else
                        btnEMail.Enabled = false;
                }
            }
            if (txtDateApproved.Text != "" && txtDateESigned.Text != "")
                btnEMail.Enabled = true;
            else if (txtDateApproved.Text != "")
            {
                btnEMail.Enabled = false; tsbEdit.Enabled = false;
            }
            else
                btnEMail.Enabled = false;

            if (nMode == 0 && strFileAccess == "FA" && (strGroup == "QA" || strGroup == "EXEC" || strGroup == "IT"))
                btnAddRev.Enabled = true;
            else
                btnAddRev.Enabled = false;
            //if (strGroup == "EXEC" || strGroup == "IT")
            //{
            //    DataTable dt = PSSClass.FinalReports.FinRptExtData(Convert.ToInt32(txtRptNo.Text));
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        if (dt.Rows[0]["DateCancelled"].ToString() != "")
            //            chkCancelled.Checked = true;
            //        else
            //            chkCancelled.Checked = false;
            //        if (dt.Rows[0]["NoCharge"].ToString() == "True")
            //            chkNoCharge.Checked = true;
            //        else
            //            chkNoCharge.Checked = false;
            //    }
            //    chkCancelled.Visible = true; chkNoCharge.Visible = true;
            //    dt.Dispose();
            //}
            //else
            //{
            //    chkCancelled.Visible = false; chkNoCharge.Visible = false;
            //}
            DataTable dtX = new DataTable();
            dtX = PSSClass.Sponsors.SponsorOnCH(Convert.ToInt16(txtSponsorID.Text));
            if (dtX != null && dtX.Rows.Count > 0)
                lblCH.Visible = true;
            else
                lblCH.Visible = false;
            dtX.Dispose();

            //Added 1/4/2017
            string sRptPath = "";
            for (int i = 0; i < cboWordFolder.Items.Count; i++)
            {
                sRptPath = @"M:\Rpts\" + cboWordFolder.Items[i].ToString()  + @"\C-";
                byte bWord = 0; string strWordExt = "";
                if (File.Exists(sRptPath + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + ".doc") == true)
                {
                    bWord = 1; strWordExt = "doc";
                }
                if (File.Exists(sRptPath + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + ".docx") == true)
                {
                    bWord = 1; strWordExt = "docx";
                }
                if (bWord == 1)
                {
                    lnkWordDoc.Text = sRptPath + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + strWordExt ;
                    picWordRpt.Visible = true; picWordRpt.BringToFront();  
                    cboWordFolder.SelectedIndex = i;
                    if (lblDateScanned.Text == "")
                    {
                        lblScanDteRpt.Visible = true; lblDateScanned.Visible = true; lblDateScanned.Text = "NOT SCANNED";
                    }
                    break;
                }
            }
            //Added 2-23-2017
            if (nLSw == 1)//Samples Login, QA 
            {
                tlsFile.Enabled = true;
                btnPrint.Enabled = true; btnMSWord.Enabled = true; cboGBLs.Enabled = true; cboSCs.Enabled = true; cboFormats.Enabled = true;
                lnkCancDtls.Enabled = false; chkCancelled.Enabled = false; chkNoCharge.Enabled = false;
                //chkDataSource.Visible = true; chkDataSource.Enabled = true;//added 11-7-2017
            }
            else if (nLSw == 2) //2 - Study Director
            {
                tlsFile.Enabled = false; btnPrint.Enabled = true; btnMSWord.Enabled = true; cboGBLs.Enabled = false; cboSCs.Enabled = false; cboFormats.Enabled = false;
                lnkCancDtls.Enabled = true; chkCancelled.Enabled = true; chkNoCharge.Enabled = true;
                //chkDataSource.Visible = false;//added 11-7-2017
            }
            else if (nLSw == 3) //3 - EMail Report
            {
                tlsFile.Enabled = false; btnPrint.Enabled = true; btnMSWord.Enabled = false; cboGBLs.Enabled = false; cboSCs.Enabled = false; cboFormats.Enabled = false;
                lnkCancDtls.Enabled = false; chkCancelled.Enabled = false; chkNoCharge.Enabled = false;
                //chkDataSource.Visible = false;//added 11-7-2017
            }
            cboWordFolder.Enabled = true;
        }

        private void LoadFinRptFees()
        {
            dtOtherFees.Rows.Clear();

            dtOtherFees = PSSClass.FinalReports.ExRptOtherFees(Convert.ToInt32(txtRptNo.Text));
            if (dtOtherFees == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            string strAccess = "";
            if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "InvoiceNo" && dgvFile.CurrentCell.Value.ToString() == "" &&
                dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "" && dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["SponsorID"].Value.ToString() != "130")
            {
                strAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "FinalBilling");
                if (strAccess == "")
                    return;

                nBilling = 1;
                int intOpen = PSSClass.General.OpenForm(typeof(FinalBilling));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(FinalBilling));
                }
                FinalBilling childForm = new FinalBilling();
                childForm.Text = "FINAL BILLING";
                childForm.MdiParent = this.MdiParent;
                childForm.nFB = 1;
                childForm.nSpID = Convert.ToInt16(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["SponsorID"].Value);
                childForm.strSpName = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["Sponsor"].Value.ToString();
                childForm.nConID = Convert.ToInt16(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["ContactID"].Value);
                childForm.strConName = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["Contact"].Value.ToString();
                childForm.Show();
            }
            else if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "InvoiceNo" && dgvFile.CurrentCell.Value.ToString() != "")
            {
                strAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "FinalBilling");
                if (strAccess == "")
                    return;

                nBilling = 0;
                int intOpen = PSSClass.General.OpenForm(typeof(FinalBilling));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(FinalBilling));
                }
                FinalBilling childForm = new FinalBilling();
                childForm.Text = "FINAL BILLING";
                childForm.MdiParent = this.MdiParent;
                childForm.nFB = 2;
                childForm.nInvceNo = Convert.ToInt32(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["InvoiceNo"].Value);
                childForm.Show();
            }
            else if (dgvFile.Rows.Count > 0)
                LoadData();
        }


        private void dgvCellChangedHandler(object sender, EventArgs e)
        {
            try
            {
                nIndex = dgvFile.CurrentCell.ColumnIndex;

                tsddbSearch.DropDownItems[nIndex].Select();
                tslSearchData.Text = tsddbSearch.DropDownItems[nIndex].Text;
                tstbSearchField.Text = tsddbSearch.DropDownItems[nIndex].Name;

                FileAccess();

                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() != "" ||
                    dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateESigned"].Value.ToString() != "" ||
                    dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "")
                {
                    tsbEdit.Enabled = false;
                }
                //if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["Cancelled"].Value.ToString() == "1")
                //{
                //    dgvFile.DefaultCellStyle.SelectionBackColor = Color.Red;
                //    //toolTip1.Show("Cancelled Report", dgvFile);
                //}
                //else
                //{
                //    dgvFile.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
                //    //toolTip1.Hide(dgvFile);
                //}
            }
            catch { }
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (nSw == 0)
            {
                nSw = 1;
                timer1.Enabled = true;
            }
            if (e.KeyChar == 13)
            {
                pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
                OpenControls(this.pnlRecord, false);
                if (dgvFile.Rows.Count > 0)
                    LoadData();
            }
            else if (e.KeyChar == 8)
            {
                tstbSearch.Text = tstbSearch.Text.Substring(0, tstbSearch.TextLength - 1);
            }
            else
            {
                tstbSearch.Text = tstbSearch.Text + e.KeyChar.ToString();
                nCtr = 0;
            }
        }

        private void dgvKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void CloseClickHandler(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            nMode = 0;
            this.Close();
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void BuildSearchItems()
        {
            DataTable dtQ = new DataTable();
            dtQ = PSSClass.FinalReports.FinalRptMaster(1);

            if (dtQ == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }

            int i = 0;
            int n = 0;

            arrCol = new string[dtQ.Columns.Count];

            ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

            foreach (DataColumn colFile in dtQ.Columns)
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
                if (dgvFile.Columns[i].Visible == true)
                    cklColumns.SetItemCheckState(i, CheckState.Checked);
                else
                    cklColumns.SetItemCheckState(i, CheckState.Unchecked);
                //}
                i += 1;
            }
            tsddbSearch.DropDownItems.AddRange(items);
            tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[4];

            items[0] = new ToolStripMenuItem();
            items[0].Text = "Audit Trail - Master File";
            items[0].Click += new EventHandler(PrtAuditClickHandler);

            items[1] = new ToolStripMenuItem();
            items[1].Text = "Audit Trail - Revisions";
            items[1].Click += new EventHandler(PrtAuditClickHandler);

            items[2] = new ToolStripMenuItem();
            items[2].Text = "Audit Trail - PO Details";
            items[2].Click += new EventHandler(PrtAuditClickHandler);

            items[3] = new ToolStripMenuItem();
            items[3].Text = "Audit Trail - Control Page Numbers";
            items[3].Click += new EventHandler(PrtAuditClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void PrtAuditClickHandler(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (rptTitle.IndexOf("Audit") != -1)
                {
                    LabRpt rptSC = new LabRpt();
                    rptSC.WindowState = FormWindowState.Maximized;

                    if (rptTitle == "Audit Trail - Master File")
                    {
                        rptSC.rptFileName = "FINAL REPORT MASTER FILE";
                        rptSC.rptName = "Audit Trail - Final Report Master File";
                    }
                    else if (rptTitle == "Audit Trail - Revisions")
                    {
                        rptSC.rptFileName = "FINAL REPORT REVISIONS";
                        rptSC.rptName = "Audit Trail - Final Report Revisions";
                    }
                    else if (rptTitle == "Audit Trail - PO Details")
                    {
                        rptSC.rptFileName = "PURCHASE ORDER DETAILS";
                        rptSC.rptName = "Audit Trail - PO Details";
                    }
                    else if (rptTitle == "Audit Trail - Control Page Numbers")
                    {
                        rptSC.rptFileName = "CONTROL PAGE NUMBERS";
                        rptSC.rptName = "Audit Trail - Control Page Numbers";
                    }

                    rptSC.Show();
                }
            }
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

        private void txtSponsorID_TextChanged(object sender, EventArgs e)
        {
            if (txtSponsorID.Text.Trim() != "" && nMode != 0)
            {
                try
                {
                    DataView dvwSponsors;
                    dvwSponsors = new DataView(dtSponsors, "SponsorID=" + Convert.ToInt16(txtSponsorID.Text.Trim()), "SponsorID", DataViewRowState.CurrentRows);
                    PSSClass.General.DGVSetUp(dgvSponsors, dvwSponsors, 369);
                }
                catch { }
            }
        }

        private void txtContactEnterHandler(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    dgvSponsors.Visible = false;
                    LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                    if (dtContacts.Rows.Count == 0)
                    {
                        dgvContacts.Visible = false;
                    }
                    else
                    {
                        dgvContacts.Visible = true; dgvContacts.BringToFront();
                    }
                }
                catch { }
            }
        }

        private void txtContactIDEnterHandler(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvContacts.Visible = true; dgvSponsors.Visible = false;
            }
        }

        private void txtContactID_TextChanged(object sender, EventArgs e)
        {
            if (txtContactID.Text.Trim() != "" && nMode != 0)
            {
                try
                {
                    DataView dvwContacts;
                    dvwContacts = new DataView(dtContacts, "ContactID=" + Convert.ToInt16(txtContactID.Text.Trim()), "ContactID", DataViewRowState.CurrentRows);
                    PSSClass.General.DGVSetUp(dgvContacts, dvwContacts, 369);
                }
                catch { }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr += 1;
            if (nCtr > 1)
            {
                DataTable dt = new DataTable();
                dt = PSSClass.FinalReports.FinalRptMaster(1);
                PSSClass.DataEntry.DGVSearch(tstbSearchField.Text, tstbSearch.Text, dt, bsFile);
                nCtr = 0;
                tstbSearch.Text = "";
                timer1.Enabled = false;
                nSw = 0;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlPDF.Visible = false;

            if (nMode != 0 || nR != 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task(s)?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }

            if (nLSw == 1 || nLSw == 2 || nLSw == 3)
            {
                nLSw = 0;
                SendKeys.Send("{F12}");
                return;
            }
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false; dgvFile.Focus();
            FileAccess();
            if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() != "" ||
                dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateESigned"].Value.ToString() != "" ||
                dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "")
            {
                tsbEdit.Enabled = false;
            }
        }

        private void pnlRecord_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlRecord_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void pnlRecord_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void FinalReports_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "FinalReports");
            strGroup = PSSClass.Users.UserGroupCode(LogIn.nUserID);

            cboWordFolder.Items.Add(DateTime.Now.Year.ToString());
            int nYr = DateTime.Now.Year;
            for (int y = 0; y < 5; y++)
            {
                nYr -= 1;
                cboWordFolder.Items.Add(nYr.ToString());
            }
            
            LoadRecords();
            LoadSponsorsDDL();
            LoadStudyDirectors();

            if (nLSw == 0)
                BuildSearchItems();

            BuildPrintItems();

            dtSC = PSSClass.ServiceCodes.SCDDL();
            //Report Master File
            dtRptMstr.Columns.Add("CompanyCode", typeof(string));
            dtRptMstr.Columns.Add("ReportNo", typeof(string));
            dtRptMstr.Columns.Add("SponsorID", typeof(Int16));
            dtRptMstr.Columns.Add("SponsorName", typeof(string));
            dtRptMstr.Columns.Add("ContactID", typeof(Int32));
            dtRptMstr.Columns.Add("ContactName", typeof(string));
            dtRptMstr.Columns.Add("ReportNotes", typeof(string));
            dtRptMstr.Columns.Add("InternalNotes", typeof(string));
            dtRptMstr.Columns.Add("Purpose", typeof(string));
            dtRptMstr.Columns.Add("Method", typeof(string));
            dtRptMstr.Columns.Add("Results", typeof(string));
            dtRptMstr.Columns.Add("Conclusion", typeof(string));
            dtRptMstr.Columns.Add("Memorandum", typeof(string));
            //dtRptMstr.Columns.Add("Cancelled", typeof(bool));
            //dtRptMstr.Columns.Add("NoCharge", typeof(bool));
            bsRptMstr.DataSource = dtRptMstr;
            //Data Bindings
            txtCmpyCode.DataBindings.Add("Text", bsRptMstr, "CompanyCode", true);
            txtSponsorID.DataBindings.Add("Text", bsRptMstr, "SponsorID", true);
            txtSponsor.DataBindings.Add("Text", bsRptMstr, "SponsorName", true);
            txtContactID.DataBindings.Add("Text", bsRptMstr, "ContactID", true);
            txtContact.DataBindings.Add("Text", bsRptMstr, "ContactName", true);
            txtRptNotes.DataBindings.Add("Text", bsRptMstr, "ReportNotes", true);
            txtIntNotes.DataBindings.Add("Text", bsRptMstr, "InternalNotes", true);
            txtPurpose.DataBindings.Add("Text", bsRptMstr, "Purpose", true);
            txtMethod.DataBindings.Add("Text", bsRptMstr, "Method", true);
            txtResults.DataBindings.Add("Text", bsRptMstr, "Results", true);
            txtConclusion.DataBindings.Add("Text", bsRptMstr, "Conclusion", true);
            txtMemo.DataBindings.Add("Text", bsRptMstr, "Memorandum", true);
            //chkCancelled.DataBindings.Add("Checked", bsRptMstr, "Cancelled", true);
            //chkNoCharge.DataBindings.Add("Checked", bsRptMstr, "NoCharge", true);

            //Report Revisions
            dtRptRev.Columns.Add("CompanyCode", typeof(string));
            dtRptRev.Columns.Add("RevisionNo", typeof(Int16));
            dtRptRev.Columns.Add("ReportDate", typeof(DateTime));
            dtRptRev.Columns.Add("CreatedBy", typeof(string));
            dtRptRev.Columns.Add("ReasonCode", typeof(Int16));
            dtRptRev.Columns.Add("Reason", typeof(string));
            dtRptRev.Columns.Add("DateESigned", typeof(DateTime));
            dtRptRev.Columns.Add("DateEMailed", typeof(DateTime));
            dtRptRev.Columns.Add("WordReport", typeof(bool));
            dtRptRev.Columns.Add("TableNextPage", typeof(bool));
            dtRptRev.Columns.Add("QAApprovedBy", typeof(string));
            dtRptRev.Columns.Add("DateApproved", typeof(DateTime));
            dtRptRev.Columns.Add("ESignedBy", typeof(string));
            dtRptRev.Columns.Add("EMailedBy", typeof(string));
            dtRptRev.Columns.Add("ESignedByID", typeof(Int16));
            dtRptRev.Columns.Add("StudyDirID", typeof(Int16));
            dtRptRev.Columns.Add("DateScanned", typeof(DateTime));
            bsRptRev.DataSource = dtRptRev;
            bnRevisions.BindingSource = bsRptRev;
            //Data Bindings
            txtRevNo.DataBindings.Add("Text", bsRptRev, "RevisionNo");
            dtpRevDate.DataBindings.Add("Value", bsRptRev, "ReportDate", true);
            txtReason.DataBindings.Add("Text", bsRptRev, "Reason");
            cboReason.DataBindings.Add("SelectedIndex", bsRptRev, "ReasonCode", true);
            cboStudyDir.DataBindings.Add("SelectedValue", bsRptRev, "StudyDirID", true);
            txtCreatedBy.DataBindings.Add("Text", bsRptRev, "CreatedBy");
            txtDateApproved.DataBindings.Add("Text", bsRptRev, "DateApproved", true);
            txtDateESigned.DataBindings.Add("Text", bsRptRev, "DateESigned", true);
            txtDateEMailed.DataBindings.Add("Text", bsRptRev, "DateEMailed", true);
            txtQAApprover.DataBindings.Add("Text", bsRptRev, "QAApprovedBy", true);
            txtESignedBy.DataBindings.Add("Text", bsRptRev, "ESignedBy", true);
            txtEMailedBy.DataBindings.Add("Text", bsRptRev, "EMailedBy", true);
            chkWordRpt.DataBindings.Add("Checked", bsRptRev, "WordReport", true);
            chkNextPage.DataBindings.Add("Checked", bsRptRev, "TableNextPage", true);
            lblDateScanned.DataBindings.Add("Text", bsRptRev, "DateScanned", true);
            
            //Report GBLs (Logs)
            dtRptLogs.Columns.Add("Companycode", typeof(string));
            dtRptLogs.Columns.Add("RevisionNo", typeof(Int16));
            dtRptLogs.Columns.Add("LCompanyCode", typeof(string));
            dtRptLogs.Columns.Add("PSSNo", typeof(string));
            dtRptLogs.Columns.Add("ServiceCode", typeof(string));
            dtRptLogs.Columns.Add("DataFormat", typeof(string));
            dtRptLogs.Columns.Add("ServiceDesc", typeof(string));
            dtRptLogs.Columns.Add("Description", typeof(string));
            dtRptLogs.Columns.Add("DateOn", typeof(DateTime));
            dtRptLogs.Columns.Add("DateOff", typeof(DateTime));
            bsRptLogs.DataSource = dtRptLogs; ;
            bnRptLogs.BindingSource = bsRptLogs;
            //Data Bindings
            dtpDateOn.DataBindings.Add("Value", bsRptLogs, "DateOn", true);
            dtpDateOff.DataBindings.Add("Value", bsRptLogs, "DateOff", true);
            txtSCDesc.DataBindings.Add("Text", bsRptLogs, "ServiceDesc", true);
            txtDescription.DataBindings.Add("Text", bsRptLogs, "Description", true);
            cboGBLs.DataBindings.Add("Text", bsRptLogs, "PSSNo", true);
            cboSCs.DataBindings.Add("Text", bsRptLogs, "ServiceCode", true);
            cboFormats.DataBindings.Add("Text", bsRptLogs, "DataFormat", true);
            txtLCmpyCode.DataBindings.Add("Text", bsRptLogs, "LCompanyCode", true);

            //Report Other Fees
            dtOtherFees.Columns.Add("ReportNo", typeof(string));
            dtOtherFees.Columns.Add("ServiceCode", typeof(Int16));
            dtOtherFees.Columns.Add("ServiceDesc", typeof(string));
            dtOtherFees.Columns.Add("TestDesc1", typeof(string));
            dtOtherFees.Columns.Add("BillQty", typeof(decimal));
            dtOtherFees.Columns.Add("UnitPrice", typeof(decimal));
            dtOtherFees.Columns.Add("Amount", typeof(decimal));
            dtOtherFees.Columns.Add("QuotationNo", typeof(string));
            dtOtherFees.Columns.Add("RevisionNo", typeof(Int16));
            dtOtherFees.Columns.Add("ControlNo", typeof(Int16));
            dtOtherFees.Columns.Add("DateCreated", typeof(DateTime));
            dtOtherFees.Columns.Add("CreatedByID", typeof(Int16));
            dtOtherFees.Columns.Add("LastUpdate", typeof(DateTime));
            dtOtherFees.Columns.Add("LastUserID", typeof(Int16));
            dtOtherFees.Columns.Add("QCmpyCode", typeof(string));
            dtOtherFees.Columns.Add("LCmpyCode", typeof(string));
            dtOtherFees.Columns.Add("RCmpyCode", typeof(string));
            bsOtherFees.DataSource = dtOtherFees;

            //Report Extended Data
            dtRptExt.Columns.Add("Cancelled", typeof(bool));
            dtRptExt.Columns.Add("CancellationFee", typeof(decimal));
            dtRptExt.Columns.Add("CancellationCode", typeof(Int16));
            dtRptExt.Columns.Add("NoCharge", typeof(bool));
            bsRptExt.DataSource = dtRptExt;

            chkCancelled.DataBindings.Add("Checked", bsRptExt, "Cancelled");
            txtCancFee.DataBindings.Add("Text", bsRptExt, "CancellationFee");
            txtCancCode.DataBindings.Add("Text", bsRptExt, "CancellationCode");
            chkNoCharge.DataBindings.Add("Checked", bsRptExt, "NoCharge");

            if (nLSw == 1 || nLSw == 2 || nLSw == 3)
            {
                PSSClass.General.FindRecord("ReportNo", nRptNo.ToString() , bsFile, dgvFile);
                dgvFile.Select();

                LoadData();
                //SendKeys.Send("{Enter}");
            }
        }

        private void LoadStudyDirectors()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Employees.StudyDirectors();
            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            cboStudyDir.DataSource = dt;
            cboStudyDir.DisplayMember = "EmployeeName";
            cboStudyDir.ValueMember = "EmployeeID";
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSponsors;
                if (chkWildSpCon.Checked == true)
                    dvwSponsors = new DataView(dtSponsors, "SponsorName like '%" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
                else
                    dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
                dvwSetUp(dgvSponsors, dvwSponsors);
            }
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            txtContactID.Clear(); txtContact.Clear();
            dgvSponsors.Visible = false;
            LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
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
                txtContactID.Clear(); txtContact.Clear();
                dgvSponsors.Visible = false;
                LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
            }
            else if (e.KeyChar == 27)
            {
                dgvSponsors.Visible = false;
            }
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                if (e.KeyChar == 13)
                {
                    if (txtSponsorID.Text.Trim() != "")
                    {
                        txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                        LoadContactsDDL(Convert.ToInt16(txtSponsorID.Text));
                        dgvSponsors.Visible = false;
                    }
                }
                else if (e.KeyChar == 27)
                {
                    dgvSponsors.Visible = false;
                }
                else
                {
                    txtSponsor.Text = ""; txtContactID.Text = ""; txtContact.Text = ""; dgvContacts.Visible = false;
                }
            }
        }

        private void txtContactID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode != 0)
            {
                if (e.KeyChar == 13)
                    try
                    {
                        txtContact.Text = PSSClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
                        dgvContacts.Visible = false; txtRptNotes.Focus();
                        LoadLogsForRpt();
                    }
                    catch { }
                else if (e.KeyChar == 27)
                {
                    dgvContacts.Visible = false;
                }
                else
                {
                    txtContact.Text = "";
                }
            }
        }

        private void dgvContacts_DoubleClick(object sender, EventArgs e)
        {
            if (dgvContacts.Rows.Count == 0)
            {
                MessageBox.Show("No Sponsor selected. " + Environment.NewLine + "Contacts list is empty.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsorID.Focus();
                return;
            }
            txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
            txtContactID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
            dgvContacts.Visible = false; dgvSponsors.Visible = false;
            txtRptNotes.Focus();
            LoadLogsForRpt();
        }

        private void LoadLogsForRpt()
        {
            try
            {
                dtGBLDDL = PSSClass.FinalReports.LogsForRpt(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtContactID.Text));
                if (dtGBLDDL == null || dtGBLDDL.Rows.Count == 0)
                {
                    dgvSponsors.Visible = false; dgvContacts.Visible = false;
                    MessageBox.Show("No outstanding tests for final report.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                cboGBLs.DataSource = dtGBLDDL;

                cboGBLs.DisplayMember = "LogNo";
                cboGBLs.ValueMember = "CompanyCode";
                if (dtGBLDDL.Rows.Count > 0)
                {
                    cboGBLs.SelectedIndex = 0;
                }
            }
            catch { }
        }

        private void dgvContacts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvContacts_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtContact.Text = dgvContacts.CurrentRow.Cells[0].Value.ToString();
                txtContactID.Text = dgvContacts.CurrentRow.Cells[1].Value.ToString();
                dgvContacts.Visible = false;
                LoadLogsForRpt();
            }
            else if (e.KeyChar == 27)
            {
                dgvContacts.Visible = false;
            }
        }

        
        private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                dgvSponsors.Visible = false; dgvContacts.Visible = false;
            }
        }

        private void txtPurpose_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtResults_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void txtMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;

        }

        private void txtRevNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtReason_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtRevNo.Text == "0")
                e.Handled = true;
        }

        private void txtESignedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtDateESigned_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtDateEMailed_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtEMailedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtCreatedBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSCDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtContact_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    DataView dvwContacts;
                    if (chkWildSpCon.Checked == true)
                        dvwContacts = new DataView(dtContacts, "Contact like '%" + txtContact.Text.Trim().Replace("'", "''") + "%'", "Contact", DataViewRowState.CurrentRows);
                    else
                        dvwContacts = new DataView(dtContacts, "Contact like '" + txtContact.Text.Trim().Replace("'", "''") + "%'", "Contact", DataViewRowState.CurrentRows);
                    dvwSetUp(dgvContacts, dvwContacts);
                }
                catch { }
            }
        }

        private void cboGBLs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                SetDDL();
            }
        }

        private void cboSCs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(cboSCs.Text), dtSC);
                    dtRptLogs.Rows[bsRptLogs.Position]["CompanyCode"] = cboGBLs.SelectedValue.ToString();
                    dtRptLogs.Rows[bsRptLogs.Position]["ServiceCode"] = cboSCs.Text;
                    dtRptLogs.Rows[bsRptLogs.Position]["ServiceDesc"] = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(cboSCs.Text), dtSC);

                    cboFormats.DataSource = null;
                    //Setup Table Formats
                    DataTable dtFormats = new DataTable();
                    dtFormats = PSSClass.FinalReports.ExTableFormats(Convert.ToInt16(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text));
                    cboFormats.DataSource = dtFormats;
                    cboFormats.DisplayMember = "FormatNo";
                    cboFormats.ValueMember = "FormatNo";
                    dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = dtFormats.Rows[0]["FormatNo"];

                    string strFormatNo = PSSClass.FinalReports.ExFormatNo(Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(cboSCs.Text)).ToString();
                    if (strFormatNo != "0" && strFormatNo != "")
                    {
                        cboFormats.Text = strFormatNo;
                        dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = strFormatNo;
                    }
                    else
                    {
                        cboFormats.Text = "1";
                        dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = "1";
                    }
                }
                catch { }
            }
        }

        private void txtDescription_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnAddLog_Click(object sender, EventArgs e)
        {
            DataRow dR;
            dR = dtRptLogs.NewRow();
            dR["CompanyCode"] = txtCmpyCode.Text;
            dR["RevisionNo"] = txtRevNo.Text;
            dR["PSSNo"] = "";
            dR["ServiceCode"] = "";
            dR["DataFormat"] = "";
            dR["ServiceDesc"] = "";
            dR["Description"] = "";
            dR["DateOn"] = DateTime.Now;
            dR["DateOff"] = DateTime.Now;
            dtRptLogs.Rows.Add(dR);
            bsRptLogs.DataSource = dtRptLogs;
            bsRptLogs.Position = dtRptLogs.Rows.Count - 1;
            btnDelLog.Enabled = true;

            LoadLogsForRpt();

            string strGBL = "";
            byte nXSw = 0;

            for (int i = 0; i < dtGBLDDL.Rows.Count; i++)
            {
                nXSw = 0;
                strGBL = dtGBLDDL.Rows[i]["LogNo"].ToString();
                for (int j = 0; j < dtRptLogs.Rows.Count; j++)
                {
                    if (dtGBLDDL.Rows[i]["LogNo"].ToString() == dtRptLogs.Rows[j]["PSSNo"].ToString() && dtRptLogs.Rows[j].RowState.ToString() != "Deleted")
                    {
                        nXSw = 1;
                    }
                }
                if (nXSw == 0)
                {
                    break;
                }
            }
            dtRptLogs.Rows[bsRptLogs.Position]["PSSNo"] = strGBL;
        }

        private void txtConclusion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
        }

        private void cboSCs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
            else if ((e.KeyChar != 13 && e.KeyChar != 8 && e.KeyChar < 48) || e.KeyChar > 57)
                e.Handled = true;
            else if (e.KeyChar == 13)
            {
                try
                {
                    //Update Table
                    dtRptLogs.Rows[bsRptLogs.Position]["ServiceDesc"] = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(cboSCs.Text), dtSC);
                    dtRptLogs.Rows[bsRptLogs.Position]["ServiceCode"] = cboSCs.Text;

                    cboFormats.DataSource = null;
                    //Setup Table Formats
                    DataTable dtFormats = new DataTable();
                    dtFormats = PSSClass.FinalReports.ExTableFormats(Convert.ToInt16(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text));
                    cboFormats.DataSource = dtFormats;
                    cboFormats.DisplayMember = "FormatNo";
                    cboFormats.ValueMember = "FormatNo";
                    dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = dtFormats.Rows[0]["FormatNo"];

                    string strFormatNo = PSSClass.FinalReports.ExFormatNo(Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(cboSCs.Text)).ToString();
                    if (strFormatNo != "")
                    {
                        cboFormats.Text = strFormatNo;
                        dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = strFormatNo;
                    }
                }
                catch { }
                SendKeys.Send("{TAB}");
            }
            else
            {
                txtSCDesc.Text = ""; cboFormats.Text = "";
            }
        }

        private void cboFormats_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                dgvSponsors.Visible = false;
            }
        }

        private void txtContact_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                dgvContacts.Visible = false;
            }
        }

        private void btnDelLog_Click(object sender, EventArgs e)
        {
            if (dtRptLogs.Rows[bsRptLogs.Position].RowState.ToString() == "Unchanged" || dtRptLogs.Rows[bsRptLogs.Position].RowState.ToString() == "Modified")
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to delete GBL #" + cboGBLs.Text + ", SC " + cboSCs.Text + "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.Yes)
                {
                    SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();

                    SqlCommand sqlcmd = new SqlCommand();

                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                    sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
                    sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(cboGBLs.Text));
                    sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(cboSCs.Text));
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spDelFinRptLog";

                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //Update Log Test's ReportNo field
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;
                    sqlcmd.Parameters.AddWithValue("@RptNo", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
                    sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(cboGBLs.Text));
                    sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(cboSCs.Text));
                    sqlcmd.Parameters.AddWithValue("@FormatNo", Convert.ToInt16(cboFormats.Text));
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
                    sqlcnn.Close(); sqlcnn.Dispose();
                    LoadFinRevLogs();
                }
            }
            else
            {
                for (int i = 0; i < dtRptLogs.Rows.Count; i++)
                {
                    if (dtRptLogs.Rows[i].RowState.ToString() != "Deleted" && cboGBLs.Text == dtRptLogs.Rows[i]["PSSNo"].ToString() &&
                        cboSCs.Text == dtRptLogs.Rows[i]["ServiceCode"].ToString())
                    {
                        dtRptLogs.Rows[i].Delete();
                    }
                }
                bsRptLogs.DataSource = dtRptLogs;
                bsRptLogs.Position = dtRptLogs.Rows.Count - 1;
                //LoadRevLogRow(null, null);
                if (dtRptLogs.Rows.Count == 1)
                    btnDelLog.Enabled = false;
            }
        }

        private void btnDelRev_Click(object sender, EventArgs e)
        {
            if (PSSClass.Users.UserGroupCode(LogIn.nUserID) != "QA" && PSSClass.Users.UserGroupCode(LogIn.nUserID) != "EXEC")
            {
                MessageBox.Show("You are not authorized to delete revisions.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (txtRevNo.Text == "0" && dtRptRev.Rows.Count == 1)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Deleting this revision (R0) would delete the whole report!" + Environment.NewLine + "Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dReply == DialogResult.Yes)
                {
                    SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spDelFinRpt";

                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to delete R" + txtRevNo.Text + "?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dReply == DialogResult.Yes)
                {
                    SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                    ;

                    SqlCommand sqlcmd = new SqlCommand();

                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                    sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spDelFinRptRev";

                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            LoadRecords();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
        }

        private void btnEditRev_Click(object sender, EventArgs e)
        {
            nR = 2;
            if (txtDateApproved.Text != "")
            {
                if (txtDateESigned.Text == "")
                {
                    btnESign.Enabled = true;
                    btnEMail.Enabled = false;
                }
                else
                {
                    btnESign.Enabled = false;
                    if (txtDateEMailed.Text == "")
                        btnEMail.Enabled = true;
                    else
                        btnEMail.Enabled = false;
                }
            }
            OpenControls(pnlRevisions, true);
            dtpRevDate.Enabled = false;
            btnAddRev.Enabled = false; btnEditRev.Enabled = false; btnDelRev.Enabled = false; btnSaveRev.Enabled = true; btnCancelRev.Enabled = true;
            tlsFile.Enabled = false; btnClose.Visible = false;
        }

        private void btnQAESign_Click(object sender, EventArgs e)
        {
            byte nCP = 0;
            string strSC = "", strLogNo = "";

            for (int i = 0; i < dtRptLogs.Rows.Count; i++)
            {
                strSC = dtRptLogs.Rows[i]["ServiceCode"].ToString();
                strLogNo = dtRptLogs.Rows[i]["PSSNo"].ToString();

                DataTable dt = PSSClass.FinalReports.CtrlPageRet(Convert.ToInt32(strLogNo), Convert.ToInt16(strSC));
                if (dt != null && dt.Rows.Count > 0)
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("Control pages are not yet accounted for." + Environment.NewLine + Environment.NewLine + "If this report does not require" + Environment.NewLine + "Control Pages, click OK to proceed.",
                                                Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
                    if (dReply == DialogResult.Cancel)
                    {
                        MessageBox.Show("Control pages are not yet accounted for." + Environment.NewLine + "Please check your control pages.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        nCP = 1;
                        break;
                    }
                }
            }

            if (nCP == 1)
                return;

            using (ESignature eSignature = new ESignature())
            {
                eSignature.Location = new Point(245, 250);
                eSignature.eRptNo= Convert.ToInt32(txtRptNo.Text);
                eSignature.eRevNo = Convert.ToInt32(txtRevNo.Text);
                eSignature.eSign = 1;
                if (eSignature.ShowDialog() == DialogResult.OK)
                {
                    if (dtRptLogs.Rows.Count > 1)
                    {
                        string strRptDtls = "<ExtendedData><ReportData>" +
                                            "<PSSNo>" + cboGBLs.Text + "</PSSNo>" +
                                            "<SC>" + cboSCs.Text + "</SC>" +
                                            "</ReportData></ExtendedData>";
                        PSSClass.FinalReports.UpdFinRptDtls(Convert.ToInt32(txtRptNo.Text), strRptDtls);
                    }
                    LoadData();
                    AddEditMode(false);
                    tsbEdit.Enabled = false;
                    nMode = 0;
                }
            }
        }

        private void btnESign_Click(object sender, EventArgs e)
        {
            using (ESignature eSignature = new ESignature())
            {
                eSignature.eRptNo = Convert.ToInt32(txtRptNo.Text);
                eSignature.eRevNo = Convert.ToInt32(txtRevNo.Text);
                eSignature.eSign = 2;
                if (eSignature.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    AddEditMode(false);
                    tsbEdit.Enabled = false;
                    nMode = 0;
                }
            }
        }

        private void btnEMail_Click(object sender, EventArgs e)
        {
            if (strFileAccess != "FA")
            {
                MessageBox.Show("You have no permission to" + Environment.NewLine + "e-mail reports at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (nMode != 0) 
            {
                MessageBox.Show("This report is in add or edit mode. " + Environment.NewLine + "Please save report or cancel changes made." + Environment.NewLine + "E-mail task is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable dtX = new DataTable();
            dtX = PSSClass.Sponsors.SponsorOnCH(Convert.ToInt16(txtSponsorID.Text));
            if (dtX != null && dtX.Rows.Count > 0)
            {
                MessageBox.Show("Sponsor is currently on Credit Hold." + Environment.NewLine + "Process is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dtX.Dispose();
                return;
            }
            dtX.Dispose();
            //Implementation of Final Report Details - AMDC 11/10/2017, Revised 1-23-2018
            if (dtRptLogs.Rows.Count > 1)
            {
                for (int k = 0; k < dtRptLogs.Rows.Count; k++)
                {
                    DataTable dtDtls = PSSClass.FinalReports.GetTestDataValues(Convert.ToInt32(dtRptLogs.Rows[k]["PSSNo"]), Convert.ToInt16(dtRptLogs.Rows[k]["ServiceCode"]));
                    if (dtDtls != null && dtDtls.Rows.Count > 0)
                    {
                        string strPSSNo = "", strSC = ""; byte nDSw = 0;
                        for (int j = 0; j < dtDtls.Rows.Count; j++)
                        {
                            for (int m = 0; m < 10; m++)
                            {
                                if (dtDtls.Rows[j]["TestData" + (m + 1).ToString()] != DBNull.Value)
                                {
                                    strPSSNo = dtDtls.Rows[j]["LogNo"].ToString();
                                    strSC = dtDtls.Rows[j]["SC"].ToString();
                                    nDSw = 1;
                                    break;
                                }
                            }
                            if (nDSw == 1)
                                break;
                        }
                        if (nDSw == 1)
                        {
                            bsRptLogs.Position = k;
                            break;
                        }
                        dtDtls.Dispose();
                    }
                }
            }
            Tables CrTables;
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            crConnectionInfo.Type = ConnectionInfoType.SQL;
            crConnectionInfo.ServerName = "172.16.4.12";
            crConnectionInfo.DatabaseName = "PTS";
            crConnectionInfo.IntegratedSecurity = false;
            crConnectionInfo.UserID = "sa";
            crConnectionInfo.Password = "Pass2018";
            crtableLogoninfo.ConnectionInfo = crConnectionInfo;

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            string strLotNo = "";

            if (txtDateEMailed.Text != "")
            {
                string strConEMail = "";

                sqlcmd = new SqlCommand("SELECT EMailAddress FROM ContactEMAddresses WHERE ContactID = " + Convert.ToInt16(txtContactID.Text) +
                                        " AND FinalReports = 1", sqlcnn);
                sqldr = sqlcmd.ExecuteReader();
                if (sqldr.HasRows)
                {
                    sqldr.Read();
                    strConEMail = sqldr.GetValue(0).ToString();
                }
                sqldr.Close(); sqlcmd.Dispose();
                sqlcnn.Close(); sqlcnn.Dispose();

                txtTo.Text = strConEMail.Replace(";", "; ");

                pnlEMail.Location = new Point(28, 198); pnlEMail.Visible = true; pnlRecord.Enabled = false; pnlEMail.BringToFront();

                strLotNo = PSSClass.Samples.LotNo(Convert.ToInt32(cboGBLs.Text));
                if (strLotNo != "")
                    txtSubject.Text = "C-" + txtRptNo.Text + "-R" + txtRevNo.Text + " Lot No: " + strLotNo + " Article Name: " + PSSClass.Samples.ArticleDesc(txtCmpyCode.Text, Convert.ToInt32(cboGBLs.Text));
                else
                    txtSubject.Text = "C-" + txtRptNo.Text + "-R" + txtRevNo.Text + " Article Name: " + PSSClass.Samples.ArticleDesc(txtCmpyCode.Text, Convert.ToInt32(cboGBLs.Text));

                txtBody.Text = "Dear " + PSSClass.Contacts.ConFirstName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text)) + ", " + Environment.NewLine + Environment.NewLine +
                            "Thank you for your support of Prince Sterilizaton Services, LLC. " + Environment.NewLine + "Please find attached our Final Report on the samples of " + PSSClass.Samples.ArticleDesc(txtCmpyCode.Text, Convert.ToInt32(cboGBLs.Text)) + "." +
                            Environment.NewLine + Environment.NewLine;  
                            //"<i>Prince Sterilization Services is pleased to announce the availability of our Report Server. This advanced technology will " + Environment.NewLine + "make <u>your job easier</u>.  " +
                            //"Specifically you will have real-time access to up to 6 months of your e-reports. Perfect for when you" + Environment.NewLine + "cannot locate a report. You will be able to login to the protected " +
                            //"server to retrieve your results without having to send an" + Environment.NewLine + "e-mail or pick up the phone.</i>" + Environment.NewLine + Environment.NewLine +
                            //"If you are interested, Please contact with " + "<a href=" + "mailto:kkohan@princesterilization.com " + ">Kristah Kohan</a> for further details. " + Environment.NewLine + Environment.NewLine +
                            //"To see your most current report, as well as a historical listing of previously issued reports, please click on the " + Environment.NewLine +
                            //"<a href=" + "http://www.princesterilization.com" + ">Prince Sterilization Services Homepage</a> and select Login." + Environment.NewLine + Environment.NewLine;

                lnkReport.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\eFinalReports\" +  dtpRevDate.Value.Year.ToString() +  "\\C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + ".pdf";
                return;
            }
            tlsFile.Enabled = false;
            string strRptName = ""; byte bCFR = 0;
            DataTable dt = new DataTable();
            dt = PSSClass.FinalReports.SpSCRpt(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(cboSCs.Text), Convert.ToInt16(cboFormats.Text));
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No defined report for this Sponsor/Service Code." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dt.Rows[0]["FinalReportID"] != null && dt.Rows[0]["FinalReportID"].ToString().Trim() != "")
            {
                strRptName = dt.Rows[0]["FinalReportID"].ToString().Trim();
                bCFR = 1;
            }
            else
                strRptName = dt.Rows[0]["TableReportID"].ToString().Trim();

            crDoc = new ReportDocument();

            string rpt = "";
            if (bCFR == 1)
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + strRptName + ".rpt";
                crDoc.Load(rpt);
            }
            else if (cboSCs.Text == "297")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_297.rpt";
                crDoc.Load(rpt);

                if (strRptName.IndexOf("297_NVP") != -1)
                {
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\GBLNJ4\GIS\Reports\297_NVP1a.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                    //Clone
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
                else
                {
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\GBLNJ4\GIS\Reports\297_1.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpta", @"\\GBLNJ4\GIS\Reports\297_1_1345a.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptb", @"\\GBLNJ4\GIS\Reports\297_1_1345b.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptc", @"\\GBLNJ4\GIS\Reports\297_1_1345c.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[3], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptd", @"\\GBLNJ4\GIS\Reports\297_1_1345d.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[4], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpte", @"\\GBLNJ4\GIS\Reports\297_1_1345e.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[5], 1, 1, 11800, 100);

                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                    //Clone
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[3].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[4].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[5].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
            }
            else if (Convert.ToInt16(cboSCs.Text) == 332 && strRptName.IndexOf("332_3_x") != -1)// Convert.ToInt16(cboFormats.Text) == 3)
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_332_3.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Efficacy", @"\\GBLNJ4\GIS\Reports\332_3a_Efficacy.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("GrowthPromo", @"\\GBLNJ4\GIS\Reports\332_3_GrowthPromo.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                if (PSSClass.FinalReports.CategoryData332(Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text)) != "N")
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Recovery", @"\\GBLNJ4\GIS\Reports\332_3_Recovery.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 0, 1, 11800, 100);

                //crDoc.Refresh();

                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //Clone
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                if (PSSClass.FinalReports.CategoryData332(Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text)) != "N")
                {
                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
            }
            else if (Convert.ToInt16(cboSCs.Text) == 167 && (Convert.ToInt16(cboFormats.Text) == 5 || Convert.ToInt16(cboFormats.Text) == 6))
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_332_3.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Efficacy", @"\\GBLNJ4\GIS\Reports\332_3a_Efficacy.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("GrowthPromo", @"\\GBLNJ4\GIS\Reports\332_3_GrowthPromo.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //Clone
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
            }
            else if (Convert.ToInt16(cboSCs.Text) == 295 && strRptName.IndexOf("295_1") != -1)
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_295.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Reading1", @"\\GBLNJ4\GIS\Reports\295_11.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                if (Convert.ToInt16(txtSponsorID.Text) != 472)
                {
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Reading2", @"\\GBLNJ4\GIS\Reports\295_12.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Reading3", @"\\GBLNJ4\GIS\Reports\295_13.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 0, 1, 11800, 100);
                }

                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //Clone
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                if (Convert.ToInt16(txtSponsorID.Text) != 472)
                {
                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
            }
            else if (Convert.ToInt16(cboSCs.Text) == 329 && Convert.ToInt16(txtSponsorID.Text) == 1787 && cboFormats.Text == "1" && nSC329 != 329)
            {
                byte n329 = 0;
                for (int i = 0; i < dtRptLogs.Rows.Count; i++)
                {
                    if (dtRptLogs.Rows[i]["ServiceCode"].ToString() == "329")
                        n329 = 1;
                    if (n329 == 1)
                    {
                        nSC329 = Convert.ToInt16(dtRptLogs.Rows[i]["ServiceCode"]);
                    }
                }

                if (nSC329 == 43)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_329_43.rpt";
                else if (nSC329 == 276)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_329_276.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("329_rpt", @"\\GBLNJ4\GIS\Reports\329_1_1787.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                if (nSC329 == 43)
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("329_subrpt", @"\\GBLNJ4\GIS\Reports\43_1_1787.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);
                else if (nSC329 == 276)
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("329_subrpt", @"\\GBLNJ4\GIS\Reports\276_1_1787.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                crDoc.Refresh();

                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //Clone
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
            }
            else
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRptMain.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + strRptName.Replace(".rpt", "") + ".rpt",
                crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);

                foreach (CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj in crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects)
                {
                    // Check to see if it is subreport type
                    if (repObj.Kind == CrystalDecisions.ReportAppServer.ReportDefModel.CrReportObjectKindEnum.crReportObjectKindSubreport)
                    {
                        // clone the report object
                        CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                        // tell the report to update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                    }
                }
            }
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }

            sqlcmd = new SqlCommand("spFinRptMain", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
            sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
            sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
            sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(cboGBLs.Text));

            sqldr = sqlcmd.ExecuteReader();
            DataTable dTable = new DataTable();
            try
            {
                dTable.Load(sqldr);
            }
            catch { }
            crDoc.SetDataSource(dTable);
            crDoc.DataDefinition.FormulaFields["cNewPage"].Text = "'" + Convert.ToInt16(chkNextPage.CheckState).ToString() + "'";
            crDoc.DataDefinition.FormulaFields["cEReport"].Text = "'1'";

            if (bCFR == 1)
            {
                try
                {
                    DataTable dtE = PSSClass.FinalReports.GetRptGBL(Convert.ToInt32(txtRptNo.Text), Convert.ToInt16(txtRevNo.Text));
                    if (dtE != null && dtE.Rows.Count > 1)
                    {
                        string strGBL = "", strSC = "", strSCDesc = "", strSvSC = "";
                        Int32 nSvGBL = 0;
                        for (int i = 0; i < dtE.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dtE.Rows[i]["PSSNo"]) != nSvGBL)
                            {
                                nSvGBL = Convert.ToInt32(dtE.Rows[i]["PSSNo"]);
                                strGBL += dtE.Rows[i]["PSSNo"].ToString() + ", ";
                            }
                            if (strSvSC != dtE.Rows[i]["ServiceCode"].ToString())
                            {
                                strSC += dtE.Rows[i]["ServiceCode"].ToString() + ", ";
                                strSCDesc += dtE.Rows[i]["ServiceDesc"] + ", ";
                                strSvSC = dtE.Rows[i]["ServiceCode"].ToString();
                            }
                        }
                        dtE.Dispose();
                        strGBL = strGBL.Trim();
                        strSC = strSC.Trim();
                        strSCDesc = strSCDesc.Trim();
                        crDoc.DataDefinition.FormulaFields["cGBL"].Text = "'" + strGBL.Substring(0, strGBL.Length - 1) + "'";
                        crDoc.DataDefinition.FormulaFields["cSC"].Text = "'" + strSC.Substring(0, strSC.Length - 1) + "'";
                        crDoc.DataDefinition.FormulaFields["cSCDesc"].Text = "'" + strSCDesc.Substring(0, strSCDesc.Length - 1) + "'";
                    }
                    crDoc.SetParameterValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                    crDoc.SetParameterValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
                }
                catch { }
            }
            else if (cboSCs.Text == "297")
            {
                if (strRptName.IndexOf("297_NVP") != -1)
                {
                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpt");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpt");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpt");
                }
                else
                {
                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpt");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpt");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpt");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpta");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpta");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpta");


                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRptb");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRptb");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRptb");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRptc");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRptc");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRptc");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRptd");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRptd");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRptd");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpte");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpte");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpte");
                }
            }
            else if (Convert.ToInt16(cboSCs.Text) == 332 && strRptName.IndexOf("332_3_x") != -1)// Convert.ToInt16(cboFormats.Text) == 3)
            {
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Efficacy");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Efficacy");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Efficacy");

                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "GrowthPromo");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "GrowthPromo");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "GrowthPromo");

                if (PSSClass.FinalReports.CategoryData332(Convert.ToInt32(cboGBLs.Text), Convert.ToInt32(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text)) != "N")
                {
                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Recovery");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Recovery");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Recovery");
                }
            }
            else if (Convert.ToInt16(cboSCs.Text) == 167 && (Convert.ToInt16(cboFormats.Text) == 5 || Convert.ToInt16(cboFormats.Text) == 6))
            {
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Efficacy");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Efficacy");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Efficacy");

                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "GrowthPromo");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "GrowthPromo");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "GrowthPromo");
            }
            else if (cboSCs.Text == "295" && strRptName.IndexOf("295_1") != -1)//nFormat == 3)
            {
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Reading1");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Reading1");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Reading1");
                if (Convert.ToInt16(txtSponsorID.Text) != 472)
                {
                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Reading2");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Reading2");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Reading2");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Reading3");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Reading3");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Reading3");
                }
            }
            else if (cboSCs.Text == "329" && Convert.ToInt16(txtSponsorID.Text) == 1787 && cboFormats.Text == "1" && nSC329 != 329)
            {
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "329_rpt");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "329_rpt");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "329_rpt");

                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "329_subrpt");
                if (nSC329 == 43)
                    crDoc.SetParameterValue("@SC", 43, "329_subrpt");
                else if (nSC329 == 276)
                    crDoc.SetParameterValue("@SC", 276, "329_subrpt");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "329_subrpt");
            }
            else
            {
                crDoc.SetParameterValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                crDoc.SetParameterValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpt");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpt");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpt");

            }
            crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
            objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\eFinalReports\" + dtpRevDate.Value.Year.ToString() + "\\" + "C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + ".pdf"; 
            crDoc.ExportOptions.DestinationOptions = objDiskOpt;
            crDoc.Export();
            objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\eFinalReports\" + dtpRevDate.Value.Year.ToString() + "\\" + "C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                                      DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
            crDoc.Export();

            string strEMail = "";

            sqlcmd = new SqlCommand("SELECT EMailAddress FROM ContactEMAddresses WHERE ContactID = " + Convert.ToInt16(txtContactID.Text) +
                                    " AND FinalReports = 1", sqlcnn);
            sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                strEMail = sqldr.GetValue(0).ToString();
            }

            //Clean up
            crDoc.Close(); crDoc.Dispose();
            dTable.Dispose(); sqldr.Close(); sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();

            txtTo.Text = strEMail.Replace(";", "; ");

            pnlEMail.Location = new Point(200, 200); pnlEMail.Visible = true; pnlRecord.Enabled = false; pnlEMail.BringToFront();

            strLotNo = PSSClass.Samples.LotNo(Convert.ToInt32(cboGBLs.Text));
            if (strLotNo != "")
                txtSubject.Text = "C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000") + "-R" + txtRevNo.Text + " Lot No: " + strLotNo + " Article Name: " + PSSClass.Samples.ArticleDesc(txtCmpyCode.Text, Convert.ToInt32(cboGBLs.Text));
            else
                txtSubject.Text = "C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000") + "-R" + txtRevNo.Text + " Article Name: " + PSSClass.Samples.ArticleDesc(txtCmpyCode.Text, Convert.ToInt32(cboGBLs.Text));

            txtBody.Text = "Dear " + PSSClass.Contacts.ConFirstName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text)) + ", " + Environment.NewLine + Environment.NewLine +
                       "Thank you for your support of Prince Sterilization Services, LLC. " + Environment.NewLine + "Please find attached our Final Report on the samples of " + PSSClass.Samples.ArticleDesc(txtCmpyCode.Text, Convert.ToInt32(cboGBLs.Text)) + "." +
                       Environment.NewLine + Environment.NewLine;
                       //"<i>Gibraltar Laboratories is pleased to announce the availability of our Report Server. This advanced technology will " + Environment.NewLine + "make <u>your job easier</u>.  " +
                       //"Specifically you will have real-time access to up to 6 months of your e-reports. Perfect for when you" + Environment.NewLine + "cannot locate a report. You will be able to login to the protected " +
                       //"server to retrieve your results without having to send an" + Environment.NewLine + "e-mail or pick up the phone.</i>" + Environment.NewLine + Environment.NewLine +
                       //"If you are interested, Please contact with " + "<a href=" + "mailto:kkohan@princesterilization.com " + ">Kristah Kohan</a> for further details. " + Environment.NewLine + Environment.NewLine +
                       //"To see your most current report, as well as a historical listing of previously issued reports, please click on the " + Environment.NewLine +
                       //"<a href=" + "http://www.princesterilization.com" + ">Gibraltar Laboratories Homepage</a> and select Login." + Environment.NewLine + Environment.NewLine;
            lnkReport.Text = @"\\PSAPP01\IT Files\PTS\PDF Reports\eFinalReports\" + dtpRevDate.Value.Year.ToString() + "\\" + "C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + ".pdf";
            pnlPDF.Visible = true; pnlPDF.Location = new Point(555, 0); pnlPDF.BringToFront(); pnlPDF.Width = 728; pnlPDF.Height = 880;
            btnClosePDF.Location = new Point(653, 0);
            axAcroPDF.Width = 700; axAcroPDF.Height = 830;
            axAcroPDF.src = @"\\PSAPP01\IT Files\PTS\PDF Reports\eFinalReports\" + dtpRevDate.Value.Year.ToString() + "\\" + "C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + ".pdf";
            pnlEMail.BringToFront();
        }

        private void btnCtrlPages_Click(object sender, EventArgs e)
        {
            string strGroupCode = PSSClass.Users.UserGroupCode(LogIn.nUserID);

            if (cboGBLs.Text == "" || cboSCs.Text == "" || nMode != 0) 
            {
                MessageBox.Show("Please complete and save report to proceed." + Environment.NewLine + "Process is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (strFileAccess == "FA" && strGroupCode == "QA")
            { }
            else
            {
                MessageBox.Show("You have no permission to" + Environment.NewLine + "perform this task at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtDateApproved.Text == "")
            {
                dgvCtrlPages.DataSource = null;
                pnlRecord.Enabled = false;
                pnlCtrlPages.Location = new Point(320, 250); pnlCtrlPages.Visible = true; pnlCtrlPages.BringToFront();
                dtCtrlPages = PSSClass.FinalReports.FinRptCtrlPages("P", Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(cboSCs.Text));
                if (dtCtrlPages == null || dtCtrlPages.Rows.Count == 0)
                {
                    dtCtrlPages = PSSClass.FinalReports.FinRptCtrlPages("G", Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(cboSCs.Text));
                    if (dtCtrlPages == null || dtCtrlPages.Rows.Count == 0)
                    {
                        MessageBox.Show("Control pages not found!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                bsCtrlPages.DataSource = dtCtrlPages;
                dgvCtrlPages.DataSource = bsCtrlPages;
                dgvCtrlPages.RowHeadersWidth = 50;
                for (int i = 0; i < dgvCtrlPages.Rows.Count; i++)
                {
                    dgvCtrlPages.Rows[i].HeaderCell.Value = (i + 1).ToString();
                }
                dgvCtrlPages.Columns["ControlPageNo"].HeaderText = "PAGE NO.";
                dgvCtrlPages.Columns["ControlPageNo"].Width = 100;
                dgvCtrlPages.Columns["ControlPageNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvCtrlPages.Columns["PgStatus"].HeaderText = "PAGE STATUS";
                dgvCtrlPages.Columns["PgStatus"].Width = 120;
                dgvCtrlPages.Columns["Returned"].HeaderText = "RETURNED";
                dgvCtrlPages.Columns["Returned"].Width = 100;
                StandardDGVSetting(dgvCtrlPages);
            }
        }

        private void btnCloseCtrlPages_Click(object sender, EventArgs e)
        {
            pnlCtrlPages.Visible = false; pnlRecord.Enabled = true;
        }

        private static int UpdateCtrlPages(string cPgNo, bool cPgStatus)
        {
            byte nSuccess = 0;
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                return 0;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@PgNo", cPgNo);
            sqlcmd.Parameters.AddWithValue("@PgStatus", cPgStatus);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdateCtrlPage";
            try
            {
                sqlcmd.ExecuteNonQuery();
                nSuccess = 1;
            }
            catch
            {
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            return nSuccess;
        }

        private void pnlCtrlPages_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void pnlCtrlPages_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void pnlCtrlPages_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlCtrlPages.Location = PointToClient(this.pnlCtrlPages.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void btnPgReturned_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvCtrlPages.Rows.Count; i++)
            {
                dgvCtrlPages.Rows[i].Cells["Returned"].Value = 1;
            }
        }

        private void dgvCtrlPages_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex != 2)
                e.Cancel = true;
        }

        private void btnAddRev_Click(object sender, EventArgs e)
        {
            if (txtRevNo.Text != "" && PSSClass.Users.UserGroupCode(LogIn.nUserID) != "QA" && PSSClass.Users.UserGroupCode(LogIn.nUserID) != "EXEC")
            {
                MessageBox.Show("You are not authorized to make revisions.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            bnRevisions.Enabled = false;

            DataView dvw;
            DataTable dT = new DataTable();

            if (nMode == 1)
                txtRevNo.Text = "0";
            else if (nMode == 2)
                txtRevNo.Text = PSSClass.FinalReports.RptRevNo(Convert.ToInt64(txtRptNo.Text)).ToString();
            else if (nMode == 0)
            {
                dvw = dtRptLogs.DefaultView;
                dT = dvw.ToTable();
                txtRevNo.Text = PSSClass.FinalReports.RptRevNo(Convert.ToInt64(txtRptNo.Text)).ToString();
                AddEditMode(true);
            }

            cboGBLs.DataSource = null; cboSCs.DataSource = null; cboFormats.DataSource = null;
            cboGBLs.Text = ""; cboSCs.Text = ""; cboFormats.Text = "";
            
            LoadLogsForRpt();           

            DataRow dR;
            dR = dtRptRev.NewRow();

            dR["CompanyCode"] = txtCmpyCode.Text;
            dR["RevisionNo"] = txtRevNo.Text;
            dR["ReportDate"] = DateTime.Now;
            dR["Reason"] = "";
            dR["CreatedBy"] = LogIn.strUserID;
            dR["ESignedBy"] = "";
            dR["DateESigned"] = DBNull.Value;
            dR["EMailedBy"] = "";
            dR["DateEMailed"] = DBNull.Value;
            dR["QAApprovedBy"] = "";
            dR["DateApproved"] = DBNull.Value;
            dR["ESignedByID"] = 0;
            dR["StudyDirID"] = 0;
            dR["TableNextPage"] = false;
            dtRptRev.Rows.Add(dR);

            bsRptRev.DataSource = dtRptRev;
            bnRevisions.BindingSource = bsRptRev;
            bsRptRev.Position = dtRptRev.Rows.Count - 1;

            lblRevNo.Text = "R" + txtRevNo.Text;
            lblLogRevNo.Text = "R" + txtRevNo.Text;
            cboReason.SelectedIndex = 0; txtReason.Text = "";

            if (nMode == 0)
            {
                dtRptLogs.Rows.Clear();
                for (int i = 0; i < dT.Rows.Count; i++)
                {
                    DataRow dr;
                    dr = dtRptLogs.NewRow();
                    dr["Companycode"] = txtCmpyCode.Text;
                    dr["RevisionNo"] = txtRevNo.Text;
                    dr["PSSNo"] = dT.Rows[i]["PSSNo"];
                    dr["ServiceCode"] = dT.Rows[i]["Servicecode"];
                    dr["DataFormat"] = dT.Rows[i]["DataFormat"];
                    dr["ServiceDesc"] = dT.Rows[i]["ServiceDesc"];
                    dr["Description"] = dT.Rows[i]["Description"];
                    dr["DateOn"] = dT.Rows[i]["DateOn"];
                    dr["DateOff"] = dT.Rows[i]["DateOff"];
                    dtRptLogs.Rows.Add(dr);
                }
                bsRptLogs.DataSource = dtRptLogs;
                bnRptLogs.BindingSource = bsRptLogs;
                bsRptLogs.Position = dtRptLogs.Rows.Count - 1;
                cboReason.Enabled = true; txtReason.ReadOnly = false; cboStudyDir.Enabled = true;
                try
                {
                    cboGBLs.Text = dtRptLogs.Rows[bsRptLogs.Position]["PSSNo"].ToString();
                    cboSCs.Text = dtRptLogs.Rows[bsRptLogs.Position]["ServiceCode"].ToString();
                    cboFormats.Text = dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"].ToString();
                }
                catch { }
                btnAddLog.Enabled = false; btnDelLog.Enabled = false; dtpDateOn.Enabled = true; dtpDateOff.Enabled = true;
            }
            else
            {
                dtRptLogs.Rows.Clear();
                
                DataRow dr;
                dr = dtRptLogs.NewRow();
                dr["CompanyCode"] = txtCmpyCode.Text;
                dr["RevisionNo"] = txtRevNo.Text;
                dr["PSSNo"] = "";
                dr["ServiceCode"] = "";
                dr["DataFormat"] = "";
                dr["ServiceDesc"] = "";
                dr["Description"] = "";
                dr["DateOn"] = DateTime.Now;
                dr["DateOff"] = DateTime.Now;
                dtRptLogs.Rows.Add(dr);

                bsRptLogs.DataSource = dtRptLogs;
                bnRptLogs.BindingSource = bsRptLogs;
                bsRptLogs.Position = dtRptLogs.Rows.Count - 1;
                cboReason.Enabled = true; txtReason.ReadOnly = false; cboStudyDir.Enabled = true;
                try
                {
                    cboGBLs.Text = dtRptLogs.Rows[bsRptLogs.Position]["PSSNo"].ToString();
                    cboSCs.Text = dtRptLogs.Rows[bsRptLogs.Position]["ServiceCode"].ToString();
                    cboFormats.Text = dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"].ToString();
                }
                catch { }
                btnAddLog.Enabled = true;
                if (nMode == 1)
                    btnDelLog.Enabled = false;
                else
                    btnDelLog.Enabled = true;
            }
            btnAddRev.Enabled = false; btnDelRev.Enabled = false; btnClose.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bsCtrlPages.EndEdit();
            int nUpdated = 0;
            DataTable dt = new DataTable();
            dt = dtCtrlPages.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    nUpdated += UpdateCtrlPages(dt.Rows[i]["ControlPageNo"].ToString(), Convert.ToBoolean(dt.Rows[i]["Returned"]));
                }
                dt.Rows.Clear();
                MessageBox.Show(nUpdated.ToString() + " pages updated.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pnlCtrlPages.Visible = false; pnlRecord.Enabled = true;
        }

        private void btnPMRC_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dtPMRC = PSSClass.PMRCText.ExPMRCText(Convert.ToInt16(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text));
                if (dtPMRC != null && dtPMRC.Rows.Count != 0)
                {
                    bsPMRC.DataSource = dtPMRC;
                    bnPMRC.BindingSource = bsPMRC;
                    foreach (Control c in pnlPMRC.Controls)
                    {
                        c.DataBindings.Clear();
                    }
                    txtPurposeD.DataBindings.Add("Text", bsPMRC, "Purpose", true);
                    txtMethodD.DataBindings.Add("Text", bsPMRC, "Method", true);
                    txtResultsD.DataBindings.Add("Text", bsPMRC, "Results", true);
                    txtConclusionD.DataBindings.Add("Text", bsPMRC, "Conclusion");
                }
                pnlPMRC.Location = new Point(25, 118); pnlPMRC.Visible = true; pnlRecord.Enabled = false; pnlPMRC.BringToFront();
            }
        }

        private void txtRptNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bsRptLogs.EndEdit();
            for (int i = 0; i < dtRptLogs.Rows.Count; i++)
            {
                MessageBox.Show(dtRptLogs.Rows[i].RowState.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bsRptMstr.EndEdit();
            for (int i = 0; i < dtRptMstr.Rows.Count; i++)
            {
                MessageBox.Show(dtRptMstr.Rows[i].RowState.ToString());
            }
        }

        private void txtRevNo_TextChanged(object sender, EventArgs e)
        {
            lblRevNo.Text = "R" + txtRevNo.Text;
            lblLogRevNo.Text = "R" + txtRevNo.Text;
            if (txtDateApproved.Text != "")
                btnDelLog.Enabled = false;
            LoadFinRevLogs();
        }

        private void cboReason_DropDown(object sender, EventArgs e)
        {
            nReason = cboReason.SelectedIndex;
        }

        private void cboReason_DropDownClosed(object sender, EventArgs e)
        {
            if (txtRevNo.Text == "0" || (Convert.ToInt16(txtRevNo.Text) >= 0 && cboReason.SelectedIndex == 0)) 
                cboReason.SelectedIndex = nReason;
        }

        private void cboReason_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtRevNo.Text == "0" || (Convert.ToInt16(txtRevNo.Text) >= 0 && cboReason.SelectedIndex == 0))
                e.Handled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bsRptRev.EndEdit();
            for (int i = 0; i < dtRptRev.Rows.Count; i++)
            {
                MessageBox.Show(dtRptRev.Rows[i].RowState.ToString());
            }
        }

        private void btnCloseD_Click(object sender, EventArgs e)
        {
            pnlPMRC.Visible = false; pnlRecord.Enabled = true;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            txtPurpose.Text = txtPurposeD.Text;
            txtMethod.Text = txtMethodD.Text;
            txtResults.Text = txtResultsD.Text;
            txtConclusion.Text = txtConclusionD.Text;
            pnlPMRC.Visible = false; pnlRecord.Enabled = true;
        }

        private void pnlPMRC_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void pnlPMRC_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlPMRC.Location = PointToClient(this.pnlPMRC.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlPMRC_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void txtQAApprover_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtDateApproved_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnMSWord_Click(object sender, EventArgs e)
        {
            if (nMWSw == 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you have additional charges?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.Yes)
                {
                    nMWSw = 1;
                    btnOtherFees_Click(null, null);
                    return;
                }
            }
            nOSw = 0; nMWSw = 0;
            
            byte nCP = 0;
            string strSC = "", strLogNo = "";

            for (int i = 0; i < dtRptLogs.Rows.Count; i++)
            {
                strSC = dtRptLogs.Rows[i]["ServiceCode"].ToString();
                strLogNo = dtRptLogs.Rows[i]["PSSNo"].ToString();

                DataTable dt = PSSClass.FinalReports.CtrlPageRet(Convert.ToInt32(strLogNo), Convert.ToInt16(strSC));
                if (dt != null && dt.Rows.Count > 0)
                {
                    DialogResult dReply = new DialogResult();
                    dReply = MessageBox.Show("Control pages are not yet accounted for." + Environment.NewLine + Environment.NewLine + "If this report does not require" + Environment.NewLine + "Control Pages, click OK to proceed.",
                                                Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Hand);
                    if (dReply == DialogResult.Cancel)
                    {
                        MessageBox.Show("Control pages are not yet accounted for." + Environment.NewLine + "Please check your control pages.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        nCP = 1;
                        break;
                    }
                }
            }

            if (nCP == 1)
                return;

            if (nMode != 0) 
            {
                MessageBox.Show("This report is in add or edit mode. " + Environment.NewLine + "Please save report or cancel changes made." + Environment.NewLine + "Cannot generate Word report at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            cboWordFolder.Text = DateTime.Now.Year.ToString();
            string sRptPath = @"\\PSAPP01\Corp\Rpts\" + cboWordFolder.Text + @"\C-"; //1/4/2017
            //if (File.Exists(@"M:\Rpts\2016\C-" + txtRptNo.Text + ".R" + txtRevNo.Text + ".doc") == true || File.Exists(@"M:\Rpts\2016\R" + txtRptNo.Text + ".R" + txtRevNo.Text + ".docx") == true)
            if (File.Exists(sRptPath + txtRptNo.Text + ".R" + txtRevNo.Text + ".doc") == true || File.Exists(sRptPath + txtRptNo.Text + ".R" + txtRevNo.Text + ".docx") == true)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Word report already created." + Environment.NewLine + "Do you want to overwrite it?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
                else
                {
                    DialogResult dRes = new DialogResult();
                    dRes = MessageBox.Show("Are you sure you want to overwrite it?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dRes == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            else
            {
                DialogResult dAns = new DialogResult();
                dAns = MessageBox.Show("Do you want to create a Word report?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dAns == DialogResult.No)
                {
                    return;
                }
                cboWordFolder.Text = DateTime.Now.Year.ToString();
                sRptPath = @"\\PSAPP01\Corp\Rpts\" + cboWordFolder.Text + @"\C-";
            }

            Tables CrTables;
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            crConnectionInfo.Type = ConnectionInfoType.SQL;
            crConnectionInfo.ServerName = "172.16.4.12";
            crConnectionInfo.DatabaseName = "PTS";
            crConnectionInfo.IntegratedSecurity = false;
            crConnectionInfo.UserID = "sa";
            crConnectionInfo.Password = "Pass2018";
            crtableLogoninfo.ConnectionInfo = crConnectionInfo;

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            crDoc = new ReportDocument();

            string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRptWord.rpt";

            crDoc.Load(rpt);
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
            sqlcmd = new SqlCommand("spFinRptMain", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
            sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
            sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
            sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(cboGBLs.Text));

            sqldr = sqlcmd.ExecuteReader();
            DataTable dTable = new DataTable();
            try
            {
                dTable.Load(sqldr);
            }
            catch
            {
            }
            crDoc.SetDataSource(dTable);

            crDoc.ExportOptions.ExportFormatType = ExportFormatType.EditableRTF;
            crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
            objDiskOpt.DiskFileName = sRptPath + txtRptNo.Text + ".R" + txtRevNo.Text + ".doc";
            //objDiskOpt.DiskFileName = @"M:\Rpts\2016\" + "C-" + txtRptNo.Text + ".R" + txtRevNo.Text + ".doc";
            crDoc.ExportOptions.DestinationOptions = objDiskOpt;
            crDoc.Export();

            lnkWordDoc.Text = sRptPath + txtRptNo.Text + ".R" + txtRevNo.Text + ".doc";
            //lnkWordDoc.Text = @"M:\Rpts\2016\C-" + txtRptNo.Text + ".R" + txtRevNo.Text + ".doc";
            lnkWordDoc_LinkClicked(null, null);

            dTable.Dispose(); crDoc.Close(); crDoc.Dispose();
            //GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                MessageBox.Show("This report is in add or edit mode. " + Environment.NewLine + "Please save report or cancel changes made." + Environment.NewLine + "Printing is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string strGroup = PSSClass.Users.UserGroupCode(LogIn.nUserID);
            
            //Disabled 10-10-16 to allow printing of the report for checking and encoding of additional charges
            //=================================================================================================
            //DataTable dtX = new DataTable();
            //dtX = PSSClass.Sponsors.SponsorOnCH(Convert.ToInt16(txtSponsorID.Text));
            //if (dtX != null && dtX.Rows.Count > 0 && strGroup != "QA")
            //{
            //    MessageBox.Show("Sponsor is currently on Credit Hold." + Environment.NewLine + "Process is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    dtX.Dispose();
            //    return;
            //}
            //dtX.Dispose();
            //=================================================================================================

            if (nOSw == 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you have additional charges?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.Yes)
                {
                    btnOtherFees_Click(null, null);
                    return;
                }
            }
            nOSw = 0;
            //Implementation of Final Report Details - AMDC 11/10/2017, Revised 1-23-2018
            if (dtRptLogs.Rows.Count > 1)
            {
                for (int k = 0; k < dtRptLogs.Rows.Count; k++)
                {
                    DataTable dtDtls = PSSClass.FinalReports.GetTestDataValues(Convert.ToInt32(dtRptLogs.Rows[k]["PSSNo"]), Convert.ToInt16(dtRptLogs.Rows[k]["ServiceCode"]));
                    if (dtDtls != null && dtDtls.Rows.Count > 0)
                    {
                        string strPSSNo = "", strSC = ""; byte nDSw = 0;
                        for (int j = 0; j < dtDtls.Rows.Count; j++)
                        {
                            for (int m = 0; m < 10; m++)
                            {
                                if (dtDtls.Rows[j]["TestData" + (m + 1).ToString()] != DBNull.Value)
                                {
                                    strPSSNo = dtDtls.Rows[j]["LogNo"].ToString();
                                    strSC = dtDtls.Rows[j]["SC"].ToString();
                                    nDSw = 1;
                                    break;
                                }
                            }
                            if (nDSw == 1)
                                break;
                        }
                        if (nDSw == 1)
                        {
                            bsRptLogs.Position = k;
                            break;
                        }
                        dtDtls.Dispose();
                    }
                }
            }
            Tables CrTables;
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            crConnectionInfo.Type = ConnectionInfoType.SQL;
            crConnectionInfo.ServerName = "172.16.4.12";
            crConnectionInfo.DatabaseName = "PTS";
            crConnectionInfo.IntegratedSecurity = false;
            crConnectionInfo.UserID = "sa";
            crConnectionInfo.Password = "Pass2018";
            crtableLogoninfo.ConnectionInfo = crConnectionInfo;            


            string strRptName = ""; byte bCFR = 0;
            DataTable dt = new DataTable();
            dt = PSSClass.FinalReports.SpSCRpt(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(cboSCs.Text), Convert.ToInt16(cboFormats.Text));
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No defined report for this Sponsor/Service Code." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dt.Rows[0]["FinalReportID"] != null && dt.Rows[0]["FinalReportID"].ToString().Trim() != "")
            {
                strRptName = dt.Rows[0]["FinalReportID"].ToString().Trim();
                bCFR = 1;
            }
            else
                strRptName = dt.Rows[0]["TableReportID"].ToString().Trim();

            if (File.Exists(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + strRptName.Replace(".rpt", "") + ".rpt") == false)
            {
                MessageBox.Show("Report file under construction." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();

            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            crDoc = new ReportDocument();

            string rpt = "";
            if (bCFR == 1)
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + strRptName + ".rpt";
                crDoc.Load(rpt);
            }
            else if (cboSCs.Text == "297") 
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_297.rpt";
                crDoc.Load(rpt);
                if (strRptName.IndexOf("297_NVP") != -1)
                {
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\GBLNJ4\GIS\Reports\297_NVP1a.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                    //Clone
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
                else
                {
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\GBLNJ4\GIS\Reports\297_1.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpta", @"\\GBLNJ4\GIS\Reports\297_1_1345a.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptb", @"\\GBLNJ4\GIS\Reports\297_1_1345b.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptc", @"\\GBLNJ4\GIS\Reports\297_1_1345c.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[3], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRptd", @"\\GBLNJ4\GIS\Reports\297_1_1345d.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[4], 1, 1, 11800, 100);

                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpte", @"\\GBLNJ4\GIS\Reports\297_1_1345e.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[5], 1, 1, 11800, 100);


                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                    //Clone
                    CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[3].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[4].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[5].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
            }
            else if (Convert.ToInt16(cboSCs.Text) == 332 && strRptName.IndexOf("332_3_x") != -1)// Convert.ToInt16(cboFormats.Text) == 3)
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_332_3.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Efficacy", @"\\GBLNJ4\GIS\Reports\332_3a_Efficacy.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("GrowthPromo", @"\\GBLNJ4\GIS\Reports\332_3_GrowthPromo.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                if (PSSClass.FinalReports.CategoryData332(Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text)) != "N")
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Recovery", @"\\GBLNJ4\GIS\Reports\332_3_Recovery.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 0, 1, 11800, 100);

                //crDoc.Refresh();

                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //Clone
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                if (PSSClass.FinalReports.CategoryData332(Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text)) != "N")
                {
                    repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2].ReportObjects[0];
                    newObj = repObj.Clone(true);
                    //modify the line style  
                    newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                    // update the report object.
                    crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                }
            }
            else if (Convert.ToInt16(cboSCs.Text) ==167 && (Convert.ToInt16(cboFormats.Text) == 5 || Convert.ToInt16(cboFormats.Text) == 6))
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_332_3.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Efficacy", @"\\GBLNJ4\GIS\Reports\332_3a_Efficacy.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("GrowthPromo", @"\\GBLNJ4\GIS\Reports\332_3_GrowthPromo.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                //crDoc.Refresh();

                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //Clone
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
            }
            else if (Convert.ToInt16(cboSCs.Text) == 295 && strRptName.IndexOf("295_1") != -1)
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_295.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Reading1", @"\\GBLNJ4\GIS\Reports\295_11.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Reading2", @"\\GBLNJ4\GIS\Reports\295_12.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("Reading3", @"\\GBLNJ4\GIS\Reports\295_13.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2], 0, 1, 11800, 100);

                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //Clone
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);

                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[2].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
            }
            else if (Convert.ToInt16(cboSCs.Text) == 329 && Convert.ToInt16(txtSponsorID.Text) == 1787 && cboFormats.Text == "1" && nSC329 != 329)
            {
                byte n329 = 0;
                for (int i = 0; i < dtRptLogs.Rows.Count; i++)
                {
                    if (dtRptLogs.Rows[i]["ServiceCode"].ToString() == "329")
                        n329 = 1;
                    if (n329 == 1)
                    {
                        nSC329 = Convert.ToInt16(dtRptLogs.Rows[i]["ServiceCode"]);
                    }
                }

                if (nSC329 == 43)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_329_43.rpt";
                else if (nSC329 == 276)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRpt_329_276.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("329_rpt", @"\\GBLNJ4\GIS\Reports\329_1_1787.rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 0, 1, 11800, 100);

                if (nSC329 == 43)
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("329_subrpt", @"\\GBLNJ4\GIS\Reports\43_1_1787.rpt",
                        crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);
                else if (nSC329 == 276)
                    crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("329_subrpt", @"\\GBLNJ4\GIS\Reports\276_1_1787.rpt",
                            crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1], 0, 1, 11800, 100);

                crDoc.Refresh();

                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects[0];
                //Clone
                CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);


                repObj = crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[1].ReportObjects[0];
                newObj = repObj.Clone(true);
                //modify the line style  
                newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                // update the report object.
                crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
            }            
            else
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "FinalRptMain.rpt";
                crDoc.Load(rpt);

                crDoc.ReportClientDocument.SubreportController.ImportSubreportEx("SubRpt", @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + strRptName.Replace(".rpt", "") + ".rpt",
                    crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0], 1, 1, 11800, 100);

                foreach (CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject repObj in crDoc.ReportClientDocument.ReportDefController.ReportDefinition.ReportFooterArea.Sections[0].ReportObjects)
                {
                    // Check to see if it is subreport type
                    if (repObj.Kind == CrystalDecisions.ReportAppServer.ReportDefModel.CrReportObjectKindEnum.crReportObjectKindSubreport)
                    {
                        // clone the report object
                        CrystalDecisions.ReportAppServer.ReportDefModel.ISCRReportObject newObj = repObj.Clone(true);
                        //modify the line style  
                        newObj.Border.BottomLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.TopLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.RightLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;
                        newObj.Border.LeftLineStyle = CrystalDecisions.ReportAppServer.ReportDefModel.CrLineStyleEnum.crLineStyleNoLine;

                        // tell the report to update the report object.
                        crDoc.ReportClientDocument.ReportDefController.ReportObjectController.Modify(repObj, newObj);
                    }
                }
            }
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
            sqlcmd = new SqlCommand("spFinRptMain", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@CmpyCode", txtCmpyCode.Text);
            sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
            sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
            sqlcmd.Parameters.AddWithValue("@LogNo", Convert.ToInt32(cboGBLs.Text));

            sqldr = sqlcmd.ExecuteReader();
            DataTable dTable = new DataTable();
            try
            {
                dTable.Load(sqldr);
            }
            catch
            {
            }
            crDoc.SetDataSource(dTable);
            crDoc.DataDefinition.FormulaFields["cNewPage"].Text = "'" + Convert.ToInt16(chkNextPage.CheckState).ToString() + "'";
            crDoc.DataDefinition.FormulaFields["cEReport"].Text = "'0'";

            if (bCFR == 1)
            {
                try
                {
                    DataTable dtX = PSSClass.FinalReports.GetRptGBL(Convert.ToInt32(txtRptNo.Text), Convert.ToInt16(txtRevNo.Text));
                    if (dtX != null && dtX.Rows.Count > 1)
                    {
                        string strGBL = "", strSC = "", strSCDesc = "", strSvSC = "";
                        Int32 nSvGBL = 0;
                        for (int i = 0; i < dtX.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dtX.Rows[i]["PSSNo"]) != nSvGBL)
                            {
                                nSvGBL = Convert.ToInt32(dtX.Rows[i]["PSSNo"]);
                                strGBL += dtX.Rows[i]["PSSNo"].ToString() + ", ";
                            }
                            if (strSvSC != dtX.Rows[i]["ServiceCode"].ToString())
                            {
                                strSC += dtX.Rows[i]["ServiceCode"].ToString() + ", ";
                                strSCDesc += dtX.Rows[i]["ServiceDesc"] + ", ";
                                strSvSC = dtX.Rows[i]["ServiceCode"].ToString();
                            }
                        }
                        dtX.Dispose();
                        strGBL = strGBL.Trim();
                        strSC = strSC.Trim();
                        strSCDesc = strSCDesc.Trim();
                        crDoc.DataDefinition.FormulaFields["cGBL"].Text = "'" + strGBL.Substring(0, strGBL.Length - 1) + "'";
                        crDoc.DataDefinition.FormulaFields["cSC"].Text = "'" + strSC.Substring(0, strSC.Length - 1) + "'";
                        crDoc.DataDefinition.FormulaFields["cSCDesc"].Text = "'" + strSCDesc.Substring(0, strSCDesc.Length - 1) + "'";
                    }
                    crDoc.SetParameterValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                    crDoc.SetParameterValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
                }
                catch { }
            }
            else if (cboSCs.Text == "297")
            {
                if (strRptName.IndexOf("297_NVP") != -1)
                {
                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpt");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpt");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpt");
                }
                else
                {
                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpt");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpt");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpt");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpta");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpta");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpta");


                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRptb");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRptb");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRptb");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRptc");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRptc");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRptc");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRptd");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRptd");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRptd");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpte");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpte");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpte");
                }
            }
            else if (cboSCs.Text == "332" && strRptName.IndexOf("332_3_x") != -1)// Convert.ToInt16(cboFormats.Text) == 3)
            {
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Efficacy");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Efficacy");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Efficacy");

                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "GrowthPromo");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "GrowthPromo");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "GrowthPromo");

                if (PSSClass.FinalReports.CategoryData332(Convert.ToInt32(cboGBLs.Text), Convert.ToInt32(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text)) != "N")
                {
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Efficacy");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Efficacy");

                    crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Recovery");
                    crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Recovery");
                    crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Recovery");
                }
            }
            else if (cboSCs.Text == "167" && (Convert.ToInt16(cboFormats.Text) == 5 || Convert.ToInt16(cboFormats.Text) == 6))
            {
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Efficacy");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Efficacy");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Efficacy");

                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "GrowthPromo");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "GrowthPromo");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "GrowthPromo");
            }
            else if (cboSCs.Text == "295" && strRptName.IndexOf("295_1") != -1)//nFormat == 3)
            {
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Reading1");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Reading1");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Reading1");

                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Reading2");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Reading2");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Reading2");

                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "Reading3");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "Reading3");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "Reading3");
            }
            else if (cboSCs.Text == "329" && Convert.ToInt16(txtSponsorID.Text) == 1787 && cboFormats.Text == "1" && nSC329 != 329)
            {
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "329_rpt");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "329_rpt");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "329_rpt");

                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "329_subrpt");
                if (nSC329 == 43)
                    crDoc.SetParameterValue("@SC", 43, "329_subrpt");
                else if (nSC329 == 276)
                    crDoc.SetParameterValue("@SC", 276, "329_subrpt");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "329_subrpt");
            }
            else
            {
                crDoc.SetParameterValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                crDoc.SetParameterValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
                crDoc.SetParameterValue("@LogNo", Convert.ToInt32(cboGBLs.Text), "SubRpt");
                crDoc.SetParameterValue("@SC", Convert.ToInt32(cboSCs.Text), "SubRpt");
                crDoc.SetParameterValue("@SpID", Convert.ToInt16(txtSponsorID.Text), "SubRpt");
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            //Open the PrintDialog
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.AllowSelection = true;
            this.printDialog1.AllowSomePages = true;
            this.printDialog1.AllowCurrentPage = true;
            DialogResult dr = this.printDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //Get the Copy times
                int nCopy = this.printDocument1.PrinterSettings.Copies;
                //Get the number of Start Page
                int sPage = this.printDocument1.PrinterSettings.FromPage;
                //Get the number of End Page
                int ePage = this.printDocument1.PrinterSettings.ToPage;
                //Get the printer name
                string PrinterName = this.printDocument1.PrinterSettings.PrinterName;

                System.Drawing.Printing.PrinterSettings printerSettings = new System.Drawing.Printing.PrinterSettings();

                try
                {
                    printerSettings.PrinterName = PrinterName;
                    crDoc.PrintToPrinter(printerSettings, new PageSettings(), false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
            objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\eFinalReports\" + dtpRevDate.Value.Year.ToString() + "\\" + "C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + ".pdf"; 
            crDoc.ExportOptions.DestinationOptions = objDiskOpt;
            crDoc.Export();
            objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\eFinalReports\" + dtpRevDate.Value.Year.ToString() + "\\" + "C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                                      DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
            crDoc.Export();
            nOSw = 0;
            dTable.Dispose(); crDoc.Close(); crDoc.Dispose();
            //GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect(); GC.WaitForPendingFinalizers();
        }

        private void btnDataForm_Click(object sender, EventArgs e)
        {
            if (PSSClass.General.UserFileAccess(LogIn.nUserID, "TestDataValues") == "")
            {
                MessageBox.Show("You have no permission to" + Environment.NewLine + "perform this task at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (nMode != 0 ) //if (txtRptNo.Text == "(New)")
            {
                MessageBox.Show("This report is in add or edit mode. " + Environment.NewLine + "Please save report or cancel changes made." + Environment.NewLine + "Process is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboSCs.Text == "2122" || cboSCs.Text == "2123" || cboSCs.Text == "295")
            {
                TestDataValuesEM childForm = new TestDataValuesEM();
                childForm.Text = "TEST DATA VALUES - ENVIRONMENTAL MONITORING";
                childForm.pubCmpy = txtCmpyCode.Text;
                childForm.nLogNo = Convert.ToInt64(cboGBLs.Text);
                if (txtDateApproved.Text != "")
                    childForm.nRptNo = Convert.ToInt32(txtRptNo.Text);
                else
                    childForm.nRptNo = 0;
                childForm.nServiceCode = Convert.ToInt16(cboSCs.Text);
                childForm.nSponsorID = Convert.ToInt16(txtSponsorID.Text);
                childForm.ShowDialog();
                this.Activate();
            }
            else
            {
                TestDataValues childForm = new TestDataValues();
                childForm.Text = "TEST DATA VALUES";
                childForm.pubCmpy = txtCmpyCode.Text;
                childForm.nLogNo = Convert.ToInt64(cboGBLs.Text);
                if (txtDateApproved.Text != "")
                    childForm.nRptNo = Convert.ToInt32(txtRptNo.Text);
                else
                    childForm.nRptNo = 0;
                childForm.nServiceCode = Convert.ToInt16(cboSCs.Text);
                childForm.nSponsorID = Convert.ToInt16(txtSponsorID.Text);
                childForm.ShowDialog();
                this.Activate();
            }
        }

        private void btnCloseEMail_Click(object sender, EventArgs e)
        {
            pnlPDF.Visible = false;
            if (nLSw == 3)
            {
                nLSw = 0;
                SendKeys.Send("{F12}");
                return;
            }
            pnlRecord.Enabled = true; pnlEMail.Visible = false; tlsFile.Enabled = true;               
        }

        private void pnlEMail_MouseMove(object sender, MouseEventArgs e)
        {

            if (mouseDown)
            {
                pnlEMail.Location = PointToClient(this.pnlEMail.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlEMail_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void pnlEMail_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void lnkReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pnlPDF.Visible = true; pnlPDF.Location = new Point(555, 0); pnlPDF.BringToFront(); pnlPDF.Width = 728; pnlPDF.Height = 880;
            btnClosePDF.Location = new Point(653, 0); 
            axAcroPDF.Width = 700; axAcroPDF.Height = 830;
            axAcroPDF.src = @"\\PSAPP01\IT Files\PTS\PDF Reports\eFinalReports\" + dtpRevDate.Value.Year.ToString() + "\\C-" + Convert.ToInt32(txtRptNo.Text).ToString("000000")+ ".R" + txtRevNo.Text + ".pdf";
            pnlEMail.BringToFront(); 
        }

        private void btnSendEMail_Click(object sender, EventArgs e)
        {
            pnlPDF.Visible = false; 

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
            ((Outlook._MailItem)oMsg).Send();

            // Clean up.
            //oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;

            if (txtDateEMailed.Text == "")
            {
                //UPDATE EMAIL DATE
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                sqlcmd = new SqlCommand("UPDATE FinalRptRev SET DateEMailed = GetDate(), EMailedByID=" + LogIn.nUserID + " " +
                                        "WHERE ReportNo=" + Convert.ToInt32(txtRptNo.Text) + " AND RevisionNo=" + Convert.ToInt32(txtRevNo.Text), sqlcnn);
                sqlcmd.ExecuteNonQuery();
                sqlcmd.Dispose();
                sqlcnn.Close(); sqlcnn.Dispose();
            }

            ////Check if E-Signature Form is opened, then Close Form
            //byte nX = 0;
            //foreach (Form form in Application.OpenForms)
            //{
            //    if (form.GetType().ToString() == "GIS.FinalRptESign")
            //    {
            //        nX = 1;
            //        break;
            //    }
            //}
            if (nLSw == 3 || nLSw == 2) //|| nX == 1
            {
                SendKeys.Send("{F12}");
                return;
            }
            tlsFile.Enabled = true;
            AddEditMode(false);
            LoadData();
            nMode = 0;
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

        private void FinalReports_KeyDown(object sender, KeyEventArgs e)
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
                    if (nMode == 0 && strFileAccess != "RO" && (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() == "" &&
                    dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateESigned"].Value.ToString() == "" &&
                    dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() == ""))
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
                    this.Close(); this.Dispose();
                    break;

                default:
                    break;
            }
        }

        private void chkWordRpt_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWordRpt.Checked == true)
            {
                string strDateMailed = txtDateEMailed.Text.ToString();
                picWordRpt.Visible = true;
                txtDateEMailed.Text = ""; txtEMailedBy.Text = ""; 
            }
            else
            {
                picWordRpt.Visible = false; 
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            int intOpen = PSSClass.General.OpenForm(typeof(SamplesLogin));

            if (intOpen == 1)
            {
                PSSClass.General.CloseForm(typeof(SamplesLogin));
            }
            SamplesLogin childForm = new SamplesLogin();
            childForm.MdiParent = this.MdiParent;
            childForm.Text = "SAMPLES LOGIN";
            childForm.nLogNo = Convert.ToInt32(cboGBLs.Text);
            if (Convert.ToInt32(cboGBLs.Text) < 412377) //temporary fix
                childForm.pubCmpyCode = "P";
            else
                childForm.pubCmpyCode = "G";
            //childForm.pubCmpyCode = txtLCmpyCode.Text;
            childForm.nFR = 1;
            childForm.strCriteria = "PSS No.";
            childForm.strData = cboGBLs.Text;
            childForm.nSearch = 13;
            childForm.Show();
        }

        private void btnCloseFees_Click(object sender, EventArgs e)
        {
            pnlOtherFees.Visible = false; pnlRecord.Enabled = true;
            if (nMWSw == 0)
                btnPrint_Click(null, null);
            else
                btnMSWord_Click(null, null);
        }

        private void btnOtherFees_Click(object sender, EventArgs e)
        {
            if (PSSClass.Users.IsReviewer(LogIn.nUserID) == false)
            {
                MessageBox.Show("You are not authorized to" + Environment.NewLine + "enter other charges at this time.",Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (txtRptNo.Text != "")
            {
                DataTable dtInv = PSSClass.FinalBilling.FinRptInvNo(Convert.ToInt32(txtRptNo.Text));
                if (dtInv.Rows.Count > 0)
                {
                    if (dtInv.Rows[0]["DateMailed"].ToString() != "")
                    {
                        dgvOtherFees.ReadOnly = true; lblInvNo.Text = "Invoice No. " + dtInv.Rows[0]["InvoiceNo"].ToString(); lblInvNo.Visible = true;
                    }
                    else
                    {
                        dgvOtherFees.ReadOnly = false; lblInvNo.Visible = false;
                    }
                }
                dtInv.Dispose();

                dtOtherFees.Rows.Clear();
                txtSw.Text = "1";

                LoadFinRptFees();
                pnlRecord.Enabled = false; pnlOtherFees.Visible = true; pnlOtherFees.BringToFront(); pnlOtherFees.Location = new Point(12, 198);
                if (dtOtherFees.Rows.Count == 0)
                {
                    DataTable dt = new DataTable();
                    dt = PSSClass.FinalReports.ExTestOtheCosts(Convert.ToInt32(txtRptNo.Text));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dR = dtOtherFees.NewRow();

                            dR["ReportNo"] = txtRptNo.Text;
                            dR["ServiceCode"] = dt.Rows[i]["ServiceCode"];
                            dR["ServiceDesc"] = dt.Rows[i]["ServiceDesc"];
                            dR["TestDesc1"] = dt.Rows[i]["TestDesc1"];
                            dR["BillQty"] = 0;
                            dR["UnitPrice"] = dt.Rows[i]["UnitPrice"];
                            dR["Amount"] = 0;
                            dR["QuotationNo"] = dt.Rows[i]["QuotationNo"];
                            dR["RevisionNo"] = dt.Rows[i]["RevisionNo"];
                            dR["ControlNo"] = dt.Rows[i]["ControlNo"];
                            dR["QCmpyCode"] = dt.Rows[i]["QCmpyCode"];
                            dR["LCmpyCode"] = dt.Rows[i]["LCmpyCode"];
                            dR["RCmpyCode"] = dt.Rows[i]["RCmpyCode"];
                            dR["DateCreated"] = DateTime.Now;
                            dR["CreatedByID"] = LogIn.nUserID;
                            dR["LastUpdate"] = DateTime.Now;
                            dR["LastUserID"] = LogIn.nUserID;
                            dtOtherFees.Rows.Add(dR);
                        }
                    }
                    txtSw.Text = "0";
                }
                bsOtherFees.DataSource = dtOtherFees;
                dgvOtherFees.DataSource = bsOtherFees;
                dgvOtherFees.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
                dgvOtherFees.Columns["ServiceCode"].Width = 80;
                dgvOtherFees.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvOtherFees.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
                dgvOtherFees.Columns["ServiceDesc"].Width = 300;
                dgvOtherFees.Columns["TestDesc1"].HeaderText = "TEST DESCRIPTION";
                dgvOtherFees.Columns["TestDesc1"].Width = 300;
                dgvOtherFees.Columns["BillQty"].HeaderText = "BILL QTY";
                dgvOtherFees.Columns["BillQty"].DefaultCellStyle.Format = "#,##0.00";
                dgvOtherFees.Columns["BillQty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvOtherFees.Columns["BillQty"].Width = 75;
                dgvOtherFees.Columns["UnitPrice"].HeaderText = "UNIT PRICE";
                dgvOtherFees.Columns["UnitPrice"].DefaultCellStyle.Format = "$#,##0.00";
                dgvOtherFees.Columns["UnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvOtherFees.Columns["UnitPrice"].Width = 90;
                dgvOtherFees.Columns["Amount"].HeaderText = "AMOUNT";
                dgvOtherFees.Columns["Amount"].DefaultCellStyle.Format = "$#,##0.00";
                dgvOtherFees.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvOtherFees.Columns["Amount"].Width = 90;
                dgvOtherFees.Columns["ReportNo"].Visible = false;
                dgvOtherFees.Columns["QuotationNo"].Visible = false;
                dgvOtherFees.Columns["RevisionNo"].Visible = false;
                dgvOtherFees.Columns["ControlNo"].Visible = false;
                dgvOtherFees.Columns["QCmpyCode"].Visible = false;
                dgvOtherFees.Columns["LCmpyCode"].Visible = false;
                dgvOtherFees.Columns["RCmpyCode"].Visible = false;
                dgvOtherFees.Columns["DateCreated"].Visible = false;
                dgvOtherFees.Columns["CreatedByID"].Visible = false;
                dgvOtherFees.Columns["LastUpdate"].Visible = false;
                dgvOtherFees.Columns["LastUserID"].Visible = false;
            }
            nOSw = 1;
        }

        private void dgvOtherFees_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvOtherFees.CurrentCell.OwningColumn.Name != "BillQty")
                e.Cancel = true;
        }

        private void dgvOtherFees_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dgvOtherFees.Rows[dgvOtherFees.CurrentCell.RowIndex].Cells["Amount"].Value = Convert.ToDecimal(dgvOtherFees.Rows[dgvOtherFees.CurrentCell.RowIndex].Cells["BillQty"].Value) *
                                                                                                    Convert.ToDecimal(dgvOtherFees.Rows[dgvOtherFees.CurrentCell.RowIndex].Cells["UnitPrice"].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveFees_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            if (txtSw.Text == "0")
            {
                dtOtherFees.AcceptChanges();
                foreach (DataRow row in dtOtherFees.Rows)
                {
                    if (Convert.ToInt16(row["BillQty"]) != 0)
                        row.SetAdded();
                }
            }
            else
            {
                bsOtherFees.EndEdit();
            }
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlDataAdapter da;

            da = new SqlDataAdapter("SELECT * FROM FinalRptFees", sqlcnn);
            SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(da);

            dt = dtOtherFees.GetChanges(DataRowState.Added);
            if (dt != null)
            {
                try
                {
                    cmdBuilder.GetInsertCommand();
                    da.Update(dt);
                }
                catch { }
            }

            dt = dtOtherFees.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                try
                {
                    cmdBuilder.GetUpdateCommand();
                    da.Update(dt);
                }
                catch { }
            }
            dt = dtOtherFees.GetChanges(DataRowState.Deleted);
            if (dt != null)
            {
                try
                {
                    cmdBuilder.GetDeleteCommand();
                    da.Update(dt);
                }
                catch { }
            }
            cmdBuilder.Dispose();
            try
            {
                dt.Dispose(); da.Dispose();
            }
            catch { }


            sqlcnn.Close(); sqlcnn.Dispose();
            pnlOtherFees.Visible = false; pnlRecord.Enabled = true;
            if (nMWSw == 0)
            {
                LoadFinRptRev();
                btnPrint_Click(null, null);
            }
            else
            {
                btnMSWord_Click(null, null);
            }
        }

        private void picWordRpt_Click(object sender, EventArgs e)
        {
            lnkWordDoc_LinkClicked(null, null);
        }

        private void FinalReports_Activated(object sender, EventArgs e)
        {
            try
            {
                if (nBilling == 1)
                {
                    string strRptNo = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["ReportNo"].Value.ToString();
                    LoadRecords();
                    bsFile.Filter = "ReportNo<>0";
                    PSSClass.General.FindRecord("ReportNo", strRptNo, bsFile, dgvFile);
                    nBilling = 0;
                }
            }
            catch { }
            try
            {
                if (dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateApproved"].Value.ToString() != "" ||
                    dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateESigned"].Value.ToString() != "" ||
                    dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["DateEMailed"].Value.ToString() != "")
                {
                    tsbEdit.Enabled = false;
                }
            }
            catch { }
            this.WindowState = FormWindowState.Maximized; this.Refresh();
        }

        private void lnkWordDoc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //string sRptPath = @"M:\Rpts\" + DateTime.Now.Year.ToString() + @"\C-"; //1/4/2017

            string sRptPath = @"M:\Rpts\" + cboWordFolder.Text + @"\C-"; // 01/04/2017
            
            string strDoc = sRptPath + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + ".doc";
            string strDocX = sRptPath + Convert.ToInt32(txtRptNo.Text).ToString("000000") + ".R" + txtRevNo.Text + ".docx";
            if (System.IO.File.Exists(strDoc))
            {
                lnkWordDoc.Text = strDoc;
            }
            else if (System.IO.File.Exists(strDocX))
            {
                lnkWordDoc.Text = strDocX;
            }
            try
            {
                System.Diagnostics.Process.Start(lnkWordDoc.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            if (nMode != 0) 
            {
                MessageBox.Show("This report is in add or edit mode. " +  Environment.NewLine + "Please save report or cancel changes made." + Environment.NewLine + "Print preview is suspended at this time.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string strRptName = ""; byte bCFR = 0;

            if (cboFormats.Text == "")
                cboFormats.Text = "1";

            //Implementation of Final Report Details - AMDC 11/10/2017, Revised 1-23-2018
            if (dtRptLogs.Rows.Count > 1)
            {
                for (int k = 0; k < dtRptLogs.Rows.Count; k++)
                {
                    DataTable dtDtls = PSSClass.FinalReports.GetTestDataValues(Convert.ToInt32(dtRptLogs.Rows[k]["PSSNo"]), Convert.ToInt16(dtRptLogs.Rows[k]["ServiceCode"]));
                    if (dtDtls != null && dtDtls.Rows.Count > 0)
                    {
                        string strPSSNo = "", strSC = ""; byte nDSw = 0;
                        for (int j = 0; j < dtDtls.Rows.Count; j++)
                        {
                            for (int m = 0; m < 10; m++)
                            {
                                if (dtDtls.Rows[j]["TestData" + (m + 1).ToString()] != DBNull.Value)
                                {
                                    strPSSNo = dtDtls.Rows[j]["LogNo"].ToString();
                                    strSC = dtDtls.Rows[j]["SC"].ToString();
                                    nDSw = 1;
                                    break;
                                }
                            }
                            if (nDSw == 1)
                                break;
                        }
                        if (nDSw == 1)
                        {
                            bsRptLogs.Position = k;
                            break;
                        }
                        dtDtls.Dispose();
                    }
                }
            }
            DataTable dt = new DataTable();
            dt = PSSClass.FinalReports.SpSCRpt(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(cboSCs.Text), Convert.ToInt16(cboFormats.Text));
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No defined report for this Sponsor/Service Code." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dt.Rows[0]["FinalReportID"] != null && dt.Rows[0]["FinalReportID"].ToString().Trim() != "")
            {
                strRptName = dt.Rows[0]["FinalReportID"].ToString().Trim();
                bCFR = 1;
            }
            else
                strRptName = dt.Rows[0]["TableReportID"].ToString().Trim();

            if (File.Exists(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + strRptName.Replace(".rpt", "") + ".rpt") == false)
            {
                MessageBox.Show("Report file under construction." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            byte n329 = 0; Int16 nSC329 = 0;
            for (int i = 0; i < dtRptLogs.Rows.Count; i++)
            {
                if (dtRptLogs.Rows[i]["ServiceCode"].ToString() == "329")
                    n329 = 1;
                if (n329 == 1)
                {
                    nSC329 = Convert.ToInt16(dtRptLogs.Rows[i]["ServiceCode"]);
                }
            }

            LabRpt rpt = new LabRpt();
            rpt.Owner = this;
            rpt.rptName = "FinalReport";
            rpt.rptFile = strRptName.Replace(".rpt", "") + ".rpt";
            rpt.nNxtPg = Convert.ToInt16(chkNextPage.CheckState);
            //rpt.WindowState = FormWindowState.Maximized;
            rpt.CmpyCode = txtCmpyCode.Text;
            rpt.nRptNo = Convert.ToInt32(txtRptNo.Text);
            rpt.nRevNo = Convert.ToInt16(txtRevNo.Text);
            rpt.nLogNo = Convert.ToInt32(cboGBLs.Text);
            rpt.nSC = Convert.ToInt32(cboSCs.Text);
            rpt.SpID = Convert.ToInt16(txtSponsorID.Text);
            rpt.nFormat = Convert.ToInt16(cboFormats.Text);
            rpt.nF329 = nSC329;
            rpt.nExType = 0;
            rpt.bCFR = bCFR;
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                try
                {
                    dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = cboFormats.Text;
                }
                catch { }
            }
        }

        private void cboGBLs_TextUpdate(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                if (cboGBLs.Text != "")
                {
                    try
                    {
                        dtRptLogs.Rows[bsRptLogs.Position]["PSSNo"] = cboGBLs.Text;
                        dtRptLogs.Rows[bsRptLogs.Position]["Description"] = PSSClass.Samples.SampleDesc(Convert.ToInt32(cboGBLs.Text));
                    }
                    catch { }
                }
            }
        }

        private void cboSCs_TextUpdate(object sender, EventArgs e)
        {
            if (cboSCs.Text.Trim() != "")
            {
                try
                {
                    //Update Table
                    dtRptLogs.Rows[bsRptLogs.Position]["ServiceCode"] = cboSCs.Text;
                    dtRptLogs.Rows[bsRptLogs.Position]["ServiceDesc"] = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(cboSCs.Text), dtSC);
                }
                catch { }
            }
        }

        private void cboGBLs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 0)
                e.Handled = true;
            else if ((e.KeyChar != 13 && e.KeyChar != 8 && e.KeyChar < 48) || e.KeyChar > 57)
                e.Handled = true;
            else if (e.KeyChar == 13)
            {
                try
                {
                    dtRptLogs.Rows[bsRptLogs.Position]["PSSNo"] = cboGBLs.Text;
                    dtRptLogs.Rows[bsRptLogs.Position]["Description"] = PSSClass.Samples.SampleDesc(Convert.ToInt32(cboGBLs.Text));

                    dtSCDDL = PSSClass.FinalReports.LogSCForRpt(Convert.ToInt32(cboGBLs.Text));
                    if (dtSCDDL == null || dtSCDDL.Rows.Count == 0)
                    {
                        MessageBox.Show("No outstanding tests found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        cboGBLs.Text = ""; cboSCs.Text = ""; cboFormats.Text = ""; txtSCDesc.Text = "";
                        return;
                    }
                    cboSCs.DataSource = dtSCDDL;
                    cboSCs.DisplayMember = "ServiceCode";
                    cboSCs.ValueMember = "ServiceCode";
                    cboSCs.SelectedIndex = -1;
                    cboSCs.SelectedIndex = 0;
                }
                catch { }
                SendKeys.Send("{TAB}");
            }
            else
            {
                cboSCs.DataSource = null; cboFormats.DataSource = null;
                txtDescription.Text = ""; cboSCs.Text = ""; txtSCDesc.Text = ""; cboFormats.Text = "";
            }
        }

        private void cboGBLs_Leave(object sender, EventArgs e)
        {
            if (nMode != 0 && cboGBLs.Text.Trim() != "")
            {
                dtGBLDDL.PrimaryKey = new DataColumn[] { dtGBLDDL.Columns["LogNo"] };
                object[] fkeys = new object[1];
                fkeys[0] = cboGBLs.Text;
                DataRow foundRow = dtGBLDDL.Rows.Find(fkeys);
                if (foundRow == null  && dtRptLogs != null && bsRptLogs.Position != -1)
                {
                    MessageBox.Show("Invalid GBL entry.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboGBLs.Text = ""; cboSCs.Text = ""; cboFormats.Text = "";
                    cboGBLs.Focus();
                    bsRptLogs.Position = dtRptLogs.Rows.Count;
                    return;
                }
                else if (foundRow == null && bsRptLogs.Position != -1 && dtRptLogs.Rows[bsRptLogs.Position].RowState.ToString() == "Added")
                {
                    MessageBox.Show("Invalid GBL entry.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboGBLs.Text = ""; cboSCs.Text = ""; cboFormats.Text = "";
                    cboGBLs.Focus();
                    bsRptLogs.Position = dtRptLogs.Rows.Count;
                    return;
                }
                SetDDL();
            }
        }

        private void SetDDL()
        {
            try
            {
                dtRptLogs.Rows[bsRptLogs.Position]["PSSNo"] = cboGBLs.Text;
                dtRptLogs.Rows[bsRptLogs.Position]["Description"] = PSSClass.Samples.SampleDesc(Convert.ToInt32(cboGBLs.Text));
            }
            catch { }
            try
            {
                txtDescription.Text = PSSClass.Samples.SampleDesc(Convert.ToInt32(cboGBLs.Text));
                dtSCDDL = PSSClass.FinalReports.LogSCForRpt(Convert.ToInt32(cboGBLs.Text));
                if ((dtSCDDL == null || dtSCDDL.Rows.Count == 0) && dtRptLogs.Rows[bsRptLogs.Position].RowState.ToString() == "Added")
                {
                    MessageBox.Show("No outstanding tests found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    cboGBLs.Text = ""; cboSCs.Text = ""; cboFormats.Text = "";
                    return;
                }
                cboSCs.DataSource = dtSCDDL;
                cboSCs.DisplayMember = "ServiceCode";
                cboSCs.ValueMember = "ServiceCode";
                cboSCs.SelectedIndex = 0;
                txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(dtSCDDL.Rows[0]["ServiceCode"]), dtSC);
                dtRptLogs.Rows[bsRptLogs.Position]["ServiceCode"] = dtSCDDL.Rows[0]["ServiceCode"];

                string strFormatNo = PSSClass.FinalReports.ExFormatNo(Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(dtSCDDL.Rows[0]["ServiceCode"])).ToString();
                if (strFormatNo != "0" && strFormatNo != "")
                {
                    cboFormats.Text = strFormatNo;
                    dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = strFormatNo;
                }
                else
                {
                    cboFormats.Text = "1";
                    dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = "1";
                }
            }
            catch { }
        }

        private void cboSCs_Leave(object sender, EventArgs e)
        {
            if (nMode != 0 && cboSCs.Text.Trim() != "")
            {
                dtSCDDL.PrimaryKey = new DataColumn[] { dtSCDDL.Columns["ServiceCode"] };
                object[] fkeys = new object[1];
                fkeys[0] = cboSCs.Text;
                DataRow foundRow = dtSCDDL.Rows.Find(fkeys);
                if (foundRow == null)
                {
                    MessageBox.Show("Invalid service code entry.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboSCs.Focus();
                    return;
                }

                cboFormats.DataSource = null;
                //Setup Table Formats
                DataTable dtFormats = new DataTable();
                dtFormats = PSSClass.FinalReports.ExTableFormats(Convert.ToInt16(cboSCs.Text), Convert.ToInt16(txtSponsorID.Text));
                cboFormats.DataSource = dtFormats;
                cboFormats.DisplayMember = "FormatNo";
                cboFormats.ValueMember = "FormatNo";
                try
                {
                    dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = dtFormats.Rows[0]["FormatNo"];
                }
                catch { }

                string strFormatNo = PSSClass.FinalReports.ExFormatNo(Convert.ToInt32(cboGBLs.Text), Convert.ToInt16(cboSCs.Text)).ToString();
                if (strFormatNo != "")
                {
                    cboFormats.Text = strFormatNo;
                    try
                    {
                        dtRptLogs.Rows[bsRptLogs.Position]["DataFormat"] = strFormatNo;
                    }
                    catch { }
                }
            }
        }

        private void chkLocked_Click(object sender, EventArgs e)
        {
            bool bCheck = chkLocked.Checked;
            if (txtRevNo.Text != "" && PSSClass.Users.UserGroupCode(LogIn.nUserID) != "QA" && PSSClass.Users.UserGroupCode(LogIn.nUserID) != "EXEC")
            {
                MessageBox.Show("You are not authorized to lock/unlock a report.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                chkLocked.Checked = !bCheck;
                return;
            }

            if (chkLocked.Checked == false)
            {
                if (txtDateEMailed.Text != "" || lblDateScanned.Text != "") //txtDateESigned.Text != "" || 
                {
                    MessageBox.Show("You are about to unlock a report that has been submitted." + Environment.NewLine +
                                    "Please contact the IT Department to perform this task.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    chkLocked.Checked = true;
                    return;
                }
                DialogResult dReply = new DialogResult();
                if (chkLocked.Checked == false)
                    dReply = MessageBox.Show("Do you want to unlock this report?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                else
                    dReply = MessageBox.Show("Do you want to lock this report?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    chkLocked.Checked = !bCheck;
                    return;
                }
                using (ESignature eSignature = new ESignature())
                {
                    eSignature.Location = new Point(245, 250);
                    eSignature.eRptNo = Convert.ToInt32(txtRptNo.Text);
                    eSignature.eRevNo = Convert.ToInt32(txtRevNo.Text);
                    eSignature.eSign = 7;
                    if (eSignature.ShowDialog() == DialogResult.OK)
                    {
                        LoadData();
                        AddEditMode(false);
                        nMode = 0;
                    }
                    else
                        chkLocked.Checked = true;
                }
            }
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront();
            }
        }

        private void picContacts_Click(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                dgvContacts.Visible = true; dgvContacts.BringToFront();
            }
        }

        private void pnlPDF_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlPDF.Location = PointToClient(this.pnlPDF.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlPDF_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void pnlPDF_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void lblPDF_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void lblPDF_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlPDF.Location = PointToClient(this.pnlPDF.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void lblPDF_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void btnClosePDF_Click(object sender, EventArgs e)
        {
            pnlPDF.Visible = false;
        }

        private void FileAccess()
        {
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsddbPrint.Enabled = false; pnlRevisions.Enabled = false; pnlRevLogs.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true; pnlRevisions.Enabled = true; pnlRevLogs.Enabled = true;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true; pnlRevisions.Enabled = true; pnlRevLogs.Enabled = true;
            }
            else
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; pnlRevisions.Enabled = false; pnlRevLogs.Enabled = false;
            }
            tsddbSearch.Enabled = true;
        }

        private void FinalReports_FormClosing(object sender, FormClosingEventArgs e)
        {
            //dtSponsors.Dispose(); dtContacts.Dispose(); dtGBLDDL.Dispose(); dtSCDDL.Dispose(); dtSC.Dispose();
            //dtRptMstr.Dispose(); dtRptRev.Dispose(); dtOtherFees.Dispose(); dtRptLogs.Dispose();
            //dtCtrlPages.Dispose(); dtPMRC.Dispose();
            this.Dispose();
        }

        private void chkNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                //Update Table Paging
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.AddWithValue("@RptNo", Convert.ToInt32(txtRptNo.Text));
                sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(txtRevNo.Text));
                if (chkNextPage.Checked == true)
                    sqlcmd.Parameters.AddWithValue("@TblNxtPg", 1);
                else
                    sqlcmd.Parameters.AddWithValue("@TblNxtPg", 0);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdRptTablePage";
                sqlcmd.ExecuteNonQuery();
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            }
            catch { }
        }

        private void txtCancFee_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
            else if ((Char.IsDigit(e.KeyChar) == false && e.KeyChar != 8 && e.KeyChar != 46))
                e.Handled = true;
        }

        private void lnkCancDtls_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (strGroup == "EXEC" || strGroup == "IT" || LogIn.nUserID == 73 || LogIn.nUserID == 247 || LogIn.nUserID == 394) //added K Kohan 11/14/2016, added Ruffy 2-1-2018, added Marlyn 2-26-2018
            {
                int nX = (pnlRecord.Location.X + pnlRecord.Width - pnlCancelFee.Width) / 2;
                int nY = (pnlRecord.Location.Y + pnlRecord.Height - pnlCancelFee.Height) / 2;
                pnlRecord.Enabled = false; pnlCancelFee.Visible = true; pnlCancelFee.BringToFront(); pnlCancelFee.Location = new Point(nX, nY);
                if (nMode == 0)
                {
                    OpenControls(pnlCancelFee, true);
                    btnOKCanc.Text = "O&K";
                }
                else
                {
                    OpenControls(pnlCancelFee, false);
                    btnOKCanc.Text = "Cl&ose";
                }
            }
        }

        private void btnOKCanc_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                if (btnOKCanc.Text == "O&K")
                {
                    if (txtCancFee.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter charges.", Application.ProductName);
                        return;
                    }
                }
                UpdateCancelFee();
            }
            pnlCancelFee.Visible = false; pnlRecord.Enabled = true; btnClose.Enabled = true; lnkCancDtls.Visible = true;
        }

        private void chkCancelled_CheckStateChanged(object sender, EventArgs e)
        {
            if (chkCancelled.Checked == true)
            {
                int nX = (pnlRecord.Location.X + pnlRecord.Width - pnlCancelFee.Width) / 2;
                int nY = (pnlRecord.Location.Y + pnlRecord.Height - pnlCancelFee.Height) / 2;
                pnlRecord.Enabled = false; pnlCancelFee.Visible = true; pnlCancelFee.BringToFront(); pnlCancelFee.Location = new Point(nX, nY);
                OpenControls(pnlCancelFee, true);
                btnOKCanc.Text = "O&K";
                chkNoCharge.Checked = false;
            }
        }

        private void btnCancCancFee_Click(object sender, EventArgs e)
        {
            if (nMode == 0)
            {
                lnkCancDtls.Visible = false;
                if (strGroup == "EXEC" || strGroup == "IT" || LogIn.nUserID == 73 || LogIn.nUserID == 247 || LogIn.nUserID == 394) //added K Kohan 11-14-2016, added Ruffy 2-1-2018, added Marlyn 2-26-2018
                {
                    chkCancelled.Visible = true; chkNoCharge.Visible = true;
                    chkCancelled.Enabled = true; chkNoCharge.Enabled = true;
                    chkCancelled.Checked = false; chkNoCharge.Checked = false;
                    rdoNoCharge.Checked = false; rdoStandard.Checked = false; rdoFixAmount.Checked = false;
                    dtRptExt = PSSClass.FinalReports.CancelledRpt(Convert.ToInt32(txtRptNo.Text));
                    bsRptExt.DataSource = dtRptExt;
                    if (nMode == 0)
                        lnkCancDtls.Visible = false;
                    else
                        lnkCancDtls.Visible = true;
                    if (dtRptExt.Rows.Count > 0)
                    {
                        if (dtRptExt.Rows[0]["CancellationCode"].ToString() == "0")
                            rdoNoCharge.Checked = true;
                        else if (dtRptExt.Rows[0]["CancellationCode"].ToString() == "1")
                            rdoStandard.Checked = true;
                        else if (dtRptExt.Rows[0]["CancellationCode"].ToString() == "2")
                            rdoFixAmount.Checked = true;
                    }
                }
            }
            else
            {
                lnkCancDtls.Visible = true; lnkCancDtls.Enabled = true;
            }
            pnlCancelFee.Visible = false; 
            pnlRecord.Enabled = true; btnClose.Enabled = true;
        }

        private void rdoNoCharge_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNoCharge.Checked == true)
            {
                txtCancCode.Text = "0"; txtCancFee.Text = "0.00"; chkCancelled.Checked = true;
            }
        }

        private void rdoFixAmount_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFixAmount.Checked == true)
            {
                txtCancCode.Text = "2"; txtCancFee.Select(); txtCancFee.SelectAll(); chkCancelled.Checked = true; chkNoCharge.Checked = false;
            }
        }

        private void rdoStandard_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoStandard.Checked == true)
            {
                if (dtRptLogs.Rows.Count == 0)
                {
                    MessageBox.Show("No login billing reference selected." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                txtCancCode.Text = "1"; txtCancFee.Select(); txtCancFee.SelectAll(); chkNoCharge.Checked = false; chkCancelled.Checked = true;
                decimal nDF = 0, nTF;
                for (int i = 0; i < dtRptLogs.Rows.Count; i++)
                {
                    DataTable dt = PSSClass.FinalReports.CancFeeRef(Convert.ToInt32(dtRptLogs.Rows[i]["PSSNo"]), Convert.ToInt16(dtRptLogs.Rows[i]["ServiceCode"]));
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        nDF += Convert.ToDecimal(dt.Rows[j]["Amount"]);
                    }
                    dt.Dispose();
                }
                nTF = (nDF * Convert.ToDecimal(0.10)) + 250;
                txtCancFee.Text = nTF.ToString("#,##0.00");
            }
        }

        private void bnLogMoveFirst_Click(object sender, EventArgs e)
        {
            if (strRptGBL == cboGBLs.Text && strRptSC == cboSCs.Text)
                chkDataSource.Checked = true;
            else
                chkDataSource.Checked = false;
        }

        private void bnLogMovePrev_Click(object sender, EventArgs e)
        {
            if (strRptGBL == cboGBLs.Text && strRptSC == cboSCs.Text)
                chkDataSource.Checked = true;
            else
                chkDataSource.Checked = false;
        }

        private void bnLogMoveLast_Click(object sender, EventArgs e)
        {
            if (strRptGBL == cboGBLs.Text && strRptSC == cboSCs.Text)
                chkDataSource.Checked = true;
            else
                chkDataSource.Checked = false;
        }

        private void bnLogMoveNext_Click(object sender, EventArgs e)
        {
            if (strRptGBL == cboGBLs.Text && strRptSC == cboSCs.Text)
                chkDataSource.Checked = true;
            else
                chkDataSource.Checked = false;
        }

        private void cboWordFolder_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
