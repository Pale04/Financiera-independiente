using CreditServiceReference;
using DomainClasses;
using SessionServiceReference;

namespace Business_logic
{
    public class CreditDocumentManager
    {
        public List<DomainClasses.Document> GetCreditDocuments(int creditId)
        {
            CreditServiceClient service = new();
            var documents = service.GetCreditsDocuments(creditId);

            List<DomainClasses.Document> result = [];

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

        public int ReplaceDocuments(List<DomainClasses.Document> documents)
        {
            CreditServiceClient service = new();

            List<CreditDocumentDC> documentsDC = new();

            foreach (DomainClasses.Document document in documents) 
            {
                documentsDC.Add(new()
                {
                    Id = document.Id,
                    Name = document.Name,
                    RegistryDate = document.RegistryDate.ToString(),
                    DocumentationId = document.DocumentationId,
                    RegistrerId = document.Registrer,
                    File = document.File
                });
            }

            return service.UpdateCreditDocuments(documents[0].CreditId, documentsDC.ToArray()).StatusCode;
        }
    }
}
