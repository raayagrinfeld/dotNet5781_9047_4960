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
            BO.Driving d = new Driving();
            bl.AddSourceStation(30000, d);
            bl.AddDeatinationStation(30001, d);
            List<BusLineBO> b = bl.fingALinesBeatweenStation(d).ToList();
            for(int i=0;i<b.Count();i++)
            {
                Console.WriteLine(b.ElementAt(i));
            }
        }

    }
}