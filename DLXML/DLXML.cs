using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using APIDL;
using System.Xml.Linq;
using System.Security.Cryptography;

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
        string ConsecutiveStationsPath = @"ConsecutiveStations.xml"; //XElement
        string BusSchedulePath = @"BusSchedule.xml"; //XElement

        string BusLinePath = @"BusLine.xml"; //XMLSerializer
        string BusLineStationPath = @"BusLineStation.xml"; //XMLSerializer
        string BusStationPath = @"BusStation.xml"; //XMLSerializer
        string RunNumbersPath = @"RumNumbers.xml";//XMLSerializer


        #endregion

        #region BusLine
        public void AddBusLine(BusLine bus) //add bus line
        {
            List<BusLine> ListBuses = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            if (ListBuses.FirstOrDefault(b => b.BusLineKey == bus.BusLineKey &b.IsActive) != null)
                throw new DO.BadBusLineKeyException(bus.BusLineKey, "This bus line already exist");
            ListBuses.Add(bus); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListBuses, BusLinePath);
        }
        public void DeleteBusLine(int busLineKey)   //delete bus line
        {
            List<BusLine> ListBusLines = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            DO.BusLine sic = ListBusLines.Find(b =>
            {
                if (b.BusLineKey == busLineKey & b.IsActive)
                {
                    b.IsActive = false; //torning busline to not active
                    return true;
                }
                else return false;
            });
            if (sic == null)
                throw new DO.BadBusLineKeyException(busLineKey, "this bus line does not exsist");

            XMLTools.SaveListToXMLSerializer(ListBusLines, BusLinePath);
        }
        public BusLine GetBusLine(int busLineKey)   //get bus line
        {
            List<BusLine> ListBusLines = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            DO.BusLine busLine = ListBusLines.Find(b => b.BusLineKey == busLineKey&b.IsActive);
            if (busLine != null)
                return busLine;
            else
                throw new BadBusLineKeyException(busLineKey, $"bad bus line key: {busLineKey}");
        }
        public IEnumerable<BusLine> GetAllBusLines()    //gets all bus line
        {
            List<BusLine> ListBusLines = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            return from bus in ListBusLines
                   where bus.IsActive
                   select bus;
        }
        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)  //get all bus line by a predicate
        {
            List<BusLine> ListBusLines = XMLTools.LoadListFromXMLSerializer<BusLine>(BusLinePath);

            return from bus in ListBusLines
                   where bus.IsActive & predicate(bus)
                   select bus;
        }
        public void UpdateBusLine(BusLine bus)  //updates bus line
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
        }
        #endregion

        #region BusLineStation
        public BusLineStation GetBusLineStation(int busLineKey, int key)    //get station of bus
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);

            BusLineStation busLineStation = ListBusLineStations.Find(b => b.BusLineKey == busLineKey & b.BusStationKey == key & b.IsActive);
            if (busLineStation == null)
                throw new DO.BadBusLineStationsException(busLineKey, key, "this line does not have this station");
            return busLineStation;
        }
        public void AddBusLineStation(BusLineStation station)   //adds station to bus
        {
            List<BusLineStation> BusLineStationList = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);

            if (BusLineStationList.FirstOrDefault(b => (b.BusStationKey == station.BusStationKey && b.BusLineKey == station.BusLineKey&&b.IsActive)) != null)
                throw new DO.BadBusLineStationsException(station.BusStationKey, station.BusLineKey, "This Station Alrady exist in this bus path");

            BusLineStationList.Add(station);

            XMLTools.SaveListToXMLSerializer(BusLineStationList, BusLineStationPath);
        }
        
        public void DeleteBusLineStationInOneBusLine(int BusStationKey, int BusLineKey) //deletes a station from a busline
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);
            BusLineStation sic = ListBusLineStations.Find(b =>
            {
                if (b.BusStationKey == BusStationKey & b.BusLineKey == BusLineKey & b.IsActive)
                {
                    b.IsActive = false;//deactivating
                    return true;
                }
                else return false;
            });
            if (sic == null)
                throw new DO.BadBusLineStationsException(BusStationKey, BusLineKey, "this bus line ststion does not exsist");
            
            //so it goes trough all the stations that were after the station to delete and decrise their StationNumberInLine
            ListBusLineStations.FindAll(b =>
            {
                if (b.StationNumberInLine > sic.StationNumberInLine & b.BusLineKey == BusLineKey & b.IsActive)
                {
                    b.StationNumberInLine--;
                }
                return false;          
            });
            XMLTools.SaveListToXMLSerializer(ListBusLineStations, BusLineStationPath);
        }
        public IEnumerable<BusLineStation> GetAllBusLineStationBy(Predicate<BusLineStation> predicate)  //get all station of buss by a predicat
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);

            return from station in ListBusLineStations
                   where station.IsActive & predicate(station)
                   select station;

        }

       
        public int GetBusLineStationKey(int BusLineKey, int StationNumberInLine)  //its an helping func that return the station key by the statin number
        {
            List<BusLineStation> ListBusLineStations = XMLTools.LoadListFromXMLSerializer<BusLineStation>(BusLineStationPath);

            DO.BusLineStation station = ListBusLineStations.Find(b => (b.BusLineKey == BusLineKey & b.StationNumberInLine == StationNumberInLine & b.IsActive));
            if (StationNumberInLine == 0)
                return -1;
            if (station != null)
                return station.BusStationKey; 
            else
                throw new DO.BadBusLineStationsException(BusLineKey, StationNumberInLine, $"bad bus line key and Station number in line: {BusLineKey} {StationNumberInLine}");
        }
        public void UpdateBusLineStation(BusLineStation station)    //update a station of bus
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
        public void AddBusStation(BusStation station)   //adds a station
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            if (ListBusStations.FirstOrDefault(s => s.BusStationKey == station.BusStationKey & s.IsActive) != null)
                throw new DO.BadBusLineKeyException(station.BusStationKey, "This bus station already exist");

            ListBusStations.Add(station); 

            XMLTools.SaveListToXMLSerializer(ListBusStations, BusStationPath);
        }
        public void DeleteBusStation(int busStationKey) //deletes a station
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
        public IEnumerable<BusStation> GetAllBusStations()  //get all stations
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            return from station in ListBusStations
                   where station.IsActive
                   select station;
        }
        public IEnumerable<BusStation> GetAllBusStationsBy(Predicate<BusStation> predicate) //gets all station by a predicate
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            return from station in ListBusStations
                   where station.IsActive & predicate(station)
                   select station;
        }
        public BusStation GetBusStation(int busStationKey)  //get a station
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            DO.BusStation station = ListBusStations.Find(b => (b.BusStationKey == busStationKey & b.IsActive));
            if (station != null)
                return station; //no need to Clone()
            else
               throw new DO.BadBusStationKeyException(busStationKey, $"bad bus stationkey: {busStationKey}");
        }
        public void UpdateBusStation(BusStation station)    //updates information of station
        {
            List<BusStation> ListStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);
            BusStation busStation = ListStations.Find(b => (b.BusStationKey == station.BusStationKey & b.IsActive));
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
        public void AddConsecutiveStations(ConsecutiveStations consecutiveStations) //adds a consecotive station
        {
            XElement ConsecutiveStationRootElem = XMLTools.LoadListFromXMLElement(ConsecutiveStationsPath);

            XElement ConsecutiveStationSearch = (from p in ConsecutiveStationRootElem.Elements()
                                   where (p.Element("Station1Key").Value) == consecutiveStations.Station1Key.ToString() & (p.Element("Station2Key").Value) == consecutiveStations.Station2Key.ToString() && Boolean.Parse((p.Element("IsActive").Value))
                                                 select p).FirstOrDefault();

            if (ConsecutiveStationSearch != null)
                throw new DO.BadConsecutiveStationsException (consecutiveStations.Station1Key, consecutiveStations.Station2Key, "Duplicate Consecutive Stations");

            XElement ConSElem = new XElement("ConsecutiveStations",
                                   new XElement("Station1Key", consecutiveStations.Station1Key),
                                   new XElement("Station2Key", consecutiveStations.Station2Key),
                                   new XElement("Distance", consecutiveStations.Distance),
                                   new XElement("IsActive", consecutiveStations.IsActive),
                                   new XElement("DriveDistanceTime", consecutiveStations.DriveDistanceTime.ToString()));

            ConsecutiveStationRootElem.Add(ConSElem);

            XMLTools.SaveListToXMLElement(ConsecutiveStationRootElem, ConsecutiveStationsPath);
        }
        public void DeletConsecutiveStations(int key1, int key2)    //deletes a conecutive station
        {
            XElement ConsecutiveStationRootElem = XMLTools.LoadListFromXMLElement(ConsecutiveStationsPath);

            XElement ConsecutiveStationSearch = (from p in ConsecutiveStationRootElem.Elements()
                                                 where (p.Element("Station1Key").Value) == key1.ToString() && (p.Element("Station2Key").Value) == key2.ToString() && Boolean.Parse(p.Element("IsActive").Value)
                                                 select p).FirstOrDefault();

            if (ConsecutiveStationSearch == null)
                throw new DO.BadConsecutiveStationsException(key1, key2, "Duplicate Consecutive Stations");


            (ConsecutiveStationSearch.Element("IsActive").Value) = false.ToString();

            XMLTools.SaveListToXMLElement(ConsecutiveStationRootElem, ConsecutiveStationsPath);
        }
        public IEnumerable<ConsecutiveStations> GetAlConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate)    //gets all consecutive station by a predicate
        {
            XElement ConsecutiveStationRootElem = XMLTools.LoadListFromXMLElement(ConsecutiveStationsPath);

            return (from p in ConsecutiveStationRootElem.Elements()
                    let  c= new ConsecutiveStations()
                    {
                        Station1Key = Int32.Parse(p.Element("Station1Key").Value),
                        Station2Key = Int32.Parse(p.Element("Station2Key").Value),
                        DriveDistanceTime = TimeSpan.Parse(p.Element("DriveDistanceTime").Value),
                        IsActive = Boolean.Parse(p.Element("IsActive").Value),
                        Distance = Double.Parse(p.Element("Distance").Value),

                    }
                    where(predicate(c)& c.IsActive)
                    select c
                   );
        }
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations() //get all consecutive stations
        {
            XElement ConsecutiveStationRootElem = XMLTools.LoadListFromXMLElement(ConsecutiveStationsPath);

            return (from p in ConsecutiveStationRootElem.Elements()
                    select new ConsecutiveStations()
                    {
                        Station1Key = Int32.Parse(p.Element("Station1Key").Value),
                        Station2Key = Int32.Parse(p.Element("Station2Key").Value),
                        DriveDistanceTime = TimeSpan.Parse(p.Element("DriveDistanceTime").Value),
                        IsActive = Boolean.Parse(p.Element("IsActive").Value),
                        Distance =Int32.Parse( p.Element("Distance").Value)                       
                    }
                   );
        }
        public ConsecutiveStations GetConsecutiveStations(int key1, int key2)   //get consecutive station
        {
            XElement ConsecutiveStationRootElem = XMLTools.LoadListFromXMLElement(ConsecutiveStationsPath);
            ConsecutiveStations ConsecutiveStationSearch = (from p in ConsecutiveStationRootElem.Elements()
                                                            where (p.Element("Station1Key").Value) == key1.ToString() && (p.Element("Station2Key").Value) == key2.ToString() && Boolean.Parse((p.Element("IsActive").Value))
                                                            select new ConsecutiveStations()
                                                            {
                                                                Station1Key = Int32.Parse(p.Element("Station1Key").Value),
                                                                Station2Key = Int32.Parse(p.Element("Station2Key").Value),
                                                                Distance = Double.Parse(p.Element("Distance").Value),
                                                                IsActive = Boolean.Parse(p.Element("IsActive").Value),
                                                                DriveDistanceTime = TimeSpan.Parse(p.Element("DriveDistanceTime").Value)
                                                            }).FirstOrDefault();
            if (ConsecutiveStationSearch == null)
                throw new DO.BadConsecutiveStationsException(key1, key2, "Consecutive Stations not found");
            return ConsecutiveStationSearch;
        }

        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)  //updates consecutive station
        {
            XElement ConsecutiveStationRootElem = XMLTools.LoadListFromXMLElement(ConsecutiveStationsPath);

            XElement ConsecutiveStationSearch = (from p in ConsecutiveStationRootElem.Elements()
                                                 where (p.Element("Station1Key").Value) == consecutiveStations.Station1Key.ToString() & (p.Element("Station2Key").Value) == consecutiveStations.Station2Key.ToString() && Boolean.Parse((p.Element("IsActive").Value))
                                                 select p).FirstOrDefault();

            if (ConsecutiveStationSearch == null)
                throw new DO.BadConsecutiveStationsException(consecutiveStations.Station1Key, consecutiveStations.Station2Key, "Consecutive Stations not found");
            else
            {
                ConsecutiveStationSearch.Element("Station1Key").Value = consecutiveStations.Station1Key.ToString();
                ConsecutiveStationSearch.Element("Station2Key").Value = consecutiveStations.Station2Key.ToString();
                ConsecutiveStationSearch.Element("IsActive").Value = consecutiveStations.IsActive.ToString();
                ConsecutiveStationSearch.Element("Distance").Value = consecutiveStations.Distance.ToString();
                ConsecutiveStationSearch.Element("DriveDistanceTime").Value = consecutiveStations.DriveDistanceTime.ToString();
            }

            XMLTools.SaveListToXMLElement(ConsecutiveStationRootElem, ConsecutiveStationsPath);
        }
        #endregion

        #region UserXML
        public void AddUser(User user)  //adds user
        {

            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            XElement userSearch = (from p in UserRootElem.Elements()
                             where (p.Element("UserName").Value) == user.UserName& Boolean.Parse(p.Element("IsActive").Value)
                                   select p).FirstOrDefault();

            if (userSearch != null)
                throw new DO.BadUserNameException(user.UserName, "Duplicate user name");
           
            string decode = hashPassword(user.Password + user.Salt.ToString());
           
            XElement UserElem = new XElement("User",
                                   new XElement("UserName", user.UserName),
                                   new XElement("Password",decode ),
                                   new XElement("Salt", user.Salt),
                                   new XElement("ManagementPermission", user.ManagementPermission),
                                   new XElement("IsActive", user.IsActive),
                                   new XElement("Gender", user.Gender.ToString()),
                                   new XElement("imagePath", user.imagePath));

            UserRootElem.Add(UserElem);

            XMLTools.SaveListToXMLElement(UserRootElem, UserPath);
        }
        public void DeletUser(string userName)  //delete user
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            XElement userSearch = (from p in UserRootElem.Elements()
                                   where (p.Element("UserName").Value) == userName &&Boolean.Parse(p.Element("IsActive").Value)
                                   select p).FirstOrDefault();

            if (userSearch == null)
                throw new DO.BadUserNameException(userName, "user not found");
            (userSearch.Element("IsActive").Value) = false.ToString();

            XMLTools.SaveListToXMLElement(UserRootElem, UserPath);
        }
        public IEnumerable<User> GetAllUsers()  //get all users
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            return (from p in UserRootElem.Elements()
                    where (Boolean.Parse(p.Element("IsActive").Value))
                    select new User()
                    {
                        UserName = p.Element("UserName").Value,
                        Password = p.Element("Password").Value,
                        Salt =Int32.Parse(p.Element("Salt").Value),
                        Gender = (gender)Enum.Parse(typeof(gender), p.Element("Gender").Value),
                        IsActive = Boolean.Parse(p.Element("IsActive").Value),
                        ManagementPermission = Boolean.Parse(p.Element("ManagementPermission").Value),
                        imagePath= p.Element("imagePath").Value

                    }
                   );
        }
        public IEnumerable<User> GetAlUersBy(Predicate<User> predicate)    //get all users by predicat
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            return (from p in UserRootElem.Elements()
                    where (Boolean.Parse(p.Element("IsActive").Value))
                    let u = new User()
                    {
                        UserName = p.Element("UserName").Value,
                        Password = p.Element("Password").Value,
                        Salt = Int32.Parse(p.Element("Salt").Value),
                        Gender = (gender)Enum.Parse(typeof(gender), p.Element("Gender").Value),
                        IsActive = Boolean.Parse(p.Element("IsActive").Value),
                        ManagementPermission = Boolean.Parse(p.Element("ManagementPermission").Value),
                        imagePath = p.Element("imagePath").Value,
                    }
                    where predicate(u)
                    select u
                   );
        }
        public User GetUser(string userName)    //get user
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            User u = (from p in UserRootElem.Elements()
                      where (p.Element("UserName").Value) == userName && Boolean.Parse(p.Element("IsActive").Value)
                      select new User()
                      {
                          UserName = p.Element("UserName").Value,
                          Password = (p.Element("Password").Value),
                          Gender = (gender)Enum.Parse(typeof(gender), p.Element("Gender").Value),
                          IsActive = Boolean.Parse(p.Element("IsActive").Value),
                          ManagementPermission = Boolean.Parse(p.Element("ManagementPermission").Value),
                          imagePath = p.Element("imagePath").Value,
                          Salt= Int32.Parse(p.Element("Salt").Value)
                      }
                        ).FirstOrDefault();

            if (u == null)
                throw new DO.BadUserNameException(userName, "user not found");

            return u;
        }
        public void UpdateUser(User user)   //updates user
        {
            XElement UserRootElem = XMLTools.LoadListFromXMLElement(UserPath);

            XElement userSearch = (from p in UserRootElem.Elements()
                                   where (p.Element("UserName").Value) == user.UserName && Boolean.Parse(p.Element("IsActive").Value)
                                   select p).FirstOrDefault();

            if (userSearch == null)
                throw new DO.BadUserNameException(user.UserName, "user not found");
            else
            {
                userSearch.Element("UserName").Value = user.UserName;
                userSearch.Element("Password").Value = user.Password;
                userSearch.Element("IsActive").Value = user.IsActive.ToString();
                userSearch.Element("ManagementPermission").Value = user.ManagementPermission.ToString();
                userSearch.Element("imagePath").Value = user.imagePath;
                userSearch.Element("Salt").Value = user.Salt.ToString();
                userSearch.Element("Gender").Value = user.Gender.ToString();
            }
            XMLTools.SaveListToXMLElement(UserRootElem, UserPath);
        }
        private static string hashPassword(string passwordWithSalt)
        {
            SHA512 shaM = new SHA512Managed();
            return Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt)));
        }

        #endregion

        #region BusSchedules
        public BusesSchedule GetBusesSchedule(int busLineKey, TimeSpan time)    //get bus Schedule
        {
            XElement ScheduleRootElem = XMLTools.LoadListFromXMLElement(BusSchedulePath);

            BusesSchedule BSchedule = (from p in ScheduleRootElem.Elements()
                                       where (p.Element("BusLineKey").Value) == busLineKey.ToString() && Boolean.Parse(p.Element("IsActive").Value) & TimeSpan.Parse(p.Element("StartHour").Value )==time
                                       select new BusesSchedule()
                                       {
                                           BusLineKey = Int32.Parse(p.Element("BusLineKey").Value),
                                           StartHour = TimeSpan.Parse(p.Element("StartHour").Value),
                                           IsActive = Boolean.Parse(p.Element("IsActive").Value)
                                       }
                         ).FirstOrDefault();

            if (BSchedule == null)
                throw new DO.BadBusesScheduleKeyException(busLineKey, time.ToString(), "schedule not found");

            return BSchedule;
        }

        public IEnumerable<BusesSchedule> GetAllBusSchedules()  //get all Schedule of buss
        {
            XElement ScheduleRootElem = XMLTools.LoadListFromXMLElement(BusSchedulePath);

            return (from p in ScheduleRootElem.Elements()
                    where Boolean.Parse(p.Element("IsActive").Value)
                    select new BusesSchedule()
                    {
                        BusLineKey = Int32.Parse(p.Element("BusLineKey").Value),
                        StartHour = TimeSpan.Parse(p.Element("StartHour").Value),
                        IsActive = Boolean.Parse(p.Element("IsActive").Value)
                    }
                         );
        }

        public IEnumerable<BusesSchedule> GetAllBusSchedulesBy(Predicate<BusesSchedule> predicate)  //get all bus Schedules by predicat
        {
            XElement ScheduleRootElem = XMLTools.LoadListFromXMLElement(BusSchedulePath);

            return (from p in ScheduleRootElem.Elements()
                    let sch= new BusesSchedule()
                    {
                        BusLineKey = Int32.Parse(p.Element("BusLineKey").Value),
                        StartHour = TimeSpan.Parse(p.Element("StartHour").Value),
                        IsActive = Boolean.Parse(p.Element("IsActive").Value)
                    }
                    where (sch.IsActive& predicate(sch))
                    select sch);
        }

        public void AddBusSchedule(BusesSchedule schedule)  //adds bus Schedule
        {
            XElement ScheduleRootElem = XMLTools.LoadListFromXMLElement(BusSchedulePath);

            XElement ScheduleSearch = (from p in ScheduleRootElem.Elements()
                                       where (p.Element("BusLineKey").Value) == schedule.BusLineKey.ToString() && Boolean.Parse(p.Element("IsActive").Value) & TimeSpan.Parse(p.Element("StartHour").Value) == schedule.StartHour
                                       select p).FirstOrDefault();

            if (ScheduleSearch != null)
                throw new DO.BadBusesScheduleKeyException(schedule.BusLineKey, schedule.StartHour.ToString(), "Duplicate schedule");

            XElement ScheduleElem = new XElement("BusesSchedule",
                                   new XElement("BusLineKey", schedule.BusLineKey.ToString()),
                                   new XElement("StartHour", schedule.StartHour.ToString()),
                                   new XElement("IsActive", schedule.IsActive));

            ScheduleRootElem.Add(ScheduleElem);

            XMLTools.SaveListToXMLElement(ScheduleRootElem, BusSchedulePath);
        }

        public void UpdateBusSchedule(BusesSchedule scheduleold, BusesSchedule schedulenew) //updates bus Schedule
        {
            XElement ScheduleRootElem = XMLTools.LoadListFromXMLElement(BusSchedulePath);

            XElement ScheduleSearch = (from p in ScheduleRootElem.Elements()
                                       where (p.Element("BusLineKey").Value) == scheduleold.BusLineKey.ToString() && Boolean.Parse(p.Element("IsActive").Value) & TimeSpan.Parse(p.Element("StartHour").Value) == scheduleold.StartHour
                                       select p).FirstOrDefault();

            if (ScheduleSearch == null)
                throw new DO.BadBusesScheduleKeyException(scheduleold.BusLineKey, scheduleold.StartHour.ToString(), "schedule not found");
            DeleteBusSchedule(scheduleold.BusLineKey, scheduleold.StartHour);
            AddBusSchedule(schedulenew);

            XMLTools.SaveListToXMLElement(ScheduleRootElem, BusSchedulePath);
        }

        public void DeleteBusSchedule(int busLineKey, TimeSpan time)    //deletes bus Schedule
        {
            XElement ScheduleRootElem = XMLTools.LoadListFromXMLElement(BusSchedulePath);

            XElement ScheduleSearch = (from p in ScheduleRootElem.Elements()
                                       where (p.Element("BusLineKey").Value) == busLineKey.ToString() && Boolean.Parse(p.Element("IsActive").Value) & TimeSpan.Parse(p.Element("StartHour").Value) == time
                                       select p).FirstOrDefault();

            if (ScheduleSearch == null)
                throw new DO.BadBusesScheduleKeyException(busLineKey, time.ToString(), "schedule not found");
            (ScheduleSearch.Element("IsActive").Value) = false.ToString();

            XMLTools.SaveListToXMLElement(ScheduleRootElem, BusSchedulePath);
        }


        #endregion

        #region RunNumbers
        public int GetRunNumber_BusLIne()   //get run number of bus line
        {

            List<string> ListRunNumbers = XMLTools.LoadListFromXMLSerializer<string>(RunNumbersPath);
            string RunNumberBus = ListRunNumbers.FirstOrDefault(b => b.StartsWith("BusLineRumNumber"));
            if (RunNumberBus != null)
            {
                ListRunNumbers.Remove(RunNumberBus);
                XMLTools.SaveListToXMLSerializer(ListRunNumbers, RunNumbersPath);
                ListRunNumbers.Add("BusLineRumNumber" + ((Int32.Parse(RunNumberBus.Remove(0, "BusLineRumNumber".Length))) + 1).ToString());
                XMLTools.SaveListToXMLSerializer(ListRunNumbers, RunNumbersPath);
                return Int32.Parse(RunNumberBus.Remove(0, "BusLineRumNumber".Length));
            }
            else
            {
                ListRunNumbers.Add("BusLineRumNumber" + 20000.ToString());
                XMLTools.SaveListToXMLSerializer(ListRunNumbers, RunNumbersPath);
                return -1;
            }
        }

        public int GetRunNumber_BusStation()    //get run number of station
        {
            List<string> ListRunNumbers = XMLTools.LoadListFromXMLSerializer<string>(RunNumbersPath);
            string RunNumberBusStation = ListRunNumbers.FirstOrDefault(b => b.StartsWith("BusStationRumNumber"));
            if (RunNumberBusStation != null)
            {
                ListRunNumbers.Remove(RunNumberBusStation);
                XMLTools.SaveListToXMLSerializer(ListRunNumbers, RunNumbersPath);
                ListRunNumbers.Add("BusStationRumNumber" + ((Int32.Parse(RunNumberBusStation.Remove(0, "BusStationRumNumber".Length))) + 1).ToString());
                XMLTools.SaveListToXMLSerializer(ListRunNumbers, RunNumbersPath);
                return Int32.Parse(RunNumberBusStation.Remove(0, "BusStationRumNumber".Length));
            }
            else
            {
                ListRunNumbers.Add("BusStationRumNumber" + 39051.ToString());
                XMLTools.SaveListToXMLSerializer(ListRunNumbers, RunNumbersPath);
                return -1;
            }
        }

        public int GetRunNumber_BusesSChedule() //get bus Schedule run number
        {
            List<string> ListRunNumbers = XMLTools.LoadListFromXMLSerializer<string>(RunNumbersPath);
            string RunNumberBusesSChedule = ListRunNumbers.FirstOrDefault(b => b.StartsWith("BusesSCheduleRumNumber"));
            if (RunNumberBusesSChedule != null)
            {
                ListRunNumbers.Remove(RunNumberBusesSChedule);
                XMLTools.SaveListToXMLSerializer(ListRunNumbers, RunNumbersPath);
                ListRunNumbers.Add("BusesSCheduleRumNumber"+((Int32.Parse(RunNumberBusesSChedule.Remove(0, "BusesSCheduleRumNumber".Length))) + 1).ToString());
                XMLTools.SaveListToXMLSerializer(ListRunNumbers, RunNumbersPath);
                return Int32.Parse(RunNumberBusesSChedule.Remove(0, "BusesSCheduleRumNumber".Length));
            }
            else
            {
                ListRunNumbers.Add("BusesSCheduleRumNumber" + 40048.ToString());
                XMLTools.SaveListToXMLSerializer(ListRunNumbers, RunNumbersPath);
                return -1;
            }
        }
        #endregion


    }
}
