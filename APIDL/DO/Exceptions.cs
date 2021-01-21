using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    #region BusExceptions
    public class BadBusLineKeyException : Exception
    {
        public int BUSLINEKEY;
        public BadBusLineKeyException(int busLineKey) : base() => BUSLINEKEY = busLineKey;
        public BadBusLineKeyException(int busLineKey, string message) :
            base(message) => BUSLINEKEY = busLineKey;
        public BadBusLineKeyException(int busLineKey, string message, Exception innerException) :
            base(message, innerException) => BUSLINEKEY = busLineKey;
        public override string ToString() => base.ToString() + $", bad line key: {BUSLINEKEY}";
    }
    #endregion
    #region BusStationExceptions
    public class BadBusStationKeyException : Exception
    {
        public int BUSSATIONKEY;
        public BadBusStationKeyException(int busStationKey) : base() => BUSSATIONKEY = busStationKey;
        public BadBusStationKeyException(int busStationKey, string message) :
            base(message) => BUSSATIONKEY = busStationKey;
        public BadBusStationKeyException(int busStationKey, string message, Exception innerException) :
            base(message, innerException) => BUSSATIONKEY = busStationKey;
        public override string ToString() => base.ToString() + $", bad line key: {BUSSATIONKEY}";
    }
    #endregion
    #region UserExseption
    public class BadUserNameException : Exception
    {
        public string USERNAME;
        public BadUserNameException(string userName) : base() => USERNAME = userName;
        public BadUserNameException(string userName, string massege) :
            base(massege) => USERNAME = userName;
        public BadUserNameException(string userName, string massege, Exception innerException) :
            base(massege, innerException) => USERNAME = userName;
        public override string ToString() => base.ToString() + $", bad user name: {USERNAME}";
    }
    #endregion
    #region ConsecutiveStations
    public class BadConsecutiveStationsException : Exception
    {
        public int KEY1, KEY2;
        public BadConsecutiveStationsException(int Key1, int Key2) : base() { KEY1 = Key1; KEY2 = Key2; }
        public BadConsecutiveStationsException(int Key1, int Key2, string message) :
            base(message)
        { KEY1 = Key1; KEY2 = Key2; }
        public BadConsecutiveStationsException(int Key1, int Key2, string message, Exception innerException) :
            base(message, innerException)
        { KEY1 = Key1; KEY2 = Key2; }
        public override string ToString() => base.ToString() + $", bad Consecutive Stations station key: {KEY1} and {KEY2}";
    }
    #endregion 
    #region BusLineStation
    public class BadBusLineStationsException : Exception
    {
        public int KEY1, KEY2;
        public BadBusLineStationsException(int Key1, int Key2) : base() { KEY1 = Key1; KEY2 = Key2; }
        public BadBusLineStationsException(int Key1, int Key2, string message) :
            base(message)
        { KEY1 = Key1; KEY2 = Key2; }
        public BadBusLineStationsException(int Key1, int Key2, string message, Exception innerException) :
            base(message, innerException)
        { KEY1 = Key1; KEY2 = Key2; }
        public override string ToString() => base.ToString() + $", bad Bus Line Stations station  station key: {KEY1} and Bus Line Key {KEY2}";
    }
    #endregion
    #region Xml
    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }
    #endregion
    #region BusesSchedule
    public class BadBusesScheduleKeyException : Exception
    {
        public int BUSLINEKEY;
        public string Time;
        public BadBusesScheduleKeyException(int BusLineKey, string time) : base()  {BUSLINEKEY=BusLineKey ; Time = time; }
        public BadBusesScheduleKeyException(int BusLineKey, string time, string message) :
            base(message)
        { BUSLINEKEY = BusLineKey; Time = time; }
        public BadBusesScheduleKeyException(int BusLineKey, string time, string message, Exception innerException) :
            base(message, innerException)
        { BUSLINEKEY = BusLineKey; Time = time; }
        public override string ToString() => base.ToString() + $", bad schedule : {BUSLINEKEY}{Time}";
    }
    #endregion
}

