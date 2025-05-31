using Business_logic;
using Business_logic.Catalogs;
using DomainClasses;
using Notification.Wpf;
using System.ServiceModel;
using System.Windows.Controls;
using Financiera_GUI.Utilities;
using CatalogServiceReference;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Media;

namespace Financiera_GUI.Credit
{
    public partial class wCreditDetails : Page
    {
        private DomainClasses.Credit? _credit;
        private DomainClasses.CreditCondition? _creditCondition;
        private DomainClasses.Customer? _customer;
        private List<DomainClasses.Document>? _documents;
        private List<RequiredDocument>? _requiredDocumentation;

        NotificationManager _notificationManager = new NotificationManager();

        public wCreditDetails(int creditId)
        {
            InitializeComponent();
            _credit = GetCreditData(creditId);

            if (_credit != null)
            {
                _creditCondition = GetCreditCondition(_credit.ConditionId);
                _customer = GetCustomerData(_credit.Beneficiary);
                _documents = GetDocuments(creditId);
                _requiredDocumentation = GetRequiredDocumentation();

                if (_customer != null && _creditCondition != null && _documents != null)
                {
                    LoadData();
                }
                else
                {
                    _notificationManager.Show("No se pudo cargar la información del crédito", NotificationType.Error);

                }
            }
        }

        private List<RequiredDocument>? GetRequiredDocumentation()
        {
            RequiredDocumentationManager manager = new();

            try
            {
                return manager.GetActive();
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Error);
                return null;
            }
        }

        private List<DomainClasses.Document>? GetDocuments(int creditId)
        {
            CreditDocumentManager manager = new();

            try
            {
                return manager.GetCreditDocuments(creditId);
            }
            catch (Exception ex)
            {
                _notificationManager.Show("No se pudo recuperar la información del crédito", NotificationType.Error);
                return null;
            }
        }

        private Customer? GetCustomerData(string rfc)
        {
            CustomerManager manager = new();

            try
            {
                return manager.GetByRfc(rfc);
            }
            catch (Exception ex)
            {
                _notificationManager.Show("No se pudo recuperar la información del crédito", NotificationType.Error);
                return null;
            }
        }

        private DomainClasses.CreditCondition? GetCreditCondition(int conditionId)
        {
            CreditConditionManager manager = new();

            try
            {
                var conditions = manager.GetByPagination(1000, 1000, false);

                foreach (var condition in conditions)
                {
                    if (condition.Id == conditionId)
                    {
                        return condition;
                    }
                }
            }
            catch (Exception error)
            {
                _notificationManager.Show("No se pudo recuperar la información del crédito", NotificationType.Error);
                return null;
            }

            _notificationManager.Show("No se pudo recuperar la información del crédito", NotificationType.Error);
            return null;
        }

        private DomainClasses.Credit? GetCreditData(int creditId)
        {
            CreditManager manager = new CreditManager();

            try
            {
                var credit = manager.GetCredit(creditId);

                if (credit != null)
                {
                    return credit;
                }

                _notificationManager.Show("No se pudo recuperar la información del crédito", NotificationType.Error);
            }
            catch (CommunicationException error)
            {
                _notificationManager.Show("No se pudo recuperar la información del crédito", NotificationType.Error);
            }

            return null;
        }

        private void LoadData()
        {
            double total = (double)_credit.Capital + (((double)_credit.Capital * ((double)_creditCondition.InterestRate / 100)) * (1.0 + ((double)_creditCondition.IVA / 100)));
            int monthlyPayments = (int)Math.Floor(total / (double)_creditCondition.PaymentsPerMonth);

            titleLabel.Content = $"Crédito N.{_credit.Id}";
            clientNameLabel.Content = $"Cliente: {_customer.Name}";
            clientAddressLabel.Content = $"Dirección: {_customer.HouseAddress}";

            capitalLabel.Content = $"Capital: ${_credit.Capital}";
            durationLabel.Content = $"Plazo: {_credit.Duration}";
            paymentsLabel.Content = $"{_creditCondition.PaymentsPerMonth} pagos mensuales de ${monthlyPayments}";
            interestLabel.Content = $"Tasa de interés: {_creditCondition.InterestRate}%";

            foreach (var document in _documents)
            {
                RequiredDocument? documentation = null;

                foreach (var requiredDocument in _requiredDocumentation)
                {
                    if (requiredDocument.Id == document.DocumentationId)
                    {
                        documentation = requiredDocument;
                        break;
                    }
                }

                if (documentation == null)
                {
                    _notificationManager.Show("No se pudo recuperar la información del crédito", NotificationType.Error);
                    return;
                }

                ucDocumentButton button = new()
                {
                    Text = documentation.Name,
                    AcceptedFile = documentation.FileType.ToString(),
                    DocumentationId = document.DocumentationId,
                    Color = Brushes.Gray,
                    selectable = false,
                    DocumentationName = documentation.Name
                };
                //button.SetImage("..\\Images\\upload_file_icon.png");

                documentsPanel.Children.Add(button);
            }
        }

        private void DetermineCredit(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new wEvaluateApplication_S1(_credit));
        }

        private void Back(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}