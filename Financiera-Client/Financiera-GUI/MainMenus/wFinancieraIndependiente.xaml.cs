using System.Windows;
using System.Windows.Navigation;
using Business_logic;
using Financiera_GUI.Credit;

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
                    //TODO: Go to wCreditApplications
                    //ContentFrame.NavigationService.Navigate(new wCreditApplications());
                    ContentFrame.NavigationService.Navigate(new wRequestsManagement());
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
                wLogin login = new wLogin();
                login.Show();
                this.Close();
            }
        }
    }
}
