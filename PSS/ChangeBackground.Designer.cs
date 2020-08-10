namespace GIS
{
    partial class ChangeBackground
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
            this.pnlBackground = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cboBackgrounds = new System.Windows.Forms.ComboBox();
            this.btnChange = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBackground.Location = new System.Drawing.Point(18, 47);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(366, 221);
            this.pnlBackground.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Change to:";
            // 
            // cboBackgrounds
            // 
            this.cboBackgrounds.FormattingEnabled = true;
            this.cboBackgrounds.Items.AddRange(new object[] {
            "Background 1",
            "Background 2",
            "Background 3"});
            this.cboBackgrounds.Location = new System.Drawing.Point(76, 20);
            this.cboBackgrounds.Name = "cboBackgrounds";
            this.cboBackgrounds.Size = new System.Drawing.Size(169, 21);
            this.cboBackgrounds.TabIndex = 3;
            this.cboBackgrounds.SelectedIndexChanged += new System.EventHandler(this.cboBackgrounds_SelectedIndexChanged);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(299, 18);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(85, 23);
            this.btnChange.TabIndex = 4;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // ChangeBackground
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 303);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.cboBackgrounds);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnlBackground);
            this.Name = "ChangeBackground";
            this.Text = "Change Window Background";
            this.Load += new System.EventHandler(this.ChangeBackground_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlBackground;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboBackgrounds;
        private System.Windows.Forms.Button btnChange;
    }
}