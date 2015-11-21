using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSInterfaces.Models
{
    public class Route
    {
        public int  RouteId { get; set; }
        public GPSPos StartPoint{ get; set; }
        public GPSPos EndPoint{ get; set; }
        public virtual ICollection<AdditionalCost> AdditionalCosts { get; set; }

        public virtual ICollection<GPSDataModel> RouteData { get; set; }
        public Route()
        {
            RouteData = new List<GPSDataModel>();
            AdditionalCosts = new List<AdditionalCost>();
        }
    }
}
