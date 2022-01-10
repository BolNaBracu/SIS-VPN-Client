using SIS_VPN_Client_Application.logic;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIS_VPN_Client_Application.usercontrols.menu
{
    public partial class ConnectControl : UserControl
    {
        public ConnectControl()
        {
            InitializeComponent();
            webView.Source = new Uri("http://10.8.0.1:3000/", UriKind.Absolute);
        }

        private void webView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            webView.CoreWebView2.WebResourceResponseReceived += CoreWebView2_WebResourceResponseReceived;
        }

        private void CoreWebView2_WebResourceResponseReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (e.Response != null && e.Response.StatusCode > 300)
            {
                MessageBoxResult result = MessageBox.Show("Error connecting to SIS-VPN service!\n" +
                    "Retry connecting?", $"Problem {e.Response.StatusCode}", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    ConnectVPN.Instance.DisconnectFromOpenVPN();
                    ConnectVPN.Instance.ConnectWithOpenVPN();
                }
            }
        }
    }
}