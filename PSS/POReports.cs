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
    public partial class POReports : Form
    {
        public string rptName;
        public string rptTitle;
        public string rptFile;
        public int nDepartmentID;
        public int nVendorID;
        public int nUsageByType;      
        public DateTime dteStart;
        public DateTime dteEnd;

        public POReports()
        {
            InitializeComponent();
        }

        private void POReports_Load(object sender, EventArgs e)
        {
            CreateReport();
        }

        private void CreateReport()
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ReportDocument crDoc = new ReportDocument();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            string rpt = "";

            if (nUsageByType == 1)
            {
                rpt = @"\\gblnj4\GIS\Reports\" + "POUsageByDepartment.rpt";
            }
            else
            {
                rpt = @"\\gblnj4\GIS\Reports\" + "POUsageByVendor.rpt";              
            }            

            crDoc.Load(rpt);
            crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
            crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
            sqlcmd = new SqlCommand("spRptPOUsage", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@DepartmentID", nDepartmentID);
            sqlcmd.Parameters.AddWithValue("@VendorID", nVendorID);
            sqlcmd.Parameters.AddWithValue("@UsagebyType", nUsageByType);
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
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
