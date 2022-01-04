using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Web.WebView2.Core;

using SISVPN.Client;
using SISVPN.Client.EventArgs;
using SISVPN.Common.Models;

namespace SIS_VPN_Client_Application.usercontrols.menu
{
    /// <summary>
    /// Interaction logic for ConnectControl.xaml
    /// </summary>
    public partial class ConnectControl : UserControl
    {
        private CoreWebView2Environment env;
        private Semaphore waitForAnswerSemaphore;
        private ResponseObject responseObject;

        public ConnectControl()
        {
            InitializeComponent();

            Connection.Instance.OnConnectionChanged += Connection_OnConnectionChanged;
            Connection.Instance.OnResponseReceived += Connection_OnResponseReceived;
            waitForAnswerSemaphore = new(0, 1);

            _ = LoadAsync();
        }

        private void Connection_OnConnectionChanged(object sender, OnConnectionEstablishedEventArgs onConnectionEstablishedEventArgs)
        {
            if (onConnectionEstablishedEventArgs.State)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(async () =>
                {
                    env = await CoreWebView2Environment.CreateAsync();
                    await webView.EnsureCoreWebView2Async(env);
                    webView.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
                    webView.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
                    webView.Source = new Uri("https://www.bing.com/", UriKind.Absolute);
                }));
            }
            else
            {
                MessageBox.Show("Connection to server lost!\nWe are trying to connect in the background...", "Connection lost!", MessageBoxButton.OK, MessageBoxImage.Warning);
                _ = LoadAsync();
            }
        }

        private void Connection_OnResponseReceived(object sender, OnResponseReceivedEventArgs e)
        {
            responseObject = e.Response;
            _ = waitForAnswerSemaphore.Release();
        }

        private async Task LoadAsync()
        {
            await Connection.Instance.BeginAsync("127.0.0.1");
        }

        private async Task WaitForResponse()
        {
            await Connection.Instance.GetResponseAsync();
        }

        private async void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            HttpRequestMessage httpRequestMessage = new();
            httpRequestMessage.Method = new(e.Request.Method);
            httpRequestMessage.RequestUri = new(e.Request.Uri);
            try
            {
                await Connection.Instance.SendHttpRequest(httpRequestMessage);
                await WaitForResponse();
            }
            catch (Exception)
            {
                return;
            }

            MemoryStream mStrm = new MemoryStream(Encoding.UTF8.GetBytes(responseObject.ResponseBody));
            e.Response = env.CreateWebResourceResponse(mStrm, responseObject.StatusCode, responseObject.ReasonPhrase, responseObject.Headers);

            return;
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