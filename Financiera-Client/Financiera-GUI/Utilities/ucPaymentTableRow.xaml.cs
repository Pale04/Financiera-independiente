using DomainClasses;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Financiera_GUI.Utilities
{
    /// <summary>
    /// Lógica de interacción para ucPaymentTableRow.xaml
    /// </summary>
    public partial class ucPaymentTableRow : UserControl
    {
        private readonly NotificationManager _notificationManager;
        private readonly Payment _payment;
       
        public ucPaymentTableRow(DomainClasses.Payment payment)
        {
            InitializeComponent();
            _payment = payment;
            
            LoadInformation();
        }

        private void LoadInformation()
        {
            var date = _payment.CollectionDate;
            var culture = new CultureInfo("es-MX");

            
            Payment.Content = "Pago " + _payment.Id;
            PaymentDate.Content = date.ToString("dd 'de' MMMM 'de' yyyy", culture);
            Amount.Content = $"${_payment.Amount} M.N(00/100)";
        }
    }
}
