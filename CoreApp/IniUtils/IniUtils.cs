using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApp.IniUtils
{
    public class IniUtils
    {
        private static FileIniDataParser parser;

        static IniUtils()
        {
            Properties.Settings.Default.ConfigName = "Config_" + Environment.UserDomainName + "_" + Environment.UserName + ".ini";
            parser = new FileIniDataParser();
        }

        public static void SetConfig(string category, string key, string value)
        {
            if (!File.Exists(Properties.Settings.Default.ConfigName))
            {
                using (File.Create(Properties.Settings.Default.ConfigName))
                {
                    WriteConfig(category, key, value);
                }
            }
            else
            {
                WriteConfig(category, key, value);
            }
        }

        public static string GetConfig(string category, string key)
        {
            if (!File.Exists(Properties.Settings.Default.ConfigName))
            {
                using (File.Create(Properties.Settings.Default.ConfigName))
                {
                    return ReadConfig(category, key);
                }
            }
            else
            {
                return ReadConfig(category, key);
            }
        }

        private static string ReadConfig(string category, string key)
        {
            try
            {
                IniData data = parser.ReadFile(Properties.Settings.Default.ConfigName);
                return data[category][key];
            }
            catch
            {
                return null;
            }
        }

        private static void WriteConfig(string category, string key, string value)
        {
            IniData data = parser.ReadFile(Properties.Settings.Default.ConfigName);
            data[category][key] = value;
            parser.WriteFile(Properties.Settings.Default.ConfigName, data);
        }
    }
}
