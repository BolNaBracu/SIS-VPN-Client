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
    }
}