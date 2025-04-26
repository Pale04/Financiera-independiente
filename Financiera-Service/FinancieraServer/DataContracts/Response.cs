namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class ResponseWithContent<T> : Response
    {
        [DataMember]
        public T? Data { get; set; }
    }

    [DataContract]
    public class Response
    {
        public Response() { }

        [DataMember]
        public int StatusCode { get; set; }

        [DataMember]
        public string? Message { get; set; }
    }
}
