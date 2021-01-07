using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BlApi;
using BO;

namespace UIwpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static IBL bl = BlFactory.GetBl("1");
        public static int numOfActivatedMainWindow = 0;
        static App()
        {

        }
    }
}
