using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace DS
{
    public static class DataSource
    {
        public static List<BusLine> BusLineList;
        public static List<BusLineStation> BusLineStationList;
        public static List<BusStation> BusStationList;
        public static List<User> UserList;
        public static List<ConsecutiveStations> ConsecutiveStationsList;
        public static Random r = new Random();
        static DataSource()
        {
            BusLineStationList = new List<BusLineStation>();
            BusStationList = new List<BusStation>();
            BusLineStationList = new List<BusLineStation>();
            ConsecutiveStationsList = new List<ConsecutiveStations>();
            UserList = new List<User>();
            InitAllLists();
        }
        static void InitAllLists()
        {
            for (int i = 0; i < 50; i++)
            {
                BusStationList.Add(new BusStation
                {
                    BusStationKey = RunNumbers.BusStationRunNumber++,
                    Coordinates = new GeoCoordinate(r.NextDouble() * (33.3 - 31) + 31, r.NextDouble() * (35.5 - 34.3) + 34.3)
                });
            }
            for (int i = 0; i < 12; i++)
            {
                BusLineList.Add(new BusLine
                {
                    BusLineKey = RunNumbers.BusLineRunNumber++,
                    LineNumber = r.Next(1000000, 10000000),
                    Area = (Areas)r.Next(0, 5),
                    IsActive = true
                });
            }
            for(int i=0;i<12;i++)
            {
                int busLineKey = r.Next(20000, RunNumbers.BusLineRunNumber);
                int prevBusLineStation = -1;
                for (int j=0;j<5;j++)
                {
                    int busStationKey = r.Next(30000, RunNumbers.BusStationRunNumber);
                    BusLineStationList.Add(new BusLineStation
                    {
                        BusLineKey = busLineKey,
                        BusStationKey = busStationKey,
                        IsActive = true,
                        StationNumberInLine = j
                    });
                    var ConsecutiveStation = new ConsecutiveStations { Station1Key=prevBusLineStation, Station2Key = busStationKey, IsActive = true };
                    if (ConsecutiveStation.Station1Key == -1)
                    {
                        ConsecutiveStation.Distance = 0;
                        ConsecutiveStation.DriveDistanceTime = TimeSpan.Zero;
                    }
                    else
                    {
                        ConsecutiveStation.Distance = BusStationList.Find(b => (b.BusStationKey == prevBusLineStation & b.IsActive)).Coordinates.GetDistanceTo(BusStationList.Find(b => (b.BusStationKey == busStationKey & b.IsActive)).Coordinates);
                        ConsecutiveStation.DriveDistanceTime = TimeSpan.FromMinutes(ConsecutiveStation.Distance * 0.01);
                    }
                    ConsecutiveStationsList.Add(ConsecutiveStation);
                    prevBusLineStation = busStationKey;
                    if(j==0)
                    {
                        BusLineList.FirstOrDefault(b => (b.LineNumber == busLineKey)).FirstStation = BusLineStationList.ElementAt(i * j).BusStationKey;
                    }
                    if (j == 5)
                    {
                        BusLineList.FirstOrDefault(b => (b.LineNumber == busLineKey)).LastStation = BusLineStationList.ElementAt(i * j).BusStationKey;
                    }
                }
            }
            UserList.Add(new User { UserName = "raaya", Password = "123", IsActive = true, ManagementPermission=true });
            UserList.Add(new User { UserName = "odelia", Password = "1666", IsActive = true, ManagementPermission = true });
        }
    }
}
