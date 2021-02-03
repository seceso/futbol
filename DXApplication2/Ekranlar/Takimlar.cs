using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;

namespace DXApplication2.Ekranlar
{
    public partial class Takimlar : DevExpress.XtraEditors.XtraForm
    {
        public Takimlar()
        {
            InitializeComponent();
            TakimlariListele();
        }
        public string IdAl;
        public int takimId;

        //public SqlConnection baglantiLocalhost =
        //    new SqlConnection("server=psl.dynu.com;database=futbol;user=selsor;password=123456;pooling=false");

        //public SqlConnection baglantiLocalhost = new SqlConnection("server=localhost;database=futbol;user=sa;password=123456");//trusted_connection=true
        public SqlConnection baglantiLocalhost =
            new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");

        //trusted_connection=true

        public void TakimlariListele()
        {
            baglantiLocalhost.Open();
            //string cmd = "select * from tanimlar.takimlar where durum=1 and whoscoredId is not null and Ikon is null";
            string cmd = @"SELECT * FROM Tanimlar.Takimlar tt (NOLOCK)
WHERE EXISTS (SELECT DISTINCT m.home_team_id FROM dbo.matchinformation2 m (NOLOCK)
WHERE m.home_team_id=tt.whoScoredId AND m.start_time>'2016-01-01')";

            SqlDataAdapter TakimListesiDataAdapter = new SqlDataAdapter(cmd
                , baglantiLocalhost);
            DataTable takimListesiListesiDataTable = new DataTable();
            TakimListesiDataAdapter.Fill(takimListesiListesiDataTable);
            macListesi.DataSource = takimListesiListesiDataTable;
            baglantiLocalhost.Close();
        }
        private void güncelleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            takimId = Convert.ToInt32(row[0].ToString());
            Ekranlar.TakimGuncelle et = new Ekranlar.TakimGuncelle();
            et.Id = deger;
            et.Show();
        }
        public int deger
        {
            get { return takimId; }
            set { takimId = value; }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TakimlariListele();

        }
        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;
            if (!view.IsNewItemRow(view.FocusedRowHandle)) //&& view.FocusedColumn.FieldName == "Column3")
                e.Cancel = true;
        }


    }
}