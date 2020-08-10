using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace PSS
{
    public partial class FinancialRpt : Form
    {
        private int nTimer = 0, nRNo = 0;
        string strReportName;
        public FinancialRpt()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            this.openFileDialog1.Filter = 
                "Excel (*.xls;*.xlsx;)" +
                "All files (*.*)|*.*";// Set the file dialog to filter for files.

            this.openFileDialog1.Multiselect = false;/// do not allow the user to select multiple images. 
            this.openFileDialog1.Title = "SELECT FILE";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtName.Text = openFileDialog1.FileName;
                lnkFile.Text = openFileDialog1.FileName;


                string strFile = "<File><FileName>";
                string strEsc = txtName.Text.Trim();
                strEsc = strEsc.Replace("&", "&amp;");
                strEsc = strEsc.Replace(">", "&gt;");
                strEsc = strEsc.Replace("<", "&lt;");
                strEsc = strEsc.Replace("'", "&apos;");
                strEsc = strEsc.Replace("\"", "&quot;");
                strFile = strFile + strEsc + "</FileName></File>";

                File.WriteAllText(Application.StartupPath + "\\FinPath" + ".xml", strFile, Encoding.ASCII);
            }
        }

        private void lnkFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkFile.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void linkAR_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(lnkAR.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnAR_Click(object sender, EventArgs e)
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            this.openFileDialog1.Filter =
                "Excel (*.xls;*.xlsx;)" +
                "All files (*.*)|*.*";// Set the file dialog to filter for files.
            this.openFileDialog1.Multiselect = false; // Do not allow the user to select multiple images.
            this.openFileDialog1.Title = "SELECT FILE";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtAR.Text = openFileDialog1.FileName;
                lnkAR.Text = openFileDialog1.FileName;

                string strFile = "<File><FileName>";
                string strEsc = txtAR.Text.Trim();
                strEsc = strEsc.Replace("&", "&amp;");
                strEsc = strEsc.Replace(">", "&gt;");
                strEsc = strEsc.Replace("<", "&lt;");
                strEsc = strEsc.Replace("'", "&apos;");
                strEsc = strEsc.Replace("\"", "&quot;");
                strFile = strFile + strEsc + "</FileName></File>";

                File.WriteAllText(Application.StartupPath + "\\ARPath" + ".xml", strFile, Encoding.ASCII);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (nTimer == 0)
            {
                nTimer = 1;
                timer1.Enabled = false;
                GenerateReport(strReportName);
                lblProgress.Visible = false;
            }
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            nTimer = 0; timer1.Enabled = true; nRNo = 1; lblProgress.Visible = true; strReportName = "PSSFinancial";
        }

        private void GenerateReport(string strRpt)
        {
            try
            {

                MgmtRpts rpt = new MgmtRpts();
                rpt.rptName = strRpt;
                rpt.nYr = Convert.ToInt16(cboFSYear.Text);

                if (strRpt == "PSSFinancial")
                {
                    string strIndex = cboMonths.SelectedIndex.ToString();
                    int intMonth = Convert.ToInt32(DateTime.Today.Month);
                    if (cboMonths.SelectedIndex != -1)
                    {
                        intMonth = cboMonths.SelectedIndex + 1;
                    }

                    rpt.nMo = intMonth;
                    if (chkSummary.Checked)
                        rpt.nFSFormat = 1;
                    else
                    {
                        if (rdoAcctgF1.Checked == true)
                            rpt.nFSFormat = 2;
                        else if (rdoAcctgF2.Checked == true)
                            rpt.nFSFormat = 3;
                        else if (rdoStdFormat.Checked == true)
                            rpt.nFSFormat = 0;
                    }
                }                

                rpt.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lblProgress_Click(object sender, EventArgs e)
        {
            
        }

        private void btnRptDetails_Click(object sender, EventArgs e)
        {
            nTimer = 0; timer1.Enabled = true; lblProgress.Visible = true; nRNo = 1; strReportName = "PSSFinancial_12Month";
        }

        private void FinancialRpt_Load(object sender, EventArgs e)
        {
            string strXML = Application.StartupPath + "\\FinPath"+ ".xml";
            DataSet dsXML = new DataSet();
            DataView dvwXML;
            try
            {
                dsXML.ReadXml(strXML);
                dvwXML = new DataView(dsXML.Tables[0]);
                txtName.Text = dvwXML[0].Row["FileName"].ToString();
                dsXML.Clear(); dsXML.Dispose(); dvwXML.Dispose();
                lnkFile.Text = txtName.Text;
            }
            catch {}

            strXML = Application.StartupPath + "\\ARPath" + ".xml";
            DataSet dsXMLAR = new DataSet();
            DataView dvwXMLAR;
            try
            {
                dsXMLAR.ReadXml(strXML);
                dvwXMLAR = new DataView(dsXMLAR.Tables[0]);
                txtAR.Text = dvwXMLAR[0].Row["FileName"].ToString();
                dsXMLAR.Clear(); dsXMLAR.Dispose(); dvwXMLAR.Dispose();
                lnkAR.Text = txtAR.Text;
            }
            catch { }
            int nY = DateTime.Now.Year;
            for (int i = 1; i < 8; i++)
            {
                cboFSYear.Items.Add(nY.ToString());
                nY--;
            }
            cboFSYear.SelectedIndex = 0;
        }
    }
}
