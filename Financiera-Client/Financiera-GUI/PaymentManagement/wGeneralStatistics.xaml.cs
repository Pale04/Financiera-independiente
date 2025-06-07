using Business_logic.Payments;
using DomainClasses;
using Financiera_GUI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Financiera_GUI.PaymentManagement
{
    public partial class wGeneralStatistics : Page
    {
        private List<Payment>? _payments;
        private DateTime _firstDate = DateTime.Now.AddMonths(-1);
        private DateTime _lastDate = DateTime.Now;

        public wGeneralStatistics()
        {
            InitializeComponent();
            _payments = GetPayments();
            if (_payments != null)
            {
                RefreshRows();
                SetStatistics();
            }
        }
        private void SetStatistics()
        {
            double total = 0;
            double totalCollected = 0;
            double collectedPayments = 0;

            foreach (Payment payment in _payments)
            {
                total += (double)payment.Amount;

                if (payment.GetState().Equals(PaymentStatus.Collected))
                {
                    totalCollected += (double)payment.Amount;
                    collectedPayments++;
                }
            }

            totalCreditsLabel.Content = _payments.Count.ToString();
            totalAmountLabel.Content = total;
            totalCollectedLabel.Content = totalCollected;
            collectionEficiencyLabel.Content = $"{(collectedPayments/ _payments.Count)*100}%";
        }

        private void RefreshRows()
        {
            paymentsPanel.Children.Clear();

            foreach (Payment payment in _payments)
            {
                paymentsPanel.Children.Add(new ucStatisticsRow()
                {
                    CreditId = payment.CreditId.ToString(),
                    PaymentDate = payment.CollectionDate.ToString(),
                    State = payment.GetState().ToString(),
                    Amount = payment.Amount.ToString()
                });
            }
        }

        private List<Payment>? GetPayments()
        {
            PaymentManager manager = new();

            return manager.GetPaymentsFromDateRange(_firstDate, _lastDate);
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
