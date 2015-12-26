using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServiceLibrary.ProjectService;
using System.ComponentModel;
using GPSDataService.Models;
//using GPSDataService.Models;

namespace TransportProject.ViewModels
{
    public class AddEditCostVM: INotifyPropertyChanged
    {

        string _desc;
        double _price;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Description
        {
            get { return _desc; }
            set
            {
                _desc = value;
                RaisePropertyChange("Description");
                RaisePropertyChange("isDataValid");
            }
        }

        public double Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                RaisePropertyChange("Description");
                RaisePropertyChange("isDataValid");
            }
        }

        public bool isDataValid
        {
            get
            {
                string s = Description.Trim();
                bool x = s.Length > 0 && Price > 0;
                return x;
            }
        }

        public AddEditCostVM()
        {
            Description = "";
            Price = 0;
        }

        public AddEditCostVM(AdditionalCost cost)
        {
            Description = cost.Description;
            Price = cost.Price;
        }

        private void RaisePropertyChange(string propName)
        {
            var x = PropertyChanged;
            if (x != null) x(this, new PropertyChangedEventArgs(propName));
        }
    }
}
