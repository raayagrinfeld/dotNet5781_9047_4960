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
using System.Windows.Navigation;
using System.Windows.Media;
using System.Media;
using BlApi;
using BO;
using BL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Device.Location;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

enum Managment { Managmaent, regularUser};
namespace PL
{
    /// <summary>
    /// Interaction logic for MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {
        public static IBL bl = BlFactory.GetBL();
        User user;
        BusLineBO selectedBusLine=null;
        StationBO selectedStation=null;
        User selectedUser = null;
       // OpenFileDialog op;
       // User userWindow;
        private ObservableCollection<BusLineBO> busLineBOObservableCollectionFilter;
        //private ObservableCollection<StationBO> StationBOObservableCollection;
        private ObservableCollection<User> UserBOObservableCollectionFilter;

        public MainManagerWindow(User logedInUser)
        {
            user = logedInUser;
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(gender));
            GenderTextBox.ItemsSource= Enum.GetValues(typeof(gender));
            GropByArea.ItemsSource= Enum.GetValues(typeof(Areas));
            Premissiom.ItemsSource = Enum.GetValues(typeof(Managment));
            refreshcontent();
           
        }


        private void refreshcontent()
        {
            busLineBOListView.ItemsSource = bl.GetAllBusLines();
            busLineBOObservableCollectionFilter= new ObservableCollection<BusLineBO>(bl.GetAllBusLines());
            stationBOListView.ItemsSource = bl.GetAllBusStations();
            userBOListView.ItemsSource = bl.GetAllUsers();
            UserBOObservableCollectionFilter = new ObservableCollection<User>(bl.GetAllUsers());
            lineNumber.Text = "";
            GropByArea.SelectedItem = null;
            BusStationKey.Text = "";
            StationName.Text = "";
            UserNameSearch.Text = "";
            Premissiom.SelectedItem = null;
            if (selectedBusLine != null)
            {
                busLineStationsListBox.ItemsSource = selectedBusLine.busLineStations;
            }
            if (selectedStation != null)
            {
                listofBusAcurdingtoStationList.ItemsSource = selectedStation.busLines;
            }
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
            refreshcontent();
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


#region filter BusLine, station and user
        private void SearchFilterChangedBusLine(object sender, TextChangedEventArgs e)
        {

            busLineBOListView.ItemsSource = new ObservableCollection<BusLineBO>((from item in busLineBOObservableCollectionFilter
                                                                                        where CheckIfStringsAreEqual(lineNumber.Text, item.LineNumber.ToString())
                                                                                 select item));
        }
        private void GropByArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in bl.GetBusLineGrouptByArea())
                if (GropByArea.SelectedItem != null && item.Key == (Areas)GropByArea.SelectedItem)
                {
                    busLineBOObservableCollectionFilter = new ObservableCollection<BusLineBO>(item);
                    busLineBOListView.ItemsSource = busLineBOObservableCollectionFilter;
                }
            lineNumber.Text ="";
        }
        private void ClearGrouping_click(object sender, RoutedEventArgs e)
        {
            refreshcontent();
        }
        private void SearchFilterChangedBusStation(object sender, TextChangedEventArgs e)
        {
            stationBOListView.ItemsSource = new ObservableCollection<StationBO>((from item in bl.GetAllBusStations()
                                                                                      where CheckIfStringsAreEqual(BusStationKey.Text, item.BusStationKey.ToString())
                                                                                      select item
                                                                                        into g
                                                                                      where CheckIfStringsAreEqual(StationName.Text, g.StationName)
                                                                                      select g));
        }
        private void SearchFilterChangedUser(object sender, TextChangedEventArgs e)
        {
            userBOListView.ItemsSource = new ObservableCollection<User>((from item in bl.GetAllUsers()
                                                                         where CheckIfStringsAreEqual(UserNameSearch.Text, item.UserName)
                                                                         select item));                                            
        }
        private void Premissiom_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in bl.GetUserGrouptByManagment())
                if (Premissiom.SelectedItem != null && (item.Key == ((int)Premissiom.SelectedItem==0)))
                {
                    UserBOObservableCollectionFilter = new ObservableCollection<User>(item);
                    userBOListView.ItemsSource = UserBOObservableCollectionFilter;
                }
            UserNameSearch.Text = "";
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


        #endregion

