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
    /// Interaction logic for AddEditRouteWindow.xaml
    /// </summary>
    public partial class AddEditRouteWindow : Window
    {
        ViewModels.AddEditRouteVM _vm;
        public AddEditRouteWindow(ViewModels.AddEditRouteVM vm)
        {
            _vm = vm;
            DataContext = _vm;
            InitializeComponent();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
