namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class CreditDC
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public int Duration { get; set; }
        [DataMember]
        public int Capital { get; set; }
        [DataMember]
        public int RegistrerId { get; set; }
        [DataMember]
        public string BeneficiaryId { get; set; }
        [DataMember]
        public int ConditionId { get; set; }
        [DataMember]
        public string RegistryDate { get; set; }
    }
}
