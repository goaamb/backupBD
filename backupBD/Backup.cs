using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Goaamb;

namespace backupBD
{
    class Backup
    {
        public Backup() 
        {
            Util.ReadIniFile();
            BD.LoadDataBases();
        }
        public static void verificar()
        {
            while (true)
            {
                List<Dictionary<String, Object>> Datos = obtenerDatos(BD.BDLocal);
                Thread.Sleep(new TimeSpan(3, 0, 0));
            }
        }

        private static List<Dictionary<string, object>> obtenerDatos(BD bD)
        {
            if (bD != null && bD.isOpen())
            {
                if (bD.type == BD.TypeBD.SQLServer)
                {
                }
                else
                {
                }
            }
            return null;
        }
    }
}
