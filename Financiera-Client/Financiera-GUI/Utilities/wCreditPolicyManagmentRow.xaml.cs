using Business_logic;
using Business_logic.Catalogs;
using CatalogServiceReference;
using DomainClasses;
using Notification.Wpf;
using System;
using System.Collections.Generic;
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

namespace Financiera_GUI.Utilities
{
    /// <summary>
    /// Lógica de interacción para wCreditPolicyManagmentRow.xaml
    /// </summary>
    public partial class wCreditPolicyManagmentRow : UserControl
    {
        private readonly Policy _policy;
        private readonly NotificationManager _notificationManager;
        public wCreditPolicyManagmentRow(Policy policy)
        {
            InitializeComponent();
            _policy = policy;
            _notificationManager = new NotificationManager();
            LoadInformation();
            LoadStateIcon();
        }


        private void LoadInformation()
        {
            politicTitle.Content = _policy.Title;
            PoliticDescription.Content = _policy.Description;
            LoadStateIcon();
        }

        private void LoadStateIcon()
        {
            politicState.Content = _policy.State ? "Activa" : "Inactiva";
            changeStateIcon.Source = _policy.State ? new BitmapImage(new Uri("pack://application:,,,/Images/deactivate_icon.png")) : new BitmapImage(new Uri("pack://application:,,,/Images/activate_icon.png"));
        }

        
        private void EnableEdit()
        {
            if (politicTitle.Visibility == Visibility.Visible)
            {
                politicTitle.Visibility = Visibility.Collapsed;
                PoliticDescription.Visibility = Visibility.Collapsed;
                politicState.Visibility = Visibility.Collapsed;
                titleInput.Visibility = Visibility.Visible;
                descriptionInput.Visibility = Visibility.Visible;
                saveChangesButton.Visibility = Visibility.Visible;
            }
            else
            {
                politicTitle.Visibility = Visibility.Visible;
                PoliticDescription.Visibility = Visibility.Visible;
                politicState.Visibility = Visibility.Visible;
                titleInput.Visibility = Visibility.Collapsed;
                descriptionInput.Visibility = Visibility.Collapsed;
                saveChangesButton.Visibility = Visibility.Collapsed;
            }
        }

        private void Update(object sender, MouseButtonEventArgs e)
        {
            EnableEdit(); 
        }

        private void SaveChanges(object sender, MouseButtonEventArgs e)
        {
            if (!IsValid())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
            }

            CreditPolicyManager manager = new();
            Policy policyUpdated = new()
            {
                Id = _policy.Id,
                Title = titleInput.Text,
                Description = descriptionInput.Text,
                Registrer = UserSession.Instance.Employee.id
            };

            try
            {
                int statusCode = manager.UpdatePolicy(policyUpdated);

                switch (statusCode)
                {
                    case 1:
                        _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
                        break;
                    case 2:
                        _notificationManager.Show(NotificationMessages.GlobalInvalidFields, NotificationType.Warning, "WindowArea");
                        break;
                    case 3:
                        _notificationManager.Show(NotificationMessages.PolicyDuplicated, NotificationType.Warning, "WindowArea");
                        break;
                    case 4:
                        _notificationManager.Show(NotificationMessages.PolicyNotFound, NotificationType.Error, "WindowArea");
                        break;
                    case 0:
                        _notificationManager.Show(NotificationMessages.GlobalRegistrationSuccess, NotificationType.Success, "WindowArea");
                        break;
                }

                _policy.Title = policyUpdated.Title;
                _policy.Description = policyUpdated.Description;
                _policy.Registrer = policyUpdated.Registrer;
                LoadInformation();
                EnableEdit();
            }
            catch(CommunicationException error)
            {
                _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
            }
        }

        private void UpdateState(object sender, MouseButtonEventArgs e)
        {
            CreditPolicyManager manager = new();
            string newState = _policy.State ? "Desactivar" : "Activar";
            MessageBoxResult messageBox = MessageBox.Show($"Se selecciono la opción {newState}, ¿Está seguro de continuar?", "Cambiar estado de política",MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            if (messageBox != MessageBoxResult.Yes)
            {
                return;
            }
            bool state = !_policy.State;
            try
            {
                int statusCode = manager.UpdatePolicyState(_policy.Id, state);
                switch (statusCode)
                {
                    case 1:
                        _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
                        break;
                    case 2:
                        _notificationManager.Show(NotificationMessages.GlobalInvalidFields, NotificationType.Warning, "WindowArea");
                        break;
                    case 3:
                        _notificationManager.Show(NotificationMessages.PolicyDuplicated, NotificationType.Warning, "WindowArea");
                        break;
                    case 4:
                        _notificationManager.Show(NotificationMessages.PolicyNotFound, NotificationType.Error, "WindowArea");
                        break;
                    case 0:
                        _notificationManager.Show(NotificationMessages.GlobalRegistrationSuccess, NotificationType.Success, "WindowArea");
                        break;
                }
            }
            catch (CommunicationException error)
            {
                _notificationManager.Show(NotificationMessages.GlobalInternalError, NotificationType.Error, "WindowArea");
            }

            _policy.State = !_policy.State;
            LoadStateIcon();
        }

        public bool IsValid()
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(titleInput.Text) ||  !string.IsNullOrWhiteSpace(descriptionInput.Text) || !string.IsNullOrEmpty(titleInput.Text) || !string.IsNullOrEmpty(descriptionInput.Text)) 
            {
                isValid = true;
            }
            return isValid;
        }
    }
}
