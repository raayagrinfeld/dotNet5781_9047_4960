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

            dl.AddBusLine((new BusLine
            {
                BusLineKey = 20000,
                LineNumber = 3895500,
                Area = DO.Areas.North,
                IsActive = true
            }));
        }
    }
}