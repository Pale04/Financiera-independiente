using DomainClasses;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;

using System.IO;


namespace Business_logic
{
    public static class PdfGenerator
    {
        public static void GenerateCreditRequestDocument(Credit credit, CreditCondition condition, Customer customer, List<PersonalReference> references, string path)
        {
            if (credit == null)
            {
                throw new ArgumentNullException();
            }

            iText.Layout.Document document = new iText.Layout.Document(new PdfDocument(new PdfWriter(path + $"\\Solicitud-{credit.Id}.pdf")));

            document.Add(new Paragraph("Solicitud de crédito").SetFontSize(18).SetHorizontalAlignment(HorizontalAlignment.CENTER));
            document.Add(new Paragraph("Financiera independiente").SetFontSize(16).SetHorizontalAlignment(HorizontalAlignment.CENTER));

            Table personalInfoTable = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            Cell titleCell = new Cell(1, 2).Add(new Paragraph("Datos personales"));
            titleCell.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            personalInfoTable.AddCell(titleCell);

            personalInfoTable.AddCell("Nombre");
            personalInfoTable.AddCell(customer.Name);
            personalInfoTable.AddCell("Fecha de nacimiento");
            personalInfoTable.AddCell(customer.BirthDate.ToString());
            personalInfoTable.AddCell("RFC");
            personalInfoTable.AddCell(customer.Rfc);
            personalInfoTable.AddCell("Número de telefono 1");
            personalInfoTable.AddCell(customer.PhoneNumber1);
            personalInfoTable.AddCell("Número de telefono 2");
            personalInfoTable.AddCell(customer.PhoneNumber2);
            personalInfoTable.AddCell("Correo electrónico");
            personalInfoTable.AddCell(customer.Email);
            personalInfoTable.AddCell("Domicilio");
            personalInfoTable.AddCell(customer.HouseAddress);

            document.Add(personalInfoTable);

            Table referencesTable = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            Cell referencesCell = new Cell(1, 2).Add(new Paragraph("Contactos de referencia"));
            referencesCell.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            referencesTable.AddCell(referencesCell);

            for (int i = 0; i < references.Count; i++)
            {
                PersonalReference reference = references[i];
                referencesTable.AddCell(new Cell(1, 2).Add(new Paragraph($"Referencia {i}")));
                referencesTable.AddCell("Nombre");
                referencesTable.AddCell(references[i].Name);
                referencesTable.AddCell("Parentesco");
                referencesTable.AddCell(references[i].Relationship);
                referencesTable.AddCell("Número de telefono");
                referencesTable.AddCell(references[i].PhoneNumber);
            }

            document.Add(referencesTable);

            Table creditTable = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            Cell creditCell = new Cell(1, 2).Add(new Paragraph("Información del crédito"));
            creditCell.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            creditTable.AddCell(creditCell);

            creditTable.AddCell("Capital");
            creditTable.AddCell(credit.Capital.ToString());
            creditTable.AddCell("Condicion");
            creditTable.AddCell(condition.ToString());
            creditTable.AddCell("Duración");
            creditTable.AddCell(credit.Duration.ToString());
            creditTable.AddCell("folio");
            creditTable.AddCell(condition.Id.ToString());

            document.Add(creditTable);

            double total = (double)credit.Capital + (((double)credit.Capital * ((double)condition.InterestRate / 100)) * (1.0 + ((double)condition.IVA / 100)));
            string summary = $"Capital: ${credit.Capital}\nPagos mensuales: {condition.PaymentsPerMonth} de ${total / (credit.Duration * condition.PaymentsPerMonth)}\nDuración en meses: {credit.Duration}\nInteres: {condition.InterestRate}%\n\nTotal a pagar: ${total}";
            document.Add(new Paragraph(summary).SetFontSize(18).SetHorizontalAlignment(HorizontalAlignment.CENTER));

            document.Close();
        }

        public static void GeneratePaymentLayoutDocument(List<Payment> paymentsList, int creditId)
        {
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            iText.Layout.Document document = new iText.Layout.Document(new PdfDocument(new PdfWriter(path + $"\\Pagos-{creditId}.pdf")));

            document.Add(new Paragraph("Tabla de pagos").SetFontSize(18).SetHorizontalAlignment(HorizontalAlignment.CENTER));
            document.Add(new Paragraph("Financiera independiente").SetFontSize(16).SetHorizontalAlignment(HorizontalAlignment.CENTER));

            Table paymentsTable = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();

            paymentsTable.AddCell("Folio");
            paymentsTable.AddCell("Monto");
            paymentsTable.AddCell("Fecha");

            foreach (Payment payment in paymentsList)
            {
                paymentsTable.AddCell(payment.Id.ToString());
                paymentsTable.AddCell(payment.Amount.ToString());
                paymentsTable.AddCell(payment.CollectionDate.ToString());
            }

            document.Add(paymentsTable);
            document.Close();
        }
    }
}
