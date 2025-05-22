namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class CreditConditionDC
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public bool State { get; set; }
        [DataMember]
        public int InterestRate { get; set; }
        [DataMember]
        public int IVA { get; set; }
        [DataMember]
        public int PaymentsPerMonth { get; set; }
        [DataMember]
        public int RegistrerId { get; set; }
    }
}
