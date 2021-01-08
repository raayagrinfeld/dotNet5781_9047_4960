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
        private ObservableCollection<BusLineBO> busLineBOFilter;

        public MainWindowMangaerxaml()
        {
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
            if (--App.numOfActivatedMainWindow == 0)
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

        //private void SearchFilterChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (busLineBOFilter != null)
        //    {
        //        ObservableCollection<BusLineBO> it = new ObservableCollection<BusLineBO>((from item in busLineBOFilter
        //                                                                                  where CheckIfStringsAreEqual(FirstName.Text, item.FirstName)
        //                                                                            select item
        //                                                                        into g
        //                                                                            where CheckIfStringsAreEqual(LestName.Text, g.LastName)
        //                                                                            select g
        //                                                                        into j
        //                                                                            where CheckIfStringsAreEqual(ID.Text, j.Id)
        //                                                                            select j).ToList());
        //        TestersList.ItemsSource = it;
        //        numOfTesters.Text = it.Count.ToString();
        //    }
        //}
        //private bool CheckIfStringsAreEqual(string a, string b)
        //{
        //    if (a.Length > b.Length)
        //        return false;
        //    int c = Math.Min(a.Length, b.Length);
        //    a = a.ToLower();
        //    b = b.ToLower();
        //    for (int i = 0; i < c; i++)
        //    {
        //        if (a[i] != b[i])
        //            return false;
        //    }
        //    return true;
        //}
        //private void SearchFilterChanged(object sender, TextChangedEventArgs e)//search a bus
        //{
        //    sender
        //    foreach (var item in busLineBOObservableCollection)
        //    {
        //        ListBoxItem bus = (ListBoxItem)busesBox.ItemContainerGenerator.ContainerFromItem(item);
        //        string searchS = SearchBox.Text;
        //        int num = searchS.Length;
        //        if ((num <= item.LicenseNumber.Length) && (searchS == (item as Bus).LicenseNumber.Substring(0, num)))
        //        {
        //            bus.Visibility = Visibility.Visible;
        //        }
        //        else
        //            bus.Visibility = Visibility.Collapsed;
        //    }
        //}
    }
}
