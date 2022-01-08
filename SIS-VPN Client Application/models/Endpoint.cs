using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIS_VPN_Client_Application.models
{
    public class Endpoint : PropertyChangedBase
    {
        private bool selected;
        private string name;
        private string configPath;

        public Endpoint()
        {

        }

        public Endpoint(bool selected, string name, string configPath)
        {
            this.selected = selected;
            this.name = name;
            this.configPath = configPath;
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
    }
}
