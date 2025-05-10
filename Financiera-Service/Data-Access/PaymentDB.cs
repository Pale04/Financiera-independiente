using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_Access
{
    public class PaymentDB
    {
        public List<Payment> GetPaymentLayout(DateOnly fisrtdDate, DateOnly endDate)
        {
            List<Payment> payments = new();
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.CollectionsAgent)))
            {
                payments = context.Payments
                    .OrderBy(p => p.collectionDate)
                    .Where(p => p.collectionDate >= fisrtdDate && p.collectionDate <= endDate && p.state == "pending")
                    .Include(p => p.credit.beneficiary)
                    .ToList();
            }
            return payments;
        }
    }
}
