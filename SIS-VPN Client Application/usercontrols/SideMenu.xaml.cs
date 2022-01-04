using SISVPN.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SIS_VPN_Client_Application.usercontrols
{
    public enum SideMenuOptions
    {
        Connect,
        Endpoints,
        Options,
        About
    }

    public class OptionSelectedEventArgs
    {
        public SideMenuOptions SideMenuOption { get; }

        public OptionSelectedEventArgs(SideMenuOptions sideMenuOption)
        {
            this.SideMenuOption = sideMenuOption;
        }
    }

    /// <summary>
    /// This class takes care of Side Menu presentation and captures its actions.
    /// </summary>
    public partial class SideMenu : UserControl
    {
        private bool IsConnected { get; set; }

        public SideMenuOptions SelectedOption { get; private set; }
        public SideMenu()
        {
            DataContext = this;
            InitializeComponent();
            Connection.Instance.OnConnectionChanged += Connection_OnConnectionChanged;
        }

        private void Connection_OnConnectionChanged(object sender, SISVPN.Client.EventArgs.OnConnectionEstablishedEventArgs onConnectionEstablishedEventArgs)
        {
            IsConnected = onConnectionEstablishedEventArgs.State;

            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                ConnectButton.Content = IsConnected
                ? "Connected"
                : "No connection";

                ConnectImage.Source = IsConnected
                    ? new BitmapImage(new Uri(@"/resources/symbols/symbol-connected.png", UriKind.Relative))
                    : new BitmapImage(new Uri(@"/resources/symbols/symbol-disconnected.png", UriKind.Relative));
            }));
        }

        public delegate void OptionSelected(object sender, OptionSelectedEventArgs e);
        public event OptionSelected OnOptionSelected;

        private void ConnectButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedOption = SideMenuOptions.Connect;
            AlertSubscribersOnOptionChange();
        }

        private void EndpointsButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedOption = SideMenuOptions.Endpoints;
            AlertSubscribersOnOptionChange();
        }

        private void OptionsButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedOption = SideMenuOptions.Options;
            AlertSubscribersOnOptionChange();
        }

        private void AboutButton_Checked(object sender, RoutedEventArgs e)
        {
            SelectedOption = SideMenuOptions.About;
            AlertSubscribersOnOptionChange();
        }

        private void AlertSubscribersOnOptionChange()
        {
            OnOptionSelected(this, new OptionSelectedEventArgs(SelectedOption));
        }
    }
}
