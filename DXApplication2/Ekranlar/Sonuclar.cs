using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Controls;
using System.Windows.Forms;

namespace DXApplication2.Ekranlar
{
    public partial class Sonuclar : Form
    {
        public Sonuclar()
        {
            InitializeComponent();
            sonuclariListele();
        }

        public SqlConnection baglantiLocalhost =
            new SqlConnection(
                "server=5.135.247.254,1435;database=FootballStatistics;user=sa;password=sa123456!;pooling=false");


        private void sonuclariListele()
        {
            try
            {
                if (baglantiLocalhost.State == ConnectionState.Closed)
                    baglantiLocalhost.Open();
                var cmdMaclariListele = new SqlCommand("dbo.SonuclariListele", baglantiLocalhost);
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
                MessageBox.Show("" + ex);
            }
        }

        private void MaclariListele3()
        {
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            SqlCommand ListeGetir = new SqlCommand(@"  SELECT  lf.Id ,
        ( SELECT TOP 1
                    ls.countryName
          FROM      dbo.LeagueStage ls
          WHERE     ls.Id = lf.tournament_stageFK
        ) Ulke ,
        ( SELECT TOP 1
                    ls.TournamentName
          FROM      dbo.LeagueStage ls
          WHERE     ls.Id = lf.tournament_stageFK
        ) Turnuva ,
        tournament_stageFK ,
        lf.MatchDay ,
        lf.StartDate Tarih ,
        HomeTeamName ,
        HomeTeamLogo ,
        AwayTeamName ,
        AwayTeamLogo,
        HomeTeamScore + '-' + AwayTeamScore + '|' +'('+ HomeTeamHalfScore + '-' + AwayTeamHalfScore + ')' Skor
FROM    dbo.LeagueFixture lf
WHERE   lf.StartDate BETWEEN DATEADD(DAY, -3, GETDATE())
                     AND     GETDATE() and lf.eventstatusFK=6
ORDER BY Tarih;	", baglantiLocalhost);
            SqlDataReader ListeGetirReader = ListeGetir.ExecuteReader();
            while (ListeGetirReader.Read())
            {
                gridControl1.DataSource = ListeGetirReader;
            }


            if (baglantiLocalhost.State == ConnectionState.Open)
                baglantiLocalhost.Close();
        }
        //public DataRowView dr;
        //public DataRow dr;

        public int ev, misafir;

        private void gridView1_CustomDrawCell(object sender,
            DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
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


            //}
        }
    }
}