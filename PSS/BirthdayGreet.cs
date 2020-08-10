using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace GIS
{
    public partial class BirthdayGreet : Form
    {
        private int nTimer = 0;

        public BirthdayGreet()
        {
            InitializeComponent();
        }

        private void BirthdayGreet_Load(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToLongDateString();
                
            DataTable dt = GISClass.Employees.BirthdayCelebrants();
            if (dt != null && dt.Rows.Count > 0)
            {
                int n = 0, p = 10;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PictureBox picPhoto = new PictureBox();
                    picPhoto.Location = new System.Drawing.Point(10, 10 + n);
                    picPhoto.Name = "pic";
                    picPhoto.Size = new System.Drawing.Size(100, 100);
                    picPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                    picPhoto.BorderStyle = BorderStyle.Fixed3D;
                    if (System.IO.File.Exists(@"\\gblnj6\gbldata$\GIS\hr\" + dt.Rows[i]["LoginName"].ToString() + ".jpg") == true)
                    {
                        picPhoto.Load(@"\\gblnj6\gbldata$\GIS\hr\" + dt.Rows[i]["LoginName"].ToString() + ".jpg");
                    }
                    else
                    {
                        try
                        {
                            picPhoto.Load(@"\\gblnj6\gbldata$\GIS\hr\Logo.jpg");
                        }
                        catch { }
                    }
                    Label lbl = new Label();
                    lbl.Text = dt.Rows[i]["EmpName"].ToString();
                    lbl.Width = 200;
                    lbl.Font = new Font(lbl.Font, FontStyle.Bold);
                    lbl.BackColor = Color.Transparent;
                    lbl.Location = new System.Drawing.Point(10, 100 + p);
                    pnlPhoto.Controls.Add(picPhoto);
                    pnlPhoto.Controls.Add(lbl);
                    n += 120; p += 120;
                }
                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nTimer == 3)
            {
                timer1.Enabled = false;
                this.Close(); this.Dispose();
            }
            nTimer++;
        }

        private void BirthdayGreet_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
