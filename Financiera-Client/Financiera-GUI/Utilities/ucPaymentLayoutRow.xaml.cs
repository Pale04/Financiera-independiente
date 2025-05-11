using DomainClasses;
using System.Windows.Controls;

namespace Financiera_GUI.Utilities
{
    public partial class ucPaymentLayoutRow : UserControl
    {
        public ucPaymentLayoutRow(PaymentLayout paymentLayout)
        {
            InitializeComponent();
            idLabel.Content = paymentLayout.Folio.ToString();
            clientNameLabel.Content = paymentLayout.ClientName;
            collectionDateLabel.Content = paymentLayout.CollectionDate.ToString("dd/MM/yyyy");
            amountLabel.Content = paymentLayout.Amount.ToString("C");
            bankLabel.Content = paymentLayout.Bank;
            clabeLabel.Content = paymentLayout.BankAccountClabe;
        }
    }
}
