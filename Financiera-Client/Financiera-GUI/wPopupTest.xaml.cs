using Notification.Wpf;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Financiera_GUI
{
    /// <summary>
    /// Lógica de interacción para wPopupTest.xaml
    /// </summary>
    public partial class wPopupTest : Window
    {
        public wPopupTest()
        {
            InitializeComponent();
            var notificationManager = new NotificationManager();
            notificationManager.Show("Prueba", "Hola mundo, soy una notificación temporal", NotificationType.Information, "WindowArea");
        }
    }
}
