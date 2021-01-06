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
            BusLineBO busLineBO = new BusLineBO { BusLineKey = 20202, IsActive=true};
            busLineBO.busLineStations = new List<BusLineStationBO>();
            bl.AddBusLine(busLineBO);
            bl.AddStation(busLineBO, 30001);
            bl.AddStation(busLineBO, 30002);
            Console.WriteLine(busLineBO);
            BusLineBO busLineBOs = bl.GetBusLine(20000);
            try
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
                bl.AddStation(busLineBOs, 30004);
                bl.AddStation(busLineBOs, 30005);
            }
            catch (BO.BadBusLineStationsException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(busLineBOs);
            bl.deleteBusStationInBusLine(busLineBOs, 30003);
            Console.WriteLine(busLineBOs);
            bl.deleteBusStationInBusLine(busLineBOs, 30005);
            Console.WriteLine(busLineBOs);
            bl.deleteBusStationInBusLine(busLineBOs, busLineBOs.FirstStation);
            Console.WriteLine(busLineBOs);
            //bl.deleteBusStationInBusLine(busLineBOs[0], busLineBOs[0].FirstStation);
            //Console.WriteLine("delete first station:");
            //Console.WriteLine(busLineBOs[0]);
        }
    }
}