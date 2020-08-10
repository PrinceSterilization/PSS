using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GIS
{
    public partial class ChangeBackground : Form
    {
        public ChangeBackground()
        {
            InitializeComponent();
        }

        private void cboBackgrounds_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlBackground.BackgroundImage = Image.FromFile(@"\\gblnj4\GIS\Version\Images\" + cboBackgrounds.Text + ".jpg");
        }

        private void ChangeBackground_Load(object sender, EventArgs e)
        {
            pnlBackground.BackgroundImageLayout = ImageLayout.Stretch;
            pnlBackground.BackgroundImage = Image.FromFile(Application.StartupPath + @"\GIS Background.jpg");
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            File.Copy(@"\\gblnj4\GIS\Version\Images\" + cboBackgrounds.Text + ".jpg", Application.StartupPath + @"\GIS Background New.jpg", true);
            MessageBox.Show("Please logout then login back to GIS to effect the changes.",Application.ProductName);
            //this.MdiParent.BackgroundImage = Image.FromFile(Application.StartupPath + @"\GIS Background New.jpg");
            this.Dispose();
        }
    }
}
