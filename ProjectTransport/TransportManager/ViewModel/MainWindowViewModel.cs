using GPSInterfaces.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManager.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //      FIELDS
        private List<Route> _routes;

        //      PROPERTIES
        public List<Route> Routes {
            get
            {
                return _routes;
            }
            internal set
            {
                _routes = value;
                RaisePropertyChanged("Routes");
            }
        }

        //      COMMANDS

        //      NOTIFY PROPERTY

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propName)
        {
            var x = PropertyChanged;
            if (x != null) PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
