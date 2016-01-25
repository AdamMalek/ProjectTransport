using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MapTest.MapHelper;
using GMap.NET.WindowsPresentation;
using System.Runtime.InteropServices;
using MapTest.Charting;
using System.Windows.Controls.DataVisualization.Charting;
using GMap.NET;
using System.Windows.Controls.DataVisualization;
using MapTest.CustomMarkers;
using GMap.NET.MapProviders;
using GPSDataService.Models;
using TransportProject.Helpers;
using System.Threading.Tasks;

namespace TransportProject.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    enum OptimalRoute
    {
        FastestRoute,
        NoTollRoute
    }

    public partial class MapWindow : MetroWindow
    {
        private List<RouteToDisplay> _routes;
        private List<GMapMarker> _markers;
        private Dictionary<double, double> _valueList;
        private List<RoutePosValue> _positionValues;
        private double RouteDistance;
        private Route _route;
        public List<Route> routes;    
        private List<AdditionalCost> costs;
        bool canWork = true;
        private List<RouteInfo> _routeInfo; 

        GMapHelper gMapHelper;
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        public MapWindow(Route route)
        {
            InitializeComponent();
            myMap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            _route = route;
            cbxRoutes.IsEnabled = false;
            btnFastestRoute.IsEnabled = false;
            btnCheapestRoute.IsEnabled = false;
            myMap.DragButton = MouseButton.Left;
            DoMagic();
        }

        public MapWindow(List<Route> sameNameRoutes)
        {
            InitializeComponent();
            myMap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            myMap.DragButton = MouseButton.Left;
            routes = sameNameRoutes;
            _route = sameNameRoutes[0];
            cbxRoutes.ItemsSource = routes;
            GetRoutesInfo();
        }

        //funkcja, ktora pobiera informacje o drogach przekazanych w konstruktorze w celu ustalenia drog optymalnych.
        private void GetRoutesInfo()
        {
            costs = new List<AdditionalCost>();
            _routeInfo = new List<RouteInfo>();
            double fuelConsumed;

            for (int i = 0; i < routes.Count; i++)
            {
                if (costs.Count != 0)
                {
                    costs.Clear();

                }
                costs = routes[i].RouteData.SelectMany(n => n.AdditionalCosts).Distinct().ToList();
                fuelConsumed =  routes[i].RouteData.ElementAt(0).FuelLevel - routes[i].RouteData.ElementAt(routes[i].RouteData.Count - 1).FuelLevel;

                _routeInfo.Add(new RouteInfo
                {
                    RouteId = i+1,
                    CostsSum = costs.Sum(n => n.Price) + fuelConsumed * 4.0,
                    RouteTime = routes[i].RouteData.ElementAt(routes[i].RouteData.Count - 1).Time.Subtract(routes[i].RouteData.ElementAt(0).Time)
                });
            }
            
        }


        private void DoMagic()
        {
            List<GPSData> positions = new List<GPSData>();
            
            if (positions.Count != 0)
                positions.Clear();
            positions = _route.RouteData.ToList();

           if (positions == null || positions.Count < 3)
                canWork = false;

           if (canWork == true)
            {
                gMapHelper = new GMapHelper(this);
                DisplayRoute(positions, _route, gMapHelper);

                if(canWork == true)
                {
                    ChartHelper chartHelper = new ChartHelper(_routes);
                    _positionValues = new List<RoutePosValue>();
                    if (_positionValues.Count != 0)
                        _positionValues.Clear();

                    _positionValues = chartHelper.InitializePosValueList();
                    GMapMarker marker = new GMapMarker(_positionValues[0].Position);
                    marker.Shape = new DisplayMarker(this, marker);
                    myMap.Markers.Add(marker);
                    //rbtnFuelConsumed.IsChecked = true;
                    rbtnFuelConsumed.IsChecked = true;
                    FillTbInfo();
                }
                else if(canWork == false)
                {
                    MessageBox.Show("Gps data invalid!");
                    this.Close();
                }
            }
            else if(canWork == false)
            {
                MessageBox.Show("Gps data invalid!");
                this.Close();
            }
        }

        //funkcja która wypełnia textblocka z informacjami o szczegółach trasy.
        private void FillTbInfo()
        {
            string start, end;
            double avgSpeed, time1;
            TimeSpan time;

            PointLatLng startPoint = _routes[0].Directions.Route[0];
            PointLatLng endPoint = new PointLatLng(_route.EndPoint.Latitude, _route.EndPoint.Longitude);

            start = _route.RouteName.Split('-').ElementAt(0);
            end = _route.RouteName.Split('-').ElementAt(1);
            GDirections directions;
            RouteDistance = _routes.Sum(n=>n.Distance);
            

            time = _route.RouteData.ElementAt(_route.RouteData.Count - 1).Time.Subtract(_route.RouteData.ElementAt(0).Time);
            time1 = time.TotalHours;
            avgSpeed = RouteDistance / time1;

            tbRouteInfo.Text = "Start: " + start + "\r\nKoniec: " + end + "\r\nCałkowity dystans: " + Math.Round(RouteDistance,2) 
                + "km\r\nCzas jazdy: " + time + "h\r\nŚrednia prędkość: " + Math.Round(avgSpeed,2) + "km/h";

        }
        private void DisplayRoute(List<GPSData> points, Route route, GMapHelper helper)
        {
            _markers = new List<GMapMarker>();
            _routes = new List<RouteToDisplay>();
            if(_markers.Count != 0 && _routes.Count != 0 )
            {
                _markers.Clear(); _routes.Clear();
            }
            _markers = helper.SetMarkersList(points, route);
            _routes = helper.SetRoutesList(points, route);

            if(_routes == null)
            {
                canWork = false;              
            }

            else
            {
                int k = points.Count;
                int p = myMap.Markers.Count;

                if(p != 0)
                {
                    myMap.Markers.Clear();
                    //for (int i = 0; i < p; i++)
                    //{
                    //    myMap.Markers.RemoveAt(0);
                        
                    //}
                }

                for (int i = 0; i < k; i++)
                {
                    myMap.Markers.Add(_markers[i]);

                    if (i < k - 1)
                    {
                        myMap.Markers.Add(_routes[i].Route);
                    }
                }



                myMap.ZoomAndCenterMarkers(null);
            }
            
        }

        //wypełnianie wykresów
        private void rbtnFuelConsumed_Checked(object sender, RoutedEventArgs e)
        {
            _valueList = new Dictionary<double, double>();
            if (_valueList.Count != 0)
                _valueList.Clear();

            for (int i = 0; i < (_positionValues.Count - _routes[0].Directions.Route.Count); i++)
            {
                _valueList.Add(_positionValues[i + _routes[0].Directions.Route.Count].Distance, _positionValues[i + _routes[0].Directions.Route.Count].FuelConsumed);
            }
            ((LineSeries)lineChart.Series[0]).ItemsSource = _valueList;
            yAxis.Title = "Spalanie [l/100km]";
            chart.Title = "Spalanie [l/100km]";
        }
        //wypełnianie wykresów
        private void rbtnHeight_Checked(object sender, RoutedEventArgs e)
        {
            _valueList = new Dictionary<double, double>();
            if (_valueList.Count != 0)
                _valueList.Clear();

            for (int i = 0; i < _positionValues.Count; i++)
            {
                _valueList.Add(_positionValues[i].Distance, _positionValues[i].Height);
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

        private void SetCursor(int x, int y)
        {
            // left boundary
            var xl = (int)this.Left + (int)lineChart.Margin.Left + (int)yAxis.ActualWidth + 12;
            // top boundary
            var yt = (int)this.Top + (int)lineChart.Margin.Top + 77;



            SetCursorPos(x + xl, yt + y);
        }



        private double GetYValue(double X)
        {
            double Y = _valueList.SingleOrDefault(n => n.Key == X).Value;
            return Y;
        }


        //metoda do ustawiania pozycji markera, ktory bedzie "jezdzil" po mapie 
        private void SetDisplayMarker(double X)
        {
            int markersCount = _routes.Count + 3;
            PointLatLng position = GetPosFromX(X);
            GMapMarker tempMarker = new GMapMarker(position);
            tempMarker.Shape = new DisplayMarker(this, tempMarker);

           
            if(myMap.Markers.ElementAt(myMap.Markers.Count-1).ZIndex == 2)
            {
                myMap.Markers.RemoveAt(myMap.Markers.Count - 2);
                myMap.Markers.Add(tempMarker);
            }           
            else
            {
                myMap.Markers.RemoveAt(myMap.Markers.Count - 1);
                myMap.Markers.Add(tempMarker);
            }
            
            


        }


        private PointLatLng GetPosFromX(double X)
        {
            PointLatLng tempPoint;
            tempPoint = _positionValues.First(n => n.Distance == X).Position;
            return tempPoint;
        }


        //event odpowiadajacy za zczytywanie pozycji myszki z wykresu i przenoszenie markera na mapie. oraz za wypelnianie textblocka z aktualnym info
        private void lineChart_MouseMove(object sender, MouseEventArgs e)
        {

            var chart = (Chart)sender;
            var xAxisRange = (ICategoryAxis)xAxis;
            var yAxisRange = (IRangeAxis)yAxis;
            double xHit, yHit;





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
                SetDisplayMarker(xHit);
            }

            string s;
            if (rbtnFuelConsumed.IsChecked == true)
                s = "Spalanie [l/100km] = ";
            else
                s = "Wysokość[mnpm] = ";

            //wypelnianie textblocka z informacjami.
            tbChartInfo.Text = "Dystans[km] = " + Math.Round(xHit, 2).ToString() + "\t" + s + Math.Round(yHit, 2).ToString();
            if(xHit==-1)
                tbChartInfo.Text = "Dystans[km] = 0" + "\t" + s + Math.Round(yHit, 2).ToString();

        }


        //wyznaczanie drogi optymalnej
        //private void btnGetOptimalRoute_Click(object sender, RoutedEventArgs e)
        //{
        //    if(myMap.Markers.Count == _routes.Count * 2 + 3)
        //    {
        //        PointLatLng startPoint = _routes[_routes.Count - 1].Directions.Route[_routes[_routes.Count - 1].Directions.Route.Count - 1];
        //        PointLatLng endPoint = new PointLatLng(_route.EndPoint.Latitude, _route.EndPoint.Longitude);
        //        //var cost = route.RouteData.SelectMany(n => n.AdditionalCosts).Distinct().ToList();
        //        double costSum = (double)costs.Sum(n => n.Price);
        //        GDirections directions;
        //        GMapRoute optimalRoute;

        //        if (GetOptimalRoute(costSum) == OptimalRoute.FastestRoute)
        //        {
        //            var route = GMapProviders.GoogleMap.GetDirections(out directions, startPoint, endPoint, false, false, false, false, false);
        //            optimalRoute = new GMapRoute(directions.Route);
        //            {
        //                optimalRoute.ZIndex = 2;
        //            }
        //        }
        //        else
        //        {
        //            var route = GMapProviders.GoogleMap.GetDirections(out directions, startPoint, endPoint, false, true, false, false, false);
        //            optimalRoute = new GMapRoute(directions.Route);
        //            {
        //                optimalRoute.ZIndex = 2;
        //            }
        //        }

        //        myMap.Markers.Add(optimalRoute);
        //        myMap.ZoomAndCenterMarkers(null);
        //    }

        //    else
        //    {
        //        MessageBox.Show("Dodałeś już drogę optymalną!");
        //    }
            

        //}

        //private OptimalRoute GetOptimalRoute(double costs)
        //{
        //    ulong fuelConsumptionSum = (ulong)_positionValues.Where(n => n.FuelConsumed != 0).Sum(o => o.FuelConsumed);
        //    double quantity = (double)_positionValues.Where(n => n.FuelConsumed != 0).Count();
        //    double avgFuelConsumption = fuelConsumptionSum / quantity;

        //    double costLimit = 0.1 * avgFuelConsumption * (RouteDistance / 100);

        //    if(costs < costLimit)
        //    {
        //        return OptimalRoute.FastestRoute;
        //    }
        //    else
        //    {
        //        return OptimalRoute.NoTollRoute;
        //    }

        //}

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //event dla wyswietlenia innej drogi z comboboxa
        private async void cbxRoutes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            Route s = (Route)cbxRoutes.SelectedValue;
            _route = s;
            DoMagic();
            rbtnHeight.IsChecked = true;  
        }


        //funkcja do pobierania indeksu najtanszej drogi. 
        private int GetCheapestRoute()
        {
            double minCost = _routeInfo.Min(n => n.CostsSum);
            int index = _routeInfo.Single(n => n.CostsSum == minCost).RouteId;
            return index -1;
        }
        
        
        //funkcja do pobierania indeksu najszybszej drogi. 

        private int GetFastestRoute()
        {
            TimeSpan minTime = _routeInfo.Min(n => n.RouteTime);
            int index = _routeInfo.Single(n => n.RouteTime == minTime).RouteId;
            return index -1;
        }
        private void btnCheapestRoute_Click(object sender, RoutedEventArgs e)
        {
            
            cbxRoutes.SelectedIndex = GetCheapestRoute();
        }

        private void btnFastestRoute_Click(object sender, RoutedEventArgs e)
        {
            cbxRoutes.SelectedIndex = GetFastestRoute();
        }


        //
    }
}
