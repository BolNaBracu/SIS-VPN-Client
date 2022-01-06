using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;

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

        private async void LoadAsync()
        {
            CoreWebView2EnvironmentOptions Options = new CoreWebView2EnvironmentOptions();
            env =
                await CoreWebView2Environment.CreateAsync(null, null, Options);

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
    }
}