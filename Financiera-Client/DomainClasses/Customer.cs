namespace DomainClasses
{
    public class Customer
    { 
        public string Rfc { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public string HouseAddress { get; set; }
        public string WorkAddress { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }
        public bool State { get; set; }
        public BankAccount[] BankAccounts { get; set; }
        public PersonalReference[] PersonalReferences { get; set; }

        public bool IsValidForCreation()
        {
            return IsValidForUpdate() &&
                   BankAccounts != null && BankAccounts.Length == 2 &&
                   PersonalReferences != null && PersonalReferences.Length == 2;
        }

        public bool IsValidForUpdate()
        {
            return !string.IsNullOrEmpty(Rfc) &&
                   !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(HouseAddress) &&
                   !string.IsNullOrEmpty(WorkAddress) &&
                   !string.IsNullOrEmpty(PhoneNumber1) &&
                   !string.IsNullOrEmpty(PhoneNumber2) &&
                   !string.IsNullOrEmpty(Email);
        }
    }
}
