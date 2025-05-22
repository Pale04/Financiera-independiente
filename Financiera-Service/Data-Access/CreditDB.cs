using Data_Access.Entities;

namespace Data_Access
{
    public class CreditDB
    {
        public Credit? GetCredit(int id)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                return context.Credits.Find(id);
            }
        }

        public List<CreditRequest> GetPendingRequests()
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                return context.CreditRequests
                    .Take(100)
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

            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.LoanOfficer)))
            {
                context.Add(credit);
                result = context.SaveChanges();
                return context.Credits.OrderBy(d => d.id).Last().id;
            }

            return result;
        }

        public int UpdateState(int creditId, string state)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Analyst)))
            {
                Credit credit = context.Credits.Find(creditId);

                if (credit != null)
                {
                    credit.state = state;
                    if (context.SaveChanges() > 0)
                    {
                        return 0;
                    }      
                }
                return 1;
            }
        }

        public Credit ExistsById(int id)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Reader)))
            {
                return context.Credits.Find(id);
            }
        }

        public CreditPayment GetCreditPaymentInfo(int id)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.Analyst)))
            {
                return context.CreditPayments.FirstOrDefault(c => c.id == id);
            }
        }
    }
}
