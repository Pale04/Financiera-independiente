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

namespace Financiera_GUI
{
    /// <summary>
    /// Lógica de interacción para wLogin.xaml
    /// </summary>
    public partial class wLogin : Window
    {
        public wLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Make logic for each user
        }

        private void hlResetPassword_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Add Funtionality for Use Case 3
        }

        private void tbUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            tbUsername.Clear();
        }
    }
}
