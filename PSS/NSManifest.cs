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
    public partial class NSManifest : Form
    {
        public NSManifest()
        {
            InitializeComponent();
        }

        private void Manifest_Load(object sender, EventArgs e)
        {
            LoadCurrent();
            LoadPrevious();
        }

        private void LoadCurrent()
        {
            SqlConnection sqlcnn = GISClass.DBConnection.MDFConnection("GLSQL02", "GISdb",  true , "", "", "");
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                SendKeys.Send("{F12}");
            }
            DataSet sqlds = new DataSet();
            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT FillCode, NSCase, S1001, S1002, S1003, S1004, S1005, S1006, S1007, S1008, S1009, S1010, S1011, S1012, S1013, S1014, S1015, S1016, S1017, S1018, S1019, S1020," +
                                                      "S1021, S1022, S1023, S1024, S1025, S1026, S1027, S1028, S1029, S1030, S1031, S1032, S1033, S1034, S1035, S1036, S1037, S1038, S1039, S1040, S1041, S1042,"+
                                                      "S1043, S1044, S1045, S1046, S1047, S1048, S1049, S1050, S1051, S1052, S1053, S1054, S1055, S1056, S1057, S1058, S1059, S1060, S1061, S1062, S1063, S1064," +
                                                      "S1065, S1066, S1067, S1068, S1069, S1070, S1071, S1072, S1073, S1074, S1075, S1076, S1077, S1078, S1080, S1081, S1082, S1083, S1084, S1085, S1086, S1087," +
                                                      "S1088, S1089, S1090, S1091, S1092, S1093, S1094, S1095, S1096, S1097, S1103 " +
                                                      "FROM NSManifest ORDER BY FillCode", sqlcnn); 
            sqlda.Fill(sqlds, "NSCurrent");
            sqlcnn.Close();

            bsCurrent.DataSource = sqlds.Tables["NSCurrent"];
            dgvCurrent.DataSource = bsCurrent;

            //dgvCurrent.DataSource = sqlds.Tables["NSCurrent"];

            dgvCurrent.Columns[0].Width = 90;
            for (int i = 1; i < dgvCurrent.Columns.Count; i++)
            {
                dgvCurrent.Columns[i].Width = 50;
                dgvCurrent.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;  
            }
            txtCurrent.Text = dgvCurrent.Rows.Count.ToString();
            dgvCurrent.EnableHeadersVisualStyles = false;
            dgvCurrent.Columns[1].Frozen = true;
        }

        private void LoadPrevious()
        {
            SqlConnection sqlcnn = GISClass.DBConnection.MDFConnection("GLSQL02", "GISdb", true, "", "", "");
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                SendKeys.Send("{F12}");
            }
            DataSet sqlds = new DataSet();
            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT FillCode, NSCase, S1001, S1002, S1003, S1004, S1005, S1006, S1007, S1008, S1009, S1010, S1011, S1012, S1013, S1014, S1015, S1016, S1017, S1018, S1019, S1020," +
                                                      "S1021, S1022, S1023, S1024, S1025, S1026, S1027, S1028, S1029, S1030, S1031, S1032, S1033, S1034, S1035, S1036, S1037, S1038, S1039, S1040, S1041, S1042,"+
                                                      "S1043, S1044, S1045, S1046, S1047, S1048, S1049, S1050, S1051, S1052, S1053, S1054, S1055, S1056, S1057, S1058, S1059, S1060, S1061, S1062, S1063, S1064," + 
                                                      "S1065, S1066, S1067, S1068, S1069, S1070, S1071, S1072, S1073, S1074, S1075, S1076, S1077, S1078, S1080, S1081, S1082, S1083, S1084, S1085, S1086," +
                                                      "S1087, S1088, S1089, S1090, S1091, S1092, S1093, S1094, S1095, S1096, S1097, S1103 " +
                                                      "FROM NSManifest_010915 ORDER BY FillCode", sqlcnn);//_110813
            sqlda.Fill(sqlds, "NSPrevious");
            sqlcnn.Close();

            bsPrevious.DataSource = sqlds.Tables["NSPrevious"];
            dgvPrevious.DataSource = bsPrevious;

            //dgvPrevious.DataSource = sqlds.Tables["NSPrevious"];

            dgvPrevious.Columns[0].Width = 90;

            for (int i = 1; i < dgvPrevious.Columns.Count; i++)
            {
                dgvPrevious.Columns[i].Width = 50;
                dgvPrevious.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            txtPrevious.Text = dgvPrevious.Rows.Count.ToString();
            dgvPrevious.EnableHeadersVisualStyles = false;
            dgvPrevious.Columns[1].Frozen = true;
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            int nPrev = 0;
            int nCurr = 0;
            int nUpd = 0;
            int x;
            for (int i = 0; i < dgvPrevious.Rows.Count; i++)
            {

                int idx = bsCurrent.Find("FillCode", dgvPrevious.Rows[i].Cells[0].Value.ToString());
                if (idx >= 0)
                {
                    x = 0;
                    for (int j = 1; j < dgvCurrent.Columns.Count; j++)
                    {
                        if (dgvPrevious.Rows[i].Cells[j].Value.ToString() != dgvCurrent.Rows[idx].Cells[j].Value.ToString())
                        {
                            dgvPrevious.Rows[i].Cells[j].Style.BackColor = Color.Red;
                            dgvPrevious.Rows[i].Cells[j].Style.ForeColor = Color.White;
                            x = 1;
                        }
                    }
                    if (x == 1)
                    {
                        DataGridViewRow nRow = new DataGridViewRow();

                        nRow.CreateCells(dgvUpdated);
                        nRow.Cells[0].Value = dgvPrevious.Rows[i].Cells[0].Value.ToString();
                        nRow.Cells[1].Value = i;
                        dgvUpdated.Rows.Add(nRow);
                        nUpd += 1;                    
                    }
                }
                else
                {
                    dgvPrevious.Rows[i].Selected = true;

                    DataGridViewRow newRow = new DataGridViewRow();

                    newRow.CreateCells(dgvNoMatchPrevious);
                    newRow.Cells[0].Value = dgvPrevious.Rows[i].Cells[0].Value.ToString();
                    newRow.Cells[1].Value = i;
                    dgvNoMatchPrevious.Rows.Add(newRow);
                    nPrev+=1;
                }
            }

            txtNMPrevious.Text = nPrev.ToString();

            for (int i = 0; i < dgvCurrent.Rows.Count; i++)
            {

                int idx = bsPrevious.Find("FillCode", dgvCurrent.Rows[i].Cells[0].Value.ToString());
                if (idx >= 0)
                {
                    //bsCurrent.Position = idx;
                    //dgvCurrent.Rows[idx].Selected = true;
                }
                else
                {
                    dgvCurrent.Rows[i].Selected = true;

                    DataGridViewRow newRow = new DataGridViewRow();

                    newRow.CreateCells(dgvNoMatchCurrent);
                    newRow.Cells[0].Value = dgvCurrent.Rows[i].Cells[0].Value.ToString();
                    newRow.Cells[1].Value = i;
                    dgvNoMatchCurrent.Rows.Add(newRow);
                    nCurr += 1;
                }
            }
            txtNMCurrent.Text = nCurr.ToString();
            txtUpdated.Text = nUpd.ToString();
            MessageBox.Show("Process completed.");
        }

        private void btnExportPrevious_Click(object sender, EventArgs e)
        {
            {
                // creating Excel Application 
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                // creating new WorkBook within Excel application 
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                // creating new Excelsheet in workbook 

                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                // see the excel sheet behind the program 
                app.Visible = true;

                // get the reference of first sheet. By default its name is Sheet1. 

                // store its reference to worksheet 

                worksheet = workbook.Sheets["Sheet1"];

                worksheet = workbook.ActiveSheet;



                // changing the name of active sheet 

                worksheet.Name = "Exported from gridview";

                // storing header part in Excel 

                for (int i = 1; i < dgvPrevious.Columns.Count + 1; i++)
                {

                    worksheet.Cells[1, i] = dgvPrevious.Columns[i - 1].HeaderText;

                }

                // storing Each row and column value to excel sheet 

                for (int i = 0; i < dgvPrevious.Rows.Count - 1; i++)
                {

                    for (int j = 0; j < dgvPrevious.Columns.Count; j++)
                    {

                        worksheet.Cells[i + 2, j + 1] = dgvPrevious.Rows[i].Cells[j].Value.ToString();

                    }

                }

                // save the application 

                workbook.SaveAs("c:\\Previous.xls", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                // Exit from the application 

                app.Quit();
            } 

        }

        private void btnFixX_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = GISClass.DBConnection.MDFConnection("GLSQL02", "GISdb", true, "", "", "");
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                SendKeys.Send("{F12}");
            }

            SqlCommand sqlcmd = new SqlCommand("UPDATE NSManifest_Temp SET " +
                                               "S1001=LTRIM(RTRIM(S1001)),S1002=LTRIM(RTRIM(S1002)),S1003=LTRIM(RTRIM(S1003)),S1004=LTRIM(RTRIM(S1004)),S1005=LTRIM(RTRIM(S1005)),S1006=LTRIM(RTRIM(S1006)),S1007=LTRIM(RTRIM(S1007)),S1008=LTRIM(RTRIM(S1008)),S1009=LTRIM(RTRIM(S1009)),S1010=LTRIM(RTRIM(S1010))," +
                                                "S1011=LTRIM(RTRIM(S1011)),S1012=LTRIM(RTRIM(S1012)),S1013=LTRIM(RTRIM(S1013)),S1014=LTRIM(RTRIM(S1014)),S1015=LTRIM(RTRIM(S1015)),S1016=LTRIM(RTRIM(S1016)),S1017=LTRIM(RTRIM(S1017)),S1018=LTRIM(RTRIM(S1018)),S1019=LTRIM(RTRIM(S1019)),S1020=LTRIM(RTRIM(S1020))," +
                                                "S1021=LTRIM(RTRIM(S1021)),S1022=LTRIM(RTRIM(S1022)),S1023=LTRIM(RTRIM(S1023)),S1024=LTRIM(RTRIM(S1024)),S1025=LTRIM(RTRIM(S1025)),S1026=LTRIM(RTRIM(S1026)),S1027=LTRIM(RTRIM(S1027)),S1028=LTRIM(RTRIM(S1028)),S1029=LTRIM(RTRIM(S1029)),S1030=LTRIM(RTRIM(S1030))," +
                                                "S1031=LTRIM(RTRIM(S1031)),S1032=LTRIM(RTRIM(S1032)),S1033=LTRIM(RTRIM(S1033)),S1034=LTRIM(RTRIM(S1034)),S1035=LTRIM(RTRIM(S1035)),S1036=LTRIM(RTRIM(S1036)),S1037=LTRIM(RTRIM(S1037)),S1038=LTRIM(RTRIM(S1038)),S1039=LTRIM(RTRIM(S1039)),S1040=LTRIM(RTRIM(S1040))," +
                                                "S1041=LTRIM(RTRIM(S1041)),S1042=LTRIM(RTRIM(S1042)),S1043=LTRIM(RTRIM(S1043)),S1044=LTRIM(RTRIM(S1044)),S1045=LTRIM(RTRIM(S1045)),S1046=LTRIM(RTRIM(S1046)),S1047=LTRIM(RTRIM(S1047)),S1048=LTRIM(RTRIM(S1048)),S1049=LTRIM(RTRIM(S1049)),S1050=LTRIM(RTRIM(S1050))," +
                                                "S1051=LTRIM(RTRIM(S1051)),S1052=LTRIM(RTRIM(S1052)),S1053=LTRIM(RTRIM(S1053)),S1054=LTRIM(RTRIM(S1054)),S1055=LTRIM(RTRIM(S1055)),S1056=LTRIM(RTRIM(S1056)),S1057=LTRIM(RTRIM(S1057)),S1058=LTRIM(RTRIM(S1058)),S1059=LTRIM(RTRIM(S1059)),S1060=LTRIM(RTRIM(S1060))," +
                                                "S1061=LTRIM(RTRIM(S1061)),S1062=LTRIM(RTRIM(S1062)),S1063=LTRIM(RTRIM(S1063)),S1064=LTRIM(RTRIM(S1064)),S1065=LTRIM(RTRIM(S1065)),S1066=LTRIM(RTRIM(S1066)),S1067=LTRIM(RTRIM(S1067)),S1068=LTRIM(RTRIM(S1068)),S1069=LTRIM(RTRIM(S1069)),S1070=LTRIM(RTRIM(S1070))," +
                                                "S1071=LTRIM(RTRIM(S1071)),S1072=LTRIM(RTRIM(S1072)),S1073=LTRIM(RTRIM(S1073)),S1074=LTRIM(RTRIM(S1074)),S1075=LTRIM(RTRIM(S1075)),S1076=LTRIM(RTRIM(S1076)),S1077=LTRIM(RTRIM(S1077)),S1078=LTRIM(RTRIM(S1078)),S1080=LTRIM(RTRIM(S1080)),S1081=LTRIM(RTRIM(S1081))," +
                                                "S1082=LTRIM(RTRIM(S1082)),S1083=LTRIM(RTRIM(S1083)),S1084=LTRIM(RTRIM(S1084)),S1085=LTRIM(RTRIM(S1085)),S1086=LTRIM(RTRIM(S1086)),S1087=LTRIM(RTRIM(S1087)),S1088=LTRIM(RTRIM(S1088)),S1089=LTRIM(RTRIM(S1089)),S1090=LTRIM(RTRIM(S1090)),S1091=LTRIM(RTRIM(S1091))," +
                                                "S1092=LTRIM(RTRIM(S1092)),S1093=LTRIM(RTRIM(S1093)),S1094=LTRIM(RTRIM(S1094)),S1095=LTRIM(RTRIM(S1095)),S1096=LTRIM(RTRIM(S1096)),S1097=LTRIM(RTRIM(S1097)),S1103=LTRIM(RTRIM(S1103))", sqlcnn);
            SqlDataReader sqlDR = sqlcmd.ExecuteReader();
            sqlDR.Close(); sqlDR.Dispose();
            sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
            MessageBox.Show("Process completed.");
        }

        private void btnNSCase_Click(object sender, EventArgs e)
        {          
            SqlConnection sqlcnn = GISClass.DBConnection.MDFConnection("GLSQL02", "GISdb", true, "", "", "");
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                SendKeys.Send("{F12}");
            }

            SqlCommand sqlcmd = new SqlCommand();
            string strFC = ""; string strNS = ""; string strQ = "";
            for (int i = 0; i < dgvPrevious.Rows.Count; i++)
            {
                strFC = dgvPrevious.Rows[i].Cells[0].Value.ToString();
                strNS = dgvPrevious.Rows[i].Cells[1].Value.ToString();
                strQ = "UPDATE NSManifest_Temp SET NSCase='" + strNS.Trim() + "' " +
                       "WHERE FillCode='" + strFC.Trim() + "'";
                //if (strFC == "06442105")
                //    MessageBox.Show("Wait");
                sqlcmd.Connection = sqlcnn;
                sqlcmd.CommandText = strQ;
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.ExecuteNonQuery();
            }
            MessageBox.Show("Process completed.",Application.ProductName,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
