using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using BO;
using BL;
using BlApi;
namespace PL
{
    public class ConvertStationToStationName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is StationBO)
            {
                StationBO helper = (StationBO)value;
                return helper.StationName;
            }
            else
                return null;   
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string)
            {
                IBL bl= BlFactory.GetBl("1");
                IEnumerable<StationBO> station = bl.GetAllBusStationsBy(b => (b.StationName == value & b.IsActive));
                if(station!=null& (station.Count()==1))
                {
                    return station.First();
                }
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
