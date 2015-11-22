using GPSInterfaces.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportManager.DAL
{
    public class GPSContext: DbContext
    {
        public DbSet<Route> Routes { get; set; }
        public DbSet<AdditionalCost> AdditionalCosts { get; set; }

        public DbSet<GPSDataModel> RouteData{ get; set; }

        public GPSContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GPSContext>());
        }
    }
}
