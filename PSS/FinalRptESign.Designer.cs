namespace PSS
{
    partial class FinalRptESign
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvQA = new System.Windows.Forms.DataGridView();
            this.dgvSD = new System.Windows.Forms.DataGridView();
            this.dgvForEMail = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblQATotal = new System.Windows.Forms.Label();
            this.lblSDTotal = new System.Windows.Forms.Label();
            this.lblForEMail = new System.Windows.Forms.Label();
            this.dgvSDNames = new System.Windows.Forms.DataGridView();
            this.StudyDir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StudyDirID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SDImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.LoginName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsQAESign = new System.Windows.Forms.BindingSource(this.components);
            this.bsSDESign = new System.Windows.Forms.BindingSource(this.components);
            this.bsForEMail = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvForEMail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSDNames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQAESign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSDESign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsForEMail)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvQA
            // 
            this.dgvQA.AllowUserToAddRows = false;
            this.dgvQA.AllowUserToDeleteRows = false;
            this.dgvQA.AllowUserToResizeColumns = false;
            this.dgvQA.AllowUserToResizeRows = false;
            this.dgvQA.BackgroundColor = System.Drawing.Color.White;
            this.dgvQA.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvQA.Location = new System.Drawing.Point(12, 33);
            this.dgvQA.Name = "dgvQA";
            this.dgvQA.ReadOnly = true;
            this.dgvQA.Size = new System.Drawing.Size(970, 164);
            this.dgvQA.TabIndex = 0;
            this.dgvQA.DoubleClick += new System.EventHandler(this.dgvQA_DoubleClick);
            this.dgvQA.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvQA_KeyDown);
            this.dgvQA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvQA_KeyPress);
            // 
            // dgvSD
            // 
            this.dgvSD.AllowUserToAddRows = false;
            this.dgvSD.AllowUserToDeleteRows = false;
            this.dgvSD.AllowUserToResizeColumns = false;
            this.dgvSD.AllowUserToResizeRows = false;
            this.dgvSD.BackgroundColor = System.Drawing.Color.White;
            this.dgvSD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSD.Location = new System.Drawing.Point(12, 242);
            this.dgvSD.Name = "dgvSD";
            this.dgvSD.ReadOnly = true;
            this.dgvSD.Size = new System.Drawing.Size(800, 204);
            this.dgvSD.TabIndex = 2;
            this.dgvSD.DoubleClick += new System.EventHandler(this.dgvSD_DoubleClick);
            // 
            // dgvForEMail
            // 
            this.dgvForEMail.AllowUserToAddRows = false;
            this.dgvForEMail.AllowUserToDeleteRows = false;
            this.dgvForEMail.AllowUserToResizeColumns = false;
            this.dgvForEMail.AllowUserToResizeRows = false;
            this.dgvForEMail.BackgroundColor = System.Drawing.Color.White;
            this.dgvForEMail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvForEMail.Location = new System.Drawing.Point(12, 485);
            this.dgvForEMail.Name = "dgvForEMail";
            this.dgvForEMail.ReadOnly = true;
            this.dgvForEMail.Size = new System.Drawing.Size(970, 119);
            this.dgvForEMail.TabIndex = 3;
            this.dgvForEMail.DoubleClick += new System.EventHandler(this.dgvForEMail_DoubleClick);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Firebrick;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "QA Approval";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Firebrick;
            this.label2.Location = new System.Drawing.Point(9, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 21);
            this.label2.TabIndex = 5;
            this.label2.Text = "Study Directors";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Firebrick;
            this.label3.Location = new System.Drawing.Point(9, 466);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "Ready for E-Mail";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblQATotal
            // 
            this.lblQATotal.BackColor = System.Drawing.Color.DarkGreen;
            this.lblQATotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblQATotal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQATotal.ForeColor = System.Drawing.Color.White;
            this.lblQATotal.Location = new System.Drawing.Point(872, 200);
            this.lblQATotal.Name = "lblQATotal";
            this.lblQATotal.Size = new System.Drawing.Size(110, 20);
            this.lblQATotal.TabIndex = 7;
            this.lblQATotal.Text = "TOTAL :";
            this.lblQATotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSDTotal
            // 
            this.lblSDTotal.BackColor = System.Drawing.Color.DarkGreen;
            this.lblSDTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSDTotal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSDTotal.ForeColor = System.Drawing.Color.White;
            this.lblSDTotal.Location = new System.Drawing.Point(872, 449);
            this.lblSDTotal.Name = "lblSDTotal";
            this.lblSDTotal.Size = new System.Drawing.Size(110, 20);
            this.lblSDTotal.TabIndex = 8;
            this.lblSDTotal.Text = "TOTAL :";
            this.lblSDTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblForEMail
            // 
            this.lblForEMail.BackColor = System.Drawing.Color.DarkGreen;
            this.lblForEMail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblForEMail.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForEMail.ForeColor = System.Drawing.Color.White;
            this.lblForEMail.Location = new System.Drawing.Point(872, 609);
            this.lblForEMail.Name = "lblForEMail";
            this.lblForEMail.Size = new System.Drawing.Size(110, 20);
            this.lblForEMail.TabIndex = 9;
            this.lblForEMail.Text = "TOTAL :";
            this.lblForEMail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvSDNames
            // 
            this.dgvSDNames.AllowUserToAddRows = false;
            this.dgvSDNames.AllowUserToDeleteRows = false;
            this.dgvSDNames.BackgroundColor = System.Drawing.Color.White;
            this.dgvSDNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSDNames.ColumnHeadersVisible = false;
            this.dgvSDNames.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StudyDir,
            this.StudyDirID,
            this.SDImage,
            this.LoginName});
            this.dgvSDNames.Location = new System.Drawing.Point(817, 242);
            this.dgvSDNames.Name = "dgvSDNames";
            this.dgvSDNames.ReadOnly = true;
            this.dgvSDNames.RowHeadersVisible = false;
            this.dgvSDNames.Size = new System.Drawing.Size(164, 203);
            this.dgvSDNames.TabIndex = 10;
            this.dgvSDNames.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSDNames_CellClick);
            // 
            // StudyDir
            // 
            this.StudyDir.HeaderText = "STUDY DIRECTOR";
            this.StudyDir.Name = "StudyDir";
            this.StudyDir.ReadOnly = true;
            this.StudyDir.Width = 110;
            // 
            // StudyDirID
            // 
            this.StudyDirID.HeaderText = "ID";
            this.StudyDirID.Name = "StudyDirID";
            this.StudyDirID.ReadOnly = true;
            this.StudyDirID.Visible = false;
            this.StudyDirID.Width = 10;
            // 
            // SDImage
            // 
            this.SDImage.HeaderText = "Image";
            this.SDImage.Name = "SDImage";
            this.SDImage.ReadOnly = true;
            this.SDImage.Width = 50;
            // 
            // LoginName
            // 
            this.LoginName.HeaderText = "LOGIN NAME";
            this.LoginName.Name = "LoginName";
            this.LoginName.ReadOnly = true;
            this.LoginName.Visible = false;
            this.LoginName.Width = 5;
            // 
            // FinalRptESign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 647);
            this.Controls.Add(this.dgvSDNames);
            this.Controls.Add(this.lblForEMail);
            this.Controls.Add(this.lblSDTotal);
            this.Controls.Add(this.lblQATotal);
            this.Controls.Add(this.dgvForEMail);
            this.Controls.Add(this.dgvQA);
            this.Controls.Add(this.dgvSD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FinalRptESign";
            this.Text = "FINAL REPORT E-SIGNATURES";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.FinalRptESign_Activated);
            this.Load += new System.EventHandler(this.FinalRptESign_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvQA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvForEMail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSDNames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsQAESign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSDESign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsForEMail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvQA;
        private System.Windows.Forms.DataGridView dgvSD;
        private System.Windows.Forms.DataGridView dgvForEMail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bsQAESign;
        private System.Windows.Forms.Label lblQATotal;
        private System.Windows.Forms.Label lblSDTotal;
        private System.Windows.Forms.Label lblForEMail;
        private System.Windows.Forms.BindingSource bsSDESign;
        private System.Windows.Forms.BindingSource bsForEMail;
        private System.Windows.Forms.DataGridView dgvSDNames;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudyDir;
        private System.Windows.Forms.DataGridViewTextBoxColumn StudyDirID;
        private System.Windows.Forms.DataGridViewImageColumn SDImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoginName;
    }
}