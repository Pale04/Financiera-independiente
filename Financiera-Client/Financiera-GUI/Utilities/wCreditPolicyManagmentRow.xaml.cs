using DomainClasses;
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

        }

        private void UpdateState(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
