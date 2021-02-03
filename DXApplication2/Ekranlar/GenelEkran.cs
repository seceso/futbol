using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using deneme;
using DevExpress.XtraGrid.Views.Grid;
using System.Net;
using System.IO;
using DevExpress.XtraEditors.Repository;

namespace DXApplication2
{
    public partial class GenelEkran : UserControl
    {
        public GenelEkran()
        {
            InitializeComponent();
            Ulkeler();

        }
        public string country = "";
        public int stageFK;
        public string turnuva="";
        public SqlConnection baglantiLocalhost =
                new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");
        public void Ulkeler()
        {
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            string sorgu = @"SELECT  country_name FROM country ORDER BY country_name";
            SqlCommand ccc = new SqlCommand(sorgu, baglantiLocalhost);
            SqlDataReader reader = ccc.ExecuteReader();
            while (reader.Read())
            {
                cboxUlke.Properties.Items.Add(reader[0].ToString());
            }

            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
            baglantiLocalhost.Open();
        }
        public void Turnuvalar()
        {
            if (cboxUlke.SelectedIndex == -1)
            {
                country = "";
            }
            else
            {
                country = cboxUlke.Text;

            }
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            string sorgu = @"SELECT  distinct TournamentName
                            FROM    dbo.LeagueStage (NOLOCK)
                            INNER JOIN country c ON c.countryId = countryFK
                            WHERE   c.country_name = '" + country + "'";
            SqlCommand ccc = new SqlCommand(sorgu, baglantiLocalhost);
            SqlDataReader reader = ccc.ExecuteReader();
            while (reader.Read())
            {
                cboxTurnuva.Properties.Items.Add(reader[0].ToString());
            }

            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
            baglantiLocalhost.Open();

            //comboBoxEdit2.SelectedIndex = 0;
        }
        public void sezon()
        {
            if (cboxUlke.SelectedIndex == -1)
            {
                turnuva = "";
            }
            else
            {
                turnuva = cboxTurnuva.Text;

            }
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            string sorgu = @"SELECT name FROM    dbo.LeagueStage ls ( NOLOCK )
                            INNER JOIN country c ON c.countryId = countryFK
                            WHERE   ls.TournamentName = '" + turnuva + "'";
            SqlCommand ccc = new SqlCommand(sorgu, baglantiLocalhost);
            SqlDataReader reader = ccc.ExecuteReader();
            while (reader.Read())
            {
                cbozSezon.Properties.Items.Add(reader[0].ToString());
            }

            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
            baglantiLocalhost.Open();

            //comboBoxEdit3.SelectedIndex = 0;
        }
     

        public void LigleriGetir()
        {
            if (cboxUlke.SelectedIndex == -1)
            {
                country = "";
            }
            else
            {
                country = cboxUlke.Text;

            }
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            string sorgu = @"SELECT DISTINCT
        l.name ,l.Id
        FROM    dbo.League l
        INNER JOIN dbo.LeagueStage ls ON ls.TournamentName = l.name
        INNER JOIN country c ( NOLOCK ) ON c.countryId = ls.countryFK
        WHERE   l.isTournament = 0
        AND c.country_name ='" + country + "' ORDER BY l.Id";
            SqlCommand ccc = new SqlCommand(sorgu, baglantiLocalhost);
            SqlDataReader reader = ccc.ExecuteReader();
            while (reader.Read())
            {
                cboxTurnuva.Properties.Items.Add(reader[0].ToString());
            }

            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
            baglantiLocalhost.Open();
        }


        private void KupalariGetir()
        {
            if (cboxUlke.SelectedIndex == -1)
            {
                country = "";
            }
            else
            {
                country = cboxUlke.Text;

            }
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            string sorgu = @"SELECT DISTINCT
        l.name ,l.Id
        FROM    dbo.League l
        INNER JOIN dbo.LeagueStage ls ON ls.TournamentName = l.name
        INNER JOIN country c ( NOLOCK ) ON c.countryId = ls.countryFK
        WHERE   l.isTournament = 1
        AND c.country_name ='" + country + "' ORDER BY l.Id";
            SqlCommand ccc = new SqlCommand(sorgu, baglantiLocalhost);
            SqlDataReader reader = ccc.ExecuteReader();
            while (reader.Read())
            {
                cboxTurnuva.Properties.Items.Add(reader[0].ToString());
            }

            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
            baglantiLocalhost.Open();
        }

        private void cboxUlke_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboxTurnuva.Properties.Items.Clear();
            cboxTip.SelectedIndex = -1;
            cbozSezon.Properties.Items.Clear();
            cboxTurnuva.SelectedIndex = -1;
            cbozSezon.SelectedIndex = -1;

            //LigleriGetir();
        }

