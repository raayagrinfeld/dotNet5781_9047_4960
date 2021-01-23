using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IBL
    {
        #region runNumber
        int getNextBusLineRunNumber();// get Next Bus Line Run Number
        int getNextBusStationRunNumber();//get Next Bus Station Run Number
        #endregion

        #region BusLineBO
        BusLineBO GetBusLine(int busLineKey);//Get Bus Line by key
        IEnumerable<BusLineBO> GetAllBusLines();//Get All Bus Lines
        IEnumerable<BusLineBO> GetAllBusLinesBy(Predicate<BusLineBO> predicate);//Get All Bus Lines by predicate
        bool HasBusStation(BusLineBO busLine, int stationKey);//chack if the station exsist
        void UpdateBusLine(BusLineBO busLine);//Update Bus Line
        void AddBusLine(BusLineBO bus);//Add Bus Line
        void DeleteBusLine(int busLineKey);//Delete Bus Line
        IEnumerable<IGrouping<Areas, BusLineBO>> GetBusLineGrouptByArea();//Get Bus Lines Groupt By Area

        #endregion

        #region BusLineStationBO
        IEnumerable<BO.BusLineStationBO> GetAllBusLineStationOfBusLine(BusLineBO busLine);//Get All Bus Line Stations Of Bus Line
        double DistanceBetweanStations(BusLineBO busLine, int firstStationKey, int lastStationKey);//get Distance Betwean Stations
        TimeSpan TimeBetweanStations(BusLineBO busLine, int firstStationKey, int lastStationKey);//get Time Betwean Stations
        void AddStation(BusLineBO busLine, int stationKey);
        void deleteBusStationInBusLine(BusLineBO busLine, int stationKey);
        #endregion

        #region StationBO
        StationBO GetBusStation(int busStationKey);
        IEnumerable<StationBO> GetAllBusStations();
        IEnumerable<StationBO> GetAllBusStationsBy(Predicate<StationBO> predicate);
        void AddBusStation(StationBO station);
        void UpdateBusStation(StationBO station);
        void DeleteBusStation(int busStationKey);
        bool HasLine(StationBO station, int lineNumber);
        #endregion

        #region Driving
        void AddDeatinationStation(int stationKey, Driving driving);
        void AddSourceStation(int stationKey, Driving driving);
        IEnumerable<BO.BusLineBO> fingAllLinesBeatweenStation(Driving driving);
        #endregion

        #region User
        BO.User GetUser(string userName);
        IEnumerable<IGrouping<bool, User>> GetUserGrouptByManagment();
        IEnumerable<BO.User> GetAllUsers();
        IEnumerable<BO.User> GetAlUersBy(Predicate<BO.User> predicate);
        void AddUser(BO.User user);
        void UpdateUser(BO.User user);
        void DeletUser(string userName);
        string Decode(string HashPassword, int Salt);
        #endregion

        #region DrivingLine
        DrivingLine GetDrivingLine(int busLineKey, TimeSpan time, StationBO DestinationStation = null);
        IEnumerable<DrivingLine> GetAllDrivings(StationBO DestinationStation=null);
        IEnumerable<DrivingLine> GetAllDrivingsBy(Predicate<DrivingLine> predicate, StationBO DestinationStation = null); 
        void AddDrivingLine(DrivingLine drivingLine);
        void UpdateDrivingLine(DrivingLine drivingLineold, DrivingLine drivingLinenew);
        void DeleteDrivingLine(int busLineKey, TimeSpan time);
        IEnumerable<DrivingLine> BusLineInDrivingToStation(StationBO DestinationStation, TimeSpan time);
        #endregion

    }
}