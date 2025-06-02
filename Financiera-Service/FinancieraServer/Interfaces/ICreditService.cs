using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface ICreditService
    {
        [OperationContract]
        ResponseWithContent<int> AddCreditRequest(CreditRequestDC request);

        [OperationContract]
        ResponseWithContent<List<CreditDC>> GetActiveCredits();

        [OperationContract]
        ResponseWithContent<List<CreditDC>> GetCreditsByBeneficiary(string beneficiaryId);

        [OperationContract]
        ResponseWithContent<List<CreditRequestSummaryDC>> GetCreditRequests();

        [OperationContract]
        Response DetermineRequest(int requestId, bool granted);

        [OperationContract]
        ResponseWithContent<CreditDC> GetCredit(int creditId);

        [OperationContract]
        ResponseWithContent<List<CreditDocumentDC>> GetCreditsDocuments(int creditId);

        [OperationContract]
        Response UpdateCreditDocuments(int creditId, List<CreditDocumentDC> documents);


        [OperationContract]
        ResponseWithContent<CreditPaymentDC> GetPaymentInfo(int creditId);

    }
}
