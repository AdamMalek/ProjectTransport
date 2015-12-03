using System;
using System.Collections.Generic;

namespace GPSDataService.Models
{
    public class GPSData
    {
        public int Id { get; set; }
        public GPSPos Position { get; set; }
        public DateTime Time { get; set; }
        public double Height { get; set; }
        public double FuelLevel { get; set; }
        public virtual ICollection<AdditionalCost> AdditionalCosts { get; set; }
        public virtual Route Route { get; set; }

        public GPSData()
        {
            AdditionalCosts = new List<AdditionalCost>();
        }
    }
}