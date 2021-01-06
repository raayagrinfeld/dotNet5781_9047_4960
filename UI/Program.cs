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
            List<User> userlist = new List<User>();
            userlist = bl.GetAllUsers().ToList();
            for (int i = 0; i < userlist.Count; i++)
            {
                Console.WriteLine(userlist[i]);
            }
            List<BusLineBO> busLineBOs = bl.GetAllBusLines().ToList();
            for (int i = 0; i < busLineBOs.Count; i++)
            {
                Console.WriteLine(busLineBOs[i]);
            }
        }
    }
}