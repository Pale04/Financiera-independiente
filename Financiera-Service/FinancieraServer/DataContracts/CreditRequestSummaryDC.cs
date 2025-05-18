namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class CreditRequestSummaryDC
    {
        [DataMember]
        public int Id;
        [DataMember]
        public int Duration;
        [DataMember]
        public int Capital;
        [DataMember]
        public string ClientName;
        [DataMember]
        public int InterestRate;
    }
}
