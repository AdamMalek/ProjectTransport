using MapTest.MapHelper;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapTest.Charting
{
    public class ChartHelper
    {
        private List<RouteToDisplay> _routes;

        public ChartHelper(List<RouteToDisplay> routes)
        {
            _routes = routes;
        }




        //funkcja ktora zwraca ilosc punktow, dla ktorych trzeba obliczyc wartosc w aproksymacji (jesli s == true to wysokosc, w przeciwnym wypadku spalanie). wazne jest to, 
        // ze kazdy punkt koncowy danej drogi jest jednoczesnie punktem poczatkowym kolejnej drogi i punkty by sie zdublowaly stad ta metoda.
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
                        n.Add(_routes[i].Directions.Route.Count - 1);



                }
            }
            else
            {
                for (int i = 1; i < _routes.Count; i++)
                {
                    if (i == _routes.Count - 1)
                        n.Add(_routes[i].Directions.Route.Count);
                    else
                        n.Add(_routes[i].Directions.Route.Count - 1);
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
                //dodawanie pożądanych punktow do listy. (leci po liscie drog, dla kazdej drogi dodaje dystans i wysokosc albo stan paliwa, zalezne od wykresu)
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

                //aproksymacja, jako parametry lista punktow oraz wynik funkcji zwracajacej ilosc pozadanych punktow
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

        //uzupelnianie listy w ktorej do danych wysokosci, dystansu i spalania przyporzadkowane jest pozycja gps zeby mozna bylo to przeniesc na mape biorac dane z wykresu.
        public List<RoutePosValue> InitializePosValueList()
        {
            List<RoutePosValue> _positionValues = new List<RoutePosValue>();
            double[] height, fuelconsumption; //lokalne listy do przechowywania wartosci aproksymacji dla wysokosci i spalania
            double tempDist = 0; //zmienna pomocnicza
            double interval;

            //obliczanie wartosci dla wysokosci oraz spalania dla pozadanych punktow
            height = Approximate(true);
            fuelconsumption = Approximate(false);
            int p = 0, k = 0;


            for (int i = 0; i < _routes.Count; i++)
            {
                //zmienna pomocnicza do obliczania dystansu (jako, ze miedzy kazda para punktow jest inna liczba pozycji gps, to dystans musi odpowiednio wzrastac).
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

                    else if (i == _routes.Count - 1 && j == _routes[i].Directions.Route.Count - 2)
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
                            Position = _routes[i].Directions.Route[j + 1],
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

            return _positionValues;


        }


        private double GetInterval(int k)
        {
            double distance = _routes[k].Distance;

            return distance / _routes[k].Directions.Route.Count;
        }
    }
}
