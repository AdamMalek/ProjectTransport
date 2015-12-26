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
        bool _isLogged = false;
        string _validationToken = "";

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
                    route.User = GetUser(db);
                    db.Routes.Add(route);
                    db.SaveChanges();
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
                    route.User = GetUser(db);
                    db.Routes.Add(route);
                    db.SaveChanges();
                }
                return true;
            }
        }

        public IEnumerable<Route> GetAllRoutes()
        {
            if (!_isLogged) return null;
            using (var db = new GPSContext())
            {
                return CreateFromDB(GetUser(db).Routes.ToList());
            }
        }

        public Route GetRouteById(int id)
        {
            if (!_isLogged) return null;
            using (var db = new GPSContext())
            {
                return CreateFromDB(GetRoute(db, id));
            }
        }

        public bool UpdateRoute(Route route)
        {
            if (!_isLogged || !isValid(route)) return false;

            using (var db = new GPSContext())
            {
                Route dbRoute = GetRoute(db, route.RouteId);

                if (dbRoute != null)
                {
                    //foreach (var data in route.RouteData)
                    //{
                    //    if (!isValid(data)) return false;
                    //    GPSData dbData = GetRouteData(db, data.Id);
                    //    if (dbData != null)
                    //    {
                    //        dbData.FuelLevel = data.FuelLevel;
                    //        dbData.Height = data.Height;
                    //        dbData.Position = data.Position;
                    //        dbData.Time = data.Time;

                    //        foreach (var cost in dbData.AdditionalCosts)
                    //        {
                    //            if (!isValid(cost)) return false;
                    //            AdditionalCost dbCost = GetAdditionalCost(db, cost.Id);
                    //            if (dbCost != null)
                    //            {
                    //                dbCost.Price = cost.Price;
                    //                dbCost.Description = cost.Description;
                    //            }
                    //            else
                    //            {
                    //                dbData.AdditionalCosts.Add(cost);
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        dbRoute.RouteData.Add(data);
                    //    }
                    //}
                    dbRoute.RouteName = route.RouteName;
                    dbRoute.StartPoint = route.StartPoint;
                    dbRoute.EndPoint = route.EndPoint;

                    db.SaveChanges();
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
            if (!_isLogged || route == null) return false;


            using (var db = new GPSContext())
            {
                Route dbRoute = GetRoute(db, route.RouteId);

                if (dbRoute != null)
                {
                    while (dbRoute.RouteData.Count > 0)
                    {
                        var dbData = dbRoute.RouteData.FirstOrDefault();   
                        if (dbData != null)
                        {
                            while (dbData.AdditionalCosts.Count > 0)
                            {
                                var dbCost = dbData.AdditionalCosts.First();
                                dbData.AdditionalCosts.Remove(dbCost);
                                db.AdditionalCosts.Remove(dbCost);
                            }
                            dbRoute.RouteData.Remove(dbData);
                            db.RouteData.Remove(dbData);
                        }
                    }
                    db.Routes.Remove(dbRoute);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool AddData(GPSData data)
        {
            if (!_isLogged || !isValid(data)) return false;

            using (var db = new GPSContext())
            {
                Route route = GetRoute(db, data.Route.RouteId);

                if (route != null)
                {
                    data.Route = route;
                    route.RouteData.Add(data);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool UpdateData(GPSData data)
        {
            if (!_isLogged || !isValid(data)) return false;

            using (var db = new GPSContext())
            {
                GPSData dbData = GetRouteData(db, data.Id);
                if (dbData != null)
                {
                    dbData.FuelLevel = data.FuelLevel;
                    dbData.Height = data.Height;
                    dbData.Position = data.Position;
                    dbData.Time = data.Time;

                    foreach (var cost in data.AdditionalCosts)
                    {
                        AdditionalCost dbCost = GetAdditionalCost(db, cost.Id);
                        if (dbCost != null)
                        {
                            dbCost.Price = cost.Price;
                            dbCost.Description = cost.Description;
                        }
                        else
                        {
                            if (!isValid(cost)) return false;
                            cost.RouteData = dbData;
                            dbData.AdditionalCosts.Add(cost);
                        }
                    }

                    var baseIDs = dbData.AdditionalCosts.Select(cost => cost.Id);
                    var dataIDs = data.AdditionalCosts.Select(cost => cost.Id);

                    var diff = baseIDs.Except(dataIDs);

                    while (diff.Count() > 0)
                    {
                        int id = diff.First();
                        AdditionalCost dbCost = GetAdditionalCost(db, id);
                        if (dbCost != null)
                            db.AdditionalCosts.Remove(dbCost);
                    }

                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DeleteData(GPSData data)
        {
            if (!_isLogged || data == null) return false;

            using (var db = new GPSContext())
            {
                Route dbRoute = GetRoute(db, data.Route.RouteId);
                if (dbRoute != null)
                {
                    GPSData dbData = GetRouteData(db, data.Id);
                    if (dbData != null)
                    {
                        while (dbData.AdditionalCosts.Count > 0)
                        {
                            var dbCost = dbData.AdditionalCosts.First();
                            dbData.AdditionalCosts.Remove(dbCost);
                            db.AdditionalCosts.Remove(dbCost);
                        }
                        dbRoute.RouteData.Remove(dbData);
                        db.RouteData.Remove(dbData);
                    }
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string RequestValidationToken()
        {
            if (_validationToken == "AUTHENTICATED") return null;

            _validationToken = Guid.NewGuid().ToString().Replace("-","");

            return _validationToken;
        }

        public string Login(string login, string password)
        {
            if (_isLogged  || _validationToken=="") return null;

            using (var db = new GPSContext())
            {
                User usr = db.Users.FirstOrDefault(user => (user.Login == login));
                if (usr == null || Helpers.MD5Encoder.EncodeMD5(usr.Password + _validationToken) != password) return null;

                if (usr != null)
                {
                    _userKey = usr.UserId;
                    _isLogged = true;
                    _validationToken = "AUTHENTICATED";
                    return _userKey;
                }
                else
                {
                    return null;
                }
            }
        }

        public string Register(string login, string password)
        {
            if (_isLogged) return null;
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
        private User GetUser(GPSContext db)
        {
            if (!_isLogged) return null;
            return db.Users.First(usr => usr.UserId == _userKey);
        }

        private Route GetRoute(GPSContext db, int id)
        {
            if (!_isLogged) return null;
            return db.Routes.FirstOrDefault(r => r.RouteId == id && r.User.UserId == _userKey);
        }

        private GPSData GetRouteData(GPSContext db, int id)
        {
            if (!_isLogged) return null;
            return db.RouteData.FirstOrDefault(r => r.Id == id && r.Route.User.UserId == _userKey);
        }

        private AdditionalCost GetAdditionalCost(GPSContext db, int id)
        {
            if (!_isLogged) return null;
            return db.AdditionalCosts.FirstOrDefault(cost => cost.Id == id && cost.RouteData.Route.User.UserId == _userKey);
        }

        private IEnumerable<Route> CreateFromDB(List<Route> list)
        {
            List<Route> routes = new List<Route>();
            foreach (var r in list)
            {
                routes.Add(CreateFromDB(r));
            }
            return routes;
        }

        private Route CreateFromDB(Route route)
        {
            Route dbRoute = new Route();

            dbRoute.RouteId = route.RouteId;
            dbRoute.RouteName = route.RouteName;
            dbRoute.StartPoint = new GPSPos
            {
                Latitude = route.StartPoint.Latitude,
                Longitude = route.StartPoint.Longitude,
            };
            dbRoute.EndPoint = new GPSPos
            {
                Latitude = route.EndPoint.Latitude,
                Longitude = route.EndPoint.Longitude,
            };

            dbRoute.RouteData = new List<GPSData>();
            foreach (var data in route.RouteData)
            {
                GPSData dbData = new GPSData();
                dbData.Id = data.Id;
                dbData.Height = data.Height;
                dbData.FuelLevel = data.FuelLevel;
                dbData.Time = new DateTime(data.Time.Ticks);
                dbData.Route = dbRoute;
                dbData.Position = new GPSPos()
                {
                    Latitude = data.Position.Latitude,
                    Longitude= data.Position.Longitude
                };
                dbData.AdditionalCosts = new List<AdditionalCost>();
                foreach (var cost in data.AdditionalCosts)
                {
                    AdditionalCost dbCost = new AdditionalCost();
                    dbCost.Id = cost.Id;
                    dbCost.Price = cost.Price;
                    dbCost.Description = cost.Description;
                    dbCost.RouteData = dbData;
                    dbData.AdditionalCosts.Add(dbCost);
                }
                dbRoute.RouteData.Add(dbData);
            }

            return dbRoute;
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
            foreach (var cost in data.AdditionalCosts)
            {
                if (!isValid(cost)) return false;
            }

            return true;
        }

        private bool isValid(GPSPos pos)
        {
            return !(pos.Latitude < -90 || pos.Latitude > 90 || pos.Longitude < -180 || pos.Longitude > 180);
        }

        private bool isValid(AdditionalCost cost)
        {
            return (cost.Description.Trim().Count() > 0 && cost.Price > 0);
        }
    }
}
