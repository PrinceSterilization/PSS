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
    public partial class QuoteFollowUpSheet : Form
    {
        public int CutOffDays;    

        public QuoteFollowUpSheet()
        {
            InitializeComponent();
        }

        private void QuoteFollowUpSheet_Load(object sender, EventArgs e)
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

            string rpt = @"\\gblnj4\GIS\Reports\" + "QuoteFollowUpList.rpt";
            //string rpt = "C:\\Maria" + @"\Dev\GIS\GIS\bin\Debug\" + "QuoteFollowUpList.rpt";

            crDoc.Load(rpt);
            sqlcmd = new SqlCommand("spQuoteFollowUp", sqlcnn);
            sqlcmd.CommandType = CommandType.StoredProcedure;

            sqlcmd.Parameters.AddWithValue("@CutOffDays", CutOffDays);

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
