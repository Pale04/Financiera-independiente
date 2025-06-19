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
        private List<CreditRequestSummary> _requests;
        private List<CreditRequestSummary> _shownRequests;
        NotificationManager _notificationManager = new NotificationManager();

        private int _currentPage = 1;
        private static int PAGE_SIZE = 12;

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

            _shownRequests = _requests;
            UpdateList();
        }

        private void UpdateList()
        {
            requestsPanel.Children.RemoveRange(1, requestsPanel.Children.Count);
            for (int i = (_currentPage * PAGE_SIZE) - PAGE_SIZE; i < int.Min(_shownRequests.Count, _currentPage * PAGE_SIZE); i++)
            {
                requestsPanel.Children.Add(new ucCreditRequestRow(_shownRequests[i]));
            }

            if (_currentPage * PAGE_SIZE < _shownRequests.Count)
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
            (int capitalOpt, int durationOpt) = GetFilters();

            switch (capitalOpt) 
            {
                case 0:
                    _shownRequests = _requests;
                    break;
                case 1:
                    _shownRequests = _requests.Where(item => item.Capital <= 25000).ToList();
                    break;
                case 2:
                    _shownRequests = _requests.Where(item => item.Capital > 25000 && item.Capital <= 50000).ToList();
                    break;
                case 3:
                    _shownRequests = _requests.Where(item => item.Capital > 50000 && item.Capital <= 75000).ToList();
                    break;
                case 4:
                    _shownRequests = _requests.Where(item => item.Capital > 75000 && item.Capital <= 100000).ToList();
                    break;
                case 5:
                    _shownRequests = _requests.Where(item => item.Capital > 100000).ToList();
                    break;
            }

            switch (durationOpt)
            {
                case 1:
                    _shownRequests = _shownRequests.Where(item => item.Duration < 12).ToList();
                    break;
                case 2:
                    _shownRequests = _shownRequests.Where(item => item.Duration >= 12 && item.Duration <= 36).ToList();
                    break;
                case 3:
                    _shownRequests = _shownRequests.Where(item => item.Duration > 36).ToList();
                    break;
            }

            UpdateList();
        }

        private (short, short) GetFilters()
        {
            short capitalOpt = 0;

            if (rdCapital1.IsChecked == true)
            {
                capitalOpt = 1;
            }
            else if (rdCapital2.IsChecked == true)
            {
                capitalOpt = 2;
            }
            else if (rdCapital3.IsChecked == true)
            {
                capitalOpt = 3;
            }
            else if (rdCapital4.IsChecked == true)
            {
                capitalOpt = 4;
            }
            else if (rdCapital5.IsChecked == true)
            {
                capitalOpt = 5;
            }

            short durationOpt = 0;

            if (rdDuration1.IsChecked == true)
            {
                durationOpt = 1;
            }
            else if (rdDuration2.IsChecked == true)
            {
                durationOpt = 2;
            }
            else if (rdDuration3.IsChecked == true)
            {
                durationOpt = 3;
            }

            return (capitalOpt, durationOpt);
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

        private void ResetFilter(object sender, RoutedEventArgs e)
        {
            _shownRequests = _requests;
            rdCapital1.IsChecked = false;
            rdCapital2.IsChecked = false;
            rdCapital3.IsChecked = false;
            rdCapital4.IsChecked = false;
            rdCapital5.IsChecked = false;
            rdDuration1.IsChecked = false;
            rdDuration2.IsChecked = false;
            rdDuration3.IsChecked = false;

            UpdateList();
        }
    }
}