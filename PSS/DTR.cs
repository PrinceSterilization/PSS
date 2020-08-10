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
    public partial class DTR : Form
    {
        DataTable dtDTR = new DataTable();
        private byte nSw = 0;
        private int nCtr = 0;

        public DTR()
        {
            InitializeComponent();
        }

        private void DTR_Load(object sender, EventArgs e)
        {
            dtDTR = PSSClass.DTR.EmpDTR();
            if (dtDTR == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            bsDTR.DataSource = dtDTR;
            bnDTR.BindingSource = bsDTR;
            dtrDTR.DataSource = bsDTR;
            LoadLog(DateTime.Now);

            dtrDTR.CurrentItemIndex = 0;
            dtrDTR.ScrollItemIntoView(0, true);
            dtrDTR.Select();
            dtrDTR.Focus();
        }

        private void LoadLog(DateTime cDLog)
        {
            DataTable dt = new DataTable();
            dt = PSSClass.DTR.DateLog(cDLog);
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            bsDateLog.DataSource = dt;
            dgvLog.DataSource = bsDateLog;
            dgvLog.EnableHeadersVisualStyles = false;
            dgvLog.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLog.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvLog.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLog.Columns["EmpCode"].HeaderText = "EMP. NO.";
            dgvLog.Columns["EmpCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLog.Columns["EmpCode"].Width = 55;
            dgvLog.Columns["EmpName"].HeaderText = "EMPLOYEE NAME";
            dgvLog.Columns["EmpName"].Width = 130;
            dgvLog.Columns["LIn"].HeaderText = "TIME IN";
            dgvLog.Columns["LIn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLog.Columns["LIn"].Width = 70;
            dgvLog.Columns["Lin"].DefaultCellStyle.Format = "hh:mm tt";
            dgvLog.Columns["LBrkOut"].HeaderText = "LUNCH OUT";
            dgvLog.Columns["LBrkOut"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLog.Columns["LBrkOut"].Width = 70;
            dgvLog.Columns["LBrkOut"].DefaultCellStyle.Format = "hh:mm tt";
            dgvLog.Columns["LBrkIn"].HeaderText = "TIME IN FR. LUNCH";
            dgvLog.Columns["LBrkIn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLog.Columns["LBrkIn"].Width = 70;
            dgvLog.Columns["LBrkIn"].DefaultCellStyle.Format = "hh:mm tt";
            dgvLog.Columns["LOut"].HeaderText = "TIME OUT";
            dgvLog.Columns["Lout"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvLog.Columns["LOut"].Width = 70;
            dgvLog.Columns["LOut"].DefaultCellStyle.Format = "hh:mm tt";
        }

        private void dtrDTR_DrawItem(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemEventArgs e)
        {
            var lbl = (Label)e.DataRepeaterItem.Controls.Find("lblEmpID", false)[0];
            lbl.Text = dtDTR.Rows[e.DataRepeaterItem.ItemIndex]["EmployeeID"].ToString();

            var lblLogIn = (Label)e.DataRepeaterItem.Controls.Find("lblLogInName", false)[0];
            lblLogIn.Text = dtDTR.Rows[e.DataRepeaterItem.ItemIndex]["LogInName"].ToString();
            try
            {
                var pic = (PictureBox)e.DataRepeaterItem.Controls.Find("picPhoto", false)[0];
                pic.Load(@"\\PSAPP01\IT Files\PTS\Images\HR\" + lblLogIn.Text + ".jpg");
            }
            catch 
            {
                var pic = (PictureBox)e.DataRepeaterItem.Controls.Find("picPhoto", false)[0];
                pic.Load(@"\\PSAPP01\IT Files\PTS\Images\PSS Logo.png");
            }

            DataTable dt = new DataTable();
            dgvDTR.DataSource = null;
            dt = PSSClass.DTR.EmpLog(Convert.ToInt16(lbl.Text));
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            try
            {
                var dgv = (DataGridView)e.DataRepeaterItem.Controls.Find("dgvDTR", false)[0];
                dgv.DataSource = dt;
                dgv.Columns[0].Width = 75;
                dgv.Columns[1].Width = 110;
                dgv.Columns[1].DefaultCellStyle.Format = "hh:mm tt";
                dgv.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[2].Width = 110;
                dgv.Columns[2].DefaultCellStyle.Format = "hh:mm tt";
                dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToLongTimeString();
            nCtr++;
            if (nCtr == 5)
            {
                nCtr = 0; //lblPress.Visible = true; 
            }
            else if (nCtr == 180)
            {
                LoadLog(DateTime.Now);
                nCtr = 0;
            }
            nSw = 1;
        }

        private void DTR_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                nCtr = 0;
                txtEmpNo.Focus(); //lblPress.Visible = false;
                dtpLogDate.Value = DateTime.Now;
            }
        }

        private void txtEmpNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) == false && e.KeyChar != 8 && e.KeyChar != 13)
            {
                e.Handled = true;
                return;
            }
            else if (e.KeyChar == (char)Keys.Enter) 
            {
                int foundIndex;
                string searchString;
                searchString = txtEmpNo.Text;
                foundIndex = bsDTR.Find("EmployeeCode", searchString.Trim());
                if (foundIndex > -1)
                {
                    dtrDTR.CurrentItemIndex = foundIndex;
                    dtrDTR.ScrollItemIntoView(foundIndex, true);
                }
                else
                {
                    MessageBox.Show("Employee no. " + searchString.Trim() + " not found.");
                    txtEmpNo.Text = "";
                    nCtr = 0;
                    return;
                }

                if (((Label)dtrDTR.CurrentItem.Controls["lblEmpID"]).Text != LogIn.nUserID.ToString())
                {
                    MessageBox.Show("You are not authorized to login/out this employee!",Application.ProductName,MessageBoxButtons.OK);
                    txtEmpNo.Text = "";
                    return;
                }

                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problems. Please contact your system administrator.");
                    return;
                }
                ;
                SqlCommand sqlcmd = new SqlCommand("spAddLog", sqlcnn);
                sqlcmd.CommandType = CommandType.StoredProcedure;

                sqlcmd.Parameters.AddWithValue("EmpID", ((Label)dtrDTR.CurrentItem.Controls["lblEmpID"]).Text);
                sqlcmd.Parameters.AddWithValue("LDate", DateTime.Now.ToShortDateString());
                sqlcmd.Parameters.AddWithValue("LogTime", DateTime.Now);
                if (rdoTimeIn.Checked == true)
                    sqlcmd.Parameters.AddWithValue("LogType", "A");
                else if (rdoLunchOut.Checked == true)
                    sqlcmd.Parameters.AddWithValue("LogType", "B");
                else if (rdoBackfrLunch.Checked == true)
                    sqlcmd.Parameters.AddWithValue("LogType", "C");
                else if (rdoTimeOut.Checked == true)
                    sqlcmd.Parameters.AddWithValue("LogType", "D");
                sqlcmd.Parameters.AddWithValue("UserID", LogIn.nUserID);
                try
                {
                    SqlDataReader sqldr = sqlcmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName);
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                txtEmpNo.Text = "";
                LoadLog(DateTime.Now);
                dtrDTR.BeginResetItemTemplate();
                dtrDTR.EndResetItemTemplate();
                rdoTimeIn.Checked = false; rdoLunchOut.Checked = false; rdoBackfrLunch.Checked = false; rdoTimeOut.Checked = false;
            }
        }

        private void dtpLogDate_ValueChanged(object sender, EventArgs e)
        {
            LoadLog(dtpLogDate.Value);
            nCtr = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (cboReports.Text == "")
            {
                MessageBox.Show("Please select report name.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cboReports.Text != "Payroll Time Attendance" && cboReports.Text != "Daily Time Attendance")
            {
                if (dtpFrom.Value > dtpTo.Value)
                {
                    MessageBox.Show("Invalid date range.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            pnlPeriod.Visible = false; pnlPayroll.Visible = false;
            DTRRpt rptTA = new DTRRpt();
            rptTA.WindowState = FormWindowState.Maximized;
            if (cboReports.Text == "Daily Time Attendance")
            {
                rptTA.rptName = "DTA";
                rptTA.rptLabel = "DAILY TIME ATTENDANCE";
                rptTA.rptLogDate = dtpLogDate.Value.ToShortDateString();
            }
            else if (cboReports.Text == "Periodic Time Attendance")
            {
                rptTA.rptName = "PTA";
                rptTA.rptLabel = "PERIODIC TIME ATTENDANCE";
                rptTA.rptLogDate = "PERIOD: " + dtpFrom.Value.ToShortDateString() + " - " + dtpTo.Value.ToShortDateString();
                rptTA.rptDateFrom = dtpFrom.Value.ToShortDateString();
                rptTA.rptDateTo = dtpTo.Value.ToShortDateString();
            }
            else if (cboReports.Text == "Time Attendance Summary")
            {
                rptTA.rptName = "PTASummary";
                rptTA.rptLabel = "PERIODIC TIME ATTENDANCE SUMMARY";
                rptTA.rptLogDate = "PERIOD: " + dtpFrom.Value.ToShortDateString() + " - " + dtpTo.Value.ToShortDateString();
                rptTA.rptDateFrom = dtpFrom.Value.ToShortDateString();
                rptTA.rptDateTo = dtpTo.Value.ToShortDateString();
            }
            else if (cboReports.Text == "Employee Time Attendance")
            {
                string strEmpID = ((Label)dtrDTR.CurrentItem.Controls["lblEmpID"]).Text;
                rptTA.rptName = "EmpPTA";
                rptTA.rptEmpID = Convert.ToInt16(strEmpID);
                rptTA.rptLabel = "EMPLOYEE TIME ATTENDANCE HISTORY";
                rptTA.rptLogDate = "PERIOD: " + dtpFrom.Value.ToShortDateString() + " - " + dtpTo.Value.ToShortDateString();
                rptTA.rptDateFrom = dtpFrom.Value.ToShortDateString();
                rptTA.rptDateTo = dtpTo.Value.ToShortDateString();
            }
            else if (cboReports.Text == "Payroll Time Attendance")
            {
                btnOKPayroll_Click(null, null);
                return;
            }
            rptTA.Show();
        }

        private void cboReports_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cboReports.Text != "Payroll Time Attendance" && cboReports.Text != "Daily Time Attendance")
            {
                pnlPeriod.Visible = true; pnlPeriod.BringToFront(); pnlPayroll.Visible = false;
            }
            else if (cboReports.Text == "Payroll Time Attendance")
            {
                pnlPayroll.Visible = true; pnlPayroll.BringToFront(); pnlPeriod.Visible = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            btnPrint.Visible = false;
            btnPrint_Click(null, null);
            btnPrint.Visible = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlPeriod.Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLog(DateTime.Now);
            dtpLogDate.Value = DateTime.Now;
            nCtr = 0;
        }

        private void btnCancelPayroll_Click(object sender, EventArgs e)
        {
            pnlPayroll.Visible = false;
        }

        private void btnOKPayroll_Click(object sender, EventArgs e)
        {

            if (dtpCODF1.Value > dtpCODT1.Value)
            {
                MessageBox.Show("Invalid date range.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtpCODF1.Select();
                return;
            }
            if (dtpCODF2.Value > dtpCODT2.Value)
            {
                MessageBox.Show("Invalid date range.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dtpCODF2.Select();
                return;
            }
            pnlPayroll.Visible = false;
            DTRRpt rptTA = new DTRRpt();
            rptTA.WindowState = FormWindowState.Maximized;
            if (cboReports.Text == "Payroll Time Attendance")
            {
                rptTA.rptName = "PayrollTA";
                rptTA.rptLabel = "PAYROLL TIME ATTENDANCE";
                rptTA.rptLogDate = "PAY PERIOD: " + dtpCODF1.Value.ToShortDateString() + " - " + dtpCODT2.Value.ToShortDateString();
                rptTA.rptDateFrom = dtpCODF1.Value.ToShortDateString();
                rptTA.rptDateTo = dtpCODT1.Value.ToShortDateString();
                rptTA.rptDateFrom2 = dtpCODF2.Value.ToShortDateString();
                rptTA.rptDateTo2 = dtpCODT2.Value.ToShortDateString();
            }
            rptTA.Show();
        }

        private void dgvLog_CurrentCellChanged(object sender, EventArgs e)
        {
            if (nSw == 1)
            {
                try
                {
                    int foundIndex;
                    string searchString;
                    searchString = dgvLog.Rows[dgvLog.CurrentCell.RowIndex].Cells["EmpCode"].Value.ToString();
                    foundIndex = bsDTR.Find("EmployeeCode", searchString.Trim());
                    if (foundIndex > -1)
                    {
                        dtrDTR.CurrentItemIndex = foundIndex;
                        dtrDTR.ScrollItemIntoView(foundIndex, true);
                    }
                }
                catch
                { }
            }
        }

        private void dtrDTR_ItemValueNeeded(object sender, Microsoft.VisualBasic.PowerPacks.DataRepeaterItemValueEventArgs e)
        {
            if (e.ItemIndex < dtDTR.Rows.Count)
            {
                switch (e.Control.Name)
                {
                    case "lblEmpID":
                        try
                        {
                            e.Value = dtDTR.Rows[e.ItemIndex]["EmployeeID"].ToString();
                        }
                        catch { }
                        break;
                    case "lblEmpNo":
                        try
                        {
                            e.Value = dtDTR.Rows[e.ItemIndex]["EmployeeCode"].ToString();
                        }
                        catch { }
                        break;
                    case "lblLoginName":
                        try
                        {
                            e.Value = dtDTR.Rows[e.ItemIndex]["LoginName"].ToString();
                        }
                        catch { }
                        break;
                    case "lblEmpName":
                        try
                        {
                            e.Value = dtDTR.Rows[e.ItemIndex]["EmpName"].ToString();
                        }
                        catch { }
                        break;
                    case "lblDepartment":
                        try
                        {
                            e.Value = dtDTR.Rows[e.ItemIndex]["DepartmentName"].ToString();
                        }
                        catch { }
                        break;
                    case "lblJobTitle":
                        try
                        {
                            e.Value = dtDTR.Rows[e.ItemIndex]["JobTitle"].ToString();
                        }
                        catch { }
                        break;
                    case "lblDateHired":
                        try
                        {
                            e.Value = Convert.ToDateTime(dtDTR.Rows[e.ItemIndex]["HireDate"]).ToShortDateString();
                        }
                        catch { }
                        break;
                }
            }
        }

        private void rdoTimeIn_Click(object sender, EventArgs e)
        {
            txtEmpNo.Focus();
        }

        private void rdoLunchOut_Click(object sender, EventArgs e)
        {
            txtEmpNo.Focus();
        }

        private void rdoBackfrLunch_Click(object sender, EventArgs e)
        {
            txtEmpNo.Focus();
        }

        private void rdoTimeOut_Click(object sender, EventArgs e)
        {
            txtEmpNo.Focus();
        }
    }
}
