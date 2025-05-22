using Business_logic;
using DomainClasses;
using Financiera_GUI.Utilities;
using Notification.Wpf;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Financiera_GUI.Credit
{
    public partial class wCreditApplications : Page
    {
        private List<CreditRequestSummary>? _requests;
        NotificationManager _notificationManager = new NotificationManager();

        private int _currentPage = 1;
        private readonly int PAGE_SIZE = 12;

        public wCreditApplications()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            CreditManager manager = new();

            try
            {
                _requests = manager.GetCreditRequests();
            }
            catch (CommunicationException error)
            {
                _notificationManager.Show("No se pudieron recuperar las solicitudes de crédito", NotificationType.Error);
            }

            if (_requests == null)
            {
                _notificationManager.Show("No se pudieron recuperar las solicitudes de crédito", NotificationType.Error);
                return;
            }

            UpdateList();
        }

        private void UpdateList()
        {
            requestsPanel.Children.RemoveRange(1, requestsPanel.Children.Count);
            for (int i = (_currentPage * PAGE_SIZE) - PAGE_SIZE; i < int.Min(_requests.Count, _currentPage * PAGE_SIZE); i++)
            {
                requestsPanel.Children.Add(new ucCreditRequestRow(_requests[i]));
            }

            if (_currentPage * PAGE_SIZE > _requests.Count)
            {
                nextPageButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                nextPageButton.Visibility = Visibility.Visible;
            }

            if (_currentPage == 1)
            {
                previousPageButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                previousPageButton.Visibility = Visibility.Visible;
            }
        }

        private void ApplyFilter(object sender, RoutedEventArgs e)
        {

        }

        private void NextPage(object sender, MouseButtonEventArgs e)
        {
            _currentPage++;
            UpdateList();
        }

        private void PreviousPage(object sender, MouseButtonEventArgs e)
        {
            _currentPage--;
            UpdateList();
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();

        }

        private void ViewCreditInfo(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new wCreditDetails(((ucCreditRequestRow)sender).CreditId));
        }
    }
}
