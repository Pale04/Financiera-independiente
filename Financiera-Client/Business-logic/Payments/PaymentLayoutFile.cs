using DomainClasses;
using FormatValidator;
using System.Text;
using Validator = FormatValidator.Validator;

namespace Business_logic.Payments
{
    class PaymentLayoutFile
    {
        private List<PaymentLayout> _paymentLayout;
        private string[] _fileHeaders;
        private string _filePath;
        private Validator _validator;

        public PaymentLayoutFile(List<PaymentLayout> paymentLayout)
        {
            _paymentLayout = paymentLayout;
            _fileHeaders = ["Folio", "Nombre del cliente", "Fecha de cobro", "Importe", "Banco", "CLABE"];
        }

        public PaymentLayoutFile(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Save the payment layout to a CSV file in the user's Downloads folder.
        /// </summary>
        /// <param name="startDate">Collection date of the first payment in dd-MM-yyyy format</param>
        /// <param name="endDate">Collection date of the last payment in dd-MM-yyyy format</param>
        /// <exception cref="UnauthorizedAccessException"></exception>"
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>"
        /// <returns>Path to the CSV file</returns>
        public string SaveToCsv(string startDate, string endDate)
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloadsPath, string.Concat("LayoutCobro_", startDate, "_", endDate, ".csv"));

            using StreamWriter writer = new StreamWriter(filePath);

            writer.WriteLine(string.Join(",", _fileHeaders));
            foreach (PaymentLayout payment in _paymentLayout)
            {
                writer.Write($"{payment.Folio},");
                writer.Write($"{payment.ClientName},");
                writer.Write($"{payment.CollectionDate},");
                writer.Write($"{payment.Amount},");
                writer.Write($"{payment.Bank},");
                writer.WriteLine($"{payment.BankAccountClabe}");
            }

            return filePath;
        }

        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>"
        public string Validate()
        {
            List<RowValidationError> validationErrors = GetFormatErrors();
            StringBuilder errorMessage = new(string.Empty);

            if (validationErrors.Count > 0)
            {
                errorMessage.Append("Las siguiente filas no contienen el formato correcto: ");
                for (int i = 0; i < validationErrors.Count; i++)
                {
                    errorMessage.Append(validationErrors[i].Row);
                    if (i < validationErrors.Count - 1)
                    {
                        errorMessage.Append(", ");
                    }
                }
            }

            return errorMessage.ToString();
        }

        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>"
        private List<RowValidationError> GetFormatErrors()
        {
            ConfigureValidations();
            FileSourceReader reader = new(_filePath);
            return new List<RowValidationError>(_validator.Validate(reader));
        }

        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>"
        private void ConfigureValidations()
        {
            string configuration = File.ReadAllText("PaymentLayoutConfiguration.json");
            _validator = Validator.FromJson(configuration);
        }

        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>"
        /// <exception cref="IndexOutOfRangeException"></exception>
        public List<Payment> GetPaymentsFromCsv()
        {
            List<Payment> payments = new();
            using StreamReader reader = new(_filePath);
            if (reader.ReadLine() != null)
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] columns = line.Split(',');
                    Payment payment = new()
                    {
                        Id = int.Parse(columns[0]),
                        State = columns[6].ToLower().Equals("si") ? PaymentStatus.Collected : PaymentStatus.NotCollected,
                    };
                    payments.Add(payment);
                }
            }
            return payments;
        }
    }
}
