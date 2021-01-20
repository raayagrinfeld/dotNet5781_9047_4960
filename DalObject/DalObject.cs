
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APIDL;
using DO;
using DS;

namespace DL
{
    sealed class DalObject : IDAL
    {
        #region singelton
        static readonly DalObject instance = new DalObject();
        static DalObject() { }// static ctor to ensure instance init is done just before first usage
        DalObject() { } // default => private
        public static DalObject Instance { get => instance; }// The public Instance property to use
        #endregion

        #region BusLine
        public BusLine GetBusLine(int busLineKey)
        {
            BusLine busLine = DataSource.BusLineList.FirstOrDefault(b => (b.BusLineKey == busLineKey & b.IsActive));
            if (busLine != null)
                return busLine.Clone();
            else
                throw new BadBusLineKeyException(busLineKey, $"bad bus line key: {busLineKey}");
        }
        public IEnumerable<BusLine> GetAllBusLines()
        {
            return from BusLine in DataSource.BusLineList
                   where BusLine.IsActive
                   select BusLine.Clone();
        }
        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            return from BusLine in DataSource.BusLineList
                   where (predicate(BusLine)) & (BusLine.IsActive)
                   select BusLine.Clone();
        }
        public void AddBusLine(BusLine bus)
        {
            if (DataSource.BusLineList.FirstOrDefault(b => (b.BusLineKey == bus.BusLineKey & b.IsActive)) != null)
                throw new BadBusLineKeyException(bus.BusLineKey, "Duplicate Bus Key");
            DataSource.BusLineList.Add(bus.Clone());
        }
        public void UpdateBusLine(BusLine bus)
        {
            BusLine busLine = DataSource.BusLineList.Find(b => b.BusLineKey == bus.BusLineKey);
            if (busLine != null)
            {
                DeleteBusLine(bus.BusLineKey);
                AddBusLine(bus.Clone());
            }
            else
            {
                throw new BadBusLineKeyException(bus.BusLineKey, $"bad bus line key: {bus.BusLineKey}");
            }
        }
        public void UpdateBusLine(int busLineKey, Action<BusLine> update)
        {
            throw new NotImplementedException();
        }
        public void DeleteBusLine(int busLineKey)
        {
            BusLine busLine = DataSource.BusLineList.Find(b =>
            {
                if (b.BusLineKey == busLineKey & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (busLine == null)
            {
                throw new BadBusLineKeyException(busLineKey, $"bad bus line key: {busLineKey}");
            }
        }

        #endregion

        #region BusLineStation
        public BusLineStation GetBusLineStation(int busLineKey, int key)
        {
            BusLineStation busLineStation = DataSource.BusLineStationList.Find(b => (b.BusStationKey == key & b.BusLineKey == busLineKey & b.IsActive));
            if (busLineStation != null)
            {
                return busLineStation;
            }
            throw new BadBusLineStationsException(key, busLineKey, "station is not in the bus Line path");
        }

        public int GetBusLineStationKey(int BusLineKey, int StationNumberInLine)//helping func, returns -1 if the station doesnt exist
        {
            var buslinestation = DataSource.BusLineStationList.FirstOrDefault(b => (b.BusLineKey == BusLineKey & b.StationNumberInLine == StationNumberInLine & b.IsActive));
            if (buslinestation == null)
            {
                return -1;
            }
            return buslinestation.BusStationKey;
        }

        public IEnumerable<BusLineStation> GetAllBusLineStationBy(Predicate<BusLineStation> predicate)
        {
            return from BLstation in DataSource.BusLineStationList
                   where predicate(BLstation)
                   select BLstation.Clone();
        }

        public void AddBusLineStation(BusLineStation station)
        {
            if (DataSource.BusLineStationList.FirstOrDefault(b => (b.BusStationKey == station.BusStationKey & b.BusLineKey == station.BusLineKey & b.IsActive)) != null)
                throw new BadBusLineStationsException(station.BusStationKey, station.BusLineKey, "Duplicate Bus Line Station");
            DataSource.BusLineStationList.Add(station.Clone());
        }

        public void UpdateBusLineStation(BusLineStation station)
        {
            BusLineStation busStation = DataSource.BusLineStationList.Find(b => (b.BusStationKey == station.BusStationKey & b.BusLineKey == station.BusLineKey & b.IsActive));
            if (busStation != null)
            {
                DeleteBusLineStationInOneBusLine(station.BusStationKey, station.BusLineKey);
                AddBusLineStation(station.Clone());
            }
            else
            {
                throw new BadBusLineStationsException(station.BusStationKey, station.BusLineKey, "station is not in the bus Line path");
            }
        }

        public void DeleteBusLineStationInOneBusLine(int BusStationKey, int BusLineKey)
        {
            BusLineStation busLineStation = DataSource.BusLineStationList.FirstOrDefault(b =>
            {
                if (b.BusStationKey == BusStationKey & b.BusLineKey == BusLineKey & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                return false;
            });
            if (busLineStation == null)
            {
                throw new BadBusLineStationsException(BusStationKey, BusLineKey, "station is not in the bus Line path");
            }
            DataSource.BusLineStationList.FindAll(b =>
            {
                if (b.StationNumberInLine> busLineStation.StationNumberInLine & b.BusLineKey == BusLineKey & b.IsActive)
                {
                    b.StationNumberInLine--;
                }
                return false;           //so it goes trough all the stations
            });
        }

        public void DeleteBusLineStationAllBusLine(int BusStationKey)
        {
            BusLineStation bLstation;
            do
            {
                bLstation = DataSource.BusLineStationList.Find(b =>
                {
                    if (b.BusStationKey == BusStationKey & b.IsActive)
                    {
                        b.IsActive = false;
                        return true;
                    }
                    else return false;
                });
            }
            while (bLstation != null);
        }
        #endregion
        //#region DrivingLine
        //public void addLineInTravel(DrivingLine bus)
        //{
        //    throw new NotImplementedException();
        //}
        //public IEnumerable<DrivingLine> GetBusLinetInDriveList(Predicate<DrivingLine> predicate)
        //{
        //    throw new NotImplementedException();
        //}
        //#endregion

        #region BusStation
        public BusStation GetBusStation(int busStationKey)
        {
            BusStation busStation = DataSource.BusStationList.Find(b => (b.BusStationKey == busStationKey & b.IsActive));
            if (busStation != null)
                return busStation.Clone();
            else
                throw new BadBusStationKeyException(busStationKey, $"bad bus line key: {busStationKey}");
        }
        public IEnumerable<BusStation> GetAllBusStations()
        {
            return from BusStation in DataSource.BusStationList
                   where BusStation.IsActive
                   select BusStation.Clone();
        }
        public IEnumerable<BusStation> GetAllBusStationsBy(Predicate<BusStation> predicate)
        {
            return from BusStation in DataSource.BusStationList
                   where (predicate(BusStation) & (BusStation.IsActive))
                   select BusStation.Clone();
        }
        public void AddBusStation(BusStation station)
        {
            if (DataSource.BusStationList.FirstOrDefault(b => (b.BusStationKey == station.BusStationKey & b.IsActive)) != null)
                throw new BadBusStationKeyException(station.BusStationKey, "Duplicate Bus Key");
            DataSource.BusStationList.Add(station.Clone());
        }
        public void UpdateBusStation(BusStation station)
        {
            BusStation busStation = DataSource.BusStationList.Find(b => b.BusStationKey == station.BusStationKey);
            if (busStation != null)
            {
                DeleteBusStation(station.BusStationKey);
                AddBusStation(station.Clone());
            }
            else
            {
                throw new BadBusStationKeyException(station.BusStationKey, $"bad bus line key: {station.BusStationKey}");
            }
        }
        public void DeleteBusStation(int busStationKey)
        {
            BusStation busLine = DataSource.BusStationList.Find(b =>
            {
                if (b.BusStationKey == busStationKey & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (busLine == null)
            {
                throw new BadBusStationKeyException(busStationKey, $"bad bus line key: {busStationKey}");
            }
            else
            {
                DataSource.BusLineStationList.Where(b =>
                {
                    if (b.BusStationKey == busStationKey & b.IsActive)
                    {
                        b.IsActive = false;
                        return true;
                    }
                    else return false;
                });
            }
        }
        #endregion

        #region User
        public User GetUser(string userName)
        {
            User user = DataSource.UserList.FirstOrDefault(b => (b.UserName == userName & b.IsActive));
            if (user != null)
            {
                return user.Clone();
            }
            else
            {
                throw new BadUserNameException(userName, $"bad bus user name: {userName}");
            }
        }
        public IEnumerable<User> GetAllUsers()
        {
            return from User in DataSource.UserList
                   where User.IsActive
                   select User.Clone();
        }

        public IEnumerable<User> GetAlUersBy(Predicate<User> predicate)
        {
            return from user in DataSource.UserList
                   where (predicate(user)) & (user.IsActive)
                   select user.Clone();
        }

        public void AddUser(User user)
        {
            if (DataSource.UserList.FirstOrDefault(b => (b.UserName == user.UserName & b.IsActive)) != null)
                throw new BadUserNameException(user.UserName, "Duplicate user name");
            DataSource.UserList.Add(user.Clone());
        }

        public void UpdateUser(User user)
        {
            User u = DataSource.UserList.Find(b => b.UserName == user.UserName);
            if (u != null)
            {
                DeletUser(user.UserName);
                AddUser(user.Clone());
            }
            else
            {
                throw new BadUserNameException(user.UserName, $"bad username: {user.UserName} does not exist");
            }
        }

        public void DeletUser(string userName)
        {
            User user = DataSource.UserList.Find(b =>
            {
                if (b.UserName == userName & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (user == null)
            {
                throw new BadUserNameException(userName, $"bad userName: {userName}");
            }
        }
        #endregion

        #region ConsecutiveStations
        public ConsecutiveStations GetConsecutiveStations(int key1, int key2)
        {
            ConsecutiveStations consecutiveStations = DataSource.ConsecutiveStationsList.Find(b => (b.Station1Key == key1 & b.Station2Key == key2 & b.IsActive));
            if (consecutiveStations != null)
            {
                return consecutiveStations.Clone();
            }
            else
            {
                throw new BadConsecutiveStationsException(key1, key2, $"bad Consecutive Stations keys: {key1} {key2}");
            }
        }

        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            return from ConsecutiveStations in DataSource.ConsecutiveStationsList
                   where ConsecutiveStations.IsActive
                   select ConsecutiveStations.Clone();
        }

        public IEnumerable<ConsecutiveStations> GetAlConsecutiveStationsBy(Predicate<ConsecutiveStations> predicate)
        {
            return from ConsecutiveStations in DataSource.ConsecutiveStationsList
                   where (predicate(ConsecutiveStations)) & ConsecutiveStations.IsActive
                   select ConsecutiveStations.Clone();
        }

        public void AddConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            if (DataSource.ConsecutiveStationsList.FirstOrDefault(b => (b.Station1Key == consecutiveStations.Station1Key & b.Station2Key == consecutiveStations.Station2Key & b.IsActive)) != null)
                throw new BadConsecutiveStationsException(consecutiveStations.Station1Key, consecutiveStations.Station2Key, "Duplicate consecutive Stations");
            DataSource.ConsecutiveStationsList.Add(consecutiveStations.Clone());
        }

        public void UpdateConsecutiveStations(ConsecutiveStations consecutiveStations)
        {
            ConsecutiveStations c = DataSource.ConsecutiveStationsList.Find(b => (b.Station1Key == consecutiveStations.Station1Key & b.Station2Key == consecutiveStations.Station2Key & b.IsActive));
            if (c != null)
            {
                DeletConsecutiveStations(consecutiveStations.Station1Key, consecutiveStations.Station2Key);
                AddConsecutiveStations(consecutiveStations.Clone());
            }
            else
            {
                throw new BadConsecutiveStationsException(consecutiveStations.Station1Key, consecutiveStations.Station2Key, $"bad bus user name: {consecutiveStations.Station1Key}{consecutiveStations.Station2Key}");
            }
        }

        public void DeletConsecutiveStations(int key1, int key2)
        {
            ConsecutiveStations consecutiveStations = DataSource.ConsecutiveStationsList.Find(b =>
            {
                if (b.Station1Key == key1 & b.Station2Key == key2 & b.IsActive)
                {
                    b.IsActive = false;
                    return true;
                }
                else return false;
            });
            if (consecutiveStations == null)
            {
                throw new BadConsecutiveStationsException(key1, key2, $"bad Consecutive Stations keys: {key1}{key2}");
            }
        }
        #endregion

        #region BusSchedules
        public BusesSchedule GetBusesSchedule(int scheduleKey)
        {
            return null;
        }

        public IEnumerable<BusesSchedule> GetAllBusSchedules()
        {
            return null;
        }

        public IEnumerable<BusesSchedule> GetAllBusSchedulesBy(Predicate<BusesSchedule> predicate)
        {
            return null;
        }

        public void AddBusSchedule(BusesSchedule schedule)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusSchedule(BusesSchedule schedule)
        {
        }

        public void DeleteBusSchedule(int scheduleKey)
        {
        }

        public BusesSchedule GetBusesSchedule(int busLineKey, TimeSpan time)
        {
            return null;
        }
        #endregion
    }
}