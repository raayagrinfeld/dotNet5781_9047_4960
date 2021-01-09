using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace UIwpf
{
    /// <summary>
    /// Interaction logic for MainWindowMangaerxaml.xaml
    /// </summary>
    /// 
    
    
    public partial class MainWindowMangaerxaml : Window
    {
        public static IBL bl = BlFactory.GetBl("1");

        private ObservableCollection<BusLineBO> busLineBOObservableCollection;
        private ObservableCollection<StationBO> StationBOObservableCollection;
        private ObservableCollection<User> UserBOObservableCollection;

        public MainWindowMangaerxaml()
        {
            busLineBOObservableCollection = new ObservableCollection<BusLineBO>( bl.GetAllBusLines());
            StationBOObservableCollection = new ObservableCollection<StationBO>( bl.GetAllBusStations());
            UserBOObservableCollection = new ObservableCollection<User>( bl.GetAllUsers());
            
            InitializeComponent();

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

        }

        private void MenuItem_Click_ShowUserInterface(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_DeleteBusLine(object sender, RoutedEventArgs e)
        {

        }

        private void SearchFilterChanged(object sender, TextChangedEventArgs e)
        {
            
            //ObservableCollection<BusLineBO> it = new ObservableCollection<BusLineBO>((from item in busLineBOObservableCollection
            //                                                                            where CheckIfStringsAreEqual(lineNumber.text, item.LineNumber)
            //                                                                            select item
            //                                                                            into g
            //                                                                            where CheckIfStringsAreEqual(LestName.Text, g.LastName)
            //                                                                            select g



            //busLineBODataGrid.ItemsSource = it;
            //numOfTesters.Text = it.Count.ToString();
            
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
