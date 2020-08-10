namespace PSS
{
    partial class POESign
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
            this.lblSecondApprovalTotal = new System.Windows.Forms.Label();
            this.lblFirstApprovalTotal = new System.Windows.Forms.Label();
            this.dgvFirstApproval = new System.Windows.Forms.DataGridView();
            this.dgvSecondApproval = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.lblApproval1 = new System.Windows.Forms.Label();
            this.bsSecondApproval = new System.Windows.Forms.BindingSource(this.components);
            this.bsFirstApproval = new System.Windows.Forms.BindingSource(this.components);
            this.lblApproval2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFirstApproval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSecondApproval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSecondApproval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFirstApproval)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSecondApprovalTotal
            // 
            this.lblSecondApprovalTotal.BackColor = System.Drawing.Color.DarkGreen;
            this.lblSecondApprovalTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSecondApprovalTotal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSecondApprovalTotal.ForeColor = System.Drawing.Color.White;
            this.lblSecondApprovalTotal.Location = new System.Drawing.Point(903, 409);
            this.lblSecondApprovalTotal.Name = "lblSecondApprovalTotal";
            this.lblSecondApprovalTotal.Size = new System.Drawing.Size(110, 20);
            this.lblSecondApprovalTotal.TabIndex = 18;
            this.lblSecondApprovalTotal.Text = "TOTAL :";
            this.lblSecondApprovalTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFirstApprovalTotal
            // 
            this.lblFirstApprovalTotal.BackColor = System.Drawing.Color.DarkGreen;
            this.lblFirstApprovalTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFirstApprovalTotal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFirstApprovalTotal.ForeColor = System.Drawing.Color.White;
            this.lblFirstApprovalTotal.Location = new System.Drawing.Point(904, 200);
            this.lblFirstApprovalTotal.Name = "lblFirstApprovalTotal";
            this.lblFirstApprovalTotal.Size = new System.Drawing.Size(110, 20);
            this.lblFirstApprovalTotal.TabIndex = 17;
            this.lblFirstApprovalTotal.Text = "TOTAL :";
            this.lblFirstApprovalTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvFirstApproval
            // 
            this.dgvFirstApproval.AllowUserToAddRows = false;
            this.dgvFirstApproval.AllowUserToDeleteRows = false;
            this.dgvFirstApproval.AllowUserToResizeColumns = false;
            this.dgvFirstApproval.AllowUserToResizeRows = false;
            this.dgvFirstApproval.BackgroundColor = System.Drawing.Color.White;
            this.dgvFirstApproval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFirstApproval.Cursor = System.Windows.Forms.Cursors.Default;
            this.dgvFirstApproval.Location = new System.Drawing.Point(12, 33);
            this.dgvFirstApproval.Name = "dgvFirstApproval";
            this.dgvFirstApproval.ReadOnly = true;
            this.dgvFirstApproval.Size = new System.Drawing.Size(1002, 164);
            this.dgvFirstApproval.TabIndex = 11;
            this.dgvFirstApproval.DoubleClick += new System.EventHandler(this.dgvFirstApproval_DoubleClick);
            this.dgvFirstApproval.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvQA_KeyDown);
            this.dgvFirstApproval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvQA_KeyPress);
            // 
            // dgvSecondApproval
            // 
            this.dgvSecondApproval.AllowUserToAddRows = false;
            this.dgvSecondApproval.AllowUserToDeleteRows = false;
            this.dgvSecondApproval.AllowUserToResizeColumns = false;
            this.dgvSecondApproval.AllowUserToResizeRows = false;
            this.dgvSecondApproval.BackgroundColor = System.Drawing.Color.White;
            this.dgvSecondApproval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSecondApproval.Location = new System.Drawing.Point(12, 242);
            this.dgvSecondApproval.Name = "dgvSecondApproval";
            this.dgvSecondApproval.ReadOnly = true;
            this.dgvSecondApproval.Size = new System.Drawing.Size(1002, 164);
            this.dgvSecondApproval.TabIndex = 12;
            this.dgvSecondApproval.DoubleClick += new System.EventHandler(this.dgvSecondApproval_DoubleClick);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Firebrick;
            this.label2.Location = new System.Drawing.Point(43, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 21);
            this.label2.TabIndex = 15;
            this.label2.Text = "Study Directors";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblApproval1
            // 
            this.lblApproval1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApproval1.ForeColor = System.Drawing.Color.Firebrick;
            this.lblApproval1.Location = new System.Drawing.Point(9, 14);
            this.lblApproval1.Name = "lblApproval1";
            this.lblApproval1.Size = new System.Drawing.Size(386, 21);
            this.lblApproval1.TabIndex = 14;
            this.lblApproval1.Text = "Approval Level 1";
            this.lblApproval1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblApproval2
            // 
            this.lblApproval2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApproval2.ForeColor = System.Drawing.Color.Firebrick;
            this.lblApproval2.Location = new System.Drawing.Point(12, 218);
            this.lblApproval2.Name = "lblApproval2";
            this.lblApproval2.Size = new System.Drawing.Size(455, 21);
            this.lblApproval2.TabIndex = 21;
            this.lblApproval2.Text = "Approval Level 2:  Orders $2,500 and up";
            this.lblApproval2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // POESign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 444);
            this.Controls.Add(this.lblApproval2);
            this.Controls.Add(this.lblSecondApprovalTotal);
            this.Controls.Add(this.lblFirstApprovalTotal);
            this.Controls.Add(this.dgvFirstApproval);
            this.Controls.Add(this.dgvSecondApproval);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblApproval1);
            this.MaximizeBox = false;
            this.Name = "POESign";
            this.Tag = "POESign";
            this.Text = "PO E-Signatures";
            this.Activated += new System.EventHandler(this.POESign_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFirstApproval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSecondApproval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSecondApproval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFirstApproval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsSecondApproval;
        private System.Windows.Forms.Label lblSecondApprovalTotal;
        private System.Windows.Forms.Label lblFirstApprovalTotal;
        private System.Windows.Forms.DataGridView dgvFirstApproval;
        private System.Windows.Forms.DataGridView dgvSecondApproval;
        private System.Windows.Forms.BindingSource bsFirstApproval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblApproval1;
        private System.Windows.Forms.Label lblApproval2;
    }
}