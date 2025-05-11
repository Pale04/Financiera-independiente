using DomainClasses;
using PaymentServiceReference;
using System.ServiceModel;
using ValidaCLABE;

namespace Business_logic
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
                            Bank = result.Banco.Institucion
                        });
                    }
                    return paymentLayout;   
            }
        }

        public string GeneratePaymentLayoutCsv(List<PaymentLayout> paymentLayout, DateOnly startDate, DateOnly endDate)
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloadsPath, $"LayoutCobro_{startDate:dd-MM-yyyy}_{endDate:dd-MM-yyyy}.csv");

            try
            {
                using StreamWriter writer = new StreamWriter(filePath);
                writer.WriteLine("Folio,Nombre del cliente,Fecha de cobro,Importe,Banco,CLABE");
                foreach (PaymentLayout payment in paymentLayout)
                {
                    writer.Write($"{payment.Folio},");
                    writer.Write($"{payment.ClientName},");
                    writer.Write($"{payment.CollectionDate},");
                    writer.Write($"{payment.Amount},");
                    writer.Write($"{payment.Bank},");
                    writer.WriteLine($"{payment.BankAccountClabe}");
                }
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

            return filePath;
        }
    }
}
