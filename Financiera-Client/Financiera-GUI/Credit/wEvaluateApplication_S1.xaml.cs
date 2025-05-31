using Business_logic.Catalogs;
using CatalogServiceReference;
using DomainClasses;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
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
    /// Lógica de interacción para wEvaluateApplication_S1.xaml
    /// </summary>
    public partial class wEvaluateApplication_S1 : Page
    {
        public ObservableCollection<PolicyViewModel> ActivePolicies { get; } = new ObservableCollection<PolicyViewModel>();
        private readonly NotificationManager _notificationManager;
        private DomainClasses.Credit _creditReferenced;
        public wEvaluateApplication_S1(DomainClasses.Credit credit)
        {
            InitializeComponent();
            DataContext = this;
            _notificationManager = new NotificationManager();
            GetActivePolicies();
            _creditReferenced = credit;
        }

        public void GetActivePolicies()
        {
            ActivePolicies.Clear();
            CreditPolicyManager manager = new();
            List<Policy> activePolicies = new();

            try
            {
                activePolicies = manager.GetActivePolicies();
                foreach (var policy in activePolicies)
                {
                    
                    ActivePolicies.Add(new PolicyViewModel(policy));
                }
            }
            catch (CommunicationException error)
            {
                _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
            }
        }

        public void Back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new wCreditApplications());
        }

        private void EvaluatePolicies(object sender, RoutedEventArgs e)
        {
            bool policiesFullfilled = true;
            foreach (var policyEvaluated in ActivePolicies)
            {
                if (policyEvaluated.IsChecked == false)
                {
                    policiesFullfilled = false;
                }
            }
            
            NavigationService.Navigate(new wEvaluateApplication_S2( _creditReferenced, policiesFullfilled));
        }

        private void Evaluate(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox chk && chk.DataContext is PolicyViewModel pvm)
            {
                pvm.IsChecked = chk.IsChecked ?? false;
            }
        }
    }


}
