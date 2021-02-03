using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXApplication2.Ekranlar
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

        }

        public static int kullaniciId;
        public static int loginId;
        public static string IP;
        public static string kullanici;




        public string GetIP()
        {
            var strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            var addr = ipEntry.AddressList;
            return addr[1].ToString();
        }

        readonly DXApplication2.Ekranlar.AnaEkran a = new DXApplication2.Ekranlar.AnaEkran();

        //SqlConnection baglanti = new SqlConnection("server=psl.dynu.com;database=futbol;user=selsor;password=123456");
        readonly SqlConnection baglanti =
            new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");
        public static String sifre, kullaniciAdi, windowsKullaniciAdi, bilgisayarAdi, domainAdi, IpAdresi, girisTarihi, LoginId;
        public static int KullaniciId, personelId, deneme;
        public static String x;
        private void BtnGiris_Click(object sender, EventArgs e)
        {
            if (txtKullaniciAdi.Text == string.Empty)
            {
                MessageBox.Show(@"Lütfen Kullanıcı Adınızı Giriniz.", "Tahmin Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtKullaniciAdi.Focus();
                return;
            }

            if (txtSifre.Text == string.Empty)
            {
                MessageBox.Show("Lütfen Şifrenizi Kontrol Ediniz.", "Tahmin Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtSifre.Focus();
                return;
            }

            baglanti.Open();
            SqlCommand cmd = new SqlCommand(@"SELECT kullaniciAdi,sifre,Id from Kullanicilar where kullaniciAdi = '" + txtKullaniciAdi.Text + "' and sifre = '" + txtSifre.Text + "'", baglanti);
            SqlDataReader data = cmd.ExecuteReader();

            while (data.Read())
            {
                kullaniciAdi = data[0].ToString();
                sifre = data[1].ToString();
                personelId = Convert.ToInt32(data[2].ToString());
            }

            baglanti.Close();



            if (sifre == txtSifre.Text)
            {
                baglanti.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT Id from kullanicilar where kullaniciAdi = '" + txtKullaniciAdi.Text + "' and sifre = '" + txtSifre.Text + "'", baglanti);
                SqlDataReader data2 = cmd2.ExecuteReader();

                while (data2.Read())
                {
                    KullaniciId = Convert.ToInt32(data2[0].ToString());
                }
                baglanti.Close();
                windowsKullaniciAdi = System.Windows.Forms.SystemInformation.UserName.ToString();
                bilgisayarAdi = System.Environment.MachineName.ToString();
                IpAdresi = GetIP();
                girisTarihi = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss");

                baglanti.Open();
                SqlCommand loginKaydiEkle = new SqlCommand(String.Format("INSERT INTO login (KullaniciId,BilgisayarAdi,WindowsKullaniciAdi,GirisTarihi,Ip) VALUES ('{0}','{1}','{2}','{3}','{4}')", KullaniciId, bilgisayarAdi, windowsKullaniciAdi, girisTarihi, IpAdresi), baglanti);
                loginKaydiEkle.ExecuteNonQuery();
                baglanti.Close();

                baglanti.Open();
                SqlCommand cmd3 = new SqlCommand("SELECT top 1 Id from Login order by Id desc ", baglanti);
                SqlDataReader data3 = cmd3.ExecuteReader();
                while (data3.Read())
                {
                    LoginId = data3[0].ToString();
                }

                baglanti.Close();

                this.Hide();


            }
            else
            {
                MessageBox.Show(@"Geçersiz Şifre", "Tahmin Manager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSifre.Focus();
                txtSifre.SelectAll();
                return;

            }
            x = LoginId.ToString();
            kullaniciAdi = txtKullaniciAdi.Text;

            kullaniciId = KullaniciId;
            loginId = Convert.ToInt32(LoginId);
            IP = IpAdresi;
            kullanici = kullaniciAdi;


            baglanti.Close();
            a.Show();
        }
        private void txtSifre_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnGiris.PerformClick();
            }
        }



    }
}
