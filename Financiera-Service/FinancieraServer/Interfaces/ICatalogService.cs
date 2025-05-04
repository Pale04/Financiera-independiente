using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface ICatalogService
    {
        [OperationContract]
        ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPaginationNext(int pageSize, int lastId);

        [OperationContract]
        ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPaginationPrevious(int pageSize, int firstId);

        [OperationContract]
        Response AddRequiredDocument(RequiredDocumentDC requiredDocument);

        [OperationContract]
        Response UpdateRequiredDocument(RequiredDocumentDC requiredDocument);

        [OperationContract]
        Response UpdateRequiredDocumentState(int id, bool isActive);

        [OperationContract]
        ResponseWithContent<List<CreditConditionDC>> GetCreditConditionsByPagination(int pageSize, int markId, bool next);

        [OperationContract]
        Response AddCreditCondition(CreditConditionDC creditCondition);

        [OperationContract]
        Response UpdateCreditCondition(CreditConditionDC creditCondition);

        [OperationContract]
        Response UpdateCreditConditionState(int id, bool state);
    }
}
