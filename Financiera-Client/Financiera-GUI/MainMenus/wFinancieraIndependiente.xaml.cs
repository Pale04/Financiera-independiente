using System.Windows;
using System.Windows.Navigation;
using Business_logic;

namespace Financiera_GUI.MainMenus
{
    public partial class wFinancieraIndependiente : Window
    {
        LoginManager manager = new LoginManager();

        public wFinancieraIndependiente()
        {
            InitializeComponent();

            switch (UserSession.Instance.Employee.Role)
            {
                case "admin":
                    ContentFrame.NavigationService.Navigate(new wSystemManagement());
                    break;
                case "analist":
                    //TODO: Change to wCreditApplication
                    break;
                case "adviser":
                    ContentFrame.NavigationService.Navigate(new wRequestsManagement());
                    break;
                case "collector":
                    ContentFrame.NavigationService.Navigate(new wPaymentManagment());
                    break;
            }
        }

        private void BtnLogoutClick(object sender, RoutedEventArgs e)

        {
            int response = manager.Logout(UserSession.Instance.Employee.User);

            if (response == 0)
            {
                ContentFrame.NavigationService.Navigate(new wLogin());
            }
        }
    }
}
