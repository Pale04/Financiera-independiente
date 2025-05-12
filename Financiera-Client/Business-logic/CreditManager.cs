using CreditServiceReference;
using DomainClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            if (CustomerHasActiveCredit(credit.Beneficiary))
            {
                return 2;
            }
            else if (CustomerHasRecentCreditRequest(credit.Beneficiary))
            {
                return 3;
            }

                CreditServiceClient creditService = new CreditServiceClient();
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
                    file = document.File,
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
            return true;
        }

        public bool CustomerHasRecentCreditRequest(string rfc)
        {
            return true;
        }
    }
}
