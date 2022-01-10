using SIS_VPN_Client_Application.models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SIS_VPN_Client_Application.logic
{
    public class ConnectVPN
    {
        public delegate void ConnectionStateChanged(bool newState);
        public event ConnectionStateChanged OnConnectionStateChanged;

        private bool _isConnected = false;
        private bool IsConnected
        {
            get => _isConnected;
            set
            {
                _isConnected = value;
                OnConnectionStateChanged?.Invoke(value);
            }
        }
        private Process vpnProcess = null;
        public Endpoint selectedConfigEndpoint { get; set; } = null;

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

        public async Task ConnectWithOpenVPNAsync()
        {
            if (selectedConfigEndpoint is null)
            {
                throw new ArgumentException("Method doesn't know which OpenVPN configuration to run.");
            }

            vpnProcess = new Process();
            ProcessStartInfo startInfo = new()
            {
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory + @"OpenVPN\bin\",
                FileName = "openvpn.exe",
                Arguments = $"--config {selectedConfigEndpoint.FileName}",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            vpnProcess.StartInfo = startInfo;
            vpnProcess.Start();

            await Task.Delay(4000);

            IsConnected = true;
        }

        public void DisconnectFromOpenVPN()
        {
            if (IsConnected)
            {
                Process.Start(new ProcessStartInfo
                {
                    UseShellExecute = true,
                    Verb = "runas",
                    FileName = "taskkill",
                    Arguments = $"/f /IM openvpn.exe",
                    CreateNoWindow = true
                }).WaitForExit();

                IsConnected = false;
            }
        }
    }
}
