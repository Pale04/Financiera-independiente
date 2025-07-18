﻿using CreditServiceReference;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Business_logic
{
    public class CreditManager
    {
        public (int, int) Add(Credit credit, List<Document> documents)
        {
            try
            {
                if (CustomerHasActiveCredit(credit.Beneficiary))
                {
                    return (0, 2);
                }
                else if (CustomerHasRecentCreditRequest(credit.Beneficiary))
                {
                    return (0, 3);
                }
            }
            catch (CommunicationException error)
            {
                return (0, 1);
            }

            Response response;

            CreditDC creditDC = new()
            {
                State = "requested",
                Duration = credit.Duration,
                Capital = credit.Capital,
                RegistrerId = UserSession.Instance.Employee.Id,
                BeneficiaryId = credit.Beneficiary,
                ConditionId = credit.ConditionId,
            };

            List<CreditDocumentDC> documentsDC = new List<CreditDocumentDC>();

            foreach (Document document in documents)
            {
                documentsDC.Add(new()
                {
                    Name = document.Name,
                    File = document.File,
                    RegistrerId = UserSession.Instance.Employee.Id,
                    RegistryDate = DateTime.Now.ToString(),
                    DocumentationId = document.DocumentationId,
                });
            }

            CreditRequestDC requestDC = new()
            {
                Credit = creditDC,
                Documents = documentsDC.ToArray()
            };

            CreditServiceClient creditService = new CreditServiceClient();

            try
            {
                var wpfResponse = creditService.AddCreditRequest(requestDC);
                return (wpfResponse.Data, wpfResponse.StatusCode);
            }
            catch (CommunicationException error)
            {
                return (0, 1);
            }
        }

        public bool CustomerHasActiveCredit(string rfc)
        {
            CreditServiceClient creditService = new CreditServiceClient();

            var credits = creditService.GetCreditsByBeneficiary(rfc);

            foreach (var credit in credits.Data)
            {
                if (credit.State == "active")
                {
                    return true;
                }
            }

            return false;
        }

        public int DeterminateResquest(Credit credit, bool isApproved)
        {
            if (credit == null)
            {
                return 2;
            }
            else
            {
                try
                {
                    CreditServiceClient client = new();
                    Response response = client.DetermineRequest(credit.Id, isApproved);
                    return response.StatusCode;
                }
                catch (CommunicationException error)
                {
                    return 1;
                }
            }
        }

        public bool CustomerHasRecentCreditRequest(string rfc)
        {
            CreditServiceClient creditService = new CreditServiceClient();

            var credits = creditService.GetCreditsByBeneficiary(rfc);

            foreach (var credit in credits.Data)
            {
                if (DateTime.Compare(DateTime.Parse(credit.RegistryDate), DateTime.Now.AddMonths(3)) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        public List<CreditRequestSummary> GetCreditRequests()
        {
            CreditServiceClient creditService = new CreditServiceClient();

            var creditsDb = creditService.GetCreditRequests();

            List<CreditRequestSummary> credits = [];

            foreach (var credit in creditsDb.Data)
            {
                credits.Add(new()
                {
                    Id = credit.Id,
                    Duration = credit.Duration,
                    Capital = credit.Capital,
                    ClientName = credit.ClientName,
                    InterestRate = credit.InterestRate
                });
            }

            return credits;
        }

        public Credit GetCredit(int creditId)
        {
            CreditServiceClient creditService = new CreditServiceClient();

            var response = creditService.GetCredit(creditId);

            CreditDC creditDb = response.Data;

            return new()
            {
                Id = creditId,
                State = creditDb.State,
                Duration = (byte)creditDb.Duration,
                Capital = creditDb.Capital,
                Beneficiary = creditDb.BeneficiaryId,
                ConditionId = creditDb.ConditionId,
            };
        }

        public CreditCondition GetCreditCondition(int idCredit)
        {
            CreditServiceClient creditService = new CreditServiceClient();
            try
            {
                var conditionInfo = creditService.GetPaymentInfo(idCredit);
                if (conditionInfo.StatusCode == 0)
                {
                    CreditCondition creditConditionInfo = new()
                    {
                        InterestRate = conditionInfo.Data.interestRate,
                        IVA = conditionInfo.Data.IVA,
                        PaymentsPerMonth = conditionInfo.Data.paymentsPerMonth
                    };
                    return creditConditionInfo;
                }
                else
                {
                    throw new Exception(ErrorMessages.BadRequest);
                }
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }
        }
    }
}
