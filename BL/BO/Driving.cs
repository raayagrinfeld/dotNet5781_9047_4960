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
/* public class DrivingLine
    {
       // public int BusKey { get; set; }
        //public string LicenseNumber { get; set; }
        public int LineNumber { get; set; }
        // public int LineKey { get; set; } it in // because i dont anderstend why you need it
        public DateTime Start { get; set; }

        
        //need more things in the next stages
    }*/