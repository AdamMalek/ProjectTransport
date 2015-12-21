using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TransportProject.ViewModels;

namespace TransportProject
{
    public partial class MainWindow : Window
    {
        MainWindowVM _vm;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = (MainWindowVM)DataContext;
        }

        private void OnLogin(eLoginStatus status)
        {
            if (status == eLoginStatus.LoginError)
            {
                MessageBox.Show("Login failed!");
            }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }

        private void btnAddRoute_Click(object sender, RoutedEventArgs e)
        {
            AddEditRouteVM vm = new AddEditRouteVM();
            Views.AddEditRouteWindow wnd = new Views.AddEditRouteWindow(vm);
            if (wnd.ShowDialog() == true)
            {
                ProjectService.Route r = new ProjectService.Route();
                r.RouteName = vm.RouteName;
                r.StartPoint = vm.StartPoint;
                r.EndPoint = vm.EndPoint;
                r.RouteData = new List<ProjectService.GPSData>().ToArray();

                if (_vm.RegisterRoute(r))
                    MessageBox.Show("Added route: " + vm.RouteName);
                else
                    MessageBox.Show("Error!");
            }
            else
            {

            }

        }
    }
}
