using DomainClasses;
using PaymentServiceReference;
using System.ServiceModel;
using ValidaCLABE;

namespace Business_logic.Payments
{
    public class PaymentManager
    {
        public List<PaymentLayout> GetPaymentLayout(DateOnly firstDate, DateOnly endDate)
        {
            PaymentServiceClient client = new PaymentServiceClient();
            ResponseWithContentOfArrayOfPaymentLayoutDC1nk_PiFui response = new();

            try
            {
                response = client.GetPaymentLayout(firstDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            }
            catch (CommunicationException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    //TODO: log the error
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    //TODO: log the error
                    throw new Exception(ErrorMessages.BadRequest);
                default:
                    List<PaymentLayoutDC> serializedList = response.Data.ToList();
                    List<PaymentLayout> paymentLayout = new();
                    foreach (PaymentLayoutDC payment in serializedList)
                    {
                        ValidacionResult result = ValidarCLABE.Validar(payment.BankAccountClabe);
                        paymentLayout.Add(new PaymentLayout
                        {
                            Folio = payment.Folio,
                            ClientName = payment.ClientName,
                            CollectionDate = DateOnly.Parse(payment.CollectionDate),
                            Amount = payment.Amount,
                            BankAccountClabe = payment.BankAccountClabe,
                            Bank = result.Banco != null ? result.Banco.Institucion : "Banco desconocido"
                        });
                    }
                    return paymentLayout;
            }
        }

        public string GeneratePaymentLayoutCsv(List<PaymentLayout> paymentLayout, DateOnly startDate, DateOnly endDate)
        {
            PaymentLayoutFile paymentLayoutFile = new(paymentLayout);
            string savingPath;
            try
            {
                savingPath = paymentLayoutFile.SaveToCsv(startDate.ToString("dd-MM-yyyy"), endDate.ToString("dd-MM-yyyy"));
            }
            catch (UnauthorizedAccessException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.PaymentLayoutMissingPermission);
            }
            catch (DirectoryNotFoundException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.PaymentLayoutMissingDirectory);
            }
            catch (IOException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.PaymentLayoutGenerationError);
            }

            return savingPath;
        }

        /// <summary>
        /// Validate and transform the payment layout csv file to a list of payments
        /// </summary>
        /// <param name="filePath">csv path</param>
        /// <returns>Payments list</returns>
        /// <exception cref="InvalidOperationException">When the file has an invalid format</exception>
        /// <exception cref="Exception">when a error ocurred with the file access</exception>"
        public List<Payment> GetPaymentsFromCsv(string filePath)
        {
            PaymentLayoutFile paymentLayoutFile = new(filePath);

            try
            {
                string errorMessage = paymentLayoutFile.Validate();
                if (string.IsNullOrEmpty(errorMessage))
                {
                    return paymentLayoutFile.GetPaymentsFromCsv();
                }
                else
                {
                    throw new InvalidOperationException(errorMessage);
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new Exception(ErrorMessages.PaymentLayoutMissingPermission);
            }
            catch (FileNotFoundException)
            {
                throw new Exception(ErrorMessages.BadRequest);
            }
            catch (DirectoryNotFoundException)
            {
                throw new Exception(ErrorMessages.BadRequest);
            }
            catch (IOException)
            {
                throw new Exception(ErrorMessages.PaymentLayoutUploadError);
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception(ErrorMessages.PaymentLayoutIncomplete);
            }
        }

        public int UpdatePaymentsState(Payment payment)
        {
            PaymentServiceClient client = new PaymentServiceClient();
            Response response = new();

            PaymentDC updatedPayment = new()
            {
                Id = payment.Id,
                PaymentState = payment.GetState().Equals(PaymentStatus.Collected) ? PaymentState.Collected : PaymentState.NotCollected,
                RegistrerId = UserSession.Instance.Employee.Id
            };

            try
            {
                response = client.UpdatePaymentState(updatedPayment);
            }
            catch (CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    throw new Exception(ErrorMessages.BadRequest);
                case 4:
                    throw new InvalidOperationException(ErrorMessages.PaymentNotFound);
                default:
                    return 0;
            }
        }

        public int AddPolicy(Payment payment)
        {
            PaymentServiceClient client = new();
            int statusCode = 1;


            System.DateOnly currentDate = payment.CollectionDate;
            
            PaymentDC paymentDC = new()
            {
                Amount = payment.Amount,
                CreditId = payment.CreditId,
                CollectionDate = currentDate.ToString("yyyy-MM-dd"),
                RegistrerId = payment.RegistrerId

            };

            try
            {
                Response response = client.AddPayment(paymentDC);
                statusCode = response.StatusCode;
            }
            catch(CommunicationException error)
            {
                throw new Exception(ErrorMessages.ServerError); 
            }
            
           
            return statusCode;
        }
    }
}
