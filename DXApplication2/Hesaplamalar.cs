using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXApplication2
{
    class Hesaplamalar
    {
        //SqlConnection baglantiLocalhost = new SqlConnection("server=localhost;database=futbol;user=sa;password=123456");
        //public SqlConnection baglantiLocalhost = new SqlConnection("server=psl.dynu.com;database=futbol;user=selsor;password=123456;pooling=false");//trusted_connection=true
        public SqlConnection baglantiLocalhost =
            new SqlConnection(
                //"server=localhost\\MSSQLSERVER01;database=FootballStatistics;user=sa;password=123456;pooling=false");
                "server=194.31.59.132;database=FootballStatistics;user=sa;password=Buse2015;pooling=false");

        public string StageFK;

        public string StageFKBul(string macId)
        {
            if (baglantiLocalhost.State == ConnectionState.Closed)
                baglantiLocalhost.Open();
            string SezonBasi = "2017-04-01";

            SqlCommand StageFKBul =
                new SqlCommand(
                    "SELECT * FROM api.Lig WHERE TournamentName = '" + macId + "' AND endDate >'" + SezonBasi + "' ",
                    baglantiLocalhost);
            SqlDataReader StageFKBulReader = StageFKBul.ExecuteReader();
            while (StageFKBulReader.Read())
            {
                StageFK = StageFKBulReader[0].ToString();
            }

            baglantiLocalhost.Close();
            return StageFK;
        }

        public double EvsahibiGenelAttıgiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            baglantiLocalhost.Open();


            SqlCommand EvsahibiGenelAttıgiGolSayisi =
                new SqlCommand(
                    "SELECT OMS,AG FROM  [dbo].[TakimByGenelPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId + "')",
                    baglantiLocalhost);
            SqlDataReader EvsahibiGenelAttıgiGolSayisiReader = EvsahibiGenelAttıgiGolSayisi.ExecuteReader();
            while (EvsahibiGenelAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(EvsahibiGenelAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiGenelAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiGenelYedigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiGenelYedigiGolSayisi =
                new SqlCommand(
                    "SELECT OMS,YG FROM  [dbo].[TakimByGenelPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId + "')",
                    baglantiLocalhost);
            SqlDataReader EvsahibiGenelYedigiGolSayisiReader = EvsahibiGenelYedigiGolSayisi.ExecuteReader();
            while (EvsahibiGenelYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiGenelYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiGenelYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIcSahaAttigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIcSahaAttigiGolSayisi = new SqlCommand(
                "SELECT OMS,AG FROM [dbo].[TakimByIcSahaPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId + "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIcSahaAttigiGolSayisiReader = EvsahibiIcSahaAttigiGolSayisi.ExecuteReader();
            while (EvsahibiIcSahaAttigiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(EvsahibiIcSahaAttigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIcSahaAttigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIcSahaYedigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIcSahaYedigiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByIcSahaPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId + "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIcSahaYedigiGolSayisiReader = EvsahibiIcSahaYedigiGolSayisi.ExecuteReader();
            while (EvsahibiIcSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiIcSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIcSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiDisSahaAttıgiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiDisSahaAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG  from [dbo].[TakimByDisSahaPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId + "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiDisSahaAttıgiGolSayisiReader = EvsahibiDisSahaAttıgiGolSayisi.ExecuteReader();
            while (EvsahibiDisSahaAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(EvsahibiDisSahaAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiDisSahaAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiDisSahaYedigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiDisSahaAttıgiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByDisSahaPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId + "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiDisSahaAttıgiGolSayisiReader = EvsahibiDisSahaAttıgiGolSayisi.ExecuteReader();
            while (EvsahibiDisSahaAttıgiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiDisSahaAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiDisSahaAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIlkYariAttıgiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIlkYariAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from [dbo].TakimByDevre1PuanDurumu  ('" + liginAdi + "','" + evsahibiTakimId + "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIlkYariAttıgiGolSayisiReader = EvsahibiIlkYariAttıgiGolSayisi.ExecuteReader();
            while (EvsahibiIlkYariAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(EvsahibiIlkYariAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIlkYariAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIlkYariIcSahaAttıgiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIlkYariIcSahaAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from [dbo].TakimByDevre1IcSahaPuanDurumu  ('" + liginAdi + "','" + evsahibiTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIlkYariIcSahaAttıgiGolSayisiReader =
                EvsahibiIlkYariIcSahaAttıgiGolSayisi.ExecuteReader();
            while (EvsahibiIlkYariIcSahaAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(EvsahibiIlkYariIcSahaAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIlkYariIcSahaAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIlkYariDisSahaAttıgiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIlkYariDisSahaAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from [dbo].TakimByDevre1DisSahaPuanDurumu  ('" + liginAdi + "','" + evsahibiTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIlkYariDisAttıgiGolSayisiReader =
                EvsahibiIlkYariDisSahaAttıgiGolSayisi.ExecuteReader();
            while (EvsahibiIlkYariDisAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(EvsahibiIlkYariDisAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIlkYariDisAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIlkYariYedigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIlkYariYedigiGolSayisi = new SqlCommand(
                "Select OMS,YG from [dbo].TakimByDevre1PuanDurumu  ('" + liginAdi + "','" + evsahibiTakimId + "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIlkYariYedigiGolSayisiReader = EvsahibiIlkYariYedigiGolSayisi.ExecuteReader();
            while (EvsahibiIlkYariYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiIlkYariYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIlkYariYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIkinciYariAttıgiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIkinciYariAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from [dbo].TakimByDevre2PuanDurumu  ('" + liginAdi + "','" + evsahibiTakimId + "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIkinciYariAttıgiGolSayisiReader = EvsahibiIkinciYariAttıgiGolSayisi.ExecuteReader();
            while (EvsahibiIkinciYariAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(EvsahibiIkinciYariAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIkinciYariAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIkinciYariYedigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIkinciYariYedigiGolSayisi = new SqlCommand(
                "Select  OMS,YG  from  [dbo].TakimByDevre2PuanDurumu ('" + liginAdi + "','" + evsahibiTakimId + "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIkinciYariYedigiGolSayisiReader = EvsahibiIkinciYariYedigiGolSayisi.ExecuteReader();
            while (EvsahibiIkinciYariYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiIkinciYariYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIkinciYariYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirGenelAttigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirGenelAttigiGolSayisi =
                new SqlCommand(
                    "Select OMS,AG from [dbo].TakimByGenelPuanDurumu ('" + liginAdi + "','" + misafirTakimId + "')",
                    baglantiLocalhost);
            SqlDataReader MisafirGenelAttigiGolSayisiReader = MisafirGenelAttigiGolSayisi.ExecuteReader();
            while (MisafirGenelAttigiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(MisafirGenelAttigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirGenelAttigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirGenelYedigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double YedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirGenelYedigiGolSayisi =
                new SqlCommand(
                    "Select  OMS,YG  from [dbo].TakimByGenelPuanDurumu ('" + liginAdi + "','" + misafirTakimId + "')",
                    baglantiLocalhost);
            SqlDataReader MisafirGenelYedigiGolSayisiReader = MisafirGenelYedigiGolSayisi.ExecuteReader();
            while (MisafirGenelYedigiGolSayisiReader.Read())
            {
                YedigiGol = Convert.ToDouble(MisafirGenelYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirGenelYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = YedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirDisSahaAttigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirDisSahaAttigiGolSayisi = new SqlCommand(
                "Select OMS,AG from [dbo].TakimByDisSahaPuanDurumu ('" + liginAdi + "','" + misafirTakimId + "')",
                baglantiLocalhost);
            SqlDataReader MisafirDisSahaAttigiGolSayisiReader = MisafirDisSahaAttigiGolSayisi.ExecuteReader();
            while (MisafirDisSahaAttigiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(MisafirDisSahaAttigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirDisSahaAttigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirDisSahaYedigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirDisSahaYedigiGolSayisi = new SqlCommand(
                "Select  OMS,YG  from [dbo].TakimByDisSahaPuanDurumu ('" + liginAdi + "','" + misafirTakimId + "')",
                baglantiLocalhost);
            SqlDataReader MisafirDisSahaYedigiGolSayisiReader = MisafirDisSahaYedigiGolSayisi.ExecuteReader();
            while (MisafirDisSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(MisafirDisSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirDisSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIcSahaAttigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIcSahaAttigiGolSayisi =
                new SqlCommand(
                    "Select OMS,AG from [dbo].[TakimByIcSahaPuanDurumu] ('" + liginAdi + "','" + misafirTakimId + "')",
                    baglantiLocalhost);
            SqlDataReader MisafirIcSahaAttigiGolSayisiReader = MisafirIcSahaAttigiGolSayisi.ExecuteReader();
            while (MisafirIcSahaAttigiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(MisafirIcSahaAttigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirIcSahaAttigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIcSahaYedigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIcSahaYedigiGolSayisi =
                new SqlCommand(
                    "Select  OMS,YG  from [dbo].[TakimByIcSahaPuanDurumu]('" + liginAdi + "','" + misafirTakimId + "')",
                    baglantiLocalhost);
            SqlDataReader MisafirIcSahaYedigiGolSayisiReader = MisafirIcSahaYedigiGolSayisi.ExecuteReader();
            while (MisafirIcSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(MisafirIcSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirIcSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIlkYariAttıgiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIlkYariAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from dbo.TakimByDevre1PuanDurumu ('" + liginAdi + "','" + misafirTakimId + "')",
                baglantiLocalhost);
            SqlDataReader MisafirIlkYariAttıgiGolSayisiReader = MisafirIlkYariAttıgiGolSayisi.ExecuteReader();
            while (MisafirIlkYariAttıgiGolSayisiReader.Read())
            {
                if (MisafirIlkYariAttıgiGolSayisiReader[1] != null)
                    attigiGol = Convert.ToDouble(MisafirIlkYariAttıgiGolSayisiReader[1].ToString());
                else
                {
                    attigiGol = 0;
                }

                oynadigiMac = Convert.ToDouble(MisafirIlkYariAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIlkYariIcSahaAttıgiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIlkYariAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from dbo.TakimByDevre1IcSahaPuanDurumu ('" + liginAdi + "','" + misafirTakimId + "')",
                baglantiLocalhost);
            SqlDataReader MisafirIlkYariAttıgiGolSayisiReader = MisafirIlkYariAttıgiGolSayisi.ExecuteReader();
            while (MisafirIlkYariAttıgiGolSayisiReader.Read())
            {
                if (MisafirIlkYariAttıgiGolSayisiReader[1] != null)
                    attigiGol = Convert.ToDouble(MisafirIlkYariAttıgiGolSayisiReader[1].ToString());
                else
                {
                    attigiGol = 0;
                }

                oynadigiMac = Convert.ToDouble(MisafirIlkYariAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }


        public double MisafirIlkYariYedigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;

            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIlkYariYedigiGolSayisi = new SqlCommand(
                "Select  OMS,YG  from dbo.TakimByDevre1PuanDurumu ('" + liginAdi + "','" + misafirTakimId + "')",
                baglantiLocalhost);
            SqlDataReader MisafirIlkYariYedigiGolSayisiReader = MisafirIlkYariYedigiGolSayisi.ExecuteReader();
            while (MisafirIlkYariYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(MisafirIlkYariYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirIlkYariYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIkinciYariAttıgiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIkinciYariAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG  from [dbo].TakimByDevre2PuanDurumu ('" + liginAdi + "','" + misafirTakimId + "')",
                baglantiLocalhost);
            SqlDataReader MisafirIkinciYariAttıgiGolSayisiReader = MisafirIkinciYariAttıgiGolSayisi.ExecuteReader();
            while (MisafirIkinciYariAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(MisafirIkinciYariAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirIkinciYariAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIkinciYariYedigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIkinciYariYedigiGolSayisi = new SqlCommand(
                "Select  OMS,YG  from [dbo].TakimByDevre2PuanDurumu ('" + liginAdi + "','" + misafirTakimId + "')",
                baglantiLocalhost);
            SqlDataReader MisafirIkinciYariYedigiGolSayisiReader = MisafirIkinciYariYedigiGolSayisi.ExecuteReader();
            while (MisafirIkinciYariYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(MisafirIkinciYariYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirIkinciYariYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiMacSonucuAttigiGolTahmini, MisafirMacSonucuAttigiGolTahmini;

        public double EvsahibiIlkYariIcSahaYedigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIlkYariIcSahaYedigiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByDevre1IcSahaPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIlkYariIcSahaYedigiGolSayisiReader =
                EvsahibiIlkYariIcSahaYedigiGolSayisi.ExecuteReader();
            while (EvsahibiIlkYariIcSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiIlkYariIcSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIlkYariIcSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIlkYariDisSahaYedigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiDisSahaAttıgiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByDevre1DisSahaPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiDisSahaAttıgiGolSayisiReader = EvsahibiDisSahaAttıgiGolSayisi.ExecuteReader();
            while (EvsahibiDisSahaAttıgiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiDisSahaAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiDisSahaAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIkinciYariIcSahaAttigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand Evsahibi2YariIcSahaAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from [dbo].TakimByDevre2IcSahaPuanDurumu  ('" + liginAdi + "','" + evsahibiTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader Evsahibi2YariIcSahaAttıgiGolSayisiReader = Evsahibi2YariIcSahaAttıgiGolSayisi.ExecuteReader();
            while (Evsahibi2YariIcSahaAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(Evsahibi2YariIcSahaAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(Evsahibi2YariIcSahaAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIkinciYariDisSahaAttigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand Evsahibi2YariDisSahaAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from [dbo].TakimByDevre2DisSahaPuanDurumu  ('" + liginAdi + "','" + evsahibiTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader Evsahibi2YariDisSahaAttıgiGolSayisiReader =
                Evsahibi2YariDisSahaAttıgiGolSayisi.ExecuteReader();
            while (Evsahibi2YariDisSahaAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(Evsahibi2YariDisSahaAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(Evsahibi2YariDisSahaAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIkinciYariIcSahaYedigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIlkYariDisSahaYedigiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByDevre2IcSahaPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIlkYariDisSahaYedigiGolSayisiReader =
                EvsahibiIlkYariDisSahaYedigiGolSayisi.ExecuteReader();
            while (EvsahibiIlkYariDisSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiIlkYariDisSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIlkYariDisSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double EvsahibiIkinciYariDisSahaYedigiGolOrtalamasi(int evsahibiTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand Evsahibi2YariDisSahaYedigiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByDevre2DisSahaPuanDurumu] ('" + liginAdi + "','" + evsahibiTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader Evsahibi2YariDisSahaYedigiGolSayisiReader =
                Evsahibi2YariDisSahaYedigiGolSayisi.ExecuteReader();
            while (Evsahibi2YariDisSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(Evsahibi2YariDisSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(Evsahibi2YariDisSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIlkYariDisSahAttigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIlkYariIcSahaAttigiGolSayisi = new SqlCommand(
                "Select OMS,AG  from [dbo].[TakimByDevre1DisSahaPuanDurumu] ('" + liginAdi + "','" + misafirTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader MisafirIlkYariIcSahaAttigiGolSayisiReader =
                MisafirIlkYariIcSahaAttigiGolSayisi.ExecuteReader();
            while (MisafirIlkYariIcSahaAttigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(MisafirIlkYariIcSahaAttigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirIlkYariIcSahaAttigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIlkYariIcSahaYedigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIlkYariIcSahaYedigiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByDevre1IcSahaPuanDurumu] ('" + liginAdi + "','" + misafirTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIlkYariIcSahaYedigiGolSayisiReader =
                EvsahibiIlkYariIcSahaYedigiGolSayisi.ExecuteReader();
            while (EvsahibiIlkYariIcSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiIlkYariIcSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIlkYariIcSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIlkYariDisSahaYedigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand EvsahibiIlkYariIcSahaYedigiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByDevre1DisSahaPuanDurumu] ('" + liginAdi + "','" + misafirTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader EvsahibiIlkYariIcSahaYedigiGolSayisiReader =
                EvsahibiIlkYariIcSahaYedigiGolSayisi.ExecuteReader();
            while (EvsahibiIlkYariIcSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(EvsahibiIlkYariIcSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(EvsahibiIlkYariIcSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIkinciYariIcSahaAttigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand Evsahibi2YariIcSahaAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from [dbo].TakimByDevre2IcSahaPuanDurumu  ('" + liginAdi + "','" + misafirTakimId + "')",
                baglantiLocalhost);
            SqlDataReader Evsahibi2YariIcSahaAttıgiGolSayisiReader = Evsahibi2YariIcSahaAttıgiGolSayisi.ExecuteReader();
            while (Evsahibi2YariIcSahaAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(Evsahibi2YariIcSahaAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(Evsahibi2YariIcSahaAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIkinciYariDisSahaAttigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double attigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand Evsahibi2YariDisSahaAttıgiGolSayisi = new SqlCommand(
                "Select OMS,AG from [dbo].TakimByDevre2DisSahaPuanDurumu  ('" + liginAdi + "','" + misafirTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader evsahibi2YariDisSahaAttıgiGolSayisiReader =
                Evsahibi2YariDisSahaAttıgiGolSayisi.ExecuteReader();
            while (evsahibi2YariDisSahaAttıgiGolSayisiReader.Read())
            {
                attigiGol = Convert.ToDouble(evsahibi2YariDisSahaAttıgiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(evsahibi2YariDisSahaAttıgiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = attigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIkinciYariIcSahaYedigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIlkYariDisSahaYedigiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByDevre2IcSahaPuanDurumu] ('" + liginAdi + "','" + misafirTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader MisafirIlkYariDisSahaYedigiGolSayisiReader =
                MisafirIlkYariDisSahaYedigiGolSayisi.ExecuteReader();
            while (MisafirIlkYariDisSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(MisafirIlkYariDisSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirIlkYariDisSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }

        public double MisafirIkinciYariDisSahaYedigiGolOrtalamasi(int misafirTakimId, string liginAdi)
        {
            double yedigiGol = 0;
            double oynadigiMac = 0;
            double ortalama = 0;
            //string StageFK = StageFKBul(liginAdi);
            baglantiLocalhost.Open();

            SqlCommand MisafirIlkYariDisSahaYedigiGolSayisi = new SqlCommand(
                "Select OMS,YG  from [dbo].[TakimByDevre2DisSahaPuanDurumu] ('" + liginAdi + "','" + misafirTakimId +
                "')",
                baglantiLocalhost);
            SqlDataReader MisafirIlkYariDisSahaYedigiGolSayisiReader =
                MisafirIlkYariDisSahaYedigiGolSayisi.ExecuteReader();
            while (MisafirIlkYariDisSahaYedigiGolSayisiReader.Read())
            {
                yedigiGol = Convert.ToDouble(MisafirIlkYariDisSahaYedigiGolSayisiReader[1].ToString());
                oynadigiMac = Convert.ToDouble(MisafirIlkYariDisSahaYedigiGolSayisiReader[0].ToString());
            }

            baglantiLocalhost.Close();
            ortalama = yedigiGol / oynadigiMac;
            ortalama = Convert.ToDouble(String.Format("{0:0.##}", ortalama));
            return ortalama;
        }
    }
}