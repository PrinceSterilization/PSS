using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

namespace PSS
{
    public partial class AuditRpt : Form
    {
        public string rptName;
        public DateTime dteFrom;
        public DateTime dteTo;
        public Int32 pLogNo;
        public DataTable dt;
        
        private ReportDocument crDoc;

        public AuditRpt()
        {
            InitializeComponent();
        }

        private void AuditRpt_Load(object sender, EventArgs e)
        {
            CreateReport();
        }

        private void CreateReport()
        {
            crDoc = new ReportDocument();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlCommand sqlcmd = new SqlCommand();
            if (rptName == "Audit LogMaster")
            {
                //DataTable dtAudit = new DataTable();
                //dtAudit.Columns.Add("KeyDataName", typeof(string));
                //dtAudit.Columns.Add("KeyDataValue", typeof(string));
                //dtAudit.Columns.Add("DataName", typeof(string));
                //dtAudit.Columns.Add("ActionTaken", typeof(string));
                //dtAudit.Columns.Add("OldValue", typeof(string));
                //dtAudit.Columns.Add("NewValue", typeof(string));
                //dtAudit.Columns.Add("DateDone", typeof(string));
                //dtAudit.Columns.Add("DoneBy", typeof(string));

                //DataTable dtAuLog = PSSClass.AuditReport.AuLogMaster(pLogNo);//dteFrom, dteTo
                //DataTable dtAuLogGIS = PSSClass.AuditReport.AuLogMasterGIS(pLogNo);//dteFrom, dteTo

                //if (dtAuLog != null && dtAuLog.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtAuLog.Rows.Count; i++)
                //    {
                //        for (int j = 0; j < dtAuLogGIS.Rows.Count; j++)
                //        {
                //            if (dtAuLog.Rows[i]["GBLNo"].ToString() == dtAuLogGIS.Rows[j]["GBLNo"].ToString())
                //            {
                //                for (int k = 0; k < dtAuLog.Columns.Count; k++)
                //                {
                //                    if (k < dtAuLog.Columns.Count - 3)
                //                    {
                //                        if (dtAuLog.Rows[i][k].ToString() != dtAuLogGIS.Rows[i][k].ToString())
                //                        {
                //                            DataRow dR = dtAudit.NewRow();
                //                            dR["KeyDataName"] = "GBLNo";
                //                            dR["KeyDataValue"] = dtAuLog.Rows[i]["GBLNo"].ToString();
                //                            dR["DataName"] = dtAuLog.Columns[k];
                //                            dR["OldValue"] = dtAuLog.Rows[i][k];
                //                            dR["NewValue"] = dtAuLogGIS.Rows[i][k];
                //                            dR["ActionTaken"] = dtAuLog.Rows[i]["FileMaintCode"].ToString();
                //                            dR["Datedone"] = dtAuLog.Rows[i]["FileMaintDate"].ToString();
                //                            dR["DoneBy"] = dtAuLog.Rows[i]["FileMaintByID"].ToString();
                //                            dtAudit.Rows.Add(dR);
                //                        }
                //                    }
                //                }
                //                break;
                //            }
                //        }
                //    }
                //}
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AuditLogMaster.rpt";
                crDoc.Load(rpt);
                if (dteFrom.ToShortDateString() == dteTo.ToShortDateString())
                    crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE PERIOD: " + dteFrom.ToShortDateString() + "'";
                else
                    crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE PERIOD: " + dteFrom.ToShortDateString() + " - " + dteTo.ToShortDateString() + "'";
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
            else if (rptName == "Audit Products")
            {
                string rpt = @"\\PSAPP01\IT Files\PTS\Crystal Reports\" + "AuditProducts.rpt";
                crDoc.Load(rpt);
                if (dteFrom.ToShortDateString() == dteTo.ToShortDateString())
                    crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE PERIOD: " + dteFrom.ToShortDateString() + "'";
                else
                    crDoc.DataDefinition.FormulaFields["cPeriod"].Text = "'" + "FOR THE PERIOD: " + dteFrom.ToShortDateString() + " - " + dteTo.ToShortDateString() + "'";
                crDoc.SetDataSource(dt);
                crReport.ReportSource = crDoc;
                crReport.Refresh();
            }
        }

        private void AuditRpt_FormClosing(object sender, FormClosingEventArgs e)
        {
            crDoc.Close(); crDoc.Dispose();
            this.Dispose();
        }
    }
}
