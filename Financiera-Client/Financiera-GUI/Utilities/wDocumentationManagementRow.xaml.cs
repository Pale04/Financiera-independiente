using Business_logic.Catalogs;
using DomainClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Financiera_GUI.Utilities
{
    public partial class wDocumentationManagementRow : UserControl
    {
        private RequiredDocument _document;

        public wDocumentationManagementRow(RequiredDocument document)
        {
            InitializeComponent();
            _document = document;
            UpdateInformation();
            UpdateStatusIcon();
        }

        private void UpdateStatusIcon()
        {
            stateLabel.Content = _document.Status ? "Activado" : "Desactivado";
            changeStateIcon.Source = _document.Status ? new BitmapImage(new Uri("pack://application:,,,/Images/deactivate_icon.png")) : new BitmapImage(new Uri("pack://application:,,,/Images/activate_icon.png"));
        }

        private void UpdateInformation()
        {
            documentNameLabel.Content = _document.Name;
            formatLabel.Content = _document.FileType.ToString();
        }

        public void UpdateState(object sender, RoutedEventArgs e)
        {
            RequiredDocumentationManager requiredDocumentationManager = new();
            string action = _document.Status ? "desactivar" : "activar";

            MessageBoxResult confirmation = MessageBox.Show($"¿Está seguro de {action} el documento?", "Confirmar actualización de estado", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (confirmation != MessageBoxResult.Yes)
            {
                return;
            }

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
                //TODO: show autoclosable message
                return;
            }

            //TODO: show autoclosable message
            _document.Status = updatedRequiredDocument.Status;
            UpdateStatusIcon();
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
                //TODO: show autoclosable message
                return;
            }

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
                    //TODO: show autoclosable message
                    return;
                }

                //TODO: show autoclosable message
                _document.Name = updatedRequiredDocument.Name;
                _document.FileType = updatedRequiredDocument.FileType;
            }

            UpdateInformation();
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
