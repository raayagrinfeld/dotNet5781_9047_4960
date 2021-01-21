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
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainUserWindow.xaml
    /// </summary>
    public partial class MainUserWindow : Window
    {
        public static IBL bl = BlFactory.GetBL();
        User user;
        Driving drive =new Driving();
        StationBO selectedStation;

        public MainUserWindow(User logedInUser)
        {
            InitializeComponent();
            user = logedInUser;
            StationsBox.ItemsSource = bl.GetAllBusStations();
            StationsBox2.ItemsSource = bl.GetAllBusStations();
            StationsBox.Text = bl.GetAllBusStations().ToString();
            NoBusLable.Visibility = Visibility.Collapsed;
            listBuses.Visibility = Visibility.Collapsed;
            stationBOListView.ItemsSource = bl.GetAllBusStations();

        }

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

        private void SearchBus_Click(object sender, RoutedEventArgs e)
        {
            if (StationsBox.SelectedIndex != -1 & StationsBox2.SelectedIndex != -1)
            {
                drive.Source = (StationBO)StationsBox.SelectedValue;
                drive.Destination = (StationBO)StationsBox2.SelectedItem;
                listBuses.ItemsSource = bl.fingALinesBeatweenStation(drive).ToList();
                if (bl.fingALinesBeatweenStation(drive).ToList().Count() == 0)
                {
                    NoBusLable.Visibility = Visibility.Visible;
                }
                else
                {
                    listBuses.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("You mast choose a source and destination", "Search message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region sort by header click
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void SearchFilterChangedBusStation(object sender, TextChangedEventArgs e)
        {

        }

        private void stationBOListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStation = (stationBOListView.SelectedItem as StationBO);
            if (selectedStation != null)
            {
                stationListBorder.Visibility = Visibility.Collapsed;
                StationDetailedBorder.Visibility = Visibility.Visible;
                grid1.DataContext = selectedStation;
                longtitudTextBox.Text = selectedStation.Coordinates.Longitude.ToString();
                latitudTextBox.Text = selectedStation.Coordinates.Latitude.ToString();
                listofBusAcurdingtoStationList.ItemsSource = selectedStation.busLines;
            }
        }
        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction, sender as ListView);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }
        private void Sort(string sortBy, ListSortDirection direction, ListView listView)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(listView.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
        #endregion
    }
}