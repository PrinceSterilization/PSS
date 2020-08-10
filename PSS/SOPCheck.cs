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
    public partial class SOPCheck : Form
    {
        public SOPCheck()
        {
            InitializeComponent();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            dgvSOP.DataSource = null;
            ds = SOPList();
            if (ds == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            //dgvSOP.DataSource = ds.Tables[0];

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string strNo = "1234567890";
                int nL = row["SOPID"].ToString().Length;
                string strSOP = row["SOPID"].ToString().Substring(0,3);
                string strSuffix = row["SOPID"].ToString().Substring(nL - 1, 1);
                string strRevNo = row["RevisionNo"].ToString();
                string strFileName = row["FileName"].ToString();

                int nI = strNo.IndexOf(strSOP.Substring(0, 1));

                if (nI != -1)
                {
                    int nSOP = Convert.ToInt16(strSOP);
                    string strFile = nSOP.ToString() + strSuffix + ".R" + strRevNo + ".doc";
                    if (strFile != strFileName)
                    {
                        dgvSOP.Rows.Add(strFile, strFileName);
                    }
                }
            }
            ds.Dispose();
        }

        private static DataSet SOPList()
        {
            SqlConnection sqlcnn = GISClass.DBConnection.MDFConnection("GBLNJ4", "ATMS", true, "", "", "");
            if (sqlcnn == null)
            {
                sqlcnn.Dispose();
                return null;
            }
            DataSet sqlds = new DataSet();
            SqlDataAdapter sqlda = new SqlDataAdapter("SELECT SOPID, RevisionNo, FileName " +
                                                      "FROM SOPRevisions ORDER BY SOPID,RevisionNo", sqlcnn);
            sqlda.Fill(sqlds, "Revisions");
            try
            {
                sqlcnn.Close(); sqlcnn.Dispose();
                return sqlds;
            }
            catch
            {
                sqlcnn.Dispose();
                return null;
            }
        }

    }
}
