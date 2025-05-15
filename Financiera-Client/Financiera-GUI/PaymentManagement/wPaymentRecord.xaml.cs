using Microsoft.Win32;
using Notification.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Business_logic;
using DomainClasses;
using System.Text;

namespace Financiera_GUI.PaymentManagement
{
    public partial class wPaymentRecord : Page
    {
        private string _filePath;
        private readonly NotificationManager _notificationManager;

        public wPaymentRecord()
        {
            InitializeComponent();
            _filePath = string.Empty;
            _notificationManager = new NotificationManager();
        }

        public void Back(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }

        public void ShowFileExplorer(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Selecciona un archivo",
                Filter = "Todos los archivos (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                uploadIcon.Visibility = Visibility.Collapsed;
                chooseFileLabel.Visibility = Visibility.Collapsed;
                choosedFileName.Content = openFileDialog.SafeFileName;
                choosedFileName.Visibility = Visibility.Visible;
                chooseFileButton.Background = new SolidColorBrush(Color.FromRgb(224, 225, 221));

                _filePath = openFileDialog.FileName;
            }
        }

        public void RegisterPayments(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                _notificationManager.Show("Debe seleccionar el archivo con los pagos para continuar", NotificationType.Information, "WindowArea");
                return;
            }

            PaymentManager paymentManager = new PaymentManager();
            List<Payment> payments = new();

            try
            {
                payments = paymentManager.GetPaymentsFromCsv(_filePath);
            }
            catch (InvalidOperationException error)
            {
                MessageBox.Show(error.Message, "Error en en la estructura del archivo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            catch (Exception error)
            {
                _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                return;
            }

            StringBuilder badPayments = new StringBuilder(string.Empty);
            foreach (Payment payment in payments)
            {
                try
                {
                    paymentManager.UpdatePaymentsState(payment);
                }
                catch (InvalidOperationException error)
                {
                    badPayments.Append(string.Concat(payment.Id, ", "));
                }
                catch (Exception error)
                {
                    _notificationManager.Show(error.Message, NotificationType.Warning, "WindowArea");
                    return;
                }
            }

            if(badPayments.Length > 0)
            {
                MessageBox.Show("Todos los pagos fueron actualizados, excepto los siguientes debido a que no existen: " + badPayments.ToString(), "Pagos inexistentes", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _notificationManager.Show("Los pagos han sido registrados correctamente", NotificationType.Success, "WindowArea");
        }
    }
}