        private void cboxTip_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboxTurnuva.Properties.Items.Clear();
            cbozSezon.Properties.Items.Clear();
            if (cboxTip.Text == "Lig")
            {
                cboxTurnuva.SelectedIndex = -1;
                cbozSezon.SelectedIndex = -1;
                LigleriGetir();

            }
            if (cboxTip.Text == "Kupa")
            {
                cboxTurnuva.SelectedIndex = -1;
                cbozSezon.SelectedIndex = -1;
                KupalariGetir();

            }

        }

        private void cboxTurnuva_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbozSezon.Properties.Items.Clear();
            sezon();
        }

        public void maclariListele()
        {
            string sorgu = @"SELECT  lf.Id ,( SELECT TOP 1 ls.countryName FROM  dbo.LeagueStage ls  WHERE  ls.Id = lf.tournament_stageFK) Ulke ,( SELECT TOP 1 ls.TournamentName FROM dbo.LeagueStage ls  WHERE  ls.Id = lf.tournament_stageFK ) Turnuva ,tournament_stageFK , CASE WHEN SUBSTRING(lf.MatchDay, 0, 3) LIKE '%.%' THEN CAST(SUBSTRING(lf.MatchDay, 0, 2) AS INT) ELSE CAST(SUBSTRING(lf.MatchDay, 0, 3) AS INT) END Matchday , lf.StartDate Tarih ,        HomeTeamName ,        (SELECT logo FROM logo l (NOLOCK) WHERE  l.TakimId=lf.HomeTeamParticipantFK) HomeTeamLogo ,        AwayTeamName ,        (SELECT logo FROM logo l (NOLOCK) WHERE  l.TakimId=lf.AwayTeamParticipantFK) AwayTeamLogo ,        HomeTeamScore + '-' + AwayTeamScore + '|' + '(' + HomeTeamHalfScore        + '-' + AwayTeamHalfScore + ')' Skor FROM   dbo.LeagueFixture lf WHERE  lf.eventstatusFK <> 0  AND lf.tournament_stageFK = ( SELECT    ls.Id   FROM      dbo.LeagueStage ls ( NOLOCK ) INNER JOIN country c ON c.countryId = countryFK  WHERE  ls.TournamentName = '" + cboxTurnuva.Text + "' AND ls.name = '" + cbozSezon.Text + "')   ORDER BY matchday";

            //SqlDataAdapter MaclariListeleAdapter = new SqlDataAdapter(cmdMaclariListele);
            //DataSet MaclariListeleDataset = new DataSet();
            //cmdMaclariListele.CommandTimeout = 120;
            //MaclariListeleAdapter.Fill(MaclariListeleDataset, "Deneme");
            //gridControl1.DataSource = MaclariListeleDataset.Tables[0];



            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            SqlCommand ListeGetir = new SqlCommand(sorgu, baglantiLocalhost);
            SqlDataAdapter MaclariListeleAdapter = new SqlDataAdapter(ListeGetir);
            DataSet listegetirds = new DataSet();
            MaclariListeleAdapter.Fill(listegetirds, "Deneme");
            gridControl1.DataSource = listegetirds.Tables[0];
            //gridView1.Columns["HomeTeamLogo"].ColumnEdit = new RepositoryItemPictureEdit();
            //gridView1.Columns["AwayTeamLogo"].ColumnEdit = new RepositoryItemPictureEdit();

            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
        }
        public int ev, misafir;

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Bastım!!!");
            maclariListele();
        }

        public void ImageYap(string url)
        {
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(url);
            MemoryStream ms = new MemoryStream(bytes);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == clmMacNo)

            {
                e.Appearance.BackColor = Color.AntiqueWhite;
                e.Appearance.BackColor2 = Color.DarkSlateGray;
                e.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            }

            if (e.Column == clmEvsahibi)
            {

                GridView currentView = sender as GridView;
                var x = currentView.GetRowCellValue(e.RowHandle, clmSkor).ToString();
                int y = x.Length;

                if (y == 7)
                {
                    var z = x.Substring(0, y - 4);
                    ev = Convert.ToInt32(z.Substring(0, 1));
                    misafir = Convert.ToInt32(z.Substring(2, 1));
                }
                if (y > 7)
                {
                    if (y > 9)
                    {
                        var b = x.Substring(0, y - 6);
                        ev = Convert.ToInt32(b.Substring(0, 2));
                        misafir = Convert.ToInt32(b.Substring(3, 1));
                    }
                    else
                    {
                        var z = x.Substring(0, y - 6);
                        ev = Convert.ToInt32(z.Substring(0, 1));
                        misafir = Convert.ToInt32(z.Substring(2, 1));
                    }

                }
                if (ev > misafir)
                {
                    e.Appearance.BackColor = Color.AntiqueWhite;
                    e.Appearance.BackColor2 = Color.SpringGreen;
                    e.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                }


                if (ev == misafir)
                {
                    e.Appearance.BackColor = Color.AntiqueWhite;
                    e.Appearance.BackColor2 = Color.Yellow;
                    e.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                }


                if (ev < misafir)
                {
                    e.Appearance.BackColor = Color.AntiqueWhite;
                    e.Appearance.BackColor2 = Color.Tomato;
                    e.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                }

            }











            if (e.Column == clmMisafir)
            {
                GridView currentView = sender as GridView;
                var x = currentView.GetRowCellValue(e.RowHandle, clmSkor).ToString();
                int y = x.Length;

                if (y == 7)
                {
                    var z = x.Substring(0, y - 4);
                    ev = Convert.ToInt32(z.Substring(0, 1));
                    misafir = Convert.ToInt32(z.Substring(2, 1));
                }
                if (y > 7)
                {
                    if (y > 9)
                    {
                        var b = x.Substring(0, y - 6);
                        ev = Convert.ToInt32(b.Substring(0, 2));
                        misafir = Convert.ToInt32(b.Substring(3, 1));
                    }
                    else
                    {
                        var z = x.Substring(0, y - 6);
                        ev = Convert.ToInt32(z.Substring(0, 1));
                        misafir = Convert.ToInt32(z.Substring(2, 1));
                    }
                }
                if (ev < misafir)
                {
                    e.Appearance.BackColor = Color.AntiqueWhite;
                    e.Appearance.BackColor2 = Color.SpringGreen;
                    e.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                }


                if (ev == misafir)
                {
                    e.Appearance.BackColor = Color.AntiqueWhite;
                    e.Appearance.BackColor2 = Color.Yellow;
                    e.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                }


                if (ev > misafir)
                {
                    e.Appearance.BackColor = Color.AntiqueWhite;
                    e.Appearance.BackColor2 = Color.Tomato;
                    e.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
                }

            }
        }
    }
}
