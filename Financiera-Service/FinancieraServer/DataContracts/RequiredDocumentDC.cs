namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class RequiredDocumentDC
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool State { get; set; } 

        [DataMember]
        public string FileType { get; set; }
    }
}
