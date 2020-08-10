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
    public partial class ConrolPageSheetRpt : Form
    {
        public long ControlPageID;       
        public int nQ;
        public string PrinterName;

        public ConrolPageSheetRpt()
        {
            InitializeComponent();        
        }

        private void ControlPageSheetRpt_Load(object sender, EventArgs e)
        {

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            ReportDocument crDoc = new ReportDocument();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            string rpt = @"\\gblnj4\GIS\Reports\" + "ControlPageForm.rpt";
            //string rpt = @"\\GLSQL03\GIS\Reports\" + "ControlPageForm.rpt";
            //string rpt = "C:\\Maria" + @"\Dev\GIS\GIS\bin\Debug\" + "ControlPageForm.rpt";

            crDoc.Load(rpt);
            sqlcmd = new SqlCommand("spRptControPageSheet", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure; 

            sqlcmd.Parameters.AddWithValue("@ControlPageID", ControlPageID);
            
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

            crReport.ShowPrintButton = false;
            crDoc.PrintOptions.PrinterName = PrinterName;
            crDoc.PrintToPrinter(1, false, 0, 0);
            crReport.ReportSource = crDoc;
            crReport.Refresh();        
        }     

              
    }
}
