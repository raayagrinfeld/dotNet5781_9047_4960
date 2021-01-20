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
            

            for (int i = 0; i < 12; i++)
            {
                    dl.AddBusSchedule(new BusesSchedule
                    {
                        BusKey = 20000 + i,
                        ScheduleKey = DO.RunNumbers.BusesSCheduleRunNumber++,
                        StartHour =  new TimeSpan(19, 23, 0),
                        EndtHour =  new TimeSpan(19+r.Next(1,4 ), r.Next(0, 36), 0),
                        Frequency = new TimeSpan(0, r.Next(0, 90), 0),
                        IsActive = true
                    });
                   
            }


        }
    }
}