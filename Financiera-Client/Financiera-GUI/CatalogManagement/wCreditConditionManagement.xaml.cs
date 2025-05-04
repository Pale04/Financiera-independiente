using Business_logic.Catalogs;
using DomainClasses;
using Notification.Wpf;
using System.Windows.Controls;
using System.Windows;
using Financiera_GUI.Utilities;
using Business_logic;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Financiera_GUI.CatalogManagement
{
    public partial class wCreditConditionManagement : Page
    {
        private int _lastId;
        private int _firstId;
        private int _pageSize;
        private int _actualPage;
        private List<CreditCondition> _nextPage;
        private readonly NotificationManager _notificationManager;

        public wCreditConditionManagement()
        {
            InitializeComponent();
            _notificationManager = new NotificationManager();

            _pageSize = 12;
            RebootPages();
        }

        private void RebootPages()
        {
            _actualPage = 0;
            _lastId = 0;
            _firstId = 0;
            _nextPage = new();

            SaveNextPage();
            LoadPage(true);
        }

        private void SaveNextPage()
        {
            CreditConditionManager creditConditionManager = new();
            List<CreditCondition> creditConditions = new();

            try
            {
                creditConditions = creditConditionManager.GetByPagination(_pageSize, _lastId, true);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Error, "WindowArea");
                return;
            }

            _nextPage.Clear();
            foreach (CreditCondition creditCondition in creditConditions)
            {
                _nextPage.Add(creditCondition);
            }

            if (_actualPage > 0)
            {
                nextPageButton.Visibility = _nextPage.Count == 0 ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private void LoadPage(bool next)
        {
            CreditConditionManager creditConditionManager = new();
            List<CreditCondition> creditConditions = new();

            if (next)
            {
                creditConditions = _nextPage;
            }
            else
            {
                try
                {
                    creditConditions = creditConditionManager.GetByPagination(_pageSize, _firstId, false);
                }
                catch (Exception error)
                {
                    _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                }
            }

            creditConditionsTable.Children.RemoveRange(1, creditConditionsTable.Children.Count);
            foreach(CreditCondition creditCondition in creditConditions)
            {
                creditConditionsTable.Children.Add(new wCreditConditionManagementRow(creditCondition));
            }

            _actualPage += next ? 1 : -1;
            _firstId = creditConditions.FirstOrDefault()?.Id ?? _firstId;
            _lastId = creditConditions.LastOrDefault()?.Id ?? _lastId;
            SaveNextPage();

            previousPageButton.Visibility = _actualPage == 1 ? Visibility.Hidden : Visibility.Visible;
        }

        public void NextPage(object sender, RoutedEventArgs e)
        {
            LoadPage(true);
        }

        public void PreviousPage(object sender, RoutedEventArgs e)
        {
            LoadPage(false);
        }
        public void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        public void Register(object sender, RoutedEventArgs e)
        {
            if (!ValidFields())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
                return;
            }

            int paymentsPerMonth = paymentsPerMonthInput.SelectedIndex switch
            {
                0 => 4,
                1 => 2,
                _ => 1
            };

            int interestRate;
            int iva;
            try
            {
                interestRate = int.Parse(interestRateInput.Text);
                iva = int.Parse(ivaInput.Text);
            }
            catch(FormatException)
            {
                _notificationManager.Show("La tasa de interés y el IVA deben ser números enteros", NotificationType.Warning, "WindowArea");
                return;
            }


            CreditConditionManager creditConditionManager = new();
            try
            {
                creditConditionManager.AddCreditCondition(new CreditCondition
                {
                    PaymentsPerMonth = paymentsPerMonth,
                    InterestRate = interestRate,
                    IVA = iva,
                    RegistrerId = UserSession.Instance.Employee.id
                });
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                return;
            }

            _notificationManager.Show(NotificationMessages.GlobalRegistrationSuccess, NotificationType.Success, "WindowArea");
            interestRateInput.Text = string.Empty;
            RebootPages();
        }

        private bool ValidFields()
        {
            return !string.IsNullOrEmpty((string)paymentsPerMonthInput.SelectionBoxItem) &&
                   !string.IsNullOrWhiteSpace(interestRateInput.Text) &&
                   !string.IsNullOrWhiteSpace(ivaInput.Text);
        }

        private void PreviewNumberInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
    }
}
