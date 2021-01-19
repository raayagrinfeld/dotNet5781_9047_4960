using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using BlApi;
using BO;
using DO;
using APIDL;
using System.Device.Location;


namespace PlConsole
{
    class Program
    {
        static IBL bl;
        static IDAL dl = DalFactory.GetDal();
        static void Main(string[] args)
        {
            Random r = new Random();

            //dl.AddBusSchedule(new DO.BusesSchedule { BusKey = 20001, IsActive = true, EndtHour = new TimeSpan(13, 0, 0, 0), Frequency = new TimeSpan(1, 0, 0, 0), StartHour = new TimeSpan(8, 0, 0, 0), ScheduleKey = 7 });


        }
    }
}