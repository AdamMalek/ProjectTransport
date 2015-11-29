using GPSInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TransportManager.ViewModel
{
    public class LoginWindowViewModel
    {

        public delegate void OnLoginOrRegisterSuccessful(object sender, string userHash);
        public event OnLoginOrRegisterSuccessful login;

        public ICommand LoginCommand { get; internal set; }
        public ICommand CreateAccountCommand { get; internal set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public LoginWindowViewModel()
        {
            Login = "";
            Password = "";

            LoginCommand = new RelayCommand(LoginFunc, CanLogin);
            CreateAccountCommand = new RelayCommand(RegisterFunc, (obj) => true);
        }

        private void RegisterFunc(object obj)
        {
            try
            {
                ChannelFactory<IWCFGPSDataService> channelFactory = new ChannelFactory<IWCFGPSDataService>("GPSServiceEndpoint");
                IWCFGPSDataService proxy = channelFactory.CreateChannel();
                bool RegisterCompleted = proxy.Register(Login, GPSInterfaces.Helpers.MD5Encoder.EncodeMD5(Password));
                if (!RegisterCompleted)
                {
                    MessageBox.Show("Error while registering!");
                }
                else
                {
                    MessageBox.Show("Register successful");
                    if (login != null) login(this, GPSInterfaces.Helpers.MD5Encoder.EncodeMD5(Login));
                }
            }
            catch
            {
                MessageBox.Show("Error while connecting to server!");
            }
        }

        private bool CanLogin(object obj)
        {
            return !(Login.Trim().Length == 0);
        }

        private void LoginFunc(object obj)
        {
            var passwordBox = obj as PasswordBox;
            Password = passwordBox.Password;
            try
            {
                ChannelFactory<IWCFGPSDataService> channelFactory = new ChannelFactory<IWCFGPSDataService>("GPSServiceEndpoint");
                IWCFGPSDataService proxy = channelFactory.CreateChannel();
                var LoginCorrect = proxy.Login(Login,GPSInterfaces.Helpers.MD5Encoder.EncodeMD5(Password));

                if (!LoginCorrect)
                {
                    MessageBox.Show("Login or password incorrect!");
                }
                else
                {
                    MessageBox.Show("Login successful");
                    if (login != null) login(this, GPSInterfaces.Helpers.MD5Encoder.EncodeMD5(Login));
                }
            }
            catch
            {                
                MessageBox.Show("Error while connecting to server!");
            }
        }
    }
}
