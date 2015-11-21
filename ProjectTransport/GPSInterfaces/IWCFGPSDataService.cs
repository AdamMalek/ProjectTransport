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
        void AddRoute(Models.Route data);

        [OperationContract]
        string TestServerMethod(string param);
    }
}
