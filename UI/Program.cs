using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using BlApi;
using BO;


namespace PlConsole
{
    class Program
    {
        static IBL bl;

        static void Main(string[] args)
        {
            bl = BlFactory.GetBl("1");
            BusLineBO busLineBOs = bl.GetBusLine(20000);
            bl.AddStation(busLineBOs, 30080);
            IEnumerable<BusLineStationBO> b = busLineBOs.busLineStations;
            for(int i=0;i< b.Count();i++)
            {
                Console.WriteLine(b.ElementAt(i));
            }
        }
    }
}