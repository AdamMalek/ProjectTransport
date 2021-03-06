﻿using GPSInterfaces;
using GPSInterfaces.DAL;
using GPSInterfaces.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TransportManager.DAL;

namespace TransportManager.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        MainWindowViewModel()
        {
            SaveXLS = new RelayCommand(SaveToXML,CanSave);
            LoadDB = new RelayCommand(Load, (obj) => true);
            Logout = new RelayCommand(LogoutFunc,(obj) => true);
        }

        public delegate void OnLogoutSuccessful(object sender);
        public event OnLogoutSuccessful OnLogout;

        IWCFGPSDataService proxy;

        public MainWindowViewModel(Guid session) :this()
        {
            _session = session;
            ChannelFactory<IWCFGPSDataService> channels = new ChannelFactory<IWCFGPSDataService>("GPSServiceEndpoint");
            proxy = channels.CreateChannel();
        }
        //      FIELDS

        private Guid _session;
        private List<Route> _routes;

        //      PROPERTIES
        public List<Route> Routes {
            get
            {
                return _routes;
            }
            internal set
            {
                _routes = value;
                RaisePropertyChanged("Routes");
            }
        }

        //      COMMANDS
        public ICommand SaveXLS { get; set; }

        public ICommand LoadDB { get; set; }
        public ICommand Logout { get; set; }

        private void Load(object obj)
        {
            var xx = proxy.GetAllRoutes(_session);
            Routes = xx.ToList();
        }

        private void LogoutFunc(object obj)
        {
            bool result = proxy.LogOut(_session);
            if (result)
            {
                MessageBox.Show("Logout completed!");
                if (OnLogout != null) OnLogout(this);

            }
            else
            {
                MessageBox.Show("Logout failed!");
            }
        }

        private bool CanSave(object obj)
        {
            return true;
        }

        private void SaveToXML(object obj)
        {
            //using (var db = new GPSContext())
            //{
            //    Route r = new Route();
            //    r.RouteName = "Kato - BB";
            //    r.RouteId = 1337;
            //    r.EndPoint = new GPSPos { Latitude = -15, Longitude = -15 };
            //    r.StartPoint = new GPSPos { Latitude = 15, Longitude = 15 };
            //    GPSData gps = new GPSData
            //    {
            //        FuelLevel = 42,
            //        Height = 1499,
            //        Id = 1909,
            //        Position = new GPSPos { Latitude = 44,Longitude=44},
            //        Time = DateTime.Now                     
            //    };
            //    gps.AdditionalCosts.Add(new AdditionalCost { Id = 32, Description = "paliwo", Price = 389 });
            //    r.RouteData.Add(gps);

            //    db.Routes.Add(r);
            //    db.SaveChanges();

            //    System.Windows.MessageBox.Show("Done");
            //}
        }


        //      NOTIFY PROPERTY

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propName)
        {
            var x = PropertyChanged;
            if (x != null) PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
