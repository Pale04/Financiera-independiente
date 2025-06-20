﻿using Data_Access;
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

        Response IPaymentService.AddPayment(PaymentDC payment)
        {
            if (payment == null)
            {
                return new Response(2);
            }

            try
            {
                PaymentDB paymentDB = new();

                int response = paymentDB.AddPayment(new Payment()
                {
                    amount = payment.Amount,
                    creditId = payment.CreditId,
                    collectionDate = ConvertDate(payment.CollectionDate),
                    registrer = payment.RegistrerId
                });
                if(response == 0)
                {
                    return new Response(0);
                }
                else
                {
                    return new Response(1);
                }
                
            }
            catch (DbException error)
            {
                _logger.LogError($"Error trying to add the Payment {error.Message}");
                return new Response(1, "Error trying to add the payment");
            }
        }

        public DateOnly ConvertDate(string paymentDate)
        {
            DateOnly dateconverted = DateOnly.Parse(paymentDate);
            return dateconverted;
        }

        public ResponseWithContent<List<PaymentDC>> GetPaymentsFromDateRange(string startDate, string endDate)
        {
            try
            {
                PaymentDB paymentDB = new();
                var paymentList = paymentDB.GetFromDateRange(DateOnly.Parse(startDate), DateOnly.Parse(endDate));

                List<PaymentDC> payments = [];

                foreach (Payment payment in paymentList)
                {
                    PaymentState state;
                    switch (payment.state)
                    {
                        case "Collected":
                            state = PaymentState.Collected;
                            break;
                        case "NotCollected":
                            state = PaymentState.NotCollected;
                            break;
                        default:
                            _logger.LogError($"Payment n.{payment.id} has an invalid state: {payment.state}");
                            return new(1, "Error trying to get payments");
                    }

                    payments.Add(new()
                    {
                        Id = payment.id,
                        CollectionDate = payment.collectionDate.ToString(),
                        Amount = payment.amount,
                        PaymentState = state,
                        RegistrerId = payment.registrer,
                        CreditId = payment.creditId
                    });
                }

                return new(0, payments);
            }
            catch (DbException error)
            {
                _logger.LogError($"An error with code {error.HResult} occurred trying to get payments within a date range: {error.Message}");
                return new(1, "Error trying to get payments");
            }
        }
    }
}
