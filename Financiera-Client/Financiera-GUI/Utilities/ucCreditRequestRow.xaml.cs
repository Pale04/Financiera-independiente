using DomainClasses;
using Microsoft.IdentityModel.Protocols.WsTrust;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Financiera_GUI.Utilities
{
    public partial class ucCreditRequestRow : UserControl
    {
        public int CreditId;

        public ucCreditRequestRow(CreditRequestSummary request)
        {
            InitializeComponent();
            CreditId = request.Id;
            creditIdLabel.Content = request.Id;
            beneficiaryLabel.Content = request.ClientName;
            CapitalLabel.Content = request.Capital;
            DurationLabel.Content = request.Duration;
            InterestLabel.Content = request.InterestRate;
        }

        public static readonly RoutedEvent ViewCreditEvent = EventManager.RegisterRoutedEvent(
            name: "ViewCredit",
            routingStrategy: RoutingStrategy.Bubble,
            handlerType: typeof(RoutedEventHandler),
            ownerType: typeof(ucCreditRequestRow));

        public event RoutedEventHandler ViewCredit
        {
            add { AddHandler(ViewCreditEvent, value); }
            remove { RemoveHandler(ViewCreditEvent, value); }
        }

        void RaiseViewCreditEvent()
        {
            RoutedEventArgs routedEventArgs = new(routedEvent: ViewCreditEvent);
            

            RaiseEvent(routedEventArgs);
        }

        private void ViewDetails(object sender, MouseButtonEventArgs e)
        {
            RaiseViewCreditEvent();
        }
    }
}