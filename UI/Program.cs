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
        }
    }
}