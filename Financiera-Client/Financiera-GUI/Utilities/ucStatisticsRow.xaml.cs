using System.Windows.Controls;

namespace Financiera_GUI.Utilities
{
    public partial class ucStatisticsRow : UserControl
    {
        public string CreditId
        {
            set => creditLabel.Content = value;
        }

        public string PaymentDate
        {
            set => dateLabel.Content = value;
        }

        public string Amount
        {
            set => amountLabel.Content = value;
        }

        public string State
        {
            set => stateLabel.Content = value;
        }

        public ucStatisticsRow()
        {
            InitializeComponent();
        }
    }
}
