using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace BO
{
    public class Driving
    {
        
        public StationBO Source { get; set; }
        public StationBO Destination { get; set; }
        public IEnumerable<BO.BusLineBO> BusLines { get; set; }
        public override string ToString()
        {
            return Source.ToString() + Destination.ToString();
        }
    }
}
