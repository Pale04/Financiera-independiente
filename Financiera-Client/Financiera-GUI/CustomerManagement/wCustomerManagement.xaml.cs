using System.Windows.Controls;
using Business_logic;
using DomainClasses;
using Notification.Wpf;
using System.Windows;
using Financiera_GUI.Utilities;
using System.Windows.Navigation;

namespace Financiera_GUI.CustomerManagement
{
    public partial class wCustomerManagement : Page
    {
        private string _lastRfc;
        private string _firstRfc;
        private int _pageSize;
        private int _actualPage;
        private List<Customer> _nextPage;
        private readonly NotificationManager _notificationManager;

        public wCustomerManagement()
        {
            InitializeComponent();
            _notificationManager = new NotificationManager();
            Reload();
        }

        public void Reload()
        {
            _pageSize = 12;
            _actualPage = 0;
            _lastRfc = "0";
            _firstRfc = "0";
            _nextPage = new();
            searchInput.Text = string.Empty;
            SaveNextPage();
            LoadPage(true);
        }

        private void SaveNextPage()
        {
            CustomerManager customerManager = new();
            List<Customer> customers = new();

            try
            {
                customers = customerManager.GetByPagination(_pageSize, _lastRfc, true);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Error, "WindowArea");
                return;
            }

            _nextPage.Clear();
            foreach (Customer customer in customers)
            {
                _nextPage.Add(customer);
            }

            if (_actualPage > 0)
            {
                nextPageButton.Visibility = _nextPage.Count == 0 ? Visibility.Hidden : Visibility.Visible;
            }
        }

        private void LoadPage(bool next)
        {
            List<Customer> customers = new();

            if (next)
            {
                customers = _nextPage;
            }
            else
            {
                CustomerManager customerManager = new();
                try
                {
                    customers = customerManager.GetByPagination(_pageSize, _firstRfc, false);
                }
                catch (Exception error)
                {
                    _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                }
            }

            customerTable.Children.RemoveRange(1, customerTable.Children.Count);
            foreach (Customer customer in customers)
            {
                customerTable.Children.Add(new ucCustomerManagement(customer, this));
            }

            _actualPage += next ? 1 : -1;
            _firstRfc = customers.FirstOrDefault()?.Rfc ?? _firstRfc;
            _lastRfc = customers.LastOrDefault()?.Rfc ?? _lastRfc;
            SaveNextPage();

            previousPageButton.Visibility = _actualPage == 1 ? Visibility.Hidden : Visibility.Visible;
        }

        private void SearchClient(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CustomerManager customerManager = new();
            Customer customer; 

            try
            {
                customer = customerManager.GetByRfc(searchInput.Text);
            }
            catch (Exception error)
            {
                NotificationType notificationType = error.Message.Equals("4") ? NotificationType.Information : NotificationType.Warning;
                string message = error.Message.Equals("4") ? NotificationMessages.SearchNotFound : error.Message;
                _notificationManager.Show(message, notificationType, "WindowArea");
                return;
            }

            customerTable.Children.RemoveRange(1, customerTable.Children.Count);
            customerTable.Children.Add(new ucCustomerManagement(customer, this));
        }

        private void Back(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void RegisterCustomer(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new wCustomerRegistration());
        }

        private void PreviousPage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoadPage(false);
        }

        private void NextPage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoadPage(true);
        }
    }
}
