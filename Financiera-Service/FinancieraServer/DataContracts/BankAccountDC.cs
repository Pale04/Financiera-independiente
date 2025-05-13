namespace FinancieraServer.DataContracts
{
    [DataContract]
    public enum BankAccountType
    {
        [EnumMember]
        Receive,
        [EnumMember]
        Collect
    }

    [DataContract]
    public class BankAccountDC
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Clabe { get; set; }
        [DataMember]
        public BankAccountType Purpose { get; set; }
        [DataMember]
        public string CustomerRfc { get; set; }
    }
}
