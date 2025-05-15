using System.Windows.Controls;
using Business_logic;
using DomainClasses;
using Financiera_GUI.Utilities;
using Notification.Wpf;

namespace Financiera_GUI.PaymentManagement
{
    public partial class wPaymentLayout : Page
    {
        private readonly NotificationManager _notificationManager;
        private DateOnly _actualDate;
        private DateOnly _endDate;
        private List<PaymentLayout> _paymentLayout;

        public wPaymentLayout()
        {
            InitializeComponent();
            _notificationManager = new NotificationManager();
            _actualDate = DateOnly.FromDateTime(DateTime.Now);

            endDateInput.SelectedDate = DateTime.Now;
            endDateInput.DisplayDateStart = DateTime.Now;
            endDateInput.DisplayDateEnd = DateTime.Now.AddMonths(1);

            LoadLayout();
        }

        private void LoadLayout()
        {
            PaymentManager paymentManager = new PaymentManager();
            _endDate = DateOnly.FromDateTime(endDateInput.SelectedDate.Value);

            try
            {
                _paymentLayout = paymentManager.GetPaymentLayout(_actualDate, _endDate);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Error, "WindowArea");
                return;
            }

            if (_paymentLayout.Count == 0)
            {
                _notificationManager.Show("No hay pagos pendientes por mostrar", NotificationType.Information, "WindowArea");
            }
            else
            {
                ChargePaymentsTable();
            }

            if (_endDate != _actualDate)
            {
                layoutDatesHelpLabel.Content = $"Pagos de hoy hata el {_endDate:dd/MM/yyyy}";
            }
            else
            {
                layoutDatesHelpLabel.Content = $"Pagos de hoy";
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
                layoutPath = paymentManager.GeneratePaymentLayoutCsv(_paymentLayout, _actualDate, _endDate);
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
    }
}
