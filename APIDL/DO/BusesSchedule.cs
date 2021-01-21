using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusesSchedule
    {
        public int BusLineKey { get; set; }
        public TimeSpan StartHour { get; set; }
        public bool IsActive { get; set; }


    }
}
