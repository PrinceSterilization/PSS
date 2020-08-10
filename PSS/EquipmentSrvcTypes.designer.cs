namespace PSS
{
    partial class EquipmentSrvcTypes
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
            this.pnlRecord = new System.Windows.Forms.Panel();
            this.txtServiceName = new GISControls.TextBoxChar();
            this.ServiceName = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtServiceType = new GISControls.TextBoxChar();
            this.ServiceType = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bsEquipmentSrvcType = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsEquipmentSrvcType)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.txtServiceName);
            this.pnlRecord.Controls.Add(this.ServiceName);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtServiceType);
            this.pnlRecord.Controls.Add(this.ServiceType);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Location = new System.Drawing.Point(12, 88);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(500, 109);
            this.pnlRecord.TabIndex = 105;
            this.pnlRecord.Visible = false;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // txtServiceName
            // 
            this.txtServiceName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServiceName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServiceName.Location = new System.Drawing.Point(120, 65);
            this.txtServiceName.MaxLength = 50;
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.Size = new System.Drawing.Size(350, 21);
            this.txtServiceName.TabIndex = 6;
            // 
            // ServiceName
            // 
            this.ServiceName.BackColor = System.Drawing.Color.Transparent;
            this.ServiceName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServiceName.ForeColor = System.Drawing.Color.Black;
            this.ServiceName.Location = new System.Drawing.Point(20, 65);
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.Padding = new System.Windows.Forms.Padding(2);
            this.ServiceName.Size = new System.Drawing.Size(95, 21);
            this.ServiceName.TabIndex = 7;
            this.ServiceName.Text = "Service Name:";
            this.ServiceName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(421, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtServiceType
            // 
            this.txtServiceType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServiceType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServiceType.Location = new System.Drawing.Point(120, 40);
            this.txtServiceType.MaxLength = 50;
            this.txtServiceType.Name = "txtServiceType";
            this.txtServiceType.Size = new System.Drawing.Size(82, 21);
            this.txtServiceType.TabIndex = 1;
            // 
            // ServiceType
            // 
            this.ServiceType.BackColor = System.Drawing.Color.Transparent;
            this.ServiceType.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServiceType.ForeColor = System.Drawing.Color.Black;
            this.ServiceType.Location = new System.Drawing.Point(20, 40);
            this.ServiceType.Name = "ServiceType";
            this.ServiceType.Padding = new System.Windows.Forms.Padding(2);
            this.ServiceType.Size = new System.Drawing.Size(90, 21);
            this.ServiceType.TabIndex = 5;
            this.ServiceType.Text = "Service Code:";
            this.ServiceType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(502, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Equipment Service Types";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EquipmentSrvcTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(1916, 704);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "EquipmentSrvcTypes";
            this.Load += new System.EventHandler(this.EquipmentSrvcType_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EquipmentSrvcType_KeyDown);
            this.Controls.SetChildIndex(this.lblLoadStatus, 0);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsEquipmentSrvcType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Button btnClose;
        private GISControls.TextBoxChar txtServiceType;
        private System.Windows.Forms.Label ServiceType;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.BindingSource bsEquipmentSrvcType;
        private GISControls.TextBoxChar txtServiceName;
        private System.Windows.Forms.Label ServiceName;
    }
}
