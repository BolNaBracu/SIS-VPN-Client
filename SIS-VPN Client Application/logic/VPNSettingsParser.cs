using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SIS_VPN_Client_Application.logic
{
    internal class VPNSettingsParser
    {
        private readonly string configPath;
        private readonly List<string> configEntries;

        public VPNSettingsParser(string configPath)
        {
            this.configPath = configPath;
            configEntries = new List<string>();
        }

        public void ReadConfigFile()
        {
            List<string> allConfigLines = File.ReadAllLines(configPath).ToList();

            configEntries.Clear();
            foreach (string line in allConfigLines)
            {
                if (line.Contains("ca"))
                {
                    break;
                }

                configEntries.Add(line);
            }
        }

        public string ReadIPAddress()
        {
            string ipLine = configEntries.FirstOrDefault(entry => entry.Contains("remote") && entry.Contains("."));

            if (ipLine == null)
            {
                return "";
            }

            return ipLine.Split(' ')[1];
        }
    }
}