using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using BL;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for UpdateDrivingLine.xaml
    /// </summary>
    public partial class UpdateDrivingLine : Window
    {
        IBL bl = BlFactory.GetBL();
        public UpdateDrivingLine(DrivingLine drivingLine)
        {
            InitializeComponent();
            grid1.DataContext = drivingLine;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DrivingLine drivingLine = grid1.DataContext as DrivingLine;
            drivingLine.IsActive = true;
            drivingLine.LastStationName = bl.GetBusLine(drivingLine.BusLineKey).LastStationName;
            try
            {
                bl.AddDrivingLine(drivingLine);
            }
            catch (BO.BadDrivingLineException ex)
            {

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource drivingLineViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("drivingLineViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // drivingLineViewSource.Source = [generic data source]
        }
    }
}
