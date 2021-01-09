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
using Microsoft.Win32;

namespace UIwpf
{
    /// <summary>
    /// Interaction logic for UserSettings.xaml
    /// </summary>
    public partial class UserSettings : Window
    {
        OpenFileDialog op; //for getting image input from user
        bool isImageChanged = false;

        public UserSettings(User user)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            grid1.DataContext = user;
            if(user.imagePath!=null)
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
                isImageChanged = true;
                UserImage.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
    }
}
