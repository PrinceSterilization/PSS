using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSS
{
    public partial class HRReports : Form
    {
        private int nTimer = 0, nRNo = 1;

        public HRReports()
        {
            InitializeComponent();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            nTimer = 0; timer1.Enabled = true; lblProgress.Visible = true;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (nTimer == 0)
            {
                nTimer = 1;
                timer1.Enabled = false;
                GenerateReport();
                lblProgress.Visible = false;
            }
        }

        private void GenerateReport()
        {
            HRRpt rpt = new HRRpt();
            if (nRNo == 1)
                rpt.rptName = "Current Staffing";
            else if (nRNo == 2)
                rpt.rptName = "Staffing by Date Range";
            else if (nRNo == 3)
            {
                rpt.rptName = "Yearly Staffing";
                rpt.pYear = Convert.ToInt16(cboYear.Text);
            }
            else if (nRNo == 4)
            {
                rpt.rptName = "Hired Employees";
                rpt.pYear = Convert.ToInt16(cboYear.Text);
            }
            else if (nRNo == 5)
            {
                rpt.rptName = "Former Employees";
                rpt.pYear = Convert.ToInt16(cboYear.Text);
            }
            else if (nRNo == 6)
            {
                rpt.rptName = "Employees 401K";
                rpt.pYear = Convert.ToInt16(cboYear.Text);
            }
            else if (nRNo == 7)
            {
                rpt.rptName = "Yearly Turnover";
                rpt.pYear = Convert.ToInt16(cboYear.Text);
            }
            else if (nRNo == 8)
            {
                rpt.rptName = "Employee Phone List - First Name";
            }
            else if (nRNo == 9)
            {
                rpt.rptName = "Employee Phone List - Last Name";
            }
            else if (nRNo == 10)
            {
                rpt.rptName = "Employees Birthday List";
            }
            else if (nRNo == 11)
            {
                rpt.rptName = "Employees Hired By Period";
            }
            else if (nRNo == 12)
            {
                rpt.rptName = "Employees Terminated By Period";
            }
            else if (nRNo == 13)
            {
                rpt.rptName = "Employees List By Period";
            }
            else if (nRNo == 14)
            {
                rpt.rptName = "Employees Education";
            }
            rpt.dteStart = dtpStart.Value;
            rpt.dteEnd = dtpEnd.Value;
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //}
        }

        private void rdoCurrentStaff_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 1; pnlDateRange.Visible = true; pnlYear.Visible = false;
        }

        private void rdoStaffByDate_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 2; pnlDateRange.Visible = true; pnlYear.Visible = false;
        }

        private void rdoYearlyStaff_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 3; pnlYear.Visible = true; pnlYear.Top = 6; pnlDateRange.Visible = false;
        }

        private void rdoHiredEmp_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 4; pnlYear.Visible = true; pnlYear.Top = 6; pnlDateRange.Visible = false;
        }

        private void rdoFormerEmp_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 5; pnlYear.Visible = true; pnlYear.Top = 6; pnlDateRange.Visible = false;
        }

        private void rdo401K_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 6; pnlYear.Visible = true; pnlYear.Top = 6; pnlDateRange.Visible = false;
        }

        private void rdoYrTurnover_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 7; pnlYear.Visible = true; pnlYear.Top = 6; pnlDateRange.Visible = false;
        }

        private void HRReports_Load(object sender, EventArgs e)
        {
            string dte = "1/1/1970";// +DateTime.Now.Year.ToString();
            string sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");

            string strdte = "12/31/" + DateTime.Now.Year.ToString();
            string strEdte = Convert.ToDateTime(strdte).ToString("MM/dd/yyyy");

            dtpStart.Value = Convert.ToDateTime(sdte);
            dtpEnd.Value = Convert.ToDateTime(strEdte);
            cboYear.SelectedIndex = 0;
        }

        private void rdoEmpPhoneFN_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 8; pnlYear.Visible = false; pnlDateRange.Visible = true;
        }

        private void rdoEmpPhoneLN_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 9; pnlYear.Visible = false; pnlDateRange.Visible = true;
        }

        private void rdoBirthdayList_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 10; pnlYear.Visible = false; pnlDateRange.Visible = true;
        }

        private void rdoEmpHiredByPeriod_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 11; pnlYear.Visible = false; pnlDateRange.Visible = true;
        }

        private void rdoEmpTerByPeriod_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 12; pnlYear.Visible = false; pnlDateRange.Visible = true;
        }

        private void rdoEmployeesList_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 13; pnlYear.Visible = false; pnlDateRange.Visible = true;
        }

        private void rdoEducation_CheckedChanged(object sender, EventArgs e)
        {
            nRNo = 14; pnlYear.Visible = false; pnlDateRange.Visible = true;
        }
    }
}
