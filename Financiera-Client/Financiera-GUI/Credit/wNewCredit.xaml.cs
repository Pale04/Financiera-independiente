using DomainClasses;
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

namespace Financiera_GUI.Credit
{
    public partial class wNewCredit : Page
    {
        bool _signed = false;

        public wNewCredit()
        {
            InitializeComponent();
        }

        private void LoadCreditConditions()
        {

        }

        private void LoadRequiredDocuments()
        {

        }

        private void Back(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void CreateCreditRequest(object sender, RoutedEventArgs e)
        {
            if (!_signed)
            {
                return;
            }

            CreditCondition condition = GetSelectedCondition();
        }

        private CreditCondition GetSelectedCondition()
        {
            return null;
        }

        private bool AreDocumentsUploaded()
        {
            return true;
        }

        private void ShowSignWindow(object sender, RoutedEventArgs e)
        {

        }
    }
}
