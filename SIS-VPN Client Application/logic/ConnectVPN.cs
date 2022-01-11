using SIS_VPN_Client_Application.models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

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
        public Endpoint SelectedConfigEndpoint { get; set; } = null;

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

        public int DelayTimer { get; internal set; } = 5000;

        public async Task ConnectWithOpenVPNAsync()
        {
            if (SelectedConfigEndpoint is null)
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
                Arguments = $"--config {SelectedConfigEndpoint.FileName}",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            vpnProcess.StartInfo = startInfo;
            try
            {
                vpnProcess.Start();
            }
            catch
            {
                MessageBox.Show("OpenVPN couldn't start!\n" +
                    "Either \"openvpn.exe\" is missing, or you don't have sufficient rights to run this application.", "Execution stopped", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await Task.Delay(DelayTimer - (DelayTimer / 5) + 100);

            IsConnected = true;
        }

        public bool DisconnectFromOpenVPN()
        {
            if (IsConnected)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        UseShellExecute = true,
                        Verb = "runas",
                        FileName = "taskkill",
                        Arguments = $"/f /IM openvpn.exe",
                        WindowStyle = ProcessWindowStyle.Hidden
                    }).WaitForExit();
                    IsConnected = false;

                    return true;
                }
                catch
                {
                    MessageBox.Show("OpenVPN remains turned on!", "Termination stopped", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            return false;
        }
    }
}
