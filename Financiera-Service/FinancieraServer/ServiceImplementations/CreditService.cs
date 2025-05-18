using Data_Access;
using Data_Access.Entities;
using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;
using System.Data.Common;

namespace FinancieraServer.ServiceImplementations
{
    public class CreditService : ICreditService
    {
        private ILogger<AccountService> _logger;

        public Response AddCreditRequest(CreditRequestDC request)
        {
            Credit credit = new()
            {
                state = "requested",
                duration = (byte)request.Credit.Duration,
                capital = request.Credit.Capital,
                beneficiary = request.Credit.BeneficiaryId,
                registrer = request.Credit.RegistrerId,
                conditionId = request.Credit.ConditionId,
                registryDate = DateTime.Now
            };

            CreditDB creditDB = new();

            try
            {
                credit.id = creditDB.Add(credit);

                if (credit.id < 1)
                {
                    _logger.LogWarning("Couldn´t add a credit, but no error was raised");
                    return new Response(1, "An error ocurred while saving data");
                }
            }
            catch (DbException error)
            {
                _logger.LogError($"An error with code {error.ErrorCode} occurred while saving credit at {DateTime.Now}: ", error.Message);
                return new Response(1, "An error ocurred while saving the credit info");
            }

            List<Document> documents = new List<Document>();

            for (int i = 0; i < request.Documents.Length; i++)
            {
                Document document = new()
                {
                    name = request.Documents.ElementAt(i).Name,
                    registryDate = DateTime.Parse(request.Documents.ElementAt(i).RegistryDate),
                    registrer = request.Documents.ElementAt(i).RegistrerId,
                    documentationId = request.Documents.ElementAt(i).DocumentationId,
                    creditId = credit.id,
                    active = true
                };

                DocumentManager manager = new();
                try
                {
                    document.name = manager.SaveDocument(document, request.Documents.ElementAt(i).File);
                }
                catch (Exception error)
                {
                    _logger.LogError($"Error saving file at {DateTime.Now}: ", error.Message);
                    return new Response(1, "Error saving documents");
                }

                documents.Add(document);
            }

            DocumentDB documentDB = new();
            int result;

            foreach (Document document in documents)
            {
                try
                {
                    result = documentDB.Add(document);

                    if (result < 1)
                    {
                        _logger.LogWarning("Couldn´t add a credit, but no error was raised");
                        return new Response(1, "An error ocurred while saving documents");
                    }
                }
                catch (DbException error)
                {
                    _logger.LogError($"An error with code {error.ErrorCode} occurred while saving documents at {DateTime.Now}: ", error.Message);
                    return new Response(1, "An error ocurred while saving documents");
                }
            }

            return new Response(0, "Credit and documents added successfully");
        }

        public Response DetermineRequest(int requestId, bool granted)
        {
            try
            {
                Credit credit = new()
                {
                    id = requestId
                };
                CreditDB creditDB = new();
                credit = creditDB.ExistsById(credit.id);
                int response;

                if (credit != null)
                {
                    if (!granted)
                    {
                        response = creditDB.UpdateState(credit.id, "rejected");
                        return new Response(response);
                    }
                    else
                    {
                        response = creditDB.UpdateState(credit.id, "active");
                        return new Response(response);
                    }
                }
                else
                {
                    return new Response(4, "Cannot find the credit specified");
                }
            }
            catch (DbException error)
            {
                _logger.LogError($"An error happen trying to approve or reject the credit application {error.Message}");
                return new Response(1, "An error ocurred while approve / reject the application");
            }
        }

        public ResponseWithContent<List<CreditDC>> GetActiveCredits()
        {
            throw new NotImplementedException();
        }

        public ResponseWithContent<List<CreditRequestSummaryDC>> GetCreditRequests()
        {
            CreditDB creditDB = new();

            try
            {
                var creditsDb = creditDB.GetPendingRequests();
                List<CreditRequestSummaryDC> credits = [];

                foreach (CreditRequest credit in creditsDb)
                {
                    credits.Add(new()
                    {
                        Id = credit.id,
                        Duration = credit.duration,
                        Capital = credit.capital,
                        ClientName = credit.name,
                        InterestRate = credit.interestRate
                    });
                }

                return new(0, credits);
            }
            catch (DbException error)
            {
                _logger.LogError($"An error with code {error.ErrorCode} occurred while retrieving credits at {DateTime.Now}: ", error.Message);
                return new(1, "An error ocurred while retrieving credits");
            }
        }

        public ResponseWithContent<List<CreditDC>> GetCreditsByBeneficiary(int beneficiaryId)
        {
            CreditDB creditDB = new();

            try
            {
                var creditsDb = creditDB.GetAllByCustomer(beneficiaryId);
                List<CreditDC> credits = [];

                foreach (Credit credit in creditsDb)
                {
                    credits.Add(new()
                    {
                        Id = credit.id,
                        State = credit.state,
                        Duration = credit.duration,
                        Capital = credit.capital,
                        RegistrerId = credit.registrer,
                        BeneficiaryId = credit.beneficiary,
                        ConditionId = credit.conditionId,
                        RegistryDate = credit.registryDate.ToString()
                    });
                }

                return new(0, credits);
            }
            catch (DbException error)
            {
                _logger.LogError($"An error with code {error.ErrorCode} occurred while saving credit at {DateTime.Now}: ", error.Message);
                return new(1, "An error ocurred while saving the credit info");
            }
        }
    }
}
