using DevExpress.XtraCharts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXApplication2
{
    public partial class FormDeneme : Form
    {
        public FormDeneme()
        {
            InitializeComponent();
        }
        public SqlConnection baglantiLocalhost =
            new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create an empty chart.
            //ChartControl sideBySideBarChart = new ChartControl();


            // Barda ki ilk sütun
            Series series1 = new Series("Attığı Gol", ViewType.Bar);
            series1.Points.Add(new SeriesPoint("AC Milan", 10));
            series1.Points.Add(new SeriesPoint("Barcelona", 12));
            //series1.Points.Add(new SeriesPoint("Chelsea", 14));
            //series1.Points.Add(new SeriesPoint("Dortmund", 17));

            // Barda ki ikinci sütun
            Series series2 = new Series("Yediği Gol", ViewType.Bar);
            series2.Points.Add(new SeriesPoint("AC Milan", 15));
            series2.Points.Add(new SeriesPoint("Barcelona", 18));
            //series2.Points.Add(new SeriesPoint("Chelsea", 25));
            //series2.Points.Add(new SeriesPoint("Dortmund", 33));

            Series series3 = new Series("Oyandığı Maç Sayısı", ViewType.Bar);
            series3.Points.Add(new SeriesPoint("AC Milan", 10));
            series3.Points.Add(new SeriesPoint("Barcelona", 10));

            // Add the series to the chart.
            chartControl1.Series.Add(series1);
            chartControl1.Series.Add(series2);
            chartControl1.Series.Add(series3);


            // Açıklama gözüksün
            chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.True;

            // Yatay için = true yapmak lazım
            ((XYDiagram)chartControl1.Diagram).Rotated = false;

            // Chart için başlık yazabilirim
            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = "Karşılaştırma Atılan Gol";
            chartControl1.Titles.Add(chartTitle1);

            // Add the chart to the form.
            //chartControl1.Dock = DockStyle.Fill;
            this.Controls.Add(chartControl1);
        }


        


    }
}
