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

namespace PSS
{
    public partial class AcctgRpt : Form
    {
        public string rptName;
        public string rptTitle;
        public string rptFile;
        public int nSpID;
        public string strPO;
        public int nPOBType;

        public string CmpyCode;
        public string QuoteNo;
        public int RevNo;
        public int nQ;
        public int nInvNo;
        public DateTime dteStart;
        public DateTime dteEnd;
        public Int16 nConID;

        private ReportDocument crDoc;

        public AcctgRpt()
        {
            InitializeComponent();
        }

        private void AcctgRpt_Load(object sender, EventArgs e)
        {
            CreateReport();
        }

        private void CreateReport()
        {

            crDoc = new ReportDocument();
            
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

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
            string rpt = "";

            if (rptName == "DraftPrepayInv")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "Prepayment.rpt";

                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spPrepayments", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@CmpyCode", CmpyCode);
                sqlcmd.Parameters.AddWithValue("@QuoteNo", QuoteNo);
                sqlcmd.Parameters.AddWithValue("@RevNo", RevNo);

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
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "PrepayInvoice")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "PrepayInvoice.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spPrepayInvoice", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@CmpyCode", CmpyCode);
                sqlcmd.Parameters.AddWithValue("@InvNo", nInvNo);

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
                crReport.ReportSource = crDoc;
                if (nQ == 1) //Preview
                {
                    crReport.Refresh();
                }
                else if (nQ == 2) //Direct Print
                {
                    crDoc.PrintToPrinter(1, true, 0, 0);
                    crReport.Visible = false;
                    this.Dispose();
                }
                else if (nQ == 3)
                {
                    crReport.Refresh();
                    crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Invoices\" + DateTime.Now.Year.ToString() + "\\" + nInvNo.ToString("0000000") + ".pdf";
                    crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                    crDoc.Export();
                    this.Dispose();
                }
            }
            else if (rptName == "Invoice" || rptName == "InvoiceIngredion" || rptName == "DraftInvoiceMfst" || rptName == "InvoiceFHI" || rptName == "InvoiceOthers")
            {
                //decimal nPP = 0;
                if (rptName == "Invoice")
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "Invoice.rpt";
                //else if (rptName == "InvoiceFHI")
                //    rpt = @"\\gblnj4\GIS\Reports\" + "InvoiceFHI.rpt";
                //else if (rptName == "InvoiceIngredion")
                //    rpt = @"\\gblnj4\GIS\Reports\" + "IngredionInvMfst.rpt";
                //else if (rptName == "DraftInvoiceMfst")
                //    rpt = @"\\gblnj4\GIS\Reports\" + "IngredionInvMfstX.rpt";
                else if (rptName == "InvoiceOthers")
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "InvoiceOthers.rpt";

                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                if (rptName == "DraftInvoiceMfst")
                    sqlcmd = new SqlCommand("spInvoiceX", sqlcnn);
                else if (rptName == "InvoiceFHI")
                    sqlcmd = new SqlCommand("spFHITemplate", sqlcnn);
                else if (rptName == "InvoiceOthers")
                    sqlcmd = new SqlCommand("spInvoiceOthers", sqlcnn);
                else
                    sqlcmd = new SqlCommand("spInvoice", sqlcnn);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@InvNo", nInvNo);
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

                //if (rptName == "Invoice")
                //{
                //    crDoc.SetParameterValue("@InvNo", nInvNo, "InvOtherFees");
                //}
                crReport.ReportSource = crDoc;

                if (nQ == 1) //Preview
                {
                    crReport.Refresh();
                }
                else if (nQ == 2) //Direct Print
                {
                    //Open the PrintDialog
                    this.printDialog1.Document = this.printDocument1;
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

                        try
                        {
                            //Set the printer name to print the report to. By default the sample
                            //report does not have a defult printer specified. This will tell the
                            //engine to use the specified printer to print the report. Print out 
                            //a test page (from Printer properties) to get the correct value.

                            crDoc.PrintOptions.PrinterName = PrinterName;


                            //Start the printing process. Provide details of the print job
                            //using the arguments.
                            crDoc.PrintToPrinter(nCopy, false, sPage, ePage);
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.ToString());
                        }
                    }
                    crReport.Visible = false;
                    this.Dispose();
                }
                else if (nQ == 3) //email
                {
                    crReport.Refresh();
                    crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Invoices\" + DateTime.Now.Year.ToString()+ @"\I-" + nInvNo.ToString("0000000") + ".pdf";
                    crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                    crDoc.Export();
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\Invoices\" + DateTime.Now.Year.ToString() + @"\I-" + nInvNo.ToString("0000000") + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                          DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                    crDoc.Export();
                    this.Dispose();
                }
            }
            else if (rptName == "InvoiceIngredion")
            {
                decimal nPP = 0;
                string strQ = "", strQNo = "", strRNo = "";
                int nI = 0;

                DataTable dt = PSSClass.FinalBilling.LoadInvServiceFees(nInvNo);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strQNo = dt.Rows[i]["QuoteNo"].ToString();
                        nI = strQNo.IndexOf("R");
                        strQ = strQNo.Substring(0, nI - 1);
                        strRNo = strQNo.Substring(nI + 1, strQNo.Length - (nI + 1));

                        DataTable dtPP = PSSClass.FinalBilling.InvPrepay(dt.Rows[i]["CompanyCode"].ToString(), strQ, Convert.ToInt16(strRNo));
                        if (dtPP != null && dtPP.Rows.Count != 0)
                        {
                            for (int j = 0; j < dtPP.Rows.Count; j++)
                            {
                                nPP = nPP + Convert.ToDecimal(dtPP.Rows[j]["AmtDue"]);
                            }
                        }
                    }
                }
                //string rpt = @"\\gblnj4\GIS\Reports\" + "Invoice.rpt";
                rpt = @"\\gblnj4\GIS\Reports\" + "IngredionInvMfstX.rpt";
                crDoc.Load(rpt);
                //crDoc.DataDefinition.FormulaFields["nPP"].Text = "'" + nPP.ToString() + "'";
                crDoc.DataDefinition.FormulaFields["nPP"].Text = "'0'";

                sqlcmd = new SqlCommand("spInvoiceX", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@InvNo", nInvNo);

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
                crReport.ReportSource = crDoc;

                if (nQ == 1) //Preview
                {
                    crReport.Refresh();
                }
                else if (nQ == 2) //Direct Print
                {
                    //Open the PrintDialog
                    this.printDialog1.Document = this.printDocument1;
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

                        try
                        {
                            //Set the printer name to print the report to. By default the sample
                            //report does not have a defult printer specified. This will tell the
                            //engine to use the specified printer to print the report. Print out 
                            //a test page (from Printer properties) to get the correct value.

                            crDoc.PrintOptions.PrinterName = PrinterName;


                            //Start the printing process. Provide details of the print job
                            //using the arguments.
                            crDoc.PrintToPrinter(nCopy, false, sPage, ePage);
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.ToString());
                        }
                    }
                    crReport.Visible = false;
                    this.Dispose();
                }
            }
            else if (rptName == "RptForInvoice.rpt")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "RptForInvoice.rpt";
                crDoc.Load(rpt);
                sqlcmd = new SqlCommand("spRptForInvoice", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                //sqlcmd.Parameters.AddWithValue("@InvNo", nInvNo);

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
                crReport.ReportSource = crDoc;
                if (nQ == 1) //Preview
                {
                    crReport.Refresh();
                }
                else if (nQ == 2) //Direct Print
                {
                    crDoc.PrintToPrinter(1, true, 0, 0);
                    crReport.Visible = false;
                    this.Dispose();
                }
                //else if (nQ == 3)
                //{
                //    crReport.Refresh();
                //    crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                //    objDiskOpt.DiskFileName = @"\\gblnj4\GIS\Reports\I-" + nInvNo.ToString() + ".pdf";
                //    crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                //    crDoc.Export();
                //    this.Dispose();
                //}
            }
            else if (rptName == "POBalance")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "POBalance.rpt";
                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spPOBalance", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@SpID", nSpID);
                sqlcmd.Parameters.AddWithValue("@PONo", strPO);
                sqlcmd.Parameters.AddWithValue("@POBType", nPOBType);

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
                crDoc.DataDefinition.FormulaFields["cPOBType"].Text = "'" + nPOBType.ToString() + "'";
                crDoc.SetDataSource(dTable);
                crReport.ReportSource = crDoc;
                if (nQ == 1) //Preview
                {
                    crReport.Refresh();
                }
                else if (nQ == 2) //Direct Print
                {
                    crDoc.PrintToPrinter(1, true, 0, 0);
                    crReport.Visible = false;
                    this.Dispose();
                }
                //else if (nQ == 3)
                //{
                //    crReport.Refresh();
                //    crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //    crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //    DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                //    objDiskOpt.DiskFileName = @"\\gblnj4\GIS\Reports\I-" + nInvNo.ToString() + ".pdf";
                //    crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                //    crDoc.Export();
                //    this.Dispose();
                //}
            }
            else if (rptName == "POBalanceSum")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "POBalanceSum.rpt";
                crDoc.Load(rpt);

                sqlcmd = new SqlCommand("spPOBalanceSum", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

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
                crReport.ReportSource = crDoc;
                if (nQ == 1) //Preview
                {
                    crReport.Refresh();
                }
                else if (nQ == 2) //Direct Print
                {
                    crDoc.PrintToPrinter(1, true, 0, 0);
                    crReport.Visible = false;
                    this.Dispose();
                }
            }

            else if (rptName == "SOA")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "SOA.rpt";
                crDoc.Load(rpt);
                sqlcmd = new SqlCommand("spSOA", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@CustomerID", nSpID.ToString());

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
                crReport.ReportSource = crDoc;
                if (nQ == 1) //Preview
                {
                    crReport.Refresh();
                }
                else if (nQ == 2) //Direct Print
                {
                    crDoc.PrintToPrinter(1, true, 0, 0);
                    crReport.Visible = false;
                    this.Dispose();
                }
                else if (nQ == 3)
                {
                    crReport.Refresh();
                    crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\SOA\" + DateTime.Now.Year.ToString() + @"\SOA-" + nSpID.ToString() + ".pdf";
                    crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                    crDoc.Export();
                    objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\SOA\" + DateTime.Now.Year.ToString() + @"\SOA-" + nSpID.ToString() + "-" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                      DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                    crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                    crDoc.Export();
                }
            }
            else if (rptName == "Comparative Revenue Report" || rptName == "Comparative Revenue Report - Grouped by Sponsor")
            {
                if (rptName == "Comparative Revenue Report")
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "InvoiceComparativeReport.rpt";
                else
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "InvoiceComparativeReport-BySponsor.rpt";
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptInvoiceComparativeReport", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
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
                crReport.ReportSource = crDoc;
                crReport.ShowGroupTreeButton = false;
                crReport.Refresh();
            }
            else if (rptName == "TTM")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "TTM.rpt";
                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'FOR THE 12-MONTH PERIOD: " + dteStart.ToShortDateString() + " - " + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptMngtTTM", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
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
                crReport.ReportSource = crDoc;
                crReport.ShowGroupTreeButton = false;
                crReport.Refresh();
            }
            else if (rptName == "Exceptions List")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AdjustmentsList.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptAdjustmentsList", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
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
                crReport.ReportSource = crDoc;
                crReport.ShowGroupTreeButton = false;
                crReport.Refresh();
            }
            else if (rptName == "Invoices To Be Posted" || rptName == "Invoices To Be Posted - Grouped by Sponsor" || rptName == "Invoices To Be Posted - Sorted by Sponsor ASC")
            {
                if (rptName == "Invoices To Be Posted")
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "InvoicesToBePosted.rpt";
                else if (rptName == "Invoices To Be Posted - Grouped by Sponsor")
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "InvoicesToBePosted_GrpBySponsor.rpt";
                else if (rptName == "Invoices To Be Posted - Sorted by Sponsor ASC")
                    rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "InvoicesToBePostedBySponsorAsc.rpt";

                crDoc.Load(rpt);
                crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                sqlcmd = new SqlCommand("spRptInvoiceComparativeReport", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
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
                crReport.ReportSource = crDoc;
                crReport.ShowGroupTreeButton = false;
                crReport.Refresh();
                dTable.Dispose();
            }
            else if (rptName == "TemporaryInvoice")
            {
                rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "InvoiceTemp.rpt";

                crDoc.Load(rpt);
                sqlcmd = new SqlCommand("spInvoiceTemp", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@PONo", strPO);
                sqlcmd.Parameters.AddWithValue("@ConID", nConID);
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
                crReport.ReportSource = crDoc;
                crReport.ShowGroupTreeButton = false;
                crReport.Refresh();
            }
            this.WindowState = FormWindowState.Maximized;
        }

        private void AcctgRpt_FormClosing(object sender, FormClosingEventArgs e)
        {
            crDoc.Close(); crDoc.Dispose();
            try
            {
                crReport.ReportSource = null; crReport.Dispose();
            }
            catch { }
        }
    }
}
