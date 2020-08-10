using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GIS
{
    public partial class BudgetMaint : Form
    {
        protected DataTable dtFile = new DataTable();
        protected DataTable dtAccts = new DataTable();
        private string strFileAccess = "RO";
        private int nT = 0;

        public BudgetMaint()
        {
            InitializeComponent();
        }

        private void BudgetMaint_Load(object sender, EventArgs e)
        {
            dtAccts.Columns.Add("Yr", typeof(Int16));
            dtAccts.Columns.Add("AcctID", typeof(Int16));

            btnUpdate.Enabled = false;

            strFileAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "BudgetMaint");
            if (strFileAccess == "FA")
                btnUpdate.Enabled = true;

            DataTable dtX = new DataTable();
            dtX.Columns.Add("FSY", typeof(string));

            DateTime dte = DateTime.Now;
            DataRow dR;
            int nY = dte.Year;
            for (int i = 0; i < 5; i++)
            {
                dR = dtX.NewRow();
                dR["FSY"] = nY.ToString();
                dtX.Rows.Add(dR);
                nY -= 1;
            }
            dR = dtX.NewRow();
            dR["FSY"] = "-select-";
            dtX.Rows.InsertAt(dR, 0);
            cboFY.DataSource = dtX;
            cboFY.DisplayMember = "FSY";
            cboFY.ValueMember = "FSY";
        }

        private void cboFY_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dtFile = GISClass.Financials.GetTable("Yr", Convert.ToInt16(cboFY.Text), "spBudgetMaint");
                bsFile.DataSource = dtFile;
                bnFile.BindingSource = bsFile;
                dgvFile.DataSource = bsFile;
                DataGridSetting();
            }
            catch { }
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["AcctID"].HeaderText = "ACCOUNT ID";
            dgvFile.Columns["AcctID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvFile.Columns["AcctID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["AcctTitle"].HeaderText = "ACCOUNT TITLE";
            dgvFile.Columns["AcctTitle"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            string strM = "";
            string strMH = "";
            DateTime dte = Convert.ToDateTime("1/1/2000");
            for (int i = 1; i <= 12; i++)
            {
                strM = "MonthBudget" + i.ToString("00");
                strMH = dte.ToString("MMM");
                dgvFile.Columns[strM].HeaderText = strMH;
                dgvFile.Columns[strM].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvFile.Columns[strM].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvFile.Columns[strM].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvFile.Columns[strM].DefaultCellStyle.Format = "$#,##0.00";
                dgvFile.Columns[strM].Width = 90;
                dte = dte.AddMonths(1);
            }
            dgvFile.Columns["AnnualBudget"].HeaderText = "ANNUAL BUDGET";
            dgvFile.Columns["AnnualBudget"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvFile.Columns["AnnualBudget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["AnnualBudget"].DefaultCellStyle.Format = "$#,##0.00";
            dgvFile.Columns["AnnualBudget"].Width = 90;
            dgvFile.Columns["BookCode"].Visible = false;
            dgvFile.Columns["AcctSw"].Visible = false;
            dgvFile.Columns["SubAcctID"].Visible = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, GraphicsUnit.Point);
            dgvFile.DefaultCellStyle.Font = new Font("Arial", 9, GraphicsUnit.Point);

            for (int i = 0; i < dgvFile.Rows.Count; i++)
            {
                if (dgvFile.Rows[i].Cells["AcctSw"].Value.ToString() == "1")
                {
                    DataRow dR = dtAccts.NewRow();
                    dR["Yr"] = Convert.ToInt16(cboFY.Text);
                    dR["AcctID"] = Convert.ToInt16(dgvFile.Rows[i].Cells["AcctID"].Value);
                    dtAccts.Rows.Add(dR);
                }
            }
        }

        private void dgvFile_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvFile.CurrentCell.OwningColumn.Name.ToString().IndexOf("Month") == -1  ||
                dgvFile.CurrentCell.OwningColumn.Name.ToString().IndexOf("Month") != -1 && strFileAccess != "FA")
            {
                e.Cancel = true;
            }
        }

        private void BudgetMaint_FormClosing(object sender, FormClosingEventArgs e)
        {
            bsFile.EndEdit();
            DataTable dtEdited = dtFile.GetChanges(DataRowState.Modified);
            if (dtEdited != null && dtEdited.Rows.Count > 0)
            {
                if (MessageBox.Show("Changes were made the file." + Environment.NewLine + "Do you want to exit without saving?", Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    return;
                }
                for (int i = 0; i < dtEdited.Rows.Count; i++)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        GISClass.Financials.UpdateBudget(Convert.ToInt16(cboFY.Text), Convert.ToInt16(dtEdited.Rows[i]["AcctID"]), Convert.ToInt16(j), Convert.ToDecimal(dtEdited.Rows[i]["MonthBudget" + j.ToString("00")]));
                    }
                }
                dtEdited.Dispose();
            }
        }

        private void dgvFile_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvFile.CurrentCell.OwningColumn.Name.ToString() != "Budget")
            {
                e.Control.KeyPress += new KeyPressEventHandler(CheckNum);
            }
        }

        private void CheckNum(object sender, KeyPressEventArgs e)
        { 
            if (!char.IsControl(e.KeyChar) && 
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.' &&
                e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cboFY.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select year.", Application.ProductName);
                cboFY.SelectedIndex = 0;
                return;
            }
            if (dgvFile.Rows.Count == 0)
            {
                MessageBox.Show("Accounts File is empty.", Application.ProductName);
                return;
            }

            if (MessageBox.Show("This process would calculate current year budget" + Environment.NewLine + "based on previous year's actual amounts." + Environment.NewLine + Environment.NewLine +
                "Do you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (MessageBox.Show("WARNING: This process would overwrite any existing" + Environment.NewLine + "data already stored for the current year." + Environment.NewLine + Environment.NewLine +
                "Are you sure you want to proceed?", Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            lblUpdate.Visible = true; 
            if (dtAccts != null && dtAccts.Rows.Count > 0)
            {
                for (int i = 0; i < dtFile.Rows.Count; i++)
                {
                    GISClass.Financials.UpdateSummaryAccts(Convert.ToInt16(cboFY.Text), Convert.ToInt16(dtFile.Rows[i]["AcctID"]));
                }
            }
            lblUpdate.Visible = false;

            bsFile.EndEdit();
            DataTable dtEdited = dtFile.GetChanges(DataRowState.Modified);
            if (dtEdited != null && dtEdited.Rows.Count > 0)
            {
                for (int i = 0; i < dtEdited.Rows.Count; i++)
                {
                    for (int j = 1; j <= 12; j++)
                    {
                        GISClass.Financials.UpdateBudget(Convert.ToInt16(cboFY.Text), Convert.ToInt16(dtEdited.Rows[i]["AcctID"]), Convert.ToInt16(j), Convert.ToDecimal(dtEdited.Rows[i]["MonthBudget" + j.ToString("00")]));
                    }
                }
                dtEdited.Dispose();
                dtFile.AcceptChanges();
                MessageBox.Show("Budget file successfully updated.");
            }
            else
            {
                MessageBox.Show("No updates were made to this file.");
            }
        }

        private void btnGenBudget_Click(object sender, EventArgs e)
        {
            if (cboFY.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select year.", Application.ProductName);
                cboFY.SelectedIndex = 0;
                return;
            }
            if (txtPercent.Text != "" && txtPercent.Text != "0")
                lblUpdate.Text = "Calculating budget for the current year...";
            else
                lblUpdate.Text = "Retrieving previous year values as budget...";

            lblUpdate.Visible = true; lblUpdate.BringToFront();
            btnPreviewVar.Visible = false;
            tmrCalculate.Enabled = true;

        }

        private void txtPercent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void tmrCalculate_Tick(object sender, EventArgs e)
        {
            nT++;
            if (nT == 2)
            {
                decimal nPAmt = 0;
                Int16 nYr = Convert.ToInt16(cboFY.Text);
                nYr -= 1;
                decimal n = 1;
                if (txtPercent.Text.Trim() != "" && Convert.ToInt16(txtPercent.Text) != 0)
                    n = (1 + Convert.ToDecimal(txtPercent.Text) / 100);

                DataTable dtPrev = GISClass.Financials.GetPrevAmount(nYr);
                for (int i = 0; i < dgvFile.Rows.Count; i++)
                {
                    nPAmt = 0;
                    DataRow[] foundRows;
                    foundRows = dtPrev.Select("AccountID = " + dgvFile.Rows[i].Cells["AcctID"].Value.ToString());
                    if (foundRows.Length > 0)
                    {
                        for (int j = 1; j <= 12; j++)
                        {
                            if (dgvFile.Rows[i].Cells["BookCode"].Value.ToString() == "2")
                            {
                                dgvFile.Rows[i].Cells["MonthBudget" + j.ToString("00")].Value = Convert.ToDecimal(foundRows[0]["MonthBudget" + j.ToString("00")]) * (-1) * n;
                                nPAmt += (Convert.ToDecimal(foundRows[0]["MonthBudget" + j.ToString("00")]) * (-1) * n);
                            }
                            else
                            {
                                dgvFile.Rows[i].Cells["MonthBudget" + j.ToString("00")].Value = Convert.ToDecimal(foundRows[0]["MonthBudget" + j.ToString("00")]) * n;
                                nPAmt += Convert.ToDecimal(foundRows[0]["MonthBudget" + j.ToString("00")]) * n;
                            }
                        }
                    }
                    dgvFile.Rows[i].Cells["AnnualBudget"].Value = nPAmt;
                }
                dtPrev.Dispose();
                lblUpdate.Visible = false; btnPreviewVar.Visible = true;
                tmrCalculate.Enabled = false;
                nT = 0;
            }
        }

        private void btnPreviewVar_Click(object sender, EventArgs e)
        {
            TemplateForm.OpenControls(this, false);
            foreach (Control Ctrl in this.Controls)
            {
                if ((Ctrl.GetType().ToString()) == "System.Windows.Forms.Button")
                    ((Button)Ctrl).Enabled = false;
            }
            foreach (DataGridViewRow row in dgvFile.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.DarkGray;
            }
            pnlReports.Enabled = true; pnlReports.Visible = true; btnPreview.Enabled = true; btnClose.Enabled = true;
        }

        private void BudgetMaint_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void BudgetMaint_Deactivate(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            TemplateForm.OpenControls(this, true);
            dgvFile.BackgroundColor = Color.White;
            foreach (Control Ctrl in this.Controls)
            {
                if ((Ctrl.GetType().ToString()) == "System.Windows.Forms.Button")
                    ((Button)Ctrl).Enabled = true;
            }
            foreach (DataGridViewRow row in dgvFile.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }

            pnlReports.Visible = false; 
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (rdoMonthly.Checked == true && cboMonthly.SelectedIndex == -1)
            {
                MessageBox.Show("Please select month.", Application.ProductName);
                return;
            }
            if (rdoQtrly.Checked == true && cboQtrly.SelectedIndex == -1)
            {
                MessageBox.Show("Please select quarter.", Application.ProductName);
                return;
            }
            DataTable dtActual = GISClass.Financials.GetActualAmount(Convert.ToInt16(cboFY.Text));
            decimal nBudget = 0;
            Int16 nMonth = 1; Int16 nQ = 1;
            for (int i = 0; i < dtActual.Rows.Count; i++)
            {
                if (Convert.ToInt16(dtActual.Rows[i]["BSCode"]) == 4)
                {
                    nBudget = 0;
                    DataRow[] foundRows;
                    foundRows = dtFile.Select("AcctID = " + dtActual.Rows[i]["AccountID"]);
                    if (foundRows.Length > 0)
                    {
                        for (int j = 1; j <= 12; j++)
                        {
                            nBudget += Convert.ToDecimal(foundRows[0]["MonthBudget" + j.ToString("00")]);
                        }
                    }
                    dtActual.Rows[i]["Budget"] = nBudget;
                }
            }
            MgmtRpts rpt = new MgmtRpts();
            if (rdoYTD.Checked == true)
            {
                rpt.rptName = "VarianceYTD";
                nMonth = Convert.ToInt16(DateTime.Now.Month);
            }
            else if (rdoMonthly.Checked == true)
            {
                rpt.rptName = "VarianceMonthly";
                nMonth = Convert.ToInt16(cboMonthly.SelectedIndex + 1);
            }
            else if (rdoQtrly.Checked == true)
            {
                rpt.rptName = "VarianceQtrly";
                nQ = Convert.ToInt16(cboQtrly.SelectedIndex + 1);
            }
            rpt.nYr = Convert.ToInt16(cboFY.Text);
            rpt.nMo = nMonth;
            rpt.nQtr = nQ ;
            rpt.dtRpt = dtActual;
            try
            {
                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dgvFile_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            decimal nPAmt = 0;
            for (int j = 1; j <= 12; j++)
            {
                nPAmt += Convert.ToDecimal(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["MonthBudget" + j.ToString("00")].Value);
            }
            dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["AnnualBudget"].Value = nPAmt;
        }

        private void rdoQtrly_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoQtrly.Checked == true)
                cboMonthly.SelectedIndex = -1;
        }

        private void rdoYTD_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoYTD.Checked == true)
            {
                cboMonthly.SelectedIndex = -1; cboQtrly.SelectedIndex = -1;
            }
        }

        private void rdoMonthly_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMonthly.Checked == true)
                cboQtrly.SelectedIndex = -1;
        }
    }
}
