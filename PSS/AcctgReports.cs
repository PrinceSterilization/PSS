using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace PSS
{
   
	public partial class AcctgReports : Form
	{
		private int nTimer = 0, nRNo = 1;
		private byte nSw = 0;

		public AcctgReports()
		{
			InitializeComponent();
			//this.MinimumSize = new Size(this.Width, this.Height);
		}
	   
		private void btnProceed_Click(object sender, EventArgs e)
		{
			if (dtpStart.Value > dtpEnd.Value)
			{
				MessageBox.Show("Invalid data range.", Application.ProductName);
				return;
			}

			switch (nRNo)
				{
				case 1:
				{
						if (cboFY.SelectedIndex == -1)
						{
							MessageBox.Show("Please select fiscal year.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						else
						{
							GenerateReport();
						}
						break;
				}
				case 8:
					{
						int nEDD = DateTime.DaysInMonth(dtpEnd.Value.Year, dtpEnd.Value.Month);
						DateTime dStart = Convert.ToDateTime(dtpStart.Value.Month.ToString() + "/1/" + dtpStart.Value.Year.ToString());
						DateTime dEnd = Convert.ToDateTime(dtpEnd.Value.Month.ToString() + "/" + nEDD.ToString() + "/" + dtpEnd.Value.Year.ToString());
						double nMo = dEnd.Subtract(dStart).Days / (365.25 / 12);
						nMo = Math.Round(nMo, 2);
						if (nMo >= 11 && nMo <= 12)
						{

						}
						else
						{
							MessageBox.Show("Please enter a 12-month period.", Application.ProductName);
							return;
						}
						nTimer = 0; tClock.Enabled = true; lblProgress.Visible = true;
						lblProgress.Text = "Generating report...please standby.";
						nTimer = 0; tClock.Enabled = true;
						break;
					}
				case 9:{ theBigPicture(nRNo); break;}
				case 10: { theBigPicture(nRNo); break; }
				case 11: { theBigPicture(nRNo); break; }
				case 12: { theBigPicture(nRNo); break; }
				case 13: { theBigPicture(nRNo); break; }
				case 14: { theBigPicture(nRNo); break; }
				default:
					{
						nTimer = 0; tClock.Enabled = true; lblProgress.Visible = true;
						lblProgress.Text = "Generating report...please standby.";
						nTimer = 0; tClock.Enabled = true;
						break;
					}
				
			}
			
			
		}
	
		private void GenerateReport()
		{
			if (nRNo == 1)
			{
				DisplaySummary();
			}
			else
			{
				AcctgRpt rpt = new AcctgRpt();
				if (nRNo == 2)
					rpt.rptName = "Invoices To Be Posted - Sorted by Sponsor ASC";
				else if (nRNo == 3)
					rpt.rptName = "Comparative Revenue Report";
				else if (nRNo == 4)
					rpt.rptName = "Comparative Revenue Report - Grouped by Sponsor";
				else if (nRNo == 5)
					rpt.rptName = "Exceptions List";
				else if (nRNo == 6)
					rpt.rptName = "Invoices To Be Posted";
				else if (nRNo == 7)
					rpt.rptName = "Invoices To Be Posted - Grouped by Sponsor";
				else if (nRNo == 8)
					rpt.rptName = "TTM";


				rpt.dteStart = dtpStart.Value;
				rpt.dteEnd = dtpEnd.Value;

				try
				{
					rpt.Show();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void DisplaySummary()
		{
			DataTable dt = PSSClass.ManagementReports.SummaryTable(Convert.ToInt16(cboFY.Text));
			dgvSummary.DataSource = dt;
			dgvSummary.Columns["Mo"].HeaderText = "Month #";
			dgvSummary.Columns["Mo"].Width = 65;
			dgvSummary.Columns["NoLogIns"].HeaderText = "Logins";
			dgvSummary.Columns["NoLogIns"].Width = 70;
			dgvSummary.Columns["NoRpts"].HeaderText = "Reports";
			dgvSummary.Columns["NoRpts"].Width = 70;
			dgvSummary.Columns["NoInv"].HeaderText = "Invoices";
			dgvSummary.Columns["NoInv"].Width = 70;
			dgvSummary.Columns["Mo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dgvSummary.Columns["NoLogins"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgvSummary.Columns["NoLogins"].DefaultCellStyle.Format = "#,##0";
			dgvSummary.Columns["NoRpts"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgvSummary.Columns["NoRpts"].DefaultCellStyle.Format = "#,##0";
			dgvSummary.Columns["NoInv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgvSummary.Columns["NoInv"].DefaultCellStyle.Format = "#,##0";
			dgvSummary.Columns["Sales"].DefaultCellStyle.Format = "#,##0";
			dgvSummary.Columns["Sales"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgvSummary.Columns["Sales"].Width = 90;
			dgvSummary.Columns["SalesRpt"].HeaderText = "$/Report";
			dgvSummary.Columns["SalesRpt"].DefaultCellStyle.Format = "$#,##0";
			dgvSummary.Columns["SalesRpt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgvSummary.Columns["SalesRpt"].Width = 90;
			dgvSummary.Columns["SalesInv"].HeaderText = "$/Invoice";
			dgvSummary.Columns["SalesInv"].DefaultCellStyle.Format = "$#,##0";
			dgvSummary.Columns["SalesInv"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgvSummary.Columns["SalesInv"].Width = 90;
			dgvSummary.Columns["SalesLogin"].HeaderText = "$/Login";
			dgvSummary.Columns["SalesLogin"].DefaultCellStyle.Format = "$#,##0";
			dgvSummary.Columns["SalesLogin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgvSummary.Columns["SalesLogin"].Width = 90;
			dgvSummary.Columns["LoginRpt"].HeaderText = "Login/Report";
			dgvSummary.Columns["LoginRpt"].DefaultCellStyle.Format = "#,##0.00";
			dgvSummary.Columns["LoginRpt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			dgvSummary.Columns["LoginRpt"].Width = 90;
			dgvSummary.EnableHeadersVisualStyles = false;
			dgvSummary.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
			dgvSummary.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
			dgvSummary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
			dgvSummary.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
			dgvSummary.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
			dgvSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			dgvSummary.RowHeadersVisible = false;
			pnlSummary.Visible = true;
			pnlSummary.Location = new Point(15, 15);
			pnlSummary.BringToFront(); this.MaximumSize = new Size(862, 468);
			this.Size = new Size(dgvSummary.Width + 80, dgvSummary.Height + 190);
			
			lblSummary.Text =  cboFY.Text + " Logins, Reports, Invoice and Sales Summary ";
			lblNote.Text = "Note: Sales data is based from Executive Dashboard entries.";
		}

		private void AcctgReports_Load(object sender, EventArgs e)
		{
			//string dte = "1/1/" + DateTime.Now.Year.ToString(); -- Revised as per DLP's request 6/6/2016
			string sdte = DateTime.Now.ToString("MM/dd/yyyy"); //Convert.ToDateTime(dte).ToString("MM/dd/yyyy"); -- Revised as per DLP's request 6/6/2016

			dtpStart.Value = Convert.ToDateTime(sdte);

			int nY = DateTime.Now.Year;
			for (int i = 1; i < 8; i++)
			{
				cboFY.Items.Add(nY.ToString());
				nY--;
			}
			DefaultDateRange();
			this.Size = new Size(pnlReports.Width + 60, pnlReports.Height + 180);
		}
		private void DefaultDateRange()
		{
			int nEDD = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
			DateTime dStart = Convert.ToDateTime(DateTime.Now.Month.ToString() + "/1/" + DateTime.Now.Year.ToString());
			DateTime dEnd = Convert.ToDateTime(DateTime.Now.Month.ToString() + "/" + nEDD.ToString() + "/" + DateTime.Now.Year.ToString());
			dtpStart.Value = dStart;
			dtpEnd.Value = dEnd;
			cboFY.SelectedIndex = 0;
		}
		private void rdoCompInvRpt_Click(object sender, EventArgs e)
		{
			nRNo = 3;
			DefaultDateRange();
		}
		private void rdoExceptions_Click(object sender, EventArgs e)
		{
			nRNo = 5;
			DefaultDateRange();
		}
		private void rdoInvToBePosted_Click(object sender, EventArgs e)
		{
			nRNo = 6;
			DefaultDateRange();
		}
		private void rdoInvToBePostedBySp_Click(object sender, EventArgs e)
		{
			nRNo = 7;
			DefaultDateRange();
		}
		private void rdoCompInvRptBySp_Click(object sender, EventArgs e)
		{
			nRNo = 4;
			DefaultDateRange();
		}
		private void rdoLRISS_Click(object sender, EventArgs e)
		{
			nRNo = 1;
			DefaultDateRange();
		}
		private void cboFY_SelectedIndexChanged(object sender, EventArgs e)
		{
			nSw = 1;
			dgvSummary.DataSource = null;
			string dte = "1/1/" + cboFY.Text;
			string sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");
			dtpStart.Value = Convert.ToDateTime(sdte);

			dte = "12/31/" + cboFY.Text;
			sdte = Convert.ToDateTime(dte).ToString("MM/dd/yyyy");
			dtpEnd.Value = Convert.ToDateTime(sdte);

			//lblNote.Text = "Generating report...please standby.";
		}
		private void btnReturn_Click(object sender, EventArgs e)
		{
			pnlSummary.Visible = false;
			this.Size = new Size(405, 370);


			//pnlSummary.Visible = false; this.MaximumSize = new Size(pnlReports.Width + 60, pnlReports.Height + 160);
			//this.Size = new Size(pnlReports.Width + 60, pnlReports.Height + 180);
			
		}
		private void AcctgReports_Activated(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Normal;
		}
		private void tClock_Tick(object sender, EventArgs e)
		{
			if (nTimer == 0)
			{
				nTimer = 1;
				tClock.Enabled = false;
				GenerateReport();
				lblProgress.Visible = false;
			}
		}
		private void rdoInvToBePostedByDiff_Click(object sender, EventArgs e)
		{
			nRNo = 2;
			dtpStart.Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
			dtpEnd.Value = Convert.ToDateTime(DateTime.Now.ToShortDateString());
		}
		private void rdoTTM_Click(object sender, EventArgs e)
		{
			nRNo = 8;
			int nSYY = DateTime.Now.Year - 1;
			int nSMM = DateTime.Now.Month;
			int nEMM = DateTime.Now.Month - 1;
			int nEDD = DateTime.DaysInMonth(DateTime.Now.Year, nEMM);
			DateTime dStart = Convert.ToDateTime(nSMM.ToString() + "/1/" + nSYY.ToString());
			DateTime dEnd = Convert.ToDateTime(nEMM + "/" + nEDD.ToString() + "/" + DateTime.Now.Year.ToString());
			dtpStart.Value = dStart;
			dtpEnd.Value = dEnd;
		}
		private void theBigPicture(int intRdoNum)
		{

			switch(intRdoNum)
				{
				case 13:
				{
						if (grpPipeSummary.Visible == false)
						{
							grpPipeSummary.Location = new Point(15, 15);
							//Populate Dropdown with PO Statuses                            
							Populate_cmbPOStatus();
							//Populate gvPipelineSummary
							Populate_GridView("PO_Summary", gvPipelineSummary);
							int intRecordsCount = gvPipelineSummary.Rows.Count;
							if (intRecordsCount > 0)
							{
								Populate_SummaryTotals(intRecordsCount);
							}

							int intGridProjectedHight = (intRecordsCount + 3) * 25;
							if (intGridProjectedHight > 600)
							{
								intGridProjectedHight = 600;
							}
							int ingGroupHeight = intGridProjectedHight + 145;
							grpPipeSummary.Height = ingGroupHeight;
							gvPipelineSummary.Height = intGridProjectedHight;
							if (grpBigPicture.Visible == true) { grpBigPicture.Visible = false; }
							//Resize                            
							this.Size = new Size(grpPipeSummary.Width + 50, grpPipeSummary.Height + 50);
							grpPipeSummary.Visible = true;
							
						}
						break;
				}
				case 14:
				{
						if (grpBigPicture.Visible == false)
						{
							try
							{
								grpBigPicture.Location = new Point(15, 15);
								//Populate gvPipelineDetails
								Populate_GridView("PO_Details", gvPipelineDetails);
								//Set Visibility and Clickability of columns and cells
								int intRecordsCount = gvPipelineDetails.Rows.Count;
								if (intRecordsCount > 0)
								{
									//Hide SponsorID
									gvPipelineDetails.Columns[0].Visible = false;
									//Hide PDF Path
									gvPipelineDetails.Columns[gvPipelineDetails.Columns.Count - 1].Visible = false;
									//Hide PO Notes
									gvPipelineDetails.Columns[gvPipelineDetails.Columns.Count - 2].Visible = false;

									//Change colors for Clickable cells                                 
									//gvPipelineDetails.Columns["PO Invoices"].DefaultCellStyle.ForeColor = Color.Maroon;
									gvPipelineDetails.Columns["Invoiced Revenue"].DefaultCellStyle.ForeColor = Color.Maroon;
									gvPipelineDetails.Columns["PO No"].DefaultCellStyle.ForeColor = Color.Maroon;
									gvPipelineDetails.Columns["Preliminary Insufficient PO"].DefaultCellStyle.ForeColor = Color.Green;
									
									//Populate Labels with Total Amounts
									Populate_Totals(intRecordsCount);
									
									//Resize Grid
									int intGridProjectedHight = (intRecordsCount + 2) * 25;
									if (intGridProjectedHight > 600)
									{
										intGridProjectedHight = 600;
									}
									int ingGroupHeight = intGridProjectedHight + 145;
									grpBigPicture.Height = ingGroupHeight;
									gvPipelineDetails.Height = intGridProjectedHight;

									
								}
								
								if (grpPipeSummary.Visible == true) { grpPipeSummary.Visible = false; }
								this.Size = new Size(grpBigPicture.Width + 50, grpBigPicture.Height + 50);
								grpBigPicture.Visible = true;
							}
							catch (Exception)
							{

								throw;
							}							                      
						}
						break;
				}
				case 100:
					{
						if (grpBigPicture.Visible == false)
						{
							grpBigPicture.Location = new Point(15, 15);
							//Populate gvPipelineDetails
							Populate_GridView("PO_Details", gvPipelineDetails);
							if (gvPipelineDetails.Rows.Count > 0)
							{
								gvPipelineDetails.Columns[0].Visible = false;
								gvPipelineDetails.Columns[gvPipelineDetails.Columns.Count - 1].Visible = false;
								gvPipelineDetails.Columns[gvPipelineDetails.Columns.Count - 2].Visible = false;

								gvPipelineDetails.Columns["Invoiced Revenue"].DefaultCellStyle.ForeColor = Color.Maroon;
								gvPipelineDetails.Columns["PO No"].DefaultCellStyle.ForeColor = Color.Maroon;
								gvPipelineDetails.Columns["Preliminary Insufficient PO"].DefaultCellStyle.ForeColor = Color.BlueViolet;
							}
							this.Size = new Size(grpBigPicture.Width + 50, grpBigPicture.Height + 50);
							grpBigPicture.Visible = true;
							if (grpPipeSummary.Visible == true) { grpPipeSummary.Visible = false; }

						}
						break;
					}
			}

		}
		private string ConvertDollarValue(string strInput)
		{	
			if (strInput[0].ToString() == "(")
			{
				strInput = strInput.Replace("(", "-");
				strInput = strInput.Replace(")", "");                
			}
			strInput = strInput.Replace("$", "");
			strInput = strInput.Replace(",", "");
			strInput = strInput.Trim();
			return strInput;
		}

		private void Populate_Totals(int intRowCounter)
		{
			try
			{
				double
					dblBacklog = 0.00,
					dblValidation = 0.00,
					dblPOAmount = 0.00, 
					dblPOAmtJan1 = 0.00, 
					dblPrepayment = 0.00,
					dblAppliedPrepayment = 0.00,
					dblNetPrepayment = 0.00,
					dblRevenue = 0.00, 
					dblProforma = 0.00, 
					dblNetProforma = 0.00,
					dblRevNetProf = 0.00,  
					dblInsufPO = 0.00, 
					dblAdjustments = 0.00;
				string strInput;
				for (int i = 0; i < intRowCounter; i++)
				{
					//PO Backlog
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[2].Value.ToString());
					dblBacklog += Convert.ToDouble(strInput);

					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[4].Value.ToString());
					dblValidation += Convert.ToDouble(strInput);
					
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[6].Value.ToString());
					dblPOAmount += Convert.ToDouble(strInput);
					
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[7].Value.ToString());
					dblPOAmtJan1 += Convert.ToDouble(strInput);
					
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[8].Value.ToString());
					dblPrepayment += Convert.ToDouble(strInput);
					
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[9].Value.ToString());
					dblAppliedPrepayment += Convert.ToDouble(strInput);
				   
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[10].Value.ToString());
					dblNetPrepayment += Convert.ToDouble(strInput);
					
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[11].Value.ToString());
					dblRevenue += Convert.ToDouble(strInput);
					
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[12].Value.ToString());
					dblProforma += Convert.ToDouble(strInput);
					
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[13].Value.ToString());
					dblNetProforma += Convert.ToDouble(strInput);
					
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[14].Value.ToString());
					dblRevNetProf += Convert.ToDouble(strInput);
				   
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[15].Value.ToString());
					if (strInput == "0.00")
					{
						gvPipelineDetails.Rows[i].Cells[15].Style.ForeColor = Color.Green;
					}
					else
					{
						gvPipelineDetails.Rows[i].Cells[15].Style.ForeColor = Color.Maroon;
					}
					dblInsufPO += Convert.ToDouble(strInput);
					
					strInput = ConvertDollarValue(gvPipelineDetails.Rows[i].Cells[17].Value.ToString());
					dblAdjustments += Convert.ToDouble(strInput);
				}

				lblBacklog.Text = dblBacklog.ToString("C", CultureInfo.CurrentCulture);
				lblFinalValidation.Text = dblValidation.ToString("C", CultureInfo.CurrentCulture);
				lblTotal_PO.Text = dblPOAmount.ToString("C", CultureInfo.CurrentCulture);
				lblPOJan1.Text = dblPOAmtJan1.ToString("C", CultureInfo.CurrentCulture);
				lblPrepayment.Text = dblPrepayment.ToString("C", CultureInfo.CurrentCulture);
				lblAppPrepayment.Text = dblAppliedPrepayment.ToString("C", CultureInfo.CurrentCulture);
				lblNetPrepayment.Text = dblNetPrepayment.ToString("C", CultureInfo.CurrentCulture);
				lblRevenue.Text = dblRevenue.ToString("C", CultureInfo.CurrentCulture);
				lblProforma.Text = dblProforma.ToString("C", CultureInfo.CurrentCulture);
				lblNetProforma.Text = dblNetProforma.ToString("C", CultureInfo.CurrentCulture);
				lblRevNetPro.Text = dblRevNetProf.ToString("C", CultureInfo.CurrentCulture);
				lblInsufficientPO.Text = dblInsufPO.ToString("C", CultureInfo.CurrentCulture);
				lblAdjustments.Text = dblAdjustments.ToString("C", CultureInfo.CurrentCulture);
			}
			catch (Exception)
			{

				throw;
			}
			
		
		}
		private void Populate_SummaryTotals(int intRowCounter)
		{
			try
			{
				double dblBacklog = 0.00, dblFinalValidation = 0.00;
				string strInput;
				for (int i = 0; i < intRowCounter; i++)
				{
					//PO Backlog
					strInput = ConvertDollarValue(gvPipelineSummary.Rows[i].Cells[2].Value.ToString());
					dblBacklog += Convert.ToDouble(strInput);
					//PO Amt on Jan 1
					strInput = ConvertDollarValue(gvPipelineSummary.Rows[i].Cells[4].Value.ToString());
					dblFinalValidation += Convert.ToDouble(strInput);
				}

				lblPOBacklog.Text = dblBacklog.ToString("C", CultureInfo.CurrentCulture);
				lblPOFinalValid.Text = dblFinalValidation.ToString("C", CultureInfo.CurrentCulture);
			}
			catch (Exception)
			{
				throw;
			}
		}
		private void Populate_GridView(string strSelection, DataGridView myGV)
		{
			DataGridView gv = new DataGridView();
			gv = myGV;
			DataTable dt = new DataTable();
			try
			{
				dt = PSSClass.PO.UniversalPO(strSelection);
				BindingSource bs = new BindingSource();
				bs.DataSource = dt;				
				gv.DataSource = dt;
			}
			catch (Exception ex)
			{

			}
			finally
			{
				dt.Dispose();
			}


		}
		private void Populate_GridView(string strSelection, string strP1, string strP2, string strP3, DataGridView myGV)
		{
			DataGridView gv = new DataGridView();
			gv = myGV;
			DataTable dt = new DataTable();
			try
			{
				dt = PSSClass.PO.UniversalPO_3(strSelection, strP1, strP2, strP3);
				int intCount = dt.Rows.Count;
				BindingSource bs = new BindingSource();
				bs.DataSource = dt;               
				gv.DataSource = dt;
			}
			catch (Exception ex)
			{

			}
			finally
			{
				dt.Dispose();
			}


		}
		private void Populate_cmbPOStatus()
		{
			cmbPOStatus.Items.Clear();
			try
			{
				DataTable dt = new DataTable();
				dt = PSSClass.PO.UniversalPO("PO_Status");
				DDL_Item myddl1 = new DDL_Item();        
				myddl1.ddlItem_Name = "Show All";
				myddl1.ddlItem_Value = "0";
				cmbPOStatus.Items.Add(myddl1);
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DDL_Item myddl = new DDL_Item();
					string strName= Convert.ToString(dt.Rows[i][0]);
					string strValue= Convert.ToString(dt.Rows[i][1]);
					myddl.ddlItem_Name = strName;
					myddl.ddlItem_Value = strValue;
					cmbPOStatus.Items.Add(myddl);
				}        
				cmbPOStatus.DisplayMember = "ddlItem_Name";
				cmbPOStatus.ValueMember = "ddlItem_Value";        
			}
			catch (Exception)
			{
				throw;
			}
			
		}
		
		//private void Populate_gvOverviewPO(string strSelection)
		//{
		   
		//	//DataTable dt = new DataTable();
		//	//try
		//	//{
		//	//     dt= PSSClass.PO.UniversalPO(strSelection);
		//	//    BindingSource bs = new BindingSource();
		//	//    bs.DataSource = dt;
		//	//    //gvOverviewPO.DataSource = bs;
		//	//    gvOverviewPO.DataSource = dt;
		//	//    if (gvOverviewPO.Rows.Count > 0)
		//	//    {
		//	//        gvOverviewPO.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
		//	//    }

		//	//}
		//	//catch (Exception ex)
		//	//{

		//	//}
		//	//finally
		//	//{
		//	//    dt.Dispose();
		//	//}

			
		//}
		private void RdoBigPicture_CheckedChanged(object sender, EventArgs e)
		{
			nRNo = 9;            
		}

		private void BtnClose_Click(object sender, EventArgs e)
		{
			//gvOverviewPO.Columns.Clear();
			grpBigPicture.Visible = false;
			this.Size = new Size(405,370);
		}

		private void GrpBigPicture_Enter(object sender, EventArgs e)
		{

		}

		private void RdoProforma_CheckedChanged(object sender, EventArgs e)
		{
			nRNo = 10;
		}

		private void RdoFullPicture_CheckedChanged(object sender, EventArgs e)
		{
			nRNo = 11;
		}

		private void RdoDetailPicture_CheckedChanged(object sender, EventArgs e)
		{
			nRNo = 12;
		}

		private void RadioButton2_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void RdoPipelineSummary_CheckedChanged(object sender, EventArgs e)
		{
			nRNo = 13;
		}

		private void RdoPipelineDetails_CheckedChanged(object sender, EventArgs e)
		{
			nRNo = 14;
		}

		private void BtnCloseSummary_Click(object sender, EventArgs e)
		{
			grpPipeSummary.Visible = false;
			this.Size = new Size(405, 370);
		}

	   
		private void gvPipelineDetails_CellContentClick(Object sender, DataGridViewCellEventArgs e)
		{
			try
			{				
				Clear_InvoiceLabels();
				grpInvoiceDetails.Visible = false;				
			}
			catch (Exception)
			{
				throw;
			}			
		}



		private void gvPipelineDetails_CellContentDoubleClick(Object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				int intSelectColumn = e.ColumnIndex;
				//PDF Attachment
				if (intSelectColumn == gvPipelineDetails.Columns["PO No"].Index)
				{
					int intSelectedRow = e.RowIndex;
					string strPath = gvPipelineDetails.Rows[intSelectedRow].Cells[gvPipelineDetails.Columns.Count - 1].Value.ToString();
					if (String.IsNullOrEmpty(strPath) == true || strPath == "N/A" || strPath == "na")
					{
						MessageBox.Show("Missing related document", "Missing Attachment");
					}
					else
					{
						System.Diagnostics.Process.Start(@strPath);
					}
				}

				////Show Invoices
				else if (intSelectColumn == gvPipelineDetails.Columns["Invoiced Revenue"].Index)
				{
					grpInvoiceDetails.Visible = false;
					grpInvoiceDetails.Location = new Point(15, 15);
					Clear_InvoiceLabels();
					int intSelectedRow = e.RowIndex;
					//Get SponsorID, Sponsor
					string strSponsorID = gvPipelineDetails.Rows[intSelectedRow].Cells[0].Value.ToString();
					string strSponsor = gvPipelineDetails.Rows[intSelectedRow].Cells[1].Value.ToString();
					//Get PONo, PO Date, PO Amount, PO Notes
					string strPONo = gvPipelineDetails.Rows[intSelectedRow].Cells[3].Value.ToString();
					string strPODate = gvPipelineDetails.Rows[intSelectedRow].Cells[5].Value.ToString();
					string strPOAmount = gvPipelineDetails.Rows[intSelectedRow].Cells[6].Value.ToString();
					string strInvTotalAmount = gvPipelineDetails.Rows[intSelectedRow].Cells[11].Value.ToString();
					string strPONotes = gvPipelineDetails.Rows[intSelectedRow].Cells[gvPipelineDetails.Columns.Count - 2].Value.ToString();



					//Link To respectful Labels
					lblSponsor.Text = strSponsor;
					lblPONo.Text = strPONo;
					lblPODate.Text = strPODate;
					lblPOAmount.Text = strPOAmount;
					lblInvTotal.Text = strInvTotalAmount;
					lblPONotes.Text = strPONotes;
					//Populate Gridview With Data
					Populate_GridView("show_PO_Inv", strSponsorID, strPONo, "", gvInvoiceDetails);
					int intRecordsCount = gvInvoiceDetails.Rows.Count;
					int intGridProjectedHight = (intRecordsCount + 3) * 25;
					if (intGridProjectedHight > 600)
					{
						intGridProjectedHight = 600;
					}
					int ingGroupHeight = intGridProjectedHight + 145;
					grpInvoiceDetails.Height = ingGroupHeight;
					gvInvoiceDetails.Height = intGridProjectedHight;
					//Show Result   

					if (grpBigPicture.Visible == true) { grpBigPicture.Visible = false; }
					grpInvoiceDetails.Visible = true;
					//grpInvoiceDetails.BringToFront();                    
					this.Size = new Size(grpInvoiceDetails.Width + 50, grpInvoiceDetails.Height + 50);
				}
				else if (intSelectColumn == gvPipelineDetails.Columns["Preliminary Insufficient PO"].Index)
				{
					int intSelectedRow = e.RowIndex;
					string strPONotes = gvPipelineDetails.Rows[intSelectedRow].Cells[gvPipelineDetails.Columns.Count - 2].Value.ToString();
					if (string.IsNullOrEmpty(strPONotes))
					{
						strPONotes = "No Notes";
					}
					
					MessageBox.Show(strPONotes,"PO NOTES:");					


				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Oops");                
			}            
		}
		private void Clear_InvoiceLabels()
		{
			lblSponsor.Text = "";
			lblPONo.Text = "";
			lblPODate.Text = "";
			lblPOAmount.Text = "";
			lblInvTotal.Text = "";
			lblPONotes.Text = "";
		}
		private void cmbPOStatus_SelectedIndexChanged(object sender, EventArgs e)
		{ 
			try
			{
				ComboBox mycmb = (ComboBox)sender;
				var strSelectedIndex = mycmb.SelectedIndex;
				if (strSelectedIndex >= 0)
				{
					DDL_Item myddl = new DDL_Item();
					myddl = (DDL_Item)cmbPOStatus.Items[mycmb.SelectedIndex];
					string strValue = myddl.ddlItem_Value;

					if (strValue != "0")
					{
						if (chkZeroFinals.Checked == true)
						{
							Populate_GridView("PO_Summary_By_Status_0", "", strValue, "", gvPipelineSummary);
						}
						else
						{
							Populate_GridView("PO_Summary_By_Status", "", strValue, "", gvPipelineSummary);
						}
						
					}
					else
					{
						if (chkZeroFinals.Checked == true)
						{
							Populate_GridView("PO_Summary_0", gvPipelineSummary);
						}
						else
						{
							Populate_GridView("PO_Summary", gvPipelineSummary);
						}
						
					}
					int intRecordsCount = gvPipelineSummary.Rows.Count;
					if (intRecordsCount > 0)
					{
						Populate_SummaryTotals(intRecordsCount);


						int intGridProjectedHight = (intRecordsCount + 3) * 25;
						if (intGridProjectedHight > 600)
						{
							intGridProjectedHight = 600;
						}
						int ingGroupHeight = intGridProjectedHight + 145;
						grpPipeSummary.Height = ingGroupHeight;
						gvPipelineSummary.Height = intGridProjectedHight;
						//Resize                            
						this.Size = new Size(grpPipeSummary.Width + 50, grpPipeSummary.Height + 50);
						grpPipeSummary.Visible = true;
						if (grpBigPicture.Visible == true) { grpBigPicture.Visible = false; }
					}
				}
				
			}
			catch (Exception)
			{

				throw;
			}
		}
		private void btnCloseInvoices_Click(object sender, EventArgs e)
		{
			grpInvoiceDetails.Visible = false;
			Clear_InvoiceLabels();
			string strSearch = txtSearch.Text;
			if (!String.IsNullOrEmpty(strSearch))
			{
				//Show Search Results
				Show_SearchResults(strSearch, "Search");
			}
			else
			{
				Show_SearchResults("", "");
			}
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			string strSearch = txtSearch.Text;
			if (!String.IsNullOrEmpty(strSearch))
			{
				//Show Search Results
				Show_SearchResults(strSearch,"Search");
			}
			else
			{
				MessageBox.Show("Please insert part of the Sponsors name or PO No");
			}
		}

		private void Show_SearchResults(string strSearch, string strAction)
		{
			try
			{
				gvPipelineDetails.DataSource = null;
				gvPipelineDetails.Rows.Clear();
				//Populate gridview
				if (strAction == "Search")
				{
					Populate_GridView("Search_PO", strSearch, "", "", gvPipelineDetails);
				}
				else
				{
					Populate_GridView("PO_Details", gvPipelineDetails);
				}
				
				int intRecordsCount = gvPipelineDetails.Rows.Count;
				if (intRecordsCount > 0)
				{
					//Hide SponsorID
					gvPipelineDetails.Columns[0].Visible = false;
					//Hide PDF Link
					gvPipelineDetails.Columns[gvPipelineDetails.Columns.Count - 1].Visible = false;
					//Hide PO Notes
					gvPipelineDetails.Columns[gvPipelineDetails.Columns.Count - 2].Visible = false;
					//Color Links
					gvPipelineDetails.Columns["Invoiced Revenue"].DefaultCellStyle.ForeColor = Color.Maroon;
					gvPipelineDetails.Columns["PO No"].DefaultCellStyle.ForeColor = Color.Maroon;
					gvPipelineDetails.Columns["Preliminary Insufficient PO"].DefaultCellStyle.ForeColor = Color.Green;
					//Populate Labels with Total Amounts
					Populate_Totals(intRecordsCount);
				}
				int intGridProjectedHight = (intRecordsCount + 5) * 25;
				if (intGridProjectedHight > 600)
				{
					intGridProjectedHight = 600;
				}
				int ingGroupHeight = intGridProjectedHight + 145;
				grpBigPicture.Height = ingGroupHeight;
				gvPipelineDetails.Height = intGridProjectedHight;
				//Resize
				this.Size = new Size(grpBigPicture.Width + 50, grpBigPicture.Height + 50);
				grpBigPicture.Visible = true;
				if (grpPipeSummary.Visible == true) { grpPipeSummary.Visible = false; }
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			txtSearch.Text = "";
			Show_SearchResults("", "");
		}

		private void dtpStart_ValueChanged(object sender, EventArgs e)
		{
			if (rdoTTM.Checked == true)
			{
				int nEMM = 1;
				int nEYY = dtpStart.Value.Year;
				if (dtpStart.Value.Month > 1)
				{
					nEMM = dtpStart.Value.Month - 1;
					nEYY = dtpStart.Value.Year + 1;
				}
				else if (dtpStart.Value.Month == 1)
				{
					nEMM = 12;
				}
				else
					nEYY = dtpStart.Value.Year + 1;
				int nEDD = DateTime.DaysInMonth(nEYY, nEMM);
				DateTime dEnd = Convert.ToDateTime(nEMM.ToString() + "/" + nEDD.ToString() + "/" + nEYY.ToString());
				dtpEnd.Value = dEnd;
			}
		}        
	}
	public class DDL_Item
	{
		public String ddlItem_Name { get; set; }
		public String ddlItem_Value { get; set; }
	}
}
