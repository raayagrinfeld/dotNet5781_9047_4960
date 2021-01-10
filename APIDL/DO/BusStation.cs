using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DO
{
    public class BusStation
    {
        public int BusStationKey { get; set; }
        public GeoCoordinate Coordinates { get; set; }
        public string StationName { get; set; }
        public string StationAddress { get; set; }
        public bool HasARoof { get; set; } //we need to add more options
        public bool IsActive { get; set; }
        public override string ToString()
        {
            return BusStationKey + StationName;
        }
    }
}
