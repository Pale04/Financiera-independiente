namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class CreditDocumentDC
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string RegistryDate { get; set; }
        [DataMember]
        public int RegistrerId { get; set; }
        [DataMember]
        public int DocumentationId { get; set; }
        [DataMember]
        public byte[] file { get; set; }
    }
}
