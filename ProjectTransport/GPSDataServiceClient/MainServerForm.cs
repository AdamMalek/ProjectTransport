using GPSInterfaces;
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

namespace GPSDataServiceClient
{
    public partial class MainServerForm : Form
    {
        public MainServerForm()
        {
            InitializeComponent();
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            lblServerResponse.Text = "Sending Request...";

            if (txtRequestParam.Text.Trim().Length != 0)
            {
                try
                {
                    ChannelFactory<IWCFGPSDataService> channelFactory = new ChannelFactory<IWCFGPSDataService>("GPSServiceEndpoint");
                    IWCFGPSDataService proxy = channelFactory.CreateChannel();
                    lblServerResponse.Text = "Request Sent";
                    lblServerResponse.Text = proxy.TestServerMethod(txtRequestParam.Text);
                }
                catch
                {
                    lblServerResponse.Text="Error!";
                    MessageBox.Show("Error while connecting to server!");
                }
            }
            else
            {
                lblServerResponse.Text = "Error!";
                MessageBox.Show("Enter valid parameter");
            }
        }
    }
}
