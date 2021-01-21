using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusesSchedule
    {
        public int BusKey { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndtHour { get; set; }
        public TimeSpan Frequency { get; set; }
        public bool IsActive { get; set; }


    }
}
