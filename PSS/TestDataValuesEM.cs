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

namespace PSS
{
    public partial class TestDataValuesEM : Form
    {
        public string pubCmpy;
        public Int64 nLogNo;
        public int nServiceCode;
        public int nSponsorID;
        public int nMode = 1;
        public byte nEdit = 0;
        public Int32 nRptNo;
        public int nSlashes;
        public string strFormat;

        private string[,] arrLabels = new string[6, 15];
        private DataTable dtMain = new DataTable();
        private DataTable dtSampleSC = new DataTable();
        private DataTable dtEMLocations = new DataTable();

        private byte nDF = 1;

        public TestDataValuesEM()
        {
            InitializeComponent();
        }

        private void LoadAnalysts()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Employees.Analysts();
            if (dt == null)
            {
                return;
            }
            cboAnalysts.DataSource = dt;
            cboAnalysts.DisplayMember = "EmployeeName";
            cboAnalysts.ValueMember = "EmployeeID";

            DataRow dR = dt.NewRow();
            dR["EmployeeName"] = "--select--";
            dR["EmployeeID"] = "0";
            dt.Rows.InsertAt(dR, 0);
            cboAnalysts.SelectedIndex = 0;
        }

        private void LoadLocations()
        {
            dtEMLocations = PSSClass.EnvMonitoring.EMLocations(nSponsorID);
            if (dtEMLocations == null)
            {
                return;
            }
            dgvLocations.DataSource = dtEMLocations;
            DataGridView();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();

            SqlCommand sqlcmd = new SqlCommand();

            //Save
            bsMain.EndEdit();
            DataTable dt = new DataTable();
            dt = dtMain.GetChanges();
            if (dt != null)
            {
                sqlcmd.Connection = sqlcnn;

                //sqlcmd.Parameters.AddWithValue("@CmpyCode", pubCmpy); to update over the weekend 
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                sqlcmd.Parameters.AddWithValue("@DataFormat", cboTableFormats.Text);
                if (txtExptNo.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@ExpNo", Convert.ToInt32(txtExptNo.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@ExpNo", DBNull.Value);
                if (txtPageNo.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@PageNo", Convert.ToInt32(txtPageNo.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@PageNo", DBNull.Value);

                sqlcmd.Parameters.AddWithValue("@AnalystID", cboAnalysts.SelectedValue);
                sqlcmd.Parameters.AddWithValue("@Media", txtMedia.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdLogTest";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlcnn.Dispose();
                    return;
                }
                sqlcmd.Dispose();
                sqlcnn.Close(); sqlcnn.Dispose();
                dtMain.AcceptChanges();
            }

            bsData.EndEdit();
            string strTestData = "";
            dt = new DataTable();
            dt = dtSampleSC.GetChanges();
            if (dt != null)
            {
                sqlcnn = PSSClass.DBConnection.PSSConnection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strTestData = "<TestData>";
                    strTestData = strTestData + "<LocationID>" + dt.Rows[i]["LocationID"].ToString() + "</LocationID>";
                    for (int j = 1; j <= 18; j++)
                    {
                        if (dt.Rows[i]["V" + j.ToString()].ToString().Trim() != null && dt.Rows[i]["V" + j.ToString()].ToString().Trim() != "")
                        {
                            //Escape
                            string strX = dt.Rows[i]["V" + j.ToString()].ToString().Trim();
                            strX = strX.Replace("&", "&amp;");
                            strX = strX.Replace(">", "&gt;");
                            strX = strX.Replace("<", "&lt;");
                            strX = strX.Replace("'", "&apos;");
                            strX = strX.Replace("\"", "&quot;");
                            strTestData = strTestData + "<Value" + j.ToString() + ">" + strX + "</Value" + j.ToString() + ">";
                        }
                    }
                    if (dt.Rows[i]["Notes"] != null && dt.Rows[i]["Notes"].ToString().Trim() != "")
                    {
                        string strN = dt.Rows[i]["Notes"].ToString().Trim();
                        strN = strN.Replace("&", "&amp;");
                        strN = strN.Replace(">", "&gt;");
                        strN = strN.Replace("<", "&lt;");
                        strN = strN.Replace("'", "&apos;");
                        strN = strN.Replace("\"", "&quot;");
                        strTestData = strTestData + "<Note>" + strN + "</Note>";
                    }
                    strTestData = strTestData + "</TestData>";

                    DataTable dtEM = PSSClass.EnvMonitoring.EMTestData(nLogNo, nServiceCode, dt.Rows[i]["LocSlash"].ToString());
                    if (dtEM == null || dtEM.Rows.Count == 0)
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@CmpyCode", pubCmpy);
                        sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                        sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                        sqlcmd.Parameters.AddWithValue("@SlashNo", dt.Rows[i]["LocSlash"].ToString());
                        sqlcmd.Parameters.AddWithValue("@TestDataValues", strTestData);
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spAddTestDataValuesEM";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch
                        {
                        }
                        sqlcmd.Dispose();
                    }
                    else
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@CmpyCode", pubCmpy);
                        sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                        sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                        sqlcmd.Parameters.AddWithValue("@SlashNo", dt.Rows[i]["LocSlash"].ToString());
                        sqlcmd.Parameters.AddWithValue("@TestDataValues", strTestData);
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spUpdTestDataValuesEM";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch
                        {
                        }
                        sqlcmd.Dispose();
                    }                        sqlcmd.Parameters.AddWithValue("@CmpyCode", pubCmpy);

                }
                dtSampleSC.AcceptChanges();
                if (txtReportNo.Text != "")
                {
                    string strRptDtls = "<ExtendedData><ReportData>" +
                                        "<GBLNo>" + txtGBLNo.Text + "</GBLNo>" +
                                        "<SC>" + txtSC.Text + "</SC>" +
                                        "</ReportData></ExtendedData>";
                    PSSClass.FinalReports.UpdFinRptDtls(Convert.ToInt32(txtReportNo.Text), strRptDtls);
                }
            }
            //Update Data Format
            sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            //sqlcmd.Parameters.AddWithValue("@CmpyCode", pubCmpy); - to update over the weekend
            sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
            sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
            sqlcmd.Parameters.AddWithValue("@DataFormat", cboTableFormats.Text);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdDataFormat";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return;
            }
            sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
            this.Dispose();
        }

        private void TestDataValuesEM_Load(object sender, EventArgs e)
        {
            txtGBLNo.Text = nLogNo.ToString();
            txtSC.Text = nServiceCode.ToString();
            txtSpID.Text = nSponsorID.ToString();
            LoadDataFormats();
            LoadAnalysts();
            LoadLocations();

            dtMain.Columns.Add("ExperimentNo", typeof(Int32));
            dtMain.Columns.Add("PageNo", typeof(Int32));
            dtMain.Columns.Add("AnalystID", typeof(Int16));
            dtMain.Columns.Add("Media", typeof(string));
            dtMain.Columns.Add("TableFormat", typeof(Int16));
            bsMain.DataSource = dtMain;

            txtExptNo.DataBindings.Add("Text", bsMain, "ExperimentNo", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtPageNo.DataBindings.Add("Text", bsMain, "PageNo", true, DataSourceUpdateMode.OnPropertyChanged, "");
            cboAnalysts.DataBindings.Add("SelectedValue", bsMain, "AnalystID", true, DataSourceUpdateMode.OnPropertyChanged, "");
            cboTableFormats.DataBindings.Add("SelectedValue", bsMain, "TableFormat", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtAnalystID.DataBindings.Add("Text", bsMain, "AnalystID", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtMedia.DataBindings.Add("Text", bsMain, "Media", true, DataSourceUpdateMode.OnPropertyChanged, "");

            //Table Setup for Test Data Values
            dtSampleSC.Columns.Add("CompanyCode", typeof(string));
            dtSampleSC.Columns.Add("LogNo", typeof(string));
            dtSampleSC.Columns.Add("SlashNo", typeof(string));
            dtSampleSC.Columns.Add("LocSlash", typeof(string));
            dtSampleSC.Columns.Add("LocationID", typeof(Int16));
            dtSampleSC.Columns.Add("V1", typeof(string));
            dtSampleSC.Columns.Add("V2", typeof(string));
            dtSampleSC.Columns.Add("V3", typeof(string));
            dtSampleSC.Columns.Add("V4", typeof(string));
            dtSampleSC.Columns.Add("V5", typeof(string));
            dtSampleSC.Columns.Add("V6", typeof(string));
            dtSampleSC.Columns.Add("V7", typeof(string));
            dtSampleSC.Columns.Add("V8", typeof(string));
            dtSampleSC.Columns.Add("V9", typeof(string));
            dtSampleSC.Columns.Add("V10", typeof(string));
            dtSampleSC.Columns.Add("V11", typeof(string));
            dtSampleSC.Columns.Add("V12", typeof(string));
            dtSampleSC.Columns.Add("V13", typeof(string));
            dtSampleSC.Columns.Add("V14", typeof(string));
            dtSampleSC.Columns.Add("V15", typeof(string));
            dtSampleSC.Columns.Add("V16", typeof(string));
            dtSampleSC.Columns.Add("V17", typeof(string));
            dtSampleSC.Columns.Add("V18", typeof(string));
            //dtSampleSC.Columns.Add("V19", typeof(string));
            //dtSampleSC.Columns.Add("V20", typeof(string));
            //dtSampleSC.Columns.Add("V21", typeof(string));
            //dtSampleSC.Columns.Add("V22", typeof(string));
            //dtSampleSC.Columns.Add("V23", typeof(string));
            //dtSampleSC.Columns.Add("V24", typeof(string));
            //dtSampleSC.Columns.Add("V25", typeof(string));
            //dtSampleSC.Columns.Add("V26", typeof(string));
            //dtSampleSC.Columns.Add("V27", typeof(string));
            //dtSampleSC.Columns.Add("V28", typeof(string));
            //dtSampleSC.Columns.Add("V29", typeof(string));
            //dtSampleSC.Columns.Add("V30", typeof(string));
            //dtSampleSC.Columns.Add("V31", typeof(string));
            //dtSampleSC.Columns.Add("V32", typeof(string));
            //dtSampleSC.Columns.Add("V33", typeof(string));
            //dtSampleSC.Columns.Add("V34", typeof(string));
            //dtSampleSC.Columns.Add("V35", typeof(string));
            //dtSampleSC.Columns.Add("V36", typeof(string));
            //dtSampleSC.Columns.Add("V37", typeof(string));
            //dtSampleSC.Columns.Add("V38", typeof(string));
            //dtSampleSC.Columns.Add("V39", typeof(string));
            //dtSampleSC.Columns.Add("V40", typeof(string));
            //dtSampleSC.Columns.Add("V41", typeof(string));
            //dtSampleSC.Columns.Add("V42", typeof(string));
            //dtSampleSC.Columns.Add("V43", typeof(string));
            //dtSampleSC.Columns.Add("V44", typeof(string));
            //dtSampleSC.Columns.Add("V45", typeof(string));
            //dtSampleSC.Columns.Add("SD1", typeof(bool));
            //dtSampleSC.Columns.Add("SD2", typeof(bool));
            //dtSampleSC.Columns.Add("SD3", typeof(bool));
            //dtSampleSC.Columns.Add("SD4", typeof(bool));
            dtSampleSC.Columns.Add("Notes", typeof(bool));
            bsData.DataSource = dtSampleSC;
            //Databindings for Test Data Values
            txtSlashNo.DataBindings.Add("Text", bsData, "SlashNo");
            txtLocSlash.DataBindings.Add("Text", bsData, "LocSlash");
            txtLocID.DataBindings.Add("Text", bsData, "LocationID", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue1.DataBindings.Add("Text", bsData, "V1", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue2.DataBindings.Add("Text", bsData, "V2", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue3.DataBindings.Add("Text", bsData, "V3", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue4.DataBindings.Add("Text", bsData, "V4", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue5.DataBindings.Add("Text", bsData, "V5", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue6.DataBindings.Add("Text", bsData, "V6", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue7.DataBindings.Add("Text", bsData, "V7", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue8.DataBindings.Add("Text", bsData, "V8", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue9.DataBindings.Add("Text", bsData, "V9", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue10.DataBindings.Add("Text", bsData, "V10", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue11.DataBindings.Add("Text", bsData, "V11", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue12.DataBindings.Add("Text", bsData, "V12", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue13.DataBindings.Add("Text", bsData, "V13", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue14.DataBindings.Add("Text", bsData, "V14", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue15.DataBindings.Add("Text", bsData, "V15", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue16.DataBindings.Add("Text", bsData, "V16", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue17.DataBindings.Add("Text", bsData, "V17", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue18.DataBindings.Add("Text", bsData, "V18", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue19.DataBindings.Add("Text", bsData, "V19", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue20.DataBindings.Add("Text", bsData, "V20", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue21.DataBindings.Add("Text", bsData, "V21", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue22.DataBindings.Add("Text", bsData, "V22", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue23.DataBindings.Add("Text", bsData, "V23", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue24.DataBindings.Add("Text", bsData, "V24", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue25.DataBindings.Add("Text", bsData, "V25", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue26.DataBindings.Add("Text", bsData, "V26", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue27.DataBindings.Add("Text", bsData, "V27", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue28.DataBindings.Add("Text", bsData, "V28", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue29.DataBindings.Add("Text", bsData, "V29", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue30.DataBindings.Add("Text", bsData, "V30", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue31.DataBindings.Add("Text", bsData, "V31", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue32.DataBindings.Add("Text", bsData, "V32", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue33.DataBindings.Add("Text", bsData, "V33", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue34.DataBindings.Add("Text", bsData, "V34", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue35.DataBindings.Add("Text", bsData, "V35", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue36.DataBindings.Add("Text", bsData, "V36", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue37.DataBindings.Add("Text", bsData, "V37", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue38.DataBindings.Add("Text", bsData, "V38", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue39.DataBindings.Add("Text", bsData, "V39", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue40.DataBindings.Add("Text", bsData, "V40", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue41.DataBindings.Add("Text", bsData, "V41", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue42.DataBindings.Add("Text", bsData, "V42", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue43.DataBindings.Add("Text", bsData, "V43", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue44.DataBindings.Add("Text", bsData, "V44", true, DataSourceUpdateMode.OnPropertyChanged, "");
            //txtDataValue45.DataBindings.Add("Text", bsData, "V45", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtNote.DataBindings.Add("Text", bsData, "Notes", true, DataSourceUpdateMode.OnPropertyChanged, "");
            LoadDataValues();
            this.Top = 155; this.Left = 5;
            bnData.Enabled = true;
            if (nRptNo != 0)
            {
                txtReportNo.Text = nRptNo.ToString();
                DataTable dt = PSSClass.FinalReports.FinRptStatus(nRptNo);

                if (dt == null || dt.Rows.Count == 0)
                {
                    OpenControls(this, true); OpenControls(tabPage1, true); btnCreate.Enabled = true;
                }
                else
                {
                    OpenControls(this, false); OpenControls(tabPage1, false); btnCreate.Enabled = false;
                }
            }
            txtSlashNo.Text = "001" + "-" + nSlashes.ToString("000");
            txtSlashes.Text = nSlashes.ToString();
        }

        private void LoadDataValues()
        {
            dtSampleSC = PSSClass.Samples.ExTestDataValuesEM(pubCmpy, nLogNo, nServiceCode);//, txtSlashNo.Text
            if (dtSampleSC == null)
            {
                return;
            }
            bsData.DataSource = dtSampleSC;
            bnData.BindingSource = bsData;
            if (dtSampleSC.Rows.Count > 0)
            {
                try
                {
                    DataRow dR = dtMain.NewRow();
                    dR["ExperimentNo"] = dtSampleSC.Rows[0]["ExperimentNo"];
                    dR["PageNo"] = dtSampleSC.Rows[0]["PageNo"];
                    dR["AnalystID"] = dtSampleSC.Rows[0]["AnalystID"];
                    dR["Media"] = dtSampleSC.Rows[0]["Media"];
                    dR["TableFormat"] = dtSampleSC.Rows[0]["TableFormat"];
                    dtMain.Rows.Add(dR);
                    dtMain.AcceptChanges();
                    bsMain.DataSource = dtMain;
                    OpenControls(this, true); OpenControls(tabData, true);
                }
                catch { }
            }
            else
            {
                try
                {
                    DataRow dR = dtMain.NewRow();
                    dR["ExperimentNo"] = 0;
                    dR["PageNo"] = 0;
                    dR["AnalystID"] = 0;
                    dR["Media"] = "";
                    dR["TableFormat"] = 1;
                    dtMain.Rows.Add(dR);
                    dtMain.AcceptChanges();
                    bsMain.DataSource = dtMain;
                    OpenControls(this, true); OpenControls(tabData, true);
                }
                catch { }
            }
        }

        private void LoadDataFormats()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Samples.SCDataFormats(nServiceCode, nSponsorID);
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No defined data form for this Service Code/Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nDF = 0;
                this.Dispose();
                return;
            }
            cboTableFormats.DataSource = dt;
            cboTableFormats.DisplayMember = "FormatNo";
            cboTableFormats.ValueMember = "FormatNo";
            cboTableFormats.SelectedIndex = 0;
            txtTableRptID.Text = dt.Rows[0]["TableReportID"].ToString();
        }

        private void OpenControls(Control c, bool b)
        {
            foreach (Control Ctrl in c.Controls)
            {
                switch (Ctrl.GetType().ToString())
                {
                    case "System.Windows.Forms.CheckBox":
                        ((CheckBox)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.RadioButton":
                        ((RadioButton)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.TextBox":
                        ((TextBox)Ctrl).ReadOnly = !b;
                        break;

                    case "GISControls.TextBoxChar": //User Control
                        ((GISControls.TextBoxChar)Ctrl).ReadOnly = !b;
                        break;

                    case "GISControls.TextBoxAdjHt": //User Control
                        ((GISControls.TextBoxAdjHt)Ctrl).ReadOnly = !b;
                        break;

                    case "System.Windows.Forms.RichTextBox":
                        ((RichTextBox)Ctrl).ReadOnly = !b;
                        break;

                    case "System.Windows.Forms.ComboBox":
                        ((ComboBox)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.MaskedTextBox":
                        ((MaskedTextBox)Ctrl).ReadOnly = !b;
                        break;

                    case "System.Windows.Forms.DateTimePicker":
                        ((DateTimePicker)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.DataGridView":
                        ((DataGridView)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.GroupBox":
                        ((GroupBox)Ctrl).Enabled = b;
                        break;

                    default:
                        if (Ctrl.Controls.Count > 0)
                            OpenControls(Ctrl, true);
                        break;
                }
            }
            c.Enabled = true;
        }

        private void cboTableFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTableFormats.ValueMember != null)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = PSSClass.Samples.ExTestDataLabels(nServiceCode, nSponsorID, Convert.ToInt16(cboTableFormats.Text));//Convert.ToInt16(cboTableFormats.Text)
                    if (dt == null)
                    {
                        MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    lblDataLabel1.Text = dt.Rows[0]["L1"].ToString();
                    lblDataLabel2.Text = dt.Rows[0]["L2"].ToString();
                    lblDataLabel3.Text = dt.Rows[0]["L3"].ToString();
                    lblDataLabel4.Text = dt.Rows[0]["L4"].ToString();
                    lblDataLabel5.Text = dt.Rows[0]["L5"].ToString();
                    lblDataLabel6.Text = dt.Rows[0]["L6"].ToString();
                    lblDataLabel7.Text = dt.Rows[0]["L7"].ToString();
                    lblDataLabel8.Text = dt.Rows[0]["L8"].ToString();
                    lblDataLabel9.Text = dt.Rows[0]["L9"].ToString();
                    lblDataLabel10.Text = dt.Rows[0]["L10"].ToString();
                    lblDataLabel11.Text = dt.Rows[0]["L11"].ToString();
                    lblDataLabel12.Text = dt.Rows[0]["L12"].ToString();
                    lblDataLabel13.Text = dt.Rows[0]["L13"].ToString();
                    lblDataLabel14.Text = dt.Rows[0]["L14"].ToString();
                    lblDataLabel15.Text = dt.Rows[0]["L15"].ToString();
                    lblDataLabel16.Text = dt.Rows[0]["L16"].ToString();
                    lblDataLabel17.Text = dt.Rows[0]["L17"].ToString();
                    lblDataLabel18.Text = dt.Rows[0]["L18"].ToString();
                    //lblDataLabel19.Text = dt.Rows[0]["L19"].ToString();
                    //lblDataLabel20.Text = dt.Rows[0]["L20"].ToString();
                    //lblDataLabel21.Text = dt.Rows[0]["L21"].ToString();
                    //lblDataLabel22.Text = dt.Rows[0]["L22"].ToString();
                    //lblDataLabel23.Text = dt.Rows[0]["L23"].ToString();
                    //lblDataLabel24.Text = dt.Rows[0]["L24"].ToString();
                    //lblDataLabel25.Text = dt.Rows[0]["L25"].ToString();
                    //lblDataLabel26.Text = dt.Rows[0]["L26"].ToString();
                    //lblDataLabel27.Text = dt.Rows[0]["L27"].ToString();
                    //lblDataLabel28.Text = dt.Rows[0]["L28"].ToString();
                    //lblDataLabel29.Text = dt.Rows[0]["L29"].ToString();
                    //lblDataLabel30.Text = dt.Rows[0]["L30"].ToString();
                    //lblDataLabel31.Text = dt.Rows[0]["L31"].ToString();
                    //lblDataLabel32.Text = dt.Rows[0]["L32"].ToString();
                    //lblDataLabel33.Text = dt.Rows[0]["L33"].ToString();
                    //lblDataLabel34.Text = dt.Rows[0]["L34"].ToString();
                    //lblDataLabel35.Text = dt.Rows[0]["L35"].ToString();
                    //lblDataLabel36.Text = dt.Rows[0]["L36"].ToString();
                    //lblDataLabel37.Text = dt.Rows[0]["L37"].ToString();
                    //lblDataLabel38.Text = dt.Rows[0]["L38"].ToString();
                    //lblDataLabel39.Text = dt.Rows[0]["L39"].ToString();
                    //lblDataLabel40.Text = dt.Rows[0]["L40"].ToString();
                    //lblDataLabel41.Text = dt.Rows[0]["L41"].ToString();
                    //lblDataLabel42.Text = dt.Rows[0]["L42"].ToString();
                    //lblDataLabel43.Text = dt.Rows[0]["L43"].ToString();
                    //lblDataLabel44.Text = dt.Rows[0]["L44"].ToString();
                    //lblDataLabel45.Text = dt.Rows[0]["L45"].ToString();
                    txtTableRptID.Text = dt.Rows[0]["TableReportID"].ToString();
                    lblDescription.Text = dt.Rows[0]["TableDesc"].ToString();
                }
                catch { }
            }
        }

        private void txtLocation_Enter(object sender, EventArgs e)
        {
            if (dgvLocations.Enabled == true)
            {
                dgvLocations.Visible = true; txtLocation.Select();
            }
        }

        private void dgvLocations_DoubleClick(object sender, EventArgs e)
        {
            txtLocation.Text = dgvLocations.Rows[dgvLocations.CurrentCell.RowIndex].Cells["SampleLoc"].Value.ToString();
            txtLocID.Text = dgvLocations.Rows[dgvLocations.CurrentCell.RowIndex].Cells["LocationID"].Value.ToString();
            dgvLocations.Visible = false;
        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {
            //if (nMode != 0)
            //{
                DataView dvwLocations;
                dvwLocations = new DataView(dtEMLocations, "SampleLoc like '%" + txtLocation.Text.Trim().Replace("'", "''") + "%'", "SampleLoc", DataViewRowState.CurrentRows);
                dgvLocations.DataSource = dvwLocations;
                DataGridView();
            //}
        }

        private void DataGridView()
        {
            dgvLocations.EnableHeadersVisualStyles = false;
            dgvLocations.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLocations.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvLocations.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLocations.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvLocations.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgvLocations.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvLocations.Columns[0].Width = 120;
            dgvLocations.Columns[1].Width = 75;
            dgvLocations.Columns[2].Width = 142;
            dgvLocations.Columns[3].Width = 75;
            dgvLocations.Columns[4].Width = 140;
            dgvLocations.Columns[5].Visible = false;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (dtSampleSC.Rows.Count > 0)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("This process would delete existing data.", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dReply == DialogResult.OK)
                {
                    DialogResult dAnswer = new DialogResult();
                    dAnswer = MessageBox.Show("Please confirm if you want to do this.", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (dAnswer == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                    return;
            }
            dtSampleSC.Rows.Clear();
            int nS = Convert.ToInt16(txtSlashes.Text);
            for (int i = 0; i < nS; i++)
            {
                DataRow dR = dtSampleSC.NewRow();
                dR["CompanyCode"] = pubCmpy;
                dR["LogNo"] = txtGBLNo.Text;
                dR["LocSlash"] = (i + 1).ToString("000");
                dR["V1"] = "";
                dR["V2"] = "";
                dR["V3"] = "";
                dR["V4"] = "";
                if (nSponsorID == 1345)
                    dR["V5"] = "Yes";
                else
                    dR["V5"] = "";
                dR["V6"] = "";
                dR["V7"] = "";
                dR["V8"] = "";
                dR["V9"] = "";
                dR["V10"] = "";
                dR["V11"] = "";
                dR["V12"] = "";
                dR["V13"] = "";
                dR["V14"] = "";
                dR["V15"] = "";
                dR["V16"] = "";
                dR["V17"] = "";
                dR["V18"] = "";
                dtSampleSC.Rows.Add(dR);
            }
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();

            SqlCommand sqlcmd = new SqlCommand();

            //Save
            bsMain.EndEdit();
            DataTable dt = new DataTable();
            dt = dtMain.GetChanges();
            if (dt != null)
            {
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                sqlcmd.Parameters.AddWithValue("@DataFormat", cboTableFormats.Text);
                if (txtExptNo.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@ExpNo", Convert.ToInt32(txtExptNo.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@ExpNo", DBNull.Value);
                if (txtPageNo.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@PageNo", Convert.ToInt32(txtPageNo.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@PageNo", DBNull.Value);

                sqlcmd.Parameters.AddWithValue("@AnalystID", cboAnalysts.SelectedValue);
                sqlcmd.Parameters.AddWithValue("@Media", txtMedia.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdLogTest";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlcnn.Dispose();
                    return;
                }
                sqlcmd.Dispose();
                sqlcnn.Close(); sqlcnn.Dispose();
                dtMain.AcceptChanges();
            }

            bsData.EndEdit();
            string strTestData = "";
            dt = new DataTable();
            dt = dtSampleSC.GetChanges();
            if (dt != null)
            {
                sqlcnn = PSSClass.DBConnection.PSSConnection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strTestData = "<TestData>";
                    strTestData = strTestData + "<LocationID>" + dt.Rows[i]["LocationID"].ToString() + "</LocationID>";
                    for (int j = 1; j <= 18; j++)
                    {
                        if (dt.Rows[i]["V" + j.ToString()].ToString().Trim() != null && dt.Rows[i]["V" + j.ToString()].ToString().Trim() != "")
                        {
                            //Escape
                            string strX = dt.Rows[i]["V" + j.ToString()].ToString().Trim();
                            strX = strX.Replace("&", "&amp;");
                            strX = strX.Replace(">", "&gt;");
                            strX = strX.Replace("<", "&lt;");
                            strX = strX.Replace("'", "&apos;");
                            strX = strX.Replace("\"", "&quot;");
                            strTestData = strTestData + "<Value" + j.ToString() + ">" + strX + "</Value" + j.ToString() + ">";
                        }
                    }
                    if (dt.Rows[i]["Notes"] != null && dt.Rows[i]["Notes"].ToString().Trim() != "")
                    {
                        string strN = dt.Rows[i]["Notes"].ToString().Trim();
                        strN = strN.Replace("&", "&amp;");
                        strN = strN.Replace(">", "&gt;");
                        strN = strN.Replace("<", "&lt;");
                        strN = strN.Replace("'", "&apos;");
                        strN = strN.Replace("\"", "&quot;");
                        strTestData = strTestData + "<Note>" + strN + "</Note>";
                    }
                    strTestData = strTestData + "</TestData>";

                    DataTable dtEM = PSSClass.EnvMonitoring.EMTestData(nLogNo, nServiceCode, dt.Rows[i]["LocSlash"].ToString());
                    if (dtEM == null || dtEM.Rows.Count == 0)
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                        sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                        sqlcmd.Parameters.AddWithValue("@SlashNo", dt.Rows[i]["LocSlash"].ToString());
                        sqlcmd.Parameters.AddWithValue("@TestDataValues", strTestData);
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spAddTestDataValuesEM";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch
                        {
                        }
                        sqlcmd.Dispose();
                    }
                    else
                    {
                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;

                        sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                        sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                        sqlcmd.Parameters.AddWithValue("@SlashNo", dt.Rows[i]["LocSlash"].ToString());
                        sqlcmd.Parameters.AddWithValue("@TestDataValues", strTestData);
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spUpdTestDataValuesEM";
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
                dtSampleSC.AcceptChanges();
            }

            //Update Data Format
            sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
            sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
            sqlcmd.Parameters.AddWithValue("@DataFormat", cboTableFormats.Text);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdDataFormat";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return;
            }
            sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();

            if (File.Exists(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + txtTableRptID.Text.Replace(".rpt", "") + ".rpt") == false)
            {
                MessageBox.Show("Report file is under construction." + Environment.NewLine + "Please contact the IT Department for updates.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string strRptName = "";
            if (txtSC.Text == "297")
            {
                strRptName = "SpeedRpt_297.rpt";
            }
            else
            {
                if (File.Exists(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + txtTableRptID.Text.Replace(".rpt", "") + ".rpt") == false)
                {
                    MessageBox.Show("Report file is under construction." + Environment.NewLine + "Please contact the IT Department for updates.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                strRptName = txtTableRptID.Text + ".rpt";
            }
            LabRpt rpt = new LabRpt();
            rpt.rptName = "SpeedReport";
            rpt.CmpyCode = "P";
            rpt.rptFile = strRptName;
            rpt.WindowState = FormWindowState.Maximized;
            rpt.nLogNo = Convert.ToInt32(txtGBLNo.Text);
            rpt.nSC = Convert.ToInt32(txtSC.Text);
            rpt.SpID = Convert.ToInt16(txtSpID.Text);

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void txtLocID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataRow[] dRows = dtEMLocations.Select("LocationID = " + Convert.ToInt16(txtLocID.Text)); //.ToString()
                if (dRows.Length > 0)
                    txtLocation.Text = dRows[0]["SampleLoc"].ToString();
                else
                    txtLocation.Text = "";
            }
            catch { }
        }

        private void btnEMail_Click(object sender, EventArgs e)
        {
            string strRptName = "";
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to send this to the Sponsor?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
            if (txtSC.Text == "297")
            {
                strRptName = "SpeedRpt_297.rpt";
            }
            else
            {
                if (File.Exists(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + txtTableRptID.Text.Replace(".rpt", "") + ".rpt") == false)
                {
                    MessageBox.Show("Report file is under construction." + Environment.NewLine + "Please contact the IT Department for updates.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                strRptName = txtTableRptID.Text + ".rpt";
            }
            LabRpt rpt = new LabRpt();
            rpt.rptName = "SpeedReport";
            rpt.rptFile = strRptName;
            rpt.CmpyCode = "P";
            rpt.WindowState = FormWindowState.Maximized;
            rpt.nLogNo = Convert.ToInt32(txtGBLNo.Text);
            rpt.nSC = Convert.ToInt32(txtSC.Text);
            rpt.SpID = Convert.ToInt16(txtSpID.Text);

            try
            {
                rpt.Show();
            }
            catch { }
            string pdfFile = @"\\PSAPP01\IT Files\PTS\PDF Reports\SpeedReports\" + "SR-" + txtGBLNo.Text + "-" + txtSC.Text + ".pdf";
            //MessageBox.Show(pdfFile);
            int nConID = PSSClass.Samples.LogContactID(Convert.ToInt32(txtGBLNo.Text));

            string strText = "", strEMail = "";
            string strCFName = "";// PSSClass.Quotations.ContactFirstName(txtQuoteNo.Text, nRevNo);
            string strSignature = ReadSignature();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            sqlcmd = new SqlCommand("SELECT FirstName FROM Contacts WHERE ContactID = " + nConID, sqlcnn);
            sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                strCFName = sqldr.GetValue(0).ToString();
            }
            sqldr.Close(); sqlcmd.Dispose();

            sqlcmd = new SqlCommand("SELECT EMailAddress FROM ContactEMAddresses WHERE ContactID = " + nConID.ToString() +
                                    " AND SpeedReports = 1", sqlcnn);
            sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                strEMail = sqldr.GetValue(0).ToString();
                strEMail.Replace(";", ",");
            }
            sqldr.Close(); sqlcmd.Dispose();
            if (strEMail.Trim() == "")
            {
                MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please contact Technical Services Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sqlcnn.Close(); sqlcnn.Dispose();
            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.

            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

            oMsg.HTMLBody = "<FONT face=\"Arial\">";
            // Set HTMLBody. 
            //add the body of the email
            strText = "Dear " + strCFName + ", <br/><br/>" +
                "For your convenience attached you will find our Speed Report™ on Environmental Monitoring.<br/><br/>";

            oMsg.HTMLBody += strText.Trim() + strSignature;

            //Add an attachment.
            oMsg.Attachments.Add(pdfFile);
            //oMsg.Attachments.Add(crafFile);
            //Subject line
            oMsg.Subject = "Article: " + PSSClass.Samples.ArticleDesc(pubCmpy, Convert.ToInt32(txtGBLNo.Text));
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
            //Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(strEMail); // "adelacruz@gibraltarlabsinc.com"

            string[] EMAddresses = strEMail.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < EMAddresses.Length; i++)
            {
                if (EMAddresses[i].Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                    oRecip.Resolve();
                }
            }
            //oRecip.Resolve();
            oMsg.Display();

            // Send.
            //oMsg.Send();
            //((Outlook._MailItem)oMsg).Send();
            // Clean up.
            //oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;
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

        private void btnEMLocFile_Click(object sender, EventArgs e)
        {
            this.Close(); this.Dispose();
            int intOpen = PSSClass.General.OpenForm(typeof(EMLocations));

            if (intOpen == 0)
            {
                EMLocations childForm = new EMLocations();
                childForm.MdiParent = Program.mdi;                    
                childForm.Text = "ENVIRONMENTAL MONITORING LOCATIONS";
                childForm.Show();
            }
        }

        private void TestDataValuesEM_Activated(object sender, EventArgs e)
        {
            string strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "TestDataValuesEM");

            if (nRptNo != 0)
            {
                txtReportNo.Text = nRptNo.ToString();
                DataTable dt = PSSClass.FinalReports.FinRptStatus(nRptNo);

                if (dt == null || dt.Rows.Count == 0)
                {
                    if (strFileAccess == "RO")
                    {
                        OpenControls(this, false); OpenControls(tabPage1, false); OpenControls(tabPage3, false);
                        btnPrintPreview.Enabled = false; btnEMail.Enabled = false; btnCreate.Enabled = false;
                    }
                    else if (strFileAccess == "RW" || strFileAccess == "FA")
                    {
                        if (PSSClass.FinalReports.RptDateApproved(nRptNo) == false)
                        {
                            OpenControls(this, true); OpenControls(tabPage1, true); OpenControls(tabPage3, true);
                            btnCreate.Enabled = true;
                        }
                        else
                        {
                            OpenControls(this, false); OpenControls(tabPage1, false); OpenControls(tabPage3, false);
                            btnCreate.Enabled = false;
                        }
                        btnPrintPreview.Enabled = true; 
                        //if (txtSpID.Text != "3058" && txtSpID.Text != "1345")// Temporary tag - Collagen Matrix - Allendale from 3066
                        //    btnEMail.Enabled = false;
                        //else
                            btnEMail.Enabled = true;
                    }
                }
                else
                {
                    OpenControls(this, false); OpenControls(tabPage1, false); OpenControls(tabPage3, false);
                    btnPrintPreview.Enabled = false; btnEMail.Enabled = false; btnCreate.Enabled = false;
                }
            }
            else
            {
                if (strFileAccess == "RO")
                {
                    OpenControls(this, false); OpenControls(tabPage1, false); OpenControls(tabPage3, false);
                    btnPrintPreview.Enabled = false; btnEMail.Enabled = false; btnCreate.Enabled = false;
                }
                else if (strFileAccess == "RW" || strFileAccess == "FA")
                {
                    OpenControls(this, true); OpenControls(tabPage1, true); OpenControls(tabPage3, true);
                    btnPrintPreview.Enabled = true; btnCreate.Enabled = true;
                    //if (txtSpID.Text != "3058" && txtSpID.Text != "1345")// Temporary tag - Collagen Matrix - Allendale from 3066
                    //    btnEMail.Enabled = false;
                    //else
                        btnEMail.Enabled = true;
                }
            }
            OpenControls(pnlLogData, false);
        }

        private void picFillCodes_Click(object sender, EventArgs e)
        {
            if (dgvLocations.Enabled == true)
            {
                dgvLocations.Visible = true; txtLocation.Select();
            }
        }
    }
}
