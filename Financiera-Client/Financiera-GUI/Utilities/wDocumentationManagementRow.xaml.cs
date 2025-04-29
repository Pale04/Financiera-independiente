using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Financiera_GUI.Utilities
{
    public partial class wDocumentationManagementRow : UserControl
    {
        public wDocumentationManagementRow(string documentName, string format, string state)
        {
            InitializeComponent();
            documentNameLabel.Content = documentName;
            formatLabel.Content = format;
            stateLabel.Content = state;
            changeStateIcon.Source = state == "Activado" ? new BitmapImage(new Uri("pack://application:,,,/Images/deactivate_icon.png")) : new BitmapImage(new Uri("pack://application:,,,/Images/activate_icon.png"));
        }
    }
}
