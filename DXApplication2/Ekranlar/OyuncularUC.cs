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

namespace DXApplication2
{
    public partial class OyuncularUC : UserControl
    {
        public OyuncularUC()
        {
            InitializeComponent();
            Listele("");
        }
        public void Listele(string takimAdi)
        {
            baglantiLocalhost.Open();
            SqlCommand context = new SqlCommand(@"SELECT  DISTINCT
                                                m.name ,
                                                COUNT(m.EventId) ToplamMac
                                                FROM    dbo.MatchLineUps m
                                                WHERE   participantFK IN ( SELECT   f.HomeTeamParticipantFK
                                                                   FROM     dbo.LeagueFixture f
                                                                   WHERE    f.HomeTeamName = 'Aberdeen' )
                                                GROUP BY m.name
                                                ORDER BY name	
                                                ", baglantiLocalhost);
            SqlDataAdapter MaclariListeleAdapter = new SqlDataAdapter(context);
            DataSet MaclariListeleDataset = new DataSet();
            context.CommandTimeout = 30000;
            MaclariListeleAdapter.Fill(MaclariListeleDataset, "Deneme");
            gridControl1.DataSource = MaclariListeleDataset.Tables[0];
            baglantiLocalhost.Close();
        }
        public void OyuncuAnaliz(string oyuncu)
        {
            tablo.Columns.Add("X", typeof(string));
            tablo.Columns.Add("Y", typeof(string));
            tablo.Columns.Add("Z", typeof(string));
            gridControl2.DataSource = tablo;

            //tablo.Rows.Add(new object[] { MacNo, tarih, lig, evsahibi, EvLogo, misafir, MLogo, skor, a });
            //gridControl1.DataSource = tablo;
            //gridView2.Columns[0].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
        }
        public SqlConnection baglantiLocalhost =
         new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");
        public DataTable tablo = new DataTable();
    }
}
