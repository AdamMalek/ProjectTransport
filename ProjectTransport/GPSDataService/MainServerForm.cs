using GPSDataService.DAL;
using GPSDataService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPSDataService
{
    public partial class ServiceForm : Form
    {
        ServiceHost host;
        public ServiceForm()
        {
            InitializeComponent();
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            this.Text = "Service Starting...";

            host = new ServiceHost(typeof(WCFGPSDataService));

            host.Open();

            lbLog.Items.Add("Service started");

            this.Text = "Service Started";
            pnStatus.BackColor = Color.Green;
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            this.Text = "Service Stopping...";

            if (host != null)
                host.Close();

            lbLog.Items.Add("Service stopped");

            this.Text = "Service Not Running";
            pnStatus.BackColor = Color.Red;
        }

        private void btnTestWrite_Click(object sender, EventArgs e)
        {
            User user1 = new User("test1", Helpers.MD5Encoder.EncodeMD5("test1"));
            User user2 = new User("test2", Helpers.MD5Encoder.EncodeMD5("test2"));

            Route testRoute = new Route();
            testRoute.RouteName = "Krakow - Katowice";
            testRoute.User = user1;
            testRoute.StartPoint = new GPSPos { Latitude = 23.43f, Longitude = 133.22f };
            testRoute.EndPoint = new GPSPos { Latitude = 35.31f, Longitude = 172.14f };
            GPSData data1 = new GPSData
            {
                Id = 0,
                FuelLevel = 99.2,
                Height = 332,
                Position = new GPSPos { Latitude = 26.33f, Longitude = 162.10f },
                Time = new DateTime(2015, 11, 21, 19, 20, 44)
            };
            data1.AdditionalCosts.Add(new AdditionalCost { Description = "Wjazd na autostrade", Price = 25.22f });
            testRoute.RouteData.Add(data1);
            GPSData data2 = new GPSData
            {
                Id = 1,
                FuelLevel = 94.2,
                Height = 132,
                Position = new GPSPos { Latitude = 30.94f, Longitude = 170.70f },
                Time = new DateTime(2015, 11, 21, 19, 33, 27)
            };
            data2.AdditionalCosts.Add(new AdditionalCost { Description = "Wyjazd z autostrady", Price = 35.22f });
            testRoute.RouteData.Add(data2);

            Route testRoute2 = new Route();
            testRoute2.User = user2;

            testRoute2.RouteName = "Bielsko - Gdansk";
            testRoute2.StartPoint = new GPSPos { Latitude = 23.43f, Longitude = 133.22f };
            testRoute2.EndPoint = new GPSPos { Latitude = 35.31f, Longitude = 172.14f };
            GPSData data11 = new GPSData
            {
                Id = 0,
                FuelLevel = 99.2,
                Height = 332,
                Position = new GPSPos { Latitude = 26.33f, Longitude = 162.10f },
                Time = new DateTime(2015, 11, 21, 19, 20, 44)
            };
            data11.AdditionalCosts.Add(new AdditionalCost { Description = "Wjazd na autostrade", Price = 25.22f });
            testRoute2.RouteData.Add(data11);
            GPSData data21 = new GPSData
            {
                Id = 1,
                FuelLevel = 94.2,
                Height = 132,
                Position = new GPSPos { Latitude = 30.94f, Longitude = 170.70f },
                Time = new DateTime(2015, 11, 21, 19, 33, 27)
            };
            data21.AdditionalCosts.Add(new AdditionalCost { Description = "Wyjazd z autostrady", Price = 35.22f });
            testRoute2.RouteData.Add(data21);

            using (var db = new GPSContext())
            {
                db.Users.Add(user1);
                db.Users.Add(user2);
                db.Routes.Add(testRoute);
                db.Routes.Add(testRoute2);
                db.SaveChanges();
                lbLog.Items.Add("Write test successfull!");
            }

        }

        private void btnTestRead_Click(object sender, EventArgs e)
        {
           // Route r;

            //using (var db = new GPSContext())
            //{
            //    var sr = db.Routes.FirstOrDefault();

            //    r = CreateFromDB(sr);

            //    lbLog.Items.Add("Read test successfull!");

            //    return;
            //}
            //var login = "admin"; var password = "21232f297a57a5a743894a0e4a801fc3";
            //using (var db = new GPSContext())
            //{
            //    User usr = db.Users.FirstOrDefault(user => (user.Login == login && user.Password == password));
            //    if (usr != null)
            //        lbLog.Items.Add("Read test successfull!");

            //}
            using (var db = new GPSContext())
            {
                List<Route> routes = new List<Route>();

                User currentUser = db.Users.FirstOrDefault(u => u.UserId == "21232f297a57a5a743894a0e4a801fc3");
                List<Route> userRoutes = currentUser.Routes.ToList();
                foreach (var route in userRoutes)
                {
                    routes.Add(CreateFromDB(route));
                }
                routes.AsEnumerable();
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
    }
}
