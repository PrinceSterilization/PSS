using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

namespace PSS
{
    public partial class FollowupsExcluded : PSS.TemplateForm
    {

        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;

        public FollowupsExcluded()
        {
            InitializeComponent();

            tsddbSearch.Enabled = false; 
            LoadRecords();                                                   

            BuildPrintItems();
            BuildSearchItems();

            bnMoveFirst.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveLast.Click += new EventHandler(MoveRecordClickHandler);
            bnMoveNext.Click += new EventHandler(MoveRecordClickHandler);
            bnMovePrevious.Click += new EventHandler(MoveRecordClickHandler);

            tsbAdd.Click += new EventHandler(AddClickHandler);
            tsbEdit.Click += new EventHandler(EditClickHandler);
            tsbDelete.Click += new EventHandler(DeleteClickHandler);
            tsbSave.Click += new EventHandler(SaveClickHandler);
            tsbCancel.Click += new EventHandler(CancelClickHandler);
            tsbExit.Click += new EventHandler(CloseClickHandler);
            tsbSearch.Click += new EventHandler(SearchOKClickHandler);
            tsbFilter.Click += new EventHandler(SearchFilterClickHandler);
            tsbRefresh.Click += new EventHandler(RefreshClickHandler);
            dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Quotations.QuoteNoFollowUp();

            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }

            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvQuotesExcluded.DataSource = bsFile;

            DataGridSetting();
           
            tsbAdd.Enabled = false;
            tsbEdit.Enabled = false;
            tsbDelete.Enabled = false;
            tsbCancel.Enabled = false;
        }

