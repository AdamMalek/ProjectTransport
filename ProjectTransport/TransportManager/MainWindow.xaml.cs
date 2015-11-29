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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TransportManager.ViewModel;

namespace TransportManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindow()
        {
            InitializeComponent();
        }
        MainWindowViewModel _vm;
        public MainWindow(Guid guid):this()
        {
            _vm = new MainWindowViewModel(guid);
            _vm.OnLogout += Vm_OnLogout;
            DataContext = _vm;
        }

        private void Vm_OnLogout(object sender)
        {
            //DialogResult = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _vm.Logout.Execute(null);
            Environment.Exit(0);
        }
    }
}
