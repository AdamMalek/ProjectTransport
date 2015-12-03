using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GPSDataService;
using GPSDataService.Models;
using GPSDataService.DAL;

namespace GPSDataService
{
    public class WCFGPSDataService : IRemoteService, IClientService
    {
        public bool AddRoute(Route data)
        {
            using (var db = new GPSContext())
            {
                if (DataValid(data))
                {
                    db.Routes.Add(data);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AddRoutes(IEnumerable<Route> routedata)
        {
            using (var db = new GPSContext())
            {
                foreach (var data in routedata)
                {
                    if (DataValid(data))
                    {
                        db.Routes.Add(data);
                    }
                    else
                    {
                        return false;
                    }
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool Delete(Route route)
        {
            using (var db = new GPSContext())
            {
                Route r = db.Routes.FirstOrDefault(m => m.RouteId == route.RouteId);
                if (r != null)
                {
                    db.Routes.Remove(r);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DeleteRoutes(IEnumerable<Route> data)
        {
            using (var db = new GPSContext())
            {
                foreach (var route in data)
                {
                    Route r = db.Routes.FirstOrDefault(m => m.RouteId == route.RouteId);
                    if (r != null)
                    {
                        db.Routes.Remove(r);
                    }
                    else
                    {
                        return false;
                    }
                }
                db.SaveChanges();
                return true;
            }
        }

        public bool UpdateRoute(Route route)
        {
            using (var db = new GPSContext())
            {
                Route r = db.Routes.FirstOrDefault(m => m.RouteId == route.RouteId);
                if (r != null)
                {
                    r.RouteName = route.RouteName;
                    r.StartPoint = route.StartPoint;
                    r.EndPoint = r.EndPoint;
                    r.RouteData = r.RouteData;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public IEnumerable<Route> GetAllRoutes()
        {
            using (var db = new GPSContext())
            {
                List<Route> routes = new List<Route>();
                User currentUser = null;
                List<Route> userRoutes = db.Routes.Where(r => r.UserId == currentUser.UserId).ToList();
                foreach (var route in userRoutes)
                {
                    routes.Add(CreateFromDB(route));
                }
                return routes.AsEnumerable();
            }
        }

        public Route GetRouteById(int id)
        {
            using (var db = new GPSContext())
            {
                User usr = null;
                Route route = db.Routes.FirstOrDefault(m => m.RouteId == id && m.UserId == usr.UserId);
                if (route != null)
                {
                    route = CreateFromDB(route);
                }
                return route;
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
            if (GetUserById(data.UserId) == null) return false;
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

        private User GetUserById(string userid)
        {
            using (var db = new GPSContext())
            {
                return db.Users.FirstOrDefault(usr => usr.UserId == userid);
            }
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

        public string Login(string login, string password)
        {
            using (var db = new GPSContext())
            {
                User usr = db.Users.FirstOrDefault(user => (user.Login == login && user.Password == password));
                if (usr != null)
                {
                    try
                    {
                        return usr.UserId;

                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
        }

        public bool LogOut()
        {
            //
            return true;
        }


        public string Register(string login, string password)
        {
            using (var db = new GPSContext())
            {
                if (db.Users.FirstOrDefault(n => n.Login == login) != null) return null;
                try
                {
                    User usr = new User(login, password);
                    db.Users.Add(usr);
                    db.SaveChanges();
                    return usr.UserId;
                }
                catch
                {
                    return null;
                }
            }
        }

        public string Test()
        {
            return "Test successful";
        }
    }
}
