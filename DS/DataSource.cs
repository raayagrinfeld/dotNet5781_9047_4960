using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using Excel = Microsoft.Office.Interop.Excel;


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
        public static Excel.Application xlApp = new Excel.Application();
        public static bool a = System.IO.File.Exists(@"C:\Users\Keren\Source\Repos\raayagrinfeld\dotNet5781_9047_4960\DS\excel_seet\station_Names.xlsx");
        public static Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@"C:\Users\Keren\Source\Repos\raayagrinfeld\dotNet5781_9047_4960\DS\excel_seet\station_Names.xlsx");
        public static Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
        public static Excel.Range xlRange = xlWorksheet.UsedRange;

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
            for (int i = 2; i < 180; i++)
            {
                BusStationList.Add(new BusStation
                {
                    BusStationKey = RunNumbers.BusStationRunNumber++,
                    Coordinates = new GeoCoordinate(xlRange.Cells[i, 5].Value2,xlRange.Cells[i, 6].Value2),
                    StationAddress= xlRange.Cells[i, 4].Value2,
                    StationName= xlRange.Cells[i, 3].Value2,
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
                    var bus = BusStationList.FirstOrDefault(b => b.BusStationKey == busStationKey);
                    if (j==1)
                    {
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).FirstStation = busStationKey;
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).FirstStationName = bus.StationName;
                    }
                    if(j==5)
                    {
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).LastStation = busStationKey;
                        BusLineList.Find(b => b.BusLineKey == (20000 + i)).LastStationName = bus.StationName;
                    }
                    BusLineStationList.Add(new BusLineStation
                    {
                        BusLineKey = 20000+i,
                        StationName=bus.StationName,
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
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
