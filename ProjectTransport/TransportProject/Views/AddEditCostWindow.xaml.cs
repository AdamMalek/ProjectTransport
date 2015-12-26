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

namespace TransportProject.Views
{
    /// <summary>
    /// Interaction logic for AddEditCost.xaml
    /// </summary>
    public partial class AddEditCostWindow : Window
    {
        AddEditCostVM _vm;

        public AddEditCostWindow(AddEditCostVM vm)
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
