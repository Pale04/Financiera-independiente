using Business_logic;
using DomainClasses;
using Financiera_GUI.MainMenus;
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
    public partial class wLogin : Window
    {
        LoginManager loginManager = new LoginManager();
        EmployeeClass employeeLogin = new EmployeeClass();
        public wLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text.Equals(null) || psbPassword.Password.Equals(null))
            {
                lbPasswordEmptyField.Visibility = Visibility.Visible;
                lbUserEmptyField.Visibility = Visibility.Visible;
            }
            else
            {
                employeeLogin.user = tbUsername.Text;
                employeeLogin.password = psbPassword.Password;

                int codeLogin = loginManager.Login(employeeLogin);
                if(codeLogin == 0)
                {
                    employeeLogin = loginManager.getSessionInfo(employeeLogin.user);
                    switch (employeeLogin.role)
                    {
                        case "admin":
                            //TODO: Change to wSystemManagment
                            break;
                        case "analist":
                            //TODO: Change to wCreditApplication
                            break;
                        case "adviser":
                            //TODO: Change to adviser menu
                            break;
                        case "collector":
                            //TODO: Change to wPaymentManagment
                            break;
                    }
                }
                else
                {
                    //TODO: SendMessage
                }
                
            }
        }

        private void hlResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text.Equals(null))
            {
                lbUserEmptyField.Visibility = Visibility.Visible;
            }
            else
            {
                wResetPassword resetPassword = new wResetPassword(tbUsername.Text);
                resetPassword.Show();
                this.Close();
            }

        }

        private void tbUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            tbUsername.Clear();
        }
    }
}
