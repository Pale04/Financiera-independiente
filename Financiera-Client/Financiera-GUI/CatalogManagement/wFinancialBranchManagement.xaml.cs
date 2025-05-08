using Business_logic.Catalogs;
using DomainClasses;
using Financiera_GUI.Utilities;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
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

namespace Financiera_GUI.CatalogManagement
{
    public partial class wFinancialBranchManagement : Page
    {
        private List<Subsidiary> _subsidiaries;
        private NotificationManager _notificationManager = new();

        private int _currentPage = 1;
        private static int PAGE_SIZE = 12;

        public wFinancialBranchManagement()
        {
            InitializeComponent();
            SubsidiaryManager subsidiaryManager = new();

            try
            {
                _subsidiaries = subsidiaryManager.GetAll();
            }
            catch (Exception error)
            {
                var notificationManager = new NotificationManager();
                notificationManager.Show("Error al registrar", "Ocurrió un error de nuestro lado, intente nuevamente", NotificationType.Error, "WindowArea");
                _subsidiaries = [];
                return;
            }

            UpdateList();
        }

        private void UpdateList()
        {
            subsidiariesTable.Children.RemoveRange(1, subsidiariesTable.Children.Count);
            for (int i = (_currentPage * PAGE_SIZE) - PAGE_SIZE; i < int.Min(_subsidiaries.Count, _currentPage * PAGE_SIZE); i++)
            {
                subsidiariesTable.Children.Add(new ucSubsidiaryManagementRow(_subsidiaries[i]));
            }

            if (_currentPage * PAGE_SIZE < _subsidiaries.Count)
            {
                nextPageButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                nextPageButton.Visibility = Visibility.Visible;
            }

            if (_currentPage == 1)
            {
                previousPageButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                previousPageButton.Visibility = Visibility.Visible;
            }
        }

        private void Register(object sender, MouseButtonEventArgs e)
        {
            string address = addressInput.Text.Trim();
            if (string.IsNullOrEmpty(address))
            {
                _notificationManager.Show("Error", "No se puede agregar una sucursal sin dirección", NotificationType.Warning, "WindowArea");
                return;
            }

            SubsidiaryManager subsidiaryManager = new();
            switch (subsidiaryManager.Add(address))
            {
                case 0:
                    UpdateList();
                    _notificationManager.Show("Registrado", "La sucursal se registró correctamente", NotificationType.Success, "WindowArea");
                    break;
                
                case 1:
                    _notificationManager.Show("Error al registrar", "Ocurrió un error de nuestro lado, intente nuevamente", NotificationType.Error, "WindowArea");
                    break;

                case 2:
                    _notificationManager.Show("Error al registrar", "La dirección ingresada está vacía o contiene caracteres no validos", NotificationType.Warning, "WindowArea");
                    break;

                case 3:
                    _notificationManager.Show("Error al registrar", "La sucursal que intentó registrar ya existe", NotificationType.Warning, "WindowArea");
                    break;
            }
        }

        private void PreviousPage(object sender, MouseButtonEventArgs e)
        {
            pageCounter.Content = (int)pageCounter.Content - 1;
            UpdateList();
        }

        private void NextPage(object sender, MouseButtonEventArgs e)
        {
            pageCounter.Content = (int)pageCounter.Content + 1;
            UpdateList();
        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();

        }
    }
}
