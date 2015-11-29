using GPSInterfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GPSInterfaces
{
    [ServiceContract]
    public interface IWCFGPSDataService
    {
        [OperationContract]
        bool AddRoute(Route data);

        [OperationContract]
        bool AddRoutes(IEnumerable<Route> data);

        [OperationContract]
        IEnumerable<Route> GetAllRoutes(string userHash);

        [OperationContract]
        Route GetRouteById(string userHash, int id);

        [OperationContract]
        bool UpdateRoute(Route route);

        [OperationContract]
        string Test();

        [OperationContract]
        bool Delete(Route route);

        [OperationContract]
        bool Login(string login, string password);

        [OperationContract]
        bool Register(string login, string password);

        [OperationContract]
        bool DeleteRoutes(IEnumerable<Route> data);        
    }
}
