using Microsoft.Win32;
using SIS_VPN_Client_Application.logic;
using SIS_VPN_Client_Application.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SIS_VPN_Client_Application.usercontrols.menu
{
    /// <summary>
    /// Interaction logic for EndpointsControl.xaml
    /// </summary>
    public partial class EndpointsControl : UserControl, INotifyPropertyChanged
    {
        private Endpoint selectedEndpoint;
        public Endpoint SelectedEndpoint
        {
            get => selectedEndpoint;
            set
            {
                selectedEndpoint = value;
                if (ConnectVPN.Instance.DisconnectFromOpenVPN())
                {
                    _ = Task.Run(async () => await ConnectVPN.Instance.ConnectWithOpenVPNAsync());
                }
                ConnectVPN.Instance.SelectedConfigEndpoint = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Endpoint> Endpoints { get; set; }
        public readonly static string settingsPath = "endpoints.json";

        public EndpointsControl()
        {
            Endpoints = new ObservableCollection<Endpoint>();

            LoadSavedEndpoints();

            InitializeComponent(); // Note to self, init collections before InitializeComponent :(
        }

        private void LoadSavedEndpoints()
        {
            if (!File.Exists(settingsPath))
            {
                return;
            }

            try
            {
                string endpointsJson = File.ReadAllText(settingsPath);

                List<Endpoint> savedEndpoints = JsonSerializer.Deserialize<List<Endpoint>>(endpointsJson);

                Endpoints.Clear();
                savedEndpoints.ForEach(endpoint => Endpoints.Add(endpoint));

                SelectedEndpoint = Endpoints.FirstOrDefault(endpoint => endpoint.IsSelected);

                ConnectVPN.Instance.SelectedConfigEndpoint = SelectedEndpoint;
            }
            catch (InvalidOperationException)
            {
            }
            catch (JsonException)
            {
            }
        }

        private void SaveEndpoints()
        {
            string endpointsJson = JsonSerializer.Serialize(Endpoints);

            try
            {
                File.WriteAllText(settingsPath, endpointsJson);
            }
            catch (IOException ioex)
            {
                MessageBox.Show(ioex.Message, "Error");
            }
        }

        private void ButtonSaveEndpoints_Click(object sender, RoutedEventArgs e)
        {
            SaveEndpoints();
        }

        private void ButtonOpenConfig_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEndpoint == null)
            {
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select only from this directory!",
                Filter = "OpenVPN Config (*.ovpn)|*.ovpn",
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"OpenVPN\bin\"
            };

            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }

            string configPath = openFileDialog.FileName;

            if (!configPath.StartsWith(AppDomain.CurrentDomain.BaseDirectory + @"OpenVPN\bin\"))
            {
                MessageBox.Show(@"OpenVPN file must be in ""OpenVPN\bin\"" folder!", "Invalid file location.", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            VPNSettingsParser parser = new VPNSettingsParser(configPath);

            parser.ReadConfigFile();
            SelectedEndpoint.IPAddress = parser.ReadIPAddress();
            SelectedEndpoint.FileName = configPath.Split(AppDomain.CurrentDomain.BaseDirectory + @"OpenVPN\bin\")[1];
        }

        private void ButtonNewEndpoint_Click(object sender, RoutedEventArgs e)
        {
            Endpoints.Add(new Endpoint(true, "New", ""));
        }

        private void ButtonDeleteEndpoint_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEndpoint == null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {selectedEndpoint.Name} endpoint?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            Endpoints.Remove(selectedEndpoint);

            if (Endpoints.Count > 0)
            {
                SelectedEndpoint = Endpoints.Last();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}