#region bus line butten clicks
        //add button click
        private void Button_Click_AddBusLine(object sender, RoutedEventArgs e)
        {
            busLineListBorder.Visibility = Visibility.Collapsed;
            BusLineBO AddbusLine = new BusLineBO();
            AddbusLine.BusLineKey = bl.getNextBusLineRunNumber();
            AddBusDataGrid.DataContext = AddbusLine;
            firstStationNameComboBox.DataContext = bl.GetAllBusStations();
            lastStationNameComboBox.DataContext = bl.GetAllBusStations();
            areaComboBox.ItemsSource = Enum.GetValues(typeof(Areas));
            AddBusLineBorder.Visibility = Visibility.Visible;
        }
        private void Button_Click_AddBusFinalClick(object sender, RoutedEventArgs e)
        {
            if(lineNumberTextBox1.Text==""|| firstStationNameComboBox.SelectedItem==null|| lastStationNameComboBox.SelectedItem == null|| areaComboBox.SelectedItem==null)
            {
                MessageBox.Show("There is not enough information to create the bus, Fill in the rest or cancel", "Problem in the process of adding busline", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                BusLineBO AddbusLine = AddBusDataGrid.DataContext as BusLineBO;
                if(AddbusLine!=null)
                {
                    bl.AddBusLine(AddbusLine);
                    bl.AddStation(AddbusLine, (firstStationNameComboBox.SelectedItem as StationBO).BusStationKey);
                    bl.AddStation(AddbusLine, (lastStationNameComboBox.SelectedItem as StationBO).BusStationKey);
                    AddbusLine.Area = (Areas)areaComboBox.SelectedItem;
                    AddbusLine.LineNumber = Int32.Parse(lineNumberTextBox1.Text);
                    bl.UpdateBusLine(AddbusLine);
                    AddBusLineBorder.Visibility = Visibility.Collapsed;
                    busLineListBorder.Visibility = Visibility.Visible;
                    refreshcontent();
                }
                
            }
            
        }

        //update button click
        private void Button_Click_UpdateBusInformation(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_UpdateStationInformation(object sender, RoutedEventArgs e)
        {

        }
        


        //delete button click
        private void Button_Click_DeleteStationFromBusLine(object sender, RoutedEventArgs e)
        {
            bl.deleteBusStationInBusLine(selectedBusLine, ((sender as Button).DataContext as BusLineStationBO).BusStationKey);
            refreshcontent();
        }
        private void Button_Click_DeleteBusLine(object sender, RoutedEventArgs e)
        {
            bl.DeleteBusLine(((sender as Button).DataContext as BusLineBO).BusLineKey);
            refreshcontent();
        }
        private void Button_Click_DeleteBusStaion(object sender, RoutedEventArgs e)
        {
            bl.DeleteBusStation(((sender as Button).DataContext as StationBO).BusStationKey);
            refreshcontent();
        }
        private void Button_Click_DeleteBusLineFromStation(object sender, RoutedEventArgs e)
        {
            bl.deleteBusStationInBusLine(((sender as Button).DataContext as BusLineBO), selectedStation.BusStationKey);
            refreshcontent();
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
        private void busLineBOListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            busLineBOListView_SelectionChanged(sender, null);
        }
        private void stationBOListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStation = (stationBOListView.SelectedItem as StationBO);
            if (selectedStation != null)
            {
                stationListBorder.Visibility = Visibility.Collapsed;
                StationDetailedBorder.Visibility = Visibility.Visible;
                grid1.DataContext = selectedStation;
                listofBusAcurdingtoStation.DataContext = selectedStation.busLines;
                longtitudTextBox.Text = selectedStation.Coordinates.Longitude.ToString();
                latitudTextBox.Text = selectedStation.Coordinates.Latitude.ToString();
                listofBusAcurdingtoStationList.ItemsSource = selectedStation.busLines;
            }
        }
        private void UserListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = (userBOListView.SelectedItem as User);
            if (selectedUser != null)
            {
                userListBorder.Visibility = Visibility.Collapsed;
                UserDetialedBorder.Visibility = Visibility.Visible;
                UserDetialedGrid.DataContext = selectedUser;
            }
        }

        //go back
        private void Button_Click_BackArrowBusLine(object sender, RoutedEventArgs e)
        {
            AddBusLineBorder.Visibility = Visibility.Collapsed;
            BusLineDetialedBorder.Visibility = Visibility.Collapsed;
            busLineListBorder.Visibility = Visibility.Visible;
            StationOptionsToAdd.Visibility = Visibility.Collapsed;
            selectedBusLine = null;
        }

        #endregion

#region sort by header click
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
        #endregion

