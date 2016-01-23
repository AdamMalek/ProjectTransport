using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ServiceLibrary.ProjectService;
using GPSDataService.Models;

namespace TransportProject.ViewModels
{
    public enum eLoginStatus
    {
        LoginSuccessful = 0,
        LoginError = 1
    };

    public enum eRouteRegisterMethod
    {
        Add,
        Update,
        Remove
    }

    public enum eDataRegisterMethod
    {
        Add,
        Update,
        Remove
    }

    public class MainWindowVM : INotifyPropertyChanged
    {
        private string _username;
        public string Username { get { return _username; } set { _username = value; RaisePropertyChange("Username"); } }
        public string Password { get; set; }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                _isLoggedIn = value;
                RaisePropertyChange("IsLoggedIn");
                RaisePropertyChange("IsLoggedOut");
            }
        }
        public bool IsLoggedOut
        {
            get
            {
                return !IsLoggedIn;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _userKey;
        
        List<Route> _routes;

        public List<Route> Routes
        {
            get
            {
                if (_routes == null) return null;
                return _routes.OrderBy(m=> m.RouteName).ToList(); }
            set
            {
                _routes = value;
                RaisePropertyChange("CanExport");
                RaisePropertyChange("Routes");
            }
        }


        Route _selectedRoute;

        public List<Route> SameNameRoutes {
            get {
                if (_routes == null || _selectedRoute == null) return null;
               return _routes.Where(r => r.RouteName == _selectedRoute.RouteName).ToList();
            }
        }
        
       

        internal bool RegisterRoute(Route r, eRouteRegisterMethod method)
        {
            if (method == eRouteRegisterMethod.Add)
            {
                var x = proxy.AddRoute(r);
                SyncRoutes();
                return x;
            }
            else if (method == eRouteRegisterMethod.Update)
            {
                var x = proxy.UpdateRoute(r);
                SyncRoutes();
                return x;
            }
            else
            {
                var x = proxy.Delete(r);
                SyncRoutes();
                return x;
            }
        }
        internal bool RegisterData(GPSData r, eDataRegisterMethod method)
        {
            bool x;
            if (method == eDataRegisterMethod.Add)
            {
                r.Route = SelectedRoute;
                x = proxy.AddData(r);
            }
            else if (method == eDataRegisterMethod.Update)
            {
                x = proxy.UpdateData(r);
            }
            else
            {
                x = proxy.DeleteData(r);
            }
            SyncRoutes();
            return x;
        }

        int _selRouteId;
        int _selDataId;

        private void SyncRoutes()
        {
            if (SelectedRoute != null) _selRouteId = SelectedRoute.RouteId;
            if (SelectedGPSData != null) _selDataId = SelectedGPSData .Id;
            Routes = proxy.GetAllRoutes().ToList();
            SelectedRoute = Routes.FirstOrDefault(r => r.RouteId == _selRouteId);
            if (SelectedRoute == null) SelectedRoute = Routes.FirstOrDefault();
            if (SelectedRoute != null) SelectedGPSData = SelectedRoute.RouteData.FirstOrDefault(data => data.Id == _selDataId);

        }

        internal void CloseConnection()
        {
            if (proxy != null) 
                proxy.Close();
        }

        public Route SelectedRoute
        {
            get
            { return _selectedRoute; }
            set
            {
                _selectedRoute = value;
                RaisePropertyChange("SelectedRoute");
                RaisePropertyChange("isRouteSelected");
                RaisePropertyChange("SameNameRoutes");
            }
        }

        

        public bool isRouteSelected { get { return SelectedRoute != null && IsLoggedIn; } }
        public bool isDataSelected { get { return SelectedGPSData != null && IsLoggedIn; } }

        GPSData _selectedGPSData;

        public GPSData SelectedGPSData
        {
            get
            { return _selectedGPSData; }
            set
            {
                _selectedGPSData = value;
                RaisePropertyChange("SelectedGPSData");
                RaisePropertyChange("isDataSelected");
            }
        }


        public ICommand LoginCMD { get; internal set; }
        public ICommand LogoutCMD { get; internal set; }
        public bool CanExport { get { return IsLoggedIn && Routes != null && Routes.Count > 0; } }
        public bool OpenAfter { get; set; }

        public bool Export(string filename)
        {
            ExcelIO.ExcelSaver saver = new ExcelIO.ExcelSaver(filename);
            return saver.Export(Routes.AsEnumerable(), OpenAfter);
        }

        bool _isWaiting = false;
        public bool isWaiting { get { return _isWaiting;  } set { _isWaiting = value; RaisePropertyChange("isWaiting"); } }




        public MainWindowVM()
        {
            Password = "";
            LoginCMD = new RelayCommand(Login, canLogin);
            LogoutCMD = new RelayCommand(Logout, (obj) => true);
        }

        private void Logout(object obj)
        {
            CloseConnection();
            IsLoggedIn = false;
            Routes = null;
            SelectedRoute = null;
        }

        private bool canLogin(object obj)
        {
            return ((Username != null && Username.Trim().Length > 0) && (Password != null && Password.Length > 0));
        }

        public delegate void OnLogin(eLoginStatus status);
        public event OnLogin LoginEvent;

        private ClientServiceClient proxy;

        private async void LoginToService()
        {
            isWaiting = true;

            proxy = new ClientServiceClient();
            string token = await proxy.RequestValidationTokenAsync();
            string pass = EncodeMD5(Password + token);
            _userKey = await proxy.LoginAsync(Username, pass);

            if (_userKey == null)
            {
                IsLoggedIn = false;
                RaiseLoginEvent(eLoginStatus.LoginError);
                proxy.Close();
            }
            else
            {
                IsLoggedIn = true;
                RaiseLoginEvent(eLoginStatus.LoginSuccessful);
                Routes = proxy.GetAllRoutes().ToList();
            }
            isWaiting = false;
        }

        private void Login(object obj)
        {
            LoginToService();
        }

        private void RaiseLoginEvent(eLoginStatus status)
        {
            var x = LoginEvent;
            if (x != null) x(status);
        }

        private string EncodeMD5(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            return encoded;
        }


        private void RaisePropertyChange(string propName)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(propName));
        }
    }
}
