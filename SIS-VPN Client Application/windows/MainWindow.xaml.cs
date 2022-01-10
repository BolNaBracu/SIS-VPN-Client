using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using SIS_VPN_Client_Application.logic;
using SIS_VPN_Client_Application.usercontrols;
using SIS_VPN_Client_Application.usercontrols.menu;

namespace SIS_VPN_Client_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            DataContext = this;
            WaitForConnection();
            InitializeComponent();
        }

        private void WaitForConnection()
        {
            ConnectVPN.Instance.ConnectWithOpenVPN();
        }

        private readonly Dictionary<string, Control> controls = new Dictionary<string, Control> {
            { "WelcomeControl", new WelcomeControl() },
            { "Connect", new ConnectControl() }
        };

        private string currentControl = "WelcomeControl";
        public Control CurrentControl
        {
            get
            {
                controls.TryGetValue(currentControl, out Control value);
                return value;
            }
        }

        private void ChangeCurrentControl(SideMenuOptions option)
        {
            currentControl = option.ToString();
        }

        private void TopBarControl_OnMovingWindow()
        {
            DragMove();
        }

        private void SideMenu_OnOptionSelected(object sender, usercontrols.OptionSelectedEventArgs e)
        {
            currentControl = e.SideMenuOption.ToString();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentControl)));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ConnectVPN.Instance.DisconnectFromOpenVPN();
        }
    }
}
