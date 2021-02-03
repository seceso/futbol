using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TahminManager;



namespace DXApplication2.Web
{
    class SpiderAsync
    {
        public void GetAllLeagues(Action<KeyValuePair<string, string>, int, int> writer = null)
        {
            if (Globe.LeaguesDic.Count > 0)
            {
                var total = Globe.LeaguesDic.Count;
                var current = 1;

                foreach (KeyValuePair<string, string> item in Globe.LeaguesDic)
                {
                    writer?.Invoke(item, total, current++);
                    GetLeague(item.Key, item.Value);
                }
            }
            Globe.WriteLog("Tüm Ligler İndirildi");
        }

        public void GetAllMatches(Action<int, int, string> maclarwriter = null,
            Action<string> macwriter = null)
        {
            if (Globe.LeaguesDic.Count > 0)
            {
                ContentFilter filter = new ContentFilter();


                var current = 1;

                foreach (KeyValuePair<string, string> item in Globe.LeaguesDic)
                {
                    string directory = Globe.RootDir + "ligler\\" + item.Key + "\\";
                    //Her lig için dizin tanımlar
                    string fileName = directory + Globe.LeagueFileName;
                    //livescores.txt yolunu tanımlar
                    Globe.WriteLog("Kontrol edilen lig : " + fileName);
                    string htmlContent = Globe.LoadFile(fileName);
                    //html content=livescores.txt içindeki kaynak kod


                    List<int> originalMatchIDs = GetOriginalMatchIDs(directory);
                    //foreach (int i in originalMatchIDs)
                    //{
                    //    Globe.WriteLog("Bu ne originalMatchIDs : " + i);
                    //}

                    List<int> matchIDs = filter.GetMatchIDs(htmlContent, originalMatchIDs);
                    var total = matchIDs.Count;
                    //foreach (int i in originalMatchIDs)//{
                    //    Globe.WriteLog("Peki bu ne matchIDs : " + i);
                    //}
                    maclarwriter?.Invoke(total, current++, fileName);
                    GetMatches(item.Key, matchIDs, macwriter);
                    Globe.WriteLog("Tüm maçlar indirildi");
                }
            }
        }


