using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace GIS
{
    public partial class UserReminders : Form
    {
        public UserReminders()
        {
            InitializeComponent();
        }

        private void UserReminders_Load(object sender, EventArgs e)
        {
            dgvReminders.Rows.Add("Final Reports", 0);
            dgvReminders.Rows.Add("PO Approval", 0);
            dgvReminders.Rows.Add("Work Leave Approval", 0);

            //Final Reports E-Signatures
            DataTable dtRpts = GISClass.FinalReports.FinrRptSDESign();
            if (dtRpts != null && dtRpts.Rows.Count > 0)
            {
                dgvReminders.Rows[0].Cells[1].Value = dtRpts.Rows.Count;
            }

            //PO Approval
            DataTable dtPendingPO = GISClass.Procurements.POFirstApprovalESign();
            if (dtPendingPO != null && dtPendingPO.Rows.Count > 0)
            {
                dgvReminders.Rows[1].Cells[1].Value = dtPendingPO.Rows.Count;
            }

            //Work Leave Approval
            DataTable dtWLRem = GISClass.Users.WorkLeaveRem(Convert.ToInt16(LogIn.nUserID));//LogIn.nUserID 114
            if (dtWLRem != null && dtWLRem.Rows.Count > 0)
            {
                dgvReminders.Rows[2].Cells[1].Value = dtWLRem.Rows.Count;
            }

            DataTable dtWLEmp = GISClass.Users.WorkLeaveEmp();
            dgvEmpOnWL.DataSource = dtWLEmp;
            dgvEmpOnWL.Columns[0].HeaderText = "EMPLOYEE NAME";
            dgvEmpOnWL.Columns[0].Width = 250;
            dgvEmpOnWL.Columns[1].HeaderText = "LEAVE TYPE";
            dgvEmpOnWL.Columns[1].Width = 150;
        }

        private void dgvReminders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == 0 && e.ColumnIndex == 1 && dgvReminders.Rows[e.RowIndex].Cells[1].Value.ToString() != "0")
            {
                //Get Study Directors
                DataTable dtSD = GISClass.Employees.StudyDirectors();
                if (dtSD.Rows.Count > 0)
                {
                    //Filter SDs by User ID
                    DataView dvw = new DataView(dtSD);
                    dvw.RowFilter = "EmployeeID = " + Convert.ToInt16(LogIn.nUserID).ToString();//LogIn.nUserID 114
                    dtSD = dvw.ToTable();
                    if (dtSD != null && dtSD.Rows.Count > 0) //if User is Study Director, get Final Reports for E-Signature
                    {
                        int intOpen = GISClass.General.OpenForm(typeof(FinalRptESign));

                        if (intOpen == 0)
                        {
                            FinalRptESign childForm = new FinalRptESign();
                            childForm.MdiParent = this.MdiParent;
                            childForm.Text = "E-SIGNATURES";
                            childForm.Show();
                        }
                        //else
                        //{
                        //    MessageBox.Show("The form is already open. Please select" + Environment.NewLine + "the form from the Window menu.", Application.ProductName);
                        //    return;
                        //}
                    }
                    else
                    {
                        MessageBox.Show("You have no permission to " + Environment.NewLine + "open the form at this time.", Application.ProductName);
                        //lblMessage.Text = "You have no permission to open the form at this time.";
                        //lblMessage.Visible = true;
                        return;
                    }
                }
            }
            else if (e.RowIndex == 1 && e.ColumnIndex == 1 && dgvReminders.Rows[e.RowIndex].Cells[1].Value.ToString() != "0")
            {
                //Get PO Approvers
                DataTable dtPOAppr = GISClass.Employees.POApprovers();
                if (dtPOAppr.Rows.Count > 0)
                {
                    //Filter PO Approvers by User ID
                    DataView dvw = new DataView(dtPOAppr);
                    dvw.RowFilter = "EmployeeID = " + Convert.ToInt16(LogIn.nUserID).ToString();//LogIn.nUserID 114
                    dtPOAppr = dvw.ToTable();
                    if (dtPOAppr != null && dtPOAppr.Rows.Count > 0) //if User is PO Approver, activate form
                    {
                        int intOpen = GISClass.General.OpenForm(typeof(POESign));

                        if (intOpen == 0)
                        {
                            POESign childForm = new POESign();
                            childForm.MdiParent = this.MdiParent;
                            childForm.Text = "PO E-SIGNATURES";
                            childForm.Show();
                        }
                        //else
                        //{
                        //    MessageBox.Show("The form is already open. Please select" + Environment.NewLine + "the form from the Window menu.", Application.ProductName);
                        //    return;
                        //}
                    }
                    else
                    {
                        MessageBox.Show("You have no permission to " + Environment.NewLine + "open the form at this time.", Application.ProductName);
                        //lblMessage.Text = "You have no permission to open the form at this time.";
                        //lblMessage.Visible = true;
                        return;
                    }
                }
            }
            else if (e.RowIndex == 2 && e.ColumnIndex == 1 && dgvReminders.Rows[e.RowIndex].Cells[1].Value.ToString() != "0")
            {
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = Application.StartupPath + @"\GHRS.exe";
                    proc.StartInfo.Arguments = LogIn.nUserID.ToString();
                    proc.Start();
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); this.Dispose();
        }

        private void UserReminders_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void UserReminders_Deactivate(object sender, EventArgs e)
        {
            this.Close(); this.Dispose();
        }
    }
}
