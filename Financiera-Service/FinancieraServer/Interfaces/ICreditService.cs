using FinancieraServer.DataContracts;

namespace FinancieraServer.Interfaces
{
    [ServiceContract]
    public interface ICreditService
    {
        [OperationContract]
        Response AddCreditRequest(CreditRequestDC request);

        [OperationContract]
        ResponseWithContent<List<CreditDC>> GetActiveCredits();

        [OperationContract]
        ResponseWithContent<List<CreditDC>> GetCreditsByBeneficiary(int beneficiaryId);

        [OperationContract]
        ResponseWithContent<List<CreditRequestSummaryDC>> GetCreditRequests();

        [OperationContract]
        Response DetermineRequest(int requestId, bool granted);
    }
}
