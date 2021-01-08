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
using System.Windows.Navigation;

namespace UIwpf
{
    /// <summary>
    /// Interaction logic for signUpOrLogIn.xaml
    /// </summary>
    public partial class signUpOrLogIn : Page
    {
        public signUpOrLogIn()
        {
            InitializeComponent();
            if (this.NavigationService == null)
            {
                this.goBackButton.IsEnabled = false;
            }
        }

        void goBackButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to previous entry in journal back stack
            if (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