#region stations click
        private void addStation_Click(object sender, RoutedEventArgs e)
        {
            stationListBorder.Visibility = Visibility.Collapsed;
            addStationBorder.Visibility = Visibility.Visible;
        }
        private void add_From_addStationWindow_Click(object sender, RoutedEventArgs e)
        {
            if(addLatitudeTextBox.Text!=""&addLongitudeTextBox.Text!="")
            {
                if (31 < Double.Parse(addLatitudeTextBox.Text) & Double.Parse(addLatitudeTextBox.Text) < 35)
                {
                    if (31 < Double.Parse(addLongitudeTextBox.Text) & Double.Parse(addLongitudeTextBox.Text) < 35)
                    {
                        if (addStationAddressTextBox.Text != "")
                        {   
                            if (addStationNameTextBox.Text != "")
                            {
                                    try
                                    {
                                        Random r = new Random();
                                        bl.AddBusStation(new StationBO { busLines = null, BusStationKey = 39100+r.Next(0,899),Coordinates=new GeoCoordinate(Double.Parse(addLatitudeTextBox.Text), Double.Parse(addLongitudeTextBox.Text)),StationAddress= addStationAddressTextBox.Text,IsActive=true, HasARoof= (bool)addRoof.IsChecked,StationName= addStationNameTextBox.Text });
                                        refreshcontent();
                                        stationListBorder.Visibility = Visibility.Visible;
                                        addStationBorder.Visibility = Visibility.Collapsed;

                                    }
                                    catch (BO.BadBusStationKeyException ex)
                                    {
                                        exsistStation.Visibility = Visibility.Visible;
                                    }
                            }
                            else
                            {
                                addStationNameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                                addStationNameTextBox.Clear();
                            }
                        }
                        else
                        {
                            addStationAddressTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                            addStationAddressTextBox.Clear();
                        }
                    }
                    else
                    {
                        addLongitudeTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                        addLongitudeTextBox.Clear();
                    }
                }
                else
                {
                    addLatitudeTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                    addLatitudeTextBox.Clear();
                }
            }
            else
            {
                addLongitudeTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                addLongitudeTextBox.Clear();

                addLatitudeTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                addLatitudeTextBox.Clear();
            }
        }
        private void Button_Click_BackArrowBusStation(object sender, RoutedEventArgs e)
        {
            stationListBorder.Visibility = Visibility.Visible;
            addStationBorder.Visibility = Visibility.Collapsed;
            StationDetailedBorder.Visibility = Visibility.Collapsed;
        }
        #endregion

#region users click
        //go to add user window
        private void Button_addUser_Click(object sender, RoutedEventArgs e)
        {
            userListBorder.Visibility = Visibility.Collapsed;
            addUserBorder.Visibility = Visibility.Visible;
        }
        private void Button_Click_BackArrowUser(object sender, RoutedEventArgs e)
        {
            userListBorder.Visibility = Visibility.Visible;
            addUserBorder.Visibility = Visibility.Collapsed;
            UserDetialedBorder.Visibility = Visibility.Collapsed;
        }
        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddUser(new User { UserName = userNameTextBox.Text, Password = passwordTextBox.Text, IsActive = true, ManagementPermission = false });
                User user = bl.GetUser(userNameTextBox.Text);
                if (user.UserName == "" || user.Password == "")
                {
                    SoundPlayer simpleSound = new SoundPlayer(@"c:/Windows/Media/Windows Background.wav");
                    simpleSound.Play();
                    passwordTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                    passwordTextBox.Clear();
                    userNameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                    userNameTextBox.Clear();
                }
                else
                {
                    if (CombBx_Gender.SelectedIndex != -1)
                    {
                        user.Gender = (gender)CombBx_Gender.SelectedValue;
                    }
                    else
                    {
                        user.Gender = gender.male;
                    }
                    refreshcontent();
                    passwordTextBox.Clear();
                    userNameTextBox.Clear();
                    CombBx_Gender.SelectedIndex = -1;
                    userListBorder.Visibility = Visibility.Visible;
                    addUserBorder.Visibility = Visibility.Collapsed;
                }

            }
            catch (BO.BadUserNameException ex)
            {
                SoundPlayer simpleSound = new SoundPlayer(@"c:/Windows/Media/Windows Background.wav");
                simpleSound.Play();
                passwordTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                passwordTextBox.Clear();
                userNameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                userNameTextBox.Clear();
            }
        }
        private void Button_Click_DeleteUser(object sender, RoutedEventArgs e)
        {
            bl.DeletUser(((sender as Button).DataContext as User).UserName);
            refreshcontent();
        }

        #endregion

        private void cordinationChanged(object sender, TextChangedEventArgs e)
        {
          //  selectedStation.Coordinates = new GeoCoordinate(double.Parse(latitudTextBox.Text), double.Parse(longtitudTextBox.Text));
        }

        private void Button_Click_AddStationToBus(object sender, RoutedEventArgs e)
        {
            StationOptionsToAdd.Visibility = Visibility.Visible;
            StationOptionsToAdd.DataContext = bl.GetAllBusStations();
        }

        private void StationOptionsToAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationBO selectedStationToAdd = (StationOptionsToAdd.SelectedItem as StationBO);
            if (selectedStationToAdd != null& selectedBusLine!=null)
            {
                StationOptionsToAdd.Visibility = Visibility.Collapsed;
                bl.AddStation(selectedBusLine, selectedStationToAdd.BusStationKey);
                refreshcontent();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowLocation showLocation = new ShowLocation("http://maps.google.com/maps?&z=15&q=" +latitudTextBox.Text +"+" + longtitudTextBox.Text + "&ll=" +latitudTextBox.Text +"+" + longtitudTextBox.Text);
            showLocation.Show();
        }

        
    }
}

