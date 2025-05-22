namespace DomainClasses
{
    public class CreditCondition
    {
        public int Id { get; set; }
        public bool State { get; set; }
        public int InterestRate { get; set; }
        public int IVA { get; set; }
        public int PaymentsPerMonth { get; set; }
        public int RegistrerId { get; set; }

        public bool IsValidForCreation()
        {
            return InterestRate >= 0 && IVA >= 0 && PaymentsPerMonth > 0 && RegistrerId > 0;
        }

        public bool IsValidForUpdate()
        {
            return Id > 0 && InterestRate >= 0 && IVA >= 0 && PaymentsPerMonth > 0;
        }

        override public string ToString()
        {
            return $"{InterestRate}% de interes a {PaymentsPerMonth} pagos mensuales";
        }
    }
}
