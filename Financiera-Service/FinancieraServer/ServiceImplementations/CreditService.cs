using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;

namespace FinancieraServer.ServiceImplementations
{
    public class CreditService : ICreditService
    {
        public Response AddCreditRequest(CreditRequestDC request)
        {
            throw new NotImplementedException();
        }

        public Response DetermineRequest(int requestId, bool granted)
        {
            throw new NotImplementedException();
        }

        public ResponseWithContent<List<CreditDC>> GetActiveCredits()
        {
            throw new NotImplementedException();
        }

        public ResponseWithContent<List<CreditRequestDC>> GetCreditRequests()
        {
            throw new NotImplementedException();
        }

        public ResponseWithContent<List<CreditDC>> GetCreditsByBeneficiary(int beneficiaryId)
        {
            throw new NotImplementedException();
        }
    }
}
