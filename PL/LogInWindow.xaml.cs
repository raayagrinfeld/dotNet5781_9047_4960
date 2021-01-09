﻿using System;
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
        IBL bl;

        public LogInWindow()
        {
            bl = BlFactory.GetBl("1");
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void PassBox_passAdmin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
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
                        MainUserWindow windowUser = new MainUserWindow();
                        windowUser.Show();
                        this.Close();
                    }
                }
            }
        }
        private void signup_Click(object sender, RoutedEventArgs e)
        {
            //signUp windowUser = new signUp();
            //windowUser.Show();
            this.Close();
            User user = bl.GetUser(userNameTextBox.Text);
            if (user.Password == passwordTextBox.Password)
            {
                MainManagerWindow windowMangaer = new MainManagerWindow(user);
                windowMangaer.Show();
                this.Close();
            }
        }
    }
}