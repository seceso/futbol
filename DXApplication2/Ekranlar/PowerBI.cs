using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXApplication2.Ekranlar
{
    public partial class PowerBI : Form
    {
        public string not = "Son sayfa yenileme zamanı : ";
        public PowerBI()
        {
            InitializeComponent();


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            webBrowser1.Refresh();



        }
    }
}
