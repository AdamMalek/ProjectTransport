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

using ServiceLibrary.ProjectService;
using Microsoft.Win32;
using GPSDataService.Models;
using TransportProject.Views;
//using GPSDataService.Models;

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
                Route r = new Route();
                r.RouteName = vm.RouteName;
                r.StartPoint = vm.StartPoint;
                r.EndPoint = vm.EndPoint;
                r.RouteData = new List<GPSData>().ToArray();

                if (_vm.RegisterRoute(r, eRouteRegisterMethod.Add))
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
                Route r = _vm.SelectedRoute;
                r.RouteName = vm.RouteName;
                r.StartPoint.Latitude = vm.StartPoint.Latitude;
                r.StartPoint.Longitude = vm.StartPoint.Longitude;
                r.EndPoint.Latitude = vm.EndPoint.Latitude;
                r.EndPoint.Longitude = vm.EndPoint.Longitude;

                if (_vm.RegisterRoute(r, eRouteRegisterMethod.Update))
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
                var y = _vm.RegisterRoute(_vm.SelectedRoute, eRouteRegisterMethod.Remove);
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
                GPSData newData = new GPSData
                {
                    AdditionalCosts = vm.AdditionalCosts.ToArray(),
                    FuelLevel = vm.FuelLevel,
                    Height = vm.Height,
                    Position = vm.Position,
                    Time = vm.Time,
                    Route = _vm.SelectedRoute
            };
                var tmp = _vm.SelectedRoute.RouteData.ToList();
                tmp.Add(newData);
                _vm.SelectedRoute.RouteData = tmp.ToArray();
                _vm.RegisterData(newData, eDataRegisterMethod.Add);
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
                _vm.RegisterData(_vm.SelectedGPSData, eDataRegisterMethod.Update);
            }
        }

        private void btnDeleteGPSData_Click(object sender, RoutedEventArgs e)
        {
            var x = MessageBox.Show("Are you sure you want to delete this point? This operation cannot be reverted!", "Are You sure?", MessageBoxButton.YesNo);
            if (x == MessageBoxResult.Yes)
            {
                var y = _vm.RegisterData(_vm.SelectedGPSData, eDataRegisterMethod.Remove);
                if (y)
                    MessageBox.Show("Deleted!");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = ".XLS files| *.xls";
            dialog.Title = "Show location to save exported data.";
            var dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                if (_vm.Export(dialog.FileName) == true) MessageBox.Show("Completed!");
                else MessageBox.Show("Error during exporting!");
            }
        }

        private void listBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MapWindow mapWindow = new MapWindow(_vm.SelectedRoute);
            if(mapWindow.myMap.Markers.Count != 0)
            {
                mapWindow.Show();
            }
           
        }

        private void listBox2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MapWindow mapWindow = new MapWindow(_vm.SameNameRoutes);
            if (mapWindow.myMap.Markers.Count != 0)
            {
                mapWindow.Show();
            }
        }
    }
}
