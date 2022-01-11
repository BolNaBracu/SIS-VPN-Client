using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SIS_VPN_Client_Application.models
{
    public abstract class PropertyChangedBase : INotifyPropertyChanged // Cos repeating a single line is better than repeating two of them :'D
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
