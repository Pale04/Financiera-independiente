using Business_logic.Catalogs;
using DomainClasses;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public partial class ucSubsidiaryManagementRow : UserControl
    {
        private Subsidiary _subsidiary;
        private readonly NotificationManager _notificationManager;

        public ucSubsidiaryManagementRow(Subsidiary subsidiary)
        {
            InitializeComponent();
            _subsidiary = subsidiary;
            _notificationManager = new NotificationManager();
            LoadInformation();
            LoadStatusIcon();
        }

        private void LoadStatusIcon()
        {
            stateLabel.Content = _subsidiary.State ? "Activado" : "Desactivado";
            changeStateIcon.Source = _subsidiary.State ? new BitmapImage(new Uri("pack://application:,,,/Images/deactivate_icon.png")) : new BitmapImage(new Uri("pack://application:,,,/Images/activate_icon.png"));
        }

        private void LoadInformation()
        {
            addressLabel.Content = _subsidiary.Address;
            idLabel.Content = _subsidiary.Id;
            stateLabel.Content = _subsidiary.State;
        }

        private void toggleEditableFields()
        {
            if (updateButton.IsVisible)
            {
                updateButton.Visibility = Visibility.Collapsed;
                saveChangesButton.Visibility = Visibility.Visible;

                addressInput.Visibility = Visibility.Visible;
                addressLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                updateButton.Visibility = Visibility.Visible;
                saveChangesButton.Visibility = Visibility.Collapsed;

                addressInput.Visibility = Visibility.Collapsed;
                addressLabel.Visibility = Visibility.Visible;
            }
        }

        private void UpdateAddress(object sender, MouseButtonEventArgs e)
        {
            toggleEditableFields();
            addressInput.Text = _subsidiary.Address;
        }

        private void UpdateState(object sender, MouseButtonEventArgs e)
        {
            SubsidiaryManager subsidiaryManager = new();
            string action = _subsidiary.State? "desactivar" : "activar";

            MessageBoxResult confirmation = MessageBox.Show($"¿Está seguro de {action} la sucursal?", "Confirmar actualización de estado", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);

            if (confirmation == MessageBoxResult.Yes)
            {
                try
                {
                    subsidiaryManager.updateState(_subsidiary.Id, !_subsidiary.State);
                }
                catch (Exception error)
                {
                    _notificationManager.Show("Error al actualizar", "Ocurrió un error al actualizar la información, intente más tarde", NotificationType.Error, "WindowArea");
                    return;
                }

                _notificationManager.Show(NotificationMessages.GlobalStatusUpdateSuccess, NotificationType.Success, "WindowArea");
                _subsidiary.State = !_subsidiary.State;
                LoadStatusIcon();
            }
        }

        private void SaveChanges(object sender, MouseButtonEventArgs e)
        {
            toggleEditableFields();
            SubsidiaryManager subsidiaryManager = new();
            string address = addressInput.Text.Trim();

            try
            {
                switch (subsidiaryManager.updateAddress(_subsidiary.Id, address))
                {
                    case 0:
                        _notificationManager.Show("Actualizado", "La sucursal se actualizó correctamente", NotificationType.Success, "WindowArea");
                        _subsidiary.Address = address;
                        addressLabel.Content = address;
                        break;

                    case 1:
                        _notificationManager.Show("Error al actualizar", "Ocurrió un error de nuestro lado, intente nuevamente", NotificationType.Error, "WindowArea");
                        break;

                    case 2:
                        _notificationManager.Show("Error al actualizar", "La dirección ingresada está vacía o contiene caracteres no validos", NotificationType.Warning, "WindowArea");
                        break;

                    case 3:
                        _notificationManager.Show("Error al actualizar", "Ya existe una sucursal con esa dirección", NotificationType.Warning, "WindowArea");
                        break;
                }
            }
            catch (Exception error)
            {
                _notificationManager.Show("Error al actualizar", "Ocurrió un error al actualizar la información, intente más tarde", NotificationType.Error, "WindowArea");
                return;
            }
        }
    }
}
