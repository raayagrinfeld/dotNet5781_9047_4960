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
        Microsoft.Win32.OpenFileDialog op; //for getting image input from user
        User user;
        BusLineBO selectedBusLine=null;
        StationBO selectedStation=null;
        User selectedUser = null;
       // OpenFileDialog op;
       // User userWindow;
        private ObservableCollection<BusLineBO> busLineBOObservableCollectionFilter;
        //private ObservableCollection<BusLineStationBO> nisoy;
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
            //latitudTextBox.
            refreshcontent();
        }

        #region General
        private void refreshcontent()
        {
            busLineBOObservableCollectionFilter = new ObservableCollection<BusLineBO>(bl.GetAllBusLines());
            userBOListView.ItemsSource = bl.GetAllUsers();
            UserBOObservableCollectionFilter = new ObservableCollection<User>(bl.GetAllUsers());
            lineNumber.Text = "";
            GropByArea.SelectedItem = null;
            UserNameSearch.Text = "";
            Premissiom.SelectedItem = null;
            if (selectedBusLine != null)
            {
                try
                {
                    selectedBusLine = bl.GetBusLine(selectedBusLine.BusLineKey);
                    busLineDetialedGrid.DataContext = selectedBusLine;
                    busLineStationsListview.ItemsSource = selectedBusLine.busLineStations;
                    DrivingLineListView.ItemsSource = bl.GetAllDrivingsBy(b => b.BusLineKey == selectedBusLine.BusLineKey &b.IsActive);
                }
                catch(BO.BadBusLineKeyException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR in updating the bus-line information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                busLineBOListView.ItemsSource = bl.GetAllBusLines();
            }
            if (selectedStation!=null)
            {
                try
                {
                    listofBusAcurdingtoStationList.ItemsSource = bl.GetBusStation(selectedStation.BusStationKey).busLines;
                }
                catch (BO.BadBusStationKeyException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR in updating the bus-station information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                stationBOListView.ItemsSource = bl.GetAllBusStations();
                StationName.Text = "";
                BusStationKey.Text = "";

            }
            if(selectedUser==null)
            {
                userBOListView.ItemsSource = bl.GetAllUsers();
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
        }

        //private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        //{
        //    LogInWindow logInWindow = new LogInWindow();
        //    logInWindow.Show();
        //    //this.Close();
        //}
        #endregion

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
        private void ClearGrouping_click_busLine(object sender, RoutedEventArgs e)
        {
            refreshcontent();
        }
        private void ClearGrouping_click_user(object sender, RoutedEventArgs e)
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
        #region add
        private void StationOptionsToAdd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationBO selectedStationToAdd = (StationOptionsToAdd.SelectedItem as StationBO);
            if (selectedStationToAdd != null & selectedBusLine != null)
            {
                try
                {
                    StationOptionsToAdd.Visibility = Visibility.Collapsed;
                    bl.AddStation(selectedBusLine, selectedStationToAdd.BusStationKey);
                    busLineStationsListview.ItemsSource = selectedBusLine.busLineStations;
                    refreshcontent();
                }
                catch (BadBusStationKeyException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR in adding station in bus", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BadBusLineStationsException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR in adding station in bus", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BadBusLineKeyException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR in adding station in bus", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void Button_Click_AddStationToBus(object sender, RoutedEventArgs e)
        {
            StationOptionsToAdd.Visibility = Visibility.Visible;
            StationOptionsToAdd.DataContext = bl.GetAllBusStations();
        }

        private void AddDriving_Click(object sender, RoutedEventArgs e)
        {
            AddDrivingLine addDriving = new AddDrivingLine(new DrivingLine { BusLineKey = selectedBusLine.BusLineKey, LastStationName = selectedBusLine.LastStationName });
            addDriving.ShowDialog();
            while (addDriving.IsActive)
            {
            }
            refreshcontent();
        }
        private void Button_Click_AddBusLine(object sender, RoutedEventArgs e)
        {
            busLineListBorder.Visibility = Visibility.Collapsed;
            BusLineBO AddbusLine = new BusLineBO();
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
                AddbusLine.BusLineKey = bl.getNextBusLineRunNumber();
                AddbusLine.IsActive = true;
                if (AddbusLine!=null)
                {
                    try
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
                    catch (BO.BadBusLineKeyException ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR in adding the new bus-line", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BO.BadBusStationKeyException ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR in adding the new bus-line", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (BO.BadBusLineStationsException ex)
                    {
                        MessageBox.Show(ex.Message, "ERROR in adding the new bus-line", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                
            }
            
        }
        #endregion

        #region delete
        private void Button_Click_DeleteStationFromBusLine(object sender, RoutedEventArgs e)
        {
            try
            {


                bl.deleteBusStationInBusLine(selectedBusLine, ((sender as Button).DataContext as BusLineStationBO).BusStationKey);
                refreshcontent();
            }
            catch (BO.BadBusLineStationsException ex)//if station doesnt exist oe if there is less the 3 station in bus-line
            {
                MessageBox.Show(ex.Message, "ERROR in deleting station in bus", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Button_Click_DeleteBusLine(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteBusLine(((sender as Button).DataContext as BusLineBO).BusLineKey);
                refreshcontent();
            }
            catch (BO.BadBusLineKeyException ex)
            {
                MessageBox.Show(ex.Message, "ERROR in deleting bus-line", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void Button_Click_DeleteBusLineFromStation(object sender, RoutedEventArgs e)//deletes line from station = deleting busstation in busline
        {
            try
            {
                bl.deleteBusStationInBusLine(((sender as Button).DataContext as BusLineBO), selectedStation.BusStationKey);
                refreshcontent();
            }
            catch (BO.BadBusLineStationsException ex)//if station doesnt exist oe if there is less the 3 station in bus-line
            {
                MessageBox.Show(ex.Message, "ERROR in deleting station in bus", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region selction chenged
        private void busLineBOListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBusLine = (busLineBOListView.SelectedItem as BusLineBO);
            if (selectedBusLine != null)
            {
                busLineListBorder.Visibility = Visibility.Collapsed;
                BusLineDetialedBorder.Visibility = Visibility.Visible;
                busLineDetialedGrid.DataContext = selectedBusLine;
                busLineStationsListview.ItemsSource = selectedBusLine.busLineStations;
                DrivingLineListView.ItemsSource = bl.GetAllDrivingsBy(b => b.BusLineKey == selectedBusLine.BusLineKey);
            }
        }
        private void DrivingLineView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDrivingLine updateDriving = new UpdateDrivingLine (DrivingLineListView.SelectedItem as DrivingLine);
            updateDriving.ShowDialog();
            while (updateDriving.IsActive)
            {
            }
            refreshcontent();
        }
        private void Button_Click_DeleteDrivingLine(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteDrivingLine(((sender as Button).DataContext as DrivingLine).BusLineKey, ((sender as Button).DataContext as DrivingLine).StartHour);
                refreshcontent();
            }
            catch(BadDrivingLineException ex)
            {
                MessageBox.Show(ex.Message, "ERROR in deleting driving line of bus-line", MessageBoxButton.OK, MessageBoxImage.Error);
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
                if(selectedUser.imagePath!="")
                { 
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(selectedUser.imagePath));
                    bitmap.EndInit();
                    UserImage.Source = bitmap;
                }
            }
        }
        #endregion

        #region go back
        private void Button_Click_BackArrowBusLine(object sender, RoutedEventArgs e)
        { 
            AddBusLineBorder.Visibility = Visibility.Collapsed;
            BusLineDetialedBorder.Visibility = Visibility.Collapsed;
            busLineListBorder.Visibility = Visibility.Visible;
            StationOptionsToAdd.Visibility = Visibility.Collapsed;
            selectedBusLine = null;
            refreshcontent();
        }
        #endregion
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
        #region add
        private void addStation_Click(object sender, RoutedEventArgs e)
        {
            stationListBorder.Visibility = Visibility.Collapsed;
            addStationBorder.Visibility = Visibility.Visible;
        }
        private void add_From_addStationWindow_Click(object sender, RoutedEventArgs e)
        {
            if (addLatitudeTextBox.Text != "" & addLongitudeTextBox.Text != "")
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
                                    bl.AddBusStation(new StationBO { busLines = null, BusStationKey = bl.getNextBusStationRunNumber(), Coordinates = new GeoCoordinate(Double.Parse(addLatitudeTextBox.Text), Double.Parse(addLongitudeTextBox.Text)), StationAddress = addStationAddressTextBox.Text, IsActive = true, HasARoof = (bool)addRoof.IsChecked, StationName = addStationNameTextBox.Text });
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
        #endregion

        #region delete
        private void Button_Click_DeleteBusStaion(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteBusStation(((sender as Button).DataContext as StationBO).BusStationKey);
                refreshcontent();
            }
            catch (BO.BadBusLineStationsException ex)
            {
                MessageBox.Show(ex.Message, "ERROR in deleting station", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region filters
        private void TextBox_OnlyNumbersAndDigit_PreviewKeyDown(object sender, KeyEventArgs e)//allow only numbers
        {
                TextBox text = sender as TextBox;
                if (text == null) return;
                if (e == null) return;

                //allow get out of the text box
                if ((e.Key == Key.Enter) || e.Key == Key.Return || e.Key == Key.Tab)
                    return;

                //allow list of system keys (add other key here if you want to allow)
                if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                    e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
                 || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right|| e.Key==Key.OemPeriod)
                    return;

                char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

                //allow control system keys
                if (Char.IsControl(c)) return;

                //allow digits (without Shift or Alt)
                if (Char.IsDigit(c))
                    if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                        return; //let this key be written inside the textbox

                //forbid letters and signs (#,$, %, ...)
                e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
                return;
        }
        #endregion

        #region go back
        private void Button_Click_BackArrowBusStation(object sender, RoutedEventArgs e)
        {
            stationListBorder.Visibility = Visibility.Visible;
            addStationBorder.Visibility = Visibility.Collapsed;
            StationDetailedBorder.Visibility = Visibility.Collapsed;
            if(selectedStation!=null)
            {
                try
                {
                    selectedStation.Coordinates = new GeoCoordinate(Double.Parse(latitudTextBox.Text), Double.Parse(longtitudTextBox.Text));
                    bl.UpdateBusStation(selectedStation);
                }
                catch (BadBusLineKeyException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR in updating the bus-station information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            selectedStation = null;
            refreshcontent();
        }
        #endregion

        #region map
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowLocation showLocation = new ShowLocation("http://maps.google.com/maps?&z=15&q=" + latitudTextBox.Text + "+" + longtitudTextBox.Text + "&ll=" + latitudTextBox.Text + "+" + longtitudTextBox.Text);
            showLocation.Show();
        }
        #endregion
        #endregion

        #region users click
        #region image
        private void Button_Click_UploadImage(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                op = new OpenFileDialog();
                op.Title = "Select a picture";
                op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                  "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                  "Portable Network Graphic (*.png)|*.png";
                if (op.ShowDialog() == true)
                {
                    string s = op.FileName;
                    if (s.Contains("UserIcons"))
                    {
                        s = s.Remove(0, s.IndexOf("UserIcons"));
                    }
                    selectedUser.imagePath = s;
                    UserImage.Source = new BitmapImage(new Uri(op.FileName));
                }
            }
        }
        #endregion

        #region add
        private void Button_addUser_Click(object sender, RoutedEventArgs e)
        {
            userListBorder.Visibility = Visibility.Collapsed;
            addUserBorder.Visibility = Visibility.Visible;
        }
        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            User userToAdd = new User { UserName = userNameTextBox.Text, Password = passwordTextBox.Text, IsActive = true, ManagementPermission = false };
            if (userToAdd.UserName == "" || userToAdd.Password == "")
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
                    userToAdd.Gender = (gender)CombBx_Gender.SelectedValue;
                }
                else
                {
                    userToAdd.Gender = gender.male;
                }
                try
                {
                    bl.AddUser(userToAdd);
                    refreshcontent();
                    passwordTextBox.Clear();
                    userNameTextBox.Clear();
                    CombBx_Gender.SelectedIndex = -1;
                    userListBorder.Visibility = Visibility.Visible;
                    addUserBorder.Visibility = Visibility.Collapsed;
                }
                catch (BadUserNameException ex)
                {
                    SoundPlayer simpleSound = new SoundPlayer(@"c:/Windows/Media/Windows Background.wav");
                    simpleSound.Play();
                    passwordTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                    passwordTextBox.Clear();
                    userNameTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                    userNameTextBox.Clear();
                    MessageBox.Show(ex.Message, "ERROR in adding the user information", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        #endregion

        #region go back
        private void Button_Click_BackArrowUser(object sender, RoutedEventArgs e)
        {
            userListBorder.Visibility = Visibility.Visible;
            addUserBorder.Visibility = Visibility.Collapsed;
            UserDetialedBorder.Visibility = Visibility.Collapsed;
            if(selectedUser!=null)
            {
                try
                {
                    bl.UpdateUser(selectedUser);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(System.IO.Path.GetFullPath("UserIcons/user.png"));
                    bitmap.EndInit();
                    UserImage.Source = bitmap;
                }
                catch(BadUserNameException ex)
                {
                    MessageBox.Show(ex.Message, "ERROR in updating the user information", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            selectedUser = null;
            refreshcontent();
        }
        #endregion

        #region delete
        private void Button_Click_DeleteUser(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeletUser(((sender as Button).DataContext as User).UserName);
                refreshcontent();
            }
            catch(BadUserNameException ex)
            {
                MessageBox.Show(ex.Message, "ERROR in deleting user", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #endregion




    }
}

