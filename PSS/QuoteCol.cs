﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;

namespace GIS
{
    public partial class QuoteCol : GIS.TemplateForm
    {
        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;

        private string strFileAccess = "RO";

        //for DatagridView search
        private int nCtr = 0;
        private int nSw = 0;
        //======================

        public QuoteCol()
        {
            InitializeComponent();

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

        private void LoadRecords()
        {
            DataTable dt = new DataTable();
            dt = GISClass.Quotations.QuoteColHeaders(1);
            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during loading." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nMode = 9;
                return;
            }
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;

            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            nMode = 0;
            FileAccess();
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

        private void BuildPrintItems()
        {
            ToolStripMenuItem[] items = new ToolStripMenuItem[2];

            items[0] = new ToolStripMenuItem();
            items[0].Name = "DepartmentCode";
            items[0].Text = "Sorted by Department Code";
            items[0].Click += new EventHandler(PrintCatCodeClickHandler);

            items[1] = new ToolStripMenuItem();
            items[1].Name = "CategoryDesc";
            items[1].Text = "Sorted by Department Name";
            items[1].Click += new EventHandler(PrintCatNameClickHandler);

            tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = GISClass.Quotations.QuoteColHeaders(1);
            if (dt == null)
            {
                MessageBox.Show("Connection problen encountered during build-up of search items." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        private void PrintCatNameClickHandler(object sender, EventArgs e)
        {
            //RptCat rptCatNameList = new RptCat();
            //rptCatNameList.WindowState = FormWindowState.Maximized;
            //rptCatNameList.rptName = "CatName";
            //rptCatNameList.rptLabel = "QuoteCol REFERENCE LIST SORTED BY NAME";
            //rptCatNameList.Show();
        }

        private void PrintCatCodeClickHandler(object sender, EventArgs e)
        {
        //    RptCat rptCatNameList = new RptCat();
        //    rptCatNameList.WindowState = FormWindowState.Maximized;
        //    rptCatNameList.rptName = "CatCode";
        //    rptCatNameList.rptLabel = "QuoteCol REFERENCE LIST SORTED BY CODE";
        //    rptCatNameList.Show();
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
            if (tstbSearch.Text.Trim() != "")
            {
                try
                {
                    bsFile.Filter = "CategoryID<>0";
                    GISClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                    dgvFile.Select();
                    if (pnlRecord.Visible == true)
                        LoadData();
                }
                catch { }
            }
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
                    dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                    tsbRefresh.Enabled = true;
                }
                catch { }
            }
        }

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            bsFile.Filter = "CategoryID<>0";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtCode.Text = dgvFile.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dgvFile.CurrentRow.Cells[1].Value.ToString();
            txtID.Text = dgvFile.CurrentRow.Cells[2].Value.ToString();
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
        }

        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns[0].HeaderText = "COLUMN NO.";
            dgvFile.Columns[1].HeaderText = "DESCRIPTION";
            dgvFile.Columns[2].HeaderText = "ID";
            dgvFile.Columns[0].Width = 160;
            dgvFile.Columns[0].DefaultCellStyle.Padding = new Padding(30, 0, 0, 0);
            dgvFile.Columns[1].Width = 300;
            dgvFile.Columns[2].Visible = false;
        }

        private void dgvDoubleClickHandler(object sender, EventArgs e)
        {
            pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
            OpenControls(this.pnlRecord, false);
            LoadData();
        }

        private void dgvKeyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (nSw == 0)
            {
                nSw = 1;
                timer1.Enabled = true;
            }
            if (e.KeyChar == 13)
            {
                pnlRecord.Visible = true; dgvFile.Visible = false; btnClose.Visible = true;
                OpenControls(this.pnlRecord, false);
                if (dgvFile.Rows.Count > 0)
                    LoadData();
            }
            else if (e.KeyChar == 8)
            {
                tstbSearch.Text = tstbSearch.Text.Substring(0, tstbSearch.TextLength - 1);
            }
            else
            {
                tstbSearch.Text = tstbSearch.Text + e.KeyChar.ToString();
                nCtr = 0;
            }
        }

