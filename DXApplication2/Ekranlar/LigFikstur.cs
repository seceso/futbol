using deneme;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXApplication2.Ekranlar
{
    public partial class LigFikstur : Form
    {
        public LigFikstur()
        {
            InitializeComponent();
          
             
    }
        public SqlConnection baglantiLocalhost =
                new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");

        private void genelEkran1_Load(object sender, EventArgs e)
        {

        }
    }
}
