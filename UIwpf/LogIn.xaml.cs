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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;
using BlApi;

namespace UIwpf
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        static IBL bl  = BlFactory.GetBl("1");
        public LogIn()
        {
            InitializeComponent();
        }
        private void signup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddUser(new User { IsActive = true, ManagementPermission = false, Password = passwordTextBox.Text, UserName = userNameTextBox.Text });
            }
            catch(BO.BadUserNameException ex)
            {
                MessageBox.Show(ex.Message);
            }
            MainUserPage userPage = new MainUserPage();
            this.NavigationService.Navigate(userPage);
        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = bl.GetUser(userNameTextBox.Text);
                if(user.Password== passwordTextBox.Text)
                {
                    if(user.ManagementPermission==false)
                    {
                        MainUserPage userPage = new MainUserPage();
                        this.NavigationService.Navigate(userPage);
                    }
                    else
                    {
                        MainWindowMangaerxaml mangaerxaml = new MainWindowMangaerxaml();
                        this.NavigationService.Navigate(mangaerxaml);
                    }
                }
                else
                {
                    MessageBox.Show("wrong password, please try again");
                }
            }
            catch (BO.BadUserNameException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
