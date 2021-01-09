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


namespace PL
{
    /// <summary>
    /// Interaction logic for MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {
        public static IBL bl = BlFactory.GetBl("1");
        User user;
        //private ObservableCollection<BusLineBO> busLineBOObservableCollection;
        //private ObservableCollection<StationBO> StationBOObservableCollection;
        //private ObservableCollection<User> UserBOObservableCollection;

        public MainManagerWindow(User logedInUser)
        {
            //busLineBOObservableCollection = new ObservableCollection<BusLineBO>( bl.GetAllBusLines());
            //StationBOObservableCollection = new ObservableCollection<StationBO>( bl.GetAllBusStations());
            //UserBOObservableCollection = new ObservableCollection<User>( bl.GetAllUsers());
            user = logedInUser;
            InitializeComponent();

            busLineBODataGrid.ItemsSource = bl.GetAllBusLines();
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
            UserSettings userSettings = new UserSettings(user);
            userSettings.Show();
        }

        private void MenuItem_Click_ShowUserInterface(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Close();
        }

        private void Button_Click_DeleteBusLine(object sender, RoutedEventArgs e)
        {
            bl.DeleteBusLine(((sender as Button).DataContext as BusLineBO).BusLineKey);
            busLineBODataGrid.ItemsSource = bl.GetAllBusLines();
        }

        private void SearchFilterChanged(object sender, TextChangedEventArgs e)
        {

            ObservableCollection<BusLineBO> it = new ObservableCollection<BusLineBO>((from item in bl.GetAllBusLines()
                                                                                      where CheckIfStringsAreEqual(lineNumber.Text, item.LineNumber.ToString())
                                                                                      select item
                                                                                        into g
                                                                                      where CheckIfStringsAreEqual(Area.Text, g.Area.ToString())
                                                                                      select g));



            busLineBODataGrid.ItemsSource = it;
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
    }
}
}
