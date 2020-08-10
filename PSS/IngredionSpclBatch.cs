using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GIS
{
    public partial class IngredionSpclBatch : Form
    {
        public IngredionSpclBatch()
        {
            InitializeComponent();
        }

        private void IngredionSpclBatch_Load(object sender, EventArgs e)
        {
            DataTable dtSp = GISClass.Ingredion.SponsorNames();
            cboSponsors.DataSource = dtSp;
            cboSponsors.DisplayMember = "SponsorName";
            cboSponsors.ValueMember = "SponsorID";

            cboSponsorID.DataSource = dtSp;
            cboSponsorID.DisplayMember = "SponsorID";
            cboSponsorID.ValueMember = "SponsorName";

            try
            {
                DataTable dtContacts = GISClass.Ingredion.Contacts(Convert.ToInt16(cboSponsors.SelectedValue));
                cboContacts.DataSource = dtContacts;
                cboContacts.DisplayMember = "Contact";
                cboContacts.ValueMember = "ContactID";
            }
            catch { }

            DataTable dtPIN = new DataTable();
            dtPIN.Columns.Add("FillCode", typeof(string));
            dtPIN.Columns.Add("PINCode", typeof(string));

            DataRow dRS = dtPIN.NewRow();
            //50-3130
            dRS["FillCode"] = "503130";
            dRS["PINCode"] = "32609100 IMF";
            dtPIN.Rows.Add(dRS);
            //50-3136
            dRS = dtPIN.NewRow();
            dRS["FillCode"] = "503136";
            dRS["PINCode"] = "32609101 IMF";
            dtPIN.Rows.Add(dRS);

            //50-1116
            dRS = dtPIN.NewRow(); 
            dRS["FillCode"] = "501116";
            dRS["PINCode"] = "32109101 IMF";
            dtPIN.Rows.Add(dRS);

            //50-1901
            dRS = dtPIN.NewRow();
            dRS["FillCode"] = "501901";
            dRS["PINCode"] = "32300B00 IMF";
            dtPIN.Rows.Add(dRS);

            //50-1340
            dRS = dtPIN.NewRow();
            dRS["FillCode"] = "501340";
            dRS["PINCode"] = "32106400";
            dtPIN.Rows.Add(dRS);

            //50-1803
            dRS = dtPIN.NewRow();
            dRS["FillCode"] = "501803";
            dRS["PINCode"] = "10900300";
            dtPIN.Rows.Add(dRS);

            cboFillCodes.DataSource = dtPIN;
            cboFillCodes.DisplayMember = "FillCode";
            cboFillCodes.ValueMember = "PINCode";
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
            txtQuoteNo.Text = "2017.1223.R0";//2015.0992.R2 Changed from R1 8/3/2016
            try
            {
                txtPONo.Text = GISClass.Sponsors.SponsorLastPO(Convert.ToInt16(cboSponsorID.Text));
            }
            catch { }
            txtBookNo.Text = "828";
        }

        private void cboSponsorID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cboSponsors.SelectedValue = cboSponsorID.Text;
            }
            catch { }
            txtQuoteNo.Text = "2017.1223.R0";//2015.0992.R2 Changed from R1 8/3/2016
            try
            {
                txtPONo.Text = GISClass.Sponsors.SponsorLastPO(Convert.ToInt16(cboSponsorID.Text));
            }
            catch { }
            txtBookNo.Text = "828";
        }

        private void cboContacts_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnOKPrint_Click(object sender, EventArgs e)
        {
            if (cboFillCodes.SelectedIndex == -1)
            {
                MessageBox.Show("Please select fill code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cboFillCodes.Text = "";
                return;
            }

            //Log Master Record
            //Int32 nLogNo = GISClass.General.NewID("LogMaster", "GBLNo");
            Int32 nLogNo = GISClass.General.NewGBLNo("LogMaster", "GBLNo");

            int nBags = Convert.ToInt16(txtBags.Text);
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered. Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@nMode", 1);
            sqlcmd.Parameters.AddWithValue("@SampleNo", nLogNo);
            sqlcmd.Parameters.AddWithValue("@RecDte", DateTime.Now);
            sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(cboSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@ConID", cboContacts.SelectedValue);
            sqlcmd.Parameters.AddWithValue("@Article",cboFillCodes.Text);
            sqlcmd.Parameters.AddWithValue("@CtlSub", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@SampDesc", (nBags-1).ToString() + " whirl-pack bags, containing white powder.");
            sqlcmd.Parameters.AddWithValue("@AddlNotes", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@SSNo", txtSSFormNo.Text);
            sqlcmd.Parameters.AddWithValue("@Rush", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@StorageCode", 1);
            sqlcmd.Parameters.AddWithValue("@StorageDesc", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@ReceiptCode", 1);
            sqlcmd.Parameters.AddWithValue("@Locked", 0);
            sqlcmd.Parameters.AddWithValue("@AnaDone", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@MngrChecked", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@DteCancelled", DBNull.Value);
            sqlcmd.Parameters.AddWithValue("@RetestNo", 0);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditLogMstr";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException exSql)
            {
                if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                {
                    MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                return;
            }
            sqlcmd.Dispose();

            //Log Samples
            for (int i = 0; i < nBags; i++)
            {
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", 1);
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SlashID", i);
                if (i == 0)//Composite Sample entry
                {
                    sqlcmd.Parameters.AddWithValue("@SlashNo", txtCompBag.Text);
                    sqlcmd.Parameters.AddWithValue("@SampleDesc", "Bags 1-" + nBags.ToString() + " Comp.");
                    sqlcmd.Parameters.AddWithValue("@AddlData", "<SamplesData><Value1>" + txtLotNo.Text + "</Value1><Value2></Value2></SamplesData>");
                }
                else
                {
                    sqlcmd.Parameters.AddWithValue("@SlashNo", i.ToString("000"));
                    sqlcmd.Parameters.AddWithValue("@SampleDesc", "Bag #" + i.ToString());
                    sqlcmd.Parameters.AddWithValue("@AddlData", "<SamplesData><Value1>" + txtLotNo.Text + "</Value1><Value2></Value2></SamplesData>");
                }
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditLogSample";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException exSql)
                {
                    if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                    {
                        MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    return;
                }
            }
            sqlcmd.Dispose();
            
            //Lot Tests
            DataTable dtLogTests = new DataTable();
            dtLogTests = GISClass.Ingredion.SpecialBatch(cboFillCodes.Text);
            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", 1);
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SC", dtLogTests.Rows[i]["ServiceCode"]);
                sqlcmd.Parameters.AddWithValue("@ProtNo", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@StartDte", DateTime.Now);
                sqlcmd.Parameters.AddWithValue("@EndDte", DateTime.Now.AddDays(5));
                sqlcmd.Parameters.AddWithValue("@QuoteNo", txtQuoteNo.Text);
                sqlcmd.Parameters.AddWithValue("@PONo", txtPONo.Text);
                sqlcmd.Parameters.AddWithValue("@Samples", "1");
                sqlcmd.Parameters.AddWithValue("@Slashes", "1");
                sqlcmd.Parameters.AddWithValue("@BookNo", txtBookNo.Text);
                sqlcmd.Parameters.AddWithValue("@BillQty", dtLogTests.Rows[i]["BillQty"]);
                sqlcmd.Parameters.AddWithValue("@EC", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@ECType", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@ECLen", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@ECEndDte", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@DteSampled", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@ExtData", "<SCExtData></SCExtData>");
                sqlcmd.Parameters.AddWithValue("@AddlNotes", "");
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditLogTest";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException exSql)
                {
                    if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                    {
                        MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    return;
                }
            }
            sqlcmd.Dispose();

            //LogTestSamples
            //INGREDION Test Results Table
            //DataTable dtSC = new DataTable();
            //dtSC = GISClass.Ingredion.SpecialBatchSC(cboFillCodes.Text);
            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@nMode", 1);
                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SC", dtLogTests.Rows[i]["ServiceCode"]);
                sqlcmd.Parameters.AddWithValue("@DateTested", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@TestResult", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@Note", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@AnalystID", DBNull.Value);
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spAddEditIngredionData";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (System.Data.SqlClient.SqlException exSql)
                {
                    if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                    {
                        MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    return;
                }
            }
            sqlcmd.Dispose();
            //Main Test Results Table
            byte nSwSC = 0, nSwSlash = 0; string strSlash = "";
            DataTable dtSlashSC  = new DataTable();
            dtSlashSC  = GISClass.Ingredion.SpecialBatchSlash(cboFillCodes.Text);
            for (int i = 0; i < dtSlashSC.Rows.Count; i++)
            {
                nSwSC = 0; nSwSlash = 0;

                strSlash = dtSlashSC.Rows[i]["SlashNo"].ToString();
                try
                {
                    int nS = Convert.ToInt16(strSlash);
                    strSlash = nS.ToString("00");
                }
                catch { }

                for (int j = 0; j < dtLogTests.Rows.Count; j++ )
                {
                    if (dtSlashSC.Rows[i]["ServiceCode"].ToString() == dtLogTests.Rows[j]["ServiceCode"].ToString())
                    {
                        nSwSC = 1;
                        break;
                    }
                }

                for (int k = 0; k < nBags; k++)
                {
                    if (strSlash == k.ToString("00"))
                    {
                        nSwSlash = 1;
                        break;
                    }
                }

                if (nSwSC == 1 && nSwSlash == 1)
                {
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;
                    sqlcmd.Parameters.AddWithValue("@nMode", 1);
                    sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                    sqlcmd.Parameters.AddWithValue("@SC", dtSlashSC.Rows[i]["ServiceCode"]);
                    sqlcmd.Parameters.AddWithValue("@SlashNo", strSlash);
                    sqlcmd.Parameters.AddWithValue("@SpID", DBNull.Value);
                    sqlcmd.Parameters.AddWithValue("@TestResults", "<TestData></TestData>");
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spAddEditSlashSC";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException exSql)
                    {
                        if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                        {
                            MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                        return;
                    }
                }
            }
            //Billing Reference
            DataTable dt = GISClass.Quotations.LoadLoginTests(txtQuoteNo.Text);
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }

            for (int i = 0; i < dtLogTests.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (dtLogTests.Rows[i]["Servicecode"].ToString().Trim() == dt.Rows[j]["ServiceCode"].ToString().Trim())
                    {
                        string strQ = txtQuoteNo.Text;
                        int nI = strQ.IndexOf("R");
                        string strQNo = strQ.Substring(0, nI - 1);
                        string strRNo = strQ.Substring((nI + 1), strQ.Length - (nI + 1));

                        sqlcmd = new SqlCommand();
                        sqlcmd.Connection = sqlcnn;
                        sqlcmd.Parameters.AddWithValue("@nMode", 1);
                        sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                        sqlcmd.Parameters.AddWithValue("@SC", dtLogTests.Rows[i]["ServiceCode"]);
                        sqlcmd.Parameters.AddWithValue("@QuoteNo", strQNo);
                        sqlcmd.Parameters.AddWithValue("@RevNo", Convert.ToInt16(strRNo));
                        sqlcmd.Parameters.AddWithValue("@ControlNo", dt.Rows[j]["ControlNo"]);
                        sqlcmd.Parameters.AddWithValue("@BillQty", dtLogTests.Rows[i]["BillQty"]);
                        if (dt.Rows[j]["UnitPrice"] != DBNull.Value && dt.Rows[j]["UnitPrice"].ToString() != "0")
                            sqlcmd.Parameters.AddWithValue("@UnitPrice", dt.Rows[j]["UnitPrice"]);
                        else
                            sqlcmd.Parameters.AddWithValue("@UnitPrice", DBNull.Value);
                        sqlcmd.Parameters.AddWithValue("@Rush", DBNull.Value);
                        sqlcmd.Parameters.AddWithValue("@RushPrice", DBNull.Value);
                        sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        sqlcmd.CommandText = "spAddEditBillRef";
                        try
                        {
                            sqlcmd.ExecuteNonQuery();
                        }
                        catch (System.Data.SqlClient.SqlException exSql)
                        {
                            if (exSql.Message.ToString().IndexOf("PRIMARY KEY") >= 0)
                            {
                                MessageBox.Show(exSql.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                            return;
                        }
                        sqlcmd.Dispose(); 
                    }
                }
            }
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            //Inserts Record to Sample Tracking Database - TestStatus Table 10/10/2017
            GISClass.Samples.AddTestStatus(nLogNo);

            //MessageBox.Show("Special batch login successfully saved.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            int intOpen = GISClass.General.OpenForm(typeof(SamplesLogin));

            if (intOpen == 1)
            {
                GISClass.General.CloseForm(typeof(SamplesLogin));
            }
            IngredionManifestLog childForm = new IngredionManifestLog();
            childForm.MdiParent = this.MdiParent;
            childForm.Text = "INGREDION LOGINS";
            childForm.nLogNo = nLogNo;
            childForm.nFR = 1;
            childForm.Show();
            this.Dispose();
        }

        private void btnCancelPrint_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtSSFormNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                DataTable dtX = GISClass.Samples.SSFLogMaster(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX == null || dtX.Rows.Count == 0)
                {
                    MessageBox.Show("No matching SSF number", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cboSponsors.Text = dtX.Rows[0]["SponsorName"].ToString();
                cboSponsorID.Text = dtX.Rows[0]["SponsorID"].ToString();
                cboContacts.SelectedValue = dtX.Rows[0]["ContactID"];
                cboFillCodes.SelectedValue = dtX.Rows[0]["ArticleDesc"].ToString();
                
                //txtContact.Text = GISClass.Contacts.ConName(Convert.ToInt16(txtContactID.Text), Convert.ToInt16(txtSponsorID.Text));
                DataTable dtPONo = GISClass.PO.PODDL(Convert.ToInt16(cboSponsorID.Text));
                if (dtPONo != null && dtPONo.Rows.Count > 0)
                {
                    txtPONo.Text = dtPONo.Rows[0]["PONo"].ToString();
                }
                dtX = null;
                dtX = GISClass.Samples.SSFLogSamples(Convert.ToInt32(txtSSFormNo.Text));
                if (dtX != null && dtX.Rows.Count > 0)
                {
                    txtBags.Text = Convert.ToInt16(dtX.Rows[0]["SampleQty"]).ToString();
                    txtCompBag.Text = "1(" + GISClass.General.CompositeEntry(Convert.ToInt16(txtBags.Text)) + ")";
                    txtLotNo.Text = dtX.Rows[0]["LotNo"].ToString();
                    txtBookNo.Text = "828"; //to be added in the settings
                }
                dtX.Dispose();
            }
        }
    }
}
