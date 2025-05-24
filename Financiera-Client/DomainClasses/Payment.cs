namespace DomainClasses
{
    public enum PaymentStatus
    {
        Collected,
        NotCollected
    }

    public class Payment
    {
        public PaymentStatus _state;

        public int Id { get; set; }
        public PaymentStatus State { 
            set { _state = value; } 
        }

        public DateOnly CollectionDate { get; set; }
        public decimal Amount { get; set; }

        public int CreditId { get; set; }

        public int RegistrerId { get; set; }

        public PaymentStatus GetState()
        {
            return _state;
        }
    }
}
