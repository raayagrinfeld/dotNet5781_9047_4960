using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BL;

namespace BlApi
{
    public static class BlFactory
    {
        public static IBL GetBL()
        {
            /*
            Type type = typeof(BlImp1);
            IBL bl = type.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null) as IBL;
            return bl;*/
            return BlImp1.Instance;
        }
    }
}