namespace DXApplication2.Ekranlar
{
    partial class LigFikstur
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
            this.genelEkran1 = new DXApplication2.GenelEkran();
            this.SuspendLayout();
            // 
            // genelEkran1
            // 
            this.genelEkran1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genelEkran1.Location = new System.Drawing.Point(0, 0);
            this.genelEkran1.Name = "genelEkran1";
            this.genelEkran1.Size = new System.Drawing.Size(876, 527);
            this.genelEkran1.TabIndex = 0;
            this.genelEkran1.Load += new System.EventHandler(this.genelEkran1_Load);
            // 
            // LigFikstur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 527);
            this.Controls.Add(this.genelEkran1);
            this.Name = "LigFikstur";
            this.Text = "LigFikstur";
            this.ResumeLayout(false);

        }

        #endregion

        private GenelEkran genelEkran1;
    }
}