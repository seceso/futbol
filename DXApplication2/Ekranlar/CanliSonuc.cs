using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Validation;
using System.Timers;
using System.Web;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using unirest_net.http;

namespace DXApplication2.Ekranlar
{
    public partial class CanliSonuc : Form
    {
        public CanliSonuc()
        {
            InitializeComponent();

            tablo.Columns.Add("Maç No", typeof(int));
            tablo.Columns.Add("Tarih", typeof(string));
            tablo.Columns.Add("Lig", typeof(string));
            tablo.Columns.Add("Evsahibi", typeof(string));
            tablo.Columns.Add("E", typeof(string));
            tablo.Columns.Add("Misafir", typeof(string));
            tablo.Columns.Add("M", typeof(string));
            tablo.Columns.Add("Skor", typeof(string));
            tablo.Columns.Add("Dakika", typeof(string));
            gridControl1.DataSource = tablo;
            gridView1.Columns[1].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            
            tablo2.Columns.Add("Mac No", typeof(int));
            tablo2.Columns.Add("Dk", typeof(int));
            tablo2.Columns.Add("Olay", typeof(string));
            tablo2.Columns.Add("Aciklama", typeof(string));
            gridControl2.DataSource = tablo2;
            gridView2.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;


            try
            {
                deneme.Api api = new deneme.Api();
                Listele();
            }
            catch (DbEntityValidationException f)
            {
                foreach (var eve in f.EntityValidationErrors)
                {
                    MessageBox.Show(string.Format("Entity türü \"{0}\" şu hatalara sahip \"{1}\" Geçerlilik hataları:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    //Response.Write();
                    foreach (var ve in eve.ValidationErrors)
                    {
                        MessageBox.Show(string.Format("- Özellik: \"{0}\", Hata: \"{1}\"", ve.PropertyName,
                            ve.ErrorMessage));
                    }

                    //Response.End();
                }
            }


            if (timer1.Enabled == true)
            {
                timer1.Enabled = false;
            }
            else
            {
                timer1.Enabled = true;
            }

            if (timer2.Enabled == true)
            {
                timer2.Enabled = false;
            }
            else
            {
                timer2.Enabled = true;
            }


            //MessageBox.Show("İşlem Bitti!!");
        }

        //public SqlConnection baglantiLocalhost =
        //    new SqlConnection("server=localhost;database=futbol;user=selsor;password=123456;pooling=false");

        public DataTable tablo = new DataTable();
        public DataTable tablo2 = new DataTable();


        public void Listele()
        {
            tablo.Clear();


            Control.CheckForIllegalCrossThreadCalls = false;


            HttpResponse<string> response = Unirest.get("https://api-football-v1.p.rapidapi.com/v2/fixtures/live?timezone=Europe/Istanbul")
                .header("X-RapidAPI-Host", "api-football-v1.p.rapidapi.com")
                .header("X-RapidAPI-Key", "23318d19bamsh244e6077658cdc4p1b34f5jsnd8ffa072526c")
                .asJson<string>();


            //string timezone = "Europe/Istanbul";
            //HttpResponse<string> response =
            //    Unirest.get("https://api-football-v1.p.rapidapi.com/v2/fixtures/live" +
            //                "?timezone=" + timezone)
            //        .header("X-RapidAPI-Host", "Api-football-v1.p.rapidapi.com")
            //        .header("X-RapidAPI-Key", "23318d19bamsh244e6077658cdc4p1b34f5jsnd8ffa072526c")
            //        .asJson<string>();


            Rootobject Api = JsonConvert.DeserializeObject<Rootobject>(response.Body);
            foreach (var fixture in Api.api.fixtures)
            {
                DateTime tarih;
                int MacNo = fixture.fixture_id;
                tarih = Convert.ToDateTime(fixture.event_date);
                string lig = fixture.league.country+" | "+fixture.league.name;
                string evsahibi = fixture.homeTeam.team_name;
                string EvLogo = fixture.homeTeam.logo;
                string misafir = fixture.awayTeam.team_name;
                string MLogo = fixture.awayTeam.logo;
                string skor = fixture.goalsHomeTeam.ToString() + " - " + fixture.goalsAwayTeam;
                string a = fixture.elapsed.ToString();
                //        a = item.ElapsedMin;
                tablo.Rows.Add(new object[] { MacNo, tarih, lig, evsahibi, EvLogo, misafir, MLogo, skor, a });
                gridControl1.DataSource = tablo;

                foreach (var item in fixture.events)
                {
                    int dakika = item.elapsed;
                    string olay = item.detail;
                    string aciklama = item.type;

                tablo2.Rows.Add(new object[] { MacNo,dakika, olay, aciklama });
                gridControl2.DataSource = tablo2;
                }
            }
        }



        public class Rootobject
        {
            public Api api { get; set; }
        }

        public class Api
        {
            public int results { get; set; }
            public Fixture[] fixtures { get; set; }
        }

        public class Fixture
        {
            public int fixture_id { get; set; }
            public int league_id { get; set; }
            public League league { get; set; }
            public DateTime event_date { get; set; }
            public int event_timestamp { get; set; }
            public int firstHalfStart { get; set; }
            public int? secondHalfStart { get; set; }
            public string round { get; set; }
            public string status { get; set; }
            public string statusShort { get; set; }
            public int elapsed { get; set; }
            public string venue { get; set; }
            public string referee { get; set; }
            public Hometeam homeTeam { get; set; }
            public Awayteam awayTeam { get; set; }
            public int goalsHomeTeam { get; set; }
            public int goalsAwayTeam { get; set; }
            public Score score { get; set; }
            public Event[] events { get; set; }
        }

        public class League
        {
            public string name { get; set; }
            public string country { get; set; }
            public string logo { get; set; }
            public string flag { get; set; }
        }

        public class Hometeam
        {
            public int team_id { get; set; }
            public string team_name { get; set; }
            public string logo { get; set; }
        }

        public class Awayteam
        {
            public int team_id { get; set; }
            public string team_name { get; set; }
            public string logo { get; set; }
        }

        public class Score
        {
            public string halftime { get; set; }
            public object fulltime { get; set; }
            public object extratime { get; set; }
            public object penalty { get; set; }
        }

        public class Event
        {
            public int elapsed { get; set; }
            public int team_id { get; set; }
            public string teamName { get; set; }
            public int? player_id { get; set; }
            public string player { get; set; }
            public int? assist_id { get; set; }
            public string assist { get; set; }
            public string type { get; set; }
            public string detail { get; set; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 60000;
            Listele();
            //MessageBox.Show("Refresh!!!!","Tekrar",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //timer2.Interval = 50000;
            //if (tablo2.Rows.Count > 0)
            //{
            //    RefreshListe();
            //}
        }

        public void RefreshListe()
        {
            //tablo2.Clear();

           int  MacId = Convert.ToInt32(gridView1.GetFocusedRowCellValue("Maç No").ToString());

            gridView2.SetAutoFilterValue(gridView2.Columns[0], MacId.ToString(),AutoFilterCondition.Equals);
        }


        private void gridView1_Click(object sender, EventArgs e)
        {
            RefreshListe();
        }
    }
}