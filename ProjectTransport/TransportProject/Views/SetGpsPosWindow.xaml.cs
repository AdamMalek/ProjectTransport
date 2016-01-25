using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using MapTest.CustomMarkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TransportProject.ViewModels;

namespace TransportProject.Views
{
    /// <summary>
    /// Interaction logic for SetGpsPosWindow.xaml
    /// </summary>
    public partial class SetGpsPosWindow : Window
    {
        GMapMarker marker1, marker2;
        AddEditRoutePointVM _vm;
        public SetGpsPosWindow(AddEditRoutePointVM vm)
        {
            InitializeComponent();
            map.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            map.SetPositionByKeywords("Warsaw, Poland");
            map.DragButton = MouseButton.Right;
            _vm = vm;
            SetMarkerPos();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            GDirections directions;
            var route = GMapProviders.GoogleMap.GetDirections(out directions, marker1.Position, marker2.Position, true, false, false, false, false);

            if(directions == null)
            {
                MessageBox.Show("Invalid GPS position!");
            }
            else
            {
                _vm.Latitude = marker1.Position.Lat;
                _vm.Longitude = marker1.Position.Lng;
                DialogResult = true;
                
            }

        }

        private void SetMarkerPos()
        {
            PointLatLng pos1 = new PointLatLng
            {
                Lat = 52.2296756,
                Lng = 21.012228700000037
            };

            PointLatLng pos2 = new PointLatLng
            {
                Lat = 50.064650099999994,
                Lng = 19.9449799
            };
            marker1 = new GMapMarker(pos1);
            marker1.Shape = new CustomMarkerGreen(this, marker1, "Move me around!");

            marker2 = new GMapMarker(pos2);

            map.Markers.Add(marker1);
            map.Markers.Add(marker2);
            
        }
    }
}
