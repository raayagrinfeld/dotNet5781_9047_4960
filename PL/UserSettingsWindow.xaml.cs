using Microsoft.Win32;
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
    /// Interaction logic for UserSettingsWindow.xaml
    /// </summary>
    public partial class UserSettingsWindow : Window
    {
        OpenFileDialog op; //for getting image input from user
        User userWindow;
        public UserSettingsWindow(User user)
        {
            userWindow = user;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(gender));
            grid1.DataContext = user;
            if (user.imagePath != null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(System.IO.Path.GetFullPath(user.imagePath));
                bitmap.EndInit();
                UserImage.Source = bitmap;
            }
        }


        private void Button_Click_UploadImage(object sender, RoutedEventArgs e)
        {
            op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                userWindow.imagePath = op.FileName;
                UserImage.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
    }
}