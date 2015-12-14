using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GPSDataService.Models
{
    [DataContract]
    public class AdditionalCost
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public float Price { get; set; }
        [DataMember]
        public virtual GPSData RouteData { get; set; }
    }
}
