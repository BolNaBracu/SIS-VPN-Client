using System;
using System.Diagnostics;

namespace SIS_VPN_Client_Application.logic
{
    public class ConnectVPN
    {
        private Process vpnProcess = null;

        private ConnectVPN() { }

        private static ConnectVPN instance = null;
        public static ConnectVPN Instance
        {
            get
            {
                if (instance is null)
                {
                    instance = new ConnectVPN();
                }
                return instance;
            }
        }

        public bool ConnectWithOpenVPN()
        {
            vpnProcess = new Process();
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"OpenVPN\bin\",
                FileName = "openvpn.exe",
                //Arguments = "--config SISVPN_Client.ovpn"
                Arguments = "--config SISVPN_Client_Singapore.ovpn"
            };

            vpnProcess.StartInfo = startInfo;
            return vpnProcess.Start() == false ? false : true;
        }

        public void DisconnectFromOpenVPN()
        {
            if (vpnProcess is not null)
            {
                vpnProcess.Kill();
                vpnProcess.Close();
            }
            Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/f /im openvpn.exe",
                CreateNoWindow = true,
                UseShellExecute = false
            }).WaitForExit();
        }
    }
}
