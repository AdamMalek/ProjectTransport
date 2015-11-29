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

        public delegate void OnLoginOrRegisterSuccessful(object sender, Guid session);
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
                var RegisterCompleted = proxy.Register(Login, GPSInterfaces.Helpers.MD5Encoder.EncodeMD5(Password));
                if (RegisterCompleted == null)
                {
                    MessageBox.Show("Error while registering!");
                }
                else
                {
                    MessageBox.Show("Register successful");
                    if (login != null) login(this, RegisterCompleted.GetValueOrDefault());
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
                var LoginStatus = proxy.Login(Login,GPSInterfaces.Helpers.MD5Encoder.EncodeMD5(Password));

                if (LoginStatus == null)
                {
                    MessageBox.Show("Login or password incorrect!");
                }
                else if (LoginStatus == Guid.Empty)
                {
                    MessageBox.Show("This account is in use already!");
                }
                else
                {
                    MessageBox.Show("Login successful");
                    if (login != null) login(this, LoginStatus.GetValueOrDefault());
                }
            }
            catch
            {                
                MessageBox.Show("Error while connecting to server!");
            }
        }
    }
}
