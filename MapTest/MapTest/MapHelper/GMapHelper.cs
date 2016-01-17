﻿using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GPSInterfaces.Models;
using MapTest.CustomMarkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapTest.MapHelper
{
    public class GMapHelper
    {
        MainWindow _window;


        public GMapHelper(MainWindow window)
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

        private GMapMarker AddMarker(Route route)
        {
            PointLatLng position = new PointLatLng(route.EndPoint.Latitude, route.EndPoint.Longitude);
            GMapMarker tempMarker = new GMapMarker(position);
            tempMarker.Shape = new CustomMarker(_window, tempMarker, "End: " + route.EndPoint.Latitude.ToString() + " "
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


        public List<RouteToDisplay> SetRoutesList(List<GPSData> points, Route route)
        {
            List<RouteToDisplay> tempRoutes = new List<RouteToDisplay>();
            int k = points.Count;

            for (int i = 0; i < k + 1; i++)
            {
                if (i < k - 1)
                    tempRoutes.Add(AddRoute(points[i], points[i + 1]));
            }

            return tempRoutes;

        }

        public List<GMapMarker> SetMarkersList(List<GPSData> points, Route route)
        {
            List<GMapMarker> tempMarkers = new List<GMapMarker>();
            int k = points.Count;

            for (int i = 0; i < k + 1; i++)
            {
                if (i != k)
                    tempMarkers.Add(AddMarker(points[i]));
                if (i == k)
                    tempMarkers.Add(AddMarker(route));  
            }

            return tempMarkers;
        }
    }
}