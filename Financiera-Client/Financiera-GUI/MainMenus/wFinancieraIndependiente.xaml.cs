using System.Windows;

namespace Financiera_GUI.MainMenus
{
    public partial class wFinancieraIndependiente : Window
    {
        public wFinancieraIndependiente()
        {
            InitializeComponent();
            ContentFrame.Content = new Catalogs.wDocumentationManagement();
        }
    }
}
