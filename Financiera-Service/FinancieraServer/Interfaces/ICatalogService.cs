using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface ICatalogService
    {
        // Required documentation methods -------------------------------
        [OperationContract]
        ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPaginationNext(int pageSize, int lastId);

        [OperationContract]
        ResponseWithContent<List<RequiredDocumentDC>> GetRequiredDocumentationByPaginationPrevious(int pageSize, int firstId);

        [OperationContract]
        ResponseWithContent<List<RequiredDocumentDC>> GetActiveRequiredDocumentation();

        [OperationContract]
        Response AddRequiredDocument(RequiredDocumentDC requiredDocument);

        [OperationContract]
        Response UpdateRequiredDocument(RequiredDocumentDC requiredDocument);

        [OperationContract]
        Response UpdateRequiredDocumentState(int id, bool isActive);

        // Credit conditions methods -------------------------------------
        [OperationContract]
        ResponseWithContent<List<CreditConditionDC>> GetCreditConditionsByPagination(int pageSize, int markId, bool next);

        [OperationContract]
        ResponseWithContent<List<CreditConditionDC>> GetActiveCreditConditions();

        [OperationContract]
        Response AddCreditCondition(CreditConditionDC creditCondition);

        [OperationContract]
        Response UpdateCreditCondition(CreditConditionDC creditCondition);

        [OperationContract]
        Response UpdateCreditConditionState(int id, bool state);

        // Subsidiaries methods -------------------------------------------
        [OperationContract]
        ResponseWithContent<List<SubsidiaryDC>> GetSubsidiaries();

        [OperationContract]
        Response AddSubsidiary(string address);

        [OperationContract]
        Response UpdateSubsidiaryAddress(int id, string address);

        [OperationContract]
        Response UpdateSubsidiaryState(int id, bool activeSubsidiary);


        //Policies Methods -------------------------------------------------
        [OperationContract]
        ResponseWithContent<List<CreditPolicyDC>> GetCreditPoliciesByPagination(int pageSize, int markId, bool next);

        [OperationContract]
        Response AddCreditPolicy(CreditPolicyDC newPolicy);

        [OperationContract]
        Response UpdateCreditPolicy(CreditPolicyDC policyUpdated);

        [OperationContract]
        Response UpdateCreditPolicyState(int id, bool statePolicyUpdated);

    }
}
