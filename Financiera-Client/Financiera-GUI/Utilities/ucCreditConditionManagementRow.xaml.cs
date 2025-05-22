using Business_logic.Catalogs;
using DomainClasses;
using Notification.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Financiera_GUI.Utilities
{
    public partial class ucCreditConditionManagementRow : UserControl
    {
        private readonly CreditCondition _creditCondition;
        private readonly NotificationManager _notificationManager;

        public ucCreditConditionManagementRow(CreditCondition creditCondition)
        {
            InitializeComponent();
            _creditCondition = creditCondition;
            _notificationManager = new NotificationManager();
            LoadInformation();
            LoadStatusIcon();
        }

        private void LoadInformation()
        {
            paymentsPerMonthLabel.Content = _creditCondition.PaymentsPerMonth switch
            {
                1 => "Mensual",
                2 => "Quincenal",
                _ => "Semanal"
            };
            interestRateLabel.Content = _creditCondition.InterestRate.ToString() + "%";
            ivaLabel.Content = _creditCondition.IVA.ToString() + "%";
        }

        private void LoadStatusIcon()
        {
            stateLabel.Content = _creditCondition.State ? "Activado" : "Desactivado";
            changeStateIcon.Source = _creditCondition.State ? new BitmapImage(new Uri("pack://application:,,,/Images/deactivate_icon.png")) : new BitmapImage(new Uri("pack://application:,,,/Images/activate_icon.png"));
        }

        public void UpdateState(object sender, RoutedEventArgs e)
        {
            CreditConditionManager creditConditionManager = new();
            string action = _creditCondition.State ? "desactivar" : "activar";

            MessageBoxResult confirmation = MessageBox.Show($"¿Está seguro de {action} la condición de crédito?", "Confirmar actualización de estado", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (confirmation != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                creditConditionManager.UpdateCreditConditionState(_creditCondition.Id, !_creditCondition.State);
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                return;
            }

            _notificationManager.Show(NotificationMessages.GlobalStatusUpdateSuccess, NotificationType.Success, "WindowArea");
            _creditCondition.State = !_creditCondition.State;
            LoadStatusIcon();
        }

        public void Update(object sender, RoutedEventArgs e)
        {
            ChangeEditableFields();
            paymentsPerMonthInput.SelectedIndex = _creditCondition.PaymentsPerMonth switch
            {
                1 => 2,
                2 => 1,
                _ => 0
            };
            interestRateInput.Text = _creditCondition.InterestRate.ToString();
            ivaInput.Text = _creditCondition.IVA.ToString();
        }

        public void SaveChanges(object sender, RoutedEventArgs e)
        {
            if (!ValidFields())
            {
                _notificationManager.Show(NotificationMessages.GlobalEmptyFields, NotificationType.Warning, "WindowArea");
                return;
            }

            int paymentsPerMonth = paymentsPerMonthInput.SelectedIndex switch
            {
                0 => 4,
                1 => 2,
                _ => 1
            };
            int interestRate;
            int iva;
            try
            {
                interestRate = int.Parse(interestRateInput.Text);
                iva = int.Parse(ivaInput.Text);
            }
            catch (FormatException)
            {
                _notificationManager.Show("La tasa de interés y el IVA deben ser números enteros", NotificationType.Warning, "WindowArea");
                return;
            }

            if (paymentsPerMonth != _creditCondition.PaymentsPerMonth || interestRate != _creditCondition.InterestRate || iva != _creditCondition.IVA)
            {
                CreditConditionManager creditConditionManager = new();
                CreditCondition updatedCreditCondition = new()
                {
                    Id = _creditCondition.Id,
                    PaymentsPerMonth = paymentsPerMonth,
                    InterestRate = interestRate,
                    IVA = iva
                };

                try
                {
                    creditConditionManager.UpdateCreditCondition(updatedCreditCondition);
                }
                catch (Exception error)
                {
                    _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                    return;
                }

                _notificationManager.Show(NotificationMessages.GlobalUpdateSuccess, NotificationType.Success, "WindowArea");
                _creditCondition.PaymentsPerMonth = updatedCreditCondition.PaymentsPerMonth;
                _creditCondition.InterestRate = updatedCreditCondition.InterestRate;
                _creditCondition.IVA = updatedCreditCondition.IVA;
            }

            LoadInformation();
            ChangeEditableFields();
        }

        private bool ValidFields()
        {
            return !string.IsNullOrEmpty((string)paymentsPerMonthInput.SelectionBoxItem) && !string.IsNullOrWhiteSpace(interestRateInput.Text) && !string.IsNullOrWhiteSpace(ivaInput.Text);
        }

        private void ChangeEditableFields()
        {
            if (paymentsPerMonthLabel.Visibility == Visibility.Visible)
            {
                paymentsPerMonthLabel.Visibility = Visibility.Hidden;
                interestRateLabel.Visibility = Visibility.Hidden;
                ivaLabel.Visibility = Visibility.Hidden;
                updateButton.Visibility = Visibility.Hidden;

                paymentsPerMonthInput.Visibility = Visibility.Visible;
                interesRateEditableField.Visibility = Visibility.Visible;
                ivaEditableField.Visibility = Visibility.Visible;
                saveChangesButton.Visibility = Visibility.Visible;
            }
            else
            {
                paymentsPerMonthInput.Visibility = Visibility.Hidden;
                interesRateEditableField.Visibility = Visibility.Hidden;
                ivaEditableField.Visibility = Visibility.Hidden;
                saveChangesButton.Visibility = Visibility.Hidden;

                paymentsPerMonthLabel.Visibility = Visibility.Visible;
                interestRateLabel.Visibility = Visibility.Visible;
                ivaLabel.Visibility = Visibility.Visible;
                updateButton.Visibility = Visibility.Visible;                
            }
        }
    }
}
