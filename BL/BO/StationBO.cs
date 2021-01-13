using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class StationBO
    {
        public int BusStationKey { get; set; }
        public GeoCoordinate Coordinates { get; set; }
        public string StationName { get; set; }
        public string StationAddress { get; set; }
        public bool HasARoof { get; set; } //we need to add more options
        public bool IsActive { get; set; }

        public IEnumerable<BO.BusLineBO> busLines { get; set; }
        public override string ToString()
        {
            return StationName+ "-"+BusStationKey.ToString() ;
        }
    }
}
