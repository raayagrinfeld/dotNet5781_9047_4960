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

namespace BL
{
    public class BlImp1 : IBL
    {
        IDAL dl = DalFactory.GetDal();

        #region BusLineBO
        BO.BusLineBO BusLineDOBOAdapter(DO.BusLine busLineDo)
        {
            BO.BusLineBO busLineBO = new BO.BusLineBO();
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
                BusLineBO busLineBO = GetBusLine(busLine.BusLineKey);
                if (busLineBO != null)
                {
                    DeleteBusLine(busLine.BusLineKey);
                    AddBusLine(busLine);
                }
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
                throw new BO.BadBusLineKeyException("this bus already exsist", busExaption);
            }
        }
        public IEnumerable<IGrouping<BO.Areas, int>> GetBusLineGrouptByArea()
        {
            return from BusLine in GetAllBusLines()
                   group BusLine.BusLineKey by BusLine.Area into g
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
                if(busLine.busLineStations.Count()==0)
                {
                    busLine.FirstStation = stationKey;
                }
                var thisBusStation=dl.GetBusStation(stationKey);
                var BLstation = new BusLineStation { BusLineKey = busLine.BusLineKey, BusStationKey = stationKey, StationNumberInLine = busLine.busLineStations.Count()+1, IsActive = true};
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
                catch(DO.BadConsecutiveStationsException)
                    { }
                busLine.busLineStations = from b in dl.GetAllBusLineStationBy(b => (b.BusLineKey == busLine.BusLineKey & b.IsActive))
                                          let busStationKey2 = dl.GetBusLineStationKey(busLine.BusLineKey, b.StationNumberInLine - 1)
                                          let ConsecutiveStations = dl.GetConsecutiveStations(busStationKey2, b.BusStationKey)
                                          select ConsecutiveStations.CopyToBusLineStationBO(b);
                //stationBO.busLines = from b in GetAllBusLines()
                //                     where HasBusStation(b, stationKey)
                //                     select b;
                busLine.LastStation = stationKey;
            }
            catch (DO.BadBusStationKeyException ex)
            {
                throw new BO.BadBusStationKeyException("the station not exsist", ex);
            }
            catch (DO.BadBusLineKeyException ex)
            {
                throw new BO.BadBusLineKeyException("the line not exsist", ex);
            }
            catch(DO.BadBusLineStationsException ex)
            {
                throw new BO.BadBusLineStationsException(ex.Message, ex);
            }
        }
        public void deleteBusStationInBusLine(BusLineBO busLine, int stationKey)
        {
            try
            {
                BusLineStation busLinsStation = dl.GetAllBusLineStationBy(b => (b.IsActive & b.BusStationKey == stationKey & b.BusLineKey == busLine.BusLineKey)).First();
                dl.DeleteBusLineStationInOneBusLine(stationKey, busLine.BusLineKey);
                if (busLine.LastStation == stationKey)
                {
                    busLine.LastStation = busLine.busLineStations.FirstOrDefault(b => (b.StationNumberInLine == busLinsStation.StationNumberInLine - 1 & b.IsActive)).BusStationKey;
                }
                else
                {
                    if (busLine.FirstStation == stationKey)
                    {
                        busLine.LastStation = dl.GetBusLineStationKey(busLine.BusLineKey, 1);
                    }
                    int keyPrev = dl.GetBusLineStationKey(busLine.BusLineKey, busLinsStation.StationNumberInLine - 1);
                    int keyNext = dl.GetBusLineStationKey(busLine.BusLineKey, busLinsStation.StationNumberInLine);
                    var newConsecutiveStation = new ConsecutiveStations { Station1Key = keyPrev, Station2Key = keyNext, IsActive = true };
                    if(keyPrev==-1)
                    {
                        newConsecutiveStation.Distance = 0;
                        newConsecutiveStation.DriveDistanceTime = TimeSpan.Zero;
                    }
                    else
                    {
                        newConsecutiveStation.Distance = GetBusStation(keyPrev).Coordinates.GetDistanceTo(GetBusStation(keyNext).Coordinates);
                        newConsecutiveStation.DriveDistanceTime = TimeSpan.FromMinutes(newConsecutiveStation.Distance * 0.01);
                    }
                    try
                    {
                        dl.AddConsecutiveStations(newConsecutiveStation);
                    }
                    catch(DO.BadConsecutiveStationsException)
                    { }
                }
            }
            catch (DO.BadBusLineStationsException ex)
            {
                throw new BO.BadBusStationKeyException("this station in not exsist or unactive", ex);
            }
            catch (DO.BadBusLineKeyException ex)
            {
                throw new BO.BadBusStationKeyException("this line in not exsist or unactive", ex);
            }
        }
        public IEnumerable<BO.BusLineStationBO> GetAllBusLineStationOfBusLine(BusLineBO busLine)
        {
            return from b in busLine.busLineStations.Where(b => b.IsActive)
                   select b;
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
            return BusStationDOBOAdapter(dl.GetBusStation(busStationKey));
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
                dl.DeleteBusStation(busStationKey);
                foreach (var busLine in GetBusStation(busStationKey).busLines)
                {
                    deleteBusStationInBusLine(busLine, busStationKey);
                }
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
            IEnumerable<BO.BusLineBO> busLines = GetAllBusLinesBy(b =>
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

            return busLines.OrderBy(b => (TimeBetweanStations(b, driving.Source.BusStationKey, driving.Destination.BusStationKey)));
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
            return UserDOBOAdapter(dl.GetUser(userName));
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
                throw new BO.BadUserNameException("this username exsist", busExaption);
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
                throw new BO.BadUserNameException("this username exsist", busExaption);
            }
        }

        #endregion
    }
}