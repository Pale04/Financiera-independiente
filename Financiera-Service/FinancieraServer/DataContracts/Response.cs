namespace FinancieraServer.DataContracts
{
    [DataContract]
    public class ResponseWithContent<T> : Response
    {
        public ResponseWithContent(int statusCode) : base(statusCode) { }

        public ResponseWithContent(int statusCode, string message) : base(statusCode, message) { }

        public ResponseWithContent(int statusCode , T data) : base(statusCode)
        {
            Data = data;
        }

        [DataMember]
        public T? Data { get; set; }
    }

    [DataContract]
    public class Response
    {
        public Response(int statusCode)
        {
            StatusCode = statusCode;
        }

        public Response(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        [DataMember]
        public int StatusCode { get; set; }

        [DataMember]
        public string? Message { get; set; }

    }
}
