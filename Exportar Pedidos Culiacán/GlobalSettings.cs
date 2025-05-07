﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exportar_Pedidos_Culiacán
{
    public class GlobalSettings
    {
        private static GlobalSettings instance;
        public string StringConnection { get; set; }
        public string Ip { get; set; }
        public string Puerto { get; set; }
        public string Direccion { get; set; }
        public string User { get; set; }
        public string Pw { get; set; }
        public string nameFile { get; set; }
        public int Bd { get; set; }
        public int Trn { get; set; }
        public List<string> Config { get; set; }
        public string PathConfig { get; } = "C:\\ConfigDB\\";
        private GlobalSettings()
        {
            Config = new List<string>();

        }
        public static GlobalSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalSettings();
                }
                return instance;
            }
        }
    }
}