        private void dgvKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            txtCode.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            LoadData();
            txtCode.Focus(); btnClose.Visible = false;
        }

        private void DeleteRecord()
        {
            LoadData();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                SqlCommand sqlcmd = new SqlCommand();

                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.Add(new SqlParameter("@CatID", SqlDbType.SmallInt));
                sqlcmd.Parameters["@CatID"].Value = Convert.ToInt16(txtID.Text);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelQCol";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            LoadRecords();
        }

        private void SaveRecord()
        {
            if (txtCode.Text.Trim() == "")
            {
                MessageBox.Show("Please enter column no.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCode.Focus();
                return;
            }

            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter description.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            if (nMode == 1)
                txtID.Text = GISClass.DataEntry.NewID("QuotationCol", "CategoryID").ToString();

            if (GISClass.DataEntry.MatchingRecord("CategoryID", "CategoryDesc", "QuotationCol", txtName.Text, nMode, Convert.ToInt16(txtID.Text)) == true)
            {
                MessageBox.Show("Matching department name found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.Add(new SqlParameter("@nMode", SqlDbType.SmallInt));
            sqlcmd.Parameters["@nMode"].Value = nMode;

            sqlcmd.Parameters.Add(new SqlParameter("@CatID", SqlDbType.SmallInt));
            sqlcmd.Parameters["@CatID"].Value = Convert.ToInt16(txtID.Text);

            sqlcmd.Parameters.Add(new SqlParameter("@CatDesc", SqlDbType.VarChar));
            sqlcmd.Parameters["@CatDesc"].Value = txtName.Text.Trim();

            sqlcmd.Parameters.Add(new SqlParameter("@ColNo", SqlDbType.TinyInt));
            sqlcmd.Parameters["@ColNo"].Value = Convert.ToInt16(txtCode.Text);

            sqlcmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            sqlcmd.Parameters["@UserID"].Value = LogIn.nUserID;

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditQCol";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return;
            }
            sqlcnn.Dispose();
            dgvFile.Refresh();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            LoadRecords();
            GISClass.General.FindRecord("CategoryID", txtID.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            AddEditMode(false);
        }

        private void CancelSave()
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
            ClearControls(this);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            AddEditMode(false);
            nMode = 0;
        }

        private void QuoteCol_Load(object sender, EventArgs e)
        {
            if (nMode == 9)
            {
                SendKeys.Send("{F12}");
                return;
            }
            
            strFileAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "Quotecol");

            LoadRecords();
            DataGridSetting();
            BuildPrintItems();
            BuildSearchItems();

            nMode = 0;
            this.WindowState = FormWindowState.Maximized;
            this.Focus();
            this.BringToFront();      
        }

        private void QuoteCol_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    if (nMode == 0 && strFileAccess != "RO")
                    {
                        AddEditMode(true); AddRecord();
                    }
                    break;

                case Keys.F3:
                    if (nMode == 0 && strFileAccess != "RO")
                    {
                        AddEditMode(true); EditRecord();
                    }
                    break;

                //case Keys.F4:
                //    if (nMode == 0)
                //    {
                //        DeleteRecord();
                //    }
                //    break;

                case Keys.F5:
                    if (nMode != 0)
                        SaveRecord();
                    break;

                case Keys.F6:
                    if (nMode != 0)
                        CancelSave();
                    break;

                case Keys.F7:
                    if (nMode == 0)
                        tsddbPrint.ShowDropDown();
                    break;

                case Keys.F8:
                    if (nMode == 0)
                        tsddbSearch.ShowDropDown();
                    break;
                case Keys.F9:
                    if (nMode == 0)
                        SearchOKClickHandler(null, null);
                    break;

                case Keys.F10:
                    if (nMode == 0)
                        SearchFilterClickHandler(null, null);
                    break;

                case Keys.F11:
                    if (nMode == 0)
                        RefreshClickHandler(null, null);
                    break;

                case Keys.F12:
                    if (nMode != 0)
                    {
                        DialogResult dReply = new DialogResult();
                        dReply = MessageBox.Show("Do you want to cancel the current task?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dReply == DialogResult.No)
                            return;
                    }
                    this.Close();
                    break;

                default:
                    break;
            }
        }

        private void pnlRecord_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlRecord_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void pnlRecord_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
        }

        private void lblHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
        }

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr += 1;
            if (nCtr > 1)
            {
                DataTable dt = new DataTable();
                dt = GISClass.Quotations.QuoteColHeaders(1);
                GISClass.DataEntry.DGVSearch(tstbSearchField.Text, tstbSearch.Text, dt, bsFile);
                nCtr = 0;
                tstbSearch.Text = "";
                timer1.Enabled = false;
                nSw = 0;
            }
        }

    }
}
