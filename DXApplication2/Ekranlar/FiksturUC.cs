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

namespace DXApplication2.Ekranlar
{
    public partial class FiksturUC : UserControl
    {
        public FiksturUC()
        {
            InitializeComponent();
            baglantiLocalhost.Open();
            SqlCommand context = new SqlCommand(@"SELECT  Id ,
        TournamentName ,
        HomeTeamName ,
        HomeTeamScore ,
        AwayTeamName ,
        AwayTeamScore ,
        StartDate ,
        MatchDay
        FROM    dbo.LeagueFixture
        WHERE   ( HomeTeamName = 'aberdeen'
          OR AwayTeamName = 'aberdeen'
        )	
", baglantiLocalhost);
            SqlDataAdapter MaclariListeleAdapter = new SqlDataAdapter(context);
            DataSet MaclariListeleDataset = new DataSet();
            context.CommandTimeout = 30000;
            MaclariListeleAdapter.Fill(MaclariListeleDataset, "Deneme");
            gridControl1.DataSource = MaclariListeleDataset.Tables[0];
            baglantiLocalhost.Close();


            //gridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            //gridView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;

        }

        public SqlConnection baglantiLocalhost =
           new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");
    }
}
