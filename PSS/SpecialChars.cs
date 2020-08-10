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
    public partial class SpecialChars : Form
    {
        public SpecialChars()
        {
            InitializeComponent();
        }

        private void SpecialChars_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

            DataTable dtQ = new DataTable();
            dtQ = PSSClass.General.SpecialChars(1);
            if (dtQ == null)
            {
                MessageBox.Show("Connection problem encountered.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            bsSymbols.DataSource = dtQ;
            DataGridSetting();
            dgvSymbols.RowCount = dtQ.Rows.Count;
            int nChar = 0; char strChar;
            for (int i = 0; i < dtQ.Rows.Count; i++)
            {
                if (dtQ.Rows[i]["CharAlt"] != DBNull.Value)
                    strChar = Convert.ToChar(dtQ.Rows[i]["CharAlt"]);
                else
                {
                    nChar = Convert.ToInt16(dtQ.Rows[i]["CharCode"].ToString());
                    strChar = Convert.ToChar(nChar);
                }
                dgvSymbols.Rows[i].Cells[0].Value = strChar.ToString();
                dgvSymbols.Rows[i].Cells[1].Value = dtQ.Rows[i]["CharDesc"];
            }
        }

        private void DataGridSetting()
        {
            dgvSymbols.EnableHeadersVisualStyles = false;
            dgvSymbols.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSymbols.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvSymbols.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvSymbols.Columns["CharCode"].HeaderText = "SYMBOL";
            dgvSymbols.Columns["CharDesc"].HeaderText = "DESCRIPTION";
            dgvSymbols.Columns["CharCode"].Width = 75;
            dgvSymbols.Columns["CharCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSymbols.Columns["CharDesc"].Width = 218;
        }

        private void dgvSymbols_Click(object sender, EventArgs e)
        {
            try
            {
                txtChar.Text = dgvSymbols.Rows[dgvSymbols.CurrentCell.RowIndex].Cells[0].Value.ToString();
                System.Windows.Forms.Clipboard.SetText(txtChar.Text);
            }
            catch { }
        }
    }
}
