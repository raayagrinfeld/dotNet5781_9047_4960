using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// is a DO.BusLineStation +DO.ConsecutiveStation
    /// </summary>
    public class BusLineStationBO
    {
        public int BusStationKey { get; set; }
        public string StationName { get; set; }
        public int StationNumberInLine { get; set; }
        public double DistanceFromLastStation { get; set; }
        public TimeSpan DriveDistanceTimeFromLastStation { get; set; }
        public bool IsActive { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}