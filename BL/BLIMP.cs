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
using System.Security.Cryptography;
using System.Text;


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
        #endregion

        #region BusLineBO
        BusLineBO BusLineDOBOAdapter(DO.BusLine busLineDo) //adaptes DO busline to BO busline
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

            //defines bus line station ienumrable with linq 
            busLineBO.busLineStations = from b in dl.GetAllBusLineStationBy(b => (b.BusLineKey == BusLineKeyOfDO & b.IsActive))
                                        let busStationKey2 = dl.GetBusLineStationKey(BusLineKeyOfDO, b.StationNumberInLine - 1)
                                        let ConsecutiveStations = dl.GetConsecutiveStations(busStationKey2, b.BusStationKey)
                                        select ConsecutiveStations.CopyToBusLineStationBO(b);

            return busLineBO;
        }
        DO.BusLine BusLineBODOAdapter(BO.BusLineBO busLineBO) //adaptes BO busline to DO busline
        {
            DO.BusLine busLineDO = new BusLine();
            busLineBO.CopyPropertiesTo(busLineDO);
            return busLineDO;
        }
        public bool HasBusStation(BusLineBO busLine, int stationKey)    //checks if bus has station
        {
            if (busLine.busLineStations.FirstOrDefault(b => (b.BusStationKey == stationKey & b.IsActive)) != null)
            {
                return true;
            }
            return false;
        }
        public BusLineBO GetBusLine(int busLineKey) //get bus line
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
        public IEnumerable<BusLineBO> GetAllBusLines()  //gets all bus lines
        {
            return from b in dl.GetAllBusLinesBy(b => b.IsActive)
                   select BusLineDOBOAdapter(b);
        }
        public IEnumerable<BusLineBO> GetAllBusLinesBy(Predicate<BusLineBO> predicate)  //gets all bus line by predicate
        {
            return from b in GetAllBusLines()
                   where predicate(b)
                   select b;
        }
        public void UpdateBusLine(BusLineBO busLine)    //updates bus line
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
        public void AddBusLine(BusLineBO bus)   //adds bus line
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
        public void DeleteBusLine(int busLineKey)   //deletes bus line
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
        public IEnumerable<IGrouping<BO.Areas, BusLineBO>> GetBusLineGrouptByArea() //returns bus line groupt by area
        {
            return from BusLine in GetAllBusLines()
                   group BusLine by BusLine.Area into g
                   select g;
        }
        #endregion

        #region BusLineStationBO
        BO.BusLineStationBO BusLineStationDOBOAdapter(DO.BusLineStation busLineStationDo)   //adaptes DO buslinestation to BO buslinestation
        {
            BO.BusLineStationBO busLineStationBO = new BO.BusLineStationBO();
            var busStationKey2 = dl.GetBusLineStationKey(busLineStationDo.BusLineKey, busLineStationDo.StationNumberInLine - 1);
            busLineStationBO = dl.GetConsecutiveStations(busStationKey2, busLineStationDo.BusStationKey).CopyToBusLineStationBO(busLineStationDo);
            return busLineStationBO;
        }
        DO.BusLineStation BusLineStationBODOAdapter(BO.BusLineStationBO busLineStationBo) //adaptes BO buslinestation to DO buslinestation
        {
            DO.BusLineStation busLineStationDO = new DO.BusLineStation();
            busLineStationBo.CopyPropertiesTo(busLineStationDO);
            return busLineStationDO;
        }
        public double DistanceBetweanStations(BusLineBO busLine, int firstStationKey, int lastStationKey)   //Gets distence betwean two station in a busline
        {
            BusLineStationBO firstStation = BusLineStationDOBOAdapter(dl.GetBusLineStation(busLine.BusLineKey, firstStationKey));
            BusLineStationBO LastStation = BusLineStationDOBOAdapter(dl.GetBusLineStation(busLine.BusLineKey, lastStationKey));
            List<BusLineStationBO> PartOfBusLineStation = busLine.busLineStations.OrderBy(b => b.StationNumberInLine).ToList().FindAll(b => b.StationNumberInLine >= firstStation.StationNumberInLine & b.StationNumberInLine <= LastStation.StationNumberInLine & b.IsActive);
            double ColectiveDistance = 0;
            foreach (BusLineStationBO BLStationBO in PartOfBusLineStation)
            {
                ColectiveDistance += BLStationBO.DistanceFromLastStation;
            }
            return ColectiveDistance;
        }
        public TimeSpan TimeBetweanStations(BusLineBO busLine, int firstStationKey, int lastStationKey) //Gets time betwean two station in a busline
        {
            try
            {
                BusLineStationBO firstStation = BusLineStationDOBOAdapter(dl.GetBusLineStation(busLine.BusLineKey, firstStationKey));
                BusLineStationBO LastStation = BusLineStationDOBOAdapter(dl.GetBusLineStation(busLine.BusLineKey, lastStationKey));
                List<BusLineStationBO> PartOfBusLineStation = busLine.busLineStations.OrderBy(b => b.StationNumberInLine).ToList().FindAll(b => b.StationNumberInLine >= firstStation.StationNumberInLine & b.StationNumberInLine <= LastStation.StationNumberInLine & b.IsActive);
                TimeSpan timeOfDravel = TimeSpan.Zero;
                foreach (BusLineStationBO BLStationBO in PartOfBusLineStation)
                {
                    timeOfDravel += BLStationBO.DriveDistanceTimeFromLastStation;
                }
                return timeOfDravel;
            }
            catch (DO.BadBusLineKeyException busExaption)
            {
                throw new BO.BadBusLineKeyException("this bus not exsist", busExaption);
            }
        }
        public void AddStation(BusLineBO busLine, int stationKey)   //add station to busline
        {
            try
            {
                var thisBusStation = dl.GetBusStation(stationKey);
                if (busLine.busLineStations == null || busLine.busLineStations.Count() == 0)    //if its the first station define first satation 
                {
                    busLine.FirstStation = stationKey;
                    busLine.FirstStationName = thisBusStation.StationName;
                    if (busLine.busLineStations == null)
                        busLine.busLineStations = new List<BusLineStationBO>();
                }
                var BLstation = new BusLineStation { BusLineKey = busLine.BusLineKey, BusStationKey = stationKey, StationNumberInLine = busLine.busLineStations.Count() + 1, IsActive = true, StationName = thisBusStation.StationName };
                ConsecutiveStations ConsecutiveStation = new ConsecutiveStations { Station1Key = dl.GetBusLineStationKey(busLine.BusLineKey, busLine.busLineStations.Count()), Station2Key = stationKey, IsActive = true };
                dl.AddBusLineStation(BLstation);
                if (ConsecutiveStation.Station1Key == -1)   //if its the first station the distence is 0
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
                catch (DO.BadConsecutiveStationsException)  //its okay if the is a consecutive station allrady
                { }
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
        public void deleteBusStationInBusLine(BusLineBO busLine, int stationKey)    //deletes a station in bus
        {
            BusLineStationBO station_to_delete;
            StationBO PrevStation;
            StationBO NextStation;
            if (busLine.busLineStations.Count() < 3)    //minimom 2 station in bus
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
            {   //adds new consecutive station for the station befor and after the station to delete
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
                if (busLine.LastStation == stationKey)  //if we delete the last station we need to update the last station information 
                {
                    PrevStation = GetBusStation(dl.GetBusLineStationKey(busLine.BusLineKey, station_to_delete.StationNumberInLine - 1));
                    busLine.LastStation = PrevStation.BusStationKey;
                    busLine.LastStationName = PrevStation.StationName;
                }
                else
                {
                    if (busLine.FirstStation == stationKey) //if we delete the first station we need to update the first station information 
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

        public IEnumerable<BO.BusLineStationBO> GetAllBusLineStationOfBusLine(BusLineBO busLine)    //get all station in bus line
        {
            return busLine.busLineStations;
        }
        #endregion

        #region StationBO
        public BO.StationBO BusStationDOBOAdapter(DO.BusStation busStationDo)   //adaptes DO station to BO staiton
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
        DO.BusStation BusStationBODOAdapter(BO.StationBO busStationBO)  //adaptes BO station to DO staiton
        {
            DO.BusStation busStationDO = new BusStation();
            busStationBO.CopyPropertiesTo(busStationDO);
            busStationDO.Coordinates = busStationBO.Coordinates;
            return busStationDO;
        }
        public StationBO GetBusStation(int busStationKey)   //get station
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
        public IEnumerable<StationBO> GetAllBusStations()   //get all stations
        {
            return from b in dl.GetAllBusStationsBy(b => b.IsActive)
                   select BusStationDOBOAdapter(b);
        }

        public IEnumerable<StationBO> GetAllBusStationsBy(Predicate<StationBO> predicate)   //get all station by predicate
        {
            return from b in GetAllBusStations()
                   where predicate(b)
                   select b;
        }
        public void AddBusStation(StationBO station)    //add station
        {
            try
            {
                dl.AddBusStation(BusStationBODOAdapter(station));
            }
            catch (DO.BadBusStationKeyException busExaption)
            {
                throw new BO.BadBusStationKeyException("dupliceited bus line station", busExaption);
            }
        }
        public void DeleteBusStation(int busStationKey) //deletes station
        {
            try
            {
                DeleteBusStationInAllLines(busStationKey);  //deletes all mention of station alsow in all buss
                dl.DeleteBusStation(busStationKey);
            }
            catch (DO.BadBusStationKeyException busExaption)
            {
                throw new BO.BadBusStationKeyException("this bus does not exsist", busExaption);
            }
        }
        public void DeleteBusStationInAllLines(int busStationKey)   //deletes a station from all buss
        {
            StationBO busStation = GetBusStation(busStationKey);
            foreach (var item in busStation.busLines.ToList())
            {
                deleteBusStationInBusLine(item, busStationKey);
            }
        }

        public void UpdateBusStation(StationBO station)     //updates station and all consecutive station that have him
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
                foreach (var item in ConsecutiveStationToUpdate) //updates consecutive stations that contine stationkey
                {
                    try
                    {
                        double distance = GetBusStation(item.Station1Key).Coordinates.GetDistanceTo(GetBusStation(item.Station2Key).Coordinates);
                        ConsecutiveStations NewConsecutiveStation = new ConsecutiveStations { Station1Key = item.Station1Key, Station2Key = item.Station2Key, IsActive = true, Distance = distance };
                        NewConsecutiveStation.DriveDistanceTime = TimeSpan.FromMinutes(NewConsecutiveStation.Distance * 0.01);
                        dl.UpdateConsecutiveStations(NewConsecutiveStation);
                    }
                    catch(DO.BadBusStationKeyException ex)
                    { }
                }
            }
            catch (DO.BadBusLineKeyException busExaption)
            {
                throw new BO.BadBusLineKeyException("this bus does not exsist", busExaption);
            }
        }
        public bool HasLine(StationBO station, int lineNumber)  //check if a bus pass troug station
        {
            if (station.busLines.FirstOrDefault(s => (s.BusLineKey == lineNumber & s.IsActive)) != null)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Driving
        public void AddDeatinationStation(int stationKey, Driving driving)  //add destination to search driving
        {
            driving.Destination = GetBusStation(stationKey);
        }
        public void AddSourceStation(int stationKey, Driving driving)   //add source to search driving
        {
            driving.Source = GetBusStation(stationKey);
        }
        public IEnumerable<BO.BusLineBO> fingAllLinesBeatweenStation(Driving driving)   //findes all bus lines that pass between two stations
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
        BO.User UserDOBOAdapter(DO.User user) //adaps DO user to BO user
        {
            BO.User userBO = new BO.User();
            user.CopyPropertiesTo(userBO);
            return userBO;
        }


        DO.User UserBODOAdapter(BO.User user) //adaps BO user to DO user
        {
            DO.User userDO = new DO.User();
            user.CopyPropertiesTo(userDO);
            return userDO;
        }
        public BO.User GetUser(string userName) //get user
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
        public IEnumerable<IGrouping<bool, BO.User>> GetUserGrouptByManagment() //returns user groupt by managment permmision
        {
            return from user in GetAllUsers()
                   group user by user.ManagementPermission into g
                   select g;
        }
        public IEnumerable<BO.User> GetAllUsers()   //gety all users
        {
            return from u in dl.GetAlUersBy(u => u.IsActive)
                   select UserDOBOAdapter(u);
        }
        public IEnumerable<BO.User> GetAlUersBy(Predicate<BO.User> predicate) //get all users by predicat
        {
            return from u in GetAllUsers()
                   where predicate(u)
                   select u;
        }
        public void AddUser(BO.User user)   //add user
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
        public void DeletUser(string userName)  //delete user
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
        public void UpdateUser(BO.User user)    //updates user
        {
            try
            {
                dl.UpdateUser(UserBODOAdapter(user));
            }
            catch (DO.BadUserNameException busExaption)
            {
                throw new BO.BadUserNameException("this username doesnt exsist", busExaption);
            }
        }
       public  string Decode(string HashPassword, int Salt) //decode password
        {
            return hashPassword(HashPassword + Salt);
        }
        private static string hashPassword(string passwordWithSalt)     //encript password
        {
            SHA512 shaM = new SHA512Managed();
            return Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt)));
        }
        #endregion

        #region DrivingLine

        DrivingLine DrivingLineBODOAdapter(DO.BusesSchedule busesSchedule, StationBO DestinationStation = null) //from bo Schedule to driving line (arival time is base on if we get destination)
        {
            DrivingLine drivingLine = new DrivingLine();
            busesSchedule.CopyPropertiesTo(drivingLine);
            if (DestinationStation == null)
                drivingLine.ArrivalTime = busesSchedule.StartHour + TimeBetweanStations( GetBusLine(busesSchedule.BusLineKey) , dl.GetBusLine(busesSchedule.BusLineKey).FirstStation, dl.GetBusLine(busesSchedule.BusLineKey).LastStation);
            else
                drivingLine.ArrivalTime = busesSchedule.StartHour + TimeBetweanStations(GetBusLine(busesSchedule.BusLineKey), dl.GetBusLine(busesSchedule.BusLineKey).FirstStation, DestinationStation.BusStationKey);
            return drivingLine;
        }
        DO.BusesSchedule DrivingLineDOBOAdapter(DrivingLine drivingLine)    //from driving line to Schedule
        {
            BusesSchedule schedule = new BusesSchedule();
            drivingLine.CopyPropertiesTo(schedule);
            return schedule;
        }

        public void AddDrivingLine(DrivingLine drivingLine) //add driving line
        {
            try
            {
                dl.AddBusSchedule(DrivingLineDOBOAdapter(drivingLine));
            }
            catch (DO.BadBusesScheduleKeyException ex)
            {
                throw new BO.BadDrivingLineException("duplicate driving line", ex);
            }
        }

        public void UpdateDrivingLine(DrivingLine drivingLineold, DrivingLine drivingLinenew)   //updates driving line
        {
            try
            {
                dl.UpdateBusSchedule(DrivingLineDOBOAdapter(drivingLineold), (DrivingLineDOBOAdapter(drivingLinenew)));
            }
            catch (DO.BadBusesScheduleKeyException ex)
            {
                throw new BO.BadDrivingLineException("duplicate driving line", ex);
            }
        }

        public void DeleteDrivingLine(int busLineKey, TimeSpan time)    //delete driving line
        {
            try
            {
                dl.DeleteBusSchedule(busLineKey,time);
            }
            catch (DO.BadBusesScheduleKeyException ex)
            {
                throw new BO.BadDrivingLineException("duplicate driving line", ex);
            }
        }

        //gets all bus lines acording to destination and the current time
        public IEnumerable<DrivingLine> BusLineInDrivingToStation(StationBO DestinationStation, TimeSpan time)  
        {
            return GetAllDrivingsBy(b => b.IsActive && b.StartHour < time && b.ArrivalTime > time, DestinationStation);
        }

        //get driving line
        public DrivingLine GetDrivingLine(int busLineKey, TimeSpan time, StationBO DestinationStation)
        {
            try
            {
                return DrivingLineBODOAdapter(dl.GetBusesSchedule(busLineKey, time),DestinationStation);
            }
            catch(DO.BadBusesScheduleKeyException ex)
            {
                throw new BO.BadDrivingLineException("driving line doesnt exist", ex);
            }

        }

        //get all driving lines
        public IEnumerable<DrivingLine> GetAllDrivings(StationBO DestinationStation)
        {
            if (DestinationStation == null)
            {
                return from b in dl.GetAllBusSchedules()
                       select DrivingLineBODOAdapter(b, null);
            }
            return from b in DestinationStation.busLines
                   let busSchegualWeWant = dl.GetAllBusSchedulesBy(w => w.BusLineKey == b.BusLineKey)
                   from w in busSchegualWeWant
                   select DrivingLineBODOAdapter(w, DestinationStation);
        }

        //get all driving lines bt predicat
        public IEnumerable<DrivingLine> GetAllDrivingsBy(Predicate<DrivingLine> predicate, StationBO DestinationStation)
        {
            return from b in GetAllDrivings(DestinationStation)
                   where predicate(b)
                   select b;
        }
        #endregion
    }
}