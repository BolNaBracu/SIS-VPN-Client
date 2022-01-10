using System;
using System.Text.Json.Serialization;

namespace SIS_VPN_Client_Application.models
{
    public class Endpoint : PropertyChangedBase
    {
        private bool selected;
        private string name;
        private string fileName;
        private string ipAddress;

        public Endpoint()
        {

        }

        public Endpoint(bool selected, string name, string fileName)
        {
            this.selected = selected;
            this.name = name;
            this.fileName = fileName;
            ipAddress = "";
        }

        public Endpoint(bool selected, string name, string fileName, string ipAddress)
        {
            this.selected = selected;
            this.name = name;
            this.fileName = fileName;
            this.ipAddress = ipAddress;
        }

        public bool IsSelected
        {
            get => selected;
            set
            {
                if (selected == value)
                {
                    return;
                }

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
                {
                    return;
                }

                name = value;

                NotifyPropertyChanged();
                NotifyPropertyChanged("NameAndAddress");
            }
        }

        public string FileName
        {
            get => fileName;
            set
            {
                if (fileName == value)
                {
                    return;
                }

                fileName = value;

                NotifyPropertyChanged();
            }
        }

        public string IPAddress
        {
            get => ipAddress;
            set
            {
                if (ipAddress == value)
                {
                    return;
                }

                ipAddress = value;

                NotifyPropertyChanged();
                NotifyPropertyChanged("NameAndAddress");
            }
        }

        [JsonIgnore]
        public string NameAndAddress // Would have been better to slap this into the View, but Radiobutton binding expects a property of the bound class
=> $"{Name} - {IPAddress}";
    }
}
