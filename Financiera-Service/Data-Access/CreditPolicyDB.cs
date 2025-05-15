using Data_Access.Entities;
using System.ComponentModel;
using System.Data;

namespace Data_Access
{
    public class CreditPolicyDB
    {

        public List<CreditPolicy> GetByPagePrevious(int pageSize, int firstId)
        {
            List<CreditPolicy> previousCreditPolicies = new();
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                previousCreditPolicies = context.CreditPolicies
                   .OrderBy(p => p.id)
                   .Where(p => p.id < firstId)
                   .TakeLast(pageSize)
                   .ToList();
            }
            return previousCreditPolicies;
        }

        public List<CreditPolicy> GetByPageNext(int pageSize, int lastId)
        {
            List<CreditPolicy> nextCreditPolicies = new();
            using(var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                nextCreditPolicies = context.CreditPolicies
                    .OrderBy(p => p.id)
                    .Where(p => p.id > lastId)
                    .Take(pageSize)
                    .ToList();
            }
            return nextCreditPolicies;
        }

        public bool Exists(CreditPolicy creditPolicy)
        {
            using(var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.CreditPolicies.Any(p => p.title == creditPolicy.title && p.description == creditPolicy.description);
            }
            
        }

        public int AddCreditPolicy(CreditPolicy creditPolicy)
        {
            DateOnly dateExpired = makeExpireDate();

            creditPolicy.effectiveDate = dateExpired;

            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var policy = new CreditPolicy()
                {
                    title =  creditPolicy.title,
                    description = creditPolicy.description,
                    state = true,
                    registrer = creditPolicy.registrer,
                    effectiveDate = creditPolicy.effectiveDate,
                };
                context.CreditPolicies.Add(policy);
                if (context.SaveChanges() > 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
                    
            }
        }


        public int UpdateCreditPolicy(CreditPolicy creditPolicy)
        {
            using(var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var policy = context.CreditPolicies.Find(creditPolicy.id);
                if(policy != null)
                {
                    policy.title = creditPolicy.title;
                    policy.description = creditPolicy.description;
                    policy.effectiveDate = creditPolicy.effectiveDate;
                    policy.registrer = creditPolicy.registrer;
                    if (context.SaveChanges() > 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 4;
                } 
            }
        }

        public int UpdateStateCreditPolicy(bool newState, int id)
        {
            using(var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var policyUpdated = context.CreditPolicies.Find(id);
                if(policyUpdated != null)
                {
                    policyUpdated.state = newState;
                    if (context.SaveChanges() > 0)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 4;
                }
            }
        }

        public DateOnly makeExpireDate()
        {
            DateOnly expireDate = new DateOnly();
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Today);
            expireDate = fechaActual.AddYears(5);
            return expireDate;
        }


        public bool IdExists(int idPolicy)
        {
            using(var context =new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                var dbResponse =context.CreditPolicies.Find(idPolicy);
                if(dbResponse != null)
                {
                    return true;
                }
                return false;
            } 
        }
    }
}
