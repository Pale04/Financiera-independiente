namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class PersonalReferenceDC
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Relationship { get; set; }
        [DataMember]
        public string CustomerRfc { get; set; }
    
        public bool IsValidForUpdate()
        {
            return Id > 0 && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(Relationship);
        }
    }
}
