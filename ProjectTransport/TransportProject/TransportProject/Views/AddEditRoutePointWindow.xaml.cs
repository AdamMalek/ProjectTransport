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

        //private void button_Copy2_Click(object sender, RoutedEventArgs e)
        //{
        //    var x = MessageBox.Show("Are you sure you want to delete this?", "Are you sure?", MessageBoxButton.YesNo);
        //    if (x == MessageBoxResult.Yes)
        //    {
                
        //    }
        //}
    }
}
