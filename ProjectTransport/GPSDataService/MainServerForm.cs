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
            User user1 = new User("test1", "test1");
            User user2 = new User("test2", "test2");

            Route testRoute = new Route();
            testRoute.RouteName = "Bielsko - Warszawa";
            testRoute.User = user1;
            testRoute.StartPoint = new GPSPos { Latitude = 49.82237679999999, Longitude = 19.05838449999999 };
            testRoute.EndPoint = new GPSPos { Latitude = 52.2296756, Longitude = 21.012228700000037 };

            GPSData data1 = new GPSData
            {
                Id = 0,
                FuelLevel = 99.2,
                Height = 332,
                Position = new GPSPos { Latitude = 49.82237679999999, Longitude = 19.05838449999999 },
                Time = new DateTime(2015, 12, 1, 11, 20, 44)
            };
            data1.AdditionalCosts.Add(new AdditionalCost { Description = "Wjazd na autostrade", Price = 25.22f });
            testRoute.RouteData.Add(data1);
            GPSData data2 = new GPSData
            {
                Id = 1,
                FuelLevel = 94.2,
                Height = 125,
                Position = new GPSPos { Latitude = 49.85489339999999, Longitude = 19.34128420000002 },
                Time = new DateTime(2015, 12, 1, 12, 00, 44)
            };
            testRoute.RouteData.Add(data2);

            GPSData data3 = new GPSData
            {
                Id = 2,
                FuelLevel = 92.2,
                Height = 255,
                Position = new GPSPos { Latitude = 49.88278560000001, Longitude = 19.49395789999994 },
                Time = new DateTime(2015, 12, 1, 12, 20, 44)
            };
            testRoute.RouteData.Add(data3);
            GPSData data4 = new GPSData
            {
                Id = 3,
                FuelLevel = 84.2,
                Height = 66,
                Position = new GPSPos { Latitude = 50.06465009999999, Longitude = 19.94497990000002 },
                Time = new DateTime(2015, 12, 1, 12, 25, 27)
            };
            data4.AdditionalCosts.Add(new AdditionalCost { Description = "Wyjazd z autostrady", Price = 35.22f });
            testRoute.RouteData.Add(data4);

            GPSData data5 = new GPSData
            {
                Id = 4,
                FuelLevel = 73.2,
                Height = 455,
                Position = new GPSPos { Latitude = 50.8660773, Longitude = 20.628567699999962 },
                Time = new DateTime(2015, 12, 1, 12, 40, 27)
            };
            testRoute.RouteData.Add(data5);

            GPSData data6 = new GPSData
            {
                Id = 5,
                FuelLevel = 60.2,
                Height = 222,
                Position = new GPSPos { Latitude = 51.40272359999999, Longitude = 21.14713329999995 },
                Time = new DateTime(2015, 12, 1, 12, 40, 27)
            };
            testRoute.RouteData.Add(data6);

            Route testRoute2 = new Route();
            testRoute2.User = user2;

            testRoute2.RouteName = "Bielsko - Gdansk";
            testRoute2.StartPoint = new GPSPos { Latitude = 49.82237679999999, Longitude = 19.05838449999999 };
            testRoute2.EndPoint = new GPSPos { Latitude = 54.35202520000001, Longitude = 18.64663840000003 };
            GPSData data10 = new GPSData
            {
                Id = 0,
                FuelLevel = 115,
                Height = 477,
                Position = new GPSPos { Latitude = 49.82237679999999, Longitude = 19.05838449999999 },
                Time = new DateTime(2016, 1, 19, 9, 30, 44)
            };
            data10.AdditionalCosts.Add(new AdditionalCost { Description = "Wjazd na autostrade", Price = 25.22f });
            testRoute2.RouteData.Add(data10);

            GPSData data11 = new GPSData
            {
                Id = 1,
                FuelLevel = 105,
                Height = 250,
                Position = new GPSPos { Latitude = 50.26489189999999, Longitude = 19.02378150000004 },
                Time = new DateTime(2016, 1, 19, 10, 20, 44)
            };
            data11.AdditionalCosts.Add(new AdditionalCost { Description = "Wjazd na autostrade", Price = 25.22f });
            testRoute2.RouteData.Add(data11);
            GPSData data21 = new GPSData
            {
                Id = 2,
                FuelLevel = 94.2,
                Height = 132,
                Position = new GPSPos { Latitude = 50.8118195, Longitude = 19.120309399999996 },
                Time = new DateTime(2016, 1, 19, 11, 33, 27)
            };
            data21.AdditionalCosts.Add(new AdditionalCost { Description = "Wyjazd z autostrady", Price = 35.22f });
            testRoute2.RouteData.Add(data21);
            GPSData data31 = new GPSData
            {
                Id = 3,
                FuelLevel = 66,
                Height = 333,
                Position = new GPSPos { Latitude = 51.7592485, Longitude = 19.45598330000007 },
                Time = new DateTime(2016, 1, 19, 14, 33, 27)
            };
            data31.AdditionalCosts.Add(new AdditionalCost { Description = "Wyjazd z autostrady", Price = 11.44f });
            testRoute2.RouteData.Add(data31);
            GPSData data41 = new GPSData
            {
                Id = 4,
                FuelLevel = 54,
                Height = 270,
                Position = new GPSPos { Latitude = 52.230618, Longitude = 19.364278000000013 },
                Time = new DateTime(2016, 1, 19, 16, 11, 27)
            };
            data41.AdditionalCosts.Add(new AdditionalCost { Description = "Wyjazd z autostrady", Price = 22.22f });
            testRoute2.RouteData.Add(data41);
            GPSData data51 = new GPSData
            {
                Id = 5,
                FuelLevel = 35,
                Height = 70,
                Position = new GPSPos { Latitude = 53.0137902, Longitude = 18.59844369999996 },
                Time = new DateTime(2016, 1, 19, 18, 33, 27)
            };
            data51.AdditionalCosts.Add(new AdditionalCost { Description = "Wyjazd z autostrady", Price = 30.22f });
            testRoute2.RouteData.Add(data51);
            

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
