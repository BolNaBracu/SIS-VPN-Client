using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace SIS_VPN_Client_Application.usercontrols.menu
{
    /// <summary>
    /// Interaction logic for ConnectControl.xaml
    /// </summary>
    public partial class ConnectControl : UserControl
    {
        CoreWebView2Environment env;
        public ConnectControl()
        {
            InitializeComponent();
            LoadAsync();
        }

        private void ConnectWithOpenVPN()
        {
            Process process = new Process();
            ProcessStartInfo startinfo = new ProcessStartInfo();
            startinfo.FileName = AppDomain.CurrentDomain.BaseDirectory + @"openvpn\openvpn.exe";
            startinfo.Arguments = "--config SIS-VPN_Client.ovpn";
            startinfo.Verb = "runas";
            process.StartInfo = startinfo;
            process.Start();
        }

        private void DisconnectFromOpenVPN()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/f /im openvpn.exe"
            });
        }

        private async void LoadAsync()
        {
            CoreWebView2EnvironmentOptions Options = new CoreWebView2EnvironmentOptions();
            env =
                await CoreWebView2Environment.CreateAsync(null, null, Options);

            ConnectWithOpenVPN();
            await webView.EnsureCoreWebView2Async(env);

            webView.Source = new Uri("https://google.com/", UriKind.Absolute);
        }

        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(addressBar.Text)) return;

            if (webView != null && webView.CoreWebView2 != null)
            {
                try
                {
                    webView.CoreWebView2.Navigate(addressBar.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Invalid URL", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ButtonDisconnect_Click(object sender, RoutedEventArgs e)
        {
            DisconnectFromOpenVPN();
        }
    }
}