using SIS_VPN_Client_Application.logic;
using System.Windows;

namespace SIS_VPN_Client_Application
{
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ConnectVPN.Instance.DisconnectFromOpenVPN();
        }
    }
}
