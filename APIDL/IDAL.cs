
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace APIDL
{
    public interface IDAL
    {
        #region BusLine
        IEnumerable<BusLine> GetAllBusLines();
        IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate);
        void AddBusLine(BusLine bus);
        void UpdateBusLine(BusLine bus);
        void DeleteBusLine(int busLineKey);
        BusLine GetBusLine(int busLineKey);
        #endregion

        #region BusStation
        BusStation GetBusStation(int busStationKey);
        IEnumerable<BusStation> GetAllBusStations();
        IEnumerable<BusStation> GetAllBusStationsBy(Predicate<BusStation> predicate);
        void AddBusStation(BusStation station);
        void UpdateBusStation(BusStation station);
        void DeleteBusStation(int busStationKey);
        #endregion

        #region BusLineStation
        IEnumerable<BusLineStation> GetAllBusLineStationBy(Predicate<BusLineStation> predicate);
        int GetBusLineStationKey(int BusLineKey, int key);
        BusLineStation GetBusLineStation(int busLineKey, int StationNumberInLine);
        void AddBusLineStation(BusLineStation station);
        void UpdateBusLineStation(BusLineStation station);
        void DeleteBusLineStationInOneBusLine(int BusStationKey, int BusLineKey);
        void DeleteBusLineStationAllBusLine(int BusStationKey);

        #endregion

        #region User
        User GetUser(string userName);
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetAlUersBy(Predicate<User> predicate);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeletUser(string userName);
        #endregion

        #region ConsecutiveStations
        ConsecutiveStations GetConsecutiveStations(int key1, int key2);
        IEnumerable<ConsecutiveStations> GetAllConsecutiveStations();
        IEnumerable<ConsecutiveStations> GetAlConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate);
        void AddConsecutiveStations(ConsecutiveStations consecutiveStations);
        void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations);
        void DeletConsecutiveStations(int key1, int key2);
        #endregion

        #region BusesSchedule
        
        BusesSchedule GetBusesSchedule(int scheduleKey);
        BusesSchedule GetBusesSchedule(int busLineKey, TimeSpan time);
        IEnumerable<BusesSchedule> GetAllBusSchedules();
        IEnumerable<BusesSchedule> GetAllBusSchedulesBy(Predicate< BusesSchedule> predicate);
        void AddBusSchedule(BusesSchedule schedule);
        void UpdateBusSchedule(BusesSchedule schedule);
        void DeleteBusSchedule(int scheduleKey);

        #endregion

        #region RunNumbers
        int GetRunNumber_BusLIne();
        int GetRunNumber_BusStation();
        int GetRunNumber_BusesSChedule();


        #endregion

    }
}