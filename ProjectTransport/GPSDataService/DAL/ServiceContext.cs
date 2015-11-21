using GPSInterfaces.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSDataService.DAL
{
    public class ServiceContext: DbContext
    {
        public DbSet<Route> Routes { get; set; }

        public ServiceContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ServiceContext>());
        }
    }
}
