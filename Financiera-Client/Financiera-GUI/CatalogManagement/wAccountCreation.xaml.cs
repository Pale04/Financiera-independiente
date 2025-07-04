﻿using Business_logic;
using DomainClasses;
using Notification.Wpf;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Financiera_GUI.CatalogManagement
{
    public partial class wAccountCreation : Page
    {
        NotificationManager _notificationManager;

        public wAccountCreation()
        {
            InitializeComponent();
            _notificationManager = new NotificationManager();
            DateTime currentDate = DateTime.Now;
            //birthdayPicker.BlackoutDates.Add(new CalendarDateRange(currentDate.AddYears(-18), DateTime.Now.AddYears(100)));
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult confirmation = MessageBox.Show($"¿Está seguro que desea salir? Los cambios se perderán", "Cancelar registro", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (confirmation == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
            }
        }

        private void CreateAccount(object sender, RoutedEventArgs e)
        {
            ClearErrorLabels();
            if (CheckForEmptyOrWrongFields())
            {
                return;
            }

            saveBtn.IsEnabled = false;

            string name = nameField.textField.Text.Trim();
            string address = addressField.textField.Text.Trim();
            string phone = phoneField.textField.Text.Trim();
            string mail = mailField.textField.Text.Trim();
            string username = userField.textField.Text.Trim();
            string password = pwdField.Password.Trim();
            DateTime? birthday = birthdayPicker.SelectedDate;
            string role = GetSelectedRole();

            AccountManager manager = new();

            EmployeeClass employee = new()
            {
                Name = name,
                Address = address,
                PhoneNumber = phone,
                Mail = mail,
                User = username,
                Password = password,
                Birthday = DateOnly.FromDateTime((DateTime)birthday),
                Role = role,
                SucursalId = UserSession.Instance.Employee.SucursalId
            };

            try
            {
                switch (manager.CreateAccount(employee))
                {
                    case 0:
                        _notificationManager.Show("Registrado", "Empleado registrado correctamente", NotificationType.Success, "WindowArea");
                        NavigationService.GoBack();
                        break;

                    case 1:
                        _notificationManager.Show("Error al registrar", "Ocurrió un error de nuestro lado, intente nuevamente", NotificationType.Error, "WindowArea");
                        break;

                    case 2:
                        _notificationManager.Show("Error al registrar", "Existen campos vacíos o con caracteres o formato invalidos", NotificationType.Warning, "WindowArea");
                        break;

                    case 3:
                        _notificationManager.Show("Error al registrar", "Ya existe un trabajador con el nombre de usuario o correo ingresados", NotificationType.Warning, "WindowArea");
                        break;
                }

                saveBtn.IsEnabled = true;
            }
            catch (Exception error)
            {
                _notificationManager.Show("Error al registrar", "Ocurrió un error de nuestro lado, intente nuevamente", NotificationType.Error, "WindowArea");
                saveBtn.IsEnabled = true;
                return;            
            }
        }

        private string GetSelectedRole()
        {
            string role = "";

            if (rbAdmin.IsChecked == true)
            {
                role = "admin";
            }
            else if (rbAnalist.IsChecked == true)
            {
                role = "analist";
            }
            else if (rbAdviser.IsChecked == true)
            {
                role = "adviser";
            }
            else if (rbCollector.IsChecked == true)
            {
                role = "collector";
            }

            return role;
        }

        private void ClearErrorLabels()
        {
            nameField.errorLabel.Visibility = Visibility.Collapsed;
            addressField.errorLabel.Visibility = Visibility.Collapsed;
            phoneField.errorLabel.Visibility = Visibility.Collapsed;
            mailField.errorLabel.Visibility = Visibility.Collapsed;
            userField.errorLabel.Visibility = Visibility.Collapsed;
            pwdErrorLabel.Visibility = Visibility.Collapsed;
            bdayErrorLabel.Visibility = Visibility.Collapsed;
            roleErrorLabel.Visibility = Visibility.Collapsed;
        }

        private bool CheckForEmptyOrWrongFields()
        {
            bool invalidFields = false;
            DateOnly temp;

            if (String.IsNullOrEmpty(nameField.textField.Text.Trim()))
            {
                nameField.showError("Requerido*");
                invalidFields = true;
            }
            if (String.IsNullOrEmpty(addressField.textField.Text.Trim()))
            {
                addressField.showError("Requerido*");
                invalidFields = true;
            }
            if (String.IsNullOrEmpty(phoneField.textField.Text.Trim()))
            {
                phoneField.showError("Requerido*");
                invalidFields = true;
            }
            else if (phoneField.textField.Text.Trim().Length != 10 || !phoneField.textField.Text.Trim().All(char.IsDigit))
            {
                phoneField.showError("El telefono debe constar de 10 números");
                invalidFields = true;
            }
            if (String.IsNullOrEmpty(mailField.textField.Text.Trim()))
            {
                mailField.showError("Requerido*");
                invalidFields = true;
            }
            else if (!new Regex("^[\\w\\-\\.]+@([\\w-]+\\.)+[\\w-]{2,}$").IsMatch(mailField.textField.Text.Trim()))
            {
                mailField.showError("El correo no tiene una estructura valida");
                invalidFields = true;
            }
            if (String.IsNullOrEmpty(userField.textField.Text.Trim()))
            {
                userField.showError("Requerido*");
                invalidFields = true;
            }
            if (String.IsNullOrWhiteSpace(pwdField.Password.Trim()))
            {
                pwdErrorLabel.Content = "Requerido*";
                pwdErrorLabel.Visibility = Visibility.Visible;
                invalidFields = true;
            }
            else if (!new Regex("(?=.*[A-Z])(?=.*[!@#$&*+])(?=.*[0-9])(?=.*[a-z]).{8,}").IsMatch(pwdField.Password.Trim()))
            {
                pwdErrorLabel.Content = "La contraseña no es lo suficientemente fuerte";
                pwdErrorLabel.Visibility = Visibility.Visible;
                invalidFields = true;
            }
            if (birthdayPicker.SelectedDate == null)
            {
                bdayErrorLabel.Content = "Ingrese la fecha de nacimiento del usuario";
                bdayErrorLabel.Visibility = Visibility.Visible;
                invalidFields = true;
            }
            else if (birthdayPicker.SelectedDate == null)
            {
                bdayErrorLabel.Content = "La fecha no es valida";
                bdayErrorLabel.Visibility = Visibility.Visible;
                invalidFields = true;
            }
            else if (birthdayPicker.SelectedDate.Value > DateTime.Now.AddYears(-18))
            {
                bdayErrorLabel.Content = "El trabajador debe ser mayor a 18 años";
                bdayErrorLabel.Visibility = Visibility.Visible;
                invalidFields = true;
            }
            if (GetSelectedRole() == "")
            {
                roleErrorLabel.Content = "Ingrese el rol del usuario";
                roleErrorLabel.Visibility = Visibility.Visible;
                invalidFields = true;
            }

            return invalidFields;
        }
    }
}
