using DomainClasses;
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
        private List<Subsidiary> subsidiaries;
        public wFinancialBranchManagement()
        {
            InitializeComponent();
            subsidiaries = Catalog
        }
    }
}
