using Data_Access;
using FinancieraServer.DataContracts;
using FinancieraServer.Interfaces;
using System.Data.Common;
using Data_Access.Entities;

namespace FinancieraServer.ServiceImplementations
{
    public class PaymentService : IPaymentService
    {
        private ILogger<PaymentService> _logger;

        public PaymentService(ILogger<PaymentService> logger)
        {
            _logger = logger;
        }

        public ResponseWithContent<List<PaymentLayoutDC>> GetPaymentLayout(string firstDate, string endDate)
        {
            if (string.IsNullOrEmpty(firstDate) || string.IsNullOrEmpty(endDate))
            {
                _logger.LogInformation("Invalid parameters for get payment layout: {firstDate}, {endDate}", firstDate, endDate);
                return new ResponseWithContent<List<PaymentLayoutDC>>(2, "Empty or null parameters");
            }

            DateOnly convertedFirstDate;
            DateOnly convertedEndDate;
            try
            {
                convertedFirstDate = DateOnly.Parse(firstDate);
                convertedEndDate = DateOnly.Parse(endDate);
            }
            catch (FormatException error)
            {
                _logger.LogInformation("Error while parsing: {firstDate}, {endDate} to DateOnly object in GetPaymentLayout: {error}",firstDate,endDate,error);
                return new ResponseWithContent<List<PaymentLayoutDC>>(2, "Invalid date format");
            }

            PaymentDB paymentDB = new();
            List<PaymentLayout> paymentLayout = new();
            try
            {
                paymentLayout = paymentDB.GetPaymentLayout(convertedFirstDate, convertedEndDate);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while getting payment layout from DB: {error}", error);
                return new ResponseWithContent<List<PaymentLayoutDC>>(1, "Error while getting payment layout from DB");
            }

            List<PaymentLayoutDC> serializedPaymentLayout = new();
            foreach (PaymentLayout payment in paymentLayout)
            {
                serializedPaymentLayout.Add(new PaymentLayoutDC
                {
                    Folio = payment.id,
                    ClientName = payment.name,
                    CollectionDate = payment.collectionDate.ToString("yyyy-MM-dd"),
                    Amount = payment.amount,
                    BankAccountClabe = payment.clabe
                });
            }

            return new ResponseWithContent<List<PaymentLayoutDC>>(0, serializedPaymentLayout);
        }
        
        public Response UpdatePaymentState(PaymentDC payment)
        {
            if (payment.Id < 1)
            {
                _logger.LogInformation("Attempt of update payment state with invalid id {id}", payment.Id);
                return new Response(2, "Invalid members");
            }

            PaymentDB paymentDB = new();
            string state = payment.PaymentState switch
            { 
                PaymentState.Collected => "collected",
                PaymentState.NotCollected => "not_collected"
            };

            try
            {
                if (!paymentDB.PaymentExists(payment.Id))
                {
                    _logger.LogInformation("Attempt of update payment state with non-existing id {id}", payment.Id);
                    return new Response(4, "Payment does not exist");
                }
                paymentDB.UpdatePaymentState(payment.Id, state);
            }
            catch (DbException error)
            {
                _logger.LogWarning("Error while attempting to update payment state {error}", error);
                return new Response(1, "Error while attempting to update payment state");
            }

            return new Response(0);
        }
    }
}
