using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Drawing.Printing;

namespace PSS
{
    public partial class PurchaseOrdersRpt : Form
    {
        public string CmpyCode;
        public string PONo;
        public int DlvrTo;
        public Int16 pubPrtMode;
        public string rptName;
        public string rptFileName;
        private ReportDocument crDoc = new ReportDocument();

        public PurchaseOrdersRpt()
        {
            InitializeComponent();
        }

        private void PurchaseOrdersRpt_Load(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            if (DlvrTo == 1)
            {
                rptName = "POForm16.rpt";
            }
            else
            {
                rptName = "POForm122.rpt";
            }
            string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + rptName;


            crDoc.Load(rpt);
            CrTables = crDoc.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
            sqlcmd = new SqlCommand("spRptPOForm", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@CmpyCode", CmpyCode);
            sqlcmd.Parameters.AddWithValue("@PONo", PONo);
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
            if (pubPrtMode == 1)
                crReport.ShowPrintButton = false;
            else if (pubPrtMode == 2)
                crReport.ShowPrintButton = true;
            else if (pubPrtMode == 3)
            {
                crDoc.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDoc.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                DiskFileDestinationOptions objDiskOpt = new DiskFileDestinationOptions();
                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\PO\" + "20" + PONo.Substring(2,2) + "\\" + CmpyCode + PONo + ".pdf";
                crDoc.ExportOptions.DestinationOptions = objDiskOpt;
                crDoc.Export();
                objDiskOpt.DiskFileName = @"\\PSAPP01\IT Files\PTS\PDF Reports\PO\" + "20" + PONo.Substring(2, 2) + "\\" + CmpyCode + PONo + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") +
                                         DateTime.Now.Day.ToString("00") + "_" + DateTime.Now.TimeOfDay.Hours.ToString("00") + DateTime.Now.TimeOfDay.Minutes.ToString("00") + DateTime.Now.TimeOfDay.Seconds.ToString("00") + ".pdf";
                crDoc.Export();
            }
        }

        private void PurchaseOrdersRpt_FormClosing(object sender, FormClosingEventArgs e)
        {
            crDoc.Close(); crDoc.Dispose();
            this.Dispose();
        }     
    }
}
