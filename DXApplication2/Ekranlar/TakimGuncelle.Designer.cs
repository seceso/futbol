namespace DXApplication2.Ekranlar
{
    partial class TakimGuncelle
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
            this.lblTakimId = new System.Windows.Forms.Label();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnIkonKaydet = new System.Windows.Forms.Button();
            this.txtTakim = new System.Windows.Forms.TextBox();
            this.pbLogoIkon = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnTakimSil = new System.Windows.Forms.Button();
            this.btnGuncelle = new System.Windows.Forms.Button();
            this.btnTakimAdiDegistir = new System.Windows.Forms.Button();
            this.txtWhoScoredId = new System.Windows.Forms.TextBox();
            this.WhoScoredId = new System.Windows.Forms.Label();
            this.pbLogoIkon.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTakimId
            // 
            this.lblTakimId.AutoSize = true;
            this.lblTakimId.BackColor = System.Drawing.Color.Transparent;
            this.lblTakimId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblTakimId.Location = new System.Drawing.Point(67, 23);
            this.lblTakimId.Name = "lblTakimId";
            this.lblTakimId.Size = new System.Drawing.Size(34, 13);
            this.lblTakimId.TabIndex = 12;
            this.lblTakimId.Text = "Takim";
            // 
            // btnKaydet
            // 
            this.btnKaydet.ForeColor = System.Drawing.Color.Purple;
            this.btnKaydet.Location = new System.Drawing.Point(38, 205);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(193, 28);
            this.btnKaydet.TabIndex = 5;
            this.btnKaydet.Text = "Logo Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.BtnKaydet_Click);
            // 
            // btnIkonKaydet
            // 
            this.btnIkonKaydet.ForeColor = System.Drawing.Color.Purple;
            this.btnIkonKaydet.Location = new System.Drawing.Point(36, 205);
            this.btnIkonKaydet.Name = "btnIkonKaydet";
            this.btnIkonKaydet.Size = new System.Drawing.Size(196, 28);
            this.btnIkonKaydet.TabIndex = 6;
            this.btnIkonKaydet.Text = "Ikon Kaydet";
            this.btnIkonKaydet.UseVisualStyleBackColor = true;
            this.btnIkonKaydet.Click += new System.EventHandler(this.BtnIkonKaydet_Click);
            // 
            // txtTakim
            // 
            this.txtTakim.Location = new System.Drawing.Point(144, 19);
            this.txtTakim.Name = "txtTakim";
            this.txtTakim.ReadOnly = true;
            this.txtTakim.Size = new System.Drawing.Size(212, 21);
            this.txtTakim.TabIndex = 14;
            // 
            // pbLogoIkon
            // 
            this.pbLogoIkon.BackColor = System.Drawing.Color.Transparent;
            this.pbLogoIkon.Controls.Add(this.groupBox2);
            this.pbLogoIkon.Controls.Add(this.groupBox1);
            this.pbLogoIkon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pbLogoIkon.Location = new System.Drawing.Point(32, 74);
            this.pbLogoIkon.Name = "pbLogoIkon";
            this.pbLogoIkon.Size = new System.Drawing.Size(589, 303);
            this.pbLogoIkon.TabIndex = 16;
            this.pbLogoIkon.TabStop = false;
            this.pbLogoIkon.Text = "Logo ve İkon Seçimi";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnIkonKaydet);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupBox2.Location = new System.Drawing.Point(302, 35);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(270, 239);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "İkon";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(118, 93);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnKaydet);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.groupBox1.Location = new System.Drawing.Point(17, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 239);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Logo";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Location = new System.Drawing.Point(44, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(180, 180);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // btnTakimSil
            // 
            this.btnTakimSil.ForeColor = System.Drawing.Color.Purple;
            this.btnTakimSil.Location = new System.Drawing.Point(462, 19);
            this.btnTakimSil.Name = "btnTakimSil";
            this.btnTakimSil.Size = new System.Drawing.Size(75, 49);
            this.btnTakimSil.TabIndex = 19;
            this.btnTakimSil.Text = "Sil";
            this.btnTakimSil.UseVisualStyleBackColor = true;
            this.btnTakimSil.Click += new System.EventHandler(this.btnTakimSil_Click);
            // 
            // btnGuncelle
            // 
            this.btnGuncelle.ForeColor = System.Drawing.Color.Purple;
            this.btnGuncelle.Location = new System.Drawing.Point(362, 19);
            this.btnGuncelle.Name = "btnGuncelle";
            this.btnGuncelle.Size = new System.Drawing.Size(75, 23);
            this.btnGuncelle.TabIndex = 18;
            this.btnGuncelle.Text = "Güncelle";
            this.btnGuncelle.UseVisualStyleBackColor = true;
            this.btnGuncelle.Click += new System.EventHandler(this.BtnGuncelle_Click);
            // 
            // btnTakimAdiDegistir
            // 
            this.btnTakimAdiDegistir.ForeColor = System.Drawing.Color.Purple;
            this.btnTakimAdiDegistir.Location = new System.Drawing.Point(362, 45);
            this.btnTakimAdiDegistir.Name = "btnTakimAdiDegistir";
            this.btnTakimAdiDegistir.Size = new System.Drawing.Size(75, 23);
            this.btnTakimAdiDegistir.TabIndex = 17;
            this.btnTakimAdiDegistir.Text = "Kaydet";
            this.btnTakimAdiDegistir.UseVisualStyleBackColor = true;
            this.btnTakimAdiDegistir.Click += new System.EventHandler(this.BtnTakimAdiDegistir_Click);
            // 
            // txtWhoScoredId
            // 
            this.txtWhoScoredId.Location = new System.Drawing.Point(144, 45);
            this.txtWhoScoredId.Name = "txtWhoScoredId";
            this.txtWhoScoredId.ReadOnly = true;
            this.txtWhoScoredId.Size = new System.Drawing.Size(212, 21);
            this.txtWhoScoredId.TabIndex = 15;
            // 
            // WhoScoredId
            // 
            this.WhoScoredId.AutoSize = true;
            this.WhoScoredId.BackColor = System.Drawing.Color.Transparent;
            this.WhoScoredId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.WhoScoredId.Location = new System.Drawing.Point(67, 49);
            this.WhoScoredId.Name = "WhoScoredId";
            this.WhoScoredId.Size = new System.Drawing.Size(72, 13);
            this.WhoScoredId.TabIndex = 13;
            this.WhoScoredId.Text = "WhoScoredId";
            // 
            // TakimGuncelle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 409);
            this.Controls.Add(this.lblTakimId);
            this.Controls.Add(this.txtTakim);
            this.Controls.Add(this.pbLogoIkon);
            this.Controls.Add(this.btnTakimSil);
            this.Controls.Add(this.btnGuncelle);
            this.Controls.Add(this.btnTakimAdiDegistir);
            this.Controls.Add(this.txtWhoScoredId);
            this.Controls.Add(this.WhoScoredId);
            this.Name = "TakimGuncelle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TakimGuncelle";
            this.pbLogoIkon.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblTakimId;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnIkonKaydet;
        private System.Windows.Forms.TextBox txtTakim;
        private System.Windows.Forms.GroupBox pbLogoIkon;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnTakimSil;
        private System.Windows.Forms.Button btnGuncelle;
        private System.Windows.Forms.Button btnTakimAdiDegistir;
        private System.Windows.Forms.TextBox txtWhoScoredId;
        public System.Windows.Forms.Label WhoScoredId;
    }
}