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

            for (int i = 0; i < 12; i++)
            {
                int prevBusLineStation = -1;
                for (int j = 1; j < 6; j++)
                {
                    int busStationKey;
                    do
                    {
                        busStationKey = r.Next(38880, RunNumbers.BusStationRunNumber);
                    }
                    while ((dl.GetAllBusLineStationBy(b=> true).FirstOrDefault(b => (b.BusLineKey == (20000 + i) & b.BusStationKey == busStationKey)) != null) || (dl.GetAllBusStations().FirstOrDefault(b => (b.BusStationKey == busStationKey)) == null));
                    BusStation bus = dl.GetAllBusStations().FirstOrDefault(b => b.BusStationKey == busStationKey);
                    if (j == 1)
                    {
                        dl.GetAllBusLines().FirstOrDefault(b => b.BusLineKey == (20000 + i)).FirstStation = busStationKey;
                        dl.GetAllBusLines().FirstOrDefault(b => b.BusLineKey == (20000 + i)).FirstStationName = bus.StationName;
                    }
                    if (j == 5)
                    {
                        dl.GetAllBusLines().FirstOrDefault(b => b.BusLineKey == (20000 + i)).LastStation = busStationKey;
                        dl.GetAllBusLines().FirstOrDefault(b => b.BusLineKey == (20000 + i)).LastStationName = bus.StationName;
                    }
                    dl.AddBusLineStation(new BusLineStation
                    {
                        BusLineKey = 20000 + i,
                        StationName = bus.StationName,
                        BusStationKey = busStationKey,
                        IsActive = true,
                        StationNumberInLine = j
                    });
                    var ConsecutiveStation = new ConsecutiveStations { Station1Key = prevBusLineStation, Station2Key = busStationKey, IsActive = true };
                    if (ConsecutiveStation.Station1Key == -1)
                    {
                        ConsecutiveStation.Distance = 0;
                        ConsecutiveStation.DriveDistanceTime = TimeSpan.Zero;
                    }
                    else
                    {
                        ConsecutiveStation.Distance = dl.GetAllBusStations().FirstOrDefault(b => (b.BusStationKey == prevBusLineStation & b.IsActive)).Coordinates.GetDistanceTo(dl.GetAllBusStations().FirstOrDefault(b => (b.BusStationKey == busStationKey & b.IsActive)).Coordinates);
                        ConsecutiveStation.DriveDistanceTime = TimeSpan.FromMinutes(ConsecutiveStation.Distance * 0.01);
                    }
                    dl.AddConsecutiveStations(ConsecutiveStation);
                    prevBusLineStation = busStationKey;
                }
            }


        }
    }
}