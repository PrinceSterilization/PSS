namespace PSS
{
    partial class POView
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
            this.picPO = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picPO)).BeginInit();
            this.SuspendLayout();
            // 
            // picPO
            // 
            this.picPO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPO.Location = new System.Drawing.Point(0, 0);
            this.picPO.Name = "picPO";
            this.picPO.Size = new System.Drawing.Size(594, 421);
            this.picPO.TabIndex = 0;
            this.picPO.TabStop = false;
            // 
            // POView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 421);
            this.Controls.Add(this.picPO);
            this.Name = "POView";
            this.Text = "POView";
            this.Load += new System.EventHandler(this.POView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picPO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picPO;
    }
}