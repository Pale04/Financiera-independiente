using Business_logic;
using Business_logic.Catalogs;
using DomainClasses;
using Financiera_GUI.Utilities;
using Notification.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Financiera_GUI.Credit
{
    public partial class wNewCredit : Page
    {
        NotificationManager _notificationManager = new NotificationManager();

        private List<RequiredDocument> _requiredDocuments;
        private List<CreditCondition> _conditions;
        public string BeneficiaryId { get; set; }

        public wNewCredit()
        {
            InitializeComponent();
            LoadCreditConditions();
            LoadRequiredDocuments();
        }

        private void LoadCreditConditions()
        {
            CreditConditionManager conditionManager = new();

            try
            {
                _conditions = conditionManager.GetActive();
            }
            catch (Exception error)
            {
                Console.Write("No se pudieron recuperar las condiciones");
                _notificationManager.Show("Error", error.Message, NotificationType.Error, "WindowArea");
                return;
            }

            foreach (CreditCondition condition in _conditions)
            {
                RadioButton rb = new RadioButton() { Content = condition.ToString() };
                conditionsPanel.Children.Add(rb);
            }
        }

        private void LoadRequiredDocuments()
        {
            RequiredDocumentationManager documentationManager = new();

            try
            {
                _requiredDocuments =  documentationManager.GetActive();
            }
            catch (Exception error)
            {
                _notificationManager.Show("Error", "No se pudieron recuperar la lista de documentos necesarios", NotificationType.Error, "WindowArea");
                return;
            }

            if (_requiredDocuments.Count < 1)
            {
                return;
            }

            for (int i = 0; i < Math.Floor((double)_requiredDocuments.Count/4); i++)
            {
                documentsGrid.ColumnDefinitions.Add(new());
            }

            int currentDocument = 0;

            foreach (RequiredDocument requiredDocument in _requiredDocuments)
            {
                ucDocumentButton documentBtn = new ucDocumentButton()
                {
                    Text = requiredDocument.Name,
                    AcceptedFile = requiredDocument.FileType.ToString()
                };
                documentBtn.AcceptedFile = requiredDocument.FileType.ToString();
                documentBtn.SetImage(".\\Images\\upload_file_icon.png");
                documentsGrid.Children.Add(documentBtn);
                Grid.SetColumn(documentBtn, currentDocument);
                Grid.SetRow(documentBtn, (int)Math.Floor((double)currentDocument/4));

                currentDocument++;
            }
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult confirmation = MessageBox.Show($"¿Está seguro que desea salir? Los cambios se perderán", "Cancelar solicitud", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (confirmation == MessageBoxResult.Yes)
            { 
                NavigationService.GoBack(); 
            }
        }

        private void CreateCreditRequest(object sender, RoutedEventArgs e)
        {
            ResetErrorLabels();

            CreditCondition condition = GetSelectedCondition();            
            List<Document> documents = GetDocuments();

            if (condition == null)
            {
                _notificationManager.Show("Incompleto", "Seleccione una condición de crédito", NotificationType.Warning, "WindowArea");
                return;
            }
            else if (documents == null)
            {
                _notificationManager.Show("Incompleto", "Suba todos los documentos necesarios", NotificationType.Warning, "WindowArea");
                return;
            }

            byte duration;
            int capital;
            
            if (!byte.TryParse(durationField.Text.Trim(), out duration))
            {
                durationField.showError("Requerido*");
                return;
            }
            else if (!int.TryParse(capitalField.Text.Trim(), out capital))
            {
                capitalField.showError("Requerido*");
                return;
            }

            DomainClasses.Credit credit = new()
            {
                Duration = duration,
                Capital = capital,
                Beneficiary = BeneficiaryId,
                ConditionId = condition.Id,
            };

            wRequestSummary summaryDialog = new(credit, condition);

            if (summaryDialog.ShowDialog() == false)
            {
                return;
            }
             
            CreditManager creditManager = new();
            switch (creditManager.Add(credit, documents))
            {
                case 1:
                    _notificationManager.Show("Error", "Ocurrió un error al crear solicitud, intente nuevamente", NotificationType.Error, "WindowArea");
                    break;
                case 2:
                    _notificationManager.Show("Crédito activo", "El cliente ya tiene un crédito activo", NotificationType.Warning, "WindowArea");
                    break;
                case 3:
                    _notificationManager.Show("No elegible", "El cliente tiene una solicitud rechazada menor a 3 meses", NotificationType.Warning, "WindowArea");
                    break;
                case 0:
                    NavigationService.GoBack();
                    break;
            }
        }

        private void ResetErrorLabels()
        {
            durationField.errorLabel.Visibility = Visibility.Collapsed;
            capitalField.errorLabel.Visibility = Visibility.Collapsed;
        }

        private List<Document> GetDocuments()
        {
            List<Document> documents = [];

            foreach (ucDocumentButton button in documentsGrid.Children)
            {
                if (button.File == null)
                {
                    return null;
                }

                documents.Add(new()
                {
                    Name = button.FileName,
                    File = button.File,
                    RegistryDate = DateTime.Now,
                    Registrer = UserSession.Instance.Employee.Id,
                    DocumentationId = button.DocumentationId
                });
            }

            return documents;
        }

        private CreditCondition GetSelectedCondition()
        {
            foreach (RadioButton button in conditionsPanel.Children)
            {
                if (button.IsChecked == true)
                {
                    Console.WriteLine("encontrada condicion seleccionada");
                    foreach (CreditCondition condition in _conditions)
                    {
                        if (condition.ToString().Equals(button.Content))
                        {
                            return condition;
                        }
                    }
                }
            }

            return null;
        }
    }
}
