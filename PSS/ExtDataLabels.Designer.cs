﻿namespace GIS
{
    partial class ExtDataLabels
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
            this.txtDesc = new GISControls.TextBoxChar();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bsExtDataLabels = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).BeginInit();
            this.pnlRecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsExtDataLabels)).BeginInit();
            this.SuspendLayout();
            // 
            // cklColumns
            // 
            this.cklColumns.Size = new System.Drawing.Size(122, 196);
            // 
            // pnlRecord
            // 
            this.pnlRecord.BackColor = System.Drawing.Color.AliceBlue;
            this.pnlRecord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecord.Controls.Add(this.txtDesc);
            this.pnlRecord.Controls.Add(this.btnClose);
            this.pnlRecord.Controls.Add(this.txtID);
            this.pnlRecord.Controls.Add(this.label3);
            this.pnlRecord.Controls.Add(this.lblHeader);
            this.pnlRecord.Location = new System.Drawing.Point(12, 87);
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.Size = new System.Drawing.Size(498, 82);
            this.pnlRecord.TabIndex = 106;
            this.pnlRecord.Visible = false;
            this.pnlRecord.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseDown);
            this.pnlRecord.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseMove);
            this.pnlRecord.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlRecord_MouseUp);
            // 
            // txtDesc
            // 
            this.txtDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDesc.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDesc.Location = new System.Drawing.Point(96, 40);
            this.txtDesc.MaxLength = 50;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(382, 21);
            this.txtDesc.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Firebrick;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(420, -1);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 22);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "C&lose [X]";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtID
            // 
            this.txtID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtID.Location = new System.Drawing.Point(421, 40);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(57, 21);
            this.txtID.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(13, 41);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(2);
            this.label3.Size = new System.Drawing.Size(81, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Description";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(-3, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(500, 21);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "EXTENDED DATA LABEL";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseDown);
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ExtDataLabels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.ClientSize = new System.Drawing.Size(900, 657);
            this.Controls.Add(this.pnlRecord);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "ExtDataLabels";
            this.Tag = "ExtDataLabels";
            this.Text = "EXTENDED DATA LABELS";
            this.Load += new System.EventHandler(this.ExtDataLabels_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExtDataLabels_KeyDown);
            this.Controls.SetChildIndex(this.chkFullText, 0);
            this.Controls.SetChildIndex(this.chkShowInactive, 0);
            this.Controls.SetChildIndex(this.cklColumns, 0);
            this.Controls.SetChildIndex(this.pnlRecord, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bsFile)).EndInit();
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsExtDataLabels)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRecord;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtID;
        private GISControls.TextBoxChar txtDesc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.BindingSource bsExtDataLabels;
    }
}
