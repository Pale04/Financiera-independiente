using Notification.Wpf;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using DomainClasses;
using System.Windows.Navigation;
using Business_logic;

namespace Financiera_GUI.CustomerManagement
{
    public partial class wCustomerRegistration : Page
    {
        private readonly NotificationManager _notificationManager;

        public wCustomerRegistration()
        {
            _notificationManager = new NotificationManager();
            InitializeComponent();
            CustomizeDatePicker();
        }

        private void CustomizeDatePicker()
        {
            birthdateInput.SelectedDate = DateTime.Now.AddYears(-18);
            birthdateInput.DisplayDateStart = DateTime.Now.AddYears(-100);
            birthdateInput.DisplayDateEnd = DateTime.Now.AddYears(-18);
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void NavigateToBankingInformationTab(object sender, MouseButtonEventArgs e)
        {
            if (!ValidPersonalInformation())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
            }
            else
            {
                PersonalInformationTab.IsEnabled = false;
                BankingInformationTab.IsEnabled = true;
                BankingInformationTab.IsSelected = true;
            }
        }

        private void NavigateToPersonalReferencesTab(object sender, MouseButtonEventArgs e)
        {
            if (!ValidBankingInformation())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
            }
            else if (receiveBankAccountClabe.Text.Length != 18)
            {
                _notificationManager.Show("La CLABE de la cuenta de depósito debe tener 18 dígitos", NotificationType.Warning, "WindowArea");
            }
            else if (collectBankAccountClabe.Text.Length != 18)
            {
                _notificationManager.Show("La CLABE de la cuenta de cobro debe tener 18 dígitos", NotificationType.Warning, "WindowArea");
            }
            else
            {
                BankingInformationTab.IsEnabled = false;
                PersonalReferencesTab.IsEnabled = true;
                PersonalReferencesTab.IsSelected = true;
            }
        }

        private void BackToPersonalInformationTab(object sender, MouseButtonEventArgs e)
        {
            BankingInformationTab.IsEnabled = false;
            PersonalInformationTab.IsEnabled = true;
            PersonalInformationTab.IsSelected = true;
        }

        private void BackToBankingInformationTab(object sender, MouseButtonEventArgs e)
        {
            PersonalReferencesTab.IsEnabled = false;
            BankingInformationTab.IsEnabled = true;
            BankingInformationTab.IsSelected = true;
        }

        private void RegisterCustomer(object sender, MouseButtonEventArgs e)
        {
            if (!ValidPersonalReferences())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
                return;
            }

            CustomerManager customerManager = new CustomerManager();
            Customer newCustomer = CollectFields();
            try
            {
                customerManager.AddCustomer(newCustomer);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                return;
            }

            _notificationManager.Show(NotificationMessages.GlobalRegistrationSuccess, NotificationType.Success, "WindowArea");
            RebootFields();
        }

        private Customer CollectFields()
        {
            return new Customer()
            {
                Rfc = rfcInput.Text,
                BirthDate = DateOnly.FromDateTime(birthdateInput.SelectedDate.Value),
                Name = nameInput.Text,
                HouseAddress = houseAddressInput.Text,
                WorkAddress = workAddressInput.Text,
                PhoneNumber1 = phoneNumber1Input.Text,
                PhoneNumber2 = phoneNumber2Input.Text,
                Email = emailInput.Text,
                BankAccounts = [
                    new BankAccount()
                    {
                        Clabe = receiveBankAccountClabe.Text,
                        Purpose = "receive"
                    },
                    new BankAccount()
                    {
                        Clabe = collectBankAccountClabe.Text,
                        Purpose = "collect"
                    }
                ],
                PersonalReferences = [
                    new PersonalReference()
                    {
                        Name = personalReference1NameInput.Text,
                        PhoneNumber = personalReference1PohoneNumber1Input.Text,
                        Relationship = personalReference1RelationshipInput.Text
                    },
                    new PersonalReference()
                    {
                        Name = personalReference2NameInput.Text,
                        PhoneNumber = personalReference2PohoneNumber1Input.Text,
                        Relationship = personalReference2RelationshipInput.Text
                    }
                ]
            };
        }

        private bool ValidPersonalInformation()
        {
            return !string.IsNullOrWhiteSpace(rfcInput.Text) &&
                   !string.IsNullOrWhiteSpace(nameInput.Text) &&
                   !string.IsNullOrWhiteSpace(houseAddressInput.Text) &&
                   !string.IsNullOrWhiteSpace(workAddressInput.Text) &&
                   !string.IsNullOrWhiteSpace(phoneNumber1Input.Text) &&
                   !string.IsNullOrWhiteSpace(phoneNumber2Input.Text) &&
                   !string.IsNullOrWhiteSpace(emailInput.Text);
        }

        private bool ValidBankingInformation()
        {
            return !string.IsNullOrWhiteSpace(receiveBankAccountClabe.Text) &&
                   !string.IsNullOrWhiteSpace(collectBankAccountClabe.Text);
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

        private void PreviewNumberInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private void RebootFields()
        {
            rfcInput.Text = string.Empty;
            nameInput.Text = string.Empty;
            birthdateInput.SelectedDate = DateTime.Now.AddYears(-18);
            houseAddressInput.Text = string.Empty;
            workAddressInput.Text = string.Empty;
            phoneNumber1Input.Text = string.Empty;
            phoneNumber2Input.Text = string.Empty;
            emailInput.Text = string.Empty;
            receiveBankAccountClabe.Text = string.Empty;
            collectBankAccountClabe.Text = string.Empty;
            personalReference1NameInput.Text = string.Empty;
            personalReference1PohoneNumber1Input.Text = string.Empty;
            personalReference1RelationshipInput.Text = string.Empty;
            personalReference2NameInput.Text = string.Empty;
            personalReference2PohoneNumber1Input.Text = string.Empty;
            personalReference2RelationshipInput.Text = string.Empty;
            PersonalInformationTab.IsEnabled = true;
            PersonalInformationTab.IsSelected = true;
            PersonalReferencesTab.IsEnabled = false;
        }
    }
}
