using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TransportProject.ViewModels;

using ServiceLibrary.ProjectService;
//using GPSDataService.Models;


namespace TransportProject.Views
{
    /// <summary>
    /// Interaction logic for AddEditRoutePoint.xaml
    /// </summary>
    public partial class AddEditRoutePointWindow : Window
    {
        ViewModels.AddEditRoutePointVM _vm;
        public AddEditRoutePointWindow(ViewModels.AddEditRoutePointVM vm)
        {
            _vm = vm;
            DataContext = _vm;
            InitializeComponent();
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            var x = MessageBox.Show("Are you sure you want to delete this cost? This operation cannot be reverted!", "Are You sure?", MessageBoxButton.YesNo);
            if (x == MessageBoxResult.Yes)
            {
                var y = _vm.AdditionalCosts.Remove(_vm.SelectedCost);
                if (y)
                    MessageBox.Show("Deleted!");
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AddEditCostVM vm = new AddEditCostVM();
            AddEditCostWindow w = new AddEditCostWindow(vm);
            var result = w.ShowDialog();

            if (result == true)
            {
                _vm.AddNewCost(vm.Description,vm.Price);
            }
            
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            AddEditCostVM vm = new AddEditCostVM(_vm.SelectedCost);
            AddEditCostWindow w = new AddEditCostWindow(vm);
            var result = w.ShowDialog();

            if (result == true)
            {
                _vm.EditCost(vm.Description,vm.Price);
            }
        }

        private void btnAddFromMap_Click(object sender, RoutedEventArgs e)
        {
            SetGpsPosWindow window = new SetGpsPosWindow(_vm);
            var result = window.ShowDialog();
            if(result == true)
            {
                textBox1_Copy.Value = _vm.Latitude;
                textBox1_Copy3.Value = _vm.Longitude;
            }
        }

        //private void button_Copy2_Click(object sender, RoutedEventArgs e)
        //{
        //    var x = MessageBox.Show("Are you sure you want to delete this?", "Are you sure?", MessageBoxButton.YesNo);
        //    if (x == MessageBoxResult.Yes)
        //    {

        //    }
        //}
    }
}
