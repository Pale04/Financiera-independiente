using System.Windows;
using Business_logic;

namespace Financiera_GUI.MainMenus
{
    public partial class wFinancieraIndependiente : Window
    {
        LoginManager manager = new LoginManager();

        public wFinancieraIndependiente()
        {
            InitializeComponent();

            switch (UserSession.Instance.Employee.role)
            {
                case "admin":
                    ContentFrame.NavigationService.Navigate(new wSystemManagement());
                    break;
                case "analist":
                    //TODO: Change to wCreditApplication
                    break;
                case "adviser":
                    //TODO: Change to adviser menu
                    break;
                case "collector":
                    //TODO: Change to wPaymentManagment
                    break;
            }
        }

        private void BtnLogoutClick(object sender, RoutedEventArgs e)
        {
            int response = manager.Logout(UserSession.Instance.Employee.user);

            if (response == 0)
            {
                ContentFrame.NavigationService.Navigate(new wLogin());
            }
        }
    }
}
