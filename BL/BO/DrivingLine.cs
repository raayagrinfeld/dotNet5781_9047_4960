﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.BO
{
   public  class DrivingLine
    {
        public int BusLineKey { get; set; }
        public TimeSpan StartHour { get; set; }
        public string LastStationName { get; set; }
        public TimeSpan ArrivalTime { get; set; }
    }
}