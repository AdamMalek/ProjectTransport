using GPSDataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GPSDataService
{
    [ServiceContract]
    public interface IRemoteService
    {
        [OperationContract]
        bool RemoteAddRoute(Route data);       
    }

    [ServiceContract
        (SessionMode= SessionMode.Required)
    ]
    public interface IClientService
    {
        [OperationContract]
        bool AddRoute(Route data);

        [OperationContract]
        bool AddRoutes(IEnumerable<Route> data);

        [OperationContract]
        IEnumerable<Route> GetAllRoutes();

        [OperationContract]
        Route GetRouteById(int id);

        [OperationContract]
        bool UpdateRoute(Route route);

        [OperationContract]
        string Test();

        [OperationContract]
        bool Delete(Route route);

        [OperationContract]
        string Login(string login, string password);

        [OperationContract]
        string Register(string login, string password);
    }
}
