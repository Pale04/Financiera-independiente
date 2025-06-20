using Business_logic;
using CatalogServiceReference;
using DomainClasses;
using Notification.Wpf;
using SessionServiceReference;
using System.Windows;
using System.Windows.Controls;
using Financiera_GUI.Utilities;
using Business_logic.Payments;
using System.ServiceModel;


namespace Financiera_GUI.Credit
{
    /// <summary>
    /// Lógica de interacción para wEvaluateApplication_S3.xaml
    /// </summary>
    public partial class wEvaluateApplication_S3 : Page
    {
        private DomainClasses.Credit _creditReferenced;
        private readonly NotificationManager _notificationManager;
        private DomainClasses.CreditCondition _creditConditionInfo;
        private List<DomainClasses.Payment> _creditPayments;
        private int _firstId;
        private int _lastId;
        private int _pageSize;
        private int _actualPage;
        private int _currentIndex;
        private List<DomainClasses.Payment> _paymentsNextPage;
        public wEvaluateApplication_S3(DomainClasses.Credit credit)
        {
            InitializeComponent();
            _creditReferenced = credit;
            _notificationManager = new NotificationManager();
            _creditConditionInfo = GetCreditCondition(_creditReferenced.Id);
            decimal total = CalculateTotal(_creditConditionInfo, _creditReferenced);
            int payments = CalculateNumberOfPayments(_creditConditionInfo, _creditReferenced);
            _creditPayments = GeneratePayments(total, payments, _creditReferenced, _creditConditionInfo);
            AddPayments(_creditPayments);
            _pageSize = 12;
            _currentIndex = 0;
            RebootPages();
            
            
        }

        private void AddPayments(List<DomainClasses.Payment> payments)
        {
            PaymentManager manager = new PaymentManager();
            bool anyFailed = false;
            try
            {
                foreach (var payment in payments)
                {
                    int code = manager.AddPayment(payment);
                    if(code != 0)
                    {
                        anyFailed = true;
                    }
                }

                if (anyFailed)
                {
                    _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
                }
                else
                {
                    _notificationManager.Show(NotificationMessages.GlobalRegistrationSuccess, NotificationType.Success, "WindowArea");
                }
            }
            catch(CommunicationException error)
            {
                _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
                
            }
        }

        public DomainClasses.CreditCondition GetCreditCondition(int idCredit)
        {
            CreditManager creditManager = new CreditManager();
            try
            {
                DomainClasses.CreditCondition creditCodition =  creditManager.GetCreditCondition(idCredit);
                if (creditCodition != null)
                {
                    return creditCodition;
                }
                else
                {
                    _notificationManager.Show("No se encontró la información de las condiciones de crédito", NotificationType.Error, "WindowArea");
                    return null;
                }
            }
            catch (Exception error)
            {
                _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
                return null;
            }
            
        }

        public decimal CalculateTotal(DomainClasses.CreditCondition creditCondition, DomainClasses.Credit credit)
        {
            decimal capital = credit.Capital;
            decimal iva = creditCondition.IVA / 100m;
            decimal interestRate = creditCondition.InterestRate / 100m;

            decimal total = capital + (capital * interestRate * (1 + iva));
            return total;
            
        }

        public int CalculateNumberOfPayments(DomainClasses.CreditCondition creditCondition, DomainClasses.Credit credit)
        {
            int totalPayments = credit.Duration * creditCondition.PaymentsPerMonth;
            return totalPayments;
        }

        public List<DomainClasses.Payment> GeneratePayments(decimal capital, int numberOfPayments, DomainClasses.Credit credit, DomainClasses.CreditCondition creditCondition)
        {
            List<DomainClasses.Payment> payments = new();
            decimal totalPerPayment = capital / numberOfPayments;

            decimal totalPerPaymentRounded = Math.Round(totalPerPayment, 2, MidpointRounding.AwayFromZero);
            System.DateOnly startDate = System.DateOnly.FromDateTime(DateTime.Today).AddMonths(1);
            System.DateOnly currentDate = startDate;
            int daysBetweenPayments = creditCondition.PaymentsPerMonth switch
            {
                1 => 30, 
                2 => 15,
                4 => 7,
                
            };
            int id = 1;
            for (int i = 0; i < numberOfPayments; i++)
            {
                
                DomainClasses.Payment payment = new()
                {
                    Id = id,
                    Amount = totalPerPaymentRounded,
                    CollectionDate = currentDate,
                    CreditId = credit.Id,
                    RegistrerId = UserSession.Instance.Employee.Id
                };
                payments.Add(payment);
                id++;
                currentDate = currentDate.AddDays(daysBetweenPayments);
            }

            PdfGenerator.GeneratePaymentLayoutDocument(payments, credit.Id);
            MessageBox.Show("Se ha guardado el documento de pagos en su carpeta de Descargas", "Documento generado", MessageBoxButton.OK, MessageBoxImage.Information);

            return payments;
        }

        public void RebootPages()
        {
            _actualPage = 1;
            _currentIndex = 0;
            _paymentsNextPage = new();
            if (_creditPayments.Count > 0)
            {
                _firstId = _creditPayments.First().Id;
                _lastId = _creditPayments.Count >= _pageSize ? _creditPayments[_pageSize - 1].Id : _creditPayments.Last().Id;
            }
            else
            {
                _firstId = 0;
                _lastId = 0;
            }

            SaveNextPage();
            LoadPage(false);
        }

        public void LoadPage(bool next)
        {
            if (next)
            {
                if (_currentIndex + _pageSize >= _creditPayments.Count)
                    return;

                _currentIndex += _pageSize;
            }
            else
            {
                if (_currentIndex - _pageSize < 0)
                {
                    _currentIndex = 0; // Primer página
                }
                else
                {
                    _currentIndex -= _pageSize;
                }
            }

            var payments = _creditPayments
                .Skip(_currentIndex)
                .Take(_pageSize)
                .ToList();

            _actualPage = (_currentIndex / _pageSize) + 1;

            TbPayments.Children.RemoveRange(1, TbPayments.Children.Count);
            foreach (var payment in payments)
            {
                TbPayments.Children.Add(new ucPaymentTableRow(payment));
            }

            SaveNextPage();

            previousPageButton.Visibility = _currentIndex == 0 ? Visibility.Hidden : Visibility.Visible;
            nextPageButton.Visibility = (_currentIndex + _pageSize >= _creditPayments.Count) ? Visibility.Hidden : Visibility.Visible;
        }


        public void SaveNextPage()
        {
            
            int lastIndex = _creditPayments.FindIndex(p => p.Id == _lastId);
            if (lastIndex == -1 || lastIndex + 1 >= _creditPayments.Count)
            {
                _paymentsNextPage = new();
                nextPageButton.Visibility = Visibility.Hidden;
                return;
            }

            _paymentsNextPage = _creditPayments
                .Skip(lastIndex + 1)
                .Take(_pageSize)
                .ToList();

            nextPageButton.Visibility = (_actualPage > 0 && _paymentsNextPage.Count == 0)
                ? Visibility.Hidden
                : Visibility.Visible;
        }

        private void PreviousPage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoadPage(false);
        }

        private void NextPage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LoadPage(true);
        }

        private void EvaluatePolicies(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new wCreditApplications());
        }
    }
}
