using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PSS
{
    public partial class TATDashBoard : Form
    {
        public DateTime dteRangeFrom;
        public DateTime dteRangeTo;

        private DataTable dtDataSource = new DataTable();
        private DataTable dtChart = new DataTable();
        private DataTable dtChartWW = new DataTable();
        private DataTable dtTATChart = new DataTable();

        private DataTable dtSponsors = new DataTable();
        private DataTable dtSC = new DataTable();
        private Int16 nCtr = 0;
        private BackgroundWorker bw = new BackgroundWorker();
        private byte nLoadSw = 0;
        private string priGrpCode = "";
        private Int16 nDuration = 0;

        public TATDashBoard()
        {
            InitializeComponent();

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;

            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            DGVSetting(dgvDataSource);
            DGVSetting(dgvSponsors);
            DGVSetting(dgvSC);
            pnlAllChart.BackgroundImage = new Bitmap(Application.StartupPath + @"\Chart 1.png");
            pnlTATChart.BackgroundImage = new Bitmap(Application.StartupPath + @"\Chart 2.png");
        }

        private void DGVSetting(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void TATDashBoard_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.MinimumSize = new Size(1064, 743);
            dtpFrom.Value = dteRangeFrom;
            dtpTo.Value = dteRangeTo;

            dtSponsors = PSSClass.Sponsors.SponsorNamesDDL();
            dgvSponsors.DataSource = dtSponsors;

            dgvSponsors.Columns[0].Width = 369;
            dgvSponsors.Columns[1].Visible = false;

            dtSC = PSSClass.ServiceCodes.SCDDL();
            if (dtSC == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            dgvSC.DataSource = dtSC;
            DGVSetting(dgvSC);
            dgvSC.Columns[0].Width = 369;
            dgvSC.Columns[1].Visible = false;

            Label lblNotes = new Label();
            lblNotes.Text = Environment.NewLine + Environment.NewLine + 
                            "Categories: " + Environment.NewLine + Environment.NewLine +
                            "  1: Date Received to Proposed Start Date" + Environment.NewLine + Environment.NewLine +
                            "  2: Date Received to Actual Start Date" + Environment.NewLine + Environment.NewLine +
                            "  3: Actual Start Date to Actual End Date" + Environment.NewLine + Environment.NewLine +
                            "  4: Actual End Date to Report Date" + Environment.NewLine + Environment.NewLine +
                            "  5: Report Date to QA Approved Date" + Environment.NewLine + Environment.NewLine +
                            "  6: QA Approved Date to Report Mail Date" + Environment.NewLine + Environment.NewLine +
                            "  7: Report Mail Date to Invoice Date" + Environment.NewLine + Environment.NewLine +
                            "  8: Invoice Date to Invoice Mail Date" + Environment.NewLine + Environment.NewLine +
                            "  9: Date Received to Actual End Date " + Environment.NewLine + Environment.NewLine +
                            "10: Date Received to Invoice Mail Date" + Environment.NewLine + Environment.NewLine +
                            "11: Date Received to Report Mail Date";
            lblNotes.Dock = DockStyle.Fill;
            lblNotes.TextAlign = ContentAlignment.TopLeft;
            pnlNotes.Controls.Add(lblNotes);
            pnlNotes.BringToFront();
            cboGrpCode.SelectedIndex = 0; //cboGrpCode.Visible = true;
        }

        
        private void LoadChart()
        {
            //Setup Chart
            //this.components = new System.ComponentModel.Container();

            Chart chart = new Chart();
            ChartArea chartArea = new ChartArea();

            chart.Dock = System.Windows.Forms.DockStyle.Fill;
            chart.Size = new Size(600, 250);
            chart.Titles.Add(txtSponsor.Text + Environment.NewLine + "SC " + txtSC.Text + " - " + txtSCDesc.Text);
            chart.Titles.Add("Period Covered:" + dtpFrom.Value.ToShortDateString() + " - " + dtpTo.Value.ToShortDateString());

            //Legends
            Legend legend1 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };
            Legend legend2 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };
            //Legend legend3 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };

            //Formats
            //chartArea.AxisX.LabelStyle.Format = "dd/MMM\nhh:mm";
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.Title = "Categories";
            chartArea.AxisY.Title = "No. of Days";

            chart.ChartAreas.Add(chartArea);
            chart.Legends.Add(legend1);
            chart.Legends.Add(legend2);
            //chart.Legends.Add(legend3);
            //series.XValueType = ChartValueType.DateTime;

            // set up some data
            //DataTable dtSource = PSSClass.ManagementReports.TATSummary(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), dtpFrom.Value, dtpTo.Value);
            //if (dtSource != null && dtSource.Rows.Count > 0)
            if (dtChart != null && dtChart.Rows.Count > 0)
                {
                //var xvals = new[]
                //{
                //new DateTime(2012, 4, 4),
                //new DateTime(2012, 4, 5), 
                //new DateTime(2012, 4, 6), 
                //new DateTime(2012, 4, 7)
                //};
                //var yvals = new[] { 1, 3, 7, 12 };

                //Min Series
                Series minSeries = new Series()
                {
                    Name = "Lowest",
                    IsVisibleInLegend = true,
                    Color = System.Drawing.Color.LightSeaGreen,
                    ChartType = SeriesChartType.Column
                };
                chart.Series.Add(minSeries);
                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinRecProp"]));
                var n1 = minSeries.Points[0];
                n1.AxisLabel = "1";
                n1.Color = Color.LightSeaGreen;
                n1.Label = dtChart.Rows[0]["MinRecProp"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinRecOn"]));
                var n2 = minSeries.Points[1];
                n2.AxisLabel = "2";
                n2.Color = Color.LightSeaGreen;
                n2.Label = dtChart.Rows[0]["MinRecOn"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinOnOff"]));
                var n3 = minSeries.Points[2];
                n3.AxisLabel = "3";
                n3.Color = Color.LightSeaGreen;
                n3.Label = dtChart.Rows[0]["MinOnOff"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinOffRep"]));
                var n4 = minSeries.Points[3];
                n4.AxisLabel = "4";
                n4.Color = Color.LightSeaGreen;
                n4.Label = dtChart.Rows[0]["MinOffRep"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinRepQA"]));
                var n5 = minSeries.Points[4];
                n5.AxisLabel = "5";
                n5.Color = Color.LightSeaGreen;
                n5.Label = dtChart.Rows[0]["MinRepQA"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinQAEMail"]));
                var n6 = minSeries.Points[5];
                n6.AxisLabel = "6";
                n6.Color = Color.LightSeaGreen;
                n6.Label = dtChart.Rows[0]["MinQAEMail"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinREMailInv"]));
                var n7 = minSeries.Points[6];
                n7.AxisLabel = "7";
                n7.Color = Color.LightSeaGreen;
                n7.Label = dtChart.Rows[0]["MinREMailInv"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinInvEMail"]));
                var n8 = minSeries.Points[7];
                n8.AxisLabel = "8";
                n8.Color = Color.LightSeaGreen;
                n8.Label = dtChart.Rows[0]["MinInvEMail"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinRecOff"]));
                var n9 = minSeries.Points[8];
                n9.AxisLabel = "9";
                n9.Color = Color.LightSeaGreen;
                n9.Label = dtChart.Rows[0]["MinRecOff"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinRecInvEMail"]));
                var n10 = minSeries.Points[9];
                n10.AxisLabel = "10";
                n10.Color = Color.LightSeaGreen;
                n10.Label = dtChart.Rows[0]["MinRecInvEMail"].ToString();

                minSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MinRecREMail"]));
                var n11 = minSeries.Points[10];
                n11.AxisLabel = "11";
                n11.Color = Color.LightSeaGreen;
                n11.Label = dtChart.Rows[0]["MinRecREMail"].ToString();

                //Max Series
                Series maxSeries = new Series()
                {
                    Name = "Highest",
                    IsVisibleInLegend = true,
                    Color = System.Drawing.Color.Salmon,
                    ChartType = SeriesChartType.Column
                };
                chart.Series.Add(maxSeries);
                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxRecProp"]));
                var m1 = maxSeries.Points[0];
                m1.AxisLabel = "1";
                m1.Color = Color.Salmon;
                m1.Label = dtChart.Rows[0]["MaxRecProp"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxRecOn"]));
                var m2 = maxSeries.Points[1];
                m2.AxisLabel = "2";
                m2.Color = Color.Salmon;
                m2.Label = dtChart.Rows[0]["MaxRecOn"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxOnOff"]));
                var m3 = maxSeries.Points[2];
                m3.AxisLabel = "3";
                m3.Color = Color.Salmon;
                m3.Label = dtChart.Rows[0]["MaxOnOff"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxOffRep"]));
                var m4 = maxSeries.Points[3];
                m4.AxisLabel = "4";
                m4.Color = Color.Salmon;
                m4.Label = dtChart.Rows[0]["MaxOffRep"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxRepQA"]));
                var m5 = maxSeries.Points[4];
                m5.AxisLabel = "5";
                m5.Color = Color.Salmon;
                m5.Label = dtChart.Rows[0]["MaxRepQA"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxQAEMail"]));
                var m6 = maxSeries.Points[5];
                m6.AxisLabel = "6";
                m6.Color = Color.Salmon;
                m6.Label = dtChart.Rows[0]["MaxQAEMail"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxREMailInv"]));
                var m7 = maxSeries.Points[6];
                m7.AxisLabel = "7";
                m7.Color = Color.Salmon;
                m7.Label = dtChart.Rows[0]["MaxREMailInv"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxInvEMail"]));
                var m8 = maxSeries.Points[7];
                m8.AxisLabel = "8";
                m8.Color = Color.Salmon;
                m8.Label = dtChart.Rows[0]["MaxInvEMail"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxRecOff"]));
                var m9 = maxSeries.Points[8];
                m9.AxisLabel = "9";
                m9.Color = Color.Salmon;
                m9.Label = dtChart.Rows[0]["MaxRecOff"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxRecInvEMail"]));
                var m10 = maxSeries.Points[9];
                m10.AxisLabel = "10";
                m10.Color = Color.Salmon;
                m10.Label = dtChart.Rows[0]["MaxRecInvEMail"].ToString();

                maxSeries.Points.Add(Convert.ToDouble(dtChart.Rows[0]["MaxRecREMail"]));
                var m11 = maxSeries.Points[10];
                m10.AxisLabel = "11";
                m11.Color = Color.Salmon;
                m11.Label = dtChart.Rows[0]["MaxRecREMail"].ToString();

                ////Average Series
                //Series series = new Series();
                //series = new Series
                //{
                //    Name = "Average",
                //    IsVisibleInLegend = true,
                //    Color = System.Drawing.Color.CornflowerBlue,
                //    ChartType = SeriesChartType.Column
                //};
                //chart.Series.Add(series);

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgRecProp"]));
                //var a1 = series.Points[0];
                //a1.Color = Color.CornflowerBlue;
                //a1.AxisLabel = "1";
                //a1.Label = dtChart.Rows[0]["AvgRecProp"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgRecOn"]));
                //var a2 = series.Points[1];
                //a2.Color = Color.CornflowerBlue;
                //a2.AxisLabel = "2";
                //a2.Label = dtChart.Rows[0]["AvgRecOn"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgOnOff"]));
                //var a3 = series.Points[2];
                //a3.Color = Color.CornflowerBlue;
                //a3.AxisLabel = "3";
                //a3.Label = dtChart.Rows[0]["AvgOnOff"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgOffRep"]));
                //var a4 = series.Points[3];
                //a4.Color = Color.CornflowerBlue;
                //a4.AxisLabel = "4";
                //a4.LegendText = "OffRep";
                //a4.Label = dtChart.Rows[0]["AvgOffRep"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgRepQA"]));
                //var a5 = series.Points[4];
                //a5.Color = Color.CornflowerBlue;
                //a5.AxisLabel = "5";
                //a5.LegendText = "RepQA";
                //a5.Label = dtChart.Rows[0]["AvgRepQA"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgQAEMail"]));
                //var a6 = series.Points[5];
                //a6.Color = Color.CornflowerBlue;
                //a6.AxisLabel = "6";
                //a6.LegendText = "QAEMail";
                //a6.Label = dtChart.Rows[0]["AvgQAEMail"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgREMailInv"]));
                //var a7 = series.Points[6];
                //a7.Color = Color.CornflowerBlue;
                //a7.AxisLabel = "7";
                //a7.LegendText = "REMailInv";
                //a7.Label = dtChart.Rows[0]["AvgREMailInv"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgInvEMail"]));
                //var a8 = series.Points[7];
                //a8.Color = Color.CornflowerBlue;
                //a8.AxisLabel = "8";
                //a8.LegendText = "InvEMail";
                //a8.Label = dtChart.Rows[0]["AvgInvEMail"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgRecOff"]));
                //var a9 = series.Points[8];
                //a9.Color = Color.CornflowerBlue;
                //a9.AxisLabel = "9";
                //a9.LegendText = "RecOff";
                //a9.Label = dtChart.Rows[0]["AvgRecOff"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgRecInvEMail"]));
                //var a10 = series.Points[9];
                //a10.Color = Color.CornflowerBlue;
                //a10.AxisLabel = "10";
                //a10.LegendText = "RecInvEMail";
                //a10.Label = dtChart.Rows[0]["AvgRecInvEMail"].ToString();

                //series.Points.Add(Convert.ToDouble(dtChart.Rows[0]["AvgRecREMail"]));
                //var a11 = series.Points[10];
                //a11.Color = Color.CornflowerBlue;
                //a11.AxisLabel = "11";
                //a11.LegendText = "RecREMail";
                //a11.Label = dtChart.Rows[0]["AvgRecREMail"].ToString();

                //// bind the datapoints
                //chart.Series["Series1"].Points.DataBindXY(xvals, yvals);

                //// copy the series and manipulate the copy
                //chart.DataManipulator.CopySeriesValues("Series1", "Series2");
                //chart.DataManipulator.FinancialFormula(
                //    FinancialFormula.WeightedMovingAverage,
                //    "Series2"
                //);
                //chart.Series["Series2"].ChartType = SeriesChartType.FastLine;

                // draw!
                chart.Invalidate();
                chart.Dock = DockStyle.Fill;
                pnlAllChart.Controls.Add(chart);
                //// write out a file
                //chart.SaveImage("chart.png", ChartImageFormat.Png);
            }
        }


        private void LoadChartWW()
        {
            //Setup Chart
            //this.components = new System.ComponentModel.Container();

            Chart chart = new Chart();
            ChartArea chartArea = new ChartArea();

            chart.Dock = System.Windows.Forms.DockStyle.Fill;
            chart.Size = new Size(600, 250);
            chart.Titles.Add(txtSponsor.Text + Environment.NewLine + "SC " + txtSC.Text + " - " + txtSCDesc.Text);

            //Legends
            Legend legend1 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };
            Legend legend2 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };
            Legend legend3 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };
            Legend legend4 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };

            //Formats
            //chartArea.AxisX.LabelStyle.Format = "dd/MMM\nhh:mm";
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Title = "No. of Days";

            chart.ChartAreas.Add(chartArea);
            chart.Legends.Add(legend1);
            chart.Legends.Add(legend2);
            chart.Legends.Add(legend3);
            chart.Legends.Add(legend4);
            //series.XValueType = ChartValueType.DateTime;
            //DataTable dtSource = new DataTable();
            // setup data
            if (cboGrpCode.SelectedIndex <= 0)
            {
                if (cboCategory.SelectedIndex != 8)
                {
                    chart.Titles.Add("Period Covered:" + dtpFrom.Value.ToShortDateString() + " - " + dtpTo.Value.ToShortDateString() +
                    Environment.NewLine + "Category " + cboCategory.Text);
                }
                else
                {
                    chart.Titles.Add("Period Covered:" + dtpFrom.Value.ToShortDateString() + " - " + dtpTo.Value.ToShortDateString() +
                    Environment.NewLine + "Date Received to Actual End Date");
                }
                //dtSource = PSSClass.ManagementReports.TATWWSummary(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), "", dtpFrom.Value, dtpTo.Value);
            }
            else
            {
                chart.Titles.Add("Period Covered:" + dtpFrom.Value.ToShortDateString() + " - " + dtpTo.Value.ToShortDateString() +
                Environment.NewLine + "Test Method: " + cboGrpCode.Text);
                chartArea.AxisX.Title = "Categories";
                //dtSource = PSSClass.ManagementReports.TATWWSummary(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), cboGrpCode.Text, dtpFrom.Value, dtpTo.Value);
            }
            if (dtChartWW != null && dtChartWW.Rows.Count > 0)
            {
                if (dtChartWW.Rows.Count > 1)//All Groups
                {
                    //if (cboGrpCode.SelectedIndex <= 0)
                    //{
                    int nMFI = 0, nDII = 0, nDISI = 0, nMFII = 0;
                    //MF Series
                    for (int i = 0; i < dtChartWW.Rows.Count; i++)
                    {
                        if (dtChartWW.Rows[i]["GroupCode"].ToString() == "MF")
                            nMFI = i;
                        else if (dtChartWW.Rows[i]["GroupCode"].ToString() == "DI")
                            nDII = i;
                        else if (dtChartWW.Rows[i]["GroupCode"].ToString() == "DI+S")
                            nDISI = i;
                        else if (dtChartWW.Rows[i]["GroupCode"].ToString() == "MF+IPM")
                            nMFII = i;
                    }
                    if (cboGrpCode.SelectedIndex == 0)
                    {
                        switch (cboCategory.SelectedIndex)
                        {
                            case 0:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinRecProp"])); 
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinRecProp"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinRecProp"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinRecProp"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinRecProp"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinRecProp"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinRecProp"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinRecProp"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxRecProp"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxRecProp"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxRecProp"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxRecProp"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxRecProp"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxRecProp"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxRecProp"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxRecProp"].ToString();
                                }
                                break;
                            case 1:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinRecOn"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinRecOn"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinRecOn"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinRecOn"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinRecOn"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinRecOn"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinRecOn"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinRecOn"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxRecOn"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxRecOn"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxRecOn"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxRecOn"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxRecOn"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxRecOn"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxRecOn"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxRecOn"].ToString();
                                }
                                break;
                            case 2:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinOnOff"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinOnOff"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinOnOff"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinOnOff"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinOnOff"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinOnOff"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinOnOff"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinOnOff"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxOnOff"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxOnOff"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxOnOff"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxOnOff"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxOnOff"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxOnOff"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxOnOff"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxOnOff"].ToString();
                                }
                                break;
                            case 3:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinOffRep"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinOffRep"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinOffRep"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinOffRep"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinOffRep"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinOffRep"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinOffRep"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinOffRep"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxOffRep"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxOffRep"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxOffRep"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxOffRep"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxOffRep"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxOffRep"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxOffRep"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxOffRep"].ToString();
                                }
                                break;
                            case 4:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinRepQA"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinRepQA"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinRepQA"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinRepQA"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinRepQA"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinRepQA"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinRepQA"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinRepQA"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxRepQA"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxRepQA"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxRepQA"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxRepQA"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxRepQA"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxRepQA"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxRepQA"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxRepQA"].ToString();
                                }
                                break;
                            case 5:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinREMailInv"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinREMailInv"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinREMailInv"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinREMailInv"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinQAEMail"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinQAEMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinQAEMail"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinQAEMail"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxQAEMail"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxQAEMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxQAEMail"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxQAEMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxQAEMail"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxQAEMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxQAEMail"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxQAEMail"].ToString();
                                }
                                break;
                            case 6:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinREMailInv"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinREMailInv"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinREMailInv"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinREMailInv"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinREMailInv"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinREMailInv"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinREMailInv"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinREMailInv"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxREMailInv"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxREMailInv"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxREMailInv"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxREMailInv"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxREMailInv"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxREMailInv"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxREMailInv"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxREMailInv"].ToString();
                                }
                                break;
                            case 7:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinInvEMail"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinInvEMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinInvEMail"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinInvEMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinInvEMail"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinInvEMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinInvEMail"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinInvEMail"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxInvEMail"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxInvEMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxInvEMail"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxInvEMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxInvEMail"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxInvEMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxInvEMail"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxInvEMail"].ToString();
                                }
                                break;
                            case 8:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinRecOff"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinRecOff"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinRecOff"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinRecOff"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinRecOff"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinRecOff"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinRecOff"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinRecOff"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxRecOff"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxRecOff"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxRecOff"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxRecOff"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxRecOff"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxRecOff"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxRecOff"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxRecOff"].ToString();
                                }
                                break;
                            case 9:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinRecInvEMail"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinRecInvEMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinRecInvEMail"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinRecInvEMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinRecInvEMail"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinRecInvEMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinRecInvEMail"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinRecInvEMail"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxRecInvEMail"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxRecInvEMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxRecInvEMail"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxRecInvEMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxRecInvEMail"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxRecInvEMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxRecInvEMail"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxRecInvEMail"].ToString();
                                }
                                break;
                            case 10:
                                {
                                    Series MinSeries = new Series()
                                    {
                                        Name = "Min",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.LightSeaGreen,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MinSeries);
                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MinRecREMail"]));
                                    var min1 = MinSeries.Points[0];
                                    min1.AxisLabel = "MF";
                                    min1.Color = Color.LightSeaGreen;
                                    min1.Label = dtChartWW.Rows[nMFI]["MinRecREMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MinRecREMail"]));
                                    var min2 = MinSeries.Points[1];
                                    min2.AxisLabel = "DI";
                                    min2.Color = Color.LightSeaGreen;
                                    min2.Label = dtChartWW.Rows[nDII]["MinRecREMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MinRecREMail"]));
                                    var min3 = MinSeries.Points[2];
                                    min3.AxisLabel = "DI+S";
                                    min3.Color = Color.LightSeaGreen;
                                    min3.Label = dtChartWW.Rows[nDISI]["MinRecREMail"].ToString();

                                    MinSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MinRecREMail"]));
                                    var min4 = MinSeries.Points[3];
                                    min4.AxisLabel = "MF+IPM";
                                    min4.Color = Color.LightSeaGreen;
                                    min4.Label = dtChartWW.Rows[nMFII]["MinRecREMail"].ToString();

                                    Series MaxSeries = new Series()
                                    {
                                        Name = "Max",
                                        IsVisibleInLegend = true,
                                        Color = System.Drawing.Color.Salmon,
                                        ChartType = SeriesChartType.Column
                                    };
                                    chart.Series.Add(MaxSeries);
                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFI]["MaxRecREMail"]));
                                    var max1 = MaxSeries.Points[0];
                                    max1.AxisLabel = "MF";
                                    max1.Color = Color.Salmon;
                                    max1.Label = dtChartWW.Rows[nMFI]["MaxRecREMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDII]["MaxRecREMail"]));
                                    var max2 = MaxSeries.Points[1];
                                    max2.AxisLabel = "DI";
                                    max2.Color = Color.Salmon;
                                    max2.Label = dtChartWW.Rows[nDII]["MaxRecREMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nDISI]["MaxRecREMail"]));
                                    var max3 = MaxSeries.Points[2];
                                    max3.AxisLabel = "DI + S";
                                    max3.Color = Color.Salmon;
                                    max3.Label = dtChartWW.Rows[nDISI]["MaxRecREMail"].ToString();

                                    MaxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[nMFII]["MaxRecREMail"]));
                                    var max4 = MaxSeries.Points[3];
                                    max4.AxisLabel = "MF + IPM";
                                    max4.Color = Color.Salmon;
                                    max4.Label = dtChartWW.Rows[nMFII]["MaxRecREMail"].ToString();
                                }
                                break;
                        }
                    }
                }
                else
                {
                    //Min Series
                    Series minSeries = new Series()
                    {
                        Name = "Lowest",
                        IsVisibleInLegend = true,
                        Color = System.Drawing.Color.LightSeaGreen,
                        ChartType = SeriesChartType.Column
                    };
                    chart.Series.Add(minSeries);
                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinRecProp"]));
                    var n1 = minSeries.Points[0];
                    n1.AxisLabel = "1";
                    n1.Color = Color.LightSeaGreen;
                    n1.Label = dtChartWW.Rows[0]["MinRecProp"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinRecOn"]));
                    var n2 = minSeries.Points[1];
                    n2.AxisLabel = "2";
                    n2.Color = Color.LightSeaGreen;
                    n2.Label = dtChartWW.Rows[0]["MinRecOn"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinOnOff"]));
                    var n3 = minSeries.Points[2];
                    n3.AxisLabel = "3";
                    n3.Color = Color.LightSeaGreen;
                    n3.Label = dtChartWW.Rows[0]["MinOnOff"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinOffRep"]));
                    var n4 = minSeries.Points[3];
                    n4.AxisLabel = "4";
                    n4.Color = Color.LightSeaGreen;
                    n4.Label = dtChartWW.Rows[0]["MinOffRep"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinRepQA"]));
                    var n5 = minSeries.Points[4];
                    n5.AxisLabel = "5";
                    n5.Color = Color.LightSeaGreen;
                    n5.Label = dtChartWW.Rows[0]["MinRepQA"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinQAEMail"]));
                    var n6 = minSeries.Points[5];
                    n6.AxisLabel = "6";
                    n6.Color = Color.LightSeaGreen;
                    n6.Label = dtChartWW.Rows[0]["MinQAEMail"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinREMailInv"]));
                    var n7 = minSeries.Points[6];
                    n7.AxisLabel = "7";
                    n7.Color = Color.LightSeaGreen;
                    n7.Label = dtChartWW.Rows[0]["MinREMailInv"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinInvEMail"]));
                    var n8 = minSeries.Points[7];
                    n8.AxisLabel = "8";
                    n8.Color = Color.LightSeaGreen;
                    n8.Label = dtChartWW.Rows[0]["MinInvEMail"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinRecOff"]));
                    var n9 = minSeries.Points[8];
                    n9.AxisLabel = "9";
                    n9.Color = Color.LightSeaGreen;
                    n9.Label = dtChartWW.Rows[0]["MinRecOff"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinRecInvEMail"]));
                    var n10 = minSeries.Points[9];
                    n10.AxisLabel = "10";
                    n10.Color = Color.LightSeaGreen;
                    n10.Label = dtChartWW.Rows[0]["MinRecInvEMail"].ToString();

                    minSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MinRecREMail"]));
                    var n11 = minSeries.Points[10];
                    n11.AxisLabel = "11";
                    n11.Color = Color.LightSeaGreen;
                    n11.Label = dtChartWW.Rows[0]["MinRecREMail"].ToString();

                    //Max Series
                    Series maxSeries = new Series()
                    {
                        Name = "Highest",
                        IsVisibleInLegend = true,
                        Color = System.Drawing.Color.Salmon,
                        ChartType = SeriesChartType.Column
                    };
                    chart.Series.Add(maxSeries);
                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxRecProp"]));
                    var m1 = maxSeries.Points[0];
                    m1.AxisLabel = "1";
                    m1.Color = Color.Salmon;
                    m1.Label = dtChartWW.Rows[0]["MaxRecProp"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxRecOn"]));
                    var m2 = maxSeries.Points[1];
                    m2.AxisLabel = "2";
                    m2.Color = Color.Salmon;
                    m2.Label = dtChartWW.Rows[0]["MaxRecOn"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxOnOff"]));
                    var m3 = maxSeries.Points[2];
                    m3.AxisLabel = "3";
                    m3.Color = Color.Salmon;
                    m3.Label = dtChartWW.Rows[0]["MaxOnOff"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxOffRep"]));
                    var m4 = maxSeries.Points[3];
                    m4.AxisLabel = "4";
                    m4.Color = Color.Salmon;
                    m4.Label = dtChartWW.Rows[0]["MaxOffRep"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxRepQA"]));
                    var m5 = maxSeries.Points[4];
                    m5.AxisLabel = "5";
                    m5.Color = Color.Salmon;
                    m5.Label = dtChartWW.Rows[0]["MaxRepQA"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxQAEMail"]));
                    var m6 = maxSeries.Points[5];
                    m6.AxisLabel = "6";
                    m6.Color = Color.Salmon;
                    m6.Label = dtChartWW.Rows[0]["MaxQAEMail"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxREMailInv"]));
                    var m7 = maxSeries.Points[6];
                    m7.AxisLabel = "7";
                    m7.Color = Color.Salmon;
                    m7.Label = dtChartWW.Rows[0]["MaxREMailInv"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxInvEMail"]));
                    var m8 = maxSeries.Points[7];
                    m8.AxisLabel = "8";
                    m8.Color = Color.Salmon;
                    m8.Label = dtChartWW.Rows[0]["MaxInvEMail"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxRecOff"]));
                    var m9 = maxSeries.Points[8];
                    m9.AxisLabel = "9";
                    m9.Color = Color.Salmon;
                    m9.Label = dtChartWW.Rows[0]["MaxRecOff"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxRecInvEMail"]));
                    var m10 = maxSeries.Points[9];
                    m10.AxisLabel = "10";
                    m10.Color = Color.Salmon;
                    m10.Label = dtChartWW.Rows[0]["MaxRecInvEMail"].ToString();

                    maxSeries.Points.Add(Convert.ToDouble(dtChartWW.Rows[0]["MaxRecREMail"]));
                    var m11 = maxSeries.Points[10];
                    m10.AxisLabel = "11";
                    m11.Color = Color.Salmon;
                    m11.Label = dtChartWW.Rows[0]["MaxRecREMail"].ToString();
                }
                //draw the chart
                chart.Invalidate();
                chart.Dock = DockStyle.Fill;
                pnlAllChart.Controls.Add(chart);
                //// write out a file
                //chart.SaveImage("chart.png", ChartImageFormat.Png);
            }
        }

        private void LoadData()
        {
            if (txtSponsorID.Text == "1389" && txtSC.Text == "329") //Wedgewood
            {
                if (cboGrpCode.SelectedIndex <= 0)
                {
                    nLoadSw = 1;
                    //dtDataSource = PSSClass.ManagementReports.TATWWDataSource(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), "", dtpFrom.Value, dtpTo.Value);
                }
                else
                    nLoadSw = 2;
                //dtDataSource = PSSClass.ManagementReports.TATWWDataSource(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), cboGrpCode.Text, dtpFrom.Value, dtpTo.Value); 
            }
            else
                nLoadSw = 3;
            //dtDataSource = PSSClass.ManagementReports.TATDataSource(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), dtpFrom.Value, dtpTo.Value);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value > dtpTo.Value || dtpTo.Value >= DateTime.Now)
            {
                MessageBox.Show("Invalid date range.", Application.ProductName);
                return;
            }

            if (txtSponsorID.Text == "" || txtSponsor.Text == "")
            {
                MessageBox.Show("Please select Sponsor.", Application.ProductName);
                return;
            }
            if (txtSC.Text == "" || txtSCDesc.Text == "")
            {
                MessageBox.Show("Please select Service Code.", Application.ProductName);
                return;
            }
            dgvDataSource.DataSource = null; pnlAllChart.Controls.Clear(); pnlTATChart.Controls.Clear();
            pnlAllChart.BackgroundImage = new Bitmap(Application.StartupPath + @"\Chart 1.png");
            pnlTATChart.BackgroundImage = new Bitmap(Application.StartupPath + @"\Chart 2.png");
            lblProgress.Text = "Retrieving data and generating charts..."; lblProgress.Visible = true; 

            int nDays = PSSClass.ServiceCodes.SCDuration(Convert.ToInt16(txtSC.Text));
            nDuration = Convert.ToInt16(nDays);

            if (txtSponsorID.Text == "1389" && txtSC.Text == "329") //Wedgewood
            {
                if (cboGrpCode.SelectedIndex <= 0)
                {
                    nLoadSw = 1; priGrpCode = "";
                }
                else
                {
                    nDays = PSSClass.ManagementReports.TATWWDuration(cboGrpCode.Text);
                    nDuration = Convert.ToInt16(nDays);
                    nLoadSw = 2; priGrpCode = cboGrpCode.Text;
                }
            }
            else
            {
                nLoadSw = 3;
            }
            if (bw.IsBusy != true)
            {
                EnableControls(false);
                bw.RunWorkerAsync();
            }
        }

        private void LoadDataSources()
        {
            dgvDataSource.DataSource = null;
            dgvDataSource.DataSource = dtDataSource;
            dgvDataSource.Columns["GBLNo"].HeaderText = "GBL NO.";
            if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
            {
                dgvDataSource.Columns["OtherID"].HeaderText = "OTHER ID";
                dgvDataSource.Columns["OtherID"].Width = 120;
                dgvDataSource.Columns["GroupCode"].HeaderText = "TEST GROUP";
            }
            dgvDataSource.Columns["DateReceived"].HeaderText = "DATE RECEIVED";
            dgvDataSource.Columns["PropStartDate"].HeaderText = "PROP. START DATE";
            dgvDataSource.Columns["PropEndDate"].HeaderText = "PROP. END DATE";
            dgvDataSource.Columns["DateOn"].HeaderText = "ACTUAL START DATE";
            dgvDataSource.Columns["DateOff"].HeaderText = "ACTUAL END DATE";
            dgvDataSource.Columns["ReportDate"].HeaderText = "REPORT DATE";
            dgvDataSource.Columns["DateApproved"].HeaderText = "DATE QA APPROVED";
            dgvDataSource.Columns["DateEMailed"].HeaderText = "DATE E-MAILED";
            dgvDataSource.Columns["InvDate"].HeaderText = "INV. DATE";
            dgvDataSource.Columns["InvMailDate"].HeaderText = "INV. MAIL DATE";
            dgvDataSource.Columns["GBLNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDataSource.Columns["GBLNo"].Width = 75;
            if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
            {
                for (int i = 3; i <= 12; i++)
                {
                    dgvDataSource.Columns[i].Width = 75;
                    dgvDataSource.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvDataSource.Columns[i].DefaultCellStyle.Format = "MM/dd/yyyy";
                }
                for (int i = 0; i <= 10; i++)
                {
                    dgvDataSource.Columns[i + 13].HeaderText = "CAT " + (i + 1).ToString();
                    dgvDataSource.Columns[i + 13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvDataSource.Columns[i + 13].Width = 55;
                }
            }
            else
            {
                for (int i = 1; i <= 10; i++)
                {
                    dgvDataSource.Columns[i].Width = 75;
                    dgvDataSource.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvDataSource.Columns[i].DefaultCellStyle.Format = "MM/dd/yyyy";
                }
                for (int i = 0; i <= 10; i++)
                {
                    dgvDataSource.Columns[i + 11].HeaderText = "CAT " + (i + 1).ToString();
                    dgvDataSource.Columns[i + 11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvDataSource.Columns[i + 11].Width = 57;
                }
            }
            if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
                dgvDataSource.Columns["GroupCode"].Frozen = true;
            else
                dgvDataSource.Columns["DateReceived"].Frozen = true;
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            for (int i = 1; (i <= 9); i++)
            {
                if ((worker.CancellationPending == false))
                {
                    // Perform a time consuming operation and report progress.
                    if (nLoadSw == 1)//WW GroupCode = ""
                    {
                        try
                        {
                            dtDataSource = PSSClass.ManagementReports.TATWWDataSource(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), "", dtpFrom.Value, dtpTo.Value);
                            dtChartWW = PSSClass.ManagementReports.TATWWSummary(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), "", dtpFrom.Value, dtpTo.Value);
                            dtTATChart = PSSClass.ManagementReports.TATWWSC(Convert.ToInt16(txtSC.Text), "", nDuration, dtpFrom.Value, dtpTo.Value);
                        }
                        catch
                        {
                            e.Cancel = true;
                        }
                    }
                    else if (nLoadSw == 2) //WW GroupCode <> ""
                    {
                        try
                        {
                            dtDataSource = PSSClass.ManagementReports.TATWWDataSource(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), priGrpCode, dtpFrom.Value, dtpTo.Value);
                            dtChartWW = PSSClass.ManagementReports.TATWWSummary(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), priGrpCode, dtpFrom.Value, dtpTo.Value);
                            dtTATChart = PSSClass.ManagementReports.TATWWSC(Convert.ToInt16(txtSC.Text), priGrpCode, nDuration, dtpFrom.Value, dtpTo.Value);
                        }
                        catch
                        {
                            e.Cancel = true;
                        }
                    }
                    else if (nLoadSw == 3)// Non-WW
                    {
                        try
                        {
                            dtDataSource = PSSClass.ManagementReports.TATDataSource(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), dtpFrom.Value, dtpTo.Value);
                            dtChart = PSSClass.ManagementReports.TATSummary(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), dtpFrom.Value, dtpTo.Value);
                            dtTATChart = PSSClass.ManagementReports.TATSC(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), nDuration, dtpFrom.Value, dtpTo.Value);
                        }
                        catch
                        {
                            e.Cancel = true;
                        }
                    }
                    nLoadSw = 0;
                    worker.ReportProgress((i * 10));
                    System.Threading.Thread.Sleep(100);
                }
                else
                {
                    e.Cancel = true; break;
                }
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblProgress.Text = "Retrieving data and generating charts..." + (e.ProgressPercentage.ToString() + "%");
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                MessageBox.Show("A previous task is cancelled.");
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Process has an error.");
            }
            else
            {
                nLoadSw = 0;
                if (dtDataSource == null || dtDataSource.Rows.Count == 0)
                {
                    MessageBox.Show("No records found on given date.", Application.ProductName);
                    EnableControls(true);
                    return;
                }
                lblProgress.Text = "Generating charts...100%";
                LoadDataSources();
                if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
                    LoadChartWW();
                else
                    LoadChart();
                LoadTATChart();
                EnableControls(true);
            }
        }

        private void EnableControls(bool bStatus)
        {
            lblProgress.Visible = !bStatus; 
            txtSponsorID.Enabled = bStatus; txtSponsor.Enabled = bStatus;
            txtSC.Enabled = bStatus; txtSCDesc.Enabled = bStatus;
            picSponsors.Enabled = bStatus; picSC.Enabled = bStatus;
            dtpFrom.Enabled = bStatus; dtpTo.Enabled = bStatus; btnOK.Enabled = bStatus; cboGrpCode.Enabled = bStatus; 
        }

        private void LoadTATChart()
        {
            //Setup Chart
            //this.components = new System.ComponentModel.Container();

            Chart tatchart = new Chart();
            ChartArea tatchartArea = new ChartArea();

            tatchart.Dock = System.Windows.Forms.DockStyle.Fill;
            tatchart.Size = new Size(350, 250);
            tatchart.Titles.Add(txtSponsor.Text + Environment.NewLine + "SC " + txtSC.Text + " - " + txtSCDesc.Text);
            tatchart.Titles.Add("Period Covered:" + dtpFrom.Value.ToShortDateString() + " - " + dtpTo.Value.ToShortDateString());
            //tatchart.Titles.Add("Test Schedule Adherence" + Environment.NewLine + "Date Received to Report Mail Date" + Environment.NewLine + 
            //    "(Duration: " + nDuration.ToString() + " Days)");

            ////Legends
            //Legend legend1 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };
            //Legend legend2 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };
            //Legend legend3 = new Legend() { BackColor = Color.Transparent, ForeColor = Color.Black, Title = "" };

            //Formats
            //chartArea.AxisX.LabelStyle.Format = "dd/MMM\nhh:mm";
            tatchartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            tatchartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            tatchartArea.AxisX.LabelStyle.Font = new Font("Consolas", 8);
            tatchartArea.AxisY.LabelStyle.Font = new Font("Consolas", 8);
            tatchartArea.AxisX.Interval = 1;
            tatchartArea.AxisY.Title = "Total Count";

            tatchart.ChartAreas.Add(tatchartArea);
            //chart.Legends.Add(legend1);
            //chart.Legends.Add(legend2);
            //chart.Legends.Add(legend3);
            //series.XValueType = ChartValueType.DateTime;

            // set up some data
            if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
            {
                if (cboGrpCode.SelectedIndex <= 0)
                {
                    tatchart.Titles.Add("Test Schedule Adherence" + Environment.NewLine + "Date Received to Report Mail Date" + Environment.NewLine +
                        "(Duration: " + nDuration.ToString() + " Days)");
                    dtTATChart = PSSClass.ManagementReports.TATWWSC(Convert.ToInt16(txtSC.Text), "", nDuration, dtpFrom.Value, dtpTo.Value);
                }
                else
                {
                    tatchart.Titles.Add("Test Schedule Adherence" + Environment.NewLine + "Date Received to Report Mail Date" + Environment.NewLine +
                    "Test Method: " + cboGrpCode.Text + " (Duration: " + nDuration.ToString() + " Days)");
                    dtTATChart = PSSClass.ManagementReports.TATWWSC(Convert.ToInt16(txtSC.Text), cboGrpCode.Text, nDuration, dtpFrom.Value, dtpTo.Value);
                }
            }
            else
            {
                tatchart.Titles.Add("Test Schedule Adherence" + Environment.NewLine + "Date Received to Report Mail Date" + Environment.NewLine +
                    "(Duration: " + nDuration.ToString() + " Days)");
                dtTATChart = PSSClass.ManagementReports.TATSC(Convert.ToInt16(txtSponsorID.Text), Convert.ToInt16(txtSC.Text), nDuration, dtpFrom.Value, dtpTo.Value);
            }
            if (dtTATChart != null && dtTATChart.Rows.Count > 0)
            {
                //Series
                Series dSeries = new Series()
                {
                    Name = "TATSummary",
                    IsVisibleInLegend = false,
                    Color = System.Drawing.Color.LightGreen,
                    ChartType = SeriesChartType.Column
                };
                int nTotal;
                nTotal = Convert.ToInt16(dtTATChart.Rows[0]["BelowDuration"]) + Convert.ToInt16(dtTATChart.Rows[0]["OnSchedule"]) +
                               Convert.ToInt16(dtTATChart.Rows[0]["AboveDuration"]);

                if (nTotal != 0)
                {
                    //decimal nBelow = (Convert.ToDecimal(dtTATChart.Rows[0]["BelowDuration"]) / nTotal) * 100;
                    decimal nOnTime = ((Convert.ToDecimal(dtTATChart.Rows[0]["OnSchedule"]) + Convert.ToDecimal(dtTATChart.Rows[0]["BelowDuration"])) / nTotal) * 100;
                    decimal nAbove = (Convert.ToDecimal(dtTATChart.Rows[0]["Aboveduration"]) / nTotal) * 100;

                    tatchart.Series.Add(dSeries);
                    //dSeries.Points.Add(Convert.ToDouble(dtTATChart.Rows[0]["BelowDuration"]));
                    //var d1 = dSeries.Points[0];
                    //d1.AxisLabel = "Under";
                    //d1.Color = Color.LightGreen;
                    //d1.Label = dtTATChart.Rows[0]["BelowDuration"].ToString() + " (" + nBelow.ToString("##0.00") + "%)"; ;

                    dSeries.Points.Add(Convert.ToDouble(dtTATChart.Rows[0]["BelowDuration"]) + Convert.ToDouble(dtTATChart.Rows[0]["OnSchedule"]));
                    var d2 = dSeries.Points[0];
                    d2.AxisLabel = "On Schedule";
                    d2.Color = Color.LightSeaGreen;
                    d2.Label = (Convert.ToInt16(dtTATChart.Rows[0]["BelowDuration"]) + Convert.ToInt16(dtTATChart.Rows[0]["OnSchedule"])).ToString() + " (" + nOnTime.ToString("##0.00") + "%)"; ;

                    dSeries.Points.Add(Convert.ToDouble(dtTATChart.Rows[0]["AboveDuration"]));
                    var d3 = dSeries.Points[1];
                    d3.AxisLabel = "Over";
                    d3.Color = Color.Salmon;
                    d3.Label = dtTATChart.Rows[0]["AboveDuration"].ToString() + " (" + nAbove.ToString("##0.00") + "%)";

                    // draw!
                    tatchart.Invalidate();
                    tatchart.Dock = DockStyle.Fill;
                    pnlTATChart.Controls.Add(tatchart);
                }
                //// write out a file
                //chart.SaveImage("chart.png", ChartImageFormat.Png);

            }
        }

        private void dgvDataSource_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDataSource.Rows.Count > 0 && dgvDataSource.CurrentCell.OwningColumn.Name == "GBLNo" && dgvDataSource.CurrentCell.Value.ToString() != "")
            {
                SamplesLogin childForm = new SamplesLogin();
                childForm.Text = "SAMPLES LOGIN";
                childForm.MdiParent = Program.mdi;
                childForm.nSearch = 3;
                childForm.nFR = 5;
                childForm.strCriteria = "GBL No.";
                childForm.strData = dgvDataSource.CurrentCell.Value.ToString();
                childForm.nLogNo = Convert.ToInt32(dgvDataSource.CurrentCell.Value);
                childForm.Show();
            }
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSC.Visible = false;
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSponsors;
            dvwSponsors = new DataView(dtSponsors, "SponsorName like '" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
            dgvSponsors.DataSource = dvwSponsors;
            DGVSetting(dgvSponsors);
        }

        private void txtSCDesc_Enter(object sender, EventArgs e)
        {
            dgvSC.Visible = true; dgvSC.BringToFront(); dgvSponsors.Visible = false;
        }

        private void txtSCDesc_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSC;
            dvwSC = new DataView(dtSC, "ServiceDesc like '%" + txtSCDesc.Text.Trim().Replace("'", "''") + "%'", "ServiceDesc", DataViewRowState.CurrentRows);
            dgvSC.DataSource = dvwSC;
            DGVSetting(dgvSC);
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
            {
                try
                {
                    txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                    if (txtSponsor.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                    return;
                }
                if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
                {
                    lblGroup.Visible = true; cboGrpCode.Visible = true;
                    if (cboGrpCode.SelectedIndex == 0)
                    {
                        lblCategory.Visible = true; cboCategory.Visible = true; cboCategory.SelectedIndex = 10;
                    }
                    else
                    {
                        lblCategory.Visible = false; cboCategory.Visible = false;
                    }
                }
                else
                {
                    lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                }
                dgvSponsors.Visible = false; txtSC.Focus();
            }
            else
            {
                dgvSponsors.Visible = false; txtSponsor.Text = "";
            }
        }

        private void txtSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
            {
                try
                {
                    txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
                    if (txtSCDesc.Text.Trim() == "")
                    {
                        MessageBox.Show("No matching Service Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                        return;
                    }
                }
                catch 
                {
                    MessageBox.Show("No matching Service Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                    return;
                }
                if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
                {
                    lblGroup.Visible = true; cboGrpCode.Visible = true;
                    if (cboGrpCode.SelectedIndex == 0)
                    {
                        lblCategory.Visible = true; cboCategory.Visible = true; cboCategory.SelectedIndex = 10;
                    }
                    else
                    {
                        lblCategory.Visible = false; cboCategory.Visible = false;
                    }
                }
                else
                {
                    lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                }
                dgvSC.Visible = false; 
            }
            else
            {
                dgvSC.Visible = false; txtSCDesc.Text = "";
            }

        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtSponsorID.Text = "";
        }

        private void txtSCDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtSC.Text = "";
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            dgvSponsors.Visible = false;
            if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
            {
                lblGroup.Visible = true; cboGrpCode.Visible = true;
                if (cboGrpCode.SelectedIndex == 0)
                {
                    lblCategory.Visible = true; cboCategory.Visible = true; cboCategory.SelectedIndex = 10;
                }
                else
                {
                    lblCategory.Visible = false; cboCategory.Visible = false;
                }
            }
            else
            {
                lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false; 
            }
            cboGrpCode.SelectedIndex = 0;
        }

        private void dgvSponsors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                dgvSponsors.Visible = false;
                if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
                {
                    lblGroup.Visible = true; cboGrpCode.Visible = true;
                    if (cboGrpCode.SelectedIndex == 0)
                    {
                        lblCategory.Visible = true; cboCategory.Visible = true; cboCategory.SelectedIndex = 10;
                    }
                    else
                    {
                        lblCategory.Visible = false; cboCategory.Visible = false;
                    }
                }
                else
                {
                    lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                }
                cboGrpCode.SelectedIndex = 0;
            }
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void dgvSC_DoubleClick(object sender, EventArgs e)
        {
            txtSCDesc.Text = dgvSC.CurrentRow.Cells[0].Value.ToString();
            txtSC.Text = dgvSC.CurrentRow.Cells[1].Value.ToString();
            dgvSC.Visible = false;
            if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
            {
                lblGroup.Visible = true; cboGrpCode.Visible = true;
                if (cboGrpCode.SelectedIndex == 0)
                {
                    lblCategory.Visible = true; cboCategory.Visible = true; cboCategory.SelectedIndex = 10;
                }
                else
                {
                    lblCategory.Visible = false; cboCategory.Visible = false;
                }
            }
            else
            {
                lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
            }
            cboGrpCode.SelectedIndex = 0;
        }

        private void dgvSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSCDesc.Text = dgvSC.CurrentRow.Cells[0].Value.ToString();
                txtSC.Text = dgvSC.CurrentRow.Cells[1].Value.ToString();
                dgvSC.Visible = false;
                if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
                {
                    lblGroup.Visible = true; cboGrpCode.Visible = true;
                    if (cboGrpCode.SelectedIndex == 0)
                    {
                        lblCategory.Visible = true; cboCategory.Visible = true; cboCategory.SelectedIndex = 10;
                    }
                    else
                    {
                        lblCategory.Visible = false; cboCategory.Visible = false;
                    }
                }
                else
                {
                    lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                }
                cboGrpCode.SelectedIndex = 0;
            }
        }

        private void dgvSC_Leave(object sender, EventArgs e)
        {
            dgvSC.Visible = false;
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            dgvSponsors.Visible = true; dgvSponsors.BringToFront(); dgvSC.Visible = false;
        }

        private void picSC_Click(object sender, EventArgs e)
        {
            dgvSC.Visible = true; dgvSC.BringToFront(); dgvSponsors.Visible = false;
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void txtSC_Enter(object sender, EventArgs e)
        {
            dgvSC.Visible = false;
        }
     
        private void cboGrpCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void cboGrpCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvDataSource.DataSource = null; pnlAllChart.Controls.Clear(); pnlTATChart.Controls.Clear();
            pnlAllChart.BackgroundImage = new Bitmap(Application.StartupPath + @"\Chart 1.png");
            pnlTATChart.BackgroundImage = new Bitmap(Application.StartupPath + @"\Chart 2.png");
            if (cboGrpCode.SelectedIndex == 0 && txtSponsorID.Text.Trim() == "1389" && txtSC.Text == "329")
            {
                lblCategory.Visible = true; cboCategory.Visible = true; cboCategory.SelectedIndex = 10;
            }
            else
            {
                lblCategory.Visible = false; cboCategory.Visible = false;
            }
        }

        private void TATDashBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bw != null && bw.IsBusy)
            {
                bw.CancelAsync();
                bw.Dispose();
            }
            bw = null;
        }

        private void cboCategory_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtSC_Leave(object sender, EventArgs e)
        {
            try
            {
                txtSCDesc.Text = PSSClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
                if (txtSCDesc.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Service Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                    return;
                }
            }
            catch 
            {
                MessageBox.Show("No matching Service Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                return;
            }
            if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
            {
                lblGroup.Visible = true; cboGrpCode.Visible = true;
                if (cboGrpCode.SelectedIndex == 0)
                {
                    lblCategory.Visible = true; cboCategory.Visible = true; cboCategory.SelectedIndex = 10;
                }
                else
                {
                    lblCategory.Visible = false; cboCategory.Visible = false;
                }
            }
            else
            {
                lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
            }
            dgvSC.Visible = false; 
        }

        private void txtSponsorID_Leave(object sender, EventArgs e)
        {
            try
            {
                txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                    return;
                }
            }
            catch
            {
                MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
                return;
            }
            if (txtSponsorID.Text == "1389" && txtSC.Text == "329")
            {
                lblGroup.Visible = true; cboGrpCode.Visible = true;
                if (cboGrpCode.SelectedIndex == 0)
                {
                    lblCategory.Visible = true; cboCategory.Visible = true; cboCategory.SelectedIndex = 10;
                }
                else
                {
                    lblCategory.Visible = false; cboCategory.Visible = false;
                }
            }
            else
            {
                lblGroup.Visible = false; cboGrpCode.Visible = false; lblCategory.Visible = false; cboCategory.Visible = false;
            }
            dgvSponsors.Visible = false;
        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvDataSource.DataSource = null; pnlAllChart.Controls.Clear(); pnlTATChart.Controls.Clear();
            pnlAllChart.BackgroundImage = new Bitmap(Application.StartupPath + @"\Chart 1.png");
            pnlTATChart.BackgroundImage = new Bitmap(Application.StartupPath + @"\Chart 2.png");
        }
    }
}
