using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DXApplication2.Ekranlar
{
    public partial class TakimGuncelle : DevExpress.XtraEditors.XtraForm
    {
        public TakimGuncelle()
        {
            InitializeComponent();
        }
        public int Id;
        //public SqlConnection baglantiLocalhost = new SqlConnection("server=psl.dynu.com;database=futbol;user=selsor;password=123456;pooling=false");
        //public SqlConnection baglantiLocalhost = new SqlConnection("server=localhost;database=futbol;user=sa;password=123456");//trusted_connection=true
        public SqlConnection baglantiLocalhost =
            new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");

        [Obsolete]
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog fdialog = new OpenFileDialog();
                fdialog.Filter = "Pictures|*.png";
                fdialog.InitialDirectory = "C://";
                byte[] byteResim = null;

                if (DialogResult.OK == fdialog.ShowDialog())
                {
                    string resimYol = fdialog.FileName;
                    pictureBox2.Image = Image.FromFile(resimYol);
                    FileInfo fInfo = new FileInfo(resimYol);
                    long sayac = fInfo.Length;
                    FileStream fStream = new FileStream(resimYol, FileMode.Open, FileAccess.Read);
                    BinaryReader bReader = new BinaryReader(fStream);
                    byteResim = bReader.ReadBytes((int)sayac);
                }
                else
                {
                    return;
                }

                if (baglantiLocalhost.State == ConnectionState.Closed) // Bağlantı kapalı mı kontrol ediyoruz.
                    baglantiLocalhost.Open(); // Bağlantıyı açıyoruz.

                SqlCommand komut = new SqlCommand("ResimKaydet", baglantiLocalhost); // ResimKaydet adında bir store procedure yazıyoruz.

                komut.CommandType = CommandType.StoredProcedure; //Komutumuzun bir store procedure olduğunu gösteriyoruz.
                komut.Parameters.Add("@Resim", byteResim);
                komut.Parameters.Add("@TakimId", Id);
                komut.ExecuteNonQuery();


                //Eğer kayıt gerçekleşirse etkilenen sayısı bir olucaktır. Yani kayıt gerçekleşmiş olmuş olucaktır.

                baglantiLocalhost.Close();


                MessageBox.Show(string.Empty + Id + " numaralı takımın logo kaydı başarıyla gerçekleşti.");

            }


            catch (Exception hata)
            {

                MessageBox.Show(hata.Message);
            }
        }
        public void TakimSil()
        {
            baglantiLocalhost.Open();
            SqlCommand cmdTakimSil = new SqlCommand("update tanimlar.takimlar set durum=0 where Id='" + Id + "'", baglantiLocalhost);
            int etkilenen = cmdTakimSil.ExecuteNonQuery();
            if (etkilenen > 0)
                MessageBox.Show(string.Empty + Id + "numaralı kayıt pasife çekildi");
            else
                MessageBox.Show("Kayıt başarısız oldu!!");
        }


        public void LogoGoster(int Id)
        {
            baglantiLocalhost.Open();
            SqlCommand cmdLogoGoster = new SqlCommand("select logo from tanimlar.takimlar where Id='" + Id + "'", baglantiLocalhost);
            Image LogoGoster = null;
            SqlDataReader okuyucu = cmdLogoGoster.ExecuteReader();

            while (okuyucu.Read())
            {
                if (okuyucu.IsDBNull(0))
                    break;
                byte[] logo = (byte[])okuyucu[0];
                MemoryStream ms = new MemoryStream(logo, 0, logo.Length);
                ms.Write(logo, 0, logo.Length);
                LogoGoster = Image.FromStream(ms, true);
                pictureBox2.Image = LogoGoster;
            }
            baglantiLocalhost.Close();
        }
        public void IkonGoster(int Id)
        {
            baglantiLocalhost.Open();
            SqlCommand cmdIkonGoster = new SqlCommand("select Ikon from tanimlar.takimlar where Id='" + Id + "'", baglantiLocalhost);
            Image IkonGoster = null;
            SqlDataReader okuyucu = cmdIkonGoster.ExecuteReader();
            while (okuyucu.Read())
            {
                if (okuyucu.IsDBNull(0))
                    break;
                byte[] Ikon = (byte[])okuyucu[0];
                MemoryStream ms = new MemoryStream(Ikon, 0, Ikon.Length);
                ms.Write(Ikon, 0, Ikon.Length);
                IkonGoster = Image.FromStream(ms, true);
                pictureBox1.Image = IkonGoster;
            }
            baglantiLocalhost.Close();
        }

        [Obsolete]
        private void BtnIkonKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fdialog = new OpenFileDialog();
                fdialog.Filter = "Pictures|*.ico";
                fdialog.InitialDirectory = "C://";
                byte[] byteResim = null;

                if (DialogResult.OK == fdialog.ShowDialog())
                {
                    string resimYol = fdialog.FileName;
                    pictureBox1.Image = Image.FromFile(resimYol);
                    FileInfo fInfo = new FileInfo(resimYol);
                    long sayac = fInfo.Length;
                    FileStream fStream = new FileStream(resimYol, FileMode.Open, FileAccess.Read);
                    BinaryReader bReader = new BinaryReader(fStream);
                    byteResim = bReader.ReadBytes((int)sayac);

                }
                else
                {
                    return;
                }
                if (baglantiLocalhost.State == ConnectionState.Closed) // Bağlantı kapalı mı kontrol ediyoruz.

                    baglantiLocalhost.Open(); // Bağlantıyı açıyoruz.


                using (SqlCommand komut = new SqlCommand("IkonKaydet", baglantiLocalhost) // ResimKaydet adında bir store procedure yazıyoruz.
                )
                {
                    komut.CommandType = CommandType.StoredProcedure; //Komutumuzun bir store procedure olduğunu gösteriyoruz.
                    komut.Parameters.Add("@Resim", byteResim);
                    komut.Parameters.Add("@TakimId", Id);
                    komut.ExecuteNonQuery();//Eğer kayıt gerçekleşirse etkilenen sayısı bir olucaktır. Yani kayıt gerçekleşmiş olmuş olucaktır.
                }

                baglantiLocalhost.Close();

                MessageBox.Show(string.Empty + Id + " numaralı takımın ikon kaydı başarıyla gerçekleşti.");

            }


            catch (Exception hata)
            {

                MessageBox.Show(hata.Message);
            }
        }
        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            txtTakim.ReadOnly = false;
            txtWhoScoredId.ReadOnly = false;
        }
        private void BtnTakimAdiDegistir_Click(object sender, EventArgs e)
        {
            baglantiLocalhost.Open();
            SqlCommand cmdTakimGuncelle = new SqlCommand("update tanimlar.takimlar set ad='" + txtTakim.Text + "',whoScoredId=cast('" + txtWhoScoredId.Text + "' as int)  where Id='" + Id + "'", baglantiLocalhost);

            int etkilenen = cmdTakimGuncelle.ExecuteNonQuery();
            if (etkilenen > 0)
                MessageBox.Show(string.Empty + Id + " numaralı kayıt '" + txtTakim.Text + "' olarak güncellenmiştir");
            else
                MessageBox.Show("Kayıt başarısız oldu!!");
            TahminManager.web.Memory.FlushMemory();
            this.Close();
        }
        private void btnTakimSil_Click(object sender, EventArgs e)
        {
            TakimSil();
        }



    }
}