using SIS_VPN_Client_Application.logic;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIS_VPN_Client_Application.usercontrols.menu
{
    public partial class ConnectControl : UserControl, INotifyPropertyChanged
    {
        private bool isRefreshNeeded;

        private string _connectingMessage;
        public string ConnectingMessage
        {
            get => _connectingMessage;
            set
            {
                _connectingMessage = value;
                RaisePropertyChanged("ConnectingMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ConnectControl()
        {
            DataContext = this;
            InitializeComponent();
        }

        public void EstablishConnection()
        {
            ConnectingMessage = $"You're being connected to {ConnectVPN.Instance.SelectedConfigEndpoint.Name}";
            ConnectVPN.Instance.OnConnectionStateChanged += VPN_OnConnectionStateChanged;
        }

        private void VPN_OnConnectionStateChanged(bool newState)
        {
            _ = Dispatcher.Invoke(async () =>
                {
                    if (newState)
                    {
                        webView.Source = new Uri("http://10.8.0.1:3000/", UriKind.Absolute);

                        if (isRefreshNeeded)
                        {
                            try
                            {
                                await webView.ExecuteScriptAsync("document.location.reload()");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }

                        await Task.Delay(ConnectVPN.Instance.DelayTimer - (ConnectVPN.Instance.DelayTimer / 5 * 4) + 100);
                        webView.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        webView.Visibility = Visibility.Hidden;
                        isRefreshNeeded = true;
                    }
                    ConnectingMessage = newState ? "" : $"Reconnecting to {ConnectVPN.Instance.SelectedConfigEndpoint.Name}";
                    connectionProgressBar.Visibility = newState ? Visibility.Hidden : Visibility.Visible;
                    isRefreshNeeded = !newState;
                });
        }

        private void webView_CoreWebView2InitializationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            webView.CoreWebView2.WebResourceResponseReceived += CoreWebView2_WebResourceResponseReceived;
        }

        private void CoreWebView2_WebResourceResponseReceived(object sender, Microsoft.Web.WebView2.Core.CoreWebView2WebResourceResponseReceivedEventArgs e)
        {
            if (e.Response != null && e.Response.StatusCode >= 400)
            {
                MessageBoxResult result = MessageBox.Show("Error connecting to SIS-VPN service!\n" +
                    "Retry connecting?", $"Problem {e.Response.StatusCode}", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    ConnectVPN.Instance.DisconnectFromOpenVPN();
                    Task.Run(async () => await ConnectVPN.Instance.ConnectWithOpenVPNAsync());
                }
            }
        }
    }
}