using DevExpress.Utils;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DXApplication2.Ekranlar
{
    public partial class MacBulteni : Form
    {
        private readonly int kullaniciID = Login.kullaniciId;

        public SqlConnection baglantiLocalhost =
            new SqlConnection(
                "server=194.31.59.132;database=FootballStatistics;user=sa;password=Buse2015;pooling=false");
        //"server=localhost\\MSSQLSERVER01;database=FootballStatistics;user=sa;password=123456;pooling=false");

        public ChartTitle chartTitle1 = new ChartTitle();
        public ChartTitle chartTitle2 = new ChartTitle();
        public ChartTitle chartTitle3 = new ChartTitle();
        public ChartTitle chartTitle4 = new ChartTitle();

        public string EvsahibiTakim, MisafirTakim;
        public int EvsahibiTakimId, MisafirTakimId;
        private string IP = Login.IP;

        private Dictionary<string, Bitmap> iconsCache = new Dictionary<string, Bitmap>();
        private string kullanici = Login.kullanici;

        public int KuponId, TahminId;

        public int ligid;

        private int loginId = Login.loginId;
        public int MacId;
        private double misli, kazanc;
        public List<int> originalMatchIDs;
        public int satirsayisi;
        public Series series1 = new Series("Genel - Attığı Gol Ortalaması", ViewType.Bar);
        public Series series10 = new Series("İkinci Yarı - Yediği Gol Ortalaması", ViewType.Bar);
        public Series series11 = new Series("İç Saha - Attığı Gol Ortalaması", ViewType.Area);
        public Series series12 = new Series("İç Saha - Yediği Gol Ortalaması", ViewType.Area);
        public Series series13 = new Series("İç Saha - Attığı Gol Ortalaması", ViewType.Area);
        public Series series14 = new Series("İç Saha - Yediği Gol Ortalaması", ViewType.Area);
        public Series series15 = new Series("İlk Yarı - Attığı Gol Ortalaması", ViewType.Area);
        public Series series16 = new Series("İlk Yarı - Yediği Gol Ortalaması", ViewType.Area);
        public Series series17 = new Series("İkinci Yarı - Attığı Gol Ortalaması", ViewType.Area);
        public Series series18 = new Series("İkinci Yarı - Yediği Gol Ortalaması", ViewType.Area);
        public Series series2 = new Series("Genel - Yediği Gol Ortalaması", ViewType.Bar);
        public Series series3 = new Series("İç Saha - Attığı Gol Ortalaması", ViewType.Bar);
        public Series series4 = new Series("İç Saha - Yediği Gol Ortalaması", ViewType.Bar);
        public Series series5 = new Series("Dış Saha - Attığı Gol Ortalaması", ViewType.Bar);
        public Series series6 = new Series("Dış Saha - Yediği Gol Ortalaması", ViewType.Bar);
        public Series series7 = new Series("İlk Yarı - Attığı Gol Ortalaması", ViewType.Bar);
        public Series series8 = new Series("İlk Yarı - Yediği Gol Ortalaması", ViewType.Bar);
        public Series series9 = new Series("İkinci Yarı - Attığı Gol Ortalaması", ViewType.Bar);

        public string StageFK = string.Empty;

        public DataTable tbl = new DataTable();
        private double topOran = 1;

        public MacBulteni()
        {
            InitializeComponent();
            gridView1.BestFitColumns();
            MaclariListeleFootballStatistics();
            //MaclariListele3();

            tbl.Columns.Add("Maç No", typeof(int));
            tbl.Columns.Add("Evsahibi", typeof(string));
            tbl.Columns.Add("Misafir", typeof(string));
            tbl.Columns.Add("Tahmin", typeof(string));
            tbl.Columns.Add("Oran", typeof(double));

            gridKupon.DataSource = tbl;
            textBox1.ReadOnly = true;
            textBox2.Text = "2";
            textBox3.ReadOnly = true;
            splitContainerControl2.SplitterPosition = splitContainerControl2.Width / 2;
            splitContainerControl5.SplitterPosition = splitContainerControl5.Width / 2;
            //splitContainerControl2.Panel1.Size = new Size(500, 500);
            //splitContainerControl2.Panel2.Size = new Size(500, 500);
            

        }

        private void MaclariListele()
        {
            try
            {
                if (baglantiLocalhost.State == ConnectionState.Closed)
                    baglantiLocalhost.Open();
                var cmdMaclariListele = new SqlCommand("dbo.MaclariListele", baglantiLocalhost);
                cmdMaclariListele.CommandType = CommandType.StoredProcedure;
                var MaclariListeleAdapter = new SqlDataAdapter(cmdMaclariListele);
                var MaclariListeleDataset = new DataSet();
                cmdMaclariListele.CommandTimeout = 120;
                MaclariListeleAdapter.Fill(MaclariListeleDataset, "Deneme");
                gridControl1.DataSource = MaclariListeleDataset.Tables[0];
                if (baglantiLocalhost.State == ConnectionState.Open)
                    baglantiLocalhost.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Empty + ex);
            }
        }

        private void IcSahaPuanDurumuGetir()
        {
            try
            {
                var row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                var LiginAdi = row[3].ToString();
                var ulke = row[4].ToString();
                if (baglantiLocalhost.State == ConnectionState.Closed)
                    baglantiLocalhost.Open();

                var LigIdCek =
                    new SqlCommand(
                        "select top 1 Id from api.lig where adi = '" + LiginAdi + "' and ulke='" + ulke +
                        "' order by sezon desc", baglantiLocalhost);
                var oku = LigIdCek.ExecuteReader();

                while (oku.Read())
                    ligid = Convert.ToInt32(oku[0].ToString());

                baglantiLocalhost.Close();

                baglantiLocalhost.Open();
                var cmdGenelPuanDurumu = new SqlCommand(
                    "select * from dbo.IcSahaPuanDurumuByLigId( " + ligid + ") order by p desc,av desc,ag desc",
                    baglantiLocalhost);
                var MaclariListeleAdapter = new SqlDataAdapter(cmdGenelPuanDurumu);
                var MaclariListeleDataset = new DataSet();
                cmdGenelPuanDurumu.CommandTimeout = 120;
                MaclariListeleAdapter.Fill(MaclariListeleDataset, "Deneme");
                gridControl5.DataSource = MaclariListeleDataset.Tables[0];
                if (baglantiLocalhost.State == ConnectionState.Open)
                    baglantiLocalhost.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Empty + ex);
            }
        }

        private void DisSahaPuanDurumuGetir()
        {
            try
            {
                var row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                var LiginAdi = row[3].ToString();
                var ulke = row[4].ToString();
                if (baglantiLocalhost.State == ConnectionState.Closed)
                    baglantiLocalhost.Open();

                var LigIdCek =
                    new SqlCommand(
                        "select top 1 Id from api.lig where adi = '" + LiginAdi + "' and ulke='" + ulke +
                        "' order by sezon desc", baglantiLocalhost);
                var oku = LigIdCek.ExecuteReader();

                while (oku.Read())
                    ligid = Convert.ToInt32(oku[0].ToString());

                baglantiLocalhost.Close();

                baglantiLocalhost.Open();
                var cmdGenelPuanDurumu = new SqlCommand(
                    "select * from dbo.DisSahaPuanDurumuByLigId( " + ligid + ") order by p desc,av desc,ag desc",
                    baglantiLocalhost);
                var MaclariListeleAdapter = new SqlDataAdapter(cmdGenelPuanDurumu);
                var MaclariListeleDataset = new DataSet();
                cmdGenelPuanDurumu.CommandTimeout = 120;
                MaclariListeleAdapter.Fill(MaclariListeleDataset, "Deneme");
                gridControl4.DataSource = MaclariListeleDataset.Tables[0];
                if (baglantiLocalhost.State == ConnectionState.Open)
                    baglantiLocalhost.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : "+string.Empty + ex);
            }
        }

        private void GenelPuandurumuGetir()
        {
            try
            {
                var row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                var LiginAdi = row[3].ToString();
                var ulke = row[4].ToString();
                if (baglantiLocalhost.State == ConnectionState.Closed)
                    baglantiLocalhost.Open();

                var LigIdCek =
                    new SqlCommand(
                        "select top 1 Id from api.lig where adi = '" + LiginAdi + "' and ulke='" + ulke +
                        "' order by sezon desc", baglantiLocalhost);
                var oku = LigIdCek.ExecuteReader();

                while (oku.Read())
                    ligid = Convert.ToInt32(oku[0].ToString());

                baglantiLocalhost.Close();

                baglantiLocalhost.Open();
                var cmdGenelPuanDurumu = new SqlCommand(
                    "select * from dbo.GenelPuanDurumuByLigId( " + ligid + ") order by p desc,av desc,ag desc",
                    baglantiLocalhost);
                var MaclariListeleAdapter = new SqlDataAdapter(cmdGenelPuanDurumu);
                var MaclariListeleDataset = new DataSet();
                cmdGenelPuanDurumu.CommandTimeout = 120;
                MaclariListeleAdapter.Fill(MaclariListeleDataset, "Deneme");
                gridControl2.DataSource = MaclariListeleDataset.Tables[0];
                if (baglantiLocalhost.State == ConnectionState.Open)
                    baglantiLocalhost.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Empty + ex);
            }
        }

        private void MaclariListeleFootballStatistics()
        {
            try
            {
                if (baglantiLocalhost.State == ConnectionState.Closed)
                    baglantiLocalhost.Open();
                var cmdMaclariListele = new SqlCommand("dbo.MaclariListele", baglantiLocalhost);
                cmdMaclariListele.CommandType = CommandType.StoredProcedure;
                var MaclariListeleAdapter = new SqlDataAdapter(cmdMaclariListele);
                var MaclariListeleDataset = new DataSet();
                cmdMaclariListele.CommandTimeout = 120;
                MaclariListeleAdapter.Fill(MaclariListeleDataset, "Deneme");
                gridControl1.DataSource = MaclariListeleDataset.Tables[0];
                if (baglantiLocalhost.State == ConnectionState.Open)
                    baglantiLocalhost.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Empty + ex);
            }
        }

        private void MaclariListele3()
        {
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            var ListeGetir = new SqlCommand(@"        SELECT  TEMP.* ,
                CAST (90
                / CASE WHEN ISNULL(( SELECT dbo.MS1YuzdeHesapla(TEMP.tournament_stageFK,
                                                              TEMP.HomeTeamName,
                                                              TEMP.AwayTeamName)
                                   ), 90) > 0
                       THEN ISNULL(( SELECT dbo.MS1YuzdeHesapla(TEMP.tournament_stageFK,
                                                              TEMP.HomeTeamName,
                                                              TEMP.AwayTeamName)
                                   ), 90)
                       ELSE 90
                  END AS DECIMAL(4, 2)) MS1 ,
                CAST (90
                / CASE WHEN ISNULL(( SELECT dbo.MS0YuzdeHesapla(TEMP.tournament_stageFK,
                                                              TEMP.HomeTeamName,
                                                              TEMP.AwayTeamName)
                                   ), 90) > 0
                       THEN ISNULL(( SELECT dbo.MS0YuzdeHesapla(TEMP.tournament_stageFK,
                                                              TEMP.HomeTeamName,
                                                              TEMP.AwayTeamName)
                                   ), 90)
                       ELSE 90
                  END AS DECIMAL(4, 2)) MS0 ,
                CAST  (90
                / CASE WHEN ISNULL(( SELECT dbo.MS2YuzdeHesapla(TEMP.tournament_stageFK,
                                                              TEMP.HomeTeamName,
                                                              TEMP.AwayTeamName)
                                   ), 90) > 0
                       THEN ISNULL(( SELECT dbo.MS2YuzdeHesapla(TEMP.tournament_stageFK,
                                                              TEMP.HomeTeamName,
                                                              TEMP.AwayTeamName)
                                   ), 90)
                       ELSE 90
                  END AS DECIMAL(4, 2)) MS2
        FROM    ( SELECT    Id ,
							tournament_stageFK,
                            StartDate Tarih ,
                            TournamentName ,
                            HomeTeamName ,
                            HomeTeamLogo ,
                            AwayTeamName ,
                            AwayTeamLogo
                  FROM      dbo.LeagueFixture
                  WHERE     StartDate BETWEEN GETDATE() AND DATEADD(DAY, 3,
                                                              GETDATE())
                ) TEMP
				ORDER BY  Tarih", baglantiLocalhost);
            try
            {
                var ListeGetirReader = ListeGetir.ExecuteReader();
                gridControl1.DataSource = ListeGetirReader;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Empty + ex);
            }


            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
        }

        private void GenelPuanDurumuListele()
        {
            try
            {
                //if (baglantiLocalhost.State == ConnectionState.Closed)
                //    baglantiLocalhost.Open();
                if (baglantiLocalhost.State == ConnectionState.Closed)
                    baglantiLocalhost.Open();
                var cmdMaclariListele = new SqlCommand("dbo.MaclariListele", baglantiLocalhost);
                cmdMaclariListele.CommandType = CommandType.StoredProcedure;

                var MaclariListeleAdapter = new SqlDataAdapter(cmdMaclariListele);
                var MaclariListeleDataset = new DataSet();
                cmdMaclariListele.CommandTimeout = 120;
                MaclariListeleAdapter.Fill(MaclariListeleDataset, "Deneme");
                gridControl1.DataSource = MaclariListeleDataset.Tables[0];


                if (baglantiLocalhost.State == ConnectionState.Open)
                    baglantiLocalhost.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Empty + ex);
            }
        }

        private void MaclariListele2()
        {
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            var ListeGetir = new SqlCommand(@"   SELECT  TEMP.* ,
            CAST (90
            / CASE WHEN ISNULL(( SELECT dbo.MS1YuzdeHesapla(lig,
                                                            home_team_name,
                                                            away_team_name)
                               ), 90) > 0
                   THEN ISNULL(( SELECT dbo.MS1YuzdeHesapla(lig,
                                                            home_team_name,
                                                            away_team_name)
                               ), 90)
                   ELSE 90
              END AS DECIMAL(4, 2)) MS1 ,
            CAST (90
            / CASE WHEN ISNULL(( SELECT dbo.MS0YuzdeHesapla(lig,
                                                            home_team_name,
                                                            away_team_name)
                               ), 90) > 0
                   THEN ISNULL(( SELECT dbo.MS0YuzdeHesapla(lig,
                                                            home_team_name,
                                                            away_team_name)
                               ), 90)
                   ELSE 90
              END AS DECIMAL(4, 2)) MS0 ,
            CAST  (90
            / CASE WHEN ISNULL(( SELECT dbo.MS2YuzdeHesapla(lig,
                                                            home_team_name,
                                                            away_team_name)
                               ), 90) > 0
                   THEN ISNULL(( SELECT dbo.MS2YuzdeHesapla(lig,
                                                            home_team_name,
                                                            away_team_name)
                               ), 90)
                   ELSE 90
              END AS DECIMAL(4, 2)) MS2
    FROM    ( SELECT    matchid MacNo ,
                        CAST(matchDate AS DATETIME) Tarih ,
                        l.Ikon L ,
                        l.Adi lig ,
                        ( SELECT TOP 1
                                    tt.Ikon
                          FROM      Tanimlar.Takimlar tt ( NOLOCK )
                          WHERE     tt.whoScoredId = f.hometeamid
                        ) E ,
                        ( SELECT TOP 1
                                    tt.Ad
                          FROM      Tanimlar.Takimlar tt ( NOLOCK )
                          WHERE     tt.whoScoredId = f.hometeamid
                        ) home_team_name ,
                        ( SELECT TOP 1
                                    tt2.Ikon
                          FROM      Tanimlar.Takimlar tt2 ( NOLOCK )
                          WHERE     tt2.whoScoredId = f.awayteamid
                        ) M ,
                        ( SELECT TOP 1
                                    tt.Ad
                          FROM      Tanimlar.Takimlar tt ( NOLOCK )
                          WHERE     tt.whoScoredId = f.awayTeamid
                        ) away_team_name
              FROM      Fixture f ( NOLOCK )
                        INNER JOIN Ligler l ( NOLOCK ) ON l.WhoScoredKodu = f.TournamentCode
              WHERE     f.Durum = 1
                        AND f.matchDate BETWEEN GETDATE()
                                        AND     DATEADD(DAY, 5, GETDATE())
--and matchid=906266 
            ) TEMP
--WHERE (lig = @Lig OR lig = NULL OR TEMP.E = @Takim OR TEMP.E = NULL OR TEMP.M = @Takim OR TEMP.M = NULL)
--WHERE (TEMP.E LIKE @Takim OR TEMP.M LIKE @Takim)
    ORDER BY Tarih", baglantiLocalhost);

            var ListeGetirReader = ListeGetir.ExecuteReader();
            while (ListeGetirReader.Read()) gridControl1.DataSource = ListeGetirReader;

            //SplashScreenManager.ShowForm(typeof(frmWait));
            //SplashScreenManager.Default.SetWaitFormCaption("Lütfen bekleyiniz..");
            //SplashScreenManager.Default.SetWaitFormDescription("Liste üzerinde çalışılıyor..");
            ////Başlamadan önce 3 saniye bekletiliyor.
            //Thread.Sleep(1000);

            ////MessageBox.Show(""+gridView1.RowCount);
            //SplashScreenManager.Default.SendCommand(frmWait.WaitFormCommand.SetProgressBarVisible, true);
            ////Ardından Progressbarımızın maximum değerini bildiriyoruz.. (default olarak 100'dür bunu bildirmez isek for içerisinde i = 101 olduğunda hata alırız.)
            //SplashScreenManager.Default.SendCommand(frmWait.WaitFormCommand.SetProgressBarMaximum, gridView1.RowCount);
            ////Grid içerisindeki kayıt sayısı kadar dönerek progressbar üzerinde görüntüleme işlemi yapılıyor.
            //for (var i = 0; i < gridView1.RowCount; i++)
            //{
            //    //Açıklama yazısı içinde şu kadar işlem tamamlandı şeklinde bilgi veriyoruz..
            //    SplashScreenManager.Default.SetWaitFormDescription(string.Format("Yapılan işlem: {0}/{1}", i + 1,
            //        gridView1.RowCount));

            //    //Waitformumuza SetProgressBarPosition isimli komutu ve değerimizi göndererek, progressbarın ilerlemesini sağlıyoruz.
            //    SplashScreenManager.Default.SendCommand(frmWait.WaitFormCommand.SetProgressBarPosition, i);
            //}

            //SplashScreenManager.CloseForm();


            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            MaclariListeleFootballStatistics();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    var row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    lblEvSahibi.Text = row[5].ToString();
                    lblMisafir.Text = row[7].ToString();
                    //splitContainerControl2.Panel1.Text = row[4].ToString();
                    //splitContainerControl2.Panel2.Text = row[7].ToString();
                    var LiginAdi = row[3].ToString();
                    //lblEvsahibiIYSkor.Text = row[9].ToString();
                    //lblEvsahibiIkiYSkor.Text = row[11].ToString();
                    //lblEvSahibiSkor.Text = row[5].ToString();
                    //lblMisafirSkor.Text = row[8].ToString();
                    //lblMisafirIYSkor.Text = row[10].ToString();
                    //lblMisafirIkiYSkor.Text = row[12].ToString();

                    //LigLogoGetir();
                    //LigIdGetir();
                    //EvSahibitakimIdsiniGetir();
                    //MisafirtakimIdsiniGetir();
                    //EvSahibiLogoGetir();
                    //MisafirLogoGetir();

                    var Ortalamalar = new Hesaplamalar();

                    lblEvSahibiGenelGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiGenelAttıgiGolOrtalamasi(EvsahibiTakimId, LiginAdi));
                    lblEvSahibiGenelYedigiGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiGenelYedigiGolOrtalamasi(EvsahibiTakimId, LiginAdi));
                    lblEvSahibiIcSahaGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiIcSahaAttigiGolOrtalamasi(EvsahibiTakimId, LiginAdi));
                    lblEvSahibiIcSahaYedigiGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiIcSahaYedigiGolOrtalamasi(EvsahibiTakimId, LiginAdi));
                    lblEvSAhibiDisSahaGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiDisSahaAttıgiGolOrtalamasi(EvsahibiTakimId, LiginAdi));
                    lblEvsahibiDisSahaYedigiGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiDisSahaYedigiGolOrtalamasi(EvsahibiTakimId, LiginAdi));
                    lblEvSahibiIlkYariGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiIlkYariAttıgiGolOrtalamasi(EvsahibiTakimId, LiginAdi));
                    lblEvSahibiIlkYariYedigiGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiIlkYariYedigiGolOrtalamasi(EvsahibiTakimId, LiginAdi));
                    lblEvSahibiIkinciYariGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiIkinciYariAttıgiGolOrtalamasi(EvsahibiTakimId, LiginAdi));
                    lblEvSahibiIkinciYariYedigiGol.Text =
                        Convert.ToString(Ortalamalar.EvsahibiIkinciYariYedigiGolOrtalamasi(EvsahibiTakimId, LiginAdi));


                    lblMisafirGenelGol.Text =
                        Convert.ToString(Ortalamalar.MisafirGenelAttigiGolOrtalamasi(MisafirTakimId, LiginAdi));
                    lblMisafirGenelYedigiGol.Text =
                        Convert.ToString(Ortalamalar.MisafirGenelYedigiGolOrtalamasi(MisafirTakimId, LiginAdi));
                    lblMisafirIcSahaGol.Text =
                        Convert.ToString(Ortalamalar.MisafirIcSahaAttigiGolOrtalamasi(MisafirTakimId, LiginAdi));
                    lblMisafirIcSahaYedigiGol.Text =
                        Convert.ToString(Ortalamalar.MisafirIcSahaYedigiGolOrtalamasi(MisafirTakimId, LiginAdi));
                    lblMisafirDisSahaGol.Text =
                        Convert.ToString(Ortalamalar.MisafirDisSahaAttigiGolOrtalamasi(MisafirTakimId, LiginAdi));
                    lblMisafirDisSahaYedigiGol.Text =
                        Convert.ToString(Ortalamalar.MisafirDisSahaYedigiGolOrtalamasi(MisafirTakimId, LiginAdi));
                    lblMisafirIlkYariGol.Text =
                        Convert.ToString(Ortalamalar.MisafirIlkYariAttıgiGolOrtalamasi(MisafirTakimId, LiginAdi));
                    lblMisafirIlkYariYedigiGol.Text =
                        Convert.ToString(Ortalamalar.MisafirIlkYariYedigiGolOrtalamasi(MisafirTakimId, LiginAdi));
                    lblMisafirIkinciYariGol.Text =
                        Convert.ToString(Ortalamalar.MisafirIkinciYariAttıgiGolOrtalamasi(MisafirTakimId, LiginAdi));
                    lblMisafirIkinciYariYedigiGol.Text =
                        Convert.ToString(Ortalamalar.MisafirIkinciYariYedigiGolOrtalamasi(MisafirTakimId, LiginAdi));


                    Ortalamalar.EvsahibiMacSonucuAttigiGolTahmini =
                        (Ortalamalar.EvsahibiGenelAttıgiGolOrtalamasi(EvsahibiTakimId, LiginAdi) +
                         Ortalamalar.MisafirGenelYedigiGolOrtalamasi(MisafirTakimId, LiginAdi) +
                         Ortalamalar.EvsahibiIcSahaAttigiGolOrtalamasi(EvsahibiTakimId, LiginAdi) +
                         Ortalamalar.MisafirDisSahaYedigiGolOrtalamasi(MisafirTakimId, LiginAdi)) / 4;

                    Ortalamalar.MisafirMacSonucuAttigiGolTahmini =
                        (Ortalamalar.MisafirGenelAttigiGolOrtalamasi(MisafirTakimId, LiginAdi) +
                         Ortalamalar.EvsahibiGenelYedigiGolOrtalamasi(EvsahibiTakimId, LiginAdi) +
                         Ortalamalar.MisafirDisSahaAttigiGolOrtalamasi(MisafirTakimId, LiginAdi) +
                         Ortalamalar.EvsahibiIcSahaYedigiGolOrtalamasi(EvsahibiTakimId, LiginAdi)) / 4;

                    var fark1 = Ortalamalar.EvsahibiMacSonucuAttigiGolTahmini -
                                Ortalamalar.MisafirMacSonucuAttigiGolTahmini;
                    var fark2 = Ortalamalar.MisafirMacSonucuAttigiGolTahmini -
                                Ortalamalar.EvsahibiMacSonucuAttigiGolTahmini;
                    var toplamGolSayisi = Ortalamalar.MisafirMacSonucuAttigiGolTahmini +
                                          Ortalamalar.EvsahibiMacSonucuAttigiGolTahmini;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Liste yokkkkkkk!   " + ex);
            }
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                //gridView1.GetFocusedRowCellValue("Id").ToString()

                var sutunlar = new List<string>();
                sutunlar.Add("Maç No");
                sutunlar.Add("Tarih");
                sutunlar.Add("L");
                sutunlar.Add("Lig");
                sutunlar.Add("E");
                sutunlar.Add("Ev Sahibi");
                sutunlar.Add("M");
                sutunlar.Add("Misafir");
                MacId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("MacNo").ToString());

                var tahmin = e.Column.ToString();
                if (!sutunlar.Contains(tahmin))
                {
                    var evsahibi = gridView1.GetFocusedRowCellValue("EvSahibi").ToString();
                    var misafir = gridView1.GetFocusedRowCellValue("Misafir").ToString();


                    if (tahmin == "LogoEv" || tahmin == "LogoMis")
                    {
                        MessageBox.Show("Yanlış yere tıkladınız!!!", "Dikkat", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        var cellValue = e.CellValue;
                        if (cellValue != null)
                        {


                            var oran = Convert.ToDouble(cellValue.ToString().Replace('.', ','));
                            originalMatchIDs = (from r in tbl.AsEnumerable() select r.Field<int>("Maç No")).ToList();

                            if (sender != null && satirsayisi <= 9)
                            {
                                if (!originalMatchIDs.Contains(MacId))
                                {
                                    tbl.Rows.Add(MacId, evsahibi, misafir, tahmin, oran);
                                    gridKupon.DataSource = tbl;
                                    satirsayisi = tbl.Rows.Count;
                                    topOran *= oran;
                                    textBox1.Text = Math.Round(topOran, 2, MidpointRounding.AwayFromZero).ToString();
                                    if (textBox2.Text != string.Empty && Convert.ToDouble(textBox2.Text) >= 2)
                                    {
                                        misli = Convert.ToDouble(textBox2.Text);
                                        textBox3.Text =
                                            (Math.Round(topOran, 2, MidpointRounding.AwayFromZero) * misli).ToString();
                                    }
                                    else
                                    {
                                        MessageBox.Show("En az 2 TL'lik kupon yapabilirsiniz!!!");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(string.Empty + MacId +
                                                    " numaralı maç kuponunuza daha önce eklenmiştir!!!");
                                }
                            }
                            else
                            {
                                MessageBox.Show(@"En Fazla 10 Adet maçı kuponunuza ekleyebilirsiniz!!!");
                            }


                            if (sender == null) MessageBox.Show(@"Kayıt bulunamadı!!");
                        }
                    }
                }
            }
            catch (DbEntityValidationException f)
            {
                foreach (var eve in f.EntityValidationErrors)
                {
                    MessageBox.Show(
                        string.Format(
                            "Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));

                    foreach (var ve in eve.ValidationErrors)
                        MessageBox.Show(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"",
                            ve.PropertyName,
                            ve.ErrorMessage));
                }
            }


            //gridKupon.DataSource = tbl;
            try
            {
                chartTitle1.Text = string.Empty;
                chartTitle2.Text = string.Empty;
                chartTitle3.Text = string.Empty;
                chartTitle4.Text = string.Empty;

                chartControl1.Titles.Remove(chartTitle1);
                chartControl2.Titles.Remove(chartTitle2);
                chartControl3.Titles.Remove(chartTitle3);
                chartControl4.Titles.Remove(chartTitle4);

                chartControl1.Series.Remove(series1);
                chartControl1.Series.Remove(series2);
                chartControl1.Series.Remove(series3);
                chartControl1.Series.Remove(series4);
                chartControl1.Series.Remove(series5);
                chartControl1.Series.Remove(series6);
                chartControl1.Series.Remove(series7);
                chartControl1.Series.Remove(series8);
                chartControl1.Series.Remove(series9);
                chartControl1.Series.Remove(series10);
                chartControl1.Series.Remove(series11);
                chartControl1.Series.Remove(series12);
                chartControl2.Series.Remove(series13);
                chartControl2.Series.Remove(series14);

                chartControl3.Series.Remove(series15);
                chartControl3.Series.Remove(series16);
                chartControl4.Series.Remove(series17);
                chartControl4.Series.Remove(series18);

                //int Mac = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Id").ToString());
                lblEvSahibi.Text = gridView1.GetFocusedRowCellValue("EvSahibi").ToString();
                lblMisafir.Text = gridView1.GetFocusedRowCellValue("Misafir").ToString();

                //System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                //lblEvSahibi.Text = row[5].ToString();
                //lblMisafir.Text = row[7].ToString();
                var LiginAdi = gridView1.GetFocusedRowCellValue("Lig").ToString();
                EvsahibiTakim = lblEvSahibi.Text;
                MisafirTakim = lblMisafir.Text;
                if (baglantiLocalhost.State == ConnectionState.Closed)
                    baglantiLocalhost.Open();
                var cek =
                    new SqlCommand(
                        @"SELECT ligId,EvsahibiTakimId,MisafirTakimId FROM Api.Fikstur WHERE Id=" + MacId +
                        string.Empty,
                        baglantiLocalhost);
                var cekReader = cek.ExecuteReader();
                while (cekReader.Read())
                {
                    StageFK = cekReader[0].ToString();
                    EvsahibiTakimId = Convert.ToInt32(cekReader[1].ToString());
                    MisafirTakimId = Convert.ToInt32(cekReader[2].ToString());
                }

                baglantiLocalhost.Close();
                var Ortalamalar = new Hesaplamalar();

                lblEvSahibiGenelGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiGenelAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiGenelYedigiGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiGenelYedigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIcSahaGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIcSahaAttigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIcSahaYedigiGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIcSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSAhibiDisSahaGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiDisSahaAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvsahibiDisSahaYedigiGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiDisSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIlkYariGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIlkYariAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIlkYariGolIc.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIlkYariIcSahaAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIlkYariGolDis.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIlkYariDisSahaAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIlkYariYedigiGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIlkYariYedigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIlkYariYedigiGolIc.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIlkYariIcSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIlkYariYedigiGolDis.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIlkYariDisSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIkinciYariGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIkinciYariAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIkinciYariGolIc.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIkinciYariIcSahaAttigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIkinciYariGolDis.Text =
                    Convert.ToString(
                        Ortalamalar.EvsahibiIkinciYariDisSahaAttigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIkinciYariYedigiGol.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIkinciYariYedigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIkinciYariYedigiGolIc.Text =
                    Convert.ToString(Ortalamalar.EvsahibiIkinciYariIcSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK));
                lblEvSahibiIkinciYariYedigiGolDis.Text =
                    Convert.ToString(
                        Ortalamalar.EvsahibiIkinciYariDisSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK));


                lblMisafirGenelGol.Text =
                    Convert.ToString(Ortalamalar.MisafirGenelAttigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirGenelYedigiGol.Text =
                    Convert.ToString(Ortalamalar.MisafirGenelYedigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIcSahaGol.Text =
                    Convert.ToString(Ortalamalar.MisafirIcSahaAttigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIcSahaYedigiGol.Text =
                    Convert.ToString(Ortalamalar.MisafirIcSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirDisSahaGol.Text =
                    Convert.ToString(Ortalamalar.MisafirDisSahaAttigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirDisSahaYedigiGol.Text =
                    Convert.ToString(Ortalamalar.MisafirDisSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIlkYariGol.Text =
                    Convert.ToString(Ortalamalar.MisafirIlkYariAttıgiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIlkYariYedigiGol.Text =
                    Convert.ToString(Ortalamalar.MisafirIlkYariYedigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIkinciYariGol.Text =
                    Convert.ToString(Ortalamalar.MisafirIkinciYariAttıgiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIkinciYariYedigiGol.Text =
                    Convert.ToString(Ortalamalar.MisafirIkinciYariYedigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIlkYariGolIc.Text =
                    Convert.ToString(Ortalamalar.MisafirIlkYariIcSahaAttıgiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIlkYariGolDis.Text =
                    Convert.ToString(Ortalamalar.MisafirIlkYariDisSahAttigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIlkYariYedigiGolIc.Text =
                    Convert.ToString(Ortalamalar.MisafirIlkYariIcSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIlkYariYedigiGolDis.Text =
                    Convert.ToString(Ortalamalar.MisafirIlkYariDisSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIkinciYariGolIc.Text =
                    Convert.ToString(Ortalamalar.MisafirIkinciYariIcSahaAttigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIkinciYariGolDis.Text =
                    Convert.ToString(Ortalamalar.MisafirIkinciYariDisSahaAttigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIkinciYariYedigiGolIc.Text =
                    Convert.ToString(Ortalamalar.MisafirIkinciYariIcSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK));
                lblMisafirIkinciYariYedigiGolDis.Text =
                    Convert.ToString(Ortalamalar.MisafirIkinciYariDisSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK));

                Ortalamalar.EvsahibiMacSonucuAttigiGolTahmini =
                    (Ortalamalar.EvsahibiGenelAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK) +
                     Ortalamalar.MisafirGenelYedigiGolOrtalamasi(MisafirTakimId, StageFK) +
                     Ortalamalar.EvsahibiIcSahaAttigiGolOrtalamasi(EvsahibiTakimId, StageFK) +
                     Ortalamalar.MisafirDisSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK)) / 4;

                Ortalamalar.MisafirMacSonucuAttigiGolTahmini =
                    (Ortalamalar.MisafirGenelAttigiGolOrtalamasi(MisafirTakimId, StageFK) +
                     Ortalamalar.EvsahibiGenelYedigiGolOrtalamasi(EvsahibiTakimId, StageFK) +
                     Ortalamalar.MisafirDisSahaAttigiGolOrtalamasi(MisafirTakimId, StageFK) +
                     Ortalamalar.EvsahibiIcSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)) / 4;

                var fark1 = Ortalamalar.EvsahibiMacSonucuAttigiGolTahmini -
                            Ortalamalar.MisafirMacSonucuAttigiGolTahmini;
                var fark2 = Ortalamalar.MisafirMacSonucuAttigiGolTahmini -
                            Ortalamalar.EvsahibiMacSonucuAttigiGolTahmini;
                var toplamGolSayisi = Ortalamalar.MisafirMacSonucuAttigiGolTahmini +
                                      Ortalamalar.EvsahibiMacSonucuAttigiGolTahmini;

                // Barda ki ilk sütun - Attığı Gol Sayısı
                series1 = new Series("Genel - Attığı Gol Ortalaması", ViewType.Bar);
                series1.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiGenelAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series1.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirGenelAttigiGolOrtalamasi(MisafirTakimId, StageFK)));

                series2 = new Series("Genel - Yediği Gol Ortalaması", ViewType.Bar);
                series2.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiGenelYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series2.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirGenelYedigiGolOrtalamasi(MisafirTakimId, StageFK)));

                series3 = new Series("İç Saha - Attığı Gol Ortalaması", ViewType.Bar);
                series3.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIcSahaAttigiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series3.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIcSahaAttigiGolOrtalamasi(MisafirTakimId, StageFK)));

                series4 = new Series("İç Saha - Yediği Gol Ortalaması", ViewType.Bar);
                series4.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIcSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series4.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIcSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK)));

                series5 = new Series("Dış Saha - Attığı Gol Ortalaması", ViewType.Bar);
                series5.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiDisSahaAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series5.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirDisSahaAttigiGolOrtalamasi(MisafirTakimId, StageFK)));

                series6 = new Series("Dış Saha - Yediği Gol Ortalaması", ViewType.Bar);
                series6.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiDisSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series6.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirDisSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK)));

                series7 = new Series("İlk Yarı - Attığı Gol Ortalaması", ViewType.Bar);
                series7.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIlkYariAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series7.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIlkYariAttıgiGolOrtalamasi(MisafirTakimId, StageFK)));

                series8 = new Series("İlk Yarı - Yediği Gol Ortalaması", ViewType.Bar);
                series8.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIlkYariYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series8.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIlkYariYedigiGolOrtalamasi(MisafirTakimId, StageFK)));

                series9 = new Series("İkinci Yarı - Attığı Gol Ortalaması", ViewType.Bar);
                series9.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIkinciYariAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series9.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIkinciYariAttıgiGolOrtalamasi(MisafirTakimId, StageFK)));

                series10 = new Series("İkinci Yarı - Yediği Gol Ortalaması", ViewType.Bar);
                series10.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIkinciYariYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series10.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIkinciYariYedigiGolOrtalamasi(MisafirTakimId, StageFK)));

                // Add the series to the chart.
                //chartControl1.Series.Add(series1);
                //chartControl1.Series.Add(series2);
                ////chartControl1.Series.Add(series3);
                ////chartControl1.Series.Add(series4);
                ////chartControl1.Series.Add(series5);
                ////chartControl1.Series.Add(series6);
                //chartControl1.Series.Add(series7);
                //chartControl1.Series.Add(series8);
                //chartControl1.Series.Add(series9);
                //chartControl1.Series.Add(series10);

                series11 = new Series("Attığı Gol Sayısı | Genel", ViewType.Bar);
                series11.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiGenelAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series11.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirGenelAttigiGolOrtalamasi(MisafirTakimId, StageFK)));


                series12 = new Series("Yediği Gol Sayısı | Genel", ViewType.Bar);
                series12.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirGenelYedigiGolOrtalamasi(MisafirTakimId, StageFK)));
                series12.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiGenelYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)));


                series13 = new Series("Attığı Gol Sayısı | İç - Dış Saha", ViewType.Bar);
                series13.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIcSahaAttigiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series13.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirDisSahaAttigiGolOrtalamasi(MisafirTakimId, StageFK)));


                series14 = new Series("Yediği Gol Sayısı | İç - Dış Saha", ViewType.Bar);
                series14.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirDisSahaYedigiGolOrtalamasi(MisafirTakimId, StageFK)));
                series14.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIcSahaYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)));


                series15 = new Series("İlk Yarı | Attığı Gol Ortalaması", ViewType.Bar);
                series15.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIlkYariAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series15.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIlkYariAttıgiGolOrtalamasi(MisafirTakimId, StageFK)));

                series16 = new Series("İlk Yarı | Yediği Gol Ortalaması", ViewType.Bar);
                series16.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIlkYariYedigiGolOrtalamasi(MisafirTakimId, StageFK)));
                series16.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIlkYariYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)));


                series17 = new Series("İkinci Yarı | Attığı Gol Ortalaması", ViewType.Bar);
                series17.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIkinciYariAttıgiGolOrtalamasi(EvsahibiTakimId, StageFK)));
                series17.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIkinciYariAttıgiGolOrtalamasi(MisafirTakimId, StageFK)));

                series18 = new Series("İkinci Yarı | Yediği Gol Ortalaması", ViewType.Bar);
                series18.Points.Add(new SeriesPoint(MisafirTakim,
                    Ortalamalar.MisafirIkinciYariYedigiGolOrtalamasi(MisafirTakimId, StageFK)));
                series18.Points.Add(new SeriesPoint(EvsahibiTakim,
                    Ortalamalar.EvsahibiIkinciYariYedigiGolOrtalamasi(EvsahibiTakimId, StageFK)));

                chartControl1.Series.Add(series11);
                chartControl1.Series.Add(series12);
                chartControl2.Series.Add(series13);
                chartControl2.Series.Add(series14);
                chartControl3.Series.Add(series15);
                chartControl3.Series.Add(series16);
                chartControl4.Series.Add(series17);
                chartControl4.Series.Add(series18);

                // Açıklama gözüksün
                chartControl1.Legend.Visibility = DefaultBoolean.True;
                chartControl2.Legend.Visibility = DefaultBoolean.True;
                chartControl3.Legend.Visibility = DefaultBoolean.True;
                chartControl4.Legend.Visibility = DefaultBoolean.True;

                //chartControl1.Legend.UseCheckBoxes = false;
                //chartControl1.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.RightOutside;
                //chartControl1.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;

                // Yatay için = true yapmak lazım
                ((XYDiagram)chartControl1.Diagram).Rotated = false;

                // Chart için başlık yazabilirim

                chartTitle1.Text = "Genel Gol Analizi";
                chartControl1.Titles.Add(chartTitle1);

                chartTitle2.Text = "İç - Dış Saha Gol Analizi";
                chartControl2.Titles.Add(chartTitle2);

                chartTitle3.Text = "İlk Yarı Gol Analizi";
                chartControl3.Titles.Add(chartTitle3);

                chartTitle4.Text = "İkinci Yarı Gol Analizi";
                chartControl4.Titles.Add(chartTitle4);
            }
            catch (DbEntityValidationException f)
            {
                foreach (var eve in f.EntityValidationErrors)
                {
                    MessageBox.Show(
                        string.Format(
                            "Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));

                    foreach (var ve in eve.ValidationErrors)
                        MessageBox.Show(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"",
                            ve.PropertyName,
                            ve.ErrorMessage));
                }
            }

            GenelPuandurumuGetir();
            IcSahaPuanDurumuGetir();
            DisSahaPuanDurumuGetir();
        }


        private void gridKupon_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var row2 = gridView2.GetDataRow(gridView2.FocusedRowHandle);
                var oran = Convert.ToDouble(row2[4].ToString());
                tbl.Rows.Remove(row2);
                satirsayisi -= 1; //gridKupon.DataSource=tbl;
                //MessageBox.Show("" + satirsayisi);
                //MessageBox.Show("" + topOran);

                if (satirsayisi == 0)
                {
                    textBox1.Text = "0";
                    textBox3.Text = "0";
                    topOran = 1;
                }
                else
                {
                    misli = Convert.ToDouble(textBox2.Text);
                    topOran = Math.Round(Convert.ToDouble(textBox3.Text), 2, MidpointRounding.AwayFromZero);
                    topOran /= oran;
                    //topOran = Math.Round(topOran / oran, 2, MidpointRounding.AwayFromZero);


                    textBox1.Text = Math.Round(topOran / misli, 2, MidpointRounding.AwayFromZero).ToString();
                    if (textBox2.Text != " " && Convert.ToDouble(textBox2.Text) >= 2)
                    {
                        textBox3.Text = Math.Round(topOran, 2, MidpointRounding.AwayFromZero).ToString();
                        //bunifuImageButton1.Enabled = true;
                    }
                    else
                    {
                        //bunifuImageButton1.Enabled = false;

                        MessageBox.Show("En az 2 TL'lik kupon yapabilirsiniz!!!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Listede hiç maç yok!!!");
            }
        }

        private void gridView2_ShowingEditor(object sender, CancelEventArgs e)
        {
            var view = sender as GridView;
            if (!view.IsNewItemRow(view.FocusedRowHandle)) //&& view.FocusedColumn.FieldName == "Column3")
                e.Cancel = true;
        }


        private void KuponIslemKaydet()
        {
            try

            {
                gridKupon.DataSource = tbl;

                var kuponId = KuponNumarasi();

                foreach (DataRow row in tbl.Rows)
                {
                    var macId = Convert.ToInt32(row[0].ToString());
                    var KuponOran = Convert.ToDouble(row[4].ToString());
                    var tahmin = row[3].ToString();
                    var tahminId = Tahmin(tahmin);

                    if (baglantiLocalhost.State == ConnectionState.Closed)
                        baglantiLocalhost.Open();
                    var KuponIslemKaydet = new SqlCommand(@"INSERT INTO [dbo].[KuponIslem]
           ([KuponId]
           ,[MacId]
           ,[TahminId]
           ,[Oran]
           ,[Durum]
           ,[created_date])
            VALUES
           (@KuponId
           ,@MacId
           ,@TahminId
           ,@Oran
           ,1
           ,getdate())", baglantiLocalhost);

                    KuponIslemKaydet.CommandType = CommandType.Text;
                    KuponIslemKaydet.Parameters.AddWithValue("@KuponId", kuponId);
                    KuponIslemKaydet.Parameters.AddWithValue("@MacId", macId);
                    KuponIslemKaydet.Parameters.AddWithValue("@TahminId", tahminId);
                    KuponIslemKaydet.Parameters.AddWithValue("@Oran", KuponOran);
                    KuponIslemKaydet.ExecuteNonQuery();
                    if (baglantiLocalhost.State == ConnectionState.Open)
                        baglantiLocalhost.Close();
                }

                tbl.Clear();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Hata : " + Ex);
            }
        }

        private void KuponKaydet()
        {
            try
            {
                kazanc = topOran * misli;


                if (baglantiLocalhost.State == ConnectionState.Closed)
                    baglantiLocalhost.Open();

                var KuponKaydet =
                    new SqlCommand(
                        @"Insert into Kupon (KullaniciId,Oran,Misli,Kazanc,durum,created_date,kuponTipiId) VALUES(@KullaniciId, @Oran,@Misli,@Kazanc,1,getdate(),1)",
                        baglantiLocalhost);
                KuponKaydet.CommandType = CommandType.Text;
                KuponKaydet.Parameters.AddWithValue("@KullaniciId", kullaniciID);
                KuponKaydet.Parameters.AddWithValue("@Oran", topOran);
                KuponKaydet.Parameters.AddWithValue("@Misli", misli);
                KuponKaydet.Parameters.AddWithValue("@Kazanc", kazanc);
                var sonuc = KuponKaydet.ExecuteNonQuery();
                if (sonuc > 0)
                    MessageBox.Show("Kuponunuz başarılı bir şekilde oluşturuldu!!");
                if (baglantiLocalhost.State == ConnectionState.Open)
                    baglantiLocalhost.Close();
            }


            catch (Exception Ex)
            {
                MessageBox.Show("Hata : " + Ex);
            }
        }

        public int KuponNumarasi()
        {
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            var KuponNo = new SqlCommand("select max(Id) from Kupon", baglantiLocalhost);
            var KuponNoOku = KuponNo.ExecuteReader();
            while (KuponNoOku.Read()) KuponId = Convert.ToInt32(KuponNoOku[0].ToString());

            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
            return KuponId;
        }

        public int Tahmin(string tahminn)
        {
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            var Tahmin =
                new SqlCommand("select Id from Tahminler where adi='" + tahminn + "'", baglantiLocalhost);
            var TahminOku = Tahmin.ExecuteReader();
            while (TahminOku.Read()) TahminId = Convert.ToInt32(TahminOku[0].ToString());

            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
            return TahminId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //    SqlConnection con = new SqlConnection("Data Source=GATE-PC\\SQLEXPRESS;Initial Catalog=dbProfile;Integrated Security=True");
            //con.Open();
            if (satirsayisi > 1 && Convert.ToDouble(textBox2.Text) >= 2)
            {
                KuponKaydet();
                KuponIslemKaydet();
                satirsayisi = 0;
                if (satirsayisi == 0)
                {
                    textBox1.Text = "0";
                    textBox3.Text = "0";
                    topOran = 1;
                }
            }
            else
            {
                MessageBox.Show(@"En az 2 maç ve 2 TL için kupon oluşturabilirsiniz!!!");
            }
        }

        private void layoutMaclar_CustomButtonClick(object sender,
            BaseButtonEventArgs e)
        {
            MaclariListeleFootballStatistics();
        }

        private void gridView1_ShowingEditor_1(object sender, CancelEventArgs e)
        {
            var view = sender as GridView;
            if (!view.IsNewItemRow(view.FocusedRowHandle)) //&& view.FocusedColumn.FieldName == "Column3")
                e.Cancel = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                GenelPuandurumuGetir();
                IcSahaPuanDurumuGetir();
                DisSahaPuanDurumuGetir();
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            MessageBox.Show("Deneme");
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            if (satirsayisi > 1 && Convert.ToDouble(textBox2.Text) >= 2)
            {
                KuponKaydet();
                KuponIslemKaydet();
                satirsayisi = 0;
                if (satirsayisi == 0)
                {
                    textBox1.Text = "0";
                    textBox3.Text = "0";
                    topOran = 1;
                }
            }
            else
            {
                MessageBox.Show(@"En az 2 maç ve 2 TL için kupon oluşturabilirsiniz!!!");
            }
        }

        private void layoutControlGroup17_CustomButtonClick(object sender,
            BaseButtonEventArgs e)
        {
            MaclariListeleFootballStatistics();
        }

        private void gridView1_CustomDrawCell(object sender,
            RowCellCustomDrawEventArgs e)
        {
            if (e.Column == clmL)
            {
                e.Appearance.BackColor = Color.AntiqueWhite;
                e.Appearance.BackColor2 = Color.DodgerBlue;
                e.Appearance.GradientMode = LinearGradientMode.ForwardDiagonal;
            }
        }

        //private void DeplasmanLogoGetir(object sender,
        //    DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        //{
        //    DataRow dr = (e.Row as DataRowView).Row;
        //    string url = dr[7].ToString();
        //    if (iconsCache.ContainsKey(url))
        //    {
        //        e.Value = iconsCache[url];
        //        return;
        //    }

        //    var request = WebRequest.Create(url);
        //    using (
        //        var response = request.GetResponse())
        //    {
        //        using (var stream = response.GetResponseStream())
        //        {
        //            Image original = Image.FromStream(stream);
        //            Bitmap resized = new Bitmap(original, new Size(original.Width / 8, original.Height / 8));
        //            e.Value = resized;
        //        }
        //    }
        //}

        //private void gridView1_CustomUnboundColumnData(object sender,
        //    DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        //{
        //    if (e.Column.FieldName == "Image")
        //    {
        //        DataRow dr = (e.Row as DataRowView).Row;
        //        string url = dr[5].ToString();
        //        if (iconsCache.ContainsKey(url))
        //        {
        //            e.Value = iconsCache[url];
        //            return;
        //        }

        //        var request = WebRequest.Create(url);
        //        using (
        //            var response = request.GetResponse())
        //        {
        //            using (var stream = response.GetResponseStream())
        //            {
        //                Image original = Image.FromStream(stream);
        //                Bitmap resized = new Bitmap(original, new Size(original.Width / 3, original.Height / 3));
        //                e.Value = resized;
        //            }
        //        }
        //    }

        //    if (e.Column.FieldName == "Image2")
        //    {
        //        DataRow dr = (e.Row as DataRowView).Row;
        //        string url = dr[7].ToString();
        //        if (iconsCache.ContainsKey(url))
        //        {
        //            e.Value = iconsCache[url];
        //            return;
        //        }

        //        var request = WebRequest.Create(url);
        //        using (
        //            var response = request.GetResponse())
        //        {
        //            using (var stream = response.GetResponseStream())
        //            {
        //                Image original = Image.FromStream(stream);
        //                Bitmap resized = new Bitmap(original, new Size(original.Width / 3, original.Height / 3));
        //                e.Value = resized;
        //            }
        //        }
        //    }
        //}
    }
}