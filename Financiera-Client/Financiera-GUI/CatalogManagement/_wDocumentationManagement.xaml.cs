using System.Windows.Controls;
using Financiera_GUI.Utilities;

namespace Financiera_GUI.Catalogs
{
    public partial class _wDocumentationManagement : Page
    {
        public _wDocumentationManagement()
        {
            InitializeComponent();
        }

        public void ReturnPage(object sender, System.Windows.RoutedEventArgs e)
        {
            //TODO: Implementar la lógica para regresar a la página anterior
        }

        public void RegisterDocument(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.documentsTable.Children.Add(new wDocumentationManagementRow("IFE","PDF","Desactivado"));
        }
    }
}
