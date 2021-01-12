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
using BlApi;
using BO;
using BL;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainUserWindow.xaml
    /// </summary>
    public partial class MainUserWindow : Window
    {
        public static IBL bl = BlFactory.GetBl("1");
        User user;
        Driving drive =new Driving();
        public MainUserWindow(User logedInUser)
        {
            InitializeComponent();
            user = logedInUser;
            StationsBox.ItemsSource = bl.GetAllBusStations();
            StationsBox2.ItemsSource = bl.GetAllBusStations();
            StationsBox.Text = bl.GetAllBusStations().ToString();
        }
     /*   public MainUserWindow()
        {
            InitializeComponent();
            StationsBox.ItemsSource = bl.GetAllBusStations();
            StationsBox2.ItemsSource = bl.GetAllBusStations();
        }*/
        private void Button_Click_MinimizeWindow(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void Button_Click_MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                SystemCommands.RestoreWindow(this);
            else
                SystemCommands.MaximizeWindow(this);
        }

        private void Button_Click_CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {

            ((Button)sender).Width *= 1.1;
            ((Button)sender).Height *= 1.1;

        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Width /= 1.1;
            ((Button)sender).Height /= 1.1;
        }

        private void When_Window_CLosed(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MenuItem_Click_OpenUserSettingsWindow(object sender, RoutedEventArgs e)
        {
            UserSettingsWindow userSettings = new UserSettingsWindow(user);
            userSettings.Show();
        }

        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Close();
        }

       private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource drivingViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("drivingViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // drivingViewSource.Source = [generic data source]
            //System.Windows.Data.CollectionViewSource stationBOViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("stationBOViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // stationBOViewSource.Source = [generic data source]
        }
        private void SearchBus_Click(object sender, RoutedEventArgs e)
        {
            if (StationsBox.SelectedIndex != -1 & StationsBox2.SelectedIndex != -1)
            {
                drive.Source = (StationBO)StationsBox.SelectedValue;
                drive.Destination = (StationBO)StationsBox2.SelectedItem;
                listBuses.ItemsSource = bl.fingALinesBeatweenStation(drive).ToList();
                listBuses.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("You mast choose a source and destination", "Search message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}