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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using MapTest.CustomMarkers;
using GPSInterfaces.Models;
using GPSInterfaces.DAL;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Controls.DataVisualization;
using MapTest.MapHelper;
using System.Runtime.InteropServices;
using System.IO;

namespace MapTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    



    public partial class MainWindow : Window
    {
        private List<RouteToDisplay> _routes;
        private List<GMapMarker> _markers;
        private Dictionary<double, double> _valueList;
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        public MainWindow()
        {
            InitializeComponent();
            myMap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            myMap.SetPositionByKeywords("Poland, Bielsko-Biala");
            myMap.DragButton = MouseButton.Left;
        }


        private GMapMarker AddMarker(GPSData point)
        {
            PointLatLng position = new PointLatLng(point.Position.Latitude, point.Position.Longitude);
            GMapMarker tempMarker = new GMapMarker(position);
            tempMarker.Shape = new CustomMarker(this, tempMarker, "Position: " + point.Position.Latitude.ToString() + " " 
                + point.Position.Longitude.ToString() + "\r\nHeight: " + point.Height.ToString() + "\r\nFuel level:" + point.FuelLevel.ToString());

            return tempMarker;
        }

        private GMapMarker AddMarker(Route route)
        {
            PointLatLng position = new PointLatLng(route.EndPoint.Latitude, route.EndPoint.Longitude);
            GMapMarker tempMarker = new GMapMarker(position);
            tempMarker.Shape = new CustomMarker(this, tempMarker, "End: " + route.EndPoint.Latitude.ToString() + " "
                + route.EndPoint.Longitude.ToString());

            return tempMarker;
        }

        private RouteToDisplay AddRoute(GPSData position1, GPSData position2)
        {
            PointLatLng start = new PointLatLng(position1.Position.Latitude, position1.Position.Longitude);
            PointLatLng end = new PointLatLng(position2.Position.Latitude, position2.Position.Longitude);

            RouteToDisplay tempRoute = new RouteToDisplay();

            GDirections directions;
            var route = GMapProviders.GoogleMap.GetDirections(out directions, start, end, true, false, false, false, false);

            tempRoute.Route = new GMapRoute(directions.Route);
            {
                tempRoute.Route.ZIndex = 1;
            }
            tempRoute.StartFuelLevel = position1.FuelLevel;
            tempRoute.EndFuelLevel = position2.FuelLevel;
            tempRoute.StartHeight = position1.Height;
            tempRoute.EndHeight = position2.Height;
            tempRoute.Directions = directions;

            return tempRoute;
        }


        private void DisplayRoute(List<GPSData> points, Route route)
        {
            
            _markers = new List<GMapMarker>();
            _routes = new List<RouteToDisplay>();

            int k = points.Count;

            for (int i = 0; i < k+1; i++)
            {
                if(i!=k)
                    _markers.Add(AddMarker(points[i]));
                if (i == k)
                    _markers.Add(AddMarker(route));

                if(i < k-1)
                    _routes.Add(AddRoute(points[i], points[i + 1]));
            }


            for (int i = 0; i < k+1; i++)
            {
                myMap.Markers.Add(_markers[i]);

                if (i < k - 1)
                {
                    myMap.Markers.Add(_routes[i].Route);
                }
            }

            myMap.ZoomAndCenterMarkers(null);
            rbtnFuelConsumed.IsChecked = true;
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //List<PointLatLng> points = new List<PointLatLng>()
            //{
            //    new PointLatLng(49.82237679999999, 19.05838449999999),
            //    new PointLatLng(49.85489339999999, 19.34128420000002),
            //    new PointLatLng(49.88278560000001, 19.49395789999994),
            //    new PointLatLng(50.06465009999999, 19.94497990000002)
            //};

            List<GPSData> positions;
            Route route;
            using (var db = new GPSContext())
            {
                route = db.Routes.First();
                positions = route.RouteData.ToList();
            }

            DisplayRoute(positions, route);




        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Route testRoute = new Route();
            testRoute.RouteName = "Bielsko - Warszawa";
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

            using (var db = new GPSContext())
            {    
                db.Routes.Add(testRoute);
                db.SaveChanges();
            }

        }





       

        private void rbtnFuelConsumed_Checked(object sender, RoutedEventArgs e)
        {
            _valueList = new Dictionary<double, double>();
            
            for (int i = 0; i < _routes.Count; i++)
            {
                if (_valueList.Count == 0)
                    _valueList.Add(_routes[i].Distance, _routes[i].FuelConsumed);
                else
                    _valueList.Add(_valueList.Keys.ElementAt(i-1) + _routes[i].Distance, _routes[i].FuelConsumed);
            }
            ((LineSeries)lineChart.Series[0]).ItemsSource = _valueList;
            yAxis.Title = "Spalanie [l/100km]";
            chart.Title = "Spalanie [l/100km]";


        }

        private void rbtnHeight_Checked(object sender, RoutedEventArgs e)
        {
            _valueList = new Dictionary<double, double>();

            for (int i = 0; i < _routes.Count; i++)
            {
                if (_valueList.Count == 0)
                    _valueList.Add(0, _routes[i].StartHeight);
                else if (i == _routes.Count - 1)
                {
                    _valueList.Add(_valueList.Keys.ElementAt(i-1) + _routes[i-1].Distance, _routes[i].StartHeight);
                    _valueList.Add(_valueList.Keys.ElementAt(i) + _routes[i].Distance, _routes[i].EndHeight);
                }
                else
                    _valueList.Add(_valueList.Keys.ElementAt(i-1) + _routes[i-1].Distance, _routes[i].StartHeight);
            }
            ((LineSeries)lineChart.Series[0]).ItemsSource = _valueList;
            chart.Title = "Wysokość [mnpm]";
            yAxis.Title = "Wysokość [mnpm]";
        }


        public static FrameworkElement FindDescendantWithName(DependencyObject root, string name)
        {
            var numChildren = VisualTreeHelper.GetChildrenCount(root);

            for (var i = 0; i < numChildren; i++)
            {
                var child = (FrameworkElement)VisualTreeHelper.GetChild(root, i);
                if (child.Name == name)
                {
                    return child;
                }

                var descendantOfChild = FindDescendantWithName(child, name);
                if (descendantOfChild != null)
                {
                    return descendantOfChild;
                }
            }

            return null;
        }


        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            
        }


        private double GetYValue(double X)
        {
            double Y = _valueList.SingleOrDefault(n => n.Key == X).Value;
            return Y;
        }

        private void SetCursor(int x, int y)
        {
            // left boundary
            var xl = (int)App.Current.MainWindow.Left + (int)lineChart.Margin.Left + (int)yAxis.ActualWidth +20;
            // top boundary
            var yt = (int)App.Current.MainWindow.Top + (int)lineChart.Margin.Top + 77;



            SetCursorPos(x + xl, yt+y);
        }

      
        private void lineChart_MouseMove(object sender, MouseEventArgs e)
        {

            this.Cursor = Cursors.Cross;

            var chart = (Chart)sender;
            var xAxisRange = (ICategoryAxis)xAxis;
            var yAxisRange = (IRangeAxis)yAxis;
            double xHit, yHit;

            xAxis.Cursor = Cursors.Cross;
            

            var plotArea = FindDescendantWithName(chart, "PlotArea");
            if (plotArea == null)
            {
                return;
            }

            var mousePositionInPixels = e.GetPosition(plotArea);
            if (mousePositionInPixels.X >= 0 && mousePositionInPixels.X <= plotArea.Width)
                xHit = (double)xAxisRange.GetCategoryAtPosition(new UnitValue(mousePositionInPixels.X, Unit.Pixels));
            else
                xHit = -1;

            

            yHit = GetYValue(xHit);

            var yHitt = (int)plotArea.Height - (int)yAxisRange.GetPlotAreaCoordinate(yHit).Value;


            if (_valueList.ContainsKey(xHit))
            {
                SetCursor((int)mousePositionInPixels.X, yHitt);
            }

            if (xHit != -1)
            {
                
            }




            textBlock.Text = "X value: " + xHit.ToString() + "\r\nY value: " + yHit.ToString() + "\r\nY value in px = " + mousePositionInPixels.Y + "\r\nX value in px = " + mousePositionInPixels.X + "\r\n " + yHitt;
        }
    }
    
}
