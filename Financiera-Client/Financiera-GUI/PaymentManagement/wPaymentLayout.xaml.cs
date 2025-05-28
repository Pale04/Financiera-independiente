using System.Windows.Controls;
using Business_logic.Payments;
using DomainClasses;
using Financiera_GUI.Utilities;
using Notification.Wpf;

namespace Financiera_GUI.PaymentManagement
{
    public partial class wPaymentLayout : Page
    {
        private readonly NotificationManager _notificationManager;
        private DateOnly _startDate;
        private DateOnly _endDate;
        private List<PaymentLayout> _paymentLayout;

        public wPaymentLayout()
        {
            InitializeComponent();

            _notificationManager = new NotificationManager();
            _startDate = DateOnly.FromDateTime(DateTime.Now);
            _endDate = DateOnly.FromDateTime(DateTime.Now);

            startDateInput.SelectedDate = DateTime.Now;
            startDateInput.DisplayDateStart = DateTime.Now;
            startDateInput.DisplayDateEnd = DateTime.Now.AddDays(35);

            endDateInput.SelectedDate = DateTime.Now;
            endDateInput.DisplayDateStart = DateTime.Now;
            endDateInput.DisplayDateEnd = DateTime.Now.AddDays(35);

            LoadLayout();
        }

        private void LoadLayout()
        {
            if (_startDate.CompareTo(_endDate) > 0)
            {
                _notificationManager.Show("El rango de fechas seleccionado es inválido", NotificationType.Warning, "WindowArea");
                return;
            }

            PaymentManager paymentManager = new PaymentManager();

            try
            {
                _paymentLayout = paymentManager.GetPaymentLayout(_startDate, _endDate);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Error, "WindowArea");
                return;
            }

            if (_paymentLayout.Count == 0)
            {
                _notificationManager.Show("No hay pagos pendientes por mostrar", NotificationType.Information, "WindowArea");
                downloadLayoutButton.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                ChargePaymentsTable();
                downloadLayoutButton.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ChargePaymentsTable()
        {
            paymentsTable.Children.Clear();
            foreach (PaymentLayout payment in _paymentLayout)
            {
                paymentsTable.Children.Add(new ucPaymentLayoutRow(payment));
            }
        }

        private void GeneratePaymentLayout(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoadLayout();
        }

        private void DownloadPaymentLayout(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PaymentManager paymentManager = new();
            string layoutPath = string.Empty;

            try
            {
                layoutPath = paymentManager.GeneratePaymentLayoutCsv(_paymentLayout, _startDate, _endDate);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
            }

            if (!string.IsNullOrEmpty(layoutPath))
            {
                _notificationManager.Show(NotificationMessages.PaymentLayoutGenerationSuccessful, NotificationType.Success, "WindowArea");
            }
            else
            {
                _notificationManager.Show(NotificationMessages.PaymentLayoutGenerationFailed, NotificationType.Error, "WindowArea");
            }
        }

        private void Back(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void LimitEndDateInput(object sender, SelectionChangedEventArgs e)
        {
            _startDate = DateOnly.FromDateTime(startDateInput.SelectedDate ?? DateTime.Now);
        }

        private void LimitStartDateInput(object sender, SelectionChangedEventArgs e)
        {
            _endDate = DateOnly.FromDateTime(endDateInput.SelectedDate ?? DateTime.Now);
        }
    }
}
