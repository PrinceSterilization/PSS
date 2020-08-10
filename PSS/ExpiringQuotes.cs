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
    public partial class ExpiringQuotes : Form
    {
        private DataTable dtReissue;

        public ExpiringQuotes()
        {
            InitializeComponent();
        }

        private void ExpiringQuotes_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            txtYear.Text = DateTime.Now.Year.ToString();
            LoadRecords();
            //btnPrintCurrent.Visible = true; btnPrintExcList.Visible = true;
            //dgvReissue.Visible = true;
        }

        private void LoadRecords()
        {
            if (txtYear.Text.Trim() == "")
                return;
               
            dtReissue = PSSClass.Quotations.QuotesExpiring(Convert.ToInt16(txtYear.Text), 1);

            if (dtReissue != null && dtReissue.Rows.Count > 0)
            {
                dgvReissue.DataSource = dtReissue;
                dgvReissue.EnableHeadersVisualStyles = false;
                dgvReissue.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReissue.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                dgvReissue.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvReissue.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
                dgvReissue.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
                dgvReissue.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dgvReissue.Columns["CmpyCode"].HeaderText = "CMPY CODE";
                dgvReissue.Columns["QuoteNo"].HeaderText = "QUOTE NO.";
                dgvReissue.Columns["RevNo"].HeaderText = "REV. NO.";
                dgvReissue.Columns["SpID"].HeaderText = "SPONSOR ID";
                dgvReissue.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
                dgvReissue.Columns["DteMailed"].HeaderText = "DATE E-MAILED";
                dgvReissue.Columns["ExpDate"].HeaderText = "EXPIRY DATE";
                dgvReissue.Columns["ExpMonths"].HeaderText = "MONTHS TO EXPIRY DATE";
                dgvReissue.Columns["CmpyCode"].Width = 50;
                dgvReissue.Columns["QuoteNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReissue.Columns["QuoteNo"].Width = 75;
                dgvReissue.Columns["RevNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReissue.Columns["RevNo"].Width = 60;
                dgvReissue.Columns["SpID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReissue.Columns["SpID"].Width = 65;
                dgvReissue.Columns["SponsorName"].Width = 200;
                dgvReissue.Columns["DteMailed"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReissue.Columns["DteMailed"].DefaultCellStyle.Format = "MM/dd/yyyy";
                dgvReissue.Columns["DteMailed"].Width = 75;
                dgvReissue.Columns["ExpMonths"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReissue.Columns["ExpMonths"].Width = 100;
                dgvReissue.Columns["ExpDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvReissue.Columns["ExpDate"].DefaultCellStyle.Format = "MM/dd/yyyy";
                dgvReissue.Columns["ExpDate"].Width = 75;
                dgvReissue.Columns["ExpEMailSent"].Visible = false;
            }
            else
                dgvReissue.DataSource = null;
        }

        private void dgvReissue_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dgvReissue.CurrentCell.OwningColumn.Name != "ReissueQuote")
            {
                e.Cancel = true;
            }
        }

        private void dgvReissue_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvReissue.CurrentCell.OwningColumn.Name.ToString() == "QuoteNo")
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to exclude this quotation?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.Yes)
                {
                    SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                    SqlCommand sqlcmd = new SqlCommand();

                    sqlcmd.Connection = sqlcnn;
                    sqlcmd.Parameters.AddWithValue("@CmpyCode", dgvReissue.Rows[dgvReissue.CurrentCell.RowIndex].Cells["CmpyCode"].Value.ToString());
                    sqlcmd.Parameters.AddWithValue("@QuoteNo", dgvReissue.CurrentCell.Value.ToString());
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spQuoteExcReIssue";

                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    dtReissue = PSSClass.Quotations.QuotesExpiring(Convert.ToInt16(txtYear.Text), 1);
                    dgvReissue.DataSource = dtReissue;
                }
                else
                {
                    string strQ = dgvReissue.CurrentCell.Value.ToString();
                    Int16 nRN = Convert.ToInt16(dgvReissue.Rows[e.RowIndex].Cells[1].Value);
                    this.WindowState = FormWindowState.Minimized;
                    Quotes Q = new Quotes();
                    Q.MdiParent = Program.mdi;
                    Q.nPSw = 3;
                    Q.strQuoteNo = strQ;
                    Q.lnkRevNo = nRN;
                    Q.Text = "QUOTATIONS";
                    Q.Show();
                }
            }
        }

        private void txtYear_TextChanged(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void ExpiringQuotes_Activated(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(802, 610);
            LoadRecords();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (dgvReissue.Rows.Count == 0)
            {
                MessageBox.Show("No quotes to print preview.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            QuotationRpt rptQuotation = new QuotationRpt();
            rptQuotation.WindowState = FormWindowState.Maximized;
            rptQuotation.nQ = 2;
            try
            {
                string strQNo = dgvReissue.Rows[dgvReissue.CurrentCell.RowIndex].Cells["QuoteNo"].Value.ToString(); ;// strQuote.Substring(0, 9);
                string strRevNo = dgvReissue.Rows[dgvReissue.CurrentCell.RowIndex].Cells["RevNo"].Value.ToString(); //strQuote.Substring(strQuote.IndexOf("R") + 1, strQuote.Length - (strQuote.IndexOf("R") + 1));
                rptQuotation.QuoteNo = strQNo;
                rptQuotation.RevNo = Convert.ToInt16(strRevNo);
                rptQuotation.CmpyCode = dgvReissue.Rows[dgvReissue.CurrentCell.RowIndex].Cells["CmpyCode"].Value.ToString(); ;
                rptQuotation.Show();
            }
            catch { }
        }
    }
}
