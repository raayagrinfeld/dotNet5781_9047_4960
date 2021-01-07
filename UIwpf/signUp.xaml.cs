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
using BlApi;

namespace UIwpf
{
    /// <summary>
    /// Interaction logic for signUp.xaml
    /// </summary>
    public partial class signUp : Window
    {
        static IBL bl;
        public signUp()
        {
            bl = BlFactory.GetBl("1");
            InitializeComponent();
            //signUpGrid.DataContext = bl.GetAllUsers(); 
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddUser(new User { IsActive = true, ManagementPermission = false, Password = passwordTextBox.Text, UserName = userNameTextBox.Text });
            }
            catch(BO.BadUserNameException ex)
            {
                MessageBox.Show(ex.Message);
            }
            Close();
        }



    }
}
