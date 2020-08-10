using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSS
{
    public partial class SalesRptSettings : Form
    {
        public string rptName;
        public string rptTitle;
        public int rptScope;

        public SalesRptSettings()
        {
            InitializeComponent();
        }

        private void SalesRptSettings_Load(object sender, EventArgs e)
        {
            this.Size = new Size(442, 353);
            this.Text = rptTitle; txtYear.Text = DateTime.Now.Year.ToString();
            btnClear.Enabled = false;
            if (rptName == "QtrlyForecast.rpt")
            {
                rdoQuoteNo.Enabled = false; rdoSponsor.Checked = true;
                rdoValueAsc.Enabled = true; rdoValueDesc.Enabled = true;
            }
            else if (rptName == "CustomerYrReview.rpt")
            {
                rdoQuoteNo.Enabled = false; rdoSponsor.Checked = true;
                rdoValueAsc.Enabled = false; rdoValueDesc.Enabled = false;
            }
            else if (rptName == "QuotesRptSp.rpt")
            {
                rdoQuoteNo.Enabled = false; rdoSponsor.Checked = true;
                rdoValueAsc.Enabled = false; rdoValueDesc.Enabled = false;
            }
            else if (rptName == "QuotesRptSC.rpt")
            {
                rdoQuoteNo.Enabled = false; rdoSponsor.Enabled = false;
                rdoValueAsc.Enabled = false; rdoValueDesc.Enabled = false;
            }
            else if (rptName == "QuotesRptDept.rpt")
            {
                rdoQuoteNo.Enabled = false; rdoSponsor.Enabled = false;
                rdoValueAsc.Enabled = false; rdoValueDesc.Enabled = false;
            }
            else if (rptName == "QuotesRejected.rpt")
            {
                btnClear.Enabled = true;
                rdoQuoteNo.Enabled = false; rdoSponsor.Enabled = true; rdoQuoteNo.Checked = false;
                rdoValueAsc.Enabled = false; rdoValueDesc.Enabled = false;
            }
            else
            {
                rdoQuoteNo.Enabled = true; rdoQuoteNo.Checked = true;
                rdoValueAsc.Enabled = false; rdoValueDesc.Enabled = false;
            }
        }

        private void btnOKPrint_Click(object sender, EventArgs e)
        {
            SalesRpt rpt = new SalesRpt();
            rpt.rptTitle = rptTitle;
            rpt.rptName = rptName;

            if (txtYear.Text.Trim() == "")
                rpt.nYr = 0;
            else
                rpt.nYr = Convert.ToInt16(txtYear.Text);

            if (rdoQuoteNo.Checked == true)
                rpt.nSort = 1;
            else if (rdoSponsor.Checked == true)
               rpt.nSort = 2;
            else if (rdoValueDesc.Checked == true)
                rpt.nSort = 3;
            else if (rdoValueAsc.Checked == true)
                rpt.nSort = 4;
            rpt.nScope = rptScope;
            rpt.rptTag = rptName;
            if (rptName == "CustomerYrReview.rpt")
            {
                int nYr = Convert.ToInt16(txtYear.Text);
                int nRange = nYr - 4;
                int nRYr = nYr - 4;
                DataTable dtSummary = new DataTable();
                dtSummary.Columns.Add("Yr", typeof(Int16));
                dtSummary.Columns.Add("NewCustCount", typeof(Int16));
                dtSummary.Columns.Add("NewCustRev", typeof(decimal));
                dtSummary.Columns.Add("LostCustCount", typeof(Int16));
                dtSummary.Columns.Add("LostCustRev", typeof(decimal));

                DataTable dtX = new DataTable();

                for (int i = 0; i < 5; i++)
                {
                    dtX = PSSClass.ManagementReports.FirstNewCustCount(Convert.ToInt16(nRYr));
                    if (dtX != null && dtX.Rows.Count > 0)
                    {
                        DataRow dR = dtSummary.NewRow();
                        dR["Yr"] = nRYr;
                        dR["NewCustCount"] = dtX.Rows[0]["TotCount"];

                        dtX = PSSClass.ManagementReports.FirstNewCustRev(Convert.ToInt16(nRYr));
                        if (dtX != null && dtX.Rows.Count > 0)
                        {
                            dR["NewCustRev"] = dtX.Rows[0]["Rev"];
                        }

                        dtX = PSSClass.ManagementReports.LostCustCount(Convert.ToInt16(nRYr));
                        if (dtX != null && dtX.Rows.Count > 0)
                        {
                            dR["LostCustCount"] = dtX.Rows[0]["TotCount"];
                        }

                        dtX = PSSClass.ManagementReports.LostCustRev(Convert.ToInt16(nRYr));
                        if (dtX != null && dtX.Rows.Count > 0)
                        {
                            dR["LostCustRev"] = dtX.Rows[0]["Rev"];
                        }
                        dtSummary.Rows.Add(dR);
                    }
                    nRYr += 1;
                }
                rpt.dtSales = dtSummary;
            }
            rpt.Show();
        }

        private void btnCancelPrint_Click(object sender, EventArgs e)
        {
            this.Close(); this.Dispose();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            rdoQuoteNo.Checked = false; rdoSponsor.Checked = false; rdoValueAsc.Checked = false; rdoValueDesc.Checked = false;
        }
    }
}
