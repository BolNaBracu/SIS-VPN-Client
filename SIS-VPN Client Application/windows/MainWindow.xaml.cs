using SIS_VPN_Client_Application.logic;
using SIS_VPN_Client_Application.usercontrols;
using SIS_VPN_Client_Application.usercontrols.menu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SIS_VPN_Client_Application
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private readonly Dictionary<string, Control> controls = new()
        {
            { "WelcomeControl", new WelcomeControl() },
            { "Endpoints", new EndpointsControl() },
            { "Options", new OptionsControl() },
            { "Connect", new ConnectControl() },
            { "About", new AboutControl() }
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
            if (currentControl == "Connect")
            {
                ((ConnectControl)CurrentControl).EstablishConnection();
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentControl)));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ConnectVPN.Instance.DisconnectFromOpenVPN();
        }
    }
}
