using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using DomainClasses;
using Notification.Wpf;
using Business_logic;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Media;

namespace Financiera_GUI.CustomerManagement
{
    public partial class wCustomerInfo : Page
    {
        private Customer _customer;
        private readonly NotificationManager _notificationManager;
        private wCustomerManagement _previousReference;

        public wCustomerInfo(string rfc, wCustomerManagement previousReference)
        {
            InitializeComponent();
            _notificationManager = new NotificationManager();
            _previousReference = previousReference;
            LoadCustomerInformation(rfc);
            CustomizeDatePicker();
        }

        private void CustomizeDatePicker()
        {
            birthdateInput.DisplayDateStart = DateTime.Now.AddYears(-100);
            birthdateInput.DisplayDateEnd = DateTime.Now.AddYears(-18);
        }

        private void LoadCustomerInformation(string rfc)
        {
            CustomerManager customerManager = new();

            try
            {
                _customer = customerManager.GetByRfc(rfc);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                return;
            }

            rfcLabel.Content = _customer.Rfc;
            nameInput.Text = _customer.Name;
            birthdateInput.SelectedDate = _customer.BirthDate.ToDateTime(TimeOnly.MinValue);
            houseAddressInput.Text = _customer.HouseAddress;
            workAddressInput.Text = _customer.WorkAddress;
            phoneNumber1Input.Text = _customer.PhoneNumber1;
            phoneNumber2Input.Text = _customer.PhoneNumber2;
            emailInput.Text = _customer.Email;
            receiveBankAccountClabe.Text = _customer.BankAccounts[0].Purpose == "receive" ? _customer.BankAccounts[0].Clabe : _customer.BankAccounts[1].Clabe;
            collectBankAccountClabe.Text = _customer.BankAccounts[0].Purpose == "receive" ? _customer.BankAccounts[0].Clabe : _customer.BankAccounts[1].Clabe;
            personalReference1NameInput.Text = _customer.PersonalReferences[0].Name;
            personalReference1PohoneNumber1Input.Text = _customer.PersonalReferences[0].PhoneNumber;
            personalReference1RelationshipInput.Text = _customer.PersonalReferences[0].Relationship;
            personalReference2NameInput.Text = _customer.PersonalReferences[1].Name;
            personalReference2PohoneNumber1Input.Text = _customer.PersonalReferences[1].PhoneNumber;
            personalReference2RelationshipInput.Text = _customer.PersonalReferences[1].Relationship;
            LoadAccountStateInformation();
        }

        private void LoadAccountStateInformation()
        {
            stateLabel.Content = _customer.State ? "Activo" : "Desactivado";
            stateLabel.Foreground = _customer.State ? Brushes.Green : Brushes.Red;
            deactivateAccountButton.Visibility = _customer.State ? Visibility.Visible : Visibility.Collapsed;
            activateAccountButton.Visibility = _customer.State ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            _previousReference.Reload();
            NavigationService.GoBack();
        }

        private void PreviewNumberInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private void UpdatePersonalInformation(object sender, MouseButtonEventArgs e)
        {
            ConvertPersonalInformationFields(true);
        }

        private void ConvertPersonalInformationFields(bool enable)
        {
            nameInput.IsReadOnly = !enable;
            birthdateInput.IsEnabled = enable;
            houseAddressInput.IsReadOnly = !enable;
            workAddressInput.IsReadOnly = !enable;
            phoneNumber1Input.IsReadOnly = !enable;
            phoneNumber2Input.IsReadOnly = !enable;
            emailInput.IsReadOnly = !enable;

            if (enable)
            {
                _notificationManager.Show("Campos habilitados para edición", NotificationType.Information, "WindowArea");
            }
            savePersonalInformationButton.Visibility = enable ? Visibility.Visible : Visibility.Hidden;
            updatePersonalInformationButton.Visibility = enable ? Visibility.Hidden : Visibility.Visible;
        }

