using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class DeepCopyUtilities
    {
        public static void CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
        }
        public static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
            return to;
        }
        public static BO.BusLineStationBO CopyToBusLineStationBO(this DO.ConsecutiveStations consecutiveStations, DO.BusLineStation busLineStation)
        {
            BO.BusLineStationBO result = (BO.BusLineStationBO)busLineStation.CopyPropertiesToNew(typeof(BO.BusLineStationBO));
            result.DistanceFromLastStation = consecutiveStations.Distance;
            result.DriveDistanceTimeFromLastStation = consecutiveStations.DriveDistanceTime;
            return result;
        }
    }
}