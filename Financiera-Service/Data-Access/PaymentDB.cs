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

        public bool PaymentExists(int paymentId)
        {
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.CollectionsAgent)))
            {
                return context.Payments.Any(p => p.id == paymentId);
            }
        }

        public int UpdatePaymentState(int paymentId, string state)
        {
            int result = 0;
            using (var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.CollectionsAgent)))
            {
                var payment = context.Payments.FirstOrDefault(p => p.id == paymentId);
                if (payment != null && payment.state.Equals("pending"))
                {
                    using (var command = context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.Text;

                        var idParameter = command.CreateParameter();
                        idParameter.ParameterName = "@id";
                        idParameter.Value = paymentId;

                        var registrerParameter = command.CreateParameter();
                        registrerParameter.ParameterName = "@registrer";
                        registrerParameter.Value = payment.registrer;
                        
                        var creditParameter = command.CreateParameter();
                        creditParameter.ParameterName = "@creditId";
                        creditParameter.Value = payment.creditId;

                        if (state.Equals("collected"))
                        {
                            command.CommandText = "EXEC UpdatePaymentAsCollected @id, @registrer, @creditId";
                            command.Parameters.Add(idParameter);
                            command.Parameters.Add(registrerParameter);
                            command.Parameters.Add(creditParameter);
                        }
                        else
                        {
                            var collectionDateParameter = command.CreateParameter();
                            collectionDateParameter.ParameterName = "@collectionDate";
                            collectionDateParameter.Value = payment.collectionDate;

                            var amountParameter = command.CreateParameter();
                            amountParameter.ParameterName = "@amount";
                            amountParameter.Value = payment.amount;

                            command.CommandText = "EXEC UpdatePaymentAsNotCollected @id, @collectionDate, @amount, @registrer, @creditId";
                            command.Parameters.Add(idParameter);
                            command.Parameters.Add(collectionDateParameter);
                            command.Parameters.Add(amountParameter);
                            command.Parameters.Add(registrerParameter);
                            command.Parameters.Add(creditParameter);
                        }
                        
                        context.Database.OpenConnection();
                        result = command.ExecuteNonQuery();
                    }
                }
            }
            return result;
        }

        public int AddPayment(Payment payment)
        {
            using(var context = new independent_financialContext(ConnectionStringGenerator.GetConnectionString(ConnectionRole.CollectionsAgent)))
            {
                payment.state = "pending";
                context.Payments.Add(payment);
                if(context.SaveChanges() > 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
                
            }
        }

    }
}
