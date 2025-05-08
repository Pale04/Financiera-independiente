using Business_logic;
using DomainClasses;
using Notification.Wpf;
using SessionServiceReference;
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
    public partial class wResetPassword : Window
    {
        string user;
        Business_logic.AccountManager accountManager = new AccountManager();
        private readonly NotificationManager _notificationManager;

        public wResetPassword(string username)
        {
            this.user = username;
            InitializeComponent();
            SendEmail(user);
        }

        private void Code1_KeyDown(object sender, KeyEventArgs e)
        {
            if(Code1.Text.Length == 1)
            {
                Code2.Focus();
            }
            
        }

        private void Code2_KeyDown(object sender, KeyEventArgs e)
        {
            if (Code2.Text.Length == 1)
            {
                Code3.Focus();
            }
        }

        private void Code3_KeyDown(object sender, KeyEventArgs e)
        {
            if (Code3.Text.Length == 1)
            {
                Code4.Focus();
            }
        }

        private void Code4_KeyDown(object sender, KeyEventArgs e)
        {
            if (Code4.Text.Length == 1)
            {
                Code5.Focus();
            }
        }

        private void Code5_KeyDown(object sender, KeyEventArgs e)
        {

            if (Code5.Text.Length == 1)
            {
                Code6.Focus();
            }
        }

        private void Code6_KeyDown(object sender, KeyEventArgs e)
        {
            if (Code6.Text.Length == 1)
            {
                string code = getVerificationCode();
                if(code.Length < 6)
                {
                    _notificationManager.Show(NotificationMessages.ResetEmptyFields, NotificationType.Warning);
                }
                else
                {

                    if (accountManager.isCodeValid(user, code))
                    {
                        EnableFields();
                    }
                    else
                    {
                        _notificationManager.Show(NotificationMessages.ResetIncorrectCode);
                    }
                }
            }
        }

        public void EnableFields()
        {
            btnSend.IsEnabled = true;
            psbConfirm.IsEnabled = true;
            psbNewPassword.IsEnabled = true;
        }

        public string getVerificationCode()
        {
            string verificationCode;
            verificationCode = Code1.Text;
            verificationCode += Code2.Text;
            verificationCode += Code3.Text;
            verificationCode += Code4.Text;
            verificationCode += Code5.Text;
            verificationCode += Code6.Text;

            return verificationCode;

        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            EmployeeClass employee = new()
            {
                user = user
            };
            int response = accountManager.ChangePassword(employee, psbNewPassword.Password, psbConfirm.Password);

            switch (response)
            {
                case 0:
                    _notificationManager.Show(NotificationMessages.GlobalUpdateSuccess, NotificationType.Information);
                    break;

                case 1:
                    _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error);
                    break;
                case 2:
                    _notificationManager.Show(NotificationMessages.ResetInvalidCredentials, NotificationType.Warning);
                    break;
            }
        }

        

        private void hlReenvio_Click(object sender, RoutedEventArgs e)
        {
            EmployeeClass employee = new()
            {
                user = user
            };
            accountManager.SendEmail(employee);
        }

        private void SendEmail(string employee)
        {
            EmployeeClass employeeAccount = new()
            {
                user = employee
            };
            accountManager.SendEmail(employeeAccount);
        }

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          wLogin wLogin = new wLogin();
          wLogin.Show();
          this.Close();
        }
    }
}
