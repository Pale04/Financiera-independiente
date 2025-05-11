namespace DomainClasses
{
    public class PaymentLayout
    {
        public int Folio { get; set; }
        public string ClientName { get; set; }
        public DateOnly CollectionDate { get; set; }
        public decimal Amount { get; set; }
        public string BankAccountClabe { get; set; }
        public string Bank { get; set; }
    }
}
