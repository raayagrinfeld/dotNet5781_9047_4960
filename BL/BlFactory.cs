using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL;

namespace BlApi
{
    public static class BlFactory
    {
        public static IBL GetBl(string type)
        {
            switch (type)
            {
                case "1":
                    return new BlImp1();
                case "2":
                //return new BLImp2();
                default:
                    return new BlImp1();
            }
        }
    }
}