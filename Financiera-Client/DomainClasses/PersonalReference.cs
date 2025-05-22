namespace DomainClasses
{
    public class PersonalReference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Relationship { get; set; }
        public string CustomerRfc { get; set; }

        public bool IsValidForUpdate()
        {
            return Id > 0 && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(PhoneNumber) && !string.IsNullOrWhiteSpace(Relationship);
        }
    }
}
