using Business_logic;
using DomainClasses;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Financiera_GUI.Credit
{
    /// <summary>
    /// Lógica de interacción para wEvaluateApplication_S2.xaml
    /// </summary>
    public partial class wEvaluateApplication_S2 : Page
    {
        private readonly NotificationManager _notificationManager;
        private bool _policiesApproved;
        private DomainClasses.Credit _creditReferenced;
        public wEvaluateApplication_S2(DomainClasses.Credit creditReferenced, bool isApproved)
        {
            _policiesApproved = isApproved;
            InitializeComponent();
            _notificationManager = new NotificationManager();
            setEvaluation();
            _creditReferenced = creditReferenced;
        }

        public void setEvaluation()
        {
            DateTime currentDate = DateTime.Now;
            var DateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            string formatedDateTime = $"{currentDate.ToLongDateString()}, {currentDate.ToShortTimeString()}";
            LbDate.Content =  $"Fecha y Hora:  {formatedDateTime}";
            string analist = UserSession.Instance.Employee.Name;
            LbEmployeeName.Content = $"Analista: {analist}";

            CbState.Text = "Selecciona una opción";
            CbState.Items.Insert(0, "Aceptada");
            CbState.Items.Insert(1, "Rechazada");

            if (!_policiesApproved)
            {
                CbState.SelectedIndex = 1;
                CbState.IsEnabled = false;
                TbComments.Text = "No se cumplen las políticas de la empresa para un crédito.";
            }
        }


        public void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate( new wEvaluateApplication_S1(_creditReferenced));
        }

        private void EvaluatePolicies(object sender, RoutedEventArgs e)
        {
            if (CbState.SelectedIndex == 1)
            {
                _policiesApproved = false;
            }
            else
            {
                _policiesApproved = true;
            }

            CreditManager manager = new();
            int statusCode = manager.DeterminateResquest(_creditReferenced ,_policiesApproved);
            switch (statusCode)
            {
                case 1:
                    _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error);
                    break;
                case 2:
                    _notificationManager.Show(NotificationMessages.EvaluationInvalidCredit, NotificationType.Warning);
                    break;
                case 4:
                    _notificationManager.Show(NotificationMessages.EvaluationEmptyCredit, NotificationType.Warning);
                    break;
                case 0:
                    _notificationManager.Show(NotificationMessages.GlobalStatusUpdateSuccess, NotificationType.Success);
                    if (!_policiesApproved)
                    {
                        NavigationService.Navigate(new wCreditDetails(_creditReferenced.Id));
                    }
                    else
                    {
                        NavigationService.Navigate(new wEvaluateApplication_S3(_creditReferenced));
                    }

                        break;
            }
        }
    }
}
