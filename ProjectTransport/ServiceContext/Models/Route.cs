using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GPSDataService.Models
{
    [DataContract(IsReference =true)]
    public class Route
    {
        [DataMember]
        public int  RouteId { get; set; }

        [DataMember]
        public string RouteName { get; set; }

        [DataMember]
        public GPSPos StartPoint{ get; set; }

        [DataMember]
        public GPSPos EndPoint{ get; set; }

        [DataMember]
        public virtual User User { get; set; }

        [DataMember]
        public virtual ICollection<GPSData> RouteData { get; set; }
        public Route()
        {
            RouteData = new List<GPSData>();
        }
    }
}
