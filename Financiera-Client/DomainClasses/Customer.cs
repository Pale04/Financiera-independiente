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
    }
}
