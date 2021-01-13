﻿using System;
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

        public void AddBusLine(BusLine bus)
        {
            List<BusLine> ListStudents = XMLTools.LoadListFromXMLSerializer<Student>(studentsPath);

            if (ListStudents.FirstOrDefault(s => s.ID == student.ID) != null)
                throw new DO.BadPersonIdException(student.ID, "Duplicate student ID");

            if (GetPerson(student.ID) == null)
                throw new DO.BadPersonIdException(student.ID, "Missing person ID");

            ListStudents.Add(student); //no need to Clone()

            XMLTools.SaveListToXMLSerializer(ListStudents, studentsPath);
        }

        #region BusLineStation
        public void AddBusLineStation(BusLineStation station)
        {
            throw new NotImplementedException();
        }

        public void AddBusStation(BusStation station)
        {
            throw new NotImplementedException();
        }

        public void AddConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            throw new NotImplementedException();
        }

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

        public void DeletConsecutiveStations(int key1, int key2)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusLine(int busLineKey)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusLineStationAllBusLine(int BusStationKey)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusLineStationInOneBusLine(int BusStationKey, int BusLineKey)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusStation(int busStationKey)
        {
            throw new NotImplementedException();
        }

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
