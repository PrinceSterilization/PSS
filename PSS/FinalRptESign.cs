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
    public partial class FinalRptESign : Form
    {
        private DataTable dtQAESign = new DataTable();
        private DataTable dtSDESign = new DataTable();
        private DataTable dtForEMail = new DataTable();

        public FinalRptESign()
        {
            InitializeComponent();
        }

        private void FinalRptESign_Load(object sender, EventArgs e)
        {
        }

        private void LoadQAESign()
        {
            try
            {
                dtQAESign = PSSClass.FinalReports.FinrRptQAESign();
                bsQAESign.DataSource = dtQAESign;
                dgvQA.DataSource = bsQAESign;
                QAGridSetting();
                lblQATotal.Text = "TOTAL : " + dtQAESign.Rows.Count.ToString("#,##0");
            }
            catch { }
        }

        private void LoadSDESign()
        {
            try
            {
                dtSDESign = PSSClass.FinalReports.FinrRptSDESign();
                bsSDESign.DataSource = dtSDESign;
                dgvSD.DataSource = bsSDESign;
                SDGridSetting();
                lblSDTotal.Text = "TOTAL : " + dtSDESign.Rows.Count.ToString("#,##0");

                dgvSDNames.Rows.Clear();
                dgvSDNames.RowTemplate.Height = 50;
                int nSD = 0;
                for (int i = 0; i < dtSDESign.Rows.Count; i++)
                {
                    if (nSD != Convert.ToInt16(dtSDESign.Rows[i]["StudyDirID"]))
                    {
                        Image image;
                        if (System.IO.File.Exists(@"\\PSAPP01\IT Files\PTS\Images\HR\" + dtSDESign.Rows[i]["LoginName"].ToString() + "_sd" + ".jpg") == true) 
                        {
                            image = Image.FromFile(@"\\PSAPP01\IT Files\PTS\Images\HR\" + dtSDESign.Rows[i]["LoginName"].ToString() + "_sd" + ".jpg");
                        }
                        else
                        {
                            image = Image.FromFile(@"\\PSAPP01\IT Files\PTS\Images\PSS Logo.png");
                        }
                        this.dgvSDNames.Rows.Add(dtSDESign.Rows[i]["StudyDir"], dtSDESign.Rows[i]["StudyDirID"], image);
                        nSD = Convert.ToInt16(dtSDESign.Rows[i]["StudyDirID"]);
                    }
                }
                if (dgvSDNames.Rows.Count > 0)
                {
                    //string strID = dgvSDNames.Rows[0].Cells["StudyDirID"].Value.ToString();
                    string strID = LogIn.nUserID.ToString();
                    bsSDESign.Filter = "StudyDirID = " + Convert.ToInt16(strID);
                    if (dgvSD.Rows.Count == 0)
                    {
                        strID = dgvSDNames.Rows[0].Cells["StudyDirID"].Value.ToString();
                        bsSDESign.Filter = "StudyDirID = " + Convert.ToInt16(strID);
                    }
                }
            }
            catch { }
        }

        private void LoadForEMail()
        {
            try
            {
                dtForEMail = PSSClass.FinalReports.FinrRptForEMail();
                bsForEMail.DataSource = dtForEMail;
                dgvForEMail.DataSource = bsForEMail;
                ForEMailGridSetting();
                lblForEMail.Text = "TOTAL : " + dtForEMail.Rows.Count.ToString("#,##0");
            }
            catch { }
        }

        private void QAGridSetting()
        {
            dgvQA.EnableHeadersVisualStyles = false;
            dgvQA.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQA.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvQA.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvQA.Columns["RptNo"].HeaderText = "REPORT NO.";
            dgvQA.Columns["RevNo"].HeaderText = "REVISION NO.";
            dgvQA.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvQA.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvQA.Columns["StudyDir"].HeaderText = "STUDY DIRECTOR";
            dgvQA.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvQA.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvQA.Columns["RptNo"].Width = 70;
            dgvQA.Columns["RptNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQA.Columns["RevNo"].Width = 70;
            dgvQA.Columns["RevNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQA.Columns["SponsorID"].Width = 70;
            dgvQA.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQA.Columns["SponsorName"].Width = 250;
            dgvQA.Columns["StudyDir"].Width = 90;
            dgvQA.Columns["ServiceCode"].Width = 70;
            dgvQA.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQA.Columns["ServiceDesc"].Width = 290;
        }

        private void SDGridSetting()
        {
            dgvSD.EnableHeadersVisualStyles = false;
            dgvSD.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSD.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvSD.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvSD.Columns["RptNo"].HeaderText = "REPORT NO.";
            dgvSD.Columns["RevNo"].HeaderText = "REVISION NO.";
            dgvSD.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvSD.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvSD.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvSD.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvSD.Columns["RptNo"].Width = 70;
            dgvSD.Columns["RptNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSD.Columns["RevNo"].Width = 70;
            dgvSD.Columns["RevNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSD.Columns["SponsorID"].Width = 70;
            dgvSD.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSD.Columns["SponsorName"].Width = 250;
            dgvSD.Columns["ServiceCode"].Width = 70;
            dgvSD.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSD.Columns["ServiceDesc"].Width = 210;
            dgvSD.Columns["StudyDirID"].Visible = false;
            dgvSD.Columns["StudyDir"].Visible = false;
            dgvSD.Columns["LoginName"].Visible = false;
        }

        private void ForEMailGridSetting()
        {
            dgvForEMail.EnableHeadersVisualStyles = false;
            dgvForEMail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvForEMail.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvForEMail.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgvForEMail.Columns["RptNo"].HeaderText = "REPORT NO.";
            dgvForEMail.Columns["RevNo"].HeaderText = "REVISION NO.";
            dgvForEMail.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvForEMail.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvForEMail.Columns["StudyDir"].HeaderText = "STUDY DIRECTOR";
            dgvForEMail.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvForEMail.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvForEMail.Columns["RptNo"].Width = 70;
            dgvForEMail.Columns["RptNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvForEMail.Columns["RevNo"].Width = 70;
            dgvForEMail.Columns["RevNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvForEMail.Columns["SponsorID"].Width = 70;
            dgvForEMail.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvForEMail.Columns["SponsorName"].Width = 250;
            dgvForEMail.Columns["StudyDir"].Width = 90;
            dgvForEMail.Columns["ServiceCode"].Width = 70;
            dgvForEMail.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvForEMail.Columns["ServiceDesc"].Width = 290;
        }

        private void dgvQA_DoubleClick(object sender, EventArgs e)
        {
            if (dgvQA.Rows.Count > 0)
            {
                int intOpen = PSSClass.General.OpenForm(typeof(FinalReports));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(FinalReports));
                }
                FinalReports childForm = new FinalReports();
                childForm.Text = "FINAL REPORTS";
                childForm.MdiParent = this.MdiParent;
                childForm.nRptNo = Convert.ToInt32(dgvQA.Rows[dgvQA.CurrentCell.RowIndex].Cells[0].Value);
                childForm.nLSw = 1;
                childForm.Show();
            }
        }

        private void dgvQA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvQA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                dgvQA_DoubleClick(null, null);
        }

        private void FinalRptESign_Activated(object sender, EventArgs e)
        {
            LoadQAESign();
            LoadSDESign();
            LoadForEMail();
        }

        private void dgvSD_DoubleClick(object sender, EventArgs e)
        {
            if (dgvSD.Rows.Count > 0)
            {
                int intOpen = PSSClass.General.OpenForm(typeof(FinalReports));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(FinalReports));
                }
                FinalReports childForm = new FinalReports();
                childForm.Text = "FINAL REPORTS";
                childForm.MdiParent = this.MdiParent;
                childForm.nRptNo = Convert.ToInt32(dgvSD.Rows[dgvSD.CurrentCell.RowIndex].Cells[0].Value);
                childForm.nLSw = 2;
                childForm.Show();
            }
        }

        private void dgvSDNames_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string strID = dgvSDNames.Rows[dgvSDNames.CurrentCell.RowIndex].Cells["StudyDirID"].Value.ToString();
            bsSDESign.Filter = "StudyDirID = " + Convert.ToInt16(strID);
        }

        private void dgvForEMail_DoubleClick(object sender, EventArgs e)
        {
            if (dgvForEMail.Rows.Count > 0)
            {
                int intOpen = PSSClass.General.OpenForm(typeof(FinalReports));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(FinalReports));
                }
                FinalReports childForm = new FinalReports();
                childForm.Text = "FINAL REPORTS";
                childForm.MdiParent = this.MdiParent;
                childForm.nRptNo = Convert.ToInt32(dgvForEMail.Rows[dgvForEMail.CurrentCell.RowIndex].Cells[0].Value);
                childForm.nLSw = 3;
                childForm.Show();
            }
        }
    }
}
