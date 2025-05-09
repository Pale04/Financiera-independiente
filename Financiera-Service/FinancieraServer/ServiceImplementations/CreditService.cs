using Data_Access.Entities;
using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;
using System.Globalization;

namespace FinancieraServer.ServiceImplementations
{
    public class CreditService : ICreditService
    {
        public Response AddCreditRequest(CreditRequestDC request)
        {
            Credit credit = new()
            {
                state = "requested",
                duration = (byte)request.Credit.Duration,
                capital = request.Credit.Capital,
                beneficiary = request.Credit.beneficiaryId,
                registrer = request.Credit.registrerId,
                conditionId = request.Credit.ConditionId,
                registryDate = DateTime.ParseExact(request.Credit.RegistryDate, "dd/MM/yyyy")
            };

            List<Document> documents = new List<Document>();

            for (int i = 0; i < request.Documents.Length; i++)
            {
                documents.Add(new()
                {
                    name = request.Documents.ElementAt(i).Name,
                    file = request.Documents.ElementAt(i).file,
                    registryDate = DateTime.ParseExact(request.Documents.ElementAt(i).RegistryDate, "yyyy-MM-dd HH:mm:ss[.nnn]", CultureInfo.InvariantCulture),
                    registrer = request.Documents.ElementAt(i).RegistrerId,
                    documentationId = request.Documents.ElementAt(i).DocumentationId,
                    creditId = ,
                    active = true
                });
            }
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
