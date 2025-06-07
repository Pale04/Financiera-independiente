using Financiera_GUI.PaymentManagement;
using System.Windows.Controls;
using System.Windows.Input;

namespace Financiera_GUI.MainMenus
{
    public partial class wPaymentManagment : Page
    {
        public wPaymentManagment()
        {
            InitializeComponent();
        }

        private void BtnGeneratePaymentLayoutClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new wPaymentLayout());
        }

        private void BtnRegisterCreditPaymentsClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new wPaymentRecord());
        }

        private void BtnGenerateEfficiencyReportClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new wGeneralStatistics());
        }
    }
}
