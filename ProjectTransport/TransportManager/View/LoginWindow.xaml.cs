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
using TransportManager.ViewModel;

namespace TransportManager.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var xx = DataContext;
            ((LoginWindowViewModel)xx).login += LoginWindow_login;
        }

        private void LoginWindow_login(object sender, string userHash)
        {
            IsEnabled = false;
            Visibility = Visibility.Hidden;
            MainWindow mw = new MainWindow(userHash);
            mw.Show();
        }
    }
}
