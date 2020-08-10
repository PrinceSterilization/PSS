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

namespace GIS
{
    public partial class StabilityReport : Form
    {
        public int nDepartmentID;
        public int nSponsorID;
        public DateTime dteStart;
        public DateTime dteEnd;

        public StabilityReport()
        {
            InitializeComponent();
        }

        private void StabilityReport_Load(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact your administrator.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ReportDocument crDoc = new ReportDocument();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            string rpt = @"\\gblnj4\GIS\Reports\StabilityReport.rpt";

            crDoc.Load(rpt);
            crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
            crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
            sqlcmd = new SqlCommand("spRptStabilityReport", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@DepartmentID", nDepartmentID);
            sqlcmd.Parameters.AddWithValue("@SponsorID", nSponsorID);
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
            crReport.Refresh();
        }
    }
}
