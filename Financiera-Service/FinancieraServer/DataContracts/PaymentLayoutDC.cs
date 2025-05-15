namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class PaymentLayoutDC
    {
        [DataMember]
        public int Folio { get; set; }
        [DataMember]
        public string ClientName { get; set; }
        [DataMember]
        public string CollectionDate { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public string BankAccountClabe { get; set; }
    }
}