using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportProject.ProjectService;

namespace TransportProject.ViewModels
{
    public class AddEditRouteVM : INotifyPropertyChanged
    {
        string _routeName;

        public ProjectService.GPSPos StartPoint { get; set; }
        public ProjectService.GPSPos EndPoint { get; set; }

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
            StartPoint = new ProjectService.GPSPos();
            EndPoint = new ProjectService.GPSPos();
        }

        public AddEditRouteVM(ProjectService.Route r)
        {
            RouteName = r.RouteName;
            StartPoint = new ProjectService.GPSPos();
            StartPoint.Latitude = r.StartPoint.Latitude;
            StartPoint.Longitude = r.StartPoint.Longitude;
            EndPoint = new ProjectService.GPSPos();
            EndPoint.Latitude = r.EndPoint.Latitude;
            EndPoint.Longitude = r.EndPoint.Longitude;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChange(string propName)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(propName));
        }
    }
}
