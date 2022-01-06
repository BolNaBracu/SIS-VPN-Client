using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            ConnectVPN.Instance.OnConnectionChange += VPN_OnConnectionChange;
            _ = Task.Run(async () => await ConnectVPN.Instance.WaitForConnectionAsync());
        }

        private void VPN_OnConnectionChange(object sender, OnConnectionChangeEventArgs e)
        {
            if (e.NewState == true)
            {
                if (!controls.TryGetValue("Connect", out _))
                {
                    controls.Add("Connect", new ConnectControl());
                }
            }
            else
            {
                if (controls.TryGetValue("Connect", out _))
                {
                    controls.Remove("Connect");
                }
            }
        }

        private readonly Dictionary<string, Control> controls = new Dictionary<string, Control> {
            { "WelcomeControl", new WelcomeControl() }
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
