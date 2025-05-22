using CatalogServiceReference;
using DomainClasses;
using Notification.Wpf;
using System.Windows;
using System.Windows.Controls;
using Financiera_GUI.Utilities;

using Business_logic.Catalogs;
using Business_logic;
using System.ServiceModel;

namespace Financiera_GUI.CatalogManagement
{
    /// <summary>
    /// Lógica de interacción para wCreditPoliciesManagment.xaml
    /// </summary>
    public partial class wCreditPoliciesManagment : Page
    {
        private int _firstId;
        private int _lastId;
        private int _pageSize;
        private int _actualPage;
        private List<Policy> _policiesNextPage;
        private readonly NotificationManager _notificationManager;

        public wCreditPoliciesManagment()
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
            _policiesNextPage = new();

            SaveNextPage();
            LoadPage(true);
        }

        public void SaveNextPage()
        {
            CreditPolicyManager manager = new();
            List<Policy> creditPolicies = new();

            try{
                creditPolicies = manager.GetPoliciesByPagination(_pageSize, _lastId, true);
            }
            catch (Exception error)
            {
                _notificationManager.Show("Error al cargar las políticas de la base de datos", NotificationType.Error, "WindowArea");
                return;
            }

            _policiesNextPage.Clear();
            foreach (var policy in creditPolicies)
            {
                _policiesNextPage.Add(policy);
            }

            if(_actualPage > 0)
            {
                nextPageButton.Visibility = _policiesNextPage.Count == 0 ? Visibility.Hidden : Visibility.Visible;

            }

        }

        private void LoadPage(bool next)
        {
            CreditPolicyManager manager = new();
            List<Policy> creditPolicies = new();

            if (next)
            {
                creditPolicies = _policiesNextPage;
            }
            else
            {
                try
                {
                    creditPolicies = manager.GetPoliciesByPagination(_pageSize, _firstId, false);
                }
                catch (Exception error)
                {
                    _notificationManager.Show($"Error al cargar las políticas en la tabla: {error.Message}", NotificationType.Error, "WindowArea");
                    return;
                }
            }
            TbCreditPolicies.Children.RemoveRange(1, TbCreditPolicies.Children.Count);
            foreach (var policy in creditPolicies)
            {
                TbCreditPolicies.Children.Add(new wCreditPolicyManagmentRow(policy));
            }

            _actualPage += next ? 1 : -1;
            _firstId = creditPolicies.FirstOrDefault()?.Id ?? _firstId;
            _lastId = creditPolicies.LastOrDefault()?.Id ?? _lastId;
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

        private void Register(object sender, RoutedEventArgs e)
        {
            if (!IsValid())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
            }
            else
            {
                Policy newPolicy = new()
                {
                    Title = TbTitle.Text,
                    Description = TbDescription.Text,
                    Registrer = UserSession.Instance.Employee.Id,
                    State = true
                };
                CreditPolicyManager manager = new();
                try
                {
                    int response = new();
                    response = manager.AddPolicy(newPolicy);

                    switch (response)
                    {
                        case 1:
                            _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
                            break;
                        case 2:
                            _notificationManager.Show(NotificationMessages.GlobalInvalidFields, NotificationType.Warning, "WindowArea");
                            break;
                        case 3:
                            _notificationManager.Show(NotificationMessages.PolicyDuplicated, NotificationType.Warning, "WindowArea");
                            break;
                        case 4:
                            _notificationManager.Show(NotificationMessages.PolicyNotFound, NotificationType.Error, "WindowArea");
                            break;
                        case 0:
                            _notificationManager.Show(NotificationMessages.GlobalRegistrationSuccess, NotificationType.Success, "WindowArea");
                            break;
                    }
                    Clear();
                    RebootPages();
                }
                catch (CommunicationException error)
                {
                    _notificationManager.Show($"Error: {error.Message}", NotificationType.Error, "WindowArea");
                }
            }
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(TbTitle.Text) && !string.IsNullOrWhiteSpace(TbDescription.Text);
        }

        private void Clear()
        {
            TbTitle.Clear();
            TbDescription.Clear();
        }
    }
}