        private void SavePersonalInformationChanges(object sender, MouseButtonEventArgs e)
        {
            if (!ValidPersonalInformation())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
                return;
            }
            else if (PersonalInformationFieldsChanged())
            {
                Customer updatedCustomer = new()
                {
                    Rfc = _customer.Rfc,
                    Name = nameInput.Text,
                    BirthDate = DateOnly.FromDateTime(birthdateInput.SelectedDate.Value),
                    HouseAddress = houseAddressInput.Text,
                    WorkAddress = workAddressInput.Text,
                    PhoneNumber1 = phoneNumber1Input.Text,
                    PhoneNumber2 = phoneNumber2Input.Text,
                    Email = emailInput.Text
                };
                CustomerManager customerManager = new();

                try
                {
                    customerManager.UpdateCustomerPersonalInformation(updatedCustomer);
                }
                catch (Exception error)
                {
                    _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                    return;
                }

                _notificationManager.Show(NotificationMessages.GlobalUpdateSuccess, NotificationType.Success, "WindowArea");
            }
            ConvertPersonalInformationFields(false);
        }

        private bool ValidPersonalInformation()
        {
            return !string.IsNullOrWhiteSpace(nameInput.Text) &&
                   !string.IsNullOrWhiteSpace(houseAddressInput.Text) &&
                   !string.IsNullOrWhiteSpace(workAddressInput.Text) &&
                   !string.IsNullOrWhiteSpace(phoneNumber1Input.Text) &&
                   !string.IsNullOrWhiteSpace(phoneNumber2Input.Text) &&
                   !string.IsNullOrWhiteSpace(emailInput.Text);
        }

        private bool PersonalInformationFieldsChanged()
        {
            return nameInput.Text != _customer.Name||
                   birthdateInput.SelectedDate != _customer.BirthDate.ToDateTime(TimeOnly.MinValue) ||
                   houseAddressInput.Text != _customer.HouseAddress ||
                   workAddressInput.Text != _customer.WorkAddress ||
                   phoneNumber1Input.Text != _customer.PhoneNumber1 ||
                   phoneNumber2Input.Text != _customer.PhoneNumber2 ||
                   emailInput.Text != _customer.Email;
        }

        private void UpdateBankingInformation(object sender, MouseButtonEventArgs e)
        {
            ConvertBankingInformationFields(true);
        }

        private void ConvertBankingInformationFields(bool enable)
        {
            receiveBankAccountClabe.IsReadOnly = !enable;
            collectBankAccountClabe.IsReadOnly = !enable;

            if (enable)
            {
                _notificationManager.Show("Campos habilitados para edición", NotificationType.Information, "WindowArea");
            }
            saveBankingInformationButton.Visibility = enable ? Visibility.Visible : Visibility.Hidden;
            updateBankingInformationButton.Visibility = enable ? Visibility.Hidden : Visibility.Visible;
        }

        private void SaveBankingInformationChanges(object sender, MouseButtonEventArgs e)
        {
            if (!ValidBankingInformation())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
                return;
            }

            foreach (BankAccount bankAccount in _customer.BankAccounts)
            {
                TextBox textBox = bankAccount.Purpose == "receive" ? receiveBankAccountClabe : collectBankAccountClabe;
                if (textBox.Text != bankAccount.Clabe)
                {
                    BankAccount updatedAccount = new()
                    {
                        Id = bankAccount.Id,
                        Clabe = textBox.Text,
                        Purpose = bankAccount.Purpose
                    };
                    CustomerManager customerManager = new();

                    try
                    {
                        customerManager.UpdateCustomerBankAccount(updatedAccount);
                    }
                    catch (Exception error)
                    {
                        _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                        return;
                    }

                    _notificationManager.Show(NotificationMessages.GlobalUpdateSuccess, NotificationType.Success, "WindowArea");
                }
            }

