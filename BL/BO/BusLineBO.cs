using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusLineBO
    {
        public int BusLineKey { get; set; }
        public int LineNumber { get; set; }
        public Areas Area { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<BO.BusLineStationBO> busLineStations;
        public override string ToString()
        {
            return this.ToStringProperty();
        }
    }
}