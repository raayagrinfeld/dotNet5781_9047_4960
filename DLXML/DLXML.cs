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

            if (GetBusLine(bus.BusLineKey) == null)
                throw new DO.BadBusLineKeyException(bus.BusLineKey, "Missing bus line");

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
                throw new DO.BadBusLineStationsException(BusStationKey, 0, "this bus line ststion is not exsist");

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
                throw new DO.BadBusLineStationsException(BusStationKey, BusLineKey, "this bus line ststion is not exsist");

            XMLTools.SaveListToXMLSerializer(ListBusLineStations, BusLineStationPath);
        }
        #endregion

        #region BusStation
        public void AddBusStation(BusStation station)
        {
            List<BusStation> ListBusStations = XMLTools.LoadListFromXMLSerializer<BusStation>(BusStationPath);

            if (ListBusStations.FirstOrDefault(s => s.BusStationKey == station.BusStationKey & s.IsActive) != null)
                throw new DO.BadBusLineKeyException(station.BusStationKey, "This bus station already exist");

            if (GetBusStation(station.BusStationKey) == null)
                throw new DO.BadBusLineKeyException(station.BusStationKey, "Missing bus station");

            ListBusStations.Add(station); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListBusStations, BusLinePath);
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
        #endregion

        public void DeletUser(string userName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ConsecutiveStations> GetAlConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusLine> GetAllBusLines()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusLineStation> GetAllBusLineStationBy(Predicate<BusLineStation> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusStation> GetAllBusStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusStation> GetAllBusStationsBy(Predicate<BusStation> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAlUersBy(Predicate<User> predicate)
        {
            throw new NotImplementedException();
        }

        public BusLine GetBusLine(int busLineKey)
        {
            throw new NotImplementedException();
        }

        public int GetBusLineStationKey(int BusLineKey, int StationNumberInLine)
        {
            throw new NotImplementedException();
        }

        public BusStation GetBusStation(int busStationKey)
        {
            throw new NotImplementedException();
        }

        public ConsecutiveStations GetConsecutiveStations(int key1, int key2)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string userName)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusLine(BusLine bus)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusLine(int busLineKey, Action<BusLine> update)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusLineStation(BusLineStation station)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusStation(BusStation station)
        {
            throw new NotImplementedException();
        }

        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
