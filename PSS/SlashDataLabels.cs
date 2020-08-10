using System;
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
    public partial class SlashDataLabels : GIS.TemplateForm
    {
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;

        //private string[,] arrLabels = new string[3,5];
        private string[,] arrLabels = new string[1, 15];

        DataTable dtHeaders = new DataTable();
        DataTable dtSponsors = new DataTable();

        public SlashDataLabels()
        {
            InitializeComponent();
            tsbAdd.Enabled = false; tsbDelete.Enabled = false;
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
            dtHeaders = GISClass.Samples.SlashDataLabels();
            if (dtHeaders == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }

            bsFile.DataSource = dtHeaders;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();

            if (nMode == 0)
            {
                dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
                dgvFile.Columns[0].Frozen = true;
            }
            StandardDGVSetting(dgvFile);
        }

        private void DataGridSetting()
        {
            dgvFile.Columns["SponsorID"].HeaderText = "SPONSOR ID";
            dgvFile.Columns["SponsorID"].Width = 75;
            dgvFile.Columns["SponsorID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["SponsorName"].Width = 350;
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
            //ToolStripMenuItem[] items = new ToolStripMenuItem[2];

            //items[0] = new ToolStripMenuItem();
            //items[0].Name = "DepartmentCode";
            //items[0].Text = "Sorted by Department Code";
            //items[0].Click += new EventHandler(PrintDeptCodeClickHandler);

            //items[1] = new ToolStripMenuItem();
            //items[1].Name = "DepartmentName";
            //items[1].Text = "Sorted by Department Name";
            //items[1].Click += new EventHandler(PrintDeptNameClickHandler);

            //tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            int i = 0;

            DataTable dt = new DataTable();
            dt = GISClass.Samples.SlashDataLabels();
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
        private void PrintDeptNameClickHandler(object sender, EventArgs e)
        {
            DepartmentRpt rptDeptNameList = new DepartmentRpt();
            rptDeptNameList.WindowState = FormWindowState.Maximized;
            rptDeptNameList.rptName = "DeptName";
            rptDeptNameList.rptLabel = "DEPARTMENTS REFERENCE LIST SORTED BY NAME";
            rptDeptNameList.Show();
        }

        private void PrintDeptCodeClickHandler(object sender, EventArgs e)
        {
            DepartmentRpt rptDeptNameList = new DepartmentRpt();
            rptDeptNameList.WindowState = FormWindowState.Maximized;
            rptDeptNameList.rptName = "DeptCode";
            rptDeptNameList.rptLabel = "DEPARTMENTS REFERENCE LIST SORTED BY CODE";
            rptDeptNameList.Show();
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
                    bsFile.Filter = "SponsorID<>0";
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
            bsFile.Filter = "SponsorID<>0";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtSponsorID.Text = dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString();
            txtSponsor.Text = dgvFile.CurrentRow.Cells["SponsorName"].Value.ToString();
            SetUpDGV();

            DataTable dt = new DataTable();
            dt = GISClass.Samples.ExSlashDataLabels(Convert.ToInt16(txtSponsorID.Text));
            if (dt == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data labels setup for this Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //Load Labels to array
            //arrLabels = new string[3, 5];
            arrLabels = new string[1, 15];
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "Label" + (i + 1).ToString();
                if (dt.Rows[0][strLbl].ToString() != "")
                    arrLabels[0, i] = dt.Rows[0][strLbl].ToString();
            }
            //for (int i = 0; i < 5; i++)
            //{
            //    string strLbl = "L" + (i + 6).ToString();
            //    if (dt.Rows[0][strLbl].ToString() != "")
            //        arrLabels[1, i] = dt.Rows[0][strLbl].ToString();
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    string strLbl = "L" + (i + 11).ToString();
            //    if (dt.Rows[0][strLbl].ToString() != "")
            //        arrLabels[2, i] = dt.Rows[0][strLbl].ToString();
            //}
            //Load arrayvalues to datagridviews
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    dgvCol1.Rows[i].Cells[1].Value = arrLabels[0, i];
                }
                catch { }
            }
            //for (int i = 0; i < 5; i++)
            //{
            //    try
            //    {
            //        dgvCol2.Rows[i].Cells[1].Value = arrLabels[1, i];
            //    }
            //    catch { }
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    try
            //    {
            //        dgvCol3.Rows[i].Cells[1].Value = arrLabels[2, i];
            //    }
            //    catch { }
            //}
        }

        private void SetUpDGV()
        {
            dgvCol1.Rows.Clear(); dgvCol1.Columns.Clear();
            //dgvCol2.Rows.Clear(); dgvCol2.Columns.Clear();
            //dgvCol3.Rows.Clear(); dgvCol3.Columns.Clear();

            dgvCol1.RowCount = 15; //dgvCol2.RowCount = 5; dgvCol3.RowCount = 5;
            dgvCol1.ColumnCount = 2; //dgvCol2.ColumnCount = 2; dgvCol3.ColumnCount = 2;

            dgvCol1.Columns[0].Width = 98; dgvCol1.Columns[1].Width = 400; dgvCol1.Enabled = true;
            //dgvCol2.Columns[0].Width = 95; dgvCol2.Columns[1].Width = 196; dgvCol2.Enabled = true;
            //dgvCol3.Columns[0].Width = 95; dgvCol3.Columns[1].Width = 196; dgvCol3.Enabled = true;

            dgvCol1.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            //dgvCol2.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            //dgvCol3.Columns[0].DefaultCellStyle.BackColor = Color.Beige;

            for (int i = 0; i < 15; i++)
            {
                dgvCol1.Rows[i].Cells[0].Value = "Label  " + (i + 1).ToString();
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    dgvCol2.Rows[i].Cells[0].Value = "Label  " + (i + 6).ToString();
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    dgvCol3.Rows[i].Cells[0].Value = "Label  " + (i + 11).ToString();
            //}
            dgvCol1.ClearSelection(); //dgvCol2.ClearSelection(); dgvCol3.ClearSelection();
            dgvCol1.CurrentCell = dgvCol1.Rows[0].Cells[1];
        }

        private void CancelClickHandler(object sender, EventArgs e)
        {
            CancelSave();
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
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;
            ClearControls(this.pnlRecord);
            OpenControls(this.pnlRecord, true);
            SetUpDGV();
            arrLabels = new string[3,5];

        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            LoadData();
            btnClose.Visible = false; dgvCol1.CurrentCell = dgvCol1.Rows[0].Cells[1];
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

                sqlcmd.Parameters.Add(new SqlParameter("@SpID", SqlDbType.SmallInt));
                sqlcmd.Parameters["@SpID"].Value = Convert.ToInt16(txtSponsorID.Text);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelSlashDataLabel";

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
            if (txtSponsorID.Text.Trim() == "")
            {
                MessageBox.Show("Please enter select Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsor.Focus();
                return;
            }
            
            string strDataLabels = "<SlashDataLabels>";

            for (int i = 0; i < 15; i++)
            {
                if (arrLabels[0, i] != null)
                {
                    if (arrLabels[0, i].ToString().Trim() != "")
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 1).ToString() + ">" + arrLabels[0, i] + "</Label" + (i + 1).ToString() + ">";
                    }
                    else
                    {
                        strDataLabels = strDataLabels + "<Label" + (i + 1).ToString() + "></Label" + (i + 1).ToString() + ">";
                    }
                }
                else
                {
                    strDataLabels = strDataLabels + "<Label" + (i + 1).ToString() + "></Label" + (i + 1).ToString() + ">";
                }
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    if (arrLabels[1, i] != null)
            //    {
            //        if (arrLabels[1, i].ToString().Trim() != "")
            //        {
            //            strDataLabels = strDataLabels + "<Label" + (i + 16).ToString() + ">" + arrLabels[1, i] + "</Label" + (i + 16).ToString() + ">";
            //        }
            //        else
            //        {
            //            strDataLabels = strDataLabels + "<Label" + (i + 16).ToString() + "></Label" + (i + 16).ToString() + ">";
            //        }
            //    }
            //    else
            //    {
            //        strDataLabels = strDataLabels + "<Label" + (i + 16).ToString() + "></Label" + (i + 16).ToString() + ">";
            //    }
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    if (arrLabels[2, i] != null)
            //    {
            //        if (arrLabels[2, i].ToString().Trim() != "")
            //        {
            //            strDataLabels = strDataLabels + "<Label" + (i + 31).ToString() + ">" + arrLabels[2, i] + "</Label" + (i + 31).ToString() + ">";
            //        }
            //        else
            //        {
            //            strDataLabels = strDataLabels + "<Label" + (i + 31).ToString() + "></Label" + (i + 31).ToString() + ">";
            //        }
            //    }
            //    else
            //    {
            //        strDataLabels = strDataLabels + "<Label" + (i + 31).ToString() + "></Label" + (i + 31).ToString() + ">";
            //    }
            //}

            strDataLabels = strDataLabels + "</SlashDataLabels>";

            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;
            sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@DataLabels", strDataLabels);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdSlashDataLabels";
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
            GISClass.General.FindRecord("SponsorID", txtSponsorID.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            AddEditMode(false);
            tsbAdd.Enabled = false; tsbDelete.Enabled = false;
            nMode = 0; 
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

        private void SlashDataLabels_Load(object sender, EventArgs e)
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

        private void pnlRecord_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
            }
        }

        private void pnlRecord_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
                mouseDown = false;
        }

        private void pnlRecord_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDown = true;
                mousePos = new Point(e.X, e.Y);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront();
        }

        private void dgvCol1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (nMode != 2 && e.ColumnIndex == 1))
                e.Cancel = true;
        }

        private void dgvCol1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCol1.CurrentCell.Value != null)
            {
                arrLabels[0, e.RowIndex] = dgvCol1.CurrentCell.Value.ToString();
            }
            else
            {
                arrLabels[0, e.RowIndex] = "";
            }
        }
    }
}
