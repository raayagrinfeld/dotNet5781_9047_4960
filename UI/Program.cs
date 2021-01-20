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

            dl.GetConsecutiveStations(-1, 38895);
            dl.GetConsecutiveStations(38895, 39013);


        }
    }
}