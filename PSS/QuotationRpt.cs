using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.IO;

namespace PSS
{
    public partial class QuotationRpt : Form
    {
        public string CmpyCode;
        public string QuoteNo;
        public int RevNo;
        public Int16 pubSpID;
        public int nQ;
        public byte nOld = 0;
        public byte nP;
        public string strHT = "0";
        public byte nGLP = 0;

        private ReportDocument crDoc;
        
        public QuotationRpt()
        {
            InitializeComponent();
        }

        private void QuotationRpt_Load(object sender, EventArgs e)
        {
            //Check for Sterikits SCs 
            string strSkit = "0";
            DataTable dtSKit = new DataTable();
            dtSKit = PSSClass.Quotations.QuoteSterikits(CmpyCode, QuoteNo, RevNo);
            if (dtSKit != null && dtSKit.Rows.Count > 0)
            {
                strSkit = "1";
                dtSKit.Dispose();
            }
            //Check for Sterilization SCs 
            DataTable dtSter = new DataTable();
            dtSter = PSSClass.Quotations.QuoteSterilization(CmpyCode, QuoteNo, RevNo);
            if (dtSter != null && dtSter.Rows.Count > 0)
            {
                strSkit = "2";
                dtSter.Dispose();
            }
            //Check for ONLINE SSF users 
            DataTable dtOSSF = new DataTable();
            dtOSSF = PSSClass.Quotations.QuoteOnlineSSF(CmpyCode, QuoteNo, RevNo);
            if (dtOSSF != null && dtOSSF.Rows.Count > 0 && strSkit == "0")
            {
                strSkit = "3";
                dtOSSF.Dispose();
            }
            //Check for new Sponsor
            DataTable dtNewSp = new DataTable();
            dtNewSp = PSSClass.Quotations.QuoteNewSp(pubSpID);
            if (dtNewSp == null || dtNewSp.Rows.Count == 0 && strSkit =="0")
            {
                strSkit = "3";
                dtNewSp.Dispose();
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

            if (nOld == 1)
            {
                if (PSSClass.Quotations.QuoteCols(QuoteNo, RevNo) == 2)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "Quotation.rpt";
                else if (PSSClass.Quotations.QuoteCols(QuoteNo, RevNo) == 3)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "QuotationExt.rpt";
                else if (PSSClass.Quotations.QuoteCols(QuoteNo, RevNo) == 4)
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "QuotationExt4.rpt";
            }
            else
            {
            if (PSSClass.Quotations.QuotePriceType(CmpyCode, QuoteNo, RevNo) == 1)
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "QuotationReg.rpt";
            else if (PSSClass.Quotations.QuotePriceType(CmpyCode, QuoteNo, RevNo) == 2)
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "QuotationRegRush.rpt";
            else if (PSSClass.Quotations.QuotePriceType(CmpyCode, QuoteNo, RevNo) == 3)
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "QuotationRush.rpt";
            }
            crDoc.Load(rpt);
            sqlcmd = new SqlCommand("spQuotationRpt", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@CmpyCode", CmpyCode);
            sqlcmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
            sqlcmd.Parameters.AddWithValue("@RevNo", RevNo);


            sqldr = sqlcmd.ExecuteReader();

            DataTable dTable = new DataTable();

            try
            {
                dTable.Load(sqldr);
                sqlcnn.Close(); sqlcnn.Dispose();
            }
            catch
            {
                sqlcnn.Close();  sqlcnn.Dispose();
            }
            crDoc.SetDataSource(dTable);
            crDoc.DataDefinition.FormulaFields["cSterikit"].Text = "'" + strSkit + "'";
            crDoc.DataDefinition.FormulaFields["cCmpyCode"].Text = "'" + CmpyCode + "'";
            crReport.ReportSource = crDoc;
            crReport.Refresh();

            if (nP == 0)
            {
                crReport.ShowPrintButton = false; crReport.ShowExportButton = false;
            }
            else
            {
                crReport.ShowPrintButton = true; crReport.ShowExportButton = true;
            }
            if (nQ == 1 || nQ == 9)
            {
                crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                string strCmpy = "P";
                if (CmpyCode.Trim() != "P")
                    strCmpy = "";

                if (nQ == 1)
                {
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + QuoteNo.Substring(0, 4) + @"\" + strCmpy + QuoteNo + ".R" + RevNo.ToString().Trim() + ".pdf";// "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                }
                else
                {
                    if (File.Exists(@"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + QuoteNo.Substring(0, 4) + @"\" + strCmpy + QuoteNo + ".R" + RevNo.ToString().Trim() + ".pdf") == true)
                        File.Move(@"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + QuoteNo.Substring(0, 4) + @"\" + strCmpy + QuoteNo + ".R" + RevNo.ToString().Trim() + ".pdf",
                                  @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + QuoteNo.Substring(0, 4) + @"\" + strCmpy + QuoteNo + ".R" + RevNo.ToString().Trim() + "_DR_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + "_" +
                                                       DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".pdf");
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Quotes\" + QuoteNo.Substring(0, 4) + @"\" + strCmpy + QuoteNo + ".R" + RevNo.ToString().Trim() + ".pdf";               
                }
                crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                crDoc.Export();
            }
            dTable.Dispose(); 
        }

        private void QuotationRpt_FormClosing(object sender, FormClosingEventArgs e)
        {
            crDoc.Close(); crDoc.Dispose(); 
            try
            {
                crReport.ReportSource = null; crReport.Dispose();
            }
            catch { }
            this.Dispose();
        }
    }
}
