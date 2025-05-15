using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using CatalogServiceReference;
using DomainClasses;

namespace Business_logic.Catalogs
{
    public class CreditPolicyManager
    {
        public List<Policy> GetPoliciesByPagination(int pageSize, int markId, bool next)
        {
            CatalogServiceClient client = new();
            ResponseWithContentOfArrayOfCreditPolicyDC1nk_PiFui response;
            List<Policy> policies = new();

            try
            {
                response = client.GetCreditPoliciesByPagination(pageSize, markId, next);
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    throw new Exception(ErrorMessages.BadRequest);
                default:
                    List<CreditPolicyDC> policiesList = response.Data.ToList();
                    foreach (var policy in policiesList)
                    {
                        policies.Add(new Policy
                        {
                            Id = policy.Id,
                            Title = policy.Title,
                            Description = policy.Description,
                            State = policy.State
                        });
                    }
                    return policies;
                
            }
        }


        public int AddPolicy(Policy newPolicy)
        {
            if (!newPolicy.isValid())
            {
                return 2;
            }

            CatalogServiceClient client = new();
            CreditPolicyDC newCreditPolicy = new()
            {
                Title = newPolicy.Title,
                Description = newPolicy.Description,
                register = newPolicy.Registrer,
                State = true
            };

            Response response = new();
            response = client.AddCreditPolicy(newCreditPolicy);
            int statusCode = response.StatusCode;


            return statusCode;
        }


        public int UpdatePolicy(Policy policyUpdated)
        {
            if (!policyUpdated.isValid())
            {
                return 2;
            }

            CatalogServiceClient client = new();
            Response response = new();

            CreditPolicyDC creditPolicyUpdated = new()
            {
                Id = policyUpdated.Id,
                Title = policyUpdated.Title,
                Description = policyUpdated.Description,
                register = policyUpdated.Registrer,
            };

            response = client.UpdateCreditPolicy(creditPolicyUpdated);

            int statusCode = response.StatusCode;

            return statusCode;
        }


        public int UpdatePolicyState(int idPolicy, bool state)
        {

            CatalogServiceClient client = new();
            Response response = new();
            response = client.UpdateCreditPolicyState(idPolicy, state);

            int statusCode = response.StatusCode;

            return statusCode;
        }
    }
}
