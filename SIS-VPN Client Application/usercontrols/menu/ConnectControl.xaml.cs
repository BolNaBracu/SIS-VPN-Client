using System;
using System.Net.Http;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using SISVPN.Client;

namespace SIS_VPN_Client_Application.usercontrols.menu
{
    /// <summary>
    /// Interaction logic for ConnectControl.xaml
    /// </summary>
    public partial class ConnectControl : UserControl
    {
        CoreWebView2Environment env;
        Semaphore waitForAnswerSemaphore;
        HttpResponseMessage httpResponse;

        public ConnectControl()
        {
            InitializeComponent();
            LoadAsync();
            Connection.Instance.Begin("127.0.0.1");
            Connection.Instance.OnResponseReceived += Connection_OnResponseReceived;
            waitForAnswerSemaphore = new(0, 1);
        }

        private void Connection_OnResponseReceived(object sender, OnResponseReceivedEventArgs e)
        {
            httpResponse = e.Response;
            waitForAnswerSemaphore.Release();
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

        private async void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            HttpRequestMessage httpRequestMessage = new();
            httpRequestMessage.Method = new(e.Request.Method);
            httpRequestMessage.RequestUri = new(e.Request.Uri);
            await Connection.Instance.SendHttpRequest(httpRequestMessage);
            waitForAnswerSemaphore.WaitOne();

            e.Response = env.CreateWebResourceResponse(await httpResponse.Content.ReadAsStreamAsync(), ((int)httpResponse.StatusCode), httpResponse.ReasonPhrase, httpResponse.Headers.ToString());
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