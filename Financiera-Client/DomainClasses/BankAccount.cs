namespace DomainClasses
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string Clabe { get; set; }
        public string Purpose { get; set; }
        public string CustomerRfc { get; set; }
        
        public bool IsValidForUpdate()
        {
            return Id > 0 && !string.IsNullOrWhiteSpace(Clabe) && !string.IsNullOrWhiteSpace(Purpose);
        }
    }
}
