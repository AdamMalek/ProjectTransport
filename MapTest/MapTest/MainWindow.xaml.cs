using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
        private List<RoutePosValue> _positionValues;
        

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
            InitializePosValueList();
            //rbtnFuelConsumed.IsChecked = true;
            rbtnHeight.IsChecked = true;





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
            
            for (int i = 0; i < _positionValues.Count; i++)
            {
                _valueList.Add(_positionValues[i + _routes[0].Directions.Route.Count - 1].Distance, _positionValues[i + _routes[0].Directions.Route.Count - 1].FuelConsumed);
            }
            ((LineSeries)lineChart.Series[0]).ItemsSource = _valueList;
            yAxis.Title = "Spalanie [l/100km]";
            chart.Title = "Spalanie [l/100km]";


        }

        private void rbtnHeight_Checked(object sender, RoutedEventArgs e)
        {
            _valueList = new Dictionary<double, double>();

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


        private List<double> GetPointsQuantity(bool s)
        {
            List<double> n = new List<double>();
            if (s)
            {
                for (int i = 0; i < _routes.Count; i++)
                {
                    if (i == _routes.Count - 1)
                        n.Add(_routes[i].Directions.Route.Count);
                    else
                        n.Add(_routes[i].Directions.Route.Count-1);



                }
            }
            else
            {
                for (int i = 1; i < _routes.Count; i++)
                {
                    if (i == _routes.Count - 1)
                        n.Add(_routes[i].Directions.Route.Count);
                    else
                        n.Add(_routes[i].Directions.Route.Count-1);
                }
            }

            
            return n;
        }

        private double[] Approximate(bool heightOrFuelConsump)
        {
            double[] result;
            List<Point> points = new List<Point>();
            double dist = 0;
            if (heightOrFuelConsump) //obliczanie wartosci dla wysokosci
            {
                for (int i = 0; i < _routes.Count; i++)
                {
                    if (i == 0)
                    {
                        points.Add(new Point
                        {
                            X = 0,
                            Y = _routes[i].StartHeight
                        });
                    }
                    else if (i == _routes.Count - 1)
                    {
                        points.Add(new Point
                        {
                            X = dist,
                            Y = _routes[i].StartHeight
                        });
                        points.Add(new Point
                        {
                            X = dist + _routes[i].Distance,
                            Y = _routes[i].EndHeight
                        });
                    }
                    else
                    {
                        points.Add(new Point
                        {
                            X = dist,
                            Y = _routes[i].StartHeight
                        });
                    }


                    dist += _routes[i].Distance;

                }

                result = Approximation.Approximation.Approximate(points, GetPointsQuantity(true));
            }

            else //obliczanie wartosci dla spalania
            {
                for (int i = 0; i < _routes.Count; i++)
                {
                    if (i == 0)
                    {
                        points.Add(new Point
                        {
                            X = _routes[i].Distance,
                            Y = _routes[i].FuelConsumed
                        });
                    }

                    else
                    {
                        points.Add(new Point
                        {
                            X = dist + _routes[i].Distance,
                            Y = _routes[i].FuelConsumed
                        });
                    }
                    dist += _routes[i].Distance;
                }

                result = Approximation.Approximation.Approximate(points, GetPointsQuantity(false));
            }


            return result;
        }

        private void InitializePosValueList()
        {
            _positionValues = new List<RoutePosValue>();
            double[] height, fuelconsumption; //lokalne listy do przechowywania wartosci aproksymacji dla wysokosci i spalania
            double tempDist = 0; //zmienna pomocnicza
            double interval;


            height = Approximate(true);
            fuelconsumption = Approximate(false);
            int p = 0, k = 0;


            for (int i = 0; i < _routes.Count; i++)
            {
                interval = GetInterval(i);
                //_positionValues.Add(new RoutePosValue
                //{
                //    Position = _routes[i].Route.Position,
                //    Distance = tempDist,
                //    FuelConsumed = _routes[i].FuelConsumed,
                //    Height = _routes[i].StartHeight
                //});


                //if (i == _routes.Count - 1)
                //{
                //    _positionValues.Add(new RoutePosValue
                //    {
                //        Position = _routes[i].Route.Position,
                //        Distance = tempDist,
                //        FuelConsumed = _routes[i].FuelConsumed,
                //        Height = _routes[i].EndHeight
                //    });
                //}
                for (int j = 0; j < _routes[i].Directions.Route.Count - 1; j++)
                {
                    if (i == 0)
                    {
                        _positionValues.Add(new RoutePosValue
                        {
                            Position = _routes[i].Directions.Route[j],
                            Distance = tempDist,
                            FuelConsumed = 0,
                            Height = height[p]
                        });
                        p++;

                    }

                    else if (i==_routes.Count - 1 && j == _routes[i].Directions.Route.Count - 2)
                    {
                        _positionValues.Add(new RoutePosValue
                        {
                            Position = _routes[i].Directions.Route[j],
                            Distance = tempDist,
                            FuelConsumed = fuelconsumption[k],
                            Height = height[p]
                        });
                        p++; k++; tempDist += interval;
                        _positionValues.Add(new RoutePosValue
                        {
                            Position = _routes[i].Directions.Route[j+1],
                            Distance = tempDist,
                            FuelConsumed = fuelconsumption[k],
                            Height = height[p]
                        });
                    }

                    else
                    {
                        
                        _positionValues.Add(new RoutePosValue
                        {
                            Position = _routes[i].Directions.Route[j],
                            Distance = tempDist,
                            FuelConsumed = fuelconsumption[k],
                            Height = height[p]
                        });
                        k++; p++;
                    }
                    tempDist += interval;

                }

            }


        }

        private double GetInterval(int k)
        {
            double distance = _routes[k].Distance;

            return distance / _routes[k].Directions.Route.Count;
        }

        private void SetDisplayMarker(double X, double Y)
        {
            if(rbtnFuelConsumed.IsChecked == true)
            {
                
            }

            else
            {

            }
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
