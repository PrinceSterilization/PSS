using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Messaging;
using System.Data.SqlClient;

namespace GIS
{
    public partial class GISMessenger : Form
    {
        public int nUID;
        private int nCtr = 0;
 
        private int pUID = 0;
        private DataTable dtInMessages = new DataTable();
        private DataTable dtEmployees = new DataTable();

        BackgroundWorker m_oWorker;
        private byte bWSw = 0;
        byte bCancelled = 0;

        public GISMessenger()
        {
            InitializeComponent();
            m_oWorker = new BackgroundWorker();
            m_oWorker.WorkerSupportsCancellation = true;
            // Create a background worker thread that ReportsProgress &
            // SupportsCancellation
            // Hook up the appropriate events.

            m_oWorker.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
            //m_oWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_oWorker_RunWorkerCompleted);

            //timer1.Enabled = true;
            lblProfile.Visible = true;
            lblProfile.Text = "Loading employees list...please standby!";
        }


        private void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            byte bSw = 0;
            while (bCancelled == 0)
            {
                if (m_oWorker.CancellationPending == false)
                {
                    if (bSw == 0)
                    {
                        dtEmployees = GISClass.Employees.EmpNames();
                        bsSDNames.DataSource = dtEmployees;
                        //Messenger childForm = new Messenger();
                        //childForm.ShowDialog();
                        //childForm.BringToFront();
                        System.Threading.Thread.Sleep(500);
                        LoadEmployees();
                        bSw = 1;
                    }
                }
                else
                {
                    e.Cancel = true;
                    bCancelled = 1;
                }
            }
        }

