using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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

        public delegate void OnConnectionChangeEvent(object sender, OnConnectionChangeEventArgs e);
        public event OnConnectionChangeEvent OnConnectionChange;


        public async Task WaitForConnectionAsync()
        {
            bool connectedWithVPN = await ConnectWithOpenVPNAsync();
            OnConnectionChange?.Invoke(this, new OnConnectionChangeEventArgs(connectedWithVPN));
        }

        private async Task<bool> ConnectWithOpenVPNAsync()
        {
            vpnProcess = new Process();
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"OpenVPN\bin\",
                FileName = "openvpn.exe",
                Arguments = "--config SISVPN_Client.ovpn"
            };

            vpnProcess.StartInfo = startInfo;
            if (vpnProcess.Start() == false) return false;

            await Task.Delay(1000);
            return true;
        }

        public void DisconnectFromOpenVPN()
        {
            if (vpnProcess is not null)
            {
                vpnProcess.Kill();
                vpnProcess.Close();
            }
        }
    }
}
