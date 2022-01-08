using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SIS_VPN_Client_Application.models
{
    public class Endpoint : PropertyChangedBase
    {
        private bool selected;
        private string name;
        private string configPath;
        private string ipAddress;

        public Endpoint()
        {
   
        }

        public Endpoint(bool selected, string name, string configPath)
        {
            this.selected = selected;
            this.name = name;
            this.configPath = configPath;
            this.ipAddress = "";
        }

        public Endpoint(bool selected, string name, string configPath, string ipAddress)
        {
            this.selected = selected;
            this.name = name;
            this.configPath = configPath;
            this.ipAddress = ipAddress;
        }

        public bool IsSelected
        {
            get => selected;
            set
            {
                if (selected == value)
                    return;
                selected = value;

                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;
                name = value;

                NotifyPropertyChanged();
                NotifyPropertyChanged("NameAndAddress");
            }
        }

        public string ConfigPath
        {
            get => configPath;
            set
            {
                if (configPath == value)
                    return;
                configPath = value;

                NotifyPropertyChanged();
            }
        }

        public string IPAddress
        {
            get => ipAddress;
            set
            {
                if (ipAddress == value)
                    return;
                ipAddress = value;

                NotifyPropertyChanged();
                NotifyPropertyChanged("NameAndAddress");
            }
        }

        [JsonIgnore]
        public string NameAndAddress // Would have been better to slap this into the View, but Radiobutton binding expects a property of the bound class
        {
            get => $"{Name} - {IPAddress}";
        }
    }
}
