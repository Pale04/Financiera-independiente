using CreditServiceReference;
using DomainClasses;

namespace Business_logic
{
    public class CreditDocumentManager
    {
        public List<Document> GetCreditDocuments(int creditId)
        {
            CreditServiceClient service = new();
            var documents = service.GetCreditsDocuments(creditId);

            List<Document> result = [];

            foreach (CreditDocumentDC document in documents.Data) 
            {
                result.Add(new()
                {
                    Id = document.Id,
                    Name = document.Name,
                    RegistryDate = DateTime.Parse(document.RegistryDate),
                    Registrer = document.RegistrerId,
                    DocumentationId = document.DocumentationId,
                    CreditId = creditId,
                    File = document.File
                });
            }

            return result;
        }
    }
}
