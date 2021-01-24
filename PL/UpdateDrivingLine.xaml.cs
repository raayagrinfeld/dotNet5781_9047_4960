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
            DrivingLine driveline = grid1.DataContext as DrivingLine;
            driveline.IsActive = true;
            driveline.LastStationName = bl.GetBusLine(driveline.BusLineKey).LastStationName;
            try
            {
                bl.DeleteDrivingLine(driveline.BusLineKey, driveline.StartHour);
                driveline.StartHour = TimeSpan.Parse(startHourTextBox.Text);
                bl.AddDrivingLine(driveline);
                this.Close();
            }
            catch (BO.BadDrivingLineException ex)
            {
                MessageBox.Show(ex.Message, "problen in update driving line", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
