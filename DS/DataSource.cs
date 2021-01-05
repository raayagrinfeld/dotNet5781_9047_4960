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
            UserList = new List<User>();
        }
        static void InitAllLists()
        {
            BusStationList = new List<BusStation>();
            for (int i = 0; i < 50; i++)
            {
                BusStationList.Add(new BusStation
                {
                    BusStationKey = RunNumbers.BusStationRunNumber++,
                    Coordinates = new GeoCoordinate(r.NextDouble() * (33.3 - 31) + 31, r.NextDouble() * (35.5 - 34.3) + 34.3)
                });
            }
            BusLineList = new List<BusLine>();
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
        }
    }
}
