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
        void AddStation(BusLineBO busLine, int stationKey);//Add Station to bus line
        void deleteBusStationInBusLine(BusLineBO busLine, int stationKey);//delete Bus Station In Bus Line
        #endregion

        #region StationBO
        StationBO GetBusStation(int busStationKey);//Get Bus Station by key
        IEnumerable<StationBO> GetAllBusStations();//Get All Bus Stations
        IEnumerable<StationBO> GetAllBusStationsBy(Predicate<StationBO> predicate);// Get All Bus Stations By predicate
        void AddBusStation(StationBO station);//Add Bus Station
        void UpdateBusStation(StationBO station);//Update Bus Station
        void DeleteBusStation(int busStationKey);//Delete Bus Station
        bool HasLine(StationBO station, int lineNumber);//check if the line exsist
        #endregion

        #region Driving
        void AddDeatinationStation(int stationKey, Driving driving);//Add Deatination Station
        void AddSourceStation(int stationKey, Driving driving);//Add Source Station
        IEnumerable<BO.BusLineBO> fingAllLinesBeatweenStation(Driving driving);// find All Lines Beatween Station
        #endregion

        #region User
        BO.User GetUser(string userName);//Get User by user name
        IEnumerable<IGrouping<bool, User>> GetUserGrouptByManagment();// Get User Groupt By Managment
        IEnumerable<BO.User> GetAllUsers();//Get All Users
        IEnumerable<BO.User> GetAlUersBy(Predicate<BO.User> predicate);//Get All Users by predicate
        void AddUser(BO.User user);//Add User
        void UpdateUser(BO.User user);//Update User
        void DeletUser(string userName);//DeletUser
        string Decode(string HashPassword, int Salt);//Decode password
        #endregion

        #region DrivingLine
        DrivingLine GetDrivingLine(int busLineKey, TimeSpan time, StationBO DestinationStation = null);//Get Driving Line by bus and time
        IEnumerable<DrivingLine> GetAllDrivings(StationBO DestinationStation=null);//Get All Drivings
        IEnumerable<DrivingLine> GetAllDrivingsBy(Predicate<DrivingLine> predicate, StationBO DestinationStation = null); // Get All Drivings By predicate
        void AddDrivingLine(DrivingLine drivingLine);//Add Driving Line
        void UpdateDrivingLine(DrivingLine drivingLineold, DrivingLine drivingLinenew);// Update Driving Line
        void DeleteDrivingLine(int busLineKey, TimeSpan time);//Delete Driving Line
        IEnumerable<DrivingLine> BusLineInDrivingToStation(StationBO DestinationStation, TimeSpan time);//Bus Line In Driving To Station
        #endregion

    }
}