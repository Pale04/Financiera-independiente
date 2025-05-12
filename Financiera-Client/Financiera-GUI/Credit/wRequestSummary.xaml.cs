using DomainClasses;
using System.Windows;

namespace Financiera_GUI.Credit
{
    public partial class wRequestSummary : Window
    {
        public wRequestSummary(DomainClasses.Credit credit, CreditCondition condition)
        {
            InitializeComponent();
            string summary = $"Capital: {credit.Capital}\nPagos mensuales: {condition.PaymentsPerMonth}\nDuración en meses: {credit.Duration}\nInteres: {condition.InterestRate}%\n\nTotal a pagar: {credit.Capital * (100 + condition.InterestRate)}";
        }

        private void Accept(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
