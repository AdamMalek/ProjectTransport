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
using TransportProject.ProjectService;

namespace TransportProject.ViewModels
{
    public enum eLoginStatus
    {
        LoginSuccessful = 0,
        LoginError = 1
    };

    public enum eRegisterMethod
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

        private ProjectService.ClientServiceClient proxy;
        private string _userKey;

        List<ProjectService.Route> _routes;

        public List<ProjectService.Route> Routes
        {
            get
            { return _routes; }
            set
            {
                _routes = value;
                RaisePropertyChange("Routes");
            }
        }


        ProjectService.Route _selectedRoute;

        internal bool RegisterRoute(Route r, eRegisterMethod method)
        {
            if (method == eRegisterMethod.Add)
            {
                var x = proxy.AddRoute(r);
                SyncRoutes();
                return x;
            }
            else if (method == eRegisterMethod.Update)
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

        private void SyncRoutes()
        {
            Routes = proxy.GetAllRoutes().ToList();
        }

        internal void CloseConnection()
        {
            proxy.Close();
        }

        public ProjectService.Route SelectedRoute
        {
            get
            { return _selectedRoute; }
            set
            {
                _selectedRoute = value;
                RaisePropertyChange("SelectedRoute");
            }
        }

        ProjectService.GPSData _selectedGPSData;

        public ProjectService.GPSData SelectedGPSData
        {
            get
            { return _selectedGPSData; }
            set
            {
                _selectedGPSData = value;
                RaisePropertyChange("SelectedGPSData");
            }
        }


        public ICommand LoginCMD { get; internal set; }
        public ICommand LogoutCMD { get; internal set; }


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

        private void Login(object obj)
        {
            proxy = new ProjectService.ClientServiceClient();
            _userKey = proxy.Login(Username, EncodeMD5(Password));

            if (_userKey == null)
            {
                IsLoggedIn = false;
                RaiseLoginEvent(eLoginStatus.LoginError);
            }
            else
            {
                IsLoggedIn = true;
                RaiseLoginEvent(eLoginStatus.LoginSuccessful);
                Routes = proxy.GetAllRoutes().ToList();
            }
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
