using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface ICatalogService
    {
        [OperationContract]
        ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPagination(int pageSize, int lastId);
    }
}
