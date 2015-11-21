using GPSDataService.DAL;
using GPSInterfaces.Models;
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
            Route testRoute = new Route();
            testRoute.StartPoint = new GPSPos { Latitude = 23.43f, Longitude = 133.22f };
            testRoute.EndPoint = new GPSPos { Latitude = 35.31f, Longitude = 172.14f };
            testRoute.AdditionalCosts.Add(new AdditionalCost { Description = "Wjazd na autostrade", Price = 25.22f });
            testRoute.AdditionalCosts.Add(new AdditionalCost { Description = "Wyjazd z autostrady", Price = 35.22f });
            testRoute.RouteData.Add(new GPSDataModel
            {
                Id = 0,
                FuelLevel = 99.2,
                Height = 332,
                Position = new GPSPos { Latitude = 26.33f, Longitude = 162.10f },
                Time = new DateTime(2015, 11, 21, 19, 20, 44)
            });

            testRoute.RouteData.Add(new GPSDataModel
            {
                Id = 1,
                FuelLevel = 94.2,
                Height = 132,
                Position = new GPSPos { Latitude = 30.94f, Longitude = 170.70f },
                Time = new DateTime(2015, 11, 21, 19, 33, 27)
            });

        
                using (var db = new GPSContext())
                {
                    db.Routes.Add(testRoute);
                    db.SaveChanges();
                    lbLog.Items.Add("Write test successfull!");
                }

        }
    }
}
