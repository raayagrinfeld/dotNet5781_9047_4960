using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using APIDL;
using System.Xml.Linq;

namespace DL
{
    class DLXML : IDAL
    {
        #region singelton
        static readonly DLXML instance = new DLXML();
        static DLXML() { }// static ctor to ensure instance init is done just before first usage
        DLXML() { } // default => private
        public static DLXML Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files

        string UserPath = @"User.xml"; //XElement

        string BusLinePath = @"BusLine.xml"; //XMLSerializer
        string BusLineStationPath = @"BusLineStation.xml"; //XMLSerializer
        string BusStationPath = @"BusStation.xml"; //XMLSerializer
        string ConsecutiveStationsPath = @"ConsecutiveStations.xml"; //XMLSerializer
        string BusSchedulePath = @"BusSchedule.xml"; //XMLSerializer


        #endregion

        #region BusLine
        public void AddBusLine(BusLine bus)
        {
            List<BusLine> ListBuses = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            if (ListBuses.FirstOrDefault(b => b.BusLineKey == bus.BusLineKey &b.IsActive) != null)
                throw new DO.BadBusLineKeyException(bus.BusLineKey, "This bus line already exist");

           if (GetBusLine(bus.BusLineKey) != null)
             throw new DO.BadBusLineKeyException(bus.BusLineKey, "exsist bus line");

            ListBuses.Add(bus); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListBuses, BusLinePath);
        }
        public void DeleteBusLine(int busLineKey)
        {
            List<BusLine> ListBusLines = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            DO.BusLine sic = ListBusLines.Find(b =>
            {
                if (b.BusLineKey == busLineKey & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (sic == null)
                throw new DO.BadBusLineKeyException(busLineKey, "this bus line is not exsist");

            XMLTools.SaveListToXMLSerializer(ListBusLines, BusLinePath);
        }
        public BusLine GetBusLine(int busLineKey)
        {
            List<BusLine> ListBusLines = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            DO.BusLine bus = ListBusLines.Find(b => b.BusLineKey == busLineKey&b.IsActive);
            return bus;
            // if (bus != null)
            return bus; //no need to Clone()
           // else
                //throw new DO.BadBusLineKeyException(busLineKey, $"bad bus line key: {busLineKey}");
        }
        public IEnumerable<BusLine> GetAllBusLines()
        {
            List<BusLine> ListBusLines = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            return from bus in ListBusLines
                   where bus.IsActive
                   select bus;
        }
        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            List<BusLine> ListBusLines = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            return from bus in ListBusLines
                   where bus.IsActive & predicate(bus)
                   select bus;
        }
        public void UpdateBusLine(BusLine bus)
        {
            List<BusLine> ListBuses = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);
            BusLine busLine = ListBuses.Find(b => b.BusLineKey == bus.BusLineKey&b.IsActive);
            if (busLine != null)
            {
                DeleteBusLine(bus.BusLineKey);
                AddBusLine(bus);
            }
            else
            {
                throw new BadBusLineKeyException(bus.BusLineKey, $"bad bus line key: {bus.BusLineKey}");
            }
            XMLTools.SaveListToXMLSerializer(ListBuses, BusLinePath);
        }
        public void UpdateBusLine(int busLineKey, Action<BusLine> update)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region BusLineStation
        public void AddBusLineStation(BusLineStation station)
        {
            List<BusLineStation> BusLineStationList = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);

            if (BusLineStationList.FirstOrDefault(b => (b.BusStationKey == station.BusStationKey && b.BusLineKey == station.BusLineKey&&b.IsActive)) != null)
                throw new DO.BadBusLineStationsException(station.BusStationKey, station.BusLineKey, "This Station Alrady exist in this bus path");

            BusLineStationList.Add(station);

            XMLTools.SaveListToXMLSerializer(BusLineStationList, BusLineStationPath);
        }
        public void DeleteBusLineStationAllBusLine(int BusStationKey)
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);
            IEnumerable<BusLineStation> sic = ListBusLineStations.Where(b =>
            {
                if (b.BusStationKey == BusStationKey & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (sic == null)
                throw new DO.BadBusLineStationsException(BusStationKey, 0, "this bus line ststion does not exsist");

            XMLTools.SaveListToXMLSerializer(ListBusLineStations, BusLineStationPath);
        }
        public void DeleteBusLineStationInOneBusLine(int BusStationKey, int BusLineKey)
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);
            BusLineStation sic = ListBusLineStations.Find(b =>
            {
                if (b.BusStationKey == BusStationKey & b.BusLineKey == BusLineKey & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (sic == null)
                throw new DO.BadBusLineStationsException(BusStationKey, BusLineKey, "this bus line ststion does not exsist");

            XMLTools.SaveListToXMLSerializer(ListBusLineStations, BusLineStationPath);
        }
        public IEnumerable<BusLineStation> GetAllBusLineStationBy(Predicate<BusLineStation> predicate)
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);

            return from station in ListBusLineStations
                   where station.IsActive & predicate(station)
                   select station;

        }
        public int GetBusLineStationKey(int BusLineKey, int StationNumberInLine)
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);

            DO.BusLineStation station = ListBusLineStations.Find(b => (b.BusLineKey == BusLineKey & b.StationNumberInLine == StationNumberInLine & b.IsActive));
            if (station != null)
                return station.BusStationKey; //no need to Clone()
            else
                throw new DO.BadBusLineStationsException(BusLineKey, StationNumberInLine, $"bad bus line key and Station number in line: {BusLineKey} {StationNumberInLine}");
        }
        public void UpdateBusLineStation(BusLineStation station)
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);
            BusLineStation sta = ListBusLineStations.Find(b => (b.BusStationKey == station.BusStationKey & b.BusLineKey == station.BusLineKey & b.IsActive));
            if (sta != null)
            {
                DeleteBusLineStationInOneBusLine(station.BusStationKey, station.BusLineKey);
                AddBusLineStation(station);
            }
            else
            {
                throw new BadBusLineStationsException(station.BusStationKey, station.BusLineKey, "station is not in the bus Line path");
            }
        }
        #endregion

        #region BusStation
        public void AddBusStation(BusStation station)
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            if (ListBusStations.FirstOrDefault(s => s.BusStationKey == station.BusStationKey & s.IsActive) != null)
                throw new DO.BadBusLineKeyException(station.BusStationKey, "This bus station already exist");

           // if (GetBusStation(station.BusStationKey) == null)
              //  throw new DO.BadBusLineKeyException(station.BusStationKey, "Missing bus station");

            ListBusStations.Add(station); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListBusStations, BusStationPath);
        }
        public void DeleteBusStation(int busStationKey)
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            DO.BusStation sic = ListBusStations.Find(b =>
            {
                if (b.BusStationKey == busStationKey & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (sic == null)
                throw new DO.BadBusStationKeyException(busStationKey, "this bus station is not exsist");

            XMLTools.SaveListToXMLSerializer(ListBusStations, BusStationPath);
        }
        public IEnumerable<BusStation> GetAllBusStations()
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            return from station in ListBusStations
                   where station.IsActive
                   select station;
        }
        public IEnumerable<BusStation> GetAllBusStationsBy(Predicate<BusStation> predicate)
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            return from station in ListBusStations
                   where station.IsActive & predicate(station)
                   select station;
        }
        public BusStation GetBusStation(int busStationKey)
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            DO.BusStation station = ListBusStations.Find(b => (b.BusStationKey == busStationKey & b.IsActive));
            //if (station != null)
                return station; //no need to Clone()
            //else
              //  throw new DO.BadBusStationKeyException(busStationKey, $"bad bus stationkey: {busStationKey}");
        }
        public void UpdateBusStation(BusStation station)
        {
            List<BusStation> ListStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);
            BusStation busStation = ListStations.Find(b => b.BusStationKey == station.BusStationKey);
            if (busStation != null)
            {
                DeleteBusStation(station.BusStationKey);
                AddBusStation(station);
            }
            else
            {
                throw new BadBusStationKeyException(station.BusStationKey, $"bad bus line key: {station.BusStationKey}");
            }
        }
        #endregion

        #region ConsecutiveStation
        public void AddConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            List<ConsecutiveStations> ConsecutiveStationsList = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(ConsecutiveStationsPath);

            if (ConsecutiveStationsList.FirstOrDefault(b => (b.Station1Key == consecutiveStations.Station1Key && b.Station2Key == consecutiveStations.Station2Key&&b.IsActive)) != null)
                throw new DO.BadConsecutiveStationsException(consecutiveStations.Station1Key, consecutiveStations.Station2Key, "dupicated consecutive station");


            ConsecutiveStationsList.Add(consecutiveStations);

            XMLTools.SaveListToXMLSerializer(ConsecutiveStationsList, ConsecutiveStationsPath);
        }
        public void DeletConsecutiveStations(int key1, int key2)
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(ConsecutiveStationsPath);

            DO.ConsecutiveStations sic = ListConsecutiveStations.Find(s =>
            {
                if (s.Station1Key == key1 & s.Station2Key == key2 & s.IsActive)
                {
                    s.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (sic == null)
                throw new DO.BadConsecutiveStationsException(key1, key2, "this consecutive station is not exsist");

            XMLTools.SaveListToXMLSerializer(ListConsecutiveStations, ConsecutiveStationsPath);
        }
        public IEnumerable<ConsecutiveStations> GetAlConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate)
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(ConsecutiveStationsPath);

            return from consecutiveStation in ListConsecutiveStations
                   where (predicate(consecutiveStation)) & consecutiveStation.IsActive
                   select consecutiveStation;
        }
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(ConsecutiveStationsPath);

            return from stations in ListConsecutiveStations
                   where stations.IsActive
                   select stations;
        }
        public ConsecutiveStations GetConsecutiveStations(int key1, int key2)
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(ConsecutiveStationsPath);

            DO.ConsecutiveStations stations = ListConsecutiveStations.Find(s => (s.Station1Key == key1 & s.Station2Key == key2 & s.IsActive));
            if (stations != null)
                return stations; //no need to Clone()
            else
                throw new DO.BadConsecutiveStationsException(key1, key2, $"bad consecutive stations: {key1}{key2}");
        }
        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            List<ConsecutiveStations> ListConsecutiveStations = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(ConsecutiveStationsPath);

            DO.ConsecutiveStations sic = ListConsecutiveStations.Find(b => (b.Station1Key == consecutiveStations.Station1Key & b.Station2Key == consecutiveStations.Station2Key & b.IsActive));
            if (sic != null)
            {
                DeletConsecutiveStations(consecutiveStations.Station1Key, consecutiveStations.Station2Key);
                AddConsecutiveStations(consecutiveStations);
            }
            else
            {
                throw new BadConsecutiveStationsException(consecutiveStations.Station1Key, consecutiveStations.Station2Key, $"bad bus user name: {consecutiveStations.Station1Key}{consecutiveStations.Station2Key}");
            }
        }
        #endregion

        #region UserXML
        public void AddUser(User user)
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            XElement userSearch = (from p in UserRootElem.Elements()
                             where (p.Element("UserName").Value) == user.UserName
                             select p).FirstOrDefault();

            if (userSearch != null)
                throw new DO.BadUserNameException(user.UserName, "Duplicate user name");

            XElement UserElem = new XElement("User",
                                   new XElement("UserName", user.UserName),
                                   new XElement("Password", user.Password),
                                   new XElement("ManagementPermission", user.ManagementPermission),
                                   new XElement("IsActive", user.IsActive),
                                   new XElement("gender", user.gender.ToString()),
                                   new XElement("imagePath", user.imagePath));

            UserRootElem.Add(UserElem);

            XMLTools.SaveListToXMLElement(UserRootElem, UserPath);
        }
        public void DeletUser(string userName)
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            XElement userSearch = (from p in UserRootElem.Elements()
                                   where (p.Element("UserName").Value) == userName
                                   select p).FirstOrDefault();

            if (userSearch == null)
                throw new DO.BadUserNameException(userName, "Duplicate user name");
            (userSearch.Element("IsActive").Value) = false.ToString();

            XMLTools.SaveListToXMLElement(UserRootElem, UserPath);
        }
        public IEnumerable<User> GetAllUsers()
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            return (from p in UserRootElem.Elements()
                    select new User()
                    {
                        UserName = p.Element("UserName").Value,
                        Password = p.Element("Password").Value,
                        gender = (gender)Enum.Parse(typeof(gender), p.Element("Gender").Value),
                        IsActive = Boolean.Parse(p.Element("IsActive").Value),
                        ManagementPermission = Boolean.Parse(p.Element("ManagementPermission").Value),
                        imagePath= p.Element("imagePath").Value

                    }
                   );
        }
        public IEnumerable<User> GetAlUersBy(Predicate<User> predicate)
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            return (from p in UserRootElem.Elements()
                    let u = new User()
                    {
                        UserName = p.Element("UserName").Value,
                        Password = p.Element("Password").Value,
                        gender = (gender)Enum.Parse(typeof(gender), p.Element("Gender").Value),
                        IsActive = Boolean.Parse(p.Element("IsActive").Value),
                        ManagementPermission = Boolean.Parse(p.Element("ManagementPermission").Value),
                        imagePath = p.Element("imagePath").Value
                    }
                    where predicate(u)
                    select u
                   );
        }
        public User GetUser(string userName)
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            User u = (from p in UserRootElem.Elements()
                      where (p.Element("UserName").Value) == userName
                      select new User()
                      {
                          UserName = p.Element("UserName").Value,
                          Password = p.Element("Password").Value,
                          gender = (gender)Enum.Parse(typeof(gender), p.Element("Gender").Value),
                          IsActive = Boolean.Parse(p.Element("IsActive").Value),
                          ManagementPermission = Boolean.Parse(p.Element("ManagementPermission").Value),
                          imagePath = p.Element("imagePath").Value
                      }
                        ).FirstOrDefault();

            if (u == null)
                throw new DO.BadUserNameException(userName, "Duplicate user name");

            return u;
        }
        public void UpdateUser(User user)
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            XElement userSearch = (from p in UserRootElem.Elements()
                                   where (p.Element("UserName").Value) == user.UserName
                                   select p).FirstOrDefault();

            if (userSearch == null)
                throw new DO.BadUserNameException(user.UserName, "Duplicate user name");
            else
            {
                userSearch.Element("UserName").Value = user.UserName;
                userSearch.Element("Password").Value = user.Password;
                userSearch.Element("IsActive").Value = user.IsActive.ToString();
                userSearch.Element("ManagementPermission").Value = user.ManagementPermission.ToString();
                userSearch.Element("imagePath").Value = user.imagePath;
            }
            XMLTools.SaveListToXMLElement(UserRootElem, UserPath);
        }
        #endregion

        #region BusSchedules
        public BusesSchedule GetBusesSchedule(int scheduleKey)
        {
            List<BusesSchedule> ListBusesSchedules = XMLTools.LoadListFromXMLSerializer<BusesSchedule>(BusSchedulePath);

            DO.BusesSchedule sche = ListBusesSchedules.Find(b => b.ScheduleKey == scheduleKey & b.IsActive);
            if (sche != null)
                return sche; //no need to Clone()
            else
                throw new DO.BadBusesScheduleKeyException(scheduleKey, $"bad schedule key: {scheduleKey}");
        }
        public BusesSchedule GetBusesSchedule(int busLineKey, TimeSpan time)
        {
            List<BusesSchedule> ListBusesSchedules = XMLTools.LoadListFromXMLSerializer<BusesSchedule>(BusSchedulePath);

            DO.BusesSchedule sche = ListBusesSchedules.Find(b => b.BusKey == busLineKey&b.EndtHour>time &b.StartHour<time & b.IsActive);
            if (sche != null)
                return sche; //no need to Clone()
            else
                throw new DO.BadBusesScheduleKeyException(-1, $"this line don't work in this hour");
        }

        public IEnumerable<BusesSchedule> GetAllBusSchedules()
        {
            List<BusesSchedule> ListBusesSchedules = XMLTools.LoadListFromXMLSerializer<BusesSchedule>(BusSchedulePath);

            return from sch in ListBusesSchedules
                   where sch.IsActive
                   select sch;
        }

        public IEnumerable<BusesSchedule> GetAllBusSchedulesBy(Predicate<BusesSchedule> predicate)
        {
            List<BusesSchedule> ListBusesSchedules = XMLTools.LoadListFromXMLSerializer<BusesSchedule>(BusSchedulePath);
            return from sch in ListBusesSchedules
                   where sch.IsActive& predicate(sch)
                   select sch;
        }

        public void AddBusSchedule(BusesSchedule schedule)
        {
            XElement ScheduleRootElem = XMLTools.LoadListFromXMLElement(BusSchedulePath);

            XElement ScheduleSearch = (from p in ScheduleRootElem.Elements()
                                   where (p.Element("ScheduleKey").Value) == schedule.ScheduleKey.ToString()
                                   select p).FirstOrDefault();

            if (ScheduleSearch != null)
                throw new DO.BadBusesScheduleKeyException(schedule.ScheduleKey, "Duplicate user name");

            XElement ScheduleElem = new XElement("BusesSchedule",
                                   new XElement("ScheduleKey", schedule.ScheduleKey.ToString()),
                                   new XElement("BusKey", schedule.BusKey.ToString()),
                                   new XElement("StartHour", schedule.StartHour.ToString()),
                                   new XElement("EndtHour", schedule.EndtHour.ToString()),
                                   new XElement("Frequency", schedule.Frequency.ToString()),
                                   new XElement("IsActive", schedule.IsActive));

            ScheduleRootElem.Add(ScheduleElem);

            XMLTools.SaveListToXMLElement(ScheduleRootElem, BusSchedulePath);
        }

        public void UpdateBusSchedule(BusesSchedule schedule)
        {
            List<BusesSchedule> ListBusesSchedules = XMLTools.LoadListFromXMLSerializer<BusesSchedule>(BusSchedulePath);
            BusesSchedule sche= ListBusesSchedules.Find(b => b.ScheduleKey == schedule.ScheduleKey&b.IsActive);
            if (sche != null)
            {
                DeleteBusSchedule(sche.ScheduleKey);
                AddBusSchedule(sche);
            }
            else
            {
                throw new DO.BadBusesScheduleKeyException(schedule.ScheduleKey, $"bad schedule key: {schedule.ScheduleKey}");
            }
            XMLTools.SaveListToXMLSerializer(ListBusesSchedules, BusSchedulePath);
        }

        public void DeleteBusSchedule(int scheduleKey)
        {
            List<BusesSchedule> ListBusesSchedules = XMLTools.LoadListFromXMLSerializer<BusesSchedule>(BusSchedulePath);
            BusesSchedule sche = ListBusesSchedules.Find(b =>
            {
                if (b.ScheduleKey == scheduleKey & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (sche == null)
                throw new DO.BadBusesScheduleKeyException(scheduleKey, $"bad schedule key: {scheduleKey}");

            XMLTools.SaveListToXMLSerializer(ListBusesSchedules, BusSchedulePath);
        }
        #endregion


    }
}
