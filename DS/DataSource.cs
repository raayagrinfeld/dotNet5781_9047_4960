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
            BusLineList = new List<BusLine>();
            BusLineStationList = new List<BusLineStation>();
            BusStationList = new List<BusStation>();
            ConsecutiveStationsList = new List<ConsecutiveStations>();
            UserList = new List<User>();
            BusLineList = new List<BusLine>();
            InitAllLists();
        }
        static void InitAllLists()
        {
            for (int i = 0; i < 50; i++)
            {
                BusStationList.Add(new BusStation
                {
                    BusStationKey = RunNumbers.BusStationRunNumber++,
                    Coordinates = new GeoCoordinate(r.NextDouble() * (33.3 - 31) + 31, r.NextDouble() * (35.5 - 34.3) + 34.3),
                    IsActive = true
                }) ;
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
                int prevBusLineStation = -1;
                for (int j=1;j<6;j++)
                {
                    int busStationKey;
                    do
                    {
                        busStationKey = r.Next(30000, RunNumbers.BusStationRunNumber);
                    }
                    while (BusLineStationList.FirstOrDefault(b => (b.BusLineKey == (20000 + i) & b.BusStationKey == busStationKey)) != null);
                    if(j==1)
                    {
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).FirstStation = busStationKey;
                    }
                    if(j==5)
                    {
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).LastStation = busStationKey;
                    }
                    BusLineStationList.Add(new BusLineStation
                    {
                        BusLineKey = 20000+i,
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
                }
            }
            UserList.Add(new User { UserName = "raaya", Password = "123", IsActive = true, ManagementPermission=true , gender=(gender)1, imagePath=null});
            UserList.Add(new User { UserName = "odelia", Password = "1666", IsActive = true, ManagementPermission = true, gender = (gender)1 ,imagePath="Icons/wonan.png"});
        }
    }
}
