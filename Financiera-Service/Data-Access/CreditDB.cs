using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access
{
    public class CreditDB
    {
        public List<Credit> GetPendingRequests()
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Credits
                    .Where(c => c.state == "requested")
                    .OrderBy(d => d.id)
                    .ToList();
            }
        }

        public List<Credit> GetActiveCredits()
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Credits
                    .Where(c => c.state == "active")
                    .OrderBy(d => d.id)
                    .ToList();
            }
        }
        public List<Credit> GetAllByCustomer(string customerId)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Credits.Where(c => c.beneficiary == customerId).ToList();
            }
        }

        public int Add(Credit credit)
        {
            int result = 0;

            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                context.Add(credit);
                result = context.SaveChanges();
                return context.Credits.Last().id;
            }

            return result;
        }

        public int UpdateState(int creditId, string state)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                Credit credit = context.Credits.Find(creditId);

                if (credit != null)
                {
                    credit.state = state;
                    return context.SaveChanges();
                }

                return 0;
            }
        }
    }
}
