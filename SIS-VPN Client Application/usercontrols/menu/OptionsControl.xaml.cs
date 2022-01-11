using System.IO;
using System.Windows;
using System.Windows.Controls;
using SIS_VPN_Client_Application.logic;

namespace SIS_VPN_Client_Application.usercontrols.menu
{
    /// <summary>
    /// Interaction logic for OptionsControl.xaml
    /// </summary>
    public partial class OptionsControl : UserControl
    {
        public delegate void EndpointsFileDeleted(object sender);
        public event EndpointsFileDeleted OnEndpointsFileDeleted;

        public OptionsControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(EndpointsControl.settingsPath))
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete file \"endpoints.json\"?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    File.Delete(EndpointsControl.settingsPath);
                    OnEndpointsFileDeleted?.Invoke(this);
                    MessageBox.Show("File \"endpoints.json\" deleted!", "File deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("File \"endpoints.json\" doesn't exist!", "File not deleted", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            ConnectVPN.Instance.DelayTimer = (int)((e.Source as Slider).Value * 1000);
            MessageBox.Show("Connection wait delay set to " + ConnectVPN.Instance.DelayTimer + "ms", "New delay set", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
