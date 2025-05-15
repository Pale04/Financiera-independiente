using Financiera_GUI.CatalogManagement;
using System.Windows.Controls;

namespace Financiera_GUI.MainMenus
{
    public partial class wSystemManagement : Page
    {
        public wSystemManagement()
        {
            InitializeComponent();
        }

        private void BtnCreateAccountClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //TODO: Implement the logic for navigate to the account creation page
        }

        private void BtnEditAccountClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //TODO: Implement the logic for navigate to the account update page
        }
        private void BtnCreditPoliciesManagementClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new wCreditPoliciesManagment());
        }
        private void BtnCreditConditionManagementClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new wCreditConditionManagement());
        }
        private void BtnDocumentationManagementClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new wDocumentationManagement());
        }
        private void BtnFinancialBranchesManagementClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new wFinancialBranchManagement());
        }
    }
}
