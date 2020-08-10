namespace GIS
{
    partial class LabelESign
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
            this.lblApprovalTotal = new System.Windows.Forms.Label();
            this.dgvSterility = new System.Windows.Forms.DataGridView();
            this.lblApproval = new System.Windows.Forms.Label();
            this.bsApproval = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSterility)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsApproval)).BeginInit();
            this.SuspendLayout();
            // 
            // lblApprovalTotal
            // 
            this.lblApprovalTotal.BackColor = System.Drawing.Color.DarkGreen;
            this.lblApprovalTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblApprovalTotal.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApprovalTotal.ForeColor = System.Drawing.Color.White;
            this.lblApprovalTotal.Location = new System.Drawing.Point(508, 223);
            this.lblApprovalTotal.Name = "lblApprovalTotal";
            this.lblApprovalTotal.Size = new System.Drawing.Size(128, 20);
            this.lblApprovalTotal.TabIndex = 20;
            this.lblApprovalTotal.Text = "TOTAL :";
            this.lblApprovalTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvSterility
            // 
            this.dgvSterility.AllowUserToAddRows = false;
            this.dgvSterility.AllowUserToDeleteRows = false;
            this.dgvSterility.AllowUserToResizeColumns = false;
            this.dgvSterility.AllowUserToResizeRows = false;
            this.dgvSterility.BackgroundColor = System.Drawing.Color.White;
            this.dgvSterility.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSterility.Cursor = System.Windows.Forms.Cursors.Default;
            this.dgvSterility.Location = new System.Drawing.Point(12, 56);
            this.dgvSterility.Name = "dgvSterility";
            this.dgvSterility.ReadOnly = true;
            this.dgvSterility.Size = new System.Drawing.Size(624, 164);
            this.dgvSterility.TabIndex = 18;
            this.dgvSterility.DoubleClick += new System.EventHandler(this.dgvSterility_DoubleClick);
            this.dgvSterility.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSterility_KeyDown);
            this.dgvSterility.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgvSterlity_KeyPress);
            // 
            // lblApproval
            // 
            this.lblApproval.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApproval.ForeColor = System.Drawing.Color.Firebrick;
            this.lblApproval.Location = new System.Drawing.Point(9, 37);
            this.lblApproval.Name = "lblApproval";
            this.lblApproval.Size = new System.Drawing.Size(103, 21);
            this.lblApproval.TabIndex = 19;
            this.lblApproval.Text = "Approval List:";
            this.lblApproval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabelESign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 262);
            this.Controls.Add(this.lblApprovalTotal);
            this.Controls.Add(this.dgvSterility);
            this.Controls.Add(this.lblApproval);
            this.Name = "LabelESign";
            this.Tag = "LabelESign";
            this.Text = "Label E-Sign";
            this.Activated += new System.EventHandler(this.LabelESign_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSterility)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsApproval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblApprovalTotal;
        private System.Windows.Forms.DataGridView dgvSterility;
        private System.Windows.Forms.Label lblApproval;
        private System.Windows.Forms.BindingSource bsApproval;
    }
}