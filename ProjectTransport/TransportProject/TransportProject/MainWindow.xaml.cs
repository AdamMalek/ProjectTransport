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

                if (_vm.RegisterRoute(r, eRegisterMethod.Add))
                    MessageBox.Show("Added route: " + vm.RouteName);
                else
                    MessageBox.Show("Error!");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _vm.CloseConnection();
        }

        private void btnEditRoute_Click(object sender, RoutedEventArgs e)
        {
            AddEditRouteVM vm = new AddEditRouteVM(_vm.SelectedRoute);
            Views.AddEditRouteWindow wnd = new Views.AddEditRouteWindow(vm);
            if (wnd.ShowDialog() == true)
            {
                ProjectService.Route r = _vm.SelectedRoute;
                r.RouteName = vm.RouteName;
                r.StartPoint = vm.StartPoint;
                r.EndPoint = vm.EndPoint;

                if (_vm.RegisterRoute(r, eRegisterMethod.Update))
                    MessageBox.Show("Edited route: " + vm.RouteName);
                else
                    MessageBox.Show("Error!");
            }
        }

        private void btnDeleteRoute_Click(object sender, RoutedEventArgs e)
        {
            var x = MessageBox.Show("Are you sure you want to delete this route? This operation cannot be reverted!", "Are You sure?", MessageBoxButton.YesNo);
            if (x == MessageBoxResult.Yes)
            {
                var y = _vm.RegisterRoute(_vm.SelectedRoute, eRegisterMethod.Remove);
                if (y)
                    MessageBox.Show("Deleted!");
            }
        }

        private void btnAddGPSData_Click(object sender, RoutedEventArgs e)
        {
            var vm = new AddEditRoutePointVM();
            Views.AddEditRoutePointWindow w = new Views.AddEditRoutePointWindow(vm);
            var x = w.ShowDialog();
            if (x == true)
            {
                ProjectService.GPSData newData = new ProjectService.GPSData
                {
                    AdditionalCosts = vm.AdditionalCosts.ToArray(),
                    FuelLevel = vm.FuelLevel,
                    Height = vm.Height,
                    Position = vm.Position,
                    Time = vm.Time
            };
                var tmp = _vm.SelectedRoute.RouteData.ToList();
                tmp.Add(newData);
                _vm.SelectedRoute.RouteData = tmp.ToArray();
                _vm.RegisterRoute(_vm.SelectedRoute, eRegisterMethod.Update);
            }
        }

        private void btnEditGPSData_Click(object sender, RoutedEventArgs e)
        {
            var vm = new AddEditRoutePointVM(_vm.SelectedGPSData);
            Views.AddEditRoutePointWindow w = new Views.AddEditRoutePointWindow(vm);
            var x = w.ShowDialog();
            if (x == true)
            {
                _vm.SelectedGPSData.AdditionalCosts = vm.AdditionalCosts.ToArray();
                _vm.SelectedGPSData.FuelLevel = vm.FuelLevel;
                _vm.SelectedGPSData.Height = vm.Height;
                _vm.SelectedGPSData.Position = vm.Position;
                _vm.SelectedGPSData.Time = vm.Time;
                _vm.RegisterRoute(_vm.SelectedRoute, eRegisterMethod.Update);
            }
        }
    }
}
