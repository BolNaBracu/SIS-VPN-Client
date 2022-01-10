using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS_VPN_Client_Application.logic
{
    class VPNSettingsParser
    {
        private string configPath;
        private List<string> configEntries;

        public VPNSettingsParser(string configPath)
        {
            this.configPath = configPath;
            this.configEntries = new List<string>();
        }

        public void ReadConfigFile()
        {
            var allConfigLines = File.ReadAllLines(configPath).ToList();

            configEntries.Clear();
            foreach(var line in allConfigLines)
            {
                if (line.Contains("ca")) break;

                configEntries.Add(line);
            }
        }

        public string ReadIPAddress()
        {
            var ipLine = configEntries.FirstOrDefault(entry => entry.Contains("remote") && entry.Contains("."));

            if (ipLine == null)
                return "";

            return ipLine.Split(' ')[1];
        }
    }
}