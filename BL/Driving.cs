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
    }
}
/* public class DrivingLine
    {
       // public int BusKey { get; set; }
        //public string LicenseNumber { get; set; }
        public int LineNumber { get; set; }
        // public int LineKey { get; set; } it in // because i dont anderstend why you need it
        public DateTime Start { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }
        //need more things in the next stages
    }*/