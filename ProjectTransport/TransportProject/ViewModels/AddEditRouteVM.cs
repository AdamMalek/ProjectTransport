using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceLibrary.ProjectService;
using GPSDataService.Models;
//using GPSDataService.Models;


namespace TransportProject.ViewModels
{
    public class AddEditRouteVM : INotifyPropertyChanged
    {
        string _routeName;

        public GPSPos StartPoint { get; set; }
        public GPSPos EndPoint { get; set; }

        public string RouteName
        {
            get { return _routeName; }
            set
            {
                _routeName = value; RaisePropertyChange("RouteName"); //RaisePropertyChange("isDataValid");
            }
        }

        double _startLatitude;
        double _startLongitude;
        double _endLatitude;
        double _endLongitude;
        public double StartPointLatitude
        {
            get { return _startLatitude; }
            set
            {
                if (value < -90 || value > 90)
                    _startLatitude = 0;
                else
                    _startLatitude = value;

                StartPoint.Latitude = _startLatitude;
                RaisePropertyChange("isDataValid");
                RaisePropertyChange("StartPointLatitude");
            }
        }
        public double StartPointLongitude
        {
            get { return _startLongitude; }
            set
            {
                if (value < -180 || value > 180)
                    _startLongitude = 0;
                else
                    _startLongitude = value;

                StartPoint.Longitude= _startLongitude;
                RaisePropertyChange("isDataValid");
                RaisePropertyChange("StartPointLongitude");
            }
        }

        public double EndPointLatitude
        {
            get { return _endLatitude; }
            set
            {
                if (value < -90 || value > 90)
                    _endLatitude = 0;
                else
                    _endLatitude = value;

                EndPoint.Latitude = _endLatitude;
                RaisePropertyChange("isDataValid");
                RaisePropertyChange("EndPointLatitude");
            }
        }
        public double EndPointLongitude
        {
            get { return _endLongitude; }
            set {
                if (value < -180 || value > 180)
                    _endLongitude = 0;
                else
                    _endLongitude = value;

                EndPoint.Longitude = _endLongitude;
                RaisePropertyChange("isDataValid");
                RaisePropertyChange("EndPointLongitude");
            }
        }

        public bool isDataValid
        {
            get
            {
                if (RouteName == null || RouteName.Trim().Length == 0) return false;
                return true;
            }
        }

        public AddEditRouteVM()
        {
            StartPoint = new GPSPos();
            EndPoint = new GPSPos();
        }

        public AddEditRouteVM(Route r)
        {
            RouteName = r.RouteName;
            StartPoint = r.StartPoint;
            _startLatitude = r.StartPoint.Latitude;
            _startLongitude = r.StartPoint.Longitude;
            EndPoint = r.EndPoint;
            _endLatitude = r.EndPoint.Latitude;
            _endLongitude = r.EndPoint.Longitude;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChange(string propName)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(propName));
        }
    }
}