        private void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            m_oWorker.Dispose();
            m_oWorker = null;
            if (bWSw == 0)
            {
                
            }
        }

            
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (pUID == 0 || pUID == nUID)
            {
                lblProfile.Visible = true;
                lblProfile.Text = "Please select message recipient.";
                timer1.Enabled = true;
                nCtr = 0;
                return;
            }
            try
            {
                MessageQueue msgQ;
                string strCompID = "GLSQL03";
                string strUserID = GISClass.Users.LogID(pUID);

                msgQ = new MessageQueue("FormatName:DIRECT=OS:" + strCompID + "\\Private$\\" + strUserID);
                msgQ.Send(rtbNewMessage.Text, "Message from " + LogIn.strUserID);

                SqlConnection sqlcnn = GISClass.DBConnection.GISConnection();
                if (sqlcnn == null)
                {
                    MessageBox.Show("Connection problen encountered." + Environment.NewLine + "Please try again later.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@MsgDate", DateTime.Now);
                sqlcmd.Parameters.AddWithValue("@MsgOut", rtbNewMessage.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@RecID",  pUID);
                sqlcmd.Parameters.AddWithValue("@SenderID", nUID);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spMessageSave";

                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblProfile.Visible = true;
                    lblProfile.Text = "Error encountered - " + ex.Message + "Please call the IT Software Team.";
                    timer1.Enabled = true;
                    nCtr = 0;
                    sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                    return;
                }
                sqlcmd.Dispose(); sqlcnn.Close(); sqlcnn.Dispose();
                rtbNewMessage.Clear();
            }

            catch (MessageQueueException ex)
            {
                //MessageBox.Show(ex.Message); 
                lblProfile.Visible = true;
                lblProfile.Text = ex.Message;
                timer1.Enabled = true;
                nCtr = 0;
                return;
                
            }
            ShowMessages();
        }

        private void LoadEmployees()
        {
            if (dtEmployees != null && dtEmployees.Rows.Count > 0)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                bnEmployees.BindingSource = bsSDNames;
                dgvSDNames.DataSource = bsSDNames;
                picImage.BackgroundImage = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\Logo.jpg");
                dgvSDNames.Columns["EmpName"].Width = 162;
                dgvSDNames.Columns["PicImage"].Width = 40;
                dgvSDNames.Columns["NoMessages"].Width = 105;
                dgvSDNames.Columns["LoginName"].Visible = false;
                dgvSDNames.Columns["EmployeeID"].Visible = false;

                    foreach (DataGridViewRow row in dgvSDNames.Rows)
                    {
                        Image image;
                        if (System.IO.File.Exists(@"\\gblnj6\gbldata$\GIS\hr\" + row.Cells["LoginName"].Value.ToString() + ".jpg") == true)
                        {
                            image = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\" + row.Cells["LoginName"].Value.ToString() + ".jpg");
                        }
                        else
                        {
                            image = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\Logo.jpg");
                        }
                        DataGridViewImageCell cell = row.Cells[1] as DataGridViewImageCell;
                        cell.Value = image;
                        cell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    }
                });
                //for (int i = 0; i < dtEmployees.Rows.Count; i++)
                //{
                //    Image image;
                //    if (System.IO.File.Exists(@"\\gblnj6\gbldata$\GIS\hr\" + dtEmployees.Rows[i]["LoginName"].ToString() + ".jpg") == true)
                //    {
                //        image = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\" + dtEmployees.Rows[i]["LoginName"].ToString() + ".jpg");
                //    }
                //    else
                //    {
                //        image = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\Logo.jpg");
                //    }
                //    //this.dgvSDNames.Rows.Add(dtEmployees.Rows[i]["EmpName"], image, "0 Message(s)", dtEmployees.Rows[i]["LoginName"], dtEmployees.Rows[i]["EmployeeID"]);
                //    DataGridViewImageCell cell = (DataGridViewImageCell)dgvSDNames.Rows[i].Cells[1];
                //    cell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                //}
            }
        }

        private void ShowMessages()
        {
            try
            {
                MessageQueue msgQ;
                System.Messaging.Message m = new System.Messaging.Message();

                string strCompID = "GLSQL03";
                string strUserID = GISClass.Users.LogID(LogIn.nUserID);
                msgQ = new MessageQueue("FormatName:DIRECT=OS:" + strCompID + "\\Private$\\" + strUserID);
                m = msgQ.Receive(new TimeSpan(0, 0, 1));
            }
            catch { }

            int nRange = 0;
            if (rdoThisDate.Checked == true)
                nRange = 0;
            else if (rdoExtendTo.Checked == true)
                nRange = 1;
            else if (rdoShowAll.Checked == true)
                nRange = 2;
            
            if (pUID != 0)
            {
                rtbMessages.Clear();

                try
                {
                    DateTime dte = Convert.ToDateTime(dtpDate.Value);
                    DateTime dTDte = Convert.ToDateTime(dtpExtDate.Value);

                    string strFDte = dte.ToString("MM/dd/yyyy");
                    string strTDte = dTDte.ToString("MM/dd/yyyy");
                    string strDashes = new String('_', 70); ;

                    DataTable dt = GISClass.Messaging.Messages(LogIn.nUserID, pUID, strFDte, strTDte, nRange);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            rtbMessages.SelectionColor = Color.Blue;
                            if (dt.Rows[i]["Sender"].ToString() == GISClass.Users.LogID(LogIn.nUserID))
                                rtbMessages.Text = rtbMessages.Text + Environment.NewLine + Environment.NewLine + dt.Rows[i]["MessageDate"] + Environment.NewLine +
                                "Me" + Environment.NewLine + Environment.NewLine + dt.Rows[i]["MessageOut"] + Environment.NewLine + strDashes;
                            else
                                rtbMessages.Text = rtbMessages.Text + Environment.NewLine + Environment.NewLine + dt.Rows[i]["MessageDate"] + Environment.NewLine +
                                    "*** " + dt.Rows[i]["Sender"] + " ***" + Environment.NewLine + Environment.NewLine + dt.Rows[i]["MessageOut"] + Environment.NewLine + strDashes;
                        }
                    }
                }
                catch { }
            }
            rtbMessages.SelectionStart = rtbMessages.Text.LastIndexOfAny(Environment.NewLine.ToCharArray()) + 1;
            rtbMessages.ScrollToCaret();
        }

        private void GISMessenger_Load(object sender, EventArgs e)
        {
            this.Location = new Point(1024, 75);

           
            m_oWorker.RunWorkerAsync();

            //Temporary Fix for J Mastej (114), Marlyn Moreno 
            //===============================================
            if (LogIn.nUserID == 114 || LogIn.nUserID == 394)
            {
                float width_ratio = (Screen.PrimaryScreen.Bounds.Width / 1600f);// 1920f 1920f
                float heigh_ratio = (Screen.PrimaryScreen.Bounds.Height / 1024f);// 1080f

                SizeF scale = new SizeF(width_ratio, heigh_ratio);
                this.Scale(scale);

                foreach (Control control in this.Controls)
                {
                    control.Font = new Font("Arial", control.Font.SizeInPoints * heigh_ratio * width_ratio);
                }
                this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            }
            else
            {
                this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            }
        }

        private void GISMessenger_Activated(object sender, EventArgs e)
        {
            this.Text = "GIS Messenger : " + LogIn.strUserID;
            this.WindowState = FormWindowState.Normal;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            nCtr++;
            if (nCtr > 2)
            {
                lblProfile.Visible = false;
                nCtr = 0;
                timer1.Enabled = false;
                dgvSDNames.FirstDisplayedCell = null;
                dgvSDNames.ClearSelection();
                btnClose.Select();
            }
        }

        private void rtbNewMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            timer1.Enabled = false;
        }

        private void dtpDate_ValueChanged(object sender, EventArgs e)
        {
            ShowMessages();
        }

        //private void bgwMessenger_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker worker = sender as BackgroundWorker;
        //    while (true)
        //    {
        //        if ((worker.CancellationPending == true))
        //        {
        //            e.Cancel = true;
        //            break;
        //        }
        //        else
        //        {
        //            // Perform a time consuming operation and report progress.
        //            System.Threading.Thread.Sleep(500);
        //            if (dtEmployees == null || dtEmployees.Rows.Count == 0)
        //            {
        //                picImage.BackgroundImage = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\Logo.jpg");
        //                dtEmployees = GISClass.Employees.EmpNames();
        //                bsSDESign.DataSource = dtEmployees;
        //                bnEmployees.BindingSource = bsSDESign;
        //                dgvSDNames.DataSource = bsSDESign;
        //                dgvSDNames.Columns["EmpName"].Width = 162;
        //                dgvSDNames.Columns["PicImage"].Width = 40;
        //                dgvSDNames.Columns["NoMessages"].Width = 105;
        //                dgvSDNames.Columns["LoginName"].Visible = false;
        //                dgvSDNames.Columns["EmployeeID"].Visible = false;



        //                foreach (DataGridViewRow row in dgvSDNames.Rows)
        //                {
        //                    Image image;
        //                    if (System.IO.File.Exists(@"\\gblnj6\gbldata$\GIS\hr\" + row.Cells["LoginName"].Value.ToString() + ".jpg") == true)
        //                    {
        //                        image = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\" + row.Cells["LoginName"].Value.ToString() + ".jpg");
        //                    }
        //                    else
        //                    {
        //                        image = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\Logo.jpg");
        //                    }
        //                    DataGridViewImageCell cell = row.Cells[1] as DataGridViewImageCell;
        //                    cell.Value = image;
        //                    cell.ImageLayout = DataGridViewImageCellLayout.Stretch;
        //                }
        //                //for (int i = 0; i < dtEmployees.Rows.Count; i++)
        //                //{
        //                //    Image image;
        //                //    if (System.IO.File.Exists(@"\\gblnj6\gbldata$\GIS\hr\" + dtEmployees.Rows[i]["LoginName"].ToString() + ".jpg") == true)
        //                //    {
        //                //        image = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\" + dtEmployees.Rows[i]["LoginName"].ToString() + ".jpg");
        //                //    }
        //                //    else
        //                //    {
        //                //        image = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\Logo.jpg");
        //                //    }
        //                //    //this.dgvSDNames.Rows.Add(dtEmployees.Rows[i]["EmpName"], image, "0 Message(s)", dtEmployees.Rows[i]["LoginName"], dtEmployees.Rows[i]["EmployeeID"]);
        //                //    DataGridViewImageCell cell = (DataGridViewImageCell)dgvSDNames.Rows[i].Cells[1];
        //                //    cell.ImageLayout = DataGridViewImageCellLayout.Stretch;
        //                //}
        //            }
        //            try
        //            {
        //                MessageQueue msgQ;
        //                System.Messaging.Message m = new System.Messaging.Message();

        //                string strCompID = "GLSQL03";
        //                string strUserID = GISClass.Users.LogID(LogIn.nUserID);
        //                msgQ = new MessageQueue("FormatName:DIRECT=OS:" + strCompID + "\\Private$\\" + strUserID);

        //                int nMsgCtr = 0;

        //                //var enumerator = msgQ.GetMessageEnumerator2();
        //                MessageEnumerator Enumerator = msgQ.GetMessageEnumerator2();
        //                msgQ.MessageReadPropertyFilter.SetAll();

        //                // loop through all the messages 
        //                while (Enumerator.MoveNext())
        //                {
        //                    nMsgCtr++;
        //                }
        //                if (nMsgCtr > 0)
        //                    ShowMessages();
        //                break;
        //            }
        //            catch
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}

        //private void bgwMessenger_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (!bgwMessenger.IsBusy)
        //       bgwMessenger.RunWorkerAsync();
        //}

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dgvSDNames_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (dgvSDNames.Rows.Count > 0 && dgvSDNames.CurrentRow.Index != -1)
                {
                    pUID = Convert.ToInt16(dgvSDNames.Rows[dgvSDNames.CurrentCell.RowIndex].Cells["EmployeeID"].Value);
                    if (System.IO.File.Exists(@"\\gblnj6\gbldata$\GIS\hr\" + dtEmployees.Rows[dgvSDNames.CurrentCell.RowIndex]["LoginName"].ToString() + ".jpg") == true)
                    {
                        picImage.BackgroundImage = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\" + dtEmployees.Rows[dgvSDNames.CurrentCell.RowIndex]["LoginName"].ToString() + ".jpg");
                    }
                    else
                    {
                        picImage.BackgroundImage = Image.FromFile(@"\\gblnj6\gbldata$\GIS\hr\Logo.jpg");
                    }
                    picImage.Visible = true;
                }
            }
            catch { }
            rtbMessages.Clear(); rdoThisDate.Checked = false; rdoExtendTo.Checked = false; rdoShowAll.Checked = false; 
        }

        private void rdoThisDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoThisDate.Checked == true)
                ShowMessages();
        }

        private void rdoExtendTo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoExtendTo.Checked == true)
                ShowMessages();
        }

        private void rdoShowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoShowAll.Checked == true)
                ShowMessages();
        }

        private void btnClose_Enter(object sender, EventArgs e)
        {
            try
            {
                this.dgvSDNames.CurrentCell = this.dgvSDNames[0, dgvSDNames.Rows.Count - 1];
                this.dgvSDNames.CurrentCell = this.dgvSDNames[0, 1];
            }
            catch { }
            picImage.Visible = false;
        }
    }
}
