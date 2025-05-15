using Data_Access;
using Data_Access.Entities;
using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;
using System.Data.Common;

namespace FinancieraServer.ServiceImplementations
{
    public partial class CatalogService
    {
        
        public ResponseWithContent<List<CreditPolicyDC>> GetCreditPoliciesByPagination(int pageSize, int markId, bool next)
        {
            if(pageSize <= 0 || markId < 0)
            {
                _logger.LogInformation($"Attempt to get policies with the page size of {pageSize} and Id {markId}");
                return new ResponseWithContent<List<CreditPolicyDC>>(2, "Invalid ID or Page Size");
            }

            CreditPolicyDB creditPolicyDB = new();
            List<CreditPolicy> databaseCreditPolicies = new();

            try
            {
                databaseCreditPolicies = next ? creditPolicyDB.GetByPageNext(pageSize, markId) : creditPolicyDB.GetByPagePrevious(pageSize, markId);
            }
            catch (DbException error)
            {
                _logger.LogError($"An error ocurred trying to get the credit policies: {error}");
                return new ResponseWithContent<List<CreditPolicyDC>>(1, "Error while trying to get the credit policies");
            }

            List<CreditPolicyDC> creditPolicies = new();
            foreach (var creditPolicy in databaseCreditPolicies)
            {
                creditPolicies.Add(new CreditPolicyDC
                {
                    Id = creditPolicy.id,
                    Title = creditPolicy.title,
                    Description = creditPolicy.description,
                    State = creditPolicy.state,
                    register = creditPolicy.registrer,
                    EffectiveDate = creditPolicy.effectiveDate
                });
            }

            return new ResponseWithContent<List<CreditPolicyDC>>(0, creditPolicies);
        }

        public Response AddCreditPolicy(CreditPolicyDC newPolicy)
        {
            CreditPolicyDB creditPolicyDB = new();
            CreditPolicy policy = new()
            {
                title = newPolicy.Title,
                description = newPolicy.Description,
                registrer = newPolicy.register,
                state = true,
                effectiveDate = newPolicy.EffectiveDate
            };

            try
            {
                if (creditPolicyDB.Exists(policy))
                {
                    return new Response(3, "Error: policy already exists");
                }

                int code = creditPolicyDB.AddCreditPolicy(policy);
                if (code != 0)
                {
                    return new Response(1, "Error adding the new policy to the database");
                }

                _logger.LogInformation($"Added new Policy: {policy.title}");


            }
            catch (DbException error)
            {
                _logger.LogError($"An error ocurred trying to register the credit policy: {error}");
                return new Response(1, "An database error ocurred trying to add the new policy");
            }
            return new Response(0);
        }

        public Response UpdateCreditPolicy(CreditPolicyDC policyUpdated)
        {
            CreditPolicyDB policyDB = new();
            CreditPolicy policyToUpdate = new()
            {
                id = policyUpdated.Id,
                title = policyUpdated.Title,
                description = policyUpdated.Description,
                registrer = policyUpdated.register,
                effectiveDate = policyUpdated.EffectiveDate
            };

            try
            {
                if (!policyDB.IdExists(policyToUpdate.id))
                {
                    return new Response(4, "Cannot found the policy");
                }
                else
                {
                    int code = policyDB.UpdateCreditPolicy(policyToUpdate);

                    if (code != 0)
                    {
                        return new Response(1, "An error trying to upload the policy");
                    }
                    else
                    {
                        _logger.LogInformation($"The policy {policyToUpdate.title} was updated successfully");
                        
                    }
                }
                return new Response(0);

            }
            catch (DbException error)
            {
                _logger.LogError($"An error ocurred while updating the policy data: {error}");
                return new Response(1, "An error ocurred trying to update the policy data");
            }
        }

        public Response UpdateCreditPolicyState(int id, bool statePolicyUpdated)
        {
            CreditPolicyDB policyDB = new();
            CreditPolicy creditPolicy = new()
            {
                id = id
            };
            try
            {
                if (!policyDB.IdExists(id))
                {
                    return new Response(4, "Cannot found the policy");
                }

                int code = policyDB.UpdateStateCreditPolicy(statePolicyUpdated, id);
                if(code != 0)
                {
                    return new Response(1, "An error trying to upload the policy state");
                }
                else
                {
                    _logger.LogInformation("Policy state uploaded successfully");
                    return new Response(0);
                }
            }
            catch (DbException error)
            {
                _logger.LogError($"An error ocurred while updating the policy state: {error}");
                return new Response(1, "An error ocurred trying to update the policy  state");
            }


        }
    }
}