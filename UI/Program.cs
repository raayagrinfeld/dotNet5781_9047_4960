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
            BO.Driving d = new Driving {Source= bl.GetBusStation(30001), Destination= bl.GetBusStation(30005)};
            /* bl.AddSourceStation(30007, d);
             bl.AddDeatinationStation(30042, d);*/

            try
            {
                bl.AddStation(bl.GetBusLine(20000), 30001);
                bl.AddStation(bl.GetBusLine(20000), 30002); 
                bl.AddStation(bl.GetBusLine(20000), 30003);
                bl.AddStation(bl.GetBusLine(20000), 30004);
                bl.AddStation(bl.GetBusLine(20000), 30005);
                bl.AddStation(bl.GetBusLine(20000), 30006);
                bl.AddStation(bl.GetBusLine(20000), 30007);
                bl.AddStation(bl.GetBusLine(20000), 30008);
                Console.WriteLine(bl.GetBusLine(20000));
                bl.deleteBusStationInBusLine(bl.GetBusLine(20000), 30005);
                Console.WriteLine(bl.GetBusLine(20000));
                bl.deleteBusStationInBusLine(bl.GetBusLine(20000), 30001);
                Console.WriteLine(bl.GetBusLine(20000));
                bl.deleteBusStationInBusLine(bl.GetBusLine(20000), 30008);
                Console.WriteLine(bl.GetBusLine(20000));

            }
            catch (BO.BadBusStationKeyException ex)
            {
                Console.WriteLine(ex.Message);
            }

            /*IEnumerable<BusLineBO> bh = bl.GetAllBusLines();
            for (int i = 0; i < bh.Count(); i++)
            {
                bl.AddStation(busLineBOs, 30080);
            }
            catch(BO.BadBusStationKeyException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                bl.AddStation(busLineBOs, 30003);
            }
            catch (BO.BadBusLineStationsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                bl.AddStation(busLineBOs, 30004);
            }
            catch (BO.BadBusLineStationsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                bl.AddStation(busLineBOs, 30005);
            }
            catch (BO.BadBusLineStationsException ex)
            {
                Console.WriteLine(b.ElementAt(i));
            }
            Driving driving = new Driving();
            bl.AddDeatinationStation(30002, driving);
            bl.AddSourceStation(30001, driving);
            bl.fingALinesBeatweenStation(driving);
            Console.WriteLine(driving);
            foreach(BusLineBO busLine in driving.BusLines)
            {
                Console.WriteLine("bus Line Key:"+busLine.BusLineKey +" time:" +bl.TimeBetweanStations(busLine,30003,30005)); 
            }
            //bl.deleteBusStationInBusLine(busLineBOs[0], busLineBOs[0].FirstStation);
            //Console.WriteLine("delete first station:");
            //Console.WriteLine(busLineBOs[0]);
            */
        }

    }
}