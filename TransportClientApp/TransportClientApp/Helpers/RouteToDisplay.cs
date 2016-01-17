using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapTest.MapHelper
{
    public class RouteToDisplay
    {
        public GMapRoute Route { get; set; }
        public GDirections Directions;

        public double StartFuelLevel;
        public double EndFuelLevel;
        public double StartHeight;
        public double EndHeight;

        public double FuelConsumed
        {
            get
            {
                return (StartFuelLevel - EndFuelLevel) / Distance * 100;
            }
        }

        public double Distance
        {
            get
            {
                return Directions.DistanceValue / 1000;
            }

        }
        public RouteToDisplay()
        {

        }
    }
}
