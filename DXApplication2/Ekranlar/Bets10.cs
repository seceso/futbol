using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using deneme;


namespace DXApplication2.Ekranlar
{
    public partial class Bets10 : Form
    {
        public Bets10()
        {
            InitializeComponent();         
        }

        private void btnFikstur_Click(object sender, EventArgs e)
        {
            fiksturUC1.BringToFront();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            oyuncularUC1.BringToFront();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            ıstatistikUC1.BringToFront();
        }
    }
}
