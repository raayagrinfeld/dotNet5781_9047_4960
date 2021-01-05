using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class ConsecutiveStations
    {
        public int Station1Key { get; set; }
        public int Station2Key { get; set; }
        public double Distance { get; set; }
        public TimeSpan DriveDistanceTime { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}
