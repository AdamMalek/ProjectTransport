using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ServiceLibrary.ProjectService;
using System.Collections.ObjectModel;
using GPSDataService.Models;
//using GPSDataService.Models;

namespace TransportProject.ViewModels
{
    public class AddEditRoutePointVM:INotifyPropertyChanged
    {
        public DateTime Time { get; set; }
        public double FuelLevel { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Height { get; set; }
        public GPSPos Position { get { return new GPSPos { Latitude = Latitude, Longitude = Longitude }; } set { Latitude = value.Latitude; Longitude = value.Longitude; RaisePropertyChange("Longitude"); RaisePropertyChange("Latitude"); } }
        public bool viewModelChanged { get; set; }
        
        public ObservableCollection<AdditionalCost> AdditionalCosts
        {
            get;
            set;
        }

        private AdditionalCost _selectedCost;
        public AdditionalCost SelectedCost { get { return _selectedCost; } set { _selectedCost = value; RaisePropertyChange("SelectedCost"); RaisePropertyChange("isSelected"); } }

        public bool isSelected { get { return SelectedCost != null; } }

        public ICommand DeleteCommand { get; set; }

        public AddEditRoutePointVM()
        {
            SelectedCost = null;
            viewModelChanged = false;
            AdditionalCosts = new ObservableCollection<AdditionalCost>();
            Time = DateTime.Now;
            DeleteCommand = new RelayCommand(Delete,(obj) => SelectedCost != null);
        }

        public AddEditRoutePointVM(GPSData data):this()
        {
            if (data == null) return;
            FuelLevel = data.FuelLevel;
            Height = data.Height;
            Position = new GPSPos();
            Latitude = data.Position.Latitude;
            Longitude = data.Position.Longitude;
            
            foreach (var cost in data.AdditionalCosts)
            {
                AdditionalCosts.Add(new AdditionalCost { Id = cost.Id, Description = cost.Description, Price = cost.Price });
            }
            SelectedCost = AdditionalCosts.FirstOrDefault();
            Time = new DateTime(data.Time.Ticks);   
        }

        private void Delete(object obj)
        {
            AdditionalCosts.Remove(SelectedCost);
        }

        public void AddNewCost(string desc, double price)
        {
            AdditionalCosts.Add(new AdditionalCost { Id = 0,Description = desc, Price = price});
        }

        public void EditCost(string desc, double price)
        {
            SelectedCost.Price = price;
            SelectedCost.Description = desc;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChange(string propName)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(propName));
        }
    }
}
