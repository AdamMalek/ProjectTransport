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

namespace TransportProject.ViewModels
{
    public class MainWindowVM: INotifyPropertyChanged
    {
        private string _username;
        public string Username { get { return _username; } set { _username = value; RaisePropertyChange("Username"); } }
        public string Password { get; set; }

        private bool _isLoggedIn;
        public bool IsLoggedIn {
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
        public bool IsLoggedOut { get
            {
                return !IsLoggedIn;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ProjectService.ClientServiceClient proxy;
        private string _userKey;

        public ICommand LoginCMD { get; internal set; }
        public ICommand LogoutCMD { get; internal set; }


        public MainWindowVM()
        {
            LoginCMD = new RelayCommand(Login,canLogin);
            LogoutCMD = new RelayCommand(Logout,(obj) => true);
            proxy = new ProjectService.ClientServiceClient();
        }

        private void Logout(object obj)
        {
            throw new NotImplementedException();
        }

        private bool canLogin(object obj)
        {            
            return (Username!=null && Username.Trim().Length > 0);
        }

        private void Login(object obj)
        {
            _userKey = proxy.Login(Username, EncodeMD5(Password));

            if (_userKey == null) ??;

            IsLoggedIn = (_userKey != null);

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
