﻿using DomainClasses;
using Financiera_GUI.Credit;
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

namespace Financiera_GUI.MainMenus
{
    public partial class wRequestsManagement : Page
    {
        public wRequestsManagement()
        {
            InitializeComponent();
        }

        public void BtnCreditRequestListClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new wCreditApplications());
        }

        private void BtnSearchCustomerClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new CustomerManagement.wCustomerManagement());
        }
    }
}
