using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TransportProject.ViewModels
{
    public class AddEditRoutePointVM:INotifyPropertyChanged
    {
        public DateTime Time { get; set; }
        public double FuelLevel { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Height { get; set; }
        public ProjectService.GPSPos Position { get { return new ProjectService.GPSPos { Latitude = Latitude, Longitude = Longitude }; } set { Latitude = value.Latitude; Longitude = value.Longitude; RaisePropertyChange("Longitude"); RaisePropertyChange("Latitude"); } }

        List<ProjectService.AdditionalCost> _additionalCosts;


        public List<ProjectService.AdditionalCost> AdditionalCosts { get { return _additionalCosts; } set { _additionalCosts = value; RaisePropertyChange("AdditionalCosts"); } }


        public ProjectService.AdditionalCost SelectedCost { get; set; }


        public ICommand DeleteCommand { get; set; }

        public AddEditRoutePointVM()
        {
            _additionalCosts = new List<ProjectService.AdditionalCost>();
            DeleteCommand = new RelayCommand(Delete,(obj) => SelectedCost != null);
        }

        public AddEditRoutePointVM(ProjectService.GPSData data):this()
        {
            FuelLevel = data.FuelLevel;
            Height = data.Height;
            Position = new ProjectService.GPSPos();
            Latitude = data.Position.Latitude;
            Longitude = data.Position.Longitude;
            foreach (var cost in data.AdditionalCosts)
            {
                AdditionalCosts.Add(new ProjectService.AdditionalCost { Id = cost.Id, Description = cost.Description, Price = cost.Price });
            }
            Time = new DateTime(data.Time.Ticks);   
        }

        private void Delete(object obj)
        {
            AdditionalCosts.Remove(SelectedCost);
        }

        public void AddNewCost(string desc, double price)
        {
            AdditionalCosts.Add(new ProjectService.AdditionalCost {Description = desc, Price = price });
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
