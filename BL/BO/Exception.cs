using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    #region BusExceptions
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
    //#region ConsecutiveStations
    //public class BadConsecutiveStationsException : Exception
    //{
    //    public int KEY1, KEY2;
    //    public BadConsecutiveStationsException(string message, Exception innerException) :
    //        base(message, innerException)
    //    {
    //        KEY1 = ((DO.BadConsecutiveStationsException)innerException).KEY1;
    //        KEY2 = ((DO.BadConsecutiveStationsException)innerException).KEY2;
    //    }
    //    public override string ToString() => base.ToString() + $", bad Consecutive Stations station key: {KEY1} and {KEY2}";
    //}
    //#endregion
    #region BusLineStationExceptions
    public class BadBusLineStationException : Exception
    {
        public int BUSLINEKEY;
        public BadBusLineStationException(string message, Exception innerException) :
            base(message, innerException) => BUSLINEKEY = ((DO.BadBusLineKeyException)innerException).BUSLINEKEY;
        public override string ToString() => base.ToString() + $", bad station key: {BUSLINEKEY}";
    }
    #endregion
}

