using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GPSDataService.Models;
using MapTest.CustomMarkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportProject.Views;

namespace MapTest.MapHelper
{
    public class GMapHelper
    {
        MapWindow _window;


        public GMapHelper(MapWindow window)
        {
            _window = window;
        }
        private GMapMarker AddMarker(GPSData point)
        {
            PointLatLng position = new PointLatLng(point.Position.Latitude, point.Position.Longitude);
            GMapMarker tempMarker = new GMapMarker(position);
            tempMarker.Shape = new CustomMarker(_window, tempMarker, "Position: " + point.Position.Latitude.ToString() + " "
                + point.Position.Longitude.ToString() + "\r\nHeight: " + point.Height.ToString() + "\r\nFuel level:" + point.FuelLevel.ToString());

            return tempMarker;
        }

       

        private RouteToDisplay AddRoute(GPSData position1, GPSData position2)
        {
            PointLatLng start = new PointLatLng(position1.Position.Latitude, position1.Position.Longitude);
            PointLatLng end = new PointLatLng(position2.Position.Latitude, position2.Position.Longitude);

            RouteToDisplay tempRoute = new RouteToDisplay();

            GDirections directions;
            var route = GMapProviders.GoogleMap.GetDirections(out directions, start, end, true, false, false, false, false);
            if(directions == null)
            {
                return null;
            }
            else
            {
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


        }


        public List<RouteToDisplay> SetRoutesList(List<GPSData> points, Route route)
        {
            List<RouteToDisplay> tempRoutes = new List<RouteToDisplay>();
            int k = points.Count;

            for (int i = 0; i < k-1; i++)
            {          
                tempRoutes.Add(AddRoute(points[i], points[i + 1]));
                if (tempRoutes[i] == null)
                    return null;
            }

            return tempRoutes;

        }

        public List<GMapMarker> SetMarkersList(List<GPSData> points, Route route)
        {
            List<GMapMarker> tempMarkers = new List<GMapMarker>();
            int k = points.Count;

            for (int i = 0; i < k; i++)
            {
                tempMarkers.Add(AddMarker(points[i]));
                 
            }

            return tempMarkers;
        }
    }
}
