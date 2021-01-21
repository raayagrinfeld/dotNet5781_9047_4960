﻿using System;
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
        int getNextBusLineRunNumber();
        int getNextBusStationRunNumber();
        int getNextBusScheduleRunNumber();
        #endregion

        #region BusLineBO
        BusLineBO GetBusLine(int busLineKey);
        IEnumerable<BusLineBO> GetAllBusLines();
        IEnumerable<BusLineBO> GetAllBusLinesBy(Predicate<BusLineBO> predicate);
        bool HasBusStation(BusLineBO busLine, int stationKey);//done
        void UpdateBusLine(BusLineBO busLine);
        void AddBusLine(BusLineBO bus);
        void DeleteBusLine(int busLineKey);
        IEnumerable<IGrouping<Areas, BusLineBO>> GetBusLineGrouptByArea();

        #endregion

        #region BusLineStationBO
        IEnumerable<BO.BusLineStationBO> GetAllBusLineStationOfBusLine(BusLineBO busLine);//done
        double DistanceBetweanStations(BusLineBO busLine, int firstStationKey, int lastStationKey);
        TimeSpan TimeBetweanStations(BusLineBO busLine, int firstStationKey, int lastStationKey);
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
        IEnumerable<BO.BusLineBO> fingALinesBeatweenStation(Driving driving);
        #endregion

        #region User
        BO.User GetUser(string userName);
        IEnumerable<IGrouping<bool, User>> GetUserGrouptByManagment();
        IEnumerable<BO.User> GetAllUsers();
        IEnumerable<BO.User> GetAlUersBy(Predicate<BO.User> predicate);
        void AddUser(BO.User user);
        void UpdateUser(BO.User user);
        void DeletUser(string userName);
        #endregion

        #region DrivingLine
        DrivingLine GetDrivingLine(int busLineKey, TimeSpan time, StationBO DestinationStation = null);
        IEnumerable<DrivingLine> GetAllDrivings(StationBO DestinationStation=null);
        IEnumerable<DrivingLine> GetAllDrivingsBy(Predicate<DrivingLine> predicate, StationBO DestinationStation = null); 
        void AddDrivingLine(DrivingLine drivingLine);
        void UpdateDrivingLine(DrivingLine drivingLine);
        void DeleteDrivingLine(int busLineKey, TimeSpan time);
        IEnumerable<DrivingLine> BusLineInDrivingToStation(StationBO DestinationStation, TimeSpan time);
        #endregion

    }
}