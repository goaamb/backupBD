using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace backupBD
{
    class Util
    {
        public static Dictionary<string, string> settings;

        private static void ReadIniFile()
        {
            String iniFile;
            String ruta;
            Console.Out.WriteLine("Reading IniFile");
            ruta = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            iniFile = @ruta.Replace("file:\\", "") + "\\settings.ini";

            if (!File.Exists(iniFile))
            {
                crearINI(iniFile);
            }

            settings = Util.leerINI(iniFile);
            Console.Out.WriteLine("End Reading IniFile");
        }
        public static Dictionary<string, string> leerINI(string iniFile)
        {
            List<string> categories = Inifile.GetCategories(iniFile);
            Dictionary<string, string> allKeys = new Dictionary<string, string>();
            string defaultValue = "";
            foreach (string category in categories)
            {
                List<string> keys = Inifile.GetKeys(iniFile, category);
                foreach (string key in keys)
                {
                    string content = Inifile.GetIniFileString(iniFile, category, key, defaultValue);
                    if (!allKeys.ContainsKey(category + "." + key))
                    {
                        allKeys.Add(category + "." + key, content);
                    }
                }
            }
            return allKeys;
        }

        private static void crearINI(string iniFile)
        {
            Inifile.WritePrivateProfileString("GLOBAL", "SUCURSAL", "1", iniFile);
            Inifile.WritePrivateProfileString("LOCAL", "SERVER", "localhost", iniFile);
            Inifile.WritePrivateProfileString("LOCAL", "PORT", "3306", iniFile);
            Inifile.WritePrivateProfileString("LOCAL", "DATABASE", "DB", iniFile);
            Inifile.WritePrivateProfileString("LOCAL", "USER", "USER", iniFile);
            Inifile.WritePrivateProfileString("LOCAL", "PASSWORD", "PASSWORD", iniFile);
            Inifile.WritePrivateProfileString("LOCAL", "TYPE", "SQLSERVER", iniFile);
            Inifile.WritePrivateProfileString("SERVER", "SERVER", "localhost", iniFile);
            Inifile.WritePrivateProfileString("SERVER", "PORT", "3306", iniFile);
            Inifile.WritePrivateProfileString("SERVER", "DATABASE", "DB", iniFile);
            Inifile.WritePrivateProfileString("SERVER", "USER", "USER", iniFile);
            Inifile.WritePrivateProfileString("SERVER", "PASSWORD", "PASSWORD", iniFile);
            Inifile.WritePrivateProfileString("SERVER", "TYPE", "MYSQL", iniFile);
        }
    }
}
