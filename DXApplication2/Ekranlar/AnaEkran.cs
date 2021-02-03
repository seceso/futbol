using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;

namespace DXApplication2.Ekranlar
{
    public partial class AnaEkran : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public AnaEkran()
        {
            InitializeComponent();
            //ribbonControl1.Minimized = true;
            
        }
        public bool AcilmisMi(string formName)
        {
            Form[] formlar = this.MdiChildren;
            for (int i = 0; i < formlar.Length; i++)
                if (formlar[i].Name == formName)
                    return true;
            return false;
        }
        public void BasaGetir(string formName)
        {
            Form[] formlar = this.MdiChildren;
            for (int i = 0; i < formlar.Length; i++)
                if (formlar[i].Name == formName)
                    formlar[i].BringToFront();
        }
        private void AnaEkran_Load(object sender, System.EventArgs e)
        {
            //GirisSayfasi gs = new GirisSayfasi() { MdiParent = this };
            //gs.Show();

            MacBulteni mb = new MacBulteni() { MdiParent = this };
            mb.Show();

            ribbonControl1.Minimized = true;

        }
        private void barButtonItemCanliSonuclar_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("CanliSonuc"))
            {
                BasaGetir("CanliSonuc");
                return;
            }
            CanliSonuc gs = new CanliSonuc() { MdiParent = this };
            gs.Show();
        }
        private void barButtonItemMacBulteni_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("MacBulteni"))
            {
                BasaGetir("MacBulteni");
                return;
            }
            MacBulteni mb = new MacBulteni() { MdiParent = this };
            mb.Show();
        }
        private void barButtonItemVeriCek_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("Whoscored"))
            {
                BasaGetir("Whoscored");
                return;
            }
            Whoscored mb = new Whoscored() { MdiParent = this };mb.Show();
        }
        private void barButtonItemTakimlar_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("Takimlar"))
            {
                BasaGetir("Takimlar");
                return;
            }
            Takimlar t = new Takimlar() { MdiParent = this };
            t.Show();
        }
        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Çıkmak İstediğinizden Emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (sonuc == DialogResult.Yes)
            {

                Application.Exit();
            }
            else
            {
                MessageBox.Show("Devamm");
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {


            //if (AcilmisMi("GrafikDeneme"))
            //{
            //    BasaGetir("GrafikDeneme");
            //    return;
            //}
            //GrafikDeneme t = new GrafikDeneme() { MdiParent = this };
            //t.Show();

        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("FormDeneme"))
            {
                BasaGetir("FormDeneme");
                return;
            }
            FormDeneme t = new FormDeneme() { MdiParent = this };
            t.Show();
        }

        private void barButtonItemProfilim_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("Profil"))
            {
                BasaGetir("Profil");
                return;
            }
            Profil t = new Profil() { MdiParent = this };
            t.Show();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("FormDeneme"))
            {
                BasaGetir("FormDeneme");
                return;
            }
            FormDeneme t = new FormDeneme() { MdiParent = this };
            t.Show();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("Form1"))
            {
                BasaGetir("Form1");
                return;
            }
            deneme.Form1 t = new deneme.Form1() { MdiParent = this };
            t.Show();
        }

        private void barButtonItemMacSonuc_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("Sonuclar"))
            {
                BasaGetir("Sonuclar");
                return;
            }
            Sonuclar t = new Sonuclar() { MdiParent = this };
            t.Show();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (AcilmisMi("Bets10"))
            //{
            //    BasaGetir("Bets10");
            //    return;
            //}
            //Bets10 t = new Bets10() { MdiParent = this }; ;
            //t.Show();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void barButtonItemLigIst_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("LigFikstur"))
            {
                BasaGetir("LigFikstur");
                return;
            }
            LigFikstur t = new LigFikstur() { MdiParent = this }; ;
            t.Show();
        }

        private void barButtonItemLigler_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (AcilmisMi("PowerBI"))
            {
                BasaGetir("PowerBI");
                return;
            }
            PowerBI t = new PowerBI() { MdiParent = this }; ;
            t.Show();
        }
    }
    }
