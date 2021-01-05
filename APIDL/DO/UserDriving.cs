using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class UserDriving
    {
        public int UserDrivingKey { get; set; }
        public string UserName { get; set; }//im not sure that this is necesery
        public int BusLineKey { get; set; }//line number he is on it?
        public int UpStationKey { get; set; }// the bus station key i think
        public int DownStationKey { get; set; }// the bus station key i think
        public DateTime UpTime { get; set; }
        public DateTime DownTime { get; set; }


    }
}
