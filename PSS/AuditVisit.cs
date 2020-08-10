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
    public partial class AuditVisit : Form
    {
        private int nATimer = 0, nAudit = 0, nVTimer = 0, nVisit = 0, nCtr = 1;

        public AuditVisit()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close(); this.Dispose();
        }

        private void AuditVisit_Load(object sender, EventArgs e)
        {
            //DateTime dy = Convert.ToDateTime("8/18/15");

            string strStartDate = "", strEndDate = "";

            DataTable dtAudits = new DataTable();
            DataTable dtVisits = new DataTable();

            DateTime dtStartWeek = DateTime.Now.AddDays((int)DateTime.Now.DayOfWeek * -1);
            DateTime dtEndWeek = dtStartWeek.AddDays(6);

            strStartDate = dtStartWeek.ToShortDateString();
            strEndDate = dtEndWeek.ToShortDateString();

            dtStartWeek = Convert.ToDateTime(strStartDate);
            dtEndWeek = Convert.ToDateTime(strEndDate);
                
            //DateTime startOfWeek =  dy.AddDays((int)dy.DayOfWeek * -1);
            //DateTime endOfWeek = startOfWeek.AddDays(6);


            //Check if there is an Audit Schedule today
            string strDate = DateTime.Now.ToShortDateString();
            DataTable dtAuditDay = new DataTable();
            dtAuditDay = PSSClass.Visits.VisitAuditDay(Convert.ToDateTime(strDate));
            if (dtAuditDay != null && dtAuditDay.Rows.Count > 0)
            {
                dgvAudits.DataSource = dtAuditDay; 
                lblAudit.Text = "We have an audit scheduled for today!";
                nAudit = 1;
                if (timer1.Enabled == false)
                    timer1.Enabled = true;
            }
            else
            {
                dtAudits = PSSClass.Visits.VisitAuditWeek(dtStartWeek, dtEndWeek);
                dgvAudits.DataSource = dtAudits;
            }

            dgvAudits.Columns["VisitDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvAudits.Columns["VisitDate"].Width = 90;
            dgvAudits.Columns["VisitDate"].HeaderText = "DATE";
            dgvAudits.Columns["Visitor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvAudits.Columns["Visitor"].Width = 360;
            dgvAudits.Columns["Visitor"].HeaderText = "CLIENT / INSTITUTION";
            DGVSetting(dgvAudits);

            //Client Visit
            DataTable dtVisitDay = new DataTable();
            dtVisitDay = PSSClass.Visits.VisitDay(Convert.ToDateTime(strDate));
            if (dtVisitDay != null && dtVisitDay.Rows.Count > 0)
            {
                dgvVisits.DataSource = dtVisitDay;
                lblVisit.Text = "We have a client visit scheduled for today!";
                nVisit = 1;
                if (timer1.Enabled == false)
                    timer1.Enabled = true;
            }
            else
            {
                dtVisits = PSSClass.Visits.VisitWeek(dtStartWeek, dtEndWeek);
                dgvVisits.DataSource = dtVisits;
            }

            dgvVisits.Columns["VisitDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvVisits.Columns["VisitDate"].Width = 90;
            dgvVisits.Columns["VisitDate"].HeaderText = "DATE";
            dgvVisits.Columns["Visitor"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvVisits.Columns["Visitor"].Width = 360;
            dgvVisits.Columns["Visitor"].HeaderText = "CLIENT / INSTITUTION";
            DGVSetting(dgvVisits);

            if ((dtAudits == null || dtAudits.Rows.Count == 0)  && (dtAuditDay == null || dtAuditDay.Rows.Count == 0))
            {
                lblAudit.Text = "Audit Schedule (None)";
            }

            if ((dtVisits== null || dtVisits.Rows.Count == 0) && (dtVisitDay == null || dtVisitDay.Rows.Count == 0))
            {
                lblVisit.Text = "Client Visit Schedule (None)";
            }
            this.Location = new Point(10, 80);

            //Temporary Fix for J Mastej (114), Marlyn Moreno 
            //===============================================
            if (LogIn.nUserID == 114 || LogIn.nUserID == 394)
            {
                float width_ratio = (Screen.PrimaryScreen.Bounds.Width / 1280f);// 1920f 1280f
                float heigh_ratio = (Screen.PrimaryScreen.Bounds.Height / 800f);// //1080f 800f

                SizeF scale = new SizeF(width_ratio, heigh_ratio);
                this.Scale(scale);

                foreach (Control control in this.Controls)
                {
                    control.Font = new Font("Arial", control.Font.SizeInPoints * heigh_ratio * width_ratio);
                }
            }
        }

        public void DGVSetting(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, GraphicsUnit.Point);
            dgv.DefaultCellStyle.Font = new Font("Arial", 10, GraphicsUnit.Point);
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Random rand = new Random();
            //for (int i = 0; i < 255; i++)
            //{
            //    int A = rand.Next(i);
            //    int R = rand.Next(i);
            //    int G = rand.Next(i);
            //    int B = rand.Next(i);
            //    lblAudit.ForeColor = Color.FromArgb(A, R, G, B);
            //}
            if (nAudit == 1)
            {
                if (nATimer == 0)
                {
                    lblAudit.ForeColor = Color.White;
                    nATimer = 1;
                }
                else
                {
                    lblAudit.ForeColor = Color.Black;
                    nATimer = 0;
                }
            }

            if (nVisit == 1)
            {
                if (nVTimer == 0)
                {
                    lblVisit.ForeColor = Color.White;
                    nVTimer = 1;
                }
                else
                {
                    lblVisit.ForeColor = Color.Black;
                    nVTimer = 0;
                }
            }
            
            lblReminder.Left = nCtr * 25;
            
            if (nCtr > 8)
                nCtr = 1;

            nCtr++;
        }

        private void AuditVisit_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false; 
        }
    }
}
