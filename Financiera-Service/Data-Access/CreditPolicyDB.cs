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
                   .Take(pageSize)
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
            int code = 0;
            DateOnly dateExpired = new DateOnly();
            dateExpired = makeExpireDate();

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
                code = context.SaveChanges();
            }
            return code;
        }


        public int UpdateCreditPolicy(CreditPolicy creditPolicy)
        {
            int code = 0;
            using(var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var policy = context.CreditPolicies.Find(creditPolicy.id);
                if(policy != null)
                {
                    policy.title = creditPolicy.title;
                    policy.description = creditPolicy.description;
                    policy.effectiveDate = creditPolicy.effectiveDate;
                    policy.registrer = creditPolicy.registrer;
                    code = context.SaveChanges();
                }   
            }

            return code;
        }

        public int UpdateStateCreditPolicy(bool newState, int id)
        {
            int code = 0;
            using(var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Administrator)))
            {
                var policyUpdated = context.CreditPolicies.Find(id);
                if(policyUpdated != null)
                {
                    policyUpdated.state = newState;
                    code = context.SaveChanges();
                }
            }

            return code;
        }

        public DateOnly makeExpireDate()
        {
            DateOnly expireDate = new DateOnly();
            DateOnly fechaActual = DateOnly.FromDateTime(DateTime.Today);
            expireDate = fechaActual.AddYears(5);
            return expireDate;
        }
    }
}
