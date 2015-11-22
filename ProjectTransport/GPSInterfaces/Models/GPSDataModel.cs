using System;

namespace GPSInterfaces.Models
{
    public class GPSDataModel
    {
        public int Id { get; set; }
        public GPSPos Position { get; set; }
        public DateTime Time { get; set; }
        public double Height { get; set; }
        public double FuelLevel { get; set; }
    }
}