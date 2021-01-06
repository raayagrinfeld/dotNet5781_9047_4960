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
            List<BusLineBO> busLineBOs = bl.GetAllBusLines().ToList();
            for (int i = 0; i < busLineBOs.Count; i++)
            {
                Console.WriteLine(busLineBOs[i]);
            }
            bl.deleteBusStationInBusLine(busLineBOs[0], busLineBOs[0].FirstStation);
            Console.WriteLine("delete first station:");
            Console.WriteLine(busLineBOs[0]);
        }
    }
}