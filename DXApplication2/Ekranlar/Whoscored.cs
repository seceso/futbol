using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DXApplication2.Web;
using TahminManager;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Http;


namespace DXApplication2.Ekranlar
{
    public partial class Whoscored : DevExpress.XtraEditors.XtraForm
    {
        public Whoscored()
        {
            InitializeComponent();
        }

        //public SqlConnection baglantiLocalhost =
        //    new SqlConnection("server=psl.dynu.com;database=futbol;user=selsor;password=123456;pooling=false");

        public SqlConnection baglantiLocalhost =
            new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");

        public string spiderLigAdi;


        public void Writer(KeyValuePair<string, string> pair, int total, int current, string url)
        {
            if (lstLigler != null)
            {
                lstLigler.Items.Add(pair.Key + " - " + pair.Value + "[" + current + "/" + total + "]");
            }

        }

        public void MaclarWriter(int total, int current, string filename)
        {
            if (lstMaclar != null)
            {
                lstMaclar.Items.Add("[ İndirilen Dosya : " + filename + "]" + "[" + current + "/" + total + "] ");
            }
        }

        public void MacWriter(string filename)
        {
            if (lstMaclar != null)
            {
                lstMaclar.Items.Add("[ xxxx : " + filename + "] -->" + DateTime.Now.ToString());
            }
        }

        public void VeriTabaniWriter(string filename)
        {
            if (lstMacBilgileri != null)
            {
                lstMacBilgileri.Items.Add("[ Veritabanına yazılan; " + filename + "]-->" + DateTime.Now.ToString());
            }
        }

        public void FiksturWriter(int total, int current, string filename)
        {
            if (lstFikstur != null)
            {
                lstFikstur.Items.Add("[ İndirilen Dosya : " + filename + "]" + "[" + current + "/" + total + "] -->" +
                                     DateTime.Now.ToString());
            }
        }

     

        private async void btnFikstur_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            var config = new Configuration();
            config.GetConfiguration();
            var spider = new SpiderAsync();
            await Task.Factory.StartNew(() =>
            {
                spider.GetAllFixtures(FiksturWriter);
                lstFikstur.SelectedIndex = lstFikstur.Items.Count - 1;
            });
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var config = new Configuration();
                config.GetConfiguration();

                var directory = new DirectoryInfo(Globe.RootDir + "ligler\\");
                var leagueDir = directory.GetDirectories();

                foreach (DirectoryInfo league in leagueDir)
                {
                    var updateDB = new UpdateDataBase(league.Name, league.FullName);
                    await Task.Factory.StartNew(() =>
                    {
                        updateDB.LoadFolder();
                        updateDB.LoadFolder2();
                    });
                }
            }
            catch (Exception ex)
            {
                Globe.WriteLog("Veritabanına yazamadım : " + ex.Message);
            }
        }

        public string DosyayiokuStringDondur(string filePath)
        {
            using (var streamreader = new StreamReader(filePath))
            {
                var text = streamreader.ReadToEnd();
                streamreader.Close();
                return text;
            }
        }

        public string lig, ligValue;

        private void button2_Click(object sender, EventArgs e)
        {
            //var config = new Configuration();
            //config.GetTurkConfiguration();


            //try
            //{
            //    var spider = new SpiderAsync();
            //    spider.GetAllLeagues(Writer);
            //}
            //catch (Exception ex)
            //{
            //    Globe.WriteLog("Lig klasörlerini oluştururken bir sorun oluştu : " + ex.Message);
            //}

            //try
            //{
            //    var spider = new SpiderAsync();
            //    spider.GetAllMatches();
            //}
            //catch (Exception ex)
            //{
            //    Globe.WriteLog("Maç dosyalarını oluştururken bir sorun oluştu : " + ex.Message);
            //}
        }

        private async void btnTaskDeneme_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            var config = new Configuration();
            config.GetConfiguration();
            var spider = new SpiderAsync();
            await Task.Factory.StartNew(() => { spider.GetAllLeagues(); });
            Thread.Sleep(300000);
            await Task.Factory.StartNew(() => { spider.GetAllMatches(); });
            Thread.Sleep(3600000);

            var directory = new DirectoryInfo(Globe.RootDir + "ligler\\");
            var leagueDir = directory.GetDirectories();

            foreach (DirectoryInfo league in leagueDir)
            {
                await Task.Factory.StartNew(() =>
                {
                    var updateDB = new UpdateDataBase(league.Name, league.FullName);
                    updateDB.LoadFolder();
                    updateDB.LoadFolder2();
                });
            }
            await Task.WhenAll();
            MessageBox.Show("İşlemler tamamlandı!!!");
        }

        private void btnFiksturGuncelle_Click(object sender, EventArgs e)
        {
            var config = new Configuration();
            config.GetConfiguration();
            var spider = new SpiderAsync();
            Task.Factory.StartNew(() => { spider.GetAllFixtures2(FiksturWriter); });
        }

        private void btnTurkiyeLigi_Click(object sender, EventArgs e)
        {
            var config = new Configuration();
            config.GetTurkConfiguration();

            try
            {
                var spider = new SpiderAsync();
                spider.GetAllLeagues();
            }
            catch (Exception ex)
            {
                Globe.WriteLog("Lig klasörlerini oluştururken bir sorun oluştu : " + ex.Message);
            }

            try
            {
                var spider = new SpiderAsync();
                spider.GetAllMatches(MaclarWriter, MacWriter);
            }
            catch (Exception ex)
            {
                Globe.WriteLog("Maç dosyalarını oluştururken bir sorun oluştu : " + ex.Message);
            }
        }

        private void btnLigler_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            var config = new Configuration();
            config.GetConfiguration();
            lstLigler.Items.Clear();

            try
            {
                var spider = new SpiderAsync();
                Task.Factory.StartNew(() =>
                {
                    spider.GetAllLeagues();
                    lstLigler.SelectedIndex = lstLigler.Items.Count - 1;

                    MessageBox.Show(@"İşlem Bitti");
                });
            }
            catch (Exception ex)
            {
                Globe.WriteLog("Lig klasörlerini oluştururken bir sorun oluştu : " + ex.Message);
            }
        }

        private void btnMaclar_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            var config = new Configuration();
            config.GetConfiguration();
            lstMaclar.Items.Clear();
            try
            {
                var spider = new SpiderAsync();
                Task.Factory.StartNew(() =>
                {
                    spider.GetAllMatches(MaclarWriter, MacWriter);
                    MessageBox.Show(@"İşlem Bitti");
                });
            }
            catch (Exception ex)
            {
                Globe.WriteLog("Lig klasörlerini oluştururken bir sorun oluştu : " + ex.Message);
            }
        }

       

        private async void btnMacBilgileri_Click(object sender, EventArgs e)
        {
            try
            {
                CheckForIllegalCrossThreadCalls = false;
                var config = new Configuration();
                config.GetConfiguration();
                lstMacBilgileri.Items.Clear();

                var directory = new DirectoryInfo(Globe.RootDir + "ligler\\");
                var leagueDir = directory.GetDirectories();

                foreach (DirectoryInfo league in leagueDir)
                {
                    await Task.Factory.StartNew(() =>
                    {
                        var updateDB = new UpdateDataBase(league.Name, league.FullName);
                        updateDB.LoadFolder(VeriTabaniWriter);
                        lstMacBilgileri.SelectedIndex = lstMacBilgileri.Items.Count - 1;
                    });
                }
            }
            catch (Exception ex)
            {
                Globe.WriteLog("Veritabanına yazamadım : " + ex.Message);
            }
        }

        
    }
}