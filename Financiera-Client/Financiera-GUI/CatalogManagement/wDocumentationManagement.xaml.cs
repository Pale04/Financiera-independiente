using System.Windows.Controls;
using Financiera_GUI.Utilities;
using Business_logic.Catalogs;
using DomainClasses;
using System.Windows;
using Notification.Wpf;

namespace Financiera_GUI.Catalogs
{
    public partial class wDocumentationManagement : Page
    {
        private int _lastId;
        private int _firstId;
        private int _pageSize;
        private int _actualPage;
        private List<RequiredDocument> _nextPage;
        private readonly NotificationManager _notificationManager;

        public wDocumentationManagement()
        {
            InitializeComponent();
            _notificationManager = new NotificationManager();

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
                    _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                }
            }
            
            documentsTable.Children.RemoveRange(1, documentsTable.Children.Count);
            foreach (RequiredDocument document in requiredDocuments)
            {
                documentsTable.Children.Add(new wDocumentationManagementRow(document));
            }

            _actualPage += forward ? 1 : -1;
            _firstId = requiredDocuments.FirstOrDefault()?.Id ?? _firstId;
            _lastId = requiredDocuments.LastOrDefault()?.Id ?? _lastId;
            SaveNextPage();

            if (_actualPage == 1)
            {
                previousPageButton.Visibility = Visibility.Hidden;
            }
            else
            {
                previousPageButton.Visibility = Visibility.Visible;
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
                _notificationManager.Show(error.Message, NotificationType.Success, "WindowArea");
            }

            _nextPage.Clear();
            foreach (RequiredDocument document in requiredDocuments)
            {
                _nextPage.Add(document);
            }

            nextPageButton.Visibility = _nextPage.Count == 0 ? Visibility.Hidden : Visibility.Visible;
        }

        public void NextPage(object sender, RoutedEventArgs e)
        {
            LoadPage(true);
        }

        public void PreviousPage(object sender, RoutedEventArgs e)
        {
            LoadPage(false);
        }

        public void Back(object sender, RoutedEventArgs e)
        {
            //TODO: Implementar la lógica para regresar a la página anterior
        }

        public void RegisterDocument(object sender, RoutedEventArgs e)
        {
            RequiredDocumentationManager requiredDocumentationManagement = new();
            
            if (!ValidFields())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
                return;
            }

            string name = documentNameInput.Text;
            FileType fileType;
            switch ((string)fileTypeInput.SelectionBoxItem)
            {
                case "PDF":
                    fileType = FileType.pdf;
                    break;
                case "Imagen":
                    fileType = FileType.image;
                    break;
                default:
                    _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
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
                _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                return;
            }

            _notificationManager.Show(NotificationMessages.GlobalRegistrationSuccess, NotificationType.Success, "WindowArea");
            documentNameInput.Text = string.Empty;
            RebootPages();
        }

        private bool ValidFields()
        {
            return !string.IsNullOrWhiteSpace(documentNameInput.Text) && !string.IsNullOrEmpty((string)fileTypeInput.SelectionBoxItem);
        }
    }
}
