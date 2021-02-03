using DXApplication2.Web;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace TahminManager
{
    class UpdateDataBase
    {
        private readonly string LeagueName;
        private readonly string LeagueDir;
        public MatchInfo matchInfo;


        public UpdateDataBase(string leagueName, string leagueDir)
        {
            this.LeagueName = leagueName;
            this.LeagueDir = leagueDir;
        }

        public string HtmlContent;
        public void LoadFolder(Action<string> VeriTabaniWriter = null)
        {
            DirectoryInfo directory = new DirectoryInfo(LeagueDir);
            FileInfo[] fileInfos = directory.GetFiles();
            //klasör altındaki dosyaları fileInfos dizisine ekledi

            DAL dal = new DAL();
            List<int> originalMatchIDs = dal.GetOriginalMatchIDs();
            List<int> originalPlayerStatisticsMatchIDs = dal.GetOriginalPlayerStatisticsMatchIDs();
            List<int> originalTeamStatisticsMatchIDs = dal.GetOriginalTeamStatisticsMatchIDs();
            //veritabanında kayıtlı olanları originalMatchIDs listesine aldı

            if (fileInfos.Length > 0)
            {
                Console.WriteLine(LeagueName + ": yükleniyor...");

                foreach (FileInfo file in fileInfos)
                {
                    if (file.Length < Globe.IncorrectFileSize)
                        continue;
                    //dosya uzunluğu 5 Mb altındaysa atla

                    string fileName = Path.GetFileNameWithoutExtension(file.FullName);
                    //string fileName = "865781";
                    if (fileName.Contains("_Teams"))
                        continue;

                    if (fileName.Equals("LiveScores") || originalMatchIDs.Contains(int.Parse(fileName)) || originalPlayerStatisticsMatchIDs.Contains(int.Parse(fileName)) || originalTeamStatisticsMatchIDs.Contains(int.Parse(fileName)))
                        continue;

                    //Uzantısız Dosya adı Livescores olan ya da veri tabanında kayıtlı olan dosya isimleri varsa atla

                    try
                    {
                        HtmlContent = Globe.LoadFile(file.FullName);
                        //string htmlContent = Globe.LoadFile("C:\\WhoScored-master\\htmlContent\\Italy-Serie-A\\865781.txt");

                    }
                    catch (Exception ex)
                    {
                        Globe.WriteLog(fileName + " : Kaynak çekilemeyen dosya!!! " + ex);
                    }

                    //dosya kaynağını htmlContent'e ata
                    ContentFilter filter = new ContentFilter();

                    try
                    {
                        matchInfo = filter.MacBilgisiOlusturYontem1(int.Parse(fileName), LeagueName, HtmlContent);
                        if (matchInfo.awayid == 0)
                        {
                            matchInfo = filter.MacBilgisiOlusturYontem2(int.Parse(fileName), LeagueName, HtmlContent);

                        }
                    }
                    catch (Exception)
                    {

                        Globe.WriteLog("Hatalı kaynak kodu!!!");
                    }
                    //MatchInfo matchInfo = filter.GetMatchInfo(int.Parse(fileName), LeagueName, HtmlContent);
                    //maç idsi,lig adı ve kaynağa göre maç bilgisini çek.

                    try
                    {
                        InsertData(matchInfo);
                        VeriTabaniWriter?.Invoke(LeagueName + " - " + int.Parse(fileName));


                    }
                    catch (Exception ex)
                    {
                        Globe.WriteLog("Veritabanına yazılamayan maç. Kaynağı kontrol et: " + file.FullName + " " + ex.Message);
                    }
                }
            }
        }
        public void LoadFolder2()
        {
            DirectoryInfo directory = new DirectoryInfo(LeagueDir);
            FileInfo[] fileInfos = directory.GetFiles();
            //klasör altındaki dosyaları fileInfos dizisine ekledi

            DAL dal = new DAL();
            List<int> originalMatchIDs = dal.GetOriginalMatchIDs();
            List<int> originalPlayerStatisticsMatchIDs = dal.GetOriginalPlayerStatisticsMatchIDs();
            List<int> originalTeamStatisticsMatchIDs = dal.GetOriginalTeamStatisticsMatchIDs();

            //veritabanında kayıtlı olanları originalMatchIDs listelerine aldı

            if (fileInfos.Length > 0)
            {
                Console.WriteLine(LeagueName + ": yükleniyor...");

                foreach (FileInfo file in fileInfos)
                {
                    if (file.Length < Globe.IncorrectFileSize)
                        continue;
                    //dosya uzunluğu 5 Mb altındaysa atla

                    string fileName = Path.GetFileNameWithoutExtension(file.FullName);
                    //string fileName = "865781";
                    if (fileName.Contains("_Teams"))
                        continue;

                    if (fileName.Equals("LiveScores") || originalMatchIDs.Contains(int.Parse(fileName)) || originalPlayerStatisticsMatchIDs.Contains(int.Parse(fileName)) || originalTeamStatisticsMatchIDs.Contains(int.Parse(fileName)))
                        continue;

                    //Uzantısız Dosya adı Livescores olan ya da veri tabanında kayıtlı olan dosya isimleri varsa atla

                    try
                    {
                        HtmlContent = Globe.LoadFile(file.FullName);
                        //string htmlContent = Globe.LoadFile("C:\\WhoScored-master\\htmlContent\\Italy-Serie-A\\865781.txt");

                    }
                    catch (Exception ex)
                    {
                        Globe.WriteLog(fileName + " : Kaynak çekilemeyen dosya!!! " + ex);
                    }

                    //dosya kaynağını htmlContent'e ata
                    ContentFilter filter = new ContentFilter();


                    MatchInfo matchInfo = filter.GetMatchInfo(int.Parse(fileName), LeagueName, HtmlContent);
                    //maç idsi,lig adı ve kaynağa göre maç bilgisini çek.

                    try
                    {
                        InsertData2(matchInfo);
                        Globe.WriteLog(file.FullName + " Başarılı bir şekilde veritabanına yazıldı");
                    }
                    catch (Exception ex)
                    {
                        Globe.WriteLog("Veritabanına yazılamayan maç. Kaynağı kontrol et: " + file.FullName + " " + ex.Message);
                    }
                }
            }
        }
        private void InsertData(MatchInfo matchInfo)
        {
            // Tüm bilgileri dolduruyor.

            DAL dal = new DAL();
            dal.InsertMatchInformation(matchInfo);

            try
            {
                if (matchInfo.HomeTeamStatistics.rating > 0 || matchInfo.AwayTeamStatistics.rating > 0 || !matchInfo.HomeTeamStatistics.rating.Equals(null) || !matchInfo.AwayTeamStatistics.rating.Equals(null))
                {
                    dal.InsertTeamStatistics(matchInfo.HomeTeamStatistics, matchInfo.League, matchInfo.id, true);
                    dal.InsertTeamStatistics(matchInfo.AwayTeamStatistics, matchInfo.League, matchInfo.id, false);
                }

            }
            catch (Exception ex)
            {
                Globe.WriteLog(string.Empty + matchInfo.id + " : Takım istatistikleri olmadığından yazılamadı.\n HATA : " + ex);
            }

            try
            {

                if (matchInfo.HomeTeamPlayerStatistics[0] != null)
                {
                    foreach (PlayerStatistics player in matchInfo.HomeTeamPlayerStatistics)
                    {
                        dal.InsertPlayerStatistics(player, matchInfo.HomeTeamStatistics.id, matchInfo.HomeTeamStatistics.name, matchInfo.League, matchInfo.id, true);
                    }

                    foreach (PlayerStatistics player in matchInfo.AwayTeamPlayerStatistics)
                    {
                        dal.InsertPlayerStatistics(player, matchInfo.AwayTeamStatistics.id, matchInfo.AwayTeamStatistics.name, matchInfo.League, matchInfo.id, false);
                    }

                }
            }

            catch (Exception ex)
            {
                Globe.WriteLog(string.Empty + matchInfo.id + " : Oyuncu istatistikleri olmadığından yazılamadı.\n HATA : " + ex);
            }


        }

        private void InsertData2(MatchInfo matchInfo)
        {

            //Maç bilgileri haricinde takım ve oyunucu istatistiklerini dolduruyor.  
            DAL dal = new DAL();

            if (matchInfo.League == "England_BarclaysPL" || matchInfo.League == "Germany-Bundesliga" || matchInfo.League == "Brazil_LigaDoBrasil" || matchInfo.League == "Italy-Serie-A" || matchInfo.League == "Spain-La-Liga" || matchInfo.League == "League 1" || matchInfo.League == "Netherlands-Eredivisie" || matchInfo.League == "Russia-Premier-League" || matchInfo.League == "Major League Soccer" || matchInfo.League == "Turkey-Super-Lig" || matchInfo.League == "Championship" || matchInfo.League == "Europe-UEFA-Champions-League" || matchInfo.League == "UEFA Europa League")
            {
                if (matchInfo.HomeTeamStatistics.rating > 0 || matchInfo.AwayTeamStatistics.rating > 0)
                {
                    dal.InsertTeamStatistics(matchInfo.HomeTeamStatistics, matchInfo.League, matchInfo.id, true);
                    dal.InsertTeamStatistics(matchInfo.AwayTeamStatistics, matchInfo.League, matchInfo.id, false);
                }

                if (matchInfo.HomeTeamPlayerStatistics[0] != null)
                {
                    foreach (PlayerStatistics player in matchInfo.HomeTeamPlayerStatistics)
                    {
                        dal.InsertPlayerStatistics(player, matchInfo.HomeTeamStatistics.id, matchInfo.HomeTeamStatistics.name, matchInfo.League, matchInfo.id, true);
                    }

                    foreach (PlayerStatistics player in matchInfo.AwayTeamPlayerStatistics)
                    {
                        dal.InsertPlayerStatistics(player, matchInfo.AwayTeamStatistics.id, matchInfo.AwayTeamStatistics.name, matchInfo.League, matchInfo.id, false);
                    }
                }
            }
            else
            {
                Globe.WriteLog(matchInfo.League + " istatistik çekilecek bir lig değil!!!");
            }
        }
        //public SqlConnection baglantiLocalhost = new SqlConnection("server=psl.dynu.com;database=futbol;user=selsor;password=123456;pooling=false");//trusted_connection=true

        public SqlConnection baglantiLocalhost =
               new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");


    }
}