        private void CloseClickHandler(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                DialogResult dReply = new DialogResult();
                dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dReply == DialogResult.No)
                {
                    return;
                }
            }
            this.Close();
        }

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[1];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "FollowupsExcludedList";
            items[0].Text = "Follow-ups Excluded List";
            items[0].Click += new EventHandler(PrintFollowupsExcludedListClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = PSSClass.Quotations.QuoteNoFollowUp();

            if (dt == null)
            {
                MessageBox.Show("Connection problem encountered during build-up of search items." + Environment.NewLine + "Please contact your Software Developer.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            arrCol = new string[dt.Columns.Count];

            ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length];

            foreach (DataColumn colFile in dt.Columns)
            {
                items[i] = new ToolStripMenuItem();
                items[i].Name = colFile.ColumnName;

                //Using LINQ to insert space between capital letters
                var val = colFile.ColumnName;
                val = string.Concat(val.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

                items[i].Text = val;
                items[i].Click += new EventHandler(SearchItemClickHandler);
                arrCol[i] = colFile.DataType.ToString();
                cklColumns.Items.Add(val);
                i += 1;
            }
            for (int j = 0; j < cklColumns.Items.Count; j++)
            {
                cklColumns.SetItemChecked(j, true);
            }

            tsddbSearch.DropDownItems.AddRange(items);
            tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
        }

        private void dgvCellMouseClickEventHandler(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                cklColumns.Visible = true; cklColumns.BringToFront();
            }
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData();
                btnClose.Visible = true;
            }
        }

        private void cklSelIdxChEventHandler(object sender, EventArgs e)
        {
            string strCol = cklColumns.Items[cklColumns.SelectedIndex].ToString().Replace(" ", "");
            if (dgvFile.Columns[strCol].Visible == true)
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = false;
            else
                dgvFile.Columns[cklColumns.SelectedIndex].Visible = true;
            cklColumns.Visible = false;
        }

        private void AddClickHandler(object sender, EventArgs e)
        {
            AddRecord();
        }

        private void EditClickHandler(object sender, EventArgs e)
        {
            EditRecord();
        }

        private void DeleteClickHandler(object sender, EventArgs e)
        {
            DeleteRecord();
        }

        private void SaveClickHandler(object sender, EventArgs e)
        {
            SaveRecord();
        }

        private void SearchItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            tstbSearchField.Text = clickedItem.Name;
            tstbSearch.SelectAll();
            tstbSearch.Focus();
            nIndex = tsddbSearch.DropDownItems.IndexOf(clickedItem);
            tslSearchData.Text = clickedItem.Text;
        }

        private void SearchOKClickHandler(object sender, EventArgs e)
        {
            //if (tstbSearch.Text.Trim() != "")
            //{
            //    try
            //    {
            //        bsFile.Filter = "QuotationNo<>''";
            //        PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvQuoteForFollowUp);
            //        dgvQuoteForFollowUp.Select();
            //        if (pnlRecord.Visible == true)
            //            cboCutOffDays.Text = Convert.ToString(7);
            //        LoadRecords(Convert.ToInt16(cboCutOffDays.Text));
            //    }
            //    catch { }
            //}
        }

        private void SearchFilterClickHandler(object sender, EventArgs e)
        {
            if (tstbSearch.Text.Trim() != "")
            {
                try
                {
                    if (arrCol[nIndex] == "System.String")
                    {
                        string strSearch = tstbSearch.Text.Replace("'", "''");
                        DateTime dte;
                        if (DateTime.TryParse(strSearch, out dte))
                        {
                            bsFile.Filter = tstbSearchField.Text + " = '" + Convert.ToDateTime(tstbSearch.Text).ToString("MM/dd/yyyy") + "'";
                        }
                        else
                        {
                            if (chkFullText.Checked == true)
                                bsFile.Filter = tstbSearchField.Text + "='" + strSearch + "'";
                            else
                                bsFile.Filter = tstbSearchField.Text + " LIKE '%" + strSearch + "%'";
                        }
                    }
                    else if (arrCol[nIndex] == "System.DateTime")
                    {
                        bsFile.Filter = tstbSearchField.Text + " = #" + Convert.ToDateTime(tstbSearch.Text).ToString("MM/dd/yyyy") + "#";
                    }
                    else
                    {
                        bsFile.Filter = tstbSearchField.Text + "=" + tstbSearch.Text;
                    }

                    tsbRefresh.Enabled = true;
                    
                }
                catch { }
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            //bsFile.Filter = "QuotationNo<>''";
            //tsbRefresh.Enabled = false; tstbSearch.Text = "";
            //txtTotalQuotes.Text = dgvQuoteForFollowUp.RowCount.ToString();
        }

        private void PrintFollowupsExcludedListClickHandler(object sender, EventArgs e)
        {
            FollowupsExcludedList rpt = new FollowupsExcludedList();

            rpt.WindowState = FormWindowState.Maximized;

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void LoadData()
        {
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void DataGridSetting()
        {
            dgvQuotesExcluded.EnableHeadersVisualStyles = false;
            dgvQuotesExcluded.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQuotesExcluded.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvQuotesExcluded.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvQuotesExcluded.Columns["QuotationNo"].HeaderText = "Quote No";
            dgvQuotesExcluded.Columns["RevisionNo"].HeaderText = "Rev No";
            dgvQuotesExcluded.Columns["DateCreated"].HeaderText = "Date Created";
            dgvQuotesExcluded.Columns["DateEmailed"].HeaderText = "Date Emailed";
            dgvQuotesExcluded.Columns["SponsorName"].HeaderText = "Sponsor Name";
            dgvQuotesExcluded.Columns["ContactName"].HeaderText = "ContactName";
            dgvQuotesExcluded.Columns["CommentsNonPrinting"].HeaderText = "Comments";
            dgvQuotesExcluded.Columns["QuotationNo"].Width = 94;
            dgvQuotesExcluded.Columns["RevisionNo"].Width = 38;
            dgvQuotesExcluded.Columns["DateCreated"].Width = 70;
            dgvQuotesExcluded.Columns["DateEmailed"].Width = 70;
            dgvQuotesExcluded.Columns["SponsorName"].Width = 225;
            dgvQuotesExcluded.Columns["ContactName"].Width = 130;
            dgvQuotesExcluded.Columns["CommentsNonPrinting"].Width = 234;
            dgvQuotesExcluded.Columns["DateCreated"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvQuotesExcluded.Columns["DateEmailed"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvQuotesExcluded.Columns["QuotationNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvQuotesExcluded.Columns["RevisionNo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
            OpenControls(this.pnlRecord, false);
            LoadData();
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
                OpenControls(this.pnlRecord, false);
                LoadData();
            }
        }

        private void dgvKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void AddRecord()
        {
        }

        private void EditRecord()
        {
        }

        private void DeleteRecord()
        {
        }

        private void SaveRecord()
        {
        }

        private void CancelSave()
        {
        }

        private void QuotesExcluded_Load(object sender, EventArgs e)
        {
            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }
            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();

        }

        private void QuoteFollowUp_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    if (nMode == 0)
                    {
                        AddRecord();
                    }
                    break;

                case Keys.F3:
                    if (nMode == 0)
                    {
                        EditRecord();
                    }
                    break;

                case Keys.F4:
                    if (nMode == 0)
                    {
                        DeleteRecord();
                    }
                    break;

                case Keys.F5:
                    if (nMode == 1 || nMode == 2)
                    {
                        SaveRecord();
                    }
                    break;

                case Keys.F6:
                    if (nMode == 1 || nMode == 2)
                    {
                        CancelSave();
                    }
                    break;

                case Keys.F7:
                    if (nMode == 0)
                    {
                        tsddbPrint.ShowDropDown();
                    }
                    break;

                case Keys.F8:
                    if (nMode == 0)
                    {
                        tsddbSearch.ShowDropDown();
                    }
                    break;
                case Keys.F12:
                    if (nMode == 1 || nMode == 2)
                    {
                        DialogResult dReply = new DialogResult();
                        dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dReply == DialogResult.No)
                        {
                            return;
                        }
                    }
                    this.Close();
                    break;

                default:
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            this.Close(); this.Dispose();
        }

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void lblHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void dgvQuotesExcluded_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvQuotesExcluded.IsCurrentCellDirty)
            {
                dgvQuotesExcluded.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvQuotesExcluded_DoubleClick(object sender, EventArgs e)
        {
            if (dgvQuotesExcluded.Rows.Count > 0)
            {
                int intOpen = PSSClass.General.OpenForm(typeof(Quotes));

                if (intOpen == 1)
                {
                    PSSClass.General.CloseForm(typeof(Quotes));
                }
                Quotes childForm = new Quotes();
                childForm.Text = "QUOTATIONS";
                childForm.MdiParent = this.MdiParent;
                childForm.strQuoteNo = dgvQuotesExcluded.CurrentCell.Value.ToString().Substring(0, 9);
                childForm.nPSw = 1;
                childForm.Show();
            }
        }

        private void FollowupsExcluded_Load(object sender, EventArgs e)
        {

        }       
      
       
    }
}
