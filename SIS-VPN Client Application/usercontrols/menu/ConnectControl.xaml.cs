using System;
using System.Collections.Generic;
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

        private async void LoadAsync()
        {
            CoreWebView2EnvironmentOptions Options = new CoreWebView2EnvironmentOptions();
            env =
                await CoreWebView2Environment.CreateAsync(null, null, Options);
            await webView.EnsureCoreWebView2Async(env);
            webView.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            webView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
            webView.Source = new Uri("https://www.bing.com/", UriKind.Absolute);
        }

        private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            Console.WriteLine(e.Request);
            // TODO: Get Response from server, write it in here:
            e.Response = env.CreateWebResourceResponse(null, 401, "Unauthorized", "headers"); ;
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