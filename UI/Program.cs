using System;
using System.Collections;
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
            userlist = (List<User>)bl.GetAllUsers();
            for (int i = 0; i < userlist.Count; i++)
            {
                Console.WriteLine(userlist[i]);
                userlist.RemoveAt(i);
            }

        }
    }
}