using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPSDataService.Models
{
    [DataContract(IsReference =true)]
    public class GPSData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public GPSPos Position { get; set; }

        [DataMember]
        public DateTime Time { get; set; }
        [DataMember]
        public double Height { get; set; }
        [DataMember]
        public double FuelLevel { get; set; }
        [DataMember]
        public virtual ICollection<AdditionalCost> AdditionalCosts { get; set; }
        [DataMember]
        public virtual Route Route { get; set; }

        public GPSData()
        {
            AdditionalCosts = new List<AdditionalCost>();
        }
    }
}