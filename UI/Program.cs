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
            bl = BlFactory.GetBL();
            Driving driving = new Driving();
               bl.AddStation(bl.GetBusLine(20000), 38888);
                bl.AddStation(bl.GetBusLine(20000), 38919);
                bl.AddStation(bl.GetBusLine(20004), 38888);
                bl.AddStation(bl.GetBusLine(20004), 38919);
            Console.WriteLine(bl.GetBusLine(20000));
            Console.WriteLine(bl.GetBusLine(20004));
            bl.AddDeatinationStation(38919, driving);
            bl.AddSourceStation(38888, driving);
            bl.fingALinesBeatweenStation(driving);
            //Console.WriteLine(driving);
            foreach(BusLineBO busLine in driving.BusLines)
            {
                Console.WriteLine("bus Line Key:"+busLine.BusLineKey +" time:" +bl.TimeBetweanStations(busLine,30003,30005)); 
            }
            //bl.deleteBusStationInBusLine(busLineBOs[0], busLineBOs[0].FirstStation);
            //Console.WriteLine("delete first station:");
            //Console.WriteLine(busLineBOs[0]);
            
        }

    }
}