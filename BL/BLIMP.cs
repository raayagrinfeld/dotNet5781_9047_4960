using System;
using System;
using BlApi;
using APIDL;
//using DL;
using BO;
using System.Device.Location;
using DO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    public class BlImp1 : IBL
    {
        IDAL dl = DalFactory.GetDal();

        #region singelton
        static readonly BlImp1 instance = new BlImp1();
        static BlImp1() { }// static ctor to ensure instance init is done just before first usage
        BlImp1() { } // default => private
        public static BlImp1 Instance { get => instance; }// The public Instance property to use
        #endregion

        #region runNumber
        public int getNextBusLineRunNumber()
        {
            return dl.GetRunNumber_BusLIne();
        }

        public int getNextBusStationRunNumber()
        {
            return dl.GetRunNumber_BusStation();
        }
        public int getNextBusScheduleRunNumber()
        {
            return dl.GetRunNumber_BusesSChedule();
        }
        #endregion

        #region BusLineBO
        BusLineBO BusLineDOBOAdapter(DO.BusLine busLineDo)
        {
            BusLineBO busLineBO = new BusLineBO();
            int BusLineKeyOfDO = busLineDo.BusLineKey;
            try
            {
                busLineDo = dl.GetBusLine(BusLineKeyOfDO);
            }
            catch (DO.BadBusLineKeyException ex)
            {
                throw new BO.BadBusLineKeyException("bus line key does not exist", ex);
            }
            busLineDo.CopyPropertiesTo(busLineBO);

            busLineBO.busLineStations = from b in dl.GetAllBusLineStationBy(b => (b.BusLineKey == BusLineKeyOfDO & b.IsActive))
                                        let busStationKey2 = dl.GetBusLineStationKey(BusLineKeyOfDO, b.StationNumberInLine - 1)
                                        let ConsecutiveStations = dl.GetConsecutiveStations(busStationKey2, b.BusStationKey)
                                        select ConsecutiveStations.CopyToBusLineStationBO(b);

            return busLineBO;
        }
        DO.BusLine BusLineBODOAdapter(BO.BusLineBO busLineBO)
        {
            DO.BusLine busLineDO = new BusLine();
            busLineBO.CopyPropertiesTo(busLineDO);
            return busLineDO;
        }
        public bool HasBusStation(BusLineBO busLine, int stationKey)
        {
            if (busLine.busLineStations.FirstOrDefault(b => (b.BusStationKey == stationKey & b.IsActive)) != null)
            {
                return true;
            }
            return false;
        }
        public BusLineBO GetBusLine(int busLineKey)
        {
            try
            {
                return BusLineDOBOAdapter(dl.GetBusLine(busLineKey));
            }
            catch (DO.BadBusLineKeyException busExaption)
            {
                throw new BO.BadBusLineKeyException("this bus does not exsist", busExaption);
            }

        }
        public IEnumerable<BusLineBO> GetAllBusLines()
        {
            return from b in dl.GetAllBusLinesBy(b => b.IsActive)
                   select BusLineDOBOAdapter(b);
        }
        public IEnumerable<BusLineBO> GetAllBusLinesBy(Predicate<BusLineBO> predicate)
        {
            return from b in GetAllBusLines()
                   where predicate(b)
                   select b;
        }
        public void UpdateBusLine(BusLineBO busLine)
        {
            try
            {
                dl.UpdateBusLine(BusLineBODOAdapter(busLine));
            }
            catch (DO.BadBusLineKeyException busExaption)
            {
                throw new BO.BadBusLineKeyException("this bus does not exsist", busExaption);
            }
        }
        public void AddBusLine(BusLineBO bus)
        {
            try
            {
                dl.AddBusLine(BusLineBODOAdapter(bus));
            }
            catch (DO.BadBusLineKeyException busExaption)
            {
                throw new BO.BadBusLineKeyException("this bus already exsist", busExaption);
            }
        }
        public void DeleteBusLine(int busLineKey)
        {
            try
            {
                dl.DeleteBusLine(busLineKey);
            }
            catch (DO.BadBusLineKeyException busExaption)
            {
                throw new BO.BadBusLineKeyException("this bus not exsist", busExaption);
            }
        }
        public IEnumerable<IGrouping<BO.Areas, BusLineBO>> GetBusLineGrouptByArea()
        {
            return from BusLine in GetAllBusLines()
                   group BusLine by BusLine.Area into g
                   select g;
        }
        #endregion

        #region BusLineStationBO
        BO.BusLineStationBO BusLineStationDOBOAdapter(DO.BusLineStation busLineStationDo)
        {
            BO.BusLineStationBO busLineStationBO = new BO.BusLineStationBO();
            var busStationKey2 = dl.GetBusLineStationKey(busLineStationDo.BusLineKey, busLineStationDo.StationNumberInLine - 1);
            busLineStationBO = dl.GetConsecutiveStations(busStationKey2, busLineStationDo.BusStationKey).CopyToBusLineStationBO(busLineStationDo);
            return busLineStationBO;
        }
        DO.BusLineStation BusLineStationBODOAdapter(BO.BusLineStationBO busLineStationBo)
        {
            DO.BusLineStation busLineStationDO = new DO.BusLineStation();
            busLineStationBo.CopyPropertiesTo(busLineStationDO);
            return busLineStationDO;
        }
        public double DistanceBetweanStations(BusLineBO busLine, int firstStationKey, int lastStationKey)
        {
            List<BusLineStationBO> PartOfBusLineStation = busLine.busLineStations.OrderBy(b => b.StationNumberInLine).ToList().FindAll(b => b.StationNumberInLine >= firstStationKey & b.StationNumberInLine <= lastStationKey & b.IsActive);
            double ColectiveDistance = 0;
            foreach (BusLineStationBO BLStationBO in PartOfBusLineStation)
            {
                ColectiveDistance += BLStationBO.DistanceFromLastStation;
            }
            return ColectiveDistance;
        }
        public TimeSpan TimeBetweanStations(BusLineBO busLine, int firstStationKey, int lastStationKey)
        {
            List<BusLineStationBO> PartOfBusLineStation = busLine.busLineStations.OrderBy(b => b.StationNumberInLine).ToList().FindAll(b => b.StationNumberInLine >= firstStationKey & b.StationNumberInLine <= lastStationKey & b.IsActive);
            TimeSpan timeOfDravel = TimeSpan.Zero;
            foreach (BusLineStationBO BLStationBO in PartOfBusLineStation)
            {
                timeOfDravel += BLStationBO.DriveDistanceTimeFromLastStation;
            }
            return timeOfDravel;
        }
        public void AddStation(BusLineBO busLine, int stationKey)
        {
            try
            {
                var thisBusStation = dl.GetBusStation(stationKey);
                if (busLine.busLineStations == null || busLine.busLineStations.Count() == 0)
                {
                    busLine.FirstStation = stationKey;
                    busLine.FirstStationName = thisBusStation.StationName;
                    if (busLine.busLineStations == null)
                        busLine.busLineStations = new List<BusLineStationBO>();
                }
                var BLstation = new BusLineStation { BusLineKey = busLine.BusLineKey, BusStationKey = stationKey, StationNumberInLine = busLine.busLineStations.Count() + 1, IsActive = true, StationName = thisBusStation.StationName };
                ConsecutiveStations ConsecutiveStation = new ConsecutiveStations { Station1Key = dl.GetBusLineStationKey(busLine.BusLineKey, busLine.busLineStations.Count()), Station2Key = stationKey, IsActive = true };
                dl.AddBusLineStation(BLstation);
                if (ConsecutiveStation.Station1Key == -1)
                {
                    ConsecutiveStation.Distance = 0;
                    ConsecutiveStation.DriveDistanceTime = TimeSpan.Zero;
                }
                else
                {
                    ConsecutiveStation.Distance = thisBusStation.Coordinates.GetDistanceTo(GetBusStation(ConsecutiveStation.Station1Key).Coordinates);
                    ConsecutiveStation.DriveDistanceTime = TimeSpan.FromMinutes(ConsecutiveStation.Distance * 0.01);
                }
                try
                {
                    dl.AddConsecutiveStations(ConsecutiveStation);
                }
                catch (DO.BadConsecutiveStationsException)
                { }
                //busLine.busLineStations = from b in dl.GetAllBusLineStationBy(b => (b.BusLineKey == busLine.BusLineKey & b.IsActive))
                //                          let busStationKey2 = dl.GetBusLineStationKey(busLine.BusLineKey, b.StationNumberInLine - 1)
                //                          let ConsecutiveStations = dl.GetConsecutiveStations(busStationKey2, b.BusStationKey)
                //                          select ConsecutiveStations.CopyToBusLineStationBO(b);
                busLine.LastStation = stationKey;
                busLine.LastStationName = thisBusStation.StationName;
                UpdateBusLine(busLine);
            }
            catch (DO.BadBusStationKeyException ex)
            {
                throw new BO.BadBusStationKeyException("the station does not exsist", ex);
            }
            catch (DO.BadBusLineKeyException ex)
            {
                throw new BO.BadBusLineKeyException("the line does not exsist", ex);
            }
            catch (DO.BadBusLineStationsException ex)
            {
                throw new BO.BadBusLineStationsException(ex.Message, ex);
            }
        }
        public void deleteBusStationInBusLine(BusLineBO busLine, int stationKey)
        {
            BusLineStationBO station_to_delete;
            StationBO PrevStation;
            StationBO NextStation;
            if (busLine.busLineStations.Count() < 3)
            {
                throw new BO.BadBusLineStationsException(busLine.BusLineKey, stationKey, "can not delete, mini amount of station is 2");
            }
            try
            {
                station_to_delete = BusLineStationDOBOAdapter(dl.GetBusLineStation(busLine.BusLineKey, stationKey));
                dl.DeleteBusLineStationInOneBusLine(stationKey, busLine.BusLineKey);
            }
            catch (DO.BadBusLineStationsException ex)
            {
                throw new BO.BadBusLineStationsException(ex.Message, ex);
            }
            if (busLine.FirstStation != stationKey && busLine.LastStation != stationKey)
            {
                PrevStation = GetBusStation(dl.GetBusLineStationKey(busLine.BusLineKey, station_to_delete.StationNumberInLine - 1));
                NextStation = GetBusStation(dl.GetBusLineStationKey(busLine.BusLineKey, station_to_delete.StationNumberInLine));
                var newConsecutiveStation = new ConsecutiveStations { Station1Key = PrevStation.BusStationKey, Station2Key = NextStation.BusStationKey, IsActive = true };
                newConsecutiveStation.Distance = PrevStation.Coordinates.GetDistanceTo(NextStation.Coordinates);
                newConsecutiveStation.DriveDistanceTime = TimeSpan.FromMinutes(newConsecutiveStation.Distance * 0.01);
                try
                {
                    dl.AddConsecutiveStations(newConsecutiveStation);
                }
                catch (DO.BadConsecutiveStationsException)//tis okay if the consecutive station allrady exist
                { }
            }
            else
            {
                if (busLine.LastStation == stationKey)
                {
                    PrevStation = GetBusStation(dl.GetBusLineStationKey(busLine.BusLineKey, station_to_delete.StationNumberInLine - 1));
                    busLine.LastStation = PrevStation.BusStationKey;
                    busLine.LastStationName = PrevStation.StationName;
                }
                else
                {
                    if (busLine.FirstStation == stationKey)
                    {
                        NextStation = GetBusStation(dl.GetBusLineStationKey(busLine.BusLineKey, station_to_delete.StationNumberInLine));
                        var newConsecutiveStation = new ConsecutiveStations { Station1Key = -1, Station2Key = NextStation.BusStationKey, IsActive = true, Distance = 0, DriveDistanceTime = TimeSpan.Zero };
                        try
                        {
                            dl.AddConsecutiveStations(newConsecutiveStation);
                        }
                        catch (DO.BadConsecutiveStationsException)//tis okay if the consecutive station allrady exist
                        { }
                        busLine.FirstStation = NextStation.BusStationKey;
                        busLine.FirstStationName = NextStation.StationName;
                    }
                }
            }
            UpdateBusLine(busLine);
        }

        public IEnumerable<BO.BusLineStationBO> GetAllBusLineStationOfBusLine(BusLineBO busLine)
        {
            return busLine.busLineStations;
        }
        #endregion

        #region StationBO
        public BO.StationBO BusStationDOBOAdapter(DO.BusStation busStationDo)
        {
            BO.StationBO busStationBO = new BO.StationBO();
            busStationDo.CopyPropertiesTo(busStationBO);
            busStationBO.Coordinates = busStationDo.Coordinates;
            busStationBO.busLines = from b in GetAllBusLines()
                                    where (b.busLineStations.FirstOrDefault
                                    (s => (s.BusStationKey == busStationDo.BusStationKey & s.IsActive)) != null)
                                    select b;
            return busStationBO;
        }
        DO.BusStation BusStationBODOAdapter(BO.StationBO busStationBO)
        {
            DO.BusStation busStationDO = new BusStation();
            busStationBO.CopyPropertiesTo(busStationDO);
            busStationDO.Coordinates = busStationBO.Coordinates;
            return busStationDO;
        }
        public StationBO GetBusStation(int busStationKey)
        {
            try
            {
                return BusStationDOBOAdapter(dl.GetBusStation(busStationKey));
            }
            catch (DO.BadBusStationKeyException ex)
            {
                throw new BO.BadBusStationKeyException("this station does not exsist", ex);
            }
        }
        public IEnumerable<StationBO> GetAllBusStations()
        {
            return from b in dl.GetAllBusStationsBy(b => b.IsActive)
                   select BusStationDOBOAdapter(b);
        }

        public IEnumerable<StationBO> GetAllBusStationsBy(Predicate<StationBO> predicate)
        {
            return from b in GetAllBusStations()
                   where predicate(b)
                   select b;
        }
        public void AddBusStation(StationBO station)
        {
            try
            {
                dl.AddBusStation(BusStationBODOAdapter(station));
            }
            catch (DO.BadBusStationKeyException busExaption)
            {
                throw new BO.BadBusStationKeyException("dupliceited bus lune station", busExaption);
            }
        }
        public void DeleteBusStation(int busStationKey)
        {
            try
            {
                dl.DeleteBusLineStationAllBusLine(busStationKey);
                dl.DeleteBusStation(busStationKey);
               //IEnumerable< ConsecutiveStations >ConsecutiveStationToUpdate = dl.GetAlConsecutiveStationsBy(b => b.IsActive &&  (b.Station1Key == busStationKey || b.Station2Key == busStationKey));
               // if (ConsecutiveStationToUpdate.Count()!=0)
                //{
                 //   foreach (ConsecutiveStations item in ConsecutiveStationToUpdate)
                 //   {
                       // dl.DeletConsecutiveStations(item.Station1Key, item.Station2Key);
                 //   }
                //}
            }
            catch (DO.BadBusStationKeyException busExaption)
            {
                throw new BO.BadBusStationKeyException("this bus does not exsist", busExaption);
            }
        }
        public void UpdateBusStation(StationBO station)
        {
            try
            {
                StationBO busStationBO = GetBusStation(station.BusStationKey);
                if (busStationBO != null)
                {
                    DeleteBusStation(station.BusStationKey);
                    AddBusStation(station);
                }
                var ConsecutiveStationToUpdate = dl.GetAlConsecutiveStationsBy(b => b.IsActive && b.Station1Key != -1 && (b.Station1Key == station.BusStationKey || b.Station2Key == station.BusStationKey));
                foreach (var item in ConsecutiveStationToUpdate)
                {
                    double distance = GetBusStation(item.Station1Key).Coordinates.GetDistanceTo(GetBusStation(item.Station2Key).Coordinates);
                    ConsecutiveStations NewConsecutiveStation =new ConsecutiveStations { Station1Key = item.Station1Key, Station2Key = item.Station2Key, IsActive = true , Distance=distance};
                    NewConsecutiveStation.DriveDistanceTime= TimeSpan.FromMinutes(NewConsecutiveStation.Distance * 0.01);
                    dl.UpdateConsecutiveStations(NewConsecutiveStation);
                }
            }
            catch (DO.BadBusLineKeyException busExaption)
            {
                throw new BO.BadBusLineKeyException("this bus does not exsist", busExaption);
            }
        }
        public bool HasLine(StationBO station, int lineNumber)
        {
            if (station.busLines.FirstOrDefault(s => (s.BusLineKey == lineNumber & s.IsActive)) != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Driving
        public void AddDeatinationStation(int stationKey, Driving driving)
        {
            driving.Destination = GetBusStation(stationKey);
        }
        public void AddSourceStation(int stationKey, Driving driving)
        {
            driving.Source = GetBusStation(stationKey);
        }
        public IEnumerable<BO.BusLineBO> fingALinesBeatweenStation(Driving driving)
        {
            driving.BusLines = GetAllBusLinesBy(b =>
            {
                if (HasBusStation(b, driving.Source.BusStationKey) & (HasBusStation(b, driving.Destination.BusStationKey)))
                {
                    if (b.busLineStations.FirstOrDefault(s => (s.BusStationKey == driving.Source.BusStationKey)).StationNumberInLine
                    < b.busLineStations.FirstOrDefault(s => (s.BusStationKey == driving.Destination.BusStationKey)).StationNumberInLine)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else return false;
            });
            return driving.BusLines.OrderBy(b => (TimeBetweanStations(b, driving.Source.BusStationKey, driving.Destination.BusStationKey)));
        }
        #endregion

        #region UserBO
        BO.User UserDOBOAdapter(DO.User user)
        {
            BO.User userBO = new BO.User();
            user.CopyPropertiesTo(userBO);
            return userBO;
        }


        DO.User UserBODOAdapter(BO.User user)
        {
            DO.User userDO = new DO.User();
            user.CopyPropertiesTo(userDO);
            return userDO;
        }
        public BO.User GetUser(string userName)
        {
            try
            {
                return UserDOBOAdapter(dl.GetUser(userName));
            }
            catch (DO.BadUserNameException busExaption)
            {
                throw new BO.BadUserNameException("this username exsist", busExaption);
            }
        }
        public IEnumerable<IGrouping<bool, BO.User>> GetUserGrouptByManagment()
        {
            return from user in GetAllUsers()
                   group user by user.ManagementPermission into g
                   select g;
        }
        public IEnumerable<BO.User> GetAllUsers()
        {
            return from u in dl.GetAlUersBy(u => u.IsActive)
                   select UserDOBOAdapter(u);
        }
        public IEnumerable<BO.User> GetAlUersBy(Predicate<BO.User> predicate)
        {
            return from u in GetAllUsers()
                   where predicate(u)
                   select u;
        }
        public void AddUser(BO.User user)
        {
            try
            {
                dl.AddUser(UserBODOAdapter(user));
            }
            catch (DO.BadUserNameException busExaption)
            {
                throw new BO.BadUserNameException("this username exsist", busExaption);
            }
        }
        public void DeletUser(string userName)
        {
            try
            {
                dl.DeletUser(userName);
            }
            catch (DO.BadUserNameException busExaption)
            {
                throw new BO.BadUserNameException("this username doesnt exist", busExaption);
            }
        }
        public void UpdateUser(BO.User user)
        {
            try
            {
                BO.User userBO = GetUser(user.UserName);
                if (userBO != null)
                {
                    DeletUser(user.UserName);
                    AddUser(user);
                }
            }
            catch (DO.BadUserNameException busExaption)
            {
                throw new BO.BadUserNameException("this username doesnt exsist", busExaption);
            }
        }


        #endregion

        #region DrivingLine
       
        DrivingLine DrivingLineBODOAdapter(DO.BusesSchedule busesSchedule)
        {
            return null;
        }
        public DrivingLine GetDrivingLine(int ScheduleKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DrivingLine> GetAllDrivings()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DrivingLine> GetAllDrivingsBy(Predicate<DrivingLine> predicate)
        {
            throw new NotImplementedException();
        }

        public void AddDrivingLine(DrivingLine drivingLine)
        {
            throw new NotImplementedException();
        }

        public void UpdateDrivingLine(DrivingLine drivingLine)
        {
            throw new NotImplementedException();
        }

        public void DeleteDrivingLine(int ScheduleKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DrivingLine> BusLineInDrivingToStation(StationBO DestinationStation, TimeSpan time)
        {
            return GetAllDrivingsBy(b => b.IsActive && b.StartHour < time && b.ArrivalTime > time, DestinationStation);
        }

        public DrivingLine GetDrivingLine(int ScheduleKey, StationBO DestinationStation)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DrivingLine> GetAllDrivings(StationBO DestinationStation)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DrivingLine> GetAllDrivingsBy(Predicate<DrivingLine> predicate, StationBO DestinationStation)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}