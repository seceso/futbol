using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXApplication2
{
    class Baglanti
    {
        public static void openconnection()
        {

            string connectionString = GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

            }

        }

        public static void closeconnection()
        {
            string connectionString = GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Close();

            }
        }
        static public string GetConnectionString()
        {

            //return "Server = psl.dynu.com; uid=selsor;Password=123456;Database = futbol; Trusted_Connection = false";

            return "server=localhost;database=FootballStatistics;user=sa;password=123456;Trusted_Connection = false";

        }
    }
}
