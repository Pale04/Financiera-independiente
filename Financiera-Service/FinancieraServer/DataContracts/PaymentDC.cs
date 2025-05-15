namespace FinancieraServer.DataContracts
{
    [DataContract]
    public enum PaymentState
    {
        [EnumMember]
        Collected,
        [EnumMember]
        NotCollected
    }

    [DataContract]
    public class PaymentDC
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string CollectionDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public PaymentState PaymentState { get; set; }
        [DataMember]
        public int RegistrerId { get; set; }
        [DataMember]
        public int CreditId { get; set; }
    }
}
