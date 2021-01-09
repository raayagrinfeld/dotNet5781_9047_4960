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
using BO;
using BL;
using BlApi;

namespace UIwpf
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        IBL bl = BlFactory.GetBl("1");

        public LogInWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void PassBox_passAdmin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Return)
            {
                User user = bl.GetUser(userNameTextBox.Text);
                if (user.Password==passwordTextBox.Password)
                {
                    MainWindowMangaerxaml windowMangaer = new MainWindowMangaerxaml(user);
                    windowMangaer.Show();
                    this.Close();
                }
            }
        }
    }
}