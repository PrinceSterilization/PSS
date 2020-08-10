using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

using System.Data.OleDb; 

namespace GIS
{
    public partial class PenfordManifest : GIS.TemplateForm
    {        
        int nCtr = 0;
        private string strFileAccess = "RO";

        DataTable dtManifest = new DataTable();                                         // MY 04/29/2015 - GridView Manifestice Master table
        
        public PenfordManifest()
        {
            InitializeComponent();

            strFileAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "Manifest");

            BuildPrintItems();

            tsbExit.Click += new EventHandler(CloseClickHandler);

            tsbAdd.Enabled = false;
            tsbEdit.Enabled = false;
            tsbFilter.Enabled = false;
            tsbRefresh.Enabled = false;
            tsbSearch.Enabled = false;           
            tsddbSearch.Enabled = false;

            btnView.Enabled = false;
            btnLoad.Enabled = false;
        }

        private void FileAccess()
        {
            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; //tsbDelete.Enabled = true;
            }
        }

        private void CloseClickHandler(object sender, EventArgs e)
        {           
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlRecord.Visible = false; btnClose.Visible = false; this.Dispose();
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "ManifestExList";
            items[0].Text = "Manifest Exception List";
            items[0].Click += new EventHandler(PrintManifestExceptionListClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void PrintManifestExceptionListClickHandler(object sender, EventArgs e)
        {
            ManifestRpt rpt = new ManifestRpt();   
            rpt.WindowState = FormWindowState.Maximized;

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog(); 
            this.openFileDialog1.InitialDirectory = @"C:\";
            this.openFileDialog1.Filter = "Excel|*.xls|Excel 2010|*.xlsx";          
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "SELECT MANIFEST FILE";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtName.Text = openFileDialog1.FileName;
                btnView.Enabled = true;
                btnLoad.Enabled = true;
                btnBrowse.Enabled = false;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Manifest file to view!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }
            System.Diagnostics.Process.Start(@txtName.Text);
        }

        private void ReadExcelFile()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please choose a Manifest file!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            String path = txtName.Text.Trim();

            String connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;";

            //The connection to that file
            OleDbConnection conn = new OleDbConnection(connStr);

            //The query
            string strSQL = "SELECT * FROM [Penford Micro Testing$]";

            //The command 
            OleDbCommand cmd = new OleDbCommand(/*The query*/strSQL, /*The connection*/conn);
            DataTable dT = new DataTable();
            conn.Open();

            try
            {
                OleDbDataReader dR = cmd.ExecuteReader();
                dT.Load(dR);
                bS.DataSource = dT;
                dgvManifest.DataSource = bS;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void DeleteManifest(Int16 cMCode)
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcmd.Connection = sqlcnn;            
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spDelManifest";
            sqlcmd.Parameters.AddWithValue("ManifestCode", cMCode);
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }      

        private void SaveManifest(string cFillCode, string cServiceDesc, Int16 cMCode)
        {
            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@FillCode", cFillCode);
            sqlcmd.Parameters.AddWithValue("@ServiceDesc", cServiceDesc);
            sqlcmd.Parameters.AddWithValue("@ManifestCode", cMCode);
            sqlcmd.Parameters.AddWithValue("@CreatedByID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddManifest";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return;
            }
            sqlcnn.Dispose();
        }

        private void CreateBaseManifest()
        {
            int intCount = 0;
           
            // get ahead of time non empty Fill code reccount for pbload value calculation
            for (int j = 1; j < dgvManifest.RowCount; j++)
            {
                if (dgvManifest.Rows[intCount].Cells[1].Value.ToString() != "")
                {
                    intCount++;
                }
            }

            for (int j = 1; j < intCount; j++)
            {
                for (int i = 2; i < dgvManifest.ColumnCount; i++)
                {
                    if (dgvManifest.Rows[j].Cells[i].Value.ToString().ToUpper() == "X")
                    {
                        SaveManifest(dgvManifest.Rows[j].Cells[1].Value.ToString(),
                                     dgvManifest.Rows[0].Cells[i].Value.ToString(), 2);
                    }
                }
                pbLoad.Value = (int)((100 * j) / intCount);
            }                    
        }    
 
        private void btnLoad_Click(object sender, EventArgs e)
        {
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to load this manifest?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {                
                btnView.Enabled = false;
                btnLoad.Enabled = false;
                pbLoad.Visible = true;
                lblProgress.Visible = true;
                ReadExcelFile();
                DeleteManifest(2);               
                timerLoad.Start();
            }
        }

        private void timerLoad_Tick(object sender, EventArgs e)
        {
            if (nCtr == 2)
            {
                timerLoad.Enabled = false;
                lblProgress.Visible = true;
                pbLoad.Visible = true;             
                CreateBaseManifest();
                timerLoad.Stop();
                lblProgress.Visible = false;
                pbLoad.Visible = false;
                btnBrowse.Enabled = true;
                MessageBox.Show("Manifest successfully loaded!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            nCtr++;
        }
    }
}