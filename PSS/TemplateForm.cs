//TemplateForm.cs
// AUTHOR       : ALEXANDER M. DELA CRUZ
// TITLE        : Software Developer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Source for Maintenance Forms to be inherited

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSS
{
    public partial class TemplateForm : Form
    {
        public static string rptName = "";
        public static string rptTitle = "";
        public static int rptScope = 1;

        public TemplateForm()
        {
            InitializeComponent();
        }

        private void TemplateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose(true);
            e.Cancel = true; 
        }

        private void TemplateForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void tsbSearch_Click(object sender, EventArgs e)
        {
        }
      
        public void AddEditMode(bool bState)
        {
            tlsFile.TabStop = !bState;
            tsbAdd.Enabled = !bState; tsbEdit.Enabled = !bState; tsbDelete.Enabled = false;
            tsbSave.Enabled = bState; tsbCancel.Enabled = bState;
            tsbSearch.Enabled = !bState; tsddbPrint.Enabled = !bState; tsbFilter.Enabled = !bState;
            tsddbSearch.Enabled = !bState; bnFile.Enabled = !bState;
        }

        public static void ClearControls(Control c)
        {
            foreach (Control Ctrl in c.Controls)
            {
                switch (Ctrl.GetType().ToString())
                {
                    case "System.Windows.Forms.CheckBox":
                        ((CheckBox)Ctrl).Checked = false;
                        break;

                    case "System.Windows.Forms.TextBox":
                        ((TextBox)Ctrl).Text = "";
                        break;

                    case "GISControls.TextBoxChar": //Custom Control
                        ((GISControls.TextBoxChar)Ctrl).Text = "";
                        break;

                    case "GISControls.TextBoxAdjHt": //Custom Control
                        ((GISControls.TextBoxAdjHt)Ctrl).Text = "";
                        break;

                    case "System.Windows.Forms.RichTextBox":
                        ((RichTextBox)Ctrl).Text = "";
                        break;

                    case "System.Windows.Forms.RadioButton":
                        ((RadioButton)Ctrl).Checked = false;
                        break;

                    case "System.Windows.Forms.ComboBox":
                        ((ComboBox)Ctrl).SelectedIndex = -1;
                        ((ComboBox)Ctrl).Text = "";
                        break;

                    case "System.Windows.Forms.MaskedTextBox":
                        ((MaskedTextBox)Ctrl).Text = "";
                        break;

                    default:
                        if (Ctrl.Controls.Count > 0)
                            ClearControls(Ctrl);
                        break;
                }
            }
        }

        public static void OpenControls(Control c, bool b)
        {
            foreach (Control Ctrl in c.Controls)
            {
                switch (Ctrl.GetType().ToString())
                {
                    case "System.Windows.Forms.CheckBox":
                        ((CheckBox)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.RadioButton":
                        ((RadioButton)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.TextBox":
                        ((TextBox)Ctrl).ReadOnly = !b;
                        break;

                    case "GISControls.TextBoxChar": //User Control
                        ((GISControls.TextBoxChar)Ctrl).ReadOnly = !b;
                        if (((GISControls.TextBoxChar)Ctrl).Name == "txtPONo")
                        {
                            string strText=((GISControls.TextBoxChar)Ctrl).Text.ToString();
                            ((GISControls.TextBoxChar)Ctrl).ReadOnly = b;
                        }
                       
                        break;

                    case "GISControls.TextBoxAdjHt": //User Control
                        ((GISControls.TextBoxAdjHt)Ctrl).ReadOnly = !b;
                        break;

                    case "System.Windows.Forms.RichTextBox":
                        ((RichTextBox)Ctrl).ReadOnly = !b;
                        break;

                    case "System.Windows.Forms.ComboBox":
                        ((ComboBox)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.MaskedTextBox":
                        ((MaskedTextBox)Ctrl).ReadOnly = !b;
                        break;

                    case "System.Windows.Forms.DateTimePicker":
                        ((DateTimePicker)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.DataGridView":
                        ((DataGridView)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.GroupBox":
                        ((GroupBox)Ctrl).Enabled = b;
                        break;

                    default:
                        if (Ctrl.Controls.Count > 0)
                            OpenControls(Ctrl, true);
                        break;
                }
            }
            c.Enabled = true;
        }

        private void tsbAdd_Click(object sender, EventArgs e)
        {
            AddEditMode(true);
        }

        private void tsbEdit_Click(object sender, EventArgs e)
        {
            AddEditMode(true);
        }

        private void tstbSearch_Enter(object sender, EventArgs e)
        {
            tstbSearch.SelectAll(); 
        }

        public void dvwSetUp(DataGridView dgvObj, DataView dvw)
        {
            dgvObj.Columns[0].Width = 369;
            dgvObj.Columns[1].Visible = false;
            dgvObj.DataSource = dvw;
        }

        public void dvwSetUpWidth(DataGridView dgvObj, DataView dvw, Int16 colWidth)
        {
            dgvObj.Columns[0].Width = colWidth;
            dgvObj.Columns[1].Visible = false;
            dgvObj.DataSource = dvw;
        }

        public void StandardDGVSetting(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.DefaultCellStyle.Font = new Font("Arial", 8, GraphicsUnit.Point);
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void dgvFile_Click(object sender, EventArgs e)
        {
            try
            {
                int nC = dgvFile.CurrentCell.ColumnIndex;
                tsddbSearch.DropDownItems[nC].Select();
                tslSearchData.Text = tsddbSearch.DropDownItems[nC].Text;
                tstbSearchField.Text = tsddbSearch.DropDownItems[nC].Name;
            }
            catch { }
        }

        private void dgvFile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cklColumns.Visible = false;
        }

        private void dgvFile_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            tsbRefresh.Enabled = true;
        }

        private void tsddbPrint_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            rptName = e.ClickedItem.Name;
            rptTitle = e.ClickedItem.Text;
        }
    }
}
