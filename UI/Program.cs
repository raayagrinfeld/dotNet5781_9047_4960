using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using BlApi;
using BO;
using DO;
using APIDL;
using System.Device.Location;


namespace PlConsole
{
    class Program
    {
        static IBL bl= BlFactory.GetBL();
        static IDAL dl = DalFactory.GetDal();
        static void Main(string[] args)
        {
            Random r = new Random();
            dl.AddUser(new DO.User { UserName = "raaya", Password = "123", IsActive = true, ManagementPermission = true, Gender = DO.gender.female, imagePath = null,Salt=r.Next() });
            dl.AddUser(new DO.User { UserName = "odelia", Password = "1666", IsActive = true, ManagementPermission = true, Gender = (DO.gender)0, imagePath = "Icons/cancel.png", Salt = r.Next() });
            dl.AddUser(new DO.User { UserName = "aviva", Password = "1111", IsActive = true, ManagementPermission = false, Gender = (DO.gender)0, imagePath = null, Salt = r.Next() });
            dl.AddUser(new DO.User { UserName = "myiah", Password = "6543", IsActive = true, ManagementPermission = false, Gender = (DO.gender)0, imagePath = "Icons/wonan.png", Salt = r.Next() });
        }
    }
}