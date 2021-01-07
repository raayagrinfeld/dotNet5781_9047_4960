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
        static IBL bl;
        public LogIn()
        {
            InitializeComponent();
            bl = BlFactory.GetBl("1");
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
            //להוסיף כאן שצריך ללכת לחלון של היוזר דטה
        }
        private void login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = bl.GetUser(userNameTextBox.Text);
                if(user.Password== passwordTextBox.Text)
                {
                    // להוסיף גם כאן גישה לחלון של היוזר דטה
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
            //להוסיף כאן שצריך ללכת לחלון של היוזר דטה
        }

    }
}
