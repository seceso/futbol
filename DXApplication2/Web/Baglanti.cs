using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;

namespace TahminManager.web
{
    public class Baglanti
    {
        static public string GetConnectionString()
        {

            return "server=localhost;database=futbol;user=sa;password=123456";//trusted_connection=true
                    //return "server=psl.dynu.com;database=futbol;user=selsor;password=123456";//trusted_connection=true


        }

        public void openconnection()
        {

            string connectionString = GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();

            }

        }

        public Baglanti()
        {
            // TODO: Complete member initialization
        }
    }
}
