namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class AccountDC
    {
        [DataMember]
        public string username { set; get; }

        [DataMember]
        public string password { set; get; }

        [DataMember]
        public string role { set; get; }
    }
}
