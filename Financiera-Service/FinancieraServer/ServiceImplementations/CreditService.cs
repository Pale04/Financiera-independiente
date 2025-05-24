using Azure.Core;
using Data_Access;
using Data_Access.Entities;
using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;
using System.Collections.Immutable;
using System.Data.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinancieraServer.ServiceImplementations
{
    public class CreditService : ICreditService
    {
        private ILogger<CreditService> _logger;

        public CreditService(ILogger<CreditService> logger)
        {
            _logger = logger;
        }

        public ResponseWithContent<CreditDC> GetCredit(int creditId)
        {
            CreditDB creditDB = new();

            try
            {
                CreditDC creditDC = new();
                var credit = creditDB.GetCredit(creditId);

                if (credit == null)
                {
                    _logger.LogInformation($"Couldn´t find credit with id {creditId}");
                }
                else
                {
                    creditDC = new()
                    {
                        Id = creditId,
                        State = credit.state,
                        Duration = credit.duration,
                        Capital = credit.capital,
                        RegistrerId = credit.registrer,
                        BeneficiaryId = credit.beneficiary,
                        ConditionId = credit.conditionId,
                        RegistryDate = credit.registryDate.ToString()
                    };
                }

                return new(0, creditDC);
            }
            catch (DbException error)
            {
                _logger.LogError($"An error with code {error.ErrorCode} occurred while saving credit at {DateTime.Now}: ", error.Message);
                return new(1, "An error ocurred while saving the credit info");
            }
        }

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

        public ResponseWithContent<List<CreditDC>> GetCreditsByBeneficiary(string beneficiaryId)
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

        public Response UpdateCreditDocuments(int creditId, List<CreditDocumentDC> documents)
        {
            DocumentDB documentDB = new();

            try
            {
                var documentsDb = documentDB.GetByCreditId(creditId);

                foreach (Document document in documentsDb)
                {
                    foreach (CreditDocumentDC documentDC in documents)
                    {
                        if (document.name != documentDC.Name)
                        {
                            Document newDocument = new()
                            {
                                id = documentDC.Id,
                                name = documentDC.Name,
                                active = true,
                                registryDate = DateTime.Parse(documentDC.RegistryDate),
                                registrer = documentDC.RegistrerId,
                                documentationId = documentDC.DocumentationId,
                                creditId = creditId,
                            };

                            if (documentDB.ReplaceDocument(newDocument, document.id) < 1)
                            {
                                return new(1, "Error replacing document");
                            }

                            DocumentManager manager = new();
                            if (string.IsNullOrEmpty(manager.SaveDocument(newDocument, documentDC.File)))
                            {
                                return new(1, "Error saving document on server");
                            }
                        }
                    }
                }
            }
            catch (DbException error)
            {
                _logger.LogError($"An error with code {error.HResult} occurred while updating documents at {DateTime.Now}: ", error.Message);
                return new(1, "An error ocurred while saving the credit info");
            }

            return new(0);
        }

        public ResponseWithContent<List<CreditDocumentDC>> GetCreditsDocuments(int creditId)
        {
            DocumentDB documentDB = new();

            try
            {
                var documents = documentDB.GetByCreditId(creditId);

                List<CreditDocumentDC> documentsDC = [];

                foreach (var document in documents)
                {
                    DocumentManager manager = new();
                    byte[]? file = manager.GetDocument($"creditDocuments/{document.name}");

                    if (file == null)
                    {
                        _logger.LogError($"An error occurred while retrieving documents at {DateTime.Now}: File not found \"{document.name}\"");
                        return new(1, "An error occurred while retrieving documents");
                    }

                    if (document.active)
                    {
                        documentsDC.Add(new()
                        {
                            Id = document.id,
                            Name = document.name,
                            RegistryDate = document.registryDate.ToString(),
                            RegistrerId = document.registrer,
                            DocumentationId = document.documentationId,
                            File = file
                        });
                    }
                }

                return new(0, documentsDC);
            }
            catch (DbException error)
            {
                _logger.LogError($"An error with code {error.Message} occurred while retrieving documents at {DateTime.Now}: ", error.Message);
                return new(1, "An error ocurred while retrieving documents");
            }
        }

        public ResponseWithContent<CreditPaymentDC> GetPaymentInfo(int creditId)
        {
            CreditDB creditDB = new();
            try
            {
                var creditPayment = creditDB.GetCreditPaymentInfo(creditId);
                if (creditPayment != null)
                {
                    CreditPaymentDC creditPaymentDC = new()
                    {
                        id = creditPayment.id,
                        state = creditPayment.state,
                        duration = creditPayment.duration,
                        capital = creditPayment.capital,
                        beneficiary = creditPayment.beneficiary,
                        registryDate = creditPayment.registryDate,
                        registrer = creditPayment.registrer,
                        conditionId = creditPayment.conditionId,
                        interestRate = creditPayment.interestRate,
                        IVA = creditPayment.IVA,
                        paymentsPerMonth = creditPayment.paymentsPerMonth

                    };
                    return new ResponseWithContent<CreditPaymentDC>(0, creditPaymentDC);
                }
                else
                {
                    return new ResponseWithContent<CreditPaymentDC>(4, "Cannot find the credit information");
                }
            }
            catch (DbException error)
            {
                _logger.LogError($"An error with code {error.Message} trying to get the condition and capital information ");
                return new ResponseWithContent<CreditPaymentDC>(1, "An error ocurred while getting the creditpayment info");
            }
        }
    }
}

