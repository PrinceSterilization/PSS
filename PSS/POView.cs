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
    public partial class POView : Form
    {
        public string imgFile = "";

        public POView()
        {
            InitializeComponent();
        }

        private void POView_Load(object sender, EventArgs e)
        {
            picPO.Load(imgFile);
        }
    }
}
