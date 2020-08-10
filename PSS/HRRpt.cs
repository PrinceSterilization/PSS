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
    public partial class HRRpt : Form
    {
        public string rptName;
        public string rptTitle;
        public string rptFile;

        public DateTime dteStart;
        public DateTime dteEnd;
        public int pYear;

        public HRRpt()
        {
            InitializeComponent();
        }

        private void AcctgRpt_Load(object sender, EventArgs e)
        {
            CreateReport();
        }

        private void CreateReport()
        {
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

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            ReportDocument crDoc = new ReportDocument();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;
            if (rptName == "Current Staffing")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpCurrentStaffing.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpCurrentStaffing", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                //sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                //sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);

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
                //crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                //crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";

                crReport.ReportSource = crDoc;
                
                crReport.Refresh();
            }
            else if (rptName == "Staffing by Date Range")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpStaffingReport.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpStaffingReport", sqlcnn);
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
                crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Yearly Staffing")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpYearlyStaffing.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpYearlyStaffing", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                //sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                //sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);

                sqlcmd.Parameters.AddWithValue("@RptYear",pYear);

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
                //crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                //crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cRptYear"].Text = "'" + pYear.ToString() + "'";
                crReport.Refresh();
            }
            else if (rptName == "Hired Employees")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpHiredEmpService.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpHiredEmpService", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                //sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                //sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
                sqlcmd.Parameters.AddWithValue("@RptYear", pYear);

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
                //crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                //crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cRptYear"].Text = "'" + pYear.ToString() + "'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Former Employees")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpTerminatedEmpService.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }

                sqlcmd = new SqlCommand("spRptEmpTerminatedEmpService", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                //sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                //sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
                sqlcmd.Parameters.AddWithValue("@RptYear", pYear);
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
                //crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                //crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cRptYear"].Text = "'" + pYear.ToString() + "'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Employees 401K")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "Emp401KInfo.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmp401KInfo", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                //sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                //sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
                sqlcmd.Parameters.AddWithValue("@RptYear", pYear); //RptYear

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
                //crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                //crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cRptYear"].Text = "'" + pYear.ToString() + "'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Yearly Turnover")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpYearlyTurnover.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpYearlyTurnover", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@RptYear", pYear);
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
                crDoc.DataDefinition.FormulaFields["cRptYear"].Text = "'" + pYear.ToString() + "'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Employee Phone List - First Name")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpPhoneList.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpPhoneList", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
                sqlcmd.Parameters.AddWithValue("@SortNo", 1);

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
                crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cSortNo"].Text = "'1'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Employee Phone List - Last Name")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpPhoneList.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpPhoneList", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("@StartDate", dteStart);
                sqlcmd.Parameters.AddWithValue("@EndDate", dteEnd);
                sqlcmd.Parameters.AddWithValue("@SortNo", 2);

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
                crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cSortNo"].Text = "'2'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Employees Birthday List")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpBirthdayList.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpCurrentStaffing", sqlcnn);
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
                crReport.Refresh();
            }
            else if (rptName == "Employees Hired By Period")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpHiredByPeriod.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpHired", sqlcnn);
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
                crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Employees Terminated By Period")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpTerminated.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpTerminated", sqlcnn);
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
                crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Employees List By Period")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpListByPeriod.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpList", sqlcnn);
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
                crDoc.DataDefinition.FormulaFields["cStartDate"].Text = "'" + dteStart.ToShortDateString() + "'";
                crDoc.DataDefinition.FormulaFields["cEndDate"].Text = "'" + dteEnd.ToShortDateString() + "'";
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Employees Education")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "EmpEducation.rpt";
                crDoc.Load(rpt);
                CrTables = crDoc.Database.Tables;
                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                sqlcmd = new SqlCommand("spRptEmpEducation", sqlcnn);
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
                crReport.Refresh();
            }
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
