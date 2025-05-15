using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data_Access
{
    public class PaymentDB
    {
        public List<PaymentLayout> GetPaymentLayout(DateOnly firstdDate, DateOnly endDate)
        {
            List<PaymentLayout> paymentLayout = new();
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.CollectionsAgent)))
            {
                paymentLayout = context.PaymentLayouts
                    .Where(p => p.collectionDate >= firstdDate && p.collectionDate <= endDate)
                    .OrderBy(p => p.collectionDate)
                    .ToList();
            }
            return paymentLayout;
        }
    }
}
