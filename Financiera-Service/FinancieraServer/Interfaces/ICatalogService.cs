using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface ICatalogService
    {
        [OperationContract]
        ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPagination(int pageSize, int lastId);

        [OperationContract]
        Response AddRequiredDocument(RequiredDocumentDC requiredDocument);

        [OperationContract]
        Response UpdateRequiredDocument(RequiredDocumentDC requiredDocument);

        [OperationContract]
        Response UpdateRequiredDocumentState(int id, bool isActive);
    }
}
