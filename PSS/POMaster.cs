using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PSS
{
    public partial class POMaster : PSS.TemplateForm
    {
        byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private DataTable dtSponsors = new DataTable();
        private string strFileAccess = "RO";

        public POMaster()
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
            txtSponsor.GotFocus += new EventHandler(txtSponsorEnterHandler);
        }

        private void LoadRecords()
        {
            DataTable dt = PSSClass.PO.POMstr(2);
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            FileAccess();
        }
        private void LoadRecordsByStatus()
        {
            int intStatusCode = 0;
            if (chkShowInactive.Checked)
            {
                intStatusCode = 1;
            }
            DataTable dt = PSSClass.PO.POMstrSts(intStatusCode);
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            FileAccess();
        }
        private void LoadRecordsByStatus(int intStatusID)
        {            
            DataTable dt = PSSClass.PO.POMstrSts(intStatusID);
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            DataGridSetting();
            dgvFile.Visible = true; dgvFile.BringToFront(); pnlRecord.Visible = false;
            FileAccess();
        }
        private void LoadSponsorsDDL()
        {
            dgvSponsors.DataSource = null;
            dtSponsors = PSSClass.Sponsors.SponsorNamesDDL();
            if (dtSponsors == null)
            {
                MessageBox.Show("Connection problems. Please contact your System Administrator.");
                return;
            }
            bsSponsors.DataSource = dtSponsors;
            dgvSponsors.DataSource = bsSponsors;
            StandardDGVSetting(dgvSponsors);
            dgvSponsors.Columns[0].Width = 375;
            dgvSponsors.Columns[1].Visible = false;
        }
        private void BuildPrintItems()
        {
            //ToolStripMenuItem[] items = new ToolStripMenuItem[2];

            //items[0] = new ToolStripMenuItem();
            //items[0].Name = "POName";
            //items[0].Text = "Sorted by PO Name";
            //items[0].Click += new EventHandler(PrintPONameClickHandler);

            //items[1] = new ToolStripMenuItem();
            //items[1].Name = "PONo";
            //items[1].Text = "Grouped by Regions";
            //items[1].Click += new EventHandler(PrintPORegionClickHandler);

            //tsddbPrint.DropDownItems.AddRange(items);
        }

        private void BuildSearchItems()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.PO.POMstr(1);
            if (dt == null)
            {
                MessageBox.Show("Connection problems. Please contact your system administrator.");
                return;
            }

            int i = 0;
            int n = 0;

            arrCol = new string[dt.Columns.Count];

            ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - n];

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
                i += 1;
            }
            tsddbSearch.DropDownItems.AddRange(items);
            tslSearchData.Text = tsddbSearch.DropDownItems[0].Text;
            tstbSearchField.Text = tsddbSearch.DropDownItems[0].Name;
        }

        private void MoveRecordClickHandler(object sender, EventArgs e)
        {
            if (pnlRecord.Visible == true)
            {
                LoadData();
                btnClose.Visible = true;
            }
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
        private void PrintPONameClickHandler(object sender, EventArgs e)
        {
        }

        private void PrintPORegionClickHandler(object sender, EventArgs e)
        {
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
            try
            {
                bsFile.Filter = "PONo<>''";
                PSSClass.General.FindRecord(tstbSearchField.Text, tstbSearch.Text, bsFile, dgvFile);
                dgvFile.Select();
                if (pnlRecord.Visible == true)
                    LoadData();
            }
            catch { }
        }

        private void SearchFilterClickHandler(object sender, EventArgs e)
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

        private void RefreshClickHandler(object sender, EventArgs e)
        {
            bsFile.Filter = "PONo<>''";
            tsbRefresh.Enabled = false; tstbSearch.Text = "";
        }

        private void LoadData()
        {
            ClearControls(this.pnlRecord);
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false;
            txtSponsor.Text = dgvFile.CurrentRow.Cells["SponsorName"].Value.ToString();
            txtPONo.Text = dgvFile.CurrentRow.Cells["PONo"].Value.ToString();
            txtAmount.Text = dgvFile.CurrentRow.Cells["Amount"].Value.ToString();
            if (dgvFile.CurrentRow.Cells["PODate"].Value != DBNull.Value)
                dtpPODate.Value = Convert.ToDateTime(dgvFile.CurrentRow.Cells["PODate"].Value);
            else
                dtpPODate.Value = Convert.ToDateTime("1/1/2000");
            txtName.Text = dgvFile.CurrentRow.Cells["FilePath"].Value.ToString();
            txtSponsorID.Text = dgvFile.CurrentRow.Cells["SponsorID"].Value.ToString();
            //PO Status
            var POStatus = dgvFile.CurrentRow.Cells["POStatus"].Value.ToString();
            lblStatus.Text = POStatus;
            chkPOStatus.ForeColor = Color.FromArgb(192, 0, 0);
            chkPOStatus.Checked = false;
            if (POStatus.ToUpper() == "CANCELED")
            {
                chkPOStatus.Checked = true;
                
            }
            //PO Notes
            txtPONotes.Text= dgvFile.CurrentRow.Cells["PO Notes"].Value.ToString();
            //'Current Yr Amt'
            txtCrntYrAmnt.Text = dgvFile.CurrentRow.Cells["Current Yr Amt"].Value.ToString();
            //'Next Years Amt'
            txtNxtYrsAmnt.Text = dgvFile.CurrentRow.Cells["Next Years Amt"].Value.ToString();
        }

        private bool MatchingRecord(string strKeyField, string strMatchField, string strTableName, string strMatchData)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand("SELECT " + strKeyField + ", " + strMatchField + " FROM " + strTableName +
                                               " WHERE " + strMatchField + "='" + strMatchData + "'", sqlcnn);
            SqlDataReader sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                if (nMode == 1)
                    return true;
                else
                {
                    sqldr.Read();
                    string strID = Convert.ToString(sqldr.GetValue(0));
                    if (strID != txtPONo.Text)
                        return true;
                }
            }
            sqldr.Close(); sqlcmd.Dispose();
            sqlcnn.Close(); sqlcnn.Dispose();
            return false;
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
            //Header
            dgvFile.Columns["SponsorName"].HeaderText = "SPONSOR NAME";
            dgvFile.Columns["PONo"].HeaderText = "PO NO.";
            dgvFile.Columns["PODate"].HeaderText = "PO DATE";
            dgvFile.Columns["Amount"].HeaderText = "TOTAL AMOUNT ($)";
            dgvFile.Columns["Current Yr Amt"].HeaderText = "CURRENT YEAR AMOUNT ($)";
            dgvFile.Columns["Next Years Amt"].HeaderText = "NEXT YEARS AMOUNT ($)";
            dgvFile.Columns["FilePath"].HeaderText = "FILE LOCATION/NAME";
            dgvFile.Columns["POStatus"].HeaderText = "PO STATUS";
            //Columns Width
            dgvFile.Columns["SponsorName"].Width = 350;
            dgvFile.Columns["PONo"].Width = 150;
            dgvFile.Columns["Amount"].Width = 150;
            dgvFile.Columns["Current Yr Amt"].Width = 150;
            dgvFile.Columns["Next Years Amt"].Width = 150;
            dgvFile.Columns["PODate"].Width = 150;
            dgvFile.Columns["FilePath"].Width = 500;
            dgvFile.Columns["POStatus"].Width = 100;
            dgvFile.Columns["PO Notes"].Width = 500;
           
            //Columns Style
            dgvFile.Columns["PODate"].DefaultCellStyle.Format = "MM/dd/yyyy";
            dgvFile.Columns["PODate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.Columns["Amount"].DefaultCellStyle.Format = "#,##0.00";
            dgvFile.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //Current Yr Amt
            dgvFile.Columns["Current Yr Amt"].DefaultCellStyle.Format = "#,##0.00";
            dgvFile.Columns["Current Yr Amt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //Next Yr Amt
            dgvFile.Columns["Next Years Amt"].DefaultCellStyle.Format = "#,##0.00";
            dgvFile.Columns["Next Years Amt"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //Visibility
            dgvFile.Columns["SponsorID"].Visible = false;
            dgvFile.Columns["PO Notes"].Visible = true;
           // dgvFile.Columns["Next Years Amt"].Visible = false;
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
            txtPONo.ReadOnly = false; txtSponsorID.ReadOnly = false; txtSponsor.ReadOnly = false;
            dtpPODate.Value = DateTime.Now;
            txtSponsor.Focus();
        }

        private void EditRecord()
        {
            nMode = 2;
            OpenControls(this.pnlRecord, true);
            LoadData();
            txtPONo.ReadOnly = true; txtSponsorID.ReadOnly = true; txtSponsor.ReadOnly = true;
            txtName.Focus(); btnClose.Visible = false;
        }

        private void DeleteRecord()
        {
            LoadData();

            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to delete this record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.Yes)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ;
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;
                sqlcmd.Parameters.Add(new SqlParameter("@SponsorID", SqlDbType.Int));
                sqlcmd.Parameters["@SponsorID"].Value = Convert.ToInt16(txtSponsorID.Text);

                sqlcmd.Parameters.Add(new SqlParameter("@PONo", SqlDbType.NVarChar));
                sqlcmd.Parameters["@PONo"].Value = txtPONo.Text;

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spDelPO";

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
            if (txtPONo.Text.Trim() == "")
            {
                MessageBox.Show("PO No. is blank.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPONo.Focus();
                return;
            }

            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please enter location/file name", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtName.Focus();
                return;
            }

            if (txtSponsorID.Text.Trim() == "")
            {
                MessageBox.Show("Please select Sponsor", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSponsor.Focus();
                return;
            }

            if (MatchingRecord("PONo", "PONo", "POMaster", txtPONo.Text) == true)
            {
                MessageBox.Show("Matching record found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPONo.Focus();
                return;
            }

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problem encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            ;
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@nMode", nMode);
            sqlcmd.Parameters.AddWithValue("@SponsorID", Convert.ToInt16(txtSponsorID.Text));
            sqlcmd.Parameters.AddWithValue("@PONo", txtPONo.Text.ToUpper());
            if (dtpPODate.Value.ToShortDateString() == Convert.ToDateTime("1/1/2000").ToShortDateString() ||
                dtpPODate.Checked == false)
                sqlcmd.Parameters.AddWithValue("@PODate", DBNull.Value);
            else
                sqlcmd.Parameters.AddWithValue("@PODate", dtpPODate.Value);
            sqlcmd.Parameters.AddWithValue("@Amount", Convert.ToDecimal(txtAmount.Text));
            sqlcmd.Parameters.AddWithValue("@FilePath", txtName.Text.Trim());
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);
            //PO Status 6 - for Canceled and for all other statuses
            byte nPOStatus = 0;
            if (chkPOStatus.Checked == true)
            {                
                nPOStatus = 6;
            }
            sqlcmd.Parameters.AddWithValue("@POStatus", nPOStatus);
            //PO Notes
            string strNotes = txtPONotes.Text;
            strNotes = FormatingSpecialChars(strNotes);
            //var regex = new Regex(@"[^a-zA-Z0-9\s]");
            //if (!regex.IsMatch(strNotes))
            //{
            //    e.Handled = true;
            //}

            sqlcmd.Parameters.AddWithValue("@PONotes", strNotes);
            sqlcmd.Parameters.AddWithValue("@CrntYrAmnt", Convert.ToDecimal(txtCrntYrAmnt.Text));
            sqlcmd.Parameters.AddWithValue("@NxtYrsAmnt", Convert.ToDecimal(txtNxtYrsAmnt.Text));
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spAddEditPO";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                int code = System.Runtime.InteropServices.Marshal.GetExceptionCode();
                if (code == -532462766)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
            dgvFile.Refresh();
            pnlRecord.Visible = false;
            dgvFile.Visible = true;
            bnFile.Enabled = true;
            chkShowInactive.Checked = false;
            AddEditMode(false);
            LoadRecords();
            PSSClass.General.FindRecord("PONo", txtPONo.Text, bsFile, dgvFile);
            ClearControls(this.pnlRecord);
            nMode = 0;
        }
        public string FormatingSpecialChars(string strInput)
        {
            string strInputParam = strInput;
            strInputParam = strInputParam.Replace("'", "`");
            strInputParam = strInputParam.Replace("--", "-");            
            strInputParam = strInputParam.Replace("&amp;", "&");
            strInputParam = strInputParam.Replace("&AMP;", "&");
            //strInputParam = strInputParam.Replace("\r\n", "<br />");
            strInputParam = strInputParam.Replace("<br />", "\r\n");
            return strInputParam;
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
            AddEditMode(false);
            LoadRecords();
            pnlRecord.Visible = false; dgvFile.Visible = true; bnFile.Enabled = true;
            nMode = 0;
        }

        //private void dvwSetUp(DataGridView dgvObj, DataView dvw)
        //{
        //    dgvObj.Columns[0].Width = 142;
        //    dgvObj.Columns[1].Visible = false;
        //    dgvObj.DataSource = dvw;
        //}

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
            dgvSponsors.Visible = false;
            this.Close();
        }

        private void txtSponsorID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (nMode == 1)
            {
                if (e.KeyChar == 13)
                {
                    if (txtSponsorID.Text.Trim() != "")
                    {
                        txtSponsor.Text = PSSClass.Sponsors.SpName(Convert.ToInt16(txtSponsorID.Text));
                        dgvSponsors.Visible = false;
                    }
                }
                else if (e.KeyChar == 27)
                {
                    dgvSponsors.Visible = false;
                }
                else
                {
                    txtSponsor.Text = "";
                }
            }
        }

        private void txtSponsor_TextChanged(object sender, EventArgs e)
        {
            if (nMode != 0)
            {
                DataView dvwSponsors;
                dvwSponsors = new DataView(dtSponsors, "SponsorName like '%" + txtSponsor.Text.Replace("'", "''").Trim() + "%'", "SponsorName", DataViewRowState.CurrentRows);
                PSSClass.General.DGVSetUp(dgvSponsors, dvwSponsors, 375);
            }
        }

        private void txtSponsorEnterHandler(object sender, EventArgs e)
        {
            if (nMode == 1)
            {
                dgvSponsors.Visible = true; dgvSponsors.BringToFront();
            }
            else
                dgvSponsors.Visible = false;
        }

        private void picSponsors_Click(object sender, EventArgs e)
        {
            LoadSponsorsDDL();
            dgvSponsors.Visible = true; dgvSponsors.BringToFront();
        }

        private void dgvSponsors_DoubleClick(object sender, EventArgs e)
        {
            txtSponsor.Text = dgvSponsors.CurrentRow.Cells[0].Value.ToString();
            txtSponsorID.Text = dgvSponsors.CurrentRow.Cells[1].Value.ToString();
            dgvSponsors.Visible = false;
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
                dgvSponsors.Visible = false;
            }
            else if (e.KeyChar == 27)
            {
                dgvSponsors.Visible = false;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (nMode == 1 || nMode == 2)
            {
                this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

                // Set the file dialog to filter for graphics files. 
                this.openFileDialog1.Filter =
                    "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|" +
                    "PDF (*.PDF)|*.PDF|" +
                    "All files (*.*)|*.*";

                // Allow the user to select multiple images. 
                this.openFileDialog1.Multiselect = false;
                this.openFileDialog1.Title = "SELECT SCANNED PO FILE";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtName.Text = openFileDialog1.FileName;
                }
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() != "")
                System.Diagnostics.Process.Start(@txtName.Text);

            //POView vwPO = new POView();
            //vwPO.WindowState = FormWindowState.Maximized;
            //vwPO.imgFile = txtName.Text;
            //vwPO.Text = "PO " + txtPONo.Text;
            //vwPO.ShowDialog();
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

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                pnlRecord.Location = PointToClient(this.pnlRecord.PointToScreen(new Point(e.X - mousePos.X, e.Y - mousePos.Y)));
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

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseDown = false;
            }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            nMode = 0; pnlRecord.Visible = false; dgvFile.Visible = true; dgvFile.BringToFront(); btnClose.Visible = false;
            dgvFile.Focus();
            FileAccess();
        }

        private void POMaster_Load(object sender, EventArgs e)
        {
            strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "POMaster");

            LoadSponsorsDDL();
            LoadRecords();
            Populate_cmbStatusPO();
            BuildPrintItems();
            BuildSearchItems();
        }

        private void POMaster_KeyDown(object sender, KeyEventArgs e)
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

        private void ChkShowInactive_CheckedChanged(object sender, EventArgs e)
        {
            LoadRecordsByStatus();
        }

        private void CmbStatusPO_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboBox mycmb = (ComboBox)sender;
                var strSelectedIndex = mycmb.SelectedIndex;
                if (strSelectedIndex != -1)
                {
                    string strSelectedValue = cmbStatusPO.SelectedValue.ToString();
                    if (!string.IsNullOrEmpty(strSelectedValue) && (strSelectedValue != "System.Data.DataRowView"))
                    {
                        Int32 intSelectedValue = Convert.ToInt32(strSelectedValue);
                        LoadRecordsByStatus(intSelectedValue);
                    }
                }                
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private void ChkPOStatus_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void Populate_cmbStatusPO()
        {
            
            try
            {
                DataTable dt = new DataTable();
                dt = PSSClass.PO.UniversalPO("PO_Statuses");
                cmbStatusPO.DataSource = dt;
                cmbStatusPO.DisplayMember = "StsName";
                cmbStatusPO.ValueMember = "StsID";
                cmbStatusPO.SelectedIndex = 0;
            
            }
            catch (Exception)
            {

                throw;
            }
           
           
        }

      
    }
}
