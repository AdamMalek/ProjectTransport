using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSInterfaces.Models
{
    public class Route
    {
        public GPSPos StartPoint{ get; set; }
        public GPSPos EndPoint{ get; set; }
        public List<AdditionalCost> AdditionalCosts { get; set; }

        public List<GPSDataModel> RouteData { get; set; }
        public Route()
        {
            RouteData = new List<GPSDataModel>();
        }
    }
}
