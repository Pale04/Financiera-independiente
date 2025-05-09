using Business_logic.Catalogs;
using DomainClasses;
using Notification.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Financiera_GUI.Utilities
{
    public partial class wDocumentationManagementRow : UserControl
    {
        private RequiredDocument _document;
        private readonly NotificationManager _notificationManager;

        public wDocumentationManagementRow(RequiredDocument document)
        {
            InitializeComponent();
            _document = document;
            _notificationManager = new NotificationManager();
            LoadInformation();
            LoadStatusIcon();
        }

        private void LoadStatusIcon()
        {
            stateLabel.Content = _document.Status ? "Activado" : "Desactivado";
            changeStateIcon.Source = _document.Status ? new BitmapImage(new Uri("pack://application:,,,/Images/deactivate_icon.png")) : new BitmapImage(new Uri("pack://application:,,,/Images/activate_icon.png"));
        }

        private void LoadInformation()
        {
            documentNameLabel.Content = _document.Name;
            formatLabel.Content = _document.FileType switch
            {
                FileType.pdf => "PDF",
                _ => "Imagen",
            };
        }

        public void UpdateState(object sender, RoutedEventArgs e)
        {
            string action = _document.Status ? "desactivar" : "activar";
            MessageBoxResult confirmation = MessageBox.Show($"¿Está seguro de {action} el documento?", "Confirmar actualización de estado", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (confirmation != MessageBoxResult.Yes)
            {
                return;
            }

            RequiredDocumentationManager requiredDocumentationManager = new();
            RequiredDocument updatedRequiredDocument = new()
            {
                Id = _document.Id,
                Status = !_document.Status
            };

            try
            {
                requiredDocumentationManager.UpdateRequireDocumentStatus(updatedRequiredDocument);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                return;
            }

            _notificationManager.Show(NotificationMessages.GlobalStatusUpdateSuccess, NotificationType.Success, "WindowArea");
            _document.Status = updatedRequiredDocument.Status;
            LoadStatusIcon();
        }

        public void Update(object sender, RoutedEventArgs e)
        {
            ChangeEditableFields();
            documentNameInput.Text = _document.Name;
            fileTypeInput.SelectedIndex = _document.FileType == FileType.pdf ? 0 : 1;
        }

        public void SaveChanges(object sender, RoutedEventArgs e)
        {
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

            if (!name.Equals(_document.Name) || fileType != _document.FileType)
            {
                RequiredDocumentationManager requiredDocumentationManager = new();
                RequiredDocument updatedRequiredDocument = new()
                {
                    Id = _document.Id,
                    Name = name,
                    FileType = fileType,
                };

                try
                {
                    requiredDocumentationManager.UpdateRequiredDocument(updatedRequiredDocument);
                }
                catch (Exception error)
                {
                    _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                    return;
                }

                _notificationManager.Show(NotificationMessages.GlobalUpdateSuccess, NotificationType.Success, "WindowArea");
                _document.Name = updatedRequiredDocument.Name;
                _document.FileType = updatedRequiredDocument.FileType;
            }

            LoadInformation();
            ChangeEditableFields();
        }

        private bool ValidFields()
        {
            return !string.IsNullOrWhiteSpace(documentNameInput.Text) && !string.IsNullOrEmpty((string)fileTypeInput.SelectionBoxItem);
        }

        private void ChangeEditableFields()
        {
            if (documentNameLabel.Visibility == Visibility.Visible)
            {
                documentNameLabel.Visibility = Visibility.Collapsed;
                formatLabel.Visibility = Visibility.Collapsed;
                updateButton.Visibility = Visibility.Collapsed;

                documentNameInput.Visibility = Visibility.Visible;
                fileTypeInput.Visibility = Visibility.Visible;
                saveChangesButton.Visibility = Visibility.Visible;
            }
            else
            {
                documentNameInput.Visibility = Visibility.Collapsed;
                fileTypeInput.Visibility = Visibility.Collapsed;
                saveChangesButton.Visibility = Visibility.Collapsed;

                documentNameLabel.Visibility = Visibility.Visible;
                formatLabel.Visibility = Visibility.Visible;
                updateButton.Visibility = Visibility.Visible;
            }
        }
    }
}
