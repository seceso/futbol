using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DXApplication2.Web;
using TahminManager;
using System.Runtime.InteropServices;


namespace DXApplication2.Ekranlar
{
    public partial class Ligler : Form
    {
        public Ligler()
        {
            InitializeComponent();
        }

        public void Writer(KeyValuePair<string, string> pair, int total, int current)
        {
            if (ListBox1 != null)
                ListBox1.Items.Add(pair.Key + " - " + pair.Value + "[" + current + "/" + total + "]");
        }

        public void MacWriter(KeyValuePair<string, string> pair, int total, int current)
        {
            if (ListBox1 != null)
                ListBox1.Items.Add(pair.Key + " - " + pair.Value + "[" + current + "/" + total + "]");
        }

        public void MaclarWriter(KeyValuePair<string, string> pair, int total, int current)
        {
            if (ListBox1 != null)
                ListBox1.Items.Add(pair.Key + " - " + pair.Value + "[" + current + "/" + total + "]");
        }
        
        private void TurkiyeLigiConfig()
        {
            Globe.LeaguesDic.Clear();
            Globe.LeaguesDic.Add("Turkey-Super-Lig", "/Regions/225/Tournaments/17/Turkey-Super-Lig");
        }
        private void btnTurkiyeLigi_Click(object sender, EventArgs e)
        {
            TurkiyeLigiConfig();
            try
            {
                SpiderAsync spider = new SpiderAsync();
                spider.GetAllLeagues(Writer);
            }
            catch (Exception)
            {
                ListBox1.Items.Add(Globe.LeaguesDic.ToString());
            }


            try
            {
                SpiderAsync spider=new SpiderAsync();
                spider.GetAllMatches();
            }
            catch (Exception)
            {
                
                throw;
            }

        }
    }
}
