using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusLineStation
    {
        public int BusStationKey { get; set; }
        public int BusLineKey { get; set; }
        public int StationNumberInLine { get; set; }
        public bool IsActive { get; set; }
    }
}
