namespace FinancieraServer.DataContracts
{
    [DataContract]
    public enum PaymentState
    {
        [EnumMember]
        Pending,
        [EnumMember]
        Collected,
        [EnumMember]
        Failed
    }

    [DataContract]
    public class PaymentDC
    {
        [DataMember]
        public int Folio { get; set; }
        [DataMember]
        public string CollectionDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public PaymentState State { get; set; }
        [DataMember]
        public int RegistrerId { get; set; }
        [DataMember]
        public int CreditId { get; set; }
        [DataMember]
        public string BeneficiaryRfc { get; set; }
    }
}
