﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    #region BusLineExceptions
    public class BadBusLineKeyException : Exception
    {
        public int BUSLINEKEY;
        public BadBusLineKeyException(string message, Exception innerException) :
            base(message, innerException) => BUSLINEKEY = ((DO.BadBusLineKeyException)innerException).BUSLINEKEY;
        public override string ToString() => base.ToString() + $", bad line key: {BUSLINEKEY}";
    }
    #endregion
    #region BusStationExceptions
    public class BadBusStationKeyException : Exception
    {
        public int BUSSATIONKEY;
        public BadBusStationKeyException(string message, Exception innerException) :
            base(message, innerException) => BUSSATIONKEY = ((DO.BadBusStationKeyException)innerException).BUSSATIONKEY;
        public override string ToString() => base.ToString() + $", bad line key: {BUSSATIONKEY}";
    }
    #endregion
    #region UserExseption
    public class BadUserNameException : Exception
    {
        public string USERNAME;
        public BadUserNameException(string massege, Exception innerException) :
            base(massege, innerException) => USERNAME = ((DO.BadUserNameException)innerException).USERNAME;
        public override string ToString() => base.ToString() + $", bad user name: {USERNAME}";
    }
    #endregion
    #region BusLineStationExceptions
    public class BadBusLineStationsException : Exception
    {
        public int KEY1, KEY2;
        public BadBusLineStationsException(int Key1, int Key2, string message) :
            base(message)
        { KEY1 = Key1; KEY2 = Key2; }
        public BadBusLineStationsException(string message, Exception innerException) :
            base(message, innerException)
        { KEY1 = ((DO.BadBusLineStationsException)innerException).KEY1; KEY2 = ((DO.BadBusLineStationsException)innerException).KEY2; }
    }
    #endregion
    #region LineDrivingExceptions
    public class BadDrivingLineException: Exception
    {
        public int BUSLINEKEY;
        public string TIME;
        public BadDrivingLineException(string message, Exception innerException) :
            base(message, innerException)
        { BUSLINEKEY = ((DO.BadBusesScheduleKeyException)innerException).BUSLINEKEY; TIME = ((DO.BadBusesScheduleKeyException)innerException).Time; }
        public override string ToString() => base.ToString() + $", bad driving line time and bus: {TIME}{BUSLINEKEY}";
    }
    #endregion
}

