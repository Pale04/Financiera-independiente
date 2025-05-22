using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DomainClasses;
using Financiera_GUI.CustomerManagement;

namespace Financiera_GUI.Utilities
{
    public partial class ucCustomerManagement : UserControl
    {
        private Customer _customer;
        private wCustomerManagement _previousReference;

        public ucCustomerManagement(Customer customer, wCustomerManagement previousReference)
        {
            InitializeComponent();
            _previousReference = previousReference;
            _customer = customer;
            rfcLabel.Content = _customer.Rfc;
            nameLabel.Content = _customer.Name;
            stateLabel.Content = _customer.State ? "Activo" : "Desactivado";
        }

        private void ShowDetail(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new wCustomerInfo(_customer.Rfc,_previousReference));
        }
    }
}
