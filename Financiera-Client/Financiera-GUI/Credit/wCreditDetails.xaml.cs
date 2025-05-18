using System.Windows.Controls;

namespace Financiera_GUI.Credit
{
    public partial class wCreditDetails : Page
    {
        private DomainClasses.Credit _credit;
        private DomainClasses.CreditCondition _creditCondition;

        public wCreditDetails(int creditId)
        {
            InitializeComponent();
        }

        private void GetCreditData()
        {

        }
    }
}
