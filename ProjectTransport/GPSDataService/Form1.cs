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


            this.Text = "Service Started";
            pnStatus.BackColor = Color.Green;

        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            this.Text = "Service Stopping...";

            if (host != null)
                host.Close();

            this.Text = "Service Not Running";
            pnStatus.BackColor = Color.Red;
        }
    }
}
