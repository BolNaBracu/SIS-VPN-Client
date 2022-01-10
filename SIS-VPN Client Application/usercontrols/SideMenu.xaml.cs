using System.Windows;
using System.Windows.Controls;

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
        public SideMenuOptions SelectedOption { get; set; }

        private bool connectionEstablished;

        private void SetConnectionEstablished(bool value)
        {
            connectionEstablished = value;
            ConnectButton.IsEnabled = value;
        }

        public SideMenu()
        {
            DataContext = this;
            InitializeComponent();
            SetConnectionEstablished(true);
        }

        public delegate void OptionSelected(object sender, OptionSelectedEventArgs e);
        public event OptionSelected OnOptionSelected;

        private void ConnectButton_Checked(object sender, RoutedEventArgs e)
        {
            if (connectionEstablished)
            {
                SelectedOption = SideMenuOptions.Connect;
                AlertSubscribersOnOptionChange();
            }
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
