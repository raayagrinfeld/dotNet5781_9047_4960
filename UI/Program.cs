using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using BlApi;
using BO;
using DO;
using APIDL;
using System.Device.Location;


namespace PlConsole
{
    class Program
    {
        static IBL bl;
        static IDAL dl = DalFactory.GetDal();
        static void Main(string[] args)
        {
            Random r = new Random();
            dl.AddConsecutiveStations(new ConsecutiveStations { 
                Distance = dl.GetBusStation(39029).Coordinates.GetDistanceTo(dl.GetBusStation(39013).Coordinates), 
                DriveDistanceTime= TimeSpan.FromMinutes((dl.GetBusStation(39029).Coordinates.GetDistanceTo(dl.GetBusStation(39013).Coordinates)*0.01)),
                 IsActive=true,
                  Station1Key= 39029,
                   Station2Key= 39013
            });

           


        }
    }
}