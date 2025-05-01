using System.Windows.Controls;
using Financiera_GUI.Utilities;
using Business_logic.Catalogs;
using DomainClasses;

namespace Financiera_GUI.Catalogs
{
    public partial class wDocumentationManagement : Page
    {
        private int _lastId;
        private int _firstId;
        private int _pageSize;
        private int _actualPage;
        private List<RequiredDocument> _nextPage;

        public wDocumentationManagement()
        {
            InitializeComponent();
            _pageSize = 10;
            RebootPages();
        }

        public void RebootPages()
        {
            _actualPage = 0;
            _lastId = 0;
            _firstId = 0;
            _nextPage = new();

            SaveNextPage();
            LoadPage(true);
        }

        private void LoadPage(bool forward)
        {
            RequiredDocumentationManager requiredDocumentationManagement = new();
            List<RequiredDocument> requiredDocuments = new();

            if(forward)
            {
                requiredDocuments = _nextPage;   
            }
            else
            {
                try
                {
                    requiredDocuments = requiredDocumentationManagement.GetByPagination(_pageSize, _firstId, false);
                }
                catch (Exception error)
                {
                    //TODO: show autoclosable message
                }
            }
            
            documentsTable.Children.RemoveRange(1, documentsTable.Children.Count);
            foreach (RequiredDocument document in requiredDocuments)
            {
                documentsTable.Children.Add(new wDocumentationManagementRow(document.Name, document.FileType.ToString(), document.Status ? "Activado" : "Desactivado"));
            }

            _actualPage += forward ? 1 : -1;
            _firstId = requiredDocuments.FirstOrDefault()?.Id ?? _firstId;
            _lastId = requiredDocuments.LastOrDefault()?.Id ?? _lastId;
            SaveNextPage();

            if (_actualPage == 1)
            {
                previousPageButton.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                previousPageButton.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void SaveNextPage()
        {
            RequiredDocumentationManager requiredDocumentationManagement = new();
            List<RequiredDocument> requiredDocuments = new();

            try
            {
                requiredDocuments = requiredDocumentationManagement.GetByPagination(_pageSize, _lastId, true);
            }
            catch (Exception error)
            {
                //TODO: show autoclosable message
            }

            _nextPage.Clear();
            foreach (RequiredDocument document in requiredDocuments)
            {
                _nextPage.Add(document);
            }

            nextPageButton.Visibility = _nextPage.Count == 0 ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
        }

        public void NextPage(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadPage(true);
        }

        public void PreviousPage(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadPage(false);
        }

        public void Back(object sender, System.Windows.RoutedEventArgs e)
        {
            //TODO: Implementar la lógica para regresar a la página anterior
        }

        public void RegisterDocument(object sender, System.Windows.RoutedEventArgs e)
        {
            RequiredDocumentationManager requiredDocumentationManagement = new();
            
            //TODO: validar campos

            string name = documentNameInput.Text;
            FileType fileType;
            switch ((string)fileTypeInput.SelectionBoxItem)
            {
                case "PDF":
                    fileType = FileType.pdf;
                    break;
                case "Image":
                    fileType = FileType.image;
                    break;
                default:
                    //TODO: show error message
                    return;
            };

            try
            {
                requiredDocumentationManagement.AddRequiredDocument(new RequiredDocument
                {
                    Name = name,
                    FileType = fileType
                });
            }
            catch (Exception error)
            {
                //TODO: show autoclosable message
                return;
            }

            //TODO: show autoclosable message
            RebootPages();
        }
    }
}