        public string FiksturConfig()
        {
            baglantiLocalhost.Open();
            SqlCommand takimBilgileriCek = new SqlCommand(@"select distinct cast(home_team_id as nvarchar(8))+','+ home_team_name+',' from(select distinct tt.Ad home_team_name,home_team_id from matchinformation2 m inner join Tanimlar.Takimlar tt on tt.whoScoredId=m.home_team_id) TEMP", baglantiLocalhost);
            SqlDataReader takimBilgileriCekReader = takimBilgileriCek.ExecuteReader();
            string bilgiString = string.Empty;
            while (takimBilgileriCekReader.Read())
            {
                bilgiString += takimBilgileriCekReader[0].ToString();
            }

            baglantiLocalhost.Close();
            return bilgiString;
        }
        public string DosyayiokuStringDondur(string filePath)
        {
            using (StreamReader streamreader = new StreamReader(filePath))
            {
                string text = streamreader.ReadToEnd();
                streamreader.Close();
                return text;
            }
        }
        public string hatali, hatali2;
        public string ligadi;
        public void GetAllFixtures(Action<int, int, string> fiksturwriter = null)
        {
            Globe.FixturesDic.Clear();
            string bilgiString = FiksturConfig();
            //MessageBox.Show(bilgiString);

            string[] temp = bilgiString.Split(',');
            try
            {
                for (int i = 0; i < temp.Length - 1; i += 2)
                {
                    Globe.FixturesDic.Add(temp[i].Trim(), temp[i + 1].Trim());

                    hatali = temp[i].ToString();
                    hatali2 = temp[i + 1].ToString();
                    //Ayarlardaki fixture ismi ve klasör yollarını çekiyor.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(hatali + " - " + hatali2 + " Hatalı : " + ex);
            }

            if (Globe.FixturesDic.Count > 0)
            {
                var total = Globe.FixturesDic.Count;

                foreach (KeyValuePair<string, string> item in Globe.FixturesDic)
                {
                    try
                    {

                        GetFikstur(item.Key, item.Value, fiksturwriter);
                        ligadi = item.Key + " - " + item.Value;
                    }
                    catch (Exception ex)
                    {
                        Globe.WriteLog(item.Key + "- " + item.Value + " Fikstür bilgisi çekilemedi : " + ex);
                    }
                }
            }
        }
        public void GetAllFixtures2(Action<int, int, string> FiksturWriter = null,
            Action<string> macwriter = null)
        {
            Globe.FixturesDic.Clear();
            string bilgiString = FiksturConfig();
            //MessageBox.Show(bilgiString);

            string[] temp = bilgiString.Split(',');
            try
            {
                for (int i = 0; i < temp.Length - 1; i += 2)
                {
                    Globe.FixturesDic.Add(temp[i].Trim(), temp[i + 1].Trim());

                    hatali = temp[i].ToString();
                    hatali2 = temp[i + 1].ToString();
                    //Ayarlardaki fixture ismi ve klasör yollarını çekiyor.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(hatali + " - " + hatali2 + " Hatalı : " + ex);
            }

            if (Globe.FixturesDic.Count > 0)
            {
                var total = Globe.FixturesDic.Count;
                var current = 1;
                foreach (KeyValuePair<string, string> item in Globe.FixturesDic)
                {
                    try
                    {

                        GuncelleFikstur(item.Key, item.Value, macwriter);
                        ligadi = item.Key + " - " + item.Value;
                        FiksturWriter?.Invoke(total, current++, ligadi);
                    }
                    catch (Exception ex)
                    {
                        Globe.WriteLog(item.Key + "- " + item.Value + " Fikstür bilgisi çekilemedi : " + ex);
                    }
                }
            }
        }


        public string eklenen = string.Empty;
        public void GetFikstur(string id, string teamName, Action<int, int, string> fiksturwriter = null)
        {

            string dir = Globe.RootDir + "TAKIMLAR\\" + teamName + "\\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string url = Globe.WhoScoredFixturesUrl + id + "/Fixtures/";
            string fileName = dir + id + ".txt";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }


            if (!File.Exists(fileName))
            {
                //Task<string> getHtmlContentByUrl = GetHtmlContentByUrl(url);
                //string htmlContent = await GetHtmlContentByUrl(url);
                string htmlContent = BilgiCek(url);

                if (htmlContent != null && htmlContent != string.Empty)
                {

                    SaveContent(fileName, htmlContent);
                    fiksturwriter?.Invoke(1, 1, fileName);
                    //TXT dosyası oluşturuldu.
                    Globe.WriteLog(teamName + " " + fileName + " kaynağı başarıyla oluşturuldu!!");
                    FikstureMacDoldur(fileName, fiksturwriter);
                }
                else
                {

                    Globe.WriteLog(teamName + " kaynağı oluşturulamadı!!");
                }
            }


        }

        public void GuncelleFikstur(string id, string teamName, Action<string> macwriter = null)
        {

            string dir = Globe.RootDir + "TAKIMLAR\\" + teamName + "\\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string url = Globe.WhoScoredFixturesUrl + id + "/Fixtures/";
            string fileName = dir + id + ".txt";

            try
            {

                string s = DosyayiokuStringDondur(fileName);
                int index = s.IndexOf("DataStore.prime('teamfixtures', $.extend({ teamId: ");
                int index2 = s.IndexOf("var teamFixtures");
                string y = s.Substring(index + 76, index2 - index);

                string g = y.Replace("\r\n,", string.Empty);
                string[] z = g.Split(new Char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                //string x = Convert.ToString(File.GetCreationTime(fileName));
                //int eskidosyauzunluk = y.Length;

                if (File.Exists(fileName))
                {
                    Thread.Sleep(3000);

                    string htmlContent2 = BilgiCek(url);
                    int index3 = htmlContent2.IndexOf("DataStore.prime('teamfixtures', $.extend({ teamId: ");
                    int index4 = htmlContent2.IndexOf("var teamFixtures");
                    string q = s.Substring(index3 + 76, index4 - index3);

                    string h = q.Replace("\r\n,", string.Empty);
                    string[] o = h.Split(new Char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);


                    bool d = z.SequenceEqual(o);
                    if (d == true)
                    {
                        Globe.WriteLog("Fikstür bilgilerinde bir değişiklik yok!");
                    }
                    else
                    {
                        File.Delete(fileName);
                        if (!String.IsNullOrEmpty(htmlContent2))
                        {
                            SaveContent(fileName, htmlContent2);
                            FikstureMacDoldur2(fileName);

                        }
                    }



                }
            }
            catch (Exception ex)
            {
                Globe.WriteLog("İstatistikten dolayı yanlış dosya numarası : " + ex);
                return;
            }

            //if (!File.Exists(fileName))
            //{
            //    //Task<string> getHtmlContentByUrl = GetHtmlContentByUrl(url);
            //    //string htmlContent = await GetHtmlContentByUrl(url);
            //    string htmlContent = KaynakKodunuCek(url);

            //    if (htmlContent != null && htmlContent != "")
            //    {

            //        SaveContent(fileName, htmlContent);
            //        //TXT dosyası oluşturuldu.
            //        Globe.WriteLog(teamName + " " + fileName + " kaynağı başarıyla oluşturuldu!!");
            //        FikstureMacDoldur(fileName);
            //    }
            //    else
            //    {

            //        Globe.WriteLog(teamName + " kaynağı oluşturulamadı!!");
            //    }
            //}


        }
        protected internal void FikstureMacDoldur(string fileName, Action<int, int, string> macwriter = null)
        {

            string s;
            s = DosyayiokuStringDondur(fileName);

            int index = s.IndexOf("DataStore.prime('teamfixtures', $.extend({ teamId: ");
            int index2 = s.IndexOf("var teamFixtures");
            string y = s.Substring(index + 76, index2 - index);

            string g = y.Replace("\r\n,", string.Empty);
            string[] z = g.Split(new Char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {


                foreach (string i in z)
                {
                    string l = i.Replace("]", string.Empty);
                    string m = l.Replace("\'", string.Empty);
                    //MessageBox.Show(l);
                    string[] kl = m.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (kl.Length == 7)
                    {
                        Globe.WriteLog("Hatalı kayıt");
                    }
                    else
                    {


                        try
                        {


                            //string[] deneme = fileName.Split('\\');
                            //string[] deneme2 = deneme[3].Split('.');
                            //int matchid = Convert.ToInt32(deneme2[0]);
                            int matchid = Convert.ToInt32(kl[0].ToString());
                            DAL dal = new DAL();
                            List<int> originalMatchIDs = dal.GetOriginalFixtureMatchIDs();
                            if (!originalMatchIDs.Contains(matchid))
                            {


                                if (kl.Length == 28)
                                {
                                    //int matchid = Convert.ToInt32(kl[0].ToString());
                                    string unknown = kl[1].ToString();
                                    string macTarihi = kl[2].ToString();
                                    string[] macTarihiParcala = macTarihi.Split('-');
                                    string gun = macTarihiParcala[0];
                                    string ay = macTarihiParcala[1];
                                    string yil = macTarihiParcala[2];
                                    string zaman = kl[3].ToString();
                                    string[] zamanparcala = zaman.Split(':');
                                    string saat = zamanparcala[0];
                                    string dakika = zamanparcala[1];
                                    string saniye = "00";
                                    int saat1;
                                    saat1 = (Convert.ToInt32(saat) + 2);
                                    saat = Convert.ToString(saat1);
                                    //MessageBox.Show(saat +" <-> "+saat1);
                                    macTarihi = yil + "/" + ay + "/" + gun + " " + saat + ":" + dakika + ":" + saniye;
                                    int homeTeamid = Convert.ToInt32(kl[4].ToString());
                                    string homeTeamName = kl[5].ToString();
                                    string unknown2 = kl[6].ToString();
                                    int awayTeamid = Convert.ToInt32(kl[7].ToString());
                                    string awayTeamName = kl[8].ToString();
                                    string unknown3 = kl[9].ToString();
                                    string FTResult = kl[10].ToString();
                                    string HTResult = kl[11].ToString();

                                    int? FTHomeGoals, FTAwayGoals;
                                    if (FTResult != "vs")
                                    {
                                        string[] FTGoals = kl[10].ToString().Split(':');
                                        string YildizSil1 = FTGoals[0].Replace("*", string.Empty);
                                        string YildizSil2 = FTGoals[1].Replace("*", string.Empty);

                                        FTHomeGoals = Convert.ToInt32(YildizSil1.TrimEnd());
                                        FTAwayGoals = Convert.ToInt32(YildizSil2.TrimStart());
                                    }
                                    else
                                    {
                                        FTHomeGoals = 0;
                                        FTAwayGoals = 0;
                                    }
                                    string[] HTGoals = kl[11].ToString().Split(':');
                                    int HTHomeGoals = Convert.ToInt32(HTGoals[0].TrimEnd());
                                    int HTAwaygoals = Convert.ToInt32(HTGoals[1].TrimStart());

                                    string unknown4 = kl[12].ToString();
                                    string unknown5 = kl[13].ToString();
                                    string State = kl[14].ToString();
                                    string Year = kl[15].ToString();
                                    string Tournament = kl[16].ToString();
                                    string Result = kl[17].ToString();
                                    string unknown7 = kl[18].ToString();
                                    string unknown8 = kl[19].ToString();
                                    string unknown9 = kl[20].ToString();
                                    string unknown10 = kl[21].ToString();
                                    string TournamentCode = kl[22].ToString();
                                    string nation = kl[23].ToString();
                                    string nation2 = kl[24].ToString();
                                    string unknown11 = kl[25].ToString();
                                    string unknown12 = kl[26].ToString();
                                    string unknown13 = kl[27].ToString();






                                    if (!originalMatchIDs.Contains(matchid))
                                    {
                                        if (Result == "1" || Result == "2")
                                        {
                                            string Durum = "2";
                                            baglantiLocalhost.Open();
                                            SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                            int sayi = fixtureKaydet.ExecuteNonQuery();
                                            if (sayi > 0)
                                                Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                            baglantiLocalhost.Close();
                                        }
                                        else
                                        {
                                            string Durum = "1";
                                            baglantiLocalhost.Open();
                                            SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                            int sayi = fixtureKaydet.ExecuteNonQuery();
                                            if (sayi > 0)
                                                Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                            baglantiLocalhost.Close();
                                        }

                                    }
                                    else
                                    {
                                        Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına daha önce eklenmiştir.");
                                    }

                                }

                                if (kl.Length == 26)
                                {
                                    //int matchid = Convert.ToInt32(kl[0].ToString());
                                    string unknown = kl[1].ToString();
                                    string macTarihi = kl[2].ToString();
                                    string[] macTarihiParcala = macTarihi.Split('-');
                                    string gun = macTarihiParcala[0];
                                    string ay = macTarihiParcala[1];
                                    string yil = macTarihiParcala[2];
                                    string zaman = kl[3].ToString();
                                    string[] zamanparcala = zaman.Split(':');
                                    string saat = zamanparcala[0];
                                    string dakika = zamanparcala[1];
                                    string saniye = "00";
                                    int saat1;
                                    saat1 = (Convert.ToInt32(saat) + 2);
                                    saat = Convert.ToString(saat1);
                                    macTarihi = yil + "/" + ay + "/" + gun + " " + saat + ":" + dakika + ":" + saniye;
                                    int homeTeamid = Convert.ToInt32(kl[4].ToString());
                                    string homeTeamName = kl[5].ToString();
                                    string unknown2 = kl[6].ToString();
                                    int awayTeamid = Convert.ToInt32(kl[7].ToString());
                                    string awayTeamName = kl[8].ToString();
                                    string unknown3 = kl[9].ToString();
                                    string FTResult = kl[10].ToString();
                                    int? FTHomeGoals, FTAwayGoals;
                                    if (FTResult != "vs")
                                    {
                                        string[] FTGoals = kl[10].ToString().Split(':');
                                        FTHomeGoals = Convert.ToInt32(FTGoals[0].TrimEnd());
                                        FTAwayGoals = Convert.ToInt32(FTGoals[1].TrimStart());
                                    }
                                    else
                                    {
                                        FTHomeGoals = 0;
                                        FTAwayGoals = 0;
                                    }
                                    //string[] HTGoals = kl[11].ToString().Split(':');
                                    string unknown4 = kl[11].ToString();
                                    string unknown5 = kl[12].ToString();
                                    string Year = kl[13].ToString();
                                    string Tournament = kl[14].ToString();
                                    string Result = kl[15].ToString();
                                    string unknown7 = kl[16].ToString();
                                    string unknown8 = kl[17].ToString();
                                    string unknown9 = kl[18].ToString();
                                    string unknown10 = kl[19].ToString();
                                    string TournamentCode = kl[20].ToString();
                                    string nation = kl[21].ToString();
                                    string nation2 = kl[22].ToString();
                                    string unknown11 = kl[23].ToString();
                                    string unknown12 = kl[24].ToString();
                                    string unknown13 = kl[25].ToString();


                                    if (Result == "1" || Result == "2")
                                    {
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();

                                        //if (baglantiLocalhost.State == ConnectionState.Closed)
                                        //    baglantiLocalhost.Open();
                                        String Durum = "2";
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);

                                        SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + Durum + ")", baglantiLocalhost);
                                        int sayi = fixtureKaydet.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }
                                    else
                                    {
                                        String Durum = "1";
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();

                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);

                                        SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + Durum + ")", baglantiLocalhost);
                                        int sayi = fixtureKaydet.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }


                                }

                                if (kl.Length != 26 && kl.Length != 28 && kl.Length != 1 && kl.Length != 2 && kl.Length == 27)
                                {
                                    //int matchid = Convert.ToInt32(kl[0].ToString());
                                    string unknown = kl[1].ToString();
                                    string macTarihi = kl[2].ToString();
                                    string[] macTarihiParcala = macTarihi.Split('-');
                                    string gun = macTarihiParcala[0];
                                    string ay = macTarihiParcala[1];
                                    string yil = macTarihiParcala[2];
                                    string zaman = kl[3].ToString();
                                    string[] zamanparcala = zaman.Split(':');
                                    string saat = zamanparcala[0];
                                    string dakika = zamanparcala[1];
                                    string saniye = "00";
                                    int saat1;
                                    saat1 = (Convert.ToInt32(saat) + 2);
                                    saat = Convert.ToString(saat1);
                                    macTarihi = yil + "/" + ay + "/" + gun + " " + saat + ":" + dakika + ":" + saniye;
                                    int homeTeamid = Convert.ToInt32(kl[4].ToString());
                                    string homeTeamName = kl[5].ToString();
                                    string unknown2 = kl[6].ToString();
                                    int awayTeamid = Convert.ToInt32(kl[7].ToString());
                                    string awayTeamName = kl[8].ToString();
                                    string unknown3 = kl[9].ToString();
                                    int? FTHomeGoals, FTAwayGoals;
                                    string FTResult = kl[10].ToString();

                                    if (FTResult != "vs")
                                    {
                                        string[] FTGoals = kl[10].ToString().Split(':');
                                        FTHomeGoals = Convert.ToInt32(FTGoals[0].TrimEnd());
                                        FTAwayGoals = Convert.ToInt32(FTGoals[1].TrimStart());
                                    }
                                    else
                                    {
                                        FTHomeGoals = 0;
                                        FTAwayGoals = 0;
                                    }
                                    //string[] HTGoals = kl[11].ToString().Split(':');
                                    string unknown4 = kl[11].ToString();
                                    string unknown5 = kl[12].ToString();
                                    string Year = kl[13].ToString();
                                    string State = kl[14].ToString();
                                    string Tournament = kl[15].ToString();
                                    string Result = kl[16].ToString();
                                    string unknown7 = kl[17].ToString();
                                    string unknown8 = kl[18].ToString();
                                    string unknown9 = kl[19].ToString();
                                    string unknown10 = kl[20].ToString();
                                    string TournamentCode = kl[21].ToString();
                                    string nation = kl[22].ToString();
                                    string nation2 = kl[23].ToString();
                                    string unknown11 = kl[24].ToString();
                                    string unknown12 = kl[25].ToString();
                                    string unknown13 = kl[26].ToString();

                                    if (Result == "1" || Result == "2")
                                    {
                                        String Durum = "2";
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[State],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + State + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + ")", baglantiLocalhost);
                                        SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + Durum + ")", baglantiLocalhost);


                                        int sayi = fixtureKaydet.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }
                                    else
                                    {
                                        String Durum = "1";
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[State],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + State + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + ")", baglantiLocalhost);
                                        SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + Durum + ")", baglantiLocalhost);


                                        int sayi = fixtureKaydet.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }


                                }

                                if (kl.Length == 25)
                                {
                                    string unknown = kl[1].ToString();
                                    string macTarihi = kl[2].ToString();
                                    string[] macTarihiParcala = macTarihi.Split('-');
                                    string gun = macTarihiParcala[0];
                                    string ay = macTarihiParcala[1];
                                    string yil = macTarihiParcala[2];
                                    string zaman = kl[3].ToString();
                                    string[] zamanparcala = zaman.Split(':');
                                    string saat = zamanparcala[0];
                                    string dakika = zamanparcala[1];
                                    string saniye = "00";
                                    int saat1;
                                    saat1 = (Convert.ToInt32(saat) + 2);
                                    saat = Convert.ToString(saat1);
                                    macTarihi = yil + "/" + ay + "/" + gun + " " + saat + ":" + dakika + ":" + saniye;
                                    int homeTeamid = Convert.ToInt32(kl[4].ToString());
                                    string homeTeamName = kl[5].ToString();
                                    string unknown2 = kl[6].ToString();
                                    int awayTeamid = Convert.ToInt32(kl[7].ToString());
                                    string awayTeamName = kl[8].ToString();
                                    string unknown3 = kl[9].ToString();
                                    string FTResult = kl[10].ToString();
                                    int? FTHomeGoals, FTAwayGoals;
                                    if (FTResult != "vs")
                                    {
                                        string[] FTGoals = kl[10].ToString().Split(':');
                                        FTHomeGoals = Convert.ToInt32(FTGoals[0].TrimEnd());
                                        FTAwayGoals = Convert.ToInt32(FTGoals[1].TrimStart());
                                    }
                                    else
                                    {
                                        FTHomeGoals = 0;
                                        FTAwayGoals = 0;
                                    }

                                    string unknown4 = kl[11].ToString();
                                    string unknown5 = kl[12].ToString();
                                    string Year = kl[13].ToString();
                                    string Tournament = kl[14].ToString();
                                    string Result = kl[15].ToString();
                                    string unknown7 = kl[16].ToString();
                                    string unknown8 = kl[17].ToString();
                                    string unknown9 = kl[18].ToString();
                                    string unknown10 = kl[19].ToString();
                                    string nation = kl[20].ToString();
                                    string nation2 = kl[21].ToString();
                                    string unknown11 = kl[22].ToString();
                                    string unknown12 = kl[23].ToString();
                                    string unknown13 = kl[24].ToString();

                                    if (Result == "1" || Result == "2")
                                    {
                                        String Durum = "2";
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[State],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + State + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);
                                        SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[nation],[nation2],[unknown11],[unknown12],[unknown13],[FTHomeGoals],[FTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "','" + unknown13 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + Durum + ")", baglantiLocalhost);

                                        int sayi = fixtureKaydet.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }
                                    else
                                    {
                                        String Durum = "1";
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[State],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + State + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);
                                        SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[nation],[nation2],[unknown11],[unknown12],[unknown13],[FTHomeGoals],[FTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "','" + unknown13 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + Durum + ")", baglantiLocalhost);

                                        int sayi = fixtureKaydet.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }


                                }



                            }

                        }

                        catch (Exception ex)
                        {
                            Globe.WriteLog(kl[0] + " Uzatmalara giden maçta, skor yanına konan yıldız benzeri karakterler patlatıyor!! : " + ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globe.WriteLog(fileName + " : Kaynak kod içerisinde fikstür yok. " + ex);
            }
        }
        public void GetLeague(string leagueName, string leagueUrl)
        {
            string dir = Globe.RootDir + "ligler\\" + leagueName + "\\";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string url = Globe.WhoScoredUrl + leagueUrl;
            string fileName = dir + Globe.LeagueFileName;
            //C:\WhoScored-master\htmlContent\Brazil_LigaDoBrasil\livescores.txt

            try
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
                Globe.WriteLog("Downloading from url: " + url);
                Thread.Sleep(3000);
                string htmlContent = BilgiCek(url);
                SaveContent(fileName, htmlContent);
                Globe.WriteLog(fileName + " oluşturuldu");
            }
            catch (Exception ex)
            {
                Globe.WriteLog(fileName + "- Hata : " + ex);
            }
        }


        private static string BilgiCek(string url)
        {
            var result = string.Empty;
            using (var webClient = new System.Net.WebClient())
            {
                string _UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                webClient.Headers.Add(HttpRequestHeader.UserAgent, _UserAgent);
                result = webClient.DownloadString(url);
            }
            return result;
        }



        //public SqlConnection baglantiLocalhost = new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");//trusted_connection=true
        //public SqlConnection baglantiLocalhost = new SqlConnection("server=psl.dynu.com;database=futbol;user=selsor;password=123456;pooling=false");//trusted_connection=true
        string matchHtmlContent;
        public SqlConnection baglantiLocalhost =
            new SqlConnection("server=localhost;database=futbol;user=sa;password=123456;pooling=false");

        public void GetMatches(string leagueName, List<int> matchIDs, Action<string> macwriter = null)
        {
            if (matchIDs.Count > 0)
            {
                string dir = Globe.RootDir + "ligler\\" + leagueName + "\\";

                foreach (int id in matchIDs)
                {

                    string url = Globe.WhoScoredMatchesUrl + id + @"/LiveStatistics";

                    Globe.WriteLog("Downloading from url: " + url);
                    Thread.Sleep(3000);
                    matchHtmlContent = BilgiCek(url);
                    //string matchHtmlContent = KaynakKodunuCek(url);
                    //Task<string> getHtmlContentByUrl = GetHtmlContentByUrl(url);
                    //string matchHtmlContent = await GetHtmlContentByUrl(url);

                    string fileName = dir + id + ".txt";

                    //int index = matchHtmlContent.IndexOf("initialData");
                    //if (index > 0)
                    //{

                    //    SaveContent(fileName, matchHtmlContent);

                    //    if (macwriter != null)
                    //        macwriter(fileName);

                    //}

                    int index = matchHtmlContent.IndexOf("var matchStats =");
                    if (index > 0)
                    {
                        //Thread.Sleep(3000);
                        SaveContent(fileName, matchHtmlContent);

                        macwriter?.Invoke(fileName);

                    }
                    else
                    {
                        url = Globe.WhoScoredMatchesUrl + id;
                        matchHtmlContent = string.Empty;
                        Thread.Sleep(3000);
                        matchHtmlContent = BilgiCek(url);
                        SaveContent(fileName, matchHtmlContent);
                        macwriter?.Invoke(fileName);

                    }

                    Globe.WriteLog(fileName + " oluşturuldu");

                }

                //foreach (int id in matchIDs)
                //{
                //    string url2 = Globe.WhoScoredMatchesUrl + id;

                //    Globe.WriteLog("Downloading from url: " + url2);
                //    Task<string> getHtmlContentByUrl = GetHtmlContentByUrl(url2);
                //    string matchHtmlContent = await GetHtmlContentByUrl(url2);

                //    string fileName = dir + id + ".txt";

                //    SaveContent(fileName, matchHtmlContent);
                //}
            }
        }

        private async Task<string> GetHtmlContentByUrl(string url)
        {
            string htmlContent = string.Empty;

            try
            {

                try
                {
                    HttpClient client = new HttpClient();
                    Task<string> getHtmlContentByUrl = client.GetStringAsync(url);
                    htmlContent = await getHtmlContentByUrl;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(@"Forbidden: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"SpiderAsync.GetHtmlContentByUrl: " + ex.Message);
            }

            return htmlContent;
        }

        public static string KaynakKodunuCek(string adres)
        {

            try
            {


                HttpWebRequest istek = (HttpWebRequest)WebRequest.Create(adres);
                istek.KeepAlive = false;

                HttpWebResponse cevap = (HttpWebResponse)istek.GetResponse();
                using (StreamReader okuyucu = new StreamReader(cevap.GetResponseStream(), Encoding.UTF8))
                {
                    return okuyucu.ReadToEnd();
                }
            }
            catch
            {
                Globe.WriteLog("Kaynak kod çekilemedi!! link : " + adres);
                //MessageBox.Show("Kaynak kod çekilemedi!! link : "+ adres);
                return adres;
            }

        }
        private void SaveContent(string fileName, string htmlContent)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fileName, true, Encoding.Default);
                sw.WriteLine(htmlContent);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                Globe.WriteLog("SpiderAsync.SaveContent: " + ex.Message);
            }
        }
        private List<int> GetOriginalMatchIDs(string directory)
        {
            List<int> originalMatchIDs = new List<int>();
            DirectoryInfo dir = new DirectoryInfo(directory);
            //dosya yolunu aldı

            FileInfo[] fileInfos = dir.GetFiles();

            if (fileInfos.Length > 0)
            {
                try
                {


                    foreach (FileInfo file in fileInfos)
                    {
                        if (file.FullName.Contains(Globe.LeagueFileName))
                            continue;

                        if (file.Length < Globe.IncorrectFileSize)
                        {
                            file.Delete();
                            continue;
                        }

                        if (file.FullName.Contains("_Teams"))
                            continue;

                        try
                        {
                            int id = int.Parse(Path.GetFileNameWithoutExtension(file.FullName));
                            if (!originalMatchIDs.Contains(id))
                                originalMatchIDs.Add(id);
                        }
                        catch (Exception ex)
                        {
                            Globe.WriteLog("Maç kodu hatası, sıkıntı yok" + ex);
                        }
                    }
                }
                catch (Exception ex)
                {

                    Globe.WriteLog("Dosya adı hatalı : " + ex);
                }
            }

            return originalMatchIDs;
        }

        private static string KaynakAl(string adres)
        {
            WebResponse benimResponse = null;

            try
            {
                WebRequest wr = WebRequest.Create(adres);
                benimResponse = wr.GetResponse();
            }
            catch (WebException)
            {
                MessageBox.Show("İnternet bağlantınızı ve Güvenlik duvarı ayarlarını kontrol ediniz", "Bağlantı hatası !!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            Stream str = benimResponse.GetResponseStream();

            StreamReader reader = new StreamReader(str);

            adres = reader.ReadToEnd();
            return adres;
        }

        protected internal void FikstureMacDoldur2(string fileName)
        {

            string s;
            s = DosyayiokuStringDondur(fileName);

            int index = s.IndexOf("DataStore.prime('teamfixtures', $.extend({ teamId: ");
            int index2 = s.IndexOf("var teamFixtures");
            string y = s.Substring(index + 76, index2 - index);

            string g = y.Replace("\r\n,", string.Empty);
            string[] z = g.Split(new Char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {


                foreach (string i in z)
                {
                    string l = i.Replace("]", string.Empty);
                    string m = l.Replace("\'", string.Empty);
                    //MessageBox.Show(l);
                    string[] kl = m.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (kl.Length == 7)
                    {
                        Globe.WriteLog("Hatalı kayıt");
                    }
                    else
                    {


                        try
                        {


                            //string[] deneme = fileName.Split('\\');
                            //string[] deneme2 = deneme[3].Split('.');
                            //int matchid = Convert.ToInt32(deneme2[0]);
                            int matchid = Convert.ToInt32(kl[0].ToString());
                            DAL dal = new DAL();
                            List<int> originalMatchIDs = dal.GetOriginalFixtureMatchIDs();
                            if (originalMatchIDs.Contains(matchid))
                            {


                                if (kl.Length == 28)
                                {
                                    //int matchid = Convert.ToInt32(kl[0].ToString());
                                    string unknown = kl[1].ToString();
                                    string macTarihi = kl[2].ToString();
                                    string[] macTarihiParcala = macTarihi.Split('-');
                                    string gun = macTarihiParcala[0];
                                    string ay = macTarihiParcala[1];
                                    string yil = macTarihiParcala[2];
                                    string zaman = kl[3].ToString();
                                    string[] zamanparcala = zaman.Split(':');
                                    string saat = zamanparcala[0];
                                    string dakika = zamanparcala[1];
                                    string saniye = "00";
                                    int saat1;
                                    saat1 = (Convert.ToInt32(saat) + 2);
                                    saat = Convert.ToString(saat1);
                                    //MessageBox.Show(saat +" <-> "+saat1);
                                    macTarihi = yil + "/" + ay + "/" + gun + " " + saat + ":" + dakika + ":" + saniye;
                                    int homeTeamid = Convert.ToInt32(kl[4].ToString());
                                    string homeTeamName = kl[5].ToString();
                                    string unknown2 = kl[6].ToString();
                                    int awayTeamid = Convert.ToInt32(kl[7].ToString());
                                    string awayTeamName = kl[8].ToString();
                                    string unknown3 = kl[9].ToString();
                                    string FTResult = kl[10].ToString();
                                    string HTResult = kl[11].ToString();

                                    int? FTHomeGoals, FTAwayGoals;
                                    if (FTResult != "vs")
                                    {
                                        string[] FTGoals = kl[10].ToString().Split(':');
                                        string YildizSil1 = FTGoals[0].Replace("*", string.Empty);
                                        string YildizSil2 = FTGoals[1].Replace("*", string.Empty);

                                        FTHomeGoals = Convert.ToInt32(YildizSil1.TrimEnd());
                                        FTAwayGoals = Convert.ToInt32(YildizSil2.TrimStart());
                                    }
                                    else
                                    {
                                        FTHomeGoals = 0;
                                        FTAwayGoals = 0;
                                    }
                                    string[] HTGoals = kl[11].ToString().Split(':');
                                    int HTHomeGoals = Convert.ToInt32(HTGoals[0].TrimEnd());
                                    int HTAwaygoals = Convert.ToInt32(HTGoals[1].TrimStart());

                                    string unknown4 = kl[12].ToString();
                                    string unknown5 = kl[13].ToString();
                                    string State = kl[14].ToString();
                                    string Year = kl[15].ToString();
                                    string Tournament = kl[16].ToString();
                                    string Result = kl[17].ToString();
                                    string unknown7 = kl[18].ToString();
                                    string unknown8 = kl[19].ToString();
                                    string unknown9 = kl[20].ToString();
                                    string unknown10 = kl[21].ToString();
                                    string TournamentCode = kl[22].ToString();
                                    string nation = kl[23].ToString();
                                    string nation2 = kl[24].ToString();
                                    string unknown11 = kl[25].ToString();
                                    string unknown12 = kl[26].ToString();
                                    string unknown13 = kl[27].ToString();






                                    if (!originalMatchIDs.Contains(matchid))
                                    {
                                        if (Result == "1" || Result == "2")
                                        {
                                            baglantiLocalhost.Open();
                                            SqlCommand mactarihiGuncelle = new SqlCommand(@"update fixture set matchDate='" + macTarihi + "' where matchid=" + matchid + string.Empty, baglantiLocalhost);
                                            //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                            int sayi = mactarihiGuncelle.ExecuteNonQuery();
                                            if (sayi > 0)
                                                Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                            baglantiLocalhost.Close();
                                        }
                                        else
                                        {
                                            baglantiLocalhost.Open();
                                            SqlCommand mactarihiGuncelle = new SqlCommand(@"update fixture set matchDate='" + macTarihi + "' where matchid=" + matchid + string.Empty, baglantiLocalhost);
                                            int sayi = mactarihiGuncelle.ExecuteNonQuery();

                                            //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                            //int sayi = fixtureKaydet.ExecuteNonQuery();
                                            if (sayi > 0)
                                                Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                            baglantiLocalhost.Close();
                                        }

                                    }
                                    else
                                    {
                                        Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına daha önce eklenmiştir.");
                                    }

                                }

                                if (kl.Length == 26)
                                {
                                    //int matchid = Convert.ToInt32(kl[0].ToString());
                                    string unknown = kl[1].ToString();
                                    string macTarihi = kl[2].ToString();
                                    string[] macTarihiParcala = macTarihi.Split('-');
                                    string gun = macTarihiParcala[0];
                                    string ay = macTarihiParcala[1];
                                    string yil = macTarihiParcala[2];
                                    string zaman = kl[3].ToString();
                                    string[] zamanparcala = zaman.Split(':');
                                    string saat = zamanparcala[0];
                                    string dakika = zamanparcala[1];
                                    string saniye = "00";
                                    int saat1;
                                    saat1 = (Convert.ToInt32(saat) + 2);
                                    saat = Convert.ToString(saat1);
                                    macTarihi = yil + "/" + ay + "/" + gun + " " + saat + ":" + dakika + ":" + saniye;
                                    int homeTeamid = Convert.ToInt32(kl[4].ToString());
                                    string homeTeamName = kl[5].ToString();
                                    string unknown2 = kl[6].ToString();
                                    int awayTeamid = Convert.ToInt32(kl[7].ToString());
                                    string awayTeamName = kl[8].ToString();
                                    string unknown3 = kl[9].ToString();
                                    string FTResult = kl[10].ToString();
                                    int? FTHomeGoals, FTAwayGoals;
                                    if (FTResult != "vs")
                                    {
                                        string[] FTGoals = kl[10].ToString().Split(':');
                                        FTHomeGoals = Convert.ToInt32(FTGoals[0].TrimEnd());
                                        FTAwayGoals = Convert.ToInt32(FTGoals[1].TrimStart());
                                    }
                                    else
                                    {
                                        FTHomeGoals = 0;
                                        FTAwayGoals = 0;
                                    }
                                    //string[] HTGoals = kl[11].ToString().Split(':');
                                    string unknown4 = kl[11].ToString();
                                    string unknown5 = kl[12].ToString();
                                    string Result = kl[13].ToString();
                                    string Year = kl[14].ToString();
                                    string Tournament = kl[15].ToString();
                                    string unknown7 = kl[16].ToString();
                                    string unknown8 = kl[17].ToString();
                                    string unknown9 = kl[18].ToString();
                                    string Season = kl[19].ToString();
                                    string TournamentCode = kl[20].ToString();
                                    string nation = kl[21].ToString();
                                    string nation2 = kl[22].ToString();
                                    string unknown11 = kl[23].ToString();
                                    string unknown12 = kl[24].ToString();
                                    string unknown13 = kl[25].ToString();


                                    if (Result == "1" || Result == "2")
                                    {
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);

                                        SqlCommand mactarihiGuncelle = new SqlCommand(@"update fixture set matchDate='" + macTarihi + "' where matchid=" + matchid + string.Empty, baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                        int sayi = mactarihiGuncelle.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }
                                    else
                                    {
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();

                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);

                                        SqlCommand mactarihiGuncelle = new SqlCommand(@"update fixture set matchDate='" + macTarihi + "' where matchid=" + matchid + string.Empty, baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                        int sayi = mactarihiGuncelle.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanında başarıyla güncellenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }


                                }

                                if (kl.Length != 26 && kl.Length != 28 && kl.Length != 1 && kl.Length != 2 && kl.Length == 27)
                                {
                                    //int matchid = Convert.ToInt32(kl[0].ToString());
                                    string unknown = kl[1].ToString();
                                    string macTarihi = kl[2].ToString();
                                    string[] macTarihiParcala = macTarihi.Split('-');
                                    string gun = macTarihiParcala[0];
                                    string ay = macTarihiParcala[1];
                                    string yil = macTarihiParcala[2];
                                    string zaman = kl[3].ToString();
                                    string[] zamanparcala = zaman.Split(':');
                                    string saat = zamanparcala[0];
                                    string dakika = zamanparcala[1];
                                    string saniye = "00";
                                    int saat1;
                                    saat1 = (Convert.ToInt32(saat) + 2);
                                    saat = Convert.ToString(saat1);
                                    macTarihi = yil + "/" + ay + "/" + gun + " " + saat + ":" + dakika + ":" + saniye;
                                    int homeTeamid = Convert.ToInt32(kl[4].ToString());
                                    string homeTeamName = kl[5].ToString();
                                    string unknown2 = kl[6].ToString();
                                    int awayTeamid = Convert.ToInt32(kl[7].ToString());
                                    string awayTeamName = kl[8].ToString();
                                    string unknown3 = kl[9].ToString();
                                    int? FTHomeGoals, FTAwayGoals;
                                    string FTResult = kl[10].ToString();

                                    if (FTResult != "vs")
                                    {
                                        string[] FTGoals = kl[10].ToString().Split(':');
                                        FTHomeGoals = Convert.ToInt32(FTGoals[0].TrimEnd());
                                        FTAwayGoals = Convert.ToInt32(FTGoals[1].TrimStart());
                                    }
                                    else
                                    {
                                        FTHomeGoals = 0;
                                        FTAwayGoals = 0;
                                    }
                                    //string[] HTGoals = kl[11].ToString().Split(':');
                                    string unknown4 = kl[11].ToString();
                                    string unknown5 = kl[12].ToString();
                                    string Year = kl[13].ToString();
                                    string State = kl[14].ToString();
                                    string Tournament = kl[15].ToString();
                                    string Result = kl[16].ToString();
                                    string unknown7 = kl[17].ToString();
                                    string unknown8 = kl[18].ToString();
                                    string unknown9 = kl[19].ToString();
                                    string unknown10 = kl[20].ToString();
                                    string TournamentCode = kl[21].ToString();
                                    string nation = kl[22].ToString();
                                    string nation2 = kl[23].ToString();
                                    string unknown11 = kl[24].ToString();
                                    string unknown12 = kl[25].ToString();
                                    string unknown13 = kl[26].ToString();

                                    if (Result == "1" || Result == "2")
                                    {
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[State],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + State + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + ")", baglantiLocalhost);
                                        SqlCommand mactarihiGuncelle = new SqlCommand(@"update fixture set matchDate='" + macTarihi + "' where matchid=" + matchid + string.Empty, baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                        int sayi = mactarihiGuncelle.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }
                                    else
                                    {
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[State],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + State + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + ")", baglantiLocalhost);
                                        SqlCommand mactarihiGuncelle = new SqlCommand(@"update fixture set matchDate='" + macTarihi + "' where matchid=" + matchid + string.Empty, baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                        int sayi = mactarihiGuncelle.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }


                                }

                                if (kl.Length == 25)
                                {
                                    string unknown = kl[1].ToString();
                                    string macTarihi = kl[2].ToString();
                                    string[] macTarihiParcala = macTarihi.Split('-');
                                    string gun = macTarihiParcala[0];
                                    string ay = macTarihiParcala[1];
                                    string yil = macTarihiParcala[2];
                                    string zaman = kl[3].ToString();
                                    string[] zamanparcala = zaman.Split(':');
                                    string saat = zamanparcala[0];
                                    string dakika = zamanparcala[1];
                                    string saniye = "00";
                                    int saat1;
                                    saat1 = (Convert.ToInt32(saat) + 2);
                                    saat = Convert.ToString(saat1);
                                    macTarihi = yil + "/" + ay + "/" + gun + " " + saat + ":" + dakika + ":" + saniye;
                                    int homeTeamid = Convert.ToInt32(kl[4].ToString());
                                    string homeTeamName = kl[5].ToString();
                                    string unknown2 = kl[6].ToString();
                                    int awayTeamid = Convert.ToInt32(kl[7].ToString());
                                    string awayTeamName = kl[8].ToString();
                                    string unknown3 = kl[9].ToString();
                                    string FTResult = kl[10].ToString();
                                    int? FTHomeGoals, FTAwayGoals;
                                    if (FTResult != "vs")
                                    {
                                        string[] FTGoals = kl[10].ToString().Split(':');
                                        FTHomeGoals = Convert.ToInt32(FTGoals[0].TrimEnd());
                                        FTAwayGoals = Convert.ToInt32(FTGoals[1].TrimStart());
                                    }
                                    else
                                    {
                                        FTHomeGoals = 0;
                                        FTAwayGoals = 0;
                                    }

                                    string unknown4 = kl[11].ToString();
                                    string unknown5 = kl[12].ToString();
                                    string Year = kl[13].ToString();
                                    string Tournament = kl[14].ToString();
                                    string Result = kl[15].ToString();
                                    string unknown7 = kl[16].ToString();
                                    string unknown8 = kl[17].ToString();
                                    string unknown9 = kl[18].ToString();
                                    string unknown10 = kl[19].ToString();
                                    string nation = kl[20].ToString();
                                    string nation2 = kl[21].ToString();
                                    string unknown11 = kl[22].ToString();
                                    string unknown12 = kl[23].ToString();
                                    string unknown13 = kl[24].ToString();

                                    if (Result == "1" || Result == "2")
                                    {
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[State],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + State + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);
                                        SqlCommand mactarihiGuncelle = new SqlCommand(@"update fixture set matchDate='" + macTarihi + "' where matchid=" + matchid + string.Empty, baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                        int sayi = mactarihiGuncelle.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }
                                    else
                                    {
                                        if (baglantiLocalhost.State == ConnectionState.Closed)
                                            baglantiLocalhost.Open();
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[homeTeamName],[unknown2],[awayTeamid],[awayTeamName],[unknown3],[FTResult],[unknown4],[unknown5],[Year],[State],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + homeTeamName + "','" + unknown2 + "'," + awayTeamid + ",'" + awayTeamName + "','" + unknown3 + "','" + FTResult + "','" + unknown4 + "','" + unknown5 + "','" + State + "','" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "')", baglantiLocalhost);
                                        SqlCommand mactarihiGuncelle = new SqlCommand(@"update fixture set matchDate='" + macTarihi + "' where matchid=" + matchid + string.Empty, baglantiLocalhost);
                                        //SqlCommand fixtureKaydet = new SqlCommand(@"INSERT INTO [dbo].[Fixture] ([CreatedDate],[matchid],[unknown],[matchDate],[homeTeamid],[unknown2],[awayTeamid],[unknown3],[FTResult],[HTResult],[unknown4],[unknown5],[State],[Year],[Tournament],[Result],[unknown7],[unknown8],[unknown9],[unknown10],[TournamentCode],[nation],[nation2],[unknown11],[unknown12],[FTHomeGoals],[FTAwayGoals],[HTHomeGoals],[HTAwayGoals],[Durum]) VALUES (Getdate()," + matchid + ",'" + unknown + "','" + macTarihi + "'," + homeTeamid + ",'" + unknown + "'," + awayTeamid + ",'" + unknown3 + "','" + FTResult + "',Isnull('" + HTResult + "',''),'" + unknown4 + "','" + unknown5 + "',Isnull('" + State + "',''),'" + Year + "','" + Tournament + "','" + Result + "','" + unknown7 + "','" + unknown8 + "','" + unknown9 + "','" + unknown10 + "','" + TournamentCode + "','" + nation + "','" + nation2 + "','" + unknown11 + "','" + unknown12 + "'," + FTHomeGoals + "," + FTAwayGoals + "," + HTHomeGoals + "," + HTAwaygoals + "," + Durum + ")", baglantiLocalhost);
                                        int sayi = mactarihiGuncelle.ExecuteNonQuery();
                                        if (sayi > 0)
                                            Globe.WriteLog(matchid + " numaralı " + homeTeamName + "-" + awayTeamName + " maçı fikstür veritabanına başarıyla eklenmiştir.");
                                        if (baglantiLocalhost.State == ConnectionState.Open)
                                            baglantiLocalhost.Close();
                                    }


                                }



                            }

                        }

                        catch (Exception ex)
                        {
                            Globe.WriteLog(kl[0] + " Uzatmalara giden maçta, skor yanına konan yıldız benzeri karakterler patlatıyor!! : " + ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globe.WriteLog(fileName + " : Kaynak kod içerisinde fikstür yok. " + ex);
            }
        }

        public static async Task<dynamic> getDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }

    }

}