using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXApplication2.Web
{
    public class istatistikApi
    {
        public SqlConnection baglantiLocalhost =
           new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");
        public List<int> EvsahibiKadro(List<int> EvsahibiKadrosu,int eventId)
        {
            EvsahibiKadrosu = null;
            eventId = 1191564;
               SqlCommand kadro = new SqlCommand(@"SELECT player_id FROM dbo.MatchLineUps WHERE  EventId='"+eventId+"' and number=1", baglantiLocalhost);
            SqlDataReader kadroReader = kadro.ExecuteReader();
            while(kadroReader.Read())
            {
                EvsahibiKadrosu.Add(Convert.ToInt32(kadroReader[0].ToString()));

            }
            return EvsahibiKadrosu;
        }

    }
}
