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
    public partial class ExtSCDataLabels : GIS.TemplateForm
    {
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;

        private string[,] arrLabels = new string[1, 15];

        DataTable dtHeaders = new DataTable();
        DataTable dtSponsors = new DataTable();
        DataTable dtSC = new DataTable();

        public ExtSCDataLabels()
        {
            InitializeComponent();
            LoadRecords();
            LoadSponsorsDDL();
            LoadSCDDL();

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
            dtHeaders = GISClass.Samples.SCDataLabels();
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

        private void LoadSponsorsDDL()
        {
            dgvSponsors.DataSource = null;
            dtSponsors = GISClass.Sponsors.SponsorNamesDDL();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            dgvSponsors.DataSource = dtSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 380;
            dgvSponsors.Columns[1].Visible = false;
        }

        private void LoadSCDDL()
        {
            dgvSC.DataSource = null;
            dtSC = GISClass.ServiceCodes.SCDDL();
            if (dtSC == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }
            dgvSC.DataSource = dtSC;
            StandardDGVSetting(dgvSC);
            dgvSC.Columns[0].Width = 380;
            dgvSC.Columns[1].Visible = false;
        }

        private void DataGridSetting()
        {
            dgvFile.Columns["ServiceCode"].HeaderText = "SERVICE CODE";
            dgvFile.Columns["ServiceCode"].Width = 75;
            dgvFile.Columns["ServiceCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["ServiceDesc"].HeaderText = "SERVICE DESCRIPTION";
            dgvFile.Columns["ServiceDesc"].Width = 350;
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
            dt = GISClass.Samples.SCDataLabels();
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
        //private void PrintDeptNameClickHandler(object sender, EventArgs e)
        //{
        //    RptDept rptDeptNameList = new RptDept();
        //    rptDeptNameList.WindowState = FormWindowState.Maximized;
        //    rptDeptNameList.rptName = "DeptName";
        //    rptDeptNameList.rptLabel = "DEPARTMENTS REFERENCE LIST SORTED BY NAME";
        //    rptDeptNameList.Show();
        //}

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
                    bsFile.Filter = "DepartmentID<>0";
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
            txtSC.Text = dgvFile.CurrentRow.Cells["ServiceCode"].Value.ToString();
            txtSCDesc.Text = dgvFile.CurrentRow.Cells["ServiceDesc"].Value.ToString();
            txtSponsorID.Text = dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString();
            txtSponsor.Text = dgvFile.CurrentRow.Cells["SponsorName"].Value.ToString();
            SetUpDGV();

            DataTable dt = new DataTable();
            dt = GISClass.Samples.ExSCExtDataLabels(Convert.ToInt16(txtSC.Text), Convert.ToInt16(txtSponsorID.Text));
            if (dt == null)
            {
                MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Load Labels to array
            arrLabels = new string[1, 15];
            //nBatch = 0;
            for (int i = 0; i < 15; i++)
            {
                string strLbl = "Label" + (i + 1).ToString();
                if (dt.Rows[0][strLbl].ToString() != "")
                    arrLabels[0, i] = dt.Rows[0][strLbl].ToString();
            }

            //Load arrayvalues to datagridviews
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    dgvCol1.Rows[i].Cells[1].Value = arrLabels[0, i];
                }
                catch { }
            }
        }

        private void SetUpDGV()
        {
            dgvCol1.Rows.Clear(); dgvCol1.Columns.Clear();

            dgvCol1.RowCount = 15; 
            dgvCol1.ColumnCount = 2; 

            dgvCol1.Columns[0].Width = 92; dgvCol1.Columns[1].Width = 455; dgvCol1.Enabled = true;

            dgvCol1.Columns[0].DefaultCellStyle.BackColor = Color.Beige;

            for (int i = 0; i < 15; i++)
            {
                dgvCol1.Rows[i].Cells[0].Value = "Label  " + (i + 1).ToString();
            }

            dgvCol1.ClearSelection();
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
            arrLabels = new string[1, 15];
            //arrLabels = new string[6, 15];
            //nBatch = 0;
            txtSC.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            LoadData();
            txtSponsorID.ReadOnly = true; txtSponsor.ReadOnly = true;
            txtSC.ReadOnly = true; txtSCDesc.ReadOnly = true; btnClose.Visible = false;
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

                sqlcmd.Parameters.Add(new SqlParameter("@SC", SqlDbType.SmallInt));
                sqlcmd.Parameters["@SC"].Value = Convert.ToInt16(txtSC.Text);

                sqlcmd.Parameters.Add(new SqlParameter("@SpID", SqlDbType.SmallInt));
                sqlcmd.Parameters["@SpID"].Value = Convert.ToInt16(txtSponsorID.Text);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelExtSCData";

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

            if (txtSC.Text.Trim() == "")
            {
                MessageBox.Show("Please enter Service Code.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSC.Focus();
                return;
            }

            string strDataLabels = "<ExtSCLabels>";

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

            strDataLabels = strDataLabels + "</ExtSCLabels>";

            SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@SpID", Convert.ToInt16(txtSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@SC", Convert.ToInt16(txtSC.Text));
            sqlcmd.Parameters.AddWithValue("@DataLabels", strDataLabels);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditExtSCLabels";
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
            GISClass.General.FindRecord("ServiceCode", txtSC.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            AddEditMode(false);
            nMode = 0; //nBatch = 0;
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
            {
                mouseDown = false;
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

        private void txtSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSCDesc.Text = GISClass.ServiceCodes.SCDesc(Convert.ToInt16(txtSC.Text), dtSC);
                if (txtSCDesc.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Service Code found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvSC.Visible = false; txtSponsor.Focus();
            }
            else if (nMode == 1)
            {
                dgvSC.Visible = false; txtSCDesc.Text = "";
            }
        }

        private void txtSC_Enter(object sender, EventArgs e)
        {
            dgvSC.Visible = false;
        }

        private void txtSCDesc_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSC;
            dvwSC = new DataView(dtSC, "ServiceDesc like '%" + txtSCDesc.Text.Trim().Replace("'", "''") + "%'", "ServiceDesc", DataViewRowState.CurrentRows);
            dvwSetUp(dgvSC, dvwSC); dgvSC.Columns[0].Width = 380;
        }

        private void txtSCDesc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 1)
                txtSC.Text = "";
        }

        private void txtSCDesc_Enter(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                dgvSC.Visible = true; dgvSC.BringToFront(); txtSC.SelectAll();
            }
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = GISClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                if (txtSponsor.Text.Trim() == "")
                {
                    MessageBox.Show("No matching Sponsor ID found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                dgvSponsors.Visible = false; dgvCol1.Select();
            }
            else if (nMode == 1)
            {
                txtSponsor.Text = ""; dgvSponsors.Visible = false;
            }
        }

        private void txtSponsorID_Enter(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                dgvSponsors.Visible = false; dgvSC.Visible = false;
            }
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            DataView dvwSponsors;
            dvwSponsors = new DataView(dtSponsors, "SponsorName like '%" + txtSponsor.Text.Trim().Replace("'", "''") + "%'", "SponsorName", DataViewRowState.CurrentRows);
            dvwSetUp(dgvSponsors, dvwSponsors); dgvSponsors.Columns[0].Width = 380;
        }

        private void txtSponsor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 1)
                txtSponsorID.Text = "";
        }

        private void txtSponsor_Enter(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront(); txtSponsor.SelectAll(); dgvSC.Visible = false;
            }
        }

        private void dgvSC_DoubleClick(object sender, EventArgs e)
        {
            txtSCDesc.Text = dgvSC.CurrentRow.Cells[0].Value.ToString();
            txtSC.Text = dgvSC.CurrentRow.Cells[1].Value.ToString();
            dgvSC.Visible = false; txtSponsor.Focus();
        }

        private void dgvSC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSCDesc.Text = dgvSC.CurrentRow.Cells[0].Value.ToString();
                txtSC.Text = dgvSC.CurrentRow.Cells[1].Value.ToString();
                dgvSC.Visible = false; txtSponsor.Focus();
            }
            else if (e.KeyChar == 27)
            {
                dgvSC.Visible = false;
            }
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            dgvSponsors.Visible = false; dgvCol1.Select();
        }

        private void dgvSponsors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
        }

        private void dgvSponsors_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
                txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
                dgvSponsors.Visible = false; dgvCol1.Select();
            }
            else if (e.KeyChar == 27)
            {
                dgvSponsors.Visible = false;
            }
        }

        private void dgvSponsors_Leave(object sender, EventArgs e)
        {
            dgvSponsors.Visible = false;
        }

        private void picSC_Click(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                LoadSCDDL(); dgvSC.Visible = true;
            }
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                LoadSponsorsDDL(); dgvSponsors.Visible = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
        }

        private void dgvCol1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
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

        private void ExtSCDataLabels_Load(object sender, EventArgs e)
        {

        }

    }
}
