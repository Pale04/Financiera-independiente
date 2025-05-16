using DomainClasses;
using System.Windows;

namespace Financiera_GUI.Credit
{
    public partial class wRequestSummary : Window
    {
        public wRequestSummary(DomainClasses.Credit credit, CreditCondition condition)
        {
            InitializeComponent();
            double total = credit.Capital * ((100 + condition.InterestRate + condition.IVA) / 100);
            string summary = $"Capital: {credit.Capital}\nPagos mensuales: {condition.PaymentsPerMonth} de ${total/(credit.Duration*condition.PaymentsPerMonth)}\nDuración en meses: {credit.Duration}\nInteres: {condition.InterestRate}%\n\nTotal a pagar: ${total}";
            textField.Text = summary;
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
