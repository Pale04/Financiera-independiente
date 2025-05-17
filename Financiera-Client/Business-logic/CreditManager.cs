using CreditServiceReference;
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
        public int Add(Credit credit, List<Document> documents)
        {
            try
            {
                if (CustomerHasActiveCredit(credit.Beneficiary))
                {
                    return 2;
                }
                else if (CustomerHasRecentCreditRequest(credit.Beneficiary))
                {
                    return 3;
                }
            }
            catch (CommunicationException error)
            {
                return 1;
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
                return creditService.AddCreditRequest(requestDC).StatusCode;
            }
            catch (CommunicationException error)
            {
                return 1;
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
    }
}
