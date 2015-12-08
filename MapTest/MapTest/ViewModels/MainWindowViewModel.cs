using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TransportManager.ViewModel;

namespace MapTest.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            DrawMap = new RelayCommand(draw, (obj) => true);
        }

        public ICommand DrawMap { get; set; }



        private void draw(object obj)
        {

        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propName)
        {
            var x = PropertyChanged;
            if (x != null) PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
