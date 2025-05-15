using Business_logic;
using DomainClasses;
using Financiera_GUI.MainMenus;
using Notification.Wpf;
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

namespace Financiera_GUI
{
    public partial class wLogin : Window
    {
        LoginManager loginManager = new LoginManager();
        EmployeeClass employeeLogin = new EmployeeClass();
        private readonly NotificationManager _notificationManager;
        public wLogin()
        {
            InitializeComponent();
            _notificationManager = new NotificationManager();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbUsername.Text) || string.IsNullOrEmpty(psbPassword.Password))
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
                    UserSession.Instance.Employee = employeeLogin;
                    wFinancieraIndependiente mainMenu = new wFinancieraIndependiente();
                    mainMenu.Show();
                    this.Close();
                }
                else
                {
                    switch (codeLogin)
                    {
                        case 1:
                            _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error);
                            break;
                        case 2:
                            _notificationManager.Show(NotificationMessages.LoginWrongCredentials, NotificationType.Error);
                            break;
                        case 3:
                            _notificationManager.Show(NotificationMessages.LoginActiveSessions, NotificationType.Warning);
                            break;
                        case 4:
                            _notificationManager.Show(NotificationMessages.LoginUserNotFund, NotificationType.Warning);
                            break;
                    }
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
                string username = tbUsername.Text;
                wResetPassword wReset = new wResetPassword(username);
                wReset.Show();
                this.Close();
            }

        }

        private void tbUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            tbUsername.Clear();
        }
    }
}
