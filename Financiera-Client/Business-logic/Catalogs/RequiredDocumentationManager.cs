using CatalogServiceReferenece;
using DomainClasses;
using System.ServiceModel;

namespace Business_logic.Catalogs
{
    public class RequiredDocumentationManager
    {
        public List<RequiredDocument> GetByPagination(int pageSize, int markId, bool next)
        {
            CatalogServiceClient client = new();
            ResponseWithContentOfArrayOfRequiredDocumentDC1nk_PiFui response;

            try
            {
                response = next ? client.GetRequiredDocumentationByPaginationNext(pageSize, markId) :
                    client.GetRequiredDocumentationByPaginationPrevious(pageSize, markId);
            } 
            catch (CommunicationException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.ServerError);
            }

            if (response.StatusCode == 1)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.ServerError);
            }
            else if (response.StatusCode == 2)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.BadRequest);
            }

            List<RequiredDocumentDC> serializedDocumentsList = response.Data.ToList();
            List<RequiredDocument> requiredDocuments = new();
            foreach (RequiredDocumentDC document in serializedDocumentsList)
            {
                requiredDocuments.Add(new RequiredDocument
                {
                    Id = document.Id,
                    Name = document.Name,
                    FileType = (FileType)Enum.Parse(typeof(FileType), document.FileType.ToString()),
                    Status = document.State
                });
            }

            return requiredDocuments;
        }

        public int AddRequiredDocument(RequiredDocument document)
        {
            if(!document.IsValidForCreation())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }

            CatalogServiceClient client = new();
            Response response;

            try
            {
                response = client.AddRequiredDocument(new RequiredDocumentDC
                {
                    Name = document.Name,
                    FileType = document.FileType.ToString(),
                });
            }
            catch (CommunicationException error)
            {
                //TODO: Log the error
                throw new Exception(ErrorMessages.ServerError);
            }

            switch (response.StatusCode)
            {
                case 1:
                    //TODO: Log the error
                    throw new Exception(ErrorMessages.ServerError);
                case 2:
                    //TODO: Log the error
                    throw new Exception(ErrorMessages.BadRequest);
                case 3:
                    throw new Exception(ErrorMessages.DuplicatedRequiredDocument);
                default:
                    return 0;
            }
        }

        public int UpdateRequiredDocument(RequiredDocument document)
        {
            if(!document.IsValidForUpdate())
            {
                throw new Exception(ErrorMessages.InvalidFields);
            }

            CatalogServiceClient client = new();
            Response response;

            try
            {
                response = client.UpdateRequiredDocument(new RequiredDocumentDC
                {
                    Id = document.Id,
                    Name = document.Name,
                    FileType = document.FileType.ToString(),
                });
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
                case 3:
                    throw new Exception(ErrorMessages.DuplicatedRequiredDocument);
                default:
                    return 0;
            }
        }

        public int UpdateRequireDocumentStatus(RequiredDocument requiredDocument)
        {
            CatalogServiceClient client = new();
            Response response;

            try
            {
                response = client.UpdateRequiredDocumentState(requiredDocument.Id, requiredDocument.Status);
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
                    return 0;
            }
        }
    }
}
