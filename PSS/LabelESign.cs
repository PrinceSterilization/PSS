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
    public partial class LabelESign : Form
    {
        private DataTable dtApproval = new DataTable();
       
        public LabelESign()
        {
            InitializeComponent();
        }

        private void LoadApprovalESign()
        {
            try
            {
                dtApproval = GISClass.Tools.LabelApprovalESign();
                bsApproval.DataSource = dtApproval;
                dgvSterility.DataSource = bsApproval;
                ApprovalGridSetting();
                lblApprovalTotal.Text = "TOTAL : " + dtApproval.Rows.Count.ToString("#,##0");
            }
            catch { }
        }
        
        private void ApprovalGridSetting()
        {
            dgvSterility.Columns["GBLNo"].HeaderText = "GBL No";
            dgvSterility.Columns["SponsorID"].HeaderText = "Sponsor ID";
            dgvSterility.Columns["SponsorName"].HeaderText = "Sponsor Name";
            dgvSterility.Columns["SterClassDesc"].HeaderText = "Classification";
            //dgvFile.Columns["CreatedBy"].HeaderText = "Created By";
            //dgvFile.Columns["DateCreated"].HeaderText = "Date Created";
            dgvSterility.Columns["GBLNo"].Width = 80;
            dgvSterility.Columns["SponsorID"].Width = 70;
            dgvSterility.Columns["SponsorName"].Width = 300;
            dgvSterility.Columns["SterClassDesc"].Width = 300;
            //dgvFile.Columns["CreatedBy"].Width = 80;
            //dgvFile.Columns["DateCreated"].Width = 70;
            dgvSterility.Columns["SterClassID"].Visible = false;
            //dgvFile.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvSterility.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvFile.Columns["DateCreated"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;  
        }

        private void dgvSterility_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSterlity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                dgvSterility_DoubleClick(null, null);
        }

        private void dgvSterility_DoubleClick(object sender, EventArgs e)
        {
            if (dgvSterility.Rows.Count > 0)
            {
                int intOpen = GISClass.General.OpenForm(typeof(Labels));

                if (intOpen == 1)
                {
                    GISClass.General.CloseForm(typeof(Labels));
                }
                Labels childForm = new Labels();
                childForm.Text = "Labels";
                childForm.MdiParent = this.MdiParent;
                childForm.intGBLNo = Convert.ToInt64(dgvSterility.Rows[dgvSterility.CurrentCell.RowIndex].Cells["GBLNo"].Value.ToString());
                childForm.intSterClassID = Convert.ToInt16(dgvSterility.Rows[dgvSterility.CurrentCell.RowIndex].Cells["SterClassID"].Value.ToString());
                childForm.nLabelSw = 1;
                childForm.nLabelTypeID = 4;
                childForm.Show();
            }
        }        

        private void LabelESign_Activated(object sender, EventArgs e)
        {
            LoadApprovalESign();            
        }       
     
                    
    }
}
