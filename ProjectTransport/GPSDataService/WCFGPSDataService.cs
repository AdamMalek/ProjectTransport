using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GPSInterfaces;
using GPSInterfaces.Models;
using GPSInterfaces.DAL;

namespace GPSDataService
{
    public class WCFGPSDataService : IWCFGPSDataService
    {
        public void AddRoute(Route data)
        {
            using (var db = new GPSContext())
            {
                if (DataValid(data))
                {
                    db.Routes.Add(data);
                    db.SaveChanges();
                }
            }
        }

        public Route GetTestRoute()
        {
            using (var db = new GPSContext())
            {
                return CreateFromDB(db.Routes.FirstOrDefault());
            }
        }

        private Route CreateFromDB(Route sr)
        {
            Route r = new Route();

            r.RouteName = sr.RouteName;
            r.RouteId = sr.RouteId;
            r.StartPoint = new GPSPos { Latitude = sr.StartPoint.Latitude, Longitude = sr.StartPoint.Longitude };
            r.EndPoint = new GPSPos { Latitude = sr.EndPoint.Latitude, Longitude = sr.EndPoint.Longitude };



            foreach (var routeData in sr.RouteData)
            {
                GPSData data = new GPSData
                {
                    FuelLevel = routeData.FuelLevel,
                    Id = routeData.Id,
                    Height = routeData.Id,
                    Time = new DateTime(routeData.Time.ToBinary()),
                    Position = new GPSPos { Latitude = routeData.Position.Latitude, Longitude = routeData.Position.Longitude }
                };

                foreach (var cost in routeData.AdditionalCosts)
                {
                    data.AdditionalCosts.Add(new AdditionalCost { Description = cost.Description, Id = cost.Id, Price = cost.Price });
                }

                r.RouteData.Add(data);
            }

            return r;
        }

        private bool DataValid(Route data)
        {
            if (data == null) return false;

            //Verifying if Start and End Points are valid
            if (!isValid(data.StartPoint) || !isValid(data.EndPoint)) return false;

            //Verifying if all route points are valid
            foreach (var singlePoint in data.RouteData)
            {
                if (!isValid(singlePoint)) return false;
            }

            return true;
        }

        private bool isValid(GPSData data)
        {
            if (data.FuelLevel > 100 || data.FuelLevel < 0) return false;
            if (data.Position.Latitude < -90 || data.Position.Latitude > 90) return false;
            if (data.Position.Longitude < -180 || data.Position.Longitude > 180) return false;

            return true;
        }

        private bool isValid(GPSPos pos)
        {
            return !(pos.Latitude < -90 || pos.Latitude > 90 || pos.Longitude < -180 || pos.Longitude > 180);
        }
    }
}
