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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {
        public static IBL bl = BlFactory.GetBl("1");
        User user;
        BusLineBO selectedBusLine=null;
        StationBO selectedStation=null;
        //private ObservableCollection<BusLineBO> busLineBOObservableCollection;
        //private ObservableCollection<StationBO> StationBOObservableCollection;
        //private ObservableCollection<User> UserBOObservableCollection;

        public MainManagerWindow(User logedInUser)
        {
            //busLineBOObservableCollection = new ObservableCollection<BusLineBO>(bl.GetAllBusLines());
            //StationBOObservableCollection = new ObservableCollection<StationBO>(bl.GetAllBusStations());
            //UserBOObservableCollection = new ObservableCollection<User>(bl.GetAllUsers());
            user = logedInUser;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            busLineBOListView.ItemsSource = bl.GetAllBusLines();
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

        private void MenuItem_Click_ShowUserInterface(object sender, RoutedEventArgs e)
        {
            MainUserWindow userWindow = new MainUserWindow(user);
            userWindow.Show();
            this.Close();
        }

        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Close();
        }

        
        //--------filter BusLine, station and user-------------------
        private void SearchFilterChangedBusLine(object sender, TextChangedEventArgs e)
        {

            busLineBOListView.ItemsSource = new ObservableCollection<BusLineBO>((from item in bl.GetAllBusLines()
                                                                                      where CheckIfStringsAreEqual(lineNumber.Text, item.LineNumber.ToString())
                                                                                      select item
                                                                                        into g
                                                                                      where CheckIfStringsAreEqual(Area.Text, g.Area.ToString())
                                                                                      select g));



            //busLineBOListView.ItemsSource = it;
            //busLineBOObservableCollection = it;
        }
        private void SearchFilterChangedBusStation(object sender, TextChangedEventArgs e)
        {
            stationBOListView.ItemsSource = new ObservableCollection<StationBO>((from item in bl.GetAllBusStations()
                                                                                      where CheckIfStringsAreEqual(BusStationKey.Text, item.BusStationKey.ToString())
                                                                                      select item
                                                                                        into g
                                                                                      where CheckIfStringsAreEqual(StationName.Text, g.StationName)
                                                                                      select g));



            //stationBOListView.ItemsSource = it;
        }
        private bool CheckIfStringsAreEqual(string a, string b)
        {
            if (a.Length > b.Length)
                return false;
            int c = Math.Min(a.Length, b.Length);
            a = a.ToLower();
            b = b.ToLower();
            for (int i = 0; i < c; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }




        //----------------bus line butten clicks
        //add button click
        private void Button_Click_AddBusLine(object sender, RoutedEventArgs e)
        {
            busLineListBorder.Visibility = Visibility.Collapsed;
            BusLineBO AddbusLine = new BusLineBO();
            AddbusLine.BusLineKey = bl.getNextBusLineRunNumber();
            firstStationNameComboBox.DataContext = bl.GetAllBusStations();
            lastStationNameComboBox.DataContext = bl.GetAllBusStations();
            areaComboBox.DataContext = typeof(Areas);
            AddBusLineBorder.Visibility = Visibility.Visible;
        }
        private void Button_Click_AddBusFinalClick(object sender, RoutedEventArgs e)
        {
            AddBusLineBorder.Visibility = Visibility.Collapsed;
            busLineListBorder.Visibility = Visibility.Visible;
            bl.AddBusLine(AddBusDataGrid.DataContext as BusLineBO);
            busLineListBorder.DataContext = bl.GetAllBusLines();
        }

        //update button click
        private void Button_Click_UpdateBusInformation(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_UpdateStationInformation(object sender, RoutedEventArgs e)
        {

        }
        //delete button click
        private void Button_Click_DeleteBusLine(object sender, RoutedEventArgs e)
        {
            bl.DeleteBusLine(((sender as Button).DataContext as BusLineBO).BusLineKey);
            busLineBOListView.ItemsSource = bl.GetAllBusLines();
        }
        private void Button_Click_DeleteBusStaion(object sender, RoutedEventArgs e)
        {
            bl.DeleteBusStation(((sender as Button).DataContext as StationBO).BusStationKey);
            stationBOListView.ItemsSource = bl.GetAllBusStations();
        }
        //selction chenged
        private void busLineBOListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBusLine = (busLineBOListView.SelectedItem as BusLineBO);
            if (selectedBusLine != null)
            {
                busLineListBorder.Visibility = Visibility.Collapsed;
                BusLineDetialedBorder.Visibility = Visibility.Visible;
                busLineDetialedGrid.DataContext = selectedBusLine;
                busLineStationsListBox.ItemsSource = selectedBusLine.busLineStations;
            }
        }
        private void stationBOListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStation = (stationBOListView.SelectedItem as StationBO);
            if (selectedStation != null)
            {
                //busLineListBorder.Visibility = Visibility.Collapsed;
                //BusLineDetialedBorder.Visibility = Visibility.Visible;
                //busLineDetialedGrid.DataContext = selectedStation;
                //busLineStationsListBox.ItemsSource = selectedBusLine.busLineStations;
            }
        }

        //go back
        private void Button_Click_BackArrowBusLine(object sender, RoutedEventArgs e)
        {
            AddBusLineBorder.Visibility = Visibility.Collapsed;
            BusLineDetialedBorder.Visibility = Visibility.Collapsed;
            busLineListBorder.Visibility = Visibility.Visible;
        }


        //---------------- sort by header click
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
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
        private void Sort(string sortBy, ListSortDirection direction,ListView listView)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(listView.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        
    }
}

