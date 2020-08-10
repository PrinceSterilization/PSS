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
    public partial class PrepaymentList : Form
    {
        public PrepaymentList()
        {
            InitializeComponent();
        }

        private void PrepaymentList_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Billing.PrepayEstimates();
            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No quotations available for prepayment.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            DataView dataView = new DataView(dt);
            bsPrepayments.DataSource = dataView;
            dgvFile.DataSource = bsPrepayments;
            DataGridSetting();
            this.Location = new Point(1, 150);
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["QuotationNo"].HeaderText = "QUOTE NO.";
            dgvFile.Columns["QuotationNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["RevisionNo"].HeaderText = "REVISION NO.";
            dgvFile.Columns["RevisionNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["RevisionNo"].Width = 90;
            dgvFile.Columns["Comments"].HeaderText = "PO NO.";
            dgvFile.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["ContactID"].HeaderText = "CONTACT ID";
            dgvFile.Columns["ContactID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["Contact"].HeaderText = "CONTACT";
            dgvFile.Columns["Amount"].HeaderText = "ESTIMATED PREPAYMENT AMOUNT";
            dgvFile.Columns["QuotationNo"].Width = 90;
            dgvFile.Columns["RevisionNo"].Width = 85;
            dgvFile.Columns["Comments"].Width = 100;
            dgvFile.Columns["SponsorID"].Width = 85;
            dgvFile.Columns["SponsorName"].Width = 268;
            dgvFile.Columns["Contact"].Width = 150;
            dgvFile.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvFile.Columns["Amount"].DefaultCellStyle.Format = "#,##0.00";
            dgvFile.Columns["ContactID"].Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); this.Dispose();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PrePayment.strQuoteNo = dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["QuotationNo"].Value.ToString();
            PrePayment.nRevNo = Convert.ToInt16(dgvFile.Rows[dgvFile.CurrentCell.RowIndex].Cells["RevisionNo"].Value);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dgvFile_DoubleClick(object sender, EventArgs e)
        {
            if (dgvFile.Rows.Count > 0 && dgvFile.CurrentCell.OwningColumn.Name == "QuotationNo" && dgvFile.CurrentCell.Value.ToString() != "")
            {
                int intOpen = PSSClass.General.OpenForm(typeof(Quotes));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(Quotes));
                }
                Quotes childForm = new Quotes();
                childForm.Text = "QUOTATIONS";
                childForm.MdiParent = this.MdiParent;
                childForm.strQuoteNo = dgvFile.CurrentCell.Value.ToString();
                childForm.nPSw = 1;
                childForm.Show();
            }
        }
    }
}
