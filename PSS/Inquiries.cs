using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;

namespace GIS
{
    public partial class Inquiries : GIS.TemplateForm
    {
        SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
        private byte nMode = 0;

        private bool mouseDown;
        private Point mousePos;
        private string[] arrCol;
        private int nIndex;
        private int nCtr = 0;
        private int nSw = 0;
        private int visitid = 0;
        private DataTable dtSponsors = new DataTable();
        private DataTable dtVisitors = new DataTable();
        private string strFileAccess = "RO";

        public Inquiries()
        {
            InitializeComponent();
            //bnMoveFirst.Click += new EventHandler(MoveRecordClickHandler);
            //bnMoveLast.Click += new EventHandler(MoveRecordClickHandler);
            //bnMoveNext.Click += new EventHandler(MoveRecordClickHandler);
            //bnMovePrevious.Click += new EventHandler(MoveRecordClickHandler);

            tsbAdd.Click += new EventHandler(AddClickHandler);
            //tsbEdit.Click += new EventHandler(EditClickHandler);
            //tsbDelete.Click += new EventHandler(DeleteClickHandler);
            //tsbSave.Click += new EventHandler(SaveClickHandler);
            //tsbCancel.Click += new EventHandler(CancelClickHandler);
            tsbExit.Click += new EventHandler(CloseClickHandler);
            //tsbSearch.Click += new EventHandler(SearchOKClickHandler);
            //tsbFilter.Click += new EventHandler(SearchFilterClickHandler);
            //tsbRefresh.Click += new EventHandler(RefreshClickHandler);
            //tstbSearch.KeyPress += new KeyPressEventHandler(SearchKeyPressHandler);
            //dgvFile.DoubleClick += new EventHandler(dgvDoubleClickHandler);
            //dgvFile.KeyPress += new KeyPressEventHandler(dgvKeyPressHandler);
            //dgvFile.KeyDown += new KeyEventHandler(dgvKeyDownHandler);
            //dgvFile.CellMouseClick += new DataGridViewCellMouseEventHandler(dgvCellMouseClickEventHandler);
            //dgvFile.CurrentCellChanged += new EventHandler(dgvCellChangedHandler);
            //cklColumns.SelectedIndexChanged += new EventHandler(cklSelIdxChEventHandler);
        }

        private void AddClickHandler(object sender, EventArgs e)
        {
            AddRecord();
        }

        private void EditClickHandler(object sender, EventArgs e)
        {
            //EditRecord();
        }

        private void SaveClickHandler(object sender, EventArgs e)
        {
            //SaveRecord();
        }

        private void AddRecord()
        {
            nMode = 1;
            pnlRecord.Visible = true; pnlRecord.BringToFront(); dgvFile.Visible = false; btnClose.Visible = false;

            ClearControls(pnlRecord);
            OpenControls(pnlRecord, false);


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

        private void Inquiries_Load(object sender, EventArgs e)
        {
            strFileAccess = GISClass.General.UserFileAccess(LogIn.nUserID, "Inquiries");
            FileAccess();
            //LoadRecords();
        }

        private void FileAccess()
        {
            //Reload User's Access to this file - included in this function for sudden change in access level

            if (strFileAccess == "RO")
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false;
            }
            else if (strFileAccess == "RW")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true;
            }
            else if (strFileAccess == "FA")
            {
                tsbAdd.Enabled = true; tsbEdit.Enabled = true; tsddbPrint.Enabled = true;
            }
            else
            {
                tsbAdd.Enabled = false; tsbEdit.Enabled = false; tsddbPrint.Enabled = false;
            }
            tsddbSearch.Enabled = true;
        }

        private void LoadRecords()
        {
            DataTable dt = GISClass.Versions.VersionUsers();
            bsFile.DataSource = dt;
            bnFile.BindingSource = bsFile;
            dgvFile.DataSource = bsFile;
            bsFile.Filter = "InquiryID <> 0";
            DataGridSetting();
            try
            {
                if (tsddbSearch.DropDownItems.Count == 0)
                {
                    int i = 0;

                    arrCol = new string[dt.Columns.Count];

                    ToolStripMenuItem[] items = new ToolStripMenuItem[arrCol.Length - 4];// ToolStripMenuItem[arrCol.Length - 4];

                    foreach (DataColumn colFile in dt.Columns)
                    {
                        if (colFile.ColumnName.ToString() != "DateCreated" &&
                            colFile.ColumnName.ToString() != "CreatedByID" &&
                            colFile.ColumnName.ToString() != "LastUpdate" &&
                            colFile.ColumnName.ToString() != "UserID")
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
                    }
                    for (int j = 0; j < cklColumns.Items.Count; j++)
                    {
                        cklColumns.SetItemChecked(j, true);
                    }
                    tsddbSearch.DropDownItems.AddRange(items);
                    tslSearchData.Text = tsddbSearch.DropDownItems[1].Text;
                    tstbSearchField.Text = tsddbSearch.DropDownItems[1].Name;
                }
            }
            catch (Exception c)
            {
                MessageBox.Show(c.ToString());
            }
            tsbEdit.Enabled = false;
        }


        private void DataGridSetting()
        {
            dgvFile.EnableHeadersVisualStyles = false;
            dgvFile.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFile.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgvFile.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvFile.Columns["VersionNo"].HeaderText = "VERSION NO";
            dgvFile.Columns["UserName"].HeaderText = "USER NAME";
            dgvFile.Columns["UserName"].Width = 350;
            dgvFile.Columns["VersionNo"].Width = 250;
            dgvFile.Columns["UserID"].Visible = false;
            dgvFile.Columns["DateCreated"].Visible = false;
            dgvFile.Columns["CreatedByID"].Visible = false;
            dgvFile.Columns["LastUpdate"].Visible = false;
            dgvFile.Columns["LastUserID"].Visible = false;
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
    }
}
