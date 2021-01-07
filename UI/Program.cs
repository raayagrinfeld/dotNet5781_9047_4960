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
            BO.Driving d = new Driving {Source= bl.GetBusStation(30007), Destination= bl.GetBusStation(30042)};
           /* bl.AddSourceStation(30007, d);
            bl.AddDeatinationStation(30042, d);*/
           /* IEnumerable<BusLineBO> bh = bl.GetAllBusLines();
            for (int i = 0; i < bh.Count(); i++)
            {
                Console.WriteLine(bh.ElementAt(i));
            }*/
            List<BusLineBO> b = bl.fingALinesBeatweenStation(d).ToList();
            for(int i=0;i<b.Count();i++)
            {
                Console.WriteLine(b.ElementAt(i));
            }
        }

    }
}