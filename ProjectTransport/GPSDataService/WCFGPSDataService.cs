using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GPSInterfaces;
using GPSInterfaces.Models;
using GPSDataService.DAL;

namespace GPSDataService
{
    public class WCFGPSDataService : IWCFGPSDataService
    {
        public void AddRoute(Route data)
        {
            using (var db = new ServiceContext())
            {
                if (DataValid(data))
                {
                    db.Routes.Add(data);
                    db.SaveChanges();
                }
            }
        }

        public string TestServerMethod(string param)
        {
            if (param.Contains("haha"))
                return "XD";
            else
                return "Hello!";
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

        private bool isValid(GPSDataModel data)
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
