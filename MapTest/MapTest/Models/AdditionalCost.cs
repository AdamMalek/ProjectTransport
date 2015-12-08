using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSInterfaces.Models
{
    public class AdditionalCost
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public virtual GPSData RouteData { get; set; }
    }
}
