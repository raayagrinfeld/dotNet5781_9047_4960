using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BusesSchedule
    {
        public int ScheduleKey { get; set; }
        public int BusKey { get; set; }
        public TimeSpan StartHour { get; set; }
        public TimeSpan EndtHour { get; set; }
        public int Frequency { get; set; }//who many times the line out in this time


    }
}
