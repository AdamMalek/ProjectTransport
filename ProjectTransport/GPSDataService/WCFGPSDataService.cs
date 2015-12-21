using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using GPSDataService;
using GPSDataService.DAL;
using GPSDataService.Models;
using System.Data.Entity;

namespace GPSDataService
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WCFGPSDataService : IRemoteService, IClientService
    {
        string _userKey;
        User _currentUser;
        bool _isLogged = false;

        public bool RemoteAddRoute(Route route)
        {
            if (isValid(route))
            {
                using (var db = new GPSContext())
                {
                    User user = db.Users.FirstOrDefault(usr => usr.UserId == route.User.UserId);
                    if (user == null)
                    {
                        return false;
                    }
                    else
                    {
                        route.User = user;
                    }

                    db.Routes.Add(route);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool AddRoute(Route route)
        {
            if (!_isLogged) return false;
            if (isValid(route))
            {
                using (var db = new GPSContext())
                {
                    route.User = db.Users.First(usr => usr.UserId == _userKey);
                    db.Routes.Add(route);
                    db.SaveChanges();
                    UpdateUserData();
                    return true;
                }
            }
            return false;
        }

        public bool AddRoutes(IEnumerable<Route> data)
        {
            if (!_isLogged) return false;

            foreach (var route in data)
            {
                if (isValid(route)) return false;
            }

            using (var db = new GPSContext())
            {
                foreach (var route in data)
                {
                    route.User = _currentUser;
                    db.Routes.Add(route);
                    db.SaveChanges();
                }
                UpdateUserData();
                return true;
            }
        }

        public IEnumerable<Route> GetAllRoutes()
        {
            if (!_isLogged) return null;
            return _currentUser.Routes.ToList();
        }

        public Route GetRouteById(int id)
        {
            if (!_isLogged) return null;
            return _currentUser.Routes.FirstOrDefault(route => route.RouteId == id);
        }

        public bool UpdateRoute(Route route)
        {
            if (!_isLogged) return false;

            using (var db = new GPSContext())
            {
                Route dbRoute = db.Routes.FirstOrDefault(r => r.RouteId == route.RouteId);

                if (dbRoute != null)
                {
                    dbRoute.RouteName = route.RouteName;
                    dbRoute.StartPoint = route.StartPoint;
                    dbRoute.EndPoint = route.EndPoint;
                    dbRoute.RouteData = route.RouteData;
                    db.SaveChanges();
                    UpdateUserData();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string Test()
        {
            if (!_isLogged) return "NO_LOGON";

            return _userKey;
        }

        public bool Delete(Route route)
        {
            if (!_isLogged) return false;


            using (var db = new GPSContext())
            {
                Route dbRoute = db.Routes.FirstOrDefault(r => r.RouteId == route.RouteId);

                if (dbRoute != null)
                {
                    db.Routes.Remove(dbRoute);
                    db.SaveChanges();
                    UpdateUserData();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string Login(string login, string password)
        {
            using (var db = new GPSContext())
            {
                User usr = db.Users.Include(x => x.Routes.Select(y => y.RouteData.Select(z => z.AdditionalCosts))).
                    FirstOrDefault(user => (user.Login == login && user.Password == password));

                if (usr != null)
                {
                    _currentUser = usr;
                    _userKey = usr.UserId;
                    _isLogged = true;
                    return _userKey;
                }
                else
                {
                    return null;
                }
            }
        }

        private void UpdateUserData()
        {
            using (var db = new GPSContext())
            {
                _currentUser = db.Users.Include(x => x.Routes.Select(y => y.RouteData.Select(z => z.AdditionalCosts))).
                        FirstOrDefault(user => (user.UserId == _userKey));
            }
        }

        public string Register(string login, string password)
        {
            using (var db = new GPSContext())
            {
                User usr = db.Users.FirstOrDefault(user => (user.Login == login && user.Password == password));
                if (usr == null)
                {
                    usr = new User();
                    usr.Login = login;
                    usr.Password = password;
                    usr.UserId = Helpers.MD5Encoder.EncodeMD5(login);

                    db.Users.Add(usr);
                    db.SaveChanges();

                    _currentUser = usr;
                    _userKey = usr.UserId;
                    _isLogged = true;

                    return _userKey;
                }
                else
                {
                    return null;
                }
            }
        }
        // HELPERS

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

                    r.RouteData.Add(data);
                }
            }
            return r;
        }

        // DATA VALIDATION
        private bool isValid(Route route)
        {
            if (route == null) return false;
            if (!isValid(route.StartPoint) || !isValid(route.EndPoint)) return false;
            foreach (var singlePoint in route.RouteData)
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
