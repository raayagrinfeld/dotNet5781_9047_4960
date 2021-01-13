using System;
using System.Media;
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
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        IBL bl=  BlFactory.GetBL();


        public LogInWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            
            CombBx_Gender.ItemsSource = Enum.GetValues(typeof(gender));
        }

        private void PassBox_passAdmin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    User user = bl.GetUser(userNameTextBox.Text);
                    if (user.Password == passwordTextBox.Password)
                    {
                        if (user.ManagementPermission)
                        {
                            MainManagerWindow windowMangaer = new MainManagerWindow(user);
                            windowMangaer.Show();
                            this.Close();
                        }
                        else
                        {
                            MainUserWindow windowUser = new MainUserWindow(user);
                            windowUser.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        SoundPlayer simpleSound = new SoundPlayer(@"c:/Windows/Media/Windows Background.wav");
                        simpleSound.Play();
                        passwordTextBox.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                        passwordTextBox.Clear();
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
        }
        private void signup_Click(object sender, RoutedEventArgs e)
        {
            login.Visibility = Visibility.Collapsed;
            signUp.Visibility = Visibility.Visible;
        }
        private void signup_InWindow_Click(object sender, RoutedEventArgs e)
        {
            exsist.Visibility = Visibility.Collapsed;
            try
            {
                bl.AddUser(new User { UserName = SignUpUser.Text, Password = signUpPaswword.Password, IsActive = true, ManagementPermission = false });
                User user = bl.GetUser(SignUpUser.Text);
                if (user.UserName == "" || user.Password == "")
                {
                    SoundPlayer simpleSound = new SoundPlayer(@"c:/Windows/Media/Windows Background.wav");
                    simpleSound.Play();
                    signUpPaswword.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                    signUpPaswword.Clear();
                    SignUpUser.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                    SignUpUser.Clear();
                }
                else
                {
                    if (CombBx_Gender.SelectedIndex != -1)
                    {
                        user.Gender = (gender)CombBx_Gender.SelectedItem;
                    }
                    else
                    {
                        user.Gender = gender.male;
                    }
                    MainUserWindow windowUser = new MainUserWindow(user);
                    windowUser.Show();
                    this.Close();
                }

            }
            catch (BO.BadUserNameException ex)
            {
                SoundPlayer simpleSound = new SoundPlayer(@"c:/Windows/Media/Windows Background.wav");
                simpleSound.Play();
                signUpPaswword.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                signUpPaswword.Clear();
                SignUpUser.BorderBrush = new SolidColorBrush(Color.FromRgb(250, 23, 23));
                SignUpUser.Clear();
                exsist.Visibility = Visibility.Visible;
            }
        }
    }
}