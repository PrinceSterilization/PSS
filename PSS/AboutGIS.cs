using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;

namespace GIS
{
    public partial class AboutGIS : Form
    {
        Video vidGIS;

        public AboutGIS()
        {
            InitializeComponent();
        }

        private void Help_Load(object sender, EventArgs e)
        {

            int width = pnlVideo.Width;
            int height = pnlVideo.Height;
            pnlVideo.Size = new Size(width, height);
            // load the selected video file 
            //vidGIS = new Video(Application.StartupPath + "\\gbl wars.wmv");
            vidGIS = new Video(@"\\gblnj4\d$\GIS\videos\gbl wars.wmv");
            // set the panel as the video object’s owner 
            vidGIS.Owner = pnlVideo;
            if (vidGIS.State != StateFlags.Running)
            {
                vidGIS.Play();
            }
            //vidGIS.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // store the original size of the panel 
                int width = pnlVideo.Width;
                int height = pnlVideo.Height;
                pnlVideo.Size = new Size(width, height);
                // load the selected video file 
                vidGIS = new Video(openFileDialog1.FileName);
                // set the panel as the video object’s owner 
                vidGIS.Owner = pnlVideo;
                vidGIS.Stop();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (vidGIS.State != StateFlags.Running)  
            {
                vidGIS.Play();  
            }
        }

        private void Help_FormClosing(object sender, FormClosingEventArgs e)
        {
            vidGIS.Stop();
            vidGIS.Dispose();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "Pause")
            {
                vidGIS.Pause();
                btnPause.Text = "Play";
            }
            else
            {
                vidGIS.Play();
                btnPause.Text = "Pause";
            }
        }
    }
}
