using DXApplication2.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TahminManager
{
    class Configuration
    {
        public void GetConfiguration()
        {
            ReadConfig();
            //whoscoredspider.ini dosyasından ayarları çekiyor.
            InitializeRootDir();
            //ayarlardaki ana dosya yolunu okuyor, klasör yoksa oluşturuyor.
            InitializeTime();
            //ayarlardaki çalışma zamanını okuyor.
            InitializeLeagues();
            //ayarlardaki Ligleri çekiyor
            InitializeUpdateDBFlag();
            //ayarlardaki DB güncellensinmi parametresini çekiyor.

        }
        public void GetTurkConfiguration()
        {
            ReadConfig();
            //whoscoredspider.ini dosyasından ayarları çekiyor.
            InitializeRootDir();
            //ayarlardaki ana dosya yolunu okuyor, klasör yoksa oluşturuyor.
            InitializeTime();
            //ayarlardaki çalışma zamanını okuyor.
            InitializeTurkish();
            //ayarlardaki Ligleri çekiyor
            InitializeUpdateDBFlag();
            //ayarlardaki DB güncellensinmi parametresini çekiyor.
        }
        private void ReadConfig()
        {
            Globe.ConfigDic.Clear();

            try
            {
                StreamReader configReader = new StreamReader(Globe.ConfigFile, Encoding.Default);

                string line = string.Empty;

                while (line != null)
                {
                    line = configReader.ReadLine();

                    if (line != null && !line.Equals(string.Empty) && !line.StartsWith("//"))
                    {
                        string[] temp = line.Split('=');
                        Globe.ConfigDic.Add(temp[0].Trim(), temp[1].Trim());
                    }
                }

                configReader.Close();
            }
            catch (Exception ex)
            {
                Globe.WriteLog("Configuration.ReadConfig: " + ex.Message);
            }
        }

        private void InitializeRootDir()
        {
            if (Globe.ConfigDic.ContainsKey(Globe.CONFIG_ROOT_DIR))
            {
                Globe.ConfigDic.TryGetValue(Globe.CONFIG_ROOT_DIR, out Globe.RootDir);

                if (!Directory.Exists(Globe.RootDir))
                {
                    Directory.CreateDirectory(Globe.RootDir);
                }
            }
        }

        private void InitializeTime()
        {
            if (Globe.ConfigDic.ContainsKey(Globe.CONFIG_WORK_TIME))
            {
                string workTime = string.Empty;
                Globe.ConfigDic.TryGetValue(Globe.CONFIG_WORK_TIME, out workTime);

                try
                {
                    string[] temp = workTime.Split(':');
                    //Yazılı zamanı saat ve dakika olarak ikiye bölüyor
                    Globe.WorkTime_Hour = int.Parse(temp[0]);
                    Globe.WorkTime_Minute = int.Parse(temp[1]);
                }
                catch (Exception ex)
                {
                    Globe.WriteLog("Configuration.InitializeTime: " + ex.Message);
                }
            }
        }

        private void InitializeUpdateDBFlag()
        {
            if (Globe.ConfigDic.ContainsKey(Globe.CONFIG_UPDATE_DB))
            {
                string updateDBFlag = string.Empty;
                Globe.ConfigDic.TryGetValue(Globe.CONFIG_UPDATE_DB, out updateDBFlag);

                try
                {
                    Globe.UpdateDBFlag = Convert.ToBoolean(int.Parse(updateDBFlag));
                }
                catch (Exception ex)
                {
                    Globe.WriteLog("Configuration.InitializeUpdateDBFlag: " + ex.Message);
                }
            }
        }

        private void InitializeLeagues()
        {
            Globe.LeaguesDic.Clear();
            string leagues = string.Empty;

            if (Globe.ConfigDic.ContainsKey(Globe.CONFIG_LEAGUES))
            {
                Globe.ConfigDic.TryGetValue(Globe.CONFIG_LEAGUES, out leagues);
            }

            string[] temp = leagues.Split(',');

            for (int i = 0; i < temp.Length; i += 2)
            {
                Globe.LeaguesDic.Add(temp[i].Trim(), temp[i + 1].Trim());
                //Ayarlardaki lig ismi ve klasör yollarını çekiyor.
            }
        }
        private void InitializeTurkish()
        {
            Globe.LeaguesDic.Clear();
            Globe.LeaguesDic.Add("Turkey-Super-Lig", "/Regions/225/Tournaments/17/Turkey-Super-Lig");
        }
    }
}
