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
using BL;
using BO;
using BlApi;

namespace UIwpf
{
    /// <summary>
    /// Interaction logic for signUp.xaml
    /// </summary>
    //public partial class signUp : Window
    //{
    //    IBL bl;
    //    public signUp()
    //    {
    //        InitializeComponent();
    //        bl = BlFactory.GetBl("1");
    //    }
    //    private void PassBox_passAdmin_KeyDown(object sender, KeyEventArgs e)
    //    {
    //        if (e.Key == Key.Return)
    //        {
    //            if (passwordTextBox.Password == passwordConfirmTextBox.Password)
    //            {
    //                try
    //                {
    //                    bl.AddUser(new User { IsActive = true, ManagementPermission = false, Password = passwordTextBox.Password, UserName = userNameTextBox.Text });

    //                }
    //                catch (BO.BadUserNameException ex)
    //                {
                        
    //                }
    //            }
    //        }
    //    }
    //}
}