            ConvertBankingInformationFields(false);
        }

        private bool ValidBankingInformation()
        {
            return !string.IsNullOrWhiteSpace(receiveBankAccountClabe.Text) &&
                   !string.IsNullOrWhiteSpace(collectBankAccountClabe.Text);
        }

        private void UpdatePersonalReferences(object sender, MouseButtonEventArgs e)
        {
            ConvertPersonalReferencesFields(true);
        }

        private void ConvertPersonalReferencesFields(bool enable)
        {
            personalReference1NameInput.IsReadOnly = !enable;
            personalReference1PohoneNumber1Input.IsReadOnly = !enable;
            personalReference1RelationshipInput.IsReadOnly = !enable;
            personalReference2NameInput.IsReadOnly = !enable;
            personalReference2PohoneNumber1Input.IsReadOnly = !enable;
            personalReference2RelationshipInput.IsReadOnly = !enable;

            if (enable)
            {
                _notificationManager.Show("Campos habilitados para edición", NotificationType.Information, "WindowArea");
            }
            savePersonalReferencesButton.Visibility = enable ? Visibility.Visible : Visibility.Hidden;
            updatePersonalReferencesButton.Visibility = enable ? Visibility.Hidden : Visibility.Visible;
        }

        private void SavePersonalReferencesChanges(object sender, MouseButtonEventArgs e)
        {
            if (!ValidPersonalReferences())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
                return;
            }

            for (int i = 0; i < _customer.PersonalReferences.Count(); i++)
            {
                if (PersonalReferencesFieldsChanged(i))
                {
                    SaveAnyPersonalReference(i);
                }
            }
            _notificationManager.Show(NotificationMessages.GlobalUpdateSuccess, NotificationType.Success, "WindowArea");
            ConvertPersonalReferencesFields(false);
        }

        private void SaveAnyPersonalReference(int personalReferenceNumber)
        {
            TextBox nameTexBox = personalReferenceNumber == 0 ? personalReference1NameInput : personalReference2NameInput;
            TextBox phoneTextBox = personalReferenceNumber == 0 ? personalReference1PohoneNumber1Input : personalReference2PohoneNumber1Input;
            TextBox relationshipTextBox = personalReferenceNumber == 0 ? personalReference1RelationshipInput : personalReference2RelationshipInput;

            PersonalReference updatedPersonalReference = new()
            {
                Id = _customer.PersonalReferences[personalReferenceNumber].Id,
                Name = nameTexBox.Text,
                PhoneNumber = phoneTextBox.Text,
                Relationship = relationshipTextBox.Text
            };
            CustomerManager customerManager = new();

            try
            {
                customerManager.UpdateCustomerPersonalReference(updatedPersonalReference);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                return;
            }
        }

        private bool ValidPersonalReferences()
        {
            return !string.IsNullOrWhiteSpace(personalReference1NameInput.Text) &&
                   !string.IsNullOrWhiteSpace(personalReference1PohoneNumber1Input.Text) &&
                   !string.IsNullOrWhiteSpace(personalReference1RelationshipInput.Text) &&
                   !string.IsNullOrWhiteSpace(personalReference2NameInput.Text) &&
                   !string.IsNullOrWhiteSpace(personalReference2PohoneNumber1Input.Text) &&
                   !string.IsNullOrWhiteSpace(personalReference2RelationshipInput.Text);
        }

        private bool PersonalReferencesFieldsChanged(int personalReferenceNumber)
        {
            TextBox nameTexBox = personalReferenceNumber == 0 ? personalReference1NameInput : personalReference2NameInput;
            TextBox phoneTextBox = personalReferenceNumber == 0 ? personalReference1PohoneNumber1Input : personalReference2PohoneNumber1Input;
            TextBox relationshipTextBox = personalReferenceNumber == 0 ? personalReference1RelationshipInput : personalReference2RelationshipInput;

            return nameTexBox.Text != _customer.PersonalReferences[personalReferenceNumber].Name ||
                   phoneTextBox.Text != _customer.PersonalReferences[personalReferenceNumber].PhoneNumber ||
                   relationshipTextBox.Text != _customer.PersonalReferences[personalReferenceNumber].Relationship;
        }

        private void DeactivateAccount(object sender, MouseButtonEventArgs e)
        {
            string action = _customer.State ? "desactivar" : "activar";
            MessageBoxResult confirmation = MessageBox.Show($"¿Está seguro de {action} la cuenta del cliente?", "Confirmar actualización de estado de cuenta", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
            if (confirmation == MessageBoxResult.Yes)
            {
                CustomerManager customerManager = new();
                try
                {
                    customerManager.UpdateCustomerState(_customer.Rfc, !_customer.State);
                }
                catch (Exception error)
                {
                    _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                    return;
                }
                _customer.State = !_customer.State;
                LoadAccountStateInformation();
                _notificationManager.Show(NotificationMessages.GlobalStatusUpdateSuccess, NotificationType.Success, "WindowArea");
            }
        }
    }
}
